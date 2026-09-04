using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System.ComponentModel;

namespace Syncfusion.Maui.Toolkit.SunburstChart
{
    /// <summary>
    /// Represents the toolbar that appears during drill down operations in SunburstChart.
    /// </summary>
    internal class DrillDownToolbar : Border, INotifyPropertyChanged
    {
        #region Fields

        public new event PropertyChangedEventHandler? PropertyChanged;

        internal Label backIcon;
        internal Label resetIcon;
        private readonly HorizontalStackLayout stackLayout;
		private readonly SfSunburstChart chart;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="DrillDownToolbar"/> class.
		/// </summary>
		public DrillDownToolbar(SfSunburstChart sunburstChart)
		{
			chart = sunburstChart;
			// Create stack layout for buttons
			stackLayout = new HorizontalStackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
            };

            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += OnLabelTapped;

			// Create back icon button
			backIcon = CreateLabel("\ue72d");
			backIcon.GestureRecognizers.Add(tapGesture);

			// Create reset button
			resetIcon = CreateLabel("\ue726");
			resetIcon.GestureRecognizers.Add(tapGesture);

            // Add labels to the stack layout
            stackLayout.Children.Add(backIcon);
            stackLayout.Children.Add(resetIcon);

			Background = new SolidColorBrush(Color.FromArgb("#F7F2FB"));
			Content = stackLayout;
            Stroke = new SolidColorBrush(Color.FromArgb("#CAC4D0"));
            StrokeThickness = 1;
            IsVisible = false;
            WidthRequest = 120;
            HeightRequest = 48;
        }

		Label CreateLabel(string text)
		{
			return new Label
			{
				Text = text,
				FontSize = 24,
				FontFamily = "MauiMaterialAssets",
				TextColor = Color.FromArgb("#49454F"),
				WidthRequest = 60,
				HeightRequest = 48,
				VerticalOptions = LayoutOptions.Fill,
				HorizontalOptions = LayoutOptions.Fill,
				HorizontalTextAlignment = Microsoft.Maui.TextAlignment.Center,
				VerticalTextAlignment = Microsoft.Maui.TextAlignment.Center,
			};
		}

		#endregion

		#region Methods

		/// <summary>
		/// Sets the toolbar location with alignment adjustments (Xamarin equivalent)
		/// </summary>
		/// <param name="bounds">The chart area bounds</param>
		/// <param name="settings">Drill down settings</param>
		internal void SetToolbarLocation(Rect bounds, SunburstToolbarSettings settings)
        {
            double x = 0;
            double y = 0;

            // Horizontal alignment (same logic as Xamarin)
            switch (settings.HorizontalAlignment)
            {
                case SunburstToolbarAlignment.Start:
                    x = bounds.Left + settings.OffsetX;
                    break;
                case SunburstToolbarAlignment.Center:
                    x = (bounds.Right - bounds.Left) / 2 - Width / 2 + settings.OffsetX;
                    break;
                case SunburstToolbarAlignment.End:
                    x = bounds.Right - Width + settings.OffsetX;
                    break;
            }

            // Vertical alignment (same logic as Xamarin)
            switch (settings.VerticalAlignment)
            {
                case SunburstToolbarAlignment.Start:
                    y = bounds.Top + settings.OffsetY;
                    break;
                case SunburstToolbarAlignment.Center:
                    y = (bounds.Bottom - bounds.Top) / 2 - Height / 2 + settings.OffsetY;
                    break;
                case SunburstToolbarAlignment.End:
					//TODO: Need to calculate and position the toolbar exactly at bottom.
					y = (bounds.Bottom - Height + settings.OffsetY);
                    break;
            }

            AbsoluteLayout.SetLayoutBounds(this, new Rect(x, y, Width, Height));
            AbsoluteLayout.SetLayoutFlags(this, Microsoft.Maui.Layouts.AbsoluteLayoutFlags.None);
        }

        void OnLabelTapped(object? sender, TappedEventArgs e)
		{
			var manager = chart.DrillDownManager;

			if (sender is Label tappedLabel)
            {
                if (tappedLabel.Text == "\ue72d")
                {
					manager?.DrillUp();
				}
                else
                {
					manager?.Reset();
				}
            }
        }

        protected new void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}