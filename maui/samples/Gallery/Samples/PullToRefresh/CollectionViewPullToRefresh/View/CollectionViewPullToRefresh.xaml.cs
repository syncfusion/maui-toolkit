namespace Syncfusion.Maui.ControlsGallery.PullToRefresh.SfPullToRefresh
{
	public partial class CollectionViewPullToRefresh : SampleView
	{
		public CollectionViewPullToRefresh()
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