namespace Syncfusion.Maui.ControlsGallery.Buttons.SfSegmentedControl
{

	/// <summary>
	/// Provides the view for the Getting Started sample.
	/// </summary>
	public partial class GettingStarted : SampleView
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="GettingStarted"/> class.
		/// </summary>
		public GettingStarted()
		{
			InitializeComponent();
#if WINDOWS || MACCATALYST
			Content = new GettingStartedDesktopUI();
#elif ANDROID || IOS
			Content = new GettingStartedMobileUI();
#endif
		}
	}
}