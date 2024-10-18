
using System.Collections.ObjectModel;

namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
    public class CartesianBubbleViewModel : BaseViewModel
    {
        public ObservableCollection<ChartDataModel> GDPGrowthCollection { get; set; }
        public ObservableCollection<ChartDataModel> ActionData { get; set; }
        public ObservableCollection<ChartDataModel> HorrorData { get; set; }
        public ObservableCollection<ChartDataModel> ScienceFictionData { get; set; }
        public ObservableCollection<ChartDataModel> ThrillerData { get; set; }

        public CartesianBubbleViewModel()
        {

            GDPGrowthCollection = new ObservableCollection<ChartDataModel>()
        {
         new ChartDataModel("China",96.8,15134,1.41),
         new ChartDataModel("India",74.3,6518.8,1.36),
         new ChartDataModel( "Indonesia", 95.6, 11371.7, 0.26),
         new ChartDataModel( "Italy", 99.1, 42045.92, 0.05),
         new ChartDataModel( "Malaysia", 94.8, 27577.37, 0.08),
         new ChartDataModel( "Romania", 98.8, 28523.45, 0.01),
         new ChartDataModel( "Russia", 99.7, 26656.41, 0.14),
         new ChartDataModel( "Mexico", 95.3, 19928.4, 0.12),
         new ChartDataModel( "Uganda", 76.5, 2124.792, 0.04),
         new ChartDataModel( "Nigeria", 62, 5155.076, 0.19),
         new ChartDataModel( "Algeria", 81.4, 11630.68, 0.7),
         new ChartDataModel( "Greece", 97.9, 29141.17, 0.01),
         new  ChartDataModel( "Jordan", 98.2, 10013.4, 0.01),
         new ChartDataModel( "Colombia", 95, 14314.86, 0.04),
         new ChartDataModel( "Mongolia", 98.4, 12028.64, 0.003),
         new  ChartDataModel( "Brazil", 93.2, 14668.26, 0.21),
         new  ChartDataModel( "Nepal", 67.9, 3773.716,  0.02),
         new  ChartDataModel( "Sudan", 60.6, 4369.762,  0.04),

        };

            ActionData = new ObservableCollection<ChartDataModel>()
        {
                new ChartDataModel("Transformers II",150,836,369,6),
                new ChartDataModel("Skyfall",200,1109,599,7.7),
                new ChartDataModel("The Avengers",220,1520,1205,8),
                new ChartDataModel("Spider-Man 3",258,891,471,6.2),
                new ChartDataModel("Transformers III",195,1124,371,6.2),
                new ChartDataModel("The Dark Knight Rises",250,1085,1418,8.4),
                new ChartDataModel("The Dark Knight",185,1005,2127,9),
                new ChartDataModel("Inception",160,826,1888,8.8),

        };

            HorrorData = new ObservableCollection<ChartDataModel>()
        {
                new ChartDataModel("Interview with the Vampire",60,224,83, 6.9),
                new ChartDataModel("Scream",14,173,268,7.2),
                new ChartDataModel("I Know What You Did Last Summer", 17,126, 126,5.7),
                new ChartDataModel("The Ring", 48,249, 303,7.1),
                new ChartDataModel("Van Helsing", 160,300,233,6.1),
                new ChartDataModel("Scream 2", 24,172, 148,6.2),
                new ChartDataModel("The Conjuring", 13,318,410,7.5),
                new ChartDataModel("Flatliners",26,61,76,6.6),

        };

            ScienceFictionData = new ObservableCollection<ChartDataModel>()
        {
                new ChartDataModel("Armageddon",140,554,377,6.7),
                new ChartDataModel("Star Wars: Episode I",115,924,667,6.5),
                new ChartDataModel("Star Wars: Episode II",120,649,587,6.6),
                new ChartDataModel("The Matrix Reloaded", 150,739,487,7.2),
                new ChartDataModel("Star Wars: Episode III", 113,850,654,7.5),
                new ChartDataModel("War of the Worlds", 132,592,394,6.5),
                new ChartDataModel("World War Z", 200,532, 564,7),
                new ChartDataModel("Dawn of the Planet of the Apes", 170,711,395,7.6),
        };

            ThrillerData = new ObservableCollection<ChartDataModel>()
        {
                new ChartDataModel("Men in Black",90,589,487,7.3),
                new ChartDataModel("Godzilla",130,379,175,5.4),
                new ChartDataModel("The Sixth Sense",40,673,860,8.1),
                new ChartDataModel("Ocean's Eleven",85,451,482,7.8),
                new ChartDataModel("Terminator 3",200,435,357,6.3),
                new ChartDataModel("Casino Royale",150,599,547,8),
                new ChartDataModel("Live Free or Die Hard",110,384,375,7.1),
                new ChartDataModel("Clash of the Titans",125,493,260,5.8),
                new ChartDataModel("Mission: Impossible",145,695,435,7.4),
                new ChartDataModel("Godzilla",160,529,359,6.4),
        };
        }

    }
}
