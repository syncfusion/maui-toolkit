using Syncfusion.Maui.Toolkit.Picker;
using System.Collections.ObjectModel;

namespace Syncfusion.Maui.ControlsGallery.Picker.SfPicker
{
    public class GettingStartedBehavior : Behavior<SampleView>
    {
        /// <summary>
        /// Picker view.
        /// </summary>
        Syncfusion.Maui.Toolkit.Picker.SfPicker? _picker;

        /// <summary>
        /// The show header switch
        /// </summary>
        Switch? _showHeaderSwitch, _showFooterSwitch, _showEnableLoopingSwitch;

        /// <summary>
        /// The text display mode combo box.
        /// </summary>
        Microsoft.Maui.Controls.Picker? _textDisplayComboBox;

        /// <summary>
        /// The text display to set the combo box item source.
        /// </summary>
        ObservableCollection<object>? _textDisplay;

        /// <summary>
        /// Check the application theme is light or dark.
        /// </summary>
        readonly bool _isLightTheme = Application.Current?.RequestedTheme == AppTheme.Light;

        /// <summary>
        /// Begins when the behavior attached to the view 
        /// </summary>
        /// <param name="sampleView">bindable value</param>
        protected override void OnAttachedTo(SampleView sampleView)
        {
            base.OnAttachedTo(sampleView);

#if MACCATALYST
            Border border = sampleView.Content.FindByName<Border>("border");
            border.IsVisible = true;
            border.Stroke = Colors.Transparent;
            _picker = sampleView.Content.FindByName<Syncfusion.Maui.Toolkit.Picker.SfPicker>("Picker1");
#elif IOS
            Border border = sampleView.Content.FindByName<Border>("border1");
            border.IsVisible = true;
            border.Stroke = Colors.Transparent;
            _picker = sampleView.Content.FindByName<Syncfusion.Maui.Toolkit.Picker.SfPicker>("Picker3");
#elif ANDROID
            Border frame = sampleView.Content.FindByName<Border>("frame1");
            frame.IsVisible = true;
            frame.Stroke = Colors.Transparent;
            _picker = sampleView.Content.FindByName<Syncfusion.Maui.Toolkit.Picker.SfPicker>("Picker2");
#else
            Border frame = sampleView.Content.FindByName<Border>("frame");
            frame.IsVisible = true;
            frame.Stroke = Colors.Transparent;
            _picker = sampleView.Content.FindByName<Syncfusion.Maui.Toolkit.Picker.SfPicker>("Picker");
#endif
            _picker.HeaderView.Height = 50;
            _picker.HeaderView.Text = "Select a Country";
            _picker.SelectionView.Padding = 0;
            _picker.SelectionView.CornerRadius = 0;
            _picker.SelectedTextStyle.TextColor = _isLightTheme ? Color.FromArgb("#FFFFFF") : Color.FromArgb("#381E72");

            _showHeaderSwitch = sampleView.Content.FindByName<Switch>("showHeaderSwitch");
            _showFooterSwitch = sampleView.Content.FindByName<Switch>("showFooterSwitch");
            _showEnableLoopingSwitch = sampleView.Content.FindByName<Switch>("enableLoopingSwitch");
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

            if (_showFooterSwitch != null)
            {
                _showFooterSwitch.Toggled += ShowFooterSwitch_Toggled;
            }

            if (_showEnableLoopingSwitch != null)
            {
                _showEnableLoopingSwitch.Toggled += EnableLoopingSwitch_Toggled;
            }
        }

        /// <summary>
        /// The text display combo box selection changed event.
        /// </summary>
        /// <param name="sender">Return the object.</param>
        /// <param name="e">The Event Arguments.</param>
        void TextdisplayComboBox_SelectionChanged(object? sender, EventArgs e)
        {
            if (_picker == null || _textDisplayComboBox == null)
            {
                return;
            }

            if (sender is Microsoft.Maui.Controls.Picker picker)
            {
                string? format = picker.SelectedItem.ToString();
                switch (format)
                {
                    case "Default":
                        _picker.TextDisplayMode = PickerTextDisplayMode.Default;
                        break;

                    case "Fade":
                        _picker.TextDisplayMode = PickerTextDisplayMode.Fade;
                        break;

                    case "Shrink":
                        _picker.TextDisplayMode = PickerTextDisplayMode.Shrink;
                        break;

                    case "FadeAndShrink":
                        _picker.TextDisplayMode = PickerTextDisplayMode.FadeAndShrink;
                        break;
                }
            }
        }

        /// <summary>
        /// Method for show header switch toggle changed.
        /// </summary>
        /// <param name="sender">Return the object.</param>
        /// <param name="e">The Event Arguments.</param>
        void ShowHeaderSwitch_Toggled(object? sender, ToggledEventArgs e)
        {
            if (_picker != null)
            {
                if (e.Value == true)
                {
                    _picker.HeaderView.Height = 50;
                    _picker.HeaderView.Text = "Select a Country";
                }
                else if (e.Value == false)
                {
                    _picker.HeaderView.Height = 0;
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
            if (_picker != null)
            {
                _picker.ColumnHeaderView.Height = e.Value == true ? 40 : 0;
            }
        }

        /// <summary>
        /// Method for show footer switch toggle changed.
        /// </summary>
        /// <param name="sender">Return the object.</param>
        /// <param name="e">The Event Arguments.</param>
        void ShowFooterSwitch_Toggled(object? sender, ToggledEventArgs e)
        {
            if (_picker != null)
            {
                if (e.Value == true)
                {
                    _picker.FooterView.Height = 40;
                }
                else if (e.Value == false)
                {
                    _picker.FooterView.Height = 0;
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
            if (_picker != null)
            {
                if (e.Value == true)
                {
                    _picker.EnableLooping = true;
                }
                else if (e.Value == false)
                {
                    _picker.EnableLooping = false;
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

            if (_showFooterSwitch != null)
            {
                _showFooterSwitch.Toggled -= ShowFooterSwitch_Toggled;
                _showFooterSwitch = null;
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