using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.Toolkit.UnitTest
{
    public class ChartAnnotationUnitTests
    {
        [Theory]
        [InlineData(ChartCoordinateUnit.Axis)]
        [InlineData(ChartCoordinateUnit.Pixel)]
        public void CoordinateUnit(ChartCoordinateUnit coordinateUnit)
        {
            var annotation = new TextAnnotation();
            annotation.CoordinateUnit = coordinateUnit;

            Assert.Equal(coordinateUnit, annotation.CoordinateUnit);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestIsVisibleProperty(bool isVisible)
        {
            var annotation = new TextAnnotation();
            annotation.IsVisible = isVisible;

            Assert.Equal(isVisible, annotation.IsVisible);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(20)]
        [InlineData(30)]
        public void TestX1Property(object x1)
        {
            var annotation = new TextAnnotation();
            annotation.X1 = x1;

            Assert.Equal(x1, annotation.X1);
        }

        [Theory]
        [InlineData("PrimaryXAxis")]
        [InlineData("SecondaryXAxis")]
        public void TestXAxisNameProperty(string xAxisName)
        {
            var annotation = new TextAnnotation();
            annotation.XAxisName = xAxisName;

            Assert.Equal(xAxisName, annotation.XAxisName);
        }

        [Theory]
        [InlineData(10.0)]
        [InlineData(20.0)]
        [InlineData(30.0)]
        public void TestY1Property(double y1)
        {
            var annotation = new TextAnnotation();
            annotation.Y1 = y1;

            Assert.Equal(y1, annotation.Y1);
        }

        [Theory]
        [InlineData("PrimaryYAxis")]
        [InlineData("SecondaryYAxis")]
        public void TestYAxisNameProperty(string yAxisName)
        {
            var annotation = new TextAnnotation();
            annotation.YAxisName = yAxisName;

            Assert.Equal(yAxisName, annotation.YAxisName);
        }

        // Text annotation

        [Fact]
        public void TestLabelStyleProperty()
        {
            var annotation = new TextAnnotation();
            var labelStyle = new ChartAnnotationLabelStyle
            {
                FontSize = 12,
                FontFamily = "Arial",
                TextColor = Colors.Red
            };

            annotation.LabelStyle = labelStyle;

            Assert.Equal(labelStyle, annotation.LabelStyle);
            Assert.Equal(12, annotation.LabelStyle.FontSize);
            Assert.Equal("Arial", annotation.LabelStyle.FontFamily);
            Assert.Equal(Colors.Red, annotation.LabelStyle.TextColor);
        }

        [Theory]
        [InlineData("Annotation Text 1")]
        [InlineData("Annotation Text 2")]
        public void TestTextProperty(string text)
        {
            var annotation = new TextAnnotation();
            annotation.Text = text;

            Assert.Equal(text, annotation.Text);
        }

        // View annotation

        [Theory]
        [InlineData(ChartAlignment.Start)]
        [InlineData(ChartAlignment.Center)]
        [InlineData(ChartAlignment.End)]
        public void TestHorizontalAlignmentProperty(ChartAlignment horizontalAlignment)
        {
            var annotation = new ViewAnnotation();
            annotation.HorizontalAlignment = horizontalAlignment;

            Assert.Equal(horizontalAlignment, annotation.HorizontalAlignment);
        }

        [Theory]
        [InlineData(ChartAlignment.Start)]
        [InlineData(ChartAlignment.Center)]
        [InlineData(ChartAlignment.End)]
        public void TestVerticalAlignmentProperty(ChartAlignment verticalAlignment)
        {
            var annotation = new ViewAnnotation();
            annotation.VerticalAlignment = verticalAlignment;

            Assert.Equal(verticalAlignment, annotation.VerticalAlignment);
        }

        [Fact]
        public void TestViewProperty()
        {
            var annotation = new ViewAnnotation();
            var view = new Label { Text = "Annotation View" };

            annotation.View = view;

            Assert.Equal(view, annotation.View);
            Assert.Equal("Annotation View", ((Label)annotation.View).Text);
        }

        // Shape annotation

        [Fact]
        public void TestFillProperty()
        {
            var annotation = new EllipseAnnotation();
            var fillBrush = new SolidColorBrush(Colors.Blue);

            annotation.Fill = fillBrush;

            Assert.Equal(fillBrush, annotation.Fill);
        }

        [Fact]
        public void TestLabelStyleProperty_ShapeAnnotation()
        {
            var annotation = new EllipseAnnotation();
            var labelStyle = new ChartAnnotationLabelStyle
            {
                FontSize = 12,
                FontFamily = "Arial",
                TextColor = Colors.Red
            };

            annotation.LabelStyle = labelStyle;

            Assert.Equal(labelStyle, annotation.LabelStyle);
            Assert.Equal(12, annotation.LabelStyle.FontSize);
            Assert.Equal("Arial", annotation.LabelStyle.FontFamily);
            Assert.Equal(Colors.Red, annotation.LabelStyle.TextColor);
        }

        [Fact]
        public void TestStrokeProperty()
        {
            var annotation = new LineAnnotation();
            var strokeBrush = new SolidColorBrush(Colors.Green);

            annotation.Stroke = strokeBrush;

            Assert.Equal(strokeBrush, annotation.Stroke);
        }

        [Fact]
        public void TestStrokeDashArrayProperty()
        {
            var annotation = new LineAnnotation();
            var dashArray = new DoubleCollection { 2, 3 };

            annotation.StrokeDashArray = dashArray;

            Assert.Equal(dashArray, annotation.StrokeDashArray);
        }

        [Theory]
        [InlineData(1.0)]
        [InlineData(2.5)]
        public void TestStrokeWidthProperty(double strokeWidth)
        {
            var annotation = new LineAnnotation();
            annotation.StrokeWidth = strokeWidth;

            Assert.Equal(strokeWidth, annotation.StrokeWidth);
        }

        [Theory]
        [InlineData("Annotation Text 1")]
        [InlineData("Annotation Text 2")]
        public void TestTextProperty_ShapeAnnotation(string text)
        {
            var annotation = new RectangleAnnotation();
            annotation.Text = text;

            Assert.Equal(text, annotation.Text);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(20)]
        public void TestX2Property(object x2)
        {
            var annotation = new LineAnnotation();
            annotation.X2 = x2;

            Assert.Equal(x2, annotation.X2);
        }

        [Theory]
        [InlineData(10.0)]
        [InlineData(20.0)]
        public void TestY2Property(double y2)
        {
            var annotation = new LineAnnotation();
            annotation.Y2 = y2;

            Assert.Equal(y2, annotation.Y2);
        }

        // Ellipse annotation

        [Theory]
        [InlineData(100.0)]
        [InlineData(200.0)]
        public void TestHeightProperty(double height)
        {
            var annotation = new EllipseAnnotation();
            annotation.Height = height;

            Assert.Equal(height, annotation.Height);
        }

        [Theory]
        [InlineData(ChartAlignment.Start)]
        [InlineData(ChartAlignment.Center)]
        [InlineData(ChartAlignment.End)]
        public void TestHorizontalAlignmentProperty_EllipseAnnotation(ChartAlignment horizontalAlignment)
        {
            var annotation = new EllipseAnnotation();
            annotation.HorizontalAlignment = horizontalAlignment;

            Assert.Equal(horizontalAlignment, annotation.HorizontalAlignment);
        }

        [Theory]
        [InlineData(ChartAlignment.Start)]
        [InlineData(ChartAlignment.Center)]
        [InlineData(ChartAlignment.End)]
        public void TestVerticalAlignmentProperty_EllipseAnnotation(ChartAlignment verticalAlignment)
        {
            var annotation = new EllipseAnnotation();
            annotation.VerticalAlignment = verticalAlignment;

            Assert.Equal(verticalAlignment, annotation.VerticalAlignment);
        }

        [Theory]
        [InlineData(100.0)]
        [InlineData(200.0)]
        public void TestWidthProperty(double width)
        {
            var annotation = new EllipseAnnotation();
            annotation.Width = width;

            Assert.Equal(width, annotation.Width);
        }

        // Line annotation

        [Theory]
        [InlineData(ChartLineCap.Arrow)]
        [InlineData(ChartLineCap.None)]
        public void TestLineCapProperty(ChartLineCap lineCap)
        {
            var annotation = new LineAnnotation();
            annotation.LineCap = lineCap;

            Assert.Equal(lineCap, annotation.LineCap);
        }

        //HorizontalLine annotation

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestShowAxisLabel_HorizontalLineAnnotation(bool value)
        {
            var annotation = new HorizontalLineAnnotation();
            annotation.ShowAxisLabel = value;

            Assert.Equal(value, annotation.ShowAxisLabel);
        }

        [Fact]
            
        public void TestAxisLabelStyle_HorizontalLineAnnotation()
        {
            var annotation = new HorizontalLineAnnotation();
            var charStyleLabel = new ChartLabelStyle
            { 
                FontSize = 12,
            };
            annotation.AxisLabelStyle = charStyleLabel;
            Assert.Equal(charStyleLabel, annotation.AxisLabelStyle);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestShowAxisLabel__VerticalLineAnnotation(bool value)
        {
            var annotation = new VerticalLineAnnotation();
            annotation.ShowAxisLabel = value;

            Assert.Equal(value, annotation.ShowAxisLabel);
        }

        [Fact]

        public void TestAxisLabelStyle_VerticalLineAnnotation()
        {
            var annotation = new VerticalLineAnnotation();
            var charStyleLabel = new ChartLabelStyle
            {
                FontSize = 12,
            };
            annotation.AxisLabelStyle = charStyleLabel;
            Assert.Equal(charStyleLabel, annotation.AxisLabelStyle);
        }
        
    }
}

