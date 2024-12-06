using Syncfusion.Maui.Toolkit.Graphics.Internals;
using System.Collections;

namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// Represents the segment for <see cref="RangeAreaSeries"/>.
	/// </summary>
	public partial class RangeAreaSegment : AreaSegment, IMarkerDependentSegment
	{
		#region Fields

		PathF? _path;

		#endregion

		#region Properties

		/// <summary>
		/// Gets the data point values from the series that are bound with y1(High) for the segment.
		/// </summary>
		internal double[]? HighValues { get; set; }

		/// <summary>
		/// Gets the data point values from the series that are bound with y2(Low) for the segment.
		/// </summary>
		internal double[]? LowValues { get; set; }

		internal List<float>? HighStrokePoints { get; set; }

		internal List<float>? LowStrokePoints { get; set; }

		internal int LabelIndex { get; set; }


		#endregion

		#region Methods

		#region Protected Methods

		/// <inheritdoc/>
		protected internal override void Draw(ICanvas canvas)
		{
			if (Empty)
			{
				return;
			}

			canvas.Alpha = Opacity;

			if (HasStroke)
			{
				canvas.StrokeSize = (float)StrokeWidth;
				canvas.StrokeColor = Stroke.ToColor();

				if (StrokeDashArray != null)
				{
					canvas.StrokeDashPattern = StrokeDashArray.ToFloatArray();
				}
			}

			DrawPath(canvas, FillPoints, HighStrokePoints, LowStrokePoints);
		}

		/// <inheritdoc/>
		protected internal override void OnLayout()
		{
			if (XValues == null)
			{
				return;
			}

			CalculateInteriorPoints();

			if (HasStroke)
			{
				CalculateStrokePoints();
			}
		}

		#endregion

		#region Internal Methods

		/// <summary>
		/// Converts the data points to corresponding screen points for rendering the range area segment.
		/// </summary>
		internal void SetData(IList xVals, IList highVals, IList lowVals)
		{
			if (Series is CartesianSeries series)
			{
				var count = xVals.Count;
				XValues = new double[count];
				HighValues = new double[count];
				LowValues = new double[count];

				xVals.CopyTo(XValues, 0);
				highVals.CopyTo(HighValues, 0);
				lowVals.CopyTo(LowValues, 0);

				var high = HighValues.Min();
				var low = LowValues.Min();
				var yMin = high > low ? low : high;
				high = HighValues.Max();
				low = LowValues.Max();
				var yMax = high > low ? high : low;

				series.XRange += new DoubleRange(XValues.Min(), XValues.Max());
				series.YRange += new DoubleRange(yMin, yMax);
			}
		}

		internal override int GetDataPointIndex(float x, float y)
		{
			if (Series != null && Series.SeriesContainsPoint(new PointF(x, y)))
			{
				return Series._segments.IndexOf(this);
			}

			return -1;
		}

		internal override void OnDataLabelLayout()
		{
			if (Series is RangeAreaSeries series && series.LabelTemplate is not null)
			{
				var dataLabelSettings = series.DataLabelSettings;
				ChartDataLabelStyle labelStyle = dataLabelSettings.LabelStyle;

				if (XValues == null || HighValues == null || LowValues == null || series.ActualData == null)
				{
					return;
				}

				for (int i = 0; i < series.PointsCount; i++)
				{
					int highIndex = 0;

					Index = i;

					for (int j = 0; j < 2; j++)
					{
						if (j == 0)
						{
							highIndex = (2 * i) + 1;

							double x = XValues[i];
							double y = HighValues[i];

							InVisibleRange = series.IsDataInVisibleRange(x, y);
							LabelContent = dataLabelSettings.GetLabelContent(HighValues[i]);
							LabelIndex = highIndex - 1;

							if (DataLabels != null && DataLabels.Count > highIndex - 1)
							{
								var dataLabel = DataLabels[highIndex - 1];

								dataLabel.LabelStyle = labelStyle;
								dataLabel.Index = i;
								dataLabel.Item = series.ActualData[i];
								dataLabel.Label = LabelContent ?? string.Empty;

								if (!InVisibleRange || IsEmpty)
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
							double x = XValues[i];
							double y = LowValues[i];

							InVisibleRange = series.IsDataInVisibleRange(x, y);
							LabelContent = dataLabelSettings.GetLabelContent(LowValues[i]);
							LabelIndex = lowIndex - 1;

							if (DataLabels != null && DataLabels.Count > lowIndex - 1)
							{
								var dataLabel = DataLabels[lowIndex - 1];

								dataLabel.LabelStyle = labelStyle;
								dataLabel.Index = i;
								dataLabel.Item = series.ActualData[i];
								dataLabel.Label = LabelContent ?? string.Empty;

								if (!InVisibleRange || IsEmpty)
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

		#region Private Methods

		void DrawPath(ICanvas canvas, List<float>? fillPoints, List<float>? highStrokePoints, List<float>? lowStrokePoints)
		{
			if (Series == null)
			{
				return;
			}

			_path = new PathF();

			float animationValue = Series.AnimationValue;
			bool isDynamicAnimation = Series.CanAnimate() && Series.XRange.Equals(Series.PreviousXRange) && PreviousFillPoints != null && fillPoints != null && fillPoints.Count == PreviousFillPoints.Count;

			if (Series.CanAnimate() && !isDynamicAnimation)
			{
				AnimateSeriesClipRect(canvas, animationValue);
			}

			if (fillPoints != null)
			{
				RangeAreaSegment.DrawAreaPath(ref _path, fillPoints, isDynamicAnimation, animationValue, PreviousFillPoints);

				canvas.SetFillPaint(Fill, _path.Bounds);
				canvas.FillPath(_path);
			}

			if (HasStroke && highStrokePoints != null && lowStrokePoints != null)
			{
				DrawStroke(canvas, animationValue, isDynamicAnimation, highStrokePoints);
				DrawStroke(canvas, animationValue, isDynamicAnimation, lowStrokePoints);
			}
		}

		void DrawStroke(ICanvas canvas, float animationValue, bool isDynamicAnimation, IList<float> strokePoints)
		{
			_path = new PathF();

			RangeAreaSegment.DrawAreaPath(ref _path, strokePoints, isDynamicAnimation, animationValue, PreviousStrokePoints);

			canvas.DrawPath(_path);
		}

		void CalculateInteriorPoints()
		{
			float startX, startY;

			if (Series is CartesianSeries cartesian && cartesian.ActualXAxis != null && cartesian.ActualYAxis != null && XValues != null && HighValues != null && LowValues != null)
			{
				var count = HighValues.Length;
				FillPoints = [];

				double xValue = XValues[0], lowValue = LowValues[0], highValue;
				startX = cartesian.TransformToVisibleX(xValue, lowValue);
				startY = cartesian.TransformToVisibleY(xValue, lowValue);
				FillPoints.Add(startX);
				FillPoints.Add(startY);

				for (int i = 0; i < count; i++)
				{
					xValue = XValues[i];
					highValue = HighValues[i];
					FillPoints.Add(cartesian.TransformToVisibleX(xValue, highValue));
					FillPoints.Add(cartesian.TransformToVisibleY(xValue, highValue));
				}

				for (int i = count - 1; i > 0; i--)
				{
					xValue = XValues[i];
					lowValue = LowValues[i];
					FillPoints.Add(cartesian.TransformToVisibleX(xValue, lowValue));
					FillPoints.Add(cartesian.TransformToVisibleY(xValue, lowValue));
				}

				FillPoints.Add(startX);
				FillPoints.Add(startY);
			}
		}

		void CalculateStrokePoints()
		{
			float startX, startY;

			if (Series is CartesianSeries cartesian && cartesian.ActualXAxis != null && cartesian.ActualYAxis != null && XValues != null && HighValues != null && LowValues != null)
			{
				var count = HighValues.Length;
				HighStrokePoints = [];
				LowStrokePoints = [];

				double xValue = XValues[0], lowValue = LowValues[0], highValue = HighValues[0];
				startX = cartesian.TransformToVisibleX(xValue, highValue);
				startY = cartesian.TransformToVisibleY(xValue, highValue);
				HighStrokePoints.Add(startX);
				HighStrokePoints.Add(startY);

				for (int i = 0; i < count; i++)
				{
					xValue = XValues[i];
					highValue = HighValues[i];
					HighStrokePoints.Add(cartesian.TransformToVisibleX(xValue, highValue));
					HighStrokePoints.Add(cartesian.TransformToVisibleY(xValue, highValue));
				}

				xValue = XValues[0];
				startX = cartesian.TransformToVisibleX(xValue, lowValue);
				startY = cartesian.TransformToVisibleY(xValue, lowValue);
				LowStrokePoints.Add(startX);
				LowStrokePoints.Add(startY);

				for (int i = 0; i < count; i++)
				{
					xValue = XValues[i];
					lowValue = LowValues[i];
					LowStrokePoints.Add(cartesian.TransformToVisibleX(xValue, lowValue));
					LowStrokePoints.Add(cartesian.TransformToVisibleY(xValue, lowValue));
				}
			}
		}

		Rect IMarkerDependentSegment.GetMarkerRect(double markerWidth, double markerHeight, int tooltipIndex)
		{
			Rect rect = new Rect();

			if (Series != null && FillPoints != null)
			{
				if (XValues?.Length > tooltipIndex)
				{
					var xIndex = (2 * tooltipIndex) + 2;
					rect = new Rect(FillPoints[xIndex] - (markerWidth / 2), FillPoints[xIndex + 1] - (markerHeight / 2), markerWidth, markerHeight);
				}
			}

			return rect;
		}

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

		static void DrawAreaPath(ref PathF path, IList<float> points, bool isDynamicAnimation, float animationValue, List<float>? previousPoints)
		{
			for (int i = 0; i < points.Count; i++)
			{
				var x = points[i];
				var y = points[i + 1];

				if (isDynamicAnimation && previousPoints != null)
				{
					x = CartesianSegment.GetDynamicAnimationValue(animationValue, x, previousPoints[i], x);
					y = CartesianSegment.GetDynamicAnimationValue(animationValue, y, previousPoints[i + 1], y);
				}

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
