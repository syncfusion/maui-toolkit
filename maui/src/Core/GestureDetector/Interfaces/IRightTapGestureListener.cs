namespace Syncfusion.Maui.Toolkit.Internals
{
	/// <summary>
	/// Defines a listener for right tap gesture events.
	/// </summary>
	public interface IRightTapGestureListener : IGestureListener
	{
		#region Methods

		/// <summary>
		/// Called when a right tap gesture is detected.
		/// </summary>
		/// <param name="e">The event arguments containing information about the right tap gesture.</param>
		void OnRightTap(RightTapEventArgs e);

		/// <summary>
		/// Called when a right tap gesture is detected, providing the sender object.
		/// </summary>
		/// <param name="sender">The object that raised the event.</param>
		/// <param name="e">The event arguments containing information about the right tap gesture.</param>
		void OnRightTap(object sender, RightTapEventArgs e)
		{
			// The empty implementation in the interface allows for optional implementation in derived classes.
		}

		#endregion
	}
}
