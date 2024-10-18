using System.Collections.ObjectModel;

namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
    public class AutoScrollingViewModel : BaseViewModel
    {
        DateTime Date;
        int count;
        readonly Random random = new();
        bool canStopTimer;
        
        public ObservableCollection<ChartDataModel> LiveChartData { get; set; }

        public AutoScrollingViewModel()
        {
            LiveChartData = new ObservableCollection<ChartDataModel>();
            
        }

        private bool UpdateVerticalData()
        {
            if (canStopTimer) return false;
            
            Date = Date.Add(TimeSpan.FromSeconds(1));
            LiveChartData.Add(new ChartDataModel(Date, random.Next(5, 48)));
            count = count + 1;
            return true;
        }

        public void StopTimer()
        {
            canStopTimer = true;
            count = 1;
        }

        public void StartTimer()
        {
            LiveChartData.Clear();
             Date = new DateTime(2019, 1, 1, 01, 00, 00);
            LiveChartData.Add(new ChartDataModel(Date, random.Next(5, 48)));
            LiveChartData.Add(new ChartDataModel(Date.Add(TimeSpan.FromSeconds(1)), random.Next(5, 48)));
            LiveChartData.Add(new ChartDataModel(Date.Add(TimeSpan.FromSeconds(2)), random.Next(5, 48)));
            Date = Date.Add(TimeSpan.FromSeconds(2));
            canStopTimer = false;
            count = 1;
            Application.Current?.Dispatcher.StartTimer(new TimeSpan(0, 0, 0, 0,500), UpdateVerticalData);
        }
    }
}
