using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.Toolkit.UnitTest.Charts
{
	public class ChartLegendUnitTests
	{
		[Fact]
		public void ChartLegend_Constructor_InitializesDefaultsCorrectly()
		{
			var chartLegend = new ChartLegend();

			Assert.True(chartLegend.IsVisible);
			Assert.Equal(LegendPlacement.Top, chartLegend.Placement);
			Assert.Null(chartLegend.ItemsLayout);
			Assert.False(chartLegend.ToggleSeriesVisibility);
			Assert.Null(chartLegend.ItemTemplate);
			Assert.Null(chartLegend.LabelStyle);
		}

		[Fact]
		public void ChartLegendLabelStyle_Constructor_InitializesDefaultsCorrectly()
		{
			var labelStyle = new ChartLegendLabelStyle();

			Assert.Equal(new Color(0.28627452f, 0.27058825f, 0.30980393f, 1), labelStyle.TextColor);
			Assert.Equal(new Thickness(0), labelStyle.Margin);
			Assert.Equal(12, labelStyle.FontSize);
			Assert.Null(labelStyle.FontFamily);
			Assert.Equal(FontAttributes.None, labelStyle.FontAttributes);
		}

		[Theory]
		[InlineData(LegendPlacement.Left)]
		[InlineData(LegendPlacement.Top)]
		[InlineData(LegendPlacement.Right)]
		[InlineData(LegendPlacement.Bottom)]
		public void Placement_SetAndGet_ReturnsExpectedValue(LegendPlacement placement)
		{
			var legend = new ChartLegend
			{
				Placement = placement
			};

			Assert.Equal(placement, legend.Placement);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void ToggleSeriesVisibility_SetAndGet_ReturnsExpectedValue(bool visibility)
		{
			var legend = new ChartLegend
			{
				ToggleSeriesVisibility = visibility
			};

			Assert.Equal(visibility, legend.ToggleSeriesVisibility);
		}

		[Fact]
		public void ItemTemplate_SetAndGet_ReturnsExpectedValue()
		{
			var expectedTemplate = new DataTemplate(() => new Label { Text = "Test" });
			var legend = new ChartLegend
			{
				ItemTemplate = expectedTemplate
			};

			var actualTemplate = legend.ItemTemplate;

			Assert.Equal(expectedTemplate, actualTemplate);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void IsVisible_SetAndGet_ReturnsExpectedValue(bool isVisible)
		{
			var legend = new ChartLegend
			{
				IsVisible = isVisible
			};

			var actualVisibility = legend.IsVisible;

			Assert.Equal(isVisible, actualVisibility);
		}

		[Fact]
		public void ItemsLayout_SetAndGet_ReturnsExpectedValue()
		{
			var expectedLayout = new StackLayout();
			var legend = new ChartLegend
			{
				ItemsLayout = expectedLayout
			};

			var actualLayout = legend.ItemsLayout;

			Assert.Equal(expectedLayout, actualLayout);
		}

		[Fact]
		public void LabelStyle_SetAndGet_ReturnsExpectedValue()
		{
			var expectedStyle = new ChartLegendLabelStyle
			{
				FontSize = 12,
				FontFamily = "Arial",
				FontAttributes = FontAttributes.Bold,
				Margin = new Thickness(5),
				TextColor = Colors.Red
			};
			var legend = new ChartLegend
			{
				LabelStyle = expectedStyle
			};

			var actualStyle = legend.LabelStyle;

			Assert.Equal(expectedStyle, actualStyle);
		}

		[Theory]
		[InlineData(12.0)]
		[InlineData(14.5)]
		[InlineData(16.75)]
		public void FontSize_SetAndGet_ReturnsExpectedValue(double fontSize)
		{
			var labelStyle = new ChartLegendLabelStyle
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
			var labelStyle = new ChartLegendLabelStyle
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
			var labelStyle = new ChartLegendLabelStyle
			{
				FontAttributes = fontAttributes
			};

			var actualFontAttributes = labelStyle.FontAttributes;

			Assert.Equal(fontAttributes, actualFontAttributes);
		}

		[Theory]
		[InlineData(1, 2, 3, 4)]
		[InlineData(5, 6, 7, 8)]
		[InlineData(9, 10, 11, 12)]
		public void Margin_SetAndGet_ReturnsExpectedValue(int left, int top, int right, int bottom)
		{
			var expectedMargin = new Thickness(left, top, right, bottom);
			var labelStyle = new ChartLegendLabelStyle
			{
				Margin = expectedMargin
			};

			var actualMargin = labelStyle.Margin;

			Assert.Equal(expectedMargin, actualMargin);
		}
		[Fact]
		public void TextColor_SetAndGet_ReturnsExpectedValue()
		{
			var label = new ChartLegendLabelStyle();
			var expected = Colors.Green;

			label.TextColor = expected;

			Assert.Equal(expected, label.TextColor);
		}
	}
}
