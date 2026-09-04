namespace Syncfusion.Maui.Toolkit.Internals
{
    using System;

    /// <summary>
    /// Represents the scroll position and animation settings used for programmatic scrolling operations.
    /// </summary>
    internal class ScrollToParameters
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ScrollToParameters"/> class.
        /// </summary>
        /// <param name="scrollX">The horizontal scroll position.</param>
        /// <param name="scrollY">The vertical scroll position.</param>
        /// <param name="animated">A value indicating whether scrolling is animated.</param>
        internal ScrollToParameters(double scrollX, double scrollY, bool animated = false)
        {
            ScrollX = scrollX;
            ScrollY = scrollY;
            Animated = animated;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the vertical scroll offset.
        /// </summary>
        internal double ScrollY { get; }

        /// <summary>
        /// Gets the horizontal scroll offset.
        /// </summary>
        internal double ScrollX { get; }

        /// <summary>
        /// Gets a value indicating whether scrolling is animated.
        /// </summary>
        internal bool Animated { get; }

        #endregion
    }

    /// <summary>
    /// Provides data for the <see cref="SfInteractiveScrollView.ScrollChanged"/> event.
    /// </summary>
    internal class ScrollChangedEventArgs : EventArgs
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ScrollChangedEventArgs"/> class.
        /// </summary>
        /// <param name="scrollX">The current horizontal scroll position.</param>
        /// <param name="scrollY">The current vertical scroll position.</param>
        /// <param name="oldScrollX">The previous horizontal scroll position.</param>
        /// <param name="oldScrollY">The previous vertical scroll position.</param>
        internal ScrollChangedEventArgs(double scrollX, double scrollY, double oldScrollX, double oldScrollY)
        {
            OldScrollX = oldScrollX;
            OldScrollY = oldScrollY;
            ScrollX = scrollX;
            ScrollY = scrollY;
            HorizontalChange = scrollX - oldScrollX;
            VerticalChange = scrollY - oldScrollY;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the change in the vertical scroll offset.
        /// </summary>
        internal double VerticalChange { get; private set; }

        /// <summary>
        /// Gets the change in the horizontal scroll offset.
        /// </summary>
        internal double HorizontalChange { get; private set; }

        /// <summary>
        /// Gets the previous horizontal scroll offset.
        /// </summary>
        internal double OldScrollX { get; }

        /// <summary>
        /// Gets the previous vertical scroll offset.
        /// </summary>
        internal double OldScrollY { get; }

        /// <summary>
        /// Gets the current horizontal scroll offset.
        /// </summary>
        internal double ScrollX { get; }

        /// <summary>
        /// Gets the current vertical scroll offset.
        /// </summary>
        internal double ScrollY { get; }

        #endregion
    }
}