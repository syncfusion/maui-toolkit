using Syncfusion.Maui.Toolkit.Themes;

namespace Syncfusion.Maui.Toolkit.Popup
{
	/// <summary>
	/// Represents the style of <see cref="SfPopup"/>.
	/// </summary>
	public class PopupStyle : Element, IThemeElement
	{
		#region Bindable Properties

		/// <summary>
		/// Identifies the <see cref="OverlayColor"/> <see cref="BindableProperty"/>.
		/// </summary>
		public static readonly BindableProperty OverlayColorProperty =
			BindableProperty.Create(nameof(OverlayColor), typeof(Brush), typeof(PopupStyle), new SolidColorBrush(Color.FromArgb("#80000000")), BindingMode.Default, null);

		/// <summary>
		/// Identifies the <see cref="HeaderBackground"/> <see cref="BindableProperty"/>.
		/// </summary>
		public static readonly BindableProperty HeaderBackgroundProperty =
			BindableProperty.Create(nameof(HeaderBackground), typeof(Brush), typeof(PopupStyle), new SolidColorBrush(Color.FromArgb("#00FFFFFF")));

		/// <summary>
		/// Identifies the <see cref="HeaderTextColor"/> <see cref="BindableProperty"/>.
		/// </summary>
		public static readonly BindableProperty HeaderTextColorProperty =
			BindableProperty.Create(nameof(HeaderTextColor), typeof(Color), typeof(PopupStyle), Color.FromArgb("#1C1B1F"));

		/// <summary>
		/// Identifies the <see cref="MessageTextColor"/> <see cref="BindableProperty"/>.
		/// </summary>
		public static readonly BindableProperty MessageTextColorProperty =
			BindableProperty.Create(nameof(MessageTextColor), typeof(Color), typeof(PopupStyle), Color.FromArgb("#49454F"));

		/// <summary>
		/// Identifies the <see cref="MessageBackground"/> <see cref="BindableProperty"/>.
		/// </summary>
		public static readonly BindableProperty MessageBackgroundProperty =
			BindableProperty.Create(nameof(MessageBackground), typeof(Brush), typeof(PopupStyle), new SolidColorBrush(Color.FromArgb("#00FFFFFF")));

		/// <summary>
		/// Identifies the <see cref="PopupBackground"/> <see cref="BindableProperty"/>.
		/// </summary>
		public static readonly BindableProperty PopupBackgroundProperty =
			BindableProperty.Create(nameof(PopupBackground), typeof(Brush), typeof(PopupStyle), new SolidColorBrush(Color.FromArgb("#EEE8F4")));

		/// <summary>
		/// Identifies the <see cref="MessageFontSize"/> <see cref="BindableProperty"/>.
		/// </summary>
		public static readonly BindableProperty MessageFontSizeProperty =
			BindableProperty.Create(nameof(MessageFontSize), typeof(double), typeof(PopupStyle), 14d);

		/// <summary>
		/// Identifies the <see cref="MessageFontFamily"/> <see cref="BindableProperty"/>.
		/// </summary>
		public static readonly BindableProperty MessageFontFamilyProperty =
			BindableProperty.Create(nameof(MessageFontFamily), typeof(string), typeof(PopupStyle), default(string));

		/// <summary>
		/// Identifies the <see cref="MessageFontAttribute"/> <see cref="BindableProperty"/>.
		/// </summary>
		public static readonly BindableProperty MessageFontAttributeProperty =
			BindableProperty.Create(nameof(MessageFontAttribute), typeof(FontAttributes), typeof(PopupStyle), FontAttributes.None);

		/// <summary>
		/// Identifies the <see cref="MessageTextAlignment"/> <see cref="BindableProperty"/>.
		/// </summary>
		public static readonly BindableProperty MessageTextAlignmentProperty =
			BindableProperty.Create(nameof(MessageTextAlignment), typeof(TextAlignment), typeof(PopupStyle), TextAlignment.Start);

		/// <summary>
		/// Identifies the <see cref="FooterBackground"/> <see cref="BindableProperty"/>.
		/// </summary>
		public static readonly BindableProperty FooterBackgroundProperty =
			BindableProperty.Create(nameof(FooterBackground), typeof(Brush), typeof(PopupStyle), new SolidColorBrush(Color.FromArgb("#00FFFFFF")), BindingMode.Default, null);

		/// <summary>
		/// Identifies the <see cref="FooterFontSize"/> <see cref="BindableProperty"/>.
		/// </summary>
		public static readonly BindableProperty FooterFontSizeProperty =
			BindableProperty.Create(nameof(FooterFontSize), typeof(double), typeof(PopupStyle), 14d);

		/// <summary>
		/// Identifies the <see cref="FooterFontFamily"/> <see cref="BindableProperty"/>.
		/// </summary>
		public static readonly BindableProperty FooterFontFamilyProperty =
			BindableProperty.Create(nameof(FooterFontFamily), typeof(string), typeof(PopupStyle), default(string));

		/// <summary>
		/// Identifies the <see cref="FooterFontAttribute"/> <see cref="BindableProperty"/>.
		/// </summary>
		public static readonly BindableProperty FooterFontAttributeProperty =
			BindableProperty.Create(nameof(FooterFontAttribute), typeof(FontAttributes), typeof(PopupStyle), FontAttributes.None);

		/// <summary>
		/// Identifies the <see cref="AcceptButtonBackground"/> <see cref="BindableProperty"/>.
		/// </summary>
		public static readonly BindableProperty AcceptButtonBackgroundProperty =
			BindableProperty.Create(nameof(AcceptButtonBackground), typeof(Brush), typeof(PopupStyle), new SolidColorBrush(Colors.Transparent));

		/// <summary>
		/// Identifies the <see cref="AcceptButtonTextColor"/> <see cref="BindableProperty"/>.
		/// </summary>
		public static readonly BindableProperty AcceptButtonTextColorProperty =
			BindableProperty.Create(nameof(AcceptButtonTextColor), typeof(Color), typeof(PopupStyle), Color.FromArgb("#6750A4"));

		/// <summary>
		/// Identifies the <see cref="DeclineButtonTextColor"/> <see cref="BindableProperty"/>.
		/// </summary>
		public static readonly BindableProperty DeclineButtonTextColorProperty =
			BindableProperty.Create(nameof(DeclineButtonTextColor), typeof(Color), typeof(PopupStyle), Color.FromArgb("#6750A4"));

		/// <summary>
		/// Identifies the <see cref="DeclineButtonBackground"/> <see cref="BindableProperty"/>.
		/// </summary>
		public static readonly BindableProperty DeclineButtonBackgroundProperty =
			BindableProperty.Create(nameof(DeclineButtonBackground), typeof(Brush), typeof(PopupStyle), new SolidColorBrush(Colors.Transparent));

		/// <summary>
		/// Identifies the <see cref="Stroke"/> <see cref="BindableProperty"/>.
		/// </summary>
		public static readonly BindableProperty StrokeProperty =
			BindableProperty.Create(nameof(Stroke), typeof(Color), typeof(PopupStyle), Colors.Transparent);

		/// <summary>
		/// Identifies the <see cref="HeaderFontSize"/> <see cref="BindableProperty"/>.
		/// </summary>
		public static readonly BindableProperty HeaderFontSizeProperty =
			BindableProperty.Create(nameof(HeaderFontSize), typeof(double), typeof(PopupStyle), 24d);

		/// <summary>
		/// Identifies the <see cref="HeaderFontFamily"/> <see cref="BindableProperty"/>.
		/// </summary>
		public static readonly BindableProperty HeaderFontFamilyProperty =
			BindableProperty.Create(nameof(HeaderFontFamily), typeof(string), typeof(PopupStyle), default(string));

		/// <summary>
		/// Identifies the <see cref="HeaderFontAttribute"/> <see cref="BindableProperty"/>.
		/// </summary>
		public static readonly BindableProperty HeaderFontAttributeProperty =
			BindableProperty.Create(nameof(HeaderFontAttribute), typeof(FontAttributes), typeof(PopupStyle), FontAttributes.None);

		/// <summary>
		/// Identifies the <see cref="HeaderTextAlignment"/> <see cref="BindableProperty"/>.
		/// </summary>
		public static readonly BindableProperty HeaderTextAlignmentProperty =
			BindableProperty.Create(nameof(HeaderTextAlignment), typeof(TextAlignment), typeof(PopupStyle), TextAlignment.Start);

		/// <summary>
		/// Identifies the <see cref="StrokeThickness"/> <see cref="BindableProperty"/>.
		/// </summary>
		public static readonly BindableProperty StrokeThicknessProperty =
			BindableProperty.Create(nameof(StrokeThickness), typeof(int), typeof(PopupStyle), 0);

		/// <summary>
		/// Identifies the <see cref="HasShadow"/> <see cref="BindableProperty"/>.
		/// </summary>
		public static readonly BindableProperty HasShadowProperty =
			BindableProperty.Create(nameof(HasShadow), typeof(bool), typeof(PopupStyle), false);

		/// <summary>
		/// Identifies the <see cref="BlurIntensity"/> <see cref="BindableProperty"/>.
		/// </summary>
		public static readonly BindableProperty BlurIntensityProperty =
			BindableProperty.Create(nameof(BlurIntensity), typeof(PopupBlurIntensity), typeof(PopupStyle), PopupBlurIntensity.Light);

		/// <summary>
		/// Identifies the <see cref="BlurRadius"/> <see cref="BindableProperty"/>.
		/// </summary>
		public static readonly BindableProperty BlurRadiusProperty =
			BindableProperty.Create(nameof(BlurRadius), typeof(float), typeof(PopupStyle), 5f);

		/// <summary>
		/// Identifies the <see cref="CornerRadius"/> <see cref="BindableProperty"/>.
		/// </summary>
		public static readonly BindableProperty CornerRadiusProperty =
			BindableProperty.Create(nameof(CornerRadius), typeof(CornerRadius), typeof(PopupStyle), new CornerRadius(28));

		/// <summary>
		/// Identifies the <see cref="FooterButtonCornerRadius"/> <see cref="BindableProperty"/>.
		/// </summary>
		public static readonly BindableProperty FooterButtonCornerRadiusProperty =
			BindableProperty.Create(nameof(FooterButtonCornerRadius), typeof(CornerRadius), typeof(PopupStyle), new CornerRadius(20));

		/// <summary>
		/// Identifies the <see cref="CloseButtonIcon"/> <see cref="BindableProperty"/>.
		/// </summary>
		public static readonly BindableProperty CloseButtonIconProperty =
			BindableProperty.Create(nameof(CloseButtonIcon), typeof(ImageSource), typeof(PopupStyle), null);

		/// <summary>
		/// Identifies the <see cref="CloseButtonIconStroke"/> <see cref="BindableProperty"/>.
		/// </summary>
		internal static BindableProperty CloseButtonIconStrokeProperty =
			BindableProperty.Create(nameof(CloseButtonIconStroke), typeof(Color), typeof(PopupStyle), defaultValue: Color.FromArgb("#49454F"));

		/// <summary>
		/// Identifies the <see cref="CloseButtonIconStrokeThickness"/> <see cref="BindableProperty"/>.
		/// </summary>
		internal static BindableProperty CloseButtonIconStrokeThicknessProperty =
		 BindableProperty.Create(nameof(CloseButtonIconStrokeThickness), typeof(double), typeof(PopupStyle), defaultValue: 1.8d);

		/// <summary>
		/// Identifies the <see cref="HoveredCloseButtonIconBackground"/> <see cref="BindableProperty"/>.
		/// </summary>
		internal static BindableProperty HoveredCloseButtonIconBackgroundProperty =
			BindableProperty.Create(nameof(HoveredCloseButtonIconBackground), typeof(Color), typeof(PopupStyle), defaultValue: Color.FromArgb("#1449454F"));

		/// <summary>
		/// Identifies the <see cref="PressedCloseButtonIconBackground"/> <see cref="BindableProperty"/>.
		/// </summary>
		internal static BindableProperty PressedCloseButtonIconBackgroundProperty =
		   BindableProperty.Create(nameof(PressedCloseButtonIconBackground), typeof(Color), typeof(PopupStyle), defaultValue: Color.FromArgb("#00000000"));

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets the overlay color when PopupView is displayed.
		/// </summary>
		/// <value>
		/// The overlay color displayed when the PopupView is shown. Default is transparent.
		/// </value>
		/// <remarks>
		/// Opacity of the <see cref="OverlayColor"/> can be customized using Alpha value.
		/// </remarks>
		/// <example>
		/// <para>The following code example demonstrates how to apply opacity with <see cref="OverlayColor"/>.</para>
		/// <code lang="C#">
		/// <![CDATA[
		/// using System.ComponentModel;
		///
		/// namespace PopupMAUI
		/// {
		///     public partial class MainPage : ContentPage
		///     {
		///         public MainPage()
		///         {
		///            InitializeComponent();
		///         }
		///         void clickToShowPopup_Clicked(object sender, EventArgs e)
		///         {
		///             (BindingContext as ViewModel).IsOpen = true;
		///         }
		///      }
		/// }
		/// ]]>
		/// </code>
		/// <code lang="XAML">
		/// <![CDATA[
		/// <?xml version = "1.0" encoding="utf-8" ?>
		/// <ContentPage xmlns = "http://schemas.microsoft.com/dotnet/2021/maui"
		///     xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
		///     xmlns:syncfusion="clr-namespace:Syncfusion.Maui.Toolkit.Popup;assembly=Syncfusion.Maui.Toolkit"
		///     xmlns:local="clr-namespace:PopupMAUI"
		///     x:Class="PopupMAUI.MainPage">
		/// <ContentPage.BindingContext>
		///     <local:ViewModel/>
		/// </ContentPage.BindingContext>
		/// <ContentPage.Content>
		///    <StackLayout WidthRequest = "500" >
		///        <syncfusion:SfPopup x:Name="popup" IsOpen="{Binding IsOpen}">
		///                    <syncfusion:SfPopup.PopupStyle>
		///                         <syncfusion:PopupStyle OverlayColor="rgba(0,0,0,0.5)"/>
		///                     </syncfusion:SfPopup.PopupStyle>
		///        </syncfusion:SfPopup>
		///        <Button x:Name="clickToShowPopup" Text="Click To Show Popup" Clicked="clickToShowPopup_Clicked"/>
		///    </StackLayout>
		/// </ContentPage.Content>
		/// </ContentPage>
		/// ]]>
		/// </code>
		/// </example>
		/// <seealso cref="SfPopup.ShowOverlayAlways"/>
		/// <seealso cref="SfPopup.OverlayMode"/>

		public Brush OverlayColor
		{
			get => (Brush)GetValue(OverlayColorProperty);
			set => SetValue(OverlayColorProperty, value);
		}

		/// <summary>
		/// Gets or sets the background color to be applied for the header.
		/// </summary>
		/// <seealso cref="FooterBackground"/>
		/// <seealso cref="MessageBackground"/>
		public Brush HeaderBackground
		{
			get => (Brush)GetValue(HeaderBackgroundProperty);
			set => SetValue(HeaderBackgroundProperty, value);
		}

		/// <summary>
		/// Gets or sets the text color to be applied for the header title.
		/// </summary>
		/// <seealso cref="MessageTextColor"/>
		public Color HeaderTextColor
		{
			get => (Color)GetValue(HeaderTextColorProperty);
			set => SetValue(HeaderTextColorProperty, value);
		}

		/// <summary>
		/// Gets or sets the background color of content.
		/// </summary>
		/// <seealso cref="HeaderBackground"/>
		/// <seealso cref="FooterBackground"/>
		public Brush MessageBackground
		{
			get => (Brush)GetValue(MessageBackgroundProperty);
			set => SetValue(MessageBackgroundProperty, value);
		}

		/// <summary>
		/// Gets or sets the background color of the PopupView.
		/// </summary>
		public Brush PopupBackground
		{
			get => (Brush)GetValue(PopupBackgroundProperty);
			set => SetValue(PopupBackgroundProperty, value);
		}

		/// <summary>
		/// Gets or sets the foreground color of content.
		/// </summary>
		/// <seealso cref="HeaderTextColor"/>
		public Color MessageTextColor
		{
			get => (Color)GetValue(MessageTextColorProperty);
			set => SetValue(MessageTextColorProperty, value);
		}

		/// <summary>
		/// Gets or sets the font size of the content.
		/// </summary>
		/// <seealso cref="HeaderFontSize"/>
		/// <seealso cref="FooterFontSize"/>
		public double MessageFontSize
		{
			get => (double)GetValue(MessageFontSizeProperty);
			set => SetValue(MessageFontSizeProperty, value);
		}

		/// <summary>
		/// Gets or sets the font style to be applied for the content.
		/// </summary>
		/// <seealso cref="HeaderFontFamily"/>
		/// <seealso cref="FooterFontFamily"/>
		public string MessageFontFamily
		{
			get => (string)GetValue(MessageFontFamilyProperty);
			set => SetValue(MessageFontFamilyProperty, value);
		}

		/// <summary>
		/// Gets or sets the font attribute to be applied for the content.
		/// </summary>
		/// <seealso cref="HeaderFontAttribute"/>
		/// <seealso cref="FooterFontAttribute"/>
		public FontAttributes MessageFontAttribute
		{
			get => (FontAttributes)GetValue(MessageFontAttributeProperty);
			set => SetValue(MessageFontAttributeProperty, value);
		}

		/// <summary>
		/// Gets or sets the text alignment of the content.
		/// </summary>
		/// <seealso cref="HeaderTextAlignment"/>
		public TextAlignment MessageTextAlignment
		{
			get => (TextAlignment)GetValue(MessageTextAlignmentProperty);
			set => SetValue(MessageTextAlignmentProperty, value);
		}

		/// <summary>
		/// Gets or sets the background color of the <see cref="SfPopup"/> footer.
		/// </summary>
		/// <seealso cref="HeaderBackground"/>
		/// <seealso cref="MessageBackground"/>
		public Brush FooterBackground
		{
			get => (Brush)GetValue(FooterBackgroundProperty);
			set => SetValue(FooterBackgroundProperty, value);
		}

		/// <summary>
		/// Gets or sets the font size of the footer buttons.
		/// </summary>
		/// <seealso cref="HeaderFontSize"/>
		/// <seealso cref="MessageFontSize"/>
		public double FooterFontSize
		{
			get => (double)GetValue(FooterFontSizeProperty);
			set => SetValue(FooterFontSizeProperty, value);
		}

		/// <summary>
		/// Gets or sets the font style to be applied for the footer buttons.
		/// </summary>
		/// <seealso cref="HeaderFontFamily"/>
		/// <seealso cref="MessageFontFamily"/>
		public string FooterFontFamily
		{
			get => (string)GetValue(FooterFontFamilyProperty);
			set => SetValue(FooterFontFamilyProperty, value);
		}

		/// <summary>
		/// Gets or sets the font attribute to be applied for the footer buttons.
		/// </summary>
		/// <seealso cref="HeaderFontAttribute"/>
		/// <seealso cref="MessageFontAttribute"/>
		public FontAttributes FooterFontAttribute
		{
			get => (FontAttributes)GetValue(FooterFontAttributeProperty);
			set => SetValue(FooterFontAttributeProperty, value);
		}

		/// <summary>
		/// Gets or sets the background color of accept button in the footer.
		/// </summary>
		/// <seealso cref="HeaderBackground"/>
		/// <seealso cref="FooterBackground"/>
		/// <seealso cref="MessageBackground"/>
		/// <seealso cref="DeclineButtonBackground"/>
		public Brush AcceptButtonBackground
		{
			get => (Brush)GetValue(AcceptButtonBackgroundProperty);
			set => SetValue(AcceptButtonBackgroundProperty, value);
		}

		/// <summary>
		/// Gets or sets the foreground color of accept button in the footer.
		/// </summary>
		/// <seealso cref="HeaderTextColor"/>
		/// <seealso cref="MessageTextColor"/>
		/// <seealso cref="DeclineButtonTextColor"/>
		public Color AcceptButtonTextColor
		{
			get => (Color)GetValue(AcceptButtonTextColorProperty);
			set => SetValue(AcceptButtonTextColorProperty, value);
		}

		/// <summary>
		/// Gets or sets the background color of decline button in the footer.
		/// </summary>
		/// <seealso cref="HeaderBackground"/>
		/// <seealso cref="FooterBackground"/>
		/// <seealso cref="AcceptButtonBackground"/>
		/// <seealso cref="MessageBackground"/>
		public Brush DeclineButtonBackground
		{
			get => (Brush)GetValue(DeclineButtonBackgroundProperty);
			set => SetValue(DeclineButtonBackgroundProperty, value);
		}

		/// <summary>
		/// Gets or sets the foreground color of decline button in the footer.
		/// </summary>
		/// <seealso cref="HeaderTextColor"/>
		/// <seealso cref="MessageTextColor"/>
		/// <seealso cref="AcceptButtonTextColor"/>
		public Color DeclineButtonTextColor
		{
			get => (Color)GetValue(DeclineButtonTextColorProperty);
			set => SetValue(DeclineButtonTextColorProperty, value);
		}

		/// <summary>
		/// Gets or sets the border color for the <see cref="PopupView"/>.
		/// </summary>
		public Color Stroke
		{
			get => (Color)GetValue(StrokeProperty);
			set => SetValue(StrokeProperty, value);
		}

		/// <summary>
		/// Gets or sets the font size of the header title.
		/// </summary>
		/// <seealso cref="FooterFontSize"/>
		/// <seealso cref="MessageFontSize"/>
		public double HeaderFontSize
		{
			get => (double)GetValue(HeaderFontSizeProperty);
			set => SetValue(HeaderFontSizeProperty, value);
		}

		/// <summary>
		/// Gets or sets the font style to be applied for the header title.
		/// </summary>
		/// <seealso cref="FooterFontFamily"/>
		/// <seealso cref="MessageFontFamily"/>
		public string HeaderFontFamily
		{
			get => (string)GetValue(HeaderFontFamilyProperty);
			set => SetValue(HeaderFontFamilyProperty, value);
		}

		/// <summary>
		/// Gets or sets the font attribute to be applied for the header title.
		/// </summary>
		/// <seealso cref="FooterFontAttribute"/>
		/// <seealso cref="MessageFontAttribute"/>
		public FontAttributes HeaderFontAttribute
		{
			get => (FontAttributes)GetValue(HeaderFontAttributeProperty);
			set => SetValue(HeaderFontAttributeProperty, value);
		}

		/// <summary>
		/// Gets or sets the text alignment of the header.
		/// </summary>
		/// <seealso cref="MessageTextAlignment"/>
		public TextAlignment HeaderTextAlignment
		{
			get => (TextAlignment)GetValue(HeaderTextAlignmentProperty);
			set => SetValue(HeaderTextAlignmentProperty, value);
		}

		/// <summary>
		/// Gets or sets the border thickness for the <see cref="PopupView"/>.
		/// </summary>
		public int StrokeThickness
		{
			get => (int)GetValue(StrokeThicknessProperty);
			set => SetValue(StrokeThicknessProperty, value);
		}

		/// <summary>
		/// Gets or sets a value indicating whether a drop shadow is displayed around the Popupview. The default value is true.
		/// </summary>
		public bool HasShadow
		{
			get => (bool)GetValue(HasShadowProperty);
			set => SetValue(HasShadowProperty, value);
		}

		/// <summary>
		/// Gets or sets a value that indicates the intensity of the blur effect in the overlay.
		/// </summary>
		/// <seealso cref="SfPopup.OverlayMode"/>
		/// <seealso cref="SfPopup.ShowOverlayAlways"/>
		public PopupBlurIntensity BlurIntensity
		{
			get => (PopupBlurIntensity)GetValue(BlurIntensityProperty);
			set => SetValue(BlurIntensityProperty, value);
		}

		/// <summary>
		/// Gets or sets the blur radius of the blur effect applied to the overlay when the <see cref="BlurIntensity"/> is <see cref=" PopupBlurIntensity.Custom"/>. Does not have any effect when <see cref="BlurIntensity"/> has values other than <see cref="PopupBlurIntensity.Custom"/>.
		/// </summary>
		public float BlurRadius
		{
			get => (float)GetValue(BlurRadiusProperty);
			set => SetValue(BlurRadiusProperty, value);
		}

		/// <summary>
		/// Gets or sets the corner radius for the <see cref="PopupView"/>.
		/// </summary>
		/// <remarks>
		/// On Android 33 and above, it is possible to set different corner radii for each corner using the <see cref="CornerRadius"/> class. However, on versions below Android 33, if the same value is provided for all corners, a corner radius will be applied. If different values are provided for each corner, the corner radius may not be applied.
		/// </remarks>
		public CornerRadius CornerRadius
		{
			get => (CornerRadius)GetValue(CornerRadiusProperty);
			set => SetValue(CornerRadiusProperty, value);
		}

		/// <summary>
		/// Gets or sets the corner radius of the accept and decline buttons in the footer. The default value is 20.
		/// </summary>
		public CornerRadius FooterButtonCornerRadius
		{
			get => (CornerRadius)GetValue(FooterButtonCornerRadiusProperty);
			set => SetValue(FooterButtonCornerRadiusProperty, value);
		}

		/// <summary>
		/// Gets or sets the image to be placed in the header close button for the <see cref="PopupView"/>.
		/// </summary>
		/// <seealso cref="SfPopup.ShowCloseButton"/>
		public ImageSource CloseButtonIcon
		{
			get => (ImageSource)GetValue(CloseButtonIconProperty);
			set => SetValue(CloseButtonIconProperty, value);
		}

		#endregion

		#region Internal Properties

		/// <summary>
		/// Gets or sets the icon color for close button.
		/// </summary>
		internal Color CloseButtonIconStroke
		{
			get => (Color)GetValue(CloseButtonIconStrokeProperty);
			set => SetValue(CloseButtonIconStrokeProperty, value);
		}

		/// <summary>
		/// Gets or sets the icon stroke thickness for close button.
		/// </summary>
		internal double CloseButtonIconStrokeThickness
		{
			get => (double)GetValue(CloseButtonIconStrokeThicknessProperty);
			set => SetValue(CloseButtonIconStrokeThicknessProperty, value);
		}

		/// <summary>
		/// Gets or sets the background for close button icon when pointer hover.
		/// </summary>
		internal Color HoveredCloseButtonIconBackground
		{
			get => (Color)GetValue(HoveredCloseButtonIconBackgroundProperty);
			set => SetValue(HoveredCloseButtonIconBackgroundProperty, value);
		}

		/// <summary>
		/// Gets or sets the background for close button icon when pointer pressed.
		/// </summary>
		internal Color PressedCloseButtonIconBackground
		{
			get => (Color)GetValue(PressedCloseButtonIconBackgroundProperty);
			set => SetValue(PressedCloseButtonIconBackgroundProperty, value);
		}

		#endregion

		#region IThemeElement Members

		/// <summary>
		/// Method invoke when theme changes.
		/// </summary>
		/// <param name="oldTheme">Old theme name.</param>
		/// <param name="newTheme">New theme name.</param>
		void IThemeElement.OnControlThemeChanged(string oldTheme, string newTheme)
		{
		}

		/// <summary>
		/// Method invoke at whenever common theme changes.
		/// </summary>
		/// <param name="oldTheme">Old theme name.</param>
		/// <param name="newTheme">New theme name.</param>
		void IThemeElement.OnCommonThemeChanged(string oldTheme, string newTheme)
		{
		}

		#endregion

		#region Internal Methods

		/// <summary>
		/// Gets the background color to be applied for the header.
		/// </summary>
		/// <returns> background color to be applied for the header.</returns>
		internal Brush GetHeaderBackground() => HeaderBackground;

		/// <summary>
		/// Gets the text color to be applied for the header title.
		/// </summary>
		/// <returns> text color to be applied for the header title.</returns>
		internal Color GetHeaderTextColor() => HeaderTextColor;

		/// <summary>
		/// Gets the background color of content.
		/// </summary>
		/// <returns> background color of content.</returns>
		internal Brush GetMessageBackground() => MessageBackground;

		/// <summary>
		/// Gets the background color of PopupView.
		/// </summary>
		/// <returns> background color of PopupView.</returns>
		internal Brush GetPopupBackground() => PopupBackground;
		/// <summary>
		/// Gets the foreground color of content.
		/// </summary>
		/// <returns> foreground color of content.</returns>
		internal Color GetMessageTextColor() => MessageTextColor;

		/// <summary>
		/// Gets the background color of the footer.
		/// </summary>
		/// <returns> background color of the footer.</returns>
		internal Brush GetFooterBackground() => FooterBackground;

		/// <summary>
		/// Gets the background color of accept button in the footer.
		/// </summary>
		/// <returns> background color of accept button in the footer.</returns>
		internal Brush GetAcceptButtonBackground() => AcceptButtonBackground;

		/// <summary>
		/// Gets the foreground color of accept button in the footer.
		/// </summary>
		/// <returns> foreground color of accept button in the footer.</returns>
		internal Color GetAcceptButtonTextColor() => AcceptButtonTextColor;

		/// <summary>
		/// Gets the background color of decline button in the footer.
		/// </summary>
		/// <returns> background color of decline button in the footer.</returns>
		internal Brush GetDeclineButtonBackground() => DeclineButtonBackground;

		/// <summary>
		/// Gets the foreground color of decline button in the footer.
		/// </summary>
		/// <returns> foreground color of decline button in the footer.</returns>
		internal Color GetDeclineButtonTextColor() => DeclineButtonTextColor;

		/// <summary>
		/// Gets the border color for the <see cref="PopupView"/>.
		/// </summary>
		/// <returns> border color for the PopupView.</returns>
		internal Color GetStroke() => Stroke;

		/// <summary>
		/// Gets the border thickness for the <see cref="PopupView"/>.
		/// </summary>
		/// <returns> border thickness for the PopupView.</returns>
		internal virtual double GetStrokeThickness() => (double)StrokeThickness;

		/// <summary>
		/// Gets the font size of the header title.
		/// </summary>
		/// <returns> font size of the header title.</returns>
		internal virtual double GetHeaderFontSize() => HeaderFontSize;

		/// <summary>
		/// Gets the font size of the content.
		/// </summary>
		/// <returns> font size of the content.</returns>
		internal virtual double GetMessageFontSize() => MessageFontSize;

		/// <summary>
		/// Gets the font size of the footer buttons.
		/// </summary>
		/// <returns> font size of the footer buttons.</returns>
		internal virtual double GetFooterFontSize() => FooterFontSize;

		/// <summary>
		/// Gets the font attribute to be applied for the header title.
		/// </summary>
		/// <returns> font attribute to be applied for the header title.</returns>
		internal FontAttributes GetHeaderFontAttribute() => HeaderFontAttribute;

		/// <summary>
		/// Gets the font attribute to be applied for the content.
		/// </summary>
		/// <returns> font attribute to be applied for the content.</returns>
		internal FontAttributes GetMessageFontAttribute() => MessageFontAttribute;

		/// <summary>
		/// Gets the font attribute to be applied for the footer buttons.
		/// </summary>
		/// <returns> font attribute to be applied for the footer buttons.</returns>
		internal FontAttributes GetFooterFontAttribute() => FooterFontAttribute;

		/// <summary>
		/// Gets the icon color for close button.
		/// </summary>
		/// <returns> icon color for close button.</returns>
		internal Color GetCloseButtonIconStroke() => CloseButtonIconStroke;

		/// <summary>
		/// Gets the icon stroke thickness for close button.
		/// </summary>
		/// <returns> icon stroke thickness for close button.</returns>
		internal virtual double GetCloseButtonIconStrokeThickness() => CloseButtonIconStrokeThickness;

		/// <summary>
		/// Gets the background for close button icon when pointer hover.
		/// </summary>
		/// <returns> background for close button icon when pointer hover.</returns>
		internal Color GetHoveredCloseButtonIconBackground() => HoveredCloseButtonIconBackground;

		/// <summary>
		/// Gets the background for close button icon when pointer press.
		/// </summary>
		/// <returns> background for close button icon when pointer press.</returns>
		internal Color GetPressedCloseButtonIconBackground() => PressedCloseButtonIconBackground;

		/// <summary>
		/// Gets the overlay color when PopupView is displayed.
		/// </summary>
		/// <returns> overlay color when PopupView is displayed.</returns>
		internal Brush GetOverlayColor() => OverlayColor;

		/// <summary>
		/// Method to set the theme properties for the <see cref="SfPopup"/>.
		/// </summary>
		/// <param name="popup">Instance of SfPopup.</param>
		internal void SetThemeProperties(SfPopup popup)
		{
			Parent = popup;
			ApplyDynamicResource(HeaderBackgroundProperty, "SfPopupNormalHeaderBackground");
			ApplyDynamicResource(HeaderTextColorProperty, "SfPopupNormalHeaderTextColor");
			ApplyDynamicResource(MessageBackgroundProperty, "SfPopupNormalMessageBackground");
			ApplyDynamicResource(MessageTextColorProperty, "SfpopupNormalMessageTextColor");
			ApplyDynamicResource(FooterBackgroundProperty, "SfPopupNormalFooterBackground");
			ApplyDynamicResource(AcceptButtonBackgroundProperty, "SfPopupNormalAcceptButtonBackground");
			ApplyDynamicResource(AcceptButtonTextColorProperty, "SfPopupNormalAcceptButtonTextColor");
			ApplyDynamicResource(DeclineButtonTextColorProperty, "SfPopupNormalDeclineButtonTextColor");
			ApplyDynamicResource(DeclineButtonBackgroundProperty, "SfPopupNormalDeclineButtonBackground");
			ApplyDynamicResource(StrokeProperty, "SfPopupNormalStroke");
			ApplyDynamicResource(StrokeThicknessProperty, "SfPopupNormalStrokeThickness");
			ApplyDynamicResource(HeaderFontSizeProperty, "SfPopupNormalHeaderFontSize");
			ApplyDynamicResource(MessageFontSizeProperty, "SfPopupNormalMessageFontSize");
			ApplyDynamicResource(FooterFontSizeProperty, "SfPopupNormalFooterFontSize");
			ApplyDynamicResource(CloseButtonIconStrokeProperty, "SfPopupNormalCloseButtonIconStroke");
			ApplyDynamicResource(CloseButtonIconStrokeThicknessProperty, "SfPopupNormalCloseButtonIconStrokeThickness");
			ApplyDynamicResource(HoveredCloseButtonIconBackgroundProperty, "SfPopupHoverCloseButtonIconBackground");
			ApplyDynamicResource(PressedCloseButtonIconBackgroundProperty, "SfPopupPressedCloseButtonIconBackground");
			ApplyDynamicResource(OverlayColorProperty, "SfPopupNormalOverlayBackground");
			ApplyDynamicResource(PopupBackgroundProperty, "SfPopupNormalBackground");
			ThemeElement.InitializeThemeResources(this, "SfPopupTheme");
		}

		#endregion

		/// <summary>
		/// Method to set dynamic resource for PopupStyle properties.
		/// </summary>
		/// <param name="bindableProperty">Bindable property.</param>
		/// <param name="key">Key name for property.</param>
		void ApplyDynamicResource(BindableProperty bindableProperty, string key)
		{
			if (!IsSet(bindableProperty))
			{
				SetDynamicResource(bindableProperty, key);
			}
		}
	}
}