using System.Collections;
using System.Data;

namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// Represents a segment of the <see cref="StackingAreaSeries"/>
	/// </summary>
	public partial class StackingAreaSegment : AreaSegment, IMarkerDependentSegment
	{
		#region Properties

		/// <summary>
		/// Gets the bottom values.
		/// </summary>
		internal double[]? BottomValues { get; set; }

		/// <summary>
		/// Gets the top values.
		/// </summary>
		internal double[]? TopValues { get; set; }

		#endregion

		#region Methods

		#region Internal Methods

		/// <summary>
		/// Converts the data points to corresponding screen points for rendering the stacking area segment.
		/// </summary>
		internal void SetData(IList xValues, IList yEnds, IList yStarts)
		{
			if (Series is CartesianSeries series && series.ActualYAxis != null)
			{
				var count = xValues.Count;
				XValues = new double[count];
				BottomValues = new double[count];
				TopValues = new double[count];

				xValues.CopyTo(XValues, 0);
				yEnds.CopyTo(TopValues, 0);
				yStarts.CopyTo(BottomValues, 0);

				var yMin = TopValues.Min();
				yMin = double.IsNaN(yMin) ? TopValues.Length > 0 ? TopValues.Where(e => !double.IsNaN(e)).DefaultIfEmpty().Min() : 0 : yMin;

				Series.XRange += new DoubleRange(XValues.Min(), XValues.Max());
				Series.YRange += new DoubleRange(yMin, TopValues.Max());
			}
		}

		internal override int GetDataPointIndex(float x, float y)
		{
			return -1;
		}

		#endregion

		#region Protected Internal Methods

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

		#region Private Methods

		void CalculateInteriorPoints()
		{
			if (Series is CartesianSeries cartesian && cartesian.ActualXAxis != null && XValues != null && BottomValues != null && TopValues != null)
			{
				float startX, startY;
				var count = XValues.Length;
				FillPoints = [];
				startX = Series.TransformToVisibleX(XValues[0], BottomValues[0]);
				startY = Series.TransformToVisibleY(XValues[0], BottomValues[0]);
				FillPoints.Add(startX);
				FillPoints.Add(startY);

				for (int i = 0; i < count; i++)
				{
					FillPoints.Add(Series.TransformToVisibleX(XValues[i], TopValues[i]));
					FillPoints.Add(Series.TransformToVisibleY(XValues[i], TopValues[i]));
				}

				for (int i = count - 1; i > 0; i--)
				{
					if (!double.IsNaN(BottomValues[i]))
					{
						FillPoints.Add(Series.TransformToVisibleX(XValues[i], BottomValues[i]));
						FillPoints.Add(Series.TransformToVisibleY(XValues[i], BottomValues[i]));
					}
				}

				FillPoints.Add(startX);
				FillPoints.Add(startY);
			}
		}

		void CalculateStrokePoints()
		{
			if (Series != null && XValues != null && BottomValues != null && TopValues != null)
			{
				float x, y;
				var halfStrokeWidth = (float)StrokeWidth / 2;
				StrokePoints = [];
				double xValue, yValue;
				int i;
				var count = XValues.Length;

				for (i = 0; i < count; i++)
				{
					xValue = XValues[i];
					yValue = TopValues[i];
					x = Series.TransformToVisibleX(xValue, yValue);
					y = Series.TransformToVisibleY(xValue, yValue);
					StrokePoints.Add(x);
					y += yValue >= 0 ? halfStrokeWidth : -halfStrokeWidth;
					StrokePoints.Add(y);
				}
			}
		}

		void IMarkerDependentSegment.DrawMarker(ICanvas canvas)
		{
			if (Series is IMarkerDependent series && FillPoints != null)
			{
				var marker = series.MarkerSettings;
				var fill = marker.Fill;
				var type = marker.Type;
				var count = FillPoints.Count / 2;

				for (int i = 2; i < count; i += 2)
				{
					var rect = new Rect(FillPoints[i] - (marker.Width / 2), FillPoints[i + 1] - (marker.Height / 2), marker.Width, marker.Height);

					canvas.SetFillPaint(fill == default(Brush) ? Fill : fill, rect);

					series.DrawMarker(canvas, Index, type, rect);
				}
			}
		}
		#endregion

		#endregion
	}
}
