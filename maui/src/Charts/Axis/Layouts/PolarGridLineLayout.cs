using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.Charts
{
	internal partial class PolarGridLineLayout : SfDrawableView
	{
		#region Fields

		Size _desiredSize;
		float _totalSpokes;
		readonly PolarChartArea _area;
		ChartAxis? _xAxis => _area.GetPrimaryAxis();
		ChartAxis? _yAxis => _area.GetSecondaryAxis();
		bool _isRadar
		{
			get
			{
				if (_area != null)
				{
					return (_area._polarChart.GridLineType == PolarChartGridLineType.Polygon);
				}

				return false;
			}
		}

		#endregion

		#region Constructor

		public PolarGridLineLayout(PolarChartArea polarArea)
		{
			_area = polarArea;
		}

		#endregion

		#region Methods

		void OnDraw(ICanvas canvas)
		{
			if (_area != null)
			{
				canvas.CanvasSaveState();
				canvas.Translate(0, 0);
				if (_xAxis != null)
				{
					_totalSpokes = _xAxis.VisibleLabels.Count;
					DrawPrimaryAxisGridLine(_xAxis, canvas);
				}

				if (_yAxis != null)
				{
					DrawSecondaryAxisGridLine(_yAxis, canvas);
				}

				canvas.CanvasRestoreState();
			}
		}

		public SizeF Measure(SizeF availableSize)
		{
			_desiredSize = new SizeF(availableSize.Width, availableSize.Height);
			return availableSize;
		}


		#region Protected Method

		override protected void OnDraw(ICanvas canvas, RectF dirtyRect)
		{
			base.OnDraw(canvas, dirtyRect);
			OnDraw(canvas);
		}

		#endregion

		#region Private Methods

		void DrawPrimaryAxisGridLine(ChartAxis axis, ICanvas canvas)
		{
			var gridLineStyle = axis.MajorGridLineStyle ?? PolarGridLineLayout.GetDefaultGridLineStyle();
			if (!axis.CanDrawMajorGridLines())
			{
				return;
			}
			else
			{
				canvas.StrokeSize = (float)gridLineStyle.StrokeWidth;
				canvas.StrokeColor = gridLineStyle.Stroke.ToColor();
				if (gridLineStyle.StrokeDashArray != null)
				{
					canvas.StrokeDashPattern = gridLineStyle.StrokeDashArray.ToFloatArray();
				}
			}

			float angle = 360 / _totalSpokes;
			float radius = float.NaN;
			if (_yAxis != null)
			{
				radius = (float)_yAxis.ComputedDesiredSize.Height;
			}

			for (int i = 0; i < _totalSpokes; i++)
			{
				Point pointF = _area.PolarAngleToPoint(axis, radius, i * angle);
				canvas.DrawLine((float)(_desiredSize.Width / 2), (float)(_desiredSize.Height / 2), (float)pointF.X, (float)pointF.Y);
			}
		}

		void DrawSecondaryAxisGridLine(ChartAxis axis, ICanvas canvas)
		{
			List<double> tickPositions = axis.TickPositions;
			if (tickPositions.Count > 0)
			{
				float top = (float)(axis.RenderedRect.Top - _area.PlotAreaMargin.Top);
				float bottom = (float)(axis.RenderedRect.Bottom - _area.PlotAreaMargin.Top);
				float height = bottom - top;
				var gridLineStyle = axis.MajorGridLineStyle ?? PolarGridLineLayout.GetDefaultGridLineStyle();

				if (axis.CanDrawMajorGridLines())
				{
					foreach (var position in tickPositions)
					{
						float value = axis.ValueToCoefficient(position);
						float y = height * (1f - value);
						if (_area.Series?.Count > 0 && !(_isRadar))
						{
							PolarGridLineLayout.DrawPolarAxisGridLine(canvas, (float)(_desiredSize.Width / 2), (float)(_desiredSize.Height / 2), y, gridLineStyle);
						}
						else
						{
							DrawRadarAxisGridLine(canvas, axis, y, gridLineStyle);
						}
					}
				}

				if (axis.SmallTickRequired && axis is RangeAxisBase rangeAxisBase && rangeAxisBase.CanDrawMinorGridLines())
				{
					List<double> smallTicks = rangeAxisBase.SmallTickPoints;
					var minorGridLineStyle = rangeAxisBase.MinorGridLineStyle ?? PolarGridLineLayout.GetDefaultGridLineStyle();
					foreach (var pos in smallTicks)
					{
						float value = axis.ValueToCoefficient(pos);
						float y = height * (1f - value);
						if (_area.Series?.Count > 0 && !(_isRadar))
						{
							PolarGridLineLayout.DrawPolarAxisGridLine(canvas, (float)(_desiredSize.Width / 2), (float)(_desiredSize.Height / 2), y, minorGridLineStyle);
						}
						else
						{
							DrawRadarAxisGridLine(canvas, axis, y, minorGridLineStyle);
						}
					}
				}
			}
		}

		static void DrawPolarAxisGridLine(ICanvas canvas, float centerX, float centerY, float radius, ChartLineStyle gridLineStyle)
		{
			var x = centerX - radius;
			var y = centerY - radius;
			var size = radius * 2;

			if (gridLineStyle.CanDraw() && gridLineStyle.Stroke != null)
			{
				canvas.StrokeSize = (float)gridLineStyle.StrokeWidth;
				canvas.StrokeColor = gridLineStyle.Stroke.ToColor();
				if (gridLineStyle.StrokeDashArray != null)
				{
					canvas.StrokeDashPattern = gridLineStyle.StrokeDashArray.ToFloatArray();
				}
			}

			canvas.DrawEllipse(x, y, size, size);
		}

		void DrawRadarAxisGridLine(ICanvas canvas, ChartAxis axis, float y, ChartLineStyle gridLineStyle)
		{
			float angleTrack;
			int j = 1, totalCircle;
			var chart = _area._polarChart;
			if (chart.PrimaryAxis is CategoryAxis || chart.PrimaryAxis is DateTimeAxis)
			{
				if (_area.VisibleSeries?.Count == 0 && chart.Series.Count > 0)
				{
					totalCircle = chart.Series[0].PointsCount;
				}
				else
				{
					totalCircle = chart.PrimaryAxis.VisibleLabels.Count;
				}
			}
			else
			{
				totalCircle = chart.PrimaryAxis.VisibleLabels.Count - 1;
			}

			if (totalCircle == 0)
			{
				totalCircle = 1;
			}

			angleTrack = 360 / totalCircle;
			PathF radarPath = new PathF();

			Point startPointF = _area.PolarAngleToPoint(axis, y, 0);
			radarPath.MoveTo((float)startPointF.X, (float)startPointF.Y);

			while (j < totalCircle)
			{
				Point midPointF = _area.PolarAngleToPoint(axis, y, j * angleTrack);
				radarPath.LineTo((float)midPointF.X, (float)midPointF.Y);
				j++;
			}

			if (gridLineStyle.CanDraw())
			{
				canvas.StrokeSize = (float)gridLineStyle.StrokeWidth;
				canvas.StrokeColor = gridLineStyle.Stroke.ToColor();
				if (gridLineStyle.StrokeDashArray != null)
				{
					canvas.StrokeDashPattern = gridLineStyle.StrokeDashArray.ToFloatArray();
				}
			}

			radarPath.Close();
			canvas.DrawPath(radarPath);
		}

		static ChartLineStyle GetDefaultGridLineStyle()
		{
			return new ChartLineStyle() { Stroke = new SolidColorBrush(Color.FromArgb("#E7E0EC")), StrokeWidth = 1 };
		}

		#endregion

		#endregion
	}
}