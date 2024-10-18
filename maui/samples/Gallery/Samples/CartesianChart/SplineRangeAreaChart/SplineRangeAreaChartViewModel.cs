using System.Collections.ObjectModel;

namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
    public class SplineRangeAreaChartViewModel : BaseViewModel
    {
        public ObservableCollection<ChartDataModel> SplineRangeAreaData { get; set; }
        public SplineRangeAreaChartViewModel()
        {
            SplineRangeAreaData = new ObservableCollection<ChartDataModel>
            {
                new ChartDataModel(new DateTime(2023, 08, 01),99,85.3,70),
                new ChartDataModel(new DateTime(2023, 08, 02),97,84.7,66),
                new ChartDataModel(new DateTime(2023, 08, 03),91,81.5,72),
                new ChartDataModel(new DateTime(2023, 08, 04),84,72.2,61),
                new ChartDataModel(new DateTime(2023, 08, 05),95,75.6,57),
                new ChartDataModel(new DateTime(2023, 08, 06),99,82.7,68),
                new ChartDataModel(new DateTime(2023, 08, 07),99,82.2,64),
                new ChartDataModel(new DateTime(2023, 08, 08),102,86.7,68),
                new ChartDataModel(new DateTime(2023, 08, 09),104,90.3,73),
                new ChartDataModel(new DateTime(2023, 08, 10),102,90.0,77),
                new ChartDataModel(new DateTime(2023, 08, 11),102,86.7,70),
                new ChartDataModel(new DateTime(2023, 08, 12),100,87.9,72),
                new ChartDataModel(new DateTime(2023, 08, 13),102,87.2,73),
                new ChartDataModel(new DateTime(2023, 08, 14),97,84.2,66),
                new ChartDataModel(new DateTime(2023, 08, 15),99,82.7,64),
                new ChartDataModel(new DateTime(2023, 08, 16),97,83.2,66),
                new ChartDataModel(new DateTime(2023, 08, 17),97,83.4,66),
                new ChartDataModel(new DateTime(2023, 08, 18),95,83.0,66),
                new ChartDataModel(new DateTime(2023, 08, 19),99,84.2,66),
                new ChartDataModel(new DateTime(2023, 08, 20),102,87.1,70),
                new ChartDataModel(new DateTime(2023, 08, 21),104,90.0,72),
                new ChartDataModel(new DateTime(2023, 08, 22),104,90.8,73),
                new ChartDataModel(new DateTime(2023, 08, 23),104,90.2,73),
                new ChartDataModel(new DateTime(2023, 08, 24),104,89.3,72),
                new ChartDataModel(new DateTime(2023, 08, 25),102,89.2,70),
                new ChartDataModel(new DateTime(2023, 08, 26),91,84.3,77),
                new ChartDataModel(new DateTime(2023, 08, 27),77,69.6,61),
                new ChartDataModel(new DateTime(2023, 08, 28),84,70.8,55),
                new ChartDataModel(new DateTime(2023, 08, 29),82,73.1,64),
                new ChartDataModel(new DateTime(2023, 08, 30),84,72.6,59),
                new ChartDataModel(new DateTime(2023, 08, 31),90,75.4,59),
            };
        }
    }
}

