using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using Microsoft.Maui.Controls.Shapes;
using Syncfusion.Maui.Toolkit.Internals;
using Syncfusion.Maui.Toolkit.Themes;

namespace Syncfusion.Maui.Toolkit.SegmentedControl
{
	/// <summary>
	/// The SfSegmentedControl that allows you to display a set of segments, typically used for switch among different views. Each segment in the control represents an item that the user can select. 
	/// The control can be populated with a collection of <see cref="SfSegmentItem"/> objects and a collection of strings.
	/// The SfSegmentedControl provides various customization options such as segment height, width, selection indicator settings, text and icon styles, and more.
	/// It supports setting the currently selected segment using the SelectedIndex property and provides an option to enable auto-scrolling when the selected index changes.
	/// You can also customize the appearance of individual segments using the SegmentTemplate.
	/// </summary>
	public partial class SfSegmentedControl : SfView, ISegmentItemInfo, IKeyboardListener, IParentThemeElement
	{
		#region Fields

		/// <summary>
		/// The scroll view for internal scrolling when segments exceed available width.
		/// </summary>
		ScrollViewExt? _scrollView;

		/// <summary>
		/// ToDo - Ensure that the views within the segmented control stay within the bounds of the calculated width and height.
		/// The views are clipped using a Grid element instead of directly applying the clipping to the ScrollView.
		/// The Grid container utilizes a RoundRectangleGeometry for clipping, allowing full functionality of the ScrollView on all platforms, including macOS and iOS.
		/// </summary>
		Grid? _grid;

		/// <summary>
		/// Gets or sets the navigation view that currently has keyboard focus.
		/// </summary>
		/// <remarks>
		/// This field represents the <see cref="KeyNavigationView"/> instance that currently possesses keyboard focus.
		/// When a navigation view has keyboard focus, it implies that it is the active element that will respond to keyboard input.
		/// This field can hold a reference to the <see cref="KeyNavigationView"/> object that has gained focus through keyboard interaction.
		/// If no navigation view currently has keyboard focus, this field can be <c>null</c>.
		/// </remarks>
		KeyNavigationView? _keyNavigationView;

		/// <summary>
		/// The OutlinedBorderView for rendering a rounded rectangle around the segmented control.
		/// </summary>
		OutlinedBorderView? _outlinedBorderView;

		/// <summary>
		/// The segment layout for rendering and managing individual segment items view.
		/// </summary>
		SegmentLayout? _segmentLayout;

		/// <summary>
		/// The actual width of segment, used for customization and layout calculations.
		/// </summary>
		double _segmentWidth;

		/// <summary>
		/// The actual height of segment, used for customization and layout calculations.
		/// </summary>
		double _segmentHeight;

		/// <summary>
		/// Gets or sets a value indicating whether automatic scrolling to the selected index is enabled.
		/// </summary>
		bool _isAutoScrollToSelectedIndex = true;

#if WINDOWS
		/// <summary>
		/// Holds the scroll offset value to maintain the move to time value on view change. 
		/// This offset is used after layout arrange because it doesn't apply when the scroll view is initially created.
		/// </summary>
		double _scrollOffset;
#endif

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="SfSegmentedControl"/> class.
		/// </summary>
		public SfSegmentedControl()
		{
			SfSegmentedResources.InitializeDefaultResource("Syncfusion.Maui.Toolkit.SegmentedControl.Resources.SfSegmentedControl",typeof(SfSegmentedControl));
			ThemeElement.InitializeThemeResources(this, "SfSegmentedControlTheme");
			SetDynamicResource(HoveredBackgroundProperty, "SfSegmentedControlHoveredBackground");
			SetDynamicResource(KeyboardFocusStrokeProperty, "SfSegmentedControlKeyboardFocusStroke");
			SetDynamicResource(SelectedSegmentTextColorProperty, "SfSegmentedControlSelectionTextColor");
			MinimumWidthRequest = 180;
			MinimumHeightRequest = 40;
			SelectionIndicatorSettings.Parent = this;
			TextStyle.Parent = this;
#if !WINDOWS
			this.AddKeyboardListener(this);
#endif

#if __IOS__
			this.IgnoreSafeArea = true;
#endif

#if WINDOWS
			// In Windows, the clip might throw an exception when setting the clip bounds on dispatcher.
            // Therefore, initializing the clip in initial loading to avoid the exception.
			Clip = new RoundRectangleGeometry();
#endif

		}

#endregion

		#region Properties

		/// <summary>
		/// Gets the segment control items collection.
		/// </summary>
		ObservableCollection<SfSegmentItem>? ISegmentInfo.Items => _items;

		/// <summary>
		/// Gets or sets the collection of items for the segment control. The items represent the individual segments within the control.
		/// </summary>
		internal ObservableCollection<SfSegmentItem>? _items;

		/// <summary>
		/// Gets a value indicating whether the layout is in Right-to-Left (RTL) direction.
		/// </summary>
		bool ISegmentInfo.IsRTL => SegmentViewHelper.GetEffectiveFlowDirection(this) == FlowDirection.RightToLeft;

		/// <summary>
		/// Gets the hovered background brush for the segment.
		/// </summary>
		Brush ISegmentInfo.HoveredBackground => HoveredBackground;

		/// <summary>
		/// Gets the focused keyboard stroke for the segment.
		/// </summary>
		Brush ISegmentInfo.KeyboardFocusStroke => KeyboardFocusStroke;

		/// <summary>
		/// Gets the selected text color for the segment.
		/// </summary>
		Color ISegmentInfo.SelectedSegmentTextColor => this.SelectedSegmentTextColor;

		#endregion

		#region Public methods

		/// <summary>
		/// Method to scrolls the scroll viewer to the specified index.
		/// </summary>
		/// <param name="index">The index of the item to scroll to.</param>
		/// <seealso cref="SetSegmentEnabled(int, bool)"/>
		/// <remarks>
		/// This method ensures that the segment at the given index is brought into view within the scrollable area.
		/// </remarks>
		/// <example>
		/// The below examples shows, how to use the <see cref="ScrollTo(int)"/> method to the <see cref="SfSegmentedControl"/>.
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <button:SfSegmentedControl x:Name="segmentedControl"
		///                            WidthRequest="102"
		///                            VisibleSegmentsCount="2">
		///    <button:SfSegmentedControl.ItemsSource>
		///        <x:Array Type="{x:Type x:String}">
		///            <x:String>Day</x:String>
		///            <x:String>Week</x:String>
		///            <x:String>Month</x:String>
		///            <x:String>Year</x:String>
		///        </x:Array>
		///    </button:SfSegmentedControl.ItemsSource>
		/// </button:SfSegmentedControl>
		/// ]]></code>
		///  # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// segmentedControl.ScrollTo(2);
		/// ]]></code>
		/// </example>
		public void ScrollTo(int index)
		{
			if (_scrollView == null || _items == null || index < 0 || index >= _items.Count)
			{
				return;
			}

			UpdateScrollPositionToSelectedIndex(index, true);
		}

		/// <summary>
		/// Sets the enabled state of a specific segment at the specified index.
		/// </summary>
		/// <param name="index">The index of the segment to set the enabled state for.</param>
		/// <param name="isEnabled">Determines whether the segment should be enabled (true) or disabled (false).</param>
		/// <seealso cref="ScrollTo(int)"/>
		/// <remarks>
		/// This method updates the enabled state of a segment item and its corresponding view in the segment layout.
		/// </remarks>
		/// <example>
		/// The below examples shows, how to use the <see cref="SetSegmentEnabled(int, bool)"/> method to the <see cref="SfSegmentedControl"/>.
		/// # [XAML](#tab/tabid-3)
		/// <code Lang="XAML"><![CDATA[
		/// <button:SfSegmentedControl x:Name="segmentedControl">
		///    <button:SfSegmentedControl.ItemsSource>
		///        <x:Array Type="{x:Type x:String}">
		///            <x:String>Day</x:String>
		///            <x:String>Week</x:String>
		///            <x:String>Month</x:String>
		///            <x:String>Year</x:String>
		///        </x:Array>
		///    </button:SfSegmentedControl.ItemsSource>
		/// </button:SfSegmentedControl>
		/// ]]></code>
		///  # [C#](#tab/tabid-4)
		/// <code Lang="C#"><![CDATA[
		/// segmentedControl.SetSegmentEnabled(2, false);
		/// ]]></code>
		/// </example>
		public void SetSegmentEnabled(int index, bool isEnabled)
		{
			if (_scrollView == null || _items == null || index < 0 || index >= _items.Count)
			{
				return;
			}

			// Get the SfSegmentItem instance at the specified index.
			SfSegmentItem segmentItem = _items[index];

			// Set the enabled state of the segment.
			segmentItem.IsEnabled = isEnabled;

			// Update the enabled state of the corresponding item view in the segment layout.
			_segmentLayout?.UpdateItemViewEnabledState(index, isEnabled);
		}

		#endregion

		#region Private methods

		/// <summary>
		/// Method to calculates the rectangle for drawing the outlined border view.
		/// </summary>
		/// <param name="dirtyRect">The available rectangle.</param>
		RectF GetOutlinedBorderRect(RectF dirtyRect)
		{
			float left = SegmentViewHelper.GetHorizontalXPosition(dirtyRect, _segmentWidth, HorizontalOptions.Alignment);
			float top = SegmentViewHelper.GetVerticalYPosition(dirtyRect, _segmentHeight, VerticalOptions.Alignment);
			float right = (float)_segmentWidth;
			float bottom = (float)_segmentHeight;

			// Ensure that the values are not less than zero.
			right = Math.Max(right, 0);
			bottom = Math.Max(bottom, 0);

			return new RectF(left, top, right, bottom);
		}

		/// <summary>
		/// Method to calculates the rectangle for drawing the segment layout view.
		/// </summary>
		/// <param name="dirtyRect">The available rectangle.</param>
		RectF GetSegmentLayoutRect(RectF dirtyRect)
		{
			// Determine the stroke thickness and half of the stroke radius.
			float strokeThickness = (float)StrokeThickness;
			float strokeRadius = strokeThickness / 2f;

			// Gets the outlined border rectangle area for calculating segment layout.
			RectF borderRect = GetOutlinedBorderRect(dirtyRect);
			float left, right, top, bottom;

#if ANDROID || IOS || MACCATALYST
			left = borderRect.Left + strokeRadius;
			right = borderRect.Width - strokeThickness;
#else
			left = borderRect.Left;
			right = borderRect.Width;
#endif
			// Calculate the coordinates for the draw rectangle with the stroke value.
			top = borderRect.Top + strokeThickness;
			bottom = borderRect.Height - (2 * strokeThickness);

			return new RectF(left, top, right, bottom);
		}

		/// <summary>
		/// Method to calculates the rectangle for drawing the keyboard navigation view.
		/// </summary>
		/// <param name="dirtyRect">The available rectangle.</param>
		RectF GetKeyboardLayoutRect(RectF dirtyRect)
		{
			// Gets the outlined border rectangle area for calculating key navigation view.
			RectF borderRect = GetOutlinedBorderRect(dirtyRect);
			int padding = 3;
			float left, top, right, bottom;

			// Calculate the coordinates for the draw rectangle with the padding.
			left = borderRect.Left - padding;
			top = borderRect.Top - padding;
			right = borderRect.Width + (2 * padding);
			bottom = borderRect.Height + (2 * padding);

			return new RectF(left, top, right, bottom);
		}

		/// <summary>
		/// Invokes when scroll view is scrolled.
		/// </summary>
		/// <param name="sender">The object.</param>
		/// <param name="e">The scrolled event args.</param>
		void OnScrollViewScrolled(object? sender, ScrolledEventArgs e)
		{
			// As we handled key navigation focused view at this layout. So while scrolling views we need to update the key navigation for the item at the focused index based on scrolling.
			_segmentLayout?.UpdateKeyNavigationViewOnScroll();
			if (AutoScrollToSelectedSegment)
			{
				return;
			}

			_scrollView?.ScrollToAsync(e.ScrollX, _scrollView.ScrollY, false);
		}

		/// <summary>
		/// Initializes the segment items and sets up the scrolling behavior for the SegmentView.
		/// </summary>
		void InitializeSegment()
		{
			if (_items == null || _items.Count == 0)
			{
				InitializeOutlinedBorderView();
				return;
			}

			RemoveOutlinedBorderView();
			_grid = [];
#if __IOS__
			_grid.IgnoreSafeArea = true;
#endif
			_grid.SizeChanged += GridSizeChanged;

			// The FlowDirection of the grid is set to LeftToRight to handle RTL rendering logic manually, as RTL behavior may not be consistent across Windows and macOS platforms.
			// Based on parent flow direction, the grid flow direction is set. Because the grid not transalate the touch in RTL direction.
			bool isRTL = SegmentViewHelper.GetEffectiveFlowDirection((IView)Parent) == FlowDirection.RightToLeft;
			_grid.FlowDirection = isRTL ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
			_scrollView = new ScrollViewExt
			{
				Orientation = ScrollOrientation.Horizontal,
				HorizontalScrollBarVisibility = ScrollBarVisibility.Never,
				VerticalScrollBarVisibility = ScrollBarVisibility.Never,
			};

			_grid.Children.Add(_scrollView);
			_segmentLayout = new SegmentLayout(this);
			_scrollView.Content = _segmentLayout;
			_scrollView.Scrolled += OnScrollViewScrolled;
			Children.Add(_grid);

			InitializeOutlinedBorderView();

			_keyNavigationView = new KeyNavigationView(this);
			Children.Add(_keyNavigationView);

			PerformSegmentItemSelection();
		}

		/// <summary>
		/// It will trigger while grid size is changed.
		/// </summary>
		/// <param name="sender">The sender</param>
		/// <param name="e">The event args.</param>
		void GridSizeChanged(object? sender, EventArgs e)
		{
			//// ScrollView in the vertical orientation is always measure the infinity height and width, since that the resize the control and measure the parent grid height and wedith does not trigger the override the measure and arrange.since we will call the invalidlayout manually for the content of the scrollview.
			_segmentLayout?.InvalidateLayout();
		}

		/// <summary>
		/// Performs actions related to segment item selection.
		/// </summary>
		/// <remarks>
		/// This method triggers the selection changed event for the segment item, updates the scroll position to the selected index, and wires up relevant events.
		/// </remarks>
		void PerformSegmentItemSelection()
		{
			// Trigger the selection changed event for the segment item.
			_segmentLayout?.TriggerSelectionChanged();

			// Update the scroll position to the selected index.
			Dispatcher.Dispatch(() =>
			{
				UpdateScrollPositionToSelectedIndex(SelectedIndex ?? -1, AutoScrollToSelectedSegment);
			});

			WireEvents();
		}

		/// <summary>
		/// Method to update the layout of the segment view, including corner radius and clipping.
		/// </summary>
		/// <param name="bounds">The bounds.</param>
		void SetClipBounds(Rect bounds)
		{
			UpdateClip(bounds);
		}

		/// <summary>
		/// Method to update the clip based on the corner radius.
		/// </summary>
		/// <param name="bounds">The bounds.</param>
		void UpdateClip(Rect bounds)
		{
			Rect clipRect = GetKeyboardLayoutRect(bounds);
#if ANDROID
			var newClip = new RoundRectangleGeometry()
			{
				Rect = clipRect,
				CornerRadius = CornerRadius,
			};

			Clip ??= newClip;

			var previousClip = (RoundRectangleGeometry)Clip;
			if (previousClip != null && (previousClip.Rect != newClip.Rect || previousClip.CornerRadius != newClip.CornerRadius))
			{
				Clip = new RoundRectangleGeometry()
				{
					Rect = clipRect,
					CornerRadius = CornerRadius,
				};
			}
#else
			Clip = new RoundRectangleGeometry()
			{
				Rect = clipRect,
				CornerRadius = CornerRadius,
			};
#endif
			if (_grid == null)
			{
				return;
			}

			Rect segmentLayoutRect = GetSegmentLayoutRect(bounds);

			// Get the rectangle for clipping based on the current bounds.
			clipRect = new RectF(0, 0, (float)segmentLayoutRect.Width, (float)segmentLayoutRect.Height);
#if ANDROID
			newClip = new RoundRectangleGeometry()
			{
				Rect = clipRect,
				CornerRadius = CornerRadius,
			};

			_grid.Clip ??= newClip;

			previousClip = (RoundRectangleGeometry)_grid.Clip;

			// Apply clipping to the grid to avoid the view going away from the segment layout bounds while scrolling.
			if (previousClip != null && (previousClip.Rect != newClip.Rect || previousClip.CornerRadius != newClip.CornerRadius))
			{
				_grid.Clip = newClip;
			}
#else
			// Apply clipping to the grid to avoid the view going away from the segment layout bounds while scrolling.
			_grid.Clip = new RoundRectangleGeometry()
			{
				Rect = clipRect,
				CornerRadius = CornerRadius,
			};
#endif
		}

		/// <summary>
		/// Initializes the segment items based on the ItemsSource.
		/// </summary>
		void InitializeSegmentItems()
		{
			if (ItemsSource is not IList itemsSource || itemsSource.Count == 0)
			{
				return;
			}

			_items?.Clear();
			_items ??= [];

			// Retrieve the items from the original ItemsSource and convert them to segmentItem instances.
			if (itemsSource is IEnumerable<SfSegmentItem> segmentItems)
			{
				foreach (SfSegmentItem item in segmentItems)
				{
					_items.Add(item);
				}
			}
			else if (itemsSource is IEnumerable<FileImageSource> images)
			{
				foreach (FileImageSource item in images)
				{
					SfSegmentItem segmentItem = new SfSegmentItem { ImageSource = item };
					_items.Add(segmentItem);
				}
			}
			else
			{
				foreach (object item in itemsSource)
				{
					if (item == null)
					{
						continue;
					}

					SfSegmentItem segmentItem = new SfSegmentItem { Text = item.ToString()! };
					_items.Add(segmentItem);
				}
			}
		}

		/// <summary>
		/// Method to initialize the outlined border view.
		/// </summary>
		void InitializeOutlinedBorderView()
		{
			_outlinedBorderView = new OutlinedBorderView(this);
			Children.Add(_outlinedBorderView);
		}

		/// <summary>
		/// Method to remove the outlined border view.
		/// </summary>
		void RemoveOutlinedBorderView()
		{
			if (_outlinedBorderView == null)
			{
				return;
			}

			if (Children.Contains(_outlinedBorderView))
			{
				Children.Remove(_outlinedBorderView);
			}

			_outlinedBorderView = null;
		}

		/// <summary>
		/// Method to wire the events.
		/// </summary>
		void WireEvents()
		{
			PropertyChanged += OnSegmentedControlPropertyChanged;
			if (ItemsSource is IEnumerable segmentItems)
			{
				if (segmentItems is INotifyCollectionChanged segmentItemsChanged)
				{
					segmentItemsChanged.CollectionChanged += OnSegmentItemsCollectionChanged;
				}
			}

			if (TextStyle != null)
			{
				TextStyle.PropertyChanged += OnTextStylePropertyChanged;
			}

			if (SelectionIndicatorSettings != null)
			{
				SelectionIndicatorSettings.PropertyChanged += OnSelectionIndicatorSettingsPropertyChanged;
			}
		}

		/// <summary>
		/// Handles changes to the segment items collection by responding to different types of notifications, such as add, remove, replace, reset and move.
		/// </summary>
		/// <param name="sender">The source of the collection change event.</param>
		/// <param name="e">Event arguments containing details about the change.</param>
		void OnSegmentItemsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					// Handle the addition of new items to the collection.
					AddSegmentItems(e);
					_segmentLayout?.AddSegmentItemView(e.NewStartingIndex);
					break;

				case NotifyCollectionChangedAction.Remove:
					// Handle the removal of items from the collection.
					RemoveSegmentItem(e.OldStartingIndex);
					_segmentLayout?.RemoveSegmentItemView(e.OldStartingIndex);
					break;

				case NotifyCollectionChangedAction.Move:
					// Handle the movement of items within the collection.
					MoveSegmentItem(e.OldStartingIndex, e.NewStartingIndex);
					_segmentLayout?.MoveSegmentItem(e.OldStartingIndex, e.NewStartingIndex);
					break;

				case NotifyCollectionChangedAction.Replace:
					// Handle the replacement of items in the collection.
					if (ItemsSource is ObservableCollection<SfSegmentItem> segmentItems)
					{
						ReplaceSegmentItem(e.NewStartingIndex, segmentItems[e.NewStartingIndex]);
						_segmentLayout?.ReplaceSegmentItem(e.NewStartingIndex, segmentItems[e.NewStartingIndex]);
					}
					break;

				case NotifyCollectionChangedAction.Reset:
					// Handle the resetting of the collection.
					ResetSegmentItems();
					_segmentLayout?.ClearSegmentItemsView();
					break;
			}

			// Invalidate layouts and drawable to reflect the changes in the UI.
			_segmentLayout?.InvalidateLayout();
			_segmentLayout?.InvalidateSegmentItemsLayout();
			_segmentLayout?.UpdateItemSelection();
			_segmentLayout?.InvalidateDrawable();
			InvalidateLayout();
		}

		/// <summary>
		/// Removes an item from the segment items collection at the specified index.
		/// </summary>
		/// <param name="index">The index of the item to remove.</param>
		void RemoveSegmentItem(int index)
		{
			// Check if the Items collection is null or the index is out of range.
			if (_items == null || index < 0 || index >= _items.Count)
			{
				return;
			}

			// Remove the item at the specified index.
			_items.RemoveAt(index);
		}

		/// <summary>
		/// Replaces an item in the segment items collection at the specified index with a new item.
		/// </summary>
		/// <param name="index">The index of the item to replace.</param>
		/// <param name="newItem">The new item to insert at the specified index.</param>
		void ReplaceSegmentItem(int index, SfSegmentItem newItem)
		{
			// Check if the Items collection is null or the index is out of range.
			if (_items == null || index < 0 || index >= _items.Count)
			{
				return;
			}

			// Replace the item at the specified index with the new item.
			_items[index] = newItem;
		}

		/// <summary>
		/// Moves an item within the segment items collection from an old index to a new index.
		/// </summary>
		/// <param name="oldIndex">The current index of the item to move.</param>
		/// <param name="newIndex">The target index where the item should be moved.</param>
		void MoveSegmentItem(int oldIndex, int newIndex)
		{
			// Check if the Items collection is null or if the indexes are out of range.
			if (_items == null || oldIndex < 0 || oldIndex >= _items.Count || newIndex < 0 || newIndex >= _items.Count)
			{
				return;
			}

			// Retrieve the item at the old index, remove it, and insert it at the new index.
			var item = _items[oldIndex];
			_items.RemoveAt(oldIndex);
			_items.Insert(newIndex, item);
		}

		/// <summary>
		/// Resets the segment items collection by clearing all items and re-adding them from the ItemsSource.
		/// </summary>
		void ResetSegmentItems()
		{
			// Clear all items from the collection if it is not null.
			if (_items != null)
			{
				_items.Clear();
				_items = null;
			}
		}

		/// <summary>
		/// Adds items to the segment items collection from the ItemsSource.
		/// </summary>
		void AddSegmentItems(NotifyCollectionChangedEventArgs e)
		{
			// Retrieve the ItemsSource as an IList.
			if (ItemsSource is not IList itemsSource || itemsSource.Count == 0)
			{
				return;
			}

			// Initialize the Items collection if it is null.
			_items ??= [];

			// Add a single item at a time based on index
			if (itemsSource is IEnumerable<SfSegmentItem> segmentItems)
			{
				// Check if the index is within the valid range
				if (e.NewStartingIndex >= 0 && e.NewStartingIndex < segmentItems.Count())
				{
					var item = segmentItems.ElementAt(e.NewStartingIndex);
					// Insert the item at the specified index
					_items.Insert(e.NewStartingIndex, item);
				}
			}
			else if (itemsSource is IEnumerable<FileImageSource> images)
			{
				// Check if the index is within the valid range
				if (e.NewStartingIndex >= 0 && e.NewStartingIndex < images.Count())
				{
					var image = images.ElementAt(e.NewStartingIndex);
					var segmentItem = new SfSegmentItem { ImageSource = image };
					// Insert the item at the specified index
					_items.Insert(e.NewStartingIndex, segmentItem);
				}
			}
			else
			{
				// Check if the index is within the valid range
				if (e.NewStartingIndex >= 0 && e.NewStartingIndex < itemsSource.Count)
				{
					var item = itemsSource[e.NewStartingIndex];
					if (item != null)
					{
						string? itemText = item.ToString();
						var segmentItem = new SfSegmentItem { Text = itemText ?? string.Empty };
						// Insert the item at the specified index
						_items.Insert(e.NewStartingIndex, segmentItem);
					}
				}
			}
		}

		/// <summary>
		/// Updates the scroll position of the segmented control's scroll view to the specified segment index.
		/// </summary>
		/// <param name="index">The selected segment index.</param>
		/// <param name="isAutoScrollToSelectedIndex">Determines whether the scroll position should automatically update to the selected segment index.</param>
		void UpdateScrollPositionToSelectedIndex(int index, bool isAutoScrollToSelectedIndex)
		{
			if (_scrollView == null || _items == null || !isAutoScrollToSelectedIndex)
			{
				return;
			}

			// Ensure the selected index is within valid bounds.
			index = Math.Max(0, Math.Min(index, _items.Count - 1));

			// Calculate the width of the scroll viewer's view port.
			double viewportWidth = _scrollView.Width;

			// Calculate the total width of all the segments.
			double totalSegmentWidth = 0.0;
			foreach (SfSegmentItem segmentItem in _items)
			{
				if (segmentItem == null)
				{
					continue;
				}

				totalSegmentWidth += SegmentItemViewHelper.GetItemWidth(this, segmentItem);
			}

			// Calculate the maximum allowed scroll position to prevent scrolling too far.
			double maxOffSet = Math.Max(0, totalSegmentWidth - viewportWidth);

			// Calculate the scroll position for the selected item.
			double offSet = 0.0;
			for (int i = 0; i < index; i++)
			{
				SfSegmentItem segmentItem = _items[i];
				if (segmentItem == null)
				{
					continue;
				}

				offSet += SegmentItemViewHelper.GetItemWidth(this, segmentItem);
			}

			// For RTL, reverse the scrollX value to account for right-to-left flow direction.
			bool isRTL = SegmentViewHelper.GetEffectiveFlowDirection(this) == FlowDirection.RightToLeft;
			offSet = isRTL ? maxOffSet - offSet : offSet;

			// Ensure the scrollX value is within valid bounds.
			offSet = Math.Max(0, Math.Min(offSet, maxOffSet));

			// Scroll to the calculated position.
			_scrollView.ScrollToAsync(offSet, _scrollView.ScrollY, true);
#if WINDOWS
			_scrollOffset = offSet;
#endif
		}

		/// <summary>
		/// Updates the scroll position during key-based navigation based on the focused view.
		/// </summary>
		/// <param name="focusedView">The <see cref="SegmentItemView"/> that currently has focus.</param>
		void UpdateScrollOnKeyNavigation(SegmentItemView? focusedView)
		{
			if (_scrollView == null || focusedView == null)
			{
				return;
			}

			bool isFirstViewFocused = false, isLastViewFocused = false;
			double focusedViewLeft = MathF.Round((float)focusedView.Bounds.Left);

			_ = focusedView.Bounds.Right;
			double scrollViewWidth = _scrollView.Frame.Width;
			double scrollViewContentWidth = _scrollView.ContentSize.Width;
			double scrollX = _scrollView.ScrollX;

			// Calculate the desired start position for scrolling.
			double startXPosition = focusedViewLeft;

			// Calculate the end position of the view within the scroll view.
			double endXPosition = MathF.Ceiling((float)(startXPosition + focusedView.Width));

			if (scrollViewContentWidth < scrollViewWidth)
			{
				return;
			}

			// Calculate the bounds of the visible segment items within the scroll view.
			double visibleStartItem = scrollX;
			double visibleEndItem = Math.Round((float)scrollX + scrollViewWidth);

			// Initialize the current scroll positions.
			double offSet = scrollX;
			// Check if the focused view is outside of the visible segment item.
			if (endXPosition >= visibleEndItem)
			{
				// Calculate the new scroll position to bring the focused view into the visible area.
				offSet = startXPosition - (scrollViewWidth - focusedView.Width);

				isLastViewFocused = true;
				offSet = Math.Max(0, Math.Min(offSet, scrollViewContentWidth - scrollViewWidth));
			}
			else if (focusedViewLeft <= visibleStartItem)
			{
				// Calculate the new scroll position to bring the focused view into the visible area.
				offSet = focusedViewLeft;

				isFirstViewFocused = true;
				offSet = Math.Max(0, Math.Min(offSet, scrollViewContentWidth - scrollViewWidth));
			}

			// Scroll to the new position and update the navigation view.
			_scrollView.ScrollToAsync(offSet, _scrollView.ScrollY, true);

			// Calculate the rect to update navigation view.
			Rect rect = new Rect(focusedViewLeft - offSet, focusedView.Y, focusedView.Width, focusedView.Height);

			// Re-updates the navigation view with calculated bounds based on scrolling position.
			_keyNavigationView?.UpdateNavigationView(focusedView._segmentItem, rect, isFirstViewFocused, isLastViewFocused);
		}

		/// <summary>
		/// Method invokes on segmented control property changed.
		/// </summary>
		/// <param name="sender">The segmented control object.</param>
		/// <param name="e">The segmented control property changed event arguments.</param>
		void OnSelectionIndicatorSettingsPropertyChanged(object? sender, PropertyChangedEventArgs e)
		{
			_segmentLayout?.UpdateSelectedSegmentItemStyle();
		}

		/// <summary>
		/// Method invokes on segmented control property changed.
		/// </summary>
		/// <param name="sender">The segmented control object.</param>
		/// <param name="e">The segmented control property changed event arguments.</param>
		void OnTextStylePropertyChanged(object? sender, PropertyChangedEventArgs e)
		{
			_segmentLayout?.InvalidateSegmentItemsDraw();
		}

		/// <summary>
		/// Handles the PropertyChanged event for the SegmentedControl, specifically monitoring changes in <see cref="FlowDirection"/>, and IsEnabled properties/>.
		/// </summary>
		/// <param name="sender">The segmented control object.</param>
		/// <param name="e">The segmented control property changed event arguments.</param>
		void OnSegmentedControlPropertyChanged(object? sender, PropertyChangedEventArgs e)
		{
			if (sender == null)
			{
				return;
			}

			SfSegmentedControl? segmentedControl = (SfSegmentedControl)sender;
			string? propertyName = e.PropertyName;
			if (segmentedControl == null || propertyName == null)
			{
				return;
			}

			if (propertyName != nameof(SfSegmentedControl.FlowDirection) && propertyName != nameof(SfSegmentedControl.IsEnabled)
				&& propertyName != nameof(SfSegmentedControl.HorizontalOptions))
			{
				return;
			}

			if (propertyName == nameof(SfSegmentedControl.IsEnabled))
			{
				UpdateLayoutInteraction(IsEnabled);
				return;
			}

			if (propertyName == nameof(SfSegmentedControl.HorizontalOptions))
			{
				segmentedControl.InvalidateLayout();
				segmentedControl._segmentLayout?.InvalidateLayout();
				return;
			}

			UpdateFlowDirection();
		}

		/// <summary>
		/// Method to invalidate the layout.
		/// </summary>
		void InvalidateLayout()
		{
			InvalidateMeasure();
		}

		/// <summary>
		/// Method to enable/disable the segment layout interaction based on isEnabled property values.
		/// </summary>
		/// <param name="isEnabled">A boolean indicating whether the layout should be enabled or disabled.</param>
		void UpdateLayoutInteraction(bool isEnabled)
		{
			if (_grid != null)
			{
				_grid.IsEnabled = isEnabled;
			}

			_segmentLayout?.UpdateLayoutInteraction(isEnabled);
		}

		/// <summary>
		/// Method to update the flow direction.
		/// </summary>
		void UpdateFlowDirection()
		{
#if WINDOWS
			// ToDo: As flow direction behavior may not be consistent across Windows platform. So handling manually to update the flow direction.
			// Clearing the segment items view and children to ensure proper flow direction rendering.
			_segmentLayout?.ClearSegmentItemsView();
			Children.Clear();
#endif
			InvalidateLayout();
			_outlinedBorderView?.InvalidateDrawable();
			_segmentLayout?.UpdateFlowDirection();
		}

		#endregion

		#region Override methods

		/// <summary>
		/// Measures the size of the content within the segment view based on the provided width and height constraints.
		/// </summary>
		/// <exclude/>
		/// <param name="widthConstraint">The maximum width request of the layout.</param>
		/// <param name="heightConstraint">The maximum height request of the layout.</param>
		/// <returns>The maximum size of the layout.</returns>
		protected override Size MeasureContent(double widthConstraint, double heightConstraint)
		{
			if (widthConstraint <= 0 || heightConstraint <= 0)
			{
				return default;
			}

			// Determine the actual width and height based on the provided constraints or the minimum request size.
			double width = double.IsFinite(widthConstraint) ? widthConstraint : MinimumWidthRequest;
			double height = double.IsFinite(heightConstraint) ? heightConstraint : MinimumHeightRequest;

			// Initialize the segment items and segment views.
			if (Children.Count == 0 || (_segmentLayout == null && ItemsSource != null))
			{
				InitializeSegmentItems();
				InitializeSegment();
			}

			// Get the actual segment width and height based on the request sizes and constraints.
			_segmentWidth = SegmentViewHelper.GetActualSegmentWidth(this, WidthRequest, MinimumWidthRequest, width, HorizontalOptions.Alignment);
			_segmentHeight = SegmentViewHelper.GetActualSegmentHeight(this, HeightRequest, MinimumHeightRequest, height);

#if ANDROID
			_segmentLayout?.UpdateSegmentLayoutHeight(_segmentHeight);
#endif
			Rect bounds = new Rect(0, 0, _segmentWidth, _segmentHeight);
			Rect keyboardLayoutRect = GetKeyboardLayoutRect(bounds);

			// Measure each child within the segment using the calculated segment width and height.
			foreach (var child in Children)
			{
				Rect rect = default;
				if (child is KeyNavigationView)
				{
					child.Measure(keyboardLayoutRect.Width, keyboardLayoutRect.Height);
					continue;
				}
				else if (child is OutlinedBorderView)
				{
					rect = GetOutlinedBorderRect(bounds);
					child.Measure(rect.Width, rect.Height);
					continue;
				}

				rect = GetSegmentLayoutRect(bounds);
				child.Measure(rect.Width, rect.Height);
			}

			// Enhanced the alignment behavior of the segmented control based on HorizontalOptions and VerticalOptions properties. If WidthRequest and HeightRequest are provided, these values are considered for arranging children. If not, the width and height values for segment arrangement are calculated.
			width = WidthRequest != -1 ? width : keyboardLayoutRect.Width;
			height = HeightRequest != -1 ? height : keyboardLayoutRect.Height;
			//// This condition ensures that the MinimumWidthRequest is not set to a value smaller than the calculated width. If the calculated width is less than the current MinimumWidthRequest, it updates the MinimumWidthRequest to match the calculated width. 
			//// This helps maintain a consistent minimum width constraint for the layout.
			if (width < MinimumWidthRequest)
			{
				MinimumWidthRequest = width;
			}

			return new Size(width, height);
		}

		/// <summary>
		/// Arranges the content of the segment view within the specified bounds.
		/// </summary>
		/// <exclude/>
		/// <param name="bounds">The available rect.</param>
		/// <returns>The layout size.</returns>
		protected override Size ArrangeContent(Rect bounds)
		{
			// Get the rectangular area for drawing the segment layout view.
			Rect segmentLayoutRect = GetSegmentLayoutRect(bounds);

			// Arrange each child view within the drawRect using AbsoluteLayout.
			foreach (View child in Children.Cast<View>())
			{
				if (child is KeyNavigationView)
				{
					// Get the rectangular area for drawing the keyboard navigation view.
					Rect keyboardLayoutRect = GetKeyboardLayoutRect(bounds);
					AbsoluteLayout.SetLayoutBounds(child, keyboardLayoutRect);
					continue;
				}
				else if (child is OutlinedBorderView)
				{
					// Get the rectangular area for drawing the outlined border view.
					Rect outlinedBorderRect = GetOutlinedBorderRect(bounds);
					AbsoluteLayout.SetLayoutBounds(child, outlinedBorderRect);
					continue;
				}

				AbsoluteLayout.SetLayoutBounds(child, segmentLayoutRect);
			}

			if (bounds.Height > 0 && bounds.Width > 0)
			{
				// Update the layout to apply the changes.
				SetClipBounds(bounds);
			}
#if WINDOWS
			//// TODO: In windows scroll offset not set on initial loading hence value changed here to maintain move to selected index position.
			//// On initial set scroll view doesn't have proper height hence value not updated in view. So scroll updated after children layout.
			if (_scrollView != null && _scrollOffset != _scrollView.ScrollX)
			{
				_scrollView.ScrollToAsync(_scrollOffset, _scrollView.ScrollY, true);
			}
#endif
			return base.ArrangeContent(bounds);
		}

#if WINDOWS
		/// <summary>
		/// Raises when <see cref="SfSegmentedControl"/>'s handler gets changed.
		/// <exclude/>
		/// </summary>
		protected override void OnHandlerChanged()
		{
			base.OnHandlerChanged();
			if (this.Handler != null && this.Handler.PlatformView != null && this.Handler.PlatformView is Microsoft.UI.Xaml.UIElement nativeView)
			{
				nativeView.IsTabStop = true;
			}
		}
#endif

		/// <summary>
		/// Invokes on the binding context of the view changed.
		/// </summary>
		/// <exclude/>
		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();
			TextStyle.BindingContext = BindingContext;
			SelectionIndicatorSettings.BindingContext = BindingContext;
		}

#if ANDROID
		/// <summary>
		/// Invokes on the size allocated for the view.
		/// </summary>
		/// <param name="width">The width of the view.</param>
		/// <param name="height">The height of the view.</param>
		protected override void OnSizeAllocated(double width, double height)
		{
			base.OnSizeAllocated(width, height);
			if (width > 0 && height > 0)
			{
				SetClipBounds(new Rect(0, 0, width, height));
			}
		}
#endif

		#endregion

		#region Interface Implementations

		/// <summary>
		/// Retrieves the resource dictionary for the current theme of the parent element.
		/// </summary>
		/// <returns>Returns the resource dictionary for the current theme of the parent element.</returns>
		ResourceDictionary IParentThemeElement.GetThemeDictionary()
		{
			return new SfSegmentedControlStyles();
		}

		/// <summary>
		/// Handles changes in the theme for individual controls.
		/// </summary>
		/// <param name="oldTheme">The old theme value.</param>
		/// <param name="newTheme">The new theme value.</param>
		void IThemeElement.OnControlThemeChanged(string oldTheme, string newTheme)
		{
		}

		/// <summary>
		/// Handles changes in the common theme shared across multiple elements.
		/// </summary>
		/// <param name="oldTheme">The old theme value.</param>
		/// <param name="newTheme">The new theme value.</param>
		void IThemeElement.OnCommonThemeChanged(string oldTheme, string newTheme)
		{
		}

		/// <summary>
		/// Gets a value indicating whether the view can become the first responder to listen the keyboard actions.
		/// </summary>
		/// <remarks>This property will be considered only in iOS Platform.</remarks>
		bool IKeyboardListener.CanBecomeFirstResponder
		{
			get { return true; }
		}

		/// <inheritdoc/>
		void IKeyboardListener.OnKeyDown(KeyEventArgs e)
		{
			_segmentLayout?.ProcessOnKeyDown(e);
		}

		/// <inheritdoc/>
		void IKeyboardListener.OnKeyUp(KeyEventArgs args)
		{

		}

		/// <summary>
		/// Triggers the selection changed event for the segment item.
		/// </summary>
		/// <param name="eventArgs">The <see cref="SelectionChangedEventArgs"/> containing the old and new index and values.</param>
		void ISegmentItemInfo.TriggerSelectionChangedEvent(SelectionChangedEventArgs eventArgs)
		{
			_isAutoScrollToSelectedIndex = true;
			SelectionChanged?.Invoke(this, eventArgs);
		}

		/// <summary>
		/// Triggers the tapped event for the segment item.
		/// </summary>
		/// <param name="eventArgs">The <see cref="SegmentTappedEventArgs"/> containing the containing the tapped item.</param>
		void ISegmentItemInfo.TriggerTappedEvent(SegmentTappedEventArgs eventArgs)
		{
			this.Tapped?.Invoke(this, eventArgs);
		}

		/// <summary>
		/// Updates the selected index for the segment item.
		/// </summary>
		/// <param name="index">The new selected index value.</param>
		void ISegmentItemInfo.UpdateSelectedIndex(int index)
		{
			_isAutoScrollToSelectedIndex = false;
			SelectedIndex = index;
		}

		/// <summary>
		/// Methods to clear the keyboard focused view.
		/// </summary>
		void ISegmentItemInfo.ClearFocusedView()
		{
			_keyNavigationView?.ClearFocusedView();
		}

		/// <summary>
		/// Updates the scroll view position to focused index for the segment item.
		/// </summary>
		/// <param name="index">The focused index value.</param>
		void ISegmentItemInfo.UpdateScrollOnKeyNavigation(int index)
		{
			SegmentItemView? focusedView = _segmentLayout?.GetSegmentItemView(index);
			if (focusedView == null)
			{
				return;
			}

			_keyNavigationView?.UpdateNavigationView(focusedView._segmentItem, focusedView.Bounds);
			UpdateScrollOnKeyNavigation(focusedView);
		}

		#endregion
	}
}