using Syncfusion.Maui.Toolkit.Graphics.Internals;
using Syncfusion.Maui.Toolkit.Themes;
using ITextElement = Syncfusion.Maui.Toolkit.Graphics.Internals.ITextElement;

namespace Syncfusion.Maui.Toolkit.TabView
{
	/// <summary>
	/// Defines settings to customize the appearance and behavior of the center button.
	/// </summary>
	public class CenterButtonSettings : Element, ITextElement, IThemeElement
	{
		#region Bindable properties
		/// <summary>
		/// Identifies the <see cref="Title"/> bindable property.
		/// </summary>
		public static readonly BindableProperty TitleProperty =
			BindableProperty.Create(
				nameof(Title),
				typeof(string),
				typeof(CenterButtonSettings),
				null);

		/// <summary>
		/// Identifies the <see cref="Height"/> bindable property.
		/// </summary>
		public static readonly BindableProperty HeightProperty =
			BindableProperty.Create(
				nameof(Height),
				typeof(double),
				typeof(CenterButtonSettings),
				48d);

		/// <summary>
		/// Identifies the <see cref="Width"/> bindable property.
		/// </summary>
		public static readonly BindableProperty WidthProperty =
			BindableProperty.Create(
				nameof(Width),
				typeof(double),
				typeof(CenterButtonSettings),
				70d);

		/// <summary>
		/// Identifies the <see cref="Background"/> bindable property.
		/// </summary>
		public static readonly BindableProperty BackgroundProperty =
			BindableProperty.Create(
				nameof(Background),
				typeof(Brush),
				typeof(CenterButtonSettings),
				null);

		/// <summary>
		/// Identifies the <see cref="Stroke"/> bindable property.
		/// </summary>
		public static readonly BindableProperty StrokeProperty =
			BindableProperty.Create(
				nameof(Stroke),
				typeof(Color),
				typeof(CenterButtonSettings),
				null);

		/// <summary>
		/// Identifies the <see cref="StrokeThickness"/> bindable property.
		/// </summary>
		public static readonly BindableProperty StrokeThicknessProperty =
			BindableProperty.Create(
				nameof(StrokeThickness),
				typeof(double),
				typeof(CenterButtonSettings),
				0d);

		/// <summary>
		/// Identifies the <see cref="CornerRadius"/> bindable property.
		/// </summary>
		public static readonly BindableProperty CornerRadiusProperty =
			BindableProperty.Create(
				nameof(CornerRadius),
				typeof(CornerRadius),
				typeof(CenterButtonSettings),
				new CornerRadius(0));

		/// <summary>
		/// Identifies the <see cref="TextColor"/> bindable property.
		/// </summary>
		public static readonly BindableProperty TextColorProperty =
			BindableProperty.Create(
				nameof(TextColor),
				typeof(Color),
				typeof(CenterButtonSettings),
				Color.FromArgb("#49454F"));

		/// <summary>
		/// Identifies the <see cref="ImageSource"/> bindable property.
		/// </summary>
		public static readonly BindableProperty ImageSourceProperty =
			BindableProperty.Create(
				nameof(ImageSource),
				typeof(ImageSource),
				typeof(CenterButtonSettings),
				null);

		/// <summary>
		/// Identifies the <see cref="ImageSize"/> bindable property.
		/// </summary>
		public static readonly BindableProperty ImageSizeProperty =
			BindableProperty.Create(
				nameof(ImageSize),
				typeof(double),
				typeof(CenterButtonSettings),
				20d);

		/// <summary>
		/// Identifies the <see cref="DisplayMode"/> bindable property.
		/// </summary>
		public static readonly BindableProperty DisplayModeProperty =
			BindableProperty.Create(
				nameof(DisplayMode),
				typeof(CenterButtonDisplayMode),
				typeof(CenterButtonSettings),
				CenterButtonDisplayMode.Text);

		/// <summary>
		/// Identifies the <see cref="FontFamily"/> bindable property.
		/// </summary>
		public static readonly BindableProperty FontFamilyProperty =
			FontElement.FontFamilyProperty;

		/// <summary>
		/// Identifies the <see cref="FontAttributes"/> bindable property.
		/// </summary>
		public static readonly BindableProperty FontAttributesProperty =
			FontElement.FontAttributesProperty;

		/// <summary>
		/// Identifies the <see cref="FontSize"/> bindable property.
		/// </summary>
		public static readonly BindableProperty FontSizeProperty =
			FontElement.FontSizeProperty;

		/// <summary>
		/// Identifies the <see cref="FontAutoScalingEnabled"/> bindable property.
		/// </summary>
		public static readonly BindableProperty FontAutoScalingEnabledProperty =
			FontElement.FontAutoScalingEnabledProperty;
		#endregion

		#region Properties
		/// <summary> 
		/// Gets or sets a value that can be used to display title on the center button.
		/// </summary> 
		/// <value> 
		/// It accepts the string value.
		/// </value> 
		public string Title
		{
			get => (string)GetValue(TitleProperty);
			set => SetValue(TitleProperty, value);
		}

		/// <summary> 
		/// Gets or sets a value that can be used to adjust the height of the center button.
		/// </summary> 
		/// <value>
		/// It accepts the double value, and the default value is 48.
		/// </value>
		public double Height
		{
			get => (double)GetValue(HeightProperty);
			set => SetValue(HeightProperty, value);
		}

		/// <summary> 
		/// Gets or sets a value that can be used to adjust the width of the center button.
		/// </summary> 
		/// <value> 
		/// It accepts the double value, and the default value is 70.
		/// </value> 
		public double Width
		{
			get => (double)GetValue(WidthProperty);
			set => SetValue(WidthProperty, value);
		}

		/// <summary> 
		/// Gets or sets a value that can be used to customize the background color of the center button.
		/// </summary> 
		/// <value> 
		/// It accepts Brush values.
		/// </value> 
		public Brush Background
		{
			get => (Brush)GetValue(BackgroundProperty);
			set => SetValue(BackgroundProperty, value);
		}

		/// <summary> 
		/// Gets or sets a value that can be used to customize the border color of the center button.
		/// </summary> 
		/// <value> 
		/// It accepts color values.
		/// </value> 
		public Color Stroke
		{
			get => (Color)GetValue(StrokeProperty);
			set => SetValue(StrokeProperty, value);
		}

		/// <summary>  
		/// Gets or sets a value that can be used to customize the border width of the center button.
		/// </summary> 
		/// <value> 
		/// It accepts double values, and the default value is 0.
		/// </value> 
		public double StrokeThickness
		{
			get => (double)GetValue(StrokeThicknessProperty);
			set => SetValue(StrokeThicknessProperty, value);
		}

		/// <summary>
		/// Gets or sets a value that can be used to customize the corner radius of the center button.
		/// </summary> 
		/// <value> 
		/// It accepts CornerRadius values, and the default value is 0.
		/// </value> 
		public CornerRadius CornerRadius
		{
			get => (CornerRadius)GetValue(CornerRadiusProperty);
			set => SetValue(CornerRadiusProperty, value);
		}

		/// <summary> 
		/// Gets or sets font family to the center button’s title.
		/// </summary> 
		/// <value> 
		/// It accepts string values, and the default value is null.
		/// </value> 
		public string FontFamily
		{
			get => (string)GetValue(FontFamilyProperty);
			set => SetValue(FontFamilyProperty, value);
		}

		/// <summary> 
		/// Gets or sets font attributes to the center button’s title.
		/// </summary> 
		/// <value> 
		/// It accepts FontAttributes values, and the default value is None.
		/// </value> 
		public FontAttributes FontAttributes
		{
			get => (FontAttributes)GetValue(FontAttributesProperty);
			set => SetValue(FontAttributesProperty, value);
		}

		/// <summary> 
		/// Gets or sets a value that determines whether the font of the center button should scale automatically according to the operating system settings.
		/// </summary>
		public bool FontAutoScalingEnabled
		{
			get { return (bool)GetValue(FontAutoScalingEnabledProperty); }
			set { SetValue(FontAutoScalingEnabledProperty, value); }
		}

		/// <summary> 
		/// Gets or sets a value that can be used to customize the text size of the center button’s title.
		/// </summary> 
		/// <value> 
		/// It accepts double values, and the default value is 14.
		/// </value> 
		[System.ComponentModel.TypeConverter(typeof(FontSizeConverter))]
		public double FontSize
		{
			get => (double)GetValue(FontSizeProperty);
			set => SetValue(FontSizeProperty, value);
		}

		/// <summary> 
		/// Gets or sets a value that can be used to customize the text color of the center button.
		/// </summary> 
		/// <value> 
		/// It accepts color values.
		/// </value> 
		public Color TextColor
		{
			get => (Color)GetValue(TextColorProperty);
			set => SetValue(TextColorProperty, value);
		}

		/// <summary> 
		/// Gets or sets a value that can be used to display image on the center button.
		/// </summary> 
		/// <value> 
		/// It accepts the ImageSource value.
		/// </value> 
		public ImageSource ImageSource
		{
			get => (ImageSource)GetValue(ImageSourceProperty);
			set => SetValue(ImageSourceProperty, value);
		}

		/// <summary> 
		/// Gets or sets a value that can be used to customize the image size of the center button.
		/// </summary> 
		/// <value> 
		/// It accepts double values, and the default value is 20.
		/// </value> 
		public double ImageSize
		{
			get => (double)GetValue(ImageSizeProperty);
			set => SetValue(ImageSizeProperty, value);
		}

		/// <summary> 
		/// Gets or sets a value to choose the display type of the center button.
		/// </summary> 
		/// <value> 
		/// It accepts CenterButtonDisplayMode values, and the default value is Text.
		/// </value> 
		public CenterButtonDisplayMode DisplayMode
		{
			get => (CenterButtonDisplayMode)GetValue(DisplayModeProperty);
			set => SetValue(DisplayModeProperty, value);
		}

		/// <summary>
		/// Gets the font of the center button.
		/// </summary>
		public Microsoft.Maui.Font Font { get; }
		#endregion

		#region Constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="CenterButtonSettings"/> class.
		/// </summary>
		public CenterButtonSettings()
		{
			ThemeElement.InitializeThemeResources(this, "SfTabViewTheme");
		}
		#endregion

		#region Interface Implementation
		/// <summary>
		/// Property changed method for font family.
		/// </summary>
		void ITextElement.OnFontFamilyChanged(string oldValue, string newValue)
		{
			// This method is intentionally left empty because the font family change
			// does not require any additional handling in this implementation.
			// It is required by the ITextElement interface.
		}

		/// <summary>
		/// Property changed method for font size.
		/// </summary>
		void ITextElement.OnFontSizeChanged(double oldValue, double newValue)
		{
			// This method is intentionally left empty because the font size change
			// does not require any additional handling in this implementation.
			// It is required by the ITextElement interface.
		}


		/// <summary>
		/// Invoked when the FontAutoScalingEnabled property changes.
		/// </summary>
		void ITextElement.OnFontAutoScalingEnabledChanged(bool oldValue, bool newValue)
		{
			// This method is intentionally left empty because the font auto-scaling enabled change
			// does not require any additional handling in this implementation.
			// It is required by the ITextElement interface.
		}

		/// <summary>
		/// Returns the default font value.
		/// </summary>
		/// <returns>
		/// Method returns the default font size value.
		/// </returns>
		double ITextElement.FontSizeDefaultValueCreator()
		{
			return 14d;
		}

		/// <summary>
		/// Method used to handle font attributes changes.
		/// </summary>
		void ITextElement.OnFontAttributesChanged(FontAttributes oldValue, FontAttributes newValue)
		{
			// This method is intentionally left empty because the font attributes change
			// does not require any additional handling in this implementation.
			// It is required by the ITextElement interface.
		}

		/// <summary>
		/// Property changed method for font.
		/// </summary>
		void ITextElement.OnFontChanged(Microsoft.Maui.Font oldValue, Microsoft.Maui.Font newValue)
		{
			// This method is intentionally left empty because the font change
			// does not require any additional handling in this implementation.
			// It is required by the ITextElement interface.
		}

		/// <summary>
		/// Handles changes to the control theme.
		/// </summary>
		void IThemeElement.OnControlThemeChanged(string oldTheme, string newTheme)
		{
			// This method is intentionally left empty.
			// It is required by the IThemeElement interface.
		}

		/// <summary>
		/// Handles changes to the common theme.
		/// </summary>
		void IThemeElement.OnCommonThemeChanged(string oldTheme, string newTheme)
		{
			// This method is intentionally left empty.
			// It is required by the IThemeElement interface.
		}

		#endregion
	}
}
