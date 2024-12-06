namespace Syncfusion.Maui.ControlsGallery.TextInputLayout.SfTextInputLayout
{
	public partial class SignUpPage : SampleView
	{
		public SignUpPage()
		{
			InitializeComponent();
#if ANDROID || IOS
			Content = new SignUpPageMobile();
#else
			Content = new SignUpPageDesktop();
#endif
		}

	}
}