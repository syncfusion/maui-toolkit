using MauiView = Microsoft.Maui.Controls.View;

namespace Syncfusion.Maui.Toolkit.Popup
{
	/// <summary>
	/// Represents the <see cref="PopupExtension"/> class.
	/// </summary>
	internal static partial class PopupExtension
	{
		#region Internal Methods

		/// <summary>
		/// Gets the X coordinate of the view.
		/// </summary>
		/// <param name="view">The view.</param>
		/// <returns>Returns the X coordinate of the view.</returns>
		internal static int GetX(this View view) => 0;

		/// <summary>
		/// Gets the Y coordinate of the view.
		/// </summary>
		/// <param name="view">The view.</param>
		/// <returns>Returns the Y coordinate of the view.</returns>
		internal static int GetY(this View view) => 0;

		/// <summary>
		/// Gets the width of the device.
		/// </summary>
		/// <returns>The width of the device.</returns>
		internal static int GetScreenWidth() => 0;

		/// <summary>
		/// Gets the height of the device.
		/// </summary>
		/// <returns>The height of the device.</returns>
		internal static int GetScreenHeight() => 0;

		/// <summary>
		/// Gets action bar height.
		/// </summary>
		/// <returns>Returns action bar height.</returns>
		internal static int GetActionBarHeight(bool ignoreActionBar) => 0;

		/// <summary>
		/// Gets the status bar height.
		/// </summary>
		/// <returns>Returns the status bar height.</returns>
		internal static int GetStatusBarHeight() => 0;

		/// <summary>
		/// Used to applied the blur effect for popup.
		/// </summary>
		/// <param name="view">The instance of the MauiView.</param>
		/// <param name="popup">The instance of the SfPopup.</param>
		/// <param name="isopen">Specifies whether the popup is open or not.</param>
		internal static void Blur(this View view, SfPopup popup, bool isopen)
		{
		}

		/// <summary>
		/// Used to clear the blur effect for popup.
		/// </summary>
		/// <param name="popup">The instance of the SfPopup.</param>
		internal static void ClearBlurViews(SfPopup popup)
		{
		}

		/// <summary>
		/// Calculates the X and Y point of the popup, relative to the given view.
		/// </summary>
		/// <param name="popupView">popup view to display in the view.</param>
		/// <param name="relative">Positions the popup view relatively to the relative view.</param>
		/// <param name="position">The relative position from the view.</param>
		/// <param name="absoluteX">Absolute X Point where the popup should be positioned from the relative view.</param>
		/// <param name="absoluteY">Absolute Y-Point where the popup should be positioned from the relative view.</param>
		/// <param name="relativeX">References the X position of popup relative to view.</param>
		/// <param name="relativeY">References the Y position of popup relative to view.</param>
		internal static void CalculateRelativePoint(PopupView popupView, MauiView relative, PopupRelativePosition position, double absoluteX, double absoluteY, ref double relativeX, ref double relativeY)
		{
		}

		#endregion
	}
}