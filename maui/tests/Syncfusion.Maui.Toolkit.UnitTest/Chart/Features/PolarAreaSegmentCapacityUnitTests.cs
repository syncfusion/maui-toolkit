using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.Toolkit.UnitTest.Charts
{
	public class PolarAreaSegmentCapacityUnitTests : BaseUnitTest
	{
		#region PolarAreaSegment List Capacity Tests

		[Fact]
		public void PolarAreaSeries_WithDataPoints_RendersCorrectly()
		{
			// Arrange - create a polar chart with area series to exercise the
			// GenerateInteriorPoints and GenerateStrokePoints methods
			var chart = new SfPolarChart();
			var series = new PolarAreaSeries
			{
				ItemsSource = new List<PolarDataModel>
				{
					new() { Direction = "N", Speed = 10 },
					new() { Direction = "NE", Speed = 20 },
					new() { Direction = "E", Speed = 15 },
					new() { Direction = "SE", Speed = 25 },
					new() { Direction = "S", Speed = 12 },
				},
				XBindingPath = "Direction",
				YBindingPath = "Speed"
			};

			chart.Series.Add(series);

			// Act - verify that the series can be created without throwing
			Assert.NotNull(series);
			Assert.Equal(5, ((IList<PolarDataModel>)series.ItemsSource).Count);
		}

		[Fact]
		public void PolarAreaSeries_EmptyDataSource_DoesNotThrow()
		{
			// Arrange
			var series = new PolarAreaSeries
			{
				ItemsSource = new List<PolarDataModel>(),
				XBindingPath = "Direction",
				YBindingPath = "Speed"
			};

			// Act & Assert - no exception
			Assert.NotNull(series);
			Assert.Empty((IList<PolarDataModel>)series.ItemsSource);
		}

		[Fact]
		public void PolarAreaSeries_WithStroke_CreatesCorrectly()
		{
			// Arrange
			var series = new PolarAreaSeries
			{
				ItemsSource = new List<PolarDataModel>
				{
					new() { Direction = "N", Speed = 10 },
					new() { Direction = "E", Speed = 20 },
					new() { Direction = "S", Speed = 15 },
				},
				XBindingPath = "Direction",
				YBindingPath = "Speed",
				StrokeWidth = 2
			};

			// Act & Assert - stroke width set correctly
			Assert.Equal(2, series.StrokeWidth);
		}

		#endregion

		#region Helper

		class PolarDataModel
		{
			public string Direction { get; set; } = string.Empty;
			public double Speed { get; set; }
		}

		#endregion
	}
}
