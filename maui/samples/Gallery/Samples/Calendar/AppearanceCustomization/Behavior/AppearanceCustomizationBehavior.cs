using Syncfusion.Maui.Toolkit.Calendar;

namespace Syncfusion.Maui.ControlsGallery.Calendar.Calendar
{
    /// <summary>
    /// Represents a class which contains appearance customization.
    /// </summary>
    internal class AppearanceCustomizationBehavior : Behavior<SampleView>
    {
        /// <summary>
        /// Calendar view 
        /// </summary>
        SfCalendar? _calendar;

        /// <summary>
        /// The combo box that allows users to choose to whether to select date or a range.
        /// </summary>
        Microsoft.Maui.Controls.Picker? _comboBox;

        /// <summary>
        /// Check the application theme is light or dark.
        /// </summary>
        readonly bool _isLightTheme = Application.Current?.RequestedTheme == AppTheme.Light;

        /// <summary>
        /// Begins when the behavior attached to the view 
        /// </summary>
        /// <param name="bindable">bindable value</param>
        protected override void OnAttachedTo(SampleView bindable)
        {
            Border border = bindable.Content.FindByName<Border>("border");
            border.IsVisible = true;
            border.Stroke = _isLightTheme ? Color.FromRgba("#CAC4D0") : Color.FromRgba("#49454F");
            _calendar = bindable.Content.FindByName<SfCalendar>("iOSCircleCalendar");
            _calendar.SelectionBackground = _isLightTheme ? Color.FromRgba("#6750A4").WithAlpha(0.5f) : Color.FromRgba("#D0BCFF").WithAlpha(0.5f);
            _calendar.TodayHighlightBrush = _isLightTheme ? Color.FromRgba("#6750A4") : Color.FromRgba("#D0BCFF");
            _calendar.SelectionShape = CalendarSelectionShape.Circle;
            _comboBox = bindable.Content.FindByName<Microsoft.Maui.Controls.Picker>("comboBox");
            _comboBox.ItemsSource = new List<string>() { "Circle", "Rectangle" };
            _comboBox.SelectedIndex = 0;
            _comboBox.SelectedIndexChanged += ComboBox_SelectionChanged;
        }

        /// <summary>
        /// Begins when the behavior attached to the view 
        /// </summary>
        /// <param name="bindable">bindable value</param>
        protected override void OnDetachingFrom(SampleView bindable)
        {
            base.OnDetachingFrom(bindable);
            if (_comboBox != null)
            {
                _comboBox.SelectedIndexChanged -= ComboBox_SelectionChanged;
                _comboBox = null;
            }
        }

        /// <summary>
        /// Method for Combo box selection type changed.
        /// </summary>
        /// <param name="sender">Return the object</param>
        /// <param name="e">Event Arguments</param>
        void ComboBox_SelectionChanged(object? sender, EventArgs e)
        {
            if (_calendar != null && sender is Microsoft.Maui.Controls.Picker picker && picker.SelectedItem is string selectionShape)
            {
                if (_calendar.BindingContext is AppearanceViewModel)
                {
                    AppearanceViewModel appearanceViewModel = (AppearanceViewModel)_calendar.BindingContext;
                    bool isCircleShape = selectionShape == "Circle";
					appearanceViewModel.UpdateSelectionShape(isCircleShape);
                    if (isCircleShape)
                    {
                        _calendar.SelectionShape = CalendarSelectionShape.Circle;
                    }
                    else
                    {
                        _calendar.SelectionShape = CalendarSelectionShape.Rectangle;
                    }
                }
            }
        }
    }
}