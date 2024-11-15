namespace Syncfusion.Maui.Toolkit.Graphics.Internals
{
    /// <summary>
    /// Interface for measure the text 
    /// </summary>
    public interface ITextMeasurer
    {
        /// <summary>
        /// This method returns the text's measured size 
        /// </summary>
        /// <param name="text">text</param>
        /// <param name="textSize">text size</param>
        /// <param name="attributes">attributes</param>
        /// <param name="fontFamily">font family</param>
        /// <returns>Measured size</returns>
        Size MeasureText(string text, float textSize, FontAttributes attributes = FontAttributes.None, string? fontFamily = null);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="textElement"></param>
        /// <returns></returns>
        Size MeasureText(string text, ITextElement textElement);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="width"></param>
        /// <param name="textElement"></param>
        /// <returns></returns>
        Size MeasureText(string text, double width, ITextElement textElement);
    }
}