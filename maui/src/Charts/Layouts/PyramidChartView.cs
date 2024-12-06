using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.Charts
{
	internal partial class PyramidChartView : SfDrawableView
	{
		#region Field

		internal readonly IPyramidChartDependent _chart;

		#endregion

		#region Constructor

		internal PyramidChartView(IPyramidChartDependent charts)
		{
			_chart = charts;
		}

		#endregion

		#region Override method

		protected override void OnDraw(ICanvas canvas, RectF dirtyRect)
		{
			canvas.CanvasSaveState();
			canvas.ClipRectangle(dirtyRect);
			var bounds = _chart.SeriesBounds;
			canvas.Translate((float)bounds.X, (float)bounds.Y);

			foreach (var segment in _chart.Segments)
			{
				segment.Draw(canvas);
			}

			canvas.CanvasRestoreState();
		}

		#endregion
	}
}
