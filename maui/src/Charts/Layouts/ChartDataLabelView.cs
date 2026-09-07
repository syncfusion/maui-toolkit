using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.Charts
{
	internal partial class DataLabelView : SfDrawableView
	{
		#region Fields

		readonly ChartPlotArea _chartPlotArea;

		#endregion

		#region Constructor

		public DataLabelView(ChartPlotArea plotArea)
		{
			_chartPlotArea = plotArea;
		}

		#endregion

		#region Methods

		#region Protected Methods

		protected override void OnDraw(ICanvas canvas, RectF dirtyRect)
		{
			var visibleSeries = _chartPlotArea.VisibleSeries;

			canvas.CanvasSaveState();
			canvas.ClipRectangle(dirtyRect);

			if (visibleSeries != null)
			{
				foreach (var series in visibleSeries)
				{
					if (series.IsVisible && !series.CanAnimate() && series.ShowDataLabels && series._segments.Count > 0 && series.LabelTemplate == null)
					{
						canvas.CanvasSaveState();

						if (series.NeedToAnimateDataLabel)
						{
							canvas.Alpha = series.AnimationValue;
						}

						series.DrawDataLabels(canvas);
						RegisterDataLabelRegions(series);
						canvas.CanvasRestoreState();
					}

					if (series is CircularSeries circularSeries && series.ShowDataLabels && series._segments.Count > 0 && circularSeries.LabelTemplate != null)
					{
						circularSeries.UpdateDataLabelPositions(canvas);
					}
				}
			}

			canvas.CanvasRestoreState();
		}

		#region Private methods

		/// <summary>
		/// Registers data label regions for hit detection
		/// </summary>
		void RegisterDataLabelRegions(ChartSeries series)
		{
			if (series._segments == null)
				return;

			for (int i = 0; i < series._segments.Count; i++)
			{
				var segment = series._segments[i];

				// Get label bounds from segment
				if (segment is ChartSegment chartSegment)
				{
					// Register each data label region
					segment.LabelBounds = GetDataLabelBounds(chartSegment);
				}
			}
		}

		/// <summary>
		/// Gets the bounding rectangle of a data label
		/// </summary>
		RectF GetDataLabelBounds(ChartSegment segment)
		{
			// For other series types, calculate from label position and size
			var labelContent = segment.LabelContent?.ToString() ?? string.Empty;
			if (string.IsNullOrEmpty(labelContent))
				return RectF.Zero;

			// Estimate label size - can be improved with actual measurement
			float estimatedWidth = labelContent.Length * 6f;
			float estimatedHeight = 16f;

			PointF labelPos = segment.LabelPositionPoint;

			return new RectF(
				labelPos.X - (estimatedWidth / 2),
				labelPos.Y - (estimatedHeight / 2),
				estimatedWidth,
				estimatedHeight
			);
		}

		#endregion

		#endregion

		#endregion
	}

	internal partial class PyramidDataLabelView : SfDrawableView
	{
		#region Fields

		readonly IPyramidChartDependent _chart;

		#endregion

		#region Constructor

		public PyramidDataLabelView(IPyramidChartDependent chart)
		{
			_chart = chart;
		}

		#endregion

		#region Methods

		#region Protected Methods

		protected override void OnDraw(ICanvas canvas, RectF dirtyRect)
		{
			canvas.CanvasSaveState();
			_chart.DrawDataLabels(canvas, dirtyRect);
			canvas.CanvasRestoreState();
		}

		#endregion

		#endregion
	}

	internal partial class PolarDataLabelView : SfDrawableView
	{
		#region Fields

		readonly PolarChartArea _chartArea;

		#endregion

		#region Constructor

		public PolarDataLabelView(PolarChartArea chartArea)
		{
			_chartArea = chartArea;
		}

		#endregion

		#region Methods

		#region Protected Methods

		protected override void OnDraw(ICanvas canvas, RectF dirtyRect)
		{
			var visibleSeries = _chartArea.VisibleSeries;

			canvas.CanvasSaveState();
			canvas.ClipRectangle(dirtyRect);

			if (visibleSeries != null)
			{
				foreach (var series in visibleSeries)
				{
					if (series.IsVisible && !series.CanAnimate() && series.ShowDataLabels && series._segments.Count > 0 && series.LabelTemplate == null)
					{
						canvas.CanvasSaveState();

						if (series.NeedToAnimateDataLabel)
						{
							canvas.Alpha = series.AnimationValue;
						}

						series.DrawDataLabels(canvas);
						canvas.CanvasRestoreState();
					}
				}
			}

			canvas.CanvasRestoreState();
		}

		#endregion

		#endregion
	}
}