using Syncfusion.Maui.Toolkit.Picker;
using System.Collections.ObjectModel;

namespace Syncfusion.Maui.ControlsGallery.Picker.SfDatePicker
{
    public class GettingStartedBehavior : Behavior<SampleView>
    {
        /// <summary>
        /// Picker view.
        /// </summary>
        Syncfusion.Maui.Toolkit.Picker.SfDatePicker? _datePicker;

        /// <summary>
        /// The show header switch.
        /// </summary>
        Switch? _showHeaderSwitch, _showColumnHeaderSwitch, _showFooterSwitch, _clearSelectionSwitch;

        /// <summary>
        /// The date format combo box.
        /// </summary>
        Microsoft.Maui.Controls.Picker? _formatComboBox, _textDisplayComboBox;

        /// <summary>
        /// The date format to set the combo box item source.
        /// </summary>
        ObservableCollection<object>? _formats;

        /// <summary>
        /// The text display to set the combo box item source.
        /// </summary>
        ObservableCollection<object>? _textDisplay;

        /// <summary>
        /// Check the application theme is light or dark.
        /// </summary>
        readonly bool _isLightTheme = Application.Current?.RequestedTheme == AppTheme.Light;

        /// <summary>
        /// Get the previous selected date from date picker.
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
            _datePicker = sampleView.Content.FindByName<Syncfusion.Maui.Toolkit.Picker.SfDatePicker>("DatePicker1");
#else
            Border frame = sampleView.Content.FindByName<Border>("frame");
            frame.IsVisible = true;
            frame.Stroke = _isLightTheme ? Color.FromArgb("#CAC4D0") : Color.FromArgb("#49454F");
            _datePicker = sampleView.Content.FindByName<Syncfusion.Maui.Toolkit.Picker.SfDatePicker>("DatePicker");
#endif

            _previousDate = _datePicker.SelectedDate;
            _datePicker.HeaderView.Height = 50;
            _datePicker.HeaderView.Text = "Select a Date";
            _datePicker.BlackoutDates = new ObservableCollection<DateTime>()
            {
                DateTime.Now.AddDays(2),
                DateTime.Now.AddDays(5),
                DateTime.Now.AddDays(8),
                DateTime.Now.AddDays(-3),
                DateTime.Now.AddDays(-4),
                DateTime.Now.AddDays(-8),
                DateTime.Now.AddMonths(1).AddDays(1),
                DateTime.Now.AddMonths(1).AddDays(3),
                DateTime.Now.AddMonths(1).AddDays(6),
                DateTime.Now.AddMonths(1).AddDays(-2),
                DateTime.Now.AddMonths(1).AddDays(-5),
                DateTime.Now.AddMonths(1).AddDays(-6),
                DateTime.Now.AddMonths(-1).AddDays(2),
                DateTime.Now.AddMonths(-1).AddDays(5),
                DateTime.Now.AddMonths(-1).AddDays(1),
                DateTime.Now.AddMonths(-1).AddDays(-7),
                DateTime.Now.AddMonths(-1).AddDays(-1),
                DateTime.Now.AddMonths(-1).AddDays(-6),
            };

            _datePicker.SelectionChanged += DatePicker_SelectionChanged;
            _showHeaderSwitch = sampleView.Content.FindByName<Switch>("showHeaderSwitch");
            _showColumnHeaderSwitch = sampleView.Content.FindByName<Switch>("showColumnHeaderSwitch");
            _showFooterSwitch = sampleView.Content.FindByName<Switch>("showFooterSwitch");
            _clearSelectionSwitch = sampleView.Content.FindByName<Switch>("clearSelectionSwitch");
            _datePicker.SelectedTextStyle.TextColor = _isLightTheme ? Color.FromArgb("#FFFFFF") : Color.FromArgb("#381E72");

            _formats = new ObservableCollection<object>()
            {
                 "dd MM", "dd MM yyyy", "dd MMM yyyy", "M d yyyy", "MM dd yyyy", "MM yyyy", "MMM yyyy", "yyyy MM dd", "Default"
            };

            _formatComboBox = sampleView.Content.FindByName<Microsoft.Maui.Controls.Picker>("formatComboBox");
            _formatComboBox.ItemsSource = _formats;
            _formatComboBox.SelectedIndex = 1;
            _formatComboBox.SelectedIndexChanged += FormatComboBox_SelectionChanged;

            _textDisplay = new ObservableCollection<object>()
            {
                "Default", "Fade", "Shrink", "FadeAndShrink"
            };

            _textDisplayComboBox = sampleView.Content.FindByName<Microsoft.Maui.Controls.Picker>("textDisplayComboBox");
            _textDisplayComboBox.ItemsSource = _textDisplay;
            _textDisplayComboBox.SelectedIndex = 0;
            _textDisplayComboBox.SelectedIndexChanged += TextDisplayComboBox_SelectionChanged;

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
        }

        /// <summary>
        /// Method for date picker selection changed.
        /// </summary>
        /// <param name="sender">Return the object.</param>
        /// <param name="e">The Event Arguments.</param>
        void DatePicker_SelectionChanged(object? sender, DatePickerSelectionChangedEventArgs e)
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
            if (_datePicker != null)
            {
                if (e.Value == true)
                {
                    _datePicker.HeaderView.Height = 50;
                    _datePicker.HeaderView.Text = "Select a Date";
                }
                else if (e.Value == false)
                {
                    _datePicker.HeaderView.Height = 0;
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
            if (_datePicker != null)
            {
                if (e.Value == true)
                {
                    _datePicker.ColumnHeaderView = new DatePickerColumnHeaderView()
                    {
                        Height = 40,
                        DayHeaderText = "Day",
                        MonthHeaderText = "Month",
                        YearHeaderText = "Year",
                    };
                }
                if (e.Value == false)
                {
                    _datePicker.ColumnHeaderView = new DatePickerColumnHeaderView()
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
            if (_datePicker != null)
            {
                if (e.Value == true)
                {
                    _datePicker.FooterView.Height = 40;
                }
                else if (e.Value == false)
                {
                    _datePicker.FooterView.Height = 0;
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
            if (_datePicker != null)
            {
                if (e.Value == true)
                {
                    _datePicker.SelectedDate = null;
                }
                else if (e.Value == false)
                {
                    _datePicker.SelectedDate = _previousDate;
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
            if (_datePicker == null || _formatComboBox == null)
            {
                return;
            }

            if (sender is Microsoft.Maui.Controls.Picker picker)
            {
                string? format = picker.SelectedItem.ToString();
                switch (format)
                {
                    case "dd MM":
                        _datePicker.Format = PickerDateFormat.dd_MM;
                        break;

                    case "dd MM yyyy":
                        _datePicker.Format = PickerDateFormat.dd_MM_yyyy;
                        break;

                    case "dd MMM yyyy":
                        _datePicker.Format = PickerDateFormat.dd_MMM_yyyy;
                        break;

                    case "M d yyyy":
                        _datePicker.Format = PickerDateFormat.M_d_yyyy;
                        break;

                    case "MM dd yyyy":
                        _datePicker.Format = PickerDateFormat.MM_dd_yyyy;
                        break;

                    case "MM yyyy":
                        _datePicker.Format = PickerDateFormat.MM_yyyy;
                        break;

                    case "MMM yyyy":
                        _datePicker.Format = PickerDateFormat.MMM_yyyy;
                        break;

                    case "yyyy MM dd":
                        _datePicker.Format = PickerDateFormat.yyyy_MM_dd;
                        break;
                    case "Default":
                        _datePicker.Format = PickerDateFormat.Default;
                        break;
                }
            }
        }

        /// <summary>
        /// The text display combo box selection changed event.
        /// </summary>
        /// <param name="sender">Return the object.</param>
        /// <param name="e">The Event Arguments.</param>
        void TextDisplayComboBox_SelectionChanged(object? sender, EventArgs e)
        {
            if (_datePicker == null || _textDisplayComboBox == null)
            {
                return;
            }

            if (sender is Microsoft.Maui.Controls.Picker picker)
            {
                string? format = picker.SelectedItem.ToString();
                switch (format)
                {
                    case "Default":
                        _datePicker.TextDisplayMode = PickerTextDisplayMode.Default;
                        break;

                    case "Fade":
                        _datePicker.TextDisplayMode = PickerTextDisplayMode.Fade;
                        break;

                    case "Shrink":
                        _datePicker.TextDisplayMode = PickerTextDisplayMode.Shrink;
                        break;

                    case "FadeAndShrink":
                        _datePicker.TextDisplayMode = PickerTextDisplayMode.FadeAndShrink;
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
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="GettingStartedBehavior"/> class.
        /// </summary>
        public GettingStartedBehavior()
        {
        }
    }
}