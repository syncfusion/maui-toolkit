
namespace Syncfusion.Maui.ControlsGallery.Chips.SfChip
{
	public partial class ChipCustomization : SampleView
	{
		public ChipCustomization()
		{
			InitializeComponent();
#if ANDROID || IOS
			this.Content = new ChipCustomizationMobile();
#elif WINDOWS || MACCATALYST
        this.Content=new ChipCustomizationDesktop();
#endif
		}
	}
}