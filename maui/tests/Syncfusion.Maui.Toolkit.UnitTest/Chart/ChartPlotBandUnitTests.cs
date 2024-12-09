using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.Toolkit.UnitTest
{
	public class ChartPlotBandTests
	{
		[Theory]
		[InlineData(10)]
		[InlineData(20)]
		public void AssociatedAxisStart_SetValue_ReturnsExpectedValue(object expectedValue)
		{
			var numericalPlotBand = new NumericalPlotBand
			{
				AssociatedAxisStart = expectedValue
			};
			var actualValue = numericalPlotBand.AssociatedAxisStart;

			Assert.Equal(expectedValue, actualValue);
		}

		[Theory]
		[InlineData(20)]
		[InlineData(30)]
		public void AssociatedAxisEnd_SetValue_ReturnsExpectedValue(object expectedValue)
		{
			var numericalPlotBand = new NumericalPlotBand
			{
				AssociatedAxisEnd = expectedValue
			};
			var actualValue = numericalPlotBand.AssociatedAxisEnd;

			Assert.Equal(expectedValue, actualValue);
		}

		[Theory]
		[InlineData("YAxes")]
		[InlineData("XAxes")]
		public void AssociatedAxisName_SetValue_ReturnsExpectedValue(string expectedValue)
		{
			var numericalPlotBand = new NumericalPlotBand
			{
				AssociatedAxisName = expectedValue
			};
			var actualValue = numericalPlotBand.AssociatedAxisName;

			Assert.Equal(expectedValue, actualValue);
		}

		[Fact]
		public void Fill_SetValue_ReturnsExpectedValue()
		{
			var expectedValue = Colors.Red;
			var numericalPlotBand = new NumericalPlotBand
			{
				Fill = expectedValue
			};
			var actualValue = numericalPlotBand.Fill;

			Assert.Equal(expectedValue, actualValue);
		}

		[Fact]
		public void Stroke_SetValue_ReturnsExpectedValue()
		{
			var expectedValue = Colors.Green;
			var numericalPlotBand = new NumericalPlotBand
			{
				Stroke = expectedValue
			};
			var actualValue = numericalPlotBand.Stroke;

			Assert.Equal(expectedValue, actualValue);
		}

		[Theory]
		[InlineData(1.0)]
		[InlineData(2.5)]
		public void StrokeWidth_SetValue_ReturnsExpectedValue(double expectedValue)
		{
			var numericalPlotBand = new NumericalPlotBand
			{
				StrokeWidth = expectedValue
			};
			var actualValue = numericalPlotBand.StrokeWidth;

			Assert.Equal(expectedValue, actualValue);
		}

		[Theory]
		[InlineData(new double[] { 2, 2 })]
		[InlineData(new double[] { 4, 4 })]
		public void StrokeDashArray_SetValue_ReturnsExpectedValue(double[] dashArray)
		{
			var expectedValue = new DoubleCollection(dashArray);
			var numericalPlotBand = new NumericalPlotBand
			{
				StrokeDashArray = expectedValue
			};
			var actualValue = numericalPlotBand.StrokeDashArray;

			Assert.Equal(expectedValue, actualValue);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void IsVisible_SetValue_ReturnsExpectedValue(bool expectedValue)
		{
			var numericalPlotBand = new NumericalPlotBand
			{
				IsVisible = expectedValue
			};
			var actualValue = numericalPlotBand.IsVisible;

			Assert.Equal(expectedValue, actualValue);
		}

		[Theory]
		[InlineData("Sample Text 1")]
		[InlineData("Sample Text 2")]
		public void Text_SetValue_ReturnsExpectedValue(string expectedValue)
		{
			var numericalPlotBand = new NumericalPlotBand
			{
				Text = expectedValue
			};
			var actualValue = numericalPlotBand.Text;

			Assert.Equal(expectedValue, actualValue);
		}

		[Theory]
		[InlineData(12.0)]
		[InlineData(15.0)]
		public void Size_SetValue_ReturnsExpectedValue(double expectedValue)
		{
			var numericalPlotBand = new NumericalPlotBand
			{
				Size = expectedValue
			};
			var actualValue = numericalPlotBand.Size;

			Assert.Equal(expectedValue, actualValue);
		}

		[Theory]
		[InlineData(5.0)]
		[InlineData(10.0)]
		public void RepeatEvery_SetValue_ReturnsExpectedValue(double expectedValue)
		{
			var numericalPlotBand = new NumericalPlotBand
			{
				RepeatEvery = expectedValue
			};
			var actualValue = numericalPlotBand.RepeatEvery;

			// Assert
			Assert.Equal(expectedValue, actualValue);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void IsRepeatable_SetValue_ReturnsExpectedValue(bool expectedValue)
		{
			var numericalPlotBand = new NumericalPlotBand
			{
				IsRepeatable = expectedValue
			};
			var actualValue = numericalPlotBand.IsRepeatable;

			Assert.Equal(expectedValue, actualValue);
		}

		[Theory]
		[InlineData(ChartLabelAlignment.Center)]
		[InlineData(ChartLabelAlignment.Start)]
		[InlineData(ChartLabelAlignment.End)]
		public void VerticalTextAlignment_SetValue_ReturnsExpectedValue(ChartLabelAlignment alignment)
		{
			var cartPlotBandLabelStyle = new ChartPlotBandLabelStyle
			{
				VerticalTextAlignment = alignment
			};
			var actualAlignment = cartPlotBandLabelStyle.VerticalTextAlignment;

			Assert.Equal(alignment, actualAlignment);
		}

		[Theory]
		[InlineData(ChartLabelAlignment.Center)]
		[InlineData(ChartLabelAlignment.Start)]
		[InlineData(ChartLabelAlignment.End)]
		public void HorizontalTextAlignment_SetValue_ReturnsExpectedValue(ChartLabelAlignment alignment)
		{
			var cartPlotBandLabelStyle = new ChartPlotBandLabelStyle
			{
				HorizontalTextAlignment = alignment
			};
			var actualAlignment = cartPlotBandLabelStyle.HorizontalTextAlignment;

			Assert.Equal(alignment, actualAlignment);
		}

		[Theory]
		[InlineData(0)]
		[InlineData(45)]
		[InlineData(90)]
		public void Angle_SetValue_ReturnsExpectedValue(double angle)
		{
			var cartPlotBandLabelStyle = new ChartPlotBandLabelStyle
			{
				Angle = angle
			};
			var actualAngle = cartPlotBandLabelStyle.Angle;

			Assert.Equal(angle, actualAngle);
		}

		[Theory]
		[InlineData(0)]
		[InlineData(10)]
		[InlineData(-5)]
		public void OffsetX_SetValue_ReturnsExpectedValue(double offsetX)
		{
			var cartPlotBandLabelStyle = new ChartPlotBandLabelStyle
			{
				OffsetX = offsetX
			};
			var actualOffsetX = cartPlotBandLabelStyle.OffsetX;

			Assert.Equal(offsetX, actualOffsetX);
		}

		[Theory]
		[InlineData(0)]
		[InlineData(10)]
		[InlineData(-5)]
		public void OffsetY_SetValue_ReturnsExpectedValue(double offsetY)
		{
			var cartPlotBandLabelStyle = new ChartPlotBandLabelStyle
			{
				OffsetY = offsetY
			};
			var actualOffsetY = cartPlotBandLabelStyle.OffsetY;

			Assert.Equal(offsetY, actualOffsetY);
		}

		[Fact]
		public void PlotBandLabelStyle_SetValue_ReturnsExpectedValue()
		{
			var stroke = new SolidColorBrush(Colors.Red);
			var strokeWidth = 5.0;
			var fontSize = 14.0;
			var textColor = Colors.Orange;
			var horizontalTextAlignment = ChartLabelAlignment.Center;
			var verticalTextAlignment = ChartLabelAlignment.Start;
			var angle = 45.0;
			var offsetX = 10.0;
			var offsetY = -5.0;
			var cartPlotBandLabelStyle = new ChartPlotBandLabelStyle
			{
				Stroke = stroke,
				StrokeWidth = strokeWidth,
				FontSize = fontSize,
				TextColor = textColor,
				HorizontalTextAlignment = horizontalTextAlignment,
				VerticalTextAlignment = verticalTextAlignment,
				Angle = angle,
				OffsetX = offsetX,
				OffsetY = offsetY
			};
			var numericalPlotBand = new NumericalPlotBand
			{
				LabelStyle = cartPlotBandLabelStyle
			};
			var actualStyle = numericalPlotBand.LabelStyle;

			Assert.Equal(stroke, actualStyle.Stroke);
			Assert.Equal(strokeWidth, actualStyle.StrokeWidth);
			Assert.Equal(fontSize, actualStyle.FontSize);
			Assert.Equal(textColor, actualStyle.TextColor);
			Assert.Equal(horizontalTextAlignment, actualStyle.HorizontalTextAlignment);
			Assert.Equal(verticalTextAlignment, actualStyle.VerticalTextAlignment);
			Assert.Equal(angle, actualStyle.Angle);
			Assert.Equal(offsetX, actualStyle.OffsetX);
			Assert.Equal(offsetY, actualStyle.OffsetY);
		}

		[Theory]
		[InlineData("2023-01-01")]
		[InlineData("2024-12-31")]
		public void Start_SetValue_ReturnsExpectedValue(string dateString)
		{
			var expectedValue = DateTime.Parse(dateString);
			var dateTimePlotBand = new DateTimePlotBand
			{
				Start = expectedValue
			};
			var actualValue = dateTimePlotBand.Start;

			Assert.Equal(expectedValue, actualValue);
		}

		[Theory]
		[InlineData("2023-01-01")]
		[InlineData("2024-12-31")]
		public void End_SetValue_ReturnsExpectedValue(string dateString)
		{
			var expectedValue = DateTime.Parse(dateString);
			var dateTimePlotBand = new DateTimePlotBand
			{
				End = expectedValue
			};
			var actualValue = dateTimePlotBand.End;

			Assert.Equal(expectedValue, actualValue);
		}

		[Theory]
		[InlineData("2023-01-01")]
		[InlineData("2024-12-31")]
		public void RepeatUntil_SetValue_ReturnsExpectedValue(string dateString)
		{
			var expectedValue = DateTime.Parse(dateString);
			var dateTimePlotBand = new DateTimePlotBand
			{
				RepeatUntil = expectedValue
			};
			var actualValue = dateTimePlotBand.RepeatUntil;

			Assert.Equal(expectedValue, actualValue);
		}

		[Theory]
		[InlineData(DateTimeIntervalType.Auto)]
		[InlineData(DateTimeIntervalType.Milliseconds)]
		[InlineData(DateTimeIntervalType.Seconds)]
		[InlineData(DateTimeIntervalType.Minutes)]
		[InlineData(DateTimeIntervalType.Hours)]
		[InlineData(DateTimeIntervalType.Days)]
		[InlineData(DateTimeIntervalType.Months)]
		[InlineData(DateTimeIntervalType.Years)]
		public void SizeType_SetValue_ReturnsExpectedValue(DateTimeIntervalType intervalType)
		{
			var dateTimePlotBand = new DateTimePlotBand
			{
				SizeType = intervalType
			};
			var actualValue = dateTimePlotBand.SizeType;

			Assert.Equal(intervalType, actualValue);
		}

		[Theory]
		[InlineData(DateTimeIntervalType.Auto)]
		[InlineData(DateTimeIntervalType.Milliseconds)]
		[InlineData(DateTimeIntervalType.Seconds)]
		[InlineData(DateTimeIntervalType.Minutes)]
		[InlineData(DateTimeIntervalType.Hours)]
		[InlineData(DateTimeIntervalType.Days)]
		[InlineData(DateTimeIntervalType.Months)]
		[InlineData(DateTimeIntervalType.Years)]
		public void RepeatEveryType_SetValue_ReturnsExpectedValue(DateTimeIntervalType intervalType)
		{
			var dateTimePlotBand = new DateTimePlotBand
			{
				RepeatEveryType = intervalType
			};
			var actualValue = dateTimePlotBand.RepeatEveryType;

			Assert.Equal(intervalType, actualValue);
		}

		[Theory]
		[InlineData(0.0)]
		[InlineData(10.5)]
		[InlineData(-5.0)]
		public void NumericalPlotBandStart_SetValue_ReturnsExpectedValue(double expectedValue)
		{
			var numericalPlotBand = new NumericalPlotBand
			{
				Start = expectedValue
			};
			var actualValue = numericalPlotBand.Start;

			Assert.Equal(expectedValue, actualValue);
		}

		[Theory]
		[InlineData(0.0)]
		[InlineData(20.5)]
		[InlineData(-10.0)]
		public void NumericalPlotBandRepeatUntil_SetValue_ReturnsExpectedValue(double expectedValue)
		{
			var numericalPlotBand = new NumericalPlotBand
			{
				RepeatUntil = expectedValue
			};
			var actualValue = numericalPlotBand.RepeatUntil;

			Assert.Equal(expectedValue, actualValue);
		}

		[Theory]
		[InlineData(0.0)]
		[InlineData(30.5)]
		[InlineData(-15.0)]
		public void NumericalPlotBandEnd_SetValue_ReturnsExpectedValue(double expectedValue)
		{
			var numericalPlotBand = new NumericalPlotBand
			{
				End = expectedValue
			};
			var actualValue = numericalPlotBand.End;

			Assert.Equal(expectedValue, actualValue);
		}
	}
}