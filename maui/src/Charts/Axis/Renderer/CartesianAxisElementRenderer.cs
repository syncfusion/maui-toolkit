using System;
using System.Collections.Generic;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.Charts
{
	internal class CartesianAxisElementRenderer : ILayoutCalculator
    {
        #region Fields
        SizeF _desiredSize;
        double _left, _top;
        ChartAxis _chartAxis;
        bool ILayoutCalculator.IsVisible { get; set; } = true;

        #endregion

        #region Constructor
        public CartesianAxisElementRenderer(ChartAxis axis)
        {
            _chartAxis = axis;
        }
        #endregion

        #region Methods
        public void OnDraw(ICanvas canvas, Size finalSize)
        {
            if (_chartAxis.AxisLineStyle.CanDraw())
            {
                canvas.CanvasSaveState();
                DrawAxisLine(canvas);
                canvas.CanvasRestoreState();
            }

            if (_chartAxis.MajorTickStyle.CanDraw())
            {
                canvas.CanvasSaveState();
                DrawTicks(canvas, _chartAxis.MajorTickStyle, _chartAxis.TickPositions, false);
                canvas.CanvasRestoreState();
            }

            var rangeAxis = _chartAxis as RangeAxisBase;
            if (_chartAxis.SmallTickRequired && rangeAxis != null && rangeAxis.MinorTickStyle.CanDraw())
            {
                canvas.CanvasSaveState();
                DrawTicks(canvas, rangeAxis.MinorTickStyle, rangeAxis.SmallTickPoints, true);
                canvas.CanvasRestoreState();
            }
        }

        public double GetLeft()
        {
            return _left;
        }

        public void SetLeft(double left)
        {
            _left = left;
        }

        public double GetTop()
        {
            return _top;
        }

        public void SetTop(double top)
        {
            _top = top;
        }

        public Size GetDesiredSize()
        {
            return _desiredSize;
        }

        public Size Measure(Size availableSize)
        {
            SizeF size;
            double smallTickLineSize = 0;

            if (_chartAxis.SmallTickRequired)
            {
                smallTickLineSize = ((RangeAxisBase)_chartAxis).MinorTickStyle.TickSize;
            }

            if (_chartAxis.IsVertical)
            {
                size = new Size(Math.Max(_chartAxis.MajorTickStyle.TickSize, smallTickLineSize) + _chartAxis.AxisLineStyle.StrokeWidth, availableSize.Width);
            }
            else
            {
                size = new Size(availableSize.Height, Math.Max(_chartAxis.MajorTickStyle.TickSize, smallTickLineSize) + _chartAxis.AxisLineStyle.StrokeWidth);
            }

            _desiredSize = size;
            return size;
        }

        #region private members
        void DrawTicks(ICanvas canvas, ChartAxisTickStyle tickStyle, List<double> tickPoints, bool isDrawMinorTick)
        {
            AxisElementPosition elementPosition = _chartAxis.TickPosition;
            bool isOpposed = _chartAxis.IsOpposed();

            float tickSize = (float)tickStyle.TickSize;
            float axisLineWidth = (float)_chartAxis.AxisLineStyle.StrokeWidth;

            float x1 = 0, y1 = 0, x2 = 0, y2 = 0;
            if (tickPoints.Count > 0)
            {
                if (!_chartAxis.IsVertical)
                {
                    y1 = elementPosition == AxisElementPosition.Inside ? isOpposed ? axisLineWidth : _desiredSize.Height - tickSize - axisLineWidth : isOpposed ? _desiredSize.Height - tickSize - axisLineWidth : axisLineWidth;
                    y2 = y1 + tickSize;
                }
                else
                {
                    x1 = elementPosition == AxisElementPosition.Inside ? isOpposed ? _desiredSize.Width - tickSize - axisLineWidth : axisLineWidth : isOpposed ? axisLineWidth : _desiredSize.Width - tickSize - axisLineWidth;
                    x2 = x1 + tickSize;
                }
            }

            foreach (var position in tickPoints)
            {
                double value = _chartAxis.ValueToCoefficient(position);

                if (!_chartAxis.IsVertical)
                {
                    x1 = x2 = (float)_chartAxis.GetActualPlotOffsetStart() + (float)Math.Round(_chartAxis.RenderedRect.Width * value);
                }
                else
                {
                    if (!_chartAxis.IsPolarArea)
                        y1 = y2 = (float)_chartAxis.GetActualPlotOffsetEnd() + (float)Math.Round(_chartAxis.RenderedRect.Height * (1 - value));
                    else
                    {
                        var angle = _chartAxis.PolarStartAngle;
                        value = (angle == 0 || angle == 90) ? value : (-value);
                        if (angle == 0 || angle == 180)
                        {
                            x1 = x2 = (float)(_chartAxis.ActualPlotOffset + (float)Math.Round(_chartAxis.RenderedRect.Height * value) + _desiredSize.Width);
                            if (elementPosition == AxisElementPosition.Inside)
                            {
                                y1 = isOpposed ? _desiredSize.Height - tickSize - axisLineWidth : axisLineWidth;
                            }
                            else
                            {
                                y1 = isOpposed ? axisLineWidth : _desiredSize.Height + axisLineWidth;
                            }

                            y2 = y1 + tickSize;
                        }
                        else
                        {
                            y1 = y2 = (float)(_chartAxis.ActualPlotOffset + (float)Math.Round(_chartAxis.RenderedRect.Height * (1 + value)));
                            if (elementPosition == AxisElementPosition.Inside)
                            {
                                x1 = isOpposed ? _desiredSize.Width - tickSize - axisLineWidth : axisLineWidth;
                            }
                            else
                            {
                                x1 = isOpposed ? axisLineWidth : _desiredSize.Width - tickSize - axisLineWidth;
                            }

                            x2 = x1 + tickSize;
                        }
                    }
                }

                canvas.StrokeSize = (float)tickStyle.StrokeWidth;
                canvas.StrokeColor = tickStyle.Stroke.ToColor();

                if (!isDrawMinorTick)
                {
                    _chartAxis.DrawMajorTick(canvas, position, new PointF(x1, y1), new PointF(x2, y2));
                }
                else
                {
                    _chartAxis.DrawMinorTick(canvas, position, new PointF(x1, y1), new PointF(x2, y2));
                }
            }
        }

        void DrawAxisLine(ICanvas canvas)
        {
            float x1, y1, x2, y2;
            float width = (float)_chartAxis.ArrangeRect.Width;
            float height = (float)_chartAxis.ArrangeRect.Height;

            var style = _chartAxis.AxisLineStyle;

            bool isOpposed = _chartAxis.IsOpposed() ^ _chartAxis.TickPosition == AxisElementPosition.Inside;
            float axisLineOffset = (float)_chartAxis.AxisLineOffset;
            float offset = ((float)style.StrokeWidth) / 2;

            if (!_chartAxis.IsVertical)
            {
                x1 = axisLineOffset;
                x2 = width - axisLineOffset;
                y1 = y2 = isOpposed ? _desiredSize.Height - offset : offset;
            }
            else
            {
                if (!_chartAxis.IsPolarArea)
                {
                    x1 = x2 = isOpposed ? offset : _desiredSize.Width - offset;
                    y1 = axisLineOffset;
                    y2 = height - axisLineOffset;
                }
                else
                {
                    var angle = _chartAxis.PolarStartAngle;
                    if (angle == 0 || angle == 180)
                    {
                        height = angle == 0 ? height : (-height);
                        y1 = y2 = isOpposed ? offset : _desiredSize.Height - offset;
                        x1 = axisLineOffset + _desiredSize.Width;
                        x2 = height + _desiredSize.Width + axisLineOffset;
                    }
                    else
                    {
                        x1 = x2 = isOpposed ? offset : _desiredSize.Width - offset;
                        y1 = axisLineOffset;
                        y2 = height - axisLineOffset;
                        if (angle == 90)
                        {
                            y1 = y2;
                            y2 = height + _desiredSize.Height;
                        }
                    }
                }
            }

            canvas.StrokeColor = style.Stroke.ToColor();
            canvas.StrokeSize = (float)style.StrokeWidth;

            if (style.StrokeDashArray != null)
            {
                canvas.StrokeDashPattern = style.StrokeDashArray.ToFloatArray();
            }

            _chartAxis.DrawAxisLine(canvas, x1, y1, x2, y2);
        }
        #endregion
        #endregion
    }

}
