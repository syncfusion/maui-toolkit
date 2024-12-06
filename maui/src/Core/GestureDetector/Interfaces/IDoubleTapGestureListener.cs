namespace Syncfusion.Maui.Toolkit.Internals
{
	/// <summary>
	/// Defines a listener for double tap gesture events.
	/// </summary>
	public interface IDoubleTapGestureListener : IGestureListener
	{
		#region Methods

		/// <summary>
		/// Called when a double tap gesture is detected.
		/// </summary>
		/// <param name="e">The event arguments containing information about the double tap.</param>
		void OnDoubleTap(TapEventArgs e);

		#endregion

	}
}

