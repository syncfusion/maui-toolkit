using Android.Content;
using Android.Views;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

namespace Syncfusion.Maui.Toolkit.Calendar
{
    /// <summary>
    /// Custom handler for custom scroll layout.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812: Avoid uninstantiated internal classes", Justification = "We require this")]
    internal class SnapLayoutHandler : LayoutHandler
    {
        /// <summary>
        /// Create a native view for snap layout.
        /// </summary>
        /// <returns>Native snap layout.</returns>
        protected override LayoutViewGroup CreatePlatformView()
        {
            if (VirtualView == null)
            {
                throw new InvalidOperationException($"{nameof(VirtualView)} must be set to create a LayoutViewGroup");
            }

            NativeSnapLayout viewGroup = new NativeSnapLayout(Context!);
            SnapLayout? scrollLayout = VirtualView as SnapLayout;
            if (scrollLayout != null)
            {
                //// Set the native intercept event property by Maui intercept method because in native we did not decide the whether the child scroll view reaches it end and try to scroll after the end so that we call the Maui method for intercept event.
                viewGroup.InterceptTouchEvent = scrollLayout.OnInterceptTouchEvent;
                viewGroup.DisAllowInterceptTouchEvent = scrollLayout.OnDisAllowInterceptTouchEvent;
                viewGroup.TouchEvent = scrollLayout.OnHandleTouch;
            }

            //// .NET MAUI layouts should not impose clipping on their children.
            viewGroup.SetClipChildren(false);
            return viewGroup;
        }
    }

    /// <summary>
    /// Custom native snap layout that handles the intercept event.
    /// </summary>
    internal class NativeSnapLayout : LayoutViewGroup
    {
        // The gestureDetector holds the instance of the fling gesture listner.
        GestureDetector? _gestureDetector;

        // The flingGestureListener holds the instance of the fling gesture listner.
        FlingGestureListener? _flingGestureListener;

        /// <summary>
        /// Initializes a new instance of the <see cref="NativeSnapLayout"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public NativeSnapLayout(Context context)
            : base(context)
        {
            _flingGestureListener = new FlingGestureListener();
            _gestureDetector = new GestureDetector(Context, _flingGestureListener);
            //// Pan or tap gestures on Maui does not triggered while we override intercept event and return value based on movement. In swiping, initial down action return false and move action return true value. So the pan or tap gestures not triggered. Using the touch to call Maui touch event to resolve the issue.                  
            Touch += Handle_Touch;
        }

        /// <summary>
        /// Gets or sets to decide whether hold/pass the touch to children.
        /// </summary>
        internal Func<Point, string, bool>? InterceptTouchEvent { get; set; }

        /// <summary>
        /// Gets or sets to decide whether hold/pass the touch to parent.
        /// </summary>
        internal Func<Point, string, bool?>? DisAllowInterceptTouchEvent { get; set; }

        /// <summary>
        /// Gets or sets to call the Maui touch event.
        /// </summary>
        internal Action<Point, string, Point>? TouchEvent { get; set; }

        /// <summary>
        ///  Return false then pass the touch to its children.
        ///  Return true then it holds the touch to its own.
        /// </summary>
        /// <param name="motionEvent">Motion event details.</param>
        /// <returns> Return true to steal motion events from the children and have them dispatched to this ViewGroup through onTouchEvent(). The current target will receive an ACTION_CANCEL event, and no further messages will be delivered here.</returns>
        public override bool OnInterceptTouchEvent(MotionEvent? motionEvent)
        {
            if (InterceptTouchEvent == null || motionEvent == null)
            {
                return false;
            }

            int actionIndex = motionEvent.ActionIndex;
            //// The touch point value based on density.
            Point screenPoint = new Point(motionEvent.GetX(actionIndex), motionEvent.GetY(actionIndex));
            Func<double, double> fromPixels = Android.App.Application.Context.FromPixels;
            //// Calculate the touch point for Maui (without density value).
            Point point = new Point(fromPixels(screenPoint.X), fromPixels(screenPoint.Y));
            switch (motionEvent.Action)
            {
                case MotionEventActions.Down:
                    {
                        bool isIntercept = InterceptTouchEvent(point, "Down");
                        if (DisAllowInterceptTouchEvent != null)
                        {
                            bool? isDisAllowIntercept = DisAllowInterceptTouchEvent(point, "Down");
                            if (isDisAllowIntercept != null)
                            {
                                Parent?.RequestDisallowInterceptTouchEvent(isDisAllowIntercept.Value);
                            }
                        }

                        return isIntercept;
                    }

                case MotionEventActions.Move:
                    {
                        bool isIntercept = InterceptTouchEvent(point, "Move");
                        if (DisAllowInterceptTouchEvent != null)
                        {
                            bool? isDisAllowIntercept = DisAllowInterceptTouchEvent(point, "Move");
                            if (isDisAllowIntercept != null)
                            {
                                Parent?.RequestDisallowInterceptTouchEvent(isDisAllowIntercept.Value);
                            }
                        }

                        return isIntercept;
                    }

                case MotionEventActions.Up:
                    {
                        return InterceptTouchEvent(point, "Up");
                    }

                case MotionEventActions.Cancel:
                    {
                        return InterceptTouchEvent(point, "Cancel");
                    }
            }

            return false;
        }

        /// <summary>
        /// Dispose the object instances.
        /// </summary>
        /// <param name="disposing">The dispose.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Touch -= Handle_Touch;
                if (_gestureDetector != null)
                {
                    _gestureDetector.Dispose();
                    _gestureDetector = null;
                }

                if (_flingGestureListener != null)
                {
                    _flingGestureListener?.Dispose();
                    _flingGestureListener = null;
                }
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Pass the touch and its position to the layout used for
        /// swiping layout swiping.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="touchEvent">The touch event.</param>
        void Handle_Touch(object? sender, TouchEventArgs touchEvent)
        {
            if (TouchEvent == null || touchEvent.Event == null)
            {
                return;
            }

            _gestureDetector?.OnTouchEvent(touchEvent.Event);
            int actionIndex = touchEvent.Event.ActionIndex;
            //// The touch point value based on density.
            Point screenPoint = new Point(touchEvent.Event.GetX(actionIndex), touchEvent.Event.GetY(actionIndex));
            Func<double, double> fromPixels = Android.App.Application.Context.FromPixels;
            //// Calculate the touch point for Maui (without density value).
            Point point = new Point(fromPixels(screenPoint.X), fromPixels(screenPoint.Y));
            switch (touchEvent.Event.Action)
            {
                case MotionEventActions.Down:
                case MotionEventActions.PointerDown:
                    {
                        _flingGestureListener!.Velocity = Point.Zero;
                        TouchEvent(point, "Down", _flingGestureListener.Velocity);
                        break;
                    }

                case MotionEventActions.Move:
                    {
                        TouchEvent(point, "Move", _flingGestureListener!.Velocity);
                        break;
                    }

                case MotionEventActions.Up:
                case MotionEventActions.PointerUp:
                    {
                        TouchEvent(point, "Up", _flingGestureListener!.Velocity);
                        _flingGestureListener.Velocity = Point.Zero;
                        break;
                    }

                case MotionEventActions.Cancel:
                    {
                        TouchEvent(point, "Cancel", _flingGestureListener!.Velocity);
                        _flingGestureListener.Velocity = Point.Zero;
                        break;
                    }
            }
        }

        /// <summary>
        /// Fling gesture listener that handles the on fling.
        /// </summary>
        internal class FlingGestureListener : GestureDetector.SimpleOnGestureListener
        {
            // The velocity variable value will be update while on fling method triggered and on touch event triggered.
            internal Point Velocity;

            /// <summary>
            /// Initializes a new instance of the <see cref="FlingGestureListener"/> class.
            /// </summary>
            internal FlingGestureListener()
            {
            }

            /// <inheritdoc/>
            public override bool OnFling(MotionEvent? e1, MotionEvent? e2, float velocityX, float velocityY)
            {
                Func<double, double> fromPixels = Android.App.Application.Context.FromPixels;
                Velocity = new Point(fromPixels(velocityX), fromPixels(velocityY));
                return true;
            }
        }
    }
}