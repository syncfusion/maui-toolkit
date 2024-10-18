using System.Collections.ObjectModel;

namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
    public class RangeColumnSerieViewModel : BaseViewModel
    {
        public ObservableCollection<ChartDataModel> TemperatureData { get; set; }

        public RangeColumnSerieViewModel()
        {
            TemperatureData = new ObservableCollection<ChartDataModel>()
        {
                
                                   new ChartDataModel("Jan",13, 3),
                                   new ChartDataModel("Feb",14, 3),
                                   new ChartDataModel("Mar",17, 6),
                                   new ChartDataModel("Apr",20, 8),
                                   new ChartDataModel("May",24, 13),
                                   new ChartDataModel("Jun",29, 17),
                                   new ChartDataModel("Jul",32, 19),
                                   new ChartDataModel("Aug",30, 18),
                                   new ChartDataModel("Sep",27, 16),
                                   new ChartDataModel("Oct",23, 12),
                                   new ChartDataModel("Nov",18, 8),
                                   new ChartDataModel("Dec",15, 4),
            };

        }
    }
}
