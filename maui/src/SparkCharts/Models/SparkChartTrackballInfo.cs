namespace Syncfusion.Maui.Toolkit.SparkCharts
{
	/// <summary>
	/// Represents information about a single trackball point in a SparkChart.
	/// </summary>
	internal class SparkChartTrackballInfo
	{
		#region Internal Properties

		/// <summary>
		/// Gets or sets the X coordinate in screen space.
		/// </summary>
		internal float X { get; set; }

		/// <summary>
		/// Gets or sets the Y coordinate in screen space.
		/// </summary>
		internal float Y { get; set; }

		/// <summary>
		/// Gets or sets the index of the data point.
		/// </summary>
		internal int Index { get; set; }

		/// <summary>
		/// Gets or sets the fill brush for the trackball marker.
		/// </summary>
		internal Brush? Fill { get; set; }

		/// <summary>
		/// Gets or sets the X value of the data point.
		/// </summary>
		internal double XValue { get; set; }

		/// <summary>
		/// Gets or sets the Y value of the data point.
		/// </summary>
		internal double YValue { get; set; }

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets the content to be displayed on trackball tooltip.
		/// </summary>
		public string Label { get; set; } = string.Empty;

		/// <summary>
		/// Gets the <see cref="SparkChartTrackballInfo"/> associated business model.
		/// </summary>
		public object? DataItem { get; internal set; }

		/// <summary>
		/// Gets or sets the style for the trackball label.
		/// </summary>
		public SparkChartLabelStyle? LabelStyle { get; set; }

		/// <summary>
		/// Gets or sets the style for the trackball marker.
		/// </summary>
		public SparkChartMarkerSettings? MarkerSettings { get; set; }

		#endregion
	}
}
