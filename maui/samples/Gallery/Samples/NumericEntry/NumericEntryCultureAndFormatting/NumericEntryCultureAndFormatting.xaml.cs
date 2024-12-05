
namespace Syncfusion.Maui.ControlsGallery.NumericEntry.SfNumericEntry;

public partial class NumericEntryCultureAndFormatting : SampleView
{

#if ANDROID || IOS
	readonly NumericEntryCultureAndFormattingMobile _numericEntryCultureAndFormattingMobile;
#else
	readonly NumericEntryCultureAndFormattingDesktop _numericEntryCultureAndFormattingDesktop;
#endif
    public NumericEntryCultureAndFormatting()
	{
		InitializeComponent();
#if ANDROID || IOS
        _numericEntryCultureAndFormattingMobile = new NumericEntryCultureAndFormattingMobile();
        Content = _numericEntryCultureAndFormattingMobile.Content;
        OptionView = _numericEntryCultureAndFormattingMobile.OptionView;
#else
        _numericEntryCultureAndFormattingDesktop = new NumericEntryCultureAndFormattingDesktop();
        Content = _numericEntryCultureAndFormattingDesktop.Content;
        WidthRequest = _numericEntryCultureAndFormattingDesktop.WidthRequest;
        OptionView = _numericEntryCultureAndFormattingDesktop.OptionView;
        Margin = _numericEntryCultureAndFormattingDesktop.Margin;
#endif
    }
}