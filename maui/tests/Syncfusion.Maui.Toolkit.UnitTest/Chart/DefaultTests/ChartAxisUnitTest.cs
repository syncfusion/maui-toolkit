using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.Toolkit.UnitTest.Charts
{
	public class ChartAxisUnitTest
	{

		[Fact]
		public void CategoryAxis_InitializesBaseProperties()
		{
			var axis = new CategoryAxis(); 
			Assert.IsType<CategoryAxis>(axis);
			Assert.IsAssignableFrom<ChartAxis>(axis);
			Assert.Equal(LabelPlacement.OnTicks, axis.LabelPlacement);
			Assert.True(axis.ArrangeByIndex);
			Assert.True(axis.IsVisible);
			Assert.Equal(0d, axis.AxisLineOffset);
			Assert.Equal(0d, axis.LabelRotation);
			Assert.NotNull(axis.LabelStyle);
			Assert.IsType<ChartAxisLabelStyle>(axis.LabelStyle);
		}

		[Fact]
		public void CategoryAxis_InitializesLineStyles()
		{
			var axis = new CategoryAxis(); 
			Assert.NotNull(axis.AxisLineStyle);
			Assert.IsType<ChartLineStyle>(axis.AxisLineStyle);
			Assert.NotNull(axis.MajorGridLineStyle);
			Assert.IsType<ChartLineStyle>(axis.MajorGridLineStyle);
			Assert.NotNull(axis.MajorTickStyle);
			Assert.IsType<ChartAxisTickStyle>(axis.MajorTickStyle);
			Assert.Equal(Color.FromArgb("#E7E0EC"), axis.MajorGridLineStroke.ToColor());
			Assert.True(axis.ShowMajorGridLines);
		}

		[Fact]
		public void CategoryAxis_InitializesCrossingProperties()
		{
			var axis = new CategoryAxis(); 
			Assert.Equal(double.NaN, axis.CrossesAt);
			Assert.True(axis.RenderNextToCrossingValue);
			Assert.Null(axis.CrossAxisName);
			Assert.Equal(double.NaN, axis.ActualCrossingValue);
			Assert.Null(axis.Title);
			Assert.False(axis.IsInversed);
			Assert.Equal(EdgeLabelsDrawingMode.Shift, axis.EdgeLabelsDrawingMode);
			Assert.Equal(AxisLabelsIntersectAction.Hide, axis.LabelsIntersectAction);
		}

		[Fact]
		public void CategoryAxis_InitializesZoomProperties()
		{
			var axis = new CategoryAxis();
			Assert.NotNull(axis.VisibleLabels);
			Assert.Empty(axis.VisibleLabels);
			Assert.Equal(0d, axis.LabelExtent);
			Assert.Equal(AxisElementPosition.Outside, axis.LabelsPosition);
			Assert.Equal(0d, axis.ZoomPosition);
			Assert.Equal(1d, axis.ZoomFactor);
			Assert.True(axis.EnableAutoIntervalOnZooming);
			Assert.Equal(ChartAutoScrollingMode.End, axis.AutoScrollingMode);
		}

		[Fact]
		public void CategoryAxis_InitializesTrackballProperties()
		{
			var axis = new CategoryAxis();
			Assert.False(axis.IsScrolling);
			Assert.Null(axis.AxisRenderer);
			Assert.Equal(0, axis.SideBySideSeriesCount);
			Assert.Equal(double.NaN, axis.AutoScrollingDelta);
			Assert.False(axis.ShowTrackballLabel);
			Assert.NotNull(axis.TrackballLabelStyle);
			Assert.IsType<ChartLabelStyle>(axis.TrackballLabelStyle);
			Assert.Equal(SolidColorBrush.Black, axis.TrackballAxisBackground);
			Assert.Equal(12.0, axis.TrackballAxisFontSize);
		}

		[Fact]
		public void CategoryAxis_InitializesPositionProperties()
		{
			var axis = new CategoryAxis(); 
			Assert.Null(axis.AxisLabelsRenderer);
			Assert.Null(axis.AxisElementRenderer);
			Assert.Equal(string.Empty, axis.Name);
			Assert.Equal(AxisElementPosition.Outside, axis.TickPosition);
			Assert.Equal(0, axis.PlotOffsetEnd);
			Assert.Equal(0, axis.PlotOffsetStart);
			Assert.NotNull(axis.TickPositions);
			Assert.Empty(axis.TickPositions);
		}

		[Fact]
		public void CategoryAxis_InitializesSeriesAndPlotBandProperties()
		{
			var axis = new CategoryAxis(); 
			Assert.True(double.IsNaN(axis.Interval));
			Assert.NotNull(axis.RegisteredSeries);
			Assert.Empty(axis.RegisteredSeries);
			Assert.NotNull(axis.ActualPlotBands);
			Assert.Empty(axis.ActualPlotBands);
			Assert.Null(axis.RangeStyles);
			Assert.Equal(3, axis.MaximumLabels);
			Assert.Null(axis.PlotBands);
		}

		[Fact]
		public void CategoryAxis_InitializesLabelStyleProperties()
		{
			var axis = new CategoryAxis();
			var labelStyle = axis.LabelStyle;
			Assert.True(axis.VisibleRange.IsEmpty);
			Assert.NotNull(labelStyle);
			Assert.IsType<ChartAxisLabelStyle>(labelStyle);
			Assert.Equal(12d, labelStyle.FontSize);
			Assert.Equal(FontAttributes.None, labelStyle.FontAttributes);
			Assert.Null(labelStyle.FontFamily);
			Assert.Equal(Color.FromArgb("#49454F"), labelStyle.TextColor);
			Assert.Equal(string.Empty, labelStyle.LabelFormat);
		}

		[Fact]
		public void CategoryAxis_InitializesMissingProperties()
		{
			var axis = new CategoryAxis();
			var labelStyle = axis.LabelStyle;
			Assert.True(double.IsNaN(axis.AxisInterval));
			Assert.True(double.IsNaN(axis.AutoScrollingDelta));
			Assert.Null(axis.CrossAxisName);
			Assert.Equal(double.NaN, axis.VisibleMaximum);
			Assert.Equal(double.NaN, axis.VisibleMinimum);
			Assert.Null(axis.TrackballLabelTemplate);
		}

		[Fact]
		public void CategoryAxis_OffsetAndSizeProperties()
		{
			var axis = new CategoryAxis();
			Assert.Equal(0f, axis.LeftOffset);
			Assert.Equal(0f, axis.TopOffset);
			Assert.Equal(0d, axis.ActualPlotOffset);
			Assert.Equal(0d, axis.ActualPlotOffsetStart);
			Assert.Equal(0d, axis.ActualPlotOffsetEnd);
			Assert.Equal(Size.Zero, axis.ComputedDesiredSize);
			Assert.Equal(0d, axis.InsidePadding);
			Assert.Equal(Size.Zero, axis.AvailableSize);
		}

		[Fact]
		public void CategoryAxis_IntervalAndRangeProperties()
		{
			var axis = new CategoryAxis();
			Assert.Equal(0d, axis.ActualInterval);
			Assert.Equal(0d, axis.ActualRange.Start);
			Assert.Equal(0d, axis.ActualRange.End);
			Assert.Equal(0d, axis.ActualRange.Delta);
			Assert.Equal(0d, axis.ActualRange.Median);
			Assert.Equal(0d, axis.VisibleInterval);
			Assert.False(axis.SmallTickRequired);
			Assert.Equal(default(RectF), axis.RenderedRect);
			Assert.True(double.IsNaN(axis.AxisInterval));
			Assert.Equal(new int[] { 10, 5, 2, 1 }, ChartAxis.IntervalDivs);
			Assert.Equal(Rect.Zero, axis.ArrangeRect);
		}

		[Fact]
		public void CategoryAxis_BooleanProperties()
		{
			var axis = new CategoryAxis();
			Assert.False(axis.IsPolarArea);
			Assert.True(double.IsNaN(axis.ActualAutoScrollDelta));
			Assert.False(axis.CanAutoScroll);
			Assert.False(axis.IsVertical);
			Assert.False(axis._isOverriddenOnCreateLabelsMethod);
			Assert.True(axis.ArrangeByIndex);
			Assert.NotNull(axis.AssociatedAxes);
			Assert.Empty(axis.AssociatedAxes);
		}

		[Fact]
		public void CategoryAxis_NullAndNumericProperties()
		{
			var axis = new CategoryAxis();
			Assert.Null(axis.Area);
			Assert.Null(axis.CartesianArea);
			Assert.Null(axis.PolarArea);
			Assert.Equal(270, axis.PolarStartAngle);
			Assert.Equal(0f, axis.LeftOffset);
			Assert.Equal(0f, axis.TopOffset);
			Assert.Equal(0d, axis.ActualPlotOffset);
			Assert.Equal(0d, axis.ActualPlotOffsetStart);
		}

		[Fact]
		public void DateTimeAxis_DefaultDateTimeProperties()
		{
			var axis = new DateTimeAxis();
			Assert.Equal(DateTimeIntervalType.Auto, axis.IntervalType);
			Assert.Null(axis.Minimum); 
			Assert.IsType<DateTimeAxis>(axis);
			Assert.IsAssignableFrom<RangeAxisBase>(axis);
			Assert.Null(axis.Maximum);
			Assert.True(double.IsNaN(axis.Interval));
			Assert.Equal(DateTimeRangePadding.Auto, axis.RangePadding);
			Assert.Equal(DateTimeIntervalType.Auto, axis.AutoScrollingDeltaType);
			Assert.Equal(DateTimeIntervalType.Auto, axis.ActualIntervalType);
			Assert.Null(axis.DefaultMinimum);
			Assert.Null(axis.DefaultMaximum);
		}

		[Fact]
		public void DateTimeAxis_DefaultAxisProperties()
		{
			var axis = new DateTimeAxis();
			Assert.True(axis.IsVisible);
			Assert.Equal(0d, axis.AxisLineOffset);
			Assert.Equal(0d, axis.LabelRotation);
			Assert.Equal(double.NaN, axis.CrossesAt);
			Assert.True(axis.RenderNextToCrossingValue);
			Assert.Null(axis.CrossAxisName);
			Assert.Null(axis.Title);
			Assert.False(axis.IsInversed);
		}

		[Fact]
		public void DateTimeAxis_DefaultStyleProperties()
		{
			var axis = new DateTimeAxis();
			Assert.NotNull(axis.LabelStyle);
			Assert.IsType<ChartAxisLabelStyle>(axis.LabelStyle);
			Assert.NotNull(axis.AxisLineStyle);
			Assert.IsType<ChartLineStyle>(axis.AxisLineStyle);
			Assert.NotNull(axis.MajorGridLineStyle);
			Assert.IsType<ChartLineStyle>(axis.MajorGridLineStyle);
			Assert.NotNull(axis.MajorTickStyle);
			Assert.IsType<ChartAxisTickStyle>(axis.MajorTickStyle);
			Assert.NotNull(axis.TrackballLabelStyle);
		}

		[Fact]
		public void DateTimeAxis_DefaultProperties()
		{
			var axis = new DateTimeAxis();
			Assert.Null(axis.DefaultMinimum);
			Assert.Null(axis.DefaultMaximum); 
			Assert.Equal(ChartAutoScrollingMode.End, axis.AutoScrollingMode);
			Assert.Equal(double.NaN, axis.AutoScrollingDelta);
			Assert.Equal(EdgeLabelsDrawingMode.Shift, axis.EdgeLabelsDrawingMode);
			Assert.Equal(0d, axis.LabelExtent);
			Assert.Equal(AxisLabelsIntersectAction.Hide, axis.LabelsIntersectAction);
			Assert.Equal(string.Empty, axis.Name);
		}

		[Fact]
		public void DateTimeAxis_DefaultZoomAndScrollProperties()
		{
			var axis = new DateTimeAxis(); 
			Assert.Equal(AxisElementPosition.Outside, axis.TickPosition);
			Assert.Equal(AxisElementPosition.Outside, axis.LabelsPosition);
			Assert.IsType<ChartLabelStyle>(axis.TrackballLabelStyle);
			Assert.True(axis.ShowMajorGridLines);
			Assert.False(axis.ShowTrackballLabel);
			Assert.Equal(0d, axis.ZoomPosition);
			Assert.Equal(1d, axis.ZoomFactor);
			Assert.True(axis.EnableAutoIntervalOnZooming);
		}
		 
		[Fact]
		public void DateTimeAxis_DefaultLabelStyleProperties()
		{
			var axis = new DateTimeAxis();
			var labelStyle = axis.LabelStyle; 
			Assert.True(axis.VisibleRange.IsEmpty);
			Assert.Equal(default(DateTime), axis.ActualMinimum);
			Assert.Equal(default(DateTime), axis.ActualMaximum);
			Assert.Equal(12d, labelStyle.FontSize);
			Assert.Equal(FontAttributes.None, labelStyle.FontAttributes);
			Assert.Null(labelStyle.FontFamily);
			Assert.Equal(Color.FromArgb("#49454F"), labelStyle.TextColor);
			Assert.Equal(string.Empty, labelStyle.LabelFormat);
		}

		[Fact]
		public void DateTimeAxis_DefaultCollectionProperties()
		{
			var axis = new DateTimeAxis();
			Assert.Null(axis.PlotBands);
			Assert.NotNull(axis.VisibleLabels);
			Assert.Empty(axis.VisibleLabels);
			Assert.NotNull(axis.TickPositions);
			Assert.Empty(axis.TickPositions);
			Assert.NotNull(axis.RegisteredSeries);
			Assert.Empty(axis.RegisteredSeries);
			Assert.NotNull(axis.ActualPlotBands);
			Assert.Empty(axis.ActualPlotBands);
		}

		[Fact]
		public void DateTimeAxis_OffsetAndSizeProperties()
		{
			var axis = new DateTimeAxis();
			Assert.Equal(0f, axis.LeftOffset);
			Assert.Equal(0f, axis.TopOffset);
			Assert.Equal(0d, axis.ActualPlotOffset);
			Assert.Equal(0d, axis.ActualPlotOffsetStart);
			Assert.Equal(0d, axis.ActualPlotOffsetEnd);
			Assert.Equal(Size.Zero, axis.ComputedDesiredSize);
			Assert.Equal(0d, axis.InsidePadding);
			Assert.Equal(Size.Zero, axis.AvailableSize);
		}

		[Fact]
		public void DateTimeAxis_IntervalAndRangeProperties()
		{
			var axis = new DateTimeAxis();

			Assert.Equal(0d, axis.ActualRange.Start);
			Assert.Equal(0d, axis.ActualRange.End);
			Assert.Equal(0d, axis.ActualRange.Delta);
			Assert.Equal(0d, axis.ActualRange.Median);
			Assert.Equal(0d, axis.ActualInterval); 
			Assert.Equal(0d, axis.VisibleInterval);
			Assert.False(axis.SmallTickRequired);
			Assert.Equal(default(RectF), axis.RenderedRect);
			Assert.Equal(new int[] { 10, 5, 2, 1 }, ChartAxis.IntervalDivs);
			Assert.Equal(0, axis.MinorTicksPerInterval);
			Assert.Equal(0d, axis.PlotOffsetStart);
		}

		[Fact]
		public void DateTimeAxis_BooleanProperties()
		{
			var axis = new DateTimeAxis();
			Assert.False(axis.IsPolarArea);
			Assert.True(double.IsNaN(axis.ActualAutoScrollDelta));
			Assert.False(axis.CanAutoScroll);
			Assert.False(axis.IsVertical);
			Assert.Equal(Rect.Zero, axis.ArrangeRect);
			Assert.False(axis.IsScrolling);
			Assert.False(axis._isOverriddenOnCreateLabelsMethod);
			Assert.Equal(0d, axis.PlotOffsetEnd); 
			Assert.Equal(270, axis.PolarStartAngle); 
			Assert.Equal(EdgeLabelsVisibilityMode.Default, axis.EdgeLabelsVisibilityMode);
			Assert.Equal(0, axis.MinorTicksPerInterval);
		}

		[Fact]
		public void DateTimeAxis_NullAndCollectionProperties()
		{
			var axis = new DateTimeAxis();
			Assert.NotNull(axis.AssociatedAxes);
			Assert.Empty(axis.AssociatedAxes);
			Assert.Null(axis.Area);
			Assert.Null(axis.CartesianArea);
			Assert.Null(axis.AxisRenderer);
			Assert.Null(axis.RangeStyles);
			Assert.Equal(0, axis.SideBySideSeriesCount);
			Assert.Equal(3, axis.MaximumLabels); 
			Assert.NotNull(axis.MinorGridLineStyle);
			Assert.IsType<ChartLineStyle>(axis.MinorGridLineStyle);
		} 

		[Fact]
		public void DateTimeAxis_RangeAxisBaseProperties()
		{
			var axis = new DateTimeAxis(); 
			Assert.NotNull(axis.MinorTickStyle);
			Assert.IsType<ChartAxisTickStyle>(axis.MinorTickStyle);
			Assert.True(axis.ShowMinorGridLines);
			Assert.NotNull(axis.SmallTickPoints);
			Assert.Empty(axis.SmallTickPoints);
			Assert.NotNull(axis.MinorGridLineStroke);
			Assert.IsType<SolidColorBrush>(axis.MinorGridLineStroke);
			Assert.Equal(Color.FromArgb("#EDEFF1"), (axis.MinorGridLineStroke as SolidColorBrush)!.Color);
			Assert.Equal(0.5f, axis.MinorGridLineStyle.StrokeWidth);
			Assert.Equal(4d, axis.MinorTickStyle.TickSize); 
		}

		[Fact]
		public void NumericalAxis_DefaultNumericProperties()
		{
			var axis = new NumericalAxis(); 
			Assert.IsType<NumericalAxis>(axis);
			Assert.IsAssignableFrom<RangeAxisBase>(axis);
			Assert.Equal(NumericalPadding.Auto, axis.RangePadding);
			Assert.True(double.IsNaN(axis.Interval));
			Assert.Null(axis.Minimum);
			Assert.Null(axis.Maximum);
			Assert.True(double.IsNaN(axis.DefaultMinimum));
			Assert.True(double.IsNaN(axis.DefaultMaximum));
		}

		[Fact]
		public void NumericalAxis_DefaultAxisProperties()
		{
			var axis = new NumericalAxis();
			Assert.True(axis.IsVisible);
			Assert.Equal(0d, axis.AxisLineOffset);
			Assert.Equal(0d, axis.LabelRotation);
			Assert.Equal(double.NaN, axis.CrossesAt);
			Assert.True(axis.RenderNextToCrossingValue);
			Assert.Null(axis.CrossAxisName);
			Assert.Null(axis.Title);
			Assert.False(axis.IsInversed);
			Assert.Equal(EdgeLabelsDrawingMode.Shift, axis.EdgeLabelsDrawingMode);
		}

		[Fact]
		public void NumericalAxis_DefaultStyleProperties()
		{
			var axis = new NumericalAxis();
			Assert.NotNull(axis.LabelStyle);
			Assert.IsType<ChartAxisLabelStyle>(axis.LabelStyle);
			Assert.NotNull(axis.AxisLineStyle);
			Assert.IsType<ChartLineStyle>(axis.AxisLineStyle);
			Assert.NotNull(axis.MajorGridLineStyle);
			Assert.IsType<ChartLineStyle>(axis.MajorGridLineStyle);
			Assert.NotNull(axis.MajorTickStyle);
			Assert.IsType<ChartAxisTickStyle>(axis.MajorTickStyle);
			Assert.NotNull(axis.TrackballLabelStyle);
		}

		[Fact]
		public void NumericalAxis_DefaultZoomAndScrollProperties()
		{
			var axis = new NumericalAxis(); 
			Assert.True(axis.ShowMajorGridLines);
			Assert.False(axis.ShowTrackballLabel);
			Assert.IsType<ChartLabelStyle>(axis.TrackballLabelStyle);
			Assert.Equal(0d, axis.ZoomPosition);
			Assert.Equal(1d, axis.ZoomFactor);
			Assert.True(axis.EnableAutoIntervalOnZooming);
			Assert.Equal(ChartAutoScrollingMode.End, axis.AutoScrollingMode);
			Assert.Equal(double.NaN, axis.AutoScrollingDelta);
		}

	 

		[Fact]
		public void NumericalAxis_DefaultLabelStyleProperties()
		{
			var axis = new NumericalAxis();
			var labelStyle = axis.LabelStyle; 
			Assert.True(axis.VisibleRange.IsEmpty);
			Assert.Equal(0, axis.ActualMinimum);
			Assert.Equal(0, axis.ActualMaximum);
			Assert.Equal(12d, labelStyle.FontSize);
			Assert.Equal(FontAttributes.None, labelStyle.FontAttributes);
			Assert.Null(labelStyle.FontFamily);
			Assert.Equal(Color.FromArgb("#49454F"), labelStyle.TextColor);
			Assert.Equal(string.Empty, labelStyle.LabelFormat);
		}

		[Fact]
		public void NumericalAxis_DefaultCollectionProperties()
		{
			var axis = new NumericalAxis(); 
			Assert.Null(axis.PlotBands);
			Assert.NotNull(axis.VisibleLabels);
			Assert.Empty(axis.VisibleLabels);
			Assert.NotNull(axis.TickPositions);
			Assert.Empty(axis.TickPositions);
			Assert.NotNull(axis.RegisteredSeries);
			Assert.Empty(axis.RegisteredSeries);
			Assert.NotNull(axis.ActualPlotBands);
			Assert.Empty(axis.ActualPlotBands);
		}

		[Fact]
		public void NumericalAxis_OffsetAndSizeProperties()
		{
			var axis = new NumericalAxis();
			Assert.Equal(0f, axis.LeftOffset);
			Assert.Equal(0f, axis.TopOffset);
			Assert.Equal(0d, axis.ActualPlotOffset);
			Assert.Equal(0d, axis.ActualPlotOffsetStart);
			Assert.Equal(0d, axis.ActualPlotOffsetEnd);
			Assert.Equal(Size.Zero, axis.ComputedDesiredSize);
			Assert.Equal(0d, axis.InsidePadding);
			Assert.Equal(Size.Zero, axis.AvailableSize);
		}

		[Fact]
		public void NumericalAxis_IntervalAndRangeProperties()
		{
			var axis = new NumericalAxis();
			Assert.Equal(0d, axis.ActualInterval);
			Assert.Equal(0d, axis.ActualRange.Start);
			Assert.Equal(0d, axis.ActualRange.End);
			Assert.Equal(0d, axis.ActualRange.Delta);
			Assert.Equal(0d, axis.ActualRange.Median);
			Assert.Equal(0d, axis.VisibleInterval);
			Assert.False(axis.SmallTickRequired);
			Assert.Equal(default(RectF), axis.RenderedRect);
			Assert.Equal(new int[] { 10, 5, 2, 1 }, ChartAxis.IntervalDivs);
			Assert.Equal(0, axis.MinorTicksPerInterval);
			Assert.Equal(0d, axis.PlotOffsetStart);
		}

		[Fact]
		public void NumericalAxis_BooleanProperties()
		{
			var axis = new NumericalAxis();
			Assert.False(axis.IsPolarArea);
			Assert.True(double.IsNaN(axis.ActualAutoScrollDelta));
			Assert.False(axis.CanAutoScroll);
			Assert.False(axis.IsVertical);
			Assert.False(axis.IsScrolling);
			Assert.False(axis._isOverriddenOnCreateLabelsMethod);
			Assert.Equal(AxisElementPosition.Outside, axis.TickPosition);
			Assert.Equal(AxisElementPosition.Outside, axis.LabelsPosition);
		}

		[Fact]
		public void NumericalAxis_NullAndCollectionProperties()
		{
			var axis = new NumericalAxis();
			Assert.NotNull(axis.AssociatedAxes);
			Assert.Empty(axis.AssociatedAxes);
			Assert.Null(axis.Area);
			Assert.Null(axis.CartesianArea);
			Assert.Null(axis.AxisRenderer);
			Assert.Null(axis.RangeStyles);
			Assert.Null(axis.TrackballLabelTemplate);
			Assert.Equal(Rect.Zero, axis.ArrangeRect);
		}

		[Fact]
		public void NumericalAxis_RemainingProperties()
		{
			var axis = new NumericalAxis();
			Assert.Equal(0d, axis.PlotOffsetEnd);
			Assert.Equal(0, axis.SideBySideSeriesCount);
			Assert.Equal(3, axis.MaximumLabels);
			Assert.Equal(270, axis.PolarStartAngle); 
			Assert.Equal(EdgeLabelsVisibilityMode.Default, axis.EdgeLabelsVisibilityMode);
			Assert.Equal(0, axis.MinorTicksPerInterval);
			Assert.NotNull(axis.MinorGridLineStyle);
			Assert.IsType<ChartLineStyle>(axis.MinorGridLineStyle);
		}

		[Fact]
		public void NumericalAxis_RangeAxisBaseProperties()
		{
			var axis = new NumericalAxis();

			Assert.NotNull(axis.MinorTickStyle);
			Assert.IsType<ChartAxisTickStyle>(axis.MinorTickStyle);
			Assert.True(axis.ShowMinorGridLines); 
			Assert.NotNull(axis.SmallTickPoints);
			Assert.Empty(axis.SmallTickPoints);
			Assert.NotNull(axis.MinorGridLineStroke);
			Assert.IsType<SolidColorBrush>(axis.MinorGridLineStroke);
			Assert.Equal(Color.FromArgb("#EDEFF1"), (axis.MinorGridLineStroke as SolidColorBrush)!.Color); 
			Assert.Equal(0.5f, axis.MinorGridLineStyle.StrokeWidth);
			Assert.Equal(4d, axis.MinorTickStyle.TickSize);

		}

		[Fact]
		public void LogarithmicAxis_DefaultNumericProperties()
		{
			var axis = new LogarithmicAxis(); 
			Assert.IsType<LogarithmicAxis>(axis);
			Assert.IsAssignableFrom<RangeAxisBase>(axis);
			Assert.Null(axis.Minimum);
			Assert.Null(axis.Maximum);
			Assert.Equal(10d, axis.LogarithmicBase);
			Assert.True(double.IsNaN(axis.Interval));
			Assert.True(double.IsNaN(axis.DefaultMinimum));
			Assert.True(double.IsNaN(axis.DefaultMaximum));
		}

		[Fact]
		public void LogarithmicAxis_DefaultVisibleRange()
		{
			var axis = new LogarithmicAxis();
			DoubleRange expectedValue = DoubleRange.Empty;
			Assert.Equal(expectedValue.Start, axis.VisibleRange.Start);
			Assert.Equal(expectedValue.End, axis.VisibleRange.End);
			Assert.Equal(expectedValue.Delta, axis.VisibleRange.Delta);
			Assert.Equal(expectedValue.Median, axis.VisibleRange.Median);
			Assert.Equal(expectedValue.IsEmpty, axis.VisibleRange.IsEmpty); 
			Assert.Equal(0d, axis.ZoomPosition);
			Assert.Equal(1d, axis.ZoomFactor);
			Assert.Equal(EdgeLabelsDrawingMode.Shift, axis.EdgeLabelsDrawingMode);
		}

		[Fact]
		public void LogarithmicAxis_DefaultAxisProperties()
		{
			var axis = new LogarithmicAxis();
			Assert.True(axis.IsVisible);
			Assert.Equal(0d, axis.AxisLineOffset);
			Assert.Equal(0d, axis.LabelRotation);
			Assert.Equal(double.NaN, axis.CrossesAt);
			Assert.True(axis.RenderNextToCrossingValue);
			Assert.Null(axis.CrossAxisName);
			Assert.Null(axis.Title);
			Assert.False(axis.IsInversed);
		}

		[Fact]
		public void LogarithmicAxis_DefaultStyleProperties()
		{
			var axis = new LogarithmicAxis(); 
			Assert.Equal(double.NaN, axis.AutoScrollingDelta);
			Assert.NotNull(axis.LabelStyle);
			Assert.IsType<ChartAxisLabelStyle>(axis.LabelStyle);
			Assert.NotNull(axis.AxisLineStyle);
			Assert.IsType<ChartLineStyle>(axis.AxisLineStyle);
			Assert.NotNull(axis.MajorGridLineStyle);
			Assert.IsType<ChartLineStyle>(axis.MajorGridLineStyle);
			Assert.NotNull(axis.MajorTickStyle);
			Assert.IsType<ChartAxisTickStyle>(axis.MajorTickStyle);
		} 

		[Fact]
		public void LogarithmicAxis_DefaultLabelStyleProperties()
		{
			var axis = new LogarithmicAxis();
			var labelStyle = axis.LabelStyle;
			Assert.Equal(0, axis.ActualMinimum);
			Assert.Equal(0, axis.ActualMaximum);
			Assert.Equal(12d, labelStyle.FontSize);
			Assert.Equal(FontAttributes.None, labelStyle.FontAttributes);
			Assert.Null(labelStyle.FontFamily); 
			Assert.Null(axis.PlotBands);
			Assert.Equal(Color.FromArgb("#49454F"), labelStyle.TextColor);
			Assert.Equal(string.Empty, labelStyle.LabelFormat);
		}

		[Fact]
		public void LogarithmicAxis_DefaultCollectionProperties()
		{
			var axis = new LogarithmicAxis();
			Assert.NotNull(axis.VisibleLabels);
			Assert.Empty(axis.VisibleLabels);
			Assert.NotNull(axis.TickPositions);
			Assert.Empty(axis.TickPositions);
			Assert.NotNull(axis.RegisteredSeries);
			Assert.Empty(axis.RegisteredSeries);
			Assert.NotNull(axis.ActualPlotBands);
			Assert.Empty(axis.ActualPlotBands); 
			Assert.True(axis.EnableAutoIntervalOnZooming);
			Assert.Equal(ChartAutoScrollingMode.End, axis.AutoScrollingMode);
		}

		[Fact]
		public void LogarithmicAxis_DefaultOffsetProperties()
		{
			var axis = new LogarithmicAxis();
			Assert.Equal(0f, axis.LeftOffset);
			Assert.Equal(0f, axis.TopOffset);
			Assert.Equal(0d, axis.ActualPlotOffset);
			Assert.Equal(0d, axis.ActualPlotOffsetStart);
			Assert.Equal(0d, axis.ActualPlotOffsetEnd);
			Assert.Equal(0d, axis.PlotOffsetStart);
			Assert.Equal(0d, axis.PlotOffsetEnd); 
			Assert.Equal(Size.Zero, axis.ComputedDesiredSize);
			Assert.Equal(0d, axis.InsidePadding);
		}
		 

		[Fact]
		public void LogarithmicAxis_DefaultIntervalProperties()
		{
			var axis = new LogarithmicAxis();
			Assert.Equal(0d, axis.ActualInterval);
			Assert.Equal(0d, axis.ActualRange.Start);
			Assert.Equal(0d, axis.ActualRange.End);
			Assert.Equal(0d, axis.ActualRange.Delta);
			Assert.Equal(0d, axis.ActualRange.Median);
			Assert.Equal(0d, axis.VisibleInterval);
			Assert.Equal(new int[] { 10, 5, 2, 1 }, ChartAxis.IntervalDivs);
			Assert.Equal(0, axis.MinorTicksPerInterval); 
			Assert.False(axis.SmallTickRequired);
			Assert.False(axis.IsPolarArea); 
			Assert.False(axis.IsVertical);
			Assert.False(axis.CanAutoScroll);
		}

		[Fact]
		public void LogarithmicAxis_DefaultBooleanProperties()
		{
			var axis = new LogarithmicAxis();
			Assert.False(axis.IsScrolling);
			Assert.False(axis._isOverriddenOnCreateLabelsMethod);
			Assert.False(axis.ShowTrackballLabel); 
			Assert.NotNull(axis.AssociatedAxes);
			Assert.Empty(axis.AssociatedAxes);
			Assert.Null(axis.RangeStyles); 
			Assert.Equal(Size.Zero, axis.AvailableSize);
			Assert.Equal(default(RectF), axis.RenderedRect);
			Assert.Equal(Rect.Zero, axis.ArrangeRect);
		}

		 

		[Fact]
		public void LogarithmicAxis_DefaultNullProperties()
		{
			var axis = new LogarithmicAxis();
			Assert.Null(axis.Area);
			Assert.Null(axis.CartesianArea);
			Assert.Null(axis.AxisRenderer);
			Assert.Null(axis.TrackballLabelTemplate); 
			Assert.True(double.IsNaN(axis.ActualAutoScrollDelta));
			Assert.Equal(0, axis.SideBySideSeriesCount);
			Assert.Equal(3, axis.MaximumLabels);
			Assert.Equal(270, axis.PolarStartAngle);
		} 
		[Fact]
		public void LogarithmicAxis_DefaultEnumProperties()
		{
			var axis = new LogarithmicAxis();

			Assert.Equal(EdgeLabelsVisibilityMode.Default, axis.EdgeLabelsVisibilityMode);
			Assert.Equal(0, axis.MinorTicksPerInterval);
			Assert.NotNull(axis.MinorGridLineStyle);
			Assert.IsType<ChartLineStyle>(axis.MinorGridLineStyle);
			Assert.NotNull(axis.MinorTickStyle);
			Assert.IsType<ChartAxisTickStyle>(axis.MinorTickStyle);
			Assert.Equal(AxisElementPosition.Outside, axis.TickPosition);
			Assert.Equal(AxisElementPosition.Outside, axis.LabelsPosition);
		}

		[Fact]
		public void LogarithmicAxis_RangeAxisBaseProperties()
		{
			var axis = new LogarithmicAxis();
			 
			Assert.True(axis.ShowMinorGridLines); 
			Assert.NotNull(axis.SmallTickPoints);
			Assert.Empty(axis.SmallTickPoints);
			Assert.NotNull(axis.MinorGridLineStroke);
			Assert.IsType<SolidColorBrush>(axis.MinorGridLineStroke);
			Assert.Equal(Color.FromArgb("#EDEFF1"), (axis.MinorGridLineStroke as SolidColorBrush)!.Color); 
			Assert.Equal(0.5f, axis.MinorGridLineStyle.StrokeWidth);
			Assert.Equal(4d, axis.MinorTickStyle.TickSize);
			 
		}
	}
}
