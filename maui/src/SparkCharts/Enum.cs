namespace Syncfusion.Maui.Toolkit.SparkCharts
{
	/// <summary>
	/// Defines the rendering mode for empty or invalid data points in a Sparkline.
	/// </summary>
	internal enum SparkChartEmptyPointMode
	{
		/// <summary>
		/// Renders the empty point as the average of the neighboring points.
		/// </summary>
		Average,
		/// <summary>
		/// Renders the empty point with a value of zero.
		/// </summary>
		Zero,
		/// <summary>
		/// Renders the empty point as a gap, breaking the line or chart.
		/// </summary>
		None
	}

	/// <summary>
	/// Defines the shape of the spark chart markers.
	/// </summary>
	public enum SparkChartMarkerShape
	{
		/// <summary>
		/// Renders a circle shape marker.
		/// </summary>
		Circle,
		/// <summary>
		/// Renders a cross shape marker.
		/// </summary>
		Cross,
		/// <summary>
		/// Renders a diamond shape marker.
		/// </summary>
		Diamond,
		/// <summary>
		/// Renders a hexagon shape marker.
		/// </summary>
		Hexagon,
		/// <summary>
		/// Renders a horizontal line shape marker.
		/// </summary>
		HorizontalLine,
		/// <summary>
		/// Renders an inverted triangle shape marker.
		/// </summary>
		InvertedTriangle,
		/// <summary>
		/// Renders a pentagon shape marker.
		/// </summary>
		Pentagon,
		/// <summary>
		/// Renders a plus shape marker.
		/// </summary>
		Plus,
		/// <summary>
		/// Renders a rectangle shape marker.
		/// </summary>
		Rectangle,
		/// <summary>
		/// Renders a triangle shape marker.
		/// </summary>
		Triangle,
		/// <summary>
		/// Renders a vertical line shape marker.
		/// </summary>
		VerticalLine
	}
}
