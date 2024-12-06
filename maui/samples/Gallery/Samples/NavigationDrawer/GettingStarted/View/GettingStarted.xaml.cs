namespace Syncfusion.Maui.ControlsGallery.NavigationDrawer.NavigationDrawer
{
	public partial class GettingStarted : SampleView
	{
#if ANDROID || IOS
		readonly GettingStartedMobile _gettingStartedMobile;
#elif WINDOWS || MACCATALYST
		readonly GettingStartedDesktop _gettingStartedDesktop;
#endif
		public GettingStarted()
		{
			InitializeComponent();
#if ANDROID || IOS
			_gettingStartedMobile = new GettingStartedMobile();
			Content = _gettingStartedMobile.Content;
			OptionView = _gettingStartedMobile.OptionView;
#elif WINDOWS || MACCATALYST
			_gettingStartedDesktop = new GettingStartedDesktop();
			Content = _gettingStartedDesktop.Content;
			OptionView = _gettingStartedDesktop.OptionView;
#endif
		}
	}
}