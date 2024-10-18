#region Copyright Syncfusion Inc. 2001-2022.
// Copyright Syncfusion Inc. 2001-2022. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using Syncfusion.Maui.Toolkit.Charts;
using MAUIPicker = Microsoft.Maui.Controls.Picker;

namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
    public partial class ErrorBarChart : SampleView
    {
        #region Constructor

        #region  Public Constructor

        public ErrorBarChart()
        {
            InitializeComponent();
        }

        #endregion

        #endregion

        #region Methods


        #region  Private Methods

        public override void OnAppearing()
        {
            base.OnAppearing();
            if(!IsCardView)
            {
                errorBarChart.Title = (Label)layout.Resources["title"];
                xAxis.Title = new ChartAxisTitle() { Text = "Country Code" }; 
                yAxis.Title = new ChartAxisTitle() { Text = "Sales Percentage" };
            }
        }

        private void typePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (MAUIPicker)sender;
            int selectedIndex = picker.SelectedIndex;
            if (selectedIndex == 0)
            {
                errorBar.Type = ErrorBarType.Fixed;
            }
            else if (selectedIndex == 1)
            {
                errorBar.Type = ErrorBarType.Percentage;
            }
            else if (selectedIndex == 2)
            {
                errorBar.Type = ErrorBarType.StandardError;
            }
            else if (selectedIndex == 3)
            {
                errorBar.Type = ErrorBarType.StandardDeviation;
            }
        }

        private void modePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (MAUIPicker)sender;
            int selectedIndex = picker.SelectedIndex;
            if (selectedIndex == 0)
            {
                errorBar.Mode = ErrorBarMode.Vertical;
                horStepper.IsEnabled = false;
                verStepper.IsEnabled = true;
            
            }
            else if (selectedIndex == 1)
            {
                errorBar.Mode = ErrorBarMode.Horizontal;
                horStepper.IsEnabled = true;
                verStepper.IsEnabled = false;
                
            }
            else
            {
                errorBar.Mode = ErrorBarMode.Both;
                horStepper.IsEnabled = true;
                verStepper.IsEnabled = true;
            }
        }

        private void directionPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (MAUIPicker)sender;
            int selectedIndex = picker.SelectedIndex;
            if (selectedIndex == 0)
            {
                if (errorBar.Mode == ErrorBarMode.Horizontal)
                {
                    errorBar.HorizontalDirection = ErrorBarDirection.Both;
                }
                else if (errorBar.Mode == ErrorBarMode.Vertical)
                {
                    errorBar.VerticalDirection = ErrorBarDirection.Both;
                }
                else
                {
                    errorBar.HorizontalDirection = ErrorBarDirection.Both;
                    errorBar.VerticalDirection = ErrorBarDirection.Both;
                }
            }
            else if (selectedIndex == 1)
            {
                if (errorBar.Mode == ErrorBarMode.Horizontal)
                {
                    errorBar.HorizontalDirection = ErrorBarDirection.Plus;
                }
                else if (errorBar.Mode == ErrorBarMode.Vertical)
                {
                    errorBar.VerticalDirection = ErrorBarDirection.Plus;
                }
                else if (errorBar.Mode == ErrorBarMode.Both)
                {
                    errorBar.HorizontalDirection = errorBar.VerticalDirection = ErrorBarDirection.Plus;
                }
            }
            else
            {
                if (errorBar.Mode == ErrorBarMode.Horizontal)
                {
                    errorBar.HorizontalDirection = ErrorBarDirection.Minus;
                }
                if (errorBar.Mode == ErrorBarMode.Vertical)
                {
                    errorBar.VerticalDirection = ErrorBarDirection.Minus;
                }
                if (errorBar.Mode == ErrorBarMode.Both)
                {
                    errorBar.HorizontalDirection = errorBar.VerticalDirection = ErrorBarDirection.Minus;
                }
            }
        }

  

        #endregion

        #endregion

    }
}
