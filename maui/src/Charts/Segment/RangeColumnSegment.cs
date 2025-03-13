using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// Represents a segment of <see cref="RangeColumnSeries"/>.
	/// </summary>
	public partial class RangeColumnSegment : ColumnSegment
	{
		#region Fields

		double _x1, _x2, _y1, _y2, _xValue;
		internal PointF[] _labelPositionPoints = new PointF[2];
		internal string[] _dataLabel = new string[2];

		#endregion

		#region Methods

		#region Internal Methods

		/// <summary>
		/// Converts the data points to corresponding screen points for rendering the range column segment.
		/// </summary>
		internal override void SetData(double[] values)
		{
			if (Series is not RangeColumnSeries series)
			{
				return;
			}

			_x1 = values[0];
			_x2 = values[1];
			_y1 = values[2];
			_y2 = values[3];
			_xValue = values[4];

			Empty = double.IsNaN(_x1) || double.IsNaN(_x2) || double.IsNaN(_y1) || double.IsNaN(_y2);

			series.XRange += DoubleRange.Union(_xValue);
			series.YRange += new DoubleRange(_y1, _y2);
		}

		internal override void OnDataLabelLayout()
		{
			if (Series is RangeColumnSeries series)
			{
				CalculateDataLabelsPosition(_xValue, _y1, _y2, series);
			}
		}

		#endregion

		#region Protected Internal Methods

		/// <inheritdoc/>
		protected internal override void Draw(ICanvas canvas)
		{
			if (Series is not RangeColumnSeries rangeColumn || rangeColumn.ActualXAxis == null)
			{
				return;
			}

			if (!float.IsNaN(Left) && !float.IsNaN(Top) && !float.IsNaN(Right) && !float.IsNaN(Bottom))
			{
				if (rangeColumn.CanAnimate())
				{
					LayoutSegment();
				}

				canvas.StrokeSize = (float)StrokeWidth;
				canvas.StrokeColor = Stroke.ToColor();
				canvas.StrokeDashPattern = StrokeDashArray?.ToFloatArray();
				canvas.Alpha = Opacity;
				CornerRadius cornerRadius = rangeColumn.CornerRadius;

				//segment Drawing
				var rect = new Rect() { Left = Left, Top = Top, Right = Right, Bottom = Bottom };
				var actualCrossingValue = rangeColumn.ActualXAxis.ActualCrossingValue;
				canvas.SetFillPaint(Fill, rect);

				if (cornerRadius.TopLeft > 0 || cornerRadius.TopRight > 0 || cornerRadius.BottomLeft > 0 || cornerRadius.BottomRight > 0)
				{
					if (_y1 < (double.IsNaN(actualCrossingValue) ? 0 : actualCrossingValue))
					{
						canvas.FillRoundedRectangle(rect, cornerRadius.BottomLeft, cornerRadius.BottomRight, cornerRadius.TopLeft, cornerRadius.TopRight);
					}
					else
					{
						canvas.FillRoundedRectangle(rect, cornerRadius.TopLeft, cornerRadius.TopRight, cornerRadius.BottomLeft, cornerRadius.BottomRight);
					}
				}
				else
				{
					canvas.FillRectangle(rect);
				}

				//Draw Stroke
				if (HasStroke)
				{
					if (cornerRadius.TopLeft > 0 || cornerRadius.TopRight > 0 || cornerRadius.BottomLeft > 0 || cornerRadius.BottomRight > 0)
					{
						if (_y1 < (double.IsNaN(actualCrossingValue) ? 0 : actualCrossingValue))
						{
							canvas.DrawRoundedRectangle(rect, cornerRadius.BottomLeft, cornerRadius.BottomRight, cornerRadius.TopLeft, cornerRadius.TopRight);
						}
						else
						{
							canvas.DrawRoundedRectangle(rect, cornerRadius.TopLeft, cornerRadius.TopRight, cornerRadius.BottomLeft, cornerRadius.BottomRight);
						}
					}
					else
					{
						canvas.DrawRectangle(rect);
					}
				}
			}
		}

		/// <inheritdoc/>
		protected internal override void OnLayout()
		{
			LayoutSegment();
		}

		#endregion

		#region Private Methods

		void LayoutSegment()
		{
			if (Series is not RangeColumnSeries series || series.ChartArea == null || series.ActualXAxis == null)
			{
				return;
			}

			var xAxis = series.ActualXAxis;
			var start = Math.Floor(xAxis.VisibleRange.Start);
			var end = Math.Ceiling(xAxis.VisibleRange.End);

			double y1Value = _y1;
			double y2Value = _y2;
			var midY = (_y1 + _y2) / 2;

			if (series.CanAnimate())
			{
				var animationValue = series.AnimationValue;

				if (!series.XRange.Equals(series.PreviousXRange) || (float.IsNaN(PreviousY1) && float.IsNaN(PreviousY2)))
				{
					y1Value = midY + ((y1Value - midY) * series.AnimationValue);
					y2Value = midY - ((midY - y2Value) * series.AnimationValue);
				}
				else
				{
					y1Value = RangeColumnSegment.GetColumnDynamicAnimationValue(animationValue, PreviousY1, _y1);
					y2Value = RangeColumnSegment.GetColumnDynamicAnimationValue(animationValue, PreviousY2, _y2);
				}
			}

			Left = Top = Right = Bottom = float.NaN;

			if (_x1 <= end && _x2 >= start)
			{
				Left = series.TransformToVisibleX(_x1, y1Value);
				Top = series.TransformToVisibleY(_x1, y1Value);
				Right = series.TransformToVisibleX(_x2, y2Value);
				Bottom = series.TransformToVisibleY(_x2, y2Value);

				if (Left > Right)
				{
					(Right, Left) = (Left, Right);
				}

				if (Top > Bottom)
				{
					(Bottom, Top) = (Top, Bottom);
				}

				Y1 = (float)y1Value;
				Y2 = (float)y2Value;
			}
			else
			{
				Left = float.NaN;
			}

			SegmentBounds = new RectF(Left, Top, Right - Left, Bottom - Top);
		}

		void CalculateDataLabelsPosition(double xValue, double high, double low, RangeColumnSeries series)
		{
			IsZero = double.IsNaN(high) && double.IsNaN(low);
			InVisibleRange = series.IsDataInVisibleRange(xValue, high) && series.IsDataInVisibleRange(xValue, low);
			double x = xValue, x1 = xValue, y = series.GetDataLabelPositionAtIndex(Index, high), y1 = series.GetDataLabelPositionAtIndex(Index, low);
			series.CalculateDataPointPosition(Index, ref x, ref y);
			series.CalculateDataPointPosition(Index, ref x1, ref y1);
			PointF highPoint = new PointF((float)x, (float)y);
			PointF lowPoint = new PointF((float)x1, (float)y1);
			series._sumOfHighValues = float.IsNaN(series._sumOfHighValues) ? series.SumOfValues(series.HighValues) : series._sumOfHighValues;
			series._sumOfLowValues = float.IsNaN(series._sumOfLowValues) ? series.SumOfValues(series.LowValues) : series._sumOfLowValues;
			_dataLabel[0] = series.GetLabelContent(high, series._sumOfHighValues);
			_dataLabel[1] = series.GetLabelContent(low, series._sumOfLowValues);
			UpdateDataLabels(highPoint, lowPoint);
		}

		void UpdateDataLabels(PointF highPoint, PointF lowPoint)
		{
			for (int i = 0; i < 2; i++)
			{
				if (Series is RangeColumnSeries series)
				{
					var dataLabelSettings = series.DataLabelSettings;

					if (DataLabels != null && DataLabels.Count > i)
					{
						ChartDataLabel dataLabel = DataLabels[i];

						dataLabel.LabelStyle = dataLabelSettings.LabelStyle;
						dataLabel.Background = dataLabelSettings.LabelStyle.Background;
						dataLabel.Index = Index;
						dataLabel.Item = Item;
						dataLabel.Label = _dataLabel[i];

						if (i == 0)
						{
							_labelPositionPoints[i] = series.IsDataInVisibleRange(_xValue, _y1) && !Empty ? dataLabelSettings.CalculateDataLabelPoint(series, this, highPoint, dataLabelSettings.LabelStyle, "HighType") : new PointF(float.NaN, float.NaN);

						}
						else if (i == 1)
						{
							_labelPositionPoints[i] = series.IsDataInVisibleRange(_xValue, _y2) && !Empty ? dataLabelSettings.CalculateDataLabelPoint(series, this, lowPoint, dataLabelSettings.LabelStyle, "LowType") : new PointF(float.NaN, float.NaN);
						}

						dataLabel.XPosition = _labelPositionPoints[i].X;
						dataLabel.YPosition = _labelPositionPoints[i].Y;
					}
				}
			}
		}

		static float GetColumnDynamicAnimationValue(float animationValue, double oldValue, double currentValue)
		{
			if (!double.IsNaN(oldValue) && !double.IsNaN(currentValue))
			{
				return (float)((currentValue > oldValue) ?
					oldValue + ((currentValue - oldValue) * animationValue)
					: oldValue - ((oldValue - currentValue) * animationValue));
			}
			else
			{
				return double.IsNaN(oldValue) ? (float)currentValue * animationValue : (float)(oldValue - (oldValue * animationValue));
			}
		}

		#endregion

		#endregion
	}
}