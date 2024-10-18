using System.Collections.ObjectModel;

namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
    public class CartesianTrackballViewModel : BaseViewModel
    {
        public ObservableCollection<ChartDataModel> ChartData1 { get; set; }
        public string[] DisplayMode => new string[] { "FloatAllPoints", "NearestPoint", "GroupAllPoints" };
        public string[] TrackballLabel => new string[] { "Default", "Template" };

        public DateTime Minimum { get; set; }
        public DateTime Maximum { get; set; }

        public CartesianTrackballViewModel()
        {
            Minimum = new DateTime(2000, 1, 1);
            Maximum = new DateTime(2006, 1, 1);
            ChartData1 = new ObservableCollection<ChartDataModel>()
            {
                new ChartDataModel(new DateTime(2000,1,1), 15, 39,60),
                new ChartDataModel(new DateTime(2000,9,14),20,30,55),
                new ChartDataModel(new DateTime(2001,1,1),25,28,48),
                new ChartDataModel(new DateTime(2001,9,16),21,35,57),
                new ChartDataModel(new DateTime(2002,1,1),13,39,62),
                new ChartDataModel(new DateTime(2002,9,7),18,41,64),
                new ChartDataModel(new DateTime(2003,1,1),24,45,57),
                new ChartDataModel(new DateTime(2003,9,14),23,48,53),
                new ChartDataModel(new DateTime(2004,1,1),19,54,63),
                new ChartDataModel(new DateTime(2004,9,6),31,55,50),
                new ChartDataModel(new DateTime(2005,1,1),39,57,66),
                new ChartDataModel(new DateTime(2005,9,10),50,60,65),
                new ChartDataModel(new DateTime(2006,1,1),24,60,79),
            };
        }
    }
}
