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