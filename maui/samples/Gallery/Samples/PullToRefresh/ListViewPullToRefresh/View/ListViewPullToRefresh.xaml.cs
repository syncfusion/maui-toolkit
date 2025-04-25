namespace Syncfusion.Maui.ControlsGallery.PullToRefresh.SfPullToRefresh
{
	public partial class ListViewPullToRefresh : SampleView
	{
		public ListViewPullToRefresh()
		{
			InitializeComponent();
			this.Unloaded += ListViewPullToRefresh_Unloaded;
		}

		private void ListViewPullToRefresh_Unloaded(object? sender, EventArgs e)
		{
			pullToRefresh.DisconnectHandlers();
		}
	}
}