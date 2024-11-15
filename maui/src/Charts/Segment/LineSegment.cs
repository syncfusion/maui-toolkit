using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.Charts
{
    /// <summary>
    /// Represents a segment of the <see cref="LineSeries"/> chart, defining the start and end points of the line.
    /// </summary>
    public class LineSegment : CartesianSegment, IMarkerDependentSegment
    {
        #region Properties

        /// <summary>
        /// Gets the x-coordinate of the starting point of the line segment.
        /// </summary>
        public float X1 { get; internal set; }

        /// <summary>
        /// Gets the y-coordinate of the starting point of the line segment.
        /// </summary>
        public float Y1 { get; internal set; }

        /// <summary>
        /// Gets the x-coordinate of the ending point of the line segment.
        /// </summary>
        public float X2 { get; internal set; }

        /// <summary>
        /// Gets the y-coordinate of the ending point of the line segment.
        /// </summary>
        public float Y2 { get; internal set; }

        internal double X1Pos { get; set; }

        internal double Y1Pos { get; set; }

        internal double X2Pos { get; set; }

        internal double Y2Pos { get; set; }

        internal float PreviousX1 { get; set; } = float.NaN;

        internal float PreviousY1 { get; set; } = float.NaN;

        internal float PreviousX2 { get; set; } = float.NaN;

        internal float PreviousY2 { get; set; } = float.NaN;

        #endregion

        #region Methods

        /// <inheritdoc/>
        protected internal override void Draw(ICanvas canvas)
        {
            if (Series == null || SeriesView == null || Empty)
            {
                return;
            }

            float x1 = X1;
            float y1 = Y1;
            float x2 = X2;
            float y2 = Y2;

            if (Series.CanAnimate())
            {
                float animationValue = Series.AnimationValue;

                if (Series.IsDataPointAddedDynamically)
                {
                    int index = Series.Segments.IndexOf(this);

                    if (!Series.XRange.Equals(Series.PreviousXRange) && Series.Segments.Count - 1 == index + 1)
                    {
                        LineSegment? previousSegment = Series.Segments[index - 1] as LineSegment;

                        if (previousSegment != null)
                        {
                            float prevX = previousSegment.X2;
                            float prevY = previousSegment.Y2;
                            x1 = prevX + (x1 - prevX) * animationValue;
                            y1 = prevY + (y1 - prevY) * animationValue;

                            DrawDynamicAnimation(animationValue, x1, y1, ref x2, ref y2);
                        }
                    }
                }
                else if (!Series.XRange.Equals(Series.PreviousXRange) || (float.IsNaN(PreviousX1) && float.IsNaN(PreviousY1) && float.IsNaN(PreviousX2) && float.IsNaN(PreviousY2)))
                {
                    AnimateSeriesClipRect(canvas, animationValue);
                }
                else
                {
                    y1 = GetDynamicAnimationValue(animationValue, y1, PreviousY1, Y1);
                    y2 = GetDynamicAnimationValue(animationValue, y2, PreviousY2, Y2);
                    x1 = GetDynamicAnimationValue(animationValue, x1, PreviousX1, X1);
                    x2 = GetDynamicAnimationValue(animationValue, x2, PreviousX2, X2);
                }
            }

            canvas.StrokeSize = (float)StrokeWidth;
            canvas.StrokeColor = Fill.ToColor();
            canvas.Alpha = Opacity;

            if (StrokeDashArray != null && StrokeDashArray.Count > 0)
            {
                canvas.StrokeDashPattern = StrokeDashArray.ToFloatArray();
            }

            canvas.StrokeLineCap = LineCap.Round;
            DrawLine(canvas, x1, x2, y1, y2);
        }

        internal virtual void DrawDynamicAnimation(float animationValue, float x1, float y1, ref float x2, ref float y2)
        {
            x2 = x1 + (x2 - x1) * animationValue;
            y2 = y1 + (y2 - y1) * animationValue;
        }

        internal virtual void DrawLine(ICanvas canvas, float x1, float x2, float y1, float y2)
        {
            canvas.DrawLine(x1, y1, x2, y2);
        }

        /// <inheritdoc/>
        protected internal override void OnLayout()
        {
            if (Series is not CartesianSeries series || series.ActualXAxis == null)
            {
                return;
            }

            var xAxis = series.ActualXAxis;
            var start = Math.Floor(xAxis.VisibleRange.Start);
            var end = Math.Ceiling(xAxis.VisibleRange.End);
            X1 = Y1 = X2 = Y2 = float.NaN;

            if ((X1Pos >= start && X1Pos <= end) || (X2Pos >= start && X2Pos <= end) || (start >= X1Pos && start <= X2Pos))
            {
                X1 = series.TransformToVisibleX(X1Pos, Y1Pos);
                Y1 = series.TransformToVisibleY(X1Pos, Y1Pos);
                X2 = series.TransformToVisibleX(X2Pos, Y2Pos);
                Y2 = series.TransformToVisibleY(X2Pos, Y2Pos);
            }
        }

        /// <summary>
        /// Converts the data points to corresponding screen points for rendering the line segment.
        /// </summary>
        internal override void SetData(double[] values)
        {
            if (Series == null)
            {
                return;
            }

            X1Pos = values[0];
            Y1Pos = values[1];
            X2Pos = values[2];
            Y2Pos = values[3];

            Empty = double.IsNaN(X1Pos) || double.IsNaN(X2Pos) || double.IsNaN(Y1Pos) || double.IsNaN(Y2Pos);

            if (Series.PointsCount == 1)
            {
                Series.XRange = new DoubleRange(X1Pos, X1Pos);
                Series.YRange = new DoubleRange(Y1Pos, Y1Pos);
            }
            else
            {
                Series.XRange += new DoubleRange(X1Pos, X2Pos);
                Series.YRange += new DoubleRange(Y1Pos, Y2Pos);
            }
        }

        internal override int GetDataPointIndex(float x, float y)
        {
            if (Series != null)
            {
                if (IsRectContains(X1, Y1, x, y, (float)StrokeWidth))
                {
                    return Series.Segments.IndexOf(this);
                }
                else if (Series.Segments.IndexOf(this) == Series.Segments.Count - 1 && IsRectContains(X2, Y2, x, y, (float)StrokeWidth))
                {
                    return Series.Segments.IndexOf(this) + 1;
                }
            }

            return -1;
        }

        Rect IMarkerDependentSegment.GetMarkerRect(double markerWidth, double markerHeight, int tooltipIndex)
        {
            return new Rect(X1 - (markerWidth / 2), Y1 - (markerHeight / 2), markerWidth, markerHeight);
        }

        internal void SetPreviousData(float[] values)
        {
            PreviousX1 = values[0];
            PreviousY1 = values[1];
            PreviousX2 = values[2];
            PreviousY2 = values[3];
        }

        internal override void OnDataLabelLayout()
        {
            CalculateDataLabelPosition(X1Pos, Y1Pos, Y1Pos);
        }

        void IMarkerDependentSegment.DrawMarker(ICanvas canvas)
        {
            if (Series is IMarkerDependent series)
            {
                var marker = series.MarkerSettings;
                var fill = marker.Fill;
                var type = marker.Type;

                var rect = new Rect(X1 - (marker.Width / 2), Y1 - (marker.Height / 2), marker.Width, marker.Height);

                canvas.SetFillPaint(fill == default(Brush) ? Fill : fill, rect);
                series.DrawMarker(canvas, Index, type, rect);
            }
        }

        #endregion
    }
}
