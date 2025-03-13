using Microsoft.Maui.Controls.Shapes;
using Syncfusion.Maui.Toolkit.Internals;
using Syncfusion.Maui.Toolkit.Themes;
using Syncfusion.Maui.Toolkit.Helper;

namespace Syncfusion.Maui.Toolkit.BottomSheet
{

	/// <summary>
	/// Represents the <see cref="SfBottomSheet"/> control that displays from the bottom of the screen.
	/// </summary>
	[ContentProperty(nameof(Content))]
	public partial class SfBottomSheet : SfView, IParentThemeElement
    {
		#region Fields

    	// Overlay and content
    	/// <summary>
    	/// The grid used to create the overlay effect.
    	/// </summary>
    	SfGrid? _overlayGrid;

    	/// <summary>
    	/// The border control representing the main bottom sheet.
    	/// </summary>
    	BottomSheetBorder? _bottomSheet;

    	/// <summary>
    	/// The grid containing the content of the bottom sheet.
    	/// </summary>
    	SfGrid? _bottomSheetContent;

    	/// <summary>
    	/// A border control used to add padding to the bottom sheet content.
    	/// </summary>
    	SfBorder? _contentBorder;

    	// Grabber
    	/// <summary>
    	/// The border control representing the grabber (drag handle) of the bottom sheet.
    	/// </summary>
    	SfBorder? _grabber;

    	/// <summary>
    	/// The shape used to provide corner radius for the grabber.
    	/// </summary>
		RoundRectangle? _grabberStrokeShape;

    	// Shape
    	/// <summary>
    	/// The shape used to provide corner radius for the bottom sheet.
    	/// </summary>
    	RoundRectangle? _bottomSheetStrokeShape;

    	// State
    	/// <summary>
    	/// Indicates whether the bottom sheet is in a half-expanded state.
    	/// </summary>
    	bool _isHalfExpanded = true;

    	/// <summary>
    	/// Indicates whether the bottom sheet is currently open.
    	/// </summary>
    	bool _isSheetOpen;

    	/// <summary>
    	/// Indicates whether a pointer (touch or mouse) is currently pressed on the bottom sheet.
    	/// </summary>
    	bool _isPointerPressed;

    	// Touch tracking
    	/// <summary>
    	/// The initial Y-coordinate of a touch event on the bottom sheet.
    	/// </summary>
    	double _initialTouchY;

    	/// <summary>
    	/// The starting Y-coordinate of a swipe gesture on the bottom sheet.
    	/// </summary>
    	double _startTouchY;

    	/// <summary>
    	/// The ending Y-coordinate of a swipe gesture on the bottom sheet.
    	/// </summary>
    	double _endTouchY;

    	// Event args
    	/// <summary>
    	/// Event arguments used to track state changes in the bottom sheet.
    	/// </summary>
    	readonly StateChangedEventArgs _stateChangedEventArgs = new StateChangedEventArgs();

    	// Constants
    	/// <summary>
    	/// The default opacity value for the overlay.
    	/// </summary>
    	const double DefaultOverlayOpacity = 0.5;

    	/// <summary>
    	/// The default height value for the collapsed state of the bottom sheet.
    	/// </summary>
    	const double MinimizedHeight = 100;

    	// Grabber constants
    	/// <summary>
    	/// The default height of the grabber.
    	/// </summary>
    	const double DefaultGrabberHeight = 4;

    	/// <summary>
    	/// The default width of the grabber.
    	/// </summary>
    	const double DefaultGrabberWidth = 32;

    	/// <summary>
    	/// The default corner radius of the grabber.
    	/// </summary>
    	const double DefaultGrabberCornerRadius = 12;

		// Ratio constants
    	/// <summary>
    	/// The default height of the row containing the grabber.
    	/// </summary>
    	const double DefaultGrabberRowHeight = 30;

		/// <summary>
		/// The minimum allowed value for the HalfExpandedRatio property.
		/// </summary>
		/// <remarks>
		/// This value represents the smallest fraction of the screen height that the bottom sheet can occupy when half-expanded.
		/// </remarks>
		const double MinHalfExpandedRatio = 0.1;

		/// <summary>
		/// The maximum allowed value for the HalfExpandedRatio property.
		/// </summary>
		/// <remarks>
		/// This value represents the largest fraction of the screen height that the bottom sheet can occupy when half-expanded.
		/// </remarks>
		const double MaxHalfExpandedRatio = 0.9;

		/// <summary>
		/// The minimum allowed value for the FullExpandedRatio property.
		/// </summary>
		/// <remarks>
		/// This value represents the smallest fraction of the screen height that the bottom sheet can occupy when full-expanded.
		/// </remarks>
		const double MinFullfExpandedRatio = 0.1;

		/// <summary>
		/// The maximum allowed value for the FullExpandedRatio property.
		/// </summary>
		/// <remarks>
		/// This value represents the largest fraction of the screen height that the bottom sheet can occupy when full-expanded.
		/// </remarks>
		const double MaxFullfExpandedRatio = 1;

		/// <summary>
		/// The default ratio value for the half-expanded.
		/// </summary>
		const double DefaultHalfExpandedRatio = 0.5;

		/// <summary>
		/// The default ratio value for the full-expanded.
		/// </summary>
		const double DefaultFullExpandedRatio = 1;

		#endregion

		#region Bindable Properties

		/// <summary>
		/// Identifies the <see cref="Content"/> bindable property.
		/// </summary>
		/// <remarks>
		/// It is mandatory to set the <see cref="Content"/> property for the <see cref="SfBottomSheet"/> when initializing.
		/// </remarks>
		public static readonly BindableProperty ContentProperty =
			BindableProperty.Create(
				nameof(Content),
				typeof(View),
				typeof(SfBottomSheet),
				null,
				BindingMode.Default,
				null,
				propertyChanged: OnContentChanged);

		// Content and State
		/// <summary>
		/// Identifies the <see cref="BottomSheetContent"/> bindable property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="BottomSheetContent"/> bindable property.
		/// </value>
		public static readonly BindableProperty BottomSheetContentProperty = BindableProperty.Create(
		    nameof(BottomSheetContent),
		    typeof(View),
		    typeof(SfBottomSheet),
		    null,
		    BindingMode.Default,
		    propertyChanged: OnContentPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="State"/> bindable property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="State"/> bindable property.
		/// </value>
		public static readonly BindableProperty StateProperty = BindableProperty.Create(
		    nameof(State),
		    typeof(BottomSheetState),
		    typeof(SfBottomSheet),
		    BottomSheetState.Hidden,
		    BindingMode.Default,
		    propertyChanged: OnStatePropertyChanged);

		/// <summary>
		/// Identifies the <see cref="HalfExpandedRatio"/> bindable property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="HalfExpandedRatio"/> bindable property.
		/// </value>
		public static readonly BindableProperty HalfExpandedRatioProperty = BindableProperty.Create(
		    nameof(HalfExpandedRatio),
		    typeof(double),
		    typeof(SfBottomSheet),
			DefaultHalfExpandedRatio,
		    BindingMode.Default,
		    propertyChanged: OnHalfExpandedRatioPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="FullExpandedRatio"/> bindable property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="FullExpandedRatio"/> bindable property.
		/// </value>
		public static readonly BindableProperty FullExpandedRatioProperty = BindableProperty.Create(
			nameof(FullExpandedRatio),
			typeof(double),
			typeof(SfBottomSheet),
			DefaultFullExpandedRatio,
			BindingMode.Default,
			propertyChanged: OnFullExpandedRatioPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="CollapsedHeight"/> bindable property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="CollapsedHeight"/> bindable property.
		/// </value>
		public static readonly BindableProperty CollapsedHeightProperty = BindableProperty.Create(
			nameof(CollapsedHeight),
			typeof(double),
			typeof(SfBottomSheet),
			MinimizedHeight,
			BindingMode.Default,
			propertyChanged: OnCollapsedHeightPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="BottomSheetContentWidth"/> bindable property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="BottomSheetContentWidth"/> bindable property.
		/// </value>
		public static readonly BindableProperty BottomSheetContentWidthProperty = BindableProperty.Create(
			nameof(BottomSheetContentWidth),
			typeof(double),
			typeof(SfBottomSheet),
			300.0,
			BindingMode.Default,
			propertyChanged: OnBottomSheetContentWidthPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="ContentWidthMode"/> bindable property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="ContentWidthMode"/> bindable property.
		/// </value>
		public static readonly BindableProperty ContentWidthModeProperty = BindableProperty.Create(
			nameof(ContentWidthMode),
			typeof(BottomSheetContentWidthMode),
			typeof(SfBottomSheet),
			BottomSheetContentWidthMode.Full,
			BindingMode.Default,
			propertyChanged: OnContentWidthModePropertyChanged);

		/// <summary>
		/// Identifies the <see cref="AllowedState"/> bindable property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="AllowedState"/> bindable property.
		/// </value>
		public static readonly BindableProperty AllowedStateProperty = BindableProperty.Create(
		    nameof(AllowedState),
		    typeof(BottomSheetAllowedState),
		    typeof(SfBottomSheet),
		    BottomSheetAllowedState.All,
		    BindingMode.Default,
		    propertyChanged: OnAllowedStatePropertyChanged);

		// Appearance
		/// <summary>
		/// Identifies the <see cref="IsModal"/> bindable property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="IsModal"/> bindable property.
		/// </value>
		public static readonly BindableProperty IsModalProperty = BindableProperty.Create(
		    nameof(IsModal),
		    typeof(bool),
		    typeof(SfBottomSheet),
		    true,
		    BindingMode.Default,
		    propertyChanged: OnIsModalPropertyChanged);
		
		/// <summary>
		/// Identifies the <see cref="ShowGrabber"/> bindable property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="ShowGrabber"/> bindable property.
		/// </value>
		public static readonly BindableProperty ShowGrabberProperty = BindableProperty.Create(
		    nameof(ShowGrabber),
		    typeof(bool),
		    typeof(SfBottomSheet),
		    true,
		    BindingMode.Default,
		    propertyChanged: OnShowGrabberPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="IsOpen"/> bindable property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="IsOpen"/> bindable property.
		/// </value>
		public static readonly BindableProperty IsOpenProperty = BindableProperty.Create(
			nameof(IsOpen),
			typeof(bool),
			typeof(SfBottomSheet),
			false,
			BindingMode.TwoWay,
			propertyChanged: OnIsOpenPropertyChanged);

		// Appearance (continued)
		/// <summary>
		/// Identifies the <see cref="GrabberBackground"/> bindable property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="GrabberBackground"/> bindable property.
		/// </value>
		public static readonly BindableProperty GrabberBackgroundProperty = BindableProperty.Create(
		    nameof(GrabberBackground), 
		    typeof(Brush), 
		    typeof(SfBottomSheet), 
		    new SolidColorBrush(Color.FromArgb("#CAC4D0")), 
		    BindingMode.Default,
		    propertyChanged: OnGrabberBackgroundPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="Background"/> bindable property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="Background"/> bindable property.
		/// </value>
		public static readonly new BindableProperty BackgroundProperty = BindableProperty.Create(
		    nameof(Background), 
		    typeof(Brush), 
		    typeof(SfBottomSheet), 
		    new SolidColorBrush(Color.FromArgb("#F7F2FB")), 
		    BindingMode.Default,
		    propertyChanged: OnBackgroundPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="CornerRadius"/> bindable property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="CornerRadius"/> bindable property.
		/// </value>
		public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(
		    nameof(CornerRadius), 
		    typeof(CornerRadius), 
		    typeof(SfBottomSheet), 
		    new CornerRadius(0), 
		    BindingMode.Default,
		    propertyChanged: OnCornerRadiusPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="ContentPadding"/> bindable property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="ContentPadding"/> bindable property.
		/// </value>
		public static readonly BindableProperty ContentPaddingProperty = BindableProperty.Create(
		    nameof(ContentPadding), 
		    typeof(Thickness), 
		    typeof(SfBottomSheet), 
		    new Thickness(5), 
		    BindingMode.Default,
		    propertyChanged: OnContentPaddingPropertyChanged);

		// Behavior
		/// <summary>
		/// Identifies the <see cref="EnableSwiping"/> bindable property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="EnableSwiping"/> bindable property.
		/// </value>
		public static readonly BindableProperty EnableSwipingProperty = BindableProperty.Create(
		    nameof(EnableSwiping), 
		    typeof(bool), 
		    typeof(SfBottomSheet), 
		    true,
		    BindingMode.Default);

		// Grabber customization
		/// <summary>
		/// Identifies the <see cref="GrabberHeight"/> bindable property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="GrabberHeight"/> bindable property.
		/// </value>
		public static readonly BindableProperty GrabberHeightProperty = BindableProperty.Create(
		    nameof(GrabberHeight), 
		    typeof(double), 
		    typeof(SfBottomSheet), 
		    DefaultGrabberHeight, 
		    BindingMode.Default, 
		    propertyChanged: OnGrabberHeightPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="GrabberWidth"/> bindable property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="GrabberWidth"/> bindable property.
		/// </value>
		public static readonly BindableProperty GrabberWidthProperty = BindableProperty.Create(
		    nameof(GrabberWidth), 
		    typeof(double), 
		    typeof(SfBottomSheet), 
		    DefaultGrabberWidth, 
		    BindingMode.Default, 
		    propertyChanged: OnGrabberWidthPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="GrabberCornerRadius"/> bindable property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="GrabberCornerRadius"/> bindable property.
		/// </value>
		public static readonly BindableProperty GrabberCornerRadiusProperty = BindableProperty.Create(
		    nameof(GrabberCornerRadius), 
		    typeof(CornerRadius), 
		    typeof(SfBottomSheet), 
		    new CornerRadius(DefaultGrabberCornerRadius), 
		    BindingMode.Default, 
		    propertyChanged: OnGrabberCornerRadiusPropertyChanged);

		#endregion

		#region Internal Bindable Properties

		/// <summary>
		/// Identifies the <see cref="OverlayBackgroundColor"/> bindable property.
		/// </summary>
		/// <remarks>
		/// This property determines the background color of the overlay that appears behind the bottom sheet.
		/// The default value is a semi-transparent black color (#80000000).
		/// </remarks>
		internal static readonly BindableProperty OverlayBackgroundColorProperty = BindableProperty.Create(
			nameof(OverlayBackgroundColor), 
			typeof(Color), 
			typeof(SfBottomSheet), 
			Color.FromArgb("#80000000"), 
			BindingMode.Default, 
			propertyChanged: OnOverlayBackgroundColorChanged);

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="SfBottomSheet"/> class.
		/// </summary>
		public SfBottomSheet()
        {
            ThemeElement.InitializeThemeResources(this, "SfBottomSheetTheme");
			InitializeLayout();
			ApplyThemeResources();
        }

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the view that can be used to customize the main content of the SfBottomSheet.
		/// </summary>
		/// <value>
		/// A <see cref="View"/> that represents the content view of the bottom sheet.
		/// </value>
		public View Content
		{
			get { return (View)GetValue(ContentProperty); }
			set { SetValue(ContentProperty, value); }
		}

		/// <summary>
		/// Gets or sets the content of the SfBottomSheet control.
		/// </summary>
		/// <value>
		/// A <see cref="View"/> that represents the content of the bottom sheet.
		/// </value>
		public View BottomSheetContent
		{
		    get => (View)GetValue(BottomSheetContentProperty);
		    set => SetValue(BottomSheetContentProperty, value);
		}

		/// <summary>
		/// Gets or sets the expanded state of the SfBottomSheet control.
		/// </summary>
		/// <value>
		/// A <see cref="BottomSheetState"/> value. The default is <see cref="BottomSheetState.Hidden"/>.
		/// Possible values are:
		/// - <see cref="BottomSheetState.Hidden"/>
		/// - <see cref="BottomSheetState.Collapsed"/>
		/// - <see cref="BottomSheetState.HalfExpanded"/>
		/// - <see cref="BottomSheetState.FullExpanded"/>
		/// </value>
		public BottomSheetState State
		{
		    get => (BottomSheetState)GetValue(StateProperty);
		    set => SetValue(StateProperty, value);
		}

		/// <summary>
		/// Gets or sets the height ratio of the bottom sheet in half-expanded state.
		/// </summary>
		/// <value>
		/// A <see cref="double"/> value between 0 and 1. The default value is 0.5.
		/// </value>
		/// <remarks>
		/// This value represents the fraction of the screen height that the bottom sheet will occupy when half-expanded.
		/// For example, 0.5 means the bottom sheet will cover half of the screen.
		/// </remarks>
		public double HalfExpandedRatio
		{
		    get => (double)GetValue(HalfExpandedRatioProperty);
		    set => SetValue(HalfExpandedRatioProperty, value);
		}

		/// <summary>
		/// Gets or sets the height ratio of the bottom sheet in full-expanded state. 
		/// </summary>
		/// <value>
		/// A <see cref="double"/> value between 0 and 1. The default value is 1.
		/// </value>
		/// <remarks>
		/// This value represents the fraction of the screen height that the bottom sheet will occupy when full-expanded.
		/// For example, 0.75 means the bottom sheet will cover 3/4th of the screen.
		/// </remarks>
		public double FullExpandedRatio
		{
			get => (double)GetValue(FullExpandedRatioProperty);
			set => SetValue(FullExpandedRatioProperty, value);
		}

		/// <summary>
		/// Gets or sets the height of the bottom sheet in collapsed state. 
		/// </summary>
		/// <value>
		/// A <see cref="double"/> value representing the height in device-independent units. The default value is 100.
		/// </value>
		/// <remarks>
		/// This value represents the height bottom sheet will occupy when collapsed.
		/// </remarks>
		public double CollapsedHeight
		{
			get => (double)GetValue(CollapsedHeightProperty);
			set => SetValue(CollapsedHeightProperty, value);
		}

		/// <summary>
		/// Specifies the custom width value (in pixels) for the BottomSheet when ContentWidthMode is set to Custom.
		/// </summary>
		public double BottomSheetContentWidth
		{
			get => (double)GetValue(BottomSheetContentWidthProperty);
			set => SetValue(BottomSheetContentWidthProperty, value);
		}

		/// <summary>
		/// Gets or sets a value that customizes the content width for the bottom sheet.
		/// </summary>
		/// <value>
		/// A <see cref="BottomSheetContentWidthMode"/> value. The default is <see cref="BottomSheetContentWidthMode.Full"/>.
		/// Possible values are:
		/// - <see cref="BottomSheetContentWidthMode.Full"/>
		/// - <see cref="BottomSheetContentWidthMode.Custom"/>
		/// </value> 
		public BottomSheetContentWidthMode ContentWidthMode
		{
			get => (BottomSheetContentWidthMode)GetValue(ContentWidthModeProperty);
			set => SetValue(ContentWidthModeProperty, value);
		}

		/// <summary>
		/// Gets or sets a value that indicates the allowed states for the bottom sheet.
		/// </summary>
		/// <value>
		/// A <see cref="BottomSheetAllowedState"/> value. The default is <see cref="BottomSheetAllowedState.All"/>.
		/// Possible values are:
		/// - <see cref="BottomSheetAllowedState.All"/>
		/// - <see cref="BottomSheetAllowedState.HalfExpanded"/>
		/// - <see cref="BottomSheetAllowedState.FullExpanded"/>
		/// </value> 
		public BottomSheetAllowedState AllowedState
		{
		    get => (BottomSheetAllowedState)GetValue(AllowedStateProperty);
		    set => SetValue(AllowedStateProperty, value);
		}

		/// <summary>
		/// Gets or sets a value that indicates whether the SfBottomSheet acts as a modal dialog.
		/// </summary>
		/// <value>
		/// A <see cref="bool"/> value. The default value is <c>true</c>.
		/// </value>
		/// <remarks>
		/// When set to <c>true</c>, the bottom sheet will behave like a modal dialog, preventing interaction with underlying content.
		/// </remarks>
		public bool IsModal
		{
		    get => (bool)GetValue(IsModalProperty);
		    set => SetValue(IsModalProperty, value);
		}

		/// <summary>
		/// Gets or sets a value indicating whether to show the drag handle (grabber) in the SfBottomSheet.
		/// </summary>
		/// <value> 
		/// A <see cref="bool"/> value. The default value is <c>true</c>.
		/// </value>
		public bool ShowGrabber
		{
		    get => (bool)GetValue(ShowGrabberProperty);
		    set => SetValue(ShowGrabberProperty, value);
		}

		/// <summary>
		/// Gets or sets a value that indicates whether the SfBottomSheet is opened or not.
		/// </summary>
		/// <value>
		/// A <see cref="bool"/> value. The default value is <c>false</c>.
		/// </value>
		/// <remarks>
		/// When set to <c>true</c>, the bottom sheet will be opened.
		/// </remarks>
		public bool IsOpen
		{
			get => (bool)GetValue(IsOpenProperty);
			set => SetValue(IsOpenProperty, value);
		}

		/// <summary>
		/// Gets or sets the background of the SfBottomSheet.
		/// </summary>
		/// <value>
		/// A <see cref="Brush"/> value representing the background.
		/// </value>
		/// <remarks>
		/// This property overrides the background property from the base class.
		/// </remarks>
		public new Brush Background
		{
		    get => (Brush)GetValue(BackgroundProperty);
		    set => SetValue(BackgroundProperty, value);
		}

		/// <summary>
		/// Gets or sets the corner radius of the SfBottomSheet.
		/// </summary>
		/// <value>
		/// A <see cref="CornerRadius"/> value. The default value is 0.
		/// </value>
		/// <remarks>
		/// This property allows you to round the corners of the bottom sheet.
		/// Set all values to 0 for sharp corners, or provide individual values for each corner.
		/// </remarks>
		public CornerRadius CornerRadius
		{
		    get => (CornerRadius)GetValue(CornerRadiusProperty);
		    set => SetValue(CornerRadiusProperty, value);
		}

		/// <summary>
		/// Gets or sets the padding of the content in SfBottomSheet.
		/// </summary>
		/// <value>
		/// A <see cref="Thickness"/> value representing the padding. The default value is 5 on all sides.
		/// </value>
		/// <remarks>
		/// Use this property to add space between the content and the edges of the bottom sheet.
		/// </remarks>
		public Thickness ContentPadding
		{
		    get => (Thickness)GetValue(ContentPaddingProperty);
		    set => SetValue(ContentPaddingProperty, value);
		}

		/// <summary>
		/// Gets or sets the background color of the grabber in SfBottomSheet.
		/// </summary>
		/// <value>
		/// A <see cref="Brush"/> value representing the grabber's background color.
		/// </value>
		public Brush GrabberBackground
		{
		    get => (Brush)GetValue(GrabberBackgroundProperty);
		    set => SetValue(GrabberBackgroundProperty, value);
		}

		/// <summary>
		/// Gets or sets a value indicating whether swiping is enabled in the SfBottomSheet control.
		/// </summary>
		/// <value>
		/// A <see cref="bool"/> value. The default value is <c>true</c>.
		/// </value>
		/// <remarks>
		/// When set to <c>true</c>, users can swipe the bottom sheet to change its state.
		/// When set to <c>false</c>, swiping gestures are disabled.
		/// </remarks>
		public bool EnableSwiping
		{
		    get => (bool)GetValue(EnableSwipingProperty);
		    set => SetValue(EnableSwipingProperty, value);
		}

		/// <summary>
		/// Gets or sets the height of the grabber in SfBottomSheet.
		/// </summary>
		/// <value>
		/// A <see cref="double"/> value representing the height in device-independent units. The default value is 4.
		/// </value>
		/// <remarks>
		/// This property allows you to customize the size of the grabber handle.
		/// </remarks>
		public double GrabberHeight
		{
		    get => (double)GetValue(GrabberHeightProperty);
		    set => SetValue(GrabberHeightProperty, value);
		}

		/// <summary>
		/// Gets or sets the width of the grabber in SfBottomSheet.
		/// </summary>
		/// <value>
		/// A <see cref="double"/> value representing the width in device-independent units. The default value is 32.
		/// </value>
		/// <remarks>
		/// This property allows you to customize the size of the grabber handle.
		/// </remarks>
		public double GrabberWidth
		{
		    get => (double)GetValue(GrabberWidthProperty);
		    set => SetValue(GrabberWidthProperty, value);
		}

		/// <summary>
		/// Gets or sets the corner radius of the grabber in SfBottomSheet.
		/// </summary>
		/// <value>
		/// A <see cref="CornerRadius"/> value. The default value is 0 for all corners.
		/// </value>
		/// <remarks>
		/// Use this property to round the corners of the grabber handle.
		/// </remarks>
		public CornerRadius GrabberCornerRadius
		{
		    get => (CornerRadius)GetValue(GrabberCornerRadiusProperty);
		    set => SetValue(GrabberCornerRadiusProperty, value);
		}

		#endregion

		#region Internal Properties

		/// <summary>
		/// Gets or sets the background color of the overlay grid.
		/// </summary>
		/// <value>
		/// A <see cref="Color"/> value representing the background color of the overlay.
		/// The default value is a semi-transparent black color.
		/// </value>
		internal Color OverlayBackgroundColor
		{
		    get => (Color)GetValue(OverlayBackgroundColorProperty);
		    set => SetValue(OverlayBackgroundColorProperty, value);
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Shows the SfBottomSheet.
		/// </summary>
		/// <exception cref="InvalidOperationException">
		/// Thrown when the bottom sheet or overlay grid is not initialized.
		/// </exception>
		public void Show()
		{
			if (_bottomSheet is null || _overlayGrid is null)
			{
				return;
			}

			_bottomSheet.IsVisible = true;
		    if (!IsValidHeight())
		    {
		        RegisterSizeChangedEvent();
		        return;
		    }

			SetupBottomSheetForShow();
			AnimateBottomSheet(GetTargetPosition());
			IsOpen = true;
		}

		/// <summary>
		/// Closes the SfBottomSheet.
		/// </summary>
		/// <exception cref="InvalidOperationException">
		/// Thrown when the bottom sheet or overlay grid is not initialized.
		/// </exception>
		public void Close()
		{
		    if(_bottomSheet is null || _overlayGrid is null)
			{
				return;
			}

		    AnimateBottomSheet(Height, onFinish: () =>
		    {
		        _bottomSheet.IsVisible = false;
		        _overlayGrid.IsVisible = false;
			});

			if (_isSheetOpen)
		    {
		        _isSheetOpen = false;
				IsOpen = false;
				State = BottomSheetState.Hidden;
		    }
		}

		#endregion

		#region Internal Methods

		/// <summary>
		/// Raises the StateChanged event when the sheet's state changes.
		/// </summary>
		/// <param name="args">The state changed event arguments.</param>
		internal virtual void OnStateChanged(StateChangedEventArgs args)
		{
		    StateChanged?.Invoke(this, args);
		}

		/// <summary>
		/// Handles touch-related logic for the bottom sheet.
		/// </summary>
		/// <param name="action">The pointer action.</param>
		/// <param name="point">The touch point.</param>
		internal void OnHandleTouch(PointerActions action, Point point)
		{
		    if (!EnableSwiping || !_isSheetOpen || _bottomSheet is null)
		    {
		        return;
		    }

		    double touchY = GetPlatformAdjustedTouchY(point);

		    switch (action)
		    {
		        case PointerActions.Pressed:
		            HandleTouchPressed(touchY);
		            return;
		        case PointerActions.Moved:
		            HandleTouchMoved(touchY);
		            return;
		        case PointerActions.Released:
		            HandleTouchReleased(touchY);
		            return;
		        default:
		            // Log or handle unexpected action
		            return;
		    }
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Initializes the layout of the SfBottomSheet.
		/// </summary>
		void InitializeLayout()
		{
		    InitializeOverlayGrid();
		    InitializeGrabber();
		    InitializeBottomSheetContent();
		    InitializeBottomSheetBorder();
		    InitializeContentBorder();

		    if (_bottomSheet is not null && _overlayGrid is not null)
		    {
				Children.Add(_overlayGrid);
				Children.Add(_bottomSheet);
				_bottomSheet.IsVisible = false;
			}
		}

		/// <summary>
		/// This method is used to clear and update the children to the SfBottomSheet.
		/// </summary>
		void UpdateContentView()
		{
			Children.Clear();
			UpdateAllChild();
		}

		/// <summary>
		/// The method used to update all children of the SfBottomSheet.
		/// </summary>
		void UpdateAllChild()
		{
			AddChild(Content);
			AddChild(_overlayGrid);
			AddChild(_bottomSheet);
		}

		/// <summary>
		/// The method used to add child of the SfBottomSheet.
		/// </summary>
		/// <param name="child">The view to be set as child of the bottom sheet.</param>
		void AddChild(View? child)
		{
			if (child is not null)
			{
				Children.Add(child);
			}
		}

		/// <summary>
		/// Initializes the overlay grid of the bottom sheet.
		/// </summary>
		void InitializeOverlayGrid()
		{
		    _overlayGrid = new SfGrid()
		    {
		        BackgroundColor = OverlayBackgroundColor,
		        Opacity = DefaultOverlayOpacity,
		        IsVisible = false
		    };

		    var tapGestureRecognizer = new TapGestureRecognizer();
    		tapGestureRecognizer.Tapped += OnOverlayGridTapped;
    		_overlayGrid.GestureRecognizers.Add(tapGestureRecognizer);
		}

		/// <summary>
		/// Initializes the grabber (drag handle) of the bottom sheet.
		/// </summary>
		void InitializeGrabber()
		{
		    _grabberStrokeShape = new RoundRectangle() { CornerRadius = DefaultGrabberCornerRadius };

		    _grabber = new SfBorder()
		    {
		        Background = GrabberBackground,
		        Stroke = Colors.Transparent,
		        HeightRequest = DefaultGrabberHeight,
		        WidthRequest = DefaultGrabberWidth,
		        HorizontalOptions = LayoutOptions.Center,
		        VerticalOptions = LayoutOptions.Center,
		        StrokeShape = _grabberStrokeShape
		    };
		}

		/// <summary>
		/// Initializes the content of the bottom sheet.
		/// </summary>
		void InitializeBottomSheetContent()
		{
		    _bottomSheetContent = new SfGrid()
		    {
		        Background = Background,
		        RowDefinitions = new RowDefinitionCollection
		        {
		            new RowDefinition { Height = new GridLength(DefaultGrabberRowHeight) },
		            new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
		        }
		    };

		    // Add grabber to the first row if it has been initialized
   			if (_grabber is not null)
    		{
        		_bottomSheetContent.Children.Add(_grabber);
        		SfGrid.SetRow(_grabber, 0);
    		}
		}

		/// <summary>
		/// Initializes the border of the bottom sheet.
		/// </summary>
		void InitializeBottomSheetBorder()
		{
		    _bottomSheetStrokeShape = new RoundRectangle() { CornerRadius = CornerRadius };

		    _bottomSheet = new BottomSheetBorder(this)
		    {
		        Background = Background,
		        StrokeThickness = 0,
		        VerticalOptions = LayoutOptions.Start,
				HorizontalOptions = LayoutOptions.Fill,
		        HeightRequest = CalculateInitialHeight(),
				IsVisible = false,
		        StrokeShape = _bottomSheetStrokeShape,
		        Content = _bottomSheetContent ?? throw new InvalidOperationException("Bottom sheet content is not initialized."),
				Padding = ContentPadding
		    };
		}

		/// <summary>
		/// Calculates the initial height for the half-expanded state.
		/// </summary>
		/// <returns>The calculated initial height.</returns>
		double CalculateInitialHeight()
		{
			return Height > 0 ? Height * HalfExpandedRatio : 0;
		}

		/// <summary>
		/// Initializes the content border of the bottom sheet.
		/// </summary>
		void InitializeContentBorder()
		{
		    _contentBorder = new SfBorder()
		    {
		        StrokeThickness = 0
		    };
		}

		/// <summary>
		/// Applies theme resources to the control.
		/// </summary>
		void ApplyThemeResources()
		{
		   SetDynamicResource(OverlayBackgroundColorProperty, "SfBottomSheetOverlayBackgroundColor");
		}

		/// <summary>
		/// Updates the HalfExpandedRatio property with the given value.
		/// </summary>
		/// <param name="newValue">The new value for HalfExpandedRatio.</param>
		void UpdateHalfExpandedRatioProperty(double newValue)
		{
		    double clampedValue = Math.Clamp(newValue, MinHalfExpandedRatio, MaxHalfExpandedRatio);
		    HalfExpandedRatio = clampedValue;
		    if (State is BottomSheetState.HalfExpanded)
		    {
		        OnSizeAllocated(Width, Height);
		    }
		}

		/// <summary>
		/// Updates the FullExpandedRatio property with the given value.
		/// </summary>
		/// <param name="newValue">The new value for FullExpandedRatio.</param>
		void UpdateFullExpandedRatioProperty(double newValue)
		{
			double clampedValue = Math.Clamp(newValue, MinFullfExpandedRatio, MaxFullfExpandedRatio);
			FullExpandedRatio = clampedValue;
			if (State is BottomSheetState.FullExpanded)
			{
				OnSizeAllocated(Width, Height);
			}
		}

		/// <summary>
		/// Updates the CollapsedHeight property with the given value.
		/// </summary>
		/// <param name="newValue">The new value for CollapsedHeight.</param>
		void UpdateCollapsedHeightProperty(double newValue)
		{
			if(CollapsedHeight<=0)
			{
				return;
			}

			if (State is BottomSheetState.Collapsed)
			{
				OnSizeAllocated(Width, Height);
			}
		}

		/// <summary>
		/// Updates the BottomSheetContentWidth property with the given value.
		/// </summary>
		/// <param name="newValue">The new value for BottomSheetContentWidth.</param>
		void UpdateContentWidthProperty(double newValue)
		{
			if (BottomSheetContentWidth <= 0 || ContentWidthMode == BottomSheetContentWidthMode.Full)
			{
				return;
			}

			if(_bottomSheet is not null)
			{
				_bottomSheet.WidthRequest = newValue;
			}
		}


		/// <summary>
		/// Updates the ContentWidthMode property with the given value.
		/// </summary>
		/// <param name="newValue">The new value for ContentWidthMode.</param>
		void UpdateContentWidthModeProperty(BottomSheetContentWidthMode newValue)
		{
			if (_bottomSheet is not null)
			{
				if (newValue is BottomSheetContentWidthMode.Full)
				{
					_bottomSheet.WidthRequest = Width;
				}
				else if (BottomSheetContentWidth > 0)
				{
					_bottomSheet.WidthRequest = BottomSheetContentWidth;
				}
			}
		}

		/// <summary>
		/// Sets the content of the bottom sheet.
		/// </summary>
		/// <param name="content">The view to be set as the content of the bottom sheet.</param>
		/// <exception cref="ArgumentNullException">Thrown when the content is null.</exception>
		/// <exception cref="InvalidOperationException">
		/// Thrown when either the bottom sheet content or content border is not initialized.
		/// </exception>
		void SetBottomSheetContent(View? content)
		{
		    if (content is null || _bottomSheetContent is null)
		    {
				return;
		    }

		    // Remove existing content if any
		    if (_bottomSheetContent.Children.Count > 1)
		    {
		        _bottomSheetContent.Children.RemoveAt(1);
		    }

		    // Set new content
		    if (_contentBorder is not null)
		    {
		        _contentBorder.Content = content;
		    }

		    // Add content border to bottom sheet
		    _bottomSheetContent.Children.Add(_contentBorder);
		    SfGrid.SetRow(_contentBorder, 1);
		}


		/// <summary>
		/// Updates the height of the first row in the bottom sheet content based on the visibility of the grabber.
		/// </summary>
		void UpdateRowHeight()
		{
		    const int GrabberRowIndex = 0;

		    if (_bottomSheetContent is not null)
		    {
		        _bottomSheetContent.RowDefinitions[GrabberRowIndex].Height = 
		            ShowGrabber ? new GridLength(DefaultGrabberRowHeight) : new GridLength(0);
		    }
		}


		/// <summary>
		/// Handles the tapped event on the overlay grid.
		/// This method is called when the user taps on the overlay area of the bottom sheet.
		/// </summary>
		/// <param name="sender">The object that raised the event.</param>
		/// <param name="e">The event arguments.</param>
		void OnOverlayGridTapped(object? sender, EventArgs e)
		{
		    if (_isSheetOpen)
		    {
		        Close();
		    }
		}

		/// <summary>
		/// Updates the state of the bottom sheet based on the current AllowedState and open status.
		/// </summary>
		void UpdateState()
		{
		    var (newState, newIsHalfExpanded) = AllowedState switch
		    {
		        BottomSheetAllowedState.HalfExpanded => (_isSheetOpen ? BottomSheetState.HalfExpanded : State, true),
		        BottomSheetAllowedState.FullExpanded => (_isSheetOpen ? BottomSheetState.FullExpanded : State, false),
		        BottomSheetAllowedState.All => (State, _isHalfExpanded),
		        _ => (!_isSheetOpen ? BottomSheetState.Hidden : State, true)
		    };

		    if (!newState.Equals(State))
		    {
		        SetValue(StateProperty, newState);
		    }

		    _isHalfExpanded = newIsHalfExpanded;
		}

		/// <summary>
		/// Updates the background of the bottom sheet and its content.
		/// </summary>
		/// <param name="brush">The brush to be set as the background. If null, the default background is used.</param>
		void UpdateBottomSheetBackground(Brush brush)
		{
		    if (_bottomSheet is null || _bottomSheetContent is null)
		    {
		        return;
		    }

		    var background = brush ?? (Brush)SfBottomSheet.BackgroundProperty.DefaultValue;
		    _bottomSheet.Background = background;
		    _bottomSheetContent.Background = background;
		}


		/// <summary>
		/// Updates the background of the grabber.
		/// </summary>
		/// <param name="brush">The brush to be set as the grabber's background. If null, the default background is used.</param>
		void UpdateGrabberBackground(Brush brush)
		{
		    if (_grabber is null)
		    {
		        return;
		    }

		    _grabber.Background = brush ?? (Brush)SfBottomSheet.GrabberBackgroundProperty.DefaultValue;
		}


		/// <summary>
		/// Updates the corner radius of the bottom sheet.
		/// </summary>
		/// <param name="cornerRadius">The new corner radius to be applied.</param>
		void UpdateCornerRadius(CornerRadius cornerRadius)
		{
		    if (_bottomSheet?.StrokeShape is null || _bottomSheetStrokeShape is null)
		    {
		        return;
		    }

		    cornerRadius = EnsureValidCornerRadius(cornerRadius);

		    if (!_bottomSheetStrokeShape.CornerRadius.Equals(cornerRadius))
		    {
		        _bottomSheetStrokeShape.CornerRadius = cornerRadius;
		        _bottomSheet.StrokeShape = _bottomSheetStrokeShape;
		        OnPropertyChanged(nameof(CornerRadius));
		    }
		}

		/// <summary>
		/// Ensures that the provided corner radius has valid (non-negative) values.
		/// </summary>
		/// <param name="cornerRadius">The corner radius to check.</param>
		/// <returns>A valid corner radius. If the input was invalid, returns the default value.</returns>
		CornerRadius EnsureValidCornerRadius(CornerRadius cornerRadius)
		{
		    return (cornerRadius.TopLeft < 0 || cornerRadius.TopRight < 0 || cornerRadius.BottomLeft < 0 || cornerRadius.BottomRight < 0)
		        ? (CornerRadius)SfBottomSheet.CornerRadiusProperty.DefaultValue
		        : cornerRadius;
		}


		/// <summary>
		/// Updates the padding of the content border if it has changed.
		/// </summary>
		/// <param name="padding">The new padding to be applied.</param>
		void UpdatePadding(Thickness padding)
		{
		    if (_bottomSheet is not null && !_bottomSheet.Padding.Equals(padding))
		    {
		        _bottomSheet.Padding = padding;
		        OnPropertyChanged(nameof(ContentPadding));
		    }
		}

		/// <summary>
		/// Updates the height of the grabber, ensuring it's not negative.
		/// </summary>
		/// <param name="newValue">The new height to be set for the grabber.</param>
		void UpdateGrabberHeightProperty(double newValue)
		{
		    if (_grabber is null)
		    {
		        return;
		    }

		    _grabber.HeightRequest = (newValue<=0) ? (double)(GrabberHeightProperty.DefaultValue) : newValue; 
		}

		/// <summary>
		/// Updates the width of the grabber, ensuring it's not negative.
		/// </summary>
		/// <param name="newValue">The new width to be set for the grabber.</param>
		void UpdateGrabberWidthProperty(double newValue)
		{
		    if (_grabber is null)
		    {
		        return;
		    }

		    _grabber.WidthRequest = (newValue<=0) ? (double)GrabberWidthProperty.DefaultValue : newValue;
		}


		/// <summary>
		/// Updates the corner radius of the grabber.
		/// </summary>
		/// <param name="cornerRadius">The new corner radius to be applied to the grabber.</param>
		void UpdateGrabberCornerRadius(CornerRadius cornerRadius)
		{
		    if (_grabber?.StrokeShape is null || _grabberStrokeShape is null)
		    {
		        return;
		    }

		    cornerRadius = EnsureValidCornerRadius(cornerRadius, SfBottomSheet.GrabberCornerRadiusProperty.DefaultValue);

		    if (!_grabberStrokeShape.CornerRadius.Equals(cornerRadius))
		    {
		        _grabberStrokeShape.CornerRadius = cornerRadius;
		        _grabber.StrokeShape = _grabberStrokeShape;
		        OnPropertyChanged(nameof(GrabberCornerRadius));
		    }
		}

		/// <summary>
		/// Ensures that the provided corner radius has valid (non-negative) values.
		/// </summary>
		/// <param name="cornerRadius">The corner radius to check.</param>
		/// <param name="defaultValue">The default value to use if the corner radius is invalid.</param>
		/// <returns>A valid corner radius. If the input was invalid, returns the default value.</returns>
		CornerRadius EnsureValidCornerRadius(CornerRadius cornerRadius, object defaultValue)
		{
		    return (cornerRadius.TopLeft < 0 || cornerRadius.TopRight < 0 || cornerRadius.BottomLeft < 0 || cornerRadius.BottomRight < 0)
		        ? (CornerRadius)defaultValue
		        : cornerRadius;
		}


		/// <summary>
		/// Handles the SizeChanged event of the control.
		/// This method is called when the size of the control changes, and it shows the bottom sheet.
		/// </summary>
		/// <param name="sender">The object that raised the event.</param>
		/// <param name="e">The event arguments.</param>
		void OnSizeChanged(object? sender, EventArgs e)
		{
		    if (_bottomSheet is not null && _bottomSheet.IsVisible)
		    {
		        Show();
		    }
		}


		/// <summary>
		/// Updates the position of the bottom sheet based on swipe gestures, transitioning between states.
		/// </summary>
		void UpdatePosition()
		{
		    const double SwipeThreshold = 100;
		    const double DoubleSwipeThreshold = SwipeThreshold * 2;
		    double swipeDistance = _endTouchY - _startTouchY;

		    switch (State)
		    {
		        case BottomSheetState.FullExpanded:
		            HandleFullExpandedState(swipeDistance, SwipeThreshold, DoubleSwipeThreshold);
		            break;
		        case BottomSheetState.HalfExpanded:
		            HandleHalfExpandedState(swipeDistance, SwipeThreshold);
		            break;
		        case BottomSheetState.Collapsed:
		            HandleCollapsedState(swipeDistance, SwipeThreshold);
		            break;
		    }
		}

		/// <summary>
		/// Handles the state transition of the bottom sheet when in a full-expanded state.
		/// </summary>
		/// <param name="swipeDistance">The distance swiped by the user.</param>
		/// <param name="swipeThreshold">The threshold required to transition to a half-expanded state.</param>
		/// <param name="doubleSwipeThreshold">The threshold required to transition to a collapsed state.</param>
		void HandleFullExpandedState(double swipeDistance, double swipeThreshold, double doubleSwipeThreshold)
		{
		    if (swipeDistance > swipeThreshold && AllowedState is not BottomSheetAllowedState.FullExpanded)
		    {
				UpdateStateBasedOnNearestPoint();
			}
		    else if (swipeDistance > doubleSwipeThreshold)
		    {
		        State = BottomSheetState.Collapsed;
		    }
		    else
		    {
		        Show();
		    }
		}

		/// <summary>
		/// Handles the state transition of the bottom sheet when in a half-expanded state.
		/// </summary>
		/// <param name="swipeDistance">The distance swiped by the user.</param>
		/// <param name="swipeThreshold">The threshold required to transition to a different state.</param>
		void HandleHalfExpandedState(double swipeDistance, double swipeThreshold)
		{
		    if (-swipeDistance > swipeThreshold && AllowedState is not BottomSheetAllowedState.HalfExpanded)
		    {
		        State = BottomSheetState.FullExpanded;
		    }
		    else if (swipeDistance > swipeThreshold)
		    {
		        State = BottomSheetState.Collapsed;
		    }
		    else
		    {
		        Show();
		    }
		}

		/// <summary>
		/// Handles the state transition of the bottom sheet when in a collapsed state.
		/// </summary>
		/// <param name="swipeDistance">The distance swiped by the user.</param>
		/// <param name="swipeThreshold">The threshold required to transition to a different state.</param>
		void HandleCollapsedState(double swipeDistance, double swipeThreshold)
		{
		    if (-swipeDistance > swipeThreshold)
		    {
				if(AllowedState == BottomSheetAllowedState.HalfExpanded)
				{
					State = BottomSheetState.HalfExpanded;
				}
				else if (AllowedState == BottomSheetAllowedState.FullExpanded)
				{
					State = BottomSheetState.FullExpanded;
				}
				else
				{
					UpdateStateBasedOnNearestPoint();
				}
			}
		    else
		    {
		        Show();
		    }
		}

		/// <summary>
		/// Methods to update the bottom sheet's state based on the nearest point.
		/// </summary>
		void UpdateStateBasedOnNearestPoint()
		{
			double fullExpandedHeight = Height * (1 - FullExpandedRatio);
			double halfExpandedHeight = Height * (1 - HalfExpandedRatio);
			double collapsedHeight = Height - CollapsedHeight;
			List<double> predefinedPoints = new List<double>
					{
						fullExpandedHeight, halfExpandedHeight, collapsedHeight
					};

			double nearestPoint = predefinedPoints.OrderBy(p => Math.Abs(p - _endTouchY)).First();

			if (nearestPoint == fullExpandedHeight)
			{
				if(State is not BottomSheetState.FullExpanded)
				{
					State = BottomSheetState.FullExpanded;
				}
				else
				{
					Show();
				}
			}
			else if (nearestPoint == halfExpandedHeight)
			{
				State = BottomSheetState.HalfExpanded;
			}
			else
			{
				if (State is not BottomSheetState.Collapsed)
				{
					State = BottomSheetState.Collapsed;
				}
				else
				{
					Show();
				}
			}
		}


		/// <summary>
		/// Updates the state and raises the StateChanged event when the state changes.
		/// </summary>
		/// <param name="oldState">The previous state of the bottom sheet.</param>
		/// <param name="newState">The new state of the bottom sheet.</param>
		void UpdateStateChanged(BottomSheetState oldState, BottomSheetState newState)
		{
		    if (!oldState.Equals(newState))
		    {
		        _stateChangedEventArgs.OldState = oldState;
		        _stateChangedEventArgs.NewState = newState;
				OnStateChanged(_stateChangedEventArgs);
		    }
		}

		/// <summary>
		/// Checks if the current height is valid for bottom sheet operations.
		/// </summary>
		/// <returns>True if the height is greater than 0 and not positive infinity; otherwise, false.</returns>
		bool IsValidHeight()
		{
			return Height > 0 && Height is not double.PositiveInfinity;
		}

		/// <summary>
		/// Registers the OnSizeChanged event handler, ensuring it's only registered once.
		/// </summary>
		void RegisterSizeChangedEvent()
		{
		    SizeChanged -= OnSizeChanged;
		    SizeChanged += OnSizeChanged;
		}


		/// <summary>
		/// Sets up the initial state of the bottom sheet and overlay grid for display.
		/// </summary>
		void SetupBottomSheetForShow()
		{
		    if (_isSheetOpen || _bottomSheet is null || _overlayGrid is null)
		    {
		        return;
		    }

		    // Position the bottom sheet just below the visible area
		    _bottomSheet.TranslationY = Height;
		    _bottomSheet.IsVisible = true;
		    _overlayGrid.IsVisible = IsModal;
		    _overlayGrid.Opacity = 0;
		}


		/// <summary>
		/// Calculates the target position for the bottom sheet based on its current state.
		/// </summary>
		/// <returns>The target Y position for the bottom sheet.</returns>
		double GetTargetPosition()
		{
		    if (State is BottomSheetState.FullExpanded || !_isHalfExpanded)
		    {
		        return GetFullExpandedPosition();
		    }

		    if (State is BottomSheetState.Collapsed)
		    {
		        return GetCollapsedPosition();
		    }

		    return GetHalfExpandedPosition();
		}

		/// <summary>
		/// Calculates and sets the position for the bottom sheet when fully expanded.
		/// </summary>
		/// <returns>The calculated position for the fully expanded state. Currently, it always returns 0.</returns>
		double GetFullExpandedPosition()
		{
		    if (State is BottomSheetState.Hidden)
		    {
		        State = BottomSheetState.FullExpanded;
		    }

			double targetPosition = Math.Abs(Height * (1 - FullExpandedRatio));
			if (_bottomSheet is not null)
			{
				_bottomSheet.HeightRequest = Height * FullExpandedRatio;
			}

		    return targetPosition;
		}

		/// <summary>
		/// Calculates the position of the bottom sheet when in the collapsed state.
		/// </summary>
		/// <returns>The calculated position for the collapsed state.</returns>
		double GetCollapsedPosition()
		{
		    double targetPosition = Height - CollapsedHeight;
			return targetPosition;
		}

		/// <summary>
		/// Calculates the position of the bottom sheet when in the half-expanded state.
		/// </summary>
		/// <returns>The calculated target position for the half-expanded state.</returns>
		double GetHalfExpandedPosition()
		{
		    double targetPosition = Height * (1 - HalfExpandedRatio);
			if (_bottomSheet is not null)
			{
				if (!_isSheetOpen || _bottomSheet.TranslationY > targetPosition)
				{
					State = BottomSheetState.HalfExpanded;
					_bottomSheet.HeightRequest = Height * HalfExpandedRatio;
				}
			}

		    return targetPosition;
		}

		/// <summary>
		/// Animates the bottom sheet and overlay grid to their target positions.
		/// </summary>
		/// <param name="targetPosition">The target Y position for the bottom sheet.</param>
		/// <param name="onFinish">Optional action to be executed when the animation finishes.</param>
		void AnimateBottomSheet(double targetPosition, Action? onFinish = null)
		{
			if (_bottomSheet.AnimationIsRunning("bottomSheetAnimation"))
			{
				_bottomSheet.AbortAnimation("bottomSheetAnimation");
			}

			if (_overlayGrid.AnimationIsRunning("overlayGridAnimation"))
			{
				_overlayGrid.AbortAnimation("overlayGridAnimation");
			}

			const int animationDuration = 150;
		    const int topPadding = 2;
			_isSheetOpen = true;
			if (_bottomSheet is not null)
			{
				var bottomSheetAnimation = new Animation(d => _bottomSheet.TranslationY = d, _bottomSheet.TranslationY, targetPosition + topPadding);
				_bottomSheet?.Animate("bottomSheetAnimation", bottomSheetAnimation, length: animationDuration, easing: Easing.Linear, finished: (v, e) =>
				{
					UpdateBottomSheetHeight();
					onFinish?.Invoke();
				});
			}

			AnimateOverlay(animationDuration);
		}

		/// <summary>
		/// Animates the overlay of the bottom sheet based on state transitions.
		/// </summary>
		void AnimateOverlay(int animationDuration)
		{
			if (_overlayGrid is not null)
			{
				double startValue = 0;
				double endValue = 0;
				_overlayGrid.IsVisible = IsModal;

				if (IsModal)
				{
					if (State is BottomSheetState.Collapsed || State is BottomSheetState.Hidden)
					{
						startValue = _overlayGrid.Opacity;
						endValue = 0;
					}
					else
					{
						startValue = _overlayGrid.Opacity;
						endValue = DefaultOverlayOpacity;
					}

					var overlayGridAnimation = new Animation(d => _overlayGrid.Opacity = d, startValue, endValue);
					_overlayGrid.Animate("overlayGridAnimation", overlayGridAnimation,
						length: (uint)animationDuration,
						easing: Easing.Linear,
						finished: (e, v) =>
						{
							if (State is BottomSheetState.Collapsed || State is BottomSheetState.Hidden)
							{
								_overlayGrid.IsVisible = false;
							}
						});
				}
			}
		}

		/// <summary>
		/// Updates the visibility of the bottom sheet based on IsOpen.
		/// </summary>
		void UpdateBottomSheetVisibility()
		{
			if(IsOpen && !_isSheetOpen)
			{
				Show();
			}
			else if(!IsOpen && _isSheetOpen)
			{
				Close();
			}
		}

		/// <summary>
		/// Updates the height of the bottom sheet based on its current state.
		/// </summary>
		void UpdateBottomSheetHeight()
		{
			if (!_isSheetOpen || _bottomSheet is null)
			{
				return;
			}

		    if (State is BottomSheetState.HalfExpanded)
		    {
		        _bottomSheet.HeightRequest = Height * HalfExpandedRatio;
		    }
		    else if (State is BottomSheetState.Collapsed)
		    {
		        _bottomSheet.HeightRequest = CollapsedHeight;
		    }
			else if(State is BottomSheetState.FullExpanded)
			{
				_bottomSheet.HeightRequest = Height * FullExpandedRatio;
			}
		}


		/// <summary>
		/// Adjusts the Y coordinate of a touch point based on the platform.
		/// </summary>
		/// <param name="point">The original touch point.</param>
		/// <returns>The adjusted Y coordinate.</returns>
		double GetPlatformAdjustedTouchY(Point point)
		{
#if IOS || MACCATALYST || ANDROID
		    return point.Y + Height - _bottomSheet?.HeightRequest ?? 0;
#else
		    return point.Y;
#endif
		}

		/// <summary>
		/// Handles the initial touch press event on the bottom sheet.
		/// </summary>
		/// <param name="touchY">The Y coordinate of the touch point.</param>
		void HandleTouchPressed(double touchY)
		{
		    _initialTouchY = touchY;
		    _isPointerPressed = true;
		    _startTouchY = _initialTouchY;
		}


		/// <summary>
		/// Handles touch movement on the bottom sheet.
		/// </summary>
		/// <param name="touchY">The current Y coordinate of the touch point.</param>
		void HandleTouchMoved(double touchY)
		{
		    if (!_isPointerPressed || _bottomSheet == null)
		    {
		        return;
		    }

		    double diffY = touchY - _initialTouchY;
		    double newTranslationY = Math.Max(0, _bottomSheet.TranslationY + diffY);

		    if (ShouldRestrictMovement(newTranslationY, diffY))
		    {
		        return;
		    }

		    UpdateBottomSheetPosition(newTranslationY, touchY);
		}


		/// <summary>
		/// Determines if the movement of the bottom sheet should be restricted.
		/// </summary>
		/// <param name="newTranslationY">The new translation Y value.</param>
		/// <param name="diffY">The difference in Y position from the last touch point.</param>
		/// <returns>True if movement should be restricted, false otherwise.</returns>
		bool ShouldRestrictMovement(double newTranslationY, double diffY)
		{
		    if (_bottomSheet is null)
		    {
		        return false;
		    }

			double endPosition = 0;
			double updatedHeight = Height - newTranslationY;
			switch (State)
			{
				case BottomSheetState.FullExpanded:
					endPosition = Height * FullExpandedRatio;
					break;

				case BottomSheetState.HalfExpanded:
					endPosition = Height * HalfExpandedRatio;
					break;

				case BottomSheetState.Collapsed:
					endPosition = CollapsedHeight;
					break;
			}

			bool isHalfExpandedAndRestricted = State is BottomSheetState.HalfExpanded &&
		                                       AllowedState is BottomSheetAllowedState.HalfExpanded &&
		                                       updatedHeight > endPosition;

		    bool isCollapsedAndMovingDown = State is BottomSheetState.Collapsed && updatedHeight < endPosition;

			bool isFullExpandedRestricted = State is BottomSheetState.FullExpanded &&
											updatedHeight > endPosition;

			bool isBehind = (newTranslationY > Height - CollapsedHeight) || (newTranslationY < Height * (1 - FullExpandedRatio));

			return isHalfExpandedAndRestricted || isCollapsedAndMovingDown || isFullExpandedRestricted || isBehind;
		}


		/// <summary>
		/// Updates the position of the bottom sheet based on touch input.
		/// </summary>
		/// <param name="newTranslationY">The new translation Y value for the bottom sheet.</param>
		/// <param name="touchY">The current Y coordinate of the touch point.</param>
		void UpdateBottomSheetPosition(double newTranslationY, double touchY)
		{
		    if (_bottomSheet is null || _overlayGrid is null)
		    {
		        return;
		    }

		    _bottomSheet.TranslationY = newTranslationY;
		    _initialTouchY = touchY;
		    _bottomSheet.HeightRequest = Height - newTranslationY;
			_overlayGrid.IsVisible = IsModal && (_bottomSheet.HeightRequest > CollapsedHeight);
			_overlayGrid.Opacity = CalculateOverlayOpacity(_bottomSheet.HeightRequest);
		}

		/// <summary>
		/// Calculates the overlay opacity based on the current height of the bottom sheet.
		/// </summary>
		/// <param name="currentHeight">The current height of the bottom sheet.</param>
		/// <returns>The calculated opacity value ranging from 0 to 0.5</returns>
		double CalculateOverlayOpacity(double currentHeight)
		{
			const double maxOpacity = 0.5;

			// Calculate how far along the transition from collapsed to half-expanded.
			double transitionProgress = (currentHeight - CollapsedHeight) / ((HalfExpandedRatio * Height) - CollapsedHeight);

			// Clamp the transition progress to between 0 and 1.
			transitionProgress = Math.Clamp(transitionProgress, 0, 1);

			// Calculate and return the opacity based on the transition progress.
			return transitionProgress * maxOpacity;
		}

		/// <summary>
		/// Handles the touch release event on the bottom sheet.
		/// </summary>
		/// <param name="touchY">The Y coordinate of the touch point when released.</param>
		void HandleTouchReleased(double touchY)
		{
		    _endTouchY = _initialTouchY;
		    _initialTouchY = 0;
		    _isPointerPressed = false;

		    if(_bottomSheet is not null && touchY >= _bottomSheet.TranslationY)
			{
				UpdatePosition();
			}
		}


		/// <summary>
		/// Updates the height and position of the bottom sheet when it's in a half-expanded state.
		/// </summary>
		/// <param name="height">The total height available for the bottom sheet.</param>
	    void UpdateHalfExpandedHeight(double height)
		{
		    if (_bottomSheet is { } sheet)
		    {
		        sheet.TranslationY = height * (1 - HalfExpandedRatio);
		        sheet.HeightRequest = height * HalfExpandedRatio;
		    }
		}

		/// <summary>
		/// Updates the height and position of the bottom sheet when it's in a collapsed state.
		/// </summary>
		/// <param name="height">The total height available for the bottom sheet.</param>
	    void UpdateCollapsedHeight(double height)
		{
			if (_bottomSheet is not null)
			{
				_bottomSheet.TranslationY = height - CollapsedHeight;
				_bottomSheet.HeightRequest = CollapsedHeight;
			}
		}

		/// <summary>
		/// Updates the height of the bottom sheet based on its current state.
		/// </summary>
		/// <param name="height">The height to be applied to the bottom sheet.</param>
		void UpdateBottomSheetHeight(double height)
		{
		    switch (State)
		    {
		        case BottomSheetState.Hidden:
		            SetBottomSheetHidden();
		            break;
				case BottomSheetState.Collapsed:
					UpdateCollapsedHeight(height);
					break;
				case BottomSheetState.HalfExpanded:
		            UpdateHalfExpandedHeight(height);
		            break;
		        case BottomSheetState.FullExpanded:
		            SetBottomSheetFullyExpanded(height);
		            break;
		    }
		}

		/// <summary>
		/// Sets the bottom sheet to a hidden state by adjusting its height to zero.
		/// </summary>
		void SetBottomSheetHidden()
		{
			if (_bottomSheet is not null)
			{
				_bottomSheet.HeightRequest = 0;
			}
		}

		/// <summary>
		/// Sets the bottom sheet to a fully expanded state by adjusting its height to the specified value.
		/// </summary>
		/// <param name="height">The height to set for the bottom sheet in its fully expanded state.</param>
		void SetBottomSheetFullyExpanded(double height)
		{
			if (_bottomSheet is not null)
			{
				_bottomSheet.TranslationY = Math.Abs(height * (1 - FullExpandedRatio));
				_bottomSheet.HeightRequest = height * FullExpandedRatio;
			}
		}

		/// <summary>
		/// Configures platform-specific behavior for the bottom sheet.
		/// </summary>
		void ConfigurePlatformSpecificBehavior()
		{
#if IOS || MACCATALYST || WINDOWS
    		ConfigureTouch();
#endif
		}

		#endregion

		#region Override Methods

		/// <summary>
		/// Handles size allocation for the bottom sheet, adjusting its height based on the current state.
		/// </summary>
		/// <param name="width">The allocated width.</param>
		/// <param name="height">The allocated height.</param>
		protected override void OnSizeAllocated(double width, double height)
		{
		    base.OnSizeAllocated(width, height);

		    if (_bottomSheet is null || height <= 0 || height is double.PositiveInfinity)
		    {
		        return;
		    }

		    UpdateBottomSheetHeight(height);
		}

		/// <summary>
		/// Called when the handler for the control changes. Configures platform-specific behavior.
		/// </summary>
		protected override void OnHandlerChanged()
		{
		    base.OnHandlerChanged();
		    ConfigurePlatformSpecificBehavior();
		}

		#endregion

		#region Property Changed Methods

		/// <summary>
		/// Handles changes to the <see cref="IsModal"/> property.
		/// </summary>
		/// <param name="bindable">The bindable object (should be SfBottomSheet).</param>
		/// <param name="oldValue">The old value of the IsModal property.</param>
		/// <param name="newValue">The new value of the IsModal property.</param>
		static void OnIsModalPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
		    if (bindable is SfBottomSheet sheet)
		    {
		        if (sheet._overlayGrid is not null && (sheet.State is BottomSheetState.FullExpanded || sheet.State is BottomSheetState.HalfExpanded))
		        {
		            sheet._overlayGrid.IsVisible = sheet.IsModal;
					sheet.AnimateOverlay(150);
		        }
		    }
		}


		/// <summary>
		/// Handles changes to the <see cref="ShowGrabber"/> property.
		/// </summary>
		/// <param name="bindable">The bindable object (should be SfBottomSheet).</param>
		/// <param name="oldValue">The old value of the ShowGrabber property.</param>
		/// <param name="newValue">The new value of the ShowGrabber property.</param>
		static void OnShowGrabberPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
		    if (bindable is SfBottomSheet sheet)
		    {
		        if (sheet._grabber is not null)
		        {
		            sheet._grabber.IsVisible = sheet.ShowGrabber;
		            sheet.UpdateRowHeight();
		        }
		    }
		}

		/// <summary>
		/// Handles changes to the <see cref="IsOpen"/> property.
		/// </summary>
		/// <param name="bindable">The bindable object (should be SfBottomSheet).</param>
		/// <param name="oldValue">The old value of the IsOpen property.</param>
		/// <param name="newValue">The new value of the IsOpen property.</param>
		static void OnIsOpenPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfBottomSheet sheet)
			{
				if (!oldValue.Equals(newValue))
				{
					sheet.UpdateBottomSheetVisibility();
				}
			}
		}

		/// <summary>
		/// Handles changes to the <see cref="BottomSheetContent"/> property of the bottom sheet.
		/// </summary>
		/// <param name="bindable">The bindable object (should be SfBottomSheet).</param>
		/// <param name="oldValue">The old value of the Content property.</param>
		/// <param name="newValue">The new value of the Content property.</param>
		static void OnContentPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
		    if (bindable is SfBottomSheet sheet)
		    {
		        if (newValue is View newContent)
		        {
		            sheet.SetBottomSheetContent(newContent);
		        }
		    }
		}

		/// <summary>
		/// Invoked whenever the <see cref="ContentProperty"/> is set.
		/// </summary>
		/// <param name="bindable">The bindable.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		static void OnContentChanged(BindableObject bindable, object oldValue, object newValue)
		{
			(bindable as SfBottomSheet)?.UpdateContentView();
		}

		/// <summary>
		/// Handles changes to the <see cref="State"/> property of the bottom sheet.
		/// </summary>
		/// <param name="bindable">The bindable object (should be SfBottomSheet).</param>
		/// <param name="oldValue">The old value of the state.</param>
		/// <param name="newValue">The new value of the state.</param>
		static void OnStatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is not SfBottomSheet sheet)
				return;

			var newState = (BottomSheetState)newValue;

			if ((sheet.AllowedState == BottomSheetAllowedState.HalfExpanded && newState == BottomSheetState.FullExpanded) ||
				(sheet.AllowedState == BottomSheetAllowedState.FullExpanded && newState == BottomSheetState.HalfExpanded))
			{
				sheet.State = sheet._isSheetOpen
					? (sheet.AllowedState == BottomSheetAllowedState.HalfExpanded ? BottomSheetState.HalfExpanded : BottomSheetState.FullExpanded)
					: BottomSheetState.Hidden;
				return;
			}

			if (newState == BottomSheetState.Hidden)
			{
				sheet._isHalfExpanded = (sheet.AllowedState != BottomSheetAllowedState.FullExpanded);
				if (sheet._isSheetOpen)
				{
					sheet._isSheetOpen = false;
					sheet.Close();
				}
			}
			else if(newState == BottomSheetState.Collapsed)
			{
				if(sheet._isSheetOpen)
				{
					sheet._isHalfExpanded = true;
					sheet.Show();
				}
			}
			else
			{
				sheet._isHalfExpanded = newState == BottomSheetState.HalfExpanded;
				if (sheet._isSheetOpen)
				{
					sheet.Show();
				}
			}

			sheet.UpdateStateChanged((BottomSheetState)oldValue, newState);
		}

		/// <summary>
		/// Handles changes to the <see cref="HalfExpandedRatio"/> property of the bottom sheet.
		/// </summary>
		/// <param name="bindable">The bindable object (should be SfBottomSheet).</param>
		/// <param name="oldValue">The old value of the half-expanded ratio.</param>
		/// <param name="newValue">The new value of the half-expanded ratio.</param>
		static void OnHalfExpandedRatioPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfBottomSheet sheet)
			{
				if (!oldValue.Equals(newValue) && newValue is not null)
				{
					sheet.UpdateHalfExpandedRatioProperty((double)newValue);
				}
			}
		}

		/// <summary>
		/// Handles changes to the <see cref="FullExpandedRatio"/> property of the bottom sheet.
		/// </summary>
		/// <param name="bindable">The bindable object (should be SfBottomSheet).</param>
		/// <param name="oldValue">The old value of the full-expanded ratio.</param>
		/// <param name="newValue">The new value of the full-expanded ratio.</param>
		static void OnFullExpandedRatioPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfBottomSheet sheet)
			{
				if (!oldValue.Equals(newValue) && newValue is not null)
				{
					sheet.UpdateFullExpandedRatioProperty((double)newValue);
				}
			}
		}

		/// <summary>
		/// Handles changes to the <see cref="CollapsedHeight"/> property of the bottom sheet.
		/// </summary>
		/// <param name="bindable">The bindable object (should be SfBottomSheet).</param>
		/// <param name="oldValue">The old value of the collapsed height.</param>
		/// <param name="newValue">The new value of the collapsed height.</param>
		static void OnCollapsedHeightPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfBottomSheet sheet)
			{
				if (!oldValue.Equals(newValue) && newValue is not null)
				{
					sheet.UpdateCollapsedHeightProperty((double)newValue);
				}
			}
		}

		/// <summary>
		/// Handles changes to the <see cref="BottomSheetContentWidth"/> property of the bottom sheet.
		/// </summary>
		/// <param name="bindable">The bindable object (should be SfBottomSheet).</param>
		/// <param name="oldValue">The old value of the content width.</param>
		/// <param name="newValue">The new value of the content width.</param>
		static void OnBottomSheetContentWidthPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfBottomSheet sheet)
			{
				if (!oldValue.Equals(newValue) && newValue is not null)
				{
					sheet.UpdateContentWidthProperty((double)newValue);
				}
			}
		}

		/// <summary>
		/// Handles changes to the <see cref="ContentWidthMode"/> property of the bottom sheet.
		/// </summary>
		/// <param name="bindable">The bindable object (should be SfBottomSheet).</param>
		/// <param name="oldValue">The old value of the content width mode.</param>
		/// <param name="newValue">The new value of the content width mode.</param>
		static void OnContentWidthModePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfBottomSheet sheet)
			{
				if (!oldValue.Equals(newValue) && newValue is not null)
				{
					sheet.UpdateContentWidthModeProperty((BottomSheetContentWidthMode)newValue);
				}
			}
		}

		/// <summary>
		/// Handles changes to the <see cref="AllowedState"/> property of the bottom sheet.
		/// </summary>
		/// <param name="bindable">The bindable object (should be SfBottomSheet).</param>
		/// <param name="oldValue">The old value of the allowed state.</param>
		/// <param name="newValue">The new value of the allowed state.</param>
		static void OnAllowedStatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfBottomSheet sheet)
			{
				sheet.UpdateState();
			}
		}

		/// <summary>
		/// Handles changes to the <see cref="Background"/> property of the bottom sheet.
		/// </summary>
		/// <param name="bindable">The bindable object (should be SfBottomSheet).</param>
		/// <param name="oldValue">The old value of the background.</param>
		/// <param name="newValue">The new value of the background.</param>
		static void OnBackgroundPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfBottomSheet sheet)
			{
				sheet.UpdateBottomSheetBackground((Brush)newValue);
			}
		}

		/// <summary>
		/// Handles changes to the <see cref="CornerRadius"/> property of the bottom sheet.
		/// </summary>
		/// <param name="bindable">The bindable object (should be SfBottomSheet).</param>
		/// <param name="oldValue">The old value of the corner radius.</param>
		/// <param name="newValue">The new value of the corner radius.</param>
		static void OnCornerRadiusPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfBottomSheet sheet)
			{
				sheet.UpdateCornerRadius((CornerRadius)newValue);
			}
		}

		/// <summary>
		/// Handles changes to the <see cref="ContentPadding"/> property of the bottom sheet.
		/// </summary>
		/// <param name="bindable">The bindable object (should be SfBottomSheet).</param>
		/// <param name="oldValue">The old value of the content padding.</param>
		/// <param name="newValue">The new value of the content padding.</param>
		static void OnContentPaddingPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfBottomSheet sheet)
			{
				sheet.UpdatePadding((Thickness)newValue);
			}
		}

		/// <summary>
		/// Handles changes to the <see cref="GrabberBackground"/> property of the bottom sheet.
		/// </summary>
		/// <param name="bindable">The bindable object (should be SfBottomSheet).</param>
		/// <param name="oldValue">The old value of the grabber background.</param>
		/// <param name="newValue">The new value of the grabber background.</param>
		static void OnGrabberBackgroundPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfBottomSheet sheet)
			{
				sheet.UpdateGrabberBackground((Brush)newValue);
			}
		}

		/// <summary>
		/// Handles changes to the <see cref="OverlayBackgroundColor"/> property of the bottom sheet.
		/// </summary>
		/// <param name="bindable">The bindable object (should be SfBottomSheet).</param>
		/// <param name="oldValue">The old value of the overlay background color.</param>
		/// <param name="newValue">The new value of the overlay background color.</param>
		static void OnOverlayBackgroundColorChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfBottomSheet sheet && sheet._overlayGrid is not null)
			{
				sheet._overlayGrid.Background = sheet.OverlayBackgroundColor;
			}
		}

		/// <summary>
		/// Handles changes to the <see cref="GrabberHeight"/> property of the bottom sheet.
		/// </summary>
		/// <param name="bindable">The bindable object (should be SfBottomSheet).</param>
		/// <param name="oldValue">The old value of the grabber height.</param>
		/// <param name="newValue">The new value of the grabber height.</param>
		static void OnGrabberHeightPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfBottomSheet sheet)
			{
				if (!oldValue.Equals(newValue) && newValue is not null)
				{
					sheet.UpdateGrabberHeightProperty((double)newValue);
				}
			}
		}

		/// <summary>
		/// Handles changes to the <see cref="GrabberWidth"/> property of the bottom sheet.
		/// </summary>
		/// <param name="bindable">The bindable object (should be SfBottomSheet).</param>
		/// <param name="oldValue">The old value of the grabber width.</param>
		/// <param name="newValue">The new value of the grabber width.</param>
		static void OnGrabberWidthPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfBottomSheet sheet)
			{
				if (!oldValue.Equals(newValue) && newValue is not null)
				{
					sheet.UpdateGrabberWidthProperty((double)newValue);
				}
			}
		}

		/// <summary>
		/// Handles changes to the <see cref="GrabberCornerRadius"/> property of the bottom sheet.
		/// </summary>
		/// <param name="bindable">The bindable object (should be SfBottomSheet).</param>
		/// <param name="oldValue">The old value of the grabber corner radius.</param>
		/// <param name="newValue">The new value of the grabber corner radius.</param>
		static void OnGrabberCornerRadiusPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfBottomSheet sheet)
			{
				sheet.UpdateGrabberCornerRadius((CornerRadius)newValue);
			}
		}

		#endregion

		#region Interface Implementation

		/// <summary>
		/// Returns the theme dictionary for the SfBottomSheet control.
		/// </summary>
		/// <returns>A ResourceDictionary containing the theme styles for the control.</returns>
		public ResourceDictionary GetThemeDictionary()
		{
		    return new SfBottomSheetStyle();
		}

		/// <summary>
		/// Handles changes to the control-specific theme.
		/// </summary>
		/// <param name="oldTheme">The previous theme.</param>
		/// <param name="newTheme">The new theme.</param>
		public void OnControlThemeChanged(string oldTheme, string newTheme)
		{
		    // TODO: Implement control-specific theme change logic
		}
		
		/// <summary>
		/// Handles changes to the common theme.
		/// </summary>
		/// <param name="oldTheme">The previous theme.</param>
		/// <param name="newTheme">The new theme.</param>
		public void OnCommonThemeChanged(string oldTheme, string newTheme)
		{
		    // TODO: Implement common theme change logic
		}

		#endregion

		#region Events

		/// <summary>
		/// Invoke the event when the state of <see cref=" SfBottomSheet "/> is changed.
		/// </summary>
		public event EventHandler<StateChangedEventArgs>? StateChanged;

		#endregion
	}
}

