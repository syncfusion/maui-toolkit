using CoreGraphics;
using UIKit;

namespace Syncfusion.Maui.Toolkit.GridSplitter
{
    /// <summary>
    /// iOS / MacCatalyst partial of <see cref="SfGridSplitter"/>.
    /// Wires a global "pointer left the splitter" listener on the
    /// splitter's platform view so every separator's hover state is
    /// cleared when the cursor leaves the entire splitter area.
    /// </summary>
    public partial class SfGridSplitter
    {
        /// <summary>
        /// Tracks whether the global pointer-leave hook has been
        /// attached to the splitter's platform view. Idempotent.
        /// </summary>
        bool _globalLeaveHandlersHooked;

        /// <summary>
        /// Cached reference to the splitter's iOS platform view so
        /// the global pointer-leave handler can be detached on
        /// handler change.
        /// </summary>
        UIView? _splitterPlatformView;

        /// <summary>
        /// <see cref="SfView.OnHandlerChanged"/> override. Wires the
        /// splitter's platform-view leave handlers so hover state is
        /// cleared when the touch leaves the entire splitter.
        /// </summary>
        protected override void OnHandlerChanged()
        {
            base.OnHandlerChanged();
            if (Handler?.PlatformView != null)
            {
                AttachGlobalPointerLeaveHook();
            }
            else
            {
                DetachGlobalPointerLeaveHook();
            }
        }

        /// <summary>
        /// Detaches the global pointer-leave listeners before the handler is
        /// replaced.
        /// </summary>
        protected override void OnHandlerChanging(HandlerChangingEventArgs args)
        {
            base.OnHandlerChanging(args);
            DetachGlobalPointerLeaveHook();
        }

        void AttachGlobalPointerLeaveHook()
        {
            if (_globalLeaveHandlersHooked)
            {
                return;
            }

            if (Handler?.PlatformView is UIView view)
            {
                _splitterPlatformView = view;
                var recognizer = new UIPanGestureRecognizer(OnSplitterPanEvent);
                recognizer.CancelsTouchesInView = false;
                recognizer.DelaysTouchesBegan = false;
                recognizer.DelaysTouchesEnded = false;
                recognizer.MaximumNumberOfTouches = 1;
                recognizer.MinimumNumberOfTouches = 1;
                view.AddGestureRecognizer(recognizer);
                _splitterPlatformView = view;
                view.IsAccessibilityElement = false;
                _globalLeaveHandlersHooked = true;
            }
        }

        void DetachGlobalPointerLeaveHook()
        {
            if (!_globalLeaveHandlersHooked || _splitterPlatformView == null)
            {
                return;
            }

            _splitterPlatformView = null;
            _globalLeaveHandlersHooked = false;
        }

        void OnSplitterPanEvent(UIPanGestureRecognizer recognizer)
        {
            if (recognizer.State != UIGestureRecognizerState.Cancelled &&
                recognizer.State != UIGestureRecognizerState.Failed)
            {
                return;
            }

            // The pan was cancelled or failed. The touch has left
            // the splitter area. Clear hover state so the control
            // does not stay in a stale hover.
            ForceClearAllSeparatorsHoverState();
        }
    }
}
