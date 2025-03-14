using Android.Content.Res;
using Android.Graphics;
using Android.Text;
using Java.Util;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Font = Microsoft.Maui.Font;
using Rect = Android.Graphics.Rect;

namespace Syncfusion.Maui.Toolkit.Graphics.Internals
{

	internal partial class TextMeasurer : ITextMeasurer
	{

		internal static partial ITextMeasurer CreateTextMeasurer()
		{
			return new TextMeasurer();
		}

		/// <summary>
		/// Get the device's screen density
		/// </summary>
		/// <returns></returns>
		private static float GetDeviceDensity()
		{
			return (float)DeviceDisplay.MainDisplayInfo.Density;
		}

		/// <summary>
		/// This method returns the text's measured size 
		/// </summary>
		/// <param name="text">text</param>
		/// <param name="textSize">text size</param>
		/// <param name="attributes">attributes</param>
		/// <param name="fontFamily">font family</param>
		/// <returns>Measured size</returns>
		public Size MeasureText(string text, float textSize, FontAttributes attributes = FontAttributes.None, string? fontFamily = null)
		{
			TextPaint? paint = TextUtils.TextPaintCache;
			paint.Reset();
			if (fontFamily == null && attributes != FontAttributes.None)
			{
				var style = ToTypefaceStyle(attributes);
				TextUtils.TextPaintCache.SetTypeface(Typeface.Create(Typeface.Default, style));
			}
			//TODO: Calculate the size with embedded fonts.  

			paint.TextSize =  textSize * GetDeviceDensity();
			_ = textSize > 0 ? textSize : 12;
			Rect? bounds = TextUtils.BoundsCache;
			paint.GetTextBounds(text, 0, text.Length, bounds);
			var fontMetrics = paint.GetFontMetrics();
			double fontWidth = (double)(bounds.Width() / GetDeviceDensity());
        	double fontHeight = fontMetrics != null ? ((double)(fontMetrics.Bottom - fontMetrics.Top)) / GetDeviceDensity() : 0;
			return new Size(fontWidth, fontHeight);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="text"></param>
		/// <param name="textElement"></param>
		/// <returns></returns>
		public Size MeasureText(string text, ITextElement textElement)
		{
			TextPaint? paint = TextUtils.TextPaintCache;
			paint.Reset();
			IFontManager? fontManager = textElement.FontManager;
			Font font = textElement.Font;
			if (fontManager != null)
			{
				Android.Graphics.Typeface? tf = fontManager.GetTypeface(font);
				paint.SetTypeface(tf);
				UpdateFontSize(textElement, paint);
				paint.TextSize = (float) textElement.FontSize * GetDeviceDensity();
				Android.Graphics.Rect? bounds = Syncfusion.Maui.Toolkit.Graphics.Internals.TextUtils.BoundsCache;
				paint.GetTextBounds(text, 0, text.Length, bounds);
				var fontMetrics = paint.GetFontMetrics();
				double fontWidth = (double)(bounds.Width() / GetDeviceDensity());
                double fontHeight = fontMetrics != null ? ((double)(fontMetrics.Bottom - fontMetrics.Top)) / GetDeviceDensity() : 0;
                return new Size(fontWidth, fontHeight); 
			}
			else
			{
				return new Size(0, 0);
			}
		}

		private static TypefaceStyle ToTypefaceStyle(FontAttributes attributes)
		{
			TypefaceStyle style = TypefaceStyle.Normal;
			if ((attributes & (FontAttributes.Bold | FontAttributes.Italic)) == (FontAttributes.Bold | FontAttributes.Italic))
			{
				style = TypefaceStyle.BoldItalic;
			}
			else if ((attributes & FontAttributes.Bold) != 0)
			{
				style = TypefaceStyle.Bold;
			}
			else if ((attributes & FontAttributes.Italic) != 0)
			{
				style = TypefaceStyle.Italic;
			}

			return style;
		}

		public Size MeasureText(string text, double width, ITextElement textElement)
		{
			TextPaint? paint = TextUtils.TextPaintCache;
			paint.Reset();
			IFontManager? fontManager = textElement.FontManager;

			if (fontManager != null)
			{
				Font font = textElement.Font;
				Typeface? tf = fontManager.GetTypeface(font);
				paint.SetTypeface(tf);

				UpdateFontSize(textElement, paint);

				Rect? bounds = TextUtils.BoundsCache;
				paint.GetTextBounds(text, 0, text.Length, bounds);
#pragma warning disable CA1422 // Validate platform compatibility
				StaticLayout staticLayout = new StaticLayout((string?)text, paint, (int)width, Android.Text.Layout.Alignment.AlignNormal, 1, 0, true);
#pragma warning restore CA1422 // Validate platform compatibility
				return new Size(staticLayout.Width, staticLayout.Height);
			}
			else
			{
				return new Size(0, 0);
			}
		}

		private static void UpdateFontSize(ITextElement textElement, TextPaint paint)
		{
			var fontScale = Resources.System!.Configuration!.FontScale;
			double fontSize = textElement.FontSize > 0 ? textElement.FontSize : 12;
			if (textElement.FontAutoScalingEnabled)
			{
				paint.TextSize = (float)(fontSize * fontScale);
			}
			else
			{
				paint.TextSize = (float)fontSize;
			}
		}
	}


	internal static class TextUtils
	{
		internal static readonly TextPaint TextPaintCache = new();

		internal static readonly Rect BoundsCache = new();
	}
}
