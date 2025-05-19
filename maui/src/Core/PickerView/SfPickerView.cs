using Microsoft.Maui.Layouts;

namespace Syncfusion.Maui.Toolkit.Internals
{
    /// <summary>
    /// Represents an abstract base class for <see cref="SfPickerView"/>.
    /// </summary>
#if NET8_0_OR_GREATER && (IOS || MACCATALYST)
    internal abstract class SfPickerView : ScrollView, ICrossPlatformLayout
#else
    internal abstract class SfPickerView : ScrollView
#endif
    {
        /// <summary>
        /// Called when the picker view scroll ends.
        /// </summary>
        /// <param name="scrollEndPosition">The scroll end position.</param>
        internal abstract void OnPickerViewScrollEnd(double scrollEndPosition);

        /// <summary>
        /// Called when the picker view scroll starts.
        /// </summary>
        internal abstract void OnPickerViewScrollStart();

        //// We are virtualized the SfPickerView control, the children is arranged based on the scrolled position. The scroll view content is SfView. The SfView content is SfDrawableView. The SfDrawableView content is SfPickerItems.
        //// In this implementation, the blank issue is occurred while scrolling the SfPickerView.
        //// While scrolling, the children is not arranged based on the scrolled position. So that rendering is blank.
        //// This issue is not reproducible in .net6.0 and .net7.0. So, we have overridden the CrossPlatformArrange method to resolve this issue.
#if NET8_0_OR_GREATER && (IOS || MACCATALYST)
        /// <summary>
        /// Overdried this method due to .net8 blank issue.
        /// </summary>
        /// <returns>Size of scroll view.</returns>
        Size ICrossPlatformLayout.CrossPlatformArrange(Rect bounds)
        {
            // 855697 - Checked, from where container bounds changes and overdried this method.
            // from ScrollViewHandler ICrossPlatformLayout.CrossPlatformArrange native bounds used instead of frame.
            if (this is IScrollView scrollView)
            {
                bounds.X = 0;
                bounds.Y = 0;
                return scrollView.ArrangeContentUnbounded(bounds);
            }

            return bounds.Size;
        }
#endif

#if WINDOWS
        /// <summary>
        /// Handles key press events for the scroll view on Windows platforms.
        /// </summary>
        /// <param name="e">The key routed event arguments.</param>
        internal virtual void HandleScrollViewKeyPress(Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
        }
#endif
    }
}