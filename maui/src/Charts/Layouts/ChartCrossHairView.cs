using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.Charts.Chart.Layouts
{
	internal partial class ChartCrosshairView : SfView
	{
		#region Properties

		internal ChartCrosshairBehavior? CrosshairBehavior { get; set; }

		#endregion

		#region Constructor

		public ChartCrosshairView()
		{
			DrawingOrder = DrawingOrder.BelowContent;
		}

		#endregion

		#region Methods

		#region Protected Methods

		protected override void OnDraw(ICanvas canvas, RectF dirtyRect)
		{
			if (CrosshairBehavior != null)
			{
				canvas.SaveState();
				CrosshairBehavior.DrawElements(canvas, dirtyRect);
				canvas.RestoreState();
			}
		}

		#endregion

		#endregion
	}
}
