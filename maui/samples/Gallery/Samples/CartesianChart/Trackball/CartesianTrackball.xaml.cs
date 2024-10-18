using Syncfusion.Maui.Toolkit.Charts;
using MAUIPicker = Microsoft.Maui.Controls.Picker;

namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
    public partial class CartesianTrackball : SampleView
    {
        public CartesianTrackball()
        {
            InitializeComponent();
        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();
            trackballChart.Handler?.DisconnectHandler();
        }

        private void picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (MAUIPicker)sender;
            int selectedIndex = picker.SelectedIndex;
            if (selectedIndex == 0)
            {
                trackball.DisplayMode = LabelDisplayMode.FloatAllPoints;
                TemplateSwitch.IsEnabled = true;
                label.IsEnabled = true;
                label.Opacity = 1;
            }
            else if (selectedIndex == 1)
            {
                trackball.DisplayMode = LabelDisplayMode.NearestPoint;
                TemplateSwitch.IsEnabled = true;
                label.IsEnabled = true;
                label.Opacity = 1;
            }
            else if (selectedIndex == 2)
            {
                trackball.DisplayMode = LabelDisplayMode.GroupAllPoints;
                TemplateSwitch.IsEnabled = false;
                label.IsEnabled = false;
                label.Opacity = 0.5;
            }
        }

        private void Switch_Toggled(object sender, ToggledEventArgs e)
        {
            var state = e.Value;

            if (state)
            {
                foreach (CartesianSeries series in trackballChart.Series)
                {
                    if (series != null)
                    {
                        if (trackballChart.Resources["trackballLabelTemplate"] is DataTemplate dataTemplate)
                        {
                            series.TrackballLabelTemplate = dataTemplate;
                        }
                    }
                }
            }
            else
            {
                foreach (CartesianSeries series in trackballChart.Series)
                {
                    if (series != null)
                    {
                        series.TrackballLabelTemplate = null!;
                    }
                }
            }
        }


        

        private void trackballChart_TrackballCreated(object sender, TrackballEventArgs e)
        {
            foreach (var info in e.TrackballPointsInfo)
            {
                if (trackball.DisplayMode == LabelDisplayMode.GroupAllPoints)
                {
                    if (!string.IsNullOrEmpty(info.Label))
                    {
                        info.Label = info.Series.Label + " : " + $" {info.Label}M";
                    }
                }
                else if (trackball.DisplayMode != LabelDisplayMode.GroupAllPoints)
                {
                    if (info.Series.TrackballLabelTemplate == null)
                    {
                        if (!string.IsNullOrEmpty(info.Label))
                        {
                            info.Label = $"{info.Label}M";
                        }
                    } 
                } 
            }
        }
    }
}
