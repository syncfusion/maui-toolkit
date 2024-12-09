namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
	public partial class StepAreaChart_DynamicUpdate : SampleView
	{
		public StepAreaChart_DynamicUpdate()
		{
			InitializeComponent();
			if (!(BaseConfig.RunTimeDeviceLayout == SBLayout.Mobile))
			{
				viewModel1.StartTimer();
			}
		}

		public override void OnAppearing()
		{
			base.OnAppearing();
			if (BaseConfig.RunTimeDeviceLayout == SBLayout.Mobile)
			{
				viewModel1.StopTimer();
				viewModel1.StartTimer();
			}
		}

		public override void OnDisappearing()
		{
			base.OnDisappearing();
			viewModel1?.StopTimer();

			Chart1.Handler?.DisconnectHandler();
		}
	}
}