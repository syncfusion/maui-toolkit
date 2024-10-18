using System.Collections.ObjectModel;

namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
   public class StepAreaSeriesViewModel : BaseViewModel
    {
        public ObservableCollection<ChartDataModel> StepAreaData1 { get; set; }
        public StepAreaSeriesViewModel()
        {
            StepAreaData1 = new ObservableCollection<ChartDataModel>
            {
                new ChartDataModel(new DateTime(2023, 02, 01),252.88,181.92),
                new ChartDataModel(new DateTime(2023, 02, 02),264.68,188.63),
                new ChartDataModel(new DateTime(2023, 02, 03),258.92,194.68),
                new ChartDataModel(new DateTime(2023, 02, 06),256.91,195.35),
                new ChartDataModel(new DateTime(2023, 02, 07),267.84,197.03),
                new ChartDataModel(new DateTime(2023, 02, 08),266.98,201.39),
                new ChartDataModel(new DateTime(2023, 02, 09),263.52,207.43),
                new ChartDataModel(new DateTime(2023, 02, 10),262.95,196.69),
                new ChartDataModel(new DateTime(2023, 02, 13),271.78,195.01),
                new ChartDataModel(new DateTime(2023, 02, 14),272.45,209.78),
                new ChartDataModel(new DateTime(2023, 02, 15),269.25,214.15),
                new ChartDataModel(new DateTime(2023, 02, 16),262.37,202.06),
                new ChartDataModel(new DateTime(2023, 02, 17),258.35,208.44),
                new ChartDataModel(new DateTime(2023, 02, 21),252.88,198.03),
                new ChartDataModel(new DateTime(2023, 02, 22),251.44,200.72),
                new ChartDataModel(new DateTime(2023, 02, 23),255.47,202.40),
                new ChartDataModel(new DateTime(2023, 02, 24),249.42,197.03),
                new ChartDataModel(new DateTime(2023, 02, 27),250.29,208.11),
                new ChartDataModel(new DateTime(2023, 02, 28),249.35,206.09)
            };
                
        }
    }
}
