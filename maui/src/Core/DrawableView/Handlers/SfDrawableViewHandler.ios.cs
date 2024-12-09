using Microsoft.Maui.Graphics.Platform;
using Microsoft.Maui.Handlers;
using Syncfusion.Maui.Toolkit.Platform;
using UIKit;

namespace Syncfusion.Maui.Toolkit.Graphics.Internals
{
	/// <summary>
	/// Handles the drawing view functionality for the SfDrawableView on the ios and mac platform.
	/// </summary>
	public partial class SfDrawableViewHandler : ViewHandler<IDrawableView, PlatformGraphicsView>
	{
		#region Methods

		/// <summary>
		/// Creates a new instance of the platform-specific graphics view.
		/// </summary>
		/// <exclude/>
		/// <returns>The ios platform graphics view.</returns>
		protected override PlatformGraphicsView CreatePlatformView()
		{
			return new PlatformGraphicsViewExt(VirtualView) { BackgroundColor = UIColor.Clear };
		}

		/// <summary>
		/// Invalidates the view, causing it to be redrawn.
		/// </summary>
		public void Invalidate()
		{
			PlatformView?.InvalidateDrawable();
		}

		#endregion
	}
}
