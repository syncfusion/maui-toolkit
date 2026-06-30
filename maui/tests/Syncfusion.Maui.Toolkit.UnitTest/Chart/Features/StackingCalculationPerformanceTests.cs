using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.Toolkit.UnitTest.Charts
{
	public class StackingCalculationPerformanceTests : BaseUnitTest
	{
		#region GetYValue Tests

		[Fact]
		public void GetYValue_ReturnsPercentageOfTotal()
		{
			// Arrange: 3 series with values at index 0: 20, 30, 50 → total = 100
			var series1 = new StackingColumn100Series();
			SetPrivateField(series1, "<YValues>k__BackingField", new List<double> { 20.0 } as IList<double>);

			var series2 = new StackingColumn100Series();
			SetPrivateField(series2, "<YValues>k__BackingField", new List<double> { 30.0 } as IList<double>);

			var series3 = new StackingColumn100Series();
			SetPrivateField(series3, "<YValues>k__BackingField", new List<double> { 50.0 } as IList<double>);

			var seriesList = new List<StackingSeriesBase> { series1, series2, series3 };

			// Act: GetYValue for yValue=20 at index 0 should return (20/100)*100 = 20
			var result = InvokeStaticPrivateMethodClass(
				typeof(CartesianChartArea), "GetYValue", seriesList, 20.0, 0);

			// Assert
			Assert.Equal(20.0, (double)result);
		}

		[Fact]
		public void GetYValue_WithZeroYValue_ReturnsZero()
		{
			var series1 = new StackingColumn100Series();
			SetPrivateField(series1, "<YValues>k__BackingField", new List<double> { 50.0 } as IList<double>);

			var series2 = new StackingColumn100Series();
			SetPrivateField(series2, "<YValues>k__BackingField", new List<double> { 50.0 } as IList<double>);

			var seriesList = new List<StackingSeriesBase> { series1, series2 };

			// yValue=0 should remain 0 regardless of total
			var result = InvokeStaticPrivateMethodClass(
				typeof(CartesianChartArea), "GetYValue", seriesList, 0.0, 0);

			Assert.Equal(0.0, (double)result);
		}

		[Fact]
		public void GetYValue_WithNaNValues_IgnoresNaNInTotal()
		{
			// Series with NaN values should be treated as 0 in total calculation
			var series1 = new StackingColumn100Series();
			SetPrivateField(series1, "<YValues>k__BackingField", new List<double> { double.NaN } as IList<double>);

			var series2 = new StackingColumn100Series();
			SetPrivateField(series2, "<YValues>k__BackingField", new List<double> { 40.0 } as IList<double>);

			var series3 = new StackingColumn100Series();
			SetPrivateField(series3, "<YValues>k__BackingField", new List<double> { 60.0 } as IList<double>);

			var seriesList = new List<StackingSeriesBase> { series1, series2, series3 };

			// Total = 0 + 40 + 60 = 100, yValue=40 → (40/100)*100 = 40
			var result = InvokeStaticPrivateMethodClass(
				typeof(CartesianChartArea), "GetYValue", seriesList, 40.0, 0);

			Assert.Equal(40.0, (double)result);
		}

		[Fact]
		public void GetYValue_SkipsSeriesWithInsufficientData()
		{
			// Series1 has only 1 element, request index 1 → should be skipped
			var series1 = new StackingColumn100Series();
			SetPrivateField(series1, "<YValues>k__BackingField", new List<double> { 100.0 } as IList<double>);

			var series2 = new StackingColumn100Series();
			SetPrivateField(series2, "<YValues>k__BackingField", new List<double> { 10.0, 50.0 } as IList<double>);

			var series3 = new StackingColumn100Series();
			SetPrivateField(series3, "<YValues>k__BackingField", new List<double> { 20.0, 50.0 } as IList<double>);

			var seriesList = new List<StackingSeriesBase> { series1, series2, series3 };

			// At index 1: series1 skipped (count=1), total = 50+50 = 100
			// yValue=50 → (50/100)*100 = 50
			var result = InvokeStaticPrivateMethodClass(
				typeof(CartesianChartArea), "GetYValue", seriesList, 50.0, 1);

			Assert.Equal(50.0, (double)result);
		}

		[Fact]
		public void GetYValue_WithNegativeValues_UsesMathAbs()
		{
			var series1 = new StackingColumn100Series();
			SetPrivateField(series1, "<YValues>k__BackingField", new List<double> { -30.0 } as IList<double>);

			var series2 = new StackingColumn100Series();
			SetPrivateField(series2, "<YValues>k__BackingField", new List<double> { 70.0 } as IList<double>);

			var seriesList = new List<StackingSeriesBase> { series1, series2 };

			// Total = Math.Abs(-30) + Math.Abs(70) = 100
			// yValue=70 → (70/100)*100 = 70
			var result = InvokeStaticPrivateMethodClass(
				typeof(CartesianChartArea), "GetYValue", seriesList, 70.0, 0);

			Assert.Equal(70.0, (double)result);
		}

		#endregion
	}
}
