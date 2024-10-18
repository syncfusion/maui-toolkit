using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.Toolkit.UnitTest
{
    public class ChartModelUnitTests
    {
        [Fact]
        public void ChartDataLabel_Constructor_InitializesDefaultsCorrectly()
        {
            ChartDataLabel chartDataLabel = new ChartDataLabel();

            Assert.Empty(chartDataLabel.Label);
            Assert.Equal(Brush.Transparent, chartDataLabel.Background);
            Assert.Equal(-1, chartDataLabel.Index);
            Assert.Equal(double.NaN, chartDataLabel.XPosition);
            Assert.Equal(double.NaN, chartDataLabel.YPosition);
            Assert.Null(chartDataLabel.Item);
        }

        [Theory]
        [InlineData("ChartData")]
        [InlineData("Test label property")]
        public void Label_SetValue_ReturnsExpectedValue(string label)
        {
            ChartDataLabel chartDataLabel = new ChartDataLabel();
            chartDataLabel.Label = label;

            Assert.Equal(label, chartDataLabel.Label);
        }

        [Theory]
        [InlineData("#FF0000")]
        [InlineData("#FFFF00")]
        [InlineData("#0000FF")]
        public void Background_SetValue_ReturnsExpectedValue(string colorName)
        {
            ChartDataLabel chartDataLabel = new ChartDataLabel();
            var color = Color.FromArgb(colorName);
            var background = new SolidColorBrush(color);
            chartDataLabel.Background=background;

            Assert.Equal(background, chartDataLabel.Background);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(-2.5)]
        public void Index_SetValue_ReturnsExpectedValue(int index)
        {
            ChartDataLabel chartDataLabel = new ChartDataLabel();
            chartDataLabel.Index = index;

            Assert.Equal(index, chartDataLabel.Index);
        }

        [Theory]
        [InlineData(-2)]
        [InlineData(1.2)]
        public void XPosition_SetValue_ReturnsExpectedValue(double xPosition)
        {
            ChartDataLabel chartDataLabel = new ChartDataLabel();
            chartDataLabel.XPosition = xPosition;

            Assert.Equal(xPosition, chartDataLabel.XPosition);
        }

        [Theory]
        [InlineData(1.0)]
        [InlineData(-1.2)]
        public void YPosition_SetValue_ReturnsExpectedValue(double yPosition)
        {
            ChartDataLabel chartDataLabel = new ChartDataLabel();
            chartDataLabel.YPosition = yPosition;

            Assert.Equal(yPosition, chartDataLabel.YPosition);
        }

        [Theory]
        [InlineData("String")]
        [InlineData(0)]
        public void Item_SetValue_ReturnsExpectedValue(object item)
        {
            ChartDataLabel chartDataLabel = new ChartDataLabel();
            chartDataLabel.Item = item;
            
            Assert.Equal(item, chartDataLabel.Item);
        }
    }
}
