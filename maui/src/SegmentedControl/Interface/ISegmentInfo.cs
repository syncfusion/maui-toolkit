using System.Collections.ObjectModel;

namespace Syncfusion.Maui.Toolkit.SegmentedControl
{
	/// <summary>
	/// Represents the properties and settings for a segment in a segmented control.
	/// </summary>
	internal interface ISegmentInfo
	{
		/// <summary>
		/// Gets the collection of segment items.
		/// </summary>
		ObservableCollection<SfSegmentItem>? Items { get; }

		/// <summary>
		/// Gets a value indicating whether the segment is enabled.
		/// </summary>
		bool IsEnabled { get; }

		/// <summary>
		/// Gets a value indicating whether to show a separator between segments.
		/// </summary>
		bool ShowSeparator { get; }

		/// <summary>
		/// Gets or sets a value indicating whether the ripple effect animation should be applied to a segment item when it is selected for default and segment template added. 
		/// </summary>
		bool EnableRippleEffect { get; }

		/// <summary>
		/// Gets or sets the selection behavior of segment items, allowing either single selection or single deselection.
		/// </summary>
		SegmentSelectionMode SelectionMode { get; }

		/// <summary>
		/// Gets a value indicating whether the layout is in Right-to-Left (RTL) direction.
		/// </summary>
		bool IsRTL { get; }

		/// <summary>
		/// Gets the text style for the segment item.
		/// </summary>
		SegmentTextStyle TextStyle { get; }

		/// <summary>
		/// Gets the data template for the segment item.
		/// </summary>
		DataTemplate SegmentTemplate { get; }

		/// <summary>
		/// Gets the width of the segment item.
		/// </summary>
		double SegmentWidth { get; }

		/// <summary>
		/// Gets the height of the segment item.
		/// </summary>
		double SegmentHeight { get; }

		/// <summary>
		/// Gets the thickness of the segment's stroke.
		/// </summary>
		double StrokeThickness { get; }

		/// <summary>
		/// Gets the count of visible segments.
		/// </summary>
		int VisibleSegmentsCount { get; }

		/// <summary>
		/// Gets the selected index of the segment.
		/// </summary>
		int? SelectedIndex { get; }

		/// <summary>
		/// Gets the stroke brush for the segment.
		/// </summary>
		Brush Stroke { get; }

		/// <summary>
		/// Gets the selection indicator settings for the segment.
		/// </summary>
		SelectionIndicatorSettings SelectionIndicatorSettings { get; }

		/// <summary>
		/// Gets the background brush for the segment.
		/// </summary>
		Brush SegmentBackground { get; }

		/// <summary>
		/// Gets the hovered background brush for the segment.
		/// </summary>
		Brush HoveredBackground { get; }

		/// <summary>
		/// Gets the focused keyboard stroke for the segment.
		/// </summary>
		Brush KeyboardFocusStroke { get; }

		/// <summary>
		/// Gets the selected text color for the segment.
		/// </summary>
		Color SelectedSegmentTextColor { get; }

		/// <summary>
		/// Gets the background brush for the segment when it is disabled.
		/// </summary>
		Brush DisabledSegmentBackground { get; }

		/// <summary>
		/// Gets the text color for the segment when it is disabled.
		/// </summary>
		Color DisabledSegmentTextColor { get; }

		/// <summary>
		/// Gets the corner radius for the segment.
		/// </summary>
		CornerRadius CornerRadius { get; }

		/// <summary>
		/// Gets the corner radius for the segment items.
		/// </summary>
		CornerRadius SegmentCornerRadius { get; }
	}

	/// <summary>
	/// Provides additional functionality for a segment item in a segmented control.
	/// </summary>
	internal interface ISegmentItemInfo : ISegmentInfo
	{
		/// <summary>
		/// Triggers the selection changed event for the segment item.
		/// </summary>
		/// <param name="eventArgs">The <see cref="SelectionChangedEventArgs"/> containing the old and new index and values.</param>
		void TriggerSelectionChangedEvent(SelectionChangedEventArgs eventArgs);

		/// <summary>
		/// Triggers the tapped event for the segment item.
		/// </summary>
		/// <param name="eventArgs">The <see cref="SegmentTappedEventArgs"/> containing the tapped item.</param>
		void TriggerTappedEvent(SegmentTappedEventArgs eventArgs);

		/// <summary>
		/// Updates the selected index for the segment item.
		/// </summary>
		/// <param name="index">The new selected index value.</param>
		void UpdateSelectedIndex(int index);

		/// <summary>
		/// Clears the keyboard focused view for the segment item.
		/// </summary>
		void ClearFocusedView();

		/// <summary>
		/// Updates the scroll view position to focused index for the segment item.
		/// </summary>
		/// <param name="index">The focused index value.</param>
		void UpdateScrollOnKeyNavigation(int index);
	}
}