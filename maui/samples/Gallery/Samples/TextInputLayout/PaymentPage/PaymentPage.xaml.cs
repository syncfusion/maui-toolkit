namespace Syncfusion.Maui.ControlsGallery.TextInputLayout.SfTextInputLayout
{
	public partial class PaymentPage : SampleView
	{
		public PaymentPage()
		{
			InitializeComponent();
#if ANDROID || IOS
			Content = new PaymentPageMobile();
#else
			Content = new PaymentPageDesktop();
#endif

		}



	}
}