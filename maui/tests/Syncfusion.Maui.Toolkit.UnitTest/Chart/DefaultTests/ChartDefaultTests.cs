using System.Collections.ObjectModel;
using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.Toolkit.UnitTest.Charts
{
	public class ChartDefaultTests
	{
		[Fact]
		public void SfCartesianChartDefaultTests_Part1()
		{
			var chart = new SfCartesianChart();
			Assert.NotNull(chart.Series);
			Assert.IsType<ChartSeriesCollection>(chart.Series);
			Assert.Empty(chart.Series);
			Assert.NotNull(chart.XAxes);
			Assert.IsType<ObservableCollection<ChartAxis>>(chart.XAxes);
			Assert.Empty(chart.XAxes);
			Assert.NotNull(chart.YAxes);
			Assert.IsType<ObservableCollection<RangeAxisBase>>(chart.YAxes);
		}

		[Fact]
		public void SfCartesianChartDefaultTests_Part2()
		{
			var chart = new SfCartesianChart();
			Assert.Empty(chart.YAxes);
			Assert.True(chart.EnableSideBySideSeriesPlacement);
			Assert.False(chart.IsTransposed);
			Assert.NotNull(chart.PaletteBrushes);
			Assert.Equal(ChartColorModel.DefaultBrushes, chart.PaletteBrushes);
			Assert.Null(chart.ZoomPanBehavior);
			Assert.Null(chart.SelectionBehavior);
		}

		[Fact]
		public void SfCartesianChartDefaultTests_Part3()
		{
			var chart = new SfCartesianChart();
			Assert.Null(chart.TrackballBehavior);
			Assert.NotNull(chart.Annotations);
			Assert.IsType<ChartAnnotationCollection>(chart.Annotations);
			Assert.Empty(chart.Annotations);
			Assert.False(chart.CanAnnotationUnderPlotArea);
			Assert.Equal(Rect.Zero, chart.SeriesBounds);
		}

		[Fact]
		public void SfCartesianChartDefaultTests_Part4()
		{
			var chart = new SfCartesianChart();
			Assert.NotNull(chart.TrackLineStroke);
			Assert.IsType<SolidColorBrush>(chart.TrackLineStroke);
			Assert.Equal(Color.FromArgb("#DE000000"), ((SolidColorBrush)chart.TrackLineStroke).Color);
			Assert.Null(chart.Title);
			Assert.Null(chart.Legend);
		}

		[Fact]
		public void SfCartesianChartDefaultTests_Part5()
		{
			var chart = new SfCartesianChart();
			Assert.Null(chart.TooltipBehavior);
			Assert.Null(chart.InteractiveBehavior);
			Assert.Null(chart.PlotAreaBackgroundView);
			Assert.NotNull(chart.LegendStyle);
			Assert.IsType<ChartThemeLegendLabelStyle>(chart.LegendStyle);
			Assert.Null(chart.TooltipBackground);
			Assert.Null(chart.TooltipTextColor);
			Assert.Equal(double.NaN, chart.TooltipFontSize);
		}

		[Fact]
		public void SfCartesianChartDefaultTests_Part6()
		{
			var chart = new SfCartesianChart();
			Assert.NotNull(chart.TitleView);
			Assert.IsType<ChartTitleView>(chart.TitleView);
			Assert.NotNull(chart.BehaviorLayout);
			Assert.IsType<AbsoluteLayout>(chart.BehaviorLayout);
			Assert.True(chart.IsRequiredDataLabelsMeasure);
			Assert.NotNull(chart.Content);
			Assert.IsType<Grid>(chart.Content);
			Assert.Equal(FlowDirection.LeftToRight, chart.FlowDirection);
		}

		[Fact]
		public void SfCircularChartDefaultTests_Part1()
		{
			var chart = new SfCircularChart();
			Assert.NotNull(chart.Series);
			Assert.IsType<ChartSeriesCollection>(chart.Series);
			Assert.Empty(chart.Series);
			Assert.Null(chart.Title);
			Assert.Null(chart.Legend);
			Assert.Null(chart.TooltipBehavior);
			Assert.Null(chart.InteractiveBehavior);
			Assert.Null(chart.PlotAreaBackgroundView);
		}

		[Fact]
		public void SfCircularChartDefaultTests_Part2()
		{
			var chart = new SfCircularChart();
			Assert.NotNull(chart.LegendStyle);
			Assert.IsType<ChartThemeLegendLabelStyle>(chart.LegendStyle);
			Assert.Null(chart.TooltipBackground);
			Assert.Null(chart.TooltipTextColor);
			Assert.Equal(double.NaN, chart.TooltipFontSize);
			Assert.Equal(Rect.Zero, chart.SeriesBounds);
		}

		[Fact]
		public void SfCircularChartDefaultTests_Part3()
		{
			var chart = new SfCircularChart();

			Assert.NotNull(chart.TitleView);
			Assert.IsType<ChartTitleView>(chart.TitleView);
			Assert.NotNull(chart.BehaviorLayout);
			Assert.IsType<AbsoluteLayout>(chart.BehaviorLayout);
			Assert.True(chart.IsRequiredDataLabelsMeasure);
			Assert.NotNull(chart.Content);
			Assert.IsType<Grid>(chart.Content);
			Assert.Equal(FlowDirection.LeftToRight, chart.FlowDirection);
		}

		[Fact]
		public void SfPyramidChartDefaultTests_Part1()
		{
			var chart = new SfPyramidChart();
			Assert.Null(chart.ItemsSource);
			Assert.Null(chart.TooltipTemplate);
			Assert.False(chart.EnableTooltip);
			Assert.Null(chart.XBindingPath);
			Assert.Null(chart.YBindingPath);
			Assert.NotNull(chart.PaletteBrushes);
			Assert.Equal(ChartColorModel.DefaultBrushes, chart.PaletteBrushes);
		}

		[Fact]
		public void SfPyramidChartDefaultTests_Part2()
		{
			var chart = new SfPyramidChart();
			Assert.Null(chart.SelectionBehavior);
			Assert.False(chart.ShowDataLabels);
			Assert.Null(chart.LabelTemplate);
			Assert.NotNull(chart.DataLabelSettings);
			Assert.IsType<PyramidDataLabelSettings>(chart.DataLabelSettings);
			Assert.Equal(ChartLegendIconType.Circle, chart.LegendIcon);
			Assert.NotNull(chart.Stroke);
			Assert.Equal(Colors.Transparent, chart.Stroke);
		}

		[Fact]
		public void SfPyramidChartDefaultTests_Part3()
		{
			var chart = new SfPyramidChart();
			Assert.Equal(2d, chart.StrokeWidth);
			Assert.Equal(0d, chart.GapRatio);
			Assert.Equal(PyramidMode.Linear, chart.Mode);
			Assert.NotNull(chart.DataLabels);
			Assert.IsType<ObservableCollection<ChartDataLabel>>(chart.DataLabels);
			Assert.Empty(chart.DataLabels);
		}

		[Fact]
		public void SfPyramidChartDefaultTests_Part4()
		{
			var chart = new SfPyramidChart();
			Assert.NotNull(chart.LabelTemplateView);
			Assert.IsType<DataLabelLayout>(chart.LabelTemplateView);
			Assert.Equal(Rect.Zero, chart.SeriesBounds);
			Assert.Null(chart.Title);
			Assert.Null(chart.Legend);
			Assert.Null(chart.TooltipBehavior);
			Assert.Null(chart.InteractiveBehavior);
			Assert.Null(chart.PlotAreaBackgroundView);
		}

		[Fact]
		public void SfPyramidChartDefaultTests_Part5()
		{
			var chart = new SfPyramidChart();
			Assert.NotNull(chart.LegendStyle);
			Assert.IsType<ChartThemeLegendLabelStyle>(chart.LegendStyle);
			Assert.Null(chart.TooltipBackground);
			Assert.Null(chart.TooltipTextColor);
			Assert.Equal(double.NaN, chart.TooltipFontSize);
		}

		[Fact]
		public void SfPyramidChartDefaultTests_Part6()
		{
			var chart = new SfPyramidChart();
			Assert.NotNull(chart.TitleView);
			Assert.IsType<ChartTitleView>(chart.TitleView);
			Assert.NotNull(chart.BehaviorLayout);
			Assert.IsType<AbsoluteLayout>(chart.BehaviorLayout);
			Assert.False(chart.IsRequiredDataLabelsMeasure);
			Assert.NotNull(chart.Content);
			Assert.IsType<Grid>(chart.Content);
			Assert.Equal(FlowDirection.LeftToRight, chart.FlowDirection);
		}

		[Fact]
		public void SfFunnelChartDefaultTests_Part1()
		{
			var chart = new SfFunnelChart();
			Assert.Null(chart.ItemsSource);
			Assert.Null(chart.XBindingPath);
			Assert.Null(chart.YBindingPath);
			Assert.NotNull(chart.PaletteBrushes);
			Assert.Equal(ChartColorModel.DefaultBrushes, chart.PaletteBrushes);
			Assert.NotNull(chart.Stroke);
			Assert.Equal(Colors.Transparent, chart.Stroke);
			Assert.Equal(2d, chart.StrokeWidth);
		}

		[Fact]
		public void SfFunnelChartDefaultTests_Part2()
		{
			var chart = new SfFunnelChart();
			Assert.Equal(ChartLegendIconType.Circle, chart.LegendIcon);
			Assert.Null(chart.TooltipTemplate);
			Assert.False(chart.EnableTooltip);
			Assert.Null(chart.SelectionBehavior);
			Assert.False(chart.ShowDataLabels);
			Assert.NotNull(chart.DataLabelSettings);
			Assert.IsType<FunnelDataLabelSettings>(chart.DataLabelSettings);
		}

		[Fact]
		public void SfFunnelChartDefaultTests_Part3()
		{
			var chart = new SfFunnelChart();
			Assert.Equal(0d, chart.GapRatio);
			Assert.Null(chart.LabelTemplate);
			Assert.Equal(40d, chart.NeckWidth);
			Assert.Equal(0d, chart.NeckHeight);
			Assert.NotNull(chart.DataLabels);
			Assert.IsType<ObservableCollection<ChartDataLabel>>(chart.DataLabels);
			Assert.Empty(chart.DataLabels);
		}

		[Fact]
		public void SfFunnelChartDefaultTests_Part4()
		{
			var chart = new SfFunnelChart();
			Assert.NotNull(chart.LabelTemplateView);
			Assert.IsType<DataLabelLayout>(chart.LabelTemplateView);
			Assert.Equal(Rect.Zero, chart.SeriesBounds);
			Assert.Null(chart.Title);
			Assert.Null(chart.Legend);
			Assert.Null(chart.TooltipBehavior);
			Assert.Null(chart.InteractiveBehavior);
			Assert.Null(chart.PlotAreaBackgroundView);
		}

		[Fact]
		public void SfFunnelChartDefaultTests_Part5()
		{
			var chart = new SfFunnelChart();
			Assert.NotNull(chart.LegendStyle);
			Assert.IsType<ChartThemeLegendLabelStyle>(chart.LegendStyle);
			Assert.Null(chart.TooltipBackground);
			Assert.Null(chart.TooltipTextColor);
			Assert.Equal(double.NaN, chart.TooltipFontSize);
		}

		[Fact]
		public void SfFunnelChartDefaultTests_Part6()
		{
			var chart = new SfFunnelChart();
			Assert.NotNull(chart.TitleView);
			Assert.IsType<ChartTitleView>(chart.TitleView);
			Assert.NotNull(chart.BehaviorLayout);
			Assert.IsType<AbsoluteLayout>(chart.BehaviorLayout);
			Assert.False(chart.IsRequiredDataLabelsMeasure);
			Assert.NotNull(chart.Content);
			Assert.IsType<Grid>(chart.Content);
			Assert.Equal(FlowDirection.LeftToRight, chart.FlowDirection);
		}

		[Fact]
		public void SfPolarChartDefaultTests_Part1()
		{
			var chart = new SfPolarChart();
			Assert.Null(chart.PrimaryAxis);
			Assert.Null(chart.SecondaryAxis);
			Assert.NotNull(chart.Series);
			Assert.IsType<ChartPolarSeriesCollection>(chart.Series);
			Assert.Empty(chart.Series);
			Assert.NotNull(chart.PaletteBrushes);
			Assert.Equal(ChartColorModel.DefaultBrushes, chart.PaletteBrushes);
		}

		[Fact]
		public void SfPolarChartDefaultTests_Part2()
		{
			var chart = new SfPolarChart();
			Assert.Equal(PolarChartGridLineType.Circle, chart.GridLineType);
			Assert.Equal(ChartPolarAngle.Rotate270, chart.StartAngle);
			Assert.Null(chart.Title);
			Assert.Null(chart.Legend);
			Assert.Null(chart.TooltipBehavior);
			Assert.Null(chart.InteractiveBehavior);
			Assert.Null(chart.PlotAreaBackgroundView);
		}

		[Fact]
		public void SfPolarChartDefaultTests_Part3()
		{
			var chart = new SfPolarChart();
			Assert.NotNull(chart.LegendStyle);
			Assert.IsType<ChartThemeLegendLabelStyle>(chart.LegendStyle);
			Assert.Null(chart.TooltipBackground);
			Assert.Null(chart.TooltipTextColor);
			Assert.Equal(double.NaN, chart.TooltipFontSize);
			Assert.Equal(Rect.Zero, chart.SeriesBounds);
			Assert.Equal(270, chart.PolarStartAngle);
		}

		[Fact]
		public void SfPolarChartDefaultTests_Part4()
		{
			var chart = new SfPolarChart();
			Assert.NotNull(chart.TitleView);
			Assert.IsType<ChartTitleView>(chart.TitleView);
			Assert.NotNull(chart.BehaviorLayout);
			Assert.IsType<AbsoluteLayout>(chart.BehaviorLayout);
			Assert.False(chart.IsRequiredDataLabelsMeasure);
			Assert.NotNull(chart.Content);
			Assert.IsType<Grid>(chart.Content);
			Assert.Equal(FlowDirection.LeftToRight, chart.FlowDirection);
		}
	}
}
