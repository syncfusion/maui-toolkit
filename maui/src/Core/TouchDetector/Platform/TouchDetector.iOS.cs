using MauiView = Microsoft.Maui.Controls.View;
using UIKit;
using Foundation;
using CoreGraphics;
using Microsoft.Maui.Graphics;
using Microsoft.Maui;
using Microsoft.Maui.Devices;
using System;
using PlatformView = UIKit.UIView;

namespace Syncfusion.Maui.Toolkit.Internals
{
    /// <summary>
    /// Handles touch detection for iOS and macOS platforms.
    /// </summary>
    public partial class TouchDetector
    {
        #region Internal Methods

        /// <summary>
        /// Subscribes to native touch events for the given MAUI view.
        /// </summary>
        internal void SubscribeNativeTouchEvents(MauiView? mauiView)
        {
            if (mauiView != null && mauiView.Handler != null)
            {
                var handler = mauiView.Handler;
                if (handler?.PlatformView is UIView nativeView)
                {
                    UITouchRecognizerExt touchRecognizer = new UITouchRecognizerExt(this);
                    nativeView.AddGestureRecognizer(touchRecognizer);

                    if (OperatingSystem.IsIOSVersionAtLeast(13))
                    {
                        UIHoverRecognizerExt hoverGesture = new UIHoverRecognizerExt(this);
                        nativeView.AddGestureRecognizer(hoverGesture);
                    }

                    UIScrollRecognizerExt scrollRecognizer = new UIScrollRecognizerExt(this);
                    nativeView.AddGestureRecognizer(scrollRecognizer);
                }
            }
        }

        /// <summary>
        /// Unsubscribes from native touch events for the given element handler.
        /// </summary>
        internal void UnsubscribeNativeTouchEvents(IElementHandler handler)
        {
            if (handler != null)
            {
                if (handler.PlatformView is UIView nativeView)
                {
                    var gestures = nativeView.GestureRecognizers;

                    if (gestures != null)
                    {
                        foreach (var item in gestures)
                        {
                            if (item is UITouchRecognizerExt || item is UIHoverRecognizerExt || item is UIScrollRecognizerExt)
                            {
                                nativeView.RemoveGestureRecognizer(item);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Calculates the position of a touch event relative to a given element or the entire view.
        /// </summary>
        internal static Point? CalculatePosition(IElement? element, MauiView mauiView, UIGestureRecognizer platformRecognizer)
        {
            if (mauiView.Handler == null)
                return null;

            var virtualView = mauiView.Handler.VirtualView;
            var platformView = element?.ToPlatform();

            if (virtualView == null)
                return null;

            CGPoint? result = null;
            if (element == null)
                result = platformRecognizer.LocationInView(null);
            else if (platformView is PlatformView view)
                result = platformRecognizer.LocationInView(view);

            if (result == null)
                return null;

            return new Point((int)result.Value.X, (int)result.Value.Y);

        }

        #endregion
    }

    #region Hover Recognizer

    /// <summary>
    /// Extends UIHoverGestureRecognizer to handle hover events for the TouchDetector.
    /// </summary>
    internal class UIHoverRecognizerExt : UIHoverGestureRecognizer
    {
        #region Fields

        WeakReference<TouchDetector>? _touchDetector;
        WeakReference<ITouchListener>? _touchListener;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="UIHoverRecognizerExt"/> class.
        /// </summary>
        internal UIHoverRecognizerExt(TouchDetector listener) : base(Hovering)
        {
            TouchDetector = listener;
            if (TouchDetector.MauiView is ITouchListener _touchListener)
                TouchListener = _touchListener;
            ShouldRecognizeSimultaneously += GestureRecognizer;

            AddTarget(OnHover);
        }

        #endregion

        #region Properties

        TouchDetector? TouchDetector
        {
            get => _touchDetector != null && _touchDetector.TryGetTarget(out var v) ? v : null;
            set => _touchDetector = value == null ? null : new(value);
        }

        ITouchListener? TouchListener
        {
            get => _touchListener != null && _touchListener.TryGetTarget(out var v) ? v : null;
            set => _touchListener = value == null ? null : new(value);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Having static member for base action hence <see cref="UIHoverGestureRecognizer"/> does not have default consturctor.
        /// </summary>
        static void Hovering()
        {
            // The method is intentionally left empty as it serves as a base action placeholder.
        }

        /// <summary>
        /// Determines whether this gesture recognizer should recognize simultaneously with other gesture recognizers.
        /// </summary>
        bool GestureRecognizer(UIGestureRecognizer gestureRecognizer, UIGestureRecognizer otherGestureRecognizer)
        {
            if (otherGestureRecognizer is UITouchRecognizerExt || TouchListener == null)
            {
                return true;
            }

            return !TouchListener.IsTouchHandled;
        }

        /// <summary>
        /// Handles hover events and triggers the appropriate action on the TouchDetector.
        /// </summary>
        void OnHover()
        {
            if (TouchDetector == null || !TouchDetector.MauiView.IsEnabled || TouchDetector.MauiView.InputTransparent)
            {
                return;
            }

            var state = State == UIGestureRecognizerState.Began ? PointerActions.Entered :
                State == UIGestureRecognizerState.Changed ? PointerActions.Moved :
                State == UIGestureRecognizerState.Ended ? PointerActions.Exited : PointerActions.Cancelled;

            long pointerId = Handle.Handle.ToInt64();
            CGPoint point = LocationInView(View);

            TouchDetector.OnTouchAction((relativeTo) => TouchDetector.CalculatePosition(relativeTo, TouchDetector.MauiView, this), pointerId, state, new Point(point.X, point.Y));
        }

        #endregion
    }

    #endregion

    #region Touch Recognizer

    /// <summary>
    /// Extends <see cref="UIPanGestureRecognizer"/> to handle touch events for the TouchDetector.
    /// </summary>
    internal class UITouchRecognizerExt : UIPanGestureRecognizer
    {
        #region Fields

        WeakReference<TouchDetector>? _touchDetector;
        WeakReference<ITouchListener>? _touchListener;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the UITouchRecognizerExt class.
        /// </summary>
        internal UITouchRecognizerExt(TouchDetector listener)
        {
            TouchDetector = listener;
            if (TouchDetector.MauiView is ITouchListener _touchListener)
                TouchListener = _touchListener;

            ShouldRecognizeSimultaneously += GestureRecognizer;
        }

        #endregion

        #region Properties

        TouchDetector? TouchDetector
        {
            get => _touchDetector != null && _touchDetector.TryGetTarget(out var v) ? v : null;
            set => _touchDetector = value == null ? null : new(value);
        }

        ITouchListener? TouchListener
        {
            get => _touchListener != null && _touchListener.TryGetTarget(out var v) ? v : null;
            set => _touchListener = value == null ? null : new(value);
        }

        #endregion

        #region Private Methods

        bool GestureRecognizer(UIGestureRecognizer gestureRecognizer, UIGestureRecognizer otherGestureRecognizer)
        {
            if (otherGestureRecognizer is GestureDetector.UIPanGestureExt || otherGestureRecognizer is UIScrollRecognizerExt || TouchListener == null)
            {
                return true;
            }

            return !TouchListener.IsTouchHandled;
        }

        #endregion

        #region Override methods

        /// <summary>
        /// Handles the beginning of touch events.
        /// </summary>
        /// <param name="touches">The set of touches that began.</param>
        /// <param name="uiEvent">The UIEvent associated with the touches.</param>
        public override void TouchesBegan(NSSet touches, UIEvent uiEvent)
        {
            base.TouchesBegan(touches, uiEvent);

            if (TouchDetector == null || !TouchDetector.MauiView.IsEnabled || TouchDetector.MauiView.InputTransparent)
            {
                return;
            }

            if (touches.AnyObject is UITouch touch)
            {
                PointerDeviceType pointerDeviceType = touch.Type == UITouchType.Stylus ? PointerDeviceType.Stylus : PointerDeviceType.Touch;
#if MACCATALYST
                pointerDeviceType = PointerDeviceType.Mouse;
#endif
                long pointerId = touch.Handle.Handle.ToInt64();
                CGPoint point = touch.LocationInView(View);
                TouchDetector.OnTouchAction(
                   new PointerEventArgs((relativeTo) => TouchDetector.CalculatePosition(relativeTo, TouchDetector.MauiView, this), pointerId, PointerActions.Pressed, pointerDeviceType, new Point(point.X, point.Y))
                   {
                       IsLeftButtonPressed = touch.TapCount == 1
                   });
            }
        }

        /// <summary>
        /// Handles the movement of touch events.
        /// </summary>
        /// <param name="touches">The set of touches that moved.</param>
        /// <param name="uiEvent">The UIEvent associated with the touches.</param>
        public override void TouchesMoved(NSSet touches, UIEvent uiEvent)
        {
            base.TouchesMoved(touches, uiEvent);
            if (TouchDetector == null || !TouchDetector.MauiView.IsEnabled || TouchDetector.MauiView.InputTransparent)
            {
                return;
            }

            if (touches.AnyObject is UITouch touch)
            {
                PointerDeviceType pointerDeviceType = touch.Type == UITouchType.Stylus ? PointerDeviceType.Stylus : PointerDeviceType.Touch;
#if MACCATALYST
                pointerDeviceType = PointerDeviceType.Mouse;
#endif
                long pointerId = touch.Handle.Handle.ToInt64();
                CGPoint point = touch.LocationInView(View);
                TouchDetector.OnTouchAction(new PointerEventArgs((relativeTo) => TouchDetector.CalculatePosition(relativeTo, TouchDetector.MauiView, this), pointerId, PointerActions.Moved, pointerDeviceType, new Point(point.X, point.Y))
                {
                    IsLeftButtonPressed = touch.TapCount == 1
                });
            }
        }

        /// <summary>
        /// Handles the end of touch events.
        /// </summary>
        /// <param name="touches">The set of touches that ended.</param>
        /// <param name="uiEvent">The UIEvent associated with the touches.</param>
        public override void TouchesEnded(NSSet touches, UIEvent uiEvent)
        {
            base.TouchesEnded(touches, uiEvent);
            if (TouchDetector == null || !TouchDetector.MauiView.IsEnabled || TouchDetector.MauiView.InputTransparent)
            {
                return;
            }

            if (touches.AnyObject is UITouch touch)
            {
                PointerDeviceType pointerDeviceType = touch.Type == UITouchType.Stylus ? PointerDeviceType.Stylus : PointerDeviceType.Touch;
#if MACCATALYST
                pointerDeviceType = PointerDeviceType.Mouse;
#endif
                long pointerId = touch.Handle.Handle.ToInt64();
                CGPoint point = touch.LocationInView(View);
                TouchDetector.OnTouchAction((relativeTo) => TouchDetector.CalculatePosition(relativeTo, TouchDetector.MauiView, this), pointerId, PointerActions.Released, pointerDeviceType, new Point(point.X, point.Y));
            }
        }

        /// <summary>
        /// Handles the cancellation of touch events.
        /// </summary>
        /// <param name="touches">The set of touches that were cancelled.</param>
        /// <param name="uiEvent">The UIEvent associated with the touches.</param>
        public override void TouchesCancelled(NSSet touches, UIEvent uiEvent)
        {
            base.TouchesCancelled(touches, uiEvent);
            if (TouchDetector == null || !TouchDetector.MauiView.IsEnabled || TouchDetector.MauiView.InputTransparent)
            {
                return;
            }

            if (touches.AnyObject is UITouch touch)
            {
                PointerDeviceType pointerDeviceType = touch.Type == UITouchType.Stylus ? PointerDeviceType.Stylus : PointerDeviceType.Touch;
#if MACCATALYST
                pointerDeviceType = PointerDeviceType.Mouse;
#endif
                long pointerId = touch.Handle.Handle.ToInt64();
                CGPoint point = touch.LocationInView(View);
                TouchDetector.OnTouchAction((relativeTo) => TouchDetector.CalculatePosition(relativeTo, TouchDetector.MauiView, this), pointerId, PointerActions.Cancelled, pointerDeviceType, new Point(point.X, point.Y));
            }
        }

        #endregion
    }

    #endregion

    #region Scroll Recognizer

    /// <summary>
    /// Extends <see cref="UIPanGestureRecognizer"/> to handle scroll events for the TouchDetector.
    /// </summary>
    internal class UIScrollRecognizerExt : UIPanGestureRecognizer
    {
        #region Fields

        WeakReference<TouchDetector>? _touchDetector;
        WeakReference<ITouchListener>? _touchListener;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="UIScrollRecognizerExt"/> class.
        /// </summary>
        internal UIScrollRecognizerExt(TouchDetector listener)
        {
            TouchDetector = listener;
            if (TouchDetector.MauiView is ITouchListener _touchListener)
                TouchListener = _touchListener;

            AddTarget(OnScroll);

            if (UpdateAllowedScrollTypesMask())
            {
                AllowedScrollTypesMask = UIScrollTypeMask.All;
            }

            ShouldRecognizeSimultaneously += GestureRecognizer;
            ShouldReceiveTouch += GesturerTouchRecognizer;
        }

        #endregion

        #region Properties

        TouchDetector? TouchDetector
        {
            get => _touchDetector != null && _touchDetector.TryGetTarget(out var v) ? v : null;
            set => _touchDetector = value == null ? null : new(value);
        }

        ITouchListener? TouchListener
        {
            get => _touchListener != null && _touchListener.TryGetTarget(out var v) ? v : null;
            set => _touchListener = value == null ? null : new(value);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Updates the allowed scroll types mask based on the current platform and version.
        /// </summary>
        bool UpdateAllowedScrollTypesMask()
        {
            if (DeviceInfo.Platform == DevicePlatform.iOS)
            {
                // Check if the current platform is iOS and if it is 13.4 or above
                var iosVersion = new Version(UIDevice.CurrentDevice.SystemVersion);
                if (iosVersion >= new Version(13, 4)) { return true; }
            }
            else if (DeviceInfo.Platform == DevicePlatform.MacCatalyst)
            {

#if MACCATALYST13_4_OR_GREATER

                return true;
#endif
            }

            return false;
        }

        /// <summary>
        /// Determines whether this gesture recognizer should receive the touch.
        /// </summary>
        /// <returns>
        /// Always returns false, indicating that this recognizer should not receive the touch.
        /// </returns>
        bool GesturerTouchRecognizer(UIGestureRecognizer recognizer, UITouch touch)
        {
            return false;
        }

        /// <summary>
        /// Handles scroll events and triggers the appropriate action on the TouchDetector.
        /// </summary>
        void OnScroll()
        {
            if (TouchDetector == null || !TouchDetector.MauiView.IsEnabled || TouchDetector.MauiView.InputTransparent)
            {
                return;
            }

            long pointerId = Handle.Handle.ToInt64();
            CGPoint delta = TranslationInView(View);
            CGPoint point = LocationInView(View);

            TouchDetector.OnScrollAction(pointerId, new Point(point.X, point.Y), delta.Y != 0 ? delta.Y : delta.X);
        }

        /// <summary>
        /// Determines whether this gesture recognizer should recognize simultaneously with other gesture recognizers.
        /// </summary>
        bool GestureRecognizer(UIGestureRecognizer gestureRecognizer, UIGestureRecognizer otherGestureRecognizer)
        {
            if (otherGestureRecognizer is GestureDetector.UIPanGestureExt || otherGestureRecognizer is UITouchRecognizerExt || TouchListener == null)
            {
                return true;
            }

            return !TouchListener.IsTouchHandled;
        }

        #endregion
    }

    #endregion
}
