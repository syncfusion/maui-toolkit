namespace Syncfusion.Maui.ControlsGallery.NavigationDrawer.NavigationDrawer
{
	public partial class GettingStartedDesktop : SampleView
	{
		public GettingStartedDesktop()
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
