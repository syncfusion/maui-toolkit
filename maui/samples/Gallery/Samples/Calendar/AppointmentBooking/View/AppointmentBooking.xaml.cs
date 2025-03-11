using System.Globalization;
using Syncfusion.Maui.Toolkit.Calendar;
using Syncfusion.Maui.Toolkit.Buttons;
using Syncfusion.Maui.Toolkit.Popup;
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

            if (popUp != null)
            {
                popUp.FooterTemplate = GetFooterTemplate(popUp);
                popUp.ContentTemplate = GetContentTemplate(popUp);
            }

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
        /// Method to get the dynamic color.
        /// </summary>
        /// <param name="resourceName">The resource name.</param>
        /// <returns>The color.</returns>
        Color GetDynamicColor(string? resourceName = null)
        {
            if (resourceName != null && App.Current != null && App.Current.Resources.TryGetValue(resourceName, out var colorValue) && colorValue is Color color)
            {
                return color;
            }
            else
            {
                if (App.Current != null && App.Current.RequestedTheme == AppTheme.Light)
                {
                    return Color.FromRgb(0xFF, 0xFF, 0xFF);
                }
                else if (App.Current != null && App.Current.RequestedTheme == AppTheme.Dark)
                {
                    return Color.FromRgb(0x38, 0x1E, 0x72);
                }
            }

            return Colors.Transparent;
        }

        /// <summary>
        /// Method to get the Ok button style.
        /// </summary>
        /// <returns>The button style.</returns>
        Style GetOkButtonStyle()
        {
            return new Style(typeof(Button))
            {
                Setters =
                {
                    new Setter { Property = Button.CornerRadiusProperty, Value = 15 },
                    new Setter { Property = Button.BorderColorProperty, Value = Color.FromArgb("#6750A4") },
                    new Setter { Property = Button.BorderWidthProperty, Value = 1 },
                    new Setter { Property = Button.BackgroundColorProperty, Value = GetDynamicColor("SfCalendarTodayHighlightColor") },
                    new Setter { Property = Button.TextColorProperty, Value = GetDynamicColor() },
                    new Setter { Property = Button.FontSizeProperty, Value = 14 },
                }
            };
        }

        /// <summary>
        /// Method to get the footer template.
        /// </summary>
        /// <param name="popup">The pop up.</param>
        /// <returns>The data template.</returns>
        DataTemplate GetFooterTemplate(SfPopup popup)
        {
            var footerTemplate = new DataTemplate(() =>
            {
                var grid = new Grid
                {
                    ColumnSpacing = 12,
                    Padding = new Thickness(24)
                };
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                var oKButton = new Button
                {
                    Text = "OK",
                    Style = GetOkButtonStyle(),
                    WidthRequest = 96,
                    HeightRequest = 40
                };
                oKButton.Clicked += (sender, args) =>
                {
                    popup.Dismiss();
                };
                grid.Children.Add(oKButton);
                Grid.SetColumn(oKButton, 1);
                return grid;
            });

            return footerTemplate;
        }

        /// <summary>
        /// Method to get the content template.
        /// </summary>
        /// <param name="popup">The pop up.</param>
        /// <returns>The data template.</returns>
        DataTemplate GetContentTemplate(SfPopup popup)
        {
            var contentTemplate = new DataTemplate(() =>
            {
                var grid = new Grid();
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.1, GridUnitType.Star) });
                var label = new Label
                {
                    LineBreakMode = LineBreakMode.WordWrap,
                    Padding = new Thickness(20, 0, 0, 0),
                    FontSize = 16,
                    HorizontalOptions = LayoutOptions.Start,
                    HorizontalTextAlignment = TextAlignment.Start,
                    WidthRequest = 300,
                };

                label.BindingContext = popup;
                label.SetBinding(Label.TextProperty, "Message");
                var stackLayout = new StackLayout
                {
                    Margin = new Thickness(0, 2, 0, 0),
                    HeightRequest = 1,
                };

                stackLayout.BackgroundColor = _isLightTheme ? Color.FromArgb("#611c1b1f") : Color.FromArgb("#61e6e1e5");
                grid.Children.Add(label);
                grid.Children.Add(stackLayout);
                Grid.SetRow(label, 0);
                Grid.SetRow(stackLayout, 1);
                return grid;
            });

            return contentTemplate;
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

            popUp.Show();
        }

        /// <summary>
        /// Book the appointment on selected date and time slot.
        /// </summary>
        /// <param name="calendar">Calendar instance.</param>
        /// <param name="buttonLayout">Time slot button layout.</param>
        void BookAppointment(SfCalendar calendar, FlexLayout buttonLayout)
        {
            if (popUp == null)
            {
                return;
            }

            if (calendar.SelectedDate == null)
            {
                popUp.HeaderTitle = "Alert !";
                popUp.Message = "Please select a date to book an appointment";
                return;
            }

            if (_timeSlot == string.Empty)
            {
                popUp.HeaderTitle = "Alert !";
                popUp.Message = "Please select a time to book an appointment";
                return;
            }

            popUp.HeaderTitle = "Confirmation";
            DateTime dateTime = calendar.SelectedDate.Value;
            string dayText = dateTime.ToString("MMMM" + " " + dateTime.Day.ToString() + ", " + dateTime.ToString("yyyy"), CultureInfo.CurrentUICulture);
            popUp.Message = "Appointment booked for " + dayText + " " + _timeSlot;
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
                popUp.HeaderTitle = "Alert !";
                popUp.Message = "Please select a date to book an appointment ";
                popUp.Show();
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