namespace Syncfusion.Maui.Toolkit.Internals
{
	/// <summary>
	/// Defines a listener for pinch gesture events.
	/// </summary>
	public interface IPinchGestureListener : IGestureListener
	{
		#region Methods

		/// <summary>
		/// Invoked when a pinch gesture is detected.
		/// </summary>
		/// <param name="e">Contains information about the pinch gesture event.</param>
		void OnPinch(PinchEventArgs e);

		#endregion
	}
}
