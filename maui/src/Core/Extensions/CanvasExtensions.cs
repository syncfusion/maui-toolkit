#if IOS || MACCATALYST
using UIKit;
#endif

namespace Syncfusion.Maui.Toolkit.Graphics.Internals
{
	/// <summary>
	/// Provides extension methods for the <see cref="ICanvas"/> interface.
	/// </summary>
	public static partial class CanvasExtensions
    {
		/// <summary>
		/// Saves the current state of the canvas.
		/// </summary>
		/// <param name="canvas">The canvas whose state is to be saved.</param>
		public static void CanvasSaveState(this ICanvas canvas)
		{
#if IOS || MACCATALYST
			if (UIKit.UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
			{
				// For iOS 13 and above, use the SaveState and RestoreState methods.
				canvas.SaveState();
			}
			else
			{
				// Before iOS 13, use the CGContext equivalent.
				var context = UIGraphics.GetCurrentContext();
				context?.SaveState();
			}
#else
			canvas.SaveState();
#endif
		}

		/// <summary>
		/// Restores the canvas to its previous saved state.
		/// </summary>
		/// <param name="canvas">The canvas whose state is to be restored.</param>
		public static void CanvasRestoreState(this ICanvas canvas)
		{
#if IOS || MACCATALYST
			if (UIKit.UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
			{
				// For iOS 13 and above, use the SaveState and RestoreState methods.
				canvas.RestoreState();
			}
			else
			{
				// Before iOS 13, use the CGContext equivalent.
				var context = UIGraphics.GetCurrentContext();
				context?.RestoreState();
			}
#else
			canvas.RestoreState();
#endif
		}

		/// <summary>
		/// Draw triangle with provided rect.
		/// </summary>
		/// <param name="canvas">Corresponding canvas</param>
		/// <param name="rect">Symbol location and size</param>
		/// <param name="hasBorder">Need to draw border</param>
		public static void DrawTriangle(this ICanvas canvas, RectF rect, bool hasBorder)
        {
            float x = rect.X;
            float y = rect.Y;
            float width = x + rect.Width;
            float height = y + rect.Height;
            float midWidth = x + (rect.Width / 2);

            var path = new PathF();
            path.MoveTo(x, height);
            path.LineTo(midWidth, y);
            path.LineTo(width, height);
            path.LineTo(x, height);
            path.Close();
            canvas.FillPath(path);

            if (hasBorder)
            {
                canvas.DrawPath(path);
            }
        }

        /// <summary>
        /// Draw Inver triangle with provided rect.
        /// </summary>
        /// <param name="canvas">Corresponding canvas</param>
        /// <param name="rect">Symbol location and size</param>
        /// <param name="hasBorder">Need to draw border</param>
        public static void DrawInverseTriangle(this ICanvas canvas, RectF rect, bool hasBorder)
        {
            float x = rect.X;
            float y = rect.Y;
            float width = x + rect.Width;
            float height = y + rect.Height;
            float midWidth = x + (rect.Width / 2);

            var path = new PathF();
            path.MoveTo(x, y);
            path.LineTo(width, y);
            path.LineTo(midWidth, height);
            path.LineTo(x, y);
            path.Close();
            canvas.FillPath(path);

            if (hasBorder)
            {
                canvas.DrawPath(path);
            }
        }

        /// <summary>
        /// Draw cross with provided rect.
        /// </summary>
        /// <param name="canvas">Corresponding canvas</param>
        /// <param name="rect">Symbol location and size</param>
        /// <param name="hasBorder">Need to draw border</param>
        /// <param name="thickness">Symbol thickness</param>
        public static void DrawCross(this ICanvas canvas, RectF rect, bool hasBorder, float thickness = 5)
        {
            float x = rect.X;
            float y = rect.Y;
            float width = x + rect.Width;
            float height = y + rect.Height;
            float midWidth = x + (rect.Width / 2);
            float midHeight = y + (rect.Height / 2);
            float crossWidth = rect.Width / thickness;
            float crossHeight = rect.Height / thickness;

            var path = new PathF();
            path.MoveTo(width - crossWidth, y);
            path.LineTo(width, y + crossHeight);
            path.LineTo(midWidth + crossWidth, midHeight);
            path.LineTo(width, height - crossHeight);
            path.LineTo(width - crossWidth, height);
            path.LineTo(midWidth, midHeight + crossHeight);
            path.LineTo(x + crossWidth, height);
            path.LineTo(x, height - crossHeight);
            path.LineTo(midWidth - crossWidth, midHeight);
            path.LineTo(x, y + crossHeight);
            path.LineTo(x + crossWidth, y);
            path.LineTo(midWidth, midHeight - crossHeight);
            path.LineTo(width - crossWidth, y);
            path.Close();
            canvas.FillPath(path);

            if (hasBorder)
            {
                canvas.DrawPath(path);
            }
        }

        /// <summary>
        /// Draw plus with provided rect.
        /// </summary>
        /// <param name="canvas">Corresponding canvas</param>
        /// <param name="rect">Symbol location and size</param>
        /// <param name="hasBorder">Need to draw border</param>
        /// <param name="thickness">Symbol thickness</param>
        public static void DrawPlus(this ICanvas canvas, RectF rect, bool hasBorder, float thickness = 5)
        {
            float x = rect.X;
            float y = rect.Y;
            float width = x + rect.Width;
            float height = y + rect.Height;
            float midWidth = x + (rect.Width / 2);
            float midHeight = y + (rect.Height / 2);
            float crossWidth = rect.Width / thickness;
            float crossHeight = rect.Height / thickness;

            var path = new PathF();
            path.MoveTo(midWidth + crossWidth, y);
            path.LineTo(midWidth + crossWidth, midHeight - crossHeight);
            path.LineTo(width, midHeight - crossHeight);
            path.LineTo(width, midHeight + crossHeight);
            path.LineTo(midWidth + crossWidth, midHeight + crossHeight);
            path.LineTo(midWidth + crossWidth, height);
            path.LineTo(midWidth - crossWidth, height);
            path.LineTo(midWidth - crossWidth, midHeight + crossHeight);
            path.LineTo(x, midHeight + crossHeight);
            path.LineTo(x, midHeight - crossHeight);
            path.LineTo(midWidth - crossWidth, midHeight - crossHeight);
            path.LineTo(midWidth - crossWidth, y);
            path.LineTo(midWidth + crossWidth, y);
            path.Close();

            canvas.FillPath(path);

            if (hasBorder)
            {
                canvas.DrawPath(path);
            }
        }

        /// <summary>
        /// Draw diamond with provided rect.
        /// </summary>
        /// <param name="canvas">Corresponding canvas</param>
        /// <param name="rect">Symbol location and size</param>
        /// <param name="hasBorder">Need to draw border</param>
        public static void DrawDiamond(this ICanvas canvas, RectF rect, bool hasBorder)
        {
            float x = rect.X;
            float y = rect.Y;
            float width = x + rect.Width;
            float height = y + rect.Height;
            float midWidth = x + (rect.Width / 2);
            float midHeight = y + (rect.Height / 2);

            var path = new PathF();
            path.MoveTo(midWidth, y);
            path.LineTo(width, midHeight);
            path.LineTo(midWidth, height);
            path.LineTo(x, midHeight);
            path.LineTo(midWidth, y);
            path.Close();

            canvas.FillPath(path);

            if (hasBorder)
            {
                canvas.DrawPath(path);
            }
        }

        /// <summary>
        /// Draw hexagon with provided rect.
        /// </summary>
        /// <param name="canvas">Corresponding canvas</param>
        /// <param name="rect">Symbol location and size</param>
        /// <param name="hasBorder">Need to draw border</param>
        public static void DrawHexagon(this ICanvas canvas, RectF rect, bool hasBorder)
        {
            float x = rect.X;
            float y = rect.Y;
            float width = x + rect.Width;
            float height = y + rect.Height;
            float midWidth = x + (rect.Width / 2);
            float crossHeight = rect.Height / 4;

            var path = new PathF();
            path.MoveTo(midWidth, y);
            path.LineTo(width, y + crossHeight);
            path.LineTo(width, height - crossHeight);
            path.LineTo(midWidth, height);
            path.LineTo(x, height - crossHeight);
            path.LineTo(x, y + crossHeight);
            path.LineTo(midWidth, y);
            path.Close();

            canvas.FillPath(path);

            if (hasBorder)
            {
                canvas.DrawPath(path);
            }
        }

        /// <summary>
        /// Draw pentagon with provided rect.
        /// </summary>
        /// <param name="canvas">Corresponding canvas</param>
        /// <param name="rect">Symbol location and size</param>
        /// <param name="hasBorder">Need to draw border</param>
        public static void DrawPentagon(this ICanvas canvas, RectF rect, bool hasBorder)
        {
            float x = rect.X;
            float y = rect.Y;
            float width = x + rect.Width;
            float height = y + rect.Height;
            float midWidth = x + (rect.Width / 2);
            float crossWidth = rect.Width / 5;
            float crossHeight = rect.Height / 3;

            var path = new PathF();
            path.MoveTo(midWidth, y);
            path.LineTo(width, y + crossHeight);
            path.LineTo(width - crossWidth, height);
            path.LineTo(x + crossWidth, height);
            path.LineTo(x, y + crossHeight);
            path.LineTo(midWidth, y);
            path.Close();

            canvas.FillPath(path);
            if (hasBorder)
            {
                canvas.DrawPath(path);
            }
        }

        /// <summary>
        /// Draw away symbol with provided rect.
        /// </summary>
        /// <param name="canvas">Corresponding canvas.</param>
        /// <param name="rect">Symbol location and size.</param>
        public static void DrawAwaySymbol(ICanvas canvas, RectF rect)
        {
            var path = new PathF();
            path.MoveTo(rect.X + (rect.Width / 2) - 1, rect.Y);
            path.LineTo(rect.X + (rect.Width / 2) - 1, rect.Y + (rect.Height / 2) + 1);
            path.LineTo(rect.X + (rect.Width / 2) + 4.5f, rect.Y + rect.Height - 1.5f);
            canvas.DrawPath(path);
        }

        /// <summary>
        /// Draw tick symbol with provided rect.
        /// </summary>
        /// <param name="canvas">Corresponding canvas.</param>
        /// <param name="rect">Symbol location and size.</param>
        public static void DrawTick(ICanvas canvas, RectF rect)
        {
            var path = new PathF();
            path.MoveTo(rect.X + 1, rect.Y + rect.Height - 6);
            path.LineTo(rect.X + 4.5f, rect.Y + rect.Height - 2.5f);
            path.LineTo(rect.X + rect.Width - 0.5f, rect.Y + 2);
            canvas.DrawPath(path);
        }
    }
}
