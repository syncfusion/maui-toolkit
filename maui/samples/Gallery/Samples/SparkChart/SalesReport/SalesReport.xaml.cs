namespace Syncfusion.Maui.ControlsGallery.SparkChart.SfSparkChart;

public partial class SalesReport : SampleView
{
#if ANDROID || IOS
	ScrollView? scrollview;
#endif

	public SalesReport()
	{
		InitializeComponent();

#if ANDROID || IOS
		scrollview = new ScrollView()
		{ 
			Orientation = ScrollOrientation.Horizontal,
			HorizontalScrollBarVisibility = ScrollBarVisibility.Never
		};
		scrollview.Content = MainGrid;
		Content = scrollview;
#endif
	}

	public override void OnDisappearing()
	{
		base.OnDisappearing();
		MainGrid.Handler?.DisconnectHandler();
	}
}