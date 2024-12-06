namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// Provides data for the chart axis <see cref="ChartAxis.LabelCreated"/> event.
	/// </summary>
	/// <remarks>This class contains information about the text, position, and label style of the chart axis label.</remarks>
	public class ChartAxisLabelEventArgs : EventArgs
	{
		#region Public Properties

		/// <summary>
		/// Gets or sets a string value that displays on the chart axis label.
		/// </summary>
		public string? Label { get; set; }

		/// <summary>
		/// Gets a value for the chart axis label's position.
		/// </summary>
		public double Position { get; internal set; }

		/// <summary>
		/// Gets or sets the value to customize the appearance of chart axis labels. 
		/// </summary>
		/// <value>It accepts the <see cref="Charts.ChartAxisLabelStyle"/> value.</value>
		public ChartAxisLabelStyle? LabelStyle { get; set; }

		#endregion

		#region Constructor

		/// <summary>
		///  Initializes a new instance of the <see cref="ChartAxisLabelEventArgs"/> class.
		/// </summary>
		public ChartAxisLabelEventArgs(string? labelContent, double position)
		{
			Label = labelContent;
			Position = position;
		}

		#endregion
	}
}