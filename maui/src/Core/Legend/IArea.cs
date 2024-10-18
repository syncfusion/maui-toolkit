using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Toolkit.Internals
{
	/// <summary>
	/// Defines the properties and methods for an area, including layout bounds, relayout requirements, and plot area management.
	/// </summary>
	internal interface IArea
	{
		/// <summary>
		/// Gets or sets the bounds of the area.
		/// </summary>
		Rect AreaBounds { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the layout needs to be updated.
		/// </summary>
		bool NeedsRelayout { get; set; }

		/// <summary>
		/// Gets the plot area associated with this area.
		/// </summary>
		IPlotArea PlotArea { get; }

		/// <summary>
		/// Schedules an update for the area, ensuring that any necessary changes are applied.
		/// </summary>
		void ScheduleUpdateArea();
	}

}
