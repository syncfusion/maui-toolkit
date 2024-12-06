namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// Represents the content and label style for the axis label.
	/// </summary>
	public class ChartAxisLabel
	{
		#region Fields
		bool _isVisible = true;
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the axis label text.
		/// </summary>
		public object Content { get; set; }

		/// <summary>
		/// Gets the axis label position.
		/// </summary>
		public double Position { get; internal set; }

		internal bool IsVisible
		{
			get { return _isVisible; }
			set { _isVisible = value; }
		}

		/// <summary>
		/// Gets or sets the axis label style to customize the label appearance.
		/// </summary>
		public ChartAxisLabelStyle? LabelStyle { get; set; }

		internal double RotateOriginX { get; set; }

		internal double RotateOriginY { get; set; }

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="ChartAxisLabel"/> class.
		/// </summary>
		public ChartAxisLabel(double position, object labelContent)
		{
			Position = position;
			Content = labelContent;
		}

		#endregion
	}

}
