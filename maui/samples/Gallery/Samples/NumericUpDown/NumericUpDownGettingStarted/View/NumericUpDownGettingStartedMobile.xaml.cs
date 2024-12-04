using Syncfusion.Maui.ControlsGallery.NumericUpDown;

namespace Syncfusion.Maui.ControlsGallery.NumericUpDown.SfNumericUpDown;

public partial class NumericUpDownGettingStartedMobile : SampleView
{
	public NumericUpDownGettingStartedMobile()
	{
		InitializeComponent();
		Syncfusion.Maui.ControlsGallery.NumericUpDown.GettingStartedViewModel viewModel = new Syncfusion.Maui.ControlsGallery.NumericUpDown.GettingStartedViewModel();
        listView.BindingContext = viewModel;
    }
}
    
