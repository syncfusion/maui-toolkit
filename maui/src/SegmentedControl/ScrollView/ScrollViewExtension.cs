using Microsoft.Maui.Graphics;
using Microsoft.Maui.Layouts;
namespace Syncfusion.Maui.Toolkit.SegmentedControl
{
	/// <summary>
	/// Represents the extension class of scroll view for Mac and iOS platforms.
	/// </summary>
#if IOS || MACCATALYST
	internal class ScrollViewExt : ScrollView, ICrossPlatformLayout
	{
		/// <summary>
		/// Override this method to handle scrolling issues on Mac and iOS.
		/// </summary>
		/// <param name="bounds">The bounds.</param>
		/// <returns>Size of scroll view.</returns>
		Size ICrossPlatformLayout.CrossPlatformArrange(Microsoft.Maui.Graphics.Rect bounds)
		{
			//// In Mac and iOS the scroll view scrolls beyond the view and empty space is visible.
			if (this is IScrollView scrollView)
			{
				bounds.X = 0;
				bounds.Y = 0;
				return scrollView.ArrangeContentUnbounded(bounds);
			}

			return bounds.Size;
		}
	}
#else
	internal partial class ScrollViewExt : ScrollView
	{
	}
#endif
}