using MauiView = Microsoft.Maui.Controls.View;

namespace Syncfusion.Maui.Toolkit.Internals
{
	internal partial class SfWindowOverlay
	{
		#region Internal Methods

		/// <summary>
		/// Adds or updates the child layout absolutely to the overlay stack.
		/// </summary>
		/// <param name="child">Adds the child to the floating window.</param>
		/// <param name="x">Positions the child in the x point from the application left.</param>
		/// <param name="y">Positions the child in the y point from the application top.</param>
		/// <param name="horizontalAlignment">The horizontal alignment behaves as like below,
		/// <list type="bullet">
		/// <item><description>For <see cref="WindowOverlayHorizontalAlignment.Left"/>, the child left position will starts from the x.</description></item>
		/// <item><description>For <see cref="WindowOverlayHorizontalAlignment.Right"/>, the child right position will starts from the x.</description></item>
		/// <item><description>For <see cref="WindowOverlayHorizontalAlignment.Center"/>, the child center position will be the x.</description></item>
		/// </list></param>
		/// <param name="verticalAlignment">The vertical alignment behaves as like below,
		/// <list type="bullet">
		/// <item><description>For <see cref="WindowOverlayVerticalAlignment.Top"/>, the child top position will starts from the y.</description></item>
		/// <item><description>For <see cref="WindowOverlayVerticalAlignment.Bottom"/>, the child bottom position will starts from the y.</description></item>
		/// <item><description>For <see cref="WindowOverlayVerticalAlignment.Center"/>, the child center position will be the y.</description></item>
		/// </list></param>
		internal void AddOrUpdate(
			MauiView child,
			double x,
			double y,
			WindowOverlayHorizontalAlignment horizontalAlignment = WindowOverlayHorizontalAlignment.Left,
			WindowOverlayVerticalAlignment verticalAlignment = WindowOverlayVerticalAlignment.Top)
		{
		}

		/// <summary>
		/// Adds or updates the child layout relatively to the overlay stack. After the relative positioning, the x and y will the added
		/// with the left and top positions.
		/// </summary>
		/// <param name="child">Adds the child to the floating window.</param>
		/// <param name="relative">Positions the child relatively to the relative view.</param>
		/// <param name="x">Adds the x point to the child left after the relative positioning.</param>
		/// <param name="y">Adds the y point to the child top after the relative positioning.</param>
		/// <param name="horizontalAlignment">The horizontal alignment behaves as like below,
		/// <list type="bullet">
		/// <item><description>For <see cref="WindowOverlayHorizontalAlignment.Left"/>, the child left position will starts from the relative.Left.</description></item>
		/// <item><description>For <see cref="WindowOverlayHorizontalAlignment.Right"/>, the child right position will starts from the relative.Right.</description></item>
		/// <item><description>For <see cref="WindowOverlayHorizontalAlignment.Center"/>, the child center position will be the relative.Center.</description></item>
		/// </list></param>
		/// <param name="verticalAlignment">The vertical alignment behaves as like below,
		/// <list type="bullet">
		/// <item><description>For <see cref="WindowOverlayVerticalAlignment.Top"/>, the child bottom position will starts from the relative.Top.</description></item>
		/// <item><description>For <see cref="WindowOverlayVerticalAlignment.Bottom"/>, the child top position will starts from the relative.Bottom.</description></item>
		/// <item><description>For <see cref="WindowOverlayVerticalAlignment.Center"/>, the child center position will be the relative.Center.</description></item>
		/// </list></param>
		internal void AddOrUpdate(
			MauiView child,
			MauiView relative,
			double x = 0,
			double y = 0,
			WindowOverlayHorizontalAlignment horizontalAlignment = WindowOverlayHorizontalAlignment.Left,
			WindowOverlayVerticalAlignment verticalAlignment = WindowOverlayVerticalAlignment.Top)
		{
		}

		/// <summary>
		/// Eliminates the view from the floating window.
		/// </summary>
		/// <param name="view">Specifies the view to be removed from the floating window.</param>
		internal void Remove(MauiView view)
		{
		}

		/// <summary>
		/// Removes the currently active overlay from the display.
		/// </summary>
		/// <remarks>This method deactivates and removes any overlay that is currently being displayed. 
		/// If no overlay is active, the method performs no action.</remarks>
		internal void RemoveOverlay()
		{
		}

		/// <summary>
		/// Adds a child view to the current window.
		/// </summary>
		/// <remarks>This method ensures that the child view is added to the window and converted to its
		/// platform-specific representation. If the <paramref name="childView"/> is <see langword="null"/> or the
		/// overlay stack is not initialized, the method exits without performing any action.</remarks>
		/// <param name="childView">The child view to be added. Must not be <see langword="null"/>.</param>
		internal bool AddToOverlay(MauiView childView)
		{
			return false;
		}

		/// <summary>
		/// Positions the popup at the specified coordinates relative to the screen.
		/// </summary>
		/// <remarks>The method ensures that the popup is added to the view hierarchy if it is not already
		/// present.  The coordinates are automatically scaled based on the device's screen density.</remarks>
		/// <param name="x">The horizontal position, in device-independent units (DIPs), where the popup should be placed. Defaults to
		/// 0.</param>
		/// <param name="y">The vertical position, in device-independent units (DIPs), where the popup should be placed. Defaults to 0.</param>
		internal void PositionOverlayContent(double x = 0, double y = 0)
		{
		}

		/// <summary>
		/// Dispose the objects in window overlay.
		/// </summary>
		internal void Dispose()
		{
		}

		/// <summary>
		/// Removes the current overlay window from root view with all its children.
		/// </summary>
		internal void RemoveFromWindow()
		{
		}

		#endregion

		#region Private Methods
		/// <summary>
		/// Creates a overlay stack for adding the Overlay child.
		/// </summary>
		void Initialize()
		{
		}

		#endregion
	}
}