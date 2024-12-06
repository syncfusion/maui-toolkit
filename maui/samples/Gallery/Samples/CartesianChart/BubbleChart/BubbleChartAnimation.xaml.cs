namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
	public partial class BubbleChartAnimation : SampleView
	{
		public BubbleChartAnimation()
		{
			InitializeComponent();
			if (!(BaseConfig.RunTimeDeviceLayout == SBLayout.Mobile))
			{
				viewModel.StartTimer();
			}
		}

		public override void OnAppearing()
		{
			base.OnAppearing();
			if (BaseConfig.RunTimeDeviceLayout == SBLayout.Mobile)
			{
				viewModel.StopTimer();
				viewModel.StartTimer();
			}

			if (!IsCardView)
			{
				bubbleChart.Title = (Label)layout.Resources["title"];
			}
		}

		public override void OnDisappearing()
		{
			base.OnDisappearing();
			viewModel?.StopTimer();

			bubbleChart.Handler?.DisconnectHandler();
		}
	}
}