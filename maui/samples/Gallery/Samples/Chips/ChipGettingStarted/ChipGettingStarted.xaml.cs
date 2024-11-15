
namespace Syncfusion.Maui.ControlsGallery.Chips.SfChip
{

	public partial class ChipGettingStarted : SampleView
	{
		public ChipGettingStarted()
		{
			InitializeComponent();
#if WINDOWS || MACCATALYST
		this.Content=new ChipGettingStartedDesktop();
#elif ANDROID || IOS
			this.Content = new ChipGettingStartedMobile();
#endif
		}
	}
}