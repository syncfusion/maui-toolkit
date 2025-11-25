using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Syncfusion.Maui.Toolkit.Internals;
using Syncfusion.Maui.Toolkit.Themes;
using MauiView = Microsoft.Maui.Controls.View;

namespace Syncfusion.Maui.Toolkit.Popup
{
	/// <summary>
	/// <see cref="SfPopup"/> displays an alert message with customizable buttons or loads a desired view within a pop-up window.
	/// </summary>
	public partial class SfPopup : SfView, IParentThemeElement
	{
		#region Fields

		/// <summary>
		/// The overlay of the _popupView.
		/// </summary>
		internal SfWindowOverlay? _popupOverlay;

		/// <summary>
		/// The popup overlay container view of the _popupView.
		/// </summary>
		internal SfPopupOverlayContainer? _popupOverlayContainer;

		/// <summary>
		/// The popup view of the <see cref="SfPopup"/> that will be displayed when setting the <see cref="SfPopup.IsOpen"/> property as <c>true</c>.
		/// </summary>
		internal PopupView? _popupView;

		/// <summary>
		/// Default height of the _popupView.
		/// </summary>
		internal double _popupViewHeight;

		/// <summary>
		/// Default body height of the PopupView.
		/// </summary>
		internal double _defaultPopupViewHeight;

		/// <summary>
		/// Default width of the _popupView.
		/// </summary>
		internal double _popupViewWidth;

		/// <summary>
		/// Default width of the PopupView.
		/// </summary>
		internal double _defaultPopupViewWidth = 313;

		/// <summary>
		/// Backing field to store the corner radius value.
		/// </summary>
		internal double _radiusValue = 0;

		/// <summary>
		/// Boolean value indicating whether the Popup opening or closing animation is in progress.
		/// </summary>
		internal bool _isPopupAnimationInProgress;

		/// <summary>
		/// Boolean value indicating whether the container opening or closing animation is in progress.
		/// </summary>
		internal bool _isContainerAnimationInProgress;

		/// <summary>
		/// Gets a value indicating whether the flow direction is RTL or not.
		/// </summary>
		internal bool _isRTL = false;

		/// <summary>
		/// Gets a value indicating whether the IsOpen is in progress.
		/// </summary>
		internal bool _isOpenInProgress = false;

		/// <summary>
		/// Represents a task completion source that can be used to create and control a Tasks with a result of type bool.
		/// </summary>
		TaskCompletionSource<bool>? _taskCompletionSource;

		/// <summary>
		/// X-point of the Popup, after calculation of the popup view x position.
		/// </summary>
		double _popupXPosition;

		/// <summary>
		/// Y-point of the Popup, after calculation of the popup view y position.
		/// </summary>
		double _popupYPosition;

		/// <summary>
		/// View relative to which popup should be displayed.
		/// </summary>
		View? _relativeView;

		/// <summary>
		/// Position relative to the RelativeView, from which popup should be displayed.
		/// </summary>
		PopupRelativePosition _relativePosition;

		/// <summary>
		/// absolute X-Point where the popup should be positioned from the relative view.
		/// </summary>
		double _absoluteXPoint;

		/// <summary>
		/// absolute Y-Point where the popup should be positioned from the relative view.
		/// </summary>
		double _absoluteYPoint;

		/// <summary>
		/// Boolean value indicating whether the popup can be shown in fullscreen or not.
		/// </summary>
		bool _showFullScreen;

		/// <summary>
		/// Backing field for the <see cref="AppliedHeaderHeight"/> property.
		/// </summary>
		double _appliedHeaderHeight;

		/// <summary>
		/// Backing field for the <see cref="AppliedFooterHeight"/> property.
		/// </summary>
		double _appliedFooterHeight;

		/// <summary>
		/// Backing field for the <see cref="AppliedBodyHeight"/> property.
		/// </summary>
		double _appliedBodyHeight;

		/// <summary>
		/// Backing field for the <see cref="AppliedPopupBodyWidth"/> property.
		/// </summary>
		double _appliedPopupBodyWidth;

		/// <summary>
		/// At Show(x,y) given x point in Sample to display the Popup.
		/// </summary>
		double _showXPosition;

		/// <summary>
		/// At Show(x,y) given y point in Sample to display the Popup.
		/// </summary>
		double _showYPosition;

		/// <summary>
		/// Minimal padding value used to identify whether _popupView is positioned at screen edges.
		/// </summary>
		int _minimalPadding = 0;

		/// <summary>
		///  Backing field for SemanticDescription.
		/// </summary>
		string? _semanticDescription;

		/// <summary>
		///  Backing field for CanShowPopupInFullScreen.
		/// </summary>
		bool _canShowPopupInFullScreen;

		/// <summary>
		///  Field for to avoid Opening the Popup again, after canceling the Closing event.
		/// </summary>
		bool _canOpenPopup = true;

		/// <summary>
		/// Field to store keyboard height.
		/// </summary>
		double _keyboardHeight = 0;

		/// <summary>
		/// Backing field for the <see cref="ScreenHeight"/> property.
		/// </summary>
		int _screenHeight;

		/// <summary>
		/// Backing field for the <see cref="ScreenWidth"/> property.
		/// </summary>
		int _screenWidth;

		/// <summary>
		/// Backing field for the <see cref="StatusBarHeight"/> property.
		/// </summary>
		double _statusBarHeight;

		/// <summary>
		/// Backing field for the <see cref="SafeAreaAtLeft"/> property.
		/// </summary>
		int _safeAreaAtLeft;

		/// <summary>
		/// Backing field for the <see cref="SafeAreaAtRight"/> property.
		/// </summary>
		int _safeAreaAtRight;

		/// <summary>
		/// Backing field for the <see cref="SafeAreaAtTop"/> property.
		/// </summary>
		int _safeAreaAtTop;

		/// <summary>
		/// Backing field for the <see cref="SafeAreaAtBottom"/> property.
		/// </summary>
		int _safeAreaAtBottom;

		/// <summary>
		/// Indicates if the AddToOverlay operation was successful.
		/// </summary>
		bool _isOverlayAdded;

		/// <summary>
		/// Indicates if opening the popup is deferred until certain conditions are met.
		/// </summary>
		bool _isOpenDeferred;

		#endregion

		#region Bindable Properties

		/// <summary>
		/// Identifies the AutoCloseDuration bindable property.
		/// </summary>
		public static readonly BindableProperty AutoCloseDurationProperty =
			BindableProperty.Create(nameof(AutoCloseDuration), typeof(int), typeof(SfPopup), 0, BindingMode.Default, propertyChanged: OnAutoCloseDurationPropertyChanged);

		/// <summary>
		/// Identifies the IsOpen bindable property.
		/// </summary>
		public static readonly BindableProperty IsOpenProperty =
			BindableProperty.Create(nameof(IsOpen), typeof(bool), typeof(SfPopup), false, BindingMode.TwoWay, null, propertyChanged: OnIsOpenPropertyChanged);

		/// <summary>
		/// Identifies the StaysOpen bindable property.
		/// </summary>
		public static readonly BindableProperty StaysOpenProperty =
			BindableProperty.Create(nameof(StaysOpen), typeof(bool), typeof(SfPopup), false, BindingMode.Default, null, propertyChanged: OnStaysOpenChanged);

		/// <summary>
		/// Identifies the RelativePosition bindable property.
		/// </summary>
		public static readonly BindableProperty RelativePositionProperty =
		  BindableProperty.Create(nameof(RelativePosition), typeof(PopupRelativePosition), typeof(SfPopup), PopupRelativePosition.AlignTop, BindingMode.Default, null, null);

		/// <summary>
		/// Identifies the RelativeView bindable property.
		/// </summary>
		public static readonly BindableProperty RelativeViewProperty =
		  BindableProperty.Create(nameof(RelativeView), typeof(View), typeof(SfPopup), null, BindingMode.Default, null, null);

		/// <summary>
		/// Identifies the AbsoluteX bindable property.
		/// </summary>
		public static readonly BindableProperty AbsoluteXProperty =
		  BindableProperty.Create(nameof(AbsoluteX), typeof(int), typeof(SfPopup), 0, BindingMode.Default, null, null);

		/// <summary>
		/// Identifies the AbsoluteY bindable property.
		/// </summary>
		public static readonly BindableProperty AbsoluteYProperty =
		  BindableProperty.Create(nameof(AbsoluteY), typeof(int), typeof(SfPopup), 0, BindingMode.Default, null, null);

		/// <summary>
		/// Identifies the ShowOverlay bindable property.
		/// </summary>
		public static readonly BindableProperty ShowOverlayAlwaysProperty =
			BindableProperty.Create(nameof(ShowOverlayAlways), typeof(bool), typeof(SfPopup), true, BindingMode.Default, null, null);

		/// <summary>
		/// Identifies the <see cref="SfPopup.OverlayMode"/> <see cref="BindableProperty"/>.
		/// </summary>
		public static readonly BindableProperty OverlayModeProperty =
			BindableProperty.Create(nameof(OverlayMode), typeof(PopupOverlayMode), typeof(SfPopup), PopupOverlayMode.Transparent, BindingMode.Default, null, propertyChanged: OnOverlayModePropertyChanged);

		/// <summary>
		/// Identifies the AppearanceMode bindable property.
		/// </summary>
		public static readonly BindableProperty AppearanceModeProperty =
			BindableProperty.Create(nameof(AppearanceMode), typeof(PopupButtonAppearanceMode), typeof(SfPopup), PopupButtonAppearanceMode.OneButton, BindingMode.Default, null, propertyChanged: OnAppearanceModePropertyChanged);

		/// <summary>
		/// Identifies the HeaderTemplate bindable property.
		/// </summary>
		public static readonly BindableProperty HeaderTemplateProperty =
			BindableProperty.Create(nameof(HeaderTemplate), typeof(DataTemplate), typeof(SfPopup), null, BindingMode.Default, null, propertyChanged: OnHeaderTemplatePropertyChanged);

		/// <summary>
		/// Identifies the ContentTemplate bindable property.
		/// </summary>
		public static readonly BindableProperty ContentTemplateProperty =
			BindableProperty.Create(nameof(ContentTemplate), typeof(DataTemplate), typeof(SfPopup), null, BindingMode.Default, null, propertyChanged: OnContentTemplatePropertyChanged);

		/// <summary>
		/// Identifies the FooterTemplate bindable property.
		/// </summary>
		public static readonly BindableProperty FooterTemplateProperty =
			BindableProperty.Create(nameof(FooterTemplate), typeof(DataTemplate), typeof(SfPopup), null, BindingMode.Default, null, propertyChanged: OnFooterTemplatePropertyChanged);

		/// <summary>
		/// Identifies the ShowHeader bindable property.
		/// </summary>
		public static readonly BindableProperty ShowHeaderProperty =
			BindableProperty.Create(nameof(ShowHeader), typeof(bool), typeof(SfPopup), true, BindingMode.Default, null, propertyChanged: OnShowHeaderPropertyChanged);

		/// <summary>
		/// Identifies the ShowFooter bindable property.
		/// </summary>
		public static readonly BindableProperty ShowFooterProperty =
			BindableProperty.Create(nameof(ShowFooter), typeof(bool), typeof(SfPopup), false, BindingMode.Default, null, propertyChanged: OnShowFooterPropertyChanged);

		/// <summary>
		/// Identifies the HeaderTitle bindable property.
		/// </summary>
		public static readonly BindableProperty HeaderTitleProperty =
			BindableProperty.Create(nameof(HeaderTitle), typeof(string), typeof(SfPopup), "Title", BindingMode.Default, null, propertyChanged: OnHeaderTitlePropertyChanged);

		/// <summary>
		/// Identifies the AcceptButtonText bindable property.
		/// </summary>
		public static readonly BindableProperty AcceptButtonTextProperty =
			BindableProperty.Create(nameof(AcceptButtonText), typeof(string), typeof(SfPopup), "ACCEPT", BindingMode.Default, null, propertyChanged: OnAcceptButtonTextPropertyChanged);

		/// <summary>
		/// Identifies the DeclineButtonText bindable property.
		/// </summary>
		public static readonly BindableProperty DeclineButtonTextProperty =
			BindableProperty.Create(nameof(DeclineButtonText), typeof(string), typeof(SfPopup), "DECLINE", BindingMode.Default, null, propertyChanged: OnDeclineButtonTextPropertyChanged);

		/// <summary>
		/// Identifies the StartX bindable property.
		/// </summary>
		public static readonly BindableProperty StartXProperty =
			BindableProperty.Create(nameof(StartX), typeof(int), typeof(SfPopup), -1, BindingMode.Default, null, propertyChanged: OnStartXPropertyChanged);

		/// <summary>
		/// Identifies the IgnoreActionBar bindable property.
		/// </summary>
		public static readonly BindableProperty IgnoreActionBarProperty =
			BindableProperty.Create(nameof(IgnoreActionBar), typeof(bool), typeof(SfPopup), false, BindingMode.Default, null, null);

		/// <summary>
		/// Identifies the StartY bindable property.
		/// </summary>
		public static readonly BindableProperty StartYProperty =
			BindableProperty.Create(nameof(StartY), typeof(int), typeof(SfPopup), -1, BindingMode.Default, null, propertyChanged: OnStartYPropertyChanged);
		
		/// <summary>
        /// Identifies the Padding bindable property.
        /// </summary>
        public static new readonly BindableProperty PaddingProperty =
            BindableProperty.Create("Padding", typeof(Thickness), typeof(SfPopup), new Thickness(0.0), BindingMode.Default, propertyChanged: OnPaddingPropertyChanged);

		/// <summary>
		/// Identifies the PopupStyle bindable property.
		/// </summary>
		public static readonly BindableProperty PopupStyleProperty =
			BindableProperty.Create(nameof(PopupStyle), typeof(PopupStyle), typeof(SfPopup), null, BindingMode.Default, null, propertyChanged: OnPopupStylePropertyChanged, defaultValueCreator: CreatePopupStyle);

		/// <summary>
		/// Identifies the ShowCloseButton bindable property.
		/// </summary>
		public static readonly BindableProperty ShowCloseButtonProperty =
			BindableProperty.Create(nameof(ShowCloseButton), typeof(bool), typeof(SfPopup), false, BindingMode.Default, null, propertyChanged: OnShowCloseButtonPropertyChanged);

		/// <summary>
		/// Identifies the HeaderHeight bindable property.
		/// </summary>
		public static readonly BindableProperty HeaderHeightProperty =
			BindableProperty.Create(nameof(HeaderHeight), typeof(int), typeof(SfPopup), 72, BindingMode.Default, null, propertyChanged: OnHeaderHeightPropertyChanged);

		/// <summary>
		/// Identifies the FooterHeight bindable property.
		/// </summary>
		public static readonly BindableProperty FooterHeightProperty =
			BindableProperty.Create(nameof(FooterHeight), typeof(int), typeof(SfPopup), 88, BindingMode.Default, null, propertyChanged: OnFooterHeightPropertyChanged);

		/// <summary>
		/// Identifies the AcceptCommand bindable property.
		/// </summary>
		public static readonly BindableProperty AcceptCommandProperty =
			BindableProperty.Create(nameof(AcceptCommand), typeof(ICommand), typeof(SfPopup), null, BindingMode.Default, null, null);

		/// <summary>
		/// Identifies the DeclineCommand bindable property.
		/// </summary>
		public static readonly BindableProperty DeclineCommandProperty =
			BindableProperty.Create(nameof(DeclineCommand), typeof(ICommand), typeof(SfPopup), null, BindingMode.Default, null, null);

		/// <summary>
		/// Identifies the AutoSizeMode bindable property.
		/// </summary>
		public static readonly BindableProperty AutoSizeModeProperty =
			BindableProperty.Create(nameof(AutoSizeMode), typeof(PopupAutoSizeMode), typeof(SfPopup), PopupAutoSizeMode.None, BindingMode.Default, null, OnAutoSizeModePropertyChanged);

		/// <summary>
		/// Identifies the IsFullScreen bindable property.
		/// </summary>
		public static readonly BindableProperty IsFullScreenProperty =
			BindableProperty.Create(nameof(IsFullScreen), typeof(bool), typeof(SfPopup), false, BindingMode.Default, null, propertyChanged: OnIsFullScreenPropertyChanged);

		/// <summary>
		/// Identifies the Message bindable property.
		/// </summary>
		public static readonly BindableProperty MessageProperty =
			BindableProperty.Create(nameof(Message), typeof(string), typeof(SfPopup), "Popup Message", BindingMode.Default, null, propertyChanged: OnMessagePropertyChanged);

		/// <summary>
		/// Identifies the AnimationDuration bindable property.
		/// </summary>
		public static readonly BindableProperty AnimationDurationProperty =
			BindableProperty.Create(nameof(AnimationDuration), typeof(double), typeof(SfPopup), 300d, BindingMode.Default, null, propertyChanged: OnAnimationDurationPropertyChanged);

		/// <summary>
		/// Identifies the AnimationMode bindable property.
		/// </summary>
		public static readonly BindableProperty AnimationModeProperty =
			BindableProperty.Create(nameof(AnimationMode), typeof(PopupAnimationMode), typeof(SfPopup), PopupAnimationMode.Fade, BindingMode.Default, null, propertyChanged: OnAnimationModePropertyChanged);

		/// <summary>
		///  Identifies the AnimationEasing bindable property.
		/// </summary>
		public static readonly BindableProperty AnimationEasingProperty =
			BindableProperty.Create(nameof(AnimationEasing), typeof(PopupAnimationEasing), typeof(SfPopup), PopupAnimationEasing.Linear, BindingMode.Default, null, propertyChanged: OnAnimationEasingPropertyChanged);

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="SfPopup"/> class.
		/// </summary>
		public SfPopup()
		{
			Initialize();
			ThemeElement.InitializeThemeResources(this, "SfPopupTheme");
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets a value indicating whether the popup view is open or not.
		/// </summary>
		/// <value>
		/// The value indicating whether the popup view is open or not.
		/// </value>
		/// <remarks>
		/// The popup view will be opened and closed based on this value.
		/// </remarks>
		/// <example>
		/// <para>The following code example demonstrates how to set <see cref="IsOpen"/> property for the <see cref="SfPopup"/> control.</para>
		/// <code lang="C#">
		/// <![CDATA[
		/// void clickToShowPopup_Clicked(object sender, EventArgs e)
		/// {
		///     popup.IsOpen = true;
		/// }
		/// ]]>
		/// </code>
		/// <code lang="XAML">
		/// <![CDATA[
		/// <syncfusion:SfPopup x:Name="popup" IsOpen="True"/>
		/// ]]>
		/// </code>
		/// </example>
		/// <seealso cref="Opening"/>
		/// <seealso cref="Opened"/>
		/// <seealso cref="Closing"/>
		/// <seealso cref="Closed"/>
		/// <seealso cref="Show(bool)"/>
		/// <seealso cref="Show(double, double)"/>
		/// <seealso cref="ShowRelativeToView(View, PopupRelativePosition, double, double)"/>
		public bool IsOpen
		{
			get => (bool)GetValue(IsOpenProperty);
			set => SetValue(IsOpenProperty, value);
		}

		/// <summary>
		/// Gets or sets a value indicating whether overlay should be transparent or blurred.
		/// </summary>
		/// <value>
		/// <b>OverlayMode.Blur</b> If the overlay should be blurred otherwise<b>OverlayMode.Transparent</b>.The default value is <b>OverlayMode.Transparent</b>.
		/// </value>
		/// <remarks>
		/// <b>OverlayMode.Blur</b> will be applied to android 31 and above. In below android 31, <b>OverlayMode.Transparent</b> will be applied by default.
		/// </remarks>
		/// <example>
		/// <para>The following code example demonstrates how to set <see cref="OverlayMode"/> property for the <see cref="SfPopup"/> control.</para>
		/// <code lang="XAML">
		/// <![CDATA[
		/// 
		/// <syncfusion:SfPopup x:Name="popup" 
		///						OverlayMode="Blur">
		/// ]]>
		/// </code>
		/// </example>
		/// <seealso cref="ShowOverlayAlways"/>
		/// <seealso cref="PopupStyle.OverlayColor"/>
		/// <seealso cref="PopupStyle.BlurIntensity"/>
		/// <seealso cref="PopupStyle.BlurRadius"/>
		public PopupOverlayMode OverlayMode
		{
			get => (PopupOverlayMode)GetValue(OverlayModeProperty);
			set => SetValue(OverlayModeProperty, value);
		}

		/// <summary>
		/// Gets or sets the relative position, where the pop-up should be displayed relatively to <see cref="RelativeView"/>. The relative position can also be absolutely adjusted using the <see cref="AbsoluteX"/> and <see cref="AbsoluteY"/> properties.
		/// </summary>
		/// <example>
		/// <para>The following code example demonstrates how to set RelativePosition property for the <see cref="SfPopup"/> control.</para>
		/// <code lang="C#">
		/// <![CDATA[
		/// void clickToShowPopup_Clicked(object sender, EventArgs e)
		/// {
		///     popup.Show();
		/// }
		/// ]]>
		/// </code>
		/// <code lang="XAML">
		/// <![CDATA[
		/// <StackLayout WidthRequest = "500" >
		///	   <Button x:Name="setRelativeView" Text="Set Relative" />
		///	   <syncfusion:SfPopup x:Name="popup" AbsoluteX="10" AbsoluteY="10" RelativeView="{x:Reference setRelativeView}" RelativePosition="AlignBottom"/>
		///    <Button x:Name="clickToShowPopup" Text="Click To Show Popup" Clicked="clickToShowPopup_Clicked"/>
		/// </StackLayout>
		/// ]]>
		/// </code>
		/// </example>
		/// <seealso cref="RelativeView"/>
		/// <seealso cref="AbsoluteX"/>
		/// <seealso cref="AbsoluteY"/>
		/// <seealso cref="ShowRelativeToView(View, PopupRelativePosition, double, double)"/>
		/// <seealso cref="IsOpen"/>
		/// <seealso cref="Show(bool)"/>
		public PopupRelativePosition RelativePosition
		{
			get => (PopupRelativePosition)GetValue(RelativePositionProperty);
			set => SetValue(RelativePositionProperty, value);
		}

		/// <summary>
		/// Gets or sets the absolute x-point to display a pop-up when positioning it relatively to the specified <see cref="RelativeView"/> based on the <see cref="RelativePosition"/>.
		/// The pop-up will be displayed based on this property value only when relatively displaying it by using the <see cref="RelativeView"/> property.
		/// </summary>
		/// <example>
		/// <para>The following code example demonstrates how to set <see cref="AbsoluteX"/> property for the <see cref="SfPopup"/> control.</para>
		/// <code lang="XAML">
		/// <![CDATA[
		/// 
		/// <syncfusion:SfPopup x:Name="popup" 
		///						AbsoluteX="100">
		/// ]]>
		/// </code>
		/// </example>
		/// <seealso cref="RelativePosition"/>
		/// <seealso cref="RelativeView"/>
		/// <seealso cref="AbsoluteY"/>
		/// <seealso cref="ShowRelativeToView(View, PopupRelativePosition, double, double)"/>
		/// <seealso cref="IsOpen"/>
		/// <seealso cref="Show(bool)"/>
		public int AbsoluteX
		{
			get => (int)GetValue(AbsoluteXProperty);
			set => SetValue(AbsoluteXProperty, value);
		}

		/// <summary>
		/// Gets or sets the absolute y-point to display a pop-up when positioning it relatively to the specified <see cref="RelativeView"/> based on the <see cref="RelativePosition"/>.
		/// The pop-up will be displayed based on this property value only when relatively displaying it by using the <see cref="RelativeView"/> property.
		/// </summary>
		/// <example>
		/// <para>The following code example demonstrates how to set <see cref="AbsoluteY"/> property for the <see cref="SfPopup"/> control.</para>
		/// <code lang="XAML">
		/// <![CDATA[
		/// 
		/// <syncfusion:SfPopup x:Name="popup" 
		///						AbsoluteY="100">
		/// ]]>
		/// </code>
		/// </example>
		/// <seealso cref="RelativePosition"/>
		/// <seealso cref="RelativeView"/>
		/// <seealso cref="AbsoluteX"/>
		/// <seealso cref="ShowRelativeToView(View, PopupRelativePosition, double, double)"/>
		/// <seealso cref="IsOpen"/>
		/// <seealso cref="Show(bool)"/>
		public int AbsoluteY
		{
			get => (int)GetValue(AbsoluteYProperty);
			set => SetValue(AbsoluteYProperty, value);
		}

		/// <summary>
		/// Gets or sets the view relative to which the popup should be displayed based on the <see cref="RelativePosition"/>. The relative position can also be absolutely adjusted using the <see cref="AbsoluteX"/> and <see cref="AbsoluteY"/> properties.
		/// </summary>
		/// <value>
		/// The relative position can also be absolutely adjusted using the <see cref="AbsoluteX"/> and <see cref="AbsoluteY"/> properties.
		/// </value>
		/// <remarks>
		/// Positions the <see cref="SfPopup"/> relative to a specified view, with options for absolute positioning or relative alignment.
		/// </remarks>
		/// <example>
		/// <para>The following code example demonstrates how to set RelativeView property for the <see cref="SfPopup"/> control.</para>
		/// <code lang="C#">
		/// <![CDATA[
		/// void clickToShowPopup_Clicked(object sender, EventArgs e)
		/// {
		///     popup.Show();
		/// }
		/// ]]>
		/// </code>
		/// <code lang="XAML">
		/// <![CDATA[
		///<StackLayout WidthRequest = "500" >
		///	   <Button x:Name="setRelativeView" Text="Set Relative" />
		///	   <syncfusion:SfPopup x:Name="popup" AbsoluteX="10" AbsoluteY="10" RelativeView="{x:Reference setRelativeView}" RelativePosition="AlignBottom"/>
		///    <Button x:Name="clickToShowPopup" Text="Click To Show Popup" Clicked="clickToShowPopup_Clicked"/>
		///</StackLayout>
		/// ]]>
		/// </code>
		/// </example>
		/// <seealso cref="RelativePosition"/>
		/// <seealso cref="AbsoluteX"/>
		/// <seealso cref="AbsoluteY"/>
		/// <seealso cref="ShowRelativeToView(View, PopupRelativePosition, double, double)"/>
		/// <seealso cref="IsOpen"/>
		/// <seealso cref="Show(bool)"/>
		public View RelativeView
		{
			get => (View)GetValue(RelativeViewProperty);
			set => SetValue(RelativeViewProperty, value);
		}

		/// <summary>
		/// Gets or sets a value indicating whether the <see cref="_popupView"/> should be opened, when interacting outside its boundary area.
		/// </summary>
		/// <example>
		/// <para>The following code example demonstrates how to set <see cref="StaysOpen"/> property for the <see cref="SfPopup"/> control.</para>
		/// <code lang="XAML">
		/// <![CDATA[ 
		/// <syncfusion:SfPopup x:Name="popup" 
		///						StaysOpen="True">
		/// ]]>
		/// </code>
		/// </example>
		/// <seealso cref="ShowCloseButton"/>
		public bool StaysOpen
		{
			get => (bool)GetValue(StaysOpenProperty);
			set => SetValue(StaysOpenProperty, value);
		}

		/// <summary>
		/// Gets or Sets a value indicating whether an overlay can be shown around the <see cref="_popupView"/>.
		/// </summary>
		/// <example>
		/// <para>The following code example demonstrates how to set <see cref="ShowOverlayAlways"/> property for the <see cref="SfPopup"/> control.</para>
		/// <code lang="XAML">
		/// <![CDATA[ 
		/// <syncfusion:SfPopup x:Name="popup" 
		///						ShowOverlayAlways="True">
		/// ]]>
		/// </code>
		/// </example>
		/// <seealso cref="OverlayMode"/>
		/// <seealso cref="PopupStyle.OverlayColor"/>
		/// <seealso cref="PopupStyle.BlurIntensity"/>
		/// <seealso cref="PopupStyle.BlurRadius"/>
		public bool ShowOverlayAlways
		{
			get => (bool)GetValue(ShowOverlayAlwaysProperty);
			set => SetValue(ShowOverlayAlwaysProperty, value);
		}

		/// <summary>
		/// Gets or sets the type of layout template of the popup view.
		/// </summary>
		/// <value>
		/// <b>PopupButtonAppearanceMode.OneButton</b> displays a single button in the popup view footer. <b>PopupButtonAppearanceMode.TwoButton</b> displays two buttons in the _popupView footer.
		/// </value>
		/// <example>
		/// <para>The following code example demonstrates how to set <see cref="AppearanceMode"/> property for the <see cref="SfPopup"/> control.</para>
		/// <code lang="XAML">
		/// <![CDATA[ 
		/// <syncfusion:SfPopup x:Name="popup" 
		///						AppearanceMode="OneButton">
		/// ]]>
		/// </code>
		/// </example>
		/// <seealso cref="PopupStyle"/>
		/// <seealso cref="FooterTemplate"/>
		/// <seealso cref="FooterHeight"/>
		/// <seealso cref="AcceptButtonText"/>
		/// <seealso cref="DeclineButtonText"/>
		/// <seealso cref="AcceptCommand"/>
		/// <seealso cref="DeclineCommand"/>
		public PopupButtonAppearanceMode AppearanceMode
		{
			get => (PopupButtonAppearanceMode)GetValue(AppearanceModeProperty);
			set => SetValue(AppearanceModeProperty, value);
		}

		/// <summary>
		/// Gets or sets the template to be loaded in the header of the _popupView.
		/// </summary>
		/// <value>
		/// Gets or sets the template to be loaded in the header of the <see cref="_popupView"/>. The default value is null.
		/// </value>
		/// <remarks>
		/// <see cref="PopupStyle"/> does not apply to templated elements.
		/// </remarks>
		/// <example>
		/// <para>The following code example demonstrates how to set <see cref="HeaderTemplate"/> property for the <see cref="SfPopup"/> control.</para>
		/// <code lang="XAML">
		/// <![CDATA[
		/// <syncfusion:SfPopup x:Name="popup">
		///      <syncfusion:SfPopup.HeaderTemplate>
		///          <DataTemplate>
		///              <Label Text = "Customized Header"
		///                 FontAttributes="Bold"
		///                 BackgroundColor="Blue"
		///                 FontSize="16"
		///                 HorizontalTextAlignment="Center"
		///                 VerticalTextAlignment="Center"/>
		///          </DataTemplate>
		///      </syncfusion:SfPopup.HeaderTemplate>
		///  </syncfusion:SfPopup>
		/// ]]>
		/// </code>
		/// </example>
		/// <seealso cref="HeaderTitle"/>
		/// <seealso cref="FooterTemplate"/>
		/// <seealso cref="ShowCloseButton"/>
		/// <seealso cref="ContentTemplate"/>
		/// <seealso cref="HeaderHeight"/>
		/// <seealso cref="PopupStyle"/>
		public DataTemplate HeaderTemplate
		{
			get => (DataTemplate)GetValue(HeaderTemplateProperty);
			set => SetValue(HeaderTemplateProperty, value);
		}

		/// <summary>
		/// Gets or sets the template to be loaded in the body of the _popupView.
		/// </summary>
		/// <value>
		/// Gets or sets the template to be loaded in the body of the <see cref="_popupView"/>. The default value is null.
		/// </value>
		/// <remarks>
		/// <see cref="PopupStyle"/> does not apply to templated elements.
		/// </remarks>
		/// <example>
		/// <para>The following code example demonstrates how to set <see cref="ContentTemplate"/> property for the <see cref="SfPopup"/> control.</para>
		/// <code lang="XAML">
		/// <![CDATA[
		/// <syncfusion:SfPopup x:Name="popup">
		///          <syncfusion:SfPopup.ContentTemplate>
		///              <DataTemplate>
		///                  <Label Text = "This is SfPopup"
		///                         BackgroundColor="SkyBlue"
		///                         HorizontalTextAlignment="Center"/>
		///              </DataTemplate>
		///          </syncfusion:SfPopup.ContentTemplate>
		///  </syncfusion:SfPopup>
		/// ]]>
		/// </code>
		/// </example>
		/// <seealso cref="Message"/>
		/// <seealso cref="FooterTemplate"/>
		/// <seealso cref="HeaderTemplate"/>
		/// <seealso cref="AutoSizeMode"/>
		/// <seealso cref="PopupStyle"/>
		public DataTemplate ContentTemplate
		{
			get => (DataTemplate)GetValue(ContentTemplateProperty);
			set => SetValue(ContentTemplateProperty, value);
		}

		/// <summary>
		/// Gets or sets the template to be loaded in the footer of the popup view.
		/// </summary>
		/// <value>
		/// The template to be loaded in the footer of the popup view,The default value is null.
		/// </value>
		/// <remarks>
		/// <see cref="PopupStyle"/> does not apply to templated elements.
		/// <see cref="ShowFooter"/> need to be enabled to display footer.
		/// </remarks>
		/// <example>
		/// <para>The following code example demonstrates how to set <see cref="FooterTemplate"/> property for the <see cref="SfPopup"/> control.</para>
		/// <code lang="C#">
		/// <![CDATA[
		/// <syncfusion:SfPopup x:Name="popup">
		///		<syncfusion:SfPopup.FooterTemplate>
		///			<DataTemplate>
		///				<Label Text = "Customized Footer"
		///					   FontAttributes="Bold"
		///                    BackgroundColor="Blue"
		///                    FontSize="16"
		///                    HorizontalTextAlignment="Center"
		///                    VerticalTextAlignment="Center"/>
		///         </DataTemplate>
		///		</syncfusion:SfPopup.FooterTemplate>
		/// </syncfusion:SfPopup>
		/// ]]>
		/// </code>
		/// </example>
		/// <seealso cref="FooterTemplate"/>
		/// <seealso cref="ContentTemplate"/>
		/// <seealso cref="ShowFooter"/>
		/// <seealso cref="FooterHeight"/>
		/// <seealso cref="AppearanceMode"/>
		/// <seealso cref="AcceptButtonText"/>
		/// <seealso cref="DeclineButtonText"/>
		/// <seealso cref="AcceptCommand"/>
		/// <seealso cref="DeclineCommand"/>
		/// <seealso cref="PopupStyle"/>
		public DataTemplate FooterTemplate
		{
			get => (DataTemplate)GetValue(FooterTemplateProperty);
			set => SetValue(FooterTemplateProperty, value);
		}

		/// <summary>
		/// Gets or sets a value indicating whether the header is to be included in the popup view.
		/// </summary>
		/// <example>
		/// <para>The following code example demonstrates how to set <see cref="ShowHeader"/> property for the <see cref="SfPopup"/> control.</para>
		/// <code lang="XAML">
		/// <![CDATA[ 
		/// <syncfusion:SfPopup x:Name="popup" 
		///						ShowHeader="True">
		/// ]]>
		/// </code>
		/// </example>
		/// <seealso cref="ShowCloseButton"/>
		/// <seealso cref="HeaderHeight"/>
		/// <seealso cref="HeaderTemplate"/>
		/// <seealso cref="HeaderTitle"/>
		public bool ShowHeader
		{
			get => (bool)GetValue(ShowHeaderProperty);
			set => SetValue(ShowHeaderProperty, value);
		}

		/// <summary>
		/// Gets or sets the time delay in milliseconds for automatically closing the <see cref="SfPopup"/>.
		/// </summary>
		/// <example>
		/// <para>The following code example demonstrates how to set <see cref="AutoCloseDuration"/> property for the <see cref="SfPopup"/> control.</para>
		/// <code lang="XAML">
		/// <![CDATA[ 
		/// <syncfusion:SfPopup x:Name="popup" 
		///						AutoCloseDuration="500">
		/// ]]>
		/// </code>
		/// </example>
		public int AutoCloseDuration
		{
			get => (int)GetValue(AutoCloseDurationProperty);
			set => SetValue(AutoCloseDurationProperty, value);
		}

		/// <summary>
		/// Gets or sets a value indicating whether the footer is to be included in the popup view.
		/// </summary>
		/// <value>
		/// <c>true</c> if show footer; otherwise, <c>false</c>. The default value is <c>false</c>.
		/// </value>
		/// <example>
		/// <para>The following code example demonstrates how to set <see cref="ShowFooter"/> property for the <see cref="SfPopup"/> control.</para>
		/// <code lang="XAML">
		/// <![CDATA[ 
		/// <syncfusion:SfPopup x:Name="popup" 
		///						ShowFooter="True">
		/// ]]>
		/// </code>
		/// </example>
		/// <seealso cref="FooterTemplate"/>
		/// <seealso cref="FooterHeight"/>
		/// <seealso cref="AppearanceMode"/>
		/// <seealso cref="AcceptButtonText"/>
		/// <seealso cref="DeclineButtonText"/>
		/// <seealso cref="AcceptCommand"/>
		/// <seealso cref="DeclineCommand"/>
		public bool ShowFooter
		{
			get => (bool)GetValue(ShowFooterProperty);
			set => SetValue(ShowFooterProperty, value);
		}

		/// <summary>
		/// Gets or sets the header title of the popup view.
		/// </summary>
		/// <example>
		/// <para>The following code example demonstrates how to set <see cref="HeaderTitle"/> property for the <see cref="SfPopup"/> control.</para>
		/// <code lang="XAML">
		/// <![CDATA[ 
		/// <syncfusion:SfPopup x:Name="popup" 
		///						HeaderTitle="PopupHeader">
		/// ]]>
		/// </code>
		/// </example>
		/// <seealso cref="HeaderTemplate"/>
		/// <seealso cref="ShowCloseButton"/>
		/// <seealso cref="HeaderHeight"/>
		/// <seealso cref="PopupStyle.HeaderBackground"/>
		/// <seealso cref="PopupStyle.HeaderTextColor"/>
		/// <seealso cref="PopupStyle.HeaderFontAttribute"/>
		/// <seealso cref="PopupStyle.HeaderFontFamily"/>
		/// <seealso cref="PopupStyle.HeaderFontSize"/>
		/// <seealso cref="PopupStyle.HeaderTextAlignment"/>
		public string HeaderTitle
		{
			get => (string)GetValue(HeaderTitleProperty);
			set => SetValue(HeaderTitleProperty, value);
		}

		/// <summary>
		/// Gets or sets the popup message of the popup view.
		/// </summary>
		/// <example>
		/// <para>The following code example demonstrates how to set <see cref="Message"/> property for the <see cref="SfPopup"/> control.</para>
		/// <code lang="XAML">
		/// <![CDATA[
		/// 
		/// <syncfusion:SfPopup x:Name="popup" 
		///						Message="Popup Content">
		/// ]]>
		/// </code>
		/// </example>
		/// <seealso cref="ContentTemplate"/>
		/// <seealso cref="PopupStyle.MessageBackground"/>
		/// <seealso cref="PopupStyle.MessageTextColor"/>
		public string Message
		{
			get => (string)GetValue(MessageProperty);
			set => SetValue(MessageProperty, value);
		}

		/// <summary>
		/// Gets or sets the text of accept button in the footer.
		/// </summary>
		/// <example>
		/// <para>The following code example demonstrates how to set <see cref="AcceptButtonText"/> property for the <see cref="SfPopup"/> control.</para>
		/// <code lang="XAML">
		/// <![CDATA[
		/// <syncfusion:SfPopup x:Name="popup" 
		///						AccpetButtonText="OK">
		/// ]]>
		/// </code>
		/// </example>
		/// <seealso cref="FooterTemplate"/>
		/// <seealso cref="FooterHeight"/>
		/// <seealso cref="PopupStyle.FooterBackground"/>
		/// <seealso cref="PopupStyle.AcceptButtonTextColor"/>
		/// <seealso cref="PopupStyle.DeclineButtonTextColor"/>
		/// <seealso cref="PopupStyle.FooterFontAttribute"/>
		/// <seealso cref="PopupStyle.FooterFontFamily"/>
		/// <seealso cref="PopupStyle.FooterFontSize"/>
		public string AcceptButtonText
		{
			get => (string)GetValue(AcceptButtonTextProperty);
			set => SetValue(AcceptButtonTextProperty, value);
		}

		/// <summary>
		/// Gets or sets the text of decline button in the footer.
		/// </summary>
		/// <example>
		/// <para>The following code example demonstrates how to set <see cref="DeclineButtonText"/> property for the <see cref="SfPopup"/> control.</para>
		/// <code lang="XAML">
		/// <![CDATA[ 
		/// <syncfusion:SfPopup x:Name="popup" 
		///						DeclineButtonText="Cancel">
		/// ]]>
		/// </code>
		/// </example>
		/// <seealso cref="FooterTemplate"/>
		/// <seealso cref="FooterHeight"/>
		/// <seealso cref="PopupStyle.FooterBackground"/>
		/// <seealso cref="PopupStyle.AcceptButtonTextColor"/>
		/// <seealso cref="PopupStyle.DeclineButtonTextColor"/>
		/// <seealso cref="PopupStyle.FooterFontAttribute"/>
		/// <seealso cref="PopupStyle.FooterFontFamily"/>
		/// <seealso cref="PopupStyle.FooterFontSize"/>
		public string DeclineButtonText
		{
			get => (string)GetValue(DeclineButtonTextProperty);
			set => SetValue(DeclineButtonTextProperty, value);
		}

		/// <summary>
		/// Gets or sets the x-position of the _popupView.
		/// </summary>
		/// <example>
		/// <para>The following code example demonstrates how to set <see cref="StartX"/> property for the <see cref="SfPopup"/> control.</para>
		/// <code lang="XAML">
		/// <![CDATA[ 
		/// <syncfusion:SfPopup x:Name="popup" 
		///						StartX="100">
		/// ]]>
		/// </code>
		/// </example>
		public int StartX
		{
			get => (int)GetValue(StartXProperty);
			set => SetValue(StartXProperty, value);
		}

		/// <summary>
		/// Gets or sets the y-position of the _popupView.
		/// </summary>
		/// <example>
		/// <para>The following code example demonstrates how to set <see cref="StartY"/> property for the <see cref="SfPopup"/> control.</para>
		/// <code lang="XAML">
		/// <![CDATA[ 
		/// <syncfusion:SfPopup x:Name="popup" 
		///						StartY="100">
		/// ]]>
		/// </code>
		/// </example>
		public int StartY
		{
			get => (int)GetValue(StartYProperty);
			set => SetValue(StartYProperty, value);
		}

		/// <summary>
        /// Gets or sets the padding space between popup overlay and the popupView.
        /// </summary>
        /// <remarks>
        /// The popupView will be padded inside the overlay when the popupView bounds intersect the padding region.
        /// </remarks>
        public new Thickness Padding
        {
            get { return (Thickness)GetValue(PaddingProperty); }
            set { SetValue(PaddingProperty, value); }
        }

		/// <summary>
		/// Gets or sets the style to be applied to the _popupView in <see cref="SfPopup"/>.
		/// </summary>
		/// <example>
		/// <para>The following code example demonstrates how to set <see cref="PopupStyle"/> property for the <see cref="SfPopup"/> control.</para>
		/// <code lang="XAML">
		/// <![CDATA[
		/// <popup:SfPopup x:Name="popup">
		///		<popup:SfPopup.PopupStyle>
		///		    <popup:PopupStyle PopupBackground="#C3B0D6" />
		///		</popup:SfPopup.PopupStyle>
		///	</popup:SfPopup>
		/// ]]>
		/// </code>
		/// </example>
		public PopupStyle PopupStyle
		{
			get => (PopupStyle)GetValue(PopupStyleProperty);
			set => SetValue(PopupStyleProperty, value);
		}

		/// <summary>
		/// Gets or sets the header height of the popup view.
		/// </summary>
		/// <example>
		/// <para>The following code example demonstrates how to set <see cref="HeaderHeight"/> property for the <see cref="SfPopup"/> control.</para>
		/// <code lang="XAML">
		/// <![CDATA[ 
		/// <syncfusion:SfPopup x:Name="popup" 
		///						HeaderHeight="100">
		/// ]]>
		/// </code>
		/// </example>
		/// <seealso cref="HeaderTemplate"/>
		/// <seealso cref="HeaderTitle"/>
		/// <seealso cref="ShowHeader"/>
		public int HeaderHeight
		{
			get => (int)GetValue(HeaderHeightProperty);
			set => SetValue(HeaderHeightProperty, value);
		}

		/// <summary>
		/// Gets or sets the footer height of the popup view.
		/// </summary>
		/// <example>
		/// <para>The following code example demonstrates how to set FooterHeight property for the <see cref="SfPopup"/> control.</para>
		/// <code lang="XAML">
		/// <![CDATA[ 
		/// <syncfusion:SfPopup x:Name="popup" 
		///						FooterHeight="100">
		/// ]]>
		/// </code>
		/// </example>
		/// <seealso cref="FooterTemplate"/>
		/// <seealso cref="ShowFooter"/>
		public int FooterHeight
		{
			get => (int)GetValue(FooterHeightProperty);
			set => SetValue(FooterHeightProperty, value);
		}

		/// <summary>
		/// Gets or sets a value indicating whether to show the close button in the header.
		/// </summary>
		/// <example>
		/// <para>The following code example demonstrates how to set <see cref="ShowCloseButton"/> property for the <see cref="SfPopup"/> control.</para>
		/// <code lang="XAML">
		/// <![CDATA[ 
		/// <syncfusion:SfPopup x:Name="popup" 
		///						ShowCloseButton="True">
		/// ]]>
		/// </code>
		/// </example>
		/// <seealso cref="HeaderTitle"/>
		/// <seealso cref="HeaderTemplate"/>
		/// <seealso cref="ShowHeader"/>
		/// <seealso cref="PopupStyle.CloseButtonIcon"/>
		public bool ShowCloseButton
		{
			get => (bool)GetValue(ShowCloseButtonProperty);
			set => SetValue(ShowCloseButtonProperty, value);
		}

		/// <summary>
		/// Gets or sets the command to invoke when the accept button in the footer is tapped.
		/// </summary>
		/// <example>
		/// Here is an example of how to set the <see cref="AcceptCommand"/> property
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <popup:SfPopup AcceptComand="Binding PopupAcceptCommand}" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// public class PopupViewModel
		/// {
		///		public ICommand PopupAcceptCommand { get; set; }
		///		public PopupViewModel()
		///		{
		///			PopupAcceptCommand = new Command(PopupAccept);
		///		}
		///		
		///		private void PopupAccept()
		///		{
		///			// You can write your set of codes that needs to be executed.
		///		}
		/// }
		/// ]]></code>
		/// </example>
		/// <seealso cref="AppearanceMode"/>
		/// <seealso cref="Opening"/>
		/// <seealso cref="Opened"/>
		/// <seealso cref="Closing"/>
		/// <seealso cref="Closed"/>
		public ICommand AcceptCommand
		{
			get => (ICommand)GetValue(AcceptCommandProperty);
			set => SetValue(AcceptCommandProperty, value);
		}

		/// <summary>
		/// Gets or sets the command to invoke when the decline button in the footer is tapped.
		/// </summary>
		/// <example>
		/// Here is an example of how to set the <see cref="DeclineCommand"/> property
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <popup:SfPopup AcceptComand="Binding PopupDeclineCommand}" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// public class PopupViewModel
		/// {
		///		public ICommand PopupDeclineCommand { get; set; }
		///		public PopupViewModel()
		///		{
		///			PopupDeclineCommand = new Command(PopupDecline);
		///		}
		///		
		///		private void PopupDecline()
		///		{
		///			// You can write your set of codes that needs to be executed.
		///		}
		/// }
		/// ]]></code>
		/// </example>
		/// <seealso cref="AppearanceMode"/>
		/// <seealso cref="Opening"/>
		/// <seealso cref="Opened"/>
		/// <seealso cref="Closing"/>
		/// <seealso cref="Closed"/>
		public ICommand DeclineCommand
		{
			get => (ICommand)GetValue(DeclineCommandProperty);
			set => SetValue(DeclineCommandProperty, value);
		}

		/// <summary>
		/// Gets or sets a value indicating whether to show the popup view in full screen or not.
		/// </summary>
		/// <remarks>
		/// If <see cref="IsFullScreen"/> is set as true, the height request and width request given for the popup view will not be considered.
		/// </remarks>
		/// <example>
		/// <para>The following code example demonstrates how to set <see cref="IsFullScreen"/> property for the <see cref="SfPopup"/> control.</para>
		/// <code lang="XAML">
		/// <![CDATA[ 
		/// <syncfusion:SfPopup x:Name="popup" 
		///						IsFullScreen="True">
		/// ]]>
		/// </code>
		/// </example>
		/// <seealso cref="IsOpen"/>
		/// <seealso cref="Show(bool)"/>
		public bool IsFullScreen
		{
			get => (bool)GetValue(IsFullScreenProperty);
			set => SetValue(IsFullScreenProperty, value);
		}

		/// <summary>
		/// Gets or sets a value that determines how to size the popup view based on its template contents.
		/// </summary>
		/// <value>
		/// The default value is <see cref="PopupAutoSizeMode.None"/>.
		/// </value>
		/// <remarks>
		/// <see cref="AutoSizeMode"/> will be applied to <see cref="SfPopup"/> only if <see cref="ContentTemplate"/> is defined.
		/// </remarks>
		/// <example>
		/// <para>The following code example demonstrates how to set <see cref="AutoSizeMode"/> property for the <see cref="SfPopup"/> control.</para>
		/// <code lang="XAML">
		/// <![CDATA[ 
		/// <syncfusion:SfPopup x:Name="popup" 
		///						AutoSizeMode="Both">
		/// ]]>
		/// </code>
		/// </example>
		/// <seealso cref="ContentTemplate"/>
		public PopupAutoSizeMode AutoSizeMode
		{
			get => (PopupAutoSizeMode)GetValue(AutoSizeModeProperty);
			set => SetValue(AutoSizeModeProperty, value);
		}

		/// <summary>
		/// Gets or sets the animation to be applied for the popup view when opening and closing it.
		/// </summary>
		/// <example>
		/// <para>The following code example demonstrates how to set <see cref="AnimationMode"/> property for the <see cref="SfPopup"/> control.</para>
		/// <code lang="XAML">
		/// <![CDATA[ 
		/// <syncfusion:SfPopup x:Name="popup" 
		///						AnimationMode="Fade">
		/// ]]>
		/// </code>
		/// </example>
		public PopupAnimationMode AnimationMode
		{
			get => (PopupAnimationMode)GetValue(AnimationModeProperty);
			set => SetValue(AnimationModeProperty, value);
		}

		/// <summary>
		/// Gets or sets the duration in milliseconds of the animation played when opening and closing the popup view.
		/// </summary>
		/// <value>
		/// The duration in milliseconds of the animation played at the opening and closing of the popup view.
		/// </value>
		/// <example>
		/// <para>The following code example demonstrates how to set <see cref="AnimationDuration"/> property for the <see cref="SfPopup"/> control.</para>
		/// <code lang="XAML">
		/// <![CDATA[ 
		/// <syncfusion:SfPopup x:Name="popup" 
		///						AnimationDuration="100">
		/// ]]>
		/// </code>
		/// </example>
		public double AnimationDuration
		{
			get => (double)GetValue(AnimationDurationProperty);
			set => SetValue(AnimationDurationProperty, value);
		}

		/// <summary>
		/// Gets or sets the animation easing effect to be applied to the popup view's opening and closing animation.
		/// </summary>
		/// <value>
		/// The animation easing effect to be applied for the popup view when it opens and closes.
		/// </value>
		/// <example>
		/// <para>The following code example demonstrates how to set <see cref="AnimationEasing"/> property for the <see cref="SfPopup"/> control.</para>
		/// <code lang="XAML">
		/// <![CDATA[ 
		/// <syncfusion:SfPopup x:Name="popup" 
		///						AnimationEasing="SinIn">
		/// ]]>
		/// </code>
		/// </example>
		public PopupAnimationEasing AnimationEasing
		{
			get => (PopupAnimationEasing)GetValue(AnimationEasingProperty);
			set => SetValue(AnimationEasingProperty, value);
		}

		/// <summary>
		/// Gets or sets a value indicating whether the action bar bounds should be considered while positioning the <see cref="SfPopup"/> .
		/// </summary>
		/// <value> <c>true</c> to consider the action bar area to position the <see cref="SfPopup"/>. otherwise, <c>false</c>.</value>
		/// <example>
		/// <para>The following code example demonstrates how to set <see cref="IgnoreActionBar"/> property for the <see cref="SfPopup"/> control.</para>
		/// <code lang="XAML">
		/// <![CDATA[ 
		/// <syncfusion:SfPopup x:Name="popup" 
		///						IgnoreActionBar="True">
		/// ]]>
		/// </code>
		/// </example>
		public bool IgnoreActionBar
		{
			get => (bool)GetValue(IgnoreActionBarProperty);
			set => SetValue(IgnoreActionBarProperty, value);
		}

		#endregion

		#region Internal Properties

		/// <summary>
		/// Gets or sets the height applied to the header of the _popupView.
		/// </summary>
		internal double AppliedHeaderHeight
		{
			get { return _appliedHeaderHeight; }
			set { _appliedHeaderHeight = value; }
		}

		/// <summary>
		/// Gets or sets the height applied to the header of the _popupView.
		/// </summary>
		internal double AppliedFooterHeight
		{
			get { return _appliedFooterHeight; }
			set { _appliedFooterHeight = value; }
		}

		/// <summary>
		/// Gets or sets the height applied to the body of the _popupView.
		/// </summary>
		internal double AppliedBodyHeight
		{
			get { return _appliedBodyHeight; }
			set { _appliedBodyHeight = value; }
		}

		/// <summary>
		/// Gets or sets the popupview content width without stroke thickness.
		/// </summary>
		internal double AppliedPopupBodyWidth
		{
			get { return _appliedPopupBodyWidth; }
			set { _appliedPopupBodyWidth = Math.Max(0, value); }
		}

		/// <summary>
		///  Gets or sets the Semantic description value.
		/// </summary>
		internal string SemanticDescription
		{
			get
			{
				if (string.IsNullOrEmpty(_semanticDescription))
				{
					_semanticDescription = SemanticProperties.GetDescription(this);
				}

				return _semanticDescription;
			}

			set
			{
				_semanticDescription = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether CanShowPopupInFullScreen true or false.
		/// </summary>
		internal bool CanShowPopupInFullScreen
		{
			get
			{
				return _canShowPopupInFullScreen;
			}

			set
			{
				_canShowPopupInFullScreen = value;
				OnPropertyChanged(nameof(CanShowPopupInFullScreen));
			}
		}

		/// <summary>
		/// Gets or sets the screen height of the device.
		/// </summary>
		internal int ScreenHeight
		{
			get { return _screenHeight; }
			set { _screenHeight = value; }
		}

		/// <summary>
		/// Gets or sets the screen width of the device.
		/// </summary>
		internal int ScreenWidth
		{
			get { return _screenWidth; }
			set { _screenWidth = value; }
		}

		/// <summary>
		/// Gets or sets the height of the status bar.
		/// </summary>
		internal double StatusBarHeight
		{
			get { return _statusBarHeight; }
			set { _statusBarHeight = value; }
		}

		/// <summary>
		/// Gets or sets the size, in pixels, of the safe area at the left edge of the screen.
		/// </summary>
		internal int SafeAreaAtLeft
		{
			get { return _safeAreaAtLeft; }
			set { _safeAreaAtLeft = value; }
		}

		/// <summary>
		/// Gets or sets the size of the safe area at the right edge of the screen, in pixels.
		/// </summary>
		internal int SafeAreaAtRight
		{
			get { return _safeAreaAtRight; }
			set { _safeAreaAtRight = value; }
		}

		/// <summary>
		/// Gets or sets the size of the safe area at the top edge of the screen, in pixels.
		/// </summary>
		internal int SafeAreaAtTop
		{
			get { return _safeAreaAtTop; }
			set { _safeAreaAtTop = value; }
		}

		/// <summary>
		/// Gets or sets the size of the safe area at the bottom edge of the screen, in pixels.
		/// </summary>
		internal int SafeAreaAtBottom
		{
			get { return _safeAreaAtBottom; }
			set { _safeAreaAtBottom = value; }
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Displays a popup with the specified title and message.
		/// </summary>
		/// <param name="title">The title of the popup.</param>
		/// <param name="message">The message to be displayed in the popup.</param>
		/// <param name="autoCloseDuration">The delay in milliseconds after which the popup will automatically close.</param>
		/// <example>
		/// <para>The following code example demonstrates how to display <see cref="SfPopup"/> control using <see cref="Show(string,string,int)"/> method.</para>
		/// <code lang="C#">
		/// <![CDATA[ 
		///	void OnButton_Clicked(object sender, EventArgs e)
		///	{
		///		popup.Show("HeaderTitle","PopupMessageView",1000);	
		/// }
		/// ]]>
		/// </code>
		/// </example>
		public static void Show(string title, string message, int autoCloseDuration = 0)
		{
			SfPopup popup = new SfPopup()
			{
				HeaderTitle = title,
				Message = message,
				AutoCloseDuration = autoCloseDuration,
			};

			popup.Show();
		}

		/// <summary>
		/// Displays a popup with the specified title and message, along with an accept button.
		/// </summary>
		/// <param name="title">The title of the popup.</param>
		/// <param name="message">The message to be displayed in the popup.</param>
		/// <param name="acceptText">The text to be displayed on the accept button in the footer.</param>
		/// <param name="autoCloseDuration">The delay in milliseconds after which the popup will automatically close.</param>
		/// <example>
		/// <para>The following code example demonstrates how to display <see cref="SfPopup"/> control using <see cref="Show(string,string,string,int)"/> method.</para>
		/// <code lang="C#">
		/// <![CDATA[ 
		///	void OnButton_Clicked(object sender, EventArgs e)
		///	{
		///		popup.Show("HeaderTitle","PopupMessageView","OK",1000);	
		/// }
		/// ]]>
		/// </code>
		/// </example>
		public static void Show(string title, string message, string acceptText, int autoCloseDuration = 0)
		{
			SfPopup popup = new SfPopup()
			{
				HeaderTitle = title,
				Message = message,
				AcceptButtonText = acceptText,
				ShowFooter = true,
				AutoCloseDuration = autoCloseDuration,
			};
			popup.Show();
		}

		/// <summary>
		/// Displays a popup with the specified title, message, accept button, and a decline button with the provided text.
		/// </summary>
		/// <param name="title">The title of the popup.</param>
		/// <param name="message">The message to be displayed in the popup.</param>
		/// <param name="acceptText">The text to be displayed on the accept button in the footer.</param>
		/// <param name="declineText">The text to be displayed on the decline button in the footer.</param>
		/// <param name="autoCloseDuration">The delay in milliseconds after which the popup will automatically close.</param>
		/// <returns>A task representing the asynchronous operation.
		/// The result will be true if the popup was closed using the accept button; otherwise, false.</returns>
		/// <example>
		/// <para>The following code example demonstrates how to display <see cref="SfPopup"/> control using <see cref="Show(string,string,string,string,int)"/> method.</para>
		/// <code lang="C#">
		/// <![CDATA[ 
		///	void OnButton_Clicked(object sender, EventArgs e)
		///	{
		///		popup.Show("HeaderTitle","PopupMessageView","OK","Cancel",1000);	
		/// }
		/// ]]>
		/// </code>
		/// </example>
		public static async Task<bool> Show(string title, string message, string acceptText, string declineText, int autoCloseDuration = 0)
		{
			SfPopup popup = new SfPopup()
			{
				HeaderTitle = title,
				Message = message,
				AcceptButtonText = acceptText,
				DeclineButtonText = declineText,
				ShowHeader = true,
				ShowFooter = true,
				AppearanceMode = PopupButtonAppearanceMode.TwoButton,
				AutoCloseDuration = autoCloseDuration,
			};
			popup._taskCompletionSource = new TaskCompletionSource<bool>();
			popup.Show();
			return await popup._taskCompletionSource.Task;
		}

		/// <summary>
		/// Displays a Popup.
		/// </summary>
		/// <returns>A task representing the asynchronous operation.
		/// The result will be true if the popup was closed using the accept button; otherwise, false.
		/// </returns>
		/// <example>
		/// <para>The following code example demonstrates how to display <see cref="SfPopup"/> control using <see cref="ShowAsync()"/> method.</para>
		/// <code lang="C#"><![CDATA[
		///	async void OnButton_Clicked(object sender, EventArgs e)
		///	{
		///		var result = await popup.ShowAsync();
		/// }
		/// ]]></code>
		///  </example>
		public async Task<bool> ShowAsync()
		{
			_taskCompletionSource = new TaskCompletionSource<bool>();
			Show();
			return await _taskCompletionSource.Task;
		}

		/// <summary>
		/// Displays the Popup in the screen.
		/// </summary>
		/// <value>
		/// Displays the Popup on the screen. The default value is false, which indicates the popup is not displayed in full screen.
		/// </value>
		/// <remarks>
		/// The <see cref="Show(bool)"/> method allows you to specify whether the popup should be displayed in full screen by passing a boolean value for the <paramref name="isfullscreen"/> parameter.
		/// </remarks>
		/// <param name="isfullscreen">Specifies whether the popup should be displayed in full screen or not.</param>
		/// <example>
		/// <para>The following code example demonstrates how to display <see cref="SfPopup"/> control using <see cref="Show(bool)"/> method.</para>
		/// <code lang="C#">
		/// <![CDATA[ 
		///	void OnButton_Clicked(object sender, EventArgs e)
		///	{
		///		popup.Show();	
		/// }
		/// ]]>
		/// </code>
		/// </example>
		/// <seealso cref="Show(double, double)"/>
		/// <seealso cref="ShowRelativeToView(View, PopupRelativePosition, double, double)"/>
		/// <seealso cref="IsOpen"/>
		/// <seealso cref="IsFullScreen"/>
		public void Show(bool isfullscreen = false)
		{
			if (_popupOverlay is null)
			{
				return;
			}

			if (!IsOpen)
			{
				_showFullScreen = isfullscreen;
				OpenOrClosePopup(true);
			}
		}

		/// <summary>
		/// Displays the popup at the specific x and y point.
		/// </summary>
		/// <value>
		/// Displays the popup at the specified x and y position on the screen. The default value is (0, 0), indicating the top-left corner.
		/// </value>
		/// <remarks>
		/// This method allows you to specify the exact screen coordinates where the popup should appear, based on the provided <paramref name="xPosition"/> and <paramref name="yPosition"/> values.
		/// </remarks>
		/// <param name="xPosition">The x point at which the popup should be displayed.</param>
		/// <param name="yPosition">The y point at which the popup should be displayed.</param>
		/// <example>
		/// <para>The following code example demonstrates how to display <see cref="SfPopup"/> control using <see cref="Show(double, double)"/> method.</para>
		/// <code lang="C#">
		/// <![CDATA[ 
		///	void OnButton_Clicked(object sender, EventArgs e)
		///	{
		///		popup.Show(100,100);	
		/// }
		/// ]]>
		/// </code>
		/// </example>
		/// <seealso cref="Show(bool)"/>
		/// <seealso cref="ShowRelativeToView(View, PopupRelativePosition, double, double)"/>
		/// <seealso cref="IsOpen"/>
		/// <seealso cref="IsFullScreen"/>
		public void Show(double xPosition, double yPosition)
		{
			_showXPosition = Math.Max(0, xPosition);
			_showYPosition = Math.Max(0, yPosition);

			if (!IsOpen)
			{
				OpenOrClosePopup(true);
			}
		}

		/// <summary>
		/// Displays popup relative to the given view.
		/// </summary>
		/// <value>
		/// Displays the popup relative to a specified view, adjusting its position based on the given relative position and optional absolute coordinates.
		/// </value>
		/// <remarks>
		/// Positions the popup relative to the specified view, with optional absolute offsets. The popup is updated if already open, and blur effects are applied on Android. Resizing is recalculated if AutoSizeMode is enabled.
		/// </remarks>
		/// <param name="relativeView">The view relative to which popup should be displayed.</param>
		/// <param name="relativePosition">The position where popup should be displayed relative to the given view.</param>
		/// <param name="absoluteX">Absolute x Point where the popup should be positioned from the relative view.</param>
		/// <param name="absoluteY">Absolute y Point where the popup should be positioned from the relative view.</param>
		/// <example>
		/// <para>The following code example demonstrates how to display <see cref="SfPopup"/> control using <see cref="ShowRelativeToView(View, PopupRelativePosition, double, double)"/> method.</para>
		/// <code lang="C#">
		/// <![CDATA[
		/// void setRelativeView_Clicked(object sender, EventArgs e)
		/// {
		///		sfPopup.ShowRelativeToView(showPopup, PopupRelativePosition.AlignToRightOf, 10, 10);
		/// }
		/// ]]>
		/// </code>
		/// </example>
		/// <seealso cref="Show(bool)"/>
		/// <seealso cref="Show(double, double)"/>
		/// <seealso cref="IsOpen"/>
		/// <seealso cref="IsFullScreen"/>
		public void ShowRelativeToView(View relativeView, PopupRelativePosition relativePosition, double absoluteX = double.NaN, double absoluteY = double.NaN)
		{
			if (relativeView is null || relativeView.Handler is null)
			{
				return;
			}

			_relativeView = relativeView;
			_relativePosition = relativePosition;
			_absoluteXPoint = double.IsNaN(absoluteX) ? 0 : absoluteX;
			_absoluteYPoint = double.IsNaN(absoluteY) ? 0 : absoluteY;

			if (!IsOpen)
			{
				OpenOrClosePopup(true);
			}
		}

		/// <summary>
		/// Dismisses the <see cref="_popupView"/> from the view.
		/// </summary>
		/// <example>
		/// <para>The following code demonstrates how to dismiss the <see cref="SfPopup"/>.</para>
		/// <code lang="C#">
		/// <![CDATA[ 
		///	void DismissButton_Clicked(object sender, EventArgs e)
		///	{
		///		popup.Dismiss();	
		/// }
		/// ]]>
		/// </code>
		/// </example>
		public void Dismiss()
		{
			// Maui:990257 When Dismiss() is called, the popup should close regardless of whether the StaysOpen property is set to true.
			if (IsOpen)
			{
				OpenOrClosePopup(false);
			}
		}

		/// <summary>
		/// Refreshes the popup view for run-time value changes.
		/// </summary>
		public void Refresh()
		{
			if (IsOpen && _popupView is not null)
			{
				ResetPopupWidthHeight();
				_popupView.ApplyShadowAndCornerRadius();
				_popupView.InvalidateForceLayout();
			}
		}

		#endregion

		#region Internal Methods

		/// <summary>
		/// Sets the semantic description value to the respective child elements.
		/// </summary>
		internal void SetSemanticDescription()
		{
			if (_popupView is not null && _popupView._headerView is not null)
			{
				if (_popupView._headerView._popupCloseButton is not null)
				{
					SemanticProperties.SetDescription(_popupView._headerView._popupCloseButton, SemanticDescription + " Close Button");
				}

#if WINDOWS
				if (_popupView._headerView._titleLabel is not null)
				{
					SemanticProperties.SetDescription(_popupView._headerView._titleLabel, SemanticDescription);
				}

				if (_popupView._popupMessageView is not null)
				{
					SemanticProperties.SetDescription(_popupView._popupMessageView._messageView, SemanticDescription);
				}
#else
				if (_popupView._headerView._titleLabel is not null)
				{
					SemanticProperties.SetDescription(_popupView._headerView._titleLabel, SemanticDescription + " Title");
				}

				if (_popupView._popupMessageView is not null)
				{
					SemanticProperties.SetDescription(_popupView._popupMessageView._messageView, SemanticDescription + " Message view");
				}
#endif
			}
		}

		/// <summary>
		/// Dismisses the <see cref="_popupView"/> from the view.
		/// </summary>
		/// <returns>Returns whether to cancel closing of the Popup.</returns>
		internal bool DismissPopup()
		{
			if (_popupView is null || !_popupView.IsVisible)
			{
				return false;
			}

			bool cancel = RaisePopupClosingEvent();
			if (!cancel)
			{
				if (_popupOverlay is not null)
				{
					if (ShowOverlayAlways)
					{
						PopupExtension.ClearBlurViews(this);
					}

					ApplyContainerAnimation();
					ApplyPopupAnimation();
					UnWireEvents();
					UnWirePlatformSpecificEvents();

					_relativeView = null;
					_relativePosition = PopupRelativePosition.AlignTop;
					_absoluteXPoint = -1;
					_absoluteYPoint = -1;
					_showXPosition = -1;
					_showYPosition = -1;
					_keyboardHeight = 0;
					SetPopupViewDefaultHeight();

					if (_popupOverlayContainer is not null)
					{
						_popupOverlayContainer.Parent = null;
					}

					if (_popupView is not null)
					{
						_popupView.Parent = null;
					}
				}

				if (_popupView is not null && _popupView._popup._taskCompletionSource is not null)
				{
					_popupView._popup._taskCompletionSource.SetResult(_popupView._acceptButtonClicked);
					_popupView._popup._taskCompletionSource = null;
				}
			}

			if (_popupView is not null)
			{
				_popupView._acceptButtonClicked = false;
			}

			return cancel;
		}

		/// <summary>
		/// Raises the <see cref="Closed"/> event.
		/// </summary>
		internal void RaisePopupClosedEvent()
		{
			if (Closed is not null)
			{
				Closed(this, EventArgs.Empty);
			}
		}

		/// <summary>
		/// Raises the Popup Closing event.
		/// </summary>
		/// <returns>Returns whether to cancel closing of the Popup.</returns>
		internal bool RaisePopupClosingEvent()
		{
			if (Closing is not null)
			{
				var popupClosingEventArgs = new CancelEventArgs();
				Closing(this, popupClosingEventArgs);
				return popupClosingEventArgs.Cancel;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		///  Raises the <see cref="Opened"/> event.
		/// </summary>
		internal void RaisePopupOpenedEvent()
		{
			if (Opened is not null)
			{
				Opened(this, EventArgs.Empty);
			}
		}

		/// <summary>
		/// Raises the Popup Opening event.
		/// </summary>
		/// <returns>Returns whether to cancel opening of the Popup.</returns>
		internal bool RaisePopupOpeningEvent()
		{
			if (Opening is not null)
			{
				var popupOpeningEventArgs = new CancelEventArgs();
				Opening(this, popupOpeningEventArgs);
				return popupOpeningEventArgs.Cancel;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Closes the popup, when touch is made outside the popup view, provided StaysOpen is false.
		/// </summary>
		internal void ClosePopupIfRequired()
		{
			if (!StaysOpen)
			{
				IsOpen = false;
			}
		}

		/// <summary>
		/// Checks whether the layout of the popup view is made for the given size and position.
		/// </summary>
		/// <returns>
		/// True, if popup layout for given size and position. False otherwise.
		/// </returns>
		/// <remarks>
		/// If the layout is made for the given size and position, _popupView will not be layout for the
		/// full view of the application. Hence in this case, the calculations must be made for the <see cref="_popupOverlay"/> instead of the _popupView.
		/// In order to check whether the calculations must be made either to the
		/// _popupView or Container, this case is checked.
		/// </remarks>
		internal bool CanLayoutForGivenSizeAndPosition()
		{
			// TODO: When implementing OpenAtTouchPoint
			return true;
		}

		/// <summary>
		/// Get template for the popup view content, header and footer.
		/// </summary>
		/// <param name="template">template to select.</param>
		/// <returns template="DataTemplate">Selected template.</returns>
		internal DataTemplate GetTemplate(DataTemplate template)
		{
			var selector = template as DataTemplateSelector;
			if (selector is not null)
			{
				template = selector.SelectTemplate(this, null);
			}

			return template;
		}

		/// <summary>
		/// Used to reset the popupview width and height.
		/// </summary>
		internal void ResetPopupWidthHeight()
		{
			CalculatePopupSizeAndPosition();

			// Maui-4921 : Runtime changes.
			UpdatePopupStyles();
		}

		/// <summary>
		/// Used to rest the Popup based on DynamicContentSize in AutoSize.
		/// </summary>
		internal void ResetPopupBasedOnDynamicContentSize()
		{
			if (IsOpen)
			{
				ResetPopupWidthHeight();
			}
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Initializes the properties for <see cref="SfPopup"/>.
		/// </summary>
		void Initialize()
		{
			SfPopupResources.InitializeDefaultResource("Syncfusion.Maui.Toolkit.Popup.Resources.SfPopup", typeof(SfPopup));
			SetPopupViewDefaultHeight();
			_showXPosition = -1;
			_showYPosition = -1;
			_popupView = new PopupView(this);
			_popupOverlay = new SfWindowOverlay();
			_popupOverlayContainer = new SfPopupOverlayContainer(this);
			_popupOverlay.SetWindowOverlayContainer(_popupOverlayContainer);
			SetPopupPositionBasedOnKeyboard();
			InitializeOverlay();
			if (!HeaderTitle.Equals("Title", StringComparison.Ordinal))
			{
				_popupView.SetHeaderTitleText(HeaderTitle);
			}

			if (!Message.Equals("Popup Message", StringComparison.Ordinal))
			{
				_popupView.SetMessageText(Message);
			}

			if (!AcceptButtonText.Equals("ACCEPT", StringComparison.Ordinal))
			{
				_popupView.SetAcceptButtonText(AcceptButtonText);
			}

			if (!DeclineButtonText.Equals("DECLINE", StringComparison.Ordinal))
			{
				_popupView.SetDeclineButtonText(DeclineButtonText);
			}
		}

		/// <summary>
		/// Initializes the overlay for the popup view. This method sets up the necessary conditions and native views
		/// required for displaying the popup in scenarios where the popup is initialized in the constructor or XAML
		/// of the first page in the app, or in shell context. It ensures that events are wired or unwired appropriately
		/// based on the overlay success status.
		/// </summary>
		void InitializeOverlay()
		{
			// This method is used to initialize the native views for the PopupView. When popup initialized in the constrctuor of first page in the app or in Xaml.
			// Example - new Window(new MainPage() or new NavigationPage(new Mainpage() or shell), Popup either declared in Xaml of mainpage or in the constructor of main page.
			if (!_isOverlayAdded && _popupOverlay is not null && _popupView is not null)
			{
				if (_popupOverlay.AddToOverlay(_popupView))
				{
					_isOverlayAdded = true;
					UnWireEvents();
				}
				else
				{
					UnWireEvents();
					WireEvents();
#if WINDOWS
                    // PlatformRootview got intialized but its parent - the XamlRoot will not be set if pages were initialized in constructor.
                    WirePlatformViewLoaded();
#endif
				}
			}
		}

		/// <summary>
		/// Gets or sets the time delay in milliseconds for automatically closing the Popup. The Default value is 0.
		/// </summary>
		async void AutoClosePopup()
		{
			await Task.Delay(AutoCloseDuration);
			IsOpen = false;
		}

		/// <summary>
		/// Updates all popup-related dimension fields, including screen size, bar heights,
		/// and safe area insets, ensuring the popup is properly sized and positioned
		/// according to the current device's display metrics and safe zones.
		/// </summary>
		void SyncPopupDimensionFields()
		{
			ScreenHeight = PopupExtension.GetScreenHeight();
			ScreenWidth = PopupExtension.GetScreenWidth();
			StatusBarHeight = PopupExtension.GetStatusBarHeight();
			SafeAreaAtLeft = PopupExtension.GetSafeAreaHeight("Left");
			SafeAreaAtRight = PopupExtension.GetSafeAreaHeight("Right");
			SafeAreaAtTop = PopupExtension.GetSafeAreaHeight("Top");
			SafeAreaAtBottom = PopupExtension.GetSafeAreaHeight("Bottom");
		}

		/// <summary>
		/// Used to set <see cref="IsOpen"/> value.
		/// </summary>
		/// <param name="open">The value to be set to the IsOpen property.</param>
		void OpenOrClosePopup(bool open)
		{
			IsOpen = open;
		}

		/// <summary>
		/// Used to trigger closed or opened event.
		/// </summary>
		void RaisePopupEvent()
		{
			if (!IsOpen)
			{
				RaisePopupClosedEvent();
			}
			else
			{
				RaisePopupOpenedEvent();
			}
		}

		/// <summary>
		/// Displays the <see cref="_popupView"/> in the view.
		/// </summary>
		void DisplayPopup()
		{
			// 989436 : [Android] Popup is not shown when IsOpen is set in XAML or constructor at loading with a Shell page.
			// In Android, the Shell page is not loaded initially, so the popup is displayed only after the Shell page has loaded.
			if (_popupView == null || (PopupExtension.GetMainWindowPage() is Shell shellPage && !shellPage.IsLoaded))
			{
				_isOpenDeferred = true;
				WireEvents();
				return;
			}
#if ANDROID
			// 926747 : The WindowManager parameters reset when the popup is closed. Therefore, the LayoutNoLimits flag must be set when the popup is opened.
			SetWindowFlags();
#endif
			SyncPopupDimensionFields();

			// 848559: Implementation for accessibility.
			SetSemanticDescription();

			// When Popup is loaded in ShellPage and Show the popup directly in constructor or xaml page, shell CurrentPage is null and Popup is not displayed in iOS and MAC.
			// Maui:826232  Exception thrown when show() is directly called from Constructor & OnAppearing, as handler is null. Added condition for handler to hook the wire events.
			SetParent();

			// change Popup close button icon at runtime
			_popupView._headerView?.UpdateHeaderCloseButton();

			CalculatePopupSizeAndPosition();
			ApplyOverlayBackground();

			// When set the AutoSizeMode, Popup MessageView is measured after added the view. So, we need to calculate the Height, width and position again.
			UpdatePopupView();
			if (!RaisePopupOpeningEvent())
			{
				ApplyContainerAnimation();
				ApplyPopupAnimation();
				WireEvents();

				// position the popup when window size and orientation changed.
				WirePlatformSpecificEvents();
			}
			else
			{
				OpenOrClosePopup(false);
				RemovePopupViewAndResetValues();
			}
		}

		/// <summary>
		/// Used to calculate the popup size and position.
		/// </summary>
		void CalculatePopupSizeAndPosition()
		{
			// Calculates the height and width of popup.
			CalculateSize();

			// Position Popup view
			PositionPopupView();
		}

		/// <summary>
		/// Used to update the _popupView while set the AutoSizeMode.
		/// </summary>
		void UpdatePopupView()
		{
			if (AutoSizeMode != PopupAutoSizeMode.None && !CanShowPopupInFullScreen)
			{
				ResetPopupWidthHeight();
				return;
			}

			UpdatePopupStyles();
		}

		/// <summary>
		/// Used to update the PopupStyles.
		/// </summary>
		void UpdatePopupStyles()
		{
			if (_popupView is null)
			{
				return;
			}

			_popupView._popupMessageView?.UpdateMessageViewStyle();
			_popupView._headerView?.UpdateHeaderAppearance();
			_popupView._footerView?.UpdateFooterAppearance();
			_popupView.ApplyShadowAndCornerRadius();
			ApplyOverlayBackground();

#if WINDOWS
			_popupView.Background = PopupStyle.GetPopupBackground();
#endif

#if IOS
			// To Do: When using the _popupView.Background property to set the background, the background layer will be named "MauiBackgroundLayer".However, during the UpdateBackground process to remove the old layer, there is a mistake in the code where they have checked for Name == "BackgroundLayer" instead of "MauiBackgroundLayer".This oversight causes the old layer to remain in the view, leading to unintended behavior.To resolve this issue, we should use UpdateBackground method always.
			UpdateNativePopupViewBackground(PopupStyle.GetPopupBackground());
#endif
		}

		/// <summary>
		/// Used to update background for popup overlay.
		/// </summary>
		void ApplyOverlayBackground()
		{
			if (_popupOverlayContainer is not null)
			{
				if (ShowOverlayAlways)
				{
					if (OverlayMode is PopupOverlayMode.Transparent)
					{
						_popupOverlayContainer.ApplyBackgroundColor(PopupStyle.GetOverlayColor());
					}
					else if (OverlayMode is PopupOverlayMode.Blur)
					{
#if WINDOWS
						_popupOverlayContainer.ApplyBackgroundColor(Colors.Transparent);
#endif
						PopupExtension.Blur(_popupOverlayContainer, this, IsOpen);
					}
				}
				else
				{
					_popupOverlayContainer.ApplyBackgroundColor(Colors.Transparent);
				}
			}
		}

		/// <summary>
		/// X and Y position of the _popupView is set here.
		/// </summary>
		void PositionPopupView()
		{
			var leftPadding = GetPadding().Left;
			var topPadding = GetPadding().Top;
			var rightPadding = GetPadding().Right;
			var bottomPadding = GetPadding().Bottom;
			var horizontalStartPoint = SafeAreaAtLeft;
			var horizontalEndPoint = ScreenWidth - SafeAreaAtRight - (_isRTL ? leftPadding : rightPadding);
			var verticalStartPoint = SafeAreaAtTop + GetActionBarHeight() + StatusBarHeight;
			var verticalEndPoint = ScreenHeight - _keyboardHeight - SafeAreaAtBottom - bottomPadding;

			double x = 0;
			double y = 0;

			if (CanShowPopupInFullScreen)
			{
				x = (double)horizontalStartPoint;
				y = (double)verticalStartPoint;
			}
			else if (_relativeView is not null)
			{
				CalculateRelativePoint(_relativeView, _relativePosition, _absoluteXPoint, _absoluteYPoint, ref x, ref y);
			}
			else
			{
                x = _isRTL ? horizontalEndPoint - _showXPosition - _popupViewWidth : _showXPosition + horizontalStartPoint;
                y = _showYPosition + verticalStartPoint;

                if (_showXPosition is -1 && _showYPosition is -1)
                {
                    if (RelativeView is not null && RelativeView.Handler is not null && RelativeView.Handler.PlatformView is not null)
                    {
                        CalculateRelativePoint(RelativeView, RelativePosition, AbsoluteX, AbsoluteY, ref x, ref y);
                    }
                    else
                    {
                        x = StartX is -1 ? (ScreenWidth - _popupViewWidth) / 2 : _isRTL ? horizontalEndPoint - StartX - _popupViewWidth : StartX + horizontalStartPoint;
                        y = StartY is -1 ? (ScreenHeight - _popupViewHeight) / 2 : StartY + verticalStartPoint;
                    }
                }
            }

            x = ValidatePopupPosition(x, _popupViewWidth, horizontalStartPoint + (_isRTL ? rightPadding : leftPadding), horizontalEndPoint);
            y = ValidatePopupPosition(y, _popupViewHeight, verticalStartPoint + topPadding, verticalEndPoint);
            LayoutPopup(x, y);
		}

		/// <summary>
		/// Validates the position of the popup view.
		/// </summary>
		/// <param name="point">The Point calculated to position the popup.</param>
		/// <param name="popupViewSize">The actual size of the view.</param>
		/// <param name="screenStartingPoint">Indicates the starting bounds of the application.</param>
		/// <param name="screenEndPoint">Indicates the end bounds of the application.</param>
		/// <returns>Returns the validated position of the popup view.</returns>
		private double ValidatePopupPosition(double point, double popupViewSize, double screenStartingPoint, double screenEndPoint)
		{
			if (point < screenStartingPoint)
			{
				return screenStartingPoint;
			}
			else if (point + popupViewSize > screenEndPoint)
			{
				return screenEndPoint - popupViewSize;
			}
			else
			{
				return point;
			}
		}

		/// <summary>
		/// Calculates the X and Y point of the popup, relative to the given view.
		/// </summary>
		/// <param name="relativeView">Positions the popup view relatively to the relative view.</param>
		/// <param name="position">The relative position from the view.</param>
		/// <param name="absoluteX">Absolute X Point where the popup should be positioned from the relative view.</param>
		/// <param name="absoluteY">Absolute Y-Point where the popup should be positioned from the relative view.</param>
		/// <param name="relativeX">References the X position of popup relative to view.</param>
		/// <param name="relativeY">References the Y position of popup relative to view.</param>
		void CalculateRelativePoint(MauiView relativeView, PopupRelativePosition position, double absoluteX, double absoluteY, ref double relativeX, ref double relativeY)
		{
			Rect relativeViewBounds = this.GetRelativeViewBounds(relativeView);
			relativeViewBounds.X += _isRTL ? -absoluteX : absoluteX;
			relativeViewBounds.Y += absoluteY;
			if (position is PopupRelativePosition.AlignToLeftOf || position is PopupRelativePosition.AlignTopLeft || position is PopupRelativePosition.AlignBottomLeft)
			{
				if (_isRTL)
				{
					relativeX = relativeViewBounds.X + relativeViewBounds.Width;
				}
				else
				{
					relativeX = relativeViewBounds.X - _popupViewWidth;
				}
			}
			else if (position is PopupRelativePosition.AlignToRightOf || position is PopupRelativePosition.AlignTopRight || position is PopupRelativePosition.AlignBottomRight)
			{
				if (_isRTL)
				{
					relativeX = relativeViewBounds.X - _popupViewWidth;
				}
				else
				{
					relativeX = relativeViewBounds.X + relativeViewBounds.Width;
				}
			}
			else
			{
				if (_isRTL)
				{
					relativeX = relativeViewBounds.X + relativeViewBounds.Width - _popupViewWidth;
				}
				else
				{
					relativeX = relativeViewBounds.X;
				}
			}

			// Calculating the Y-Position relative to the given view.
			if (position is PopupRelativePosition.AlignTop || position is PopupRelativePosition.AlignTopLeft || position is PopupRelativePosition.AlignTopRight)
			{
				relativeY = relativeViewBounds.Y - _popupViewHeight;
			}
			else if (position is PopupRelativePosition.AlignBottom || position is PopupRelativePosition.AlignBottomLeft || position is PopupRelativePosition.AlignBottomRight)
			{
				relativeY = relativeViewBounds.Y + relativeViewBounds.Height;
			}
			else
			{
				relativeY = relativeViewBounds.Y;
			}
		}

		/// <summary>
		/// Validates whether the IgnoreActionBar should be considered or not and gets the actionbar height.
		/// </summary>
		/// <returns>Returns action bar height.</returns>
		int GetActionBarHeight()
		{
			if (CanShowPopupInFullScreen || ((StartY is not -1 || _showYPosition is not -1) && IgnoreActionBar))
			{
				return 0;
			}
			else
			{
				return PopupExtension.GetActionBarHeight();
			}
		}

		/// <summary>
		/// This method adds or updates the _popupView at the specified X and Y coordinates.
		/// </summary>
		/// <param name="x">The X-coordinate for positioning the popup.</param>
		/// <param name="y">The Y-coordinate for positioning the popup.</param>
		void LayoutPopup(double x, double y)
		{
			_popupXPosition = x;
			_popupYPosition = y;

			if (_popupView is not null)
			{
				// Popup height doesn't resize automatically when content changes dynamically.Here skip calling ResetPopupWidthHeight from the _popupView's MeasureContent.
				if (_popupView._popupMessageView is not null)
				{
					_popupView._popupMessageView._isContentModified = false;
				}

				_popupOverlay?.PositionOverlayContent(x, y);

#if ANDROID
            // 936804 : The navigation bar does not hide when opening a popup, even though it hides for the window in the sample. The overlay is not set to full screen when the window has LayoutNoLimits and FullScreen enabled.
            HideNavigationBar();
#endif
#if IOS || MACCATALYST
				if (_popupView is not null && _popupView.Handler is not null && _popupView.Handler.HasContainer)
				{
					// In iOS and MAC, a WrapperView is created with framework shadow and added as a subview of the overlay. _popupView is added as a child of WrapperView, but its bounds are not updated by default.
					UpdateBounds();
					ApplyNativePopupViewClip();
				}
#endif
			}
		}

		/// <summary>
		/// Width of the PopupView and AppliedPopupWidth of the popup is calculated here.
		/// </summary>
		bool CalculatePopupViewWidth(double availableWidth, double availableHeight)
		{
			double strokeThickness = PopupStyle.GetStrokeThickness();
			double tempPopupViewWidth = 0;

			if (CanShowPopupInFullScreen)
			{
				tempPopupViewWidth = availableWidth;
			}
			else
			{
				double contentWidthRequest = -1;
				if (ContentTemplate is not null && _popupView is not null && _popupView._popupMessageView is not null && _popupView._popupMessageView.Content is not null
				&& (WidthRequest >= 0 || _popupView._popupMessageView.WidthRequest >= 0))
				{
					// MAUI-826711 Created multiple instance of ContentTemplate in Maui SfPopup.
					// Removed the CreateContent() and used the PopupView.PopupMessageView.Content.
					contentWidthRequest = GetFinalWidth(_popupView._popupMessageView.Content as View);
				}

				// PopupView width request has the highest priority.
				// If user sets any value to the PopupView.WidthRequest or MinimumWidthRequest
				// then popupview will be layout to that width only.
				if (contentWidthRequest >= 0 || WidthRequest >= 0)
				{
					// to-do - template width based.
					// MAUI-826711 Created multiple instance of ContentTemplate in Maui SfPopup.
					// Removed the CreateContent() and used the PopupView.PopupMessageView.Content.
					double popupSizeRequest = WidthRequest;
					if (popupSizeRequest > contentWidthRequest)
					{
						tempPopupViewWidth = Math.Max(MinimumWidthRequest, popupSizeRequest);
					}
					else
					{
						tempPopupViewWidth = contentWidthRequest + strokeThickness;
					}
				}
				else
				{
					if (AutoSizeMode is PopupAutoSizeMode.Height || AutoSizeMode is PopupAutoSizeMode.None || ContentTemplate is null)
					{
						tempPopupViewWidth = Math.Max(MinimumWidthRequest, _defaultPopupViewWidth);
					}
					else
					{
						return true;
					}
				}
			}

			ValidateAndSetPopupWidth(availableWidth, calculatedPopupViewWidth: tempPopupViewWidth);
			return false;
		}

		int GetFinalWidth(View template)
		{
			if (template is not null)
			{
				if (template.MinimumWidthRequest >= 0 || template.WidthRequest >= 0)
				{
					return (int)Math.Max(template.MinimumWidthRequest, template.WidthRequest);
				}
			}

			return -1;
		}

		int GetFinalHeight(View template)
		{
			if (template is not null)
			{
				if (template.MinimumHeightRequest >= 0 || template.HeightRequest >= 0)
				{
					return (int)Math.Max(template.MinimumHeightRequest, template.HeightRequest);
				}
			}

			return -1;
		}

		/// <summary>
		/// Padding value is validated with minimal padding.
		/// </summary>
		/// <returns> Return the maximum padding value with strokethickness.</returns>
		Thickness GetPadding()
		{
			if (Padding.IsEmpty)
			{
				return Padding;
			}
			else
			{
				return new Thickness(Math.Max(Padding.Left, _minimalPadding), Math.Max(Padding.Top, _minimalPadding), Math.Max(Padding.Right, _minimalPadding), Math.Max(Padding.Bottom, _minimalPadding));
			}
		}

		/// <summary>
		/// PopupView size is set here.
		/// </summary>
		void CalculateSize(double availableScreenWidth = -1, double availableScreenHeight = -1)
		{
			var availableWidth = availableScreenWidth is not -1 ? availableScreenWidth : (ScreenWidth - (GetPadding().Left + GetPadding().Right + SafeAreaAtLeft + SafeAreaAtRight));
			var availableHeight = availableScreenHeight != -1 ? availableScreenHeight : (ScreenHeight - (GetPadding().Top + GetPadding().Bottom + SafeAreaAtTop + SafeAreaAtBottom + GetActionBarHeight() + StatusBarHeight + _keyboardHeight));
			ResetHeaderFooterHeight();
			bool canNeedAutoSizeWidth = CalculatePopupViewWidth(availableWidth, availableHeight);
			bool canNeedAutoSizeHeight = CalculatePopupViewHeight(availableWidth, availableHeight);

			if (canNeedAutoSizeHeight || canNeedAutoSizeWidth)
			{
				CalculateAutoSize(availableWidth, availableHeight, canNeedAutoSizeWidth, canNeedAutoSizeHeight);
			}
		}

		/// <summary>
		/// Height of the PopupView and AppliedBodyHeight of the popup is calculated here.
		/// </summary>
		bool CalculatePopupViewHeight(double availableWidth, double availableHeight)
		{
            double strokeThickness = PopupStyle.GetStrokeThickness();
            double tempPopupViewHeight = 0;
            if (CanShowPopupInFullScreen)
            {
                tempPopupViewHeight = availableHeight;
            }
            else
            {
                double contentHeightRequest = -1;
                if (ContentTemplate is not null && _popupView is not null && _popupView._popupMessageView is not null && _popupView._popupMessageView.Content is not null
                    && (HeightRequest >= 0 || _popupView._popupMessageView.HeightRequest >= 0))
                {
                    // MAUI-826711 Created multiple instance of ContentTemplate in Maui SfPopup.
                    // Removed the CreateContent() and used the PopupView.PopupMessageView.Content.
                    contentHeightRequest = GetFinalHeight(_popupView._popupMessageView.Content as View);
                }

                if (contentHeightRequest >= 0)
                {
                    tempPopupViewHeight = contentHeightRequest + AppliedHeaderHeight + AppliedFooterHeight + strokeThickness;
                }
                else if (HeightRequest >= 0)
                {
                    tempPopupViewHeight = Math.Max(HeightRequest, MinimumHeightRequest);
                }
                else
                {
                    if (AutoSizeMode is PopupAutoSizeMode.Width || AutoSizeMode is PopupAutoSizeMode.None || ContentTemplate is null)
                    {
                        tempPopupViewHeight = Math.Max(MinimumHeightRequest, _defaultPopupViewHeight + AppliedHeaderHeight + AppliedFooterHeight);
                    }
                    else
                    {
                        return true;
                    }
                }
            }

            ValidateAndSetPopupHeight(availableHeight, calculatedPopupViewHeight: tempPopupViewHeight);
            return false;
		}

		/// <summary>
		/// Calculates and return content template content measured size by passing the avilable width and height when AutoSizeMode is set.
		/// </summary>
		void CalculateAutoSize(double availableWidth, double availableHeight, bool canAutoSizeWidth, bool canAutoSizeHeight)
		{
            // MAUI-826711 Created multiple instance of ContentTemplate in Maui SfPopup.
            // Removed the CreateContent() and used the PopupView.PopupMessageView.Content.
            if (_popupView is not null && _popupView._popupMessageView is not null && _popupView._popupMessageView.Content is not null)
            {
				// When AutoSizeMode is set to Both and a height request or content height is specified, only the width should be measured and auto-resized to the measured size, since the height request takes priority.
				// Similarly, when AutoSizeMode is set to Both and a width request is specified, only the height should be measured and auto-resized to the measured size, as the width request takes priority.
				if (AutoSizeMode is PopupAutoSizeMode.Width || (canAutoSizeWidth && !canAutoSizeHeight))
				{
					// MAUI-828703 Runtime Content changes is not updated properly in AutoSizeMode property.
					ValidateAndSetPopupWidth(availableWidth, calculatedContentWidth: (_popupView._popupMessageView as IView).Measure(double.PositiveInfinity, AppliedBodyHeight).Width);
				}
				else if (AutoSizeMode is PopupAutoSizeMode.Height || (canAutoSizeHeight && !canAutoSizeWidth))
				{
					// MAUI-828703 Runtime Content changes is not updated properly in AutoSizeMode property.
					ValidateAndSetPopupHeight(availableHeight, calculatedContentHeight: (_popupView._popupMessageView as IView).Measure(AppliedPopupBodyWidth, double.PositiveInfinity).Height);
				}
				else if (AutoSizeMode is PopupAutoSizeMode.Both)
				{
					// MAUI-828703 Runtime Content changes is not updated properly in AutoSizeMode property.
					Size sizeRequest = (_popupView._popupMessageView as IView).Measure(availableWidth, availableHeight);
					ValidateAndSetPopupWidth(availableWidth, calculatedContentWidth: sizeRequest.Width);
					ValidateAndSetPopupHeight(availableHeight, calculatedContentHeight: sizeRequest.Height);
				}
            }
		}

		/// <summary>
		/// Method validates the PoupViewWidth and sets the PopupViewWidth and AppliedPopupContentWidth.
		/// </summary>
		/// <param name="availableWidth">Denotes the available width for the popupView.</param>
		/// <param name="calculatedContentWidth">Width calculated from MessageViewContent.</param>
		/// <param name="calculatedPopupViewWidth">Width calculated for PopupView.</param>
		void ValidateAndSetPopupWidth(double availableWidth, double calculatedContentWidth = -1, double calculatedPopupViewWidth = -1)
		{
            double strokeThickness = PopupStyle is not null ? PopupStyle.StrokeThickness : 0;
            if (calculatedContentWidth is not -1)
            {
                // If the content width is calculated, then PopupViewWidth should be set to the content width.
                calculatedPopupViewWidth = calculatedContentWidth + strokeThickness;
            }

            if (calculatedPopupViewWidth > availableWidth)
            {
                _popupViewWidth = availableWidth;
            }
            else
            {
                _popupViewWidth = calculatedPopupViewWidth;
            }

            _popupViewWidth = Math.Max(0, _popupViewWidth);
            AppliedPopupBodyWidth = Math.Max(0, _popupViewWidth - strokeThickness);
		}

		/// <summary>
		/// Method validates the PoupViewHeight and sets the PopupViewHeight and AppliedBodyHeight.
		/// </summary>
		/// <param name="availableHeight">Denotes the available height for the popupView.</param>
		/// <param name="calculatedContentHeight">Height calculated from MessageViewContent.</param>
		/// <param name="calculatedPopupViewHeight">Height calculated for PopupView.</param>
		void ValidateAndSetPopupHeight(double availableHeight, double calculatedContentHeight = -1, double calculatedPopupViewHeight = -1)
		{
			double strokeThickness = PopupStyle is not null ? PopupStyle.StrokeThickness : 0;
			if (calculatedContentHeight is not -1)
			{
				// If the content height is calculated, then PopupViewHeight should be set to the content height.
				calculatedPopupViewHeight = calculatedContentHeight + AppliedHeaderHeight + AppliedFooterHeight + strokeThickness;
			}

			if (calculatedPopupViewHeight > availableHeight)
			{
				_popupViewHeight = availableHeight;
			}
			else
			{
				_popupViewHeight = calculatedPopupViewHeight;
			}

			_popupViewHeight = Math.Max(0, _popupViewHeight);
			AppliedBodyHeight = Math.Max(0, _popupViewHeight - AppliedHeaderHeight - AppliedFooterHeight - strokeThickness);
		}

		void RemovePopupViewAndResetValues()
		{
			if (_popupView is not null && _popupOverlay is not null)
			{
				if (_popupOverlayContainer is not null)
				{
					_popupOverlayContainer.IsVisible = false;
				}

				_popupOverlay?.RemoveOverlay();
			}

			_popupXPosition = -1;
			_popupYPosition = -1;
			_showXPosition = -1;
			_showYPosition = -1;
			SetPopupViewDefaultHeight();
			_showFullScreen = false;
			_relativeView = null;
			_keyboardHeight = 0;
		}

		/// <summary>
		/// Calculate the height of the header and footer.
		/// </summary>
		void ResetHeaderFooterHeight()
		{
			AppliedHeaderHeight = ShowHeader ? HeaderHeight : 0;
			AppliedFooterHeight = ShowFooter ? FooterHeight : 0;
		}

		/// <summary>
		/// Adjust _popupView Y point and _popupViewHeight when keyboard comes to the View.
		/// </summary>
		void SetPopupPositionBasedOnKeyboard()
		{
#if ANDROID
			PopupPositionBasedOnKeyboard();
#endif
		}

		/// <summary>
		/// Adjust _popupView body height for AutoSizing.
		/// </summary>
		/// <param name="keyboardTopPoint">Keyboard's top point in the screen.</param>
		void AdjustBodyHeightForAutoSizing(double keyboardTopPoint)
		{
			var screenHeight = keyboardTopPoint - StatusBarHeight - SafeAreaAtTop - GetActionBarHeight();

			if (AppliedBodyHeight + AppliedHeaderHeight + AppliedFooterHeight >= screenHeight)
			{
				AppliedBodyHeight = screenHeight - (AppliedHeaderHeight + AppliedFooterHeight);
			}

			_popupViewHeight = AppliedHeaderHeight + AppliedBodyHeight + AppliedFooterHeight;
		}

		/// <summary>
		/// Sets the flow direction value to the popup view when it is applied to its parent.
		/// </summary>
		void SetRTL()
		{
			if (((this as IVisualElementController).EffectiveFlowDirection & EffectiveFlowDirection.RightToLeft) is EffectiveFlowDirection.RightToLeft)
			{
				_isRTL = true;
			}
			else
			{
				_isRTL = false;
			}
#if WINDOWS
			UpdateRTL();
#endif
		}

		/// <summary>
		/// Used to wire common required events.
		/// </summary>
		void WireEvents()
		{
			var application = IPlatformApplication.Current?.Application as Microsoft.Maui.Controls.Application;

			// Application.Windows.Count is 0 when display popup in load time. So, used page appearing event.
			if (application is not null && application.Windows is not null && application.Windows.Count is 0)
			{
				application.PageAppearing += OnApplicationPageAppearing;
				return;
			}

			var page = PopupExtension.GetMainPage();

			var windowPage = PopupExtension.GetMainWindowPage();
			if (page is not null && page.Handler is null && windowPage is not null)
			{
				if (windowPage is Shell shellPage && shellPage is not null)
				{
					// Calling Show() in OnAppearing makes the app unresponsive if the MainPage handler is not null, due to a loop caused by the Loaded event and DisplayPopup() in OnMainPageLoaded().
					if (shellPage.IsLoaded && windowPage.Handler is not null)
					{
						return;
					}
					else if (!shellPage.IsLoaded)
					{
						shellPage.Loaded -= ShellPage_Loaded;
						shellPage.Loaded += ShellPage_Loaded;
					}
				}

				// [iOS] PopupOverlay appears as a gray box in OnAppearing(); condition added for iOS/macOS, working fine in Android/Windows.
#if IOS || MACCATALYST
				if (windowPage.Navigation.ModalStack.LastOrDefault() is not null)
				{
					// LayoutChanged only wired for Modal page. Since when Popup is opened OnAppearing of Modal page height and width is not update in native level.
#if NET10_0
					page.SizeChanged -= this.OnPageSizeChanged;
					page.SizeChanged += this.OnPageSizeChanged;
#else
					page.SizeChanged -= this.OnPageLayoutChanged;
					page.SizeChanged += this.OnPageLayoutChanged;
#endif
				}
#else
				// while calling show() from onAppearing, page.window is not null. So fails the first condition and hook the onMainPageLoaded.
				// When show popup in OnAppearing() of first Navigation page Application gets crash.
				page.Loaded -= OnMainPageLoaded;
				page.Loaded += OnMainPageLoaded;
#endif
			}
			else if (page is not null && page.Window is null && application is not null)
			{
				application.PropertyChanged += OnCurrentPropertyChanged;
			}
			else if (application is not null && windowPage is not null)
			{
				windowPage.PropertyChanged += OnAppMainPagePropertyChanged;
				application.ModalPushed += OnAppCurrentModalPushed;
				application.ModalPopped += OnAppCurrentModalPopped;
			}
			else if (application is null && Application.Current is not null)
			{
				// Popup cannot be opened in the constructor as IPlatformApplication.Current.Application is null.
				Application.Current.PropertyChanged += OnCurrentPropertyChanged;
			}
		}

		/// <summary>
		/// Used to unwire the invoked events.
		/// </summary>
		void UnWireEvents()
		{
			var application = IPlatformApplication.Current?.Application as Microsoft.Maui.Controls.Application;
			var windowPage = PopupExtension.GetMainWindowPage();
			if (application is not null && windowPage is not null)
			{
				windowPage.Loaded -= OnMainPageLoaded;
				windowPage.PropertyChanged -= OnAppMainPagePropertyChanged;
				application.PageAppearing -= OnApplicationPageAppearing;
				application.PropertyChanged -= OnCurrentPropertyChanged;
				application.ModalPushed -= OnAppCurrentModalPushed;
				application.ModalPopped -= OnAppCurrentModalPopped;
			}

			if (Application.Current is not null && windowPage is not null)
			{
				Application.Current.PropertyChanged -= OnCurrentPropertyChanged;
				windowPage.Loaded -= OnMainPageLoaded;
			}
		}

		/// <summary>
		/// Raise when the application page appeared.
		/// </summary>
		/// <param name="sender">Represents application.</param>
		/// <param name="e">Corresponding property changed event args.</param>
		void OnApplicationPageAppearing(object? sender, Page e)
		{
			WireEvents();
		}

		/// <summary>
		/// Raise when the current page size has changed.
		/// </summary>
		/// <param name="sender">Represents current Page.</param>
		/// <param name="e">Corresponding property changed event args.</param>
		private void OnPageSizeChanged(object? sender, EventArgs e)
		{
			this.InitializeOverlay();
			this.CheckAndOpenDeferredPopup();

			// Here need to unwire the LayoutChanged. If not, this will cause issue when resizing the windows with Popup.IsOpen false.
			if (sender is Page page)
			{
				page.SizeChanged -= this.OnPageSizeChanged;
			}
		}

		/// <summary>
		/// Raise when the current page layout has changed.
		/// </summary>
		/// <param name="sender">Represents current Page.</param>
		/// <param name="e">Corresponding property changed event args.</param>
		void OnPageLayoutChanged(object? sender, EventArgs e)
		{
			InitializeOverlay();
			CheckAndOpenDeferredPopup();

			// Unwire the LayoutChanged to avoid issues when resizing the window with Popup.IsOpen set to false.
			if (sender is Page page)
			{
#if NET10_0
				page.SizeChanged -= this.OnPageSizeChanged;
#else
				page.LayoutChanged -= this.OnPageLayoutChanged;
#endif
			}
		}

		/// <summary>
		/// Checks if the popup opening has been deferred and opens the popup if the conditions are now met.
		/// This is typically used to handle scenarios where the popup initialization is delayed due to certain prerequisites not being met initially.
		/// </summary>
		void CheckAndOpenDeferredPopup()
		{
			if (_isOpenDeferred)
			{
				SfPopup.OnIsOpenPropertyChanged(this, true, true);
			}
		}

		/// <summary>
		/// Raises on Application.Current property changes.
		/// </summary>
		/// <param name="sender">Instance of Application.Current.</param>
		/// <param name="e">Corresponding property changed event args.</param>
		void OnCurrentPropertyChanged(object? sender, PropertyChangedEventArgs e)
		{
			var application = IPlatformApplication.Current?.Application as Microsoft.Maui.Controls.Application;
			var windowPage = PopupExtension.GetMainWindowPage();
			if (windowPage is not null)
			{
				windowPage.Loaded += OnMainPageLoaded;
			}
			else if (application is null && Application.Current is not null)
			{
				var current = Application.Current;
				if (current.Windows is not null && current.Windows.Count > 0 && current.Windows[0] is not null)
				{
					// Popup cannot be opened in the constructor as IPlatformApplication.Current.Application is null.
					var mainPage = current.Windows[0].Page;
					if (mainPage is not null)
					{
						mainPage.Loaded += OnMainPageLoaded;
					}
				}
			}
		}

		/// <summary>
		/// Raises on when MainPage is Loaded.
		/// </summary>
		/// <param name="sender">Instance of Application.Current.MainPage.</param>
		/// <param name="e">Corresponding property changed event args.</param>
		void OnMainPageLoaded(object? sender, EventArgs e)
		{
			InitializeOverlay();
			CheckAndOpenDeferredPopup();
		}

		/// <summary>
		/// Raises on when ShellPage is Loaded.
		/// </summary>
		/// <param name="sender">Instance of shell page.</param>
		/// <param name="e">Corresponding property changed event args.</param>
		void ShellPage_Loaded(object? sender, EventArgs e)
		{
			if (sender is Shell shell && shell.CurrentPage is not null)
			{
				shell.CurrentPage.Loaded -= OnMainPageLoaded;
				shell.CurrentPage.Loaded += OnMainPageLoaded;
			}
		}

		/// <summary>
		/// Raises on  Application.Current.MainPage property changes.
		/// </summary>
		/// <param name="sender">Instance of Application.Current.MainPage.</param>
		/// <param name="e">Corresponding property changed event args.</param>
		void OnAppMainPagePropertyChanged(object? sender, PropertyChangedEventArgs e)
		{
			if (IsOpen)
			{
				if (e.PropertyName is "Parent" || (sender is not null && (((sender is NavigationPage || sender is TabbedPage) && e.PropertyName is "CurrentPage") || (sender is FlyoutPage && e.PropertyName is "Detail") || (sender is Shell && e.PropertyName is "CurrentState"))))
				{
					var isValidParent = CheckForParentPage(sender);
					if (!isValidParent)
					{
						IsOpen = false;
						if (!IsOpen)
						{
							AbortPopupViewAnimation();
						}
					}
				}
			}
		}

		/// <summary>
		/// Raised when a page has been popped modally.
		/// </summary>
		/// <param name="sender">Current page of Application.</param>
		/// <param name="e">Event arguments for <see cref="ModalPoppedEventArgs"/>.</param>
		void OnAppCurrentModalPopped(object? sender, ModalPoppedEventArgs e)
		{
			IsOpen = false;
			if (!IsOpen)
			{
				AbortPopupViewAnimation();
			}
		}

		/// <summary>
		/// Raised when a page has been pushed modally.
		/// </summary>
		/// <param name="sender">Current page of Application.</param>
		/// <param name="e">Event arguments for <see cref="ModalPushedEventArgs"/>.</param>
		void OnAppCurrentModalPushed(object? sender, ModalPushedEventArgs e)
		{
			if (IsOpen)
			{
				var isValidParent = CheckForParentPage(e.Modal);
				if (!isValidParent)
				{
					IsOpen = false;
					if (!IsOpen)
					{
						AbortPopupViewAnimation();
					}
				}
			}
		}

		/// <summary>
		/// Gets the parent page of the view.
		/// </summary>
		/// <param name="view">The view whose parent page has to be obtained.</param>
		/// <returns>The parent page of the view.</returns>
		Page? GetViewParentPage(View view)
		{
			// Popup not shown in type B when shown in navigation page constructor, reset parent page if incorrect.
			if (view.Parent is not View && view.Parent is Page page && page != PopupExtension.GetMainPage())
			{
				view.Parent = PopupExtension.GetMainPage();
			}

			var parentView = view.Parent as View;
			if (parentView is null)
			{
				// Root view directly passed.
				return view.Parent as Page;
			}
			else
			{
				// This condition will pass, when the rootView is not passed and some view in rootView is passed.
				while (parentView is not null && parentView.Parent is not null && !(parentView.Parent is Page))
				{
					parentView = parentView.Parent as View;
				}

				return parentView is not null ? parentView.Parent as Page : null;
			}
		}

		/// <summary>
		/// Check whether popup parent page and pushed page is same or not.
		/// </summary>
		/// <param name="sender">Instance of Application.Current.MainPage.</param>
		/// <returns> Returns true when popup parent page and pushed page is same.</returns>
		bool CheckForParentPage(object? sender)
		{
			var isValidParent = false;
			var parentPage = GetViewParentPage(this);
			if (parentPage is not null)
			{
				if (sender is NavigationPage navigationPage)
				{
					isValidParent = parentPage == navigationPage.CurrentPage;
				}
				else if (sender is TabbedPage tabbedPage)
				{
					isValidParent = parentPage == tabbedPage.CurrentPage;
				}
				else if (sender is FlyoutPage flyoutPage)
				{
					isValidParent = parentPage == flyoutPage.Detail;
				}
				else if (sender is Shell shell)
				{
					isValidParent = parentPage == shell.CurrentPage;
				}
				else
				{
					isValidParent = parentPage == sender as ContentPage;
				}
			}

			return isValidParent;
		}

		/// <summary>
		/// Set parent page for sfpopup.
		/// </summary>
		void SetParent()
		{
			var page = PopupExtension.GetMainPage();
			if (page is not null)
			{
				if (Parent is null)
				{
					Parent = page;
				}

				if (_popupView is not null && _popupOverlayContainer is not null)
				{
					if (_popupOverlayContainer.Parent is null || _popupView.Parent is null)
					{
						_popupOverlayContainer.Parent = page;

   						// _popupOverlayContainer visibility is set to false after dismissing the popup.
						// _popupOverlayContainer will be set as the parent to popupView here from DisplayPopup().
						// Due to Maui 9.0.50 changes [https://github.com/dotnet/maui/pull/20154],
						// the IsVisible property of _popupView is set to false when reopening with the same instance of the popup, since _popupOverlayContainer visibility will now be false.
						// _popupOverlayContainer visibility is set to true in a later section, but _popupView visibility will still remain false.
						// causes popup to appear blank for second time.
						_popupOverlayContainer.IsVisible = true;
						_popupView.Parent = _popupOverlayContainer;
					}
				}
			}
		}

		/// <summary>
		/// Gets the default popupView height.
		/// </summary>
		void SetPopupViewDefaultHeight()
		{
			double headerHeight = ShowHeader ? HeaderHeight : 0;
			double footerHeight = ShowFooter ? FooterHeight : 0;
			_defaultPopupViewHeight = 240;
			if (ShowHeader && ShowFooter)
			{
				_defaultPopupViewHeight -= headerHeight + footerHeight;
			}
			else if (ShowHeader)
			{
				_defaultPopupViewHeight = 176 - headerHeight;
			}
			else if (ShowFooter)
			{
				_defaultPopupViewHeight -= footerHeight;
			}
			else
			{
				_defaultPopupViewHeight = 176;
			}

			_defaultPopupViewHeight = Math.Max(0, _defaultPopupViewHeight);
		}

		#region Animation methods

		/// <summary>
		/// Applies animation easing effect to _popupView.
		/// </summary>
		/// <returns>returns easing effect of view.</returns>
		Easing GetAnimationEasing()
		{
			switch (AnimationEasing)
			{
				case PopupAnimationEasing.SinIn:
					return Easing.SinIn;
				case PopupAnimationEasing.SinInOut:
					return Easing.SinInOut;
				case PopupAnimationEasing.SinOut:
					return Easing.SinOut;
				default:
					return Easing.Linear;
			}
		}

		/// <summary>
		/// Applies animation to the <see cref="SfPopupOverlayContainer"/>.
		/// </summary>
		void ApplyContainerAnimation()
		{
			if (_isContainerAnimationInProgress || _popupOverlayContainer is null || !CanAnimate())
			{
				return;
			}

			double startopacity = 0;
			double endopacity = 1;
			uint animationrate = 16;
			var easing = GetAnimationEasing();
			SetAnimationProgress(_popupOverlayContainer, true);
			var fadeAnimation = new Animation(v => _popupOverlayContainer.Opacity = v, IsOpen ? startopacity : endopacity, IsOpen ? endopacity : startopacity);
			fadeAnimation.Commit(_popupOverlayContainer, "ContainerFadeAnimation", animationrate, (uint)AnimationDuration, easing, (finalvalue, cancel) => SetAnimationProgress(_popupOverlayContainer, false));
		}

		/// <summary>
		/// Applies animation to the popup view.
		/// </summary>
		void ApplyPopupAnimation()
		{
			if (_isPopupAnimationInProgress || _popupView is null)
			{
				return;
			}

			if (!CanAnimate())
			{
				ProcessAnimationCompleted(_popupView);
				return;
			}
#if IOS
			// Native configuration for Scale and Translation will get skipped if Parent is null and Frame is Zero.
			if (_popupView is IView popupView)
			{
				popupView.Frame = new Rect(_popupXPosition, _popupYPosition, _popupViewHeight, _popupViewWidth);
			}
#endif

			double startopacity = 0;
			double endopacity = 1;
			float startScale = 0.75f;
			float endScale = 1.0f;
			double startTranslate;
			double endTranslate;
			switch (AnimationMode)
			{
				case PopupAnimationMode.Fade:
					FadeAnimate(_popupView, IsOpen ? startopacity : endopacity, IsOpen ? endopacity : startopacity);
					break;
				case PopupAnimationMode.Zoom:
					ZoomAnimate(_popupView, IsOpen ? startScale : endScale, IsOpen ? endScale : startScale);
					break;
				case PopupAnimationMode.SlideOnLeft:
#if ANDROID
					startTranslate = -_popupViewWidth;
					endTranslate = _popupXPosition;
#else
					startTranslate = -(_popupViewWidth + _popupXPosition);
					endTranslate = 0;
#if WINDOWS
					if (_isRTL)
					{
						startTranslate = -startTranslate;
					}
#endif
#endif
					SlideAnimate(_popupView, IsOpen ? startTranslate : endTranslate, IsOpen ? endTranslate : startTranslate);
					break;

				case PopupAnimationMode.SlideOnRight:
#if ANDROID
					startTranslate = ScreenWidth;
					endTranslate = _popupXPosition;
#else
					startTranslate = ScreenWidth - _popupXPosition;
					endTranslate = 0;
#if WINDOWS
					if (_isRTL)
					{
						startTranslate = -startTranslate;
					}
#endif
#endif
					SlideAnimate(_popupView, IsOpen ? startTranslate : endTranslate, IsOpen ? endTranslate : startTranslate);
					break;

				case PopupAnimationMode.SlideOnTop:
#if ANDROID
					startTranslate = -_popupViewHeight;
					endTranslate = _popupYPosition;
#else
					startTranslate = -(_popupViewHeight + _popupYPosition);
					endTranslate = 0;
#endif
					SlideAnimate(_popupView, IsOpen ? startTranslate : endTranslate, IsOpen ? endTranslate : startTranslate);
					break;

				case PopupAnimationMode.SlideOnBottom:
#if ANDROID
					startTranslate = PopupExtension.GetScreenHeight();
					endTranslate = _popupYPosition;
#else
					startTranslate = PopupExtension.GetScreenHeight() - _popupYPosition;
					endTranslate = 0;
#endif
					SlideAnimate(_popupView, IsOpen ? startTranslate : endTranslate, IsOpen ? endTranslate : startTranslate);
					break;
			}
		}

		/// <summary>
		/// Animates the opacity of the popup view from a start value to an end value.
		/// </summary>
		/// <param name="view">The view to which the animation to be applied.</param>
		/// <param name="startvalue">Start value of the animator.</param>
		/// <param name="endvalue">End value of the animator.</param>
		void FadeAnimate(View view, double startvalue, double endvalue)
		{
			uint animationRate = 16;
			var easing = GetAnimationEasing();
			SetAnimationProgress(view, true);
			var fadeAnimation = new Animation(v => view.Opacity = v, startvalue, endvalue);
			fadeAnimation.Commit(view, "FadeAnimation", animationRate, (uint)AnimationDuration, easing, (finalvalue, cancel) => ProcessAnimationCompleted(view));
		}

		/// <summary>
		/// Animates the scale of the popup view from a start value to an end value.
		/// </summary>
		/// <param name="view">The view to which the animation to be applied.</param>
		/// <param name="startvalue">Start value of the animator.</param>
		/// <param name="endvalue">End value of the animator.</param>
		void ZoomAnimate(View view, float startvalue, float endvalue)
		{
			uint animationRate = 16;
			var easing = GetAnimationEasing();
			SetAnimationProgress(view, true);
			var zoomAnimation = new Animation(Zoom, startvalue, endvalue);
			zoomAnimation.Commit(view, "ZoomAnimation", animationRate, (uint)AnimationDuration, easing, (finalvalue, cancel) => ProcessAnimationCompleted(view));
		}

		/// <summary>
		/// Animates the TranslationX and TranslationY of the popup view from a start value to an end value.
		/// </summary>
		/// <param name="view">The view to which the animation to be applied.</param>
		/// <param name="startvalue">Start value of the animator.</param>
		/// <param name="endvalue">End value of the animator.</param>
		void SlideAnimate(View view, double startvalue, double endvalue)
		{
			uint animationRate = 16;
			var easing = GetAnimationEasing();
			SetAnimationProgress(view, true);
			Animation slideAnimation;

			if (AnimationMode is PopupAnimationMode.SlideOnLeft || AnimationMode is PopupAnimationMode.SlideOnRight)
			{
				slideAnimation = new Animation(v => view.TranslationX = v, startvalue, endvalue);
			}
			else
			{
				slideAnimation = new Animation(v => view.TranslationY = v, startvalue, endvalue);
			}

			slideAnimation.Commit(view, "SlideAnimation", animationRate, (uint)AnimationDuration, easing, (finalvalue, cancel) => ProcessAnimationCompleted(view));
		}

		/// <summary>
		/// Set Scale value for _popupView.
		/// </summary>
		/// <param name="scale">Scale value for _popupView.</param>
		void Zoom(double scale)
		{
			if (_popupView is not null)
			{
#if ANDROID
				// Zoom animation is not performs from center point of _popupView.
				var centerX = _popupXPosition + (_popupViewWidth / 2);
				var centerY = _popupYPosition + (_popupViewHeight / 2);
				_popupView.TranslationX = centerX - ((_popupViewWidth * scale) / 2);
				_popupView.TranslationY = centerY - ((_popupViewHeight * scale) / 2);
#endif
				_popupView.Scale = scale;
			}
		}

		/// <summary>
		/// Triggers after completion of the animation.
		/// Removes the popup view from the overlay and raise the popup events.
		/// </summary>
		/// <param name="view">The view to which the animation progress to be set.</param>
		void ProcessAnimationCompleted(View view)
		{
			if (AnimationMode is not PopupAnimationMode.None)
			{
				SetAnimationProgress(view, false);
			}

			if (!IsOpen)
			{
				ResetAnimatedProperties();
				RemovePopupViewAndResetValues();
			}

			RaisePopupEvent();
		}

		/// <summary>
		/// Checks if the animation should proceed based on the animation duration and mode.
		/// </summary>
		/// <returns>Returns true if the animation duration is greater than 0 and the animation mode is not None, indicating that the animation should proceed. Returns false otherwise.</returns>
		private bool CanAnimate()
		{
			return AnimationDuration > 0 && AnimationMode is not PopupAnimationMode.None;
		}

		/// <summary>
		/// Reset the _popupView Animated Properties.
		/// </summary>
		void ResetAnimatedProperties()
		{
			if (_popupView is not null)
			{
				_popupView.Scale = 1;
				_popupView.Opacity = 1;
				_popupView.TranslationX = 0;
				_popupView.TranslationY = 0;
			}

			if (_popupOverlayContainer is not null)
			{
				_popupOverlayContainer.Opacity = 1;
			}
		}

		/// <summary>
		/// Abort the _popupView Animation.
		/// </summary>
		void AbortPopupViewAnimation()
		{
			if (CanAnimate())
			{
				if (_popupView.AnimationIsRunning("ZoomAnimation"))
				{
					_popupView.AbortAnimation("ZoomAnimation");
				}
				else if (_popupView.AnimationIsRunning("FadeAnimation"))
				{
					_popupView.AbortAnimation("FadeAnimation");
				}
				else if (_popupView.AnimationIsRunning("SlideAnimation"))
				{
					_popupView.AbortAnimation("SlideAnimation");
				}

				if (_popupOverlayContainer.AnimationIsRunning("ContainerFadeAnimation"))
				{
					_popupOverlayContainer.AbortAnimation("ContainerFadeAnimation");
				}
			}
		}

		/// <summary>
		/// Used to set the value for Animation is progressing or not.
		/// </summary>
		/// <param name="view">Animated view.</param>
		/// <param name="animationInProgress">Animation value.</param>
		void SetAnimationProgress(View view, bool animationInProgress)
		{
			if (view.GetType() == typeof(SfPopupOverlayContainer))
			{
				_isContainerAnimationInProgress = animationInProgress;
			}
			else
			{
				_isPopupAnimationInProgress = animationInProgress;
			}
		}

		#endregion

		#endregion

		#region Override Methods

		/// <summary>
		/// Need to handle the run time changes of <see cref="PropertyChangedEventArgs"/> of <see cref="SfPopup"/>.
		/// </summary>
		/// <param name="propertyName">Represents the property changed event arguments.</param>
		protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
		{
			if (propertyName is null)
			{
				return;
			}

			if (propertyName.Equals("Parent", StringComparison.Ordinal))
			{
				SetRTL();
			}

			if (_popupView is null)
			{
				return;
			}

			else if (propertyName.Equals("CanShowPopupInFullScreen", StringComparison.Ordinal))
			{
				_popupView.UpdateHeaderView();
				if (IsOpen && !_isOpenInProgress)
				{
					ResetPopupWidthHeight();
					_popupView.InvalidateForceLayout();
				}
			}
			else if (propertyName.Equals("FlowDirection", StringComparison.Ordinal))
			{
				SetRTL();
				_popupView.FlowDirection = _isRTL ? FlowDirection.RightToLeft : FlowDirection;
				if (IsOpen)
				{
					ResetPopupWidthHeight();
				}
			}
			else if (propertyName.Equals("HeightRequest", StringComparison.Ordinal) || propertyName.Equals("WidthRequest", StringComparison.Ordinal))
			{
				if (IsOpen)
				{
					ResetPopupWidthHeight();

					// _popupView  WidthRequest and HeightRequest are not updated properly when the Popup is Open.
					_popupView.InvalidateForceLayout();
				}
			}

			base.OnPropertyChanged(propertyName);
		}

		/// <summary>
		/// Occurs when the popup view binding context is changed.
		/// </summary>
		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();
			if (_popupView is not null)
			{
				_popupView.BindingContext = BindingContext;
			}
		}

		/// <summary>
		/// Raised when the popup handler changes.
		/// </summary>
		protected override void OnHandlerChanged()
		{
			if (Handler is null && _popupOverlay is not null)
			{
				_popupOverlay.Dispose();
				_popupOverlay = null;
			}

			if (!_isOverlayAdded && Handler != null)
            {
				InitializeOverlay();

				// 989436: Popup is not shown when IsOpen is set in XAML or in the constructor during loading with a direct page. So, the popup needs to be displayed when it is still not shown from OnIsOpenChanged.
				if (_isOverlayAdded)
				{
					CheckAndOpenDeferredPopup();
				}
			}

			base.OnHandlerChanged();
		}

		#endregion

		#region PopupTheme Support

#nullable disable
		/// <summary>
		/// Method invoke to get the initial set of color's from theme dictionary.
		/// </summary>
		/// <returns>Return the Data Grid theme dictionary.</returns>
		ResourceDictionary IParentThemeElement.GetThemeDictionary()
		{
			return null;
		}
#nullable restore

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
			if (!newTheme.Equals(oldTheme, StringComparison.OrdinalIgnoreCase))
			{
				PopupStyle.SetThemeProperties(this);
			}
		}

		#endregion

		#region Bindable Properties Callbacks

		/// <summary>
		/// Delegate for <see cref="StaysOpen"/> bindable property changed.
		/// </summary>
		/// <param name="bindable">Instance of the SfPopupLayout class.</param>
		/// <param name="oldValue">Old value in the property.</param>
		/// <param name="newValue">New value obtained in the property.</param>
		static void OnStaysOpenChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var popup = bindable as SfPopup;
		}

		/// <summary>
		/// Delegate for <see cref="AutoCloseDuration"/> bindable property changed.
		/// </summary>
		/// <param name="bindable">Instance of the SfPopupLayout class.</param>
		/// <param name="oldValue">Old value in the property.</param>
		/// <param name="newValue">New value obtained in the property.</param>
		static void OnAutoCloseDurationPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var popup = bindable as SfPopup;
			if (popup is not null && popup.IsOpen && (int)newValue > 0)
			{
				// AutoCloseDuration not working, if it set after setting IsOpen to true in xaml.
				popup.AutoClosePopup();
			}
		}

		/// <summary>
		/// Delegate for <see cref="OverlayMode"/> bindable property changed.
		/// </summary>
		/// <param name="bindable">Instance of the SfPopupLayout class.</param>
		/// <param name="oldValue">Old value in the property.</param>
		/// <param name="newValue">New value obtained in the property.</param>
		static void OnOverlayModePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var popup = bindable as SfPopup;
			if (popup is not null && popup.IsOpen)
			{
				popup.ApplyOverlayBackground();
			}
		}

		/// <summary>
		/// Delegate for <see cref="IsOpen"/> bindable property changed.
		/// </summary>
		/// <param name="bindable">Instance of the SfPopupLayout class.</param>
		/// <param name="oldValue">Old value in the property.</param>
		/// <param name="newValue">New value obtained in the property.</param>
		static void OnIsOpenPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var popup = bindable as SfPopup;
			if (popup is null)
			{
				return;
			}

			if (!popup._isOverlayAdded)
			{
				popup.InitializeOverlay();

				if (!popup._isOverlayAdded)
				{
					popup._isOpenDeferred = true;
					return;
				}

			}

			if (popup.IsOpen && (popup._isContainerAnimationInProgress || popup._isPopupAnimationInProgress))
			{
				return;
			}

			if (popup.IsOpen)
			{
				popup._isOpenInProgress = true;
				popup.CanShowPopupInFullScreen = popup.IsFullScreen || popup._showFullScreen;
			}

			popup.SyncPopupDimensionFields();

			if (popup.IsOpen && popup.AutoCloseDuration > 0)
			{
				popup.DisplayPopup();
				popup.AutoClosePopup();
				return;
			}

			if (popup.IsOpen)
			{
				// A condition is added to avoid reopening the Popup after canceling the Closing event.
				if (popup._canOpenPopup)
				{
					popup.DisplayPopup();
				}
			}
			else
			{
				if (popup.DismissPopup())
				{
					// To avoid opening the Popup again, after cancelling the Closing event.
					popup._canOpenPopup = false;

					// IsOpen value of SfPopup is incorrect after canceling the Closing event.
					popup.IsOpen = true;
				}
				else
				{
					popup._canOpenPopup = true;
				}
			}

			popup._isOpenInProgress = false;
		}

		/// <summary>
		/// Delegate for <see cref="IsFullScreen"/> bindable property changed.
		/// </summary>
		/// <param name="bindable">Instance of the <see cref="SfPopup"/> class.</param>
		/// <param name="oldValue">Old value in the property.</param>
		/// <param name="newValue">New value obtained in the property.</param>
		static void OnIsFullScreenPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var popup = (SfPopup)bindable;
			if (popup is not null)
			{
				popup.CanShowPopupInFullScreen = (bool)newValue;
			}
		}

		/// <summary>
		/// Delegate for AppearanceMode bindable property changed.
		/// </summary>
		/// <param name="bindable">instance of the SfPopup class.</param>
		/// <param name="oldValue">Old value in the property.</param>
		/// <param name="newValue">New value obtained in the property.</param>
		static void OnAppearanceModePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var popup = (SfPopup)bindable;
			if (popup is not null && popup._popupView is not null && popup._popupView._footerView is not null && popup._popupView.IsViewLoaded)
			{
				popup._popupView._footerView.UpdateFooterChild();
				if (popup.IsOpen)
				{
					popup._popupView._footerView.UpdateFooterAppearance();
				}
			}
		}

		/// <summary>
		///  Delegate for HeaderTemplate bindable property changed.
		/// </summary>
		/// <param name="bindable">instance of the SfPopup class.</param>
		/// <param name="oldValue">Old value in the property.</param>
		/// <param name="newValue">New value obtained in the property.</param>
		static void OnHeaderTemplatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var popup = (SfPopup)bindable;
			if (popup is not null && popup._popupView is not null)
			{
				popup._popupView.UpdateHeaderView();
				if (popup.IsOpen)
				{
					popup.ResetPopupWidthHeight();
				}
			}
		}

		/// <summary>
		///  Delegate for ContentTemplate bindable property changed.
		/// </summary>
		/// <param name="bindable">instance of the SfPopup class.</param>
		/// <param name="oldValue">Old value in the property.</param>
		/// <param name="newValue">New value obtained in the property.</param>
		static void OnContentTemplatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var popup = (SfPopup)bindable;
			if (popup is not null && popup._popupView is not null)
			{
				popup._popupView.UpdateMessageView();
				if (popup.IsOpen)
				{
					popup.ResetPopupWidthHeight();
				}
			}
		}

		/// <summary>
		/// Delegate for FooterTemplate bindable property changed.
		/// </summary>
		/// <param name="bindable">instance of the SfPopup class.</param>
		/// <param name="oldValue">Old value in the property.</param>
		/// <param name="newValue">New value obtained in the property.</param>
		static void OnFooterTemplatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var popup = (SfPopup)bindable;
			if (popup is not null && popup._popupView is not null)
			{
				popup._popupView.UpdateFooterView();
				if (popup.IsOpen)
				{
					popup.ResetPopupWidthHeight();
				}
			}
		}

		/// <summary>
		/// Delegate for ShowHeader bindable property changed.
		/// </summary>
		/// <param name="bindable">instance of the SfPopup class.</param>
		/// <param name="oldValue">Old value in the property.</param>
		/// <param name="newValue">New value obtained in the property.</param>
		static void OnShowHeaderPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var popup = (SfPopup)bindable;

			if (popup is not null)
			{
				popup.SetPopupViewDefaultHeight();
				if (popup._popupView is not null)
				{
					if(popup._popupView._headerView is not null)
					{
						if ((bool)newValue)
						{
							if (!popup._popupView.Contains(popup._popupView._headerView))
							{
								popup._popupView.Add(popup._popupView._headerView);
							}
						}
						else
						{
							if (popup._popupView.Contains(popup._popupView._headerView))
							{
								popup._popupView.Remove(popup._popupView._headerView);
								if (popup._popupView._headerView.Handler is not null)
								{
									popup._popupView._headerView.Handler.DisconnectHandler();
								}
							}
						}
					}

					// MessageView padding values are changed, when ShowHeader is false. So, we need to update the MessageView.
					popup._popupView._popupMessageView?.UpdatePadding();
				}

				// Reset clipped popup height after update to prevent black area when ShowHeader is set at runtime.
				if (popup.IsOpen)
				{
					popup.ResetPopupWidthHeight();
					popup._popupView?.InvalidateForceLayout();
				}
			}
		}

		/// <summary>
		/// Delegate for ShowFooter bindable property changed.
		/// </summary>
		/// <param name="bindable">instance of the SfPopup class.</param>
		/// <param name="oldValue">Old value in the property.</param>
		/// <param name="newValue">New value obtained in the property.</param>
		static void OnShowFooterPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var popup = (SfPopup)bindable;

			if (popup is not null)
			{
				popup.SetPopupViewDefaultHeight();

				// Return if the IsViewLoaded is false.
				if (popup._popupView is not null)
				{
					if (popup._popupView._footerView is not null)
					{
						if ((bool)newValue)
						{
							if (!popup._popupView.Contains(popup._popupView._footerView))
							{
								popup._popupView.Add(popup._popupView._footerView);
							}
						}
						else
						{
							if (popup._popupView.Contains(popup._popupView._footerView))
							{
								popup._popupView.Remove(popup._popupView._footerView);
							}
						}
					}

					// MessageView padding values are changed, when ShowHeader is false. So, we need to update the MessageView.
					popup._popupView._popupMessageView?.UpdatePadding();
				}

				// Need to reposition the popup when its height is updated and reset the clipped height after the update.
				if (popup.IsOpen)
				{
					popup.ResetPopupWidthHeight();
					popup._popupView?.InvalidateForceLayout();
				}
			}
		}

		/// <summary>
		/// Delegate for HeaderTitle bindable property changed.
		/// </summary>
		/// <param name="bindable">instance of the SfPopup class.</param>
		/// <param name="oldValue">Old value in the property.</param>
		/// <param name="newValue">New value obtained in the property.</param>
		static void OnHeaderTitlePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var popup = (SfPopup)bindable;
			if (popup is not null)
			{
				if (newValue is not null && newValue != oldValue)
				{
					popup._popupView?.SetHeaderTitleText(newValue.ToString() ?? string.Empty);
				}
				else
				{
					popup._popupView?.SetHeaderTitleText(SfPopupResources.GetLocalizedString("Title"));
				}
			}
		}

		/// <summary>
		/// Delegate for AcceptButtonText bindable property changed.
		/// </summary>
		/// <param name="bindable">instance of the SfPopup class.</param>
		/// <param name="oldValue">Old value in the property.</param>
		/// <param name="newValue">New value obtained in the property.</param>
		static void OnAcceptButtonTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var popup = (SfPopup)bindable;
			if (popup is not null)
			{
				if (newValue is not null && newValue != oldValue)
				{
					popup._popupView?.SetAcceptButtonText(newValue.ToString() ?? string.Empty);
				}
				else
				{
					popup._popupView?.SetAcceptButtonText(SfPopupResources.GetLocalizedString("AcceptButtonText"));
				}
			}
		}

		/// <summary>
		/// Delegate for DeclineButtonText bindable property changed.
		/// </summary>
		/// <param name="bindable">instance of the SfPopup class.</param>
		/// <param name="oldValue">Old value in the property.</param>
		/// <param name="newValue">New value obtained in the property.</param>
		static void OnDeclineButtonTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var popup = (SfPopup)bindable;
			if (popup is not null)
			{
				if (newValue is not null && newValue != oldValue)
				{
					popup._popupView?.SetDeclineButtonText(newValue.ToString() ?? string.Empty);
				}
				else
				{
					popup._popupView?.SetDeclineButtonText(SfPopupResources.GetLocalizedString("DeclineButtonText"));
				}
			}
		}

		/// <summary>
		/// Delegate for PopupStyle bindable property changed.
		/// </summary>
		/// <param name="bindable">instance of the _popupView class.</param>
		/// <param name="oldValue">Old value in the property.</param>
		/// <param name="newValue">New value obtained in the property.</param>
		static void OnPopupStylePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var popup = bindable as SfPopup;

			if (popup is not null)
			{
				if (newValue is not null)
				{
					popup.PopupStyle.SetThemeProperties(popup);

					// The PopupStyle is set after UpdatePopupStyles when IsOpen is set in XAML for the shell page, causing the styles not to be applied.
					if (popup.IsOpen)
					{
						var page = PopupExtension.GetMainPage();
						var application = IPlatformApplication.Current?.Application as Microsoft.Maui.Controls.Application;
						var windowPage = PopupExtension.GetMainWindowPage();
						if ((page is not null && page.Window is not null && page.Handler is not null) || (windowPage is Shell shellPage && shellPage is not null && windowPage.Handler is not null))
						{
							popup.UpdatePopupStyles();
						}
					}
				}
				else
				{
					// Setting PopupStyle to null at runtime requires creating a new default style for the popup.
					var popupStyle = new PopupStyle();
					popupStyle.SetThemeProperties(popup);
					popup.PopupStyle = popupStyle;
				}
			}
		}

		/// <summary>
		/// Delegate for PopupStyle default value creator.
		/// </summary>
		/// <param name="bindable">Instance of the SfPopup class.</param>
		/// <returns>Returns the popup style instance.</returns>
		static object? CreatePopupStyle(BindableObject bindable)
		{
			var popup = bindable as SfPopup;
			var popupStyle = new PopupStyle();
			if (popup is not null)
			{
				popupStyle.SetThemeProperties(popup);
			}

			return popupStyle;
		}

		/// <summary>
		/// Delegate for AnimationMode bindable property changed.
		/// </summary>
		/// <param name="bindable">instance of the _popupView class.</param>
		/// <param name="oldValue">Old value in the property.</param>
		/// <param name="newValue">New value obtained in the property.</param>
		static void OnAnimationModePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
		}

		/// <summary>
		/// Delegate for AnimationEasing bindable property changed.
		/// </summary>
		/// <param name="bindable">instance of the _popupView class.</param>
		/// <param name="oldValue">Old value in the property.</param>
		/// <param name="newValue">New value obtained in the property.</param>
		static void OnAnimationEasingPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
		}

		/// <summary>
		/// Delegate for ShowCloseButton bindable property changed.
		/// </summary>
		/// <param name="bindable">instance of the SfPopup class.</param>
		/// <param name="oldValue">Old value in the property.</param>
		/// <param name="newValue">New value obtained in the property.</param>
		static void OnShowCloseButtonPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
            var popup = (SfPopup)bindable;
            if (popup is not null && popup._popupView is not null)
            {
                popup._popupView.UpdateHeaderView();
                if (popup.IsOpen && popup._popupView is not null)
                {
                    popup._popupView.InvalidateForceLayout();
                }
            }
		}

		/// <summary>
		/// Delegate for AnimationDuration bindable property changed.
		/// </summary>
		/// <param name="bindable">instance of the _popupView class.</param>
		/// <param name="oldValue">Old value in the property.</param>
		/// <param name="newValue">New value obtained in the property.</param>
		static void OnAnimationDurationPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
		}

		/// <summary>
		/// Delegate for HeaderHeight bindable property changed.
		/// </summary>
		/// <param name="bindable">instance of the SfPopup class.</param>
		/// <param name="oldValue">Old value in the property.</param>
		/// <param name="newValue">New value obtained in the property.</param>
		static void OnHeaderHeightPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var popup = (SfPopup)bindable;
			if (popup is not null)
			{
				popup.SetPopupViewDefaultHeight();
				if (popup.IsOpen)
				{
					popup.CalculatePopupSizeAndPosition();
					popup._popupView?.InvalidateForceLayout();
				}
			}
		}

		/// <summary>
		/// Delegate for FooterHeight bindable property changed.
		/// </summary>
		/// <param name="bindable">instance of the SfPopup class.</param>
		/// <param name="oldValue">Old value in the property.</param>
		/// <param name="newValue">New value obtained in the property.</param>
		static void OnFooterHeightPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var popup = (SfPopup)bindable;
			if (popup is not null)
			{
				popup.SetPopupViewDefaultHeight();
				if (popup.IsOpen)
				{
					popup.CalculatePopupSizeAndPosition();
					popup._popupView?.InvalidateForceLayout();
				}
			}
		}

		/// <summary>
		/// Delegate for <see cref="AutoSizeMode"/> bindable property changed.
		/// </summary>
		/// <param name="bindable">Instance of the SfPopupLayout class.</param>
		/// <param name="oldValue">Old value in the property.</param>
		/// <param name="newValue">New value obtained in the property.</param>
		static void OnAutoSizeModePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var popup = (SfPopup)bindable;
			if (popup is not null && popup.IsOpen)
			{
				popup.UpdatePopupView();

				// Popup is not measured properly when setting AutoSizeMode inside the Opening event of SfPopup.
				popup._popupView?.InvalidateForceLayout();
			}
		}

		/// <summary>
		/// Delegate for Message bindable property changed.
		/// </summary>
		/// <param name="bindable">instance of the _popupView class.</param>
		/// <param name="oldValue">Old value in the property.</param>
		/// <param name="newValue">New value obtained in the property.</param>
		static void OnMessagePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var popup = (SfPopup)bindable;
			if (popup is not null)
			{
				if (newValue is not null && newValue != oldValue)
				{
					popup._popupView?.SetMessageText(newValue.ToString() ?? string.Empty);
				}
				else
				{
					popup._popupView?.SetMessageText(SfPopupResources.GetLocalizedString("Message"));
				}
			}
		}

		/// <summary>
		/// Delegate for StartX bindable property changed.
		/// </summary>
		/// <param name="bindable">Instance of the <see cref="SfPopup"/> class.</param>
		/// <param name="oldValue">Old value in the property.</param>
		/// <param name="newValue">New value obtained in the property.</param>
		static void OnStartXPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var popup = (SfPopup)bindable;
			if (popup is not null && popup.IsOpen && newValue is not null && newValue != oldValue)
			{
				popup.PositionPopupView();
			}
		}

		/// <summary>
		/// Delegate for StartY bindable property changed.
		/// </summary>
		/// <param name="bindable">Instance of the <see cref="SfPopup"/> class.</param>
		/// <param name="oldValue">Old value in the property.</param>
		/// <param name="newValue">New value obtained in the property.</param>
		static void OnStartYPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var popup = (SfPopup)bindable;
			if (popup is not null && popup.IsOpen && newValue is not null && newValue != oldValue)
			{
				popup.PositionPopupView();
			}
		}

		/// <summary>
        /// Delegate for Padding bindable property changed.
        /// </summary>
        /// <param name="bindable">Instance of the <see cref="SfPopup"/> class.</param>
        /// <param name="oldValue">Old value in the property.</param>
        /// <param name="newValue">New value obtained in the property.</param>
        private static void OnPaddingPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var popup = (SfPopup)bindable;
            if (popup is not null && popup.IsOpen && newValue is not null && newValue != oldValue)
            {
                popup.PositionPopupView();
            }
        }

		#endregion

		#region Events

		/// <summary>
		/// This event will be fired whenever the popupView is shown in the view.
		/// </summary>
		/// <remarks>
		/// This event fires whenever the <see cref="IsOpen"/> property is set as <c>true</c>.
		/// </remarks>
		/// <example>
		/// <para>The following example demonstrates how to use the <see cref="Opened"/> event in the <see cref="SfPopup"/> control.</para>
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <popup:SfPopup x:Name="popup" Closed="Popup_Opened" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// private void Popup_Opened(object sender, EventArgs e)
		/// {
		///		// Codes that needs to be executed once popup is opened.
		/// }
		/// ]]></code>
		/// </example>
		/// <seealso cref="Opening"/>
		/// <seealso cref="Closing"/>
		/// <seealso cref="Closed"/>
		public event EventHandler? Opened;

		/// <summary>
		/// This event will be fired whenever the popup view is dismissed from the view.
		/// </summary>
		/// <remarks>
		/// This event fires whenever the <see cref="IsOpen"/> property is set as <c>false</c>.
		/// </remarks>
		/// <example>
		/// <para>The following example demonstrates how to use the <see cref="Closed"/> event in the <see cref="SfPopup"/> control.</para>
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <popup:SfPopup x:Name="popup" Closed="Popup_Closed" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// private void Popup_Closed(object sender, EventArgs e)
		/// {
		///		// Codes that needs to be executed once popup is completely closed.
		/// }
		/// ]]></code>
		/// </example>
		/// <seealso cref="Closing"/>
		/// <seealso cref="Opening"/>
		/// <seealso cref="Opened"/>
		public event EventHandler? Closed;

		/// <summary>
		/// This event will be fired whenever the popup view is opening in the view. Occurring of this event can be cancelled based on conditions.
		/// </summary>
		/// <example>
		/// <para>The following example demonstrates how to use the <see cref="Opening"/> event in the <see cref="SfPopup"/> control.</para>
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <popup:SfPopup x:Name="popup" Closed="Popup_Opening" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// private void Popup_Opening(object sender, EventArgs e)
		/// {
		///		// Codes that needs to be executed when the popup is opening.
		/// }
		/// ]]></code>
		/// </example>
		/// <seealso cref="Opened"/>
		/// <seealso cref="Closing"/>
		/// <seealso cref="Closed"/>
		public event EventHandler<CancelEventArgs>? Opening;

		/// <summary>
		/// This event will be fired whenever the <see cref="_popupView"/> is closing in the view. Occurring of this event can be cancelled based on conditions.
		/// </summary>
		/// <example>
		/// <para>The following example demonstrates how to use the <see cref="Closing"/> event in the <see cref="SfPopup"/> control.</para>
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <popup:SfPopup x:Name="popup" Closed="Popup_Closing" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// private void Popup_Closing(object sender, EventArgs e)
		/// {
		///		// Codes that needs to be executed when the popup is closing.
		/// }
		/// ]]></code>
		/// </example>
		/// <seealso cref="Closed"/>
		/// <seealso cref="Opening"/>
		/// <seealso cref="Opened"/>
		public event EventHandler<CancelEventArgs>? Closing;

		#endregion
	}
}
