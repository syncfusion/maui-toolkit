using System.Globalization;
using System.Windows.Input;
using Syncfusion.Maui.Toolkit.Graphics.Internals;
using Path = Microsoft.Maui.Controls.Shapes.Path;
using ITextElement = Syncfusion.Maui.Toolkit.Graphics.Internals.ITextElement;
#if WINDOWS
using Windows.Globalization.NumberFormatting;
#elif IOS
using UIKit;
#endif

namespace Syncfusion.Maui.Toolkit.NumericEntry
{
	/// <summary>
	/// The <see cref="SfNumericEntry"/> class allows users to input numeric values with various formatting options. 
	/// It provides support for features like value range constraints, numeric formats, and culture-specific settings.
	/// </summary>
	/// <example>
	/// The below example demonstrates how to initialize the <see cref="SfNumericEntry"/>.
	/// # [XAML](#tab/tabid-1)
	/// <code Lang="XAML"><![CDATA[
	/// <numeric:SfNumericEntry
	///       x:Name="numericEntry"
	///       Value="123"
	///       Minimum="0"
	///       Maximum="1000"
	///       WidthRequest="200"/>
	/// ]]></code>
	/// # [C#](#tab/tabid-2)
	/// <code Lang="C#"><![CDATA[
	/// SfNumericEntry numericEntry = new SfNumericEntry();
	/// numericEntry.Value = 123;
	/// numericEntry.Minimum = 0;
	/// numericEntry.Maximum = 1000;
	/// numericEntry.WidthRequest = 200;
	/// ]]></code>
	/// ***
	/// </example>
	public partial class SfNumericEntry
	{
		#region Bindable Properties

		/// <summary>
		/// Identifies the <see cref="ReturnCommand"/> bindable property.The return command will trigger whenever the return key is pressed. The default value is null.
		/// </summary>
		/// <value>The identifier for the <see cref="ReturnCommand"/> bindable property.</value>
		public static readonly BindableProperty ReturnCommandProperty =
			BindableProperty.Create(
				nameof(ReturnCommand),
				typeof(ICommand),
				typeof(SfNumericEntry),
				null,
				BindingMode.Default,
				null,
				OnReturnCommandPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="ReturnCommandParameter"/> bindable property. ReturnCommandParameter is a type of object and it specifies the parameter for the ReturnCommand.
		/// </summary>
		/// <value>The identifier for the <see cref="ReturnCommandParameter"/> bindable property.</value>
		public static readonly BindableProperty ReturnCommandParameterProperty =
			BindableProperty.Create(
				nameof(ReturnCommandParameter),
				typeof(object),
				typeof(SfNumericEntry),
				null,
				BindingMode.Default,
				null,
				OnReturnCommandParameterPropertyChanged);

		/// <summary>
		/// Identifies <see cref="Value"/> dependency property.
		/// </summary>
		/// <value>The identifier for the <see cref="Value"/> bindable property.</value>
		public static readonly BindableProperty ValueProperty =
			BindableProperty.Create(
				nameof(Value),
				typeof(double?),
				typeof(SfNumericEntry),
				null,
				BindingMode.TwoWay,
				propertyChanged: OnValueChanged);

		/// <summary>
		/// Identifies <see cref="Minimum"/> dependency property.
		/// </summary>
		/// <value>The identifier for the <see cref="Minimum"/> bindable property.</value>
		public static readonly BindableProperty MinimumProperty =
			BindableProperty.Create(
				nameof(Minimum),
				typeof(double),
				typeof(SfNumericEntry),
				double.MinValue,
				BindingMode.Default,
				propertyChanged: OnMinValueChanged);

		/// <summary>
		/// Identifies <see cref="Maximum"/> dependency property.
		/// </summary>
		/// <value>The identifier for the <see cref="Maximum"/> bindable property.</value>
		public static readonly BindableProperty MaximumProperty =
			BindableProperty.Create(
				nameof(Maximum),
				typeof(double),
				typeof(SfNumericEntry),
				double.MaxValue,
				BindingMode.Default,
				propertyChanged: OnMaxValueChanged);

		/// <summary>
		/// Identifies <see cref="MaximumNumberDecimalDigits"/> dependency property.
		/// </summary>
		/// <value>The identifier for the <see cref="MaximumNumberDecimalDigits"/> bindable property.</value>
		public static readonly BindableProperty MaximumNumberDecimalDigitsProperty =
			BindableProperty.Create(
				nameof(MaximumNumberDecimalDigits),
				typeof(int),
				typeof(SfNumericEntry),
				2,
				BindingMode.Default,
				propertyChanged: OnMaximumNumberDecimalDigitsChanged);

		/// <summary>
		/// Identifies <see cref="CustomFormat"/> dependency property.
		/// </summary>
		/// <value>The identifier for the <see cref="CustomFormat"/> bindable property.</value>
		public static readonly BindableProperty CustomFormatProperty =
			BindableProperty.Create(
				nameof(CustomFormat),
				typeof(string),
				typeof(SfNumericEntry),
				null,
				BindingMode.Default,
				propertyChanged: OnCustomFormatChanged);

		/// <summary>
		/// Identifies <see cref="Placeholder"/> dependency property.
		/// </summary>
		/// <value>The identifier for the <see cref="Placeholder"/> bindable property.</value>
		public static readonly BindableProperty PlaceholderProperty =
			BindableProperty.Create(
				nameof(Placeholder),
				typeof(string),
				typeof(SfNumericEntry),
				string.Empty,
				BindingMode.Default,
				propertyChanged: OnPlaceholderChanged);

		/// <summary>
		/// Identifies <see cref="AllowNull"/> dependency property.
		/// </summary>
		/// <value>The identifier for the <see cref="AllowNull"/> dependency property.</value>
		public static readonly BindableProperty AllowNullProperty =
			BindableProperty.Create(
				nameof(AllowNull),
				typeof(bool),
				typeof(SfNumericEntry),
				true,
				BindingMode.Default,
				propertyChanged: OnAllowNullValueChanged);

		/// <summary>
		/// Identifies <see cref="IsEditable"/> dependency property.
		/// </summary>
		/// <value>The identifier for the <see cref="IsEditable"/> dependency property.</value>
		public static readonly BindableProperty IsEditableProperty =
			BindableProperty.Create(
				nameof(IsEditable),
				typeof(bool),
				typeof(SfNumericEntry),
				true,
				BindingMode.Default,
				propertyChanged: OnIsEditableChanged);

		/// <summary>
		/// Identifies <see cref="EntryVisibility"/> dependency property.
		/// </summary>
		/// <value>The identifier for the <see cref="EntryVisibility"/> dependency property.</value>
		public static readonly BindableProperty EntryVisibilityProperty =
			BindableProperty.Create(
				nameof(EntryVisibility),
				typeof(Visibility),
				typeof(SfNumericEntry),
				Visibility.Visible,
				BindingMode.Default,
				propertyChanged: OnEntryVisibilityChanged);

		/// <summary>
		/// Identifies <see cref="Culture"/> dependency property.
		/// </summary>
		/// <value>The identifier for the <see cref="Culture"/> dependency property.</value>
		public static readonly BindableProperty CultureProperty =
			BindableProperty.Create(
				nameof(Culture),
				typeof(CultureInfo),
				typeof(SfNumericEntry),
				CultureInfo.CurrentCulture,
				BindingMode.Default,
				propertyChanged: OnCultureChanged);

		/// <summary>
		/// Identifies <see cref="ShowClearButton"/> dependency property.
		/// </summary>
		/// <value>The identifier for the <see cref="ShowClearButton"/> dependency property.</value>
		public static readonly BindableProperty ShowClearButtonProperty =
			BindableProperty.Create(
				nameof(ShowClearButton), 
				typeof(bool), 
				typeof(SfNumericEntry), 
				true, 
				BindingMode.Default,
				propertyChanged: OnShowClearButtonChanged);

		/// <summary>
		/// Identifies <see cref="PercentDisplayMode"/> dependency property.
		/// </summary>
		/// <value>The identifier for the <see cref="PercentDisplayMode"/> dependency property.</value>
		public static readonly BindableProperty PercentDisplayModeProperty =
			BindableProperty.Create(
				nameof(PercentDisplayMode),
				typeof(PercentDisplayMode),
				typeof(SfNumericEntry),
				PercentDisplayMode.Compute,
				BindingMode.Default,
				propertyChanged: OnPercentDisplayModePropertyChanged);

		/// <summary>
		/// Identifies <see cref="ShowBorder"/> dependency property.
		/// </summary>
		/// <value>The identifier for the <see cref="ShowBorder"/> dependency property.</value>
		public static readonly BindableProperty ShowBorderProperty =
			BindableProperty.Create(
				nameof(ShowBorder),
				typeof(bool),
				typeof(SfNumericEntry),
				true,
				BindingMode.Default,
				propertyChanged: OnShowBorderPropertyChanged);

		/// <summary>
		/// Identifies <see cref="PlaceholderColor"/> bindable property.
		/// </summary>
		/// <value>The identifier for the <see cref="PlaceholderColor"/> bindable property.</value>
		public static readonly BindableProperty PlaceholderColorProperty =
			BindableProperty.Create(
				nameof(PlaceholderColor),
				typeof(Color),
				typeof(SfNumericEntry),
				null,
				BindingMode.Default,
				propertyChanged: OnPlaceholderColorPropertyChanged);

		/// <summary>
		/// Identifies <see cref="ClearButtonColor"/> bindable property.
		/// </summary>
		/// <value>The identifier for the <see cref="ClearButtonColor"/> bindable property.</value>
		public static readonly BindableProperty ClearButtonColorProperty =
			BindableProperty.Create(
				nameof(ClearButtonColor),
				typeof(Color),
				typeof(SfNumericEntry),
				ClearIconStrokeColor,
				BindingMode.Default,
				propertyChanged: OnClearButtonColorPropertyChanged);

		/// <summary>
		/// Identifies <see cref="Stroke"/> bindable property.
		/// </summary>
		/// <value>The identifier for the <see cref="Stroke"/> bindable property.</value>
		public static readonly BindableProperty StrokeProperty =
			BindableProperty.Create(
				nameof(Stroke),
				typeof(Brush),
				typeof(SfNumericEntry),
				GetDefaultStroke(),
				BindingMode.Default,
				propertyChanged: OnStrokePropertyChanged);

		/// <summary>
		/// Identifies the <see cref="FontSize"/> bindable property.
		/// </summary>
		/// <value> The identifier for <see cref="FontSize"/> bindable property. </value>
		public static readonly BindableProperty FontSizeProperty = FontElement.FontSizeProperty;

		/// <summary>
		/// Identifies the <see cref="FontFamily"/> bindable property.
		/// </summary>
		/// <value> The identifier for <see cref="FontFamily"/> bindable property.</value>
		public static readonly BindableProperty FontFamilyProperty = FontElement.FontFamilyProperty;

		/// <summary>
		/// Identifies the <see cref="FontAttributes"/> bindable property.
		/// </summary>
		/// <value>The identifier for <see cref="FontAttributes"/> bindable property. </value>
		public static readonly BindableProperty FontAttributesProperty = FontElement.FontAttributesProperty;

		/// <summary>
		/// Identifies the <see cref="FontAutoScalingEnabled"/> bindable property.
		/// </summary>
		/// <value>The identifier for <see cref="FontAutoScalingEnabled"/> bindable property.</value>
		public static readonly BindableProperty FontAutoScalingEnabledProperty = FontElement.FontAutoScalingEnabledProperty;

		/// <summary>
		/// Identifies the <see cref="TextColor"/> bindable property.
		/// </summary>
		/// <value>The identifier for <see cref="TextColor"/> bindable property.</value>
		public static readonly BindableProperty TextColorProperty =
			BindableProperty.Create(
				nameof(TextColor),
				typeof(Color),
				typeof(SfNumericEntry),
				Colors.Black,
				BindingMode.Default,
				null,
				OnITextElementPropertyChanged);

		/// <summary>
		/// Identifies <see cref="CursorPosition"/> dependency property.
		/// </summary>
		/// <value>The identifier for the <see cref="CursorPosition"/> bindable property.</value>
		public static readonly BindableProperty CursorPositionProperty =
			BindableProperty.Create(
				nameof(CursorPosition),
				typeof(int),
				typeof(SfNumericEntry),
				0,
				BindingMode.Default);

		/// <summary>
		/// Identifies <see cref="SelectionLength"/> dependency property.
		/// </summary>
		/// <value>The identifier for the <see cref="SelectionLength"/> bindable property.</value>
		public static readonly BindableProperty SelectionLengthProperty =
			BindableProperty.Create(
				nameof(SelectionLength),
				typeof(int),
				typeof(SfNumericEntry),
				0,
				BindingMode.Default);

		/// <summary>
		/// Identifies <see cref="ValueChangeMode"/> dependency property.
		/// </summary>
		/// <value>The identifier for the <see cref="ValueChangeMode"/> bindable property.</value>
		public static readonly BindableProperty ValueChangeModeProperty =
			BindableProperty.Create(
				nameof(ValueChangeMode),
				typeof(ValueChangeMode),
				typeof(SfNumericEntry),
				ValueChangeMode.OnLostFocus,
				BindingMode.Default,
				propertyChanged: OnValueChangeModePropertyChanged);

		/// <summary>
		/// Identifies <see cref="HorizontalTextAlignment"/> dependency property.
		/// </summary>
		/// <value>The identifier for the <see cref="HorizontalTextAlignment"/> bindable property.</value>
		public static readonly BindableProperty HorizontalTextAlignmentProperty =
			BindableProperty.Create(
				nameof(HorizontalTextAlignment),
				typeof(TextAlignment),
				typeof(SfNumericEntry),
				TextAlignment.Start,
				BindingMode.Default,
				null,
				OnHorizontalTextAlignmentPropertyChanged);

		/// <summary>
		/// Identifies <see cref="VerticalTextAlignment"/> dependency property.
		/// </summary>
		/// <value>The identifier for the <see cref="VerticalTextAlignment"/> bindable property.</value>
		public static readonly BindableProperty VerticalTextAlignmentProperty =
			BindableProperty.Create(
				nameof(VerticalTextAlignment),
				typeof(TextAlignment),
				typeof(SfNumericEntry),
				TextAlignment.Center,
				BindingMode.Default,
				null,
				OnVerticalTextAlignmentPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="ReturnType"/> bindable property. This property can be used to change the ReturnType.
		/// </summary>
		/// <value>The identifier for the <see cref="ReturnType"/> bindable property.</value>
		public static readonly BindableProperty ReturnTypeProperty =
			BindableProperty.Create(
				nameof(ReturnType),
				typeof(ReturnType),
				typeof(SfNumericEntry),
				ReturnType.Default,
				BindingMode.Default,
				null,
				OnReturnTypePropertyChanged);

		/// <summary>
		/// Identifies the <see cref="ClearButtonPath"/> bindable property. This property can be used to Customize the clear button of NumericEntry control.
		/// </summary>
		/// <value>The identifier for the <see cref="ClearButtonPath"/> bindable property.</value>
		public static readonly BindableProperty ClearButtonPathProperty =
			   BindableProperty.Create(
				   nameof(ClearButtonPath),
				   typeof(Path),
				   typeof(SfNumericEntry),
				   null,
				   BindingMode.OneWay,
				   null,
				   OnClearButtonPathChanged);

#if WINDOWS
        /// <summary>
        /// Identifies <see cref="NumberFormatter"/> dependency property.
        /// </summary>
        static readonly BindableProperty NumberFormatterProperty =
            BindableProperty.Create(nameof(NumberFormatter), typeof(INumberFormatter), typeof(SfNumericEntry), null, propertyChanged: OnNumberFormatChanged);
#endif

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets a color that describes the stroke.
		/// </summary>
		/// <value>
		/// The default value is <c>Colors.LightGray</c>
		/// </value>
		/// <example>
		/// Below is an example of how to configure the <see cref="Stroke"/> property using XAML and C#:
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <numericEntry:SfNumericEntry
		///     x:Name="numericEntry"
		///     Stroke="LightGray" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// SfNumericEntry numericEntry = new SfNumericEntry();
		/// numericEntry.Stroke = Colors.LightGray;
		/// this.Content = numericEntry;
		/// ]]></code>
		/// 
		/// ***
		/// </example>
		public Brush Stroke
		{
			get { return (Brush)GetValue(StrokeProperty); }
			set { SetValue(StrokeProperty, value); }
		}

		/// <summary>
		/// Gets or sets a color that describes the color of placeholder text.
		/// </summary>
		/// <value>
		/// The default value is Colors.Black.
		/// </value>
		/// <example>
		/// Below is an example of how to configure the <see cref="PlaceholderColor"/> property using XAML and C#:
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <numericEntry:SfNumericEntry
		///     x:Name="numericEntry"
		///     Placeholder="Enter a value"
		///     PlaceholderColor="Red" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// SfNumericEntry numericEntry = new SfNumericEntry();
		/// numericEntry.Placeholder = "Enter a value";
		/// numericEntry.PlaceholderColor = Colors.Red;
		/// this.Content = numericEntry;
		/// ]]></code>
		/// 
		/// ***
		/// </example>
		public Color PlaceholderColor
		{
			get { return (Color)GetValue(PlaceholderColorProperty); }
			set { SetValue(PlaceholderColorProperty, value); }
		}

		/// <summary>
		/// Gets or sets a color that describes the color of clear button.
		/// </summary>
		/// <value>
		/// The default value is Colors.Black.
		/// </value>
		/// <example>
		/// Below is an example of how to configure the <see cref="ClearButtonColor"/> property using XAML and C#:
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <numericEntry:SfNumericEntry
		///     x:Name="numericEntry"
		///     ClearButtonColor="Red" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// SfNumericEntry numericEntry = new SfNumericEntry();
		/// numericEntry.ClearButtonColor = Colors.Red;
		/// this.Content = numericEntry;
		/// ]]></code>
		/// 
		/// ***
		/// </example>
		public Color ClearButtonColor
		{
			get { return (Color)GetValue(ClearButtonColorProperty); }
			set { SetValue(ClearButtonColorProperty, value); }
		}


		/// <summary>
		/// Gets or sets the value that indicates whether the font for the NumericEntry text is bold,italic, or neither.
		/// </summary>
		/// <value>Specifies the font attributes.The default value is  <see cref="FontAttributes.None"/> </value>
		/// <example>
		/// Below is an example of how to configure the <see cref="FontAttributes"/> property using XAML and C#:
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <numericEntry:SfNumericEntry
		///     x:Name="numericEntry"
		///     FontAttributes="Bold" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// SfNumericEntry numericEntry = new SfNumericEntry();
		/// numericEntry.FontAttributes = FontAttributes.Bold;
		/// this.Content = numericEntry;
		/// ]]></code>
		/// 
		/// ***
		/// </example>
		public FontAttributes FontAttributes
		{
			get { return (FontAttributes)GetValue(FontAttributesProperty); }
			set { SetValue(FontAttributesProperty, value); }
		}

		/// <summary>
		/// Gets or sets the font family for the text of <see cref="SfNumericEntry"/>.
		/// </summary>
		/// <value>Specifies the font family.The default value is string.empty. </value>
		/// <example>
		/// Below is an example of how to configure the <see cref="FontFamily"/> property using XAML and C#:
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <numericEntry:SfNumericEntry
		///     x:Name="numericEntry"
		///     FontFamily="Arial" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// SfNumericEntry numericEntry = new SfNumericEntry();
		/// numericEntry.FontFamily = "Arial";
		/// this.Content = numericEntry;
		/// ]]></code>
		/// 
		/// ***
		/// </example>
		public string FontFamily
		{
			get { return (string)GetValue(FontFamilyProperty); }
			set { SetValue(FontFamilyProperty, value); }
		}

		/// <summary>
		/// Gets or sets the value of FontSize. This property can be used to the size of the font for <see cref="SfNumericEntry"/>.
		/// </summary>
		/// <value>Specifies the font size.The default value is 14d.</value>
		/// <example>
		/// Below is an example of how to configure the <see cref="FontSize"/> property using XAML and C#:
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <numericEntry:SfNumericEntry
		///     x:Name="numericEntry"
		///     FontSize="18" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// SfNumericEntry numericEntry = new SfNumericEntry();
		/// numericEntry.FontSize = 18;
		/// this.Content = numericEntry;
		/// ]]></code>
		/// 
		/// ***
		/// </example>
		[System.ComponentModel.TypeConverter(typeof(FontSizeConverter))]
		public double FontSize
		{
			get { return (double)GetValue(FontSizeProperty); }
			set { SetValue(FontSizeProperty, value); }
		}

		/// <summary>
		/// Enables automatic font size adjustment based on device settings.
		/// </summary>
		/// <value>The default value is false.</value>
		/// <example>
		/// Below is an example of how to configure the <see cref="FontAutoScalingEnabled"/> property using XAML and C#:
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <numericEntry:SfNumericEntry
		///     x:Name="numericEntry"
		///     FontAutoScalingEnabled="True" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// SfNumericEntry numericEntry = new SfNumericEntry();
		/// numericEntry.FontAutoScalingEnabled = true;
		/// this.Content = numericEntry;
		/// ]]></code>
		/// 
		/// ***
		/// </example>
		public bool FontAutoScalingEnabled
		{
			get { return (bool)GetValue(FontAutoScalingEnabledProperty); }
			set { SetValue(FontAutoScalingEnabledProperty, value); }
		}

		/// <summary>
		/// Gets or sets the font for the text of <see cref="SfNumericEntry"/>.
		/// </summary>
		public Microsoft.Maui.Font Font => (Microsoft.Maui.Font)GetValue(FontElement.FontProperty);

		/// <summary>
		/// Gets or sets the color for the text of the <see cref="SfNumericEntry"/> control.
		/// </summary>
		/// <value>
		/// It accepts <see cref="Color"/> values and the default value is Colors.Black.
		/// </value>
		/// <example>
		/// Below is an example of how to configure the <see cref="TextColor"/> property using XAML and C#:
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <numericEntry:SfNumericEntry
		///     x:Name="numericEntry"
		///     TextColor="Blue" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// SfNumericEntry numericEntry = new SfNumericEntry();
		/// numericEntry.TextColor = Colors.Blue;
		/// this.Content = numericEntry;
		/// ]]></code>
		/// 
		/// ***
		/// </example>
		public Color TextColor
		{
			get { return (Color)GetValue(TextColorProperty); }
			set { SetValue(TextColorProperty, value); }
		}

		/// <summary>
		/// Gets or sets the cursor position of the <see cref="SfNumericEntry"/> control.
		/// </summary>
		/// <value>
		/// The default value is 0.
		/// </value>
		/// <example>
		/// Below is an example of how to configure the <see cref="CursorPosition"/> property using XAML and C#:
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <numericEntry:SfNumericEntry
		///     x:Name="numericEntry"
		///     CursorPosition="5" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// SfNumericEntry numericEntry = new SfNumericEntry();
		/// numericEntry.CursorPosition = 5;
		/// this.Content = numericEntry;
		/// ]]></code>
		/// 
		/// ***
		/// </example>
		public int CursorPosition
		{
			get { return (int)GetValue(CursorPositionProperty); }
			set { SetValue(CursorPositionProperty, value); }
		}

		/// <summary>
		/// Gets or sets the selection length of the <see cref="SfNumericEntry"/> control.
		/// </summary>
		/// <value>The default value is 0.</value>
		/// <example>
		/// Below is an example of how to configure the <see cref="SelectionLength"/> property using XAML and C#:
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <numericEntry:SfNumericEntry
		///     x:Name="numericEntry"
		///     SelectionLength="3" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// SfNumericEntry numericEntry = new SfNumericEntry();
		/// numericEntry.SelectionLength = 3;
		/// this.Content = numericEntry;
		/// ]]></code>
		/// 
		/// ***
		/// </example>
		public int SelectionLength
		{
			get { return (int)GetValue(SelectionLengthProperty); }
			set { SetValue(SelectionLengthProperty, value); }
		}

		/// <summary>
		/// Gets or sets whether the value should be updated or not based on this property while entering the value.
		/// </summary>
		/// <value>The default value is <see cref="ValueChangeMode.OnLostFocus"/></value>
		/// <example>
		/// Below is an example of how to configure the <see cref="ValueChangeMode"/> property using XAML and C#:
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <numericEntry:SfNumericEntry
		///     x:Name="numericEntry"
		///     ValueChangeMode="OnKeyFocus" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// SfNumericEntry numericEntry = new SfNumericEntry();
		/// numericEntry.ValueChangeMode = ValueChangeMode.OnKeyFocus;
		/// this.Content = numericEntry;
		/// ]]></code>
		/// 
		/// ***
		/// </example>
		public ValueChangeMode ValueChangeMode
		{
			get { return (ValueChangeMode)GetValue(ValueChangeModeProperty); }
			set { SetValue(ValueChangeModeProperty, value); }
		}

		/// <summary>
		/// Gets or sets the path to customize the appearance of the clear button in the NumericEntry control.
		/// </summary>
		/// <value>The default value is null.</value>
		/// <example>
		/// Below is an example of how to configure the <see cref="ClearButtonPath"/> property using XAML and C#:
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <numericEntry:SfNumericEntry
		///     x:Name="numericEntry"
		///     ClearButtonPath="M10,10 L20,20 L30,10" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// SfNumericEntry numericEntry = new SfNumericEntry();
		/// numericEntry.ClearButtonPath = "M10,10 L20,20 L30,10";
		/// this.Content = numericEntry;
		/// ]]></code>
		/// 
		/// ***
		/// </example>
		/// <value>The default value is null</value>
		public Path ClearButtonPath
		{
			get { return (Path)GetValue(ClearButtonPathProperty); }
			set { SetValue(ClearButtonPathProperty, value); }
		}

		/// <summary>
		/// Gets or sets the ReturnCommand to run when the user presses the return key, either physically or on the on-screen keyboard. 
		/// </summary>
		/// <value>Specifies the ReturnCommand. The default value is null.</value>
		/// <example>
		/// Below is an example of how to configure the <see cref="ReturnCommand"/> property using XAML and C#:
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <numericEntry:SfNumericEntry
		///     x:Name="numericEntry"
		///     ReturnCommand="{Binding OnReturnCommand}" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// SfNumericEntry numericEntry = new SfNumericEntry();
		/// numericEntry.ReturnCommand = new Command(() => Console.WriteLine("Return pressed"));
		/// this.Content = numericEntry;
		/// ]]></code>
		/// 
		/// ***
		/// </example>
		public ICommand ReturnCommand
		{
			get { return (ICommand)GetValue(ReturnCommandProperty); }
			set { SetValue(ReturnCommandProperty, value); }
		}

		/// <summary>
		/// Gets or sets the parameter object for the <see cref="ReturnCommand" /> that can be used to provide extra information. 
		/// </summary>
		///	<value>The default value is null</value>
		/// <example>
		/// Below is an example of how to configure the <see cref="ReturnCommandParameter"/> property using XAML and C#:
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <numericEntry:SfNumericEntry
		///     x:Name="numericEntry"
		///     ReturnCommand="{Binding OnReturnCommand}"
		///     ReturnCommandParameter="Numeric Entry Data" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// SfNumericEntry numericEntry = new SfNumericEntry();
		/// numericEntry.ReturnCommand = new Command((param) => Console.WriteLine($"Return pressed with: {param}"));
		/// numericEntry.ReturnCommandParameter = "Numeric Entry Data";
		/// this.Content = numericEntry;
		/// ]]></code>
		/// 
		/// ***
		/// </example>
		public object ReturnCommandParameter
		{
			get { return (object)GetValue(ReturnCommandParameterProperty); }
			set { SetValue(ReturnCommandParameterProperty, value); }
		}

		/// <summary>
		/// Gets or sets the numeric value of a <see cref="SfNumericEntry"/>.
		/// </summary>
		/// <value>
		/// The default value is <c>null</c>.
		/// </value>
		/// <example>
		/// Below is an example of how to configure the <see cref="Value"/> property using XAML and C#:
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <numericEntry:SfNumericEntry
		///     x:Name="numericEntry"
		///     Value="123.45" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// SfNumericEntry numericEntry = new SfNumericEntry();
		/// numericEntry.Value = 123.45;
		/// this.Content = numericEntry;
		/// ]]></code>
		/// 
		/// ***
		/// </example>
		public double? Value
		{
			get { return (double?)GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}

		/// <summary>
		/// Gets or sets the numerical minimum for <see cref="Value"/>.
		/// </summary>
		/// <value>The default value is <see cref="double.MinValue"/> </value>
		/// <example>
		/// Below is an example of how to configure the <see cref="Minimum"/> property using XAML and C#:
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <numericEntry:NumericEntry
		///     x:Name="numericEntry"
		///     Minimum="10" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// SfNumericEntry numericEntry = new SfNumericEntry();
		/// numericEntry.Minimum = 10;
		/// this.Content = numericEntry;
		/// ]]></code>
		/// 
		/// ***
		/// </example>
		public double Minimum
		{
			get { return (double)GetValue(MinimumProperty); }
			set { SetValue(MinimumProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether to show or hide the border in SfNumericEntry.
		/// </summary>
		/// <value>The default value is <c>true</c></value>
		/// <example>
		/// Below is an example of how to configure the <see cref="ShowBorder"/> property using XAML and C#:
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <numericEntry:SfNumericEntry
		///     x:Name="numericEntry"
		///     ShowBorder="False" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// SfNumericEntry numericEntry = new SfNumericEntry();
		/// numericEntry.ShowBorder = false;
		/// this.Content = numericEntry;
		/// ]]></code>
		/// 
		/// ***
		/// </example>
		public bool ShowBorder
		{
			get { return (bool)GetValue(ShowBorderProperty); }
			set { SetValue(ShowBorderProperty, value); }
		}

		/// <summary>
		/// Gets or sets the percent display mode that parses the entered text as percentage in specified mode.
		/// </summary>
		/// <value>
		/// The default value is <see cref="PercentDisplayMode.Compute"/>.
		/// </value>
		/// <remarks>
		/// This property does not work when a <c>CustomFormat</c> is applied to the control. 
		/// In such cases, the custom format takes precedence, and the percentage display mode is ignored.
		/// </remarks>
		/// <example>
		/// Below is an example of how to configure the <see cref="PercentDisplayMode"/> property using XAML and C#:
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <numericEntry:SfNumericEntry
		///     x:Name="numericEntry"
		///     PercentDisplayMode="Value" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// SfNumericEntry numericEntry = new SfNumericEntry();
		/// numericEntry.PercentDisplayMode = PercentDisplayMode.Value;
		/// this.Content = numericEntry;
		/// ]]></code>
		/// 
		/// ***
		/// </example>
		public PercentDisplayMode PercentDisplayMode
		{
			get { return (PercentDisplayMode)GetValue(PercentDisplayModeProperty); }
			set { SetValue(PercentDisplayModeProperty, value); }
		}

		/// <summary>
		/// Gets or sets the culture for <see cref="SfNumericEntry"/>.
		/// </summary>
		/// <value>
		/// The default value is <c>"en-US"</c>.
		/// </value>
		/// /// <example>
		/// Below is an example of how to configure the <see cref="Culture"/> property using XAML and C#:
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <numericEntry:SfNumericEntry
		///     x:Name="numericEntry"
		///     Culture="fr-FR" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// SfNumericEntry numericEntry = new SfNumericEntry();
		/// numericEntry.Culture = new System.Globalization.CultureInfo("fr-FR");
		/// this.Content = numericEntry;
		/// ]]></code>
		/// 
		/// ***
		/// </example>
		public CultureInfo Culture
		{
			get { return (CultureInfo)GetValue(CultureProperty); }
			set { SetValue(CultureProperty, value); }
		}

		/// <summary>
		/// Gets or sets the numerical maximum for <see cref="Value"/>.
		/// </summary>
		/// <value>The default value is <see cref="double.MaxValue"/>.</value>
		/// <example>
		/// Below is an example of how to configure the <see cref="Maximum"/> property using XAML and C#:
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <numericEntry:SfNumericEntry
		///     x:Name="numericEntry"
		///     Maximum="100" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// SfNumericEntry numericEntry = new SfNumericEntry();
		/// numericEntry.Maximum = 100;
		/// this.Content = numericEntry;
		/// ]]></code>
		/// 
		/// ***
		/// </example>
		public double Maximum
		{
			get { return (double)GetValue(MaximumProperty); }
			set { SetValue(MaximumProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value to change the horizontal alignment of text within the NumericEntry control.
		/// </summary>
		/// <value>Specifies the text alignment.The default value is <see cref="TextAlignment.Start"/>.</value>
		/// <example>
		/// Below is an example of how to configure the <see cref="HorizontalTextAlignment"/> property using XAML and C#:
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <numericEntry:SfNumericEntry
		///     x:Name="numericEntry"
		///     HorizontalTextAlignment="Center" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// SfNumericEntry numericEntry = new SfNumericEntry();
		/// numericEntry.HorizontalTextAlignment = TextAlignment.Center;
		/// this.Content = numericEntry;
		/// ]]></code>
		/// 
		/// ***
		/// </example>
		public TextAlignment HorizontalTextAlignment
		{
			get { return (TextAlignment)GetValue(HorizontalTextAlignmentProperty); }
			set { SetValue(HorizontalTextAlignmentProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value to change the vertical alignment of text within the NumericEntry control.
		/// </summary>
		/// <value>Specifies the text alignment.The default value is <see cref="TextAlignment.Center"/>.</value>
		/// <example>
		/// Below is an example of how to configure the <see cref="VerticalTextAlignment"/> property using XAML and C#:
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <numericEntry:SfNumericEntry
		///     x:Name="numericEntry"
		///     VerticalTextAlignment="Center" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// SfNumericEntry numericEntry = new SfNumericEntry();
		/// numericEntry.VerticalTextAlignment = TextAlignment.Center;
		/// this.Content = numericEntry;
		/// ]]></code>
		/// 
		/// ***
		/// </example>
		public TextAlignment VerticalTextAlignment
		{
			get { return (TextAlignment)GetValue(VerticalTextAlignmentProperty); }
			set { SetValue(VerticalTextAlignmentProperty, value); }
		}

		/// <summary>
		/// Gets or sets the value of ReturnType. This property can be used to set the ReturnType.
		/// </summary>
		/// <value>The default value is Default</value>
		/// <example>
		/// Below is an example of how to configure the <see cref="ReturnType"/> property using XAML and C#:
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <numericEntry:SfNumericEntry
		///     x:Name="numericEntry"
		///     ReturnType="Done" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// SfNumericEntry numericEntry = new SfNumericEntry();
		/// numericEntry.ReturnType = ReturnType.Done;
		/// this.Content = numericEntry;
		/// ]]></code>
		/// 
		/// ***
		/// </example>
		public ReturnType ReturnType
		{
			get { return (ReturnType)GetValue(ReturnTypeProperty); }
			set { SetValue(ReturnTypeProperty, value); }
		}

		/// <summary>
		/// Gets or sets the maximum number of decimal digits allowed in the numeric input.
		/// </summary>
		/// <value>The default value is 2.</value>
		/// <example>
		/// Below is an example of how to configure the <see cref="MaximumNumberDecimalDigits"/> property using XAML and C#:
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <numericEntry:SfNumericEntry
		///     x:Name="numericEntry"
		///     MaximumNumberDecimalDigits="1" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// SfNumericEntry numericEntry = new SfNumericEntry();
		/// numericEntry.MaximumNumberDecimalDigits = 1;
		/// this.Content = numericEntry;
		/// ]]></code>
		/// 
		/// ***
		/// </example>
		public int MaximumNumberDecimalDigits
		{
			get { return (int)GetValue(MaximumNumberDecimalDigitsProperty); }
			set { SetValue(MaximumNumberDecimalDigitsProperty, value); }
		}

		/// <summary>
		/// Gets or sets the format used to specify the formatting of <see cref="Value"/>.
		/// </summary>
		/// <value>
		/// The default value is <c>null</c>.
		/// </value>
		/// <example>
		/// Below is an example of how to configure the <see cref="CustomFormat"/> property using XAML and C#:
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <editor:SfNumericEntry
		///     x:Name="editor"
		///     CustomFormatter="#.# dollars" />
		/// ]]></code>
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// SfNumericEntry editor = new SfNumericEntry();
		/// editor.CustomFormatter = "#.# dollars";
		/// this.Content = editor;
		/// ]]></code>
		/// 
		/// To display a minimum of 2 decimal digits and maximum of 4 digits, with grouping:
		/// <code><![CDATA[
		/// editor.CustomFormatter = "#,0.00##";
		/// ]]></code>
		/// To remove group separator:
		/// <code><![CDATA[
		/// editor.CustomFormatter = "#,#.#";
		/// ]]></code>
		/// To display currency format:
		/// <code><![CDATA[
		/// editor.CustomFormatter = "C";
		/// ]]></code>
		/// To display percentage format, with a minimum of 2 decimal digits:
		/// <code><![CDATA[
		/// editor.CustomFormatter = "#.#%";
		/// ]]></code>
		/// </example>
		public string CustomFormat
		{
			get { return (string)GetValue(CustomFormatProperty); }
			set { SetValue(CustomFormatProperty, value); }
		}

		/// <summary>
		/// Gets or sets the text that is displayed in the control until the value is changed by a user action or some other operation.
		/// </summary>
		/// <value>The default value is <c>String.Empty</c></value>
		/// <example>
		/// Below is an example of how to configure the <see cref="Placeholder"/> property using XAML and C#:
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <numericEntry:SfNumericEntry
		///     x:Name="numericEntry"
		///     Placeholder="Enter a value" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// SfNumericEntry numericEntry = new SfNumericEntry();
		/// numericEntry.Placeholder = "Enter a value";
		/// this.Content = numericEntry;
		/// ]]></code>
		/// 
		/// ***
		/// </example>
		public string Placeholder
		{
			get { return (string)GetValue(PlaceholderProperty); }
			set { SetValue(PlaceholderProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether the <see cref="SfNumericEntry"/> allows null value or not.
		/// </summary>
		/// <value>
		/// <b>true</b> when <see cref="SfNumericEntry"/> allows null value input, otherwise <b>false</b>. The default value is <b>true</b>.
		/// </value>
		/// <example>
		/// Below is an example of how to configure the <see cref="AllowNull"/> property using XAML and C#:
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <numericEntry:SfNumericEntry
		///     x:Name="numericEntry"
		///     AllowNull="True" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// SfNumericEntry numericEntry = new SfNumericEntry();
		/// numericEntry.AllowNull = true;
		/// this.Content = numericEntry;
		/// ]]></code>
		/// 
		/// ***
		/// </example>
		public bool AllowNull
		{
			get { return (bool)GetValue(AllowNullProperty); }
			set { SetValue(AllowNullProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether the <see cref="SfNumericEntry"/> allows editing the value or not.
		/// </summary>
		/// <value>
		/// <b>true</b> when <see cref="SfNumericEntry"/> is editable, otherwise <b>false</b>. The default value is <b>true</b>.
		/// </value>
		/// <example>
		/// Below is an example of how to configure the <see cref="IsEditable"/> property using XAML and C#:
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <numericEntry:SfNumericEntry
		///     x:Name="numericEntry"
		///     IsEditable="False" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// SfNumericEntry numericEntry = new SfNumericEntry();
		/// numericEntry.IsEditable = false;
		/// this.Content = numericEntry;
		/// ]]></code>
		/// 
		/// ***
		/// </example>
		public bool IsEditable
		{
			get { return (bool)GetValue(IsEditableProperty); }
			set { SetValue(IsEditableProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether to show or hide the clear button in SfNumericEntry.
		/// </summary>
		/// <value>
		/// The default value is <c>true</c>.
		/// </value>
		/// <example>
		/// Below is an example of how to configure the <see cref="ShowClearButton"/> property using XAML and C#:
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <numericEntry:SfNumericEntry
		///     x:Name="numericEntry"
		///     ShowClearButton="False" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// SfNumericEntry numericEntry = new SfNumericEntry();
		/// numericEntry.ShowClearButton = false;
		/// this.Content = numericEntry;
		/// ]]></code>
		/// 
		/// ***
		/// </example>
		public bool ShowClearButton
		{
			get { return (bool)GetValue(ShowClearButtonProperty); }
			set { SetValue(ShowClearButtonProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether to show or hide the text box.
		/// </summary>
		/// <value>
		/// The default value is <see cref="Visibility.Visible"/>.
		/// </value>
		internal Visibility EntryVisibility
		{
			get { return (Visibility)GetValue(EntryVisibilityProperty); }
			set { SetValue(EntryVisibilityProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether the parent is text input layout.
		/// </summary>
		internal bool IsTextInputLayout
		{
			get;
			set;
		}

		/// <summary>
		/// Gets a value indicating whether the SemanticProperties.Description is not set by user.
		/// </summary>
		internal bool IsDescriptionNotSetByUser
		{
			get { return _isDescriptionNotSetByUser; }
			set { _isDescriptionNotSetByUser = value; }
		}

#if WINDOWS

		/// <summary>
		/// Gets or sets the number formatter used to format the numeric value.
		/// </summary>
		/// <value>
		/// An instance of INumberFormatter that defines how numbers should be formatted.
		/// </value>
		private INumberFormatter NumberFormatter
        {
            get { return (INumberFormatter)GetValue(NumberFormatterProperty); }
            set { SetValue(NumberFormatterProperty, value); }
        }
#endif

		#endregion

		#region Property Changed

		/// <summary>
		/// Called when the font is changed.
		/// </summary>
		/// <param name="oldValue">The old font value.</param>
		/// <param name="newValue">The new font value.</param>
		public void OnFontChanged(Microsoft.Maui.Font oldValue, Microsoft.Maui.Font newValue)
		{
			InvalidateDrawable();
		}

		/// <summary>
		/// Returns the default font size.
		/// </summary>
		/// <returns>Default font size value.</returns>
		double ITextElement.FontSizeDefaultValueCreator()
		{
			return 14d;
		}

		/// <summary>
		/// Invoked when the <see cref="FontAttributesProperty"/> changes.
		/// </summary>
		/// <param name="oldValue">The old value of the font attributes.</param>
		/// <param name="newValue">The new value of the font attributes.</param>
		void ITextElement.OnFontAttributesChanged(FontAttributes oldValue, FontAttributes newValue)
		{
			UpdateEntryProperties();
		}

		/// <summary>
		/// Invoked when the <see cref="FontFamilyProperty"/> changes.
		/// </summary>
		/// <param name="oldValue">The old font family value.</param>
		/// <param name="newValue">The new font family value.</param>
		void ITextElement.OnFontFamilyChanged(string oldValue, string newValue)
		{
			UpdateEntryProperties();
		}

		/// <summary>
		/// Invoked when the <see cref="FontSizeProperty"/> changes.
		/// </summary>
		/// <param name="oldValue">The old font size value.</param>
		/// <param name="newValue">The new font size value.</param>
		void ITextElement.OnFontSizeChanged(double oldValue, double newValue)
		{
			UpdateEntryProperties();
		}

		/// <summary>
		/// Invoked when the <see cref="FontAutoScalingEnabledProperty"/> changes.
		/// </summary>
		/// <param name="oldValue">The old auto-scaling enabled value.</param>
		/// <param name="newValue">The new auto-scaling enabled value.</param>
		void ITextElement.OnFontAutoScalingEnabledChanged(bool oldValue, bool newValue)
		{
			UpdateEntryProperties();
		}


		/// <summary>
		/// Invoked whenever the <see cref="StrokeProperty"/> is set.
		/// </summary>
		static void OnStrokePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfNumericEntry numericEntry)
			{
				numericEntry.InvalidateDrawable();
			}
		}

		/// <summary>
		/// Invoked whenever the <see cref="PlaceholderColorProperty"/> is set.
		/// </summary>
		/// <param name="bindable">The bindable.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		static void OnPlaceholderColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfNumericEntry numericEntry && numericEntry._textBox != null)
			{
				numericEntry._textBox.PlaceholderColor = (Color)newValue;
			}
		}

		/// <summary>
		/// Invoked whenever the <see cref="ClearButtonColorProperty"/> is set.
		/// </summary>
		/// <param name="bindable">The bindable.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		static void OnClearButtonColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfNumericEntry numericEntry)
			{
				numericEntry.InvalidateDrawable();
				numericEntry.UpdateTextInputLayoutUI();
			}
		}

		/// <summary>
		/// Invoked whenever the <see cref="TextColorProperty"/> is set.
		/// </summary>
		static void OnITextElementPropertyChanged(BindableObject bindable, object? oldValue, object newValue)
		{
			if (bindable is SfNumericEntry numericEntry)
			{
				numericEntry?.UpdateEntryProperties();
			}
		}

		/// <summary>
		/// Invoked whenever the <see cref="ValueChangeModeProperty"/> is set.
		/// </summary>
		static void OnValueChangeModePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfNumericEntry numericEntry)
			{
				numericEntry.InvalidateDrawable();
			}
		}

		/// <summary>
		/// Invoked whenever the <see cref="PercentDisplayModeProperty"/> is set.
		/// </summary>
		static void OnPercentDisplayModePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfNumericEntry numericEntry)
			{
				numericEntry.FormatValue();
			}
		}
		/// <summary>
		/// Invoked whenever the <see cref="ShowBorderProperty"/> is set.
		/// </summary>
		static void OnShowBorderPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfNumericEntry numericEntry)
			{
				numericEntry.InvalidateDrawable();
			}
		}

		/// <summary>
		/// Invoked whenever the <see cref="ClearButtonPathProperty"/> is set.
		/// </summary>
		/// <param name="bindable"></param>
		/// <param name="oldValue"></param>
		/// <param name="newValue"></param>
		static void OnClearButtonPathChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfNumericEntry numericEntry)
			{
				if (numericEntry._textInputLayout != null)
				{
					numericEntry.UpdateTextInputLayoutUI();
				}
				else
				{
					numericEntry.InvalidateDrawable();
				}
			}
		}

		/// <summary>
		/// Occurs when minimum property is changed.
		/// </summary>
		static void OnMinValueChanged(BindableObject bindable, object oldValue, object newValue)
		{
			SfNumericEntry numericEntry = (SfNumericEntry)bindable;

			if (numericEntry.Maximum < numericEntry.Minimum)
			{
				numericEntry.Maximum = numericEntry.Minimum;
			}

			if (numericEntry.Value < numericEntry.Minimum)
			{
				numericEntry.SetValue(ValueProperty, numericEntry.Minimum);
				numericEntry.FormatValue();
			}
			numericEntry.UpdateButtonColor(numericEntry.Value);

		}

		/// <summary>
		/// Occurs when maximum property is changed.
		/// </summary>
		static void OnMaxValueChanged(BindableObject bindable, object oldValue, object newValue)
		{
			SfNumericEntry numericEntry = (SfNumericEntry)bindable;

			if (numericEntry.Minimum > numericEntry.Maximum)
			{
				numericEntry.Minimum = numericEntry.Maximum;
			}

			if (numericEntry.Value > numericEntry.Maximum)
			{
				numericEntry.SetValue(ValueProperty, numericEntry.Maximum);
				numericEntry.FormatValue();
			}

			numericEntry.UpdateButtonColor(numericEntry.Value);
		}

		/// <summary>
		/// Occurs when custom format property is changed.
		/// </summary>
		static void OnCustomFormatChanged(BindableObject bindable, object oldValue, object newValue)
		{
			SfNumericEntry numericEntry = (SfNumericEntry)bindable;

			numericEntry.FormatValue();
			numericEntry.UpdateMaximumFractionDigit();
		}

#if WINDOWS
		/// <summary>
		/// Occurs when number formatter property is changed.
		/// </summary>
		static void OnNumberFormatChanged(BindableObject bindable, object oldValue, object newValue)
		{
			SfNumericEntry numericEntry = (SfNumericEntry)bindable;
			numericEntry.FormatValue();
			numericEntry.UpdateMaximumFractionDigit();
		}
#endif

		static void OnPlaceholderChanged(BindableObject bindable, object oldValue, object newValue)
		{
			SfNumericEntry numericEntry = (SfNumericEntry)bindable;
			numericEntry.UpdatePlaceHolderText((string)newValue);
		}

		/// <summary>
		/// Occurs when is editable property is changed.
		/// </summary>
		static void OnIsEditableChanged(BindableObject bindable, object oldValue, object newValue)
		{
			SfNumericEntry numericEntry = (SfNumericEntry)bindable;
			numericEntry.UpdateTextBoxEditability((bool)newValue);
		}

		/// <summary>
		/// Occurs when allow null property is changed.
		/// </summary>
		static void OnAllowNullValueChanged(BindableObject bindable, object oldValue, object newValue)
		{
			SfNumericEntry numericEntry = (SfNumericEntry)bindable;

			if (!numericEntry.AllowNull && (numericEntry.Value == null || double.IsNaN((double)numericEntry.Value)))
			{
				numericEntry.SetValue(ValueProperty, numericEntry.ValidateMinMax(0.0));
				numericEntry.FormatValue();
			}
		}

		/// <summary>
		/// Occurs when <see cref="EntryVisibility"/> property is changed.
		/// </summary>
		static void OnEntryVisibilityChanged(BindableObject bindable, object oldValue, object newValue)
		{
			SfNumericEntry numericEntry = (SfNumericEntry)bindable;
			numericEntry.UpdateEntryVisibility();
		}

		/// <summary>
		/// Occurs when <see cref="Culture"/> property is changed.
		/// </summary>
		static void OnCultureChanged(BindableObject bindable, object oldValue, object newValue)
		{
			SfNumericEntry numericEntry = (SfNumericEntry)bindable;
			numericEntry.FormatValue();
		}

		/// <summary>
		/// Property changed method for HorizontalTextAlignment property.
		/// </summary>
		/// <param name="bindable">The original source of property changed event.</param>
		/// <param name="oldValue">The old value of HorizontalTextAlignment property.</param>
		/// <param name="newValue">The new value of HorizontalTextAlignment property.</param>
		static void OnHorizontalTextAlignmentPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfNumericEntry numericEntry)
			{
				numericEntry?.UpdateEntryProperties();
			}
		}

		/// <summary>
		/// Property changed method for VerticalTextAlignment property.
		/// </summary>
		/// <param name="bindable">The original source of property changed event.</param>
		/// <param name="oldValue">The old value of VerticalTextAlignment property.</param>
		/// <param name="newValue">The new value of VerticalTextAlignment property.</param>
		static void OnVerticalTextAlignmentPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfNumericEntry numericEntry)
			{
				numericEntry?.UpdateEntryProperties();
			}
		}

		/// <summary>
		/// Occurs when MaximumNumberDecimalDigits property is changed.
		/// </summary>
		/// <param name="bindable"></param>
		/// <param name="oldValue"></param>
		/// <param name="newValue"></param>

		static void OnMaximumNumberDecimalDigitsChanged(BindableObject bindable, object oldValue, object newValue)
		{
			SfNumericEntry numericEntry = (SfNumericEntry)bindable;
			numericEntry.UpdateMaximumFractionDigit();
			numericEntry.FormatValue();
		}

		/// <summary>
		/// Occurs when ReturnType property is changed.
		/// </summary>
		/// <param name="bindable"></param>
		/// <param name="oldValue"></param>
		/// <param name="newValue"></param>
		static void OnReturnTypePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfNumericEntry numericEntry)
			{
				if (numericEntry != null && numericEntry._textBox != null)
				{
					numericEntry._textBox.ReturnType = numericEntry.ReturnType;
#if IOS
					numericEntry.UpdateReturnButtonText();
#endif
				}
			}
		}

		/// <summary>
		/// Property changed method for ReturnCommand property.
		/// </summary>
		/// <param name="bindable">The bindable.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		static void OnReturnCommandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfNumericEntry numericEntry && numericEntry._textBox != null)
			{
				numericEntry._textBox.ReturnCommand = numericEntry.ReturnCommand;
			}
		}

		/// <summary>
		/// Property changed method for ReturnCommandParameter property.
		/// </summary>
		/// <param name="bindable">The bindable.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		static void OnReturnCommandParameterPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfNumericEntry numericEntry && numericEntry._textBox != null)
			{
				numericEntry._textBox.ReturnCommandParameter = numericEntry.ReturnCommandParameter;
			}
		}

		/// <summary>
		/// Called when <see cref="ShowClearButton"/> property changed.
		/// </summary>
		static void OnShowClearButtonChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfNumericEntry numericEntry)
			{
				numericEntry.UpdateClearButtonVisibility();
				numericEntry.UpdateTextInputLayoutUI();
			}

		}

		/// <summary>
		/// Validates whether the entered text is within allowed constraints.
		/// </summary>
		/// <param name="sender">The object initiating the event.</param>
		/// <param name="e">Event arguments for validation.</param>
#if !ANDROID
        void OnTextBoxTextChanged(object? sender, Microsoft.Maui.Controls.TextChangedEventArgs e)
#elif ANDROID
		void OnTextBoxTextChanged(object? sender, Android.Text.TextChangedEventArgs e)
#endif
		{
			UpdateClearButtonVisibility();
#if ANDROID
			ValidateTextChanged(sender, e);
#endif
			if (ValueChangeMode == ValueChangeMode.OnKeyFocus && _textBox != null && _textBox.IsFocused)
			{
#if MACCATALYST || IOS
                string accentKeys = "`^ˆ˙´˳¨ʼ¯ˍ˝˚ˀ¸ˇ˘˜˛‸";
                GetValidText(accentKeys);
                if (_isNotValidText)
                {
                    return;
                }
                else if (string.IsNullOrEmpty(_textBox.Text))
                {
                    Value = Parse(_textBox.Text);
                }
                else if (double.TryParse(_textBox.Text, Culture, out _))
                {
                    Value = Parse(_textBox.Text);
                }
#endif

#if WINDOWS
                if (_textBox.Text != "-")
                {
                    Value = Parse(_textBox.Text);
                }
#elif ANDROID
				if (_textBox.Text != "-" && _textBox.Text != GetNumberDecimalSeparator(GetNumberFormat()))
				{
					if (_textBox.Text == "-" + GetNumberDecimalSeparator(GetNumberFormat()))
					{
						Value = AllowNull ? null : 0.0;
						return;
					}

					Value = Parse(_textBox.Text);
					_textBox.CursorPosition = _cursorPosition;
				}
#endif
			}
		}

		/// <summary>
		/// Invoked whenever the <see cref="ValueProperty"/> is changed.
		/// </summary>
		/// <param name="bindable">The object that owns the bindable property.</param>
		/// <param name="oldValue">The old value of the property.</param>
		/// <param name="newValue">The new value of the property.</param>
		static void OnValueChanged(BindableObject bindable, object oldValue, object newValue)
		{
			SfNumericEntry numericEntry = (SfNumericEntry)bindable;
			if (numericEntry._valueUpdating)
			{
				return;
			}
			numericEntry._valueUpdating = true;
			double? newValueAsDouble = ConvertToDouble(newValue, oldValue);

			double? old_Value = (double?)oldValue;

			if (newValueAsDouble.HasValue && !double.IsNaN(newValueAsDouble.Value))
			{
				newValueAsDouble = ValidateAndUpdateValue(numericEntry, newValueAsDouble.Value);
			}

			ValidateAndUpdateValue(numericEntry, newValueAsDouble);

			// Do not raise event when old and new value are equal.
			// After min and max validation, new and old value might be equal.
			if (old_Value != newValueAsDouble)
			{
				if (numericEntry.ValueChanged != null)
				{
					RaiseValueChangedEvent(numericEntry, old_Value, newValueAsDouble);
				}
			}

			UpdateTextBoxAndButtonColors(numericEntry, newValueAsDouble);
		}

		#endregion
	}
}
