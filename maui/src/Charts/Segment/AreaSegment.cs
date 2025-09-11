using Syncfusion.Maui.Toolkit.Graphics.Internals;
using System.Collections;
using System.Data;

namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// Represents a segment of the <see cref="AreaSeries"/>, providing properties to manage 
	/// the data points and outline fill points necessary for rendering the area segment.
	/// </summary>
	public partial class AreaSegment : CartesianSegment, IMarkerDependentSegment
	{
		#region Properties

		/// <summary>
		/// Gets the data point values from the series that are associated with the x-axis for the segment.
		/// </summary>
		public double[]? XValues { get; internal set; }

		/// <summary>
		/// Gets the data point values from the series that are associated with the y-axis for the segment.
		/// </summary>
		public double[]? YValues { get; internal set; }

		/// <summary>
		/// A list of points used to render the filled area of the segment, calculated based on the x and y values.
		/// </summary>
		internal List<float>? FillPoints { get; set; }

		/// <summary>
		/// A list of points used to render the stroke of the area segment, calculated based on the x and y values.
		/// </summary>
		internal List<float>? StrokePoints { get; set; }

		internal List<float>? PreviousFillPoints { get; set; } = null;

		internal List<float>? PreviousStrokePoints { get; set; } = null;

		PathF? _path;

		#endregion

		#region Methods

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
			}

			if (StrokeDashArray != null)
			{
				canvas.StrokeDashPattern = StrokeDashArray.ToFloatArray();
			}

			DrawPath(canvas, FillPoints, StrokePoints);
		}

		void DrawPath(ICanvas canvas, List<float>? fillPoints, List<float>? strokePoints)
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
				AreaSegment.DrawSegment(canvas, ref _path, fillPoints, animationValue, isDynamicAnimation, PreviousFillPoints);

				canvas.SetFillPaint(Fill, _path.Bounds);
				canvas.FillPath(_path);
			}

			if (HasStroke && strokePoints != null)
			{
				_path = new PathF();
				AreaSegment.DrawSegment(canvas, ref _path, strokePoints, animationValue, isDynamicAnimation, PreviousStrokePoints);
				canvas.DrawPath(_path);
			}
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

		/// <summary>
		/// Calculate interior points.
		/// </summary>
		void CalculateInteriorPoints()
		{
			if (Series is CartesianSeries cartesian && cartesian.ActualXAxis is ChartAxis xAxis && XValues != null && YValues != null)
			{
				var crossingValue = double.IsNaN(xAxis.RenderingCrossesValue) ? cartesian.GetAxisCrossingValue(xAxis) : xAxis.RenderingCrossesValue;
				var count = XValues.Length;
				FillPoints = [];
				double xValue = XValues[0];
				crossingValue = double.IsNaN(crossingValue) ? 0 : crossingValue;
				FillPoints.Add(cartesian.TransformToVisibleX(xValue, 0));
				FillPoints.Add(cartesian.TransformToVisibleY(xValue, crossingValue));

				for (int i = 0; i < count; i++)
				{
					xValue = XValues[i];
					var yValue = YValues[i];
					FillPoints.Add(cartesian.TransformToVisibleX(xValue, yValue));
					FillPoints.Add(cartesian.TransformToVisibleY(xValue, yValue));
				}

				xValue = XValues[count - 1];
				FillPoints.Add(cartesian.TransformToVisibleX(xValue, 0));
				FillPoints.Add(cartesian.TransformToVisibleY(xValue, crossingValue));
			}
		}

		/// <summary>
		/// Calculate stroke points.
		/// </summary>
		void CalculateStrokePoints()
		{
			if (Series != null && XValues != null && YValues != null)
			{
				float x, y;
				var halfStrokeWidth = (float)StrokeWidth / 2;
				StrokePoints = [];
				double yValue;
				var count = XValues.Length;

				for (int i = 0; i < count; i++)
				{
					yValue = YValues[i];
					x = Series.TransformToVisibleX(XValues[i], yValue);
					y = Series.TransformToVisibleY(XValues[i], yValue);
					StrokePoints.Add(x);
					y += yValue >= 0 ? halfStrokeWidth : -halfStrokeWidth;
					StrokePoints.Add(y);
				}
			}
		}

		/// <summary>
		/// Converts the data points to corresponding screen points for rendering the area segment.
		/// </summary>
		internal override void SetData(IList xValues, IList yValues)
		{
			if (Series is CartesianSeries series && series.ActualYAxis != null)
			{
				var count = xValues.Count;
				YValues = new double[count];
				XValues = new double[count];
				xValues.CopyTo(XValues, 0);
				yValues.CopyTo(YValues, 0);

				var yMin = YValues.Min();
				yMin = double.IsNaN(yMin) ? YValues.Length > 0 ? YValues.Where(e => !double.IsNaN(e)).DefaultIfEmpty().Min() : 0 : yMin;

				Empty = double.IsNaN(yMin);

				series.XRange += new DoubleRange(XValues.Min(), XValues.Max());
				series.YRange += new DoubleRange(yMin, YValues.Max());
			}
		}

		internal override int GetDataPointIndex(float x, float y)
		{
			return -1;
		}

		internal override void OnDataLabelLayout()
		{
			if (Series is XYDataSeries xyDataSeries && xyDataSeries.LabelTemplate != null)
			{
				var dataLabelSettings = xyDataSeries.DataLabelSettings;

				var yValues = this is StackingAreaSegment segment ? segment.TopValues : YValues;

				if (XValues == null || yValues == null)
				{
					return;
				}

				for (int i = 0; i < xyDataSeries.PointsCount; i++)
				{
					double x = XValues[i], y = yValues[i];

					Index = i;
					LabelContent = xyDataSeries.GetLabelContent(y, xyDataSeries.SumOfValues(yValues));

					//Assign the values for data labels

					if (DataLabels != null && DataLabels.Count > i)
					{
						ChartDataLabel dataLabel = DataLabels[i];

						dataLabel.LabelStyle = dataLabelSettings.LabelStyle;
						dataLabel.Background = dataLabelSettings.LabelStyle.Background;
						dataLabel.Index = i;
						dataLabel.Item = xyDataSeries.ActualData?[i];
						dataLabel.Label = LabelContent;

						if (!xyDataSeries.IsDataInVisibleRange(x, y))
						{
							LabelPositionPoint = new PointF(float.NaN, float.NaN);
						}
						else
						{
							PointF labelPoint = GetDataLabelPosition(x, y, xyDataSeries);
							LabelPositionPoint = CartesianDataLabelSettings.CalculateDataLabelPoint(xyDataSeries, this, labelPoint, dataLabelSettings.LabelStyle);
						}

						dataLabel.XPosition = LabelPositionPoint.X;
						dataLabel.YPosition = LabelPositionPoint.Y;
					}
				}
			}
		}

		PointF GetDataLabelPosition(double xValue, double yValue, XYDataSeries series)
		{
			double yPosition = yValue;
			series.CalculateDataPointPosition(Index, ref xValue, ref yPosition);
			return new PointF((float)xValue, (float)yPosition);
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

				for (int i = 2; i < FillPoints.Count - 3; i += 2)
				{
					var rect = new Rect(FillPoints[i] - (marker.Width / 2), FillPoints[i + 1] - (marker.Height / 2), marker.Width, marker.Height);

					canvas.SetFillPaint(fill == default(Brush) ? Fill : fill, rect);

					series.DrawMarker(canvas, Index, type, rect);
				}
			}
		}

		static void DrawSegment(ICanvas canvas, ref PathF path, List<float> fillPoints, float animationValue, bool isDynamicAnimation, List<float>? previousFillPoints)
		{
			for (int i = 0; i < fillPoints.Count; i++)
			{
				var x = fillPoints[i];
				var y = fillPoints[i + 1];

				if (isDynamicAnimation && previousFillPoints != null)
				{
					x = CartesianSegment.GetDynamicAnimationValue(animationValue, x, previousFillPoints[i], x);
					y = CartesianSegment.GetDynamicAnimationValue(animationValue, y, previousFillPoints[i + 1], y);
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
	}
}
