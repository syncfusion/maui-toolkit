using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Toolkit.Graphics.Internals;
using System;
using Font = Microsoft.Maui.Font;

namespace Syncfusion.Maui.Toolkit.SunburstChart
{
    /// <summary>
    /// Represents to customize the appearance of sunburst data labels.
    /// </summary>
    public class SunburstDataLabelSettings : BindableObject, ITextElement
    {
        //When a large font size is set, the data labels may overlap with sunburst hierarchical
        //levels, so the FontAutoScalingEnabled API has been disabled.
        bool _fontAutoScalingEnabled = false;

        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="RotationMode"/> bindable property.
        /// </summary>
        public static readonly BindableProperty RotationModeProperty =
            BindableProperty.Create(
                nameof(RotationMode), 
                typeof(SunburstLabelRotationMode),
                typeof(SunburstDataLabelSettings),
                SunburstLabelRotationMode.Angle, 
                BindingMode.Default);

        /// <summary>
        /// Identifies the <see cref="OverFlowMode"/> bindable property.
        /// </summary>   
        public static readonly BindableProperty OverFlowModeProperty =
            BindableProperty.Create(
                nameof(OverFlowMode), 
                typeof(SunburstLabelOverflowMode), 
                typeof(SunburstTooltipSettings),
                SunburstLabelOverflowMode.Trim, 
                BindingMode.Default);

        /// <summary>
        /// Identifies the <see cref="TextColor"/> bindable property.
        /// </summary>
        public static readonly BindableProperty TextColorProperty =
            BindableProperty.Create(
                nameof(TextColor), 
                typeof(Color), 
                typeof(SunburstDataLabelSettings), 
                null,
                BindingMode.Default, 
                null, 
                OnTextColorChanged);

        /// <summary>
        /// Identifies the <see cref="FontSize"/> bindable property.
        /// </summary>   
        public static readonly BindableProperty FontSizeProperty = FontElement.FontSizeProperty;

        /// <summary>
        /// Identifies the <see cref="FontFamily"/> bindable property.
        /// </summary>
        public static readonly BindableProperty FontFamilyProperty = FontElement.FontFamilyProperty;

        /// <summary>
        /// Identifies the <see cref="FontAttributes"/> bindable property.
        /// </summary>
        public static readonly BindableProperty FontAttributesProperty = FontElement.FontAttributesProperty;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the label rotation mode for data labels.
        /// </summary>
        /// <value>The default value is <see cref="SunburstLabelRotationMode.Angle"/>.</value>
        public SunburstLabelRotationMode RotationMode
        {
            get { return (SunburstLabelRotationMode)GetValue(RotationModeProperty); }
            set { SetValue(RotationModeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the label overflow mode for the data label. 
        /// </summary>
        /// <value>The default value is <see cref="SunburstLabelOverflowMode.Trim"/>.</value>
        public SunburstLabelOverflowMode OverFlowMode
        {
            get { return (SunburstLabelOverflowMode)GetValue(OverFlowModeProperty); }
            set { SetValue(OverFlowModeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color for the data labels. 
        /// </summary>
        public Color TextColor
        {
            get { return (Color)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the font size for data labels. 
        /// </summary>
        public double FontSize
        {
            get { return (double)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the font family for data labels.
        /// </summary>
        /// <value>It accepts string values.</value>
        public string FontFamily
        {
            get { return (string)GetValue(FontFamilyProperty); }
            set { SetValue(FontFamilyProperty, value); }
        }

        /// <summary>
        /// Gets or sets the font attributes for data labels.
        /// </summary>
        /// <value>It accepts <see cref="Microsoft.Maui.Controls.FontAttributes"/> values and the default value is <see cref="FontAttributes.None"/>.</value>
        public FontAttributes FontAttributes
        {
            get { return (FontAttributes)GetValue(FontAttributesProperty); }
            set { SetValue(FontAttributesProperty, value); }
        }

        // Explicit interface implementation for ITextElement
        double ITextElement.FontSize => FontSize;
        Font ITextElement.Font => (Font)GetValue(FontElement.FontProperty);

        bool ITextElement.FontAutoScalingEnabled 
        {
             get => _fontAutoScalingEnabled; 
             set => _fontAutoScalingEnabled = value; 
        }

        #endregion

        #region Implement Interface

        double ITextElement.FontSizeDefaultValueCreator()
        {
            return 11f;
        }

        void ITextElement.OnFontFamilyChanged(string oldValue, string newValue)
        {

        }

        void ITextElement.OnFontAttributesChanged(FontAttributes oldValue, FontAttributes newValue)
        {

        }

        void ITextElement.OnFontChanged(Font oldValue, Font newValue)
        {

        }

        void ITextElement.OnFontSizeChanged(double oldValue, double newValue)
        {

        }

        static void OnTextColorChanged(BindableObject bindable, object oldValue, object newValue)
        {

        }

        void ITextElement.OnFontAutoScalingEnabledChanged(bool oldValue, bool newValue)
        {
           
        }

        #endregion
    }
}
