
namespace Syncfusion.Maui.ControlsGallery.TextInputLayout.SfTextInputLayout
{
	public partial class SignUpPage : SampleView
	{
		public SignUpPage()
		{
			InitializeComponent();
#if ANDROID || IOS
			this.Content = new SignUpPageMobile();
#else
        this.Content = new SignUpPageDesktop();
#endif
		}

	}
}