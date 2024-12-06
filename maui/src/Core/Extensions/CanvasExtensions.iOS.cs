using CoreGraphics;
using Foundation;
using Microsoft.Maui.Graphics.Platform;
using UIKit;

namespace Syncfusion.Maui.Toolkit.Graphics.Internals
{
	/// <summary>
	/// Provides extension methods for the <see cref="ICanvas"/> interface on the mac and ios platfrom.
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
			if (canvas is PlatformCanvas)
			{
				IFontManager? fontManager;
				UIFont? uiFont;

				// TASK-836003: In MS native embedding IPlatformApplication.Current returns null, due to which the fontManager value is not set.
				fontManager = textElement.FontManager;
				if (fontManager != null)
				{
					uiFont = fontManager.GetFont(textElement.Font, textElement.FontSize);
					if (!textElement.FontAutoScalingEnabled && uiFont != null)
					{
						uiFont = uiFont.WithSize((nfloat)textElement.FontSize);
					}
				}
				else
				{
					if (textElement.Font.Family == null)
					{
						uiFont = UIFont.SystemFontOfSize((float)textElement.FontSize);
					}
					else
					{
						uiFont = UIFont.FromName(textElement.Font.Family, (float)textElement.FontSize);
					}

					// Apply the FontAttributes to the UIFont instance
					var fontDescriptor = uiFont.FontDescriptor;
					if (textElement.FontAttributes.HasFlag(FontAttributes.Bold))
					{
						fontDescriptor = fontDescriptor.CreateWithTraits(UIFontDescriptorSymbolicTraits.Bold);
					}

					if (textElement.FontAttributes.HasFlag(FontAttributes.Italic))
					{
						fontDescriptor = fontDescriptor.CreateWithTraits(UIFontDescriptorSymbolicTraits.Italic);
					}

					uiFont = UIFont.FromDescriptor(fontDescriptor, (float)textElement.FontSize);
				}

				UIStringAttributes? uiStringAttributes = new UIStringAttributes
				{
					ForegroundColor = textElement.TextColor.AsUIColor(),
					Font = uiFont
				};
				NSString drawText = new NSString(value);
				drawText.DrawString(new CGPoint(x, y), uiStringAttributes);
				drawText.Dispose();
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
		/// <param name="lineBreakMode">The line break mode of text</param>
		public static void DrawText(this ICanvas canvas, string value, Rect rect, HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment, ITextElement textElement, LineBreakMode lineBreakMode = LineBreakMode.WordWrap)
		{
			if (canvas is PlatformCanvas)
			{
				IFontManager? fontManager;
				UIFont? uiFont;
				// TASK-836003: In MS native embedding IPlatformApplication.Current returns null, due to which the fontManager value is not set.
				fontManager = textElement.FontManager;
				if (fontManager != null)
				{
					uiFont = fontManager.GetFont(textElement.Font, textElement.FontSize);
					if (!textElement.FontAutoScalingEnabled && uiFont != null)
					{
						uiFont = uiFont.WithSize((nfloat)textElement.FontSize);
					}
				}
				else
				{
					if (textElement.Font.Family == null)
					{
						uiFont = UIFont.SystemFontOfSize((float)textElement.FontSize);
					}
					else
					{
						uiFont = UIFont.FromName(textElement.Font.Family, (float)textElement.FontSize);
					}

					// Apply the FontAttributes to the UIFont instance
					var fontDescriptor = uiFont.FontDescriptor;
					if (textElement.FontAttributes.HasFlag(FontAttributes.Bold))
					{
						fontDescriptor = fontDescriptor.CreateWithTraits(UIFontDescriptorSymbolicTraits.Bold);
					}

					if (textElement.FontAttributes.HasFlag(FontAttributes.Italic))
					{
						fontDescriptor = fontDescriptor.CreateWithTraits(UIFontDescriptorSymbolicTraits.Italic);
					}

					uiFont = UIFont.FromDescriptor(fontDescriptor, (float)textElement.FontSize);
				}

				UIStringAttributes? uiStringAttributes = new UIStringAttributes
				{
					ForegroundColor = textElement.TextColor.AsUIColor(),
					Font = uiFont,
					ParagraphStyle = new NSMutableParagraphStyle
					{
						AllowsDefaultTighteningForTruncation = false
					}
				};
				UILineBreakMode Mode = GetLineBreakMode(lineBreakMode);
				uiStringAttributes.ParagraphStyle.LineBreakMode = Mode;

				if (!IsTruncationMode(Mode))
				{
					uiStringAttributes.ParagraphStyle.AllowsDefaultTighteningForTruncation = true;
				}

				//Use the native alignment option to align the text horizontally in the given rect.
				if (horizontalAlignment == HorizontalAlignment.Left)
				{
					uiStringAttributes.ParagraphStyle.Alignment = UITextAlignment.Left;
				}
				else if (horizontalAlignment == HorizontalAlignment.Right)
				{
					uiStringAttributes.ParagraphStyle.Alignment = UITextAlignment.Right;
				}
				else if (horizontalAlignment == HorizontalAlignment.Center)
				{
					uiStringAttributes.ParagraphStyle.Alignment = UITextAlignment.Center;
				}

				NSString drawText = new NSString(value);

				//Pass the width of the given rect to obtain the exact width and height of the text.
				CGRect measuredTextSize = drawText.GetBoundingRect(new CGSize(rect.Width, double.PositiveInfinity), NSStringDrawingOptions.UsesLineFragmentOrigin, uiStringAttributes, null);
				double x = rect.X;
				double y = rect.Y;
				if (verticalAlignment == VerticalAlignment.Center)
				{
					y = y + (rect.Height / 2) - (measuredTextSize.Height / 2);
					y = y < rect.Y ? rect.Y : y;
				}
				else if (verticalAlignment == VerticalAlignment.Bottom)
				{
					y = y + rect.Height - measuredTextSize.Height;
					y = y < rect.Y ? rect.Y : y;
				}

				drawText.DrawString(new CGRect(x, y, rect.Width, rect.Height), uiStringAttributes);
				drawText.Dispose();
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
			if (canvas is PlatformCanvas nativeCanvas)
			{
				CGContext context = nativeCanvas.Context;
				int j = 0;
				int count = points.Length / 2;
				CGPoint[] cgpoints = new CGPoint[count];
				for (int i = 0; i < count; i++)
				{
					cgpoints[i] = new CGPoint(points[j++], points[j++]);
				}

				context.SaveState();
				context.SetLineWidth((nfloat)lineDrawing.StrokeWidth);
				context.SetShouldAntialias(lineDrawing.EnableAntiAliasing);
				context.AddLines(cgpoints);

				if (lineDrawing.StrokeDashArray != null && lineDrawing.StrokeDashArray.Count > 0)
				{
					context.SetLineDash(0, GetNativeDashArrays(lineDrawing.StrokeDashArray));
				}

				context.SetAlpha(((NSNumber)lineDrawing.Opacity).FloatValue);
				context.SetStrokeColor(lineDrawing.Stroke.AsCGColor());

				context.StrokePath();
				context.RestoreState();
			}
		}

		/// <summary>
		/// Method used to convert <see cref="DoubleCollection"/> to native float array.
		/// </summary>
		/// <param name="dashes"></param>
		/// <returns></returns>
		private static nfloat[]? GetNativeDashArrays(DoubleCollection dashes)
		{
			if (dashes != null)
			{
				int count = dashes.Count;
				nfloat[] strokeDashes = new nfloat[count];

				int i = 0;
				foreach (var value in dashes)
				{
					strokeDashes[i] = ((NSNumber)value).FloatValue;
					i++;
				}

				return strokeDashes;
			}

			return null;
		}


		private static UILineBreakMode GetLineBreakMode(LineBreakMode lineBreakMode)
		{
			return lineBreakMode switch
			{
				LineBreakMode.NoWrap => UILineBreakMode.Clip,
				LineBreakMode.WordWrap => UILineBreakMode.WordWrap,
				LineBreakMode.CharacterWrap => UILineBreakMode.CharacterWrap,
				LineBreakMode.HeadTruncation => UILineBreakMode.HeadTruncation,
				LineBreakMode.MiddleTruncation => UILineBreakMode.MiddleTruncation,
				LineBreakMode.TailTruncation => UILineBreakMode.TailTruncation,
				_ => UILineBreakMode.WordWrap,
			};
		}


		private static bool IsTruncationMode(UILineBreakMode Mode)
		{
			if (Mode.Equals(UILineBreakMode.HeadTruncation) || Mode.Equals(UILineBreakMode.TailTruncation) || Mode.Equals(UILineBreakMode.MiddleTruncation))
			{
				return false;
			}
			return true;
		}

	}
}
