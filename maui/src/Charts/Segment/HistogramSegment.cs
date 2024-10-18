using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.Charts
{
    /// <summary>
    /// Represents a segment of the <see cref="HistogramSeries"/> in a chart, displaying a vertical bar.
    /// </summary>
    public class HistogramSegment : ColumnSegment
    {
        #region Internal properties

        internal double HistogramLabelPosition { get; set; }
        internal double PointsCount { get; set; }

        #endregion

        #region Internal Methods

        internal override void OnDataLabelLayout()
        {
            CalculateDataLabelPosition(HistogramLabelPosition, PointsCount, PointsCount);
        }

        /// <summary>
        /// Converts the data points to corresponding screen points for rendering the histogram segment.
        /// </summary>
        internal override void SetData(double[] values)
        {
            base.SetData(values);

            if (Series is HistogramSeries series)
                series.XRange += new DoubleRange(values[0], values[1]);
        }

        #endregion

        #region Protected Internal Methods

        /// <summary>
        /// Draws the histogram segment on the specified canvas.
        /// </summary>
        /// <param name="canvas"></param>
        protected internal override void Draw(ICanvas canvas)
        {
            if (Series is not HistogramSeries series || series.ActualXAxis == null)
                return;

            if (series.CanAnimate())
                Layout();

            if (!float.IsNaN(Left) && !float.IsNaN(Top) && !float.IsNaN(Right) && !float.IsNaN(Bottom))
            {
                canvas.StrokeSize = (float)StrokeWidth;
                canvas.StrokeColor = Stroke.ToColor();
                canvas.Alpha = Opacity;
                var rect = new Rect() { Left = Left, Top = Top, Right = Right, Bottom = Bottom };
                canvas.SetFillPaint(Fill, rect);
                canvas.FillRectangle(rect);

                if (HasStroke)
                    canvas.DrawRectangle(rect);
            }
        }

        #endregion
    }

    internal class DistributionSegment : ILineDrawing
    {
        #region Fields

        HistogramSeries _histogramSeries;
        bool _enableAntiAliasing = false;
        Brush? _stroke;
        double _strokeWidth;
        float _opacity = 1;
        DoubleCollection? _strokeDashArray;

        #endregion

        #region Internal Properties

        internal float[]? DrawPoints { get; set; }

        #endregion

        #region Interface Implementation

        Color ILineDrawing.Stroke
        {
            get
            {
                if (_stroke is SolidColorBrush brush)
                    return brush.Color;
                else
                    return Colors.Black;
            }
            set => _stroke = new SolidColorBrush(value);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        double ILineDrawing.StrokeWidth
        {
            get => _strokeWidth;
            set => _strokeWidth = value;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        bool ILineDrawing.EnableAntiAliasing
        {
            get => _enableAntiAliasing;
            set => _enableAntiAliasing = value;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        float ILineDrawing.Opacity
        {
            get => _opacity;
            set => _opacity = value;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        DoubleCollection? ILineDrawing.StrokeDashArray
        {
            get => _strokeDashArray;
            set => _strokeDashArray = value;
        }

        #endregion

        #region Constructor

        internal DistributionSegment(HistogramSeries series)
        {
            _histogramSeries = series;
        }

        #endregion

        #region Internal Methods

        internal void UpdateCurveStyle()
        {
            if (_histogramSeries.CurveStyle is ChartLineStyle chartLineStyle)
            {
                _stroke = chartLineStyle.Stroke;
                _strokeWidth = chartLineStyle.StrokeWidth;
                _strokeDashArray = chartLineStyle.StrokeDashArray;
            }
        }

        internal void OnDraw(ICanvas canvas)
        {
            if (_histogramSeries.ShowNormalDistributionCurve && !_histogramSeries.CanAnimate() && DrawPoints != null)
            {
                canvas.DrawLines(DrawPoints, this);
            }
        }

        #endregion
    }
}