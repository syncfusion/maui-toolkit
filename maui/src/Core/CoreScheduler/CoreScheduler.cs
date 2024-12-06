using Microsoft.Maui.Animations;

namespace Syncfusion.Maui.Toolkit.Internals
{
	/// <summary>
	/// Specifies the types of core schedulers available.
	/// </summary>
	public enum CoreSchedulerType
	{
		/// <summary>
		/// Scheduler for frame-based operations.
		/// </summary>
		Frame,

		/// <summary>
		/// Scheduler for main thread operations.
		/// </summary>
		Main,

		/// <summary>
		/// Scheduler for composite operations.
		/// </summary>
		Composite
	}

	/// <summary>
	/// Represents an abstract base class for core schedulers, providing scheduling functionality for various operations.
	/// </summary>
	public abstract class CoreScheduler
	{
		private Action? _callback;

		/// <summary>
		/// Gets a value indicating whether the scheduler is currently scheduled.
		/// </summary>
		public bool IsScheduled { get; private set; }

		/// <summary>
		/// Creates a core scheduler of the specified type.
		/// </summary>
		/// <param name="type">The type of core scheduler to create. Defaults to <see cref="CoreSchedulerType.Frame"/>.</param>
		/// <returns>A new instance of a core scheduler.</returns>
		public static CoreScheduler CreateScheduler(CoreSchedulerType type = CoreSchedulerType.Frame)
		{
			return type switch
			{
				CoreSchedulerType.Frame => new CoreFrameScheduler(),
				CoreSchedulerType.Main => new CoreMainScheduler(),
				_ => new CoreCompositeScheduler(),
			};
		}

		/// <summary>
		/// Schedules the specified action to be executed.
		/// </summary>
		/// <param name="action">The action to be scheduled.</param>
		/// <returns><c>true</c> if the action was successfully scheduled; otherwise, <c>false</c>.</returns>
		public bool ScheduleCallback(Action action)
		{
			if (action == null)
			{
				throw new ArgumentNullException(nameof(action));
			}

			if (!IsScheduled)
			{
				IsScheduled = true;
				_callback = action;
				return OnSchedule(InvokeCallback);
			}

			return false;
		}

		private void InvokeCallback()
		{
			if (_callback != null)
			{
				_callback();
				_callback = null;
				IsScheduled = false;
			}
		}

		/// <summary>
		/// Schedules the specified action to be executed. This method must be implemented by derived classes.
		/// </summary>
		/// <param name="action">The action to be scheduled.</param>
		/// <returns><c>true</c> if the action was successfully scheduled; otherwise, <c>false</c>.</returns>
		protected abstract bool OnSchedule(Action action);
	}

	/// <summary>
	/// Represents a scheduler that operates on a frame-based schedule, inheriting from <see cref="CoreScheduler"/>.
	/// </summary>
	public class CoreFrameScheduler : CoreScheduler
	{
		private Action? _callback;
		private readonly Microsoft.Maui.Animations.Animation _animation;
		IAnimationManager? _animationManager;

		/// <summary>
		/// Initializes a new instance of the <see cref="CoreFrameScheduler"/> class.
		/// </summary>
		public CoreFrameScheduler()
		{
			_animation = new Microsoft.Maui.Animations.Animation(OnFrameStart, start: 0.001f, duration: 0);
		}

		/// <summary>
		/// Schedules the specified action to be executed. This method must be implemented by derived classes.
		/// </summary>
		/// <param name="action">The action to be scheduled.</param>
		/// <returns><c>true</c> if the action was successfully scheduled; otherwise, <c>false</c>.</returns>
		protected override bool OnSchedule(Action action)
		{
			if (Application.Current != null)
			{
				var handler = Application.Current.Handler;
				if (handler != null && handler.MauiContext != null)
				{
					_animationManager = handler.MauiContext.Services.GetRequiredService<IAnimationManager>();
				}

				if (_animationManager != null)
				{
					_callback = action;
					_animation.Reset();
					_animation.Commit(_animationManager);
					return true;
				}
			}
			return false;
		}

		private void OnFrameStart(double value)
		{
			if (_callback != null)
			{
				_callback();
				_callback = null;
			}
		}
	}

	/// <summary>
	/// Represents a scheduler that operates on the main thread, inheriting from <see cref="CoreScheduler"/>.
	/// </summary>
	public class CoreMainScheduler : CoreScheduler
	{
		/// <summary>
		/// Schedules the specified action to be executed. This method must be implemented by derived classes.
		/// </summary>
		/// <param name="action">The action to be scheduled.</param>
		/// <returns><c>true</c> if the action was successfully scheduled; otherwise, <c>false</c>.</returns>
		[Obsolete]
#pragma warning disable CS0809 // Obsolete member overrides non-obsolete member
		protected override bool OnSchedule(Action action)
#pragma warning restore CS0809 // Obsolete member overrides non-obsolete member
		{
			Device.BeginInvokeOnMainThread(action);
			return true;
		}
	}

	/// <summary>
	/// Represents a composite scheduler that combines multiple scheduling strategies, inheriting from <see cref="CoreScheduler"/>.
	/// </summary>
	internal class CoreCompositeScheduler : CoreScheduler
	{
		readonly CoreFrameScheduler _frameScheduler = new();
		readonly CoreMainScheduler _mainScheduler = new();
		Action? _callback;

		/// <summary>
		/// Schedules the specified action to be executed. This method must be implemented by derived classes.
		/// </summary>
		/// <param name="action">The action to be scheduled.</param>
		/// <returns><c>true</c> if the action was successfully scheduled; otherwise, <c>false</c>.</returns>
		protected override bool OnSchedule(Action action)
		{
			_callback = action;
			_frameScheduler.ScheduleCallback(InvokeCallback);
			_mainScheduler.ScheduleCallback(InvokeCallback);
			return true;
		}

		private void InvokeCallback()
		{
			if (IsScheduled)
			{
				_callback?.Invoke();
			}
		}
	}
}
