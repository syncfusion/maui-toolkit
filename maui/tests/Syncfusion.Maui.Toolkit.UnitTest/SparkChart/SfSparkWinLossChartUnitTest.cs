using Xunit;
using Syncfusion.Maui.Toolkit.SparkCharts;
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Toolkit.UnitTest.SparkCharts
{
    public class SfSparkWinLossChartUnitTest
    {
        [Fact]
        public void Constructor_InitializesDefaultsCorrectly()
        {
            var chart = new SfSparkWinLossChart();

            Assert.Equal(Color.FromArgb("#116DF9"), (chart.PositivePointsFill as SolidColorBrush)?.Color);
            Assert.Equal(Color.FromArgb("#FF4E4E"), (chart.NegativePointsFill as SolidColorBrush)?.Color);
            Assert.Equal(Color.FromArgb("#E2227E"), (chart.NeutralPointFill as SolidColorBrush)?.Color);
        }

        [Fact]
        public void Color_SetAndGet_ReturnsExpectedValue()
        {
            var chart = new SfSparkWinLossChart();
            var greenBrush = new SolidColorBrush(Colors.Green);
            chart.PositivePointsFill = greenBrush;
            Assert.Equal(greenBrush, chart.PositivePointsFill);
        }

        [Fact]
        public void NegativePointsFill_SetAndGet_ReturnsExpectedValue()
        {
            var chart = new SfSparkWinLossChart();
            var darkRedBrush = new SolidColorBrush(Colors.DarkRed);
            chart.NegativePointsFill = darkRedBrush;
            Assert.Equal(darkRedBrush, chart.NegativePointsFill);
        }

        [Fact]
        public void NeutralPointFill_SetAndGet_ReturnsExpectedValue()
        {
            var chart = new SfSparkWinLossChart();
            var grayBrush = new SolidColorBrush(Colors.Gray);
            chart.NeutralPointFill = grayBrush;
            Assert.Equal(grayBrush, chart.NeutralPointFill);
        }
    }
}