using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.Charts
{
    /// <summary>
    /// Represents a segment of the <see cref="SfFunnelChart"/>, responsible for rendering and customizing the appearance of individual funnel segments.
    /// </summary>
    public partial class FunnelSegment : ChartSegment, IPyramidLabels
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
        double _top;
        double _bottom;
		double _neckWidth;
		double _neckHeight;

        double _dataLabelYValue;
        object? _dataLabelXValue;

        Rect _labelRect;
        float _labelXPosition;
        float _labelYPosition;
        Point[]? _linePoints = new Point[3];
        bool _isIntersected = false;
        bool _isLabelVisible = true;
        Size _dataLabelSize = Size.Zero;
        Size _actualSize = Size.Zero;
		Point _slopeCenter = Point.Zero;
        DataLabelPlacement _position = DataLabelPlacement.Auto;
        float IPyramidLabels.DataLabelX { get => _labelXPosition; set => _labelXPosition = value; }
        float IPyramidLabels.DataLabelY { get => _labelYPosition; set => _labelYPosition = value; }
        Point[]? IPyramidLabels.LinePoints { get => _linePoints; set => _linePoints = value; }
        Rect IPyramidLabels.LabelRect { get => _labelRect; set => _labelRect = value; }
        Size IPyramidLabels.DataLabelSize { get => _dataLabelSize; set => _dataLabelSize = value; }
        Size IPyramidLabels.ActualLabelSize { get => _actualSize; set => _actualSize = value; }
        string IPyramidLabels.DataLabel => LabelContent ?? string.Empty;
        bool IPyramidLabels.IsEmpty { get => Empty; set => Empty = value; }
        bool IPyramidLabels.IsIntersected { get => _isIntersected; set => _isIntersected = value; }
        bool IPyramidLabels.IsLabelVisible { get => _isLabelVisible; set => _isLabelVisible = value; }
        Brush IPyramidLabels.Fill => Fill ?? Brush.Transparent;
        Point IPyramidLabels.SlopePoint => _slopeCenter;
        DataLabelPlacement IPyramidLabels.Position { get => _position; set => _position = value; }

        #endregion

        #region Methods

        #region Internal methods

        /// <summary>
        /// Converts the data points to corresponding screen points for rendering the funnel segment.
        /// </summary>
        internal void SetData(double y, double height, double neckWidth, double neckHeight, object? xValue, double yValue)
        {
            _top = y;
            _bottom = y + height;
            _topRadius = y / 2;
            _bottomRadius = (y + height) / 2;
            _neckWidth = neckWidth;
            _neckHeight = neckHeight;
            _dataLabelXValue = xValue;
            _dataLabelYValue = yValue;
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
                double minWidth = 0.5f * (1 - (_neckWidth / boundsWidth));
                double bottomY = minWidth * (_bottom - _top) / (_bottomRadius - _topRadius);
                double topWidth = Math.Min(_topRadius, minWidth);
                double bottomWidth = Math.Min(_bottomRadius, minWidth);
                _isBroken = (_topRadius >= minWidth) ^ (_bottomRadius > minWidth);

                _values = new float[12];
                _values[0] = (float)(boundsLeft + (topWidth * boundsWidth));
                _values[1] = _values[3] = (float)(boundsTop + (_top * boundsHeight));
                _values[2] = (float)(boundsLeft + ((float)(1 - topWidth) * boundsWidth));
                _values[6] = (float)(boundsLeft + ((float)(1 - bottomWidth) * boundsWidth));
                _values[7] = _values[9] = (float)(boundsTop + (_bottom * boundsHeight));
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
            if (Chart != null && !Empty && Chart.DataLabelSettings.LabelStyle is ChartDataLabelStyle style)
            {
                var bounds = Chart.SeriesBounds;
                _labelXPosition = (float)bounds.X + SegmentBounds.Center.X;
                _labelYPosition = (float)bounds.Y + SegmentBounds.Center.Y;

                LabelContent = Chart.DataLabelSettings.GetLabelContent(_dataLabelXValue, _dataLabelYValue);
                double left;
                double right;
                
                if (_isBroken)
                {
                    right = (_values[5] + bounds.Y > _labelYPosition ? ((_values[4] + _values[2]) / 2) : ((_values[4] + _values[6]) / 2)) + bounds.Left;
                    left = (_values[11] + bounds.Y > _labelYPosition ? ((_values[10] + _values[0]) / 2) : ((_values[10] + _values[8]) / 2)) + bounds.Left;
                }
                else
                {
                    right = ((_values[2] + _values[6]) / 2) + bounds.Left;
                    left = ((_values[0] + _values[8]) / 2) + bounds.Left;
                }

                var y = ((_values[3] + _values[7]) / 2) + bounds.Top;

                _slopeCenter = new Point(right - 3, y);

                //Add the DataLabel properties.
                UpdateDataLabels();

                _dataLabelSize = Chart.LabelTemplate is null ? LabelContent.Measure(style) : CalculateTemplateSize();
                _actualSize = new Size(_dataLabelSize.Width + style.Margin.HorizontalThickness, _dataLabelSize.Height + style.Margin.VerticalThickness);
                _position = DataLabelPlacement.Inner;

                switch (Chart.DataLabelSettings.LabelPlacement)
                {
                    case DataLabelPlacement.Center:
                    case DataLabelPlacement.Auto:
                        if (_actualSize.Width > right - left || _actualSize.Height > SegmentBounds.Height)
						{
							_position = DataLabelPlacement.Outer;
						}

						break;
                    case DataLabelPlacement.Outer:
                        _position = DataLabelPlacement.Outer;
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
					if (funnelChart.LabelTemplateView.Cast<View>().FirstOrDefault(child => DataLabels[0] == child.BindingContext) is DataLabelItemView templateView && templateView.ContentView is View content)
					{
						if (!content.DesiredSize.IsZero)
						{
							return content.DesiredSize;
						}

#if NET9_0_OR_GREATER
                        var desiredSize = templateView.Measure(double.PositiveInfinity, double.PositiveInfinity);

                        if (desiredSize.IsZero)
						{
							return content.Measure(double.PositiveInfinity, double.PositiveInfinity);
						}
#else
						var desiredSize = templateView.Measure(double.PositiveInfinity, double.PositiveInfinity, MeasureFlags.IncludeMargins).Request;

						if (desiredSize.IsZero)
						{
							return content.Measure(double.PositiveInfinity, double.PositiveInfinity, MeasureFlags.IncludeMargins).Request;
						}
#endif

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
