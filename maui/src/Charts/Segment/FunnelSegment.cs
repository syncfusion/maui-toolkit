using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Toolkit.Graphics.Internals;
using System;
using System.Linq;

namespace Syncfusion.Maui.Toolkit.Charts
{
    /// <summary>
    /// Represents a segment of the <see cref="SfFunnelChart"/>, responsible for rendering and customizing the appearance of individual funnel segments.
    /// </summary>
    public class FunnelSegment : ChartSegment, IPyramidLabels
    {
        #region Fields

        double _topRadius, _bottomRadius;
        internal IPyramidChartDependent? Chart { get; set; }
        bool _isBroken;
        RectF _actualBounds;
        float[] _values = new float[12];

        #endregion

        #region Properties

        /// <summary>
        /// Represents the draw points of the funnel segments.
        /// </summary>
        /// <remarks>
        /// <para>Funnel segments are often drawn with six points, which are listed below.</para>
        /// Points[0] : Top Left
        /// Points[1] : Top Right
        /// Points[2] : Middle Right
        /// Points[3] : Bottom Right
        /// Points[4] : Bottom Left
        /// Points[5] : Middle Left
        /// </remarks>
        public Point[] Points { get; internal set; } = new Point[6];
        double top;
        double bottom;
        double NeckWidth;
        double NeckHeight;

        double dataLabelYValue;
        object? dataLabelXValue;

        Rect labelRect;
        float labelXPosition;
        float labelYPosition;
        Point[]? linePoints = new Point[3];
        bool isIntersected = false;
        bool isLabelVisible = true;
        Size dataLabelSize = Size.Zero;
        Size actualSize = Size.Zero;
        Point slopeCenter = Point.Zero;
        DataLabelPlacement position = DataLabelPlacement.Auto;
        float IPyramidLabels.DataLabelX { get => labelXPosition; set => labelXPosition = value; }
        float IPyramidLabels.DataLabelY { get => labelYPosition; set => labelYPosition = value; }
        Point[]? IPyramidLabels.LinePoints { get => linePoints; set => linePoints = value; }
        Rect IPyramidLabels.LabelRect { get => labelRect; set => labelRect = value; }
        Size IPyramidLabels.DataLabelSize { get => dataLabelSize; set => dataLabelSize = value; }
        Size IPyramidLabels.ActualLabelSize { get => actualSize; set => actualSize = value; }
        string IPyramidLabels.DataLabel => LabelContent ?? string.Empty;
        bool IPyramidLabels.IsEmpty { get => Empty; set => Empty = value; }
        bool IPyramidLabels.IsIntersected { get => isIntersected; set => isIntersected = value; }
        bool IPyramidLabels.IsLabelVisible { get => isLabelVisible; set => isLabelVisible = value; }
        Brush IPyramidLabels.Fill => Fill ?? Brush.Transparent;
        Point IPyramidLabels.SlopePoint => slopeCenter;
        DataLabelPlacement IPyramidLabels.Position { get => position; set => position = value; }

        #endregion

        #region Methods

        #region Internal methods

        /// <summary>
        /// Converts the data points to corresponding screen points for rendering the funnel segment.
        /// </summary>
        internal void SetData(double y, double height, double neckWidth, double neckHeight, object? xValue, double yValue)
        {
            top = y;
            bottom = y + height;
            _topRadius = y / 2;
            _bottomRadius = (y + height) / 2;
            NeckWidth = neckWidth;
            NeckHeight = neckHeight;
            dataLabelXValue = xValue;
            dataLabelYValue = yValue;
            Empty = double.IsNaN(yValue) || yValue == 0;
        }

        internal override int GetDataPointIndex(float x, float y)
        {
            if (SegmentBounds.Contains(x, y))
            {
                var slopeLeft = Math.Atan((_values[8] - _values[0]) / (_values[9] - _values[1]));
                var slopeRight = Math.Atan((_values[6] - _values[2]) / (_values[7] - _values[3]));
                var left = Math.Atan((_values[8] - x) / (_values[9] - y));
                var right = Math.Atan((_values[6] - x) / (_values[7] - y));

                if (_isBroken)
                {
                    if (_values[5] <= y)
                    {
                        if ((_values[4] >= x) && (_values[10] <= x))
                        {
                            return Index;
                        }
                    }
                    else
                    {
                        slopeLeft = Math.Atan((_values[10] - _values[0]) / (_values[11] - _values[1]));
                        slopeRight = Math.Atan((_values[4] - _values[2]) / (_values[5] - _values[3]));
                        left = Math.Atan((_values[10] - x) / (_values[11] - y));
                        right = Math.Atan((_values[4] - x) / (_values[5] - y));

                        if (left <= slopeLeft && right >= slopeRight)
                        {
                            return Index;
                        }
                    }
                }
                else if (left <= slopeLeft && right >= slopeRight)
                {
                    return Index;
                }
            }

            return -1;
        }

        #endregion

        #region Protected internal methods 

        /// <inheritdoc/>
        protected internal override void OnLayout()
        {
            if (Chart != null)
            {
                _actualBounds = new RectF(new Point(0, 0), Chart.SeriesBounds.Size);
                double boundsHeight = _actualBounds.Height;
                double boundsWidth = _actualBounds.Width;
                double boundsTop = _actualBounds.Top;
                double boundsLeft = _actualBounds.Left;
                double minWidth = 0.5f * (1 - (NeckWidth / boundsWidth));
                double bottomY = minWidth * (bottom - top) / (_bottomRadius - _topRadius);
                double topWidth = Math.Min(_topRadius, minWidth);
                double bottomWidth = Math.Min(_bottomRadius, minWidth);
                _isBroken = (_topRadius >= minWidth) ^ (_bottomRadius > minWidth);

                _values = new float[12];
                _values[0] = (float)(boundsLeft + (topWidth * boundsWidth));
                _values[1] = _values[3] = (float)(boundsTop + (top * boundsHeight));
                _values[2] = (float)(boundsLeft + ((float)(1 - topWidth) * boundsWidth));
                _values[6] = (float)(boundsLeft + ((float)(1 - bottomWidth) * boundsWidth));
                _values[7] = _values[9] = (float)(boundsTop + (bottom * boundsHeight));
                _values[4] = _isBroken ? (float)(boundsLeft + ((float)(1 - minWidth) * boundsWidth)) : _values[2];
                _values[5] = _values[11] = _isBroken ? ((float)(boundsTop + (bottomY * boundsHeight))) : _values[3];
                _values[8] = (float)(boundsLeft + ((float)bottomWidth * boundsWidth));
                _values[10] = _isBroken? (float)(boundsLeft + ((float)minWidth * boundsWidth)) : _values[0];

                Points[0] = new Point(_values[0], _values[1]);
                Points[1] = new Point(_values[2], _values[3]);
                Points[2] = new Point(_values[4], _values[5]);
                Points[3] = new Point(_values[6], _values[7]);
                Points[4] = new Point(_values[8], _values[9]);
                Points[5] = new Point(_values[10], _values[11]);

                SegmentBounds = new RectF(_values[0], _values[1], _values[2] - _values[0], _values[7] - _values[1]);
            }
        }

        /// <inheritdoc/>
        protected internal override void Draw(ICanvas canvas)
        {
            if (_values == null)
            {
                return;
            }

            PathF path = new PathF();

            path.MoveTo(Points[0]);

            for (int i = 1; i < Points.Length; i++)
            {
                path.LineTo(Points[i]);
            }

            path.Close();

            canvas.SetFillPaint(Fill, path.Bounds);
            canvas.FillPath(path);

            if (HasStroke)
            {
                canvas.StrokeColor = Stroke.ToColor();
                canvas.StrokeSize = (float)StrokeWidth;
                canvas.DrawPath(path);
            }
        }

        internal override void OnDataLabelLayout()
        {
            if (Chart != null && !Empty && Chart.DataLabelSettings.LabelStyle is ChartLabelStyle style)
            {
                var bounds = Chart.SeriesBounds;
                labelXPosition = (float)bounds.X + SegmentBounds.Center.X;
                labelYPosition = (float)bounds.Y + SegmentBounds.Center.Y;

                LabelContent = Chart.DataLabelSettings.GetLabelContent(dataLabelXValue, dataLabelYValue);
                double left;
                double right;
                
                if (_isBroken)
                {
                    right = (_values[5] + bounds.Y > labelYPosition ? ((_values[4] + _values[2]) / 2) : ((_values[4] + _values[6]) / 2)) + bounds.Left;
                    left = (_values[11] + bounds.Y > labelYPosition ? ((_values[10] + _values[0]) / 2) : ((_values[10] + _values[8]) / 2)) + bounds.Left;
                }
                else
                {
                    right = ((_values[2] + _values[6]) / 2) + bounds.Left;
                    left = ((_values[0] + _values[8]) / 2) + bounds.Left;
                }

                var y = ((_values[3] + _values[7]) / 2) + bounds.Top;

                slopeCenter = new Point(right - 3, y);

                //Add the DataLabel properties.
                UpdateDataLabels();

                dataLabelSize = Chart.LabelTemplate is null ? LabelContent.Measure(style) : CalculateTemplateSize();
                actualSize = new Size(dataLabelSize.Width + style.Margin.HorizontalThickness, dataLabelSize.Height + style.Margin.VerticalThickness);
                position = DataLabelPlacement.Inner;

                switch (Chart.DataLabelSettings.LabelPlacement)
                {
                    case DataLabelPlacement.Center:
                    case DataLabelPlacement.Auto:
                        if (actualSize.Width > right - left || actualSize.Height > SegmentBounds.Height)
                            position = DataLabelPlacement.Outer;
                        break;
                    case DataLabelPlacement.Outer:
                        position = DataLabelPlacement.Outer;
                        break;
                }


                Chart.DataLabelHelper.AddLabel(this);
            }
        }

        internal override void UpdateDataLabels()
        {
            var dataLabelSettings = Chart?.DataLabelSettings;

            if (Chart == null || dataLabelSettings == null)
            {
                return;
            }

            if (DataLabels != null && DataLabels.Count > 0)
            {
                ChartDataLabel dataLabel = DataLabels[0];

                dataLabel.LabelStyle = dataLabelSettings.LabelStyle;
                dataLabel.Background = dataLabelSettings.LabelStyle.Background;
                dataLabel.Index = Index;
                dataLabel.Item = Item;
                dataLabel.Label = LabelContent ?? string.Empty;
            }
        }

        Size CalculateTemplateSize()
        {
            if (Chart is SfFunnelChart funnelChart)
            {
                if (funnelChart.LabelTemplateView != null && funnelChart.LabelTemplateView.Any())
                {
                    var templateView = funnelChart.LabelTemplateView.Cast<View>().FirstOrDefault(child => DataLabels[0] == child.BindingContext) as DataLabelItemView;

                    if (templateView != null && templateView.ContentView is View content)
                    {
                        if (!content.DesiredSize.IsZero)
                        {
                            return content.DesiredSize;
                        }

                        var desiredSize = templateView.Measure(double.PositiveInfinity, double.PositiveInfinity, MeasureFlags.IncludeMargins).Request;

                        if (desiredSize.IsZero)
                            return content.Measure(double.PositiveInfinity, double.PositiveInfinity, MeasureFlags.IncludeMargins).Request;

                        return desiredSize;
                    }
                }
            }

            return SizeF.Zero;
        }

        #endregion

        #endregion

    }
}
