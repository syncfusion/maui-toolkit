
namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
	public partial class CategoryAxisChart : SampleView
	{
		public CategoryAxisChart()
		{
			InitializeComponent();
		}

		public override void OnDisappearing()
		{
			base.OnDisappearing();
			categoryChart.Handler?.DisconnectHandler();
		}
	}
}
