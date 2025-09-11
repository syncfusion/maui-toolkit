using Syncfusion.Maui.Toolkit.Graphics.Internals;
using System.Collections;

namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// Represents a segment of the <see cref="SplineAreaSeries"/>.
	/// </summary>
	public partial class SplineAreaSegment : AreaSegment
	{
		#region Fields

		double _minY, _maxY;

		#endregion

		#region Internal Properties

		internal double[] XVal { get; set; }

		internal double[] YVal { get; set; }

		internal double[] ControlStartX { get; set; }

		internal double[] ControlStartY { get; set; }

		internal double[] ControlEndX { get; set; }

		internal double[] ControlEndY { get; set; }

		internal List<float> StartControlPoints { get; set; }

		internal List<float> EndControlPoints { get; set; }

		internal List<float> StrokeControlStartPoints { get; set; }

		internal List<float> StrokeControlEndPoints { get; set; }

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="SplineAreaSegment"/> class.
		/// </summary>
		public SplineAreaSegment()
		{
			YVal = [];
			XVal = [];
			ControlStartX = [];
			ControlStartY = [];
			ControlEndX = [];
			ControlEndY = [];
			StartControlPoints = [];
			EndControlPoints = [];
			StrokeControlStartPoints = [];
			StrokeControlEndPoints = [];
		}

		#endregion

		#region Methods

		/// <inheritdoc/>
		protected internal override void OnLayout()
		{
			if (XVal == null)
			{
				return;
			}

			CalculateInteriorPoints();

			if (HasStroke)
			{
				CalculateStrokePoints();
			}
		}

		/// <inheritdoc/>
		protected internal override void Draw(ICanvas canvas)
		{
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

			DrawPath(canvas, FillPoints, StrokePoints);
		}

		/// <summary>
		/// Converts the data points to corresponding screen points for rendering the spline area segment.
		/// </summary>
		internal override void SetData(IList? xValues, IList? yValues, IList? startControlPointsX, IList? startControlPointsY, IList? endControlPointsX, IList? endControlPointsY)
		{
			int count = 0;

			if (xValues != null)
			{
				count = xValues.Count;
			}

			YVal = new double[count];
			XVal = new double[count];
			ControlStartX = new double[count];
			ControlStartY = new double[count];
			ControlEndX = new double[count];
			ControlEndY = new double[count];

			xValues?.CopyTo(XVal, 0);
			yValues?.CopyTo(YVal, 0);
			startControlPointsX?.CopyTo(ControlStartX, 0);
			startControlPointsY?.CopyTo(ControlStartY, 0);
			endControlPointsX?.CopyTo(ControlEndX, 0);
			endControlPointsY?.CopyTo(ControlEndY, 0);

			UpdateRange();

			XValues = XVal;
			YValues = YVal;
		}

		/// <summary>
		/// Updates the range with control points
		/// </summary>
		void UpdateRange()
		{
			if (Series is CartesianSeries series && series.ActualYAxis != null)
			{
				_minY = YVal.Min();
				_minY = double.IsNaN(_minY) ? YVal.Length > 0 ? YVal.Where(e => !double.IsNaN(e)).DefaultIfEmpty().Min() : 0 : _minY;
				_maxY = YVal.Max();

				double startControlMin = ControlStartY.Min();
				double startControlMax = ControlStartY.Max();

				double endControlMin = ControlEndY.Min();
				double endControlMax = ControlEndY.Max();

				if (_maxY < startControlMax)
				{
					_maxY = startControlMax;
				}

				if (_minY > startControlMin)
				{
					_minY = startControlMin;
				}

				if (_maxY < endControlMax)
				{
					_maxY = endControlMax;
				}

				if (_minY > endControlMin)
				{
					_minY = endControlMin;
				}

				series.XRange += new DoubleRange(XVal.Min(), XVal.Max());
				series.YRange += new DoubleRange(_minY, _maxY);
			}
		}

		/// <summary>
		/// Calculate interior points.
		/// </summary>
		void CalculateInteriorPoints()
		{
			if (Series is not CartesianSeries cartesian || cartesian.ActualXAxis == null)
			{
				return;
			}

			var xAxis = cartesian.ActualXAxis;
			var crossingValue = double.IsNaN(xAxis.RenderingCrossesValue) ? cartesian.GetAxisCrossingValue(xAxis) : xAxis.RenderingCrossesValue;
			var count = XVal.Length;
			FillPoints = [];
			StartControlPoints = [];
			EndControlPoints = [];

			double xValue = XVal[0], startX, startY, endX, endY;

			FillPoints.Add(cartesian.TransformToVisibleX(xValue, 0));
			FillPoints.Add(cartesian.TransformToVisibleY(xValue, crossingValue));

			double yValue;
			for (int i = 0; i < count - 1; i++)
			{
				xValue = XVal[i];
				yValue = YVal[i];

				FillPoints.Add(cartesian.TransformToVisibleX(xValue, yValue));
				FillPoints.Add(cartesian.TransformToVisibleY(xValue, yValue));

				startX = ControlStartX[i];
				startY = ControlStartY[i];
				StartControlPoints.Add(cartesian.TransformToVisibleX(startX, startY));
				StartControlPoints.Add(cartesian.TransformToVisibleY(startX, startY));

				endX = ControlEndX[i];
				endY = ControlEndY[i];
				EndControlPoints.Add(cartesian.TransformToVisibleX(endX, endY));
				EndControlPoints.Add(cartesian.TransformToVisibleY(endX, endY));
			}

			xValue = XVal[count - 1];
			yValue = YVal[count - 1];
			FillPoints.Add(cartesian.TransformToVisibleX(xValue, yValue));
			FillPoints.Add(cartesian.TransformToVisibleY(xValue, yValue));
			FillPoints.Add(cartesian.TransformToVisibleX(xValue, 0));
			FillPoints.Add(cartesian.TransformToVisibleY(xValue, crossingValue));
		}

		void CalculateStrokePoints()
		{
			if (Series is CartesianSeries series && SeriesView != null && series.ActualYAxis != null)
			{
				float x, y, ControlStartXVal, ControlStartYVal, ControlEndXVal, ControlEndYVal;
				StrokePoints = [];
				StrokeControlStartPoints = [];
				StrokeControlEndPoints = [];
				int segsCount = series._segments.Count;
				var halfStrokeWidth = (float)StrokeWidth / 2;
				double yValue, xValue, startX, startY, endX, endY;

				var start = series.ActualYAxis.VisibleRange.Start;

				var count = XVal.Length;

				for (int i = 0; i < count; i++)
				{
					yValue = YVal[i];
					xValue = XVal[i];
					x = series.TransformToVisibleX(xValue, yValue);
					y = series.TransformToVisibleY(xValue, yValue);

					startX = ControlStartX[i];
					startY = ControlStartY[i];
					ControlStartXVal = series.TransformToVisibleX(startX, startY);
					ControlStartYVal = series.TransformToVisibleY(startX, startY);

					endX = ControlEndX[i];
					endY = ControlEndY[i];
					ControlEndXVal = series.TransformToVisibleX(endX, endY);
					ControlEndYVal = series.TransformToVisibleY(endX, endY);

					StrokePoints.Add(x);
					y += yValue >= 0 ? halfStrokeWidth : -halfStrokeWidth;
					StrokePoints.Add(y);

					StrokeControlStartPoints.Add(ControlStartXVal);
					ControlStartYVal += startY >= 0 ? halfStrokeWidth : -halfStrokeWidth;
					StrokeControlStartPoints.Add(ControlStartYVal);
					StrokeControlEndPoints.Add(ControlEndXVal);
					ControlEndYVal += endY >= 0 ? halfStrokeWidth : -halfStrokeWidth;
					StrokeControlEndPoints.Add(ControlEndYVal);
				}

				if (segsCount == 1 || series._segments.Last() == this)
				{
					xValue = XVal[count - 1];
					x = series.TransformToVisibleX(xValue, start);
					y = series.TransformToVisibleY(xValue, start);
					y += start >= 0 ? halfStrokeWidth : -halfStrokeWidth;
					StrokePoints.Add(x);
					StrokePoints.Add(y);
				}
			}
		}

		void DrawPath(ICanvas canvas, List<float>? fillPoints, List<float>? strokePoints)
		{
			if (Series == null || fillPoints == null)
			{
				return;
			}

			var path = new PathF();

			if (Series.CanAnimate())
			{
				AnimateSeriesClipRect(canvas, Series.AnimationValue);
			}

			path.MoveTo(fillPoints[0], fillPoints[1]);
			path.LineTo(fillPoints[2], fillPoints[3]);

			if (StartControlPoints != null && EndControlPoints != null)
			{
				for (int i = 0; i < StartControlPoints?.Count; i++)
				{
					var endPointX = fillPoints[i + 4];
					var endPointY = fillPoints[i + 5];

					var controlStartX = StartControlPoints[i];
					var controlStartY = StartControlPoints[i + 1];
					var controlEndX = EndControlPoints[i];
					var controlEndY = EndControlPoints[i + 1];

					path.CurveTo(controlStartX, controlStartY, controlEndX, controlEndY, endPointX, endPointY);
					i++;
				}
			}

			var fillPointsCount = fillPoints.Count;
			path.LineTo(fillPoints[fillPointsCount - 2], fillPoints[fillPointsCount - 1]);
			path.LineTo(fillPoints[0], fillPoints[1]);

			canvas.SetFillPaint(Fill, path.Bounds);
			canvas.FillPath(path);

			if (HasStroke && strokePoints != null)
			{
				path = new PathF();

				path.MoveTo(strokePoints[0], strokePoints[1]);

				if (StartControlPoints != null && EndControlPoints != null)
				{
					for (int i = 0; i < StartControlPoints?.Count; i++)
					{
						var endPointX = strokePoints[i + 2];
						var endPointY = strokePoints[i + 3];

						var controlStartX = StrokeControlStartPoints[i];
						var controlStartY = StrokeControlStartPoints[i + 1];
						var controlEndX = StrokeControlEndPoints[i];
						var controlEndY = StrokeControlEndPoints[i + 1];

						path.CurveTo(controlStartX, controlStartY, controlEndX, controlEndY, endPointX, endPointY);
						i++;
					}
				}

				canvas.DrawPath(path);
			}
		}

		#endregion
	}
}
