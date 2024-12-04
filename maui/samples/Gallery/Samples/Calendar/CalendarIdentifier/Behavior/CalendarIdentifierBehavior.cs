using Syncfusion.Maui.Toolkit.Calendar;

namespace Syncfusion.Maui.ControlsGallery.Calendar.Calendar
{
    internal class CalendarIdentifierBehavior : Behavior<SampleView>
    {
        /// <summary>
        /// Calendar view 
        /// </summary>
        SfCalendar? _calendar;

        /// <summary>
        /// The radio buttons collection.
        /// </summary>
        IEnumerable<RadioButton>? _radioButtons;

        /// <summary>
        /// Begins when the behavior attached to the view 
        /// </summary>
        /// <param name="bindable">bindable value</param>
        protected override void OnAttachedTo(SampleView bindable)
        {
            base.OnAttachedTo(bindable);
#if MACCATALYST
            HorizontalStackLayout desktopLayout = bindable.Content.FindByName<HorizontalStackLayout>("desktopCalendar");
            desktopLayout.IsVisible = true;
            Border border = bindable.Content.FindByName<Border>("desktopBorder");
            border.IsVisible = true;
            _calendar = bindable.Content.FindByName<SfCalendar>("desktopIdentifier1");
            Grid optionView = bindable.Content.FindByName<Grid>("desktopOptionView");
            _radioButtons = optionView.Children.OfType<RadioButton>();
#elif IOS
            Grid mobileLayout = bindable.Content.FindByName<Grid>("mobileCalendar");
            mobileLayout.IsVisible = true;
            Border border = bindable.Content.FindByName<Border>("mobileBorder");
            border.IsVisible = true;
            _calendar = bindable.Content.FindByName<SfCalendar>("mobileIdentifier1");
            Border optionBorder = bindable.Content.FindByName<Border>("mobileOptionBorder");
            optionBorder.IsVisible = true;
            HorizontalStackLayout optionView = bindable.Content.FindByName<HorizontalStackLayout>("mobileOptionBorderView");
            _radioButtons = optionView.Children.OfType<RadioButton>();
#elif ANDROID
            Grid mobileLayout = bindable.Content.FindByName<Grid>("mobileCalendar");
            mobileLayout.IsVisible = true;
            Border frame = bindable.Content.FindByName<Border>("mobileFrame");
            frame.IsVisible = true;
            _calendar = bindable.Content.FindByName<SfCalendar>("mobileIdentifier");
            Border optionFrame = bindable.Content.FindByName<Border>("mobileOptionFrame");
            optionFrame.IsVisible = true;
            HorizontalStackLayout optionView = bindable.Content.FindByName<HorizontalStackLayout>("mobileOptionFrameView");
            _radioButtons = optionView.Children.OfType<RadioButton>();
#else
            HorizontalStackLayout desktopLayout = bindable.Content.FindByName<HorizontalStackLayout>("desktopCalendar");
            desktopLayout.IsVisible = true;
            Border frame = bindable.Content.FindByName<Border>("desktopFrame");
            frame.IsVisible = true;
            _calendar = bindable.Content.FindByName<SfCalendar>("desktopIdentifier");
            Grid optionView = bindable.Content.FindByName<Grid>("desktopOptionView");
            _radioButtons = optionView.Children.OfType<RadioButton>();
#endif

            if (_radioButtons != null)
            {
                foreach (var item in _radioButtons)
                {
                    string? radioButtonText = item.Content.ToString();
                    //// Handle the is checked on xaml does not rendered correctly on windows.
                    if (_calendar != null && string.Equals(radioButtonText, "Hijri", StringComparison.Ordinal))
                    {
                        item.IsChecked = true;
                    }

                    item.CheckedChanged += OnRadioButton_StateChanged;
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
            if (_radioButtons != null)
            {
                foreach (var item in _radioButtons)
                {
                    item.CheckedChanged -= OnRadioButton_StateChanged;
                }

                _radioButtons = null;
            }
        }

        /// <summary>
        /// Method for Radio button selection type changed.
        /// </summary>
        /// <param name="sender">Return the object</param>
        /// <param name="e">Event Arguments</param>
        void OnRadioButton_StateChanged(object? sender, CheckedChangedEventArgs e)
        {
            string? radioButtonText = (sender as RadioButton)?.Content.ToString();
            if (_calendar == null)
            {
                return;
            }

            _calendar.MonthView.HeaderView.TextFormat = "ddddd";
            if (string.Equals(radioButtonText, "Gregorian", StringComparison.Ordinal))
            {
                _calendar.Identifier = Syncfusion.Maui.Toolkit.Calendar.CalendarIdentifier.Gregorian;
            }
            else if (string.Equals(radioButtonText, "Hijri", StringComparison.Ordinal))
            {
                _calendar.Identifier = Syncfusion.Maui.Toolkit.Calendar.CalendarIdentifier.Hijri;
            }
            else if (string.Equals(radioButtonText, "Korean", StringComparison.Ordinal))
            {
                _calendar.Identifier = Syncfusion.Maui.Toolkit.Calendar.CalendarIdentifier.Korean;
            }
            else if (string.Equals(radioButtonText, "Persian", StringComparison.Ordinal))
            {
                _calendar.Identifier = Syncfusion.Maui.Toolkit.Calendar.CalendarIdentifier.Persian;
            }
            else if (string.Equals(radioButtonText, "Taiwan", StringComparison.Ordinal))
            {
                _calendar.Identifier = Syncfusion.Maui.Toolkit.Calendar.CalendarIdentifier.Taiwan;
            }
            else if (string.Equals(radioButtonText, "Thai Buddhist", StringComparison.Ordinal))
            {
                _calendar.Identifier = Syncfusion.Maui.Toolkit.Calendar.CalendarIdentifier.ThaiBuddhist;
                _calendar.MonthView.HeaderView.TextFormat = "ddd";
            }
            else if (string.Equals(radioButtonText, "UmAlQura", StringComparison.Ordinal))
            {
                _calendar.Identifier = Syncfusion.Maui.Toolkit.Calendar.CalendarIdentifier.UmAlQura;
            }
        }
    }
}