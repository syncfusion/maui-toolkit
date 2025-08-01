namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// Represents a segment of the <see cref="DoughnutSeries"/> chart, extending the functionality of a pie chart segment.
	/// This segment is used specifically for doughnut charts, which differ from pie charts by having a central hole.
	/// </summary>
	public partial class DoughnutSegment : PieSegment
	{
		#region Fields
		
		RectF _actualBounds, _currentBounds;
		double _startAngle, _endAngle;
		double _segmentRadius;
        PointF _innerCurveStartPoint, _innerCurveStartMidpoint, _startCurveCeterPoint, _outerCurveStartMidPoint, _outerCurveStartPoint,
               _innerCurveEndPoint, _innerCurveEndMidpoint, _outerCurveEndMidPoint, _outerCurveEndPoint, _endCurveCenterPoint;
        float _explodeOffsetX, _explodeOffsetY;
        bool _isExploded;
		
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
			UpdatePosition(series);
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
					drawStartAngle = PieSegment.GetDynamicAnimationAngleValue(animationValue, PreviousStartAngle, StartAngle);
					drawEndAngle = PieSegment.GetDynamicAnimationAngleValue(animationValue, PreviousEndAngle, EndAngle);
				}
				else
				{
					drawStartAngle = (float)(series.StartAngle + ((StartAngle - series.StartAngle) * animationValue));
					drawEndAngle = (float)EndAngle * animationValue;
				}
			}

			UpdatePosition(series);
			var endArcAngle = series._isClockWise ? drawStartAngle + drawEndAngle : drawEndAngle;
			DrawSegment(canvas, series, drawStartAngle, endArcAngle);
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
				_endAngle = doughnutSeries._isClockWise ? mEndAngle : mStartAngle + mEndAngle;
				StartAngle = mStartAngle;
				EndAngle = doughnutSeries._isClockWise ? mEndAngle : mStartAngle + mEndAngle;
				YValue = mYValue;
			}
		}

		internal override int GetDataPointIndex(float x, float y)
		{
			if (Series != null && IsPointInDoughnutSegment(_segmentRadius, x, y))
			{
				return Series._segments.IndexOf(this);
			}

			return -1;
		}

		#endregion

		#region private Methods

		void UpdatePosition(DoughnutSeries series)
		{
			var center = series.Center;
			RectF seriesClipRect = series.AreaBounds;
			double minScale = Math.Min(seriesClipRect.Width, seriesClipRect.Height) * series.Radius;

  			float halfMinScale = (float)minScale / 2;
    		float centerOffsetX = (float)(center.X - halfMinScale);
    		float centerOffsetY = (float)(center.Y - halfMinScale);

    		_actualBounds = new RectF(centerOffsetX, centerOffsetY, halfMinScale * 2, halfMinScale * 2);
			_currentBounds = new RectF(_actualBounds.Location, _actualBounds.Size);

			if (series.ExplodeIndex == Index || series.ExplodeAll)
			{
				if (!_isExploded)
				{
					_explodeOffsetX = 0;
					_explodeOffsetY = 0;
				}

				float angle = series._isClockWise ? (float)((2f * (series.StartAngle + ((StartAngle - series.StartAngle) * series.AnimationValue)) * 0.0174532925f) + ((EndAngle * series.AnimationValue) * 0.0174532925f)) / 2f
			   : (float)(((series.StartAngle + ((StartAngle - series.StartAngle) * series.AnimationValue)) * 0.0174532925f) + (EndAngle * series.AnimationValue * 0.0174532925f)) / 2;
				_explodeOffsetX = (float)(series.ExplodeRadius * Math.Cos(angle));
				_explodeOffsetY = (float)(series.ExplodeRadius * Math.Sin(angle));
				_actualBounds = _actualBounds.Offset(_explodeOffsetX, _explodeOffsetY);
				_currentBounds = _actualBounds;

				_isExploded = true;
			}
			else if (_isExploded)
			{
				_explodeOffsetX = 0;
				_explodeOffsetY = 0;
				_isExploded = false;
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
					angle += 360;
				}

				double distanceSquare = (dx * dx) + (dy * dy);
				double segmentStartAngle = 360 + StartAngle;
				double segmentEndAngle = doughnutSeries._isClockWise ? Math.Abs(_startAngle) + Math.Abs(_endAngle) : 360 + EndAngle;

				if (doughnutSeries.StartAngle < doughnutSeries.EndAngle)
				{
					if (doughnutSeries.StartAngle > 0 && segmentEndAngle > 360 && angle < doughnutSeries.StartAngle)
					{
						angle += 360;
					}
				}
				else
				{
					if (_endAngle > 0 && segmentEndAngle < 360 && angle > _startAngle)
					{
						angle += 360;
					}
				}

				var innerRadius = InnerRadius;
				var outerRadius = radius;

				if (distanceSquare >= innerRadius * innerRadius && distanceSquare <= outerRadius * outerRadius)
				{
					if (doughnutSeries.StartAngle > doughnutSeries.EndAngle)
					{
						if (doughnutSeries._isClockWise && _startAngle > angle && angle > segmentEndAngle)
						{
							return true;
						}
						else if (!doughnutSeries._isClockWise && segmentStartAngle > angle && angle > segmentEndAngle)
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

		void DrawSegment(ICanvas canvas, DoughnutSeries series, float drawStartAngle, float drawEndAngle)
		{
			PathF path = new PathF();
			float outerRadius = (float)_segmentRadius;
			float innerRadius = InnerRadius;
			float midRadius = (float)(outerRadius + innerRadius) / 2;
			float radius = (float)(outerRadius - innerRadius) / 2;
			float deviationAngle = ChartUtils.CalculateAngleDeviation(midRadius, radius, 360);

			if (series.CapStyle == CapStyle.BothFlat || series.VisibleSegmentCount == 1)
			{
				DrawFlatSegment(canvas, series, drawStartAngle, drawEndAngle, path);
			}
			else
			{
				float segmentStartAngle = drawStartAngle;
				float segmentEndAngle = drawEndAngle;

				if (series.CapStyle == CapStyle.StartCurve || series.CapStyle == CapStyle.BothCurve)
				{
					if ((drawStartAngle + deviationAngle) >= drawEndAngle && series.CapStyle == CapStyle.StartCurve)
					{ 
						deviationAngle = drawEndAngle - drawStartAngle;
					}

					if (series.CapStyle == CapStyle.BothCurve)
					{
						float segmentSweepAngle = drawEndAngle - drawStartAngle;
						float maxDeviation = 2 * deviationAngle;

						if (segmentSweepAngle < maxDeviation)
						{ 
							deviationAngle = segmentSweepAngle / 2;
						}
					}

					segmentStartAngle += deviationAngle;
					PointF[] startCurvePoints = CalculateCurvePoints(series, segmentStartAngle, drawStartAngle, innerRadius, outerRadius, midRadius);
					_innerCurveStartPoint = startCurvePoints[0];
					_innerCurveStartMidpoint = startCurvePoints[1];
					_startCurveCeterPoint = startCurvePoints[2];
					_outerCurveStartMidPoint = startCurvePoints[3];
					_outerCurveStartPoint = startCurvePoints[4];

				}

				if (series.CapStyle == CapStyle.EndCurve || series.CapStyle == CapStyle.BothCurve)
				{
					if ((drawEndAngle - deviationAngle) <= drawStartAngle && series.CapStyle == CapStyle.EndCurve)
					{
						deviationAngle = drawEndAngle - drawStartAngle;
					}

					segmentEndAngle -= deviationAngle;
					PointF[] endCurvePoints = CalculateCurvePoints(series, segmentEndAngle, drawEndAngle, innerRadius, outerRadius, midRadius);
					_innerCurveEndPoint = endCurvePoints[0];
					_innerCurveEndMidpoint = endCurvePoints[1];
					_endCurveCenterPoint = endCurvePoints[2];
					_outerCurveEndMidPoint = endCurvePoints[3];
					_outerCurveEndPoint = endCurvePoints[4];
				}

				// To draw the start curve for the segment
				if (series.CapStyle == CapStyle.StartCurve || series.CapStyle == CapStyle.BothCurve)
				{
					path.MoveTo(_innerCurveStartPoint);
					path.CurveTo(_innerCurveStartPoint, _innerCurveStartMidpoint, _startCurveCeterPoint);
					path.CurveTo(_startCurveCeterPoint, _outerCurveStartMidPoint, _outerCurveStartPoint);		
				}

				if (NormalizeAngle(segmentStartAngle) != NormalizeAngle(segmentEndAngle))
				{
					// To draw the outer arc for the segment 
					path.AddArc(_actualBounds.Left, _actualBounds.Top, _actualBounds.Right, _actualBounds.Bottom, -segmentStartAngle, -(segmentEndAngle), series._isClockWise);

					// To draw the inner arc for the segment 
					path.AddArc(_currentBounds.Left, _currentBounds.Top, _currentBounds.Right, _currentBounds.Bottom, -(segmentEndAngle), -segmentStartAngle, !series._isClockWise);
				}

				// To draw the end curve for the segment
				if (series.CapStyle == CapStyle.EndCurve || series.CapStyle == CapStyle.BothCurve)
				{
					path.MoveTo(_innerCurveEndPoint);
					path.CurveTo(_innerCurveEndPoint, _innerCurveEndMidpoint, _endCurveCenterPoint);
					path.CurveTo(_endCurveCenterPoint, _outerCurveEndMidPoint, _outerCurveEndPoint);
				}

				canvas.SetFillPaint(Fill, path.Bounds);
				canvas.Alpha = Opacity;
				canvas.FillPath(path);
				SegmentStrokePath(canvas, series, segmentStartAngle, segmentEndAngle, drawStartAngle);
			}
		}

		void DrawFlatSegment(ICanvas canvas, DoughnutSeries series, float drawStartAngle, float drawEndAngle, PathF path)
		{
			var radius = (float)series.InnerRadius * (Math.Min(_actualBounds.Width, _actualBounds.Height) / 2);
			var x = (float)(_currentBounds.X + (_currentBounds.Width / 2) + (radius * Math.Cos(drawStartAngle * (Math.PI / 180))));
			var y = (float)(_currentBounds.Y + (_currentBounds.Width / 2) + (radius * Math.Sin(drawStartAngle * (Math.PI / 180))));

			path.MoveTo(x, y);
			path.AddArc(_actualBounds.Left, _actualBounds.Top, _actualBounds.Right, _actualBounds.Bottom, -drawStartAngle, -(drawEndAngle), series._isClockWise);
			path.AddArc(_currentBounds.Left, _currentBounds.Top, _currentBounds.Right, _currentBounds.Bottom, -(drawEndAngle), -drawStartAngle, !series._isClockWise);
			path.Close();
			canvas.SetFillPaint(Fill, path.Bounds);
			canvas.Alpha = Opacity;
			canvas.FillPath(path);

			if (HasStroke)
			{
				canvas.StrokeColor = Stroke.ToColor();
				canvas.StrokeSize = (float)StrokeWidth;
				canvas.DrawPath(path);
			}
		}

		PointF[] CalculateCurvePoints(DoughnutSeries series, float segmentAngle, float arcAngle, float innerRadius, float outerRadius, float midRadius)
		{
			// Convert degrees to radians
			const double DegreeToRadian = Math.PI / 180;
			double angleRadius = segmentAngle * DegreeToRadian;
			double arcRadius = arcAngle * DegreeToRadian;

			var center = series.Center;

			// Calculate points using a helper function for readability
			PointF innerPoint = ComputePoint(center, innerRadius, angleRadius);
			PointF outerPoint = ComputePoint(center, outerRadius, angleRadius);
			PointF centerPoint = ComputePoint(center, midRadius, arcRadius);
			PointF innerMidPoint = ComputePoint(center, innerRadius, arcRadius);
			PointF outerMidPoint = ComputePoint(center, outerRadius, arcRadius);

			return [innerPoint, innerMidPoint, centerPoint, outerMidPoint, outerPoint];
		}

		private PointF ComputePoint(PointF center, float radius, double angle)
		{
			return new PointF(
				(float)(center.X + (radius * Math.Cos(angle)) + _explodeOffsetX),
				(float)(center.Y + (radius * Math.Sin(angle)) + _explodeOffsetY)
			);
		}

		void SegmentStrokePath(ICanvas canvas, DoughnutSeries series, float segmentStartAngle, float segmentEndAngle, float segmentAngle)
		{
			if (HasStroke)
			{
				PathF strokePath = new PathF();

				// To draw the start curve for the segment stroke
				if (series.CapStyle == CapStyle.BothCurve || series.CapStyle == CapStyle.StartCurve)
				{
					strokePath.MoveTo(_innerCurveStartPoint);
					strokePath.CurveTo(_innerCurveStartPoint, _innerCurveStartMidpoint, _startCurveCeterPoint);
					strokePath.CurveTo(_startCurveCeterPoint, _outerCurveStartMidPoint, _outerCurveStartPoint);
				}

				if (NormalizeAngle(segmentStartAngle) != NormalizeAngle(segmentEndAngle))
				{
					// To draw the outer arc for the segment stroke
					strokePath.AddArc(_actualBounds.Left, _actualBounds.Top, _actualBounds.Right, _actualBounds.Bottom, -segmentStartAngle, -(segmentEndAngle), series._isClockWise);
				}

				// To draw the end curve for the segment stroke
				if (series.CapStyle == CapStyle.EndCurve || series.CapStyle == CapStyle.BothCurve)
				{
					strokePath.MoveTo(_innerCurveEndPoint);
					strokePath.CurveTo(_innerCurveEndPoint, _innerCurveEndMidpoint, _endCurveCenterPoint);
					strokePath.CurveTo(_endCurveCenterPoint, _outerCurveEndMidPoint, _outerCurveEndPoint);
					strokePath.MoveTo(_innerCurveEndPoint);
				}

				if (NormalizeAngle(segmentStartAngle) != NormalizeAngle(segmentEndAngle))
				{
					// To draw the inner arc for the segment stroke
					strokePath.AddArc(_currentBounds.Left, _currentBounds.Top, _currentBounds.Right, _currentBounds.Bottom, -(segmentEndAngle), -segmentStartAngle, !series._isClockWise);
				}

				if (NormalizeAngle(segmentStartAngle) == NormalizeAngle(segmentEndAngle) && series.CapStyle == CapStyle.StartCurve)
				{
					strokePath.Close();
				}

				canvas.StrokeColor = Stroke.ToColor();
				canvas.StrokeLineCap = LineCap.Round;
				canvas.StrokeSize = (float)StrokeWidth;
				canvas.DrawPath(strokePath); 

				if (series.CapStyle == CapStyle.EndCurve)
				{
					double angleRad = segmentAngle * (Math.PI / 180);
					var innerPoint = new PointF((float)(series.Center.X + (InnerRadius * Math.Cos(angleRad)) + _explodeOffsetX), (float)(series.Center.Y + (InnerRadius * Math.Sin(angleRad)) + _explodeOffsetY));
					var outerPoint = new PointF((float)(series.Center.X + (_segmentRadius * Math.Cos(angleRad)) + _explodeOffsetX), (float)(series.Center.Y + (_segmentRadius * Math.Sin(angleRad)) + _explodeOffsetY));

					canvas.StrokeLineCap = LineCap.Round;

					//To draw line for the segment stroke
					canvas.DrawLine(innerPoint, outerPoint);
				}
			}
		}

        // Normalizes angle precision to avoid rendering artifacts specific to platform.
		static float NormalizeAngle(float angle)
		{
#if IOS || MACCATALYST
			return MathF.Round(angle, 4); 
#else
			return MathF.Round(angle, 5);
#endif
		}

		#endregion

		#endregion
	}
}
