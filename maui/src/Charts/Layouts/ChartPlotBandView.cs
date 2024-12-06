using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.Charts
{
	internal partial class ChartPlotBandView : SfDrawableView
	{
		#region Fields

		readonly CartesianChartArea _area;

		#endregion

		#region Constructor

		internal ChartPlotBandView(CartesianChartArea cartesianChartArea)
		{
			_area = cartesianChartArea;
		}

		#endregion

		#region Methods

		#region Protected Methods

		protected override void OnDraw(ICanvas canvas, RectF dirtyRect)
		{
			canvas.CanvasSaveState();
			canvas.ClipRectangle(dirtyRect);

			ChartPlotBandView.DrawPlotBandsForAxes(_area._xAxes, canvas, dirtyRect);
			ChartPlotBandView.DrawPlotBandsForAxes(_area._yAxes, canvas, dirtyRect);

			canvas.CanvasRestoreState();
		}

		#endregion

		#region Private Methods

		static void DrawPlotBandsForAxes(IEnumerable<ChartAxis> axes, ICanvas canvas, RectF dirtyRect)
		{
			foreach (var axis in axes)
			{
				if (axis.ActualPlotBands != null && axis.ActualPlotBands.Count > 0)
				{
					foreach (ChartPlotBand plotBand in axis.ActualPlotBands)
					{
						if (plotBand.IsVisible)
						{
							plotBand.DrawPlotBands(canvas, dirtyRect);
						}
					}
				}
			}
		}

		#endregion

		#endregion
	}
}