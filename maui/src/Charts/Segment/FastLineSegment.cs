using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.Charts
{
    /// <summary>
    /// Represents a segment of the <see cref="FastLineSeries"/> chart, responsible for drawing the line and managing its visual appearance.
    /// </summary>
    public class FastLineSegment : CartesianSegment, ILineDrawing
    {
        #region Fields

        /// <summary>
        /// Array holds drawing pixel positions.
        /// </summary>
        float[]? _drawPoints;
        int _arrayCount;
        internal List<double>? XValues;
        internal IList<double>? YValues;
        bool _enableAntiAliasing = false;
        Brush? _stroke;

#if __ANDROID__
        readonly float _displayScale;
#endif

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="FastLineSegment"/> class.
        /// </summary>
        public FastLineSegment()
        {
#if __ANDROID__
#nullable disable
           _displayScale = Android.Content.Res.Resources.System.DisplayMetrics.Density;
#nullable enable
#endif
        }

        #endregion

        #region Properties

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        Color ILineDrawing.Stroke
        {
            get
            {
                if (Fill is SolidColorBrush brush)
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
            get => StrokeWidth;
            set => StrokeWidth = value;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        bool ILineDrawing.EnableAntiAliasing
        {
            get
            {
                if (Series is FastLineSeries fastLineSeries)
                    return fastLineSeries.EnableAntiAliasing;
                else
                    return false;
            }
            set => _enableAntiAliasing = value;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        float ILineDrawing.Opacity
        {
            get => Opacity;
            set => Opacity = value;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        DoubleCollection? ILineDrawing.StrokeDashArray
        {
            get => StrokeDashArray;
            set => StrokeDashArray = value;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected internal override void Draw(ICanvas canvas)
        {
            if (Series == null || Empty || _drawPoints == null)
            {
                return;
            }

            if (Series.CanAnimate())
            {
                AnimateSeriesClipRect(canvas, Series.AnimationValue);
            }

            canvas.DrawLines(_drawPoints, this);
        }

        /// <inheritdoc />
        protected internal override void OnLayout()
        {
            if (Series is not FastLineSeries fastLineSeries || fastLineSeries.ActualXAxis == null || fastLineSeries.ActualYAxis == null)
            {
                return;
            }

            var chart = fastLineSeries.ChartArea;

            if (XValues == null || chart == null || YValues == null)
                return;

            bool isTransposed = chart.IsTransposed;
            float preXPos = 0, preYPos = 0;
            double preXValue = 0d, preYValue = 0d;
            int dataCount = XValues.Count;
            float[] linePoints = new float[dataCount * 4];
            _arrayCount = 0;

            var xAxis = fastLineSeries.ActualXAxis;
            float xAxisWidth = xAxis.RenderedRect.Width;
            float xAxisHeight = xAxis.RenderedRect.Height;
            float xAxisLeftOffset = xAxis.LeftOffset;
            float xAxisTopOffset = xAxis.TopOffset;
            double xStart = xAxis.VisibleRange.Start;
            double xDelta = xAxis.VisibleRange.Delta;
            double xEnd = xAxis.VisibleRange.End;
            bool xAxisIsVertical = xAxis.IsVertical;
            bool xAxisIsInversed = xAxis.IsInversed;

            var yAxis = fastLineSeries.ActualYAxis;
            float yAxisHeight = yAxis.RenderedRect.Height;
            float yAxisWidth = yAxis.RenderedRect.Width;
            float yAxisTopOffset = yAxis.TopOffset;
            float yAxisLeftOffset = yAxis.LeftOffset;
            double yDelta = yAxis.VisibleRange.Delta;
            double yEnd = yAxis.VisibleRange.End;
            double yStart = yAxis.VisibleRange.Start;
            bool yAxisIsVertical = yAxis.IsVertical;
            bool yAxisIsInversed = yAxis.IsInversed;

            double xSize = isTransposed ? xAxisHeight : xAxisWidth;
            double ySize = isTransposed ? yAxisWidth : yAxisHeight;

            double xTolerance = fastLineSeries.ToleranceCoefficient * (xDelta > 0 ? xDelta : -xDelta) / xSize;
            double yTolerance = fastLineSeries.ToleranceCoefficient * (yDelta > 0 ? yDelta : -yDelta) / ySize;

            if (XValues != null)
            {
                if (!fastLineSeries.IsIndexed)
                {
                    if (dataCount > 0)
                    {
                        preXValue = XValues[0];
                        preYValue = YValues[0];

                        preXPos = fastLineSeries.TransformToVisibleX(preXValue, preYValue);
                        preYPos = fastLineSeries.TransformToVisibleY(preXValue, preYValue);
#if IOS || MACCATALYST
                        linePoints[_arrayCount++] = preXPos;
                        linePoints[_arrayCount++] = preYPos;
#endif
                    }

                    for (int i = 1; i < dataCount; i++)
                    {
                        if (i >= XValues.Count || i >= YValues.Count)
                            break;

                        double xValue = XValues[i];
                        double yValue = YValues[i];

                        if ((xEnd <= xValue && xStart >= XValues[i - 1]) &&
                            ((yStart >= yValue && yEnd <= YValues[i - 1])
                            || (yEnd <= yValue && yStart >= YValues[i - 1])))
                        {
                            float x = fastLineSeries.TransformToVisibleX(xValue, yValue);
                            float y = fastLineSeries.TransformToVisibleY(xValue, yValue);
                            preXPos = fastLineSeries.TransformToVisibleX(XValues[i - 1], YValues[i - 1]);
                            preYPos = fastLineSeries.TransformToVisibleY(XValues[i - 1], YValues[i - 1]);

                            UpdateLinePoints(linePoints, preXPos, preYPos, x, y);

                            preXPos = x;
                            preYPos = y;
                            preXValue = xValue;
                            preYValue = yValue;
                        }
                        else if ((xValue <= xEnd && xValue >= xStart) ||
                            (yValue >= yStart && yValue <= yEnd) ||
                            (preXValue <= xEnd && preXValue >= xStart) ||
                            (preYValue <= yEnd && preYValue >= yStart) ||
                            ((i != dataCount - 1) && ((XValues[i + 1] <= xEnd && XValues[i + 1] >= xStart) ||
                            (YValues[i + 1] >= yStart && YValues[i + 1] <= yEnd))))
                        {
                            double xDiff = preXValue - xValue;
                            double yDiff = preYValue - yValue;

                            if ((xDiff > 0 ? xDiff : -xDiff) >= xTolerance ||
                                (yDiff > 0 ? yDiff : -yDiff) >= yTolerance ||
                                double.IsNaN(xDiff) || double.IsNaN(yDiff))
                            {
                                float x, y;

                                if (isTransposed)
                                {
                                    x = ValueToPoint(yValue, yStart, yDelta, yAxisIsInversed, yAxisIsVertical,
                                        yAxisWidth, yAxisHeight, yAxisLeftOffset, yAxisTopOffset);

                                    y = ValueToPoint(xValue, xStart, xDelta, xAxisIsInversed, xAxisIsVertical,
                                      xAxisWidth, xAxisHeight, xAxisLeftOffset, xAxisTopOffset);
                                }
                                else
                                {
                                    x = ValueToPoint(xValue, xStart, xDelta, xAxisIsInversed, xAxisIsVertical,
                                       xAxisWidth, xAxisHeight, xAxisLeftOffset, xAxisTopOffset);

                                    y = ValueToPoint(yValue, yStart, yDelta, yAxisIsInversed, yAxisIsVertical,
                                       yAxisWidth, yAxisHeight, yAxisLeftOffset, yAxisTopOffset);
                                }

                                UpdateLinePoints(linePoints, preXPos, preYPos, x, y);

                                preXPos = x;
                                preYPos = y;
                                preXValue = xValue;
                                preYValue = yValue;
                            }
                        }
                    }
                }
                else
                {
                    if (dataCount > 0)
                    {
                        preXValue = XValues[0];
                        preYValue = YValues[0];
                        preXPos = fastLineSeries.TransformToVisibleX(preXValue, preYValue);
                        preYPos = fastLineSeries.TransformToVisibleY(preXValue, preYValue);
#if IOS || MACCATALYST
                        linePoints[_arrayCount++] = preXPos;
                        linePoints[_arrayCount++] = preYPos;
#endif
                    }

                    for (int i = 1; i < dataCount; i++)
                    {
                        if (i >= YValues.Count)
                            break;

                        double yValue = YValues[i];
                        if ((i <= xEnd + 1) && (i >= xStart - 1))
                        {
                            double xDiff = preXValue - i;
                            double yDiff = preYValue - yValue;

                            if ((xDiff > 0 ? xDiff : -xDiff) >= xTolerance
                                || (yDiff > 0 ? yDiff : -yDiff) >= yTolerance || double.IsNaN(xDiff) || double.IsNaN(yDiff))
                            {
                                float x, y;

                                if (isTransposed)
                                {
                                    x = ValueToPoint(yValue, yStart, yDelta, yAxisIsInversed, yAxisIsVertical,
                                           yAxisWidth, yAxisHeight, yAxisLeftOffset, yAxisTopOffset);

                                    y = ValueToPoint(XValues[i], xStart, xDelta, xAxisIsInversed, xAxisIsVertical,
                                       xAxisWidth, xAxisHeight, xAxisLeftOffset, xAxisTopOffset);
                                }
                                else
                                {
                                    x = ValueToPoint(XValues[i], xStart, xDelta, xAxisIsInversed, xAxisIsVertical,
                                       xAxisWidth, xAxisHeight, xAxisLeftOffset, xAxisTopOffset);

                                    y = ValueToPoint(yValue, yStart, yDelta, yAxisIsInversed, yAxisIsVertical,
                                        yAxisWidth, yAxisHeight, yAxisLeftOffset, yAxisTopOffset);
                                }

                                UpdateLinePoints(linePoints, preXPos, preYPos, x, y);

                                preXPos = x;
                                preYPos = y;
                                preXValue = XValues[i];
                                preYValue = yValue;
                            }
                        }
                    }
                }
            }

            _drawPoints = new float[_arrayCount];

            //Here, we copied calculated linePoints to DrawPoints. Because we need float array draw points for fast line rendering. 
            //So, we created empty float array with user given data size. Based on series's ToleranceCoefficient, we reduce draw points. 
            //Then we copy calculated draw points alone for drawing. 
            Array.Copy(linePoints, 0, _drawPoints, 0, _arrayCount);
        }

        /// <summary>
        /// Converts the data points to corresponding screen points for rendering the fastLine segment.
        /// </summary>
        internal void SetData(List<double> xValues, IList<double> yValues)
        {
            if (Series is XYDataSeries series && series.ActualYAxis != null)
            {
                double xMin = double.MaxValue, xMax = double.MinValue, yMin = double.MaxValue, yMax = double.MinValue;
                XValues = xValues;
                YValues = yValues;
                int dataCount = YValues.Count;

                if (dataCount == 0)
                {
                    return;
                }

                if (series.IsIndexed)
                {
                    xMin = XValues[0];
                    xMax = XValues[dataCount - 1];
                    for (int i = 0; i < dataCount; i++)
                    {
                        if (i >= YValues.Count)
                            break;

                        double yValue = YValues[i];

                        if (!Empty && double.IsNaN(yValue))
                            Empty = true;

                        if (yValue > yMax)
                        {
                            yMax = yValue;
                        }

                        if (yValue < yMin)
                        {
                            yMin = yValue;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < dataCount; i++)
                    {
                        if (i >= XValues.Count || i >= YValues.Count)
                            break;

                        var xValue = XValues[i];
                        var yValue = YValues[i];

                        if (!Empty && (double.IsNaN(yValue) || double.IsNaN(xValue)))
                            Empty = true;

                        if (xValue > xMax)
                        {
                            xMax = xValue;
                        }

                        if (xValue < xMin)
                        {
                            xMin = xValue;
                        }

                        if (yValue > yMax)
                        {
                            yMax = yValue;
                        }

                        if (yValue < yMin)
                        {
                            yMin = yValue;
                        }
                    }
                }

                if (xMin == double.MaxValue)
                {
                    xMin = double.NaN;
                }

                if (xMax == double.MinValue)
                {
                    xMax = double.NaN;
                }

                if (yMin == double.MaxValue)
                {
                    yMin = double.NaN;
                }

                if (yMax == double.MinValue)
                {
                    yMax = double.NaN;
                }


                Series.XRange += new DoubleRange(xMin, xMax);
                Series.YRange += new DoubleRange(yMin, yMax);
            }
        }

        /// <summary>
        /// Update calculated pixels values in array.
        /// </summary>
        /// <param name="linePoints"></param>
        /// <param name="preXPos"></param>
        /// <param name="preYPos"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        void UpdateLinePoints(float[] linePoints, float preXPos, float preYPos, float x, float y)
        {
            if (!double.IsNaN(preYPos))
            {
#if __ANDROID__
                linePoints[_arrayCount++] = preXPos * _displayScale;
                linePoints[_arrayCount++] = preYPos * _displayScale;
                linePoints[_arrayCount++] = x * _displayScale;
                linePoints[_arrayCount++] = y * _displayScale;
#elif WINDOWS
                linePoints[_arrayCount++] = preXPos;
                linePoints[_arrayCount++] = preYPos;
                linePoints[_arrayCount++] = x;
                linePoints[_arrayCount++] = y;
#else

                linePoints[_arrayCount++] = x;
                linePoints[_arrayCount++] = y;
#endif
            }
        }

        /// <summary>
        /// Method used to calculate data value to pixel value. 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="start"></param>
        /// <param name="delta"></param>
        /// <param name="isInversed"></param>
        /// <param name="isVertical"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="leftOffset"></param>
        /// <param name="topOffset"></param>
        /// <returns></returns>
        float ValueToPoint(double value, double start, double delta, bool isInversed,
           bool isVertical, float width, float height, float leftOffset, float topOffset)
        {
            double result;

            result = (value - start) / delta;

            float coefficient = (float)(isInversed ? 1f - result : result);

            if (!isVertical)
            {
                return (width * coefficient) + leftOffset;
            }
            else
            {
                return (height * (1 - coefficient)) + topOffset;
            }
        }

        internal override void OnDataLabelLayout()
        {
            if (Series is XYDataSeries xyDataSeries && xyDataSeries.LabelTemplate != null)
            {
                if (XValues == null || YValues == null) return;

                var dataLabelSettings = xyDataSeries.DataLabelSettings;
                ChartDataLabelStyle labelStyle = dataLabelSettings.LabelStyle;

                for (int i = 0; i < XValues.Count; i++)
                {
                    double x = XValues[i], y = YValues[i];

                    if (double.IsNaN(y)) continue;

                    Index = i;
                    InVisibleRange = xyDataSeries.IsDataInVisibleRange(x, y);
                    LabelContent = dataLabelSettings.GetLabelContent(YValues[i]);

                    if (DataLabels != null && DataLabels.Count > i)
                    {
                        var dataLabel = DataLabels[i];

                        dataLabel.LabelStyle = labelStyle;
                        dataLabel.Background = labelStyle.Background;
                        dataLabel.Index = i;
                        dataLabel.Item = xyDataSeries.ActualData?[i];
                        dataLabel.Label = LabelContent;

                        if (!InVisibleRange || IsEmpty)
                        {
                            LabelPositionPoint = new PointF(float.NaN, float.NaN);
                        }
                        else
                        {
                            xyDataSeries.CalculateDataPointPosition(i, ref x, ref y);
                            PointF labelPoint = new PointF((float)x, (float)y);
                            LabelPositionPoint = dataLabelSettings.CalculateDataLabelPoint(xyDataSeries, this, labelPoint, labelStyle);
                        }

                        dataLabel.XPosition = LabelPositionPoint.X;
                        dataLabel.YPosition = LabelPositionPoint.Y;
                    }
                }
            }
        }

        #endregion
    }
}
