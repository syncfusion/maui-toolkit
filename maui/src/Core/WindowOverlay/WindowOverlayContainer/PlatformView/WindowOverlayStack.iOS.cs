using CoreGraphics;
using Foundation;
using UIKit;

namespace Syncfusion.Maui.Toolkit.Internals
{
    internal class WindowOverlayStack : UIView
    {
        internal bool _canHandleTouch = false;
        WindowOverlayContainer? _virtualView;

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
            _virtualView = mauiView;
        }

        internal void DisConnect()
        {
            _virtualView = null;
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
