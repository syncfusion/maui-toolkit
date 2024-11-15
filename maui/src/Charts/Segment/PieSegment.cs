namespace Syncfusion.Maui.Toolkit.Charts
{
    /// <summary>
    /// Represents a segment of the <see cref="PieSeries"/> chart.
    /// </summary>
    public class PieSegment : ChartSegment
    {
        #region Fields

        RectF _actualBounds, _currentBounds;

        RectF _innerRect;

        //Space between connector line edge and data label.
        const float _labelGap = 5;

        internal double connectorLength = 0.1;

        internal float connectorBendLineLength = 15;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the start angle of the pie segment.
        /// </summary>
        public double StartAngle { get; internal set; }

        /// <summary>
        /// Gets the end angle of the pie segment.
        /// </summary>
        public double EndAngle { get; internal set; }

        /// <summary>
        /// Gets the y-value of the pie segment.
        /// </summary>
        public double YValue { get; internal set; }

        /// <summary>
        /// Gets the radius of the pie segment.
        /// </summary>
        public float Radius { get; internal set; }

        internal bool Exploded { get; set; }

        internal double MidAngle { get; set; }

        internal double PreviousStartAngle { get; set; } = double.NaN;

        internal double PreviousEndAngle { get; set; } = double.NaN;

        internal Rect LabelRect { get; set; }

        internal bool IsLeft { get; set; } = false;

        internal RenderingPosition RenderingPosition { get; set; }

        internal string TrimmedText { get; set; } = string.Empty;

        internal double SegmentStartAngle { get; set; }

        internal double SegmentEndAngle { get; set; }

        internal double SegmentMidAngle { get; set; }

        internal double SegmentNewAngle { get; set; }

        internal Position DataLabelRenderingPosition { get; set; }

        internal double isLabelUpdated;

        #endregion

        #region Methods

        #region Protected Methods

        /// <inheritdoc/>
        protected internal override void OnLayout()
        {
            if (double.IsNaN(YValue) || Series is not PieSeries pieSeries) return;

            var areaBounds = pieSeries.AreaBounds;
            var minSize = Math.Min(areaBounds.Width, areaBounds.Height);
            float centerX = pieSeries.Center.X;
            float centerY = pieSeries.Center.Y;

            //For calculating pie center angle.
            MidAngle = pieSeries.isClockWise ? (StartAngle + (EndAngle / 2)) * 0.0174532925f : (StartAngle + EndAngle) / 2 * 0.0174532925f;
            Radius = pieSeries.ActualRadius;

            float minScale = (float)(minSize * pieSeries.Radius);
            float x = ((centerX * 2) - minScale) / 2;
            float y = ((centerY * 2) - minScale) / 2;

            _actualBounds = new RectF(x, y, minScale, minScale);
            _currentBounds = _actualBounds;

            if (Index == pieSeries.ExplodeIndex || pieSeries.ExplodeAll)
            {
                _actualBounds = _actualBounds.Offset((float)(pieSeries.ExplodeRadius * Math.Cos(MidAngle)), (float)(pieSeries.ExplodeRadius * Math.Sin(MidAngle)));
                _currentBounds = _actualBounds;
            }
        }

        /// <inheritdoc/>
        protected internal override void Draw(ICanvas canvas)
        {
            if (Series is not PieSeries pieSeries || double.IsNaN(YValue))
            {
                return;
            }

            var newWidth = _actualBounds.Width;
            var newHeight = _actualBounds.Height;
            float drawStartAngle = (float)StartAngle;
            float drawEndAngle = (float)EndAngle;

            if (pieSeries.CanAnimate())
            {
                float animationValue = pieSeries.AnimationValue;

                if (!double.IsNaN(PreviousStartAngle) && !double.IsNaN(PreviousEndAngle))
                {
                    drawStartAngle = GetDynamicAnimationAngleValue(animationValue, PreviousStartAngle, StartAngle);
                    drawEndAngle = GetDynamicAnimationAngleValue(animationValue, PreviousEndAngle, EndAngle);
                }
                else
                {
                    newWidth = _actualBounds.Width * animationValue;
                    newHeight = _actualBounds.Height * animationValue;
                    drawStartAngle = (float)(pieSeries.StartAngle + ((StartAngle - pieSeries.StartAngle) * animationValue));
                    drawEndAngle = (float)(EndAngle * animationValue);
                }
            }

            var offsetX = _actualBounds.Left + ((_actualBounds.Width - newWidth) / 2);
            var offsetY = _actualBounds.Top + ((_actualBounds.Height - newHeight) / 2);
            _currentBounds = new RectF(offsetX, offsetY, newWidth, newHeight);
            var endArcAngle = pieSeries.isClockWise ? drawStartAngle + drawEndAngle : drawEndAngle;

            //Drawing segment.
            var path = new PathF();
            path.MoveTo(_currentBounds.Left + (_currentBounds.Width / 2), _currentBounds.Top + (_currentBounds.Height / 2));
            path.AddArc(_currentBounds.Left, _currentBounds.Top, _currentBounds.Right, _currentBounds.Bottom, -drawStartAngle, -endArcAngle, pieSeries.isClockWise);
            path.Close();
            canvas.SetFillPaint(Fill, path.Bounds);
            canvas.Alpha = Opacity;
            canvas.FillPath(path);

            //Drawing stroke.
            if (HasStroke)
            {
                path.MoveTo(_currentBounds.Left + (_currentBounds.Width / 2), _currentBounds.Top + (_currentBounds.Height / 2));
                path.AddArc(_currentBounds.Left, _currentBounds.Top, _currentBounds.Right, _currentBounds.Bottom, -drawStartAngle, -endArcAngle, pieSeries.isClockWise);
                path.Close();
                canvas.StrokeColor = Stroke.ToColor();
                canvas.StrokeSize = (float)StrokeWidth;
                canvas.DrawPath(path);
            }
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Converts the data points to corresponding screen points for rendering the pie segment.
        /// </summary>
        internal virtual void SetData(double startAngle, double endAngle, double yValue)
        {
            if (Series is PieSeries pieSeries)
            {
                StartAngle = startAngle;
                EndAngle = pieSeries.isClockWise ? endAngle : startAngle + endAngle;
                YValue = yValue;
            }
        }

        internal void SetPreviousData(double[] values)
        {
            PreviousStartAngle = values[0];
            PreviousEndAngle = values[1];
        }

        internal float GetDynamicAnimationAngleValue(float animationValue, double oldAngle, double currentAngle)
        {
            return (float)((currentAngle > oldAngle) ?
                    oldAngle + ((currentAngle - oldAngle) * animationValue)
                    : oldAngle - ((oldAngle - currentAngle) * animationValue));
        }

        internal override int GetDataPointIndex(float x, float y)
        {
            if (Series is PieSeries pieSeries && IsPointInPieSegment(pieSeries.ActualRadius, x, y))
            {
                return pieSeries.Segments.IndexOf(this);
            }

            return -1;
        }

        internal override void OnDataLabelLayout()
        {
            if (Series is not PieSeries pieSeries || double.IsNaN(YValue) || pieSeries == null
                || pieSeries.DataLabelSettings == null) return;

            var dataLabelSettings = pieSeries.DataLabelSettings;
            IsEmpty = double.IsNaN(YValue) || YValue == 0;
            float segmentRadius = pieSeries.GetDataLabelRadius();
            segmentRadius = Index == pieSeries.ExplodeIndex ? segmentRadius + (float)pieSeries.ExplodeRadius : segmentRadius;
            PointF center = pieSeries.Center;
            DataLabelXPosition = center.X + (Math.Cos(MidAngle) * segmentRadius);
            DataLabelYPosition = center.Y + (Math.Sin(MidAngle) * segmentRadius);
            SegmentMidAngle = (SegmentStartAngle + SegmentEndAngle) / 2;
            SegmentNewAngle = SegmentMidAngle;
            LabelContent = pieSeries.GetLabelContent(YValue, pieSeries.SumOfYValues());
            PointF labelPosition = new PointF((float)DataLabelXPosition, (float)DataLabelYPosition);
            ChartDataLabelStyle labelStyle = dataLabelSettings.LabelStyle;
            UpdateDataLabels();
            DataLabelPosition();
            SizeF labelSize = pieSeries.LabelTemplate == null ? labelStyle.MeasureLabel(LabelContent) : pieSeries.GetLabelTemplateSize(this);

            LabelPositionPoint = GetDataLabelPosition(pieSeries, this, dataLabelSettings, labelSize, labelPosition, (float)labelStyle.LabelPadding);
        }

        void DataLabelPosition()
        {
            DataLabelRenderingPosition =
                ((SegmentMidAngle >= -90 & SegmentMidAngle! < 0) ||
                        (SegmentMidAngle! >= 0 && SegmentMidAngle! < 90) ||
                        (SegmentMidAngle! >= 270) || (SegmentMidAngle <= -270 & SegmentMidAngle! < 0))
                    ? Position.Right
                    : Position.Left;
        }

        internal override void UpdateDataLabels()
        {
            if (Series is CircularSeries series)
            {
                var dataLabelSettings = series.ChartDataLabelSettings;

                if (dataLabelSettings == null) return;

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
        }

        #endregion

        #region Private Methods

        bool IsPointInPieSegment(double radius, double x, double y)
        {
            if (Series is PieSeries pieSeries)
            {
                var dx = x - ((_currentBounds.Left + _currentBounds.Right) / 2);
                var dy = y - ((_currentBounds.Top + _currentBounds.Bottom) / 2);
                var pointLength = Math.Sqrt((dx * dx) + (dy * dy));
                var angle = GetAngle((float)x, (float)y);
                double startAngle = 360 + StartAngle;
                double endAngle = pieSeries.isClockWise ? Math.Abs(StartAngle) + Math.Abs(EndAngle) : 360 + EndAngle;

                if (pieSeries.StartAngle < pieSeries.EndAngle)
                {
                    if (pieSeries.StartAngle > 0 && endAngle > 360 && angle < pieSeries.StartAngle)
                    {
                        angle = angle + 360;
                    }
                }
                else
                {
                    if (EndAngle > 0 && endAngle < 360 && angle > StartAngle)
                    {
                        angle = angle + 360;
                    }
                }

                if (pointLength <= radius)
                {
                    if (pieSeries.StartAngle > pieSeries.EndAngle)
                    {
                        if (pieSeries.isClockWise && StartAngle > angle && angle > endAngle)
                        {
                            return true;
                        }
                        else if (!pieSeries.isClockWise && startAngle > angle && angle > endAngle)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (Math.Abs(StartAngle) < angle && angle < endAngle)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        double GetAngle(float x, float y)
        {
            if (Series is PieSeries pieSeries)
            {
                double dx = x - pieSeries.Center.X;
                double dy = -(y - pieSeries.Center.Y);

                var inRads = Math.Atan2(dy, dx);
                inRads = inRads < 0 ? Math.Abs(inRads) : (2 * Math.PI) - inRads;
                return ChartMath.RadianToDegree(inRads);
            }

            return double.NaN;
        }

        PointF GetDataLabelPosition(CircularSeries series, PieSegment dataLabel, CircularDataLabelSettings dataLabelSettings, SizeF labelSize, PointF labelPosition, float padding)
        {
            XPoints.Clear();
            YPoints.Clear();

            IsVisible = true;

            float x = labelPosition.X;
            float y = labelPosition.Y;
            float x1 = x;
            float y1 = y;
            ChartDataLabelStyle labelStyle = dataLabelSettings.LabelStyle;
            var borderWidth = (float)labelStyle.StrokeWidth / 2;

            x1 = x1 + ((float)labelStyle.OffsetX);
            y1 = y1 + ((float)labelStyle.OffsetY);

            if (dataLabelSettings.LabelPosition == ChartDataLabelPosition.Inside)
            {
                _innerRect = new RectF(x1, y1, labelSize.Width + borderWidth * 2 + padding, labelSize.Height + borderWidth * 2 + padding);
                PointF point1 = GetPositionForInsideSmartLabel(series, dataLabel, x1, y1, labelSize, borderWidth, padding);
                x1 = point1.X;
                y1 = point1.Y;
            }
            else
            {
                PointF point = GetPositionForOutsideSmartLabel(series, dataLabel, labelSize, borderWidth, padding);
                x1 = point.X;
                y1 = point.Y;
            }

            return new PointF(x1, y1);
        }

        PointF GetPositionForInsideSmartLabel(CircularSeries series, PieSegment? dataMarkerLabel, float actualX, float actualY, SizeF labelSize, float borderWidth, float padding)
        {
            if (dataMarkerLabel == null) return PointF.Zero;

            bool isIntersected = false;
            float x = actualX, y = actualY;
            PointF point2 = new PointF(x, y);
            _innerRect = new RectF(point2.X, point2.Y, _innerRect.Size.Width + borderWidth * 2 + padding, _innerRect.Size.Height + borderWidth * 2 + padding);

            if (series.DataLabelSettings.SmartLabelAlignment == SmartLabelAlignment.Shift)
            {
                dataMarkerLabel.LabelRect = _innerRect;
                dataMarkerLabel.LabelPositionPoint = new Point(_innerRect.X, _innerRect.Y);
                dataMarkerLabel.IsLeft = false;
                int currentPointIndex = dataMarkerLabel.Index;

                for (int i = 0; i < currentPointIndex; i++)
                {
                    var pieSegment = series.Segments[i] as PieSegment;
                    if (pieSegment == null) return PointF.Zero;

                    if (series.IsOverlap(dataMarkerLabel.LabelRect, pieSegment.LabelRect))
                    {
                        isIntersected = true;
                    }
                }

                if (dataMarkerLabel.Index == 0)
                    series.AdjacentLabelRect = Rect.Zero;

                isIntersected = !isIntersected ? series.AdjacentLabelRect.IntersectsWith(_innerRect) : isIntersected;
            }

            if (isIntersected)
            {
                float angle = (float)dataMarkerLabel.MidAngle;
                PointF center = series.GetCenter();
                float segmentRadius = series.GetRadius();
                dataMarkerLabel.DataLabelXPosition = center.X + (Math.Cos(angle) * segmentRadius);
                dataMarkerLabel.DataLabelYPosition = center.Y + (Math.Sin(angle) * segmentRadius);
                Point rect = GetPositionForOutsideSmartLabel(series, dataMarkerLabel, labelSize, borderWidth, padding);
                _innerRect.X = (float)rect.X;
                _innerRect.Y = (float)rect.Y;
                point2 = new PointF(_innerRect.X, _innerRect.Y);
            }
            else
            {
                double circularAngle = Math.Abs(dataMarkerLabel.MidAngle) % (Math.PI * 2);
                bool isLeft = circularAngle > 1.57 && circularAngle < 4.71;
                dataMarkerLabel.LabelRect = new Rect(_innerRect.X, _innerRect.Y, labelSize.Width, labelSize.Height);
                dataMarkerLabel.LabelPositionPoint = new Point(_innerRect.X, _innerRect.Y);
                dataMarkerLabel.IsLeft = isLeft;
                dataMarkerLabel.RenderingPosition = RenderingPosition.Inner;
            }

            _innerRect.X = point2.X;
            _innerRect.Y = point2.Y;
            series.InnerBounds.Add(_innerRect);
            series.AdjacentLabelRect = _innerRect;

            return new PointF(_innerRect.X, _innerRect.Y);
        }

        PointF GetPositionForOutsideSmartLabel(CircularSeries series, PieSegment dataMarkerLabel, SizeF labelSize, float borderWidth, float padding)
        {
            if (series is not PieSeries pieSeries || series.AreaBounds == Rect.Zero) return PointF.Zero;

            float x = (float)dataMarkerLabel.DataLabelXPosition;
            float y = (float)dataMarkerLabel.DataLabelYPosition;

            double radius = Index == (float)pieSeries.ExplodeIndex ? series.GetRadius() + (float)pieSeries.ExplodeRadius : series.GetRadius();
            PointF center = series.GetCenter();
            double dataLabelMidRadius = radius + (radius * connectorLength);
            PointF startPoint = series.calculateOffset(
                   SegmentMidAngle!, radius, center);
            PointF endPoint = series.calculateOffset(SegmentMidAngle!,
            (dataLabelMidRadius), center);

            dataMarkerLabel.XPoints = new List<float>();
            dataMarkerLabel.YPoints = new List<float>();

            dataMarkerLabel.XPoints.Add(startPoint.X);
            dataMarkerLabel.YPoints.Add(startPoint.Y);

            dataMarkerLabel.XPoints.Add(endPoint.X);
            dataMarkerLabel.YPoints.Add(endPoint.Y);

            dataMarkerLabel.XPoints.Add(x);
            dataMarkerLabel.YPoints.Add(y);

            Rect rect = new Rect(x, y, labelSize.Width, labelSize.Height);

            if (DataLabelRenderingPosition == Position.Right)
            {
                XPoints[2] = endPoint.X + connectorBendLineLength;
                YPoints[2] = endPoint.Y;
                rect = new Rect(XPoints[2] + _labelGap + labelSize.Width / 2, YPoints[2], labelSize.Width, labelSize.Height);
            }
            else
            {
                XPoints[2] = endPoint.X - connectorBendLineLength;
                YPoints[2] = endPoint.Y;
                rect = new Rect(XPoints[2] - labelSize.Width / 2 - _labelGap, YPoints[2], labelSize.Width, labelSize.Height);
            }

            dataMarkerLabel.LabelRect = rect;
            dataMarkerLabel.LabelPositionPoint = new PointF((float)rect.X, (float)rect.Y);
            dataMarkerLabel.IsLeft = DataLabelRenderingPosition == Position.Left;
            dataMarkerLabel.RenderingPosition = series.DataLabelSettings.SmartLabelAlignment == SmartLabelAlignment.Shift ? RenderingPosition.Outer : dataMarkerLabel.RenderingPosition;

            return new PointF((float)rect.X, (float)rect.Y);
        }

        #endregion

        #endregion
    }

    internal enum RenderingPosition
    {
        Inner,
        Outer
    }

}
