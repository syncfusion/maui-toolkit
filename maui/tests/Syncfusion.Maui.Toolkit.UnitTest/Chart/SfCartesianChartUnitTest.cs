using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.Toolkit.UnitTest.Charts
{
	public class SfCartesianChartUnitTest
	{
		[Fact]
		public void Constructor_InitializesDefaultsCorrectly()
		{
			var chart = new SfCartesianChart();

			Assert.Empty(chart.Series);

			Assert.Empty(chart.XAxes);

			Assert.Empty(chart.YAxes);

			Assert.True(chart.EnableSideBySideSeriesPlacement);

			Assert.False(chart.IsTransposed);

			var defaultBrushes = chart.PaletteBrushes;

			Assert.Equal(10, defaultBrushes.Count);
			Assert.Equal(Color.FromRgba(0, 189, 174, 255), (defaultBrushes[0] as SolidColorBrush)?.Color);

			Assert.Null(chart.ZoomPanBehavior);

			Assert.Null(chart.SelectionBehavior);

			Assert.Null(chart.TrackballBehavior);

			Assert.NotNull(chart.Annotations);
		}

		[Theory]
		[InlineData(0)]
		[InlineData(1)]
		public void Series_SetAndGet_ReturnsExpectedValue(int seriesCount)
		{
			var chart = new SfCartesianChart();
			var expectedSeries = new ChartSeriesCollection();

			for (int i = 0; i < seriesCount; i++)
			{
				expectedSeries.Add(new LineSeries());
			}

			chart.Series = expectedSeries;

			Assert.Equal(expectedSeries.Count, chart.Series.Count);
		}


		[Fact]
		public void XAxes_SetAndGet_ReturnsExpectedValue()
		{
			var chart = new SfCartesianChart();

			var xAxis = new NumericalAxis();

			chart.XAxes.Add(xAxis);

			Assert.Single(chart.XAxes);
			Assert.Contains(xAxis, chart.XAxes);
		}

		[Fact]
		public void YAxes_SetAndGet_ReturnsExpectedValue()
		{
			var chart = new SfCartesianChart();

			var yAxis = new NumericalAxis();

			chart.YAxes.Add(yAxis);

			Assert.Single(chart.YAxes);
			Assert.Contains(yAxis, chart.YAxes);
		}


		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void EnableSideBySideSeriesPlacement_SetAndGet_ReturnsExpectedValue(bool expected)
		{
			var chart = new SfCartesianChart
			{
				EnableSideBySideSeriesPlacement = expected
			};

			Assert.Equal(expected, chart.EnableSideBySideSeriesPlacement);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void IsTransposed_SetAndGet_ReturnsExpectedValue(bool expected)
		{
			var chart = new SfCartesianChart
			{
				IsTransposed = expected
			};

			Assert.Equal(expected, chart.IsTransposed);
		}

		[Fact]
		public void PaletteBrushes_SetAndGet_ReturnsExpectedValue()
		{
			var chart = new SfCartesianChart();
			var expectedBrushes = new List<Brush>
			{
				new SolidColorBrush(Colors.Red),
				new SolidColorBrush(Colors.Blue)
			};

			chart.PaletteBrushes = expectedBrushes;

			Assert.Equal(expectedBrushes, chart.PaletteBrushes);
		}

		[Fact]
		public void ZoomPanBehavior_SetAndGet_ReturnsExpectedValue()
		{
			var chart = new SfCartesianChart();
			var zoomPanBehavior = new ChartZoomPanBehavior
			{
				EnableDoubleTap = true,
				EnablePinchZooming = true,
				EnablePanning = true
			};

			chart.ZoomPanBehavior = zoomPanBehavior;

			Assert.Equal(zoomPanBehavior, chart.ZoomPanBehavior);
		}

		[Fact]
		public void SelectionBehavior_SetAndGet_ReturnsExpectedValue()
		{
			var chart = new SfCartesianChart();
			var selectionBehavior = new SeriesSelectionBehavior { SelectionBrush = Colors.Blue };

			chart.SelectionBehavior = selectionBehavior;

			Assert.Equal(selectionBehavior, chart.SelectionBehavior);
		}

		[Fact]
		public void TrackballBehavior_SetAndGet_ReturnsExpectedValue()
		{
			var chart = new SfCartesianChart();
			var trackballBehavior = new ChartTrackballBehavior();

			chart.TrackballBehavior = trackballBehavior;

			Assert.Equal(trackballBehavior, chart.TrackballBehavior);
		}

		[Fact]
		public void Annotations_SetAndGet_ReturnsExpectedValue()
		{
			var chart = new SfCartesianChart();
			var annotations = new ChartAnnotationCollection
			{
				new VerticalLineAnnotation { X1 = 1 }
			};

			chart.Annotations = annotations;

			Assert.Single(chart.Annotations);
			Assert.Equal(1, chart.Annotations[0].X1);
		}
	}
}
