using Syncfusion.Maui.Toolkit.SunburstChart;
using MAUIPicker = Microsoft.Maui.Controls.Picker;

namespace Syncfusion.Maui.ControlsGallery.SunburstChart.SfSunburstChart;

public partial class Selection : SampleView
{
	public Selection()
	{
		InitializeComponent();
        InitializeProperties();
    }
    private void InitializeProperties()
    {
        selectionFill.SelectedIndex = 0;
        selectionStroke.SelectedIndex = 0;
        selectionType.SelectedIndex = 0;
        selectionDisplayMode.SelectedIndex = 0;
        strokeWidth.Value = 3;
        opacity.Value = 0.7;
    }

    public override void OnDisappearing()
    {
        base.OnDisappearing();

        sunburstChart.Handler?.DisconnectHandler();
    }

    private void selectionType_SelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (MAUIPicker)sender;
        int selectedIndex = picker.SelectedIndex;

        if (selectedIndex == 0)
        {
            selection.Type = SunburstSelectionType.Child;
        }
        else if (selectedIndex == 1)
        {
            selection.Type = SunburstSelectionType.Parent;
        }
        else if (selectedIndex == 2)
        {
            selection.Type = SunburstSelectionType.Single;
        }
        else if (selectedIndex == 3)
        {
            selection.Type = SunburstSelectionType.Group;
        }
    }

    private void selectionDisplayMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (MAUIPicker)sender;
        int selectedIndex = picker.SelectedIndex;

        if (selectedIndex == 0)
        {
            selection.DisplayMode = SunburstSelectionDisplayMode.HighlightByBrush;
        }
        else if (selectedIndex == 1)
        {
            selection.DisplayMode = SunburstSelectionDisplayMode.HighlightByOpacity;
        }
        else if (selectedIndex == 2)
        {
            selection.DisplayMode = SunburstSelectionDisplayMode.HighlightByStroke;
        }
    }

    private void selectionFill_SelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (MAUIPicker)sender;
        int selectedIndex = picker.SelectedIndex;

        if (selectedIndex == 0)
        {
            selection.Fill = new SolidColorBrush(Color.FromArgb("#D1007B"));
        }
        else if (selectedIndex == 1)
        {
            selection.Fill = new SolidColorBrush(Color.FromArgb("#DEBB04"));
        }
        else if (selectedIndex == 2)
        {
            selection.Fill = new SolidColorBrush(Color.FromArgb("#E0573E"));
        }
    }

    private void selectionStroke_SelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (MAUIPicker)sender;
        int selectedIndex = picker.SelectedIndex;

        if (selectedIndex == 0)
        {
            selection.Stroke = Colors.Black;
        }
        else if (selectedIndex == 1)
        {
            selection.Stroke = new SolidColorBrush(Color.FromArgb("#E8C304"));
		}
        else if (selectedIndex == 2)
        {
            selection.Stroke = Colors.Turquoise;
        }
    }

    private void selection_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        var slider = (Slider)sender;
        if (selectionViewModel != null)
        {
            var value = slider.StyleId;
            switch (value)
            {
                case "strokeWidth":
                    selection.StrokeWidth = slider.Value;
                    break;
                case "opacity":
                    selection.Opacity = slider.Value;
                    break;
            }
        }
    }
}