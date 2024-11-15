namespace Syncfusion.Maui.Toolkit.Internals
{
    /// <summary>
    /// Enables MAUI view to recognize gestures.
    /// </summary>
    public partial class GestureDetector : IDisposable
    {
        #region Fields

        List<IRightTapGestureListener>? _rightTapGestureListeners;
        List<ITapGestureListener>? _tapGestureListeners;
        List<IDoubleTapGestureListener>? _doubleTapGestureListeners;
        List<IPinchGestureListener>? _pinchGestureListeners;
        List<IPanGestureListener>? _panGestureListeners;
        List<ILongPressGestureListener>? _longPressGestureListeners;
        bool _disposed;
        bool _isViewListenerAdded;

        #endregion

        #region Properties

        internal View MauiView { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="GestureDetector"/> class.
        /// </summary>
        /// <param name="mauiView">The MAUI view to attach the gesture detector to.</param>
        public GestureDetector(View mauiView)
        {
            this.MauiView = mauiView;

            if (mauiView.Handler != null)
            {
                SubscribeNativeGestureEvents(mauiView);
            }
            else
            {
                mauiView.HandlerChanged += MauiView_HandlerChanged;
                mauiView.HandlerChanging += MauiView_HandlerChanging;
            }
        }

        #endregion

        #region Private Methods

        void AddPanGestureListener(IPanGestureListener panGesture)
        {
            if (_panGestureListeners == null)
                _panGestureListeners = new List<IPanGestureListener>();
            _panGestureListeners.Add(panGesture);
        }

        void AddPinchGestureListener(IPinchGestureListener pinchGesture)
        {
            if (_pinchGestureListeners == null)
                _pinchGestureListeners = new List<IPinchGestureListener>();
            _pinchGestureListeners.Add(pinchGesture);
        }

        void AddLongPressGestureListener(ILongPressGestureListener longPressGesture)
        {
            if (_longPressGestureListeners == null)
                _longPressGestureListeners = new List<ILongPressGestureListener>();
            _longPressGestureListeners.Add(longPressGesture);
        }

        void AddTapGestureListener(ITapGestureListener tapGesture)
        {
            if (_tapGestureListeners == null)
                _tapGestureListeners = new List<ITapGestureListener>();
            _tapGestureListeners.Add(tapGesture);
        }

        void AddRightTapGestureListener(IRightTapGestureListener rightTapGesture)
        {
            if (_rightTapGestureListeners == null)
                _rightTapGestureListeners = new List<IRightTapGestureListener>();
            _rightTapGestureListeners.Add(rightTapGesture);
        }

        void AddDoubleTapGestureListener(IDoubleTapGestureListener doubleTapGesture)
        {
            if (_doubleTapGestureListeners == null)
                _doubleTapGestureListeners = new List<IDoubleTapGestureListener>();
            _doubleTapGestureListeners.Add(doubleTapGesture);
        }

        /// <summary>
        /// Handles the event when the MAUI view's handler has changed.
        /// </summary>
        void MauiView_HandlerChanged(object? sender, EventArgs e)
        {
            if (sender is View view && view.Handler != null)
                SubscribeNativeGestureEvents(view);
        }

        /// <summary>
        /// Handles the event when the MAUI view's handler is changing.
        /// </summary>
        void MauiView_HandlerChanging(object? sender, HandlerChangingEventArgs e)
        {
            UnsubscribeNativeGestureEvents(e.OldHandler);
        }

        void HandleSingleTap(Point touchPoint, int tapCount)
        {
            if (tapCount == 1 && _tapGestureListeners != null)
            {
                TapEventArgs eventArgs = new TapEventArgs(touchPoint, tapCount);
                foreach (var listener in _tapGestureListeners)
                {
                    listener.OnTap(MauiView, eventArgs);
                    listener.OnTap(eventArgs);
                }
            }

        }

        void HandleDoubleTap(Point touchPoint, int tapCount)
        {
            if (tapCount == 2 && _doubleTapGestureListeners != null)
            {
                TapEventArgs eventArgs = new TapEventArgs(touchPoint, tapCount);
                foreach (var listener in _doubleTapGestureListeners)
                {
                    listener.OnDoubleTap(eventArgs);
                }
            }
        }

        /// <summary>
        /// Unsubscribe the events 
        /// </summary>
        void Unsubscribe(View? mauiView)
        {
            if (mauiView != null)
            {
                if (mauiView.Handler != null)
                {
                    UnsubscribeNativeGestureEvents(mauiView.Handler);
                }
                mauiView.HandlerChanged -= MauiView_HandlerChanged;
                mauiView.HandlerChanging -= MauiView_HandlerChanging;
                mauiView = null;
            }
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Adds a gesture listener to the detector.
        /// </summary>
        /// <param name="listener">The gesture listener to add.</param>
        public void AddListener(IGestureListener listener)
        {
            if (listener is IPanGestureListener panGesture)
            {
                AddPanGestureListener(panGesture);
            }

            if (listener is IPinchGestureListener pinchGesture)
            {
                AddPinchGestureListener(pinchGesture);
            }

            if (listener is ILongPressGestureListener longPressGesture)
            {
                AddLongPressGestureListener(longPressGesture);
            }

            if (listener is ITapGestureListener tapGesture)
            {
                AddTapGestureListener(tapGesture);
            }

            if (listener is IRightTapGestureListener rightTapGesture)
            {
                AddRightTapGestureListener(rightTapGesture);
            }

            if (listener is IDoubleTapGestureListener doubleTapGesture)
            {
                AddDoubleTapGestureListener(doubleTapGesture);
            }

            // Create native listeners if this method is called dynamically or after the MauiView's handler has been set.
            if (!_isViewListenerAdded)
            {
                CreateNativeListener();
            }
            _isViewListenerAdded = true;
        }

        /// <summary>
        /// Removes a gesture listener from the detector.
        /// </summary>
        /// <param name="listener">The gesture listener to remove.</param>
        public void RemoveListener(IGestureListener listener)
        {
            if (listener is IPanGestureListener panGesture &&
                _panGestureListeners != null &&
                _panGestureListeners.Contains(panGesture))
                _panGestureListeners.Remove(panGesture);

            if (listener is IPinchGestureListener pinchGesture &&
                _pinchGestureListeners != null &&
                _pinchGestureListeners.Contains(pinchGesture))
                _pinchGestureListeners.Remove(pinchGesture);

            if (listener is ILongPressGestureListener longPressGesture &&
                _longPressGestureListeners != null &&
                _longPressGestureListeners.Contains(longPressGesture))
                _longPressGestureListeners.Remove(longPressGesture);

            if (listener is ITapGestureListener tapGesture &&
                _tapGestureListeners != null &&
                _tapGestureListeners.Contains(tapGesture))
                _tapGestureListeners.Remove(tapGesture);

            if (listener is IRightTapGestureListener rightTapGesture &&
                _rightTapGestureListeners != null &&
                _rightTapGestureListeners.Contains(rightTapGesture))
                _rightTapGestureListeners.Remove(rightTapGesture);

            if (listener is IDoubleTapGestureListener doubleTapGesture &&
                _doubleTapGestureListeners != null &&
                _doubleTapGestureListeners.Contains(doubleTapGesture))
                _doubleTapGestureListeners.Remove(doubleTapGesture);
        }

        /// <summary>
        /// Clears all registered gesture listeners from the <see cref="GestureDetector"/>.
        /// </summary>
        public void ClearListeners()
        {
            _rightTapGestureListeners?.Clear();
            _tapGestureListeners?.Clear();
            _doubleTapGestureListeners?.Clear();
            _pinchGestureListeners?.Clear();
            _panGestureListeners?.Clear();
            _longPressGestureListeners?.Clear();
        }

        /// <summary>
        /// Checks if there are any registered gesture listeners.
        /// </summary>
        /// <returns>True if there is at least one listener registered; otherwise, false.</returns>
        public bool HasListener()
        {
            return _tapGestureListeners?.Count > 0 ||
                _doubleTapGestureListeners?.Count > 0 ||
                _longPressGestureListeners?.Count > 0 ||
                _pinchGestureListeners?.Count > 0 ||
                _panGestureListeners?.Count > 0 ||
                _rightTapGestureListeners?.Count > 0;
        }

        /// <summary>
        /// Disposes the GestureDetector, releasing all managed and unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the GestureDetector and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">True to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            _disposed = true;

            if (disposing)
            {
                _isViewListenerAdded = false;
                ClearListeners();
                this.Unsubscribe(MauiView);
            }
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Handles pinch gestures and notifies all registered pinch gesture listeners.
        /// </summary>
        internal virtual void OnPinch(Func<IElement?, Point?>? position, GestureStatus state, Point point, double pinchAngle, float scale)
        {
            if (_pinchGestureListeners != null)
            {
                PinchEventArgs eventArgs = new PinchEventArgs(position, state, point, pinchAngle, scale);
                foreach (var listener in _pinchGestureListeners)
                {
                    listener.OnPinch(eventArgs);
                }
            }
        }

        /// <summary>
        /// Handles pan gestures and notifies all registered pan gesture listeners.
        /// </summary>
        internal virtual void OnScroll(Func<IElement?, Point?> getPosition, GestureStatus state, Point startPoint, Point scalePoint, Point velocity)
        {
            if (_panGestureListeners != null)
            {
                PanEventArgs eventArgs = new PanEventArgs(getPosition, state, startPoint, scalePoint, velocity);
                foreach (var listener in _panGestureListeners)
                {
                    listener.OnPan(eventArgs);
                }
            }
        }

        /// <summary>
        /// Handles tap gestures and notifies the appropriate listeners.
        /// </summary>
        internal virtual void OnTapped(Point touchPoint, int tapCount)
        {
            HandleSingleTap(touchPoint, tapCount);
            HandleDoubleTap(touchPoint, tapCount);
        }

        /// <summary>
        /// Handles right tap gestures and notifies all registered right tap gesture listeners.
        /// </summary>
        internal virtual void OnRightTapped(Point touchPoint, PointerDeviceType pointerDeviceType)
        {
            RightTapEventArgs eventArgs;
            if (_rightTapGestureListeners != null)
            {
                eventArgs = new RightTapEventArgs(touchPoint, pointerDeviceType);
                foreach (var listener in _rightTapGestureListeners)
                {
                    listener.OnRightTap(MauiView, eventArgs);
                    listener.OnRightTap(eventArgs);
                }
            }
        }

        /// <summary>
        /// Handles long press gestures and notifies all registered long press gesture listeners.
        /// </summary>
#if IOS || MACCATALYST
        internal virtual void OnLongPress(Func<IElement?, Point?>? position, Point touchPoint, GestureStatus status)
        {
            if (_longPressGestureListeners != null)
            {
                LongPressEventArgs eventArgs = new LongPressEventArgs(position, touchPoint, status);
                foreach (var listener in _longPressGestureListeners)
                {
                    listener.OnLongPress(eventArgs);
                }
            }
        }
#else
        internal virtual void OnLongPress(Func<IElement?, Point?>? position, Point touchPoint)
        {
            if (_longPressGestureListeners != null)
            {
                LongPressEventArgs eventArgs = new LongPressEventArgs(position, touchPoint);
                foreach (var listener in _longPressGestureListeners)
                {
                    listener.OnLongPress(eventArgs);
                }
            }
        }
#endif
        #endregion
    }
}