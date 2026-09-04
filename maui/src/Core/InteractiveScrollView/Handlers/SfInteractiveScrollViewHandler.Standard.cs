namespace Syncfusion.Maui.Toolkit.Internals
{
    using Microsoft.Maui.Handlers;
    using System;

    /// <summary>
    /// Provides the platform-specific handler implementation for <see cref="SfInteractiveScrollView"/>.
    /// </summary>
    internal partial class SfInteractiveScrollViewHandler : ViewHandler<SfInteractiveScrollView, object>
    {
        #region Handler Lifecycle

        /// <summary>
        /// Creates and returns the native platform view.
        /// </summary>
        /// <returns>The native platform view.</returns>
        protected override object CreatePlatformView()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Property Mapper Methods

        /// <summary>
        /// Maps the content of the virtual view to the platform view.
        /// </summary>
        /// <param name="handler">The handler instance.</param>
        /// <param name="scrollView">The associated interactive scroll view.</param>
        internal static void MapContent(
            SfInteractiveScrollViewHandler handler,
            SfInteractiveScrollView scrollView)
        {
        }

        /// <summary>
        /// Maps the horizontal scroll bar visibility setting to the platform view.
        /// </summary>
        /// <param name="handler">The handler instance.</param>
        /// <param name="scrollView">The associated interactive scroll view.</param>
        internal static void MapHorizontalScrollBarVisibility(
            SfInteractiveScrollViewHandler handler,
            SfInteractiveScrollView scrollView)
        {
        }

        /// <summary>
        /// Maps the vertical scroll bar visibility setting to the platform view.
        /// </summary>
        /// <param name="handler">The handler instance.</param>
        /// <param name="scrollView">The associated interactive scroll view.</param>
        internal static void MapVerticalScrollBarVisibility(
            SfInteractiveScrollViewHandler handler,
            SfInteractiveScrollView scrollView)
        {
        }

        /// <summary>
        /// Maps the <see cref="SfInteractiveScrollView.Orientation"/> property to the platform view.
        /// </summary>
        /// <param name="handler">The handler instance.</param>
        /// <param name="scrollView">The associated interactive scroll view.</param>
        internal static void MapScrollOrientation(
            SfInteractiveScrollViewHandler handler,
            SfInteractiveScrollView scrollView)
        {
        }

        /// <summary>
        /// Maps the <see cref="SfInteractiveScrollView.SuppressAutoScroll"/> property to the platform view.
        /// </summary>
        /// <param name="handler">The handler instance.</param>
        /// <param name="scrollView">The associated interactive scroll view.</param>
        internal static void MapSuppressAutoScroll(
            SfInteractiveScrollViewHandler handler,
            SfInteractiveScrollView scrollView)
        {
        }

        /// <summary>
        /// Maps a scroll request from the virtual view to the platform view.
        /// </summary>
        /// <param name="handler">The handler instance.</param>
        /// <param name="scrollView">The associated interactive scroll view.</param>
        /// <param name="args">The scroll request parameters.</param>
        internal static void MapScrollTo(
            SfInteractiveScrollViewHandler handler,
            SfInteractiveScrollView scrollView,
            object? args)
        {
        }

        #endregion
    }
}