using System.Collections.ObjectModel;
using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.Picker;

/// <summary>
/// Holds the picker items drawing based on virtualization.
/// </summary>
internal class PickerDrawableView : SfDrawableView
{
    #region Fields

    /// <summary>
    /// The selected index holds the current selected index based on the current scroll position.
    /// It is used to restricted the unwanted drawing of the item source element while scrolling.
    /// </summary>
    int _selectedIndex = 0;

    /// <summary>
    /// The items source.
    /// </summary>
    ObservableCollection<string> _itemsSource;

    /// <summary>
    /// The items source collection based on size.
    /// </summary>
    readonly ObservableCollection<string> _sizeBasedItemsSource;

    /// <summary>
    /// The drawn width used to calculate the items source based on drawn width value.
    /// </summary>
    double _drawnWidth = 0;

    /// <summary>
    /// The picker view info.
    /// </summary>
    readonly IPickerLayout _pickerLayoutInfo;

    /// <summary>
    /// Method to get the current scroll view starting index.
    /// </summary>
    readonly Func<int> _getStartingIndex;

    /// <summary>
    /// The picker view info.
    /// </summary>
    readonly PickerView _pickerView;

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="PickerDrawableView"/> class.
    /// </summary>
    /// <param name="pickerLayoutInfo">The picker layout info.</param>
    /// <param name="pickerView">The picker view info.</param>
    /// <param name="itemsSource">The items source.</param>
    /// <param name="getStartingIndex">Method to get the current scroll view starting index.</param>
    internal PickerDrawableView(IPickerLayout pickerLayoutInfo, PickerView pickerView, ObservableCollection<string> itemsSource, Func<int> getStartingIndex)
    {
        _pickerView = pickerView;
        _pickerLayoutInfo = pickerLayoutInfo;
        _itemsSource = itemsSource;
        _getStartingIndex = getStartingIndex;
        _sizeBasedItemsSource = new ObservableCollection<string>();
    }

    #endregion

    #region Internal Methods

    /// <summary>
    /// Method to update the selected index.
    /// </summary>
    /// <param name="index">The index.</param>
    internal void UpdateSelectedIndexValue(int index)
    {
        if (index == _selectedIndex)
        {
            return;
        }

        _selectedIndex = index;
        InvalidateDrawable();
    }

    /// <summary>
    /// Method to update the selected index on animation end.
    /// </summary>
    /// <param name="index">The index.</param>
    internal void UpdateSelectedIndexOnAnimationEnd(int index)
    {
        _selectedIndex = index;
        InvalidateDrawable();
    }

    /// <summary>
    /// Method to update the items source.
    /// </summary>
    /// <param name="itemsSource">The items source.</param>
    internal void UpdateItemsSource(ObservableCollection<string> itemsSource)
    {
        _itemsSource = itemsSource;
        _sizeBasedItemsSource.Clear();
        InvalidateDrawable();
    }

    /// <summary>
    /// Method to update the picker view draw.
    /// </summary>
    internal void UpdatePickerViewDraw()
    {
        InvalidateDrawable();
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Used for checking whether value is disabled or not.
    /// </summary>
    /// <param name="currentValue">Current value of picker column.</param>
    /// <param name="isSelected">Current value is selected or not.</param>
    /// <returns>Returns true or false based on disabled values.</returns>
    bool UpdateBlackoutStyle(string currentValue, bool isSelected)
    {
        //// Since black out selection not applicable.
        if (_pickerLayoutInfo.PickerInfo is SfPicker)
        {
            return false;
        }

        if (_pickerLayoutInfo.PickerInfo is SfTimePicker timePicker)
        {
            //// Used to check the given time is black out time or not.
            if (_pickerLayoutInfo.Column.HeaderText == SfPickerResources.GetLocalizedString(timePicker.ColumnHeaderView.MinuteHeaderText))
            {
                TimeSpan selectedTime = timePicker.SelectedTime ?? timePicker._previousSelectedDateTime.TimeOfDay;
                return timePicker.BlackoutTimes.Any(blackOutTime => blackOutTime.Hours == selectedTime.Hours && blackOutTime.Minutes == int.Parse(currentValue));
            }
        }

        if (_pickerLayoutInfo.PickerInfo is SfDatePicker datePicker)
        {
            //// Used to check the given date is black out date or not.
            if (_pickerLayoutInfo.Column.HeaderText == SfPickerResources.GetLocalizedString(datePicker.ColumnHeaderView.DayHeaderText))
            {
                DateTime selectedDate = datePicker.SelectedDate ?? datePicker._previousSelectedDateTime;
                return datePicker.BlackoutDates.Any(blackOutDate => DatePickerHelper.IsBlackoutDate(false, currentValue, blackOutDate, selectedDate));
            }
        }

        if (_pickerLayoutInfo.PickerInfo is SfDateTimePicker dateTimePicker)
        {
            DateTime selectedDateTime = dateTimePicker.SelectedDate ?? dateTimePicker._previousSelectedDateTime;

            //// Used to check the given date time is black out date time or not.
            if (_pickerLayoutInfo.Column.HeaderText == SfPickerResources.GetLocalizedString(dateTimePicker.ColumnHeaderView.DayHeaderText))
            {
                return dateTimePicker.BlackoutDateTimes.Any(blackOutDateTime => DatePickerHelper.IsBlackoutDate(false, currentValue, blackOutDateTime, selectedDateTime) && blackOutDateTime.TimeOfDay == TimeSpan.Zero);
            }

            if (_pickerLayoutInfo.Column.HeaderText == SfPickerResources.GetLocalizedString(dateTimePicker.ColumnHeaderView.MinuteHeaderText))
            {
                return dateTimePicker.BlackoutDateTimes.Any(blackOutDateTime => DatePickerHelper.IsBlackoutDate(true, currentValue, blackOutDateTime, selectedDateTime) && blackOutDateTime.Hour == selectedDateTime.Hour && blackOutDateTime.Minute == int.Parse(currentValue));
            }
        }

        return false;
    }

    #endregion

    #region Override Methods

    /// <summary>
    /// Method to draw the item source element inside the scroll view.
    /// </summary>
    /// <param name="canvas">The canvas.</param>
    /// <param name="dirtyRect">The dirty rectangle.</param>
    protected override void OnDraw(ICanvas canvas, RectF dirtyRect)
    {
        if (dirtyRect.Width == 0 || dirtyRect.Height == 0)
        {
            return;
        }

        int padding = 5;
        if (_drawnWidth != dirtyRect.Width || _sizeBasedItemsSource.Count == 0)
        {
            _drawnWidth = dirtyRect.Width;
            _sizeBasedItemsSource.Clear();
            double maxTextWidth = _drawnWidth - padding;
            foreach (string value in _itemsSource)
            {
                //// Initially checks that the given value is need to be trimmed by using font size and text length.
                string unSelectedText = value.Length * (_pickerLayoutInfo.PickerInfo.TextStyle.FontSize * 0.6) > maxTextWidth ? PickerHelper.TrimText(value, maxTextWidth, _pickerLayoutInfo.PickerInfo.TextStyle) : value;
                string selectedText = value.Length * (_pickerLayoutInfo.PickerInfo.SelectedTextStyle.FontSize * 0.6) > maxTextWidth ? PickerHelper.TrimText(value, maxTextWidth, _pickerLayoutInfo.PickerInfo.SelectedTextStyle) : value;

                string textToAdd = unSelectedText.Length < selectedText.Length ? unSelectedText : selectedText;
                _sizeBasedItemsSource.Add(textToAdd);
            }
        }

        canvas.SaveState();
        //// The item height.
        double itemHeight = _pickerLayoutInfo.PickerInfo.ItemHeight;
        double yPosition = 0;
        int startIndex = _getStartingIndex();

        float minimumThresholdFontSize = 8;
        var unselectedTextStyle = _pickerLayoutInfo.PickerInfo.TextStyle;
        var selectedTextStyle = _pickerLayoutInfo.PickerInfo.SelectedTextStyle;
        int maxDistance = (int)(_pickerView.GetPickerItemViewPortCount() / 2) + 1;

        //// Draw item source item based on the item source collection. And change the text style based on selected item and selected index.
        for (int i = startIndex; i < _sizeBasedItemsSource.Count; i++)
        {
            Rect rectangle = new Rect(0, yPosition, dirtyRect.Width, itemHeight);
            PickerTextStyle textStyle;
            PickerTextStyle blackoutStyle = _pickerLayoutInfo.PickerInfo.DisabledTextStyle;
            blackoutStyle.FontSize = unselectedTextStyle.FontSize;

            if (i == _selectedIndex)
            {
                PickerTextStyle defaultSelectedStyle = new PickerTextStyle { FontSize = selectedTextStyle.FontSize, TextColor = selectedTextStyle.TextColor, FontFamily = selectedTextStyle.FontFamily, FontAttributes = selectedTextStyle.FontAttributes, FontAutoScalingEnabled = selectedTextStyle.FontAutoScalingEnabled };
                if (_pickerLayoutInfo.Column.SelectedItem == null || _pickerLayoutInfo.Column.SelectedIndex <= -1)
                {
                    defaultSelectedStyle = _pickerLayoutInfo.PickerInfo.TextStyle;
                }

                canvas.DrawText(_sizeBasedItemsSource[i], rectangle, HorizontalAlignment.Center, VerticalAlignment.Center, defaultSelectedStyle);
            }
            else
            {
                int distance = Math.Abs(i - _selectedIndex);

                if (distance <= maxDistance)
                {
                    switch (_pickerLayoutInfo.PickerInfo.TextDisplayMode)
                    {
                        case PickerTextDisplayMode.Default:
                            textStyle = _pickerLayoutInfo.PickerInfo.TextStyle;
                            canvas.DrawText(_sizeBasedItemsSource[i], rectangle, HorizontalAlignment.Center, VerticalAlignment.Center, UpdateBlackoutStyle(_sizeBasedItemsSource[i], false) ? blackoutStyle : textStyle);
                            break;

                        case PickerTextDisplayMode.Fade:
                            {
                                PickerTextStyle fadeStyle = new PickerTextStyle() { TextColor = unselectedTextStyle.TextColor, FontSize = unselectedTextStyle.FontSize, FontAttributes = unselectedTextStyle.FontAttributes, FontFamily = unselectedTextStyle.FontFamily, FontAutoScalingEnabled = unselectedTextStyle.FontAutoScalingEnabled };
                                float opacity = 1.0f - (distance / (float)maxDistance);
                                fadeStyle.TextColor = fadeStyle.TextColor.WithAlpha(opacity);
                                canvas.DrawText(_sizeBasedItemsSource[i], rectangle, HorizontalAlignment.Center, VerticalAlignment.Center, UpdateBlackoutStyle(_sizeBasedItemsSource[i], false) ? blackoutStyle : fadeStyle);
                            }

                            break;

                        case PickerTextDisplayMode.Shrink:
                            {
                                PickerTextStyle shrinkStyle = new PickerTextStyle() { TextColor = unselectedTextStyle.TextColor, FontSize = unselectedTextStyle.FontSize, FontAttributes = unselectedTextStyle.FontAttributes, FontFamily = unselectedTextStyle.FontFamily, FontAutoScalingEnabled = unselectedTextStyle.FontAutoScalingEnabled };
                                float size = (float)(shrinkStyle.FontSize - (distance / (float)maxDistance * (shrinkStyle.FontSize - minimumThresholdFontSize)));
                                shrinkStyle.FontSize = Math.Max(minimumThresholdFontSize, size);
                                canvas.DrawText(_sizeBasedItemsSource[i], rectangle, HorizontalAlignment.Center, VerticalAlignment.Center, UpdateBlackoutStyle(_sizeBasedItemsSource[i], false) ? blackoutStyle : shrinkStyle);
                            }

                            break;

                        case PickerTextDisplayMode.FadeAndShrink:
                            {
                                PickerTextStyle fadeandShrinkStyle = new PickerTextStyle() { TextColor = unselectedTextStyle.TextColor, FontSize = unselectedTextStyle.FontSize, FontAttributes = unselectedTextStyle.FontAttributes, FontFamily = unselectedTextStyle.FontFamily, FontAutoScalingEnabled = unselectedTextStyle.FontAutoScalingEnabled };
                                float opacity = 1.0f - (distance / (float)maxDistance);
                                float size = (float)(fadeandShrinkStyle.FontSize - (distance / (float)maxDistance * (fadeandShrinkStyle.FontSize - minimumThresholdFontSize)));
                                fadeandShrinkStyle.FontSize = Math.Max(minimumThresholdFontSize, size);
                                fadeandShrinkStyle.TextColor = fadeandShrinkStyle.TextColor.WithAlpha(opacity);
                                canvas.DrawText(_sizeBasedItemsSource[i], rectangle, HorizontalAlignment.Center, VerticalAlignment.Center, UpdateBlackoutStyle(_sizeBasedItemsSource[i], false) ? blackoutStyle : fadeandShrinkStyle);
                            }

                            break;
                    }
                }
            }

            yPosition += itemHeight;
            if (yPosition >= dirtyRect.Height)
            {
                break;
            }
        }

        canvas.RestoreState();
    }

    #endregion
}