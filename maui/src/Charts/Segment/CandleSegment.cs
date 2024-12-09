using Microsoft.Maui.Graphics;
using System;

namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// Represents a segment for the <see cref="CandleSeries"/> chart, commonly used in financial or stock charting.
	/// <para>Each segment corresponds to a single data point in the series, showing the open, high, low, and close values for a given period.</para>
	/// </summary>
	public partial class CandleSegment : HiLoOpenCloseSegment
	{
		#region Methods

		#region Internal Methods

		/// <summary>
		/// Converts the data points to corresponding screen points for rendering the candle segment.
		/// </summary>
		internal override void SetData(double[] values, bool isFill, bool isBull)
		{
			if (Series is not CandleSeries series)
			{
				return;
			}

			StartX = values[0];
			CenterX = values[1];
			EndX = values[2];
			Open = values[3];
			High = values[4];
			Low = values[5];
			Close = values[6];
			XValue = values[7];
			IsFill = isFill;
			IsBull = isBull;

			series.XRange += DoubleRange.Union(XValue);
			series.YRange += new DoubleRange(High, Low);
		}

		internal override int GetDataPointIndex(float x, float y)
		{
			bool verticalMax = IsRectContains(CenterHigh, HighPointY, CenterLow, Top, x, y, (float)StrokeWidth);
			bool verticalMin = IsRectContains(CenterHigh, Bottom, CenterLow, LowPointY, x, y, (float)StrokeWidth);

			if (Series != null && (SegmentBounds.Contains(x, y) || verticalMax || verticalMin))
			{
				return Series._segments.IndexOf(this);
			}

			return -1;
		}

		#endregion

		#region Protected Methods

		/// <inheritdoc/>
		protected internal override void OnLayout()
		{
			if (Series is not CandleSeries series)
			{
				return;
			}

			Layout(series);
		}

		/// <inheritdoc/>
		protected internal override void Draw(ICanvas canvas)
		{
			if (Series is not CandleSeries series)
			{
				return;
			}

			if (series.CanAnimate())
			{
				Layout(series);
			}

			canvas.Alpha = Opacity;
			canvas.StrokeSize = (float)StrokeWidth;

			if (Stroke != null)
			{
				canvas.StrokeColor = Stroke.ToColor();
			}
			else
			{
				if (IsBull)
				{
					canvas.StrokeColor = series.BullishFill.ToColor();
				}
				else
				{
					canvas.StrokeColor = series.BearishFill.ToColor();
				}
			}

			var rect = new Rect() { Left = Left, Top = Top, Right = Right, Bottom = Bottom };
			canvas.SetFillPaint(Fill, rect);
			canvas.FillRectangle(rect);
			canvas.DrawRectangle(rect);

			if (series.ChartArea?.IsTransposed is true)
			{
				canvas.DrawLine(Left, LowPointY, CenterLow, LowPointY);
				canvas.DrawLine(Right, LowPointY, CenterHigh, LowPointY);
			}
			else
			{
				canvas.DrawLine(CenterHigh, HighPointY, CenterLow, Top);
				canvas.DrawLine(CenterHigh, Bottom, CenterLow, LowPointY);
			}
		}

		#endregion

		#region Private Methods

		void Layout(CandleSeries series)
		{
			var xAxis = series.ActualXAxis;

			if (xAxis == null)
			{
				return;
			}

			var start = Math.Floor(xAxis.VisibleRange.Start);
			var end = Math.Ceiling(xAxis.VisibleRange.End);

			var temp = High;

			if (High < Low)
			{
				High = Low;
				Low = temp;
			}

			High = High < Close ? Close : High < Open ? Open : High;

			temp = Close;

			if (Low > High)
			{
				High = Low;
				Low = temp;
			}

			Low = Low > Open ? Open : Low > Close ? Close : Low;

			Left = Right = Top = Bottom = float.NaN;

			if (StartX <= end && EndX >= start)
			{
				Left = series.TransformToVisibleX(StartX, Open);
				Top = series.TransformToVisibleY(StartX, Open);
				Right = series.TransformToVisibleX(EndX, Close);
				Bottom = series.TransformToVisibleY(EndX, Close);

				CenterLow = series.TransformToVisibleX(CenterX, Low);
				CenterHigh = series.TransformToVisibleX(CenterX, High);

				HighPointY = series.TransformToVisibleY(CenterX, High);
				LowPointY = series.TransformToVisibleY(CenterX, Low);

				if (series.CanAnimate())
				{
					AnimationValuesCalculation(series);
				}

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

			SegmentBounds = new RectF(Left, Top, Right - Left, Bottom - Top);
		}

		void AnimationValuesCalculation(CandleSeries series)
		{
			float animationValue = series.AnimationValue;

			if (series.ChartArea?.IsTransposed is true)
			{
				float centerX = Left + ((Left - Right) / 2);
				Left = centerX + ((Left - centerX) * animationValue);
				Right = centerX - ((centerX - Right) * animationValue);
				CenterHigh = centerX - ((centerX - CenterHigh) * animationValue);
				CenterLow = centerX + ((CenterLow - centerX) * animationValue);
			}
			else
			{
				float centerY = Top + ((Bottom - Top) / 2);
				Top = centerY - ((centerY - Top) * animationValue);
				Bottom = centerY + ((Bottom - centerY) * animationValue);
				HighPointY = centerY - ((centerY - HighPointY) * animationValue);
				LowPointY = centerY + ((LowPointY - centerY) * animationValue);
			}
		}

		#endregion

		#endregion
	}
}
