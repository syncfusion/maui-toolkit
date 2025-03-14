namespace Syncfusion.Maui.ControlsGallery.OtpInput.OtpInput;

public partial class SignUpPage : SampleView
{
	public SignUpPage()
	{
		InitializeComponent();
#if ANDROID || IOS
		Content = new SignUpMobile();
#elif WINDOWS || MACCATALYST
		Content = new SignUpDesktop();
#endif
	}
}