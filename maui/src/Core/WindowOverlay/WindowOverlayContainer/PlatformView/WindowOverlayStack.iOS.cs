using CoreGraphics;
using Foundation;
using UIKit;

namespace Syncfusion.Maui.Toolkit.Internals
{
    internal class WindowOverlayStack : UIView
    {
        internal bool _canHandleTouch = false;
        WindowOverlayContainer? _virtualView;
        bool _isConnected = false;

        public override UIView HitTest(CGPoint point, UIEvent? uievent)
        {
            UIView? view = base.HitTest(point, uievent);
            // TODO: Check possibility to pass null and remove suppressing the warning.
#pragma warning disable CS8603 // Possible null reference return.
            return !_canHandleTouch && view == this ? null : view;
#pragma warning restore CS8603 // Possible null reference return.
        }

        internal void Connect(WindowOverlayContainer mauiView)
        {
            if (_isConnected && ReferenceEquals(_virtualView, mauiView))
            {
                return;
            }

            _virtualView = mauiView;
            _isConnected = true;
        }

        internal void DisConnect()
        {
            if (!_isConnected)
            {
                return;
            }

            _virtualView = null;
            _isConnected = false;
        }

        public override void TouchesBegan(NSSet touches, UIEvent? evt)
        {
            base.TouchesBegan(touches, evt);
            if (touches.AnyObject is UITouch touch)
            {
                CGPoint point = touch.LocationInView(this);
                _virtualView?.ProcessTouchInteraction((float)point.X, (float)point.Y);
            }
        }
    }
}
