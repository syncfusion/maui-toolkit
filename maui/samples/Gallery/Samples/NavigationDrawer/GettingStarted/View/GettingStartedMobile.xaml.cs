namespace Syncfusion.Maui.ControlsGallery.NavigationDrawer.NavigationDrawer
{
	public partial class GettingStartedMobile : SampleView
	{
		public GettingStartedMobile()
		{
			InitializeComponent();
		}
		private void TapGestureRecognizer_Tapped(object? sender, TappedEventArgs e)
		{
			navigationDrawer.ToggleSecondaryDrawer();
		}

		private void CloseIcon_Tapped(object? sender, TappedEventArgs e)
		{
			navigationDrawer.SecondaryDrawerSettings.IsOpen = false;
		}
	}
}
