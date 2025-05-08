namespace Syncfusion.Maui.ControlsGallery.PullToRefresh.SfPullToRefresh
{
	public partial class ListViewPullToRefresh : SampleView
	{
		public ListViewPullToRefresh()
		{
			InitializeComponent();			
		}

		public override void OnDisappearing()
		{
			base.OnDisappearing();
			pullToRefresh.Handler?.DisconnectHandler();
		}
	}
}