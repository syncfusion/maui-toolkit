namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// Provides data for the <see cref="SfCartesianChart.AxisLabelTapped"/> event.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This class contains information about the axis label that was tapped/clicked by the user.
	/// It includes the axis reference, label details, and the screen position of the tap.
	/// </para>
	/// <para>
	/// <strong>Usage Example:</strong>
	/// <code>
	/// var chart = new SfCartesianChart();
	/// chart.AxisLabelTapped += (sender, args) =>
	/// {
	///     var axis = args.Axis;
	///     var label = args.AxisLabel;
	///     var position = args.Position;
	///     Debug.WriteLine($"Tapped label at position {position}");
	/// };
	/// </code>
	/// </para>
	/// </remarks>
	/// <seealso cref="SfCartesianChart.AxisLabelTapped"/>
	/// <seealso cref="ChartAxis"/>
	/// <seealso cref="ChartAxisLabel"/>
	public class AxisLabelTappedEventArgs : EventArgs
	{
		/// <summary>
		/// Gets the axis instance containing the tapped label.
		/// </summary>
		public ChartAxis Axis { get; }

		/// <summary>
		/// Gets the tapped axis label object.
		/// </summary>
		public ChartAxisLabel AxisLabel { get; }

		/// <summary>
		/// Gets the screen coordinates (X, Y) of the tap.
		/// </summary>
		public PointF Position { get; }

		/// <summary>
		/// Initializes a new instance of the <see cref="AxisLabelTappedEventArgs"/> class.
		/// </summary>
		/// <param name="axis">The axis instance containing the tapped label.</param>
		/// <param name="axisLabel">The tapped axis label object.</param>
		/// <param name="position">The screen coordinates (X, Y) of the tap.</param>
		public AxisLabelTappedEventArgs(ChartAxis axis, ChartAxisLabel axisLabel, PointF position)
		{
			Axis = axis;
			AxisLabel = axisLabel;
			Position = position;
		}
	}
}
