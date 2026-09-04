namespace Syncfusion.Maui.Toolkit.InteractiveViewer
{
	/// <summary>
	/// Represents a static helper class that contains utility methods related to the interactive viewer.
	/// </summary>
	internal static class InteractiveViewerHelper
	{
		#region Internal Methods

		/// <summary>
		/// Gets the appropriate scroll orientation based on the pan enabled state and pan axis.
		/// </summary>
		/// <param name="isPanEnabled">A value indicating whether panning is enabled.</param>
		/// <param name="panAxis">The axis along which panning is allowed.</param>
		/// <returns>
		/// The corresponding <see cref="ScrollOrientation"/> value based on the pan settings;
		/// otherwise, <see cref="ScrollOrientation.Neither"/> when panning is disabled.
		/// </returns>
		internal static ScrollOrientation GetScrollOrientation(bool isPanEnabled, PanAxis panAxis)
		{
			return isPanEnabled
				? panAxis switch
				{
					PanAxis.Horizontal => ScrollOrientation.Horizontal,
					PanAxis.Vertical => ScrollOrientation.Vertical,
					_ => ScrollOrientation.Both
				}
				: ScrollOrientation.Neither;
		}

		/// <summary>
		/// Gets the next rotation angle in the clockwise direction.
		/// </summary>
		/// <param name="currentRotation">The current rotation angle in degrees.</param>
		/// <returns>The next rotation angle in degrees.</returns>
		internal static double GetRotationAngle(double currentRotation)
		{
			return (currentRotation + 90) % 360;
		}

		#endregion
	}
}