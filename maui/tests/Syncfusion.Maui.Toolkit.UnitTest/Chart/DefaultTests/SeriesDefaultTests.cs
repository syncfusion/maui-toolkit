using System.Collections.ObjectModel;
using Syncfusion.Maui.Toolkit.Charts;
using Chart = Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.Toolkit.UnitTest;

public partial class SeriesDefaultTests
{
	[Fact]
	public void AreaSeries_InitializesBasicPropertiesCorrectly()
	{
		var series = new AreaSeries();
		Assert.Null(series.StrokeDashArray);
		Assert.False(series.IsStacking);
		Assert.Null(series.Stroke);
		Assert.False(series.ShowMarkers);
		Assert.Null(series.ActualXAxis);
		Assert.Null(series.ActualYAxis);
		Assert.Empty(series.Label);
		Assert.True(series.ShowTrackballLabel);
		Assert.True(series.IsVisible);
		Assert.Null(series.ItemsSource);
		Assert.Null(series.XBindingPath);
		Assert.Null(series.YBindingPath);
		Assert.False(series.ListenPropertyChange);
	}

	[Fact]
	public void AreaSeries_InitializesMarkerSettingsCorrectly()
	{
		var series = new AreaSeries();
		Assert.Null(series.LabelTemplate);
		Assert.Null(series.TooltipTemplate);
		Assert.IsType<ChartMarkerSettings>(series.MarkerSettings);
		var markerSettings = series.MarkerSettings;
		Assert.Equal(Chart.ShapeType.Circle, markerSettings.Type);
		Assert.Null(markerSettings.Fill);
		Assert.Null(markerSettings.Stroke);
		Assert.Equal(0d, markerSettings.StrokeWidth);
		Assert.Equal(8d, markerSettings.Width);
		Assert.Equal(8d, markerSettings.Height);
	}

	[Fact]
	public void AreaSeries_InitializesStylePropertiesCorrectly()
	{
		var series = new AreaSeries();
		Assert.Null(series.Fill);
		Assert.Null(series.PaletteBrushes);
		Assert.Equal(1, series.StrokeWidth);
		Assert.Equal(1, series.Opacity);
		Assert.False(series.EnableTooltip);
		Assert.False(series.ShowDataLabels);
		Assert.False(series.EnableAnimation);
		Assert.Equal(ChartLegendIconType.Circle, series.LegendIcon);
	}

	[Fact]
	public void AreaSeries_InitializesDataLabelStyleCorrectly()
	{
		var series = new AreaSeries();
		var labelStyle = series.DataLabelSettings.LabelStyle;
		Assert.NotNull(labelStyle);
		Assert.Equal(3d, labelStyle.LabelPadding);
		Assert.Equal(0d, labelStyle.OffsetX);
		Assert.Equal(0d, labelStyle.OffsetY);
		Assert.Equal(0d, labelStyle.Angle);
		Assert.Equal(new CornerRadius(8), labelStyle.CornerRadius);
		Assert.Equal(new Thickness(5), labelStyle.Margin);
		Assert.Equal(Colors.Transparent, labelStyle.TextColor);
	}

	[Fact]
	public void AreaSeries_InitializesDataLabelBackgroundCorrectly()
	{
		var series = new AreaSeries();
		var labelStyle = series.DataLabelSettings.LabelStyle;
		Assert.NotNull(series.DataLabelSettings);
		Assert.Equal(DataLabelPlacement.Auto, series.DataLabelSettings.LabelPlacement);
		Assert.True(series.DataLabelSettings.UseSeriesPalette);
		Assert.Equal(DataLabelAlignment.Top, series.DataLabelSettings.BarAlignment);
		Assert.IsType<ChartDataLabelStyle>(series.DataLabelSettings.LabelStyle);
		Assert.NotNull(labelStyle);
		Assert.NotNull(labelStyle.Background);
		Assert.IsType<SolidColorBrush>(labelStyle.Background);
		var brush = (SolidColorBrush)labelStyle.Background;
		Assert.Equal(Colors.Transparent, brush.Color);
	}

	[Fact]
	public void AreaSeries_InitializesDataLabelFontSettingsCorrectly()
	{
		var series = new AreaSeries();
		var labelStyle = series.DataLabelSettings.LabelStyle;
		Assert.Null(series.SelectionBehavior);
		Assert.NotNull(labelStyle);
		Assert.Equal(0d, labelStyle.StrokeWidth);
		Assert.Equal(SolidColorBrush.Transparent, labelStyle.Stroke);
		Assert.Equal(12d, labelStyle.FontSize);
		Assert.Null(labelStyle.FontFamily);
		Assert.Equal(FontAttributes.None, labelStyle.FontAttributes);
		Assert.Equal(string.Empty, labelStyle.LabelFormat);
		Assert.Empty(series.YValues);
	}

	[Fact]
	public void AreaSeries_InitializeInternalsFontSettingsCorrectly()
	{
		var series = new AreaSeries();
		Assert.Null(series.Chart);
		Assert.Equal(1d, series.AnimationDuration);
		Assert.Equal(1, series.AnimationValue);
		Assert.Null(series.SeriesAnimation);
		Assert.False(series.NeedToAnimateSeries);
		Assert.False(series.NeedToAnimateDataLabel);
		Assert.False(series.SegmentsCreated);
		Assert.Null(series.OldSegments);
		Assert.Equal(0, series.TooltipDataPointIndex);
	}

	[Fact]
	public void AreaSeries_InitializeInternalFontSettingsCorrectly()
	{
		var series = new AreaSeries();
		Assert.Empty(series.GroupedXValuesIndexes);
		Assert.Empty(series.GroupedActualData);
		Assert.Empty(series.GroupedXValues);
		Assert.NotNull(series.DataLabels);
		Assert.NotNull(series.LabelTemplateView);
		Assert.False(series.IsMultipleYPathRequired);
		Assert.Equal(0, series.XData);
		Assert.Equal(0, series.PointsCount);
		Assert.Equal(ChartValueType.Double, series.XValueType);
		Assert.Null(series.XValues);
	}

	[Fact]
	public void AreaSeries_InitializeInternalSettingsCorrectly()
	{
		var series = new AreaSeries();
		Assert.Null(series.YComplexPaths);
		Assert.Null(series.ActualXValues);
		Assert.Null(series.SeriesYValues);
		Assert.Null(series.ActualSeriesYValues);
		Assert.Null(series.YPaths);
		Assert.Null(series.ActualData);
		Assert.Null(series.XComplexPaths);
		Assert.True(series.IsLinearData);
		Assert.False(series.IsDataPointAddedDynamically);
	}

	[Fact]
	public void AreaSeries_InitializeInternalsSettingsCorrectly()
	{
		var series = new AreaSeries();
		series.ActualXAxis = new CategoryAxis();
		Assert.False(series.IsSideBySide);
		Assert.True(series.YValues.Count == 0);
		Assert.Equal(series.DataLabelSettings, series.ChartDataLabelSettings);
		Assert.Null(series.ChartArea);
		Assert.Equal(series.ActualXAxis is CategoryAxis, series.IsIndexed);
		if (series.ActualXAxis is CategoryAxis category)
		{
			Assert.Equal(category.ArrangeByIndex, series.IsIndexed);
		}
		
		Assert.Equal(0, series.SideBySideIndex);
		Assert.False(series.IsSbsValueCalculated);
	}

	[Fact]
	public void BoxAndWhiskerSeries_InitializesBasicPropertiesCorrectly()
	{
		var series = new BoxAndWhiskerSeries();
		Assert.Equal(BoxPlotMode.Exclusive, series.BoxPlotMode);
		Assert.Equal(Chart.ShapeType.Circle, series.OutlierShapeType);
		Assert.False(series.ShowMedian);
		Assert.True(series.ShowOutlier);
		Assert.Equal(0d, series.Spacing);
		Assert.Equal(SolidColorBrush.Black, series.Stroke);
		Assert.Equal(0.8d, series.Width);
		Assert.Null(series.YDataCollection);
		Assert.False(series.IsOutlierTouch);
		Assert.Empty(series.YValues);
		Assert.False(series.ListenPropertyChange);
	}

	[Fact]
	public void BoxAndWhiskerSeries_InitializesAxisAndLabelPropertiesCorrectly()
	{
		var series = new BoxAndWhiskerSeries();
		Assert.Null(series.ActualXAxis);
		Assert.Null(series.ActualYAxis);
		Assert.Empty(series.Label);
		Assert.True(series.ShowTrackballLabel);
		Assert.True(series.IsVisible);
		Assert.Null(series.ItemsSource);
		Assert.Null(series.XBindingPath);
		Assert.Null(series.YBindingPath);
	}

	[Fact]
	public void BoxAndWhiskerSeries_InitializesStylePropertiesCorrectly()
	{
		var series = new BoxAndWhiskerSeries();
		Assert.Null(series.Fill);
		Assert.Null(series.PaletteBrushes);
		Assert.Equal(1, series.StrokeWidth);
		Assert.Equal(1, series.Opacity);
		Assert.False(series.EnableTooltip);
		Assert.False(series.ShowDataLabels);
		Assert.False(series.EnableAnimation);
		Assert.Null(series.TooltipTemplate);
		Assert.Null(series.SelectionBehavior);
	}

	[Fact]
	public void BoxAndWhiskerSeries_InitializesDataLabelSettingsCorrectly()
	{
		var series = new BoxAndWhiskerSeries();
		Assert.NotNull(series.DataLabelSettings);
		Assert.False(series.IsStacking);
		Assert.Equal(DataLabelPlacement.Auto, series.DataLabelSettings.LabelPlacement);
		Assert.True(series.DataLabelSettings.UseSeriesPalette);
		Assert.Equal(DataLabelAlignment.Top, series.DataLabelSettings.BarAlignment);
		Assert.IsType<ChartDataLabelStyle>(series.DataLabelSettings.LabelStyle);
		Assert.IsAssignableFrom<IDrawCustomLegendIcon>(series);
		var labelStyle = series.DataLabelSettings.LabelStyle;
		Assert.NotNull(labelStyle);
		Assert.NotNull(labelStyle.Background);
		Assert.IsType<SolidColorBrush>(labelStyle.Background);
	}

	[Fact]
	public void BoxAndWhiskerSeries_InitializesDataLabelStyleCorrectly()
	{
		var series = new BoxAndWhiskerSeries();
		var labelStyle = series.DataLabelSettings.LabelStyle;
		var brush = (SolidColorBrush)labelStyle.Background;
		Assert.Equal(Colors.Transparent, brush.Color);
		Assert.NotNull(labelStyle);
		Assert.Equal(3d, labelStyle.LabelPadding);
		Assert.Equal(0d, labelStyle.OffsetX);
		Assert.Equal(0d, labelStyle.OffsetY);
		Assert.Equal(0d, labelStyle.Angle);
		Assert.Equal(new CornerRadius(8), labelStyle.CornerRadius);
		Assert.Equal(new Thickness(5), labelStyle.Margin);
		Assert.Equal(Colors.Transparent, labelStyle.TextColor);
	}

	[Fact]
	public void BoxAndWhiskerSeries_InitializesDataLabelFontSettingsCorrectly()
	{
		var series = new BoxAndWhiskerSeries();
		var labelStyle = series.DataLabelSettings.LabelStyle;
		Assert.Null(series.LabelTemplate);
		Assert.NotNull(labelStyle);
		Assert.Equal(0d, labelStyle.StrokeWidth);
		Assert.Equal(SolidColorBrush.Transparent, labelStyle.Stroke);
		Assert.Equal(12d, labelStyle.FontSize);
		Assert.Null(labelStyle.FontFamily);
		Assert.Equal(FontAttributes.None, labelStyle.FontAttributes);
		Assert.Equal(string.Empty, labelStyle.LabelFormat);
		Assert.Null(series.YDataCollection);
		Assert.False(series.IsOutlierTouch);
	}

	[Fact]
	public void BoxAndWhiskerSeries_InitializeInternalsFontSettingsCorrectly()
	{
		var series = new BoxAndWhiskerSeries();
		Assert.Null(series.Chart);
		Assert.Equal(1d, series.AnimationDuration);
		Assert.Equal(1, series.AnimationValue);
		Assert.Null(series.SeriesAnimation);
		Assert.False(series.NeedToAnimateSeries);
		Assert.False(series.NeedToAnimateDataLabel);
		Assert.False(series.SegmentsCreated);
		Assert.Null(series.OldSegments);
		Assert.Equal(0, series.TooltipDataPointIndex);
	}

	[Fact]
	public void BoxAndWhiskerSeries_InitializeInternalFontSettingsCorrectly()
	{
		var series = new BoxAndWhiskerSeries();
		Assert.Empty(series.GroupedXValuesIndexes);
		Assert.Empty(series.GroupedActualData);
		Assert.Empty(series.GroupedXValues);
		Assert.NotNull(series.DataLabels);
		Assert.NotNull(series.LabelTemplateView);
		Assert.False(series.IsMultipleYPathRequired);
		Assert.Equal(0, series.XData);
		Assert.Equal(0, series.PointsCount);
		Assert.Equal(ChartValueType.Double, series.XValueType);
		Assert.Null(series.XValues);
	}
	[Fact]
	public void BoxAndWhiskerSeries_InitializeInternalSettingsCorrectly()
	{
		var series = new BoxAndWhiskerSeries();
		Assert.Null(series.YComplexPaths);
		Assert.Null(series.ActualXValues);
		Assert.Null(series.SeriesYValues);
		Assert.Null(series.ActualSeriesYValues);
		Assert.Null(series.YPaths);
		Assert.Null(series.ActualData);
		Assert.Null(series.XComplexPaths);
		Assert.True(series.IsLinearData);
		Assert.False(series.IsDataPointAddedDynamically);
	}

	[Fact]
	public void BoxAndWhiskerSeries_InitializeInternalsSettingsCorrectly()
	{
		var series = new BoxAndWhiskerSeries();
		series.ActualXAxis = new CategoryAxis();
		Assert.True(series.IsSideBySide);
		Assert.True(series.YValues.Count == 0);
		Assert.Equal(series.DataLabelSettings, series.ChartDataLabelSettings);
		Assert.Null(series.ChartArea);
		Assert.Equal(series.ActualXAxis is CategoryAxis, series.IsIndexed);

		if (series.ActualXAxis is CategoryAxis category)
		{
			Assert.Equal(category.ArrangeByIndex, series.IsIndexed);
		}

		Assert.Equal(0, series.SideBySideIndex);
		Assert.False(series.IsSbsValueCalculated);
	}

	[Fact]
	public void BubbleSeries_InitializesBasicPropertiesCorrectly()
	{
		var series = new BubbleSeries();
		Assert.Null(series.LabelTemplate);
		Assert.False(series.IsStacking);
		Assert.Equal(SolidColorBrush.Transparent, series.Stroke);
		Assert.Null(series.ActualXAxis);
		Assert.Null(series.ActualYAxis);
		Assert.Empty(series.Label);
		Assert.True(series.ShowTrackballLabel);
		Assert.True(series.IsVisible);
		Assert.Null(series.ItemsSource);
		Assert.Null(series.XBindingPath);
		Assert.Null(series.YBindingPath);
		Assert.False(series.ListenPropertyChange);
	}

	[Fact]
	public void BubbleSeries_InitializesStylePropertiesCorrectly()
	{
		var series = new BubbleSeries();
		Assert.Empty(series.YValues);
		Assert.Empty(series.SizeValuePath);
		Assert.Null(series.Fill);
		Assert.Null(series.PaletteBrushes);
		Assert.Equal(1, series.StrokeWidth);
		Assert.Equal(1, series.Opacity);
		Assert.False(series.EnableTooltip);
		Assert.False(series.ShowDataLabels);
		Assert.False(series.EnableAnimation);
		Assert.Equal(ChartLegendIconType.Circle, series.LegendIcon);
	}

	[Fact]
	public void BubbleSeries_InitializesDataLabelSettingsCorrectly()
	{
		var series = new BubbleSeries();
		Assert.Null(series.TooltipTemplate);
		Assert.Null(series.SelectionBehavior);
		Assert.Equal(3, series.MinimumRadius);
		Assert.Equal(10, series.MaximumRadius);
		Assert.NotNull(series.DataLabelSettings);
		Assert.Equal(DataLabelPlacement.Auto, series.DataLabelSettings.LabelPlacement);
		Assert.True(series.DataLabelSettings.UseSeriesPalette);
		Assert.Equal(DataLabelAlignment.Top, series.DataLabelSettings.BarAlignment);
		Assert.IsType<ChartDataLabelStyle>(series.DataLabelSettings.LabelStyle);
	}

	[Fact]
	public void BubbleSeries_InitializesDataLabelStyleCorrectly()
	{
		var series = new BubbleSeries();
		var labelStyle = series.DataLabelSettings.LabelStyle;
		Assert.NotNull(labelStyle);
		Assert.NotNull(labelStyle.Background);
		Assert.NotNull(labelStyle);
		Assert.Equal(3d, labelStyle.LabelPadding);
		Assert.Equal(0d, labelStyle.OffsetX);
		Assert.Equal(0d, labelStyle.OffsetY);
		Assert.Equal(0d, labelStyle.Angle);
		Assert.Equal(new CornerRadius(8), labelStyle.CornerRadius);
		Assert.Equal(new Thickness(5), labelStyle.Margin);
		Assert.Equal(Colors.Transparent, labelStyle.TextColor);
		Assert.IsType<SolidColorBrush>(labelStyle.Background);
	}

	[Fact]
	public void BubbleSeries_InitializesDataLabelFontSettingsCorrectly()
	{
		var series = new BubbleSeries();
		var labelStyle = series.DataLabelSettings.LabelStyle;
		var brush = (SolidColorBrush)labelStyle.Background;
		Assert.Equal(Colors.Transparent, brush.Color);
		Assert.NotNull(labelStyle);
		Assert.True(series.ShowZeroSizeBubbles);
		Assert.Equal(0d, labelStyle.StrokeWidth);
		Assert.Equal(SolidColorBrush.Transparent, labelStyle.Stroke);
		Assert.Equal(12d, labelStyle.FontSize);
		Assert.Null(labelStyle.FontFamily);
		Assert.Equal(FontAttributes.None, labelStyle.FontAttributes);
		Assert.Equal(string.Empty, labelStyle.LabelFormat);
		Assert.True(series.IsMultipleYPathRequired);
	}

	[Fact]
	public void BubbleSeries_InitializeInternalsFontSettingsCorrectly()
	{
		var series = new BubbleSeries();
		Assert.Null(series.Chart);
		Assert.Equal(1d, series.AnimationDuration);
		Assert.Equal(1, series.AnimationValue);
		Assert.Null(series.SeriesAnimation);
		Assert.False(series.NeedToAnimateSeries);
		Assert.False(series.NeedToAnimateDataLabel);
		Assert.False(series.SegmentsCreated);
		Assert.Null(series.OldSegments);
		Assert.Equal(0, series.TooltipDataPointIndex);
	}
	[Fact]
	public void BubbleSeries_InitializeInternalFontSettingsCorrectly()
	{
		var series = new BubbleSeries();
		Assert.Empty(series.GroupedXValuesIndexes);
		Assert.Empty(series.GroupedActualData);
		Assert.Empty(series.GroupedXValues);
		Assert.NotNull(series.DataLabels);
		Assert.NotNull(series.LabelTemplateView);
		Assert.True(series.IsMultipleYPathRequired);
		Assert.Equal(0, series.XData);
		Assert.Equal(0, series.PointsCount);
		Assert.Equal(ChartValueType.Double, series.XValueType);
		Assert.Null(series.XValues);
	}
	[Fact]
	public void BubbleSeries_InitializeInternalSettingsCorrectly()
	{
		var series = new BubbleSeries();
		Assert.Null(series.YComplexPaths);
		Assert.Null(series.ActualXValues);
		Assert.Null(series.SeriesYValues);
		Assert.Null(series.ActualSeriesYValues);
		Assert.Null(series.YPaths);
		Assert.Null(series.ActualData);
		Assert.Null(series.XComplexPaths);
		Assert.True(series.IsLinearData);
		Assert.False(series.IsDataPointAddedDynamically);
	}
	[Fact]
	public void BubbleSeries_InitializeInternalsSettingsCorrectly()
	{
		var series = new BubbleSeries();
		series.ActualXAxis = new CategoryAxis();
		Assert.False(series.IsSideBySide);
		Assert.False(series.IsStacking);
		Assert.True(series.YValues.Count == 0);
		Assert.Equal(series.DataLabelSettings, series.ChartDataLabelSettings);
		Assert.Null(series.ChartArea);
		Assert.Equal(series.ActualXAxis is CategoryAxis, series.IsIndexed);

		if (series.ActualXAxis is CategoryAxis category)
		{
			Assert.Equal(category.ArrangeByIndex, series.IsIndexed);
		}

		Assert.Equal(0, series.SideBySideIndex);
		Assert.False(series.IsSbsValueCalculated);
	}

	[Fact]
	public void CandleSeriesDefaultTests_Part2()
	{
		var series = new CandleSeries();
		Assert.Equal(string.Empty, series.High);
		Assert.Equal(string.Empty, series.Low);
		Assert.Equal(string.Empty, series.Open);
		Assert.Equal(string.Empty, series.Close);
		Assert.Null(series.Stroke);
		Assert.False(series.EnableSolidCandle);
		Assert.True(series.IsSideBySide);
		Assert.Equal(0d, series.Spacing);
		Assert.False(series.ListenPropertyChange);
	}

	[Fact]
	public void CandleSeriesDefaultTests_Part3()
	{
		var series = new CandleSeries();
		Assert.Equal(0.8d, series.Width);
		Assert.NotNull(series.HighValues);
		Assert.NotNull(series.LowValues);
		Assert.NotNull(series.OpenValues);
		Assert.NotNull(series.CloseValues);
		Assert.Empty(series.HighValues);
		Assert.Empty(series.LowValues);
		Assert.Empty(series.OpenValues);
	}
	[Fact]
	public void CandleSeriesDefaultTests_Part5()
	{
		var series = new CandleSeries();
		Assert.Equal(float.NaN, series._sumOfHighValues);
		Assert.Equal(float.NaN, series._sumOfLowValues);
		Assert.Equal(float.NaN, series._sumOfOpenValues);
		Assert.Equal(float.NaN, series._sumOfCloseValues);
		Assert.IsType<SolidColorBrush>(series.BearishFill);
		Assert.Equal(Color.FromArgb("#C15146"), (series.BearishFill as SolidColorBrush)?.Color);
		Assert.IsType<SolidColorBrush>(series.BullishFill);
		Assert.Equal(Color.FromArgb("#90A74F"), (series.BullishFill as SolidColorBrush)?.Color);
	}

	[Fact]
	public void CandleSeriesDefaultTests_Part6()
	{
		var series = new CandleSeries();
		Assert.Null(series.ActualXAxis);
		Assert.Null(series.ActualYAxis);
		Assert.NotNull(series.DataLabelSettings);
		Assert.Equal(string.Empty, series.Label);
		Assert.True(series.ShowTrackballLabel);
		Assert.Null(series.TrackballLabelTemplate);
		Assert.Null(series.XAxisName);
		Assert.Null(series.YAxisName);
	}

	[Fact]
	public void CandleSeriesDefaultTests_Part7()
	{
		var series = new CandleSeries();
		Assert.True(series.IsVisible);
		Assert.True(series.IsVisibleOnLegend);
		Assert.Null(series.ItemsSource);
		Assert.Equal(LabelContext.YValue, series.LabelContext);
		Assert.Null(series.LabelTemplate);
		Assert.Equal(ChartLegendIconType.Circle, series.LegendIcon);
		Assert.Null(series.PaletteBrushes);
		Assert.False(series.ShowDataLabels);
	}

	[Fact]
	public void CandleSeriesDefaultTests_Part8()
	{
		var series = new CandleSeries();
		Assert.Null(series.ActualData);
		Assert.Null(series.TooltipTemplate);
		Assert.False(series.EnableTooltip);
		Assert.Null(series.Fill);
		Assert.Null(series.XBindingPath);
		Assert.Null(series.SelectionBehavior);
		Assert.Equal(1d, series.Opacity);
		Assert.False(series.EnableAnimation);
	}

	[Fact]
	public void CandleSeriesDefaultTests_Part9()
	{
		var series = new CandleSeries();
		Assert.True(series.IsMultipleYPathRequired);
		Assert.Null(series.Chart);
		Assert.Equal(1d, series.AnimationDuration);
		Assert.Equal(1, series.AnimationValue);
		Assert.Null(series.SeriesAnimation);
		Assert.False(series.NeedToAnimateSeries);
		Assert.False(series.NeedToAnimateDataLabel);
		Assert.False(series.SegmentsCreated);
	}

	[Fact]
	public void CandleSeriesDefaultTests_Part10()
	{
		var series = new CandleSeries();
		Assert.Null(series.OldSegments);
		Assert.Equal(0, series.TooltipDataPointIndex);
		Assert.Empty(series.GroupedXValuesIndexes);
		Assert.Empty(series.GroupedActualData);
		Assert.Empty(series.GroupedXValues);
		Assert.NotNull(series.DataLabels);
		Assert.NotNull(series.LabelTemplateView);
		Assert.Null(series.YPaths);
	}

	[Fact]
	public void CandleSeriesDefaultTests_Part11()
	{
		var series = new CandleSeries();
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
	public void CandleSeriesDefaultTests_Part13()
	{
		var series = new CandleSeries();
		Assert.True(series.IsSideBySide);
		Assert.Equal(0, series.SideBySideIndex);
		Assert.False(series.IsSbsValueCalculated);
		Assert.Null(series.ChartArea);
		Assert.True(series.IsLinearData);
		Assert.False(series.IsDataPointAddedDynamically);
		Assert.False(series.IsStacking);
		Assert.Null(series.XComplexPaths);
		Assert.False(series.IsIndexed);
		Assert.Empty(series.CloseValues);
	}

	[Fact]
	public void CandleSeriesDefaultTests_Part17()
	{
		var series = new CandleSeries();
		Assert.NotNull(series.HighValues);
		Assert.NotNull(series.LowValues);
		Assert.NotNull(series.OpenValues);
		Assert.NotNull(series.CloseValues);
		Assert.IsType<List<double>>(series.HighValues);
		Assert.IsType<List<double>>(series.LowValues);
		Assert.IsType<List<double>>(series.OpenValues);
		Assert.IsType<List<double>>(series.CloseValues);
	}

	[Fact]
	public void CandleSeriesDefaultTests_Part18()
	{
		var series = new CandleSeries();
		Assert.IsType<CartesianDataLabelSettings>(series.DataLabelSettings);
		Assert.Equal(series.DataLabelSettings, series.ChartDataLabelSettings);
		Assert.NotNull(series.DataLabels);
		Assert.IsType<ObservableCollection<ChartDataLabel>>(series.DataLabels);
		Assert.Equal(5f, series._defaultSelectionStrokeWidth);
		Assert.NotNull(series.LabelTemplateView);
		Assert.IsType<DataLabelLayout>(series.LabelTemplateView);
		Assert.Empty(series.DataLabels);
	}
	[Fact]
    public void WaterfallSeriesDefaultTests_Part1()
    {
        var series = new WaterfallSeries();
        Assert.True(series.AllowAutoSum);
        Assert.True(series.ShowConnectorLine);
        Assert.NotNull(series.ConnectorLineStyle);
        Assert.Equal(string.Empty, series.SummaryBindingPath);
        Assert.Null(series.SummaryPointsBrush);
        Assert.Null(series.NegativePointsBrush);
        Assert.Equal(0.8d, series.Width);
        Assert.Equal(0d, series.Spacing);
        Assert.Equal(1d, series.StrokeWidth);
		Assert.False(series.ListenPropertyChange);
	}

    [Fact]
    public void WaterfallSeriesDefaultTests_Part2()
    {
        var series = new WaterfallSeries();
        Assert.Null(series.YBindingPath);
        Assert.Null(series.ActualXAxis);
        Assert.Null(series.ActualYAxis);
        Assert.Equal(string.Empty, series.Label);
        Assert.True(series.ShowTrackballLabel);
        Assert.Null(series.TrackballLabelTemplate);
        Assert.Null(series.XAxisName);
        Assert.Null(series.YAxisName);
    }

    [Fact]
    public void WaterfallSeriesDefaultTests_Part3()
    {
        var series = new WaterfallSeries();
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
    public void WaterfallSeriesDefaultTests_Part4()
    {
        var series = new WaterfallSeries();
        Assert.False(series.EnableTooltip);
        Assert.Null(series.Fill);
        Assert.Null(series.XBindingPath);
        Assert.Null(series.SelectionBehavior);
        Assert.Equal(1d, series.Opacity);
        Assert.False(series.EnableAnimation);
        Assert.True(series.IsSideBySide);
        Assert.NotNull(series.YValues);
        Assert.Empty(series.YValues);
    }

    [Fact]
    public void WaterfallSeriesDefaultTests_Part5()
    {
        var series = new WaterfallSeries();
		series.ActualXAxis = new CategoryAxis();
		Assert.Equal(series.DataLabelSettings, series.ChartDataLabelSettings);
        Assert.True(series.SbsInfo.IsEmpty);
        Assert.Null(series.ChartArea);
        Assert.Equal(series.ActualXAxis is CategoryAxis, series.IsIndexed);
        
        if (series.ActualXAxis is CategoryAxis category)
        {
            Assert.Equal(category.ArrangeByIndex, series.IsIndexed);
        }

        Assert.Equal(0, series.SideBySideIndex);
        Assert.False(series.IsSbsValueCalculated);
    }

    [Fact]
    public void WaterfallSeriesDefaultTests_Part6()
    {
        var series = new WaterfallSeries();
        Assert.Null(series.Chart);
        Assert.Equal(1d, series.AnimationDuration);
        Assert.Equal(1, series.AnimationValue);
        Assert.Null(series.SeriesAnimation);
        Assert.False(series.NeedToAnimateSeries);
        Assert.False(series.NeedToAnimateDataLabel);
        Assert.False(series.SegmentsCreated);
        Assert.False(series.VisibleXRange.IsEmpty);
        Assert.False(series.VisibleYRange.IsEmpty);
    }

    [Fact]
    public void WaterfallSeriesDefaultTests_Part7()
    {
        var series = new WaterfallSeries();
        Assert.Null(series.OldSegments);
        Assert.False(series.PreviousXRange.IsEmpty);
        Assert.Equal(0, series.TooltipDataPointIndex);
        Assert.Empty(series.GroupedXValuesIndexes);
        Assert.Empty(series.GroupedActualData);
        Assert.Empty(series.GroupedXValues);
        Assert.NotNull(series.DataLabels);
        Assert.NotNull(series.LabelTemplateView);
        Assert.False(series.IsMultipleYPathRequired);
    }

    [Fact]
    public void WaterfallSeriesDefaultTests_Part8()
    {
        var series = new WaterfallSeries();
        Assert.True(series.IsSideBySide);
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
    public void WaterfallSeriesDefaultTests_Part9()
    {
        var series = new WaterfallSeries();
        Assert.Null(series.YPaths);
        Assert.Null(series.ActualData);
        Assert.Null(series.XComplexPaths);
        Assert.True(series.IsLinearData);
        Assert.False(series.IsDataPointAddedDynamically);
    }

	[Fact]
	public void WaterfallSeriesDefaultTests_Part10()
	{
		var series = new WaterfallSeries();

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
    public void StepLineSeriesDefaultTests_Part1()
    {
        var series = new StepLineSeries();
        Assert.Null(series.StrokeDashArray);
        Assert.False(series.ShowMarkers);
        Assert.NotNull(series.MarkerSettings);
        Assert.IsType<ChartMarkerSettings>(series.MarkerSettings);
        Assert.Equal(1d, series.StrokeWidth);
        Assert.Null(series.YBindingPath);
        Assert.Null(series.ActualXAxis);
        Assert.Null(series.ActualYAxis);
		Assert.False(series.ListenPropertyChange);
	}

    [Fact]
    public void StepLineSeriesDefaultTests_Part2()
    {
        var series = new StepLineSeries();
        Assert.Equal(string.Empty, series.Label);
        Assert.True(series.ShowTrackballLabel);
        Assert.Null(series.TrackballLabelTemplate);
        Assert.Null(series.XAxisName);
        Assert.Null(series.YAxisName);
        Assert.True(series.IsVisible);
        Assert.True(series.IsVisibleOnLegend);
    }

    [Fact]
    public void StepLineSeriesDefaultTests_Part3()
    {
        var series = new StepLineSeries();
        Assert.Null(series.ItemsSource);
        Assert.Equal(LabelContext.YValue, series.LabelContext);
        Assert.Null(series.LabelTemplate);
        Assert.Equal(Chart.ChartLegendIconType.Circle, series.LegendIcon);
        Assert.Null(series.PaletteBrushes);
        Assert.False(series.ShowDataLabels);
        Assert.Null(series.TooltipTemplate);
        Assert.False(series.EnableTooltip);
    }

    [Fact]
    public void StepLineSeriesDefaultTests_Part4()
    {
        var series = new StepLineSeries();
        Assert.Null(series.Fill);
        Assert.Null(series.XBindingPath);
        Assert.Null(series.SelectionBehavior);
        Assert.Equal(1d, series.Opacity);
        Assert.False(series.EnableAnimation);
        Assert.False(series.IsSideBySide);
        Assert.NotNull(series.YValues);
        Assert.Empty(series.YValues);
    }

    [Fact]
    public void StepLineSeriesDefaultTests_Part5()
    {
        var series = new StepLineSeries();
		series.ActualXAxis = new CategoryAxis();
		Assert.Equal(series.DataLabelSettings, series.ChartDataLabelSettings);
        Assert.True(series.SbsInfo.IsEmpty);
        Assert.Null(series.ChartArea);
        Assert.Equal(series.ActualXAxis is CategoryAxis, series.IsIndexed);
        
        if (series.ActualXAxis is CategoryAxis category)
        {
            Assert.Equal(category.ArrangeByIndex, series.IsIndexed);
        }
    }

    [Fact]
    public void StepLineSeriesDefaultTests_Part6()
    {
        var series = new StepLineSeries();
        Assert.Equal(0, series.SideBySideIndex);
        Assert.False(series.IsSbsValueCalculated);
        Assert.Null(series.Chart);
        Assert.Equal(1d, series.AnimationDuration);
        Assert.Equal(1, series.AnimationValue);
        Assert.Null(series.SeriesAnimation);
        Assert.False(series.NeedToAnimateSeries);
        Assert.False(series.NeedToAnimateDataLabel);
    }

    [Fact]
    public void StepLineSeriesDefaultTests_Part7()
    {
        var series = new StepLineSeries();
        Assert.False(series.SegmentsCreated);
		Assert.False(series.VisibleXRange.IsEmpty);
		Assert.False(series.VisibleYRange.IsEmpty);
		Assert.False(series.PreviousXRange.IsEmpty);
		Assert.Null(series.OldSegments);
        Assert.Equal(0, series.TooltipDataPointIndex);
        Assert.Empty(series.GroupedXValuesIndexes);
        Assert.Empty(series.GroupedActualData);
    }

    [Fact]
    public void StepLineSeriesDefaultTests_Part8()
    {
        var series = new StepLineSeries();
        Assert.Empty(series.GroupedXValues);
        Assert.NotNull(series.DataLabels);
        Assert.NotNull(series.LabelTemplateView);
        Assert.False(series.IsMultipleYPathRequired);
        Assert.False(series.IsSideBySide);
        Assert.Equal(0, series.XData);
        Assert.Equal(0, series.PointsCount);
        Assert.Equal(ChartValueType.Double, series.XValueType);
    }

    [Fact]
    public void StepLineSeriesDefaultTests_Part9()
    {
        var series = new StepLineSeries();
        Assert.Null(series.XValues);
        Assert.Null(series.YComplexPaths);
        Assert.Null(series.ActualXValues);
        Assert.Null(series.SeriesYValues);
        Assert.Null(series.ActualSeriesYValues);
        Assert.Null(series.YPaths);
        Assert.Null(series.ActualData);
        Assert.Null(series.XComplexPaths);
        Assert.True(series.IsLinearData);
        Assert.False(series.IsDataPointAddedDynamically);
    }

	[Fact]
	public void StepLineSeriesDefaultTests_Part10()
	{
		var series = new StepLineSeries();

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
    public void StepAreaSeriesDefaultTests_Part1()
    {
        var series = new StepAreaSeries();
        Assert.Null(series.StrokeDashArray);
        Assert.Null(series.Stroke);
        Assert.False(series.ShowMarkers);
        Assert.NotNull(series.MarkerSettings);
        Assert.IsType<ChartMarkerSettings>(series.MarkerSettings);
        Assert.Equal(1d, series.StrokeWidth);
        Assert.Null(series.YBindingPath);
        Assert.Null(series.ActualXAxis);
		Assert.False(series.ListenPropertyChange);
	}

    [Fact]
    public void StepAreaSeriesDefaultTests_Part2()
    {
        var series = new StepAreaSeries();
        Assert.Null(series.ActualYAxis);
        Assert.Equal(string.Empty, series.Label);
        Assert.True(series.ShowTrackballLabel);
        Assert.Null(series.TrackballLabelTemplate);
        Assert.Null(series.XAxisName);
        Assert.Null(series.YAxisName);
        Assert.True(series.IsVisible);
    }

    [Fact]
    public void StepAreaSeriesDefaultTests_Part3()
    {
        var series = new StepAreaSeries();
        Assert.True(series.IsVisibleOnLegend);
        Assert.Null(series.ItemsSource);
        Assert.Equal(LabelContext.YValue, series.LabelContext);
        Assert.Null(series.LabelTemplate);
        Assert.Equal(Chart.ChartLegendIconType.Circle, series.LegendIcon);
        Assert.Null(series.PaletteBrushes);
        Assert.False(series.ShowDataLabels);
        Assert.Null(series.TooltipTemplate);
    }

    [Fact]
    public void StepAreaSeriesDefaultTests_Part4()
    {
        var series = new StepAreaSeries();
        Assert.False(series.EnableTooltip);
        Assert.Null(series.Fill);
        Assert.Null(series.XBindingPath);
        Assert.Null(series.SelectionBehavior);
        Assert.Equal(1d, series.Opacity);
        Assert.False(series.EnableAnimation);
        Assert.False(series.IsSideBySide);
        Assert.NotNull(series.YValues);
        Assert.Empty(series.YValues);
    }

    [Fact]
    public void StepAreaSeriesDefaultTests_Part5()
    {
        var series = new StepAreaSeries();
		series.ActualXAxis = new CategoryAxis();
		Assert.Equal(series.DataLabelSettings, series.ChartDataLabelSettings);
        Assert.True(series.SbsInfo.IsEmpty);
        Assert.Null(series.ChartArea);
        Assert.Equal(series.ActualXAxis is CategoryAxis, series.IsIndexed);
        
        if (series.ActualXAxis is CategoryAxis category)
        {
            Assert.Equal(category.ArrangeByIndex, series.IsIndexed);
        }
    }

	[Fact]
    public void StepAreaSeriesDefaultTests_Part6()
    {
        var series = new StepAreaSeries();
        Assert.Equal(0, series.SideBySideIndex);
        Assert.False(series.IsSbsValueCalculated);
        Assert.Null(series.Chart);
        Assert.Equal(1d, series.AnimationDuration);
        Assert.Equal(1, series.AnimationValue);
        Assert.Null(series.SeriesAnimation);
        Assert.False(series.NeedToAnimateSeries);
        Assert.False(series.NeedToAnimateDataLabel);
    }

    [Fact]
    public void StepAreaSeriesDefaultTests_Part7()
    {
        var series = new StepAreaSeries();
        Assert.False(series.SegmentsCreated);
		Assert.False(series.VisibleXRange.IsEmpty);
		Assert.False(series.VisibleYRange.IsEmpty);
		Assert.False(series.PreviousXRange.IsEmpty);
		Assert.Null(series.OldSegments);
        Assert.Equal(0, series.TooltipDataPointIndex);
        Assert.Empty(series.GroupedXValuesIndexes);
        Assert.Empty(series.GroupedActualData);
    }

    [Fact]
    public void StepAreaSeriesDefaultTests_Part8()
    {
        var series = new StepAreaSeries();
        Assert.Empty(series.GroupedXValues);
        Assert.NotNull(series.DataLabels);
        Assert.NotNull(series.LabelTemplateView);
        Assert.False(series.IsMultipleYPathRequired);
        Assert.False(series.IsSideBySide);
        Assert.Equal(0, series.XData);
        Assert.Equal(0, series.PointsCount);
        Assert.Equal(ChartValueType.Double, series.XValueType);
    }

    [Fact]
    public void StepAreaSeriesDefaultTests_Part9()
    {
        var series = new StepAreaSeries();
        Assert.Null(series.XValues);
        Assert.Null(series.YComplexPaths);
        Assert.Null(series.ActualXValues);
        Assert.Null(series.SeriesYValues);
        Assert.Null(series.ActualSeriesYValues);
        Assert.Null(series.YPaths);
        Assert.Null(series.ActualData);
        Assert.Null(series.XComplexPaths);
        Assert.True(series.IsLinearData);
        Assert.False(series.IsDataPointAddedDynamically);
    }

	[Fact]
	public void StepAreaSeriesDefaultTests_Part10()
	{
		var series = new StepAreaSeries();

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
    public void StackingLineSeriesDefaultTests_Part1()
    {
        var series = new StackingLineSeries();
        Assert.False(series.ShowMarkers);
        Assert.NotNull(series.MarkerSettings);
        Assert.IsType<ChartMarkerSettings>(series.MarkerSettings);
        Assert.Equal(string.Empty, series.GroupingLabel);
        Assert.Null(series.Stroke);
        Assert.Null(series.StrokeDashArray);
        Assert.True(series.IsStacking);
        Assert.Null(series.TopValues);
		Assert.False(series.ListenPropertyChange);
	}

    [Fact]
    public void StackingLineSeriesDefaultTests_Part2()
    {
        var series = new StackingLineSeries();
        Assert.Null(series.BottomValues);
        Assert.Equal(1d, series.StrokeWidth);
        Assert.Null(series.YBindingPath);
        Assert.Null(series.ActualXAxis);
        Assert.Null(series.ActualYAxis);
        Assert.Equal(string.Empty, series.Label);
        Assert.True(series.ShowTrackballLabel);
    }

    [Fact]
    public void StackingLineSeriesDefaultTests_Part3()
    {
        var series = new StackingLineSeries();
        Assert.Null(series.TrackballLabelTemplate);
        Assert.Null(series.XAxisName);
        Assert.Null(series.YAxisName);
        Assert.True(series.IsVisible);
        Assert.True(series.IsVisibleOnLegend);
        Assert.Null(series.ItemsSource);
        Assert.Equal(LabelContext.YValue, series.LabelContext);
        Assert.Null(series.LabelTemplate);
    }

    [Fact]
    public void StackingLineSeriesDefaultTests_Part4()
    {
        var series = new StackingLineSeries();
        Assert.Equal(Chart.ChartLegendIconType.Circle, series.LegendIcon);
        Assert.Null(series.PaletteBrushes);
        Assert.False(series.ShowDataLabels);
        Assert.Null(series.TooltipTemplate);
        Assert.False(series.EnableTooltip);
        Assert.Null(series.Fill);
        Assert.Null(series.XBindingPath);
        Assert.Null(series.SelectionBehavior);
    }

    [Fact]
    public void StackingLineSeriesDefaultTests_Part5()
    {
        var series = new StackingLineSeries();
        Assert.Equal(1d, series.Opacity);
        Assert.False(series.EnableAnimation);
        Assert.False(series.IsSideBySide);
        Assert.NotNull(series.YValues);
        Assert.Empty(series.YValues);
        Assert.Equal(series.DataLabelSettings, series.ChartDataLabelSettings);
        Assert.True(series.SbsInfo.IsEmpty);
        Assert.Null(series.ChartArea);
    }

    [Fact]
    public void StackingLineSeriesDefaultTests_Part6()
    {
        var series = new StackingLineSeries();
		series.ActualXAxis = new CategoryAxis();
		Assert.Equal(series.ActualXAxis is CategoryAxis, series.IsIndexed);
        
        if (series.ActualXAxis is CategoryAxis category)
        {
            Assert.Equal(category.ArrangeByIndex, series.IsIndexed);
        }

        Assert.Equal(0, series.SideBySideIndex);
        Assert.False(series.IsSbsValueCalculated);
    }

    [Fact]
    public void StackingLineSeriesDefaultTests_Part7()
    {
        var series = new StackingLineSeries();
        Assert.Null(series.Chart);
        Assert.Equal(1d, series.AnimationDuration);
        Assert.Equal(1, series.AnimationValue);
        Assert.Null(series.SeriesAnimation);
        Assert.False(series.NeedToAnimateSeries);
        Assert.False(series.NeedToAnimateDataLabel);
        Assert.False(series.SegmentsCreated);
        Assert.False(series.VisibleXRange.IsEmpty);
    }

    [Fact]
    public void StackingLineSeriesDefaultTests_Part8()
    {
        var series = new StackingLineSeries();
        Assert.False(series.VisibleYRange.IsEmpty);
        Assert.Null(series.OldSegments);
        Assert.False(series.PreviousXRange.IsEmpty);
        Assert.Equal(0, series.TooltipDataPointIndex);
        Assert.Empty(series.GroupedXValuesIndexes);
        Assert.Empty(series.GroupedActualData);
        Assert.Empty(series.GroupedXValues);
        Assert.NotNull(series.DataLabels);
    }

    [Fact]
    public void StackingLineSeriesDefaultTests_Part9()
    {
        var series = new StackingLineSeries();
        Assert.NotNull(series.LabelTemplateView);
        Assert.False(series.IsMultipleYPathRequired);
        Assert.False(series.IsSideBySide);
        Assert.Equal(0, series.XData);
        Assert.Equal(0, series.PointsCount);
        Assert.Equal(ChartValueType.Double, series.XValueType);
        Assert.Null(series.XValues);
        Assert.Null(series.YComplexPaths);
    }

    [Fact]
    public void StackingLineSeriesDefaultTests_Part10()
    {
        var series = new StackingLineSeries();
        Assert.Null(series.ActualXValues);
        Assert.Null(series.SeriesYValues);
        Assert.Null(series.ActualSeriesYValues);
        Assert.Null(series.YPaths);
        Assert.Null(series.ActualData);
        Assert.Null(series.XComplexPaths);
        Assert.True(series.IsLinearData);
        Assert.False(series.IsDataPointAddedDynamically);
	}

	[Fact]
	public void StackingLineSeriesDefaultTests_Part11()
	{
		var series = new StackingLineSeries();

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
    public void StackingLine100SeriesDefaultTests_Part1()
    {
        var series = new StackingLine100Series();
        Assert.False(series.ShowMarkers);
        Assert.NotNull(series.MarkerSettings);
        Assert.IsType<ChartMarkerSettings>(series.MarkerSettings);
        Assert.Equal(string.Empty, series.GroupingLabel);
        Assert.Null(series.Stroke);
        Assert.Null(series.StrokeDashArray);
        Assert.True(series.IsStacking);
        Assert.Null(series.TopValues);
		Assert.False(series.ListenPropertyChange);
	}

    [Fact]
    public void StackingLine100SeriesDefaultTests_Part2()
    {
        var series = new StackingLine100Series();
        Assert.Null(series.BottomValues);
        Assert.Equal(1d, series.StrokeWidth);
        Assert.Null(series.YBindingPath);
        Assert.Null(series.ActualXAxis);
        Assert.Null(series.ActualYAxis);
        Assert.Equal(string.Empty, series.Label);
        Assert.True(series.ShowTrackballLabel);
    }

    [Fact]
    public void StackingLine100SeriesDefaultTests_Part3()
    {
        var series = new StackingLine100Series();
        Assert.Null(series.TrackballLabelTemplate);
        Assert.Null(series.XAxisName);
        Assert.Null(series.YAxisName);
        Assert.True(series.IsVisible);
        Assert.True(series.IsVisibleOnLegend);
        Assert.Null(series.ItemsSource);
        Assert.Equal(LabelContext.YValue, series.LabelContext);
        Assert.Null(series.LabelTemplate);
    }

    [Fact]
    public void StackingLine100SeriesDefaultTests_Part4()
    {
        var series = new StackingLine100Series();
        Assert.Equal(Chart.ChartLegendIconType.Circle, series.LegendIcon);
        Assert.Null(series.PaletteBrushes);
        Assert.False(series.ShowDataLabels);
        Assert.Null(series.TooltipTemplate);
        Assert.False(series.EnableTooltip);
        Assert.Null(series.Fill);
        Assert.Null(series.XBindingPath);
        Assert.Null(series.SelectionBehavior);
    }

    [Fact]
    public void StackingLine100SeriesDefaultTests_Part5()
    {
        var series = new StackingLine100Series();
        Assert.Equal(1d, series.Opacity);
        Assert.False(series.EnableAnimation);
        Assert.False(series.IsSideBySide);
        Assert.NotNull(series.YValues);
        Assert.Empty(series.YValues);
        Assert.Equal(series.DataLabelSettings, series.ChartDataLabelSettings);
        Assert.True(series.SbsInfo.IsEmpty);
        Assert.Null(series.ChartArea);
    }

    [Fact]
    public void StackingLine100SeriesDefaultTests_Part6()
    {
        var series = new StackingLine100Series();
		series.ActualXAxis = new CategoryAxis();
		Assert.Equal(series.ActualXAxis is CategoryAxis, series.IsIndexed);
        
        if (series.ActualXAxis is CategoryAxis category)
        {
            Assert.Equal(category.ArrangeByIndex, series.IsIndexed);
        }

        Assert.Equal(0, series.SideBySideIndex);
        Assert.False(series.IsSbsValueCalculated);
    }

    [Fact]
    public void StackingLine100SeriesDefaultTests_Part7()
    {
        var series = new StackingLine100Series();
        Assert.Null(series.Chart);
        Assert.Equal(1d, series.AnimationDuration);
        Assert.Equal(1, series.AnimationValue);
        Assert.Null(series.SeriesAnimation);
        Assert.False(series.NeedToAnimateSeries);
        Assert.False(series.NeedToAnimateDataLabel);
        Assert.False(series.SegmentsCreated);
        Assert.False(series.VisibleXRange.IsEmpty);
    }

    [Fact]
    public void StackingLine100SeriesDefaultTests_Part8()
    {
        var series = new StackingLine100Series();
        Assert.False(series.VisibleYRange.IsEmpty);
        Assert.Null(series.OldSegments);
        Assert.False(series.PreviousXRange.IsEmpty);
        Assert.Equal(0, series.TooltipDataPointIndex);
        Assert.Empty(series.GroupedXValuesIndexes);
        Assert.Empty(series.GroupedActualData);
        Assert.Empty(series.GroupedXValues);
        Assert.NotNull(series.DataLabels);
    }

    [Fact]
    public void StackingLine100SeriesDefaultTests_Part9()
    {
        var series = new StackingLine100Series();
        Assert.NotNull(series.LabelTemplateView);
        Assert.False(series.IsMultipleYPathRequired);
        Assert.False(series.IsSideBySide);
        Assert.Equal(0, series.XData);
        Assert.Equal(0, series.PointsCount);
        Assert.Equal(ChartValueType.Double, series.XValueType);
        Assert.Null(series.XValues);
        Assert.Null(series.YComplexPaths);
    }

    [Fact]
    public void StackingLine100SeriesDefaultTests_Part10()
    {
        var series = new StackingLine100Series();
        Assert.Null(series.ActualXValues);
        Assert.Null(series.SeriesYValues);
        Assert.Null(series.ActualSeriesYValues);
        Assert.Null(series.YPaths);
        Assert.Null(series.ActualData);
        Assert.Null(series.XComplexPaths);
        Assert.True(series.IsLinearData);
        Assert.False(series.IsDataPointAddedDynamically);
    }

	[Fact]
	public void StackingLine100SeriesDefaultTests_Part11()
	{
		var series = new StackingLine100Series();

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
    public void StackingColumnSeriesDefaultTests_Part1()
    {
        var series = new StackingColumnSeries();
        Assert.Equal(0d, series.Spacing);
        Assert.Equal(0.8d, series.Width);
        Assert.True(series.IsSideBySide);
        Assert.Equal(string.Empty, series.GroupingLabel);
        Assert.Null(series.Stroke);
        Assert.Null(series.StrokeDashArray);
        Assert.True(series.IsStacking);
        Assert.Null(series.TopValues);
		Assert.False(series.ListenPropertyChange);
	}

    [Fact]
    public void StackingColumnSeriesDefaultTests_Part2()
    {
        var series = new StackingColumnSeries();
        Assert.Null(series.BottomValues);
        Assert.Equal(1d, series.StrokeWidth);
        Assert.Null(series.YBindingPath);
        Assert.Null(series.ActualXAxis);
        Assert.Null(series.ActualYAxis);
        Assert.Equal(string.Empty, series.Label);
        Assert.True(series.ShowTrackballLabel);
    }

    [Fact]
    public void StackingColumnSeriesDefaultTests_Part3()
    {
        var series = new StackingColumnSeries();
        Assert.Null(series.TrackballLabelTemplate);
        Assert.Null(series.XAxisName);
        Assert.Null(series.YAxisName);
        Assert.True(series.IsVisible);
        Assert.True(series.IsVisibleOnLegend);
        Assert.Null(series.ItemsSource);
        Assert.Equal(LabelContext.YValue, series.LabelContext);
        Assert.Null(series.LabelTemplate);
    }

    [Fact]
    public void StackingColumnSeriesDefaultTests_Part4()
    {
        var series = new StackingColumnSeries();
        Assert.Equal(Chart.ChartLegendIconType.Circle, series.LegendIcon);
        Assert.Null(series.PaletteBrushes);
        Assert.False(series.ShowDataLabels);
        Assert.Null(series.TooltipTemplate);
        Assert.False(series.EnableTooltip);
        Assert.Null(series.Fill);
        Assert.Null(series.XBindingPath);
        Assert.Null(series.SelectionBehavior);
    }

    [Fact]
    public void StackingColumnSeriesDefaultTests_Part5()
    {
        var series = new StackingColumnSeries();
        Assert.Equal(1d, series.Opacity);
        Assert.False(series.EnableAnimation);
        Assert.True(series.IsSideBySide);
        Assert.NotNull(series.YValues);
        Assert.Empty(series.YValues);
        Assert.Equal(series.DataLabelSettings, series.ChartDataLabelSettings);
		Assert.True(series.SbsInfo.IsEmpty);
		Assert.Null(series.ChartArea);
    }

    [Fact]
    public void StackingColumnSeriesDefaultTests_Part6()
    {
        var series = new StackingColumnSeries();
		series.ActualXAxis = new CategoryAxis();
		Assert.Equal(series.ActualXAxis is CategoryAxis, series.IsIndexed);
        
        if (series.ActualXAxis is CategoryAxis category)
        {
            Assert.Equal(category.ArrangeByIndex, series.IsIndexed);
        }

        Assert.Equal(0, series.SideBySideIndex);
        Assert.False(series.IsSbsValueCalculated);
    }

    [Fact]
    public void StackingColumnSeriesDefaultTests_Part7()
    {
        var series = new StackingColumnSeries();
        Assert.Null(series.Chart);
        Assert.Equal(1d, series.AnimationDuration);
        Assert.Equal(1, series.AnimationValue);
        Assert.Null(series.SeriesAnimation);
        Assert.False(series.NeedToAnimateSeries);
        Assert.False(series.NeedToAnimateDataLabel);
        Assert.False(series.SegmentsCreated);
        Assert.False(series.VisibleXRange.IsEmpty);
    }

    [Fact]
    public void StackingColumnSeriesDefaultTests_Part8()
    {
        var series = new StackingColumnSeries();
        Assert.False(series.VisibleYRange.IsEmpty);
        Assert.False(series.PreviousXRange.IsEmpty);
        Assert.Null(series.OldSegments);
        Assert.Equal(0, series.TooltipDataPointIndex);
        Assert.Empty(series.GroupedXValuesIndexes);
        Assert.Empty(series.GroupedActualData);
        Assert.Empty(series.GroupedXValues);
        Assert.NotNull(series.DataLabels);
    }

    [Fact]
    public void StackingColumnSeriesDefaultTests_Part9()
    {
        var series = new StackingColumnSeries();
        Assert.NotNull(series.LabelTemplateView);
        Assert.False(series.IsMultipleYPathRequired);
        Assert.True(series.IsSideBySide);
        Assert.Equal(0, series.XData);
        Assert.Equal(0, series.PointsCount);
        Assert.Equal(ChartValueType.Double, series.XValueType);
        Assert.Null(series.XValues);
        Assert.Null(series.YComplexPaths);
    }

    [Fact]
    public void StackingColumnSeriesDefaultTests_Part10()
    {
        var series = new StackingColumnSeries();
        Assert.Null(series.ActualXValues);
        Assert.Null(series.SeriesYValues);
        Assert.Null(series.ActualSeriesYValues);
        Assert.Null(series.YPaths);
        Assert.Null(series.ActualData);
        Assert.Null(series.XComplexPaths);
        Assert.True(series.IsLinearData);
        Assert.False(series.IsDataPointAddedDynamically);
    }

	[Fact]
	public void StackingColumnSeriesDefaultTests_Part11()
	{
		var series = new StackingColumnSeries();

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
    public void StackingColumn100SeriesDefaultTests_Part1()
    {
        var series = new StackingColumn100Series();
        Assert.Equal(0d, series.Spacing);
        Assert.Equal(0.8d, series.Width);
        Assert.True(series.IsSideBySide);
        Assert.Equal(string.Empty, series.GroupingLabel);
        Assert.Null(series.Stroke);
        Assert.Null(series.StrokeDashArray);
        Assert.True(series.IsStacking);
        Assert.Null(series.TopValues);
		Assert.False(series.ListenPropertyChange);
	}

    [Fact]
    public void StackingColumn100SeriesDefaultTests_Part2()
    {
        var series = new StackingColumn100Series();
        Assert.Null(series.BottomValues);
        Assert.Equal(1d, series.StrokeWidth);
        Assert.Null(series.YBindingPath);
        Assert.Null(series.ActualXAxis);
        Assert.Null(series.ActualYAxis);
        Assert.Equal(string.Empty, series.Label);
        Assert.True(series.ShowTrackballLabel);
    }

    [Fact]
    public void StackingColumn100SeriesDefaultTests_Part3()
    {
        var series = new StackingColumn100Series();
        Assert.Null(series.TrackballLabelTemplate);
        Assert.Null(series.XAxisName);
        Assert.Null(series.YAxisName);
        Assert.True(series.IsVisible);
        Assert.True(series.IsVisibleOnLegend);
        Assert.Null(series.ItemsSource);
        Assert.Equal(LabelContext.YValue, series.LabelContext);
        Assert.Null(series.LabelTemplate);
    }

    [Fact]
    public void StackingColumn100SeriesDefaultTests_Part4()
    {
        var series = new StackingColumn100Series();
        Assert.Equal(Chart.ChartLegendIconType.Circle, series.LegendIcon);
        Assert.Null(series.PaletteBrushes);
        Assert.False(series.ShowDataLabels);
        Assert.Null(series.TooltipTemplate);
        Assert.False(series.EnableTooltip);
        Assert.Null(series.Fill);
        Assert.Null(series.XBindingPath);
        Assert.Null(series.SelectionBehavior);
    }

	    [Fact]
    public void StackingColumn100SeriesDefaultTests_Part5()
    {
        var series = new StackingColumn100Series();
        Assert.Equal(1d, series.Opacity);
        Assert.False(series.EnableAnimation);
        Assert.True(series.IsSideBySide);
        Assert.NotNull(series.YValues);
        Assert.Empty(series.YValues);
        Assert.Equal(series.DataLabelSettings, series.ChartDataLabelSettings);
		Assert.True(series.SbsInfo.IsEmpty);
		Assert.Null(series.ChartArea);
    }

    [Fact]
    public void StackingColumn100SeriesDefaultTests_Part6()
    {
        var series = new StackingColumn100Series();
		series.ActualXAxis = new CategoryAxis();
		Assert.Equal(series.ActualXAxis is CategoryAxis, series.IsIndexed);
        
        if (series.ActualXAxis is CategoryAxis category)
        {
            Assert.Equal(category.ArrangeByIndex, series.IsIndexed);
        }

        Assert.Equal(0, series.SideBySideIndex);
        Assert.False(series.IsSbsValueCalculated);
    }

    [Fact]
    public void StackingColumn100SeriesDefaultTests_Part7()
    {
        var series = new StackingColumn100Series();
        Assert.Null(series.Chart);
        Assert.Equal(1d, series.AnimationDuration);
        Assert.Equal(1, series.AnimationValue);
        Assert.Null(series.SeriesAnimation);
        Assert.False(series.NeedToAnimateSeries);
        Assert.False(series.NeedToAnimateDataLabel);
        Assert.False(series.SegmentsCreated);
		Assert.False(series.VisibleXRange.IsEmpty);
	}

    [Fact]
    public void StackingColumn100SeriesDefaultTests_Part8()
    {
        var series = new StackingColumn100Series();
		Assert.False(series.PreviousXRange.IsEmpty);
		Assert.False(series.VisibleYRange.IsEmpty);
		Assert.Null(series.OldSegments);
        Assert.Equal(0, series.TooltipDataPointIndex);
        Assert.Empty(series.GroupedXValuesIndexes);
        Assert.Empty(series.GroupedActualData);
        Assert.Empty(series.GroupedXValues);
        Assert.NotNull(series.DataLabels);
    }

    [Fact]
    public void StackingColumn100SeriesDefaultTests_Part9()
    {
        var series = new StackingColumn100Series();
        Assert.NotNull(series.LabelTemplateView);
        Assert.False(series.IsMultipleYPathRequired);
        Assert.True(series.IsSideBySide);
        Assert.Equal(0, series.XData);
        Assert.Equal(0, series.PointsCount);
        Assert.Equal(ChartValueType.Double, series.XValueType);
        Assert.Null(series.XValues);
        Assert.Null(series.YComplexPaths);
    }

    [Fact]
    public void StackingColumn100SeriesDefaultTests_Part10()
    {
        var series = new StackingColumn100Series();
        Assert.Null(series.ActualXValues);
        Assert.Null(series.SeriesYValues);
        Assert.Null(series.ActualSeriesYValues);
        Assert.Null(series.YPaths);
        Assert.Null(series.ActualData);
        Assert.Null(series.XComplexPaths);
        Assert.True(series.IsLinearData);
        Assert.False(series.IsDataPointAddedDynamically);
    }

	[Fact]
	public void StackingColumn100SeriesDefaultTests_Part11()
	{
		var series = new StackingColumn100Series();

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
    public void StackingAreaSeriesDefaultTests_Part1()
    {
        var series = new StackingAreaSeries();
        Assert.False(series.ShowMarkers);
        Assert.NotNull(series.MarkerSettings);
        Assert.IsType<ChartMarkerSettings>(series.MarkerSettings);
        Assert.Equal(string.Empty, series.GroupingLabel);
        Assert.Null(series.Stroke);
        Assert.Null(series.StrokeDashArray);
        Assert.True(series.IsStacking);
        Assert.Null(series.TopValues);
		Assert.False(series.ListenPropertyChange);
	}

    [Fact]
    public void StackingAreaSeriesDefaultTests_Part2()
    {
        var series = new StackingAreaSeries();
        Assert.Null(series.BottomValues);
        Assert.Equal(1d, series.StrokeWidth);
        Assert.Null(series.YBindingPath);
        Assert.Null(series.ActualXAxis);
        Assert.Null(series.ActualYAxis);
        Assert.Equal(string.Empty, series.Label);
        Assert.True(series.ShowTrackballLabel);
    }

    [Fact]
    public void StackingAreaSeriesDefaultTests_Part3()
    {
        var series = new StackingAreaSeries();
        Assert.Null(series.TrackballLabelTemplate);
        Assert.Null(series.XAxisName);
        Assert.Null(series.YAxisName);
        Assert.True(series.IsVisible);
        Assert.True(series.IsVisibleOnLegend);
        Assert.Null(series.ItemsSource);
        Assert.Equal(LabelContext.YValue, series.LabelContext);
        Assert.Null(series.LabelTemplate);
    }

    [Fact]
    public void StackingAreaSeriesDefaultTests_Part4()
    {
        var series = new StackingAreaSeries();
        Assert.Equal(Chart.ChartLegendIconType.Circle, series.LegendIcon);
        Assert.Null(series.PaletteBrushes);
        Assert.False(series.ShowDataLabels);
        Assert.Null(series.TooltipTemplate);
        Assert.False(series.EnableTooltip);
        Assert.Null(series.Fill);
        Assert.Null(series.XBindingPath);
        Assert.Null(series.SelectionBehavior);
    }

    [Fact]
    public void StackingAreaSeriesDefaultTests_Part5()
    {
        var series = new StackingAreaSeries();
        Assert.Equal(1d, series.Opacity);
        Assert.False(series.EnableAnimation);
        Assert.False(series.IsSideBySide);
        Assert.NotNull(series.YValues);
        Assert.Empty(series.YValues);
        Assert.Equal(series.DataLabelSettings, series.ChartDataLabelSettings);
		Assert.True(series.SbsInfo.IsEmpty);
		Assert.Null(series.ChartArea);
    }

    [Fact]
    public void StackingAreaSeriesDefaultTests_Part6()
    {
        var series = new StackingAreaSeries();
		series.ActualXAxis = new CategoryAxis();
		Assert.Equal(series.ActualXAxis is CategoryAxis, series.IsIndexed);
        
        if (series.ActualXAxis is CategoryAxis category)
        {
            Assert.Equal(category.ArrangeByIndex, series.IsIndexed);
        }

        Assert.Equal(0, series.SideBySideIndex);
        Assert.False(series.IsSbsValueCalculated);
    }

    [Fact]
    public void StackingAreaSeriesDefaultTests_Part7()
    {
        var series = new StackingAreaSeries();
        Assert.Null(series.Chart);
        Assert.Equal(1d, series.AnimationDuration);
        Assert.Equal(1, series.AnimationValue);
        Assert.Null(series.SeriesAnimation);
        Assert.False(series.NeedToAnimateSeries);
        Assert.False(series.NeedToAnimateDataLabel);
        Assert.False(series.SegmentsCreated);
		Assert.False(series.VisibleXRange.IsEmpty);
	}

    [Fact]
    public void StackingAreaSeriesDefaultTests_Part8()
    {
        var series = new StackingAreaSeries();
        Assert.Null(series.OldSegments);
		Assert.False(series.PreviousXRange.IsEmpty);
		Assert.False(series.VisibleYRange.IsEmpty);
		Assert.Equal(0, series.TooltipDataPointIndex);
        Assert.Empty(series.GroupedXValuesIndexes);
        Assert.Empty(series.GroupedActualData);
        Assert.Empty(series.GroupedXValues);
        Assert.NotNull(series.DataLabels);
    }

    [Fact]
    public void StackingAreaSeriesDefaultTests_Part9()
    {
        var series = new StackingAreaSeries();
        Assert.NotNull(series.LabelTemplateView);
        Assert.False(series.IsMultipleYPathRequired);
        Assert.False(series.IsSideBySide);
        Assert.Equal(0, series.XData);
        Assert.Equal(0, series.PointsCount);
        Assert.Equal(ChartValueType.Double, series.XValueType);
        Assert.Null(series.XValues);
        Assert.Null(series.YComplexPaths);
    }

    [Fact]
    public void StackingAreaSeriesDefaultTests_Part10()
    {
        var series = new StackingAreaSeries();
        Assert.Null(series.ActualXValues);
        Assert.Null(series.SeriesYValues);
        Assert.Null(series.ActualSeriesYValues);
        Assert.Null(series.YPaths);
        Assert.Null(series.ActualData);
        Assert.Null(series.XComplexPaths);
        Assert.True(series.IsLinearData);
        Assert.False(series.IsDataPointAddedDynamically);
    }

	[Fact]
	public void StackingAreaSeriesDefaultTests_Part11()
	{
		var series = new StackingAreaSeries();

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
    public void StackingArea100SeriesDefaultTests_Part1()
    {
        var series = new StackingArea100Series();
        Assert.False(series.ShowMarkers);
        Assert.NotNull(series.MarkerSettings);
        Assert.IsType<ChartMarkerSettings>(series.MarkerSettings);
        Assert.Equal(string.Empty, series.GroupingLabel);
        Assert.Null(series.Stroke);
        Assert.Null(series.StrokeDashArray);
        Assert.True(series.IsStacking);
        Assert.Null(series.TopValues);
		Assert.False(series.ListenPropertyChange);
	}

    [Fact]
    public void StackingArea100SeriesDefaultTests_Part2()
    {
        var series = new StackingArea100Series();
        Assert.Null(series.BottomValues);
        Assert.Equal(1d, series.StrokeWidth);
        Assert.Null(series.YBindingPath);
        Assert.Null(series.ActualXAxis);
        Assert.Null(series.ActualYAxis);
        Assert.Equal(string.Empty, series.Label);
        Assert.True(series.ShowTrackballLabel);
    }

    [Fact]
    public void StackingArea100SeriesDefaultTests_Part3()
    {
        var series = new StackingArea100Series();
        Assert.Null(series.TrackballLabelTemplate);
        Assert.Null(series.XAxisName);
        Assert.Null(series.YAxisName);
        Assert.True(series.IsVisible);
        Assert.True(series.IsVisibleOnLegend);
        Assert.Null(series.ItemsSource);
        Assert.Equal(LabelContext.YValue, series.LabelContext);
        Assert.Null(series.LabelTemplate);
    }

    [Fact]
    public void StackingArea100SeriesDefaultTests_Part4()
    {
        var series = new StackingArea100Series();
        Assert.Equal(Chart.ChartLegendIconType.Circle, series.LegendIcon);
        Assert.Null(series.PaletteBrushes);
        Assert.False(series.ShowDataLabels);
        Assert.Null(series.TooltipTemplate);
        Assert.False(series.EnableTooltip);
        Assert.Null(series.Fill);
        Assert.Null(series.XBindingPath);
        Assert.Null(series.SelectionBehavior);
    }

    [Fact]
    public void StackingArea100SeriesDefaultTests_Part5()
    {
        var series = new StackingArea100Series();
        Assert.Equal(1d, series.Opacity);
        Assert.False(series.EnableAnimation);
        Assert.False(series.IsSideBySide);
        Assert.NotNull(series.YValues);
        Assert.Empty(series.YValues);
        Assert.Equal(series.DataLabelSettings, series.ChartDataLabelSettings);
		Assert.True(series.SbsInfo.IsEmpty);
		Assert.Null(series.ChartArea);
    }

    [Fact]
    public void StackingArea100SeriesDefaultTests_Part6()
    {
        var series = new StackingArea100Series();
		series.ActualXAxis = new CategoryAxis();
		Assert.Equal(series.ActualXAxis is CategoryAxis, series.IsIndexed);
        
        if (series.ActualXAxis is CategoryAxis category)
        {
            Assert.Equal(category.ArrangeByIndex, series.IsIndexed);
        }

        Assert.Equal(0, series.SideBySideIndex);
        Assert.False(series.IsSbsValueCalculated);
    }

    [Fact]
    public void StackingArea100SeriesDefaultTests_Part7()
    {
        var series = new StackingArea100Series();
        Assert.Null(series.Chart);
        Assert.Equal(1d, series.AnimationDuration);
        Assert.Equal(1, series.AnimationValue);
        Assert.Null(series.SeriesAnimation);
        Assert.False(series.NeedToAnimateSeries);
        Assert.False(series.NeedToAnimateDataLabel);
        Assert.False(series.SegmentsCreated);
		Assert.False(series.VisibleXRange.IsEmpty);
	}

    [Fact]
    public void StackingArea100SeriesDefaultTests_Part8()
    {
        var series = new StackingArea100Series();
        Assert.Null(series.OldSegments);
		Assert.False(series.VisibleYRange.IsEmpty);
		Assert.False(series.PreviousXRange.IsEmpty);
		Assert.Equal(0, series.TooltipDataPointIndex);
        Assert.Empty(series.GroupedXValuesIndexes);
        Assert.Empty(series.GroupedActualData);
        Assert.Empty(series.GroupedXValues);
        Assert.NotNull(series.DataLabels);
    }

	[Fact]
    public void StackingArea100SeriesDefaultTests_Part9()
    {
        var series = new StackingArea100Series();
        Assert.NotNull(series.LabelTemplateView);
        Assert.False(series.IsMultipleYPathRequired);
        Assert.False(series.IsSideBySide);
        Assert.Equal(0, series.XData);
        Assert.Equal(0, series.PointsCount);
        Assert.Equal(ChartValueType.Double, series.XValueType);
        Assert.Null(series.XValues);
        Assert.Null(series.YComplexPaths);
    }

    [Fact]
    public void StackingArea100SeriesDefaultTests_Part10()
    {
        var series = new StackingArea100Series();
        Assert.Null(series.ActualXValues);
        Assert.Null(series.SeriesYValues);
        Assert.Null(series.ActualSeriesYValues);
        Assert.Null(series.YPaths);
        Assert.Null(series.ActualData);
        Assert.Null(series.XComplexPaths);
        Assert.True(series.IsLinearData);
        Assert.False(series.IsDataPointAddedDynamically);
    }

	[Fact]
	public void StackingArea100SeriesDefaultTests_Part11()
	{
		var series = new StackingArea100Series();

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
	public void SplineSeriesDefaultTests_Part1()
	{
		var series = new SplineSeries();

		// Test SplineSeries specific properties
		Assert.Equal(SplineType.Natural, series.Type);
		Assert.Null(series.StrokeDashArray);
		Assert.False(series.ShowMarkers);
		Assert.NotNull(series.MarkerSettings);
		Assert.IsType<ChartMarkerSettings>(series.MarkerSettings);

		// Test inherited properties from XYDataSeries
		Assert.Equal(1d, series.StrokeWidth);
		Assert.Null(series.YBindingPath);
		Assert.False(series.ListenPropertyChange);
	}

	[Fact]
	public void SplineSeriesDefaultTests_Part2()
	{
		var series = new SplineSeries();

		// Test inherited properties from CartesianSeries
		Assert.Null(series.ActualXAxis);
		Assert.Null(series.ActualYAxis);
		Assert.Equal(string.Empty, series.Label);
		Assert.True(series.ShowTrackballLabel);
		Assert.Null(series.TrackballLabelTemplate);
		Assert.Null(series.XAxisName);
		Assert.Null(series.YAxisName);
	}

	[Fact]
	public void SplineSeriesDefaultTests_Part3()
	{
		var series = new SplineSeries();

		// Test inherited properties from ChartSeries
		Assert.True(series.IsVisible);
		Assert.True(series.IsVisibleOnLegend);	
		Assert.Null(series.ItemsSource);
		Assert.Equal(LabelContext.YValue, series.LabelContext);
		Assert.Null(series.LabelTemplate);
		Assert.Equal(Chart.ChartLegendIconType.Circle, series.LegendIcon);
		Assert.Null(series.PaletteBrushes);
		Assert.False(series.ShowDataLabels);
		Assert.Null(series.TooltipTemplate);
	}

	[Fact]
	public void SplineSeriesDefaultTests_Part4()
	{
		var series = new SplineSeries();

		Assert.False(series.EnableTooltip);
		Assert.Null(series.Fill);
		Assert.Null(series.XBindingPath);
		Assert.Null(series.SelectionBehavior);
		Assert.Equal(1d, series.Opacity);
		Assert.False(series.EnableAnimation);
		Assert.False(series.IsSideBySide);
		Assert.NotNull(series.YValues);
		Assert.Empty(series.YValues);
	}

	[Fact]
	public void SplineSeriesDefaultTests_Part5()
	{
		var series = new SplineSeries();
		series.ActualXAxis = new CategoryAxis();
		Assert.Equal(series.DataLabelSettings, series.ChartDataLabelSettings);
		Assert.True(series.SbsInfo.IsEmpty);
		Assert.Null(series.ChartArea);
		Assert.Equal(series.ActualXAxis is CategoryAxis, series.IsIndexed);

		if (series.ActualXAxis is CategoryAxis category)
		{
			Assert.Equal(category.ArrangeByIndex, series.IsIndexed);
		}

		Assert.Equal(0, series.SideBySideIndex);
		Assert.False(series.IsSbsValueCalculated);
	}

	[Fact]
	public void SplineSeriesDefaultTests_Part6()
	{
		var series = new SplineSeries();

		Assert.Null(series.Chart);
		Assert.Equal(1d, series.AnimationDuration);
		Assert.Equal(1, series.AnimationValue);
		Assert.Null(series.SeriesAnimation);
		Assert.False(series.NeedToAnimateSeries);
		Assert.False(series.NeedToAnimateDataLabel);
		Assert.False(series.SegmentsCreated);
		Assert.False(series.VisibleXRange.IsEmpty);
		Assert.False(series.VisibleYRange.IsEmpty);
	}

	[Fact]
	public void SplineSeriesDefaultTests_Part7()
	{
		var series = new SplineSeries();

		Assert.Null(series.OldSegments);
		Assert.False(series.PreviousXRange.IsEmpty);
		Assert.Equal(0, series.TooltipDataPointIndex);
		Assert.Empty(series.GroupedXValuesIndexes);
		Assert.Empty(series.GroupedActualData);
		Assert.Empty(series.GroupedXValues);
		Assert.NotNull(series.DataLabels);
		Assert.NotNull(series.LabelTemplateView);
		Assert.False(series.IsMultipleYPathRequired);
	}

	[Fact]
	public void SplineSeriesDefaultTests_Part8()
	{
		var series = new SplineSeries();

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
	public void SplineSeriesDefaultTests_Part9()
	{
		var series = new SplineSeries();

		Assert.Null(series.YPaths);
		Assert.Null(series.ActualData);
		Assert.Null(series.XComplexPaths);
		Assert.True(series.IsLinearData);
		Assert.False(series.IsDataPointAddedDynamically);
		// Additional assertions
		Assert.False(series.IsStacking);
		Assert.False(series.IsIndividualSegment());
	}

	[Fact]
	public void SplineSeriesDefaultTests_Part10()
	{
		var series = new SplineSeries();

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
	public void SplineRangeAreaSeriesDefaultTests_Part1()
	{
		var series = new SplineRangeAreaSeries();

		// Test SplineRangeAreaSeries specific properties
		Assert.False(series.ShowMarkers);
		Assert.NotNull(series.MarkerSettings);
		Assert.IsType<ChartMarkerSettings>(series.MarkerSettings);
		Assert.Equal(SplineType.Natural, series.Type);
		// Test inherited properties from RangeSeriesBase
		Assert.Equal(string.Empty, series.High);
		Assert.Equal(string.Empty, series.Low);
		Assert.False(series.ListenPropertyChange);
	}

	[Fact]
	public void SplineRangeAreaSeriesDefaultTests_Part2()
	{
		var series = new SplineRangeAreaSeries();

		// Test inherited properties from CartesianSeries
		Assert.Null(series.ActualXAxis);
		Assert.Null(series.ActualYAxis);
		Assert.Equal(string.Empty, series.Label);
		Assert.True(series.ShowTrackballLabel);
		Assert.Null(series.TrackballLabelTemplate);
		Assert.Null(series.XAxisName);
		Assert.Null(series.YAxisName);
	}

	[Fact]
	public void SplineRangeAreaSeriesDefaultTests_Part3()
	{
		var series = new SplineRangeAreaSeries();

		// Test inherited properties from ChartSeries
		Assert.True(series.IsVisible);
		Assert.True(series.IsVisibleOnLegend);
		Assert.Null(series.ItemsSource);
		Assert.Equal(LabelContext.YValue, series.LabelContext);
		Assert.Null(series.LabelTemplate);
		Assert.Equal(Chart.ChartLegendIconType.Circle, series.LegendIcon);
		Assert.Null(series.PaletteBrushes);
		Assert.False(series.ShowDataLabels);
		Assert.Null(series.TooltipTemplate);
	}

	[Fact]
	public void SplineRangeAreaSeriesDefaultTests_Part4()
	{
		var series = new SplineRangeAreaSeries();

		Assert.False(series.EnableTooltip);
		Assert.Null(series.Fill);
		Assert.Null(series.XBindingPath);
		Assert.Null(series.SelectionBehavior);
		Assert.Equal(1d, series.Opacity);
		Assert.False(series.EnableAnimation);
		Assert.False(series.IsSideBySide);
		Assert.NotNull(series.HighValues);
		Assert.Empty(series.HighValues);
		Assert.NotNull(series.LowValues);
		Assert.Empty(series.LowValues);
	}

	[Fact]
	public void SplineRangeAreaSeriesDefaultTests_Part5()
	{
		var series = new SplineRangeAreaSeries();
		series.ActualXAxis = new CategoryAxis();
		Assert.Equal(series.DataLabelSettings, series.ChartDataLabelSettings);
		Assert.True(series.SbsInfo.IsEmpty);
		Assert.Null(series.ChartArea);
		Assert.Equal(series.ActualXAxis is CategoryAxis, series.IsIndexed);

		if (series.ActualXAxis is CategoryAxis category)
		{
			Assert.Equal(category.ArrangeByIndex, series.IsIndexed);
		}

		Assert.Equal(0, series.SideBySideIndex);
		Assert.False(series.IsSbsValueCalculated);
	}

	[Fact]
	public void SplineRangeAreaSeriesDefaultTests_Part6()
	{
		var series = new SplineRangeAreaSeries();

		Assert.Null(series.Chart);
		Assert.Equal(1d, series.AnimationDuration);
		Assert.Equal(1, series.AnimationValue);
		Assert.Null(series.SeriesAnimation);
		Assert.False(series.NeedToAnimateSeries);
		Assert.False(series.NeedToAnimateDataLabel);
		Assert.False(series.SegmentsCreated);
		Assert.False(series.VisibleXRange.IsEmpty);
		Assert.False(series.VisibleYRange.IsEmpty);
	}

	[Fact]
	public void SplineRangeAreaSeriesDefaultTests_Part7()
	{
		var series = new SplineRangeAreaSeries();

		Assert.Null(series.OldSegments);
		Assert.False(series.PreviousXRange.IsEmpty);
		Assert.Equal(0, series.TooltipDataPointIndex);
		Assert.Empty(series.GroupedXValuesIndexes);
		Assert.Empty(series.GroupedActualData);
		Assert.Empty(series.GroupedXValues);
		Assert.NotNull(series.DataLabels);
		Assert.NotNull(series.LabelTemplateView);
		Assert.False(series.IsMultipleYPathRequired);
	}

	[Fact]
	public void SplineRangeAreaSeriesDefaultTests_Part8()
	{
		var series = new SplineRangeAreaSeries();

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
	public void SplineRangeAreaSeriesDefaultTests_Part9()
	{
		var series = new SplineRangeAreaSeries();

		Assert.Null(series.YPaths);
		Assert.Null(series.ActualData);
		Assert.Null(series.XComplexPaths);
		Assert.True(series.IsLinearData);
		Assert.False(series.IsDataPointAddedDynamically);
		// Additional assertions
		Assert.False(series.IsStacking);
		Assert.Null(series.Stroke);
	}

	[Fact]
	public void SplineRangeAreaSeriesDefaultTests_Part10()
	{
		var series = new SplineRangeAreaSeries();

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
	public void SplineAreaSeriesDefaultTests_Part1()
	{
		var series = new SplineAreaSeries();

		// Test SplineAreaSeries specific properties
		Assert.Equal(SplineType.Natural, series.Type);
		// Test inherited properties from AreaSeries
		Assert.Null(series.Fill);
		Assert.Null(series.Stroke);
		Assert.False(series.ShowMarkers);
		Assert.NotNull(series.MarkerSettings);
		Assert.IsType<ChartMarkerSettings>(series.MarkerSettings);
		Assert.Null(series.StrokeDashArray);
		// Test inherited properties from XYDataSeries
		Assert.Equal(1d, series.StrokeWidth);
		Assert.Null(series.YBindingPath);
		Assert.False(series.ListenPropertyChange);
	}

	[Fact]
	public void SplineAreaSeriesDefaultTests_Part2()
	{
		var series = new SplineAreaSeries();

		// Test inherited properties from CartesianSeries
		Assert.Null(series.ActualXAxis);
		Assert.Null(series.ActualYAxis);
		Assert.Equal(string.Empty, series.Label);
		Assert.True(series.ShowTrackballLabel);
		Assert.Null(series.TrackballLabelTemplate);
		Assert.Null(series.XAxisName);
		Assert.Null(series.YAxisName);
	}

	[Fact]
	public void SplineAreaSeriesDefaultTests_Part3()
	{
		var series = new SplineAreaSeries();

		// Test inherited properties from ChartSeries
		Assert.True(series.IsVisible);
		Assert.True(series.IsVisibleOnLegend);
		Assert.Null(series.ItemsSource);
		Assert.Equal(LabelContext.YValue, series.LabelContext);
		Assert.Null(series.LabelTemplate);
		Assert.Equal(Chart.ChartLegendIconType.Circle, series.LegendIcon);
		Assert.Null(series.PaletteBrushes);
		Assert.False(series.ShowDataLabels);
		Assert.Null(series.TooltipTemplate);
	}

	[Fact]
	public void SplineAreaSeriesDefaultTests_Part4()
	{
		var series = new SplineAreaSeries();

		Assert.False(series.EnableTooltip);
		Assert.Null(series.XBindingPath);
		Assert.Null(series.SelectionBehavior);
		Assert.Equal(1d, series.Opacity);
		Assert.False(series.EnableAnimation);
		Assert.False(series.IsSideBySide);
		Assert.NotNull(series.YValues);
		Assert.Empty(series.YValues);
	}

	[Fact]
	public void SplineAreaSeriesDefaultTests_Part5()
	{
		var series = new SplineAreaSeries();
		series.ActualXAxis = new CategoryAxis();
		Assert.Equal(series.DataLabelSettings, series.ChartDataLabelSettings);
		Assert.True(series.SbsInfo.IsEmpty);
		Assert.Null(series.ChartArea);
		Assert.Equal(series.ActualXAxis is CategoryAxis, series.IsIndexed);

		if (series.ActualXAxis is CategoryAxis category)
		{
			Assert.Equal(category.ArrangeByIndex, series.IsIndexed);
		}

		Assert.Equal(0, series.SideBySideIndex);
		Assert.False(series.IsSbsValueCalculated);
	}

	[Fact]
	public void SplineAreaSeriesDefaultTests_Part6()
	{
		var series = new SplineAreaSeries();

		Assert.Null(series.Chart);
		Assert.Equal(1d, series.AnimationDuration);
		Assert.Equal(1, series.AnimationValue);
		Assert.Null(series.SeriesAnimation);
		Assert.False(series.NeedToAnimateSeries);
		Assert.False(series.NeedToAnimateDataLabel);
		Assert.False(series.SegmentsCreated);
		Assert.False(series.VisibleXRange.IsEmpty);
		Assert.False(series.VisibleYRange.IsEmpty);
	}

	[Fact]
	public void SplineAreaSeriesDefaultTests_Part7()
	{
		var series = new SplineAreaSeries();

		Assert.Null(series.OldSegments);
		Assert.False(series.PreviousXRange.IsEmpty);
		Assert.Equal(0, series.TooltipDataPointIndex);
		Assert.Empty(series.GroupedXValuesIndexes);
		Assert.Empty(series.GroupedActualData);
		Assert.Empty(series.GroupedXValues);
		Assert.NotNull(series.DataLabels);
		Assert.NotNull(series.LabelTemplateView);
		Assert.False(series.IsMultipleYPathRequired);
	}

	[Fact]
	public void SplineAreaSeriesDefaultTests_Part8()
	{
		var series = new SplineAreaSeries();

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
	public void SplineAreaSeriesDefaultTests_Part9()
	{
		var series = new SplineAreaSeries();

		Assert.Null(series.YPaths);
		Assert.Null(series.ActualData);
		Assert.Null(series.XComplexPaths);
		Assert.True(series.IsLinearData);
		Assert.False(series.IsDataPointAddedDynamically);
		Assert.False(series.IsStacking);
		Assert.False(series.IsIndividualSegment());
	}

	[Fact]
	public void SplineAreaSeriesDefaultTests_Part10()
	{
		var series = new SplineAreaSeries();

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
	public void ScatterSeriesDefaultTests_Part1()
	{
		var series = new ScatterSeries();

		// Test ScatterSeries specific properties
		Assert.Equal(5d, series.PointHeight);
		Assert.Equal(5d, series.PointWidth);
		Assert.Null(series.Stroke);
		Assert.Equal(Chart.ShapeType.Circle, series.Type);
		// Test inherited properties from XYDataSeries
		Assert.Equal(1d, series.StrokeWidth);
		Assert.Null(series.YBindingPath);
		Assert.False(series.ListenPropertyChange);
	}

	[Fact]
	public void ScatterSeriesDefaultTests_Part2()
	{
		var series = new ScatterSeries();

		// Test inherited properties from CartesianSeries
		Assert.Null(series.ActualXAxis);
		Assert.Null(series.ActualYAxis);
		Assert.Equal(string.Empty, series.Label);
		Assert.True(series.ShowTrackballLabel);
		Assert.Null(series.TrackballLabelTemplate);
		Assert.Null(series.XAxisName);
		Assert.Null(series.YAxisName);
	}

	[Fact]
	public void ScatterSeriesDefaultTests_Part3()
	{
		var series = new ScatterSeries();

		// Test inherited properties from ChartSeries
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
	public void ScatterSeriesDefaultTests_Part4()
	{
		var series = new ScatterSeries();

		Assert.False(series.EnableTooltip);
		Assert.Null(series.Fill);
		Assert.Null(series.XBindingPath);
		Assert.Null(series.SelectionBehavior);
		Assert.Equal(1d, series.Opacity);
		Assert.False(series.EnableAnimation);
	}

	[Fact]
	public void ScatterSeriesDefaultTests_Part5()
	{
		var series = new ScatterSeries();

		// Test internal properties
		Assert.Null(series.Chart);
		Assert.Equal(1d, series.AnimationDuration);
		Assert.Equal(1, series.AnimationValue);
		Assert.Null(series.SeriesAnimation);
		Assert.False(series.NeedToAnimateSeries);
		Assert.False(series.NeedToAnimateDataLabel);
		Assert.False(series.SegmentsCreated);
	}

	[Fact]
	public void ScatterSeriesDefaultTests_Part6()
	{
		var series = new ScatterSeries();

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
	public void ScatterSeriesDefaultTests_Part7()
	{
		var series = new ScatterSeries();

		Assert.NotNull(series.DataLabels);
		Assert.Empty(series.DataLabels);
		Assert.NotNull(series.LabelTemplateView);
		Assert.False(series.IsMultipleYPathRequired);
		Assert.False(series.IsSideBySide);
		Assert.Equal(0, series.XData);
		Assert.Equal(0, series.PointsCount);
	}

	[Fact]
	public void ScatterSeriesDefaultTests_Part8()
	{
		var series = new ScatterSeries();

		Assert.Equal(ChartValueType.Double, series.XValueType);
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
	public void ScatterSeriesDefaultTests_Part9()
	{
		var series = new ScatterSeries();

		Assert.True(series.IsLinearData);
		Assert.False(series.IsDataPointAddedDynamically);
		Assert.False(series.IsStacking);
		Assert.True(series.XRange.IsEmpty);
		Assert.True(series.YRange.IsEmpty);
	}

	[Fact]
	public void ScatterSeriesDefaultTests_Part10()
	{
		var series = new ScatterSeries();

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
}
