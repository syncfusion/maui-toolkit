using System.Collections;

namespace Syncfusion.Maui.Toolkit.Charts
{
    /// <summary>
    /// Represents a segment of an <see cref="ErrorBarSeries"/> chart, used to display error bars for data points.
    /// </summary>
    public class ErrorBarSegment : ChartSegment
    {
        #region Fields

        #region Private Fields

        int _index = 0;
        float _strokeWidth;
        Color? _strokeColor;
        double _horizontalErrorValue;
        double _verticalErrorValue;
        List<Point> _leftPointCollection = new();
        List<Point> _rightPointCollection = new();
        List<Point> _topPointCollection = new();
        List<Point> _bottomPointCollection = new();

        #endregion

        #endregion

        #region Properties

        DoubleRange XRange { get; set; }
        DoubleRange YRange { get; set; }

        internal List<ErrorSegmentPoint[]> ErrorSegmentPoints { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorBarSegment"/> class.
        /// </summary>
        public ErrorBarSegment()
        {
            ErrorSegmentPoints = new List<ErrorSegmentPoint[]>();
        }

        #endregion

        #region Methods

        #region Internal Methods

        /// <summary>
        /// Converts the data points to corresponding screen points for rendering the error bar segment.
        /// </summary>
        internal override void SetData(IList xList, IList yList)
        {
            var xValues = (IList<double>)xList;
            var yValues = (IList<double>)yList;
            _index = xValues.Count;
            
            if (Series is ErrorBarSeries errorBarSeries)
            {
                var horSdValue = GetSdErrorValue(xValues);
                var verSdValue = GetSdErrorValue(yValues);

                switch (errorBarSeries.Type)
                {
                    case ErrorBarType.StandardDeviation:
                    case ErrorBarType.StandardError:
                        _horizontalErrorValue = horSdValue[1];
                        _verticalErrorValue = verSdValue[1];
                        break;

                    case ErrorBarType.Custom:
                        _horizontalErrorValue = errorBarSeries.HorizontalErrorValues.Count > 0 ? errorBarSeries.HorizontalErrorValues.Max() : 0;
                        _verticalErrorValue = errorBarSeries.VerticalErrorValues.Count > 0 ? errorBarSeries.VerticalErrorValues.Max() : 0;
                        break;

                    default:
                        _horizontalErrorValue = errorBarSeries.HorizontalErrorValue;
                        _verticalErrorValue = errorBarSeries.VerticalErrorValue;
                        break;
                }

                double xMin = xValues.Min();
                double xMax = xValues.Max();
                double yMin = yValues.Min();
                double yMax = yValues.Max();
                double leftPointMin = xMin - _horizontalErrorValue;
                double leftPointMax = xMax - _horizontalErrorValue;
                double rightPointMin = xMin + _horizontalErrorValue;
                double rightPointMax = xMax + _horizontalErrorValue;
                double bottomPointMin = yMin - _verticalErrorValue;
                double bottomPointMax = yMax - _verticalErrorValue;
                double topPointMin = yMin + _verticalErrorValue;
                double topPointMax = yMax + _verticalErrorValue;
                XRange = new DoubleRange(Math.Min(leftPointMin, rightPointMin), Math.Max(leftPointMax, rightPointMax));
                errorBarSeries.XRange += XRange;
                
                for (int i = 0; i < _index; i++)
                {
                    if (errorBarSeries.Type == ErrorBarType.Percentage)
                    {
                        if ((errorBarSeries.ActualXAxis is DateTimeAxis) || (errorBarSeries.ActualYAxis is CategoryAxis))
                        {
                            _horizontalErrorValue = (i + 1) * (errorBarSeries.HorizontalErrorValue / 100);
                            _verticalErrorValue = yValues[i] * (errorBarSeries.VerticalErrorValue / 100);
                        }
                        else
                        {
                            _horizontalErrorValue = xValues[i] * (errorBarSeries.HorizontalErrorValue / 100);
                            _verticalErrorValue = yValues[i] * (errorBarSeries.VerticalErrorValue / 100);
                        }
                    }
                    else if (errorBarSeries.Type == ErrorBarType.Custom)
                    {
                        _horizontalErrorValue = errorBarSeries.HorizontalErrorValues.Count > 0 ? errorBarSeries.HorizontalErrorValues[i] : 0;
                        _verticalErrorValue = errorBarSeries.VerticalErrorValues.Count > 0 ? errorBarSeries.VerticalErrorValues[i] : 0;
                    }

                    if (errorBarSeries.Type == ErrorBarType.StandardDeviation)
                    {
                        if (errorBarSeries.HorizontalDirection == ErrorBarDirection.Plus)
                        {
                            _leftPointCollection.Add(new Point(horSdValue[0], yValues[i]));
                            _rightPointCollection.Add(new Point(horSdValue[0] + _horizontalErrorValue, yValues[i]));
                        }
                        else if (errorBarSeries.HorizontalDirection == ErrorBarDirection.Minus)
                        {
                            _leftPointCollection.Add(new Point(horSdValue[0] - _horizontalErrorValue, yValues[i]));
                            _rightPointCollection.Add(new Point(horSdValue[0], yValues[i]));
                        }
                        else
                        {
                            _leftPointCollection.Add(new Point(horSdValue[0] - _horizontalErrorValue, yValues[i]));
                            _rightPointCollection.Add(new Point(horSdValue[0] + _horizontalErrorValue, yValues[i]));
                        }

                        if (errorBarSeries.VerticalDirection == ErrorBarDirection.Plus)
                        {
                            _bottomPointCollection.Add(new Point(xValues[i], verSdValue[0]));
                            _topPointCollection.Add(new Point(xValues[i], verSdValue[0] + _verticalErrorValue));
                        }
                        else if (errorBarSeries.VerticalDirection == ErrorBarDirection.Minus)
                        {
                            _bottomPointCollection.Add(new Point(xValues[i], verSdValue[0] - _verticalErrorValue));
                            _topPointCollection.Add(new Point(xValues[i], verSdValue[0]));
                        }
                        else
                        {
                            _bottomPointCollection.Add(new Point(xValues[i], verSdValue[0] - _verticalErrorValue));
                            _topPointCollection.Add(new Point(xValues[i], verSdValue[0] + _verticalErrorValue));
                        }
                    }
                    else
                    {
                        if (errorBarSeries.HorizontalDirection == ErrorBarDirection.Plus)
                        {
                            _leftPointCollection.Add(new Point(xValues[i], yValues[i]));
                            _rightPointCollection.Add(new Point(xValues[i] + _horizontalErrorValue, yValues[i]));
                        }
                        else if (errorBarSeries.HorizontalDirection == ErrorBarDirection.Minus)
                        {
                            _leftPointCollection.Add(new Point(xValues[i] - _horizontalErrorValue, yValues[i]));
                            _rightPointCollection.Add(new Point(xValues[i], yValues[i]));
                        }
                        else
                        {
                            _leftPointCollection.Add(new Point(xValues[i] - _horizontalErrorValue, yValues[i]));
                            _rightPointCollection.Add(new Point(xValues[i] + _horizontalErrorValue, yValues[i]));
                        }

                        if (errorBarSeries.VerticalDirection == ErrorBarDirection.Plus)
                        {
                            _bottomPointCollection.Add(new Point(xValues[i], yValues[i]));
                            _topPointCollection.Add(new Point(xValues[i], yValues[i] + _verticalErrorValue));
                        }
                        else if (errorBarSeries.VerticalDirection == ErrorBarDirection.Minus)
                        {
                            _bottomPointCollection.Add(new Point(xValues[i], yValues[i] - _verticalErrorValue));
                            _topPointCollection.Add(new Point(xValues[i], yValues[i]));
                        }
                        else
                        {
                            _bottomPointCollection.Add(new Point(xValues[i], yValues[i] - _verticalErrorValue));
                            _topPointCollection.Add(new Point(xValues[i], yValues[i] + _verticalErrorValue));
                        }
                    }
                }

                if (errorBarSeries.Type == ErrorBarType.Percentage)
                {
                    double topVerticalPointMinY = _topPointCollection.Select(p => p.Y).Min();
                    double topVerticalPointMaxY = _topPointCollection.Select(p => p.Y).Max();
                    double bottomVerticalPointMinY = _bottomPointCollection.Select(p => p.Y).Min();
                    double bottomVerticalPointMaxY = _bottomPointCollection.Select(p => p.Y).Max();
                    YRange = new DoubleRange(Math.Min(bottomVerticalPointMinY, topVerticalPointMinY), Math.Max(bottomVerticalPointMaxY, topVerticalPointMaxY));
                }
                else
                {
                    YRange = new DoubleRange(Math.Min(bottomPointMin, topPointMin), Math.Max(bottomPointMax, topPointMax));
                }

                errorBarSeries.YRange += YRange;
            }
        }

        #endregion

        #region Protected Internal Methods

        /// <inheritdoc/>
        protected internal override void OnLayout()
        {
            if (Series is ErrorBarSeries errorBarSeries)
            {
                ErrorSegmentPoints.Clear();

                for (int i = 0; i < _index; i++)
                {
                    float horCapLineSize = errorBarSeries.HorizontalCapLineStyle != null
                        ? (float)errorBarSeries.HorizontalCapLineStyle.CapLineSize : 10;
                    float verLineCapSize = errorBarSeries.VerticalCapLineStyle != null
                        ? (float)errorBarSeries.VerticalCapLineStyle.CapLineSize : 10;
                    ErrorSegmentPoint[] errorSegment = new ErrorSegmentPoint[2];
                    
                    if (!(double.IsNaN(_leftPointCollection[i].Y) || double.IsNaN(_rightPointCollection[i].Y)))
                    {
                        float horRightPoint1 = errorBarSeries.TransformToVisibleX(_rightPointCollection[i].X, _rightPointCollection[i].Y);
                        float horRightPoint2 = errorBarSeries.TransformToVisibleY(_rightPointCollection[i].X, _rightPointCollection[i].Y);
                        float horLeftPoint3 = errorBarSeries.TransformToVisibleX(_leftPointCollection[i].X, _leftPointCollection[i].Y);
                        float horLeftPoint4 = errorBarSeries.TransformToVisibleY(_leftPointCollection[i].X, _leftPointCollection[i].Y);
                        errorSegment[0] = new ErrorSegmentPoint();
                        errorSegment[0].X1 = horRightPoint1;
                        errorSegment[0].Y1 = horRightPoint2;
                        errorSegment[0].X2 = horLeftPoint3;
                        errorSegment[0].Y2 = horLeftPoint4;
                        //Horizontal Right Cap
                        RectF rightRectangle = new RectF(horRightPoint1 - (horCapLineSize / 2), horRightPoint2 - (horCapLineSize / 2), horCapLineSize, horCapLineSize);
                        errorSegment[0].RightRect = rightRectangle;
                        RectF leftRectangle = new RectF(horLeftPoint3 - (horCapLineSize / 2), horLeftPoint4 - (horCapLineSize / 2), horCapLineSize, horCapLineSize);
                        errorSegment[0].LeftRect = leftRectangle;
                    }

                    if (!(double.IsNaN(_topPointCollection[i].Y) || double.IsNaN(_bottomPointCollection[i].Y)))
                    {
                        float verBottomPoint1 = errorBarSeries.TransformToVisibleX(_bottomPointCollection[i].X, _bottomPointCollection[i].Y);
                        float verBottomPoint2 = errorBarSeries.TransformToVisibleY(_bottomPointCollection[i].X, _bottomPointCollection[i].Y);
                        float verTopPoint1 = errorBarSeries.TransformToVisibleX(_topPointCollection[i].X, _topPointCollection[i].Y);
                        float verTopPoint2 = errorBarSeries.TransformToVisibleY(_topPointCollection[i].X, _topPointCollection[i].Y);
                        errorSegment[1] = new ErrorSegmentPoint();
                        errorSegment[1].X1 = verBottomPoint1;
                        errorSegment[1].Y1 = verBottomPoint2;
                        errorSegment[1].X2 = verTopPoint1;
                        errorSegment[1].Y2 = verTopPoint2;
                        RectF bottomRectangle = new RectF(verBottomPoint1 - (verLineCapSize / 2), verBottomPoint2 - (verLineCapSize / 2), verLineCapSize, verLineCapSize);
                        errorSegment[1].BottomRect = bottomRectangle;
                        RectF topRectangle = new RectF(verTopPoint1 - (verLineCapSize / 2), verTopPoint2 - (verLineCapSize / 2), verLineCapSize, verLineCapSize);
                        errorSegment[1].TopRect = topRectangle;
                    }

                    ErrorSegmentPoints.Add(errorSegment);
                }
            }
        }

        /// <inheritdoc/>
        protected internal override void Draw(ICanvas canvas)
        {
            if (Series is not ErrorBarSeries errorBarSeries || errorBarSeries.ChartArea == null)
            {
                return;
            }

            var isTransposed = errorBarSeries.ChartArea.IsTransposed;

            canvas.Alpha = Opacity;

            for (int i = 0; i < _index; i++)
            {
                if (errorBarSeries.Mode == ErrorBarMode.Horizontal)
                {
                    DrawErrorBar(canvas, i, false, errorBarSeries.HorizontalLineStyle, errorBarSeries.HorizontalCapLineStyle, isTransposed);
                }
                else if (errorBarSeries.Mode == ErrorBarMode.Vertical)
                {
                    DrawErrorBar(canvas, i, true, errorBarSeries.VerticalLineStyle, errorBarSeries.VerticalCapLineStyle, isTransposed);
                }
                else
                {
                    DrawErrorBar(canvas, i, false, errorBarSeries.HorizontalLineStyle, errorBarSeries.HorizontalCapLineStyle, isTransposed);
                    DrawErrorBar(canvas, i, true, errorBarSeries.VerticalLineStyle, errorBarSeries.VerticalCapLineStyle, isTransposed);
                }
            }
        }

        #endregion

        #region Private Methods

        void DrawErrorBar(ICanvas canvas, int index, bool isVertical, ErrorBarLineStyle? lineStyle, ErrorBarCapLineStyle? capStyle, bool isTransposed)
        {
            _strokeColor = lineStyle != null ? lineStyle.Stroke.ToColor() : Fill.ToColor();
            _strokeWidth = lineStyle != null ? (float)lineStyle.StrokeWidth : (float)StrokeWidth;

            int vertical = isVertical ? 1 : 0;

            DrawLine(canvas, index, vertical, _strokeColor, _strokeWidth);

            _strokeColor = Fill.ToColor();
            _strokeWidth = (float)StrokeWidth;

            if (capStyle == null)
            {
                DrawCapLine(canvas, index, vertical, _strokeColor, _strokeWidth, isTransposed);
            }
            else if (capStyle.IsVisible)
            {
                _strokeColor = capStyle.Stroke.ToColor();
                _strokeWidth = (float)capStyle.StrokeWidth;

                if (lineStyle != null)
                {
                    canvas.StrokeLineCap = lineStyle.StrokeCap switch
                    {
                        ErrorBarStrokeCap.Square => LineCap.Square,
                        ErrorBarStrokeCap.Round => LineCap.Round,
                        _ => LineCap.Butt
                    };
                }

                DrawCapLine(canvas, index, vertical, _strokeColor, _strokeWidth, isTransposed);
            }
        }

        void DrawLine(ICanvas canvas, int index, int j, Color strokeColor, float strokeWidth)
        {
            canvas.StrokeColor = strokeColor;
            canvas.StrokeSize = strokeWidth;
            canvas.DrawLine(ErrorSegmentPoints[index][j].X1, ErrorSegmentPoints[index][j].Y1, ErrorSegmentPoints[index][j].X2, ErrorSegmentPoints[index][j].Y2);
        }

        void DrawCapLine(ICanvas canvas, int index, int j, Color strokeColor, float strokeWidth, bool isTransposed)
        {
            canvas.StrokeColor = strokeColor;
            canvas.StrokeSize = strokeWidth;

            var shapeType = j == 0 ? isTransposed ? ShapeType.HorizontalLine : ShapeType.VerticalLine : isTransposed ? ShapeType.VerticalLine : ShapeType.HorizontalLine;

            var rectTop = j == 0 ? ErrorSegmentPoints[index][j].LeftRect : ErrorSegmentPoints[index][j].BottomRect;
            var rectBottom = j == 0 ? ErrorSegmentPoints[index][j].RightRect : ErrorSegmentPoints[index][j].TopRect;

            canvas.DrawShape(rectTop, shapeType, HasStroke, false);
            canvas.DrawShape(rectBottom, shapeType, HasStroke, false);
        }

        /// <summary>
        /// Standard Deviation Methods for the xValues and yValues
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        double[] GetSdErrorValue(IList<double> values)
        {
            var valueDoubles = new double[2];

            if (Series is not ErrorBarSeries errorBarSeries)
            {
                return valueDoubles;
            }

            var sum = values.Sum();
            var mean = sum / values.Count;
            var dev = new List<double>();
            var sQDev = new List<double>();

            for (var i = 0; i < values.Count; i++)
            {
                dev.Add(values[i] - mean);
                sQDev.Add(dev[i] * dev[i]);
            }

            var sumSqDev = sQDev.Sum(x => x);

            var sDValue = Math.Sqrt(sumSqDev / (values.Count - 1));
            var sDErrorValue = sDValue / Math.Sqrt(values.Count);
            valueDoubles[0] = mean;
            valueDoubles[1] = errorBarSeries.Type == ErrorBarType.StandardDeviation ? sDValue : sDErrorValue;
            return valueDoubles;
        }

        #endregion

        #endregion
    }

    /// <summary>
    /// Represents the coordinates and bounding rectangles for the error bar chart segment.
    /// </summary>
    internal class ErrorSegmentPoint
    {
        #region Properties

        #region Public  Properties

        public float X1 { get; set; }
        public float Y1 { get; set; }
        public float X2 { get; set; }
        public float Y2 { get; set; }
        public RectF LeftRect { get; set; }
        public RectF RightRect { get; set; }
        public RectF TopRect { get; set; }
        public RectF BottomRect { get; set; }

        #endregion

        #endregion
    }
}
