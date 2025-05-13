using System.Collections.ObjectModel;
using Syncfusion.Maui.Toolkit.Charts;
using Syncfusion.Maui.Toolkit.Themes;

namespace Syncfusion.Maui.Toolkit.UnitTest.Charts
{
	public class InterfaceUnitTests : BaseUnitTest
	{
		[Fact]
		public void GetThemeDictionary()
		{
			SfPyramidChart sfPyramidChart = new SfPyramidChart();
			var result = ((IParentThemeElement)sfPyramidChart).GetThemeDictionary();
			Assert.IsType<SfPyramidChartStyles>(result);
		}

		[Fact]
		public void UpdateLegendItemsSource_WithVisibleLegend_UpdatesLegendItems()
		{
			var sfPyramidChart = new SfPyramidChart();
			ObservableCollection<ILegendItem> legendItems = [];
			sfPyramidChart.Legend = new ChartLegend { IsVisible = true };

			_ = new PyramidChartArea(sfPyramidChart);
			SetPrivateField(sfPyramidChart, "_pointsCount", 5);
			SetPrivateField(sfPyramidChart, "_actualData", new List<object> { 10, 40, 30, 50, 60 });
			((IPyramidChartDependent)sfPyramidChart).UpdateLegendItemsSource(legendItems);
			Assert.Equal(5, legendItems.Count);
		}

		[Fact]
		public void GenerateSegments()
		{
			var sfPyramidChart = new SfPyramidChart()
			{ GapRatio = 0.5, };
			SetPrivateField(sfPyramidChart, "_pointsCount", 1);
			SetPrivateField(sfPyramidChart, "_yValues", new List<double> { 5 });
			((IPyramidChartDependent)sfPyramidChart).GenerateSegments();
			var result = (ObservableCollection<ChartSegment>?)GetPrivateField(sfPyramidChart, "_segments");

			Assert.True((result!.Count >= 1));
		}

		[Fact]
		public void GetThemeDictionary_Polar()
		{
			SfPolarChart sfPolarChart = new SfPolarChart();
			var result = ((IParentThemeElement)sfPolarChart).GetThemeDictionary();
			Assert.IsType<SfCartesianChartStyles>(result);
		}

		[Fact]
		public void GetThemeDictionary_Funnel()
		{
			SfFunnelChart sfFunnelChart = new SfFunnelChart();
			var result = ((IParentThemeElement)sfFunnelChart).GetThemeDictionary();
			Assert.IsType<SfFunnelChartStyles>(result);
		}
		[Fact]
		public void GetThemeDictionary_Circular()
		{
			SfCircularChart sfCircularChart = new SfCircularChart();
			var result = ((IParentThemeElement)sfCircularChart).GetThemeDictionary();
			Assert.IsType<SfCircularChartStyles>(result);
		}

		[Fact]
		public void GetThemeDictionary_Cartesian()
		{
			SfCartesianChart sfCartesianChart = new SfCartesianChart();
			var result = ((IParentThemeElement)sfCartesianChart).GetThemeDictionary();
			Assert.IsType<SfCartesianChartStyles>(result);
		}

		[Fact]
		public void GetThemeDictionary_Theme()
		{
			SfCartesianChart sfCartesianChart = new SfCartesianChart();
			ChartThemeLegendLabelStyle theme = new ChartThemeLegendLabelStyle(sfCartesianChart);
			var result = ((IParentThemeElement)theme).GetThemeDictionary();
			Assert.IsType<SfChartCommonStyle>(result);
		}

		[Fact]
		public void GetLabelContent_ContextXValue_ValidXValue()
		{
			_ = new SfPyramidChart();
			PyramidDataLabelSettings pyramidDataLabelSettings = new PyramidDataLabelSettings();
			var xValue = "TestXValue";
			var yValue = 100.0;
			pyramidDataLabelSettings.Context = PyramidDataLabelContext.XValue;
			var result = ((IPyramidDataLabelSettings)pyramidDataLabelSettings).GetLabelContent(xValue, yValue);
			Assert.Equal("TestXValue", result);
		}

		[Fact]
		public void GetLabelContent_ContextYValue_ValidYValue_NoLabelFormat()
		{
			PyramidDataLabelSettings pyramidDataLabelSettings = new PyramidDataLabelSettings();
			object? xValue = null;
			var yValue = 123.456;
			pyramidDataLabelSettings.Context = PyramidDataLabelContext.YValue;
			var result = ((IPyramidDataLabelSettings)pyramidDataLabelSettings).GetLabelContent(xValue, yValue);
			Assert.Equal("123.46", result);
		}

		[Fact]
		public void GetLabelContent_ContextYValue_ValidYValue_WithLabelFormat()
		{
			PyramidDataLabelSettings pyramidDataLabelSettings = new PyramidDataLabelSettings();
			object? xValue = null;
			var yValue = 987.654;
			pyramidDataLabelSettings.Context = PyramidDataLabelContext.YValue;
			pyramidDataLabelSettings.LabelStyle = new ChartDataLabelStyle { LabelFormat = "F1" };
			var result = ((IPyramidDataLabelSettings)pyramidDataLabelSettings).GetLabelContent(xValue, yValue);
			Assert.Equal("987.7", result);
		}

		[Fact]
		public void GetSelectionBrush()
		{
			var sfCartesianChart = new SfCartesianChart();
			ColumnSeries columnSeries = new ColumnSeries();
			SeriesSelectionBehavior seriesSelectionBehavior = new SeriesSelectionBehavior() { SelectedIndex = 0, SelectionBrush = Colors.Red };
			sfCartesianChart.SelectionBehavior = seriesSelectionBehavior;
			sfCartesianChart.Series.Add(columnSeries);
			var categoryAxis = new CategoryAxis { IsVertical = false };
			sfCartesianChart.XAxes.Add(categoryAxis);
			var expected = Colors.Red;
			var result = ((IChart)sfCartesianChart).GetSelectionBrush(columnSeries);
			Assert.Equal(expected, result);
			Assert.NotNull(result);
			Assert.IsType<SolidColorBrush>(result);
		}

		[Fact]
		public void OnLegendItemCreated_AppliesStyleCorrectly()
		{
			var chartLegend = new ChartLegend();
			var columnSeries = new ColumnSeries();
			var legendItem = new LegendItem
			{
				IconType = ChartUtils.GetShapeType(columnSeries.LegendIcon),
				Item = columnSeries,
				Source = columnSeries,
				Text = columnSeries.Label,
				Index = 0,
				IsToggled = !columnSeries.IsVisible
			};
			var expectedFontSize = 12;
			((IChartLegend)chartLegend).OnLegendItemCreated(legendItem);
			Assert.Equal(expectedFontSize, legendItem.FontSize);
		}

		[Fact]
		public void OnLegendItemCreated_DoesNotModifyUnrelatedProperties()
		{
			var chartLegend = new ChartLegend();
			var columnSeries = new ColumnSeries();
			var originalIconType = ChartUtils.GetShapeType(columnSeries.LegendIcon);
			var legendItem = new LegendItem
			{
				IconType = originalIconType,
				Item = columnSeries,
				Source = columnSeries,
				Text = columnSeries.Label,
				Index = 0,
				IsToggled = !columnSeries.IsVisible
			};
			((IChartLegend)chartLegend).OnLegendItemCreated(legendItem);
			Assert.Equal(originalIconType, legendItem.IconType);
		}

		[Fact]
		public void OnLegendItemCreated_InvokesEvent()
		{
			var chartLegend = new ChartLegend();
			var columnSeries = new ColumnSeries();
			var legendItem = new LegendItem
			{
				IconType = ChartUtils.GetShapeType(columnSeries.LegendIcon),
				Item = columnSeries,
				Source = columnSeries,
				Text = "TestLabel",
				Index = 0,
				IsToggled = !columnSeries.IsVisible
			};

			bool eventInvoked = false;
			chartLegend.LegendItemCreated += (sender, args) =>
			{
				eventInvoked = true;
				Assert.Equal(legendItem, args.LegendItem);
			};
			((IChartLegend)chartLegend).OnLegendItemCreated(legendItem);
			Assert.True(eventInvoked);
		}

		[Fact]
		public void GetMaximumSizeCoefficient_ReturnsDefaultSize()
		{
			var expectedResult = 0.25d;
			var chartLegend = new ChartLegend();
			var result = InvokePrivateMethod(chartLegend, "GetMaximumSizeCoefficient", []);
			Assert.Equal(expectedResult, result);
		}

		[Fact]
		public void GetMaximumSizeCoefficient_NoException()
		{
			var chartLegend = new ChartLegend();
			var exception = Record.Exception(() => InvokePrivateMethod(chartLegend, "GetMaximumSizeCoefficient", []));
			Assert.Null(exception);
		}
	}
}
