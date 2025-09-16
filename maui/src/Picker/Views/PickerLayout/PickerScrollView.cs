using System.Collections.ObjectModel;
using System.ComponentModel;
using Syncfusion.Maui.Toolkit.Internals;

namespace Syncfusion.Maui.Toolkit.Picker
{
    /// <summary>
    /// This represents a class that contains information about the picker scroll view.
    /// </summary>
    internal class PickerScrollView : SfPickerView
    {
        #region Fields

        /// <summary>
        /// The picker view info.
        /// </summary>
        readonly IPickerLayout _pickerLayoutInfo;

        /// <summary>
        /// The picker info.
        /// </summary>
        readonly IPicker _pickerInfo;

        /// <summary>
        /// The picker view .
        /// </summary>
        readonly PickerView _pickerView;

        /// <summary>
        /// Holds the measure height value because on windows, while change mode from default(resize the height) to dialog native change, base measure automatically change the scroll position, so it moves to other positions, so checking the content size before the scroll to fix the issue.
        /// </summary>
        double _availableHeight = 0;

#if WINDOWS || MACCATALYST
		/// <summary>
		/// The last index position.
		/// </summary>
		private double _lastIndexPosition = 0;

		/// <summary>
		/// Check whether the looping is enabled or not.
		/// </summary>
		bool _isLoopingEnabled = false;

        /// <summary>
        /// The new scroll position value.
        /// </summary>
        double _newScrollPosition = 0;
#endif

#if __ANDROID__

        /// <summary>
        /// Touch end position value on scroll end.
        /// </summary>
        double _touchEndPosition = -1;

        /// <summary>
        /// Timer used to trigger scroll end based on item height.
        /// </summary>
        IDispatcherTimer? _timer;
#endif

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="PickerScrollView"/> class.
        /// </summary>
        /// <param name="pickerLayoutInfo">The picker layout info.</param>
        /// <param name="pickerViewInfo">The picker view info.</param>
        /// <param name="itemsSource">The items source.</param>
        internal PickerScrollView(IPickerLayout pickerLayoutInfo, IPicker pickerViewInfo, ObservableCollection<string> itemsSource)
        {
            _pickerInfo = pickerViewInfo;
            _pickerLayoutInfo = pickerLayoutInfo;
            //// No need to show the scroll bar in picker control so that the scroll bar visibility set to be as never.
            VerticalScrollBarVisibility = ScrollBarVisibility.Never;
            _pickerView = new PickerView(_pickerLayoutInfo, pickerViewInfo, itemsSource);
            //// This event is triggered while the scroll view is scrolling.
            //// Using this event to update the selected and unselected text style when the scroll view is scrolling.
            Scrolled += OnViewScrolling;
            Content = _pickerView;
            PropertyChanged += OnScrollPropertyChanged;
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Method to update the selected index changed.
        /// </summary>
        internal void UpdateSelectedIndexValue()
        {
            if (!_pickerLayoutInfo.PickerInfo.IsValidParent)
            {
                Dispatcher.Dispatch(() =>
                {
#if __ANDROID__
                    RequestLayout();
#endif
                    UpdateSelectedIndex();
                });
            }
            else
            {
#if __ANDROID__
                RequestLayout();
#endif
                UpdateSelectedIndex();
            }
        }

        /// <summary>
        /// Method to update the items source.
        /// </summary>
        /// <param name="itemsSource">The items source.</param>
        internal void UpdateItemsSource(ObservableCollection<string> itemsSource)
        {
            _pickerView.UpdateItemsSource(itemsSource);
            InvalidateMeasure();
            if (!_pickerLayoutInfo.PickerInfo.IsValidParent)
            {
                Dispatcher.Dispatch(() =>
                {
#if __ANDROID__
                    RequestLayout();
#endif
                    UpdateSelectedIndex();
                });
            }
            else
            {
#if __ANDROID__
                RequestLayout();
#endif
                UpdateSelectedIndex();
            }
        }

        /// <summary>
        /// Method to update the picker scroll view.
        /// </summary>
        internal void InvalidatePickerViewDraw()
        {
            _pickerView.InvalidatePickerViewDraw();
        }

        /// <summary>
        /// Method to update the item height.
        /// </summary>
        internal void UpdateItemHeight()
        {
            _pickerView.UpdateItemHeight();
            TriggerPickerScrollViewMeasure();
            double itemHeight = _pickerLayoutInfo.PickerInfo.ItemHeight;
            int itemsCount = PickerHelper.GetItemsCount(_pickerLayoutInfo.Column.ItemsSource);
            int selectedIndex = PickerHelper.GetValidSelectedIndex(_pickerLayoutInfo.Column.SelectedIndex, itemsCount);
            double viewPortItemCount = Math.Round(_pickerView.GetViewPortHeight() / itemHeight);
            bool enableLooping = _pickerLayoutInfo.PickerInfo.EnableLooping && itemsCount > viewPortItemCount;
            double scrollPosition = selectedIndex * itemHeight;
            //// Check if looping is enabled and the current scroll end position is reaches the view height.
            if (enableLooping && scrollPosition < (viewPortItemCount * itemHeight) && scrollPosition >= 0)
            {
                //// Adjust the scroll end position by adding the total height of all items to the current scroll end position.
                scrollPosition = (itemsCount * itemHeight) + scrollPosition;
#if WINDOWS || MACCATALYST
                //// In Window and Mac, need to update the new scroll position to avoid the scroll restriction.
                if (!_isLoopingEnabled && enableLooping)
                {
                    _newScrollPosition = scrollPosition;
                }
#endif
            }

            ScrollToAsync(0, enableLooping ? scrollPosition : selectedIndex * itemHeight, false);
        }

        /// <summary>
        /// Method to update the enable looping.
        /// </summary>
        internal void UpdateEnableLooping()
        {
#if WINDOWS || MACCATALYST
            //// While looping is enabled, call the measure and arrange to increase the picker view height to get proper scroll position.
            int itemsCount = PickerHelper.GetItemsCount(_pickerLayoutInfo.Column.ItemsSource);
            double viewPortItemCount = Math.Round(_pickerView.GetViewPortHeight() / _pickerLayoutInfo.PickerInfo.ItemHeight);
            bool enableLooping = _pickerLayoutInfo.PickerInfo.EnableLooping && itemsCount > viewPortItemCount;
            if (enableLooping)
            {
                _isLoopingEnabled = false;
                _lastIndexPosition = 0;
            }
#endif
            UpdateSelectedIndexPosition();
            if (_pickerLayoutInfo.PickerInfo.ItemTemplate != null)
            {
                _pickerView.UpdateItemTemplate();
            }

            _pickerView.UpdateItemHeight();
        }

        /// <summary>
        /// Method to update the picker item template changes.
        /// </summary>
        internal void UpdateItemTemplate()
        {
            _pickerView.UpdateItemTemplate();
        }

        /// <summary>
        /// Unwire the events.
        /// </summary>
        internal void Dispose()
        {
            Scrolled -= OnViewScrolling;
            PropertyChanged -= OnScrollPropertyChanged;
            if (Content != null)
            {
                Content = null;
            }
        }

        /// <summary>
        /// Method to update the keyboard interaction.
        /// </summary>
        internal void UpdateKeyboardViewFocus()
        {
            _pickerView.UpdatePickerFocus();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Method to call scroll view is scrolling.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The scroll event args.</param>
        void OnViewScrolling(object? sender, ScrolledEventArgs e)
        {
            double itemHeight = _pickerLayoutInfo.PickerInfo.ItemHeight;
            int itemsCount = PickerHelper.GetItemsCount(_pickerLayoutInfo.Column.ItemsSource);
            double viewPortItemCount = Math.Round(_pickerView.GetViewPortHeight() / itemHeight);
            bool enableLooping = _pickerLayoutInfo.PickerInfo.EnableLooping && itemsCount > viewPortItemCount;
            if (enableLooping)
            {
                double totalContentHeight = itemsCount * itemHeight;
                //// When scrolling upward, update the scroll position based on the total content height.
                if (e.ScrollY < (viewPortItemCount * itemHeight))
                {
                    //// Need to update the scroll position to avoid the flickering issue while scrolling upward.
                    _pickerView.TriggerPickerViewMeasure();
                    ScrollToAsync(0, e.ScrollY + totalContentHeight, false);
                }
                //// When scrolling downward, update the scroll position based on the total content height.
                else if (e.ScrollY > totalContentHeight + (viewPortItemCount * itemHeight))
                {
                    //// Need to update the scroll position to avoid the flickering issue while scrolling downward.
                    _pickerView.TriggerPickerViewMeasure();
                    ScrollToAsync(0, e.ScrollY - totalContentHeight, false);
                }
            }

            //// Assume the scrollY position is 109 and the item height is 20, While using Math.Round then the selectionIndex is 109 / 20 = 5.4 => 5(SelectionIndex).
            //// Assume the scrollY position is 110 and the item height is 20, While using Math.Round then the selectionIndex is 110 / 20 = 5.5 => 6(SelectionIndex).
            int selectionIndex = (int)Math.Round(e.ScrollY / itemHeight);
#if WINDOWS || MACCATALYST
            //// While looping change dynamically, update the selected index based on new position.
            if (enableLooping && _isLoopingEnabled == false && (_newScrollPosition - e.ScrollY) > 0 && _lastIndexPosition != (itemsCount * itemHeight) - itemHeight)
            {
                selectionIndex = (int)Math.Round(_newScrollPosition / itemHeight);
                _isLoopingEnabled = true;
                _newScrollPosition = 0;
            }
#endif

            //// While looping, check the selected index value.
            if (enableLooping)
            {
                selectionIndex = selectionIndex % itemsCount;
                if (selectionIndex < 0)
                {
                    selectionIndex = 0;
                }
            }

            selectionIndex = PickerHelper.GetValidSelectedIndex(selectionIndex, itemsCount);
            if (UpdateBlackoutSelection())
            {
                return;
            }

            _pickerView.UpdateSelectedIndexValue(selectionIndex);
            if (enableLooping && _pickerLayoutInfo.PickerInfo.ItemTemplate != null)
            {
                _pickerView.UpdateItemTemplate();
            }

#if !WINDOWS
            //// Need to trigger the measure to update the height.
            if (enableLooping)
            {
                _pickerView.UpdateItemHeight();
            }
#endif
        }

#if __ANDROID__
        /// <summary>
        /// Triggered while the touch completed.
        /// </summary>
        void OnTouchCompleted()
        {
            double itemHeight = _pickerLayoutInfo.PickerInfo.ItemHeight;
            if (_timer == null)
            {
                _timer = Dispatcher.CreateTimer();
                _timer.Interval = TimeSpan.FromMilliseconds(50);
            }

            _timer.Tick += (s, e) =>
            {
                if (_touchEndPosition == ScrollY)
                {
                    double selectionIndex = Math.Round(_touchEndPosition / itemHeight);
                    //// From above example the selectionIndex is 5. So that the current item position is 5 * 20 = 100.
                    double currentItemPosition = selectionIndex * itemHeight;
                    int itemsCount = PickerHelper.GetItemsCount(_pickerLayoutInfo.Column.ItemsSource);
                    double viewPortItemCount = Math.Round(_pickerView.GetViewPortHeight() / itemHeight);
                    bool enableLooping = _pickerLayoutInfo.PickerInfo.EnableLooping && itemsCount > viewPortItemCount;
                    //// While looping, check the selected index value.
                    if (enableLooping)
                    {
                        selectionIndex = selectionIndex % itemsCount;
                        if (selectionIndex < 0)
                        {
                            selectionIndex = 0;
                        }
                    }

                    //// From above example the scrollEndPosition is 109. So that the positionDifference is 109 - 100 = 9.
                    double positionDifference = _touchEndPosition - currentItemPosition;
                    //// For windows, While using custom animation for windows, triggers scroll event. So that default scroll to async method is used.
                    //// For other than windows, While scroll view fast scrolling and we started new scrolling before end of the previous scrolling then the scroll view is not scrolled to the exact position.
                    // Method to update the position based on animation value.
                    void UpdateProperty(double value)
                    {
                        //// From above example the positionDifference is 9. So that the center position is 109 - (9 * 1) = 100. So that the scroll position is 100.
                        double centerPosition = _touchEndPosition - (positionDifference * value);
                        //// From above example the center position is 100. So that the scroll position is 100.
                        //// The scroll to async does not trigger the scroll event. Because in android we handled scroll using touch event.
                        //// In iOS and Mac we handled scroll end using drag end event.
                        ScrollToAsync(0, centerPosition, false);
                    }

                    void Finished(double value, bool isFinished)
                    {
                        _pickerLayoutInfo.UpdateSelectedIndexValue((int)selectionIndex, false);
                    }

                    if (_pickerLayoutInfo.PickerInfo.IsValidParent)
                    {
                        //// Animation duration is 100 milliseconds for smooth animation.
                        AnimationExtensions.Animate(this, "Scrolled", UpdateProperty, 0, 1, 16, 100U, Easing.Linear, Finished);
                    }
                    else
                    {
                        ScrollToAsync(0, currentItemPosition, false);
                        RequestLayout();
                        _pickerLayoutInfo.UpdateSelectedIndexValue((int)selectionIndex, false);
                    }

                    _timer.Stop();
                    _timer = null;
                }
                else
                {
                    _touchEndPosition = ScrollY;
                }
            };

            _timer.Start();
        }

        /// <summary>
        /// Method used to request layout while the scroll view is scrolled. because the
        /// scroll view does not scroll properly while the previous scroll does not end correctly.
        /// Example, if dialog is closed while the scroll view scrolling on fast fling then the
        /// scroll view does not scroll to the exact position on first time due to incomplete previous scroll.
        /// </summary>
        void RequestLayout()
        {
            if (Handler != null && Handler.PlatformView != null && Handler.PlatformView is NativeCustomScrolLayout scrolLayout)
            {
                scrolLayout.RequestLayout();
            }
        }
#endif

        /// <summary>
        /// Method to update the selected index changed while the scroll view content size is greater than item source needed size.
        /// </summary>
        void UpdateSelectedIndex()
        {
            double itemHeight = _pickerLayoutInfo.PickerInfo.ItemHeight;
            //// While scrolled the scroll position is updated in UI but not update selected index. After scroll position updated in UI then the selected index changed based on the scroll position and its property changed event will triggered.
            //// In this case no need to update the scroll position again.
            int scrolledIndex = (int)Math.Round(ScrollY / itemHeight);
            int itemsCount = PickerHelper.GetItemsCount(_pickerLayoutInfo.Column.ItemsSource);
            int selectedIndex = PickerHelper.GetValidSelectedIndex(_pickerLayoutInfo.Column.SelectedIndex, itemsCount);
            if (selectedIndex == scrolledIndex)
            {
                return;
            }

            double totalContentSize = (itemsCount * itemHeight) + DesiredSize.Height - itemHeight;
            double scrollPosition = selectedIndex * itemHeight;
            double contentHeight = Math.Floor(ContentSize.Height);
            double viewPortItemCount = Math.Round(_pickerView.GetViewPortHeight() / itemHeight);
            bool enableLooping = _pickerLayoutInfo.PickerInfo.EnableLooping && itemsCount > viewPortItemCount;
            if (contentHeight < Math.Floor(totalContentSize) && contentHeight - DesiredSize.Height <= Math.Ceiling(scrollPosition))
            {
                return;
            }

            //// Check if looping is enabled and the current scroll end position is reaches the view height.
            if (enableLooping && scrollPosition < (viewPortItemCount * itemHeight) && scrollPosition >= 0)
            {
                //// Adjust the scroll end position by adding the total height of all items to the current scroll end position.
                scrollPosition = (itemsCount * itemHeight) + scrollPosition;
#if WINDOWS || MACCATALYST
                if (!_isLoopingEnabled && enableLooping)
                {
                    _newScrollPosition = scrollPosition;
                }
#endif
            }

#if MACCATALYST
            if (!_isLoopingEnabled && enableLooping)
            {
                _isLoopingEnabled = true;
            }
#endif

            //// No need to animate the scroll position while changing the selected index value.
            ScrollToAsync(0, scrollPosition, false);
        }

        /// <summary>
        /// Method to update the selected scroll position on initial scrolling and content size changed.
        /// </summary>
        void UpdateSelectedIndexPosition()
        {
            if (UpdateBlackoutSelection())
            {
                return;
            }

            double itemHeight = _pickerLayoutInfo.PickerInfo.ItemHeight;
            int itemsCount = PickerHelper.GetItemsCount(_pickerLayoutInfo.Column.ItemsSource);
            int selectedIndex = PickerHelper.GetValidSelectedIndex(_pickerLayoutInfo.Column.SelectedIndex, itemsCount);
            //// Check if any column selected index is initially set to -1 or less than -1, and change the other column selected index as -1.
            if (_pickerLayoutInfo.Column.SelectedIndex <= -1)
            {
                for (int index = 0; index < _pickerInfo.Columns.Count; index++)
                {
                    _pickerInfo.Columns[index].SelectedIndex = -1;
                }
            }
            //// Check the selected item is null and selected index has default value and change the selected index value.
            else if (_pickerLayoutInfo.Column.SelectedIndex == 0 && _pickerLayoutInfo.Column.SelectedItem == null)
            {
                if (_pickerLayoutInfo.Column.Parent == null)
                {
                    _pickerLayoutInfo.Column.SelectedIndex = -1;
                }
            }
            //// Check the default value of selected index and update the index based on selected item.
            else if (_pickerLayoutInfo.Column.SelectedIndex == 0 && _pickerLayoutInfo.Column.SelectedItem != null)
            {
                int valueCount = PickerHelper.GetSelectedItemIndex(_pickerLayoutInfo.Column);
                selectedIndex = PickerHelper.GetValidSelectedIndex(valueCount, itemsCount);
                _pickerLayoutInfo.Column.SelectedIndex = selectedIndex;
            }

            double newScrollPosition = selectedIndex * itemHeight;
            double viewPortItemCount = Math.Round(_pickerView.GetViewPortHeight() / itemHeight);
            bool enableLooping = _pickerLayoutInfo.PickerInfo.EnableLooping && itemsCount > viewPortItemCount;
            //// Check if looping is enabled and the current scroll end position is reaches the view height.
            if (enableLooping && newScrollPosition < (viewPortItemCount * itemHeight) && newScrollPosition >= 0)
            {
                //// Adjust the scroll end position by adding the total height of all items to the current scroll end position.
                newScrollPosition = (itemsCount * itemHeight) + newScrollPosition;
#if WINDOWS || MACCATALYST
                if (!_isLoopingEnabled && enableLooping)
                {
                    _newScrollPosition = newScrollPosition;
                }
#endif
            }

            //// Checking position rather than index because switching to default mode and change screen height and again move into dialog mode then the selected item does not shown correctly center of selection highlight.
            if (newScrollPosition == ScrollY)
            {
                return;
            }

            //// No need to animate the scroll position while changing the selected index value.
            ScrollToAsync(0, newScrollPosition, false);
        }

        /// <summary>
        /// Triggered whenever the scroll view property changed. Used to scroll after the content size changed.
        /// </summary>
        /// <param name="sender">The scroll view instance.</param>
        /// <param name="e">Property changed event argument.</param>
        void OnScrollPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(ContentSize))
            {
                return;
            }

            double itemHeight = _pickerLayoutInfo.PickerInfo.ItemHeight;
            int itemsCount = PickerHelper.GetItemsCount(_pickerLayoutInfo.Column.ItemsSource);
            double totalContentSize = (itemsCount * itemHeight) + DesiredSize.Height - itemHeight;
            double availableContentSize = Math.Floor(ContentSize.Height);
            double viewPortItemCount = Math.Round(_pickerView.GetViewPortHeight() / itemHeight);
            bool enableLooping = _pickerLayoutInfo.PickerInfo.EnableLooping && itemsCount > viewPortItemCount;
            if (!enableLooping && (availableContentSize - 1 > totalContentSize || availableContentSize + 1 < totalContentSize))
            {
                return;
            }

            Dispatcher.Dispatch(() =>
            {
                UpdateSelectedIndexPosition();
            });
        }

        /// <summary>
        /// Method to Check and update selection based on blackout value.
        /// </summary>
        bool UpdateBlackoutSelection()
        {
            //// Since black out selection not applicable.
            if (_pickerLayoutInfo.PickerInfo is SfPicker)
            {
                return false;
            }

            //// Checks the current selected time is black out date or not to allow scrolling.
            if (_pickerLayoutInfo.PickerInfo is SfDatePicker datePicker)
            {
                if (datePicker.SelectedDate != null && datePicker.BlackoutDates.Any(blackOutDate => blackOutDate.Date == datePicker.SelectedDate.Value.Date))
                {
                    //// If the selected date is blackout date and is within the current month it reverts to previous selected date.
                    //// In else case, it will increment the selected date by one day until it finds a non-blackout date.
                    //// also if it reaches the end of the month it will get reset to the start of the month.
                    if (datePicker.SelectedDate.Value.Month == datePicker._previousSelectedDateTime.Date.Month && datePicker.SelectedDate.Value.Year == datePicker._previousSelectedDateTime.Date.Year)
                    {
                        datePicker.SelectedDate = datePicker._previousSelectedDateTime.Date;
                    }
                    else
                    {
                        DateTime selectedDate = datePicker.SelectedDate.Value.AddDays(datePicker.DayInterval);
                        selectedDate = selectedDate > datePicker.MaximumDate ? datePicker.MaximumDate : selectedDate;
                        bool isDayReset = false;
                        while (datePicker.BlackoutDates.Any(blackOutDate => blackOutDate.Date == selectedDate.Date))
                        {
                            //// Increase the date by day interval.
                            selectedDate = selectedDate.AddDays(datePicker.DayInterval);

                            if (selectedDate.Month != datePicker.SelectedDate.Value.Month)
                            {
                                //// To avoid the infinite loop while the blackout date is set for the whole month.
                                if (!isDayReset)
                                {
                                    break;
                                }

                                //// Reset the date to the start of the month based on minimum Date.
                                selectedDate = new DateTime(selectedDate.Year, selectedDate.Month, datePicker.MinimumDate.Month == selectedDate.Month ? datePicker.MinimumDate.Day : 1);
                                isDayReset = true;
                            }

                            //// Reset the date to the start of the month based on maximum Date.
                            if (selectedDate > datePicker.MaximumDate)
                            {
                                selectedDate = new DateTime(selectedDate.Year, selectedDate.Month, datePicker.MinimumDate.Month == selectedDate.Month ? datePicker.MinimumDate.Day : 1);
                            }
                        }

                        datePicker.SelectedDate = selectedDate;
                    }

                    return true;
                }
            }

            //// Checks the current selected time is black out time or not to allow scrolling.
            if (_pickerLayoutInfo.PickerInfo is SfTimePicker timePicker)
            {
                if (timePicker.SelectedTime != null && timePicker.BlackoutTimes.Any(blackOutTime => blackOutTime.Hours == timePicker.SelectedTime.Value.Hours && blackOutTime.Minutes == timePicker.SelectedTime.Value.Minutes))
                {
                    //// If the selected time is blackout time and is within the current hour it reverts to previous selected time.
                    //// In else case, it will increment the selected time by one minute until it finds a non-blackout time.
                    //// also if it reaches the end of the hour it will get reset to the start of the hour.
                    if (timePicker.SelectedTime.Value.Hours == timePicker._previousSelectedDateTime.Hour)
                    {
                        timePicker.SelectedTime = timePicker._previousSelectedDateTime.TimeOfDay;
                    }
                    else
                    {
                        TimeSpan selectedTime = timePicker.SelectedTime.Value.Add(TimeSpan.FromMinutes(timePicker.MinuteInterval));
                        selectedTime = selectedTime > timePicker.MaximumTime ? timePicker.MaximumTime : selectedTime;
                        bool isHourReset = false;
                        while (timePicker.BlackoutTimes.Any(blackOutTime => blackOutTime.Hours == selectedTime.Hours && blackOutTime.Minutes == selectedTime.Minutes))
                        {
                            selectedTime = selectedTime.Add(TimeSpan.FromMinutes(timePicker.MinuteInterval));
                            if (selectedTime.Hours != timePicker.SelectedTime.Value.Hours)
                            {
                                //// To avoid the infinite loop while the blackout time is set for the whole hour.
                                if (isHourReset)
                                {
                                    break;
                                }

                                //// Reset the time to the start of the hour based on minimum time.
                                selectedTime = new TimeSpan(selectedTime.Hours, timePicker.MinimumTime.Hours == selectedTime.Hours ? timePicker.MinimumTime.Hours : 0, selectedTime.Seconds);
                                isHourReset = true;
                            }

                            //// Reset the time to the start of the hour based on maximum time.
                            if (selectedTime > timePicker.MaximumTime)
                            {
                                selectedTime = new TimeSpan(selectedTime.Hours, timePicker.MinimumTime.Hours == selectedTime.Hours ? timePicker.MinimumTime.Hours : 0, selectedTime.Seconds);
                            }
                        }

                        timePicker.SelectedTime = selectedTime;
                    }

                    return true;
                }
            }

            //// Checks the current selected time is black out datetime or not to allow scrolling.
            if (_pickerLayoutInfo.PickerInfo is SfDateTimePicker dateTimePicker)
            {
                bool isTimeSpanAtZero = false;
                if (dateTimePicker.SelectedDate != null && dateTimePicker.BlackoutDateTimes.Any(blackOutDateTime => DatePickerHelper.IsBlackoutDateTime(blackOutDateTime, dateTimePicker.SelectedDate, out isTimeSpanAtZero)))
                {
                    //// For calculating the blackout date time, if the whole date is black out value the if case will get executed.
                    //// If only the time is black out value the else case will get executed
                    if (isTimeSpanAtZero)
                    {
                        //// If the selected date time is blackout value and is within the current month it reverts to previous selected date time.
                        //// In else case, it will increment the selected date time by one day until it finds a non-blackout date time.
                        //// also if it reaches the end of the month it will get reset to the start of the month.
                        if (dateTimePicker.SelectedDate.Value.Month == dateTimePicker._previousSelectedDateTime.Month && dateTimePicker.SelectedDate.Value.Year == dateTimePicker._previousSelectedDateTime.Year)
                        {
                            dateTimePicker.SelectedDate = dateTimePicker._previousSelectedDateTime;
                        }
                        else
                        {
                            DateTime selectedDate = dateTimePicker.SelectedDate.Value.AddDays(dateTimePicker.DayInterval);
                            selectedDate = selectedDate > dateTimePicker.MaximumDate ? dateTimePicker.MaximumDate : selectedDate;
                            bool isDayReset = false;
                            while (dateTimePicker.BlackoutDateTimes.Any(blackOutDateTime => DatePickerHelper.IsBlackoutDateTime(blackOutDateTime, selectedDate, out bool isTimeSpanZero)))
                            {
                                selectedDate = selectedDate.AddDays(dateTimePicker.DayInterval);
                                if (selectedDate.Month != dateTimePicker.SelectedDate.Value.Month)
                                {
                                    //// To avoid the infinite loop while the blackout date is set for the whole month.
                                    if (isDayReset)
                                    {
                                        break;
                                    }

                                    selectedDate = new DateTime(selectedDate.Year, selectedDate.Month, dateTimePicker.MinimumDate.Month == dateTimePicker.MinimumDate.Month ? dateTimePicker.MinimumDate.Day : 1);
                                    isDayReset = true;
                                }

                                //// Reset the date to the start of the month based on maximum date.
                                if (selectedDate > dateTimePicker.MaximumDate)
                                {
                                    selectedDate = new DateTime(selectedDate.Year, selectedDate.Month, dateTimePicker.MinimumDate.Month == dateTimePicker.MinimumDate.Month ? dateTimePicker.MinimumDate.Day : 1);
                                }
                            }

                            dateTimePicker.SelectedDate = selectedDate;
                        }
                    }
                    else
                    {
                        //// If the selected date time is blackout value and is within the current hour it reverts to previous selected time.
                        //// In else case, it will increment the selected date time by one minute until it finds a non-blackout date time.
                        //// also if it reaches the end of the hour it will get reset to the start of the hour.
                        if (dateTimePicker.SelectedDate.Value.Hour == dateTimePicker._previousSelectedDateTime.Hour)
                        {
                            dateTimePicker.SelectedDate = dateTimePicker._previousSelectedDateTime;
                        }
                        else
                        {
                            DateTime selectedTime = dateTimePicker.SelectedDate.Value.AddMinutes(dateTimePicker.MinuteInterval);
                            selectedTime = selectedTime > dateTimePicker.MaximumDate ? dateTimePicker.MaximumDate : selectedTime;
                            bool isHourReset = false;
                            while (dateTimePicker.BlackoutDateTimes.Any(blackOutDateTime => DatePickerHelper.IsBlackoutDateTime(blackOutDateTime, selectedTime, out bool isTimeSpanZero)))
                            {
                                selectedTime = selectedTime.AddMinutes(dateTimePicker.MinuteInterval);
                                if (selectedTime.Hour != dateTimePicker.SelectedDate.Value.Hour)
                                {
                                    //// To avoid the infinite loop while the blackout time is set for the whole hour.
                                    if (isHourReset)
                                    {
                                        break;
                                    }

                                    selectedTime = new DateTime(selectedTime.Year, selectedTime.Month, selectedTime.Day, selectedTime.Hour, dateTimePicker.MinimumDate.Hour == selectedTime.Hour ? dateTimePicker.MinimumDate.Hour : 0, selectedTime.Second);
                                    isHourReset = true;
                                }

                                //// Reset the time to the start of the hour based on maximum time.
                                if (selectedTime > dateTimePicker.MaximumDate)
                                {
                                    selectedTime = new DateTime(selectedTime.Year, selectedTime.Month, selectedTime.Day, selectedTime.Hour, dateTimePicker.MinimumDate.Hour == selectedTime.Hour ? dateTimePicker.MinimumDate.Hour : 0, selectedTime.Second);
                                }
                            }

                            dateTimePicker.SelectedDate = selectedTime;
                        }
                    }

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Method to update the picker scroll view measure.
        /// </summary>
        void TriggerPickerScrollViewMeasure()
        {
            InvalidateMeasure();
        }

        #endregion

        #region Internal Override Methods

        /// <summary>
        /// Method to check the scroll view scroll started through interaction.
        /// </summary>
        internal override void OnPickerViewScrollStart()
        {
#if __ANDROID__
            if (_timer != null)
            {
                _timer.Stop();
                _timer = null;
            }
#endif
        }

        /// <summary>
        /// Method to update the scroll view scroll position when the scroll ends.
        /// </summary>
        /// <param name="scrollEndPosition">The scroll end position.</param>
        internal override void OnPickerViewScrollEnd(double scrollEndPosition)
        {
            double itemHeight = _pickerLayoutInfo.PickerInfo.ItemHeight;
            int itemsCount = PickerHelper.GetItemsCount(_pickerLayoutInfo.Column.ItemsSource);
            double viewPortItemCount = Math.Round(_pickerView.GetViewPortHeight() / itemHeight);
            bool enableLooping = _pickerLayoutInfo.PickerInfo.EnableLooping && itemsCount > viewPortItemCount;
#if WINDOWS || MACCATALYST
            scrollEndPosition = Math.Ceiling(scrollEndPosition);
            //// Check if looping is enabled, this is the first loop, and the new scroll position is greater than the scroll end position.
            if (enableLooping && _isLoopingEnabled == false && (_newScrollPosition - scrollEndPosition) > 0 && _lastIndexPosition != (itemsCount * itemHeight) - itemHeight)
            {
                //// Set the scroll end position to the new scroll position to continue the looping behavior.
                scrollEndPosition = _newScrollPosition;
            }
#endif
			//// If the selected index is less than 0, then set the scroll end position to -40 to avoid selected index rendering in center position.
			if (!enableLooping && _pickerLayoutInfo.Column.SelectedIndex <= -1)
            {
                scrollEndPosition = -40;
            }

#if __ANDROID__
            _touchEndPosition = scrollEndPosition;
            OnTouchCompleted();
            return;
#else
            double totalContentSize = (itemsCount * itemHeight) + _availableHeight - itemHeight;
            //// On windows, while change mode from default(resize the height) to dialog native change, base measure automatically change the scroll position, so it moves to other positions, so checking the content size before the scroll to fix the issue.
            double availableContentSize = Math.Floor(ContentSize.Height);
            if (!enableLooping && (availableContentSize - 1 > totalContentSize || availableContentSize + 1 < totalContentSize))
            {
                return;
            }

            if (UpdateBlackoutSelection())
            {
                return;
            }

            //// Assume the scrollEndPosition is 109 and the item height is 20, While using Math.Round then the selectionIndex is 109 / 20 = 5.4 => 5(SelectionIndex).
            //// Assume the scrollEndPosition is 110 and the item height is 20, While using Math.Round then the selectionIndex is 110 / 20 = 5.5 => 6(SelectionIndex).
            double selectionIndex = Math.Round(scrollEndPosition / itemHeight);
            //// From above example the selectionIndex is 5. So that the current item position is 5 * 20 = 100.
            double currentItemPosition = selectionIndex * itemHeight;
            //// While looping is enabled, ensure the selection index is within valid range.
            if (enableLooping)
            {
                selectionIndex = selectionIndex % itemsCount;
                if (selectionIndex < 0)
                {
                    selectionIndex += itemsCount;
                }
            }

#if WINDOWS || MACCATALYST
			if (selectionIndex == itemsCount - 1)
			{
				_lastIndexPosition = scrollEndPosition;
			}
#endif

			//// From above example the scrollEndPosition is 109. So that the positionDifference is 109 - 100 = 9.
			double positionDifference = scrollEndPosition - currentItemPosition;
            //// While scroll view fast scrolling and we started new scrolling before end of the previous scrolling then the scroll view is not scrolled to the exact position.
            //// Method to update the position based on animation value.
            void UpdateProperty(double value)
            {
                //// From above example the positionDifference is 9. So that the center position is 109 - (9 * 1) = 100. So that the scroll position is 100.
                double centerPosition = Math.Round(scrollEndPosition - (positionDifference * value));
                //// From above example the center position is 100. So that the scroll position is 100.
                //// The scroll to async does not trigger the scroll event. Because in android we handled scroll using touch event.
                //// In iOS and Mac we handled scroll end using drag end event.
                ScrollToAsync(0, centerPosition, false);
            }

            void Finished(double value, bool isFinished)
            {
                _pickerLayoutInfo.UpdateSelectedIndexValue((int)selectionIndex, false);
                _pickerView.UpdateSelectedIndexOnAnimationEnd((int)selectionIndex);
            }

            //// Animation duration is 100 milliseconds for smooth animation.
            AnimationExtensions.Animate(this, "Scrolled", UpdateProperty, 0, 1, 16, 100U, Easing.Linear, Finished);
#endif
        }

        #endregion

        #region Override Methods

        /// <summary>
        /// Method to measures child elements size in a container, scroll view is measured with given width and height constraints.
        /// </summary>
        /// <param name="widthConstraint">The maximum width request of the layout.</param>
        /// <param name="heightConstraint">The maximum height request of the layout.</param>
        /// <returns>Returns size of the scroll view.</returns>
        protected override Size MeasureOverride(double widthConstraint, double heightConstraint)
        {
            //// FB65959 - UpdateSelectedIndexPosition resets selection even when the size does not change.
#if MACCATALYST || IOS
            if (DesiredSize.Width == widthConstraint && DesiredSize.Height == heightConstraint)
            {
                //// We need to update blackout selection for mac and ios to update when both height and width or same.
                UpdateBlackoutSelection();
                return new Size(widthConstraint, heightConstraint);
            }
#endif

            bool isContentViewPortChanged = _pickerView.UpdatePickerViewPortHeight(heightConstraint);
            _availableHeight = heightConstraint;
            if (isContentViewPortChanged)
            {
                //// If the content view port size changed then the content measure will not triggered. So that the content measure is triggered manually.
                Content.Handler?.GetDesiredSize(widthConstraint, heightConstraint);
            }

            base.MeasureOverride(widthConstraint, heightConstraint);
            DesiredSize = new Size(widthConstraint, heightConstraint);

            double itemHeight = _pickerLayoutInfo.PickerInfo.ItemHeight;
            int itemsCount = PickerHelper.GetItemsCount(_pickerLayoutInfo.Column.ItemsSource);
            double totalContentSize = (itemsCount * itemHeight) + DesiredSize.Height - itemHeight;
            double availableContentSize = Math.Floor(ContentSize.Height);
            //// Need to scroll selected index value while content size is same, content size will be  assigned while handling visibility.
            //// Eg., if user shown the picker and dynamically change the visibility and update the selected index then the scroll view does not update the UI,
            //// so update the scroll position while the UI visible. Measure will trigger while the UI visibility changed.
            if (availableContentSize - 1 <= totalContentSize && availableContentSize + 1 >= totalContentSize)
            {
                Dispatcher.Dispatch(() =>
                {
                    UpdateSelectedIndexPosition();
                });
            }

            return new Size(widthConstraint, heightConstraint);
        }

        #endregion
    }
}