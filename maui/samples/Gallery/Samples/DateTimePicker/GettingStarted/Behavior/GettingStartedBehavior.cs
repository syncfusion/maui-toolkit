using Syncfusion.Maui.Toolkit.Picker;
using System.Collections.ObjectModel;

namespace Syncfusion.Maui.ControlsGallery.Picker.SfDateTimePicker
{
    /// <summary>
    /// The GettingStartedBehavior class.
    /// </summary>
    public class GettingStartedBehavior : Behavior<SampleView>
    {
        /// <summary>
        /// Picker view 
        /// </summary>
        Syncfusion.Maui.Toolkit.Picker.SfDateTimePicker? _dateTimePicker;

        /// <summary>
        /// The show header switch
        /// </summary>
        Switch? _showHeaderSwitch, _showColumnHeaderSwitch, _showFooterSwitch, _clearSelectionSwitch, _showEnableLoopingSwitch;

        /// <summary>
        /// The date format combo box.
        /// </summary>
        Microsoft.Maui.Controls.Picker? _dateFormatComboBox, _timeFormatComboBox, _textDisplayComboBox;

        /// <summary>
        /// The date format to set the combo box item source.
        /// </summary>
        ObservableCollection<object>? _dateFormat;

        /// <summary>
        /// The time format to set the combo box item source.
        /// </summary>
        ObservableCollection<object>? _timeFormat;

        /// <summary>
        /// The text display to set the combo box item source.
        /// </summary>
        ObservableCollection<object>? _textDisplay;

        /// <summary>
        /// Check the application theme is light or dark.
        /// </summary>
        readonly bool _isLightTheme = Application.Current?.RequestedTheme == AppTheme.Light;

        /// <summary>
        /// Get the previous selected date and time from date time picker.
        /// </summary>
        DateTime? _previousDate;

        /// <summary>
        /// Begins when the behavior attached to the view 
        /// </summary>
        /// <param name="sampleView">bindable value</param>
        protected override void OnAttachedTo(SampleView sampleView)
        {
            base.OnAttachedTo(sampleView);

#if IOS || MACCATALYST
            Border border = sampleView.Content.FindByName<Border>("border");
            border.IsVisible = true;
            border.Stroke = _isLightTheme ? Color.FromArgb("#CAC4D0") : Color.FromArgb("#49454F");
            _dateTimePicker = sampleView.Content.FindByName<Syncfusion.Maui.Toolkit.Picker.SfDateTimePicker>("DateTimePicker1");
#else
            Border frame = sampleView.Content.FindByName<Border>("frame");
            frame.IsVisible = true;
            frame.Stroke = _isLightTheme ? Color.FromArgb("#CAC4D0") : Color.FromArgb("#49454F");
            _dateTimePicker = sampleView.Content.FindByName<Syncfusion.Maui.Toolkit.Picker.SfDateTimePicker>("DateTimePicker");
#endif

            _previousDate = _dateTimePicker.SelectedDate;
            _dateTimePicker.BlackoutDateTimes = new ObservableCollection<DateTime>()
            {
                DateTime.Today.AddDays(2),
                DateTime.Today.AddDays(5),
                DateTime.Today.AddDays(8),
                DateTime.Today.AddDays(-3),
                DateTime.Today.AddDays(-4),
                DateTime.Today.AddDays(-8),
                DateTime.Now.AddHours(1).AddMinutes(2),
                DateTime.Now.AddHours(1).AddMinutes(4),
                DateTime.Now.AddHours(1).AddMinutes(5),
                DateTime.Now.AddHours(1).AddMinutes(-1),
                DateTime.Now.AddHours(1).AddMinutes(-3),
                DateTime.Now.AddHours(1).AddMinutes(-9),
                DateTime.Now.AddHours(1).AddMinutes(1),
                DateTime.Now.AddHours(-1).AddMinutes(2),
                DateTime.Now.AddHours(-1).AddMinutes(6),
                DateTime.Now.AddHours(-1).AddMinutes(-2),
                DateTime.Now.AddHours(-1).AddMinutes(-9),
                DateTime.Now.AddHours(-1).AddMinutes(-7),
                DateTime.Now.AddMinutes(1),
                DateTime.Now.AddMinutes(2),
                DateTime.Now.AddMinutes(4),
                DateTime.Now.AddMinutes(-8),
                DateTime.Now.AddMinutes(-4),
                DateTime.Now.AddMinutes(-1)
            };

            _dateTimePicker.SelectionChanged += DateTimePicker_SelectionChanged;
            _showHeaderSwitch = sampleView.Content.FindByName<Switch>("showHeaderSwitch");
            _showColumnHeaderSwitch = sampleView.Content.FindByName<Switch>("showColumnHeaderSwitch");
            _showFooterSwitch = sampleView.Content.FindByName<Switch>("showFooterSwitch");
            _clearSelectionSwitch = sampleView.Content.FindByName<Switch>("clearSelectionSwitch");
            _showEnableLoopingSwitch = sampleView.Content.FindByName<Switch>("enableLoopingSwitch");
            _dateTimePicker.SelectedTextStyle.TextColor = _isLightTheme ? Color.FromArgb("#FFFFFF") : Color.FromArgb("#381E72");

            _dateFormat = new ObservableCollection<object>()
            {
                 "dd MM", "dd MM yyyy", "dd MMM yyyy", "M d yyyy", "MM dd yyyy", "MM yyyy", "MMM yyyy", "yyyy MM dd", "Default"
            };

            _timeFormat = new ObservableCollection<object>()
            {
                "H mm", "H mm ss", "h mm ss tt", "h mm tt", "HH mm", "HH mm ss", "HH mm ss fff", "hh mm ss tt", "hh mm ss fff tt", "hh mm tt", "hh tt", "ss fff", "mm ss", "mm ss fff", "Default"
            };

            _dateFormatComboBox = sampleView.Content.FindByName<Microsoft.Maui.Controls.Picker>("dateFormatComboBox");
            _dateFormatComboBox.ItemsSource = _dateFormat;
            _dateFormatComboBox.SelectedIndex = 1;
            _dateFormatComboBox.SelectedIndexChanged += DateFormatComboBox_SelectionChanged;

            _timeFormatComboBox = sampleView.Content.FindByName<Microsoft.Maui.Controls.Picker>("timeFormatComboBox");
            _timeFormatComboBox.ItemsSource = _timeFormat;
            _timeFormatComboBox.SelectedIndex = 1;
            _timeFormatComboBox.SelectedIndexChanged += TimeFormatComboBox_SelectionChanged;

            _textDisplay = new ObservableCollection<object>()
            {
                "Default", "Fade", "Shrink", "FadeAndShrink"
            };

            _textDisplayComboBox = sampleView.Content.FindByName<Microsoft.Maui.Controls.Picker>("textDisplayComboBox");
            _textDisplayComboBox.ItemsSource = _textDisplay;
            _textDisplayComboBox.SelectedIndex = 0;
            _textDisplayComboBox.SelectedIndexChanged += TextdisplayComboBox_SelectionChanged;

            if (_showHeaderSwitch != null)
            {
                _showHeaderSwitch.Toggled += ShowHeaderSwitch_Toggled;
            }

            if (_showColumnHeaderSwitch != null)
            {
                _showColumnHeaderSwitch.Toggled += ShowColumnHeaderSwitch_Toggled;
            }

            if (_showFooterSwitch != null)
            {
                _showFooterSwitch.Toggled += ShowFooterSwitch_Toggled;
            }

            if (_clearSelectionSwitch != null)
            {
                _clearSelectionSwitch.Toggled += ClearSelectionSwitch_Toggled;
            }

            if (_showEnableLoopingSwitch != null)
            {
                _showEnableLoopingSwitch.Toggled += EnableLoopingSwitch_Toggled;
            }
        }

        /// <summary>
        /// Method to handle the selection changed of the date time picker.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event args.</param>
        void DateTimePicker_SelectionChanged(object? sender, DateTimePickerSelectionChangedEventArgs e)
        {
            if (_clearSelectionSwitch != null && e.NewValue != null)
            {
                _previousDate = e.NewValue.Value;
                _clearSelectionSwitch.IsToggled = false;
            }
        }

        /// <summary>
        /// Method for show header switch toggle changed.
        /// </summary>
        /// <param name="sender">Return the object.</param>
        /// <param name="e">The Event Arguments.</param>
        void ShowHeaderSwitch_Toggled(object? sender, ToggledEventArgs e)
        {
            if (_dateTimePicker != null)
            {
                _dateTimePicker.HeaderView.Height = e.Value == true ? 50 : 0;
            }
        }

        /// <summary>
        /// Method for show column header switch toggle changed.
        /// </summary>
        /// <param name="sender">Return the object.</param>
        /// <param name="e">The Event Arguments.</param>
        void ShowColumnHeaderSwitch_Toggled(object? sender, ToggledEventArgs e)
        {
            if (_dateTimePicker != null)
            {
                _dateTimePicker.ColumnHeaderView.Height = e.Value == true ? 40 : 0;
            }
        }

        /// <summary>
        /// Method for show footer switch toggle changed.
        /// </summary>
        /// <param name="sender">Return the object.</param>
        /// <param name="e">The Event Arguments.</param>
        void ShowFooterSwitch_Toggled(object? sender, ToggledEventArgs e)
        {
            if (_dateTimePicker != null)
            {
                if (e.Value == true)
                {
                    _dateTimePicker.FooterView.Height = 40;
                }
                else if (e.Value == false)
                {
                    _dateTimePicker.FooterView.Height = 0;
                }
            }
        }

        /// <summary>
        /// Method for clear selection switch toggle changed.
        /// </summary>
        /// <param name="sender">Return the object.</param>
        /// <param name="e">The Event Arguments.</param>
        void ClearSelectionSwitch_Toggled(object? sender, ToggledEventArgs e)
        {
            if (_dateTimePicker != null)
            {
                if (e.Value == true)
                {
                    _dateTimePicker.SelectedDate = null;
                }
                else if (e.Value == false)
                {
                    _dateTimePicker.SelectedDate = _previousDate;
                }
            }
        }

        /// <summary>
        /// Method for enable looping switch toggle changed.
        /// </summary>
        /// <param name="sender">Return the object.</param>
        /// <param name="e">The Event Arguments.</param>
        void EnableLoopingSwitch_Toggled(object? sender, ToggledEventArgs e)
        {
            if (_dateTimePicker != null)
            {
                if (e.Value == true)
                {
                    _dateTimePicker.EnableLooping = true;
                }
                else if (e.Value == false)
                {
                    _dateTimePicker.EnableLooping = false;
                }
            }
        }

        /// <summary>
        /// The format combo box selection changed event.
        /// </summary>
        /// <param name="sender">Return the object.</param>
        /// <param name="e">The Event Arguments.</param>
        void DateFormatComboBox_SelectionChanged(object? sender, EventArgs e)
        {
            if (_dateTimePicker == null || _dateFormatComboBox == null)
            {
                return;
            }

            if (sender is Microsoft.Maui.Controls.Picker picker)
            {
                string? format = picker.SelectedItem.ToString();
                switch (format)
                {
                    case "dd MM":
                        _dateTimePicker.DateFormat = PickerDateFormat.dd_MM;
                        break;

                    case "dd MM yyyy":
                        _dateTimePicker.DateFormat = PickerDateFormat.dd_MM_yyyy;
                        break;

                    case "dd MMM yyyy":
                        _dateTimePicker.DateFormat = PickerDateFormat.dd_MMM_yyyy;
                        break;

                    case "M d yyyy":
                        _dateTimePicker.DateFormat = PickerDateFormat.M_d_yyyy;
                        break;

                    case "MM dd yyyy":
                        _dateTimePicker.DateFormat = PickerDateFormat.MM_dd_yyyy;
                        break;

                    case "MM yyyy":
                        _dateTimePicker.DateFormat = PickerDateFormat.MM_yyyy;
                        break;

                    case "MMM yyyy":
                        _dateTimePicker.DateFormat = PickerDateFormat.MMM_yyyy;
                        break;

                    case "yyyy MM dd":
                        _dateTimePicker.DateFormat = PickerDateFormat.yyyy_MM_dd;
                        break;
                    case "Default":
                        _dateTimePicker.DateFormat = PickerDateFormat.Default;
                        break;
                }
            }
        }

        /// <summary>
        /// The format combo box selection changed event.
        /// </summary>
        /// <param name="sender">Return the object.</param>
        /// <param name="e">The Event Arguments.</param>
        void TimeFormatComboBox_SelectionChanged(object? sender, EventArgs e)
        {
            if (_dateTimePicker == null || _timeFormatComboBox == null)
            {
                return;
            }

            if (sender is Microsoft.Maui.Controls.Picker picker)
            {
                string? format = picker.SelectedItem.ToString();

                switch (format)
                {
                    case "H mm":
                        _dateTimePicker.TimeFormat = PickerTimeFormat.H_mm;
                        break;

                    case "H mm ss":
                        _dateTimePicker.TimeFormat = PickerTimeFormat.H_mm_ss;
                        break;

                    case "h mm ss tt":
                        _dateTimePicker.TimeFormat = PickerTimeFormat.h_mm_ss_tt;
                        break;

                    case "h mm tt":
                        _dateTimePicker.TimeFormat = PickerTimeFormat.h_mm_tt;
                        break;

                    case "HH mm":
                        _dateTimePicker.TimeFormat = PickerTimeFormat.HH_mm;
                        break;

                    case "HH mm ss":
                        _dateTimePicker.TimeFormat = PickerTimeFormat.HH_mm_ss;
                        break;
                    case "HH mm ss fff":
                        _dateTimePicker.TimeFormat = PickerTimeFormat.HH_mm_ss_fff;
                        break;

                    case "hh mm ss tt":
                        _dateTimePicker.TimeFormat = PickerTimeFormat.hh_mm_ss_tt;
                        break;

                    case "hh mm ss fff tt":
                        _dateTimePicker.TimeFormat = PickerTimeFormat.hh_mm_ss_fff_tt;
                        break;

                    case "hh mm tt":
                        _dateTimePicker.TimeFormat = PickerTimeFormat.hh_mm_tt;
                        break;

                    case "hh tt":
                        _dateTimePicker.TimeFormat = PickerTimeFormat.hh_tt;
                        break;

                    case "ss fff":
                        _dateTimePicker.TimeFormat = PickerTimeFormat.ss_fff;
                        break;

                    case "mm ss":
                        _dateTimePicker.TimeFormat = PickerTimeFormat.mm_ss;
                        break;

                    case "mm ss fff":
                        _dateTimePicker.TimeFormat = PickerTimeFormat.mm_ss_fff;
                        break;

                    case "Default":
                        _dateTimePicker.TimeFormat = PickerTimeFormat.Default;
                        break;
                }
            }
        }

        /// <summary>
        /// The text display combo box selection changed event.
        /// </summary>
        /// <param name="sender">Return the object.</param>
        /// <param name="e">The Event Arguments.</param>
        void TextdisplayComboBox_SelectionChanged(object? sender, EventArgs e)
        {
            if (_dateTimePicker == null || _textDisplayComboBox == null)
            {
                return;
            }

            if (sender is Microsoft.Maui.Controls.Picker picker)
            {
                string? format = picker.SelectedItem.ToString();
                switch (format)
                {
                    case "Default":
                        _dateTimePicker.TextDisplayMode = PickerTextDisplayMode.Default;
                        break;

                    case "Fade":
                        _dateTimePicker.TextDisplayMode = PickerTextDisplayMode.Fade;
                        break;

                    case "Shrink":
                        _dateTimePicker.TextDisplayMode = PickerTextDisplayMode.Shrink;
                        break;

                    case "FadeAndShrink":
                        _dateTimePicker.TextDisplayMode = PickerTextDisplayMode.FadeAndShrink;
                        break;
                }
            }
        }

        /// <summary>
        /// Begins when the behavior attached to the view 
        /// </summary>
        /// <param name="sampleView">bindable value</param>
        protected override void OnDetachingFrom(SampleView sampleView)
        {
            base.OnDetachingFrom(sampleView);
            if (_showHeaderSwitch != null)
            {
                _showHeaderSwitch.Toggled -= ShowHeaderSwitch_Toggled;
                _showHeaderSwitch = null;
            }

            if (_showColumnHeaderSwitch != null)
            {
                _showColumnHeaderSwitch.Toggled -= ShowColumnHeaderSwitch_Toggled;
                _showColumnHeaderSwitch = null;
            }

            if (_showFooterSwitch != null)
            {
                _showFooterSwitch.Toggled -= ShowFooterSwitch_Toggled;
                _showFooterSwitch = null;
            }

            if (_clearSelectionSwitch != null)
            {
                _clearSelectionSwitch.Toggled -= ClearSelectionSwitch_Toggled;
                _clearSelectionSwitch = null;
            }

            if (_showEnableLoopingSwitch != null)
            {
                _showEnableLoopingSwitch.Toggled -= EnableLoopingSwitch_Toggled;
                _showEnableLoopingSwitch = null;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GettingStartedBehavior"/> class.
        /// </summary>
        public GettingStartedBehavior()
        {
        }
    }
}