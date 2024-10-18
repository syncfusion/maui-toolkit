using System.Collections.ObjectModel;

namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
    public class AreaSeriesViewModel : BaseViewModel
    {
        public ObservableCollection<ChartDataModel> AreaData1 { get; set; }

        public AreaSeriesViewModel()
        {
            AreaData1 = new ObservableCollection<ChartDataModel>
            {
                new ChartDataModel("2000",0.87,0.72,0.48, 0.23),
                new ChartDataModel("2001", 0.91, 0.64,0.43,0.17),
                new ChartDataModel("2002",1.01,0.71, 0.47,0.17),
                new ChartDataModel( "2003", 0.95, 0.63, 0.41, 0.20),
                new ChartDataModel( "2004", 0.89, 0.65, 0.43, 0.23),
                new ChartDataModel( "2005", 1.09, 0.76, 0.54, 0.36),
                new ChartDataModel( "2006", 1.14, 0.89, 0.66, 0.43),
                new ChartDataModel( "2007", 1.44, 1.18, 0.83,0.52),
                new ChartDataModel( "2008", 1.66, 1.34, 1.09, 0.72),
                new ChartDataModel( "2009", 1.91,1.59, 1.37,1.09),
                new ChartDataModel( "2010", 2.14, 1.82, 1.62, 1.38),
                new ChartDataModel( "2011", 2.73, 2.35, 2.13, 1.82),
                new ChartDataModel("2012", 3.126, 2.69, 2.44, 2.16),
                new ChartDataModel("2013", 3.34, 3.01, 2.77, 2.51),
                new ChartDataModel("2014", 3.58, 3.22, 2.91, 2.61),
       };
        }
    }

}
