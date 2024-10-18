using System.Collections.ObjectModel;
namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
    public class ErrorBarViewModel:BaseViewModel
    {
        public ObservableCollection<ChartDataModel> EnergyProductions { get; set; }
        public ObservableCollection<ChartDataModel> ThermalExpansion { get; set; }
        public string[] ErrorBarType => new string[] { "Fixed","Percentage","Standard Error","Standard Deviation" };
        public string[] ErrorBarMode => new string[] {  "Vertical", "Horizontal", "Both"};
        public string[] ErrorBarDirection => new string[] { "Both", "Plus", "Minus" };

        public ErrorBarViewModel()
        {
            EnergyProductions = new ObservableCollection<ChartDataModel>()
            {
                new ChartDataModel{Name="IND",Value=24},
                new ChartDataModel{Name="AUS",Value=20},
                new ChartDataModel{Name="USA",Value=35},
                new ChartDataModel{Name="DEU",Value=27},
                new ChartDataModel{Name="ITA",Value=30},
                new ChartDataModel{Name="UK",Value=41},
                new ChartDataModel{Name="RUS",Value=26},
            };

            ThermalExpansion = new ObservableCollection<ChartDataModel>()
            {
                new ChartDataModel{Name="Erbium",Value=8.2,High=3.8},
                new ChartDataModel{Name="Samarium",Value=8.15,High=2.85},
                new ChartDataModel{Name="Yttrium",Value=7.15,High=3.85},
                new ChartDataModel{Name="Carbide",Value=6.45,High=2.95},
                new ChartDataModel{Name="Uranium",Value=7.45,High=3.55},
                new ChartDataModel{Name="Holmium",Value=7.45,High=3.55},
                new ChartDataModel{Name="Thulium",Value=8.45,High=3.55},
                new ChartDataModel{Name="Scandium ",Value=6.35,High=2.15},
                new ChartDataModel{Name="Tin",Value=14.6,High=5.4},
                new ChartDataModel{Name="Gallium",Value=12.2,High=5.8}
            };
        }
    }
}
