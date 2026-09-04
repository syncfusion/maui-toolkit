using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.ControlsGallery.Samples.CartesianChart.Annotation
{
	/// <summary>
	/// Helper struct to store plot area bounds in axis values.
	/// </summary>
	internal struct PlotAreaBounds
	{
		public double Left { get; set; }
		public double Right { get; set; }
		public double Top { get; set; }
		public double Bottom { get; set; }
	}

	/// <summary>
	/// Helper class for draggable annotation utilities.
	/// </summary>
	internal static class DraggableAnnotationHelper
	{
		/// <summary>
		/// Calculates plot area bounds in axis values.
		/// </summary>
		public static PlotAreaBounds GetPlotAreaBounds(Syncfusion.Maui.Toolkit.Charts.SfCartesianChart chart, ChartAxis xAxis, RangeAxisBase yAxis)
		{
			return new PlotAreaBounds
			{
				Left = chart.PointToValue(xAxis, (float)chart.SeriesBounds.Left, (float)chart.SeriesBounds.Top),
				Right = chart.PointToValue(xAxis, (float)chart.SeriesBounds.Right, (float)chart.SeriesBounds.Top),
				Top = chart.PointToValue(yAxis, (float)chart.SeriesBounds.Left, (float)chart.SeriesBounds.Top),
				Bottom = chart.PointToValue(yAxis, (float)chart.SeriesBounds.Left, (float)chart.SeriesBounds.Bottom)
			};
		}

		/// <summary>
		/// Clamps annotation coordinates to plot area bounds.
		/// </summary>
		public static void ClampCoordinatesToBounds(ref double x1, ref double x2, ref double y1, ref double y2, PlotAreaBounds bounds)
		{
			// Clamp X positions
			if (x1 < bounds.Left)
			{
				var offset = bounds.Left - x1;
				x1 = bounds.Left;
				x2 += offset;
			}
			if (x2 > bounds.Right)
			{
				var offset = x2 - bounds.Right;
				x2 = bounds.Right;
				x1 -= offset;
			}

			// Clamp Y positions
			if (y1 < bounds.Bottom)
			{
				var offset = bounds.Bottom - y1;
				y1 = bounds.Bottom;
				y2 += offset;
			}
			if (y2 > bounds.Top)
			{
				var offset = y2 - bounds.Top;
				y2 = bounds.Top;
				y1 -= offset;
			}
		}

		/// <summary>
		/// Converts object value to double, handling multiple numeric types.
		/// </summary>
		public static double ToNumeric(object? value)
		{
			return value switch
			{
				double d => d,
				float f => f,
				int i => i,
				long l => l,
				string s => double.TryParse(s, out var result) ? result : 0d,
				_ => 0d
			};
		}

		/// <summary>
		/// Clamps a value between visible min and max with an extend buffer.
		/// </summary>
		public static double ClampToVisibleRange(double value, double visibleMin, double visibleMax, double extend)
		{
			if (value <= visibleMin)
				return visibleMin + extend;
			if (value >= visibleMax)
				return visibleMax - extend;
			return value;
		}
	}

	public sealed class DraggableLineAnnotation : LineAnnotation
	{
		private Syncfusion.Maui.Toolkit.Charts.SfCartesianChart? _chart;
		private ChartAxis? _xAxis;
		private RangeAxisBase? _yAxis;
		private bool _isDragging;
		private double _previousAxisX;
		private double _previousAxisY;
		private bool _isDragEnabled = false;

		public void SetDragEnabled(bool enabled)
		{
			_isDragEnabled = enabled;
		}

		public void Attach(Syncfusion.Maui.Toolkit.Charts.SfCartesianChart chart)
		{
			_chart = chart;
			_xAxis = chart.XAxes.Count > 0 ? chart.XAxes[0] : null;
			_yAxis = chart.YAxes.Count > 0 ? chart.YAxes[0] : null;
		}

		protected override void OnTouchDown(float pointX, float pointY)
		{
			if (_chart is null || _xAxis is null || _yAxis is null)
			{
				return;
			}

			var axisX = _chart.PointToValue(_xAxis, pointX, pointY);
			var axisY = _chart.PointToValue(_yAxis, pointX, pointY);

			if (double.IsNaN(axisX) || double.IsInfinity(axisX) ||
				double.IsNaN(axisY) || double.IsInfinity(axisY))
			{
				return;
			}

			_previousAxisX = axisX;
			_previousAxisY = axisY;
			_isDragging = _isDragEnabled;
		}

		protected override void OnTouchMove(float pointX, float pointY)
		{
			if (!_isDragging || _chart is null || _xAxis is null || _yAxis is null)
			{
				return;
			}

			var axisX = _chart.PointToValue(_xAxis, pointX, pointY);
			var axisY = _chart.PointToValue(_yAxis, pointX, pointY);

			if (double.IsNaN(axisX) || double.IsInfinity(axisX) ||
				double.IsNaN(axisY) || double.IsInfinity(axisY))
			{
				return;
			}

			var deltaX = axisX - _previousAxisX;
			var deltaY = axisY - _previousAxisY;

			ApplyDelta(deltaX, deltaY);
			_previousAxisX = axisX;
			_previousAxisY = axisY;
		}

		protected override void OnTouchUp(float pointX, float pointY)
		{
			_isDragging = false;
		}

		private void ApplyDelta(double deltaX, double deltaY)
		{
			if (_chart is null || _xAxis is null || _yAxis is null)
			{
				return;
			}

			var newX1 = DraggableAnnotationHelper.ToNumeric(X1) + deltaX;
			var newX2 = DraggableAnnotationHelper.ToNumeric(X2) + deltaX;
			var newY1 = Y1 + deltaY;
			var newY2 = Y2 + deltaY;

			var bounds = DraggableAnnotationHelper.GetPlotAreaBounds(_chart, _xAxis, _yAxis);
			DraggableAnnotationHelper.ClampCoordinatesToBounds(ref newX1, ref newX2, ref newY1, ref newY2, bounds);

			X1 = newX1;
			X2 = newX2;
			Y1 = newY1;
			Y2 = newY2;
		}
	}

	public sealed class DraggableVerticalLineAnnotation : VerticalLineAnnotation
	{
		private Syncfusion.Maui.Toolkit.Charts.SfCartesianChart? _chart;
		private ChartAxis? _xAxis;
		private bool _isDragging;
		private bool _isDragEnabled = false;

		public void SetDragEnabled(bool enabled)
		{
			_isDragEnabled = enabled;
		}

		public void Attach(Syncfusion.Maui.Toolkit.Charts.SfCartesianChart chart)
		{
			_chart = chart;
			_xAxis = chart.XAxes.Count > 0 ? chart.XAxes[0] : null;
		}

		protected override void OnTouchDown(float pointX, float pointY)
		{
			if (_chart is null || _xAxis is null)
			{
				return;
			}

			var linePixel = _xAxis.ValueToPoint(Convert.ToDouble(X1));
			if (double.IsNaN(linePixel))
			{
				return;
			}

			_isDragging = _isDragEnabled;
		}

		protected override void OnTouchMove(float pointX, float pointY)
		{
			if (!_isDragging || _chart is null || _xAxis is null)
			{
				return;
			}

			var newValue = _chart.PointToValue(_xAxis, pointX, pointY);
			if (double.IsNaN(newValue) || double.IsInfinity(newValue))
			{
				return;
			}

			X1 = DraggableAnnotationHelper.ClampToVisibleRange(newValue, _xAxis.VisibleMinimum, _xAxis.VisibleMaximum, 0.1);
		}

		protected override void OnTouchUp(float pointX, float pointY)
		{
			_isDragging = false;
		}
	}

	public sealed class DraggableHorizontalLineAnnotation : HorizontalLineAnnotation
	{
		private Syncfusion.Maui.Toolkit.Charts.SfCartesianChart? _chart;
		private RangeAxisBase? _yAxis;
		private bool _isDragging;
		private bool _isDragEnabled = false;

		public void SetDragEnabled(bool enabled)
		{
			_isDragEnabled = enabled;
		}

		public void Attach(Syncfusion.Maui.Toolkit.Charts.SfCartesianChart chart)
		{
			_chart = chart;
			_yAxis = chart.YAxes.Count > 0 ? chart.YAxes[0] : null;
		}

		protected override void OnTouchDown(float pointX, float pointY)
		{
			if (_chart is null || _yAxis is null)
			{
				return;
			}

			var linePixel = _yAxis.ValueToPoint(Y1);
			if (double.IsNaN(linePixel))
			{
				return;
			}

			_isDragging = _isDragEnabled;
		}

		protected override void OnTouchMove(float pointX, float pointY)
		{
			if (!_isDragging || _chart is null || _yAxis is null)
			{
				return;
			}

			var newValue = _chart.PointToValue(_yAxis, pointX, pointY);
			if (double.IsNaN(newValue) || double.IsInfinity(newValue))
			{
				return;
			}

			Y1 = DraggableAnnotationHelper.ClampToVisibleRange(newValue, _yAxis.VisibleMinimum, _yAxis.VisibleMaximum, 2);
		}

		protected override void OnTouchUp(float pointX, float pointY)
		{
			_isDragging = false;
		}
	}

	public sealed class DraggableRectangleAnnotation : RectangleAnnotation
	{
		private Syncfusion.Maui.Toolkit.Charts.SfCartesianChart? _chart;
		private ChartAxis? _xAxis;
		private RangeAxisBase? _yAxis;
		private bool _isDragging;
		private double _previousAxisX;
		private double _previousAxisY;
		private bool _isDragEnabled = false;

		public void SetDragEnabled(bool enabled)
		{
			_isDragEnabled = enabled;
		}

		public void Attach(Syncfusion.Maui.Toolkit.Charts.SfCartesianChart chart)
		{
			_chart = chart;
			_xAxis = chart.XAxes.Count > 0 ? chart.XAxes[0] : null;
			_yAxis = chart.YAxes.Count > 0 ? chart.YAxes[0] : null;
		}

		protected override void OnTouchDown(float pointX, float pointY)
		{
			if (_chart is null || _xAxis is null || _yAxis is null)
			{
				return;
			}

			var axisX = _chart.PointToValue(_xAxis, pointX, pointY);
			var axisY = _chart.PointToValue(_yAxis, pointX, pointY);

			if (double.IsNaN(axisX) || double.IsInfinity(axisX) ||
				double.IsNaN(axisY) || double.IsInfinity(axisY))
			{
				return;
			}

			_previousAxisX = axisX;
			_previousAxisY = axisY;
			_isDragging = _isDragEnabled;
		}

		protected override void OnTouchMove(float pointX, float pointY)
		{
			if (!_isDragging || _chart is null || _xAxis is null || _yAxis is null)
			{
				return;
			}

			var axisX = _chart.PointToValue(_xAxis, pointX, pointY);
			var axisY = _chart.PointToValue(_yAxis, pointX, pointY);

			if (double.IsNaN(axisX) || double.IsInfinity(axisX) ||
				double.IsNaN(axisY) || double.IsInfinity(axisY))
			{
				return;
			}

			var deltaX = axisX - _previousAxisX;
			var deltaY = axisY - _previousAxisY;

			ApplyDelta(deltaX, deltaY);
			_previousAxisX = axisX;
			_previousAxisY = axisY;
		}

		protected override void OnTouchUp(float pointX, float pointY)
		{
			_isDragging = false;
		}

		private void ApplyDelta(double deltaX, double deltaY)
		{
			if (_chart is null || _xAxis is null || _yAxis is null)
			{
				return;
			}

			var newX1 = DraggableAnnotationHelper.ToNumeric(X1) + deltaX;
			var newX2 = DraggableAnnotationHelper.ToNumeric(X2) + deltaX;
			var newY1 = Y1 + deltaY;
			var newY2 = Y2 + deltaY;

			var bounds = DraggableAnnotationHelper.GetPlotAreaBounds(_chart, _xAxis, _yAxis);
			DraggableAnnotationHelper.ClampCoordinatesToBounds(ref newX1, ref newX2, ref newY1, ref newY2, bounds);

			X1 = newX1;
			X2 = newX2;
			Y1 = newY1;
			Y2 = newY2;
		}
	}

	public sealed class DraggableEllipseAnnotation : EllipseAnnotation
	{
		private Syncfusion.Maui.Toolkit.Charts.SfCartesianChart? _chart;
		private ChartAxis? _xAxis;
		private RangeAxisBase? _yAxis;
		private bool _isDragging;
		private double _previousAxisX;
		private double _previousAxisY;
		private bool _isDragEnabled = false;

		public void SetDragEnabled(bool enabled)
		{
			_isDragEnabled = enabled;
		}

		public void Attach(Syncfusion.Maui.Toolkit.Charts.SfCartesianChart chart)
		{
			_chart = chart;
			_xAxis = chart.XAxes.Count > 0 ? chart.XAxes[0] : null;
			_yAxis = chart.YAxes.Count > 0 ? chart.YAxes[0] : null;
		}

		protected override void OnTouchDown(float pointX, float pointY)
		{
			if (_chart is null || _xAxis is null || _yAxis is null)
			{
				return;
			}

			var axisX = _chart.PointToValue(_xAxis, pointX, pointY);
			var axisY = _chart.PointToValue(_yAxis, pointX, pointY);

			if (double.IsNaN(axisX) || double.IsInfinity(axisX) ||
				double.IsNaN(axisY) || double.IsInfinity(axisY))
			{
				return;
			}

			_previousAxisX = axisX;
			_previousAxisY = axisY;
			_isDragging = _isDragEnabled;
		}

		protected override void OnTouchMove(float pointX, float pointY)
		{
			if (!_isDragging || _chart is null || _xAxis is null || _yAxis is null)
			{
				return;
			}

			var axisX = _chart.PointToValue(_xAxis, pointX, pointY);
			var axisY = _chart.PointToValue(_yAxis, pointX, pointY);

			if (double.IsNaN(axisX) || double.IsInfinity(axisX) ||
				double.IsNaN(axisY) || double.IsInfinity(axisY))
			{
				return;
			}

			var deltaX = axisX - _previousAxisX;
			var deltaY = axisY - _previousAxisY;

			ApplyDelta(deltaX, deltaY);
			_previousAxisX = axisX;
			_previousAxisY = axisY;
		}

		protected override void OnTouchUp(float pointX, float pointY)
		{
			_isDragging = false;
		}

		private void ApplyDelta(double deltaX, double deltaY)
		{
			if (_chart is null || _xAxis is null || _yAxis is null)
			{
				return;
			}

			var newX1 = DraggableAnnotationHelper.ToNumeric(X1) + deltaX;
			var newX2 = DraggableAnnotationHelper.ToNumeric(X2) + deltaX;
			var newY1 = Y1 + deltaY;
			var newY2 = Y2 + deltaY;

			var bounds = DraggableAnnotationHelper.GetPlotAreaBounds(_chart, _xAxis, _yAxis);
			DraggableAnnotationHelper.ClampCoordinatesToBounds(ref newX1, ref newX2, ref newY1, ref newY2, bounds);

			X1 = newX1;
			X2 = newX2;
			Y1 = newY1;
			Y2 = newY2;
		}
	}

	public sealed class DraggableTextAnnotation : TextAnnotation
	{
		private Syncfusion.Maui.Toolkit.Charts.SfCartesianChart? _chart;
		private ChartAxis? _xAxis;
		private RangeAxisBase? _yAxis;
		private bool _isDragging;
		private double _previousAxisX;
		private double _previousAxisY;
		private bool _isDragEnabled = false;

		public void SetDragEnabled(bool enabled)
		{
			_isDragEnabled = enabled;
		}

		public void Attach(Syncfusion.Maui.Toolkit.Charts.SfCartesianChart chart)
		{
			_chart = chart;
			_xAxis = chart.XAxes.Count > 0 ? chart.XAxes[0] : null;
			_yAxis = chart.YAxes.Count > 0 ? chart.YAxes[0] : null;
		}

		protected override void OnTouchDown(float pointX, float pointY)
		{
			if (_chart is null || _xAxis is null || _yAxis is null)
			{
				return;
			}

			var axisX = _chart.PointToValue(_xAxis, pointX, pointY);
			var axisY = _chart.PointToValue(_yAxis, pointX, pointY);

			if (double.IsNaN(axisX) || double.IsInfinity(axisX) ||
				double.IsNaN(axisY) || double.IsInfinity(axisY))
			{
				return;
			}

			_previousAxisX = axisX;
			_previousAxisY = axisY;
			_isDragging = _isDragEnabled;
		}

		protected override void OnTouchMove(float pointX, float pointY)
		{
			if (!_isDragging || _chart is null || _xAxis is null || _yAxis is null)
			{
				return;
			}

			var axisX = _chart.PointToValue(_xAxis, pointX, pointY);
			var axisY = _chart.PointToValue(_yAxis, pointX, pointY);

			if (double.IsNaN(axisX) || double.IsInfinity(axisX) ||
				double.IsNaN(axisY) || double.IsInfinity(axisY))
			{
				return;
			}

			var deltaX = axisX - _previousAxisX;
			var deltaY = axisY - _previousAxisY;

			ApplyDelta(deltaX, deltaY);
			_previousAxisX = axisX;
			_previousAxisY = axisY;
		}

		protected override void OnTouchUp(float pointX, float pointY)
		{
			_isDragging = false;
		}

		private void ApplyDelta(double deltaX, double deltaY)
		{
			if (_chart is null || _xAxis is null || _yAxis is null)
			{
				return;
			}

			double newX1 = DraggableAnnotationHelper.ToNumeric(X1) + deltaX;
			double newY1 = Y1 + deltaY;
			X1 = DraggableAnnotationHelper.ClampToVisibleRange(newX1, _xAxis.VisibleMinimum, _xAxis.VisibleMaximum, 0.3);
			Y1 = DraggableAnnotationHelper.ClampToVisibleRange(newY1, _yAxis.VisibleMinimum, _yAxis.VisibleMaximum, 0.5);
		}
	}
}

