using Microsoft.Maui.Handlers;

namespace Syncfusion.Maui.Toolkit.GridSplitter
{
    /// <summary>
    /// Android-specific partial of <see cref="SfGridSplitter"/>.
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
        /// Cached reference to the splitter's Android platform view
        /// so the global pointer-leave handler can be detached on
        /// handler change.
        /// </summary>
        Android.Views.View? _splitterPlatformView;

        /// <summary>
        /// <see cref="SfView.OnHandlerChanged"/> override. Wires the
        /// splitter's platform-view hover-exit listener so hover
        /// state is cleared when the cursor / touch leaves the
        /// entire splitter.
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

            if (Handler?.PlatformView is Android.Views.View view)
            {
                _splitterPlatformView = view;
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

    }
}
