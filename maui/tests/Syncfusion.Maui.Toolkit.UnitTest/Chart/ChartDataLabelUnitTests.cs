using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.Toolkit.UnitTest
{
	public class ChartDataLabelTests
	{

		[Fact]
		public void TextColorProperty_SetAndGet_ReturnsExpectedValue()
		{

			var label = new ChartLabelStyle();
			var expected = Colors.Green;
			label.TextColor = expected;

			Assert.Equal(expected, label.TextColor);
		}

		[Fact]
		public void Strokey_SetAndGet_ReturnsExpectedValue()
		{

			var label = new ChartLabelStyle();
			var expected = Colors.Orange;
			label.Stroke = expected;

			Assert.Equal(expected, label.Stroke);
		}

		[Fact]
		public void Backgroundy_SetAndGet_ReturnsExpectedValue()
		{

			var label = new ChartLabelStyle();
			var expected = Colors.Orange;
			label.Background = expected;

			Assert.Equal(expected, label.Background);
		}

		[Theory]
		[InlineData(1, 2, 3, 4)]
		[InlineData(5, 6, 7, 8)]
		[InlineData(9, 10, 11, 12)]
		public void Margin_SetAndGet_ReturnsExpectedValue(int left, int top, int right, int bottom)
		{
			var expectedMargin = new Thickness(left, top, right, bottom);
			var labelStyle = new ChartLabelStyle
			{
				Margin = expectedMargin
			};

			var actualMargin = labelStyle.Margin;

			Assert.Equal(expectedMargin, actualMargin);
		}

		[Theory]
		[InlineData(1.0)]
		[InlineData(2.5)]
		[InlineData(3.75)]
		public void StrokeWidthy_SetAndGet_ReturnsExpectedValue(double strokeWidth)
		{
			var labelStyle = new ChartLabelStyle
			{
				StrokeWidth = strokeWidth
			};

			var actualStrokeWidth = labelStyle.StrokeWidth;

			Assert.Equal(strokeWidth, actualStrokeWidth);
		}

		[Theory]
		[InlineData("{0:C}")]
		[InlineData("{0:N}")]
		[InlineData("{0:P}")]
		public void LabelFormat_SetAndGet_ReturnsExpectedValue(string format)
		{
			var labelStyle = new ChartLabelStyle
			{
				LabelFormat = format
			};

			var actualFormat = labelStyle.LabelFormat;

			Assert.Equal(format, actualFormat);
		}

		[Theory]
		[InlineData(12.0)]
		[InlineData(14.5)]
		[InlineData(16.75)]
		public void FontSize_SetAndGet_ReturnsExpectedValue(double fontSize)
		{
			var labelStyle = new ChartLabelStyle
			{
				FontSize = fontSize
			};

			var actualFontSize = labelStyle.FontSize;

			Assert.Equal(fontSize, actualFontSize);
		}

		[Theory]
		[InlineData("Arial")]
		[InlineData("Times New Roman")]
		[InlineData("Verdana")]
		public void FontFamily_SetAndGet_ReturnsExpectedValue(string fontFamily)
		{
			var labelStyle = new ChartLabelStyle
			{
				FontFamily = fontFamily
			};

			var actualFontFamily = labelStyle.FontFamily;

			Assert.Equal(fontFamily, actualFontFamily);
		}

		[Theory]
		[InlineData(FontAttributes.Bold)]
		[InlineData(FontAttributes.Italic)]
		[InlineData(FontAttributes.None)]
		public void FontAttributes_SetAndGet_ReturnsExpectedValue(FontAttributes fontAttributes)
		{
			var labelStyle = new ChartLabelStyle
			{
				FontAttributes = fontAttributes
			};

			var actualFontAttributes = labelStyle.FontAttributes;

			Assert.Equal(fontAttributes, actualFontAttributes);
		}

		[Theory]
		[InlineData(5, 5, 5, 5)]
		[InlineData(10, 10, 10, 10)]
		[InlineData(15, 15, 15, 15)]
		public void CornerRadius_SetAndGet_ReturnsExpectedValue(int topLeft, int topRight, int bottomLeft, int bottomRight)
		{
			var expectedCornerRadius = new CornerRadius(topLeft, topRight, bottomLeft, bottomRight);
			var labelStyle = new ChartLabelStyle
			{
				CornerRadius = expectedCornerRadius
			};

			var actualCornerRadius = labelStyle.CornerRadius;

			Assert.Equal(expectedCornerRadius, actualCornerRadius);
		}

		[Theory]
		[InlineData(0)]
		[InlineData(3)]
		[InlineData(5.5)]
		public void LabelPadding_SetAndGet_ReturnsExpectedValue(double padding)
		{
			var labelStyle = new ChartDataLabelStyle
			{
				LabelPadding = padding
			};

			var actualPadding = labelStyle.LabelPadding;

			Assert.Equal(padding, actualPadding);
		}

		[Theory]
		[InlineData(0)]
		[InlineData(10)]
		[InlineData(-5)]
		public void OffsetX_SetAndGet_ReturnsExpectedValue(double offsetX)
		{
			var labelStyle = new ChartDataLabelStyle
			{
				OffsetX = offsetX
			};

			var actualOffsetX = labelStyle.OffsetX;

			Assert.Equal(offsetX, actualOffsetX);
		}

		[Theory]
		[InlineData(0)]
		[InlineData(10)]
		[InlineData(-5)]
		public void OffsetY_SetAndGet_ReturnsExpectedValue(double offsetY)
		{
			var labelStyle = new ChartDataLabelStyle
			{
				OffsetY = offsetY
			};

			var actualOffsetY = labelStyle.OffsetY;

			Assert.Equal(offsetY, actualOffsetY);
		}

		[Theory]
		[InlineData(0)]
		[InlineData(45)]
		[InlineData(90)]
		public void Angle_SetAndGet_ReturnsExpectedValue(double angle)
		{
			var labelStyle = new ChartDataLabelStyle
			{
				Angle = angle
			};

			var actualAngle = labelStyle.Angle;

			Assert.Equal(angle, actualAngle);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void UseSeriesPalette_SetAndGet_ReturnsExpectedValue(bool visible)
		{
			var labelSettings = new CartesianDataLabelSettings
			{
				UseSeriesPalette = visible
			};

			Assert.Equal(visible, labelSettings.UseSeriesPalette);
		}

		[Theory]
		[InlineData(DataLabelAlignment.Top)]
		[InlineData(DataLabelAlignment.Middle)]
		[InlineData(DataLabelAlignment.Bottom)]
		public void BarAlignment_SetAndGet_ReturnsExpectedValue(DataLabelAlignment alignment)
		{
			var labelSettings = new CartesianDataLabelSettings
			{
				BarAlignment = alignment
			};

			Assert.Equal(alignment, labelSettings.BarAlignment);
		}

		[Theory]
		[InlineData(DataLabelPlacement.Auto)]
		[InlineData(DataLabelPlacement.Inner)]
		[InlineData(DataLabelPlacement.Center)]
		[InlineData(DataLabelPlacement.Outer)]
		public void LabelPlacement_SetAndGet_ReturnsExpectedValue(DataLabelPlacement placement)
		{
			var labelSettings = new CartesianDataLabelSettings
			{
				LabelPlacement = placement
			};

			Assert.Equal(placement, labelSettings.LabelPlacement);
		}

		[Fact]
		public void DataLabelSettings_SetAndGet_ReturnsExpectedValue()
		{
			var barAlignment = DataLabelAlignment.Middle;
			var useSeriesPalette = true;
			var labelStyle = new ChartDataLabelStyle
			{
				Stroke = Colors.Red,
				StrokeWidth = 5,
				TextColor = Colors.Orange,
				FontSize = 14,
				FontFamily = "Arial",
				FontAttributes = FontAttributes.Bold,
				LabelFormat = "{0:C}",
				LabelPadding = 5
			};
			var labelPlacement = DataLabelPlacement.Outer;

			var dataLabelSettings = new CartesianDataLabelSettings
			{
				BarAlignment = barAlignment,
				UseSeriesPalette = useSeriesPalette,
				LabelStyle = labelStyle,
				LabelPlacement = labelPlacement
			};

			var chart = new ColumnSeries
			{
				DataLabelSettings = dataLabelSettings
			};

			var actualSettings = chart.DataLabelSettings;

			Assert.Equal(barAlignment, actualSettings.BarAlignment);
			Assert.Equal(useSeriesPalette, actualSettings.UseSeriesPalette);
			Assert.Equal(labelStyle, actualSettings.LabelStyle);
			Assert.Equal(labelPlacement, actualSettings.LabelPlacement);
		}

		[Fact]
		public void CircularDataLabelSettings_SetAndGet_ReturnsExpectedValue()
		{
			var smartLabelAlignment = SmartLabelAlignment.Shift;
			var labelPosition = ChartDataLabelPosition.Outside;


			var dataLabelSettings = new CircularDataLabelSettings
			{
				SmartLabelAlignment = smartLabelAlignment,
				LabelPosition = labelPosition,

			};

			var chart = new PieSeries
			{
				DataLabelSettings = dataLabelSettings
			};

			var actualSettings = chart.DataLabelSettings;

			Assert.Equal(smartLabelAlignment, actualSettings.SmartLabelAlignment);
			Assert.Equal(labelPosition, actualSettings.LabelPosition);

		}

		[Fact]
		public void FunnelDataLabelSettings_SetAndGet_ReturnsExpectedValue()
		{
			var context = FunnelDataLabelContext.YValue;

			var dataLabelSettings = new FunnelDataLabelSettings
			{
				Context = context
			};

			var chart = new SfFunnelChart
			{
				DataLabelSettings = dataLabelSettings
			};

			var actualSettings = chart.DataLabelSettings;

			Assert.Equal(context, actualSettings.Context);
		}

		[Fact]
		public void PyramidDataLabelSettings_SetAndGet_ReturnsExpectedValue()
		{
			var context = PyramidDataLabelContext.XValue;

			var dataLabelSettings = new PyramidDataLabelSettings
			{
				Context = context
			};

			var chart = new SfPyramidChart
			{
				DataLabelSettings = dataLabelSettings
			};

			var actualSettings = chart.DataLabelSettings;

			Assert.Equal(context, actualSettings.Context);
		}

		[Fact]
		public void LabelStyle_SetAndGet_ReturnsExpectedValue()
		{
			var stroke = Colors.Red;
			var strokeWidth = 5;
			var textColor = Colors.Orange;
			var fontSize = 14;
			var fontFamily = "Arial";
			var fontAttributes = FontAttributes.Bold;
			var labelFormat = "{0:C}";
			var labelPadding = 5;
			var labelStyle = new ChartDataLabelStyle
			{
				Stroke = stroke,
				StrokeWidth = strokeWidth,
				TextColor = textColor,
				FontSize = fontSize,
				FontFamily = fontFamily,
				FontAttributes = fontAttributes,
				LabelFormat = labelFormat,
				LabelPadding = labelPadding

			};

			var labelSettings = new CartesianDataLabelSettings
			{
				LabelStyle = labelStyle
			};

			var actualStyle = labelSettings.LabelStyle;

			Assert.Equal(stroke, actualStyle.Stroke);
			Assert.Equal(strokeWidth, actualStyle.StrokeWidth);
			Assert.Equal(textColor, actualStyle.TextColor);
			Assert.Equal(fontSize, actualStyle.FontSize);
			Assert.Equal(fontFamily, actualStyle.FontFamily);
			Assert.Equal(fontAttributes, actualStyle.FontAttributes);
			Assert.Equal(labelFormat, actualStyle.LabelFormat);
			Assert.Equal(labelPadding, actualStyle.LabelPadding);
		}


		[Fact]
		public void ChartAnnotationLabelStyle_SetAndGet_ReturnsExpectedValue()
		{
			var verticalTextAlignment = ChartLabelAlignment.Center;
			var horizontalTextAlignment = ChartLabelAlignment.Start;
			var stroke = Colors.Red;
			var strokeWidth = 2.0;
			var fontSize = 14.0;
			var textColor = Colors.Blue;

			var labelStyle = new ChartAnnotationLabelStyle
			{
				VerticalTextAlignment = verticalTextAlignment,
				HorizontalTextAlignment = horizontalTextAlignment,
				Stroke = stroke,
				StrokeWidth = strokeWidth,
				FontSize = fontSize,
				TextColor = textColor
			};

			var chart = new HorizontalLineAnnotation
			{
				LabelStyle = labelStyle
			};

			var actualStyle = chart.LabelStyle;

			Assert.Equal(verticalTextAlignment, actualStyle.VerticalTextAlignment);
			Assert.Equal(horizontalTextAlignment, actualStyle.HorizontalTextAlignment);
			Assert.Equal(stroke, actualStyle.Stroke);
			Assert.Equal(strokeWidth, actualStyle.StrokeWidth);
			Assert.Equal(fontSize, actualStyle.FontSize);
			Assert.Equal(textColor, actualStyle.TextColor);
		}

		[Fact]
		public void ChartAxisLabelStyle_SetAndGet_ReturnsExpectedValue()
		{
			var labelAlignment = ChartAxisLabelAlignment.Center;
			var maxWidth = 100.0;
			var wrappedLabelAlignment = ChartAxisLabelAlignment.End;

			var labelStyle = new ChartAxisLabelStyle
			{
				LabelAlignment = labelAlignment,
				MaxWidth = maxWidth,
				WrappedLabelAlignment = wrappedLabelAlignment
			};

			var chart = new NumericalAxis
			{
				LabelStyle = labelStyle
			};

			var actualStyle = chart.LabelStyle;

			Assert.Equal(labelAlignment, actualStyle.LabelAlignment);
			Assert.Equal(maxWidth, actualStyle.MaxWidth);
			Assert.Equal(wrappedLabelAlignment, actualStyle.WrappedLabelAlignment);
		}

		[Theory]
		[InlineData(SmartLabelAlignment.Shift)]
		[InlineData(SmartLabelAlignment.None)]
		[InlineData(SmartLabelAlignment.Hide)]
		public void SmartLabelAlignment_SetAndGet_ReturnsExpectedValue(SmartLabelAlignment alignment)
		{
			var labelStyle = new CircularDataLabelSettings
			{
				SmartLabelAlignment = alignment
			};

			var actualAlignment = labelStyle.SmartLabelAlignment;

			Assert.Equal(alignment, actualAlignment);
		}

		[Theory]
		[InlineData(ChartDataLabelPosition.Inside)]
		[InlineData(ChartDataLabelPosition.Outside)]
		public void LabelPosition_SetAndGet_ReturnsExpectedValue(ChartDataLabelPosition position)
		{
			var labelStyle = new CircularDataLabelSettings
			{
				LabelPosition = position
			};

			var actualPosition = labelStyle.LabelPosition;

			Assert.Equal(position, actualPosition);
		}

		[Fact]
		public void ConnectorLineSettings_SetAndGet_ReturnsExpectedValue()
		{
			var lineSettings = new ConnectorLineStyle()
			{
				Stroke = Colors.Red,
				StrokeWidth = 5,
				StrokeDashArray = new double[] { 1, 2, 3, 4 },
				ConnectorType = ConnectorType.Curve,

			};

			var labelStyle = new CircularDataLabelSettings
			{
				ConnectorLineSettings = lineSettings
			};

			var actualline = labelStyle.ConnectorLineSettings;

			Assert.Equal(lineSettings, actualline);

		}

		[Theory]
		[InlineData(FunnelDataLabelContext.YValue)]
		[InlineData(FunnelDataLabelContext.XValue)]
		public void FunnelDataLabelContext_SetAndGet_ReturnsExpectedValue(FunnelDataLabelContext context)
		{
			var labelStyle = new FunnelDataLabelSettings
			{
				Context = context
			};

			var actualContext = labelStyle.Context;

			Assert.Equal(context, actualContext);
		}

		[Theory]
		[InlineData(PyramidDataLabelContext.YValue)]
		[InlineData(PyramidDataLabelContext.XValue)]
		public void PyramidDataLabelContext_SetAndGet_ReturnsExpectedValue(PyramidDataLabelContext context)
		{
			var labelStyle = new PyramidDataLabelSettings
			{
				Context = context
			};

			var actualContext = labelStyle.Context;

			Assert.Equal(context, actualContext);
		}

		[Theory]
		[InlineData(ChartAxisLabelAlignment.Start)]
		[InlineData(ChartAxisLabelAlignment.Center)]
		[InlineData(ChartAxisLabelAlignment.End)]
		public void WrappedLabelAlignment_SetAndGet_ReturnsExpectedValue(ChartAxisLabelAlignment alignment)
		{
			var labelStyle = new ChartAxisLabelStyle
			{
				WrappedLabelAlignment = alignment
			};

			var actualAlignment = labelStyle.WrappedLabelAlignment;

			Assert.Equal(alignment, actualAlignment);
		}

		[Theory]
		[InlineData(0)]
		[InlineData(100)]
		[InlineData(200)]
		public void MaxWidth_SetAndGet_ReturnsExpectedValue(double maxWidth)
		{
			var labelStyle = new ChartAxisLabelStyle
			{
				MaxWidth = maxWidth
			};

			var actualMaxWidth = labelStyle.MaxWidth;

			Assert.Equal(maxWidth, actualMaxWidth);
		}

		[Theory]
		[InlineData(ChartAxisLabelAlignment.Start)]
		[InlineData(ChartAxisLabelAlignment.Center)]
		[InlineData(ChartAxisLabelAlignment.End)]
		public void LabelAlignment_SetAndGet_ReturnsExpectedValue(ChartAxisLabelAlignment alignment)
		{
			var labelStyle = new ChartAxisLabelStyle
			{
				LabelAlignment = alignment
			};

			var actualAlignment = labelStyle.LabelAlignment;

			Assert.Equal(alignment, actualAlignment);
		}

		[Theory]
		[InlineData(ChartLabelAlignment.Center)]
		[InlineData(ChartLabelAlignment.Start)]
		[InlineData(ChartLabelAlignment.End)]
		public void HorizontalTextAlignment_SetAndGet_ReturnsExpectedValue(ChartLabelAlignment alignment)
		{
			var labelStyle = new ChartAnnotationLabelStyle
			{
				HorizontalTextAlignment = alignment
			};

			var actualAlignment = labelStyle.HorizontalTextAlignment;

			Assert.Equal(alignment, actualAlignment);
		}

		[Theory]
		[InlineData(ChartLabelAlignment.Center)]
		[InlineData(ChartLabelAlignment.Start)]
		[InlineData(ChartLabelAlignment.End)]
		public void VerticalTextAlignment_SetAndGet_ReturnsExpectedValue(ChartLabelAlignment alignment)
		{
			var labelStyle = new ChartAnnotationLabelStyle
			{
				VerticalTextAlignment = alignment
			};

			var actualAlignment = labelStyle.VerticalTextAlignment;

			Assert.Equal(alignment, actualAlignment);
		}
	}
}