using Microsoft.Maui.Graphics.Win2D;
using Microsoft.Maui.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.Maui.Toolkit.Graphics.Internals
{
	/// <summary>
	/// Handles the drawing view functionality for the SfDrawableView on the Windows platform.
	/// </summary>
	public partial class SfDrawableViewHandler : ViewHandler<IDrawableView, W2DGraphicsView>
    {
		#region Methods

		/// <summary>
		/// Creates the platform-specific W2DGraphicsView.
		/// </summary>
		/// <returns>The platform-specific <see cref="W2DGraphicsView"/> object.</returns>
		protected override W2DGraphicsView CreatePlatformView()
        {
            var nativeGraphicsView = new W2DGraphicsView();
            nativeGraphicsView.Drawable = VirtualView;
            nativeGraphicsView.UseSystemFocusVisuals = true;
            nativeGraphicsView.IsTabStop = true;
            return nativeGraphicsView;
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
