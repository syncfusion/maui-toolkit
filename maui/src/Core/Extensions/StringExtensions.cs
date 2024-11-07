using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.Maui.Toolkit.Graphics.Internals
{
	/// <summary>
	/// Provides extension methods for the <see cref="string"/> class.
	/// </summary>
	public static class StringExtensions
    {
		/// <summary>
		/// Measures the size of the specified text with the given attributes.
		/// </summary>
		/// <param name="text">The text to measure.</param>
		/// <param name="textSize">The size of the text.</param>
		/// <param name="attributes">The font attributes to apply (e.g., bold, italic).</param>
		/// <param name="fontFamily">The font family to use, if any.</param>
		/// <returns>The size of the text as a <see cref="Size"/> object.</returns>
		public static Size Measure(this string text, float textSize, FontAttributes attributes = FontAttributes.None, string? fontFamily = null)
        {
            return TextMeasurer.Instance.MeasureText(text, textSize, attributes, fontFamily);
        }

		/// <summary>
		/// Gets the measured size of the text.
		/// </summary>
		/// <param name="text">The text to measure.</param>
		/// <param name="textElement">The text element that defines the text's appearance.</param>
		/// <returns>The size of the text as a <see cref="Size"/> object.</returns>
		public static Size Measure(this string text, ITextElement textElement)
        {
            return TextMeasurer.Instance.MeasureText(text, textElement);
        }

		/// <summary>
		/// Gets the measured size of the text, wrapping within the available width.
		/// </summary>
		/// <param name="text">The text to measure.</param>
		/// <param name="availableWidth">The available width for the text.</param>
		/// <param name="textElement">The text element that defines the text's appearance.</param>
		/// <returns>The size of the text as a <see cref="Size"/> object.</returns>
		public static Size Measure(this string text, double availableWidth, ITextElement textElement)
        {
            return TextMeasurer.Instance.MeasureText(text, availableWidth, textElement);
        }

        /// <summary>
        /// This method is utilized to adjust the text to fit within the available width.
        /// </summary>
        /// Note: It can be used from chart and sunbrust chart datalabels implement. 
        public static string TrimTextToFit(this string inputText, ITextElement textElement, double availableWidth)
        {
            while (inputText.Measure(textElement).Width > availableWidth && inputText.Length > 0)
            {
                inputText = inputText.Substring(0, inputText.Length - 1);
            }
            return inputText;
        }

        /// <summary>
        /// This method is utilized to get the total number of lines count from the text to fit within the available width.
        /// </summary>
        /// <param name="text">Text</param>
        /// <param name="maxWidth">Maximum width</param>
        /// <param name="textElement">The textElement</param>
        /// <param name="lineBreakMode">LineBreakMode</param>
        /// <param name="lines">List of Lines</param>
        /// <returns></returns>
        internal static double GetLinesCount(this string text, float maxWidth, ITextElement textElement, LineBreakMode lineBreakMode, out List<string> lines)
        {
            lines = new List<string>();
            var words = text.Split(' ');
            var tempWidth = 0.0;
            string currentLine = string.Empty;
            if (lineBreakMode == LineBreakMode.WordWrap)
            {
                foreach (var word in words)
                {
                    var testLine = currentLine.Length > 0 ? $"{currentLine} {word}" : word;
                    tempWidth = testLine.Measure(textElement).Width;

                    if (tempWidth > maxWidth)
                    {
                        lines.Add(currentLine.ToString());
                        currentLine = String.Empty;
                    }
                    if (currentLine == string.Empty)
                    {
                        currentLine = $"{word}";
                    }
                    else
                    {
                        currentLine = $"{currentLine} {word}";
                    }
                }
            }
            else if (lineBreakMode == LineBreakMode.CharacterWrap)
            {
                foreach (var character in text)
                {
                    var testLine = currentLine + character;
                    tempWidth = testLine.Measure(textElement).Width;

                    if (tempWidth > maxWidth)
                    {
                        lines.Add(currentLine);
                        currentLine = string.Empty;
                    }

                    currentLine += character;
                }
            }
            if (currentLine.Length > 0)
                lines.Add(currentLine.ToString());

            return lines.Count;
        }

        /// <summary>
        /// This method is utilized to adjust the text to fit within the available width and available height.
        /// </summary>
        public static string TrimTextToFitHeight(this string inputText, ITextElement textElement, LineBreakMode lineBreakMode, double availableWidth, double maxHeight)
        {
            List<string> lines;
            double lineCount = inputText.GetLinesCount((float)availableWidth, textElement, lineBreakMode, out lines);

            double perLineHeight = inputText.Measure(textElement).Height;

            int requiredLineCount = (int)(maxHeight / perLineHeight);

            if (lines[0] == string.Empty && lines.Count > 1)
            {
                lines.Remove(lines[0]);
            }
            string trimedText = string.Join(" ", lines.Take(requiredLineCount));

            return trimedText;
        }

        /// <summary>
        /// This method return the required text based on the LineBreakMode
        /// </summary>
        /// <param name="textElement">TextElement</param>
        /// <param name="text">Text</param>
        /// <param name="availableWidth">Available Width</param>
        /// <param name="availableHeight">Available Height</param>
        /// <param name="lineBreakMode">LineBreakMode</param>
        /// <returns></returns>
        public static string GetTextBasedOnLineBreakMode(this string text, ITextElement textElement, double availableWidth, double availableHeight, LineBreakMode lineBreakMode)
        {
            var textSize = text.Measure((ITextElement)textElement);

            if (textSize.Width <= availableWidth)
            {
                return text;
            }

            switch (lineBreakMode)
            {
                case LineBreakMode.TailTruncation:
                    double width = availableWidth - ("...").Measure(textElement).Width;
                    return text.TrimTextToFit(textElement, width) + "...";

                case LineBreakMode.NoWrap:
                    return text.TrimTextToFit(textElement, availableWidth);

                case LineBreakMode.MiddleTruncation:
                    int charsToKeep = (int)((availableWidth - ("...").Measure(textElement).Width) / 2);
                    string leftTrimmedText = text;
                    var leftTrimmedTextSize = leftTrimmedText.Measure((ITextElement)textElement);
                    int leftLength = 0;

                    while (leftTrimmedTextSize.Width > charsToKeep && leftTrimmedText.Length > 0)
                    {
                        leftTrimmedText = leftTrimmedText.Substring(0, leftTrimmedText.Length - 1);
                        leftTrimmedTextSize = leftTrimmedText.Measure((ITextElement)textElement);
                        leftLength++;
                    }
                    string rightText = text.Substring(leftLength);
                    string trimmedText = leftTrimmedText + "..." + rightText;
                    return trimmedText;

                case LineBreakMode.HeadTruncation:

                    string reversedText = string.Concat(text.Reverse());
                    string trimmedHeadText = reversedText.TrimTextToFit(textElement, availableWidth - ("...").Measure(textElement).Width);
                    return "..." + string.Concat(trimmedHeadText.Reverse());

                default:
                    return text.TrimTextToFitHeight(textElement, lineBreakMode, availableWidth, availableHeight);
            }
        }
    }
}
