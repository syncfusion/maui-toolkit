using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.Toolkit.UnitTest
{
    public class SfPolarChartUnitTests
    {
        [Fact]
        public void Constructor_InitializesDefaultsCorrectly()
        {
            var chart = new SfPolarChart();

            Assert.NotNull(chart.Series); 

            Assert.Null(chart.PrimaryAxis);
            
            Assert.Null(chart.SecondaryAxis); 

            Assert.Equal(ChartPolarAngle.Rotate270, chart.StartAngle); 

            Assert.Equal(PolarChartGridLineType.Circle, chart.GridLineType);
            
            var defaultBrushes = chart.PaletteBrushes;

            Assert.Equal(10, defaultBrushes.Count);

            Assert.Equal(Color.FromRgba(0, 189, 174, 255), (defaultBrushes[0] as SolidColorBrush)?.Color);
        }

        [Fact]
        public void PrimaryAxis_SetAndGet_ReturnsExpectedValue()
        {
            var chart = new SfPolarChart();
            var expectedAxis = new NumericalAxis();

            chart.PrimaryAxis = expectedAxis;

            Assert.Equal(expectedAxis, chart.PrimaryAxis);
        }

        [Fact]
        public void SecondaryAxis_SetAndGet_ReturnsExpectedValue()
        {
            var chart = new SfPolarChart();
            var expectedAxis = new NumericalAxis();

            chart.SecondaryAxis = expectedAxis;

            Assert.Equal(expectedAxis, chart.SecondaryAxis);
        }

        [Theory]
        [InlineData(ChartPolarAngle.Rotate0)]
        [InlineData(ChartPolarAngle.Rotate180)]
        [InlineData(ChartPolarAngle.Rotate90)]
        [InlineData(ChartPolarAngle.Rotate270)]
        public void StartAngle_SetAndGet_ReturnsExpectedValue(ChartPolarAngle expected)
        {
            var chart = new SfPolarChart();

            chart.StartAngle = expected;

            Assert.Equal(expected, chart.StartAngle);
        }

        [Fact]
        public void PaletteBrushes_SetAndGet_ReturnsExpectedValue()
        {
            var chart = new SfPolarChart();
            var expectedBrushes = new List<Brush>
            {
                new SolidColorBrush(Colors.Red),
                new SolidColorBrush(Colors.Blue)
            };

            chart.PaletteBrushes = expectedBrushes;

            Assert.Equal(expectedBrushes, chart.PaletteBrushes);
        }

        [Fact]
        public void Series_SetAndGet_ReturnsExpectedValue()
        {
            var chart = new SfPolarChart();
            var seriesCollection = new ChartPolarSeriesCollection
            {
                new PolarLineSeries
                {
                    ItemsSource = new List<dynamic> 
                    {
                        new { XValue = 10, YValue1 = 100, YValue2 = 110 },
                        new { XValue = 20, YValue1 = 150, YValue2 = 100 }
                    }
                }
            };

            chart.Series = seriesCollection;

            Assert.Equal(seriesCollection, chart.Series);
        }

        [Theory]
        [InlineData(PolarChartGridLineType.Circle)]
        [InlineData(PolarChartGridLineType.Polygon)]
        public void GridLineType_SetAndGet_ReturnsExpectedValue(PolarChartGridLineType expected)
        {
            var chart = new SfPolarChart();

            chart.GridLineType = expected;

            Assert.Equal(expected, chart.GridLineType);
        }
    }
}
