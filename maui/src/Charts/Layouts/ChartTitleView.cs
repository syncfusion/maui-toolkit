namespace Syncfusion.Maui.Toolkit.Charts
{
#if NET9_0_OR_GREATER
    internal partial class ChartTitleView : Border
#else
	internal partial class ChartTitleView : Frame
#endif
	{
		#region Constructor

        internal ChartTitleView()
        {
#if !NET9_0_OR_GREATER
			HasShadow = false;
#endif
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
