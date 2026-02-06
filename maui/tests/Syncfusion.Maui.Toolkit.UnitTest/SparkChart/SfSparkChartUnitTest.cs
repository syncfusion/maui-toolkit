using Syncfusion.Maui.Toolkit.SparkCharts;
using System.Collections.ObjectModel;

namespace Syncfusion.Maui.Toolkit.UnitTest.SparkCharts
{
    // Helper class for data binding tests with custom objects.
    public class SparkChartTestData
    {
        public double Value { get; set; }
        public double OtherValue { get; set; }
    }

    public class SfSparkChartUnitTest
    {
        // Concrete implementation for testing the abstract base class.
        // Internal test class that exposes protected members of SfSparkChart for unit testing.
        private class TestSparkChart : SfSparkChart
        {
            public double MinY => this.minYValue;
            public double MaxY => this.maxYValue;
            public double MinX => this.minXValue;
            public double MaxX => this.maxXValue;
        }

        #region Constructor and Default Values

        [Fact]
        public void Constructor_InitializesDefaultsCorrectly()
        {
            // Arrange
            var chart = new TestSparkChart();

            // Assert
            Assert.Null(chart.ItemsSource);
            Assert.Equal(string.Empty, chart.YBindingPath);
            // Assert.Equal(EmptyPointValues.Zero, chart.EmptyPointValue);
            Assert.Equal(double.NaN, chart.MinimumYValue);
            Assert.Equal(double.NaN, chart.MaximumYValue);
            Assert.Equal(Color.FromArgb("#116DF9"), (chart.Stroke as SolidColorBrush)?.Color);
            // Assert.Equal(1.0, chart.StrokeWidth);
        }

        #endregion

        #region Data Generation Tests

        [Fact]
        public void ItemsSource_WithListOfDoubles_GeneratesPointsCorrectly()
        {
            // Arrange
            var chart = new TestSparkChart();
            var data = new List<double> { 10, 20, 15, 25, 30 };

            // Act
            chart.ItemsSource = data;

            // Assert
            Assert.Equal(5, chart.DataCount);
            Assert.Equal(new List<double> { 10, 20, 15, 25, 30 }, chart.yValues);
            Assert.Equal(new List<double> { 0, 1, 2, 3, 4 }, chart.xValues);
        }

        [Fact]
        public void ItemsSource_WithCustomObjectsAndYBindingPath_GeneratesPointsCorrectly()
        {
            // Arrange
            var chart = new TestSparkChart();
            var data = new List<SparkChartTestData>
            {
                new() { Value = 5 },
                new() { Value = 15 },
                new() { Value = 10 }
            };

            // Act
            chart.YBindingPath = "Value";
            chart.ItemsSource = data;

            // Assert
            Assert.Equal(3, chart.DataCount);
            Assert.Equal(new List<double> { 5, 15, 10 }, chart.yValues);
        }

        [Fact]
        public void ChangingYBindingPath_RegeneratesPoints()
        {
            // Arrange
            var chart = new TestSparkChart();
            var data = new List<SparkChartTestData>
            {
                new() { Value = 5, OtherValue = 100 },
                new() { Value = 15, OtherValue = 200 }
            };
            chart.ItemsSource = data;
            chart.YBindingPath = "Value";

            // Act - Change the binding path
            chart.YBindingPath = "OtherValue";

            // Assert
            Assert.Equal(2, chart.DataCount);
            Assert.Equal(new List<double> { 100, 200 }, chart.yValues);
        }

        [Fact]
        public void ItemsSource_SetToNull_ClearsData()
        {
            // Arrange
            var chart = new TestSparkChart();
            chart.ItemsSource = new List<double> { 1, 2, 3 };

            // Act
#pragma warning disable CS8625
            chart.ItemsSource = null;
#pragma warning restore CS8625

            // Assert
            Assert.Equal(0, chart.DataCount);
            Assert.Empty(chart.yValues);
            Assert.Empty(chart.xValues);
        }

        #endregion

        #region INotifyCollectionChanged Tests

        [Fact]
        public void ItemsSource_WhenItemAddedToObservableCollection_UpdatesPoints()
        {
            // Arrange
            var data = new ObservableCollection<double> { 10, 20 };
            var chart = new TestSparkChart { ItemsSource = data };

            // Act
            data.Add(30);

            // Assert
            Assert.Equal(3, chart.DataCount);
            Assert.Equal(new List<double> { 10, 20, 30 }, chart.yValues);
        }

        [Fact]
        public void ItemsSource_WhenItemRemovedFromObservableCollection_UpdatesPoints()
        {
            // Arrange
            var data = new ObservableCollection<double> { 10, 20, 30 };
            var chart = new TestSparkChart { ItemsSource = data };

            // Act
            data.Remove(20);

            // Assert
            Assert.Equal(2, chart.DataCount);
            Assert.Equal(new List<double> { 10, 30 }, chart.yValues);
        }

        #endregion

        #region Min/Max Value Tests

        [Fact]
        public void UpdateMinMaxValues_CalculatesCorrectlyFromData()
        {
            // Arrange
            var chart = new TestSparkChart();

            // Act
            chart.ItemsSource = new List<double> { -10, 50, 0, 25 };

            // Assert
            Assert.Equal(-10, chart.MinY);
            Assert.Equal(50, chart.MaxY);
            Assert.Equal(0, chart.MinX);
            Assert.Equal(3, chart.MaxX);
        }

        [Fact]
        public void UpdateMinMaxValues_WithExplicitProperties_OverridesCalculatedValues()
        {
            // Arrange
            var chart = new TestSparkChart
            {
                ItemsSource = new List<double> { 10, 20, 30 },
                MinimumYValue = 0,
                MaximumYValue = 100
            };

            // Assert
            Assert.Equal(0, chart.MinY);
            Assert.Equal(100, chart.MaxY);
        }

        #endregion

        #region Empty Point Tests

        #endregion

        #region

        private class AxisData
        {
            public string Category { get; set; } = string.Empty;
            public double NumericData { get; set; }
            public DateTime DateTimeData { get; set; }
            public double Y { get; set; }
        }

        [Fact]
        public void CategoryAxis_NoSorting_NoGaps()
        {
            var chart = new TestSparkChart();
            var src = new List<AxisData>
            {
                new() { Category="B", Y=20 },
                new() { Category="A", Y=10 },
                new() { Category="C", Y=30 },
            };

            chart.ItemsSource = src;
            chart.YBindingPath = nameof(AxisData.Y);
            chart.XBindingPath = nameof(AxisData.Category);
            chart.AxisType = SparkChartAxisType.Category;

            Assert.Equal(3, chart.DataCount);
            Assert.Equal([0, 1, 2], chart.xValues);
            Assert.DoesNotContain(double.NaN, chart.yValues.Where((v, i) => true));
        }

        [Fact]
        public void AxisLineStyle_Settable_And_ShowAxis_Toggle()
        {
            var chart = new TestSparkChart
            {
                ShowAxis = true
            };
            Assert.True(chart.ShowAxis);

            chart.AxisLineStyle = new SparkChartLineStyle
            {
                Stroke = new SolidColorBrush(Colors.Orange),
                StrokeWidth = 2d,
                StrokeDashArray = new DoubleCollection { 2, 2 }
            };

            Assert.NotNull(chart.AxisLineStyle);
            Assert.Equal(2d, chart.AxisLineStyle.StrokeWidth);
            Assert.Equal(2, chart.AxisLineStyle.StrokeDashArray.Count);
        }

        #endregion

        [Fact]
        public void CategoryAxis_WithXBindingPath_StillUsesIndices_NoGaps_NoSorting()
        {
            var chart = new SfSparkChartUnitTest.TestSparkChart();
            var src = new[]
            {
                new SparkChartTestData { Value = 10, OtherValue = 1 },
                new SparkChartTestData { Value = 30, OtherValue = 3 },
                new SparkChartTestData { Value = 20, OtherValue = 2 }
            };

            chart.YBindingPath = nameof(SparkChartTestData.Value);
            chart.XBindingPath = nameof(SparkChartTestData.OtherValue);
            chart.AxisType = SparkChartAxisType.Category;

            chart.ItemsSource = src;

            Assert.Equal(3, chart.DataCount);
            Assert.True(chart.xValues.SequenceEqual(new List<double> { 0, 1, 2 }));
            Assert.True(chart.yValues.SequenceEqual(new List<double> { 10, 30, 20 }));
        }

        #region Range Band Tests

        [Fact]
        public void RangeBand_Defaults_AreCorrect()
        {
            // Arrange
            var chart = new TestSparkChart();

            // Assert
            Assert.True(double.IsNaN(chart.RangeBandStart));
            Assert.True(double.IsNaN(chart.RangeBandEnd));
			Assert.NotNull(chart.RangeBandFill);
			Assert.Equal(Color.FromArgb("#E7E0EC"), (chart.RangeBandFill as SolidColorBrush)?.Color);
        }

        [Fact]
        public void RangeBand_SetValues_Persists()
        {
			// Arrange
			var chart = new TestSparkChart
			{
				// Act
				RangeBandStart = 10,
				RangeBandEnd = 20
			};

			// Assert
			Assert.Equal(10, chart.RangeBandStart);
            Assert.Equal(20, chart.RangeBandEnd);
        }

        [Fact]
        public void RangeBandFill_SetBrush_Persists()
        {
            // Arrange
            var chart = new TestSparkChart();
            var brush = new SolidColorBrush(Color.FromArgb("#80FF00"));

            // Act
            chart.RangeBandFill = brush;

            // Assert
            Assert.Same(brush, chart.RangeBandFill);
            Assert.Equal(Color.FromArgb("#80FF00"), (chart.RangeBandFill as SolidColorBrush)!.Color);
        }

        [Theory]
        [InlineData(-50, -10)]  // negative range
        [InlineData(0, 0)]      // zero range
        [InlineData(0, 25)]     // zero to positive
        [InlineData(-10, 30)]   // negative to positive
        [InlineData(10, 0)]     // positive to zero (reversed)
        [InlineData(15, -5)]    // positive to negative (reversed)
        public void RangeBand_SetValues_NegativeZeroPositive_Persist(double start, double end)
        {
			// Arrange
			var chart = new TestSparkChart
			{
				// Act
				RangeBandStart = start,
				RangeBandEnd = end
			};

			// Assert
			Assert.Equal(start, chart.RangeBandStart);
            Assert.Equal(end, chart.RangeBandEnd);
        }

        #endregion
    }
}
