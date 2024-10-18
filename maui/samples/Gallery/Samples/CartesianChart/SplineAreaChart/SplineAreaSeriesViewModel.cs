using System.Collections.ObjectModel;

namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
    public class SplineAreaSeriesViewModel : BaseViewModel
    {
        public ObservableCollection<ChartDataModel> SplineAreaData1 { get; set; }

        public ObservableCollection<ChartDataModel> SplineAreaData2 { get; set; }

        public ObservableCollection<ChartDataModel> SplineAreaData3 { get; set; }

        public SplineAreaSeriesViewModel()
        {
            SplineAreaData1 = new ObservableCollection<ChartDataModel>
            {
                new ChartDataModel(2010, 10.53, 3.3),
                new ChartDataModel(2011, 9.5, 5.4),
                new ChartDataModel(2012, 10, 2.65),
                new ChartDataModel(2013, 9.4, 2.62),
                new ChartDataModel(2014, 5.8, 1.99),
                new ChartDataModel(2015, 4.9, 1.44),
                new ChartDataModel(2016, 4.5, 2),
                new ChartDataModel(2017, 3.6, 1.56),
                new ChartDataModel(2018, 3.43, 2.1),
            };

            SplineAreaData2 = new ObservableCollection<ChartDataModel>
            {
                new ChartDataModel(2002, 2.0),
                new ChartDataModel(2003, 1.7),
                new ChartDataModel(2004, 1.8),
                new ChartDataModel(2005, 2.1),
                new ChartDataModel(2006, 2.3),
                new ChartDataModel(2007, 1.7),
                new ChartDataModel(2008, 1.5),
                new ChartDataModel(2009, 2.8),
                new ChartDataModel(2010, 1.5),
                new ChartDataModel(2011, 2.3),

            };

            SplineAreaData3 = new ObservableCollection<ChartDataModel>
            {
                new ChartDataModel(2002, 0.8),
                new ChartDataModel(2003, 1.3),
                new ChartDataModel(2004, 1.1),
                new ChartDataModel(2005, 1.6),
                new ChartDataModel(2006, 2.0),
                new ChartDataModel(2007, 1.7),
                new ChartDataModel(2008, 2.3),
                new ChartDataModel(2009, 2.7),
                new ChartDataModel(2010, 1.1),
                new ChartDataModel(2011, 2.3),
            };
        }
    }

}
