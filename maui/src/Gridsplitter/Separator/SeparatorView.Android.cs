using Syncfusion.Maui.Toolkit.Internals;
using PointerEventArgs = Syncfusion.Maui.Toolkit.Internals.PointerEventArgs;

namespace Syncfusion.Maui.Toolkit.GridSplitter
{
    /// <summary>
    /// Android-specific partial of <see cref="SeparatorView"/>.
    /// Implements <see cref="ITouchListener.OnTouch"/> and <see cref="ITouchListener.OnScrollWheel"/>
    /// for the Android target framework.
    /// </summary>
    internal partial class SeparatorView
    {
        #region ITouchListener 

        /// <summary>
        /// Android implementation of <see cref="ITouchListener.OnTouch"/>.
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
                    // Cancel any pending hover-hide so the buttons
                    // stay visible while the pointer is on the strip.
                    CancelScheduledHoverHide();
                    UpdateHoverState(true, pointerPosition);
                    break;

                case PointerActions.Exited:
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
                    // schedule a hide with zero grace so the control
                    // returns to its normal state the moment the
                    // cursor leaves the strip - there is no button
                    // to wait for.
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
                    break;

                case PointerActions.Pressed:
                    // The expand/collapse buttons are now real Views
                    // hosted in the splitter's overlay grid. They
                    // receive the press directly through their own
                    // touch path. The separator only acts on presses
                    // that fall on the strip itself. Fall back to the
                    // dragPosition when pointerPosition is on the
                    // splitter's margin but the press is clearly
                    // meant for the strip (this can happen on Android
                    // when the touch is delivered at the very edge of
                    // the view's bounds but is still intended for the
                    // draggable area).
                    bool isOnStrip = pointerPosition.HasValue && IsPositionOnSeparatorStrip(pointerPosition.Value);

                    // If pointerPosition is outside the strict strip
                    // bounds but the splitter-coord dragPosition is
                    // inside the strip, accept the press as a strip
                    // press so the user can still start a drag from
                    // the layout-rotated drag-coordinate space
                    // (useful on Android where touch coordinates may
                    // be reported at sub-pixel boundaries).
                    if (!isOnStrip && splitter != null && dragPosition.HasValue)
                    {
                        isOnStrip = IsPointOnOwnStripInSplitterCoords(splitter, dragPosition.Value);
                    }

                    if (isOnStrip)
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
                        else
                        {
                            // No usable coordinates for the drag
                            // route, but the press is on the strip.
                            // Still start the drag using the last
                            // known touch point so the touch path
                            // is consistent with the rest of the
                            // platform handlers (Windows / iOS).
                            BeginInternalDrag(_lastPointerPosition ?? new Point(0, 0));
                        }
                    }
                    break;

                case PointerActions.Moved:
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
					bool wasDragging = _dragInProgress || IsDragging;
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

					// Keep the hover state active on release when the pointer is
					// still inside the unified hover region (strip, bridge, or
					// a visible button) so the visual state survives a release
					// on a button and the user can still see the buttons after
					// the click. This is the same check used on MacCatalyst and
					// Windows, ensuring consistent release / normal-state
					// behavior across all platforms. _isStickyHovered is
					// already cleared by EndInternalDrag on a non-cancelled
					// release, so we MUST consult IsPointInHoverRegion instead
					// of the sticky flag to decide whether the buttons stay
					// visible.
					bool keepHover = pointerPosition.HasValue && IsPointInHoverRegion(pointerPosition.Value);
                    UpdateHoverState(keepHover, pointerPosition);

					if (e.Action == PointerActions.Released && !wasDragging
						 && pointerPosition.HasValue && splitter != null && !s_tapGestureResolved && TryResolveAndExecuteButtonTap(splitter, pointerPosition.Value))
					{
						// Skip the direct-tap path when the release followed a drag, so the
						// expand/collapse action is only triggered by an explicit tap on
						// the floating button rather than a drag-end release-over-button.
						// The timestamp is preserved so the same suppression also covers the
						// TapGestureRecognizer's OnTapped that fires after this Released touch.
						ClearTapVisualState();
						_lastPointerPosition = null;
						s_directTapHandled = true;
					}

                    else if (e.Action == PointerActions.Released && s_tapGestureResolved)
                    {
                        s_tapGestureResolved = false;
						ClearTapVisualState();
					}
                    break;
            }
        }

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
		/// Android implementation of <see cref="ITouchListener.OnScrollWheel"/>.
		/// No-op (Android devices do not have scroll wheels on the separator area).
		/// </summary>
		/// <param name="e">Scroll event arguments.</param>
		public void OnScrollWheel(ScrollEventArgs e)
        {
            // No scroll-wheel behavior on Android for the separator.
        }

        #endregion

        #region Platform Routing 

        /// <summary>
        /// On Android, set <c>_usePlatformTouchPath = true</c>: the <c>OnTouch</c> path
        /// is the primary drag driver, and the <see cref="PanGestureRecognizer"/> registered
        /// in the shared constructor is skipped.
        /// </summary>
        partial void InitializePlatformTouchPath()
        {
            _usePlatformTouchPath = true;
        }

		#endregion

	}
}
