using Syncfusion.Maui.Toolkit.Graphics.Internals;
using ITextElement = Syncfusion.Maui.Toolkit.Graphics.Internals.ITextElement;

namespace Syncfusion.Maui.Toolkit.SegmentedControl
{
	/// <summary>
	/// Represents a static class that holds the <see cref="SegmentItemView"/> related methods.
	/// </summary>
	internal static class SegmentItemViewHelper
	{
		#region Internal methods

		/// <summary>
		/// Gets the width of the segment item.
		/// </summary>
		/// <param name="itemInfo">The segment item info.</param>
		/// <param name="segmentItem">The segment item to get the width for.</param>s
		/// <returns>The width of the segment item.</returns>
		internal static double GetItemWidth(ISegmentItemInfo itemInfo, SfSegmentItem segmentItem)
		{
			return (!double.IsNaN(segmentItem.Width) ? segmentItem.Width : itemInfo?.SegmentWidth ?? 0) + (itemInfo?.StrokeThickness ?? 0);
		}

		/// <summary>
		/// Gets the height of the segment item.
		/// </summary>
		/// <param name="itemInfo">The segment item to get the height for.</param>
		/// <returns>The height of the segment item.</returns>
		internal static double GetItemHeight(ISegmentItemInfo itemInfo)
		{
			return itemInfo.SegmentHeight + SegmentViewHelper.GetStrokeThickness(itemInfo.StrokeThickness);
		}

		/// <summary>
		/// Method to draw the segment item text.
		/// </summary>
		/// <param name="canvas">The canvas on which to draw the text.</param>
		/// <param name="text">The text to draw.</param>
		/// <param name="textStyle">The style for the text, such as font size, color, etc.</param>
		/// <param name="rect">The rectangle in which the text should be drawn.</param>
		/// <param name="horizontalAlignment">The horizontal alignment of the text within the rectangle. (Default: Left)</param>
		/// <param name="verticalAlignment">The vertical alignment of the text within the rectangle. (Default: Top)</param>
		internal static void DrawText(ICanvas canvas, string text, ITextElement textStyle, Rect rect, HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left, VerticalAlignment verticalAlignment = VerticalAlignment.Top)
		{
			if (rect.Height < 0 || rect.Width < 0)
			{
				return;
			}

			canvas.DrawText(text, rect, horizontalAlignment, verticalAlignment, textStyle);
		}

		/// <summary>
		/// Method to trim the segment item text.
		/// Framework issue:https://github.com/dotnet/Microsoft.Maui.Graphics/issues/175
		/// TODO: We can remove this if the issue resolved from framework.
		/// </summary>
		/// <param name="text">The original text to be trimmed.</param>
		/// <param name="width">The maximum width within which the text should be trimmed.</param>
		/// <param name="textStyle">The style for the text, such as font size, attributes, and family.</param>
		/// <returns>The trimmed text.</returns>
		internal static string TrimText(string text, double width, SegmentTextStyle textStyle)
		{
			//// On window resize exception throws.
			if (textStyle.FontSize < 0 || width < 0)
			{
				return text;
			}

			Size textSize = GetTextSize(text, textStyle);
			if (textSize.Width < width || textStyle.FontSize > width)
			{
				return text;
			}

			string textTrim = text;
			while ((int)textSize.Width + 1 > width)
			{
				textTrim = textTrim[..^1];
				textSize = (textTrim + "..").Measure((float)textStyle.FontSize, textStyle.FontAttributes, textStyle.FontFamily);
			}

			return textTrim + "..";
		}

		/// <summary>
		/// Gets the size of the specified text based on the provided text style.
		/// </summary>
		/// <param name="text">The text whose size needs to be measured.</param>
		/// <param name="textStyle">The style for the text, such as font size, attributes, and family.</param>
		/// <returns>The size of the text in the specified text style.</returns>
		internal static Size GetTextSize(string text, SegmentTextStyle textStyle)
		{
			if (text == string.Empty)
			{
				return default;
			}

			return text.Measure((float)textStyle.FontSize, textStyle.FontAttributes, textStyle.FontFamily);
		}

		/// <summary>
		/// Creates a view using the specified data template and binding context info.
		/// </summary>
		/// <param name="template">The data template used to create the view.</param>
		/// <param name="info">The binding context info.</param>
		/// <param name="isRTL">A flag indicating whether the scheduler is in RTL (Right-to-Left) flow direction.</param>
		/// <returns>Returns the view created using the data template.</returns>
		internal static View CreateTemplateView(DataTemplate template, object info, bool isRTL)
		{
			View view;

			// Create the content from the data template.
			var content = template.CreateContent();

			// Inside a DataTemplate, we can have View, Layout, and ViewCell directly. If the template has ViewCell as its direct content,
			// we cannot cast it to View. All layouts, controls, and views are base types of View, but ViewCell is not a type of View.
			// Hence, we need to get the View from the ViewCell.
#if !NET10_0_OR_GREATER
			if (content is ViewCell)
			{
				view = ((ViewCell)content).View;
			}
			else
			{
				view = (View)content;
			}
#else
			view = (View)content;
#endif

			// Set the binding context if it's not already set.
			view.BindingContext ??= info;

			// Update the view's flow direction based on the RTL flag to apply the flow direction of the segment.
			view.FlowDirection = isRTL ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
			return view;
		}

		/// <summary>
		/// Method to get the index of the next focusable item.
		/// </summary>
		/// <param name="itemInfo">The segment item info.</param>
		/// <param name="focusedIndex">The current focused index.</param>
		/// <param name="isRTL">A flag indicating whether the layout is RTL or not.</param>
		internal static void GetNextFocusableItemIndex(ISegmentItemInfo itemInfo, ref int focusedIndex, bool isRTL)
		{
			if (itemInfo == null || itemInfo.Items == null || itemInfo.Items.Count < 0)
			{
				return;
			}

			int itemCount = itemInfo.Items.Count;

			// Calculate the index of the next focusable item based on the arrow key and RTL layout.
			focusedIndex = isRTL ? (focusedIndex - 1 + itemCount) % itemCount : (focusedIndex + 1) % itemCount;
		}

		/// <summary>
		/// Method to get the index of the previous focusable item.
		/// </summary>
		/// <param name="itemInfo">The segment item info.</param>
		/// <param name="focusedIndex">The current focused index.</param>
		/// <param name="isRTL">A flag indicating whether the layout is RTL or not.</param>
		internal static void GetPreviousFocusableItemIndex(ISegmentItemInfo itemInfo, ref int focusedIndex, bool isRTL)
		{
			if (itemInfo == null || itemInfo.Items == null || itemInfo.Items.Count < 0)
			{
				return;
			}

			int itemCount = itemInfo.Items.Count;

			// Calculate the index of the previous focusable item based on the arrow key and RTL layout.
			focusedIndex = isRTL ? (focusedIndex + 1) % itemCount : (focusedIndex - 1 + itemCount) % itemCount;
		}

#endregion
	}
}