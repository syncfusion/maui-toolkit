using System.Collections;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Syncfusion.Maui.Toolkit.Picker;

/// <summary>
/// This represents a class that contains information about the picker layout.
/// </summary>
internal class PickerLayout : SfView, IPickerLayout
{
    #region Fields

    /// <summary>
    /// The separator line thickness.
    /// </summary>
    const int StrokeThickness = 1;

    /// <summary>
    /// The picker view info.
    /// </summary>
    readonly IPicker _pickerViewInfo;

    /// <summary>
    /// The picker scroll view.
    /// </summary>
    PickerScrollView _pickerScrollView;

    /// <summary>
    /// The picker column.
    /// </summary>
    PickerColumn _column;

    /// <summary>
    /// The items source.
    /// </summary>
    ObservableCollection<string> _itemsSource;

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="PickerLayout"/> class.
    /// </summary>
    /// <param name="pickerViewInfo">The picker view info.</param>
    /// <param name="pickerColumn">The picker column.</param>
    [RequiresUnreferencedCode("The GetPropertyValue method is not trim compatible")]
    internal PickerLayout(IPicker pickerViewInfo, PickerColumn pickerColumn)
    {
        _pickerViewInfo = pickerViewInfo;
        _column = pickerColumn;
        _itemsSource = new ObservableCollection<string>();
        UpdateItemSource(false);
        DrawingOrder = DrawingOrder.AboveContent;
#if IOS
        IgnoreSafeArea = true;
#endif
        if (_pickerViewInfo.ColumnHeaderTemplate == null)
        {
            ColumnHeaderLayout columnHeaderLayout = new ColumnHeaderLayout(_pickerViewInfo, _column.HeaderText);
            Add(columnHeaderLayout);
        }

        _pickerScrollView = new PickerScrollView(this, _pickerViewInfo, _itemsSource);
        Add(_pickerScrollView);
    }

    #endregion

    #region Public Properties

    /// <summary>
    /// Gets the pickerViewInfo variable of IPickerLayout to the IPickerView property.
    /// </summary>
    IPickerView IPickerLayout.PickerInfo => _pickerViewInfo;

    /// <summary>
    /// Gets the column variable of IPickerLayout to the IPickerView property.
    /// </summary>
    PickerColumn IPickerLayout.Column => _column;

    /// <summary>
    /// Gets the scroll position of the scroll view.
    /// </summary>
    double IPickerLayout.ScrollOffset => _pickerScrollView.ScrollY;

    #endregion

    #region Internal Methods

    /// <summary>
    /// Method to update the column header height.
    /// </summary>
    internal void UpdateColumnHeaderHeight()
    {
        TriggerPickerLayoutMeasure();
        TriggerPickerLayoutDraw();
    }

    /// <summary>
    /// Method to update the column divider color.
    /// </summary>
    internal void UpdateColumnDividerColor()
    {
        TriggerPickerLayoutDraw();
    }

    /// <summary>
    /// Method to update the column header draw.
    /// </summary>
    internal void UpdateColumnHeaderDraw()
    {
        foreach (var child in Children)
        {
            if (child is ColumnHeaderLayout columnHeaderLayout)
            {
                columnHeaderLayout.TriggerColumnHeaderDraw();
            }
        }
    }

    /// <summary>
    /// Method to update the column header divider color.
    /// </summary>
    internal void UpdateColumnHeaderDividerColor()
    {
        TriggerPickerLayoutDraw();
    }

    /// <summary>
    /// Method to update the item height.
    /// </summary>
    internal void UpdateItemHeight()
    {
        foreach (var child in Children)
        {
            if (child is PickerScrollView pickerScrollView)
            {
                pickerScrollView.UpdateItemHeight();
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
            if (child is PickerScrollView pickerScrollView)
            {
                pickerScrollView.UpdateItemTemplate();
            }
        }
    }

    /// <summary>
    /// Method to update the column header text.
    /// </summary>
    internal void UpdateColumnHeaderText()
    {
        foreach (var child in Children)
        {
            if (child is ColumnHeaderLayout columnHeaderLayout)
            {
                columnHeaderLayout.UpdateColumnHeaderText(_column.HeaderText);
            }
        }
    }

    /// <summary>
    /// Method to updater the scroll view draw.
    /// </summary>
    internal void UpdateScrollViewDraw()
    {
        foreach (var child in Children)
        {
            if (child is PickerScrollView pickerScrollView)
            {
                pickerScrollView.InvalidatePickerViewDraw();
            }
        }
    }

    /// <summary>
    /// Method to update the picker column data.
    /// </summary>
    /// <param name="column">The column data.</param>
    internal void UpdateColumnData(PickerColumn column)
    {
        _column = column;
    }

    /// <summary>
    /// Method to update the item source.
    /// </summary>
    /// <param name="isNeedResetColumn">Used to reset the picker scroll view.</param>
    [RequiresUnreferencedCode("The GetPropertyValue method is not trim compatible")]
    internal void UpdateItemSource(bool isNeedResetColumn)
    {
        ObservableCollection<string> dataSource = new ObservableCollection<string>();
        if (_column.ItemsSource != null && _column.ItemsSource is ICollection collection)
        {
            if (!string.IsNullOrEmpty(_column.DisplayMemberPath))
            {
                PropertyInfo? property = null;
                foreach (var item in collection)
                {
                    if (property == null)
                    {
                        property = item.GetType().GetProperty(_column.DisplayMemberPath);
                    }

                    if (property == null)
                    {
                        continue;
                    }

                    string? value = property.GetValue(item) as string;
                    if (value != null)
                    {
                        dataSource.Add(value);
                    }
                }
            }

            if (dataSource.Count == 0)
            {
                foreach (var item in collection)
                {
                    string? text = item?.ToString();
                    if (text != null)
                    {
                        dataSource.Add(text);
                    }
                }
            }
        }

        if (PickerHelper.IsCollectionEquals(_itemsSource, dataSource))
        {
            return;
        }

        _itemsSource = dataSource;
        if (_pickerScrollView == null)
        {
            return;
        }

        if (isNeedResetColumn)
        {
            Remove(_pickerScrollView);
            _pickerScrollView.Dispose();
            _pickerScrollView = new PickerScrollView(this, _pickerViewInfo, _itemsSource);
            Add(_pickerScrollView);
        }
        else
        {
            _pickerScrollView.UpdateItemsSource(_itemsSource);
        }

        TriggerPickerLayoutMeasure();
    }

    /// <summary>
    /// Method to update the selected index changed.
    /// </summary>
    internal void UpdateSelectedIndexValue()
    {
        if (_pickerScrollView == null)
        {
            return;
        }

        if (_pickerScrollView.ContentSize != Size.Zero)
        {
            _pickerScrollView.UpdateSelectedIndexValue();
        }
    }

    /// <summary>
    /// Method to update the focus for the keyboard interaction.
    /// </summary>
    internal void UpdateKeyboardLayoutFocus()
    {
        foreach (var child in Children)
        {
            if (child is PickerScrollView pickerScrollView)
            {
                pickerScrollView.UpdateKeyboardViewFocus();
            }
        }
    }

    /// <summary>
    /// Method to update the enable looping.
    /// </summary>
    internal void UpdateEnableLooping()
    {
        foreach (var child in this.Children)
        {
            if (child is PickerScrollView pickerScrollView)
            {
                pickerScrollView.UpdateEnableLooping();
            }
        }
    }

	/// <summary>
	/// Scrolls the picker scroll view to the specified index visually.
	/// </summary>
	/// <param name="index">The index to scroll to.</param>
	internal void ScrollToSelectedIndex(int index)
	{
		if (_pickerScrollView == null || index < 0 || index >= _itemsSource.Count)
		{
			return;
		}
		double itemHeight = _pickerViewInfo.ItemHeight;
		double scrollOffset = index * itemHeight;
		_pickerScrollView.ScrollToAsync(0, scrollOffset, false);
	}

	#endregion

	#region Private Methods

	/// <summary>
	/// Method to update the picker layout draw.
	/// </summary>
	void TriggerPickerLayoutDraw()
    {
        InvalidateDrawable();
    }

    /// <summary>
    /// Method to update the picker layout measure.
    /// </summary>
    void TriggerPickerLayoutMeasure()
    {
        InvalidateMeasure();
    }

    #endregion

    #region Override Methods

    /// <summary>
    /// Method used to arrange the children with in the bounds.
    /// </summary>
    /// <param name="bounds">The size of the layout.</param>
    /// <returns>The layout size.</returns>
    protected override Size ArrangeContent(Rect bounds)
    {
        double width = bounds.Width;
        double height = bounds.Height;
        double headerColumHeight = _pickerViewInfo.ColumnHeaderView.Height;
        foreach (var child in Children)
        {
            if (child is ColumnHeaderLayout)
            {
                if (_pickerViewInfo.ColumnHeaderTemplate != null)
                {
                    child.Arrange(new Rect(0, 0, 0, 0));
                }
                else
                {
                    child.Arrange(new Rect(0, 0, width, headerColumHeight));
                }
            }
            else if (child is PickerScrollView)
            {
                double childWidth = width - StrokeThickness;
                childWidth = childWidth < 0 ? 0 : childWidth;
                if (_pickerViewInfo.ColumnHeaderTemplate != null)
                {
                    child.Arrange(new Rect(0, 0, childWidth, height));
                }
                else
                {
                    double childHeight = height - headerColumHeight;
                    childHeight = childHeight < 0 ? 0 : childHeight;
                    child.Arrange(new Rect(0, headerColumHeight, childWidth, childHeight));
                }
            }
        }

        return bounds.Size;
    }

    /// <summary>
    /// Method used to measure the children based on width and height value.
    /// </summary>
    /// <param name="widthConstraint">The maximum width request of the layout.</param>
    /// <param name="heightConstraint">The maximum height request of the layout.</param>
    /// <returns>The maximum size of the layout.</returns>
    protected override Size MeasureContent(double widthConstraint, double heightConstraint)
    {
        //// The header column height.
        double headerColumHeight = _pickerViewInfo.ColumnHeaderView.Height;
        foreach (var child in Children)
        {
            if (child is ColumnHeaderLayout)
            {
                if (_pickerViewInfo.ColumnHeaderTemplate != null)
                {
                    child.Measure(0, 0);
                }
                else
                {
                    child.Measure(widthConstraint, headerColumHeight);
                }
            }
            else if (child is PickerScrollView)
            {
                double childWidth = widthConstraint - StrokeThickness;
                childWidth = childWidth < 0 ? 0 : childWidth;
                if (_pickerViewInfo.ColumnHeaderTemplate != null)
                {
                    child.Measure(childWidth, heightConstraint);
                }
                else
                {
                    double childHeight = heightConstraint - headerColumHeight;
                    childHeight = childHeight < 0 ? 0 : childHeight;
                    child.Measure(childWidth, childHeight);
                }
            }
        }

        return new Size(widthConstraint, heightConstraint);
    }

    /// <summary>
    /// Method to draw the selection background and border for picker view.
    /// </summary>
    /// <param name="canvas">The canvas.</param>
    /// <param name="dirtyRectangle">The dirty rectangle.</param>
    protected override void OnDraw(ICanvas canvas, RectF dirtyRectangle)
    {
        float width = dirtyRectangle.Width;
        float height = dirtyRectangle.Height;
        float headerColumHeight = (float)_pickerViewInfo.ColumnHeaderView.Height;
        float topPosition = _pickerViewInfo.ColumnHeaderTemplate != null ? dirtyRectangle.Top + 1 : dirtyRectangle.Top + headerColumHeight;
        float leftPosition = dirtyRectangle.Left;
        canvas.SaveState();

        //// The columns count is 1 then no need to draw the column separator line.
        if (_pickerViewInfo.ColumnDividerColor != Colors.Transparent && _column._columnIndex < _pickerViewInfo.Columns.Count - 1)
        {
            //// To draw the separator line between the column headers.
            canvas.StrokeColor = _pickerViewInfo.ColumnDividerColor;
            canvas.StrokeSize = StrokeThickness;
            float strokeThickness = (float)(StrokeThickness * 0.5);
            float columnRightPosition = width - strokeThickness;
            columnRightPosition = _pickerViewInfo.IsRTLLayout ? strokeThickness : columnRightPosition;
            canvas.DrawLine(columnRightPosition, topPosition, columnRightPosition, height);
        }

        if (_pickerViewInfo.ColumnHeaderView.DividerColor != Colors.Transparent && headerColumHeight > 0)
        {
            //// To draw the separator line at bottom of the column header.
            canvas.StrokeColor = _pickerViewInfo.ColumnHeaderView.DividerColor;
            canvas.StrokeSize = StrokeThickness;
            float columnHeaderBottomPosition = topPosition;
            canvas.DrawLine(leftPosition, columnHeaderBottomPosition, width, columnHeaderBottomPosition);
        }

        canvas.RestoreState();
    }

	#endregion

	#region Interface Implementation

	/// <summary>
	/// Method to update the selected index.
	/// </summary>
	/// <param name="tappedIndex">The tapped index.</param>
	/// <param name="isTapped">Is tap gesture used</param>
	/// <param name="isInitialLoading">Check whether is initial loading or not.</param>
	void IPickerLayout.UpdateSelectedIndexValue(int tappedIndex, bool isTapped, bool isInitialLoading)
    {
        if (tappedIndex >= _itemsSource.Count)
        {
            tappedIndex = _itemsSource.Count - 1;
        }
        else if (tappedIndex <= -1)
        {
            tappedIndex = -1;
        }

        _pickerViewInfo.UpdateSelectedIndexValue(tappedIndex, _column._columnIndex, isTapped, isInitialLoading);
    }

    #endregion
}