
namespace Syncfusion.Maui.ControlsGallery.Chips.SfChip;

public partial class ChipTypes : SampleView
{
	public ChipTypes()
	{
		InitializeComponent();
#if WINDOWS || MACCATALYST
		this.Content=new ChipTypesDesktop();
#elif ANDROID || IOS
		this.Content=new ChipTypesMobile();
#endif
	}

}