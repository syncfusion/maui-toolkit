namespace Syncfusion.Maui.Toolkit.Internals
{
    using Android.Content;
    using Android.Views;
    using Android.Widget;

    /// <summary>
    /// Represents a horizontal scroll view that supports bi-directional scrolling and coordinated scrolling behavior with a parent <see cref="PlatformScrollViewer"/>.
    /// </summary>
    internal class HorizontalScrollViewer : HorizontalScrollView
    {
        #region Fields

        /// <summary>
        /// Indicates whether bi-directional scrolling is enabled.
        /// </summary>
        bool _isBidirectional = true;

        /// <summary>
        /// Indicates whether the current scroll operation was initiated by a touch gesture.
        /// </summary>
        bool _scrollByTouch;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="HorizontalScrollViewer"/> class.
        /// </summary>
        /// <param name="context">The Android context associated with the view.</param>
        internal HorizontalScrollViewer(Context? context)
            : base(context)
        {
            // Prevents content from rendering outside the bounds of the scroll view.
            ClipToOutline = true;
        }

        #endregion

        #region Properties

        /// <summary>
        /// References the parent scroll viewer.
        /// </summary>
        internal PlatformScrollViewer? ParentScrollView { get; set; }

        /// <summary>
        /// Indicates whether horizontal scrolling is enabled.
        /// </summary>
        internal bool IsScrollingEnabled { get; set; } = true;

        /// <summary>
        /// Indicates whether programmatic scrolling is temporarily blocked.
        /// </summary>
        internal bool IsBlockEnabled { get; set; }

        #endregion

        #region Override Methods

        /// <summary>
        /// Handles over-scroll events that occur when scrolling reaches the content boundaries.
        /// </summary>
        /// <param name="scrollX">The current horizontal scroll position.</param>
        /// <param name="scrollY">The current vertical scroll position.</param>
        /// <param name="clampedX">Indicates whether horizontal scrolling reached its boundary.</param>
        /// <param name="clampedY">Indicates whether vertical scrolling reached its boundary.</param>
        protected override void OnOverScrolled(int scrollX, int scrollY, bool clampedX, bool clampedY)
        {
            if (IsBlockEnabled && !_scrollByTouch)
            {
                return;
            }

            base.OnOverScrolled(scrollX, scrollY, clampedX, clampedY);
            _scrollByTouch = false;
        }

        /// <summary>
        /// Scrolls the view to the specified position.
        /// </summary>
        /// <param name="x">The horizontal scroll position.</param>
        /// <param name="y">The vertical scroll position.</param>
        public override void ScrollTo(int x, int y)
        {
            if (IsBlockEnabled &&
                !_scrollByTouch &&
                ParentScrollView != null &&
                !ParentScrollView.ExplicitScrollRequest)
            {
                return;
            }

            base.ScrollTo(x, y);
            _scrollByTouch = false;
        }

        /// <summary>
        /// Intercepts touch events before they are dispatched to child views.
        /// </summary>
        /// <param name="ev">The motion event being processed.</param>
        /// <returns><see langword="true"/> if the event should be intercepted; otherwise, <see langword="false"/>.</returns>
        public override bool OnInterceptTouchEvent(MotionEvent? ev)
        {
            if (!IsScrollingEnabled)
            {
                return false;
            }

            if (ev == null || ParentScrollView == null)
            {
                return false;
            }

            if (_isBidirectional && ev.Action == MotionEventActions.Down)
            {
                ParentScrollView.LastY = ev.RawY;
                ParentScrollView.LastX = ev.RawX;
            }

            return base.OnInterceptTouchEvent(ev);
        }

        /// <summary>
        /// Handles touch events and coordinates scrolling with the parent scroll view.
        /// </summary>
        /// <param name="ev">The motion event being processed.</param>
        /// <returns><see langword="true"/> if the event is handled successfully; otherwise, <see langword="false"/>.</returns>
        public override bool OnTouchEvent(MotionEvent? ev)
        {
            if (ev == null || ParentScrollView == null)
            {
                return false;
            }

            if (!ParentScrollView.Enabled)
            {
                return false;
            }

            // If the touch is captured by the horizontal scroll view,
            // forward it to the parent scroll view.
            ParentScrollView.ShouldSkipOnTouch = true;
            ParentScrollView.OnTouchEvent(ev);

            if (_isBidirectional)
            {
                float deltaY = ParentScrollView.LastY - ev.RawY;

                ParentScrollView.LastY = ev.RawY;
                ParentScrollView.LastX = ev.RawX;

                if (ev.Action == MotionEventActions.Move &&
                    ParentScrollView.IsScrollingEnabled)
                {
                    _scrollByTouch = true;

                    // Synchronize vertical scrolling with the parent scroll view.
                    ParentScrollView.ScrollBy(0, (int)deltaY);
                }
            }

            return base.OnTouchEvent(ev);
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Sets the scrollable content displayed by the control.
        /// </summary>
        /// <param name="view">The view to display as the scrollable content.</param>
        internal void SetContent(View view)
        {
            if (ChildCount == 0)
            {
                AddView(view);
                FillViewport = true;
            }
        }

        /// <summary>
        /// Gets the total horizontal scrollable range of the content.
        /// </summary>
        /// <returns>The horizontal scroll range, in pixels.</returns>
        internal int GetHorizontalScrollRange()
        {
            return ComputeHorizontalScrollRange();
        }

        #endregion
    }
}