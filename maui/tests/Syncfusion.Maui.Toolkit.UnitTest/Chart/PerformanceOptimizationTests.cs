using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.Toolkit.UnitTest.Charts
{
	public class PerformanceOptimizationTests
	{
		#region CartesianAxisLayout - GetAxisByName

		[Fact]
		public void CartesianChart_NamedAxes_AssociateCorrectly()
		{
			var chart = new SfCartesianChart();
			var xAxis = new NumericalAxis { Name = "xAxis1" };
			var yAxis = new NumericalAxis { Name = "yAxis1" };

			chart.XAxes.Add(xAxis);
			chart.YAxes.Add(yAxis);

			var series = new LineSeries
			{
				XAxisName = "xAxis1",
				YAxisName = "yAxis1"
			};
			chart.Series.Add(series);

			Assert.Equal("xAxis1", xAxis.Name);
			Assert.Equal("yAxis1", yAxis.Name);
			Assert.Equal("xAxis1", series.XAxisName);
			Assert.Equal("yAxis1", series.YAxisName);
		}

		[Fact]
		public void CartesianChart_MultipleNamedAxes_FindsCorrectAxis()
		{
			var chart = new SfCartesianChart();
			var xAxis1 = new NumericalAxis { Name = "primary" };
			var xAxis2 = new NumericalAxis { Name = "secondary" };
			var yAxis1 = new NumericalAxis { Name = "left" };
			var yAxis2 = new NumericalAxis { Name = "right" };

			chart.XAxes.Add(xAxis1);
			chart.XAxes.Add(xAxis2);
			chart.YAxes.Add(yAxis1);
			chart.YAxes.Add(yAxis2);

			var series1 = new LineSeries
			{
				XAxisName = "primary",
				YAxisName = "left"
			};
			var series2 = new LineSeries
			{
				XAxisName = "secondary",
				YAxisName = "right"
			};

			chart.Series.Add(series1);
			chart.Series.Add(series2);

			Assert.Equal(2, chart.XAxes.Count);
			Assert.Equal(2, chart.YAxes.Count);
			Assert.Equal("secondary", series2.XAxisName);
			Assert.Equal("right", series2.YAxisName);
		}

		[Fact]
		public void CartesianChart_NullAxisName_DoesNotThrow()
		{
			var chart = new SfCartesianChart();
			var xAxis = new NumericalAxis();
			var yAxis = new NumericalAxis();

			chart.XAxes.Add(xAxis);
			chart.YAxes.Add(yAxis);

			var series = new LineSeries();
			chart.Series.Add(series);

			Assert.Null(series.XAxisName);
			Assert.Null(series.YAxisName);
		}

		#endregion

		#region DataLabelSettings - LabelStyle PropertyChanged handler

		[Fact]
		public void DataLabelSettings_LabelStyle_CanBeReassigned()
		{
			var settings = new CartesianDataLabelSettings();
			var labelStyle1 = new ChartDataLabelStyle { TextColor = Colors.Red };
			var labelStyle2 = new ChartDataLabelStyle { TextColor = Colors.Blue };

			settings.LabelStyle = labelStyle1;
			Assert.Equal(Colors.Red, settings.LabelStyle.TextColor);

			settings.LabelStyle = labelStyle2;
			Assert.Equal(Colors.Blue, settings.LabelStyle.TextColor);
		}

		[Fact]
		public void DataLabelSettings_LabelStyle_ReassignmentDoesNotThrow()
		{
			var series = new ColumnSeries();
			var settings = new CartesianDataLabelSettings();
			series.DataLabelSettings = settings;

			var exception = Record.Exception(() =>
			{
				for (int i = 0; i < 10; i++)
				{
					settings.LabelStyle = new ChartDataLabelStyle
					{
						TextColor = Colors.Red,
						FontSize = 12 + i
					};
				}
			});

			Assert.Null(exception);
		}

		#endregion

		#region PolarAreaSegment - List capacity optimization

		[Fact]
		public void PolarAreaSeries_AddedToChart_DoesNotThrow()
		{
			var chart = new SfPolarChart();
			var primaryAxis = new NumericalAxis();
			var secondaryAxis = new NumericalAxis();
			chart.PrimaryAxis = primaryAxis;
			chart.SecondaryAxis = secondaryAxis;

			var series = new PolarAreaSeries();
			chart.Series.Add(series);

			Assert.Single(chart.Series);
			Assert.IsType<PolarAreaSeries>(chart.Series[0]);
		}

		[Fact]
		public void PolarAreaSeries_WithData_InitializesCorrectly()
		{
			var chart = new SfPolarChart();
			chart.PrimaryAxis = new NumericalAxis();
			chart.SecondaryAxis = new NumericalAxis();

			var series = new PolarAreaSeries
			{
				IsClosed = true
			};
			chart.Series.Add(series);

			Assert.True(series.IsClosed);
		}

		#endregion
	}
}
