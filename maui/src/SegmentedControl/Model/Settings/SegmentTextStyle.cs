using Syncfusion.Maui.Toolkit.Graphics.Internals;
using Syncfusion.Maui.Toolkit.Themes;
using Font = Microsoft.Maui.Font;
using ITextElement = Syncfusion.Maui.Toolkit.Graphics.Internals.ITextElement;

namespace Syncfusion.Maui.Toolkit.SegmentedControl
{
	/// <summary>
	/// Gets or sets properties which allows to customize the segment item text style of the <see cref="SfSegmentedControl"/>.
	/// </summary>
	public partial class SegmentTextStyle : Element, ITextElement, IThemeElement
	{
		#region Bindable properties

		/// <summary>
		/// Identifies the <see cref="TextColor"/> bindable property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="TextColor"/> bindable property.
		/// </value>
		public static readonly BindableProperty TextColorProperty =
		   BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(SegmentTextStyle), defaultValueCreator: bindable => Color.FromArgb("#1C1B1F"));

		/// <summary>
		/// Identifies the <see cref="FontSize"/> bindable property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="FontSize"/> bindable property.
		/// </value>
		public static readonly BindableProperty FontSizeProperty = FontElement.FontSizeProperty;

		/// <summary>
		/// Identifies the <see cref="FontFamily"/> bindable property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="FontFamily"/> bindable property.
		/// </value>
		public static readonly BindableProperty FontFamilyProperty = FontElement.FontFamilyProperty;

		/// <summary>
		/// Identifies the <see cref="FontAttributes"/> bindable property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="FontAttributes"/> bindable property.
		/// </value>
		public static readonly BindableProperty FontAttributesProperty = FontElement.FontAttributesProperty;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="SegmentTextStyle"/> class.
		/// </summary>
		public SegmentTextStyle()
		{
			ThemeElement.InitializeThemeResources(this, "SfSegmentedControlTheme");
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the text color for the SfSegmentedControl.
		/// </summary>
		/// <value>
		/// The default value of <see cref="SegmentTextStyle.TextColor"/> is <see cref="Colors.Black"/>.
		/// </value>
		/// <remarks>
		/// It will be applicable to all the style related properties of the <see cref="SfSegmentedControl"/>.
		/// </remarks>
		/// <example>
		/// <code><![CDATA[
		/// var segmentTextStyle = new SegmentTextStyle
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
		/// Gets or sets the double value that represents the font size of the SfSegmentedControl.
		/// </summary>
		/// <remarks>
		/// It will be applicable to all the style related properties of the <see cref="SfSegmentedControl"/>.
		/// </remarks>
		/// <example>
		/// <code><![CDATA[
		/// var segmentTextStyle = new SegmentTextStyle
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
		/// Gets or sets the string, that represents font family of the SfSegmentedControl.
		/// </summary>
		/// <remarks>
		/// It will be applicable to all the style related properties of the <see cref="SfSegmentedControl"/>.
		/// </remarks>
		/// <example>
		/// <code><![CDATA[
		/// var segmentTextStyle = new SegmentTextStyle
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
		/// Gets or sets the FontAttributes of the SfSegmentedControl.
		/// </summary>
		/// <value>
		/// The default value is <see cref="FontAttributes.None"/>.
		/// </value>
		/// <example>
		/// <code><![CDATA[
		/// var segmentTextStyle = new SegmentTextStyle
		/// {
		///     FontAttributes = FontAttributes.Bold // Sets the font attributes to Bold
		/// };
		/// ]]></code>
		/// </example>
		public FontAttributes FontAttributes
		{
			get { return (FontAttributes)GetValue(FontAttributesProperty); }
			set { SetValue(FontAttributesProperty, value); }
		}

		/// <summary>
		/// Gets the font of the SfSegmentedControl.
		/// </summary>
		Font ITextElement.Font => (Font)GetValue(FontElement.FontProperty);

		/// <summary>
		/// Gets or sets a value indicating whether or not the font of the control should scale automatically according to the operating system settings.
		/// </summary>
		bool ITextElement.FontAutoScalingEnabled { get; set; }

		#endregion

		#region Private methods

		/// <inheritdoc/>
		double ITextElement.FontSizeDefaultValueCreator()
		{
			return 16d;
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

		/// <inheritdoc/>
		void ITextElement.OnFontAutoScalingEnabledChanged(bool oldValue, bool newValue)
		{
		}

		#endregion

		#region Interface Implementation

		/// <summary>
		/// Handles changes in the theme for individual controls.
		/// </summary>
		/// <param name="oldTheme">The old theme value.</param>
		/// <param name="newTheme">The new theme value.</param>
		void IThemeElement.OnControlThemeChanged(string oldTheme, string newTheme)
		{
		}

		/// <summary>
		/// Handles changes in the common theme shared across multiple elements.
		/// </summary>
		/// <param name="oldTheme">The old theme value.</param>
		/// <param name="newTheme">The new theme value.</param>
		void IThemeElement.OnCommonThemeChanged(string oldTheme, string newTheme)
		{
		}

		#endregion
	}
}