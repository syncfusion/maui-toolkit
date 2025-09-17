using Microsoft.Maui.Controls;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Toolkit.Themes;

namespace Syncfusion.Maui.Toolkit.SunburstChart
{
    /// <summary>
    /// Represents to specify extra information when the mouse pointer moved over an element.
    /// </summary>
    public class SunburstTooltipSettings : Element, IThemeElement
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SunburstTooltipSettings"/> class.
        /// </summary>
        public SunburstTooltipSettings()
        {
            ThemeElement.InitializeThemeResources(this, "SfSunburstChartTheme");
        }

        #endregion

        #region IThemeElement Implementation

        void IThemeElement.OnControlThemeChanged(string oldTheme, string newTheme)
        {
        }

        void IThemeElement.OnCommonThemeChanged(string oldTheme, string newTheme)
        {
        }

        #endregion

        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="Duration"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty DurationProperty =
            BindableProperty.Create(
                nameof(Duration), 
                typeof(float), 
                typeof(SunburstTooltipSettings), 
                2f,
                BindingMode.Default);

        /// <summary>
        /// Identifies the <see cref="TextColor"/> bindable property.
        /// </summary>
        public static readonly BindableProperty TextColorProperty =
            BindableProperty.Create(
                nameof(TextColor), 
                typeof(Color), 
                typeof(SunburstTooltipSettings),  
                Color.FromArgb("#F4EFF4"),
                BindingMode.Default);

        /// <summary>
        /// Identifies the <see cref="Margin"/> bindable property.
        /// </summary>
        public static readonly BindableProperty MarginProperty =
            BindableProperty.Create(
                nameof(Margin), 
                typeof(Thickness), 
                typeof(SunburstTooltipSettings), 
                new Thickness(8, 2, 8, 2),
                BindingMode.Default);

        /// <summary>
        /// Identifies the <see cref="FontSize"/> bindable property.
        /// </summary>   
        public static readonly BindableProperty FontSizeProperty =
            BindableProperty.Create(
                nameof(FontSize), 
                typeof(double), 
                typeof(SunburstTooltipSettings), 
                12d, 
                BindingMode.Default);

        /// <summary>
        /// Identifies the <see cref="FontFamily"/> bindable property.
        /// </summary>
        public static readonly BindableProperty FontFamilyProperty =
            BindableProperty.Create(
                nameof(FontFamily), 
                typeof(string), 
                typeof(SunburstTooltipSettings), 
                "Roboto", 
                BindingMode.Default);

        /// <summary>
        /// Identifies the <see cref="FontAttributes"/> bindable property.
        /// </summary>
        public static readonly BindableProperty FontAttributesProperty =
            BindableProperty.Create(
                nameof(FontAttributes), 
                typeof(FontAttributes), 
                typeof(SunburstTooltipSettings), 
                FontAttributes.None, 
                BindingMode.Default);

        /// <summary>
        /// Identifies the <see cref="Background"/> bindable property.
        /// </summary>
        public static readonly BindableProperty BackgroundProperty =
            BindableProperty.Create(
                nameof(Background), 
                typeof(Brush), 
                typeof(SunburstTooltipSettings),
                new SolidColorBrush(Color.FromArgb("#1C1B1F")), 
                BindingMode.Default);
        
        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a value to specify the duration time in seconds for which the tooltip will be displayed.
        /// </summary>
        public float Duration
        {
            get { return (float)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }

        /// <summary>
        /// Gets or sets a thickness value to adjust the tooltip margin.
        /// </summary>
        public Thickness Margin
        {
            get { return (Thickness)GetValue(MarginProperty); }
            set { SetValue(MarginProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color value to customize the text color of the tooltip label.
        /// </summary>
        public Color TextColor
        {
            get { return (Color)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value to change the label's text size.
        /// </summary>
        public double FontSize
        {
            get { return (double)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value to specify the FontFamily for the tooltip label.
        /// </summary>
        /// <value>It accepts string values.</value>
        public string FontFamily
        {
            get { return (string)GetValue(FontFamilyProperty); }
            set { SetValue(FontFamilyProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value to specify the FontAttributes for the tooltip label.
        /// </summary>
        /// <value>It accepts <see cref="Microsoft.Maui.Controls.FontAttributes"/> values and the default value is <see cref="FontAttributes.None"/>.</value>
        public FontAttributes FontAttributes
        {
            get { return (FontAttributes)GetValue(FontAttributesProperty); }
            set { SetValue(FontAttributesProperty, value); }
        }

        /// <summary>
        /// Gets or sets the brush value to customize the tooltip background.
        /// </summary>
        public Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        #endregion
    }
}
