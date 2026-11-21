namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
	public partial class LineChart : SampleView
	{
		private Dictionary<int, Brush?> _originalFills = new();

		public LineChart()
		{
			InitializeComponent();
		}

		public override void OnAppearing()
		{
			base.OnAppearing();
#if IOS
			if (IsCardView)
			{
				Chart.WidthRequest = 350;
				Chart.HeightRequest = 400;
				Chart.VerticalOptions = LayoutOptions.Start;
			}
#endif
		}

		public override void OnDisappearing()
		{
			base.OnDisappearing();
			Chart.Handler?.DisconnectHandler();
		}

		private void gradientCheckBox_CheckedChanged(object? sender, CheckedChangedEventArgs e)
		{
			if (sender is CheckBox cb)
			{
				ChartBrushes.ApplyLineGradientToggle(lineSeries1, this.Resources, paletteIndex: 0, useGradient: cb.IsChecked, _originalFills, cacheIndex: 0);
				ChartBrushes.ApplyLineGradientToggle(lineSeries2, this.Resources, paletteIndex: 1, useGradient: cb.IsChecked, _originalFills, cacheIndex: 1);
			}
		}
	}
}
