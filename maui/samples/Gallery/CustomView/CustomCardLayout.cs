using Microsoft.Maui.Controls.Shapes;
using Syncfusion.Maui.ControlsGallery.Converters;
using System.Reflection;

namespace Syncfusion.Maui.ControlsGallery.CustomView
{
    /// <summary>
    /// 
    /// </summary>
    public class CustomCardLayout :Grid
    {
        VerticalStackLayout verticalStackLayout;
        Grid titleGrid;
        Label titleLabel;
        TapGestureRecognizer? tapGestureRecognizer;
        MainPageAndroid? mobilePageAndroid;
        MainPageiOS? mobilePageiOS;
        bool showExpandIcon = true;
        SfEffectsViewAdv effectsViewAdv;

        /// <summary>
        /// 
        /// </summary>
        public static readonly BindableProperty TitleProperty =
            BindableProperty.Create(nameof(Title), typeof(string), typeof(CustomCardLayout), null, propertyChanged: OnTitleChangedProperty);

        /// <summary>
        /// 
        /// </summary>
        public static readonly BindableProperty CardContentProperty =
            BindableProperty.Create(nameof(CardContent), typeof(View), typeof(CustomCardLayout), null, propertyChanged: OnContentPropertyChanged);

        /// <summary>
        /// 
        /// </summary>
        internal SampleModel? SampleModel { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public View CardContent
        {
            get => (View)GetValue(CardContentProperty);
            set => SetValue(CardContentProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public bool ShowExpandIcon
        {
            get
            {
                return showExpandIcon;
            }
            set
            {
                showExpandIcon = value;
                this.UpdateExpandIcon();
            }
        }

        private static void OnTitleChangedProperty(BindableObject bindable, object oldValue, object newValue)
        {
            var title = bindable as CustomCardLayout;
            title?.TitleChange((string)newValue);

        }

        private static void OnContentPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var bind = bindable as CustomCardLayout;
            bind?.ContentChanged((View)newValue);
        }

        private void TitleChange(string newValue)
        {
            this.titleLabel.Text = newValue;
        }

        private void ContentChanged(View newValue)
        {
            if(this.verticalStackLayout.Children.Count > 1)
            {
                this.verticalStackLayout.RemoveAt(1);
            }
            this.verticalStackLayout.Add(newValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public CustomCardLayout(MainPageAndroid? androidPage = null, MainPageiOS? iOSPage = null, String badgeText = "")
        {
            mobilePageAndroid = androidPage;
            mobilePageiOS = iOSPage;

            titleGrid = new Grid();
            titleGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Star });
            titleGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = 48 });

            verticalStackLayout = new VerticalStackLayout();
            titleLabel = new Label()
            {
                FontFamily = "Roboto-Regular",
                FontSize = 14,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                VerticalTextAlignment = TextAlignment.Center,
            };

            if (String.IsNullOrEmpty(badgeText))
            {
                titleGrid.Children.Add(titleLabel);
                Grid.SetColumnSpan(titleLabel, 2);
            }
            else
            {
                titleLabel.Padding = new Thickness(0, 0, 20, 0);
            }
            Label label = new()
            {
                Text="\ue7cd",
                FontFamily = "MauiSampleFontIcon",
                HorizontalTextAlignment=TextAlignment.Center,
                VerticalTextAlignment=TextAlignment.Center,
                FontSize=24
            };

            effectsViewAdv = new()
            {
                Content = label
                
            };
            Border expandiconBorder = new()
            {
                WidthRequest = 40,
                HeightRequest = 40,
                StrokeThickness = 0,
                BackgroundColor = Colors.Transparent,
                StrokeShape = new RoundRectangle
                {
                    CornerRadius = new CornerRadius(30)
                },
                Content = effectsViewAdv
            };
        
            titleGrid.Children.Add(expandiconBorder);
            Grid.SetColumn(expandiconBorder, 1);

            tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += TapGestureRecognizer_Tapped;
            titleGrid.GestureRecognizers.Add(tapGestureRecognizer);
            effectsViewAdv.GestureRecognizers.Add(tapGestureRecognizer);
            this.verticalStackLayout.Add(titleGrid);
            this.verticalStackLayout.IsClippedToBounds = true;

            this.Padding = 5;
            var layoutBorder = new Border()
            {
                StrokeThickness = 1,
                Background = Colors.Transparent
            };

            if (Application.Current != null)
            {
                AppTheme currentTheme = Application.Current.RequestedTheme;

                if (currentTheme == AppTheme.Dark)
                {
                    layoutBorder.Stroke = Color.FromRgba("#49454F");
                }
                else
                {
                    layoutBorder.Stroke = Color.FromRgba("#CAC4D0");
                }
            }

            layoutBorder.StrokeShape = new RoundRectangle()
            {
                CornerRadius = new CornerRadius(10),
            };
            layoutBorder.Content = verticalStackLayout;
            this.Children.Add(layoutBorder);
        }

        private void UpdateExpandIcon()
        {
            if (this.effectsViewAdv != null)
            {
                this.effectsViewAdv.IsVisible = this.ShowExpandIcon;
            }
        }

        private void TapGestureRecognizer_Tapped(object? sender, EventArgs e)
        {
            if (this.ShowExpandIcon)
            {
#if ANDROID
                mobilePageAndroid?.LoadSample(this.SampleModel, null, true);
#else
            mobilePageiOS?.LoadSample(this.SampleModel,null, true);
#endif
            }
        }
    }
}
