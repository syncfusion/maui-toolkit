
namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
	public partial class BarChart : SampleView
	{
		private readonly Dictionary<int, Brush?> _originalFills = new();

		public BarChart()
		{
			InitializeComponent();
		}

		public override void OnDisappearing()
		{
			base.OnDisappearing();
			Chart1.Handler?.DisconnectHandler();
		}

		public override void OnAppearing()
		{
			base.OnAppearing();
			hyperLinkLayout.IsVisible = !IsCardView;
		}

		private void gradientCheckBox_CheckedChanged(object? sender, CheckedChangedEventArgs e)
		{
			if (sender is CheckBox cb)
				ChartBrushes.ApplyGradientToggle(barSeries1, this.Resources, paletteIndex: 0, useGradient: cb.IsChecked, _originalFills, cacheIndex: 0);
		}
	}
}
