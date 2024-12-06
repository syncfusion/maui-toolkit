using Microsoft.Graphics.Canvas.Text;
using Microsoft.Maui.Graphics.Win2D;
using Microsoft.Maui.Platform;
using System.Numerics;
using Windows.ApplicationModel;
using Font = Microsoft.Maui.Font;
using Windows.UI.ViewManagement;

namespace Syncfusion.Maui.Toolkit.Graphics.Internals
{
	/// <summary>
	/// Provides helper methods for determining if the current application is running as a packaged app.
	/// </summary>
	/// <exclude/>
	public static class PackagedAppHelper
	{
		/// <summary>
		///  Gets or sets a value indicating whether the application is running as a packaged or unpackaged.
		///  Returns `true` if the application is packaged, otherwise `false`.
		/// </summary>
		/// <returns>A boolean indicating if the application is packaged.</returns>
		/// <exclude/>
		public static bool IsPackaged()
		{
			try
			{
				var _ = Package.Current;
				return true;
			}
			catch (InvalidOperationException)
			{
				return false;
			}
		}
	}

	/// <summary>
	/// Provides extension methods for the <see cref="ICanvas"/> interface on the windows platfrom.
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
			if (string.IsNullOrEmpty(value) || canvas is not W2DCanvas w2DCanvas)
			{
				return;
			}

			try
			{
				using (var format = new CanvasTextFormat())
				{
					IFontManager? fontManager = textElement.FontManager;
					var font = textElement.Font;

					if (fontManager is not null)
					{
						format.FontFamily = GetFontFamily(textElement, fontManager, font);
						UpdateFontSize(textElement, format);
						format.FontStyle = font.ToFontStyle();
						format.FontWeight = font.ToFontWeight();
						w2DCanvas.Session.DrawText(value, new Vector2(x, y), textElement.TextColor.AsColor(), format);
					}
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Font error: {ex.Message}");
			}
		}

        /// <summary>
        /// Draw the text with in specified rectangle area.
        /// </summary>
        /// <param name="canvas">The canvas value.</param>
        /// <param name="value">The text value.</param>
        /// <param name="rect">The rectangle area that specifies the text bound.</param>
        /// <param name="horizontalAlignment">Text horizontal alignment option.</param>
        /// <param name="verticalAlignment">Text vertical alignment option.</param>
        /// <param name="textElement">The text style.</param>
        public static void DrawText(this ICanvas canvas, string value, Rect rect, HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment, ITextElement textElement)
        {
			if (canvas is W2DCanvas w2DCanvas)
			{
				using (var format = new CanvasTextFormat())
				{
					IFontManager? fontManager = textElement.FontManager;
					var font = textElement.Font;

					if (fontManager != null)
					{
						format.FontFamily = GetFontFamily(textElement, fontManager, font);
						if (!double.IsNaN(textElement.FontSize))
						{
							UpdateFontSize(textElement, format);
						}
						format.FontStyle = font.ToFontStyle();
						format.FontWeight = font.ToFontWeight();

						CanvasVerticalAlignment canvasVerticallAlignment = CanvasVerticalAlignment.Top;
						if (verticalAlignment == VerticalAlignment.Center)
						{
							canvasVerticallAlignment = CanvasVerticalAlignment.Center;
						}
						else if (verticalAlignment == VerticalAlignment.Bottom)
						{
							canvasVerticallAlignment = CanvasVerticalAlignment.Bottom;
						}

						format.VerticalAlignment = canvasVerticallAlignment;
						CanvasHorizontalAlignment canvasHorizontalAlignment = CanvasHorizontalAlignment.Left;
						if (horizontalAlignment == HorizontalAlignment.Center)
						{
							canvasHorizontalAlignment = CanvasHorizontalAlignment.Center;
						}
						else if (horizontalAlignment == HorizontalAlignment.Right)
						{
							canvasHorizontalAlignment = CanvasHorizontalAlignment.Right;
						}

						format.HorizontalAlignment = canvasHorizontalAlignment;
						format.Options = CanvasDrawTextOptions.Clip;
						w2DCanvas.Session.DrawText(value, new Windows.Foundation.Rect(rect.X, rect.Y, rect.Width, rect.Height), textElement.TextColor.AsColor(), format);
					}
				}
			}
		}

        /// <summary>
        /// Get the font family of the font
        /// </summary>
        /// <param name="textElement">The text element</param>
        /// <param name="fontManager">The font manager</param>
        /// <param name="font">The font</param>
        /// <returns>A string representing the local path or URI source of the font family. Defaults to "Segoe UI" if retrieval fails.</returns>
        private static string GetFontFamily(ITextElement textElement, IFontManager fontManager, Font font)
		{
			string fontPath = string.Empty;

			if (fontManager is null)
			{
				return "Segoe UI";
			}

			var fontFamily = fontManager.GetFontFamily(font);
			
			if (fontFamily is null)
			{
				return "Segoe UI";
			}

			string path = fontFamily.Source;
			string prefix = "ms-appx:///";
			string fontName = path.StartsWith(prefix) ? path.Substring(prefix.Length) : path;

			if (!string.IsNullOrEmpty(textElement.Font.Family))
			{
				try
				{
					fontPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{fontName}");
				}
				catch (Exception)
				{
					fontPath = string.Empty;
				}
			}

			if (!string.IsNullOrEmpty(fontPath))
			{
				if (!PackagedAppHelper.IsPackaged())
				{
					return $@"{fontPath}";
				}
				else
				{
					return fontFamily.Source;
				}
			}

			return "Segoe UI";
		}

        private static void UpdateFontSize(ITextElement textElement, CanvasTextFormat format)
        {
            var uiSettings = new UISettings();
            float fontScale = (float)uiSettings.TextScaleFactor;
            double fontSize = textElement.FontSize > 0 ? textElement.FontSize : 12;
            if (textElement.FontAutoScalingEnabled)
			{
				format.FontSize = (float)fontSize * fontScale;
			}
			else
			{
				format.FontSize = (float)fontSize;
			}
		}

		/// <summary>
		/// Draws lines connecting a series of points on the specified canvas using the provided line drawing settings.
		/// </summary>
		/// <param name="canvas">The canvas to draw on.</param>
		/// <param name="points">An array of points defining the lines to be drawn.</param>
		/// <param name="lineDrawing">The line drawing settings to use.</param>
		public static void DrawLines(this ICanvas canvas, float[] points, ILineDrawing lineDrawing)
        {
            if (canvas is W2DCanvas w2DCanvas)
            {
                int j = 0;

                w2DCanvas.StrokeSize = (float)lineDrawing.StrokeWidth;
                w2DCanvas.StrokeColor = lineDrawing.Stroke;
                w2DCanvas.Antialias = lineDrawing.EnableAntiAliasing;
                w2DCanvas.Alpha = lineDrawing.Opacity;

                if (lineDrawing.StrokeDashArray != null)
				{
					w2DCanvas.StrokeDashPattern = lineDrawing.StrokeDashArray.ToFloatArray();
				}
				//Draw path.

				PathF pathF = new PathF();
                while (j + 1 < points.Length)
                {
                    pathF.LineTo(points[j++], points[j++]);
                }

                //Rendering performance was improved while created as new path.
                pathF = new PathF(pathF);
                w2DCanvas.DrawPath(pathF);
            }
        }
    }
}
