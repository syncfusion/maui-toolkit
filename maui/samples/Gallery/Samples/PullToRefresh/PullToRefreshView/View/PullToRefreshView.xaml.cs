namespace Syncfusion.Maui.ControlsGallery.PullToRefresh.SfPullToRefresh
{
	public partial class PullToRefreshView : SampleView
	{
		public PullToRefreshView()
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