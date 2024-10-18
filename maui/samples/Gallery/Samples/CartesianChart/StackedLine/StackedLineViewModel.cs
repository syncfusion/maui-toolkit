using System.Collections.ObjectModel;

namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
    public class StackedLineViewModel: BaseViewModel
    {
        public ObservableCollection<ChartDataModel> StackedLineData { get; set; }
        public StackedLineViewModel()
        {
            StackedLineData = new ObservableCollection<ChartDataModel>()
            {
                new ChartDataModel("2014",3.2,2.5,5.7,7.4),
                new ChartDataModel("2015",2.2,2.9,5.7,7),
                new ChartDataModel("2016",1.9,1.8,4.5,6.8),
                new ChartDataModel("2017",2.7,2.5,3.5,6.9),
                new ChartDataModel("2018",1.4,3,4,6.7),
                new ChartDataModel("2019",1.6,2.5,3.5,6),
            };
        }
    }
}