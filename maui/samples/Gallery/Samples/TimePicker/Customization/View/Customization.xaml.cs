namespace Syncfusion.Maui.ControlsGallery.Picker.SfTimePicker
{
    public partial class Customization : SampleView
    {
        /// <summary>
        /// Check the application theme is light or dark.
        /// </summary>
        readonly bool _isLightTheme = Application.Current?.RequestedTheme == AppTheme.Light;

        /// <summary>
        /// The alarm details.
        /// </summary>
        AlarmDetails? _alarmDetails;

        /// <summary>
        /// Initializes a new instance of the <see cref="Customization"/> class.
        /// </summary>
        public Customization()
        {
            InitializeComponent();
#if ANDROID || IOS
            alarmEditPicker1.HeaderView.Height = 40;
            alarmEditPicker1.HeaderView.Text = "Edit Alarm";
            alarmEditPicker1.FooterView.Height = 40;
            alarmEditPicker1.FooterView.OkButtonText = "Save";
#else
            alarmEditPicker.HeaderView.Height = 40;
            alarmEditPicker.HeaderView.Text = "Edit Alarm";
            alarmEditPicker.FooterView.Height = 40;
            alarmEditPicker.FooterView.OkButtonText = "Save";
#endif
        }

        /// <summary>
        /// Invoked when an alarm is tapped.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event args.</param>
        void OnAlarmTapped(object sender, EventArgs e)
        {
#if ANDROID || IOS
            if (sender is Border border && border.BindingContext != null && border.BindingContext is AlarmDetails alarmDetails)
            {
                if (alarmDetails.IsAlarmEnabled)
                {
                    alarmEditPicker1.SelectedTime = alarmDetails.AlarmTime;
                    _alarmDetails = alarmDetails;
                    alarmEditPicker1.IsOpen = true;
                }
            }
#else
            if (sender is Border border && border.BindingContext != null && border.BindingContext is AlarmDetails alarmDetails)
            {
                if (alarmDetails.IsAlarmEnabled)
                {
                    alarmEditPicker.SelectedTime = alarmDetails.AlarmTime;
                    _alarmDetails = alarmDetails;
                    alarmEditPicker.IsOpen = true;
                }
            }
#endif
        }

        /// <summary>
        /// Invoked when the Ok button is clicked.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event args.</param>
        void AlarmEditPicker_OkButtonClicked(object? sender, EventArgs e)
        {
            if (sender is Syncfusion.Maui.Toolkit.Picker.SfTimePicker picker && _alarmDetails != null)
            {
                if (picker.SelectedTime != null && _alarmDetails.AlarmTime != picker.SelectedTime)
                {
                    _alarmDetails.AlarmTime = picker.SelectedTime.Value;
                }

                _alarmDetails = null;
            }

#if ANDROID || IOS
            alarmEditPicker1.IsOpen = false;
#else
            alarmEditPicker.IsOpen = false;
#endif
        }

        /// <summary>
        /// Invoked when the Cancel button is clicked.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event args.</param>
        void alarmEditPicker_CancelButtonClicked(object sender, EventArgs e)
        {
#if ANDROID || IOS
            alarmEditPicker1.IsOpen = false;
#else
            alarmEditPicker.IsOpen = false;
#endif
        }

        /// <summary>
        /// Invoked when the alarm switch is toggled.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event args.</param>
        void alarmSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            if (sender is Switch toggleSwitch && toggleSwitch.BindingContext != null && toggleSwitch.BindingContext is AlarmDetails alarmDetails)
            {
                if (e.Value)
                {
                    alarmDetails.AlarmTextColor = _isLightTheme ? Colors.Black : Colors.White;
                    alarmDetails.AlarmSecondaryTextColor = _isLightTheme ? Color.FromArgb("#49454F") : Color.FromArgb("#CAC4D0");
                }
                else
                {
                    alarmDetails.AlarmTextColor = _isLightTheme ? Colors.Black.WithAlpha(0.5f) : Colors.White.WithAlpha(0.5f);
                    alarmDetails.AlarmSecondaryTextColor = _isLightTheme ? Color.FromArgb("#49454F").WithAlpha(0.5f) : Color.FromArgb("#CAC4D0").WithAlpha(0.5f);
                }
            }

        }

        /// <summary>
        /// Invoked when the Add Alarm button is clicked.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event args.</param>
        void OnAddAlarmTapped(object sender, EventArgs e)
        {
#if ANDROID || IOS
            alarmPopup1.IsOpen = true;
#else
            alarmPopup.IsOpen = true;
#endif
        }

        /// <summary>
        /// Invoked when the alarm popup is created.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event args.</param>
        void alarmPopup_OnCreated(object sender, EventArgs e)
        {
            if (BindingContext != null && BindingContext is ViewModel bindingContext && sender is AlarmDetails details)
            {
                bindingContext.AlarmCollection.Add(details);
            }
        }
    }
}