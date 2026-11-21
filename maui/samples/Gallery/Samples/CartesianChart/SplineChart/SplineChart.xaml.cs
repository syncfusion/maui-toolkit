
namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
	public partial class SplineChart : SampleView
	{
		private Dictionary<int, Brush?> _originalFills = new();

		public SplineChart()
		{
			InitializeComponent();
		}

		public override void OnAppearing()
		{
			base.OnAppearing();
			hyperLinkLayout.IsVisible = !IsCardView;
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
				ChartBrushes.ApplyLineGradientToggle(splineSeries1, this.Resources, paletteIndex: 0, useGradient: cb.IsChecked, _originalFills, cacheIndex: 0);
				ChartBrushes.ApplyLineGradientToggle(splineSeries2, this.Resources, paletteIndex: 1, useGradient: cb.IsChecked, _originalFills, cacheIndex: 1);
			}
		}
	}
}
