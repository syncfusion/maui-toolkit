namespace Syncfusion.Maui.Toolkit.Internals
{
    using Microsoft.Maui;
    using Microsoft.Maui.Handlers;
    using Microsoft.Maui.Platform;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Input;
    using WScrollBarVisibility = Microsoft.UI.Xaml.Controls.ScrollBarVisibility;
    using ScrollBarVisibility = Microsoft.Maui.ScrollBarVisibility;
    using PlatformScrollViewer = Microsoft.UI.Xaml.Controls.ScrollViewer;
    using ScrollMode = Microsoft.UI.Xaml.Controls.ScrollMode;
    using System;

    /// <summary>
    /// Provides the Windows-specific handler implementation for <see cref="SfInteractiveScrollView"/>.
    /// This handler creates and manages the native Windows scroll viewer, maps virtual view properties and coordinates scrolling, panning,
    /// content layout updates, and platform-specific interactions to ensure consistent behavior across the Windows platform.
    /// </summary>
    internal partial class SfInteractiveScrollViewHandler : ViewHandler<SfInteractiveScrollView, PlatformScrollViewer>
    {
        #region Fields

        /// <summary>
        /// Stores a pending scroll request until content layout is completed.
        /// </summary>
        ScrollToParameters? _scrollOffsetRequest;

        /// <summary>
        /// Holds the native content element.
        /// </summary>
        FrameworkElement? _content;

        /// <summary>
        /// Stores the display DPI value.
        /// </summary>
        double _dpi = 96;

        #endregion

        #region Override methods

        /// <summary>
        /// Creates the native platform scroll viewer.
        /// </summary>
        /// <returns>The platform scroll viewer.</returns>
        protected override PlatformScrollViewer CreatePlatformView()
        {
            PlatformScrollViewer? scrollViewer = new PlatformScrollViewer();
            scrollViewer.Loaded += OnLoaded;
            return scrollViewer;
        }

        /// <summary>
        /// Connects the handler to the platform view.
        /// </summary>
        /// <param name="platformView">The platform view.</param>
        protected override void ConnectHandler(PlatformScrollViewer platformView)
        {
            base.ConnectHandler(platformView);
            platformView.ViewChanged += OnViewChanged;
            platformView.KeyDown += OnKeyDown;
            platformView.KeyUp += OnKeyUp;
            platformView.ManipulationInertiaStarting += OnManipulationInertiaStarting;
            platformView.EffectiveViewportChanged += OnEffectiveViewportChanged;
        }

        /// <summary>
        /// Disconnects the handler from the platform view.
        /// </summary>
        /// <param name="platformView">The platform view.</param>
        protected override void DisconnectHandler(PlatformScrollViewer platformView)
        {
            if (_content != null)
                _content.SizeChanged -= OnContentSizeChanged;

            platformView.Loaded -= OnLoaded;
            platformView.KeyDown -= OnKeyDown;
            platformView.KeyUp -= OnKeyUp;
            platformView.ViewChanged -= OnViewChanged;
            platformView.EffectiveViewportChanged -= OnEffectiveViewportChanged;
            platformView.ManipulationInertiaStarting -= OnManipulationInertiaStarting;
            platformView.Content = null;
            base.DisconnectHandler(platformView);
        }

        #endregion

        #region Mapping methods

        /// <summary>
        /// Maps the <see cref="SfInteractiveScrollView.Orientation"/> property to the platform view.
        /// </summary>
        /// <param name="handler">The handler instance.</param>
        /// <param name="scrollView">The associated interactive scroll view.</param>
        internal static void MapScrollOrientation(SfInteractiveScrollViewHandler handler, SfInteractiveScrollView scrollView)
        {
            handler.UpdateScrollOrientation();
        }

        /// <summary>
        /// Maps the horizontal scroll bar visibility setting to the platform view.
        /// </summary>
        /// <param name="handler">The handler instance.</param>
        /// <param name="scrollView">The associated interactive scroll view.</param>
        internal static void MapHorizontalScrollBarVisibility(SfInteractiveScrollViewHandler handler, SfInteractiveScrollView scrollView)
        {
            if (scrollView.Orientation == ScrollOrientation.Neither ||
                scrollView.Orientation == ScrollOrientation.Vertical)
                return;

            handler.PlatformView.HorizontalScrollBarVisibility = handler.GetWScrollBarVisibility(scrollView.HorizontalScrollBarVisibility);
        }

        /// <summary>
        /// Maps the vertical scroll bar visibility setting to the platform view.
        /// </summary>
        /// <param name="handler">The handler instance.</param>
        /// <param name="scrollView">The associated interactive scroll view.</param>
        internal static void MapVerticalScrollBarVisibility(SfInteractiveScrollViewHandler handler, SfInteractiveScrollView scrollView)
        {
            if (scrollView.Orientation == ScrollOrientation.Neither ||
                scrollView.Orientation == ScrollOrientation.Horizontal)
                return;

            handler.PlatformView.VerticalScrollBarVisibility = handler.GetWScrollBarVisibility(scrollView.VerticalScrollBarVisibility);
        }

        /// <summary>
        /// Maps a scroll request from the virtual view to the platform view.
        /// </summary>
        /// <param name="handler">The handler instance.</param>
        /// <param name="scrollView">The associated interactive scroll view.</param>
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
        /// Maps the <see cref="SfInteractiveScrollView.ContentSize"/> property to the native platform scroll view.
        /// </summary>
        /// <param name="handler">The handler associated with the interactive scroll view.</param>
        /// <param name="scrollView">The interactive scroll view.</param>
        internal static void MapContentSize(SfInteractiveScrollViewHandler handler, SfInteractiveScrollView scrollView)
        {
            if (handler._content == null)
                return;

            if (scrollView.ContentSize.IsZero || (Math.Round(scrollView.ContentSize.Width) == Math.Round(handler._content.ActualWidth) &&
                Math.Round(scrollView.ContentSize.Height) == Math.Round(handler._content.ActualHeight)))
            {
                scrollView.IsContentLayoutRequested = false;
                return;
            }

            scrollView.IsContentLayoutRequested = true;
        }

        /// <summary>
        /// Maps the content of the virtual view to the platform view.
        /// </summary>
        /// <param name="handler">The handler instance.</param>
        /// <param name="scrollView">The associated interactive scroll view.</param>
        internal static void MapContent(SfInteractiveScrollViewHandler handler, SfInteractiveScrollView scrollView)
        {
            if (handler.PlatformView == null || handler.MauiContext == null)
                return;

            View? content = scrollView.PresentedContent;
            if (content != null)
            {
                var platformElement = content.ToPlatform(handler.MauiContext);
                handler._content = platformElement;
                // Enable keyboard shortcuts, that allows the default scroll behavior based on key inputs.
                platformElement.IsTabStop = true;
                platformElement.SizeChanged += handler.OnContentSizeChanged;
                handler.PlatformView.Content = platformElement;
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Updates the scroll orientation settings.
        /// </summary>
        void UpdateScrollOrientation()
        {
            switch (VirtualView.Orientation)
            {
                case ScrollOrientation.Neither:
                    SetScrollBarVisibility(WScrollBarVisibility.Hidden, WScrollBarVisibility.Hidden);
                    SetScrollMode(ScrollMode.Disabled, ScrollMode.Disabled);
                    break;
                case ScrollOrientation.Vertical:
                    SetScrollBarVisibility(WScrollBarVisibility.Hidden, GetWScrollBarVisibility(VirtualView.VerticalScrollBarVisibility));
                    SetScrollMode(ScrollMode.Disabled, ScrollMode.Enabled);
                    break;
                case ScrollOrientation.Horizontal:
                    SetScrollBarVisibility(GetWScrollBarVisibility(VirtualView.HorizontalScrollBarVisibility), WScrollBarVisibility.Hidden);
                    SetScrollMode(ScrollMode.Enabled, ScrollMode.Disabled);
                    break;
                case ScrollOrientation.Both:
                    SetScrollBarVisibility(GetWScrollBarVisibility(VirtualView.HorizontalScrollBarVisibility),
                        GetWScrollBarVisibility(VirtualView.VerticalScrollBarVisibility));
                    SetScrollMode(ScrollMode.Enabled, ScrollMode.Enabled);
                    break;
            }
        }

        /// <summary>
        /// Sets the scroll bar visibility.
        /// </summary>
        /// <param name="hScrollBarVisibility">The horizontal scroll bar visibility.</param>
        /// <param name="vScrollBarVisibility">The vertical scroll bar visibility.</param>
        void SetScrollBarVisibility(WScrollBarVisibility hScrollBarVisibility, WScrollBarVisibility vScrollBarVisibility)
        {
            PlatformView.HorizontalScrollBarVisibility = hScrollBarVisibility;
            PlatformView.VerticalScrollBarVisibility = vScrollBarVisibility;
        }

        /// <summary>
        /// Sets the scroll mode.
        /// </summary>
        /// <param name="horizontalScrollMode">The horizontal scroll mode.</param>
        /// <param name="verticalScrollMode">The vertical scroll mode.</param>
        void SetScrollMode(ScrollMode horizontalScrollMode, ScrollMode verticalScrollMode)
        {
            PlatformView.HorizontalScrollMode = horizontalScrollMode;
            PlatformView.VerticalScrollMode = verticalScrollMode;
        }

        /// <summary>
        /// Gets the Windows scroll bar visibility based on the provided scroll bar visibility.
        /// </summary>
        /// <param name="scrollBarVisibility">The scroll bar visibility.</param>
        /// <returns>The Windows scroll bar visibility.</returns>
        WScrollBarVisibility GetWScrollBarVisibility(ScrollBarVisibility scrollBarVisibility)
        {
            switch (scrollBarVisibility)
            {
                case ScrollBarVisibility.Always:
                    return WScrollBarVisibility.Visible;
                case ScrollBarVisibility.Never:
                    return WScrollBarVisibility.Hidden;
                default:
                    return WScrollBarVisibility.Auto;
            }
        }

        /// <summary>
        /// Scrolls the content to the specified position.
        /// </summary>
        /// <param name="parameters">The scroll parameters.</param>
        void ScrollTo(ScrollToParameters parameters)
        {
            if (VirtualView.IsContentLayoutRequested == false)
            {
                if (parameters.ScrollX != PlatformView.HorizontalOffset || parameters.ScrollY != PlatformView.VerticalOffset)
                {
                    PlatformView.ChangeView(parameters.ScrollX, parameters.ScrollY, null, !parameters.Animated);
                }

                ScrollChangedEventArgs scrolledEventArgs = new ScrollChangedEventArgs(
                    parameters.ScrollX != PlatformView.HorizontalOffset ? parameters.ScrollX : PlatformView.HorizontalOffset,
                    parameters.ScrollY != PlatformView.VerticalOffset ? parameters.ScrollY : PlatformView.VerticalOffset,
                    VirtualView.ScrollX,
                    VirtualView.ScrollY);
                VirtualView.OnScrollChanged(scrolledEventArgs);
                _scrollOffsetRequest = null;
                return;
            }

            _scrollOffsetRequest = parameters;
        }

        /// <summary>
        /// Handles content size changed events.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        void OnContentSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (VirtualView.IsContentLayoutRequested)
                VirtualView.IsContentLayoutRequested = false;

            if (_scrollOffsetRequest != null)
            {
                ScrollTo(_scrollOffsetRequest);
                _scrollOffsetRequest = null;
            }
        }

        /// <summary>
        /// Handles effective viewport changed events.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The event args.</param>
        void OnEffectiveViewportChanged(FrameworkElement sender, EffectiveViewportChangedEventArgs args)
        {
            VirtualView.ViewportWidth = PlatformView.ViewportWidth;
            VirtualView.ViewportHeight = PlatformView.ViewportHeight;
        }

        /// <summary>
        /// Handles manipulation inertia starting events.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        void OnManipulationInertiaStarting(object sender, ManipulationInertiaStartingRoutedEventArgs e)
        {
            if (VirtualView == null || VirtualView.Orientation == ScrollOrientation.Neither)
                return;

            // NOTE: This calculation is made practicaly with user experience, since there is no reference
            // for the correct calculations is obtained for Windows. This is tested with different resolutions.
            // The friction factor is a manual constant, tested with multiple resolutions.
            double frictionFactor = 8;
            double minFlingVelocity = 0.25;
            double flingDistanceX = PlatformView.HorizontalOffset;
            double flingDistanceY = PlatformView.VerticalOffset;

            // Squaring the value, since the fling grows exponentially.
            double velocityX = Math.Pow(e.Velocities.Linear.X, 2);
            double velocityY = Math.Pow(e.Velocities.Linear.Y, 2);

            double signX = e.Velocities.Linear.X / Math.Abs(e.Velocities.Linear.X);
            double signY = e.Velocities.Linear.Y / Math.Abs(e.Velocities.Linear.Y);

            if (velocityX < minFlingVelocity && velocityY < minFlingVelocity)
                return;

            if (VirtualView.Orientation != ScrollOrientation.Vertical)
            {
                if (velocityX >= minFlingVelocity)
                {
                    flingDistanceX += (-signX * velocityX * _dpi * frictionFactor);
                }
            }

            if (VirtualView.Orientation != ScrollOrientation.Horizontal)
            {
                if (velocityY >= minFlingVelocity)
                {
                    flingDistanceY += (-signY * velocityY * _dpi * frictionFactor);
                }
            }

            PlatformView.ChangeView(flingDistanceX, flingDistanceY, null);
            VirtualView.IsScrolling = true;
            e.Handled = true;
        }

        /// <summary>
        /// Handles key release events.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The key event args.</param>
        void OnKeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Shift)
            {
                // Restore the original scroll mode which was before the shift key was pressed.
                UpdateScrollOrientation();
            }
        }

        /// <summary>
        /// Handles key press events.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The key event args.</param>
        void OnKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Shift)
            {
                PlatformView.VerticalScrollMode = ScrollMode.Disabled;
                PlatformView.VerticalScrollBarVisibility = WScrollBarVisibility.Hidden;
            }
        }

        /// <summary>
        /// Handles the loaded event of the platform scroll viewer.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (sender is PlatformScrollViewer platformScrollViewer)
            {
                _dpi = platformScrollViewer.XamlRoot.RasterizationScale * 96;
            }
        }

        /// <summary>
        /// Handles the view changed events.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The scroll viewer view changed event args.</param>
        void OnViewChanged(object? sender, ScrollViewerViewChangedEventArgs e)
        {
            if (VirtualView.IsContentLayoutRequested == true)
                return;

            ScrollChangedEventArgs scrolledEventArgs = new ScrollChangedEventArgs(PlatformView.HorizontalOffset, PlatformView.VerticalOffset, VirtualView.ScrollX, VirtualView.ScrollY);
            VirtualView.OnScrollChanged(scrolledEventArgs);

            if (!e.IsIntermediate)
            {
                VirtualView.SendScrollFinished();
                VirtualView.IsScrolling = false;
                return;
            }

            VirtualView.IsScrolling = true;
        }

        #endregion
    }
}