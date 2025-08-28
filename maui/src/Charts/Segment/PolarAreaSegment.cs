using Syncfusion.Maui.Toolkit.Graphics.Internals;
using System.Collections;

namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// Represents a segment of the <see cref="PolarAreaSeries"/> chart.
	/// </summary>
	public partial class PolarAreaSegment : ChartSegment, IMarkerDependentSegment
	{
		#region Fields

		double[]? _xValues { get; set; }
		double[]? _yValues { get; set; }
		int _pointsCount;
		PathF? _path;
		List<float>? _segmentFillPoints, _segmentStrokePoints;

		#endregion

		#region Methods

		#region Interface Implementation

		void IMarkerDependentSegment.DrawMarker(ICanvas canvas)
		{
			if (Series is IMarkerDependent series && _segmentFillPoints != null)
			{
				var marker = series.MarkerSettings;
				var fill = marker.Fill;
				var type = marker.Type;

				for (int i = 2; i < _segmentFillPoints.Count - 3; i += 2)
				{
					var rect = new Rect(_segmentFillPoints[i] - (marker.Width / 2), _segmentFillPoints[i + 1] - (marker.Height / 2), marker.Width, marker.Height);

					canvas.SetFillPaint(fill == default(Brush) ? Fill : fill, rect);

					series.DrawMarker(canvas, Index, type, rect);
				}
			}
		}

		Rect IMarkerDependentSegment.GetMarkerRect(double markerWidth, double markerHeight, int tooltipIndex)
		{
			Rect rect = new Rect();

			if (Series != null && _segmentFillPoints != null)
			{
				if (_xValues?.Length > tooltipIndex)
				{
					var xIndex = (2 * tooltipIndex) + 2;
					rect = new Rect(_segmentFillPoints[xIndex] - (markerWidth / 2), _segmentFillPoints[xIndex + 1] - (markerHeight / 2), markerWidth, markerHeight);
				}
			}

			return rect;
		}

		#endregion

		#region Internal Method

		/// <summary>
		/// Converts the data points to corresponding screen points for rendering the polar area segment.
		/// </summary>
		internal override void SetData(IList xDatas, IList yDatas)
		{
			if (Series is PolarSeries series && series.ActualYAxis != null)
			{
				_pointsCount = xDatas.Count;
				_xValues = new double[_pointsCount];
				_yValues = new double[_pointsCount];
				xDatas.CopyTo(_xValues, 0);
				yDatas.CopyTo(_yValues, 0);
				var yMin = _yValues.Min();
				yMin = double.IsNaN(yMin) ? _yValues.Length > 0 ? _yValues.Where(e => !double.IsNaN(e)).DefaultIfEmpty().Min() : 0 : yMin;
				series.XRange += new DoubleRange(_xValues.Min(), _xValues.Max());
				series.YRange += new DoubleRange(yMin, _yValues.Max());
			}
		}

		#endregion

		#region Protected Method

		/// <inheritdoc/>
		protected internal override void OnLayout()
		{
			if (_xValues == null)
			{
				return;
			}

			_segmentFillPoints = GenerateInteriorPoints(1);

			if (HasStroke)
			{
				_segmentStrokePoints = GenerateStrokePoints(1);
			}
		}

		internal override void OnDataLabelLayout()
		{
			if (Series is PolarAreaSeries series && series != null && series.LabelTemplate != null)
			{
				var dataLabeSettings = series.DataLabelSettings;

				List<double> xValues = series.GetXValues()!;

				if (dataLabeSettings == null || xValues == null || series.YValues == null)
				{
					return;
				}

				ChartDataLabelStyle labelStyle = dataLabeSettings.LabelStyle;

				for (int i = 0; i < series.PointsCount; i++)
				{
					double x = xValues[i], y = series.YValues[i];

					if (double.IsNaN(y))
					{
						continue;
					}

					series.CalculateDataPointPosition(i, ref x, ref y);
					PointF labelPoint = new PointF((float)x, (float)y);
					Index = i;
					LabelContent = series.GetLabelContent(series.YValues[i], series.SumOfValues(series.YValues));

					if (DataLabels != null && DataLabels.Count > i)
					{
						ChartDataLabel dataLabel = DataLabels[i];

						dataLabel.LabelStyle = dataLabeSettings.LabelStyle;
						dataLabel.Background = dataLabeSettings.LabelStyle.Background;
						dataLabel.Index = i;
						dataLabel.Item = series.ActualData?[i];
						dataLabel.Label = LabelContent ?? string.Empty;

						LabelPositionPoint = series.CalculateDataLabelPoint(this, labelPoint, labelStyle);
						dataLabel.XPosition = LabelPositionPoint.X;
						dataLabel.YPosition = LabelPositionPoint.Y;
					}
				}
			}
		}

		/// <inheritdoc/>
		protected internal override void Draw(ICanvas canvas)
		{
			if (Empty || Series == null)
			{
				return;
			}

			var fillPoints = _segmentFillPoints;
			var strokePoints = _segmentStrokePoints;

			if (Series.CanAnimate())
			{
				fillPoints = GenerateInteriorPoints(Series.AnimationValue);

				if (HasStroke)
				{
					strokePoints = GenerateStrokePoints(Series.AnimationValue);
				}
			}

			canvas.Alpha = Opacity;

			if (HasStroke)
			{
				canvas.StrokeSize = (float)StrokeWidth;
				canvas.StrokeColor = Stroke.ToColor();
			}

			if (StrokeDashArray != null)
			{
				canvas.StrokeDashPattern = StrokeDashArray.ToFloatArray();
			}

			DrawPath(canvas, fillPoints, strokePoints);
		}

		#endregion

		#region Private Methods

		void DrawPath(ICanvas canvas, List<float>? fillPoints, List<float>? strokePoints)
		{
			_path = new PathF();

			if (fillPoints != null)
			{
				PolarAreaSegment.DrawAreaPath(ref _path, fillPoints);

				canvas.SetFillPaint(Fill, _path.Bounds);
				canvas.FillPath(_path);
			}

			if (HasStroke && strokePoints != null)
			{
				_path = new PathF();

				PolarAreaSegment.DrawAreaPath(ref _path, strokePoints);

				canvas.DrawPath(_path);
			}
		}

		List<float> GenerateInteriorPoints(float animationValue)
		{
			var fillPoints = new List<float>();

			if (Series is not PolarSeries series)
			{
				return fillPoints;
			}

			if (series.ActualXAxis != null && series.ActualYAxis != null && _xValues != null && _yValues != null)
			{
				PointF pointF = series.TransformVisiblePoint(_xValues[0], _yValues[0], animationValue);

				fillPoints.Add(pointF.X);
				fillPoints.Add(pointF.Y);

				for (int i = 0; i < _pointsCount; i++)
				{
					PointF midPointF = series.TransformVisiblePoint(_xValues[i], _yValues[i], animationValue);
					fillPoints.Add(midPointF.X);
					fillPoints.Add(midPointF.Y);
				}

				if (series.IsClosed)
				{
					PointF endPointF = series.TransformVisiblePoint(_xValues[0], _yValues[0], animationValue);
					fillPoints.Add(endPointF.X);
					fillPoints.Add(endPointF.Y);
				}
				else
				{
					PointF endPointF = series.TransformVisiblePoint(_xValues[0], series.ActualYAxis.ActualRange.Start, animationValue);
					fillPoints.Add(endPointF.X);
					fillPoints.Add(endPointF.Y);
				}
			}

			return fillPoints;
		}

		List<float> GenerateStrokePoints(float animationValue)
		{
			var strokePoints = new List<float>();

			if (Series is not PolarSeries series)
			{
				return strokePoints;
			}

			if (series.ActualXAxis != null && series.ActualYAxis != null && _xValues != null && _yValues != null)
			{
				PointF startPoint = series.TransformVisiblePoint(_xValues[0], _yValues[0], animationValue);

				strokePoints.Add(startPoint.X);
				strokePoints.Add(startPoint.Y);

				for (int i = 1; i < _pointsCount; i++)
				{
					PointF midPoint = series.TransformVisiblePoint(_xValues[i], _yValues[i], animationValue);
					strokePoints.Add(midPoint.X);
					strokePoints.Add(midPoint.Y);
				}

				if (series.IsClosed)
				{
					PointF pointF = series.TransformVisiblePoint(_xValues[0], _yValues[0], animationValue);
					strokePoints.Add(pointF.X);
					strokePoints.Add(pointF.Y);
				}
				else
				{
					PointF pointF = series.TransformVisiblePoint(_xValues[0], series.ActualYAxis.ActualRange.Start, animationValue);
					strokePoints.Add(pointF.X);
					strokePoints.Add(pointF.Y);
				}
			}

			return strokePoints;
		}

		static void DrawAreaPath(ref PathF path, List<float> points)
		{
			for (int i = 0; i < points.Count; i++)
			{
				var x = points[i];
				var y = points[i + 1];

				if (i == 0)
				{
					path.MoveTo(x, y);
				}
				else
				{
					path.LineTo(x, y);
				}

				i++;
			}
		}

		#endregion

		#endregion
	}
}