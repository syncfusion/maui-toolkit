using Syncfusion.Maui.Toolkit.Charts;
using System.Collections.ObjectModel;

namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
    public class PlotBandViewModel : BaseViewModel
    {
        public string[] PlotBandType => new[] { "Fill", "Line", "Segment" };
        public ObservableCollection<ChartDataModel> ClimateData { get; set; }
        public NumericalPlotBandCollection NumericalPlotBandFillCollection { get; set; }
        public NumericalPlotBandCollection NumericalPlotBandLineCollection { get; set; }
        public NumericalPlotBandCollection SegmentPlotBandCollection { get; set; }
        public NumericalPlotBandCollection SegmentPlotBandCollectionWindows { get; set; }

        public PlotBandViewModel()
        {
            // Horizontal and Vertical Plot Band
            ClimateData = new ObservableCollection<ChartDataModel>();
            ClimateData.Add(new ChartDataModel() { Date = new DateTime(2023, 01, 15), Name = "Jan", Value = 22.7 });
            ClimateData.Add(new ChartDataModel() { Date = new DateTime(2023, 02, 05), Name = "Feb", Value = 24.5 });
            ClimateData.Add(new ChartDataModel() { Date = new DateTime(2023, 03, 05), Name = "Mar", Value = 26.8 });
            ClimateData.Add(new ChartDataModel() { Date = new DateTime(2023, 04, 05), Name = "Apr", Value = 28.4 });
            ClimateData.Add(new ChartDataModel() { Date = new DateTime(2023, 05, 05), Name = "May", Value = 29.6 });
            ClimateData.Add(new ChartDataModel() { Date = new DateTime(2023, 06, 05), Name = "Jun", Value = 29.2 });
            ClimateData.Add(new ChartDataModel() { Date = new DateTime(2023, 07, 05), Name = "Jul", Value = 27.6 });
            ClimateData.Add(new ChartDataModel() { Date = new DateTime(2023, 08, 05), Name = "Aug", Value = 27.2 });
            ClimateData.Add(new ChartDataModel() { Date = new DateTime(2023, 09, 05), Name = "Sep", Value = 27.3 });
            ClimateData.Add(new ChartDataModel() { Date = new DateTime(2023, 10, 05), Name = "Oct", Value = 28.3 });
            ClimateData.Add(new ChartDataModel() { Date = new DateTime(2023, 11, 05), Name = "Nov", Value = 27.2 });
            ClimateData.Add(new ChartDataModel() { Date = new DateTime(2023, 12, 01), Name = "Dec", Value = 25.2 });

            #region Plot band fill

            NumericalPlotBandFillCollection = new NumericalPlotBandCollection();
            ChartPlotBandLabelStyle FillLabelStyle = new ChartPlotBandLabelStyle();
            FillLabelStyle.SetDynamicResource(ChartPlotBandLabelStyle.TextColorProperty, "ContentForeground");
            FillLabelStyle.FontSize = 16;

            NumericalPlotBand plotBand1 = new NumericalPlotBand()
            {
                Start = 20,
                Size = 4,
                Fill = new SolidColorBrush(Color.FromArgb("#B300E190")),
                Text = "Low Temperature",
                LabelStyle = FillLabelStyle,
            };

            NumericalPlotBand plotBand2 = new NumericalPlotBand()
            {
                Start = 24,
                Size = 4,
                Fill = new SolidColorBrush(Color.FromArgb("#B3FCD404")),
                Text = "Average Temperature",
                LabelStyle = FillLabelStyle,
            };

            NumericalPlotBand plotBand3 = new NumericalPlotBand()
            {
                Start = 28,
                Size = 4,
                Fill = new SolidColorBrush(Color.FromArgb("#B3FF4E4E")),
                Text = "High Temperature",
                LabelStyle = FillLabelStyle,
            };

            NumericalPlotBandFillCollection.Add(plotBand1);
            NumericalPlotBandFillCollection.Add(plotBand2);
            NumericalPlotBandFillCollection.Add(plotBand3);

            #endregion

            #region Plot band line

            NumericalPlotBandLineCollection = new NumericalPlotBandCollection();

            ChartPlotBandLabelStyle LineLabelStyle = new ChartPlotBandLabelStyle();
            LineLabelStyle.SetDynamicResource(ChartPlotBandLabelStyle.TextColorProperty, "ContentForeground");
            LineLabelStyle.FontSize = 14;
            LineLabelStyle.OffsetX = 5;
            LineLabelStyle.OffsetY = 18;
            LineLabelStyle.HorizontalTextAlignment = ChartLabelAlignment.Start;

            NumericalPlotBand plotBand4 = new NumericalPlotBand()
            {
                Start = 24,
                End = 24,
                Stroke = new SolidColorBrush(Color.FromArgb("#00E190")),
                StrokeWidth = 2,
                Fill = new SolidColorBrush(Color.FromArgb("#00E190")),
                Text = "Low Temperature",
                LabelStyle = LineLabelStyle,
            };
          

            NumericalPlotBand plotBand5 = new NumericalPlotBand()
            {
                Start = 28,
                End = 28,
                Stroke = new SolidColorBrush(Color.FromArgb("#FCD404")),
                StrokeWidth = 2,
                Fill = new SolidColorBrush(Color.FromArgb("#FCD404")),
                Text = "Average Temperature",
                LabelStyle = LineLabelStyle,
            };

            NumericalPlotBand plotBand6 = new NumericalPlotBand()
            {
                Start = 32,
                End = 32,
                Stroke = new SolidColorBrush(Color.FromArgb("#FF4E4E")),
                StrokeWidth = 2,
                Fill = new SolidColorBrush(Color.FromArgb("#FF4E4E")),
                Text = "High Temperature",
                LabelStyle = LineLabelStyle,
            };

            NumericalPlotBandLineCollection.Add(plotBand4);
            NumericalPlotBandLineCollection.Add(plotBand5);
            NumericalPlotBandLineCollection.Add(plotBand6);

            #endregion

            #region Segmented plot band for mobile platoforms

            SegmentPlotBandCollection = new NumericalPlotBandCollection();

            ChartPlotBandLabelStyle SegmentLabelStyle1 = new ChartPlotBandLabelStyle();
            SegmentLabelStyle1.SetDynamicResource(ChartPlotBandLabelStyle.TextColorProperty, "ContentForeground");
            SegmentLabelStyle1.FontSize = 16;

            NumericalPlotBand SegmentBand1 = new NumericalPlotBand()
            {
                Start = 20,
                End = 21,
                AssociatedAxisStart = new DateTime(2023, 01, 01),
                AssociatedAxisEnd = new DateTime(2023, 04, 30),
                Text = "Low",
                Fill = new SolidColorBrush(Color.FromArgb("#B300E190")),
                LabelStyle = SegmentLabelStyle1,
            };

            NumericalPlotBand SegmentBand2 = new NumericalPlotBand()
            {
                Start = 25.5,
                End = 26.5,
                AssociatedAxisStart = new DateTime(2023, 05, 7),
                AssociatedAxisEnd = new DateTime(2023, 09, 3),
                Text = "Average",
                Fill = new SolidColorBrush(Color.FromArgb("#B3FCD404")),
                LabelStyle = SegmentLabelStyle1,
            };

            NumericalPlotBand SegmentBand3 = new NumericalPlotBand()
            {
                Start = 31,
                End = 32,
                AssociatedAxisStart = new DateTime(2023, 09, 01),
                AssociatedAxisEnd = new DateTime(2023, 12, 31),
                Text = "High",
                Fill = new SolidColorBrush(Color.FromArgb("#B3FF4E4E")),
                LabelStyle = SegmentLabelStyle1,
            };

            SegmentPlotBandCollection.Add(SegmentBand1);
            SegmentPlotBandCollection.Add(SegmentBand2);
            SegmentPlotBandCollection.Add(SegmentBand3);

            #endregion

            #region Segmented plot band for windows and macOS

            SegmentPlotBandCollectionWindows = new NumericalPlotBandCollection();

            ChartPlotBandLabelStyle SegmentLabelStyle2 = new ChartPlotBandLabelStyle();
            SegmentLabelStyle2.SetDynamicResource(ChartPlotBandLabelStyle.TextColorProperty, "ContentForeground");
            SegmentLabelStyle2.FontSize = 17;

            NumericalPlotBand SegmentBand4 = new NumericalPlotBand()
            {
                Start = 20,
                End = 22,
                AssociatedAxisEnd = 2,
                Text = "Low",
                Fill = new SolidColorBrush(Color.FromArgb("#B300E190")),
                LabelStyle = SegmentLabelStyle2,
            };

            NumericalPlotBand SegmentBand5 = new NumericalPlotBand()
            {
                Start = 25,
                End = 27,
                AssociatedAxisStart = 4.3,
                AssociatedAxisEnd = 6.8,
                Text = "Average",
                Fill = new SolidColorBrush(Color.FromArgb("#B3FCD404")),
                LabelStyle = SegmentLabelStyle2,
            };

            NumericalPlotBand SegmentBand6 = new NumericalPlotBand()
            {
                Start = 30,
                End = 32,
                AssociatedAxisStart = 9,
                Text = "High",
                Fill = new SolidColorBrush(Color.FromArgb("#B3FF4E4E")),
                LabelStyle = SegmentLabelStyle2,
            };

            SegmentPlotBandCollectionWindows.Add(SegmentBand4);
            SegmentPlotBandCollectionWindows.Add(SegmentBand5);
            SegmentPlotBandCollectionWindows.Add(SegmentBand6); 

            #endregion
        }
    }
}