using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.Toolkit.UnitTest
{
    public class ChartSeriesUnitTests
    {
        [Fact]
        public void AreaSeries_Constructor_InitializesDefaultsCorrectly()
        {
            var areaSeries = new AreaSeries();

            Assert.Null(areaSeries.Stroke); 

            Assert.False(areaSeries.ShowMarkers);
            
            Assert.Null(areaSeries.StrokeDashArray);
            
            Assert.NotNull(areaSeries.MarkerSettings);
            
            Assert.Equal(8, areaSeries.MarkerSettings.Height); 

            Assert.Null(areaSeries.MarkerSettings.Fill);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ShowMarkers_SetAndGet_ReturnsExpectedValue(bool expected)
        {
            var areaSeries = new AreaSeries();

            areaSeries.ShowMarkers = expected;

            Assert.Equal(expected, areaSeries.ShowMarkers);
        }

        [Fact]
        public void Stroke_SetAndGet_ReturnsExpectedValue()
        {
            var areaSeries = new AreaSeries();

            areaSeries.Stroke = new SolidColorBrush(Colors.Red);

            Assert.Equal(Colors.Red, ((SolidColorBrush)areaSeries.Stroke).Color);
        }

        [Theory]
        [InlineData("5,3")]
        [InlineData("10,2")]
        public void StrokeDashArray_SetAndGet_ReturnsExpectedValue(string expected)
        {
            var values = Array.ConvertAll(expected.Split(','), double.Parse);

            var areaSeries = new AreaSeries();
            areaSeries.StrokeDashArray = new DoubleCollection(values);

            Assert.Equal(new DoubleCollection(values), areaSeries.StrokeDashArray);
        }

        [Fact]
        public void MarkerSettings_SetAndGet_ReturnsExpectedValue()
        {
            var areaSeries = new AreaSeries();

            var markerSettings = new ChartMarkerSettings
            {
                Fill = new SolidColorBrush(Colors.Red),
                Height = 15,
                Width = 15
            };

            areaSeries.MarkerSettings = markerSettings;

            Assert.Equal(markerSettings, areaSeries.MarkerSettings);
        }

        [Fact]
        public void BoxAndWhiskerSeries_Constructor_InitializesDefaultsCorrectly()
        {
            var boxAndWhiskerSeries = new BoxAndWhiskerSeries();

            Assert.Equal(BoxPlotMode.Exclusive, boxAndWhiskerSeries.BoxPlotMode);
            Assert.Equal(Syncfusion.Maui.Toolkit.Charts.ShapeType.Circle, boxAndWhiskerSeries.OutlierShapeType);
            Assert.False(boxAndWhiskerSeries.ShowMedian);
            Assert.True(boxAndWhiskerSeries.ShowOutlier);
            Assert.Equal(new SolidColorBrush(Colors.Black), boxAndWhiskerSeries.Stroke);
            Assert.Equal(0d, boxAndWhiskerSeries.Spacing);
            Assert.Equal(0.8d, boxAndWhiskerSeries.Width);
        }

        [Theory]
        [InlineData(BoxPlotMode.Normal)]
        [InlineData(BoxPlotMode.Inclusive)]
        [InlineData(BoxPlotMode.Exclusive)]
        public void BoxPlotMode_SetAndGet_ReturnsExpectedValue(BoxPlotMode expected)
        {
            var boxAndWhiskerSeries = new BoxAndWhiskerSeries();

            boxAndWhiskerSeries.BoxPlotMode = expected;

            Assert.Equal(expected, boxAndWhiskerSeries.BoxPlotMode);
        }

        [Theory]
        [InlineData(Syncfusion.Maui.Toolkit.Charts.ShapeType.Circle)]
        [InlineData(Syncfusion.Maui.Toolkit.Charts.ShapeType.Rectangle)]
        [InlineData(Syncfusion.Maui.Toolkit.Charts.ShapeType.Plus)]
        [InlineData(Syncfusion.Maui.Toolkit.Charts.ShapeType.HorizontalLine)]
        [InlineData(Syncfusion.Maui.Toolkit.Charts.ShapeType.VerticalLine)]
        [InlineData(Syncfusion.Maui.Toolkit.Charts.ShapeType.InvertedTriangle)]
        [InlineData(Syncfusion.Maui.Toolkit.Charts.ShapeType.Cross)]
        [InlineData(Syncfusion.Maui.Toolkit.Charts.ShapeType.Custom)]
        [InlineData(Syncfusion.Maui.Toolkit.Charts.ShapeType.Diamond)]
        [InlineData(Syncfusion.Maui.Toolkit.Charts.ShapeType.Hexagon)]
        [InlineData(Syncfusion.Maui.Toolkit.Charts.ShapeType.Pentagon)]
        public void OutlierShapeType_SetAndGet_ReturnsExpectedValue(Syncfusion.Maui.Toolkit.Charts.ShapeType expected)
        {
            var boxAndWhiskerSeries = new BoxAndWhiskerSeries();

            boxAndWhiskerSeries.OutlierShapeType = expected;

            Assert.Equal(expected, boxAndWhiskerSeries.OutlierShapeType);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ShowMedian_SetAndGet_ReturnsExpectedValue(bool expected)
        {
            var boxAndWhiskerSeries = new BoxAndWhiskerSeries();

            boxAndWhiskerSeries.ShowMedian = expected;

            Assert.Equal(expected, boxAndWhiskerSeries.ShowMedian);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ShowOutlier_SetAndGet_ReturnsExpectedValue(bool expected)
        {
            var boxAndWhiskerSeries = new BoxAndWhiskerSeries();

            boxAndWhiskerSeries.ShowOutlier = expected;

            Assert.Equal(expected, boxAndWhiskerSeries.ShowOutlier);
        }

        [Theory]
        [InlineData(0.0)]
        [InlineData(0.5)]
        [InlineData(1.0)]
        public void Spacing_SetAndGet_ReturnsExpectedValue(double expected)
        {
            var boxAndWhiskerSeries = new BoxAndWhiskerSeries();

            boxAndWhiskerSeries.Spacing = expected;

            Assert.Equal(expected, boxAndWhiskerSeries.Spacing);
        }

        [Theory]
        [InlineData(0.0)]
        [InlineData(0.5)]
        [InlineData(0.8)]
        public void Width_SetAndGet_ReturnsExpectedValue(double expected)
        {
            var boxAndWhiskerSeries = new BoxAndWhiskerSeries();

            boxAndWhiskerSeries.Width = expected;

            Assert.Equal(expected, boxAndWhiskerSeries.Width);
        }

        [Fact]
        public void BubbleSeries_Constructor_InitializesDefaultsCorrectly()
        {
            var bubbleSeries = new BubbleSeries();

            Assert.Equal(10d, bubbleSeries.MaximumRadius);
            Assert.Equal(3d, bubbleSeries.MinimumRadius);
            Assert.Equal(SolidColorBrush.Transparent, bubbleSeries.Stroke);
            Assert.Equal(string.Empty, bubbleSeries.SizeValuePath);
            Assert.True(bubbleSeries.ShowZeroSizeBubbles);
        }

        [Theory]
        [InlineData(12d)]
        [InlineData(0d)]
        public void MaximumRadius_SetAndGet_ReturnsExpectedValue(double expected)
        {
            var bubbleSeries = new BubbleSeries();

            bubbleSeries.MaximumRadius = expected;

            Assert.Equal(expected, bubbleSeries.MaximumRadius);
        }

        [Theory]
        [InlineData(5d)]
        [InlineData(0d)]
        public void MinimumRadius_SetAndGet_ReturnsExpectedValue(double expected)
        {
            var bubbleSeries = new BubbleSeries();

            bubbleSeries.MinimumRadius = expected;

            Assert.Equal(expected, bubbleSeries.MinimumRadius);
        }

        [Fact]
        public void BubbleSeries_Stroke_SetAndGet_ReturnsExpectedValue()
        {
            var bubbleSeries = new BubbleSeries();

            bubbleSeries.Stroke = new SolidColorBrush(Colors.Red);

            Assert.Equal(new SolidColorBrush(Colors.Red), bubbleSeries.Stroke);
        }

        [Theory]
        [InlineData("Size")]
        [InlineData("Value")]
        public void SizeValuePath_SetAndGet_ReturnsExpectedValue(string expected)
        {
            var bubbleSeries = new BubbleSeries();

            bubbleSeries.SizeValuePath = expected;

            Assert.Equal(expected, bubbleSeries.SizeValuePath);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ShowZeroSizeBubbles_SetAndGet_ReturnsExpectedValue(bool expected)
        {
            var bubbleSeries = new BubbleSeries();

            bubbleSeries.ShowZeroSizeBubbles = expected;
            
            Assert.Equal(expected, bubbleSeries.ShowZeroSizeBubbles);
        }

        [Fact]
        public void CandleSeries_Constructor_InitializesDefaultsCorrectly()
        {
            var candleSeries = new CandleSeries();

            Assert.Null(candleSeries.Stroke);
            Assert.False(candleSeries.EnableSolidCandle);
        }

        [Fact]
        public void CandleSeries_Stroke_SetAndGet_ReturnsExpectedValue()
        {
            var candleSeries = new CandleSeries();

            candleSeries.Stroke = new SolidColorBrush(Colors.Red);

            Assert.Equal(new SolidColorBrush(Colors.Red), candleSeries.Stroke);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void EnableSolidCandle_SetAndGet_ReturnsExpectedValue(bool expected)
        {
            var candleSeries = new CandleSeries();

            candleSeries.EnableSolidCandle = expected;

            Assert.Equal(expected, candleSeries.EnableSolidCandle);
        }

        [Fact]
        public void ColumnSeries_Constructor_InitializesDefaultsCorrectly()
        {
            var columnSeries = new ColumnSeries();

            Assert.Equal(SolidColorBrush.Transparent,columnSeries.Stroke);
            Assert.Equal(0d,columnSeries.Spacing);
            Assert.Equal(0.8d, columnSeries.Width);
            Assert.Equal(new CornerRadius(0), columnSeries.CornerRadius);
        }

        [Theory]
        [InlineData(255, 255, 0, 0)] 
        [InlineData(255, 0, 255, 0)] 
        [InlineData(255, 0, 0, 255)] 
        public void ColumnSeries_Stroke_SetAndGet_ReturnsExpectedValue(byte a, byte r, byte g, byte b)
        {
            var columnSeries = new ColumnSeries();
            var expectedBrush = new SolidColorBrush(Color.FromRgba(r, g, b, a));

            columnSeries.Stroke = expectedBrush;

            Assert.Equal(expectedBrush, columnSeries.Stroke);
        }


        [Theory]
        [InlineData(0.1)]
        [InlineData(0.5)]
        [InlineData(0.9)]
        public void ColumnSeries_Spacing_SetAndGet_ReturnsExpectedValue(double expectedSpacing)
        {
            var columnSeries = new ColumnSeries();

            columnSeries.Spacing = expectedSpacing;

            Assert.Equal(expectedSpacing, columnSeries.Spacing);
        }

        [Theory]
        [InlineData(0.1)]
        [InlineData(0.5)]
        [InlineData(0.9)]
        public void ColumnSeries_Width_SetAndGet_ReturnsExpectedValue(double expected)
        {
            var columnSeries = new ColumnSeries();

            columnSeries.Width = expected;

            Assert.Equal(expected, columnSeries.Width);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(5)]
        [InlineData(10)]
        public void CornerRadius_SetAndGet_ReturnsExpectedValue(double expected)
        {
            var columnSeries = new ColumnSeries();

            columnSeries.CornerRadius = new CornerRadius(expected);

            Assert.Equal(new CornerRadius(expected), columnSeries.CornerRadius);
        }

        [Fact]
        public void DoughnutSeries_Constructor_InitializesDefaultsCorrectly()
        {
            var doughnutSeries = new DoughnutSeries();

            Assert.Equal(0.4d, doughnutSeries.InnerRadius);

            Assert.Null(doughnutSeries.CenterView);
        }

        [Theory]
        [InlineData(0.0)]
        [InlineData(0.4)]
        [InlineData(1.0)]
        public void InnerRadius_SetAndGet_ReturnsExpectedValue(double expected)
        {
            var doughnutSeries = new DoughnutSeries();

            doughnutSeries.InnerRadius = expected;

            Assert.Equal(expected, doughnutSeries.InnerRadius);
        }

        [Fact]
        public void CenterView_SetAndGet_ReturnsExpectedValue()
        {
            var doughnutSeries = new DoughnutSeries();

            doughnutSeries.CenterView = new StackLayout();

            Assert.Equal(new StackLayout(), doughnutSeries.CenterView);
        }

        [Fact]
        public void ErrorBarSeries_Constructor_InitializesDefaultsCorrectly()
        {
            var errorBarSeries = new ErrorBarSeries();

            Assert.Null(errorBarSeries.HorizontalErrorPath);
            Assert.Null(errorBarSeries.VerticalErrorPath);
            Assert.Equal(0.0, errorBarSeries.HorizontalErrorValue);
            Assert.Equal(0.0, errorBarSeries.VerticalErrorValue);
            Assert.Equal(ErrorBarMode.Both, errorBarSeries.Mode);
            Assert.Equal(ErrorBarType.Fixed, errorBarSeries.Type);
            Assert.Equal(ErrorBarDirection.Both, errorBarSeries.HorizontalDirection);
            Assert.Equal(ErrorBarDirection.Both, errorBarSeries.VerticalDirection);
            Assert.Equal(1 ,errorBarSeries.HorizontalLineStyle.StrokeWidth);
            Assert.Equal(Brush.Black, errorBarSeries.HorizontalLineStyle.Stroke);
            Assert.Equal(1, errorBarSeries.VerticalLineStyle.StrokeWidth);
            Assert.Equal(Brush.Black, errorBarSeries.VerticalLineStyle.Stroke);
            Assert.Equal(Brush.Black, errorBarSeries.HorizontalCapLineStyle.Stroke);
            Assert.Equal(1, errorBarSeries.HorizontalCapLineStyle.StrokeWidth);
            Assert.Equal(10, errorBarSeries.HorizontalCapLineStyle.CapLineSize);
            Assert.Equal(Brush.Black, errorBarSeries.VerticalCapLineStyle.Stroke);
            Assert.Equal(1, errorBarSeries.VerticalCapLineStyle.StrokeWidth);
            Assert.Equal(10, errorBarSeries.VerticalCapLineStyle.CapLineSize);
        }

        [Theory]
        [InlineData("Low")]
        [InlineData("High")]
        public void ErrorBarSeries_HorizontalErrorPath_SetAndGet_ReturnsExpectedValue(string expected)
        {
            var errorBarSeries = new ErrorBarSeries();

            errorBarSeries.HorizontalErrorPath = expected;

            Assert.Equal(expected, errorBarSeries.HorizontalErrorPath);
        }

        [Theory]
        [InlineData("Low")]
        [InlineData("High")]
        public void VerticalErrorPath_SetAndGet_ReturnsExpectedValue(string expected)
        {
            var errorBarSeries = new ErrorBarSeries();

            errorBarSeries.VerticalErrorPath = expected;

            Assert.Equal(expected, errorBarSeries.VerticalErrorPath);
        }

        [Theory]
        [InlineData(0.0)]
        [InlineData(0.5)]
        [InlineData(1.0)]
        public void HorizontalErrorValue_SetAndGet_ReturnsExpectedValue(double expected)
        {
            var errorBarSeries = new ErrorBarSeries();

            errorBarSeries.HorizontalErrorValue = expected;

            Assert.Equal(expected, errorBarSeries.HorizontalErrorValue);
        }

        [Theory]
        [InlineData(0.0)]
        [InlineData(5.0)]
        [InlineData(10.0)]
        public void VerticalErrorValue_SetAndGet_ReturnsExpectedValue(double expected)
        {
            var errorBarSeries = new ErrorBarSeries();

            errorBarSeries.VerticalErrorValue = expected;

            Assert.Equal(expected, errorBarSeries.VerticalErrorValue);
        }

        [Theory]
        [InlineData(ErrorBarType.Percentage)]
        [InlineData(ErrorBarType.StandardDeviation)]
        [InlineData(ErrorBarType.StandardError)]
        [InlineData(ErrorBarType.Fixed)]
        [InlineData(ErrorBarType.Custom)]
        public void Type_SetAndGet_ReturnsExpectedValue(ErrorBarType expected)
        {
            var errorBarSeries = new ErrorBarSeries();

            errorBarSeries.Type = expected;

            Assert.Equal(expected, errorBarSeries.Type);
        }

        [Theory]
        [InlineData(ErrorBarMode.Both)]
        [InlineData(ErrorBarMode.Horizontal)]
        [InlineData(ErrorBarMode.Vertical)]
        public void Mode_SetAndGet_ReturnsExpectedValue(ErrorBarMode expected)
        {
            var errorBarSeries = new ErrorBarSeries();

            errorBarSeries.Mode = expected;

            Assert.Equal(expected, errorBarSeries.Mode);
        }

        [Theory]
        [InlineData(ErrorBarDirection.Both)]
        [InlineData(ErrorBarDirection.Plus)]
        [InlineData(ErrorBarDirection.Minus)]
        public void HorizontalDirection_SetAndGet_ReturnsExpectedValue(ErrorBarDirection expected)
        {
            var errorBarSeries = new ErrorBarSeries();

            errorBarSeries.HorizontalDirection = expected;

            Assert.Equal(expected, errorBarSeries.HorizontalDirection);
        }

        [Theory]
        [InlineData(ErrorBarDirection.Both)]
        [InlineData(ErrorBarDirection.Plus)]
        [InlineData(ErrorBarDirection.Minus)]
        public void VerticalDirection_SetAndGet_ReturnsExpectedValue(ErrorBarDirection expected)
        {
            var errorBarSeries = new ErrorBarSeries();

            errorBarSeries.VerticalDirection = expected;

            Assert.Equal(expected, errorBarSeries.VerticalDirection);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(0, 255, 0)]
        public void HorizontalLineStyle_Stroke_SetAndGet_ReturnsExpectedValue(byte r, byte g, byte b)
        {
            var errorBarSeries = new ErrorBarSeries();
            var expectedColor = Color.FromRgb(r, g, b);
            var lineStyle = new ErrorBarLineStyle { Stroke = new SolidColorBrush(expectedColor) };

            errorBarSeries.HorizontalLineStyle = lineStyle;

            Assert.Equal(expectedColor, ((SolidColorBrush)errorBarSeries.HorizontalLineStyle.Stroke).Color);
        }

        [Theory]
        [InlineData(1.2)]
        [InlineData(2.5)]
        [InlineData(3.0)]
        public void HorizontalLineStyle_StrokeWidth_SetAndGet_ReturnsExpectedValue(double expected)
        {
            var errorBarSeries = new ErrorBarSeries();
            var lineStyle = new ErrorBarLineStyle { StrokeWidth = expected };

            errorBarSeries.HorizontalLineStyle = lineStyle;

            Assert.Equal(lineStyle.StrokeWidth, errorBarSeries.HorizontalLineStyle.StrokeWidth);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(0, 255, 0)]
        public void VerticalLineStyle_Stroke_SetAndGet_ReturnsExpectedValue(byte r, byte g, byte b)
        {
            var errorBarSeries = new ErrorBarSeries();
            var expectedColor = Color.FromRgb(r, g, b);
            var lineStyle = new ErrorBarLineStyle { Stroke = new SolidColorBrush(expectedColor) };

            errorBarSeries.VerticalLineStyle = lineStyle;

            Assert.Equal(expectedColor, ((SolidColorBrush)errorBarSeries.VerticalLineStyle.Stroke).Color);
        }

        [Theory]
        [InlineData(1.2)]
        [InlineData(2.5)]
        [InlineData(3.0)]
        public void VerticalLineStyle_StrokeWidth_SetAndGet_ReturnsExpectedValue(double expected)
        {
            var errorBarSeries = new ErrorBarSeries();
            var lineStyle = new ErrorBarLineStyle { StrokeWidth = expected };

            errorBarSeries.VerticalLineStyle = lineStyle;

            Assert.Equal(lineStyle.StrokeWidth, errorBarSeries.VerticalLineStyle.StrokeWidth);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(0, 255, 0)]
        public void HorizontalCapLineStyle_StrokeColor_SetAndGet_ReturnsExpectedValue(byte r, byte g, byte b)
        {
            var errorBarSeries = new ErrorBarSeries();
            var expectedColor = Color.FromRgb(r, g, b);
            var lineStyle = new ErrorBarCapLineStyle { Stroke = new SolidColorBrush(expectedColor) };

            errorBarSeries.HorizontalCapLineStyle = lineStyle;

            Assert.Equal(expectedColor, ((SolidColorBrush)errorBarSeries.HorizontalCapLineStyle.Stroke).Color);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(0, 255, 0)]
        public void VerticalCapLineStyle_StrokeColor_SetAndGet_ReturnsExpectedValue(byte r, byte g, byte b)
        {
            var errorBarSeries = new ErrorBarSeries();
            var expectedColor = Color.FromRgb(r, g, b);
            var lineStyle = new ErrorBarCapLineStyle { Stroke = new SolidColorBrush(expectedColor) };

            errorBarSeries.VerticalCapLineStyle = lineStyle;

            Assert.Equal(expectedColor, ((SolidColorBrush)errorBarSeries.VerticalCapLineStyle.Stroke).Color);
        }

        [Theory]
        [InlineData(40)]
        [InlineData(25)]
        [InlineData(15)]
        public void HorizontalCapLineStyle_CapSize_SetAndGet_ReturnsExpectedValue(int expected)
        {
            var errorBarSeries = new ErrorBarSeries();
            var capStyle = new ErrorBarCapLineStyle { CapLineSize = expected };

            errorBarSeries.HorizontalCapLineStyle = capStyle;

            Assert.Equal(expected, errorBarSeries.HorizontalCapLineStyle.CapLineSize);
        }

        [Theory]
        [InlineData(20)]
        [InlineData(10)]
        [InlineData(5)]
        public void VerticalCapLineStyle_CapSize_SetAndGet_ReturnsExpectedValue(int expected)
        {
            var errorBarSeries = new ErrorBarSeries();
            var capStyle = new ErrorBarCapLineStyle { CapLineSize = expected };

            errorBarSeries.VerticalCapLineStyle = capStyle;

            Assert.Equal(expected, errorBarSeries.VerticalCapLineStyle.CapLineSize);
        }

        [Theory]
        [InlineData(1.2)]
        [InlineData(2.5)]
        [InlineData(3.0)]
        public void VerticalCapLineStyle_StrokeWidth_SetAndGet_ReturnsExpectedValue(double expected)
        {
            var errorBarSeries = new ErrorBarSeries();
            var lineStyle = new ErrorBarCapLineStyle { StrokeWidth = expected };

            errorBarSeries.VerticalCapLineStyle = lineStyle;

            Assert.Equal(lineStyle.StrokeWidth, errorBarSeries.VerticalCapLineStyle.StrokeWidth);
        }

        [Theory]
        [InlineData(1.2)]
        [InlineData(2.5)]
        [InlineData(3.0)]
        public void HorizontalCapLineStyle_StrokeWidth_SetAndGet_ReturnsExpectedValue(double expected)
        {
            var errorBarSeries = new ErrorBarSeries();
            var lineStyle = new ErrorBarCapLineStyle { StrokeWidth = expected };

            errorBarSeries.HorizontalCapLineStyle = lineStyle;

            Assert.Equal(lineStyle.StrokeWidth, errorBarSeries.HorizontalCapLineStyle.StrokeWidth);
        }

        [Fact]
        public void FastLineSeries_Constructor_InitializesDefaultsCorrectly()
        {
            var fastLineSeries = new FastLineSeries();

            Assert.False(fastLineSeries.EnableAntiAliasing);
            Assert.Null(fastLineSeries.StrokeDashArray);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void EnableAntiAliasing_SetAndGet_ReturnsExpectedValue(bool expected)
        {
            var fastLineSeries = new FastLineSeries();

            fastLineSeries.EnableAntiAliasing = expected;

            Assert.Equal(expected, fastLineSeries.EnableAntiAliasing);
        }

        [Fact]
        public void Fastline_StrokeDashArray_SetAndGet_ReturnsExpectedValue()
        {
            var fastLineSeries = new FastLineSeries();
            var expected = new DoubleCollection { 5, 3 };

            fastLineSeries.StrokeDashArray = expected;

            Assert.Equal(expected, fastLineSeries.StrokeDashArray);
        }

        [Fact]
        public void HistogramSeries_Constructor_InitializesDefaultsCorrectly()
        {
            var histogramSeries = new HistogramSeries();

            Assert.True(histogramSeries.ShowNormalDistributionCurve); 
            Assert.Equal(1.0, histogramSeries.HistogramInterval);

            var defaultCurveStyle = histogramSeries.CurveStyle;
            Assert.NotNull(defaultCurveStyle); 
            Assert.Equal(1, defaultCurveStyle.StrokeWidth);
            Assert.Equal(SolidColorBrush.Transparent, histogramSeries.Stroke); 
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ShowNormalDistributionCurve_SetAndGet_ReturnsExpectedValue(bool expected)
        {
            var histogramSeries = new HistogramSeries();
            histogramSeries.ShowNormalDistributionCurve = expected;

            Assert.Equal(expected, histogramSeries.ShowNormalDistributionCurve);
        }

        [Theory]
        [InlineData(1.0)]
        [InlineData(10.0)]
        [InlineData(0.5)]
        public void HistogramInterval_SetAndGet_ReturnsExpectedValue(double expected)
        {
            var histogramSeries = new HistogramSeries();

            histogramSeries.HistogramInterval = expected;

            Assert.Equal(expected, histogramSeries.HistogramInterval);
        }

        [Fact]
        public void CurveStyle_SetAndGet_ReturnsExpectedValue()
        {
            var histogramSeries = new HistogramSeries();
            var expectedStyle = new ChartLineStyle { Stroke = Colors.Red, StrokeWidth = 2 };
            histogramSeries.CurveStyle = expectedStyle;

            Assert.Equal(expectedStyle, histogramSeries.CurveStyle);
        }

        [Fact]
        public void HistogramSeries_Stroke_SetAndGet_ReturnsExpectedValue()
        {
            var histogramSeries = new HistogramSeries();

            histogramSeries.Stroke = new SolidColorBrush(Colors.Red);

            Assert.Equal(new SolidColorBrush(Colors.Red), histogramSeries.Stroke);
        }

        [Fact]
        public void LineSeries_Constructor_InitializesDefaultsCorrectly()
        {
            var lineSeries = new LineSeries();

            Assert.Null(lineSeries.StrokeDashArray); 
            Assert.False(lineSeries.ShowMarkers);
            Assert.Null(lineSeries.MarkerSettings.Fill); 
            Assert.Equal(8, lineSeries.MarkerSettings.Height);
            Assert.Null(lineSeries.MarkerSettings.Stroke);
        }

        [Fact]
        public void LineSeries_StrokeDashArray_SetAndGet_ReturnsExpectedValue()
        {
            var lineSeries = new LineSeries();

            lineSeries.StrokeDashArray = new DoubleCollection { 2, 5 };

            Assert.Equal(new DoubleCollection { 2, 5 }, lineSeries.StrokeDashArray);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void LineSeries_ShowMarkers_SetAndGet_ReturnsExpectedValue(bool expected)
        {
            var lineSeries = new LineSeries();

            lineSeries.ShowMarkers = expected;

            Assert.Equal(expected, lineSeries.ShowMarkers);
        }

        [Fact]
        public void LineSeries_MarkerSettings_SetAndGet_ReturnsExpectedValue()
        {
            var lineSeries = new LineSeries();
            var markerSettings = new ChartMarkerSettings
            {
                Height = 15,
                Width = 20,
                Fill = new SolidColorBrush(Colors.Red)
            };

            lineSeries.MarkerSettings = markerSettings;

            Assert.Equal(15, lineSeries.MarkerSettings.Height);
            Assert.Equal(20, lineSeries.MarkerSettings.Width);
            Assert.Equal(Colors.Red, ((SolidColorBrush)lineSeries.MarkerSettings.Fill).Color);
        }

        [Fact]
        public void PieSeries_Constructor_InitializesDefaultsCorrectly()
        {
            var pieSeries = new PieSeries();

            Assert.Equal(-1, pieSeries.ExplodeIndex);
            Assert.Equal(10d, pieSeries.ExplodeRadius);
            Assert.False(pieSeries.ExplodeOnTouch);
            Assert.False(pieSeries.ExplodeAll);
            Assert.Equal(double.NaN, pieSeries.GroupTo);
            Assert.Equal(PieGroupMode.Value, pieSeries.GroupMode);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(3)]
        public void ExplodeIndex_SetAndGet_ReturnsExpectedValue(int expected)
        {
            var pieSeries = new PieSeries();

            pieSeries.ExplodeIndex = expected;

            Assert.Equal(expected, pieSeries.ExplodeIndex);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(10)]
        [InlineData(25.5)]
        public void ExplodeRadius_SetAndGet_ReturnsExpectedValue(double expected)
        {
            var pieSeries = new PieSeries();

            pieSeries.ExplodeRadius = expected;

            Assert.Equal(expected, pieSeries.ExplodeRadius);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ExplodeOnTouch_SetAndGet_ReturnsExpectedValue(bool expected)
        {
            var pieSeries = new PieSeries();

            pieSeries.ExplodeOnTouch = expected;

            Assert.Equal(expected, pieSeries.ExplodeOnTouch);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ExplodeAll_SetAndGet_ReturnsExpectedValue(bool expected)
        {
            var pieSeries = new PieSeries();

            pieSeries.ExplodeAll = expected;

            Assert.Equal(expected, pieSeries.ExplodeAll);
        }

        [Theory]
        [InlineData(double.NaN)]
        [InlineData(30)]
        [InlineData(100)]
        public void GroupTo_SetAndGet_ReturnsExpectedValue(double expected)
        {
            var pieSeries = new PieSeries();

            pieSeries.GroupTo = expected;

            Assert.Equal(expected, pieSeries.GroupTo);
        }

        [Theory]
        [InlineData(PieGroupMode.Value)]
        [InlineData(PieGroupMode.Percentage)]
        [InlineData(PieGroupMode.Angle)]
        public void GroupMode_SetAndGet_ReturnsExpectedValue(PieGroupMode expected)
        {
            var pieSeries = new PieSeries();

            pieSeries.GroupMode = expected;

            Assert.Equal(expected, pieSeries.GroupMode);
        }

        [Fact]
        public void PolarSeries_Constructor_InitializesDefaultsCorrectly()
        {
            var polarAreaSeries = new PolarAreaSeries();

            Assert.Null(polarAreaSeries.Stroke);

        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        public void PolarSeries_Stroke_SetAndGet_ReturnsExpectedValue(int r, int g, int b)
        {
            var polarAreaSeries = new PolarAreaSeries();
            Brush expectedBrush = new SolidColorBrush(Color.FromRgba(r, g, b, 255));

            polarAreaSeries.Stroke = expectedBrush;

            Assert.Equal(expectedBrush, polarAreaSeries.Stroke);
        }

        [Fact]
        public void RaqdialBarSeries_Constructor_InitializesDefaultsCorrectly()
        {
            var radialBarSeries = new RadialBarSeries();

            var expectedTrackFill = new SolidColorBrush(Color.FromRgba(0, 0, 0, 0.08));
            Assert.Equal(expectedTrackFill.Color, ((SolidColorBrush)radialBarSeries.TrackFill).Color);

            var expectedTrackStroke = new SolidColorBrush(Color.FromRgba(0, 0, 0, 0.24));
            Assert.Equal(expectedTrackStroke.Color, ((SolidColorBrush)radialBarSeries.TrackStroke).Color);

            Assert.Equal(0d, radialBarSeries.TrackStrokeWidth);
            Assert.True(double.IsNaN(radialBarSeries.MaximumValue));
            Assert.Equal(0.2d, radialBarSeries.GapRatio);
            Assert.Null(radialBarSeries.CenterView);
            Assert.Equal(CapStyle.BothFlat, radialBarSeries.CapStyle);
            Assert.Equal(0.4d, radialBarSeries.InnerRadius);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        public void TrackFill_SetAndGet_ReturnsExpectedValue(int r, int g, int b)
        {
            var radialBarSeries = new RadialBarSeries();
            Brush expected = new SolidColorBrush(Color.FromRgba(r, g, b, 255));

            radialBarSeries.TrackFill = expected;

            Assert.Equal(expected, radialBarSeries.TrackFill);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        public void TrackStroke_SetAndGet_ReturnsExpectedValue(int r, int g, int b)
        {
            var radialBarSeries = new RadialBarSeries();
            Brush expected = new SolidColorBrush(Color.FromRgba(r, g, b, 255));

            radialBarSeries.TrackStroke = expected;

            Assert.Equal(expected, radialBarSeries.TrackStroke);
        }

        [Theory]
        [InlineData(1d)]
        [InlineData(2d)]
        public void TrackStrokeWidth_SetAndGet_ReturnsExpectedValue(double expected)
        {
            var radialBarSeries = new RadialBarSeries();

            radialBarSeries.TrackStrokeWidth = expected;

            Assert.Equal(expected, radialBarSeries.TrackStrokeWidth);
        }

        [Theory]
        [InlineData(100d)]
        [InlineData(200d)]
        public void MaximumValue_SetAndGet_ReturnsExpectedValue(double expected)
        {
            var radialBarSeries = new RadialBarSeries();

            radialBarSeries.MaximumValue = expected;

            Assert.Equal(expected, radialBarSeries.MaximumValue);
        }

        [Theory]
        [InlineData(0.1)]
        [InlineData(0.5)]
        public void GapRatio_SetAndGet_ReturnsExpectedValue(double expected)
        {
            var radialBarSeries = new RadialBarSeries();

            radialBarSeries.GapRatio = expected;

            Assert.Equal(expected, radialBarSeries.GapRatio);
        }

        [Fact]
        public void RadialBarSeries_CenterView_SetAndGet_ReturnsExpectedValue()
        {
            var radialBarSeries = new RadialBarSeries();
            var expected = new Label { Text = "CenterView" };

            radialBarSeries.CenterView = expected;

            Assert.Equal(expected, radialBarSeries.CenterView);
        }

        [Theory]
        [InlineData(CapStyle.BothFlat)]
        [InlineData(CapStyle.BothCurve)]
        [InlineData(CapStyle.EndCurve)]
        [InlineData(CapStyle.StartCurve)]
        public void CapStyle_SetAndGet_ReturnsExpectedValue(CapStyle expected)
        {
            var radialBarSeries = new RadialBarSeries();

            radialBarSeries.CapStyle = expected;

            Assert.Equal(expected, radialBarSeries.CapStyle);
        }

        [Theory]
        [InlineData(0.0)]
        [InlineData(1.0)]
        public void RadialBarSeries_InnerRadius_SetAndGet_ReturnsExpectedValue(double expected)
        {
            var radialBarSeries = new RadialBarSeries();

            radialBarSeries.InnerRadius = expected;

            Assert.Equal(expected, radialBarSeries.InnerRadius);
        }

        [Fact]
        public void RangeAreaSeries_Constructor_InitializesDefaultsCorrectly()
        {
            var rangeAreaSeries = new RangeAreaSeries();

            Assert.False(rangeAreaSeries.ShowMarkers);
            Assert.Null(rangeAreaSeries.MarkerSettings);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void RangeAreaSeries_ShowMarkers_SetAndGet_ReturnsExpectedValue(bool expected)
        {
            var rangeAreaSeries = new RangeAreaSeries();

            rangeAreaSeries.ShowMarkers = expected;

            Assert.Equal(expected, rangeAreaSeries.ShowMarkers);
        }

        [Fact]
        public void RangeAreaSeries_MarkerSettings_SetAndGet_ReturnsExpectedValue()
        {
            var rangeAreaSeries = new RangeAreaSeries();
            var expectedMarkerSettings = new ChartMarkerSettings
            {
                Fill = new SolidColorBrush(Colors.Red),
                Height = 15,
                Width = 15
            };

            rangeAreaSeries.MarkerSettings = expectedMarkerSettings;

            Assert.Equal(expectedMarkerSettings.Fill, rangeAreaSeries.MarkerSettings.Fill);
            Assert.Equal(expectedMarkerSettings.Height, rangeAreaSeries.MarkerSettings.Height);
            Assert.Equal(expectedMarkerSettings.Width, rangeAreaSeries.MarkerSettings.Width);
        }

        [Fact]
        public void RangeColumnSeries_Constructor_InitializesDefaultsCorrectly()
        {
            var rangeColumnSeries = new RangeColumnSeries();

            Assert.Equal(0.0, rangeColumnSeries.Spacing);
            Assert.Equal(new CornerRadius(0), rangeColumnSeries.CornerRadius); 
            Assert.Equal(0.8, rangeColumnSeries.Width);

        }

        [Theory]
        [InlineData(0.0)]
        [InlineData(0.5)]
        [InlineData(1.0)]
        public void RangeColumnSeries_Spacing_SetAndGet_ReturnsExpectedValue(double expected)
        {
            var rangeColumnSeries = new RangeColumnSeries();

            rangeColumnSeries.Spacing = expected;
           
            Assert.Equal(expected, rangeColumnSeries.Spacing);
        }

        [Fact]
        public void RangeColumnSeries_CornerRadius_SetAndGet_ReturnsExpectedValue()
        {
            var rangeColumnSeries = new RangeColumnSeries();
            var expectedCornerRadius = new CornerRadius(5);

            rangeColumnSeries.CornerRadius = expectedCornerRadius;

            Assert.Equal(expectedCornerRadius.TopLeft, rangeColumnSeries.CornerRadius.TopLeft);
            Assert.Equal(expectedCornerRadius.TopRight, rangeColumnSeries.CornerRadius.TopRight);
            Assert.Equal(expectedCornerRadius.BottomLeft, rangeColumnSeries.CornerRadius.BottomLeft);
            Assert.Equal(expectedCornerRadius.BottomRight, rangeColumnSeries.CornerRadius.BottomRight);
        }

        [Theory]
        [InlineData(0.0)]
        [InlineData(0.5)]
        [InlineData(1.0)]
        public void RangeColumnSeries_Width_SetAndGet_ReturnsExpectedValue(double expected)
        {
            var rangeColumnSeries = new RangeColumnSeries();

            rangeColumnSeries.Width = expected;

            Assert.Equal(expected, rangeColumnSeries.Width);
        }

        [Fact]
        public void ScatterSeries_Constructor_InitializesDefaultsCorrectly()
        {
            var scatterSeries = new ScatterSeries();

            Assert.Equal(5, scatterSeries.PointHeight);
            Assert.Equal(5, scatterSeries.PointWidth);
            Assert.Null(scatterSeries.Stroke);
            Assert.Equal(Syncfusion.Maui.Toolkit.Charts.ShapeType.Circle, scatterSeries.Type);
        }

        [Theory]
        [InlineData(11.0d)]
        [InlineData(25.0d)]
        [InlineData(2.5d)]
        public void PointHeight_SetAndGet_ReturnsExpectedValue(double expected)
        {
            var scatterSeries = new ScatterSeries();

            scatterSeries.PointHeight = expected;

            Assert.Equal(expected, scatterSeries.PointHeight);
        }

        [Theory]
        [InlineData(11.0d)]
        [InlineData(25.0d)]
        [InlineData(2.5d)]
        public void PointWidth_SetAndGet_ReturnsExpectedValue(double expected)
        {
            var scatterSeries = new ScatterSeries();

            scatterSeries.PointWidth = expected;

            Assert.Equal(expected, scatterSeries.PointWidth);
        }

        [Fact]
        public void ScatterSeries_Stroke_SetAndGet_ReturnsExpectedValue()
        {
            var scatterSeries = new ScatterSeries();
            var expectedStroke = new SolidColorBrush(Colors.Red);

            scatterSeries.Stroke = expectedStroke;

            Assert.Equal(expectedStroke, scatterSeries.Stroke);
        }

        [Theory]
        [InlineData(Syncfusion.Maui.Toolkit.Charts.ShapeType.Circle)]
        [InlineData(Syncfusion.Maui.Toolkit.Charts.ShapeType.Rectangle)]
        [InlineData(Syncfusion.Maui.Toolkit.Charts.ShapeType.Plus)]
        [InlineData(Syncfusion.Maui.Toolkit.Charts.ShapeType.HorizontalLine)]
        [InlineData(Syncfusion.Maui.Toolkit.Charts.ShapeType.VerticalLine)]
        [InlineData(Syncfusion.Maui.Toolkit.Charts.ShapeType.InvertedTriangle)]
        [InlineData(Syncfusion.Maui.Toolkit.Charts.ShapeType.Cross)]
        [InlineData(Syncfusion.Maui.Toolkit.Charts.ShapeType.Custom)]
        [InlineData(Syncfusion.Maui.Toolkit.Charts.ShapeType.Diamond)]
        [InlineData(Syncfusion.Maui.Toolkit.Charts.ShapeType.Hexagon)]
        [InlineData(Syncfusion.Maui.Toolkit.Charts.ShapeType.Pentagon)]
        public void ScatterSeries_Type_SetAndGet_ReturnsExpectedValue(Syncfusion.Maui.Toolkit.Charts.ShapeType expected)
        {
            var scatterSeries = new ScatterSeries();

            scatterSeries.Type = expected;

            Assert.Equal(expected, scatterSeries.Type);
        }

        [Fact]
        public void SplineAreaSeries_Constructor_InitializesDefaultsCorrectly()
        {
            var splineAreaSeries = new SplineAreaSeries();

            Assert.Equal(SplineType.Natural, splineAreaSeries.Type);
        }

        [Theory]
        [InlineData(SplineType.Natural)]
        [InlineData(SplineType.Monotonic)]
        [InlineData(SplineType.Clamped)]
        [InlineData(SplineType.Cardinal)]
        public void SplineAreaSeries_Type_SetAndGet_ReturnsExpectedValue(SplineType expected)
        {
            var splineAreaSeries = new SplineAreaSeries();

            splineAreaSeries.Type = expected;

            Assert.Equal(expected, splineAreaSeries.Type);
        }

        [Fact]
        public void SplineRangeAreaSeries_Constructor_InitializesDefaultsCorrectly()
        {
            var series = new SplineRangeAreaSeries();

            Assert.False(series.ShowMarkers);
            Assert.Null(series.MarkerSettings);
            Assert.Equal(SplineType.Natural, series.Type);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void SplineRangeAreaSeries_ShowMarkers_SetAndGet_ReturnsExpectedValue(bool expected)
        {
            var series = new SplineRangeAreaSeries();

            series.ShowMarkers = expected;

            Assert.Equal(expected, series.ShowMarkers);
        }

        [Fact]
        public void SplineRangeAreaSeries_MarkerSettings_SetAndGet_ReturnsExpectedValue()
        {
            var series = new SplineRangeAreaSeries();
            var expectedMarkerSettings = new ChartMarkerSettings
            {
                Fill = new SolidColorBrush(Colors.Red),
                Height = 15,
                Width = 15
            };

            series.MarkerSettings = expectedMarkerSettings;

            Assert.Equal(expectedMarkerSettings, series.MarkerSettings);
        }

        [Theory]
        [InlineData(SplineType.Natural)]
        [InlineData(SplineType.Monotonic)]
        [InlineData(SplineType.Clamped)]
        [InlineData(SplineType.Cardinal)]
        public void SplineRangeAreaSeries_Type_SetAndGet_ReturnsExpectedValue(SplineType expected)
        {
            var series = new SplineRangeAreaSeries();

            series.Type = expected;

            Assert.Equal(expected, series.Type);
        }

        [Fact]
        public void SplineSeries_Constructor_InitializesDefaultsCorrectly()
        {
            var series = new SplineSeries();

            Assert.NotNull(series);
            Assert.Equal(SplineType.Natural, series.Type);
            Assert.False(series.ShowMarkers);
            Assert.Null(series.StrokeDashArray);
            Assert.Equal(8, series.MarkerSettings.Height);
            Assert.Null(series.MarkerSettings.Fill);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void SplineSeries_ShowMarkers_SetAndGet_ReturnsExpectedValue(bool expected)
        {
            var series = new SplineSeries();

            series.ShowMarkers = expected;

            Assert.Equal(expected, series.ShowMarkers);
        }

        [Theory]
        [InlineData(SplineType.Natural)]
        [InlineData(SplineType.Monotonic)]
        [InlineData(SplineType.Cardinal)]
        [InlineData(SplineType.Clamped)]
        public void SplineSeries_Type_SetAndGet_ReturnsExpectedValue(SplineType expected)
        {
            var series = new SplineSeries();

            series.Type = expected;

            Assert.Equal(expected, series.Type);
        }

        [Fact]
        public void SplineSeries_StrokeDashArray_SetAndGet_ReturnsExpectedValue()
        {
            var series = new SplineSeries();
            var expectedCollection = new DoubleCollection { 5, 3 };

            series.StrokeDashArray = expectedCollection;

            Assert.Equal(expectedCollection, series.StrokeDashArray);
        }

        [Fact]
        public void SplineSeries_MarkerSettings_SetAndGet_ReturnsExpectedValue()
        {
            var series = new SplineSeries();
            var expected = new ChartMarkerSettings
            {
                Fill = new SolidColorBrush(Colors.Red),
                Height = 15,
                Width = 15,
            };

            series.MarkerSettings = expected;

            Assert.Equal(expected, series.MarkerSettings);
        }

        [Fact]
        public void WaterfallSeries_Constructor_InitializesDefaultsCorrectly()
        {
            WaterfallSeries waterfallSeries = new WaterfallSeries();
            ChartLineStyle chartLineStyle = new ChartLineStyle();
            waterfallSeries.ConnectorLineStyle = chartLineStyle;

            Assert.NotNull(waterfallSeries);
            Assert.Equal(0d, waterfallSeries.Spacing);
            Assert.True(waterfallSeries.AllowAutoSum);
            Assert.True(waterfallSeries.ShowConnectorLine);
            Assert.Equal(0.8d, waterfallSeries.Width);
            Assert.NotNull(waterfallSeries.ConnectorLineStyle);
            Assert.Equal(1, chartLineStyle.StrokeWidth);
            var expectedBrush = new SolidColorBrush(Color.FromRgba(0.7921569f, 0.76862746f, 0.8156863f, 1f));
            var actualBrush = chartLineStyle.Stroke as SolidColorBrush;
            
            if(actualBrush is not null)
            {
                Assert.Equal(expectedBrush.Color, actualBrush.Color);
            }

            Assert.Null(waterfallSeries.NegativePointsBrush);
            Assert.Null(waterfallSeries.SummaryPointsBrush);
            Assert.Empty(waterfallSeries.SummaryBindingPath);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void WaterfallSerier_AllowAutoSum_SetValue_ReturnsExpectedValue(bool allowAutSum)
        {
            WaterfallSeries series = new WaterfallSeries();
            series.AllowAutoSum = allowAutSum;

            Assert.Equal(allowAutSum, series.AllowAutoSum);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void WaterfallSerier_ShowConnectorLine_SetValue_ReturnsExpectedValue(bool showConnectorLine)
        {
            WaterfallSeries waterfallSeries = new WaterfallSeries();
            waterfallSeries.ShowConnectorLine = showConnectorLine;

            Assert.Equal(showConnectorLine, waterfallSeries.ShowConnectorLine);
        }

        [Fact]
        public void WaterfallSeries_ConnectorLineStyle__SetValue_ReturnsExpectedValue()
        {
            WaterfallSeries waterfallSeries = new WaterfallSeries();
            ChartLineStyle chartLineStyle = new ChartLineStyle() { StrokeWidth = 5, Stroke = Colors.Brown };
            waterfallSeries.ConnectorLineStyle = chartLineStyle;

            Assert.Equal(waterfallSeries.ConnectorLineStyle.Stroke, chartLineStyle.Stroke);
            Assert.Equal(chartLineStyle.StrokeWidth, waterfallSeries.ConnectorLineStyle.StrokeWidth);
        }

        [Fact]
        public void WaterfallSeies_SummaryBindingPath_SetValue_ReturnsExpectedValue()
        {
            WaterfallSeries waterfallSeries = new WaterfallSeries();
            var summaryBindingPath = "IsSummary";
            waterfallSeries.SummaryBindingPath = summaryBindingPath;

            Assert.Equal(summaryBindingPath, waterfallSeries.SummaryBindingPath);
        }

        [Fact]
        public void WaterfallSeries_SummaryPointsBrush_SetValue_ReturnsExpectedValue()
        {
            WaterfallSeries waterfallSeries = new WaterfallSeries();
            var color = Color.FromArgb("#ff00");
            var summaryPointsBrush = new SolidColorBrush(color);
            waterfallSeries.SummaryPointsBrush = summaryPointsBrush;

            Assert.Same(summaryPointsBrush, waterfallSeries.SummaryPointsBrush);
        }

        [Fact]
        public void WaterfallSeries_NegativePointsBrush_SetValue_ReturnsExpectedValue()
        {
            WaterfallSeries waterfallSeries = new WaterfallSeries();
            var color = Color.FromArgb("#ff00");
            var negativePointsBrush = new SolidColorBrush(color);
            waterfallSeries.NegativePointsBrush = negativePointsBrush;

            Assert.Same(negativePointsBrush, waterfallSeries.NegativePointsBrush);
        }

        [Theory]
        [InlineData(20d)]
        [InlineData(100d)]
        public void WaterfallSeries_Width_SetValue_ReturnsExpectedValue(double width)
        {
            WaterfallSeries waterfallSeries = new WaterfallSeries();
            waterfallSeries.Width = width;

            Assert.Equal(width, waterfallSeries.Width);
        }

        [Theory]
        [InlineData(20d)]
        [InlineData(100d)]
        public void WaterfallSeries_Spacing_SetValue_ReturnsExpectedValue(double spacing)
        {
            WaterfallSeries waterfallSeries = new WaterfallSeries();
            waterfallSeries.Spacing = spacing;

            Assert.Equal(spacing, waterfallSeries.Spacing);
        }

        [Fact]
        public void StackingColumnSeries_Constructor_InitializesDefaultsCorrectly()
        {
            StackingColumnSeries stackingColumnSeries = new StackingColumnSeries();

            Assert.Empty(stackingColumnSeries.GroupingLabel);
            Assert.Null(stackingColumnSeries.StrokeDashArray);
            Assert.Equal(0d, stackingColumnSeries.Spacing);
            Assert.Equal(0.8d, stackingColumnSeries.Width);
            Assert.Equal(0d, stackingColumnSeries.CornerRadius);
        }

        [Fact]
        public void StackingColumnSeries_GrooupingLabel_SetValue_ReturnsExpectedValue()
        {
            StackingColumnSeries stackingColumnSeries = new StackingColumnSeries();
            var groupingLabel = "Groupone";
            stackingColumnSeries.GroupingLabel = groupingLabel;

            Assert.Equal(groupingLabel, stackingColumnSeries.GroupingLabel);
        }

        [Fact]
        public void StackingColumnSeries_Stroke_SetValue_ReturnsExpectedValue()
        {
            StackingColumnSeries stackingColumnSeries = new StackingColumnSeries();
            var color = Color.FromArgb("#ff00");
            var stroke = new SolidColorBrush(color);
            stackingColumnSeries.Stroke = stroke;

            Assert.Same(stroke, stackingColumnSeries.Stroke);
        }

        [Fact]
        public void StrokeDashArray_SetAndGet_ReturnsCorrectValue()
        {
            StackingColumnSeries stackingColumnSeries = new StackingColumnSeries();
            var stokeDashArray = new DoubleCollection { 5, 3 };
            stackingColumnSeries.StrokeDashArray = stokeDashArray;

            Assert.Equal(stokeDashArray, stackingColumnSeries.StrokeDashArray);
        }

        [Theory]
        [InlineData(20d)]
        [InlineData(100d)]
        public void StackingColumnSeries_Width_SetValue_ReturnsExpectedValue(double width)
        {
            StackingColumnSeries stackingColumnSeries = new StackingColumnSeries();
            stackingColumnSeries.Width = width;

            Assert.Equal(width, stackingColumnSeries.Width);
        }

        [Theory]
        [InlineData(20d)]
        [InlineData(100d)]
        public void StackingColumnSeries_Spacing_SetValue_ReturnsExpectedValue(double spacing)
        {
            StackingColumnSeries stackingColumnSeries = new StackingColumnSeries();
            stackingColumnSeries.Spacing = spacing;

            Assert.Equal(spacing, stackingColumnSeries.Spacing);
        }

        [Theory]
        [InlineData(20d)]
        [InlineData(100d)]
        public void StackingColumnSeries_CornerRadius_SetValue_ReturnsExpectedValue(double cornerRadius)
        {
            StackingColumnSeries stackingColumnSeries = new StackingColumnSeries();
            stackingColumnSeries.CornerRadius = cornerRadius;

            Assert.Equal(cornerRadius, stackingColumnSeries.CornerRadius);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void StackingLineSeries_ShowMaker_SetValue_ReturnsExpectedValue(bool showMaker)
        {
            StackingLineSeries stackingLineSeries = new StackingLineSeries();
            stackingLineSeries.ShowMarkers = showMaker;

            Assert.Equal(showMaker, stackingLineSeries.ShowMarkers);
        }

        [Theory]
        [InlineData(100d)]
        [InlineData(120d)]
        public void StackingLineSeries_Width_SetValue_ReturnsExpectedValue(double width)
        {
            StackingLineSeries stackingLineSeries = new StackingLineSeries();
            ChartMarkerSettings chartMarkerSettings = new ChartMarkerSettings();
            stackingLineSeries.MarkerSettings = chartMarkerSettings;
            chartMarkerSettings.Width = width;

            Assert.Equal(width, chartMarkerSettings.Width);
        }

        [Theory]
        [InlineData(100d)]
        [InlineData(120d)]
        public void StackingLineSeries_StrokeWidth_SetValue_ReturnsExpectedValue(double strokeWidth)
        {
            StackingLineSeries stackingLineSeries = new StackingLineSeries();
            ChartMarkerSettings chartMarkerSettings = new ChartMarkerSettings();
            stackingLineSeries.MarkerSettings = chartMarkerSettings;
            chartMarkerSettings.StrokeWidth = strokeWidth;

            Assert.Equal(strokeWidth, chartMarkerSettings.StrokeWidth);
        }

        [Theory]
        [InlineData(100d)]
        [InlineData(120d)]
        public void StackingLineSeries_Height_SetValue_ReturnsExpectedValue(double height)
        {
            StackingLineSeries stackingLineSeries = new StackingLineSeries();
            ChartMarkerSettings chartMarkerSettings = new ChartMarkerSettings();
            stackingLineSeries.MarkerSettings = chartMarkerSettings;
            chartMarkerSettings.Height = height;

            Assert.Equal(height, chartMarkerSettings.Height);
        }

        [Fact]
        public void StackingLineSeries_Fill_SetValue_ReturnsExpectedValue()
        {
            StackingLineSeries stackingLineSeries = new StackingLineSeries();
            ChartMarkerSettings chartMarkerSettings = new ChartMarkerSettings();
            stackingLineSeries.MarkerSettings = chartMarkerSettings;
            var color = Color.FromArgb("#ff00");
            var fill = new SolidColorBrush(color);
            chartMarkerSettings.Fill = fill;

            Assert.Same(fill, chartMarkerSettings.Fill);
        }

        [Fact]
        public void StackingLineSeries_Stroke_SetValue_ReturnsExpectedValue()
        {
            StackingLineSeries stackingLineSeries = new StackingLineSeries();
            ChartMarkerSettings chartMarkerSettings = new ChartMarkerSettings();
            stackingLineSeries.MarkerSettings = chartMarkerSettings;
            var color = Color.FromArgb("#ff00");
            var stroke = new SolidColorBrush(color);
            chartMarkerSettings.Stroke = stroke;

            Assert.Same(stroke, chartMarkerSettings.Stroke);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void StackingAreaSeries_ShowMaker_SetValue_ReturnsExpectedValue(bool showMaker)
        {
            StackingAreaSeries stackingAreaSeries = new StackingAreaSeries();
            stackingAreaSeries.ShowMarkers = showMaker;

            Assert.Equal(showMaker, stackingAreaSeries.ShowMarkers);
        }

        [Theory]
        [InlineData(100d)]
        [InlineData(120d)]
        public void StackingAreaSeries_Width_SetValue_ReturnsExpectedValue(double width)
        {
            StackingAreaSeries stackingAreaSeries = new StackingAreaSeries();
            ChartMarkerSettings chartMarkerSettings = new ChartMarkerSettings();
            stackingAreaSeries.MarkerSettings = chartMarkerSettings;
            chartMarkerSettings.Width = width;

            Assert.Equal(width, chartMarkerSettings.Width);
        }

        [Theory]
        [InlineData(100d)]
        [InlineData(120d)]
        public void StackingAreaSeries_StrokeWidth_SetValue_ReturnsExpectedValue(double strokeWidth)
        {
            StackingAreaSeries stackingAreaSeries = new StackingAreaSeries();
            ChartMarkerSettings chartMarkerSettings = new ChartMarkerSettings();
            stackingAreaSeries.MarkerSettings = chartMarkerSettings;
            chartMarkerSettings.StrokeWidth = strokeWidth;

            Assert.Equal(strokeWidth, chartMarkerSettings.StrokeWidth);
        }

        [Theory]
        [InlineData(100d)]
        [InlineData(120d)]
        public void StackingAreaSeries_Height_SetValue_ReturnsExpectedValue(double height)
        {
            StackingAreaSeries stackingAreaSeries = new StackingAreaSeries();
            ChartMarkerSettings chartMarkerSettings = new ChartMarkerSettings();
            stackingAreaSeries.MarkerSettings = chartMarkerSettings;
            chartMarkerSettings.Height = height;

            Assert.Equal(height, chartMarkerSettings.Height);
        }

        [Fact]
        public void StackingAreaSeries_Fill_SetValue_ReturnsExpectedValue()
        {
            StackingAreaSeries stackingAreaSeries = new StackingAreaSeries();
            ChartMarkerSettings chartMarkerSettings = new ChartMarkerSettings();
            stackingAreaSeries.MarkerSettings = chartMarkerSettings;
            var color = Color.FromArgb("#ff00");
            var fill = new SolidColorBrush(color);
            chartMarkerSettings.Fill = fill;

            Assert.Same(fill, chartMarkerSettings.Fill);
        }

        [Fact]
        public void StackingAreaSeries_Stroke_SetValue_ReturnsExpectedValue()
        {
            StackingAreaSeries stackingAreaSeries = new StackingAreaSeries();
            ChartMarkerSettings chartMarkerSettings = new ChartMarkerSettings();
            stackingAreaSeries.MarkerSettings = chartMarkerSettings;
            var color = Color.FromArgb("#ff00");
            var stroke = new SolidColorBrush(color);
            chartMarkerSettings.Stroke = stroke;

            Assert.Same(stroke, chartMarkerSettings.Stroke);
        }
    }
}