using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.Toolkit.UnitTest
{
	public partial class SeriesDefaultTests
	{
		[Fact]
		public void FastScatterSeriesDefaultTests_Part1()
		{
			var series = new FastScatterSeries();
			Assert.NotNull(series.YValues);
			Assert.Empty(series.YValues);
			Assert.Null(series.YBindingPath);
			Assert.Equal(1d, series.StrokeWidth);
		}

		[Fact]
		public void FastScatterSeriesDefaultTests_Part2()
		{
			var series = new FastScatterSeries();
			Assert.Null(series.ActualXAxis);
			Assert.Null(series.ActualYAxis);
			Assert.Equal(string.Empty, series.Label);
			Assert.True(series.ShowTrackballLabel);
			Assert.Null(series.TrackballLabelTemplate);
			Assert.Null(series.XAxisName);
			Assert.Null(series.YAxisName);
		}

		[Fact]
		public void FastScatterSeriesDefaultTests_Part3()
		{
			var series = new FastScatterSeries();
			Assert.True(series.IsVisible);
			Assert.True(series.IsVisibleOnLegend);
			Assert.Null(series.ItemsSource);
			Assert.Equal(LabelContext.YValue, series.LabelContext);
			Assert.Null(series.LabelTemplate);
			Assert.Equal(ChartLegendIconType.Circle, series.LegendIcon);
			Assert.Null(series.PaletteBrushes);
			Assert.False(series.ShowDataLabels);
			Assert.Null(series.TooltipTemplate);
		}

		[Fact]
		public void FastScatterSeriesDefaultTests_Part4()
		{
			var series = new FastScatterSeries();
			Assert.False(series.EnableTooltip);
			Assert.Null(series.Fill);
			Assert.Null(series.XBindingPath);
			Assert.Null(series.SelectionBehavior);
			Assert.Equal(1d, series.Opacity);
			Assert.False(series.EnableAnimation);
			Assert.False(series.IsSideBySide);
		}

		[Fact]
		public void FastScatterSeriesDefaultTests_Part5()
		{
			var series = new FastScatterSeries();
			Assert.Equal(series.DataLabelSettings, series.ChartDataLabelSettings);
			Assert.Null(series.ChartArea);
			Assert.Equal(0, series.SideBySideIndex);
			Assert.False(series.IsSbsValueCalculated);
		}

		[Fact]
		public void FastScatterSeriesDefaultTests_Part6()
		{
			var series = new FastScatterSeries();
			Assert.Null(series.Chart);
			Assert.Equal(1d, series.AnimationDuration);
			Assert.Equal(1, series.AnimationValue);
			Assert.Null(series.SeriesAnimation);
			Assert.False(series.NeedToAnimateSeries);
			Assert.False(series.NeedToAnimateDataLabel);
			Assert.False(series.SegmentsCreated);
		}

		[Fact]
		public void FastScatterSeriesDefaultTests_Part7()
		{
			var series = new FastScatterSeries();
			Assert.False(series.VisibleXRange.IsEmpty);
			Assert.False(series.VisibleYRange.IsEmpty);
			Assert.False(series.PreviousXRange.IsEmpty);
			Assert.Null(series.OldSegments);
			Assert.Equal(0, series.TooltipDataPointIndex);
			Assert.Empty(series.GroupedXValuesIndexes);
			Assert.Empty(series.GroupedActualData);
			Assert.Empty(series.GroupedXValues);
		}

		[Fact]
		public void FastScatterSeriesDefaultTests_Part8()
		{
			var series = new FastScatterSeries();
			Assert.NotNull(series.DataLabels);
			Assert.NotNull(series.LabelTemplateView);
			Assert.False(series.IsMultipleYPathRequired);
			Assert.Equal(0, series.XData);
			Assert.Equal(0, series.PointsCount);
			Assert.Equal(ChartValueType.Double, series.XValueType);
		}

		[Fact]
		public void FastScatterSeriesDefaultTests_Part9()
		{
			var series = new FastScatterSeries();
			Assert.Null(series.XValues);
			Assert.Null(series.YComplexPaths);
			Assert.Null(series.ActualXValues);
			Assert.Null(series.SeriesYValues);
			Assert.Null(series.ActualSeriesYValues);
			Assert.Null(series.YPaths);
			Assert.Null(series.ActualData);
			Assert.Null(series.XComplexPaths);
		}

		[Fact]
		public void FastScatterSeriesDefaultTests_Part10()
		{
			var series = new FastScatterSeries();
			Assert.True(series.IsLinearData);
			Assert.False(series.IsDataPointAddedDynamically);
			Assert.False(series.IsStacking);
			Assert.True(series.XRange.IsEmpty);
			Assert.True(series.YRange.IsEmpty);
		}
	}
}
