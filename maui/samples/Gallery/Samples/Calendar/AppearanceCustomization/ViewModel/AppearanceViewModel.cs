using Microsoft.Maui.Controls.Shapes;
using Syncfusion.Maui.Toolkit.Calendar;
using System.ComponentModel;
using System.Globalization;
using System.Xml;

namespace Syncfusion.Maui.ControlsGallery.Calendar.Calendar;

public class AppearanceViewModel : INotifyPropertyChanged
{
    /// <summary>
    /// Check the application theme is light or dark.
    /// </summary>
    readonly bool _isLightTheme = Application.Current?.RequestedTheme == AppTheme.Light;

    readonly DataTemplate _circleTemplate;

    readonly DataTemplate _rectTemplate;

    DataTemplate _template;

    public event PropertyChangedEventHandler? PropertyChanged;

    public DataTemplate Template
    {
        get
        {
            return _template;
        }
        set
        {
            _template = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Template)));
        }
    }

    public AppearanceViewModel()
    {
        _circleTemplate = new DataTemplate(() =>
        {
            Grid grid = new Grid();

            Border border = new Border();
            border.BackgroundColor = _isLightTheme ? Color.FromRgba("#EEE8F4") : Color.FromRgba("#302D38");
            border.StrokeShape = new RoundRectangle()
            {
                CornerRadius = new CornerRadius(25)
            };

            border.SetBinding(Border.StrokeThicknessProperty, "Date", converter: new DateToStrokeConverter());
            border.Stroke = _isLightTheme ? Color.FromRgba("#6750A4") : Color.FromRgba("#D0BCFF");

            Label label = new Label();
            label.SetBinding(Label.TextProperty, BindingHelper.CreateBinding(
   propertyName: "Date.Day",
   getter: static (CalendarCellDetails context) => context.Date.Day));

            label.HorizontalOptions = LayoutOptions.Center;
            label.VerticalOptions = LayoutOptions.Center;
            label.Padding = new Thickness(2);
            border.Content = label;

            grid.Add(border);
            grid.Padding = new Thickness(2);

            return grid;
        });

        _rectTemplate = new DataTemplate(() =>
        {
            Grid grid = new Grid();

            Border border = new Border();
            border.BackgroundColor = _isLightTheme ? Color.FromRgba("#EEE8F4") : Color.FromRgba("#302D38");
            border.StrokeShape = new RoundRectangle()
            {
                CornerRadius = new CornerRadius(2)
            };

            border.SetBinding(Border.StrokeThicknessProperty, "Date", converter: new DateToStrokeConverter());
            border.Stroke = _isLightTheme ? Color.FromRgba("#6750A4") : Color.FromRgba("#D0BCFF");

            Label label = new Label();
            label.SetBinding(Label.TextProperty, BindingHelper.CreateBinding(
propertyName: "Date.Day",
getter: static (CalendarCellDetails context) => context.Date.Day));

            label.HorizontalOptions = LayoutOptions.Center;
            label.VerticalOptions = LayoutOptions.Center;
            border.Content = label;

            grid.Add(border);
            grid.Padding = new Thickness(2);

            return grid;
        });

        _template = _circleTemplate;
    }

    public void UpdateSelectionShape(bool isCircleShape)
    {
        Template = isCircleShape ? _circleTemplate : _rectTemplate;
    }
}

internal class DateToStrokeConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var date = value as DateTime?;
        if (date.HasValue && date.Value.Date == DateTime.Now.Date)
        {
            return 1;
        }

        return 0;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return null;
    }
}
