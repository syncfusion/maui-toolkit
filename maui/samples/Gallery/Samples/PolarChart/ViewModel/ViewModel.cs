using System.Collections.ObjectModel;

namespace  Syncfusion.Maui.ControlsGallery.PolarChart.SfPolarChart
{
    public class ViewModel : BaseViewModel
    {
        public string[] SeriesType => new string[] { "Area", "Line" };
        public string[] IsClosed => new string[] { "True", "False" };
        public string[] PolarAngle => new string[] { "0", "90", "180", "270" };
        public ObservableCollection<ChartDataModel> CorporateData { get; set; }
        public ObservableCollection<ChartDataModel> PlantDetails { get; set; }
        public ObservableCollection<ChartDataModel> ExpenditureData { get; set; }
        public ObservableCollection<ChartDataModel> HealthData { get; set; }

        public ViewModel()
        {
            // Default Polar
            PlantDetails = new ObservableCollection<ChartDataModel>()
            {
                 
#if ANDROID || IOS
                  new ChartDataModel(){ Category = "N", Value1 = 80, Value2 = 63, Value3 = 42},
                  new ChartDataModel(){ Category = "NE", Value1 = 85, Value2 = 70, Value3 = 40},
                  new ChartDataModel(){ Category = "E", Value1 = 78 , Value2 = 45, Value3 = 25},
                  new ChartDataModel(){ Category = "SE", Value1 = 90 , Value2 = 70, Value3 = 40},
                  new ChartDataModel(){ Category = "S", Value1 = 78 , Value2 = 47, Value3 = 20},
                  new ChartDataModel(){ Category = "SW", Value1 = 83 , Value2 = 65, Value3 = 45},
                  new ChartDataModel(){ Category = "W", Value1 = 79 , Value2 = 58, Value3 = 40},
                  new ChartDataModel(){ Category = "NW", Value1 = 88 , Value2 = 73, Value3 = 28},
#else
                  new ChartDataModel(){ Category = "North", Value1 = 80, Value2 = 63, Value3 = 42},
                  new ChartDataModel(){ Category = "NorthEast", Value1 = 85, Value2 = 70, Value3 = 40},
                  new ChartDataModel(){ Category = "East", Value1 = 78 , Value2 = 45, Value3 = 25},
                  new ChartDataModel(){ Category = "SouthEast", Value1 = 90 , Value2 = 70, Value3 = 40},
                  new ChartDataModel(){ Category = "South", Value1 = 78 , Value2 = 47, Value3 = 20},
                  new ChartDataModel(){ Category = "SouthWest", Value1 = 83 , Value2 = 65, Value3 = 45},
                  new ChartDataModel(){ Category = "West", Value1 = 79 , Value2 = 58, Value3 = 40},
                  new ChartDataModel(){ Category = "NorthWest", Value1 = 88 , Value2 = 73, Value3 = 28},
#endif
            };

            // Default Radar
            CorporateData = new ObservableCollection<ChartDataModel>()
            {
#if ANDROID || IOS
                  new ChartDataModel() { Category = "Quality", Value1 = 80, Value2=62, Value3=70},
                  new ChartDataModel() { Category = "Service", Value1 = 78, Value2=68, Value3=60},
                  new ChartDataModel() { Category = "Delivery", Value1 = 75, Value2=70, Value3=65},
                  new ChartDataModel() { Category = "UX", Value1 = 72, Value2=85, Value3=63},
                  new ChartDataModel() { Category = "Innovation", Value1 = 70, Value2=69, Value3=79},
#else
                  new ChartDataModel() { Category = "Product Quality", Value1 = 80, Value2=62, Value3=70},
                  new ChartDataModel() { Category = "Customer Service", Value1 = 78, Value2=68, Value3=60},
                  new ChartDataModel() { Category = "Delivery Speed", Value1 = 75, Value2=70, Value3=65},
                  new ChartDataModel() { Category = "User Experience", Value1 = 72, Value2=85, Value3=63},
                  new ChartDataModel() { Category = "Innovation", Value1 = 70, Value2=69, Value3=79},
#endif
            };

            // Tooltip
            ExpenditureData = new ObservableCollection<ChartDataModel>()
            {
                new ChartDataModel() { Category = "Travel", Value1 = 25, Value2 = 15 },
                new ChartDataModel() { Category = "Medical", Value1 = 17, Value2 = 28 },
                new ChartDataModel() { Category = "Food", Value1 = 18, Value2 = 15 },
                new ChartDataModel() { Category = "Shopping", Value1 = 25, Value2 = 17 },
                new ChartDataModel() { Category = "Savings", Value1 = 15, Value2 = 25 },
            };

            // DataLabel
            HealthData = new ObservableCollection<ChartDataModel>()
            {
                new ChartDataModel() { Category = "Inflammation", Value1 = 100, Value2 = 53},
                new ChartDataModel() { Category = "Immunity", Value1 = 100, Value2 = 60},
                new ChartDataModel() { Category = "Gut & Digestion", Value1 = 100, Value2 = 75},
                new ChartDataModel() { Category = "Microbiome", Value1 = 100, Value2 = 82 },
                new ChartDataModel() { Category = "Stress", Value1 = 100, Value2 = 40},
                new ChartDataModel() { Category = "Sleep", Value1 = 100, Value2 = 68},
                new ChartDataModel() { Category = "Nutrition", Value1 = 100, Value2 = 72},
                new ChartDataModel() { Category = "Detoxification", Value1 = 100, Value2 = 60},
            };
        }
    }
}