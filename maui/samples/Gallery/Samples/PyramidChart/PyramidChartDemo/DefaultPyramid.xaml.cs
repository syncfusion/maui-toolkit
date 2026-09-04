namespace Syncfusion.Maui.ControlsGallery.PyramidChart.SfPyramidChart
{
	public partial class DefaultPyramid : SampleView
	{
		public DefaultPyramid()
		{
			InitializeComponent();
		}

		public override void OnDisappearing()
		{
			base.OnDisappearing();
			Chart.Handler?.DisconnectHandler();
		}

		private void Inversed_CheckedChanged(object? sender, Microsoft.Maui.Controls.CheckedChangedEventArgs e)
		{
			if (sender is Microsoft.Maui.Controls.CheckBox checkBox)
			{
				bool isChecked = e.Value;
				Chart.Orientation = isChecked
					? Syncfusion.Maui.Toolkit.Charts.ChartOrientation.Horizontal
					: Syncfusion.Maui.Toolkit.Charts.ChartOrientation.Vertical;
			}
		}
	}
}
