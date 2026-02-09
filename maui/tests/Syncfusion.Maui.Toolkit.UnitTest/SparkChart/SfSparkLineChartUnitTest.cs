using Xunit;
using Syncfusion.Maui.Toolkit.SparkCharts;
using Microsoft.Maui.Graphics;
using System.Linq;
using System.Globalization;

namespace Syncfusion.Maui.Toolkit.UnitTest.SparkCharts
{
    public class SfSparkLineChartUnitTest
    {
        [Fact]
        public void Constructor_InitializesPointFillsAsNull()
        {
            var chart = new SfSparkLineChart();

            Assert.Null(chart.FirstPointFill);
            Assert.Null(chart.LastPointFill);
            Assert.Null(chart.HighPointFill);
            Assert.Null(chart.LowPointFill);
            Assert.Null(chart.NegativePointsFill);
        }

        [Fact]
        public void FirstPointFill_SetAndGet_ReturnsExpectedValue()
        {
            var chart = new SfSparkLineChart();
            var greenBrush = new SolidColorBrush(Colors.Green);
            chart.FirstPointFill = greenBrush;
            Assert.Equal(greenBrush, chart.FirstPointFill);
        }

        [Fact]
        public void LastPointFill_SetAndGet_ReturnsExpectedValue()
        {
            var chart = new SfSparkLineChart();
            var blueBrush = new SolidColorBrush(Colors.Blue);
            chart.LastPointFill = blueBrush;
            Assert.Equal(blueBrush, chart.LastPointFill);
        }

        [Fact]
        public void HighPointFill_SetAndGet_ReturnsExpectedValue()
        {
            var chart = new SfSparkLineChart();
            var orangeBrush = new SolidColorBrush(Colors.Orange);
            chart.HighPointFill = orangeBrush;
            Assert.Equal(orangeBrush, chart.HighPointFill);
        }

        [Fact]
        public void LowPointFill_SetAndGet_ReturnsExpectedValue()
        {
            var chart = new SfSparkLineChart();
            var purpleBrush = new SolidColorBrush(Colors.Purple);
            chart.LowPointFill = purpleBrush;
            Assert.Equal(purpleBrush, chart.LowPointFill);
        }

        [Fact]
        public void NegativePointsFill_SetAndGet_ReturnsExpectedValue()
        {
            var chart = new SfSparkLineChart();
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
            var chart = new SfSparkLineChart();
            chart.YBindingPath = nameof(NumericData.Y);
            chart.XBindingPath = nameof(NumericData.X);
            chart.AxisType = SparkChartAxisType.Numeric;

            var src = new List<NumericData>
            {
                new() { X = 7,  Y = 70 },
                new() { X = 2,  Y = 20 },
                new() { X = 10, Y = 100 },
                new() { X = 4,  Y = 40 },
            };

            chart.ItemsSource = src;

            Assert.Equal(4, chart.DataCount);
            Assert.True(chart.xValues.SequenceEqual(new List<double> { 2, 4, 7, 10 }));
            Assert.True(chart.yValues.SequenceEqual(new List<double> { 20, 40, 70, 100 }));
            Assert.Empty(chart.GapIndexes);
            Assert.DoesNotContain(double.NaN, chart.yValues);
        }

        private class DateData
        {
            public DateTime OrderDate { get; set; }
            public double Y { get; set; }
        }

        [Fact]
        public void DateTimeAxis_SortOnly_NoGaps()
        {
            var chart = new SfSparkLineChart();
            chart.YBindingPath = nameof(DateData.Y);
            chart.XBindingPath = nameof(DateData.OrderDate);
            chart.AxisType = SparkChartAxisType.DateTime;

            var start = new DateTime(2024, 1, 1);
            var src = new List<DateData>
            {
                new() { OrderDate = start.AddDays(7), Y = 70 },
                new() { OrderDate = start.AddDays(2), Y = 20 },
                new() { OrderDate = start.AddDays(10), Y = 100 },
                new() { OrderDate = start.AddDays(4), Y = 40 },
            };

            chart.ItemsSource = src;

            Assert.Equal(4, chart.DataCount);
            var expectedX = new List<double>
            {
                start.AddDays(2).ToOADate(),
                start.AddDays(4).ToOADate(),
                start.AddDays(7).ToOADate(),
                start.AddDays(10).ToOADate(),
            };
            Assert.True(chart.xValues.SequenceEqual(expectedX));
            Assert.True(chart.yValues.SequenceEqual([20, 40, 70, 100]));
            Assert.Empty(chart.GapIndexes);
        }

        [Fact]
        public void Markers_Toggle_ShowMarkers_Property()
        {
            var chart = new SfSparkLineChart();
            Assert.False(chart.ShowMarkers);
            chart.ShowMarkers = true;
            Assert.True(chart.ShowMarkers);
        }

        private class NumericData2
        {
            public double X { get; set; }
            public double Y { get; set; }
        }

        [Fact]
        public void NumericAxis_Normalization_RoundsTo6Decimals_AndKeepsDuplicates()
        {
            var chart = new SfSparkLineChart
            {
                YBindingPath = nameof(NumericData2.Y),
                XBindingPath = nameof(NumericData2.X),
                AxisType = SparkChartAxisType.Numeric
            };

            // Values crafted to hit 6-decimal rounding edges (MidpointRounding.AwayFromZero)
            var src = new List<NumericData2>
            {
                new() { X = 1.0000001, Y = 10 },
                new() { X = 1.0000004, Y = 14 },
                new() { X = 1.0000005, Y = 15 },
                new() { X = 2.9999995, Y = 29 },
                new() { X = 3.0000004, Y = 30 },
            };

            chart.ItemsSource = src;
            Assert.Equal(5, chart.DataCount);
            Assert.Empty(chart.GapIndexes);
            var expectedX = new List<double>
            {
                Math.Round(1.0000001, 6, MidpointRounding.AwayFromZero),
                Math.Round(1.0000004, 6, MidpointRounding.AwayFromZero),
                Math.Round(1.0000005, 6, MidpointRounding.AwayFromZero),
                Math.Round(2.9999995, 6, MidpointRounding.AwayFromZero),
                Math.Round(3.0000004, 6, MidpointRounding.AwayFromZero),
            }.OrderBy(x => x).ToList();

            var expectedY = new List<double> { 10, 14, 15, 29, 30 };
            Assert.True(chart.xValues.SequenceEqual(expectedX));
            Assert.True(chart.yValues.SequenceEqual(expectedY));
        }

        private class DateData2
        {
            public DateTime OrderDate { get; set; }
            public double Y { get; set; }
        }

        [Fact]
        public void DateTimeAxis_ValuesByDay_AndKeepsDuplicates_NoGaps()
        {
            var chart = new SfSparkLineChart
            {
                YBindingPath = nameof(DateData2.Y),
                XBindingPath = nameof(DateData2.OrderDate),
                AxisType = SparkChartAxisType.DateTime
            };

            var day = new DateTime(2024, 2, 28);
            var src = new List<DateData2>
            {
                new() { OrderDate = day.AddHours( 9), Y = 10 },
                new() { OrderDate = day.AddHours(12), Y = 20 },
                new() { OrderDate = day.AddHours(16), Y = 30 },
                new() { OrderDate = day.AddDays(1).AddHours(8), Y = 40 },
            };

            chart.ItemsSource = src;
            Assert.Equal(4, chart.DataCount);
            Assert.Empty(chart.GapIndexes);

            var Day1Value = day.Date.ToOADate();
            var Day2Value = day.AddDays(1).Date.ToOADate();
            var expectedX = new List<double> { Day1Value, Day1Value, Day1Value, Day2Value };
            var expectedY = new List<double> { 10, 20, 30, 40 };

            Assert.True(chart.xValues.SequenceEqual(expectedX));
            Assert.True(chart.yValues.SequenceEqual(expectedY));
        }

        private static bool TryToDouble(object? value, out double result)
        {
            result = double.NaN;
            if (value == null)
            {
                return false;
            }

            // Fast path for IConvertible
            if (value is IConvertible)
            {
                try
                {
                    var converted = Convert.ToDouble(value, CultureInfo.InvariantCulture);
                    if (double.IsNaN(converted) || double.IsInfinity(converted))
                    {
                        return false; // reject NaN/Infinity
                    }

                    result = converted;
                    return true;
                }
                catch
                {
                    // fall through to string parse
                }
            }

            // Fallback: parse string
            var s = Convert.ToString(value, CultureInfo.InvariantCulture);
            if (!string.IsNullOrWhiteSpace(s) &&
                double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out var d) &&
                !double.IsNaN(d) && !double.IsInfinity(d)) // reject NaN/Infinity
            {
                result = d;
                return true;
            }

            return false;
        }
    }
}