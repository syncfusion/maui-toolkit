using Microsoft.Maui.Handlers;

namespace Syncfusion.Maui.Toolkit.Graphics.Internals
{
	/// <summary>
	/// Handles the drawing view functionality for the SfDrawableView.
	/// </summary>
	public partial class SfDrawableViewHandler : ViewHandler<IDrawableView, object>
	{
		/// <summary>
		/// Creates the platform-specific view.
		/// </summary>
		/// <returns>The platform-specific view object.</returns>
		/// <exception cref="NotImplementedException">Thrown when the method is not implemented.</exception>
		protected override object CreatePlatformView()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Invalidates the view, causing it to be redrawn.
		/// </summary>
		public void Invalidate()
		{

		}
	}
}
