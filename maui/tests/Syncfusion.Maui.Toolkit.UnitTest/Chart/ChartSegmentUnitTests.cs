using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.Toolkit.UnitTest
{
    public class ChartSegmentUnitTests
    {

        [Fact]
        public void WaterfallSegment_Constructor_InitializesDefaultsCorrectly()
        {
            WaterfallSegment waterfallSegment = new WaterfallSegment();

            Assert.NotNull(waterfallSegment);
            Assert.Equal(0f, waterfallSegment.Left);
            Assert.Equal(0f, waterfallSegment.Right);
            Assert.Equal(0f, waterfallSegment.Top);
            Assert.Equal(0f, waterfallSegment.Bottom);
        }

        [Theory]
        [InlineData(1f)]
        [InlineData(0f)]
        [InlineData(-0.9f)]
        public void WaterfallSegment_Left_SetValue_ReturnsExpectedValue(float left)
        {
            WaterfallSegment waterfallSegment = new WaterfallSegment();
            waterfallSegment.Left = left;

            Assert.Equal(left, waterfallSegment.Left);
        }

        [Theory]
        [InlineData(3f)]
        [InlineData(0.2f)]
        [InlineData(10f)]
        public void WaterfallSegment_Right_SetValue_ReturnsExpectedValue(float right)
        {
            WaterfallSegment waterfallSegment = new WaterfallSegment();
            waterfallSegment.Right = right;

            Assert.Equal(right, waterfallSegment.Right);
        }

        [Theory]
        [InlineData(0.9f)]
        [InlineData(-0.2f)]
        [InlineData(5f)]
        public void WaterfallSegment_Bottom_SetValue_ReturnsExpectedValue(float bottom)
        {
            WaterfallSegment waterfallSegment = new WaterfallSegment();
            waterfallSegment.Bottom = bottom;

            Assert.Equal(bottom, waterfallSegment.Bottom);
        }

        [Theory]
        [InlineData(1.3f)]
        [InlineData(2f)]
        [InlineData(12f)]
        public void WaterfallSegment_Top_SetValue_ReturnsExpectedValue(float top)
        {
            WaterfallSegment waterfallSegment = new WaterfallSegment();
            waterfallSegment.Top = top;

            Assert.Equal(top, waterfallSegment.Top);
        }

        [Fact]
        public void SplineSegment_Constructor_InitializesDefaultsCorrectly()
        {
            SplineSegment splineSegment = new SplineSegment();

            Assert.NotNull(splineSegment);
            Assert.Equal(0f, splineSegment.X1);
            Assert.Equal(0f, splineSegment.X2);
            Assert.Equal(0f, splineSegment.Y1);
            Assert.Equal(0f, splineSegment.Y2);
        }

        [Theory]
        [InlineData(3f)]
        [InlineData(0.2f)]
        [InlineData(10f)]
        public void SplineSegment_X1_SetValue_ReturnsExpectedValue(float x1)
        {
            SplineSegment splineSegment = new SplineSegment();
            splineSegment.X1 = x1;

            Assert.Equal(x1, splineSegment.X1);
        }

        [Theory]
        [InlineData(3f)]
        [InlineData(0.2f)]
        [InlineData(-2.9f)]
        public void SplineSegment_Y1_SetValue_ReturnsExpectedValue(float y1)
        {
            SplineSegment splineSegment = new SplineSegment();
            splineSegment.Y1 = y1;

            Assert.Equal(y1, splineSegment.Y1);
        }

        [Theory]
        [InlineData(1.3f)]
        [InlineData(0.2f)]
        [InlineData(-2f)]
        public void SplineSegment_X2_SetValue_ReturnsExpectedValue(float x2)
        {
            SplineSegment splineSegment = new SplineSegment();
            splineSegment.X2 = x2;

            Assert.Equal(x2, splineSegment.X2);
        }

        [Theory]
        [InlineData(3f)]
        [InlineData(-1f)]
        [InlineData(10f)]
        public void SplineSegment_Y2_SetValue_ReturnsExpectedValue(float y2)
        {
            SplineSegment splineSegment = new SplineSegment();
            splineSegment.Y2 = y2;

            Assert.Equal(y2, splineSegment.Y2);
        }

        [Fact]
        public void ScatterSegment_Constructor_InitializesDefaultsCorrectly()
        {
            ScatterSegment scatterSegment = new ScatterSegment();

            Assert.NotNull(scatterSegment);
            Assert.Equal(0f, scatterSegment.PointWidth);
            Assert.Equal(0f, scatterSegment.PointHeight);
            Assert.Equal(0f, scatterSegment.CenterX);
            Assert.Equal(0f, scatterSegment.CenterY);
        }



        [Theory]
        [InlineData(0.2f)]
        [InlineData(1.0f)]
        [InlineData(1.2f)]
        public void PointWidth_SetValue_ReturnsExpectedValue(float pointWidth)
        {
            ScatterSegment scatterSegment = new ScatterSegment();
            scatterSegment.PointWidth = pointWidth;

            Assert.Equal(pointWidth, scatterSegment.PointWidth);
        }

        [Theory]
        [InlineData(0.2f)]
        [InlineData(1.0f)]
        [InlineData(1.2f)]
        public void PointHeight_SetValue_ReturnsExpectedValue(float pointHeight)
        {
            ScatterSegment scatterSegment = new ScatterSegment();
            scatterSegment.PointWidth = pointHeight;

            Assert.Equal(pointHeight, scatterSegment.PointWidth);
        }

        [Theory]
        [InlineData(0.2f)]
        [InlineData(1.0f)]
        [InlineData(1.2f)]
        public void ScatterSegment_CenterY_SetValue_ReturnsExpectedValue(float centerY)
        {
            ScatterSegment scatterSegment = new ScatterSegment();
            scatterSegment.CenterY = centerY;

            Assert.Equal(centerY, scatterSegment.CenterY);
        }

        [Theory]
        [InlineData(0.2f)]
        [InlineData(1.0f)]
        [InlineData(1.2f)]
        public void ScatterSegment_CenterX_SetValue_ReturnsExpectedValue(float centerX)
        {
            ScatterSegment scatterSegment = new ScatterSegment();
            scatterSegment.CenterX = centerX;

            Assert.Equal(centerX, scatterSegment.CenterX);
        }

        [Fact]
        public void RadialBarSegment_Constructor_InitializesDefaultsCorrectly()
        {
            RadialBarSegment radialBarSegment = new RadialBarSegment();

            Assert.Equal(0f, radialBarSegment.StartAngle);
            Assert.Equal(0f, radialBarSegment.EndAngle);
        }

        [Theory]
        [InlineData(-100f)]
        [InlineData(200f)]
        public void RadialBarSegment_StartAngle_SetValue_ReturnsExpectedValue(float startAngle)
        {
            RadialBarSegment radialBarSegment = new RadialBarSegment();
            radialBarSegment.StartAngle = startAngle;

            Assert.Equal(startAngle, radialBarSegment.StartAngle);
        }

        [Theory]
        [InlineData(-250f)]
        [InlineData(100f)]
        public void RadialBarSegment_EndAngle_SetValue_ReturnsExpectedValue(float endAngle)
        {
            RadialBarSegment radialBarSegment = new RadialBarSegment();
            radialBarSegment.EndAngle = endAngle;

            Assert.Equal(endAngle, radialBarSegment.EndAngle);
        }

        [Fact]
        public void PieSegment_Constructor_InitializesDefaultsCorrectly()
        {
            PieSegment pieSegment = new PieSegment();

            Assert.Equal(0d, pieSegment.StartAngle);
            Assert.Equal(0d, pieSegment.EndAngle);
        }

        [Theory]
        [InlineData(-100d)]
        [InlineData(200d)]
        public void PieSegment_StartAngle_SetValue_ReturnsExpectedValue(double startAngle)
        {
            PieSegment pieSegment = new PieSegment();
            pieSegment.StartAngle = startAngle;

            Assert.Equal(startAngle, pieSegment.StartAngle);
        }

        [Theory]
        [InlineData(-250d)]
        [InlineData(100d)]
        public void PieSegment_EndAngle_SetValue_ReturnsExpectedValue(double endAngle)
        {
            PieSegment pieSegment = new PieSegment();
            pieSegment.EndAngle = endAngle;

            Assert.Equal(endAngle, pieSegment.EndAngle);
        }

        [Fact]
        public void LineSegment_Constructor_InitializesDefaultsCorrectly()
        {
            LineSegment lineSegment = new LineSegment();

            Assert.NotNull(lineSegment);
            Assert.Equal(0f, lineSegment.X1);
            Assert.Equal(0f, lineSegment.X2);
            Assert.Equal(0f, lineSegment.Y1);
            Assert.Equal(0f, lineSegment.Y2);
        }

        [Theory]
        [InlineData(3f)]
        [InlineData(0.2f)]
        [InlineData(10f)]
        public void LineSegment_X1_SetValue_ReturnsExpectedValue(float x1)
        {
            var segment = new LineSegment();
            segment.X1 = x1;

            Assert.Equal(x1, segment.X1);
        }

        [Theory]
        [InlineData(3f)]
        [InlineData(0.2f)]
        [InlineData(-2.9f)]
        public void LineSegment_Y1_SetValue_ReturnsExpectedValue(float y1)
        {
            LineSegment lineSegment = new LineSegment();
            lineSegment.Y1 = y1;

            Assert.Equal(y1, lineSegment.Y1);
        }

        [Theory]
        [InlineData(1.3f)]
        [InlineData(0.2f)]
        [InlineData(-2f)]
        public void LineSegment_X2_SetValue_ReturnsExpectedValue(float x2)
        {
            LineSegment lineSegment = new LineSegment();
            lineSegment.X2 = x2;

            Assert.Equal(x2, lineSegment.X2);
        }

        [Theory]
        [InlineData(3f)]
        [InlineData(-1f)]
        [InlineData(10f)]
        public void LineSegment_Y2_SetValue_ReturnsExpectedValue(float y2)
        {
            LineSegment lineSegment = new LineSegment();
            lineSegment.Y2 = y2;

            Assert.Equal(y2, lineSegment.Y2);
        }

        [Fact]
        public void ColumnSegment_Constructor_InitializesDefaultsCorrectly()
        {
            ColumnSegment columnSegment = new ColumnSegment();

            Assert.NotNull(columnSegment);
            Assert.Equal(0f, columnSegment.Left);
            Assert.Equal(0f, columnSegment.Right);
            Assert.Equal(0f, columnSegment.Top);
            Assert.Equal(0f, columnSegment.Bottom);
        }

        [Theory]
        [InlineData(1f)]
        [InlineData(0f)]
        [InlineData(-0.9f)]
        public void ColumnSegment_Left_SetValue_ReturnsExpectedValue(float left)
        {
            ColumnSegment columnSegment = new ColumnSegment();
            columnSegment.Left = left;

            Assert.Equal(left, columnSegment.Left);
        }

        [Theory]
        [InlineData(3f)]
        [InlineData(0.2f)]
        [InlineData(10f)]
        public void ColumnSegment_Right_SetValue_ReturnsExpectedValue(float right)
        {
            ColumnSegment columnSegment = new ColumnSegment();
            columnSegment.Right = right;

            Assert.Equal(right, columnSegment.Right);
        }

        [Theory]
        [InlineData(0.9f)]
        [InlineData(-0.2f)]
        [InlineData(5f)]
        public void ColumnSegment_Bottom_SetValue_ReturnsExpectedValue(float bottom)
        {
            ColumnSegment columnSegment = new ColumnSegment();
            columnSegment.Bottom = bottom;

            Assert.Equal(bottom, columnSegment.Bottom);
        }

        [Theory]
        [InlineData(1.3f)]
        [InlineData(2f)]
        [InlineData(12f)]
        public void ColumnSegment_Top_SetValue_ReturnsExpectedValue(float top)
        {
            ColumnSegment columnSegment = new ColumnSegment();
            columnSegment.Top = top;

            Assert.Equal(top, columnSegment.Top);
        }

        [Fact]
        public void BubbleSegment_Constructor_InitializesDefaultsCorrectly()
        {
            BubbleSegment bubbleSegment = new BubbleSegment();

            Assert.Equal(0f, bubbleSegment.Radius);
            Assert.Equal(0f, bubbleSegment.CenterX);
            Assert.Equal(0f, bubbleSegment.CenterY);
        }

        [Theory]
        [InlineData(0.2f)]
        [InlineData(1.0f)]
        [InlineData(1.2f)]
        public void Radius_SetValue_ReturnsExpectedValue(float radius)
        {
            BubbleSegment bubbleSegment = new BubbleSegment();
            bubbleSegment.Radius = radius;

            Assert.Equal(radius, bubbleSegment.Radius);
        }

        [Theory]
        [InlineData(0.2f)]
        [InlineData(1.0f)]
        [InlineData(1.2f)]
        public void BubbleSegment_CenterY_SetValue_ReturnsExpectedValue(float centerY)
        {
            BubbleSegment bubbleSegment = new BubbleSegment();
            bubbleSegment.CenterY = centerY;

            Assert.Equal(centerY, bubbleSegment.CenterY);
        }

        [Theory]
        [InlineData(0.2f)]
        [InlineData(1.0f)]
        [InlineData(1.2f)]
        public void BubbleSegment_CenterX_SetValue_ReturnsExpectedValue(float centerX)
        {
            BubbleSegment bubbleSegment = new BubbleSegment();
            bubbleSegment.CenterX = centerX;

            Assert.Equal(centerX, bubbleSegment.CenterX);
        }

        [Fact]
        public void Fill_SetValue_ReturnsExpectedValue()
        {
            AreaSegment areaSegment = new AreaSegment();
            var fill = new SolidColorBrush(Color.FromRgba(255, 0, 0, 0));
            areaSegment.Fill = fill;

            Assert.Equal(fill, areaSegment.Fill);
        }

        [Fact]
        public void Stroke_SetValue_ReturnsExpectedValue()
        {
            AreaSegment areaSegment = new AreaSegment();
            var stroke = new SolidColorBrush(Color.FromRgba(255, 255, 255, 255));
            areaSegment.Stroke = stroke;

            Assert.Equal(stroke, areaSegment.Stroke);
        }

        [Fact]
        public void StrokeWidth_SetValue_ReturnsExpectedValue()
        {
            AreaSegment areaSegment = new AreaSegment();
            double strokeWidth = 2.5;
            areaSegment.StrokeWidth = strokeWidth;

            Assert.Equal(strokeWidth, areaSegment.StrokeWidth);
        }

        [Fact]
        public void StrokeDashArray_SetValue_ReturnsExpectedValue()
        {
            AreaSegment areaSegment = new AreaSegment();
            var expectedDashArray = new DoubleCollection { 2.0, 2.0 };
            areaSegment.StrokeDashArray = expectedDashArray;

            Assert.Equal(expectedDashArray, areaSegment.StrokeDashArray);
        }

        [Theory]
        [InlineData(0.1f)]
        [InlineData(1f)]
        [InlineData(0.8f)]
        public void Opacity_SetValue_ReturnsExpectedValue(float opacityfloat)
        {
            AreaSegment areaSegment = new AreaSegment();
            areaSegment.Opacity = opacityfloat;

            Assert.Equal(opacityfloat, areaSegment.Opacity);
        }

        [Fact]
        public void XValues_SetValue_ReturnsExpectedValue()
        {
            AreaSegment areaSegment = new AreaSegment();
            double[] expectedXValues = { 1.0, 2.0, 3.0 };
            areaSegment.XValues = expectedXValues;

            Assert.Equal(expectedXValues, areaSegment.XValues);
        }

        [Fact]
        public void YValues_SetValue_ReturnsExpectedValue()
        {
            AreaSegment areaSegment = new AreaSegment();
            double[] expectedYValues = { 1.0, 2.0, 3.0 };
            areaSegment.YValues = expectedYValues;

            Assert.Equal(expectedYValues, areaSegment.YValues);
        }

        [Fact]
        public void Item_SetValue_ReturnsExpectedValue()
        {
            AreaSegment areaSegment = new AreaSegment();
            var expectedItem = new object();
            areaSegment.Item = expectedItem;

            Assert.Same(expectedItem, areaSegment.Item);
        }

        [Fact]
        public void Series_SetValue_ReturnsExpectedValue()
        {
            AreaSegment areaSegment = new AreaSegment();
            var expectedSeries = new AreaSeries();
            areaSegment.Series = expectedSeries;

            Assert.Equal(expectedSeries, areaSegment.Series);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(-2)]
        [InlineData(3)]
        public void Index_SetValue_ReturnsExpectedValue(int index)
        {
            AreaSegment areaSegment = new AreaSegment();
            areaSegment.Index = index;

            Assert.Equal(index, areaSegment.Index);
        }

        [Fact]
        public void AnimatedValue_SetValue_ReturnsExpectedValue()
        {
            AreaSegment areaSegment = new AreaSegment();
            var series = new AreaSeries { AnimationValue = 5.5f };
            areaSegment.Series = series;

            Assert.Equal(5.5f, series.AnimationValue);
        }

        [Fact]
        public void BoxAndWhiskerSegment_Constructor_InitializesDefaultsCorrectly()
        {
            BoxAndWhiskerSegment boxAndWhiskerSegment = new BoxAndWhiskerSegment();

            Assert.Equal(0d, boxAndWhiskerSegment.Maximum);
            Assert.Equal(0d, boxAndWhiskerSegment.Minimum);
            Assert.Equal(0d, boxAndWhiskerSegment.Median);
            Assert.Equal(0d, boxAndWhiskerSegment.LowerQuartile);
            Assert.Equal(0d, boxAndWhiskerSegment.UpperQuartile);
            Assert.Equal(0f, boxAndWhiskerSegment.Left);
            Assert.Equal(0f, boxAndWhiskerSegment.Right);
            Assert.Equal(0f, boxAndWhiskerSegment.Top);
            Assert.Equal(0f, boxAndWhiskerSegment.Bottom);
            Assert.Equal(0f, boxAndWhiskerSegment.Center);
        }

        [Theory]
        [InlineData(20d)]
        [InlineData(5d)]
        [InlineData(0d)]
        public void Maximum_SetValue_ReturnsExpectedValue(double maximum)
        {
            BoxAndWhiskerSegment boxAndWhiskerSegment = new BoxAndWhiskerSegment();
            boxAndWhiskerSegment.Maximum = maximum;

            Assert.Equal(maximum, boxAndWhiskerSegment.Maximum);
        }

        [Theory]
        [InlineData(20d)]
        [InlineData(5d)]
        [InlineData(0d)]
        public void Minimum_SetValue_ReturnsExpectedValue( double minimum)
        {
            BoxAndWhiskerSegment boxAndWhiskerSegment = new BoxAndWhiskerSegment();
            boxAndWhiskerSegment.Minimum = minimum;

            Assert.Equal(minimum, boxAndWhiskerSegment.Minimum);
        }

        [Theory]
        [InlineData(20d)]
        [InlineData(5d)]
        [InlineData(0d)]
        public void Median__SetValue_ReturnsExpectedValue(double median)
        {
            BoxAndWhiskerSegment boxAndWhiskerSegment = new BoxAndWhiskerSegment();
            boxAndWhiskerSegment.Median = median;

            Assert.Equal(median, boxAndWhiskerSegment.Median);
        }

        [Theory]
        [InlineData(20d)]
        [InlineData(5d)]
        [InlineData(0d)]
        public void LowerQuartile__SetValue_ReturnsExpectedValue(double lowerQuartile)
        {
            BoxAndWhiskerSegment boxAndWhiskerSegment = new BoxAndWhiskerSegment();
            boxAndWhiskerSegment.LowerQuartile = lowerQuartile;

            Assert.Equal(lowerQuartile, boxAndWhiskerSegment.LowerQuartile);
        }

        [Theory]
        [InlineData(20d)]
        [InlineData(5d)]
        [InlineData(0d)]
        public void UpperQuartile__SetValue_ReturnsExpectedValue(double upperQuartile)
        {
            BoxAndWhiskerSegment boxAndWhiskerSegment = new BoxAndWhiskerSegment();
            boxAndWhiskerSegment.UpperQuartile = upperQuartile;

            Assert.Equal(upperQuartile, boxAndWhiskerSegment.UpperQuartile);
        }

        [Theory]
        [InlineData(1f)]
        [InlineData(0f)]
        [InlineData(-0.9f)]
        public void BoxAndWhiskerSegment_Left_SetValue_ReturnsExpectedValue(float left)
        {
            BoxAndWhiskerSegment boxAndWhiskerSegment = new BoxAndWhiskerSegment();
            boxAndWhiskerSegment.Left = left;

            Assert.Equal(left, boxAndWhiskerSegment.Left);
        }

        [Theory]
        [InlineData(3f)]
        [InlineData(0.2f)]
        [InlineData(10f)]
        public void BoxAndWhiskerSegment_SetValue_ReturnsExpectedValue(float right)
        {
            BoxAndWhiskerSegment boxAndWhiskerSegment = new BoxAndWhiskerSegment();
            boxAndWhiskerSegment.Right = right;

            Assert.Equal(right, boxAndWhiskerSegment.Right);
        }

        [Theory]
        [InlineData(0.9f)]
        [InlineData(-0.2f)]
        [InlineData(5f)]
        public void BoxAndWhiskerSegment_Bottom_SetValue_ReturnsExpectedValue(float bottom)
        {
            BoxAndWhiskerSegment boxAndWhiskerSegment = new BoxAndWhiskerSegment();
            boxAndWhiskerSegment.Bottom = bottom;

            Assert.Equal(bottom, boxAndWhiskerSegment.Bottom);
        }

        [Theory]
        [InlineData(1.3f)]
        [InlineData(2f)]
        [InlineData(12f)]
        public void BoxAndWhiskerSegment_Top_SetValue_ReturnsExpectedValue(float top)
        {
            BoxAndWhiskerSegment boxAndWhiskerSegment = new BoxAndWhiskerSegment();
            boxAndWhiskerSegment.Top = top;

            Assert.Equal(top, boxAndWhiskerSegment.Top);
        }

        [Theory]
        [InlineData(1.3f)]
        [InlineData(2f)]
        [InlineData(12f)]
        public void BoxAndWhiskerSegment_Center_SetValue_ReturnsExpectedValue(float center)
        {
            BoxAndWhiskerSegment boxAndWhiskerSegment = new BoxAndWhiskerSegment();
            boxAndWhiskerSegment.Center = center;

            Assert.Equal(center, boxAndWhiskerSegment.Center);
        }
    }
}
