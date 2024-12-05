using Syncfusion.Maui.ControlsGallery.NumericUpDown;

namespace Syncfusion.Maui.ControlsGallery.NumericUpDown.SfNumericUpDown;

public partial class NumericUpDownGettingStartedDesktop : SampleView
{
	public NumericUpDownGettingStartedDesktop()
	{
		InitializeComponent();
		GettingStartedViewModel viewModel = new GettingStartedViewModel();
		listView.BindingContext = viewModel;
	}
}