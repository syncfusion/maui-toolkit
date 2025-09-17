using System.Collections.ObjectModel;
using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.Charts
{
	internal partial class PolarAxisLayoutView : SfDrawableView
	{
		#region Fields

		readonly PolarChartArea _chartArea;
		ChartAxis _xAxis => _chartArea.GetPrimaryAxis();
		ChartAxis _yAxis => _chartArea.GetSecondaryAxis();
		float _left, _right;

		#endregion

		#region Constructor

		public PolarAxisLayoutView(PolarChartArea area)
		{
			_chartArea = area;
		}

		#endregion

		#region Internal Method

		internal void AssignAxisToSeries()
		{
			var visibleSeries = _chartArea.VisibleSeries;
			if (visibleSeries == null)
			{
				return;
			}

			PolarAxisLayoutView.ClearActualAxis(visibleSeries);
			UpdateActualAxis(visibleSeries);
			UpdateSeriesRange(visibleSeries);
		}

		internal void LayoutAxis(Rect areaBounds)
		{
			Measure(areaBounds.Size);
		}

		#endregion

		#region Protected Method

		protected override void OnDraw(ICanvas canvas, RectF dirtyRect)
		{
			if (_xAxis == null || _yAxis == null)
			{
				return;
			}

			if (_yAxis.IsVertical)
			{
				canvas.CanvasSaveState();
				var arrangeRect = _yAxis.ArrangeRect;
				canvas.Translate((float)arrangeRect.Left, (float)arrangeRect.Top);
				_yAxis.AxisRenderer?.OnDraw(canvas);

				canvas.CanvasRestoreState();
			}


			DrawPrimaryAxis(canvas, _xAxis);
		}

		#endregion

		#region Private Method

		Size Measure(Size availableSize)
		{
			_left = _right = 0;
			float Top = 0, Bottom = 0;

			_chartArea.PolarAxisCenter = new PointF((float)(availableSize.Width / 2), (float)(availableSize.Height / 2));
			_chartArea.ActualSeriesClipRect = new RectF(0, 0, (float)availableSize.Width, (float)availableSize.Height);

			SizeF maximumLabelSize, verticalAxisSize;
			MeasureHorizontalAxis(availableSize);
			maximumLabelSize = ComputeMaximumLabelSize();
			verticalAxisSize = new SizeF((float)(availableSize.Width - (2 * maximumLabelSize.Width)), (float)(availableSize.Height - (2 * maximumLabelSize.Height)));
			MeasureVerticalAxis(verticalAxisSize);

			if(_yAxis != null)
				_left -= (float)_yAxis.InsidePadding;

			_chartArea.PlotAreaMargin = new Thickness(_left, Top, _right, Bottom);
			UpdateArrangeRect(availableSize);

			return Size.Zero;
		}

		SizeF ComputeMaximumLabelSize()
		{
			SizeF maxSize;
			float maxWidth = 0, maxHeight = 0;
			ChartAxis primaryAxis = _chartArea._polarChart.PrimaryAxis;
			if (primaryAxis == null)
			{
				return SizeF.Zero;
			}

			for (int i = 0; i < primaryAxis.VisibleLabels.Count; i++)
			{
				var label = primaryAxis.VisibleLabels[i];
				ChartAxisLabelStyle labelStyle = primaryAxis.LabelStyle;

				if (label.LabelStyle != null)
				{
					labelStyle = label.LabelStyle;
				}

				ChartLineStyle lineStyle = _chartArea._polarChart.PrimaryAxis.AxisLineStyle;
				ChartAxisTickStyle tickStyle = primaryAxis.MajorTickStyle;
				SizeF size = labelStyle.MeasureLabel(label.Content.Tostring());
				maxWidth = (float)Math.Max(size.Width + tickStyle.TickSize + (lineStyle.StrokeWidth * 2) + labelStyle.Margin.Left + labelStyle.Margin.Right, maxWidth);
				maxHeight = (float)Math.Max(size.Height + tickStyle.TickSize + (lineStyle.StrokeWidth * 2) + labelStyle.Margin.Top + labelStyle.Margin.Bottom, maxHeight);
			}

			maxSize = new SizeF(maxWidth, maxHeight);
			return maxSize;
		}

		void UpdateSeriesRange(ReadOnlyObservableCollection<ChartSeries> visibleSeries)
		{
			foreach (PolarSeries series in visibleSeries.Cast<PolarSeries>())
			{
				if (!series.SegmentsCreated)
				{
					series.XRange = DoubleRange.Empty;
					series.YRange = DoubleRange.Empty;
				}

				_chartArea.InternalCreateSegments(series);
				series.UpdateRange();
			}
		}

		static void ClearActualAxis(ReadOnlyObservableCollection<ChartSeries> visibleSeries)
		{
			foreach (PolarSeries series in visibleSeries.Cast<PolarSeries>())
			{
				if (series != null)
				{
					series.ActualXAxis = null;
					series.ActualYAxis = null;
				}
			}
		}

		void UpdateActualAxis(ReadOnlyObservableCollection<ChartSeries> visibleSeries)
		{
			foreach (PolarSeries series in visibleSeries.Cast<PolarSeries>())
			{
				if (series != null)
				{
					if (series.ActualXAxis == null)
					{
						var axis = _chartArea._polarChart.PrimaryAxis;
						axis.IsVertical = false;
						series.ActualXAxis = axis;
						axis.AddRegisteredSeries(series);
					}

					if (series.ActualYAxis == null)
					{
						var axis = _chartArea._polarChart.SecondaryAxis;
						axis.IsVertical = true;
						series.ActualYAxis = axis;
						axis.AddRegisteredSeries(series);
					}

					series.UpdateAssociatedAxes();
				}
			}
		}

		void MeasureHorizontalAxis(Size availableSize)
		{
			_xAxis?.ComputeSize(availableSize);
		}

		void MeasureVerticalAxis(Size availableSize)
		{
			float radius = (float)((availableSize.Height > availableSize.Width) ? (availableSize.Width / 2) : (availableSize.Height / 2));
			float minValue;
			if (availableSize.Height > availableSize.Width)
			{
				minValue = (float)(availableSize.Width / 2);
			}
			else
			{
				minValue = (float)(availableSize.Height / 2);
			}

			Size polarVerticalAxisSize = new(minValue, radius);
			var axis = _yAxis;
			if (axis != null)
			{
				axis.ComputeSize(polarVerticalAxisSize);
				_left += (float)axis.ComputedDesiredSize.Width;
			}
		}

		void UpdateArrangeRect(Size availableSize)
		{
			if (_xAxis == null || _yAxis == null)
			{
				return;
			}

			float availableWidth = (float)availableSize.Width;
			float availableHeight = (float)availableSize.Height;
			_xAxis.ArrangeRect = new RectF(0, 0, availableWidth, availableHeight);
			Size desiredSize = _yAxis.ComputedDesiredSize;
			availableWidth /= 2;
			availableHeight /= 2;
			var axisHeight = availableHeight - (float)desiredSize.Height;
			var axisWidth = availableWidth + (float)desiredSize.Width;
			_yAxis.ArrangeRect = new RectF(availableWidth - _left, axisHeight, axisWidth, availableHeight - axisHeight);
		}

		#endregion

		#region Polar Primary axis renderer methods

		void DrawPrimaryAxis(ICanvas canvas, ChartAxis primaryAxis)
		{
			int totalSpokes;
			if (primaryAxis is LogarithmicAxis)
			{
				totalSpokes = primaryAxis.VisibleLabels.Count - 1;
			}
			else
			{
				totalSpokes = primaryAxis.VisibleLabels.Count;
			}

			DrawMajorTick(canvas, primaryAxis, totalSpokes);
			DrawAxisLabel(canvas, primaryAxis, totalSpokes);
		}

		void DrawAxisLabel(ICanvas canvas, ChartAxis axis, float totalSpokes)
		{
			for (int i = 0; i < totalSpokes; i++)
			{
				var label = axis.VisibleLabels[i];
				ChartAxisLabelStyle labelStyle = axis.LabelStyle;
				if (label.LabelStyle != null)
				{
					labelStyle = label.LabelStyle;
				}

				SizeF size = labelStyle.MeasureLabel(label.Content.Tostring());
				var margin = labelStyle.Margin;
				float labelWidth = (float)(size.Width + margin.Left + margin.Right);
				float labelHeight = (float)(size.Height + margin.Top + margin.Bottom);
				PointF point = CalculateAxisLabelPoint(i, totalSpokes, labelStyle);

				var rect = new RectF((float)(point.X - size.Width / 2 - labelStyle.Margin.Left), (float)(point.Y - size.Height / 2 - labelStyle.Margin.Top), labelWidth, labelHeight);
				PolarAxisLayoutView.DrawLabelBackground(canvas, rect, labelStyle);
				labelStyle.DrawLabel(canvas, label.Content.Tostring(), point);
			}
		}

		static void DrawLabelBackground(ICanvas canvas, Rect rect, ChartAxisLabelStyle labelStyle)
		{
			float halfStrokeWidth = (float)labelStyle.StrokeWidth / 2;
			CornerRadius cornerRadius = labelStyle.CornerRadius;

			canvas.SetFillPaint(labelStyle.Background, rect);
			if (!labelStyle.HasCornerRadius && labelStyle.IsBackgroundColorUpdated)
			{
				canvas.FillRectangle(rect);
			}
			else
			{
				canvas.FillRoundedRectangle(rect, cornerRadius.TopLeft, cornerRadius.TopRight, cornerRadius.BottomLeft, cornerRadius.BottomRight);
			}

			if (labelStyle.IsStrokeColorUpdated)
			{
				canvas.StrokeColor = labelStyle.Stroke.ToColor();
				canvas.StrokeSize = halfStrokeWidth;
				if (labelStyle.HasCornerRadius)
				{
					canvas.DrawRoundedRectangle(rect, cornerRadius.TopLeft, cornerRadius.TopRight, cornerRadius.BottomLeft, cornerRadius.BottomRight);
				}
				else
				{
					canvas.DrawRectangle(rect);
				}
			}
		}

		PointF CalculateAxisLabelPoint(int index, float totalSpokes, ChartAxisLabelStyle labelStyle)
		{
			if (_xAxis == null || _yAxis == null)
			{
				return default;
			}

			float angle = 360 / totalSpokes;
			SizeF size = labelStyle.MeasureLabel(_xAxis.VisibleLabels[index].Content.Tostring());
			float labelWidth = (float)(size.Width + labelStyle.Margin.Left + labelStyle.Margin.Right);
			float labelHeight = (float)(size.Height + labelStyle.Margin.Top + labelStyle.Margin.Bottom);
			float radius = (float)(_yAxis.ComputedDesiredSize.Height + _xAxis.MajorTickStyle.TickSize);
			PointF pointF = _chartArea.PolarAngleToPoint(_xAxis, radius, index * angle);
			float labelAngle;
			var angel = _chartArea._polarChart.PolarStartAngle;
			if (_xAxis.IsInversed)
			{
				labelAngle = angel - (index * angle);
				labelAngle = labelAngle < 0 ? labelAngle + 360 : labelAngle;
			}
			else
			{
				labelAngle = (index * angle) + angel;
				labelAngle = labelAngle > 360 ? labelAngle - 360 : labelAngle;
			}

			if (labelAngle == 0 || labelAngle == 360)
			{
				pointF.X = (float)(pointF.X + labelWidth / 2 - labelStyle.Margin.Left);
				pointF.Y = (float)(pointF.Y - labelStyle.Margin.Top / 2);
			}
			else if (labelAngle == 90)
			{
				pointF.X = (float)(pointF.X + labelStyle.Margin.Left / 2);
				pointF.Y = (float)(pointF.Y + labelStyle.Margin.Top);
			}
			else if (labelAngle == 180)
			{
				pointF.X = (float)(pointF.X - labelWidth / 2 + labelStyle.Margin.Left);
				pointF.Y = (float)(pointF.Y - labelStyle.Margin.Top / 2);
			}
			else if (labelAngle == 270)
			{
				pointF.X = (float)(pointF.X + labelStyle.Margin.Left / 2);
				pointF.Y = (float)(pointF.Y - labelHeight / 2 + labelStyle.Margin.Top);
			}
			else if (IsBetween(labelAngle, 1, 89))
			{
				pointF.X = (float)(pointF.X + labelWidth / 2 - labelStyle.Margin.Left);
				pointF.Y = (float)(pointF.Y + labelStyle.Margin.Top / 2);
			}
			else if (IsBetween(labelAngle, 91, 179))
			{
				pointF.X = (float)(pointF.X - labelWidth / 2 + labelStyle.Margin.Right);
				pointF.Y = (float)(pointF.Y + labelStyle.Margin.Top / 2);
			}
			else if (IsBetween(labelAngle, 181, 269))
			{
				pointF.X = (float)(pointF.X - labelWidth / 2 + labelStyle.Margin.Left);
				pointF.Y = (float)(pointF.Y - labelStyle.Margin.Top);
			}
			else if (IsBetween(labelAngle, 271, 359))
			{
				pointF.X = (float)(pointF.X + labelWidth / 2 - labelStyle.Margin.Left);
				pointF.Y = (float)(pointF.Y - labelStyle.Margin.Top);
			}

			return new PointF(pointF.X, pointF.Y);
		}

		static bool IsBetween(float x, float lower, float upper)
		{
			return lower <= x && x <= upper;
		}

		void DrawMajorTick(ICanvas canvas, ChartAxis primaryAxis, float totalSpokes)
		{
			if (_yAxis == null || totalSpokes == 0)
			{
				return;
			}

			float angle = 360 / totalSpokes;
			double radius = _yAxis.ComputedDesiredSize.Height;
			ChartAxisTickStyle tickStyle = primaryAxis.MajorTickStyle;
			double radius1 = radius + tickStyle.TickSize;
			for (int i = 0; i < totalSpokes; i++)
			{
				PointF start = _chartArea.PolarAngleToPoint(primaryAxis, (float)radius, i * angle);
				PointF end = _chartArea.PolarAngleToPoint(primaryAxis, (float)radius1, i * angle);
				canvas.StrokeSize = (float)tickStyle.StrokeWidth;
				canvas.StrokeColor = tickStyle.Stroke.ToColor();
				canvas.DrawLine(start.X, start.Y, end.X, end.Y);
			}
		}

		#endregion
	}
}