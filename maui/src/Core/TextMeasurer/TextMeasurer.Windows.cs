using Microsoft.Maui.Platform;
using Microsoft.UI.Xaml.Controls;
using Windows.UI.ViewManagement;

namespace Syncfusion.Maui.Toolkit.Graphics.Internals
{
	internal partial class TextMeasurer : ITextMeasurer
	{
		TextBlock? _textBlock;

		internal static partial ITextMeasurer CreateTextMeasurer()
		{
			return new TextMeasurer();
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
			if (_textBlock == null)
			{
				_textBlock = new TextBlock();
			}

			_textBlock.Text = text;
			double fontSize = textSize > 0 ? textSize : 12;
			_textBlock.FontSize = fontSize;
			_textBlock.Measure(new Windows.Foundation.Size(double.PositiveInfinity, double.PositiveInfinity));
			return new Size((float)_textBlock.DesiredSize.Width, (float)_textBlock.DesiredSize.Height);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="text"></param>
		/// <param name="textElement"></param>
		/// <returns></returns>
		public Size MeasureText(string text, ITextElement textElement)
		{
			IFontManager? fontManager = textElement.FontManager;
			if (_textBlock == null)
			{
				_textBlock = new TextBlock();
			}

			_textBlock.Text = text;
			if (!double.IsNaN(textElement.FontSize))
			{
				UpdateFontSize(textElement);
			}
			var font = textElement.Font;
			if (fontManager != null)
			{
				_textBlock.FontFamily = fontManager.GetFontFamily(font);
				_textBlock.FontStyle = font.ToFontStyle();
				_textBlock.FontWeight = font.ToFontWeight();
				_textBlock.Measure(new Windows.Foundation.Size(double.PositiveInfinity, double.PositiveInfinity));
				return new Size((float)_textBlock.DesiredSize.Width, (float)_textBlock.DesiredSize.Height);
			}
			else
			{
				return new Size(0, 0);
			}
		}

		public Size MeasureText(string text, double width, ITextElement textElement)
		{
			IFontManager? fontManager = textElement.FontManager;

			if (_textBlock == null)
			{
				_textBlock = new TextBlock();
			}

			_textBlock.Text = text;

			if (!double.IsNaN(textElement.FontSize))
			{
				UpdateFontSize(textElement);
			}

			if (fontManager != null)
			{
				var font = textElement.Font;
				_textBlock.FontFamily = fontManager.GetFontFamily(font);
				_textBlock.FontStyle = font.ToFontStyle();
				_textBlock.FontWeight = font.ToFontWeight();
				_textBlock.TextWrapping = Microsoft.UI.Xaml.TextWrapping.Wrap;
				_textBlock.Measure(new Windows.Foundation.Size(width, double.PositiveInfinity));
				return new Size((float)_textBlock.DesiredSize.Width, (float)_textBlock.DesiredSize.Height);
			}
			else
			{
				return new Size(0, 0);
			}
		}

		private void UpdateFontSize(ITextElement textElement)
		{
			var uiSettings = new UISettings();
			float fontScale = (float)uiSettings.TextScaleFactor;
			double fontSize = textElement.FontSize > 0 ? textElement.FontSize : 12;
			if (textElement.FontAutoScalingEnabled)
			{
				_textBlock!.FontSize = fontSize * fontScale;
			}
			else
			{
				_textBlock!.FontSize = fontSize;
			}
		}
	}
}

