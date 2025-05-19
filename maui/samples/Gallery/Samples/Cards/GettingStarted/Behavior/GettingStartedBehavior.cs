using Syncfusion.Maui.Toolkit.Cards;

namespace Syncfusion.Maui.ControlsGallery.Cards.SfCards
{
    /// <summary>
    /// Getting started Behavior class
    /// </summary>
    public class GettingStartedBehavior : Behavior<SampleView>
    {
        /// <summary>
        /// Instance of card view.
        /// </summary>
        SfCardView? _firstCard, _secondCard, _thirdCard, _fourthCard, _fifthCard;

        /// <summary>
        /// The corner radius label.
        /// </summary>
        Label? _cornerRadiusLabel;

        /// <summary>
        /// The corner radius slider.
        /// </summary>
        Slider? _cornerRadiusSlider;

        /// <summary>
        /// The indicator switch.
        /// </summary>
        Switch? _indicatorSwitch;

        /// <summary>
        /// The indicator position option.
        /// </summary>
        Grid? _indicatorPositionOption;

        /// <summary>
        /// This combo box is used to choose the indicator position of the cards.
        /// </summary>
        Microsoft.Maui.Controls.Picker? _indicatorPositionComboBox;

        /// <summary>
        /// Begins when the behavior attached to the view 
        /// </summary>
        /// <param name="sampleView">bindable value</param>
        protected override void OnAttachedTo(SampleView sampleView)
        {
            base.OnAttachedTo(sampleView);

            _firstCard = sampleView.Content.FindByName<SfCardView>("firstCard");
            _secondCard = sampleView.Content.FindByName<SfCardView>("secondCard");
            _thirdCard = sampleView.Content.FindByName<SfCardView>("thirdCard");
            _fourthCard = sampleView.Content.FindByName<SfCardView>("fourthCard");
            _fifthCard = sampleView.Content.FindByName<SfCardView>("fifthCard");
            _cornerRadiusLabel = sampleView.Content.FindByName<Label>("cornerRadiusLabel");
            _cornerRadiusSlider = sampleView.Content.FindByName<Slider>("cornerRadiusSlider");
            _indicatorSwitch = sampleView.Content.FindByName<Switch>("indicatorSwitch");
            _indicatorPositionOption = sampleView.Content.FindByName<Grid>("indicatorPositionOption");

            _indicatorPositionComboBox = sampleView.Content.FindByName<Microsoft.Maui.Controls.Picker>("indicatorPositionComboBox");
            _indicatorPositionComboBox.ItemsSource = Enum.GetValues(typeof(CardIndicatorPosition)).Cast<CardIndicatorPosition>().ToList();
            _indicatorPositionComboBox.SelectedIndex = 0;
            _indicatorPositionComboBox.SelectedIndexChanged += IndicatorPositionComboBox_SelectedIndexChanged;

            if (_cornerRadiusSlider != null)
            {
                _cornerRadiusSlider.ValueChanged += CornerRadiusSlider_ValueChanged;
            }

            if (_indicatorSwitch != null)
            {
                _indicatorSwitch.Toggled += IndicatorSwitch_Toggled;
            }
        }

        /// <summary>
        /// Method for Cards Indication thickness changed.
        /// </summary>
        /// <param name="sender">Return the objects.</param>
        /// <param name="e">Event Arguments</param>
        void IndicatorSwitch_Toggled(object? sender, ToggledEventArgs e)
        {
            bool isIndcatorEnabled = e.Value;
            double indicatorThickness = isIndcatorEnabled ? 7 : 0;
            if (_firstCard != null && _secondCard != null && _thirdCard != null && _fourthCard != null && _fifthCard != null && _indicatorPositionOption != null)
            {
                _firstCard.IndicatorThickness = indicatorThickness;
                _secondCard.IndicatorThickness = indicatorThickness;
                _thirdCard.IndicatorThickness = indicatorThickness;
                _fourthCard.IndicatorThickness = indicatorThickness;
                _fifthCard.IndicatorThickness = indicatorThickness;
                if (isIndcatorEnabled)
                {
                    _indicatorPositionOption.IsVisible = true;
                    _firstCard.IndicatorColor = Color.FromArgb("#E2C799");
                    _secondCard.IndicatorColor = Color.FromArgb("#D4BBA0");
                    _thirdCard.IndicatorColor = Color.FromArgb("#A4A8AB");
                    _fourthCard.IndicatorColor = Color.FromArgb("#19376D");
                    _fifthCard.IndicatorColor = Color.FromArgb("#77ABB7");
                }
                else
                {
                    _indicatorPositionOption.IsVisible = false;
                }
            }
        }

        /// <summary>
        /// Method for Cards Indication position combo box changed.
        /// </summary>
        /// <param name="sender">Return the object.</param>
        /// <param name="e">Event Arguments.</param>
        void IndicatorPositionComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (sender is Microsoft.Maui.Controls.Picker picker && _firstCard != null && _secondCard != null && _thirdCard != null && _fourthCard != null && _fifthCard != null)
            {
                string? selectedPosition = picker.SelectedItem.ToString();
                switch (selectedPosition)
                {
                    case "Bottom":
                        _firstCard.IndicatorPosition = CardIndicatorPosition.Bottom;
                        _secondCard.IndicatorPosition = CardIndicatorPosition.Bottom;
                        _thirdCard.IndicatorPosition = CardIndicatorPosition.Bottom;
                        _fourthCard.IndicatorPosition = CardIndicatorPosition.Bottom;
                        _fifthCard.IndicatorPosition = CardIndicatorPosition.Bottom;
                        break;
                    case "Top":
                        _firstCard.IndicatorPosition = CardIndicatorPosition.Top;
                        _secondCard.IndicatorPosition = CardIndicatorPosition.Top;
                        _thirdCard.IndicatorPosition = CardIndicatorPosition.Top;
                        _fourthCard.IndicatorPosition = CardIndicatorPosition.Top;
                        _fifthCard.IndicatorPosition = CardIndicatorPosition.Top;
                        break;
                    case "Left":
                        _firstCard.IndicatorPosition = CardIndicatorPosition.Left;
                        _secondCard.IndicatorPosition = CardIndicatorPosition.Left;
                        _thirdCard.IndicatorPosition = CardIndicatorPosition.Left;
                        _fourthCard.IndicatorPosition = CardIndicatorPosition.Left;
                        _fifthCard.IndicatorPosition = CardIndicatorPosition.Left;
                        break;
                    case "Right":
                        _firstCard.IndicatorPosition = CardIndicatorPosition.Right;
                        _secondCard.IndicatorPosition = CardIndicatorPosition.Right;
                        _thirdCard.IndicatorPosition = CardIndicatorPosition.Right;
                        _fourthCard.IndicatorPosition = CardIndicatorPosition.Right;
                        _fifthCard.IndicatorPosition = CardIndicatorPosition.Right;
                        break;
                }
            }
        }

        /// <summary>
        /// Method to corner radius slider value changed
        /// </summary>
        /// <param name="sender">return the object</param>
        /// <param name="e">Event Arguments</param>
        void CornerRadiusSlider_ValueChanged(object? sender, ValueChangedEventArgs e)
        {
            if (_firstCard != null && _secondCard != null && _thirdCard != null && _fourthCard != null && _fifthCard != null && _cornerRadiusLabel != null)
            {
                _firstCard.CornerRadius = new CornerRadius(Math.Round(e.NewValue));
                _secondCard.CornerRadius = new CornerRadius(Math.Round(e.NewValue));
                _thirdCard.CornerRadius = new CornerRadius(Math.Round(e.NewValue));
                _fourthCard.CornerRadius = new CornerRadius(Math.Round(e.NewValue));
                _fifthCard.CornerRadius = new CornerRadius(Math.Round(e.NewValue));
                _cornerRadiusLabel.Text = "Corner radius: " + Math.Round(e.NewValue);
            }
        }

        /// <summary>
        /// Begins when the behavior attached to the view 
        /// </summary>
        /// <param name="sampleView">bindable value</param>
        protected override void OnDetachingFrom(SampleView sampleView)
        {
            base.OnDetachingFrom(sampleView);
            if (_cornerRadiusSlider != null)
            {
                _cornerRadiusSlider.ValueChanged -= CornerRadiusSlider_ValueChanged;
            }

            if (_indicatorSwitch != null)
            {
                _indicatorSwitch.Toggled -= IndicatorSwitch_Toggled;
            }

            if (_indicatorPositionComboBox != null)
            {
                _indicatorPositionComboBox.SelectedIndexChanged -= IndicatorPositionComboBox_SelectedIndexChanged;
            }
        }
    }
}