namespace Syncfusion.Maui.Toolkit.SparkCharts
{
	/// <summary>
	/// Provides event data for the <see cref="SfSparkChart.TrackballCreated"/> event, which is triggered when the trackball is created in the spark chart.
	/// </summary>
	internal class SparkChartTrackballEventArgs : EventArgs
	{
		#region Public Property 

		/// <summary>
		/// Gets the trackball information that is created when the trackball is active.
		/// This information is updated whenever the trackball moves across the chart, reflecting the current data point at the trackball's position.
		/// </summary>
		public SparkChartTrackballInfo TrackballPointInfo { get; internal set; }

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="SparkChartTrackballEventArgs"/> class.
		/// </summary>
		/// <exclude/>
		public SparkChartTrackballEventArgs(SparkChartTrackballInfo pointInfo)
		{
			TrackballPointInfo = pointInfo;
		}

		#endregion
	}
}
