namespace Syncfusion.Maui.Toolkit.Internals
{
	/// <summary>
	/// Represents a touch detection mechanism for views.
	/// </summary>
	public partial class TouchDetector : IDisposable
	{
		#region Fields

		readonly List<ITouchListener> _touchListeners;
		bool _disposed;

		#endregion

		#region Properties

		internal View MauiView { get; private set; }

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="TouchDetector"/> class.
		/// </summary>
		/// <param name="mauiView">The MAUI view to detect touches on.</param>
		public TouchDetector(View mauiView)
		{
			MauiView = mauiView;
			_touchListeners = new List<ITouchListener>();
			if (mauiView.Handler != null)
			{
				SubscribeNativeTouchEvents(mauiView);
			}
			else
			{
				mauiView.HandlerChanged += MauiView_HandlerChanged;
				mauiView.HandlerChanging += MauiView_HandlerChanging;
			}
		}

		#endregion

		#region Private Methods

		void MauiView_HandlerChanged(object? sender, EventArgs e)
		{
			if (sender is View view && view.Handler != null)
				SubscribeNativeTouchEvents(view);
		}

		void MauiView_HandlerChanging(object? sender, HandlerChangingEventArgs e)
		{
			UnsubscribeNativeTouchEvents(e.OldHandler);
		}

		/// <summary>
		/// Unsubscribe the events.
		/// </summary>
		void Unsubscribe(View? mauiView)
		{
			if (mauiView != null)
			{
				if (mauiView.Handler != null)
				{
					UnsubscribeNativeTouchEvents(mauiView.Handler);
				}
				mauiView.HandlerChanged -= MauiView_HandlerChanged;
				mauiView.HandlerChanging -= MauiView_HandlerChanging;
				mauiView = null;
			}
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Adds a touch listener to the collection.
		/// </summary>
		/// <param name="listener">The touch listener to add.</param>
		public void AddListener(ITouchListener listener)
		{
			if (!_touchListeners.Contains(listener))
				_touchListeners.Add(listener);
		}

		/// <summary>
		/// Removes a touch listener from the collection if it exists.
		/// </summary>
		/// <param name="listener">The touch listener to remove.</param>
		public void RemoveListener(ITouchListener listener)
		{
			if (_touchListeners.Contains(listener))
				_touchListeners.Remove(listener);
		}

		/// <summary>
		/// Checks if there are any touch listeners in the collection.
		/// </summary>
		/// <returns>True if there are listeners, false otherwise.</returns>
		public bool HasListener()
		{
			return _touchListeners.Count > 0;
		}

		/// <summary>
		/// Removes all touch listeners from the collection.
		/// </summary>
		public void ClearListeners()
		{
			_touchListeners.Clear();
		}

		/// <summary>
		/// Releases all resources used by the <see cref="TouchDetector"/>.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
		}

		/// <summary>
		/// Releases the unmanaged resources and optionally releases the managed resources.
		/// </summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
		protected virtual void Dispose(bool disposing)
		{
			if (_disposed)
				return;

			_disposed = true;

			if (disposing)
			{
				ClearListeners();
				Unsubscribe(MauiView);
			}
		}

		#endregion

		#region Internal Methods

		internal void OnTouchAction(Func<IElement?, Point?> getPosition, long pointerId, PointerActions action, Point point)
		{
			PointerEventArgs eventArgs = new PointerEventArgs(getPosition, pointerId, action, point);
			OnTouchAction(eventArgs);
		}

		internal void OnTouchAction(Func<IElement?, Point?> getPosition, long pointerId, PointerActions action, PointerDeviceType deviceType, Point point)
		{
			PointerEventArgs eventArgs = new PointerEventArgs(getPosition, pointerId, action, deviceType, point);
			OnTouchAction(eventArgs);
		}

		internal void OnTouchAction(PointerEventArgs eventArgs)
		{
			foreach (var listener in _touchListeners)
			{
				listener.OnTouch(eventArgs);
			}
		}

		internal bool OnScrollAction(long pointerId, Point origin, double direction, bool? handled = null)
		{
			ScrollEventArgs eventArgs = new ScrollEventArgs(pointerId, origin, direction);

			if (handled != null)
				eventArgs.Handled = handled.Value;

			foreach (var listener in _touchListeners)
			{
				listener.OnScrollWheel(eventArgs);
			}
			return eventArgs.Handled;
		}

		#endregion
	}
}