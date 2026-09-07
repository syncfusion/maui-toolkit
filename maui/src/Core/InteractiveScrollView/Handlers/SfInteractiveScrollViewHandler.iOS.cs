namespace Syncfusion.Maui.Toolkit.Internals
{
    using CoreGraphics;
    using Microsoft.Maui;
    using Microsoft.Maui.Controls;
    using Microsoft.Maui.Handlers;
    using Microsoft.Maui.Platform;
    using System;

    /// <summary>
    /// Provides the iOS and Mac Catalyst-specific handler implementation for <see cref="SfInteractiveScrollView"/>. This handler creates and manages the native platform scroll view, synchronizes virtual view properties and commands
    /// with their native counterparts, and coordinates platform-specific scrolling, keyboard interaction, content presentation, viewport updates, and scroll state notifications. It ensures consistent scrolling behavior and seamless
    /// integration between the cross-platform control and the underlying iOS and Mac platform scroll view.
    /// </summary>
    internal partial class SfInteractiveScrollViewHandler : ViewHandler<SfInteractiveScrollView, PlatformScrollViewer>
    {
        #region Fields

        /// <summary>
        /// Stores the content offset used when updating the scroll orientation.
        /// </summary>
        CGPoint _offsetOnOrientationChange;

        /// <summary>
        /// Holds the hosted .NET MAUI content view.
        /// </summary>
        View? _mauiView;

        /// <summary>
        /// Provides proxy callbacks for platform-specific scroll view events.
        /// </summary>
        SfScrollViewProxy? _proxy;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether the platform view can become the first responder.
        /// </summary>
        internal bool CanBecomeFirstResponder { get; set; }

        #endregion

        #region Override Methods

        /// <summary>
        /// Creates the native platform scroll viewer.
        /// </summary>
        /// <returns>The platform scroll viewer.</returns>
        protected override PlatformScrollViewer CreatePlatformView()
        {
            this._proxy = new(this);
            PlatformScrollViewer platformView = new PlatformScrollViewer();
            platformView.LayoutChanged += this._proxy.OnLayoutChanged;
            return platformView;
        }

        /// <summary>
        /// Connects the handler to the platform view.
        /// </summary>
        /// <param name="platformView">The platform view.</param>
        protected override void ConnectHandler(PlatformScrollViewer platformView)
        {
            base.ConnectHandler(platformView);

            if (_proxy != null)
            {
                platformView.KeyPressesBegan += this._proxy.OnKeyPressesBegan;
                platformView.KeyPressesEnded += this._proxy.OnKeyPressesEnded;
                platformView.Scrolled += this._proxy.OnScrolled;
                platformView.ScrollAnimationEnded += this._proxy.OnScrollAnimationEnded;
            }
        }

        /// <summary>
        /// Disconnects the handler from the platform view.
        /// </summary>
        /// <param name="platformView">The platform view.</param>
        protected override void DisconnectHandler(PlatformScrollViewer platformView)
        {
            if (this._proxy != null)
            {
                platformView.ScrollAnimationEnded -= this._proxy.OnScrollAnimationEnded;
                platformView.Scrolled -= this._proxy.OnScrolled;
                platformView.KeyPressesBegan -= this._proxy.OnKeyPressesBegan;
                platformView.KeyPressesEnded -= this._proxy.OnKeyPressesEnded;
                platformView.LayoutChanged -= this._proxy.OnLayoutChanged;
            }

            platformView.ClearSubviews();
            base.DisconnectHandler(platformView);
        }

        #endregion

        #region Property mapper methods

        /// <summary>
        /// Maps the <see cref="SfInteractiveScrollView.CanBecomeFirstResponder"/> property to the native platform view.
        /// </summary>
        /// <param name="handler">The handler associated with the interactive scroll view.</param>
        /// <param name="scrollView">The interactive scroll view.</param>
        internal static void MapCanBecomeFirstResponder(SfInteractiveScrollViewHandler handler, SfInteractiveScrollView scrollView)
        {
            if (handler.PlatformView == null)
                return;

            handler.PlatformView.SetCanBecomeFirstResponder(scrollView.CanBecomeFirstResponder);
        }

        /// <summary>
        /// Maps the <see cref="SfInteractiveScrollView.PresentedContent"/> property to the native platform scroll view.
        /// </summary>
        /// <param name="handler">The handler associated with the interactive scroll view.</param>
        /// <param name="scrollView">The interactive scroll view.</param>
        internal static void MapContent(SfInteractiveScrollViewHandler handler, SfInteractiveScrollView scrollView)
        {
            if (handler.PlatformView == null || handler.MauiContext == null)
                return;

            var platformScrollView = handler.PlatformView;
            if (scrollView.PresentedContent != null)
            {
                handler._mauiView = scrollView.PresentedContent;
                var nativeContent = scrollView.PresentedContent.ToPlatform(handler.MauiContext);
                platformScrollView.ClearSubviews();
                platformScrollView.AddSubview(nativeContent);
            }
        }

        /// <summary>
        /// Maps the <see cref="SfInteractiveScrollView.ContentSize"/> property to the native platform scroll view.
        /// </summary>
        /// <param name="handler">The handler associated with the interactive scroll view.</param>
        /// <param name="scrollView">The interactive scroll view.</param>
        internal static void MapContentSize(SfInteractiveScrollViewHandler handler, SfInteractiveScrollView scrollView)
        {
            if (handler.PlatformView == null)
                return;

            if (handler._proxy != null)
            {
                handler.PlatformView.Scrolled -= handler._proxy.OnScrolled;
                handler.PlatformView.UpdateContentSize(scrollView.ContentSize);
                handler.PlatformView.Scrolled += handler._proxy.OnScrolled;
            }
        }

        /// <summary>
        /// Maps the IsEnabled property to the native platform scroll view.
        /// </summary>
        /// <param name="handler">The handler associated with the interactive scroll view.</param>
        /// <param name="scrollView">The interactive scroll view.</param>
        internal static void MapIsEnabled(SfInteractiveScrollViewHandler handler, SfInteractiveScrollView scrollView)
        {
            if (handler.PlatformView == null)
                return;

            if (scrollView.IsEnabled)
            {
                handler.PlatformView.ScrollEnabled = scrollView.Orientation != ScrollOrientation.Neither;
                return;
            }

            handler.PlatformView.ScrollEnabled = scrollView.IsEnabled;
        }

        /// <summary>
        /// Maps the <see cref="SfInteractiveScrollView.HorizontalScrollBarVisibility"/> property to the native platform scroll view.
        /// </summary>
        /// <param name="handler">The handler associated with the interactive scroll view.</param>
        /// <param name="scrollView">The interactive scroll view.</param>
        internal static void MapHorizontalScrollBarVisibility(SfInteractiveScrollViewHandler handler, SfInteractiveScrollView scrollView)
        {
            handler.PlatformView?.UpdateHorizontalScrollBarVisibility(scrollView.HorizontalScrollBarVisibility);
        }

        /// <summary>
        /// Maps the <see cref="SfInteractiveScrollView.VerticalScrollBarVisibility"/> property to the native platform scroll view.
        /// </summary>
        /// <param name="handler">The handler associated with the interactive scroll view.</param>
        /// <param name="scrollView">The interactive scroll view.</param>
        internal static void MapVerticalScrollBarVisibility(SfInteractiveScrollViewHandler handler, SfInteractiveScrollView scrollView)
        {
            handler.PlatformView?.UpdateVerticalScrollBarVisibility(scrollView.VerticalScrollBarVisibility);
        }

        /// <summary>
        /// Maps the <see cref="SfInteractiveScrollView.Orientation"/> property to the native platform scroll view and updates scrolling behavior and scroll bar visibility.
        /// </summary>
        /// <param name="handler">The handler associated with the interactive scroll view.</param>
        /// <param name="scrollView">The interactive scroll view.</param>
        internal static void MapScrollOrientation(SfInteractiveScrollViewHandler handler, SfInteractiveScrollView scrollView)
        {
            if (handler.PlatformView == null)
                return;

            handler._offsetOnOrientationChange = handler.PlatformView.ContentOffset;
            handler.PlatformView.ScrollEnabled = scrollView.Orientation != ScrollOrientation.Neither;
            handler.UpdateScrollBarVisibility();
        }

        /// <summary>
        /// Maps a scroll request from the <see cref="SfInteractiveScrollView"/> to the native platform scroll view.
        /// </summary>
        /// <param name="handler">The handler associated with the interactive scroll view.</param>
        /// <param name="scrollView">The interactive scroll view.</param>
        /// <param name="args">The scroll request parameters.</param>
        internal static void MapScrollTo(SfInteractiveScrollViewHandler handler, SfInteractiveScrollView scrollView, object? args)
        {
            if (handler.PlatformView == null)
                return;

            if (args is ScrollToParameters parameters)
            {
                handler.PlatformView.ScrollTo(parameters.ScrollX, parameters.ScrollY, parameters.Animated);

                // Sometimes, the scrolled event does not occur after the above ScrollTo call
                // when the size is reduced. The following fallback condition is used.
                if (scrollView.ScrollX != handler.PlatformView.ContentOffset.X || scrollView.ScrollY != handler.PlatformView.ContentOffset.Y)
                {
                    ScrollChangedEventArgs scrolledEventArgs = new ScrollChangedEventArgs(
                        handler.PlatformView.ContentOffset.X,
                        handler.PlatformView.ContentOffset.Y,
                        scrollView.ScrollX,
                        scrollView.ScrollY);

                    scrollView.OnScrollChanged(scrolledEventArgs);
                }

                if (parameters.Animated == false)
                    handler.VirtualView.SendScrollFinished();
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Updates the scroll bar visibility based on the current orientation.
        /// </summary>
        void UpdateScrollBarVisibility()
        {
            switch (VirtualView.Orientation)
            {
                case ScrollOrientation.Neither:
                    PlatformView?.UpdateVerticalScrollBarVisibility(ScrollBarVisibility.Never);
                    PlatformView?.UpdateHorizontalScrollBarVisibility(ScrollBarVisibility.Never);
                    break;

                case ScrollOrientation.Vertical:
                    PlatformView?.UpdateVerticalScrollBarVisibility(VirtualView.VerticalScrollBarVisibility);
                    PlatformView?.UpdateHorizontalScrollBarVisibility(ScrollBarVisibility.Never);
                    break;

                case ScrollOrientation.Horizontal:
                    PlatformView?.UpdateVerticalScrollBarVisibility(ScrollBarVisibility.Never);
                    PlatformView?.UpdateHorizontalScrollBarVisibility(VirtualView.HorizontalScrollBarVisibility);
                    break;

                case ScrollOrientation.Both:
                    PlatformView?.UpdateVerticalScrollBarVisibility(VirtualView.VerticalScrollBarVisibility);
                    PlatformView?.UpdateHorizontalScrollBarVisibility(VirtualView.HorizontalScrollBarVisibility);
                    break;
            }
        }

        #endregion

        #region ScrollViewProxy Class

        /// <summary>
        /// Provides proxy callbacks for platform-specific scroll view events.
        /// </summary>
        private class SfScrollViewProxy
        {
            #region Fields

            /// <summary>
            /// Holds a weak reference to the scroll view handler.
            /// </summary>
            readonly WeakReference<SfInteractiveScrollViewHandler> scrollViewLayout;

            #endregion

            #region Constructor

            /// <summary>
            /// Initializes a new instance of the <see cref="SfScrollViewProxy"/> class.
            /// </summary>
            /// <param name="scrollView">The scroll view handler.</param>
            internal SfScrollViewProxy(SfInteractiveScrollViewHandler scrollView)
            {
                this.scrollViewLayout = new(scrollView);
            }

            #endregion

            #region Internal Methods

            /// <summary>
            /// Handles the scroll animation completed event.
            /// </summary>
            /// <param name="sender">The sender.</param>
            /// <param name="e">The event args.</param>
            internal void OnScrollAnimationEnded(object? sender, EventArgs e)
            {
                this.scrollViewLayout.TryGetTarget(out var view);
                view?.VirtualView?.SendScrollFinished();
            }

            /// <summary>
            /// Handles the layout changed events.
            /// </summary>
            /// <param name="sender">The sender.</param>
            /// <param name="e">The event args.</param>
            internal void OnLayoutChanged(object? sender, EventArgs e)
            {
                this.scrollViewLayout.TryGetTarget(out var view);
                if (view != null)
                {
                    if (view.PlatformView == null || view.VirtualView == null)
                        return;

                    view.VirtualView.ViewportWidth = view.PlatformView.Frame.Width;
                    view.VirtualView.ViewportHeight = view.PlatformView.Frame.Height;
                }
            }

            /// <summary>
            /// Handles key release events.
            /// </summary>
            /// <param name="sender">The sender.</param>
            /// <param name="e">The key event args.</param>
            internal void OnKeyPressesEnded(object? sender, UIKeyEventArgs e)
            {
                this.scrollViewLayout.TryGetTarget(out var view);
                if (view != null)
                {
                    if (view._mauiView != null)
                        e.Handled = view._mauiView.HandleKeyRelease(e.Presses, e.PressesEvent);

                    if (e.Handled == false)
                        view.VirtualView?.HandleKeyRelease(e.Presses, e.PressesEvent);
                }
            }

            /// <summary>
            /// Handles key press events.
            /// </summary>
            /// <param name="sender">The sender.</param>
            /// <param name="e">The key event args.</param>
            internal void OnKeyPressesBegan(object? sender, UIKeyEventArgs e)
            {
                this.scrollViewLayout.TryGetTarget(out var view);
                if (view != null)
                {
                    if (view._mauiView != null)
                        e.Handled = view._mauiView.HandleKeyPress(e.Presses, e.PressesEvent);

                    if (e.Handled == false)
                        view.VirtualView?.HandleKeyPress(e.Presses, e.PressesEvent);
                }
            }

            /// <summary>
            /// Handles scroll position changes and notifies the virtual view.
            /// </summary>
            /// <param name="sender">The sender.</param>
            /// <param name="e">The event args.</param>
            internal void OnScrolled(object? sender, EventArgs e)
            {
                this.scrollViewLayout.TryGetTarget(out var view);
                if (view != null)
                {
                    if (view.PlatformView == null || view.VirtualView == null)
                        return;

                    if (view.VirtualView.Orientation == ScrollOrientation.Horizontal)
                    {
                        view.PlatformView.ContentOffset =
                            new CGPoint(view.PlatformView.ContentOffset.X, view._offsetOnOrientationChange.Y);
                    }
                    else if (view.VirtualView.Orientation == ScrollOrientation.Vertical)
                    {
                        view.PlatformView.ContentOffset =
                            new CGPoint(view._offsetOnOrientationChange.X, view.PlatformView.ContentOffset.Y);
                    }

                    if (view.PlatformView.ContentOffset.X < 0)
                    {
                        view.PlatformView.ContentOffset =
                            new CGPoint(0, view.PlatformView.ContentOffset.Y);
                    }

                    if (view.PlatformView.ContentOffset.Y < 0)
                    {
                        view.PlatformView.ContentOffset =
                            new CGPoint(view.PlatformView.ContentOffset.X, 0);
                    }

                    ScrollChangedEventArgs scrolledEventArgs =
                        new ScrollChangedEventArgs(
                            view.PlatformView.ContentOffset.X,
                            view.PlatformView.ContentOffset.Y,
                            view.VirtualView.ScrollX,
                            view.VirtualView.ScrollY);

                    view.VirtualView.OnScrollChanged(scrolledEventArgs);
                }
            }

            #endregion
        }

        #endregion
    }
}