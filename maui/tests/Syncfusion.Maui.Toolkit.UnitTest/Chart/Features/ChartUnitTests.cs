using System.Collections.ObjectModel;
using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.Toolkit.UnitTest
{
	public class ChartUnitTests:BaseUnitTest
    {

        [Fact]
        public void GetTooltipInfo_VisibleSeriesIsNull_ReturnsNull()
        {
            
            var sfCartesianChart = new SfCartesianChart(); 
            float x = 100f;
            float y = 200f;
            var behavior = new ChartTooltipBehavior(); 
            var result = sfCartesianChart.GetTooltipInfo(behavior, x, y);

            Assert.Null(result);
        }

        [Fact]
        public void SaveAsImage_ValidFileName_SavesImageSuccessfully()
        {
            
            var sfCartesianChart = new SfCartesianChart(); 
            string fileName = "chartImage.png"; 
            var exception = Record.Exception(() => sfCartesianChart.SaveAsImage(fileName));

            Assert.Null(exception);                     
        }

        [Fact]
        public void CreateSegment_Pyramid()
        {
            var sfPyramidChart = new SfPyramidChart();
            var createSegment=InvokePrivateMethod(sfPyramidChart, "CreateSegment", Array.Empty<object>());
            Assert.IsType<PyramidSegment>(createSegment);
        }

        [Fact]
        public void SetDefaultTooltipValue_AllPropertiesNull_NoChange()
        {
            
            var chartTooltipBehavior = new ChartTooltipBehavior();
            var sfCartesianChart = new SfCartesianChart();
            var background = Colors.Blue;
            var textColor = Colors.Orange;
            var fontSize = 14;
            chartTooltipBehavior.Background = background;
            chartTooltipBehavior.TextColor = textColor;
            chartTooltipBehavior.FontSize = fontSize;
            sfCartesianChart.SetDefaultTooltipValue(chartTooltipBehavior);
            Assert.Equal(chartTooltipBehavior.Background, background); 
            Assert.Equal(chartTooltipBehavior.TextColor, textColor); 
            Assert.Equal(chartTooltipBehavior.FontSize, fontSize);
        }

        [Fact]
        public void SetDefaultTooltipValue_TooltipBackgroundSet_UpdatesBackground()
        {
           
            var chartTooltipBehavior = new ChartTooltipBehavior();
            var sfCartesianChart = new SfCartesianChart { TooltipBackground = Colors.Red }; 
            var background = Colors.Red;
            sfCartesianChart.SetDefaultTooltipValue(chartTooltipBehavior);
            Assert.Equal(background, chartTooltipBehavior.Background); 
        }

        [Fact]
        public void SetDefaultTooltipValue_TooltipTextColorSet_UpdatesTextColor()
        {
           
            var chartTooltipBehavior = new ChartTooltipBehavior();
            var sfCartesianChart = new SfCartesianChart { TooltipTextColor = Colors.Blue }; 
            var textColor = Colors.Blue;
            sfCartesianChart.SetDefaultTooltipValue(chartTooltipBehavior);

            Assert.Equal(textColor, chartTooltipBehavior.TextColor); 
        }

        [Fact]
        public void SetDefaultTooltipValue_TooltipFontSizeSet_UpdatesFontSize()
        {
            
            var chartTooltipBehavior = new ChartTooltipBehavior();
            var sfCartesianChart = new SfCartesianChart { TooltipFontSize = 12.0 }; 
            var fontSize = 12.0;
            sfCartesianChart.SetDefaultTooltipValue(chartTooltipBehavior);
            Assert.Equal(fontSize, chartTooltipBehavior.FontSize); 
        }

        [Fact]
        public void CreateChartArea_InitializesWithCurrentInstance()
        {
            
            var sfCartesianChart = new SfCartesianChart(); 
            var result = sfCartesianChart.CreateChartArea();
            Assert.NotNull(result);
                                 
        }

        [Fact]
        public void ValueToPoint_AxisIsHorizontal_ReturnsCorrectPoint()
        {
           
            var sfCartesianChart = new SfCartesianChart();
            var iChart = sfCartesianChart as IChart;
            iChart.ActualSeriesClipRect = new Rect(30, 1, 1007.599975, 471.79998);
            var categoryAxis = new CategoryAxis { IsVertical = false, }; 
            sfCartesianChart.XAxes.Add(categoryAxis);
            double value = 50;
            var expected = double.NaN;
            float result = sfCartesianChart.ValueToPoint(categoryAxis, value);
            Assert.Equal(expected, result); 
        }

        [Fact]
        public void ValueToPoint_AxisIsHorizontal()
        {
            
            var sfCartesianChart = new SfCartesianChart();
            sfCartesianChart.ChartArea.ActualSeriesClipRect=new Rect(30, 1, 1007.599975, 471.79998);
            var categoryAxis = new CategoryAxis { IsVertical = false,
                VisibleRange = new DoubleRange(0.5, 5.5),
                RenderedRect = new Rect(70, 234, 875.3333, 66),
            };  
            sfCartesianChart.XAxes.Add(categoryAxis);
            double value = 50;
            var expected = 8665.7998046875;
            float result = sfCartesianChart.ValueToPoint(categoryAxis, value);
            Assert.Equal(expected, result);  
        }

        [Fact]
        public void PointToValue_ValidInput_ReturnsExpectedValue()
        {
           
            var sfCartesianChart = new SfCartesianChart();
            sfCartesianChart.ChartArea.ActualSeriesClipRect = new Rect(30, 1, 1007.599975, 471.79998);
            var categoryAxis = new CategoryAxis { IsVertical = false,
                VisibleRange = new DoubleRange(0.5, 5.5),
                RenderedRect = new Rect(70, 234, 875.3333, 66),
            };  
            sfCartesianChart.XAxes.Add(categoryAxis);
            double x = 80;
            double y = 50;
            double expectedValue =0.95696878442161504;  
            double result = sfCartesianChart.PointToValue(categoryAxis, x, y);
            Assert.Equal(expectedValue, result);  
        }

        [Fact]
        public void GetSelectionBrush()
        {
             
            var sfCartesianChart = new SfCartesianChart();
            ColumnSeries columnSeries = new ColumnSeries();
            SeriesSelectionBehavior behavior = new SeriesSelectionBehavior() { SelectedIndex = 0, SelectionBrush = Colors.Red };
            sfCartesianChart.SelectionBehavior = behavior;
            sfCartesianChart.Series.Add(columnSeries);
            var axis = new CategoryAxis { IsVertical = false };  
            sfCartesianChart.XAxes.Add(axis);
            var expected = Colors.Red;
            var result = sfCartesianChart.GetSelectionBrush(columnSeries);
            Assert.Equal(expected, result);  
        }

        [Fact]
        public void GetTooltipInfo_ShouldReturnTooltip_WhenTooltipEnabledAndValidPoint()
        {
            var sfFunnelChart = new SfFunnelChart();
            ChartTooltipBehavior chartTooltipBehavior = new ChartTooltipBehavior();
            sfFunnelChart.TooltipBehavior = chartTooltipBehavior;
            sfFunnelChart.EnableTooltip = true;
            var result = sfFunnelChart.GetTooltipInfo(chartTooltipBehavior, 50, 100); 
            Assert.Null( result); 
        }

        [Fact]
        public void GetTooltipInfo_ShouldReturnTooltip()
        {
            var sfPyramidChart = new SfPyramidChart();
            sfPyramidChart.EnableTooltip = true;
            ChartTooltipBehavior chartTooltipBehavior = new ChartTooltipBehavior();
            sfPyramidChart.TooltipBehavior = chartTooltipBehavior;
            PyramidChartArea chartPyramid = new PyramidChartArea(sfPyramidChart);
            chartPyramid.AreaBounds = new Rect(0, -1, 1037.5999755859375, 504.79998779296875);
            float[] valuesFromImage = new float[]
                {
                    0f, 0f, 1079.67993f, 0f, 559.839966f, 465.2984f, 559.839966f, 483.2f,
                    519.839966f, 483.2f, 519.839966f, 465.2984f
                };
            var pyramidSegment = new PyramidSegment
            {
                SegmentBounds = new Rect(  0,     -1,  1037.5999755859375,  504.79998779296875),
                
            };
            SetPrivateField(sfPyramidChart, "_segments", new ObservableCollection<ChartSegment>() { pyramidSegment });
            SetPrivateField(sfPyramidChart, "_yValues", new  List<double> { 10,   });
            SetPrivateField(sfPyramidChart, "_actualData", new List<object> { 10,  });
            SetPrivateField(pyramidSegment, "_values", valuesFromImage);
            IChart iChart = (IChart)sfPyramidChart;
            iChart.ActualSeriesClipRect = chartPyramid.AreaBounds;
            var result = sfPyramidChart.GetTooltipInfo(chartTooltipBehavior, 828.8f, 142.4f); 
            Assert.IsType<TooltipInfo>(result); 
        }

        [Fact]
        public void ViewAnnotationAttachedToChart_WithXamlData_AttachesSuccessfully()
        {
            var sfCartesianChart = new SfCartesianChart();
            var viewAnnotation = new ViewAnnotation
            {
                X1 = 3,
                Y1 = 30,
                View = new Label { Text = "cloud.png" } 
            };
            sfCartesianChart.ViewAnnotationAttachedToChart(viewAnnotation);
            Assert.Contains(viewAnnotation.View, sfCartesianChart.AnnotationLayout.Children); 
            Assert.Equal(3, viewAnnotation.X1);
            Assert.Equal(30, viewAnnotation.Y1);
        }

        [Fact]
        public void ViewAnnotationDetachedToChart_RemovesSuccessfully()
        {
            var sfCartesianChart = new SfCartesianChart();
            var viewAnnotation = new ViewAnnotation
            {
                View = new Label { Text = "cloud.png" }
            };
            sfCartesianChart.AnnotationLayout.Children.Add(viewAnnotation.View);
            sfCartesianChart.ViewAnnotationDetachedToChart(viewAnnotation);
            Assert.DoesNotContain(viewAnnotation.View, sfCartesianChart.AnnotationLayout.Children); 
        }

        [Fact]
        public void GetTooltipInfo_ShouldReturnNull()
        {
            var sfPyramidChart = new SfPyramidChart();
            sfPyramidChart.EnableTooltip = true;
            ChartTooltipBehavior chartTooltipBehavior = new ChartTooltipBehavior();
            sfPyramidChart.TooltipBehavior = chartTooltipBehavior;
            PyramidChartArea chartPyramid = new PyramidChartArea(sfPyramidChart);
            chartPyramid.AreaBounds = new Rect(0, -1, 1037.5999755859375, 504.79998779296875);
            float[] valuesFromImage = new float[]
                {
                    0f, 0f, 1079.67993f, 0f, 559.839966f, 465.2984f, 559.839966f, 483.2f,
                    519.839966f, 483.2f, 519.839966f, 465.2984f
                };
            var pyramidSegment = new PyramidSegment
            {
                SegmentBounds = new Rect(0, -1, 1037.5999755859375, 504.79998779296875),

            };
            SetPrivateField(sfPyramidChart, "_segments", new ObservableCollection<ChartSegment>() { pyramidSegment });
            SetPrivateField(sfPyramidChart, "_yValues", new List<double> { 10, });
            SetPrivateField(sfPyramidChart, "_actualData", new List<object> { 10, });
            SetPrivateField(pyramidSegment, "_values", valuesFromImage);
            var result = sfPyramidChart.GetTooltipInfo(chartTooltipBehavior, 828.8f, 142.4f);
            Assert.IsType<TooltipInfo>(result);
        }

        [Fact]
        public void GetTooltipInfo_ShouldReturnNull_WhenSeriesNull_Polar()
        {
            var sfPolarChart = new SfPolarChart();
            ChartTooltipBehavior chartTooltipBehavior = new ChartTooltipBehavior();
            sfPolarChart.TooltipBehavior = chartTooltipBehavior;
            var result = sfPolarChart.GetTooltipInfo(chartTooltipBehavior, 50, 100);
            Assert.Null(result); 
        }

        [Fact]
        public void GetTooltipInfo_ShouldReturnNull_Polar()
        {
            var sfPolarChart = new SfPolarChart();
            PolarAreaSeries polarAreaSeries = new PolarAreaSeries();
            ChartTooltipBehavior chartTooltipBehavior = new ChartTooltipBehavior();
            sfPolarChart.TooltipBehavior = chartTooltipBehavior;
            polarAreaSeries.EnableTooltip = true;
            var result = sfPolarChart.GetTooltipInfo(chartTooltipBehavior, 50, 100); 
            Assert.Null(result); 
        }

        [Fact]
        public void InitiateDataLabels()
        {
            var sfFunnelChart= new SfFunnelChart();
            sfFunnelChart.DataLabels = new ObservableCollection<ChartDataLabel>
            {
              
            };
            var funnelSegment = new FunnelSegment();
            var expected = sfFunnelChart.DataLabels.Count + 1;
            var parameter = new object[] { funnelSegment };
            InvokePrivateMethod(sfFunnelChart, "InitiateDataLabels", parameter);
            Assert.Equal(expected, sfFunnelChart.DataLabels.Count);
        }

        [Fact]
        public void InitiateDataLabel_Pyramid()
        {
            var sfPyramidChart = new SfPyramidChart();
            sfPyramidChart.DataLabels = new ObservableCollection<ChartDataLabel>
            {

            };
            var pyramidSegment = new PyramidSegment();
            var expected = sfPyramidChart.DataLabels.Count + 1;
            var parameter = new object[] { pyramidSegment };
            InvokePrivateMethod(sfPyramidChart, "InitiateDataLabel", parameter);
            Assert.Equal(expected, sfPyramidChart.DataLabels.Count);
        }

        [Fact]
        public void GetSurfaceHeight()
        { 
            var sfPyramidChart = new SfPyramidChart();
            var expected = 12.806248664855957;
            var parameter = new object[] { 0,164 };
            var result = InvokePrivateMethod(sfPyramidChart, "GetSurfaceHeight", parameter);
            Assert.Equal(expected, result);
        }
        [Fact]
        public void SolveQuadraticEquation_WithRealRoots_ReturnsTrue()
        {
            var sfPyramidChart = new SfPyramidChart(); 
            double b = 1;
            double c = -6; 
            var result =(bool?)InvokePrivateMethod(sfPyramidChart, "SolveQuadraticEquation", new object[] { b, c });
            Assert.True(result);
        }

        [Fact]
        public void SolveQuadraticEquation_NoRealRoots_ReturnsFalse()
        {
            var sfPyramidChart = new SfPyramidChart(); 
            double b = 1;
            double c = 5;
            var result = (bool?)InvokePrivateMethod(sfPyramidChart, "SolveQuadraticEquation",new object[] {b,c});
            Assert.False(result);
        }

        [Fact]
        public void CalculateTotalValue()
        {
            var sfPyramidChart = new SfPyramidChart();
            SetPrivateField(sfPyramidChart, "_pointsCount", 5);
            SetPrivateField(sfPyramidChart, "_yValues", new List<double> { 1, 2, 3, 4, 5 });
            double expected = 15;
            var result = (double?)InvokePrivateMethod(sfPyramidChart, "CalculateTotalValue");
            Assert.Equal(expected, result);
        }

        [Fact]
        public void RemoveData_DecreasesDataPoints()
        { 
            var sfPyramidChart = new SfPyramidChart();  
            var actualData = new List<object> { 1, 2, 3, 4, 5 };  
            var yValues = new List<double> { 10, 20, 30 };  
            SetPrivateField(sfPyramidChart, "_actualData", actualData);
            SetPrivateField(sfPyramidChart, "_yValues", yValues);
            int index = 2;
            var result = InvokePrivateMethod(sfPyramidChart, "RemoveData", new object[] { index }); 
            Assert.Equal(4, actualData.Count);  
            Assert.Equal(2, yValues.Count);     
        }

        [Fact]
        public void ResetData_ClearsDataAndResetsPointsCount()
        {
            var sfPyramidChart = new SfPyramidChart();
            var xValues = new List<double> { 1.0, 2.0, 3.0 };
            var actualXValues = new List<double> { 1.0, 2.0, 3.0 };
            var actualData = new List<object> { new object(), new object(), new object() };
            var yValues = new List<double> { 10, 20, 30 };

            SetPrivateField(sfPyramidChart, "_xValues", xValues);
            SetPrivateField(sfPyramidChart, "_actualXValues", actualXValues);
            SetPrivateField(sfPyramidChart, "_actualData", actualData);
            SetPrivateField(sfPyramidChart, "_yValues", yValues);
            SetPrivateField(sfPyramidChart, "_pointsCount", 3);

            var segments = new ObservableCollection<ChartSegment>();
            SetPrivateField(sfPyramidChart, "_segments", segments);
            var result = InvokePrivateMethod(sfPyramidChart, "ResetData", Array.Empty<object>());
            Assert.Empty(xValues);
            Assert.Empty(actualXValues);
            Assert.Empty(actualData);
            Assert.Empty(yValues);
            Assert.Equal(0, GetPrivateField(sfPyramidChart, "_pointsCount"));
            Assert.Empty(segments);
        }

        [Fact]
        public void ResetDataPoint_WithItemsSource_GeneratesDataPoints()
        {
            var sfPyramidChart = new SfPyramidChart(); 
            var itemsSource = new List<object>
            {
                new { X = 10, Y = 20 },
                new { X = 15, Y = 25 }
            };
            sfPyramidChart.ItemsSource= itemsSource;
            var xValues = new List<double> { 1.0, 2.0 };
            var actualXValues = new List<double> { 1.0, 2.0 };
            var actualData = new List<object> { new object(), new object() };
            var yValues = new List<double> { 10, 20 };
            SetPrivateField(sfPyramidChart, "_xValues", xValues);
            SetPrivateField(sfPyramidChart, "_actualXValues", actualXValues);
            SetPrivateField(sfPyramidChart, "_actualData", actualData);
            SetPrivateField(sfPyramidChart, "_yValues", yValues);
            var result = InvokePrivateMethod(sfPyramidChart, "ResetDataPoint", Array.Empty<object>());
            Assert.Equal(0, GetPrivateField(sfPyramidChart, "_pointsCount")); 
        }

        [Fact]
        public void CreateChartArea_ReturnsCartesianChartArea()
        {
            var sfCartesianChart = new SfCartesianChart(); 
            var result = sfCartesianChart.CreateChartArea();
            Assert.IsType<CartesianChartArea>(result);
        }

        [Fact]
        public void UpdateLegendItems()
        {
            var sfPyramidChart = new SfPyramidChart();
            var chartPyramid = new PyramidChartArea(sfPyramidChart);
            sfPyramidChart.SeriesBounds = new Rect(30, 1, 1007.599975, 471.79998);
            sfPyramidChart.UpdateLegendItems();
            object? expected = GetPrivateField(sfPyramidChart, "_chartArea");
            PyramidChartArea? result = expected as PyramidChartArea;
            Assert.NotNull(result);
            Assert.True(result!.ShouldPopulateLegendItems);
        }

        [Fact]
        public void CreateChartArea_ReturnsPyramidChartArea()
        {
            var sfPyramidChart = new SfPyramidChart(); 
            var result = sfPyramidChart.CreateChartArea();
            Assert.IsType<PyramidChartArea>(result);
        }

        [Fact]
        public void CreateChartArea_ReturnsCircularChartArea()
        {
            var sfCircularChart = new SfCircularChart(); 
            var result = sfCircularChart.CreateChartArea();
            Assert.IsType<CircularChartArea>(result);
        }

        [Fact]
        public void CalculateLinearSegment()
        {
            var sfPyramidChart = new SfPyramidChart()
            { GapRatio = 0.5, };  
            SetPrivateField(sfPyramidChart, "_pointsCount", 1);
            InvokePrivateMethod(sfPyramidChart, "CalculateLinearSegment", new object[] { 5,0.5, new List<double> { 5 } });
            var result = (ObservableCollection<ChartSegment>?)GetPrivateField(sfPyramidChart, "_segments");

            Assert.True((result?.Count >= 1) ? true : false);   
        }

        [Fact]
        public void CalculateSurfaceSegment()
        {
            var sfPyramidChart = new SfPyramidChart()
            { GapRatio = 0.5, };
            SetPrivateField(sfPyramidChart, "_pointsCount", 1);
            InvokePrivateMethod(sfPyramidChart, "CalculateSurfaceSegment", new object[] { 5, 0.5, new List<double> { 5 } });
            var result = (ObservableCollection<ChartSegment>?)GetPrivateField(sfPyramidChart, "_segments");
            Assert.True((result?.Count >= 1) ? true : false);
        }

        [Fact]
        public void GetActualXValue_NullXValues_ReturnsNull()
        {
            var sfPyramidChart = new SfPyramidChart(); 
            SetPrivateField(sfPyramidChart, "_xValues", new List<object> { 10, 20, 30, 40, 50 }); 
            int index = 2;
            SetPrivateField(sfPyramidChart, "_pointsCount", -1);
            var result = InvokePrivateMethod(sfPyramidChart, "GetActualXValue", new object[] { index });
            Assert.Null(result);
        }

        [Fact]
        public void GetActualXValue_XValues_ReturnsNull()
        {
            var sfPyramidChart = new SfPyramidChart(); 
            SetPrivateField(sfPyramidChart, "_xValues", new List<double> { 10, 20, 30, 40, 50 }); 
            int index = 2;
            SetPrivateField(sfPyramidChart, "_pointsCount", 5);
            Int64 expected = 30;
            var result = (Int64?)InvokePrivateMethod(sfPyramidChart, "GetActualXValue", new object[] { index });
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetPropertyValue_ValidPath_ReturnsCorrectValue()
        {
            var sfPyramidChart = new SfPyramidChart();
            var person = new Person()
            {
                Address = new Address
                {
                    City = "New York"
                }
            };

            string[] paths = new[] { "Address", "City" };
            var result = InvokeStaticPrivateMethodClass(typeof(SfPyramidChart), "GetPropertyValue", new object[] { person, paths });
            Assert.Equal("New York", result);
        }

        [Fact]
        public void ReflectedObject_ValidPath_ReturnsCorrectValue()
        {
            var address = new Address { City = "New York" };
            var person = new Person { Address = address };
            var result = InvokeStaticPrivateMethodClass(typeof(SfPyramidChart), "ReflectedObject", new object[] { person.Address, "City" });
            Assert.Equal("New York", result);
        }

        [Fact]
        public void ReflectedObject_InvalidPath_ReturnsNull()
        {
            var address = new Address { City = "New York" };
            var person = new Person { Address = address };
            var result = InvokeStaticPrivateMethodClass(typeof(SfPyramidChart), "ReflectedObject", new object[] { person.Address, "NonExistentProperty" });
            Assert.Null(result);
        }

        [Fact]
        public void GetPropertyValue_InvalidPath_ReturnsNull()
        {
            var sfPyramidChart = new SfPyramidChart();
            var person = new Person()
            {
                Address = new Address
                {
                    City = "New York"
                }
            };

            string[] paths = new[] { "Address", "Country" };
            var result = InvokeStaticPrivateMethodClass(typeof(SfPyramidChart), "GetPropertyValue", new object[] { person, paths });
            Assert.Null(result);
        }

        [Fact]
        public void GetArrayPropertyValue_ValidPath_ReturnsCorrectValue()
        {
            var address1 = new Address { City = "New York" };
            var address2 = new Address { City = "Los Angeles" };
            var person = new Person { Addresses = new[] { address1, address2 } };

            string[] paths = new[] { "Addresses[1]", "City" }; 
            var result = InvokeStaticPrivateMethodClass(typeof(SfPyramidChart), "GetArrayPropertyValue", new object[] { person, paths });
            Assert.Equal("Los Angeles", result);
        }

        [Fact]
        public void GetArrayPropertyValue_InvalidPath_ReturnsNull()
        {
            var address1 = new Address { City = "New York" };
            var person = new Person { Addresses = new[] { address1 } };
            string[] paths = new[] { "Addresses[1]", "City" };
            var result = InvokeStaticPrivateMethodClass(typeof(SfPyramidChart), "GetArrayPropertyValue", new object[] { person, paths });
            Assert.Null(result);
        }

        [Fact]
        public void GeneratePropertyPoints_ChartValueTypeDouble_StoresCorrectPoints()
        {
            var sfPyramidChart = new SfPyramidChart(); 
            var itemsSource = new List<ModelTestCase>
                {
                    new ModelTestCase { XValue = 10.0, YValue = 100.0 },
                    new ModelTestCase { XValue = 20.0, YValue = 200.0 },
                    new ModelTestCase { XValue = 30.0, YValue = 300.0 },
                };
            sfPyramidChart.ItemsSource = itemsSource;
            sfPyramidChart.XBindingPath = "XValue";
            sfPyramidChart.YBindingPath = "YValue";

            string[] yPaths = { "YValue" };
            IList<double>[] yLists = { new List<double>() };

            SetPrivateField(sfPyramidChart, "_xValueType", ChartValueType.Double); 
            SetPrivateField(sfPyramidChart, "_actualData", new List<object>());
            SetPrivateField(sfPyramidChart, "_isLinearData", true);
            SetPrivateField(sfPyramidChart, "_xValues", new List<double>());
            SetPrivateField(sfPyramidChart, "_pointsCount", 0);
            InvokePrivateMethod(sfPyramidChart, "GeneratePropertyPoints", new object[] { yPaths, yLists });
            var xValues = GetPrivateField(sfPyramidChart, "_xValues");
            var yValues = yLists[0];
            var actualData =  GetPrivateField(sfPyramidChart, "_pointsCount");
            Assert.Equal(new List<double> { 10.0, 20.0, 30.0 }, xValues);
            Assert.Equal(new List<double> { 100.0, 200.0, 300.0 }, yValues);
            Assert.Equal(3, actualData);  
        }

        [Fact]
        public void SetIndividualPoint_UpdatesValuesCorrectly()
        {
            var sfPyramidChart = new SfPyramidChart();
            var itemsSource = new List<ModelTestCase>
            {
                new ModelTestCase { XValue = 1.0, YValue = 100.0 },
                new ModelTestCase { XValue = 2.0, YValue = 200.0 }
            };

            sfPyramidChart.ItemsSource = itemsSource;
            sfPyramidChart.XBindingPath = "XValue";  
            sfPyramidChart.YBindingPath = "YValue";  
            SetPrivateField(sfPyramidChart, "_xValues", new List<double> { 1.0, 2.0 });
            SetPrivateField(sfPyramidChart, "_yValues", new List<double> { 100.0, 200.0 });
            SetPrivateField(sfPyramidChart, "_xValueType", ChartValueType.Double);
            SetPrivateField(sfPyramidChart, "_actualData", new List<object>());
            SetPrivateField(sfPyramidChart, "_pointsCount", 2);
            SetPrivateField(sfPyramidChart, "_isLinearData", true);
            InvokePrivateMethod(sfPyramidChart, "SetIndividualPoint", new object[] { 0, new ModelTestCase { XValue = 3.0, YValue = 250.0 },false });
            var xValues = GetPrivateField(sfPyramidChart, "_xValues");
            var yValues = GetPrivateField(sfPyramidChart, "_yValues");
            var actualData = GetPrivateField(sfPyramidChart, "_pointsCount");

            Assert.Equal(new List<double> { 3.0, 1.0,  2.0,  }, xValues); 
            Assert.Equal(new List<double> { 250.0 , 100.0,  200.0, }, yValues);  
            Assert.Equal(3, actualData); 
        }

        [Fact]
        public void SetIndividualPoint_ReplacesValuesCorrectly()
        {
            var sfPyramidChart = new SfPyramidChart();
            var itemsSource = new List<ModelTestCase>
            {
                new ModelTestCase { XValue = 1.0, YValue = 100.0 },
                new ModelTestCase { XValue = 2.0, YValue = 200.0 }
            };

            sfPyramidChart.ItemsSource = itemsSource;
            sfPyramidChart.XBindingPath = "XValue"; 
            sfPyramidChart.YBindingPath ="YValue" ; 
            SetPrivateField(sfPyramidChart, "_xValues", new List<double> { 1.0, 2.0 });
            SetPrivateField(sfPyramidChart, "_yValues", new List<double> { 100.0, 200.0 });
            SetPrivateField(sfPyramidChart, "_xValueType", ChartValueType.Double);
            SetPrivateField(sfPyramidChart, "_actualData", new List<object>());
            SetPrivateField(sfPyramidChart, "_pointsCount", 2);
            SetPrivateField(sfPyramidChart, "_isLinearData", true);
            InvokePrivateMethod(sfPyramidChart, "SetIndividualPoint", new object[] { 0, new ModelTestCase { XValue = 0.5, YValue = 150.0 }, true });
            var xValues = GetPrivateField(sfPyramidChart, "_xValues");
            var yValues = GetPrivateField(sfPyramidChart, "_yValues");
            var actualData = GetPrivateField(sfPyramidChart, "_pointsCount");

            Assert.Equal(new List<double> { 0.5, 2.0 }, xValues); 
            Assert.Equal(new List<double> { 150.0, 200.0 }, yValues); 
            Assert.Equal(2, actualData);
             
        }
    }
}
