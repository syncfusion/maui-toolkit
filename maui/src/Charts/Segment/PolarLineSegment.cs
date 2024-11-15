using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.Charts
{
    /// <summary>
    /// Represents a segment of the <see cref="PolarLineSeries"/> chart.
    /// </summary>
    public class PolarLineSegment : ChartSegment, IMarkerDependentSegment
    {
        #region Fields

        double _x1Value, _y1Value, _x2Value, _y2Value;
        float _x1Pos = float.NaN, _x2Pos = float.NaN, _y1Pos = float.NaN, _y2Pos = float.NaN;

        #endregion

        #region Methods

        #region Interface Implementation

        void IMarkerDependentSegment.DrawMarker(ICanvas canvas)
        {
            if (Series is IMarkerDependent series)
            {
                var marker = series.MarkerSettings;
                var fill = marker.Fill;
                var type = marker.Type;

                var rect = new Rect(_x1Pos - (marker.Width / 2), _y1Pos - (marker.Height / 2), marker.Width, marker.Height);

                canvas.SetFillPaint(fill == default(Brush) ? Fill : fill, rect);
                series.DrawMarker(canvas, Index, type, rect);
            }
        }

        Rect IMarkerDependentSegment.GetMarkerRect(double markerWidth, double markerHeight, int tooltipIndex)
        {
            return new Rect(_x1Pos - (markerWidth / 2), _y1Pos - (markerHeight / 2), markerWidth, markerHeight);
        }

        #endregion

        #region Internal Method

        internal override int GetDataPointIndex(float x, float y)
        {
            if (Series != null)
            {
                if (IsRectContains((float)_x1Value, (float)_y1Value, x, y, (float)StrokeWidth))
                {
                    return Series.Segments.IndexOf(this);
                }
                else if (Series.Segments.IndexOf(this) == Series.Segments.Count - 1 && IsRectContains((float)_x2Value, (float)_y2Value, x, y, (float)StrokeWidth))
                {
                    return Series.Segments.IndexOf(this) + 1;
                }
            }

            return -1;
        }

        /// <summary>
        /// Converts the data points to corresponding screen points for rendering the polar line segment.
        /// </summary>
        internal override void SetData(double[] values)
        {
            if (Series == null) return;

            _x1Value = values[0];
            _y1Value = values[1];
            _x2Value = values[2];
            _y2Value = values[3];

            Empty = double.IsNaN(_x1Value) || double.IsNaN(_x2Value) || double.IsNaN(_y1Value) || double.IsNaN(_y2Value);

            if (Series.PointsCount == 1)
            {
                Series.XRange = new DoubleRange(_x1Value, _x1Value);
                Series.YRange = new DoubleRange(_y1Value, _y1Value);
            }
            else
            {
                Series.XRange += new DoubleRange(_x1Value, _x2Value);
                Series.YRange += new DoubleRange(_y1Value, _y2Value);
            }
        }

        internal override void OnDataLabelLayout()
        {
            if (Series != null && Series.LabelTemplate != null)
            {
                CalculateDataLabelPosition(_x1Value, _y1Value, _y1Value);
            }
        }

        void CalculateDataLabelPosition(double xValue, double yValue, double actualYValues)
        {
            if (Series is not PolarSeries polarSeries) return;

            var dataLabelSettings = polarSeries.DataLabelSettings;

            if (polarSeries.ChartArea == null || !polarSeries.ShowDataLabels || dataLabelSettings == null) return;

            double x = xValue, y = yValue;
            polarSeries.CalculateDataPointPosition(Index, ref x, ref y);
            PointF labelPoint = new PointF((float)x, (float)y);
            DataLabelXPosition = x;
            DataLabelYPosition = y;
            LabelContent = polarSeries.GetLabelContent(actualYValues, polarSeries.SumOfValues(polarSeries.YValues));
            UpdateDataLabels();
        }

        internal override void UpdateDataLabels()
        {
            if (Series is PolarSeries series)
            {
                var dataLabelSettings = series.DataLabelSettings;

                if (dataLabelSettings == null) return;

                if (DataLabels != null && DataLabels.Count > 0)
                {
                    ChartDataLabel dataLabel = DataLabels[0];

                    dataLabel.LabelStyle = dataLabelSettings.LabelStyle;
                    dataLabel.Background = dataLabelSettings.LabelStyle.Background;
                    dataLabel.Index = Index;
                    dataLabel.Item = Item;
                    dataLabel.Label = LabelContent ?? string.Empty;

                    LabelPositionPoint = series.CalculateDataLabelPoint(this, new PointF((float)DataLabelXPosition, (float)DataLabelYPosition), dataLabelSettings.LabelStyle);
                    dataLabel.XPosition = LabelPositionPoint.X;
                    dataLabel.YPosition = LabelPositionPoint.Y;
                }
            }
        }

        #endregion

        #region Protected Internal Methods

        /// <inheritdoc/>
        protected internal override void OnLayout()
        {
            if (Series is not PolarSeries series || series.ActualXAxis == null)
            {
                return;
            }

            ChartAxis xAxis = series.ActualXAxis;

            var start = Math.Floor(xAxis.VisibleRange.Start);
            var end = Math.Ceiling(xAxis.VisibleRange.End);

            if ((_x1Value >= start && _x1Value <= end) || (_x2Value >= start && _x2Value <= end) || (start >= _x1Value && start <= _x2Value))
            {
                PointF point1 = series.TransformVisiblePoint(_x1Value, _y1Value, 1);
                PointF point2 = series.TransformVisiblePoint(_x2Value, _y2Value, 1);

                _x1Pos = point1.X;
                _y1Pos = point1.Y;
                _x2Pos = point2.X;
                _y2Pos = point2.Y;
            }
        }

        /// <inheritdoc/>
        protected internal override void Draw(ICanvas canvas)
        {
            if (Series is not PolarSeries series || SeriesView == null || Empty) return;

            float x1 = _x1Pos;
            float y1 = _y1Pos;
            float x2 = _x2Pos;
            float y2 = _y2Pos;

            if (series.CanAnimate())
            {
                var animation = series.AnimationValue;
                PointF point1 = series.TransformVisiblePoint(_x1Value, _y1Value, animation);
                PointF point2 = series.TransformVisiblePoint(_x2Value, _y2Value, animation);

                x1 = point1.X;
                y1 = point1.Y;
                x2 = point2.X;
                y2 = point2.Y;
            }

            if (StrokeDashArray != null)
            {
                canvas.StrokeDashPattern = StrokeDashArray.ToFloatArray();
            }

            canvas.StrokeSize = (float)StrokeWidth;
            canvas.StrokeColor = Fill.ToColor();
            canvas.Alpha = Opacity;

            canvas.DrawLine(x1, y1, x2, y2);
        }

        #endregion

        #endregion
    }
}