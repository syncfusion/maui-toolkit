using System.Collections.ObjectModel;

namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
	public partial class DateTimeAxisViewModel : BaseViewModel
	{
		public ObservableCollection<Production> DateTimeData { get; set; }

		public DateTimeAxisViewModel()
		{
			DateTimeData = [];

			Random rand = new();
			double value = 100;
			DateTime date = new(2017, 1, 1);

			for (int i = 0; i < 100; i++)
			{
				if (rand.NextDouble() > 0.5)
				{
					value += rand.NextDouble();
				}
				else
				{
					value -= rand.NextDouble();
				}

				DateTimeData.Add(new Production { Growth = value, Date = date });
				date = date.AddDays(1);

			}
		}
	}

	public class Production
	{
		public double Growth
		{
			get;
			set;
		}

		public DateTime Date
		{
			get;
			set;
		}
	}
}
