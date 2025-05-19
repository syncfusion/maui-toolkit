using Syncfusion.Maui.Toolkit.Popup;
using Syncfusion.Maui.Toolkit.Picker;
using Syncfusion.Maui.Toolkit.TextInputLayout;

namespace Syncfusion.Maui.ControlsGallery.Picker.SfTimePicker
{
    public class AlarmPopup : SfPopup
    {
        /// <summary>
        /// The time picker instance.
        /// </summary>
		readonly Syncfusion.Maui.Toolkit.Picker.SfTimePicker _alarmTimePicker;

        /// <summary>
        /// The alarm message entry instance.
        /// </summary>
        readonly Entry _alarmMessageEntry;

        /// <summary>
        /// The alarm text input instance.
        /// </summary>
        readonly SfTextInputLayout _alarmTextInput;

        /// <summary>
        /// The event handler for the created event.
        /// </summary>
        public event EventHandler? OnCreated;

        /// <summary>
        /// Initializes a new instance of the <see cref="AlarmPopup" /> class.
        /// </summary>
        public AlarmPopup()
        {
            _alarmTimePicker = new Syncfusion.Maui.Toolkit.Picker.SfTimePicker();
            StackLayout stack = new StackLayout();
            Label label = new Label();
            label.Text = "Alarm Message";
            label.Margin = new Thickness(10, 4);
            label.FontSize = 12;
            stack.Add(label);
            _alarmTextInput = new SfTextInputLayout();
            _alarmTextInput.Padding = new Thickness(0);
            _alarmTextInput.Hint = "Alarm Message";
            _alarmTextInput.HelperText = "Enter Alarm Message";
            _alarmMessageEntry = new Entry();
            _alarmMessageEntry.HeightRequest = 40;
            _alarmMessageEntry.Text = string.Empty;
            _alarmMessageEntry.Margin = new Thickness(5, 0);
            _alarmTextInput.Content = _alarmMessageEntry;
            stack.Add(_alarmTextInput);
            Label label1 = new Label();
            label1.Text = "Alarm Time";
            label1.FontSize = 12;
            label1.Margin = new Thickness(10, 5);
            stack.Add(label1);
            _alarmTimePicker.FooterView.Height = 40;
            _alarmTimePicker.HeightRequest = 280;
            _alarmTimePicker.Format = PickerTimeFormat.h_mm_tt;
            _alarmTimePicker.OkButtonClicked += AlarmTimePicker_OkButtonClicked;
            _alarmTimePicker.CancelButtonClicked += AlarmTimePicker_CancelButtonClicked;
            stack.Add(_alarmTimePicker);
            stack.VerticalOptions = LayoutOptions.Center;
            ContentTemplate = new DataTemplate(() =>
            {
                return stack;
            });

            HeaderTemplate = new DataTemplate(() =>
            {
                return new Label() { Text = "Set Alarm", FontSize = 20, HeightRequest = 40, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center };
            });

#if ANDROID || IOS || MACCATALYST
            HeightRequest = 470;
#else
            HeightRequest = 450;
#endif
            WidthRequest = 300;
            ShowFooter = false;
            ShowHeader = true;
            HeaderHeight = 40;
            PopupStyle.CornerRadius = new CornerRadius(5);
        }

        /// <summary>
        /// Invoked when the cancel button is clicked.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event args.</param>
        void AlarmTimePicker_CancelButtonClicked(object? sender, EventArgs e)
        {
            Reset();
            IsOpen = false;
        }

        /// <summary>
        /// Invoked when the ok button is clicked.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event args.</param>
        void AlarmTimePicker_OkButtonClicked(object? sender, EventArgs e)
        {
            if (_alarmTimePicker.SelectedTime != null)
            {
                OnCreated?.Invoke(new AlarmDetails() { AlarmTime = _alarmTimePicker.SelectedTime.Value, AlarmMessage = _alarmMessageEntry.Text == string.Empty ? "No Alarm Message" : _alarmMessageEntry.Text, IsAlarmEnabled = true }, new EventArgs());

            }
            IsOpen = false;
            Reset();
        }

        /// <summary>
        /// Method to reset the alarm popup.
        /// </summary>
        public void Reset()
        {
            _alarmTimePicker.SelectedTime = DateTime.Now.TimeOfDay;
            _alarmMessageEntry.Text = string.Empty;
            _alarmMessageEntry.Placeholder = "Enter Alarm Message";
        }
    }
}