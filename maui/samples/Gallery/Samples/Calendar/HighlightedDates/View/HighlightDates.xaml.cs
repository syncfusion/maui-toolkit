using Syncfusion.Maui.Toolkit.Calendar;

namespace Syncfusion.Maui.ControlsGallery.Calendar.Calendar
{
    /// <summary>
    /// Interaction logic for GettingStarted.xaml
    /// </summary>
    public partial class HighlightDates : SampleView
    {
        /// <summary>
        /// Check the application theme is light or dark.
        /// </summary>
        readonly bool _isLightTheme = Application.Current?.RequestedTheme == AppTheme.Light;

        /// <summary>
        /// Initializes a new instance of the <see cref="HighlightDates" /> class.
        /// </summary>
        public HighlightDates()
        {
            InitializeComponent();
            comboBox.ItemsSource = new List<string>() { "Dark - Blue", "Light - Teal", "Light - Blue", "Light - Navy Blue" };
            comboBox.SelectedIndex = 0;

            SfCalendar calendar = HighlightCalendar;
#if IOS || MACCATALYST
            border.IsVisible = true;
#if MACCATALYST
            border.Stroke = _isLightTheme ? Colors.White : Color.FromRgba("#1C1B1F");
#else
            border.Stroke = Colors.Transparent;
#endif
            calendar = HighlightCalendar1;
#else
            frame.IsVisible = true;
#if ANDROID
            frame.Stroke = Colors.Transparent;
#else
            frame.Stroke = _isLightTheme ? Colors.White : Color.FromRgba("#1C1B1F");
#endif
#endif

            calendar.HeaderView.TextStyle = new CalendarTextStyle()
            {
                TextColor = Color.FromArgb("#FFFFFF")
            };
            UpdateDarkBlueTheme(calendar);
            calendar.SelectableDayPredicate = (date) =>
            {
                if (date == DateTime.Now.Date)
                {
                    return true;
                }

                if (date.Day % 11 == 0)
                {
                    if (date.DayOfWeek != DayOfWeek.Sunday && date.DayOfWeek != DayOfWeek.Saturday)
                    {
                        return false;
                    }
                }

                return true;
            };
        }

        /// <summary>
        ///  When the page disappears due to navigating away from the page within the application.
        /// </summary>
        public override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        CalendarIconDetails? GetSpecialDates(DateTime date, Color iconColor)
        {
            if (date != DateTime.Now.Date && date.Day % 9 == 0 && date.DayOfWeek != DayOfWeek.Sunday && date.DayOfWeek != DayOfWeek.Saturday)
            {
                Random random = new Random();
                Array values = Enum.GetValues(typeof(CalendarIcon));
                CalendarIcon? randomIcon = (CalendarIcon?)values.GetValue(random.Next(values.Length));
                CalendarIconDetails icon = new CalendarIconDetails();
                if (randomIcon != null)
                {
                    icon.Icon = (CalendarIcon)randomIcon;
                }

                icon.Fill = iconColor;
                return icon;
            }

            return null;
        }

        /// <summary>
        /// Method to combo box selection changed.
        /// </summary>
        /// <param name="sender">Return the object</param>
        /// <param name="e">Event Arguments</param>
        void comboBox_SelectionChanged(object sender, EventArgs e)
        {
            if (sender is Microsoft.Maui.Controls.Picker picker && picker.SelectedItem is string theme)
            {
                if (theme == null)
                {
                    return;
                }

                SfCalendar calendar = HighlightCalendar;
#if IOS || MACCATALYST
            calendar = HighlightCalendar1;
#endif

                if (theme == "Light - Teal")
                {
                    UpdateLightTealTheme(calendar);
                }
                else if (theme == "Light - Blue")
                {
                    UpdateLightBlueTheme(calendar);
                }
                else if (theme == "Light - Navy Blue")
                {
                    UpdateLightNavyBlueTheme(calendar);
                }
                else if (theme == "Dark - Blue")
                {
                    UpdateDarkBlueTheme(calendar);
                }
            }
        }

        /// <summary>
        /// Method to update light teal theme.
        /// </summary>
        /// <param name="calendar">The calendar instance.</param>
        void UpdateLightTealTheme(SfCalendar calendar)
        {
            calendar.HeaderView.Background = Color.FromArgb("#2A5D59");
            calendar.TodayHighlightBrush = Color.FromArgb("#67D1BF");
            calendar.SelectionBackground = Color.FromArgb("#67D1BF");
            Color specialDatesIconColor = Color.FromArgb("#8B6010");
            calendar.MonthView = new CalendarMonthView()
            {
#nullable disable
                SpecialDayPredicate = (date) =>
                {
                    return GetSpecialDates(date, specialDatesIconColor);
                },
#nullable enable
                WeekendDays = new List<DayOfWeek>() { DayOfWeek.Sunday, DayOfWeek.Saturday },
                HeaderView = new CalendarMonthHeaderView()
                {
                    Background = Color.FromArgb("#2A5D59"),
                    TextStyle = new CalendarTextStyle()
                    {
                        TextColor = Color.FromArgb("#FFFFFF")
                    },
                },
                Background = Color.FromArgb("#F0FFFB"),
                TextStyle= new CalendarTextStyle()
                {
                    TextColor = Color.FromArgb("#2A5D59")
                },
                TrailingLeadingDatesBackground = Color.FromArgb("#F0FFFB"),
                TrailingLeadingDatesTextStyle = new CalendarTextStyle()
                {
                    TextColor = Color.FromArgb("#2A5D59").WithAlpha(0.6f)
                },
                WeekendDatesBackground = Color.FromArgb("#E2F9F3"),
                WeekendDatesTextStyle = new CalendarTextStyle()
                {
                    TextColor = Color.FromArgb("#2A5D59")
                },
                SpecialDatesBackground = Color.FromArgb("#F0FFFB"),
                SpecialDatesTextStyle = new CalendarTextStyle()
                {
                    TextColor = Color.FromArgb("#8B6010")
                },
                DisabledDatesBackground = Color.FromArgb("#F0FFFB"),
                DisabledDatesTextStyle = new CalendarTextStyle()
                {
                    TextColor = Color.FromArgb("#2A5D59").WithAlpha(0.6f)
                },
                TodayBackground = Color.FromArgb("#F0FFFB"),
                TodayTextStyle = new CalendarTextStyle()
                {
                    TextColor = Color.FromArgb("#67D1BF")
                },
                SelectionTextStyle = new CalendarTextStyle()
                {
                    TextColor = Color.FromArgb("#004E48")
                }
            };
        }

        /// <summary>
        /// Method to update light blue theme.
        /// </summary>
        /// <param name="calendar">The calendar instance.</param>
        void UpdateLightBlueTheme(SfCalendar calendar)
        {
            calendar.HeaderView.Background = Color.FromArgb("#2242A9");
            calendar.TodayHighlightBrush = Color.FromArgb("#87B9FF");
            calendar.SelectionBackground = Color.FromArgb("#87B9FF");
            Color specialDatesIconColor = Color.FromArgb("#8F49D0");
            calendar.MonthView = new CalendarMonthView()
            {
#nullable disable
                SpecialDayPredicate = (date) =>
                {
                    return GetSpecialDates(date, specialDatesIconColor);
                },
#nullable enable
                WeekendDays = new List<DayOfWeek>() { DayOfWeek.Sunday, DayOfWeek.Saturday },
                HeaderView = new CalendarMonthHeaderView()
                {
                    Background = Color.FromArgb("#2242A9"),
                    TextStyle = new CalendarTextStyle()
                    {
                        TextColor = Color.FromArgb("#FFFFFF")
                    },
                },
                Background = Color.FromArgb("#F0F6FE"),
                TextStyle = new CalendarTextStyle()
                {
                    TextColor = Color.FromArgb("#233B85")
                },
                TrailingLeadingDatesBackground = Color.FromArgb("#F0F6FE"),
                TrailingLeadingDatesTextStyle = new CalendarTextStyle()
                {
                    TextColor = Color.FromArgb("#233B85").WithAlpha(0.6f)
                },
                WeekendDatesBackground = Color.FromArgb("#DEEAFC"),
                WeekendDatesTextStyle = new CalendarTextStyle()
                {
                    TextColor = Color.FromArgb("#233B85")
                },
                SpecialDatesBackground = Color.FromArgb("#F0F6FE"),
                SpecialDatesTextStyle = new CalendarTextStyle()
                {
                    TextColor = Color.FromArgb("#8F49D0")
                },
                DisabledDatesBackground = Color.FromArgb("#F0F6FE"),
                DisabledDatesTextStyle = new CalendarTextStyle()
                {
                    TextColor = Color.FromArgb("#233B85").WithAlpha(0.6f)
                },
                TodayBackground = Color.FromArgb("#F0F6FE"),
                TodayTextStyle = new CalendarTextStyle()
                {
                    TextColor = Color.FromArgb("#87B9FF")
                },
                SelectionTextStyle = new CalendarTextStyle()
                {
                    TextColor = Color.FromArgb("#14234F")
                }
            };
        }

        /// <summary>
        /// Method to update the light navy blue theme.
        /// </summary>
        /// <param name="calendar">The calendar instance.</param>
        void UpdateLightNavyBlueTheme(SfCalendar calendar)
        {
            calendar.HeaderView.Background = Color.FromArgb("#27187E");
            calendar.TodayHighlightBrush = Color.FromArgb("#F08B34");
            calendar.SelectionBackground = Color.FromArgb("#F08B34");
            Color specialDatesIconColor = Color.FromArgb("#5A00A5");
            calendar.MonthView = new CalendarMonthView()
            {
#nullable disable
                SpecialDayPredicate = (date) =>
                {
                    return GetSpecialDates(date, specialDatesIconColor);
                },
#nullable enable
                WeekendDays = new List<DayOfWeek>() { DayOfWeek.Sunday, DayOfWeek.Saturday },
                HeaderView = new CalendarMonthHeaderView()
                {
                    Background = Color.FromArgb("#27187E"),
                    TextStyle = new CalendarTextStyle()
                    {
                        TextColor = Color.FromArgb("#FFFFFF")
                    },
                },
                Background = Color.FromArgb("#EBEEFF"),
                TextStyle = new CalendarTextStyle()
                {
                    TextColor = Color.FromArgb("#27187E")
                },
                TrailingLeadingDatesBackground = Color.FromArgb("#EBEEFF"),
                TrailingLeadingDatesTextStyle = new CalendarTextStyle()
                {
                    TextColor = Color.FromArgb("#27187E").WithAlpha(0.6f)
                },
                WeekendDatesBackground = Color.FromArgb("#DDE2FF"),
                WeekendDatesTextStyle = new CalendarTextStyle()
                {
                    TextColor = Color.FromArgb("#27187E")
                },
                SpecialDatesBackground = Color.FromArgb("#EBEEFF"),
                SpecialDatesTextStyle = new CalendarTextStyle()
                {
                    TextColor = Color.FromArgb("#5A00A5")
                },
                DisabledDatesBackground = Color.FromArgb("#EBEEFF"),
                DisabledDatesTextStyle = new CalendarTextStyle()
                {
                    TextColor = Color.FromArgb("#27187E").WithAlpha(0.6f)
                },
                TodayBackground = Color.FromArgb("#EBEEFF"),
                TodayTextStyle = new CalendarTextStyle()
                {
                    TextColor = Color.FromArgb("#F08B34")
                },
                SelectionTextStyle = new CalendarTextStyle()
                {
                    TextColor = Color.FromArgb("#27187E")
                }
            };
        }

        /// <summary>
        /// Method to update the dark blue theme.
        /// </summary>
        /// <param name="calendar">The calendar instance.</param>
        void UpdateDarkBlueTheme(SfCalendar calendar)
        {
            calendar.HeaderView.Background = Color.FromArgb("#143F85");
            calendar.TodayHighlightBrush = Color.FromArgb("#6DC7E1");
            calendar.SelectionBackground = Color.FromArgb("#6DC7E1");
            Color specialDatesIconColor = Color.FromArgb("#FFCF55");
            calendar.MonthView = new CalendarMonthView()
            {
#nullable disable
                SpecialDayPredicate = (date) =>
                {
                    return GetSpecialDates(date, specialDatesIconColor);
                },
#nullable enable
                WeekendDays = new List<DayOfWeek>() { DayOfWeek.Sunday, DayOfWeek.Saturday },
                HeaderView = new CalendarMonthHeaderView()
                {
                    Background = Color.FromArgb("#143F85"),
                    TextStyle = new CalendarTextStyle()
                    {
                        TextColor = Color.FromArgb("#FFFFFF")
                    },
                },
                Background = Color.FromArgb("#3076B1"),
                TextStyle = new CalendarTextStyle()
                {
                    TextColor = Color.FromArgb("#FFFFFF")
                },
                TrailingLeadingDatesBackground = Color.FromArgb("#3076B1"),
                TrailingLeadingDatesTextStyle = new CalendarTextStyle()
                {
                    TextColor = Color.FromArgb("#FFFFFF").WithAlpha(0.6f)
                },
                WeekendDatesBackground = Color.FromArgb("#3F95C2"),
                WeekendDatesTextStyle = new CalendarTextStyle()
                {
                    TextColor = Color.FromArgb("#FFFFFF")
                },
                SpecialDatesBackground = Color.FromArgb("#3076B1"),
                SpecialDatesTextStyle = new CalendarTextStyle()
                {
                    TextColor = Color.FromArgb("#FFCF55")
                },
                DisabledDatesBackground = Color.FromArgb("#3076B1"),
                DisabledDatesTextStyle = new CalendarTextStyle()
                {
                    TextColor = Color.FromArgb("#FFFFFF").WithAlpha(0.6f)
                },
                TodayBackground = Color.FromArgb("#3076B1"),
                TodayTextStyle = new CalendarTextStyle()
                {
                    TextColor = Color.FromArgb("#6DC7E1")
                },
                SelectionTextStyle = new CalendarTextStyle()
                {
                    TextColor = Color.FromArgb("#000000").WithAlpha(0.87f)
                }
            };
        }
    }
}