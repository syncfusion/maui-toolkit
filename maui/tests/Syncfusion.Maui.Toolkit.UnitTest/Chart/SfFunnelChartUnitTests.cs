using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.Toolkit.UnitTest
{
    public class SfFunnelChartUnitTests
    {
        [Fact]
        public void Constructor_InitializesDefaultsCorrectly()
        {
            var chart = new SfFunnelChart();

            Assert.NotNull(chart);

            Assert.Null(chart.ItemsSource);  

            Assert.Null(chart.XBindingPath);  

            Assert.Null(chart.YBindingPath);

            var defaultBrushes = chart.PaletteBrushes;

            Assert.Equal(10, defaultBrushes.Count);
            
            Assert.Equal(Color.FromRgba(0, 189, 174, 255), (defaultBrushes[0] as SolidColorBrush)?.Color); 

            Assert.Equal(SolidColorBrush.Transparent,chart.Stroke);  

            Assert.Equal(2d, chart.StrokeWidth);  

            Assert.Equal(ChartLegendIconType.Circle, chart.LegendIcon);  

            Assert.Null(chart.TooltipTemplate);  

            Assert.False(chart.EnableTooltip);  

            Assert.Null(chart.SelectionBehavior); 

            Assert.False(chart.ShowDataLabels);

            Assert.Equal(FunnelDataLabelContext.YValue, chart.DataLabelSettings.Context);
           
            Assert.Equal(0, chart.GapRatio); 

            Assert.Null(chart.LabelTemplate);  
        }

        [Fact]
        public void ItemsSource_SetAndGet_ReturnsExpectedValue()
        {
            var chart = new SfFunnelChart();
            var expectedValue = new object();

            chart.ItemsSource = expectedValue;

            Assert.Equal(expectedValue, chart.ItemsSource);
        }

        [Theory]
        [InlineData("XValue1")]
        [InlineData("XValue2")]
        public void XBindingPath_SetAndGet_ReturnsExpectedValue(string expectedValue)
        {
            var chart = new SfFunnelChart();

            chart.XBindingPath = expectedValue;

            Assert.Equal(expectedValue, chart.XBindingPath);
        }

        [Theory]
        [InlineData("YValue1")]
        [InlineData("YValue2")]
        public void YBindingPath_SetAndGet_ReturnsExpectedValue(string expectedValue)
        {
            var chart = new SfFunnelChart();

            chart.YBindingPath = expectedValue;

            Assert.Equal(expectedValue, chart.YBindingPath);
        }

        [Fact]
        public void PaletteBrushes_SetAndGet_ReturnsExpectedValue()
        {
            var chart = new SfFunnelChart();
            var expectedValue = new List<Brush> { new SolidColorBrush(Colors.Red) };

            chart.PaletteBrushes = expectedValue;

            Assert.Equal(expectedValue, chart.PaletteBrushes);
        }

        [Fact]
        public void Stroke_SetAndGet_ReturnsExpectedValue()
        {
            var chart = new SfFunnelChart();
            var expectedValue = new SolidColorBrush(Colors.Black);

            chart.Stroke = expectedValue;

            Assert.Equal(expectedValue, chart.Stroke);
        }

        [Theory]
        [InlineData(1.0)]
        [InlineData(2.5)]
        public void StrokeWidth_SetAndGet_ReturnsExpectedValue(double expectedValue)
        {
            var chart = new SfFunnelChart();

            chart.StrokeWidth = expectedValue;

            Assert.Equal(expectedValue, chart.StrokeWidth);
        }

        [Theory]
        [InlineData(ChartLegendIconType.Circle)]
        [InlineData(ChartLegendIconType.Diamond)]
        [InlineData(ChartLegendIconType.Cross)]
        [InlineData(ChartLegendIconType.Hexagon)]
        [InlineData(ChartLegendIconType.Rectangle)]
        [InlineData(ChartLegendIconType.HorizontalLine)]
        [InlineData(ChartLegendIconType.InvertedTriangle)]
        [InlineData(ChartLegendIconType.Triangle)]
        [InlineData(ChartLegendIconType.Pentagon)]
        [InlineData(ChartLegendIconType.Plus)]
        [InlineData(ChartLegendIconType.SeriesType)]
        [InlineData(ChartLegendIconType.VerticalLine)]
        public void LegendIcon_SetAndGet_ReturnsExpectedValue(ChartLegendIconType expectedValue)
        {
            var chart = new SfFunnelChart();

            chart.LegendIcon = expectedValue;

            Assert.Equal(expectedValue, chart.LegendIcon);
        }

        [Fact]
        public void TooltipTemplate_SetAndGet_ReturnsExpectedValue()
        {
            var chart = new SfFunnelChart();
            var expectedValue = new DataTemplate();

            chart.TooltipTemplate = expectedValue;

            Assert.Equal(expectedValue, chart.TooltipTemplate);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void EnableTooltip_SetAndGet_ReturnsExpectedValue(bool expectedValue)
        {
            var chart = new SfFunnelChart();

            chart.EnableTooltip = expectedValue;

            Assert.Equal(expectedValue, chart.EnableTooltip);
        }

        [Fact]
        public void SelectionBehavior_SetAndGet_ReturnsExpectedValue()
        {
            var chart = new SfFunnelChart();
            var expectedValue = new DataPointSelectionBehavior();

            chart.SelectionBehavior = expectedValue;

            Assert.Equal(expectedValue, chart.SelectionBehavior);
        }

        [Theory]
        [InlineData(0.1)]
        [InlineData(0.5)]
        public void GapRatio_SetAndGet_ReturnsExpectedValue(double expectedValue)
        {
            var chart = new SfFunnelChart();

            chart.GapRatio = expectedValue;

            Assert.Equal(expectedValue, chart.GapRatio);
        }

        [Fact]
        public void ShowDataLabels_SetAndGet_ReturnsExpectedValue()
        {
            var chart = new SfFunnelChart();
            var expectedValue = true;

            chart.ShowDataLabels = expectedValue;

            Assert.Equal(expectedValue, chart.ShowDataLabels);
        }

        [Fact]
        public void DataLabelSettings_SetAndGet_ReturnsExpectedValue()
        {
            var chart = new SfFunnelChart();
            var expectedValue = new FunnelDataLabelSettings();

            chart.DataLabelSettings = expectedValue;

            Assert.Equal(expectedValue, chart.DataLabelSettings);
        }

        [Fact]
        public void LabelTemplate_SetAndGet_ReturnsExpectedValue()
        {
            var chart = new SfFunnelChart();
            var expectedValue = new DataTemplate();

            chart.LabelTemplate = expectedValue;

            Assert.Equal(expectedValue, chart.LabelTemplate);
        }

    }
}
