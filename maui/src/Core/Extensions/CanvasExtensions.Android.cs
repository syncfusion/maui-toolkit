using Android.Content.Res;
using Android.Graphics;
using Android.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Platform;
using System.Runtime.Versioning;
using Font = Microsoft.Maui.Font;
using Paint = Android.Graphics.Paint;
using Rect = Microsoft.Maui.Graphics.Rect;

namespace Syncfusion.Maui.Toolkit.Graphics.Internals
{
	/// <summary>
	/// Provides extension methods for the <see cref="ICanvas"/> interface on the Andorid platfrom.
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
            if (canvas is ScalingCanvas scalingCanvas)
            {
                var paint = TextUtils.TextPaintCache;
                paint.Reset();
                paint.AntiAlias = true;
                paint.SetColor(textElement.TextColor);
                IFontManager? fontManager = textElement.FontManager;
                Font font = textElement.Font;
                Typeface? tf = fontManager?.GetTypeface(font);
                paint.SetTypeface(tf);
                if (scalingCanvas.ParentCanvas is PlatformCanvas nativeCanvas)
                {
                    UpdateFontSize(textElement, paint, nativeCanvas);
                    nativeCanvas.DrawText(value, x, y, paint);
                }
            }
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
            if (canvas is ScalingCanvas scalingCanvas)
            {
                var paint = TextUtils.TextPaintCache;
                paint.Reset();
                paint.AntiAlias = true;
                paint.SetColor(textElement.TextColor);
                Font font = textElement.Font;
                IFontManager? fontManager;
                Typeface? tf;

                fontManager = textElement.FontManager;
                if (fontManager != null)
                {
                    tf = fontManager.GetTypeface(font);
                }
                else
                {
                    var fontAttribute = TypefaceStyle.Normal;

                    if (textElement.Font.GetFontAttributes().HasFlag(FontAttributes.Bold))
                        fontAttribute = TypefaceStyle.Bold;
                    if (textElement.Font.GetFontAttributes().HasFlag(FontAttributes.Italic))
                        fontAttribute = TypefaceStyle.Italic;

                    if (textElement.Font.Family == null)
                    {
                        tf = Typeface.Default;
                    }
                    else
                    {
                        tf = Typeface.Create(textElement.Font.Family, fontAttribute);
                    }
                }
                paint.SetTypeface(tf);
                if (scalingCanvas.ParentCanvas is PlatformCanvas nativeCanvas)
                {
                    UpdateFontSize(textElement, paint, nativeCanvas);
                    nativeCanvas.Canvas.Save();

                    Android.Text.Layout.Alignment? alignment = Android.Text.Layout.Alignment.AlignNormal;
                    if (horizontalAlignment == HorizontalAlignment.Center)
                    {
                        alignment = Android.Text.Layout.Alignment.AlignCenter;
                    }
                    else if (horizontalAlignment == HorizontalAlignment.Right)
                    {
                        alignment = Android.Text.Layout.Alignment.AlignOpposite;
                    }

                    StaticTextLayout layout = new StaticTextLayout(value, paint, (int)(rect.Width * nativeCanvas.DisplayScale), alignment, 1.0f, 0.0f, false);
                    double rectDensityHeight = rect.Height * nativeCanvas.DisplayScale;
                    //// Check the layout does not accommodate the text inside the specified rect then
                    //// restrict the text rendering with line count.
                    if (layout.Height > rectDensityHeight && layout.LineCount > 1)
                    {
                        int lineCount = 0;
                        for (int i = 0; i < layout.LineCount; i++)
                        {
                            //// Check the line index which draws outside the specified rect.
                            if (layout.GetLineBottom(i) > rectDensityHeight)
                            {
                                break;
                            }

                            lineCount++;
                        }

                        layout.NewLineCount = lineCount - 1;
                        //// Skip the text draw while it does have height to render single line text.
                        if (layout.NewLineCount <= 0)
                        {
                            return;
                        }
                    }

                    float y = (float)rect.Y;
                    if (verticalAlignment == VerticalAlignment.Center)
                    {
                        //// Calculate the top padding based on layout height only on 
                        //// vertical center alignment.
                        float currentHeight = (layout.NewLineCount == 0 ? layout.Height : layout.GetLineBottom(layout.NewLineCount)) / nativeCanvas.DisplayScale;
                        float height = (float)rect.Height;
                        if (currentHeight < height)
                        {
                            y += (height - currentHeight) / 2;
                        }
                    }

                    canvas.Translate((float)rect.X, y);
                    layout.Draw(nativeCanvas.Canvas);
                    nativeCanvas.Canvas.Restore();
                    layout.Dispose();
                }
            }
        }

        private static void DrawText(this PlatformCanvas nativeCanvas, string value, float x, float y, TextPaint textPaint)
        {
            Canvas canvas = nativeCanvas.Canvas;
            canvas.DrawText(value, x * nativeCanvas.DisplayScale, y * nativeCanvas.DisplayScale, textPaint);
        }

        private static void UpdateFontSize(ITextElement textElement, TextPaint paint, PlatformCanvas nativeCanvas)
        {
            var fontScale = Resources.System!.Configuration!.FontScale;
            double fontSize = textElement.FontSize > 0 ? textElement.FontSize : 12;
            if (textElement.FontAutoScalingEnabled)
                paint.TextSize = (float)(textElement.FontSize * nativeCanvas.DisplayScale * fontScale);
            else
                paint.TextSize = (float)(fontSize * nativeCanvas.DisplayScale);
        }

		/// <summary>
		/// Draws lines connecting a series of points on the specified canvas using the provided line drawing settings.
		/// </summary>
		/// <param name="canvas">The canvas to draw on.</param>
		/// <param name="points">An array of points defining the lines to be drawn.</param>
		/// <param name="lineDrawing">The line drawing settings to use.</param>
		public static void DrawLines(this ICanvas canvas, float[] points, ILineDrawing lineDrawing)
        {
            if (canvas is ScalingCanvas scalingCanvas)
            {
                if (scalingCanvas.ParentCanvas is PlatformCanvas nativeCanvas)
                {
                    Paint paint = LineDrawUtils.PaintCache;
                    if (lineDrawing.StrokeDashArray != null && lineDrawing.StrokeDashArray.Count > 0)
                    {
                        DashPathEffect? dashPathEffect = GetNativeDashArrays(lineDrawing.StrokeDashArray, nativeCanvas.DisplayScale);

                        paint.SetPathEffect(dashPathEffect);
                    }
                    else
                        paint.SetPathEffect(null);

                    paint.AntiAlias = lineDrawing.EnableAntiAliasing;
                    paint.SetColor(lineDrawing.Stroke);
                    paint.Alpha = (int)(255 * lineDrawing.Opacity);
                    paint.SetStyle(Paint.Style.Stroke);
                    paint.StrokeWidth = (float)lineDrawing.StrokeWidth * nativeCanvas.DisplayScale;

                    nativeCanvas.Canvas.DrawLines(points, paint);
                }
            }
        }

		/// <summary>
		/// Converts a collection of dash lengths to a native dash path effect, scaled by the display scale factor.
		/// </summary>
		/// <param name="dashes">The collection of dash lengths.</param>
		/// <param name="displayScale">The scale factor for the display.</param>
		/// <returns>A <see cref="DashPathEffect"/> representing the dash pattern, or null if the collection is empty or null.</returns>
		private static DashPathEffect? GetNativeDashArrays(DoubleCollection dashes, float displayScale)
        {
            if (dashes != null && dashes.Count > 1)
            {
                float[] array = new float[dashes.Count];
                var i = 0;
                foreach (var dash in dashes)
                {
                    array[i] = (float)dash * displayScale;
                    i++;
                }

                return new DashPathEffect(array, 0);
            }
            else
            {
                return null;
            }
        }
    }

    internal static class LineDrawUtils
    {
        internal static readonly Paint PaintCache = new Paint();
    }

	/// <summary>
	/// Provides extension methods for the paint.
	/// </summary>
	public static class PaintExtensions
    {
		/// <summary>
		/// Sets the color of the specified paint object.
		/// </summary>
		/// <param name="paint">The paint object to set the color for.</param>
		/// <param name="color">The color to set.</param>
		public static void SetColor(this Paint paint, Microsoft.Maui.Graphics.Color color)
        {
            if (paint != null && color != null)
            {
                paint.SetARGB((int)(color.Alpha * 255f), (int)(color.Red * 255f), (int)(color.Green * 255f), (int)(color.Blue * 255f));
            }
        }
    }

    /// <summary>
    /// Internal class that used to draw text inside specified rectangle by restricting line count.
    /// </summary>
    internal class StaticTextLayout : StaticLayout
    {
        /// <summary>
        /// Holds the value when layout draw text outside the specified bounds.
        /// </summary>
        internal int NewLineCount = 0;

        /// <summary>
        /// Return base line count while the text drawn inside the specified bounds.
        /// </summary>
        public override int LineCount => this.NewLineCount == 0 ? base.LineCount : this.NewLineCount;

#pragma warning disable CA1416 // Validate platform compatibility
#pragma warning disable CA1422 // Validate platform compatibility
        public StaticTextLayout(string? source, TextPaint? paint, int width, Alignment? align, float spacingmult, float spacingadd, bool includepad) : base(source, paint, width, align, spacingmult, spacingadd, includepad)
#pragma warning restore CA1422 // Validate platform compatibility
#pragma warning restore CA1416 // Validate platform compatibility
        {

        }
    }
}
