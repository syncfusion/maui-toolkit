using System.Reflection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;

namespace Syncfusion.Maui.Toolkit.GridSplitter
{
    /// <summary>
    /// Windows-specific partial of <see cref="SfGridSplitter"/>.
    /// Wires a global pointer-leave listener on the splitter's
    /// platform view so every separator's hover state is cleared
    /// when the cursor leaves the entire splitter area. Without
    /// this safety net, a separator can stay in the hover state if
    /// the cursor moves completely off the control without the
    /// per-separator ITouchListener receiving an Exited event
    /// (e.g. when the cursor leaves through a pane edge that does
    /// not forward the pointer-leave to the separator's view).
    /// </summary>
    public partial class SfGridSplitter
    {
        /// <summary>
        /// Tracks whether the global pointer-leave hook has been
        /// attached to the splitter's platform view. Idempotent so
        /// repeated handler changes do not double-wire the event.
        /// </summary>
        bool _globalLeaveHandlersHooked;

        /// <summary>
        /// Cached reference to the splitter's platform element so
        /// the global pointer-leave handler can be detached on
        /// handler change.
        /// </summary>
        UIElement? _splitterPlatformElement;

        /// <summary>
        /// <see cref="SfView.OnHandlerChanged"/> override. Wires
        /// the splitter's platform-view pointer-leave / pointer-
        /// capture-lost listeners so hover state is cleared when
        /// the cursor leaves the entire splitter.
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

            if (Handler?.PlatformView is UIElement element)
            {
                _splitterPlatformElement = element;
                element.PointerExited += OnSplitterPointerExited;
                element.PointerCaptureLost += OnSplitterPointerCaptureLost;
                _globalLeaveHandlersHooked = true;
            }
        }

        void DetachGlobalPointerLeaveHook()
        {
            if (!_globalLeaveHandlersHooked || _splitterPlatformElement == null)
            {
                return;
            }

            _splitterPlatformElement.PointerExited -= OnSplitterPointerExited;
            _splitterPlatformElement.PointerCaptureLost -= OnSplitterPointerCaptureLost;
            _splitterPlatformElement = null;
            _globalLeaveHandlersHooked = false;
        }

        void OnSplitterPointerExited(object sender, PointerRoutedEventArgs e)
        {
            // The cursor has left the entire splitter area. Clear
            // hover state on every separator so the control returns
            // to the normal state immediately, instead of waiting for
            // a per-separator Exited event that may never arrive
            // (e.g. when the cursor leaves through a pane edge that
            // does not forward to the separator's view).
            ForceClearAllSeparatorsHoverState();
        }

        void OnSplitterPointerCaptureLost(object sender, PointerRoutedEventArgs e)
        {
            // Pointer capture was lost (e.g. another control stole
            // capture, or the OS took capture back). Clear hover
            // state so the control does not stay in a stale hover.
            ForceClearAllSeparatorsHoverState();
        }
    }
}
