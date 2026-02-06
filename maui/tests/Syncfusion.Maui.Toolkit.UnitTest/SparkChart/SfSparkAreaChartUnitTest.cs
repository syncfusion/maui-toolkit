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
        private class NumericData
        {
            public double X { get; set; }
            public double Y { get; set; }
        }

        [Fact]
        public void NumericAxis_SortOnly_NoGaps()
        {
            var chart = new SfSparkAreaChart
            {
                YBindingPath = nameof(NumericData.Y),
                XBindingPath = nameof(NumericData.X),
                AxisType = SparkChartAxisType.Numeric
            };

            var src = new List<NumericData>
            {
                new() { X = 4, Y = 40 },
                new() { X = 1, Y = 10 },
                new() { X = 3, Y = 30 },
                new() { X = 2, Y = 20 },
            };

            chart.ItemsSource = src;

            Assert.Equal(4, chart.DataCount);
            Assert.True(chart.xValues.SequenceEqual([1, 2, 3, 4]));
            Assert.True(chart.yValues.SequenceEqual([10, 20, 30, 40]));
            Assert.Empty(chart.GapIndexes);
        }

        [Fact]
        public void ShowMarkers_Toggle()
        {
            var chart = new SfSparkAreaChart();
            Assert.False(chart.ShowMarkers);
            chart.ShowMarkers = true;
            Assert.True(chart.ShowMarkers);
        }

        [Fact]
        public void NumericAxis_Normalization_RoundsTo6Decimals_AndKeepsDuplicates()
        {
            var chart = new SfSparkAreaChart
            {
                YBindingPath = "Y",
                XBindingPath = "X",
                AxisType = SparkChartAxisType.Numeric
            };

            var src = new[]
            {
                new { X = 1.0000001, Y = 10.0 },
                new { X = 1.0000004, Y = 12.0 },
                new { X = 1.0000005, Y = 15.0 },
                new { X = 2.0000004, Y = 20.0 },
            };

            chart.ItemsSource = src;
            Assert.Equal(4, chart.DataCount);
            Assert.Empty(chart.GapIndexes);

            var expectedX = src
                .Select(s => Math.Round(s.X, 6, MidpointRounding.AwayFromZero))
                .OrderBy(x => x)
                .ToList();
            var expectedY = new List<double> { 10, 12, 15, 20 };
            Assert.True(chart.xValues.SequenceEqual(expectedX));
            Assert.True(chart.yValues.SequenceEqual(expectedY));
        }

        [Fact]
        public void DateTimeAxis_BucketsByDay_AndKeepsDuplicates_NoGaps()
        {
            var chart = new SfSparkAreaChart
            {
                YBindingPath = "YValue",
                XBindingPath = "DateTime",
                AxisType = SparkChartAxisType.DateTime
            };

            var day = new DateTime(2024, 5, 10);
            var src = new[]
            {
                new { DateTime = day.AddHours(1),  YValue = 10.0 },
                new { DateTime = day.AddHours(13), YValue = 20.0 },
                new { DateTime = day.AddHours(20), YValue = 30.0 },
                new { DateTime = day.AddDays(1).AddHours(2), YValue = 40.0 }
            };

            chart.ItemsSource = src;

            Assert.Equal(4, chart.DataCount);
            Assert.Empty(chart.GapIndexes);

            var value1 = day.Date.ToOADate();
            var value2 = day.AddDays(1).Date.ToOADate();
            var expectedX = new List<double> { value1, value1, value1, value2 };
            var expectedY = new List<double> { 10, 20, 30, 40 };

            Assert.True(chart.xValues.SequenceEqual(expectedX));
            Assert.True(chart.yValues.SequenceEqual(expectedY));
        }
    }
}