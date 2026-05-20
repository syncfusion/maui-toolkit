using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.SparkCharts
{
	/// <summary>
	/// Provides a drawable view layer for rendering trackball elements in SparkCharts.
	/// </summary>
	internal class SparkChartTrackballView : SfView
	{
		#region Fields

		readonly SfSparkChart _chart;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="SparkChartTrackballView"/> class.
		/// </summary>
		/// <param name="chart">The parent SparkChart instance.</param>
		public SparkChartTrackballView(SfSparkChart chart)
		{
			_chart = chart;
			DrawingOrder = DrawingOrder.BelowContent;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Draws the trackball elements on the canvas.
		/// </summary>
		protected override void OnDraw(ICanvas canvas, RectF dirtyRect)
		{
			if (_chart.TrackballBehavior != null && _chart.TrackballBehavior.IsVisible)
			{
				_chart.TrackballBehavior.DrawElements(canvas, dirtyRect);
			}
		}

		#endregion
	}
}
