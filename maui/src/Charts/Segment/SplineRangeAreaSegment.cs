using Syncfusion.Maui.Toolkit.Graphics.Internals;
using System.Collections;

namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// Represents a segment of <see cref="SplineRangeAreaSeries"/>.
	/// </summary>
	public partial class SplineRangeAreaSegment : AreaSegment, IMarkerDependentSegment
	{
		#region Fields

		double _min, _max;

		#endregion

		#region Private Properties

		double[]? HighControlStartX { get; set; }

		double[]? HighControlStartY { get; set; }

		double[]? HighControlEndX { get; set; }

		double[]? HighControlEndY { get; set; }

		double[]? LowControlStartX { get; set; }

		double[]? LowControlStartY { get; set; }

		double[]? LowControlEndX { get; set; }

		double[]? LowControlEndY { get; set; }

		List<float>? HighStartControlPoints { get; set; }

		List<float>? HighEndControlPoints { get; set; }

		List<float>? StartControlPoints { get; set; }

		List<float>? EndControlPoints { get; set; }

		List<float>? LowStartControlPoints { get; set; }

		List<float>? LowEndControlPoints { get; set; }

		#endregion

		#region Internal Properties

		internal double[]? XVal { get; set; }

		internal double[]? HighVal { get; set; }

		internal double[]? LowVal { get; set; }

		#endregion

		#region Methods

		#region Protected methods

		/// <inheritdoc/>
		protected internal override void OnLayout()
		{
			if (XVal == null)
			{
				return;
			}

			CalculateInteriorPoints();
		}

		internal override int GetDataPointIndex(float x, float y)
		{
			if (Series != null && Series.SeriesContainsPoint(new PointF(x, y)))
			{
				return Series._segments.IndexOf(this);
			}

			return -1;
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

			DrawPath(canvas, FillPoints);
		}

		internal override void OnDataLabelLayout()
		{
			if (Series is SplineRangeAreaSeries series && series.LabelTemplate is not null)
			{
				var dataLabelSettings = series.DataLabelSettings;
				ChartDataLabelStyle labelStyle = dataLabelSettings.LabelStyle;

				if (XVal == null || HighVal == null || LowVal == null || series.ActualData == null)
				{
					return;
				}

				for (int i = 0; i < XVal.Length; i++)
				{
					int highIndex = 0;

					Index = i;

					for (int j = 0; j < 2; j++)
					{
						if (j == 0)
						{
							highIndex = (2 * i) + 1;

							double x = XVal[i];
							double y = HighVal[i];

							InVisibleRange = series.IsDataInVisibleRange(x, y);
							LabelContent = dataLabelSettings.GetLabelContent(HighVal[i]);

							if (DataLabels != null && DataLabels.Count > highIndex - 1)
							{
								var dataLabel = DataLabels[highIndex - 1];

								dataLabel.LabelStyle = labelStyle;
								dataLabel.Index = i;
								dataLabel.Item = series.ActualData[i];
								dataLabel.Label = LabelContent ?? string.Empty;

								if (!InVisibleRange || IsZero)
								{
									LabelPositionPoint = new PointF(float.NaN, float.NaN);
								}
								else
								{
									series.CalculateDataPointPosition(i, ref x, ref y);
									PointF labelPoint = new PointF((float)x, (float)y);
									LabelPositionPoint = dataLabelSettings.CalculateDataLabelPoint(series, this, labelPoint, labelStyle, "HighType");
								}

								dataLabel.XPosition = LabelPositionPoint.X;
								dataLabel.YPosition = LabelPositionPoint.Y;
							}
						}
						else
						{
							int lowIndex = highIndex + 1;

							double x = XVal[i];
							double y = LowVal[i];

							InVisibleRange = series.IsDataInVisibleRange(x, y);
							LabelContent = dataLabelSettings.GetLabelContent(LowVal[i]);

							if (DataLabels != null && DataLabels.Count > lowIndex - 1)
							{
								var dataLabel = DataLabels[lowIndex - 1];

								dataLabel.LabelStyle = labelStyle;
								dataLabel.Index = i;
								dataLabel.Item = series.ActualData[i];
								dataLabel.Label = LabelContent ?? string.Empty;

								if (!InVisibleRange || IsZero)
								{
									LabelPositionPoint = new PointF(float.NaN, float.NaN);
								}
								else
								{
									series.CalculateDataPointPosition(i, ref x, ref y);
									PointF labelPoint = new PointF((float)x, (float)y);
									LabelPositionPoint = dataLabelSettings.CalculateDataLabelPoint(series, this, labelPoint, labelStyle, "LowType");
								}

								dataLabel.XPosition = LabelPositionPoint.X;
								dataLabel.YPosition = LabelPositionPoint.Y;
							}
						}
					}
				}
			}
		}

		#endregion

		#region Internal methods

		/// <summary>
		/// Converts the data points to corresponding screen points for rendering the spline range area segment.
		/// </summary>
		internal void SetData(IList xValues, IList highValues, IList highStartControlPointsX, IList highStartControlPointsY, IList highEndControlPointsX, IList highEndControlPointsY, IList lowValues, IList lowStartControlPointsX, IList lowStartControlPointsY, IList lowEndControlPointsX, IList lowEndControlPointsY)
		{
			int count = xValues.Count;
			HighVal = new double[count];
			LowVal = new double[count];
			XVal = new double[count];

			HighControlStartX = new double[count];
			HighControlStartY = new double[count];
			HighControlEndX = new double[count];
			HighControlEndY = new double[count];

			LowControlStartX = new double[count];
			LowControlStartY = new double[count];
			LowControlEndX = new double[count];
			LowControlEndY = new double[count];

			xValues.CopyTo(XVal, 0);

			highValues.CopyTo(HighVal, 0);
			highStartControlPointsX.CopyTo(HighControlStartX, 0);
			highStartControlPointsY.CopyTo(HighControlStartY, 0);
			highEndControlPointsX.CopyTo(HighControlEndX, 0);
			highEndControlPointsY.CopyTo(HighControlEndY, 0);

			lowValues.CopyTo(LowVal, 0);
			lowStartControlPointsX.CopyTo(LowControlStartX, 0);
			lowStartControlPointsY.CopyTo(LowControlStartY, 0);
			lowEndControlPointsX.CopyTo(LowControlEndX, 0);
			lowEndControlPointsY.CopyTo(LowControlEndY, 0);

			UpdateRange();
		}

		#endregion

		#region Private methods

		void IMarkerDependentSegment.DrawMarker(ICanvas canvas)
		{
			if (Series is IMarkerDependent series && FillPoints != null)
			{
				var marker = series.MarkerSettings;
				var fill = marker.Fill;
				var type = marker.Type;

				for (int i = 2; i < FillPoints.Count; i += 2)
				{
					var rect = new Rect(FillPoints[i] - (marker.Width / 2), FillPoints[i + 1] - (marker.Height / 2), marker.Width, marker.Height);

					canvas.SetFillPaint(fill == default(Brush) ? Fill : fill, rect);

					series.DrawMarker(canvas, Index, type, rect);
				}
			}
		}

		/// <summary>
		/// Update axis range
		/// </summary>
		void UpdateRange()
		{
			if (Series is CartesianSeries series && series.ActualYAxis != null && HighVal != null && XVal != null && LowVal != null)
			{
				_min = HighVal.Min();
				_min = double.IsNaN(_min) ? HighVal.Length > 0 ? HighVal.Where(e => !double.IsNaN(e)).DefaultIfEmpty().Min() : 0 : _min;
				_max = HighVal.Max();

				if (HighControlStartY != null && HighControlEndY != null)
				{
					int highStartCount = HighControlStartY.Length;
					int highEndCount = HighControlEndY.Length;

					double highStartControlMin = HighControlStartY.Min();
					double highStartControlMax = HighControlStartY.Max();

					double highEndControlMin = HighControlEndY.Min();
					double highEndControlMax = HighControlEndY.Max();

					if (highStartCount > 1)
					{
						highStartControlMin = HighControlStartY.SkipLast(1).Min();
						highStartControlMax = HighControlStartY.SkipLast(1).Max();
					}

					if (highStartCount > 1)
					{
						highEndControlMin = HighControlEndY.SkipLast(1).Min();
						highEndControlMax = HighControlEndY.SkipLast(1).Max();
					}


					_max = Math.Max(_max, Math.Max(highStartControlMax, highEndControlMax));
					_min = Math.Min(_min, Math.Min(highStartControlMin, highEndControlMin));
				}

				_min = Math.Min(_min, LowVal.Min());
				_max = Math.Max(_max, LowVal.Max());

				if (LowControlStartY != null && LowControlEndY != null)
				{

					double lowStartControlMin = LowControlStartY.Min();
					double lowStartControlMax = LowControlStartY.Max();

					double lowEndControlMin = LowControlEndY.Min();
					double lowEndControlMax = LowControlEndY.Max();

					int lowStartCount = LowControlStartY.Length;
					int lowEndCount = LowControlEndY.Length;

					if (lowStartCount > 1)
					{
						lowStartControlMin = LowControlStartY.SkipLast(1).Min();
						lowStartControlMax = LowControlStartY.SkipLast(1).Max();
					}

					if (lowEndCount > 1)
					{
						lowEndControlMin = LowControlEndY.SkipLast(1).Min();
						lowEndControlMax = LowControlEndY.SkipLast(1).Max();
					}

					_max = Math.Max(_max, lowStartControlMax);
					_max = Math.Max(_max, lowEndControlMax);

					_min = Math.Min(_min, Math.Min(lowStartControlMin, lowEndControlMin));
				}

				series.XRange += new DoubleRange(XVal.Min(), XVal.Max());
				series.YRange += new DoubleRange(_min, _max);
			}
		}

		/// <summary>
		/// Calculate interior points.
		/// </summary>
		void CalculateInteriorPoints()
		{
			if (Series is not CartesianSeries series || series.ActualXAxis == null || HighVal == null || XVal == null || LowVal == null)
			{
				return;
			}

			var count = XVal.Length;

			FillPoints = [];
			HighStartControlPoints = [];
			HighEndControlPoints = [];
			LowStartControlPoints = [];
			LowEndControlPoints = [];
			StartControlPoints = [];
			EndControlPoints = [];

			double lowValue = LowVal[0], xValue = XVal[0], startX, startHigh, endX, endHigh, startLow, endLow;

			FillPoints.Add(series.TransformToVisibleX(xValue, lowValue));
			FillPoints.Add(series.TransformToVisibleY(xValue, lowValue));


			double highValue;
			for (int i = 0; i < count - 1; i++)
			{
				xValue = XVal[i];
				highValue = HighVal[i];

				FillPoints.Add(series.TransformToVisibleX(xValue, highValue));
				FillPoints.Add(series.TransformToVisibleY(xValue, highValue));
				if (HighControlStartX != null && HighControlStartY != null)
				{
					startX = HighControlStartX[i];
					startHigh = HighControlStartY[i];
					HighStartControlPoints.Add(series.TransformToVisibleX(startX, startHigh));
					HighStartControlPoints.Add(series.TransformToVisibleY(startX, startHigh));
				}

				if (HighControlEndX != null && HighControlEndY != null)
				{
					endX = HighControlEndX[i];
					endHigh = HighControlEndY[i];
					HighEndControlPoints.Add(series.TransformToVisibleX(endX, endHigh));
					HighEndControlPoints.Add(series.TransformToVisibleY(endX, endHigh));
				}
			}

			xValue = XVal[count - 1];
			highValue = HighVal[count - 1];
			FillPoints.Add(series.TransformToVisibleX(xValue, highValue));
			FillPoints.Add(series.TransformToVisibleY(xValue, highValue));

			for (int i = count - 1; i >= 0; i--)
			{
				xValue = XVal[i];
				lowValue = LowVal[i];
				FillPoints.Add(series.TransformToVisibleX(xValue, lowValue));
				FillPoints.Add(series.TransformToVisibleY(xValue, lowValue));

				if (i == count - 1)
				{
					xValue = XVal[i];
					lowValue = LowVal[i];
					LowEndControlPoints.Add(series.TransformToVisibleX(xValue, lowValue));
					LowEndControlPoints.Add(series.TransformToVisibleY(xValue, lowValue));

					xValue = XVal[i];
					lowValue = LowVal[i];
					LowStartControlPoints.Add(series.TransformToVisibleX(xValue, lowValue));
					LowStartControlPoints.Add(series.TransformToVisibleY(xValue, lowValue));
				}
				else
				{
					if (LowControlEndX != null && LowControlEndY != null)
					{
						endX = LowControlEndX[i];
						endLow = LowControlEndY[i];
						LowEndControlPoints.Add(series.TransformToVisibleX(endX, endLow));
						LowEndControlPoints.Add(series.TransformToVisibleY(endX, endLow));
					}

					if (LowControlStartX != null && LowControlStartY != null)
					{
						startX = LowControlStartX[i];
						startLow = LowControlStartY[i];
						LowStartControlPoints.Add(series.TransformToVisibleX(startX, startLow));
						LowStartControlPoints.Add(series.TransformToVisibleY(startX, startLow));
					}
				}
			}

			StartControlPoints.AddRange(HighStartControlPoints);
			StartControlPoints.AddRange(LowEndControlPoints);
			EndControlPoints.AddRange(HighEndControlPoints);
			EndControlPoints.AddRange(LowStartControlPoints);
		}

		void DrawPath(ICanvas canvas, List<float>? fillPoints)
		{
			if (Series == null || fillPoints == null)
			{
				return;
			}

			var path = new PathF();
			var strokePath = new PathF();

			if (Series.CanAnimate())
			{
				AnimateSeriesClipRect(canvas, Series.AnimationValue);
			}

			path.MoveTo(fillPoints[0], fillPoints[1]);
			path.LineTo(fillPoints[2], fillPoints[3]);

			if (HasStroke)
			{
				strokePath.MoveTo(fillPoints[2], fillPoints[3]);
			}

			if (StartControlPoints != null && EndControlPoints != null)
			{
				var halfCount = (StartControlPoints.Count / 2) - 1;
				for (int i = 0; i < StartControlPoints.Count; i++)
				{
					var endPointX = fillPoints[i + 4];
					var endPointY = fillPoints[i + 5];

					var controlStartX = StartControlPoints[i];
					var controlStartY = StartControlPoints[i + 1];
					var controlEndX = EndControlPoints[i];
					var controlEndY = EndControlPoints[i + 1];

					path.CurveTo(controlStartX, controlStartY, controlEndX, controlEndY, endPointX, endPointY);
					if (HasStroke)
					{
						if (halfCount == i)
						{
							strokePath.MoveTo(endPointX, endPointY);
						}
						else
						{
							strokePath.CurveTo(controlStartX, controlStartY, controlEndX, controlEndY, endPointX, endPointY);
						}
					}

					i++;
				}
			}

			int fillPointsCount = fillPoints.Count;
			path.LineTo(fillPoints[fillPointsCount - 2], fillPoints[fillPointsCount - 1]);
			path.LineTo(fillPoints[0], fillPoints[1]);

			canvas.SetFillPaint(Fill, path.Bounds);
			canvas.FillPath(path);

			if (HasStroke)
			{
				canvas.DrawPath(strokePath);
			}
		}

		#endregion

		#endregion
	}
}
