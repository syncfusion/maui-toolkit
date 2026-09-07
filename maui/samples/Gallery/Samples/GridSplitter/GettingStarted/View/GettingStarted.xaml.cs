namespace Syncfusion.Maui.ControlsGallery.GridSplitter.SfGridSplitter;

public partial class GettingStarted : SampleView
{
#if ANDROID || IOS
	GettingStartedMobile gettingStartedMobile;
#elif WINDOWS || MACCATALYST
    GettingStartedDesktop gettingStartedDesktop;
#endif
	public GettingStarted()
	{
		InitializeComponent();
#if ANDROID || IOS
		gettingStartedMobile = new GettingStartedMobile();
		this.Content = gettingStartedMobile.Content;
#elif WINDOWS || MACCATALYST
        gettingStartedDesktop = new GettingStartedDesktop();
        this.Content = gettingStartedDesktop.Content;
#endif
	}
}