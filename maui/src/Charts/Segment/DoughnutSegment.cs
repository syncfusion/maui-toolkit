using Microsoft.Maui.Graphics;
using System;

namespace Syncfusion.Maui.Toolkit.Charts
{
    /// <summary>
    /// Represents a segment of the <see cref="DoughnutSeries"/> chart, extending the functionality of a pie chart segment.
    /// This segment is used specifically for doughnut charts, which differ from pie charts by having a central hole.
    /// </summary>
    public class DoughnutSegment : PieSegment
    {
        #region Fields

        RectF _actualBounds, _currentBounds;

        double _startAngle, _endAngle;

        double _segmentRadius;

        #endregion

        #region Internal Properties

        internal float InnerRadius { get; set; }

        #endregion

        #region Methods

        #region Protected Methods

        /// <inheritdoc/>
        protected internal override void OnLayout()
        {
            if (Series is not DoughnutSeries series || double.IsNaN(YValue))
            {
                return;
            }

            //For calculating doughnut center angle.
            MidAngle = (StartAngle + (EndAngle / 2)) * 0.0174532925f;
            UpdatePosition();
        }

        /// <inheritdoc/>
        protected internal override void Draw(ICanvas canvas)
        {
            if (Series is not DoughnutSeries series || double.IsNaN(YValue))
            {
                return;
            }

            float drawStartAngle = (float)StartAngle;
            float drawEndAngle = (float)EndAngle;

            if (series.CanAnimate())
            {
                float animationValue = series.AnimationValue;

                if (!double.IsNaN(PreviousStartAngle) && !double.IsNaN(PreviousEndAngle))
                {
                    drawStartAngle = GetDynamicAnimationAngleValue(animationValue, PreviousStartAngle, StartAngle);
                    drawEndAngle = GetDynamicAnimationAngleValue(animationValue, PreviousEndAngle, EndAngle);
                }
                else
                {
                    drawStartAngle = (float)(series.StartAngle + ((StartAngle - series.StartAngle) * animationValue));
                    drawEndAngle = (float)EndAngle * animationValue;
                }
            }

            UpdatePosition();
            var endArcAngle = series.isClockWise ? drawStartAngle + drawEndAngle : drawEndAngle;

            PathF path = new PathF();

            //Drawing segment.
            path.AddArc(_actualBounds.Left, _actualBounds.Top, _actualBounds.Right, _actualBounds.Bottom, -drawStartAngle, -(endArcAngle), series.isClockWise);
            path.AddArc(_currentBounds.Left, _currentBounds.Top, _currentBounds.Right, _currentBounds.Bottom, -(endArcAngle), -drawStartAngle, !series.isClockWise);
            path.Close();
            canvas.SetFillPaint(Fill, path.Bounds);
            canvas.Alpha = Opacity;
            canvas.FillPath(path);

            //Drawing stroke.
            if (HasStroke)
            {
#if WINDOWS
                //MAUI-582: Exception faced "Value not fall within expected range, due to path closed before stroke drawing" 
                var radius = (float)series.InnerRadius * (Math.Min(_actualBounds.Width, _actualBounds.Height) / 2);
                var x = (float)(_currentBounds.X + (_currentBounds.Width / 2) + (radius * Math.Cos(drawStartAngle * (Math.PI / 180))));
                var y = (float)(_currentBounds.Y + (_currentBounds.Width / 2) + (radius * Math.Sin(drawStartAngle * (Math.PI / 180))));
                path.MoveTo(x, y);
#endif
                path.AddArc(_actualBounds.Left, _actualBounds.Top, _actualBounds.Right, _actualBounds.Bottom, -drawStartAngle, -(endArcAngle), series.isClockWise);
                path.AddArc(_currentBounds.Left, _currentBounds.Top, _currentBounds.Right, _currentBounds.Bottom, -(endArcAngle), -drawStartAngle, !series.isClockWise);
                path.Close();
                canvas.StrokeColor = Stroke.ToColor();
                canvas.StrokeSize = (float)StrokeWidth;
                canvas.DrawPath(path);
            }
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Converts the data points to corresponding screen points for rendering the doughnut segment.
        /// </summary>
        internal override void SetData(double mStartAngle, double mEndAngle, double mYValue)
        {
            if (Series is DoughnutSeries doughnutSeries)
            {
                _startAngle = mStartAngle;
                _endAngle = doughnutSeries.isClockWise ? mEndAngle : mStartAngle + mEndAngle;
                StartAngle = mStartAngle;
                EndAngle = doughnutSeries.isClockWise ? mEndAngle : mStartAngle + mEndAngle;
                YValue = mYValue;
            }
        }

        internal override int GetDataPointIndex(float x, float y)
        {
            if (Series != null && IsPointInDoughnutSegment(_segmentRadius, x, y))
            {
                return Series.Segments.IndexOf(this);
            }

            return -1;
        }

        #endregion

        #region private Methods

        void UpdatePosition()
        {
            if (Series is not DoughnutSeries series)
            {
                return;
            }

            var center = series.Center;
            RectF seriesClipRect = series.AreaBounds;
            double minScale = Math.Min(seriesClipRect.Width, seriesClipRect.Height) * series.Radius;
            _actualBounds = new RectF((float)((center.X * 2) - minScale) / 2, (float)((center.Y * 2) - minScale) / 2, (float)minScale, (float)minScale);
            _currentBounds = new RectF(_actualBounds.Left, _actualBounds.Top, _actualBounds.Width, _actualBounds.Height);

            if (series.ExplodeIndex == Index || series.ExplodeAll)
            {
                float angle = series.isClockWise ? (float)((2f * (series.StartAngle + ((StartAngle - series.StartAngle) * series.AnimationValue)) * 0.0174532925f) + ((EndAngle * series.AnimationValue) * 0.0174532925f)) / 2f
                : (float)(((series.StartAngle + ((StartAngle - series.StartAngle) * series.AnimationValue)) * 0.0174532925f) + 
                (EndAngle * series.AnimationValue * 0.0174532925f)) /2;
                _actualBounds = _actualBounds.Offset((float)(series.ExplodeRadius * Math.Cos(angle)), (float)(series.ExplodeRadius * Math.Sin(angle)));
                _currentBounds = _actualBounds;
            }

            InnerRadius = (float)series.InnerRadius * (Math.Min(_actualBounds.Width, _actualBounds.Height) / 2);
            _currentBounds = new RectF(_currentBounds.X + (_currentBounds.Width / 2) - InnerRadius, _currentBounds.Y + (_currentBounds.Height / 2) - InnerRadius, 2 * InnerRadius, 2 * InnerRadius);
            series.ActualRadius = (float)minScale / 2;
            _segmentRadius = series.ActualRadius;
        }

        bool IsPointInDoughnutSegment(double radius, double x, double y)
        {
            if (Series is DoughnutSeries doughnutSeries)
            {
                double dx, dy;

                dx = x - ((_currentBounds.Left + _currentBounds.Right) / 2);
                dy = y - ((_currentBounds.Top + _currentBounds.Bottom) / 2);

                double angle = ChartMath.RadianToDegree(Math.Atan2(dy, dx));

                if (angle < 0)
                {
                    angle = angle + 360;
                }

                double distanceSquare = (dx * dx) + (dy * dy);
                double segmentStartAngle = 360 + StartAngle;
                double segmentEndAngle = doughnutSeries.isClockWise ? Math.Abs(_startAngle) + Math.Abs(_endAngle) : 360 + EndAngle;

                if (doughnutSeries.StartAngle < doughnutSeries.EndAngle)
                {
                    if (doughnutSeries.StartAngle > 0 && segmentEndAngle > 360 && angle < doughnutSeries.StartAngle)
                    {
                        angle = angle + 360;
                    }
                }
                else
                {
                    if (_endAngle > 0 && segmentEndAngle < 360 && angle > _startAngle)
                    {
                        angle = angle + 360;
                    }
                }

                var innerRadius = InnerRadius;
                var outerRadius = radius;

                if (distanceSquare >= innerRadius * innerRadius && distanceSquare <= outerRadius * outerRadius)
                {
                    if (doughnutSeries.StartAngle > doughnutSeries.EndAngle)
                    {
                        if (doughnutSeries.isClockWise && _startAngle > angle && angle > segmentEndAngle)
                        {
                            return true;
                        }
                        else if (!doughnutSeries.isClockWise && segmentStartAngle > angle && angle > segmentEndAngle)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (Math.Abs(_startAngle) < angle && angle < segmentEndAngle)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        #endregion

        #endregion
    }
}
