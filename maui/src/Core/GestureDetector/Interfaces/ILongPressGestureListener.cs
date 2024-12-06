namespace Syncfusion.Maui.Toolkit.Internals
{
	/// <summary>
	/// Defines a listener for long press gesture events.
	/// </summary>
	public interface ILongPressGestureListener : IGestureListener
	{
		#region Methods

		/// <summary>
		/// Called when a long press gesture is detected.
		/// </summary>
		/// <param name="e">Contains information about the long press event, such as the position.</param>
		void OnLongPress(LongPressEventArgs e);

		#endregion
	}
}
