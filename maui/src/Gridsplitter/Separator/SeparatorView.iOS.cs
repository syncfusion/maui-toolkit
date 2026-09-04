using Syncfusion.Maui.Toolkit.Internals;
using PointerEventArgs = Syncfusion.Maui.Toolkit.Internals.PointerEventArgs;

namespace Syncfusion.Maui.Toolkit.GridSplitter
{
    /// <summary>
    /// iOS / MacCatalyst partial of <see cref="SeparatorView"/>.
    /// Implements <see cref="ITouchListener.OnTouch"/> and <see cref="ITouchListener.OnScrollWheel"/>
    /// for the iOS and MacCatalyst target frameworks.
    /// </summary>
    internal partial class SeparatorView
    {
        #region ITouchListener 

        /// <summary>
        /// iOS / MacCatalyst implementation of <see cref="ITouchListener.OnTouch"/>.
        /// Handles hover (Entered/Exited) and drives drag directly from Pressed/Moved/Released.
        /// </summary>
        /// <param name="e">Pointer event arguments.</param>
        public void OnTouch(PointerEventArgs e)
        {
            if (e == null)
                return;

            Point? pointerPosition = e.GetPosition(this);
            if (pointerPosition.HasValue)
            {
                _lastPointerPosition = pointerPosition.Value;
            }

            Point? dragPosition = null;
            var splitter = FindContainingSplitter();
            if (splitter != null)
            {
                dragPosition = e.GetPosition(splitter);
            }

            bool onOwnStrip = pointerPosition.HasValue && IsPositionOnSeparatorStrip(pointerPosition.Value);
            if (!onOwnStrip && splitter != null && dragPosition.HasValue)
            {
                onOwnStrip = IsPointOnOwnStripInSplitterCoords(splitter, dragPosition.Value);
            }
            if (!onOwnStrip && splitter != null && dragPosition.HasValue && TryForwardToOwner(splitter, this, dragPosition.Value, out var owner))
            {
                UpdateHoverState(false, null);
                _lastPointerPosition = null;

                owner?.OnTouch(e);
                return;
            }

            if (splitter != null && dragPosition.HasValue)
            {
                ClearStaleHoverOnSiblings(splitter, dragPosition.Value);
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
#if MACCATALYST

                    // The pointer has left the separator view. We
                    // do not know where it went. Schedule a delayed
                    // hide with the grace window so the user has a
                    // brief moment to move onto a floating button.
                    // The grace is cancelled when the user re-enters
                    // the strip or moves onto a button (button's
                    // PointerOverChanged event).
                    //
                    // Special case: when there is no reachable
                    // expand/collapse button to transition to,
                    // clear the hover state immediately - there is
                    // no button to wait for.
                    if (IsDragging || _isStickyHovered)
                    {
                        break;
                    }

                    if (HasAnyReachableButtonForTransition(splitter))
                    {
                        ScheduleHoverHide();
                    }
                    else
                    {
                        UpdateHoverState(false, null);
                    }
#else
                    // iOS: same delayed-hide / immediate-clear
                    // strategy as MacCatalyst. The cursor has left
                    // the separator view and we do not know where
                    // it went, so schedule a grace-windowed hide.
                    // The grace is cancelled if the user re-enters
                    // the strip or moves onto a button.
                    if (IsDragging || _isStickyHovered)
                    {
                        break;
                    }

                    if (HasAnyReachableButtonForTransition(splitter))
                    {
                        ScheduleHoverHide();
                    }
                    else
                    {
                        UpdateHoverState(false, null);
                    }
#endif
                    break;

                case PointerActions.Pressed:
#if !MACCATALYST && IOS
                    // The expand/collapse buttons are now real Views
                    // hosted in the splitter's overlay grid. They
                    // receive the press directly through their own
                    // touch path. The separator only acts on presses
                    // that fall on the strip itself.
                    bool isOnStrip_iOS = pointerPosition.HasValue && IsPositionOnSeparatorStrip(pointerPosition.Value);

                    if (isOnStrip_iOS)
                    {
                        UpdateHoverState(true, pointerPosition);

                        if (dragPosition.HasValue)
                        {
                            BeginInternalDrag(dragPosition.Value);
                        }
                        else if (pointerPosition.HasValue)
                        {
                            BeginInternalDrag(pointerPosition.Value);
                        }
                    }
                    break;
#else

                    if (pointerPosition.HasValue)
                    {
                        UpdateHoverState(true, pointerPosition);
                    }
                    if (dragPosition.HasValue)
                    {
                        _isInternalDragging = true;
                        _initialTouchPoint = dragPosition.Value;
                        _lastTouchPoint = dragPosition.Value;
                    }
                    else if (pointerPosition.HasValue)
                    {
                        _isInternalDragging = true;
                        _initialTouchPoint = pointerPosition.Value;
                        _lastTouchPoint = pointerPosition.Value;
                    }
                    break;
#endif

                case PointerActions.Moved:

                    if (pointerPosition.HasValue)
                    {
                        UpdateHoverState(true, pointerPosition);
                    }

                    if (dragPosition.HasValue)
                    {
                        UpdateInternalDrag(dragPosition.Value);
                    }
                    else if (pointerPosition.HasValue)
                    {
                        UpdateInternalDrag(pointerPosition.Value);
                    }
                    break;

                case PointerActions.Released:
                case PointerActions.Cancelled:
                    if (e.Action == PointerActions.Cancelled && _dragInProgress && _lastMoveEventTicks > 0)
                    {
                        long elapsedSinceMoveMs = (DateTime.UtcNow.Ticks - _lastMoveEventTicks) / TimeSpan.TicksPerMillisecond;
                        if (elapsedSinceMoveMs < SpuriousCancelWindowMs)
                        {
                            break;
                        }
                    }

                    if (dragPosition.HasValue)
                    {
                        EndInternalDrag(dragPosition.Value, cancelled: e.Action == PointerActions.Cancelled);
                    }
                    else if (pointerPosition.HasValue)
                    {
                        EndInternalDrag(pointerPosition.Value, cancelled: e.Action == PointerActions.Cancelled);
                    }
                    else
                    {
                        EndInternalDrag(_lastTouchPoint, cancelled: e.Action == PointerActions.Cancelled);
                    }

#if MACCATALYST
                    // Keep the hover state active on release when the pointer is still
                    // inside the unified hover region (strip or floating button area),
                    // so the visual state survives a release on a button and the user
                    // can still see the buttons after the click.
                    UpdateHoverState(IsPointInHoverRegion(pointerPosition), pointerPosition);
#else
                    // iOS: same unified-hover-region check as MacCatalyst,
                    // Android, and Windows so the release / normal-state
                    // behavior is consistent across all platforms.
                    UpdateHoverState(false, null);
#endif

                    if (e.Action == PointerActions.Released && pointerPosition.HasValue && splitter != null)
                    {
                        // Skip the direct-tap path when the release followed a drag, so the
                        // expand/collapse action is only triggered by an explicit tap on
                        // the floating button rather than a drag-end release-over-button.
                        // The timestamp is preserved so the same suppression also covers the
                        // TapGestureRecognizer's OnTapped that fires after this Released touch.
                        bool dragJustEnded = IsWithinDragEndSuppressionWindow();

                        if (!dragJustEnded && TryResolveAndExecuteButtonTap(splitter, pointerPosition.Value))
                        {
                            ClearTapVisualState();
                            _lastPointerPosition = null;
                            s_directTapHandled = true;
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Resolves a tap to a visible expand/collapse button and executes the corresponding action directly on iOS and MacCatalyst.
        /// Used as a workaround when <see cref="TapGestureRecognizer"/> does not receive taps outside the separator view bounds.
        /// Prevents duplicate execution if <see cref="OnTapped"/> is subsequently raised.
        /// </summary>
        bool TryResolveAndExecuteButtonTap(SfGridSplitter splitter, Point tapPositionInView)
        {
            if (splitter == null)
            {
                return false;
            }

            if (TrailingPaneIndex < 0 || TrailingPaneIndex >= splitter.SplitterPanes.Count)
            {
                return false;
            }

            if (!TryGetButtonHitTargets(out float leadingButtonX, out float leadingButtonY, out float trailingButtonX, out float trailingButtonY, out float buttonRadius))
            {
                return false;
            }

            var tapPoint = new PointF((float)tapPositionInView.X, (float)tapPositionInView.Y);
            float hitRadius = buttonRadius + 6f;

            bool leadingVisible = IsLeadingIconVisible(splitter);
            bool trailingVisible = IsTrailingIconVisible(splitter);

            bool tappedLeading = leadingVisible && IsPointInsideCircle(tapPoint, leadingButtonX, leadingButtonY, hitRadius);
            bool tappedTrailing = !tappedLeading && trailingVisible && IsPointInsideCircle(tapPoint, trailingButtonX, trailingButtonY, hitRadius);

            if (!tappedLeading && !tappedTrailing)
            {
                return TryRouteTapToOverlappingSibling(splitter, tapPositionInView, tapPoint, hitRadius);
            }
            ExecuteButtonAction(splitter, tappedLeading);
            return true;
        }

        /// <summary>
        /// iOS / MacCatalyst implementation of <see cref="ITouchListener.OnScrollWheel"/>.
        /// On MacCatalyst this could be used to drive a resize; for now it is a no-op so the
        /// platform build is consistent.
        /// </summary>
        /// <param name="e">Scroll event arguments.</param>
        public void OnScrollWheel(ScrollEventArgs e)
        {
            // No scroll-wheel behavior on iOS. On MacCatalyst the separator could opt in
            // to wheel-based resize here, but doing so is deferred to a follow-up.
        }

        #endregion

        #region Platform Routing 

        /// <summary>
        /// On iOS / MacCatalyst, set <c>_usePlatformTouchPath = true</c>: the <c>OnTouch</c>
        /// path is the primary drag driver, and the <see cref="PanGestureRecognizer"/>
        /// registered in the shared constructor is skipped.
        /// </summary>
        partial void InitializePlatformTouchPath()
        {
            _usePlatformTouchPath = true;
        }

        #endregion
    }
}
