namespace Syncfusion.Maui.ControlsGallery.Buttons.Button;

public partial class Customizations : SampleView
{
    public Customizations()
	{
		InitializeComponent();
#if ANDROID || IOS
        Content = new CustomizationsMobile();
#else
        Content = new CustomizationsDesktop();
#endif
    }
}