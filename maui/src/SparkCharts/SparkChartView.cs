using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.SparkCharts
{
	#region Internal class
	internal class SparkChartView : SfDrawableView
	{
		readonly SfSparkChart chart;

		public SparkChartView(SfSparkChart sparkChart)
		{
			chart = sparkChart;
		}

		protected override void OnDraw(ICanvas canvas, RectF dirtyRect)
		{
			chart.OnDraw(canvas, dirtyRect);
		}
	}
	#endregion

}