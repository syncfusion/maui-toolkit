using System.Collections.ObjectModel;

namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
	public partial class AutoScrollingViewModel : BaseViewModel
	{
		DateTime _date;
		int _count;
		readonly Random _random = new();
		bool _canStopTimer;

		public ObservableCollection<ChartDataModel> LiveChartData { get; set; }

		public AutoScrollingViewModel()
		{
			LiveChartData = [];

		}

		private bool UpdateVerticalData()
		{
			if (_canStopTimer)
			{
				return false;
			}

			_date = _date.Add(TimeSpan.FromSeconds(1));
			LiveChartData.Add(new ChartDataModel(_date, _random.Next(5, 48)));
			_count++;
			return true;
		}

		public void StopTimer()
		{
			_canStopTimer = true;
			_count = 1;
		}

		public void StartTimer()
		{
			LiveChartData.Clear();
			_date = new DateTime(2019, 1, 1, 01, 00, 00);
			LiveChartData.Add(new ChartDataModel(_date, _random.Next(5, 48)));
			LiveChartData.Add(new ChartDataModel(_date.Add(TimeSpan.FromSeconds(1)), _random.Next(5, 48)));
			LiveChartData.Add(new ChartDataModel(_date.Add(TimeSpan.FromSeconds(2)), _random.Next(5, 48)));
			_date = _date.Add(TimeSpan.FromSeconds(2));
			_canStopTimer = false;
			_count = 1;
			Application.Current?.Dispatcher.StartTimer(new TimeSpan(0, 0, 0, 0, 500), UpdateVerticalData);
		}
	}
}
