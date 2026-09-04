using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.Toolkit.UnitTest.Charts
{
	public class SfPyramidChartUnitTests
	{
		[Fact]
		public void Constructor_InitializesDefaultsCorrectly()
		{
			var chart = new SfPyramidChart();

			Assert.Null(chart.ItemsSource);

			Assert.Null(chart.TooltipTemplate);

			Assert.False(chart.EnableTooltip);

			Assert.Null(chart.XBindingPath);

			Assert.Null(chart.YBindingPath);

			var defaultBrushes = chart.PaletteBrushes;

			Assert.Equal(10, defaultBrushes.Count);

			Assert.Equal(Color.FromRgba(0, 189, 174, 255), (defaultBrushes[0] as SolidColorBrush)?.Color);

			Assert.Null(chart.SelectionBehavior);

			Assert.False(chart.ShowDataLabels);

			Assert.Null(chart.LabelTemplate);

			Assert.Equal(PyramidDataLabelContext.YValue, chart.DataLabelSettings.Context);

			Assert.Equal(ChartLegendIconType.Circle, chart.LegendIcon);

			Assert.Equal(Colors.Transparent, chart.Stroke);

			Assert.Equal(2, chart.StrokeWidth);

			Assert.Equal(0, chart.GapRatio);

			Assert.Equal(PyramidMode.Linear, chart.Mode);
		}

		[Fact]
		public void ItemsSource_SetAndGet_ReturnsExpectedValue()
		{
			var chart = new SfPyramidChart();
			var data = new List<dynamic>
			{
				new  { XValue = "A", YValue = 10 },
				new  { XValue = "B", YValue = 20 }
			};

			chart.ItemsSource = data;

			Assert.Equal(data, chart.ItemsSource);
		}

		[Fact]
		public void TooltipTemplate_SetAndGet_ReturnsExpectedValue()
		{
			var chart = new SfPyramidChart();
			var template = new DataTemplate();

			chart.TooltipTemplate = template;

			Assert.Equal(template, chart.TooltipTemplate);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void EnableTooltip_SetAndGet_ReturnsExpectedValue(bool expected)
		{
			var chart = new SfPyramidChart
			{
				EnableTooltip = expected
			};

			Assert.Equal(expected, chart.EnableTooltip);
		}

		[Theory]
		[InlineData("XValue1")]
		[InlineData("XValue2")]
		public void XBindingPath_SetAndGet_ReturnsExpectedValue(string expected)
		{
			var chart = new SfPyramidChart
			{
				XBindingPath = expected
			};

			Assert.Equal(expected, chart.XBindingPath);
		}

		[Theory]
		[InlineData("YValue1")]
		[InlineData("YValue2")]
		public void YBindingPath_SetAndGet_ReturnsExpectedValue(string expected)
		{
			var chart = new SfPyramidChart
			{
				YBindingPath = expected
			};

			Assert.Equal(expected, chart.YBindingPath);
		}

		[Fact]
		public void PaletteBrushes_SetAndGet_ReturnsExpectedValue()
		{
			var chart = new SfPyramidChart();

			var brushes = new List<Brush>
			{
				new SolidColorBrush(Colors.Red),
				new SolidColorBrush(Colors.Blue)
			};

			chart.PaletteBrushes = brushes;

			Assert.Equal(brushes, chart.PaletteBrushes);
		}

		[Fact]
		public void SelectionBehavior_SetAndGet_ReturnsExpectedValue()
		{
			var chart = new SfPyramidChart();

			var selectionBehavior = new DataPointSelectionBehavior
			{
				SelectionBrush = new SolidColorBrush(Colors.Red)
			};

			chart.SelectionBehavior = selectionBehavior;

			Assert.Equal(selectionBehavior, chart.SelectionBehavior);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void ShowDataLabels_SetAndGet_ReturnsExpectedValue(bool expected)
		{
			var chart = new SfPyramidChart
			{
				ShowDataLabels = expected
			};

			Assert.Equal(expected, chart.ShowDataLabels);
		}

		[Theory]
		[InlineData(0.3)]
		[InlineData(0.5)]
		[InlineData(1.0)]
		public void GapRatio_SetAndGet_ReturnsExpectedValue(double expected)
		{
			var chart = new SfPyramidChart
			{
				GapRatio = expected
			};

			Assert.Equal(expected, chart.GapRatio);
		}

		[Theory]
		[InlineData(2.0)]
		[InlineData(4.0)]
		[InlineData(8.0)]
		public void StrokeWidth_SetAndGet_ReturnsExpectedValue(double expected)
		{
			var chart = new SfPyramidChart
			{
				StrokeWidth = expected
			};

			Assert.Equal(expected, chart.StrokeWidth);
		}

		[Theory]
		[InlineData(ChartLegendIconType.Circle)]
		[InlineData(ChartLegendIconType.Diamond)]
		[InlineData(ChartLegendIconType.Cross)]
		[InlineData(ChartLegendIconType.Hexagon)]
		[InlineData(ChartLegendIconType.Rectangle)]
		[InlineData(ChartLegendIconType.HorizontalLine)]
		[InlineData(ChartLegendIconType.InvertedTriangle)]
		[InlineData(ChartLegendIconType.Triangle)]
		[InlineData(ChartLegendIconType.Pentagon)]
		[InlineData(ChartLegendIconType.Plus)]
		[InlineData(ChartLegendIconType.SeriesType)]
		[InlineData(ChartLegendIconType.VerticalLine)]
		public void LegendIcon_SetAndGet_ReturnsExpectedValue(ChartLegendIconType expected)
		{
			var chart = new SfPyramidChart
			{
				LegendIcon = expected
			};

			Assert.Equal(expected, chart.LegendIcon);
		}

		[Fact]
		public void Stroke_SetAndGet_ReturnsExpectedValue()
		{
			var chart = new SfPyramidChart
			{
				Stroke = new SolidColorBrush(Colors.Red)
			};

			Assert.Equal(new SolidColorBrush(Colors.Red), chart.Stroke);
		}

		[Fact]
		public void DataLabelSettings_SetAndGet_ReturnsExpectedValue()
		{
			var chart = new SfPyramidChart();
			var settings = new PyramidDataLabelSettings();

			chart.DataLabelSettings = settings;

			Assert.Equal(settings, chart.DataLabelSettings);
		}

		[Fact]
		public void LabelTemplate_SetAndGet_ReturnsExpectedValue()
		{
			var chart = new SfPyramidChart();
			var template = new DataTemplate();

			chart.LabelTemplate = template;

			Assert.Equal(template, chart.LabelTemplate);
		}

		[Theory]
		[InlineData(PyramidMode.Surface)]
		[InlineData(PyramidMode.Linear)]
		public void Mode_SetAndGet_ReturnsExpectedValue(PyramidMode mode)
		{
			var chart = new SfPyramidChart
			{
				Mode = mode
			};

			Assert.Equal(mode, chart.Mode);
		}

		#region PointColorPath Tests

		[Fact]
		public void PyramidChart_PointColorPath_SetAndGet_ReturnsExpectedValue()
		{
			var chart = new SfPyramidChart();
			var expected = "PointColor";

			chart.PointColorPath = expected;

			Assert.Equal(expected, chart.PointColorPath);
		}

		[Fact]
		public void PyramidChart_PointColorPath_WithBrushProperty_PopulatesPointColorValues()
		{
			var chart = new SfPyramidChart();
			var data = new List<DataPoint>
			{
				new DataPoint { X = "A", Y = 10, PointColor = new SolidColorBrush(Colors.Red) },
				new DataPoint { X = "B", Y = 20, PointColor = new SolidColorBrush(Colors.Green) },
				new DataPoint { X = "C", Y = 30, PointColor = new SolidColorBrush(Colors.Blue) }
			};

			chart.ItemsSource = data;
			chart.XBindingPath = "X";
			chart.YBindingPath = "Y";
			chart.PointColorPath = "PointColor";

			Assert.NotEmpty(chart.PointColorValues);
			Assert.Equal(3, chart.PointColorValues.Count);
			Assert.NotNull(chart.PointColorValues[0]);
			Assert.NotNull(chart.PointColorValues[1]);
			Assert.NotNull(chart.PointColorValues[2]);
		}

		[Fact]
		public void PyramidChart_PointColorPath_WithColorProperty_PopulatesPointColorValues()
		{
			var chart = new SfPyramidChart();
			var data = new List<ColorDataPoint>
			{
				new ColorDataPoint { X = "A", Y = 10, Color = Colors.Red },
				new ColorDataPoint { X = "B", Y = 20, Color = Colors.Green },
				new ColorDataPoint { X = "C", Y = 30, Color = Colors.Blue }
			};

			chart.ItemsSource = data;
			chart.XBindingPath = "X";
			chart.YBindingPath = "Y";
			chart.PointColorPath = "Color";

			Assert.NotEmpty(chart.PointColorValues);
			Assert.Equal(3, chart.PointColorValues.Count);
			Assert.NotNull(chart.PointColorValues[0]);
			Assert.NotNull(chart.PointColorValues[1]);
			Assert.NotNull(chart.PointColorValues[2]);
		}

		[Fact]
		public void PyramidChart_PointColorPath_WithStringColorProperty_PopulatesPointColorValues()
		{
			var chart = new SfPyramidChart();
			var data = new List<StringColorDataPoint>
			{
				new StringColorDataPoint { X = "A", Y = 10, Color = "#FF0000" },
				new StringColorDataPoint { X = "B", Y = 20, Color = "#00FF00" },
				new StringColorDataPoint { X = "C", Y = 30, Color = "#0000FF" }
			};

			chart.ItemsSource = data;
			chart.XBindingPath = "X";
			chart.YBindingPath = "Y";
			chart.PointColorPath = "Color";

			Assert.NotEmpty(chart.PointColorValues);
			Assert.Equal(3, chart.PointColorValues.Count);
		}

		[Fact]
		public void PyramidChart_PointColorPath_WithComplexPath_PopulatesPointColorValues()
		{
			var chart = new SfPyramidChart();
			var data = new List<NestedDataPoint>
			{
				new NestedDataPoint { X = "A", Y = 10, Metadata = new ColorMetadata { Color = Colors.Red } },
				new NestedDataPoint { X = "B", Y = 20, Metadata = new ColorMetadata { Color = Colors.Green } },
				new NestedDataPoint { X = "C", Y = 30, Metadata = new ColorMetadata { Color = Colors.Blue } }
			};

			chart.ItemsSource = data;
			chart.XBindingPath = "X";
			chart.YBindingPath = "Y";
			chart.PointColorPath = "Metadata.Color";

			Assert.NotEmpty(chart.PointColorValues);
			Assert.Equal(3, chart.PointColorValues.Count);
		}

		[Fact]
		public void PyramidChart_PointColorPath_WithNullProperty_AddsNullToPointColorValues()
		{
			var chart = new SfPyramidChart();
			var data = new List<NullableColorDataPoint>
			{
				new NullableColorDataPoint { X = "A", Y = 10, PointColor = new SolidColorBrush(Colors.Red) },
				new NullableColorDataPoint { X = "B", Y = 20, PointColor = null },
				new NullableColorDataPoint { X = "C", Y = 30, PointColor = new SolidColorBrush(Colors.Blue) }
			};

			chart.ItemsSource = data;
			chart.XBindingPath = "X";
			chart.YBindingPath = "Y";
			chart.PointColorPath = "PointColor";

			Assert.NotEmpty(chart.PointColorValues);
			Assert.Equal(3, chart.PointColorValues.Count);
			Assert.NotNull(chart.PointColorValues[0]);
			Assert.Null(chart.PointColorValues[1]);
			Assert.NotNull(chart.PointColorValues[2]);
		}

		[Fact]
		public void PyramidChart_PointColorPath_ChangingPath_UpdatesPointColorValues()
		{
			var chart = new SfPyramidChart();
			var data = new List<DualColorDataPoint>
			{
				new DualColorDataPoint { X = "A", Y = 10, PrimaryColor = Colors.Red, SecondaryColor = Colors.Orange },
				new DualColorDataPoint { X = "B", Y = 20, PrimaryColor = Colors.Green, SecondaryColor = Colors.Lime },
				new DualColorDataPoint { X = "C", Y = 30, PrimaryColor = Colors.Blue, SecondaryColor = Colors.Cyan }
			};

			chart.ItemsSource = data;
			chart.XBindingPath = "X";
			chart.YBindingPath = "Y";

			// Set initial path
			chart.PointColorPath = "PrimaryColor";
			Assert.Equal(3, chart.PointColorValues.Count);

			// Change path
			chart.PointColorPath = "SecondaryColor";
			Assert.Equal(3, chart.PointColorValues.Count);
		}

		[Fact]
		public void PyramidChart_PointColorPath_ClearedOnItemsSourceChange()
		{
			var chart = new SfPyramidChart();
			var data1 = new List<ColorDataPoint>
			{
				new ColorDataPoint { X = "A", Y = 10, Color = Colors.Red },
				new ColorDataPoint { X = "B", Y = 20, Color = Colors.Green }
			};

			chart.ItemsSource = data1;
			chart.XBindingPath = "X";
			chart.YBindingPath = "Y";
			chart.PointColorPath = "Color";

			Assert.Equal(2, chart.PointColorValues.Count);

			// Change ItemsSource
			var data2 = new List<ColorDataPoint>
			{
				new ColorDataPoint { X = "X", Y = 100, Color = Colors.Blue }
			};
			chart.ItemsSource = data2;

			// PointColorValues should be cleared and regenerated
			Assert.Single(chart.PointColorValues);
		}

		[Fact]
		public void PyramidChart_GetFillColor_WithPointColorPath_ReturnsPointColor()
		{
			var chart = new SfPyramidChart();
			var brush = new SolidColorBrush(Colors.Red);
			var data = new List<DataPoint>
			{
				new DataPoint { X = "A", Y = 10, PointColor = brush }
			};

			chart.ItemsSource = data;
			chart.XBindingPath = "X";
			chart.YBindingPath = "Y";
			chart.PointColorPath = "PointColor";

			// Get the fill color for index 0
			var fillColor = PyramidChartBase.GetFillColor(chart, 0);

			Assert.NotNull(fillColor);
			Assert.Equal(brush, fillColor);
		}

		[Fact]
		public void PyramidChart_GetFillColor_Precedence_PointColorOverPalette()
		{
			var chart = new SfPyramidChart();
			var pointColor = new SolidColorBrush(Colors.Red);
			var paletteColor = new SolidColorBrush(Colors.Green);
			var data = new List<DataPoint>
			{
				new DataPoint { X = "A", Y = 10, PointColor = pointColor }
			};

			chart.ItemsSource = data;
			chart.XBindingPath = "X";
			chart.YBindingPath = "Y";
			chart.PointColorPath = "PointColor";
			chart.PaletteBrushes = new List<Brush> { paletteColor };

			// Without series Fill, point color should be used (higher precedence than palette)
			var fillColor = PyramidChartBase.GetFillColor(chart, 0);
			Assert.Equal(pointColor, fillColor);
		}

		[Fact]
		public void PyramidChart_GetFillColor_SelectionBehavior_OverridesPointColor()
		{
			var chart = new SfPyramidChart();
			var pointColor = new SolidColorBrush(Colors.Red);
			var selectionBrush = new SolidColorBrush(Colors.Yellow);
			var data = new List<DataPoint>
			{
				new DataPoint { X = "A", Y = 10, PointColor = pointColor },
				new DataPoint { X = "B", Y = 20, PointColor = new SolidColorBrush(Colors.Blue) }
			};

			chart.ItemsSource = data;
			chart.XBindingPath = "X";
			chart.YBindingPath = "Y";
			chart.PointColorPath = "PointColor";

			// Create and assign selection behavior with selection brush
			var selectionBehavior = new DataPointSelectionBehavior
			{
				SelectionBrush = selectionBrush
			};
			chart.SelectionBehavior = selectionBehavior;

			// Programmatically select the first data point (index 0)
			selectionBehavior.SelectedIndex = 0;

			// Get fill color for the selected data point
			var selectedFillColor = PyramidChartBase.GetFillColor(chart, 0);

			// Selection brush should override point color when data point is selected
			Assert.NotNull(selectedFillColor);
			Assert.Equal(selectionBrush, selectedFillColor);

			// Verify that unselected data point uses point color, not selection brush
			var unselectedFillColor = PyramidChartBase.GetFillColor(chart, 1);
			Assert.NotNull(unselectedFillColor);
			Assert.NotEqual(selectionBrush, unselectedFillColor);
			Assert.Equal(data[1].PointColor, unselectedFillColor);
		}

		#endregion
	}
}
