using System.Data;
using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.Toolkit.UnitTest
{
	public class SegmentUnitTests : BaseUnitTest
	{
		[Fact]
		public void CalculateInteriorPoints_ShouldFillPointsCorrectly()
		{
			var areaSegment = new AreaSegment();
			var sfCartesianChart = new SfCartesianChart();
			var areaSeries = new AreaSeries
			{
				ActualXAxis = new NumericalAxis
				{
					ActualCrossingValue = 0,
					VisibleRange = new DoubleRange(0.5, 5.5),
					RenderedRect = new Rect(70, 234, 875.3333, 66),
				}
			};
			var xValues = new double[] { 1, 2, 3 };
			var yValues = new double[] { 4, 5, 6 };
			areaSegment.XValues = xValues;
			areaSegment.YValues = yValues;
			areaSegment.Series = areaSeries;
			sfCartesianChart.Series.Add(areaSeries);

			var expectedFillPoints = new List<float>
			{
				areaSeries.TransformToVisibleX(1, 0), areaSeries.TransformToVisibleY(1, 0),
				areaSeries.TransformToVisibleX(1, 4), areaSeries.TransformToVisibleY(1, 4),
				areaSeries.TransformToVisibleX(2, 5), areaSeries.TransformToVisibleY(2, 5),
				areaSeries.TransformToVisibleX(3, 6), areaSeries.TransformToVisibleY(3, 6),
				areaSeries.TransformToVisibleX(3, 0), areaSeries.TransformToVisibleY(3, 0)
			};
			var par = Array.Empty<object>();
			InvokePrivateMethod(areaSegment, "CalculateInteriorPoints", par);
			Assert.Equal(expectedFillPoints, areaSegment.FillPoints);
		}

		[Fact]
		public void CalculateStrokePoints_ShouldFillStrokePointsCorrectly()
		{
			var areaSeries = new AreaSeries();
			var sfCartesianChart = new SfCartesianChart();
			areaSeries.ActualXAxis = new NumericalAxis
			{
				ActualCrossingValue = 0,
				VisibleRange = new DoubleRange(0.5, 5.5),
				RenderedRect = new Rect(70, 234, 875.3333, 66),
			};
			var areaSegment = new AreaSegment();
			var xValues = new double[] { 1, 2, 3 };
			var yValues = new double[] { 4, 5, 6 };
			areaSegment.XValues = xValues;
			areaSegment.YValues = yValues;
			var strokeWidth = 2;
			var halfStrokeWidth = (float)strokeWidth / 2;
			areaSegment.Series = areaSeries;
			sfCartesianChart.Series.Add(areaSeries);
			var expectedStrokePoints = new List<float>
			{
				areaSeries.TransformToVisibleX(1, 4), areaSeries.TransformToVisibleY(1, 4) + halfStrokeWidth,
				areaSeries.TransformToVisibleX(2, 5), areaSeries.TransformToVisibleY(2, 5) + halfStrokeWidth,
				areaSeries.TransformToVisibleX(3, 6), areaSeries.TransformToVisibleY(3, 6) + halfStrokeWidth
			};

			var par = Array.Empty<object>();
			InvokePrivateMethod(areaSegment, "CalculateStrokePoints", par);
			Assert.Equal(expectedStrokePoints, areaSegment.StrokePoints);
		}
		[Fact]
		public void SetData_WithValidValues_ShouldPopulateXAndYValuesCorrectly()
		{
			var areaSegment = new AreaSegment();
			var areaSeries = new AreaSeries
			{
				ActualYAxis = new NumericalAxis
				{
					VisibleRange = new DoubleRange(0, 10)
				}
			};
			var xValues = new double[] { 1, 2, 3 };
			var yValues = new double[] { 4, 5, 6 };
			areaSegment.Series = areaSeries;
			areaSegment.SetData(xValues, yValues);
			Assert.Equal(xValues, areaSegment.XValues);
			Assert.Equal(yValues, areaSegment.YValues);
			Assert.False(areaSegment.Empty);
		}

		[Fact]
		public void SetData_WithNaNValues_ShouldHandleNaNProperly()
		{
			var areaSegment = new AreaSegment();
			var areaSeries = new AreaSeries
			{
				ActualYAxis = new NumericalAxis
				{
					VisibleRange = new DoubleRange(0, 10)
				}
			};
			var xValues = new double[] { 1, 2, 3 };
			var yValues = new double[] { double.NaN, 5, 6 };
			areaSegment.Series = areaSeries;
			areaSegment.SetData(xValues, yValues);
			var expectedYMin = 5;
			Assert.Equal(expectedYMin, areaSegment?.YValues?.Where(v => !double.IsNaN(v)).Min());
			Assert.False(areaSegment?.Empty);
		}

		[Fact]
		public void GetDataPointIndex_InvalidCoordinates_ShouldReturnMinusOne()
		{
			var areaSegment = new AreaSegment();
			var xValues = new double[] { 1, 2, 3 };
			var yValues = new double[] { 4, 5, 6 };
			areaSegment.XValues = xValues;
			areaSegment.YValues = yValues;
			int dataPointIndex = areaSegment.GetDataPointIndex(10, 10);
			Assert.Equal(-1, dataPointIndex);
		}

		[Fact]
		public void GetMarkerRect_WithValidTooltipIndex_ShouldReturnCorrectRect()
		{
			var areaSegment = new AreaSegment();
			var markerWidth = 10;
			var markerHeight = 15;
			int tooltipIndex = 1;
			areaSegment.FillPoints = [0, 0, 30, 40, 60, 80];
			areaSegment.XValues = [1, 2, 3];
			areaSegment.Series = new AreaSeries();
			Rect result = ((IMarkerDependentSegment)areaSegment).GetMarkerRect(markerWidth, markerHeight, tooltipIndex);
			Assert.Equal(new Rect(55, 72.5, 10, 15), result);
		}

		[Theory]
		[InlineData(5.0, 5.0, 0, 10, 0, 10, 150f, 0f)]
		[InlineData(0.0, 0.0, 0, 10, 0, 10, 0f, 0f)]
		[InlineData(10.0, 10.0, 0, 10, 0, 10, 300f, 0f)]
		[InlineData(-5.0, -5.0, -10, 10, -10, 10, 75f, 0f)]
		[InlineData(5.0, double.NaN, 0, 10, 0, 10, 150f, float.NaN)]
		[InlineData(double.NaN, 5.0, 0, 10, 0, 10, float.NaN, 0f)]
		public void GetDataLabelPosition_WithAreaSeries_ShouldReturnCorrectPosition(
						double xValue, double yValue,
						double xAxisRangeStart, double xAxisRangeEnd,
						double yAxisRangeStart, double yAxisRangeEnd,
						float expectedXPosition, float expectedYPosition)
		{

			var sfCartesianChart = new SfCartesianChart();
			var chartArea = new CartesianChartArea(sfCartesianChart);
			var xAxis = new NumericalAxis
			{
				VisibleRange = new DoubleRange(xAxisRangeStart, xAxisRangeEnd),
				RenderedRect = new Rect(0, 0, 300, 0),
			};
			var yAxis = new NumericalAxis
			{
				VisibleRange = new DoubleRange(yAxisRangeStart, yAxisRangeEnd),
				RenderedRect = new Rect(0, 0, 0, 400),
			};

			var areaSeries = new AreaSeries
			{
				ChartArea = chartArea,
				ActualXAxis = xAxis,
				ActualYAxis = yAxis
			};
			var areaSegment = new AreaSegment
			{
				Series = areaSeries
			};
			var dataLabelPosition = (PointF?)InvokePrivateMethod(areaSegment, "GetDataLabelPosition", [xValue, yValue, areaSeries]);
			Assert.Equal(expectedXPosition, dataLabelPosition?.X);
			Assert.Equal(expectedYPosition, dataLabelPosition?.Y);
		}

		[Fact]
		public void SetData_ShouldPopulateValuesCorrectly()
		{
			var boxAndWhiskerSeries = new BoxAndWhiskerSeries();
			var boxAndWhiskerSegment = new BoxAndWhiskerSegment { Series = boxAndWhiskerSeries };
			var values = new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
			boxAndWhiskerSegment.SetData(values);
			Assert.Equal(4, boxAndWhiskerSegment.Minimum);
			Assert.Equal(5, boxAndWhiskerSegment.LowerQuartile);
			Assert.Equal(6, boxAndWhiskerSegment.Median);
			Assert.Equal(7, boxAndWhiskerSegment.UpperQuartile);
			Assert.Equal(8, boxAndWhiskerSegment.Maximum);
			Assert.Equal(10, boxAndWhiskerSegment.Center);
		}

		[Fact]
		public void GetDataPointIndex_ShouldDetectOutlier()
		{
			var boxAndWhiskerSeries = new BoxAndWhiskerSeries();
			var boxAndWhiskerSegment = new BoxAndWhiskerSegment { Series = boxAndWhiskerSeries };
			SetPrivateField(boxAndWhiskerSegment, "_outlierSegmentBounds", new List<RectF> { new Rect(10, 10, 5, 5) });
			int outlierIndex = boxAndWhiskerSegment.GetDataPointIndex(12, 12);
			Assert.Equal(-1, outlierIndex);
			Assert.True(boxAndWhiskerSeries.IsOutlierTouch);
		}

		[Fact]
		public void Layout_ShouldSetLayoutValuesCorrectly()
		{
			var boxAndWhiskerSeries = new BoxAndWhiskerSeries
			{
				ActualXAxis = new NumericalAxis { VisibleRange = new DoubleRange(0, 10) }
			};
			var boxAndWhiskerSegment = new BoxAndWhiskerSegment { Series = boxAndWhiskerSeries, LowerQuartile = 3, UpperQuartile = 7 };
			SetPrivateField(boxAndWhiskerSegment, "_x1", 2);
			SetPrivateField(boxAndWhiskerSegment, "_x2", 8);
			InvokePrivateMethod(boxAndWhiskerSegment, "Layout", [boxAndWhiskerSeries]);
			Assert.NotEqual(float.NaN, boxAndWhiskerSegment.Left);
			Assert.NotEqual(float.NaN, boxAndWhiskerSegment.Right);
			Assert.NotEqual(float.NaN, boxAndWhiskerSegment.Top);
			Assert.NotEqual(float.NaN, boxAndWhiskerSegment.Bottom);
		}

		[Fact]
		public void SetData_ShouldUpdateFieldsAndSeriesRanges()
		{
			var boxAndWhiskerSegment = new BoxAndWhiskerSegment();
			var boxAndWhiskerSeries = new BoxAndWhiskerSeries();
			boxAndWhiskerSegment.Series = boxAndWhiskerSeries;

			double[] inputValues = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11];
			boxAndWhiskerSegment.SetData(inputValues);
			Assert.Equal(4, boxAndWhiskerSegment.Minimum);
			Assert.Equal(5, boxAndWhiskerSegment.LowerQuartile);
			Assert.Equal(6, boxAndWhiskerSegment.Median);
			Assert.Equal(7, boxAndWhiskerSegment.UpperQuartile);
			Assert.Equal(8, boxAndWhiskerSegment.Maximum);
			Assert.Equal(10, boxAndWhiskerSegment.Center);
		}

		[Fact]
		public void SetData_Should_SetValuesCorrectly()
		{
			var bubbleSegment = new BubbleSegment();
			double[] values = [10.0, 20.0, 5.0, 2.5];
			bubbleSegment.SetData(values);
			var xValue = GetPrivateField(bubbleSegment, "_x");
			var yValue = GetPrivateField(bubbleSegment, "_y");
			var sizeValue = GetPrivateField(bubbleSegment, "_sizeValue");
			Assert.Equal(10.0, xValue);
			Assert.Equal(20.0, yValue);
			Assert.Equal(5.0, sizeValue);
			Assert.Equal(2.5f, bubbleSegment.Radius);
			Assert.False(bubbleSegment.Empty);
		}

		[Fact]
		public void SetData_Should_SetEmpty_True_When_ValuesAreNaN()
		{
			var bubbleSegment = new BubbleSegment();
			double[] values = [double.NaN, double.NaN, double.NaN, double.NaN];
			bubbleSegment.SetData(values);
			Assert.True(bubbleSegment.Empty);
		}

		[Fact]
		public void GetDataPointIndex_Should_Return_CorrectIndex_When_PointIsWithinRadius()
		{
			var bubbleSegment = new BubbleSegment
			{
				CenterX = 10,
				CenterY = 10,
				Radius = 5,
				StrokeWidth = 1,
				Series = new BubbleSeries()
			};
			bubbleSegment.Series._segments.Add(bubbleSegment);
			int result = bubbleSegment.GetDataPointIndex(12, 12);
			Assert.Equal(0, result);
		}

		[Fact]
		public void GetDataPointIndex_Should_Return_NegativeOne_When_PointIsOutsideRadius()
		{
			var bubbleSegment = new BubbleSegment
			{
				CenterX = 10,
				CenterY = 10,
				Radius = 5,
				StrokeWidth = 1,
				Series = new BubbleSeries()
			};
			bubbleSegment.Series._segments.Add(bubbleSegment);
			int result = bubbleSegment.GetDataPointIndex(20, 20);
			Assert.Equal(-1, result);
		}

		[Fact]
		public void SetPreviousData_Should_SetPreviousValuesCorrectly()
		{
			var bubbleSegment = new BubbleSegment();
			float[] values = [5.0f, 10.0f, 2.5f];
			bubbleSegment.SetPreviousData(values);
			Assert.Equal(5.0f, bubbleSegment.PreviousCenterX);
			Assert.Equal(10.0f, bubbleSegment.PreviousCenterY);
			Assert.Equal(2.5f, bubbleSegment.PreviousRadius);
		}

		[Fact]
		public void SetData_ShouldSetValuesCorrectly_WhenValidDataIsProvided()
		{
			var candleSegment = new CandleSegment();
			var candleSeries = new CandleSeries();
			candleSegment.Series = candleSeries;
			double[] values = [1.0, 2.0, 3.0, 4.0, 5.0, 2.5, 3.5, 1.5];
			bool isFill = true;
			bool isBull = true;
			candleSegment.SetData(values, isFill, isBull);
			Assert.Equal(1.0, candleSegment.StartX);
			Assert.Equal(2.0, candleSegment.CenterX);
			Assert.Equal(3.0, candleSegment.EndX);
			Assert.Equal(4.0, candleSegment.Open);
			Assert.Equal(5.0, candleSegment.High);
			Assert.Equal(2.5, candleSegment.Low);
			Assert.Equal(3.5, candleSegment.Close);
			Assert.Equal(1.5, candleSegment.XValue);
			Assert.True(candleSegment.IsFill);
			Assert.True(candleSegment.IsBull);
		}

		[Fact]
		public void SetData_ShouldNotSetValues_WhenSeriesIsNotCandleSeries()
		{
			var candleSegment = new CandleSegment();
			double[] values = [1.0, 2.0, 3.0, 4.0, 5.0, 2.5, 3.5, 1.5];
			bool isFill = false;
			bool isBull = false;
			candleSegment.SetData(values, isFill, isBull);
			Assert.Equal(0, candleSegment.StartX);
			Assert.Equal(0, candleSegment.CenterX);
			Assert.Equal(0, candleSegment.EndX);
			Assert.Equal(0, candleSegment.Open);
			Assert.Equal(0, candleSegment.High);
			Assert.Equal(0, candleSegment.Low);
			Assert.Equal(0, candleSegment.Close);
			Assert.Equal(0, candleSegment.XValue);
			Assert.False(candleSegment.IsFill);
			Assert.False(candleSegment.IsBull);
		}

		[Fact]
		public void GetDataPointIndex_ShouldReturnIndex_WhenPointWithinSegmentBounds()
		{
			var candleSegment = new CandleSegment
			{
				SegmentBounds = new RectF(0, 0, 10, 10)
			};
			var candleSeries = new CandleSeries();
			candleSeries._segments.Add(candleSegment);
			candleSegment.Series = candleSeries;
			int index = candleSegment.GetDataPointIndex(5, 5);
			Assert.Equal(0, index);
		}

		[Fact]
		public void GetDataPointIndex_ShouldReturnMinusOne_WhenPointOutsideSegmentBounds()
		{
			var candleSegment = new CandleSegment
			{
				SegmentBounds = new RectF(0, 0, 10, 10)
			};
			var candleSeries = new CandleSeries();
			candleSeries._segments.Add(candleSegment);
			candleSegment.Series = candleSeries;
			int index = candleSegment.GetDataPointIndex(15, 15);
			Assert.Equal(-1, index);
		}

		[Fact]
		public void Layout_ShouldSetLeftToNaN_WhenOutsideAxisRange()
		{
			var candleSegment = new CandleSegment();
			var candleSeries = new CandleSeries
			{
				ActualXAxis = new NumericalAxis
				{
					VisibleRange = new DoubleRange(10, 20)
				}
			};
			candleSegment.Series = candleSeries;
			candleSegment.StartX = 2;
			candleSegment.EndX = 8;
			InvokePrivateMethod(candleSegment, "Layout", [candleSeries]);
			Assert.True(float.IsNaN(candleSegment.Left));
		}

		[Theory]
		[InlineData(0.5f, 15, 10, 20)]
		[InlineData(1.0f, 20, 10, 20)]
		[InlineData(0.0f, 10, 10, 20)]
		[InlineData(0.5f, 20, double.NaN, 20)]
		[InlineData(0.5f, 5, 10, double.NaN)]
		public void GetDynamicAnimationValue_ShouldReturnExpectedResult(float animationValue, float expected, float oldValue, float newValue)
		{
			var columnSegment = new ColumnSegment();
			float result = CartesianSegment.GetDynamicAnimationValue(animationValue, 0, oldValue, newValue);
			Assert.Equal(expected, result);
		}

		[Theory]
		[InlineData(5f, 5f, 6f, 6f, 1f, true)]
		[InlineData(5f, 5f, 10f, 10f, 1f, true)]
		[InlineData(5f, 5f, 5f, 5f, 0.5f, true)]
		public void IsRectContains_FirstOverload_ShouldReturnExpectedResult(float xPoint, float yPoint, float valueX, float valueY, float strokeWidth, bool expectedResult)
		{
			_ = new ColumnSegment();
			bool result = ColumnSegment.IsRectContains(xPoint, yPoint, valueX, valueY, strokeWidth);
			Assert.Equal(expectedResult, result);
		}

		[Theory]
		[InlineData(1f, 1f, 4f, 4f, 3f, 3f, 1f, true)]
		[InlineData(1f, 1f, 4f, 4f, 5f, 5f, 1f, true)]
		[InlineData(1f, 1f, 4f, 4f, 2f, 2f, 0.5f, true)]
		public void IsRectContains_SecondOverload_ShouldReturnExpectedResult(float x1Point, float y1Point, float x2Point, float y2Point, float valueX, float valueY, float strokeWidth, bool expectedResult)
		{
			_ = new ColumnSegment();
			bool result = ColumnSegment.IsRectContains(x1Point, y1Point, x2Point, y2Point, valueX, valueY, strokeWidth);
			Assert.Equal(expectedResult, result);
		}

		[Fact]
		public void SetData_WithValidValues_ShouldPopulateFieldsCorrectly()
		{
			var columnSegment = new ColumnSegment();
			var columnSeries = new ColumnSeries();
			columnSegment.Series = columnSeries;
			double[] values = [1.0, 2.0, 3.0, 4.0, 5.0, 6.0];
			columnSegment.SetData(values);
			var x1 = GetPrivateField(columnSegment, "_x1");
			var x2 = GetPrivateField(columnSegment, "_x2");
			var y1 = GetPrivateField(columnSegment, "_y1");
			var y2 = GetPrivateField(columnSegment, "_y2");
			var xValue = GetPrivateField(columnSegment, "_xvalue");
			var labelContent = GetPrivateField(columnSegment, "_labelContent");
			Assert.Equal(1.0, x1);
			Assert.Equal(2.0, x2);
			Assert.Equal(3.0, y1);
			Assert.Equal(4.0, y2);
			Assert.Equal(5.0, xValue);
			Assert.Equal(6.0, labelContent);
			Assert.False(columnSegment.Empty);
		}

		[Fact]
		public void SetData_WithNaNValues_ShouldMarkAsEmpty()
		{
			var columnSegment = new ColumnSegment();
			var columnSeries = new ColumnSeries();
			columnSegment.Series = columnSeries;
			double[] values = [double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN];
			columnSegment.SetData(values);
			Assert.True(columnSegment.Empty);
		}
		[Fact]
		public void SetPreviousData_WithValidValues_ShouldPopulatePreviousYValuesCorrectly()
		{
			var columnSegment = new ColumnSegment();
			float[] values = [4.0f, 5.0f];
			columnSegment.SetPreviousData(values);
			Assert.Equal(4.0f, columnSegment.PreviousY1);
			Assert.Equal(5.0f, columnSegment.PreviousY2);
		}

		[Fact]
		public void GetDataPointIndex_WithValidCoordinates_ShouldReturnIndex()
		{
			var columnSegment = new ColumnSegment
			{
				Series = new ColumnSeries(),
				SegmentBounds = new Rect(0, 0, 10, 10)
			};
			columnSegment.Series._segments.Add(columnSegment);
			int index = columnSegment.GetDataPointIndex(5, 5);
			Assert.Equal(0, index);
		}

		[Fact]
		public void GetDataPointIndex_WithInvalidCoordinates_ShouldReturnMinusOne()
		{
			var columnSegment = new ColumnSegment
			{
				Series = new ColumnSeries(),
				SegmentBounds = new Rect(0, 0, 10, 10)
			};
			int index = columnSegment.GetDataPointIndex(15, 15);
			Assert.Equal(-1, index);
		}

		[Fact]
		public void GetColumnDynamicAnimationValue_WithValidOldAndCurrentValues_ShouldCalculateCorrectly()
		{
			var columnSegment = new ColumnSegment();
			float animationValue = 0.5f;
			double oldValue = 10.0;
			double currentValue = 20.0;
			float? result = (float?)InvokePrivateMethod(columnSegment, "GetColumnDynamicAnimationValue", animationValue, oldValue, currentValue);
			Assert.Equal(15.0f, result);
		}

		[Fact]
		public void GetColumnDynamicAnimationValue_WithNaNOldValue_ShouldReturnCurrentValue()
		{
			var columnSegment = new ColumnSegment();
			float animationValue = 0.5f;
			double oldValue = double.NaN;
			double currentValue = 20.0;
			float? result = (float?)InvokePrivateMethod(columnSegment, "GetColumnDynamicAnimationValue", animationValue, oldValue, currentValue);
			Assert.Equal(10.0f, result);
		}

		[Fact]
		public void SetData_WithClockwiseDoughnutSeries_ShouldSetAnglesAndYValueCorrectly()
		{
			var doughnutSegment = new DoughnutSegment();
			var doughnutSeries = new DoughnutSeries { _isClockWise = true };
			doughnutSegment.Series = doughnutSeries;
			double mStartAngle = 30.0;
			double mEndAngle = 60.0;
			double mYValue = 100.0;
			doughnutSegment.SetData(mStartAngle, mEndAngle, mYValue);
			Assert.Equal(mStartAngle, doughnutSegment.StartAngle);
			Assert.Equal(mEndAngle, doughnutSegment.EndAngle);
			Assert.Equal(mYValue, doughnutSegment.YValue);
		}

		[Fact]
		public void SetData_WithCounterClockwiseDoughnutSeries_ShouldAdjustEndAngleCorrectly()
		{
			var doughnutSegment = new DoughnutSegment();
			var doughnutSeries = new DoughnutSeries { _isClockWise = false };
			doughnutSegment.Series = doughnutSeries;
			double mStartAngle = 30.0;
			double mEndAngle = 60.0;
			double mYValue = 100.0;
			doughnutSegment.SetData(mStartAngle, mEndAngle, mYValue);
			Assert.Equal(mStartAngle, doughnutSegment.StartAngle);
			Assert.Equal(mStartAngle + mEndAngle, doughnutSegment.EndAngle);
			Assert.Equal(mYValue, doughnutSegment.YValue);
		}

		[Fact]
		public void GetDataPointIndex_WithPointOutsideSegment_ShouldReturnMinusOne()
		{
			var doughnutSegment = new DoughnutSegment
			{
				Series = new DoughnutSeries()
			};
			SetPrivateField(doughnutSegment, "_currentBounds", new RectF(0, 0, 100, 100));
			int index = doughnutSegment.GetDataPointIndex(150, 150);
			Assert.Equal(-1, index);
		}
		[Fact]
		public void UpdatePosition_ShouldSetActualBoundsAndRadius()
		{
			var sfCircularChart = new SfCircularChart();
			CircularChartArea chartArea = new CircularChartArea(sfCircularChart)
			{
				AreaBounds = new Rect(30, 40, 100, 100)
			};
			IChart chart = sfCircularChart;
			chart.ActualSeriesClipRect = chartArea.AreaBounds;
			var doughnutSegment = new DoughnutSegment();
			var doughnutSeries = new DoughnutSeries
			{
				Center = new PointF(50, 50),

				Radius = 0.4,
				InnerRadius = 0.2,
			};
			doughnutSegment.SegmentBounds = new Rect(30, 40, 100, 100);
			doughnutSegment.Series = doughnutSeries;
			sfCircularChart.Series.Add(doughnutSeries);
			SetPrivateField(doughnutSegment, "_currentBounds", new RectF(10, 20, 100, 100));
			var store = Array.Empty<object>();
			InvokePrivateMethod(doughnutSegment, "UpdatePosition", store);
			var actualBounds = GetPrivateField(doughnutSegment, "_actualBounds");
			var segmentRadius = GetPrivateField(doughnutSegment, "_segmentRadius");
			Assert.NotEqual(RectF.Zero, actualBounds);
			Assert.Equal(doughnutSeries.InnerRadius * 20, doughnutSegment.InnerRadius);
			Assert.Equal(doughnutSeries.Radius * 50, segmentRadius);
		}

		[Fact]
		public void IsPointInDoughnutSegment_WithValidPoint_ShouldReturnTrue()
		{
			var doughnutSegment = new DoughnutSegment();
			var doughnutSeries = new DoughnutSeries();
			doughnutSegment.Series = doughnutSeries;
			SetPrivateField(doughnutSegment, "_currentBounds", new RectF(10, 20, 100, 100));
			doughnutSegment.InnerRadius = 20;
			var result = (bool?)InvokePrivateMethod(doughnutSegment, "IsPointInDoughnutSegment", [40, 50, 50]);
			Assert.True(result);
		}

		[Fact]
		public void IsPointInDoughnutSegment_WithInvalidPoint_ShouldReturnFalse()
		{
			var doughnutSegment = new DoughnutSegment
			{
				Series = new DoughnutSeries()
			};
			SetPrivateField(doughnutSegment, "_currentBounds", new RectF(0, 0, 100, 100));
			doughnutSegment.InnerRadius = 20;
			var result = (bool?)InvokePrivateMethod(doughnutSegment, "IsPointInDoughnutSegment", [10, 10, 50]);
			Assert.False(result);
		}
		[Fact]
		public void GetSdErrorValue_WithStandardDeviation_ShouldReturnCorrectValues()
		{
			var errorBarSegment = new ErrorBarSegment();
			var errorBarSeries = new ErrorBarSeries
			{
				Type = ErrorBarType.StandardDeviation
			};
			errorBarSegment.Series = errorBarSeries;
			var values = new List<double> { 1, 2, 3, 4, 5 };
			var result = (double[]?)InvokePrivateMethod(errorBarSegment, "GetSdErrorValue", [values]);
			Assert.Equal(3.0, result?[0]);
			Assert.NotNull(result);
			Assert.Equal(1.58, Math.Round(result[1], 2));
		}

		[Fact]
		public void GetSdErrorValue_WithStandardError_ShouldReturnCorrectValues()
		{
			var errorBarSegment = new ErrorBarSegment();
			var errorBarSeries = new ErrorBarSeries
			{
				Type = ErrorBarType.StandardError
			};
			errorBarSegment.Series = errorBarSeries;
			var values = new List<double> { 1, 2, 3, 4, 5 };
			var result = (double[]?)InvokePrivateMethod(errorBarSegment, "GetSdErrorValue", [values]);
			Assert.Equal(3.0, result?[0]);
			Assert.NotNull(result?[1]);
			Assert.Equal(0.71, Math.Round(result[1], 2));
		}

		[Fact]
		public void SetData_WithCustomErrorBars_ShouldCalculateCorrectly()
		{
			var errorBarSegment = new ErrorBarSegment();
			var errorBarSeries = new ErrorBarSeries
			{
				Type = ErrorBarType.Custom,
				HorizontalErrorValues = [0.5, 0.3, 0.2, 0.4, 0.6],
				VerticalErrorValues = [0.5, 0.4, 0.3, 0.5, 0.6]
			};
			errorBarSegment.Series = errorBarSeries;
			var xValues = new List<double> { 1, 2, 3, 4, 5 };
			var yValues = new List<double> { 2, 3, 4, 5, 6 };
			errorBarSegment.SetData(xValues, yValues);
			var leftPointCollection = GetPrivateField(errorBarSegment, "_leftPointCollection") as List<Point>;
			var rightPointCollection = GetPrivateField(errorBarSegment, "_rightPointCollection") as List<Point>;
			Assert.Equal(5, leftPointCollection?.Count);
			Assert.Equal(5, rightPointCollection?.Count);
			Assert.Equal(new Point(0.5, 2), leftPointCollection?[0]);
			Assert.Equal(new Point(1.5, 2), rightPointCollection?[0]);
			Assert.Equal(new Point(1.7, 3), leftPointCollection?[1]);
			Assert.Equal(new Point(2.3, 3), rightPointCollection?[1]);
		}

		[Fact]
		public void SetData_WithPercentageErrorBars_ShouldCalculateCorrectly()
		{
			var errorBarSegment = new ErrorBarSegment();
			var errorBarSeries = new ErrorBarSeries
			{
				Type = ErrorBarType.Percentage,
				HorizontalErrorValue = 10,
				VerticalErrorValue = 20
			};
			errorBarSegment.Series = errorBarSeries;

			var xValues = new List<double> { 100, 200, 300 };
			var yValues = new List<double> { 400, 500, 600 };

			errorBarSegment.SetData(xValues, yValues);
			var leftPointCollection = GetPrivateField(errorBarSegment, "_leftPointCollection") as List<Point>;
			var rightPointCollection = GetPrivateField(errorBarSegment, "_rightPointCollection") as List<Point>;

			Assert.Equal(3, leftPointCollection?.Count);
			Assert.Equal(3, rightPointCollection?.Count);
			Assert.Equal(new Point(90, 400), leftPointCollection?[0]);
			Assert.Equal(new Point(110, 400), rightPointCollection?[0]);
			Assert.Equal(new Point(180, 500), leftPointCollection?[1]);
			Assert.Equal(new Point(220, 500), rightPointCollection?[1]);
		}

		[Fact]
		public void OnLayout_ShouldUpdateSegmentPointsCorrectly()
		{
			var sfCartesianChart = new SfCartesianChart();

			_ = new CartesianChartArea(sfCartesianChart);
			var fastLineSeries = new FastLineSeries
			{

			};

			var fastLineSegment = new FastLineSegment
			{

				_xValues = [1, 2, 3, 4, 5],
				_yValues = [10, 20, 15, 30, 25],
				Series = fastLineSeries
			};
			sfCartesianChart.Series.Add(fastLineSeries);
			var xAxis = new NumericalAxis
			{
				RenderedRect = new Rect(0, 0, 100, 50),
				VisibleRange = new DoubleRange(0, 10),
				IsVertical = false,
				IsInversed = false,
			};

			var yAxis = new NumericalAxis
			{
				RenderedRect = new Rect(0, 0, 50, 100),
				VisibleRange = new DoubleRange(0, 40),
				IsVertical = false,
				IsInversed = false,
			};
			fastLineSeries.ActualXAxis = xAxis;
			fastLineSeries.ActualYAxis = yAxis;
			SetPrivateField(fastLineSegment, "_drawPoints", Array.Empty<float>());
			fastLineSegment.OnLayout();
			float[]? drawPoints = (float[]?)GetPrivateField(fastLineSegment, "_drawPoints");
			Assert.NotNull(drawPoints);
			Assert.NotEmpty(drawPoints);
			Assert.True(drawPoints.Length > 0, "Draw points array should have a length greater than zero.");
		}
		[Fact]
		public void SetData_ShouldCalculateXAndYRangesCorrectly()
		{
			var xAxis = new NumericalAxis
			{
				RenderedRect = new Rect(0, 0, 100, 50),
				VisibleRange = new DoubleRange(0, 10),
				IsVertical = false,
				IsInversed = false,
			};

			var yAxis = new NumericalAxis
			{
				RenderedRect = new Rect(0, 0, 50, 100),
				VisibleRange = new DoubleRange(0, 40),
				IsVertical = false,
				IsInversed = false,
			};
			var fastLineSeries = new FastLineSeries
			{
				ActualXAxis = xAxis,
				ActualYAxis = yAxis
			};
			var fastLineSegment = new FastLineSegment
			{
				Series = fastLineSeries
			};
			List<double> xValues = [1, 2, 3, 4, 5];
			List<double> yValues = [10, 20, 15, 30, 25];
			fastLineSegment.SetData(xValues, yValues);
			Assert.Equal(new DoubleRange(1, 5), fastLineSeries.XRange);
			Assert.Equal(new DoubleRange(10, 30), fastLineSeries.YRange);
		}

		[Fact]
		public void SetData_ShouldSetEmptyFlagWhenNaNValuesArePresent()
		{
			var xAxis = new NumericalAxis
			{
				RenderedRect = new Rect(0, 0, 100, 50),
				VisibleRange = new DoubleRange(0, 10),
				IsVertical = false,
				IsInversed = false,
			};

			var yAxis = new NumericalAxis
			{
				RenderedRect = new Rect(0, 0, 50, 100),
				VisibleRange = new DoubleRange(0, 40),
				IsVertical = false,
				IsInversed = false,
			};
			var fastLineSeries = new FastLineSeries
			{
				ActualXAxis = xAxis,
				ActualYAxis = yAxis
			};

			var fastLineSegment = new FastLineSegment
			{
				Series = fastLineSeries
			};
			List<double> xValues = [1, double.NaN, 3, 4, 5];
			List<double> yValues = [10, 20, double.NaN, 30, 25];
			fastLineSegment.SetData(xValues, yValues);
			Assert.True(fastLineSegment.Empty);
		}

		[Fact]
		public void SetData_ShouldCorrectlyHandleNonIndexedData()
		{
			var xAxis = new NumericalAxis
			{
				RenderedRect = new Rect(0, 0, 100, 50),
				VisibleRange = new DoubleRange(0, 10),
				IsVertical = false,
				IsInversed = false,
			};

			var yAxis = new NumericalAxis
			{
				RenderedRect = new Rect(0, 0, 50, 100),
				VisibleRange = new DoubleRange(0, 40),
				IsVertical = false,
				IsInversed = false,
			};
			var fastLineSeries = new FastLineSeries
			{
				ActualXAxis = xAxis,
				ActualYAxis = yAxis
			};
			var fastLineSegment = new FastLineSegment
			{
				Series = fastLineSeries
			};
			List<double> xValues = [1, 2, 3, 4, 5];
			List<double> yValues = [10, 20, 15, 30, 25];
			fastLineSegment.SetData(xValues, yValues);
			Assert.Equal(new DoubleRange(1, 5), fastLineSeries.XRange);
			Assert.Equal(new DoubleRange(10, 30), fastLineSeries.YRange);
		}

		[Fact]
		public void UpdateLinePoints_ShouldUpdatePointsCorrectly_OnWindowsPlatform()
		{
			var xAxis = new NumericalAxis
			{
				RenderedRect = new Rect(0, 0, 100, 50),
				VisibleRange = new DoubleRange(0, 10),
				IsVertical = false,
				IsInversed = false,
			};

			var yAxis = new NumericalAxis
			{
				RenderedRect = new Rect(0, 0, 50, 100),
				VisibleRange = new DoubleRange(0, 40),
				IsVertical = false,
				IsInversed = false,
			};
			var fastLineSeries = new FastLineSeries
			{
				ActualXAxis = xAxis,
				ActualYAxis = yAxis
			};
			var fastLineSegment = new FastLineSegment
			{
				Series = fastLineSeries
			};
			var linePoints = new float[8];
			float preXPos = 10.0f;
			float preYPos = 20.0f;
			float x = 30.0f;
			float y = 40.0f;

			InvokePrivateMethod(fastLineSegment, "UpdateLinePoints", [linePoints, preXPos, preYPos, x, y]);
			Assert.Equal(x, linePoints[0]);
			Assert.Equal(y, linePoints[1]);

		}
		[Fact]
		public void ValueToPoint_ShouldCalculateCorrectly_ForNonVertical()
		{
			var xAxis = new NumericalAxis
			{
				RenderedRect = new Rect(0, 0, 100, 50),
				VisibleRange = new DoubleRange(0, 10),
				IsVertical = false,
				IsInversed = false,
			};

			var yAxis = new NumericalAxis
			{
				RenderedRect = new Rect(0, 0, 50, 100),
				VisibleRange = new DoubleRange(0, 40),
				IsVertical = false,
				IsInversed = false,
			};
			var fastLineSeries = new FastLineSeries
			{
				ActualXAxis = xAxis,
				ActualYAxis = yAxis
			};
			var fastLineSegment = new FastLineSegment
			{
				Series = fastLineSeries
			};
			double value = 5;
			double start = 0;
			double delta = 10;
			bool isInversed = false;
			bool isVertical = false;
			float width = 100;
			float height = 100;
			float leftOffset = 10;
			float topOffset = 10;
			var result = (float?)InvokePrivateMethod(fastLineSegment, "ValueToPoint", [value, start, delta, isInversed, isVertical, width, height, leftOffset, topOffset]);
			Assert.Equal(60, result);
		}

		[Fact]
		public void ValueToPoint_ShouldCalculateCorrectly_ForVerticalAndInversed()
		{
			var xAxis = new NumericalAxis
			{
				RenderedRect = new Rect(0, 0, 100, 50),
				VisibleRange = new DoubleRange(0, 10),
				IsVertical = false,
				IsInversed = false,
			};

			var yAxis = new NumericalAxis
			{
				RenderedRect = new Rect(0, 0, 50, 100),
				VisibleRange = new DoubleRange(0, 40),
				IsVertical = false,
				IsInversed = false,
			};
			var fastLineSeries = new FastLineSeries
			{
				ActualXAxis = xAxis,
				ActualYAxis = yAxis
			};
			var fastLineSegment = new FastLineSegment
			{
				Series = fastLineSeries
			};
			double value = 5;
			double start = 0;
			double delta = 10;
			bool isInversed = true;
			bool isVertical = true;
			float width = 100;
			float height = 100;
			float leftOffset = 10;
			float topOffset = 10;
			var result = (float?)InvokePrivateMethod(fastLineSegment, "ValueToPoint", [value, start, delta, isInversed, isVertical, width, height, leftOffset, topOffset]);
			Assert.Equal(60, result);
		}

		[Fact]
		public void FastScatter_SetData_ShouldCalculateXAndYRangesCorrectly()
		{
			var xAxis = new NumericalAxis
			{
				RenderedRect = new Rect(0, 0, 100, 50),
				VisibleRange = new DoubleRange(0, 10),
				IsVertical = false,
				IsInversed = false,
			};

			var yAxis = new NumericalAxis
			{
				RenderedRect = new Rect(0, 0, 50, 100),
				VisibleRange = new DoubleRange(0, 40),
				IsVertical = false,
				IsInversed = false,
			};
			var fastScatterSeries = new FastScatterSeries
			{
				ActualXAxis = xAxis,
				ActualYAxis = yAxis,
				PointsCount = 5
			};
			var fastScatterSegment = new FastScatterSegment
			{
				Series = fastScatterSeries
			};
			List<double> xValues = [1, 2, 3, 4, 5];
			List<double> yValues = [10, 20, 15, 30, 25];
			fastScatterSegment.SetData(xValues, yValues);
			Assert.Equal(new DoubleRange(1, 5), fastScatterSeries.XRange);
			Assert.Equal(new DoubleRange(10, 30), fastScatterSeries.YRange);
		}

		[Fact]
		public void FastScatter_SetData_ShouldSetEmptyFlagWhenNaNValuesArePresent()
		{
			var xAxis = new CategoryAxis
			{
				RenderedRect = new Rect(0, 0, 100, 50),
				VisibleRange = new DoubleRange(0, 10),
				IsVertical = false,
				IsInversed = false,
			};

			var yAxis = new NumericalAxis
			{
				RenderedRect = new Rect(0, 0, 50, 100),
				VisibleRange = new DoubleRange(0, 40),
				IsVertical = false,
				IsInversed = false,
			};
			var fastScatterSeries = new FastScatterSeries
			{
				ActualXAxis = xAxis,
				ActualYAxis = yAxis,
				PointsCount = 5
			};

			var fastScatterSegment = new FastScatterSegment
			{
				Series = fastScatterSeries
			};
			List<double> xValues = [1, double.NaN, 3, 4, 5];
			List<double> yValues = [10, 20, double.NaN, 30, 25];
			fastScatterSegment.SetData(xValues, yValues);
			Assert.False(fastScatterSegment.Empty);
		}

		[Fact]
		public void FastScatter_OnLayout_ShouldUpdateSegmentPointsCorrectly()
		{
			var sfCartesianChart = new SfCartesianChart();

			_ = new CartesianChartArea(sfCartesianChart);
			var fastScatterSeries = new FastScatterSeries
			{
				PointsCount = 5
			};

			var fastScatterSegment = new FastScatterSegment
			{
				XValues = [1, 2, 3, 4, 5],
				YValues = [10, 20, 15, 30, 25],
				Series = fastScatterSeries
			};
			sfCartesianChart.Series.Add(fastScatterSeries);
			var xAxis = new NumericalAxis
			{
				RenderedRect = new Rect(0, 0, 100, 50),
				VisibleRange = new DoubleRange(0, 10),
				IsVertical = false,
				IsInversed = false,
			};

			var yAxis = new NumericalAxis
			{
				RenderedRect = new Rect(0, 0, 50, 100),
				VisibleRange = new DoubleRange(0, 40),
				IsVertical = false,
				IsInversed = false,
			};
			fastScatterSeries.ActualXAxis = xAxis;
			fastScatterSeries.ActualYAxis = yAxis;
			SetPrivateField(fastScatterSegment, "_fastScatterPlottingPoints", new List<PointF>());
			fastScatterSegment.OnLayout();
			var plottingPoints = GetPrivateField(fastScatterSegment, "_fastScatterPlottingPoints") as List<PointF>;
			Assert.NotNull(plottingPoints);
			Assert.NotEmpty(plottingPoints);
			Assert.Equal(fastScatterSeries.PointsCount, plottingPoints.Count);
		}
	}
}
