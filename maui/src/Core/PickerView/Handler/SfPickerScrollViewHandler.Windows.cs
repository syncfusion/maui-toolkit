using Microsoft.Maui.Handlers;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

namespace Syncfusion.Maui.Toolkit.Internals
{
    /// <summary>
    /// The ScrollViewHandler for <see cref="SfPickerView"/>.
    /// </summary>
    internal partial class SfPickerScrollViewHandler : ScrollViewHandler
    {
        #region Private Methods

        /// <summary>
        /// Raised when manipulation such like scroll action.
        /// </summary>
        /// <param name="sender">Instance of nativeView.</param>
        /// <param name="e">Arguments corresponding to <see cref="ScrollViewer"/> ViewChange.</param>
        void OnPlatformViewChanged(object? sender, ScrollViewerViewChangedEventArgs e)
        {
            //// The IsIntermediate is false then the scroll is ended.
            if (!e.IsIntermediate)
            {
                SfPickerView? scrollLayout = VirtualView as SfPickerView;
                ScrollViewer? nativeScrollView = sender as ScrollViewer;
                if (nativeScrollView == null || scrollLayout == null)
                {
                    return;
                }

                scrollLayout.OnPickerViewScrollEnd(nativeScrollView.VerticalOffset);
            }
        }

        /// <summary>
        /// Prevents the keyboard key press by setting the e.handled value as true.
        /// </summary>
        /// <param name="sender">Instance of nativeView. </param>
        /// <param name="e">Arguments of key routed event args. </param>
        void OnScrollViewKeyBoardKeyPressed(object sender, KeyRoutedEventArgs e)
        {
            SfPickerView? scrollLayout = VirtualView as SfPickerView;
            scrollLayout?.HandleScrollViewKeyPress(e);
        }

        #endregion

        #region Override Methods

        /// <summary>
        /// Connects the handler.
        /// </summary>
        /// <param name="platformView">Instance of platformView.</param>
        protected override void ConnectHandler(ScrollViewer platformView)
        {
            //// Using this event to identity the scroll view scroll is completed/end.
            platformView.ViewChanged += OnPlatformViewChanged;
            //// Using this event we can prevent the keyboard key press.
            platformView.PreviewKeyDown += OnScrollViewKeyBoardKeyPressed;
            base.ConnectHandler(platformView);
        }

        /// <summary>
        /// Disconnects the handler.
        /// </summary>
        /// <param name="platformView">Instance of platform view.</param>
        protected override void DisconnectHandler(ScrollViewer platformView)
        {
            platformView.ViewChanged -= OnPlatformViewChanged;
            platformView.PreviewKeyDown -= OnScrollViewKeyBoardKeyPressed;
            base.DisconnectHandler(platformView);
        }

        #endregion
    }
}