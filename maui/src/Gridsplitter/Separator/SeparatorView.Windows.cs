using Syncfusion.Maui.Toolkit.Internals;
using PointerEventArgs = Syncfusion.Maui.Toolkit.Internals.PointerEventArgs;
using System.Reflection;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;

namespace Syncfusion.Maui.Toolkit.GridSplitter
{
    /// <summary>
    /// Windows-specific partial of <see cref="SeparatorView"/>.
    /// Implements <see cref="ITouchListener.OnTouch"/>, adds cursor customization
    /// </summary>
    internal partial class SeparatorView
    {
        #region Fields
        bool _cursorHandlersHooked;
        UIElement? _platformElement;

        #endregion
        
        #region 

        /// <summary>
        /// Cached <see cref="PropertyInfo"/> for the WinUI <c>UIElement.ProtectedCursor</c> property.
        /// The property is not part of the public Windows App SDK surface, so it must be
        /// located via reflection. The lookup is performed once per process and reused on
        /// every cursor change to avoid the per-call <see cref="Type.GetProperty(string, BindingFlags)"/>
        /// allocation cost on a hot hover path.
        /// </summary>
        static readonly PropertyInfo? s_protectedCursorProperty = typeof(UIElement).GetProperty( "ProtectedCursor", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        #endregion

        #region ITouchListener 

        /// <summary>
        /// Handles hover state and tracks the latest pointer position for expand/collapse button hit-testing on Windows.
        /// Ensures tap handling uses a valid pointer position and preserves it across Released and Exited events.
        /// </summary>
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

            if (splitter != null && dragPosition.HasValue
                && TryForwardToOwner(splitter, this, dragPosition.Value, out var owner))
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
                    break;

                case PointerActions.Moved:

                    {
                        bool pointerInHoverRegion = pointerPosition.HasValue && IsPointInHoverRegion(pointerPosition.Value);
                        UpdateHoverState(pointerInHoverRegion, pointerPosition);
                    }

                    // Keep the cursor in sync with the current pointer
                    if (_platformElement != null)
                    {
                        UpdateCursorForPointer(_platformElement, splitter, pointerPosition);
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

                case PointerActions.Pressed:

                    {
                        // The expand/collapse buttons are now real Views
                        // hosted in the splitter's overlay grid. They
                        // receive the press directly through their own
                        // touch path. The separator only acts on presses
                        // that fall on the strip itself.
                        bool isOnStripForPress = pointerPosition.HasValue && IsPositionOnSeparatorStrip(pointerPosition.Value);

                        if (isOnStripForPress && IsHovered)
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
                    }
                    break;

                case PointerActions.Released:
                    if (dragPosition.HasValue)
                    {
                        EndInternalDrag(dragPosition.Value, cancelled: false);
                    }
                    else if (pointerPosition.HasValue)
                    {
                        EndInternalDrag(pointerPosition.Value, cancelled: false);
                    }
                    else
                    {
                        EndInternalDrag(_lastTouchPoint, cancelled: false);
                    }


                    UpdateHoverState(IsPointInHoverRegion(pointerPosition), pointerPosition);
                    break;

                case PointerActions.Cancelled:

                    if (dragPosition.HasValue)
                    {
                        EndInternalDrag(dragPosition.Value, cancelled: true);
                    }
                    else if (pointerPosition.HasValue)
                    {
                        EndInternalDrag(pointerPosition.Value, cancelled: true);
                    }
                    else
                    {
                        EndInternalDrag(_lastTouchPoint, cancelled: true);
                    }

                    ForceClearHoverState();
                    break;
            }
        }

        /// <summary>
        /// Windows implementation of <see cref="ITouchListener.OnScrollWheel"/>.
        /// No-op (we do not consume scroll input on the separator).
        /// </summary>
        /// <param name="e">Scroll event arguments.</param>
        public void OnScrollWheel(ScrollEventArgs e)
        {
            // No scroll-wheel behavior on the separator.
        }

        #endregion

        #region Cursor Customization 

        /// <summary>
        /// Windows partial hook: attaches pointer-entered/exited handlers to the platform
        /// element to set the resize cursor (<c>SizeWestEast</c> for horizontal,
        /// <c>SizeNorthSouth</c> for vertical). Idempotent.
        /// </summary>
        partial void OnSeparatorHandlerAttached()
        {
            if (_cursorHandlersHooked)
            {
                return;
            }

            if (Handler?.PlatformView is UIElement element)
            {
                _platformElement = element;
                element.PointerEntered += OnPlatformPointerEntered;
                element.PointerExited += OnPlatformPointerExited;
                element.PointerCaptureLost += OnPlatformPointerCaptureLost;
                _cursorHandlersHooked = true;
            }
        }

        /// <summary>
        /// Windows partial hook: detaches the cursor handlers and restores the default arrow.
        /// </summary>
        partial void OnSeparatorHandlerDetaching()
        {
            if (!_cursorHandlersHooked || _platformElement == null)
            {
                return;
            }

            _platformElement.PointerEntered -= OnPlatformPointerEntered;
            _platformElement.PointerExited -= OnPlatformPointerExited;
            _platformElement.PointerCaptureLost -= OnPlatformPointerCaptureLost;
            SetCursor(_platformElement, InputSystemCursorShape.Arrow);
            _platformElement = null;
            _cursorHandlersHooked = false;
        }

        void OnPlatformPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (sender is UIElement element)
            {
                var splitter = FindContainingSplitter();

                Point? localPoint = TryGetLocalPointFromPlatformEvent(e, element);
                UpdateCursorForPointer(element, splitter, localPoint);

                // Cancel any pending hover-hide so the buttons stay
                // visible the moment the pointer re-enters the strip.
                // Without this, a pending hide from a previous exit
                // would fire after the Entered event and tear down the
                // hover state that Entered just established — causing
                // the buttons to flicker on every strip-to-button-to-
                // strip round trip.
                CancelScheduledHoverHide();

                if (splitter != null)
                {
                    var ownerSeparator = ResolveOwningSeparatorFromPlatformEvent(splitter, e, element);
                    if (ownerSeparator != null && ownerSeparator != this)
                    {
                        ownerSeparator.CancelScheduledHoverHide();
                        ownerSeparator.UpdateHoverState(true, null);
                        if (IsHovered)
                        {
                            IsHovered = false;
                            InvalidateDrawable();
                            InvalidateMeasure();
                        }
                    }
                    else
                    {
                        UpdateHoverState(true, null);
                    }
                }
            }

        }

        void OnPlatformPointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (sender is UIElement element)
            {
                SetCursor(element, InputSystemCursorShape.Arrow);

                var splitter = FindContainingSplitter();

                // The pointer has left the strip. We do not know
                // where it went. Schedule a delayed hide with the
                // grace window so the user has a brief moment to
                // move onto a floating button. The grace is
                // cancelled when the user re-enters the strip or
                // moves onto a button.
                //
                // Special case: when there is no reachable
                // expand/collapse button to transition to, clear
                // the hover state immediately - there is no button
                // to wait for.
                if (!IsDragging && !_isStickyHovered)
                {
                    if (HasAnyReachableButtonForTransition(splitter))
                    {
                        ScheduleHoverHide();
                    }
                    else
                    {
                        UpdateHoverState(false, null);
                    }
                }
                else
                {
                    UpdateHoverState(false, null);
                }

                if (splitter != null)
                {
                    var ownerSeparator = ResolveOwningSeparatorFromPlatformEvent(splitter, e, element);
                    if (ownerSeparator != null && ownerSeparator != this)
                    {
                        ownerSeparator.UpdateHoverState(false, null);
                    }
                }
            }
        }

        void OnPlatformPointerCaptureLost(object sender, PointerRoutedEventArgs e)
        {
            if (sender is UIElement element)
            {
                SetCursor(element, InputSystemCursorShape.Arrow);
                var splitter = FindContainingSplitter();

                ForceClearHoverState();
                if (splitter != null)
                {
                    var ownerSeparator = ResolveOwningSeparatorFromPlatformEvent(splitter, e, element);
                    if (ownerSeparator != null && ownerSeparator != this)
                    {
                        ownerSeparator.ForceClearHoverState();
                    }
                }
            }
        }

        /// <summary>
        /// Resolves the separator associated with a Windows pointer event by converting the pointer position to splitter coordinates.
        /// Returns <c>null</c> when no separator is hit or the coordinate transformation cannot be performed.
        /// </summary>
        SeparatorView? ResolveOwningSeparatorFromPlatformEvent( SfGridSplitter splitter, PointerRoutedEventArgs e, UIElement sourceElement)
        {
            if (splitter == null || sourceElement == null)
                return null;

            var pp = e.GetCurrentPoint(sourceElement);
            double px = pp.Position.X;
            double py = pp.Position.Y;

            var splitterPlatform = splitter.Handler?.PlatformView as UIElement;
            if (splitterPlatform == null)
            {
                return null;
            }

            var transform = sourceElement.TransformToVisual(splitterPlatform);
            if (transform == null)
            {
                return null;
            }

            var point = transform.TransformPoint(new Windows.Foundation.Point(px, py));
            return ResolveOwningSeparator(splitter, this, new Point(point.X, point.Y));
        }

        /// <summary>
        /// Returns the current pointer position expressed in this separator's
        /// own coordinate system. Used by the Windows cursor logic to verify
        /// that the pointer is actually over the visible separator strip before
        /// swapping to the resize cursor.
        /// </summary>
        Point? TryGetLocalPointFromPlatformEvent(PointerRoutedEventArgs e, UIElement sourceElement)
        {
            if (e == null || sourceElement == null)
            {
                return null;
            }

            var pp = e.GetCurrentPoint(sourceElement);
            return new Point(pp.Position.X, pp.Position.Y);
        }

        /// <summary>
        /// Updates the cursor for the platform element based on whether the
        /// pointer is over the visible separator strip. The strip hit-test
        /// uses the same rectangle as the rest of the separator so the cursor
        /// is only changed when the pointer is actually on the interactive
        /// area, not when it is in the extended margin that hosts the floating
        /// buttons.
        /// </summary>
        void UpdateCursorForPointer(UIElement element, SfGridSplitter? splitter, Point? localPoint)
        {
            if (element == null)
            {
                return;
            }

            bool pointerOnStrip = localPoint.HasValue && IsPositionOnSeparatorStrip(localPoint.Value);

            if (pointerOnStrip)
            {
                bool resizeApplicable = splitter != null && !IsLeadingPaneCollapsed(splitter) && !IsTrailingPaneCollapsed(splitter) && AreAdjacentPanesResizable(splitter);

                if (resizeApplicable)
                {
                    var shape = (splitter!.Orientation == GridSplitterOrientation.Horizontal) ? InputSystemCursorShape.SizeWestEast : InputSystemCursorShape.SizeNorthSouth;
                    SetCursor(element, shape);
                    return;
                }
            }

            SetCursor(element, InputSystemCursorShape.Arrow);
        }


        /// <summary>
        /// Sets the cursor for a element using the internal cursor property.
        /// Failures are ignored to ensure resize and drag interactions continue normally.
        /// </summary>

        static void SetCursor(UIElement element, InputSystemCursorShape shape)
        {
            if (element == null)
            {
                return;
            }

            var prop = s_protectedCursorProperty;
            if (prop == null || !prop.CanWrite)
            {
                return;
            }

            prop.SetValue(element, InputSystemCursor.Create(shape));

        }

        #endregion

        #region Platform Routing 
        /// <summary>
        /// Enables the platform touch path on Windows and skips <see cref="PanGestureRecognizer"/> registration.
        /// This prevents tap gestures from being cancelled while preserving drag behavior through the touch handling path.
        /// </summary>
        partial void InitializePlatformTouchPath()
        {
            _usePlatformTouchPath = true;
        }

        #endregion
    }
}
