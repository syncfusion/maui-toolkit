namespace Syncfusion.Maui.Toolkit.Internals
{
	/// <summary>
	/// Represents the settings for double tap zoom
	/// </summary>
	public class DoubleTapSettings
	{
		/// <summary>
		/// Gets or sets the initial zoom that will be applied, when a double tap occurrs. The default value is 1.
		/// </summary>
		public double DefaultZoomFactor { get; set; } = 1;

		/// <summary>
		/// Gets or sets the percentage of the zoom that has to be increased, when a double tap occurs at the <see cref="Syncfusion.Maui.Toolkit.Internals.DoubleTapSettings.DefaultZoomFactor"/>. 
		/// The default value is 100 and it indicates that the default zoom will be doubled.
		/// </summary>
		public double ZoomDeltaPercent { get; set; } = 100;
	}

	/// <summary>
	/// Represents the settings for mouse wheel zoom.
	/// </summary>
	public class MouseWheelSettings
	{
		/// <summary>
		/// Gets or sets the modifer key to use for mouse wheel zooming. The default value is <see cref="KeyboardKey.None"/> which means that the zoom can be changed using mouse wheel scroll, without using a modifier key.
		/// </summary>
		public KeyboardKey ZoomKeyModifier { get; set; } = KeyboardKey.None;

		/// <summary>
		/// Gets or sets the percent of zoom to be increased or decreased from the current zoom factor on mouse wheel is scrolled. 
		/// </summary>
		/// <value>
		/// The value is expressed as a percentage and the default value is 25. 
		/// If the current zoom factor is 1 and zooming in is performed, the zoom factor will be increased to 1.25.
		/// If the current zoom factor is 1 and zooming out is performed, the zoom factor will be decreased to 0.8.
		/// </value>
		public double ZoomDeltaPercent { get; set; } = 25;
	}
}
