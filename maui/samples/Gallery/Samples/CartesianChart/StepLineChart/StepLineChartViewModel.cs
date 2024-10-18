using System.Collections.ObjectModel;

namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
    public class StepLineChartViewModel:BaseViewModel
    {
        public ObservableCollection<ChartDataModel> AverageRainFallData { get; set; }
        public ObservableCollection<ChartDataModel> USAGasolinePriceData { get; set; }
        public ObservableCollection<ChartDataModel> Co2IntensityData { get; set; }

        public ObservableCollection<ChartDataModel> ElectricityProductionData { get; set; }

        public StepLineChartViewModel()
        {
            AverageRainFallData = new ObservableCollection<ChartDataModel>()
            {
               new ChartDataModel("Jan",85,40),
               new ChartDataModel("Feb",57,40),
               new ChartDataModel("Mar",90,40),
               new ChartDataModel("Apr",96,50),
               new ChartDataModel("May",114,30),
               new ChartDataModel("Jun",98,40),
               new ChartDataModel("Jul",101,60),
               new ChartDataModel("Aug",89,50),
               new ChartDataModel("Sep",104,40),
               new ChartDataModel("Oct",94,40),
               new ChartDataModel("Nov",69,50),
               new ChartDataModel("Dec",85,20),
            };

            USAGasolinePriceData = new ObservableCollection<ChartDataModel>()
            {
                new ChartDataModel(new DateTime(2020,01,06),2.561,2.494),
                new ChartDataModel(new DateTime(2020,01,13),2.549,2.482),
                new ChartDataModel(new DateTime(2020,01,20),2.516,2.448),
                new ChartDataModel(new DateTime(2020,01,27),2.482,2.412),
                new ChartDataModel(new DateTime(2020,02,03),2.428,2.358),
                new ChartDataModel(new DateTime(2020,02,10),2.396,2.324),
                new ChartDataModel(new DateTime(2020,02,17),2.405,2.337),
                new ChartDataModel(new DateTime(2020,02,24),2.441,2.373),
                new ChartDataModel(new DateTime(2020,03,02),2.394,2.324),
                new ChartDataModel(new DateTime(2020,03,09),2.344,2.272),
                new ChartDataModel(new DateTime(2020,03,16),2.213,2.139),
                new ChartDataModel(new DateTime(2020,03,23),2.083,2.007),
                new ChartDataModel(new DateTime(2020,03,30),1.962,1.886),
                new ChartDataModel(new DateTime(2020,04,06),1.877,1.8),
                new ChartDataModel(new DateTime(2020,04,13),1.811,1.735),
                new ChartDataModel(new DateTime(2020,04,20),1.77,1.694),
                new ChartDataModel(new DateTime(2020,04,27),1.731,1.655),
                new ChartDataModel(new DateTime(2020,05,04),1.753,1.68),
                new ChartDataModel(new DateTime(2020,05,11),1.818,1.75),
                new ChartDataModel(new DateTime(2020,05,18),1.845,1.776),
                new ChartDataModel(new DateTime(2020,05,25),1.938,1.87),
                new ChartDataModel(new DateTime(2020,06,01),1.952,1.883),
                new ChartDataModel(new DateTime(2020,06,08),2.014,1.947),
                new ChartDataModel(new DateTime(2020,06,15),2.084,2.017),
                new ChartDataModel(new DateTime(2020,06,22),2.115,2.048),
                new ChartDataModel(new DateTime(2020,06,29),2.162,2.094),
                new ChartDataModel(new DateTime(2020,07,06),2.168,2.1),
                new ChartDataModel(new DateTime(2020,07,13),2.181,2.113),
                new ChartDataModel(new DateTime(2020,07,20),2.168,2.099),
                new ChartDataModel(new DateTime(2020,07,27),2.155,2.085),
                new ChartDataModel(new DateTime(2020,08,03),2.156,2.085),
                new ChartDataModel(new DateTime(2020,08,10),2.149,2.078),
                new ChartDataModel(new DateTime(2020,08,17),2.147,2.077),
                new ChartDataModel(new DateTime(2020,08,24),2.161,2.09),
                new ChartDataModel(new DateTime(2020,08,31),2.204,2.135),
                new ChartDataModel(new DateTime(2020,09,07),2.193,2.122),
                new ChartDataModel(new DateTime(2020,09,14),2.162,2.091),
                new ChartDataModel(new DateTime(2020,09,21),2.149,2.078),
                new ChartDataModel(new DateTime(2020,09,28),2.157,2.088),
                new ChartDataModel(new DateTime(2020,10,05),2.161,2.091),
                new ChartDataModel(new DateTime(2020,10,12),2.154,2.084),
                new ChartDataModel(new DateTime(2020,10,19),2.134,2.064),
                new ChartDataModel(new DateTime(2020,10,26),2.124,2.053),
                new ChartDataModel(new DateTime(2020,11,02),2.092,2.021),
                new ChartDataModel(new DateTime(2020,11,09),2.074,2.004),
                new ChartDataModel(new DateTime(2020,11,16),2.089,2.018),
                new ChartDataModel(new DateTime(2020,11,23),2.08,2.009),
                new ChartDataModel(new DateTime(2020,11,30),2.093,2.022),
                new ChartDataModel(new DateTime(2020,12,07),2.133,2.063),
                new ChartDataModel(new DateTime(2020,12,14),2.132,2.063),
                new ChartDataModel(new DateTime(2020,12,21),2.204,2.137),
                new ChartDataModel(new DateTime(2020,12,28),2.225,2.158),

            };

            Co2IntensityData = new ObservableCollection<ChartDataModel>()
            {
               new ChartDataModel("2000",533,495),
               new ChartDataModel("2001",535,509),
               new ChartDataModel("2002",527,506),
               new ChartDataModel("2003",532,508),
               new ChartDataModel("2004",529,494),
               new ChartDataModel("2005",531,496),
            };

            ElectricityProductionData = new ObservableCollection<ChartDataModel>()
            {
               new ChartDataModel("2004",75.4,71.4),
               new ChartDataModel("2005",74.4,72.1),
               new ChartDataModel("2006",75.6,71.3),
               new ChartDataModel("2007",78.5,72),
               new ChartDataModel("2008",80.2,71.4),
               new ChartDataModel("2009",74.2,69.5),
               new ChartDataModel("2010",76.4,70.3),
               new ChartDataModel("2011",71.1,68.4),
            };


        }
    }
}
