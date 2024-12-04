namespace Syncfusion.Maui.ControlsGallery.NumericEntry.SfNumericEntry;
public partial class NumericEntryGettingStarted : SampleView
{

#if ANDROID || IOS
	readonly NumericEntryGettingStartedMobile _numericEntryGettingStartedMobile;
#else
	readonly NumericEntryGettingStartedDesktop _numericEntryGettingStartedDesktop;
#endif
    public NumericEntryGettingStarted()
	{
		InitializeComponent();
#if ANDROID || IOS
        _numericEntryGettingStartedMobile = new NumericEntryGettingStartedMobile();
        Content = _numericEntryGettingStartedMobile.Content;
        OptionView = _numericEntryGettingStartedMobile.OptionView;
#else
        _numericEntryGettingStartedDesktop = new NumericEntryGettingStartedDesktop();
        Content = _numericEntryGettingStartedDesktop.Content;
        WidthRequest = _numericEntryGettingStartedDesktop.WidthRequest;
        OptionView = _numericEntryGettingStartedDesktop.OptionView;
        Margin = _numericEntryGettingStartedDesktop.Margin;
#endif
    }
}