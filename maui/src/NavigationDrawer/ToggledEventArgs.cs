namespace Syncfusion.Maui.Toolkit.NavigationDrawer
{
	using System;

	/// <summary>
	/// Represents custom EventArgs for toggle-related events, extending <see cref="EventArgs"/>.
	/// </summary>
	public class ToggledEventArgs : EventArgs
	{
		/// <summary>
		/// Gets or sets a value indicating whether the navigation drawer is open.
		/// </summary>
		public bool IsOpen { get; internal set; }

		/// <summary>
		/// Gets or sets a value indicating whether the event is raised for the primary drawer.
		/// True if the event is for the primary drawer; false for the secondary drawer.
		/// </summary>
		public bool IsPrimary { get; internal set; }
	}
}
