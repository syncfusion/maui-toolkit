namespace Syncfusion.Maui.ControlsGallery.PullToRefresh.SfPullToRefresh
{
	public partial class PullToRefreshView : SampleView
	{
		public PullToRefreshView()
		{
			InitializeComponent();
			this.Unloaded += PullToRefreshView_Unloaded;
		}

		private void PullToRefreshView_Unloaded(object? sender, EventArgs e)
		{
			pullToRefresh.DisconnectHandlers();
		}
	}
}