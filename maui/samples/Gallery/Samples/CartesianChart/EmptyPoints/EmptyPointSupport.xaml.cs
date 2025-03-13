
using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
	public partial class EmptyPointSupport : SampleView
	{
		public EmptyPointSupport()
		{
			InitializeComponent();
		}

		private void picker_SelectedIndexChanged(object sender, EventArgs e)
		{
			var picker = (Picker)sender;
			int selectedIndex = picker.SelectedIndex;
			switch (selectedIndex)
			{
				case 1:
					ViewModel.EmptyPointMode = EmptyPointMode.Zero;
					break;

				case 2:
					ViewModel.EmptyPointMode = EmptyPointMode.Average;
					break;

				default:
					ViewModel.EmptyPointMode = EmptyPointMode.None;
					break;
			}
		}

		public override void OnDisappearing()
		{
			base.OnDisappearing();
			Chart.Handler?.DisconnectHandler();
		}
	}
}
