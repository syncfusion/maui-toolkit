using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.Toolkit.UnitTest
{
	public class SfCircularChartUnitTest
	{
		[Fact]
		public void Series_SetAndGet_ReturnsExpectedValue()
		{
			var chart = new SfCircularChart();
			var seriesCollection = new ChartSeriesCollection
			{
				new DoughnutSeries()
			};

			chart.Series = seriesCollection;

			Assert.Equal(seriesCollection, chart.Series);
		}
	}
}
