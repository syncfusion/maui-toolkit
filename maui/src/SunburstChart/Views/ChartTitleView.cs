using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui;

namespace Syncfusion.Maui.Toolkit.SunburstChart
{
#if NET9_0_OR_GREATER
    internal class ChartTitleView : Border
#else
    internal class ChartTitleView : Frame
#endif
    {
        #region Constructor 

        /// <summary>
		/// Initializes a new instance of the <see cref="ChartTitleView"/> class.
		/// </summary>
        internal ChartTitleView()
        {
#if !NET9_0_OR_GREATER
            HasShadow = false;
#endif
            Padding = 0; //For default size to be empty
            BackgroundColor = Colors.Transparent;
            HorizontalOptions = LayoutOptions.Fill;
            VerticalOptions = LayoutOptions.Center;
        }

        #endregion

        #region Method

        /// <summary>
		/// Initializes the title view's content with a label.
		/// </summary>
		/// <param name="content">The text to display in the title.</param>
        internal void InitTitle(string? content)
        {
            var label = new Label()
            {
                Text = content,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                TextColor = Color.FromArgb("#1C1B1F"),
                FontSize = 14,
                HorizontalTextAlignment = TextAlignment.Center,
            };

            Content = label;
        }

        #endregion
    }
}
