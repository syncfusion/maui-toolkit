using System.Globalization;
using Syncfusion.Maui.Toolkit.Calendar;
using Syncfusion.Maui.Toolkit.Buttons;
using Color = Microsoft.Maui.Graphics.Color;

namespace Syncfusion.Maui.ControlsGallery.Calendar.Calendar
{
    /// <summary>
    /// Interaction logic for GettingStarted.xaml
    /// </summary>
    public partial class AppointmentBooking : SampleView
    {
        /// <summary>
        /// The time slot string is used to handle while book an appointment. While select the time slot then time slot variable value will be updates with respective tapped time slot.
        /// It is used to reset the time slot.
        /// </summary>
        string _timeSlot = string.Empty;

        /// <summary>
        /// Check the application theme is light or dark.
        /// </summary>
        readonly bool _isLightTheme = Application.Current?.RequestedTheme == AppTheme.Light;

        /// <summary>
        /// Initializes a new instance of the <see cref="Year" /> class.
        /// </summary>
        public AppointmentBooking()
        {
            InitializeComponent();
#if MACCATALYST
            border.IsVisible = true;
            border.Stroke = Colors.Transparent;
            InitializeCalendar(appointmentBooking1, deskTop);
#elif !ANDROID && !IOS
            frame.IsVisible = true;
            frame.Stroke = Colors.Transparent;
            InitializeCalendar(appointmentBooking, deskTop);
#else
            if (DeviceInfo.Idiom == DeviceIdiom.Tablet)
            {
                border.IsVisible = true;
                border.Stroke = Colors.Transparent;
                InitializeCalendar(appointmentBooking1, deskTop);
            }
            else
            {
                InitializeCalendar(mobileAppointmentBooking, mobile);
            }
#endif
        }

        /// <summary>
        /// Initialize the calendar.
        /// </summary>
        /// <param name="calendar">Calendar instance.</param>
        /// <param name="parent">Parent view of calendar control.</param>
        void InitializeCalendar(SfCalendar calendar, Grid parent)
        {
            parent.IsVisible = true;
            calendar.MaximumDate = DateTime.Now.Date.AddMonths(3);
            calendar.SelectedDate = DateTime.Now.Date;
        }

        /// <summary>
        /// Method to update the selected date changed.
        /// </summary>
        /// <param name="sender">The object.</param>
        /// <param name="e">Event arguments.</param>
        void AppointmentBooking_SelectionChanged(object sender, CalendarSelectionChangedEventArgs e)
        {
#if MACCATALYST
            UpdateDateSelection(appointmentBooking1, label1, flexLayout1);
#elif !ANDROID && !IOS
            UpdateDateSelection(appointmentBooking, label, flexLayout);
#else
            if (DeviceInfo.Idiom == DeviceIdiom.Tablet)
            {
                UpdateDateSelection(appointmentBooking1, label1, flexLayout1);
            }
            else
            {
                UpdateDateSelection(mobileAppointmentBooking, mobileLabel, mobileFlexLayout);
            }
#endif
            _timeSlot = string.Empty;
        }

        /// <summary>
        /// Update the UI based on calendar selected date.
        /// </summary>
        /// <param name="calendar">Calendar instance.</param>
        /// <param name="textLabel">Selected date text label.</param>
        /// <param name="buttonLayout">Time slot button layout.</param>
        void UpdateDateSelection(SfCalendar calendar, Label textLabel, FlexLayout buttonLayout)
        {
            if (calendar.SelectedDate == null)
            {
                return;
            }

            DateTime dateTime = calendar.SelectedDate.Value;
            string dayText = dateTime.ToString("MMMM" + " " + dateTime.Day.ToString() + ", " + dateTime.ToString("yyyy"), CultureInfo.CurrentUICulture);
            textLabel.Text = dayText;
            //// The time slot is empty then no need to reset the time slot.
            if (_timeSlot == string.Empty)
            {
                return;
            }

            foreach (SfButton child in buttonLayout.Children)
            {
                SfButton button = (SfButton)child;
                button.IsChecked = false;
            }
        }

        /// <summary>
        /// Method to Book an Appointment based on the selected date and selected time slot.
        /// </summary>
        /// <param name="sender">The object.</param>
        /// <param name="e">Event arguments.</param>
        void AppointmentanBooking(object sender, EventArgs e)
        {
#if MACCATALYST
            BookAppointment(appointmentBooking1, flexLayout1);
#elif !ANDROID && !IOS
            BookAppointment(appointmentBooking, flexLayout);
#else
            if (DeviceInfo.Idiom == DeviceIdiom.Tablet)
            {
                BookAppointment(appointmentBooking1, flexLayout1);
            }
            else
            {
                BookAppointment(mobileAppointmentBooking, mobileFlexLayout);
            }
#endif
        }

        /// <summary>
        /// Book the appointment on selected date and time slot.
        /// </summary>
        /// <param name="calendar">Calendar instance.</param>
        /// <param name="buttonLayout">Time slot button layout.</param>
        void BookAppointment(SfCalendar calendar, FlexLayout buttonLayout)
        {
            if (calendar.SelectedDate == null)
            {
                Application.Current?.Windows[0].Page?.DisplayAlert("Alert !", "Please select a date to book an appointment ", "Ok");
                return;
            }

            if (_timeSlot == string.Empty)
            {
                Application.Current?.Windows[0].Page?.DisplayAlert("Alert !", "Please select a time to book an appointment ", "Ok");
                return;
            }

            DateTime dateTime = calendar.SelectedDate.Value;
            string dayText = dateTime.ToString("MMMM" + " " + dateTime.Day.ToString() + ", " + dateTime.ToString("yyyy"), CultureInfo.CurrentUICulture);
            string text = "Appointment booked for " + dayText + " " + _timeSlot;
            Application.Current?.Windows[0].Page?.DisplayAlert("Confirmation", text, "Ok");
            calendar.SelectedDate = DateTime.Now.Date;
            calendar.DisplayDate = DateTime.Now.Date;
            _timeSlot = string.Empty;
            foreach (SfButton child in buttonLayout.Children)
            {
                SfButton button = (SfButton)child;
                button.IsChecked = false;
            }
        }

        /// <summary>
        /// Method to update the slot booking.
        /// </summary>
        /// <param name="sender">The object.</param>
        /// <param name="e">Event arguments.</param>
        void SlotBooking_Changed(object sender, EventArgs e)
        {
#if MACCATALYST
            UpdateTimeSlotSelection(appointmentBooking1, (SfButton)sender, flexLayout1);
#elif !ANDROID && !IOS
            UpdateTimeSlotSelection(appointmentBooking, (SfButton)sender, flexLayout);
#else
            if (DeviceInfo.Idiom == DeviceIdiom.Tablet)
            {
                UpdateTimeSlotSelection(appointmentBooking1, (SfButton)sender, flexLayout1);
            }
            else
            {
                UpdateTimeSlotSelection(mobileAppointmentBooking, (SfButton)sender, mobileFlexLayout);
            }
#endif
        }

        /// <summary>
        /// Update the UI based on selected time slot.
        /// </summary>
        /// <param name="calendar">Calendar instance.</param>
        /// <param name="selectedButton">Selected time slot button.</param>
        /// <param name="buttonLayout">Time slot button layout.</param>
        void UpdateTimeSlotSelection(SfCalendar calendar, SfButton selectedButton, FlexLayout buttonLayout)
        {
            if (calendar.SelectedDate == null)
            {
                Application.Current?.Windows[0].Page?.DisplayAlert("Alert !", "Please select a date to book an appointment ", "Ok");
                return;
            }

            foreach (SfButton child in buttonLayout.Children)
            {
                SfButton unPressedbutton = (SfButton)child;
                if (unPressedbutton == selectedButton)
                {
                    unPressedbutton.IsChecked = true;
                    _timeSlot = selectedButton.Text;
                    continue;
                }

                unPressedbutton.IsChecked = false;
            }
        }
    }
}