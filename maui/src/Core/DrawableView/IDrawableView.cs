namespace Syncfusion.Maui.Toolkit.Graphics.Internals
{
	/// <summary>
	/// Represents a view that can be drawn and invalidated.
	/// </summary>
	public interface IDrawableView : IView, IDrawable
	{
		/// <summary>
		/// Invalidates the drawable, causing it to be redrawn.
		/// </summary>
		void InvalidateDrawable();
	}
}
