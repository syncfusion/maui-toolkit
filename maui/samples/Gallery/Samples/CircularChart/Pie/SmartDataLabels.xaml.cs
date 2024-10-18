 using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.ControlsGallery.CircularChart.SfCircularChart
{
    public partial class SmartDataLabels: SampleView
    {
        public SmartDataLabels()
        {
            InitializeComponent();
            dataLabel.SelectedIndex = 0;
        }

        public override void OnAppearing()
        {
            base.OnAppearing();
#if IOS || ANDROID
            if (IsCardView)
            {
                Chart.WidthRequest = 350;
                Chart.HeightRequest = 400;
                Chart.VerticalOptions = LayoutOptions.Start;
            }
#endif
        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();
            Chart.Handler?.DisconnectHandler();
        }

        private void smartDataLabel_SelectedIndexChanged(object sender, EventArgs e)
        {
            var value = dataLabel.SelectedIndex;
            switch (value)
            {
                case 0:
                    dataLabelSetting.SmartLabelAlignment = SmartLabelAlignment.Shift;
                    break;
                case 1:
                    dataLabelSetting.SmartLabelAlignment = SmartLabelAlignment.None;
                    break;
                case 2:
                    dataLabelSetting.SmartLabelAlignment = SmartLabelAlignment.Hide;
                    break;
            }
        }
    }
}
