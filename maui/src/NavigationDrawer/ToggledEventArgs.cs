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
	}
}
