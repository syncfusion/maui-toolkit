using Syncfusion.Maui.Toolkit.Graphics.Internals;
using Font = Microsoft.Maui.Font;
using ITextElement = Syncfusion.Maui.Toolkit.Graphics.Internals.ITextElement;

namespace Syncfusion.Maui.Toolkit.Picker
{
    /// <summary>
    /// Represents a class which is used to customize the text style of the picker.
    /// </summary>
    public class PickerTextStyle : Element, ITextElement
    {
        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="TextColor"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="TextColor"/> dependency property.
        /// </value>
        public static readonly BindableProperty TextColorProperty =
            BindableProperty.Create(
                nameof(TextColor),
                typeof(Color),
                typeof(PickerTextStyle),
                defaultValueCreator: bindable => Colors.Black);

        /// <summary>
        /// Identifies the <see cref="FontSize"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="FontSize"/> dependency property.
        /// </value>
        public static readonly BindableProperty FontSizeProperty = FontElement.FontSizeProperty;

        /// <summary>
        /// Identifies the <see cref="FontFamily"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="FontFamily"/> dependency property.
        /// </value>
        public static readonly BindableProperty FontFamilyProperty = FontElement.FontFamilyProperty;

        /// <summary>
        /// Identifies the <see cref="FontAttributes"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="FontAttributes"/> dependency property.
        /// </value>
        public static readonly BindableProperty FontAttributesProperty = FontElement.FontAttributesProperty;

        /// <summary>
        /// Identifies the <see cref="FontAutoScalingEnabled"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="FontAutoScalingEnabled"/> bindable property.
        /// </value>
        public static readonly BindableProperty FontAutoScalingEnabledProperty = FontElement.FontAutoScalingEnabledProperty;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the text color of the text style.
        /// </summary>
        /// <value> The default value of the <see cref="PickerTextStyle.TextColor"/> is "#000000"(black).</value>
        /// <remarks>
        /// It will be applicable to all the style related properties of the picker.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// var pickerTextStyle = new PickerTextStyle
        /// {
        ///     TextColor = Colors.Red
        /// };
        /// ]]></code>
        /// </example>
        public Color TextColor
        {
            get { return (Color)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the font size of the text style.
        /// </summary>
        /// <remarks>
        /// It will be applicable to all the style related properties of the picker.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// var pickerTextStyle = new PickerTextStyle
        /// {
        ///     FontSize = 20
        /// };
        /// ]]></code>
        /// </example>
        public double FontSize
        {
            get { return (double)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the font family of the text style.
        /// </summary>
        /// <remarks>
        /// It will be applicable to all the style related properties of the picker.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// var pickerTextStyle = new PickerTextStyle
        /// {
        ///     FontFamily = "Arial"
        /// };
        /// ]]></code>
        /// </example>
        public string FontFamily
        {
            get { return (string)GetValue(FontFamilyProperty); }
            set { SetValue(FontFamilyProperty, value); }
        }

        /// <summary>
        /// Gets or sets the font attributes of the text style.
        /// </summary>
        /// <value> The default value of the <see cref="PickerTextStyle.FontAttributes"/> is <see cref="FontAttributes.None"/> </value>
        /// <remarks>
        /// It will be applicable to all the style related properties of the picker.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// var pickerTextStyle = new PickerTextStyle
        /// {
        ///     FontAttributes = FontAttributes.Bold
        /// };
        /// ]]></code>
        /// </example>
        public FontAttributes FontAttributes
        {
            get { return (FontAttributes)GetValue(FontAttributesProperty); }
            set { SetValue(FontAttributesProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not the font of the control should scale automatically according to the operating system settings.
        /// </summary>
        /// <value>
        /// It accepts Boolean values, and the default value is false.
        /// </value>
        /// <remarks>
        /// It will be applicable to all the style related properties of the picker.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// var pickerTextStyle = new PickerTextStyle
        /// {
        ///     FontAutoScalingEnabled = true
        /// };
        /// ]]></code>
        /// </example>
        public bool FontAutoScalingEnabled
        {
            get { return (bool)GetValue(FontAutoScalingEnabledProperty); }
            set { SetValue(FontAutoScalingEnabledProperty, value); }
        }

        /// <summary>
        /// Gets the font of the text style.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1033:Interface methods should be callable by child types", Justification = "<Pending>")]
        Font ITextElement.Font => (Font)GetValue(FontElement.FontProperty);

        #endregion

        #region Private Methods

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1033:Interface methods should be callable by child types", Justification = "<Pending>")]
        double ITextElement.FontSizeDefaultValueCreator()
        {
            return 14d;
        }

        /// <inheritdoc/>
        void ITextElement.OnFontAttributesChanged(FontAttributes oldValue, FontAttributes newValue)
        {
        }

        /// <inheritdoc/>
        void ITextElement.OnFontChanged(Font oldValue, Font newValue)
        {
        }

        /// <inheritdoc/>
        void ITextElement.OnFontFamilyChanged(string oldValue, string newValue)
        {
        }

        /// <inheritdoc/>
        void ITextElement.OnFontSizeChanged(double oldValue, double newValue)
        {
        }

        /// <summary>
        /// To update the font auto scaling enable state.
        /// </summary>
        /// <param name="oldValue">old value</param>
        /// <param name="newValue">new value</param>
        void ITextElement.OnFontAutoScalingEnabledChanged(bool oldValue, bool newValue)
        {
        }

        #endregion
    }
}