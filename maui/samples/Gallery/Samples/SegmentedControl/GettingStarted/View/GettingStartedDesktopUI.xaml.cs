namespace Syncfusion.Maui.ControlsGallery.Buttons.SfSegmentedControl
{

	/// <summary>
	/// Provides the view for the Getting Started sample for the desktop.
	/// </summary>
	public partial class GettingStartedDesktopUI : SampleView
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="GettingStartedDesktopUI"/> class.
		/// </summary>
		public GettingStartedDesktopUI()
		{
			InitializeComponent();
#if WINDOWS
			double density = DeviceDisplay.MainDisplayInfo.Density;
			if (density == 1.0)
			{
				SegmentsContainerGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
				SegmentsContainerGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.15, GridUnitType.Star) });
				return;
			}

			SegmentsContainerGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.85, GridUnitType.Star) });
			SegmentsContainerGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.15, GridUnitType.Star) });
#endif
		}
	}
}