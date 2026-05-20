using Syncfusion.Maui.Toolkit.Graphics.Internals;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Toolkit.Themes;
using Font = Microsoft.Maui.Font;
using ITextElement = Syncfusion.Maui.Toolkit.Graphics.Internals.ITextElement;

namespace Syncfusion.Maui.Toolkit.SparkCharts
{
	/// <summary>
	/// Represents the label style for SparkChart trackball labels.
	/// </summary>
	/// <remarks>
	/// This class provides properties to customize the appearance of trackball labels in SparkCharts.
	/// 
	/// <para> <b>TextColor - </b> To customize the text color, refer to this <see cref="TextColor"/> property. </para>
	/// <para> <b>Background - </b> To customize the background color, refer to this <see cref="Background"/> property. </para>
	/// <para> <b>Stroke - </b> To customize the stroke color, refer to this <see cref="Stroke"/> property. </para>
	/// <para> <b>StrokeWidth - </b> To modify the stroke width, refer to this <see cref="StrokeWidth"/> property. </para>
	/// <para> <b>FontSize - </b> To change the text size for labels, refer to this <see cref="FontSize"/> property. </para>
	/// <para> <b>FontFamily - </b> To change the font family for labels, refer to this <see cref="FontFamily"/> property. </para>
	/// <para> <b>FontAttributes - </b> To change the font attributes for labels, refer to this <see cref="FontAttributes"/> property. </para>
	/// <para> <b>Margin - </b> To adjust the outer margin for labels, refer to this <see cref="Margin"/> property. </para>
	/// <para> <b>LabelFormat - </b> To customize the label format for labels, refer to this <see cref="LabelFormat"/> property. </para>
	/// <para> <b>CornerRadius - </b> To defines the rounded corners for labels, refer to this <see cref="CornerRadius"/> property. </para>
	/// </remarks>
	public class SparkChartLabelStyle : Element, ITextElement, IThemeElement
	{
		//When a large font size is set, the labels associated with the label style may overlap,
		//so the FontAutoScalingEnabled API has been disabled.
		bool _fontAutoScalingEnabled;

		#region Bindable Properties

		/// <summary>
		/// Identifies the <see cref="TextColor"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="TextColor"/> bindable property determines the color of the text in the trackball label.
		/// </remarks>
		public static readonly BindableProperty TextColorProperty = BindableProperty.Create(
			nameof(TextColor),
			typeof(Color),
			typeof(SparkChartLabelStyle),
			Colors.White,
			BindingMode.Default);

		/// <summary>
		/// Identifies the <see cref="Background"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="Background"/> bindable property determines the background brush of the trackball label.
		/// </remarks>
		public static readonly BindableProperty BackgroundProperty = BindableProperty.Create(
			nameof(Background),
			typeof(Brush),
			typeof(SparkChartLabelStyle),
			new SolidColorBrush(Colors.Transparent),
			BindingMode.Default);

		/// <summary>
		/// Identifies the <see cref="Stroke"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="Stroke"/> bindable property determines the brush used 
		/// for the stroke around the trackball label.
		/// </remarks>
		public static readonly BindableProperty StrokeProperty = BindableProperty.Create(
			nameof(Stroke),
			typeof(Brush),
			typeof(SparkChartLabelStyle),
			Brush.Transparent,
			BindingMode.Default);

		/// <summary>
		/// Identifies the <see cref="StrokeWidth"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="StrokeWidth"/> bindable property determines the stroke width around the trackball label.
		/// </remarks>
		public static readonly BindableProperty StrokeWidthProperty = BindableProperty.Create(
			nameof(StrokeWidth),
			typeof(double),
			typeof(SparkChartLabelStyle),
			0d,
			BindingMode.Default);

		/// <summary>
		/// Identifies the <see cref="FontSize"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="FontSize"/> bindable property determines the size of the font used in the trackball label.
		/// </remarks>
		public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(
			nameof(FontSize),
			typeof(double),
			typeof(SparkChartLabelStyle),
			12d,
			BindingMode.Default);

		/// <summary>
		/// Identifies the <see cref="FontFamily"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="FontFamily"/> bindable property determines the font family used in the trackball label.
		/// </remarks>
		public static readonly BindableProperty FontFamilyProperty = FontElement.FontFamilyProperty;

		/// <summary>
		/// Identifies the <see cref="FontAttributes"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="FontAttributes"/> bindable property determines the font attributes (e.g., bold, italic) 
		/// used in the trackball label.
		/// </remarks>
		public static readonly BindableProperty FontAttributesProperty = FontElement.FontAttributesProperty;

		/// <summary>
		/// Identifies the <see cref="Margin"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="Margin"/> bindable property determines the margin around the trackball label.
		/// </remarks>
		public static readonly BindableProperty MarginProperty = BindableProperty.Create(
			nameof(Margin),
			typeof(Thickness),
			typeof(SparkChartLabelStyle),
			new Thickness(3.5),
			BindingMode.Default);

		/// <summary>
		/// Identifies the <see cref="LabelFormat"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="LabelFormat"/> bindable property determines the format string used for the trackball label.
		/// </remarks>
		public static readonly BindableProperty LabelFormatProperty = BindableProperty.Create(
			nameof(LabelFormat),
			typeof(string),
			typeof(SparkChartLabelStyle),
			string.Empty,
			BindingMode.Default);

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets a value to customize the appearance of the label's text color.
		/// </summary>
		/// <value>It accepts <see cref="Color"/> values and the default value is <see cref="Colors.White"/>.</value>
		public Color TextColor
		{
			get => (Color)GetValue(TextColorProperty);
			set => SetValue(TextColorProperty, value);
		}

		/// <summary>
		/// Gets or sets a value to customize the appearance of the label's background.
		/// </summary>
		/// <value>It accepts <see cref="Brush"/> values and the default value is <see cref="Colors.Black"/>.</value>
		public Brush Background
		{
			get => (Brush)GetValue(BackgroundProperty);
			set => SetValue(BackgroundProperty, value);
		}

		/// <summary>
		/// Gets or sets a value to customize the outer stroke appearance of the label.
		/// </summary>
		/// <value>It accepts <see cref="Brush"/> values and the default value is <see cref="Brush.Transparent"/>.</value>
		public Brush Stroke
		{
			get => (Brush)GetValue(StrokeProperty);
			set => SetValue(StrokeProperty, value);
		}

		/// <summary>
		/// Gets or sets a value that indicates the stroke thickness of the label.
		/// </summary>
		/// <remarks>The value needs to be greater than zero.</remarks>
		/// <value>It accepts <see cref="double"/> values and the default value is 0.</value>
		public double StrokeWidth
		{
			get => (double)GetValue(StrokeWidthProperty);
			set => SetValue(StrokeWidthProperty, value);
		}

		/// <summary>
		/// Gets or sets a value that indicates the label's font size.
		/// </summary>
		/// <remarks>The value must be greater than zero.</remarks>
		/// <value>It accepts <see cref="double"/> values and the default value is 12.</value>
		public double FontSize
		{
			get => (double)GetValue(FontSizeProperty);
			set => SetValue(FontSizeProperty, value);
		}

		/// <summary>
		/// Gets or sets a value that indicates the font family of the label.
		/// </summary>
		/// <value>It accepts <see cref="string"/> values and the default value is <see cref="string.Empty"/>.</value>
		public string FontFamily
		{
			get => (string)GetValue(FontFamilyProperty);
			set => SetValue(FontFamilyProperty, value);
		}

		/// <summary>
		/// Gets or sets a value that indicates the font attributes of the label.
		/// </summary>
		/// <value>It accepts <see cref="Microsoft.Maui.Controls.FontAttributes"/> values and the default value is <see cref="FontAttributes.None"/>.</value>
		public FontAttributes FontAttributes
		{
			get => (FontAttributes)GetValue(FontAttributesProperty);
			set => SetValue(FontAttributesProperty, value);
		}

		/// <summary>
		/// Gets or sets a value that indicates the margin of the label.
		/// </summary>
		/// <value>It accepts <see cref="Thickness"/> values and the default value is 4.</value>
		public Thickness Margin
		{
			get => (Thickness)GetValue(MarginProperty);
			set => SetValue(MarginProperty, value);
		}

		/// <summary>
		/// Gets or sets a value to customize the label's format.
		/// </summary>
		/// <value>It accepts <see cref="string"/> values and the default value is <see cref="string.Empty"/>.</value>
		public string LabelFormat
		{
			get => (string)GetValue(LabelFormatProperty);
			set => SetValue(LabelFormatProperty, value);
		}

		#endregion

		#region Interface Implementation

		Font ITextElement.Font => (Font)GetValue(FontElement.FontProperty);

		bool ITextElement.FontAutoScalingEnabled { get => _fontAutoScalingEnabled; set => _fontAutoScalingEnabled = value; }

		// IFontManager? ITextElement.GetFontManager() => ((IView)Parent).GetFontManager();

		double ITextElement.FontSizeDefaultValueCreator()
		{
			return 12d;
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

		void IThemeElement.OnControlThemeChanged(string oldTheme, string newTheme)
		{
		}

		void IThemeElement.OnCommonThemeChanged(string oldTheme, string newTheme)
		{
		}

		void ITextElement.OnFontAutoScalingEnabledChanged(bool oldValue, bool newValue)
		{
		}

		#endregion
	}
}
