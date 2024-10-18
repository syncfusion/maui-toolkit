using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System.Collections.Generic;

namespace Syncfusion.Maui.Toolkit.Charts
{
    /// <summary>
    /// Represents the segment for <see cref="StepAreaSeries"/>
    /// </summary>
    public class StepAreaSegment : AreaSegment, IMarkerDependentSegment
    {
        #region Methods

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
            if (Series is CartesianSeries cartesian && cartesian.ActualXAxis != null && XValues != null && YValues != null)
            {
                var crossingValue = cartesian.ActualXAxis.ActualCrossingValue;
                var count = XValues.Length;
                FillPoints = new List<float>();

                double yValue = YValues[0], xValue = XValues[0];
                crossingValue = double.IsNaN(crossingValue) ? 0 : crossingValue;
                FillPoints.Add(cartesian.TransformToVisibleX(xValue, 0));
                FillPoints.Add(cartesian.TransformToVisibleY(xValue, crossingValue));

                for (int i = 0; i < count; i++)
                {
                    xValue = XValues[i];
                    yValue = YValues[i];
                    double x1Value = XValues[count > i + 1 ? i + 1 : i];
                    FillPoints.Add(cartesian.TransformToVisibleX(xValue, yValue));
                    FillPoints.Add(cartesian.TransformToVisibleY(xValue, yValue));
                    var midX = cartesian.TransformToVisibleX(x1Value, yValue);
                    var midY = cartesian.TransformToVisibleY(x1Value, yValue);

                    if (i == count - 1)
                    {
                        midX = cartesian.TransformToVisibleX(xValue, yValue);
                        midY = cartesian.TransformToVisibleY(xValue, yValue);
                    }

                    FillPoints.Add(midX);
                    FillPoints.Add(midY);
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
            if (Series is CartesianSeries cartesian && cartesian.ActualXAxis != null && XValues != null && YValues != null)
            {
                var crossingValue = cartesian.ActualXAxis.ActualCrossingValue;
                var count = XValues.Length;
                StrokePoints = new List<float>();

                for (int i = 0; i < count; i++)
                {
                    double xValue = XValues[i];
                    double yValue = YValues[i];
                    double x1Value = XValues[count > i + 1 ? i + 1 : i];
                    StrokePoints.Add(cartesian.TransformToVisibleX(xValue, yValue));
                    StrokePoints.Add(cartesian.TransformToVisibleY(xValue, yValue));
                    var midX = cartesian.TransformToVisibleX(x1Value, yValue);
                    var midY = cartesian.TransformToVisibleY(x1Value, yValue);

                    if (i == count - 1)
                    {
                        midX = cartesian.TransformToVisibleX(xValue, yValue);
                        midY = cartesian.TransformToVisibleY(xValue, yValue);
                    }

                    StrokePoints.Add(midX);
                    StrokePoints.Add(midY);
                }
            }
        }

        internal override int GetDataPointIndex(float x, float y)
        {
            if (Series != null && Series.SeriesContainsPoint(new PointF(x, y)))
            {
                return Series.Segments.IndexOf(this);
            }

            return -1;
        }

        void IMarkerDependentSegment.DrawMarker(ICanvas canvas)
        {
            if (Series is IMarkerDependent series && FillPoints != null)
            {
                var marker = series.MarkerSettings;
                var fill = marker.Fill;
                var type = marker.Type;

                for (int i = 2; i < FillPoints.Count - 3; i += 4)
                {
                    var rect = new Rect(FillPoints[i] - (marker.Width / 2), FillPoints[i + 1] - (marker.Height / 2), marker.Width, marker.Height);

                    canvas.SetFillPaint(fill == default(Brush) ? Fill : fill, rect);

                    series.DrawMarker(canvas, Index, type, rect);
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
                    var xIndex = 4 * tooltipIndex + 2;
                    rect = new Rect(FillPoints[xIndex] - (markerWidth / 2), FillPoints[xIndex + 1] - (markerHeight / 2), markerWidth, markerHeight);
                }
            }

            return rect;
        }

        #endregion
    }
}
