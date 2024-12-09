using Microsoft.Maui.Graphics;
using System;

namespace Syncfusion.Maui.Toolkit.Graphics.Internals
{
	/// <summary>
	/// Provides extension methods for the <see cref="ICanvas"/> interface.
	/// </summary>
	public static partial class CanvasExtensions
	{

		/// <summary>
		/// Draws text on the specified canvas at the given coordinates using the provided text element.
		/// </summary>
		/// <param name="canvas">The canvas to draw on.</param>
		/// <param name="value">The text to draw.</param>
		/// <param name="x">The x-coordinate where the text should be drawn.</param>
		/// <param name="y">The y-coordinate where the text should be drawn.</param>
		/// <param name="textElement">The text element that defines the text's appearance.</param>
		public static void DrawText(this ICanvas canvas, string value, float x, float y, ITextElement textElement)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Draw the text with in specified rectangle area.
		/// </summary>
		/// <param name="canvas">The canvas value.</param>
		/// <param name="value">The text value.</param>
		/// <param name="rect">The rectangle area thet specifies the text bound.</param>
		/// <param name="horizontalAlignment">Text horizontal alignment option.</param>
		/// <param name="verticalAlignment">Text vertical alignment option.</param>
		/// <param name="textElement">The text style.</param>
		public static void DrawText(this ICanvas canvas, string value, Rect rect, HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment, ITextElement textElement)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Draws lines connecting a series of points on the specified canvas using the provided line drawing settings.
		/// </summary>
		/// <param name="canvas">The canvas to draw on.</param>
		/// <param name="points">An array of points defining the lines to be drawn.</param>
		/// <param name="lineDrawing">The line drawing settings to use.</param>
		public static void DrawLines(this ICanvas canvas, float[] points, ILineDrawing lineDrawing)
		{
		}
	}
}
