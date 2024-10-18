using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.Toolkit.UnitTest
{
    public class ChartBehaviorUnitTests
    {
         // Tooltip behavior tests cases

        [Fact]
        public void Background_SetValue_ReturnsExpectedValue()
        {
            ChartTooltipBehavior chartTooltipBehavior = new ChartTooltipBehavior();
            var background= new SolidColorBrush(Colors.Yellow);

            chartTooltipBehavior.Background = background;

            Assert.Equal(background, chartTooltipBehavior.Background);
        }

        [Theory]
        [InlineData(1000)]
        [InlineData(2000)]
        public void Duration_SetValue_ReturnsExpectedValue(int duration)
        {
            ChartTooltipBehavior chartTooltipBehavior = new ChartTooltipBehavior();
            chartTooltipBehavior.Duration = duration;

            Assert.Equal(duration, chartTooltipBehavior.Duration);
        }

        [Theory]
        [InlineData(FontAttributes.None)]
        [InlineData(FontAttributes.Bold)]
        [InlineData(FontAttributes.Italic)]
        public void FontAttributes_SetValue_ReturnsExpectedValue(FontAttributes fontAttributes)
        {
            ChartTooltipBehavior chartTooltipBehavior = new ChartTooltipBehavior();
            chartTooltipBehavior.FontAttributes = fontAttributes;

            Assert.Equal(fontAttributes, chartTooltipBehavior.FontAttributes);
        }

        [Theory]
        [InlineData("Arial")]
        [InlineData("Helvetica")]
        public void FontFamily_SetValue_ReturnsExpectedValue(string fontFamily)
        {
            ChartTooltipBehavior chartTooltipBehavior = new ChartTooltipBehavior();
            chartTooltipBehavior.FontFamily = fontFamily;

            Assert.Equal(fontFamily, chartTooltipBehavior.FontFamily);
        }

        [Theory]
        [InlineData(12.0f)]
        [InlineData(16.0f)]
        public void FontSize_SetValue_ReturnsExpectedValue(float fontSize)
        {
            ChartTooltipBehavior chartTooltipBehavior = new ChartTooltipBehavior();
            chartTooltipBehavior.FontSize = fontSize;

            Assert.Equal(fontSize, chartTooltipBehavior.FontSize);
        }

        [Fact]
        public void Margin_SetValue_ReturnsExpectedValue()
        {
            ChartTooltipBehavior chartTooltipBehavior = new ChartTooltipBehavior();
            var margin = new Thickness(10, 20, 30, 40);
            chartTooltipBehavior.Margin = margin;

            Assert.Equal(margin, chartTooltipBehavior.Margin);
        }

        [Fact]
        public void TextColor_SetValue_ReturnsExpectedValue()
        {
            ChartTooltipBehavior chartTooltipBehavior = new ChartTooltipBehavior();
            var textColor = Colors.Red;
            chartTooltipBehavior.TextColor = textColor;

            Assert.Equal(textColor, chartTooltipBehavior.TextColor);
        }

        // Trackball behavior tests cases

        [Theory]
        [InlineData(ChartTrackballActivationMode.TouchMove)]
        [InlineData(ChartTrackballActivationMode.LongPress)]
        [InlineData(ChartTrackballActivationMode.None)]
        public void ActivationMode_SetValue_ReturnsExpectedValue(ChartTrackballActivationMode activationMode)
        {
            ChartTrackballBehavior chartTrackballBehavior = new ChartTrackballBehavior();
            chartTrackballBehavior.ActivationMode = activationMode;

            Assert.Equal(activationMode, chartTrackballBehavior.ActivationMode);
        }

        [Theory]
        [InlineData(LabelDisplayMode.GroupAllPoints)]
        [InlineData(LabelDisplayMode.FloatAllPoints)]
        [InlineData(LabelDisplayMode.NearestPoint)]
        public void DisplayMode_SetValue_ReturnsExpectedValue(LabelDisplayMode displayMode)
        {
            ChartTrackballBehavior chartTrackballBehavior = new ChartTrackballBehavior();
            chartTrackballBehavior.DisplayMode = displayMode;

            Assert.Equal(displayMode, chartTrackballBehavior.DisplayMode);
        }

        [Fact]
        public void LabelStyle_SetValue_ReturnsExpectedValue()
        {
            ChartTrackballBehavior chartTrackballBehavior = new ChartTrackballBehavior();
            var labelStyle = new ChartLabelStyle
            {
                FontSize = 12,
                FontFamily = "Arial",
                TextColor = Colors.Red
            };

            chartTrackballBehavior.LabelStyle = labelStyle;

            Assert.Equal(labelStyle, chartTrackballBehavior.LabelStyle);
            Assert.Equal(12, chartTrackballBehavior.LabelStyle.FontSize);
            Assert.Equal("Arial", chartTrackballBehavior.LabelStyle.FontFamily);
            Assert.Equal(Colors.Red, chartTrackballBehavior.LabelStyle.TextColor);
        }

        [Fact]
        public void LineStyle_SetValue_ReturnsExpectedValue()
        {
            ChartTrackballBehavior chartTrackballBehavior = new ChartTrackballBehavior();
            var lineStyle = new ChartLineStyle
            {
                Stroke = new SolidColorBrush(Colors.Blue),
                StrokeWidth = 2
            };

            chartTrackballBehavior.LineStyle = lineStyle;

            Assert.Equal(lineStyle, chartTrackballBehavior.LineStyle);
            Assert.Equal(Colors.Blue, ((SolidColorBrush)chartTrackballBehavior.LineStyle.Stroke).Color);
            Assert.Equal(2, chartTrackballBehavior.LineStyle.StrokeWidth);
        }

        [Fact]
        public void MarkerSettings_SetValue_ReturnsExpectedValue()
        {
            ChartTrackballBehavior chartTrackballBehavior = new ChartTrackballBehavior();
            var markerSettings = new ChartMarkerSettings
            {
                Type = Syncfusion.Maui.Toolkit.Charts.ShapeType.Circle,
                Height = 10,
                Width = 10,
                Fill = new SolidColorBrush(Colors.Green)
            };

            chartTrackballBehavior.MarkerSettings = markerSettings;

            Assert.Equal(markerSettings, chartTrackballBehavior.MarkerSettings);
            Assert.Equal(Syncfusion.Maui.Toolkit.Charts.ShapeType.Circle, chartTrackballBehavior.MarkerSettings.Type);
            Assert.Equal(10, chartTrackballBehavior.MarkerSettings.Height);
            Assert.Equal(Colors.Green, ((SolidColorBrush)chartTrackballBehavior.MarkerSettings.Fill).Color);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ShowLabel_SetValue_ReturnsExpectedValue(bool showLabel)
        {
            ChartTrackballBehavior chartTrackballBehavior = new ChartTrackballBehavior();
            chartTrackballBehavior.ShowLabel = showLabel;

            Assert.Equal(showLabel, chartTrackballBehavior.ShowLabel);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void SowLine_SetValue_ReturnsExpectedValue(bool showLine)
        {
            ChartTrackballBehavior chartTrackballBehavior = new ChartTrackballBehavior();
            chartTrackballBehavior.ShowLine = showLine;

            Assert.Equal(showLine, chartTrackballBehavior.ShowLine);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ShowMarkers_SetValue_ReturnsExpectedValue(bool showMarkers)
        {
            ChartTrackballBehavior chartTrackballBehavior = new ChartTrackballBehavior();
            chartTrackballBehavior.ShowMarkers = showMarkers;

            Assert.Equal(showMarkers, chartTrackballBehavior.ShowMarkers);
        }

        // Zooming behavior tests cases

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void EnableDirectionalZooming_SetValue_ReturnsExpectedValue(bool enableDirectionalZooming)
        {
            ChartZoomPanBehavior chartZoomPanBehavior = new ChartZoomPanBehavior();
            chartZoomPanBehavior.EnableDirectionalZooming = enableDirectionalZooming;

            Assert.Equal(enableDirectionalZooming, chartZoomPanBehavior.EnableDirectionalZooming);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void EnableDoubleTap_SetValue_ReturnsExpectedValue(bool enableDoubleTap)
        {
            ChartZoomPanBehavior chartZoomPanBehavior = new ChartZoomPanBehavior();
            chartZoomPanBehavior.EnableDoubleTap = enableDoubleTap;

            Assert.Equal(enableDoubleTap, chartZoomPanBehavior.EnableDoubleTap);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void EnablePanning_SetValue_ReturnsExpectedValue(bool enablePanning)
        {
            ChartZoomPanBehavior chartZoomPanBehavior = new ChartZoomPanBehavior();
            chartZoomPanBehavior.EnablePanning = enablePanning;

            Assert.Equal(enablePanning, chartZoomPanBehavior.EnablePanning);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void EnablePinchZooming_SetValue_ReturnsExpectedValue(bool enablePinchZooming)
        {
            ChartZoomPanBehavior chartZoomPanBehavior = new ChartZoomPanBehavior();
            chartZoomPanBehavior.EnablePinchZooming = enablePinchZooming;

            Assert.Equal(enablePinchZooming, chartZoomPanBehavior.EnablePinchZooming);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void EnableSelectionZooming_SetValue_ReturnsExpectedValue(bool enableSelectionZooming)
        {
            ChartZoomPanBehavior chartZoomPanBehavior = new ChartZoomPanBehavior();
            chartZoomPanBehavior.EnableSelectionZooming = enableSelectionZooming;

            Assert.Equal(enableSelectionZooming, chartZoomPanBehavior.EnableSelectionZooming);
        }

        [Theory]
        [InlineData(1.0)]
        [InlineData(2.0)]
        public void MaximumZoomLevel_SetValue_ReturnsExpectedValue(double maximumZoomLevel)
        {
            ChartZoomPanBehavior chartZoomPanBehavior = new ChartZoomPanBehavior();
            chartZoomPanBehavior.MaximumZoomLevel = maximumZoomLevel;

            Assert.Equal(maximumZoomLevel, chartZoomPanBehavior.MaximumZoomLevel);
        }

        [Fact]
        public void SelectionRectFill_SetValue_ReturnsExpectedValue()
        {
            ChartZoomPanBehavior chartZoomPanBehavior = new ChartZoomPanBehavior();
            var selectionRectFill = new SolidColorBrush(Colors.LightGray);
            chartZoomPanBehavior.SelectionRectFill = selectionRectFill;

            Assert.Equal(selectionRectFill, chartZoomPanBehavior.SelectionRectFill);
        }

        [Fact]
        public void SelectionRectStroke_SetValue_ReturnsExpectedValue()
        {
            ChartZoomPanBehavior chartZoomPanBehavior = new ChartZoomPanBehavior();
            var selectionRectStroke = new SolidColorBrush(Colors.Black);
            chartZoomPanBehavior.SelectionRectStroke = selectionRectStroke;

            Assert.Equal(selectionRectStroke, chartZoomPanBehavior.SelectionRectStroke);
        }

        [Fact]
        public void SelectionRectStrokeDashArray_SetValue_ReturnsExpectedValue()
        {
            ChartZoomPanBehavior chartZoomPanBehavior = new ChartZoomPanBehavior();
            var dashArray = new DoubleCollection { 2, 2 };
            chartZoomPanBehavior.SelectionRectStrokeDashArray = dashArray;

            Assert.Equal(dashArray, chartZoomPanBehavior.SelectionRectStrokeDashArray);
        }

        [Theory]
        [InlineData(1.0)]
        [InlineData(2.0)]
        public void SelectionRectStrokeWidth_SetValue_ReturnsExpectedValue(double selectionRectStrokeWidth)
        {
            ChartZoomPanBehavior chartZoomPanBehavior = new ChartZoomPanBehavior();
            chartZoomPanBehavior.SelectionRectStrokeWidth = selectionRectStrokeWidth;

            Assert.Equal(selectionRectStrokeWidth, chartZoomPanBehavior.SelectionRectStrokeWidth);
        }

        [Theory]
        [InlineData(ZoomMode.X)]
        [InlineData(ZoomMode.Y)]
        [InlineData(ZoomMode.XY)]
        public void ZoomMode_SetValue_ReturnsExpectedValue(ZoomMode zoomMode)
        {
            ChartZoomPanBehavior chartZoomPanBehavior = new ChartZoomPanBehavior();
            chartZoomPanBehavior.ZoomMode = zoomMode;

            Assert.Equal(zoomMode, chartZoomPanBehavior.ZoomMode);
        }

        // Selection behavior tests cases

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void SelectedIndex_SetValue_ReturnsExpectedValue(int selectedIndex)
        {
            DataPointSelectionBehavior dataPointSelectionBehavior = new DataPointSelectionBehavior();
            dataPointSelectionBehavior.SelectedIndex = selectedIndex;

            Assert.Equal(selectedIndex, dataPointSelectionBehavior.SelectedIndex);
        }

        [Fact]
        public void SelectedIndexes_SetValue_ReturnsExpectedValue()
        {
            DataPointSelectionBehavior dataPointSelectionBehavior = new DataPointSelectionBehavior();
            var selectedIndexes = new List<int> { 0, 1, 2 };
            dataPointSelectionBehavior.SelectedIndexes = selectedIndexes;

            Assert.Equal(selectedIndexes, dataPointSelectionBehavior.SelectedIndexes);
        }

        [Fact]
        public void SelectionBrush_SetValue_ReturnsExpectedValue()
        {
            SeriesSelectionBehavior seriesSelectionBehavior = new SeriesSelectionBehavior();
            var selectionBrush = new SolidColorBrush(Colors.Blue);
            seriesSelectionBehavior.SelectionBrush = selectionBrush;

            Assert.Equal(selectionBrush, seriesSelectionBehavior.SelectionBrush);
        }

        [Theory]
        [InlineData(ChartSelectionType.Single)]
        [InlineData(ChartSelectionType.Multiple)]
        public void Type_SetValue_ReturnsExpectedValue(ChartSelectionType type)
        {
            var selectionBehavior = new DataPointSelectionBehavior();
            selectionBehavior.Type = type;

            Assert.Equal(type, selectionBehavior.Type);
        }
}
}