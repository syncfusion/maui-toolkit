
namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
	public partial class RangeColumnAnimation : SampleView
	{
		public RangeColumnAnimation()
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
				Chart.Title = (Label)layout.Resources["title"];
			}

		}

		public override void OnDisappearing()
		{
			base.OnDisappearing();
			viewModel?.StopTimer();

			Chart.Handler?.DisconnectHandler();
		}
	}
}