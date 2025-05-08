using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.Toolkit.UnitTest.Charts
{
	public class ChartFeatureBehaviourUnitTest : BaseUnitTest
	{
		#region ZoomPanbehaviour methods

		[Fact]
		public void ZoomIn_ValidatesZoomIncreases()
		{
			var chart = new SfCartesianChart();
			var xAxis = new CategoryAxis();
			var yAxis = new NumericalAxis();
			chart._chartArea._xAxes.Add(xAxis);
			chart._chartArea._yAxes.Add(yAxis);
			var zoomPanBehavior = new ChartZoomPanBehavior
			{
				Chart = chart
			};

			zoomPanBehavior.ZoomIn();

			var result = GetPrivateField(zoomPanBehavior, "_cumulativeZoomLevel");

			Assert.NotNull(result);
			Assert.Equal(1.25, (double)result);
			Assert.True(xAxis.ZoomFactor < 1);
			Assert.True(xAxis.ZoomPosition > 0);
			Assert.True(yAxis.ZoomFactor < 1);
			Assert.True(yAxis.ZoomPosition > 0);
		}

		[Fact]
		public void ZoomOut_ValidatesZoomDecreases()
		{
			var chart = new SfCartesianChart();
			var xAxis = new CategoryAxis();
			var yAxis = new NumericalAxis();
			chart._chartArea._xAxes.Add(xAxis);
			chart._chartArea._yAxes.Add(yAxis);
			var zoomPanBehavior = new ChartZoomPanBehavior
			{
				Chart = chart
			};
			SetPrivateField(zoomPanBehavior, "_cumulativeZoomLevel", 0.5);

			zoomPanBehavior.ZoomOut();
			var result = GetPrivateField(zoomPanBehavior, "_cumulativeZoomLevel");

			Assert.NotNull(result);
			Assert.Equal(0.5, (double)result);
			Assert.Equal(1, xAxis.ZoomFactor);
			Assert.Equal(0, xAxis.ZoomPosition);
			Assert.Equal(1, yAxis.ZoomFactor);
			Assert.Equal(0, xAxis.ZoomPosition);
		}

		[Fact]
		public void Reset_ValidatesZoomResets()
		{
			var chart = new SfCartesianChart();
			var xAxis = new CategoryAxis();
			var yAxis = new NumericalAxis();
			chart._chartArea._axisLayout.HorizontalAxes.Add(xAxis);
			chart._chartArea._axisLayout.VerticalAxes.Add(yAxis);
			var zoomPanBehavior = new ChartZoomPanBehavior
			{
				Chart = chart
			};
			SetPrivateField(zoomPanBehavior, "_cumulativeZoomLevel", 0.5);

			zoomPanBehavior.Reset();

			Assert.Equal(1.0, GetPrivateField(zoomPanBehavior, "_cumulativeZoomLevel"));
			Assert.Equal(1, xAxis.ZoomFactor);
			Assert.Equal(0, xAxis.ZoomPosition);
			Assert.Equal(1, yAxis.ZoomFactor);
			Assert.Equal(0, yAxis.ZoomPosition);
		}

		[Theory]
		[InlineData(0.1, 0.5)]
		[InlineData(-0.1, 1.1)]
		public void ZoomByRange_ValidatesZoomPositionAndFactor(double start, double end)
		{
			var chart = new SfCartesianChart();
			var xAxis = new CategoryAxis() { ActualRange = new DoubleRange(0.0, 1.0), Area = chart._chartArea };
			chart._chartArea._axisLayout.HorizontalAxes.Add(xAxis);
			var zoomPanBehavior = new ChartZoomPanBehavior
			{
				Chart = chart
			};

			zoomPanBehavior.ZoomByRange(xAxis, start, end);

			var expectedStart = Math.Max(0.0, Math.Min(1.0, start));
			var expectedEnd = Math.Max(0.0, Math.Min(1.0, end));
			Assert.Equal((expectedStart - xAxis.ActualRange.Start) / xAxis.ActualRange.Delta, xAxis.ZoomPosition);
			Assert.Equal((expectedEnd - expectedStart) / xAxis.ActualRange.Delta, xAxis.ZoomFactor);
		}

		[Theory]
		[InlineData(0.0, 1.0)]
		[InlineData(0.5, 0.25)]
		public void ZoomToFactor_ValidatesZoomPositionAndFactor(double zoomPosition, double zoomFactor)
		{
			var chart = new SfCartesianChart();
			var xAxis = new CategoryAxis()
			{
				ZoomPosition = 0.0,
				ZoomFactor = 1.0,
				Area = chart._chartArea
			};

			chart._chartArea._axisLayout.HorizontalAxes.Add(xAxis);
			var zoomPanBehavior = new ChartZoomPanBehavior
			{
				Chart = chart
			};

			zoomPanBehavior.ZoomToFactor(xAxis, zoomPosition, zoomFactor);

			Assert.Equal(zoomPosition, xAxis.ZoomPosition);
			Assert.Equal(zoomFactor, xAxis.ZoomFactor);
		}

		[Theory]
		[InlineData(0.5, 1.0)]
		[InlineData(0.5, 0.25)]
		public void ZoomToFactor_ZoomFactor(double zoomPosition, double zoomFactor)
		{
			var chart = new SfCartesianChart();
			var xAxis = new CategoryAxis()
			{
				ZoomPosition = 0.0,
				ZoomFactor = 1.0,
				Area = chart._chartArea
			};

			chart._chartArea._axisLayout.HorizontalAxes.Add(xAxis);
			var zoomPanBehavior = new ChartZoomPanBehavior
			{
				Chart = chart
			};

			zoomPanBehavior.ZoomToFactor(zoomFactor);

			Assert.Equal(zoomPosition, xAxis.ZoomPosition);
			Assert.Equal(zoomFactor, xAxis.ZoomFactor);
		}

		[Fact]
		public void SetTouchHandled_EnablesPanning_SetsIsHandledToTrue()
		{
			var chart = new SfCartesianChart();
			var zoomPan = new ChartZoomPanBehavior
			{
				EnablePanning = true
			};

			zoomPan.SetTouchHandled(chart);

			Assert.False(chart.IsHandled);
		}

		[Fact]
		public void OnDoubleTap_WithinClipRect_CallsOnDoubleTapWithParameters()
		{
			var chart = new SfCartesianChart();
			var xAxis = new CategoryAxis();
			var yAxis = new NumericalAxis();
			chart._chartArea._axisLayout.HorizontalAxes.Add(xAxis);
			chart._chartArea._axisLayout.VerticalAxes.Add(yAxis);
			var zoomPanBehavior = new ChartZoomPanBehavior
			{
				Chart = chart
			};
			var clipRect = new Rect(0, 0, 500, 500);
			((IChart)chart).ActualSeriesClipRect = clipRect;

			zoomPanBehavior.OnDoubleTap(chart, 250, 250);

			Assert.Equal(2.5, GetPrivateField(zoomPanBehavior, "_cumulativeZoomLevel"));
			Assert.True(xAxis.ZoomFactor < 1);
			Assert.True(xAxis.ZoomPosition > 0);
			Assert.True(yAxis.ZoomFactor < 1);
			Assert.True(yAxis.ZoomPosition > 0);
		}

		[Theory]
		[InlineData(GestureStatus.Started, true)]
		[InlineData(GestureStatus.Completed, false)]
		public void OnPinchStateChanged_ActivatesPinchZoom(GestureStatus status, bool isPinchZooming)
		{
			var handler = new ChartZoomPanBehavior();
			var chart = new SfCartesianChart();
			handler.EnablePinchZooming = true;
			var clipRect = new Rect(0, 0, 500, 500);
			((IChart)chart).ActualSeriesClipRect = clipRect;

			var location = new Point(100, 100);
			var angle = 45;
			var scale = 1.5f;

			handler.OnPinchStateChanged(chart, status, location, angle, scale);

			Assert.Equal(isPinchZooming, GetPrivateField(handler, "_isPinchZoomingActivated"));
		}

		[Fact]
		public void OnMouseWeelChanged_Test()
		{
			var handler = new ChartZoomPanBehavior();
			var chart = new SfCartesianChart();
			handler.Chart = chart;
			var xAxis = new CategoryAxis();
			var yAxis = new NumericalAxis();
			chart._chartArea._xAxes.Add(xAxis);
			chart._chartArea._yAxes.Add(yAxis);
			handler.EnablePinchZooming = true;
			var clipRect = new Rect(0, 0, 500, 500);
			((IChart)chart).ActualSeriesClipRect = clipRect;

			var location = new Point(100, 100);
			var angle = 50;

			handler.OnMouseWheelChanged(chart, location, angle);

			Assert.True(xAxis.ZoomFactor < 1);
			Assert.True(xAxis.ZoomPosition > 0);
			Assert.True(yAxis.ZoomFactor < 1);
			Assert.True(yAxis.ZoomPosition > 0);
		}

		#endregion
	}
}
