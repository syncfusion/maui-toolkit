using System.Collections.Generic;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.Charts
{
	internal class ChartPlotBandView : SfDrawableView
    {
        #region Fields

        CartesianChartArea _area;

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

            DrawPlotBandsForAxes(_area.XAxes, canvas, dirtyRect);
            DrawPlotBandsForAxes(_area.YAxes, canvas, dirtyRect);

            canvas.CanvasRestoreState();
        }

        #endregion

        #region Private Methods

        void DrawPlotBandsForAxes(IEnumerable<ChartAxis> axes, ICanvas canvas, RectF dirtyRect)
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