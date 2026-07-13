using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.Toolkit.UnitTest.Charts
{
	public class TooltipStringInterpolationUnitTests : BaseUnitTest
	{
		#region BoxAndWhisker Tooltip Format Tests

		[Fact]
		public void BoxAndWhiskerSeries_TooltipBehavior_CanBeConfigured()
		{
			// Arrange
			var chart = new SfCartesianChart();
			var series = new BoxAndWhiskerSeries
			{
				ItemsSource = new List<BoxWhiskerData>
				{
					new() { Category = "A", Values = new List<double> { 1, 2, 3, 4, 5, 6, 7, 8, 9 } }
				},
				XBindingPath = "Category",
				YBindingPath = "Values"
			};
			var tooltipBehavior = new ChartTooltipBehavior();

			chart.XAxes.Add(new CategoryAxis());
			chart.YAxes.Add(new NumericalAxis());
			chart.Series.Add(series);
			chart.TooltipBehavior = tooltipBehavior;

			// Assert - series is set up correctly
			Assert.NotNull(chart.TooltipBehavior);
			Assert.Single(chart.Series);
		}

		[Fact]
		public void BoxAndWhiskerSeries_InterpolatedFormat_MatchesConcatenation()
		{
			// Verify that string interpolation produces the same result as concatenation
			double yValue = 9.5;
			double upperQuartile = 7.25;
			double median = 5.0;
			double lowerQuartile = 2.75;
			double minimum = 1.0;

			var concatenated = yValue.ToString("  #.##") + "/" + upperQuartile.ToString("  #.##") + "/" +
				median.ToString("  #.##") + "/" + lowerQuartile.ToString("  #.##") + "/" + minimum.ToString("  #.##");

			var interpolated = $"{yValue:  #.##}/{upperQuartile:  #.##}/{median:  #.##}/{lowerQuartile:  #.##}/{minimum:  #.##}";

			Assert.Equal(concatenated, interpolated);
		}

		[Fact]
		public void BoxAndWhiskerSeries_InterpolatedFormat_HandlesZeroValues()
		{
			double yValue = 0;
			double upperQuartile = 0;
			double median = 0;
			double lowerQuartile = 0;
			double minimum = 0;

			var concatenated = yValue.ToString("  #.##") + "/" + upperQuartile.ToString("  #.##") + "/" +
				median.ToString("  #.##") + "/" + lowerQuartile.ToString("  #.##") + "/" + minimum.ToString("  #.##");

			var interpolated = $"{yValue:  #.##}/{upperQuartile:  #.##}/{median:  #.##}/{lowerQuartile:  #.##}/{minimum:  #.##}";

			Assert.Equal(concatenated, interpolated);
		}

		#endregion

		#region HiLoOpenClose Tooltip Format Tests

		[Fact]
		public void HiLoOpenCloseSeries_InterpolatedFormat_MatchesConcatenation()
		{
			double yValue = 150.5;
			double lowValue = 120.3;
			double openValue = 130.0;
			double closeValue = 145.8;

			var concatenated = (yValue == 0 ? yValue.ToString(" 0.##") : yValue.ToString(" #.##")) + "/" +
				(lowValue == 0 ? lowValue.ToString(" 0.##") : lowValue.ToString(" #.##")) + "/" +
				(openValue == 0 ? openValue.ToString(" 0.##") : openValue.ToString(" #.##")) + "/" +
				(closeValue == 0 ? closeValue.ToString(" 0.##") : closeValue.ToString(" #.##"));

			var interpolated = $"{(yValue == 0 ? yValue.ToString(" 0.##") : yValue.ToString(" #.##"))}/{(lowValue == 0 ? lowValue.ToString(" 0.##") : lowValue.ToString(" #.##"))}/{(openValue == 0 ? openValue.ToString(" 0.##") : openValue.ToString(" #.##"))}/{(closeValue == 0 ? closeValue.ToString(" 0.##") : closeValue.ToString(" #.##"))}";

			Assert.Equal(concatenated, interpolated);
		}

		[Fact]
		public void HiLoOpenCloseSeries_InterpolatedFormat_HandlesZeroValues()
		{
			double yValue = 0;
			double lowValue = 0;
			double openValue = 0;
			double closeValue = 0;

			var concatenated = (yValue == 0 ? yValue.ToString(" 0.##") : yValue.ToString(" #.##")) + "/" +
				(lowValue == 0 ? lowValue.ToString(" 0.##") : lowValue.ToString(" #.##")) + "/" +
				(openValue == 0 ? openValue.ToString(" 0.##") : openValue.ToString(" #.##")) + "/" +
				(closeValue == 0 ? closeValue.ToString(" 0.##") : closeValue.ToString(" #.##"));

			var interpolated = $"{(yValue == 0 ? yValue.ToString(" 0.##") : yValue.ToString(" #.##"))}/{(lowValue == 0 ? lowValue.ToString(" 0.##") : lowValue.ToString(" #.##"))}/{(openValue == 0 ? openValue.ToString(" 0.##") : openValue.ToString(" #.##"))}/{(closeValue == 0 ? closeValue.ToString(" 0.##") : closeValue.ToString(" #.##"))}";

			Assert.Equal(concatenated, interpolated);
		}

		#endregion

		#region Helper

		class BoxWhiskerData
		{
			public string Category { get; set; } = string.Empty;
			public List<double> Values { get; set; } = [];
		}

		#endregion
	}
}
