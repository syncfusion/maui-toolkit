using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Toolkit.Charts
{
    internal class ChartTitleView : Frame
    {
        #region Constructor

        internal ChartTitleView()
        {
            HasShadow = false;
            Padding = 0; //For default size to be empty
            BackgroundColor = Colors.Transparent;
            HorizontalOptions = LayoutOptions.Fill;
            VerticalOptions = LayoutOptions.Center;
            IsVisible = false;
        }

        #endregion

        #region Methods

        #region Internal Methods

        internal void InitTitle(string? content)
        {
            var label = new Label()
            {
                Text = content,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                TextColor = Color.FromArgb("#49454F"),
                FontSize = 16,
                HorizontalTextAlignment = TextAlignment.Center,
            };

            Content = label;
        }

        #endregion

        #endregion
    }
}
