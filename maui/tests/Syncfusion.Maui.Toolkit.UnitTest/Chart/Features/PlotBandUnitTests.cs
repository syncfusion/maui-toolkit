using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.Toolkit.UnitTest
{
	public class PlotBandUnitTests : BaseUnitTest
	{
		[Theory]
		[InlineData(10.0, 40.0, 30.0)]
		[InlineData(50.0, 30.0, 20.0)]

		public void GetBandWidth_VariousInputs_ReturnsExpectedWidth(double startBand, double end, double expected)
		{
			var numericalPlotBand = new NumericalPlotBand { End = end, Start = startBand };
			var result = (double?)InvokePrivateMethod(numericalPlotBand, "GetBandWidth", [startBand]);
			Assert.Equal(expected, result);
		}

		[Fact]
		public void GetActualPeriodStrip_RepeatEveryIsNaN_ReturnsNaN()
		{
			var dateTimePlotBand = new DateTimePlotBand
			{
				RepeatEvery = double.NaN
			};
			double start = 10;
			var result = (double?)InvokePrivateMethod(dateTimePlotBand, "GetActualPeriodStrip", [start]);
			Assert.True(double.IsNaN(result ?? double.NaN));
		}
	}
}
