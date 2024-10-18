using System.Collections.ObjectModel;

namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
    public class StackedColumnViewModel : BaseViewModel
    {
        public ObservableCollection<ChartDataModel> ProductData { get; set; }
        public ObservableCollection<ChartDataModel> RevenueData { get; set; }
        public ObservableCollection<ChartDataModel> CostData { get; set; }
        public ObservableCollection<ChartDataModel> CostData1 { get; set; }
        public ObservableCollection<ChartDataModel> CostData2 { get; set; }

        public StackedColumnViewModel()
        {
            ProductData = new ObservableCollection<ChartDataModel>()
             {
                 new ChartDataModel("Eyebrow pencil",-3.932,-3.987, -5.067,13.012),
                 new ChartDataModel("Eyeliner",-5.432,3.417,15.067,12.321),
                 new ChartDataModel("Nail polish",-4.229,-4.376,-3.504,12.814),
                 new ChartDataModel("Lipstick",-9.256,4.376,9.054,8.814),
                 new ChartDataModel("Rouge",5.221,-3.574,-7.004,11.624),
                 new ChartDataModel("Powder",18.012, -8.034,10.919,11.861)
             }; 

            RevenueData = new ObservableCollection<ChartDataModel>()
            {
                new ChartDataModel("2010", 13.767,7.726,22.769),
                new ChartDataModel("2011", 15.471,8.206,22.790),
                new ChartDataModel("2012", 18.097,9.057,24.170),
                new ChartDataModel("2013", 18.056,8.946,22.795),
                new ChartDataModel("2014", 17.739,9.164,21.533),
#if !IOS && !ANDROID
                new ChartDataModel("2015", 20.074,10.159,23.039),
                new ChartDataModel("2016", 20.172,10.009,22.532),
                new ChartDataModel("2017", 20.697,10.574,22.444),
#endif
            };

            CostData = new ObservableCollection<ChartDataModel>()
            {
             new ChartDataModel ( "Q1", 75,50.76 ),
             new ChartDataModel ("Q2", 55,58.14 ),
             new ChartDataModel ("Q3", 65,61.89 ),
             new ChartDataModel ("Q4",70,64.578)
            };

            CostData1 = new ObservableCollection<ChartDataModel>()
            {
             new ChartDataModel ( "Q1", 55, 35.9),
             new ChartDataModel ("Q2", 40,45.2 ),
             new ChartDataModel ("Q3", 55,52.34),
             new ChartDataModel ("Q4",55,48.765 )
            };

            CostData2 = new ObservableCollection<ChartDataModel>()
            {
             new ChartDataModel ( "Q1", 35,18.25 ),
             new ChartDataModel ("Q2", 20 ,18.55),
             new ChartDataModel ("Q3", 15,16.24),
             new ChartDataModel ("Q4",20,18.5 )
            };
        }
    }
}
