
namespace Syncfusion.Maui.ControlsGallery.Chips.SfChip
{

	public partial class ChipGettingStarted : SampleView
	{
		public ChipGettingStarted()
		{
			InitializeComponent();
#if WINDOWS || MACCATALYST
			Content = new ChipGettingStartedDesktop();
#elif ANDROID || IOS
			Content = new ChipGettingStartedMobile();
#endif
		}
	}
}