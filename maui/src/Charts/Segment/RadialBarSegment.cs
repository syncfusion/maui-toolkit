namespace Syncfusion.Maui.Toolkit.Charts
{
    /// <summary>
    /// Represents a segment of the <see cref="RadialBarSeries"/> chart.
    /// </summary>
    public class RadialBarSegment : ChartSegment
    {
        #region Fields

        RadialBarSeries? _currentSeries;
        float _angleDeviation, _actualRadius, _innerRadius;
        bool _isClockWise, _isTrack;
        bool _isCircularBar;
        RectF _actualBounds, _currentBounds;

        float _drawStartAngle, _drawEndAngle, _segmentStartAngle,
                      _segmentEndAngle, _trackStartAngle, _trackEndAngle;
        
        Point _endCurveStartPoint, _endCurveEndPoint, _trackEndCurveStartPoint, _trackEndCurveEndPoint;

        Point _trackInnerCurveStartPoint, _trackOuterCurveStartPoint, _trackStartCurveEndPoint,
                      _trackStartCurveStartPoint, _trackInnerCurveEndPoint, _trackOuterCurveEndPoint,
                      _trackInnerLinePoint, _trackOuterLinePoint;

        Point _innerCurveStartPoint, _outerCurveStartPoint, _startCurveEndPoint,
                     _startCurveStartPoint, _innerCurveEndPoint, _outerCurveEndPoint,
                     _segmentInnerLinePoint, _segmentOuterLinePoint;
        #endregion

        #region Internal Properties

        internal const float CurveDepth = 2;

        internal float InnerRingRadius { get; set; }

        internal float OuterRingRadius { get; set; }

        internal Brush? TrackStroke { get; set; }

        internal float TrackStrokeWidth { get; set; }

        internal Brush? TrackFill { get; set; }

        internal bool HasTrackStroke
        {
            get => TrackStrokeWidth > 0 && !ChartColor.IsEmpty(TrackStroke.ToColor());
        }

        internal double YValue { get; set; }

        internal int VisibleSegmentIndex { get; set; }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the start angle of the radial bar segment.
        /// </summary>
        public float StartAngle { get; internal set; }

        /// <summary>
        /// Gets the end angle of the radial bar segment.
        /// </summary>
        public float EndAngle { get; internal set; }

        #endregion

        #region Methods

        #region internal methods

        /// <summary>
        /// Converts the data points to corresponding screen points for rendering the radial bar segment.
        /// </summary>
        internal void SetData(float startAngle, float endAngle, float yValue)
        {
            StartAngle = startAngle;
            EndAngle = endAngle;
            YValue = yValue;
            _currentSeries = Series as RadialBarSeries;

            if (_currentSeries != null)
            {
                TrackStroke = _currentSeries.TrackStroke;
                TrackStrokeWidth = (float)_currentSeries.TrackStrokeWidth;
                TrackFill = _currentSeries.TrackFill;
            }
        }

        /// <inheritdoc />
        protected internal override void OnLayout()
        {
            if (_currentSeries == null)
            {
                return;
            }

            CalculateSegmentRadius();

            float angleDifference = (float)((float)(_currentSeries.GetAngleDifference()) - ((_currentSeries.EndAngle > _currentSeries.StartAngle) ?  0.01 : -0.01));
            _trackStartAngle = (float)_currentSeries.StartAngle;
            _trackEndAngle = (float)(_currentSeries.StartAngle + angleDifference);
            _isCircularBar = Math.Round(_currentSeries.EndAngle - _currentSeries.StartAngle - 0.0001, 1).Equals(360) ||
                Math.Round(_currentSeries.EndAngle - _currentSeries.StartAngle - 0.0001, 1).Equals(-360);
            _isTrack = true;
            CalculateSegmentAngle(_trackStartAngle, _trackEndAngle);

            _drawStartAngle = StartAngle;
            _drawEndAngle = EndAngle + StartAngle - 0.01f;
            _isCircularBar = Math.Round(_drawEndAngle - _drawStartAngle - 0.0001, 1).Equals(360) || 
                Math.Round(_drawEndAngle - _drawStartAngle - 0.0001, 1).Equals(-360);
            CalculateSegmentAngle(_drawStartAngle, _drawEndAngle);

        }

        /// <inheritdoc />
        protected internal override void Draw(ICanvas canvas)
        {
            if (_currentSeries == null)
            {
                return;
            }

            if (_currentSeries.CanAnimate())
            {
                float animationValue = _currentSeries.AnimationValue;
                _drawStartAngle = (float)(_currentSeries.StartAngle + ((StartAngle - _currentSeries.StartAngle) * animationValue));
                _drawEndAngle = StartAngle + (EndAngle * animationValue);
                _isTrack = false;
                CalculateSegmentAngle(_drawStartAngle, _drawEndAngle);
            }

            DrawTrack(canvas);
            DrawSegment(canvas);
        }

        /// <inheritdoc />
        internal override int GetDataPointIndex(float x, float y)
        {
            if (Series != null && IsPointInRadialBarSegment(x, y))
            {
                return Series.Segments.IndexOf(this);
            }

            return -1;
        }

        #endregion

        #region Private methods

        void DrawSegment(ICanvas canvas)
        {
            if (_currentSeries == null || _currentSeries.YValues[Index] == 0) return;
            
            _isCircularBar = Math.Round(_drawEndAngle - _drawStartAngle - 0.0001, 1).Equals(360) ||
                Math.Round(_drawEndAngle - _drawStartAngle - 0.0001, 1).Equals(-360);
            var segmentPath = new PathF();

            // To draw the start curve for the segment
            if ((_currentSeries.CapStyle == CapStyle.BothCurve || _currentSeries.CapStyle == CapStyle.StartCurve) && !_isCircularBar )
            {
                segmentPath.MoveTo(_innerCurveStartPoint);
                segmentPath.CurveTo(_startCurveStartPoint, _startCurveEndPoint, _outerCurveStartPoint);
            }
            // To draw the outer arc for the segment
            segmentPath.AddArc(_actualBounds.Left, _actualBounds.Top, _actualBounds.Right, _actualBounds.Bottom, -_segmentStartAngle, -_segmentEndAngle, _isClockWise);
            // To draw the inner arc for the segment
            segmentPath.AddArc(_currentBounds.Left, _currentBounds.Top, _currentBounds.Right, _currentBounds.Bottom, -_segmentEndAngle, -_segmentStartAngle, !_isClockWise);
            segmentPath.Close();

            // To draw the end curve for the segment
            if ((_currentSeries.CapStyle == CapStyle.EndCurve || _currentSeries.CapStyle == CapStyle.BothCurve) && !_isCircularBar )
            {
                segmentPath.MoveTo(_innerCurveEndPoint);
                segmentPath.CurveTo(_endCurveStartPoint, _endCurveEndPoint, _outerCurveEndPoint);
            }

            canvas.SetFillPaint(Fill, segmentPath.Bounds);
            canvas.Alpha = Opacity;
            canvas.FillPath(segmentPath);
            DrawSegmentStroke(canvas);
        }

        void DrawTrack(ICanvas canvas)
        {
            if (_currentSeries == null) return;

            _isCircularBar = Math.Round(_currentSeries.EndAngle - _currentSeries.StartAngle - 0.0001, 1).Equals(360) ||
                Math.Round(_currentSeries.EndAngle - _currentSeries.StartAngle - 0.0001, 1).Equals(-360);
            var disableTrack = (Math.Abs(_trackEndAngle - _trackStartAngle) <= 15);
            _isClockWise = _trackEndAngle >= _trackStartAngle;

            if (!disableTrack)
            {
                var track = new PathF();
                // To draw the start curve for the track
                if ((_currentSeries.CapStyle == CapStyle.BothCurve || _currentSeries.CapStyle == CapStyle.StartCurve) && !_isCircularBar)
                {
                    track.MoveTo(_trackInnerCurveStartPoint);
                    track.CurveTo(_trackStartCurveStartPoint, _trackStartCurveEndPoint, _trackOuterCurveStartPoint);
                }
                // To draw the inner arc for the track
                track.AddArc(_currentBounds.Left, _currentBounds.Top, _currentBounds.Right, _currentBounds.Bottom, -_trackStartAngle, -_trackEndAngle, _isClockWise);
                // To draw the outer arc for the track
                track.AddArc(_actualBounds.Left, _actualBounds.Top, _actualBounds.Right, _actualBounds.Bottom, -_trackEndAngle, -_trackStartAngle, !_isClockWise);
                track.Close();

                // To draw the end curve for the track
                if ((_currentSeries.CapStyle == CapStyle.EndCurve || _currentSeries.CapStyle == CapStyle.BothCurve) && !_isCircularBar )
                {
                    track.MoveTo(_trackInnerCurveEndPoint);
                    track.CurveTo(_trackEndCurveStartPoint, _trackEndCurveEndPoint, _trackOuterCurveEndPoint);
                }

                canvas.SetFillPaint(_currentSeries.TrackFill, track.Bounds);
                canvas.Alpha = Opacity;
                canvas.FillPath(track);
                DrawTrackStroke(canvas);
            }
        }

        void DrawTrackStroke(ICanvas canvas)
        {
            if (HasTrackStroke && _currentSeries != null)
            {
                if (_isCircularBar) 
                {
                    // To draw the outer arc for the track stroke
                    var outerTrack = new PathF();
                    outerTrack.AddArc(_actualBounds.Left, _actualBounds.Top, _actualBounds.Right, _actualBounds.Bottom, -_trackStartAngle, -_trackEndAngle, _isClockWise);
                    // To draw the inner arc for the track stroke
                    var innerTrack = new PathF();
                    innerTrack.AddArc(_currentBounds.Left, _currentBounds.Top, _currentBounds.Right, _currentBounds.Bottom, -_trackEndAngle, -_trackStartAngle, !_isClockWise);
                    canvas.StrokeColor = TrackStroke.ToColor();
                    canvas.StrokeSize = TrackStrokeWidth;
                    canvas.DrawPath(outerTrack);
                    canvas.DrawPath(innerTrack);
                }
                else
                {
                    var trackStrokePath = new PathF();
                    // To draw the start curve for the track stroke
                    if ((_currentSeries.CapStyle == CapStyle.BothCurve || _currentSeries.CapStyle == CapStyle.StartCurve) && !_isCircularBar)
                    {
                        trackStrokePath.MoveTo(_trackInnerCurveStartPoint);
                        trackStrokePath.CurveTo(_trackStartCurveStartPoint, _trackStartCurveEndPoint, _trackOuterCurveStartPoint);
                    }
                    // To draw the outer arc for the track stroke
                    trackStrokePath.AddArc(_actualBounds.Left, _actualBounds.Top, _actualBounds.Right, _actualBounds.Bottom, -_trackStartAngle, -_trackEndAngle, _isClockWise);
                    
                    // To draw the end curve for the track stroke
                    if ((_currentSeries.CapStyle == CapStyle.EndCurve || _currentSeries.CapStyle == CapStyle.BothCurve) && !_isCircularBar)
                    {
                        trackStrokePath.MoveTo(_trackInnerCurveEndPoint);
                        trackStrokePath.CurveTo(_trackEndCurveStartPoint, _trackEndCurveEndPoint, _trackOuterCurveEndPoint);
                        trackStrokePath.MoveTo(_trackInnerCurveEndPoint);
                    }
                    // To draw the inner arc for the track stroke
                    trackStrokePath.AddArc(_currentBounds.Left, _currentBounds.Top, _currentBounds.Right, _currentBounds.Bottom, -_trackEndAngle, -_trackStartAngle, !_isClockWise);
                    
                    canvas.StrokeColor = TrackStroke.ToColor();
                    canvas.StrokeSize = TrackStrokeWidth;
                    canvas.DrawPath(trackStrokePath);

                    if (_currentSeries.CapStyle == CapStyle.BothFlat || _currentSeries.CapStyle == CapStyle.EndCurve) 
                    {
                        // To draw line for the track stroke
                        canvas.DrawLine(_trackInnerLinePoint, _trackOuterLinePoint);
                        canvas.StrokeColor = TrackStroke.ToColor();
                        canvas.StrokeSize = TrackStrokeWidth;
                    }
                }
            }
        }

        void DrawSegmentStroke(ICanvas canvas)
        {
            if (HasStroke && _currentSeries != null)
            {
                var strokePath = new PathF();
                // To draw the start curve for the segment stroke
                if ((_currentSeries.CapStyle == CapStyle.BothCurve || _currentSeries.CapStyle == CapStyle.StartCurve) && !_isCircularBar)
                {
                    strokePath.MoveTo(_innerCurveStartPoint);
                    strokePath.CurveTo(_startCurveStartPoint, _startCurveEndPoint, _outerCurveStartPoint);
                }
                // To draw the outer arc for the segment stroke
                strokePath.AddArc(_actualBounds.Left, _actualBounds.Top, _actualBounds.Right, _actualBounds.Bottom, -_segmentStartAngle, -_segmentEndAngle, _isClockWise);
                
                // To draw the end curve for the segment stroke
                if ((_currentSeries.CapStyle == CapStyle.EndCurve || _currentSeries.CapStyle == CapStyle.BothCurve) && !_isCircularBar)
                {
                    strokePath.MoveTo(_innerCurveEndPoint);
                    strokePath.CurveTo(_endCurveStartPoint, _endCurveEndPoint, _outerCurveEndPoint);
                    strokePath.MoveTo(_innerCurveEndPoint);
                }
                // To draw the inner arc for the segment stroke
                strokePath.AddArc(_currentBounds.Left, _currentBounds.Top, _currentBounds.Right, _currentBounds.Bottom, -_segmentEndAngle, -_segmentStartAngle, !_isClockWise);
                canvas.StrokeColor = Stroke.ToColor();
                canvas.StrokeSize = (float)StrokeWidth;
                canvas.DrawPath(strokePath);

                if (_currentSeries.CapStyle == CapStyle.BothFlat || _currentSeries.CapStyle == CapStyle.EndCurve)
                {
                    // To draw line for the segment stroke
                    canvas.DrawLine(_segmentInnerLinePoint, _segmentOuterLinePoint);
                    canvas.StrokeColor = Stroke.ToColor();
                    canvas.StrokeSize = (float)StrokeWidth;
                }
            }
        }

        void CapStyleCalculation(float midRadius, float segmentRadius, float startCurveAngle, float drawStartAngle, float drawEndAngle)
        {
            float calculatedRadius = (InnerRingRadius + OuterRingRadius) / 2;
            _angleDeviation = ChartUtils.CalculateAngleDeviation(calculatedRadius, segmentRadius / 2, 360) * (_isClockWise ? 1 : -1);
            if (_currentSeries != null && _currentSeries.CapStyle != CapStyle.BothFlat)
            {
                if (!_isCircularBar)
                {
                    UpdateSegmentAngleForCurvePosition(segmentRadius, drawStartAngle, drawEndAngle);

                    if (_currentSeries.CapStyle == CapStyle.StartCurve || _currentSeries.CapStyle == CapStyle.BothCurve)
                    {
                        if (_isTrack)
                        {
                            TrackStartCurveCalculation(midRadius, segmentRadius, startCurveAngle);
                        }
                        else
                        {
                            SegmentStartCurveCalculation(midRadius, segmentRadius, startCurveAngle);
                        }
                    }

                    if (_currentSeries.CapStyle == CapStyle.EndCurve || _currentSeries.CapStyle == CapStyle.BothCurve)
                    {
                        if (_isTrack)
                        {
                            TrackEndCurveCalculation(_segmentEndAngle, midRadius, segmentRadius);
                        }
                        else
                        {
                            SegmentEndCurveCalculation(midRadius, segmentRadius);
                        }
                    }
                }
            }
            else if (_currentSeries != null)
            {
                _segmentEndAngle += Math.Abs(EndAngle) < Math.Abs(_angleDeviation) ? (2 * _angleDeviation) : 0;
                var startAngle = _segmentStartAngle;
                var endAngle = _segmentEndAngle;

                if (_currentSeries.StartAngle <= _currentSeries.EndAngle) 
                {
                    _segmentStartAngle = endAngle < startAngle ? endAngle : startAngle;
                    _segmentEndAngle = endAngle < startAngle ? startAngle : endAngle;
                }
                else
                {
                    _segmentStartAngle = endAngle < startAngle ? startAngle : endAngle;
                    _segmentEndAngle = endAngle < startAngle ? endAngle : startAngle;
                }
            }

            _isTrack = false;
            _isCircularBar = false;
        }

        void SegmentEndCurveCalculation(float midRadius, float segmentRadius)
        {
            if (_currentSeries == null) return;

            float curvePoint = (float)(_drawEndAngle + (_angleDeviation / 1.65));
            float endingPoint = _segmentEndAngle - (2 * _angleDeviation);
            if (_currentSeries.StartAngle < _currentSeries.EndAngle)
            {
                if (curvePoint > endingPoint)
                {
                    if (_segmentStartAngle > endingPoint)
                    {
                        endingPoint = _segmentStartAngle;
                        curvePoint = _segmentStartAngle + _angleDeviation + (2 * _angleDeviation);
                    }
                }

                _segmentEndAngle = _segmentEndAngle - (2 * _angleDeviation);
                _segmentEndAngle = ((endingPoint >= _segmentEndAngle) && _segmentEndAngle < _segmentStartAngle) ? _segmentStartAngle : _segmentEndAngle;
            }
            else
            {
                if (endingPoint > curvePoint)
                {
                    if (_segmentStartAngle < endingPoint)
                    {
                        endingPoint = _segmentStartAngle;
                        curvePoint = _segmentStartAngle + _angleDeviation + (2 * _angleDeviation);
                    }
                }

                _segmentEndAngle = _segmentEndAngle - (2 * _angleDeviation);
                _segmentEndAngle = (_segmentStartAngle < _segmentEndAngle) ? _segmentStartAngle : _segmentEndAngle;
            }

            Point endPoint = ChartUtils.AngleToVector(endingPoint);
            _innerCurveEndPoint = new Point(_currentBounds.Center.X + (InnerRingRadius * endPoint.X),
                                 _currentBounds.Center.Y + (InnerRingRadius * endPoint.Y));
            _outerCurveEndPoint = new Point(_currentBounds.Center.X + (OuterRingRadius * endPoint.X),
                                 _currentBounds.Center.Y + (OuterRingRadius * endPoint.Y));
            Point previousAngle = ChartUtils.AngleToVector(curvePoint);
            // We have used default value for curve depth
            _endCurveStartPoint = new Point(_currentBounds.Center.X + ((midRadius - segmentRadius / 1.75) * previousAngle.X),
                                       _currentBounds.Center.Y + ((midRadius - segmentRadius / 1.75) * previousAngle.Y));
            _endCurveEndPoint = new Point(_currentBounds.Center.X + ((midRadius + segmentRadius / 1.5) * previousAngle.X),
                                     _currentBounds.Center.Y + ((midRadius + segmentRadius / 1.5) * previousAngle.Y));
        }

        void SegmentStartCurveCalculation(float midRadius, float segmentRadius, float startCurveAngle)
        {
            if(_currentSeries == null) return;

            float curvePoint = (float)(_drawStartAngle - (_angleDeviation / 1.75));
            float endingPoint = startCurveAngle + (2 * _angleDeviation);

            if (_currentSeries.StartAngle < _currentSeries.EndAngle)
            {
                if (endingPoint > curvePoint)
                {
                    if (curvePoint > _segmentStartAngle)
                    {
                        curvePoint = _segmentStartAngle;
                        endingPoint = _segmentStartAngle;
                    }

                    _segmentStartAngle = endingPoint;
                    _segmentEndAngle = (_segmentStartAngle > _segmentEndAngle) ? _segmentStartAngle : _segmentEndAngle;
                }
            }
            else
            {
                if (curvePoint > endingPoint)
                {
                    _segmentStartAngle = endingPoint;
                    _segmentEndAngle = (_segmentStartAngle < _segmentEndAngle) ? _segmentStartAngle : _segmentEndAngle;
                }
            }

            // We have used default value for curve depth
            Point vectorPoint = ChartUtils.AngleToVector(curvePoint);
            _startCurveStartPoint = new Point(_currentBounds.Center.X + ((midRadius - segmentRadius / 1.75) * vectorPoint.X),
                                       _currentBounds.Center.Y + ((midRadius - segmentRadius / 1.75) * vectorPoint.Y));
            _startCurveEndPoint = new Point(_currentBounds.Center.X + ((midRadius + segmentRadius / 1.2) * vectorPoint.X),
                                 _currentBounds.Center.Y + ((midRadius + segmentRadius / 1.2) * vectorPoint.Y));
            Point centerPoint = ChartUtils.AngleToVector(endingPoint);
            _innerCurveStartPoint = new Point(_currentBounds.Center.X + (InnerRingRadius * centerPoint.X),
                                   _currentBounds.Center.Y + (InnerRingRadius * centerPoint.Y));
            _outerCurveStartPoint = new Point(_currentBounds.Center.X + ((midRadius + segmentRadius) * centerPoint.X),
                                   _currentBounds.Center.Y + ((midRadius + segmentRadius) * centerPoint.Y));
        }

        void TrackStartCurveCalculation(float midRadius, float segmentRadius, float startCurveAngle)
        {
            if (_currentSeries == null) return;
            // We have used default value for curve depth
            Point vectorPoint = ChartUtils.AngleToVector((float)(_trackStartAngle - _angleDeviation / 1.75));
            _trackStartCurveStartPoint = new Point(_currentBounds.Center.X + ((midRadius - segmentRadius / 1.75) * vectorPoint.X),
                                       _currentBounds.Center.Y + ((midRadius - segmentRadius / 1.75) * vectorPoint.Y));
            _trackStartCurveEndPoint = new Point(_currentBounds.Center.X + ((midRadius + segmentRadius / 1.2) * vectorPoint.X),
                                      _currentBounds.Center.Y + ((midRadius + segmentRadius / 1.2) * vectorPoint.Y));
            
            startCurveAngle += 2 * _angleDeviation;

            Point centerPoint = ChartUtils.AngleToVector(startCurveAngle);
            _trackInnerCurveStartPoint = new Point(_currentBounds.Center.X + (InnerRingRadius * centerPoint.X),
                                        _currentBounds.Center.Y + (InnerRingRadius * centerPoint.Y));
            _trackOuterCurveStartPoint = new Point(_currentBounds.Center.X + ((midRadius + segmentRadius) * centerPoint.X),
                                        _currentBounds.Center.Y + ((midRadius + segmentRadius) * centerPoint.Y));
            
            _trackStartAngle = _currentSeries.CapStyle == CapStyle.StartCurve ? startCurveAngle : _trackStartAngle;
        }

        void TrackEndCurveCalculation(float outerSegmentEndAngle, float midRadius, float segmentRadius)
        {
            if (_currentSeries == null) return;
            
            Point endPoint = ChartUtils.AngleToVector(outerSegmentEndAngle - (2 * _angleDeviation));
            _trackInnerCurveEndPoint = new Point(_currentBounds.Center.X + (InnerRingRadius * endPoint.X),
                                      _currentBounds.Center.Y + (InnerRingRadius * endPoint.Y));
            _trackOuterCurveEndPoint = new Point(_currentBounds.Center.X + (OuterRingRadius * endPoint.X),
                                      _currentBounds.Center.Y + (OuterRingRadius * endPoint.Y));
            // We have used default value for curve depth
            Point previousAngle = ChartUtils.AngleToVector((float)(_trackEndAngle + (_angleDeviation / 1.65)));
            _trackEndCurveStartPoint = new(_currentBounds.Center.X + ((midRadius - segmentRadius / 1.75) * previousAngle.X),
                                       _currentBounds.Center.Y + ((midRadius - segmentRadius / 1.75) * previousAngle.Y));
            _trackEndCurveEndPoint = new Point(_currentBounds.Center.X + ((midRadius + segmentRadius / 1.5) * previousAngle.X),
                                     _currentBounds.Center.Y + ((midRadius + segmentRadius / 1.5) * previousAngle.Y));
            _trackEndAngle = outerSegmentEndAngle - (2 * _angleDeviation);
            _trackStartAngle = _currentSeries.CapStyle == CapStyle.BothCurve ? _trackStartAngle + (2 * _angleDeviation) : _trackStartAngle;

            _isTrack = false;
        }

        void UpdateSegmentAngleForCurvePosition(float segmentRadius, float drawStartAngle, float drawEndAngle)
        {
            if (_currentSeries == null || segmentRadius == 0) return;

            if (_currentSeries.CapStyle != CapStyle.EndCurve)
            {
                _segmentStartAngle = _isClockWise ? drawStartAngle +
                    segmentRadius * CurveDepth / InnerRingRadius :
                    drawStartAngle - segmentRadius * CurveDepth / InnerRingRadius;
            }

            if (_currentSeries.CapStyle != CapStyle.StartCurve)
            {
                _segmentEndAngle = !_isClockWise ? drawEndAngle +
                    segmentRadius * CurveDepth / OuterRingRadius :
                    drawEndAngle - segmentRadius * CurveDepth / OuterRingRadius;
            }
        }

        bool IsPointInRadialBarSegment(double x, double y)
        {
            var radialBarSeries = Series as RadialBarSeries;
            if (radialBarSeries != null)
            {
                double dx, dy;
                dx = x - ((_currentBounds.Left + _currentBounds.Right) / 2);
                dy = y - ((_currentBounds.Top + _currentBounds.Bottom) / 2);

                double angle = ChartMath.RadianToDegree(Math.Atan2(dy, dx));
                double distanceSquare = (dx * dx) + (dy * dy);
                double segmentEndAngle = Math.Abs(EndAngle) + Math.Abs(StartAngle);
                if (angle < 0)
                {
                    angle = angle + 360;
                }

                if (radialBarSeries.StartAngle < radialBarSeries.EndAngle)
                {

                    if (radialBarSeries.StartAngle < 0 && segmentEndAngle < 360 && angle < radialBarSeries.StartAngle)
                    {
                        angle = angle + 360;
                    }
                }
                else
                {

                    if (EndAngle > 0 && segmentEndAngle < 360 && angle > StartAngle)
                    {
                        angle = angle + 360;
                    }
                }

                if (distanceSquare >= InnerRingRadius * InnerRingRadius && distanceSquare <= OuterRingRadius * OuterRingRadius)
                {
                    if (radialBarSeries.StartAngle > radialBarSeries.EndAngle)
                    {
                        if (StartAngle > angle && angle > _drawEndAngle)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if ((StartAngle < angle) && (angle < _drawEndAngle))
                        {
                            return true;
                        }
                        else if (StartAngle < 0 && _drawEndAngle < angle)
                        {
                            angle = angle - 360;
                            if ((StartAngle < angle) && (angle < _drawEndAngle))
                            {
                                return true;
                            }

                        }
                    }
                }
            }

            return false;
        }

        void CalculateSegmentAngle(float drawStartAngle, float drawEndAngle)
        {
            if (_currentSeries == null || (_currentSeries.YValues[Index] == 0 && !_isTrack)) return;
            
            _isClockWise = _currentSeries.EndAngle > _currentSeries.StartAngle;
            float segmentRadius = (OuterRingRadius - InnerRingRadius) / 2;
            float midRadius = OuterRingRadius - segmentRadius;
            _segmentEndAngle = drawEndAngle;
            _segmentStartAngle = drawStartAngle;

            if (_isClockWise)
            {
                _segmentEndAngle = _segmentStartAngle > _segmentEndAngle ? _segmentStartAngle : _segmentEndAngle;
            }

            float startCurveAngle = drawStartAngle;

            if (_isTrack)
            {
                if (_currentSeries.CapStyle == CapStyle.BothFlat || _currentSeries.CapStyle == CapStyle.EndCurve)
                {
                    Point startPoint = ChartUtils.AngleToVector(_trackStartAngle);
                    _trackInnerLinePoint = new Point(_currentBounds.Center.X +
                        (InnerRingRadius * startPoint.X), _currentBounds.Center.Y +
                        (InnerRingRadius * startPoint.Y));
                    _trackOuterLinePoint = new Point(_currentBounds.Center.X +
                        ((midRadius + segmentRadius) * startPoint.X), _currentBounds.Center.Y +
                        ((midRadius + segmentRadius) * startPoint.Y));
                }
            }
            else if (_currentSeries.CapStyle == CapStyle.BothFlat || _currentSeries.CapStyle == CapStyle.EndCurve) 
            {
                Point startPoint = ChartUtils.AngleToVector(drawStartAngle);
                _segmentInnerLinePoint = new Point(_currentBounds.Center.X +
                    (InnerRingRadius * startPoint.X), _currentBounds.Center.Y +
                    (InnerRingRadius * startPoint.Y));
                _segmentOuterLinePoint = new Point(_currentBounds.Center.X +
                    ((midRadius + segmentRadius) * startPoint.X), _currentBounds.Center.Y +
                    ((midRadius + segmentRadius) * startPoint.Y));
            }

            CapStyleCalculation(midRadius, segmentRadius, startCurveAngle, drawStartAngle, drawEndAngle);
        }

        void CalculateSegmentRadius()
        {
            if (_currentSeries == null || double.IsNaN(YValue))
            {
                return;
            }

            var center = _currentSeries.Center;
            RectF seriesClipRectangle = _currentSeries.AreaBounds;
            double minScale = Math.Min(seriesClipRectangle.Width, seriesClipRectangle.Height) - (Math.Min(seriesClipRectangle.Width, seriesClipRectangle.Height) * 0.2);
            minScale = minScale * _currentSeries.Radius;
            _actualBounds = new RectF((float)((center.X * 2) - minScale) / 2, (float)((center.Y * 2) - minScale) / 2, (float)minScale, (float)minScale);
            _currentBounds = new RectF(_actualBounds.Left, _actualBounds.Top, _actualBounds.Width, _actualBounds.Height);
            _innerRadius = (float)(Math.Min(_actualBounds.Height, _actualBounds.Width) / 2 * _currentSeries.InnerRadius);
            _actualRadius = _actualBounds.Width / 2 - _innerRadius;
            float centerRadius = (float)(Math.Min(_actualBounds.Height, _actualBounds.Width) / 2 * 0.2);
            double radius = (_actualRadius / _currentSeries.visibleSegmentCount) * (1 - _currentSeries.GapRatio);
            InnerRingRadius = _innerRadius + (_actualRadius / _currentSeries.visibleSegmentCount) * VisibleSegmentIndex + centerRadius;
            OuterRingRadius = (float)(InnerRingRadius + radius);
            _currentBounds = new RectF(_currentBounds.X + (_currentBounds.Width / 2) -
                InnerRingRadius, _currentBounds.Y + (_currentBounds.Height / 2) - InnerRingRadius,
                2 * InnerRingRadius, 2 * InnerRingRadius);
            _actualBounds = new RectF(_actualBounds.X + (_actualBounds.Width / 2) - OuterRingRadius,
                _actualBounds.Y + (_actualBounds.Height / 2) - OuterRingRadius, 2 * OuterRingRadius,
                2 * OuterRingRadius);
        }
        #endregion

        #endregion
    }
}
