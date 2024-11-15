using Microsoft.Maui.Graphics.Platform;
using Microsoft.Maui.Handlers;

namespace Syncfusion.Maui.Toolkit.Graphics.Internals
{
	/// <summary>
	/// Handles the drawing view functionality for the SfDrawableView on the Android platform.
	/// </summary>
	public partial class SfDrawableViewHandler : ViewHandler<IDrawableView, PlatformGraphicsView>
    {
		#region Methods

		/// <summary>
		/// Creates a new instance of the platform-specific graphics view.
		/// </summary>
		/// <exclude/>
		/// <returns>The Android platform graphics view.</returns>
		protected override PlatformGraphicsView CreatePlatformView()
        {
            return new PlatformGraphicsView(Context, VirtualView);
        }

		/// <summary>
		/// Invalidates the view, causing it to be redrawn.
		/// </summary>
		public void Invalidate()
        {
            this.PlatformView?.Invalidate();
        }

        #endregion
    }
}
