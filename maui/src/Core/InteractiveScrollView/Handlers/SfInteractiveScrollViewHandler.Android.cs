namespace Syncfusion.Maui.Toolkit.Internals
{
    using Android.Content;
    using Microsoft.Maui.Controls;
    using Microsoft.Maui;
    using Microsoft.Maui.Handlers;
    using Microsoft.Maui.Platform;
    using System;
    using MotionEvent = Android.Views.MotionEvent;
    using MotionEventActions = Android.Views.MotionEventActions;

    /// <summary>
    /// Provides the Android-specific handler implementation for <see cref="SfInteractiveScrollView"/>.
    /// This handler creates and manages the native Android scroll view, maps virtual view properties and commands to the platform view,
    /// and coordinates scrolling, panning, content layout updates, and platform-specific interactions.
    /// </summary>
    internal partial class SfInteractiveScrollViewHandler : ViewHandler<SfInteractiveScrollView, PlatformScrollViewer>
    {
        #region Fields

        /// <summary>
        /// Holds the native content view hosted by the platform scroll viewer.
        /// </summary>
        Android.Views.View? _content;

        /// <summary>
        /// Stores a pending scroll request until content layout is completed.
        /// </summary>
        ScrollToParameters? _scrollOffsetRequest;

        /// <summary>
        /// Indicates whether a pan gesture is currently active.
        /// </summary>
        bool _isPanStarted;

        /// <summary>
        /// Provides proxy callbacks for native platform events.
        /// </summary>
        SfScrollViewProxy? _proxy;

        #endregion

        #region Override methods

        /// <summary>
        /// Creates and returns the native Android scroll viewer.
        /// </summary>
        /// <returns>The native platform scroll viewer.</returns>
        protected override PlatformScrollViewer CreatePlatformView()
        {
            this._proxy = new(this);
            PlatformScrollViewer? vScroller = new PlatformScrollViewer(Context);
            return vScroller;
        }

        /// <summary>
        /// Connects the handler to the native platform view and subscribes to platform-specific events.
        /// </summary>
        /// <param name="platformView">The native platform view.</param>
        protected override void ConnectHandler(PlatformScrollViewer platformView)
        {
            base.ConnectHandler(platformView);
            if (this._proxy != null)
            {
                platformView.ScrollChanged += this._proxy.OnScrollChanged;
                platformView.LayoutChange += this._proxy.OnLayoutChange;
                platformView.OnPan += this._proxy.OnPlatformViewPan;
            }
        }

        /// <summary>
        /// Disconnects the handler from the native platform view and unsubscribes from platform-specific events.
        /// </summary>
        /// <param name="platformView">The native platform view.</param>
        protected override void DisconnectHandler(PlatformScrollViewer platformView)
        {
            if (this._proxy != null && _content != null)
            {
                platformView.ScrollChanged -= this._proxy.OnScrollChanged;
                platformView.LayoutChange -= this._proxy.OnLayoutChange;
                platformView.OnPan -= this._proxy.OnPlatformViewPan;
                _content.LayoutChange -= this._proxy.OnContentLayoutChange;
            }

            platformView.DisconnectViews();
            base.DisconnectHandler(platformView);
        }

        #endregion

        #region Property mapper methods

        /// <summary>
        /// Maps the <see cref="SfInteractiveScrollView.Orientation"/> property to the native platform view.
        /// </summary>
        /// <param name="handler">The view handler.</param>
        /// <param name="scrollView">The interactive scroll view.</param>
        internal static void MapScrollOrientation(SfInteractiveScrollViewHandler handler, SfInteractiveScrollView scrollView)
        {
            switch (scrollView.Orientation)
            {
                case ScrollOrientation.Neither:
                    handler.SetScrollingEnabled(false, false);
                    break;
                case ScrollOrientation.Vertical:
                    handler.SetScrollingEnabled(false, true);
                    handler.SetVScrollBarVisibility(scrollView.VerticalScrollBarVisibility);
                    break;
                case ScrollOrientation.Horizontal:
                    handler.SetScrollingEnabled(true, false);
                    handler.SetHScrollBarVisibility(scrollView.HorizontalScrollBarVisibility);
                    break;
                case ScrollOrientation.Both:
                    handler.SetScrollingEnabled(true, true);
                    handler.SetHScrollBarVisibility(scrollView.HorizontalScrollBarVisibility);
                    handler.SetVScrollBarVisibility(scrollView.VerticalScrollBarVisibility);
                    break;
            }
        }

        /// <summary>
        /// Maps the <see cref="SfInteractiveScrollView.SuppressAutoScroll"/> property to the native platform view.
        /// </summary>
        /// <param name="handler">The view handler.</param>
        /// <param name="interactiveScrollView">The interactive scroll view.</param>
        internal static void MapSuppressAutoScroll(SfInteractiveScrollViewHandler handler, SfInteractiveScrollView interactiveScrollView)
        {
            if (interactiveScrollView.SuppressAutoScroll)
                handler.SetSuppressAutoScrollValue(true);
            else
                handler.SetSuppressAutoScrollValue(false);
        }

        /// <summary>
        /// Maps the horizontal scroll bar visibility setting to the native platform view.
        /// </summary>
        /// <param name="handler">The view handler.</param>
        /// <param name="scrollView">The interactive scroll view.</param>
        internal static void MapHorizontalScrollBarVisibility(SfInteractiveScrollViewHandler handler, SfInteractiveScrollView scrollView)
        {
            handler.SetHScrollBarVisibility(scrollView.HorizontalScrollBarVisibility);
        }

        /// <summary>
        /// Maps the vertical scroll bar visibility setting to the native platform view.
        /// </summary>
        /// <param name="handler">The view handler.</param>
        /// <param name="scrollView">The interactive scroll view.</param>
        internal static void MapVerticalScrollBarVisibility(SfInteractiveScrollViewHandler handler, SfInteractiveScrollView scrollView)
        {
            handler.SetVScrollBarVisibility(scrollView.VerticalScrollBarVisibility);
        }

        /// <summary>
        /// Maps a scroll request from the virtual view to the native platform view.
        /// </summary>
        /// <param name="handler">The view handler.</param>
        /// <param name="scrollView">The interactive scroll view.</param>
        /// <param name="args">The scroll request parameters.</param>
        internal static void MapScrollTo(SfInteractiveScrollViewHandler handler, SfInteractiveScrollView scrollView, object? args)
        {
            ScrollToParameters? parameters = args as ScrollToParameters;
            if (parameters != null)
            {
                handler.ScrollTo(parameters);
            }
        }

        /// <summary>
        /// Maps the content of the virtual view to the native platform view.
        /// </summary>
        /// <param name="handler">The view handler.</param>
        /// <param name="scrollView">The interactive scroll view.</param>
        internal static void MapContent(SfInteractiveScrollViewHandler handler, SfInteractiveScrollView scrollView)
        {
            if (handler.PlatformView == null || handler.MauiContext == null)
                return;

            View? content = scrollView.PresentedContent;
            if (content != null)
            {
                var nativeElement = content.ToPlatform(handler.MauiContext);
                if (handler._proxy != null)
                {
                    nativeElement.LayoutChange += handler._proxy.OnContentLayoutChange;
                }

                handler._content = nativeElement;
                if (handler.PlatformView is PlatformScrollViewer vScroller)
                    vScroller.SetContent(nativeElement);
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Enables or disables horizontal and vertical scrolling on the native platform view.
        /// </summary>
        /// <param name="enableHorizontalScrolling">A value indicating whether horizontal scrolling is enabled.</param>
        /// <param name="enableVerticalScrolling">A value indicating whether vertical scrolling is enabled.</param>
        void SetScrollingEnabled(bool enableHorizontalScrolling, bool enableVerticalScrolling)
        {
            PlatformView.SetHorizontalScrollingEnabled(enableHorizontalScrolling);
            PlatformView.SetVerticalScrollingEnabled(enableVerticalScrolling);
        }

        /// <summary>
        /// Updates the virtual view with the latest scroll position reported by the native platform view.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The scroll change event data.</param>
        private void OnScrollChanged(object? sender, Android.Views.View.ScrollChangeEventArgs e)
        {
            double oldScrollX = Context.FromPixels(e.OldScrollX);
            double oldScrollY = Context.FromPixels(e.OldScrollY);
            double scrollX = Context.FromPixels(e.ScrollX);
            double scrollY = Context.FromPixels(e.ScrollY);

            ScrollChangedEventArgs scrolledEventArgs = new ScrollChangedEventArgs(scrollX, scrollY, oldScrollX, oldScrollY);
            VirtualView.OnScrollChanged(scrolledEventArgs);
        }

        /// <summary>
        /// Updates the visibility behavior of the vertical scroll bar.
        /// </summary>
        /// <param name="scrollBarVisibility">The desired vertical scroll bar visibility.</param>
        void SetVScrollBarVisibility(ScrollBarVisibility scrollBarVisibility)
        {
            switch (scrollBarVisibility)
            {
                case ScrollBarVisibility.Default:
                    PlatformView.SetVerticalScrollBarEnabled(true);
                    PlatformView.SetVerticalScrollBarFadingEnabled(true);
                    break;
                case ScrollBarVisibility.Always:
                    PlatformView.SetVerticalScrollBarEnabled(true);
                    PlatformView.SetVerticalScrollBarFadingEnabled(false);
                    break;
                case ScrollBarVisibility.Never:
                    PlatformView.SetVerticalScrollBarEnabled(false);
                    break;
            }
        }

        /// <summary>
        /// Updates the visibility behavior of the horizontal scroll bar.
        /// </summary>
        /// <param name="scrollBarVisibility">The desired horizontal scroll bar visibility.</param>
        void SetHScrollBarVisibility(ScrollBarVisibility scrollBarVisibility)
        {
            switch (scrollBarVisibility)
            {
                case ScrollBarVisibility.Default:
                    PlatformView.SetHorizontalScrollBarEnabled(true);
                    PlatformView.SetHorizontalScrollBarFadingEnabled(true);
                    break;
                case ScrollBarVisibility.Always:
                    PlatformView.SetHorizontalScrollBarEnabled(true);
                    PlatformView.SetHorizontalScrollBarFadingEnabled(false);
                    break;
                case ScrollBarVisibility.Never:
                    PlatformView.SetHorizontalScrollBarEnabled(false);
                    break;
            }
        }

        /// <summary>
        /// Enables or disables automatic scrolling to focused elements.
        /// </summary>
        /// <param name="disableAutoScroll">A value indicating whether automatic scrolling is suppressed.</param>
        void SetSuppressAutoScrollValue(bool disableAutoScroll)
        {
            if (disableAutoScroll)
                PlatformView.SetSuppressAutoScroll(true);
            else
                PlatformView.SetSuppressAutoScroll(false);
        }

        /// <summary>
        /// Scrolls the native platform view to the specified position.
        /// </summary>
        /// <param name="parameters">The scroll request parameters.</param>
        void ScrollTo(ScrollToParameters parameters)
        {
            if (VirtualView.IsContentLayoutRequested == false)
            {
                int hOffset = (int)PlatformView.Context.ToPixels(parameters.ScrollX);
                int vOffset = (int)PlatformView.Context.ToPixels(parameters.ScrollY);

                PlatformView.ScrollToOffset(
                    hOffset,
                    vOffset,
                    parameters.Animated,
                    VirtualView.Orientation,
                    () => VirtualView.SendScrollFinished());
            }
            else
            {
                _scrollOffsetRequest = parameters;
            }
        }

        #endregion

        #region ScrollViewProxy class

        /// <summary>
        /// Provides proxy callbacks for native platform events while maintaining a weak reference to the associated handler.
        /// </summary>
        private sealed class SfScrollViewProxy
        {
            #region Fields

            /// <summary>
            /// Holds a weak reference to the associated scroll view handler.
            /// </summary>
            readonly WeakReference<SfInteractiveScrollViewHandler> _scrollViewHandler;

            #endregion

            #region Constructor

            /// <summary>
            /// Initializes a new instance of the <see cref="SfScrollViewProxy"/> class.
            /// </summary>
            /// <param name="scrollViewHandler">The associated scroll view handler.</param>
            internal SfScrollViewProxy(SfInteractiveScrollViewHandler scrollViewHandler)
            {
                _scrollViewHandler = new(scrollViewHandler);
            }

            #endregion

            #region Event Handlers

            /// <summary>
            /// Handles scroll change notifications from the native platform view.
            /// </summary>
            /// <param name="sender">The source of the event.</param>
            /// <param name="e">The scroll change event data.</param>
            internal void OnScrollChanged(object? sender, Android.Views.View.ScrollChangeEventArgs e)
            {
                _scrollViewHandler.TryGetTarget(out var view);
                if (view != null)
                {
                    double oldScrollX = view.Context.FromPixels(e.OldScrollX);
                    double oldScrollY = view.Context.FromPixels(e.OldScrollY);
                    double scrollX = view.Context.FromPixels(e.ScrollX);
                    double scrollY = view.Context.FromPixels(e.ScrollY);

                    ScrollChangedEventArgs scrolledEventArgs =
                        new ScrollChangedEventArgs(scrollX, scrollY, oldScrollX, oldScrollY);

                    view.VirtualView.OnScrollChanged(scrolledEventArgs);
                }
            }

            /// <summary>
            /// Handles layout changes of the native scroll viewer and updates the viewport dimensions of the virtual view.
            /// </summary>
            /// <param name="sender">The source of the event.</param>
            /// <param name="e">The layout change event data.</param>
            internal void OnLayoutChange(object? sender, Android.Views.View.LayoutChangeEventArgs e)
            {
                _scrollViewHandler.TryGetTarget(out var view);
                if (view != null)
                {
                    if (e.OldLeft != e.Left ||
                        e.OldTop != e.Top ||
                        e.OldRight != e.Right ||
                        e.OldBottom != e.Bottom)
                    {
                        view.VirtualView.ViewportWidth =
                            view.Context.FromPixels(view.PlatformView.ViewPortWidth);

                        view.VirtualView.ViewportHeight =
                            view.Context.FromPixels(view.PlatformView.ViewPortHeight);
                    }
                }
            }

            /// <summary>
            /// Handles pan gesture notifications from the native platform view.
            /// </summary>
            /// <param name="sender">The source of the event.</param>
            /// <param name="motionEvent">The native motion event associated with the pan gesture.</param>
            internal void OnPlatformViewPan(object? sender, MotionEvent motionEvent)
            {
                _scrollViewHandler.TryGetTarget(out var view);
                if (view != null)
                {
                    switch (motionEvent.Action)
                    {
                        case MotionEventActions.Move:
                            PanEventArgs pan = new(
                                GestureStatus.Running,
                                new Point(motionEvent.GetX(), motionEvent.GetY()),
                                new Point(),
                                new Point());

                            if (!view._isPanStarted)
                            {
                                pan = new PanEventArgs(
                                    GestureStatus.Started,
                                    new Point(motionEvent.GetX(), motionEvent.GetY()),
                                    new Point(),
                                    new Point());
                            }

                            view.VirtualView.OnPanUpdated(pan);
                            view._isPanStarted = true;
                            break;

                        case MotionEventActions.Outside:
                        case MotionEventActions.Cancel:
                        case MotionEventActions.Up:
                            if (view._isPanStarted)
                            {
                                PanEventArgs panEnded = new(
                                    GestureStatus.Completed,
                                    new Point(motionEvent.GetX(), motionEvent.GetY()),
                                    new Point(),
                                    new Point());

                                view.VirtualView.OnPanUpdated(panEnded);
                            }

                            view._isPanStarted = false;
                            break;
                    }
                }
            }

            /// <summary>
            /// Handles layout changes of the hosted content view and processes any pending scroll requests.
            /// </summary>
            /// <param name="sender">The source of the event.</param>
            /// <param name="e">The layout change event data.</param>
            internal void OnContentLayoutChange(object? sender, Android.Views.View.LayoutChangeEventArgs e)
            {
                _scrollViewHandler.TryGetTarget(out var view);
                if (view != null)
                {
                    if (view.VirtualView.IsContentLayoutRequested)
                    {
                        view.VirtualView.IsContentLayoutRequested = false;
                    }

                    if (view._scrollOffsetRequest != null)
                    {
                        view.ScrollTo(view._scrollOffsetRequest);
                        view._scrollOffsetRequest = null;
                    }
                }
            }

            #endregion
        }

        #endregion
    }
}