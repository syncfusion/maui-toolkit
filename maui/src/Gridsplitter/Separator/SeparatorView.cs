using Syncfusion.Maui.Toolkit.Internals;
using Microsoft.Maui.Dispatching;
#if IOS
using UIKit;
#endif
namespace Syncfusion.Maui.Toolkit.GridSplitter
{
	/// <summary>
	/// Internal separator view used by <see cref="SfGridSplitter"/>.
	/// Handles separator rendering, interaction, accessibility, and layout between adjacent panes.
	/// </summary>
    internal partial class SeparatorView : SfView, ITouchListener
    {
        #region Fields

        /// <summary>
        /// Timestamp of last Resizing event (for throttling at ~60 FPS).
        /// </summary>
        long _lastResizingEventTicks;

		/// <summary>
		/// Records the timestamp of the most recent drag-move event.
		/// Used to ignore transient cancel events and keep active drag operations uninterrupted.
		/// </summary>
        long _lastMoveEventTicks;

		/// <summary>
		/// Defines the time window for ignoring spurious <c>Cancelled</c> events during an active drag.
		/// Helps prevent unintended drag termination while still allowing normal release handling.
		/// </summary>
        internal const int SpuriousCancelWindowMs = 50;

		/// <summary>
		/// Stores the initial touch point used to calculate drag movement during touch interactions.
		/// Supports platform-specific drag tracking without relying on <see cref="PanGestureRecognizer"/>.
		/// </summary>
        Point _initialTouchPoint;

        /// <summary>
        /// Last reported touch point during a platform-driven drag (Android/iOS partials).
        /// </summary>
        Point _lastTouchPoint;

        /// <summary>
        /// Re-entrancy guard. Set while a drag is being driven by the platform touch listener
        /// (Android/iOS) so the <see cref="PanGestureRecognizer"/> path (which is the primary
        /// path on Windows) does not double-fire drag callbacks.
        /// </summary>
        bool _isInternalDragging;

        /// <summary>
        /// True when a drag has been started by either the gesture recognizer or the
        /// platform touch listener and has not yet ended.
        /// </summary>
        internal bool _dragInProgress;

        /// <summary>
        /// Threshold (16 ms) between consecutive Resizing events for throttling.
        /// </summary>
        const long ResizingThrottleMs = 16;

        /// <summary>
        /// Drag distance threshold (DIU) before a press is promoted to a real drag.
        /// </summary>
        const double DragThresholdDiu = 4.0;

		/// <summary>
		/// Defines the size used for resize-handle rendering and hit-testing calculations.
		/// The visible handle size is defined by <see cref="DrawResizeHandle"/>, while this constant is retained for layout compatibility.
		/// </summary>
        internal const float ResizeHandleDiameter = 0f;

		/// <summary>
		/// Defines the fixed diameter of each floating expand/collapse button.
		/// The button size is independent of <see cref="SfGridSplitter.SeparatorSize"/>.
		/// </summary>
        internal const float ExpandCollapseButtonDiameter = 25f;

		/// <summary>
		/// Defines the fixed size of the chevron displayed within expand/collapse buttons.
		/// The chevron is rendered as a compact, balanced directional indicator with
		/// arm-length equal to arm-spread so the glyph reads as a clean, proportional
		/// chevron that matches the modern Fluent chevron design reference.
		/// </summary>
        internal const float ExpandCollapseArrowSize = 6f;

		/// <summary>
		/// Defines the fixed spacing between the separator edge and each floating expand/collapse button.
		/// Keeps button positioning consistent regardless of <see cref="SfGridSplitter.SeparatorSize"/>.
		/// </summary>
        internal const float FloatingButtonEdgeOffset = ExpandCollapseButtonDiameter / 2f;

		/// <summary>
		/// Defines the visual gap between the separator strip and floating expand/collapse buttons.
		/// Helps keep buttons visually distinct from the separator while maintaining accurate hit targets.
		/// </summary>
        internal const float ButtonSeparatorGap = 8f;

        /// <summary>
        /// Total horizontal hit-zone expansion (DIU on each side of the separator) for
        /// pointer interaction. Used in Horizontal orientation.
        /// </summary>
        const double HorizontalHitZonePadding = 12.0;

        /// <summary>
        /// Total vertical hit-zone expansion (DIU on each side of the separator) for
        /// pointer interaction. Used in Vertical orientation.
        /// </summary>
        const double VerticalHitZonePadding = 12.0;

        /// <summary>
        /// Stores the last pointer position inside the separator for hit testing taps.
        /// </summary>
        Point? _lastPointerPosition;

        /// <summary>
        /// Ticks captured when a separator drag finishes. Used to suppress the
        /// <see cref="TapGestureRecognizer"/>-driven <c>OnTapped</c> that would
        /// otherwise fire when the gesture-recognizer reports the tap right after
        /// a pan completes. See <see cref="OnTapped"/> for the suppression logic.
        /// </summary>
        long _lastDragEndTicks;

        /// <summary>
        /// Dispatcher timer used to delay hiding the floating
        /// expand/collapse buttons after the pointer leaves the
        /// strip. The grace period allows the user to move the
        /// pointer from the strip onto a button without the
        /// buttons flickering or disappearing before the user can
        /// press them. If the pointer enters a button (or
        /// re-enters the strip) within the grace window, the
        /// pending hide is cancelled.
        /// </summary>
        IDispatcherTimer? _hoverHideTimer;

        /// <summary>
        /// Duration (in milliseconds) of the grace period after the
        /// pointer leaves the strip during which the floating
        /// buttons remain visible. If the pointer enters a button
        /// (or re-enters the strip) within this window, the hide
        /// is cancelled and the buttons stay visible. Tuned to a
        /// short value so the control returns to the normal state
        /// quickly when the user truly moves the cursor away, while
        /// still leaving enough headroom for the strip-to-button
        /// transition on every platform.
        /// </summary>
        internal const int HoverHideGraceMs = 200;

		/// <summary>
		/// Indicates whether this separator currently owns the sticky-hover state.
		/// Used with <see cref="s_stickyOwner"/> to ensure only one separator remains sticky-hovered at a time.
		/// </summary>
        internal bool _isStickyHovered;

		/// <summary>
		/// References the separator that currently owns the sticky-hover state on mobile platforms.
		/// Ensures only one separator remains sticky-hovered at a time.
		/// </summary>
        static SeparatorView? s_stickyOwner;

#if ANDROID || IOS || MACCATALYST
		/// <summary>
		/// Indicates that a tap action has already been handled by the direct touch path.
		/// Used to prevent the gesture-recognizer path from executing the same action more than once.
		/// </summary>
        static bool s_directTapHandled;
#endif

#if ANDROID 
		/// <summary>
		/// Indicates that a tap action has already been handled by the gesture-recognizer path.
		/// Used to prevent the touch handler from executing the same expand/collapse action twice.
		/// </summary>
        static bool s_tapGestureResolved;
#endif

        /// <summary>
        /// Stores the last reported total drag delta from the pan gesture.
        /// </summary>
        double _lastPanTotal;

        /// <summary>
        /// Cached size of the separator (width/height perpendicular to the divider).
        /// Refreshed whenever the separator is arranged.
        /// </summary>
        Size _arrangedSize;

		/// <summary>
		/// Callback when drag starts (called by gesture recognizer).
		/// </summary>
		Action<int>? _onDragStart;

		/// <summary>
		/// Callback when drag delta occurs (called by gesture recognizer).
		/// </summary>
		Action<int, double>? _onDragDelta;

		/// <summary>
		/// Callback when drag stops (called by gesture recognizer).
		/// </summary>
		Action<int>? _onDragStop;

		/// <summary>
		/// The current view instance created from <see cref="SfGridSplitter.ResizeIconTemplate"/>.
		/// Null when no template is set and the built-in resize handle is displayed.
		/// </summary>
		View? _resizeIconTemplateView;

		/// <summary>
		/// Backing field for the resize-icon template used by this separator.
		/// Each separator maintains its own template instance for independent rendering and updates.
		/// </summary>
		DataTemplate? _resizeIconTemplate;

		/// <summary>
		/// Gets a value indicating whether the separator is actively being dragged.
		/// Becomes <c>true</c> after the drag threshold is exceeded and resets on release.
		/// </summary>
		internal bool IsDragging { get; private set; }

		/// <summary>
		/// Gets a value indicating whether the separator or its adjacent button regions are hovered.
		/// Controls floating icon visibility and does not indicate that a resize operation is active.
		/// </summary>
		internal bool IsHovered { get; private set; }

		/// <summary>
		/// Gets the bounds of the interaction area used for pointer input.
		/// The hit zone may be larger than the visual separator for better accessibility.
		/// </summary>
		internal Rect HitZoneBounds { get; private set; }

		/// <summary>
		/// Gets the accessibility description used by assistive technologies.
		/// Provides context such as pane separator information and resize instructions.
		/// </summary>
		internal string GetSemanticDescription() => $"Separator between pane {TrailingPaneIndex} and pane {TrailingPaneIndex + 1}. Use drag to resize.";

		/// <summary>
		/// Gets or sets the zero-based index of the pane immediately following this separator.
		/// The trailing pane is to the right in horizontal layouts and below in vertical layouts.
		/// </summary>
		internal int TrailingPaneIndex { get; set; }

		/// <summary>
		/// Backing field for <see cref="UsePlatformTouchPath"/>, indicating whether platform touch handling is used for drag interactions.
		/// </summary>
		bool _usePlatformTouchPath = false;

		/// <summary>
		/// Indicates whether platform touch handling is used instead of the
		/// <see cref="PanGestureRecognizer"/> for drag interactions.
		/// </summary>
		internal bool UsePlatformTouchPath => _usePlatformTouchPath;

		/// <summary>
		/// Minimum separator size considered visible.
		/// Used for rendering optimizations while ensuring the resize handle remains drawable.
		/// </summary>
		internal const float MinDrawableSeparatorSize = 0.01f;

		/// <summary>
		/// Defines a fixed gap between adjacent separators when a pane is collapsed,
		/// preventing separator overlap and ensuring correct interaction.
		/// </summary>
		internal const double AdjacentSeparatorGapDiu = 1.0;

        #endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="SeparatorView"/> class.
		/// </summary>
		public SeparatorView()
		{
			DrawingOrder = DrawingOrder.AboveContent;

			Background = Colors.Transparent;

			InitializePlatformTouchPath();

			if (!UsePlatformTouchPath)
			{
				var panGesture = new PanGestureRecognizer();
				panGesture.PanUpdated += OnPanUpdated;
				GestureRecognizers.Add(panGesture);
			}

			var tapGesture = new TapGestureRecognizer();
			tapGesture.Tapped += OnTapped;
			GestureRecognizers.Add(tapGesture);

			this.AddTouchListener(this);
		}

		#endregion

        #region Internal methods

		/// <summary>
		/// Starts tracking a platform-driven drag operation without entering the dragging state.
		/// Actual dragging begins only after the movement threshold is exceeded.
		/// </summary>
        internal void BeginInternalDrag(Point pressPoint)
        {
            var splitter = FindContainingSplitter();
            if (splitter != null && !ShouldShowResizeHandle(splitter))
            {
                TakeStickyHoveredOwnership();
                return;
            }

            _isInternalDragging = true;
            _initialTouchPoint = pressPoint;
            _lastTouchPoint = pressPoint;

            TakeStickyHoveredOwnership();

        }

		/// <summary>
		/// Determines if the given position is within the hit zone.
		/// Used for collapse/expand icon detection and drag threshold.
		/// </summary>
		internal bool IsWithinHitZone(Point position)
		{
			return HitZoneBounds.Contains(position);
		}

		/// <summary>
		/// Returns the separator's body rectangle in splitter-local coordinates.
		/// Used to determine whether a pointer falls within the separator's visible draggable area.
		/// </summary>
		internal Rect GetSplitterLocalBodyRect(SfGridSplitter splitter)
		{
			if (splitter == null)
			{
				return new Rect(0, 0, 0, 0);
			}

			float sepSize = (float)splitter.SeparatorSize;
			float viewWidth = (float)_arrangedSize.Width;
			float viewHeight = (float)_arrangedSize.Height;

			double x = X;
			double y = Y;
			double width = splitter.Orientation == GridSplitterOrientation.Horizontal ? sepSize : viewWidth;
			double height = splitter.Orientation == GridSplitterOrientation.Horizontal ? viewHeight : sepSize;

			double offsetX = splitter.Orientation == GridSplitterOrientation.Horizontal ? (viewWidth - sepSize) / 2.0 : 0;
			double offsetY = splitter.Orientation == GridSplitterOrientation.Horizontal ? 0 : (viewHeight - sepSize) / 2.0;

			return new Rect(x + offsetX, y + offsetY, width, height);
		}

		/// <summary>
		/// Returns the separator's interactive hit zone in splitter-local coordinates.
		/// Used to determine whether a pointer falls within the separator or its button region.
		/// </summary>
		internal Rect GetSplitterLocalHitZoneRect(SfGridSplitter splitter)
		{
			if (splitter == null)
			{
				return new Rect(0, 0, 0, 0);
			}

			var body = GetSplitterLocalBodyRect(splitter);

			double hitZonePadding = splitter.Orientation == GridSplitterOrientation.Horizontal ? HorizontalHitZonePadding : VerticalHitZonePadding;
			double buttonReach = ButtonSeparatorGap + (ExpandCollapseButtonDiameter / 2f) + 4.0;
			double effectivePadding = Math.Max(hitZonePadding, buttonReach);

			if (splitter.Orientation == GridSplitterOrientation.Horizontal)
			{
				return new Rect( body.X - effectivePadding, body.Y, body.Width + (effectivePadding * 2), body.Height);
			}
			else
			{
				return new Rect( body.X, body.Y - effectivePadding, body.Width, body.Height + (effectivePadding * 2));
			}
		}

		/// <summary>
		/// Resolves the separator that should handle a pointer event at the specified splitter-local position.
		/// Handles overlapping separators by selecting the separator with the relevant visible button or hit target.
		/// </summary>
		internal bool IsPointOnOwnStripInSplitterCoords(SfGridSplitter splitter, Point splitterLocalPoint)
		{
			if (splitter == null)
			{
				return false;
			}

			var body = GetSplitterLocalBodyRect(splitter);
			if (body.Width <= 0 || body.Height <= 0)
				return false;

			double tolerance = 1.0;
			Rect expanded = new Rect( body.X - tolerance, body.Y - tolerance, body.Width + (tolerance * 2), body.Height + (tolerance * 2));

			return expanded.Contains(splitterLocalPoint);
		}

		internal static SeparatorView? ResolveOwningSeparator(SfGridSplitter splitter, SeparatorView current, Point splitterLocalPoint)
		{
			if (splitter == null || current == null)
			{
				return current;
			}

			var siblings = splitter.GetSeparators();
			if (siblings == null || siblings.Count <= 1)
			{
				return current;
			}

			if (current.IsPointOnOwnStripInSplitterCoords(splitter, splitterLocalPoint))
			{
				return current;
			}

			if (current.TryGetVisibleButtonAtSplitterPoint(splitter, splitterLocalPoint, out bool currentTappedLeading))
			{
				bool currentIsExpand = IsExpandActionForButton(splitter, current, currentTappedLeading);
				SeparatorView? expandSibling = null;
				for (int i = 0; i < siblings.Count; i++)
				{
					var sep = siblings[i];
					if (sep == null || sep == current)
					{
						continue;
					}

					var hitZone = sep.GetSplitterLocalHitZoneRect(splitter);
					if (!hitZone.Contains(splitterLocalPoint))
					{
						continue;
					}

					if (sep.TryGetVisibleButtonAtSplitterPoint(splitter, splitterLocalPoint, out bool sibTappedLeading)
						&& IsExpandActionForButton(splitter, sep, sibTappedLeading))
					{
						expandSibling = sep;
						break;
					}
				}

				if (expandSibling != null && !currentIsExpand)
				{
					return expandSibling;
				}

				return current;
			}

			SeparatorView? expandSiblingCandidate = null;
			SeparatorView? anySiblingCandidate = null;
			for (int i = siblings.Count - 1; i >= 0; i--)
			{
				var sep = siblings[i];
				if (sep == null || sep == current)
				{
					continue;
				}

				var hitZone = sep.GetSplitterLocalHitZoneRect(splitter);
				if (!hitZone.Contains(splitterLocalPoint))
				{
					continue;
				}

				if (sep.TryGetVisibleButtonAtSplitterPoint(splitter, splitterLocalPoint, out bool sibTappedLeading))
				{
					if (anySiblingCandidate == null)
					{
						anySiblingCandidate = sep;
					}

					if (IsExpandActionForButton(splitter, sep, sibTappedLeading))
					{
						expandSiblingCandidate = sep;
						break;
					}
				}
			}

			if (expandSiblingCandidate != null)
			{
				return expandSiblingCandidate;
			}
			if (anySiblingCandidate != null)
			{
				return anySiblingCandidate;
			}

			SeparatorView? bestBodyMatch = null;
			double bestDistance = double.PositiveInfinity;
			for (int i = 0; i < siblings.Count; i++)
			{
				var sep = siblings[i];
				if (sep == null)
				{
					continue;
				}

				var hitZone = sep.GetSplitterLocalHitZoneRect(splitter);
				if (!hitZone.Contains(splitterLocalPoint))
				{
					continue;
				}

				var body = sep.GetSplitterLocalBodyRect(splitter);
				if (body.Width <= 0 || body.Height <= 0)
				{
					continue;
				}

				double distance;
				if (splitter.Orientation == GridSplitterOrientation.Horizontal)
				{
					double bodyCenterX = body.X + (body.Width / 2.0);
					distance = Math.Abs(splitterLocalPoint.X - bodyCenterX);
				}
				else
				{
					double bodyCenterY = body.Y + (body.Height / 2.0);
					distance = Math.Abs(splitterLocalPoint.Y - bodyCenterY);
				}

				if (distance < bestDistance)
				{
					bestDistance = distance;
					bestBodyMatch = sep;
				}
			}

			if (bestBodyMatch != null && bestBodyMatch != current)
			{
				return bestBodyMatch;
			}
			return current;
		}

		/// <summary>
		/// Routes a pointer event to the separator that owns the pointer position.
		/// Returns whether the event should be handled by a different separator.
		/// </summary>
		internal static bool TryForwardToOwner(SfGridSplitter splitter, SeparatorView source, Point splitterLocalPoint, out SeparatorView? owner)
		{
			owner = null;
			if (splitter == null || source == null)
			{
				return false;
			}

			var resolved = ResolveOwningSeparator(splitter, source, splitterLocalPoint);
			if (resolved == null || resolved == source)
			{
				return false;
			}

			owner = resolved;
			return true;
		}

		/// <summary>
		/// Determines whether a visible expand/collapse button on this separator contains the specified point.
		/// Used to resolve pointer ownership when separators overlap.
		/// </summary>
		internal bool HasVisibleButtonAtSplitterPoint(SfGridSplitter splitter, Point splitterLocalPoint)
		{
			return TryGetVisibleButtonAtSplitterPoint(splitter, splitterLocalPoint, out _);
		}

		/// <summary>
		/// Determines which separator button contains the specified point and identifies whether it is the leading or trailing button.
		/// Used to resolve button actions when separators overlap.
		/// </summary>
		internal bool TryGetVisibleButtonAtSplitterPoint( SfGridSplitter splitter, Point splitterLocalPoint, out bool tappedLeading)
		{
			tappedLeading = false;
			if (splitter == null)
			{
				return false;
			}

			float buttonRadius = ExpandCollapseButtonDiameter / 2f;
			float effectiveRadius = buttonRadius + 4f;

			var body = GetSplitterLocalBodyRect(splitter);

			float buttonClearance = FloatingButtonEdgeOffset + ButtonSeparatorGap;

			bool rtl = IsRightToLeftLayout();

			float leadingX, trailingX, leadingY, trailingY;
			if (splitter.Orientation == GridSplitterOrientation.Horizontal)
			{
				float leftX = (float)(body.Left - buttonClearance);
				float rightX = (float)(body.Right + buttonClearance);
				if (rtl)
				{
					leadingX = rightX;
					trailingX = leftX;
				}
				else
				{
					leadingX = leftX;
					trailingX = rightX;
				}
				leadingY = (float)body.Center.Y;
				trailingY = (float)body.Center.Y;
			}
			else
			{
				leadingX = (float)body.Center.X;
				trailingX = (float)body.Center.X;
				leadingY = (float)(body.Top - buttonClearance);
				trailingY = (float)(body.Bottom + buttonClearance);
			}

			float px = (float)splitterLocalPoint.X;
			float py = (float)splitterLocalPoint.Y;

			if (IsLeadingIconVisible(splitter) && IsPointInsideCircle(new PointF(px, py), leadingX, leadingY, effectiveRadius))
			{
				tappedLeading = true;
				return true;
			}
			if (IsTrailingIconVisible(splitter) && IsPointInsideCircle(new PointF(px, py), trailingX, trailingY, effectiveRadius))
			{
				tappedLeading = false;
				return true;
			}
			return false;
		}

		/// <summary>
		/// Determines whether a button hit on this separator would perform an expand action.
		/// Used to prioritize expand actions when overlapping separators share the same hit area.
		/// </summary>
		internal bool HasExpandButtonAtSplitterPoint(SfGridSplitter splitter, Point splitterLocalPoint)
		{
			if (splitter == null)
			{
				return false;
			}

			if (!TryGetVisibleButtonAtSplitterPoint(splitter, splitterLocalPoint, out bool tappedLeading))
			{
				return false;
			}

			int leadingIndex = TrailingPaneIndex - 1;
			int trailingIndex = TrailingPaneIndex;

			if (leadingIndex < 0 || leadingIndex >= splitter.SplitterPanes.Count)
			{
				return false;
			}

			if (trailingIndex < 0 || trailingIndex >= splitter.SplitterPanes.Count)
			{
				return false;
			}

			bool leadingCollapsed = splitter.SplitterPanes[leadingIndex].IsCollapsed;
			bool trailingCollapsed = splitter.SplitterPanes[trailingIndex].IsCollapsed;

			// The button performs an expand action whenever either adjacent
			// pane is collapsed, regardless of the neighboring pane's
			// IsCollapsible value. Only the non-collapsed case is a collapse
			// action.
			return leadingCollapsed || trailingCollapsed;
		}

		/// <summary>
		/// Updates an active drag operation using the latest pointer position.
		/// Starts dragging after the movement threshold is exceeded and raises drag update callbacks while dragging.
		/// </summary>
        internal void UpdateInternalDrag(Point currentPoint)
        {
            if (!_isInternalDragging)
			{
                return;
			}

			_lastMoveEventTicks = DateTime.UtcNow.Ticks;

			var splitter = FindContainingSplitter();
			if (splitter == null)
			{
				return;
			}

			// Suppress any resize operation when the resize handle would not be shown
			// for the current separator state. Without this, dragging could still begin
			// on platforms that drive drags from the touch listener.
			if (!ShouldShowResizeHandle(splitter))
			{
				_isInternalDragging = false;
				if (_dragInProgress)
				{
					_dragInProgress = false;
					_onDragStop?.Invoke(TrailingPaneIndex);
				}

				if (IsDragging)
				{
					IsDragging = false;
					InvalidateDrawable();
				}

				return;
			}

			double currentTotal = splitter.Orientation == GridSplitterOrientation.Horizontal ? currentPoint.X - _initialTouchPoint.X : currentPoint.Y - _initialTouchPoint.Y;

			double delta = splitter.Orientation == GridSplitterOrientation.Horizontal ? currentPoint.X - _lastTouchPoint.X : currentPoint.Y - _lastTouchPoint.Y;
#if ANDROID || (IOS && !Maccalyst)
            bool isRTL = splitter.FlowDirection == FlowDirection.RightToLeft && splitter.Orientation == GridSplitterOrientation.Horizontal;

            if (isRTL)
            {
                currentTotal = -currentTotal;
                delta = -delta;
            }
#endif
			double absTotal = Math.Abs(currentTotal);

			if (!_dragInProgress && absTotal >= DragThresholdDiu)
			{
				_dragInProgress = true;
				if (!IsDragging)
				{
					IsDragging = true;
					// Invalidate drawable so the highlight color renders on the very next frame.
					InvalidateDrawable();
				}
				_onDragStart?.Invoke(TrailingPaneIndex);
				_lastResizingEventTicks = DateTime.UtcNow.Ticks;
				_lastTouchPoint = currentPoint;
				return;
			}

			if (_dragInProgress && delta != 0)
			{
				long currentTicks = DateTime.UtcNow.Ticks;
				long elapsedMs = (currentTicks - _lastResizingEventTicks) / TimeSpan.TicksPerMillisecond;
				if (elapsedMs >= ResizingThrottleMs)
				{
					_onDragDelta?.Invoke(TrailingPaneIndex, delta);
					_lastTouchPoint = currentPoint;
					_lastResizingEventTicks = currentTicks;
				}
			}
		}

		/// <summary>
		/// Ends a platform-driven drag operation and resets the associated drag state.
		/// Fires drag completion callbacks only when an actual drag occurred, not for simple taps.
		/// </summary>
		internal void EndInternalDrag(Point endPoint, bool cancelled = false)
		{
			if (!_isInternalDragging && !_dragInProgress && !IsDragging && !_isStickyHovered)
			{
				return;
			}

			_isInternalDragging = false;
			_lastTouchPoint = endPoint;

			bool wasDragging = _dragInProgress;
			if (wasDragging && !cancelled)
			{
				_onDragStop?.Invoke(TrailingPaneIndex);
			}

			if (wasDragging || cancelled)
			{
				_lastDragEndTicks = DateTime.UtcNow.Ticks;
				_isStickyHovered = false;
				if (s_stickyOwner == this)
				{
					s_stickyOwner = null;
				}

				_lastPointerPosition = null;
			}

			_dragInProgress = false;
			if (IsDragging)
			{
				IsDragging = false;
				InvalidateDrawable();
			}
		}

		/// <summary>
		/// Cancels an active drag operation and resets the associated visual and interaction state.
		/// Used when the resize-start event is cancelled, without raising a resize-stopped event.
		/// </summary>
		internal void AbortInternalDrag()
		{
			_isInternalDragging = false;
			_dragInProgress = false;
			if (IsDragging)
			{
				IsDragging = false;
				InvalidateDrawable();
			}

			_isStickyHovered = false;
			if (s_stickyOwner == this)
			{
				s_stickyOwner = null;
			}
			_lastPointerPosition = null;

			UpdateHoverState(false, null);
		}

		/// <summary>
		/// Unconditionally clears the hover visual state without consulting the
		/// pointer position. Used for hard cancellations (platform-initiated
		/// pointer capture loss, focus changes, app backgrounding) where the
		/// hover visual must be torn down even if the last known pointer
		/// position is still inside the bridge gap between the strip and the
		/// floating button. The bridge only preserves hover during normal
		/// Moved/Exited transitions, not on hard cancellation.
		/// </summary>
		internal void ForceClearHoverState()
		{
#if !WINDOWS
			CancelScheduledHoverHide();
			_isStickyHovered = false;
			if (s_stickyOwner == this)
			{
				s_stickyOwner = null;
			}

			_lastPointerPosition = null;
#endif
			if (IsHovered)
			{
				IsHovered = false;
				InvalidateDrawable();
				InvalidateMeasure();
			}

#if !WINDOWS			
			// The expand/collapse buttons are hosted by the splitter rather than
			// the separator, so refresh the parent overlay when hover is cleared.
			var splitter = FindContainingSplitter();
			splitter?.ResetSeparatorButtonInteraction(this);
			splitter?.InvalidateMeasure();
#endif
		}

		/// <summary>
		/// Updates the separator's hover state based on the current pointer position.
		/// Ensures hover behavior remains consistent for visible icons, overlapping separators, and active interaction regions.
		/// </summary>
		/// <param name="isHovered">
		/// Hint from the caller about whether the pointer is currently over
		/// the interactive area. Combined with the last known position to
		/// determine whether the buttons should remain visible.
		/// </param>
		/// <param name="pointerPosition">
		/// The most recent pointer position in separator-local
		/// coordinates. When <c>null</c> the method clears the cached
		/// last-known position and treats the pointer as "no longer
		/// inside the hover region" so the previous
		/// strip / bridge / button position is not used to keep the
		/// hover state alive after the cursor has actually left the
		/// control. The very first time a hover state is established,
		/// callers should pass the position they have so the strip
		/// hit-test works.
		/// </param>
		internal void UpdateHoverState(bool isHovered, Point? pointerPosition = null)
		{
			bool clearingCachedPosition = !pointerPosition.HasValue && !isHovered;

			// While a drag is in progress, the strip's hover visual
			// must be locked ON regardless of where the cursor is
			// reported to be. On MacCatalyst the touch listener
			// fires Moved events with positions that briefly
			// fall outside the strip during fast drag motion,
			// which would otherwise cause the strip's hover
			// background to flicker between the active and
			// resting colors on every drag tick. Forcing the hover
			// state to true here keeps the visual stable until the
			// drag completes, after which the Exited / Released
			// path returns the separator to the resting state.
			bool isDragging = IsDragging;
			bool effectiveIsHovered = isHovered || isDragging;

			if (pointerPosition.HasValue)
			{
				_lastPointerPosition = pointerPosition.Value;
			}
			else if (clearingCachedPosition && !isDragging)
			{
				// The caller is signaling "the pointer is no longer
				// tracked" (e.g. the strip view received an Exited
				// event and we want to return to the normal state).
				// Drop the stale last-known position so the
				// strip / bridge / button hit-tests below evaluate
				// against an absent pointer instead of the last
				// on-strip value, which would otherwise keep the
				// hover state alive even though the cursor has
				// actually left the control.
				_lastPointerPosition = null;
			}

			Point? resolved = pointerPosition ?? (_lastPointerPosition.HasValue ? _lastPointerPosition : (Point?)null);


			bool pointerInButtonRegion = resolved.HasValue && IsPointWithinButtonRegion(resolved.Value);
			bool pointerOnStrip = resolved.HasValue && IsPositionOnSeparatorStrip(resolved.Value);
			bool pointerInBridge = resolved.HasValue && IsPointInStripToButtonBridge(resolved.Value);


			bool alreadyInteractive = IsHovered || IsDragging || _isStickyHovered;
			bool pointerOverInteractiveRegion = pointerOnStrip
				|| (alreadyInteractive && (pointerInButtonRegion || pointerInBridge))
				|| (effectiveIsHovered && (pointerInButtonRegion || pointerOnStrip));

			bool shouldShowButtons = pointerOverInteractiveRegion || IsDragging || _isStickyHovered;

			if (IsHovered != shouldShowButtons)
			{
				IsHovered = shouldShowButtons;
				InvalidateDrawable();
			 InvalidateMeasure();
			}
			// Always invalidate the parent layout so the floating
			// expand/collapse buttons (which are siblings of the
			// internal grid, not children of the separator) are
			// repositioned on every hover transition. Without this,
			// the first hover after a layout pass may not show the
			// buttons until a second interaction triggers another
			// layout.
			FindContainingSplitter()?.InvalidateMeasure();
		}

		/// <summary>
		/// Gets a value indicating whether any expand or collapse icon is currently visible.
		/// Used to determine whether the separator should participate in hover-region detection.
		/// </summary>
		internal bool HasAnyVisibleIcon(SfGridSplitter splitter)
		{
			if (splitter == null)
			{
				return false;
			}

			return IsLeadingIconVisible(splitter) || IsTrailingIconVisible(splitter);
		}

		/// <summary>
		/// Returns <c>true</c> when the separator currently has at least one
		/// reachable expand / collapse button that the user could plausibly
		/// be moving toward as the cursor leaves the strip. Used by the
		/// platform <c>Exited</c> handlers to decide between a delayed hide
		/// (so the user can finish the strip-to-button transition) and an
		/// immediate clear (so the control returns to the normal state the
		/// moment the cursor leaves the strip with no button to transition
		/// to). Falls back to <c>true</c> when the splitter cannot be
		/// resolved, preserving the previous "always schedule a hide" behavior
		/// for any future call site that runs before the splitter is
		/// attached.
		/// </summary>
		internal bool HasAnyReachableButtonForTransition(SfGridSplitter? splitter)
		{
			if (splitter == null)
			{
				return true;
			}

			return HasAnyVisibleIcon(splitter);
		}

		/// <summary>
		/// Clears hover state from sibling separators that do not own the interaction at the specified point.
		/// Ensures only the active separator remains highlighted when separators overlap.
		/// </summary>
		internal void ClearStaleHoverOnSiblings(SfGridSplitter splitter, Point splitterLocalPoint)
		{
			if (splitter == null)
			{
				return;
			}

			var siblings = splitter.GetSeparators();
			if (siblings == null || siblings.Count <= 1)
			{
				return;
			}

			var owner = ResolveOwningSeparator(splitter, this, splitterLocalPoint);

			for (int i = 0; i < siblings.Count; i++)
			{
				var sep = siblings[i];
				if (sep == null)
				{
					continue;
				}

				if (sep == owner)
				{
					continue; // Owner is allowed to keep its hover state.
				}

				if (!sep.IsHovered)
				{
					continue; // Nothing to clear.
				}

				sep.IsHovered = false;
				sep.InvalidateDrawable();
				sep.InvalidateMeasure();
			}
		}

		/// <summary>
		/// Clears the sticky-hover state for this separator and restores its normal appearance.
		/// Updates <see cref="IsHovered"/> and refreshes the drawable when ownership changes.
		/// </summary>
		internal void ClearStickyHovered()
		{
			_isStickyHovered = false;

			_lastPointerPosition = null;
			if (IsHovered)
			{
				IsHovered = false;
				InvalidateDrawable();
				InvalidateMeasure();
			}

			FindContainingSplitter()?.InvalidateMeasure();
		}

		/// <summary>
		/// Sets up callback handlers for drag and resize events (called by SfGridSplitter).
		/// </summary>
		internal void SetDragCallbacks( Action<int>? dragStart, Action<int, double>? dragDelta, Action<int>? dragStop)
		{
			_onDragStart = dragStart;
			_onDragDelta = dragDelta;
			_onDragStop = dragStop;
		}

		/// <summary>
		/// Invalidates the drawable area to trigger a redraw.
		/// Used by SfGridSplitter when appearance properties change (e.g., template, background).
		/// </summary>
		internal new void InvalidateDrawable()
		{
			// Request the drawable layout to redraw
			base.InvalidateDrawable();
		}

		/// <summary>
		/// Returns the current resize-icon template view and clears the separator's reference to it.
		/// The caller is responsible for removing the returned view from its parent.
		/// </summary>
		internal View? GetAndClearTemplateView()
		{
			var view = _resizeIconTemplateView;
			_resizeIconTemplateView = null;
			return view;
		}

		/// <summary>
		/// Returns the splitter-local center point of the leading
		/// (left/top) expand/collapse button, plus a flag indicating
		/// whether the button should currently be visible. The
		/// <see cref="SfGridSplitter"/> overlay grid uses this to position
		/// the real <see cref="ExpandCollapseButton"/> view next to the
		/// separator strip.
		/// </summary>
		/// <param name="centerInSplitter">
		/// When the method returns, contains the splitter-local coordinates
		/// of the button's geometric center
		/// when the button is not visible / not arranged.
		/// </param>
		/// <param name="isVisible">
		/// When the method returns, contains <c>true</c> if the button
		/// should be visible (the action is meaningful for the current
		/// pane state) and the separator is arranged.
		/// </param>
		internal void GetLeadingButtonLayout(out Point centerInSplitter, out bool isVisible)
		{
			centerInSplitter = new Point(double.NaN, double.NaN);
			isVisible = false;

			var splitter = FindContainingSplitter();
			if (splitter == null)
			{
				return;
			}

			if (!IsLeadingIconVisible(splitter))
			{
				return;
			}

			if (!TryGetButtonHitTargets(out float leadingButtonX, out float leadingButtonY,
				out _, out _, out _))
			{
				return;
			}

			centerInSplitter = new Point(
				X + leadingButtonX,
				Y + leadingButtonY);
			isVisible = true;
		}

		/// <summary>
		/// Returns the splitter-local center point of the trailing
		/// (right/bottom) expand/collapse button, plus a flag indicating
		/// whether the button should currently be visible.
		/// </summary>
		/// <param name="centerInSplitter">
		/// When the method returns, contains the splitter-local coordinates
		/// of the button's geometric center
		/// when the button is not visible / not arranged.
		/// </param>
		/// <param name="isVisible">
		/// When the method returns, contains <c>true</c> if the button
		/// should be visible.
		/// </param>
		internal void GetTrailingButtonLayout(out Point centerInSplitter, out bool isVisible)
		{
			centerInSplitter = new Point(double.NaN, double.NaN);
			isVisible = false;

			var splitter = FindContainingSplitter();
			if (splitter == null)
			{
				return;
			}

			if (!IsTrailingIconVisible(splitter))
			{
				return;
			}

			if (!TryGetButtonHitTargets(out _, out _,
				out float trailingButtonX, out float trailingButtonY, out _))
			{
				return;
			}

			centerInSplitter = new Point(
				X + trailingButtonX,
				Y + trailingButtonY);
			isVisible = true;
		}

		/// <summary>
		/// Computes which chevron direction the leading (left/top) button
		/// should display. Returns <c>true</c> for the matching flag.
		/// </summary>
		/// <param name="isHorizontal">
		/// <c>true</c> when the splitter orientation is horizontal.
		/// </param>
		/// <param name="showLeft">When the method returns, contains <c>true</c> if a left chevron should be drawn.</param>
		/// <param name="showRight">When the method returns, contains <c>true</c> if a right chevron should be drawn.</param>
		/// <param name="showUp">When the method returns, contains <c>true</c> if an up chevron should be drawn.</param>
		/// <param name="showDown">When the method returns, contains <c>true</c> if a down chevron should be drawn.</param>
		internal void GetLeadingButtonChevron(bool isHorizontal, out bool showLeft, out bool showRight, out bool showUp, out bool showDown)
		{
			showLeft = false;
			showRight = false;
			showUp = false;
			showDown = false;

			var splitter = FindContainingSplitter();
			if (splitter == null)
			{
				return;
			}

			bool leadingPaneCollapsed = IsLeadingPaneCollapsed(splitter);
			bool trailingPaneCollapsed = IsTrailingPaneCollapsed(splitter);
			bool rtl = IsRightToLeftLayout();

			if (isHorizontal)
			{
				// Same rule as the legacy in-canvas renderer: the leading
				// button points right when expanding the leading pane,
				// left otherwise. RTL mirrors the direction.
				bool drawRight = leadingPaneCollapsed && !trailingPaneCollapsed;
				bool drawLeft = !drawRight;
				if (rtl)
				{
					(drawLeft, drawRight) = (drawRight, drawLeft);
				}
				showLeft = drawLeft;
				showRight = drawRight;
			}
			else
			{
				showUp = true;
			}
		}

		/// <summary>
		/// Computes which chevron direction the trailing (right/bottom)
		/// button should display.
		/// </summary>
		internal void GetTrailingButtonChevron(bool isHorizontal, out bool showLeft, out bool showRight, out bool showUp, out bool showDown)
		{
			showLeft = false;
			showRight = false;
			showUp = false;
			showDown = false;

			var splitter = FindContainingSplitter();
			if (splitter == null)
			{
				return;
			}

			bool leadingPaneCollapsed = IsLeadingPaneCollapsed(splitter);
			bool trailingPaneCollapsed = IsTrailingPaneCollapsed(splitter);
			bool rtl = IsRightToLeftLayout();

			if (isHorizontal)
			{
				bool drawLeft = trailingPaneCollapsed && !leadingPaneCollapsed;
				bool drawRight = !drawLeft;
				if (rtl)
				{
					(drawLeft, drawRight) = (drawRight, drawLeft);
				}
				showLeft = drawLeft;
				showRight = drawRight;
			}
			else
			{
				showDown = true;
			}
		}

		/// <summary>
		/// Returns accessibility metadata for the leading (left/top) button.
		/// </summary>
		internal void GetLeadingButtonSemantics(out string name, out string help)
		{
			var splitter = FindContainingSplitter();
			if (splitter == null)
			{
				name = string.Empty;
				help = string.Empty;
				return;
			}

			int leadingIndex = TrailingPaneIndex - 1;
			int trailingIndex = TrailingPaneIndex;
			if (leadingIndex < 0 || leadingIndex >= splitter.SplitterPanes.Count
				|| trailingIndex < 0 || trailingIndex >= splitter.SplitterPanes.Count)
			{
				name = string.Empty;
				help = string.Empty;
				return;
			}

			bool leadingCollapsed = splitter.SplitterPanes[leadingIndex].IsCollapsed;
			bool trailingCollapsed = splitter.SplitterPanes[trailingIndex].IsCollapsed;

			if (leadingCollapsed)
			{
				name = "Expand pane";
				help = "Expands the previously collapsed left (or top) pane.";
			}
			else if (trailingCollapsed)
			{
				name = "Expand pane";
				help = "Expands the previously collapsed right (or bottom) pane.";
			}
			else
			{
				name = "Collapse pane";
				help = "Collapses the left (or top) pane.";
			}
		}

		/// <summary>
		/// Returns accessibility metadata for the trailing (right/bottom) button.
		/// </summary>
		internal void GetTrailingButtonSemantics(out string name, out string help)
		{
			var splitter = FindContainingSplitter();
			if (splitter == null)
			{
				name = string.Empty;
				help = string.Empty;
				return;
			}

			int leadingIndex = TrailingPaneIndex - 1;
			int trailingIndex = TrailingPaneIndex;
			if (leadingIndex < 0 || leadingIndex >= splitter.SplitterPanes.Count
				|| trailingIndex < 0 || trailingIndex >= splitter.SplitterPanes.Count)
			{
				name = string.Empty;
				help = string.Empty;
				return;
			}

			bool leadingCollapsed = splitter.SplitterPanes[leadingIndex].IsCollapsed;
			bool trailingCollapsed = splitter.SplitterPanes[trailingIndex].IsCollapsed;

			if (trailingCollapsed)
			{
				name = "Expand pane";
				help = "Expands the previously collapsed right (or bottom) pane.";
			}
			else if (leadingCollapsed)
			{
				name = "Expand pane";
				help = "Expands the previously collapsed left (or top) pane.";
			}
			else
			{
				name = "Collapse pane";
				help = "Collapses the right (or bottom) pane.";
			}
		}

		/// <summary>
		/// Executes the action for the leading (left/top) expand/collapse
		/// button. Called by the splitter when the real button view is
		/// clicked.
		/// </summary>
		internal void OnLeadingButtonClicked()
		{
            // The button itself guards against double-fire from the
            // touch listener + TapGestureRecognizer paths (see
            // ExpandCollapseButton._clickRaised), so this method is
            // only invoked once per tap.
            var splitter = FindContainingSplitter();
            if (splitter != null)
            {
                ExecuteButtonAction(splitter, tappedLeading: true);
                ClearTapVisualState();
            }
        }

        /// <summary>
        /// Executes the action for the trailing (right/bottom) expand/collapse
        /// button.
        /// </summary>
        internal void OnTrailingButtonClicked()
        {
            var splitter = FindContainingSplitter();
            if (splitter != null)
            {
                ExecuteButtonAction(splitter, tappedLeading: false);
                ClearTapVisualState();
            }
        }

        /// <summary>
        /// Updates the visibility of the resize-icon template based on the resizable state
        /// of the panes adjacent to this separator.
        /// </summary>
        internal void UpdateResizeIconTemplateVisibility()
        {
			if (_resizeIconTemplateView == null)
			{
				return;
			}

			var splitter = FindContainingSplitter();
			if (splitter == null)
			{
				return;
			}

			bool shouldShow = ShouldShowResizeHandle(splitter);

			if (_resizeIconTemplateView.IsVisible != shouldShow)
			{
				_resizeIconTemplateView.IsVisible = shouldShow;
			}
		}

		/// <summary>
		/// Sets the template used to create the resize icon and returns the instantiated view.
		/// The returned view should be added to the parent <see cref="Grid"/> for proper layout.
		/// </summary>
		internal View? SetResizeIconTemplate(DataTemplate? template)
		{
			_resizeIconTemplate = template;
			return ApplyResizeIconTemplate();
		}

		/// <summary>
		/// Determines whether the resize handle can be shown for the current separator state.
		/// </summary>
		internal bool ShouldShowResizeHandle(SfGridSplitter splitter)
		{
			if (IsLeadingPaneCollapsed(splitter) || IsTrailingPaneCollapsed(splitter))
			{
				return false;
			}

			int leadingIndex = TrailingPaneIndex - 1;
			int trailingIndex = TrailingPaneIndex;
			if (leadingIndex >= 0 && leadingIndex < splitter.SplitterPanes.Count && !splitter.SplitterPanes[leadingIndex].IsResizable)
			{
				return false;
			}
			if (trailingIndex >= 0 && trailingIndex < splitter.SplitterPanes.Count && !splitter.SplitterPanes[trailingIndex].IsResizable)
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// Cancels an active resize operation and resets the separator's visual state.
		/// No resize completion events are raised when the operation is cancelled.
		/// </summary>
		internal void AbortPanDrag()
		{
			_dragInProgress = false;
			if (IsDragging)
			{
				IsDragging = false;
				InvalidateDrawable();
			}
		}

		/// <summary>
		/// Resets the separator's visual state after an icon tap.
		/// Clears hover-related state and hides the expand/collapse
		/// buttons. The <see cref="SfGridSplitter.ArrangeOverlay"/>
		/// keeps the buttons visible while the user is still over a
		/// button (its <c>IsPointerOver</c> / <c>IsPressed</c> take
		/// precedence over the separator's <see cref="IsHovered"/>),
		/// so unconditionally clearing here is safe and returns the
		/// control to the normal state the moment the user releases
		/// the click.
		/// </summary>
        internal void ClearTapVisualState()
        {
            _isStickyHovered = false;
            if (s_stickyOwner == this)
            {
                s_stickyOwner = null;
            }

            CancelScheduledHoverHide();
            _lastPointerPosition = null;
            if (IsHovered)
            {
                IsHovered = false;
                InvalidateDrawable();
                InvalidateMeasure();
            }
            else
            {
                // Even when IsHovered was already false, re-arrange
                // the overlay so the splitter picks up the cleared
                // sticky-hover state and the buttons reflect the
                // current pointer-over state.
                InvalidateMeasure();
                InvalidateDrawable();
            }
        }

		/// <summary>
		/// Draws the separator, resize handle, and floating expand/collapse buttons.
		/// Visuals adapt to the separator state and remain independent of separator size.
		/// </summary>
		internal void DrawSeparator(ICanvas canvas, RectF dirtyRect)
		{
			if (canvas == null)
			{
				return;
			}

			var splitter = FindContainingSplitter();
			if (splitter == null)
			{
				return;
			}

			var separatorBody = GetSeparatorBodyRect(dirtyRect, splitter);
			splitter.GetCurrentStateColors(
				IsIconInteractive(),
				out Brush stripBrush,
				out _,
				out _);
			canvas.FillColor = ResolveBrushColor(stripBrush, forHover: false);
			canvas.FillRectangle(separatorBody);

			if (ShouldShowResizeHandle(splitter) && _resizeIconTemplateView == null)
			{
				DrawResizeHandle(canvas, separatorBody.Center.X, separatorBody.Center.Y, splitter);
			}

			// The expand/collapse buttons are no longer drawn inside the separator.
			// They are real <see cref="ExpandCollapseButton"/> views hosted in the
			// splitter's overlay grid (see <c>SfGridSplitter.ArrangeOverlay</c>).
		}

		/// <summary>
		/// Determines whether the separator should use its interactive visual state.
		/// Used to keep separator visuals active during hover, drag, and button interactions.
		/// </summary>
		internal bool IsIconInteractive()
		{
			if (IsHovered || IsDragging || _isStickyHovered)
			{
				return true;
			}

			if (_lastPointerPosition.HasValue && IsPointWithinButtonRegion(_lastPointerPosition.Value))
			{
				return true;
			}

			return false;
		}

		/// <summary>
		/// Returns <c>true</c> when expand/collapse buttons should be shown.
		/// Buttons remain visible while the pointer is inside either button hit region,
		/// even if the pointer is no longer over the separator body itself.
		/// </summary>
		internal bool ShouldShowExpandCollapseButtons(Point? pointerPosition = null)
		{
			var resolvedPosition = pointerPosition ?? _lastPointerPosition;
			if (!resolvedPosition.HasValue)
			{
				return false;
			}

			return IsPointWithinButtonRegion(resolvedPosition.Value);
		}

		/// <summary>
		/// Determines whether a separator-local point lies within the visible separator strip.
		/// Excludes the extended margin area used for floating expand/collapse buttons.
		/// </summary>
		internal bool IsPositionOnSeparatorStrip(Point point)
		{
			var splitter = FindContainingSplitter();
			if (splitter == null)
			{
				return false;
			}

			float viewWidth = (float)_arrangedSize.Width;
			float viewHeight = (float)_arrangedSize.Height;
			if (viewWidth <= 0 || viewHeight <= 0)
			{
				return false;
			}

			float separatorSize = (float)splitter.SeparatorSize;
			RectF stripRect = splitter.Orientation == GridSplitterOrientation.Horizontal ? new RectF((viewWidth - separatorSize) / 2f, 0, separatorSize, viewHeight)
				: new RectF(0, (viewHeight - separatorSize) / 2f, viewWidth, separatorSize);

			return stripRect.Contains((float)point.X, (float)point.Y);
		}

		/// <summary>
		/// Determines whether separator buttons should be mirrored for right-to-left (RTL) layouts.
		/// Applies only to horizontal orientations.
		/// </summary>
		internal bool IsRightToLeftLayout()
		{
			var splitter = FindContainingSplitter();
			if (splitter == null)
			{
				return false;
			}

			if (splitter.Orientation != GridSplitterOrientation.Horizontal)
			{
				return false;
			}

			FlowDirection fd = splitter.FlowDirection;

			return fd == FlowDirection.RightToLeft;
		}

		/// <summary>
		/// Determines whether the specified point lies within this separator's hit zone.
		/// Used to avoid duplicate tap handling when separators overlap.
		/// </summary>
		internal bool IsTapInOwnHitZone(Point point, SfGridSplitter splitter)
		{
			if (splitter == null)
			{
				return false;
			}

			float viewWidth = (float)_arrangedSize.Width;
			float viewHeight = (float)_arrangedSize.Height;
			if (viewWidth <= 0 || viewHeight <= 0)
			{
				return false;
			}

			double hitZonePadding = splitter.Orientation == GridSplitterOrientation.Horizontal ? HorizontalHitZonePadding : VerticalHitZonePadding;

			double buttonReach = ButtonSeparatorGap + (ExpandCollapseButtonDiameter / 2f) + 4.0;
			double effectivePadding = Math.Max(hitZonePadding, buttonReach);

			float separatorSize = (float)splitter.SeparatorSize;
			RectF hitZoneRect = splitter.Orientation == GridSplitterOrientation.Horizontal ? new RectF((float)((viewWidth - separatorSize) / 2.0 - effectivePadding),
					0, (float)(separatorSize + (effectivePadding * 2)),viewHeight)
				: new RectF(0,(float)((viewHeight - separatorSize) / 2.0 - effectivePadding),viewWidth,(float)(separatorSize + (effectivePadding * 2)));

			return hitZoneRect.Contains((float)point.X, (float)point.Y);
		}

		/// <summary>
		/// Measures the separator size based on available space.
		/// </summary>
		internal Size MeasureSeparator(double widthConstraint, double heightConstraint)
		{
			return new Size(widthConstraint, heightConstraint);
		}

		internal void ArrangeSeparator(Rect bounds)
		{
			_arrangedSize = bounds.Size;
			var splitter = FindContainingSplitter();

			// The separator view is now sized exactly to the strip (no
			// negative margin that over-sized the view box to host buttons).
			// The expand/collapse buttons are real <see cref="ExpandCollapseButton"/>
			// views hosted in the splitter's overlay grid; they extend past
			// the strip visually but live outside the separator's hit zone.
			//
			// HitZoneBounds remains a logical rectangle used internally for
			// hover-state tracking. It extends past the strip a little to
			// keep the hover visual stable when the cursor moves between the
			// strip and the floating button, but it is NOT applied as a
			// Margin on the view, so the view itself no longer covers the
			// area outside the strip. The touch path uses this logical
			// rect only as a hint; the buttons are real Views with their
			// own hit-testing.
			double hitZonePadding = (splitter != null && splitter.Orientation == GridSplitterOrientation.Horizontal)
				? HorizontalHitZonePadding : VerticalHitZonePadding;

			if (splitter != null && splitter.Orientation == GridSplitterOrientation.Horizontal)
			{
				double hitZoneX = -hitZonePadding;
				double hitZoneWidth = bounds.Width + (hitZonePadding * 2);
				HitZoneBounds = new Rect(hitZoneX, bounds.Y, hitZoneWidth, bounds.Height);
			}
			else
			{
				double hitZoneY = -hitZonePadding;
				double hitZoneHeight = bounds.Height + (hitZonePadding * 2);
				HitZoneBounds = new Rect(bounds.X, hitZoneY, bounds.Width, hitZoneHeight);
			}
		}

		#endregion

		#region Private methods

		/// <summary>
		/// Walk the parent chain to find the containing <see cref="SfGridSplitter"/>.
		/// Useful when the separator is hosted inside intermediate layout containers (Grid, StackLayout, etc.).
		/// </summary>
		SfGridSplitter? FindContainingSplitter()
		{
			Element? parent = Parent;
			while (parent != null)
			{
				if (parent is SfGridSplitter splitter)
				{
					return splitter;
				}
				parent = parent.Parent;
			}
			return null;
		}

        /// <summary>
        /// Schedules a delayed hide of the separator's interactive
        /// state. Called when the pointer leaves the strip so the
        /// user has a brief window to move the pointer onto a
        /// floating button without the buttons disappearing.
        /// Schedules a delayed hide of the separator's interactive
        /// state. Called when the pointer leaves the strip so the
        /// user has a brief window to move the pointer onto a
        /// floating button without the buttons disappearing.
        /// </summary>
        void ScheduleHoverHide()
        {
            CancelScheduledHoverHide();
            _pendingHoverHide = true;

            var dispatcher = Dispatcher;
            if (dispatcher == null)
            {
                ClearHoverImmediate();
                return;
            }

            _hoverHideTimer = dispatcher.CreateTimer();
            _hoverHideTimer.Interval = TimeSpan.FromMilliseconds(HoverHideGraceMs);
            _hoverHideTimer.IsRepeating = false;
            _hoverHideTimer.Tick += OnHoverHideTimerTick;
            _hoverHideTimer.Start();
        }

        /// <summary>
        /// Cancels a pending scheduled hide. Called when the
        /// pointer enters a button (or re-enters the strip) so the
        /// buttons stay visible. Exposed as <c>internal</c> so the
        /// splitter can call it from its button-handling code.
        /// </summary>
        internal void CancelScheduledHoverHide()
        {
            if (_hoverHideTimer != null)
            {
                _hoverHideTimer.Stop();
                _hoverHideTimer.Tick -= OnHoverHideTimerTick;
                _hoverHideTimer = null;
            }
            _pendingHoverHide = false;
        }

        /// <summary>
        /// Returns <c>true</c> while a hover-hide is scheduled.
        /// </summary>
        internal bool HasPendingHoverHide => _pendingHoverHide;

        /// <summary>
        /// Immediately clears the hover state. Used when the
        /// timer has no dispatcher available, or when the splitter
        /// wants to force-hide the buttons (e.g. when the pointer
        /// leaves the entire splitter area).
        /// </summary>
        internal void ClearHoverImmediate()
        {
            if (!IsDragging && !_isStickyHovered)
            {
                UpdateHoverState(false, null);
                if (s_stickyOwner == this)
                {
                    s_stickyOwner = null;
                }
            }
        }

        /// <summary>
        /// Schedules a delayed hide of the hover state when a button exit
        /// is reported by the splitter's <see cref="ExpandCollapseButton.PointerOverChanged"/>
        /// hook. Uses the same grace window as <see cref="ScheduleHoverHide"/>
        /// so the strip and the button paths share a single hide timer
        /// decision. While the timer is pending, the cursor can still move
        /// back onto the strip (cancel) or onto the other button (cancel)
        /// without the buttons flickering.
        /// </summary>
        internal void ScheduleHoverHideFromButtonExit()
        {
            if (IsDragging || _isStickyHovered)
            {
                return;
            }

            ScheduleHoverHide();
        }

        /// <summary>
        /// True while a delayed hover-hide is pending.
        /// </summary>
        bool _pendingHoverHide;

        void OnHoverHideTimerTick(object? sender, EventArgs e)
        {
            _hoverHideTimer?.Stop();
            _hoverHideTimer = null;
            ClearHoverImmediate();
            _pendingHoverHide = false;
        }

		/// <summary>
		/// Sets this separator as the sticky-hover owner and clears any previous owner.
		/// Keeps the separator interactive after a press, including when adjacent panes are collapsed.
		/// </summary>
        void TakeStickyHoveredOwnership()
        {
            if (s_stickyOwner != null && s_stickyOwner != this)
            {
                s_stickyOwner.ClearStickyHovered();
            }
            s_stickyOwner = this;
            _isStickyHovered = true;
        }

		/// <summary>
		/// Returns <c>true</c> when the current call site is inside a small window
		/// after a separator drag ended. Used by the platform touch paths and the
		/// shared <c>OnTapped</c> handler to suppress the expand/collapse action
		/// when the user releases a drag over the floating button area.
		/// </summary>
		internal bool IsWithinDragEndSuppressionWindow()
		{
			if (_lastDragEndTicks == 0)
			{
				return false;
			}

			long elapsedMs = (DateTime.UtcNow.Ticks - _lastDragEndTicks) / TimeSpan.TicksPerMillisecond;
			if (elapsedMs > SpuriousCancelWindowMs)
			{
				_lastDragEndTicks = 0;
				return false;
			}

			return true;
		}

		/// <summary>
		/// Consumes the post-drag suppression timestamp so subsequent taps
		/// (after the suppression window expires) are not flagged as drag-release
		/// events. Called once after the window passes.
		/// </summary>
		void ClearDragEndSuppression()
		{
			_lastDragEndTicks = 0;
		}
		
		/// <summary>
		/// Determines whether the specified point is within this separator's active hover region.
		/// The hover region is based on the separator strip and any currently visible expand/collapse icons.
		/// Hover is reported when the pointer is over the strip even when no icons are visible
		/// (e.g. both panes have <c>IsCollapsible</c> set to <c>false</c>) so that the strip still
		/// transitions to its hover visual state.
		/// </summary>
		bool IsPointInHoverRegion(Point? pointerPosition)
		{
			if (!pointerPosition.HasValue)
			{
				return false;
			}

			var splitter = FindContainingSplitter();
			if (splitter == null)
			{
				return false;
			}


			if (IsPositionOnSeparatorStrip(pointerPosition.Value))
			{
				return true;
			}


			if ((IsHovered || IsDragging || _isStickyHovered)
				&& (IsPointWithinButtonRegion(pointerPosition.Value)
					|| IsPointInStripToButtonBridge(pointerPosition.Value)))
			{
				return true;
			}

			return false;
		}

		/// <summary>
		/// Returns <c>true</c> when the specified point lies in the small gap
		/// between the visible separator strip and a visible floating
		/// expand/collapse button. The strip and the button hit zones do not
		/// cover this area, so without this bridge the hover state would be
		/// cleared as soon as the cursor crossed the strip edge while moving
		/// toward the button.
		/// </summary>
		bool IsPointInStripToButtonBridge(Point point)
		{
			var splitter = FindContainingSplitter();
			if (splitter == null)
			{
				return false;
			}

			float viewWidth = (float)_arrangedSize.Width;
			float viewHeight = (float)_arrangedSize.Height;
			if (viewWidth <= 0 || viewHeight <= 0)
			{
				return false;
			}

			if (!TryGetButtonHitTargets(out float leadingButtonX, out float leadingButtonY,
				out float trailingButtonX, out float trailingButtonY, out float buttonRadius))
			{
				return false;
			}

			float separatorSize = (float)splitter.SeparatorSize;
			if (separatorSize <= 0)
			{
				return false;
			}

			float buttonEdgeOffset = buttonRadius + 4f;
			bool isHorizontal = splitter.Orientation == GridSplitterOrientation.Horizontal;

			float px = (float)point.X;
			float py = (float)point.Y;

			if (isHorizontal)
			{
				float stripLeft = (viewWidth - separatorSize) / 2f;
				float stripRight = stripLeft + separatorSize;

				// Bridge on the left side of the strip when the leading button is visible.
				if (IsLeadingIconVisible(splitter) && px >= stripLeft - buttonEdgeOffset && px < stripLeft)
				{
					return true;
				}

				// Bridge on the right side of the strip when the trailing button is visible.
				if (IsTrailingIconVisible(splitter) && px > stripRight && px <= stripRight + buttonEdgeOffset)
				{
					return true;
				}
			}
			else
			{
				float stripTop = (viewHeight - separatorSize) / 2f;
				float stripBottom = stripTop + separatorSize;

				// Bridge on the top of the strip when the leading (top) button is visible.
				if (IsLeadingIconVisible(splitter) && py >= stripTop - buttonEdgeOffset && py < stripTop)
				{
					return true;
				}

				// Bridge on the bottom of the strip when the trailing (bottom) button is visible.
				if (IsTrailingIconVisible(splitter) && py > stripBottom && py <= stripBottom + buttonEdgeOffset)
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Recreates the resize-icon template view from the current template.
		/// Returns the instantiated view so it can be added to the parent <see cref="Grid"/>.
		/// </summary>
		View? ApplyResizeIconTemplate()
		{
			if (_resizeIconTemplateView != null)
			{
				_resizeIconTemplateView = null;
			}

			if (_resizeIconTemplate == null)
			{
				InvalidateDrawable();
				return null;
			}

			if (_resizeIconTemplate.CreateContent() is not View templateView)
			{
				InvalidateDrawable();
				return null;
			}

			templateView.InputTransparent = true;
			SetInheritedBindingContext(templateView, BindingContext);
			templateView.HorizontalOptions = LayoutOptions.Center;
			templateView.VerticalOptions = LayoutOptions.Center;
			_resizeIconTemplateView = templateView;

			InvalidateMeasure();
			InvalidateDrawable();

			return templateView;
		}

		/// <summary>
		/// Returns <c>true</c> when the pane immediately to the LEFT of this separator
		/// is collapsed (i.e. has zero width in the layout). Used to suppress the
		/// leading (left/top) expand/collapse icon and the resize handle when there
		/// is nothing to interact with on the leading side.
		/// </summary>
		bool IsLeadingPaneCollapsed(SfGridSplitter splitter)
		{
			int leadingIndex = TrailingPaneIndex - 1;
			if (leadingIndex < 0 || leadingIndex >= splitter.SplitterPanes.Count)
			{
				return false;
			}

			return splitter.SplitterPanes[leadingIndex].IsCollapsed;
		}

		/// <summary>
		/// Returns <c>true</c> when the pane immediately to the RIGHT of this separator
		/// is collapsed (i.e. has zero width in the layout). Used to suppress the
		/// trailing (right/bottom) expand/collapse icon and the resize handle when there
		/// is nothing to interact with on the trailing side.
		/// </summary>
		bool IsTrailingPaneCollapsed(SfGridSplitter splitter)
		{
			int trailingIndex = TrailingPaneIndex;
			if (trailingIndex < 0 || trailingIndex >= splitter.SplitterPanes.Count)
			{
				return false;
			}

			return splitter.SplitterPanes[trailingIndex].IsCollapsed;
		}

		/// <summary>
		/// Determines whether both panes adjacent to the separator can be resized.
		/// Used to enable resize interactions and resize handle visibility.
		/// </summary>
		bool AreAdjacentPanesResizable(SfGridSplitter splitter)
		{
			int leadingIndex = TrailingPaneIndex - 1;
			int trailingIndex = TrailingPaneIndex;

			if (leadingIndex >= 0 && leadingIndex < splitter.SplitterPanes.Count && !splitter.SplitterPanes[leadingIndex].IsResizable)
			{
				return false;
			}

			if (trailingIndex >= 0 && trailingIndex < splitter.SplitterPanes.Count && !splitter.SplitterPanes[trailingIndex].IsResizable)
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// Handles pan gesture updates for resize operations.
		/// Calculates drag movement and raises resize callbacks when applicable.
		/// </summary>
		void OnPanUpdated(object? sender, PanUpdatedEventArgs e)
		{
			if (_isInternalDragging)
			{
				return;
			}

			var splitter = FindContainingSplitter();
			if (splitter == null)
			{
				return;
			}


			if (IsLeadingPaneCollapsed(splitter) || IsTrailingPaneCollapsed(splitter) || !AreAdjacentPanesResizable(splitter))
			{
				if (_dragInProgress)
				{
					_dragInProgress = false;
					_onDragStop?.Invoke(TrailingPaneIndex);
				}

				if (IsDragging)
				{
					IsDragging = false;
					InvalidateDrawable();
				}

				return;
			}

			// Mirror the visual gating of ShouldShowResizeHandle so a resize never begins
			// when the resize handle is suppressed for the current separator state.
			if (!ShouldShowResizeHandle(splitter))
			{
				if (_dragInProgress)
				{
					_dragInProgress = false;
					_onDragStop?.Invoke(TrailingPaneIndex);
				}

				if (IsDragging)
				{
					IsDragging = false;
					InvalidateDrawable();
				}

				return;
			}

			switch (e.StatusType)
			{
				case GestureStatus.Started:
					_lastPanTotal = 0;
					break;

				case GestureStatus.Running:
					double currentTotal = splitter.Orientation == GridSplitterOrientation.Horizontal ? e.TotalX : e.TotalY;
					double delta = currentTotal - _lastPanTotal;
					double absTotal = Math.Abs(currentTotal);

					if (!_dragInProgress && absTotal >= DragThresholdDiu)
					{
						_dragInProgress = true;
						if (!IsDragging)
						{
							IsDragging = true;
							InvalidateDrawable();
						}
						_onDragStart?.Invoke(TrailingPaneIndex);
						_lastResizingEventTicks = DateTime.UtcNow.Ticks;
						_lastPanTotal = currentTotal;
					}

					if (_dragInProgress)
					{
						long currentTicks = DateTime.UtcNow.Ticks;
						long elapsedMs = (currentTicks - _lastResizingEventTicks) / TimeSpan.TicksPerMillisecond;

						if (elapsedMs >= ResizingThrottleMs)
						{
							if (delta != 0)
							{
								_onDragDelta?.Invoke(TrailingPaneIndex, delta);
								_lastPanTotal = currentTotal;
							}
							_lastResizingEventTicks = currentTicks;
						}
					}
					break;

				case GestureStatus.Completed:
				case GestureStatus.Canceled:
					if (_dragInProgress)
					{
						// Capture the drag-end timestamp before clearing state so that
						// OnTapped can ignore the tap that follows from the
						// TapGestureRecognizer right after the pan completes.
						_lastDragEndTicks = DateTime.UtcNow.Ticks;
						_dragInProgress = false;
						_onDragStop?.Invoke(TrailingPaneIndex);
					}

					if (IsDragging)
					{
						IsDragging = false;
						InvalidateDrawable();
					}
					break;
			}
		}

		/// <summary>
		/// Handles expand/collapse button taps and executes the corresponding pane action.
		/// Performs button hit-testing and ignores taps outside the visible button regions.
		/// </summary>
		void OnTapped(object? sender, TappedEventArgs e)
		{
			var splitter = FindContainingSplitter();
			if (splitter == null)
			{
				return;
			}

			if (TrailingPaneIndex < 0 || TrailingPaneIndex >= splitter.SplitterPanes.Count)
			{
				return;
			}

			// Suppress taps that arrive immediately after a separator drag completes.
			// TapGestureRecognizer and PanGestureRecognizer both fire on Windows/AMC
			// when the pointer releases, so the tap event for the release-over-button
			// case would otherwise trigger the expand/collapse action unintentionally.
			if (IsWithinDragEndSuppressionWindow())
			{
				return;
			}
			ClearDragEndSuppression();

			var hitPosition = e?.GetPosition(this) ?? _lastPointerPosition;
			if (!hitPosition.HasValue)
			{
				return;
			}

#if ANDROID || IOS || MACCATALYST

			if (s_directTapHandled)
			{
				s_directTapHandled = false;
				return;
			}
#endif

#if ANDROID || IOS || MACCATALYST
			if (!_isStickyHovered && !IsPositionOnSeparatorStrip(hitPosition.Value) && !IsPointWithinAnySeparatorButtonRegion(hitPosition.Value))
			{
				return;
			}

			if (!IsTapInOwnHitZone(hitPosition.Value, splitter))
			{
				return;
			}
#endif

			if (!TryGetButtonHitTargets(out float leadingButtonX, out float leadingButtonY, out float trailingButtonX, out float trailingButtonY, out float buttonRadius))
			{
				return;
			}

			var tapPoint = new PointF((float)hitPosition.Value.X, (float)hitPosition.Value.Y);

			float hitRadius = buttonRadius + 6f;

			bool leadingVisible = IsLeadingIconVisible(splitter);
			bool trailingVisible = IsTrailingIconVisible(splitter);

			bool tappedLeading = leadingVisible && IsPointInsideCircle(tapPoint, leadingButtonX, leadingButtonY, hitRadius);
			bool tappedTrailing = !tappedLeading && trailingVisible && IsPointInsideCircle(tapPoint, trailingButtonX, trailingButtonY, hitRadius);

			if (!tappedLeading && !tappedTrailing && IsPositionOnSeparatorStrip(hitPosition.Value))
			{
				bool horizontal = splitter.Orientation == GridSplitterOrientation.Horizontal;
				double separatorScreenX = horizontal ? X : 0;
				double separatorScreenY = horizontal ? 0 : Y;
				double leadingButtonSplitterX = separatorScreenX + leadingButtonX;
				double trailingButtonSplitterX = separatorScreenX + trailingButtonX;
				double leadingButtonSplitterY = separatorScreenY + leadingButtonY;
				double trailingButtonSplitterY = separatorScreenY + trailingButtonY;

				double splitterWidth = splitter.Width;
				double splitterHeight = splitter.Height;

				bool leadingOffSplitter = horizontal ? leadingButtonSplitterX <= 0 : leadingButtonSplitterY <= 0;
				bool trailingOffSplitter = horizontal ? trailingButtonSplitterX >= splitterWidth : trailingButtonSplitterY >= splitterHeight;

				if (leadingVisible && leadingOffSplitter)
				{
					tappedLeading = true;
				}
				else if (trailingVisible && trailingOffSplitter)
				{
					tappedTrailing = true;
				}
			}

			if (!tappedLeading && !tappedTrailing)
			{
				if (TryRouteTapToOverlappingSibling(splitter, hitPosition.Value, tapPoint, hitRadius))
				{
#if ANDROID 
					s_tapGestureResolved = true;
#endif
					ClearTapVisualState();
#if ANDROID || IOS || MACCATALYST
					s_directTapHandled = false;
#endif
					return;
				}

				// Tapped outside any visible button - leave it to the separator to
				// handle the resize gesture and do not toggle any pane.
				return;
			}

#if ANDROID || IOS || MACCATALYST

			if (!IsExpandActionForButton(splitter, this, tappedLeading))
			{
				var splitterLocalPoint = e!.GetPosition(splitter);
				if (splitterLocalPoint.HasValue)
				{
					var point = splitterLocalPoint.Value;
#if ANDROID || (IOS && !Maccalyst)

					if (splitter.FlowDirection == FlowDirection.RightToLeft && splitter.Orientation == GridSplitterOrientation.Horizontal)
					{
						point = new Point(splitter.Width - point.X,point.Y);
					}
#endif
					if (TryForwardToOwner(splitter, this, point, out var expandOwner))
					{

						if (expandOwner != null && expandOwner != this && expandOwner.TryGetVisibleButtonAtSplitterPoint(splitter, splitterLocalPoint.Value, out bool ownerTappedLeading))
						{

							expandOwner.ExecuteButtonAction(splitter, ownerTappedLeading);
#if ANDROID 
							s_tapGestureResolved = true;
#endif
							expandOwner.ClearTapVisualState();
							ClearTapVisualState();
							s_directTapHandled = false;
							return;
						}
					}
				}
			}
#endif

				ExecuteButtonAction(splitter, tappedLeading);
#if ANDROID 
				s_tapGestureResolved = true;
#endif
				ClearTapVisualState();

#if ANDROID || IOS || MACCATALYST
				s_directTapHandled = false;
#endif
			}
		
		/// <summary>
		/// Executes the expand or collapse action for the tapped button.
		/// Shared by all button-tap paths to ensure consistent behavior and event handling.
		/// </summary>
		/// <param name="splitter">The owning <see cref="SfGridSplitter"/>.</param>
		/// <param name="tappedLeading">
		/// <c>true</c> if the leading button was tapped; otherwise, <c>false</c>.
		/// </param>
        void ExecuteButtonAction(SfGridSplitter splitter, bool tappedLeading)
		{
			int leadingIndex = TrailingPaneIndex - 1;
			int trailingIndex = TrailingPaneIndex;

			if (leadingIndex < 0 || leadingIndex >= splitter.SplitterPanes.Count
				|| trailingIndex < 0 || trailingIndex >= splitter.SplitterPanes.Count)
			{
				return;
			}

			bool leadingCollapsed = splitter.SplitterPanes[leadingIndex].IsCollapsed;
			bool trailingCollapsed = splitter.SplitterPanes[trailingIndex].IsCollapsed;

			// The button's action is determined entirely by the collapse state of
			// the two adjacent panes — not by the neighboring pane's
			// IsCollapsible value. The button is only visible when the action
			// matches a pane that the user is actually allowed to expand or
			// collapse, so this method simply performs the appropriate action.
			if (tappedLeading)
			{
				if (leadingCollapsed)
				{
					// Restore the leading pane that was collapsed.
					if (splitter.SplitterPanes[leadingIndex].IsCollapsible)
					{
						_ = ExpandPaneAsync(splitter, leadingIndex);
					}
				}
				else if (trailingCollapsed)
				{
					// Restore the trailing pane that was collapsed. The leading
					// pane's IsCollapsible is irrelevant here.
					if (splitter.SplitterPanes[trailingIndex].IsCollapsible)
					{
						_ = ExpandPaneAsync(splitter, trailingIndex);
					}
				}
				else
				{
					// Both panes are visible: collapse the leading pane.
					if (splitter.SplitterPanes[leadingIndex].IsCollapsible)
					{
						_ = CollapsePaneAsync(splitter, leadingIndex);
					}
				}
			}
			else
			{
				if (trailingCollapsed)
				{
					// Restore the trailing pane that was collapsed.
					if (splitter.SplitterPanes[trailingIndex].IsCollapsible)
					{
						_ = ExpandPaneAsync(splitter, trailingIndex);
					}
				}
				else if (leadingCollapsed)
				{
					// Restore the leading pane that was collapsed. The trailing
					// pane's IsCollapsible is irrelevant here.
					if (splitter.SplitterPanes[leadingIndex].IsCollapsible)
					{
						_ = ExpandPaneAsync(splitter, leadingIndex);
					}
				}
				else
				{
					// Both panes are visible: collapse the trailing pane.
					if (splitter.SplitterPanes[trailingIndex].IsCollapsible)
					{
						_ = CollapsePaneAsync(splitter, trailingIndex);
					}
				}
			}
		}

		/// <summary>
		/// Routes a tap to the separator that owns the visible button at the tapped position.
		/// </summary>
		bool TryRouteTapToOverlappingSibling(SfGridSplitter splitter, Point hitPosition, PointF tapPoint, float hitRadius)
		{
			var siblings = splitter.GetSeparators();
			if (siblings == null)
			{
				return false;
			}

			bool horizontal = splitter.Orientation == GridSplitterOrientation.Horizontal;
			double thisSplitterX = horizontal ? X : 0;
			double thisSplitterY = horizontal ? 0 : Y;
			float tapSplitterX = (float)(thisSplitterX + tapPoint.X);
			float tapSplitterY = (float)(thisSplitterY + tapPoint.Y);

			foreach (var sibling in siblings)
			{
				if (sibling == null || sibling == this)
				{
					continue;
				}

				if (!sibling.TryGetButtonHitTargets(out float sibLeadingX, out float sibLeadingY, out float sibTrailingX, out float sibTrailingY, out float sibRadius))
				{
					continue;
				}

				double sibSplitterX = horizontal ? sibling.X : 0;
				double sibSplitterY = horizontal ? 0 : sibling.Y;
				float sibLeadingSplitterX = (float)(sibSplitterX + sibLeadingX);
				float sibLeadingSplitterY = (float)(sibSplitterY + sibLeadingY);
				float sibTrailingSplitterX = (float)(sibSplitterX + sibTrailingX);
				float sibTrailingSplitterY = (float)(sibSplitterY + sibTrailingY);

				float sibHitRadius = sibRadius + 6f;

				bool sibLeadingVisible = sibling.IsLeadingIconVisible(splitter);
				bool sibTrailingVisible = sibling.IsTrailingIconVisible(splitter);

				if (sibLeadingVisible && IsPointInsideCircle(new PointF(tapSplitterX, tapSplitterY), sibLeadingSplitterX, sibLeadingSplitterY, sibHitRadius))
				{
					#if ANDROID || (IOS && !MACCATALYST)
					if (sibling != this && !sibling._isStickyHovered)
					{
						sibling.IsHovered = true;
						sibling._isStickyHovered = true;

						sibling.InvalidateDrawable();
						sibling.InvalidateMeasure();

						return true;
					}
					#endif
					sibling.ExecuteButtonAction(splitter, tappedLeading: true);
					sibling.ClearTapVisualState();
					return true;
				}

				if (sibTrailingVisible && IsPointInsideCircle( new PointF(tapSplitterX, tapSplitterY),sibTrailingSplitterX, sibTrailingSplitterY, sibHitRadius))
				{
					sibling.ExecuteButtonAction(splitter, tappedLeading: false);

					sibling.ClearTapVisualState();
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Determines whether the leading expand/collapse icon should be visible.
		/// Visibility is driven by the action the button can perform on the adjacent
		/// panes (expand the collapsed pane, or collapse the leading pane). The
		/// button is hidden only when there is no valid action — never because of
		/// the neighboring pane's <see cref="SplitterPane.IsCollapsible"/> value.
		/// </summary>
		bool IsLeadingIconVisible(SfGridSplitter splitter)
		{
			int leadingIndex = TrailingPaneIndex - 1;
			int trailingIndex = TrailingPaneIndex;

			if (leadingIndex < 0 || leadingIndex >= splitter.SplitterPanes.Count)
			{
				return false;
			}

			if (trailingIndex < 0 || trailingIndex >= splitter.SplitterPanes.Count)
			{
				return false;
			}

			bool leadingCollapsible = splitter.SplitterPanes[leadingIndex].IsCollapsible;
			bool trailingCollapsible = splitter.SplitterPanes[trailingIndex].IsCollapsible;
			bool leadingCollapsed = splitter.SplitterPanes[leadingIndex].IsCollapsed;
			bool trailingCollapsed = splitter.SplitterPanes[trailingIndex].IsCollapsed;

			// Nothing to do when both adjacent panes are collapsed.
			if (leadingCollapsed && trailingCollapsed)
			{
				return false;
			}

			// The leading (left/top) button represents an action on the LEADING
			// pane. When the leading pane is already collapsed there is no
			// leading-side surface to interact with, so the leading button must
			// not be rendered. The action of restoring the leading pane is
			// surfaced through the trailing (right/bottom) button instead.
			if (leadingCollapsed)
			{
				return false;
			}

			// The leading button can expand the trailing pane if the trailing pane
			// is collapsed and is collapsible.
			if (trailingCollapsed)
			{
				return trailingCollapsible;
			}

			// Neither pane is collapsed, so the button can only collapse the
			// leading pane. Show it only when the leading pane is collapsible.
			return leadingCollapsible;
		}

		/// <summary>
		/// Determines whether the trailing expand/collapse icon should be visible.
		/// Visibility is driven by the action the button can perform on the adjacent
		/// panes (expand the collapsed leading pane, or collapse the trailing pane).
		/// The button is hidden only when there is no valid action or when the
		/// trailing pane is collapsed (the trailing button would float over the
		/// collapsed pane with nothing to anchor to). The button is never hidden
		/// because of the neighboring pane's <see cref="SplitterPane.IsCollapsible"/>
		/// value.
		/// </summary>
		bool IsTrailingIconVisible(SfGridSplitter splitter)
		{
			int leadingIndex = TrailingPaneIndex - 1;
			int trailingIndex = TrailingPaneIndex;

			if (leadingIndex < 0 || leadingIndex >= splitter.SplitterPanes.Count)
			{
				return false;
			}
			if (trailingIndex < 0 || trailingIndex >= splitter.SplitterPanes.Count)
			{
				return false;
			}

			bool leadingCollapsible = splitter.SplitterPanes[leadingIndex].IsCollapsible;
			bool trailingCollapsible = splitter.SplitterPanes[trailingIndex].IsCollapsible;
			bool leadingCollapsed = splitter.SplitterPanes[leadingIndex].IsCollapsed;
			bool trailingCollapsed = splitter.SplitterPanes[trailingIndex].IsCollapsed;

			// Nothing to do when both adjacent panes are collapsed.
			if (leadingCollapsed && trailingCollapsed)
			{
				return false;
			}

			// The trailing (right/bottom) button represents an action on the
			// TRAILING pane. When the trailing pane is already collapsed there
			// is no trailing-side surface to interact with, so the trailing
			// button must not be rendered. The action of restoring the trailing
			// pane is surfaced through the leading (left/top) button instead.
			if (trailingCollapsed)
			{
				return false;
			}

			// The trailing button can expand the leading pane if the leading pane
			// is collapsed and is collapsible.
			if (leadingCollapsed)
			{
				return leadingCollapsible;
			}

			// Neither pane is collapsed, so the button can only collapse the
			// trailing pane. Show it only when the trailing pane is collapsible.
			return trailingCollapsible;
		}

		/// <summary>
		/// Collapses a pane on the UI thread with exception handling.
		/// Used for expand/collapse button actions.
		/// </summary>
		async Task CollapsePaneAsync(SfGridSplitter splitter, int targetIndex)
		{
			await MainThread.InvokeOnMainThreadAsync(async () =>
			{
				await splitter.CollapsePane(targetIndex);
			});
		}

		/// <summary>
		/// Expands a pane on the UI thread and safely handles exceptions.
		/// Used to restore or expand adjacent collapsed panes.
		/// </summary>
		/// <param name="splitter"></param>
		/// <param name="targetIndex"></param>
		/// <returns></returns>
		async Task ExpandPaneAsync(SfGridSplitter splitter, int targetIndex)
		{
			await MainThread.InvokeOnMainThreadAsync(async () =>
			{
				await splitter.ExpandPane(targetIndex);

			});
		}

		/// <summary>
		/// Computes the actual separator body rectangle inside the expanded view bounds.
		/// The separator may have negative margins so the view bounds extend beyond the
		/// visible separator line to accommodate floating buttons.
		/// </summary>
		RectF GetSeparatorBodyRect(RectF dirtyRect, SfGridSplitter splitter)
		{
			if (splitter.Orientation == GridSplitterOrientation.Horizontal)
			{
				float halfWidth = (float)(splitter.SeparatorSize / 2.0);
				float left = dirtyRect.Center.X - halfWidth;
				float right = dirtyRect.Center.X + halfWidth;
				return new RectF(left, dirtyRect.Top, right - left, dirtyRect.Height);
			}
			else
			{
				float halfHeight = (float)(splitter.SeparatorSize / 2.0);
				float top = dirtyRect.Center.Y - halfHeight;
				float bottom = dirtyRect.Center.Y + halfHeight;
				return new RectF(dirtyRect.Left, top, dirtyRect.Width, bottom - top);
			}
		}

		/// <summary>
		/// Draws the resize handle on the separator.
		/// Appearance updates based on the handle's interaction state.
		/// </summary>
		void DrawResizeHandle(ICanvas canvas, float centerX, float centerY, SfGridSplitter splitter)
		{
			splitter.GetCurrentStateColors(
				IsIconInteractive(),
				out _,
				out Color barColor,
				out _);

			const float iconCross = 14f;        // 14 DIU across the icon (perpendicular to the bar)
			const float barThickness = 2f;      // 1 DIU bar thickness
			const float barSpacing = 1f;        // 1 DIU gap between the two bars
			float barOffset = (barThickness + barSpacing) / 2f;  // = 1 DIU from centre to each bar

			canvas.FillColor = barColor;
			if (splitter.Orientation == GridSplitterOrientation.Horizontal)
			{
				canvas.FillRectangle(centerX - barOffset - barThickness / 2f, centerY - iconCross / 2f, barThickness, iconCross);
				canvas.FillRectangle(centerX + barOffset - barThickness / 2f, centerY - iconCross / 2f, barThickness, iconCross);
			}
			else
			{
				canvas.FillRectangle(centerX - iconCross / 2f, centerY - barOffset - barThickness / 2f, iconCross, barThickness);
				canvas.FillRectangle(centerX - iconCross / 2f, centerY + barOffset - barThickness / 2f, iconCross, barThickness);
			}
		}

		/// <summary>
		/// Draws floating expand/collapse buttons adjacent to the separator.
		/// Buttons are shown during hover or drag and indicate the available pane action.
		/// </summary>
		void DrawFloatingExpandCollapseButtons(ICanvas canvas, RectF separatorBody, SfGridSplitter splitter)
		{
			float buttonDiameter = ExpandCollapseButtonDiameter;
			float buttonRadius = buttonDiameter / 2f;

			float buttonClearance = FloatingButtonEdgeOffset + ButtonSeparatorGap;

			bool isHorizontal = splitter.Orientation == GridSplitterOrientation.Horizontal;
			bool showButtons = IsIconInteractive() || ShouldShowExpandCollapseButtons();

			bool showLeadingButton = IsLeadingIconVisible(splitter);
			bool showTrailingButton = IsTrailingIconVisible(splitter);

			bool rtl = IsRightToLeftLayout();
			float leadingX, trailingX, leadingY, trailingY;
			if (isHorizontal)
			{
				float leftX = separatorBody.Left - buttonClearance;
				float rightX = separatorBody.Right + buttonClearance;
				if (rtl)
				{
					leadingX = rightX;
					trailingX = leftX;
				}
				else
				{
					leadingX = leftX;
					trailingX = rightX;
				}
				leadingY = separatorBody.Center.Y;
				trailingY = separatorBody.Center.Y;
			}
			else
			{
				leadingX = separatorBody.Center.X;
				trailingX = separatorBody.Center.X;
				leadingY = separatorBody.Top - buttonClearance;
				trailingY = separatorBody.Bottom + buttonClearance;
			}

			if (showButtons && showLeadingButton)
			{
				// Chevron direction follows the direction the layout will move when
				// the button is tapped. Expanding the leading pane pushes this
				// separator to the right (right arrow); collapsing the leading pane
				// pulls it to the left (left arrow). When the leading button is
				// expanding the trailing pane, the separator still moves to the
				// left (left arrow).
				bool leadingPaneCollapsed = IsLeadingPaneCollapsed(splitter);
				bool trailingPaneCollapsed = IsTrailingPaneCollapsed(splitter);

				bool drawRightChevron = isHorizontal && leadingPaneCollapsed && !trailingPaneCollapsed;
				bool drawLeftChevron = isHorizontal && !drawRightChevron;
#if !ANDROID
				if (rtl)
				{
					// In RTL the leading pane is on the right, so the chevron
					// direction is mirrored.
					(drawLeftChevron, drawRightChevron) = (drawRightChevron, drawLeftChevron);
				}
#endif
				DrawExpandCollapseButton( canvas, leadingX, leadingY, buttonRadius, isHorizontalLeftButton: drawLeftChevron,
					isHorizontalRightButton: drawRightChevron, isVerticalTopButton: !isHorizontal, isVerticalBottomButton: false,
					isCollapsed: false,orientation: splitter.Orientation);
			}

			if (showButtons && showTrailingButton)
			{
				// Chevron direction follows the direction the layout will move when
				// the button is tapped. Expanding the trailing pane pulls this
				// separator to the left (left arrow); collapsing the trailing
				// pane pushes it to the right (right arrow). When the trailing
				// button is expanding the leading pane, the separator still moves
				// to the right (right arrow).
				bool leadingPaneCollapsed = IsLeadingPaneCollapsed(splitter);
				bool trailingPaneCollapsed = IsTrailingPaneCollapsed(splitter);

				bool drawLeftChevron = isHorizontal && (trailingPaneCollapsed && !leadingPaneCollapsed);
				bool drawRightChevron = isHorizontal && !drawLeftChevron;
#if !ANDROID
				if (rtl)
				{
					// In RTL the trailing pane is on the left, so the chevron
					// direction is mirrored.
					(drawLeftChevron, drawRightChevron) = (drawRightChevron, drawLeftChevron);
				}
#endif
				DrawExpandCollapseButton( canvas, trailingX, trailingY, buttonRadius, isHorizontalLeftButton: drawLeftChevron,
					isHorizontalRightButton: drawRightChevron, isVerticalTopButton: false, isVerticalBottomButton: !isHorizontal,
					isCollapsed: false, orientation: splitter.Orientation);
			}
		}

		/// <summary>
		/// Draws an expand/collapse button with a directional arrow.
		/// Appearance updates based on the button's interaction state and theme.
		/// </summary>
		void DrawExpandCollapseButton(ICanvas canvas, float centerX, float centerY, float radius, bool isHorizontalLeftButton, bool isHorizontalRightButton,
			bool isVerticalTopButton, bool isVerticalBottomButton, bool isCollapsed, GridSplitterOrientation orientation)
		{
			var splitter = FindContainingSplitter();
			if (splitter == null)
			{
				return;
			}
			splitter.GetCurrentStateColors(
				IsIconInteractive(),
				out _,
				out _,
				out Color borderColor);
			// Buttons are only rendered while interactive, so the accent color always
			// comes from the ExpandCollapseIconColor theme resource.
			//Color borderColor = splitter.ExpandCollapseIconColor;
			Color arrowColor = borderColor;

			float borderThickness = IsDragging ? 2.0f : 1.0f;

			// Use the theme-aware fill color so the inner circle matches the active palette.
			canvas.FillColor = splitter.ExpandCollapseButtonFill;
			canvas.FillCircle(centerX, centerY, radius);

			canvas.StrokeColor = borderColor;
			canvas.StrokeSize = borderThickness;
			canvas.DrawCircle(centerX, centerY, radius);

			float arrowSize = ExpandCollapseArrowSize;
			float arrowWidth = arrowSize - 1f;

			if (orientation == GridSplitterOrientation.Horizontal)
			{
				if (isHorizontalLeftButton)
				{
					DrawChevronLeft(canvas, arrowColor, centerX, centerY, arrowSize, arrowWidth);
				} 

				if (isHorizontalRightButton)
				{
					DrawChevronRight(canvas, arrowColor, centerX, centerY, arrowSize, arrowWidth);
				}
			}
			else
			{
				if (isVerticalTopButton)
				{
					DrawChevronUp(canvas, arrowColor, centerX, centerY, arrowSize, arrowWidth);
				}

				if (isVerticalBottomButton)
				{
					DrawChevronDown(canvas, arrowColor, centerX, centerY, arrowSize, arrowWidth);
				}
			}
		}

		/// <summary>
		/// Draws a left-pointing chevron used in expand/collapse buttons.
		/// The chevron uses compact, balanced proportions matching the Fluent/modern
		/// chevron design reference: the arm length and arm spread are equal, producing
		/// a shorter, properly proportioned glyph with consistent stroke width.
		/// </summary>
		void DrawChevronLeft(ICanvas canvas, Color strokeColor, float centerX, float centerY, float size, float width)
		{
			float halfArm = size / 2f;
			canvas.StrokeColor = strokeColor;
			canvas.StrokeSize = 1.5f;
			var path = new PathF();
			path.MoveTo(new PointF(centerX + halfArm, centerY - halfArm));
			path.LineTo(new PointF(centerX - halfArm, centerY));
			path.LineTo(new PointF(centerX + halfArm, centerY + halfArm));
			canvas.DrawPath(path);
		}

		/// <summary>
		/// Draws a right-pointing chevron used in expand/collapse buttons.
		/// </summary>
		void DrawChevronRight(ICanvas canvas, Color strokeColor, float centerX, float centerY, float size, float width)
		{
			float halfArm = size / 2f;
			canvas.StrokeColor = strokeColor;
			canvas.StrokeSize = 1.5f;
			var path = new PathF();
			path.MoveTo(new PointF(centerX - halfArm, centerY - halfArm));
			path.LineTo(new PointF(centerX + halfArm, centerY));
			path.LineTo(new PointF(centerX - halfArm, centerY + halfArm));
			canvas.DrawPath(path);
		}

		/// <summary>
		/// Draws an upward-pointing chevron used in expand/collapse buttons.
		/// </summary>
		void DrawChevronUp(ICanvas canvas, Color strokeColor, float centerX, float centerY, float size, float width)
		{
			float halfArm = size / 2f;
			canvas.StrokeColor = strokeColor;
			canvas.StrokeSize = 1.5f;
			var path = new PathF();
			path.MoveTo(new PointF(centerX - halfArm, centerY + halfArm));
			path.LineTo(new PointF(centerX, centerY - halfArm));
			path.LineTo(new PointF(centerX + halfArm, centerY + halfArm));
			canvas.DrawPath(path);
		}

		/// <summary>
		/// Draws a downward-pointing chevron used in expand/collapse buttons.
		/// </summary>
		void DrawChevronDown(ICanvas canvas, Color strokeColor, float centerX, float centerY, float size, float width)
		{
			float halfArm = size / 2f;
			canvas.StrokeColor = strokeColor;
			canvas.StrokeSize = 1.5f;
			var path = new PathF();
			path.MoveTo(new PointF(centerX - halfArm, centerY - halfArm));
			path.LineTo(new PointF(centerX, centerY + halfArm));
			path.LineTo(new PointF(centerX + halfArm, centerY - halfArm));
			canvas.DrawPath(path);
		}

		/// <summary>
		/// Calculates the hit-test centers for the separator's expand/collapse buttons.
		/// Returns <c>false</c> if the separator has not been arranged.
		/// </summary>
		bool TryGetButtonHitTargets(out float leadingButtonX, out float leadingButtonY, out float trailingButtonX, out float trailingButtonY, out float buttonRadius)
		{
			leadingButtonX = 0f;
			leadingButtonY = 0f;
			trailingButtonX = 0f;
			trailingButtonY = 0f;
			buttonRadius = ExpandCollapseButtonDiameter / 2f;

			var splitter = FindContainingSplitter();
			if (splitter == null)
				return false;

			if (Bounds.Width <= 0 || Bounds.Height <= 0)
				return false;

			float viewWidth = (float)_arrangedSize.Width;
			float viewHeight = (float)_arrangedSize.Height;

			if (viewWidth <= 0 || viewHeight <= 0)
				return false;

			float separatorSize = (float)splitter.SeparatorSize;
			RectF separatorBody = splitter.Orientation == GridSplitterOrientation.Horizontal ? new RectF((viewWidth - separatorSize) / 2f, 0, separatorSize, viewHeight)
				: new RectF(0, (viewHeight - separatorSize) / 2f, viewWidth, separatorSize);

			float buttonClearance = FloatingButtonEdgeOffset + ButtonSeparatorGap;

			bool rtl = IsRightToLeftLayout();

			if (splitter.Orientation == GridSplitterOrientation.Horizontal)
			{
				float leftX = separatorBody.Left - buttonClearance;
				float rightX = separatorBody.Right + buttonClearance;
				if (rtl)
				{
					leadingButtonX = rightX;
					trailingButtonX = leftX;
				}
				else
				{
					leadingButtonX = leftX;
					trailingButtonX = rightX;
				}
				leadingButtonY = separatorBody.Center.Y;
				trailingButtonY = separatorBody.Center.Y;
			}
			else
			{
				leadingButtonX = separatorBody.Center.X;
				trailingButtonX = separatorBody.Center.X;
				leadingButtonY = separatorBody.Top - buttonClearance;
				trailingButtonY = separatorBody.Bottom + buttonClearance;
			}

#if WINDOWS || (!IOS && Maccalyst) || ANDROID
            bool isRTL = splitter.FlowDirection == FlowDirection.RightToLeft;

            if (isRTL)
            {
                (leadingButtonX, trailingButtonX) =
                    (trailingButtonX, leadingButtonX);
            }
#endif
			return true;
		}

		/// <summary>
		/// Determines whether the given point is inside a circular button.
		/// </summary>
		bool IsPointInsideCircle(PointF point, float centerX, float centerY, float radius)
		{
			float dx = point.X - centerX;
			float dy = point.Y - centerY;
			return (dx * dx) + (dy * dy) <= radius * radius;
		}

		bool IsPointWithinButtonRegion(Point point)
		{
			if (!TryGetButtonHitTargets(out float leadingButtonX, out float leadingButtonY, out float trailingButtonX, out float trailingButtonY, out float buttonRadius))
			{
				return false;
			}

			var splitter = FindContainingSplitter();
			if (splitter == null)
			{
				return false;
			}

			float effectiveRadius = buttonRadius + 4f;
			var pointF = new PointF((float)point.X, (float)point.Y);

			bool inLeadingRegion = IsLeadingIconVisible(splitter) && IsPointInsideCircle(pointF, leadingButtonX, leadingButtonY, effectiveRadius);
			bool inTrailingRegion = IsTrailingIconVisible(splitter) && IsPointInsideCircle(pointF, trailingButtonX, trailingButtonY, effectiveRadius);

			return inLeadingRegion || inTrailingRegion;
		}

		/// <summary>
		/// Determines whether the specified point falls within a visible button on this separator or an overlapping sibling separator.
		/// Used to support button interaction when separators share the same screen position.
		/// </summary>
		bool IsPointWithinAnySeparatorButtonRegion(Point point)
		{
			var splitter = FindContainingSplitter();
			if (splitter == null)
				return false;

			if (IsPointWithinButtonRegion(point))
				return true;

			var siblings = splitter.GetSeparators();
			if (siblings == null || siblings.Count <= 1)
				return false;

			bool horizontal = splitter.Orientation == GridSplitterOrientation.Horizontal;
			double thisSplitterX = horizontal ? X : 0;
			double thisSplitterY = horizontal ? 0 : Y;
			float tapSplitterX = (float)(thisSplitterX + point.X);
			float tapSplitterY = (float)(thisSplitterY + point.Y);

			for (int i = 0; i < siblings.Count; i++)
			{
				var sibling = siblings[i];
				if (sibling == null || sibling == this)
					continue;

				if (!sibling.TryGetButtonHitTargets(out float sibLeadingX, out float sibLeadingY, out float sibTrailingX, out float sibTrailingY, out float sibRadius))
					continue;

				double sibSplitterX = horizontal ? sibling.X : 0;
				double sibSplitterY = horizontal ? 0 : sibling.Y;
				float sibLeadingSplitterX = (float)(sibSplitterX + sibLeadingX);
				float sibLeadingSplitterY = (float)(sibSplitterY + sibLeadingY);
				float sibTrailingSplitterX = (float)(sibSplitterX + sibTrailingX);
				float sibTrailingSplitterY = (float)(sibSplitterY + sibTrailingY);

				float sibHitRadius = sibRadius + 4f;
				var tapPointF = new PointF(tapSplitterX, tapSplitterY);

				if (sibling.IsLeadingIconVisible(splitter) && IsPointInsideCircle(tapPointF, sibLeadingSplitterX, sibLeadingSplitterY, sibHitRadius))
				{	
					return true;
				}

				if (sibling.IsTrailingIconVisible(splitter) && IsPointInsideCircle(tapPointF, sibTrailingSplitterX, sibTrailingSplitterY, sibHitRadius))
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Gets the theme-aware separator background color.
		/// Falls back to a default color when the splitter background is not a solid color.
		/// </summary>
		Color GetThemeAwareSeparatorColor()
		{
			return GetSplitterBrushColor(forHover: false);
		}

		/// <summary>
		/// Gets the theme-aware hover color for the separator.
		/// Uses the splitter's expand/collapse icon color and falls back to a default color.
		/// </summary>
		Color GetThemeAwareHoverColor()
		{
			var splitter = FindContainingSplitter();
			return splitter!.ExpandCollapseIconColor;
		}

		/// <summary>
		/// Returns the splitter's separator background color when it is a solid brush,
		/// or a sensible default when the brush cannot be resolved to a color.
		/// </summary>
		Color GetSplitterBrushColor(bool forHover)
		{
			var splitter = FindContainingSplitter();
			if (forHover)
			{
				return splitter!.ExpandCollapseIconColor ;
			}

			if (splitter?.SeparatorBackground is SolidColorBrush solidBrush && solidBrush.Color != null)
			{
				return solidBrush.Color;
			}

			return Color.FromArgb("#CAC4D0");
		}

		/// <summary>
		/// Resolves a <see cref="Brush"/> to a drawable <see cref="Color"/>,
		/// using the splitter's properties to provide theme-aware fallback colors.
		/// </summary>
		Color ResolveBrushColor(Brush? brush, bool forHover)
		{
			if (brush is SolidColorBrush solidBrush && solidBrush.Color != null)
			{
				return solidBrush.Color;
			}

			return forHover ? GetThemeAwareHoverColor() : GetThemeAwareSeparatorColor();
		}

		/// <summary>
		/// Determines whether a separator button would perform an expand action.
		/// Used to prioritize expand-button ownership when separators overlap.
		/// The action is decided by the collapsed state of the relevant pane, not
		/// by the neighboring pane's <see cref="SplitterPane.IsCollapsible"/>
		/// value, so the expand action is correctly reported even when a
		/// non-collapsible neighbor is in between.
		/// </summary>
		static bool IsExpandActionForButton(SfGridSplitter splitter, SeparatorView separator, bool tappedLeading)
		{
			if (splitter == null || separator == null)
			{
				return false;
			}

			int leadingIndex = separator.TrailingPaneIndex - 1;
			int trailingIndex = separator.TrailingPaneIndex;

			if (leadingIndex < 0 || leadingIndex >= splitter.SplitterPanes.Count)
			{
				return false;
			}

			if (trailingIndex < 0 || trailingIndex >= splitter.SplitterPanes.Count)
			{
				return false;
			}

			bool leadingCollapsed = splitter.SplitterPanes[leadingIndex].IsCollapsed;
			bool trailingCollapsed = splitter.SplitterPanes[trailingIndex].IsCollapsed;

			// The button performs an expand action whenever either adjacent pane
			// is collapsed (independent of the neighbor's IsCollapsible value).
			// The non-collapsed case is the only collapse action.
			return leadingCollapsed || trailingCollapsed;
		}

		#endregion

		#region override methods

		/// <summary>
		/// Overrides <see cref="OnHandlerChanged"/> and forwards to platform-specific handler hooks.
		/// Enables platform implementations to attach or detach native event handling.
		/// </summary>
		protected override void OnHandlerChanged()
		{
			base.OnHandlerChanged();
			if (Handler?.PlatformView != null)
			{
				OnSeparatorHandlerAttached();
			}
			else
			{
				OnSeparatorHandlerDetaching();
			}
#if IOS
			if (Handler?.PlatformView is UIView nativeView)
			{
				nativeView.AccessibilityIdentifier = AutomationId;
			}
#endif
		}

		/// <summary>
		/// Overrides <see cref="OnHandlerChanging"/> and forwards to
		/// <see cref="OnSeparatorHandlerDetaching"/> for platform-specific cleanup.
		/// </summary>
		protected override void OnHandlerChanging(HandlerChangingEventArgs args)
		{
			base.OnHandlerChanging(args);
			OnSeparatorHandlerDetaching();
		}

		/// <summary>
		/// Updates the separator's visual state in response to property changes such as <c>IsEnabled</c>.
		/// Refreshes hover and sticky-hover state and requests a redraw.
		/// </summary>
		protected override void ChangeVisualState()
		{
			base.ChangeVisualState();

			if (!IsEnabled)
			{
				_isStickyHovered = false;
				if (s_stickyOwner == this)
				{
					s_stickyOwner = null;
				}

				_lastPointerPosition = null;
				if (IsHovered)
				{
					IsHovered = false;
				}
			}

			InvalidateDrawable();
		}

		/// <summary>
		/// Draws the separator on the canvas.
		/// </summary>
		protected override void OnDraw(ICanvas canvas, RectF dirtyRect)
		{
			base.OnDraw(canvas, dirtyRect);
			DrawSeparator(canvas, dirtyRect);
		}

		/// <summary>
		/// Measures the separator size based on the parent control's <see cref="SfGridSplitter.SeparatorSize"/>.
		/// The width (for Horizontal) or height (for Vertical) is fixed to SeparatorSize.
		/// The other axis fills the available space.
		/// </summary>
		protected override Size MeasureContent(double widthConstraint, double heightConstraint)
		{
			var splitter = FindContainingSplitter();
			if (splitter == null)
			{
				return new Size(8, 8);
			}

			double sepSize = Math.Max(0, splitter.SeparatorSize);
			if (splitter.Orientation == GridSplitterOrientation.Horizontal)
			{
				double width = Math.Min(sepSize, double.IsFinite(widthConstraint) ? widthConstraint : sepSize);
				double height = double.IsFinite(heightConstraint) ? heightConstraint : 0;
				return new Size(width, height);
			}
			else
			{
				double width = double.IsFinite(widthConstraint) ? widthConstraint : 0;
				double height = Math.Min(sepSize, double.IsFinite(heightConstraint) ? heightConstraint : sepSize);
				return new Size(width, height);
			}
		}

		/// <summary>
		/// Arranges the separator within the specified bounds and updates the hit zone.
		/// </summary>
		protected override Size ArrangeContent(Rect bounds)
		{
			ArrangeSeparator(bounds);
			return bounds.Size;
		}

		#endregion

		#region Partial methods 

		partial void InitializePlatformTouchPath();
		/// <summary>
		/// Platform hook invoked after the separator's native handler or view is attached.
		/// Platform implementations can wire native events; the shared implementation is a no-op.
		/// </summary>
		partial void OnSeparatorHandlerAttached();

		/// <summary>
		/// Platform hook invoked before the separator's native handler or view is replaced or removed.
		/// Platform implementations can release native resources; the shared implementation is a no-op.
		/// </summary>
		partial void OnSeparatorHandlerDetaching();

		#endregion

	}
}
