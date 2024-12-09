namespace Syncfusion.Maui.ControlsGallery.Chips.SfChip
{
	public partial class ChipCustomization : SampleView
	{
		public ChipCustomization()
		{
			InitializeComponent();
#if ANDROID || IOS
			Content = new ChipCustomizationMobile();
#elif WINDOWS || MACCATALYST
			Content = new ChipCustomizationDesktop();
#endif
		}
	}
}