using Xunit;
using Syncfusion.Maui.Toolkit.SparkCharts;
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Toolkit.UnitTest.SparkCharts
{
    public class SfSparkColumnChartUnitTest
    {
        [Fact]
        public void Constructor_InitializesPointFillsAsNull()
        {
            var chart = new SfSparkColumnChart();

            Assert.Null(chart.FirstPointFill);
            Assert.Null(chart.LastPointFill);
            Assert.Null(chart.HighPointFill);
            Assert.Null(chart.LowPointFill);
            Assert.Null(chart.NegativePointsFill);
        }

        [Fact]
        public void FirstPointFill_SetAndGet_ReturnsExpectedValue()
        {
            var chart = new SfSparkColumnChart();
            var greenBrush = new SolidColorBrush(Colors.Green);
            chart.FirstPointFill = greenBrush;
            Assert.Equal(greenBrush, chart.FirstPointFill);
        }

        [Fact]
        public void LastPointFill_SetAndGet_ReturnsExpectedValue()
        {
            var chart = new SfSparkColumnChart();
            var blueBrush = new SolidColorBrush(Colors.Blue);
            chart.LastPointFill = blueBrush;
            Assert.Equal(blueBrush, chart.LastPointFill);
        }

        [Fact]
        public void HighPointFill_SetAndGet_ReturnsExpectedValue()
        {
            var chart = new SfSparkColumnChart();
            var orangeBrush = new SolidColorBrush(Colors.Orange);
            chart.HighPointFill = orangeBrush;
            Assert.Equal(orangeBrush, chart.HighPointFill);
        }

        [Fact]
        public void LowPointFill_SetAndGet_ReturnsExpectedValue()
        {
            var chart = new SfSparkColumnChart();
            var purpleBrush = new SolidColorBrush(Colors.Purple);
            chart.LowPointFill = purpleBrush;
            Assert.Equal(purpleBrush, chart.LowPointFill);
        }

        [Fact]
        public void NegativePointsFill_SetAndGet_ReturnsExpectedValue()
        {
            var chart = new SfSparkColumnChart();
            var redBrush = new SolidColorBrush(Colors.Red);
            chart.NegativePointsFill = redBrush;
            Assert.Equal(redBrush, chart.NegativePointsFill);
        }
        
        private class NX
        {
            public double X { get; set; }
            public double Y { get; set; }
        }

        [Fact]
        public void NumericAxis_SortAndGaps()
        {
            var chart = new SfSparkColumnChart
            {
                YBindingPath = nameof(NX.Y),
                XBindingPath = nameof(NX.X),
                AxisType = SparkChartAxisType.Numeric
            };

            var src = new List<NX>
            {
                new() { X = 2, Y = 20 },
                new() { X = 7, Y = 70 },
                new() { X = 10, Y = 100 },
                new() { X = 4, Y = 40 },
            };

            chart.ItemsSource = src;

            // Expected range: 2..10 inclusive
            Assert.Equal(9, chart.DataCount);
            // Gaps exist at 3,5,6,8,9 -> relative indices 1,3,4,6,7
            var expectedNaNAt = new[] { 1, 3, 4, 6, 7 };
            foreach (var idx in expectedNaNAt)
            {
                Assert.True(double.IsNaN(chart.yValues[idx]));
                Assert.Contains(idx, chart.GapIndexes);
            }
        }

        private class DX
        {
            public DateTime D { get; set; }
            public double Y { get; set; }
        }

        [Fact]
        public void DateTimeAxis_SortAndDailyGaps()
        {
            var chart = new SfSparkColumnChart
            {
                YBindingPath = nameof(DX.Y),
                XBindingPath = nameof(DX.D),
                AxisType = SparkChartAxisType.DateTime
            };

            var start = new DateTime(2024, 1, 1);
            var src = new List<DX>
            {
                new() { D = start.AddDays(2),  Y = 20 },
                new() { D = start.AddDays(7),  Y = 70 },
                new() { D = start.AddDays(10), Y = 100 },
                new() { D = start.AddDays(4),  Y = 40 },
            };

            chart.ItemsSource = src;

            // Range 2..10 inclusive => 9 days
            Assert.Equal(9, chart.DataCount);
            // NaN at days: 3,5,6,8,9 relative to start+2 => indices 1,3,4,6,7
            var expectedNaNAt = new[] { 1, 3, 4, 6, 7 };
            foreach (var idx in expectedNaNAt)
            {
                Assert.True(double.IsNaN(chart.yValues[idx]));
                Assert.Contains(idx, chart.GapIndexes);
            }
        }

        private class NX2
        {
            public double X { get; set; }
            public double Y { get; set; }
        }

        [Fact]
        public void NumericAxis_IntegerBucket_LastWins_AndDenseGaps()
        {
            var chart = new SfSparkColumnChart
            {
                YBindingPath = nameof(NX2.Y),
                XBindingPath = nameof(NX2.X),
                AxisType = SparkChartAxisType.Numeric
            };

            // Buckets: round to nearest integer (AwayFromZero). Bucket 2 has two entries; last should win.
            var src = new List<NX2>
            {
                new() { X = 1.4, Y = 10 }, // bucket 1
                new() { X = 2.4, Y = 20 }, // bucket 2 (first)
                new() { X = 2.6, Y = 25 }, // bucket 3? No: 2.6 rounds to 3 (AwayFromZero). Keep different to show distinct.
                new() { X = 2.49, Y = 22 }, // bucket 2 (second, last wins for bucket 2)
                new() { X = 5.0, Y = 50 }, // bucket 5
            };

            chart.ItemsSource = src;

            // Buckets used: 1..5 => 5 data points after densification
            Assert.Equal(5, chart.DataCount);

            // Expected y at buckets:
            // 1 => 10
            // 2 => 22 (last wins among 2.4 and 2.49)
            // 3 => 25 (from 2.6 rounding to 3)
            // 4 => NaN (gap)
            // 5 => 50
            Assert.True(chart.yValues[0] == 10);
            Assert.True(chart.yValues[1] == 22);
            Assert.True(chart.yValues[2] == 25);
            Assert.True(double.IsNaN(chart.yValues[3]));
            Assert.True(chart.yValues[4] == 50);

            // Gaps recorded at index 3 (bucket 4 missing)
            Assert.Contains(3, chart.GapIndexes);
        }

        private class DateTimeAxes
        {
            public DateTime Day { get; set; }
            public double YValue { get; set; }
        }

        [Fact]
        public void DateTimeAxis_DayBucket_LastWins_AndDenseGaps()
        {
            var chart = new SfSparkColumnChart
            {
                YBindingPath = nameof(DateTimeAxes.YValue),
                XBindingPath = nameof(DateTimeAxes.Day),
                AxisType = SparkChartAxisType.DateTime
            };

            var baseDay = new DateTime(2024, 6, 1);
            var src = new List<DateTimeAxes>
            {
                new() { Day = baseDay.AddHours( 9), YValue = 10 },
                new() { Day = baseDay.AddDays(1).AddHours(8), YValue = 20 },
                new() { Day = baseDay.AddDays(1).AddHours(20), YValue = 22 },
                new() { Day = baseDay.AddDays(3).AddHours(1), YValue = 40 },
            };

            chart.ItemsSource = src;

            // Dense range: day 1..4 => 4 points
            Assert.Equal(4, chart.DataCount);
            Assert.True(chart.yValues[0] == 10);
            Assert.True(chart.yValues[1] == 22);
            Assert.True(double.IsNaN(chart.yValues[2]));
            Assert.True(chart.yValues[3] == 40);

            // Gap recorded at day 3 index (2)
            Assert.Contains(2, chart.GapIndexes);
        }
    }
}