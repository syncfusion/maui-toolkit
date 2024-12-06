
namespace Syncfusion.Maui.ControlsGallery.Carousel.Carousel
{
	public partial class GettingStarted : SampleView
	{
#if MACCATALYST || IOS
		bool _isDisappear;
#endif

#if ANDROID || IOS
		readonly GettingStartedMobile _gettingStartedPage;
#elif WINDOWS || MACCATALYST
		readonly GettingStartedDesktop _gettingStartedPage;
#endif

		public GettingStarted()
		{
			InitializeComponent();
#if ANDROID || IOS
			_gettingStartedPage = new GettingStartedMobile();
			Content = _gettingStartedPage;
#elif WINDOWS || MACCATALYST
			_gettingStartedPage = new GettingStartedDesktop();
			Content = _gettingStartedPage;
#endif
#if MACCATALYST || IOS
			_isDisappear = true;
#endif

		}

#if MACCATALYST || IOS
		public override void OnDisappearing()
		{
			if (_isDisappear)
			{
				_gettingStartedPage.GettingStarted_Unloaded();
				_isDisappear = false;
			}
		}
#endif
	}
}
