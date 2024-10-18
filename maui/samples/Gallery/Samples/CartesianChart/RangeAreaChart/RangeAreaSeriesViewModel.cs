using System.Collections.ObjectModel;

namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
    public class RangeAreaSeriesViewModel : BaseViewModel
    {
        public ObservableCollection<ChartDataModel> RangeAreaData1 { get; set; }

        public ObservableCollection<ChartDataModel> RangeAreaData2 { get; set; }
        public RangeAreaSeriesViewModel()
        {
            RangeAreaData1 = new ObservableCollection<ChartDataModel>
            {
                new ChartDataModel(new DateTime(2022, 05, 01),36,13),
                new ChartDataModel(new DateTime(2022, 05, 02),33,16),
                new ChartDataModel(new DateTime(2022, 05, 03),32, 15),
                new ChartDataModel(new DateTime(2022, 05, 04),32, 13),
                new ChartDataModel(new DateTime(2022, 05, 05),34, 13),
                new ChartDataModel(new DateTime(2022, 05, 06),37, 14),
                new ChartDataModel(new DateTime(2022, 05, 07),38, 16),
                new ChartDataModel(new DateTime(2022, 05, 08),35, 22),
                new ChartDataModel(new DateTime(2022, 05, 09),31, 16),
                new ChartDataModel(new DateTime(2022, 05, 10),32, 13),
                new ChartDataModel(new DateTime(2022, 05, 11),30, 16),
                new ChartDataModel(new DateTime(2022, 05, 12),28, 12),
                new ChartDataModel(new DateTime(2022, 05, 13),34, 10),
                new ChartDataModel(new DateTime(2022, 05, 14),38, 14),
                new ChartDataModel(new DateTime(2022, 05, 15),40, 17),
                new ChartDataModel(new DateTime(2022, 05, 16),39, 18),
                new ChartDataModel(new DateTime(2022, 05, 17),38, 18),
                new ChartDataModel(new DateTime(2022, 05, 18),38, 17),
                new ChartDataModel(new DateTime(2022, 05, 19),37,19),
                new ChartDataModel(new DateTime(2022, 05, 20),36, 21),
                new ChartDataModel(new DateTime(2022, 05, 21),35, 18),
                new ChartDataModel(new DateTime(2022, 05, 22),36, 17),
                new ChartDataModel(new DateTime(2022, 05, 23),34, 16),
                new ChartDataModel(new DateTime(2022, 05, 24),36, 17),
                new ChartDataModel(new DateTime(2022, 05, 25),38, 18),
                new ChartDataModel(new DateTime(2022, 05, 26),39, 20),
                new ChartDataModel(new DateTime(2022, 05, 27),40, 21),
                new ChartDataModel(new DateTime(2022, 05, 28),38, 21),
                new ChartDataModel(new DateTime(2022, 05, 29),33, 21),
                new ChartDataModel(new DateTime(2022, 05, 30),34, 18),
                new ChartDataModel(new DateTime(2022, 05, 31),37, 17),
            };

            RangeAreaData2 = new ObservableCollection<ChartDataModel>
            {
                new ChartDataModel(new DateTime(2022, 05, 01),12,8),
                new ChartDataModel(new DateTime(2022, 05, 02),15,9),
                new ChartDataModel(new DateTime(2022, 05, 03),15,8),
                new ChartDataModel(new DateTime(2022, 05, 04),15,8),
                new ChartDataModel(new DateTime(2022, 05, 05),19,6),
                new ChartDataModel(new DateTime(2022, 05, 06),17,6),
                new ChartDataModel(new DateTime(2022, 05, 07),19,9),
                new ChartDataModel(new DateTime(2022, 05, 08),19,9),
                new ChartDataModel(new DateTime(2022, 05, 09),19,7),
                new ChartDataModel(new DateTime(2022, 05, 10),20,12),
                new ChartDataModel(new DateTime(2022, 05, 11),17,9),
                new ChartDataModel(new DateTime(2022, 05, 12),16,7),
                new ChartDataModel(new DateTime(2022, 05, 13),19,9),
                new ChartDataModel(new DateTime(2022, 05, 14),22,6),
                new ChartDataModel(new DateTime(2022, 05, 15),21,11),
                new ChartDataModel(new DateTime(2022, 05, 16),21,12),
                new ChartDataModel(new DateTime(2022, 05, 17),24,10),
                new ChartDataModel(new DateTime(2022, 05, 18),20,9),
                new ChartDataModel(new DateTime(2022, 05, 19),20,9),
                new ChartDataModel(new DateTime(2022, 05, 20),17,11),
                new ChartDataModel(new DateTime(2022, 05, 21),19,9),
                new ChartDataModel(new DateTime(2022, 05, 22),20,7),
                new ChartDataModel(new DateTime(2022, 05, 23),17,10),
                new ChartDataModel(new DateTime(2022, 05, 24),17,9),
                new ChartDataModel(new DateTime(2022, 05, 25),17,9),
                new ChartDataModel(new DateTime(2022, 05, 26),17,10),
                new ChartDataModel(new DateTime(2022, 05, 27),18,9),
                new ChartDataModel(new DateTime(2022, 05, 28),17,8),
                new ChartDataModel(new DateTime(2022, 05, 29),15,7),
                new ChartDataModel(new DateTime(2022, 05, 30),16,7),
                new ChartDataModel(new DateTime(2022, 05, 31),15,7),
            };
        }
    }
}

