using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.ControlsGallery.CircularChart.SfCircularChart
{
	public partial class GroupToDoughnutChart : SampleView
	{
		public GroupToDoughnutChart()
		{
			InitializeComponent();
			groupTo.SelectedIndex = 0;
		}

		public override void OnAppearing()
		{
			base.OnAppearing();
			doughnutSeries1.EnableTooltip = !IsCardView;
			hyperLinkLayout.IsVisible = !IsCardView;
#if IOS || ANDROID
			if (!IsCardView)
			{
				doughnutSeries1.Radius = 0.6;
				doughnutSeries1.DataLabelSettings.LabelPlacement = DataLabelPlacement.Auto;
			}
#endif


#if IOS
			if (IsCardView)
			{
				Chart1.WidthRequest = 350;
				Chart1.HeightRequest = 400;
				Chart1.VerticalOptions = LayoutOptions.Start;
			}
#endif
		}

		public override void OnDisappearing()
		{
			base.OnDisappearing();
			Chart1.Handler?.DisconnectHandler();
		}

		private void groupTo_SelectedIndexChanged(object sender, EventArgs e)
		{

			var index = groupTo.SelectedIndex;
			if (index != -1)
			{
				switch (index)
				{
					case 0:
						{
							doughnutSeries1.GroupTo = 5;
							doughnutSeries1.YBindingPath = "Value";
							doughnutSeries1.GroupMode = PieGroupMode.Value;
							label.LabelFormat = "$#.##'T";
							break;
						}
					case 1:
						{
							doughnutSeries1.YBindingPath = "Size";
							doughnutSeries1.GroupTo = 10;
							doughnutSeries1.GroupMode = PieGroupMode.Percentage;
							label.LabelFormat = "P0";
							break;
						}
					case 2:
						{
							doughnutSeries1.GroupTo = 90;
							doughnutSeries1.YBindingPath = "Value";
							doughnutSeries1.GroupMode = PieGroupMode.Angle;
							label.LabelFormat = "$#.##'T";
							break;
						}
				}
			}
		}
	}
}