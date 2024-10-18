using Syncfusion.Maui.Toolkit.Charts; 

namespace Syncfusion.Maui.Toolkit.UnitTest
{
    public class StyleUnitTests:BaseUnitTest
    {
        [Fact]
        public void GetDefaultFontSize_ReturnsDefaultFontSize()
        {
            var chartDataLabelStyle = new ChartDataLabelStyle();
            var expectedFontSize = 12.0;
            var result = chartDataLabelStyle.GetDefaultFontSize();
            Assert.Equal(expectedFontSize, result);
        }

        [Fact]
        public void GetDefaultBackgroundColor_ReturnsTransparentBrush()
        {
            var chartDataLabelStyle = new ChartLabelStyle();
            var result = chartDataLabelStyle.GetDefaultBackgroundColor();
            var expectedBrush = new SolidColorBrush(Colors.Transparent);
            Assert.Equal(expectedBrush.Color, ((SolidColorBrush)result).Color); 
        }

        [Fact]
        public void GetDefaultTextColor_ReturnsTransparentColor()
        {
            var chartDataLabelStyle = new ChartDataLabelStyle();
            var expectedFontSize = Colors.Transparent; 
            var result = chartDataLabelStyle.GetDefaultTextColor();
            Assert.Equal(expectedFontSize, result); 
        }

        [Fact]
        public void GetDesiredSize_TextSizeWidthZeroAndTextEmpty_ReturnsTextSize()
        {
            var sfCartesianChart = new SfCartesianChart();
            sfCartesianChart.XAxes.Add(new NumericalAxis());
            var chartAxisTitle = new ChartAxisTitle();
            chartAxisTitle.Text = string.Empty;
            var textSize = new SizeF(0, 0);
            SetPrivateField(chartAxisTitle, "_textSize",textSize);
            var result = chartAxisTitle.GetDesiredSize();
            Assert.Equal((Size)textSize, result);
        }

        [Fact]
        public void GetDesiredSize_AxisIsVertical_ReturnsSizeWithMargins()
        {
            var sfCartesianChart = new SfCartesianChart();
            sfCartesianChart.XAxes.Add(new NumericalAxis());
            var chartAxisTitle = new ChartAxisTitle();
            chartAxisTitle.Text=string.Empty;
            var textSize = new SizeF(15,19);
            SetPrivateField(chartAxisTitle, "_textSize", textSize);
            var result = chartAxisTitle.GetDesiredSize();
            Assert.Equal(new Size(25, 33), result);
        }

        [Theory]
        [InlineData("Angle", true)]
        [InlineData("OffsetX", true)]
        [InlineData("OffsetY", true)]
        [InlineData("LabelPadding", true)]
        [InlineData("Margin", true)]
        [InlineData("FontSize", true)]
        [InlineData("FontFamily", true)]
        [InlineData("FontAttributes", true)]
        [InlineData("LabelFormat", true)]
        [InlineData("StrokeWidth", true)]
        [InlineData("", false)] 
        public void NeedDataLabelMeasure_ReturnsExpectedResult(string propertyName, bool expected)
        {
            var chartDataLabelStyle = new ChartDataLabelStyle();
            var result = chartDataLabelStyle.NeedDataLabelMeasure(propertyName);
            Assert.Equal(expected, result);
        }


        [Theory]
        [InlineData(true, false, true)] 
        [InlineData(false, true, true)]  
        [InlineData(false, false, false)] 
        [InlineData(true, true, true)]   
        public void CanDraw_ReturnsExpectedResult(bool isBackgroundUpdated, bool isStrokeUpdated, bool expected)
        {
            var chartDataLabelStyle = new ChartLabelStyle
            {
                IsBackgroundColorUpdated = isBackgroundUpdated,
                IsStrokeColorUpdated = isStrokeUpdated
            };
            var result = chartDataLabelStyle.CanDraw();
            Assert.Equal(expected, result);
        }

        [Fact]
        public void CanDraw_ReturnsTrue_WhenStrokeWidthIsGreaterThanZeroAndChartColorIsNotEmpty()
        {
            var chartLineStyle = new ChartLineStyle
            {
                StrokeWidth = 1,
                Stroke = Colors.Red 
            };
            var result = chartLineStyle.CanDraw();
            Assert.True(result);
        }

        [Fact]
        public void CanDraw_ReturnsFalse_WhenStrokeWidthIsZero()
        {
            var chartLineStyle = new ChartLineStyle
            {
                StrokeWidth = 0,
                Stroke = Colors.Red
            };
            var result = chartLineStyle.CanDraw();
            Assert.False(result);
        }

        [Fact]
        public void CanDraw_ReturnsFalse_WhenChartColorIsEmpty()
        {
            var chartLineStyle = new ChartLineStyle
            {
                StrokeWidth = 0,
                Stroke = Colors.Red
            };
            var result = chartLineStyle.CanDraw();
            Assert.False(result);
        }   
    }
}
