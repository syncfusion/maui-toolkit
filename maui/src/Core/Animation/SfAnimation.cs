using Microsoft.Maui.Animations;

namespace Syncfusion.Maui.Toolkit
{
    /// <summary>
    /// <see cref="SfAnimation"/> is used to create a animatable view over a certain time period 
    /// with customized options. 
    /// </summary>
    internal class SfAnimation : Microsoft.Maui.Animations.Animation
    {
		#region Fields

		/// <summary>
		/// A read-only instance representing the local view component associated with this context.
		/// </summary>
		readonly IView localView;

		/// <summary>
		/// Indicates whether the operation is currently forwarding.
		/// </summary>
		bool isForwarding = false;

		/// <summary>
		/// Indicates whether the operation is currently forwarding.
		/// </summary>
		bool isReversing = false;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="SfAnimation"/> class.
		/// </summary>
		/// <param name="view">The view associated with the animation.</param>
		/// <param name="step">The action to execute on each animation step.</param>
		/// <param name="finished">The action to execute when the animation is finished.</param>
		/// <param name="start">The starting value of the animation.</param>
		/// <param name="end">The ending value of the animation.</param>
		/// <param name="easing">The easing function to apply to the animation.</param>
		/// <param name="duration">The duration of the animation.</param>
		internal SfAnimation(IView view, Action<double>? step = null, Action? finished = null, double start = 0.0,
            double end = 1.0, Easing? easing = null, double duration = 1.0)
        {
            localView = view;
            Step = step;
            Finished = finished;
            Start = start;
            End = end;
            Easing = easing ?? Easing.Linear;
            Duration = duration;
        }

        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets or sets the animation begin value.
        /// </summary>
        /// <value>
        /// Defaults value is 0.0.
        /// </value>
        internal double Start { get; set; }

        /// <summary>
        /// Gets or sets the animation end value.
        /// </summary>
        /// <value>
        /// Defaults value is 1.0.
        /// </value>
        internal double End { get; set; }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Forwards the animation value from Start to End.
        /// </summary>
        internal void Forward()
        {
			SetAnimationManager();
            if (!isForwarding)
            {
                Pause();
                CurrentTime = Progress == 0 ? 0.0 : Progress * Duration + StartDelay;
                HasFinished = false;
                isForwarding = true;
                isReversing = false;
                Resume();
            }
        }

        /// <summary>
        /// Reverses the animation value from End to Start.
        /// </summary>
        internal void Reverse()
        {
			SetAnimationManager();
            if (!isReversing)
            {
                Pause();
                CurrentTime = Progress == 1 ? 0.0 : (1 - Progress) * Duration + StartDelay;
                HasFinished = false;
                isForwarding = false;
                isReversing = true;
                Resume();
            }
        }

		#endregion

		#region Private Methods

		/// <summary>
		/// Sets the animation manager for the current view, if applicable.
		/// </summary>
		void SetAnimationManager()
		{
			if (localView != null && localView.Handler != null && localView.Handler.MauiContext != null)
			{
				animationManger ??= GetAnimationManager(localView.Handler.MauiContext);
			}
		}

		/// <summary>
		/// Retrieves the animation manager from the specified Maui context.
		/// </summary>
		/// <param name="mauiContext">The Maui context from which to retrieve the animation manager.</param>
		/// <returns>The animation manager associated with the given Maui context.</returns>
	    IAnimationManager GetAnimationManager(IMauiContext mauiContext)
        {
            return mauiContext.Services.GetRequiredService<IAnimationManager>();
        }

		/// <summary>
		/// Calculates the current value of the animation based on the progress.
		/// </summary>
		/// <returns>The current animated value.</returns>
	    double GetAnimatingValue()
        {
            return Start + (End - Start) * Progress;
        }

		#endregion

		#region Override Methods

		/// <summary>
		/// Resets the current animation state to its initial settings.
		/// </summary>
		public override void Reset()
		{
			base.Reset();
			Progress = 0.0;
			isForwarding = false;
			isReversing = false;
		}

		/// <summary>
		/// Invoked on each tick of the animation timer, allowing the animation to update.
		/// </summary>
		/// <param name="millisecondsSinceLastUpdate">The time in milliseconds since the last update.</param>
		protected override void OnTick(double millisecondsSinceLastUpdate)
		{
			if (HasFinished)
			{
				return;
			}

			double secondsSinceLastUpdate = millisecondsSinceLastUpdate / 1000.0;
			CurrentTime += secondsSinceLastUpdate;
			if (CurrentTime < StartDelay)
			{
				return;
			}

			double start = CurrentTime - StartDelay;
			double animatingPercent = Math.Min(start / Duration, 1);
			double percent = isForwarding ? animatingPercent : 1 - animatingPercent;
			Update(percent);
			if (HasFinished)
			{
				Finished?.Invoke();
				isForwarding = false;
				isReversing = false;
				if (Repeats)
				{
					Reset();
				}
			}
		}

		/// <summary>
		/// Updates the animation state based on the specified percentage.
		/// </summary>
		/// <param name="percent">The percentage of the animation that has completed.</param>
		public override void Update(double percent)
		{
			try
			{
				Progress = Easing.Ease(percent);
				Step?.Invoke(GetAnimatingValue());
				HasFinished = isForwarding ? percent == 1 : percent == 0;
			}
			catch (Exception e)
			{
				HasFinished = true;
				Console.WriteLine(e.Message);
				throw new Exception(e.Message);
			}
		}

		#endregion
	}
}