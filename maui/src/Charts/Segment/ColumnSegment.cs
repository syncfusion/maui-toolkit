using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// Represents a segment of the <see cref="ColumnSeries"/> chart, including support for stacked column charts.
	/// This class manages the positioning and rendering of the column segment within the chart.
	/// </summary>
	public partial class ColumnSegment : CartesianSegment
	{
		#region Fields

		double _x1, _y1, _x2, _y2, _xvalue, _labelContent;

		#endregion

		#region Properties

		#region Public Properties

		/// <summary>
		/// Gets the left position value for the column segment.
		/// </summary>
		public float Left { get; internal set; }

		/// <summary>
		/// Gets the top position value for the column segment.
		/// </summary>
		public float Top { get; internal set; }

		/// <summary>
		/// Gets the right position value for the column segment.
		/// </summary>
		public float Right { get; internal set; }

		/// <summary>
		/// Gets the bottom position value for the column segment.
		/// </summary>
		public float Bottom { get; internal set; }

		#endregion

		#region Internal  Properties

		internal float Y1 { get; set; } = float.NaN;
		internal float Y2 { get; set; } = float.NaN;
		internal float PreviousY1 { get; set; } = float.NaN;
		internal float PreviousY2 { get; set; } = float.NaN;

		#endregion

		#endregion

		#region Methods

		#region Protected Methods

		/// <inheritdoc/>
		protected internal override void OnLayout()
		{
			if (Series is not CartesianSeries)
			{
				return;
			}

			Layout();
		}

		/// <inheritdoc/>
		protected internal override void Draw(ICanvas canvas)
		{
			if (Series is not CartesianSeries series || series is not ISBSDependent sBSDependent || series.ActualXAxis == null)
			{
				return;
			}

			if (series.CanAnimate())
			{
				Layout();
			}

			if (!float.IsNaN(Left) && !float.IsNaN(Top) && !float.IsNaN(Right) && !float.IsNaN(Bottom))
			{
				canvas.StrokeSize = (float)StrokeWidth;
				canvas.StrokeColor = Stroke.ToColor();
				canvas.StrokeDashPattern = StrokeDashArray?.ToFloatArray();
				canvas.Alpha = Opacity;
				CornerRadius cornerRadius = sBSDependent.CornerRadius;

				//Drawing segment.
				var rect = new Rect() { Left = Left, Top = Top, Right = Right, Bottom = Bottom };
				var actualCrossingValue = series.ActualXAxis.ActualCrossingValue;

				canvas.SetFillPaint(Fill, rect);

				if (cornerRadius.TopLeft > 0 || cornerRadius.TopRight > 0 || cornerRadius.BottomLeft > 0 || cornerRadius.BottomRight > 0)
				{
					// negative segment
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

				//Drawing stroke.
				if (HasStroke)
				{
					if (cornerRadius.TopLeft > 0 || cornerRadius.TopRight > 0 || cornerRadius.BottomLeft > 0 || cornerRadius.BottomRight > 0)
					{
						//negative segment stroke
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

		#endregion

		#region Internal Methods

		internal void Layout()
		{
			if (Series is not CartesianSeries series || series.ChartArea == null || series.ActualXAxis == null)
			{
				return;
			}

			var xAxis = series.ActualXAxis;

			var crossingValue = double.IsNaN(xAxis.RenderingCrossesValue) ? series.GetAxisCrossingValue(xAxis) : xAxis.RenderingCrossesValue;
			var start = Math.Floor(xAxis.VisibleRange.Start);
			var end = Math.Ceiling(xAxis.VisibleRange.End);
			double y1Value = _y1;
			double y2Value = _y2;

			if (!double.IsNaN(crossingValue) && Series is not StackingColumnSeries)
			{
				//TODO: Ensure the better way to having this. 
				//if (seriesIndex == 0 || (seriesIndex != 0 && Series is CartesianSeries))
				y2Value = crossingValue;
			}

			if (series.CanAnimate())
			{
				float animationValue = series.AnimationValue;

				if (!series.XRange.Equals(series.PreviousXRange) || (float.IsNaN(PreviousY1) && float.IsNaN(PreviousY2)))
				{
					y1Value *= animationValue;
					y2Value *= animationValue;
				}
				else
				{
					y1Value = ColumnSegment.GetColumnDynamicAnimationValue(animationValue, PreviousY1, _y1);
					y2Value = ColumnSegment.GetColumnDynamicAnimationValue(animationValue, PreviousY2, _y2);
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

		/// <summary>
		/// Converts the data points to corresponding screen points for rendering the column segment.
		/// </summary>
		internal override void SetData(double[] values)
		{
			if (Series is not CartesianSeries series)
			{
				return;
			}

			_x1 = values[0];
			_x2 = values[1];
			_y1 = values[2];
			_y2 = values[3];
			_xvalue = values[4];
			_labelContent = values[5];

			Empty = double.IsNaN(_x1) || double.IsNaN(_x2) || double.IsNaN(_y1) || double.IsNaN(_y2);

			series.XRange += DoubleRange.Union(_xvalue);
			series.YRange += new DoubleRange(_y1, _y2);
		}

		internal void SetPreviousData(float[] values)
		{
			PreviousY1 = values[0];
			PreviousY2 = values[1];
		}

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
			CalculateDataLabelPosition(_xvalue, _y1, _labelContent);
		}

		#endregion

		#region Private Methods


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
