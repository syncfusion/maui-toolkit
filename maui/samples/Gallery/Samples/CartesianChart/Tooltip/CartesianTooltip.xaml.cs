
namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
	public partial class CartesianTooltip : SampleView
	{
		public CartesianTooltip()
		{
			InitializeComponent();
		}

		public override void OnAppearing()
		{
			base.OnAppearing();
			hyperLinkLayout.IsVisible = !IsCardView;
		}

		public override void OnDisappearing()
		{
			base.OnDisappearing();
			tooltipChart.Handler?.DisconnectHandler();
		}
	}
}
