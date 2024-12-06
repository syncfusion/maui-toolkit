using Syncfusion.Maui.Toolkit.Charts;
using Core = Syncfusion.Maui.Toolkit;

namespace Syncfusion.Maui.Toolkit.UnitTest
{
	public class UtilsUnitTests : BaseUnitTest
	{
		[Fact]
		public void IsEqual_SameColors_ReturnsTrue()
		{
			var color1 = Colors.Red;
			var color2 = Colors.Red;
			var result = ChartColor.IsEqual(color1, color2);
			Assert.True(result);
		}

		[Fact]
		public void IsEqual_DifferentColors_ReturnsFalse()
		{
			var color1 = Colors.Red;
			var color2 = Colors.Blue;
			var result = ChartColor.IsEqual(color1, color2);
			Assert.False(result);
		}

		[Fact]
		public void IsEmpty_NullColor_ReturnsTrue()
		{
			var result = ChartColor.IsEmpty(null);
			Assert.True(result);
		}

		[Fact]
		public void IsEmpty_TransparentColor_ReturnsTrue()
		{
			var result = ChartColor.IsEmpty(Colors.Transparent);
			Assert.True(result);
		}

		[Fact]
		public void IsEmpty_NonTransparentColor_ReturnsFalse()
		{
			var result = ChartColor.IsEmpty(Colors.Red);
			Assert.False(result);
		}

		[Fact]
		public void CreateEmpty_ReturnsMaxValueColor()
		{
			var result = ChartColor.CreateEmpty();
			Assert.Equal(new Color(int.MaxValue), result);
		}

		[Fact]
		public void CreateTransparent_ReturnsTransparentColor()
		{
			var result = ChartColor.CreateTransparent();
			Assert.Equal(Colors.Transparent, result);
		}

		[Fact]
		public void CreateColor_IntValues_ReturnsCorrectColor()
		{
			var result = ChartColor.CreateColor(255, 0, 0, 255);
			Assert.Equal(new Color(1.0f, 0.0f, 0.0f, 1.0f), result);
		}

		[Fact]
		public void CreateColor_FloatValues_ReturnsCorrectColor()
		{
			var result = ChartColor.CreateColor(1.0f, 0.0f, 0.0f, 1.0f);
			Assert.Equal(new Color(1.0f, 0.0f, 0.0f, 1.0f), result);
		}

		[Fact]
		public void EqualDoubleValues_SameValues_ReturnsTrue()
		{
			double x = 1.2345;
			double y = 1.2345;
			var result = ChartDataUtils.EqualDoubleValues(x, y);
			Assert.True(result);
		}

		[Fact]
		public void EqualDoubleValues_DifferentValues_ReturnsFalse()
		{
			double x = 1.2345;
			double y = 1.2346;
			var result = ChartDataUtils.EqualDoubleValues(x, y);
			Assert.False(result);
		}

		[Fact]
		public void IncreaseInterval_DaysInterval_ReturnsCorrectDate()
		{
			var date = new DateTime(2023, 1, 1);
			double interval = 5;
			var intervalType = DateTimeIntervalType.Days;
			var expected = new DateTime(2023, 1, 6);
			var result = ChartDataUtils.IncreaseInterval(date, interval, intervalType);
			Assert.Equal(expected, result);
		}

		[Fact]
		public void IncreaseInterval_YearsInterval_ReturnsCorrectDate()
		{
			var date = new DateTime(2023, 1, 1);
			double interval = 1.5;
			var intervalType = DateTimeIntervalType.Years;
			var expected = new DateTime(2024, 7, 1);
			var result = ChartDataUtils.IncreaseInterval(date, interval, intervalType);
			Assert.Equal(expected, result);
		}

		[Fact]
		public void SubtractThickness_ShouldReturnExpectedRect()
		{
			var rect = new Rect(10, 10, 100, 100);
			var thickness = new Thickness(5, 5, 10, 10);
			var expected = new RectF(15, 15, 85, 85);
			var result = ChartMath.SubtractThickness(rect, thickness);
			Assert.Equal(expected, result);
		}

		[Theory]
		[InlineData(0, 0)]
		[InlineData(90, (float)Math.PI / 2)]
		[InlineData(180, (float)Math.PI)]
		[InlineData(360, (float)(2 * Math.PI))]
		public void DegreeToRadian_ShouldReturnExpectedRadian(float angle, float expected)
		{
			var result = ChartMath.DegreeToRadian(angle);
			Assert.Equal(expected, result, 5);
		}

		[Theory]
		[InlineData(0, 0)]
		[InlineData(Math.PI / 2, 90)]
		[InlineData(Math.PI, 180)]
		[InlineData(2 * Math.PI, 360)]
		public void RadianToDegree_ShouldReturnExpectedDegree(double radian, double expected)
		{
			var result = ChartMath.RadianToDegree(radian);
			Assert.Equal(expected, result, 5);
		}

		[Theory]
		[InlineData(5, 1, 10, 5)]
		[InlineData(-5, 1, 10, 1)]
		[InlineData(15, 1, 10, 10)]
		public void MinMax_ShouldReturnExpectedValue(double value, double min, double max, double expected)
		{
			var result = ChartMath.MinMax(value, min, max);
			Assert.Equal(expected, result);
		}

		[Theory]
		[InlineData(5.5, 2, true, 6)]
		[InlineData(5.5, 2, false, 4)]
		[InlineData(10, 3, true, 12)]
		[InlineData(10, 3, false, 9)]
		public void Round_ShouldReturnExpectedValue(double x, double div, bool up, double expected)
		{
			var result = ChartMath.Round(x, div, up);
			Assert.Equal(expected, result);
		}

		[Theory]
		[InlineData(5, 5, 3, 3, 315)]
		[InlineData(5, 5, 7, 3, 45)]
		[InlineData(5, 5, 5, 7, 180)]
		public void GetMidValue_ShouldReturnCorrectAngle(float centerX, float centerY, float x, float y, float expectedAngle)
		{
			var centerPoint = new PointF(centerX, centerY);
			float result = ChartUtils.GetMidValue(centerPoint, x, y);
			Assert.Equal(expectedAngle, result, 1);
		}

		[Theory]
		[InlineData(5, 5, new float[] { 3, 3, 7, 3 }, new float[] { 315, 45 })]
		[InlineData(5, 5, new float[] { 5, 7, 5, 3 }, new float[] { 180, 0 })]
		public void GetMidAngles_ShouldReturnCorrectAngles(float centerX, float centerY, float[] midPoints, float[] expectedAngles)
		{
			var center = new PointF(centerX, centerY);
			var midPointList = new List<float>(midPoints);
			var result = ChartUtils.GetMidAngles(center, midPointList);
			Assert.Equal(expectedAngles.Length, result.Count);
			Assert.Equal(expectedAngles[0], result[0], 1);
			Assert.Equal(expectedAngles[1], result[1], 1);

		}

		[Theory]
		[InlineData(123.45, 123.45)]
		[InlineData("123.45", 123.45)]
		[InlineData("01/01/2023", 44927)]
		public void ConvertToDouble_ShouldReturnExpectedResult(object val, double expected)
		{
			double result = ChartUtils.ConvertToDouble(val);
			Assert.Equal(expected, result, 2);
		}

		[Fact]
		public void Clone_ShouldReturnClonedObjectWithSameProperties()
		{
			var chartDataLabelStyle = new ChartDataLabelStyle
			{
				FontFamily = "Arial",
				FontAttributes = FontAttributes.Bold,
				FontSize = 12,
				Rect = new Rect(0, 0, 100, 50),

			};
			var clonedStyle = chartDataLabelStyle.Clone();
			Assert.NotNull(clonedStyle);
			Assert.Equal(chartDataLabelStyle.FontFamily, clonedStyle.FontFamily);
			Assert.Equal(chartDataLabelStyle.FontAttributes, clonedStyle.FontAttributes);
			Assert.Equal(chartDataLabelStyle.FontSize, clonedStyle.FontSize);
			Assert.Equal(chartDataLabelStyle.Rect, clonedStyle.Rect);
			Assert.Equal(chartDataLabelStyle.Parent, clonedStyle.Parent);
		}

		[Theory]
		[InlineData(new double[] { 1.0, 2.0, 3.0, 4.0, 5.0 }, 3.0, 0, 4, 2)]
		[InlineData(new double[] { 1.0, 2.0, 3.0, 4.0, 5.0 }, 1.0, 0, 4, 0)]
		[InlineData(new double[] { 1.0, 2.0, 3.0, 4.0, 5.0 }, 2.5, 0, 4, 2)]
		public void BinarySearch_ShouldReturnExpectedIndex(double[] xValues, double touchValue, int min, int max, int expectedIndex)
		{
			var xValuesList = new List<double>(xValues);
			var result = ChartUtils.BinarySearch(xValuesList, touchValue, min, max);
			Assert.Equal(expectedIndex, result);
		}

		[Theory]
		[InlineData(null, "")]
		[InlineData(123, "123")]
		[InlineData("test", "test")]
		[InlineData(true, "True")]
		public void ToString_ShouldReturnExpectedString(object? input, string expected)
		{
			var result = ChartUtils.Tostring(input);
			Assert.Equal(expected, result);
		}

		[Theory]
		[InlineData(0, 0, 0, 1, 1, 1)]
		[InlineData(1, 1, 1, 0, 0, 0)]
		[InlineData(0.2, 0.2, 0.2, 1, 1, 1)]
		public void GetContrastColor_ShouldReturnExpectedColor(float red, float green, float blue, float expectedRed, float expectedGreen, float expectedBlue)
		{
			var color = new Color(red, green, blue);
			var expectedColor = new Color(expectedRed, expectedGreen, expectedBlue);
			var result = ChartUtils.GetContrastColor(color);
			Assert.Equal(expectedColor, result);
		}

		[Theory]
		[InlineData(100, 50, 0, 100, 50)]
		public void GetRotatedSize_ShouldReturnExpectedSize(double width, double height, float degrees, double expectedWidth, double expectedHeight)
		{
			var measuredSize = new Size(width, height);
			var result = ChartUtils.GetRotatedSize(measuredSize, degrees);
			Assert.Equal(new Size(expectedWidth, expectedHeight), result);
		}

		[Fact]
		public void GetContrastColor_ShouldReturnBlack_WhenColorIsNull()
		{
			var result = ChartUtils.GetContrastColor(null);
			Assert.Equal(Colors.Black, result);
		}

		[Fact]
		public void GetTextColorBasedOnChartBackground_NullBackground_ShouldReturnBlack()
		{
			var sfCartesianChart = new SfCartesianChart();
			var result = sfCartesianChart.GetTextColorBasedOnChartBackground();
			Assert.Equal(Colors.Black, result);
		}

		[Theory]
		[InlineData(ChartLegendIconType.Circle, Core.ShapeType.Circle)]
		[InlineData(ChartLegendIconType.Rectangle, Core.ShapeType.Rectangle)]
		[InlineData(ChartLegendIconType.HorizontalLine, Core.ShapeType.HorizontalLine)]
		[InlineData(ChartLegendIconType.Diamond, Core.ShapeType.Diamond)]
		[InlineData(ChartLegendIconType.Pentagon, Core.ShapeType.Pentagon)]
		[InlineData(ChartLegendIconType.Triangle, Core.ShapeType.Triangle)]
		[InlineData(ChartLegendIconType.InvertedTriangle, Core.ShapeType.InvertedTriangle)]
		[InlineData(ChartLegendIconType.Cross, Core.ShapeType.Cross)]
		[InlineData(ChartLegendIconType.Plus, Core.ShapeType.Plus)]
		[InlineData(ChartLegendIconType.Hexagon, Core.ShapeType.Hexagon)]
		[InlineData(ChartLegendIconType.VerticalLine, Core.ShapeType.VerticalLine)]
		[InlineData(ChartLegendIconType.SeriesType, Core.ShapeType.Custom)]
		public void GetShapeType_ChartLegendIconType_ShouldReturnExpectedShapeType(ChartLegendIconType iconType, Core.ShapeType expectedShapeType)
		{
			var result = ChartUtils.GetShapeType(iconType);
			Assert.Equal(expectedShapeType, result);
		}

		[Fact]
		public void GetShapeType_ShapeType_ShouldReturnExpectedShapeType()
		{
			_ = new ChartMarkerSettings();
			var expectedShape = Core.ShapeType.Circle;
			var result = ChartUtils.GetShapeType(Charts.ShapeType.Circle);
			Assert.Equal(expectedShape, result);
		}

		[Theory]
		[InlineData(0, 0, 100, 100, 50, 0, 50, 100, 100)]
		[InlineData(10, 20, 200, 150, 30, 10, 50, 200, 150)]
		public void GetSeriesClipRect_ShouldReturnExpectedRect(double x, double y, double width, double height, double titleHeight, double expectedX, double expectedY, double expectedWidth, double expectedHeight)
		{
			var seriesClipRect = new Rect(x, y, width, height);
			var result = ChartUtils.GetSeriesClipRect(seriesClipRect, titleHeight);
			var expectedRect = new Rect(expectedX, expectedY, expectedWidth, expectedHeight);
			Assert.Equal(expectedRect, result);
		}

		[Fact]
		public void SegmentContains_LineSegment_ShouldReturnTrue()
		{
			var lineSegment = new LineSegment { X1 = 0, Y1 = 0, X2 = 10, Y2 = 10 };
			var touchPoint = new PointF(5, 5);
			var lineSeries = new LineSeries();
			var result = ChartUtils.SegmentContains(lineSegment, touchPoint, lineSeries);
			Assert.True(result);
		}

		[Fact]
		public void SegmentContains_LineSegment_ShouldReturnFalse()
		{
			var lineSegment = new LineSegment { X1 = 0, Y1 = 0, X2 = 10, Y2 = 10 };
			var touchPoint = new PointF(15, 15);
			var lineSeries = new LineSeries();
			var result = ChartUtils.SegmentContains(lineSegment, touchPoint, lineSeries);
			Assert.False(result);
		}

		[Fact]
		public void SegmentContains_StepLineSegment_ShouldReturnTrue()
		{
			var stepLineSegment = new StepLineSegment { X1 = 0, Y1 = 0, X2 = 10, Y2 = 10 };
			var touchPoint = new PointF(5, 5);
			var stepLineSeries = new StepLineSeries();
			var result = ChartUtils.SegmentContains(stepLineSegment, touchPoint, stepLineSeries);
			Assert.True(result);
		}


		[Fact]
		public void SegmentContains_SplineSegment_ShouldReturnTrue()
		{
			var splineSegment = new SplineSegment { X1 = 0, Y1 = 0, X2 = 10, Y2 = 10, StartControlX = 2, StartControlY = 2, EndControlX = 8, EndControlY = 8 };
			var touchPoint = new PointF(5, 5);
			var splineSeries = new SplineSeries();
			var result = ChartUtils.SegmentContains(splineSegment, touchPoint, splineSeries);
			Assert.True(result);
		}

		[Fact]
		public void SegmentContains_SplineSegment_ShouldReturnFalse()
		{
			var splineSegment = new SplineSegment { X1 = 0, Y1 = 0, X2 = 10, Y2 = 10, StartControlX = 2, StartControlY = 2, EndControlX = 8, EndControlY = 8 };
			var touchPoint = new PointF(15, 15);
			var splineSeries = new SplineSeries();
			var result = ChartUtils.SegmentContains(splineSegment, touchPoint, splineSeries);
			Assert.False(result);
		}

		[Fact]
		public void LineContains_ShouldReturnFalse()
		{
			var segmentStartPoint = new PointF(0, 0);
			var segmentEndPoint = new PointF(10, 10);
			var touchStartPoint = new PointF(15, 15);
			var touchEndPoint = new PointF(20, 20);
			var result = ChartUtils.LineContains(segmentStartPoint, segmentEndPoint, touchStartPoint, touchEndPoint);
			Assert.False(result);
		}

		[Theory]
		[InlineData(5, 5, true)]
		[InlineData(15, 15, false)]
		public void IsPathContains_ShouldReturnExpectedResult(float xPos, float yPos, bool expected)
		{
			var segPoints = new List<PointF>
		{
			new(0, 0),
			new(10, 0),
			new(10, 10),
			new(0, 10)
		};
			var result = ChartUtils.IsPathContains(segPoints, xPos, yPos);
			Assert.Equal(expected, result);
		}

		[Theory]
		[InlineData(5, 5, true)]
		[InlineData(15, 15, false)]
		public void IsAreaContains_ShouldReturnExpectedResult(float xPos, float yPos, bool expected)
		{
			var segPoints = new List<PointF>
		{
			new(0, 0),
			new(10, 0),
			new(10, 10),
			new(0, 10)
		};
			var result = ChartUtils.IsAreaContains(segPoints, xPos, yPos);
			Assert.Equal(expected, result);
		}

		[Theory]
		[InlineData(0, 0, 1, 1, 2, 2, 0)]
		[InlineData(0, 0, 1, 1, 2, 0, 1)]
		[InlineData(0, 0, 1, 1, 0, 2, 2)]
		public void GetPointDirection_ShouldReturnExpectedResult(float x1, float y1, float x2, float y2, float x3, float y3, int expected)
		{
			var point1 = new PointF(x1, y1);
			var point2 = new PointF(x2, y2);
			var point3 = new PointF(x3, y3);
			var result = (int?)InvokeStaticPrivateMethodClass(typeof(ChartUtils), "GetPointDirection", [point1, point2, point3]);
			Assert.Equal(expected, result);
		}

		[Fact]
		public void ToColor_ShouldReturnExpectedColor()
		{
			var solidBrush = new SolidColorBrush(Colors.Red);
			var result = solidBrush.ToColor();
			Assert.Equal(Colors.Red, result);
		}

		[Fact]
		public void ToColor_ShouldReturnTransparentForNullBrush()
		{
			Brush? brush = null;
			var result = brush.ToColor();
			Assert.Equal(Colors.Transparent, result);
		}

		[Fact]
		public void IsOverlap_ShouldReturnTrueForOverlappingRects()
		{
			var rect1 = new Rect(0, 0, 10, 10);
			var rect2 = new Rect(5, 5, 10, 10);
			var result = rect1.IsOverlap(rect2);
			Assert.True(result);
		}

		[Fact]
		public void IsOverlap_ShouldReturnFalseForNonOverlappingRects()
		{
			var rect1 = new Rect(0, 0, 10, 10);
			var rect2 = new Rect(20, 20, 10, 10);
			var result = rect1.IsOverlap(rect2);
			Assert.False(result);
		}

		[Theory]
		[InlineData(10, 5, 30, 2.387)]
		[InlineData(15, 7.5, 45, 3.581)]
		[InlineData(20, 10, 60, 4.775)]
		public void CalculateAngleDeviation_ShouldReturnExpectedResult(float calcRadius, float radius, float angleDifferent, double expected)
		{
			var result = ChartUtils.CalculateAngleDeviation(calcRadius, radius, angleDifferent);
			Assert.Equal(expected, result, 3);
		}

		[Theory]
		[InlineData(0, 1, 0)]
		[InlineData(90, 0, 1)]
		[InlineData(180, -1, 0)]
		[InlineData(270, 0, -1)]
		[InlineData(360, 1, 0)]
		public void AngleToVector_ShouldReturnExpectedResult(double angle, double expectedX, double expectedY)
		{
			var result = ChartUtils.AngleToVector(angle);
			Assert.Equal(expectedX, result.X, 6);
			Assert.Equal(expectedY, result.Y, 6);
		}

		[Fact]
		public void CompareStackingSeries_ShouldReturnTrueForSameType()
		{
			var series1 = new StackingColumnSeries();
			var series2 = new StackingColumnSeries();
			var result = series1.CompareStackingSeries(series2);
			Assert.True(result);
		}

		[Fact]
		public void CompareStackingSeries_ShouldReturnFalseForDifferentType()
		{
			var series1 = new StackingColumnSeries();
			var series2 = new StackingLineSeries();
			var result = series1.CompareStackingSeries(series2);
			Assert.False(result);
		}

		[Fact]
		public void Union_WithMultipleValues_ReturnsCorrectRange()
		{
			double[] values = [1.0, 2.0, 3.0, 4.0];
			DoubleRange result = DoubleRange.Union(values);
			Assert.Equal(1.0, result.Start);
			Assert.Equal(4.0, result.End);
		}

		[Fact]
		public void Union_WithNaNValues_ReturnsNaNRange()
		{
			double[] values = [double.NaN, 2.0, 3.0];
			DoubleRange result = DoubleRange.Union(values);
			Assert.True(double.IsNaN(result.Start));
			Assert.Equal(3.0, result.End);
		}

		[Fact]
		public void Union_WithTwoRanges_ReturnsCorrectUnion()
		{
			DoubleRange range1 = new DoubleRange(1.0, 3.0);
			DoubleRange range2 = new DoubleRange(2.0, 4.0);
			DoubleRange result = DoubleRange.Union(range1, range2);
			Assert.Equal(1.0, result.Start);
			Assert.Equal(4.0, result.End);
		}

		[Fact]
		public void Union_WithEmptyRange_ReturnsNonEmptyRange()
		{
			DoubleRange emptyRange = DoubleRange.Empty;
			DoubleRange nonEmptyRange = new DoubleRange(1.0, 2.0);
			DoubleRange result = DoubleRange.Union(emptyRange, nonEmptyRange);
			Assert.Equal(1.0, result.Start);
			Assert.Equal(2.0, result.End);
		}

		[Fact]
		public void Union_WithRangeAndValue_ReturnsCorrectUnion()
		{
			DoubleRange range = new DoubleRange(1.0, 3.0);
			double value = 4.0;
			DoubleRange result = DoubleRange.Union(range, value);
			Assert.Equal(1.0, result.Start);
			Assert.Equal(4.0, result.End);
		}

		[Fact]
		public void Scale_WithNonEmptyRange_ReturnsScaledRange()
		{
			DoubleRange range = new DoubleRange(1.0, 3.0);
			double scaleValue = 0.5;
			DoubleRange result = DoubleRange.Scale(range, scaleValue);
			Assert.Equal(0.0, result.Start);
			Assert.Equal(4.0, result.End);
		}

		[Fact]
		public void Scale_WithEmptyRange_ReturnsEmptyRange()
		{
			DoubleRange emptyRange = DoubleRange.Empty;
			double scaleValue = 0.5;
			DoubleRange result = DoubleRange.Scale(emptyRange, scaleValue);
			Assert.True(result.IsEmpty);
		}

		[Fact]
		public void Offset_WithNonEmptyRange_ReturnsOffsetRange()
		{
			DoubleRange range = new DoubleRange(1.0, 3.0);
			double offsetValue = 2.0;
			DoubleRange result = DoubleRange.Offset(range, offsetValue);
			Assert.Equal(3.0, result.Start);
			Assert.Equal(5.0, result.End);
		}

		[Fact]
		public void Offset_WithEmptyRange_ReturnsEmptyRange()
		{
			DoubleRange emptyRange = DoubleRange.Empty;
			double offsetValue = 2.0;
			DoubleRange result = DoubleRange.Offset(emptyRange, offsetValue);
			Assert.True(result.IsEmpty);
		}

		[Fact]
		public void Exclude_ExcluderEqualToRange_ReturnsFalse()
		{
			DoubleRange range = new DoubleRange(1.0, 5.0);
			DoubleRange excluder = new DoubleRange(1.0, 5.0);
			bool result = DoubleRange.Exclude(range, excluder, out DoubleRange leftRange, out DoubleRange rightRange);
			Assert.False(result);
			Assert.True(leftRange.IsEmpty);
			Assert.True(rightRange.IsEmpty);
		}

		[Fact]
		public void Exclude_EmptyRange_ReturnsFalse()
		{
			DoubleRange range = DoubleRange.Empty;
			DoubleRange excluder = new DoubleRange(1.0, 5.0);
			bool result = DoubleRange.Exclude(range, excluder, out DoubleRange leftRange, out DoubleRange rightRange);
			Assert.False(result);
			Assert.True(leftRange.IsEmpty);
			Assert.True(rightRange.IsEmpty);
		}

		[Fact]
		public void Exclude_EmptyExcluder_ReturnsFalse()
		{
			DoubleRange range = new DoubleRange(1.0, 5.0);
			DoubleRange excluder = DoubleRange.Empty;
			bool result = DoubleRange.Exclude(range, excluder, out DoubleRange leftRange, out DoubleRange rightRange);
			Assert.False(result);
			Assert.True(leftRange.IsEmpty);
			Assert.True(rightRange.IsEmpty);
		}

		[Fact]
		public void Intersects_WithOverlappingRanges_ReturnsTrue()
		{
			DoubleRange range1 = new DoubleRange(1.0, 5.0);
			DoubleRange range2 = new DoubleRange(4.0, 6.0);
			bool result = range1.Intersects(range2);
			Assert.True(result);
		}

		[Fact]
		public void Intersects_WithNonOverlappingRanges_ReturnsFalse()
		{
			DoubleRange range1 = new DoubleRange(1.0, 3.0);
			DoubleRange range2 = new DoubleRange(4.0, 6.0);
			bool result = range1.Intersects(range2);
			Assert.False(result);
		}

		[Fact]
		public void Intersects_WithEmptyRange_ReturnsFalse()
		{
			DoubleRange range1 = DoubleRange.Empty;
			DoubleRange range2 = new DoubleRange(1.0, 3.0);
			bool result = range1.Intersects(range2);
			Assert.False(result);
		}

		[Fact]
		public void Intersects_WithStartAndEndValues_ReturnsTrue()
		{
			DoubleRange range = new DoubleRange(1.0, 5.0);
			bool result = range.Intersects(4.0, 6.0);
			Assert.True(result);
		}

		[Fact]
		public void Intersects_WithStartAndEndValues_ReturnsFalse()
		{
			DoubleRange range = new DoubleRange(1.0, 3.0);
			bool result = range.Intersects(4.0, 6.0);
			Assert.False(result);
		}

		[Fact]
		public void Inside_WithValueInsideRange_ReturnsTrue()
		{
			DoubleRange range = new DoubleRange(1.0, 5.0);
			bool result = range.Inside(3.0);
			Assert.True(result);
		}

		[Fact]
		public void Inside_WithValueOutsideRange_ReturnsFalse()
		{
			DoubleRange range = new DoubleRange(1.0, 5.0);
			bool result = range.Inside(6.0);
			Assert.False(result);
		}

		[Fact]
		public void Inside_WithRangeInsideRange_ReturnsTrue()
		{
			DoubleRange range1 = new DoubleRange(1.0, 5.0);
			DoubleRange range2 = new DoubleRange(2.0, 4.0);
			bool result = range1.Inside(range2);
			Assert.True(result);
		}

		[Fact]
		public void Inside_WithRangeOutsideRange_ReturnsFalse()
		{
			DoubleRange range1 = new DoubleRange(1.0, 5.0);
			DoubleRange range2 = new DoubleRange(0.0, 6.0);
			bool result = range1.Inside(range2);
			Assert.False(result);
		}

		[Fact]
		public void Equals_WithEqualRanges_ReturnsTrue()
		{
			DoubleRange range1 = new DoubleRange(1.0, 5.0);
			DoubleRange range2 = new DoubleRange(1.0, 5.0);
			bool result = range1.Equals(range2);
			Assert.True(result);
		}

		[Fact]
		public void Equals_WithDifferentRanges_ReturnsFalse()
		{
			DoubleRange range1 = new DoubleRange(1.0, 5.0);
			DoubleRange range2 = new DoubleRange(2.0, 6.0);
			bool result = range1.Equals(range2);
			Assert.False(result);
		}

		[Fact]
		public void SetPropertyName_WithValidProperty_ReturnsTrue()
		{
			var fastReflection = new FastReflection();
			var testObject = new TestClass();
			bool result = fastReflection.SetPropertyName(nameof(TestClass.IntProperty), testObject);
			Assert.True(result);
		}

		[Fact]
		public void SetPropertyName_WithInvalidProperty_ReturnsFalse()
		{
			var fastReflection = new FastReflection();
			var testObject = new TestClass();
			bool result = fastReflection.SetPropertyName("NonExistentProperty", testObject);
			Assert.False(result);
		}

		[Fact]
		public void GetValue_WithValidProperty_ReturnsValue()
		{
			var fastReflection = new FastReflection();
			var testObject = new TestClass { IntProperty = 42 };
			fastReflection.SetPropertyName(nameof(TestClass.IntProperty), testObject);
			var value = fastReflection.GetValue(testObject);
			Assert.Equal(42, value);
		}

		[Fact]
		public void GetValue_WithInvalidProperty_ReturnsNull()
		{
			var fastReflection = new FastReflection();
			var testObject = new TestClass();
			var value = fastReflection.GetValue(testObject);
			Assert.Null(value);
		}

		[Fact]
		public void IsArray_WithArrayProperty_ReturnsTrue()
		{
			var fastReflection = new FastReflection();
			var testObject = new TestClass();
			fastReflection.SetPropertyName(nameof(TestClass.IntArrayProperty), testObject);
			bool result = fastReflection.IsArray(testObject);
			Assert.True(result);
		}

		[Fact]
		public void IsArray_WithNonArrayProperty_ReturnsFalse()
		{
			var fastReflection = new FastReflection();
			var testObject = new TestClass();
			fastReflection.SetPropertyName(nameof(TestClass.IntProperty), testObject);
			bool result = fastReflection.IsArray(testObject);
			Assert.False(result);
		}
	}
}
