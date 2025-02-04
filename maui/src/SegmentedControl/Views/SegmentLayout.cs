using Syncfusion.Maui.Toolkit.Graphics.Internals;
using Syncfusion.Maui.Toolkit.Internals;

namespace Syncfusion.Maui.Toolkit.SegmentedControl
{
	/// <summary>
	/// Represents a layout for arranging and displaying a collection of segment items.
	/// </summary>
#if WINDOWS
	internal partial class SegmentLayout : SfView, IKeyboardListener
#else
	internal class SegmentLayout : SfView
#endif
	{
		#region Fields

		/// <summary>
		/// The segment item info.
		/// </summary>
		readonly ISegmentItemInfo _itemInfo;

		/// <summary>
		/// A collection of segment item views.
		/// </summary>
		List<SegmentItemView>? _segmentItemViews;

		/// <summary>
		/// The index of the currently focused item for keyboard interactions.
		/// </summary>
		int _focusedIndex = -1;

#if ANDROID
		/// <summary>
		/// The height of the segmented control.
		/// </summary>
		private double _segmentLayoutHeight;
#endif

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="SegmentLayout"/> class with the specified segment information.
		/// </summary>
		/// <param name="itemInfo">The <see cref="ISegmentItemInfo"/> providing information about the segment items.</param>
		internal SegmentLayout(ISegmentItemInfo itemInfo)
		{
			FlowDirection = Microsoft.Maui.FlowDirection.LeftToRight;
			_itemInfo = itemInfo;
			DrawingOrder = DrawingOrder.AboveContent;
			InitializeSegmentItemsView();
#if WINDOWS
			this.AddKeyboardListener(this);
#endif
		}

		#endregion

		#region Internal methods

		/// <summary>
		/// Updates the selection of the segment items in the layout.
		/// </summary>
		internal void UpdateItemSelection()
		{
			int selectedIndex = GetSelectedIndex();
			SegmentItemView? olditemView = Children.OfType<SegmentItemView>().FirstOrDefault(x => x._isSelected);
			SegmentItemView? newitemView = GetSegmentItemView(selectedIndex);

			olditemView?.ClearSelection();

			// Resets the focused index to -1 when the selection is updated from the segment item view.
			_focusedIndex = -1;
			newitemView?.UpdateSelection();
		}

		/// <summary>
		/// Triggers the selection changed event for the layout.
		/// </summary>
		internal void TriggerSelectionChanged()
		{
			int newIndex = GetSelectedIndex();

			// Get the item view at the specified index.
			SegmentItemView? newItemView = GetSegmentItemView(newIndex);
			if (newItemView == null)
			{
				return;
			}

			newItemView.UpdateSelection();
			SelectionChangedEventArgs eventArgs = new SelectionChangedEventArgs() { OldIndex = -1, NewIndex = newIndex, OldValue = null, NewValue = newItemView._segmentItem };
			_itemInfo.TriggerSelectionChangedEvent(eventArgs);
		}

		/// <summary>
		/// Updates the enabled state of the item view at the specified index.
		/// </summary>
		/// <param name="index">The index of the item view to be updated.</param>
		/// <param name="isEnabled">Determines whether the item view should be enabled (true) or disabled (false).</param>
		internal void UpdateItemViewEnabledState(int index, bool isEnabled)
		{
			// Get the item view at the specified index.
			SegmentItemView? itemView = GetSegmentItemView(index);
			if (itemView == null)
			{
				return;
			}

			itemView.UpdateItemViewEnabledState(isEnabled);
		}

		/// <summary>
		/// Updates the KeyNavigationView when scrolling occurs.
		/// </summary>
		internal void UpdateKeyNavigationViewOnScroll()
		{
			if (_focusedIndex < 0)
			{
				return;
			}

			// As we handled key navigation focused view at the parent layout. So while scrolling views we need to update the key navigation for the item at the focused index based on scrolling.
			_itemInfo.UpdateScrollOnKeyNavigation(_focusedIndex);
		}

		/// <summary>
		/// Method to enable/disable the segment layout and its children interaction based on isEnabled property values.
		/// </summary>
		/// <param name="isEnabled">A boolean indicating whether the layout should be enabled or disabled.</param>
		internal void UpdateLayoutInteraction(bool isEnabled)
		{
			IsEnabled = isEnabled;
			if (_segmentItemViews == null)
			{
				return;
			}

			foreach (SegmentItemView itemView in _segmentItemViews)
			{
				itemView.UpdateItemViewEnabledState(isEnabled);
			}
		}

		/// <summary>
		/// Method to process on key down.
		/// </summary>
		/// <param name="e">The key event args.</param>
		internal void ProcessOnKeyDown(KeyEventArgs e)
		{
			if (e.Handled || _itemInfo == null || _itemInfo.Items == null)
			{
				return;
			}

			// Get the RTL layout value from the segment info.
			bool isRTL = _itemInfo.IsRTL;

			// Initialize the selected index if it's not set yet.
			if (_focusedIndex == -1)
			{
				_focusedIndex = GetSelectedIndex();
			}

			// Get the next or previous moved item index based on the arrow key and RTL layout.
			if (e.Key == KeyboardKey.Right)
			{
				SegmentItemViewHelper.GetNextFocusableItemIndex(_itemInfo, ref _focusedIndex, isRTL);
				_itemInfo.UpdateScrollOnKeyNavigation(_focusedIndex);
			}
			else if (e.Key == KeyboardKey.Left)
			{
				SegmentItemViewHelper.GetPreviousFocusableItemIndex(_itemInfo, ref _focusedIndex, isRTL);
				_itemInfo.UpdateScrollOnKeyNavigation(_focusedIndex);
			}

			if (e.Key == KeyboardKey.Enter)
			{
				// Ignored if the segment item is already selected.
				if (_focusedIndex == _itemInfo.SelectedIndex)
				{
					// Resetting to current index to -1. because the selection or mouse hover will be updated from the selected index next time when the keyboard is pressed.
					_focusedIndex = -1;
					return;
				}

				// Clear the selection of the previously selected item and update the new selected index.
				SegmentItemView? oldSelectedView = GetSegmentItemView(GetSelectedIndex());
				oldSelectedView?.ClearSelection();

				// Update the new selected index and wire the selection changed event.
				_itemInfo?.UpdateSelectedIndex(_focusedIndex);

				// Resetting to current index to -1. because the selection or mouse hover will be updated from the selected index next time when the keyboard is pressed.
				_focusedIndex = -1;
			}

#if WINDOWS
			if (e.Key == KeyboardKey.Left || e.Key == KeyboardKey.Right)
			{
				_itemInfo?.SetFocusVisualState(false);
			}
#endif

			if (e.Key == KeyboardKey.Tab || (e.Key == KeyboardKey.Tab && e.IsShiftKeyPressed) || e.Key == KeyboardKey.Down || e.Key == KeyboardKey.Up)
			{
				e.Handled = false;
				_focusedIndex = -1;
				_itemInfo?.ClearFocusedView();
#if WINDOWS
				_itemInfo?.SetFocusVisualState(true);
#endif
				return;
			}

			e.Handled = true;
		}

		/// <summary>
		/// Gets the segment item view at the specified index.
		/// </summary>
		/// <param name="index">The index of the segment item.</param>
		/// <returns>The item view.</returns>
		internal SegmentItemView? GetSegmentItemView(int index)
		{
			return index >= 0 ? (SegmentItemView?)Children.ElementAtOrDefault(index) : null;
		}

		/// <summary>
		/// Updates the layout by initializing segment item views and invalidating the current layout.
		/// </summary>
		internal void UpdateLayout()
		{
			InitializeSegmentItemsView();
			InvalidateLayout();
		}

		/// <summary>
		/// Method to invalidate the layout.
		/// </summary>
		internal void InvalidateLayout()
		{
			InvalidateMeasure();
		}

		/// <summary>
		/// Method to update the flow direction for the segmented control.
		/// </summary>
		internal void UpdateFlowDirection()
		{
			InvalidateLayout();
			InvalidateDrawable();
			InvalidateSegmentItemsLayout();
			InvalidateSegmentItemsDraw();
		}

		/// <summary>
		/// Method to invalidate the segment items layout.
		/// </summary>
		internal void InvalidateSegmentItemsLayout()
		{
			if (_segmentItemViews == null)
			{
				return;
			}

			foreach (SegmentItemView itemView in _segmentItemViews)
			{
				itemView.InvalidateLayout();
			}
		}

		/// <summary>
		/// Method to invalidate the segment items view draw.
		/// </summary>
		internal void InvalidateSegmentItemsDraw()
		{
			if (_segmentItemViews == null)
			{
				return;
			}

			foreach (SegmentItemView itemView in _segmentItemViews)
			{
				itemView.InvalidateDrawableView();
			}
		}

		/// <summary>
		/// Updates the visual style for selected segment items.
		/// </summary>
		internal void UpdateSelectedSegmentItemStyle()
		{
			if (_segmentItemViews == null)
			{
				return;
			}

			foreach (SegmentItemView itemView in _segmentItemViews)
			{
				if (!itemView._isSelected)
				{
					continue;
				}

				itemView.InvalidateDrawableView();
			}
		}

		/// <summary>
		/// Updates the visual representation of segment items based on the current data.
		/// </summary>
		/// <remarks>
		/// This method applies corner radius settings to segment items to properly display borders for selection.
		/// </remarks>
		internal void UpdateSegmentItemsView()
		{
			var items = _itemInfo.Items;
			if (items == null || items.Count == 0)
			{
				return;
			}

			// Apply corner radius settings to segment items.
			for (int i = 0; i < items.Count; i++)
			{
				SfSegmentItem segmentItem = items[i];
				// As we have clipped the parent grid view based on corner radius. so we need to set the corner radius for the segment item first and last views to show the border properly for segment item selections.
				segmentItem.CornerRadius = GetSegmentCornerRadius(i);
			}

			// Invalidate the drawing of segment items to reflect the changes.
			InvalidateSegmentItemsDraw();
		}

		/// <summary>
		/// Updates the visual style for disabled segment items.
		/// </summary>
		internal void UpdateDisabledSegmentItemStyle()
		{
			if (_segmentItemViews == null)
			{
				return;
			}

			foreach (SegmentItemView itemView in _segmentItemViews)
			{
				if (itemView.IsEnabled)
				{
					continue;
				}

				itemView.InvalidateDrawableView();
			}
		}

		/// <summary>
		/// Clears the segment item views.
		/// </summary>
		internal void ClearSegmentItemsView()
		{
			RemoveSegmentItemViewHandler();
		}

		/// <summary>
		/// Replaces the segment item at the specified index with the new segment item.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="newSegmentItem">The new segment item.</param>
		internal void ReplaceSegmentItem(int index, SfSegmentItem newSegmentItem)
		{
			var items = _itemInfo.Items;
			if (items == null || items.Count == 0 || index < 0 || index >= items.Count)
			{
				return;
			}

			if (_segmentItemViews == null || index >= _segmentItemViews.Count)
			{
				return;
			}

			WireSegmentItemPropertyChanged(newSegmentItem);
			//// Create a new segment item view for the new segment item
			newSegmentItem.CornerRadius = GetSegmentCornerRadius(index);
			SegmentItemView newSegmentItemView = new SegmentItemView(_itemInfo, newSegmentItem);
			//// Add the new segment item view to the segment item views list and to the children
			_segmentItemViews[index] = newSegmentItemView;
			Children.RemoveAt(index);
			Children.Insert(index, newSegmentItemView);
		}

		/// <summary>
		/// Method to remove the segment item view handler based on index.
		/// </summary>
		/// <param name="index">The index.</param>
		internal void RemoveSegmentItemView(int index)
		{
			if (_segmentItemViews == null || _segmentItemViews.Count == 0 || index < 0 || index >= _segmentItemViews.Count)
			{
				return;
			}

			SegmentItemView child = _segmentItemViews[index];
			if (child != null)
			{
				child.Dispose();
				UnWireSegmentItemPropertyChanged(child._segmentItem);
				Remove(child);
				if (child.Handler != null && child.Handler.PlatformView != null)
				{
					child.Handler.DisconnectHandler();
				}
			}

			_segmentItemViews.RemoveAt(index);
			if (_segmentItemViews.Count == 0)
			{
				_segmentItemViews = null;
			}
		}

		/// <summary>
		/// Adds or removes segment item views based on the current segment items collection.
		/// </summary>
		internal void AddSegmentItemView(int index)
		{
			var items = _itemInfo.Items;
			if (items == null || items.Count == 0)
			{
				return;
			}

			_segmentItemViews ??= [];

			SfSegmentItem segmentItem = items[index];
			WireSegmentItemPropertyChanged(segmentItem);
			segmentItem.CornerRadius = GetSegmentCornerRadius(index);
			//// Add new segment item view if it doesn't exist
			SegmentItemView segmentItemView = new SegmentItemView(_itemInfo, segmentItem);
			_segmentItemViews.Insert(index, segmentItemView);
			Children.Insert(index, segmentItemView);
		}

		/// <summary>
		/// Moves the segment item at the specified index to a new index.
		/// </summary>
		/// <param name="oldIndex">The old index.</param>
		/// <param name="newIndex">The new index.</param>
		internal void MoveSegmentItem(int oldIndex, int newIndex)
		{
			var items = _itemInfo.Items;
			if (items == null || items.Count == 0 || oldIndex < 0 || oldIndex >= items.Count || newIndex < 0 || newIndex >= items.Count)
			{
				return;
			}

			if (_segmentItemViews == null || oldIndex >= _segmentItemViews.Count || newIndex >= _segmentItemViews.Count)
			{
				return;
			}

			// Move the segment item in the items list
			SfSegmentItem segmentItem = items[oldIndex];
			items.RemoveAt(oldIndex);
			items.Insert(newIndex, segmentItem);

			// Move the segment item view in the segmentItemViews list
			SegmentItemView segmentItemView = _segmentItemViews[oldIndex];
			_segmentItemViews.RemoveAt(oldIndex);
			_segmentItemViews.Add(segmentItemView);

			// Remove and re-add the segment item view in the children collection
			Children.RemoveAt(oldIndex);
			Children.Insert(newIndex, segmentItemView);
		}

#if ANDROID
		/// <summary>
		/// Method to update the segmented layout height.
		/// </summary>
		/// <param name="height">The height</param>
		internal void UpdateSegmentLayoutHeight(double height)
		{
			if (_segmentLayoutHeight == height)
			{
				return;
			}

			_segmentLayoutHeight = height;
		}
#endif

		#endregion

		#region Private methods

		/// <summary>
		/// Initializes the view with the segment items.
		/// </summary>
		void InitializeSegmentItemsView()
		{
			var items = _itemInfo.Items;
			if (items == null || items.Count == 0)
			{
				return;
			}

			_segmentItemViews = [];
			for (int i = 0; i < items.Count; i++)
			{
				SfSegmentItem segmentItem = items[i];
				WireSegmentItemPropertyChanged(segmentItem);
				// As we have clipped the parent grid view based on corner radius. so we need to set the corner radius for the segment item first and last views to show the border properly for segment item selections.
				segmentItem.CornerRadius = GetSegmentCornerRadius(i);
				SegmentItemView segmentItemView = new SegmentItemView(_itemInfo, segmentItem);
				_segmentItemViews.Add(segmentItemView);
				Children.Add(segmentItemView);
			}
		}

		/// <summary>
		/// Gets the corner radius for a specific segment at the given index in the <see cref="SfSegmentedControl"/>.
		/// </summary>
		/// <param name="index">The index of the segment to retrieve the corner radius for.</param>
		/// <returns>
		/// The corner radius for the segment at the specified index.
		/// </returns>
		CornerRadius GetSegmentCornerRadius(int index)
		{
			if (_itemInfo == null || _itemInfo.Items == null || _itemInfo.Items.Count < 0)
			{
				return default;
			}

			CornerRadius cornerRadius = _itemInfo.CornerRadius;
			CornerRadius segmentCornerRadius = _itemInfo.SegmentCornerRadius;
			var items = _itemInfo.Items;
			bool isRTL = _itemInfo.IsRTL;

			// When the SegmentCornerRadius property is set, this method manages key navigation behavior to ensure a consistent user experience that respects the specified corner radius for segmented items.
			if (segmentCornerRadius != default)
			{
				return segmentCornerRadius;
			}

			// If the items collection contains only one item, apply a corner radius to the selected item on all four sides.
			if (items.Count == 1)
			{
				return cornerRadius;
			}
			else if (index == 0)
			{
				if (isRTL)
				{
					return new CornerRadius(0, cornerRadius.TopRight, 0, cornerRadius.BottomRight);
				}

				return new CornerRadius(cornerRadius.TopLeft, 0, cornerRadius.BottomLeft, 0);
			}
			else if (index == items.Count - 1)
			{
				if (isRTL)
				{
					return new CornerRadius(cornerRadius.TopLeft, 0, cornerRadius.BottomLeft, 0);
				}

				return new CornerRadius(0, cornerRadius.TopRight, 0, cornerRadius.BottomRight);
			}

			return default;
		}

		/// <summary>
		/// Method to draw the dividers between segment items.
		/// </summary>
		/// <param name="canvas">The <see cref="ICanvas"/> on which to draw the dividers.</param>
		/// <param name="dirtyRect">The dirty rectangle specifying the area to be redrawn.</param>
		void DrawItemDivider(ICanvas canvas, RectF dirtyRect)
		{
			// In Android, we manually restrict line drawing if stroke thickness is zero.
			if (_itemInfo.StrokeThickness == 0)
			{
				return;
			}

			canvas.CanvasSaveState();
			float strokeThickness = (float)_itemInfo.StrokeThickness;
			double strokeRadius = strokeThickness / 2;

			canvas.Antialias = true;
			canvas.StrokeSize = strokeThickness;
			canvas.StrokeColor = SegmentViewHelper.BrushToColorConverter(_itemInfo.Stroke);
			int ChildrenCount = Children.Count;
			for (int i = 0; i < ChildrenCount; i++)
			{
				View child = (View)Children[i];
				if (child == null || i == ChildrenCount - 1)
				{
					continue;
				}

				// Calculate the x-position for drawing the divider line.
				float xPos = (float)(_itemInfo.IsRTL ? child.Bounds.Left - strokeRadius : child.Bounds.Right + strokeRadius);

				// Draw the line on the right side of the segment.
				canvas.DrawLine(new Point(xPos, dirtyRect.Top), new Point(xPos, dirtyRect.Height));
			}

			canvas.CanvasRestoreState();
		}

		/// <summary>
		/// Gets the index of the currently selected segment in the <see cref="SfSegmentedControl"/>.
		/// </summary>
		/// <returns>
		/// The index of the selected segment if a segment is currently selected; otherwise, -1.
		/// </returns>
		int GetSelectedIndex()
		{
			return _itemInfo.SelectedIndex ?? -1;
		}

		/// <summary>
		/// Method to wire the segment items property changed event.
		/// </summary>
		/// <param name="segmentItem">The segment item.</param>
		void WireSegmentItemPropertyChanged(SfSegmentItem? segmentItem)
		{
			if (segmentItem != null)
			{
				segmentItem.PropertyChanged += OnSegmentItemPropertyChanged;
			}
		}

		/// <summary>
		/// Method to unwire the segment items property changed event.
		/// </summary>
		/// <param name="segmentItem">The segment item.</param>
		void UnWireSegmentItemPropertyChanged(SfSegmentItem? segmentItem)
		{
			if (segmentItem != null)
			{
				segmentItem.PropertyChanged -= OnSegmentItemPropertyChanged;
			}
		}

		/// <summary>
		/// Method invokes on segment item property changed.
		/// </summary>
		/// <param name="sender">The segment item.</param>
		/// <param name="e">The property changed event arguments.</param>
		void OnSegmentItemPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (sender == null)
			{
				return;
			}

			SfSegmentItem? segmentItem = (SfSegmentItem)sender;
			string? propertyName = e.PropertyName;
			if (segmentItem == null || propertyName == null)
			{
				return;
			}

			if (propertyName != nameof(SfSegmentItem.TextStyle) && propertyName != nameof(SfSegmentItem.Background) &&
				propertyName != nameof(SfSegmentItem.SelectedSegmentTextColor) && propertyName != nameof(SfSegmentItem.SelectedSegmentBackground) &&
				propertyName != nameof(SfSegmentItem.ImageSource))
			{
				return;
			}

			if (propertyName == nameof(SfSegmentItem.TextStyle) || propertyName == nameof(SfSegmentItem.Background))
			{
				InvalidateSegmentItemsDraw();
				return;
			}

			// Ticket-533501- Change the image source when item is selected.
			if (propertyName == nameof(SfSegmentItem.ImageSource))
			{
				foreach (SegmentItemView child in Children.Cast<SegmentItemView>())
				{
					if (child._segmentItem.Equals(segmentItem))
					{
						child.UpdateImageView();
						break;
					}
				}

				return;
			}

			UpdateSelectedSegmentItemStyle();
		}

		/// <summary>
		/// Method to remove the segment item view handlers.
		/// </summary>
		void RemoveSegmentItemViewHandler()
		{
			if (_segmentItemViews == null || _segmentItemViews.Count == 0)
			{
				return;
			}

			foreach (SegmentItemView child in _segmentItemViews)
			{
				child.Dispose();
				UnWireSegmentItemPropertyChanged(child._segmentItem);
				Remove(child);
				if (child.Handler != null && child.Handler.PlatformView != null)
				{
					child.Handler.DisconnectHandler();
				}
			}

			_segmentItemViews.Clear();
			_segmentItemViews = null;
		}

#if ANDROID
		/// <summary>
		/// Method to get the valid height for the segment layout based on the segment layout height and segment item height.
		/// </summary>
		/// <returns>Returns the height.</returns>
		double GetValidHeight()
		{
			if (_segmentLayoutHeight <= 0)
			{
				return 0;
			}
			else if (_segmentLayoutHeight < _itemInfo.SegmentHeight)
			{
				return _segmentLayoutHeight;
			}
			else
			{
				return _itemInfo.SegmentHeight;
			}
		}
#endif

		#endregion

		#region Override methods

		/// <summary>
		/// Called when the layout needs to be drawn.
		/// </summary>
		/// <param name="canvas">The <see cref="ICanvas"/> on which the layout is drawn.</param>
		/// <param name="dirtyRect">The dirty rectangle specifying the area to be redrawn.</param>
		protected override void OnDraw(ICanvas canvas, RectF dirtyRect)
		{
			if (_itemInfo.ShowSeparator)
			{
				DrawItemDivider(canvas, dirtyRect);
			}

			base.OnDraw(canvas, dirtyRect);
		}

		/// <summary>
		/// Measures the size of the layout content based on the provided width and height constraints.
		/// </summary>
		/// <param name="widthConstraint">The maximum width constraint for the layout.</param>
		/// <param name="heightConstraint">The maximum height constraint for the layout.</param>
		/// <returns>The measured size of the layout content.</returns>
		protected override Size MeasureContent(double widthConstraint, double heightConstraint)
		{
			double width = double.IsFinite(widthConstraint) ? widthConstraint : 0;
			double height = double.IsFinite(heightConstraint) ? heightConstraint : 0;
			double strokeThickness = _itemInfo.StrokeThickness;
			bool showSeparator = _itemInfo.ShowSeparator;
#if ANDROID
			height = GetValidHeight();
#else
			if (!double.IsFinite(heightConstraint))
			{
				height = _itemInfo.SegmentHeight;
			}
#endif

			if (!double.IsFinite(widthConstraint))
			{
				for (int i = 0; i < Children.Count; i++)
				{
					SegmentItemView child = (SegmentItemView)Children[i];
					double itemWidth = SegmentItemViewHelper.GetItemWidth(_itemInfo, child._segmentItem);
					if (showSeparator)
					{
						itemWidth -= strokeThickness;
					}

					Children[i].Measure(itemWidth, height);
					width += itemWidth + (showSeparator ? strokeThickness : 0);
				}
			}

			return new Size(width, height);
		}

		/// <summary>
		/// Arranges the layout content within the specified bounds.
		/// </summary>
		/// <param name="bounds">The bounds within which the layout content should be arranged.</param>
		/// <returns>The arranged size of the layout content.</returns>
		protected override Size ArrangeContent(Rect bounds)
		{
			double strokeThickness = _itemInfo.StrokeThickness;
			double strokeRadius = strokeThickness / 2;
			bool isRTL = _itemInfo.IsRTL;

			// If RTL, adjust x-position to start from the right side of the bounds.
			double xPos = isRTL ? bounds.Right - strokeRadius : bounds.Left + strokeRadius;
			double yPos = bounds.Top;
#if ANDROID
			double height = GetValidHeight();
#else
			double height = bounds.Height;
#endif
			double width;

			bool showSeparator = _itemInfo.ShowSeparator;
			for (int i = 0; i < Children.Count; i++)
			{
				SegmentItemView child = (SegmentItemView)Children[i];
				width = SegmentItemViewHelper.GetItemWidth(_itemInfo, child._segmentItem);
				if (showSeparator)
				{
					width -= strokeThickness;
				}

				if (isRTL)
				{
					xPos -= width;
					Children[i].Arrange(new Rect(xPos, yPos, width, height));
					xPos -= showSeparator ? strokeThickness : 0;
					continue;
				}

				Children[i].Arrange(new Rect(xPos, yPos, width, height));
				xPos += width + (showSeparator ? strokeThickness : 0);
			}

			return bounds.Size;
		}

		#endregion

		#region Interface implementation
#if WINDOWS
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
			ProcessOnKeyDown(e);
		}

		/// <inheritdoc/>
		void IKeyboardListener.OnKeyUp(KeyEventArgs e)
		{

		}
#endif
		#endregion
	}
}