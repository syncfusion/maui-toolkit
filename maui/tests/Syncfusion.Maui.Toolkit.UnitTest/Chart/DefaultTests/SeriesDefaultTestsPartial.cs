using System.Collections.ObjectModel;
using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.Toolkit.UnitTest
{
	public partial class SeriesDefaultTests
	{
		[Fact]
		public void RangeColumnSeriesDefaultTests_Part1()
		{
			var series = new RangeColumnSeries();
			Assert.Equal(0.0d, series.Spacing);
			Assert.Equal(0.8d, series.Width);
			Assert.Equal(string.Empty, series.High);
			Assert.Equal(string.Empty, series.Low);
			Assert.True(series.IsSideBySide);
		}

		[Fact]
		public void RangeColumnSeriesDefaultTests_Part2()
		{
			var series = new RangeColumnSeries();
			Assert.Null(series.ActualXAxis);
			Assert.Null(series.ActualYAxis);
			Assert.Equal(string.Empty, series.Label);
			Assert.True(series.ShowTrackballLabel);
			Assert.Null(series.TrackballLabelTemplate);
			Assert.Null(series.XAxisName);
			Assert.Null(series.YAxisName);
		}

		[Fact]
		public void RangeColumnSeriesDefaultTests_Part3()
		{
			var series = new RangeColumnSeries();
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
		public void RangeColumnSeriesDefaultTests_Part4()
		{
			var series = new RangeColumnSeries();
			Assert.False(series.EnableTooltip);
			Assert.Null(series.Fill);
			Assert.Null(series.XBindingPath);
			Assert.Null(series.SelectionBehavior);
			Assert.Equal(1d, series.Opacity);
			Assert.False(series.EnableAnimation);
			Assert.True(series.IsSideBySide);
			Assert.False(series.ListenPropertyChange);
		}

		[Fact]
		public void RangeColumnSeriesDefaultTests_Part5()
		{
			var series = new RangeColumnSeries();
			Assert.Equal(series.DataLabelSettings, series.ChartDataLabelSettings);
			Assert.Null(series.ChartArea);
			Assert.Equal(0, series.SideBySideIndex);
			Assert.False(series.IsSbsValueCalculated);
		}

		[Fact]
		public void RangeColumnSeriesDefaultTests_Part6()
		{
			var series = new RangeColumnSeries();
			Assert.Null(series.Chart);
			Assert.Equal(1d, series.AnimationDuration);
			Assert.Equal(1, series.AnimationValue);
			Assert.Null(series.SeriesAnimation);
			Assert.False(series.NeedToAnimateSeries);
			Assert.False(series.NeedToAnimateDataLabel);
			Assert.False(series.SegmentsCreated);
		}

		[Fact]
		public void RangeColumnSeriesDefaultTests_Part7()
		{
			var series = new RangeColumnSeries();
			Assert.Null(series.OldSegments);
			Assert.Equal(0, series.TooltipDataPointIndex);
			Assert.Empty(series.GroupedXValuesIndexes);
			Assert.Empty(series.GroupedActualData);
			Assert.Empty(series.GroupedXValues);
			Assert.NotNull(series.DataLabels);
			Assert.NotNull(series.LabelTemplateView);
		}

		[Fact]
		public void RangeColumnSeriesDefaultTests_Part8()
		{
			var series = new RangeColumnSeries();
			Assert.Equal(0, series.XData);
			Assert.Equal(0, series.PointsCount);
			Assert.Equal(ChartValueType.Double, series.XValueType);
			Assert.Null(series.XValues);
			Assert.Null(series.YComplexPaths);
			Assert.Null(series.ActualXValues);
			Assert.Null(series.SeriesYValues);
			Assert.Null(series.ActualSeriesYValues);
		}

		[Fact]
		public void RangeColumnSeriesDefaultTests_Part9()
		{
			var series = new RangeColumnSeries();
			Assert.Null(series.YPaths);
			Assert.Null(series.ActualData);
			Assert.Null(series.XComplexPaths);
			Assert.True(series.IsLinearData);
			Assert.False(series.IsDataPointAddedDynamically);
			Assert.False(series.IsStacking);
		}

		[Fact]
		public void RangeColumnSeriesDefaultTests_Part10()
		{
			var series = new RangeColumnSeries();
			Assert.True(series.XRange.IsEmpty);
			Assert.True(series.YRange.IsEmpty);
			Assert.False(series.VisibleXRange.IsEmpty);
			Assert.False(series.VisibleYRange.IsEmpty);
			Assert.False(series.PreviousXRange.IsEmpty);
		}

		[Fact]
		public void RangeColumnSeriesDefaultTests_Part11()
		{
			var series = new RangeColumnSeries();
			Assert.NotNull(series.HighValues);
			Assert.NotNull(series.LowValues);
			Assert.Empty(series.HighValues);
			Assert.Empty(series.LowValues);
			Assert.False(series.IsMultipleYPathRequired);
			Assert.Null(series.Stroke);
			Assert.Equal(1d, series.StrokeWidth);
			Assert.Null(series.StrokeDashArray);
		}

		[Fact]
		public void RangeColumnSeriesDefaultTests_Part12()
		{
			var series = new RangeColumnSeries();

			// Test that DataLabelSettings is not null and is of the correct type
			Assert.IsType<CartesianDataLabelSettings>(series.DataLabelSettings);
			var dataLabelSettings = series.DataLabelSettings as CartesianDataLabelSettings;
			Assert.NotNull(dataLabelSettings);

			// Test default values of CartesianDataLabelSettings properties
			Assert.Equal(DataLabelAlignment.Top, dataLabelSettings.BarAlignment);

			// Test default values inherited from ChartDataLabelSettings
			Assert.True(dataLabelSettings.UseSeriesPalette);
			Assert.Equal(DataLabelPlacement.Auto, dataLabelSettings.LabelPlacement);

			// Test LabelStyle properties
			Assert.NotNull(dataLabelSettings.LabelStyle);
			Assert.IsType<ChartDataLabelStyle>(dataLabelSettings.LabelStyle);
			Assert.Equal(12, dataLabelSettings.LabelStyle.FontSize);
			Assert.Equal(new Thickness(5), dataLabelSettings.LabelStyle.Margin);

			// Test other properties
			Assert.Equal(Colors.Transparent, dataLabelSettings.LabelStyle.Background);
			Assert.Equal(Brush.Transparent, dataLabelSettings.LabelStyle.Stroke);
			Assert.Equal(0, dataLabelSettings.LabelStyle.StrokeWidth);
			Assert.Null(dataLabelSettings.LabelStyle.FontFamily);
			Assert.Equal(FontAttributes.None, dataLabelSettings.LabelStyle.FontAttributes);
			Assert.Equal(Colors.Transparent, dataLabelSettings.LabelStyle.TextColor);
			Assert.Equal(string.Empty, dataLabelSettings.LabelStyle.LabelFormat);
		}
		[Fact]
		public void RangeAreaSeriesDefaultTests_Part1()
		{
			var series = new RangeAreaSeries();
			Assert.False(series.ShowMarkers);
			Assert.NotNull(series.MarkerSettings);
			Assert.IsType<ChartMarkerSettings>(series.MarkerSettings);
			Assert.Equal(string.Empty, series.High);
			Assert.Equal(string.Empty, series.Low);
			Assert.True(series.IsMultipleYPathRequired);
		}

		[Fact]
		public void RangeAreaSeriesDefaultTests_Part2()
		{
			var series = new RangeAreaSeries();
			Assert.Null(series.ActualXAxis);
			Assert.Null(series.ActualYAxis);
			Assert.Equal(string.Empty, series.Label);
			Assert.True(series.ShowTrackballLabel);
			Assert.Null(series.TrackballLabelTemplate);
			Assert.Null(series.XAxisName);
			Assert.Null(series.YAxisName);
		}

		[Fact]
		public void RangeAreaSeriesDefaultTests_Part3()
		{
			var series = new RangeAreaSeries();
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
		public void RangeAreaSeriesDefaultTests_Part4()
		{
			var series = new RangeAreaSeries();
			Assert.False(series.EnableTooltip);
			Assert.Null(series.Fill);
			Assert.Null(series.XBindingPath);
			Assert.Null(series.SelectionBehavior);
			Assert.Equal(1d, series.Opacity);
			Assert.False(series.EnableAnimation);
			Assert.False(series.IsSideBySide);
			Assert.False(series.ListenPropertyChange);
		}

		[Fact]
		public void RangeAreaSeriesDefaultTests_Part5()
		{
			var series = new RangeAreaSeries();
			Assert.Equal(series.DataLabelSettings, series.ChartDataLabelSettings);
			Assert.Null(series.ChartArea);
			Assert.Equal(0, series.SideBySideIndex);
			Assert.False(series.IsSbsValueCalculated);
		}

		[Fact]
		public void RangeAreaSeriesDefaultTests_Part6()
		{
			var series = new RangeAreaSeries();
			Assert.Null(series.Chart);
			Assert.Equal(1d, series.AnimationDuration);
			Assert.Equal(1, series.AnimationValue);
			Assert.Null(series.SeriesAnimation);
			Assert.False(series.NeedToAnimateSeries);
			Assert.False(series.NeedToAnimateDataLabel);
			Assert.False(series.SegmentsCreated);
		}

		[Fact]
		public void RangeAreaSeriesDefaultTests_Part7()
		{
			var series = new RangeAreaSeries();
			Assert.Null(series.OldSegments);
			Assert.Equal(0, series.TooltipDataPointIndex);
			Assert.Empty(series.GroupedXValuesIndexes);
			Assert.Empty(series.GroupedActualData);
			Assert.Empty(series.GroupedXValues);
			Assert.NotNull(series.DataLabels);
			Assert.NotNull(series.LabelTemplateView);
		}

		[Fact]
		public void RangeAreaSeriesDefaultTests_Part8()
		{
			var series = new RangeAreaSeries();
			Assert.False(series.IsSideBySide);
			Assert.Equal(0, series.XData);
			Assert.Equal(0, series.PointsCount);
			Assert.Equal(ChartValueType.Double, series.XValueType);
			Assert.Null(series.XValues);
			Assert.Null(series.YComplexPaths);
			Assert.Null(series.ActualXValues);
			Assert.Null(series.SeriesYValues);
			Assert.Null(series.ActualSeriesYValues);
		}

		[Fact]
		public void RangeAreaSeriesDefaultTests_Part9()
		{
			var series = new RangeAreaSeries();
			Assert.Null(series.YPaths);
			Assert.Null(series.ActualData);
			Assert.Null(series.XComplexPaths);
			Assert.True(series.IsLinearData);
			Assert.False(series.IsDataPointAddedDynamically);
			Assert.False(series.IsStacking);
		}

		[Fact]
		public void RangeAreaSeriesDefaultTests_Part10()
		{
			var series = new RangeAreaSeries();
			Assert.True(series.XRange.IsEmpty);
			Assert.True(series.YRange.IsEmpty);
			Assert.False(series.VisibleXRange.IsEmpty);
			Assert.False(series.VisibleYRange.IsEmpty);
			Assert.False(series.PreviousXRange.IsEmpty);
		}

		[Fact]
		public void RangeAreaSeriesDefaultTests_Part11()
		{
			var series = new RangeAreaSeries();
			Assert.NotNull(series.HighValues);
			Assert.NotNull(series.LowValues);
			Assert.Empty(series.HighValues);
			Assert.Empty(series.LowValues);
			Assert.Null(series.Stroke);
			Assert.Equal(1d, series.StrokeWidth);
			Assert.Null(series.StrokeDashArray);
		}

		[Fact]
		public void RangeAreaSeriesDefaultTests_Part12()
		{
			var series = new RangeAreaSeries();

			// Test that DataLabelSettings is not null and is of the correct type
			Assert.IsType<CartesianDataLabelSettings>(series.DataLabelSettings);
			var dataLabelSettings = series.DataLabelSettings as CartesianDataLabelSettings;
			Assert.NotNull(dataLabelSettings);

			// Test default values of CartesianDataLabelSettings properties
			Assert.Equal(DataLabelAlignment.Top, dataLabelSettings.BarAlignment);

			// Test default values inherited from ChartDataLabelSettings
			Assert.True(dataLabelSettings.UseSeriesPalette);
			Assert.Equal(DataLabelPlacement.Auto, dataLabelSettings.LabelPlacement);

			// Test LabelStyle properties
			Assert.NotNull(dataLabelSettings.LabelStyle);
			Assert.IsType<ChartDataLabelStyle>(dataLabelSettings.LabelStyle);
			Assert.Equal(12, dataLabelSettings.LabelStyle.FontSize);
			Assert.Equal(new Thickness(5), dataLabelSettings.LabelStyle.Margin);

			// Test other properties
			Assert.Equal(Colors.Transparent, dataLabelSettings.LabelStyle.Background);
			Assert.Equal(Brush.Transparent, dataLabelSettings.LabelStyle.Stroke);
			Assert.Equal(0, dataLabelSettings.LabelStyle.StrokeWidth);
			Assert.Null(dataLabelSettings.LabelStyle.FontFamily);
			Assert.Equal(FontAttributes.None, dataLabelSettings.LabelStyle.FontAttributes);
			Assert.Equal(Colors.Transparent, dataLabelSettings.LabelStyle.TextColor);
			Assert.Equal(string.Empty, dataLabelSettings.LabelStyle.LabelFormat);
		}

		[Fact]
		public void LineSeriesDefaultTests_Part1()
		{
			var series = new LineSeries();
			Assert.Null(series.StrokeDashArray);
			Assert.False(series.ShowMarkers);
			Assert.NotNull(series.MarkerSettings);
			Assert.IsType<ChartMarkerSettings>(series.MarkerSettings);
			Assert.Equal(1d, series.StrokeWidth);
			Assert.Null(series.YBindingPath);
			Assert.NotNull(series.YValues);
			Assert.Empty(series.YValues);
		}

		[Fact]
		public void LineSeriesDefaultTests_Part2()
		{
			var series = new LineSeries();
			Assert.Null(series.ActualXAxis);
			Assert.Null(series.ActualYAxis);
			Assert.Equal(string.Empty, series.Label);
			Assert.True(series.ShowTrackballLabel);
			Assert.Null(series.TrackballLabelTemplate);
			Assert.Null(series.XAxisName);
			Assert.Null(series.YAxisName);
		}

		[Fact]
		public void LineSeriesDefaultTests_Part3()
		{
			var series = new LineSeries();
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
		public void LineSeriesDefaultTests_Part4()
		{
			var series = new LineSeries();
			Assert.False(series.EnableTooltip);
			Assert.Null(series.Fill);
			Assert.Null(series.XBindingPath);
			Assert.Null(series.SelectionBehavior);
			Assert.Equal(1d, series.Opacity);
			Assert.False(series.EnableAnimation);
			Assert.False(series.IsSideBySide);
			Assert.False(series.ListenPropertyChange);
		}

		[Fact]
		public void LineSeriesDefaultTests_Part5()
		{
			var series = new LineSeries();
			Assert.Null(series.Chart);
			Assert.Equal(1d, series.AnimationDuration);
			Assert.Equal(1, series.AnimationValue);
			Assert.Null(series.SeriesAnimation);
			Assert.False(series.NeedToAnimateSeries);
			Assert.False(series.NeedToAnimateDataLabel);
			Assert.False(series.SegmentsCreated);
		}

		[Fact]
		public void LineSeriesDefaultTests_Part6()
		{
			var series = new LineSeries();
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
		public void LineSeriesDefaultTests_Part7()
		{
			var series = new LineSeries();
			Assert.NotNull(series.DataLabels);
			Assert.NotNull(series.LabelTemplateView);
			Assert.False(series.IsMultipleYPathRequired);
			Assert.False(series.IsSideBySide);
			Assert.Equal(0, series.XData);
			Assert.Equal(0, series.PointsCount);
			Assert.Equal(ChartValueType.Double, series.XValueType);
		}

		[Fact]
		public void LineSeriesDefaultTests_Part8()
		{
			var series = new LineSeries();
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
		public void LineSeriesDefaultTests_Part9()
		{
			var series = new LineSeries();
			Assert.True(series.IsLinearData);
			Assert.False(series.IsDataPointAddedDynamically);
			Assert.False(series.IsStacking);
			Assert.True(series.XRange.IsEmpty);
			Assert.True(series.YRange.IsEmpty);
		}

		[Fact]
		public void LineSeriesDefaultTests_Part10()
		{
			var series = new LineSeries();

			// Test that DataLabelSettings is not null and is of the correct type
			Assert.IsType<CartesianDataLabelSettings>(series.DataLabelSettings);
			var dataLabelSettings = series.DataLabelSettings as CartesianDataLabelSettings;
			Assert.NotNull(dataLabelSettings);

			// Test default values of CartesianDataLabelSettings properties
			Assert.Equal(DataLabelAlignment.Top, dataLabelSettings.BarAlignment);

			// Test default values inherited from ChartDataLabelSettings
			Assert.True(dataLabelSettings.UseSeriesPalette);
			Assert.Equal(DataLabelPlacement.Auto, dataLabelSettings.LabelPlacement);

			// Test LabelStyle properties
			Assert.NotNull(dataLabelSettings.LabelStyle);
			Assert.IsType<ChartDataLabelStyle>(dataLabelSettings.LabelStyle);
			Assert.Equal(12, dataLabelSettings.LabelStyle.FontSize);
			Assert.Equal(new Thickness(5), dataLabelSettings.LabelStyle.Margin);

			// Test other properties
			Assert.Equal(Colors.Transparent, dataLabelSettings.LabelStyle.Background);
			Assert.Equal(Brush.Transparent, dataLabelSettings.LabelStyle.Stroke);
			Assert.Equal(0, dataLabelSettings.LabelStyle.StrokeWidth);
			Assert.Null(dataLabelSettings.LabelStyle.FontFamily);
			Assert.Equal(FontAttributes.None, dataLabelSettings.LabelStyle.FontAttributes);
			Assert.Equal(Colors.Transparent, dataLabelSettings.LabelStyle.TextColor);
			Assert.Equal(string.Empty, dataLabelSettings.LabelStyle.LabelFormat);
		}

		[Fact]
		public void HistogramSeriesDefaultTests_Part1()
		{
			var series = new HistogramSeries();
			Assert.True(series.ShowNormalDistributionCurve);
			Assert.Equal(1.0, series.HistogramInterval);
			Assert.NotNull(series.CurveStyle);
			Assert.IsType<ChartLineStyle>(series.CurveStyle);
			Assert.Equal(SolidColorBrush.Transparent, series.Stroke);
		}

		[Fact]
		public void HistogramSeriesDefaultTests_Part2()
		{
			var series = new HistogramSeries();
			Assert.Null(series.ActualXAxis);
			Assert.Null(series.ActualYAxis);
			Assert.Equal(string.Empty, series.Label);
			Assert.True(series.ShowTrackballLabel);
			Assert.Null(series.TrackballLabelTemplate);
			Assert.Null(series.XAxisName);
			Assert.Null(series.YAxisName);
		}

		[Fact]
		public void HistogramSeriesDefaultTests_Part3()
		{
			var series = new HistogramSeries();
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
		public void HistogramSeriesDefaultTests_Part4()
		{
			var series = new HistogramSeries();
			Assert.False(series.EnableTooltip);
			Assert.Null(series.Fill);
			Assert.Null(series.XBindingPath);
			Assert.Null(series.SelectionBehavior);
			Assert.Equal(1d, series.Opacity);
			Assert.False(series.EnableAnimation);
			Assert.False(series.IsSideBySide);
			Assert.False(series.ListenPropertyChange);
		}

		[Fact]
		public void HistogramSeriesDefaultTests_Part5()
		{
			var series = new HistogramSeries();
			Assert.Null(series.YBindingPath);
			Assert.NotNull(series.YValues);
			Assert.Empty(series.YValues);
		}

		[Fact]
		public void HistogramSeriesDefaultTests_Part6()
		{
			var series = new HistogramSeries();
			Assert.Null(series.Chart);
			Assert.Equal(1d, series.AnimationDuration);
			Assert.Equal(1, series.AnimationValue);
			Assert.Null(series.SeriesAnimation);
			Assert.False(series.NeedToAnimateSeries);
			Assert.False(series.NeedToAnimateDataLabel);
			Assert.False(series.SegmentsCreated);
		}

		[Fact]
		public void HistogramSeriesDefaultTests_Part7()
		{
			var series = new HistogramSeries();
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
		public void HistogramSeriesDefaultTests_Part8()
		{
			var series = new HistogramSeries();
			Assert.NotNull(series.DataLabels);
			Assert.NotNull(series.LabelTemplateView);
			Assert.False(series.IsMultipleYPathRequired);
			Assert.Equal(0, series.XData);
			Assert.Equal(0, series.PointsCount);
			Assert.Equal(ChartValueType.Double, series.XValueType);
		}

		[Fact]
		public void HistogramSeriesDefaultTests_Part9()
		{
			var series = new HistogramSeries();
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
		public void HistogramSeriesDefaultTests_Part10()
		{
			var series = new HistogramSeries();
			Assert.True(series.IsLinearData);
			Assert.False(series.IsDataPointAddedDynamically);
			Assert.False(series.IsStacking);
			Assert.True(series.XRange.IsEmpty);
			Assert.True(series.YRange.IsEmpty);
		}

		[Fact]
		public void HistogramSeriesDefaultTests_Part11()
		{
			var series = new HistogramSeries();
			Assert.False(series.IsIndexed);
			Assert.False(series.IsGrouped);
			Assert.True(series.SbsInfo.IsEmpty);
			Assert.Equal(0, series.SideBySideIndex);
			Assert.False(series.IsSbsValueCalculated);
		}

		[Fact]
		public void HistogramSeriesDefaultTests_Part12()
		{
			var series = new HistogramSeries();

			// Test that DataLabelSettings is not null and is of the correct type
			Assert.IsType<CartesianDataLabelSettings>(series.DataLabelSettings);
			var dataLabelSettings = series.DataLabelSettings as CartesianDataLabelSettings;
			Assert.NotNull(dataLabelSettings);

			// Test default values of CartesianDataLabelSettings properties
			Assert.Equal(DataLabelAlignment.Top, dataLabelSettings.BarAlignment);

			// Test default values inherited from ChartDataLabelSettings
			Assert.True(dataLabelSettings.UseSeriesPalette);
			Assert.Equal(DataLabelPlacement.Auto, dataLabelSettings.LabelPlacement);

			// Test LabelStyle properties
			Assert.NotNull(dataLabelSettings.LabelStyle);
			Assert.IsType<ChartDataLabelStyle>(dataLabelSettings.LabelStyle);
			Assert.Equal(12, dataLabelSettings.LabelStyle.FontSize);
			Assert.Equal(new Thickness(5), dataLabelSettings.LabelStyle.Margin);

			// Test other properties
			Assert.Equal(Colors.Transparent, dataLabelSettings.LabelStyle.Background);
			Assert.Equal(Brush.Transparent, dataLabelSettings.LabelStyle.Stroke);
			Assert.Equal(0, dataLabelSettings.LabelStyle.StrokeWidth);
			Assert.Null(dataLabelSettings.LabelStyle.FontFamily);
			Assert.Equal(FontAttributes.None, dataLabelSettings.LabelStyle.FontAttributes);
			Assert.Equal(Colors.Transparent, dataLabelSettings.LabelStyle.TextColor);
			Assert.Equal(string.Empty, dataLabelSettings.LabelStyle.LabelFormat);
		}

		[Fact]
		public void HiLoOpenCloseSeriesDefaultTests_Part1()
		{
			var series = new HiLoOpenCloseSeries();
			Assert.True(series.IsSideBySide);
			Assert.Empty(series.HighValues);
			Assert.Empty(series.LowValues);
			Assert.Empty(series.OpenValues);
			Assert.Empty(series.CloseValues);
			Assert.Equal(series.DataLabelSettings, series.ChartDataLabelSettings);
			Assert.Null(series.ChartArea);
			Assert.Equal(0, series.SideBySideIndex);
			Assert.False(series.IsSbsValueCalculated);
		}

		[Fact]
		public void HiLoOpenCloseSeriesDefaultTests_Part2()
		{
			var series = new HiLoOpenCloseSeries();
			Assert.IsType<SolidColorBrush>(series.BearishFill);
			Assert.Equal(Color.FromArgb("#C15146"), ((SolidColorBrush)(series.BearishFill)).Color);
			Assert.IsType<SolidColorBrush>(series.BullishFill);
			Assert.Equal(Color.FromArgb("#90A74F"), ((SolidColorBrush)(series.BullishFill)).Color);
			Assert.Equal(string.Empty, series.High);
			Assert.Equal(string.Empty, series.Low);
			Assert.Equal(string.Empty, series.Open);
			Assert.Equal(string.Empty, series.Close);
		}

		[Fact]
		public void HiLoOpenCloseSeriesDefaultTests_Part3()
		{
			var series = new HiLoOpenCloseSeries();
			Assert.Equal(0d, series.Spacing);
			Assert.Equal(0.8d, series.Width);
			Assert.True(series.IsMultipleYPathRequired);
			Assert.Equal(float.NaN, series._sumOfHighValues);
			Assert.Equal(float.NaN, series._sumOfLowValues);
			Assert.Equal(float.NaN, series._sumOfOpenValues);
			Assert.Equal(float.NaN, series._sumOfCloseValues);
		}

		[Fact]
		public void HiLoOpenCloseSeriesDefaultTests_Part4()
		{
			var series = new HiLoOpenCloseSeries();
			Assert.Null(series.ActualXAxis);
			Assert.Null(series.ActualYAxis);
			Assert.Equal(string.Empty, series.Label);
			Assert.True(series.ShowTrackballLabel);
			Assert.Null(series.TrackballLabelTemplate);
			Assert.Null(series.XAxisName);
			Assert.Null(series.YAxisName);
		}

		[Fact]
		public void HiLoOpenCloseSeriesDefaultTests_Part5()
		{
			var series = new HiLoOpenCloseSeries();
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
		public void HiLoOpenCloseSeriesDefaultTests_Part6()
		{
			var series = new HiLoOpenCloseSeries();
			Assert.False(series.EnableTooltip);
			Assert.Null(series.Fill);
			Assert.Null(series.XBindingPath);
			Assert.Null(series.SelectionBehavior);
			Assert.Equal(1d, series.Opacity);
			Assert.False(series.EnableAnimation);
			Assert.False(series.ListenPropertyChange);
		}

		[Fact]
		public void HiLoOpenCloseSeriesDefaultTests_Part7()
		{
			var series = new HiLoOpenCloseSeries();
			Assert.Null(series.Chart);
			Assert.Equal(1d, series.AnimationDuration);
			Assert.Equal(1, series.AnimationValue);
			Assert.Null(series.SeriesAnimation);
			Assert.False(series.NeedToAnimateSeries);
			Assert.False(series.NeedToAnimateDataLabel);
			Assert.False(series.SegmentsCreated);
		}

		[Fact]
		public void HiLoOpenCloseSeriesDefaultTests_Part8()
		{
			var series = new HiLoOpenCloseSeries();
			Assert.Null(series.OldSegments);
			Assert.Equal(0, series.TooltipDataPointIndex);
			Assert.Empty(series.GroupedXValuesIndexes);
			Assert.Empty(series.GroupedActualData);
			Assert.Empty(series.GroupedXValues);
			Assert.NotNull(series.DataLabels);
			Assert.NotNull(series.LabelTemplateView);
		}

		[Fact]
		public void HiLoOpenCloseSeriesDefaultTests_Part9()
		{
			var series = new HiLoOpenCloseSeries();
			Assert.Equal(0, series.XData);
			Assert.Equal(0, series.PointsCount);
			Assert.Equal(ChartValueType.Double, series.XValueType);
			Assert.Null(series.XValues);
			Assert.Null(series.YComplexPaths);
			Assert.Null(series.ActualXValues);
			Assert.Null(series.SeriesYValues);
			Assert.Null(series.ActualSeriesYValues);
		}

		[Fact]
		public void HiLoOpenCloseSeriesDefaultTests_Part10()
		{
			var series = new HiLoOpenCloseSeries();
			Assert.Null(series.YPaths);
			Assert.Null(series.ActualData);
			Assert.Null(series.XComplexPaths);
			Assert.True(series.IsLinearData);
			Assert.False(series.IsDataPointAddedDynamically);
			Assert.False(series.IsStacking);
		}

		[Fact]
		public void HiLoOpenCloseSeriesDefaultTests_Part11()
		{
			var series = new HiLoOpenCloseSeries();
			Assert.True(series.XRange.IsEmpty);
			Assert.True(series.YRange.IsEmpty);
			Assert.False(series.VisibleXRange.IsEmpty);
			Assert.False(series.VisibleYRange.IsEmpty);
			Assert.False(series.PreviousXRange.IsEmpty);
		}

		[Fact]
		public void HiLoOpenCloseSeriesDefaultTests_Part12()
		{
			var series = new HiLoOpenCloseSeries();

			// Test that DataLabelSettings is not null and is of the correct type
			Assert.IsType<CartesianDataLabelSettings>(series.DataLabelSettings);
			var dataLabelSettings = series.DataLabelSettings as CartesianDataLabelSettings;
			Assert.NotNull(dataLabelSettings);

			// Test default values of CartesianDataLabelSettings properties
			Assert.Equal(DataLabelAlignment.Top, dataLabelSettings.BarAlignment);

			// Test default values inherited from ChartDataLabelSettings
			Assert.True(dataLabelSettings.UseSeriesPalette);
			Assert.Equal(DataLabelPlacement.Auto, dataLabelSettings.LabelPlacement);

			// Test LabelStyle properties
			Assert.NotNull(dataLabelSettings.LabelStyle);
			Assert.IsType<ChartDataLabelStyle>(dataLabelSettings.LabelStyle);
			Assert.Equal(12, dataLabelSettings.LabelStyle.FontSize);
			Assert.Equal(new Thickness(5), dataLabelSettings.LabelStyle.Margin);

			// Test other properties
			Assert.Equal(Colors.Transparent, dataLabelSettings.LabelStyle.Background);
			Assert.Equal(Brush.Transparent, dataLabelSettings.LabelStyle.Stroke);
			Assert.Equal(0, dataLabelSettings.LabelStyle.StrokeWidth);
			Assert.Null(dataLabelSettings.LabelStyle.FontFamily);
			Assert.Equal(FontAttributes.None, dataLabelSettings.LabelStyle.FontAttributes);
			Assert.Equal(Colors.Transparent, dataLabelSettings.LabelStyle.TextColor);
			Assert.Equal(string.Empty, dataLabelSettings.LabelStyle.LabelFormat);
		}

		[Fact]
		public void FastLineSeriesDefaultTests_Part1()
		{
			var series = new FastLineSeries();
			Assert.Equal(1, series.ToleranceCoefficient);
			Assert.False(series.EnableAntiAliasing);
			Assert.Null(series.StrokeDashArray);
			Assert.NotNull(series.YValues);
			Assert.Empty(series.YValues);
			Assert.Null(series.YBindingPath);
			Assert.Equal(1d, series.StrokeWidth);
			Assert.False(series.ListenPropertyChange);	
		}

		[Fact]
		public void FastLineSeriesDefaultTests_Part2()
		{
			var series = new FastLineSeries();
			Assert.Null(series.ActualXAxis);
			Assert.Null(series.ActualYAxis);
			Assert.Equal(string.Empty, series.Label);
			Assert.True(series.ShowTrackballLabel);
			Assert.Null(series.TrackballLabelTemplate);
			Assert.Null(series.XAxisName);
			Assert.Null(series.YAxisName);
		}

		[Fact]
		public void FastLineSeriesDefaultTests_Part3()
		{
			var series = new FastLineSeries();
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
		public void FastLineSeriesDefaultTests_Part4()
		{
			var series = new FastLineSeries();
			Assert.False(series.EnableTooltip);
			Assert.Null(series.Fill);
			Assert.Null(series.XBindingPath);
			Assert.Null(series.SelectionBehavior);
			Assert.Equal(1d, series.Opacity);
			Assert.False(series.EnableAnimation);
			Assert.False(series.IsSideBySide);
		}

		[Fact]
		public void FastLineSeriesDefaultTests_Part5()
		{
			var series = new FastLineSeries();
			Assert.Equal(series.DataLabelSettings, series.ChartDataLabelSettings);
			Assert.Null(series.ChartArea);
			Assert.Equal(0, series.SideBySideIndex);
			Assert.False(series.IsSbsValueCalculated);
		}

		[Fact]
		public void FastLineSeriesDefaultTests_Part6()
		{
			var series = new FastLineSeries();
			Assert.Null(series.Chart);
			Assert.Equal(1d, series.AnimationDuration);
			Assert.Equal(1, series.AnimationValue);
			Assert.Null(series.SeriesAnimation);
			Assert.False(series.NeedToAnimateSeries);
			Assert.False(series.NeedToAnimateDataLabel);
			Assert.False(series.SegmentsCreated);
		}

		[Fact]
		public void FastLineSeriesDefaultTests_Part7()
		{
			var series = new FastLineSeries();
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
		public void FastLineSeriesDefaultTests_Part8()
		{
			var series = new FastLineSeries();
			Assert.NotNull(series.DataLabels);
			Assert.NotNull(series.LabelTemplateView);
			Assert.False(series.IsMultipleYPathRequired);
			Assert.Equal(0, series.XData);
			Assert.Equal(0, series.PointsCount);
			Assert.Equal(ChartValueType.Double, series.XValueType);
		}

		[Fact]
		public void FastLineSeriesDefaultTests_Part9()
		{
			var series = new FastLineSeries();
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
		public void FastLineSeriesDefaultTests_Part10()
		{
			var series = new FastLineSeries();
			Assert.True(series.IsLinearData);
			Assert.False(series.IsDataPointAddedDynamically);
			Assert.False(series.IsStacking);
			Assert.True(series.XRange.IsEmpty);
			Assert.True(series.YRange.IsEmpty);
		}

		[Fact]
		public void FastLineSeriesDefaultTests_Part11()
		{
			var series = new FastLineSeries();

			// Test that DataLabelSettings is not null and is of the correct type
			Assert.IsType<CartesianDataLabelSettings>(series.DataLabelSettings);
			var dataLabelSettings = series.DataLabelSettings as CartesianDataLabelSettings;
			Assert.NotNull(dataLabelSettings);

			// Test default values of CartesianDataLabelSettings properties
			Assert.Equal(DataLabelAlignment.Top, dataLabelSettings.BarAlignment);

			// Test default values inherited from ChartDataLabelSettings
			Assert.True(dataLabelSettings.UseSeriesPalette);
			Assert.Equal(DataLabelPlacement.Auto, dataLabelSettings.LabelPlacement);

			// Test LabelStyle properties
			Assert.NotNull(dataLabelSettings.LabelStyle);
			Assert.IsType<ChartDataLabelStyle>(dataLabelSettings.LabelStyle);
			Assert.Equal(12, dataLabelSettings.LabelStyle.FontSize);
			Assert.Equal(new Thickness(5), dataLabelSettings.LabelStyle.Margin);

			// Test other properties
			Assert.Equal(Colors.Transparent, dataLabelSettings.LabelStyle.Background);
			Assert.Equal(Brush.Transparent, dataLabelSettings.LabelStyle.Stroke);
			Assert.Equal(0, dataLabelSettings.LabelStyle.StrokeWidth);
			Assert.Null(dataLabelSettings.LabelStyle.FontFamily);
			Assert.Equal(FontAttributes.None, dataLabelSettings.LabelStyle.FontAttributes);
			Assert.Equal(Colors.Transparent, dataLabelSettings.LabelStyle.TextColor);
			Assert.Equal(string.Empty, dataLabelSettings.LabelStyle.LabelFormat);
		}

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

		[Fact]
		public void ErrorBarSeriesDefaultTests_Part1()
		{
			var series = new ErrorBarSeries();
			Assert.Null(series.HorizontalErrorPath);
			Assert.Null(series.VerticalErrorPath);
			Assert.Equal(0d, series.HorizontalErrorValue);
			Assert.Equal(0d, series.VerticalErrorValue);
			Assert.Equal(ErrorBarMode.Both, series.Mode);
			Assert.Equal(ErrorBarType.Fixed, series.Type);
			Assert.Equal(ErrorBarDirection.Both, series.HorizontalDirection);
			Assert.Equal(ErrorBarDirection.Both, series.VerticalDirection);
		}

		[Fact]
		public void ErrorBarSeriesDefaultTests_Part2()
		{
			var series = new ErrorBarSeries();
			Assert.NotNull(series.HorizontalLineStyle);
			Assert.NotNull(series.VerticalLineStyle);
			Assert.NotNull(series.HorizontalCapLineStyle);
			Assert.NotNull(series.VerticalCapLineStyle);
			Assert.IsType<ErrorBarLineStyle>(series.HorizontalLineStyle);
			Assert.IsType<ErrorBarLineStyle>(series.VerticalLineStyle);
			Assert.IsType<ErrorBarCapLineStyle>(series.HorizontalCapLineStyle);
			Assert.IsType<ErrorBarCapLineStyle>(series.VerticalCapLineStyle);
		}

		[Fact]
		public void ErrorBarSeriesDefaultTests_Part3()
		{
			var series = new ErrorBarSeries();
			Assert.NotNull(series.HorizontalErrorValues);
			Assert.NotNull(series.VerticalErrorValues);
			Assert.Empty(series.HorizontalErrorValues);
			Assert.Empty(series.VerticalErrorValues);
			Assert.Null(series.YBindingPath);
			Assert.NotNull(series.YValues);
			Assert.Empty(series.YValues);
		}

		[Fact]
		public void ErrorBarSeriesDefaultTests_Part5()
		{
			var series = new ErrorBarSeries();
			Assert.Null(series.ActualXAxis);
			Assert.Null(series.ActualYAxis);
			Assert.Equal(string.Empty, series.Label);
			Assert.True(series.ShowTrackballLabel);
			Assert.Null(series.TrackballLabelTemplate);
			Assert.Null(series.XAxisName);
			Assert.Null(series.YAxisName);
		}

		[Fact]
		public void ErrorBarSeriesDefaultTests_Part6()
		{
			var series = new ErrorBarSeries();
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
		public void ErrorBarSeriesDefaultTests_Part7()
		{
			var series = new ErrorBarSeries();
			Assert.False(series.EnableTooltip);
			Assert.Null(series.Fill);
			Assert.Null(series.XBindingPath);
			Assert.Null(series.SelectionBehavior);
			Assert.Equal(1d, series.Opacity);
			Assert.False(series.EnableAnimation);
			Assert.False(series.IsSideBySide);
			Assert.False(series.ListenPropertyChange);
		}

		[Fact]
		public void ErrorBarSeriesDefaultTests_Part8()
		{
			var series = new ErrorBarSeries();
			Assert.Null(series.Chart);
			Assert.Equal(1d, series.AnimationDuration);
			Assert.Equal(1, series.AnimationValue);
			Assert.Null(series.SeriesAnimation);
			Assert.False(series.NeedToAnimateSeries);
			Assert.False(series.NeedToAnimateDataLabel);
			Assert.False(series.SegmentsCreated);
		}

		[Fact]
		public void ErrorBarSeriesDefaultTests_Part9()
		{
			var series = new ErrorBarSeries();
			Assert.Null(series.OldSegments);
			Assert.Equal(0, series.TooltipDataPointIndex);
			Assert.Empty(series.GroupedXValuesIndexes);
			Assert.Empty(series.GroupedActualData);
			Assert.Empty(series.GroupedXValues);
			Assert.NotNull(series.DataLabels);
			Assert.NotNull(series.LabelTemplateView);
		}

		[Fact]
		public void ErrorBarSeriesDefaultTests_Part10()
		{
			var series = new ErrorBarSeries();
			Assert.Equal(0, series.XData);
			Assert.Equal(0, series.PointsCount);
			Assert.Equal(ChartValueType.Double, series.XValueType);
			Assert.Null(series.XValues);
			Assert.Null(series.YComplexPaths);
			Assert.Null(series.ActualXValues);
			Assert.Null(series.SeriesYValues);
			Assert.Null(series.ActualSeriesYValues);
		}

		[Fact]
		public void ErrorBarSeriesDefaultTests_Part11()
		{
			var series = new ErrorBarSeries();
			Assert.Null(series.YPaths);
			Assert.Null(series.ActualData);
			Assert.Null(series.XComplexPaths);
			Assert.True(series.IsLinearData);
			Assert.False(series.IsDataPointAddedDynamically);
			Assert.False(series.IsStacking);
		}

		[Fact]
		public void ErrorBarSeriesDefaultTests_Part12()
		{
			var series = new ErrorBarSeries();
			Assert.True(series.XRange.IsEmpty);
			Assert.True(series.YRange.IsEmpty);
			Assert.False(series.VisibleXRange.IsEmpty);
			Assert.False(series.VisibleYRange.IsEmpty);
			Assert.False(series.PreviousXRange.IsEmpty);
			Assert.False(series.IsMultipleYPathRequired);
			Assert.Equal(1d, series.StrokeWidth);
		}

		[Fact]
		public void ErrorBarSeriesDefaultTests_Part13()
		{
			var series = new ErrorBarSeries();

			// Test that DataLabelSettings is not null and is of the correct type
			Assert.IsType<CartesianDataLabelSettings>(series.DataLabelSettings);
			var dataLabelSettings = series.DataLabelSettings as CartesianDataLabelSettings;
			Assert.NotNull(dataLabelSettings);

			// Test default values of CartesianDataLabelSettings properties
			Assert.Equal(DataLabelAlignment.Top, dataLabelSettings.BarAlignment);

			// Test default values inherited from ChartDataLabelSettings
			Assert.True(dataLabelSettings.UseSeriesPalette);
			Assert.Equal(DataLabelPlacement.Auto, dataLabelSettings.LabelPlacement);

			// Test LabelStyle properties
			Assert.NotNull(dataLabelSettings.LabelStyle);
			Assert.IsType<ChartDataLabelStyle>(dataLabelSettings.LabelStyle);
			Assert.Equal(12, dataLabelSettings.LabelStyle.FontSize);
			Assert.Equal(new Thickness(5), dataLabelSettings.LabelStyle.Margin);

			// Test other properties
			Assert.Equal(Colors.Transparent, dataLabelSettings.LabelStyle.Background);
			Assert.Equal(Brush.Transparent, dataLabelSettings.LabelStyle.Stroke);
			Assert.Equal(0, dataLabelSettings.LabelStyle.StrokeWidth);
			Assert.Null(dataLabelSettings.LabelStyle.FontFamily);
			Assert.Equal(FontAttributes.None, dataLabelSettings.LabelStyle.FontAttributes);
			Assert.Equal(Colors.Transparent, dataLabelSettings.LabelStyle.TextColor);
			Assert.Equal(string.Empty, dataLabelSettings.LabelStyle.LabelFormat);
		}

		[Fact]
		public void ColumnSeriesDefaultTests_Part1()
		{
			var series = new ColumnSeries();
			Assert.True(series.IsSideBySide);
			Assert.Equal(SolidColorBrush.Transparent, series.Stroke);
			Assert.Equal(0d, series.Spacing);
			Assert.Equal(0.8d, series.Width);
		}

		[Fact]
		public void ColumnSeriesDefaultTests_Part2()
		{
			var series = new ColumnSeries();
			Assert.Null(series.YBindingPath);
			Assert.NotNull(series.YValues);
			Assert.Empty(series.YValues);
			Assert.Equal(1d, series.StrokeWidth);
		}

		[Fact]
		public void ColumnSeriesDefaultTests_Part3()
		{
			var series = new ColumnSeries();
			Assert.Null(series.ActualXAxis);
			Assert.Null(series.ActualYAxis);
			Assert.Equal(string.Empty, series.Label);
			Assert.True(series.ShowTrackballLabel);
			Assert.Null(series.TrackballLabelTemplate);
			Assert.Null(series.XAxisName);
			Assert.Null(series.YAxisName);
		}

		[Fact]
		public void ColumnSeriesDefaultTests_Part4()
		{
			var series = new ColumnSeries();
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
		public void ColumnSeriesDefaultTests_Part5()
		{
			var series = new ColumnSeries();
			Assert.False(series.EnableTooltip);
			Assert.Null(series.Fill);
			Assert.Null(series.XBindingPath);
			Assert.Null(series.SelectionBehavior);
			Assert.Equal(1d, series.Opacity);
			Assert.False(series.EnableAnimation);
			Assert.False(series.ListenPropertyChange);
		}

		[Fact]
		public void ColumnSeriesDefaultTests_Part6()
		{
			var series = new ColumnSeries();
			Assert.Null(series.Chart);
			Assert.Equal(1d, series.AnimationDuration);
			Assert.Equal(1, series.AnimationValue);
			Assert.Null(series.SeriesAnimation);
			Assert.False(series.NeedToAnimateSeries);
			Assert.False(series.NeedToAnimateDataLabel);
			Assert.False(series.SegmentsCreated);
		}

		[Fact]
		public void ColumnSeriesDefaultTests_Part7()
		{
			var series = new ColumnSeries();
			Assert.Null(series.OldSegments);
			Assert.Equal(0, series.TooltipDataPointIndex);
			Assert.Empty(series.GroupedXValuesIndexes);
			Assert.Empty(series.GroupedActualData);
			Assert.Empty(series.GroupedXValues);
			Assert.NotNull(series.DataLabels);
			Assert.NotNull(series.LabelTemplateView);
		}

		[Fact]
		public void ColumnSeriesDefaultTests_Part8()
		{
			var series = new ColumnSeries();
			Assert.Equal(0, series.XData);
			Assert.Equal(0, series.PointsCount);
			Assert.Equal(ChartValueType.Double, series.XValueType);
			Assert.Null(series.XValues);
			Assert.Null(series.YComplexPaths);
			Assert.Null(series.ActualXValues);
			Assert.Null(series.SeriesYValues);
			Assert.Null(series.ActualSeriesYValues);
		}

		[Fact]
		public void ColumnSeriesDefaultTests_Part9()
		{
			var series = new ColumnSeries();
			Assert.Null(series.YPaths);
			Assert.Null(series.ActualData);
			Assert.Null(series.XComplexPaths);
			Assert.True(series.IsLinearData);
			Assert.False(series.IsDataPointAddedDynamically);
			Assert.False(series.IsStacking);
		}

		[Fact]
		public void ColumnSeriesDefaultTests_Part10()
		{
			var series = new ColumnSeries();
			Assert.True(series.XRange.IsEmpty);
			Assert.True(series.YRange.IsEmpty);
			Assert.False(series.VisibleXRange.IsEmpty);
			Assert.False(series.VisibleYRange.IsEmpty);
			Assert.False(series.PreviousXRange.IsEmpty);
		}

		[Fact]
		public void ColumnSeriesDefaultTests_Part11()
		{
			var series = new ColumnSeries();
			Assert.False(series.IsMultipleYPathRequired);
			Assert.True(series.IsSideBySide);
			Assert.True(series.SbsInfo.IsEmpty);
			Assert.Equal(0, series.SideBySideIndex);
			Assert.False(series.IsSbsValueCalculated);
			Assert.Null(series.ChartArea);
			Assert.False(series.IsIndexed);
			Assert.False(series.IsGrouped);
		}

		[Fact]
		public void ColumnSeriesDefaultTests_Part12()
		{
			// Create a ColumnSeries instance
			var series = new ColumnSeries();

			// Test that DataLabelSettings is not null and is of the correct type
			Assert.IsType<CartesianDataLabelSettings>(series.DataLabelSettings);
			var dataLabelSettings = series.DataLabelSettings as CartesianDataLabelSettings;
			Assert.NotNull(dataLabelSettings);

			// Test default values of CartesianDataLabelSettings properties
			Assert.Equal(DataLabelAlignment.Top, dataLabelSettings.BarAlignment);

			// Test default values inherited from ChartDataLabelSettings
			Assert.True(dataLabelSettings.UseSeriesPalette);
			Assert.Equal(DataLabelPlacement.Auto, dataLabelSettings.LabelPlacement);

			// Test LabelStyle properties
			Assert.NotNull(dataLabelSettings.LabelStyle);
			Assert.IsType<ChartDataLabelStyle>(dataLabelSettings.LabelStyle);
			Assert.Equal(12, dataLabelSettings.LabelStyle.FontSize);
			Assert.Equal(new Thickness(5), dataLabelSettings.LabelStyle.Margin);

			// Test other properties
			Assert.Equal(Colors.Transparent, dataLabelSettings.LabelStyle.Background);
			Assert.Equal(Brush.Transparent, dataLabelSettings.LabelStyle.Stroke);
			Assert.Equal(0, dataLabelSettings.LabelStyle.StrokeWidth);
			Assert.Null(dataLabelSettings.LabelStyle.FontFamily);
			Assert.Equal(FontAttributes.None, dataLabelSettings.LabelStyle.FontAttributes);
			Assert.Equal(Colors.Transparent, dataLabelSettings.LabelStyle.TextColor);
			Assert.Equal(string.Empty, dataLabelSettings.LabelStyle.LabelFormat);
		}

		[Fact]
		public void DoughnutSeriesDefaultTests_Part1()
		{
			var series = new DoughnutSeries();
			Assert.Equal(0.4d, series.InnerRadius);
			Assert.Null(series.CenterView);
			Assert.Equal(1d, series.CenterHoleSize);
			Assert.NotNull(series.PaletteBrushes);
			Assert.Equal(ChartColorModel.DefaultBrushes, series.PaletteBrushes);
		}

		[Fact]
		public void DoughnutSeriesDefaultTests_Part2()
		{
			var series = new DoughnutSeries();
			Assert.Equal(-1, series.ExplodeIndex);
			Assert.Equal(10d, series.ExplodeRadius);
			Assert.False(series.ExplodeOnTouch);
			Assert.Equal(double.NaN, series.GroupTo);
			Assert.Equal(PieGroupMode.Value, series.GroupMode);
			Assert.False(series.ExplodeAll);
		}

		[Fact]
		public void DoughnutSeriesDefaultTests_Part3()
		{
			var series = new DoughnutSeries();
			Assert.NotNull(series.ActualYValues);
			Assert.Empty(series.ActualYValues);
			Assert.NotNull(series.GroupToDataPoints);
			Assert.Empty(series.GroupToDataPoints);
		}

		[Fact]
		public void DoughnutSeriesDefaultTests_Part4()
		{
			var series = new DoughnutSeries();
			Assert.Null(series.YBindingPath);
			Assert.NotNull(series.YValues);
			Assert.Empty(series.YValues);
			Assert.Equal(SolidColorBrush.Transparent, series.Stroke);
			Assert.Equal(2d, series.StrokeWidth);
			Assert.Equal(0.8d, series.Radius);
			Assert.Equal(0d, series.StartAngle);
			Assert.Equal(360d, series.EndAngle);
		}

		[Fact]
		public void DoughnutSeriesDefaultTests_Part5()
		{
			var series = new DoughnutSeries();
			Assert.True(series.IsVisible);
			Assert.True(series.IsVisibleOnLegend);
			Assert.Null(series.ItemsSource);
			Assert.Equal(LabelContext.YValue, series.LabelContext);
			Assert.Null(series.LabelTemplate);
			Assert.Equal(ChartLegendIconType.Circle, series.LegendIcon);
			Assert.Null(series.Fill);
			Assert.False(series.ShowDataLabels);
			Assert.Null(series.TooltipTemplate);
		}

		[Fact]
		public void DoughnutSeriesDefaultTests_Part6()
		{
			var series = new DoughnutSeries();
			Assert.False(series.EnableTooltip);
			Assert.Null(series.XBindingPath);
			Assert.Null(series.SelectionBehavior);
			Assert.Equal(1d, series.Opacity);
			Assert.False(series.EnableAnimation);
			Assert.False(series.ListenPropertyChange);
		}

		[Fact]
		public void DoughnutSeriesDefaultTests_Part7()
		{
			var series = new DoughnutSeries();
			Assert.Null(series.Chart);
			Assert.Equal(1d, series.AnimationDuration);
			Assert.Equal(1, series.AnimationValue);
			Assert.Null(series.SeriesAnimation);
			Assert.False(series.NeedToAnimateSeries);
			Assert.False(series.NeedToAnimateDataLabel);
			Assert.False(series.SegmentsCreated);
		}

		[Fact]
		public void DoughnutSeriesDefaultTests_Part8()
		{
			var series = new DoughnutSeries();
			Assert.Null(series.OldSegments);
			Assert.Equal(0, series.TooltipDataPointIndex);
			Assert.Empty(series.GroupedXValuesIndexes);
			Assert.Empty(series.GroupedActualData);
			Assert.Empty(series.GroupedXValues);
			Assert.NotNull(series.DataLabels);
			Assert.NotNull(series.LabelTemplateView);
		}

		[Fact]
		public void DoughnutSeriesDefaultTests_Part9()
		{
			var series = new DoughnutSeries();
			Assert.Equal(0, series.XData);
			Assert.Equal(0, series.PointsCount);
			Assert.Equal(ChartValueType.Double, series.XValueType);
			Assert.Null(series.XValues);
			Assert.Null(series.YComplexPaths);
			Assert.Null(series.ActualXValues);
			Assert.Null(series.SeriesYValues);
			Assert.Null(series.ActualSeriesYValues);
		}

		[Fact]
		public void DoughnutSeriesDefaultTests_Part10()
		{
			var series = new DoughnutSeries();
			Assert.Null(series.YPaths);
			Assert.Null(series.ActualData);
			Assert.Null(series.XComplexPaths);
			Assert.True(series.IsLinearData);
			Assert.False(series.IsDataPointAddedDynamically);
			Assert.False(series.IsStacking);
		}

		[Fact]
		public void DoughnutSeriesDefaultTests_Part11()
		{
			var series = new DoughnutSeries();
			Assert.True(series.XRange.IsEmpty);
			Assert.True(series.YRange.IsEmpty);
			Assert.False(series.VisibleXRange.IsEmpty);
			Assert.False(series.VisibleYRange.IsEmpty);
			Assert.False(series.PreviousXRange.IsEmpty);
		}

		[Fact]
		public void DoughnutSeriesDefaultTests_Part12()
		{
			var series = new DoughnutSeries();
			Assert.Null(series.ChartArea);
			Assert.Equal(PointF.Zero, series.Center);
			Assert.Equal(0f, series.ActualRadius);
			Assert.NotNull(series.InnerBounds);
			Assert.Empty(series.InnerBounds);
			Assert.Null(series.LeftPoints);
			Assert.Null(series.RightPoints);
			Assert.Equal(Rect.Zero, series.AdjacentLabelRect);
		}

		[Fact]
		public void DoughnutSeriesDefaultTests_Part13()
		{
			var series = new DoughnutSeries();
			Assert.IsType<CircularDataLabelSettings>(series.DataLabelSettings);
			var dataLabelSettings = series.DataLabelSettings as CircularDataLabelSettings;
			Assert.NotNull(dataLabelSettings);
			// Test default values of CircularDataLabelSettings properties
			Assert.Equal(ChartDataLabelPosition.Inside, dataLabelSettings.LabelPosition);
			Assert.Equal(SmartLabelAlignment.Shift, dataLabelSettings.SmartLabelAlignment);
			Assert.NotNull(dataLabelSettings.ConnectorLineSettings);
			Assert.IsType<ConnectorLineStyle>(dataLabelSettings.ConnectorLineSettings);

			// Test default values inherited from ChartDataLabelSettings
			Assert.True(dataLabelSettings.UseSeriesPalette);
			Assert.Equal(DataLabelPlacement.Auto, dataLabelSettings.LabelPlacement);

			// Test LabelStyle properties
			Assert.NotNull(dataLabelSettings.LabelStyle);
			Assert.IsType<ChartDataLabelStyle>(dataLabelSettings.LabelStyle);
			Assert.Equal(14, dataLabelSettings.LabelStyle.FontSize);
			Assert.Equal(new Thickness(8, 6), dataLabelSettings.LabelStyle.Margin);

			Assert.Equal(Colors.Transparent, dataLabelSettings.LabelStyle.Background);
			Assert.Equal(Brush.Transparent, dataLabelSettings.LabelStyle.Stroke);
			Assert.Equal(0, dataLabelSettings.LabelStyle.StrokeWidth);
			Assert.Null(dataLabelSettings.LabelStyle.FontFamily);
			Assert.Equal(FontAttributes.None, dataLabelSettings.LabelStyle.FontAttributes);
			Assert.Equal(Colors.Transparent, dataLabelSettings.LabelStyle.TextColor);
			Assert.Equal(string.Empty, dataLabelSettings.LabelStyle.LabelFormat);

			// Test default values of internal properties
			Assert.Equal(OverflowMode.None, dataLabelSettings.OverflowMode);
		}

		[Fact]
		public void PieSeriesDefaultTests_Part1()
		{
			var series = new PieSeries();
			Assert.Equal(-1, series.ExplodeIndex);
			Assert.Equal(10d, series.ExplodeRadius);
			Assert.False(series.ExplodeOnTouch);
			Assert.Equal(double.NaN, series.GroupTo);
			Assert.Equal(PieGroupMode.Value, series.GroupMode);
			Assert.False(series.ExplodeAll);
		}

		[Fact]
		public void PieSeriesDefaultTests_Part2()
		{
			var series = new PieSeries();
			Assert.NotNull(series.PaletteBrushes);
			Assert.Equal(ChartColorModel.DefaultBrushes, series.PaletteBrushes);
			Assert.Equal(SolidColorBrush.Transparent, series.Stroke);
			Assert.Equal(2d, series.StrokeWidth);
			Assert.NotNull(series.ActualYValues);
			Assert.Empty(series.ActualYValues);
			Assert.NotNull(series.GroupToDataPoints);
			Assert.Empty(series.GroupToDataPoints);
		}

		[Fact]
		public void PieSeriesDefaultTests_Part3()
		{
			var series = new PieSeries();
			Assert.Equal(0.8d, series.Radius);
			Assert.Equal(0d, series.StartAngle);
			Assert.Equal(360d, series.EndAngle);
			Assert.True(series.IsVisible);
			Assert.True(series.IsVisibleOnLegend);
			Assert.Null(series.ItemsSource);
			Assert.Equal(LabelContext.YValue, series.LabelContext);
			Assert.Null(series.LabelTemplate);
			Assert.Equal(ChartLegendIconType.Circle, series.LegendIcon);
		}

		[Fact]
		public void PieSeriesDefaultTests_Part4()
		{
			var series = new PieSeries();
			Assert.Null(series.Fill);
			Assert.False(series.ShowDataLabels);
			Assert.Null(series.TooltipTemplate);
			Assert.False(series.EnableTooltip);
			Assert.Null(series.XBindingPath);
			Assert.Null(series.YBindingPath);
		}

		[Fact]
		public void PieSeriesDefaultTests_Part5()
		{
			var series = new PieSeries();
			Assert.Null(series.SelectionBehavior);
			Assert.Equal(1d, series.Opacity);
			Assert.False(series.EnableAnimation);
			Assert.Null(series.Chart);
			Assert.Equal(1d, series.AnimationDuration);
			Assert.Equal(1, series.AnimationValue);
			Assert.False(series.ListenPropertyChange);
		}

		[Fact]
		public void PieSeriesDefaultTests_Part6()
		{
			var series = new PieSeries();
			Assert.Null(series.SeriesAnimation);
			Assert.False(series.NeedToAnimateSeries);
			Assert.False(series.NeedToAnimateDataLabel);
			Assert.False(series.SegmentsCreated);
			Assert.Null(series.OldSegments);
			Assert.Equal(0, series.TooltipDataPointIndex);
		}

		[Fact]
		public void PieSeriesDefaultTests_Part8()
		{
			var series = new PieSeries();
			Assert.Empty(series.GroupedXValuesIndexes);
			Assert.Empty(series.GroupedActualData);
			Assert.Empty(series.GroupedXValues);
			Assert.NotNull(series.DataLabels);
			Assert.NotNull(series.LabelTemplateView);
			Assert.Equal(0, series.XData);
		}

		[Fact]
		public void PieSeriesDefaultTests_Part9()
		{
			var series = new PieSeries();
			Assert.Equal(0, series.PointsCount);
			Assert.Equal(ChartValueType.Double, series.XValueType);
			Assert.Null(series.XValues);
			Assert.Null(series.YComplexPaths);
			Assert.Null(series.ActualXValues);
			Assert.Null(series.SeriesYValues);
			Assert.Null(series.ActualSeriesYValues);
		}

		[Fact]
		public void PieSeriesDefaultTests_Part10()
		{
			var series = new PieSeries();
			Assert.Null(series.YPaths);
			Assert.Null(series.ActualData);
			Assert.Null(series.XComplexPaths);
			Assert.True(series.IsLinearData);
			Assert.False(series.IsDataPointAddedDynamically);
			Assert.False(series.IsStacking);
		}

		[Fact]
		public void PieSeriesDefaultTests_Part11()
		{
			var series = new PieSeries();
			Assert.Null(series.ChartArea);
			Assert.Equal(PointF.Zero, series.Center);
			Assert.Equal(0f, series.ActualRadius);
			Assert.NotNull(series.InnerBounds);
			Assert.Empty(series.InnerBounds);
			Assert.Null(series.LeftPoints);
			Assert.Null(series.RightPoints);
			Assert.Equal(Rect.Zero, series.AdjacentLabelRect);
		}

		[Fact]
		public void PieSeriesDefaultTests_Part12()
		{
			var series = new PieSeries();
			Assert.NotNull(series.YValues);
			Assert.Empty(series.YValues);
			Assert.False(series.IsMultipleYPathRequired);
			Assert.False(series.IsSideBySide);
			Assert.True(series.IsVisible);
			Assert.True(series.IsVisibleOnLegend);
			Assert.Equal(1d, series.Opacity);
		}

		[Fact]
		public void PieSeriesDefaultTests_Part13()
		{
			var series = new PieSeries();
			Assert.True(series.XRange.IsEmpty);
			Assert.True(series.YRange.IsEmpty);
			Assert.False(series.VisibleXRange.IsEmpty);
			Assert.False(series.VisibleYRange.IsEmpty);
			Assert.False(series.PreviousXRange.IsEmpty);
		}

		[Fact]
		public void PieSeriesDefaultTests_Part14()
		{
			var series = new PieSeries();
			Assert.IsType<CircularDataLabelSettings>(series.DataLabelSettings);
			var dataLabelSettings = series.DataLabelSettings as CircularDataLabelSettings;
			Assert.NotNull(dataLabelSettings);
			// Test default values of CircularDataLabelSettings properties
			Assert.Equal(ChartDataLabelPosition.Inside, dataLabelSettings.LabelPosition);
			Assert.Equal(SmartLabelAlignment.Shift, dataLabelSettings.SmartLabelAlignment);
			Assert.NotNull(dataLabelSettings.ConnectorLineSettings);
			Assert.IsType<ConnectorLineStyle>(dataLabelSettings.ConnectorLineSettings);

			// Test default values inherited from ChartDataLabelSettings
			Assert.True(dataLabelSettings.UseSeriesPalette);
			Assert.Equal(DataLabelPlacement.Auto, dataLabelSettings.LabelPlacement);

			// Test LabelStyle properties
			Assert.NotNull(dataLabelSettings.LabelStyle);
			Assert.IsType<ChartDataLabelStyle>(dataLabelSettings.LabelStyle);
			Assert.Equal(14, dataLabelSettings.LabelStyle.FontSize);
			Assert.Equal(new Thickness(8, 6), dataLabelSettings.LabelStyle.Margin);

			Assert.Equal(Colors.Transparent, dataLabelSettings.LabelStyle.Background);
			Assert.Equal(Brush.Transparent, dataLabelSettings.LabelStyle.Stroke);
			Assert.Equal(0, dataLabelSettings.LabelStyle.StrokeWidth);
			Assert.Null(dataLabelSettings.LabelStyle.FontFamily);
			Assert.Equal(FontAttributes.None, dataLabelSettings.LabelStyle.FontAttributes);
			Assert.Equal(Colors.Transparent, dataLabelSettings.LabelStyle.TextColor);
			Assert.Equal(string.Empty, dataLabelSettings.LabelStyle.LabelFormat);

			// Test default values of internal properties
			Assert.Equal(OverflowMode.None, dataLabelSettings.OverflowMode);
		}

		[Fact]
		public void RadialBarSeriesDefaultTests_Part1()
		{
			var series = new RadialBarSeries();
			// Test TrackFill
			Assert.NotNull(series.TrackFill);
			Assert.IsType<SolidColorBrush>(series.TrackFill);
			var trackFillBrush = (SolidColorBrush)series.TrackFill;
			Assert.Equal(Color.FromRgba(0, 0, 0, 0.08), trackFillBrush.Color);

			// Test TrackStroke
			Assert.NotNull(series.TrackStroke);
			Assert.IsType<SolidColorBrush>(series.TrackStroke);
			var trackStrokeBrush = (SolidColorBrush)series.TrackStroke;
			Assert.Equal(Color.FromRgba(0, 0, 0, 0.24), trackStrokeBrush.Color);

			Assert.Equal(0d, series.TrackStrokeWidth);
			Assert.Equal(double.NaN, series.MaximumValue);
			Assert.Equal(0.2d, series.GapRatio);
			Assert.Null(series.CenterView);
			Assert.Equal(CapStyle.BothFlat, series.CapStyle);
			Assert.Equal(0.4d, series.InnerRadius);
			Assert.Equal(1d, series.CenterHoleSize);
		}

		[Fact]
		public void RadialBarSeriesDefaultTests_Part2()
		{
			var series = new RadialBarSeries();
			Assert.NotNull(series.PaletteBrushes);
			Assert.Equal(ChartColorModel.DefaultBrushes, series.PaletteBrushes);
			Assert.Equal(-90d, series.StartAngle);
			Assert.Equal(270d, series.EndAngle);
			Assert.NotNull(series.YValues);
			Assert.Empty(series.YValues);
			Assert.Equal(PointF.Zero, series.Center);
			Assert.Equal(0f, series.ActualRadius);
		}

		[Fact]
		public void RadialBarSeriesDefaultTests_Part3()
		{
			var series = new RadialBarSeries();
			Assert.NotNull(series.InnerBounds);
			Assert.Empty(series.InnerBounds);
			Assert.Null(series.LeftPoints);
			Assert.Null(series.RightPoints);
			Assert.Equal(Rect.Zero, series.AdjacentLabelRect);
		}

		[Fact]
		public void RadialBarSeriesDefaultTests_Part4()
		{
			var series = new RadialBarSeries();
			Assert.True(series.IsVisible);
			Assert.True(series.IsVisibleOnLegend);
			Assert.Null(series.ItemsSource);
			Assert.Equal(LabelContext.YValue, series.LabelContext);
			Assert.Null(series.LabelTemplate);
			Assert.Equal(ChartLegendIconType.Circle, series.LegendIcon);
		}

		[Fact]
		public void RadialBarSeriesDefaultTests_Part5()
		{
			var series = new RadialBarSeries();
			Assert.Null(series.Fill);
			Assert.False(series.ShowDataLabels);
			Assert.Null(series.TooltipTemplate);
			Assert.False(series.EnableTooltip);
			Assert.Null(series.XBindingPath);
			Assert.Null(series.YBindingPath);
			Assert.False(series.ListenPropertyChange);
		}

		[Fact]
		public void RadialBarSeriesDefaultTests_Part6()
		{
			var series = new RadialBarSeries();
			Assert.Null(series.SelectionBehavior);
			Assert.Equal(1d, series.Opacity);
			Assert.False(series.EnableAnimation);
			Assert.Null(series.Chart);
			Assert.Equal(1d, series.AnimationDuration);
			Assert.Equal(1, series.AnimationValue);
		}

		[Fact]
		public void RadialBarSeriesDefaultTests_Part7()
		{
			var series = new RadialBarSeries();
			Assert.Null(series.SeriesAnimation);
			Assert.False(series.NeedToAnimateSeries);
			Assert.False(series.NeedToAnimateDataLabel);
			Assert.False(series.SegmentsCreated);
			Assert.Null(series.OldSegments);
			Assert.Equal(0, series.TooltipDataPointIndex);
		}

		[Fact]
		public void RadialBarSeriesDefaultTests_Part8()
		{
			var series = new RadialBarSeries();
			Assert.Empty(series.GroupedXValuesIndexes);
			Assert.Empty(series.GroupedActualData);
			Assert.Empty(series.GroupedXValues);
			Assert.NotNull(series.DataLabels);
			Assert.NotNull(series.LabelTemplateView);
			Assert.Equal(0, series.XData);
		}

		[Fact]
		public void RadialBarSeriesDefaultTests_Part9()
		{
			var series = new RadialBarSeries();
			Assert.Equal(0, series.PointsCount);
			Assert.Equal(ChartValueType.Double, series.XValueType);
			Assert.Null(series.XValues);
			Assert.Null(series.YComplexPaths);
			Assert.Null(series.ActualXValues);
			Assert.Null(series.SeriesYValues);
			Assert.Null(series.ActualSeriesYValues);
		}

		[Fact]
		public void RadialBarSeriesDefaultTests_Part10()
		{
			var series = new RadialBarSeries();
			Assert.Null(series.YPaths);
			Assert.Null(series.ActualData);
			Assert.Null(series.XComplexPaths);
			Assert.True(series.IsLinearData);
			Assert.False(series.IsDataPointAddedDynamically);
			Assert.False(series.IsStacking);
		}

		[Fact]
		public void RadialBarSeriesDefaultTests_Part11()
		{
			var series = new RadialBarSeries();
			Assert.True(series.XRange.IsEmpty);
			Assert.True(series.YRange.IsEmpty);
			Assert.False(series.VisibleXRange.IsEmpty);
			Assert.False(series.VisibleYRange.IsEmpty);
			Assert.False(series.PreviousXRange.IsEmpty);
		}

		[Fact]
		public void RadialBarSeriesDefaultTests_Part12()
		{
			var series = new RadialBarSeries();
			Assert.Null(series.ChartArea);
			Assert.False(series.IsMultipleYPathRequired);
			Assert.False(series.IsSideBySide);
			Assert.Equal(SolidColorBrush.Transparent, series.Stroke);
			Assert.Equal(2d, series.StrokeWidth);
			Assert.Equal(0.8d, series.Radius);
		}

		[Fact]
		public void RadialBarSeriesDefaultTests_Part13()
		{
			var series = new RadialBarSeries();
			Assert.IsType<CircularDataLabelSettings>(series.DataLabelSettings);
			var dataLabelSettings = series.DataLabelSettings as CircularDataLabelSettings;
			Assert.NotNull(dataLabelSettings);
			// Test default values of CircularDataLabelSettings properties
			Assert.Equal(ChartDataLabelPosition.Inside, dataLabelSettings.LabelPosition);
			Assert.Equal(SmartLabelAlignment.Shift, dataLabelSettings.SmartLabelAlignment);
			Assert.NotNull(dataLabelSettings.ConnectorLineSettings);
			Assert.IsType<ConnectorLineStyle>(dataLabelSettings.ConnectorLineSettings);

			// Test default values inherited from ChartDataLabelSettings
			Assert.True(dataLabelSettings.UseSeriesPalette);
			Assert.Equal(DataLabelPlacement.Auto, dataLabelSettings.LabelPlacement);

			// Test LabelStyle properties
			Assert.NotNull(dataLabelSettings.LabelStyle);
			Assert.IsType<ChartDataLabelStyle>(dataLabelSettings.LabelStyle);
			Assert.Equal(14, dataLabelSettings.LabelStyle.FontSize);
			Assert.Equal(new Thickness(8, 6), dataLabelSettings.LabelStyle.Margin);

			Assert.Equal(Colors.Transparent, dataLabelSettings.LabelStyle.Background);
			Assert.Equal(Brush.Transparent, dataLabelSettings.LabelStyle.Stroke);
			Assert.Equal(0, dataLabelSettings.LabelStyle.StrokeWidth);
			Assert.Null(dataLabelSettings.LabelStyle.FontFamily);
			Assert.Equal(FontAttributes.None, dataLabelSettings.LabelStyle.FontAttributes);
			Assert.Equal(Colors.Transparent, dataLabelSettings.LabelStyle.TextColor);
			Assert.Equal(string.Empty, dataLabelSettings.LabelStyle.LabelFormat);

			// Test default values of internal properties
			Assert.Equal(OverflowMode.None, dataLabelSettings.OverflowMode);
		}

		[Fact]
		public void PolarAreaSeriesDefaultTests_Part1()
		{
			var series = new PolarAreaSeries();
			Assert.Null(series.Stroke);
			Assert.True(series.IsClosed);
			Assert.Equal(2d, series.StrokeWidth);
			Assert.Null(series.StrokeDashArray);
		}

		[Fact]
		public void PolarAreaSeriesDefaultTests_Part2()
		{
			var series = new PolarAreaSeries();
			Assert.Null(series.YBindingPath);
			Assert.NotNull(series.YValues);
			Assert.Empty(series.YValues);
			Assert.Null(series.ActualXAxis);
			Assert.Null(series.ActualYAxis);
			Assert.Equal(string.Empty, series.Label);
			Assert.Null(series.ChartArea);
		}

		[Fact]
		public void PolarAreaSeriesDefaultTests_Part3()
		{
			var series = new PolarAreaSeries();
			Assert.True(series.IsVisible);
			Assert.True(series.IsVisibleOnLegend);
			Assert.Null(series.ItemsSource);
			Assert.Equal(LabelContext.YValue, series.LabelContext);
			Assert.Null(series.LabelTemplate);
			Assert.Equal(ChartLegendIconType.Circle, series.LegendIcon);
			Assert.False(series.ShowMarkers);
			Assert.NotNull(series.MarkerSettings);
			Assert.IsType<ChartMarkerSettings>(series.MarkerSettings);
		}

		[Fact]
		public void PolarAreaSeriesDefaultTests_Part4()
		{
			var series = new PolarAreaSeries();
			Assert.Null(series.PaletteBrushes);
			Assert.False(series.ShowDataLabels);
			Assert.Null(series.TooltipTemplate);
			Assert.False(series.EnableTooltip);
			Assert.Null(series.Fill);
			Assert.Null(series.XBindingPath);
			Assert.False(series.ListenPropertyChange);
		}

		[Fact]
		public void PolarAreaSeriesDefaultTests_Part5()
		{
			var series = new PolarAreaSeries();
			Assert.Null(series.SelectionBehavior);
			Assert.Equal(1d, series.Opacity);
			Assert.False(series.EnableAnimation);
			Assert.Null(series.Chart);
			Assert.Equal(1d, series.AnimationDuration);
			Assert.Equal(1, series.AnimationValue);
		}

		[Fact]
		public void PolarAreaSeriesDefaultTests_Part6()
		{
			var series = new PolarAreaSeries();
			Assert.Null(series.SeriesAnimation);
			Assert.False(series.NeedToAnimateSeries);
			Assert.False(series.NeedToAnimateDataLabel);
			Assert.False(series.SegmentsCreated);
			Assert.Null(series.OldSegments);
			Assert.Equal(0, series.TooltipDataPointIndex);
		}

		[Fact]
		public void PolarAreaSeriesDefaultTests_Part7()
		{
			var series = new PolarAreaSeries();
			Assert.Empty(series.GroupedXValuesIndexes);
			Assert.Empty(series.GroupedActualData);
			Assert.Empty(series.GroupedXValues);
			Assert.NotNull(series.DataLabels);
			Assert.NotNull(series.LabelTemplateView);
			Assert.Equal(0, series.XData);
		}

		[Fact]
		public void PolarAreaSeriesDefaultTests_Part8()
		{
			var series = new PolarAreaSeries();
			Assert.Equal(0, series.PointsCount);
			Assert.Equal(ChartValueType.Double, series.XValueType);
			Assert.Null(series.XValues);
			Assert.Null(series.YComplexPaths);
			Assert.Null(series.ActualXValues);
			Assert.Null(series.SeriesYValues);
			Assert.Null(series.ActualSeriesYValues);
		}

		[Fact]
		public void PolarAreaSeriesDefaultTests_Part9()
		{
			var series = new PolarAreaSeries();
			Assert.Null(series.YPaths);
			Assert.Null(series.ActualData);
			Assert.Null(series.XComplexPaths);
			Assert.True(series.IsLinearData);
			Assert.False(series.IsDataPointAddedDynamically);
			Assert.False(series.IsStacking);
			Assert.False(series.IsMultipleYPathRequired);
			Assert.False(series.IsSideBySide);
		}

		[Fact]
		public void PolarAreaSeriesDefaultTests_Part10()
		{
			var series = new PolarAreaSeries();
			Assert.True(series.XRange.IsEmpty);
			Assert.True(series.YRange.IsEmpty);
			Assert.False(series.VisibleXRange.IsEmpty);
			Assert.False(series.VisibleYRange.IsEmpty);
			Assert.False(series.PreviousXRange.IsEmpty);
		}

		[Fact]
		public void PolarAreaSeriesDefaultTests_Part11()
		{
			// Create a PolarAreaSeries instance
			var series = new PolarAreaSeries();

			// Test that DataLabelSettings is not null and is of the correct type
			Assert.IsType<PolarDataLabelSettings>(series.DataLabelSettings);
			var dataLabelSettings = series.DataLabelSettings as PolarDataLabelSettings;
			Assert.NotNull(dataLabelSettings);

			// Test default values inherited from ChartDataLabelSettings
			Assert.True(dataLabelSettings.UseSeriesPalette);
			Assert.Equal(DataLabelPlacement.Auto, dataLabelSettings.LabelPlacement);

			// Test LabelStyle properties
			Assert.NotNull(dataLabelSettings.LabelStyle);
			Assert.IsType<ChartDataLabelStyle>(dataLabelSettings.LabelStyle);
			Assert.Equal(12, dataLabelSettings.LabelStyle.FontSize);
			Assert.Equal(new Thickness(5), dataLabelSettings.LabelStyle.Margin);

			// Test other properties
			Assert.Equal(Colors.Transparent, dataLabelSettings.LabelStyle.Background);
			Assert.Equal(Brush.Transparent, dataLabelSettings.LabelStyle.Stroke);
			Assert.Equal(0, dataLabelSettings.LabelStyle.StrokeWidth);
			Assert.Null(dataLabelSettings.LabelStyle.FontFamily);
			Assert.Equal(FontAttributes.None, dataLabelSettings.LabelStyle.FontAttributes);

			// Default TextColor should be Color.FromRgba(170, 170, 170, 255)
			Assert.Equal(Colors.Transparent, dataLabelSettings.LabelStyle.TextColor);

			Assert.Equal(string.Empty, dataLabelSettings.LabelStyle.LabelFormat);

			// Test the GetContrastTextColor method
			var contrastColor = dataLabelSettings.GetContrastTextColor(series, null, null);
			Assert.Equal(Colors.Black, contrastColor);
		}

		[Fact]
		public void PolarLineSeriesDefaultTests_Part1()
		{
			var series = new PolarLineSeries();
			Assert.False(series.ShowMarkers);
			Assert.NotNull(series.MarkerSettings);
			Assert.IsType<ChartMarkerSettings>(series.MarkerSettings);
			Assert.Null(series.YBindingPath);
			Assert.NotNull(series.YValues);
			Assert.Empty(series.YValues);
		}

		[Fact]
		public void PolarLineSeriesDefaultTests_Part2()
		{
			var series = new PolarLineSeries();
			Assert.True(series.IsClosed);
			Assert.Equal(2d, series.StrokeWidth);
			Assert.Null(series.StrokeDashArray);
			Assert.Null(series.ActualXAxis);
			Assert.Null(series.ActualYAxis);
			Assert.Equal(string.Empty, series.Label);
			Assert.Null(series.ChartArea);
		}

		[Fact]
		public void PolarLineSeriesDefaultTests_Part3()
		{
			var series = new PolarLineSeries();
			Assert.True(series.IsVisible);
			Assert.True(series.IsVisibleOnLegend);
			Assert.Null(series.ItemsSource);
			Assert.Equal(LabelContext.YValue, series.LabelContext);
			Assert.Null(series.LabelTemplate);
			Assert.Equal(ChartLegendIconType.Circle, series.LegendIcon);
		}

		[Fact]
		public void PolarLineSeriesDefaultTests_Part4()
		{
			var series = new PolarLineSeries();
			Assert.Null(series.PaletteBrushes);
			Assert.False(series.ShowDataLabels);
			Assert.Null(series.TooltipTemplate);
			Assert.False(series.EnableTooltip);
			Assert.Null(series.Fill);
			Assert.Null(series.XBindingPath);
			Assert.False(series.ListenPropertyChange);
		}

		[Fact]
		public void PolarLineSeriesDefaultTests_Part5()
		{
			var series = new PolarLineSeries();
			Assert.Null(series.SelectionBehavior);
			Assert.Equal(1d, series.Opacity);
			Assert.False(series.EnableAnimation);
			Assert.Null(series.Chart);
			Assert.Equal(1d, series.AnimationDuration);
			Assert.Equal(1, series.AnimationValue);
		}

		[Fact]
		public void PolarLineSeriesDefaultTests_Part6()
		{
			var series = new PolarLineSeries();
			Assert.Null(series.SeriesAnimation);
			Assert.False(series.NeedToAnimateSeries);
			Assert.False(series.NeedToAnimateDataLabel);
			Assert.False(series.SegmentsCreated);
			Assert.Null(series.OldSegments);
			Assert.Equal(0, series.TooltipDataPointIndex);
		}

		[Fact]
		public void PolarLineSeriesDefaultTests_Part7()
		{
			var series = new PolarLineSeries();
			Assert.Empty(series.GroupedXValuesIndexes);
			Assert.Empty(series.GroupedActualData);
			Assert.Empty(series.GroupedXValues);
			Assert.NotNull(series.DataLabels);
			Assert.NotNull(series.LabelTemplateView);
			Assert.Equal(0, series.XData);
		}

		[Fact]
		public void PolarLineSeriesDefaultTests_Part8()
		{
			var series = new PolarLineSeries();
			Assert.Equal(0, series.PointsCount);
			Assert.Equal(ChartValueType.Double, series.XValueType);
			Assert.Null(series.XValues);
			Assert.Null(series.YComplexPaths);
			Assert.Null(series.ActualXValues);
			Assert.Null(series.SeriesYValues);
			Assert.Null(series.ActualSeriesYValues);
		}

		[Fact]
		public void PolarLineSeriesDefaultTests_Part9()
		{
			var series = new PolarLineSeries();
			Assert.False(series.IsMultipleYPathRequired);
			Assert.False(series.IsSideBySide);
			Assert.Null(series.ChartArea);
			Assert.Equal(ChartLegendIconType.Circle, series.LegendIcon);
			Assert.Null(series.Fill);
			Assert.Equal(2d, series.StrokeWidth);
			Assert.Null(series.StrokeDashArray);
		}

		[Fact]
		public void PolarLineSeriesDefaultTests_Part10()
		{
			var series = new PolarLineSeries();
			Assert.Null(series.YPaths);
			Assert.Null(series.ActualData);
			Assert.Null(series.XComplexPaths);
			Assert.True(series.IsLinearData);
			Assert.False(series.IsDataPointAddedDynamically);
			Assert.False(series.IsStacking);
		}

		[Fact]
		public void PolarLineSeriesDefaultTests_Part11()
		{
			var series = new PolarLineSeries();
			Assert.True(series.XRange.IsEmpty);
			Assert.True(series.YRange.IsEmpty);
			Assert.False(series.VisibleXRange.IsEmpty);
			Assert.False(series.VisibleYRange.IsEmpty);
			Assert.False(series.PreviousXRange.IsEmpty);
		}

		[Fact]
		public void PolarLineSeriesDefaultTests_Part12()
		{
			// Create a PolarAreaSeries instance
			var series = new PolarLineSeries();

			// Test that DataLabelSettings is not null and is of the correct type
			Assert.IsType<PolarDataLabelSettings>(series.DataLabelSettings);
			var dataLabelSettings = series.DataLabelSettings as PolarDataLabelSettings;
			Assert.NotNull(dataLabelSettings);

			// Test default values inherited from ChartDataLabelSettings
			Assert.True(dataLabelSettings.UseSeriesPalette);
			Assert.Equal(DataLabelPlacement.Auto, dataLabelSettings.LabelPlacement);

			// Test LabelStyle properties
			Assert.NotNull(dataLabelSettings.LabelStyle);
			Assert.IsType<ChartDataLabelStyle>(dataLabelSettings.LabelStyle);
			Assert.Equal(12, dataLabelSettings.LabelStyle.FontSize);
			Assert.Equal(new Thickness(5), dataLabelSettings.LabelStyle.Margin);

			// Test other properties
			Assert.Equal(Colors.Transparent, dataLabelSettings.LabelStyle.Background);
			Assert.Equal(Brush.Transparent, dataLabelSettings.LabelStyle.Stroke);
			Assert.Equal(0, dataLabelSettings.LabelStyle.StrokeWidth);
			Assert.Null(dataLabelSettings.LabelStyle.FontFamily);
			Assert.Equal(FontAttributes.None, dataLabelSettings.LabelStyle.FontAttributes);

			// Default TextColor should be Color.FromRgba(170, 170, 170, 255)
			Assert.Equal(Colors.Transparent, dataLabelSettings.LabelStyle.TextColor);

			Assert.Equal(string.Empty, dataLabelSettings.LabelStyle.LabelFormat);

			// Test the GetContrastTextColor method
			var contrastColor = dataLabelSettings.GetContrastTextColor(series, null, null);
			Assert.Equal(Colors.Black, contrastColor);
		}
	}
}
