using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
	public partial class TapToAdd : SampleView
	{

		public TapToAdd()
		{
			InitializeComponent();
		}

		public override void OnAppearing()
		{
			base.OnAppearing();
		}

		public override void OnDisappearing()
		{
			base.OnDisappearing();
			categoryChart.Handler?.DisconnectHandler();
		}

		private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
		{
			realTimeViewModel?.ResetChartData();
		}
	}

	public partial class ChartInteractionExt : ChartInteractiveBehavior
	{
		protected override void OnTouchUp(ChartBase chart, float pointX, float pointY)
		{
			base.OnTouchUp(chart, pointX, pointY);

			if (chart is Syncfusion.Maui.Toolkit.Charts.SfCartesianChart cartesianChart)
			{
				var x = cartesianChart.PointToValue(cartesianChart.XAxes[0], pointX, pointY);
				var y = cartesianChart.PointToValue(cartesianChart.YAxes[0], pointX, pointY);

				if (cartesianChart.BindingContext is TapToAddViewModel viewModel)
				{
					viewModel.LiveChartData.Add(new ChartDataModel(x, y));
				}
			}
		}
	}

}
