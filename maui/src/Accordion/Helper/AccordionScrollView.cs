using Microsoft.Maui.Layouts;

namespace Syncfusion.Maui.Toolkit.Accordion
{
    /// <summary>
    /// Represents a specialized <see cref="ScrollView"/> for displaying accordion items.
    /// This view facilitates the scrolling functionality for accordion controls.
    /// </summary>
#if NET8_0_OR_GREATER && (IOS || MACCATALYST)
    internal partial class AccordionScrollView : ScrollView, ICrossPlatformLayout
#else
    internal partial class AccordionScrollView : ScrollView
#endif
    {
		#region Methods

#if NET8_0_OR_GREATER && (IOS || MACCATALYST)

		/// <summary>
		/// Arranges the bounds for the <see cref="ItemContainer"/> when using .NET 8
		/// or greater as a workaround for a known blank issue. This method customizes
		/// the layout arrangement to ensure proper display in iOS and Mac Catalyst environments.
		/// </summary>
		/// <param name="bounds">The bounds within which the <see cref="ItemContainer"/> should be arranged.</param>
		/// <returns>The <see cref="Size"/> of the arranged scroll view.</returns>
		/// <remarks>
		/// When executed, this method overrides the native bounds used during
		/// content arrangement, to address an issue where the frame is not properly utilized.
		/// </remarks>
		Size ICrossPlatformLayout.CrossPlatformArrange(Microsoft.Maui.Graphics.Rect bounds)
        {
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
}
