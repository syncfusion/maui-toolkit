using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// Represents a segment of the <see cref="WaterfallSeries"/> chart.
	/// </summary>
	public partial class WaterfallSegment : CartesianSegment
	{
		#region Fields

		#region Private Fields

		double _differenceValue;

		float _lineX1, _lineY1, _lineX2, _lineY2;

		#endregion

		#region Internal Fields

		internal WaterfallSegment? _previousWaterfallSegment;

		internal double _x1, _y1, _x2, _y2, _xValue;

		#endregion

		#endregion

		#region Properties

		#region Internal Property

		internal WaterfallSegmentType SegmentType { get; set; }

		#endregion

		#region Public  Property

		/// <summary>
		/// Gets the left position value for the waterfall segment.
		/// </summary>
		public float Left { get; internal set; }

		/// <summary>
		/// Gets the top position value for the waterfall segment.
		/// </summary>
		public float Top { get; internal set; }

		/// <summary>
		/// Gets the right position value for the waterfall segment.
		/// </summary>
		public float Right { get; internal set; }

		/// <summary>
		/// Gets the bottom position value for the waterfall segment.
		/// </summary>
		public float Bottom { get; internal set; }

		internal double WaterfallSum { get; set; }

		internal double Sum { get; set; }

		#endregion

		#endregion

		#region Methods

		#region Protected Internal Override Methods

		/// <inheritdoc/>
		protected internal override void OnLayout()
		{
			if (Series is WaterfallSeries series)
			{
				Layout(series);
			}
		}

		/// <inheritdoc/>
		protected internal override void Draw(ICanvas canvas)
		{
			if (Series is not WaterfallSeries waterfallSeries)
			{
				return;
			}

			if (!double.IsNaN(Left) && !double.IsNaN(Top) && !double.IsNaN(Right) && !double.IsNaN(Bottom))
			{
				canvas.Alpha = Opacity;

				if (waterfallSeries.CanAnimate())
				{
					Layout(waterfallSeries);
				}

				//Drawing rectangle segment.
				var rect = new Rect() { Left = Left, Top = Top, Right = Right, Bottom = Bottom };

				canvas.SetFillPaint(Fill, rect);
				canvas.FillRectangle(rect);

				if (HasStroke)
				{
					canvas.DrawRectangle(rect);
				}

				//Drawing Connector Line.
				if (waterfallSeries.ShowConnectorLine && _previousWaterfallSegment != null)
				{
					var connectorLineStyle = waterfallSeries.ConnectorLineStyle;

					if (connectorLineStyle != null)
					{
						canvas.StrokeColor = (connectorLineStyle.Stroke as SolidColorBrush)?.Color ?? Colors.Black;
						canvas.StrokeSize = (float)connectorLineStyle.StrokeWidth;

						if (connectorLineStyle.StrokeDashArray != null)
						{
							canvas.StrokeDashPattern = connectorLineStyle.StrokeDashArray?.ToFloatArray();
						}
					}

					canvas.DrawLine(_lineX1, _lineY1, _lineX2, _lineY2);
				}
			}
		}

		#endregion

		#region Internal Override Methods

		internal override int GetDataPointIndex(float x, float y)
		{
			if (Series != null && SegmentBounds.Contains(x, y))
			{
				return Series._segments.IndexOf(this);
			}

			return -1;
		}

		internal override void OnDataLabelLayout()
		{
			if (Series is WaterfallSeries waterfallSeries)
			{
				if (waterfallSeries.AllowAutoSum == true)
				{
					CalculateDataLabelPosition(_xValue, SegmentType == WaterfallSegmentType.Sum ? _y1 : _differenceValue, SegmentType == WaterfallSegmentType.Sum ? _y1 : _differenceValue);
				}
				else
				{
					CalculateDataLabelPosition(_xValue, _differenceValue, _differenceValue);
				}
			}
		}

		/// <summary>
		/// Converts the data points to corresponding screen points for rendering the waterfall segment.
		/// </summary>
		internal override void SetData(double[] values)
		{
			if (Series is WaterfallSeries series)
			{
				_x1 = values[0];
				_x2 = values[1];
				_y1 = values[2];
				_y2 = values[3];
				_xValue = values[4];
				_differenceValue = values[5];

				series.XRange += DoubleRange.Union(_xValue);
				series.YRange += new DoubleRange(_y1, _y2);
			}
		}

		#endregion

		#region Private Methods

		void Layout(WaterfallSeries? series)
		{
			var xAxis = series?.ActualXAxis;

			if (series == null || series.ChartArea == null || xAxis == null)
			{
				return;
			}

			var start = Math.Floor(xAxis.VisibleRange.Start);
			var end = Math.Ceiling(xAxis.VisibleRange.End);
			Left = Top = Right = Bottom = float.NaN;

			if (_x1 <= end && _x2 >= start)
			{
				Left = series.TransformToVisibleX(_x1, _y1);
				Top = series.TransformToVisibleY(_x1, _y1);
				Right = series.TransformToVisibleX(_x2, _y2);
				Bottom = series.TransformToVisibleY(_x2, _y2);

				if (Left > Right)
				{
					(Right, Left) = (Left, Right);
				}

				if (Top > Bottom)
				{
					(Bottom, Top) = (Top, Bottom);
				}
			}
			else
			{
				Left = float.NaN;
			}

			float midPoint = ((Top + Bottom) / 2) - Top;
			float midPointTransposed = (Right + Left) / 2 - Right;

			if (series.CanAnimate())
			{
				if (series.ChartArea.IsTransposed)
				{
					Right += midPointTransposed * (1 - series.AnimationValue);
					Left -= midPointTransposed * (1 - series.AnimationValue);
				}
				else
				{
					Top += midPoint * (1 - series.AnimationValue);
					Bottom -= midPoint * (1 - series.AnimationValue);
				}
			}

			if (!series.CanAnimate())
			{
				//Updating the ConnectorLine points
				UpdateConnectorLine(series);
			}

			SegmentBounds = new RectF(Left, Top, Right - Left, Bottom - Top);
		}

		void UpdateConnectorLine(WaterfallSeries series)
		{
			if (_previousWaterfallSegment != null)
			{
				//Setting X-Coordinates
				_lineX1 = _previousWaterfallSegment.Right;
				_lineX2 = Left;

				//Setting Y-Coordinates for summary type segments
				if (SegmentType == WaterfallSegmentType.Sum)
				{
					if (series.AllowAutoSum)
					{
						_lineY1 = _lineY2 = (WaterfallSum >= 0) ? Top : Bottom;
					}
					else
					{
						_lineY2 = (series.YValues[(int)_xValue - 1] >= 0) ? Top : Bottom;

						if (_previousWaterfallSegment.SegmentType == WaterfallSegmentType.Positive || (_previousWaterfallSegment.SegmentType == WaterfallSegmentType.Sum && series.YValues[(int)_previousWaterfallSegment._xValue] >= 0))
						{
							_lineY1 = _previousWaterfallSegment.Top;
						}
						else
						{
							_lineY1 = _previousWaterfallSegment.Bottom;
						}
					}
				}
				//Setting Y-Coordinates for negative type segments
				else if (SegmentType == WaterfallSegmentType.Negative)
				{
					_lineY1 = _lineY2 = Top;
				}
				//Setting Y-Coordinates for positive type segments
				else
				{
					_lineY1 = _lineY2 = Bottom;
				}

				if (series.ChartArea != null && series.ChartArea.IsTransposed)
				{
					if (SegmentType == WaterfallSegmentType.Negative && _previousWaterfallSegment.SegmentType == WaterfallSegmentType.Positive)
					{
						_lineX1 = _previousWaterfallSegment.Right;
						_lineY1 = _previousWaterfallSegment.Top;
						_lineX2 = Right;
						_lineY2 = Bottom;
					}

					if (_previousWaterfallSegment.SegmentType == WaterfallSegmentType.Negative)
					{
						_lineX1 = _previousWaterfallSegment.Left;
						_lineY1 = _previousWaterfallSegment.Top;
						_lineX2 = (SegmentType == WaterfallSegmentType.Sum) ? Right : Left;
						_lineY2 = Bottom;
					}

					if (_previousWaterfallSegment.SegmentType == WaterfallSegmentType.Positive && SegmentType == WaterfallSegmentType.Positive)
					{
						_lineX1 = _previousWaterfallSegment.Right;
						_lineY1 = _previousWaterfallSegment.Top;
						_lineX2 = Left;
						_lineY2 = Bottom;
					}

					if (_previousWaterfallSegment.SegmentType == WaterfallSegmentType.Negative && SegmentType == WaterfallSegmentType.Negative)
					{
						_lineX1 = _previousWaterfallSegment.Left;
						_lineY1 = _previousWaterfallSegment.Top;
						_lineX2 = Right;
						_lineY2 = Bottom;
					}

					if (_previousWaterfallSegment.SegmentType == WaterfallSegmentType.Positive && SegmentType == WaterfallSegmentType.Sum)
					{
						_lineX1 = _previousWaterfallSegment.Right;
						_lineY1 = _previousWaterfallSegment.Top;
						_lineX2 = Right;
						_lineY2 = Bottom;
					}

					if (_previousWaterfallSegment.SegmentType == WaterfallSegmentType.Sum)
					{
						_lineX1 = _previousWaterfallSegment.Right;
						_lineY1 = _previousWaterfallSegment.Top;

						if (SegmentType == WaterfallSegmentType.Positive)
						{
							_lineX2 = Left;
							_lineY2 = Bottom;
						}
						if (SegmentType == WaterfallSegmentType.Negative)
						{
							_lineX2 = Right;
							_lineY2 = Bottom;
						}
					}
				}

				if (series.ActualYAxis != null && series.ActualYAxis.IsInversed)
				{
					_lineX1 = _previousWaterfallSegment.Right;
					_lineY1 = _previousWaterfallSegment.Bottom;
					_lineX2 = Left;
					_lineY2 = Top;
				}
				else if (series.ActualXAxis != null && series.ActualXAxis.IsInversed && SegmentType == WaterfallSegmentType.Sum)
				{

					_lineX1 = _previousWaterfallSegment.Left;
					_lineY1 = _previousWaterfallSegment.Top;
					_lineX2 = Right;
					_lineY2 = Top;
				}

				float midPointX = (_lineX1 + _lineX2) / 2;
				float midPointY = (_lineY1 + _lineY2) / 2;
				_lineX1 = midPointX + (_lineX1 - midPointX);
				_lineY1 = midPointY + (_lineY1 - midPointY);
				_lineX2 = _lineX1 + (_lineX2 - _lineX1);
				_lineY2 = _lineY1 + (_lineY2 - _lineY1);
			}
		}

		#endregion

		#endregion
	}
}