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
    internal class SfTabBar : SfStackLayout, ITapGestureListener
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
        readonly int _defaultIndicatorHeight = 3;
        readonly double _tabHeaderImageSize = 32d;
        readonly double _arrowButtonSize = 32;
		double _removedItemWidth = 0;
		bool _itemRemoved = false;

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

#if IOS
        Point _touchDownPoint = new Point();
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
        /// Identifies the <see cref="ScrollButtonIconColor"/> bindable property.
        /// </summary>
        internal static readonly BindableProperty ScrollButtonForegroundColorProperty =
            BindableProperty.Create(
                nameof(ScrollButtonIconColor),
                typeof(Color),
                typeof(SfTabBar),
                Color.FromArgb("#49454F"),
                BindingMode.Default,
                null,
                propertyChanged: OnScrollButtonForegroundColorChanged);

        /// <summary>
        /// Identifies the <see cref="ScrollButtonBackground"/> bindable property.
        /// </summary>
        internal static readonly BindableProperty ScrollButtonBackgroundProperty =
            BindableProperty.Create(
                nameof(ScrollButtonBackground),
                typeof(Color),
                typeof(SfTabBar),
                Color.FromArgb("#F7F2FB"),
                BindingMode.Default,
                propertyChanged: OnScrollButtonBackgroundColorChanged);

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

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SfTabBar"/> class.
        /// </summary>
        public SfTabBar()
        {
            Items = new TabItemCollection();
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
        /// It accepts the Color values. 
        /// </value>
        internal Color ScrollButtonBackground
        {
            get => (Color)GetValue(ScrollButtonBackgroundProperty);
            set => SetValue(ScrollButtonBackgroundProperty, value);
        }

        /// <summary>
        /// Gets or sets a value that can be used to customize the scroll button’s foreground color in <see cref="SfTabView"/>.
        /// </summary>
        /// <value>
        /// It accepts the Color values. 
        /// </value>
        internal Color ScrollButtonIconColor
        {
            get => (Color)GetValue(ScrollButtonForegroundColorProperty);
            set => SetValue(ScrollButtonForegroundColorProperty, value);
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
                return;
            var index = _tabHeaderItemContent?.Children.IndexOf((IView)(sender));
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
            if (SelectionChanged != null)
            {
                SelectionChanged(this, args);
            }
        }

        /// <summary>
        /// Invokes <see cref="TabItemTapped"/> event.
        /// </summary>
        /// <param name="args">The tab item tapped event args.</param>
        internal void RaiseTabItemTappedEvent(TabItemTappedEventArgs args)
        {
            if (TabItemTapped != null)
            {
                TabItemTapped(this, args);
            }
        }

        /// <summary>
        /// Invokes <see cref="SelectionChanging"/> event.
        /// </summary>
        /// <param name="args">The event arguments.</param>
        internal void RaiseSelectionChangingEvent(SelectionChangingEventArgs args)
        {
            if (SelectionChanging != null)
            {
                SelectionChanging(this, args);
            }
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
                    // Update the scroll view position based on the currently selected index
                    UpdateScrollViewPosition(SelectedIndex);
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
                // Remove the child at specified index
                _tabHeaderItemContent?.Children.RemoveAt(index);
                if (Items.Count > 0 && SelectedIndex >= Items.Count && index==SelectedIndex)
                {
                    SelectedIndex = Items.Count - 1;
                }
				else if (index <= this.SelectedIndex)
				{
					_removedItemWidth = tabItem.Width;

					if (index < this.SelectedIndex)
					{
						if(this.SelectedIndex<=Items.Count)
						{
							this.SelectedIndex--;
						}
						else
						{
							this.SelectedIndex = Items.Count - 1;
						}
						
					}
					else
					{
						_itemRemoved=true;
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
                        item.FontAutoScalingEnabled = isEnabled;
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
                return;

            int count = -1;
            foreach (var item in Items)
            {
                count++;
                if (item is not null)
                    AddTabItemProperties(item, count);
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
                if (tabItem.ControlTemplate == null)
                {
                    tabItem.ControlTemplate = new ControlTemplate(typeof(TabViewMaterialVisualStyle));
                }

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
            touchEffectGrid.SetBinding(SfGrid.IsVisibleProperty, new Binding("IsVisible", source: item));

            CalculateTabItemWidth();

            AddTouchEffects(touchEffectGrid);
            touchEffectGrid.Children.Add(item);

            InsertHeaderItemToContent(touchEffectGrid, index);

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
                Size desiredSize = item.Header.Measure((float)item.FontSize, item.FontAttributes, item.FontFamily);
                var width = 0d;
                if (item.IsVisible)
                {
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
                return;

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
                return;
            await _tabHeaderScrollView.ScrollToAsync(_tabHeaderItemContent?.Children[position] as Element, ScrollToPosition.Center, true).ConfigureAwait(false);
        }
#endif

        #endregion

        #region Private Methods

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
                    foreach (var item in _tabHeaderItemContent.Children)
                    {
                        if (item != null)
                        {
                            AddTabViewItemFromTemplate(item);
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

                if (ItemsSource?.Count <= 0)
                    ClearIndicatorWidth();
            }
        }

        void HandleNewItems(NotifyCollectionChangedEventArgs e, int? headerItemsCount)
        {
            if (e.NewItems != null)
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

                        if (_tabHeaderItemContent != null)
                        {
                            foreach (var headerItem in _tabHeaderItemContent.Children)
                            {
                                if (headerItem != null)
                                {
                                    AddTabViewItemFromTemplate(headerItem);
                                }
                            }
                        }
                    }
                }
            }
        }

        void OnTabItemPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.PropertyName))
            {
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
                }
                else if (e.PropertyName.Equals(nameof(SfTabItem.Width), StringComparison.Ordinal) ||
                    e.PropertyName.Equals(nameof(SfTabItem.WidthRequest), StringComparison.Ordinal))
                {
                    UpdateTabIndicatorWidth();
                }
                else if (e.PropertyName == nameof(SfTabItem.Header) ||
                    e.PropertyName.Equals(nameof(SfTabItem.ImageSource), StringComparison.Ordinal))
                {
                    UpdateHeaderDisplayMode();
                }

                if (e.PropertyName.Equals(nameof(SfTabItem.ImagePosition), StringComparison.Ordinal) ||
                    e.PropertyName.Equals(nameof(SfTabItem.Header), StringComparison.Ordinal) ||
                    e.PropertyName.Equals(nameof(SfTabItem.FontSize), StringComparison.Ordinal) ||
                    e.PropertyName.Equals(nameof(SfTabItem.ImageTextSpacing), StringComparison.Ordinal))
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
                    effectsView?.ApplyEffects(SfEffects.Highlight);
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
                effectsView.ForceReset = true;
                effectsView.Reset();
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
                effectsView.ForceReset = true;
                effectsView.Reset();
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

            effectsView?.ApplyEffects();
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
                return;

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
				return;

#if WINDOWS || MACCATALYST
			effectsView.ForceReset = true;
#endif
			effectsView.Reset();

#if WINDOWS || MACCATALYST
			if (tabItem.Bounds.Contains(e.TouchPoint))
			{
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
                    _tabSelectionIndicator.HeightRequest = _defaultIndicatorHeight;
                    _tabSelectionIndicator.VerticalOptions = LayoutOptions.End;
                    if (_roundRectangle != null)
                    {
                        _roundRectangle.CornerRadius = IndicatorCornerRadius != new CornerRadius(-1) ? IndicatorCornerRadius : new CornerRadius(4, 4, 0, 0);
                        _tabSelectionIndicator.StrokeShape = _roundRectangle;
                    }
                }
                else
                {
                    _tabSelectionIndicator.HeightRequest = _defaultIndicatorHeight;
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
                _tabHeaderParentContainer.ColumnDefinitions = new ColumnDefinitionCollection
                    {
                        new ColumnDefinition { Width = new GridLength(_arrowButtonSize) },
                        new ColumnDefinition(),
                        new ColumnDefinition { Width = new GridLength(_arrowButtonSize) },
                    };

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
                return;
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
                _tabHeaderParentContainer.ColumnDefinitions = new ColumnDefinitionCollection
                    {
                        new ColumnDefinition(),
                    };
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
            foreach (var item in Items)
            {
                if (item != null)
                {
                    Size desiredSize = item.Header.Measure((float)item.FontSize, item.FontAttributes, item.FontFamily);
                    CalculateTabItemWidthForSizeToContentForItem(item, desiredSize);
                }
            }
        }

        void CalculateTabItemWidthForSizeToContentForItem(SfTabItem item, Size desiredSize)
        {
            if (item.IsVisible)
            {
                double width;
                if (item.ImageSource != null && !string.IsNullOrEmpty(item.Header))
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
            }
        }

        void CalculateTabItemWidthForDefaultWidthMode()
        {
            var width = WidthRequest / GetVisibleItems();
            if (width > 0)
            {
                foreach (var item in Items)
                {
                    if (item != null)
                    {
                        SetHeaderDisplayMode(item);
                        UpdateTabItemWidth(item, width);
                    }
                }
            }
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
        void UpdateScrollButtonBackgroundColor()
        {
            if (_forwardArrow != null && _backwardArrow != null)
            {
                _forwardArrow.ScrollButtonBackgroundColor = ScrollButtonBackground;
                _backwardArrow.ScrollButtonBackgroundColor = ScrollButtonBackground;
            }
        }

        /// <summary>
        /// Updates the foreground color of the scroll buttons.
        /// </summary>
        void UpdateScrollButtonForegroundColor()
        {
            if (_forwardArrow != null && _backwardArrow != null)
            {
                if (_forwardArrow.IsEnabled)
                {
                    _forwardArrow.ForegroundColor = ScrollButtonIconColor;
                }
                else
                {
                    _forwardArrow.ForegroundColor = ScrollButtonDisabledIconColor;
                }

                if (_backwardArrow.IsEnabled)
                {
                    _backwardArrow.ForegroundColor = ScrollButtonIconColor;
                }
                else
                {
                    _backwardArrow.ForegroundColor = ScrollButtonDisabledIconColor;
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
            };

            // Create a grid to hold the tab items and indicator
            _tabHeaderContentContainer = new SfGrid()
            {
                HorizontalOptions = LayoutOptions.Fill,
                Children = { _tabHeaderItemContent, _tabSelectionIndicator },
            };

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
                            var index = this._tabHeaderItemContent.Children.IndexOf(item);
                            width += this._tabHeaderItemContent.Children[index].Measure(double.PositiveInfinity, double.PositiveInfinity).Width;
                        }
                    }
                }

                UpdateScrollButtonStateBasedOnScrollPosition(scrollViewWidth, width);

                if (ScrollButtonBackground != null)
                {
                    _forwardArrow.ScrollButtonBackgroundColor = ScrollButtonBackground;
                    _backwardArrow.ScrollButtonBackgroundColor = ScrollButtonBackground;
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
                    _backwardArrow.ForegroundColor = ScrollButtonDisabledIconColor;
                    _forwardArrow.ForegroundColor = ScrollButtonDisabledIconColor;
                }
                else if (_tabHeaderScrollView.ScrollX == 0)
                {
                    _backwardArrow.IsEnabled = false;
                    _forwardArrow.IsEnabled = true;
                    _backwardArrow.ForegroundColor = ScrollButtonDisabledIconColor;
                    _forwardArrow.ForegroundColor = ScrollButtonIconColor;
                }
                else if (Math.Round(_tabHeaderScrollView.ScrollX) == Math.Round(_tabHeaderScrollView.ContentSize.Width - _tabHeaderScrollView.Width))
                {
                    _backwardArrow.IsEnabled = true;
                    _forwardArrow.IsEnabled = false;
                    _forwardArrow.ForegroundColor = ScrollButtonDisabledIconColor;
                    _backwardArrow.ForegroundColor = ScrollButtonIconColor;
                }
                else
                {
                    _backwardArrow.IsEnabled = true;
                    _forwardArrow.IsEnabled = true;
                    _backwardArrow.ForegroundColor = ScrollButtonIconColor;
                    _forwardArrow.ForegroundColor = ScrollButtonIconColor;
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
                _selectedTabX = _tabHeaderItemContent.Children[SelectedIndex].Frame.X;
                if (SelectedIndex >= 0)
                {
                    _selectedTabX = CalculateTabXPosition();
                }
                _currentIndicatorWidth = _tabHeaderItemContent.Children[SelectedIndex].Measure(double.PositiveInfinity, double.PositiveInfinity).Width;
                IndicatorAnimation();
                _previousTabX = _selectedTabX;
                _previewSelectedIndex = SelectedIndex;
                if (SelectedIndex < _tabHeaderItemContent.Children.Count)
                {
                    UpdateTabIndicatorPosition();
                }
            }
        }

        double CalculateTabXPosition()
        {
            double xPosition = 0;
            if (_tabHeaderItemContent != null)
            {
                for (int i = 0; i < SelectedIndex; i++)
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
        void UpdateTabIndicatorWidth()
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
                    UpdateTabIndicatorForSelectedItem();
                }
            }
        }

        void UpdateTabIndicatorForSelectedItem()
        {
            if (_tabHeaderItemContent == null)
                return;

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
                if (IndicatorPlacement == TabIndicatorPlacement.Fill)
                {
                    _selectedTabX = _tabHeaderItemContent.Children[SelectedIndex].Frame.X;
                    _currentIndicatorWidth = _tabHeaderItemContent.Children[SelectedIndex].Width;
                    if (double.IsNaN(_currentIndicatorWidth))
                    {
                        _currentIndicatorWidth = _tabHeaderItemContent.Children[this.SelectedIndex].Measure(double.PositiveInfinity, double.PositiveInfinity).Width;
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
                        _selectedTabX = _tabHeaderItemContent.Children[SelectedIndex].Frame.X;
                        _currentIndicatorWidth = _tabHeaderItemContent.Children[SelectedIndex].Width;
                    }
                }

				if (_removedItemWidth != 0 && TabWidthMode == TabWidthMode.SizeToContent)
				{
					(_selectedTabX, _currentIndicatorWidth) = UpdateIndicatorPosition(_selectedTabX, _currentIndicatorWidth);
				}

				if (_currentIndicatorWidth > _tabSelectionIndicator.Width)
                    _tabSelectionIndicator.MaximumWidthRequest = _currentIndicatorWidth * 2;
            }
        }

		/// <summary>
		/// Updates the position of the tab indicator based on the selected tab's X position and width.
		/// </summary>
		/// <param name="selectedViewX">The X position of the selected tab item.</param>
		/// <param name="tempIndicatorWidth">The width of the tab indicator.</param>
		(double, double) UpdateIndicatorPosition(double selectedViewX, double tempIndicatorWidth)
		{
			if (this._tabHeaderItemContent != null)
			{
				selectedViewX = selectedViewX - _removedItemWidth;
				_removedItemWidth = 0;
			}

			return (selectedViewX, tempIndicatorWidth);
		}

		void UpdateIndicatorForFitMode(SfTabItem item)
        {
            if (_tabHeaderItemContent == null)
                return;

            if (GetVisibleItems() == 0)
                _currentIndicatorWidth = 0.1;
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
                if (item.HeaderHorizontalTextAlignment == TextAlignment.Start)
                {
                    _selectedTabX = (_tabHeaderItemContent.Children[SelectedIndex].Frame.Left);
                }
                else if (item.HeaderHorizontalTextAlignment == TextAlignment.End)
                {
                    _selectedTabX = (_tabHeaderItemContent.Children[SelectedIndex].Frame.Right) - _currentIndicatorWidth;
                }
                else
                {
                    _selectedTabX = (_tabHeaderItemContent.Children[SelectedIndex].Frame.Center.X) - ((_currentIndicatorWidth / 2));
                }

                if (_currentIndicatorWidth > _tabHeaderItemContent.Children[SelectedIndex].Width)
                {
                    _selectedTabX = _tabHeaderItemContent.Children[SelectedIndex].Frame.X;
                    _currentIndicatorWidth = _tabHeaderItemContent.Children[SelectedIndex].Width;
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
                previousIndicatorWidth = _tabSelectionIndicator.Width;

            previousIndicatorWidth = previousIndicatorWidth < 0 ? 0 : previousIndicatorWidth;

            var selectedItem = _tabHeaderItemContent?.Children[SelectedIndex];
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

            var parentAnimation = new Animation();
            parentAnimation.Add(0, 0.5, firstWidthAnimation);
            parentAnimation.Add(0.5, 1, secondWidthAnimation);
            parentAnimation.Add(0, 1, translateAnimation);
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
                if ((_previewSelectedIndex != SelectedIndex ||
                    (this.AnimationIsRunning("SelectionIndicatorAnimation") &&
                    (_previousIndicatorWidth != _currentIndicatorWidth ||
                    (_tabHeaderItemContent != null && _tabHeaderItemContent.Count != GetVisibleItems())))) &&
                    _selectedTabX > 0)
                {
                    parentAnimation.Commit(this, "SelectionIndicatorAnimation", 16, animationDuration, Easing.Linear, finished: (v, e) =>
                    {
#if WINDOWS || ANDROID
                        if (_tabSelectionIndicator.TranslationX == targetX)
#endif
                        {
                            UpdateFillSelectedTabItem();
                        }

                        _previousIndicatorWidth = _currentIndicatorWidth;
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
						if (_itemRemoved)
						{
							_tabSelectionIndicator.TranslationX = targetX;
							_itemRemoved = false;
						}
						else
						{
							_tabSelectionIndicator.TranslateTo(targetX, 0, animationDuration, Easing.Linear);
						}
						UpdateFillSelectedTabItem();
                        UpdateScrollViewPosition(SelectedIndex);
                    }
                }
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

        void UpdateTabWidthMode()
        {
            UpdateTabPadding();
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
                    CalculateTabItemWidth();
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
            base.OnHandlerChanged();
            if (Handler == null)
            {
                this.Children.Clear();
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

        static void OnItemsChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as SfTabBar)?.UpdateItems();

        static void OnHeaderTemplateChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as SfTabBar)?.UpdateItemsSource();

        static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as SfTabBar)?.UpdateItemsSource();

        static void OnSelectedIndexChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as SfTabBar)?.UpdateSelectedIndex((int)newValue, (int)oldValue);

        static void OnScrollButtonEnabledChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as SfTabBar)?.UpdateScrollButtonEnabled((Boolean)newValue);

        static void OnHeaderDisplayModeChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as SfTabBar)?.UpdateHeaderDisplayMode();

        static void OnIndicatorCornerRadiusChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as SfTabBar)?.UpdateCornerRadius();

        static void OnFontAutoScalingEnabledChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as SfTabBar)?.UpdateFontAutoScalingEnabled((Boolean)newValue);

        static void OnScrollButtonBackgroundColorChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as SfTabBar)?.UpdateScrollButtonBackgroundColor();

        static void OnScrollButtonForegroundColorChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as SfTabBar)?.UpdateScrollButtonForegroundColor();

        static void OnScrollButtonDisabledForegroundColorChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as SfTabBar)?.UpdateScrollButtonForegroundColor();

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