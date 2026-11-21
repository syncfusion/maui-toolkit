using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
	public partial class DefaultRangeColumn : SampleView
	{
		private readonly Dictionary<int, Brush?> _originalFills = new();

		public DefaultRangeColumn()
		{
			InitializeComponent();
		}

		public override void OnAppearing()
		{
			base.OnAppearing();
			hyperLinkLayout.IsVisible = !IsCardView;
			if (!IsCardView)
			{
				Chart.Title = (Label)layout.Resources["title"];
				yAxis.Title = new ChartAxisTitle() { Text = "Temperature [°C]" };
			}
		}

		public override void OnDisappearing()
		{
			base.OnDisappearing();
			Chart.Handler?.DisconnectHandler();
		}

		private void gradientCheckBox_CheckedChanged(object? sender, CheckedChangedEventArgs e)
		{
			if (sender is CheckBox cb)
				ChartBrushes.ApplyGradientToggle(rangeColumnSeries1, this.Resources, paletteIndex: 0, useGradient: cb.IsChecked, _originalFills, cacheIndex: 0);
		}
	}
}
