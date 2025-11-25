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
        SfHorizontalStackLayout? _tabHeaderItemContent;
        RoundRectangle? _roundRectangle;

		// Dimension and positioning fields

		readonly int _defaultTextPadding = 36;
        double _previousTabX = 0d;
        double _selectedTabX = 0d;
        double _currentIndicatorWidth = 0d;
		double _tabHeaderImageSize = 32d;
		readonly double _arrowButtonSize = 32;
		double _removedItemWidth = 0;
		private Size desiredSize;

        // State-tracking fields

        double _previousIndicatorWidth = 0d;
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
                new Thickness(52, 0, 52, 0),
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

		#endregion

		#region Public Methods

		/// <summary>
		/// To get the effects view for the tab item.
		/// </summary>
		/// <param name="tabItem">The tab item for which the effects view is to be retrieved.</param>
		/// <returns>It returns the effectsView.</returns>
		public SfEffectsView? GetEffectsView(SfTabItem tabItem)
        {
            if (tabItem != null && _tabHeaderItemContent != null)
            {
                var index = Items.IndexOf(tabItem);
                if (index >= 0)
                {
					if (IsCenterButtonEnabled && TabWidthMode is TabWidthMode.Default)
					{
						index = GetAdjustedIndexForCenterButton(index);
					}

					var child = _tabHeaderItemContent.Children[index];
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

			var index = _tabHeaderItemContent?.Children.IndexOf((IView)(sender));
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
                // Ensure tempIndicatorWidth is non-negative
                if (_currentIndicatorWidth >= 0)
                {
					int adjustedIndex = SelectedIndex; // Default to the existing selected index
					if (IsCenterButtonEnabled && TabWidthMode is TabWidthMode.Default)
					{
						adjustedIndex = GetAdjustedIndexForCenterButton(adjustedIndex);
					}

					// Update the scroll view position based on the currently selected index
					UpdateScrollViewPosition(adjustedIndex);
				}
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

			if (this._tabSelectionIndicator != null && indicatorPlacement != TabIndicatorPlacement.Fill)
			{
				this._tabSelectionIndicator.HeightRequest = this.IndicatorStrokeThickness;
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
				_tabHeaderItemContent?.Children.RemoveAt(removeItemIndex);
				AddCenterButtonViewPlaceholder();
				if (Items.Count > 0 && SelectedIndex >= Items.Count && index==SelectedIndex)
                {
                    SelectedIndex = Items.Count - 1;
                }
				else if (index <= SelectedIndex)
				{
					_removedItemWidth = tabItem.Width;

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
			if (IsCenterButtonEnabled && TabWidthMode is TabWidthMode.Default)
			{
				AddCenterButtonViewPlaceholder();
			}
			else
			{
				if (_tabHeaderItemContent is not null && _tabHeaderItemContent.Children.Contains(_centerButtonViewPlaceholder))
				{
					_tabHeaderItemContent.Children.Remove(_centerButtonViewPlaceholder);
				}
			}

			CalculateTabItemWidth();
			CalculateTabItemsSourceWidth();
			if (ItemsSource is not null && ItemsSource.Count > 0 && (!IsCenterButtonEnabled || IsCenterButtonEnabled && TabWidthMode is TabWidthMode.SizeToContent))
			{
				if (_tabHeaderItemContent is not null)
				{
					for (int index = 0; index < _tabHeaderItemContent.Count; index++)
					{
						if (_tabHeaderItemContent.Children[index] is not null && _tabItemTemplateWidth is not null && _tabItemTemplateWidth.Count > 0 && _tabItemTemplateWidth[index] > 0 && _tabHeaderItemContent.Children[index] is View view)
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
            _tabHeaderItemContent?.Children.Clear();
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
            if (_tabHeaderItemContent != null && _tabHeaderItemContent.Height != HeightRequest)
            {
                _tabHeaderItemContent.HeightRequest = HeightRequest;
                CalculateTabItemWidth();
                if (_tabSelectionIndicator != null && IndicatorPlacement == TabIndicatorPlacement.Fill)
                {
                    _tabSelectionIndicator.HeightRequest = HeightRequest;
                }

                if (_tabHeaderContentContainer != null)
                {
                    _tabHeaderContentContainer.HeightRequest = _tabHeaderItemContent.HeightRequest;
                }

                if (_tabHeaderScrollView != null)
                {
                    _tabHeaderScrollView.HeightRequest = _tabHeaderItemContent.HeightRequest;
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
            if (item != null)
            {
                item.VerticalOptions = LayoutOptions.Fill;
                item.HorizontalOptions = LayoutOptions.Center;
            }

            SfGrid touchEffectGrid = new SfGrid
            {
                Style = new Style(typeof(SfGrid))
            };
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
						if (item.ImageSource != null && !string.IsNullOrEmpty(item.Header))
						{
							width = CalculateWidthWithImageAndText(item, desiredSize);
						}
						else if (item.ImageSource != null)
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
                _tabHeaderItemContent != null &&
                _tabHeaderItemContent.Children.Count > position &&
                (TabWidthMode != TabWidthMode.Default ||
                (ItemsSource != null && ItemsSource.Count > 0 || HeaderItemTemplate != null)))
            {
                int rtlPosition = _tabHeaderItemContent.Children.Count - position - 1;
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
            if (HeaderItemTemplate == null && _tabHeaderScrollView != null &&
                _tabHeaderItemContent != null)
            {
                double totalWidth = 0;
                foreach (var child in _tabHeaderItemContent.Children)
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
                            await _tabHeaderScrollView.ScrollToAsync(_tabHeaderItemContent?.Children[rtlPosition] as Element, ScrollToPosition.MakeVisible, true).ConfigureAwait(false);
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
                            await _tabHeaderScrollView.ScrollToAsync(_tabHeaderItemContent?.Children[position] as Element, ScrollToPosition.MakeVisible, true).ConfigureAwait(false);
                        }
                    }
                }
            }
        }

#elif ANDROID

        async void UpdateScrollViewPositionForAndroid(int position, int rtlPosition)
        {
            if (_tabHeaderScrollView == null)
			{
				return;
			}

			if (((this as IVisualElementController).EffectiveFlowDirection & EffectiveFlowDirection.RightToLeft) == EffectiveFlowDirection.RightToLeft)
            {
                await _tabHeaderScrollView.ScrollToAsync(_tabHeaderItemContent?.Children[rtlPosition] as Element, ScrollToPosition.Center, true).ConfigureAwait(false);
            }
            else
            {
                await _tabHeaderScrollView.ScrollToAsync(_tabHeaderItemContent?.Children[position] as Element, ScrollToPosition.Center, true).ConfigureAwait(false);
            }
        }

#else

        async void UpdateScrollViewPositionForWindows(int position, int rtlPosition)
        {
            if (_tabHeaderScrollView == null)
			{
				return;
			}

			await _tabHeaderScrollView.ScrollToAsync(_tabHeaderItemContent?.Children[position] as Element, ScrollToPosition.Center, true).ConfigureAwait(false);
        }
#endif

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

        void UpdateTabPadding()
        {
            if (_tabHeaderContentContainer != null)
            {
                _tabHeaderContentContainer.Padding = TabWidthMode == TabWidthMode.Default ? new Thickness(0) : TabHeaderPadding;
            }

            CalculateTabItemWidth();
			CalculateTabItemsSourceWidth();
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
                BindableLayout.SetItemsSource(_tabHeaderItemContent, ItemsSource);
                BindableLayout.SetItemTemplate(_tabHeaderItemContent, HeaderItemTemplate);
                if (_tabHeaderItemContent != null)
                {
					if (IsCenterButtonEnabled && TabWidthMode is TabWidthMode.Default)
					{
						AddCenterButtonViewPlaceholder();
						CalculateTabItemsSourceWidthForDefaultWidthMode();
					}
					else
					{
						foreach (var item in _tabHeaderItemContent.Children)
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
            var headerItemsCount = _tabHeaderItemContent?.Children?.Count;

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

				if (IsCenterButtonEnabled && TabWidthMode is TabWidthMode.Default && _tabHeaderItemContent is not null)
				{
					_tabHeaderItemContent.Children.Clear();
					BindableLayout.SetItemsSource(_tabHeaderItemContent, null);
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
				if (IsCenterButtonEnabled && TabWidthMode is TabWidthMode.Default && _tabHeaderItemContent is not null)
				{
					_tabHeaderItemContent.Children.Clear();
					BindableLayout.SetItemsSource(_tabHeaderItemContent, null);
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
								headerItemsCount != _tabHeaderItemContent?.Children.Count)
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
                if (_tabHeaderItemContent != null)
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
                _tabSelectionIndicator.HeightRequest = HeightRequest;
                _tabSelectionIndicator.VerticalOptions = LayoutOptions.Fill;
                if (indicatorPlacement != TabIndicatorPlacement.Fill)
                {
                    UpdateTabSelectionIndicatorForNonFill(indicatorPlacement);
                }
                else
                {
                    if (_roundRectangle != null)
                    {
                        _roundRectangle.CornerRadius = IndicatorCornerRadius != new CornerRadius(-1) ? IndicatorCornerRadius : new CornerRadius(0);
                        _tabSelectionIndicator.StrokeShape = _roundRectangle;
                    }
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
            if (_tabHeaderParentContainer != null)
            {
                _tabHeaderParentContainer.Clear();
                _tabHeaderParentContainer.ColumnDefinitions.Clear();
                _tabHeaderParentContainer.ColumnDefinitions =
					[
						new ColumnDefinition { Width = new GridLength(_arrowButtonSize) },
                        new ColumnDefinition(),
                        new ColumnDefinition { Width = new GridLength(_arrowButtonSize) },
                    ];

                ConfigureArrowButtonTypes();

                _tabHeaderParentContainer.Children.Add(_backwardArrow);
                _tabHeaderParentContainer.Children.Add(_tabHeaderScrollView);
                _tabHeaderParentContainer.Children.Add(_forwardArrow);
                UpdateScrollButtonState();
            }
        }

        void ConfigureArrowButtonTypes()
        {
            if (_tabHeaderParentContainer == null)
			{
				return;
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
                _tabHeaderParentContainer.SetColumn(_backwardArrow, 2);
                _tabHeaderParentContainer.SetColumn(_tabHeaderScrollView, 1);
                _tabHeaderParentContainer.SetColumn(_forwardArrow, 0);
            }
            else
#endif
            {
                _tabHeaderParentContainer.SetColumn(_backwardArrow, 0);
                _tabHeaderParentContainer.SetColumn(_tabHeaderScrollView, 1);
                _tabHeaderParentContainer.SetColumn(_forwardArrow, 2);
            }
        }

        void UpdateScrollButtonDisabled()
        {
            if (_tabHeaderParentContainer != null)
            {
                _tabHeaderParentContainer.Clear();
                _tabHeaderParentContainer.ColumnDefinitions.Clear();
                _tabHeaderParentContainer.ColumnDefinitions =
					[
						new ColumnDefinition(),
                    ];
                _tabHeaderParentContainer.Children.Add(_tabHeaderScrollView);
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
                        double width = desiredSize.Width + _defaultTextPadding;
                        UpdateTabItemWidth(item, item.IsSelected ? width : _tabHeaderImageSize + _defaultTextPadding);
                    }
                }
            }
        }

        void CalculateTabItemWidthForSizeToContent()
        {
#if ANDROID || IOS
                    float _displayScale = (float)DeviceDisplay.MainDisplayInfo.Density;
#endif
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
					CalculateTabItemWidthForSizeToContentForItem(item, desiredSize);
                }
            }
        }

        void CalculateTabItemWidthForSizeToContentForItem(SfTabItem item, Size desiredSize)
        {
			_tabHeaderImageSize = CalculateTabHeaderImageSize(item.ImageSize);
			if (item.IsVisible)
            {
                double width;

				if (item.HeaderContent != null)
				{
					Size headerContentSize = item.HeaderContent.Measure(double.PositiveInfinity, double.PositiveInfinity);
					width = headerContentSize.Width + _defaultTextPadding;
				}
				else if (item.ImageSource != null && !string.IsNullOrEmpty(item.Header))
                {
                    width = GetWidthForSizeToContentWithImageAndText(item, desiredSize);
                }
                else if (item.ImageSource != null)
                {
                    item.HeaderDisplayMode = TabBarDisplayMode.Image;
                    width = _tabHeaderImageSize + _defaultTextPadding;
                }
                else
                {
                    item.HeaderDisplayMode = TabBarDisplayMode.Text;
                    width = desiredSize.Width + _defaultTextPadding;
                }
                UpdateTabItemWidth(item, width);
            }
        }

        double GetWidthForSizeToContentWithImageAndText(SfTabItem item, Size desiredSize)
        {
			_tabHeaderImageSize = CalculateTabHeaderImageSize(item.ImageSize);
			if (HeaderDisplayMode == TabBarDisplayMode.Image)
            {
                item.HeaderDisplayMode = TabBarDisplayMode.Image;
                return _tabHeaderImageSize + _defaultTextPadding;
            }
            else if (HeaderDisplayMode == TabBarDisplayMode.Text)
            {
                item.HeaderDisplayMode = TabBarDisplayMode.Text;
                return desiredSize.Width + _defaultTextPadding;
            }
            else
            {
                if (item.ImagePosition == TabImagePosition.Left || item.ImagePosition == TabImagePosition.Right)
                {
                    return desiredSize.Width + _defaultTextPadding + _tabHeaderImageSize + item.ImageTextSpacing;
                }
                else
                {
                    return desiredSize.Width + _defaultTextPadding;
                }
            }
        }

        void InsertHeaderItemToContent(SfGrid touchEffectGrid, int index)
        {
            if (_tabHeaderItemContent != null)
            {
                if (index >= 0)
                {
                    _tabHeaderItemContent.Children.Insert(index, touchEffectGrid);
                }
                else
                {
                    _tabHeaderItemContent.Children.Add(touchEffectGrid);
                }

				AddCenterButtonViewPlaceholder();
			}
        }

		/// <summary>
		/// Insert the center button view placeholder into the tab header content at the calculated middle position.
		/// </summary>
		void AddCenterButtonViewPlaceholder()
		{
			if (IsCenterButtonEnabled && TabWidthMode is TabWidthMode.Default && _tabHeaderItemContent is not null)
			{
				int totalCount = 0;
				if (ItemsSource is not null && ItemsSource.Count > 0)
				{
					totalCount = ItemsSource.Count;
				}
				else
				{
					totalCount = GetVisibleItems();
				}

				// Find or calculate correct positions for left and right items
				int leftItemsCount = totalCount / 2 + totalCount % 2;
				if (_tabHeaderItemContent.Children.Contains(_centerButtonViewPlaceholder))
				{
					_tabHeaderItemContent.Children.Remove(_centerButtonViewPlaceholder);
				}

				if (totalCount > 0 && leftItemsCount <= _tabHeaderItemContent.Count)
				{
					_tabHeaderItemContent.Children.Insert(leftItemsCount, _centerButtonViewPlaceholder);
				}
			}
		}

		void CalculateTabItemWidthForDefaultWidthMode()
        {
			double width = 0;
			int visibleItemsCount = GetVisibleItems();

			if (IsCenterButtonEnabled && TabWidthMode is TabWidthMode.Default && _centerButtonViewPlaceholder is not null && WidthRequest > 0)
			{
				double totalAvailableWidth = WidthRequest - _centerButtonViewPlaceholder.WidthRequest;
				var (leftItemWidth, rightItemWidth, leftItemsCount) = CalculateLeftRightItemsWidth(totalAvailableWidth, visibleItemsCount);
				for (int i = 0; i < Items.Count; i++)
				{
					width = i < leftItemsCount ? leftItemWidth : rightItemWidth;
					SetHeaderDisplayMode(Items[i]);
					UpdateTabItemWidth(Items[i], width);
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
							UpdateTabItemWidth(item, width);
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
			if (_tabHeaderItemContent is not null)
			{
				foreach (var item in _tabHeaderItemContent.Children)
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
			if (ItemsSource is not null && _tabHeaderItemContent is not null)
			{
				double width = 0;
				int itemsSourceCount = ItemsSource.Count;

				if (IsCenterButtonEnabled && TabWidthMode is TabWidthMode.Default && _centerButtonViewPlaceholder is not null && WidthRequest > 0)
				{
					double totalAvailableWidth = WidthRequest - _centerButtonViewPlaceholder.WidthRequest;
					var (leftItemWidth, rightItemWidth, leftItemsCount) = CalculateLeftRightItemsWidth(totalAvailableWidth, itemsSourceCount);
					for (int index = 0; index < _tabHeaderItemContent.Children.Count; index++)
					{
						var adjustedIndex = index;
						width = index < leftItemsCount ? leftItemWidth : rightItemWidth;
						if (index == leftItemsCount)
						{
							continue;
						}

						var item = _tabHeaderItemContent.Children[index];
						AddTabViewItemFromTemplate(item);
						if (item is View view && TabWidthMode is TabWidthMode.Default)
						{
							view.WidthRequest = width;
						}
					}
				}
				else
				{
					foreach (var item in _tabHeaderItemContent.Children)
					{
						if (item is not null)
						{
							AddTabViewItemFromTemplate(item);
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


		void SetHeaderDisplayMode(SfTabItem item)
        {
            if (item.ImageSource != null && !string.IsNullOrEmpty(item.Header))
            {
                item.HeaderDisplayMode = HeaderDisplayMode;
            }
            else if (item.ImageSource != null)
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

        static void UpdateTabItemWidth(SfTabItem item, double width)
        {
            item.WidthRequest = width;
            if (item.Parent != null && (item.Parent is SfGrid grid))
            {
                var parentGrid = grid;
                parentGrid.WidthRequest = item.WidthRequest;
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
                    if (item.ImageSource != null && !string.IsNullOrEmpty(item.Header))
                    {
                        item.HeaderDisplayMode = HeaderDisplayMode;
                    }
                }

                UpdateTabIndicatorWidth();
                CalculateTabItemWidth();
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

            // Initialize the horizontal stack layout for tab items
            _tabHeaderItemContent = new SfHorizontalStackLayout()
            {
                HorizontalOptions = LayoutOptions.Fill,
				Spacing = 0,
			};

            // Create a grid to hold the tab items and indicator
            _tabHeaderContentContainer = new SfGrid()
            {
                HorizontalOptions = LayoutOptions.Fill,
                Children = { _tabHeaderItemContent, _tabSelectionIndicator },
				ColumnSpacing = 0,
				RowSpacing = 0,
			};

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
			};
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
            Children.Add(_tabHeaderParentContainer);

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
			if (_tabHeaderItemContent is not null && ItemsSource is not null && ItemsSource.Count > 0 && _tabHeaderItemContent.Children.Count > 0)
			{
				_tabItemTemplateWidth = new();
				foreach (var item in _tabHeaderItemContent.Children)
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
                double width = 0;
                var scrollViewWidth = WidthRequest - _arrowButtonSize * 2;
                if (TabWidthMode == TabWidthMode.SizeToContent)
                {
                    scrollViewWidth = WidthRequest - (_arrowButtonSize * 2 + TabHeaderPadding.Left + TabHeaderPadding.Right);
                }

                if (_tabHeaderItemContent != null)
                {
                    foreach (var item in _tabHeaderItemContent.Children)
                    {
                        if (!double.IsNaN(item.Width))
                        {
                            width += item.Width;
                        }
                        else
                        {
                            var index = _tabHeaderItemContent.Children.IndexOf(item);
                            width += _tabHeaderItemContent.Children[index].Measure(double.PositiveInfinity, double.PositiveInfinity).Width;
                        }
                    }
                }

                UpdateScrollButtonStateBasedOnScrollPosition(scrollViewWidth, width);

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

        void OnScrollBackwardClicked(object? sender, EventArgs e)
        {
            if (_tabHeaderScrollView != null)
            {
                var scrollToX = Math.Clamp(_tabHeaderScrollView.ScrollX - 100d, 0, _tabHeaderScrollView.ContentSize.Width - _tabHeaderScrollView.Width);
                _tabHeaderScrollView.ScrollToAsync(scrollToX, 0, true);
            }
        }

        void OnScrollForwardClicked(object? sender, EventArgs e)
        {
            if (_tabHeaderScrollView != null)
            {
                var scrollToX = Math.Clamp(_tabHeaderScrollView.ScrollX + 100d, 0, _tabHeaderScrollView.ContentSize.Width - _tabHeaderScrollView.Width);
                _tabHeaderScrollView.ScrollToAsync(scrollToX, 0, true);
            }
        }

        /// <summary>
        /// Update tab indicator width based on template.
        /// </summary>
        void UpdateTabIndicatorTemplateWidth()
        {
            if (_tabSelectionIndicator != null &&
                _tabHeaderItemContent != null &&
                _tabHeaderItemContent.Children.Count > 0 &&
                SelectedIndex >= 0 &&
                _tabHeaderItemContent.Children.Count > SelectedIndex)
            {
				int adjustedIndex = SelectedIndex; // Default to the existing selected index
				if (IsCenterButtonEnabled && TabWidthMode is TabWidthMode.Default)
				{
					adjustedIndex = GetAdjustedIndexForCenterButton(adjustedIndex);
				}

				if (adjustedIndex < _tabHeaderItemContent.Children.Count)
				{
					_selectedTabX = _tabHeaderItemContent.Children[adjustedIndex].Frame.X;
					if (SelectedIndex >= 0)
					{
						_selectedTabX = CalculateTabXPosition();
					}
					_currentIndicatorWidth = _tabHeaderItemContent.Children[adjustedIndex].Measure(double.PositiveInfinity, double.PositiveInfinity).Width;
					IndicatorAnimation();
					_previousTabX = _selectedTabX;
					_previewSelectedIndex = adjustedIndex;
					UpdateTabIndicatorPosition();
				}
            }
        }

        double CalculateTabXPosition()
        {
            double xPosition = 0;
            if (_tabHeaderItemContent != null)
            {
				int adjustedIndex = SelectedIndex;
				if (IsCenterButtonEnabled && TabWidthMode is TabWidthMode.Default)
				{
					adjustedIndex = GetAdjustedIndexForCenterButton(adjustedIndex);
				}

				for (int i = 0; i < adjustedIndex; i++)
                {
                    var size = _tabHeaderItemContent.Children[i].Measure(double.PositiveInfinity, double.PositiveInfinity);
                    xPosition += size.Width;
                }
            }
            return xPosition;
        }

        /// <summary>
        /// Update indicator width based on items.
        /// </summary>
        internal void UpdateTabIndicatorWidth()
        {
            if (_tabSelectionIndicator != null &&
                _tabHeaderItemContent != null &&
                _tabHeaderItemContent?.Children.Count > 0 &&
                _tabHeaderItemContent.Children.Count > SelectedIndex && SelectedIndex >= 0)
            {
                if (ItemsSource != null && ItemsSource.Count > 0 || HeaderItemTemplate != null)
                {
                    UpdateTabIndicatorTemplateWidth();
                }
                else
                {
					if (this._tabHeaderItemContent.Children[(int)this.SelectedIndex].Frame.Width > 0)
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
            if (_tabHeaderItemContent == null)
			{
				return;
			}

			SfTabItem item = Items[SelectedIndex];

            if (SelectedIndex > 0 && _selectedTabX <= 0)
            {
                _selectedTabX = _tabHeaderItemContent.Children[SelectedIndex - 1].Frame.X;
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
			_previewSelectedIndex = SelectedIndex;
            if (_tabHeaderItemContent != null)
            {
                if (SelectedIndex < _tabHeaderItemContent.Children.Count)
                {
                    UpdateTabIndicatorPosition();
                }
            }
        }

        void UpdateTabIndicatorWidthOnWidthRequest(SfTabItem item)
        {
            if (_tabHeaderItemContent != null && _tabSelectionIndicator != null)
            {
				int adjustedIndex = SelectedIndex;
				if (IsCenterButtonEnabled && TabWidthMode is TabWidthMode.Default)
				{
					adjustedIndex = GetAdjustedIndexForCenterButton(SelectedIndex);
				}

				if (IndicatorPlacement == TabIndicatorPlacement.Fill)
                {
                    _selectedTabX = _tabHeaderItemContent.Children[adjustedIndex].Frame.X;
                    _currentIndicatorWidth = _tabHeaderItemContent.Children[adjustedIndex].Width;
                    if (double.IsNaN(_currentIndicatorWidth))
                    {
                        _currentIndicatorWidth = _tabHeaderItemContent.Children[adjustedIndex].Measure(double.PositiveInfinity, double.PositiveInfinity).Width;
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
                        _selectedTabX = _tabHeaderItemContent.Children[adjustedIndex].Frame.X;
                        _currentIndicatorWidth = _tabHeaderItemContent.Children[adjustedIndex].Width;
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
        }

		/// <summary>
		/// Updates the position of the tab indicator based on the selected tab's X position and width.
		/// </summary>
		/// <param name="selectedViewX">The X position of the selected tab item.</param>
		/// <param name="tempIndicatorWidth">The width of the tab indicator.</param>
		(double, double) UpdateIndicatorPosition(double selectedViewX, double tempIndicatorWidth)
		{
			if (_tabHeaderItemContent != null)
			{
				selectedViewX -= _removedItemWidth;
				_removedItemWidth = 0;
			}

			return (selectedViewX, tempIndicatorWidth);
		}

		void UpdateIndicatorForFitMode(SfTabItem item)
        {
            if (_tabHeaderItemContent == null)
			{
				return;
			}

			if (GetVisibleItems() == 0)
			{
				_currentIndicatorWidth = 0.1;
			}
			else
            {
                _currentIndicatorWidth = MeasureHeaderContentWidth(item);
                UpdateCurrentIndicatorWidth(item);
                
            }
        }

        void UpdateCurrentIndicatorWidth(SfTabItem item)
        {
            if (_tabHeaderItemContent!=null)
            {
				int adjustedIndex = SelectedIndex; // Default to the existing selected index

				if (IsCenterButtonEnabled && TabWidthMode is TabWidthMode.Default)
				{
					adjustedIndex = GetAdjustedIndexForCenterButton(SelectedIndex);
				}

				if (item.HeaderHorizontalTextAlignment == TextAlignment.Start && item.HeaderContent is null)
                {
                    _selectedTabX = (_tabHeaderItemContent.Children[adjustedIndex].Frame.Left);
                }
                else if (item.HeaderHorizontalTextAlignment == TextAlignment.End && item.HeaderContent is null)
                {
                    _selectedTabX = (_tabHeaderItemContent.Children[adjustedIndex].Frame.Right) - _currentIndicatorWidth;
                }
                else
                {
                    _selectedTabX = (_tabHeaderItemContent.Children[adjustedIndex].Frame.Center.X) - ((_currentIndicatorWidth / 2));
				}

#if ANDROID
				var itemsCount = GetVisibleItems();

				if (IsCenterButtonEnabled && TabWidthMode is TabWidthMode.Default && (_isItemRemovedWithCenterButton || _isTabItemPropertyChanged) && itemsCount % 2 == 0 && SelectedIndex == Items.Count - 1)
				{
					double totalWidth = 0;
					for (int index = 0; index < _tabHeaderItemContent.Count - 1; index++)
					{
						totalWidth += _tabHeaderItemContent.Children[index].Width;
					}

					if (_selectedTabX < totalWidth)
					{
						_selectedTabX += _tabHeaderItemContent.Children[adjustedIndex - 1].Width;
					}

					_isTabItemPropertyChanged = false;
				}
#endif

				if (_currentIndicatorWidth > _tabHeaderItemContent.Children[adjustedIndex].Width)
                {
                    _selectedTabX = _tabHeaderItemContent.Children[adjustedIndex].Frame.X;
                    _currentIndicatorWidth = _tabHeaderItemContent.Children[adjustedIndex].Width;
                }
            }
        }

        /// <summary>
        /// Method to animate the selection indicator in Tab View.
        /// </summary>
        void IndicatorAnimation()
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

			var selectedItem = _tabHeaderItemContent?.Children[adjustedIndex];
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

        void AnimateTabSelectionIndicator(Animation parentAnimation, uint animationDuration, double targetX)
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

		/// <summary>
		/// Determines whether an action should be performed based on the change in selected index, running animations, indicator dimensions, and the position of the selected view.
		/// </summary>
		/// <returns>Returns true if the conditions for performing the action are met; otherwise, returns false.</returns>
		bool ShouldPerformAction()
		{
			var isAnimationRunning = this.AnimationIsRunning("SelectionIndicatorAnimation");

#if !WINDOWS
			var isSelectedViewXConditionMet = this._selectedTabX >= 0 ||
				((((this as IVisualElementController).EffectiveFlowDirection & EffectiveFlowDirection.RightToLeft) == EffectiveFlowDirection.RightToLeft) && _selectedTabX < 0);
#else
    var isSelectedViewXConditionMet = _selectedTabX > 0;
#endif

			var hasIndexChanged = _previewSelectedIndex != SelectedIndex;
			var isIndicatorDimensionChanged = _previousIndicatorWidth != _currentIndicatorWidth;
			var isItemCountChanged = _tabHeaderItemContent is not null && _tabHeaderItemContent.Count != GetVisibleItems();
			var isPositionChanged = _previousTabX != _selectedTabX;

			return (hasIndexChanged || (isAnimationRunning && (isIndicatorDimensionChanged || isItemCountChanged || isPositionChanged)))
				   && isSelectedViewXConditionMet;
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
        /// This is update the tab selection indicator x position.
        /// </summary>
        /// <param name="value"></param>
#if MACCATALYST || IOS
        async void TranslateAnimatedMethod(double value)
#else
        void TranslateAnimatedMethod(double value)
#endif
        {
            if (_tabSelectionIndicator != null)
            {
#if MACCATALYST || IOS
                //Delay is used for transition: On Mac and iOS, the transition animation is not functioning properly and does not behave like it does on Windows and Android platforms.
                await Task.Delay(150);
#endif
                _tabSelectionIndicator.TranslationX = value;
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

            if (_tabHeaderItemContent != null)
            {
                _tabHeaderItemContent.Style = new Style(typeof(SfHorizontalStackLayout));
            }

            if (_tabSelectionIndicator != null)
            {
                _tabSelectionIndicator.Style = new Style(typeof(SfBorder));
            }
        }

        void Items_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            // Handle old items being removed
            var headerItemsCount = _tabHeaderItemContent?.Children.Count;
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
                    headerItemsCount != _tabHeaderItemContent?.Children.Count)
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
                _tabHeaderItemContent?.Children.Clear();
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
				if (TabWidthMode is TabWidthMode.SizeToContent)
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
				else
				{
#if ANDROID
					if (_tabHeaderScrollView is not null)
					{
						_tabHeaderScrollView.HorizontalOptions = LayoutOptions.Fill;
					}
#endif
					_tabHeaderParentContainer.HorizontalOptions = LayoutOptions.Fill;
				}
			}
		 }

		void UpdateTabWidthMode()
        {
            UpdateTabPadding();
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

        static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as SfTabBar)?.UpdateItemsSource();

        static void OnSelectedIndexChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as SfTabBar)?.UpdateSelectedIndex((int)newValue, (int)oldValue);

        static void OnScrollButtonEnabledChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as SfTabBar)?.UpdateScrollButtonEnabled((Boolean)newValue);

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