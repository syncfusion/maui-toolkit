
namespace Syncfusion.Maui.ControlsGallery.NavigationDrawer.NavigationDrawer
{
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
			this.OptionView = gettingStartedMobile.OptionView;
#elif WINDOWS || MACCATALYST
        gettingStartedDesktop = new GettingStartedDesktop();
        this.Content = gettingStartedDesktop.Content;
        this.OptionView = gettingStartedDesktop.OptionView;
#endif
		}
	}
}