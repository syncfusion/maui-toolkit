using Microsoft.Maui.Layouts;

namespace Syncfusion.Maui.ControlsGallery.Expander;

    /// <summary>
    /// Represents to add expander items to scroll view.
    /// </summary>
#if NET8_0_OR_GREATER && (IOS || MACCATALYST)
    internal class ExpanderScrollView : ScrollView, ICrossPlatformLayout
#else
    internal class ExpanderScrollView : ScrollView
#endif  
    {
        public ExpanderScrollView()
        {

        }

        #region Public Methods

#if NET8_0_OR_GREATER && (IOS || MACCATALYST)

        /// <summary>
        /// Override this method due to .net8 blank issue.
        /// </summary>
        /// <param name="bounds">Bounds for expander.</param>
        /// <returns>Size of scroll view.</returns>
        Size ICrossPlatformLayout.CrossPlatformArrange(Microsoft.Maui.Graphics.Rect bounds)
        {
            // Checked, from where expander bounds changes and override this method.
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

        #endregion
    }

