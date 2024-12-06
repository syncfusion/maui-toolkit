
namespace Syncfusion.Maui.ControlsGallery.Chips.SfChip
{
	public partial class ChipTypes : SampleView
	{
		public ChipTypes()
		{
			InitializeComponent();
#if WINDOWS || MACCATALYST
			Content = new ChipTypesDesktop();
#elif ANDROID || IOS
			Content = new ChipTypesMobile();
#endif
		}

	}
}