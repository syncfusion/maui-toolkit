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

        // [Fact]
        // public void EmptyPointValue_Zero_ReplacesNaNWithZero()
        // {
        //     // Arrange
        //     var chart = new TestSparkChart
        //     {
        //         ItemsSource = new List<double> { 10, double.NaN, 30 },
        //         EmptyPointValue = EmptyPointValues.Zero
        //     };

        //     // Assert
        //     Assert.Equal(new List<double> { 10, 0, 30 }, chart.yValues);
        // }

        // [Theory]
        // [InlineData(new double[] { double.NaN, 20, 30 }, new double[] { 10, 20, 30 })]       // NaN at start
        // [InlineData(new double[] { 10, double.NaN, 30 }, new double[] { 10, 20, 30 })]       // NaN in middle
        // [InlineData(new double[] { 10, 20, double.NaN }, new double[] { 10, 20, 10 })]       // NaN at end
        // public void EmptyPointValue_Average_ReplacesNaNWithCorrectAverage(IEnumerable<double> inputData, IEnumerable<double> expectedData)
        // {
        //     // Arrange
        //     var chart = new TestSparkChart
        //     {
        //         ItemsSource = inputData,
        //         EmptyPointValue = EmptyPointValues.Average
        //     };

        //     // Assert
        //     Assert.Equal(expectedData, chart.yValues);
        // }

        // [Fact]
        // public void EmptyPointValue_None_KeepsNaNInData()
        // {
        //     // Arrange
        //     var chart = new TestSparkChart
        //     {
        //         ItemsSource = new List<double> { 10, double.NaN, 30 },
        //         EmptyPointValue = EmptyPointValues.None
        //     };

        //     // Assert
        //     Assert.Equal(new List<double> { 10, double.NaN, 30 }, chart.yValues);
        // }

        #endregion
    }
}
