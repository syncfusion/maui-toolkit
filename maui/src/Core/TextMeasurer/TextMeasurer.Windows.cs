using Microsoft.Maui.Platform;
using Microsoft.UI.Xaml.Controls;
using Windows.UI.ViewManagement;

namespace Syncfusion.Maui.Toolkit.Graphics.Internals
{
    internal partial class TextMeasurer : ITextMeasurer
    {
        TextBlock? textBlock = null;

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
            if (textBlock == null)
                textBlock = new TextBlock();
            textBlock.Text = text;
            double fontSize = textSize > 0 ? textSize : 12;
            textBlock.FontSize = fontSize;
            textBlock.Measure(new Windows.Foundation.Size(double.PositiveInfinity, double.PositiveInfinity));
            return new Size((float)textBlock.DesiredSize.Width, (float)textBlock.DesiredSize.Height);
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
            if (textBlock == null)
                textBlock = new TextBlock();
            textBlock.Text = text;
            if (!double.IsNaN(textElement.FontSize))
            {
                UpdateFontSize(textElement);
            }
            var font = textElement.Font;
            if (fontManager != null)
            {
                textBlock.FontFamily = fontManager.GetFontFamily(font);
                textBlock.FontStyle = font.ToFontStyle();
                textBlock.FontWeight = font.ToFontWeight();
                textBlock.Measure(new Windows.Foundation.Size(double.PositiveInfinity, double.PositiveInfinity));
                return new Size((float)textBlock.DesiredSize.Width, (float)textBlock.DesiredSize.Height);
            }
            else
                return new Size(0, 0);
        }

        public Size MeasureText(string text, double width, ITextElement textElement)
        {
            IFontManager? fontManager = textElement.FontManager;

            if (textBlock == null)
                textBlock = new TextBlock();
            textBlock.Text = text;

            if (!double.IsNaN(textElement.FontSize))
            {
                UpdateFontSize(textElement);
            }

            if (fontManager != null)
            {
                var font = textElement.Font;
                textBlock.FontFamily = fontManager.GetFontFamily(font);
                textBlock.FontStyle = font.ToFontStyle();
                textBlock.FontWeight = font.ToFontWeight();
                textBlock.TextWrapping = Microsoft.UI.Xaml.TextWrapping.Wrap;
                textBlock.Measure(new Windows.Foundation.Size(width, double.PositiveInfinity));
                return new Size((float)textBlock.DesiredSize.Width, (float)textBlock.DesiredSize.Height);
            }
            else
                return new Size(0, 0);
        }

        private void UpdateFontSize(ITextElement textElement)
        {
            var uiSettings = new UISettings();
            float fontScale = (float)uiSettings.TextScaleFactor;
            double fontSize = textElement.FontSize > 0 ? textElement.FontSize : 12;
            if (textElement.FontAutoScalingEnabled)
                textBlock!.FontSize = fontSize * fontScale;
            else
                textBlock!.FontSize = fontSize;
        }
    }
}

