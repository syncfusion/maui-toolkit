using Syncfusion.Maui.Toolkit.Graphics.Internals;
using ITextElement = Syncfusion.Maui.Toolkit.Graphics.Internals.ITextElement;
namespace Syncfusion.Maui.Toolkit.TextInputLayout
{
	/// <summary>
	/// Represents the style for hint text, error text, and helper text label elements 
	/// in the <see cref="SfTextInputLayout"/> control. Provides customizable properties 
	/// for text appearance.
	/// </summary>
	public partial class LabelStyle : Element, ITextElement
	{
		#region Bindable properties

		/// <summary>
		/// Identifies the <see cref="TextColor"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="TextColor"/> property determines the color of the text in <see cref="SfTextInputLayout"/>.
		/// </remarks>
		public static readonly BindableProperty TextColorProperty =
		 BindableProperty.Create(
			 nameof(TextColor),
			 typeof(Color),
			 typeof(LabelStyle),
			 Color.FromRgba(0, 0, 0, 0.87));

		/// <summary>
		/// Identifies the <see cref="FontSize"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="FontSize"/> property determines the font size of the <see cref="SfTextInputLayout"/>.
		/// </remarks>
		public static readonly BindableProperty FontSizeProperty = FontElement.FontSizeProperty;

		/// <summary>
		/// Identifies the <see cref="FontFamily"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="FontFamily"/> property specifies the font family for the text in the <see cref="SfTextInputLayout"/>.
		/// </remarks>
		public static readonly BindableProperty FontFamilyProperty = FontElement.FontFamilyProperty;

		/// <summary>
		/// Identifies the <see cref="FontAttributes"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="FontAttributes"/> property specifies the font attributes for the text in the <see cref="SfTextInputLayout"/>.
		/// </remarks>
		public static readonly BindableProperty FontAttributesProperty = FontElement.FontAttributesProperty;

		/// <summary>
		/// Identifies the <see cref="FontAutoScalingEnabled"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="FontAutoScalingEnabled"/> property determines whether automatic font scaling is enabled in the <see cref="SfTextInputLayout"/>.
		/// </remarks>
		public static readonly BindableProperty FontAutoScalingEnabledProperty = FontElement.FontAutoScalingEnabledProperty;

		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the font size for the label.
		/// </summary>
		/// <example>
		/// The following code demonstrates how to use the FontSize property in the <see cref="SfTextInputLayout"/>.
		/// # [XAML](#tab/tabid-7)
		/// <code Lang="XAML"><![CDATA[
		/// <inputLayout:SfTextInputLayout x:Name="textInput"
		///                                Hint="Name">
		///  <inputLayout:SfTextInputLayout.HintLabelStyle>
		///    <inputLayout:LabelStyle FontSize="25" />
		/// </inputLayout:SfTextInputLayout.HintLabelStyle>
		///     <Entry />
		/// </inputLayout:SfTextInputLayout>
		/// ]]></code>
		/// # [C#](#tab/tabid-8)
		/// <code Lang="C#"><![CDATA[
		/// var inputLayout = new SfTextInputLayout();
		/// inputLayout.Hint = "Name";
		/// inputLayout.HintLabelStyle = new LabelStyle() { FontSize = 25 };
		/// inputLayout.Content = new Entry();
		/// ]]></code>
		/// </example>
		[System.ComponentModel.TypeConverter(typeof(FontSizeConverter))]
		public double FontSize
		{
			get { return (double)GetValue(FontSizeProperty); }
			set { SetValue(FontSizeProperty, value); }
		}

		/// <summary>
		/// Gets or sets the font family for the label.
		/// </summary>
		/// <example>
		/// The following code demonstrates how to use the FontFamily property in the <see cref="SfTextInputLayout"/>.
		/// # [XAML](#tab/tabid-7)
		/// <code Lang="XAML"><![CDATA[
		/// <inputLayout:SfTextInputLayout x:Name="textInput"
		///                                Hint="Name">
		///  <inputLayout:SfTextInputLayout.HintLabelStyle>
		///    <inputLayout:LabelStyle FontFamily="Lobster-Regular" />
		/// </inputLayout:SfTextInputLayout.HintLabelStyle>
		///     <Entry />
		/// </inputLayout:SfTextInputLayout>
		/// ]]></code>
		/// # [C#](#tab/tabid-8)
		/// <code Lang="C#"><![CDATA[
		/// var inputLayout = new SfTextInputLayout();
		/// inputLayout.Hint = "Name";
		/// inputLayout.HintLabelStyle = new LabelStyle() { FontFamily = "Lobster-Regular" };
		/// inputLayout.Content = new Entry();
		/// ]]></code>
		/// </example>
		public string FontFamily
		{
			get { return (string)GetValue(FontFamilyProperty); }
			set { SetValue(FontFamilyProperty, value); }
		}

		/// <summary>
		/// Gets or sets the font attributes for the label.
		/// </summary>
		/// <example>
		/// The following code demonstrates how to use the FontAttributes property in the <see cref="SfTextInputLayout"/>.
		/// # [XAML](#tab/tabid-7)
		/// <code Lang="XAML"><![CDATA[
		/// <inputLayout:SfTextInputLayout x:Name="textInput"
		///                                Hint="Name">
		///  <inputLayout:SfTextInputLayout.HintLabelStyle>
		///    <inputLayout:LabelStyle FontAttributes="Italic" />
		/// </inputLayout:SfTextInputLayout.HintLabelStyle>
		///     <Entry />
		/// </inputLayout:SfTextInputLayout>
		/// ]]></code>
		/// # [C#](#tab/tabid-8)
		/// <code Lang="C#"><![CDATA[
		/// var inputLayout = new SfTextInputLayout();
		/// inputLayout.Hint = "Name";
		/// inputLayout.HintLabelStyle = new LabelStyle() { FontAttributes = "Italic" };
		/// inputLayout.Content = new Entry();
		/// ]]></code>
		/// </example>
		public FontAttributes FontAttributes
		{
			get { return (FontAttributes)GetValue(FontAttributesProperty); }
			set { SetValue(FontAttributesProperty, value); }
		}

		/// <summary>
		/// Gets or sets whether automatic font scaling is enabled.
		/// </summary>
		/// <example>
		/// The following code demonstrates how to use the FontAutoScalingEnabled property in the <see cref="SfTextInputLayout"/>.
		/// # [XAML](#tab/tabid-7)
		/// <code Lang="XAML"><![CDATA[
		/// <inputLayout:SfTextInputLayout x:Name="textInput"
		///                                Hint="Name">
		///  <inputLayout:SfTextInputLayout.HintLabelStyle>
		///    <inputLayout:LabelStyle FontAutoScalingEnabled="True" />
		/// </inputLayout:SfTextInputLayout.HintLabelStyle>
		///     <Entry />
		/// </inputLayout:SfTextInputLayout>
		/// ]]></code>
		/// # [C#](#tab/tabid-8)
		/// <code Lang="C#"><![CDATA[
		/// var inputLayout = new SfTextInputLayout();
		/// inputLayout.Hint = "Name";
		/// inputLayout.HintLabelStyle = new LabelStyle() { FontAutoScalingEnabled = true };
		/// inputLayout.Content = new Entry();
		/// ]]></code>
		/// </example>
		public bool FontAutoScalingEnabled
		{
			get { return (bool)GetValue(FontAutoScalingEnabledProperty); }
			set { SetValue(FontAutoScalingEnabledProperty, value); }
		}

		/// <summary>
		/// Gets or sets the text color for the label.
		/// <remarks> 
		/// Used for helper, hint, and counter labels. For error label color, use the Stroke property in error state.
		/// </remarks> 
		/// </summary>
		/// <example>
		/// The following code demonstrates how to use the TextColor property in the <see cref="SfTextInputLayout"/>.
		/// # [XAML](#tab/tabid-7)
		/// <code Lang="XAML"><![CDATA[
		/// <inputLayout:SfTextInputLayout x:Name="textInput"
		///                                Hint="Name">
		///  <inputLayout:SfTextInputLayout.HintLabelStyle>
		///    <inputLayout:LabelStyle TextColor = "Green" />
		/// </inputLayout:SfTextInputLayout.HintLabelStyle>
		///     <Entry />
		/// </inputLayout:SfTextInputLayout>
		/// ]]></code>
		/// # [C#](#tab/tabid-8)
		/// <code Lang="C#"><![CDATA[
		/// var inputLayout = new SfTextInputLayout();
		/// inputLayout.Hint = "Name";
		/// inputLayout.HintLabelStyle = new LabelStyle() { TextColor = Color.Green };
		/// inputLayout.Content = new Entry();
		/// ]]></code>
		/// </example>
		public Color TextColor
		{
			get { return (Color)GetValue(TextColorProperty); }
			set { SetValue(TextColorProperty, value); }
		}

		Microsoft.Maui.Font ITextElement.Font => (Microsoft.Maui.Font)GetValue(FontElement.FontProperty);

		#endregion

		#region Methods

		double ITextElement.FontSizeDefaultValueCreator()
		{
			return 12d;
		}

		void ITextElement.OnFontAttributesChanged(FontAttributes oldValue, FontAttributes newValue)
		{

		}

		void ITextElement.OnFontFamilyChanged(string oldValue, string newValue)
		{

		}

		void ITextElement.OnFontSizeChanged(double oldValue, double newValue)
		{

		}

		void ITextElement.OnFontChanged(Microsoft.Maui.Font oldValue, Microsoft.Maui.Font newValue)
		{

		}

		/// <summary>
		/// Called when the <see cref="FontAutoScalingEnabledProperty"/> value changes.
		/// </summary>
		/// <param name="oldValue">The old value of <see cref="FontAutoScalingEnabled"/>.</param>
		/// <param name="newValue">The new value of <see cref="FontAutoScalingEnabled"/>.</param>
		void ITextElement.OnFontAutoScalingEnabledChanged(bool oldValue, bool newValue)
		{

		}

		#endregion
	}
}
