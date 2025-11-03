
namespace Syncfusion.Maui.Toolkit.SegmentedControl
{
	/// <summary>
	/// Represents a static class that holds the segment view related static methods.
	/// </summary>
	internal static class SegmentViewHelper
	{
		#region Internal methods

		/// <summary>
		/// Method to calculates the actual width for the segment control.
		/// </summary>
		/// <param name="segmentInfo">The segment information containing details about the segment.</param>
		/// <param name="widthRequest">The width request.</param>
		/// <param name="minWidth">The minimum width.</param>
		/// <param name="maxWidth">The maximum width.</param>
		/// <param name="alignment">The alignment option.</param>
		/// <returns>The actual width for the segment.</returns>
		internal static double GetActualSegmentWidth(ISegmentItemInfo segmentInfo, double widthRequest, double minWidth, double maxWidth, LayoutAlignment alignment)
		{
			var items = segmentInfo.Items;
			bool isAlignmentFill = alignment == LayoutAlignment.Fill;
			double strokeThickness = segmentInfo.StrokeThickness;
			int visibleSegmentsCount = segmentInfo.VisibleSegmentsCount;

			// The reason for handling padding 3 is to consider the key navigation view for the segmented control.
			double padding = 3;
			double keyFocusedViewPadding = 2 * padding;

			// When VisibleSegmentCount is specified, the control adjusts its behavior accordingly.
			// If HorizontalOption is set to Fill or FillAndExpand, it utilizes MaxWidth / VisibleSegmentCount.
			// If HorizontalOption is set to Start, Center, or End, it uses MinWidth / VisibleSegmentCount.
			double actualWidth = (int)GetEffectiveWidth(widthRequest, minWidth, maxWidth, visibleSegmentsCount, isAlignmentFill);

			// Calculate visible segment counts.
			int actualSegmentsCount = GetVisibleSegmentsCount(segmentInfo);

			// To avoid cropping in the keyboard navigation view, when visible segments are there and the alignment is set to fill, we adjust the width by subtracting the padding multiplied by the total number of segments.
			if (visibleSegmentsCount != -1 && isAlignmentFill)
			{
				actualWidth -= keyFocusedViewPadding + ((actualSegmentsCount + 1) * strokeThickness);
			}

			// If there are no items in the segment, return the maximum of widthRequest (if non-negative) and minWidth.
			if (items == null || items.Count == 0)
			{
				return actualWidth;
			}

			// Update the item width manually, if the visible segment count is greater than 0, based on the following order.
			// First, based on the WidthRequest. Second, using the Item Width property, and finally, considering the SegmentWidth property.
			for (int i = 0; i < items.Count; i++)
			{
				SfSegmentItem segmentItem = items[i];
				segmentItem.Width = visibleSegmentsCount > 0 ? actualWidth / actualSegmentsCount : segmentItem.Width;
			}

#if WINDOWS
			strokeThickness = actualSegmentsCount * strokeThickness;
#else
			// The reason for adding 1 is to consider the stroke thickness for the last segment.
			strokeThickness = (actualSegmentsCount + 1) * strokeThickness;
#endif

			// Calculate total width for visible segments.
			double totalWidth = GetTotalSegmentWidth(segmentInfo, actualSegmentsCount) + strokeThickness;

			// Calculated width based on widthRequest and maxWidth.
			if (widthRequest < 0)
			{
				return totalWidth + keyFocusedViewPadding > maxWidth ? maxWidth - keyFocusedViewPadding : totalWidth;
			}

			// Left and right of the keyboard layout in order to avoid the control crop issue while given the heightrequest.
			int keyboardPadding = 6;

			// If widthRequest is non-negative, return the maximum of widthRequest and minWidth.
			return (Math.Max(widthRequest, minWidth) - keyboardPadding);
		}

		/// <summary>
		/// Method to calculates the actual height for the segment control.
		/// </summary>
		/// <param name="itemInfo">The segment information containing details about the segment.</param>
		/// <param name="heightRequest">The height request.</param>
		/// <param name="minHeight">The minimum height.</param>
		/// <param name="maxHeight">The maximum height.</param>
		/// <returns>The actual height for the segment.</returns>
		internal static double GetActualSegmentHeight(ISegmentItemInfo itemInfo, double heightRequest, double minHeight, double maxHeight)
		{
			// If there are no items in the segment, return the maximum of heightRequest (if non-negative) and minHeight.
			if (itemInfo.Items == null || itemInfo.Items.Count == 0)
			{
				return Math.Max(heightRequest >= 0 ? heightRequest : minHeight, minHeight);
			}

			// If heightRequest is negative, calculate the total height of the segment's items and return the minimum of totalHeight and maxHeight.
			if (heightRequest < 0)
			{
				double totalHeight = SegmentItemViewHelper.GetItemHeight(itemInfo);
				if (totalHeight > maxHeight)
				{
					return totalHeight;
				}

				return Math.Min(totalHeight, maxHeight);
			}

			// Top and Bottom of the keyboard layout in order to avoid the control crop issue while given the heightrequest.
			int keyboardPadding = 6;
			// If heightRequest is non-negative, return the maximum of heightRequest and minHeight.
			return (Math.Max(heightRequest, minHeight) - keyboardPadding);
		}

		/// <summary>
		/// Method to gets the horizontal X position.
		/// </summary>
		/// <param name="dirtyRect">The dirty rect.</param>
		/// <param name="segmentWidth">The width of the segment.</param>
		/// <param name="alignment">The alignment option.</param>
		/// <returns>The horizontal X position.</returns>
		internal static float GetHorizontalXPosition(Rect dirtyRect, double segmentWidth, LayoutAlignment alignment)
		{
			int padding = 3;
			// Handling the keyboard focused view has a negative value to update the border around the segmented item.
			if (alignment != LayoutAlignment.Fill)
			{
				return padding;
			}

			float xPosition = (float)(dirtyRect.Center.X - (segmentWidth / 2));
			return xPosition > padding ? xPosition : padding;
		}

		/// <summary>
		/// Method to gets the vertical Y position.
		/// </summary>
		/// <param name="dirtyRect">The dirty rect.</param>
		/// <param name="segmentHeight">The height of the segment.</param>
		/// <param name="alignment">The alignment option.</param>
		/// <returns>The vertical X position.</returns>
		internal static float GetVerticalYPosition(Rect dirtyRect, double segmentHeight, LayoutAlignment alignment)
		{
			int padding = 3;
			// Handling the keyboard focused view has a negative value to update the border around the segmented item.
			if (alignment != LayoutAlignment.Fill)
			{
				return padding;
			}

			float yPosition = (float)(dirtyRect.Center.Y - (segmentHeight / 2));
			return yPosition > padding ? yPosition : padding;
		}

		/// <summary>
		/// Method to get the visible segments count.
		/// </summary>
		/// <param name="itemInfo">The segment item info.</param>
		/// <returns>The visible segment items count.</returns>
		internal static int GetVisibleSegmentsCount(ISegmentItemInfo itemInfo)
		{
			var visibleSegmentsCount = itemInfo.VisibleSegmentsCount;
			var items = itemInfo.Items;
			int itemsCount = items != null ? items.Count : 0;
			if (visibleSegmentsCount < 0)
			{
				return itemsCount;
			}

			// Return the count of visible segment items. If the calculated count based on the provided visibleSegmentsCount is higher than the total items count,
			// return visibleSegmentsCount. Otherwise, return the total items count.
			return itemsCount > visibleSegmentsCount ? visibleSegmentsCount : itemsCount;
		}

		/// <summary>
		/// Method to get the stroke thickness for the segmented control.
		/// </summary>
		/// <param name="strokeThickness">The stroke thickness.</param>
		/// <returns></returns>
		internal static float GetStrokeThickness(double strokeThickness)
		{
			return 2 * (float)strokeThickness;
		}

		/// <summary>
		/// Method to convert the brush to color.
		/// </summary>
		/// <param name="color">The color.</param>
		/// <returns>The color value.</returns>
		internal static Color BrushToColorConverter(Brush color)
		{
			Paint paint = color;
			return paint.ToColor() ?? Colors.Transparent;
		}

		/// <summary>
		/// Method to get the segment item text style value.
		/// </summary>
		/// <param name="itemInfo">The segment item info.</param>
		/// <param name="segmentItem">The segment item.</param>
		/// <returns>The segment text style value.</returns>
		internal static SegmentTextStyle GetSegmentTextStyle(ISegmentItemInfo? itemInfo, SfSegmentItem segmentItem)
		{
			SegmentTextStyle textStyle = segmentItem.TextStyle ?? itemInfo?.TextStyle ?? new SegmentTextStyle();
			if (textStyle.FontSize == -1)
			{
				textStyle.FontSize = 14;
			}

			return textStyle;
		}

		/// <summary>
		/// Method to get the cloned segment item text style value.
		/// </summary>
		/// <param name="itemInfo">The segment item info.</param>
		/// <param name="segmentItem">The segment item.</param>
		/// <returns>The segment text style value.</returns>
		internal static SegmentTextStyle GetClonedSegmentTextStyle(ISegmentItemInfo? itemInfo, SfSegmentItem segmentItem)
		{
			SegmentTextStyle textStyle = GetSegmentTextStyle(itemInfo, segmentItem);
			return new SegmentTextStyle()
			{
				TextColor = textStyle.TextColor,
				FontSize = textStyle.FontSize,
				FontFamily = textStyle.FontFamily,
				FontAttributes = textStyle.FontAttributes
			};
		}

		/// <summary>
		/// Method to get the segment item read only value.
		/// </summary>
		/// <param name="itemInfo">The segment item info.</param>
		/// <param name="segmentItem">The segment item.</param>
		/// <returns>The enabled value.</returns>
		internal static bool GetItemEnabled(ISegmentItemInfo? itemInfo, SfSegmentItem segmentItem)
		{
			return !segmentItem.IsEnabled ? segmentItem.IsEnabled : (itemInfo?.IsEnabled ?? false);
		}

		/// <summary>
		/// Method to get the segment item background.
		/// </summary>
		/// <param name="itemInfo">The segment item info.</param>
		/// <param name="segmentItem">The segment item.</param>
		/// <returns>The segment item background value.</returns>
		internal static Brush GetSegmentBackground(ISegmentItemInfo itemInfo, SfSegmentItem segmentItem)
		{
			return segmentItem.Background ?? itemInfo?.SegmentBackground ?? Brush.Transparent;
		}

		/// <summary>
		/// Method to get the selected segment item background.
		/// </summary>
		/// <param name="itemInfo">The segment item info.</param>
		/// <param name="segmentItem">The segment item.</param>
		/// <returns>The selected segment item background value.</returns>
		internal static Brush GetSelectedSegmentBackground(ISegmentItemInfo itemInfo, SfSegmentItem segmentItem)
		{
			return segmentItem.SelectedSegmentBackground ?? itemInfo?.SelectionIndicatorSettings?.Background ?? Brush.Blue;
		}

		/// <summary>
		/// Method to get the selected segment item stroke color.
		/// </summary>
		/// <param name="itemInfo">The segment item info.</param>
		/// <param name="segmentItem">The segment item.</param>
		/// <returns>The selected segment item stroke value.</returns>
		internal static Brush GetSelectedSegmentStroke(ISegmentItemInfo? itemInfo, SfSegmentItem segmentItem)
		{
			return segmentItem.SelectedSegmentBackground ?? new SolidColorBrush(itemInfo?.SelectionIndicatorSettings?.Stroke) ?? Brush.Blue;
		}

		/// <summary>
		/// Method to get the selected segment item text color value.
		/// </summary>
		/// <param name="itemInfo">The segment item info.</param>
		/// <param name="segmentItem">The segment item.</param>
		/// <returns>The selected segment item text color value.</returns>
		internal static Color GetSelectedSegmentForeground(ISegmentItemInfo? itemInfo, SfSegmentItem segmentItem)
		{
			bool isFillSelectionIndicator = itemInfo?.SelectionIndicatorSettings.SelectionIndicatorPlacement == SelectionIndicatorPlacement.Fill;
			if (isFillSelectionIndicator)
			{
				return segmentItem.SelectedSegmentTextColor ?? itemInfo?.SelectionIndicatorSettings?.TextColor ?? Colors.White;
			}
			else
			{
				Color textColor;
				if (itemInfo != null && itemInfo.SelectionIndicatorSettings != null && !itemInfo.SelectedSegmentTextColor.Equals(itemInfo.SelectionIndicatorSettings.TextColor))
				{
					textColor = itemInfo.SelectionIndicatorSettings.TextColor;
				}
				else
				{
					textColor = BrushToColorConverter(GetSelectedSegmentStroke(itemInfo, segmentItem));
				}

				return segmentItem.SelectedSegmentTextColor ?? textColor;
			}
		}

		/// <summary>
		/// Gets the background brush for the segment when it is in a hovered state.
		/// </summary>
		/// <param name="itemInfo">The segment item info.</param>
		/// <returns>The background brush for the hovered segment.</returns>
		internal static Brush GetHoveredSegmentBackground(ISegmentItemInfo itemInfo)
		{
			if (itemInfo.HoveredBackground != null)
			{
				return itemInfo.HoveredBackground;
			}

			return new SolidColorBrush(Color.FromArgb("#1C1B1F14"));
		}

		/// <summary>
		/// Gets the background brush for the segment when it is in a selected and hovered state.
		/// </summary>
		/// <param name="itemInfo">The segment item info.</param>
		/// <param name="segmentItem">The segment item.</param>
		/// <returns>The background brush for the selected and hovered segment.</returns>
		internal static Brush GetSelectedSegmentHoveredBackground(ISegmentItemInfo itemInfo, SfSegmentItem segmentItem)
		{
			Brush backgound = GetSelectedSegmentBackground(itemInfo, segmentItem);
			Color color = BrushToColorConverter(backgound).WithAlpha(0.8f);
			return new SolidColorBrush(color);
		}

		/// <summary>
		/// Gets the stroke for the segment when it is in a selected and hovered state.
		/// </summary>
		/// <param name="itemInfo">The segment item info.</param>
		/// <param name="segmentItem">The segment item.</param>
		/// <returns>The stroke color for the selected and hovered segment.</returns>
		internal static Brush GetSelectedSegmentHoveredStroke(ISegmentItemInfo itemInfo, SfSegmentItem segmentItem)
		{
			Brush backgound = GetSelectedSegmentStroke(itemInfo, segmentItem);
			Color color = BrushToColorConverter(backgound).WithAlpha(0.8f);
			return new SolidColorBrush(color);
		}

		/// <summary>
		/// Method to gets the effective flow direction of view.
		/// </summary>
		/// <param name="view">The actual view.</param>
		/// <returns>The flow direction.</returns>
		internal static FlowDirection GetEffectiveFlowDirection(IView view)
		{
			if (view.FlowDirection != FlowDirection.MatchParent)
			{
				return view.FlowDirection;
			}

			if (view.Parent != null && view.Parent is IView parentView)
			{
				return GetEffectiveFlowDirection(parentView);
			}

			return FlowDirection.LeftToRight;
		}

		/// <summary>
		/// Method to assign the parent element, when the new element is created or assigned
		/// </summary>
		/// <param name="oldElement">The old element</param>
		/// <param name="newElement">The new element.</param>
		/// <param name="parentElement">The parent element.</param>
		internal static void SetParent(Element? oldElement, Element? newElement, SfSegmentedControl? parentElement)
		{
			if (oldElement != null)
			{
				oldElement.Parent = null;
			}

			if (newElement != null)
			{
				newElement.Parent = parentElement;
			}
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Method to get the total segment item's width.
		/// </summary>
		/// <param name="itemInfo">The segment item info.</param>
		/// <param name="visibleSegmentsCount">The visible segments count.</param>
		/// <returns>The total segment item's width</returns>
		static double GetTotalSegmentWidth(ISegmentItemInfo itemInfo, int visibleSegmentsCount)
		{
			var items = itemInfo.Items;
			double totalSegmentWidth = 0.0;
			if (items == null || items.Count == 0)
			{
				return totalSegmentWidth;
			}

			for (int i = 0; i < visibleSegmentsCount; i++)
			{
				SfSegmentItem segmentItem = items[i];
				totalSegmentWidth += !double.IsNaN(segmentItem.Width) ? segmentItem.Width : itemInfo?.SegmentWidth ?? 0;
			}

			return totalSegmentWidth;
		}

		/// <summary>
		/// Method to get the effective width.
		/// </summary>
		/// <param name="widthRequest">The width request.</param>
		/// <param name="minWidth">The minimum width.</param>
		/// <param name="maxWidth">The maximum width.</param>
		/// <param name="visibleSegmentCount">The visible segment count width.</param>
		/// <param name="isAlignmentFill">A bool indicating whether the alignment is set to Fill.</param>
		/// <returns>The effective width.</returns>
		static double GetEffectiveWidth(double widthRequest, double minWidth, double maxWidth, int visibleSegmentCount, bool isAlignmentFill)
		{
			// When VisibleSegmentCount is specified, and if HorizontalOption is set to Fill or FillAndExpand, it takes MaxWidth.
			if (widthRequest >= 0 || visibleSegmentCount == -1 || !isAlignmentFill)
			{
				return Math.Max(widthRequest >= 0 ? widthRequest : minWidth, minWidth);
			}

			return maxWidth;
		}

		#endregion
	}
}