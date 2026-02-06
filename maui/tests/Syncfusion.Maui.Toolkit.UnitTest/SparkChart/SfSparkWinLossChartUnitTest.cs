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
        private class NumericData
        {
            public double X { get; set; }
            public double Y { get; set; }
        }

        [Fact]
        public void NumericAxis_SortAndGaps()
        {
            var chart = new SfSparkWinLossChart
            {
                YBindingPath = nameof(NumericData.Y),
                XBindingPath = nameof(NumericData.X),
                AxisType = SparkChartAxisType.Numeric
            };

            var src = new List<NumericData>
            {
                new() { X = 2,  Y =  1 },
                new() { X = 7,  Y = -1 },
                new() { X = 10, Y =  0 },
                new() { X = 4,  Y =  1 },
            };

            chart.ItemsSource = src;

            Assert.Equal(9, chart.DataCount);
            var expectedNaNAt = new[] { 1, 3, 4, 6, 7 };
            foreach (var idx in expectedNaNAt)
            {
                Assert.True(double.IsNaN(chart.yValues[idx]));
                Assert.Contains(idx, chart.GapIndexes);
            }
        }

        private class NumericData2
        {
            public double X { get; set; }
            public double Y { get; set; }
        }

        [Fact]
        public void NumericAxis_IntegerBucket_LastWins_AndDenseGaps()
        {
            var chart = new SfSparkWinLossChart
            {
                YBindingPath = nameof(NumericData2.Y),
                XBindingPath = nameof(NumericData2.X),
                AxisType = SparkChartAxisType.Numeric
            };

            var src = new List<NumericData2>
            {
                new() { X = 1.4, Y =  1 },
                new() { X = 2.4, Y = -1 },
                new() { X = 2.49, Y =  0 },
                new() { X = 4.0, Y =  1 },
            };

            chart.ItemsSource = src;

            // Buckets 1..4 => 4 points
            Assert.Equal(4, chart.DataCount);

            // y: [1, 0, NaN, 1] with a gap at bucket 3
            Assert.Equal(1, chart.yValues[0]);
            Assert.Equal(0, chart.yValues[1]);
            Assert.True(double.IsNaN(chart.yValues[2]));
            Assert.Equal(1, chart.yValues[3]);

            Assert.Contains(2, chart.GapIndexes);
        }

        private class DateData2
        {
            public DateTime D { get; set; }
            public double Y { get; set; }
        }

        [Fact]
        public void DateTimeAxis_DayBucket_LastWins_AndDenseGaps()
        {
            var chart = new SfSparkWinLossChart
            {
                YBindingPath = nameof(DateData2.Y),
                XBindingPath = nameof(DateData2.D),
                AxisType = SparkChartAxisType.DateTime
            };

            var baseDay = new DateTime(2024, 8, 1);
            var src = new List<DateData2>
            {
                new() { D = baseDay.AddHours(10), Y =  1 },
                new() { D = baseDay.AddDays(2).AddHours(9), Y = -1 },
                new() { D = baseDay.AddDays(1).AddHours(8), Y =  1 },
                new() { D = baseDay.AddDays(1).AddHours(12), Y =  0 },
            };

            chart.ItemsSource = src;

            // Dense day 1..3 => 3 points
            Assert.Equal(3, chart.DataCount);

            // day1 => 1, day2 => 0 (last wins), day3 => -1
            Assert.Equal(1, chart.yValues[0]);
            Assert.Equal(0, chart.yValues[1]);
            Assert.True(chart.yValues[2] == -1);

            // No gaps in this particular sequence
            Assert.Empty(chart.GapIndexes);
        }
    }
}