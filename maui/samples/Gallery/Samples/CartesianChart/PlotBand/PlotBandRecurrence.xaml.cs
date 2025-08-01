namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
    public partial class PlotBandRecurrence : SampleView
    {
        public PlotBandRecurrence()
        {
            InitializeComponent();
			InitializePlatBandValues();

		}

		#region InitializePlatBandValues

		private void InitializePlatBandValues()
		{
			dateTimePlotBand.IsVisible = false;
			numericalPlotBand.IsVisible = true;
		}
		
		#endregion

		public override void OnDisappearing()
        {
            base.OnDisappearing();
            plotBandRecurrenceChart.Handler?.DisconnectHandler();
        }

		private void axis_CheckedChanged(object sender, CheckedChangedEventArgs e)
		{
			var checkBox = (CheckBox)sender;
			if (ViewModel != null)
			{
				var input = checkBox.StyleId;
				switch (input)
				{
					case "xAxis":
						dateTimePlotBand.IsVisible = checkBox.IsChecked;
						break;
					case "yAxis":
						numericalPlotBand.IsVisible = checkBox.IsChecked;
						break;

				}
			}
		}
	}
}