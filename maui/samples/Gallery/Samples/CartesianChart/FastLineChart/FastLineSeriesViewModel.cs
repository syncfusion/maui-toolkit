using System.Collections.ObjectModel;

namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
    public class FastLineSeriesViewModel : BaseViewModel
    {
        public int DataCount = 100000;
        readonly Random randomNumber;

        public ObservableCollection<ChartDataModel> Data { get; set; }

        public FastLineSeriesViewModel()
        {
            randomNumber = new Random();
            Data = GenerateData();
        }

        private ObservableCollection<ChartDataModel> GenerateData()
        {
            ObservableCollection<ChartDataModel> collection = new();
            DateTime date = new(1900, 1, 1);
            double value = 100;

            for (int i = 0; i < this.DataCount; i++)
            {
                collection.Add(new ChartDataModel(date, Math.Round(value, 2)));
                date = date.Add(TimeSpan.FromHours(6));

                if (randomNumber.NextDouble() > 0.5)
                {
                    value += randomNumber.NextDouble();
                }
                else
                {
                    value -= randomNumber.NextDouble();
                }
            }

            return collection;
        }

    }

    public class RealTimeVerticalChartViewModel : BaseViewModel
    {
        int count;
        int index;
        readonly Random random = new();

        public ObservableCollection<ChartDataModel> VerticalLiveChartData { get; set; }

        public RealTimeVerticalChartViewModel()
        {
            VerticalLiveChartData = new ObservableCollection<ChartDataModel>();
            count = 0;
        }

        private bool UpdateVerticalData()
        {
            count = count + 1;

            if (count > 165)
            {
                return false;
            }
            else if (count > 150)
            {
                VerticalLiveChartData.Add(new ChartDataModel(index, random.Next(0, 0)));
            }
            else if (count > 120)
            {
                VerticalLiveChartData.Add(new ChartDataModel(index, random.Next(-2, 1)));
            }
            else if (count > 80)
            {
                VerticalLiveChartData.Add(new ChartDataModel(index, random.Next(-3, 2)));
            }
            else if (count > 25)
            {
                VerticalLiveChartData.Add(new ChartDataModel(index, random.Next(-7, 6)));
            }
            else
            {
                VerticalLiveChartData.Add(new ChartDataModel(index, random.Next(-9, 9)));
            }
            index++;
            return true;
        }

        public void StartVerticalTimer()
        {
            VerticalLiveChartData.Clear();
            count = VerticalLiveChartData.Count;
            Application.Current?.Dispatcher.StartTimer(new TimeSpan(0, 0, 0, 0, 10), UpdateVerticalData);
        }
    }

    public class RealTimeChartViewModel : BaseViewModel
    {
        bool canStopTimer;
        static int count = 0;
        static int value = 0;
        readonly float[] datas1 = new float[]
        {
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
        };

        public ObservableCollection<ChartDataModel> LiveData { get; set; }

        public RealTimeChartViewModel()
        {
            LiveData = new ObservableCollection<ChartDataModel>();
            if (BaseConfig.RunTimeDeviceLayout == SBLayout.Mobile)
            {
                UpdateLiveData();
            }
        }

        private void UpdateLiveData()
        {
            for (int i = 0; i < 500; i++)
            {
                if (count >= datas1.Length)
                    count = 0;
                LiveData.Add(new ChartDataModel(value, datas1[count]));
                count++;
                value++;
            }
        }

        private bool UpdateData()
        {
            if (canStopTimer) return false;

            if (count >= datas1.Length)
                count = 0;

            value = LiveData.Count >= 1 ? (int)(LiveData[LiveData.Count - 1].Value) + 1 : 1;

            if (value > 250)
            {
                LiveData.RemoveAt(0);
            }

            LiveData.Add(new ChartDataModel(value, datas1[count]));
            count++;
            return true;
        }

        public void StopTimer()
        {
            canStopTimer = true;

            if (!(BaseConfig.RunTimeDeviceLayout == SBLayout.Mobile))
            {
                value = 0;
                count = 0;
                LiveData.Clear();
                UpdateLiveData();
            }
        }
        public async void StartTimer()
        {
            await Task.Delay(500);

            Application.Current?.Dispatcher.StartTimer(new TimeSpan(0, 0, 0, 0, 25), UpdateData);

            canStopTimer = false;
        }
    }
}
