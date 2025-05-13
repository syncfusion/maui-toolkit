using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.Toolkit.UnitTest.Charts
{
	public class AnnotationUnitTest : BaseUnitTest
	{
		#region ChartAnnotation methods

		#region Internal methods

		[Fact]
		public void TransformCoordinates_Test()
		{
			var chart = new SfCartesianChart();

			var xAxis = new NumericalAxis
			{
				VisibleRange = new DoubleRange(0, 5),
				RenderedRect = new Rect(70, 234, 875, 66),
				IsVertical = false
			};

			var yAxis = new NumericalAxis
			{
				VisibleRange = new DoubleRange(10, 250),
				RenderedRect = new Rect(0, 0, 70, 234),
				IsVertical = true
			};

			var annotation = new EllipseAnnotation
			{
				CoordinateUnit = ChartCoordinateUnit.Axis
			};

			chart._chartArea.ActualSeriesClipRect = new Rect(70, 20, 875, 234);
			chart.SeriesBounds = new Rect(70, 10, 875, 234);

			double inputX = 10;
			double inputY = 100;

			var result = annotation.TransformCoordinates(chart, xAxis, yAxis, inputX, inputY);

			Assert.Equal((1820, 166.25), result);
		}

		[Fact]
		public void Test_UnHookStylePropertyChanged()
		{
			var style = new ChartLabelStyle();
			var annotation = new EllipseAnnotation();

			style.PropertyChanged += (s, e) => { };
			style.Parent = annotation;

			annotation.UnHookStylePropertyChanged(style);

			Assert.Null(style.Parent);
		}

		#endregion

		#region Private methods

		[Fact]
		public void Test_GetAxis()
		{
			var axes = new List<ChartAxis>
			{
				new NumericalAxis { Name = "X-Axis" },
				new NumericalAxis { Name = "Y-Axis" }
			};
			var annotation = new TextAnnotation();
			var parameter = new object[] { axes, "X-Axis" };

			var result = InvokePrivateMethod(annotation, "GetAxis", parameter) as ChartAxis;

			Assert.Equal("X-Axis", result?.Name);
		}

		#endregion

		#endregion

		#region EllipseAnnotation methods

		[Fact]
		public void ResetPosition_Test()
		{
			var annotation = new EllipseAnnotation
			{
				RenderRect = new Rect(10, 20, 100, 50),
				LabelRect = new Rect(5, 10, 50, 30)
			};

			annotation.ResetPosition();

			Assert.Equal(Rect.Zero, annotation.RenderRect);
			Assert.Equal(Rect.Zero, annotation.LabelRect);
		}

		[Theory]
		[InlineData(ChartAlignment.Start, ChartAlignment.Start, 100, 100, 50, 50, 50, 50)]
		[InlineData(ChartAlignment.Center, ChartAlignment.Center, 100, 100, 50, 50, 75, 75)]
		[InlineData(ChartAlignment.Start, ChartAlignment.Center, 100, 100, 50, 50, 75, 50)]
		[InlineData(ChartAlignment.Center, ChartAlignment.Start, 100, 100, 50, 50, 50, 75)]
		public void ApplyAlignment_Test(ChartAlignment verticalAlignment, ChartAlignment horizontalAlignment, double x, double y, double width, double height, double expectedX, double expectedY)
		{
			var annotation = new EllipseAnnotation
			{
				VerticalAlignment = verticalAlignment,
				HorizontalAlignment = horizontalAlignment
			};

			InvokePrivateMethod(annotation, "ApplyAlignment", [x, y, width, height]);

			var expectedRect = new Rect((float)expectedX, (float)expectedY, (float)width, (float)height);

			Assert.Equal(expectedRect, annotation.RenderRect);
		}

		#endregion

		#region HorizontalLineAnnotation method

		[Fact]
		public void Test_OnParentSet()
		{
			var annotation = new HorizontalLineAnnotation();
			annotation.Parent = new SfCartesianChart();

			annotation.AxisLabelStyle = new ChartLabelStyle();
			annotation.AxisLabelStyle.Parent = null;

			Assert.Null(annotation.AxisLabelStyle.Parent);

			InvokePrivateMethod(annotation, "OnParentSet");

			Assert.NotNull(annotation.AxisLabelStyle.Parent);
		}

		[Fact]
		public void HorizontalLineAnnotationResetPosition_Test()
		{
			var annotation = new HorizontalLineAnnotation
			{
				LabelRect = new Rect(5, 10, 50, 30),
				XPosition1 = 10f,
				XPosition2 = 15f,
				YPosition1 = 20f,
				YPosition2 = 25f,
				LineCapPoints = [new Point(1, 1)]
			};

			annotation.ResetPosition();

			Assert.Equal(Rect.Zero, annotation.LabelRect);
			Assert.Equal(float.NaN, annotation.XPosition1);
			Assert.Equal(float.NaN, annotation.XPosition2);
			Assert.Equal(float.NaN, annotation.YPosition1);
			Assert.Equal(float.NaN, annotation.YPosition2);
			Assert.Empty(annotation.LineCapPoints);
		}

		#endregion

		#region LineAnnotation method

		[Fact]
		public void CalculatePosition_Test()
		{
			var annotation = new LineAnnotation()
			{
				XPosition1 = 332f,
				XPosition2 = 945f,
				YPosition1 = 140f,
				YPosition2 = 100f,
				StrokeWidth = 1,
			};

			var xAxis = new NumericalAxis();

			annotation.LineCap = ChartLineCap.None;
			annotation.CalculatePosition(isVertical: false, xAxis: xAxis);

			Assert.Equal(new Rect(332, 100, 613, 1), annotation.RenderRect);

			annotation.LineCap = ChartLineCap.Arrow;

			annotation.CalculatePosition(isVertical: false, xAxis: xAxis);

			Assert.Equal(new Rect(332, 100, 604.01513671875, 1), annotation.RenderRect);

			annotation.LineCap = ChartLineCap.None;
			annotation.CalculatePosition(isVertical: true, xAxis: xAxis);

			Assert.Equal(new Rect(332, 140, 1, 40), annotation.RenderRect);

			annotation.LineCap = ChartLineCap.Arrow;
			annotation.CalculatePosition(isVertical: true, xAxis: xAxis);

			Assert.Equal(new Rect(332, 140, 1, 33.645320892333984), annotation.RenderRect);
		}

		[Theory]
		[InlineData(0, 0, 10, 10, 1)]
		[InlineData(10, 10, 20, 20, 2)]
		public void CalculateArrowPoints_ShouldReturnCorrectPoints(float x1, float y1, float x2, float y2, float strokeWidth)
		{
			var annotation = new LineAnnotation { StrokeWidth = strokeWidth };

			var result = InvokePrivateMethod(annotation, "CalculateArrowPoints", [x1, y1, x2, y2]) as List<Point>;

			Assert.Equal(3, result?.Count);
			Assert.Equal(new Point((int)x2, (int)y2), result![0]);
			Assert.NotEqual(result[0], result[1]);
			Assert.NotEqual(result[0], result[2]);
		}

		#endregion

		#region ViewAnnotation method

		[Theory]
		[InlineData(100, 100, 50, 50, ChartAlignment.Start, ChartAlignment.Start, 50, 50)]
		[InlineData(100, 100, 50, 50, ChartAlignment.Center, ChartAlignment.Center, 75, 75)]
		public void SetViewAlignment_Test(double x, double y, double width, double height, ChartAlignment verticalAlignment, ChartAlignment horizontalAlignment, double expectedX, double expectedY)
		{
			var annotation = new ViewAnnotation
			{
				VerticalAlignment = verticalAlignment,
				HorizontalAlignment = horizontalAlignment
			};

			object[] parameters = [x, y, width, height];

			InvokeRefPrivateMethod(annotation, "SetViewAlignment", ref parameters);

			Assert.Equal(expectedX, parameters[0]);
			Assert.Equal(expectedY, parameters[1]);
		}

		#endregion

		[Fact]
		public void GetDefaultFillColor_Test()
		{
			var annotation = new EllipseAnnotation();

			Brush fillColor = annotation.GetDefaultFillColor();

			var solidColorBrush = fillColor as SolidColorBrush;
			Assert.NotNull(solidColorBrush);
			Assert.Equal(Color.FromArgb("#146750A4"), solidColorBrush.Color);
		}

		[Fact]
		public void GetDefaultStrokeColor_Test()
		{
			var annotation = new EllipseAnnotation();
			var result = annotation.GetDefaultStrokeColor();

			var solidColorBrush = result as SolidColorBrush;
			Assert.NotNull(solidColorBrush);
			Assert.Equal(Color.FromArgb("#6750A4"), solidColorBrush.Color);
		}

	}
}
