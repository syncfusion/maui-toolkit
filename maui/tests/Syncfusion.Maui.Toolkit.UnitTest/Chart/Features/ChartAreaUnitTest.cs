using System.Collections.ObjectModel;
using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.Toolkit.UnitTest.Charts
{
	public class ChartAreaUnitTest : BaseUnitTest
	{
		#region CartesianChartArea method

		[Fact]
		public void ResetSBSSegments_Test()
		{
			var series1 = new ColumnSeries() { SegmentsCreated = true };
			var series2 = new ColumnSeries() { SegmentsCreated = true };

			var area = new CartesianChartArea(new SfCartesianChart())
			{
				Series = []
			};
			area.Series.Add(series1);
			area.Series.Add(series2);

			area.ResetSBSSegments();

			Assert.False(series1.SegmentsCreated);
			Assert.False(series2.SegmentsCreated);
		}

		[Fact]
		public void GetTotalWidth_Test()
		{
			var series1 = new ColumnSeries() { Width = 10 };
			var series2 = new ColumnSeries() { Width = 10 };
			var series3 = new ColumnSeries() { Width = 10 };

			var area = new CartesianChartArea(new SfCartesianChart())
			{
				SideBySideSeriesPosition = new Dictionary<object, List<CartesianSeries>>
				{
					{ 0, new List<CartesianSeries> { series1, series2 } },
					{ 1, new List<CartesianSeries> { series3 } }
				}
			};

			var totalWidth = InvokePrivateMethod(area, "GetTotalWidth");

			Assert.NotNull(totalWidth);
			Assert.Equal(20, (double)totalWidth);
		}


		[Fact]
		public void AddAxes_ShouldSetParentAndArea()
		{
			var axisArea = new CartesianChartArea(new SfCartesianChart());
			var chartAxis = new CategoryAxis();

			InvokePrivateMethod(axisArea, "AddAxes", chartAxis);

			Assert.Equal(axisArea, chartAxis.Area);
			Assert.True(axisArea.RequiredAxisReset);
			Assert.Equal(axisArea.BindingContext, chartAxis.BindingContext);
		}


		[Fact]
		public void RemoveAxes_ShouldResetParentAndArea()
		{
			var axisArea = new CartesianChartArea(new SfCartesianChart());
			var chartAxis = new CategoryAxis()
			{
				Parent = axisArea.Parent,
				Area = axisArea
			};

			InvokePrivateMethod(axisArea, "RemoveAxes", chartAxis);

			Assert.Null(chartAxis.Parent);
			Assert.True(axisArea.RequiredAxisReset);
			//Assert.Null(chartAxis.Area);
		}

		#endregion

		#region CartesianPlotArea method

		[Fact]
		public void AddSeries_ShouldSetChartAreaAndResetSegments()
		{
			var area = new CartesianChartArea(new SfCartesianChart());
			var plotArea = new CartesianPlotArea(area);
			var newSeries = new LineSeries() { EnableAnimation = true };

			plotArea.AddSeries(0, newSeries);

			var chartArea = GetPrivateField(plotArea, "_chartArea") as CartesianChartArea;

			Assert.Equal(chartArea, newSeries.ChartArea);
			Assert.True(chartArea?.RequiredAxisReset);
			Assert.Null(chartArea?.SideBySideSeriesPosition);
			Assert.False(newSeries.SegmentsCreated);
			Assert.True(newSeries.NeedToAnimateSeries);
		}

		[Fact]
		public void RemoveSeries_ShouldInvalidateGroupValuesAndResetAxes()
		{
			var area = new CartesianChartArea(new SfCartesianChart());
			var plotArea = new CartesianPlotArea(area);
			var sideBySideSeries = new ColumnSeries() { ActualXAxis = new CategoryAxis(), ActualYAxis = new NumericalAxis() };

			plotArea.RemoveSeries(0, sideBySideSeries);
			var chartArea = GetPrivateField(plotArea, "_chartArea") as CartesianChartArea;

			Assert.True(chartArea?.RequiredAxisReset);
			Assert.Null(sideBySideSeries.ActualXAxis);
			Assert.Null(sideBySideSeries.ActualYAxis);
		}

		#endregion

		#region ChartPlotArea method

		[Fact]
		public void ResetSeries_ShouldCallResetDataOnAllSeries()
		{
			var area = new CartesianChartArea(new SfCartesianChart());
			var plotArea = new CartesianPlotArea(area);
			var series1 = new LineSeries() { PointsCount = 10 };
			var series2 = new LineSeries() { PointsCount = 10 };
			plotArea._seriesViews.Children.Add(new SeriesView(series1, plotArea));
			plotArea._seriesViews.Children.Add(new SeriesView(series2, plotArea));

			InvokePrivateMethod(plotArea, "ResetSeries");

			Assert.Equal(0, series1.PointsCount);
			Assert.Equal(0, series2.PointsCount);
			Assert.Empty(plotArea._seriesViews.Children);
		}

		#endregion

		#region CircularPlotArea method

		[Fact]
		public void AddSeries_CircularPlotAreaTest()
		{
			var area = new CircularChartArea(new SfCircularChart());
			var plotArea = new CircularPlotArea(area);
			var newSeries = new PieSeries() { EnableAnimation = true };

			plotArea.AddSeries(0, newSeries);

			var chartArea = GetPrivateField(plotArea, "_circularChartArea") as CircularChartArea;

			Assert.Equal(chartArea, newSeries.ChartArea);
			Assert.False(newSeries.SegmentsCreated);
		}

		[Fact]
		public void CreateLegendItem_ShouldReturnCorrectLegendItem()
		{
			var area = new CircularChartArea(new SfCircularChart());
			var plotArea = new CircularPlotArea(area);
			var series = new PieSeries()
			{
				LegendIcon = ChartLegendIconType.Circle,
				ActualData = ["Data1", "Data2"]
			};
			var expectedIconType = ShapeType.Circle;
			var parameter = new object[] { 1, series };

			var legendItem = InvokePrivateMethod(plotArea, "CreateLegendItem", parameter) as LegendItem;

			Assert.NotNull(legendItem);
			Assert.Equal(expectedIconType, legendItem.IconType);
			Assert.Equal(series, legendItem.Source);
			Assert.Equal(series.ActualData[1], legendItem.Item);
			Assert.NotNull(legendItem.IconBrush);
		}

		[Fact]
		public void InsertLegendItemAt_ShouldInsertLegendItemAtCorrectIndex()
		{
			var chart = new SfCircularChart();
			var area = new CircularChartArea(chart);
			var plotArea = new CircularPlotArea(area);
			var series = new PieSeries()
			{
				LegendIcon = ChartLegendIconType.Rectangle,
				ActualData = ["Data1", "Data2"],
				GroupTo = double.NaN
			};

			var parameter = new object[] { 0, series };

			InvokePrivateMethod(plotArea, "InsertLegendItemAt", parameter);

			var legentItem = GetPrivateField(plotArea, "_legendItems") as ObservableCollection<ILegendItem>;

			Assert.NotNull(legentItem);
			Assert.Single(legentItem);
			var insertedItem = legentItem[0];
			Assert.Equal(series, ((LegendItem)insertedItem).Source);
		}

		#endregion

		#region PolarChartArea methods

		[Fact]
		public void TestPolar_UpdateLegendItems()
		{
			var chart = new SfPolarChart();
			var area = new PolarChartArea(chart);

			SetPrivateField(area, "_shouldPopulateLegendItems", true);
			area.UpdateLegendItems();

			var result = GetPrivateField(area, "_shouldPopulateLegendItems");

			Assert.NotNull(result);
			Assert.False((bool)result);
		}

		[Fact]
		public void GetSecondaryAxis_Test()
		{
			var chart = new SfPolarChart();
			var area = new PolarChartArea(chart);
			var expectedSecondaryAxis = new NumericalAxis();
			chart.SecondaryAxis = expectedSecondaryAxis;

			var secondaryAxis = area.GetSecondaryAxis();

			Assert.NotNull(secondaryAxis);
			Assert.Equal(expectedSecondaryAxis, secondaryAxis);
		}

		[Fact]
		public void GetPrimaryAxis_Test()
		{
			var chart = new SfPolarChart();
			var area = new PolarChartArea(chart);
			var expectedPrimaryAxis = new CategoryAxis();
			chart.PrimaryAxis = expectedPrimaryAxis;

			var primaryAxis = area.GetPrimaryAxis();

			Assert.NotNull(primaryAxis);
			Assert.Equal(expectedPrimaryAxis, primaryAxis);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void ScheduleUpdate_Test(bool isRelayout)
		{
			var chart = new SfPolarChart();
			var area = new PolarChartArea(chart);

			area.ScheduleUpdate(isRelayout);

			Assert.Equal(isRelayout, area.NeedsRelayout);
		}

		[Fact]
		public void PolarAngleToPoint_Test()
		{
			var chart = new SfPolarChart();
			var area = new PolarChartArea(chart);
			var axis = new CategoryAxis() { IsInversed = false };
			area._polarChart.PolarStartAngle = 0;
			area.PolarAxisCenter = new PointF(0, 0);
			float radius = 10;
			float theta = 45; // 45 degrees

			PointF result = area.PolarAngleToPoint(axis, radius, theta);

			float expectedX = (float)(radius * Math.Cos(45 * (Math.PI / 180)));
			float expectedY = (float)(radius * Math.Sin(45 * (Math.PI / 180)));
			Assert.Equal(expectedX, result.X, precision: 3);
			Assert.Equal(expectedY, result.Y, precision: 3);
		}

		[Fact]
		public void AddSeries_PolarChartArea_Test()
		{
			var chart = new SfPolarChart();
			var area = new PolarChartArea(chart);
			var newSeries = new PolarLineSeries() { EnableAnimation = true };

			InvokePrivateMethod(area, "AddSeries", [0, newSeries]);

			Assert.Equal(area, newSeries.ChartArea);
			Assert.Equal(chart, newSeries.Chart);
			Assert.False(newSeries.SegmentsCreated);
			Assert.True(newSeries.NeedToAnimateSeries);
		}

		[Fact]
		public void RemoveSeries_PolarChartArea_Test()
		{
			var chart = new SfPolarChart();
			var area = new PolarChartArea(chart);
			var newSeries = new PolarLineSeries() { SegmentsCreated = true, ActualXAxis = new CategoryAxis(), ActualYAxis = new NumericalAxis(), Parent = area };

			InvokePrivateMethod(area, "RemoveSeries", [0, newSeries]);

			Assert.False(newSeries.SegmentsCreated);
			Assert.Null(newSeries.Parent);
		}

		[Fact]
		public void ResetSeries_PolarChartArea_Test()
		{
			var chart = new SfPolarChart();
			var area = new PolarChartArea(chart);
			var series1 = new PolarLineSeries() { PointsCount = 10 };
			var series2 = new PolarLineSeries() { PointsCount = 10 };
			var seriesView = GetPrivateField(area, "_seriesViews") as AbsoluteLayout;

			Assert.NotNull(seriesView);

			seriesView.Children.Add(new SeriesView(series1, area));
			seriesView.Children.Add(new SeriesView(series2, area));

			InvokePrivateMethod(area, "ResetSeries");

			var seriesViewResult = GetPrivateField(area, "_seriesViews") as AbsoluteLayout;

			Assert.NotNull(seriesViewResult);
			Assert.Equal(0, series1.PointsCount);
			Assert.Equal(0, series2.PointsCount);
			Assert.Empty(seriesViewResult.Children);
		}

		[Fact]
		public void InternalCreateSegment_Test()
		{
			var chart = new SfPolarChart();
			var area = new PolarChartArea(chart);
			var series1 = new PolarLineSeries() { SegmentsCreated = false };
			var series2 = new PolarLineSeries() { SegmentsCreated = false };
			var seriesView = GetPrivateField(area, "_seriesViews") as AbsoluteLayout;

			Assert.NotNull(seriesView);

			seriesView.Children.Add(new SeriesView(series1, area));
			seriesView.Children.Add(new SeriesView(series2, area));

			InvokePrivateMethod(area, "InternalCreateSegments", series2);

			Assert.True(series2.SegmentsCreated);
			Assert.False(series1.SegmentsCreated);
		}

		#endregion

		#region PyramidChartArea methods

		[Fact]
		public void Pyramid_UpdateLegendItems()
		{
			var chart = new SfPyramidChart();
			var area = new PyramidChartArea(chart);

			SetPrivateField(area, "_shouldPopulateLegendItems", true);
			area.UpdateLegendItems();

			var result = GetPrivateField(area, "_shouldPopulateLegendItems");

			Assert.NotNull(result);
			Assert.False((bool)result);
		}

		[Fact]
		public void ToggleLegendItem_PyramidChart()
		{
			var chart = new SfPyramidChart();
			((IPyramidChartDependent)chart).SegmentsCreated = true;
			var area = new PyramidChartArea(chart);
			var legendItem = new LegendItem() { IsToggled = true };
			var legend = new ChartLegend() { IsVisible = true, ToggleSeriesVisibility = true };
			area._legend = legend;
			area._legend.ToggleVisibility = true;

			InvokePrivateMethod(area, "ToggleLegendItem", legendItem);

			Assert.False(((IPyramidChartDependent)chart).SegmentsCreated);
		}

		#endregion

	}
}
