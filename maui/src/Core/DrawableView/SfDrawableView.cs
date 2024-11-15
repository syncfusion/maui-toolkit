namespace Syncfusion.Maui.Toolkit.Graphics.Internals
{
    /// <summary>
    /// Represents a view that can be drawn on using native drawing options. 
    /// </summary>
    public class SfDrawableView : View, IDrawableView
    {
		/// <summary>
		/// Draws the content on the specified canvas within the given dirty rectangle.
		/// </summary>
		/// <param name="canvas">The canvas to draw on.</param>
		/// <param name="dirtyRect">The rectangle area that needs to be redrawn.</param>
		void IDrawable.Draw(ICanvas canvas, RectF dirtyRect)
        {
            OnDraw(canvas, dirtyRect);
        }

		/// <summary>
		/// Draws the content on the specified canvas within the given dirty rectangle.
		/// </summary>
		/// <param name="canvas">The canvas to draw on.</param>
		/// <param name="dirtyRect">The rectangle area that needs to be redrawn.</param>
		protected virtual void OnDraw(ICanvas canvas, RectF dirtyRect)
        {

        }

		/// <summary>
		/// Invalidates the drawable, causing it to be redrawn.
		/// </summary>
		public void InvalidateDrawable()
        {
            if (this.Handler is SfDrawableViewHandler handler)
                handler.Invalidate();
        }
    }
}
