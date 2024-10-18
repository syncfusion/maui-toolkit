using System.Collections.ObjectModel;

namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
    public class ColumnSeriesViewModel : BaseViewModel
    {
        public ObservableCollection<ChartDataModel> ColumnData1 { get; set; }
        public ObservableCollection<ChartDataModel> RoundedColumnData { get; set; }
        public ObservableCollection<ChartDataModel> OlympicMedals { get; set; }

        public ObservableCollection<Brush> OlympicColorModel { get; set; }

        public ColumnSeriesViewModel()
        {
            EnableAnimation = true;

            ColumnData1 = new ObservableCollection<ChartDataModel>
            {
                new ChartDataModel("China", 0.541),
                new ChartDataModel("Egypt", 0.818),
                new ChartDataModel("Bolivia", 1.51),
                new ChartDataModel("Mexico", 1.302),
                new ChartDataModel("Brazil", 2.017)

           };

            RoundedColumnData = new ObservableCollection<ChartDataModel>()
            {
                new ChartDataModel(1, 25),
                new ChartDataModel(2, 49),
                new ChartDataModel(3, 28),
                new ChartDataModel(4, 14),
                new ChartDataModel(5, 32),
                new ChartDataModel(6, 51),
                new ChartDataModel(7, 45),
                new ChartDataModel(8, 60),
            };

            OlympicMedals = new ObservableCollection<ChartDataModel>()
            {
                new ChartDataModel("Norway", 16, 8, 13),
                new ChartDataModel("Russia", 6, 12,  14),
                new ChartDataModel("Germany", 12, 10, 5),
                new ChartDataModel("Canada", 4, 8,  14),
            };

            OlympicColorModel = new ObservableCollection<Brush>()
            {
                new SolidColorBrush(Color.FromArgb("#FFD700")),
                new SolidColorBrush(Color.FromArgb("#C0C0C0")),
                new SolidColorBrush(Color.FromArgb("#CD7F32")),
            };
        }
    }
}
