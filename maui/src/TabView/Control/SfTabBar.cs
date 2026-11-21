using Microsoft.Maui.Controls.Shapes;
using Syncfusion.Maui.Toolkit.EffectsView;
using Syncfusion.Maui.Toolkit.Graphics.Internals;
using Syncfusion.Maui.Toolkit.Helper;
using Syncfusion.Maui.Toolkit.Internals;
using System.Collections;
using System.Collections.Specialized;
using CoreEventArgs = Syncfusion.Maui.Toolkit.TabView.TabSelectionChangedEventArgs;
using TouchEventArgs = Syncfusion.Maui.Toolkit.Internals.PointerEventArgs;

namespace Syncfusion.Maui.Toolkit.TabView
{
    
    /// <summary>
    /// Represents a tab bar control that manages the display and interaction of tab items.
    /// </summary>
    [ContentProperty(nameof(Items))]
	internal partial class SfTabBar : SfStackLayout, ITapGestureListener
	{
        #region Fields

        // Layout-related fields

        SfScrollView? _tabHeaderScrollView;
        SfGrid? _tabHeaderContentContainer;
        SfGrid? _tabHeaderParentContainer;
        SfBorder? _tabSelectionIndicator;
        RoundRectangle? _roundRectangle;
		TabBarPlacement _placement = TabBarPlacement.Top;
		SfHorizontalStackBase? _tabHeaderRow;
		SfVerticalStackBase? _tabHeaderColumn;
		SfStackBase? _tabItemsPanel;
		internal SfBorder? _tabHeaderParentBorder;
		RoundRectangle? _tabHeaderParenRoundRectangle;

		// Dimension and positioning fields

		double _previousTabX = 0d;
		double _previousTabY = 0d;
		double _selectedTabX = 0d;
		double _selectedTabY = 0d;
		double _currentIndicatorWidth = 0d;
		double _currentIndicatorHeight = 0d;
		double _tabHeaderImageSize = 32d;
		readonly double _arrowButtonSize = 32;
		double _removedItemWidth = 0;
		double _removedItemHeight = 0;
		SfStackBase? _from;
		SfStackBase? _to;

		// State-tracking fields

		double _previousIndicatorWidth = 0d;
		double _previousIndicatorHeight = 0d;
		int _previewSelectedIndex = 0;

        // Event-related fields

        readonly CoreEventArgs? _tabSelectionChangedEventArgs;
        readonly TabItemTappedEventArgs? _tabItemTappedEventArgs;
        SelectionChangingEventArgs? _selectionChangingEventArgs;

        // Navigation controls

        ArrowIcon? _forwardArrow;
        ArrowIcon? _backwardArrow;

		// Center button fields

		/// <summary>
		/// Represents a placeholder item that reserves space equal to the center button's width.
		/// </summary>
		SfGrid? _centerButtonViewPlaceholder;
		bool _isInitialLoading = true;
        bool _isInitialWidthSet;
		List<double> _tabItemTemplateWidth = new();
#if ANDROID
		bool _isItemRemovedWithCenterButton = false;
		bool _isTabItemPropertyChanged = false;
#endif

#if IOS
        Point _touchDownPoint = new();
#endif

		#endregion

		#region Bindable Properties

		/// <summary>
		/// Identifies the <see cref="IndicatorPlacement"/> bindable property.
		/// </summary>
		public static readonly BindableProperty IndicatorPlacementProperty =
            BindableProperty.Create(
                nameof(IndicatorPlacement),
                typeof(TabIndicatorPlacement),
                typeof(SfTabBar),
                TabIndicatorPlacement.Bottom,
                propertyChanged: OnIndicatorPlacementChanged);

        /// <summary>
        /// Identifies the <see cref="TabWidthMode"/> bindable property.
        /// </summary>
        public static readonly BindableProperty TabWidthModeProperty =
            BindableProperty.Create(
                nameof(TabWidthMode),
                typeof(TabWidthMode),
                typeof(SfTabBar),
                TabWidthMode.Default,
                propertyChanged: OnTabWidthModePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="IndicatorWidthMode"/> bindable property.
        /// </summary>
        public static readonly BindableProperty IndicatorWidthModeProperty =
            BindableProperty.Create(
                nameof(IndicatorWidthMode),
                typeof(IndicatorWidthMode),
                typeof(SfTabBar),
                IndicatorWidthMode.Fit,
                propertyChanged: OnIndicatorWidthModePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="TabHeaderPadding"/> bindable property.
        /// </summary>
        public static readonly BindableProperty TabHeaderPaddingProperty =
            BindableProperty.Create(
                nameof(TabHeaderPadding),
                typeof(Thickness),
                typeof(SfTabBar),
				defaultValueCreator: DefaultTabHeaderPaddingCreator,
                propertyChanged: OnTabHeaderPaddingPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="IndicatorBackground"/> bindable property.
        /// </summary>
        public static readonly BindableProperty IndicatorBackgroundProperty =
          BindableProperty.Create(
              nameof(IndicatorBackground),
              typeof(Brush),
              typeof(SfTabBar),
              new SolidColorBrush(Color.FromArgb("#6200EE")),
              propertyChanged: OnIndicatorBackgroundChanged);

        /// <summary>
        /// Identifies the <see cref="Items"/> bindable property.
        /// </summary>
        public static readonly BindableProperty ItemsProperty =
            BindableProperty.Create(
                nameof(Items),
                typeof(TabItemCollection),
                typeof(SfTabBar),
                null,
                propertyChanged: OnItemsChanged);

        /// <summary>
        /// Identifies the <see cref="ItemsSource"/> bindable property.
        /// </summary>
        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(
                nameof(ItemsSource),
                typeof(IList),
                typeof(SfTabBar),
                null,
                propertyChanged: OnItemsSourceChanged);

        /// <summary>
        /// Identifies the <see cref="SelectedIndex"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="SelectedIndex"/> bindable property.
        /// </value>
        public static readonly BindableProperty SelectedIndexProperty =
            BindableProperty.Create(
                nameof(SelectedIndex),
                typeof(int),
                typeof(SfTabBar),
                null,
                BindingMode.TwoWay,
                propertyChanged: OnSelectedIndexChanged);

        /// <summary>
        /// Identifies the <see cref="IsScrollButtonEnabled"/> bindable property.
        /// </summary>
        public static readonly BindableProperty IsScrollButtonEnabledProperty =
            BindableProperty.Create(
                nameof(IsScrollButtonEnabled),
                typeof(bool), typeof(SfTabBar),
                false,
                BindingMode.Default,
                propertyChanged: OnScrollButtonEnabledChanged);

        /// <summary>
        /// Identifies the <see cref="HeaderHorizontalTextAlignment"/> bindable property.
        /// </summary>
        public static readonly BindableProperty HeaderHorizontalTextAlignmentProperty =
           BindableProperty.Create(
               nameof(HeaderHorizontalTextAlignment),
               typeof(TextAlignment),
               typeof(SfTabBar),
               TextAlignment.Center,
               BindingMode.Default,
               propertyChanged: OnHeaderHorizontalAlignmentChanged);

		/// <summary>
		/// Identifies the <see cref="IsCenterButtonEnabled"/> bindable property.
		/// </summary>
		internal static readonly BindableProperty IsCenterButtonEnabledProperty =
			BindableProperty.Create(
				nameof(IsCenterButtonEnabled),
				typeof(bool),
				typeof(SfTabBar),
				false,
				BindingMode.Default,
				null,
				propertyChanged: OnIsCenterButtonEnabledChanged);

		/// <summary>
		/// Identifies the <see cref="HeaderItemTemplate"/> bindable property.
		/// </summary>
		internal static readonly BindableProperty HeaderItemTemplateProperty =
            BindableProperty.Create(
                nameof(HeaderItemTemplate),
                typeof(DataTemplate),
                typeof(SfTabBar),
                propertyChanged: OnHeaderTemplateChanged);

		/// <summary>
		/// Identifies the <see cref="HeaderItemSpacing"/> bindable property.
		/// </summary>
		internal static readonly BindableProperty HeaderItemSpacingProperty =
			BindableProperty.Create(
				nameof(HeaderItemSpacing),
				typeof(int),
				typeof(SfTabBar),
				36,
				propertyChanged: OnHeaderItemSpacingChanged);

        /// <summary>
        /// Identifies the <see cref="HeaderDisplayMode"/> bindable property.
        /// </summary>
        internal static readonly BindableProperty HeaderDisplayModeProperty =
            BindableProperty.Create(
                nameof(HeaderDisplayMode),
                typeof(TabBarDisplayMode),
                typeof(SfTabBar),
                TabBarDisplayMode.Default,
                propertyChanged: OnHeaderDisplayModeChanged);

        /// <summary>
        /// Identifies the <see cref="ContentTransitionDuration"/> bindable property.
        /// </summary>
        internal static readonly BindableProperty ContentTransitionDurationProperty =
            BindableProperty.Create(
                nameof(ContentTransitionDuration),
                typeof(double),
                typeof(SfTabBar),
                100d);

        /// <summary>
        /// Identifies the <see cref="IndicatorCornerRadius"/> bindable property.
        /// </summary>
        public static readonly BindableProperty IndicatorCornerRadiusProperty =
            BindableProperty.Create(
                nameof(IndicatorCornerRadius),
                typeof(CornerRadius),
                typeof(SfTabBar),
                new CornerRadius(-1),
                BindingMode.Default,
                propertyChanged: OnIndicatorCornerRadiusChanged);

		/// <summary>
		/// Identifies the <see cref="IndicatorStrokeThickness"/> bindable property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="IndicatorStrokeThickness"/> bindable property.
		/// </value>
		internal static readonly BindableProperty IndicatorStrokeThicknessProperty = 
			BindableProperty.Create(
				nameof(IndicatorStrokeThickness),
				typeof(double),
				typeof(SfTabBar),
				3d,
				BindingMode.Default,
				propertyChanged: OnIndicatorStrokeThicknessChanged);

		/// <summary>
		/// Identifies the <see cref="FontAutoScalingEnabled"/> bindable property.
		/// </summary>
		internal static readonly BindableProperty FontAutoScalingEnabledProperty =
            BindableProperty.Create(
                nameof(FontAutoScalingEnabled),
                typeof(bool),
                typeof(SfTabBar),
                false,
                propertyChanged: OnFontAutoScalingEnabledChanged);

		/// <summary>
		/// Identifies the <see cref="ScrollButtonColor"/> bindable property.
		/// </summary>
		internal static readonly BindableProperty ScrollButtonColorProperty =
            BindableProperty.Create(
                nameof(ScrollButtonColor),
                typeof(Color),
                typeof(SfTabBar),
                Color.FromArgb("#49454F"),
                BindingMode.Default,
                null,
                propertyChanged: OnScrollButtonColorChanged);

        /// <summary>
        /// Identifies the <see cref="ScrollButtonBackground"/> bindable property.
        /// </summary>
        internal static readonly BindableProperty ScrollButtonBackgroundProperty =
            BindableProperty.Create(
                nameof(ScrollButtonBackground),
                typeof(Brush),
                typeof(SfTabBar),
                new SolidColorBrush(Color.FromArgb("#F7F2FB")),
                BindingMode.Default,
                propertyChanged: OnScrollButtonBackgroundChanged);

        /// <summary>
        /// Identifies the <see cref="ScrollButtonDisabledIconColor"/> bindable property.
        /// </summary>
        internal static readonly BindableProperty ScrollButtonDisabledIconColorProperty =
            BindableProperty.Create(
                nameof(ScrollButtonDisabledIconColor),
                typeof(Color),
                typeof(SfTabBar),
                Color.FromArgb("#611c1b1f"),
                BindingMode.Default,
                propertyChanged: OnScrollButtonDisabledForegroundColorChanged);

		/// <summary>
		/// Identifies the <see cref="HoverBackground"/> bindable property.
		/// </summary>
		/// <remarks>This bindable property is read-only.</remarks>
		internal static readonly BindableProperty HoverBackgroundProperty = BindableProperty.Create(
		nameof(HoverBackground),
		typeof(Brush),
		typeof(SfTabBar),
		new SolidColorBrush(Color.FromArgb("#1C1B1F")),
		BindingMode.Default);

		/// <summary>
		/// Identifies the <see cref="TabHeaderAlignment"/> bindable property.
		/// </summary>
		public static readonly BindableProperty TabHeaderAlignmentProperty =
			BindableProperty.Create(
				nameof(TabHeaderAlignment),
				typeof(TabHeaderAlignment),
				typeof(SfTabBar),
				TabHeaderAlignment.Start,
				propertyChanged: OnTabHeaderAlignmentPropertyChanged);
		
		/// <summary>
		/// Identifies the <see cref="AnimationEasing"/> bindable property.
		/// </summary>
		internal static readonly BindableProperty AnimationEasingProperty =
			BindableProperty.Create(
				nameof(AnimationEasing),
				typeof(Easing),
				typeof(SfTabBar),
				Easing.Linear);

		/// <summary>
		/// Identifies the <see cref="EnableRippleAnimation"/> bindable property.
		/// </summary>
		internal static readonly BindableProperty EnableRippleAnimationProperty =
			BindableProperty.Create(
				nameof(EnableRippleAnimation),
				typeof(bool),
				typeof(SfTabBar),
				true);
		#endregion

		#region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SfTabBar"/> class.
        /// </summary>
        public SfTabBar()
        {
            Items = [];
            InitializeControl();
            _tabSelectionChangedEventArgs = new CoreEventArgs();
            _tabItemTappedEventArgs = new TabItemTappedEventArgs();
            ResetStyle();
#if NET10_0
            SafeAreaEdges = SafeAreaEdges.None;
#endif
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the value that defines the indicator placement.
        /// </summary>
        public TabIndicatorPlacement IndicatorPlacement
        {
            get => (TabIndicatorPlacement)GetValue(IndicatorPlacementProperty);
            set => SetValue(IndicatorPlacementProperty, value);
        }

        /// <summary> 
        /// Gets or sets a value that can be used to customize the horizontal text alignment in tab header. 
        /// </summary> 
        /// <value> 
        /// It accepts the <see cref="HeaderHorizontalTextAlignment"/> values, and the default value is <see cref="TextAlignment.Center"/>.
        /// </value> 
        public TextAlignment HeaderHorizontalTextAlignment
        {
            get => (TextAlignment)GetValue(HeaderHorizontalTextAlignmentProperty);
            set => SetValue(HeaderHorizontalTextAlignmentProperty, value);
        }

        /// <summary>
        /// Gets or sets the value that can be used to customize the indicator width in the tab header.
        /// </summary>
        /// <value>
        /// It accepts indicatorWidthMode values and the default value is <see cref="IndicatorWidthMode.Fit"/>.
        /// </value>
        ///<remark>
        /// It's not applicable when the <see cref="IndicatorPlacement"/> is Fill.
        /// </remark>
        public IndicatorWidthMode IndicatorWidthMode
        {
            get => (IndicatorWidthMode)GetValue(IndicatorWidthModeProperty);
            set => SetValue(IndicatorWidthModeProperty, value);
        }

        /// <summary>
        /// Gets or sets the value that defines tab width mode.
        /// </summary>
        public TabWidthMode TabWidthMode
        {
            get => (TabWidthMode)GetValue(TabWidthModeProperty);
            set => SetValue(TabWidthModeProperty, value);
        }

		/// <summary> 
		/// Gets or sets a value that can be used to customize the header position of the MAUI TabView. 
		/// </summary> 
		/// <value> 
		/// It accepts the TabHeaderAlignment values, and the default value is TabHeaderAlignment.Start. 
		/// </value> 		
		/// <remarks>  
		/// Note: This property is applicable only when the <c>TabWidthMode</c> is set to <c>SizeToContent</c>.
		/// </remarks>  
		public TabHeaderAlignment TabHeaderAlignment
		{
			get => (TabHeaderAlignment)GetValue(TabHeaderAlignmentProperty);
			set => SetValue(TabHeaderAlignmentProperty, value);
		}

        /// <summary>
        /// Gets or sets the value that can be used to customize the padding of the tab header. 
        /// </summary>
        /// <remarks>
        /// <b>Note:</b> This is only applied to the SizeToContent type of <see cref="TabWidthMode"/>.
        /// </remarks>
        public Thickness TabHeaderPadding
        {
            get => (Thickness)GetValue(TabHeaderPaddingProperty);
            set => SetValue(TabHeaderPaddingProperty, value);
        }

        /// <summary>
        /// Gets or sets the value that defines the background color of the indicator.
        /// </summary>
        public Brush IndicatorBackground
        {
            get => (Brush)GetValue(IndicatorBackgroundProperty);
            set => SetValue(IndicatorBackgroundProperty, value);
        }

        /// <summary>
        /// Gets or sets the value that defines the collection of items.
        /// </summary>
        public TabItemCollection Items
        {
            get => (TabItemCollection)GetValue(ItemsProperty);
            set => SetValue(ItemsProperty, value);
        }

        /// <summary>
        /// Gets or sets the value that defines the selected index.
        /// </summary>
        public int SelectedIndex
        {
            get => (int)GetValue(SelectedIndexProperty);
            set => SetValue(SelectedIndexProperty, value);
        }

        /// <summary> 
        /// Gets or sets a value indicating whether to enable the scroll buttons.
        /// </summary> 
        /// <value> 
        /// The default value is false.
        /// </value> 
        public bool IsScrollButtonEnabled
        {
            get => (bool)GetValue(IsScrollButtonEnabledProperty);
            set => SetValue(IsScrollButtonEnabledProperty, value);
        }

		/// <summary> 
		/// Gets or sets a value indicating whether to enable the center button.
		/// </summary> 
		/// <value> 
		/// The default value is false.
		/// </value> 
		public bool IsCenterButtonEnabled
		{
			get => (bool)GetValue(IsCenterButtonEnabledProperty);
			set => SetValue(IsCenterButtonEnabledProperty, value);
		}

		/// <summary>
		/// Gets or sets the value that defines the collection of items source.
		/// </summary>
		internal IList? ItemsSource
        {
            get => (IList?)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        /// <summary>
        /// Gets or sets the value that defines the header item template.
        /// </summary>
        internal DataTemplate? HeaderItemTemplate
        {
            get => (DataTemplate?)GetValue(HeaderItemTemplateProperty);
            set => SetValue(HeaderItemTemplateProperty, value);
        }

        /// <summary>
        /// Gets or sets the value that defines the spacing between header items.
        /// </summary>
        /// <value>
        /// It accepts int values and the default value is 36.
        /// </value>
        internal int HeaderItemSpacing
        {
	        get => (int)GetValue(HeaderItemSpacingProperty);
	        set => SetValue(HeaderItemSpacingProperty, value);
        }

        /// <summary>
        /// Gets or sets a value that can be used to customize the corner radius of selection indicator.
        /// </summary>
        /// <value>
        /// It accepts the <see cref="CornerRadius"/> values and the default value is -1.
        /// </value>
        public CornerRadius IndicatorCornerRadius
        {
            get => (CornerRadius)GetValue(IndicatorCornerRadiusProperty);
            set => SetValue(IndicatorCornerRadiusProperty, value);
        }

		/// <summary>
		/// Gets or sets a value that can be used to customize the indicator’s border height. 
		/// </summary>
		/// <value>
		/// It accepts the double values and the default value is 3.
		/// </value>
		internal double IndicatorStrokeThickness
		{
			get => (double)this.GetValue(IndicatorStrokeThicknessProperty);
			set => this.SetValue(IndicatorStrokeThicknessProperty, value);
		}

		/// <summary>
		/// Gets or sets the display mode for the tab header, determining whether to show the image and text.
		/// </summary>
		/// <value>
		/// It accepts the TabHeaderDisplayMode values, and the default value is TabHeaderDisplayMode.Default.
		/// </value>
		/// <remarks>
		/// When the <see cref="HeaderDisplayMode"/> is set to TabHeaderDisplayMode.Default, the image and text will be displayed on the tab bar based on the Header and ImageSource properties of the tab item.
		/// </remarks>
		internal TabBarDisplayMode HeaderDisplayMode
        {
            get => (TabBarDisplayMode)GetValue(HeaderDisplayModeProperty);
            set => SetValue(HeaderDisplayModeProperty, value);
        }

        /// <summary>
        /// Gets or sets a value that can be used to modify the duration of content transition in <see cref="SfTabView"/>.
        /// </summary>
        /// <value>
        /// It accepts the double values and the default value is 100.
        /// </value>
        internal double ContentTransitionDuration
        {
            get => (double)GetValue(ContentTransitionDurationProperty);
            set => SetValue(ContentTransitionDurationProperty, value);
        }

		/// <summary> 
		/// Gets or sets a value that Determines whether or not the font of the control should scale automatically according to the operating system settings. 
		/// </summary>
		/// <value>
		/// It accepts Boolean values, and the default value is false.
		/// </value>
		internal bool FontAutoScalingEnabled
        {
            get { return (bool)GetValue(FontAutoScalingEnabledProperty); }
            set { SetValue(FontAutoScalingEnabledProperty, value); }
        }

		/// <summary>
		/// Gets or sets a value that can be used to customize the scroll button’s background color in <see cref="SfTabView"/>.
		/// </summary>
		/// <value> 
		/// It accepts brush values. 
		/// </value> 
		internal Brush ScrollButtonBackground
        {
            get => (Brush)GetValue(ScrollButtonBackgroundProperty);
            set => SetValue(ScrollButtonBackgroundProperty, value);
        }

		/// <summary>
		/// Gets or sets a value that can be used to customize the scroll button’s color in <see cref="SfTabView"/>.
		/// </summary>
		/// <value> 
		/// It accepts the Color values. 
		/// </value>
		internal Color ScrollButtonColor
		{
            get => (Color)GetValue(ScrollButtonColorProperty);
            set => SetValue(ScrollButtonColorProperty, value);
        }

        /// <summary>
        /// Gets or sets a value that can be used to customize the scroll button’s disabled foreground color in <see cref="SfTabView"/>.
        /// </summary>
        /// <value>
        /// It accepts the Color values. 
        /// </value>
        internal Color ScrollButtonDisabledIconColor
        {
            get => (Color)GetValue(ScrollButtonDisabledIconColorProperty);
            set => SetValue(ScrollButtonDisabledIconColorProperty, value);
        }

		/// <summary>
		/// Gets or sets the hover color for the tab items.
		/// <remarks>The default color is Color.FromRgba("#1C1B1F").</remarks>
		/// </summary>
		internal Brush HoverBackground
		{
			get { return (Brush)this.GetValue(HoverBackgroundProperty); }
			set { this.SetValue(HoverBackgroundProperty, value); }
		}

		/// <summary>
		/// Gets or sets the easing function used for the tab selection indicator animation.
		/// </summary>
		/// <value>
		/// An <see cref="Easing"/> function that controls the indicator animation transition. The default value is <see cref="Easing.Linear"/>.
		/// </value>
		internal Easing AnimationEasing
		{
			get => (Easing)GetValue(AnimationEasingProperty);
			set => SetValue(AnimationEasingProperty, value);
		}

		/// <summary>
		/// Gets or sets a value indicating whether the animation is enabled or not when tab is selected in SfTabView.
		/// </summary>
		/// <value>
		/// It accepts the bool values. 
		/// </value>
		internal bool EnableRippleAnimation
		{
			get => (bool)GetValue(EnableRippleAnimationProperty);
			set => SetValue(EnableRippleAnimationProperty, value);
		}

		/// <summary>
		/// Gets or sets whether the tab header should be at the bottom or top or left or right of the tab content.
		/// </summary>
		/// <value>
		/// One of the <see cref="Syncfusion.Maui.Toolkit.TabView.TabBarPlacement"/> enumeration that specifies the placement of the tab bar. The default mode is <see cref="Syncfusion.Maui.Toolkit.TabView.TabBarPlacement.Top"/>.
		/// </value>
		internal TabBarPlacement Placement
		{
			get => _placement;
			set
			{
				if (_placement != value)
				{
					_placement = value;
					ApplyOrientation();
				}
			}
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// To get the effects view for the tab item.
		/// </summary>
		/// <param name="tabItem">The tab item for which the effects view is to be retrieved.</param>
		/// <returns>It returns the effectsView.</returns>
		public SfEffectsView? GetEffectsView(SfTabItem tabItem)
        {
            if (tabItem != null && _tabItemsPanel!= null)
            {
                var index = Items.IndexOf(tabItem);
                if (index >= 0)
                {
					if (IsCenterButtonEnabled && TabWidthMode is TabWidthMode.Default)
					{
						index = GetAdjustedIndexForCenterButton(index);
					}

					var child = _tabItemsPanel.Children[index];
                    if (child is SfGrid grid)
                    {
                        var effectsView = grid.Children[0] as SfEffectsView;
                        return effectsView;
                    }
                }
            }
            return null;
        }

		/// <summary>
		/// Update each header item's WidthRequest/HeightRequest based on tab bar placement.
		/// Call after changing placement so Parent.PropertyChanged runs on items.
		/// </summary>
		internal void UpdateHeaderItemsSize(TabBarPlacement placement, double tabBarSize)
		{
			var verticalWidth = tabBarSize >= 0 ? tabBarSize : 48d;
			var horizontalHeight = tabBarSize >= 0 ? tabBarSize : 48d;
			if (this.Items == null)
			{
				return;
			}

            foreach (var child in this.Items)
            {
                if (child is SfTabItem item)
                {
                    if (child is not View childView)
                        continue;

                    if (ReferenceEquals(child, _centerButtonViewPlaceholder))
                    {
                        continue;
                    }

                    if (placement == TabBarPlacement.Top || placement == TabBarPlacement.Bottom)
                    {
                        item.WidthRequest = -1;
                        item.HeightRequest = tabBarSize;
                    }
                    else
                    {
                        item.HeightRequest = -1;
                        item.WidthRequest = tabBarSize;
                    }
                    item.InvalidateMeasure();
                }
            }
        }

        /// <summary>
        /// Handles the tap event on a tab item.
        /// </summary>
        /// <param name="sender">The object that triggered the event.</param>
        /// <param name="tapEventArgs">The tap event arguments.</param>
        public void OnTap(object sender, TapEventArgs tapEventArgs)
        {
            if (sender is not View)
			{
				return;
			}

			var index = _tabItemsPanel?.Children.IndexOf((IView)(sender));
			if (IsCenterButtonEnabled && TabWidthMode is TabWidthMode.Default && ItemsSource is not null)
			{
				// Calculate the left items count
				int itemsSourceCount = ItemsSource.Count;
				int leftItemsSourceCount = itemsSourceCount / 2 + itemsSourceCount % 2; // Left should have one more when total is odd

				// If SelectedIndex is at the position of the center button, adjust the index
				if (index >= leftItemsSourceCount)
				{
					index--;
				}
			}

			if (SelectedIndex != index && index != null)
            {
                // Raise the TabItemTapped event
                var tabTappedEventArgs = new TabItemTappedEventArgs();
                RaiseTabItemTappedEvent(tabTappedEventArgs);

                // Initialize and raise the SelectionChanging event
                _selectionChangingEventArgs = new SelectionChangingEventArgs
                {
                    Index = (int)index
                };
                RaiseSelectionChangingEvent(_selectionChangingEventArgs);

                // Update the selected index if the event was not canceled
                if (!_selectionChangingEventArgs.Cancel)
                {
                    SelectedIndex = (int)index;
                }
            }
        }

        #endregion

        #region Internal Methods

        /// <summary>
        ///  This method is triggered whenever the flow direction property is changed.
        /// </summary>
        internal void UpdateFlowDirection()
        {
            foreach (var tabItem in Items)
            {
                tabItem.FlowDirection = FlowDirection;
            }
            if (IsScrollButtonEnabled)
            {
                UpdateScrollButtonEnabled(IsScrollButtonEnabled);
            }
            else
            {
                UpdateTabIndicatorWidthIfNeeded();
            }
        }

		/// <summary>
		/// Resets the cached indicator position/size state of the axis that is no longer in use after an orientation change.
		/// </summary>
		void ResetIndicatorStateForOrientationChange(bool isHorizontal)
		{
			if (isHorizontal)
			{
				_previousTabY = 0d;
				_selectedTabY = 0d;
				_currentIndicatorHeight = 0d;
				_previousIndicatorHeight = 0d;
				_removedItemHeight = 0d;
			}
			else
			{
				_previousTabX = 0d;
				_selectedTabX = 0d;
				_currentIndicatorWidth = 0d;
				_previousIndicatorWidth = 0d;
				_removedItemWidth = 0d;
			}
			_previewSelectedIndex = -1;
		}

		internal void ApplyOrientation()
		{
			if (_tabHeaderContentContainer == null || _tabHeaderScrollView == null || _tabSelectionIndicator == null)
				return;

			if (_tabHeaderContentContainer != null && _tabItemsPanel != null)
			{
				_tabHeaderContentContainer.Children.Remove(_tabItemsPanel);
			}
			var isHorizontal = _placement == TabBarPlacement.Top || _placement == TabBarPlacement.Bottom;

			ResetIndicatorStateForOrientationChange(isHorizontal);
			if (isHorizontal)
			{
				_tabItemsPanel = _tabHeaderRow;
				_tabHeaderScrollView.Orientation = ScrollOrientation.Horizontal;

				if (IndicatorPlacement != TabIndicatorPlacement.Fill)
				{
					_tabSelectionIndicator.WidthRequest = -1;
					_tabSelectionIndicator.HeightRequest = IndicatorStrokeThickness;
					_tabSelectionIndicator.HorizontalOptions = LayoutOptions.Start;
					_tabSelectionIndicator.VerticalOptions = LayoutOptions.End;
				}
				else
				{
					_tabSelectionIndicator.WidthRequest = -1;
					_tabSelectionIndicator.HeightRequest = this.HeightRequest;
					_tabSelectionIndicator.HorizontalOptions = LayoutOptions.Start;
					_tabSelectionIndicator.VerticalOptions = LayoutOptions.Fill;
				}
				_tabSelectionIndicator.TranslationX = 0;
				_tabSelectionIndicator.TranslationY = 0;
			}
			else
			{
				_tabItemsPanel = _tabHeaderColumn;
				_tabHeaderScrollView.Orientation = ScrollOrientation.Vertical;

				if (_tabItemsPanel != null)
				{
					_tabItemsPanel.VerticalOptions = LayoutOptions.Start;
					_tabItemsPanel.HorizontalOptions = LayoutOptions.Fill;
				}

				if (_tabHeaderContentContainer != null)
				{
					_tabHeaderContentContainer.VerticalOptions = LayoutOptions.Start;
					_tabHeaderContentContainer.HorizontalOptions = LayoutOptions.Fill;
				}

				_tabHeaderScrollView.VerticalOptions = LayoutOptions.Start;
				_tabHeaderScrollView.HorizontalOptions = LayoutOptions.Fill;

				if (IndicatorPlacement != TabIndicatorPlacement.Fill)
				{
					_tabSelectionIndicator.HeightRequest = -1;
					_tabSelectionIndicator.WidthRequest = IndicatorStrokeThickness;
					_tabSelectionIndicator.VerticalOptions = LayoutOptions.Start;
					_tabSelectionIndicator.HorizontalOptions = LayoutOptions.Start;
				}
				else
				{
					_tabSelectionIndicator.HeightRequest = -1;
					_tabSelectionIndicator.WidthRequest = -1;
					_tabSelectionIndicator.VerticalOptions = LayoutOptions.Start;
					_tabSelectionIndicator.HorizontalOptions = LayoutOptions.Fill;
				}

				_tabSelectionIndicator.TranslationX = 0;
				_tabSelectionIndicator.TranslationY = 0;
			}

			if (_tabHeaderContentContainer != null)
			{
				_tabHeaderContentContainer.Children.Insert(0, _tabItemsPanel);
			}

			if (_tabHeaderRow != null && _tabHeaderColumn != null)
			{
				if (isHorizontal)
				{
					_from = _tabHeaderColumn;
					_to = _tabHeaderRow;
				}
				else
				{
					_from = _tabHeaderRow;
					_to = _tabHeaderColumn;
				}

				if (_from != null && _to != null && _from.Children.Count > 0)
				{
					var moving = _from.Children.ToList();
					_from.Children.Clear();
					foreach (var c in moving)
						_to.Children.Add(c);
				}
			}

			UpdateCenterButtonPlaceholder();
			if (IsScrollButtonEnabled)
				UpdateScrollButtonEnabled(true);

            var horDefault = new Thickness(52, 0, 52, 0);
            var verDefault = new Thickness(0, 52, 0, 52);
            if (isHorizontal)
            {
                if (this.TabHeaderPadding == verDefault)
                {
                    this.SetValue(TabHeaderPaddingProperty, horDefault);
                }
            }
            else
            {
                if (this.TabHeaderPadding == horDefault)
                {
                    this.SetValue(TabHeaderPaddingProperty, verDefault);
                }
            }

            UpdateTabPadding();
            CalculateTabItemWidth();
            UpdateHeaderItemHeight();
            UpdateTabIndicatorWidth();
            UpdateTabHeaderAlignment();
            UpdateIndicatorPlacement(IndicatorPlacement);
        }

        static object DefaultTabHeaderPaddingCreator(BindableObject bindable)
        {
            var thickness = new Thickness(0);
            if (bindable is SfTabBar bar)
            {
                var isHorizontal = bar.Placement == TabBarPlacement.Top || bar.Placement == TabBarPlacement.Bottom;
                if (isHorizontal)
                {
                    thickness = new Thickness(52, 0, 52, 0);
                }
                else
                {
                    thickness = new Thickness(0, 52, 0, 52);
                }
            }
            return thickness;
        }

        void UpdateTabIndicatorWidthIfNeeded()
        {
#if !WINDOWS
            UpdateTabIndicatorWidth();
#endif
        }

        /// <summary>
        /// Invokes <see cref="SelectionChanged"/> event.
        /// </summary>
        /// <param name="args">The core event args.</param>
        internal void RaiseSelectionChangedEvent(CoreEventArgs args)
        {
			SelectionChanged?.Invoke(this, args);
		}

        /// <summary>
        /// Invokes <see cref="TabItemTapped"/> event.
        /// </summary>
        /// <param name="args">The tab item tapped event args.</param>
        internal void RaiseTabItemTappedEvent(TabItemTappedEventArgs args)
        {
			TabItemTapped?.Invoke(this, args);
		}

        /// <summary>
        /// Invokes <see cref="SelectionChanging"/> event.
        /// </summary>
        /// <param name="args">The event arguments.</param>
        internal void RaiseSelectionChangingEvent(SelectionChangingEventArgs args)
        {
			SelectionChanging?.Invoke(this, args);
		}

		/// <summary>
		/// To update the position of the tab indicator.
		/// </summary>
		internal void UpdateTabIndicatorPosition()
        {
            // Check if tabSelectionIndicator is not null
            if (_tabSelectionIndicator != null)
            {
					int adjustedIndex = SelectedIndex; // Default to the existing selected index
					if (IsCenterButtonEnabled && TabWidthMode is TabWidthMode.Default)
					{
						adjustedIndex = GetAdjustedIndexForCenterButton(adjustedIndex);
					}

					bool isHorizontal = this.Placement == TabBarPlacement.Top || this.Placement == TabBarPlacement.Bottom;
					bool isVertical = this.Placement == TabBarPlacement.Left || this.Placement == TabBarPlacement.Right;
					bool canScroll = (isHorizontal && _currentIndicatorWidth >= 0) || (isVertical && _currentIndicatorHeight >= 0);

				// Update the scroll view position based on the currently selected index
				if (canScroll)
					UpdateScrollViewPosition(adjustedIndex);
            }
        }

        /// <summary>
        /// To update the corner radius for selection indicator.
        /// </summary>
        internal void UpdateCornerRadius()
        {
            // Update the corner radius if roundRectangle is not null
            if (_roundRectangle != null)
            {
                _roundRectangle.CornerRadius = IndicatorCornerRadius;
            }
        }

		/// <summary>
		/// To update the border height for selection indicator.
		/// </summary>
		/// <param name="newValue"></param>
		internal void UpdateIndicatorStrokeThickness(double newValue)
		{
			var indicatorPlacement = this.IndicatorPlacement;

			if (_tabSelectionIndicator == null)
				return;

			if (indicatorPlacement == TabIndicatorPlacement.Fill)
				return;

			var isHorizontal = _placement == TabBarPlacement.Top || _placement == TabBarPlacement.Bottom;

			if (isHorizontal)
			{
				_tabSelectionIndicator.HeightRequest = newValue;
			}
			else
			{
				_tabSelectionIndicator.WidthRequest = newValue;
			}
		}

		/// <summary>
		/// To update the indicator placement.
		/// </summary>
		/// <param name="indicatorPlacement">The indicator placement.</param>
		internal void UpdateIndicatorPlacement(TabIndicatorPlacement indicatorPlacement)
        {
            if (Items != null)
            {
                foreach (var item in Items)
                {
                    if (item != null)
                    {
                        item.IndicatorPlacement = IndicatorPlacement;
                        item.ChangeSelectedState();
                    }
                }
            }
            UpdateTabSelectionIndicator(indicatorPlacement);
            UpdateTabIndicatorWidth();
        }

        /// <summary>
        /// To clear the tab header item.
        /// </summary>
        /// <param name="tabItem">tabItem.</param>
        /// <param name="index">index.</param>
        internal void ClearHeaderItem(SfTabItem tabItem, int index)
        {
            if (tabItem != null)
            {
				if (tabItem.HeaderContent != null)
				{
					DetachHeaderContentSizeChangeHandlers(tabItem.HeaderContent);
				}
				int removeItemIndex = index;
				if (IsCenterButtonEnabled && TabWidthMode is TabWidthMode.Default)
				{
					removeItemIndex = GetAdjustedIndexForCenterButton(index);
				}

				// Remove the child at specified index
				_tabItemsPanel?.Children.RemoveAt(removeItemIndex);
				AddCenterButtonViewPlaceholder();
				if (Items.Count > 0 && SelectedIndex >= Items.Count && index==SelectedIndex)
                {
                    SelectedIndex = Items.Count - 1;
                }
				else if (index <= SelectedIndex)
				{
					_removedItemWidth = tabItem.Width;
					_removedItemHeight = tabItem.Height;

					if (index < SelectedIndex)
					{
						if(SelectedIndex<=Items.Count)
						{
							SelectedIndex--;
						}
						else
						{
							SelectedIndex = Items.Count - 1;
						}
						
					}
					else
					{
						UpdateTabIndicatorWidth();
					}
				}

				if (IsScrollButtonEnabled)
                {
                    UpdateScrollButtonState();
                }
            }
        }

        /// <summary>
        /// To update the selected index.
        /// </summary>
        /// <param name="newIndex">newIndex.</param>
        /// <param name="oldIndex">oldIndex.</param>
        internal void UpdateSelectedIndex(int newIndex, int oldIndex)
        {
            if (newIndex != -1)
            {
                UpdateSelectedTabItemIsSelected(newIndex, oldIndex);
                UpdateTabIndicatorWidth();
                if (_tabSelectionChangedEventArgs != null)
                {
                    _tabSelectionChangedEventArgs.OldIndex = oldIndex;
                    _tabSelectionChangedEventArgs.NewIndex = newIndex;
                    RaiseSelectionChangedEvent(_tabSelectionChangedEventArgs);
                }
            }
        }

        /// <summary>
        /// Updates the font auto-scaling setting for all tab items.
        /// </summary>
        /// <param name="isEnabled">Whether font auto-scaling should be enabled.</param>
        internal void UpdateFontAutoScalingEnabled(bool isEnabled)
        {
            if (Items != null)
            {
                foreach (var item in Items)
                {
                    if (item != null)
					{
						item.FontAutoScalingEnabled = isEnabled;
					}
				}
            }
        }

        /// <summary>
        /// Updates the scroll button functionality based on the provided value.
        /// </summary>
        /// <param name="isEnabled">Whether scroll buttons should be enabled.</param>
        internal void UpdateScrollButtonEnabled(bool isEnabled)
        {
            if (IsScrollButtonEnabled)
            {
                UpdateOnScrollButtonEnabled();
            }
            else
            {
                UpdateScrollButtonDisabled();
            }
#if !WINDOWS
            UpdateTabIndicatorWidth();
#endif
        }

		/// <summary>
		/// Inserts or removes the center button based on the center button enabled state.
		/// </summary>
		internal void UpdateIsCenterButtonEnabled()
		{
			UpdateCenterButtonPlaceholder();

			CalculateTabItemWidth();
			CalculateTabItemsSourceWidth();
			if (ItemsSource is not null && ItemsSource.Count > 0 && (!IsCenterButtonEnabled || IsCenterButtonEnabled && TabWidthMode is TabWidthMode.SizeToContent))
			{
				if (_tabItemsPanel is not null)
				{
					for (int index = 0; index < _tabItemsPanel.Count; index++)
					{
						if (_tabItemsPanel.Children[index] is not null && _tabItemTemplateWidth is not null && _tabItemTemplateWidth.Count > 0 && _tabItemTemplateWidth[index] > 0 && _tabItemsPanel.Children[index] is View view)
						{
							view.WidthRequest = _tabItemTemplateWidth[index];
						}
					}
				}
			}
			
			UpdateTabIndicatorWidth();
		}

		/// <summary>
		/// Updates the "IsSelected" property for the selected tab item.
		/// </summary>
		/// <param name="newIndex">new index.</param>
		/// <param name="oldIndex">old index.</param>
		internal void UpdateSelectedTabItemIsSelected(int newIndex, int oldIndex)
        {
            if (ItemsSource != null && ItemsSource.Count > 0)
            {
                return;
            }
            else if (Items != null && Items.Count > 0)
            {
                DeselectOldItem(oldIndex);
                SelectNewItem(newIndex);
            }
        }

        /// <summary>
        /// Method used to update the items.
        /// </summary>
        internal void UpdateItems()
        {
            _tabItemsPanel?.Children.Clear();
            ClearItems();

            if (Items is null)
			{
				return;
			}

			int count = 0;
            foreach (var item in Items)
            {
				int index = count++;
				if (IsCenterButtonEnabled && TabWidthMode is TabWidthMode.Default)
				{
					index = GetAdjustedIndexForCenterButton(index);
				}

				if (item is not null)
				{
					AddTabItemProperties(item, index);
				}
			}

            Items.CollectionChanged -= Items_CollectionChanged;
            Items.CollectionChanged += Items_CollectionChanged;
        }

        /// <summary>
        /// Method used to update the height of the tab header item.
        /// </summary>
        internal void UpdateHeaderItemHeight()
        {
			if (_tabItemsPanel == null || _tabHeaderContentContainer == null || _tabHeaderScrollView == null)
				return;

			var isHorizontal = _placement == TabBarPlacement.Top || _placement == TabBarPlacement.Bottom;
			if (isHorizontal)
			{
				_tabItemsPanel.HeightRequest = HeightRequest;
				_tabItemsPanel.WidthRequest = -1;

				if (_tabSelectionIndicator != null && IndicatorPlacement == TabIndicatorPlacement.Fill)
					_tabSelectionIndicator.HeightRequest = HeightRequest;

				_tabHeaderContentContainer.HeightRequest = _tabItemsPanel.HeightRequest;
				_tabHeaderContentContainer.WidthRequest = -1;

				_tabHeaderScrollView.HeightRequest = _tabItemsPanel.HeightRequest;
				_tabHeaderScrollView.WidthRequest = -1;

				CalculateTabItemWidth();
			}
			else
			{
				_tabItemsPanel.HeightRequest = -1;
				_tabItemsPanel.WidthRequest = WidthRequest;

				_tabHeaderContentContainer.HeightRequest = -1;
				_tabHeaderContentContainer.WidthRequest = WidthRequest;
				_tabHeaderScrollView.WidthRequest = _tabItemsPanel.WidthRequest;
				var viewportHeight = HeightRequest > 0
					  ? HeightRequest
					  : ((Parent as VisualElement)?.Height ?? 0);
				if (IsScrollButtonEnabled)
				{
					_tabHeaderScrollView.HeightRequest = Math.Max(0, viewportHeight - (_arrowButtonSize * 2));
					_tabHeaderScrollView.VerticalOptions = LayoutOptions.Start;
				}
				else if (TabWidthMode == TabWidthMode.SizeToContent)
				{
					_tabHeaderScrollView.HeightRequest = viewportHeight;
					_tabHeaderScrollView.VerticalOptions = LayoutOptions.Start;
				}
				else
				{
					_tabHeaderScrollView.HeightRequest = viewportHeight;
				}
				CalculateTabItemWidth();

				if (_tabSelectionIndicator != null)
				{
					if (IndicatorPlacement != TabIndicatorPlacement.Fill)
						_tabSelectionIndicator.WidthRequest = IndicatorStrokeThickness;
					else
						_tabSelectionIndicator.WidthRequest = WidthRequest;
				}
			}
		}

        internal void UpdateSelectionIndicatorZIndex()
        {
            if (IndicatorPlacement == TabIndicatorPlacement.Fill)
            {
                _tabHeaderContentContainer?.Children.Remove(_tabSelectionIndicator);
                _tabHeaderContentContainer?.Children.Insert(0, _tabSelectionIndicator);
            }
            else
            {
                _tabHeaderContentContainer?.Children.Remove(_tabSelectionIndicator);
                _tabHeaderContentContainer?.Children.Add(_tabSelectionIndicator);
            }
        }

        /// <summary>
        /// To add the tab item properties.
        /// </summary>
        /// <param name="tabItem">tabItem.</param>
        /// <param name="index">index.</param>
        internal void AddTabItemProperties(SfTabItem tabItem, int index)
        {
            if (tabItem != null)
            {
                // Assign a control template if not already assigned
                tabItem.ControlTemplate ??= new ControlTemplate(typeof(TabViewMaterialVisualStyle));

                ConfigureTabItemProperties(tabItem);

                // Set the item as selected if its index matches the selected index
                if (index == SelectedIndex)
                {
                    tabItem.IsSelected = true;
                    if (tabItem.IsDescriptionNotSetByUser)
                    {
                        SemanticProperties.SetDescription(tabItem, "Selected " + tabItem.Header);
                    }
                }

                // Add the tab item to header items
                AddHeaderItems(tabItem, index);
            }
        }

        /// <summary>
        /// To add the tab header items.
        /// </summary>
        /// <param name="item">item.</param>
        /// <param name="index">index.</param>
        internal void AddHeaderItems(View? item, int index = -1)
        {
			SfGrid touchEffectGrid = new SfGrid
			{
				Style = new Style(typeof(SfGrid))
			};

			var isHorizontal = _placement == TabBarPlacement.Top || _placement == TabBarPlacement.Bottom;
			if (item != null)
			{
				if (isHorizontal)
				{
					item.VerticalOptions = LayoutOptions.Fill;
					item.HorizontalOptions = LayoutOptions.Center;
					touchEffectGrid.VerticalOptions = LayoutOptions.Fill;
					touchEffectGrid.HorizontalOptions = LayoutOptions.Fill;
				}
				else
				{
					item.VerticalOptions = LayoutOptions.Start;
					item.HorizontalOptions = LayoutOptions.Fill;
					touchEffectGrid.VerticalOptions = LayoutOptions.Start;
					touchEffectGrid.HorizontalOptions = LayoutOptions.Fill;
				}
			}
			
            touchEffectGrid.SetBinding(SfGrid.IsVisibleProperty, BindingHelper.CreateBinding(nameof(SfTabItem.IsVisible), getter: static (SfTabItem item) => item.IsVisible, source: item));

            CalculateTabItemWidth();

            AddTouchEffects(touchEffectGrid);
            touchEffectGrid.Children.Add(item);

            InsertHeaderItemToContent(touchEffectGrid, index);

			if (item is SfTabItem tabItem && tabItem.HeaderContent != null)
			{
				AttachHeaderContentSizeChangeHandlers(tabItem.HeaderContent);
			}

			if (IsScrollButtonEnabled)
            {
                UpdateScrollButtonState();
            }
        }

		/// <summary>
		/// This method is used to measure the header content height.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		internal double MeasureHeaderContentHeight(SfTabItem item)
		{
			if (item != null)
			{
				Size desiredSize = item.Header.Measure((float)item.FontSize, item.FontAttributes, item.FontFamily);
				var height = 0d;
				_tabHeaderImageSize = CalculateTabHeaderImageSize(item.ImageSize);
				if (item.IsVisible)
				{
					var hasImage = HasValidImageSource(item.ImageSource);
					if (hasImage && !string.IsNullOrEmpty(item.Header))
					{
						if (item.HeaderDisplayMode == TabBarDisplayMode.Image)
						{
							height = _tabHeaderImageSize;
						}
						else if (item.HeaderDisplayMode == TabBarDisplayMode.Text)
						{
							height = desiredSize.Height;
						}
						else
						{
							if (item.ImagePosition == TabImagePosition.Top || item.ImagePosition == TabImagePosition.Bottom)
							{
								height = desiredSize.Height + item.ImageTextSpacing + _tabHeaderImageSize;
							}
							else
							{
								height = Math.Max(desiredSize.Height, _tabHeaderImageSize);
							}
						}
					}
					else if (hasImage)
					{
						height = _tabHeaderImageSize;
					}
					else
					{
						height = desiredSize.Height;
					}
				}

				return height;
			}
			return 0;
		}

		/// <summary>
		/// This method is used to measure the header content width.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		internal double MeasureHeaderContentWidth(SfTabItem item)
        {
            if (item != null)
            {
                var width = 0d;
				if (item.IsVisible)
                {
					if (item.HeaderContent != null)
					{
						Size headerContentSize = item.HeaderContent.Measure(double.PositiveInfinity, double.PositiveInfinity);
						width = headerContentSize.Width;
					}
					else
					{
						Size desiredSize = item.Header.Measure((float)item.FontSize, item.FontAttributes, item.FontFamily);
						_tabHeaderImageSize = CalculateTabHeaderImageSize(item.ImageSize);
						var hasImage = HasValidImageSource(item.ImageSource);
						if (hasImage && !string.IsNullOrEmpty(item.Header))
						{
							width = CalculateWidthWithImageAndText(item, desiredSize);
						}
						else if (hasImage)
						{
							width = _tabHeaderImageSize;
						}
						else
						{
							width = desiredSize.Width;
						}
					}
                }

                return width;
            }
            return 0;
        }

        /// <summary>
        /// Method used to calculate the tab item width.
        /// </summary>
        internal void CalculateTabItemWidth()
        {
            if (Items != null && Items.Count > 0)
            {
                if (TabWidthMode == TabWidthMode.SizeToContent)
                {
                    CalculateTabItemWidthForSizeToContent();
                }
                else if (TabWidthMode == TabWidthMode.Default)
                {
                    CalculateTabItemWidthForDefaultWidthMode();
                }
                else
                {
                    CalculateTabItemWidthForCustomMode();
                }
            }
        }

		/// <summary>
		/// Calculates the width of tab items based on the selected TabWidthMode.
		/// </summary>
		internal void CalculateTabItemsSourceWidth()
		{
			if (ItemsSource is not null && ItemsSource.Count > 0)
			{
				if (TabWidthMode == TabWidthMode.SizeToContent)
				{
					CalculateTabItemsSourceWidthForSizeToContent();
				}
				else
				{
					CalculateTabItemsSourceWidthForDefaultWidthMode();
				}
			}
		}

		/// <summary>
		/// To update the scroll view position.
		/// </summary>
		/// <param name="position">position.</param>
		internal void UpdateScrollViewPosition(int position)
        {
            if (_tabHeaderScrollView != null &&
                _tabItemsPanel!= null &&
                _tabItemsPanel.Children.Count > position &&
                (TabWidthMode != TabWidthMode.Default ||
                (ItemsSource != null && ItemsSource.Count > 0 || HeaderItemTemplate != null)))
            {
                int rtlPosition = _tabItemsPanel.Children.Count - position - 1;
#if __MACCATALYST__ || __IOS__
                UpdateScrollViewPositionForIOS(position, rtlPosition);
#elif ANDROID
                UpdateScrollViewPositionForAndroid(position, rtlPosition);
#else
                UpdateScrollViewPositionForWindows(position, rtlPosition);
#endif
            }
        }

#if __MACCATALYST__ || __IOS__

        async void UpdateScrollViewPositionForIOS(int position, int rtlPosition)
        {
			var isHorizontal = _placement == TabBarPlacement.Top || _placement == TabBarPlacement.Bottom;

			if (HeaderItemTemplate == null && _tabHeaderScrollView != null &&
                _tabItemsPanel!= null)
            {
				if (isHorizontal)
				{
					HeightRequest = -1;
					double totalWidth = 0;
					foreach (var child in _tabItemsPanel.Children)
					{
						if (!double.IsNaN(child.Width))
						{
							totalWidth += child.Width;
						}
					}

					if ((totalWidth > Width) && (Width != -1))
					{
						if (((this as IVisualElementController).EffectiveFlowDirection & EffectiveFlowDirection.RightToLeft) == EffectiveFlowDirection.RightToLeft)
						{
#if IOS
							if (position == 0 && TabWidthMode == TabWidthMode.SizeToContent)
							{
								await _tabHeaderScrollView.ScrollToAsync(totalWidth + TabHeaderPadding.Right, 0, true).ConfigureAwait(false);
							}
							else
#endif
							{
								await _tabHeaderScrollView.ScrollToAsync(_tabItemsPanel?.Children[rtlPosition] as Element, ScrollToPosition.MakeVisible, true).ConfigureAwait(false);
							}
						}
						else
						{
#if IOS
							if (position == 0 && TabWidthMode == TabWidthMode.SizeToContent)
							{
								await _tabHeaderScrollView.ScrollToAsync(0, 0, true).ConfigureAwait(false);
							}
							else
#endif
							{
								await _tabHeaderScrollView.ScrollToAsync(_tabItemsPanel?.Children[position] as Element, ScrollToPosition.MakeVisible, true).ConfigureAwait(false);
							}
						}
					}
				}
				else
				{
					WidthRequest = -1;
					double totalHeight = 0;
					foreach (var child in _tabItemsPanel.Children)
					{
						if (child.Height != double.NaN)
						{
							totalHeight += child.Height;
						}
					}

					if ((totalHeight > HeightRequest) && (HeightRequest != -1))
					{
						if (((this as IVisualElementController).EffectiveFlowDirection & EffectiveFlowDirection.RightToLeft) == EffectiveFlowDirection.RightToLeft)
						{
#if IOS
							if (position == 0 && TabWidthMode == TabWidthMode.SizeToContent)
							{
								await _tabHeaderScrollView.ScrollToAsync(totalHeight + TabHeaderPadding.Top, 0, true).ConfigureAwait(false);
							}
							else
#endif
							{
								await _tabHeaderScrollView.ScrollToAsync(_tabItemsPanel?.Children[rtlPosition] as Element, ScrollToPosition.MakeVisible, true).ConfigureAwait(false);
							}
						}
						else
						{
#if IOS
							if (position == 0 && TabWidthMode == TabWidthMode.SizeToContent)
							{
								await _tabHeaderScrollView.ScrollToAsync(0, 0, true).ConfigureAwait(false);
							}
							else
#endif
							{
								await _tabHeaderScrollView.ScrollToAsync(_tabItemsPanel?.Children[position] as Element, ScrollToPosition.MakeVisible, true).ConfigureAwait(false);
							}
						}
					}
				}
            }
        }

#elif ANDROID

        async void UpdateScrollViewPositionForAndroid(int position, int rtlPosition)
        {
			if (_tabHeaderScrollView == null || _tabItemsPanel == null)
			{
				return;
			}

			var isHorizontal = _placement == TabBarPlacement.Top || _placement == TabBarPlacement.Bottom;
			var targetElement = _tabItemsPanel.Children.Count > position ? _tabItemsPanel.Children[position] as Element : null;
			var rtlElement = _tabItemsPanel.Children.Count > rtlPosition ? _tabItemsPanel.Children[rtlPosition] as Element : null;

			if (isHorizontal)
			{
				if (((this as IVisualElementController).EffectiveFlowDirection & EffectiveFlowDirection.RightToLeft) == EffectiveFlowDirection.RightToLeft)
				{
					if (rtlElement != null && IsDescendantOfScrollView(rtlElement, _tabHeaderScrollView))
					{
						await _tabHeaderScrollView.ScrollToAsync(_tabItemsPanel?.Children[rtlPosition] as Element, ScrollToPosition.Center, true).ConfigureAwait(false);
					}
				}
				else
				{
					if (targetElement != null && IsDescendantOfScrollView(targetElement, _tabHeaderScrollView))
					{
						await _tabHeaderScrollView.ScrollToAsync(_tabItemsPanel?.Children[position] as Element, ScrollToPosition.Center, true).ConfigureAwait(false);
					}
				}
			}
			else
			{
				if (targetElement != null && IsDescendantOfScrollView(targetElement, _tabHeaderScrollView))
				{
					await _tabHeaderScrollView.ScrollToAsync(_tabItemsPanel?.Children[position] as Element, ScrollToPosition.Center, true).ConfigureAwait(false);
				}
			}
	    }

#else

        async void UpdateScrollViewPositionForWindows(int position, int rtlPosition)
        {
           if (_tabHeaderScrollView == null || _tabItemsPanel == null)
		   {
				return;
		   }

			var isHorizontal = _placement == TabBarPlacement.Top || _placement == TabBarPlacement.Bottom;
			var targetElement = _tabItemsPanel.Children.Count > position ? _tabItemsPanel.Children[position] as Element : null;

				if (targetElement != null && _tabHeaderScrollView != null && IsDescendantOfScrollView(targetElement, _tabHeaderScrollView))
				{
					await _tabHeaderScrollView.ScrollToAsync(_tabItemsPanel?.Children[position] as Element, ScrollToPosition.Center, true).ConfigureAwait(false);
			    }
	    }
#endif

		static bool IsDescendantOfScrollView(Element? child, ScrollView? scrollView)
		{
			if (child == null || scrollView == null)
				return false;

			Element? parent = child.Parent;
			while (parent != null)
			{
				if (ReferenceEquals(parent, scrollView))
					return true;
				parent = parent.Parent;
			}
			return false;
		}

		#region Center button methods
		/// <summary>
		/// Updates the width of the center button.
		/// </summary>
		/// <param name="width">The new width to be set for the center button.</param>
		internal void UpdateCenterButtonWidth(double width)
		{
			if (_centerButtonViewPlaceholder is not null && width >= 0)
			{
				_centerButtonViewPlaceholder.WidthRequest = width;
				UpdateTabBarLayout();
			}
		}

		/// <summary>
		/// Updates the height of the center button.
		/// </summary>
		/// <param name="height">The new height to be set for the center button.</param>
		internal void UpdateCenterButtonHeight(double height)
		{
			if (_centerButtonViewPlaceholder is not null && height >= 0)
			{
				_centerButtonViewPlaceholder.HeightRequest = height;
			}
		}

		/// <summary>
		/// Updates the layout of the tab bar by recalculating the width of tab items, updating the height of header items, and adjusting the width of the tab indicator.
		/// </summary>
		void UpdateTabBarLayout()
		{
			UpdateCenterButtonPlaceholder();
			CalculateTabItemWidth();
			CalculateTabItemsSourceWidth();
			UpdateTabIndicatorWidth();
			if (IsScrollButtonEnabled)
			{
				UpdateScrollButtonState();
			}
		}
		#endregion

		#endregion

		#region Private Methods

		/// <summary>
		/// Attaches size change handlers to HeaderContent recursively.
		/// </summary>
		void AttachHeaderContentSizeChangeHandlers(View content)
		{
			if (content == null)
				return;

			content.SizeChanged -= OnHeaderContentSizeChanged;
			content.SizeChanged += OnHeaderContentSizeChanged;

			content.PropertyChanged -= OnHeaderContentChildrenPropertyChanged;
			content.PropertyChanged += OnHeaderContentChildrenPropertyChanged;

			if (content is Layout layout)
			{
				foreach (var child in layout.Children)
				{
					if (child is View childView)
						AttachHeaderContentSizeChangeHandlers(childView);
				}
			}
		}

		/// <summary>
		/// Detaches size change handlers from HeaderContent recursively.
		/// </summary>
		void DetachHeaderContentSizeChangeHandlers(View content)
		{
			if (content == null)
				return;

			content.SizeChanged -= OnHeaderContentSizeChanged;
			content.PropertyChanged -= OnHeaderContentChildrenPropertyChanged;

			if (content is Layout layout)
			{
				foreach (var child in layout.Children)
				{
					if (child is View childView)
						DetachHeaderContentSizeChangeHandlers(childView);
				}
			}
		}

		/// <summary>
		/// Handles HeaderContent size changes.
		/// </summary>
		void OnHeaderContentSizeChanged(object? sender, EventArgs e)
		{
			if (Dispatcher != null)
			{
				Dispatcher.Dispatch(() =>
				{
					CalculateTabItemWidth();
					UpdateTabIndicatorWidth();
				});
			}
			else
			{
				CalculateTabItemWidth();
				UpdateTabIndicatorWidth();
			}
		}

		/// <summary>
		/// Handles property changes in HeaderContent children that affect layout.
		/// </summary>
		void OnHeaderContentChildrenPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Width" || e.PropertyName == "Height" ||
				e.PropertyName == "WidthRequest" || e.PropertyName == "HeightRequest" ||
				e.PropertyName == "Margin" || e.PropertyName == "Padding" ||
				e.PropertyName == "HorizontalOptions" || e.PropertyName == "VerticalOptions")
			{
				CalculateTabItemWidth();
				UpdateTabIndicatorWidth();
			}
		}

		void UpdateTabHeaderPadding()
        {
            UpdateTabPadding();
		}

        internal void UpdateTabPadding()
        {
            if (_tabHeaderContentContainer != null)
            {
                _tabHeaderContentContainer.Padding = TabWidthMode == TabWidthMode.Default ? new Thickness(0) : TabHeaderPadding;
            }

            CalculateTabItemWidth();
			CalculateTabItemsSourceWidth();

#if ANDROID || IOS
                InvalidateMeasure();
#endif
		}

		void UpdateTabHeaderItemSpacing()
        {
	        CalculateTabItemWidth();
        }

        void AddTabViewItemFromTemplate(object? item)
        {
            var view = item as View;
            view?.AddGestureListener(this);
        }

        void UpdateItemsSource()
        {
            if (ItemsSource != null || HeaderItemTemplate != null)
            {
                BindableLayout.SetItemsSource(_tabItemsPanel, ItemsSource);
                BindableLayout.SetItemTemplate(_tabItemsPanel, HeaderItemTemplate);
				UpdateCenterButtonPlaceholder();
                if (_tabItemsPanel!= null)
                {
					if (IsCenterButtonEnabled && TabWidthMode is TabWidthMode.Default)
					{
						AddCenterButtonViewPlaceholder();
						CalculateTabItemsSourceWidthForDefaultWidthMode();
					}
					else
					{
						foreach (var item in _tabItemsPanel.Children)
						{
							if (item is not null)
							{
								AddTabViewItemFromTemplate(item);
							}
						}
					}
				}

				UpdateScrollButtonState();
                if (HeaderItemTemplate != null)
                {
                    UpdateTabIndicatorTemplateWidth();
                }

                ClearIndicatorWidth();
                if (ItemsSource is INotifyCollectionChanged itemsSource)
                {
                    itemsSource.CollectionChanged -= OnCollectionChanged;
                    itemsSource.CollectionChanged += OnCollectionChanged;
                }
            }
        }

        void ClearIndicatorWidth()
        {
            if (ItemsSource?.Count == 0)
            {
                if (_tabSelectionIndicator != null)
                {
                    _tabSelectionIndicator.WidthRequest = 0.01d;
                }
            }
        }

        void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            var headerItemsCount = _tabItemsPanel?.Children?.Count;

            HandleOldItems(e);
            HandleNewItems(e, headerItemsCount);
			UpdateTabIndicatorTemplateWidth();
		}

        void HandleOldItems(NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                if (ItemsSource?.Count > 0 && SelectedIndex >= ItemsSource?.Count)
                {
                    SelectedIndex = ItemsSource.Count - 1;
                }

				if (IsCenterButtonEnabled && TabWidthMode is TabWidthMode.Default && _tabItemsPanel is not null)
				{
					_tabItemsPanel.Children.Clear();
					BindableLayout.SetItemsSource(_tabItemsPanel, null);
					UpdateItemsSource();
				}

				if (ItemsSource?.Count <= 0)
				{
					ClearIndicatorWidth();
				}

				CalculateTabItemsSourceWidth();
			}
        }

        void HandleNewItems(NotifyCollectionChangedEventArgs e, int? headerItemsCount)
        {
            if (e.NewItems != null)
            {
				if (IsCenterButtonEnabled && TabWidthMode is TabWidthMode.Default && _tabItemsPanel is not null)
				{
					_tabItemsPanel.Children.Clear();
					BindableLayout.SetItemsSource(_tabItemsPanel, null);
					UpdateItemsSource();
				}
				else
				{
					foreach (var item in e.NewItems)
					{
						if (item != null)
						{
							var index = ItemsSource?.IndexOf(item);
							if (SelectedIndex >= index &&
								SelectedIndex + 1 < ItemsSource?.Count &&
								headerItemsCount != _tabItemsPanel?.Children.Count)
							{
								SelectedIndex += 1;
							}

							CalculateTabItemsSourceWidth();
						}
					}
				}
			}
        }

        void OnTabItemPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.PropertyName))
            {
				if (e.PropertyName.Equals(nameof(SfTabItem.Header), StringComparison.Ordinal))
				{
					if (sender is SfTabItem)
					{
						var item = sender as SfTabItem;
						if (item is not null && item.Header is null)
						{
							item.Header = string.Empty;
							return;
						}
					}
				}

				if (e.PropertyName.Equals(nameof(SfTabItem.HeaderContent), StringComparison.Ordinal))
				{
					if (sender is SfTabItem tabItem)
					{
						if (tabItem.HeaderContent != null)
						{
							AttachHeaderContentSizeChangeHandlers(tabItem.HeaderContent);
						}

						CalculateTabItemWidth();
						UpdateTabIndicatorWidth();
					}
				}

				if (e.PropertyName.Equals(nameof(SfTabItem.IsVisible), StringComparison.Ordinal))
                {
                    if (sender is SfTabItem item && item != null)
                    {
                        if (item.IsSelected && !item.IsVisible)
                        {
                            GetNextVisibleItem();
                        }
                    }
                    CalculateTabItemWidth();
					UpdateTabIndicatorWidth();
				}
                else if (e.PropertyName.Equals(nameof(SfTabItem.Width), StringComparison.Ordinal) ||
                    e.PropertyName.Equals(nameof(SfTabItem.WidthRequest), StringComparison.Ordinal))
                {
#if ANDROID
					_isTabItemPropertyChanged = IsCenterButtonEnabled && TabWidthMode is TabWidthMode.Default;
#endif
					UpdateTabIndicatorWidth();
#if ANDROID
					_isTabItemPropertyChanged = false;
#endif
				}
                else if (e.PropertyName == nameof(SfTabItem.Header) ||
                    e.PropertyName.Equals(nameof(SfTabItem.ImageSource), StringComparison.Ordinal))
                {
                    UpdateHeaderDisplayMode();
                }

				if (e.PropertyName.Equals(nameof(SfTabItem.ImagePosition), StringComparison.Ordinal) ||
					e.PropertyName.Equals(nameof(SfTabItem.Header), StringComparison.Ordinal) ||
					e.PropertyName.Equals(nameof(SfTabItem.FontSize), StringComparison.Ordinal) ||
					e.PropertyName.Equals(nameof(SfTabItem.ImageTextSpacing), StringComparison.Ordinal)||
					e.PropertyName.Equals(nameof(SfTabItem.ImageSize),StringComparison.Ordinal))
                {
                    if (TabWidthMode == TabWidthMode.SizeToContent)
                    {
                        CalculateTabItemWidth();
                    }
                    UpdateTabIndicatorWidth();
                }
            }
        }

		bool HasValidImageSource(ImageSource? source)
		{
			if (source is null)
			{
				return false;
			}

			if (source is FileImageSource file)
			{
				return !string.IsNullOrWhiteSpace(file.File);
			}
			else if (source is UriImageSource uri)
			{
				return uri.Uri is not null;
			}
			else if (source is FontImageSource font)
			{
				return !string.IsNullOrWhiteSpace(font.Glyph);
			}
			else if (source is StreamImageSource stream)
			{
				return stream.Stream is not null;
			}

			return true;
		}

		void GetNextVisibleItem()
        {
            double index = SelectedIndex;
            for (int i = SelectedIndex + 1; i < Items.Count; i++)
            {
                if (Items[i] != null && Items[i].IsVisible)
                {
                    SelectedIndex = i;
                    break;
                }
            }

            if (index == SelectedIndex)
            {
                for (int i = SelectedIndex - 1; i >= 0; i--)
                {
                    if (Items[i] != null && Items[i].IsVisible)
                    {
                        SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        void OnTabItemTouched(object? sender, TouchEventArgs e)
        {
            if (sender is SfTabItem tabItem)
            {
                var effectsView = GetEffectsView(tabItem);
                if (e.Action == PointerActions.Pressed)
                {
                    HandlePressed(tabItem, effectsView, e);
                }
                else if (e.Action == PointerActions.Released)
                {
                    OnTabItemTouchReleased(effectsView, tabItem, e);
                }
                else if (e.Action == PointerActions.Cancelled)
                {
                    ResetEffectsView(effectsView);
                }
#if WINDOWS || MACCATALYST
                else if (e.Action == PointerActions.Entered)
                {
                    if(effectsView != null)
                    {
                        effectsView.HighlightBackground = HoverBackground;
                        effectsView.ApplyEffects(SfEffects.Highlight);
                    }
                }
                else if (e.Action == PointerActions.Exited)
                {
                    HandleExited(tabItem, effectsView);
                }
#endif
            }
        }

        void ResetEffectsView(SfEffectsView? effectsView)
        {
            if (effectsView != null)
            {
                effectsView.Reset(true);
            }
        }

#if WINDOWS || MACCATALYST
        void HandleExited(SfTabItem tabItem, SfEffectsView? effectsView)
        {
#if WINDOWS
            tabItem.IsMouseMoved = true;
#endif
            if (effectsView != null)
            {
                effectsView.Reset(true);
            }
        }
#endif

        void HandlePressed(SfTabItem tabItem, SfEffectsView? effectsView, TouchEventArgs e)
        {
            HandlePressedPlatformSpecific(tabItem, e);

            if (!tabItem.IsSelected)
            {
                if (this.AnimationIsRunning("SelectionIndicatorAnimation"))
                {
                    this.AbortAnimation("SelectionIndicatorAnimation");
                }
            }

			if (EnableRippleAnimation)
			{
				effectsView?.ApplyEffects();
			}
			else
			{
				effectsView?.ApplyEffects(SfEffects.None);
			}
		}

        void HandlePressedPlatformSpecific(SfTabItem tabItem, TouchEventArgs e)
        {
#if IOS
            _touchDownPoint = e.TouchPoint;
#endif
#if IOS || WINDOWS
            tabItem.IsMouseMoved = false;
#endif
        }

        void OnTabItemTouchReleased(SfEffectsView? effectsView, SfTabItem tabItem, TouchEventArgs e)
        {
            HandleEffectsViewReset(effectsView, tabItem, e);

            if (ShouldIgnoreTouchRelease(tabItem, e))
			{
				return;
			}

			if (_tabItemTappedEventArgs != null)
            {
                HandleTabItemTapped(tabItem);
                if (_tabItemTappedEventArgs.Cancel)
                {
                    return;
                }
            }
            HandleSelectionChange(tabItem);
        }

        bool ShouldIgnoreTouchRelease(SfTabItem tabItem, TouchEventArgs e)
        {
#if WINDOWS
            if (tabItem.IsMouseMoved)
            {
                tabItem.IsMouseMoved = false;
                return true;
            }
#endif
#if IOS
            double diffX = Math.Abs(_touchDownPoint.X - e.TouchPoint.X);

            if (diffX > 5 && TabWidthMode == TabWidthMode.SizeToContent)
            {
                return true;
            }
#endif
            return false;
        }

        void HandleTabItemTapped(SfTabItem tabItem)
        {
            if (_tabItemTappedEventArgs != null)
            {
                _tabItemTappedEventArgs.TabItem = tabItem;
                RaiseTabItemTappedEvent(_tabItemTappedEventArgs);
            }
        }

        void HandleSelectionChange(SfTabItem tabItem)
        {
            if (_tabSelectionChangedEventArgs != null)
            {
                if (_tabItemsPanel!= null)
                {
                    var index = Items.IndexOf(tabItem);
                    _selectionChangingEventArgs = new SelectionChangingEventArgs
                    {
                        Index = index
                    };
                    if (index != SelectedIndex)
                    {
                        RaiseSelectionChangingEvent(_selectionChangingEventArgs);
                        if (!_selectionChangingEventArgs.Cancel)
                        {
                            SelectedIndex = index;
                        }
                    }
                }
            }
        }

		void HandleEffectsViewReset(SfEffectsView? effectsView, SfTabItem tabItem, TouchEventArgs e)
		{
			if (effectsView == null)
			{
				return;
			}

#if WINDOWS || MACCATALYST
			effectsView.Reset(true);
#else
			effectsView.Reset();
#endif

#if WINDOWS || MACCATALYST
			if (tabItem.Bounds.Contains(e.TouchPoint))
			{
				effectsView.HighlightBackground = HoverBackground;
				effectsView.ApplyEffects(SfEffects.Highlight);
			}
#endif
		}

		int GetVisibleItems()
        {
            int count = 0;
            foreach (var item in Items)
            {
                if (item != null && item.IsVisible)
                {
                    count++;
                }
            }

            return count;
        }

        void UpdateTabSelectionIndicator(TabIndicatorPlacement indicatorPlacement)
        {
            if (_tabSelectionIndicator != null)
            {
                UpdateSelectionIndicatorZIndex();
				var isHorizontal = _placement == TabBarPlacement.Top || _placement == TabBarPlacement.Bottom;
				if (isHorizontal)
				{
					_tabSelectionIndicator.HeightRequest = this.HeightRequest;
					_tabSelectionIndicator.VerticalOptions = LayoutOptions.Fill;
					ApplyIndicatorPlacement(isHorizontal: true, indicatorPlacement);
				}
				else
				{
					_tabSelectionIndicator.WidthRequest = this.WidthRequest;
					_tabSelectionIndicator.HorizontalOptions = LayoutOptions.Fill;
					ApplyIndicatorPlacement(isHorizontal: false, indicatorPlacement);
				}
			}
        }

		void ApplyIndicatorPlacement(bool isHorizontal, TabIndicatorPlacement indicatorPlacement)
		{
			if (indicatorPlacement != TabIndicatorPlacement.Fill)
			{
				bool isEndPlacement = isHorizontal
					? indicatorPlacement == TabIndicatorPlacement.Bottom
					: indicatorPlacement == TabIndicatorPlacement.Right;

				if (_tabSelectionIndicator != null)
				{
					if (isHorizontal)
					{
						_tabSelectionIndicator.HeightRequest = this.IndicatorStrokeThickness;
						_tabSelectionIndicator.VerticalOptions = isEndPlacement ? LayoutOptions.End : LayoutOptions.Start;
					}
					else
					{
						_tabSelectionIndicator.WidthRequest = this.IndicatorStrokeThickness;
						_tabSelectionIndicator.HorizontalOptions = isEndPlacement ? LayoutOptions.End : LayoutOptions.Start;
					}
				}

				ApplyCornerRadius(isEndPlacement);
			}
			else
			{
				ApplyCornerRadius(isFill: true);
			}
		}

		void ApplyCornerRadius(bool isEndPlacement = false, bool isFill = false)
		{
			if (_roundRectangle != null)
			{
				CornerRadius cornerRadius = IndicatorCornerRadius != new CornerRadius(-1)
					? IndicatorCornerRadius
					: (isFill ? new CornerRadius(0) : isEndPlacement ? new CornerRadius(4, 4, 0, 0) : new CornerRadius(0, 0, 4, 4));

				_roundRectangle.CornerRadius = cornerRadius;
				if (_tabSelectionIndicator != null)
				{
					_tabSelectionIndicator.StrokeShape = _roundRectangle;
				}
			}
		}

		void UpdateTabSelectionIndicatorForNonFill(TabIndicatorPlacement indicatorPlacement)
        {
            if (_tabSelectionIndicator != null)
            {
                if (indicatorPlacement == TabIndicatorPlacement.Bottom)
                {
                    _tabSelectionIndicator.HeightRequest = IndicatorStrokeThickness;
                    _tabSelectionIndicator.VerticalOptions = LayoutOptions.End;
                    if (_roundRectangle != null)
                    {
                        _roundRectangle.CornerRadius = IndicatorCornerRadius != new CornerRadius(-1) ? IndicatorCornerRadius : new CornerRadius(4, 4, 0, 0);
                        _tabSelectionIndicator.StrokeShape = _roundRectangle;
                    }
                }
                else
                {
                    _tabSelectionIndicator.HeightRequest = IndicatorStrokeThickness;
                    _tabSelectionIndicator.VerticalOptions = LayoutOptions.Start;
                    if (_roundRectangle != null)
                    {
                        _roundRectangle.CornerRadius = IndicatorCornerRadius != new CornerRadius(-1) ? IndicatorCornerRadius : new CornerRadius(0, 0, 4, 4);
                        _tabSelectionIndicator.StrokeShape = _roundRectangle;
                    }
                }
            }
        }
        void UpdateOnScrollButtonEnabled()
        {
			if (_tabHeaderParentContainer == null || _tabHeaderScrollView == null)
				return;
				var isHorizontal = _placement == TabBarPlacement.Top || _placement == TabBarPlacement.Bottom;
				DetachHeaderChrome();

				_tabHeaderParentContainer.Clear();

				if (isHorizontal)
				{
					// HORIZONTAL: ◀ [ScrollView] ▶
					_tabHeaderParentContainer.RowDefinitions.Clear();
					_tabHeaderParentContainer.ColumnDefinitions.Clear();
					_tabHeaderParentContainer.ColumnDefinitions = new ColumnDefinitionCollection
					{
						new ColumnDefinition { Width = new GridLength(_arrowButtonSize) },
						new ColumnDefinition(),
						new ColumnDefinition { Width = new GridLength(_arrowButtonSize) },
					};
							_tabHeaderParentContainer.RowDefinitions = new RowDefinitionCollection
					{
						new RowDefinition { Height = GridLength.Star }
					};

					_tabHeaderScrollView.Orientation = ScrollOrientation.Horizontal;

					if (_backwardArrow != null && _forwardArrow != null)
					{
						// Reset sizes for HORIZONTAL
						_backwardArrow.HeightRequest = -1;
						_forwardArrow.HeightRequest = -1;
						_backwardArrow.WidthRequest = _arrowButtonSize;
						_forwardArrow.WidthRequest = _arrowButtonSize;

						_backwardArrow.VerticalOptions = LayoutOptions.Fill;
						_forwardArrow.VerticalOptions = LayoutOptions.Fill;
						_backwardArrow.HorizontalOptions = LayoutOptions.Center;
						_forwardArrow.HorizontalOptions = LayoutOptions.Center;

						// Arrow glyphs for horizontal + RTL
#if WINDOWS
        if (((this as IVisualElementController).EffectiveFlowDirection & EffectiveFlowDirection.RightToLeft) == EffectiveFlowDirection.RightToLeft)
        {
            _forwardArrow.ButtonArrowType = ArrowType.Backward;
            _backwardArrow.ButtonArrowType = ArrowType.Forward;
        }
        else
#endif
						{
							_backwardArrow.ButtonArrowType = ArrowType.Backward;
							_forwardArrow.ButtonArrowType = ArrowType.Forward;
						}

						// Clear any rotation from vertical fallback
						_backwardArrow.Rotation = 0;
						_forwardArrow.Rotation = 0;
					}

#if WINDOWS
            if (((this as IVisualElementController).EffectiveFlowDirection & EffectiveFlowDirection.RightToLeft) == EffectiveFlowDirection.RightToLeft)
            {
                if (_forwardArrow != null && _backwardArrow != null)
                {
                    _forwardArrow.ButtonArrowType = ArrowType.Backward;
                    _backwardArrow.ButtonArrowType = ArrowType.Forward;
                }
            }
            else
            {
                if (_forwardArrow != null && _backwardArrow != null)
                {
                    _forwardArrow.ButtonArrowType = ArrowType.Forward;
                    _backwardArrow.ButtonArrowType = ArrowType.Backward;
                }
            }
#else
					if (((this as IVisualElementController).EffectiveFlowDirection & EffectiveFlowDirection.RightToLeft) == EffectiveFlowDirection.RightToLeft)
					{
						SafeAddChildToGrid(_tabHeaderParentContainer, _backwardArrow!, 0, 2);
						SafeAddChildToGrid(_tabHeaderParentContainer, _tabHeaderScrollView, 0, 1);
						SafeAddChildToGrid(_tabHeaderParentContainer, _forwardArrow!, 0, 0);
					}
					else
#endif
					{
						SafeAddChildToGrid(_tabHeaderParentContainer, _backwardArrow!, 0, 0);
						SafeAddChildToGrid(_tabHeaderParentContainer, _tabHeaderScrollView, 0, 1);
						SafeAddChildToGrid(_tabHeaderParentContainer, _forwardArrow!, 0, 2);
					}
				}
				else
				{
					_tabHeaderParentContainer.RowDefinitions.Clear();
					_tabHeaderParentContainer.ColumnDefinitions.Clear();

					_tabHeaderParentContainer.RowDefinitions = new RowDefinitionCollection
					{
						new RowDefinition { Height = new GridLength(_arrowButtonSize) },
						new RowDefinition(),
						new RowDefinition { Height = new GridLength(_arrowButtonSize) }
					};

					_tabHeaderParentContainer.ColumnDefinitions = new ColumnDefinitionCollection
					{
						new ColumnDefinition { Width = GridLength.Star }
					};

					_tabHeaderScrollView.Orientation = ScrollOrientation.Vertical;

					if (_backwardArrow != null && _forwardArrow != null)
					{
						_backwardArrow.HeightRequest = _arrowButtonSize;
						_forwardArrow.HeightRequest = _arrowButtonSize;
						_backwardArrow.WidthRequest = -1;
						_forwardArrow.WidthRequest = -1;

						_backwardArrow.VerticalOptions = LayoutOptions.Center;
						_forwardArrow.VerticalOptions = LayoutOptions.Center;
						_backwardArrow.HorizontalOptions = LayoutOptions.Center;
						_forwardArrow.HorizontalOptions = LayoutOptions.Center;

						_forwardArrow.ButtonArrowType = ArrowType.Forward;
						_backwardArrow.ButtonArrowType = ArrowType.Backward;

						_backwardArrow.Rotation = 0;
						_forwardArrow.Rotation = 0;

					}

					SafeAddChildToGrid(_tabHeaderParentContainer, _backwardArrow, 0, 0);
					SafeAddChildToGrid(_tabHeaderParentContainer, _tabHeaderScrollView, 1, 0);
					SafeAddChildToGrid(_tabHeaderParentContainer, _forwardArrow, 2, 0);
				}
				UpdateScrollButtonState();
        }

		void SafeAddChildToGrid(Grid grid, View? child, int row = 0, int column = 0)
		{
			if (child == null || grid == null)
				return;

			if (child.Parent is Layout oldParent && oldParent != grid)
			{
				oldParent.Children.Remove(child);
			}

			if (grid.Children.Contains(child))
			{
				grid.Children.Remove(child);
			}

			Grid.SetRow(child, row);
			Grid.SetColumn(child, column);
			grid.Children.Add(child);
		}

		void DetachHeaderChrome()
		{
			if (_tabHeaderParentContainer != null)
			{
				if (_backwardArrow != null && _tabHeaderParentContainer.Children.Contains(_backwardArrow))
					_tabHeaderParentContainer.Children.Remove(_backwardArrow);

				if (_forwardArrow != null && _tabHeaderParentContainer.Children.Contains(_forwardArrow))
					_tabHeaderParentContainer.Children.Remove(_forwardArrow);

				if (_tabHeaderScrollView != null && _tabHeaderParentContainer.Children.Contains(_tabHeaderScrollView))
					_tabHeaderParentContainer.Children.Remove(_tabHeaderScrollView);
			}

			if (_tabHeaderContentContainer != null)
			{
				if (_backwardArrow != null && _tabHeaderContentContainer.Children.Contains(_backwardArrow))
					_tabHeaderContentContainer.Children.Remove(_backwardArrow);

				if (_forwardArrow != null && _tabHeaderContentContainer.Children.Contains(_forwardArrow))
					_tabHeaderContentContainer.Children.Remove(_forwardArrow);
			}

			void DetachFromParent(View? view)
			{
				if (view?.Parent is Layout layout)
					layout.Children.Remove(view);
			}

			DetachFromParent(_backwardArrow);
			DetachFromParent(_forwardArrow);
			DetachFromParent(_tabHeaderScrollView);
		}

        void UpdateScrollButtonDisabled()
        {
			var isHorizontal = _placement == TabBarPlacement.Top || _placement == TabBarPlacement.Bottom;

			if (_tabHeaderParentContainer == null || _tabHeaderScrollView == null)
				return;
				this._tabHeaderParentContainer.Clear();

				if (isHorizontal)
				{
					_tabHeaderParentContainer.RowDefinitions.Clear();
					_tabHeaderParentContainer.ColumnDefinitions.Clear();
					_tabHeaderParentContainer.ColumnDefinitions = new ColumnDefinitionCollection
					{
						new ColumnDefinition { Width = GridLength.Star }
					};
					_tabHeaderParentContainer.RowDefinitions = new RowDefinitionCollection
					{
						new RowDefinition { Height = GridLength.Star }
					};

					_tabHeaderScrollView.Orientation = ScrollOrientation.Horizontal;
					SafeAddChildToGrid(_tabHeaderParentContainer, _tabHeaderScrollView, 0, 0);
				}
				else
				{
					_tabHeaderParentContainer.RowDefinitions.Clear();
					_tabHeaderParentContainer.ColumnDefinitions.Clear();

					_tabHeaderParentContainer.RowDefinitions = new RowDefinitionCollection
						{
							new RowDefinition { Height = GridLength.Star }
						};
					_tabHeaderParentContainer.ColumnDefinitions = new ColumnDefinitionCollection
						{
							new ColumnDefinition { Width = GridLength.Star }
						};

					_tabHeaderScrollView.Orientation = ScrollOrientation.Vertical;


					SafeAddChildToGrid(_tabHeaderParentContainer, _tabHeaderScrollView, 0, 0);
				}
        }

        void DeselectOldItem(int oldIndex)
        {
            // Deselect the old item
            if (oldIndex >= 0 && Items != null && Items.Count > oldIndex)
            {
                var item = Items[oldIndex];
                if (item != null)
                {
                    item.IsSelected = false;
                    if (item.IsDescriptionNotSetByUser && SemanticProperties.GetDescription(item).Equals("Selected " + item.Header, StringComparison.Ordinal))
                    {
                        SemanticProperties.SetDescription(item, item.Header);
                    }
                }
            }
        }

        void SelectNewItem(int newIndex)
        {
            // Select the new item
            if (newIndex >= 0 && Items != null && Items.Count > newIndex)
            {
                var item = Items[newIndex];
                item.IsSelected = true;
                if (item.IsDescriptionNotSetByUser && SemanticProperties.GetDescription(item).Equals(item.Header, StringComparison.Ordinal))
                {
                    SemanticProperties.SetDescription(item, "Selected " + item.Header);
                }
                if (IsLoaded)
                {
                    SemanticScreenReader.Announce(SemanticProperties.GetDescription(item));
                }
            }
        }

        void ConfigureTabItemProperties(SfTabItem tabItem)
        {
            if (tabItem != null)
            {
                // Configure tab item properties
                tabItem.FlowDirection = FlowDirection;
                tabItem.TabWidthMode = TabWidthMode;
                tabItem.HeaderHorizontalTextAlignment = HeaderHorizontalTextAlignment;
                tabItem.IndicatorPlacement = IndicatorPlacement;
                tabItem.HeaderDisplayMode = HeaderDisplayMode;
				tabItem.Header = tabItem.Header ?? string.Empty;
				tabItem.Touched += OnTabItemTouched;
                tabItem.PropertyChanged += OnTabItemPropertyChanged;
                tabItem.IsDescriptionNotSetByUser = String.IsNullOrEmpty(SemanticProperties.GetDescription(tabItem));
                tabItem.FontAutoScalingEnabled = FontAutoScalingEnabled;

                // Set semantic properties for accessibility
                if (tabItem.IsDescriptionNotSetByUser)
                {
                    SemanticProperties.SetDescription(tabItem, tabItem.Header);
                }
            }
        }

        double CalculateWidthWithImageAndText(SfTabItem item, Size desiredSize)
        {
			_tabHeaderImageSize = CalculateTabHeaderImageSize(item.ImageSize);
			double width;
            if (item.HeaderDisplayMode == TabBarDisplayMode.Image)
            {
                width = _tabHeaderImageSize;
			}
            else if (item.HeaderDisplayMode == TabBarDisplayMode.Text)
            {
                width = desiredSize.Width;
            }
            else
            {
                if (item.ImagePosition == TabImagePosition.Left || item.ImagePosition == TabImagePosition.Right)
                {
                    width = desiredSize.Width + item.ImageTextSpacing + _tabHeaderImageSize;
				}
                else
                {
                    width = Math.Max(desiredSize.Width, _tabHeaderImageSize);
                }
            }
            return width;
        }

        void CalculateTabItemWidthForCustomMode()
        {
			var isHorizontal = _placement == TabBarPlacement.Top || _placement == TabBarPlacement.Bottom;
			foreach (var item in Items)
            {
                if (item != null)
                {
					_tabHeaderImageSize = CalculateTabHeaderImageSize(item.ImageSize);
					if (item.IsSelected)
                    {
                        item.HeaderDisplayMode = TabBarDisplayMode.Default;
                    }
                    else
                    {
                        item.HeaderDisplayMode = TabBarDisplayMode.Image;
                    }

                    if (item.Header != null)
                    {
                        Size desiredSize = item.Header.Measure((float)item.FontSize, item.FontAttributes, item.FontFamily);
                        double extent = isHorizontal? desiredSize.Width + HeaderItemSpacing : desiredSize.Height + HeaderItemSpacing;
                        ApplyItemExtent(item, item.IsSelected ? extent : _tabHeaderImageSize + HeaderItemSpacing);
                    }
                }
            }
        }

        void CalculateTabItemWidthForSizeToContent()
        {
			Size desiredSize;
#if ANDROID || IOS
                    float _displayScale = (float)DeviceDisplay.MainDisplayInfo.Density;
#endif
			// Determine orientation once before iterating. This mirrors the .NET MAUI SfTabView
			// implementation (CalculateTabItemWidth -> UpdateItemsExtent) where the orientation
			// is resolved outside the loop so every item is sized for the active placement
			// (Top/Bottom -> horizontal extent, Left/Right -> vertical extent).
			var isHorizontal = _placement == TabBarPlacement.Top || _placement == TabBarPlacement.Bottom;
			bool isVertical = !isHorizontal;

			foreach (var item in Items)
            {
                if (item != null)
                {
					desiredSize = item.Header.Measure((float)item.FontSize, item.FontAttributes, item.FontFamily);
#if ANDROID || IOS
                            if (this.FontAutoScalingEnabled)
                            {
                                desiredSize = (item.Header.Measure((float)item.FontSize, item.FontAttributes, item.FontFamily)* _displayScale);
                            }
#endif
					// Apply extent for every item based on the resolved orientation.
					// Previously this method contained an early `return` for the vertical
					// case which caused only the first item to be sized, leaving the rest
					// with stale extents and producing incorrect spacing in Left/Right
					// placements (and on dynamic TabWidthMode / TabBarPlacement changes).
					CalculateTabItemWidthForSizeToContentForItem(item, desiredSize, isVertical);
				}
            }
        }

        void CalculateTabItemWidthForSizeToContentForItem(SfTabItem item, Size desiredSize, bool isVertical)
        {
			_tabHeaderImageSize = CalculateTabHeaderImageSize(item.ImageSize);
			var extent = 0d;
			if (item.IsVisible)
            {
                var hasImage = HasValidImageSource(item.ImageSource);
                if (item.HeaderContent != null)
                {
                    Size headerContentSize = item.HeaderContent.Measure(double.PositiveInfinity, double.PositiveInfinity);
                    extent = isVertical ? headerContentSize.Height + HeaderItemSpacing : headerContentSize.Width + HeaderItemSpacing;
                }
                else if (hasImage && !string.IsNullOrEmpty(item.Header))
                {
                    extent = GetWidthForSizeToContentWithImageAndText(item, desiredSize, isVertical);
                }
                else if (hasImage)
                {
                    item.HeaderDisplayMode = TabBarDisplayMode.Image;
                    extent = _tabHeaderImageSize + HeaderItemSpacing;
                }
                else
                {
                    item.HeaderDisplayMode = TabBarDisplayMode.Text;
                    extent = isVertical ? desiredSize.Height + HeaderItemSpacing : desiredSize.Width + HeaderItemSpacing;
                }
                ApplyItemExtent(item, extent);
            }
        }

        double GetWidthForSizeToContentWithImageAndText(SfTabItem item, Size desiredSize, bool isVertical)
        {
			_tabHeaderImageSize = CalculateTabHeaderImageSize(item.ImageSize);
			if (HeaderDisplayMode == TabBarDisplayMode.Image)
            {
                item.HeaderDisplayMode = TabBarDisplayMode.Image;
                return _tabHeaderImageSize + HeaderItemSpacing;
            }
            else if (HeaderDisplayMode == TabBarDisplayMode.Text)
            {
                item.HeaderDisplayMode = TabBarDisplayMode.Text;
				// Use vertical (Height) when stacked left/right, horizontal (Width) when top/bottom.
				var size = isVertical ? desiredSize.Height + HeaderItemSpacing : desiredSize.Width + HeaderItemSpacing;
				return size;
            }
            else
            {
                if (item.ImagePosition == TabImagePosition.Left || item.ImagePosition == TabImagePosition.Right)
                {
					// Match .NET MAUI SfTabView semantics: text extent uses Height for vertical
					// placements and Width for horizontal placements.
					var size = isVertical ? desiredSize.Height + HeaderItemSpacing + _tabHeaderImageSize + item.ImageTextSpacing : desiredSize.Width + HeaderItemSpacing + _tabHeaderImageSize + item.ImageTextSpacing;

					return size;
                }
                else
                {
					var size = isVertical ? desiredSize.Height + HeaderItemSpacing : desiredSize.Width + HeaderItemSpacing;
					return size;
                }
            }
        }

        void InsertHeaderItemToContent(SfGrid touchEffectGrid, int index)
        {
            if (_tabItemsPanel!= null)
            {
                if (index >= 0)
                {
                    _tabItemsPanel.Children.Insert(index, touchEffectGrid);
                }
                else
                {
                    _tabItemsPanel.Children.Add(touchEffectGrid);
                }

				AddCenterButtonViewPlaceholder();
			}
        }

		/// <summary>
		/// Insert the center button view placeholder into the tab header content at the calculated middle position.
		/// </summary>
		void AddCenterButtonViewPlaceholder()
		{
			UpdateCenterButtonPlaceholder();
		}

		void UpdateCenterButtonPlaceholder()
		{
			if (_tabItemsPanel is null)
			{
				return;
			}

			bool shouldShowPlaceholder = IsCenterButtonEnabled && TabWidthMode is TabWidthMode.Default;
			bool hasPlaceholder = _tabItemsPanel.Children.Contains(_centerButtonViewPlaceholder);
			if (!shouldShowPlaceholder)
			{
				if (hasPlaceholder)
				{
					_tabItemsPanel.Children.Remove(_centerButtonViewPlaceholder);
				}

				return;
			}

			if (hasPlaceholder)
			{
				_tabItemsPanel.Children.Remove(_centerButtonViewPlaceholder);
			}

			int totalCount = ItemsSource is not null && ItemsSource.Count > 0
				? ItemsSource.Count
				: GetVisibleItems();
			int leftItemsCount = totalCount / 2 + totalCount % 2;
			if (totalCount > 0 && leftItemsCount <= _tabItemsPanel.Count)
			{
				_tabItemsPanel.Children.Insert(leftItemsCount, _centerButtonViewPlaceholder);
			}
		}

		void CalculateTabItemWidthForDefaultWidthMode()
        {
			var isHorizontal = _placement == TabBarPlacement.Top || _placement == TabBarPlacement.Bottom;

			double width = 0;
			double extent = 0;

			int visibleItemsCount = GetVisibleItems();

			if (isHorizontal)
			{
				if (IsCenterButtonEnabled && TabWidthMode is TabWidthMode.Default && _centerButtonViewPlaceholder is not null && WidthRequest > 0)
				{
					double totalAvailableWidth = WidthRequest - _centerButtonViewPlaceholder.WidthRequest;
					var (leftItemWidth, rightItemWidth, leftItemsCount) = CalculateLeftRightItemsWidth(totalAvailableWidth, visibleItemsCount);
					for (int i = 0; i < Items.Count; i++)
					{
						width = i < leftItemsCount ? leftItemWidth : rightItemWidth;
						SetHeaderDisplayMode(Items[i]);
						ApplyItemExtent(Items[i], width);
					}
				}
				else
				{
					width = WidthRequest / visibleItemsCount;
					if (width > 0)
					{
						foreach (var item in Items)
						{
							if (item != null && item.IsVisible)
							{
								SetHeaderDisplayMode(item);
								ApplyItemExtent(item, width);
							}
						}
					}
				}
			}
			else
			{
				if (IsCenterButtonEnabled && TabWidthMode is TabWidthMode.Default && _centerButtonViewPlaceholder is not null && HeightRequest > 0)
				{
					double totalAvailableHeight = HeightRequest - _centerButtonViewPlaceholder.HeightRequest;
					var (topItemHeight, bottomItemHeight, topItemsCount) = CalculateTopBottomItemsHeight(totalAvailableHeight, visibleItemsCount);
					for (int i = 0; i < Items.Count; i++)
					{
						extent = i < topItemsCount ? topItemHeight : bottomItemHeight;
						SetHeaderDisplayMode(Items[i]);
						ApplyItemExtent(Items[i], extent);
					}
				}
				else
				{
					extent = HeightRequest / visibleItemsCount;
					if (extent > 0)
					{
						foreach (var item in Items)
						{
							if (item != null && item.IsVisible)
							{
								SetHeaderDisplayMode(item);
								ApplyItemExtent(item, extent);
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// Iterates through the tab header items and passes each one to AddTabViewItemFromTemplate.
		/// </summary>
		void CalculateTabItemsSourceWidthForSizeToContent()
		{
			if (_tabItemsPanel is not null)
			{
				foreach (var item in _tabItemsPanel.Children)
				{
					if (item is not null)
					{
						AddTabViewItemFromTemplate(item);
					}
				}
			}
		}

		/// <summary>
		/// Calculates and sets the width for each ItemsSource item when in the default width mode.
		/// </summary>
		void CalculateTabItemsSourceWidthForDefaultWidthMode()
		{
			var isHorizontal = _placement == TabBarPlacement.Top || _placement == TabBarPlacement.Bottom;

			if (isHorizontal)
			{
				if (ItemsSource is not null && _tabItemsPanel is not null)
				{
					double width = 0;
					int itemsSourceCount = ItemsSource.Count;

					if (IsCenterButtonEnabled && TabWidthMode is TabWidthMode.Default && _centerButtonViewPlaceholder is not null && WidthRequest > 0)
					{
						double totalAvailableWidth = WidthRequest - _centerButtonViewPlaceholder.WidthRequest;
						var (leftItemWidth, rightItemWidth, leftItemsCount) = CalculateLeftRightItemsWidth(totalAvailableWidth, itemsSourceCount);
						for (int index = 0; index < _tabItemsPanel.Children.Count; index++)
						{
							var adjustedIndex = index;
							width = index < leftItemsCount ? leftItemWidth : rightItemWidth;
							if (index == leftItemsCount)
							{
								continue;
							}

							var item = _tabItemsPanel.Children[index];
							AddTabViewItemFromTemplate(item);
							if (item is View view && TabWidthMode is TabWidthMode.Default)
							{
								view.WidthRequest = width;
							}
						}
					}
					else
					{
						foreach (var item in _tabItemsPanel.Children)
						{
							if (item is not null)
							{
								AddTabViewItemFromTemplate(item);
							}
						}
					}
				}
			}
			else
			{
				if (ItemsSource is not null && _tabItemsPanel is not null)
				{
					double height = 0;
					int itemsSourceCount = ItemsSource.Count;

					if (IsCenterButtonEnabled && TabWidthMode is TabWidthMode.Default && _centerButtonViewPlaceholder is not null && HeightRequest > 0)
					{
						double totalAvailableHeight = HeightRequest - _centerButtonViewPlaceholder.HeightRequest;
						var (topItemHeight, bottoItemHeight, topItemsCount) = CalculateTopBottomItemsHeight(totalAvailableHeight, itemsSourceCount);
						for (int index = 0; index < _tabItemsPanel.Children.Count; index++)
						{
							var adjustedIndex = index;
							height = index < topItemsCount ? topItemHeight : bottoItemHeight;
							if (index == topItemsCount)
							{
								continue;
							}

							var item = _tabItemsPanel.Children[index];
							AddTabViewItemFromTemplate(item);
							if (item is View view && TabWidthMode is TabWidthMode.Default)
							{
								view.HeightRequest = height;
							}
						}
					}
					else
					{
						foreach (var item in _tabItemsPanel.Children)
						{
							if (item is not null)
							{
								AddTabViewItemFromTemplate(item);
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// Calculates the width of items distributed into left and right sections.
		/// </summary>
		/// <param name="totalWidth">totalWidth.</param>
		/// <param name="itemCount">itemCount.</param>
		/// <returns></returns>
		(double leftItemWidth, double rightItemWidth, int leftItemsCount) CalculateLeftRightItemsWidth(double totalWidth, int itemCount)
		{
			int leftItemsCount = (itemCount / 2) + (itemCount % 2); // handle the odd count
			int rightItemsCount = itemCount / 2;
			double leftItemWidth = leftItemsCount > 0 ? totalWidth / 2 / leftItemsCount : 0;
			double rightItemWidth = rightItemsCount > 0 ? totalWidth / 2 / rightItemsCount : 0;
			return (leftItemWidth, rightItemWidth, leftItemsCount);
		}

		/// <summary>
		/// Calculates the height of items distributed into top and bottom sections.
		/// </summary>
		/// <param name="totalHeight">totalHeight.</param>
		/// <param name="itemCount">itemCount.</param>
		/// <returns></returns>
		(double topItemHeight, double bottomItemHeight, int topItemsCount) CalculateTopBottomItemsHeight(double totalHeight, int itemCount)
		{
			int topItemsCount = (itemCount / 2) + (itemCount % 2); // handle the odd count
			int bottomItemsCount = itemCount / 2;
			double topItemHeight = topItemsCount > 0 ? totalHeight / 2 / topItemsCount : 0;
			double bottomItemHeight = bottomItemsCount > 0 ? totalHeight / 2 / bottomItemsCount : 0;
			return (topItemHeight, bottomItemHeight, topItemsCount);
		}

		void SetHeaderDisplayMode(SfTabItem item)
        {
			var hasImage = HasValidImageSource(item.ImageSource);

			if (hasImage && !string.IsNullOrEmpty(item.Header))
            {
                item.HeaderDisplayMode = HeaderDisplayMode;
            }
            else if (hasImage)
            {
                item.HeaderDisplayMode = TabBarDisplayMode.Image;
            }
            else
            {
                item.HeaderDisplayMode = TabBarDisplayMode.Text;
            }
        }

        /// <summary>
        /// Updates the background color of the scroll buttons.
        /// </summary>
        void UpdateScrollButtonBackground()
        {
            if (_forwardArrow != null && _backwardArrow != null)
            {
                _forwardArrow.ScrollButtonBackground = ScrollButtonBackground;
                _backwardArrow.ScrollButtonBackground = ScrollButtonBackground;
            }
        }

		/// <summary>
		/// Updates the color of the scroll buttons.
		/// </summary>
		void UpdateScrollButtonColor()
        {
            if (_forwardArrow != null && _backwardArrow != null)
            {
                if (_forwardArrow.IsEnabled)
                {
                    _forwardArrow.ScrollButtonColor = ScrollButtonColor;
                }
                else
                {
                    _forwardArrow.ScrollButtonColor = ScrollButtonDisabledIconColor;
                }

                if (_backwardArrow.IsEnabled)
                {
                    _backwardArrow.ScrollButtonColor = ScrollButtonColor;
                }
                else
                {
                    _backwardArrow.ScrollButtonColor = ScrollButtonDisabledIconColor;
                }
            }
        }

        void ApplyItemExtent(SfTabItem item, double extent)
        {
			var isHorizontal = _placement == TabBarPlacement.Top || _placement == TabBarPlacement.Bottom;
			if (isHorizontal)
			{
				item.WidthRequest = extent;
				if (item.Parent != null && (item.Parent is SfGrid grid))
				{
					var parentGrid = grid;
					parentGrid.WidthRequest = item.WidthRequest;
				}
			}
			else
			{
				item.HeightRequest = extent;
				if (item.Parent != null && (item.Parent is SfGrid grid))
				{
					var parentGrid = grid;
					parentGrid.HeightRequest = item.HeightRequest;
				}
			}
        }

        /// <summary>
        /// Update the HeaderDisplayMode value for each <see cref="SfTabItem"/>.
        /// </summary>
        void UpdateHeaderDisplayMode()
        {
            if (Items != null)
            {
                foreach (var item in Items)
                {
                    SetHeaderDisplayMode(item);
                }

				CalculateTabItemWidth();
				CalculateTabItemsSourceWidth();
				UpdateHeaderItemHeight();
				UpdateTabIndicatorWidth();
				if (IsScrollButtonEnabled)
				{
					UpdateScrollButtonState();
				}
			}
        }

		/// <summary>
		/// Add the effects layout to the grid.
		/// </summary>
		/// <param name="touchEffectGrid">Grid.</param>
		static void AddTouchEffects(SfGrid touchEffectGrid)
		{
			SfEffectsView effectsView = new SfEffectsView
			{
				RippleAnimationDuration = 150,
				InitialRippleFactor = 0.75
			};
			touchEffectGrid.Children.Add(effectsView);
		}

        void InitializeControl()
        {
            InitializeHeaderContainer();
            UpdateIndicatorPlacement(IndicatorPlacement);
            if (Items != null)
            {
                Items.CollectionChanged -= Items_CollectionChanged;
                Items.CollectionChanged += Items_CollectionChanged;
            }
        }

        void InitializeHeaderContainer()
        {
            // Initialize tab selection indicator
            _tabSelectionIndicator = new SfBorder()
            {
                HorizontalOptions = LayoutOptions.Start,
                StrokeThickness = 0,
            };

            // Define the shape and corner radius for the indicator
            _roundRectangle = new RoundRectangle()
            {
                CornerRadius = IndicatorCornerRadius,
            };

            _tabSelectionIndicator.StrokeShape = _roundRectangle;
            UpdateIndicatorBackground(IndicatorBackground);

			_tabHeaderRow = new SfHorizontalStackBase
			{
				HorizontalOptions = LayoutOptions.Fill,
				Spacing = 0,
#if NET10_0
                SafeAreaEdges = SafeAreaEdges.None,
#endif
			};

			_tabHeaderColumn = new SfVerticalStackBase
			{
				VerticalOptions = LayoutOptions.Fill,
				HorizontalOptions = LayoutOptions.Fill,
				Spacing = 0,
#if NET10_0
                SafeAreaEdges = SafeAreaEdges.None,
#endif
			};

			_tabItemsPanel = _tabHeaderRow;

			// Create a grid to hold the tab items and indicator
			_tabHeaderContentContainer = new SfGrid
			{
				HorizontalOptions = LayoutOptions.Fill,
				VerticalOptions = LayoutOptions.Start,
				ColumnSpacing = 0,
				RowSpacing = 0,
#if NET10_0
                SafeAreaEdges = SafeAreaEdges.None,
#endif
			};

			_tabHeaderContentContainer.Children.Add(_tabItemsPanel);
			_tabHeaderContentContainer.Children.Add(_tabSelectionIndicator);

			InitializeCenterButtonViewPlaceholder();
			InitializeTabHeaderScrollView();
            InitializeTabHeaderParentContainer();
        }

        void InitializeTabHeaderScrollView()
        {
            // Initialize the scroll view for the tab header
            _tabHeaderScrollView = new SfScrollView()
            {
                HorizontalOptions = LayoutOptions.Fill,
                Content = _tabHeaderContentContainer,
                Orientation = ScrollOrientation.Horizontal,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Never,
				VerticalScrollBarVisibility = ScrollBarVisibility.Never,
#if NET10_0
                SafeAreaEdges = SafeAreaEdges.None,
#endif
            };
            _tabHeaderScrollView.Scrolled += TabHeaderScrollView_Scrolled;

        }

        void InitializeTabHeaderParentContainer()
        {
				_tabHeaderParentContainer = new SfGrid()
				{
					ColumnDefinitions =
				{
					new ColumnDefinition(),
				},
					ColumnSpacing = 0,
					RowSpacing = 0,
#if NET10_0
                SafeAreaEdges = SafeAreaEdges.None,
#endif
				};
				_tabHeaderParenRoundRectangle = new RoundRectangle();
				_tabHeaderParentBorder = new SfBorder()
				{
					HorizontalOptions = LayoutOptions.Fill,
					StrokeThickness = 0,
					Stroke = Colors.Transparent,
					Content = _tabHeaderParentContainer,
#if NET10_0
                SafeAreaEdges = SafeAreaEdges.None,
#endif
				};
				_tabHeaderParentBorder.StrokeShape = _tabHeaderParenRoundRectangle;

				_backwardArrow = new ArrowIcon
				{
					ButtonArrowType = ArrowType.Backward
				};
				_backwardArrow.ScrollButtonClicked += OnScrollBackwardClicked;

				_forwardArrow = new ArrowIcon
				{
					ButtonArrowType = ArrowType.Forward
				};
				_forwardArrow.ScrollButtonClicked += OnScrollForwardClicked;
				// Add the scroll view to the parent container
				_tabHeaderParentContainer.Children.Add(_tabHeaderScrollView);

				// Add parent container to the main layout
				Children.Add(_tabHeaderParentBorder);

			}

		/// <summary>
		/// Initialize the center button view placeholder.
		/// </summary>
		void InitializeCenterButtonViewPlaceholder()
		{
			_centerButtonViewPlaceholder = new SfGrid();
		}

		/// <summary>
		/// Measures and stores the width of each tab item in the tab header
		/// </summary>
		void MeasureTabItemTemplateWidth()
		{
			if (_tabItemsPanel is not null && ItemsSource is not null && ItemsSource.Count > 0 && _tabItemsPanel.Children.Count > 0)
			{
				_tabItemTemplateWidth = new();
				foreach (var item in _tabItemsPanel.Children)
				{
					double width = 0;
					if (item is View view && view is not SfGrid)
					{
						if (_isInitialLoading && view.WidthRequest != -1)
						{
							_isInitialWidthSet = true;
						}

						if (_isInitialWidthSet)
						{
							width = ((IView)view).Measure(WidthRequest, double.PositiveInfinity).Width;
						}
						else
						{
							width = ((IView)view).Measure(WidthRequest, double.PositiveInfinity).Width + 2;
						}

						_tabItemTemplateWidth.Add(width);
					}
				}
			}
		}

		void TabHeaderScrollView_Scrolled(object? sender, ScrolledEventArgs e)
        {
            UpdateScrollButtonState();
        }

        void UpdateScrollButtonState()
        {
            if (_tabHeaderScrollView != null && _backwardArrow != null && _forwardArrow != null)
            {
				var isHorizontal = _placement == TabBarPlacement.Top || _placement == TabBarPlacement.Bottom;
				double width = 0;
				var scrollViewWidth = WidthRequest - _arrowButtonSize * 2;
				double height = 0;
				var scrollViewHeight = HeightRequest - _arrowButtonSize * 2;
				if (isHorizontal)
				{
					if (TabWidthMode == TabWidthMode.SizeToContent)
					{
						scrollViewWidth = WidthRequest - (_arrowButtonSize * 2 + TabHeaderPadding.Left + TabHeaderPadding.Right);
					}

					if (_tabItemsPanel != null)
					{
						foreach (var item in _tabItemsPanel.Children)
						{
							if (!double.IsNaN(item.Width))
							{
								width += item.Width;
							}
							else
							{
								var index = _tabItemsPanel.Children.IndexOf(item);
								width += _tabItemsPanel.Children[index].Measure(double.PositiveInfinity, double.PositiveInfinity).Width;
							}
						}
					}
					UpdateScrollButtonStateBasedOnScrollPosition(scrollViewWidth, width);
				}
				else
				{
					if (TabWidthMode == TabWidthMode.SizeToContent)
					{
						scrollViewHeight = HeightRequest - (_arrowButtonSize * 2 + TabHeaderPadding.Top + TabHeaderPadding.Bottom);
					}

					if (_tabItemsPanel != null)
					{
						foreach (var item in _tabItemsPanel.Children)
						{
							if (!double.IsNaN(item.Height))
							{
								height += item.Height;
							}
							else
							{
								var index = _tabItemsPanel.Children.IndexOf(item);
								height += _tabItemsPanel.Children[index].Measure(double.PositiveInfinity, double.PositiveInfinity).Height;
							}
						}
					}
					UpdateScrollButtonStateBasedOnScrollPositionHeight(scrollViewHeight, height);
				}

                if (ScrollButtonBackground != null)
                {
                    _forwardArrow.ScrollButtonBackground = ScrollButtonBackground;
                    _backwardArrow.ScrollButtonBackground = ScrollButtonBackground;
                }
            }
        }

        void UpdateScrollButtonStateBasedOnScrollPosition(double scrollViewWidth, double width)
        {
			if (_tabHeaderScrollView != null && _backwardArrow != null && _forwardArrow != null)
            {
					if (_tabHeaderScrollView.ScrollX == 0 && width != 0 && width < scrollViewWidth)
					{
						_backwardArrow.IsEnabled = false;
						_forwardArrow.IsEnabled = false;
						_backwardArrow.ScrollButtonColor = ScrollButtonDisabledIconColor;
						_forwardArrow.ScrollButtonColor = ScrollButtonDisabledIconColor;
					}
					else if (_tabHeaderScrollView.ScrollX == 0)
					{
						_backwardArrow.IsEnabled = false;
						_forwardArrow.IsEnabled = true;
						_backwardArrow.ScrollButtonColor = ScrollButtonDisabledIconColor;
						_forwardArrow.ScrollButtonColor = ScrollButtonColor;
					}
					else if (Math.Round(_tabHeaderScrollView.ScrollX) == Math.Round(_tabHeaderScrollView.ContentSize.Width - _tabHeaderScrollView.Width))
					{
						_backwardArrow.IsEnabled = true;
						_forwardArrow.IsEnabled = false;
						_forwardArrow.ScrollButtonColor = ScrollButtonDisabledIconColor;
						_backwardArrow.ScrollButtonColor = ScrollButtonColor;
					}
					else
					{
						_backwardArrow.IsEnabled = true;
						_forwardArrow.IsEnabled = true;
						_backwardArrow.ScrollButtonColor = ScrollButtonColor;
						_forwardArrow.ScrollButtonColor = ScrollButtonColor;
					}
            }
        }

		void UpdateScrollButtonStateBasedOnScrollPositionHeight(double scrollViewHeight, double height)
		{
			if (_tabHeaderScrollView != null && _backwardArrow != null && _forwardArrow != null)
			{
				if (_tabHeaderScrollView.ScrollX == 0 && height > 0 && height <= scrollViewHeight)
				{
					_backwardArrow.IsEnabled = false;
					_forwardArrow.IsEnabled = false;
					_backwardArrow.ScrollButtonColor = ScrollButtonDisabledIconColor;
					_forwardArrow.ScrollButtonColor = ScrollButtonDisabledIconColor;
				}
				else if (_tabHeaderScrollView.ScrollY == 0)
				{
					_backwardArrow.IsEnabled = false;
					_forwardArrow.IsEnabled = true;
					_backwardArrow.ScrollButtonColor = ScrollButtonDisabledIconColor;
					_forwardArrow.ScrollButtonColor = ScrollButtonColor;
				}
				else if (Math.Round(_tabHeaderScrollView.ScrollY) == Math.Round(_tabHeaderScrollView.ContentSize.Height - _tabHeaderScrollView.Height))
				{
					_backwardArrow.IsEnabled = true;
					_forwardArrow.IsEnabled = false;
					_forwardArrow.ScrollButtonColor = ScrollButtonDisabledIconColor;
					_backwardArrow.ScrollButtonColor = ScrollButtonColor;
				}
				else
				{
					_backwardArrow.IsEnabled = true;
					_forwardArrow.IsEnabled = true;
					_backwardArrow.ScrollButtonColor = ScrollButtonColor;
					_forwardArrow.ScrollButtonColor = ScrollButtonColor;
				}
			}
		}

		void OnScrollBackwardClicked(object? sender, EventArgs e)
        {
			var isHorizontal = _placement == TabBarPlacement.Top || _placement == TabBarPlacement.Bottom;
			if (_tabHeaderScrollView != null)
            {
				if (isHorizontal)
				{
					var scrollToX = Math.Clamp(_tabHeaderScrollView.ScrollX - 100d, 0, _tabHeaderScrollView.ContentSize.Width - _tabHeaderScrollView.Width);
					_tabHeaderScrollView.ScrollToAsync(scrollToX, 0, true);
				}
				else
				{
		            var maxY = Math.Max(0, _tabHeaderScrollView.ContentSize.Height - _tabHeaderScrollView.Height);
				    var scrollToY = Math.Clamp(_tabHeaderScrollView.ScrollY - 100d, 0, maxY);
					_tabHeaderScrollView.ScrollToAsync(0, scrollToY, true);
				}
            }
        }

        void OnScrollForwardClicked(object? sender, EventArgs e)
        {
			var isHorizontal = _placement == TabBarPlacement.Top || _placement == TabBarPlacement.Bottom;
			if (_tabHeaderScrollView != null)
            {
				if (isHorizontal)
				{
					var scrollToX = Math.Clamp(_tabHeaderScrollView.ScrollX + 100d, 0, _tabHeaderScrollView.ContentSize.Width - _tabHeaderScrollView.Width);
					_tabHeaderScrollView.ScrollToAsync(scrollToX, 0, true);
				}
				else
				{
					var maxY = Math.Max(0, _tabHeaderScrollView.ContentSize.Height - _tabHeaderScrollView.Height);
					var scrollToY = Math.Clamp(_tabHeaderScrollView.ScrollY + 100d, 0, maxY);
				    _tabHeaderScrollView.ScrollToAsync(0, scrollToY, true);
				}
            }
        }

        /// <summary>
        /// Update tab indicator width based on template.
        /// </summary>
        void UpdateTabIndicatorTemplateWidth()
        {
			var isHorizontal = _placement == TabBarPlacement.Top || _placement == TabBarPlacement.Bottom;

			if (_tabSelectionIndicator != null &&
                _tabItemsPanel != null &&
                _tabItemsPanel.Children.Count > 0 &&
                SelectedIndex >= 0 &&
                _tabItemsPanel.Children.Count > SelectedIndex)
            {
				
					int adjustedIndex = SelectedIndex; // Default to the existing selected index
					if (IsCenterButtonEnabled && TabWidthMode is TabWidthMode.Default)
					{
						adjustedIndex = GetAdjustedIndexForCenterButton(adjustedIndex);
					}

					if (adjustedIndex < _tabItemsPanel.Children.Count)
					{
						if (isHorizontal)
						{
							_selectedTabX = _tabItemsPanel.Children[adjustedIndex].Frame.X;
							if (SelectedIndex >= 0)
							{
								_selectedTabX = CalculateTabXPosition();
							}
							_currentIndicatorWidth = _tabItemsPanel.Children[adjustedIndex].Measure(double.PositiveInfinity, double.PositiveInfinity).Width;
							IndicatorAnimation();
							_previousTabX = _selectedTabX;
						}
						else
						{
							_selectedTabY = _tabItemsPanel.Children[adjustedIndex].Frame.Height;
							if (SelectedIndex >= 0)
							{
								_selectedTabY = CalculateTabYPosition();
							}
						   _currentIndicatorHeight = _tabItemsPanel.Children[adjustedIndex].Measure(double.PositiveInfinity, double.PositiveInfinity).Height;
							IndicatorAnimation();
							_previousTabY = _selectedTabY;
						}
						_previewSelectedIndex = adjustedIndex;
						UpdateTabIndicatorPosition();
					}
            }
        }

        double CalculateTabXPosition()
        {
            double xPosition = 0;
            if (_tabItemsPanel != null)
            {
				int adjustedIndex = SelectedIndex;
				if (IsCenterButtonEnabled && TabWidthMode is TabWidthMode.Default)
				{
					adjustedIndex = GetAdjustedIndexForCenterButton(adjustedIndex);
				}

				for (int i = 0; i < adjustedIndex; i++)
                {
                    var size = _tabItemsPanel.Children[i].Measure(double.PositiveInfinity, double.PositiveInfinity);
                    xPosition += size.Width;
                }
            }
            return xPosition;
        }

		double CalculateTabYPosition()
			{
				double yPosition = 0;
				if (_tabItemsPanel != null && SelectedIndex >= 0)
				{
					int adjustedIndex = SelectedIndex;
					if (IsCenterButtonEnabled && TabWidthMode is TabWidthMode.Default)
					{
						adjustedIndex = GetAdjustedIndexForCenterButton(adjustedIndex);
					}

					for (int i = 0; i < adjustedIndex; i++)
					{
						var childSize = _tabItemsPanel.Children[i].Measure(_tabItemsPanel.WidthRequest, double.PositiveInfinity);
						yPosition += childSize.Height;
					}
				}
				return yPosition;
			}
		

		/// <summary>
		/// Update indicator width based on items.
		/// </summary>
		internal void UpdateTabIndicatorWidth()
        {
			if (_tabSelectionIndicator != null &&
                _tabItemsPanel != null &&
                _tabItemsPanel?.Children.Count > 0 &&
                _tabItemsPanel.Children.Count > SelectedIndex && SelectedIndex >= 0)
            {
				if (ItemsSource != null && ItemsSource.Count > 0 || HeaderItemTemplate != null)
                {
                    UpdateTabIndicatorTemplateWidth();
                }
                else
                {
					if (this._tabItemsPanel.Children[(int)this.SelectedIndex].Frame.Width > 0 || this._tabItemsPanel.Children[(int)this.SelectedIndex].Frame.Height > 0)
					{
						if (Items is not null && Items.Count > 0 && SelectedIndex < Items.Count)
						{
							UpdateTabIndicatorForSelectedItem();
						}
					}
                }
            }
        }

        void UpdateTabIndicatorForSelectedItem()
        {
            if (_tabItemsPanel == null)
			{
				return;
			}

			var isHorizontal = _placement == TabBarPlacement.Top || _placement == TabBarPlacement.Bottom;

			SfTabItem item = Items[SelectedIndex];

			if (isHorizontal)
			{
				if (SelectedIndex > 0 && _selectedTabX <= 0)
				{
					_selectedTabX = _tabItemsPanel.Children[SelectedIndex - 1].Frame.X;
				}
				if (WidthRequest > 0)
				{
					UpdateTabIndicatorWidthOnWidthRequest(item);
				}

				if (((this as IVisualElementController).EffectiveFlowDirection & EffectiveFlowDirection.RightToLeft) == EffectiveFlowDirection.RightToLeft)
				{
#if !WINDOWS
					_selectedTabX = -_selectedTabX;
#endif
				}

				IndicatorAnimation();
				_previousTabX = _selectedTabX;
				_previousIndicatorWidth = _currentIndicatorWidth;
			}
			else
			{
				if (SelectedIndex > 0 && _selectedTabY <= 0)
				{
					_selectedTabY = _tabItemsPanel.Children[SelectedIndex - 1].Frame.Y;
				}
				if (HeightRequest > 0)
				{
					UpdateTabIndicatorWidthOnWidthRequest(item);
				}

				if (((this as IVisualElementController).EffectiveFlowDirection & EffectiveFlowDirection.RightToLeft) == EffectiveFlowDirection.RightToLeft)
				{
#if !WINDOWS
					_selectedTabX = -_selectedTabX;
#endif
				}

				IndicatorAnimation();
				_previousTabY= _selectedTabY;
				_previousIndicatorHeight = _currentIndicatorHeight;
			}
			_previewSelectedIndex = SelectedIndex;
            if (_tabItemsPanel != null)
            {
                if (SelectedIndex < _tabItemsPanel.Children.Count)
                {
                    UpdateTabIndicatorPosition();
                }
            }
        }

        void UpdateTabIndicatorWidthOnWidthRequest(SfTabItem item)
        {
			var isHorizontal = _placement == TabBarPlacement.Top || _placement == TabBarPlacement.Bottom;
			if (_tabItemsPanel != null && _tabSelectionIndicator != null)
            {
					int adjustedIndex = SelectedIndex;
					if (IsCenterButtonEnabled && TabWidthMode is TabWidthMode.Default)
					{
						adjustedIndex = GetAdjustedIndexForCenterButton(SelectedIndex);
					}
				if (isHorizontal)
				{
					if (IndicatorPlacement == TabIndicatorPlacement.Fill)
					{
						_selectedTabX = _tabItemsPanel.Children[adjustedIndex].Frame.X;
						_currentIndicatorWidth = _tabItemsPanel.Children[adjustedIndex].Width;
						if (double.IsNaN(_currentIndicatorWidth))
						{
							_currentIndicatorWidth = _tabItemsPanel.Children[adjustedIndex].Measure(double.PositiveInfinity, double.PositiveInfinity).Width;
						}
					}
					else
					{
						if (IndicatorWidthMode == IndicatorWidthMode.Fit)
						{
							UpdateIndicatorForFitMode(item);
						}
						else
						{
							_selectedTabX = _tabItemsPanel.Children[adjustedIndex].Frame.X;
							_currentIndicatorWidth = _tabItemsPanel.Children[adjustedIndex].Width;
						}
					}

					if (_removedItemWidth != 0 && TabWidthMode == TabWidthMode.SizeToContent)
					{
						(_selectedTabX, _currentIndicatorWidth) = UpdateIndicatorPosition(_selectedTabX, _currentIndicatorWidth);
					}

					if (_currentIndicatorWidth > _tabSelectionIndicator.Width)
					{
						_tabSelectionIndicator.MaximumWidthRequest = _currentIndicatorWidth * 2;
					}
				}
				else
				{
					if (IndicatorPlacement == TabIndicatorPlacement.Fill)
					{
						_selectedTabY = _tabItemsPanel.Children[adjustedIndex].Frame.Y;
						_currentIndicatorHeight = _tabItemsPanel.Children[adjustedIndex].Height;
						if (double.IsNaN(_currentIndicatorHeight))
						{
							_currentIndicatorHeight = _tabItemsPanel.Children[adjustedIndex].Measure(double.PositiveInfinity, double.PositiveInfinity).Height;
						}
					}
					else
					{
						if (IndicatorWidthMode == IndicatorWidthMode.Fit)
						{
							UpdateIndicatorForFitMode(item);
						}
						else
						{
							_selectedTabY = _tabItemsPanel.Children[adjustedIndex].Frame.Y;
							_currentIndicatorHeight = _tabItemsPanel.Children[adjustedIndex].Height;
						}
					}

					if (_removedItemHeight != 0 && TabWidthMode == TabWidthMode.SizeToContent)
					{
						(_selectedTabY, _currentIndicatorHeight) = UpdateIndicatorPosition(_selectedTabY, _currentIndicatorHeight);
					}

					if (_currentIndicatorHeight > _tabSelectionIndicator.Height)
					{
						_tabSelectionIndicator.MaximumHeightRequest = _currentIndicatorHeight * 2;
					}
				}
			}
        }

		/// <summary>
		/// Updates the position of the tab indicator based on the selected tab's X position and width.
		/// </summary>
		/// <param name="selectedViewX">The X position of the selected tab item.</param>
		/// <param name="extent">The width of the tab indicator.</param>
		(double, double) UpdateIndicatorPosition(double selectedViewX, double extent)
		{
			var isHorizontal = _placement == TabBarPlacement.Top || _placement == TabBarPlacement.Bottom;

			if (isHorizontal)
			{
				if (_tabItemsPanel != null)
				{
					selectedViewX -= _removedItemWidth;
					_removedItemWidth = 0;
				}

				return (selectedViewX, extent);
			}
			else
			{
				if (_tabItemsPanel != null)
				{ 
					selectedViewX -= _removedItemHeight;
					_removedItemHeight = 0;
				}

				return (selectedViewX, extent);
			}
		}

		 bool TryGetSelectedChild(out IView? child)
		{
			child = null;
			if (_tabItemsPanel == null)
				return false;

			int count = _tabItemsPanel.Children.Count;
			int selctedIndex = (int)SelectedIndex;

			if (count <= 0)
				return false;
			if (selctedIndex < 0 || selctedIndex >= count)
				return false;

			child = _tabItemsPanel.Children[selctedIndex];
			return child != null;
		}

		void UpdateIndicatorForFitMode(SfTabItem item)
        {
			var isHorizontal = _placement == TabBarPlacement.Top || _placement == TabBarPlacement.Bottom;
			if (_tabItemsPanel == null)
			{
				return;
			}

			if (GetVisibleItems() == 0)
			{
				if (isHorizontal)
				{
					_currentIndicatorWidth = 0.1;
				}
				else
				{
					_currentIndicatorHeight = 0.1;
				}
			}
			else
            {
				if (isHorizontal)
				{
					_currentIndicatorWidth = MeasureHeaderContentWidth(item);
					UpdateCurrentIndicatorWidth(item);
				}
				else
				{
					_currentIndicatorHeight = MeasureHeaderContentHeight(item);
					UpdateCurrentIndicatorHeight(item);
				}
                
            }
        }

        void UpdateCurrentIndicatorWidth(SfTabItem item)
        {
            if (_tabItemsPanel!=null)
            {
				int adjustedIndex = SelectedIndex; // Default to the existing selected index

				if (IsCenterButtonEnabled && TabWidthMode is TabWidthMode.Default)
				{
					adjustedIndex = GetAdjustedIndexForCenterButton(SelectedIndex);
				}

				if (item.HeaderHorizontalTextAlignment == TextAlignment.Start && item.HeaderContent is null)
                {
                    _selectedTabX = (_tabItemsPanel.Children[adjustedIndex].Frame.Left);
                }
                else if (item.HeaderHorizontalTextAlignment == TextAlignment.End && item.HeaderContent is null)
                {
                    _selectedTabX = (_tabItemsPanel.Children[adjustedIndex].Frame.Right) - _currentIndicatorWidth;
                }
                else
                {
                    _selectedTabX = (_tabItemsPanel.Children[adjustedIndex].Frame.Center.X) - ((_currentIndicatorWidth / 2));
				}

#if ANDROID
				var itemsCount = GetVisibleItems();

				if (IsCenterButtonEnabled && TabWidthMode is TabWidthMode.Default && (_isItemRemovedWithCenterButton || _isTabItemPropertyChanged) && itemsCount % 2 == 0 && SelectedIndex == Items.Count - 1)
				{
					double totalWidth = 0;
					for (int index = 0; index < _tabItemsPanel.Count - 1; index++)
					{
						totalWidth += _tabItemsPanel.Children[index].Width;
					}

					if (_selectedTabX < totalWidth)
					{
						_selectedTabX += _tabItemsPanel.Children[adjustedIndex - 1].Width;
					}

					_isTabItemPropertyChanged = false;
				}
#endif

				if (_currentIndicatorWidth > _tabItemsPanel.Children[adjustedIndex].Width)
                {
                    _selectedTabX = _tabItemsPanel.Children[adjustedIndex].Frame.X;
                    _currentIndicatorWidth = _tabItemsPanel.Children[adjustedIndex].Width;
                }
            }
        }

		void UpdateCurrentIndicatorHeight(SfTabItem item)
		{
			if (_tabItemsPanel != null)
			{
				int adjustedIndex = SelectedIndex; // Default to the existing selected index

				if (IsCenterButtonEnabled && TabWidthMode is TabWidthMode.Default)
				{
					adjustedIndex = GetAdjustedIndexForCenterButton(SelectedIndex);
				}

				var yPosition = CalculateTabYPosition();
				_currentIndicatorHeight = MeasureHeaderContentHeight(item);
				_selectedTabY = yPosition + (_tabItemsPanel.Children[adjustedIndex].Frame.Height/2 - (_currentIndicatorHeight / 2));

#if ANDROID
				var itemsCount = GetVisibleItems();

				if (IsCenterButtonEnabled && TabWidthMode is TabWidthMode.Default && (_isItemRemovedWithCenterButton || _isTabItemPropertyChanged) && itemsCount % 2 == 0 && SelectedIndex == Items.Count - 1)
				{
					double totalHeight = 0;
					for (int index = 0; index < _tabItemsPanel.Count - 1; index++)
					{
						totalHeight += _tabItemsPanel.Children[index].Height;
					}

					if (_selectedTabY < totalHeight)
					{
						_selectedTabY += _tabItemsPanel.Children[adjustedIndex - 1].Height;
					}

					_isTabItemPropertyChanged = false;
				}
#endif

				if (_currentIndicatorHeight > _tabItemsPanel.Children[adjustedIndex].Height)
				{
					_selectedTabY = _tabItemsPanel.Children[adjustedIndex].Frame.Y;
					_currentIndicatorHeight = _tabItemsPanel.Children[adjustedIndex].Height;
				}
			}
		}

		/// <summary>
		/// Method to animate the selection indicator in Tab View.
		/// </summary>
		void IndicatorAnimation()
        {
			var isHorizontal = _placement == TabBarPlacement.Top || _placement == TabBarPlacement.Bottom;
			if (isHorizontal)
			{
				double previousIndicatorWidth = 0;
				if (_tabSelectionIndicator != null)
				{
					previousIndicatorWidth = _tabSelectionIndicator.Width;
				}

				previousIndicatorWidth = previousIndicatorWidth < 0 ? 0 : previousIndicatorWidth;
				int adjustedIndex = SelectedIndex;
				if (IsCenterButtonEnabled && TabWidthMode is TabWidthMode.Default)
				{
					adjustedIndex = GetAdjustedIndexForCenterButton(adjustedIndex);
				}

				var selectedItem = _tabItemsPanel?.Children[adjustedIndex];
				double selectedItemWidth = (selectedItem != null) ?
					selectedItem.Measure(double.PositiveInfinity, double.PositiveInfinity).Width : 0;

				double widthMultiplier = CalculateWidthMultiplier(previousIndicatorWidth, selectedItemWidth);
				uint animationDuration = (uint)ContentTransitionDuration;

				var firstWidthAnimation = new Animation(FirstWidthAnimatedMethod, previousIndicatorWidth, previousIndicatorWidth * widthMultiplier);
				var secondWidthAnimation = new Animation(SecondWidthAnimatedMethod, previousIndicatorWidth * widthMultiplier, _currentIndicatorWidth);

				var translateAnimation = new Animation();
				double targetX = _selectedTabX;
				if (_tabSelectionIndicator != null)
				{
#if !WINDOWS
					if (HeaderItemTemplate != null &&
						((this as IVisualElementController).EffectiveFlowDirection & EffectiveFlowDirection.RightToLeft) == EffectiveFlowDirection.RightToLeft)
					{
						targetX = -_selectedTabX;
					}
#endif

					translateAnimation = new Animation(TranslateAnimatedMethod, _tabSelectionIndicator.TranslationX, targetX);
				}

				var parentAnimation = new Animation
			{
				{ 0, 0.5, firstWidthAnimation },
				{ 0.5, 1, secondWidthAnimation },
				{ 0, 1, translateAnimation }
			};
				AnimateTabSelectionIndicator(parentAnimation, animationDuration, targetX);
			}
			else
			{
				double previousIndicatorHeight = 0;
				if (_tabSelectionIndicator != null)
				{
					previousIndicatorHeight = _tabSelectionIndicator.Height;
				}

				previousIndicatorHeight = previousIndicatorHeight < 0 ? 0 : previousIndicatorHeight;
				int adjustedIndex = SelectedIndex;
				if (IsCenterButtonEnabled && TabWidthMode is TabWidthMode.Default)
				{
					adjustedIndex = GetAdjustedIndexForCenterButton(adjustedIndex);
				}

				var selectedItem = _tabItemsPanel?.Children[adjustedIndex];
				double selectedItemHeight = (selectedItem != null) ?
					selectedItem.Measure(double.PositiveInfinity, double.PositiveInfinity).Height : 0;

				double heightMultiplier = CalculateHeightMultiplier(previousIndicatorHeight, selectedItemHeight);
				uint animationDuration = (uint)ContentTransitionDuration;

				var firstHeightAnimation = new Animation(FirstHeightAnimatedMethod, previousIndicatorHeight, previousIndicatorHeight * heightMultiplier);
				var secondHeightAnimation = new Animation(SecondHeightAnimatedMethod, previousIndicatorHeight * heightMultiplier, _currentIndicatorHeight);

				var translateAnimation = new Animation();
				double targetY = _selectedTabY;
				if (_tabSelectionIndicator != null)
				{
					translateAnimation = new Animation(TranslateAnimatedMethod, _tabSelectionIndicator.TranslationY, targetY);
				}

				var parentAnimation = new Animation
			{
				{ 0, 0.5, firstHeightAnimation },
				{ 0.5, 1, secondHeightAnimation },
				{ 0, 1, translateAnimation }
			};
				AnimateTabSelectionIndicator(parentAnimation, animationDuration, targetY);
			}
        }

        double CalculateWidthMultiplier(double previousIndicatorWidth, double selectedItemWidth)
        {
            double widthMultiplier = selectedItemWidth / _currentIndicatorWidth;
            if (_currentIndicatorWidth > previousIndicatorWidth)
            {
                widthMultiplier = (int)(selectedItemWidth / _currentIndicatorWidth);
                widthMultiplier = widthMultiplier < 1 ? widthMultiplier + 1 : widthMultiplier;
            }

            if (_currentIndicatorWidth < previousIndicatorWidth)
            {
                widthMultiplier = (int)(selectedItemWidth / previousIndicatorWidth);
                widthMultiplier = widthMultiplier < 1 ? widthMultiplier + 1 : widthMultiplier;

                if (previousIndicatorWidth / _currentIndicatorWidth > 2 && IndicatorWidthMode == IndicatorWidthMode.Stretch)
                {
                    widthMultiplier *= 0.5;
                }
            }

            widthMultiplier = widthMultiplier > 2 ? 2 : widthMultiplier;
            return widthMultiplier;
        }

		double CalculateHeightMultiplier(double previousIndicatorHeight, double selectedItemHeight)
		{
			double heightMultiplier = selectedItemHeight / _currentIndicatorHeight;
			if (_currentIndicatorHeight > previousIndicatorHeight)
			{
				heightMultiplier = (int)(selectedItemHeight / _currentIndicatorHeight);
				heightMultiplier = heightMultiplier < 1 ? heightMultiplier + 1 : heightMultiplier;
			}

			if (_currentIndicatorHeight < previousIndicatorHeight)
			{
				heightMultiplier = (int)(selectedItemHeight / previousIndicatorHeight);
				heightMultiplier = heightMultiplier < 1 ? heightMultiplier + 1 : heightMultiplier;

				if (previousIndicatorHeight / _currentIndicatorHeight > 2 && IndicatorWidthMode == IndicatorWidthMode.Stretch)
				{
					heightMultiplier *= 0.5;
				}
			}

			heightMultiplier = heightMultiplier > 2 ? 2 : heightMultiplier;
			return heightMultiplier;
		}

		void AnimateTabSelectionIndicator(Animation parentAnimation, uint animationDuration, double targetX)
        {
			var isHorizontal = _placement == TabBarPlacement.Top || _placement == TabBarPlacement.Bottom;
			if (isHorizontal)
			{
				if (_tabSelectionIndicator != null)
				{
					if (ShouldPerformAction())
					{
						parentAnimation.Commit(this, "SelectionIndicatorAnimation", 16, animationDuration, AnimationEasing, finished: (v, e) =>
						{
#if WINDOWS || ANDROID
                        if (_tabSelectionIndicator.TranslationX == targetX)
#endif
							{
								UpdateFillSelectedTabItem();
							}
						});
					}
					else
					{
						if (!this.AnimationIsRunning("SelectionIndicatorAnimation") || (this.AnimationIsRunning("SelectionIndicatorAnimation") && (_previousTabX != _selectedTabX)))
						{
							if (!double.IsNaN(_currentIndicatorWidth))
							{
								_tabSelectionIndicator.WidthRequest = _currentIndicatorWidth;
							}

							_tabSelectionIndicator.Measure(_tabSelectionIndicator.WidthRequest, _tabSelectionIndicator.HeightRequest);
							_tabSelectionIndicator.TranslationX = targetX;
							UpdateFillSelectedTabItem();
							// need to check If below code required or not.
							int adjustedIndex = SelectedIndex;
							if (IsCenterButtonEnabled && TabWidthMode is TabWidthMode.Default)
							{
								adjustedIndex = GetAdjustedIndexForCenterButton(adjustedIndex);
							}

							UpdateScrollViewPosition(adjustedIndex);
						}
					}
				}
			}
			else
			{
				if (_tabSelectionIndicator != null)
				{
					if (ShouldPerformAction())
					{
						parentAnimation.Commit(this, "SelectionIndicatorAnimation", 16, animationDuration, AnimationEasing, finished: (v, e) =>
						{
#if WINDOWS || ANDROID
                        if (_tabSelectionIndicator.TranslationY == targetX)
#endif
							{
								UpdateFillSelectedTabItem();
							}
						});
					}
					else
					{
						if (!this.AnimationIsRunning("SelectionIndicatorAnimation") || (this.AnimationIsRunning("SelectionIndicatorAnimation") && (_previousTabY != _selectedTabY)))
						{
							if (!double.IsNaN(_currentIndicatorHeight))
							{
								_tabSelectionIndicator.HeightRequest = _currentIndicatorHeight;
							}

							_tabSelectionIndicator.Measure(_tabSelectionIndicator.WidthRequest, _tabSelectionIndicator.HeightRequest);
							_tabSelectionIndicator.TranslationY = targetX;
							UpdateFillSelectedTabItem();
							// need to check If below code required or not.
							int adjustedIndex = SelectedIndex;
							if (IsCenterButtonEnabled && TabWidthMode is TabWidthMode.Default)
							{
								adjustedIndex = GetAdjustedIndexForCenterButton(adjustedIndex);
							}

							UpdateScrollViewPosition(adjustedIndex);
						}
					}
				}
			}
        }

		/// <summary>
		/// Determines whether an action should be performed based on the change in selected index, running animations, indicator dimensions, and the position of the selected view.
		/// </summary>
		/// <returns>Returns true if the conditions for performing the action are met; otherwise, returns false.</returns>
		bool ShouldPerformAction()
		{
			var isAnimationRunning = this.AnimationIsRunning("SelectionIndicatorAnimation");
			var isHorizontal = _placement == TabBarPlacement.Top || _placement == TabBarPlacement.Bottom;

			if (isHorizontal)
			{

#if !WINDOWS
				var isSelectedViewXConditionMet = this._selectedTabX >= 0 ||
					((((this as IVisualElementController).EffectiveFlowDirection & EffectiveFlowDirection.RightToLeft) == EffectiveFlowDirection.RightToLeft) && _selectedTabX < 0);
#else
    var isSelectedViewXConditionMet = _selectedTabX > 0;
#endif

				var hasIndexChanged = _previewSelectedIndex != SelectedIndex;
				var isIndicatorDimensionChanged = _previousIndicatorWidth != _currentIndicatorWidth;
				var isItemCountChanged = _tabItemsPanel is not null && _tabItemsPanel.Count != GetVisibleItems();
				var isPositionChanged = _previousTabX != _selectedTabX;

				return (hasIndexChanged || (isAnimationRunning && (isIndicatorDimensionChanged || isItemCountChanged || isPositionChanged)))
					   && isSelectedViewXConditionMet;
			}
			else
			{

#if !WINDOWS
				var isSelectedViewYConditionMet = this._selectedTabY >= 0 ||
					((((this as IVisualElementController).EffectiveFlowDirection & EffectiveFlowDirection.RightToLeft) == EffectiveFlowDirection.RightToLeft) && _selectedTabY < 0);
#else
    var isSelectedViewYConditionMet = _selectedTabY > 0;
#endif

				var hasIndexChanged = _previewSelectedIndex != SelectedIndex;
				var isIndicatorDimensionChanged = _previousIndicatorHeight != _currentIndicatorHeight;
				var isItemCountChanged = _tabItemsPanel is not null && _tabItemsPanel.Count != GetVisibleItems();
				var isPositionChanged = _previousTabY != _selectedTabY;

				return (hasIndexChanged || (isAnimationRunning && (isIndicatorDimensionChanged || isItemCountChanged || isPositionChanged)))
					   && isSelectedViewYConditionMet;
			}
		}

		/// <summary>
		/// This method is used to update the fill for the selected tab item.
		/// </summary>
		void UpdateFillSelectedTabItem()
        {
            if (IndicatorPlacement == TabIndicatorPlacement.Fill)
            {
                if (!(ItemsSource != null && ItemsSource.Count > 0 || HeaderItemTemplate != null))
                {
                    var tabItem = Items[SelectedIndex];
                    if (tabItem.IsSelected)
                    {
                        tabItem.ChangeSelectedState();
                    }
                }
            }
        }

        /// <summary>
        /// This is update the tab selection indicator width
        /// </summary>
        void SecondWidthAnimatedMethod(double value)
        {
            UpdateTabSelectionIndicatorWidth(value);
        }

        /// <summary>
        /// This is update the tab selection indicator width
        /// </summary>
        /// <param name="value"></param>
        void FirstWidthAnimatedMethod(double value)
        {
            UpdateTabSelectionIndicatorWidth(value);
        }

        void UpdateTabSelectionIndicatorWidth(double value)
        {
            if (_tabSelectionIndicator != null)
            {
                _tabSelectionIndicator.WidthRequest = value;
                if (_tabSelectionIndicator.WidthRequest >= 0)
                {
                    _tabSelectionIndicator.Measure(_tabSelectionIndicator.WidthRequest, _tabSelectionIndicator.HeightRequest);
                }
            }
        }

		/// <summary>
		/// This is update the tab selection indicator width
		/// </summary>
		void SecondHeightAnimatedMethod(double value)
		{
			UpdateTabSelectionIndicatorHeight(value);
		}

		/// <summary>
		/// This is update the tab selection indicator width
		/// </summary>
		/// <param name="value"></param>
		void FirstHeightAnimatedMethod(double value)
		{
			UpdateTabSelectionIndicatorHeight(value);
		}

		void UpdateTabSelectionIndicatorHeight(double value)
		{
			if (_tabSelectionIndicator != null)
			{
				_tabSelectionIndicator.HeightRequest = value;
				if (_tabSelectionIndicator.HeightRequest >= 0)
				{
					_tabSelectionIndicator.Measure(_tabSelectionIndicator.WidthRequest, _tabSelectionIndicator.HeightRequest);
				}
			}
		}

		/// <summary>
		/// This is update the tab selection indicator x position.
		/// </summary>
		/// <param name="value"></param>
#if MACCATALYST || IOS
        async void TranslateAnimatedMethod(double value)
#else
		void TranslateAnimatedMethod(double value)
#endif
        {
			var isHorizontal = _placement == TabBarPlacement.Top || _placement == TabBarPlacement.Bottom;
			if (_tabSelectionIndicator != null)
            {
#if MACCATALYST || IOS
                //Delay is used for transition: On Mac and iOS, the transition animation is not functioning properly and does not behave like it does on Windows and Android platforms.
                await Task.Delay(150);
#endif
				if (isHorizontal)
				{
					_tabSelectionIndicator.TranslationX = value;
				}
				else
				{
					_tabSelectionIndicator.TranslationY = value;
				}
            }
        }

        void ResetStyle()
        {
            Style = new Style(typeof(SfStackLayout));

            if (_tabHeaderScrollView != null)
            {
                _tabHeaderScrollView.Style = new Style(typeof(SfScrollView));
            }

            if (_tabHeaderContentContainer != null)
            {
                _tabHeaderContentContainer.Style = new Style(typeof(SfGrid));
            }

            if (_tabItemsPanel != null)
            {
                _tabItemsPanel.Style = new Style(typeof(SfHorizontalStackBase));
            }

            if (_tabSelectionIndicator != null)
            {
                _tabSelectionIndicator.Style = new Style(typeof(SfBorder));
            }
        }

        void Items_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            // Handle old items being removed
            var headerItemsCount = _tabItemsPanel?.Children.Count;
            if (e.OldItems != null)
            {
#if ANDROID
				_isItemRemovedWithCenterButton = IsCenterButtonEnabled && TabWidthMode is TabWidthMode.Default;
#endif
				foreach (SfTabItem tabItem in e.OldItems)
                {
                    ClearHeaderItem(tabItem, e.OldStartingIndex);
                }

                CalculateTabItemWidth();

            }

            // Handle new items being added
            if (e.NewItems != null)
            {
                foreach (SfTabItem tabItem in e.NewItems)
                {
                    if (tabItem != null)
                    {
                        var index = Items.IndexOf(tabItem);
						if (IsCenterButtonEnabled && TabWidthMode is TabWidthMode.Default)
						{
							index = GetAdjustedIndexForCenterButton(index);
						}

						AddTabItemProperties(tabItem, index);
						if (SelectedIndex >= index &&
                    SelectedIndex + 1 < Items.Count &&
                    headerItemsCount != _tabItemsPanel?.Children.Count)
                        {
                            SelectedIndex += 1;
                        }
                    }
                }
            }
            else
            {
                ClearItems();
            }

#if ANDROID
			if (_isItemRemovedWithCenterButton)
			{
				_isItemRemovedWithCenterButton = false;
			}
#endif
		}

        void ClearItems()
        {
            if (Items == null || Items.Count == 0)
            {
                _tabItemsPanel?.Children.Clear();
                if (_tabSelectionIndicator != null)
                {
                    _tabSelectionIndicator.WidthRequest = 0.01d;
                }
            }
        }

        void UpdateIndicatorBackground(Brush indicatorBackground)
        {
            if (_tabSelectionIndicator != null)
            {
                if (indicatorBackground is SolidColorBrush solidBrush)
                {
                    _tabSelectionIndicator.BackgroundColor = solidBrush.Color;
                }
                else
                {
                    _tabSelectionIndicator.Background = indicatorBackground;
                }
            }
        }

		 void UpdateTabHeaderAlignment()
		 {
			if (_tabHeaderParentContainer is not null && _tabHeaderScrollView is not null)
			{
				var isHorizontal = _placement == TabBarPlacement.Top || _placement == TabBarPlacement.Bottom;
				if (TabWidthMode is TabWidthMode.SizeToContent && isHorizontal)
				{
					if (TabHeaderAlignment == TabHeaderAlignment.Start)
					{
#if ANDROID
						_tabHeaderScrollView.HorizontalOptions = LayoutOptions.Start;
#endif
						_tabHeaderParentContainer.HorizontalOptions = LayoutOptions.Start;
					}

					else if (TabHeaderAlignment == TabHeaderAlignment.Center)
					{
#if ANDROID
						_tabHeaderScrollView.HorizontalOptions = LayoutOptions.Center;
#endif
						_tabHeaderParentContainer.HorizontalOptions = LayoutOptions.Center;
					}

					else
					{
#if ANDROID
						_tabHeaderScrollView.HorizontalOptions = LayoutOptions.End;
#endif
						_tabHeaderParentContainer.HorizontalOptions = LayoutOptions.End;
					}
				}
				else if (TabWidthMode == TabWidthMode.SizeToContent && !isHorizontal && _tabHeaderContentContainer !=null)
				{
					if (TabHeaderAlignment == TabHeaderAlignment.Start)
					{
#if ANDROID
						_tabHeaderScrollView.VerticalOptions = LayoutOptions.Start;
#endif
						_tabHeaderContentContainer.VerticalOptions = LayoutOptions.Start;
					}
					else if (TabHeaderAlignment == TabHeaderAlignment.Center)
					{
#if ANDROID
						_tabHeaderScrollView.VerticalOptions = LayoutOptions.Center;
#endif
						_tabHeaderContentContainer.VerticalOptions = LayoutOptions.Center;
					}
					else
					{
#if ANDROID
						_tabHeaderScrollView.VerticalOptions = LayoutOptions.End;
#endif
						_tabHeaderContentContainer.VerticalOptions = LayoutOptions.End;
					}
				}
				else
				{
#if ANDROID
					if (_tabHeaderScrollView is not null)
					{
						if (isHorizontal)
						{
							_tabHeaderScrollView.HorizontalOptions = LayoutOptions.Fill;
						}
						else
						{
							_tabHeaderScrollView.VerticalOptions = LayoutOptions.Fill;
						}
					}
#endif
					if (isHorizontal)
					{
						_tabHeaderParentContainer.HorizontalOptions = LayoutOptions.Fill;
					}
					else
					{
						_tabHeaderParentContainer.VerticalOptions = LayoutOptions.Fill;
					}
				}
			}
			UpdateCenterButtonPlaceholder();
		 }

		void UpdateTabWidthMode()
        {
            UpdateTabPadding();
			UpdateHeaderDisplayMode();
			UpdateTabHeaderAlignment();
			UpdateIsCenterButtonEnabled();
		}

        /// <summary>
        /// This method once the tab width mode property is changed.
        /// </summary>
        void UpdateIndicatorWidthMode()
        {
            UpdateTabIndicatorWidth();
        }

        void UpdateHeaderHorizontalAlignment()
        {
            foreach (var tabItem in Items)
            {
                tabItem.HeaderHorizontalTextAlignment = HeaderHorizontalTextAlignment;
                tabItem.TabWidthMode = TabWidthMode;
            }
            UpdateTabIndicatorWidth();
        }

		/// <summary>
		/// Calculates the tab header image size based on the item's ImageSize.
		/// </summary>
		/// <param name="imageSize">The ImageSize of the tab item.</param>
		/// <returns>The calculated tab header image size.</returns>
		double CalculateTabHeaderImageSize(double imageSize)
		{
			return imageSize * 4 / 3;
		}

		/// <summary>
		/// Adjusts the index of a tab item to account for the presence of a center button in the tab bar.
		/// </summary>
		/// <param name="index">The original index of the tab item.</param>
		/// <returns>The adjusted index accounting for the center button.</returns>
		int GetAdjustedIndexForCenterButton(int index)
		{
			if (IsCenterButtonEnabled && TabWidthMode is TabWidthMode.Default)
			{
				int totalItems = 0;
				// Calculate the left items count
				if (ItemsSource is not null)
				{
					totalItems = ItemsSource.Count;
				}
				else
				{
					totalItems = GetVisibleItems();
				}
				 
				int leftItemsCount = totalItems / 2 + totalItems % 2; // Left should have one more when total is odd

				// If SelectedIndex is at the position of the center button, adjust the index
				if (index >= leftItemsCount)
				{
					index++;
				}
			}

			return index;
		}

		#endregion

		#region Override Methods

		/// <summary>
		/// Measure Override method
		/// </summary>
		/// <param name="widthConstraint">Width</param>
		/// <param name="heightConstraint">Height</param>
		/// <returns>It returns the size</returns>
		protected override Size MeasureOverride(double widthConstraint, double heightConstraint)
        {
            if (widthConstraint > 0 && heightConstraint > 0 &&
                widthConstraint != double.PositiveInfinity &&
                heightConstraint != double.PositiveInfinity)
            {
                if (HeightRequest != heightConstraint || WidthRequest != widthConstraint)
                {
                    HeightRequest = heightConstraint;
                    WidthRequest = widthConstraint;
					if (!IsCenterButtonEnabled || (IsCenterButtonEnabled && TabWidthMode is TabWidthMode.SizeToContent) || (_isInitialLoading && TabWidthMode is TabWidthMode.Default))
					{
						MeasureTabItemTemplateWidth();
						_isInitialLoading = false;
					}

					CalculateTabItemWidth();
					CalculateTabItemsSourceWidth();
                    UpdateHeaderItemHeight();
                    UpdateTabIndicatorWidth();
                    if (IsScrollButtonEnabled)
                    {
                        UpdateScrollButtonState();
                    }
                }
            }
            return base.MeasureOverride(widthConstraint, heightConstraint);
        }

        /// <summary>
        /// Handler changed method.
        /// </summary>
        protected override void OnHandlerChanged()
        {
			if (Handler == null)
			{
				if (Items != null)
				{
					foreach (var item in Items)
					{
						item.Touched -= OnTabItemTouched;
					}
				}
			}

			else
			{
				if (Items != null)
				{
					foreach (var item in Items)
					{
						item.Touched -= OnTabItemTouched;
						item.Touched += OnTabItemTouched;
					}
				}
			}
			
			base.OnHandlerChanged();
		}

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();
			if (Items is not null)
			{
				foreach (var item in Items)
				{
					if (item is SfTabItem tabItem && tabItem is not null && tabItem.HeaderContent is not null)
					{
						tabItem.HeaderContent.BindingContext = tabItem.BindingContext;
					}
				}
			}
		}

		#endregion

        #region Property Changed Methods

        static void OnIndicatorPlacementChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as SfTabBar)?.UpdateIndicatorPlacement((TabIndicatorPlacement)newValue);

        static void OnTabWidthModePropertyChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as SfTabBar)?.UpdateTabWidthMode();

        static void OnHeaderHorizontalAlignmentChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as SfTabBar)?.UpdateHeaderHorizontalAlignment();

        static void OnIndicatorWidthModePropertyChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as SfTabBar)?.UpdateIndicatorWidthMode();

        static void OnTabHeaderPaddingPropertyChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as SfTabBar)?.UpdateTabHeaderPadding();

        static void OnIndicatorBackgroundChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as SfTabBar)?.UpdateIndicatorBackground((Brush)newValue);

		static void OnTabHeaderAlignmentPropertyChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as SfTabBar)?.UpdateTabHeaderAlignment();
		
        static void OnItemsChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as SfTabBar)?.UpdateItems();

        static void OnHeaderTemplateChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as SfTabBar)?.UpdateItemsSource();

        static void OnHeaderItemSpacingChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as SfTabBar)?.UpdateTabHeaderItemSpacing();

        static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as SfTabBar)?.UpdateItemsSource();

        static void OnSelectedIndexChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as SfTabBar)?.UpdateSelectedIndex((int)newValue, (int)oldValue);

		static void OnScrollButtonEnabledChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var tabBar = bindable as SfTabBar;
			tabBar?.UpdateScrollButtonEnabled((bool)newValue);
			tabBar?.UpdateTabIndicatorWidth();
			tabBar?.UpdateHeaderItemHeight();
#if IOS
             tabBar?.UpdateTabPadding();
#endif
		}
		static void OnIsCenterButtonEnabledChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as SfTabBar)?.UpdateIsCenterButtonEnabled();

		static void OnHeaderDisplayModeChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as SfTabBar)?.UpdateHeaderDisplayMode();

        static void OnIndicatorCornerRadiusChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as SfTabBar)?.UpdateCornerRadius();

		static void OnIndicatorStrokeThicknessChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as SfTabBar)?.UpdateIndicatorStrokeThickness((double)newValue);

		static void OnFontAutoScalingEnabledChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as SfTabBar)?.UpdateFontAutoScalingEnabled((Boolean)newValue);

        static void OnScrollButtonBackgroundChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as SfTabBar)?.UpdateScrollButtonBackground();

        static void OnScrollButtonColorChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as SfTabBar)?.UpdateScrollButtonColor();

        static void OnScrollButtonDisabledForegroundColorChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as SfTabBar)?.UpdateScrollButtonColor();
		
		#endregion

		#region Interface Implementation

        void ITapGestureListener.OnTap(TapEventArgs e)
        {
            // This method is intentionally left empty as it is required by interface
        }

		#endregion

		#region Events

		/// <summary>
		/// Occurs when the selection changed.
		/// </summary>
		public event EventHandler<CoreEventArgs>? SelectionChanged;

        /// <summary>
        /// Occurs when a tab item is tapped.
        /// </summary>
        public event EventHandler<TabItemTappedEventArgs>? TabItemTapped;

        /// <summary>
        /// Occurs when the selection is changing.
        /// </summary>
        public event EventHandler<SelectionChangingEventArgs>? SelectionChanging;

		#endregion
	}
}