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
	}
}
