using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.Toolkit.UnitTest
{
    public class ChartAxisUnitTests
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void IsVisible_SetValue_ReturnsExpectedValue(bool isVisible)
        {
            NumericalAxis numericalAxis = new NumericalAxis();
            numericalAxis.IsVisible = isVisible;

            Assert.Equal(isVisible, numericalAxis.IsVisible);
        }

        [Theory]
        [InlineData(3)]
        [InlineData(6)]
        [InlineData(-2)]
        public void AutoScrollingDelta_SetValue_ReturnsExpectedValue(double autoScrollingDelta)
        {
            NumericalAxis numericalAxis = new NumericalAxis();
            numericalAxis.AutoScrollingDelta = autoScrollingDelta;

            Assert.Equal(autoScrollingDelta, numericalAxis.AutoScrollingDelta);
        }

        [Theory]
        [InlineData(ChartAutoScrollingMode.Start)]
        [InlineData(ChartAutoScrollingMode.End)]
        public void AutoScrollingDeltaMode_SetValue_ReturnsExpectedValue(ChartAutoScrollingMode autoScrollingMode)
        {
            NumericalAxis numericalAxis = new NumericalAxis();
            numericalAxis.AutoScrollingMode = autoScrollingMode;

            Assert.Equal(autoScrollingMode, numericalAxis.AutoScrollingMode);
        }

        [Theory]
        [InlineData(3)]
        [InlineData(-1)]
        [InlineData(double.MaxValue)]
        [InlineData(double.MinValue)]
        [InlineData(double.NaN)]
        public void AxisLineOffSet_SetValue_ReturnsExpectedValue(double axisLineOffset)
        {
            NumericalAxis numericalAxis = new NumericalAxis();
            numericalAxis.AxisLineOffset = axisLineOffset;

            Assert.Equal(axisLineOffset, numericalAxis.AxisLineOffset);
        }

        [Fact]
        public void AxisLineStyle_SetValue_ReturnsExpectedValue()
        {
            NumericalAxis numericalAxis = new NumericalAxis();
            var style = new ChartLineStyle() { StrokeWidth = 2, Stroke = Colors.Red };
            numericalAxis.AxisLineStyle = style;

            Assert.Equal(Colors.Red, numericalAxis.AxisLineStyle.Stroke);
        }

        [Fact]
        public void CrossAxisName_SetValue_ReturnsExpectedValue()
        {
            NumericalAxis numericalAxis = new NumericalAxis();
            string crossAxisName = "secondaryAxis";
            numericalAxis.CrossAxisName = crossAxisName;

            Assert.Equal(crossAxisName, numericalAxis.CrossAxisName);
        }

        [Theory]
        [InlineData(double.MaxValue)]
        [InlineData(double.MinValue)]
        [InlineData(double.NaN)]
        [InlineData(10)]
        public void CrossesAt_SetValue_ReturnsExpectedValue(double crossesAt)
        {
            NumericalAxis numericalAxis = new NumericalAxis();
            numericalAxis.CrossesAt = crossesAt;

            Assert.Equal(crossesAt, numericalAxis.CrossesAt);
        }

        [Theory]
        [InlineData(EdgeLabelsDrawingMode.Center)]
        [InlineData(EdgeLabelsDrawingMode.Shift)]
        [InlineData(EdgeLabelsDrawingMode.Fit)]
        [InlineData(EdgeLabelsDrawingMode.Hide)]
        public void EdgeLabelDrawingMode_SetValue_ReturnsExpectedValue(EdgeLabelsDrawingMode edgeLabelsDrawingMode)
        {
            NumericalAxis numericalAxis = new NumericalAxis();
            numericalAxis.EdgeLabelsDrawingMode = edgeLabelsDrawingMode;

            Assert.Equal(edgeLabelsDrawingMode, numericalAxis.EdgeLabelsDrawingMode);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void AutoIntervalOnZooming_SetValue_ReturnsExpectedValue(bool enableAutoIntervalOnZooming)
        {
            NumericalAxis numericalAxis = new NumericalAxis();
            numericalAxis.EnableAutoIntervalOnZooming = enableAutoIntervalOnZooming;

            Assert.Equal(enableAutoIntervalOnZooming, numericalAxis.EnableAutoIntervalOnZooming);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void IsInversed_SetValue_ReturnsExpectedValue(bool isInversed)
        {
            NumericalAxis numericalAxis = new NumericalAxis();
            numericalAxis.IsInversed = isInversed;

            Assert.Equal(isInversed, numericalAxis.IsInversed);
        }

        [Theory]
        [InlineData(3)]
        [InlineData(-1)]
        [InlineData(double.MaxValue)]
        [InlineData(double.MinValue)]
        [InlineData(double.NaN)]
        public void AxisLabelExtent_SetValue_ReturnsExpectedValue(double labelExtent)
        {
            NumericalAxis numericalAxis = new NumericalAxis();
            numericalAxis.LabelExtent = labelExtent;

            Assert.Equal(labelExtent, numericalAxis.LabelExtent);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(65)]
        [InlineData(280)]
        [InlineData(double.NaN)]
        public void AxisLabelRotation_SetValue_ReturnsExpectedValue(double labelRotation)
        {
            NumericalAxis numericalAxis = new NumericalAxis();
            numericalAxis.LabelRotation = labelRotation;

            Assert.Equal(labelRotation, numericalAxis.LabelRotation);
        }

        [Theory]
        [InlineData(AxisLabelsIntersectAction.Hide)]
        [InlineData(AxisLabelsIntersectAction.Wrap)]
        [InlineData(AxisLabelsIntersectAction.MultipleRows)]
        public void LabelsIntersectAction_SetValue_ReturnsExpectedValue(AxisLabelsIntersectAction action)
        {
            CategoryAxis categoryAxis = new CategoryAxis();
            NumericalAxis numericalAxis = new NumericalAxis();
            categoryAxis.LabelsIntersectAction = action;
            Assert.Equal(action, categoryAxis.LabelsIntersectAction);

            numericalAxis.LabelsIntersectAction = action;
            Assert.Equal(action, numericalAxis.LabelsIntersectAction);
        }

        [Theory]
        [InlineData(AxisElementPosition.Outside)]
        [InlineData(AxisElementPosition.Inside)]
        public void LabelsPosition_SetValue_ReturnsExpectedValue(AxisElementPosition labelPosition)
        {
            CategoryAxis categoryAxis = new CategoryAxis();
            categoryAxis.LabelsPosition = labelPosition;

            Assert.Equal(labelPosition, categoryAxis.LabelsPosition);
        }

        [Fact]
        public void LabelStyle_SetValue_ReturnsExpectedValue()
        {
            CategoryAxis categoryAxis = new CategoryAxis();
            var labelStyle = new ChartAxisLabelStyle
            {
                TextColor = Colors.Red,
                FontSize = 14
            };
            categoryAxis.LabelStyle = labelStyle;

            Assert.Equal(Colors.Red, categoryAxis.LabelStyle.TextColor);
            Assert.Equal(14, categoryAxis.LabelStyle.FontSize);
        }

        [Fact]
        public void MajorGridLineStyle_SetValue_ReturnsExpectedValue()
        {
            NumericalAxis numericalAxis = new NumericalAxis();
            var gridLineStyle = new ChartLineStyle
            {
                Stroke = Colors.Black,
                StrokeWidth = 2,
                StrokeDashArray = new DoubleCollection { 3, 3 }
            };
            numericalAxis.MajorGridLineStyle = gridLineStyle;

            Assert.Equal(Colors.Black, numericalAxis.MajorGridLineStyle.Stroke);
            Assert.Equal(2, numericalAxis.MajorGridLineStyle.StrokeWidth);
            Assert.Equal(3, numericalAxis.MajorGridLineStyle.StrokeDashArray[0]);
            Assert.Equal(3, numericalAxis.MajorGridLineStyle.StrokeDashArray[1]);
        }

        [Fact]
        public void MajorTickStyle_SetValue_ReturnsExpectedValue()
        {
            NumericalAxis numericalAxis = new NumericalAxis();
            var tickStyle = new ChartAxisTickStyle
            {
                Stroke = Colors.Red,
                StrokeWidth = 1
            };
            numericalAxis.MajorTickStyle = tickStyle;

            Assert.Equal(Colors.Red, numericalAxis.MajorTickStyle.Stroke);
            Assert.Equal(1, numericalAxis.MajorTickStyle.StrokeWidth);
        }

        [Fact]
        public void Name_SetValue_ReturnsExpectedValue()
        {
            NumericalAxis numericalAxis = new NumericalAxis();
            numericalAxis.Name = "PrimaryAxis";

            Assert.Equal("PrimaryAxis", numericalAxis.Name);
        }

        [Fact]
        public void PlotOffsetEnd_SetValue_ReturnsExpectedValue()
        {
            CategoryAxis categoryAxis = new CategoryAxis();
            categoryAxis.PlotOffsetEnd = 30;

            Assert.Equal(30, categoryAxis.PlotOffsetEnd);
        }

        [Fact]
        public void PlotOffsetStart_SetValue_ReturnsExpectedValue()
        {
            CategoryAxis categoryAxis = new CategoryAxis();
            categoryAxis.PlotOffsetStart = 30;

            Assert.Equal(30, categoryAxis.PlotOffsetStart);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void RenderNextToCrossingValue_SetValue_ReturnsExpectedValue(bool renderNextToCrossingValue)
        {
            NumericalAxis numericalAxis = new NumericalAxis();
            numericalAxis.RenderNextToCrossingValue = renderNextToCrossingValue;

            Assert.Equal(renderNextToCrossingValue, numericalAxis.RenderNextToCrossingValue);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ShowMajorGridLines_SetValue_ReturnsExpectedValue(bool showMajorGridLines)
        {
            NumericalAxis numericalAxis = new NumericalAxis();
            numericalAxis.ShowMajorGridLines = showMajorGridLines;

            Assert.Equal(showMajorGridLines, numericalAxis.ShowMajorGridLines);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ShowTrackballLabel_SetValue_ReturnsExpectedValue(bool showTrackballLabel)
        {
            NumericalAxis numericalAxis = new NumericalAxis();
            numericalAxis.ShowTrackballLabel = showTrackballLabel;

            Assert.Equal(showTrackballLabel, numericalAxis.ShowTrackballLabel);
        }

        [Theory]
        [InlineData(AxisElementPosition.Outside)]
        [InlineData(AxisElementPosition.Inside)]
        public void TickPosition_SetValue_ReturnsExpectedValue(AxisElementPosition tickPosition)
        {
            NumericalAxis numericalAxis = new NumericalAxis();
            numericalAxis.TickPosition = tickPosition;

            Assert.Equal(tickPosition, numericalAxis.TickPosition);
        }

        [Fact]
        public void AxisTitle_SetValue_ReturnsExpectedValue()
        {
            CategoryAxis categoryAxis = new CategoryAxis();
            var axisTitle = new ChartAxisTitle() { Text = "Category" };
            categoryAxis.Title = axisTitle;

            Assert.NotNull(categoryAxis.Title);
            Assert.Equal("Category", categoryAxis.Title.Text);
        }

        [Fact]
        public void TrackballLabelStyle_SetValue_ReturnsExpectedValue()
        {
            NumericalAxis numericalAxis = new NumericalAxis();
            var trackballLabelStyle = new ChartAxisLabelStyle
            {
                TextColor = Colors.Blue,
                FontSize = 12
            };
            numericalAxis.TrackballLabelStyle = trackballLabelStyle;

            Assert.Equal(Colors.Blue, numericalAxis.TrackballLabelStyle.TextColor);
            Assert.Equal(12, numericalAxis.TrackballLabelStyle.FontSize);
        }

        [Fact]
        public void TrackballLabelTemplate_SetValue_ReturnsExpectedValue()
        {
            NumericalAxis numericalAxis = new NumericalAxis();
            var trackballLabelTemplate = new DataTemplate(() => new Label { Text = "Trackball Label" });
            numericalAxis.TrackballLabelTemplate = trackballLabelTemplate;

            Assert.Equal(trackballLabelTemplate, numericalAxis.TrackballLabelTemplate);
        }

        //[Fact]
        //public void VisibleLabels_SetValue_ReturnsExpectedValue()
        //{
        //    NumericalAxis numericalAxis = new NumericalAxis();
        //    numericalAxis.Minimum = 0;
        //    numericalAxis.Maximum = 100;
        //    numericalAxis.Interval = 20;

        //    Assert.NotNull(numericalAxis.VisibleLabels);
        //    Assert.Equal("100", numericalAxis.VisibleLabels[4].Content.ToString());
        //}

        [Theory]
        [InlineData(100)]
        [InlineData(double.MaxValue)]
        [InlineData(double.MinValue)]
        public void VisibleMaximum_SetValue_ReturnsExpectedValue(double visibleMaximum)
        {
            NumericalAxis numericalAxis = new NumericalAxis();
            numericalAxis.Maximum = visibleMaximum;

            Assert.Equal(visibleMaximum, numericalAxis.Maximum);
        }

        [Fact]
        public void VisibleMinimum_SetValue_ReturnsExpectedValue_Get()
        {
            NumericalAxis numericalAxis = new NumericalAxis();
            var minimum = 10d;
            numericalAxis.Minimum = minimum;

            Assert.Equal(10, numericalAxis.Minimum);
        }

        [Theory]
        [InlineData(0.5)]
        [InlineData(1.0)]
        [InlineData(0.0)]
        public void ZoomFactor_SetValue_ReturnsExpectedValue(double zoomFactor)
        {
            NumericalAxis numericalAxis = new NumericalAxis();
            numericalAxis.ZoomFactor = zoomFactor;

            Assert.Equal(zoomFactor, numericalAxis.ZoomFactor);
        }
        [Theory]
        [InlineData(0.5)]
        [InlineData(1.0)]
        [InlineData(0.0)]
        public void ZoomPosition_SetValue_ReturnsExpectedValue(double zoomPosition)
        {
            NumericalAxis numericalAxis = new NumericalAxis();
            numericalAxis.ZoomPosition = zoomPosition;

            Assert.Equal(zoomPosition, numericalAxis.ZoomPosition);
        }

        // CategoryAxis

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ArrangeByIndex_SetValue_ReturnsExpectedValue(bool arrangeByIndex)
        {
            CategoryAxis categoryAxis = new CategoryAxis();
            categoryAxis.ArrangeByIndex = arrangeByIndex;

            Assert.Equal(arrangeByIndex, categoryAxis.ArrangeByIndex);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(20)]
        [InlineData(50)]
        public void Interval_SetValue_ReturnsExpectedValue(double interval)
        {
            NumericalAxis numericalAxis = new NumericalAxis();
            numericalAxis.Interval = interval;

            Assert.Equal(interval, numericalAxis.Interval);
        }

        [Theory]
        [InlineData(LabelPlacement.BetweenTicks)]
        [InlineData(LabelPlacement.OnTicks)]
        public void LabelPlacement_SetValue_ReturnsExpectedValue(LabelPlacement labelPlacement)
        {
            CategoryAxis categoryAxis = new CategoryAxis();
            categoryAxis.LabelPlacement = labelPlacement;

            Assert.Equal(labelPlacement, categoryAxis.LabelPlacement);
        }

        [Fact]
        public void PlotBands_SetValue_ReturnsExpectedValue()
        {
            NumericalAxis numericalAxis = new NumericalAxis();
            var plotBand1 = new NumericalPlotBand() { Start = 20, End = 40, Fill = Colors.Bisque };
            numericalAxis.PlotBands = new NumericalPlotBandCollection
            {
                plotBand1,
            };

            Assert.NotNull(numericalAxis.PlotBands);
            Assert.Single(numericalAxis.PlotBands);
            Assert.Contains(numericalAxis.PlotBands, band => band.Start == 20 && band.End == 40);
        }

        // Range axis 

        [Theory]
        [InlineData(EdgeLabelsVisibilityMode.Default)]
        [InlineData(EdgeLabelsVisibilityMode.AlwaysVisible)]
        [InlineData(EdgeLabelsVisibilityMode.Visible)]
        public void EdgeLabelsVisibilityMode_SetValue_ReturnsExpectedValue(EdgeLabelsVisibilityMode visibilityMode)
        {
            NumericalAxis numericalAxis = new NumericalAxis();
            numericalAxis.EdgeLabelsVisibilityMode = visibilityMode;

            Assert.Equal(visibilityMode, numericalAxis.EdgeLabelsVisibilityMode);
        }
        [Fact]
        public void MinorGridLineStyle_SetValue_ReturnsExpectedValue()
        {
            NumericalAxis numericalAxis = new NumericalAxis();
            var minorGridLineStyle = new ChartLineStyle
            {
                Stroke = Colors.Gray,
                StrokeWidth = 1,
                StrokeDashArray = new DoubleCollection { 2, 2 }
            };
            numericalAxis.MinorGridLineStyle = minorGridLineStyle;

            Assert.Equal(Colors.Gray, numericalAxis.MinorGridLineStyle.Stroke);
            Assert.Equal(1, numericalAxis.MinorGridLineStyle.StrokeWidth);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        public void MinorTicksPerInterval_SetValue_ReturnsExpectedValue(int minorTicksPerInterval)
        {
            NumericalAxis numericalAxis = new NumericalAxis();
            numericalAxis.MinorTicksPerInterval = minorTicksPerInterval;

            Assert.Equal(minorTicksPerInterval, numericalAxis.MinorTicksPerInterval);
        }

        [Fact]
        public void MinorTickStyle_SetValue_ReturnsExpectedValue()
        {
            NumericalAxis numericalAxis = new NumericalAxis();
            var minorTickStyle = new ChartAxisTickStyle
            {
                Stroke = Colors.Blue,
                StrokeWidth = 0.5
            };
            numericalAxis.MinorTickStyle = minorTickStyle;

            Assert.Equal(Colors.Blue, numericalAxis.MinorTickStyle.Stroke);
            Assert.Equal(0.5, numericalAxis.MinorTickStyle.StrokeWidth);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ShowMinorGridLines_SetValue_ReturnsExpectedValue(bool showMinorGridLines)
        {
            NumericalAxis numericalAxis = new NumericalAxis();
            numericalAxis.ShowMinorGridLines = showMinorGridLines;

            Assert.Equal(showMinorGridLines, numericalAxis.ShowMinorGridLines);
        }

        // Numerical Axis

        [Fact]
        public void NumericalAxis_ActualMaximum_SetValue_ReturnsExpectedValue()
        {
            NumericalAxis numericalAxis = new NumericalAxis();
            numericalAxis.ActualMaximum = 100;

            Assert.Equal(100, numericalAxis.ActualMaximum);
        }

        [Fact]
        public void NumericalAxis_ActualMinimum_SetValue_ReturnsExpectedValue()
        {
            NumericalAxis numericalAxis = new NumericalAxis();
            numericalAxis.ActualMinimum = 10;

            Assert.Equal(10, numericalAxis.ActualMinimum);
        }

        [Theory]
        [InlineData(100)]
        [InlineData(double.NaN)]
        public void NumericalAxis_Maximum_SetValue_ReturnsExpectedValue(double maximum)
        {
            NumericalAxis numericalAxis = new NumericalAxis();
            numericalAxis.Maximum = maximum;

            Assert.Equal(maximum, numericalAxis.Maximum);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(double.NaN)]
        public void NumericalAxis_Minimum_SetValue_ReturnsExpectedValue(double minimum)
        {
            NumericalAxis numericalAxis = new NumericalAxis();
            numericalAxis.Minimum = minimum;

            Assert.Equal(minimum, numericalAxis.Minimum);
        }

        [Theory]
        [InlineData(NumericalPadding.None)]
        [InlineData(NumericalPadding.Round)]
        [InlineData(NumericalPadding.Additional)]
        public void NumericalAxis_RangePadding_SetValue_ReturnsExpectedValue(NumericalPadding rangePadding)
        {
            NumericalAxis numericalAxis = new NumericalAxis();
            numericalAxis.RangePadding = rangePadding;

            Assert.Equal(rangePadding, numericalAxis.RangePadding);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(10)]
        [InlineData(100)]
        public void LogarithmicBase_SetValue_ReturnsExpectedValue(double logarithmicBase)
        {
            LogarithmicAxis logarithmicAxis = new LogarithmicAxis();
            logarithmicAxis.LogarithmicBase = logarithmicBase;

            Assert.Equal(logarithmicBase, logarithmicAxis.LogarithmicBase);
        }

        // DateTimeAxis

        [Fact]
        public void DateTimeAxis_ActualMaximum_SetValue_ReturnsExpectedValue()
        {
            DateTimeAxis dateTimeAxis = new DateTimeAxis();
            dateTimeAxis.ActualMaximum = new DateTime(2023, 12, 31);

            Assert.Equal(new DateTime(2023, 12, 31), dateTimeAxis.ActualMaximum);
        }

        [Fact]
        public void DateTimeAxis_ActualMinimum_SetValue_ReturnsExpectedValue()
        {
            DateTimeAxis dateTimeAxis = new DateTimeAxis();
            dateTimeAxis.ActualMinimum = new DateTime(2023, 1, 1);

            Assert.Equal(new DateTime(2023, 1, 1), dateTimeAxis.ActualMinimum);
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
        public void AutoScrollingDeltaType_SetValue_ReturnsExpectedValue(DateTimeIntervalType intervalType)
        {
            DateTimeAxis dateTimeAxis = new DateTimeAxis();
            dateTimeAxis.AutoScrollingDeltaType = intervalType;

            Assert.Equal(intervalType, dateTimeAxis.AutoScrollingDeltaType);
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
        public void IntervalType_SetValue_ReturnsExpectedValue(DateTimeIntervalType intervalType)
        {
            DateTimeAxis dateTimeAxis = new DateTimeAxis();
            dateTimeAxis.IntervalType = intervalType;

            Assert.Equal(intervalType, dateTimeAxis.IntervalType);
        }

        [Theory]
        [InlineData("2023-12-31")]
        [InlineData(null)]
        public void DateTimeAxis_Maximum_SetValue_ReturnsExpectedValue(string maximum)
        {
            DateTimeAxis dateTimeAxis = new DateTimeAxis();
            DateTime? maxDate = maximum != null ? DateTime.Parse(maximum) : null;
            dateTimeAxis.Maximum = maxDate;

            Assert.Equal(maxDate, dateTimeAxis.Maximum);
        }

        [Theory]
        [InlineData("2023-01-01")]
        [InlineData(null)]
        public void DateTimeAxis_Minimum_SetValue_ReturnsExpectedValue(string minimum)
        {
            var dateTimeAxis = new DateTimeAxis();
            DateTime? minDate = minimum != null ? DateTime.Parse(minimum) : null;
            dateTimeAxis.Minimum = minDate;

            Assert.Equal(minDate, dateTimeAxis.Minimum);
        }

        [Fact]
        public void DateTimeAxis_PlotBands_SetValue_ReturnsExpectedValue()
        {
            DateTimeAxis dateTimeAxis = new DateTimeAxis();
            var plotBand1 = new DateTimePlotBand() { Start = new DateTime(2023, 1, 1), End = new DateTime(2023, 12, 31), Fill = Colors.Bisque };
            dateTimeAxis.PlotBands = new DateTimePlotBandCollection
            {
                plotBand1,
            };

            Assert.NotNull(dateTimeAxis.PlotBands);
            Assert.Single(dateTimeAxis.PlotBands);
            Assert.Contains(dateTimeAxis.PlotBands, band => band.Start == new DateTime(2023, 1, 1) && band.End == new DateTime(2023, 12, 31));
        }

        [Theory]
        [InlineData(DateTimeRangePadding.None)]
        [InlineData(DateTimeRangePadding.Round)]
        [InlineData(DateTimeRangePadding.Additional)]
        [InlineData(DateTimeRangePadding.Auto)]
        [InlineData(DateTimeRangePadding.RoundStart)]
        [InlineData(DateTimeRangePadding.RoundEnd)]
        [InlineData(DateTimeRangePadding.PrependInterval)]
        [InlineData(DateTimeRangePadding.AppendInterval)]
        public void DateTimeAxis_RangePadding_SetValue_ReturnsExpectedValue(DateTimeRangePadding rangePadding)
        {
            DateTimeAxis dateTimeAxis = new DateTimeAxis();
            dateTimeAxis.RangePadding = rangePadding;

            Assert.Equal(rangePadding, dateTimeAxis.RangePadding);
        }
    }
}