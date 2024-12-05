using Syncfusion.Maui.Toolkit.Calendar;

namespace Syncfusion.Maui.ControlsGallery.Calendar.Calendar
{
    /// <summary>
    /// Getting started Behavior class
    /// </summary>
    internal class DateSelectionBehavior : Behavior<SampleView>
    {
        /// <summary>
        /// Calendar view 
        /// </summary>
        SfCalendar? _calendar;

        /// <summary>
        /// The combo box that allows users to choose to whether to select date or a range.
        /// </summary>
        Picker? _comboBox;

        /// <summary>
        /// The label to display the selected date or range.
        /// </summary>
        Label? _label;

        /// <summary>
        /// The selected date which is to be displayed on the label.
        /// </summary>
        DateTime _date = DateTime.Now;

        /// <summary>
        /// The label shows the selection type based on what users choose.
        /// </summary>
        Label? _selectionLabel;

        /// <summary>
        /// The selected date range which is to be displayed on the label.
        /// </summary>
        CalendarDateRange _dateRange = new CalendarDateRange(DateTime.Now, DateTime.Now.AddDays(3));

        /// <summary>
        /// Begins when the behavior attached to the view 
        /// </summary>
        /// <param name="bindable">bindable value</param>
        protected override void OnAttachedTo(SampleView bindable)
        {
            base.OnAttachedTo(bindable);
#if IOS || MACCATALYST
            Border border = bindable.Content.FindByName<Border>("border");
            border.IsVisible = true;
#if MACCATALYST
            border.Stroke = Colors.Transparent;
#else
            border.Stroke = Colors.Transparent;
#endif
            _calendar = bindable.Content.FindByName<SfCalendar>("dateSelection1");
            _label = bindable.Content.FindByName<Label>("label1");
            _selectionLabel = bindable.Content.FindByName<Label>("selectionLabel1");
#else
            Border frame = bindable.Content.FindByName<Border>("frame");
            frame.IsVisible = true;
#if ANDROID
            frame.Stroke = Colors.Transparent;
#else
            frame.Stroke = Colors.Transparent;
#endif
            _calendar = bindable.Content.FindByName<SfCalendar>("dateSelection");
            _label = bindable.Content.FindByName<Label>("label");
            _selectionLabel = bindable.Content.FindByName<Label>("selectionLabel");
#endif
            _comboBox = bindable.Content.FindByName<Picker>("comboBox");
            _comboBox.ItemsSource = new List<string>() { "Date", "Range" };
            _comboBox.SelectedIndex = 0;
            _comboBox.SelectedIndexChanged += ComboBox_SelectionChanged;
            _calendar.SelectedDate = _date;
            _calendar.SelectedDateRange = new CalendarDateRange(_dateRange.StartDate, _dateRange.EndDate);
            _calendar.SelectionChanged += Calendar_SelectionChanged;
            _calendar.FooterView.ShowTodayButton = true;
            _calendar.FooterView.ShowActionButtons = true;
            UpdateSelectionText();
        }

        void Calendar_SelectionChanged(object? sender, CalendarSelectionChangedEventArgs e)
        {
            UpdateSelectionText();
        }

        /// <summary>
        /// Method to update the selection text.
        /// </summary>
        void UpdateSelectionText()
        {
            if (_label != null && _calendar != null && _calendar.SelectionMode == CalendarSelectionMode.Single && _calendar.SelectedDate != null)
            {
                _label.Text = _calendar.SelectedDate.Value.ToString("ddd") + ", " + _calendar.SelectedDate.Value.ToString("MMM") + " " + _calendar.SelectedDate.Value.Day.ToString();
            }
            else if (_label != null && _calendar != null && _calendar.SelectionMode == CalendarSelectionMode.Range && _calendar.SelectedDateRange != null)
            {
                string startDateText = string.Empty;
                string EndDateText = string.Empty;
                if (_calendar.SelectedDateRange.StartDate != null)
                {
                    startDateText = _calendar.SelectedDateRange.StartDate.Value.ToString("MMM") + " " + _calendar.SelectedDateRange.StartDate.Value.Day.ToString();
                }

                if (_calendar.SelectedDateRange.EndDate != null)
                {
                    EndDateText = _calendar.SelectedDateRange.EndDate.Value.ToString("MMM") + " " + _calendar.SelectedDateRange.EndDate.Value.Day.ToString();
                }

                if (EndDateText == string.Empty)
                {
                    _label.Text = startDateText;
                }
                else
                {
                    _label.Text = startDateText + " - " + EndDateText;
                }
            }
        }

        /// <summary>
        /// Method for Combo box selection type changed.
        /// </summary>
        /// <param name="sender">Return the object</param>
        /// <param name="e">Event Arguments</param>
        void ComboBox_SelectionChanged(object? sender, EventArgs e)
        {
            if (_calendar != null && sender is Picker picker && picker.SelectedItem is string selectionMode)
			{
                if (selectionMode != null)
                {
                    _calendar.SelectionMode = selectionMode == "Date" ? CalendarSelectionMode.Single : CalendarSelectionMode.Range;
                    if (_calendar.SelectionMode == CalendarSelectionMode.Single)
                    {
                        if (_selectionLabel != null)
                        {
                            _selectionLabel.Text = "Select date";
                        }

                        if (_calendar.View == CalendarView.Month)
                        {
                            DateTime visibleStartDate = new DateTime(_calendar.DisplayDate.Year, _calendar.DisplayDate.Month, 1);
                            int days = DateTime.DaysInMonth(visibleStartDate.Year, visibleStartDate.Month);
                            DateTime visibleEndDate = new DateTime(visibleStartDate.Year, visibleStartDate.Month, days);
                            if (!IsDateWithInRange(_calendar.SelectedDate, visibleStartDate.Date, visibleEndDate.Date))
                            {
                                _calendar.SelectedDate = visibleStartDate.Date;
                                _date = _calendar.SelectedDate.Value;
                            }
                        }
                    }
                    else if (_calendar.SelectionMode == CalendarSelectionMode.Range)
                    {
                        if (_selectionLabel != null)
                        {
                            _selectionLabel.Text = "Select range";
                        }

                        if (_calendar.View == CalendarView.Month)
                        {
                            DateTime visibleStartDate = new DateTime(_calendar.DisplayDate.Year, _calendar.DisplayDate.Month, 1);
                            int days = DateTime.DaysInMonth(visibleStartDate.Year, visibleStartDate.Month);
                            DateTime visibleEndDate = new DateTime(visibleStartDate.Year, visibleStartDate.Month, days);
                            if (_calendar.SelectedDateRange == null || (!IsDateWithInRange(_calendar.SelectedDateRange.StartDate, visibleStartDate, visibleEndDate) &&
                                !IsDateWithInRange(_calendar.SelectedDateRange.EndDate, visibleStartDate.Date, visibleEndDate.Date) && !IsDateWithInRange(visibleStartDate, _calendar.SelectedDateRange.StartDate, _calendar.SelectedDateRange.EndDate)))
                            {
                                _calendar.SelectedDateRange = new CalendarDateRange(visibleStartDate.Date, visibleStartDate.Date.AddDays(3));
                                _dateRange = new CalendarDateRange(_calendar.SelectedDateRange.StartDate, _calendar.SelectedDateRange.EndDate);
                            }
                        }
                    }

                    UpdateSelectionText();
                }
            }
        }

        /// <summary>
        /// Return the date with in date range.
        /// </summary>
        /// <param name="date">Date value.</param>
        /// <param name="startDate">Start date of the range.</param>
        /// <param name="endDate">End date of the range.</param>
        /// <returns>Return true, the date with in date range.</returns>
        bool IsDateWithInRange(DateTime? date, DateTime? startDate, DateTime? endDate)
        {
            if (date == null || startDate == null || endDate == null)
            {
                return false;
            }

            return date.Value.Date >= startDate.Value.Date && date.Value.Date <= endDate.Value.Date;
        }

        /// <summary>
        /// Begins when the behavior attached to the view 
        /// </summary>
        /// <param name="bindable">bindable value</param>
        protected override void OnDetachingFrom(SampleView bindable)
        {
            base.OnDetachingFrom(bindable);

            if (_calendar != null)
            {
                _calendar.SelectionChanged -= Calendar_SelectionChanged;
                _calendar = null;
            }

            if (_comboBox != null)
            {
                _comboBox.SelectedIndexChanged -= ComboBox_SelectionChanged;
                _comboBox = null;
            }

            if (_label != null)
            {
                _label = null;
            }
        }
    }
}