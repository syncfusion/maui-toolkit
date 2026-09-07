namespace Syncfusion.Maui.Toolkit.Internals
{
    using Android.Animation;
    using Android.Content;
    using Android.Views;
    using Android.Widget;
    using Microsoft.Maui;
    using System;
    using View = Android.Views.View;

    /// <summary>
    /// Represents a native Android scroll viewer that supports horizontal, vertical, and bidirectional scrolling.
    /// </summary>
    /// <remarks>
    /// The horizontal scrollbar is displayed at the bottom of the content.
    /// </remarks>
    internal partial class PlatformScrollViewer : Android.Widget.ScrollView
    {
        #region Fields

        /// <summary>
        /// Hosts the horizontal scrolling content.
        /// </summary>
        HorizontalScrollViewer? _horizontalScrollViewer;

        /// <summary>
        /// Stores the current viewport width.
        /// </summary>
        int _viewPortWidth;

        /// <summary>
        /// Stores the current viewport height.
        /// </summary>
        int _viewPortHeight;

        /// <summary>
        /// Indicates whether bidirectional scrolling is enabled.
        /// </summary>
        bool _isBidirectional = true;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether the current scroll request was explicitly initiated.
        /// </summary>
        internal bool ExplicitScrollRequest { get; set; } = false;

        /// <summary>
        /// Gets or sets the last recorded horizontal touch position.
        /// </summary>
        internal float LastX { get; set; } = 0;

        /// <summary>
        /// Gets or sets the last recorded vertical touch position.
        /// </summary>
        internal float LastY { get; set; } = 0;

        /// <summary>
        /// Gets or sets a value indicating whether the next touch event should be ignored.
        /// </summary>
        internal bool ShouldSkipOnTouch { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether scrolling is enabled.
        /// </summary>
        internal bool IsScrollingEnabled { get; set; } = true;

        /// <summary>
        /// Gets the viewport height.
        /// </summary>
        internal int ViewPortHeight
        {
            get
            {
                return _viewPortHeight;
            }
        }

        /// <summary>
        /// Gets the viewport width.
        /// </summary>
        internal int ViewPortWidth
        {
            get
            {
                return _viewPortWidth;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="PlatformScrollViewer"/> class.
        /// </summary>
        /// <param name="context">The Android context.</param>
        internal PlatformScrollViewer(Context? context) : base(context)
        {
            // ClipToOutline - This property is needed to clip the contents which are scrolled out of the viewport.
            // If false, the contents will not be clipped and visible over the other controls.
            // This can be checked by adding the scroll view as one of the children of another View Group (Linear layout).
            this.ClipToOutline = true;
            if (OperatingSystem.IsAndroidVersionAtLeast(23))
            {
                this.ScrollChange += OnVerticalScrollChange;
            }

            this.LayoutChange += OnLayoutChange;
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the scroll position is changed.
        /// </summary>
        internal event EventHandler<ScrollChangeEventArgs>? ScrollChanged;

        /// <summary>
        /// Occurs when a touch-pan gesture is detected.
        /// </summary>
        internal event EventHandler<MotionEvent>? OnPan;

        #endregion

        #region Override Methods

        /// <summary>
        /// Scrolls the view to the specified position.
        /// </summary>
        /// <param name="x">The horizontal scroll position.</param>
        /// <param name="y">The vertical scroll position.</param>
        public override void ScrollTo(int x, int y)
        {
            if (_horizontalScrollViewer != null && _horizontalScrollViewer.IsBlockEnabled && !ExplicitScrollRequest)
            {
                return;
            }

            base.ScrollTo(x, y);
            ExplicitScrollRequest = false;
        }

        /// <summary>
        /// Intercepts touch events to determine if scrolling should occur.
        /// </summary>
        /// <param name="ev">The motion event.</param>
        /// <returns><c>true</c> if the event should be intercepted; otherwise, <c>false</c>.</returns>
        public override bool OnInterceptTouchEvent(MotionEvent? ev)
        {
            if (!IsScrollingEnabled)
                return false;

            if (ev == null)
                return false;

            // set the start point for the bi-directional scroll.
            if (_isBidirectional && ev.Action == MotionEventActions.Down)
            {
                LastY = ev.RawY;
                LastX = ev.RawX;
            }

            return base.OnInterceptTouchEvent(ev);
        }

        /// <summary>
        /// Handles touch events and coordinates scrolling behavior.
        /// </summary>
        /// <param name="ev">The motion event.</param>
        /// <returns>
        /// <c>true</c> if the event was handled; otherwise, <c>false</c>.
        /// </returns>
        public override bool OnTouchEvent(MotionEvent? ev)
        {
            if (ev == null || !Enabled)
                return false;

            OnTouchUpdated(ev);
            if (ev.Action == MotionEventActions.Move)
                ExplicitScrollRequest = true;

            if (ShouldSkipOnTouch)
            {
                ShouldSkipOnTouch = false;
                return false;
            }

            if (_isBidirectional)
            {
                float dX = LastX - ev.RawX;
                LastY = ev.RawY;
                LastX = ev.RawX;
                if (ev.Action == MotionEventActions.Move && _horizontalScrollViewer != null && _horizontalScrollViewer.IsScrollingEnabled)
                {
                    _horizontalScrollViewer.ScrollBy((int)dX, 0);
                }
            }

            return base.OnTouchEvent(ev);
        }

        /// <summary>
        /// Handles over-scroll notifications.
        /// </summary>
        /// <param name="scrollX">The horizontal scroll position.</param>
        /// <param name="scrollY">The vertical scroll position.</param>
        /// <param name="clampedX">Indicates whether horizontal scrolling is clamped.</param>
        /// <param name="clampedY">Indicates whether vertical scrolling is clamped.</param>
        protected override void OnOverScrolled(int scrollX, int scrollY, bool clampedX, bool clampedY)
        {
            if (_horizontalScrollViewer != null && _horizontalScrollViewer.IsBlockEnabled && !ExplicitScrollRequest)
                return;

            base.OnOverScrolled(scrollX, scrollY, clampedX, clampedY);
            ExplicitScrollRequest = false;
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Enables or disables vertical scrolling.
        /// </summary>
        /// <param name="isEnabled">
        /// <c>true</c> to enable vertical scrolling; otherwise, <c>false</c>.
        /// </param>
        internal void SetVerticalScrollingEnabled(bool isEnabled)
        {
            IsScrollingEnabled = isEnabled;
            this.SetVerticalScrollBarEnabled(false);
        }

        /// <summary>
        /// Enables or disables horizontal scrolling.
        /// </summary>
        /// <param name="isEnabled">
        /// <c>true</c> to enable horizontal scrolling; otherwise, <c>false</c>.
        /// </param>
        internal void SetHorizontalScrollingEnabled(bool isEnabled)
        {
            if (_horizontalScrollViewer != null)
            {
                _horizontalScrollViewer.IsScrollingEnabled = isEnabled;
                this.SetHorizontalScrollBarEnabled(false);
            }
        }

        /// <summary>
        /// Raises the pan gesture event.
        /// </summary>
        /// <param name="motionEvent">The motion event data.</param>
        internal void OnTouchUpdated(MotionEvent motionEvent)
        {
            OnPan?.Invoke(this, motionEvent);
        }

        /// <summary>
        /// Enables or disables vertical scrollbar fading.
        /// </summary>
        /// <param name="enabled">Whether fading is enabled.</param>
        internal void SetVerticalScrollBarFadingEnabled(bool enabled)
        {
            this.ScrollbarFadingEnabled = enabled;
            this.Invalidate();
        }

        /// <summary>
        /// Enables or disables vertical scrollbar.
        /// </summary>
        /// <param name="enabled"><c>true</c> to display the vertical scrollbar; otherwise, <c>false</c>.</param>
        internal void SetVerticalScrollBarEnabled(bool enabled)
        {
            this.VerticalScrollBarEnabled = enabled;
            this.Invalidate();
        }

        /// <summary>
        /// Enables or disables horizontal scrollbar fading.
        /// </summary>
        /// <param name="enabled"><c>true</c> to enable scrollbar fading; otherwise, <c>false</c>.</param>
        internal void SetHorizontalScrollBarFadingEnabled(bool enabled)
        {
            if (_horizontalScrollViewer != null)
            {
                _horizontalScrollViewer.ScrollbarFadingEnabled = enabled;
                _horizontalScrollViewer.Invalidate();
            }
        }

        /// <summary>
        /// Enables or disables the horizontal scrollbar.
        /// </summary>
        /// <param name="enabled">
        /// <c>true</c> to display the horizontal scrollbar; otherwise, <c>false</c>.
        /// </param>
        internal void SetHorizontalScrollBarEnabled(bool enabled)
        {
            if (_horizontalScrollViewer != null)
            {
                _horizontalScrollViewer.HorizontalScrollBarEnabled = enabled;
                _horizontalScrollViewer.Invalidate();
            }
        }

        /// <summary>
        /// Enables or disables automatic scrolling.
        /// </summary>
        /// <param name="suppressAutoScroll">
        /// <c>true</c> to suppress automatic scrolling; otherwise, <c>false</c>.
        /// </param>
        internal void SetSuppressAutoScroll(bool suppressAutoScroll)
        {
            if (_horizontalScrollViewer != null)
            {
                _horizontalScrollViewer.IsBlockEnabled = suppressAutoScroll;
            }
        }

        /// <summary>
        /// Sets the content of the scroll viewer.
        /// </summary>
        /// <param name="view">The content view.</param>
        internal void SetContent(View view)
        {
            if (_horizontalScrollViewer == null)
            {
                _horizontalScrollViewer = new HorizontalScrollViewer(Context)
                {
                    ParentScrollView = this
                };

                if (OperatingSystem.IsAndroidVersionAtLeast(23))
                    _horizontalScrollViewer.ScrollChange += OnHorizontalScrollChange;

                this.AddView(_horizontalScrollViewer, ViewGroup.LayoutParams.MatchParent);
                this.FillViewport = true;
            }

            _horizontalScrollViewer?.SetContent(view);
        }

        /// <summary>
        /// Removes and disconnects all hosted views.
        /// </summary>
        internal void DisconnectViews()
        {
            _horizontalScrollViewer?.RemoveAllViews();
            this.RemoveAllViews();
        }

        /// <summary>
        /// Scrolls to the specified offset.
        /// </summary>
        /// <param name="x">The horizontal scroll offset.</param>
        /// <param name="y">The vertical scroll offset.</param>
        /// <param name="animated">A value indicating whether the scroll is animated.</param>
        /// <param name="scrollOrientation">The scroll orientation to apply.</param>
        /// <param name="finished">The callback invoked when scrolling completes.</param>
        internal void ScrollToOffset(int x, int y, bool animated, ScrollOrientation scrollOrientation, Action finished)
        {
            ExplicitScrollRequest = true;
            x = GetMaxHorizontalScrollOffset(x);
            y = GetMaxVerticalScrollOffset(y);
            if (!animated)
            {
                JumpToOffset(x, y, scrollOrientation, finished);
                return;
            }

            SmoothScrollToOffset(x, y, scrollOrientation, finished);
        }

        /// <summary>
        /// Raises the <see cref="ScrollChanged"/> event.
        /// </summary>
        /// <param name="args">The scroll change event data.</param>
        internal void OnScrollChanged(ScrollChangeEventArgs args)
        {
            ScrollChanged?.Invoke(this, args);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Updates the viewport size when the layout changes.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">The layout change event data.</param>
        void OnLayoutChange(object? sender, LayoutChangeEventArgs e)
        {
            // View port size is equal to the size of the control, because there is no impact of the scrollbars size in viewport, as the scrollbars are present overlay the control.
            if (e.OldLeft != e.Left || e.OldTop != e.Top || e.OldRight != e.Right || e.OldBottom != e.Bottom)
            {
                _viewPortHeight = this.ComputeVerticalScrollExtent();
                _viewPortWidth = this.ComputeHorizontalScrollExtent();
            }
        }

        /// <summary>
        /// Handles vertical scroll position changes.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">The scroll change event data.</param>
        void OnVerticalScrollChange(object? sender, ScrollChangeEventArgs e)
        {
            if (_horizontalScrollViewer != null)
            {
                ExecuteScrollChangeEvent(_horizontalScrollViewer.ScrollX, e.ScrollY, _horizontalScrollViewer.ScrollX, e.OldScrollY);
                return;
            }

            ExecuteScrollChangeEvent(e.ScrollX, e.ScrollY, e.ScrollX, e.OldScrollY);
        }

        /// <summary>
        /// Handles horizontal scroll position changes.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">The scroll change event data.</param>
        void OnHorizontalScrollChange(object? sender, ScrollChangeEventArgs e)
        {
            ExecuteScrollChangeEvent(e.ScrollX, this.ScrollY, e.OldScrollX, ScrollY);
        }

        /// <summary>
        /// Returns the maximum valid vertical scroll offset.
        /// </summary>
        /// <param name="offSet">The requested offset.</param>
        int GetMaxVerticalScrollOffset(int offSet)
        {
            if (offSet < 0)
                return -1;

            if (offSet > 0)
            {
                int maxScrollOffset = ComputeVerticalScrollRange() - ViewPortHeight;
                return offSet <= maxScrollOffset ? offSet : maxScrollOffset;
            }
            else
                return offSet;
        }

        /// <summary>
        /// Returns the maximum valid horizontal scroll offset.
        /// </summary>
        /// <param name="offSet">The requested offset.</param>
        /// <returns>The adjusted horizontal scroll offset.</returns>
        int GetMaxHorizontalScrollOffset(int offSet)
        {
            if (offSet < 0)
                return -1;

            if (_horizontalScrollViewer != null && offSet > 0)
            {
                int maxScrollOffset = _horizontalScrollViewer.GetHorizontalScrollRange() - ViewPortWidth;
                return offSet <= maxScrollOffset ? offSet : maxScrollOffset;
            }
            else
                return offSet;
        }

        /// <summary>
        /// Scrolls immediately to the specified offset.
        /// </summary>
        /// <param name="x">The horizontal scroll offset.</param>
        /// <param name="y">The vertical scroll offset.</param>
        /// <param name="scrollOrientation">The scroll orientation.</param>
        /// <param name="finished">The callback invoked when scrolling completes.</param>
        void JumpToOffset(int x, int y, ScrollOrientation scrollOrientation, Action finished)
        {
            switch (scrollOrientation)
            {
                case ScrollOrientation.Vertical:
                    ScrollTo(x, y);
                    break;
                case ScrollOrientation.Horizontal:
                    _horizontalScrollViewer?.ScrollTo(x, y);
                    break;
                case ScrollOrientation.Both:
                    UnwireScrollEvents();
                    if (_horizontalScrollViewer != null)
                    {
                        int oldScrollX = _horizontalScrollViewer.ScrollX;
                        int oldScrollY = ScrollY;
                        _horizontalScrollViewer?.ScrollTo(x, y);
                        ScrollTo(x, y);
                        ExecuteScrollChangeEvent(x, y, oldScrollX, oldScrollY);
                    }

                    WireScrollEvents();
                    break;
                case ScrollOrientation.Neither:
                    break;
            }

            finished();
        }

        /// <summary>
        /// Raises a scroll change event.
        /// </summary>
        /// <param name="scrollX">The current horizontal scroll position.</param>
        /// <param name="scrollY">The current vertical scroll position.</param>
        /// <param name="oldScrollX">The previous horizontal scroll position.</param>
        /// <param name="oldScrollY">The previous vertical scroll position.</param>
        void ExecuteScrollChangeEvent(int scrollX, int scrollY, int oldScrollX, int oldScrollY)
        {
            ScrollChangeEventArgs eventArgs = new ScrollChangeEventArgs(this, scrollX, scrollY, oldScrollX, oldScrollY);
            OnScrollChanged(eventArgs);
        }

        /// <summary>
        /// Unsubscribes from scroll change events.
        /// </summary>
        void UnwireScrollEvents()
        {
            if (OperatingSystem.IsAndroidVersionAtLeast(23))
            {
                if (_horizontalScrollViewer != null)
                    _horizontalScrollViewer.ScrollChange -= OnHorizontalScrollChange;

                ScrollChange -= OnVerticalScrollChange;
            }
        }

        /// <summary>
        /// Subscribes to scroll change events.
        /// </summary>
        void WireScrollEvents()
        {
            if (OperatingSystem.IsAndroidVersionAtLeast(23))
            {
                if (_horizontalScrollViewer != null)
                    _horizontalScrollViewer.ScrollChange += OnHorizontalScrollChange;

                ScrollChange += OnVerticalScrollChange;
            }
        }

        /// <summary>
        /// Animates scrolling to the specified offset.
        /// </summary>
        /// <param name="x">The target horizontal scroll offset.</param>
        /// <param name="y">The target vertical scroll offset.</param>
        /// <param name="scrollOrientation">The scroll orientation.</param>
        /// <param name="finished">The callback invoked when scrolling completes.</param>
        void SmoothScrollToOffset(int x, int y, ScrollOrientation scrollOrientation, Action finished)
        {
            int currentX = ScrollX;
            int currentY = ScrollY;
            if (_horizontalScrollViewer != null)
            {
                currentX = scrollOrientation == ScrollOrientation.Horizontal || scrollOrientation == ScrollOrientation.Both ? _horizontalScrollViewer.ScrollX : ScrollX;
                currentY = scrollOrientation == ScrollOrientation.Vertical || scrollOrientation == ScrollOrientation.Both ? ScrollY : _horizontalScrollViewer.ScrollY;
            }

            ValueAnimator? animator = ValueAnimator.OfFloat(0f, 1f);
            if (animator == null)
                return;

            animator.SetDuration(1000);
            animator.Update += (o, animatorUpdateEventArgs) =>
            {
                if (animatorUpdateEventArgs.Animation != null && animatorUpdateEventArgs.Animation.AnimatedValue != null)
                {
                    var animatedValue = (double)(animatorUpdateEventArgs.Animation.AnimatedValue);
                    int distX = GetDistance(currentX, x, animatedValue);
                    int distY = GetDistance(currentY, y, animatedValue);

                    switch (scrollOrientation)
                    {
                        case ScrollOrientation.Horizontal:
                            _horizontalScrollViewer?.ScrollTo(distX, distY);
                            break;
                        case ScrollOrientation.Vertical:
                            ExplicitScrollRequest = true;
                            ScrollTo(distX, distY);
                            break;
                        default:
                            _horizontalScrollViewer?.ScrollTo(distX, distY);
                            ExplicitScrollRequest = true;
                            ScrollTo(distX, distY);
                            break;
                    }
                }
            };

            animator.AnimationEnd += delegate
            {
                finished();
            };

            animator.Start();
        }

        /// <summary>
        /// Calculates the interpolated scroll position for an animation.
        /// </summary>
        /// <param name="start">The starting position.</param>
        /// <param name="position">The target position.</param>
        /// <param name="animatedValue">The animation progress.</param>
        /// <returns>The interpolated position.</returns>
        int GetDistance(double start, double position, double animatedValue)
        {
            return (int)(start + (position - start) * animatedValue);
        }

        #endregion
    }
}