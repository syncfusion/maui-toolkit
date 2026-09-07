using Syncfusion.Maui.Toolkit.Internals;
using PointerEventArgs = Syncfusion.Maui.Toolkit.Internals.PointerEventArgs;

namespace Syncfusion.Maui.Toolkit.GridSplitter
{
    /// <summary>
    /// Cross-platform implementation of <see cref="SeparatorView"/>.
    /// Platform-specific behavior is provided by the corresponding platform partial classes.
    /// </summary>
    internal partial class SeparatorView
    {
        #region ITouchListener (Standard / fallback)

        /// <summary>
        /// Handles pointer hover state and tracks the latest pointer position for expand/collapse button hit-testing.
        /// Ensures tap handling uses an up-to-date pointer position, including fast taps outside the separator bounds.
        /// The pointer position is preserved across Released and Exited events for floating button interaction.
        /// </summary>
        public void OnTouch(PointerEventArgs e)
        {
            Point? pointerPosition = e.GetPosition(this);
            if (pointerPosition.HasValue)
            {
                _lastPointerPosition = pointerPosition.Value;
            }

            var splitter = FindContainingSplitter();
            if (splitter != null && pointerPosition.HasValue)
            {
                var splitterLocalPoint = e.GetPosition(splitter);
                if (splitterLocalPoint.HasValue && TryForwardToOwner(splitter, this, splitterLocalPoint.Value, out var owner))
                {
                    UpdateHoverState(false, null);
                    _lastPointerPosition = null;

                    owner?.OnTouch(e);
                    return;
                }
            }

            if (splitter != null && pointerPosition.HasValue)
            {
                var splitterLocalPoint = e.GetPosition(splitter);
                if (splitterLocalPoint.HasValue)
                {
                    ClearStaleHoverOnSiblings(splitter, splitterLocalPoint.Value);
                }
            }

            switch (e.Action)
            {
                case PointerActions.Entered:
                    // Cancel any pending hide so the buttons stay
                    // visible while the pointer is on the strip.
                    CancelScheduledHoverHide();
                    UpdateHoverState(true, pointerPosition);
                    break;

                case PointerActions.Exited:
                    // The pointer has left the separator view. We
                    // do not know where it went (it may have moved
                    // onto a floating button, into another pane, or
                    // out of the entire control). Schedule a delayed
                    // hide with the grace window so the user has a
                    // brief moment to move onto a floating button.
                    //
                    // The grace window is cancelled when:
                    //   - the pointer re-enters the strip (via the
                    //     ITouchListener Entered path), or
                    //   - the pointer enters a floating button
                    //     (via the button's PointerOverChanged
                    //     event in SfGridSplitter).
                    //
                    // When the grace window expires without a
                    // cancel, the timer fires ClearHoverImmediate,
                    // which sets IsHovered = false and returns the
                    // control to its normal state immediately.
                    if (IsDragging || _isStickyHovered)
                    {
                        break;
                    }

                    ScheduleHoverHide();
                    break;

                case PointerActions.Moved:
                    UpdateHoverState(pointerPosition.HasValue, pointerPosition);
                    break;

                case PointerActions.Pressed:
                    UpdateHoverState(true, pointerPosition);
                    break;

                case PointerActions.Released:
                    UpdateHoverState(IsPointInHoverRegion(pointerPosition), pointerPosition);
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Standard (non-platform) implementation of <see cref="ITouchListener.OnScrollWheel"/>.
        /// No-op; the separator does not respond to scroll wheel input.
        /// </summary>
        /// <param name="e">Scroll event arguments.</param>
        public void OnScrollWheel(ScrollEventArgs e)
        {
            // Separator does not consume scroll wheel input.
        }

        #endregion

        #region Platform Routing (Standard / Windows default)

        /// <summary>
        /// Standard / Windows default: leave <c>_usePlatformTouchPath</c> at its default
        /// <c>false</c>, meaning the <see cref="PanGestureRecognizer"/> path is the primary
        /// drag driver. Android and iOS / MacCatalyst partials override this to set the
        /// backing field to <c>true</c>.
        /// </summary>
        partial void InitializePlatformTouchPath()
        {
            // No-op. The shared class defaults _usePlatformTouchPath to false.
        }

        #endregion
    }
}
