using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.Toolkit.UnitTest.Charts
{
	public class DefaultAnnotationUnitTests
	{
		[Fact]
		public void EllipseAnnotation_InitializesBasicProperties()
		{
			var ellipseAnnotation = new EllipseAnnotation(); 
			Assert.Equal(10.0, ellipseAnnotation.Width);
			Assert.Equal(10.0, ellipseAnnotation.Height);
			Assert.Equal(ChartAlignment.Center, ellipseAnnotation.HorizontalAlignment);
			Assert.Equal(ChartAlignment.Center, ellipseAnnotation.VerticalAlignment);
			Assert.Null(ellipseAnnotation.X1);
			Assert.True(double.IsNaN(ellipseAnnotation.Y1));
			Assert.Null(ellipseAnnotation.X2);
			Assert.True(double.IsNaN(ellipseAnnotation.Y2));
		}

		[Fact]
		public void EllipseAnnotation_InitializesStyleProperties()
		{
			var ellipseAnnotation = new EllipseAnnotation(); 
			Assert.Equal(string.Empty, ellipseAnnotation.Text);
			Assert.NotNull(ellipseAnnotation.Fill);
			Assert.Equal(Color.FromArgb("#146750A4"), ellipseAnnotation.Fill.ToColor()); 
			Assert.NotNull(ellipseAnnotation.Stroke);
			Assert.Equal(Color.FromArgb("#6750A4"), ellipseAnnotation.Stroke.ToColor());
			Assert.Equal(1.0, ellipseAnnotation.StrokeWidth);
			Assert.Null(ellipseAnnotation.StrokeDashArray);
			Assert.True(ellipseAnnotation.IsVisible);
		}

		[Fact]
		public void EllipseAnnotation_InitializesChartProperties()
		{
			var ellipseAnnotation = new EllipseAnnotation(); 
			Assert.Equal(string.Empty, ellipseAnnotation.XAxisName);
			Assert.Equal(string.Empty, ellipseAnnotation.YAxisName);
			Assert.Null(ellipseAnnotation.Chart); 
			Assert.Equal(default(Rect), ellipseAnnotation.RenderRect);
			Assert.Equal(default(Rect), ellipseAnnotation.LabelRect);
			Assert.Equal(ChartCoordinateUnit.Axis, ellipseAnnotation.CoordinateUnit);
		}

		[Fact]
		public void EllipseAnnotation_InitializesLabelStyleProperties()
		{
			var ellipseAnnotation = new EllipseAnnotation();
			ChartAnnotationLabelStyle annotationLabelStyle = new() { IsShapeAnnotation = ellipseAnnotation is ShapeAnnotation };
			var labelStyle = ellipseAnnotation.LabelStyle = annotationLabelStyle;
			Assert.NotNull(labelStyle);
			Assert.IsType<ChartAnnotationLabelStyle>(labelStyle);
			Assert.Equal(11, labelStyle.FontSize);
			Assert.Equal(FontAttributes.None, labelStyle.FontAttributes);
			Assert.Null(labelStyle.FontFamily);
			Assert.Equal(Color.FromArgb("#49454F"), labelStyle.TextColor);
			Assert.Equal(Colors.Transparent, labelStyle.Background.ToColor());
			Assert.Equal(new Thickness(3.5), labelStyle.Margin);
			Assert.Equal(ChartLabelAlignment.Center, labelStyle.HorizontalTextAlignment);
			Assert.Equal(ChartLabelAlignment.Center, labelStyle.VerticalTextAlignment);
		} 

		[Fact]
		public void RectangleAnnotation_InitializesStyleProperties()
		{
			var rectangleAnnotation = new RectangleAnnotation(); 
			Assert.Null(rectangleAnnotation.X1);
			Assert.True(double.IsNaN(rectangleAnnotation.Y1));
			Assert.Equal(string.Empty, rectangleAnnotation.Text);
			Assert.NotNull(rectangleAnnotation.Fill); 
			Assert.Equal(Color.FromArgb("#146750A4"), rectangleAnnotation.Fill.ToColor());
			Assert.NotNull(rectangleAnnotation.Stroke);
			Assert.Equal(Color.FromArgb("#6750A4"), rectangleAnnotation.Stroke.ToColor()); 
			Assert.Equal(1.0, rectangleAnnotation.StrokeWidth);
			Assert.Null(rectangleAnnotation.StrokeDashArray);
			Assert.True(rectangleAnnotation.IsVisible);
		}

		[Fact]
		public void RectangleAnnotation_InitializesChartProperties()
		{
			var rectangleAnnotation = new RectangleAnnotation(); 
			Assert.Null(rectangleAnnotation.X2);
			Assert.True(double.IsNaN(rectangleAnnotation.Y2));
			Assert.Equal(string.Empty, rectangleAnnotation.XAxisName);
			Assert.Equal(string.Empty, rectangleAnnotation.YAxisName);
			Assert.Null(rectangleAnnotation.Chart);
			Assert.Equal(default(Rect), rectangleAnnotation.RenderRect);
			Assert.Equal(default(Rect), rectangleAnnotation.LabelRect);
			Assert.Equal(ChartCoordinateUnit.Axis, rectangleAnnotation.CoordinateUnit);
		}

		[Fact]
		public void RectangleAnnotation_InitializesLabelStyleProperties()
		{
			var rectangleAnnotation = new RectangleAnnotation();
			ChartAnnotationLabelStyle annotationLabelStyle = new() { IsShapeAnnotation = rectangleAnnotation is ShapeAnnotation };
			var labelStyle = rectangleAnnotation.LabelStyle = annotationLabelStyle; 
			Assert.IsType<ChartAnnotationLabelStyle>(labelStyle);
			Assert.Equal(11, labelStyle.FontSize);
			Assert.Equal(FontAttributes.None, labelStyle.FontAttributes);
			Assert.Null(labelStyle.FontFamily);
			Assert.Equal(Color.FromArgb("#49454F"), labelStyle.TextColor);
			Assert.Equal(Colors.Transparent, labelStyle.Background.ToColor());
			Assert.Equal(new Thickness(3.5), labelStyle.Margin);
			Assert.Equal(ChartLabelAlignment.Center, labelStyle.HorizontalTextAlignment);
			Assert.Equal(ChartLabelAlignment.Center, labelStyle.VerticalTextAlignment);
		}

		[Fact]
		public void HorizontalLineAnnotation_InitializesBasicProperties()
		{
			var horizontalLineAnnotation = new HorizontalLineAnnotation(); 
			Assert.False(horizontalLineAnnotation.ShowAxisLabel);
			Assert.Equal(ChartLineCap.None, horizontalLineAnnotation.LineCap);
			Assert.False(float.IsNaN(horizontalLineAnnotation.XPosition1));
			Assert.False(float.IsNaN(horizontalLineAnnotation.XPosition2));
			Assert.False(float.IsNaN(horizontalLineAnnotation.YPosition1));
			Assert.False(float.IsNaN(horizontalLineAnnotation.YPosition2));
			Assert.Equal(0f, horizontalLineAnnotation.Angle);
			Assert.NotNull(horizontalLineAnnotation.LineCapPoints);
		}

		[Fact]
		public void HorizontalLineAnnotation_InitializesStyleProperties()
		{
			var horizontalLineAnnotation = new HorizontalLineAnnotation(); 
			Assert.True(double.IsNaN(horizontalLineAnnotation.Y1));
			Assert.Null(horizontalLineAnnotation.X1);
			Assert.Null(horizontalLineAnnotation.X2);
			Assert.Equal(string.Empty, horizontalLineAnnotation.Text);
			Assert.NotNull(horizontalLineAnnotation.Stroke);
			Assert.Equal(1.0, horizontalLineAnnotation.StrokeWidth);
			Assert.Null(horizontalLineAnnotation.StrokeDashArray);
			Assert.True(horizontalLineAnnotation.IsVisible);
		}

		[Fact]
		public void HorizontalLineAnnotation_InitializesChartProperties()
		{
			var horizontalLineAnnotation = new HorizontalLineAnnotation(); 
			Assert.Empty(horizontalLineAnnotation.LineCapPoints);
			Assert.Equal(string.Empty, horizontalLineAnnotation.XAxisName);
			Assert.Equal(string.Empty, horizontalLineAnnotation.YAxisName);
			Assert.Null(horizontalLineAnnotation.Chart);
			Assert.Equal(default(Rect), horizontalLineAnnotation.RenderRect);
			Assert.Equal(default(Rect), horizontalLineAnnotation.LabelRect);
			Assert.Equal(ChartCoordinateUnit.Axis, horizontalLineAnnotation.CoordinateUnit);
		}

		[Fact]
		public void HorizontalLineAnnotation_InitializesLabelStyleProperties()
		{
			var horizontalLineAnnotation = new HorizontalLineAnnotation();
			var axisLabel = horizontalLineAnnotation._axisLabelStyle; 
			var annotationLabel = horizontalLineAnnotation._annotationLabelStyle;
			Assert.Equal(ChartLabelAlignment.End, annotationLabel.HorizontalTextAlignment);
			Assert.Equal(ChartLabelAlignment.Start, annotationLabel.VerticalTextAlignment); 
			Assert.NotNull(axisLabel);
			Assert.IsType<ChartLabelStyle>(axisLabel);
			Assert.Equal(14, axisLabel.FontSize);
			var labelStyle = horizontalLineAnnotation.LabelStyle = new ChartAnnotationLabelStyle { IsShapeAnnotation = horizontalLineAnnotation is ShapeAnnotation };
			Assert.NotNull(labelStyle);
			Assert.IsType<ChartAnnotationLabelStyle>(labelStyle);
			Assert.Equal(ChartLabelAlignment.Center, labelStyle.HorizontalTextAlignment);
			Assert.Equal(ChartLabelAlignment.Center, labelStyle.VerticalTextAlignment);
		}

		[Fact]
		public void HorizontalLineAnnotation_InitializesAxisLabelStyleProperties()
		{
			var horizontalLineAnnotation = new HorizontalLineAnnotation();
			var axisLabelStyle = horizontalLineAnnotation.AxisLabelStyle; 
			Assert.NotNull(axisLabelStyle);
			Assert.IsType<ChartLabelStyle>(axisLabelStyle);
			Assert.Equal(14, axisLabelStyle.FontSize);
			Assert.Equal(FontAttributes.None, axisLabelStyle.FontAttributes);
			Assert.Null(axisLabelStyle.FontFamily);
			Assert.Equal(Colors.Transparent, axisLabelStyle.Background.ToColor());
			Assert.Equal(new Thickness(3.5), axisLabelStyle.Margin);
		}


		[Fact]
		public void VerticalLineAnnotation_InitializesDefaultProperties()
		{
			var verticalLineAnnotation = new VerticalLineAnnotation(); 
			Assert.False(verticalLineAnnotation.ShowAxisLabel);
			Assert.Equal(ChartLineCap.None, verticalLineAnnotation.LineCap);
			Assert.Equal(0f, verticalLineAnnotation.Angle);
			Assert.Equal(string.Empty, verticalLineAnnotation.Text);
			Assert.Equal(1.0, verticalLineAnnotation.StrokeWidth);
			Assert.True(verticalLineAnnotation.IsVisible);
			Assert.Equal(string.Empty, verticalLineAnnotation.XAxisName);
			Assert.Equal(string.Empty, verticalLineAnnotation.YAxisName);
			Assert.Null(verticalLineAnnotation.Chart);
		}

		[Fact]
		public void VerticalLineAnnotation_InitializesPositionProperties()
		{
			var verticalLineAnnotation = new VerticalLineAnnotation(); 
			Assert.False(float.IsNaN(verticalLineAnnotation.XPosition1));
			Assert.False(float.IsNaN(verticalLineAnnotation.XPosition2));
			Assert.False(float.IsNaN(verticalLineAnnotation.YPosition1));
			Assert.False(float.IsNaN(verticalLineAnnotation.YPosition2));
			Assert.True(double.IsNaN(verticalLineAnnotation.Y1));
			Assert.Null(verticalLineAnnotation.X1);
			Assert.Null(verticalLineAnnotation.X2);
		}

		[Fact]
		public void VerticalLineAnnotation_InitializesLineCapPoints()
		{
			var verticalLineAnnotation = new VerticalLineAnnotation();
			var axisLabel = verticalLineAnnotation._axisLabelStyle; 
			var annotationLabel = verticalLineAnnotation._annotationLabelStyle;
			Assert.Equal(ChartLabelAlignment.Start, annotationLabel.HorizontalTextAlignment);
			Assert.Equal(ChartLabelAlignment.Start, annotationLabel.VerticalTextAlignment);
			Assert.NotNull(axisLabel);
			Assert.IsType<ChartLabelStyle>(axisLabel);
			Assert.Equal(14, axisLabel.FontSize);
			Assert.NotNull(verticalLineAnnotation.Stroke);
			Assert.Null(verticalLineAnnotation.StrokeDashArray);
			Assert.NotNull(verticalLineAnnotation.LineCapPoints);
			Assert.Empty(verticalLineAnnotation.LineCapPoints);
		} 

		[Fact]
		public void VerticalLineAnnotation_InitializesAxisLabelStyle()
		{
			var verticalLineAnnotation = new VerticalLineAnnotation(); 
			Assert.NotNull(verticalLineAnnotation.AxisLabelStyle);
			Assert.IsType<ChartLabelStyle>(verticalLineAnnotation.AxisLabelStyle);
			Assert.Equal(14, verticalLineAnnotation.AxisLabelStyle.FontSize);  
			var axisLabelStyle = verticalLineAnnotation.AxisLabelStyle;
			Assert.NotNull(axisLabelStyle);
			Assert.Equal(14, axisLabelStyle.FontSize);
			Assert.Equal(FontAttributes.None, axisLabelStyle.FontAttributes);
			Assert.Null(axisLabelStyle.FontFamily);
			Assert.Equal(Colors.Transparent, axisLabelStyle.Background.ToColor());
			Assert.Equal(new Thickness(3.5), axisLabelStyle.Margin);
		}

		[Fact]
		public void VerticalLineAnnotation_InitializesLabelStyle()
		{
			var verticalLineAnnotation = new VerticalLineAnnotation(); 
			Assert.Equal(default(Rect), verticalLineAnnotation.RenderRect);
			Assert.Equal(default(Rect), verticalLineAnnotation.LabelRect);
			Assert.Equal(ChartCoordinateUnit.Axis, verticalLineAnnotation.CoordinateUnit);
			var labelStyle = verticalLineAnnotation.LabelStyle = new ChartAnnotationLabelStyle { IsShapeAnnotation = verticalLineAnnotation is ShapeAnnotation };
			Assert.NotNull(labelStyle);
			Assert.IsType<ChartAnnotationLabelStyle>(labelStyle);
			Assert.Equal(ChartLabelAlignment.Center, labelStyle.HorizontalTextAlignment);
			Assert.Equal(ChartLabelAlignment.Center, labelStyle.VerticalTextAlignment);
		}

		[Fact]
		public void LineAnnotation_InitializesBasicProperties()
		{
			var lineAnnotation = new LineAnnotation(); 
			Assert.Equal(ChartLineCap.None, lineAnnotation.LineCap);
			Assert.Equal(0f, lineAnnotation.Angle);
			Assert.Equal(string.Empty, lineAnnotation.Text);
			Assert.True(lineAnnotation.IsVisible);
			Assert.Equal(string.Empty, lineAnnotation.XAxisName);
			Assert.Equal(string.Empty, lineAnnotation.YAxisName);
			Assert.Null(lineAnnotation.Chart);
			Assert.Equal(default(Rect), lineAnnotation.RenderRect);
		}

		[Fact]
		public void LineAnnotation_InitializesPositionProperties()
		{
			var lineAnnotation = new LineAnnotation(); 
			Assert.False(float.IsNaN(lineAnnotation.XPosition1));
			Assert.False(float.IsNaN(lineAnnotation.XPosition2));
			Assert.False(float.IsNaN(lineAnnotation.YPosition1));
			Assert.False(float.IsNaN(lineAnnotation.YPosition2));
			Assert.True(double.IsNaN(lineAnnotation.Y1));
			Assert.Null(lineAnnotation.X1);
			Assert.Null(lineAnnotation.X2); 
			Assert.NotNull(lineAnnotation.LineCapPoints);
		}
		 
		[Fact]
		public void LineAnnotation_InitializesStrokeProperties()
		{
			var lineAnnotation = new LineAnnotation(); 
			Assert.Equal(default(Rect), lineAnnotation.LabelRect);
			Assert.Equal(ChartCoordinateUnit.Axis, lineAnnotation.CoordinateUnit);
			Assert.Empty(lineAnnotation.LineCapPoints);
			Assert.NotNull(lineAnnotation.Stroke);
			Assert.Equal(1.0, lineAnnotation.StrokeWidth);
			Assert.Null(lineAnnotation.StrokeDashArray);
			var labelStyle = lineAnnotation.LabelStyle = new ChartAnnotationLabelStyle { IsShapeAnnotation = lineAnnotation is ShapeAnnotation }; 
			Assert.Equal(ChartLabelAlignment.Center, labelStyle.HorizontalTextAlignment);
			Assert.Equal(ChartLabelAlignment.Center, labelStyle.VerticalTextAlignment);
		}

		[Fact]
		public void LineAnnotation_InitializesAxisLabelStyle()
		{
			var lineAnnotation = new LineAnnotation(); 
			var axisLabel = lineAnnotation._axisLabelStyle;
			Assert.NotNull(axisLabel);
			Assert.IsType<ChartLabelStyle>(axisLabel);
			Assert.Equal(12, axisLabel.FontSize);
			Assert.Equal(FontAttributes.None, axisLabel.FontAttributes);
			Assert.Null(axisLabel.FontFamily);
			Assert.Equal(Colors.Transparent, axisLabel.Background.ToColor());
			Assert.Equal(new Thickness(3.5), axisLabel.Margin);
		}

		[Fact]
		public void LineAnnotation_InitializesLabelStyle()
		{
			var lineAnnotation = new LineAnnotation(); 
			var labelStyle = lineAnnotation.LabelStyle = new ChartAnnotationLabelStyle { IsShapeAnnotation = lineAnnotation is ShapeAnnotation };
			Assert.NotNull(labelStyle);
			Assert.IsType<ChartAnnotationLabelStyle>(labelStyle);
			Assert.Equal(11, labelStyle.FontSize);
			Assert.Equal(FontAttributes.None, labelStyle.FontAttributes);
			Assert.Null(labelStyle.FontFamily);
			Assert.Equal(Color.FromArgb("#49454F"), labelStyle.TextColor);
			Assert.Equal(Colors.Transparent, labelStyle.Background.ToColor());
			Assert.Equal(new Thickness(3.5), labelStyle.Margin);
		}

		[Fact]
		public void TextAnnotation_InitializesBasicProperties()
		{
			var textAnnotation = new TextAnnotation(); 
			Assert.False(float.IsNaN(textAnnotation.YPosition1));
			Assert.Null(textAnnotation.X1);
			Assert.True(double.IsNaN(textAnnotation.Y1));
			Assert.Equal(string.Empty, textAnnotation.Text);
			Assert.Equal(string.Empty, textAnnotation.XAxisName);
			Assert.Equal(string.Empty, textAnnotation.YAxisName);
			Assert.Null(textAnnotation.Chart);
			Assert.Equal(ChartCoordinateUnit.Axis, textAnnotation.CoordinateUnit);
			Assert.True(textAnnotation.IsVisible);
		}

		[Fact]
		public void TextAnnotation_InitializesLabelStyle()
		{
			var textAnnotation = new TextAnnotation();
			var labelStyle = textAnnotation.LabelStyle = new ChartAnnotationLabelStyle { IsShapeAnnotation = false };
			Assert.NotNull(labelStyle);
			Assert.IsType<ChartAnnotationLabelStyle>(labelStyle); 
			Assert.Equal(Rect.Zero, textAnnotation.LabelRect);
			Assert.False(float.IsNaN(textAnnotation.XPosition1));
			Assert.Equal(12, labelStyle.FontSize);
			Assert.Equal(FontAttributes.None, labelStyle.FontAttributes);
			Assert.Null(labelStyle.FontFamily);
			Assert.Equal(Colors.Transparent, labelStyle.Background.ToColor());
			Assert.Equal(new Thickness(3.5), labelStyle.Margin);
		} 
	}
}
