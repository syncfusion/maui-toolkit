using System.ComponentModel;
using System.Runtime.CompilerServices;
using Syncfusion.Maui.Toolkit.Accordion;
using Syncfusion.Maui.Toolkit.EffectsView;
using Syncfusion.Maui.Toolkit.Themes;

namespace Syncfusion.Maui.Toolkit.Expander
{
    /// <summary>
    /// Represents a collapsible container that can be expanded or collapsed to show or hide its content.
    /// </summary>
    public partial class SfExpander : SfView, IParentThemeElement
    {
		#region Fields

		/// <summary>
		/// Gets a value indicating whether the flow direction is RTL or not.
		/// </summary>
		internal bool _isRTL = false;

		/// <summary>
		/// instance for the effectsView.
		/// </summary>
		internal SfEffectsView? _effectsView;

		/// <summary>
		/// Boolean value indicating whether the ripple animation is in progress.
		/// </summary>
		internal bool _isRippleAnimationProgress;

		/// <summary>  
		/// Represents the measured size of the header after layout calculations.  
		/// </summary>  
		Size _headerMeasuredSize = new(0, 0);

		/// <summary>  
		/// Represents the measured size of the content after layout calculations.  
		/// </summary>  
		Size _contentMeasuredSize = new(0, 0);

		/// <summary>
		/// Represents the Expander control height.
		/// </summary>
		double _expanderHeight;

		/// <summary>
		/// Backing field for the Expander SemanticDescription.
		/// </summary>
		string? _semanticDescription;

		/// <summary>
		/// Backing field for the <see cref="HeaderView"/>.
		/// </summary>
		ExpanderHeader? _headerView;

		/// <summary>
		/// Backing field for the <see cref="ContentView"/>.
		/// </summary>
		ExpanderContent? _contentView;

		/// <summary>
		/// Boolean value indicating whether the expand and collapse animation is in progress.
		/// </summary>
		bool _isAnimationInProgress;

		/// <summary>
		/// Backing field for the ContentHeightOnAnimation.
		/// </summary>
		double _contentheightOnAnimation = 0;

		/// <summary>
		/// Expand and collapse animator.
		/// </summary>
		SfAnimation? _expanderAnimation;

		/// <summary>
		/// Handles transitioning to the normal state.
		/// </summary>
		bool _isSelected = false;

		#endregion

		#region Bindable Properties

		/// <summary>
		/// Identifies the <see cref="Content"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="ContentProperty"/> property determines the content that will be displayed.
		/// </remarks>
		public static readonly BindableProperty ContentProperty =
            BindableProperty.Create(
				nameof(Content),
				typeof(View),
				typeof(SfExpander),
				null,
				BindingMode.Default,
				null,
				OnContentPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="HeaderIconPosition"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="HeaderIconPositionProperty"/> property determines the position of the header icon.
		/// </remarks>
		public static readonly BindableProperty HeaderIconPositionProperty =
            BindableProperty.Create(
				nameof(HeaderIconPosition),
				typeof(ExpanderIconPosition),
				typeof(SfExpander),
				ExpanderIconPosition.End,
				BindingMode.Default,
				null,
				OnHeaderIconPositionPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="Header"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="HeaderProperty"/> property specifies the header content that will be displayed.
		/// </remarks>
		public static readonly BindableProperty HeaderProperty =
            BindableProperty.Create(
				nameof(Header),
				typeof(View),
				typeof(SfExpander),
				null,
				BindingMode.Default,
				null,
				OnHeaderPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="IsExpanded"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="IsExpandedProperty"/> property determines whether the control is expanded or collapsed.
		/// </remarks>
		public static readonly BindableProperty IsExpandedProperty =
            BindableProperty.Create(
				nameof(IsExpanded),
				typeof(bool),
				typeof(SfExpander),
				false,
				BindingMode.TwoWay,
				null,
				OnIsExpandedPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="HeaderBackground"/>  bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="HeaderBackgroundProperty"/> property specifies the background appearance of the header.
		/// </remarks>
		public static readonly BindableProperty HeaderBackgroundProperty =
            BindableProperty.Create(
				nameof(HeaderBackground),
				typeof(Brush),
				typeof(SfExpander),
				new SolidColorBrush(Color.FromArgb("#00000000")),
				BindingMode.Default,
				null,
				OnHeaderBackgroundPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="HeaderIconColor"/>  bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="HeaderIconColorProperty"/> property specifies the color of the header icon.
		/// </remarks>
		public static readonly BindableProperty HeaderIconColorProperty =
           BindableProperty.Create(
			   nameof(HeaderIconColor),
			   typeof(Color),
			   typeof(SfExpander),
			   Color.FromArgb("#49454F"),
			   BindingMode.Default,
			   null,
			   OnHeaderIconColorPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="AnimationEasing"/>  bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="AnimationEasingProperty"/> property specifies the easing function to be applied during the animation of the control.
		/// </remarks>
		public static readonly BindableProperty AnimationEasingProperty =
            BindableProperty.Create(
				nameof(AnimationEasing),
				typeof(ExpanderAnimationEasing),
				typeof(SfExpander),
				ExpanderAnimationEasing.Linear,
				BindingMode.Default,
				null);

		/// <summary>
		/// Identifies the <see cref="AnimationDuration"/>  bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="AnimationDurationProperty"/> property specifies the duration of the animation in milliseconds.
		/// </remarks>
		public static readonly BindableProperty AnimationDurationProperty =
            BindableProperty.Create(
				nameof(AnimationDuration),
				typeof(double),
				typeof(SfExpander),
				300d,
				BindingMode.Default,
				null,
				OnAnimationDurationPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="HoverHeaderBackground"/>  bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="HoverHeaderBackgroundProperty"/> property specifies the background appearance of the header when the control is hovered over.
		/// </remarks>
		internal static readonly BindableProperty HoverHeaderBackgroundProperty =
            BindableProperty.Create(
				nameof(HoverHeaderBackground),
				typeof(Brush),
				typeof(SfExpander),
				new SolidColorBrush(Color.FromArgb("#141C1B1F")),
				BindingMode.Default,
				null,
				null);

		/// <summary>
		/// Identifies the <see cref="HoverIconColor"/>  bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="HoverIconColorProperty"/> property specifies the color of the icon when the header is hovered over.
		/// </remarks>
		internal static readonly BindableProperty HoverIconColorProperty =
            BindableProperty.Create(
				nameof(HoverIconColor),
				typeof(Color),
				typeof(SfExpander),
				Color.FromArgb("#1C1B1F"),
				BindingMode.Default,
				null,
				null);

		/// <summary>
		/// Identifies the <see cref="PressedIconColor"/>  bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="PressedIconColorProperty"/> property specifies the color of the icon when the header is pressed.
		/// </remarks>
		internal static readonly BindableProperty PressedIconColorProperty =
            BindableProperty.Create(
				nameof(PressedIconColor),
				typeof(Color),
				typeof(SfExpander),
				Color.FromArgb("#49454F"),
				BindingMode.Default,
				null,
				null);

		/// <summary>
		/// Identifies the <see cref="HeaderRippleBackground"/>  bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="HeaderRippleBackgroundProperty"/> property specifies the background color of the ripple effect when the header is pressed.
		/// </remarks>
		internal static readonly BindableProperty HeaderRippleBackgroundProperty =
            BindableProperty.Create(
				nameof(HeaderRippleBackground),
				typeof(Brush),
				typeof(SfExpander),
				new SolidColorBrush(Color.FromArgb("#1C1B1E")),
				BindingMode.Default,
				null,
				null);

		/// <summary>
		/// Identifies the <see cref="FocusedHeaderBackground"/>  bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="FocusedHeaderBackgroundProperty"/> property specifies the background appearance of the header when the control is focused.
		/// </remarks>
		internal static readonly BindableProperty FocusedHeaderBackgroundProperty =
            BindableProperty.Create(
				nameof(FocusedHeaderBackground),
				typeof(Brush),
				typeof(SfExpander),
				new SolidColorBrush(Color.FromArgb("#1F1C1B1F")),
				BindingMode.Default,
				null,
				null);

        /// <summary>
        /// Identifies the <see cref="FocusedIconColor"/>  bindable property.
        /// </summary>
        internal static readonly BindableProperty FocusedIconColorProperty =
            BindableProperty.Create(
				nameof(FocusedIconColor),
				typeof(Color),
				typeof(SfExpander),
				Color.FromArgb("#49454F"),
				BindingMode.Default,
				null,
				null);

		/// <summary>
		/// Identifies the <see cref="HeaderStroke"/>  bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="HeaderStrokeProperty"/> property specifies the stroke appearance of the header.
		/// </remarks>
		internal static readonly BindableProperty HeaderStrokeProperty =
            BindableProperty.Create(
				nameof(HeaderStroke),
				typeof(Color),
				typeof(SfExpander),
				Color.FromArgb("#CAC4D0"),
				BindingMode.Default,
				null,
				null);

		/// <summary>
		/// Identifies the <see cref="HeaderStrokeThickness"/>  bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="HeaderStrokeThicknessProperty"/> property specifies the thickness of the border (stroke) around the header.
		/// </remarks>
		internal static readonly BindableProperty HeaderStrokeThicknessProperty =
            BindableProperty.Create(
				nameof(HeaderStrokeThickness),
				typeof(double),
				typeof(SfExpander),
				1d,
				BindingMode.Default,
				null,
				null);

		/// <summary>
		/// Identifies the <see cref="FocusBorderColor"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="FocusBorderColorProperty"/> property specifies the color of the border when the control is focused.
		/// </remarks>
		internal static readonly BindableProperty FocusBorderColorProperty =
            BindableProperty.Create(
				nameof(FocusBorderColor),
				typeof(Color),
				typeof(SfExpander),
				Color.FromArgb("#000000"),
				BindingMode.Default,
				null);

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SfExpander"/> class.
        /// </summary>
        public SfExpander()
        {
            ThemeElement.InitializeThemeResources(this, "SfExpanderTheme");
            SetDynamicResource(HoverHeaderBackgroundProperty, "SfExpanderHoverHeaderBackground");
            SetDynamicResource(HoverIconColorProperty, "SfExpanderHoverHeaderIconColor");
            SetDynamicResource(PressedIconColorProperty, "SfExpanderPressedHeaderIconColor");
            SetDynamicResource(HeaderRippleBackgroundProperty, "SfExpanderHeaderRippleBackground");
            SetUp();
        }

        #endregion

		#region Properties

		/// <summary>
		/// Gets or sets the duration of the expand and collapse animations in milliseconds for the <see cref="SfExpander"/>.
		/// </summary>
		/// <value>
		/// It accepts <see cref="double"/>. The default value is 250 milliseconds.
		/// </value>
		/// <remarks>
		/// The <see cref="AnimationDuration"/> property allows you to customize the speed of the animation when the expander toggles between its expanded and collapsed states. 
		/// Adjusting this value can provide a faster or slower animation experience based on your application's needs.
		/// </remarks>
		/// <example>
		/// Here is an example of how to set the <see cref="AnimationDuration"/> property:
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <local:SfExpander x:Name="expander"
		///                   IsExpanded="True"
		///                   AnimationDuration="500">
		/// </local:SfExpander>
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// SfExpander expander = new SfExpander
		/// {
		///     IsExpanded="True"
		///     AnimationDuration = 500,    
		/// };
		/// ]]></code>
		/// </example>
		public double AnimationDuration
        {
            get { return (double)GetValue(AnimationDurationProperty); }
            set { SetValue(AnimationDurationProperty, value); }
		}

		/// <summary>
		/// Gets or sets the content to be displayed when the <see cref="SfExpander"/> is expanded.
		/// </summary>
		/// <value>
		/// It accepts <see cref="View"/>.The default value is null.
		/// </value>
		/// <remarks>
		/// The <see cref="Content"/> property specifies the content to be displayed inside the control, 
		/// such as text, images, or custom views.
		/// </remarks>
		/// <example>
		/// Here is an example of how to set the <see cref="Content"/> property in XAML and C#:
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <syncfusion:SfExpander.Content>
		///     <Grid Padding="18,8,0,18">
		///         <Label 
		///             CharacterSpacing="0.25" 
		///             FontFamily="Roboto-Regular"
		///             Text="11:03 AM, 15 January 2019" 
		///             FontSize="14" />                  
		///     </Grid>
		/// </syncfusion:SfExpander.Content>
		/// ]]></code>
		///
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var contentGrid = new Grid
		/// {
		///     Padding = new Thickness(18, 8, 0, 18)
		/// };
		///
		/// var label = new Label
		/// {
		///     CharacterSpacing = 0.25,
		///     FontFamily = "Roboto-Regular",
		///     Text = "11:03 AM, 15 January 2019",
		///     FontSize = 14
		/// };
		///
		/// contentGrid.Children.Add(label);
		///
		/// var expander = new SfExpander
		/// {
		///     Content = contentGrid
		/// };
		/// ]]></code>
		/// </example>
		/// <seealso cref="Header"/>
		public View? Content
        {
            get { return (View)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether the <see cref="SfExpander"/> control's content is expanded.
		/// </summary>
		/// <value>
		/// Accepts a <see cref="bool"/>. The default value is false.
		/// </value>
		/// <remarks>
		/// The IsExpanded property determines whether the content of the Expander control is displayed expanded or collapsed.
		/// When set to true, the content is displayed expanded;
		/// when set to false, the content is displayed collapsed.
		/// </remarks>
		/// <example>
		/// Here is an example demonstrating how to use the <see cref="SfExpander"/> with the IsExpanded property:
		///
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <syncfusion:SfExpander x:Name="expander"
		///                        IsExpanded="True">
		///     <syncfusion:SfExpander.Header>
		///         <Label Text="Tap to expand" FontSize="16" />
		///     </syncfusion:SfExpander.Header>
		///     <syncfusion:SfExpander.Content>
		///         <Label Text="This content is visible because IsExpanded is set to True."
		///                FontSize="14"
		///                TextColor="Black" />
		///     </syncfusion:SfExpander.Content>
		/// </syncfusion:SfExpander>
		/// ]]></code>
		///
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var expander = new SfExpander
		/// {
		///     IsExpanded = true,
		///     Header = new Label
		///     {
		///         Text = "Tap to expand",
		///         FontSize = 16
		///     },
		///     Content = new Label
		///     {
		///         Text = "This content is visible because IsExpanded is set to True.",
		///         FontSize = 14,
		///         TextColor = Colors.Black
		///     }
		/// };
		/// Content = new StackLayout
		/// {
		///     Children = { expander }
		/// };
		/// ]]></code>
		/// </example>
		public bool IsExpanded
        {
            get { return (bool)GetValue(IsExpandedProperty); }
            set { SetValue(IsExpandedProperty, value); }
		}

		/// <summary>
		/// Gets or sets the view that is displayed in the header of the <see cref="SfExpander"/>.
		/// </summary>
		/// <value>
		/// It accepts a <see cref="View"/>. The default value is false.
		/// </value>
		/// <remarks>
		/// Use this property to customize the header of the <see cref="SfExpander"/>. 
		/// You can set any view, such as a layout or a specific control, as the header.
		/// This provides flexibility in designing the UI to match the application's theme and functionality.
		/// Ensure that the view used for the header is appropriately sized to fit within the layout constraints of the <see cref="SfExpander"/>.
		/// </remarks>
		/// <example>
		/// Here is an example demonstrating how to use the <see cref="SfExpander"/> with the IsExpanded property:
		///
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <syncfusion:SfExpander x:Name="expander"
		///                        IsExpanded="True">
		///     <syncfusion:SfExpander.Header>
		///         <Label Text="Tap to expand" FontSize="16" />
		///     </syncfusion:SfExpander.Header>
		/// </syncfusion:SfExpander>
		/// ]]></code>
		///
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var expander = new SfExpander
		/// {
		///     IsExpanded = true,
		///     Header = new Label
		///     {
		///         Text = "Tap to expand",
		///         FontSize = 16
		///     },
		/// };
		/// ]]></code>
		/// </example>
		/// <seealso cref="Content"/>
		public View Header
        {
            get { return (View)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

		/// <summary>
		/// Gets or sets the <see cref="ExpanderIconPosition"/> of the header icon in the <see cref="SfExpander"/> control.
		/// </summary>
		/// <value>
		/// Accepts a <see cref="ExpanderIconPosition"/>. The default value is ExpanderIconPosition.End.
		/// </value>
		/// <remarks>
		/// The <see cref="HeaderIconPosition"/> property determines the position of the icon within the header,
		/// such as at the start or end of the header content.
		/// </remarks>
		/// <example>
		/// Here is an example demonstrating how to set the <see cref="HeaderIconPosition"/>:
		///
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <syncfusion:SfExpander HeaderIconPosition="Start"/>
		/// ]]></code>
		///
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var expander = new SfExpander
		/// {
		///     HeaderIconPosition = HeaderIconPosition.Start
		/// };
		/// ]]></code>
		/// </example>
		/// <seealso cref="HeaderBackground"/>
		public ExpanderIconPosition HeaderIconPosition
		{
			get { return (ExpanderIconPosition)GetValue(HeaderIconPositionProperty); }
			set { SetValue(HeaderIconPositionProperty, value); }
		}

		/// <summary>
		/// Gets or sets the <see cref="ExpanderAnimationEasing"/> function for an expand and collapse animation.
		/// </summary>
		/// <value>
		/// It accepts a <see cref="ExpanderAnimationEasing"/>.The default value is ExpanderAnimationEasing.Linear.
		/// </value>
		/// <remarks>
		/// Set this property to <see cref="ExpanderAnimationEasing.None"/> to disable the animation for <see cref="SfExpander"/>.
		/// </remarks>
		/// <example>
		/// Here is an example demonstrating how to set the <see cref="AnimationEasing"/> property:
		///
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <syncfusion:SfExpander AnimationEasing="SinIn"/>
		/// ]]></code>
		///
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var expander = new SfExpander
		/// {
		///     AnimationEasing = ExpanderAnimationEasing.SinIn
		/// };
		/// ]]></code>
		/// </example>
		/// <seealso cref="AnimationDuration"/>
		public ExpanderAnimationEasing AnimationEasing
        {
            get { return (ExpanderAnimationEasing)GetValue(AnimationEasingProperty); }
            set { SetValue(AnimationEasingProperty, value); }
        }

		/// <summary>
		/// Gets or sets the background color of the header area in the <see cref="SfExpander"/> control.
		/// </summary>
		/// <value>
		/// It accepts the <see cref="Brush"/>. The default value is Transparent.
		/// </value>
		/// <remarks>
		/// The <see cref="HeaderBackground"/> property specifies the background appearance of the header, allowing customization with solid colors, gradients, or images.
		/// </remarks>
		/// <example>
		/// Here is an example demonstrating how to set the <see cref="HeaderBackground"/> property:
		///
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		///  <syncfusion:SfExpander HeaderBackground = "Red"/>
		/// ]]></code>
		///
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// expander.HeaderBackground = "Red" />
		/// ]]></code>
		/// </example>
		/// <seealso cref="HeaderIconColor"/>
		public Brush HeaderBackground
        {
            get { return (Brush)GetValue(HeaderBackgroundProperty); }
            set { SetValue(HeaderBackgroundProperty, value); }
        }

		/// <summary>
		/// Gets or sets the color of the header icon in the <see cref="SfExpander"/> control.
		/// </summary>
		/// <value>
		/// It accepts the <see cref="Color"/>. The default value is <c>dark grayish-purple</c>.
		/// </value>
		/// <remarks>
		/// The <see cref="HeaderIconColor"/> property specifies the color of the icon displayed in the header, allowing customization to match the theme or design of the control.
		/// </remarks>
		/// <example>
		/// Here is an example demonstrating how to set the <see cref="HeaderIconColor"/> property:
		///
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		///  <syncfusion:SfExpander HeaderIconColor = "Red"/>
		/// ]]></code>
		///
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// expander.HeaderIconColor = "Red" />
		/// ]]></code>
		/// </example>
		/// <seealso cref="HeaderIconPosition"/>
		/// <seealso cref="HeaderBackground"/>
		public Color HeaderIconColor
        {
            get { return (Color)GetValue(HeaderIconColorProperty); }
            set { SetValue(HeaderIconColorProperty, value); }
        }

        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets or sets the height for content view during expand and collapse animation.
        /// </summary>
        internal double ContentHeightOnAnimation
        {
            get
            {
                return _contentheightOnAnimation;
            }

            set
            {
                _contentheightOnAnimation = value;
                InvalidateForceLayout();
            }
        }

        /// <summary>
        /// Gets or sets Semantic Description for this object
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
		/// Gets or sets a value indicating whether the view has been loaded.
		/// </summary>
		internal bool IsViewLoaded
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the view which is loaded in the header of the <see cref="SfExpander"/>.
        /// </summary>
        internal ExpanderHeader? HeaderView
        {
            get 
			{ 
				return _headerView; 
			}

            set 
			{ 
				_headerView = value; 
			}
        }

        /// <summary>
        /// Gets or sets the view which is loaded in the content of <see cref="SfExpander"/>.
        /// </summary>
        internal ExpanderContent? ContentView
        {
            get 
			{ 
				return _contentView; 
			}

            set 
			{ 
				_contentView = value; 
			}
        }

		/// <summary>  
		/// Gets or sets a value indicating whether the <see cref="SfExpander"/> should notify the parent view of bounds changes for proper layout updates.  
		/// </summary>  
		internal bool NeedtoAutoLayout { get; set; }

        /// <summary>
        /// Gets or sets the header hover background for the expander.
        /// </summary>
        internal Brush HoverHeaderBackground
        {
            get 
			{ 
				return (Brush)GetValue(HoverHeaderBackgroundProperty); 
			}

            set 
			{ 
				SetValue(HoverHeaderBackgroundProperty, value); 
			}
        }

        /// <summary>
        /// Gets or sets the header ripple background for the expander.
        /// </summary>
        internal Brush HeaderRippleBackground
		{
			get
			{
				return (Brush)GetValue(HeaderRippleBackgroundProperty);
			}
			set
			{
				SetValue(HeaderRippleBackgroundProperty, value);
			}
		}

        /// <summary>
        /// Gets or sets the icon hover background color for the expander.
        /// </summary>
        internal Color HoverIconColor
        {
            get 
			{ 
				return (Color)GetValue(HoverIconColorProperty); 
			}

            set 
			{ 
				SetValue(HoverIconColorProperty, value); 
			}
        }

        /// <summary>
        /// Gets or sets the icon pressed background color for the expander.
        /// </summary>
        internal Color PressedIconColor
        {
            get 
			{ 
				return (Color)GetValue(PressedIconColorProperty); 
			}

            set 
			{ 
				SetValue(PressedIconColorProperty, value); 
			}
        }

        /// <summary>
        /// Gets or sets the header border color for the accordion item.
        /// </summary>
        internal Color HeaderStroke
        {
            get 
			{ 
				return (Color)GetValue(HeaderStrokeProperty); 
			}

            set 
			{ 
				SetValue(HeaderStrokeProperty, value); 
			}
        }

        /// <summary>
        /// Gets or sets the selection background color for the accordion item.
        /// </summary>
        internal Brush FocusedHeaderBackground
        {
            get 
			{ 
				return (Brush)GetValue(FocusedHeaderBackgroundProperty);
			}

            set 
			{ 
				SetValue(FocusedHeaderBackgroundProperty, value); 
			}
        }

        /// <summary>
        /// Gets or sets the icon focused background color for the accordion item.
        /// </summary>
        internal Color FocusedIconColor
        {
            get 
			{ 
				return (Color)GetValue(FocusedIconColorProperty); 
			}

            set 
			{ 
				SetValue(FocusedIconColorProperty, value); 
			}
        }

        /// <summary>
        /// Gets or sets the header stroke thickness for the accordion item.
        /// </summary>
        internal double HeaderStrokeThickness
        {
            get 
			{ 
				return (double)GetValue(HeaderStrokeThicknessProperty); 
			}

            set 
			{ 
				SetValue(HeaderStrokeThicknessProperty, value); 
			}
        }

        /// <summary>
        /// Gets or sets the border color for the current focused item.
        /// </summary>
        internal Color FocusBorderColor
        {
            get 
			{ 
				return (Color)GetValue(FocusBorderColorProperty); 
			}

            set 
			{ 
				SetValue(FocusBorderColorProperty, value); 
			}
        }

        /// <summary>
        /// Gets or sets a value indicating whether an item is selected or not.
        /// </summary>
        internal bool IsSelected
        {
            get
            {
                return _isSelected;
            }

            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }

		#endregion

		#region Internal Methods

		/// <summary>
		/// Method to initiate the layout calls.
		/// </summary>
		internal void InvalidateForceLayout()
		{
			InvalidateMeasure();
		}

		#endregion

		#region Private Methods

		/// <summary>  
		/// Initializes the content of the expander view, setting up the necessary elements and layout.  
		/// </summary>  
		void InitializeExpanderViewContent()
		{
			if (_effectsView != null)
			{
				this.AddChildView(_effectsView, 0);
			}

			this.AddChildView(HeaderView, 0);
			this.AddChildView(ContentView, 1);
			HeaderView?.UpdateIconView();
			HeaderView?.InvalidateDrawable();
			WireMeasureInvalidate();
			HeaderView?.UpdateChildViews();
			ContentView?.AddChildView(Content);
			UpdateContentViewLayoutAndVisibility();
		}

		/// <summary>  
		/// Creates the view for rendering effects, such as visual or animation effects, on the element.  
		/// </summary>  
		SfEffectsView CreateEffectsView()
		{
			SfEffectsView effectsView = new()
			{
				RippleAnimationDuration = 300,
				InputTransparent = true
			};

			effectsView.AnimationCompleted += OnEffectsViewAnimationCompleted;
			return effectsView;
		}

		/// <summary>
		/// Triggers after the effects animation has been completed.
		/// </summary>
		void OnEffectsViewAnimationCompleted(object? sender, EventArgs e)
		{
			_isRippleAnimationProgress = false;
		}

		/// <summary>  
		/// Initiates the process to invalidate the measure and trigger a re-measure of the element.  
		/// </summary>  
		void WireMeasureInvalidate()
		{
			if (Header != null)
			{
				Header.MeasureInvalidated += OnHeaderMeasureInvalidated;
			}

			if (Content != null)
			{
				Content.MeasureInvalidated += OnContentMeasureInvalidated;
			}
		}

		/// <summary>  
		/// Triggered when the content measurement becomes invalid and needs to be recalculated.  
		/// </summary>  
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void OnContentMeasureInvalidated(object? sender, EventArgs e)
		{
			InvalidateForceLayout();
		}

		/// <summary>  
		/// Triggered when the header measurement becomes invalid and needs to be recalculated.  
		/// </summary>  
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void OnHeaderMeasureInvalidated(object? sender, EventArgs e)
		{
			InvalidateForceLayout();
		}

		/// <summary>  
		/// Calculates the height automatically based on the content and header size.  
		/// </summary>  
		/// <param name="width"></param>
		void CalculateAutoHeight(double width = 0)
		{
			CalculateHeaderAutoHeight(width);
			CalculateContentAutoHeight(width);
		}

		/// <summary>  
		/// Calculates the height of the header automatically based on its size.  
		/// </summary>  
		/// <param name="width"></param>
		void CalculateHeaderAutoHeight(double width)
		{
			var headerView = HeaderView;
			if (headerView == null)
			{
				return;
			}

			width = width > 0 ? width : Width;
			bool hasExpanderIcon = HeaderIconPosition != ExpanderIconPosition.None && headerView.IconView != null;
			int iconViewWidth = hasExpanderIcon ? headerView._iconViewSize : 0;
			if (Header != null)
			{
				_headerMeasuredSize = Header.Measure(width - iconViewWidth, double.PositiveInfinity);
			}
		}

		/// <summary>  
		/// Calculates the height of the content automatically based on its size.  
		/// </summary>  
		/// <param name="width"></param>
		void CalculateContentAutoHeight(double width)
		{
			width = width > 0 ? width : Width;
			if (IsExpanded && Content != null)
			{
				_contentMeasuredSize = Content.Measure(width, double.PositiveInfinity);
			}
			else
			{
				_contentMeasuredSize = new Size(0, 0);
			}
		}

		/// <summary>
		/// Gets the ContentView of the expander.
		/// </summary>
		ExpanderContent GetContentView()
		{
			return new ExpanderContent() { Expander = this };
		}

		/// <summary>
		/// Gets the HeaderView of the expander.
		/// </summary>
		ExpanderHeader GetHeaderView()
		{
			return new ExpanderHeader() { Expander = this };
		}

		///<summary>
		/// InitializeDefaultValues assigns default values to object properties
		///</summary>
		void InitializeDefaultValues()
		{
			HeaderView = GetHeaderView();
			ContentView = GetContentView();
			_effectsView = CreateEffectsView();
			_isAnimationInProgress = false;
			BackgroundColor = Colors.Transparent;
			_expanderAnimation = new SfAnimation(
				this,
				(value) => ProgressAnimation(value),
				AnimationCompleted);
		}

		/// <summary>
		/// Sets up default values and attaches event handlers.
		/// </summary>
		void SetUp()
		{
			InitializeDefaultValues();
			Loaded += OnExpanderLoaded;
		}

		/// <summary>
		/// Occurs when expander is loaded.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void OnExpanderLoaded(object? sender, EventArgs e)
		{
			if (!IsViewLoaded)
			{
				InitializeExpanderViewContent();
				IsViewLoaded = true;
			}
		}

		/// <summary>
		/// Updates the layout and visibility of the content view.
		/// </summary>
		void UpdateContentViewLayoutAndVisibility()
		{
			if (!IsViewLoaded || Content == null || ContentView == null)
			{
				return;
			}

			CalculateAutoHeight();

			// SfExpander does not expand or collapse when batterySaver mode is enabled
#if ANDROID
            // SfAccordion height not updated properly when the Animator duration scale settings is off.
            if (Extensions.AnimatorDurationScaleIsOff())
            {
                InvalidateForceLayout();
                return;
            }
#if !NET8_0_OR_GREATER
            if (Battery.Default.EnergySaverStatus == EnergySaverStatus.On)
            {
                return;
            }
#endif
#endif
			ApplyAnimation();
		}

		/// <summary>
		/// Applies an animation to a specified element.
		/// </summary>
		void ApplyAnimation()
		{
			double animationDuration = AnimationDuration / 1000;
			Easing easingValue = GetEasing(AnimationEasing);
			if (easingValue == Easing.Default)
			{
				animationDuration = 0;
				easingValue = Easing.Linear;
			}

			_isAnimationInProgress = true;
			if (_expanderAnimation != null && Content != null)
			{
				double startHeight = IsExpanded ? 1 : Content.DesiredSize.Height;
				double endHeight = IsExpanded ? Content.DesiredSize.Height : 0;
				_expanderAnimation.Start = startHeight;
				_expanderAnimation.End = endHeight;
				_expanderAnimation.Duration = animationDuration;
				_expanderAnimation.Easing = easingValue;
				_expanderAnimation.Reset();
				_expanderAnimation.Forward();
			}
		}

		/// <summary>
		/// Represents an animation for indicating progress, often used in user interfaces.
		/// </summary>		
		void ProgressAnimation(double value)
		{
			ContentHeightOnAnimation = value;
		}

		/// <summary>
		/// Retrieves the easing function associated with the specified easing type.
		/// </summary>
		/// <param name="easing"></param>
		/// <returns></returns>
		static Easing GetEasing(ExpanderAnimationEasing easing)
		{
			Easing easingValue = easing switch
			{
				ExpanderAnimationEasing.Linear => Easing.Linear,
				ExpanderAnimationEasing.SinIn => Easing.SinIn,
				ExpanderAnimationEasing.SinOut => Easing.SinOut,
				ExpanderAnimationEasing.SinInOut => Easing.SinInOut,
				_ => Easing.Default,
			};

			return easingValue;
		}

		/// <summary>
		/// Occurs when content is changed
		/// </summary>
		/// <param name="oldvalue"></param>
		/// <param name="newvalue"></param>
		void OnContentChanged(View? oldvalue, View? newvalue)
		{
			if (!IsViewLoaded || ContentView == null)
			{
				return;
			}

			if (oldvalue != null)
			{
				ContentView.RemoveChildrenInView(oldvalue);
			}

			ContentView.AddChildView(newvalue);
			UpdateContentViewLayoutAndVisibility();
		}

		/// <summary>
		/// Occurs when IsExpanded is changed
		/// </summary>
		/// <param name="oldvalue"></param>
		/// <param name="newvalue"></param>
		void OnIsExpandedChanged(bool oldvalue, bool newvalue)
		{
			if (!IsViewLoaded || HeaderView == null || ContentView == null || Content == null)
			{
				return;
			}

			HeaderView.UpdateIconViewDirection(newvalue);

			// SfExpander does not works properly when loaded inside ListView.
			if (AnimationDuration <= 0)
			{
				InvalidateForceLayout();
				return;
			}

			UpdateContentViewLayoutAndVisibility();

#if !NET8_0_OR_GREATER
            // SfExpander does not expand or collapse when batterySaver mode is enabled
            if (DeviceInfo.Platform == DevicePlatform.Android && Battery.Default.EnergySaverStatus == EnergySaverStatus.On)
            {
                InvalidateForceLayout();
            }
#endif
		}

		/// <summary>
		/// Occurs when header is changed
		/// </summary>
		/// <param name="oldvalue"></param>
		/// <param name="newvalue"></param>
		void OnHeaderChanged(View? oldvalue, View? newvalue)
		{
			if (!IsViewLoaded || HeaderView == null)
			{
				return;
			}

			HeaderView.UpdateChildViews();
			InvalidateForceLayout();
		}

		/// <summary>
		/// Occurs when HeaderIconPosition is changed
		/// </summary>
		/// <param name="oldvalue"></param>
		/// <param name="newvalue"></param>
		void OnHeaderIconPositionChanged(ExpanderIconPosition oldvalue, ExpanderIconPosition newvalue)
		{
			if (!IsViewLoaded || HeaderView == null)
			{
				return;
			}

			HeaderView.UpdateChildViews();
		}

		/// <summary>
		/// Occurs when icon color is changed
		/// </summary>
		/// <param name="oldvalue"></param>
		/// <param name="newvalue"></param>
		void OnIconColorChanged(Color oldvalue, Color newvalue)
		{
			if (!IsViewLoaded || HeaderView == null || HeaderView.IconView == null)
			{
				return;
			}
				
			HeaderView.UpdateIconColor(newvalue);
		}

		/// <summary>
		/// Sets the flow direction value to the expander when it is applied to its parent.
		/// </summary>
		void SetRTL()
		{
			if (((this as IVisualElementController).EffectiveFlowDirection & EffectiveFlowDirection.RightToLeft) == EffectiveFlowDirection.RightToLeft)
			{
				_isRTL = true;
			}
			else
			{
				_isRTL = false;
			}
		}

		/// <summary>
		/// Occurs when UpdateVisualState property is changed
		/// </summary>
		/// <param name="expander"></param>
		static void UpdateVisualState(SfExpander expander)
		{
			var isExpanded = expander.IsExpanded ? "Expanded" : "Collapsed";
			VisualStateManager.GoToState(expander, isExpanded);
		}

		#endregion

		#region Protected Internal Methods

		/// <summary>
		/// Occurs when expanding operation is performed
		/// </summary>
		protected internal virtual void RaiseExpandingEvent()
        {
			if (_isAnimationInProgress)
			{
				return;
			}

            bool canExpand = true;
            if (Expanding != null)
            {
                var args = new ExpandingAndCollapsingEventArgs();
                Expanding(this, args);
                canExpand = !args.Cancel;
            }

            if (canExpand)
            {
                IsExpanded = true;
            }
        }

        /// <summary>
        /// Occurs when expanding operation completes
        /// </summary>
        protected internal virtual void RaiseExpandedEvent()
        {
            if (Expanded != null)
            {
                var args = new ExpandedAndCollapsedEventArgs();
                Expanded(this, args);
            }
        }

        /// <summary>
        /// Occurs when collpasing operation starts
        /// </summary>
        protected internal virtual void RaiseCollapsingEvent()
        {
			if (_isAnimationInProgress)
			{
				return;
			}

            bool canCollapse = true;
            if (Collapsing != null)
            {
                var args = new ExpandingAndCollapsingEventArgs();
                Collapsing(this, args);
                canCollapse = !args.Cancel;
            }

            if (canCollapse)
            {
                IsExpanded = false;
            }
        }

        /// <summary>
        /// Occurs when collapsing operation completes
        /// </summary>
        protected internal virtual void RaiseCollapsedEvent()
        {
            if (Collapsed != null)
            {
                var args = new ExpandedAndCollapsedEventArgs();
                Collapsed(this, args);
            }
        }

		#endregion

		#region Override Methods

		/// <summary>
		/// Arrange the child views in Expander.
		/// </summary>
		/// <param name="bounds">Bounds of Expander.</param>
		/// <returns>Returns the required size to arrange Expander.</returns>
		protected override Size ArrangeContent(Rect bounds)
		{
			var viewwidth = Width > 0 ? Width : bounds.Right - bounds.Left;
			var headerheight = _headerMeasuredSize.Height;

			if (Header != null)
			{
				if (_effectsView is IView view)
				{
					view.Arrange(new Rect(0, 0, viewwidth, headerheight));
				}

				if (HeaderView is IView headerView)
				{
					headerView.Arrange(new Rect(0, 0, viewwidth, headerheight));
				}
			}

			if (ContentView is IView contentView)
			{
				if (!_isAnimationInProgress)
				{
					var contentviewtop = IsExpanded ? _headerMeasuredSize.Height : 0;
					contentView.Arrange(new Rect(0, contentviewtop, viewwidth, _contentMeasuredSize.Height));
				}
				else
				{
					var contentviewtop = _headerMeasuredSize.Height;
					_contentheightOnAnimation = double.IsNaN(_contentheightOnAnimation) || _contentheightOnAnimation < 0 ? 0 : _contentheightOnAnimation;
					contentView.Arrange(new Rect(0, contentviewtop, viewwidth, ContentHeightOnAnimation));
				}
			}

			return new Size(viewwidth, _expanderHeight);
		}

		/// <summary>
		/// Measure the child views in Expander.
		/// </summary>
		/// <param name="widthConstraint">Width of Expander.</param>
		/// <param name="heightConstraint">Height of Expander.</param>
		/// <returns>Returns measured size of Expander.</returns>
		protected override Size MeasureContent(double widthConstraint, double heightConstraint)
		{
			if (!_isAnimationInProgress)
			{
				if (Header != null)
				{
					CalculateHeaderAutoHeight(widthConstraint);
				}
				else if (IsViewLoaded)
				{
					_headerMeasuredSize = new Size(0, 0);
				}

				if (Content != null)
				{
					if (IsExpanded)
					{
						CalculateContentAutoHeight(widthConstraint);
					}
					else
					{
						_contentMeasuredSize = new Size(0, 0);
					}
				}
				else if (IsViewLoaded)
				{
					_contentMeasuredSize = new Size(0, 0);
				}
			}

			if (HeaderView is IView headerView)
			{
				headerView.Measure(widthConstraint, _headerMeasuredSize.Height);
			}

			if (ContentView is IView contentView)
			{
				if (_isAnimationInProgress)
				{
					_contentheightOnAnimation = double.IsNaN(_contentheightOnAnimation) || _contentheightOnAnimation < 0 ? 0 : _contentheightOnAnimation;
					_contentMeasuredSize = contentView.Measure(widthConstraint, ContentHeightOnAnimation);
				}
				else
				{
					_contentMeasuredSize = contentView.Measure(widthConstraint, _contentMeasuredSize.Height);
				}
			}

			double width = double.IsFinite(widthConstraint) ? widthConstraint : 0;
			// To update width when loaded expander inside HorizontalStackLayout and AbsoluteLayout.
			if (width == 0)
			{
				var scaledScreenSize =
#if WINDOWS
				new Size(300, 300);
#else
				new Size(DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density, DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density);
#endif
				double scaledWidth = Math.Min(scaledScreenSize.Width, scaledScreenSize.Height);
				width = scaledWidth;
			}

			_expanderHeight = _headerMeasuredSize.Height + _contentMeasuredSize.Height + (_headerMeasuredSize.Height > 0 ? Padding.Bottom : 0);
			return new Size(width, _expanderHeight);
		}


		/// <summary>
		/// Need to handle the run time changes of <see cref="PropertyChangedEventArgs"/> of <see cref="SfExpander"/>.
		/// </summary>
		/// <param name="propertyName">Represents the property changed event arguments.</param>
		protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
		{
			switch (propertyName)
			{
				case nameof(IsEnabled):
					if (HeaderView != null)
					{
						HeaderView.IsEnabled = IsEnabled;
					}
					
					if (ContentView != null)
					{
						ContentView.IsEnabled = IsEnabled;
					}
					break;
				case nameof(Parent):
				case nameof(FlowDirection):
					SetRTL();
					break;
				case nameof(IsSelected):
					var accordion = this as AccordionItemView;

					if (IsSelected && HeaderView != null)
					{
						HeaderView.IsMouseHover = false;

						if (accordion != null && accordion.AccordionItem.HasVisualStateGroups())
						{
							VisualStateManager.GoToState(this, "Focused");
							HeaderView.UpdateIconColor(HeaderIconColor);
						}
						else
						{
							HeaderView.UpdateIconColor(FocusedIconColor);
						}
					}
					else
					{
						if (accordion != null && accordion.AccordionItem.HasVisualStateGroups())
						{
							VisualStateManager.GoToState(this, "Normal");
							VisualStateManager.GoToState(this, IsExpanded ? "Expanded" : "Collapsed");
						}

						if (HeaderView == null)
						{
							return;
						}

						HeaderView.UpdateIconColor(HeaderIconColor);
					}
					HeaderView.InvalidateDrawable();
					break;
				case nameof(IsExpanded):
					if (HeaderView != null)
					{
						HeaderView.UpdateIconColor(HeaderIconColor);
						HeaderView.InvalidateDrawable();
					}
					break;
			}

			base.OnPropertyChanged(propertyName);
		}

		/// <summary>
		/// Triggers after the animation has been completed.
		/// </summary>
		protected virtual void AnimationCompleted()
        {
            _isAnimationInProgress = false;

            // Calling the Expanded/Collapsed event after animation is complete.
            if (IsExpanded)
            {
                RaiseExpandedEvent();
            }
            else
            {
                RaiseCollapsedEvent();
            }

            // Content does not get collapsed when expander is being collapsed in PCL view.
            if (Content != null && !IsExpanded)
            {
                Content.IsVisible = false;
            }
        }

        /// <summary>
        /// Helps to trigger the VisualState initially when View is loaded.
        /// </summary>
        protected override void ChangeVisualState()
        {
            base.ChangeVisualState();
            UpdateVisualState(this);
        }

        /// <summary>
        /// Raised when handler gets changed.
        /// </summary>
        protected override void OnHandlerChanged()
        {
            if (!IsViewLoaded)
            {
                InitializeExpanderViewContent();
                IsViewLoaded = true;
            }

            base.OnHandlerChanged();
        }

		#endregion

		#region PropertyChanged Methods

		/// <summary>
		/// Occurs when Content property is changed
		/// </summary>
		/// <param name="bindable"></param>
		/// <param name="oldValue"></param>
		/// <param name="newValue"></param>
		static void OnContentPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var content = newValue as View;
			var oldContent = oldValue as View;
			if (bindable is SfExpander expander)
			{
				if (content != null)
				{
					content.IsVisible = expander.IsExpanded;
				}
				else if (expander.IsViewLoaded)
				{
					expander.InvalidateForceLayout();
				}

				expander.OnContentChanged(oldContent, content);
			}
		}

		/// <summary>
		/// Occurs when Header property is changed
		/// </summary>
		/// <param name="bindable"></param>
		/// <param name="oldValue"></param>
		/// <param name="newValue"></param>
		static void OnHeaderPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
			var newHeader = newValue as View;
			var oldHeader = oldValue as View;

			// When the Content is changed at runtime, need to update its visibility based on IsExpanded property.
			if (bindable is SfExpander expander && expander.IsViewLoaded)
			{
				expander.OnHeaderChanged(newHeader, oldHeader);
			}
		}

		/// <summary>
		/// Occurs when HeaderIconPosition property is changed
		/// </summary>
		/// <param name="bindable"></param>
		/// <param name="oldValue"></param>
		/// <param name="newValue"></param>
		static void OnHeaderIconPositionPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
			// When the Content is changed at runtime, need to update its visibility based on IsExpanded property.
			if (bindable is SfExpander expander && expander.IsViewLoaded)
			{
				expander.OnHeaderIconPositionChanged((ExpanderIconPosition)newValue, (ExpanderIconPosition)oldValue);
			}
        }

		/// <summary>
		/// Occurs when IsExpanded property is changed
		/// </summary>
		/// <param name="bindable"></param>
		/// <param name="oldValue"></param>
		/// <param name="newValue"></param>
		static void OnIsExpandedPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfExpander expander)
			{
				if (expander.Content != null && (bool)newValue && !expander.Content.IsVisible)
				{
					expander.Content.IsVisible = true;
				}

				expander.OnIsExpandedChanged((bool)oldValue, (bool)newValue);
				UpdateVisualState(expander);
			}
		}

		/// <summary>
		/// Occurs when HeaderBackground property is changed
		/// </summary>
		/// <param name="bindable"></param>
		/// <param name="oldValue"></param>
		/// <param name="newValue"></param>
		static void OnHeaderBackgroundPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			// When the Content is changed at runtime, need to update its visibility based on IsExpanded property.
			if (bindable is SfExpander expander)
			{
				if (!expander.IsViewLoaded || expander.HeaderView == null)
				{
					return;
				}

				expander.HeaderView.InvalidateDrawable();
			}
		}

		/// <summary>
		/// Occurs when HeaderIconColor property is changed
		/// </summary>
		/// <param name="bindable"></param>
		/// <param name="oldValue"></param>
		/// <param name="newValue"></param>
		static void OnHeaderIconColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			// When the Content is changed at runtime, need to update its visibility based on IsExpanded property.
			if (bindable is SfExpander expander && expander.IsViewLoaded)
			{
				expander.OnIconColorChanged((Color)oldValue, (Color)newValue);
			}
		}

		/// <summary>
		/// Occurs when the AnimationDuration property is changed
		/// </summary>
		/// <param name="bindable"></param>
		/// <param name="oldValue"></param>
		/// <param name="newValue"></param>
		private static void OnAnimationDurationPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var expander = bindable as SfExpander;
   			if (expander != null && (double)newValue == 0 && expander._expanderAnimation != null && expander.IsViewLoaded)
			{
				var animation = expander._expanderAnimation;
				if (animation.AnimationManager != null)
				{
					// While setting Animation Duration as 0, the animation won't be stopped. So, removing it.
					// Since we are removing the animation, AnimationCompleted was not getting call. So, manually calling it.
					animation.AnimationManager.Remove(animation);
					expander.AnimationCompleted();
					expander.InvalidateForceLayout();
				}
			}
		}

		#endregion

		#region Interface Implementation

		/// <summary>
		/// Method invoke to get the initial set of color's from theme dictionary.
		/// </summary>
		/// <returns>Return the Expander theme dictionary.</returns>
		ResourceDictionary IParentThemeElement.GetThemeDictionary()
		{
			return new SfExpanderStyles();
		}

		/// <summary>
		/// Method invoke when theme changes are applied for internal properties.
		/// </summary>
		/// <param name="oldTheme">Old theme name.</param>
		/// <param name="newTheme">New theme name.</param>
		void IThemeElement.OnControlThemeChanged(string oldTheme, string newTheme)
		{
		}

		/// <summary>
		/// Method invoke when theme dictionary contains syncfusion theme.
		/// </summary>
		/// <param name="oldTheme">Old theme name.</param>
		/// <param name="newTheme">New theme name.</param>
		void IThemeElement.OnCommonThemeChanged(string oldTheme, string newTheme)
		{
		}

		#endregion

		#region Events

		/// <summary>
		/// Occurs when the <see cref="SfExpander"/> control is collapsed.
		/// </summary>
		/// <example>
		/// <code lang="C#"><![CDATA[
		/// expander.Collapsed += Expander_Collapsed;
		/// private void Expander_Collapsed(object sender, ExpandedAndCollapsedEventArgs e)
		/// {
		///     expander.HeaderBackground = Color.Aqua;
		/// }
		/// ]]></code>
		/// </example>
		/// <remarks> The event handler receives an argument of type <see cref="ExpandingAndCollapsingEventArgs"/> containing data related to this event.
		/// </remarks>
		/// <seealso cref="Expanding"/>
		/// <seealso cref="Expanded"/>
		/// <seealso cref="Collapsing"/>
		public event EventHandler<ExpandedAndCollapsedEventArgs>? Collapsed;

		/// <summary>
		/// Occurs when the <see cref="SfExpander"/> control is being collapsed.
		/// </summary>
		/// <example>
		/// <code lang="C#"><![CDATA[
		/// expander.Collapsing += Expander_Collapsing;
		/// private void Expander_Collapsing(object sender, ExpandingAndCollapsingEventArgs e)
		/// {
		///     // Below code cancels the collapsing operation of the SfExpander control.
		///     e.Cancel = true;
		/// }
		/// ]]></code>
		/// </example>
		/// <seealso cref="Expanding"/>
		/// <seealso cref="Expanded"/>
		/// <seealso cref="Collapsed"/>
		public event EventHandler<ExpandingAndCollapsingEventArgs>? Collapsing;

		/// <summary>
		/// Occurs when the <see cref="SfExpander"/> control is expanded.
		/// </summary>
		/// <example>
		/// <code lang="C#"><![CDATA[
		/// expander.Expanded += Expander_Expanded;
		/// private void Expander_Expanded(object sender, ExpandedAndCollapsedEventArgs e)
		/// {
		///     expander.HeaderBackground = Color.YellowGreen;
		/// }
		/// ]]></code>
		/// </example>
		/// <remarks>The event handler receives an argument of type <see cref="ExpandingAndCollapsingEventArgs"/> containing data related to this event.
		/// </remarks>
		/// <seealso cref="Expanding"/>
		/// <seealso cref="Collapsed"/>
		/// <seealso cref="Collapsing"/>
		public event EventHandler<ExpandedAndCollapsedEventArgs>? Expanded;

		/// <summary>
		/// Occurs when the <see cref="SfExpander"/> control is being expanded.
		/// </summary>
		/// <example>
		/// <code lang="C#"><![CDATA[
		/// expander.Expanding += Expander_Expanding;
		/// private void Expander_Expanding(object sender, ExpandingAndCollapsingEventArgs e)
		/// {
		///     // Below code cancels the expanding operation of the SfExpander control.
		///     e.Cancel = true;
		/// }
		/// ]]></code>
		/// </example>
		/// <seealso cref="Expanded"/>
		/// <seealso cref="Collapsed"/>
		/// <seealso cref="Collapsing"/>
		public event EventHandler<ExpandingAndCollapsingEventArgs>? Expanding;

		#endregion
	}
}
