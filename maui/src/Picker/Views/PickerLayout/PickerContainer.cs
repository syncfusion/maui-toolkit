using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using Syncfusion.Maui.Toolkit.Internals;

namespace Syncfusion.Maui.Toolkit.Picker
{
    /// <summary>
    /// This represents a class that contains information about the picker container.
    /// </summary>
    internal class PickerContainer : SfView
    {
        #region Fields

        /// <summary>
        /// The stroke thickness.
        /// </summary>
        const float StrokeThickness = 1;

        /// <summary>
        /// The picker info.
        /// </summary>
        readonly IPicker _pickerInfo;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="PickerContainer"/> class.
        /// </summary>
        /// <param name="pickerInfo">The picker info.</param>
        internal PickerContainer(IPicker pickerInfo)
        {
            DrawingOrder = DrawingOrder.BelowContent;
            _pickerInfo = pickerInfo;
#if IOS
            IgnoreSafeArea = true;
#endif
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Method to update the Picker keyboard interaction.
        /// </summary>
        /// <param name="args">The keyboard event args.</param>
        /// <param name="columnIndex">The column index.</param>
        internal void PickerRightLeftKeys(KeyEventArgs args, int columnIndex)
        {
            int columnCount = Children.Count;
            if (args.Key == KeyboardKey.Right)
            {
                columnIndex = (columnIndex + 1) % columnCount;
            }
            else if (args.Key == KeyboardKey.Left)
            {
                if (columnIndex == 0)
                {
                    columnIndex = columnCount - 1;
                }
                else
                {
                    columnIndex = Math.Abs(columnIndex - 1);
                    columnIndex = columnIndex == 0 ? columnIndex : columnIndex % columnCount;
                }
            }

            PickerLayout? pickerLayout = Children[columnIndex] as PickerLayout;
            pickerLayout?.UpdateKeyboardLayoutFocus();
        }

        /// <summary>
        /// Method to update the selected index changed.
        /// </summary>
        /// <param name="columnIndex">The updated column index.</param>
        internal void UpdateSelectedIndexValue(int columnIndex)
        {
            if (columnIndex >= Children.Count)
            {
                return;
            }

            PickerLayout? pickerLayout = Children[columnIndex] as PickerLayout;
            if (pickerLayout == null)
            {
                return;
            }

            pickerLayout.UpdateSelectedIndexValue();
        }

        /// <summary>
        /// Method to update the items source changed.
        /// </summary>
        /// <param name="columnIndex">The updated column index.</param>
        [RequiresUnreferencedCode("The GetPropertyValue method is not trim compatible")]
        internal void UpdateItemsSource(int columnIndex)
        {
            if (columnIndex >= Children.Count)
            {
                return;
            }

            PickerLayout? pickerLayout = Children[columnIndex] as PickerLayout;
            if (pickerLayout == null)
            {
                return;
            }

            pickerLayout.UpdateItemSource(false);
        }

        /// <summary>
        /// Method to invokes while the columns collection changed.
        /// </summary>
        /// <param name="e">The collection changed event arguments.</param>
        [RequiresUnreferencedCode("The GetPropertyValue method is not trim compatible")]
        internal void OnColumnsCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (Children.Count == 0)
            {
                TriggerPickerContainerMeasure();
                UpdatePickerSelectionViewDraw();
                return;
            }

            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (e.NewStartingIndex == -1)
                {
                    return;
                }

                PickerColumn column = _pickerInfo.Columns[e.NewStartingIndex];
                Insert(e.NewStartingIndex, new PickerLayout(_pickerInfo, column));
                TriggerPickerContainerMeasure();
                UpdatePickerSelectionViewDraw();
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                if (e.OldStartingIndex == -1)
                {
                    return;
                }

                Remove((View)Children[e.OldStartingIndex]);
                UpdatePickerSelectionViewDraw();
                TriggerPickerContainerMeasure();
            }
            else if (e.Action == NotifyCollectionChangedAction.Replace)
            {
                if (e.NewStartingIndex == -1)
                {
                    return;
                }

                PickerLayout? pickerLayout = Children[e.NewStartingIndex] as PickerLayout;
                if (pickerLayout != null)
                {
                    PickerColumn column = _pickerInfo.Columns[e.NewStartingIndex];
                    pickerLayout.UpdateColumnData(column);
                    pickerLayout.UpdateItemSource(true);
                    pickerLayout.UpdateSelectedIndexValue();
                }

                TriggerPickerContainerMeasure();
            }
            else if (e.Action == NotifyCollectionChangedAction.Move)
            {
                if (e.NewStartingIndex == -1)
                {
                    return;
                }

                PickerLayout? pickerLayout = Children[e.OldStartingIndex] as PickerLayout;
                if (pickerLayout != null)
                {
                    Remove(pickerLayout);
                    Insert(e.NewStartingIndex, pickerLayout);
                }

                TriggerPickerContainerMeasure();
            }
            else if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                ResetPickerColumns(_pickerInfo.Columns);
            }
        }

        /// <summary>
        /// Method to the update the picker container.
        /// </summary>
        /// <param name="newColumns">The new columns details.</param>
        [RequiresUnreferencedCode("The GetPropertyValue method is not trim compatible")]
        internal void ResetPickerColumns(ObservableCollection<PickerColumn> newColumns)
        {
            int columnsCount = newColumns.Count;
            if (columnsCount <= Children.Count)
            {
                for (int index = 0; index < columnsCount; index++)
                {
                    PickerColumn pickerColumn = newColumns[index];
                    PickerLayout? pickerLayout = Children[index] as PickerLayout;
                    if (pickerLayout != null)
                    {
                        pickerLayout.UpdateColumnData(pickerColumn);
                        pickerLayout.UpdateColumnHeaderText();
                        pickerLayout.UpdateItemSource(true);
                    }
                }

                for (int index = Children.Count - 1; index >= columnsCount; index--)
                {
                    Remove((View)Children[index]);
                }
            }
            else
            {
                for (int index = 0; index < Children.Count; index++)
                {
                    PickerColumn pickerColumn = newColumns[index];
                    PickerLayout? pickerLayout = Children[index] as PickerLayout;
                    if (pickerLayout != null)
                    {
                        pickerLayout.UpdateColumnData(pickerColumn);
                        pickerLayout.UpdateColumnHeaderText();
                        pickerLayout.UpdateItemSource(true);
                    }
                }

                for (int index = Children.Count; index < columnsCount; index++)
                {
                    PickerColumn pickerColumn = newColumns[index];
                    Add(new PickerLayout(_pickerInfo, pickerColumn));
                }
            }

            TriggerPickerContainerMeasure();
            UpdatePickerSelectionViewDraw();
        }

        /// <summary>
        /// Method to update the scroll view draw.
        /// </summary>
        internal void UpdateScrollViewDraw()
        {
            foreach (var child in Children)
            {
                if (child is PickerLayout pickerLayout)
                {
                    pickerLayout.UpdateScrollViewDraw();
                }
            }
        }

        /// <summary>
        /// Method to update the picker item template changes.
        /// </summary>
        internal void UpdateItemTemplate()
        {
            foreach (var child in Children)
            {
                if (child is PickerLayout pickerLayout)
                {
                    pickerLayout.UpdateItemTemplate();
                }
            }
        }

        /// <summary>
        /// Method to update the column header height.
        /// </summary>
        internal void UpdateColumnHeaderHeight()
        {
            //// Need to update the picker layout while change the column header height. Because , the picker layout height is based on the column header height.
            foreach (PickerLayout pickerLayout in Children)
            {
                pickerLayout.UpdateColumnHeaderHeight();
            }

            //// After update the picker layout height, need to update the picker container height.
            TriggerPickerContainerMeasure();
            UpdatePickerSelectionViewDraw();
        }

        /// <summary>
        /// Method to update the picker column header draw.
        /// </summary>
        internal void UpdateColumnHeaderDraw()
        {
            //// Need to update the picker layout drawing when changing the column header text style, background color, because the column header drawing is within the picker layout.
            foreach (PickerLayout pickerLayout in Children)
            {
                pickerLayout.UpdateColumnHeaderDraw();
            }
        }

        /// <summary>
        /// Method to update the column divider color.
        /// </summary>
        internal void UpdateColumnDividerColor()
        {
            //// Need to update the picker layout drawing when changing the column divider color, because the column divider drawing is within the picker layout.
            foreach (PickerLayout pickerLayout in Children)
            {
                pickerLayout.UpdateColumnDividerColor();
            }
        }

        /// <summary>
        /// Method to update the column header divider color.
        /// </summary>
        internal void UpdateColumnHeaderDividerColor()
        {
            foreach (PickerLayout pickerLayout in Children)
            {
                pickerLayout.UpdateColumnHeaderDividerColor();
            }
        }

        /// <summary>
        /// Method to update the item height.
        /// </summary>
        internal void UpdateItemHeight()
        {
            foreach (PickerLayout pickerLayout in Children)
            {
                pickerLayout.UpdateItemHeight();
            }

            UpdatePickerSelectionViewDraw();
        }

        /// <summary>
        /// Method to update the picker column width.
        /// </summary>
        internal void UpdatePickerColumnWidth()
        {
            TriggerPickerContainerMeasure();
            UpdatePickerSelectionViewDraw();
        }

        /// <summary>
        /// Method to used to update the header text.
        /// </summary>
        /// <param name="columnIndex">The column index.</param>
        internal void UpdateHeaderText(int columnIndex)
        {
            if (columnIndex >= Children.Count)
            {
                return;
            }

            PickerLayout? pickerLayout = Children[columnIndex] as PickerLayout;
            if (pickerLayout == null)
            {
                return;
            }

            pickerLayout.UpdateColumnHeaderText();
        }

        /// <summary>
        /// Method to update the picker selection view.
        /// </summary>
        internal void UpdatePickerSelectionView()
        {
            UpdatePickerSelectionViewDraw();
        }

        /// <summary>
        /// Method to update the enable looping.
        /// </summary>
        internal void UpdateEnableLooping()
        {
            foreach (PickerLayout pickerLayout in this.Children)
            {
                pickerLayout.UpdateEnableLooping();
            }
        }

		/// <summary>
		/// Scrolls the specified column to the given index visually without updating SelectedIndex.
		/// </summary>
		/// <param name="columnIndex">The index of the column to scroll.</param>
		/// <param name="targetIndex">The index to scroll to.</param>
		internal void ScrollToSelectedIndex(int columnIndex, int targetIndex)
		{
			if (columnIndex >= Children.Count)
			{
				return;
			}
			if (Children[columnIndex] is PickerLayout pickerLayout)
			{
				pickerLayout.ScrollToSelectedIndex(targetIndex);
			}
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Method to update the picker selection view draw.
		/// </summary>
		void UpdatePickerSelectionViewDraw()
        {
            InvalidateDrawable();
        }

        /// <summary>
        /// Method to add children for the current layout.
        /// </summary>
        [UnconditionalSuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code", Justification = "<Pending>")]
        void GenerateChildren()
        {
            foreach (PickerColumn item in _pickerInfo.Columns)
            {
                Add(new PickerLayout(_pickerInfo, item));
            }
        }

        /// <summary>
        /// Method to get the total assigned width and default column width.
        /// </summary>
        /// <param name="totalWidth">The total width.</param>
        /// <returns>Returns total assigned width and default column width.</returns>
        Point GetDefaultColumnWidth(double totalWidth)
        {
            double assignedWidth = 0;
            int unAssignedColumnCount = 0;
            foreach (PickerColumn column in _pickerInfo.Columns)
            {
                if (column.Width == -1)
                {
                    unAssignedColumnCount++;
                }
                else
                {
                    assignedWidth += column.Width <= -1 ? 0 : column.Width;
                }
            }

            assignedWidth = assignedWidth > totalWidth ? totalWidth : assignedWidth;
            double unAssignedWidth = totalWidth - assignedWidth;
            assignedWidth += unAssignedColumnCount > 0 ? unAssignedWidth : 0;
            double defaultColumnWidth = unAssignedWidth / unAssignedColumnCount;
            //// Here the assigned width is total assigned and default column width is un assigned per column width.
            return new Point(assignedWidth, defaultColumnWidth);
        }

        /// <summary>
        /// Method to trigger the picker container measure.
        /// </summary>
        void TriggerPickerContainerMeasure()
        {
            InvalidateMeasure();
        }

        #endregion

        #region Override Methods

        /// <summary>
        /// Method to draw the selection UI.
        /// </summary>
        /// <param name="canvas">The canvas.</param>
        /// <param name="dirtyRectangle">The dirty rectangle.</param>
        protected override void OnDraw(ICanvas canvas, RectF dirtyRectangle)
        {
            //// The picker selection view is drawn only when the columns count greater than 0.
            if (_pickerInfo.Columns.Count == 0 || !(_pickerInfo.TextDisplayMode == PickerTextDisplayMode.Default))
            {
                return;
            }

            for (int index = 0; index < _pickerInfo.Columns.Count; index++)
            {
                if (_pickerInfo.Columns[index].SelectedItem == null || _pickerInfo.Columns[index].SelectedIndex <= -1)
                {
                    return;
                }
            }

            canvas.SaveState();
            //// The item source height.
            double itemHeight = _pickerInfo.ItemHeight;
            double columnHeaderHeight = _pickerInfo.ColumnHeaderView.Height;
            //// The view port item count is calculated based on the view port height.
            //// Assume the view port height is 100 and item height is 50, then the view port item count is 2.
            //// Update the viewPort ItemCount based on whether the column header template is applied.
            //// If the template is not null, the column header layout is added as a child of the StackLayout, so adjust the viewPortItemCount position accordingly.
            double viewPortItemCount = _pickerInfo.ColumnHeaderTemplate != null ? Math.Round(dirtyRectangle.Height / itemHeight) : Math.Round((dirtyRectangle.Height - columnHeaderHeight) / itemHeight);
            //// The selectionIndex count is calculated based on the view port item count.
            //// Assume the view port item count is 2, then the top count is 0.
            int selectionIndex = (int)Math.Ceiling(viewPortItemCount / 2) - 1;

            if (selectionIndex < 0)
            {
                canvas.RestoreState();
                return;
            }

            //// The top padding is calculated based on the top count and item height.
            //// Assume the selectionIndex is 0 and item height is 50, then the top padding is 0.
            //// Update the Y position based on whether the column header template is applied.
            //// If the template is not null, the column header layout is added as a child of the StackLayout, so adjust the Y position accordingly.
            double yPosition = _pickerInfo.ColumnHeaderTemplate != null ? (selectionIndex * itemHeight) : (selectionIndex * itemHeight) + columnHeaderHeight;
            float xPosition = dirtyRectangle.Left;
            float width = dirtyRectangle.Width;
            float totalColumnWidth = (float)GetDefaultColumnWidth(width).X;
            if (totalColumnWidth == 0)
            {
                canvas.RestoreState();
                return;
            }

            float leftPadding = (float)(width - totalColumnWidth) * 0.5f;
            xPosition += leftPadding;

            //// Calculate the selection highlight rectangle based on padding value.
            Thickness padding = _pickerInfo.SelectionView.Padding;
            xPosition += (float)padding.Left;
            totalColumnWidth -= (float)padding.HorizontalThickness;
            yPosition += padding.Top;
            itemHeight -= padding.VerticalThickness;
            Rect selectionRectangle = new Rect(xPosition, yPosition, totalColumnWidth, itemHeight);
            CornerRadius cornerRadius = _pickerInfo.SelectionView.CornerRadius;

            Color fillColor = _pickerInfo.SelectionView.Background.ToColor();
            if (fillColor != Colors.Transparent)
            {
                canvas.FillColor = fillColor;
                canvas.FillRoundedRectangle(selectionRectangle, cornerRadius.TopLeft, cornerRadius.TopRight, cornerRadius.BottomLeft, cornerRadius.BottomRight);
            }

            if (_pickerInfo.SelectionView.Stroke != Colors.Transparent)
            {
                canvas.StrokeColor = _pickerInfo.SelectionView.Stroke;
                canvas.StrokeSize = StrokeThickness;
                float strokeOffset = StrokeThickness * 0.5f;
                selectionRectangle = new Rect(xPosition + strokeOffset, yPosition + strokeOffset, totalColumnWidth - StrokeThickness, itemHeight - StrokeThickness);
                canvas.DrawRoundedRectangle(selectionRectangle, cornerRadius.TopLeft, cornerRadius.TopRight, cornerRadius.BottomLeft, cornerRadius.BottomRight);
            }

            canvas.RestoreState();
        }

        /// <summary>
        /// Method used to arrange the children with in the bounds.
        /// </summary>
        /// <param name="bounds">The size of the layout.</param>
        /// <returns>The layout size.</returns>
        protected override Size ArrangeContent(Rect bounds)
        {
            Point columnWidthInfo = GetDefaultColumnWidth(bounds.Width);
            double defaultColumnWidth = columnWidthInfo.Y;
            double totalColumnWidth = columnWidthInfo.X;
            int columnCount = _pickerInfo.Columns.Count;
            double xPosition = (bounds.Width - totalColumnWidth) * 0.5;
            for (int index = 0; index < columnCount; index++)
            {
                int actualIndex = _pickerInfo.IsRTLLayout ? columnCount - 1 - index : index;
                var child = Children[actualIndex];
                double columnWidth = _pickerInfo.Columns[actualIndex].Width;
                columnWidth = columnWidth == -1 ? defaultColumnWidth : (columnWidth < -1 ? 0 : columnWidth);
                child.Arrange(new Rect(xPosition, 0, columnWidth, bounds.Height));
                xPosition += columnWidth;
            }

            return bounds.Size;
        }

        /// <summary>
        /// Method to measures child elements size in a container, picker layout is measured with given column width and height constraints.
        /// </summary>
        /// <param name="widthConstraint">The maximum width request of the layout.</param>
        /// <param name="heightConstraint">The maximum height request of the layout.</param>
        /// <returns>The maximum size of the layout.</returns>
        protected override Size MeasureContent(double widthConstraint, double heightConstraint)
        {
            if (Children.Count <= 0)
            {
                GenerateChildren();
            }

            Point columnWidthInfo = GetDefaultColumnWidth(widthConstraint);
            double defaultColumnWidth = columnWidthInfo.Y;

            int childCount = Math.Min(_pickerInfo.Columns.Count, Children.Count);
            for (int index = 0; index < childCount; index++)
            {
                var child = Children[index];
                double columnWidth = _pickerInfo.Columns[index].Width;
                columnWidth = columnWidth == -1 ? defaultColumnWidth : (columnWidth < -1 ? 0 : columnWidth);
                child.Measure(columnWidth, heightConstraint);
            }

            return new Size(widthConstraint, heightConstraint);
        }

        #endregion
    }
}