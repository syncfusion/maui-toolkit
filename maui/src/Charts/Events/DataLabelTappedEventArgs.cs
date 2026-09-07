namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// Provides data for the <see cref="SfCartesianChart.DataLabelTapped"/> event.
	/// </summary>
	/// <seealso cref="SfCartesianChart.DataLabelTapped"/>
	/// <seealso cref="ChartSeries"/>
	/// <seealso cref="ChartSegment"/>
	public class DataLabelTappedEventArgs : EventArgs
	{
		/// <summary>
		/// Gets the series containing the tapped data label.
		/// </summary>
		public ChartSeries Series { get; }

		/// <summary>
		/// Gets the zero-based index of the data point in the series.
		/// </summary>
		public int DataIndex { get; }

		/// <summary>
		/// Gets the original data object from ItemsSource.
		/// </summary>
		public object? DataItem { get; }

		/// <summary>
		/// Gets the screen coordinates (X, Y) of the label.
		/// </summary>
		/// <remarks>
		/// <para>
		/// These coordinates are relative to the chart area and represent where the user
		/// tapped on the screen. This value is in device-independent units (DIPs).
		/// </para>
		/// </remarks>
		/// <value>A <see cref="PointF"/> representing the screen coordinates of the tap.</value>
		public PointF Position { get; }

		/// <summary>
		/// Gets the segment/bar/point associated with the label.
		/// </summary>
		public ChartSegment? Segment { get; }

		/// <summary>
		/// Initializes a new instance of the <see cref="DataLabelTappedEventArgs"/> class.
		/// </summary>
		/// <param name="series">The series containing the tapped data label.</param>
		/// <param name="dataIndex">Index of the data point in the series.</param>
		/// <param name="dataItem">Original data object from ItemsSource.</param>
		/// <param name="position">Screen coordinates (X, Y) of the label.</param>
		/// <param name="segment">The segment/bar/point associated with label.</param>
		public DataLabelTappedEventArgs(ChartSeries series, int dataIndex, object? dataItem, PointF position, ChartSegment? segment = null)
		{
			Series = series;

			if (dataIndex < 0)
				return;

			DataIndex = dataIndex;
			DataItem = dataItem;
			Position = position;
			Segment = segment;
		}
	}
}
