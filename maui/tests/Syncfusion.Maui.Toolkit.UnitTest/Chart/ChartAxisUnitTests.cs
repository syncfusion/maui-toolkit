using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.Toolkit.UnitTest.Charts
{
	public class ChartAxisUnitTests
	{
		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void IsVisible_SetValue_ReturnsExpectedValue(bool isVisible)
		{
			NumericalAxis numericalAxis = new NumericalAxis
			{
				IsVisible = isVisible
			};

			Assert.Equal(isVisible, numericalAxis.IsVisible);
		}

		[Theory]
		[InlineData(3)]
		[InlineData(6)]
		[InlineData(-2)]
		public void AutoScrollingDelta_SetValue_ReturnsExpectedValue(double autoScrollingDelta)
		{
			NumericalAxis numericalAxis = new NumericalAxis
			{
				AutoScrollingDelta = autoScrollingDelta
			};

			Assert.Equal(autoScrollingDelta, numericalAxis.AutoScrollingDelta);
		}

		[Theory]
		[InlineData(ChartAutoScrollingMode.Start)]
		[InlineData(ChartAutoScrollingMode.End)]
		public void AutoScrollingDeltaMode_SetValue_ReturnsExpectedValue(ChartAutoScrollingMode autoScrollingMode)
		{
			NumericalAxis numericalAxis = new NumericalAxis
			{
				AutoScrollingMode = autoScrollingMode
			};

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
			NumericalAxis numericalAxis = new NumericalAxis
			{
				AxisLineOffset = axisLineOffset
			};

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
			NumericalAxis numericalAxis = new NumericalAxis
			{
				CrossesAt = crossesAt
			};

			Assert.Equal(crossesAt, numericalAxis.CrossesAt);
		}

		[Theory]
		[InlineData(EdgeLabelsDrawingMode.Center)]
		[InlineData(EdgeLabelsDrawingMode.Shift)]
		[InlineData(EdgeLabelsDrawingMode.Fit)]
		[InlineData(EdgeLabelsDrawingMode.Hide)]
		public void EdgeLabelDrawingMode_SetValue_ReturnsExpectedValue(EdgeLabelsDrawingMode edgeLabelsDrawingMode)
		{
			NumericalAxis numericalAxis = new NumericalAxis
			{
				EdgeLabelsDrawingMode = edgeLabelsDrawingMode
			};

			Assert.Equal(edgeLabelsDrawingMode, numericalAxis.EdgeLabelsDrawingMode);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void AutoIntervalOnZooming_SetValue_ReturnsExpectedValue(bool enableAutoIntervalOnZooming)
		{
			NumericalAxis numericalAxis = new NumericalAxis
			{
				EnableAutoIntervalOnZooming = enableAutoIntervalOnZooming
			};

			Assert.Equal(enableAutoIntervalOnZooming, numericalAxis.EnableAutoIntervalOnZooming);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void IsInversed_SetValue_ReturnsExpectedValue(bool isInversed)
		{
			NumericalAxis numericalAxis = new NumericalAxis
			{
				IsInversed = isInversed
			};

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
			NumericalAxis numericalAxis = new NumericalAxis
			{
				LabelExtent = labelExtent
			};

			Assert.Equal(labelExtent, numericalAxis.LabelExtent);
		}

		[Theory]
		[InlineData(0)]
		[InlineData(65)]
		[InlineData(280)]
		[InlineData(double.NaN)]
		public void AxisLabelRotation_SetValue_ReturnsExpectedValue(double labelRotation)
		{
			NumericalAxis numericalAxis = new NumericalAxis
			{
				LabelRotation = labelRotation
			};

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
			CategoryAxis categoryAxis = new CategoryAxis
			{
				LabelsPosition = labelPosition
			};

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
				StrokeDashArray = [3, 3]
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
			NumericalAxis numericalAxis = new NumericalAxis
			{
				Name = "PrimaryAxis"
			};

			Assert.Equal("PrimaryAxis", numericalAxis.Name);
		}

		[Fact]
		public void PlotOffsetEnd_SetValue_ReturnsExpectedValue()
		{
			CategoryAxis categoryAxis = new CategoryAxis
			{
				PlotOffsetEnd = 30
			};

			Assert.Equal(30, categoryAxis.PlotOffsetEnd);
		}

		[Fact]
		public void PlotOffsetStart_SetValue_ReturnsExpectedValue()
		{
			CategoryAxis categoryAxis = new CategoryAxis
			{
				PlotOffsetStart = 30
			};

			Assert.Equal(30, categoryAxis.PlotOffsetStart);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void RenderNextToCrossingValue_SetValue_ReturnsExpectedValue(bool renderNextToCrossingValue)
		{
			NumericalAxis numericalAxis = new NumericalAxis
			{
				RenderNextToCrossingValue = renderNextToCrossingValue
			};

			Assert.Equal(renderNextToCrossingValue, numericalAxis.RenderNextToCrossingValue);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void ShowMajorGridLines_SetValue_ReturnsExpectedValue(bool showMajorGridLines)
		{
			NumericalAxis numericalAxis = new NumericalAxis
			{
				ShowMajorGridLines = showMajorGridLines
			};

			Assert.Equal(showMajorGridLines, numericalAxis.ShowMajorGridLines);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void ShowTrackballLabel_SetValue_ReturnsExpectedValue(bool showTrackballLabel)
		{
			NumericalAxis numericalAxis = new NumericalAxis
			{
				ShowTrackballLabel = showTrackballLabel
			};

			Assert.Equal(showTrackballLabel, numericalAxis.ShowTrackballLabel);
		}

		[Theory]
		[InlineData(AxisElementPosition.Outside)]
		[InlineData(AxisElementPosition.Inside)]
		public void TickPosition_SetValue_ReturnsExpectedValue(AxisElementPosition tickPosition)
		{
			NumericalAxis numericalAxis = new NumericalAxis
			{
				TickPosition = tickPosition
			};

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
			NumericalAxis numericalAxis = new NumericalAxis
			{
				Maximum = visibleMaximum
			};

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
			NumericalAxis numericalAxis = new NumericalAxis
			{
				ZoomFactor = zoomFactor
			};

			Assert.Equal(zoomFactor, numericalAxis.ZoomFactor);
		}
		[Theory]
		[InlineData(0.5)]
		[InlineData(1.0)]
		[InlineData(0.0)]
		public void ZoomPosition_SetValue_ReturnsExpectedValue(double zoomPosition)
		{
			NumericalAxis numericalAxis = new NumericalAxis
			{
				ZoomPosition = zoomPosition
			};

			Assert.Equal(zoomPosition, numericalAxis.ZoomPosition);
		}

		// CategoryAxis

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void ArrangeByIndex_SetValue_ReturnsExpectedValue(bool arrangeByIndex)
		{
			CategoryAxis categoryAxis = new CategoryAxis
			{
				ArrangeByIndex = arrangeByIndex
			};

			Assert.Equal(arrangeByIndex, categoryAxis.ArrangeByIndex);
		}

		[Theory]
		[InlineData(10)]
		[InlineData(20)]
		[InlineData(50)]
		public void Interval_SetValue_ReturnsExpectedValue(double interval)
		{
			NumericalAxis numericalAxis = new NumericalAxis
			{
				Interval = interval
			};

			Assert.Equal(interval, numericalAxis.Interval);
		}

		[Theory]
		[InlineData(LabelPlacement.BetweenTicks)]
		[InlineData(LabelPlacement.OnTicks)]
		public void LabelPlacement_SetValue_ReturnsExpectedValue(LabelPlacement labelPlacement)
		{
			CategoryAxis categoryAxis = new CategoryAxis
			{
				LabelPlacement = labelPlacement
			};

			Assert.Equal(labelPlacement, categoryAxis.LabelPlacement);
		}

		[Fact]
		public void PlotBands_SetValue_ReturnsExpectedValue()
		{
			NumericalAxis numericalAxis = new NumericalAxis();
			var plotBand1 = new NumericalPlotBand() { Start = 20, End = 40, Fill = Colors.Bisque };
			numericalAxis.PlotBands =
			[
				plotBand1,
			];

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
			NumericalAxis numericalAxis = new NumericalAxis
			{
				EdgeLabelsVisibilityMode = visibilityMode
			};

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
				StrokeDashArray = [2, 2]
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
			NumericalAxis numericalAxis = new NumericalAxis
			{
				MinorTicksPerInterval = minorTicksPerInterval
			};

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
			NumericalAxis numericalAxis = new NumericalAxis
			{
				ShowMinorGridLines = showMinorGridLines
			};

			Assert.Equal(showMinorGridLines, numericalAxis.ShowMinorGridLines);
		}

		// Numerical Axis

		[Fact]
		public void NumericalAxis_ActualMaximum_SetValue_ReturnsExpectedValue()
		{
			NumericalAxis numericalAxis = new NumericalAxis
			{
				ActualMaximum = 100
			};

			Assert.Equal(100, numericalAxis.ActualMaximum);
		}

		[Fact]
		public void NumericalAxis_ActualMinimum_SetValue_ReturnsExpectedValue()
		{
			NumericalAxis numericalAxis = new NumericalAxis
			{
				ActualMinimum = 10
			};

			Assert.Equal(10, numericalAxis.ActualMinimum);
		}

		[Theory]
		[InlineData(100)]
		[InlineData(double.NaN)]
		public void NumericalAxis_Maximum_SetValue_ReturnsExpectedValue(double maximum)
		{
			NumericalAxis numericalAxis = new NumericalAxis
			{
				Maximum = maximum
			};

			Assert.Equal(maximum, numericalAxis.Maximum);
		}

		[Theory]
		[InlineData(10)]
		[InlineData(double.NaN)]
		public void NumericalAxis_Minimum_SetValue_ReturnsExpectedValue(double minimum)
		{
			NumericalAxis numericalAxis = new NumericalAxis
			{
				Minimum = minimum
			};

			Assert.Equal(minimum, numericalAxis.Minimum);
		}

		[Theory]
		[InlineData(NumericalPadding.None)]
		[InlineData(NumericalPadding.Round)]
		[InlineData(NumericalPadding.Additional)]
		public void NumericalAxis_RangePadding_SetValue_ReturnsExpectedValue(NumericalPadding rangePadding)
		{
			NumericalAxis numericalAxis = new NumericalAxis
			{
				RangePadding = rangePadding
			};

			Assert.Equal(rangePadding, numericalAxis.RangePadding);
		}

		[Theory]
		[InlineData(2)]
		[InlineData(10)]
		[InlineData(100)]
		public void LogarithmicBase_SetValue_ReturnsExpectedValue(double logarithmicBase)
		{
			LogarithmicAxis logarithmicAxis = new LogarithmicAxis
			{
				LogarithmicBase = logarithmicBase
			};

			Assert.Equal(logarithmicBase, logarithmicAxis.LogarithmicBase);
		}

		// DateTimeAxis

		[Fact]
		public void DateTimeAxis_ActualMaximum_SetValue_ReturnsExpectedValue()
		{
			DateTimeAxis dateTimeAxis = new DateTimeAxis
			{
				ActualMaximum = new DateTime(2023, 12, 31)
			};

			Assert.Equal(new DateTime(2023, 12, 31), dateTimeAxis.ActualMaximum);
		}

		[Fact]
		public void DateTimeAxis_ActualMinimum_SetValue_ReturnsExpectedValue()
		{
			DateTimeAxis dateTimeAxis = new DateTimeAxis
			{
				ActualMinimum = new DateTime(2023, 1, 1)
			};

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
			DateTimeAxis dateTimeAxis = new DateTimeAxis
			{
				AutoScrollingDeltaType = intervalType
			};

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
			DateTimeAxis dateTimeAxis = new DateTimeAxis
			{
				IntervalType = intervalType
			};

			Assert.Equal(intervalType, dateTimeAxis.IntervalType);
		}

		[Theory]
		[InlineData("2023-12-31")]
		[InlineData(null)]
		public void DateTimeAxis_Maximum_SetValue_ReturnsExpectedValue(string? maximum)
		{
			DateTimeAxis dateTimeAxis = new DateTimeAxis();
			DateTime? maxDate = maximum != null ? DateTime.Parse(maximum) : null;
			dateTimeAxis.Maximum = maxDate;

			Assert.Equal(maxDate, dateTimeAxis.Maximum);
		}

		[Theory]
		[InlineData("2023-01-01")]
		[InlineData(null)]
		public void DateTimeAxis_Minimum_SetValue_ReturnsExpectedValue(string? minimum)
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
			dateTimeAxis.PlotBands =
			[
				plotBand1,
			];

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
			DateTimeAxis dateTimeAxis = new DateTimeAxis
			{
				RangePadding = rangePadding
			};

			Assert.Equal(rangePadding, dateTimeAxis.RangePadding);
		}

		[Theory]
		[InlineData(2)]
		[InlineData(4)]
		[InlineData(0)]
		public void DateTimeCategoryInterval_SetValue_ReturnsExpectedValue(double interval)
		{
			DateTimeCategoryAxis dateTimeCategoryAxis = new DateTimeCategoryAxis
			{
				Interval = interval
			};

			Assert.Equal(interval, dateTimeCategoryAxis.Interval);
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
		public void DateTimeCategoryIntervalType_SetValue_ReturnsExpectedValue(DateTimeIntervalType intervalType)
		{
			DateTimeCategoryAxis dateTimeCategory = new DateTimeCategoryAxis
			{
				IntervalType = intervalType
			};

			Assert.Equal(intervalType, dateTimeCategory.IntervalType);
		}

		[Fact]
		public void DateTimeCategory_PlotBands_SetValue_ReturnsExpectedValue()
		{
			DateTimeCategoryAxis dateTimeCategory = new DateTimeCategoryAxis();
			var plotBand1 = new NumericalPlotBand() { Start = 1, End = 5, Fill = Colors.Bisque };
			dateTimeCategory.PlotBands =
			[
				plotBand1,
			];

			Assert.NotNull(dateTimeCategory.PlotBands);
			Assert.Single(dateTimeCategory.PlotBands);
			Assert.Contains(dateTimeCategory.PlotBands, band => band.Start == 1 && band.End == 5);
		}

		[Theory]
		[InlineData(AxisLabelsIntersectAction.Hide)]
		[InlineData(AxisLabelsIntersectAction.Wrap)]
		[InlineData(AxisLabelsIntersectAction.MultipleRows)]
		public void DateTimeCategoryLabelsIntersectAction_SetValue_ReturnsExpectedValue(AxisLabelsIntersectAction action)
		{
			DateTimeCategoryAxis dateTimeCategoryAxis = new DateTimeCategoryAxis();
			dateTimeCategoryAxis.LabelsIntersectAction = action;
			Assert.Equal(action, dateTimeCategoryAxis.LabelsIntersectAction);
		}

		[Theory]
		[InlineData(AxisElementPosition.Outside)]
		[InlineData(AxisElementPosition.Inside)]
		public void DateTimeCategoryLabelsPosition_SetValue_ReturnsExpectedValue(AxisElementPosition labelPosition)
		{
			DateTimeCategoryAxis dateTimeCategoryXAxis = new DateTimeCategoryAxis
			{
				LabelsPosition = labelPosition
			};

			Assert.Equal(labelPosition, dateTimeCategoryXAxis.LabelsPosition);
		}

		[Fact]
		public void PlotOffset_SetValue_ReturnsExpectedValue()
		{
			DateTimeCategoryAxis dateTimeCategory = new DateTimeCategoryAxis
			{
				PlotOffsetStart = 10,
				PlotOffsetEnd = 30
			};

			Assert.Equal(10, dateTimeCategory.PlotOffsetStart);
			Assert.Equal(30, dateTimeCategory.PlotOffsetEnd);
        }

		[Fact]
		public void MaximumLabels_IsDefault_ReturnsDefaultValue()
		{
			NumericalAxis axis = new();

			Assert.Equal(3, axis.MaximumLabels);
		}

		[Theory]
		[InlineData(0)]
		[InlineData(1)]
		[InlineData(5)]
		[InlineData(10)]
		public void MaximumLabels_SetValue_ReturnsExpectedValue(int maxLabels)
		{
			NumericalAxis axis = new()
			{
				MaximumLabels = maxLabels
			};

			Assert.Equal(maxLabels, axis.MaximumLabels);
		}

		[Fact]
		public void MaximumLabels_IsZero_ReturnsExpectedValue()
		{
			NumericalAxis axis = new()
			{
				MaximumLabels = 0
			};

			Size availableSize = new(800, 400);

			double intervalCount = axis.GetActualDesiredIntervalsCount(availableSize);

			Assert.Equal(1.0, intervalCount);
		}

		[Fact]
		public void MaximumLabels_IsDefault_ReturnsExpectedValue()
		{
			NumericalAxis axis = new();

			Size availableSize = new(800, 400);

			double intervalCount = axis.GetActualDesiredIntervalsCount(availableSize);

			Assert.True(intervalCount > 1.0);
		}

		#region Axis Range Style Tests

		[Fact]
		public void RangeStyles_DefaultValue_IsEmptyCollectionWithNullStyles()
		{
			// Arrange & Act
			var axis = new NumericalAxis();
			var style = new ChartAxisRangeStyle();

			// Assert - the axis provides a default empty collection; the range style's element
			// styles default to null so unset styles fall back to the axis-level styles.
			Assert.NotNull(axis.RangeStyles);
			Assert.Empty(axis.RangeStyles);
			Assert.Null(style.MajorGridLineStyle);
			Assert.Null(style.MinorGridLineStyle);
			Assert.Null(style.MajorTickStyle);
			Assert.Null(style.MinorTickStyle);
			Assert.Null(style.LabelStyle);
		}

		[Fact]
		public void RangeStyles_AddRangeStyle_CountIncreases()
		{
			// Arrange
			var axis = new NumericalAxis();
			var rangeStyle = new ChartAxisRangeStyle { Start = 15, End = 21 };

			// Act
			axis.RangeStyles.Add(rangeStyle);

			// Assert
			Assert.Single(axis.RangeStyles);
			Assert.Equal(15, axis.RangeStyles[0].Start);
			Assert.Equal(21, axis.RangeStyles[0].End);
		}

		[Theory]
		[InlineData(15, 21)]
		[InlineData(-50, -10)]
		[InlineData(0, 100)]
		[InlineData(0.5, 0.75)]
		public void RangeStyles_StartAndEnd_SetValue_ReturnsExpectedValue(double start, double end)
		{
			// Arrange
			var rangeStyle = new ChartAxisRangeStyle { Start = start, End = end };

			// Assert
			Assert.Equal(start, rangeStyle.Start);
			Assert.Equal(end, rangeStyle.End);
		}

		[Fact]
		public void RangeStyles_CompatibleWithAllAxisTypes()
		{
			// Arrange
			var numericalAxis = new NumericalAxis();
			var categoryAxis = new CategoryAxis();
			var dateTimeAxis = new DateTimeAxis();
			var logAxis = new LogarithmicAxis();
			var dateTimeCategoryAxis = new DateTimeCategoryAxis();

			var rangeStyle = new ChartAxisRangeStyle { Start = 15, End = 21 };

			// Act
			numericalAxis.RangeStyles.Add(rangeStyle);
			categoryAxis.RangeStyles.Add(new ChartAxisRangeStyle { Start = 0, End = 5 });
			dateTimeAxis.RangeStyles.Add(new ChartAxisRangeStyle
			{
				Start = new DateTime(2024, 1, 1),
				End = new DateTime(2024, 6, 30)
			});
			logAxis.RangeStyles.Add(new ChartAxisRangeStyle { Start = 10, End = 100 });
			dateTimeCategoryAxis.RangeStyles.Add(new ChartAxisRangeStyle { Start = 0, End = 3 });

			// Assert
			Assert.Single(numericalAxis.RangeStyles);
			Assert.Single(categoryAxis.RangeStyles);
			Assert.Single(dateTimeAxis.RangeStyles);
			Assert.Single(logAxis.RangeStyles);
			Assert.Single(dateTimeCategoryAxis.RangeStyles);
		}

		[Fact]
		public void RangeStyles_MajorGridLineStyle_SetValue_ReturnsExpectedValue()
		{
			// Arrange
			var rangeStyle = new ChartAxisRangeStyle();

			// Act
			rangeStyle.MajorGridLineStyle = new ChartLineStyle
			{
				Stroke = Colors.Blue,
				StrokeWidth = 3,
				StrokeDashArray = [2, 2]
			};

			// Assert
			Assert.Equal(Colors.Blue, rangeStyle.MajorGridLineStyle.Stroke);
			Assert.Equal(3, rangeStyle.MajorGridLineStyle.StrokeWidth);
			Assert.Equal(2, rangeStyle.MajorGridLineStyle.StrokeDashArray[0]);
		}

		[Fact]
		public void RangeStyles_MinorGridLineStyle_SetValue_ReturnsExpectedValue()
		{
			// Arrange
			var rangeStyle = new ChartAxisRangeStyle();

			// Act
			rangeStyle.MinorGridLineStyle = new ChartLineStyle
			{
				Stroke = Colors.Green,
				StrokeWidth = 2
			};

			// Assert
			Assert.Equal(Colors.Green, rangeStyle.MinorGridLineStyle.Stroke);
			Assert.Equal(2, rangeStyle.MinorGridLineStyle.StrokeWidth);
		}

		[Fact]
		public void RangeStyles_MajorTickStyle_SetValue_ReturnsExpectedValue()
		{
			// Arrange
			var rangeStyle = new ChartAxisRangeStyle();

			// Act
			rangeStyle.MajorTickStyle = new ChartAxisTickStyle
			{
				Stroke = Colors.Red,
				StrokeWidth = 1
			};

			// Assert
			Assert.Equal(Colors.Red, rangeStyle.MajorTickStyle.Stroke);
			Assert.Equal(1, rangeStyle.MajorTickStyle.StrokeWidth);
		}

		[Fact]
		public void RangeStyles_MinorTickStyle_SetValue_ReturnsExpectedValue()
		{
			// Arrange
			var rangeStyle = new ChartAxisRangeStyle();

			// Act
			rangeStyle.MinorTickStyle = new ChartAxisTickStyle
			{
				Stroke = Colors.Purple,
				StrokeWidth = 1
			};

			// Assert
			Assert.Equal(Colors.Purple, rangeStyle.MinorTickStyle.Stroke);
			Assert.Equal(1, rangeStyle.MinorTickStyle.StrokeWidth);
		}

		[Fact]
		public void RangeStyles_LabelStyle_SetValue_ReturnsExpectedValue()
		{
			// Arrange
			var rangeStyle = new ChartAxisRangeStyle();

			// Act
			rangeStyle.LabelStyle = new ChartAxisLabelStyle
			{
				TextColor = Color.FromArgb("#096EBF"),
				FontAttributes = FontAttributes.Bold
			};

			// Assert
			Assert.Equal(Color.FromArgb("#096EBF"), rangeStyle.LabelStyle.TextColor);
			Assert.Equal(FontAttributes.Bold, rangeStyle.LabelStyle.FontAttributes);
		}

		[Fact]
		public void RangeStyles_IsValueInRange_ValueInsideAndOutsideRange()
		{
			// Arrange
			var rangeStyle = new ChartAxisRangeStyle { Start = 15, End = 21 };

			// Act & Assert
			Assert.True(rangeStyle.IsValueInRange(15));
			Assert.True(rangeStyle.IsValueInRange(18));
			Assert.True(rangeStyle.IsValueInRange(21));
			Assert.False(rangeStyle.IsValueInRange(14.9));
			Assert.False(rangeStyle.IsValueInRange(21.1));
			Assert.False(rangeStyle.IsValueInRange(double.NaN));
		}

		[Fact]
		public void RangeStyles_OpenEndedRanges_AreSupported()
		{
			// Arrange
			var startOnly = new ChartAxisRangeStyle { Start = 15 };
			var endOnly = new ChartAxisRangeStyle { End = 21 };

			// Act & Assert
			Assert.True(startOnly.IsValueInRange(25));
			Assert.False(startOnly.IsValueInRange(14));
			Assert.True(endOnly.IsValueInRange(10));
			Assert.False(endOnly.IsValueInRange(22));
		}

		[Fact]
		public void RangeStyles_ReversedBounds_AreNormalized()
		{
			// Arrange - Start greater than End should still match values between them.
			var rangeStyle = new ChartAxisRangeStyle { Start = 21, End = 15 };

			// Act & Assert
			Assert.True(rangeStyle.IsValueInRange(17));
			Assert.False(rangeStyle.IsValueInRange(25));
			Assert.False(rangeStyle.IsValueInRange(12));
		}

		[Fact]
		public void RangeStyles_BothBoundsMissing_StyleIsNeverApplied()
		{
			// Arrange
			var rangeStyle = new ChartAxisRangeStyle();

			// Act & Assert
			Assert.False(rangeStyle.IsValueInRange(0));
			Assert.False(rangeStyle.IsValueInRange(1000));
		}

		[Fact]
		public void RangeStyles_DateTimeValues_AreSupported()
		{
			// Arrange
			var start = new DateTime(2024, 1, 1);
			var end = new DateTime(2024, 6, 30);
			var rangeStyle = new ChartAxisRangeStyle { Start = start, End = end };

			// Act & Assert
			Assert.True(rangeStyle.IsValueInRange(start.ToOADate()));
			Assert.True(rangeStyle.IsValueInRange(new DateTime(2024, 4, 15).ToOADate()));
			Assert.False(rangeStyle.IsValueInRange(new DateTime(2025, 1, 1).ToOADate()));
		}

		[Fact]
		public void RangeStyles_GetRangeStyle_ReturnsFirstMatchingStyle()
		{
			// Arrange
			var axis = new NumericalAxis();
			var firstStyle = new ChartAxisRangeStyle { Start = 10, End = 20 };
			var secondStyle = new ChartAxisRangeStyle { Start = 15, End = 25 };
			axis.RangeStyles.Add(firstStyle);
			axis.RangeStyles.Add(secondStyle);

			// Act
			var matchMid = axis.GetRangeStyle(18);
			var matchOutside = axis.GetRangeStyle(5);

			// Assert
			Assert.Same(firstStyle, matchMid);
			Assert.Null(matchOutside);
			Assert.Same(secondStyle, axis.GetRangeStyle(22));
		}

		[Fact]
		public void RangeStyles_EmptyCollection_GetRangeStyleReturnsNull()
		{
			// Arrange
			var axis = new NumericalAxis();

			// Act & Assert
			Assert.False(axis.HasRangeStyles);
			Assert.Null(axis.GetRangeStyle(15));
		}

		[Fact]
		public void RangeStyles_RemoveRangeStyle_CountDecreases()
		{
			// Arrange
			var axis = new NumericalAxis();
			var rangeStyle = new ChartAxisRangeStyle { Start = 15, End = 21 };
			axis.RangeStyles.Add(rangeStyle);

			// Act
			axis.RangeStyles.Remove(rangeStyle);

			// Assert
			Assert.Empty(axis.RangeStyles);
			Assert.False(axis.HasRangeStyles);
		}

		[Fact]
		public void RangeStyles_ClearCollection_GetRangeStyleReturnsNull()
		{
			// Arrange
			var axis = new NumericalAxis();
			axis.RangeStyles.Add(new ChartAxisRangeStyle { Start = 15, End = 21 });
			axis.RangeStyles.Add(new ChartAxisRangeStyle { Start = 40, End = 60 });

			// Act
			axis.RangeStyles.Clear();

			// Assert
			Assert.Empty(axis.RangeStyles);
			Assert.False(axis.HasRangeStyles);
			Assert.Null(axis.GetRangeStyle(50));
		}

		[Fact]
		public void RangeStyles_ReplaceCollection_NewStylesAreUsed()
		{
			// Arrange
			var axis = new NumericalAxis();
			var newCollection = new ChartAxisRangeStyleCollection
			{
				new ChartAxisRangeStyle { Start = 5, End = 8 }
			};

			// Act
			axis.RangeStyles = newCollection;

			// Assert
			Assert.Same(newCollection, axis.RangeStyles);
			Assert.True(axis.HasRangeStyles);
			Assert.NotNull(axis.GetRangeStyle(6));
		}

		[Fact]
		public void RangeStyles_MultipleRangeStyles_MutuallyExclusiveVisualBehavior()
		{
			// Arrange - two adjacent ranges covering different sections of the axis.
			var axis = new NumericalAxis();
			var rangeA = new ChartAxisRangeStyle { Start = 0, End = 10 };
			var rangeB = new ChartAxisRangeStyle { Start = 20, End = 30 };
			axis.RangeStyles.Add(rangeA);
			axis.RangeStyles.Add(rangeB);

			// Act & Assert - each value resolves to exactly one range style.
			Assert.Same(rangeA, axis.GetRangeStyle(5));
			Assert.Same(rangeB, axis.GetRangeStyle(25));
			Assert.Null(axis.GetRangeStyle(15));
		}

		[Fact]
		public void RangeStyles_LabelCreation_AppliesRangeStyleLabelStyle()
		{
			// Arrange
			var axis = new NumericalAxis();
			var rangeStyle = new ChartAxisRangeStyle
			{
				Start = 15,
				End = 21,
				LabelStyle = new ChartAxisLabelStyle { TextColor = Colors.Blue }
			};
			axis.RangeStyles.Add(rangeStyle);

			// Act - simulate label creation through the visible labels pipeline.
			var inRangeLabel = new ChartAxisLabel(18, "18");
			var axisMethod = typeof(ChartAxis).GetMethod("InvokeLabelCreated",
				System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
			axisMethod?.Invoke(axis, new object[] { inRangeLabel });

			var outOfRangeLabel = new ChartAxisLabel(25, "25");
			axisMethod?.Invoke(axis, new object[] { outOfRangeLabel });

			// Assert
			Assert.NotNull(inRangeLabel.LabelStyle);
			Assert.Equal(Colors.Blue, inRangeLabel.LabelStyle.TextColor);
			Assert.Null(outOfRangeLabel.LabelStyle);
		}

		[Fact]
		public void RangeStyles_NullStyleInCollection_IsSkippedDuringLookup()
		{
			// Arrange
			var axis = new NumericalAxis();
			axis.RangeStyles.Add(new ChartAxisRangeStyle { Start = 15, End = 21 });
			axis.RangeStyles.Add(new ChartAxisRangeStyle { Start = 30, End = 40 });

			// Act & Assert - values outside all defined ranges do not match anything.
			Assert.Null(axis.GetRangeStyle(25));
		}

		[Fact]
		public void RangeStyles_WithZooming_StyleLookupStillResolves()
		{
			// Arrange
			var axis = new NumericalAxis();
			var rangeStyle = new ChartAxisRangeStyle { Start = 15, End = 21 };
			axis.RangeStyles.Add(rangeStyle);

			// Act - simulate zoom state.
			axis.ZoomFactor = 0.5;
			axis.ZoomPosition = 0.25;

			// Assert - the range lookup is pure value-based, unaffected by zoom transformations.
			Assert.Same(rangeStyle, axis.GetRangeStyle(18));
		}

		[Theory]
		[InlineData(0, 3, 1.5)]
		[InlineData(3, 7, 5)]
		[InlineData(7, 11, 9)]
		[InlineData(0, 11, 5.5)]
		public void RangeStyles_CategoryAxisIndexBounds_StyleIsAppliedOnIndexRange(double indexStart, double indexEnd, double indexInside)
		{
			// Arrange
			var categoryAxis = new CategoryAxis();
			var rangeStyle = new ChartAxisRangeStyle { Start = indexStart, End = indexEnd };
			categoryAxis.RangeStyles.Add(rangeStyle);

			// Act & Assert
			Assert.Same(rangeStyle, categoryAxis.GetRangeStyle(indexInside));
			Assert.Null(categoryAxis.GetRangeStyle(indexEnd + 5));
		}

		[Fact]
		public void RangeStyles_AddDynamically_RaisesCollectionChangedSoAxisRedraws()
		{
			// Arrange - the axis must observe membership changes to trigger UpdateLayout.
			var axis = new NumericalAxis();
			int notifyCount = 0;
			axis.RangeStyles.RangeStyleChanged += (s, e) => notifyCount++;

			// Act - dynamically adding a style must raise the notification.
			axis.RangeStyles.Add(new ChartAxisRangeStyle { Start = 15, End = 21 });

			// Assert
			Assert.Equal(1, notifyCount);
		}

		[Fact]
		public void RangeStyles_AddMultipleDynamically_RaisesNotificationForEachChange()
		{
			// Arrange
			var axis = new NumericalAxis();
			int notifyCount = 0;
			axis.RangeStyles.RangeStyleChanged += (s, e) => notifyCount++;

			// Act - multiple dynamic additions must each raise the notification.
			axis.RangeStyles.Add(new ChartAxisRangeStyle { Start = 5, End = 8 });
			axis.RangeStyles.Add(new ChartAxisRangeStyle { Start = 15, End = 21 });
			axis.RangeStyles.Add(new ChartAxisRangeStyle { Start = 30, End = 40 });

			// Assert
			Assert.Equal(3, notifyCount);
			Assert.True(axis.HasRangeStyles);
		}

		[Fact]
		public void RangeStyles_RemoveDynamically_RaisesCollectionChangedSoAxisRedraws()
		{
			// Arrange
			var axis = new NumericalAxis();
			var rangeStyle = new ChartAxisRangeStyle { Start = 15, End = 21 };
			axis.RangeStyles.Add(rangeStyle);
			int notifyCount = 0;
			axis.RangeStyles.RangeStyleChanged += (s, e) => notifyCount++;

			// Act
			axis.RangeStyles.Remove(rangeStyle);

			// Assert
			Assert.Equal(1, notifyCount);
			Assert.False(axis.HasRangeStyles);
		}

		[Fact]
		public void RangeStyles_ClearDynamically_RaisesCollectionChangedSoAxisRedraws()
		{
			// Arrange
			var axis = new NumericalAxis();
			axis.RangeStyles.Add(new ChartAxisRangeStyle { Start = 5, End = 8 });
			axis.RangeStyles.Add(new ChartAxisRangeStyle { Start = 15, End = 21 });
			int notifyCount = 0;
			axis.RangeStyles.RangeStyleChanged += (s, e) => notifyCount++;

			// Act
			axis.RangeStyles.Clear();

			// Assert
			Assert.Equal(1, notifyCount);
			Assert.False(axis.HasRangeStyles);
		}

		[Fact]
		public void RangeStyles_ReplaceDynamically_RaisesCollectionChangedSoAxisRedraws()
		{
			// Arrange
			var axis = new NumericalAxis();
			axis.RangeStyles.Add(new ChartAxisRangeStyle { Start = 5, End = 8 });
			int notifyCount = 0;
			axis.RangeStyles.RangeStyleChanged += (s, e) => notifyCount++;

			// Act - indexer replace must raise the notification too.
			axis.RangeStyles[0] = new ChartAxisRangeStyle { Start = 15, End = 21 };

			// Assert
			Assert.Equal(1, notifyCount);
			Assert.Same(axis.GetRangeStyle(18), axis.RangeStyles[0]);
		}

		[Fact]
		public void RangeStyles_ItemStyleChange_AfterDynamicAdd_StillRaisesNotification()
		{
			// Arrange - dynamically added items must keep propagating property changes.
			var axis = new NumericalAxis();
			var rangeStyle = new ChartAxisRangeStyle
			{
				Start = 15,
				End = 21,
				MajorGridLineStyle = new ChartLineStyle()
			};
			axis.RangeStyles.Add(rangeStyle);
			int notifyCount = 0;
			axis.RangeStyles.RangeStyleChanged += (s, e) => notifyCount++;

			// Act - mutating a nested style property and the range bounds.
			rangeStyle.MajorGridLineStyle!.StrokeWidth = 4;
			rangeStyle.Start = 10;

			// Assert
			Assert.Equal(2, notifyCount);
		}

		[Fact]
		public void RangeStyles_StyleProperties_DefaultToNull_SoUnsetStylesFallBackToAxisStyles()
		{
			// Arrange & Act - a range that only customizes the major grid line must not own
			// the other element styles, allowing fallthrough to axis-level defaults.
			var rangeStyle = new ChartAxisRangeStyle();

			// Assert
			Assert.Null(rangeStyle.MajorGridLineStyle);
			Assert.Null(rangeStyle.MinorGridLineStyle);
			Assert.Null(rangeStyle.MajorTickStyle);
			Assert.Null(rangeStyle.MinorTickStyle);
			Assert.Null(rangeStyle.LabelStyle);
		}

		[Fact]
		public void RangeStyles_OverlappingRanges_EachElementTypeResolvesIndependently()
		{
			// Arrange - the user's exact overlap scenario:
			// Range A (0-50) sets major grid line style.
			// Range B (30-80) sets major tick style.
			// Range C (40-70) sets minor grid line style.
			var axis = new NumericalAxis();
			var rangeA = new ChartAxisRangeStyle
			{
				Start = 0,
				End = 50,
				MajorGridLineStyle = new ChartLineStyle { Stroke = Colors.Red, StrokeWidth = 2 }
			};
			var rangeB = new ChartAxisRangeStyle
			{
				Start = 30,
				End = 80,
				MajorTickStyle = new ChartAxisTickStyle { Stroke = Colors.Green, StrokeWidth = 2 }
			};
			var rangeC = new ChartAxisRangeStyle
			{
				Start = 40,
				End = 70,
				MinorGridLineStyle = new ChartLineStyle { Stroke = Colors.Blue, StrokeWidth = 2 }
			};
			axis.RangeStyles.Add(rangeA);
			axis.RangeStyles.Add(rangeB);
			axis.RangeStyles.Add(rangeC);

			// Act & Assert - at value 45 all three ranges contain the value; each element
			// style must resolve from its own setting range, not just the first match.
			Assert.Same(rangeA.MajorGridLineStyle, axis.GetMajorGridLineStyleInRange(45));
			Assert.Same(rangeB.MajorTickStyle, axis.GetMajorTickStyleInRange(45));
			Assert.Same(rangeC.MinorGridLineStyle, axis.GetMinorGridLineStyleInRange(45));

			// The unset element styles of overlapping ranges must fall back to null (axis default).
			Assert.Null(axis.GetMinorTickStyleInRange(45));
			Assert.Null(axis.GetLabelStyleInRange(45));
		}

		[Fact]
		public void RangeStyles_OverlappingRanges_LastRangeSettingSameElementWins()
		{
			// Arrange
			var axis = new NumericalAxis();
			var first = new ChartAxisRangeStyle
			{
				Start = 0,
				End = 50,
				MajorTickStyle = new ChartAxisTickStyle { Stroke = Colors.Green }
			};
			var last = new ChartAxisRangeStyle
			{
				Start = 20,
				End = 50,
				MajorTickStyle = new ChartAxisTickStyle { Stroke = Colors.Orange }
			};
			axis.RangeStyles.Add(first);
			axis.RangeStyles.Add(last);

			// Act & Assert - in the overlap, the last range setting the same element style wins.
			Assert.Same(last.MajorTickStyle, axis.GetMajorTickStyleInRange(35));
			// Outside the last range's start, the first range's style still applies.
			Assert.Same(first.MajorTickStyle, axis.GetMajorTickStyleInRange(10));
		}

		[Fact]
		public void RangeStyles_RangeLeavingStyleUnset_DoesNotBlockOtherOverlappingRanges()
		{
			// Arrange
			var axis = new NumericalAxis();
			var earlier = new ChartAxisRangeStyle
			{
				Start = 0,
				End = 60,
				MajorGridLineStyle = new ChartLineStyle { Stroke = Colors.Red }
			};
			var middle = new ChartAxisRangeStyle { Start = 10, End = 40 }; // sets nothing
			axis.RangeStyles.Add(earlier);
			axis.RangeStyles.Add(middle);

			// Act & Assert - inside 'middle', the earlier range's style must still resolve
			// because the middle range does not set (null) that style.
			Assert.Same(earlier.MajorGridLineStyle, axis.GetMajorGridLineStyleInRange(25));
			// Outside 'earlier' entirely, nothing resolves.
			Assert.Null(axis.GetMajorGridLineStyleInRange(70));
		}

		[Fact]
		public void RangeStyles_OverlappingRanges_LabelStyle_MergesAcrossRanges()
		{
			// Arrange
			var axis = new NumericalAxis();
			var rangeA = new ChartAxisRangeStyle
			{
				Start = 0,
				End = 50,
				LabelStyle = new ChartAxisLabelStyle { TextColor = Colors.Red }
			};
			var rangeB = new ChartAxisRangeStyle
			{
				Start = 30,
				End = 80,
				LabelStyle = new ChartAxisLabelStyle { TextColor = Colors.Blue }
			};
			axis.RangeStyles.Add(rangeA);
			axis.RangeStyles.Add(rangeB);

			// Act & Assert - inside the overlap the last range's label style wins;
			// inside range A only, its style applies; outside both, none applies.
			Assert.Same(rangeA.LabelStyle, axis.GetLabelStyleInRange(20));
			Assert.Same(rangeB.LabelStyle, axis.GetLabelStyleInRange(45));
			Assert.Null(axis.GetLabelStyleInRange(90));
		}

		[Fact]
		public void RangeStyles_LabelCreation_AppliesMergedLabelStyleFromOverlappingRanges()
		{
			// Arrange
			var axis = new NumericalAxis();
			axis.RangeStyles.Add(new ChartAxisRangeStyle
			{
				Start = 0,
				End = 50,
				LabelStyle = new ChartAxisLabelStyle { TextColor = Colors.Red }
			});
			axis.RangeStyles.Add(new ChartAxisRangeStyle
			{
				Start = 30,
				End = 80,
				LabelStyle = new ChartAxisLabelStyle { TextColor = Colors.Blue }
			});

			var overlapLabel = new ChartAxisLabel(45, "45");
			var earlierOnlyLabel = new ChartAxisLabel(10, "10");
			var outsideLabel = new ChartAxisLabel(90, "90");
			var axisMethod = typeof(ChartAxis).GetMethod("InvokeLabelCreated",
				System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

			// Act
			axisMethod?.Invoke(axis, new object[] { overlapLabel });
			axisMethod?.Invoke(axis, new object[] { earlierOnlyLabel });
			axisMethod?.Invoke(axis, new object[] { outsideLabel });

			// Assert
			Assert.Equal(Colors.Blue, overlapLabel.LabelStyle?.TextColor);
			Assert.Equal(Colors.Red, earlierOnlyLabel.LabelStyle?.TextColor);
			Assert.Null(outsideLabel.LabelStyle);
		}

		#endregion
	}
}