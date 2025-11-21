using System.Collections.ObjectModel;
using Syncfusion.Maui.Toolkit.Picker;

namespace Syncfusion.Maui.ControlsGallery.Picker.SfTimePicker
{
    public class GettingStartedBehavior : Behavior<SampleView>
    {
        /// <summary>
        /// Picker view
        /// </summary>
        Syncfusion.Maui.Toolkit.Picker.SfTimePicker? _timePicker;

        /// <summary>
        /// The show header switch
        /// </summary>
        Switch? _showHeaderSwitch, _showColumnHeaderSwitch, _showFooterSwitch, _clearSelectionSwitch, _showEnableLoopingSwitch;

        /// <summary>
        /// The date format combo box.
        /// </summary>
        Microsoft.Maui.Controls.Picker? _formatComboBox, _textDisplayComboBox;

        /// <summary>
        /// The time format to set the combo box item source.
        /// </summary>
        ObservableCollection<object>? _formats;

        /// <summary>
        /// The text display to set the combo box item source.
        /// </summary>
        ObservableCollection<object>? _textDisplay;

        /// <summary>
        /// The minimum value time picker.
        /// </summary>
        TimePicker? _minimumTimePicker;

        /// <summary>
        /// The maximum value time picker.
        /// </summary>
        TimePicker? _maximumTimePicker;

        /// <summary>
        /// Check the application theme is light or dark.
        /// </summary>
        readonly bool _isLightTheme = Application.Current?.RequestedTheme == AppTheme.Light;

        /// <summary>
        /// Get the previous selected time from time picker.
        /// </summary>
        TimeSpan? _previousTime;

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
            border.Stroke = _isLightTheme ? Color.FromArgb("#CAC4D0") : Color.FromArgb("#49454F");
            _timePicker = bindable.Content.FindByName<Syncfusion.Maui.Toolkit.Picker.SfTimePicker>("TimePicker1");
#else
            Border frame = bindable.Content.FindByName<Border>("frame");
            frame.IsVisible = true;
            frame.Stroke = _isLightTheme ? Color.FromArgb("#CAC4D0") : Color.FromArgb("#49454F");
            _timePicker = bindable.Content.FindByName<Syncfusion.Maui.Toolkit.Picker.SfTimePicker>("TimePicker");
#endif

            _previousTime = _timePicker.SelectedTime;
            _timePicker.HeaderView.Height = 50;
            _timePicker.HeaderView.Text = "Select a Time";
            _timePicker.BlackoutTimes = new ObservableCollection<TimeSpan>()
            {
                DateTime.Now.AddHours(1).AddMinutes(2).TimeOfDay,
                DateTime.Now.AddHours(1).AddMinutes(4).TimeOfDay,
                DateTime.Now.AddHours(1).AddMinutes(5).TimeOfDay,
                DateTime.Now.AddHours(1).AddMinutes(-1).TimeOfDay,
                DateTime.Now.AddHours(1).AddMinutes(-3).TimeOfDay,
                DateTime.Now.AddHours(1).AddMinutes(-9).TimeOfDay,
                DateTime.Now.AddHours(1).AddMinutes(1).TimeOfDay,
                DateTime.Now.AddHours(-1).AddMinutes(2).TimeOfDay,
                DateTime.Now.AddHours(-1).AddMinutes(6).TimeOfDay,
                DateTime.Now.AddHours(-1).AddMinutes(-2).TimeOfDay,
                DateTime.Now.AddHours(-1).AddMinutes(-9).TimeOfDay,
                DateTime.Now.AddHours(-1).AddMinutes(-7).TimeOfDay,
                DateTime.Now.AddMinutes(1).TimeOfDay,
                DateTime.Now.AddMinutes(2).TimeOfDay,
                DateTime.Now.AddMinutes(4).TimeOfDay,
                DateTime.Now.AddMinutes(-8).TimeOfDay,
                DateTime.Now.AddMinutes(-4).TimeOfDay,
                DateTime.Now.AddMinutes(-1).TimeOfDay,
            };

            _timePicker.SelectionChanged += TimePicker_SelectionChanged;
            _showHeaderSwitch = bindable.Content.FindByName<Switch>("showHeaderSwitch");
            _showColumnHeaderSwitch = bindable.Content.FindByName<Switch>("showColumnHeaderSwitch");
            _showFooterSwitch = bindable.Content.FindByName<Switch>("showFooterSwitch");
            _clearSelectionSwitch = bindable.Content.FindByName<Switch>("clearSelectionSwitch");
            _showEnableLoopingSwitch = bindable.Content.FindByName<Switch>("enableLoopingSwitch");
            _minimumTimePicker = bindable.Content.FindByName<TimePicker>("minimumTime");
            _maximumTimePicker = bindable.Content.FindByName<TimePicker>("maximumTime");
            _timePicker.SelectedTextStyle.TextColor = _isLightTheme ? Color.FromArgb("#FFFFFF") : Color.FromArgb("#381E72");

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

            if (_minimumTimePicker != null)
            {
                _minimumTimePicker.PropertyChanged += MinimumTimePicker_PropertyChanged;
            }

            if (_maximumTimePicker != null)
            {
                _maximumTimePicker.PropertyChanged += MaximumTimePicker_PropertyChanged;
            }

            _formats = new ObservableCollection<object>()
            {
                "H mm", "H mm ss", "h mm ss tt", "h mm tt", "HH mm", "HH mm ss", "HH mm ss fff", "hh mm ss tt", "hh mm ss fff tt", "hh mm tt", "hh tt", "ss fff", "mm ss", "mm ss fff", "Default"
            };

            _formatComboBox = bindable.Content.FindByName<Microsoft.Maui.Controls.Picker>("formatComboBox");
            _formatComboBox.ItemsSource = _formats;
            _formatComboBox.SelectedIndex = 1;
            _formatComboBox.SelectedIndexChanged += FormatComboBox_SelectionChanged;
            ;

            _textDisplay = new ObservableCollection<object>()
            {
                "Default", "Fade", "Shrink", "FadeAndShrink"
            };

            _textDisplayComboBox = bindable.Content.FindByName<Microsoft.Maui.Controls.Picker>("textDisplayComboBox");
            _textDisplayComboBox.ItemsSource = _textDisplay;
            _textDisplayComboBox.SelectedIndex = 0;
            _textDisplayComboBox.SelectedIndexChanged += TextdisplayComboBox_SelectionChanged;
        }

        /// <summary>
        /// Method for handle the maximum time picker property changed.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event args.</param>
        void MaximumTimePicker_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (_timePicker != null && _maximumTimePicker != null && e.PropertyName == "Time")
            {
                if (_maximumTimePicker.Time.HasValue)
                {
                    _timePicker.MaximumTime = _maximumTimePicker.Time.Value;
                }
            }
        }

        /// <summary>
        /// Method for handle the minimum time picker property changed.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event args.</param>
        void MinimumTimePicker_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (_timePicker != null && _minimumTimePicker != null && e.PropertyName == "Time")
            {
                if (_minimumTimePicker.Time.HasValue)
                {
                    _timePicker.MinimumTime = _minimumTimePicker.Time.Value;
                }
			}
        }

        /// <summary>
        /// Method for handle the selection changed event of time picker.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event args.</param>
        void TimePicker_SelectionChanged(object? sender, TimePickerSelectionChangedEventArgs e)
        {
            if (_clearSelectionSwitch != null && e.NewValue != null)
            {
                _previousTime = e.NewValue.Value;
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
            if (_timePicker != null)
            {
                if (e.Value == true)
                {
                    _timePicker.HeaderView.Height = 50;
                    _timePicker.HeaderView.Text = "Select a time";
                }
                else if (e.Value == false)
                {
                    _timePicker.HeaderView.Height = 0;
                }
            }
        }

        /// <summary>
        /// Method for show column header switch toggle changed.
        /// </summary>
        /// <param name="sender">Return the object.</param>
        /// <param name="e">The Event Arguments.</param>
        void ShowColumnHeaderSwitch_Toggled(object? sender, ToggledEventArgs e)
        {
            if (_timePicker != null)
            {
                if (e.Value == true)
                {
                    _timePicker.ColumnHeaderView = new TimePickerColumnHeaderView()
                    {
                        Height = 40,
                    };
                }
                if (e.Value == false)
                {
                    _timePicker.ColumnHeaderView = new TimePickerColumnHeaderView()
                    {
                        Height = 0,
                    };
                }
            }
        }

        /// <summary>
        /// Method for show footer switch toggle changed.
        /// </summary>
        /// <param name="sender">Return the object.</param>
        /// <param name="e">The Event Arguments.</param>
        void ShowFooterSwitch_Toggled(object? sender, ToggledEventArgs e)
        {
            if (_timePicker != null)
            {
                if (e.Value == true)
                {
                    _timePicker.FooterView.Height = 40;
                }
                else if (e.Value == false)
                {
                    _timePicker.FooterView.Height = 0;
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
            if (_timePicker != null)
            {
                if (e.Value == true)
                {
                    _timePicker.SelectedTime = null;
                }
                else if (e.Value == false)
                {
                    _timePicker.SelectedTime = _previousTime;
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
            if (_timePicker != null)
            {
                if (e.Value == true)
                {
                    _timePicker.EnableLooping = true;
                }
                else if (e.Value == false)
                {
                    _timePicker.EnableLooping = false;
                }
            }
        }

        /// <summary>
        /// The format combo box selection changed event.
        /// </summary>
        /// <param name="sender">Return the object.</param>
        /// <param name="e">The Event Arguments.</param>
        void FormatComboBox_SelectionChanged(object? sender, EventArgs e)
        {
            if (_timePicker == null || _formatComboBox == null)
            {
                return;
            }

            if (sender is Microsoft.Maui.Controls.Picker picker)
            {
                string? format = picker.SelectedItem.ToString();
                switch (format)
                {
                    case "H mm":
                        _timePicker.Format = PickerTimeFormat.H_mm;
                        break;

                    case "H mm ss":
                        _timePicker.Format = PickerTimeFormat.H_mm_ss;
                        break;

                    case "h mm ss tt":
                        _timePicker.Format = PickerTimeFormat.h_mm_ss_tt;
                        break;

                    case "h mm tt":
                        _timePicker.Format = PickerTimeFormat.h_mm_tt;
                        break;

                    case "HH mm":
                        _timePicker.Format = PickerTimeFormat.HH_mm;
                        break;

                    case "HH mm ss":
                        _timePicker.Format = PickerTimeFormat.HH_mm_ss;
                        break;

                    case "HH mm ss fff":
                        _timePicker.Format = PickerTimeFormat.HH_mm_ss_fff;
                        break;

                    case "hh mm ss tt":
                        _timePicker.Format = PickerTimeFormat.hh_mm_ss_tt;
                        break;


                    case "hh mm ss fff tt":
                        _timePicker.Format = PickerTimeFormat.hh_mm_ss_fff_tt;
                        break;

                    case "hh mm tt":
                        _timePicker.Format = PickerTimeFormat.hh_mm_tt;
                        break;

                    case "hh tt":
                        _timePicker.Format = PickerTimeFormat.hh_tt;
                        break;

                    case "ss fff":
                        _timePicker.Format = PickerTimeFormat.ss_fff;
                        break;

                    case "mm ss":
                        _timePicker.Format = PickerTimeFormat.mm_ss;
                        break;

                    case "mm ss fff":
                        _timePicker.Format = PickerTimeFormat.mm_ss_fff;
                        break;

                    case "Default":
                        _timePicker.Format = PickerTimeFormat.Default;
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
            if (_timePicker == null || _textDisplayComboBox == null)
            {
                return;
            }

            if (sender is Microsoft.Maui.Controls.Picker picker)
            {
                string? format = picker.SelectedItem.ToString();
                switch (format)
                {
                    case "Default":
                        _timePicker.TextDisplayMode = PickerTextDisplayMode.Default;
                        break;

                    case "Fade":
                        _timePicker.TextDisplayMode = PickerTextDisplayMode.Fade;
                        break;

                    case "Shrink":
                        _timePicker.TextDisplayMode = PickerTextDisplayMode.Shrink;
                        break;

                    case "FadeAndShrink":
                        _timePicker.TextDisplayMode = PickerTextDisplayMode.FadeAndShrink;
                        break;
                }
            }
        }

        /// <summary>
        /// Begins when the behavior attached to the view 
        /// </summary>
        /// <param name="bindable">bindable value</param>
        protected override void OnDetachingFrom(SampleView bindable)
        {
            base.OnDetachingFrom(bindable);
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

            if (_minimumTimePicker != null)
            {
                _minimumTimePicker.PropertyChanged -= MinimumTimePicker_PropertyChanged;
                _minimumTimePicker = null;
            }

            if (_maximumTimePicker != null)
            {
                _maximumTimePicker.PropertyChanged -= MaximumTimePicker_PropertyChanged;
                _maximumTimePicker = null;
            }
        }
    }
}