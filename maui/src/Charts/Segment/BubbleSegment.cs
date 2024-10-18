using Microsoft.Maui.Graphics;
using System;

namespace Syncfusion.Maui.Toolkit.Charts
{
    /// <summary>
    /// Represents a segment of a <see cref="BubbleSeries"/>.
    /// </summary>
    public class BubbleSegment : CartesianSegment
    {
        #region Fields

        double _x, _y, _sizeValue;

        #endregion

        #region Properties

        #region Public Proerties

        /// <summary>
        /// Gets the X-value of the bubble center.
        /// </summary>
        public float CenterX { get; internal set; }

        /// <summary>
        /// Gets the Y-value of the bubble center.
        /// </summary>
        public float CenterY { get; internal set; }

        /// <summary>
        /// Gets the radius of the bubble. 
        /// </summary>
        public float Radius { get; internal set; }

        #endregion

        #region Internal Properties

        internal float PreviousCenterX { get; set; } = float.NaN;

        internal float PreviousCenterY { get; set; } = float.NaN;

        internal float PreviousRadius { get; set; } = float.NaN;

        #endregion

        #endregion

        #region Methods

        /// <inheritdoc/>
        protected internal override void OnLayout()
        {
            if (Series is not BubbleSeries series || series.ActualXAxis == null)
            {
                return;
            }

            var xAxis = series.ActualXAxis;
            var start = Math.Floor(xAxis.VisibleRange.Start);
            var end = Math.Ceiling(xAxis.VisibleRange.End);

            CenterX = CenterY = float.NaN;

            if (_x <= end && _x >= start)
            {
                CenterX = series.TransformToVisibleX(_x, _y);
                CenterY = series.TransformToVisibleY(_x, _y);
            }

            SegmentBounds = new RectF((float.IsNaN(CenterX) ? 0 : CenterX) - Radius,
                (float.IsNaN(CenterY) ? 0 : CenterY) - Radius, Radius * 2, Radius * 2);
        }

        /// <inheritdoc/>
        protected internal override void Draw(ICanvas canvas)
        {
            if (Series == null || Empty)
            {
                return;
            }

            float radius = Radius;
            float centerX = CenterX;
            float centerY = CenterY;

            if (Series.CanAnimate())
            {
                float animationValue = Series.AnimationValue;

                if (!Series.XRange.Equals(Series.PreviousXRange) || (float.IsNaN(PreviousCenterX) && float.IsNaN(PreviousCenterY) && float.IsNaN(PreviousRadius)))
                {
                    radius *= animationValue;
                }
                else
                {
                    centerX = GetDynamicAnimationValue(animationValue, centerX, PreviousCenterX, CenterX);
                    centerY = GetDynamicAnimationValue(animationValue, centerY, PreviousCenterY, CenterY);
                    radius = GetDynamicAnimationValue(animationValue, radius, PreviousRadius, Radius);
                }
            }

            canvas.SetFillPaint(Fill, SegmentBounds);
            canvas.Alpha = Opacity;
            canvas.FillCircle(centerX, centerY, radius);

            if (HasStroke)
            {
                canvas.StrokeSize = (float)StrokeWidth;
                canvas.StrokeColor = Stroke.ToColor();
                canvas.DrawCircle(centerX, centerY, radius);
            }
        }

        /// <summary>
        /// Converts the data points to corresponding screen points for rendering the bubble segment.
        /// </summary>
        internal override void SetData(double[] values)
        {
            _x = values[0];
            _y = values[1];
            _sizeValue = values[2];
            Radius = (float)values[3];

            Empty = double.IsNaN(_x) || double.IsNaN(_y) || double.IsNaN(_sizeValue) || double.IsNaN(Radius);

            if (Series != null)
            {
                Series.XRange += !double.IsNaN(_x) ? DoubleRange.Union(_x) : DoubleRange.Empty;
                Series.YRange += !double.IsNaN(_y) ? DoubleRange.Union(_y) : DoubleRange.Empty;
            }
        }

        internal override int GetDataPointIndex(float x, float y)
        {
            double xPoint = x - CenterX;
            double yPoint = y - CenterY;
            double pointLength = Math.Sqrt((xPoint * xPoint) + (yPoint * yPoint));

            if (Series != null)
            {
                if (pointLength <= Radius + StrokeWidth)
                {
                    return Series.Segments.IndexOf(this);
                }
            }

            return -1;
        }

        internal void SetPreviousData(float[] values)
        {
            PreviousCenterX = values[0];
            PreviousCenterY = values[1];
            PreviousRadius = values[2];
        }

        internal override void OnDataLabelLayout()
        {
            CalculateDataLabelPosition(_x, _y, _y);
        }

        #endregion

    }
}
