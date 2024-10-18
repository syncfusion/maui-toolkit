using System.Collections.ObjectModel;

namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
    public class BarSeriesViewModel : BaseViewModel
    {
        public ObservableCollection<ChartDataModel> BarData1 { get; set; }
        public ObservableCollection<ChartDataModel> BarData2 { get; set; }
        public ObservableCollection<ChartDataModel> BarData3 { get; set; }
        public ObservableCollection<ChartDataModel> RundedBarData { get; set; }
        public ObservableCollection<ChartDataModel> BarRow1 { get; set; }
        public ObservableCollection<Brush> BarBackToBackColor { get; set; }

        public BarSeriesViewModel()
        {
            EnableAnimation = true;

            RundedBarData = new ObservableCollection<ChartDataModel>()
            {
                new ChartDataModel("Boat", 9.872),
                new ChartDataModel("Walk", 5.237),
                new ChartDataModel("Plane", 9.0437),
                new ChartDataModel("Bike", 4.11),
                new ChartDataModel("Car", 8.43),
            };

            BarData1 = new ObservableCollection<ChartDataModel>
            {
                new ChartDataModel("Facebook", 4.119),
                new ChartDataModel("FB Messenger", 3.408),
                new ChartDataModel("WhatsApp", 2.979),
                new ChartDataModel("Instagram", 1.843),
                new ChartDataModel("Skype", 1.039),
                new ChartDataModel("Subway Surfers", 1.025),
            };

            BarData2 = new ObservableCollection<ChartDataModel>
            {
                new ChartDataModel("Egg", 2.2),
                new ChartDataModel("Fish", 2.4),
                new ChartDataModel("Misc", 3),
                new ChartDataModel("Tea", 3.1),
            };

            BarData3 = new ObservableCollection<ChartDataModel>
            {
                new ChartDataModel("Egg", 1.2),
                new ChartDataModel("Fish", 1.3),
                new ChartDataModel("Misc", 1.5),
                new ChartDataModel("Tea", 2.2),
            };

            BarRow1 = new ObservableCollection<ChartDataModel>()
            {
                new ChartDataModel("Switzerland", -2.39, 3.69),
                new ChartDataModel("Armenia", -7.40, 5.70),
                new ChartDataModel("Bulgaria", -4.39, 4.18),
                new ChartDataModel("Hong Kong", -6.50, 6.42),
                new ChartDataModel("Brazil", -3.88, 4.62),
                new ChartDataModel("Eswatini", -2.85, 7.43),
                new ChartDataModel("Mauritania", -2.76, 2.30),
            };

            BarBackToBackColor = new ObservableCollection<Brush>()
            {
                new SolidColorBrush(Color.FromArgb("#FFD700")),
                new SolidColorBrush(Color.FromArgb("#CD7F32")),
            };
        }
    }
}
