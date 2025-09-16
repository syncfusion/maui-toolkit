namespace Syncfusion.Maui.ControlsGallery.SparkChart
{
	public class ChartTypeViewModel : BaseViewModel
	{
		#region Properties

		public List<Model> LineData { get; set; }
		public List<Model> AreaData { get; set; }
		public List<Model> ColumnData { get; set; }
		public List<Model> WinLossData { get; set; }

		#endregion

		#region Constructor

		public ChartTypeViewModel()
		{
			LineData = new List<Model> {
				new Model(){ Performance = 5000},
				new Model(){ Performance = 9000},
				new Model(){ Performance = 5000},
				new Model(){ Performance = 0},
				new Model(){ Performance = 3000},
				new Model(){ Performance = -4000},
				new Model(){ Performance = 5000},
				new Model(){ Performance = 0},
				new Model(){ Performance = 9000},
				new Model(){ Performance = -9000},
			};

			AreaData = new List<Model> {
				new Model(){ Performance = 3000},
				new Model(){ Performance = -9000},
				new Model(){ Performance = 5000},
				new Model(){ Performance = 0},
				new Model(){ Performance = 3000},
				new Model(){ Performance = 9000},
				new Model(){ Performance = 5000},
				new Model(){ Performance = 0},
				new Model(){ Performance = 9000},
				new Model(){ Performance = 2000},
			};

			ColumnData = new List<Model> {
				new Model(){ Performance = 3000},
				new Model(){ Performance = 9000},
				new Model(){ Performance = -9000},
				new Model(){ Performance = 5000},
				new Model(){ Performance = 7000},
				new Model(){ Performance = -4000},
				new Model(){ Performance = 5000},
				new Model(){ Performance = 1000},
				new Model(){ Performance = 9000},
				new Model(){ Performance = -9000},
			};

			WinLossData = new List<Model> {
				new Model(){ Performance = 9000},
				new Model(){ Performance = 9000},
				new Model(){ Performance = -5000},
				new Model(){ Performance = 6000},
				new Model(){ Performance = 7000},
				new Model(){ Performance = -4000},
				new Model(){ Performance = 5000},
				new Model(){ Performance = 0},
				new Model(){ Performance = 9000},
				new Model(){ Performance = -9000},
			};
		}

		#endregion
	}
}