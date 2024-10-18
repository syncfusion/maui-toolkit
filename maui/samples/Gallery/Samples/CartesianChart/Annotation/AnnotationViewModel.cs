using System.Collections.ObjectModel;

namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
    public class AnnotationViewModel : BaseViewModel
    {
        public ObservableCollection<ChartDataModel> AnnotationData { get; set; }

        public AnnotationViewModel()
        {
            AnnotationData = new ObservableCollection<ChartDataModel>()
            {
               new ChartDataModel(){Date=new DateTime(2020,03,02),Value = 100, Size=350},
               new ChartDataModel(){Date=new DateTime(2020,03,09),Value = 200, Size=470},
               new ChartDataModel(){Date=new DateTime(2020,03,16),Value = 1700, Size=500},
               new ChartDataModel(){Date=new DateTime(2020,03,23),Value = 1800, Size=530},
               new ChartDataModel(){Date=new DateTime(2020,03,30),Value = 1900, Size=560},
               new ChartDataModel(){Date=new DateTime(2020,04,06),Value = 2700, Size=590},
               new ChartDataModel(){Date=new DateTime(2020,04,13),Value = 3700, Size=730},
               new ChartDataModel(){Date=new DateTime(2020,04,20),Value = 3800, Size=760},
               new ChartDataModel(){Date=new DateTime(2020,04,27),Value = 3900, Size=790},
               new ChartDataModel(){Date=new DateTime(2020,05,04),Value = 4250, Size=820},
               new ChartDataModel(){Date=new DateTime(2020,05,11),Value = 4600, Size=850},
               new ChartDataModel(){Date=new DateTime(2020,05,18),Value = 4950, Size=880},
               new ChartDataModel(){Date=new DateTime(2020,05,25),Value = 5300, Size=910},
               new ChartDataModel(){Date=new DateTime(2020,06,01),Value = 5850, Size=940},
               new ChartDataModel(){Date=new DateTime(2020,06,08),Value = 6780, Size=970},
               new ChartDataModel(){Date=new DateTime(2020,06,15),Value = 7130, Size=1000},
               new ChartDataModel(){Date=new DateTime(2020,06,22),Value = 7480, Size=1030},
               new ChartDataModel(){Date=new DateTime(2020,06,29),Value = 9230, Size=1150},
               new ChartDataModel(){Date=new DateTime(2020,07,06),Value = 9580, Size=1180},
               new ChartDataModel(){Date=new DateTime(2020,07,13),Value = 9930, Size=1580},
               new ChartDataModel(){Date=new DateTime(2020,07,20),Value = 10280, Size=1610},
               new ChartDataModel(){Date=new DateTime(2020,07,27),Value = 10630, Size=1640},
               new ChartDataModel(){Date=new DateTime(2020,08,03),Value = 10980, Size=1640},
               new ChartDataModel(){Date=new DateTime(2020,08,10),Value = 11330, Size=1640},
               new ChartDataModel(){Date=new DateTime(2020,08,17),Value = 11680, Size=1670},
               new ChartDataModel(){Date=new DateTime(2020,08,24),Value = 12030, Size=1700},
               new ChartDataModel(){Date=new DateTime(2020,08,31),Value = 13430, Size=2100},
               new ChartDataModel(){Date=new DateTime(2020,09,07),Value = 14030, Size=2900},
               new ChartDataModel(){Date=new DateTime(2020,09,14),Value = 14630, Size=2930},
               new ChartDataModel(){Date=new DateTime(2020,09,21),Value = 14860, Size=3090},
               new ChartDataModel(){Date=new DateTime(2020,09,28),Value = 15090, Size=3250},
               new ChartDataModel(){Date=new DateTime(2020,10,05),Value = 15560, Size=3410},
               new ChartDataModel(){Date=new DateTime(2020,10,12),Value = 15910, Size=3440},
               new ChartDataModel(){Date=new DateTime(2020,10,19),Value = 16260, Size=3470},
               new ChartDataModel(){Date=new DateTime(2020,10,26),Value = 16610, Size=3630},
               new ChartDataModel(){Date=new DateTime(2020,11,02),Value = 16960, Size=3660},
               new ChartDataModel(){Date=new DateTime(2020,11,09),Value = 17310, Size=3690},
               new ChartDataModel(){Date=new DateTime(2020,11,16),Value = 17660, Size=3720},
               new ChartDataModel(){Date=new DateTime(2020,11,23),Value = 17890, Size=3880},
               new ChartDataModel(){Date=new DateTime(2020,11,30),Value = 18120, Size=4040},
               new ChartDataModel(){Date=new DateTime(2020,12,07),Value = 18350, Size=4070},
               new ChartDataModel(){Date=new DateTime(2020,12,14),Value = 18820, Size=4100},
               new ChartDataModel(){Date=new DateTime(2020,12,21),Value = 19620, Size=4900},
               new ChartDataModel(){Date=new DateTime(2020,12,28),Value = 20420, Size=5700},
               new ChartDataModel(){Date=new DateTime(2021,01,04),Value = 20890, Size=5730},
               new ChartDataModel(){Date=new DateTime(2021,01,11),Value = 21240, Size=5890},
               new ChartDataModel(){Date=new DateTime(2021,01,18),Value = 21710, Size=5920},
               new ChartDataModel(){Date=new DateTime(2021,01,25),Value = 22180, Size=5950},
               new ChartDataModel(){Date=new DateTime(2021,02,01),Value = 23110, Size=6110},
               new ChartDataModel(){Date=new DateTime(2021,02,08),Value = 23580, Size=6140},
               new ChartDataModel(){Date=new DateTime(2021,02,15),Value = 23930, Size=6300},
               new ChartDataModel(){Date=new DateTime(2021,02,22),Value = 24280, Size=6330},
               new ChartDataModel(){Date=new DateTime(2021,03,01),Value = 22630, Size=6360},

            };
        }

    }
}
