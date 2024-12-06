using System.Collections.ObjectModel;

namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
	public partial class FastLineSeriesViewModel : BaseViewModel
	{
		public int DataCount = 100000;
		readonly Random _randomNumber;

		public ObservableCollection<ChartDataModel> Data { get; set; }

		public FastLineSeriesViewModel()
		{
			_randomNumber = new Random();
			Data = GenerateData();
		}

		private ObservableCollection<ChartDataModel> GenerateData()
		{
			ObservableCollection<ChartDataModel> collection = [];
			DateTime date = new(1900, 1, 1);
			double value = 100;

			for (int i = 0; i < DataCount; i++)
			{
				collection.Add(new ChartDataModel(date, Math.Round(value, 2)));
				date = date.Add(TimeSpan.FromHours(6));

				if (_randomNumber.NextDouble() > 0.5)
				{
					value += _randomNumber.NextDouble();
				}
				else
				{
					value -= _randomNumber.NextDouble();
				}
			}

			return collection;
		}

	}

	public partial class RealTimeVerticalChartViewModel : BaseViewModel
	{
		int _count;
		int _index;
		readonly Random _random = new();

		public ObservableCollection<ChartDataModel> VerticalLiveChartData { get; set; }

		public RealTimeVerticalChartViewModel()
		{
			VerticalLiveChartData = [];
			_count = 0;
		}

		private bool UpdateVerticalData()
		{
			_count++;

			if (_count > 165)
			{
				return false;
			}
			else if (_count > 150)
			{
				VerticalLiveChartData.Add(new ChartDataModel(_index, _random.Next(0, 0)));
			}
			else if (_count > 120)
			{
				VerticalLiveChartData.Add(new ChartDataModel(_index, _random.Next(-2, 1)));
			}
			else if (_count > 80)
			{
				VerticalLiveChartData.Add(new ChartDataModel(_index, _random.Next(-3, 2)));
			}
			else if (_count > 25)
			{
				VerticalLiveChartData.Add(new ChartDataModel(_index, _random.Next(-7, 6)));
			}
			else
			{
				VerticalLiveChartData.Add(new ChartDataModel(_index, _random.Next(-9, 9)));
			}
			_index++;
			return true;
		}

		public void StartVerticalTimer()
		{
			VerticalLiveChartData.Clear();
			_count = VerticalLiveChartData.Count;
			Application.Current?.Dispatcher.StartTimer(new TimeSpan(0, 0, 0, 0, 10), UpdateVerticalData);
		}
	}

	public partial class RealTimeChartViewModel : BaseViewModel
	{
		bool _canStopTimer;
		static int Count;
		static int Value;
		readonly float[] _datas1 =
		[
			762,772,762,772,772,770,766,763,765,772,763,768,764,772,762,
			766,768,766,762,772,774,766,770,767,777,772,762,772,765,766,
			762,766,766,770,768,765,772,766,766,766,772,774,771,774,769,
			780,780,777,788,794,778,775,777,783,786,775,765,780,770,770,
			770,772,771,770,772,780,771,770,766,787,788,775,780,779,780,
			784,774,790,774,779,788,788,774,791,786,788,758,779,786,777,
			764,799,788,823,784,642,783,804,703,784,790,823,806,834,816,
			760,608,804,809,786,810,794,836,801,844,798,823,812,828,835,
			818,819,811,806,820,828,811,810,812,813,806,784,825,805,830,
			819,826,802,835,1023,1001,1023,1019,1023,990,879,939,812,852,818,802,854,818,820,
			806,852,809,800,811,794,800,808,812,812,816,827,850,831,812,819,820,780,810,
		];

		public ObservableCollection<ChartDataModel> LiveData { get; set; }

		public RealTimeChartViewModel()
		{
			LiveData = [];
			if (BaseConfig.RunTimeDeviceLayout == SBLayout.Mobile)
			{
				UpdateLiveData();
			}
		}

		private void UpdateLiveData()
		{
			for (int i = 0; i < 500; i++)
			{
				if (Count >= _datas1.Length)
				{
					Count = 0;
				}

				LiveData.Add(new ChartDataModel(Value, _datas1[Count]));
				Count++;
				Value++;
			}
		}

		private bool UpdateData()
		{
			if (_canStopTimer)
			{
				return false;
			}

			if (Count >= _datas1.Length)
			{
				Count = 0;
			}

			Value = LiveData.Count >= 1 ? (int)(LiveData[^1].Value) + 1 : 1;

			if (Value > 250)
			{
				LiveData.RemoveAt(0);
			}

			LiveData.Add(new ChartDataModel(Value, _datas1[Count]));
			Count++;
			return true;
		}

		public void StopTimer()
		{
			_canStopTimer = true;

			if (!(BaseConfig.RunTimeDeviceLayout == SBLayout.Mobile))
			{
				Value = 0;
				Count = 0;
				LiveData.Clear();
				UpdateLiveData();
			}
		}
		public async void StartTimer()
		{
			await Task.Delay(500);

			Application.Current?.Dispatcher.StartTimer(new TimeSpan(0, 0, 0, 0, 25), UpdateData);

			_canStopTimer = false;
		}
	}
}
