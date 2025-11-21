using System.ComponentModel;
using System.Collections.ObjectModel;
using Syncfusion.Maui.Toolkit.Calendar;

namespace Syncfusion.Maui.ControlsGallery.Calendar.Calendar
{
	/// <summary>
	/// Getting started Behavior class
	/// </summary>
	internal class GettingStartedBehavior : Behavior<SampleView>
	{
		/// <summary>
		/// Calendar view
		/// </summary>
		SfCalendar? _calendar;

		/// <summary>
		/// The current time indicator switch.
		/// </summary>
		Switch? _enableDatesSwitch;

		/// <summary>
		/// The enable swipe selection switch
		/// </summary>
		Switch? _enableSwipeSelectionSwitch;

		/// <summary>
		/// The weekNumber switch.
		/// </summary>
		Switch? _weekNumberSwitch;

		/// <summary>
		/// The allow view navigation switch
		/// </summary>
		Switch? _allowViewNavigationSwitch;

		/// <summary>
		/// The week number grid.
		/// </summary>
		Grid? _weekNumberGrid;

		/// <summary>
		/// The trailing dates switch.
		/// </summary>
		Switch? _trailingDatesSwitch;

		/// <summary>
		/// The combo box is a text box component that allows users to type a value or choose
		/// an option from the list of predefined options.
		/// </summary>
		Microsoft.Maui.Controls.Picker? _comboBox;

		/// <summary>
		/// This combo box is used to choose the selection mode of the calendar
		/// </summary>
		Microsoft.Maui.Controls.Picker? _selectionComboBox;

		/// <summary>
		/// This combo box is used to choose the selection shape of the calendar
		/// </summary>
		Microsoft.Maui.Controls.Picker? _selectionShapeComboBox;

		/// <summary>
		/// This combo box is used to choose the selection direction of the calendar
		/// </summary>
		Microsoft.Maui.Controls.Picker? _directionComboBox;

		/// <summary>
		/// Grid for SelectionDirection, TrailingDates and EnableSwipeSelection
		/// </summary>
		Grid? _selectionDirectionGrid, _trailingDatesGrid, _enableSwipeSelectionGrid;

		/// <summary>
		/// The datePicker used to choose the display date
		/// </summary>
		DatePicker? _datePicker;

		/// <summary>
		/// The show action buttons switch.
		/// </summary>
		Switch? _enableActionButtonsSwitch;

		/// <summary>
		/// The show today buttons switch.
		/// </summary>
		Switch? _enableTodayButtonSwitch;

		/// <summary>
		/// Navigate to adjacent month switch.
		/// </summary>
		Switch? _navigationToAdjacentMonth;
		/// <summary>
		/// The corner radius slider.
		/// </summary>
		Slider? _cornerRadiusSlider;

		/// <summary>
		/// Begins when the behavior attached to the view 
		/// </summary>
		/// <param name="bindable">bindable value</param>
		protected override void OnAttachedTo(SampleView bindable)
		{
			base.OnAttachedTo(bindable);
			Border border = bindable.Content.FindByName<Border>("border");
			border.IsVisible = true;
			border.Stroke = Colors.Transparent;
			_calendar = bindable.Content.FindByName<SfCalendar>("Calendar");
			_datePicker = bindable.Content.FindByName<DatePicker>("datePicker");
			_weekNumberSwitch = bindable.Content.FindByName<Switch>("weekNumberSwitch");
			_enableDatesSwitch = bindable.Content.FindByName<Switch>("enableDatesSwitch");
			_allowViewNavigationSwitch = bindable.Content.FindByName<Switch>("allowViewNavigationSwitch");
			_weekNumberGrid = bindable.Content.FindByName<Grid>("weekNumberGrid");

			_enableSwipeSelectionSwitch = bindable.Content.FindByName<Switch>("enableSwipeSelectionSwitch");
			_enableSwipeSelectionGrid = bindable.Content.FindByName<Grid>("enableSwipeSelectionGrid");

			_trailingDatesGrid = bindable.Content.FindByName<Grid>("trailingDatesGrid");
			_trailingDatesSwitch = bindable.Content.FindByName<Switch>("trailingDatesSwitch");

			_cornerRadiusSlider = bindable.Content.FindByName<Slider>("cornerRadiusSlider");

			_comboBox = bindable.Content.FindByName<Microsoft.Maui.Controls.Picker>("comboBox");
			_comboBox.ItemsSource = Enum.GetValues(typeof(CalendarView)).Cast<CalendarView>().ToList();
			_comboBox.SelectedIndex = 0;
			_comboBox.SelectedIndexChanged += ComboBox_SelectionChanged;

			_selectionComboBox = bindable.Content.FindByName<Microsoft.Maui.Controls.Picker>("selectionComboBox");
			_selectionComboBox.ItemsSource = Enum.GetValues(typeof(CalendarSelectionMode)).Cast<CalendarSelectionMode>().ToList();
			_selectionComboBox.SelectedIndex = 0;
			_selectionComboBox.SelectedIndexChanged += ComboBox_SelectionTypeChanged;

			_selectionShapeComboBox = bindable.Content.FindByName<Microsoft.Maui.Controls.Picker>("selectionShapeComboBox");
			_selectionShapeComboBox.ItemsSource = Enum.GetValues(typeof(CalendarSelectionShape)).Cast<CalendarSelectionShape>().ToList();
			_selectionShapeComboBox.SelectedIndex = 0;
			_selectionShapeComboBox.SelectedIndexChanged += ComboBox_SelectionShapeChanged;

			_selectionDirectionGrid = bindable.Content.FindByName<Grid>("selectionDirectionGrid");
			_directionComboBox = bindable.Content.FindByName<Microsoft.Maui.Controls.Picker>("directionComboBox");
			_directionComboBox.ItemsSource = Enum.GetValues(typeof(CalendarRangeSelectionDirection)).Cast<CalendarRangeSelectionDirection>().ToList();
			_directionComboBox.SelectedIndex = 0;
			_directionComboBox.SelectedIndexChanged += DirectionComboBox_SelectionChanged;

			_enableActionButtonsSwitch = bindable.Content.FindByName<Switch>("showActionButtonsSwitch");
			_enableTodayButtonSwitch = bindable.Content.FindByName<Switch>("showTodayButtonSwitch");
			_navigationToAdjacentMonth = bindable.Content.FindByName<Switch>("navigationToAdjacentMonthSwitch");
			_calendar.FooterView = new CalendarFooterView()
			{
				ShowActionButtons = true,
				ShowTodayButton = true,
			};

			_calendar.MinimumDate = new DateTime(1900, 01, 01);
			_calendar.MaximumDate = new DateTime(2100, 12, 31);
			_calendar.SelectedDate = DateTime.Now.AddDays(-3);
			ObservableCollection<DateTime> selectedDates = new();
			Random random = new();
			for (int i = 0; i < 6; i++)
			{
				selectedDates.Add(new DateTime(DateTime.Now.Year, DateTime.Now.Month, random.Next(1, 28)));
			}

			_calendar.SelectedDates = selectedDates;
			_calendar.SelectedDateRange = new CalendarDateRange(DateTime.Now.AddDays(2), DateTime.Now.AddDays(6));
			_calendar.SelectedDateRanges = new ObservableCollection<CalendarDateRange> { new CalendarDateRange(DateTime.Now.AddDays(2), DateTime.Now.AddDays(6)), new CalendarDateRange(DateTime.Now.AddDays(8), DateTime.Now.AddDays(12)) };

			if (_enableDatesSwitch != null)
			{
				_enableDatesSwitch.Toggled += EnablePastDates_Toggled;
			}

			if (_enableSwipeSelectionSwitch != null)
			{
				_enableSwipeSelectionSwitch.Toggled += EnableSwipeSelection_Toggled;
			}

			if (_weekNumberSwitch != null)
			{
				_weekNumberSwitch.Toggled += WeekNumberSwitch_Toggled;
			}

			if (_trailingDatesSwitch != null)
			{
				_trailingDatesSwitch.Toggled += TrailingDatesSwitch_Toggled;
			}

			if (_allowViewNavigationSwitch != null)
			{
				_allowViewNavigationSwitch.Toggled += AllowViewNavigationSwitch_Toggled;
			}

			if (_enableActionButtonsSwitch != null)
			{
				_enableActionButtonsSwitch.Toggled += EnableActionButtonsSwitch_Toggled;
			}

			if (_enableTodayButtonSwitch != null)
			{
				_enableTodayButtonSwitch.Toggled += EnableTodayButtonSwitch_Toggled;
			}

			if (_navigationToAdjacentMonth != null)
			{
				_navigationToAdjacentMonth.Toggled += NavigationToAdjacentMonth_StateChanged;
			}

			if (_cornerRadiusSlider != null)
			{
				_cornerRadiusSlider.ValueChanged += CornerRadiusSlider_ValueChanged;
			}

			if (_calendar != null && _datePicker != null)
			{
				_calendar.ViewChanged += Calendar_ViewChanged;
				if (_calendar.SelectedDate != null)
					_datePicker.Date = (DateTime)_calendar.SelectedDate;
				_datePicker.DateSelected += DatePicker_DateSelected;
			}
		}

		/// <summary>
		/// Method for Display date selected in DatePicker is changed.
		/// </summary>
		/// <param name="sender">Return the object</param>
		/// <param name="e">Event Arguments</param>
		void DatePicker_DateSelected(object? sender, DateChangedEventArgs e)
		{
			if (_calendar == null || _datePicker == null)
			{
				return;
			}

			if (_calendar != null && e.NewDate != null)
			{
				_calendar!.DisplayDate = e.NewDate.Value;
			}

			_datePicker.Date = e.NewDate;
		}

		/// <summary>
		/// Method for Combo box selection direction changed.
		/// </summary>
		/// <param name="sender">Return the object</param>
		/// <param name="e">Event Arguments</param>
		void DirectionComboBox_SelectionChanged(object? sender, EventArgs e)
		{
			if (_calendar != null && sender is Microsoft.Maui.Controls.Picker picker && picker.SelectedItem is CalendarRangeSelectionDirection selectionDirection)
			{
				_calendar.RangeSelectionDirection = selectionDirection;
			}
		}

		/// <summary>
		/// Method for Calendar view Changed.
		/// </summary>
		/// <param name="sender">Return the object</param>
		/// <param name="e">Event Arguments</param>
		void Calendar_ViewChanged(object? sender, CalendarViewChangedEventArgs e)
		{
			if (_weekNumberGrid != null)
			{
				_weekNumberGrid.IsVisible = e.NewView == CalendarView.Month;
			}

			if (_trailingDatesGrid != null)
			{
				_trailingDatesGrid.IsVisible = e.NewView != CalendarView.Year;
			}

			if (_comboBox != null && e.OldView != e.NewView)
			{
				_comboBox.SelectedIndex = Enum.GetValues(typeof(CalendarView)).Cast<CalendarView>().ToList().ToList().IndexOf(e.NewView);
			}

			if (_datePicker != null && _calendar != null)
			{
				DateTime visibleStartDate = e.NewVisibleDates[0];
				DateTime visibleEndDate = e.NewVisibleDates[e.NewVisibleDates.Count - 1];
				if (_calendar.View == CalendarView.Month)
				{
					visibleStartDate = e.NewVisibleDates[e.NewVisibleDates.Count / 2];
					visibleStartDate = new DateTime(visibleStartDate.Year, visibleStartDate.Month, 1);
					int days = DateTime.DaysInMonth(visibleStartDate.Year, visibleStartDate.Month);
					visibleEndDate = new DateTime(visibleStartDate.Year, visibleStartDate.Month, days);
				}
				else if (_calendar.View != CalendarView.Year)
				{
					visibleEndDate = e.NewVisibleDates[e.NewVisibleDates.Count - 3];
				}

				if (_datePicker.Date >= visibleStartDate.Date && _datePicker.Date <= visibleEndDate.Date)
				{
					return;
				}

				_datePicker.Date = visibleStartDate.Date;
			}
		}

		/// <summary>
		/// Method for Combo box calendar selection mode changed.
		/// </summary>
		/// <param name="sender">Return the object</param>
		/// <param name="e">Event Arguments</param>
		void ComboBox_SelectionTypeChanged(object? sender, EventArgs e)
		{
			if (_calendar != null && sender is Microsoft.Maui.Controls.Picker picker && picker.SelectedItem is CalendarSelectionMode selectionMode)
			{
				_calendar.SelectionMode = selectionMode;
				if (_calendar.SelectionMode == CalendarSelectionMode.Range)
				{
					if (_selectionDirectionGrid != null)
					{
						_selectionDirectionGrid.IsVisible = true;
					}

					if (_enableSwipeSelectionGrid != null)
					{
						_enableSwipeSelectionGrid.IsVisible = true;
					}
				}
				else if (_calendar.SelectionMode == CalendarSelectionMode.MultiRange)
				{
					if (_selectionDirectionGrid != null)
					{
						_selectionDirectionGrid.IsVisible = false;
					}

					if (_enableSwipeSelectionGrid != null)
					{
						_enableSwipeSelectionGrid.IsVisible = true;
					}
				}
				else
				{
					if (_selectionDirectionGrid != null)
					{
						_selectionDirectionGrid.IsVisible = false;
					}

					if (_enableSwipeSelectionGrid != null)
					{
						_enableSwipeSelectionGrid.IsVisible = false;
					}
				}
			}
		}

		/// <summary>
		/// Method for Combo box selection shape changed.
		/// </summary>
		/// <param name="sender">Return the object</param>
		/// <param name="e">Event Arguments</param>
		void ComboBox_SelectionShapeChanged(object? sender, EventArgs e)
		{
			if (_calendar != null && sender is Microsoft.Maui.Controls.Picker picker && picker.SelectedItem is CalendarSelectionShape selectionShape)
			{
				_calendar.SelectionShape = selectionShape;
			}
		}

		/// <summary>
		/// Method for Combo box selection view changed.
		/// </summary>
		/// <param name="sender">Return the object</param>
		/// <param name="e">Event Arguments</param>
		void ComboBox_SelectionChanged(object? sender, EventArgs e)
		{
			if (_calendar != null && sender is Microsoft.Maui.Controls.Picker picker && picker.SelectedItem is CalendarView view)
			{
				_calendar.View = view;
				if (_trailingDatesGrid != null)
				{
					_trailingDatesGrid.IsVisible = _calendar.View != CalendarView.Year;
				}

				if (_weekNumberGrid != null)
				{
					_weekNumberGrid.IsVisible = _calendar.View == CalendarView.Month;
				}
			}
		}

		/// <summary>
		/// Method for allow view navigation switch toggle changed.
		/// </summary>
		/// <param name="sender">return the object</param>
		/// <param name="e">Event Arguments</param>
		void AllowViewNavigationSwitch_Toggled(object? sender, ToggledEventArgs e)
		{
			if (_calendar != null)
			{
				_calendar.AllowViewNavigation = e.Value;
			}
		}

		/// <summary>
		/// Method for week number switch toggle changed.
		/// </summary>
		/// <param name="sender">return the object</param>
		/// <param name="e">Event Arguments</param>
		void WeekNumberSwitch_Toggled(object? sender, ToggledEventArgs e)
		{
			if (_calendar != null)
			{
				_calendar.MonthView.ShowWeekNumber = e.Value;
			}
		}

		/// <summary>
		/// Method for trailing dates switch toggle changed.
		/// </summary>
		/// <param name="sender">return the object</param>
		/// <param name="e">Event Arguments</param>
		void TrailingDatesSwitch_Toggled(object? sender, ToggledEventArgs e)
		{
			if (_calendar != null)
			{
				_calendar.ShowTrailingAndLeadingDates = e.Value;
			}
		}

		/// <summary>
		/// Method for current time indicator switch toggle changed.
		/// </summary>
		/// <param name="sender">return the object</param>
		/// <param name="e">Event Arguments</param>
		void EnablePastDates_Toggled(object? sender, ToggledEventArgs e)
		{
			if (_calendar != null)
			{
				_calendar.EnablePastDates = e.Value;
			}
		}

		/// <summary>
		/// Method for enable swipe selection switch toggle changed
		/// </summary>
		/// <param name="sender">return the object</param>
		/// <param name="e">Event Arguments</param>
		void EnableSwipeSelection_Toggled(object? sender, ToggledEventArgs e)
		{
			if (_calendar != null)
			{
				_calendar.EnableSwipeSelection = e.Value;
			}
		}

		/// <summary>
		/// Method for enable today button switch toggle changed
		/// </summary>
		/// <param name="sender">return the object</param>
		/// <param name="e">Event Arguments</param>
		void EnableTodayButtonSwitch_Toggled(object? sender, ToggledEventArgs e)
		{
			if (_calendar != null)
			{
				_calendar.FooterView.ShowTodayButton = e.Value;
			}
		}

		/// <summary>
		/// Method for navigation month switch toggle changed
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void NavigationToAdjacentMonth_StateChanged(object? sender, ToggledEventArgs e)
		{
			if (_calendar != null)
			{
				_calendar.NavigateToAdjacentMonth = e.Value;
			}
		}

		/// <summary>
		/// Method for enable action buttons switch toggle changed
		/// </summary>
		/// <param name="sender">return the object</param>
		/// <param name="e">Event Arguments</param>
		void EnableActionButtonsSwitch_Toggled(object? sender, ToggledEventArgs e)
		{
			if (_calendar != null)
			{
				_calendar.FooterView.ShowActionButtons = e.Value;
			}
		}

		/// <summary>
		/// Method to corner radius slider value changed
		/// </summary>
		/// <param name="sender">return the object</param>
		/// <param name="e">Event Arguments</param>
		void CornerRadiusSlider_ValueChanged(object? sender, ValueChangedEventArgs e)
		{
			if (_calendar != null)
			{
				_calendar.CornerRadius = e.NewValue;
			}
		}

		/// <summary>
		/// Begins when the behavior attached to the view 
		/// </summary>
		/// <param name="bindable">bindable value</param>
		protected override void OnDetachingFrom(SampleView bindable)
		{
			base.OnDetachingFrom(bindable);
			if (_weekNumberGrid != null)
			{
				_weekNumberGrid = null;
			}

			if (_weekNumberSwitch != null)
			{
				_weekNumberSwitch.Toggled -= WeekNumberSwitch_Toggled;
				_weekNumberSwitch = null;
			}

			if (_trailingDatesSwitch != null)
			{
				_trailingDatesSwitch.Toggled -= TrailingDatesSwitch_Toggled;
				_trailingDatesSwitch = null;
			}

			if (_enableDatesSwitch != null)
			{
				_enableDatesSwitch.Toggled -= EnablePastDates_Toggled;
				_enableDatesSwitch = null;
			}

			if (_enableSwipeSelectionSwitch != null)
			{
				_enableSwipeSelectionSwitch.Toggled -= EnableSwipeSelection_Toggled;
				_enableDatesSwitch = null;
			}

			if (_allowViewNavigationSwitch != null)
			{
				_allowViewNavigationSwitch.Toggled -= AllowViewNavigationSwitch_Toggled;
				_allowViewNavigationSwitch = null;
			}

			if (_enableActionButtonsSwitch != null)
			{
				_enableActionButtonsSwitch.Toggled -= EnableActionButtonsSwitch_Toggled;
				_enableActionButtonsSwitch = null;
			}

			if (_enableTodayButtonSwitch != null)
			{
				_enableTodayButtonSwitch.Toggled -= EnableTodayButtonSwitch_Toggled;
				_enableTodayButtonSwitch = null;
			}

			if (_navigationToAdjacentMonth != null)
			{
				_navigationToAdjacentMonth.Toggled -= NavigationToAdjacentMonth_StateChanged;
				_navigationToAdjacentMonth = null;
			}

			if (_cornerRadiusSlider != null)
			{
				_cornerRadiusSlider.ValueChanged -= CornerRadiusSlider_ValueChanged;
			}

			if (_calendar != null)
			{
				_calendar.ViewChanged -= Calendar_ViewChanged;
				_calendar = null;
			}

			if (_datePicker != null)
			{
				_datePicker.DateSelected -= DatePicker_DateSelected;
				_datePicker = null;
			}
		}
	}

	public class ViewModel : INotifyPropertyChanged
	{
		double cornerRadius = 10;

		public ViewModel()
		{

		}

		public double CornerRadius
		{
			get
			{
				return cornerRadius;
			}
			set
			{
				cornerRadius = Math.Round(value);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CornerRadius)));
			}
		}

		public event PropertyChangedEventHandler? PropertyChanged;
	}
}