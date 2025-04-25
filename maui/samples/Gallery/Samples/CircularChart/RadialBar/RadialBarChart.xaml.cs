using MAUIPicker = Microsoft.Maui.Controls.Picker;

namespace Syncfusion.Maui.ControlsGallery.CircularChart.SfCircularChart
{
	public partial class RadialBarChart : SampleView
	{
		public RadialBarChart()
		{
			InitializeComponent();
			InitializeProperties();
		}

        private void InitializeProperties() 
        {
			capStyle.SelectedIndex = 1;
			trackFill.SelectedIndex = 0;
			trackStroke.SelectedIndex = 0;
			startAngle.Value = -90;
            endAngle.Value = 270;
        }

		public override void OnAppearing()
		{
			base.OnAppearing();
#if IOS
			if (IsCardView)
			{
				chart.WidthRequest = 350;
				chart.HeightRequest = 400;
				chart.VerticalOptions = LayoutOptions.Start;
			}
#endif
		}

		public override void OnDisappearing()
		{
			base.OnDisappearing();
			chart.Handler?.DisconnectHandler();
		}

		private void trackStroke_SelectedIndexChanged(object sender, EventArgs e)
		{
			var value = (MAUIPicker)sender;
			switch (value.SelectedIndex)
			{
				case 0:
					{
						radialBarSeries.TrackStroke = new SolidColorBrush(Color.FromRgba(0, 0, 0, 0.24));
						break;
					}
				case 1:
					{
						radialBarSeries.TrackStroke = new SolidColorBrush(Color.FromRgba("#CBD5E1"));
						break;
					}
				case 2:
					{
						radialBarSeries.TrackStroke = new SolidColorBrush(Color.FromRgba("#BFDBFE"));
						break;
					}
				case 3:
					{
						radialBarSeries.TrackStroke = new SolidColorBrush(Color.FromRgba("#FED7AA"));
						break;
					}
				case 4:
					{
						radialBarSeries.TrackStroke = new SolidColorBrush(Color.FromRgba("#DDD6FE"));
						break;
					}
			}
		}

		private void trackFill_SelectedIndexChanged(object sender, EventArgs e)
		{
			var value = (MAUIPicker)sender;
			switch (value.SelectedIndex)
			{
				case 0:
					{
						radialBarSeries.TrackFill = new SolidColorBrush(Color.FromRgba(0, 0, 0, 0.08));
						break;
					}
				case 1:
					{
						radialBarSeries.TrackFill = new SolidColorBrush(Color.FromRgba("#F1F5F9"));
						break;
					}
				case 2:
					{
						radialBarSeries.TrackFill = new SolidColorBrush(Color.FromRgba("#EFF6FF"));
						break;
					}
				case 3:
					{
						radialBarSeries.TrackFill = new SolidColorBrush(Color.FromRgba("#FFF7ED"));
						break;
					}
				case 4:
					{
						radialBarSeries.TrackFill = new SolidColorBrush(Color.FromRgba("#F5F3FF"));
						break;
					}
			}
		}
	}
}

