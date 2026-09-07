namespace Syncfusion.Maui.Toolkit.Internals
{
    using CoreGraphics;
    using Foundation;
    using System;
    using UIKit;

    /// <summary>
    /// Provides data for keyboard press events raised by the platform scroll view.
    /// </summary>
    internal class UIKeyEventArgs : EventArgs
    {
        #region Properties

        /// <summary>
        /// Gets the collection of key presses associated with the event.
        /// </summary>
        internal NSSet<UIPress> Presses { get; }

        /// <summary>
        /// Gets the platform-specific press event information.
        /// </summary>
        internal UIPressesEvent PressesEvent { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the event has been handled.
        /// </summary>
        internal bool Handled { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="UIKeyEventArgs"/> class.
        /// </summary>
        /// <param name="presses">The collection of key presses associated with the event.</param>
        /// <param name="pressesEvent">The platform-specific press event information.</param>
        internal UIKeyEventArgs(NSSet<UIPress> presses, UIPressesEvent pressesEvent)
        {
            Presses = presses;
            PressesEvent = pressesEvent;
        }

        #endregion
    }

    /// <summary>
    /// Represents the native iOS scroll view used by <see cref="SfInteractiveScrollView"/>.
    /// Provides scrolling functionality, layout notifications, keyboard press event handling, and responder management required by the interactive scroll view.
    /// </summary>
    internal partial class PlatformScrollViewer : UIScrollView
    {
        #region Fields

        /// <summary>
        /// Indicates whether the scroll view can become the first responder.
        /// </summary>
        bool _canBecomeFirstResponder;

        #endregion

        #region Events

        /// <summary>
        /// Occurs when keyboard presses begin.
        /// </summary>
        internal event EventHandler<UIKeyEventArgs>? KeyPressesBegan;

        /// <summary>
        /// Occurs when keyboard presses end.
        /// </summary>
        internal event EventHandler<UIKeyEventArgs>? KeyPressesEnded;

        /// <summary>
        /// Occurs when the layout of the scroll view changes.
        /// </summary>
        internal event EventHandler<EventArgs>? LayoutChanged;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="PlatformScrollViewer"/> class.
        /// </summary>
        internal PlatformScrollViewer()
        {
            Bounces = BouncesZoom = false;
            ContentInsetAdjustmentBehavior = UIScrollViewContentInsetAdjustmentBehavior.Always;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether the scroll view can become the first responder.
        /// </summary>
        public override bool CanBecomeFirstResponder => _canBecomeFirstResponder;

        #endregion

        #region Override Methods

        /// <summary>
        /// Layout the subviews and raises the <see cref="LayoutChanged"/> event.
        /// </summary>
        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            LayoutChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Handles keyboard press events when presses begin.
        /// </summary>
        /// <param name="presses">The collection of key presses associated with the event.</param>
        /// <param name="evt">The platform-specific press event information.</param>
        public override void PressesBegan(NSSet<UIPress> presses, UIPressesEvent evt)
        {
            if (KeyPressesBegan != null)
            {
                UIKeyEventArgs eventArgs = new UIKeyEventArgs(presses, evt);
                KeyPressesBegan.Invoke(this, eventArgs);

                if (eventArgs.Handled)
                {
                    return;
                }
            }

            base.PressesBegan(presses, evt);
        }

        /// <summary>
        /// Handles keyboard press events when presses end.
        /// </summary>
        /// <param name="presses">The collection of key presses associated with the event.</param>
        /// <param name="evt">The platform-specific press event information.</param>
        public override void PressesEnded(NSSet<UIPress> presses, UIPressesEvent evt)
        {
            if (KeyPressesEnded != null)
            {
                UIKeyEventArgs eventArgs = new UIKeyEventArgs(presses, evt);
                KeyPressesEnded.Invoke(this, eventArgs);

                if (eventArgs.Handled)
                {
                    return;
                }
            }

            base.PressesEnded(presses, evt);
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Sets a value indicating whether the scroll view can become the first responder.
        /// </summary>
        /// <param name="value"><see langword="true"/> to allow the scroll view to become the first responder; otherwise, <see langword="false"/>.
        /// </param>
        internal void SetCanBecomeFirstResponder(bool value)
        {
            _canBecomeFirstResponder = value;
        }

        /// <summary>
        /// Scrolls the content to the specified vertical offset.
        /// </summary>
        /// <param name="offset">The vertical scroll offset.</param>
        /// <param name="animated"><see langword="true"/> to animate the scroll operation; otherwise, <see langword="false"/>.</param>
        internal void ScrollToVerticalOffset(double offset, bool animated)
        {
            offset = GetMaxScrollOffset(offset, ContentSize.Height, Frame.Height);
            SetContentOffset(new CGPoint(ContentOffset.X, offset), animated);
        }

        /// <summary>
        /// Scrolls the content to the specified horizontal offset.
        /// </summary>
        /// <param name="offset">The horizontal scroll offset.</param>
        /// <param name="animated"><see langword="true"/> to animate the scroll operation; otherwise, <see langword="false"/>.</param>
        internal void ScrollToHorizontalOffset(double offset, bool animated)
        {
            offset = GetMaxScrollOffset(offset, ContentSize.Width, Frame.Width);
            SetContentOffset(new CGPoint(offset, ContentOffset.Y), animated);
        }

        /// <summary>
        /// Scrolls the content to the specified horizontal and vertical offsets.
        /// </summary>
        /// <param name="horizontalOffset">The horizontal scroll offset.</param>
        /// <param name="verticalOffset">The vertical scroll offset.</param>
        /// <param name="animated"><see langword="true"/> to animate the scroll operation; otherwise,<see langword="false"/>.</param>
        internal void ScrollTo(double horizontalOffset, double verticalOffset, bool animated)
        {
            horizontalOffset = GetMaxScrollOffset(horizontalOffset, ContentSize.Width, Frame.Width);
            verticalOffset = GetMaxScrollOffset(verticalOffset, ContentSize.Height, Frame.Height);
            SetContentOffset(new CGPoint(horizontalOffset, verticalOffset), animated);
        }

        /// <summary>
        /// Calculates the maximum valid scroll offset for the specified content dimension.
        /// </summary>
        /// <param name="offset">The requested scroll offset.</param>
        /// <param name="contentEnd">The total content width or height.</param>
        /// <param name="frameLength">The width or height of the viewport.</param>
        /// <returns>The adjusted scroll offset that does not exceed the scrollable range.</returns>
        double GetMaxScrollOffset(double offset, double contentEnd, double frameLength)
        {
            double maxScrollOffset = contentEnd - frameLength;
            return offset <= maxScrollOffset ? offset : maxScrollOffset;
        }

        #endregion
    }
}