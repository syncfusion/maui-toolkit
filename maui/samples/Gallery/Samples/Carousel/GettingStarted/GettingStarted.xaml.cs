
namespace Syncfusion.Maui.ControlsGallery.Carousel.Carousel
{
    public partial class GettingStarted : SampleView
    {
#if MACCATALYST || IOS
		bool isDisappear = false;
#endif

#if ANDROID || IOS
		GettingStartedMobile GettingStartedPage;
#elif WINDOWS || MACCATALYST
        GettingStartedDesktop GettingStartedPage;
#endif

		public GettingStarted()
        {
            InitializeComponent();
#if ANDROID || IOS
			GettingStartedPage = new GettingStartedMobile();
			this.Content = GettingStartedPage;
#elif WINDOWS || MACCATALYST
			GettingStartedPage = new GettingStartedDesktop();
			this.Content = GettingStartedPage;
#endif
#if MACCATALYST || IOS
			isDisappear = true;
#endif

		}

#if MACCATALYST || IOS
		public override void OnDisappearing()
		{
			if (isDisappear)
			{
				GettingStartedPage.GettingStarted_Unloaded();
				isDisappear = false;
			}
		}
#endif
	}
}
