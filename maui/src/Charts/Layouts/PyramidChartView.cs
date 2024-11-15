using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.Charts
{
	internal class PyramidChartView : SfDrawableView
    {
        #region Field

        internal readonly IPyramidChartDependent chart;

        #endregion

        #region Constructor

        internal PyramidChartView(IPyramidChartDependent charts)
        {
            chart = charts;
        }

        #endregion

        #region Override method

        protected override void OnDraw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.CanvasSaveState();
            canvas.ClipRectangle(dirtyRect);
            var bounds = chart.SeriesBounds;
            canvas.Translate((float)bounds.X, (float)bounds.Y);

            foreach (var segment in chart.Segments)
            {
                segment.Draw(canvas);
            }

            canvas.CanvasRestoreState();
        }

        #endregion
    }
}
