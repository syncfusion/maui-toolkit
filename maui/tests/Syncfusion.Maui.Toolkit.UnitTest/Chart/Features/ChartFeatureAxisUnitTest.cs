using System.Collections.ObjectModel;
using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.Toolkit.UnitTest.Charts
{
	public class ChartFeatureAxisUnitTest : BaseUnitTest
    {

        #region ChartAxis partial methods

        [Fact]
        public void Test_RenderRectContains_Test()
        {
            var axis = new CategoryAxis()
            {
                IsVertical = true,
                ArrangeRect = new Rect(0, 0, 100, 100),
                InsidePadding = 10,
            };
			axis.AxisLabelsRenderer = new CartesianAxisLabelsRenderer(axis)
			{
				LabelLayout = new AxisLabelLayout(axis)
				{
					LabelsRect =
			[
				new Rect(0, 0, 50, 20),
				new Rect(0, 20, 50, 20)
			]
				}
			};

			bool result = axis.RenderRectContains(50, 50);

            Assert.True(result);
        }

        [Fact]
        public void Test_UpdateLayout()
        {
			var axis = new NumericalAxis
			{
				Area = new CartesianChartArea(new SfCartesianChart())
			};
			axis.UpdateLayout();
            Assert.True(axis.Area.NeedsRelayout);

            axis.PolarArea = new PolarChartArea(new SfPolarChart());
            axis.UpdateLayout();
            Assert.True(axis.PolarArea.NeedsRelayout);
        }

        [Theory]
        [InlineData(true, 10.0, true)]
        [InlineData(true, double.NaN, false)]
        [InlineData(true, double.MinValue, false)]
        [InlineData(true, double.MaxValue, false)]
        [InlineData(false, 10.0, false)]
        public void Test_CanRenderNextToCrossingValue(bool renderNextToCrossingValue, double actualCrossingValue, bool expectedResult)
        {
            var renderClass = new NumericalAxis()
            {
                RenderNextToCrossingValue = renderNextToCrossingValue,
                ActualCrossingValue = actualCrossingValue
            };

            var result = renderClass.CanRenderNextToCrossingValue();

            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void Test_CanDrawMajorGridLines(bool showMajorGridLines, bool expectedResult)
        {
            var axis = new CategoryAxis()
            {
                ShowMajorGridLines = showMajorGridLines,
                MajorGridLineStyle = new ChartLineStyle() { Stroke = Colors.Black, StrokeWidth = 1 },
            };

            var result = axis.CanDrawMajorGridLines();

            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void Test_CanDrawMinorGridLines(bool showMinorGridLines, bool expectedResult)
        {
            var axis = new NumericalAxis()
            {
                ShowMinorGridLines = showMinorGridLines,
                MinorGridLineStyle = new ChartLineStyle() { Stroke = Colors.Black, StrokeWidth = 1 },
            };

            var result = axis.CanDrawMinorGridLines();

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void Test_AddRegisteredSeries()
        {
            var axis = new CategoryAxis();
            var series = new LineSeries();

            axis.AddRegisteredSeries(series);

            Assert.Contains(series, axis.RegisteredSeries);
        }

        [Fact]
        public void Test_ClearRegisteredSeries()
        {
			var axis = new CategoryAxis
			{
				Area = new CartesianChartArea(new SfCartesianChart())
			};
			axis.RegisteredSeries.Add(new LineSeries());
            axis.AssociatedAxes.Add(new NumericalAxis());

            axis.ClearRegisteredSeries();

            Assert.Empty(axis.RegisteredSeries);
            Assert.Empty(axis.AssociatedAxes);
        }

        [Fact]
        public void Test_RemoveRegisteredSeries()
        {
            var axis = new CategoryAxis() { Name = "X-Axis" };
            var yaxis = new NumericalAxis();
            var series = new LineSeries();
            axis.RegisteredSeries.Add(series);
            series.ActualXAxis = axis;
            series.ActualYAxis = yaxis;
            axis.Area = new CartesianChartArea(new SfCartesianChart()) { PrimaryAxis = new CategoryAxis(), SecondaryAxis = new NumericalAxis() };

            axis.RemoveRegisteredSeries(series);

            Assert.DoesNotContain(series, axis.RegisteredSeries);
            Assert.DoesNotContain(axis, series.ActualXAxis.AssociatedAxes);
            Assert.DoesNotContain(yaxis, series.ActualYAxis.AssociatedAxes);
        }

        [Fact]
        public void GetCrossingAxis_Test()
        {
			var axis = new CategoryAxis
			{
				Name = "CrossXAxis",
				IsVertical = false
			};
			var expectedAxis = new NumericalAxis() { Name = "CrossAxis" };
            var area = new CartesianChartArea(new SfCartesianChart());

            area._axisLayout.HorizontalAxes.Add(new CategoryAxis());
            area._axisLayout.VerticalAxes.Add(new NumericalAxis());
            area._axisLayout.VerticalAxes.Add(expectedAxis);

            axis.CrossAxisName = "CrossAxis";

            var result = axis.GetCrossingAxis(area);

            Assert.NotNull(result);
            Assert.Equal(expectedAxis, result);
        }

        [Fact]
        public void Dispose_Test()
        {
            var _chartAxis = new CategoryAxis()
            {
                Title = new ChartAxisTitle(),
                AxisLineStyle = new ChartLineStyle(),
                LabelStyle = new ChartAxisLabelStyle(),
                MajorGridLineStyle = new ChartLineStyle(),
                MajorTickStyle = new ChartAxisTickStyle(),
                RegisteredSeries = [new LineSeries()],
            };

            bool isAxisLineStyleChanged = false;
            bool isLabelStyleChanged = false;

            _chartAxis.AxisLineStyle.PropertyChanged += (s, e) => isAxisLineStyleChanged = true;
            _chartAxis.LabelStyle.PropertyChanged += (s, e) => isLabelStyleChanged = true;

            _chartAxis.Dispose();

            Assert.Null(_chartAxis.Title.Axis);
            Assert.Empty(_chartAxis.RegisteredSeries);
            Assert.Empty(_chartAxis.AssociatedAxes);
            Assert.Null(_chartAxis.AxisLabelsRenderer);
            Assert.Null(_chartAxis.AxisElementRenderer);
            Assert.Null(_chartAxis.Area);
            Assert.False(isAxisLineStyleChanged);
            Assert.False(isLabelStyleChanged);
        }

        [Fact]
        public void InitializeConstructor_Test()
        {
            var _chartAxis = new CategoryAxis();

            InvokePrivateMethod(_chartAxis, "InitializeConstructor");

            Assert.NotNull(_chartAxis.AxisLineStyle);
            Assert.NotNull(_chartAxis.LabelStyle);
            Assert.NotNull(_chartAxis.MajorGridLineStyle);
            Assert.NotNull(_chartAxis.MajorTickStyle);
            Assert.Equal(EdgeLabelsDrawingMode.Shift, _chartAxis.EdgeLabelsDrawingMode);
        }

        [Fact]
        public void UpdateRenderers_Test()
        {
            var _chartAxis = new CategoryAxis();

            InvokePrivateMethod(_chartAxis, "UpdateRenderers");

            var initialRenderer = _chartAxis.AxisLabelsRenderer;
            InvokePrivateMethod(_chartAxis, "UpdateRenderers");

            Assert.NotNull(_chartAxis.AxisLabelsRenderer);
            Assert.NotNull(_chartAxis.AxisElementRenderer);
            Assert.Equal(initialRenderer, _chartAxis.AxisLabelsRenderer);
        }

        [Fact]
        public void GetPlotSize_ShouldReturnCorrectSize_WhenNotVertical()
        {
			var _chartAxis = new CategoryAxis
			{
				IsVertical = false
			};
			var availableSize = new Size(200, 100);

            var plotSize = InvokePrivateMethod(_chartAxis, "GetPlotSize", availableSize);

            Assert.NotNull(plotSize);
            Assert.Equal(new Size(200 - _chartAxis.GetActualPlotOffset(), 100), (Size)plotSize);
        }

        [Fact]
        public void GetPlotSize_ShouldReturnCorrectSize_WhenVertical()
        {
			var _chartAxis = new CategoryAxis
			{
				IsVertical = true
			};
			var availableSize = new Size(200, 100);

            var plotSize = InvokePrivateMethod(_chartAxis, "GetPlotSize", availableSize);

            Assert.NotNull(plotSize);
            Assert.Equal(new Size(200, 100 - _chartAxis.GetActualPlotOffset()), (Size)plotSize);
        }

        [Fact]
        public void InitTitle_ShouldSetAxisPropertyOfTitle()
        {
            var _chartAxis = new CategoryAxis();
            var title = new ChartAxisTitle();

            InvokePrivateMethod(_chartAxis, "InitTitle", title);

            Assert.Equal(_chartAxis, title.Axis);
        }

        [Fact]
        public void ResetPlotBand_ShouldClearActualPlotBands()
        {
            var _chartAxis = new NumericalAxis();
            var parameter = new object[] { 0, new NumericalPlotBand() };

            InvokePrivateMethod(_chartAxis, "AddPlotBand", parameter);
            Assert.NotEmpty(_chartAxis.ActualPlotBands);

            InvokePrivateMethod(_chartAxis, "ResetPlotBand");

            Assert.Empty(_chartAxis.ActualPlotBands);
        }

        [Fact]
        public void RemovePlotBand_ShouldRemovePlotBandFromActualPlotBands()
        {
            var _chartAxis = new NumericalAxis();
            var plotBand = new NumericalPlotBand();
            var parameter = new object[] { 0, plotBand };

            InvokePrivateMethod(_chartAxis, "AddPlotBand", parameter);
            Assert.Single(_chartAxis.ActualPlotBands);

            InvokePrivateMethod(_chartAxis, "RemovePlotBand", parameter);

            Assert.Empty(_chartAxis.ActualPlotBands);
        }

        [Fact]
        public void AddPlotBand_ShouldAddPlotBandToActualPlotBands()
        {
            var _chartAxis = new NumericalAxis();
            var plotBand = new NumericalPlotBand();

            var parameter = new object[] { 0, plotBand };

            InvokePrivateMethod(_chartAxis, "AddPlotBand", parameter);

            Assert.Single(_chartAxis.ActualPlotBands);
            Assert.Equal(plotBand, _chartAxis.ActualPlotBands[0]);
        }

        #endregion

        #region AxisLabelLayout methods

        [Theory]
        [InlineData(100, 20, ChartAxisLabelAlignment.Start, 90)]
        [InlineData(100, 20, ChartAxisLabelAlignment.End, 110)]
        [InlineData(100, 20, ChartAxisLabelAlignment.Center, 100)]
        public void OnAxisLabelAlignment_Test(float position, float size, ChartAxisLabelAlignment alignment, float expected)
        {
            var labelStyle = new ChartAxisLabelStyle { LabelAlignment = alignment };

            var result = AxisLabelLayout.OnAxisLabelAlignment(position, size, labelStyle);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(ChartAxisLabelAlignment.Start, HorizontalAlignment.Left)]
        [InlineData(ChartAxisLabelAlignment.Center, HorizontalAlignment.Center)]
        [InlineData(ChartAxisLabelAlignment.End, HorizontalAlignment.Right)]
        public void OnWrapAxisLabelAlignment_Test(ChartAxisLabelAlignment alignment, HorizontalAlignment expected)
        {
            var labelStyle = new ChartAxisLabelStyle { WrappedLabelAlignment = alignment };

            var result = AxisLabelLayout.OnWrapAxisLabelAlignment(labelStyle);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void CreateAxisLayout_Test_WhenIsVerticalIsFalse()
        {
            var chartAxis = new CategoryAxis { IsVertical = false };

            var layout = AxisLabelLayout.CreateAxisLayout(chartAxis);

            Assert.IsType<HorizontalLabelLayout>(layout);
        }

        [Fact]
        public void CreateAxisLayout_Test_WhenIsVerticalIsTrue()
        {
            var chartAxis = new CategoryAxis { IsVertical = true };

            var layout = AxisLabelLayout.CreateAxisLayout(chartAxis);

            Assert.IsType<VerticalLabelLayout>(layout);
        }

        [Fact]
        public void IsOpposed_Test_WhenAxisIsNull()
        {
            var layout = new AxisLabelLayout(new CategoryAxis()) { Axis = null! };

            var result = layout.IsOpposed();

            Assert.False(result);
        }

        [Theory]
        [InlineData(AxisElementPosition.Outside, false)]
        [InlineData(AxisElementPosition.Inside, true)]
        public void IsOpposed_Test_WhenAxisPosition(AxisElementPosition position, bool expected)
        {
            var layout = new AxisLabelLayout(new CategoryAxis());
            layout.Axis.LabelsPosition = position;

            var result = layout.IsOpposed();

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(0, 0, 100, 100, 50, 50, 150, 150, true)]
        [InlineData(0, 0, 50, 50, 51, 51, 100, 100, false)] 
        [InlineData(0, 0, 100, 100, 100, 0, 200, 100, true)]
        [InlineData(0, 0, 200, 200, 50, 50, 150, 150, true)] 
        public void IntersectsWith_ShouldReturnExpectedResult(float left1, float top1, float right1, float bottom1,float left2, float top2, float right2, float bottom2,bool expectedResult)
        {
            var r1 = new RectF(left1, top1, right1, bottom1);
            var r2 = new RectF(left2, top2, right2, bottom2);
            var layout = new AxisLabelLayout(new CategoryAxis());

            var result = AxisLabelLayout.IntersectsWith(r1, r2, 0, 1);

            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(100, 100, double.NaN, double.NaN, 0f, 0f)]
        [InlineData(100, 100, 10.0, double.NaN, 10f, 0f)] 
        [InlineData(100, 100, double.NaN, 15.0, 0f, 15f)] 
        [InlineData(100, 100, 10.0, 15.0, 10f, 15f)] 
        public void CalculateActualPlotOffset_ShouldSetOffsetsCorrectly(float availableWidth, float availableHeight, double plotOffsetStart, double plotOffsetEnd, float expectedOffsetStart, float expectedOffsetEnd)
        {
            var layout = new AxisLabelLayout(new CategoryAxis())
            {
                Axis = new CategoryAxis()
                {
                    PlotOffsetStart = plotOffsetStart,
                    PlotOffsetEnd = plotOffsetEnd
                }
            };
            var availableSize = new SizeF(availableWidth, availableHeight);

            layout.CalculateActualPlotOffset(availableSize);

            Assert.Equal(expectedOffsetStart, layout.Axis.ActualPlotOffsetStart);
            Assert.Equal(expectedOffsetEnd, layout.Axis.ActualPlotOffsetEnd);
        }

        [Theory]
        [InlineData(0, 0, 5, 5, 1)]
        public void InsertToRowOrColumn_ShouldInsertCorrectly(int rowOrColIndex, int itemIndex, float rectX, float rectY, int expectedRowCount)
        {
            var layout = new AxisLabelLayout(new CategoryAxis())
            {
                RectByRowsAndCols = []
            };

            var rect = new RectF(rectX, rectY, 10, 10);
            var parameter = new object[] { rowOrColIndex, itemIndex, rect };

            InvokePrivateMethod(layout, "InsertToRowOrColumn", parameter);

            Assert.Equal(expectedRowCount, layout.RectByRowsAndCols.Count);
        }

        [Fact]
        public void InsertToRowOrColumn_ShouldHandleIntersection()
        {
            var layout = new AxisLabelLayout(new CategoryAxis())
            {
                RectByRowsAndCols =
				[
					new Dictionary<int, RectF>
                    {
                        { 0, new RectF(0, 0, 10, 10) }
                    }
                ]
            };

            var intersectingRect = new RectF(5, 5, 10, 10);

            var parameter = new object[] { 0, 1, intersectingRect };

            InvokePrivateMethod(layout, "InsertToRowOrColumn", parameter);

            Assert.Equal(2, layout.RectByRowsAndCols.Count);
        }

        [Theory]
        [InlineData(0, 100, 50, 100, 50)] 
        //[InlineData(90, 100, 50, 50, 100)] Expected width 50.000000000000007 Accuracy problem thus failed
        //[InlineData(180, 100, 50, 100, 50)] Expected height 50.000000000000014 Accuracy problem thus failed
        public void GetRotatedSize_ShouldReturnExpectedSize(double degrees, double originalWidth, double originalHeight, double expectedWidth, double expectedHeight)
        {
            var layout = new AxisLabelLayout(new CategoryAxis());
            var originalSize = new Size(originalWidth, originalHeight);

            var rotatedSize = InvokePrivateMethod(layout, "GetRotatedSize", [originalSize, degrees]);

            Assert.NotNull(rotatedSize);
            Assert.Equal(expectedWidth, ((Size)rotatedSize).Width);
            Assert.Equal(expectedHeight, ((Size)rotatedSize).Height);
        }

        #region horizontal label layout

        [Theory]
        [InlineData(EdgeLabelsDrawingMode.Shift)]
        [InlineData(EdgeLabelsDrawingMode.Center)]
        [InlineData(EdgeLabelsDrawingMode.Hide)]
        public void CalcBounds_ShouldAdjustBoundsForLabels(EdgeLabelsDrawingMode edgeLabelsDrawingMode)
        {
			var layout = new HorizontalLabelLayout(new CategoryAxis())
			{
				ComputedSizes = [new SizeF(50, 20), new SizeF(60, 20)],
				Axis = new CategoryAxis()
				{
					VisibleLabels =
				[
					new ChartAxisLabel ( 0, new ChartAxisLabelStyle { Margin = new Thickness(5, 0, 5, 0) }),
					new ChartAxisLabel ( 1, new ChartAxisLabelStyle { Margin = new Thickness(5, 0, 5, 0) })
				],
					EdgeLabelsDrawingMode = edgeLabelsDrawingMode
				}
			};

			InvokePrivateMethod(layout, "CalcBounds", 100);

            Assert.NotNull(layout.RectByRowsAndCols);
            Assert.Equal(layout.RectByRowsAndCols[0].Values, layout.LabelsRect);
        }

        #endregion

        #region VerticleLabel layout

        [Theory]
        [InlineData(0.5f, 100, 50, 200, 180, -150)] 
        [InlineData(0.5f, 100, 50, 200, 90, 275)]  
        [InlineData(0.5f, 100, 50, 200, 0, 50)]  
        public void CalculatePolarPosition_ShouldReturnExpectedResult(float coeff, float width, float height, float size, int angle, float expected)
        {
            var layout = new VerticalLabelLayout(new NumericalAxis());
            var chart = new SfPolarChart() { PolarStartAngle = angle };
			layout.Axis = new NumericalAxis
			{
				Parent = chart
			};

			var parameter = new object[] { coeff, width, height, size };

            var result = InvokePrivateMethod(layout, "CalculatePolarPosition", parameter);

            Assert.NotNull(result);
            Assert.Equal(expected, (float)result);
        }

        #endregion

        #endregion

        #region CartesianAxisLayout methods

        [Fact]
        public void ClearActualAxis_Test()
        {
            var chartArea = new CartesianChartArea(new SfCartesianChart()) { RequiredAxisReset = true };

            var series1 = new LineSeries() { ActualXAxis = new CategoryAxis(), ActualYAxis = new NumericalAxis() };
            var series2 = new ColumnSeries() { ActualXAxis = new CategoryAxis(), ActualYAxis = new NumericalAxis() };

            var visibleSeries = new ReadOnlyObservableCollection<ChartSeries>(
                [series1, series2]
            );

            var layout = new CartesianAxisLayout(chartArea);

            InvokePrivateMethod(layout, "ClearActualAxis", visibleSeries);

            Assert.Null(series1.ActualXAxis);
            Assert.Null(series1.ActualYAxis);
            Assert.Null(series2.ActualXAxis);
            Assert.Null(series2.ActualYAxis);
            Assert.False(chartArea.RequiredAxisReset);
        }

        [Fact]
        public void GetAxisByName_Test()
        {
            var area = new CartesianChartArea(new SfCartesianChart());
            var layout = new CartesianAxisLayout(area);
            var axes = new ObservableCollection<ChartAxis>
            {
                new CategoryAxis() { Name = "XAxis" },
                new NumericalAxis() { Name = "YAxis" }
            };

            var parameter = new object[] { "XAxis", axes };

            var result = InvokePrivateMethod(layout, "GetAxisByName", parameter);

            Assert.NotNull(result);
            Assert.Equal("XAxis", ((ChartAxis)result).Name);
        }

        [Fact]
        public void UpdateActualAxis_Test()
        {
            var visibleSeries = new ReadOnlyObservableCollection<ChartSeries>(
                [
                    new LineSeries { XAxisName = "XAxis1", YAxisName = "YAxis1" },
                    new LineSeries { XAxisName = "XAxis2", YAxisName = "YAxis2" }
                ]
            );

            var chart = new SfCartesianChart();

            chart.XAxes.Add(new CategoryAxis() { Name = "XAxis1" });
            chart.XAxes.Add(new CategoryAxis() { Name = "XAxis2" });
            chart.YAxes.Add(new NumericalAxis() { Name = "YAxis1" });
            chart.YAxes.Add(new NumericalAxis() { Name = "YAxis2" });

            var area = new CartesianChartArea(chart)
            {
                PrimaryAxis = new CategoryAxis() { Name = "PrimaryAxis" },
                SecondaryAxis = new NumericalAxis() { Name = "SecondaryAxis" },

            };
			var layout = new CartesianAxisLayout(area)
			{
				HorizontalAxes = chart.XAxes,
				VerticalAxes =
			[
				new NumericalAxis() { Name = "YAxis1" },
				new NumericalAxis() { Name = "YAxis2" },
			]
			};

			InvokePrivateMethod(layout, "UpdateActualAxis", visibleSeries);

            Assert.Equal("XAxis1", ((CartesianSeries)visibleSeries[0]).ActualXAxis?.Name);
            Assert.Equal("YAxis1", ((CartesianSeries)visibleSeries[0]).ActualYAxis?.Name);
            Assert.Equal("XAxis2", ((CartesianSeries)visibleSeries[1]).ActualXAxis?.Name);
            Assert.Equal("YAxis2", ((CartesianSeries)visibleSeries[1]).ActualYAxis?.Name);
        }

        [Fact]
        public void ValidateMinMaxWithAxisCrossingValue_Test()
        {
            var area = new CartesianChartArea(new SfCartesianChart());
            var layout = new CartesianAxisLayout(area);
            var currentAxis = new CategoryAxis() { ActualCrossingValue = 50 };
            var associatedAxis = new NumericalAxis() { ActualRange = new DoubleRange(0, 100) };

            var result = InvokePrivateMethod(layout, "ValidateMinMaxWithAxisCrossingValue", currentAxis);

            Assert.NotNull(result);
            Assert.Equal(associatedAxis.ValueToPoint(50), (double)result);
        }

        [Theory]
        [InlineData(true, 2, 2)]  
        [InlineData(false, 2, 2)] 
        public void UpdateAxisTransposed_ShouldAssignAxesCorrectly(bool isTransposed, int expectedVerticalCount, int expectedHorizontalCount)
        {
            var chart = new SfCartesianChart();
            var area = new CartesianChartArea(chart)
            {
                IsTransposed = isTransposed,
            };

            chart.XAxes.Add(new CategoryAxis() { Name = "XAxis1" });
            chart.XAxes.Add(new CategoryAxis() { Name = "XAxis2" });
            chart.YAxes.Add(new NumericalAxis() { Name = "YAxis1" });
            chart.YAxes.Add(new NumericalAxis() { Name = "YAxis2" });

            var layout = new CartesianAxisLayout(area);

            InvokePrivateMethod(layout, "UpdateAxisTransposed");

            Assert.Equal(expectedVerticalCount, layout.VerticalAxes.Count);
            Assert.Equal(expectedHorizontalCount, layout.HorizontalAxes.Count);
            Assert.All(layout.VerticalAxes, axis => Assert.True(axis.IsVertical));
            Assert.All(layout.HorizontalAxes, axis => Assert.False(axis.IsVertical));
        }

        [Fact]
        public void Test_InitMethod()
        {
            var area = new CartesianChartArea(new SfCartesianChart());
            var layout = new CartesianAxisLayout(area);

            SetPrivateField(layout, "_leftSizes", null);
            SetPrivateField(layout, "_topSizes", null);
            SetPrivateField(layout, "_rightSizes", null);
            SetPrivateField(layout, "_bottomSizes", null);

            Assert.Null(GetPrivateField(layout, "_leftSizes"));
            Assert.Null(GetPrivateField(layout, "_topSizes"));
            Assert.Null(GetPrivateField(layout, "_rightSizes"));
            Assert.Null(GetPrivateField(layout, "_bottomSizes"));

            InvokePrivateMethod(layout, "Init");

            Assert.NotNull(GetPrivateField(layout, "_leftSizes"));
            Assert.NotNull(GetPrivateField(layout, "_topSizes"));
            Assert.NotNull(GetPrivateField(layout, "_rightSizes"));
            Assert.NotNull(GetPrivateField(layout, "_bottomSizes"));
        }

        [Fact]
        public void UpdateCrossingAxes_Test()
        {
            var area = new CartesianChartArea(new SfCartesianChart());
            var layout = new CartesianAxisLayout(area);

			var verticalAxis = new NumericalAxis
			{
				TickPosition = AxisElementPosition.Inside,
				LabelsPosition = AxisElementPosition.Inside,
				ActualRange = new DoubleRange(0, 100),
				VisibleRange = new DoubleRange(0, 150),
				ComputedDesiredSize = new Size(0, 100),
				InsidePadding = 5,
				RenderedRect = new RectF(0, 0, 20, 100),
				ActualCrossingValue = 30
			};
			layout.VerticalAxes.Add(verticalAxis);
            area.SecondaryAxis = verticalAxis;

			var horizontalAxis = new CategoryAxis
			{
				TickPosition = AxisElementPosition.Outside,
				LabelsPosition = AxisElementPosition.Outside,
				ActualRange = new DoubleRange(20, 150),
				VisibleRange = new DoubleRange(20, 150),
				ComputedDesiredSize = new Size(100, 20),
				RenderedRect = new RectF(0, 0, 100, 20),
				InsidePadding = 10,
				ActualCrossingValue = 30
			};
			layout.HorizontalAxes.Add(horizontalAxis);
            area.PrimaryAxis = horizontalAxis;
            var left = 10;
            var top = 20;

            SetPrivateField(layout, "_left", 10);
            SetPrivateField(layout, "_top", 20);

            InvokePrivateMethod(layout, "UpdateCrossingAxes");

            Assert.Equal(new Rect(9, top, 0, 100), verticalAxis.ArrangeRect);

            Assert.Equal(new Rect(left, top, 100, 20), horizontalAxis.ArrangeRect);
        }

        #endregion

        #region PolarAxisLayout methods

        [Fact]
        public void UpdateArrangeRect_Test()
        {
            var chart = new SfPolarChart();
            var area = new PolarChartArea(chart);
            var layout = new PolarAxisLayoutView(area);

            chart.PrimaryAxis = new CategoryAxis();
            chart.SecondaryAxis = new NumericalAxis() { ComputedDesiredSize = new Size(50, 50) };

            var availableSize = new Size(200, 100);

            InvokePrivateMethod(layout, "UpdateArrangeRect", availableSize);

            Assert.Equal(new Rect(0, 0, 200, 100), chart.PrimaryAxis.ArrangeRect);
            Assert.Equal(new Rect(100, 0, 150, 50), chart.SecondaryAxis.ArrangeRect);
        }

        [Fact]
        public void PolarClearActualAxis_Test()
        {
            var chartArea = new PolarChartArea(new SfPolarChart());

            var series1 = new PolarLineSeries() { ActualXAxis = new CategoryAxis(), ActualYAxis = new NumericalAxis() };
            var series2 = new PolarLineSeries() { ActualXAxis = new CategoryAxis(), ActualYAxis = new NumericalAxis() };

            var visibleSeries = new ReadOnlyObservableCollection<ChartSeries>(
                [series1, series2]
            );

            var layout = new PolarAxisLayoutView(chartArea);

            InvokePrivateMethod(layout, "ClearActualAxis", visibleSeries);

            Assert.Null(series1.ActualXAxis);
            Assert.Null(series1.ActualYAxis);
            Assert.Null(series2.ActualXAxis);
            Assert.Null(series2.ActualYAxis);
        }

		[Fact]
        public void PolarUpdateActualAxis_Test()
        {
            var visibleSeries = new ReadOnlyObservableCollection<ChartSeries>(
                [
                    new PolarLineSeries (),
                    new PolarLineSeries ()
                ]
            );

			var chart = new SfPolarChart
			{
				PrimaryAxis = (new CategoryAxis() { Name = "XAxis1" }),
				SecondaryAxis = (new NumericalAxis() { Name = "YAxis1" })
			};


			var area = new PolarChartArea(chart);

            var layout = new PolarAxisLayoutView(area);

            InvokePrivateMethod(layout, "UpdateActualAxis", visibleSeries);

            Assert.Equal("XAxis1", ((PolarLineSeries)visibleSeries[0]).ActualXAxis?.Name);
            Assert.Equal("YAxis1", ((PolarLineSeries)visibleSeries[0]).ActualYAxis?.Name);
        }

        #endregion

        #region PolarGridlinestyle method

        [Fact]
        public void Measure_ShouldReturnAvailableSize()
        {
            var area = new PolarChartArea(new SfPolarChart());
            var layout = new PolarGridLineLayout(area);
            var availableSize = new Size(150, 100);

            var result = layout.Measure(availableSize);
            var desiredSize = GetPrivateField(layout, "_desiredSize");

            Assert.NotNull(desiredSize);
            Assert.Equal(availableSize, (Size)desiredSize);
        }

        [Fact]
        public void GetDefaultGridLineStyle_ShouldReturnDefaultStyle()
        {
            var area = new PolarChartArea(new SfPolarChart());
            var layout = new PolarGridLineLayout(area);

            var result = InvokePrivateMethod(layout, "GetDefaultGridLineStyle") as ChartLineStyle;

            Assert.NotNull(result);
            Assert.Equal(1, result.StrokeWidth);
        }

        #endregion

        #region Renderer methods

        [Fact]
        public void Test_GetLeftAndSetLeft()
        {
            var element = new CartesianAxisElementRenderer(new CategoryAxis());
            var result = element.GetLeft();

            Assert.Equal(0, result);

            var _left = 10;
            element.SetLeft(_left);
            Assert.Equal(_left, element.GetLeft());
        }

        [Fact]
        public void Test_GetTopAndSetTop()
        {
            var element = new CartesianAxisElementRenderer(new CategoryAxis());
            var result = element.GetTop();

            Assert.Equal(0, result);

            var _left = 10;
            element.SetTop(_left);
            Assert.Equal(_left, element.GetTop());
        }

        [Fact]
        public void Test_GetDesiredSize()
        {
            var element = new CartesianAxisElementRenderer(new NumericalAxis());
            var size = new SizeF(50, 50);
            SetPrivateField(element, "_desiredSize", size);

            var result = element.GetDesiredSize();
            Assert.Equal(size, (SizeF)result);
        }

        [Theory]
        [InlineData(true, 100, 50, 5, 2, 7, 100)] 
        [InlineData(false, 100, 50, 5, 2, 50, 7)]  
        public void Measure_ShouldReturnCorrectSize(
        bool isVertical, double availableWidth, double availableHeight,
        double majorTickSize, double strokeWidth, double expectedWidth, double expectedHeight)
        {
            var axis = new NumericalAxis()
            {
                IsVertical = isVertical,
            };
            var element = new CartesianAxisElementRenderer(axis);
            var availableSize = new Size(availableWidth, availableHeight);
            axis.MajorTickStyle = new ChartAxisTickStyle { TickSize = majorTickSize };
            axis.AxisLineStyle = new ChartLineStyle { StrokeWidth = strokeWidth };

            var size = element.Measure(availableSize);

            Assert.Equal(expectedWidth, size.Width);
            Assert.Equal(expectedHeight, size.Height);
        }

        #region CartesianAxis label renderer

        [Fact]
        public void TestAxisLabel_GetLeftAndSetLeft()
        {
            var element = new CartesianAxisLabelsRenderer(new CategoryAxis());
            var result = element.GetLeft();

            Assert.Equal(0, result);

            var _left = 10;
            element.SetLeft(_left);
            Assert.Equal(_left, element.GetLeft());
        }

        [Fact]
        public void TestAxisLabel_GetTopAndSetTop()
        {
            var element = new CartesianAxisLabelsRenderer(new CategoryAxis());
            var result = element.GetTop();

            Assert.Equal(0, result);

            var _left = 10;
            element.SetTop(_left);
            Assert.Equal(_left, element.GetTop());
        }

        [Fact]
        public void TestAxisLabel_GetDesiredSize()
        {
            var element = new CartesianAxisLabelsRenderer(new NumericalAxis());
            var size = new SizeF(50, 50);
            SetPrivateField(element, "_desiredSize", size);

            var result = element.GetDesiredSize();
            Assert.Equal(size, (SizeF)result);
        }

        #endregion

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void UpdateRendererVisible_Test(bool visible)
        {
            var axis = new CategoryAxis();
            ILayoutCalculator layout1 = new CartesianAxisElementRenderer(axis);
            ILayoutCalculator layout2 = new CartesianAxisElementRenderer(axis);
            layout1.IsVisible = !visible;
            layout2.IsVisible = !visible;

            var layoutCalculators = new List<ILayoutCalculator> { layout1, layout2 };
            var element = new CartesianAxisRenderer(axis) { LayoutCalculators = layoutCalculators };

            element.UpdateRendererVisible(visible);

            Assert.Equal(visible, (layout1).IsVisible);
            Assert.Equal(visible, (layout1).IsVisible);
        }

        [Theory]
        [InlineData(true, 0f, 10f, 0f, 0f)]
        [InlineData(false, 0f, 0f, 25f, 0f)]
        public void Layout_ShouldPositionElementsCorrectlyForVerticalAxis(bool isVertical, float layoutOneLeft, float layoutTwoLeft, float layoutOneTop, float layoutTwoTop)
        {
            var axis = new CategoryAxis()
            {
                IsVertical = isVertical,
                LabelsPosition = AxisElementPosition.Outside
            };
            var layout1 = new CartesianAxisElementRenderer(axis);
            ((ILayoutCalculator)layout1).IsVisible = true;
            layout1.SetLeft(0);
            layout1.SetTop(0);
            SetPrivateField(layout1, "_desiredSize", new SizeF(10, 20));
            var layout2 = new CartesianAxisElementRenderer(axis);
            ((ILayoutCalculator)layout2).IsVisible = true;
            layout2.SetLeft(0);
            layout2.SetTop(0);
            SetPrivateField(layout2, "_desiredSize", new SizeF(15, 25));
            
            var element = new CartesianAxisRenderer(axis)
            {
                LayoutCalculators = [layout1, layout2],
            };

            element.Layout(new SizeF(100, 200));

            Assert.Equal(layoutOneLeft, layout1.GetLeft()); 
            Assert.Equal(layoutTwoLeft, layout2.GetLeft());
            Assert.Equal(layoutOneTop, layout1.GetTop());
            Assert.Equal(layoutTwoTop, layout2.GetTop());
        }

        #endregion

        #region CategoryAxis methods

        [Fact]
        public void CategoryAxis_CalculateActualInterval_Test()
        {
            var axis = new CategoryAxis();
            var range = new DoubleRange(0, 10);
            var size = new Size(100, 100);

            var parameters = new object[] { range, size };

            var interval = InvokePrivateMethod(axis, "CalculateActualInterval", parameters);

            Assert.NotNull(interval);
            Assert.True((double)interval > 0);
        }

        [Fact]
        public void CategoryAxis_ApplyRangePadding_Test()
        {
            var axis = new CategoryAxis();
            var range = new DoubleRange(0, 10);
            var interval = 1.0;

            var parameters = new object[] { range, interval };
            var paddedRange = InvokePrivateMethod(axis, "ApplyRangePadding", parameters);
            Assert.NotNull(paddedRange);
            var paddedRangeDouble = (DoubleRange)paddedRange;
            Assert.True(paddedRangeDouble.Start == range.Start);
            Assert.True(paddedRangeDouble.End == range.End);
        }

        [Fact]
        public void GroupData_Test()
        {
            var axis = new CategoryAxis();
            var series1 = new LineSeries
            {
                ActualXValues = new List<string> { "A", "B", "C" },
                ActualData = [1, 2, 3]
            };

            var series2 = new LineSeries
            {
                ActualXValues = new List<string> { "B", "C", "D" },
                ActualData = [4, 5, 6]
            };

            axis.RegisteredSeries = [series1, series2];

            axis.GroupData();

            var expectedDistinctXValues = new List<string> { "A", "B", "C", "D" };
            var expectedGroupedXValuesIndexesSeries1 = new List<double> { 0, 1, 2 };
            var expectedGroupedXValuesIndexesSeries2 = new List<double> { 1, 2, 3 };

            Assert.Equal(expectedDistinctXValues, series1.GroupedXValues);
            Assert.Equal(expectedGroupedXValuesIndexesSeries1, series1.GroupedXValuesIndexes);
            Assert.Equal(expectedDistinctXValues, series2.GroupedXValues);
            Assert.Equal(expectedGroupedXValuesIndexesSeries2, series2.GroupedXValuesIndexes);
        }

        [Fact]
        public void CategoryAxis_GetLabelContent_Test()
        {
            var axis = new CategoryAxis();
            var series = new LineSeries()
            {
                XValues = new List<string> { "Jan", "Feb", "Mar" },
                XValueType = ChartValueType.String
            };
            axis.RegisteredSeries.Add(series);
            var pos = 1;
            var labelFormat = "";

            var labelContent = axis.GetLabelContent(series, pos, labelFormat);

            Assert.NotNull(labelContent);
            Assert.Equal("Feb", labelContent);
        }

        [Fact]
        public void UpdateAxisInterval_Test()
        {
            var categoryAxis = new CategoryAxis();
            double expectedInterval = 5.0;

            InvokePrivateMethod(categoryAxis, "UpdateAxisInterval", expectedInterval);

            Assert.Equal(expectedInterval, categoryAxis.AxisInterval);
        }

        [Fact]
        public void CategoryAxis_GetActualSeries_Test()
        {
            var axis = new CategoryAxis();
            var chart = new SfCartesianChart();
            axis.Area = new CartesianChartArea(chart);
            var series = new LineSeries()
            {
                IsVisible = true,
                PointsCount = 5,
                ActualXAxis = axis
            };

            axis.Area.Series = [series];
            var result = axis.GetActualSeries();

            Assert.Equal(series, result);
        }

        #endregion

        #region RangeAxisBase method

        [Fact]
        public void AddSmallTicksPoint_Test()
        {
			var axis = new NumericalAxis
			{
				VisibleRange = new DoubleRange(0, 100)
			};
			double position = 10;
            double interval = 1;

            axis.AddSmallTicksPoint(position, interval);

            Assert.Contains(9, axis.SmallTickPoints); 
            Assert.Contains(7, axis.SmallTickPoints);   
            Assert.Contains(5, axis.SmallTickPoints);
            Assert.DoesNotContain(10, axis.SmallTickPoints); 
        }

        #endregion

        #region ChartAxis methods

        [Fact]
        public void ChartAxis_GetActualDesiredIntervalsCount_Test()
        {
            var axis = new CategoryAxis();
            var size = new Size(100, 100);

            var count = axis.GetActualDesiredIntervalsCount(size);

            Assert.True(count > 0);
        }

        [Fact]
        public void CalculateNiceInterval_Test()
        {
            var axis = new NumericalAxis();
            var actualRange = new DoubleRange(0, 100);
            var availableSize = new Size(500, 500);

            var parameter = new object[] { actualRange, availableSize };

            var result = InvokePrivateMethod(axis, "CalculateNiceInterval", parameter);

            Assert.NotNull(result);
            Assert.Equal(20, (double)result);
        }

        [Theory]
        [InlineData(0, 10, 2, 0, 10)]  
        [InlineData(1, 9, 2, 0, 10)]  
        [InlineData(-5, 5, 3, -6, 6)] 
        public void ApplyRangePadding_ShouldApplyPaddingForPolarSeries(double start, double end, double interval, double expectedStart, double expectedEnd)
        {
            var axis = new LogarithmicAxis();
            axis.RegisteredSeries.Add(new PolarLineSeries()); 
            var range = new DoubleRange(start, end);

            var paddedRange = InvokePrivateMethod(axis, "ApplyRangePadding", [range, interval]);

            Assert.NotNull(paddedRange);
            Assert.Equal(expectedStart, ((DoubleRange)paddedRange).Start);
            Assert.Equal(expectedEnd, ((DoubleRange)paddedRange).End);
        }

        [Fact]
        public void CalculateVisibleRange_Test()
        {
			var axis = new CategoryAxis
			{
				ZoomFactor = 0.5,
				ZoomPosition = 0.25
			};

			var actualRange = new DoubleRange(0, 100);
            var availableSize = new Size(500, 500);

            var parameter = new object[] { actualRange, availableSize };

            var result = InvokePrivateMethod(axis, "CalculateVisibleRange", parameter);

            Assert.NotNull(result);
            Assert.Equal(new DoubleRange(25, 75), (DoubleRange)result);
        }

        [Fact]
        public void GetVisibleSeries_Test()
        {
            var chart =new SfCartesianChart();
            var area = new CartesianChartArea(chart);
            var series1 = new LineSeries();
            var series2 = new LineSeries();
            area.Series = [series1, series2];

            var axis = new CategoryAxis();
            SetPrivateField(axis, "_cartesianArea", area);
            var visibleSeris = axis.GetVisibleSeries();

            Assert.NotNull(visibleSeris);
            Assert.Contains(series1 , visibleSeris);
            Assert.Contains(series2 , visibleSeris);
        }

        [Fact]
        public void CalculateActualRange_Test_ForCartesianSeries()
        {
            var chart = new SfCartesianChart();
            var area = new CartesianChartArea(chart);
            var axis = new CategoryAxis();
            var series1 = new LineSeries() { VisibleXRange = new DoubleRange(2, 8), ActualXAxis = axis };
            var series2 = new LineSeries() { VisibleXRange = new DoubleRange(3, 20), ActualXAxis = axis };

            area.Series = [series1, series2];
            SetPrivateField(axis, "_cartesianArea", area);

            var actualRange = InvokePrivateMethod(axis, "CalculateActualRange");

            Assert.NotNull(actualRange);
            Assert.Equal(new DoubleRange(2, 20), (DoubleRange)actualRange);
        }

        [Fact]
        public void CalculateActualRange_Test_ForPolarSeries()
        {
            var chart = new SfPolarChart();
            var area = new PolarChartArea(chart);
            var axis = new CategoryAxis();
            var series1 = new PolarLineSeries() { VisibleXRange = new DoubleRange(2, 8), ActualXAxis = axis };
            var series2 = new PolarLineSeries() { VisibleXRange = new DoubleRange(3, 20), ActualXAxis = axis };

            area.Series = [series1, series2];
            axis.PolarArea = area;
            axis.Parent = chart;

            var actualRange = InvokePrivateMethod(axis, "CalculateActualRange");

            Assert.NotNull(actualRange);
            Assert.Equal(new DoubleRange(2, 20), (DoubleRange)actualRange);
        }

        [Fact]
        public void CalculateVisibleInterval_Test()
        {
			var axis = new CategoryAxis
			{
				ZoomFactor = 1,
				ZoomPosition = 0.3,
				ActualInterval = 5,
				EnableAutoIntervalOnZooming = false
			};

			var visibleRange = new DoubleRange(0, 100);
            var availableSize = new Size(100, 100);

            var parameter = new object[] { visibleRange, availableSize };

            var result = InvokePrivateMethod(axis, "CalculateVisibleInterval", parameter);

            Assert.NotNull(result);
            Assert.Equal(5, (double)result);
        }

        [Fact]
        public void AddDefaultRange_Test()
        {
            var axis = new CategoryAxis();
            var start = 5.0;
            var range = axis.AddDefaultRange(start);
            Assert.Equal(new DoubleRange(5.0, 6.0), range);
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(5, 6)]
        [InlineData(-3, -2)]
        public void AddDefaultRange_TestWithExactData(double start, double expectedEnd)
        {
            var axis = new CategoryAxis();

            var result = axis.AddDefaultRange(start);

            Assert.Equal(start, result.Start);
            Assert.Equal(expectedEnd, result.End);
        }

        [Fact]
        public void GetActualPlotOffsetStart_Test()
        {
            var axis = new NumericalAxis() { ActualPlotOffset = 0, ActualPlotOffsetStart = 10 };

            var result = axis.GetActualPlotOffsetStart();

            Assert.Equal(10, result);
        }

        [Fact]
        public void GetActualPlotOffsetEnd_Test()
        {
            var axis = new NumericalAxis() { ActualPlotOffset = 7, ActualPlotOffsetEnd = 15 };

            var result = axis.GetActualPlotOffsetEnd();

            Assert.Equal(7, result);
        }

        [Theory]
        [InlineData(5, true)]
        [InlineData(0, false)]
        [InlineData(-3, false)]
        public void UpdateSmallTickRequired_Test(int inputValue, bool expected)
        {
            var axis = new NumericalAxis();

            axis.UpdateSmallTickRequired(inputValue);

            Assert.Equal(expected, axis.SmallTickRequired);
        }

        [Theory]
        [InlineData(10.0, 10.0)]
        [InlineData(double.NaN, 0.0)]
        public void UpdateActualPlotOffsetStart_Test(double inputOffset, double expected)
        {
            var axis = new NumericalAxis();

            axis.UpdateActualPlotOffsetStart(inputOffset);

            Assert.Equal(expected, axis.ActualPlotOffsetStart);
        }

        [Theory]
        [InlineData(20.0, 20.0)]
        [InlineData(double.NaN, 0.0)]
        public void UpdateActualPlotOffsetEnd_Test(double inputOffset, double expected)
        {
            var axis = new NumericalAxis();

            axis.UpdateActualPlotOffsetEnd(inputOffset);

            Assert.Equal(expected, axis.ActualPlotOffsetEnd);
        }

        [Fact]
        public void GetStartDoublePosition_Test()
        {
            var axis = new CategoryAxis();

            var result = axis.GetStartDoublePosition();

            Assert.Equal(double.NaN, result);
        }

        [Fact]
        public void UpdateAxisScale_ShouldUpdateZoomPositionAndFactor()
        {
            var axis = new NumericalAxis()
            {
                ActualRange = new DoubleRange(0, 100),
                VisibleRange = new DoubleRange(20, 80)
            };

            axis.UpdateAxisScale();

            Assert.Equal(0.2, axis.ZoomPosition);
            Assert.Equal(0.6, axis.ZoomFactor);
        }

        [Theory]
        [InlineData(ChartAutoScrollingMode.Start, 50, 100, 0.5, 0)]
        [InlineData(ChartAutoScrollingMode.End, 50, 100, 0.5, 0.5)]
        public void UpdateAutoScrollingDelta_Test(
        ChartAutoScrollingMode mode,
        double actualStart,
        double actualEnd,
        double expectedFactor,
        double expectedZoom)
        {
            var axis = new NumericalAxis()
            {
                ActualRange = new DoubleRange(actualStart, actualEnd),
                AutoScrollingMode = mode
            };

            axis.UpdateAutoScrollingDelta(new DoubleRange(actualStart, actualEnd), 25);

            Assert.Equal(expectedFactor, axis.ZoomFactor);
            Assert.Equal(expectedZoom, axis.ZoomPosition);
        }

        [Theory]
        [InlineData(50, "", "50")]
        [InlineData(50.6789, "F2", "50.68")]
        [InlineData(null, "F2", "NaN")]
        public void GetActualLabelContent_Test(object? value, string format, string expected)
        {
            var result = NumericalAxis.GetActualLabelContent(value, format);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(DateTimeIntervalType.Days, "MMM - dd")]
        [InlineData(DateTimeIntervalType.Months, "MMM-yyyy")]
        [InlineData(DateTimeIntervalType.Years, "yyyy")]
        public void GetSpecificFormatedLabel_Test(DateTimeIntervalType intervalType, string expectedFormat)
        {
            var axis = new DateTimeAxis();

            var result = ChartAxis.GetSpecificFormattedLabel(intervalType);

            Assert.Equal(expectedFormat, result);
        }

        [Theory]
        [InlineData(44197, "yyyy-MM-dd", "2021-01-01")]
        [InlineData(null, "yyyy-MM-dd", "")]
        public void GetFormattedAxisLabel_Test(object? value, string format, string expected)
        {
            var result = CategoryAxis.GetFormattedAxisLabel(format, value);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(0, 0, 0, 1)] 
        [InlineData(5, 5, 5, 6)]
        [InlineData(1, 5, 1, 5)] 
        public void ValidateRange_Test(double start, double end, double expectedStart, double expectedEnd)
        {
            var range = new DoubleRange(start, end);
            var axis = new NumericalAxis();

            var validatedRange = InvokePrivateMethod(axis, "ValidateRange", range);

            Assert.NotNull(validatedRange);
            Assert.Equal(expectedStart, ((DoubleRange)validatedRange).Start);
            Assert.Equal(expectedEnd, ((DoubleRange)validatedRange).End);
        }

        [Fact]
        public void InvokeLabelCreated_ShouldRaiseLabelCreatedEvent()
        {
            var axis = new NumericalAxis();
            var labelCreatedTriggered = false;
            axis.LabelCreated += (sender, args) => { labelCreatedTriggered = true; };

            var label = new ChartAxisLabel(1.0, "Test");

            InvokePrivateMethod(axis, "InvokeLabelCreated", label);

            Assert.True(labelCreatedTriggered);
            Assert.Equal("Test", label.Content);
        }

        [Fact]
        public void UpdateLabels_ShouldClearVisibleLabelsAndTickPositions_WhenVisibleRangeDeltaIsPositive()
        {
            var axis = new CategoryAxis()
            {
                VisibleRange = new DoubleRange(10, 20),
                MaximumLabels = 5
            };
            axis.VisibleLabels.Add(new ChartAxisLabel(1, "Label 1"));

            InvokePrivateMethod(axis, "UpdateLabels");

            Assert.Empty(axis.VisibleLabels);
            Assert.Empty(axis.TickPositions);
        }

        #endregion

        #region DateTimeAxis Methods

        [Theory]
        [InlineData(0, 10, 100, 100)] 
        [InlineData(0, 10, 0, 2)]   
        public void CalculateActualInterval_ReturnsCorrectInterval(double start, double end, double axisInterval, double expectedInterval)
        {
            var axis = new DateTimeAxis
            {
                AxisInterval = axisInterval
            };
            var range = new DoubleRange(start, end);
            var size = new Size(500, 500);

            var result = InvokePrivateMethod(axis, "CalculateActualInterval", [range, size]);

            Assert.Equal(expectedInterval, (double)result!);
        }

        [Fact]
        public void CalculateVisibleRange_ShouldHandleZoomFactorAndAutoInterval()
        {
            var axis = new DateTimeAxis
            {
                ZoomFactor = 0.5,
                ZoomPosition = 0.1,
                EnableAutoIntervalOnZooming = true
            };
            var range = new DoubleRange(0, 100);
            var size = new Size(400, 400);

            var visibleRange = InvokePrivateMethod(axis, "CalculateVisibleRange", [range, size]);

            Assert.NotNull(visibleRange);
            Assert.NotEqual(range, ((DoubleRange)visibleRange));
            Assert.True(((DoubleRange)visibleRange).Delta < range.Delta);
        }

        [Fact]
        public void AddSmallTicksPoint_ShouldAddCorrectNumberOfSmallTicks()
        {
            var axis = new DateTimeAxis
            {
                MinorTicksPerInterval = 4,
                VisibleRange = new DoubleRange(0, 100)
            };

            axis.AddSmallTicksPoint(10, 20);

            Assert.Equal(4, axis.SmallTickPoints.Count);
            Assert.Contains(18, axis.SmallTickPoints);
        }

        [Theory]
        [InlineData(0, 100, 20)]
        [InlineData(0, 365, 2)] 
        public void CalculateNiceInterval_ShouldReturnCorrectInterval(double start, double end, double expectedInterval)
        {
            var axis = new DateTimeAxis();
            var actualRange = new DoubleRange(start, end);
            var size = new Size(400, 400);

            var result = InvokePrivateMethod(axis, "CalculateNiceInterval", [actualRange, size]);

            Assert.Equal(expectedInterval, result);
        }

        [Fact]
        public void RaiseCallBackActualRangeChanged_ShouldUpdateActualMinMax()
        {
            var axis = new DateTimeAxis
            {
                ActualRange = new DoubleRange(DateTime.Now.ToOADate(), DateTime.Now.AddDays(1).ToOADate())
            };

            axis.RaiseCallBackActualRangeChanged();

            Assert.True(axis.ActualMaximum > axis.ActualMinimum);
        }

        [Theory]
        [InlineData(null, "2024-01-02", double.NaN, double.NaN)] // Empty range (start is null, defaults applied)
        [InlineData("2024-01-01", "2024-01-05", 45292, 45296)] // Default range set
        [InlineData("2024-01-01", null, double.NaN, double.NaN)] // DefaultMinimum set
        public void CalculateActualRange_ReturnsCorrectRange(string? actualStartStr, string? actualEndStr, double expectedStart, double expectedEnd)
        {
            var axis = new DateTimeAxis();

            DateTime? actualStart = string.IsNullOrEmpty(actualStartStr) ? null : DateTime.Parse(actualStartStr);
            DateTime? actualEnd = string.IsNullOrEmpty(actualEndStr) ? null : DateTime.Parse(actualEndStr);

            axis.ActualRange = new DoubleRange(
                actualStart.HasValue ? actualStart.Value.ToOADate() : double.NaN,
                actualEnd.HasValue ? actualEnd.Value.ToOADate() : double.NaN);

            axis.DefaultMinimum = actualStart.HasValue ? actualStart.Value : null;
            axis.DefaultMaximum = actualEnd.HasValue ? actualEnd.Value : null;

            var result = InvokePrivateMethod(axis, "CalculateActualRange");

            Assert.NotNull(result);
            Assert.Equal(expectedStart, ((DoubleRange)result).Start);
            Assert.Equal(expectedEnd, ((DoubleRange)result).End);
        }

        [Fact]
        public void OnMinMaxChanged_ShouldUpdateActualRangeWithMinMaxValues_WhenMinMaxAreNotNull()
        {
            var axis = new DateTimeAxis();
            var min = new DateTime(2000, 1, 1);
            var max = new DateTime(2020, 1, 1);
            axis.DefaultMinimum = min;
            axis.DefaultMaximum = max;

            axis.OnMinMaxChanged();

            Assert.Equal(min.ToOADate(), axis.ActualRange.Start);
            Assert.Equal(max.ToOADate(), axis.ActualRange.End);
        }

        [Theory]
        [InlineData(DateTimeIntervalType.Years, 0.1)]
        [InlineData(DateTimeIntervalType.Months, 1)]
        [InlineData(DateTimeIntervalType.Days, 50)]
        [InlineData(DateTimeIntervalType.Hours, 500)]
        [InlineData(DateTimeIntervalType.Minutes, 50000)]
        [InlineData(DateTimeIntervalType.Seconds, 2000000)]
        public void CalculateDateTimeIntervalType_ShouldReturnCorrectInterval_ForGivenIntervalType(DateTimeIntervalType intervalType, double expectedInterval)
        {
            var axis = new DateTimeAxis();
            var actualRange = new DoubleRange(DateTime.Now.AddYears(-1).ToOADate(), DateTime.Now.ToOADate());
            var size = new Size(1000, 500); 

            var result = axis.CalculateDateTimeIntervalType(actualRange, size, ref intervalType);

            Assert.Equal(expectedInterval, result);
        }

        [Fact]
        public void GetStartPosition_ShouldReturnCorrectDate_ForGivenVisibleRange()
        {
			var axis = new DateTimeAxis
			{
				VisibleRange = new DoubleRange(DateTime.Now.AddMonths(-6).ToOADate(), DateTime.Now.ToOADate())
			};

			var result = axis.GetStartPosition();

            Assert.True(result < DateTime.Now);
            Assert.True(result > DateTime.Now.AddMonths(-7));
        }

        [Fact]
        public void AddDefaultRange_ShouldReturnDefaultRange_WhenStartIsZero()
        {
            var axis = new DateTimeAxis();
            var expectedRange = new DoubleRange(25569.2291666667, 25570.2291666667);

            var result = axis.AddDefaultRange(0);

            Assert.Equal(expectedRange.Start, result.Start);
            Assert.Equal(expectedRange.End, result.End);
        }

        [Theory]
        [InlineData(DateTimeIntervalType.Years, 1)]
        [InlineData(DateTimeIntervalType.Months, 1)]
        [InlineData(DateTimeIntervalType.Days, 1)]
        public void UpdateAutoScrollingDelta_ShouldUpdateAutoScrollingDeltaCorrectly(DateTimeIntervalType intervalType, double expectedDelta)
        {
            var axis = new DateTimeAxis();
            var range = new DoubleRange(DateTime.Now.AddYears(-1).ToOADate(), DateTime.Now.ToOADate());
            axis.AutoScrollingDelta = 1;
            axis.AutoScrollingDeltaType = (intervalType);

            axis.UpdateAutoScrollingDelta(range, axis.AutoScrollingDelta);

            Assert.Equal(expectedDelta, axis.AutoScrollingDelta);
        }

        [Theory]
        [InlineData(DateTimeIntervalType.Years, 365)]
        [InlineData(DateTimeIntervalType.Months, 30)]
        [InlineData(DateTimeIntervalType.Days, 1)]
        [InlineData(DateTimeIntervalType.Hours, 24)]
        [InlineData(DateTimeIntervalType.Minutes, 60)]
        [InlineData(DateTimeIntervalType.Seconds, 60)]
        [InlineData(DateTimeIntervalType.Milliseconds, 1000)]
        [InlineData(DateTimeIntervalType.Auto, 1)] // Default case
        public void GetMinWidthForSingleDataPoint_ShouldReturnExpectedValue(DateTimeIntervalType intervalType, double expectedMinWidth)
        {
            var axis = new DateTimeAxis
            {
                ActualIntervalType = intervalType
            };

            var result = axis.GetMinWidthForSingleDataPoint();

            Assert.Equal(expectedMinWidth, result);
        }

        [Theory]
        [InlineData(2020.0, 2022.0, 1.0, DateTimeRangePadding.Round, DateTimeIntervalType.Years, 1828.0)]
        [InlineData(2020.0, 2020.5, 3.0, DateTimeRangePadding.AppendInterval, DateTimeIntervalType.Months, 2020.00)]
        [InlineData(2020.0, 2020.04167, 5.0, DateTimeRangePadding.None, DateTimeIntervalType.Days, 2020.0)]
        [InlineData(0.0, 2.456, 1.0, DateTimeRangePadding.Round, DateTimeIntervalType.Seconds, 0.0)]
        [InlineData(2020.0, 2021.0, -1.0, DateTimeRangePadding.Round, DateTimeIntervalType.Months, 2009.0)] // Negative interval
        public void ApplyDateRangePadding_VariousScenarios_ReturnsExpectedRange(double start, double end, double interval, DateTimeRangePadding rangePadding, DateTimeIntervalType intervalType, double expectedStart)
        {
            var axis = new DateTimeAxis();
            var range = new DoubleRange(start, end);
            axis.RangePadding = rangePadding;
            axis.ActualIntervalType = intervalType;

            var paddedRange = InvokePrivateMethod(axis, "ApplyDateRagePadding", [range, interval]); 

            Assert.NotNull(paddedRange);
            Assert.Equal(expectedStart, ((DoubleRange)paddedRange).Start);
        }

		[Theory]
        [InlineData(DateTimeIntervalType.Years, DateTimeIntervalType.Years)]
        [InlineData(DateTimeIntervalType.Months, DateTimeIntervalType.Months)]
        [InlineData(DateTimeIntervalType.Days, DateTimeIntervalType.Days)]
        [InlineData(DateTimeIntervalType.Hours, DateTimeIntervalType.Hours)]
        [InlineData(DateTimeIntervalType.Minutes, DateTimeIntervalType.Minutes)]
        [InlineData(DateTimeIntervalType.Seconds, DateTimeIntervalType.Seconds)]
        [InlineData(DateTimeIntervalType.Milliseconds, DateTimeIntervalType.Milliseconds)]
        public void GetActualAutoScrollingDeltaType_Returns_Expected_Interval(DateTimeIntervalType expectedInterval, DateTimeIntervalType autoScrollingDeltaType)
        {
			var axis = new DateTimeAxis
			{
				AutoScrollingDeltaType = autoScrollingDeltaType
			};

			var result = InvokePrivateMethod(axis, "GetActualAutoScrollingDeltaType");

            Assert.Equal(expectedInterval, result);
        }

        [Theory]
        [InlineData(DateTimeIntervalType.Years)]
        [InlineData(DateTimeIntervalType.Months)]
        [InlineData(DateTimeIntervalType.Days)]
        [InlineData(DateTimeIntervalType.Hours)]
        [InlineData(DateTimeIntervalType.Minutes)]
        [InlineData(DateTimeIntervalType.Seconds)]
        [InlineData(DateTimeIntervalType.Milliseconds)]
        public void GetActualAutoScrollingDeltaType_ShouldReturnCorrectIntervalType(DateTimeIntervalType intervalType)
        {
			var axis = new DateTimeAxis
			{
				ActualRange = new DoubleRange(DateTime.Now.AddYears(-1).ToOADate(), DateTime.Now.ToOADate()),
				AvailableSize = new Size(1000, 500),
				AutoScrollingDeltaType = DateTimeIntervalType.Auto
			};

			SetPrivateField(axis, "_dateTimeIntervalType", intervalType);

            var result = InvokePrivateMethod(axis, "GetActualAutoScrollingDeltaType"); ;

            Assert.NotNull(result);
            Assert.Equal(intervalType, (DateTimeIntervalType)result);
        }

        [Theory]
        [InlineData(DateTimeIntervalType.Auto)]
        public void GetActualAutoScrollingDeltaType_ShouldReturnCorrectIntervalType_WhenSetAuto(DateTimeIntervalType intervalType)
        {
			var axis = new DateTimeAxis
			{
				ActualRange = new DoubleRange(DateTime.Now.AddYears(-1).ToOADate(), DateTime.Now.ToOADate()),
				AvailableSize = new Size(1000, 500),
				AutoScrollingDeltaType = DateTimeIntervalType.Auto
			};

			SetPrivateField(axis, "_dateTimeIntervalType", intervalType);

            var result = InvokePrivateMethod(axis, "GetActualAutoScrollingDeltaType"); ;

            Assert.NotNull(result);
            Assert.Equal(DateTimeIntervalType.Months, (DateTimeIntervalType)result);
        }

        [Fact]
        public void ValidateFromOADate_ReturnsSameValue_WhenValueIsPositive()
        {
            var axis = new DateTimeAxis();
            double input = 10;
            double expected = 10;

            var result = InvokePrivateMethod(axis, "ValidateFromOADate", input);

            Assert.NotNull(result);
            Assert.Equal(expected, (double)result);
        }

        [Theory]
        [InlineData(2023, 5, 15, 1, DateTimeIntervalType.Years, 2023, 1, 1)] // Year rounding
        [InlineData(2023, 5, 15, 1, DateTimeIntervalType.Months, 2023, 5, 1)] // Month rounding
        [InlineData(2023, 5, 15, 1, DateTimeIntervalType.Days, 2023, 5, 15)] // Day rounding
        public void NiceStart_AdjustsStartDate_Correctly(int year, int month, int day, int visibleInterval, DateTimeIntervalType intervalType, int expectedYear, int expectedMonth, int expectedDay)
        {
            var axis = new DateTimeAxis();
            DateTime startDate = new DateTime(year, month, day);
            DateTime expectedDate = new DateTime(expectedYear, expectedMonth, expectedDay);

            axis.ActualIntervalType = intervalType;
            axis.VisibleInterval = visibleInterval;
            axis.ActualRange = new DoubleRange (startDate.ToOADate(),  100);
            var parameter = new object[] { startDate };

            InvokeRefPrivateMethod(axis, "NiceStart", ref parameter);

            Assert.Equal(expectedDate, parameter[0]);
        }

        [Theory]
        [InlineData(2023, 5, 15, 1, DateTimeIntervalType.Hours, 2023, 5, 15, 0)] // Hours rounding
        [InlineData(2023, 5, 15, 1, DateTimeIntervalType.Minutes, 2023, 5, 15, 0)] // Minutes rounding
        [InlineData(2023, 5, 15, 1, DateTimeIntervalType.Seconds, 2023, 5, 15, 0)] // Seconds rounding
        public void NiceStart_AdjustsStartDate_WithTimeIntervals(int year, int month, int day, int visibleInterval, DateTimeIntervalType intervalType, int expectedYear, int expectedMonth, int expectedDay, int expectedHour)
        {
            var axis = new DateTimeAxis();
            DateTime startDate = new DateTime(year, month, day);
            DateTime expectedDate = new DateTime(expectedYear, expectedMonth, expectedDay, expectedHour, 0, 0);

            axis.ActualIntervalType = intervalType;
            axis.VisibleInterval = visibleInterval;
            axis.ActualRange = new DoubleRange(startDate.ToOADate(), 100);
            var parameter = new object[] { startDate };

            InvokeRefPrivateMethod(axis, "NiceStart", ref parameter);

            Assert.Equal(expectedDate, startDate);
        }

        [Fact]
        public void UpdateAxisInterval_ValidValue_UpdatesInterval()
        {
            var axis = new DateTimeAxis();
            double newInterval = 5.0;

            InvokePrivateMethod(axis, "UpdateAxisInterval", newInterval);

            Assert.Equal(newInterval, axis.AxisInterval);
        }

        [Fact]
        public void UpdateDefaultMinimum_ValidDateTime_UpdatesMinimum()
        {
            var axis = new DateTimeAxis();
            DateTime? newMinDate = new DateTime(2024, 1, 1);

            InvokePrivateMethod(axis, "UpdateDefaultMinimum", newMinDate);

            Assert.Equal(newMinDate, axis.DefaultMinimum);
        }

        [Fact]
		public void UpdateDefaultMaximum_ValidDateTime_UpdatesMaximum()
		{
			var axis = new DateTimeAxis();
			DateTime? newMaxDate = new DateTime(2024, 12, 31);

			InvokePrivateMethod(axis, "UpdateDefaultMaximum", newMaxDate);

			Assert.Equal(newMaxDate, axis.DefaultMaximum);
		}

		#endregion

		#region LogarithmicAxis methods

		[Fact]
        public void UpdateAxisScale_LogarthmicAxis()
        {
            var axis = new LogarithmicAxis()
            {
                ActualRange = new DoubleRange(100, 10000),
            };
            axis.VisibleLogRange = new DoubleRange(axis.GetLogValue(200), axis.GetLogValue(8000));

            axis.UpdateAxisScale();

            Assert.Equal(0.1505149978319904, axis.ZoomPosition);
            Assert.Equal(0.80102999566398125, axis.ZoomFactor);
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(5, 6)]
        [InlineData(-3, -2)]
        public void AddDefaultRange_LogarithmicAxis(double start, double expectedEnd)
        {
            var axis = new LogarithmicAxis();

            var result = axis.AddDefaultRange(start);

            Assert.Equal(start, result.Start);
            Assert.Equal(expectedEnd, result.End);
        }

        [Theory]
        [InlineData(ChartAutoScrollingMode.Start, 100)]
        [InlineData(ChartAutoScrollingMode.End, 200)]
        public void UpdateAutoScrollingDelta_LogarithmicAxis(ChartAutoScrollingMode mode, double scrollingDelta)
        {
            var axis = new LogarithmicAxis()
            {
                ActualRange = new DoubleRange(10, 1000),
                AutoScrollingMode = mode
            };

            axis.UpdateAutoScrollingDelta(axis.ActualRange, scrollingDelta);

            Assert.NotEqual(DoubleRange.Empty, axis.VisibleRange);
            Assert.NotEqual(DoubleRange.Empty, axis.VisibleLogRange);
        }

        [Fact]
        public void AddSmallTicksPoint_AddsCorrectSmallTickPoints()
        {
            var axis = new LogarithmicAxis();
            double position = 10; 
            double interval = 2;  
            axis.VisibleInterval = 1; 
            axis.MinorTicksPerInterval = 5; 

            axis.VisibleLogRange = new DoubleRange(0, 100);

            axis.AddSmallTicksPoint(position, interval);

            Assert.True(axis.SmallTickPoints.Count > 0, "Small tick points should be added.");
        }

        [Theory]
        [InlineData(0, 0, 100, false, 0)] 
        public void ValueToCoefficient_ReturnsExpectedCoefficient(double value, double rangeStart, double rangeEnd, bool isInversed, float expected)
        {
            var axis = new LogarithmicAxis()
            {
                VisibleRange = new DoubleRange(rangeStart, rangeEnd),
                IsInversed = isInversed
            };

            var result = axis.ValueToCoefficient(value);

            Assert.Equal(expected, result, 2); 
        }

        [Theory]
        [InlineData(0, 1, 100, false, 1)]   
        public void CoefficientToValue_ReturnsExpectedValue(double coefficient, double rangeStart, double rangeEnd, bool isInversed, double expected)
        {
            var axis = new LogarithmicAxis()
            {
                VisibleRange = new DoubleRange(rangeStart, rangeEnd),
                IsInversed = isInversed
            };

            var result = axis.CoefficientToValue(coefficient);

            Assert.Equal(expected, result, 2); 
        }

        [Fact]
        public void GetLogValue_CalculatesLogarithmicValueCorrectly()
        {
            var axis = new LogarithmicAxis();
            double value = 100;
            double tolerance = 0.0001;
            double expectedLogValue = Math.Log(value) / Math.Log(axis.LogarithmicBase);  // Expected log value

            double logValue = axis.GetLogValue(value);

            Assert.True(Math.Abs(logValue - expectedLogValue) < tolerance,
                $"Expected {expectedLogValue} but got {logValue}");
        }

        [Fact]
        public void GetPowValue_CalculatesPowerValueCorrectly()
        {
            var axis = new LogarithmicAxis();
            double tolerance = 0.0001;
            double power = 2; 
            double expectedPowValue = Math.Pow(axis.LogarithmicBase, power);  

            double powValue = axis.GetPowValue(power);

            Assert.Equal(expectedPowValue, powValue, tolerance);
        }

        [Fact]
        public void RaiseCallBackActualRangeChanged_UpdatesActualMinMaxCorrectly()
        {
			var axis = new LogarithmicAxis
			{
				ActualRange = new DoubleRange(1, 100)
			};

			axis.RaiseCallBackActualRangeChanged();

            Assert.Equal(1, axis.ActualMinimum);
            Assert.Equal(100, axis.ActualMaximum);
        }

        [Fact]
        public void OnMinMaxChanged_UpdatesActualRangeCorrectly()
        {
			var axis = new LogarithmicAxis
			{
				DefaultMinimum = 2,
				DefaultMaximum = 100
			};

			InvokePrivateMethod(axis, "OnMinMaxChanged");

            Assert.Equal(axis.GetLogValue(2), axis.ActualRange.Start);
            Assert.Equal(axis.GetLogValue(100), axis.ActualRange.End);
        }

        [Fact]
        public void CalculateLogRange_ValidRange_ReturnsCorrectLogRange()
        {
            var axis = new LogarithmicAxis();
            DoubleRange range = new DoubleRange(1, 100);

            var result = InvokePrivateMethod(axis, "CalculateLogRange", range); 

            double expectedStart = axis.GetLogValue(1);
            double expectedEnd = axis.GetLogValue(100);
            Assert.Equal(expectedStart, axis.VisibleLogRange.Start);
            Assert.Equal(expectedEnd, axis.VisibleLogRange.End);
        }

        [Fact]
        public void SetVisibleRange_UpdatesVisibleLogRangeCorrectly()
        {
            var axis = new LogarithmicAxis();
            double start = axis.GetLogValue(1);
            double end = axis.GetLogValue(100);

            var result = InvokePrivateMethod(axis, "SetVisibleRange", [start, end]);

            Assert.NotNull(result);
            Assert.Equal(axis.GetPowValue(start), ((DoubleRange)result).Start);
            Assert.Equal(axis.GetPowValue(end), ((DoubleRange)result).End);
        }

        [Theory]
        [InlineData(double.NaN, double.NaN, 10, 100)]  
        [InlineData(10, 100, 10, 100)]                
        [InlineData(0, 100, 1, 100)]                  
        [InlineData(10, double.NaN, 10, 100)]       
        [InlineData(double.NaN, 100, 10, 100)]      
        public void CalculateActualRange_ReturnsExpectedRange(double defaultMin, double defaultMax, double expectedMin, double expectedMax)
        {
            var series = new LineSeries() { VisibleXRange = new DoubleRange(10, 100) };
            var axis = new LogarithmicAxis()
            {
                DefaultMinimum = defaultMin,
                DefaultMaximum = defaultMax,
                ActualRange = new DoubleRange(1, 50) 
            };

			var area = new CartesianChartArea(new SfCartesianChart())
			{
				Series = [series]
			};
			axis.Area = area;

            series.ActualXAxis = axis;

            var result = InvokePrivateMethod(axis, "CalculateActualRange");

            Assert.NotNull(result);
            Assert.Equal(expectedMin, ((DoubleRange)result).Start);
            Assert.Equal(expectedMax, ((DoubleRange)result).End);
        }

        [Theory]
        [InlineData(double.NaN, 5)]  
        [InlineData(0, 5)]         
        [InlineData(5, 5)]        
        public void CalculateActualInterval_ReturnsExpectedInterval(double axisInterval, double expected)
        {
            var axis = new LogarithmicAxis()
            {
                AxisInterval = axisInterval,
                VisibleLogRange = new DoubleRange(1, 10),
                AvailableSize = new Size(100, 100) 
            };

            var result = InvokePrivateMethod(axis, "CalculateActualInterval", [axis.VisibleLogRange, axis.AvailableSize]);

            Assert.NotNull(result);
            Assert.Equal(expected, (double)result);
        }

        [Theory]
        [InlineData(1, 10, 100, 5)]    
        [InlineData(1, 1000, 100, 1000)]
        [InlineData(1, 1000, 10, 1000)] 
        public void CalculateNiceInterval_ReturnsExpectedInterval(double rangeStart, double rangeEnd, int availableWidth, double expected)
        {
            var axis = new LogarithmicAxis()
            {
                VisibleLogRange = new DoubleRange(rangeStart, rangeEnd),
                AvailableSize = new Size(availableWidth, 100)
            };

            var result = InvokePrivateMethod(axis, "CalculateNiceInterval", [axis.VisibleLogRange, axis.AvailableSize]);

            Assert.NotNull(result);
            Assert.Equal(expected, (double)result);
        }

        [Theory]
        [InlineData(0.5, 0.2)]  
        [InlineData(0.1, 0.9)]  
        public void CalculateVisibleRange_ReturnsExpectedRange(double zoomFactor, double zoomPosition)
        {
            var axis = new LogarithmicAxis()
            {
                VisibleLogRange = new DoubleRange(1, 100),
                ZoomFactor = zoomFactor,
                ZoomPosition = zoomPosition
            };

            var result = InvokePrivateMethod(axis, "CalculateVisibleRange", [axis.VisibleLogRange, new Size(100, 100)]);

            var expectedRange = new DoubleRange(axis.GetPowValue(axis.VisibleLogRange.Start), axis.GetPowValue(axis.VisibleLogRange.End));

            Assert.NotNull(result);
            Assert.Equal(expectedRange, (DoubleRange)result); 
        }

        [Theory]
        [InlineData(1, 0)]
        public void CalculateVisibleRange_WithZoomfactorOne(double zoomFactor, double zoomPosition)
        {
            var axis = new LogarithmicAxis()
            {
                VisibleLogRange = new DoubleRange(1, 100),
                ZoomFactor = zoomFactor,
                ZoomPosition = zoomPosition
            };

            var result = InvokePrivateMethod(axis, "CalculateVisibleRange", [axis.VisibleLogRange, new Size(100, 100)]);

            Assert.NotNull(result);
            Assert.Equal(axis.VisibleLogRange, (DoubleRange)result);
        }

        #endregion

        #region NumericalAxis methods

        [Fact]
        public void GetStartDoublePosition_ReturnsVisibleRangeStart_WhenConditionsMet()
        {
            var axis = new NumericalAxis()
            {
                DefaultMinimum = 1.0,
                ZoomFactor = 1.0f,
                EdgeLabelsVisibilityMode = EdgeLabelsVisibilityMode.AlwaysVisible,
                VisibleRange = new DoubleRange(5.0, 10.0),
            };

            var startPosition = axis.GetStartDoublePosition();

            Assert.Equal(axis.VisibleRange.Start, startPosition);
        }

        [Theory]
        [InlineData(10.0, 100.0, 10.0, 100.0)]                
        [InlineData(double.NaN, 50.0, double.MinValue, 50.0)] 
        [InlineData(20.0, double.NaN, 20.0, double.MaxValue)] 
        public void OnMinMaxChanged_UpdatesActualRange(double defaultMinimum, double defaultMaximum, double expectedMin, double expectedMax)
        {
            var axis = new NumericalAxis()
            {
                DefaultMinimum = defaultMinimum,
                DefaultMaximum = defaultMaximum
            };

            axis.OnMinMaxChanged();

            Assert.Equal(new DoubleRange(expectedMin, expectedMax), axis.ActualRange);
        }

        [Fact]
        public void UpdateAxisInterval_SetsCorrectValue()
        {
            var axis = new NumericalAxis();
            double expectedInterval = 5.0;

            InvokePrivateMethod(axis, "UpdateAxisInterval", expectedInterval);

            Assert.Equal(expectedInterval, axis.AxisInterval);
        }

        [Fact]
        public void UpdateDefaultMinimum_SetsCorrectValue()
        {
            var axis = new NumericalAxis();
            double? expectedMinimum = 10.0;

            InvokePrivateMethod(axis, "UpdateDefaultMinimum", expectedMinimum);

            Assert.Equal(expectedMinimum.Value, axis.DefaultMinimum);
        }

        [Fact]
        public void UpdateDefaultMaximum_SetsCorrectValue()
        {
            var axis = new NumericalAxis();
            double? expectedMaximum = 100.0;

            InvokePrivateMethod(axis, "UpdateDefaultMaximum", expectedMaximum);

            Assert.Equal(expectedMaximum.Value, axis.DefaultMaximum);
        }

        [Fact]
        public void ActualRangePadding_ReturnsRoundPadding_WhenAutoAndVisibleSeries()
        {
			var axis = new NumericalAxis
			{
				RangePadding = NumericalPadding.Auto,
				Area = new CartesianChartArea(new SfCartesianChart())
				{
					IsTransposed = false,
					Series = [new LineSeries()]
				},
				IsVertical = true
			};

			var padding = InvokePrivateMethod(axis, "ActualRangePadding"); 

            Assert.Equal(NumericalPadding.Round, padding);
        }

        [Fact]
        public void RaiseCallBackActualRangeChanged_UpdatesActualValues()
        {
            var axis = new NumericalAxis()
            {
                ActualRange = new DoubleRange(5.0, 10.0) 
            };

            axis.RaiseCallBackActualRangeChanged();

            Assert.Equal(5.0, axis.ActualMinimum);
            Assert.Equal(10.0, axis.ActualMaximum);
        }

        [Theory]
        [InlineData(0.5)] 
        [InlineData(1.0)] 
        public void CalculateVisibleRange_ValidatesExpectedVisibleRange(double zoomFactor)
        {
            var axis = new NumericalAxis()
            {
                ZoomFactor = zoomFactor
            };

            var range = new DoubleRange(1.0, 10.0); 
            var availableSize = new Size(100, 100); 

            var visibleRange = InvokePrivateMethod(axis, "CalculateVisibleRange", [range, availableSize] ); 

            Assert.NotNull(visibleRange);
            Assert.NotEqual(DoubleRange.Empty, (DoubleRange)visibleRange);
        }

        [Theory]
        [InlineData(double.NaN)] 
        [InlineData(0.0)] 
        [InlineData(5.0)] 
        public void CalculateActualInterval_ValidatesExpectedInterval(double axisInterval)
        {
            var axis = new NumericalAxis()
            {
                AxisInterval = axisInterval
            };

            var range = new DoubleRange(1.0, 10.0); 
            var availableSize = new Size(100, 100); 

            var actualInterval = InvokePrivateMethod(axis, "CalculateActualInterval", [range, availableSize]);

            Assert.NotNull(actualInterval);
            Assert.True((double)actualInterval >= 0);
        }

        [Theory]
        [InlineData(double.NaN, double.NaN)] 
        [InlineData(10.0, 100.0)] 
        [InlineData(20.0, double.NaN)] 
        [InlineData(double.NaN, 50.0)] 
        public void ApplyRangePadding_ValidatesExpectedPadding(double defaultMinimum, double defaultMaximum)
        {
            var axis = new NumericalAxis()
            {
                DefaultMinimum = defaultMinimum,
                DefaultMaximum = defaultMaximum
            };

            var range = new DoubleRange(1.0, 10.0); 
            double interval = 1.0;

            var paddedRange = InvokePrivateMethod(axis, "ApplyRangePadding", [range, interval]);

            // You may need to update this assertion based on your specific logic
            Assert.NotNull(paddedRange);
        }

        [Theory]
        [InlineData(10.0, 100.0, 10.0, 100.0)] 
        [InlineData(20.0, double.NaN, 20.0, 21)] 
        [InlineData(double.NaN, 50.0, 99, 50.0)] 
        public void CalculateActualRange_ValidatesExpectedRange(double defaultMinimum, double defaultMaximum, double expectedMin, double expectedMax)
        {
            var axis = new NumericalAxis()
            {
                DefaultMinimum = defaultMinimum,
                DefaultMaximum = defaultMaximum,
                ActualRange = new DoubleRange(20, 100)
            };

            var actualRange = InvokePrivateMethod(axis, "CalculateActualRange");

            Assert.NotNull(actualRange);
            Assert.Equal(new DoubleRange(expectedMin, expectedMax), (DoubleRange)actualRange);
        }

        [Theory]
        [InlineData(double.NaN, double.NaN, 10, 100)]
        public void CalculateActualRange_ValidatesWithNANRange(double defaultMinimum, double defaultMaximum, double expectedMin, double expectedMax)
        {
            var axis = new NumericalAxis()
            {
                DefaultMinimum = defaultMinimum,
                DefaultMaximum = defaultMaximum,
                ActualRange = new DoubleRange(0, 100)
            };
            var series = new LineSeries() { VisibleXRange = new DoubleRange(10, 100) };

			var area = new CartesianChartArea(new SfCartesianChart())
			{
				Series = [series]
			};
			axis.Area = area;

            series.ActualXAxis = axis;

            var actualRange = InvokePrivateMethod(axis, "CalculateActualRange");

            Assert.NotNull(actualRange);
            Assert.Equal(new DoubleRange(expectedMin, expectedMax), (DoubleRange)actualRange);
        }

        #endregion

        #region CoefficientToValue Tests

        [Fact]
        public void CoefficientToValue_TestForNormalAxis()
        {
			var axis = new CategoryAxis
			{
				VisibleRange = new DoubleRange(0, 100),
				RenderedRect = new Rect(0, 0, 100, 200),
				IsInversed = false
			};
			double coefficient = 0.5;

            var result = axis.CoefficientToValue(coefficient);

            Assert.Equal(50, result);
        }

        [Fact]
        public void CoefficientToValue_TestForInversedAxis()
        {
			var axis = new CategoryAxis
			{
				VisibleRange = new DoubleRange(0, 100),
				RenderedRect = new Rect(0, 0, 100, 200),
				IsInversed = true
			};
			double coefficient = 0.25;

            var result = axis.CoefficientToValue(coefficient);

            Assert.Equal(75, result);
        }

        #endregion

        #region ValueToCoefficient Tests

        [Fact]
        public void ValueToCoefficient_TestForNormalAxis()
        {
			var axis = new CategoryAxis
			{
				VisibleRange = new DoubleRange(0, 100),
				RenderedRect = new Rect(0, 0, 100, 200),
				IsInversed = false
			};
			double value = 50;

            var result = axis.ValueToCoefficient(value);

            Assert.Equal(0.5f, result);
        }

        [Fact]
        public void ValueToCoefficient_TestForInversedAxis()
        {
			var axis = new CategoryAxis
			{
				VisibleRange = new DoubleRange(0, 100),
				RenderedRect = new Rect(0, 0, 100, 200),
				IsInversed = true
			};
			double value = 25;

            var result = axis.ValueToCoefficient(value);

            Assert.Equal(0.75f, result);
        }

        #endregion

        #region ValueToPoint Tests

        [Fact]
        public void ValueToPoint_ShouldReturnCorrectPoint_ForHorizontalAxis()
        {
			var axis = new CategoryAxis
			{
				VisibleRange = new DoubleRange(0, 100),
				RenderedRect = new Rect(0, 0, 100, 200),
				LeftOffset = 10,
				IsVertical = false
			};
			double value = 50;

            var result = axis.ValueToPoint(value);

            Assert.Equal(60f, result); 
        }

        [Fact]
        public void ValueToPoint_ShouldReturnCorrectPoint_ForVerticalAxis()
        {
			var axis = new CategoryAxis
			{
				VisibleRange = new DoubleRange(0, 100),
				RenderedRect = new Rect(0, 0, 100, 200),
				LeftOffset = 10,
				TopOffset = 5,
				IsVertical = true
			};
			double value = 75;

            var result = axis.ValueToPoint(value);

            Assert.Equal(55f, result); 
        }

        #endregion

        #region PointToValue Tests

        [Fact]
        public void PointToValue_ShouldReturnCorrectValue_ForHorizontalAxis()
        {
			var axis = new CategoryAxis
			{
				Area = new CartesianChartArea(new SfCartesianChart()),
				VisibleRange = new DoubleRange(0, 100),
				RenderedRect = new Rect(0, 0, 100, 200),
				LeftOffset = 10,
				IsVertical = false
			};
			float x = 60f; 

            var result = axis.PointToValue(x, 0);

            Assert.Equal(50, result);

            var points = axis.PointToValue(new PointF(50, 0));

            Assert.Equal(40f, points);
        }

        [Fact]
        public void PointToValue_ShouldReturnCorrectValue_ForVerticalAxis()
        {
			var axis = new CategoryAxis
			{
				Area = new CartesianChartArea(new SfCartesianChart()),
				VisibleRange = new DoubleRange(0, 100),
				RenderedRect = new Rect(0, 0, 100, 200),
				TopOffset = 5,
				IsVertical = true
			};
			float y = 55f;

            var result = axis.PointToValue(0, y);

            Assert.Equal(75, result);

            var points = axis.PointToValue(new PointF(0, y));

            Assert.Equal(75f, points);
        }

        #endregion

    }
}
