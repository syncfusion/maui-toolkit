using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.Charts.Chart.Layouts
{
	internal partial class ChartTrackballView : SfView
	{
		#region Properties

		internal ChartTrackballBehavior? Behavior { get; set; }

		#endregion

		#region Constructor

		public ChartTrackballView()
		{
			DrawingOrder = DrawingOrder.BelowContent;
		}

		#endregion

		#region Methods

		#region Protected Methods

		protected override void OnDraw(ICanvas canvas, RectF dirtyRect)
		{
			if (Behavior?.PointInfos.Count > 0)
			{
				canvas.CanvasSaveState();
				Behavior.DrawElements(canvas, dirtyRect);
				canvas.CanvasRestoreState();
			}
		}

		#endregion

		#endregion
	}
}
