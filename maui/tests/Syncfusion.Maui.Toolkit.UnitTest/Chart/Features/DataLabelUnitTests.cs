using System.Reflection;
using Syncfusion.Maui.Toolkit.Charts;
using PointF = Microsoft.Maui.Graphics.PointF;
using SizeF = Microsoft.Maui.Graphics.SizeF;

namespace Syncfusion.Maui.Toolkit.UnitTest
{
	public class DataLabelUnitTests : BaseUnitTest
    {
        [Fact]
        public void GetAutoLabelPosition_ColumnSeries()
        {
            var sfCartesianChart=new SfCartesianChart();
            sfCartesianChart.ChartArea.ActualSeriesClipRect = new Rect(30, 1, 1007.599975, 471.79998);
            sfCartesianChart.SeriesBounds= new Rect(30, 1, 1007.599975, 471.79998);
            ColumnSeries columnSeries = new ColumnSeries();
            sfCartesianChart.Series.Add(columnSeries);
            columnSeries.ShowDataLabels = true;

            float x = 71.97143f, y = 393.44f;
            SizeF labelSize = new SizeF(23, 26);
            float padding = 3, borderWidth = 0f;
            var cartesianDataLabelSettings = new CartesianDataLabelSettings();
            var expectedResult = new PointF(71.97143f, 393.44f);
            columnSeries.DataLabelSettings = cartesianDataLabelSettings;
            PointF result = cartesianDataLabelSettings.GetAutoLabelPosition(columnSeries, x, y, labelSize, padding, borderWidth);
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void GetLabelPositionForRectangularSeries_NullYAxis_ReturnsOriginalLabelPoint()
        {
            var columnSeries = new ColumnSeries { ActualYAxis = null };
            columnSeries.ShowDataLabels = true;
            int index = 0;
            SizeF labelSize = new SizeF(10, 10);
            PointF labelPoint = new PointF(50, 50);
            float padding = 5;
            DataLabelAlignment barAlignment = DataLabelAlignment.Top;
            var cartesianDataLabelSettings = new CartesianDataLabelSettings();
            var expectedResult = new PointF(50, 50);
            columnSeries.DataLabelSettings = cartesianDataLabelSettings;
            PointF result = cartesianDataLabelSettings.GetLabelPositionForRectangularSeries(columnSeries, index, labelSize, labelPoint, padding, barAlignment);
            Assert.Equal(labelPoint, result);
        }

        [Theory]
        [InlineData(10, false, DataLabelPlacement.Inner, DataLabelAlignment.Top, 393.44f)] 
        [InlineData(-10, false, DataLabelPlacement.Inner, DataLabelAlignment.Top, 361.44f)]  
        [InlineData(10, true, DataLabelPlacement.Inner, DataLabelAlignment.Top, 361.44f)] 
        [InlineData(-10, true, DataLabelPlacement.Inner, DataLabelAlignment.Top, 393.44f)]  

        [InlineData(10, false, DataLabelPlacement.Outer, DataLabelAlignment.Top, 361.44f)] 
        [InlineData(-10, false, DataLabelPlacement.Outer, DataLabelAlignment.Top, 393.44f)] 
        [InlineData(10, true, DataLabelPlacement.Outer, DataLabelAlignment.Top, 393.44f)]   
        [InlineData(-10, true, DataLabelPlacement.Outer, DataLabelAlignment.Top, 361.44f)]   

        [InlineData(10, false, DataLabelPlacement.Inner, DataLabelAlignment.Bottom, 361.44f)] 
        [InlineData(-10, false, DataLabelPlacement.Inner, DataLabelAlignment.Bottom, 393.44f)] 
        [InlineData(10, true, DataLabelPlacement.Inner, DataLabelAlignment.Bottom, 361.44f)] 
        [InlineData(-10, true, DataLabelPlacement.Inner, DataLabelAlignment.Bottom, 393.44f)]

        [InlineData(10, false, DataLabelPlacement.Outer, DataLabelAlignment.Bottom, 393.44f)]  
        [InlineData(-10, false, DataLabelPlacement.Outer, DataLabelAlignment.Bottom, 361.44f)] 
        [InlineData(10, true, DataLabelPlacement.Outer, DataLabelAlignment.Bottom, 393.44f)]   
        [InlineData(-10, true, DataLabelPlacement.Outer, DataLabelAlignment.Bottom, 361.44f)]  
        public void GetLabelPositionForRectangularSeries_TestCases(
        float yValue,
        bool isAxisInversed,
        DataLabelPlacement labelPlacement,
        DataLabelAlignment barAlignment,
        float expectedY)
        {
            var sfCartesianChart = new SfCartesianChart();
            sfCartesianChart.ChartArea.ActualSeriesClipRect = new Rect(30, 1, 1007.599975, 471.79998);
            sfCartesianChart.SeriesBounds = new Rect(30, 1, 1007.599975, 471.79998);
            ColumnSeries chartSeries = new ColumnSeries()
            {
                ActualYAxis = new NumericalAxis { IsInversed = isAxisInversed },
                YValues = new List<double> { yValue }
            };
            sfCartesianChart.Series.Add(chartSeries);
            chartSeries.ShowDataLabels = true;

            int index = 0;
            SizeF labelSize = new SizeF(23, 26);
            PointF labelPoint = new PointF(71.97143f, 377.44f);
            float padding = 3;
            var cartesianDataLabelSettings = new CartesianDataLabelSettings { LabelPlacement = labelPlacement };
            chartSeries.DataLabelSettings = cartesianDataLabelSettings;
            PointF result = cartesianDataLabelSettings.GetLabelPositionForRectangularSeries(chartSeries, index, labelSize, labelPoint, padding, barAlignment);
            Assert.Equal(new PointF(71.97143f, expectedY), result); 
        }

        [Fact]
        public void GetLabelPositionForTransposedRectangularSeries_PositiveValue_InnerPlacement()
        {
            var sfCartesianChart = new SfCartesianChart();
            sfCartesianChart.IsTransposed = true;
            sfCartesianChart.ChartArea.ActualSeriesClipRect = new Rect(30, 1, 1007.599975, 471.79998);
            sfCartesianChart.SeriesBounds = new Rect(30, 1, 1007.599975, 471.79998);
            var columnSeries = new ColumnSeries();
            columnSeries.ShowDataLabels = true;
            sfCartesianChart.Series.Add(columnSeries);
            int index = 0;
            SizeF labelSize = new SizeF(23, 26);
            PointF labelPoint = new PointF(197.72f,  438.099976f);
            float padding = 3;
            DataLabelAlignment barAlignment = DataLabelAlignment.Top;
            var cartesianDataLabelSettings = new CartesianDataLabelSettings();
            columnSeries.DataLabelSettings = cartesianDataLabelSettings;
            var expectedResult = new PointF(197.72f, 438.099976f);
            PointF result = cartesianDataLabelSettings.GetLabelPositionForTransposedRectangularSeries(columnSeries, index, labelSize, labelPoint, padding, barAlignment);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(10, false, DataLabelPlacement.Inner, DataLabelAlignment.Top, 183.22f)]   
        [InlineData(-10, false, DataLabelPlacement.Inner, DataLabelAlignment.Top, 212.22f)]    
        [InlineData(10, true, DataLabelPlacement.Inner, DataLabelAlignment.Top, 212.22f)]     
        [InlineData(-10, true, DataLabelPlacement.Inner, DataLabelAlignment.Top, 183.22f)]  

        [InlineData(10, false, DataLabelPlacement.Outer, DataLabelAlignment.Top, 212.22f)]    
        [InlineData(-10, false, DataLabelPlacement.Outer, DataLabelAlignment.Top, 183.22f)]    
        [InlineData(10, true, DataLabelPlacement.Outer, DataLabelAlignment.Top, 183.22f)]     
        [InlineData(-10, true, DataLabelPlacement.Outer, DataLabelAlignment.Top, 212.22f)]    

        [InlineData(10, false, DataLabelPlacement.Inner, DataLabelAlignment.Bottom, 212.22f)]  
        [InlineData(-10, false, DataLabelPlacement.Inner, DataLabelAlignment.Bottom, 183.22f)]
        [InlineData(10, true, DataLabelPlacement.Inner, DataLabelAlignment.Bottom, 212.22f)]   
        [InlineData(-10, true, DataLabelPlacement.Inner, DataLabelAlignment.Bottom, 183.22f)] 

        [InlineData(10, false, DataLabelPlacement.Outer, DataLabelAlignment.Bottom, 183.22f)]  
        [InlineData(-10, false, DataLabelPlacement.Outer, DataLabelAlignment.Bottom, 212.22f)] 
        [InlineData(10, true, DataLabelPlacement.Outer, DataLabelAlignment.Bottom, 183.22f)]  
        [InlineData(-10, true, DataLabelPlacement.Outer, DataLabelAlignment.Bottom, 212.22f)] 

        public void GetLabelPositionForTransposedRectangularSeries_TestCases(
             float yValue,
             bool isAxisInversed,
             DataLabelPlacement labelPlacement,
             DataLabelAlignment barAlignment,
            float expectedX)
        {
            var sfCartesianChart = new SfCartesianChart();
            sfCartesianChart.IsTransposed = true;
            sfCartesianChart.ChartArea.ActualSeriesClipRect = new Rect(30, 1, 1007.599975, 471.79998);
            sfCartesianChart.SeriesBounds = new Rect(30, 1, 1007.599975, 471.79998);
            ColumnSeries columnSeries = new ColumnSeries()
            {
                ActualYAxis = new NumericalAxis { IsInversed = isAxisInversed },
                YValues = new List<double> { yValue }
            };
            sfCartesianChart.Series.Add(columnSeries);
            columnSeries.ShowDataLabels = true;

            int index = 0;
            SizeF labelSize = new SizeF(23, 26);
            PointF labelPoint = new PointF(197.72f, 438.099976f);
            float padding = 3;
            var cartesianDataLabelSettings = new CartesianDataLabelSettings { LabelPlacement = labelPlacement };
            columnSeries.DataLabelSettings = cartesianDataLabelSettings;
            PointF result = cartesianDataLabelSettings.GetLabelPositionForTransposedRectangularSeries(columnSeries, index, labelSize, labelPoint, padding, barAlignment);
            Assert.Equal(new PointF(expectedX, 438.099976f), result);
        }


        [Theory]
        [InlineData(DataLabelPlacement.Outer, 0f, 471.8f, 23, 26, 3, 0f, 487.8f)] 
        [InlineData(DataLabelPlacement.Inner, 0f, 471.8f, 23, 26, 3, 0f, 455.8f)] 
        [InlineData(DataLabelPlacement.Auto, 0f, 471.8f, 23, 26, 3, 14.5f, -16f)] 
        public void GetLabelPositionForContinuousSeries_TheoryCases(DataLabelPlacement labelPlacement, float labelPointX, float labelPointY, float labelWidth, float labelHeight, float padding, float expectedX, float expectedY)
        {
            var sfCartesianChart = new SfCartesianChart();
            var lineSeries = new LineSeries()
            {
                ActualYAxis = new NumericalAxis(),
                ActualXAxis = new NumericalAxis(),
                YValues = [10]
            };
            int index = 0;
            SizeF labelSize = new SizeF(labelWidth, labelHeight);
            PointF labelPoint = new PointF(labelPointX, labelPointY);
            sfCartesianChart.Series.Add(lineSeries);
            var cartesianDataLabelSettings = new CartesianDataLabelSettings
            {
                LabelPlacement = labelPlacement
            };

            PointF expectedResult = new PointF(expectedX, expectedY);
            PointF result = cartesianDataLabelSettings.GetLabelPositionForContinuousSeries(lineSeries, index, labelSize, labelPoint, padding);
            Assert.Equal(expectedResult, result);
        }


        [Theory]
        [InlineData(DataLabelPlacement.Outer, 0f, 471.8f, 23, 26, 3, 5, 0f, 453.3f)] 
        [InlineData(DataLabelPlacement.Inner, 0f, 471.8f, 23, 26, 3, 5, 0f, 485.3f)] 
        [InlineData(DataLabelPlacement.Auto, 0f, 471.8f, 23, 26, 3, 5, 19.5f, -18.5f)] 
        public void GetLabelPositionForSeries_TheoryCases(DataLabelPlacement labelPlacement, float labelPointX, float labelPointY, float labelWidth, float labelHeight, float padding, float scatterSize, float expectedX, float expectedY)
        {
            var sfCartesianChart = new SfCartesianChart();
            var scatterSeries = new ScatterSeries()
            {
                ActualYAxis = new NumericalAxis(),
                ActualXAxis = new NumericalAxis(),
                YValues = [10]
            };
            sfCartesianChart.Series.Add(scatterSeries);
            SizeF labelSize = new SizeF(labelWidth, labelHeight);
            PointF labelPoint = new PointF(labelPointX, labelPointY);
            SizeF scatterMarkerSize = new SizeF(scatterSize, scatterSize);
            var cartesianDataLabelSettings = new CartesianDataLabelSettings
            {
                LabelPlacement = labelPlacement
            };
            scatterSeries.DataLabelSettings = cartesianDataLabelSettings;

            PointF expectedResult = new PointF(expectedX, expectedY);
            PointF result = cartesianDataLabelSettings.GetLabelPositionForSeries(scatterSeries, labelSize, labelPoint, padding, scatterMarkerSize);
            Assert.Equal(expectedResult, result);
        }
        [Fact]
        public void GetLabelPositionForAreaSeries_ValidInput_ReturnsExpectedPoint()
        {
            var sfCartesianChart = new SfCartesianChart();
            sfCartesianChart.ChartArea.ActualSeriesClipRect = new Rect(30, 1, 1007.599975, 471.79998);
            sfCartesianChart.SeriesBounds = new Rect(30, 1, 1007.599975, 471.79998);
            AreaSeries areaSeries = new AreaSeries()
            {
                ActualYAxis = new NumericalAxis(),
                ActualXAxis = new NumericalAxis(),
                YValues = [10]
            };
            AreaSegment areaSegment = new AreaSegment();
            sfCartesianChart.Series.Add(areaSeries); 
            SizeF labelSize = new SizeF(23, 26);
            PointF labelPoint = new PointF(0, 377.44f);
            float padding = 3;
            var cartesianDataLabelSettings = new CartesianDataLabelSettings();
            cartesianDataLabelSettings.LabelPlacement = DataLabelPlacement.Inner;
            areaSeries.DataLabelSettings = cartesianDataLabelSettings;

            var expectedResult = new PointF(0, 393.44f);
            PointF result = cartesianDataLabelSettings.GetLabelPositionForAreaSeries(areaSeries, areaSegment, labelSize, labelPoint, padding);
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void GetLabelPositionForAreaSeries_PlacementInner()
        {
            var sfCartesianChart = new SfCartesianChart();
            sfCartesianChart.ChartArea.ActualSeriesClipRect = new Rect(30, 1, 1007.599975, 471.79998);
            sfCartesianChart.SeriesBounds = new Rect(30, 1, 1007.599975, 471.79998);
            AreaSeries areaSeries = new AreaSeries()
            {
                ActualYAxis = new NumericalAxis(),
                ActualXAxis = new NumericalAxis(),
                YValues = [10]
            };
            AreaSegment areaSegment = new AreaSegment();
            sfCartesianChart.Series.Add(areaSeries);
            SizeF labelSize = new SizeF(23, 26);
            PointF labelPoint = new PointF(0, 377.44f);
            float padding = 3;
            var cartesianDataLabelSettings = new CartesianDataLabelSettings();
            cartesianDataLabelSettings.LabelPlacement = DataLabelPlacement.Outer;
            areaSeries.DataLabelSettings = cartesianDataLabelSettings;
            var expectedResult = new PointF(0, 361.44f); 
            PointF result = cartesianDataLabelSettings.GetLabelPositionForAreaSeries(areaSeries, areaSegment, labelSize, labelPoint, padding);
            Assert.Equal(expectedResult, result);
        }


        [Fact]
        public void IsTopWithLabelIndex_ShouldReturnTrueForTopValue()
        {
            var areaSeries = new AreaSeries
            {
                YValues = new List<double> { 10, 14, 30, 10, 40, 10, 50 }
            };
            int index = 2; 
            areaSeries.ShowDataLabels = true;
            var cartesianDataLabelSettings = new CartesianDataLabelSettings();
            areaSeries.DataLabelSettings = cartesianDataLabelSettings;
            var result = (bool?)InvokePrivateMethod(cartesianDataLabelSettings, "IsTopWithLabelIndex", new object[] { areaSeries, index });
            Assert.True(result); 
        }

        [Fact]
        public void IsTopWithLabelIndex_FirstIndex_ReturnsTrue()
        {
            var areaSeries = new AreaSeries
            {
                YValues = new List<double> { 30, 20, 10 }
            };
            areaSeries.ShowDataLabels = true;
            int index = 0; 
            var cartesianDataLabelSettings = new CartesianDataLabelSettings();
            areaSeries.DataLabelSettings = cartesianDataLabelSettings;
            var result = (bool?)InvokePrivateMethod(cartesianDataLabelSettings, "IsTopWithLabelIndex", new object[] { areaSeries, index });
            Assert.True(result); 
        }

        [Fact]
        public void IsTopWithLabelIndex_LastIndex_ReturnsTrue()
        {
            var areaSeries = new AreaSeries
            {
                YValues = new List<double> { 10, 20, 30 }
            };
            areaSeries.ShowDataLabels = true;
            int index = 2; 
            var cartesianDataLabelSettings = new CartesianDataLabelSettings();
            areaSeries.DataLabelSettings = cartesianDataLabelSettings;
            var result = (bool?)InvokePrivateMethod(cartesianDataLabelSettings, "IsTopWithLabelIndex", new object[] { areaSeries, index });
            Assert.True(result);
        }

        [Fact]
        public void IsTopWithLabelIndex_NaNPreviousValue_ReturnsTrue()
        {
            var areaSeries = new AreaSeries
            {
                YValues = new List<double> { double.NaN, 20, 30 }
            };
            areaSeries.ShowDataLabels = true;
            int index = 1;  
            var cartesianDataLabelSettings = new CartesianDataLabelSettings();
            areaSeries.DataLabelSettings = cartesianDataLabelSettings;
            var result = (bool?)InvokePrivateMethod(cartesianDataLabelSettings, "IsTopWithLabelIndex", new object[] { areaSeries, index });
            Assert.False(result);  
        }

        [Fact]
        public void IsTopWithLabelIndex_AllNaNValues_ReturnsTrue()
        {
            var areaSeries = new AreaSeries
            {
                YValues = new List<double> { double.NaN, double.NaN, double.NaN }
            };
            areaSeries.ShowDataLabels = true;
            int index = 1;
            var cartesianDataLabelSettings = new CartesianDataLabelSettings();
            areaSeries.DataLabelSettings = cartesianDataLabelSettings;
            var result = (bool?)InvokePrivateMethod(cartesianDataLabelSettings, "IsTopWithLabelIndex", new object[] { areaSeries, index });
            Assert.True(result); 
        }

        [Fact]
        public void GetLabelPositionForRangeSeries()
        {
            var sfCartesianChart = new SfCartesianChart();
            sfCartesianChart.ChartArea.ActualSeriesClipRect = new Rect(30, 1, 1007.599975, 471.79998);
            sfCartesianChart.SeriesBounds = new Rect(30, 1, 1007.599975, 471.79998);
            RangeAreaSeries RangeAreaSeries = new RangeAreaSeries()
            {
                ActualYAxis = new NumericalAxis(),
                ActualXAxis = new NumericalAxis(),
                HighValues = new List<double> { 10, 14, 30, 10, 40, 10, 50 },
                LowValues = new List<double> { 40,40,40,40,40,40,40,40 },

            };
             
            sfCartesianChart.Series.Add(RangeAreaSeries);
            var cartesianDataLabelSettings = new CartesianDataLabelSettings();
            RangeAreaSeries.DataLabelSettings = cartesianDataLabelSettings;
            var expectedResult = new PointF(14.5f,455.8f); 
            SizeF labelSize = new SizeF(23, 26);
            PointF labelPoint = new PointF( 0,  471.8f);
            float padding =3;
            string valueType = "HighType"; 
            var methodInfo = typeof(CartesianDataLabelSettings).GetMethod("GetLabelPositionForRangeSeries", BindingFlags.NonPublic | BindingFlags.Instance);
            var result = (PointF?)methodInfo?.Invoke(cartesianDataLabelSettings, new object[] { RangeAreaSeries, 0, labelSize, labelPoint, padding, valueType });
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void GetLabelPositionForCandleSeries()
        {
            var sfCartesianChart = new SfCartesianChart();
            sfCartesianChart.ChartArea.ActualSeriesClipRect = new Rect(30, 1, 1007.599975, 471.79998);
            sfCartesianChart.SeriesBounds = new Rect(30, 1, 1007.599975, 471.79998);
            CandleSeries candleSeries = new CandleSeries()
            {
                ActualYAxis = new NumericalAxis(),
                ActualXAxis = new NumericalAxis(),
                HighValues = new List<double> { 10, 14, 30, 10, 40, 10, 50 },
                LowValues = new List<double> { 30,30,30,30,30,30,30 },
                CloseValues = new List<double> { 40, 40, 40, 40, 40, 40, 40, 40 },
                OpenValues = new List<double> { 50,50,50,50,50,50,50},

            };
             
            sfCartesianChart.Series.Add(candleSeries);
            var cartesianDataLabelSettings = new CartesianDataLabelSettings();
            candleSeries.DataLabelSettings = cartesianDataLabelSettings;
            SizeF labelSize = new SizeF(23, 26);
            PointF labelPoint = new PointF(71.97143f, 0f);
            float padding = 3;
            string valueType = "HighType";
            var expectedResult = new PointF(71.97143f,  16);
            var methodInfo = typeof(CartesianDataLabelSettings).GetMethod("GetLabelPositionForCandleSeries", BindingFlags.NonPublic | BindingFlags.Instance);
            var result = (PointF?)methodInfo?.Invoke(cartesianDataLabelSettings, new object[] { candleSeries, 0, labelSize, labelPoint, padding, valueType });
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void UpdateHighTypePosition()
        {
            var sfCartesianChart = new SfCartesianChart();

            sfCartesianChart.ChartArea.ActualSeriesClipRect = new Rect(30, 1, 1007.599975, 471.79998);
            sfCartesianChart.SeriesBounds = new Rect(30, 1, 1007.599975, 471.79998);
            CandleSeries candleSeries = new CandleSeries()
            {
                ActualYAxis = new NumericalAxis(),
                ActualXAxis = new NumericalAxis(),
                HighValues = new List<double> { 10, 14, 30, 10, 40, 10, 50 },
                LowValues = new List<double> { 30, 30, 30, 30, 30, 30, 30 },
                CloseValues = new List<double> { 40, 40, 40, 40, 40, 40, 40, 40 },
                OpenValues = new List<double> { 50, 50, 50, 50, 50, 50, 50 },

            };
            var cartesianDataLabelSettings = new CartesianDataLabelSettings();
            candleSeries.DataLabelSettings = cartesianDataLabelSettings;
            cartesianDataLabelSettings.LabelPlacement = DataLabelPlacement.Inner;
            sfCartesianChart.Series.Add(candleSeries);
            float x = 71.97143f, y =  0f; 
            SizeF labelSize = new SizeF(23, 26); 
            double padding = 3; 
            double highValue = 10; 
            double lowValue = 30; 
            double expectedX = 71.97143f;
            double expectedY = -16;
            var methodInfo = typeof(CartesianDataLabelSettings).GetMethod("UpdateHighTypePosition", BindingFlags.NonPublic | BindingFlags.Instance);
            object[] parameters = new object[] { highValue, lowValue, x, y, labelSize, padding, 0, candleSeries };
            methodInfo?.Invoke(cartesianDataLabelSettings, parameters);
            x = (float)parameters[2];
            y = (float)parameters[3];
            Assert.Equal(expectedX, x);
            Assert.Equal(expectedY, y); 
        }

        [Fact]
        public void UpdateLowTypePosition()
        {
            var sfCartesianChart = new SfCartesianChart();
            sfCartesianChart.ChartArea.ActualSeriesClipRect = new Rect(30, 1, 1007.599975, 471.79998);
            sfCartesianChart.SeriesBounds = new Rect(30, 1, 1007.599975, 471.79998);
            CandleSeries candleSeries = new CandleSeries()
            {
                ActualYAxis = new NumericalAxis(),
                ActualXAxis = new NumericalAxis(),
                HighValues = new List<double> { 10, 14, 30, 10, 40, 10, 50 },
                LowValues = new List<double> { 30, 30, 30, 30, 30, 30, 30 },
                CloseValues = new List<double> { 40, 40, 40, 40, 40, 40, 40, 40 },
                OpenValues = new List<double> { 50, 50, 50, 50, 50, 50, 50 },

            };
            var cartesianDataLabelSettings = new CartesianDataLabelSettings();
            candleSeries.DataLabelSettings = cartesianDataLabelSettings;
            cartesianDataLabelSettings.LabelPlacement = DataLabelPlacement.Inner;
            sfCartesianChart.Series.Add(candleSeries);
            float x = 71.97143f, y = 471.8f; 
            SizeF labelSize = new SizeF(23, 26); 
            double padding = 3; 
            double highValue = 10; 
            double lowValue = 30; 
            double expectedX = 71.97143f;
            double expectedY = 487.79998779296875;
            var methodInfo = typeof(CartesianDataLabelSettings).GetMethod("UpdateLowTypePosition", BindingFlags.NonPublic | BindingFlags.Instance);
            object[] parameters = new object[] { highValue, lowValue, x, y, labelSize, padding, 0, candleSeries };
            methodInfo?.Invoke(cartesianDataLabelSettings, parameters);
            x = (float)parameters[2];
            y = (float)parameters[3];
            Assert.Equal(expectedX, x);
            Assert.Equal(expectedY, y); 
        }

        [Fact]
        public void UpdateOpenTypePosition()
        {
            var sfCartesianChart = new SfCartesianChart();
            sfCartesianChart.ChartArea.ActualSeriesClipRect = new Rect(30, 1, 1007.599975, 471.79998);
            sfCartesianChart.SeriesBounds = new Rect(30, 1, 1007.599975, 471.79998);
            CandleSeries candleSeries = new CandleSeries()
            {
                ActualYAxis = new NumericalAxis(),
                ActualXAxis = new NumericalAxis(),
                HighValues = new List<double> { 10, 14, 30, 10, 40, 10, 50 },
                LowValues = new List<double> { 30, 30, 30, 30, 30, 30, 30 },
                CloseValues = new List<double> { 40, 40, 40, 40, 40, 40, 40, 40 },
                OpenValues = new List<double> { 50, 50, 50, 50, 50, 50, 50 },

            };
            var cartesianDataLabelSettings = new CartesianDataLabelSettings();
            candleSeries.DataLabelSettings = cartesianDataLabelSettings;
            cartesianDataLabelSettings.LabelPlacement = DataLabelPlacement.Inner;
            sfCartesianChart.Series.Add(candleSeries);
            float x = 71.97143f, y = 117.95f; 
            SizeF labelSize = new SizeF(23, 26); 
            double padding = 3; 
            double openValue = 40; 
            double closeValue = 50;
            double expectedX = 71.97143f;
            double expectedY = 101.94999694824219;
            var methodInfo = typeof(CartesianDataLabelSettings).GetMethod("UpdateOpenTypePosition", BindingFlags.NonPublic | BindingFlags.Instance);
            object[] parameters = new object[] { openValue, closeValue, x, y, labelSize, padding, 0, candleSeries };
            methodInfo?.Invoke(cartesianDataLabelSettings, parameters);
            x = (float)parameters[2];
            y = (float)parameters[3];
            Assert.Equal(expectedX, x); 
            Assert.Equal(expectedY, y); 
        }

        [Fact]
        public void UpdateCloseTypePosition()
        {
            var sfCartesianChart = new SfCartesianChart();
            sfCartesianChart.ChartArea.ActualSeriesClipRect = new Rect(30, 1, 1007.599975, 471.79998);
            sfCartesianChart.SeriesBounds = new Rect(30, 1, 1007.599975, 471.79998);
            CandleSeries candleSeries = new CandleSeries()
            {
                ActualYAxis = new NumericalAxis(),
                ActualXAxis = new NumericalAxis(),
                HighValues = new List<double> { 10, 14, 30, 10, 40, 10, 50 },
                LowValues = new List<double> { 30, 30, 30, 30, 30, 30, 30 },
                CloseValues = new List<double> { 40, 40, 40, 40, 40, 40, 40, 40 },
                OpenValues = new List<double> { 50, 50, 50, 50, 50, 50, 50 },

            };
            var cartesianDataLabelSettings = new CartesianDataLabelSettings();
            candleSeries.DataLabelSettings = cartesianDataLabelSettings;
            cartesianDataLabelSettings.LabelPlacement = DataLabelPlacement.Inner;
            sfCartesianChart.Series.Add(candleSeries);
            float x = 71.97143f, y = 0f;
            SizeF labelSize = new SizeF(23, 26);
            double padding = 3;
            double openValue = 40;
            double closeValue = 50; 
            double expectedX = 71.97143f;
            double expectedY = 16;
            var methodInfo = typeof(CartesianDataLabelSettings).GetMethod("UpdateCloseTypePosition", BindingFlags.NonPublic | BindingFlags.Instance);
            object[] parameters = new object[] { openValue, closeValue, x, y, labelSize, padding, 0, candleSeries };
            methodInfo?.Invoke(cartesianDataLabelSettings, parameters);
            x = (float)parameters[2];
            y = (float)parameters[3];
            Assert.Equal(expectedX, x);
            Assert.Equal(expectedY, y);
        }

        [Fact]
        public void GetContrastTextColor_BackgroundSolidColor_ReturnsContrastColor()
        {
            var lineSeries = new LineSeries();
            var background = new SolidColorBrush(Colors.Blue);
            var segmentBrush = new SolidColorBrush(Colors.Red);
            var dataLabelSetting = new CartesianDataLabelSettings();
            var expectedColor = ChartUtils.GetContrastColor(Colors.Blue);
            var result = dataLabelSetting.GetContrastTextColor(lineSeries, background, segmentBrush);
            Assert.Equal(expectedColor, result);
        }
 
        [Fact]
        public void GetContrastTextColor_BackgroundTransparent_ReturnsDefaultColor()
        {
            var lineSeries = new LineSeries();
            var background = new SolidColorBrush(Colors.Transparent);
            var segmentBrush = new SolidColorBrush(Colors.Red);
            var dataLabelSetting = new CartesianDataLabelSettings();
            var expectedColor = Colors.Black;
            var result = dataLabelSetting.GetContrastTextColor(lineSeries, background, segmentBrush);
            Assert.Equal(expectedColor, result);
        }

        [Theory]
        [InlineData(123.456, "$123.46", "C")]
        [InlineData(123.456, "123.46", "")] 
        public void GetLabelContent_VariousInputs_ReturnsExpectedContent(double value, string expectedContent, string format)
        {
            var cartesianDataLabelSettings = new CartesianDataLabelSettings();
            cartesianDataLabelSettings.LabelStyle = new ChartDataLabelStyle { LabelFormat=  format };
            var result = cartesianDataLabelSettings.GetLabelContent(value);
            Assert.Equal(expectedContent, result);
        }

        [Fact]
        public void GetDefaultTextColor_ReturnsExpectedColor()
        {
            var chartAnnotationLabelStyle = new ChartAnnotationLabelStyle(); 
            var expectedColor = Microsoft.Maui.Graphics.Color.FromArgb("#49454F");
            var result = chartAnnotationLabelStyle.GetDefaultTextColor();
            Assert.Equal(expectedColor, result);
        }

        [Fact]
        public void GetDefaultBackgroundColor_ReturnsTransparentBrush()
        {
            var chartAnnotationLabelStyle = new ChartAnnotationLabelStyle(); 
            var expectedBrush = new SolidColorBrush(Colors.Transparent);
            var result = chartAnnotationLabelStyle.GetDefaultBackgroundColor();
            Assert.Equal(expectedBrush.Color, ((SolidColorBrush)result).Color);
        }
    }
}
