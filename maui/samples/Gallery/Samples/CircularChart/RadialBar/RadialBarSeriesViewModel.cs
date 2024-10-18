using Syncfusion.Maui.Toolkit.Charts;
using System.Collections.ObjectModel;

namespace Syncfusion.Maui.ControlsGallery.CircularChart.SfCircularChart
{
    public class RadialBarSeriesViewModel : BaseViewModel
    {
        public ObservableCollection<ChartDataModel> RadialBarSeriesData { get; set; }
        public ObservableCollection<ChartDataModel> RadialBarData { get; set; }
        public List<Brush> DefaultPalette { get; set; }
        public List<Brush> CustomPalette { get; set; }
        public List<string> Track { get; set; }
        public Array CapStyles {
            get {
                return Enum.GetValues(typeof(CapStyle));
            }
        }

        public RadialBarSeriesViewModel() 
        {
            RadialBarSeriesData = new ObservableCollection<ChartDataModel>()
           {
                new ChartDataModel("Labor", 10, Color.FromArgb("#00bdae"),0.22),
                new ChartDataModel("Production", 11,Color.FromArgb("#404041"),0.23),
                new ChartDataModel("Facilities", 12, Color.FromArgb("#357cd2"),0.27),
                new ChartDataModel("Insurance", 13,Color.FromArgb("#e56590"),0.28)
            };

            RadialBarData = new ObservableCollection<ChartDataModel>()
            {
                new ChartDataModel("Vehicle",62.70,"toy_car.png"),
                new ChartDataModel("Education",29.50,"chart_book.png"),
                new ChartDataModel("Personal",45.60,"savings.png"),
                new ChartDataModel("Home",85.20,"house_icon.png"),
            };

            DefaultPalette = new List<Brush>()
            {
                new SolidColorBrush(Color.FromRgb(248, 177, 149)),
                new SolidColorBrush(Color.FromRgb(246, 114, 128)),
                new SolidColorBrush(Color.FromRgb(61, 205, 171)),
                new SolidColorBrush(Color.FromRgb(1, 174, 190)),
                new SolidColorBrush(Color.FromRgb(116, 180, 155)),
            };

            CustomPalette = new List<Brush>()
            {
                new SolidColorBrush(Color.FromArgb("#47ba9f")),
                new SolidColorBrush(Color.FromArgb("#e58870")),
                new SolidColorBrush(Color.FromArgb("#9686c9")),
                new SolidColorBrush(Color.FromArgb("#e56590")),
            };

            Track = new List<string>()
            {
                "Select a color",
                "Light Gray",
                "Blue Gray",
                "Pale Blue",
                "Light Coral",
                "Pale Violet"
            };
        }
    }
}
