using System.Collections.ObjectModel;

namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
    public class CategoryAxisViewModel : BaseViewModel
    {
        public ObservableCollection<ChartDataModel> DataCollection1 { get; set; }

        public CategoryAxisViewModel()
        {
            DataCollection1 = new ObservableCollection<ChartDataModel>()
            {
                new ChartDataModel("Korea",39),
                new ChartDataModel("India",20),
                new ChartDataModel("Africa",  61),
                new ChartDataModel("China",65),
                new ChartDataModel("France",45),
                new ChartDataModel("Saudi", 10),
                //new ChartDataModel("London", 16),
                //new ChartDataModel("Mexico",31)
            };
        }
    }
}
