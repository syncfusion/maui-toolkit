using Xunit;
using Syncfusion.Maui.Toolkit.SparkCharts;
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Toolkit.UnitTest.SparkCharts
{
    public class SfSparkAreaChartUnitTest
    {
        [Fact]
        public void Constructor_InitializesPointFillsAsNull()
        {
            var chart = new SfSparkAreaChart();

            Assert.Null(chart.FirstPointFill);
            Assert.Null(chart.LastPointFill);
            Assert.Null(chart.HighPointFill);
            Assert.Null(chart.LowPointFill);
            Assert.Null(chart.NegativePointsFill);
        }

        [Fact]
        public void FirstPointFill_SetAndGet_ReturnsExpectedValue()
        {
            var chart = new SfSparkAreaChart();
            var greenBrush = new SolidColorBrush(Colors.Green);
            chart.FirstPointFill = greenBrush;
            Assert.Equal(greenBrush, chart.FirstPointFill);
        }

        [Fact]
        public void LastPointFill_SetAndGet_ReturnsExpectedValue()
        {
            var chart = new SfSparkAreaChart();
            var blueBrush = new SolidColorBrush(Colors.Blue);
            chart.LastPointFill = blueBrush;
            Assert.Equal(blueBrush, chart.LastPointFill);
        }

        [Fact]
        public void HighPointFill_SetAndGet_ReturnsExpectedValue()
        {
            var chart = new SfSparkAreaChart();
            var orangeBrush = new SolidColorBrush(Colors.Orange);
            chart.HighPointFill = orangeBrush;
            Assert.Equal(orangeBrush, chart.HighPointFill);
        }

        [Fact]
        public void LowPointFill_SetAndGet_ReturnsExpectedValue()
        {
            var chart = new SfSparkAreaChart();
            var purpleBrush = new SolidColorBrush(Colors.Purple);
            chart.LowPointFill = purpleBrush;
            Assert.Equal(purpleBrush, chart.LowPointFill);
        }

        [Fact]
        public void NegativePointsFill_SetAndGet_ReturnsExpectedValue()
        {
            var chart = new SfSparkAreaChart();
            var redBrush = new SolidColorBrush(Colors.Red);
            chart.NegativePointsFill = redBrush;
            Assert.Equal(redBrush, chart.NegativePointsFill);
        }
    }
}