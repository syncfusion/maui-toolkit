using System.ComponentModel;

namespace Syncfusion.Maui.ControlsGallery.SparkChart
{
	public class BaseViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler? PropertyChanged;

		public void OnPropertyChanged(string name)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}

		public static List<Model> GetRandomDoubleList(int? start = 1, int? end = 10)
		{
			Random rng = new Random();
			List<Model> randomNumbers = new List<Model>();

			for (int i = 0; i < 12; i++)
			{
				if (start != null && end != null)
				{
					double value = rng.Next((int)start, (int)end); // Generates integer between 1 and 9
					randomNumbers.Add(new Model() { Performance = (double)value * 1000 });
				}
			}

			return randomNumbers;

		}

		public static List<Model> GetWeeklyDataList(int? start = 1, int? end = 10)
		{
			Random rng = new Random();
			List<Model> randomNumbers = new List<Model>();

			for (int i = 0; i < 7; i++)
			{
				if (start != null && end != null)
				{
					double value = rng.Next((int)start, (int)end); // Generates integer between 1 and 9
					randomNumbers.Add(new Model() { Performance = (double)value * 1000 });
				}
			}

			return randomNumbers;

		}
	}
}