namespace Syncfusion.Maui.ControlsGallery.NumericUpDown.SfNumericUpDown;

public partial class NumericUpDownGettingStarted : SampleView
{

#if ANDROID || IOS
	readonly NumericUpDownGettingStartedMobile _numericUpDownGettingStartedMobile;
#else
	readonly NumericUpDownGettingStartedDesktop _numericUpDownGettingStartedDesktop;
#endif
    public NumericUpDownGettingStarted()
	{
		InitializeComponent();
#if ANDROID || IOS
        _numericUpDownGettingStartedMobile = new NumericUpDownGettingStartedMobile();
        Content = _numericUpDownGettingStartedMobile.Content;
        OptionView = _numericUpDownGettingStartedMobile.OptionView;
#else
        _numericUpDownGettingStartedDesktop = new NumericUpDownGettingStartedDesktop();
        Content = _numericUpDownGettingStartedDesktop.Content;
        WidthRequest = _numericUpDownGettingStartedDesktop.WidthRequest;
        OptionView = _numericUpDownGettingStartedDesktop.OptionView;
        Margin = _numericUpDownGettingStartedDesktop.Margin;
#endif
    }
}