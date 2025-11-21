using System.Collections;
using System.Runtime.CompilerServices;
using Syncfusion.Maui.Toolkit.Themes;
using Syncfusion.Maui.Toolkit.Helper;
using System.ComponentModel;

namespace Syncfusion.Maui.Toolkit.TabView
{
	/// <summary>
	/// Represents a tab view control that contains multiple items that share the same space on the screen.
	/// </summary>
	/// <example>
	/// The following example shows how to initialize the tab view.
	/// 
	/// # [XAML](#tab/tabid-1)
	/// <code><![CDATA[
	/// <tabView:SfTabView x:Name="tabView">
	///     <tabView:SfTabView.Items>
	///
	///         <tabView:SfTabItem Header="Item 1">
	///             <tabView:SfTabItem.Content>
	///                 <Grid BackgroundColor="Red" />
	///             </tabView:SfTabItem.Content>
	///         </tabView:SfTabItem>
	///
	///         <tabView:SfTabItem Header="Item 2">
	///             <tabView:SfTabItem.Content>
	///                 <Grid BackgroundColor="Green" />
	///             </tabView:SfTabItem.Content>
	///         </tabView:SfTabItem>
	///
	///         <tabView:SfTabItem Header="Item 3">
	///             <tabView:SfTabItem.Content>
	///                 <Grid BackgroundColor="Blue" />
	///             </tabView:SfTabItem.Content>
	///         </tabView:SfTabItem>
	///
	///     </tabView:SfTabView.Items>
	/// </tabView:SfTabView>
	/// ]]></code>
	/// 
	/// # [C#](#tab/tabid-2)
	/// <code><![CDATA[
	/// var tabView = new SfTabView();
	/// Grid allContactsGrid = new Grid { BackgroundColor = Colors.Red };
	/// Grid favoritesGrid = new Grid { BackgroundColor = Colors.Green };
	/// Grid contactsGrid = new Grid { BackgroundColor = Colors.Blue };
	/// var tabItems = new TabItemCollection
	/// {
	///     new SfTabItem()
	///     {
	///         Header = "Item 1",
	///         Content = allContactsGrid
	///     },
	///     new SfTabItem()
	///     {
	///         Header = "Item 2",
	///         Content = favoritesGrid
	///     },
	///     new SfTabItem()
	///     {
	///         Header = "Item 3",
	///         Content = contactsGrid
	///     }
	/// };
	///
	/// tabView.Items = tabItems;
	/// Content = tabView;
	/// ]]></code>
	/// </example>
	[ContentProperty(nameof(Items))]
	public partial class SfTabView : ContentView, IParentThemeElement
	{
		#region Fields

		SfHorizontalContent? _tabContentContainer;
		SfTabBar? _tabHeaderContainer;
		CenterButtonView? _centerButtonView;
		SfGrid? _parentGrid;
		TabSelectionChangedEventArgs? _selectionChangedEventArgs;

		#endregion

		#region Bindable Properties

		/// <summary>
		/// Identifies the <see cref="TabBarBackground"/> bindable property.
		/// </summary>
		public static readonly BindableProperty TabBarBackgroundProperty =
			BindableProperty.Create(
				nameof(TabBarBackground),
				typeof(Brush),
				typeof(SfTabView),
				null,
				propertyChanged: OnTabBarBackgroundPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="HeaderDisplayMode"/> bindable property.
		/// </summary>
		public static readonly BindableProperty HeaderDisplayModeProperty =
			BindableProperty.Create(
				nameof(HeaderDisplayMode),
				typeof(TabBarDisplayMode),
				typeof(SfTabBar),
				TabBarDisplayMode.Default,
				BindingMode.TwoWay,
				propertyChanged: OnHeaderDisplayModeChanged);

		/// <summary>
		/// Identifies the <see cref="TabBarPlacement"/> bindable property.
		/// </summary>
		public static readonly BindableProperty TabBarPlacementProperty =
			BindableProperty.Create(
				nameof(TabBarPlacement),
				typeof(TabBarPlacement),
				typeof(SfTabView),
				TabBarPlacement.Top,
				propertyChanged: OnTabBarPlacementChanged);

		/// <summary>
		/// Identifies the <see cref="IndicatorPlacement"/> bindable property.
		/// </summary>
		public static readonly BindableProperty IndicatorPlacementProperty =
			BindableProperty.Create(
				nameof(IndicatorPlacement),
				typeof(TabIndicatorPlacement),
				typeof(SfTabView),
				TabIndicatorPlacement.Bottom,
				propertyChanged: OnIndicatorPlacementChanged);

		/// <summary>
		/// Identifies the <see cref="IndicatorWidthMode"/> bindable property.
		/// </summary>
		public static readonly BindableProperty IndicatorWidthModeProperty =
			BindableProperty.Create(
				nameof(IndicatorWidthMode),
				typeof(IndicatorWidthMode),
				typeof(SfTabView),
				IndicatorWidthMode.Fit,
				propertyChanged: OnIndicatorWidthModePropertyChanged);

		/// <summary>
		/// Identifies the <see cref="TabWidthMode"/> bindable property.
		/// </summary>
		public static readonly BindableProperty TabWidthModeProperty =
			BindableProperty.Create(
				nameof(TabWidthMode),
				typeof(TabWidthMode),
				typeof(SfTabView),
				TabWidthMode.Default,
				propertyChanged: OnTabWidthModePropertyChanged);

		/// <summary>
		/// Identifies the <see cref="TabHeaderAlignment"/> bindable property.
		/// </summary>
		public static readonly BindableProperty TabHeaderAlignmentProperty =
			BindableProperty.Create(
				nameof(TabHeaderAlignment), 
				typeof(TabHeaderAlignment), 
				typeof(SfTabView), 
				TabHeaderAlignment.Start, 
				propertyChanged: OnTabHeaderAlignmentPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="IndicatorBackground"/> bindable property.
		/// </summary>
		public static readonly BindableProperty IndicatorBackgroundProperty =
			BindableProperty.Create(
				nameof(IndicatorBackground),
				typeof(Brush),
				typeof(SfTabView),
				new SolidColorBrush(Color.FromArgb("#6750A4")),
				propertyChanged: OnIndicatorBackgroundChanged);

		/// <summary>
		/// Identifies the <see cref="TabBarHeight"/> bindable property.
		/// </summary>
		public static readonly BindableProperty TabBarHeightProperty =
			BindableProperty.Create(
				nameof(TabBarHeight),
				typeof(double),
				typeof(SfTabView),
				48d,
				propertyChanged: OnTabBarHeightChanged);

		/// <summary>
		/// Identifies the <see cref="Items"/> bindable property.
		/// </summary>
		public static readonly BindableProperty ItemsProperty =
			BindableProperty.Create(
				nameof(Items),
				typeof(TabItemCollection),
				typeof(SfTabView),
				null,
				propertyChanged: OnItemsChanged);

		/// <summary>
		/// Identifies the <see cref="SelectedIndex"/> bindable property.
		/// </summary>
		public static readonly BindableProperty SelectedIndexProperty =
			BindableProperty.Create(
				nameof(SelectedIndex),
				typeof(int),
				typeof(SfTabView),
				0,
				BindingMode.TwoWay,
				propertyChanged: OnSelectedIndexChanged);

		/// <summary>
		/// Identifies the <see cref="ItemsSource"/> bindable property.
		/// </summary>
		public static readonly BindableProperty ItemsSourceProperty =
			BindableProperty.Create(
				nameof(ItemsSource),
				typeof(IList),
				typeof(SfTabView),
				null,
				propertyChanged: OnItemsSourceChanged);

		/// <summary>
		/// Identifies the <see cref="HeaderItemTemplate"/> bindable property.
		/// </summary>
		public static readonly BindableProperty HeaderItemTemplateProperty =
			BindableProperty.Create(
				nameof(HeaderItemTemplate),
				typeof(DataTemplate),
				typeof(SfTabView),
				null,
				propertyChanged: OnHeaderItemTemplateChanged);

		/// <summary>
		/// Identifies the <see cref="ContentItemTemplate"/> bindable property.
		/// </summary>
		public static readonly BindableProperty ContentItemTemplateProperty =
			BindableProperty.Create(
				nameof(ContentItemTemplate),
				typeof(DataTemplate),
				typeof(SfTabView),
				null,
				propertyChanged: OnContentItemTemplateChanged);

		/// <summary>
		/// Identifies the <see cref="TabHeaderPadding"/> bindable property.
		/// </summary>
		public static readonly BindableProperty TabHeaderPaddingProperty =
			BindableProperty.Create(
				nameof(TabHeaderPadding),
				typeof(Thickness),
				typeof(SfTabView),
				new Thickness(52, 0, 52, 0),
				propertyChanged: OnTabHeaderPaddingPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="IsScrollButtonEnabled "/> bindable property.
		/// </summary>
		public static readonly BindableProperty IsScrollButtonEnabledProperty =
			BindableProperty.Create(
				nameof(IsScrollButtonEnabled),
				typeof(bool),
				typeof(SfTabView),
				false,
				BindingMode.Default,
				propertyChanged: OnScrollButtonEnabledChanged);

		/// <summary>
		/// Identifies the <see cref="HeaderHorizontalTextAlignment "/> bindable property.
		/// </summary>
		public static readonly BindableProperty HeaderHorizontalTextAlignmentProperty =
			BindableProperty.Create(
				nameof(HeaderHorizontalTextAlignment),
				typeof(TextAlignment),
				typeof(SfTabView),
				TextAlignment.Center,
				BindingMode.Default,
				propertyChanged: OnHeaderHorizontalTextAlignmentChanged);

		/// <summary>
		/// Identifies the <see cref="ContentTransitionDuration "/> bindable property.
		/// </summary>
		public static readonly BindableProperty ContentTransitionDurationProperty =
			BindableProperty.Create(
				nameof(ContentTransitionDuration),
				typeof(double),
				typeof(SfTabView),
				100d,
				propertyChanged: OnContentTransitionDurationChanged);

		/// <summary>
		/// Identifies the <see cref="IndicatorCornerRadius"/> bindable property.
		/// </summary>
		public static readonly BindableProperty IndicatorCornerRadiusProperty =
			BindableProperty.Create(
				nameof(IndicatorCornerRadius),
				typeof(CornerRadius),
				typeof(SfTabView),
				new CornerRadius(-1),
				propertyChanged: OnIndicatorCornerRadiusChanged);

		/// <summary>
		/// Identifies the <see cref="IndicatorStrokeThickness"/> bindable property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="IndicatorStrokeThickness"/> bindable property.
		/// </value>
		public static readonly BindableProperty IndicatorStrokeThicknessProperty =
			BindableProperty.Create(
				nameof(IndicatorStrokeThickness),
				typeof(double),
				typeof(SfTabView),
				3d,
				propertyChanged: OnIndicatorStrokeThicknessChanged);

		/// <summary>
		/// Identifies the <see cref="FontAutoScalingEnabled"/> bindable property.
		/// </summary>
		public static readonly BindableProperty FontAutoScalingEnabledProperty =
			BindableProperty.Create(
				nameof(FontAutoScalingEnabled),
				typeof(bool),
				typeof(SfTabView),
				false,
				propertyChanged: OnFontAutoScalingEnabledChanged);

		/// <summary>
		/// Identifies the <see cref="EnableSwiping"/> bindable property.
		/// </summary>
		public static readonly BindableProperty EnableSwipingProperty =
			BindableProperty.Create(
				nameof(EnableSwiping),
				typeof(bool),
				typeof(SfTabView),
				false,
				BindingMode.Default,
				null);

		/// <summary>
		/// Identifies the <see cref="EnableVirtualization"/> bindable property.
		/// </summary>
		public static readonly BindableProperty EnableVirtualizationProperty =
			BindableProperty.Create(
				nameof(EnableVirtualization),
				typeof(bool),
				typeof(SfTabView),
				false,
				propertyChanged: OnEnableVirtualizationChanged);

		/// <summary>
		/// Identifies the <see cref="IsCenterButtonEnabled"/> bindable property.
		/// </summary>
		public static readonly BindableProperty IsCenterButtonEnabledProperty =
			BindableProperty.Create(
				nameof(IsCenterButtonEnabled),
				typeof(bool),
				typeof(SfTabView),
				false,
				BindingMode.Default,
				null,
				propertyChanged: OnIsCenterButtonEnabledChanged);

		/// <summary>
		/// Identifies the <see cref="CenterButtonSettings"/> bindable property.
		/// </summary>
		public static readonly BindableProperty CenterButtonSettingsProperty =
			BindableProperty.Create(
				nameof(CenterButtonSettings),
				typeof(CenterButtonSettings),
				typeof(SfTabView),
				null,
				BindingMode.Default,
				null,
				defaultValueCreator: bindale => GetCenterButtonSettingsDefaultValue(),
				propertyChanged: OnCenterButtonSettingsChanged);

		/// <summary>
		/// Identifies the <see cref="IsContentTransitionEnabled"/> bindable property.
		/// </summary>
		public static readonly BindableProperty IsContentTransitionEnabledProperty =
			BindableProperty.Create(
				nameof(IsContentTransitionEnabled),
				typeof(bool),
				typeof(SfTabView),
				true,
				propertyChanged: OnIsContentTransitionEnabledChanged);

		static readonly BindableProperty IsContentLoopingEnabledProperty =
			BindableProperty.Create(
				nameof(IsContentLoopingEnabled),
				typeof(bool),
				typeof(SfTabView),
				false);

		/// <summary>
		/// Identifies the <see cref="ScrollButtonBackground"/> bindable property.
		/// </summary>
		public static readonly BindableProperty ScrollButtonBackgroundProperty =
			BindableProperty.Create(
				nameof(ScrollButtonBackground),
				typeof(Brush),
				typeof(SfTabView),
				new SolidColorBrush(Color.FromArgb("#F7F2FB")),
				BindingMode.Default, null,
				propertyChanged: OnScrollButtonBackgroundPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="ScrollButtonColor"/> bindable property.
		/// </summary>
		public static readonly BindableProperty ScrollButtonColorProperty =
			BindableProperty.Create(
				nameof(ScrollButtonColor),
				typeof(Color),
				typeof(SfTabView),
				Color.FromArgb("#49454F"),
				BindingMode.Default,
				null,
				propertyChanged: OnScrollButtonColorChanged);

		/// <summary>
		/// Identifies the <see cref="ScrollButtonDisabledIconColor"/> bindable property.
		/// </summary>
		internal static readonly BindableProperty ScrollButtonDisabledIconColorProperty =
			BindableProperty.Create(
				nameof(ScrollButtonDisabledIconColor),
				typeof(Color),
				typeof(SfTabView),
				Color.FromArgb("#611c1b1f"),
				BindingMode.Default,
				null,
				propertyChanged: OnScrollButtonDisabledIconColorChanged);

		/// <summary>
		/// Identifies the <see cref="HoverBackground"/> bindable property.
		/// </summary>
		/// <remarks>This bindable property is read-only.</remarks>
		internal static readonly BindableProperty HoverBackgroundProperty = BindableProperty.Create(
		nameof(HoverBackground),
		typeof(Brush),
		typeof(SfTabView),
		new SolidColorBrush(Color.FromArgb("#1C1B1F")),
		BindingMode.Default,
		propertyChanged: OnHoverBackgroundChanged);

		/// <summary>
		/// Identifies the <see cref="AnimationEasing"/> bindable property.
		/// </summary>
		public static readonly BindableProperty AnimationEasingProperty =
			BindableProperty.Create(
				nameof(AnimationEasing),
				typeof(Easing),
				typeof(SfTabView),
				Easing.Linear,
				propertyChanged: OnAnimationEasingChanged);

		/// <summary>
		/// Identifies the <see cref="EnableRippleAnimation"/> bindable property.
		/// </summary>
		public static readonly BindableProperty EnableRippleAnimationProperty =
			BindableProperty.Create(
				nameof(EnableRippleAnimation),
				typeof(bool),
				typeof(SfTabView),
				true,
				propertyChanged: OnEnableRippleAnimationChanged);

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the value that can be used to customize the indicator width in the tab header.
		/// </summary>
		/// <value>
		/// It accepts <see cref="IndicatorWidthMode"/> values and the default value is <see cref="IndicatorWidthMode.Fit"/>.
		/// </value>
		///<remarks>
		/// It's not applicable when the <see cref="IndicatorPlacement"/> is <see cref="TabIndicatorPlacement.Fill"/>.
		/// </remarks>
		/// <example>
		/// Here is an example of how to set the <see cref="IndicatorWidthMode"/> property.
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <tabView:SfTabView IndicatorWidthMode="Fit">
		///     <tabView:SfTabItem Header="TAB 1">
		///         <tabView:SfTabItem.Content>
		///             <Label Text="Content" />
		///         </tabView:SfTabItem.Content>
		///     </tabView:SfTabItem>
		/// </tabView:SfTabView>
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var tabView = new SfTabView();
		/// tabView.IndicatorWidthMode = IndicatorWidthMode.Fit;
		/// var tabItems = new TabItemCollection
		/// {
		///     new SfTabItem
		///     {
		///         Header = "TAB 1",
		///         Content = new Label { Text = "Content" }
		///     }
		/// };
		///
		/// tabView.Items = tabItems;
		/// Content = tabView;
		/// ]]></code>
		/// </example>
		public IndicatorWidthMode IndicatorWidthMode
		{
			get => (IndicatorWidthMode)GetValue(IndicatorWidthModeProperty);
			set => SetValue(IndicatorWidthModeProperty, value);
		}


		/// <summary>
		/// Gets or sets the display mode for the tab header, determining whether to show the image or text.
		/// </summary>
		/// <value>
		/// It accepts the <see cref="TabBarDisplayMode"/> values, and the default value is <see cref="TabBarDisplayMode.Default"/>.
		/// </value>
		/// <remarks>
		/// When the HeaderDisplayMode is set to <see cref="TabBarDisplayMode.Default"/>, the image and text will be displayed on the tab bar based on the <see cref="SfTabItem.Header"/> and <see cref="SfTabItem.ImageSource"/> properties of the tab item.
		/// </remarks>
		/// <example>
		/// Here is an example of how to set the <see cref="HeaderDisplayMode"/> property.
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <tabView:SfTabView HeaderDisplayMode="Text">
		///     <tabView:SfTabItem Header="TAB 1">
		///         <tabView:SfTabItem.Content>
		///             <Label Text="Content" />
		///         </tabView:SfTabItem.Content>
		///     </tabView:SfTabItem>
		/// </tabView:SfTabView>
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var tabView = new SfTabView();
		/// tabView.HeaderDisplayMode = TabBarDisplayMode.Text;
		/// var tabItems = new TabItemCollection
		/// {
		///     new SfTabItem
		///     {
		///         Header = "TAB 1",
		///         Content = new Label { Text = "Content" }
		///     }
		/// };
		///
		/// tabView.Items = tabItems;
		/// Content = tabView;
		/// ]]></code>
		/// </example>
		public TabBarDisplayMode HeaderDisplayMode
		{
			get => (TabBarDisplayMode)GetValue(HeaderDisplayModeProperty);
			set => SetValue(HeaderDisplayModeProperty, value);
		}

		/// <summary>
		/// Gets or sets the background color of the tab bar.
		/// </summary>
		/// <value>
		/// The background color of the tab bar. The default value is null.
		/// </value>
		/// <example>
		/// Here is an example of how to set the <see cref="TabBarBackground"/> property.
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <tabView:SfTabView TabBarBackground="Pink">
		///     <tabView:SfTabItem Header="TAB 1">
		///         <tabView:SfTabItem.Content>
		///             <Label Text="Content" />
		///         </tabView:SfTabItem.Content>
		///     </tabView:SfTabItem>
		/// </tabView:SfTabView>
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var tabView = new SfTabView();
		/// tabView.TabBarBackground = Colors.Pink;
		/// var tabItems = new TabItemCollection
		/// {
		///     new SfTabItem
		///     {
		///         Header = "TAB 1",
		///         Content = new Label { Text = "Content" }
		///     }
		/// };
		///
		/// tabView.Items = tabItems;
		/// Content = tabView;
		/// ]]></code>
		/// </example>
		public Brush TabBarBackground
		{
			get => (Brush)GetValue(TabBarBackgroundProperty);
			set => SetValue(TabBarBackgroundProperty, value);
		}

		/// <summary>
		/// Gets or sets whether the tab header should be at the bottom or at the top of the tab content.
		/// </summary>
		/// <value>
		/// One of the <see cref="Syncfusion.Maui.Toolkit.TabView.TabBarPlacement"/> enumeration that specifies the placement of the tab bar. The default mode is <see cref="Syncfusion.Maui.Toolkit.TabView.TabBarPlacement.Top"/>.
		/// </value>
		/// <example>
		/// Here is an example of how to set the <see cref="TabBarPlacement"/> property.
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <tabView:SfTabView TabBarPlacement="Bottom">
		///     <tabView:SfTabItem Header="TAB 1">
		///         <tabView:SfTabItem.Content>
		///             <Label Text="Content" />
		///         </tabView:SfTabItem.Content>
		///     </tabView:SfTabItem>
		/// </tabView:SfTabView>
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var tabView = new SfTabView();
		/// tabView.TabBarPlacement = TabBarPlacement.Bottom;
		/// var tabItems = new TabItemCollection
		/// {
		///     new SfTabItem
		///     {
		///         Header = "TAB 1",
		///         Content = new Label { Text = "Content" }
		///     }
		/// };
		///
		/// tabView.Items = tabItems;
		/// Content = tabView;
		/// ]]></code>
		/// </example>
		public TabBarPlacement TabBarPlacement
		{
			get => (TabBarPlacement)GetValue(TabBarPlacementProperty);
			set => SetValue(TabBarPlacementProperty, value);
		}

		/// <summary>
		/// Gets or sets the placement of the selection indication.
		/// </summary>
		/// <value>
		/// One of the <see cref="Syncfusion.Maui.Toolkit.TabView.TabIndicatorPlacement"/> enumeration that specifies the placement of the selection indicator. The default mode is <see cref="Syncfusion.Maui.Toolkit.TabView.TabIndicatorPlacement.Bottom"/>.
		/// </value>
		/// <example>
		/// Here is an example of how to set the <see cref="IndicatorPlacement"/> property.
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <tabView:SfTabView IndicatorPlacement="Fill">
		///     <tabView:SfTabItem Header="TAB 1">
		///         <tabView:SfTabItem.Content>
		///             <Label Text="Content" />
		///         </tabView:SfTabItem.Content>
		///     </tabView:SfTabItem>
		/// </tabView:SfTabView>
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var tabView = new SfTabView();
		/// tabView.IndicatorPlacement = TabIndicatorPlacement.Fill;
		/// var tabItems = new TabItemCollection
		/// {
		///     new SfTabItem
		///     {
		///         Header = "TAB 1",
		///         Content = new Label { Text = "Content" }
		///     }
		/// };
		///
		/// tabView.Items = tabItems;
		/// Content = tabView;
		/// ]]></code>
		/// </example>
		public TabIndicatorPlacement IndicatorPlacement
		{
			get => (TabIndicatorPlacement)GetValue(IndicatorPlacementProperty);
			set => SetValue(IndicatorPlacementProperty, value);
		}

		/// <summary>
		/// Gets or sets the value for determining the width mode of each tab.
		/// </summary>
		/// <value>
		/// One of the <see cref="Syncfusion.Maui.Toolkit.TabView.TabWidthMode"/> enumeration values that specifies the tab item's width. The default mode is <see cref="Syncfusion.Maui.Toolkit.TabView.TabWidthMode.Default"/>.
		/// </value>
		/// <example>
		/// Here is an example of how to set the <see cref="TabWidthMode"/> property.
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <tabView:SfTabView TabWidthMode="SizeToContent">
		///     <tabView:SfTabItem Header="TAB 1">
		///         <tabView:SfTabItem.Content>
		///             <Label Text="Content" />
		///         </tabView:SfTabItem.Content>
		///     </tabView:SfTabItem>
		/// </tabView:SfTabView>
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var tabView = new SfTabView();
		/// tabView.TabWidthMode = TabWidthMode.SizeToContent;
		/// var tabItems = new TabItemCollection
		/// {
		///     new SfTabItem
		///     {
		///         Header = "TAB 1",
		///         Content = new Label { Text = "Content" }
		///     }
		/// };
		///
		/// tabView.Items = tabItems;
		/// Content = tabView;
		/// ]]></code>
		/// </example>
		public TabWidthMode TabWidthMode
		{
			get => (TabWidthMode)GetValue(TabWidthModeProperty);
			set => SetValue(TabWidthModeProperty, value);
		}

		/// <summary>
		/// Gets or sets a brush that describes the selection indicator's background.
		/// </summary>
		/// <value>
		/// Specifies the background of the selection indicator. The default value is SolidColorBrush(Color.FromArgb("#6200EE")).
		/// </value>
		/// <example>
		/// Here is an example of how to set the <see cref="IndicatorBackground"/> property.
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <tabView:SfTabView IndicatorBackground="Red">
		///     <tabView:SfTabItem Header="TAB 1">
		///         <tabView:SfTabItem.Content>
		///             <Label Text="Content" />
		///         </tabView:SfTabItem.Content>
		///     </tabView:SfTabItem>
		/// </tabView:SfTabView>
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var tabView = new SfTabView();
		/// tabView.IndicatorBackground = Colors.Red; 
		/// var tabItems = new TabItemCollection
		/// {
		///     new SfTabItem
		///     {
		///         Header = "TAB 1",
		///         Content = new Label { Text = "Content" }
		///     }
		/// };
		///
		/// tabView.Items = tabItems;
		/// Content = tabView;
		/// ]]></code>
		/// </example>
		public Brush IndicatorBackground
		{
			get => (Brush)GetValue(IndicatorBackgroundProperty);
			set => SetValue(IndicatorBackgroundProperty, value);
		}

		/// <summary>
		/// Gets or sets the height of the tab header.
		/// </summary>
		/// <value>
		/// Specifies the height of the tab header. The default value is 48d.
		/// </value>
		/// <example>
		/// Here is an example of how to set the <see cref="TabBarHeight"/> property.
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <tabView:SfTabView TabBarHeight="80">
		///     <tabView:SfTabItem Header="TAB 1">
		///         <tabView:SfTabItem.Content>
		///             <Label Text="Content" />
		///         </tabView:SfTabItem.Content>
		///     </tabView:SfTabItem>
		/// </tabView:SfTabView>
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var tabView = new SfTabView();
		/// tabView.TabBarHeight = 80;
		/// var tabItems = new TabItemCollection
		/// {
		///     new SfTabItem
		///     {
		///         Header = "TAB 1",
		///         Content = new Label { Text = "Content" }
		///     }
		/// };
		///
		/// tabView.Items = tabItems;
		/// Content = tabView;
		/// ]]></code>
		/// </example>
		public double TabBarHeight
		{
			get => (double)GetValue(TabBarHeightProperty);
			set => SetValue(TabBarHeightProperty, value);
		}

		/// <summary>
		/// Gets or sets the collection used to populate the content of the <see cref="SfTabView"/>.
		/// </summary>
		/// <value>
		/// The collection used to populate the content of the <see cref="SfTabView"/>. The default is an empty collection.
		/// </value>
		/// <example>
		/// Here is an example of how to set the <see cref="Items"/> property.
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <tabView:SfTabView>
		///     <tabView:SfTabView.Items>
		///         <tabView:SfTabItem Header="TAB 1">
		///             <tabView:SfTabItem.Content>
		///                 <Label Text="Content" />
		///             </tabView:SfTabItem.Content>
		///         </tabView:SfTabItem>
		///     </tabView:SfTabView.Items>
		/// </tabView:SfTabView>
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var tabView = new SfTabView();
		/// var tabItems = new TabItemCollection
		/// {
		///     new SfTabItem
		///     {
		///         Header = "TAB 1",
		///         Content = new Label { Text = "Content" }
		///     }
		/// };
		///
		/// tabView.Items = tabItems;
		/// Content = tabView;
		/// ]]></code>
		/// </example>
		public TabItemCollection Items
		{
			get => (TabItemCollection)GetValue(ItemsProperty);
			set => SetValue(ItemsProperty, value);
		}

		/// <summary> 
		/// Gets or sets a value that can be used to customize the horizontal text alignment in tab header. 
		/// </summary> 
		/// <value> 
		/// It accepts the <see cref="HeaderHorizontalTextAlignment"/> values, and the default value is <see cref="TextAlignment.Center"/>. 
		/// </value> 
		/// <example>
		/// Here is an example of how to set the <see cref="HeaderHorizontalTextAlignment"/> property.
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <tabView:SfTabView HeaderHorizontalTextAlignment="Center">
		///     <tabView:SfTabItem Header="TAB 1">
		///         <tabView:SfTabItem.Content>
		///             <Label Text="Content" />
		///         </tabView:SfTabItem.Content>
		///     </tabView:SfTabItem>
		/// </tabView:SfTabView>
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var tabView = new SfTabView();
		/// tabView.HeaderHorizontalTextAlignment = TextAlignment.Center;
		/// var tabItems = new TabItemCollection
		/// {
		///     new SfTabItem
		///     {
		///         Header = "TAB 1",
		///         Content = new Label { Text = "Content" }
		///     }
		/// };
		///
		/// tabView.Items = tabItems;
		/// Content = tabView;
		/// ]]></code>
		/// </example>
		public TextAlignment HeaderHorizontalTextAlignment
		{
			get => (TextAlignment)GetValue(HeaderHorizontalTextAlignmentProperty);
			set => SetValue(HeaderHorizontalTextAlignmentProperty, value);
		}

		/// <summary>
		/// Gets or sets the index specifying the currently selected item.
		/// </summary>
		/// <value>
		/// A zero-based index of the currently selected item.
		/// </value>
		/// <example>
		/// Here is an example of how to set the <see cref="SelectedIndex"/> property.
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <tabView:SfTabView SelectedIndex="2">
		///     <tabView:SfTabItem Header="TAB 1">
		///         <tabView:SfTabItem.Content>
		///             <Label Text="Content 1" />
		///         </tabView:SfTabItem.Content>
		///     </tabView:SfTabItem>
		///     
		///     <tabView:SfTabItem Header="TAB 2">
		///         <tabView:SfTabItem.Content>
		///             <Label Text="Content 2" />
		///         </tabView:SfTabItem.Content>
		///     </tabView:SfTabItem>
		///     
		///     <tabView:SfTabItem Header="TAB 3">
		///         <tabView:SfTabItem.Content>
		///             <Label Text="Content 3" />
		///         </tabView:SfTabItem.Content>
		///     </tabView:SfTabItem>
		/// </tabView:SfTabView>
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var tabView = new SfTabView();
		/// tabView.SelectedIndex = 2;
		/// var tabItems = new TabItemCollection
		/// {
		///     new SfTabItem
		///     {
		///         Header = "TAB 1",
		///         Content = new Label { Text = "Content 1" }
		///     },
		///     new SfTabItem
		///     {
		///         Header = "TAB 2",
		///         Content = new Label { Text = "Content 2" }
		///     },
		///     new SfTabItem
		///     {
		///         Header = "TAB 3",
		///         Content = new Label { Text = "Content 3" }
		///     }
		/// };
		///
		/// tabView.Items = tabItems;
		/// Content = tabView;
		/// ]]></code>
		/// </example>
		public int SelectedIndex
		{
			get => (int)GetValue(SelectedIndexProperty);
			set => SetValue(SelectedIndexProperty, value);
		}

		/// <summary>
		/// Gets or sets the collection used to populate the content of the <see cref="SfTabView"/>.
		/// </summary>
		/// <value>
		/// The collection used to populate the content of the Tab View. The default is an empty collection.
		/// </value>
		/// <example>
		/// Here is an example of how to set the <see cref="ItemsSource"/> property.
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <tabView:SfTabView ItemsSource = "{Binding TabItems}" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// SfTabView tabView = new SfTabView();
		/// tabView.ItemsSource = model.TabItems;
		/// Content = tabView;
		/// ]]></code>
		/// </example>
		public IList? ItemsSource
		{
			get => (IList?)GetValue(ItemsSourceProperty);
			set => SetValue(ItemsSourceProperty, value);
		}

		/// <summary>
		/// Gets or sets the template that is used to display the header item.
		/// </summary>
		/// <value>
		/// A <see cref="DataTemplate"/> object that is used to display the header item. The default is null.
		/// </value>
		/// <example>
		/// Here is an example of how to set the <see cref="HeaderItemTemplate"/> property.
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <tabView:SfTabView ItemsSource="{Binding TabItems}">
		/// <tabView:SfTabView.HeaderItemTemplate>
		///     <DataTemplate >
		///        <Label Padding = "5,10,10,10"  Text="{Binding Name}"/>
		///     </DataTemplate>
		/// </tabView:SfTabView.HeaderItemTemplate>
		/// </tabView:SfTabView>
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var tabView = new SfTabView();
		/// tabView.ItemsSource = model.TabItems;
		/// tabView.HeaderItemTemplate = new DataTemplate(() =>
		/// {
		///    var nameLabel = new Label { Padding = new Thickness(5,10,10,10) };
		///    nameLabel.SetBinding(Label.TextProperty, "Name");
		///    return nameLabel;
		/// });
		/// Content = tabView;
		/// ]]></code>
		/// </example>
		public DataTemplate? HeaderItemTemplate
		{
			get => (DataTemplate?)GetValue(HeaderItemTemplateProperty);
			set => SetValue(HeaderItemTemplateProperty, value);
		}

		/// <summary>
		/// Gets or sets the template that is used to display the content.
		/// </summary>
		/// <value>
		/// A <see cref="DataTemplate"/> object that is used to display the content. The default is null.
		/// </value>
		/// <example>
		/// Here is an example of how to set the <see cref="ContentItemTemplate"/> property.
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <tabView:SfTabView ItemsSource="{Binding TabItems}" >
		///  <tabView:SfTabView.ContentItemTemplate>
		///        <DataTemplate>
		///             <Label TextColor = "Black"  Text="{Binding Name}" />
		///       </DataTemplate>
		/// </tabView:SfTabView.ContentItemTemplate>
		/// </tabView:SfTabView>
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var tabView = new SfTabView();
		/// tabView.ItemsSource = model.TabItems;
		/// tabView.ContentItemTemplate = new DataTemplate(() =>
		/// {
		///   var nameLabel = new Label { TextColor = Colors.Black };
		///   nameLabel.SetBinding(Label.TextProperty, "Name");
		///   return nameLabel;
		/// });
		/// Content = tabView;
		/// ]]></code>
		/// </example>
		public DataTemplate? ContentItemTemplate
		{
			get => (DataTemplate?)GetValue(ContentItemTemplateProperty);
			set => SetValue(ContentItemTemplateProperty, value);
		}

		/// <summary>
		/// Gets or sets the value that can be used to customize the padding of the tab header. 
		/// </summary>
		/// <remarks>
		/// <b>Note:</b> This is only applied to the SizeToContent type of <see cref="TabWidthMode"/>.
		/// </remarks>
		/// <value>
		/// Specifies the padding of the tab header. The default value is new Thickness(52,0,52,0).
		/// </value>
		/// <example>
		/// Here is an example of how to set the <see cref="TabHeaderPadding"/> property.
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <tabView:SfTabView TabWidthMode="SizeToContent" TabHeaderPadding="5,10,5,10">
		///     <tabView:SfTabItem Header="TAB 1">
		///         <tabView:SfTabItem.Content>
		///             <Label Text="Content 1" />
		///         </tabView:SfTabItem.Content>
		///     </tabView:SfTabItem>
		///     
		///     <tabView:SfTabItem Header="TAB 2">
		///         <tabView:SfTabItem.Content>
		///             <Label Text="Content 2" />
		///         </tabView:SfTabItem.Content>
		///     </tabView:SfTabItem>
		///     
		///     <tabView:SfTabItem Header="TAB 3">
		///         <tabView:SfTabItem.Content>
		///             <Label Text="Content 3" />
		///         </tabView:SfTabItem.Content>
		///     </tabView:SfTabItem>
		/// </tabView:SfTabView>
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var tabView = new SfTabView();
		/// tabView.TabWidthMode = TabWidthMode.SizeToContent;
		/// tabView.TabHeaderPadding = new Thickness(5, 10, 5, 10);
		/// var tabItems = new TabItemCollection
		/// {
		///     new SfTabItem
		///     {
		///         Header = "TAB 1",
		///         Content = new Label { Text = "Content 1" }
		///     },
		///     new SfTabItem
		///     {
		///         Header = "TAB 2",
		///         Content = new Label { Text = "Content 2" }
		///     },
		///     new SfTabItem
		///     {
		///         Header = "TAB 3",
		///         Content = new Label { Text = "Content 3" }
		///     }
		/// };
		///
		/// tabView.Items = tabItems;
		/// Content = tabView;
		/// ]]></code>
		/// </example>
		public Thickness TabHeaderPadding
		{
			get => (Thickness)GetValue(TabHeaderPaddingProperty);
			set => SetValue(TabHeaderPaddingProperty, value);
		}

		/// <summary> 
		/// Gets or sets a value that determines whether the font of the control should scale automatically according to the operating system settings. 
		/// </summary>
		/// <value>
		/// A boolean value indicating whether font auto-scaling is enabled. The default value is false.
		/// </value>
		/// <example>
		/// Here is an example of how to set the <see cref="FontAutoScalingEnabled"/> property.
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <tabView:SfTabView FontAutoScalingEnabled="True">
		///     <tabView:SfTabItem Header="TAB 1">
		///         <tabView:SfTabItem.Content>
		///             <Label Text="Content" />
		///         </tabView:SfTabItem.Content>
		///     </tabView:SfTabItem>
		/// </tabView:SfTabView>
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var tabView = new SfTabView();
		/// tabView.FontAutoScalingEnabled = true;
		/// var tabItems = new TabItemCollection
		/// {
		///     new SfTabItem
		///     {
		///         Header = "TAB 1",
		///         Content = new Label { Text = "Content" }
		///     }
		/// };
		///
		/// tabView.Items = tabItems;
		/// Content = tabView;
		/// ]]></code>
		/// </example>
		public bool FontAutoScalingEnabled
		{
			get { return (bool)GetValue(FontAutoScalingEnabledProperty); }
			set { SetValue(FontAutoScalingEnabledProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value that can be used to customize the corner radius of the selection indicator.
		/// </summary>
		/// <value>
		/// A <see cref="CornerRadius"/> value that specifies the corner radius of the selection indicator. The default value is -1.
		/// </value>
		/// <example>
		/// Here is an example of how to set the <see cref="IndicatorCornerRadius"/> property.
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <tabView:SfTabView IndicatorCornerRadius="5,5,5,5">
		///     <tabView:SfTabItem Header="TAB 1">
		///         <tabView:SfTabItem.Content>
		///             <Label Text="Content" />
		///         </tabView:SfTabItem.Content>
		///     </tabView:SfTabItem>
		/// </tabView:SfTabView>
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var tabView = new SfTabView();
		/// tabView.IndicatorCornerRadius = new CornerRadius(5, 5, 5, 5);
		/// var tabItems = new TabItemCollection
		/// {
		///     new SfTabItem
		///     {
		///         Header = "TAB 1",
		///         Content = new Label { Text = "Content" }
		///     }
		/// };
		///
		/// tabView.Items = tabItems;
		/// Content = tabView;
		/// ]]></code>
		/// </example>
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
		public double IndicatorStrokeThickness
		{
			get => (double)this.GetValue(IndicatorStrokeThicknessProperty);
			set => this.SetValue(IndicatorStrokeThicknessProperty, value);
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
		/// <example>
		/// Here is an example of how to set the <see cref="TabHeaderAlignment"/> property.
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <tabView:SfTabView TabWidthMode="SizeToContent" TabHeaderAlignment="Center">
		///     <tabView:SfTabItem Header="TAB 1">
		///         <tabView:SfTabItem.Content>
		///             <Label Text="Content" />
		///         </tabView:SfTabItem.Content>
		///     </tabView:SfTabItem>
		///     <tabView:SfTabItem Header="TAB 2">
		///         <tabView:SfTabItem.Content>
		///             <Label Text="More Content" />
		///         </tabView:SfTabItem.Content>
		///     </tabView:SfTabItem>
		/// </tabView:SfTabView>
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var tabView = new SfTabView();
		/// tabView.TabWidthMode = TabWidthMode.SizeToContent;
		/// tabView.TabHeaderAlignment = TabHeaderAlignment.Center;
		/// var tabItems = new TabItemCollection
		/// {
		///     new SfTabItem
		///     {
		///         Header = "TAB 1",
		///         Content = new Label { Text = "Content" }
		///     },
		///     new SfTabItem
		///     {
		///         Header = "TAB 2",
		///         Content = new Label { Text = "More Content" }
		///     }
		/// };
		///
		/// tabView.Items = tabItems;
		/// Content = tabView;
		/// ]]></code>
		/// </example>
		public TabHeaderAlignment TabHeaderAlignment
		{
			get => (TabHeaderAlignment)GetValue(TabHeaderAlignmentProperty);
			set => SetValue(TabHeaderAlignmentProperty, value);
		}

		/// <summary>
		/// Gets or sets a value indicating whether to enable the scroll buttons.
		/// </summary>
		/// <value>
		/// A boolean value indicating whether the scroll buttons are enabled. The default value is false.
		/// </value>
		/// <example>
		/// Here is an example of how to set the <see cref="IsScrollButtonEnabled"/> property.
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <tabView:SfTabView IsScrollButtonEnabled="True">
		///     <tabView:SfTabItem Header="TAB 1">
		///         <tabView:SfTabItem.Content>
		///             <Label Text="Content" />
		///         </tabView:SfTabItem.Content>
		///     </tabView:SfTabItem>
		///     <tabView:SfTabItem Header="TAB 2">
		///         <tabView:SfTabItem.Content>
		///             <Label Text="More Content" />
		///         </tabView:SfTabItem.Content>
		///     </tabView:SfTabItem>
		/// </tabView:SfTabView>
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var tabView = new SfTabView();
		/// tabView.IsScrollButtonEnabled = true;
		/// var tabItems = new TabItemCollection
		/// {
		///     new SfTabItem
		///     {
		///         Header = "TAB 1",
		///         Content = new Label { Text = "Content" }
		///     },
		///     new SfTabItem
		///     {
		///         Header = "TAB 2",
		///         Content = new Label { Text = "More Content" }
		///     }
		/// };
		///
		/// tabView.Items = tabItems;
		/// Content = tabView;
		/// ]]></code>
		/// </example>
		public bool IsScrollButtonEnabled
		{
			get => (bool)GetValue(IsScrollButtonEnabledProperty);
			set => SetValue(IsScrollButtonEnabledProperty, value);
		}

		/// <summary> 
		/// Gets or sets a value indicating whether swiping is enabled in <see cref="SfTabView"/>. 
		/// </summary> 
		/// <value> 
		/// A boolean value indicating whether swiping is enabled. The default value is false.
		/// </value> 
		/// <example>
		/// Here is an example of how to set the <see cref="EnableSwiping"/> property.
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <tabView:SfTabView EnableSwiping="True">
		///     <tabView:SfTabItem Header="TAB 1">
		///         <tabView:SfTabItem.Content>
		///             <Label Text="Content" />
		///         </tabView:SfTabItem.Content>
		///     </tabView:SfTabItem>
		///     <tabView:SfTabItem Header="TAB 2">
		///         <tabView:SfTabItem.Content>
		///             <Label Text="More Content" />
		///         </tabView:SfTabItem.Content>
		///     </tabView:SfTabItem>
		/// </tabView:SfTabView>
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var tabView = new SfTabView();
		/// tabView.EnableSwiping = true;
		/// var tabItems = new TabItemCollection
		/// {
		///     new SfTabItem
		///     {
		///         Header = "TAB 1",
		///         Content = new Label { Text = "Content" }
		///     },
		///     new SfTabItem
		///     {
		///         Header = "TAB 2",
		///         Content = new Label { Text = "More Content" }
		///     }
		/// };
		///
		/// tabView.Items = tabItems;
		/// Content = tabView;
		/// ]]></code>
		/// </example>
		public bool EnableSwiping
		{
			get => (bool)GetValue(EnableSwipingProperty);
			set => SetValue(EnableSwipingProperty, value);
		}

		/// <summary>
		/// Gets or sets a value that can be used to modify the duration of content transition in the tab view.
		/// </summary>
		/// <value>
		/// A double value that specifies the duration of the content transition. The default value is 100.
		/// </value>
		/// <example>
		/// Here is an example of how to set the <see cref="ContentTransitionDuration"/> property.
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <tabView:SfTabView ContentTransitionDuration="0.5" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// SfTabView tabView = new SfTabView();
		/// tabView.ContentTransitionDuration = 0.5;
		/// ]]></code>
		/// </example>
		public double ContentTransitionDuration
		{
			get => (double)GetValue(ContentTransitionDurationProperty);
			set => SetValue(ContentTransitionDurationProperty, value);
		}

		///  <summary>
		/// Gets or sets a value that can be used to customize the scroll button’s background color in the <see cref="SfTabView"/>.
		/// </summary>
		/// <value>
		/// A <see cref="Brush"/> value that specifies the background color of the scroll button.
		/// </value>
		public Brush ScrollButtonBackground
		{
			get { return (Brush)GetValue(ScrollButtonBackgroundProperty); }
			set { SetValue(ScrollButtonBackgroundProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value that can be used to customize the scroll button’s foreground color in the <see cref="SfTabView"/>.
		/// </summary>
		/// <value>
		/// A <see cref="Color"/> value that specifies the foreground color of the scroll button.
		/// </value>
		public Color ScrollButtonColor
		{
			get { return (Color)GetValue(ScrollButtonColorProperty); }
			set { SetValue(ScrollButtonColorProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether lazy loading is enabled during the initial load.
		/// </summary>
		/// <value>
		/// A boolean value indicating whether lazy loading is enabled. The default value is false.
		/// </value>
		/// <example>
		/// Here is an example of how to set the <see cref="EnableVirtualization"/> property.
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <tabView:SfTabView EnableVirtualization="True" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// SfTabView tabView = new SfTabView();
		/// tabView.EnableVirtualization = true;
		/// ]]></code>
		/// </example>
		public bool EnableVirtualization
		{
			get => (bool)GetValue(EnableVirtualizationProperty);
			set => SetValue(EnableVirtualizationProperty, value);
		}

		/// <summary>
		/// Gets or sets a value that indicates whether the center button is enabled in the Tab View.
		/// </summary>
		/// <value>
		/// It accepts the Boolean values, and the default value is false. 
		/// </value>
		/// <example>
		/// Here is an example of how to set the <see cref="IsCenterButtonEnabled"/> property.
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <tabView:SfTabView IsCenterButtonEnabled="True" />
		/// ]]></code>
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// SfTabView tabView = new SfTabView();
		/// tabView.IsCenterButtonEnabled = true;
		/// ]]></code>
		/// </example>
		public bool IsCenterButtonEnabled
		{
			get => (bool)GetValue(IsCenterButtonEnabledProperty);
			set => SetValue(IsCenterButtonEnabledProperty, value);
		}

		/// <summary>
		/// Gets or sets a value that can be used to customize the appearance of the center button in Tab View.
		/// </summary>
		/// <value>
		/// It accepts the <see cref="CenterButtonSettings"/> value.
		/// </value>
		/// <example>
		/// The below examples show, how to use the property.
		/// # [XAML](#tab/tabid-3)
		/// <code Lang="XAML"><![CDATA[
		/// <tabView:SfTabView x:Name="tabView" IsCenterButtonEnabled="True">
		/// <tabView:SfTabView.CenterButtonSettings>
		/// <tabView:CenterButtonSettings Height="75", Width="150", CornerRadius="10"
		///								  StrokeThickness="3" Stroke="Aqua" TextColor="HotPink"
		///								  Title="Home" FontAttributes="Bold" FontSize="20" FontFamily="SevillanaRegular"
		///								  ImageSource="home.png" DisplayMode="ImageWithText" ImageSize="30"/>
		/// </tabView:SfTabView.CenterButtonSettings>
		/// </tabView:SfTabView>
		/// ]]></code>
		/// # [C#](#tab/tabid-4)
		/// <code Lang="C#"><![CDATA[
		/// var tabView = new SfTabView()
		/// {
		///	   IsCenterButtonEnabled=true,
		///    CenterButtonSettings = new CenterButtonSettings()
		///    {
		///        Height = 75,
		///        Width = 150,
		///        CornerRadius = 10,
		///        StrokeThickness = 3,
		///        Stroke = Colors.Aqua,
		///        TextColor = Colors.HotPink,
		///        Title = "Home",
		///        FontAttributes = FontAttributes.Bold,
		///        FontSize = 20,
		///        FontFamily = "SevillanaRegular",
		///        ImageSource = "home.png",
		///        DisplayMode = CenterButtonDisplayMode.ImageWithText,
		///        ImageSize = 30
		///    },
		/// };
		/// ]]></code>
		/// </example>
		public CenterButtonSettings CenterButtonSettings
		{
			get { return (CenterButtonSettings)GetValue(CenterButtonSettingsProperty); }
			set { SetValue(CenterButtonSettingsProperty, value); }
		}

		/// <summary>  
		/// Gets or sets a value that determines whether a transition animation is enabled for the tab content when switching between tabs. 
		/// </summary>  
		/// <value>  
		/// It accepts the Boolean values, and the default value is True.  
		/// </value>  
		/// <example>
		/// Here is an example of how to set the <see cref="IsContentTransitionEnabled"/> property.
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <tabView:SfTabView IsContentTransitionEnabled="True" />
		/// ]]></code>
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// SfTabView tabView = new SfTabView();
		/// tabView.IsContentTransitionEnabled = true;
		/// ]]></code>
		/// </example>
		public bool IsContentTransitionEnabled
		{
			get => (bool)GetValue(IsContentTransitionEnabledProperty);
			set => SetValue(IsContentTransitionEnabledProperty, value);
		}

		/// <summary>
		/// Gets or sets the color of the scroll button icon when it is disabled.
		/// </summary>
		/// <value>
		/// A <see cref="Color"/> value that specifies the color of the scroll button icon when it is disabled.
		/// </value>
		internal Color ScrollButtonDisabledIconColor
		{
			get { return (Color)GetValue(ScrollButtonDisabledIconColorProperty); }
			set { SetValue(ScrollButtonDisabledIconColorProperty, value); }
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

		bool IsContentLoopingEnabled
		{
			get => (bool)GetValue(IsContentLoopingEnabledProperty);
			set => SetValue(IsContentLoopingEnabledProperty, value);
		}
		
		/// <summary>
		/// Gets or sets the easing function used for tab transition animations.
		/// </summary>
		/// <value>
		/// An <see cref="Easing"/> function that controls the animation transitions between tabs. 
		/// This affects both the selection indicator animation and the content transition animation.
		/// The default value is <see cref="Easing.Linear"/>.
		/// </value>
		/// <example>
		/// Here is an example of how to set the <see cref="AnimationEasing"/> property.
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <tabView:SfTabView AnimationEasing="{x:Static Easing.CubicInOut}">
		///     <tabView:SfTabItem Header="TAB 1">
		///         <tabView:SfTabItem.Content>
		///             <Label Text="Content" />
		///         </tabView:SfTabItem.Content>
		///     </tabView:SfTabItem>
		/// </tabView:SfTabView>
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var tabView = new SfTabView();
		/// tabView.AnimationEasing = Easing.CubicInOut;
		/// var tabItems = new TabItemCollection
		/// {
		///     new SfTabItem
		///     {
		///         Header = "TAB 1",
		///         Content = new Label { Text = "Content" }
		///     }
		/// };
		///
		/// tabView.Items = tabItems;
		/// Content = tabView;
		/// ]]></code>
		/// </example>
		public Easing AnimationEasing
		{
			get => (Easing)GetValue(AnimationEasingProperty);
			set => SetValue(AnimationEasingProperty, value);
		}

		/// <summary>
		/// Gets or sets a value indicating whether ripple animation should be enabled on the Tab View.
		/// </summary>
		/// <value>
		/// The default value is <c>true</c>.
		/// </value>
		/// <example>
		/// Here is an example of how to set the <see cref="EnableRippleAnimation"/> property.
		/// 
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <tabView:SfTabView EnableRippleAnimation="False">
		///     <tabView:SfTabItem Header="TAB 1">
		///         <tabView:SfTabItem.Content>
		///             <Label Text="Content" />
		///         </tabView:SfTabItem.Content>
		///     </tabView:SfTabItem>
		/// </tabView:SfTabView>
		/// ]]>
		/// </code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var tabView = new SfTabView();
		/// tabView.EnableRippleAnimation = false;
		/// var tabItems = new TabItemCollection
		/// {
		///     new SfTabItem
		///     {
		///         Header = "TAB 1",
		///         Content = new Label { Text = "Content" }
		///     }
		/// };
		///
		/// tabView.Items = tabItems;
		/// Content = tabView;
		/// ]]>
		/// </code>
		/// </example>
		public bool EnableRippleAnimation
		{
			get => (bool)GetValue(EnableRippleAnimationProperty);
			set => SetValue(EnableRippleAnimationProperty, value);
		}

		#endregion

		#region Events

		/// <summary>
		/// Occurs when the current selection is changed.
		/// </summary>
		/// <example>
		/// Here is an example of how to register the <see cref="SelectionChanged"/> event.
		/// 
		/// # [C#](#tab/tabid-1)
		/// <code><![CDATA[
		/// SfTabView tabView = new SfTabView();
		/// tabView.SelectionChanged += TabView_SelectionChanged;
		/// private void TabView_SelectionChanged(object sender, TabSelectionChangedEventArgs e)
		/// {
		///     double newValue = e.NewIndex;
		///     double oldValue = e.OldIndex;
		///     e.Handled = true;
		/// }
		/// ]]></code>
		/// </example>
		public event EventHandler<TabSelectionChangedEventArgs>? SelectionChanged;

		/// <summary>
		/// Occurs when the header of the tab item is tapped.
		/// </summary>
		/// <example>
		/// Here is an example of how to register the <see cref="TabItemTapped"/> event.
		/// 
		/// # [C#](#tab/tabid-1)
		/// <code><![CDATA[
		/// SfTabView tabView = new SfTabView();
		/// tabView.SelectionChanged += TabView_TabItemTapped;
		/// private void TabView_TabItemTapped(object sender, TabItemTappedEventArgs e)
		/// {
		///    e.TabItem.FontSize = 26;
		///    e.Cancel = true;
		/// }
		/// ]]></code>
		/// </example>
		public event EventHandler<TabItemTappedEventArgs>? TabItemTapped;

		/// <summary>
		/// Occurs when the selection is changing in the <see cref="SfTabView"/>.
		/// </summary>
		/// <example>
		/// Here is an example of how to register the <see cref="SelectionChanging"/> event.
		/// 
		/// # [C#](#tab/tabid-1)
		/// <code><![CDATA[
		/// SfTabView tabView = new SfTabView();
		/// tabView.SelectionChanging += TabView_SelectionChanging;
		/// private void TabView_SelectionChanging(object sender, SelectionChangingEventArgs e)
		/// {
		///    var selectionChangingIndex = e.Index;
		///    e.Cancel = true;
		/// }
		/// ]]></code>
		/// </example>
		public event EventHandler<SelectionChangingEventArgs>? SelectionChanging;

		/// <summary>
		/// Occurs when the Center button is tapped.
		/// </summary>
		/// <example>
		/// Here is an example of how to register the <see cref="CenterButtonTapped"/> event.
		/// 
		/// # [C#](#tab/tabid-1)
		/// <code><![CDATA[
		/// SfTabView tabView = new SfTabView();
		/// tabView.CenterButtonTapped += OnCenterButtonTapped;
		/// private void OnCenterButtonTapped(object sender, SelectionChangingEventArgs e)
		/// {
		///    e.Cancel = true;
		/// }
		/// ]]></code>
		/// </example>
		public event EventHandler<EventArgs>? CenterButtonTapped;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="SfTabView"/> class.
		/// </summary>
		public SfTabView()
		{
			ThemeElement.InitializeThemeResources(this, "SfTabViewTheme");
			InitializeControl();
		}

		#endregion

		#region Override Methods

		/// <summary>
		/// Called when a property value changes.
		/// </summary>
		/// <param name="propertyName">The name of the property that changed.</param>
		/// <exclude/>
		protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
		{
			if (propertyName == "ExplicitStyle")
			{
				InitializeControl();
			}

			if (propertyName == "FlowDirection")
			{
				if (_tabHeaderContainer != null)
				{
					FlowDirection flowDirection = ((this as IVisualElementController).EffectiveFlowDirection & EffectiveFlowDirection.RightToLeft) == EffectiveFlowDirection.RightToLeft ?
								FlowDirection.RightToLeft : FlowDirection.LeftToRight;
#if !WINDOWS
					if (Content != null)
					{
						Content.FlowDirection = flowDirection;
					}
#endif
					_tabHeaderContainer.FlowDirection = flowDirection;
					_tabHeaderContainer?.UpdateFlowDirection();
				}
			}

			base.OnPropertyChanged(propertyName);
		}

		/// <summary>
		/// Call when the binding context is changed.
		/// </summary>
		/// <exclude/>
		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();
			if (CenterButtonSettings != null)
			{
				SfTabView.SetInheritedBindingContext(CenterButtonSettings, BindingContext);
			}
		}

		#endregion

		#region Internal Methods

		/// <summary>
		/// Invokes <see cref="SelectionChanged"/> event.
		/// </summary>
		/// <param name="args">The <see cref="TabSelectionChangedEventArgs"/> containing the event data.</param>
		internal void RaiseSelectionChangedEvent(TabSelectionChangedEventArgs args)
		{
			if (SelectionChanged != null && !args.Handled)
			{
				SelectionChanged(this, args);
			}
			else
			{
				args.Handled = false;
			}
		}

		/// <summary>
		/// Invokes <see cref="TabItemTapped"/> event.
		/// </summary>
		/// <param name="args">The <see cref="TabItemTappedEventArgs"/> containing the event data.</param>
		internal void RaiseTabItemTappedEvent(TabItemTappedEventArgs args)
		{
			TabItemTapped?.Invoke(this, args);
		}

		/// <summary>
		/// Invokes <see cref="SelectionChanging"/> event.
		/// </summary>
		/// <param name="args">The <see cref="SelectionChangingEventArgs"/> containing the event data.</param>
		internal void RaiseSelectionChangingEvent(SelectionChangingEventArgs args)
		{
			SelectionChanging?.Invoke(this, args);
		}

		/// <summary>
		/// Invokes <see cref="CenterButtonTapped"/> event.
		/// </summary>
		/// <param name="args">The <see cref="EventArgs"/> containing the event data.</param>
		internal void RaiseCenterButtonTappedEvent(EventArgs args)
		{
			CenterButtonTapped?.Invoke(this, args);
		}
		#endregion

		#region Property Changed Methods

		/// <summary>
		/// Handles changes to the <see cref="TabBarBackground"/> property.
		/// </summary>
		static void OnTabBarBackgroundPropertyChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as SfTabView)?.UpdateTabBarBackground((Brush)newValue);

		/// <summary>
		/// Handles changes to the <see cref="TabBarPlacement"/> property.
		/// </summary>
		static void OnTabBarPlacementChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as SfTabView)?.UpdateTabBarPlacement((TabBarPlacement)newValue);

		/// <summary>
		/// Handles changes to the <see cref="HeaderHorizontalTextAlignment"/> property.
		/// </summary>
		static void OnHeaderHorizontalTextAlignmentChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as SfTabView)?.UpdateHeaderHorizontalTextAlignment((TextAlignment)newValue);

		/// <summary>
		/// Handles changes to the <see cref="IndicatorPlacement"/> property.
		/// </summary>
		static void OnIndicatorPlacementChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as SfTabView)?.UpdateIndicatorPlacement((TabIndicatorPlacement)newValue);

		/// <summary>
		/// Handles changes to the <see cref="TabWidthMode"/> property.
		/// </summary>
		static void OnTabWidthModePropertyChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as SfTabView)?.UpdateTabWidthMode((TabWidthMode)newValue);

		/// <summary>
		/// Handles changes to the <see cref="IndicatorWidthMode"/> property.
		/// </summary>
		static void OnIndicatorWidthModePropertyChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as SfTabView)?.UpdateIndicatorWidthMode((IndicatorWidthMode)newValue);

		/// <summary>
		/// Handles changes to the <see cref="HeaderDisplayMode"/> property.
		/// </summary>
		static void OnHeaderDisplayModeChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as SfTabView)?.UpdateHeaderDisplayMode((TabBarDisplayMode)newValue);

		/// <summary>
		/// Handles changes to the <see cref="TabHeaderPadding"/> property.
		/// </summary>
		static void OnTabHeaderPaddingPropertyChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as SfTabView)?.UpdateTabHeaderPadding((Thickness)newValue);

		/// <summary>
		/// Handles changes to the <see cref="IndicatorBackground"/> property.
		/// </summary>
		static void OnIndicatorBackgroundChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as SfTabView)?.UpdateIndicatorBackground((Brush)newValue);

		/// <summary>
		/// Handles changes to the <see cref="TabBarHeight"/> property.
		/// </summary>
		static void OnTabBarHeightChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as SfTabView)?.UpdateTabBarHeight((double)newValue);

		/// <summary>
		/// Handles changes to the <see cref="Items"/> property.
		/// </summary>
		static void OnItemsChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as SfTabView)?.UpdateItems();

		/// <summary>
		/// Handles changes to the <see cref="HeaderItemTemplate"/> property.
		/// </summary>
		static void OnHeaderItemTemplateChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as SfTabView)?.UpdateHeaderItemTemplate();

		/// <summary>
		/// Handles changes to the <see cref="ContentItemTemplate"/> property.
		/// </summary>
		static void OnContentItemTemplateChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as SfTabView)?.UpdateContentItemTemplate();

		/// <summary>
		/// Handles changes to the <see cref="ItemsSource"/> property.
		/// </summary>
		static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as SfTabView)?.UpdateItemsSource();

		/// <summary>
		/// Handles changes to the <see cref="SelectedIndex"/> property.
		/// </summary>
		static void OnSelectedIndexChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as SfTabView)?.UpdateSelectedIndex((int)newValue, (int)oldValue);

		/// <summary>
		/// Handles changes to the <see cref="IsScrollButtonEnabled"/> property.
		/// </summary>
		static void OnScrollButtonEnabledChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as SfTabView)?.UpdateIsScrollButtonEnabled((bool)newValue);

		/// <summary>
		/// Handles changes to the <see cref="ContentTransitionDuration"/> property.
		/// </summary>
		static void OnContentTransitionDurationChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as SfTabView)?.UpdateContentTransitionDuration();

		/// <summary>
		/// Handles changes to the <see cref="IsContentTransitionEnabled"/> property.
		/// </summary>		
		static void OnIsContentTransitionEnabledChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as SfTabView)?.UpdateContentTransitionDuration();

		/// <summary>
		/// Handles changes to the <see cref="IndicatorCornerRadius"/> property.
		/// </summary>
		static void OnIndicatorCornerRadiusChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as SfTabView)?.UpdateCornerRadius((CornerRadius)newValue);

		/// <summary>
		/// Handles changes to the <see cref="IndicatorStrokeThickness"/> property.
		/// </summary>
		static void OnIndicatorStrokeThicknessChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as SfTabView)?.UpdateIndicatorStrokeThickness((double)newValue);

		/// <summary>
		/// Handles changes to the <see cref="FontAutoScalingEnabled"/> property.
		/// </summary>
		static void OnFontAutoScalingEnabledChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as SfTabView)?.UpdateFontAutoScalingEnabled((bool)newValue);

		/// <summary>
		/// Handles changes to the <see cref="EnableVirtualization"/> property.
		/// </summary>
		static void OnEnableVirtualizationChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as SfTabView)?.UpdateEnableVirtualization();

		/// <summary>
		/// Handles changes to the <see cref="IsCenterButtonEnabled"/> property.
		/// </summary>
		static void OnIsCenterButtonEnabledChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as SfTabView)?.UpdateIsCenterButtonEnabled();

		/// <summary>
		/// Handles changes to the <see cref="CenterButtonSettings"/> property.
		/// </summary>
		static void OnCenterButtonSettingsChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfTabView tabView)
			{
				tabView.OnDefaultCenterButtonSettingsChanged();
				tabView.CenterButtonSettings.PropertyChanged -= tabView.OnDefaultCenterButtonSettings_PropertyChanged;
				tabView.CenterButtonSettings.PropertyChanged += tabView.OnDefaultCenterButtonSettings_PropertyChanged;
			}

			if (oldValue != null)
			{
				if (oldValue is CenterButtonSettings previousSetting)
				{
					previousSetting.BindingContext = null;
					previousSetting.Parent = null;
				}
			}

			if (newValue != null)
			{
				if (newValue is CenterButtonSettings currentSetting && bindable is SfTabView sfTabView)
				{
					currentSetting.Parent = sfTabView;
					SetInheritedBindingContext(sfTabView.CenterButtonSettings, sfTabView.BindingContext);
				}
			}
		}

		/// <summary>
		/// Initialize CenterButtonSettings.
		/// </summary>
		/// <returns>The default value of BadgeSettings.</returns>
		private static CenterButtonSettings GetCenterButtonSettingsDefaultValue()
		{
			return new CenterButtonSettings();
		}

		/// <summary>
		/// Handles changes to the <see cref="TabHeaderAlignment"/> property.
		/// </summary>		

		static void OnTabHeaderAlignmentPropertyChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as SfTabView)?.UpdateTabHeaderAlignment((TabHeaderAlignment)newValue);
		
		/// <summary>
		/// Handles changes to the <see cref="AnimationEasing"/> property.
		/// </summary>
		/// <param name="bindable">The bindable object.</param>
		/// <param name="oldValue">The old value of the property.</param>
		/// <param name="newValue">The new value of the property.</param>
		static void OnAnimationEasingChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as SfTabView)?.UpdateAnimationEasing((Easing)newValue);

		/// <summary>
		/// Handles changes to the <see cref="EnableRippleAnimation"/> property.
		/// </summary>
		static void OnEnableRippleAnimationChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as SfTabView)?.UpdateEnableRippleAnimation((bool)newValue);

		#endregion

		#region Private Methods

		/// <summary>
		/// Updates the header item template if both ItemsSource and HeaderItemTemplate are set.
		/// </summary>
		void UpdateHeaderItemTemplate()
		{
			if (ItemsSource == null || HeaderItemTemplate == null)
			{
				return;
			}

			if (_tabHeaderContainer != null)
			{
				_tabHeaderContainer.ItemsSource = ItemsSource;
				_tabHeaderContainer.HeaderItemTemplate = HeaderItemTemplate;
			}
		}

		/// <summary>
		/// Updates the content item template if both ItemsSource and ContentItemTemplate are set.
		/// </summary>
		void UpdateContentItemTemplate()
		{
			if (ItemsSource == null || ContentItemTemplate == null)
			{
				return;
			}

			if (_tabContentContainer != null)
			{
				_tabContentContainer.ItemsSource = ItemsSource;
				_tabContentContainer.ContentItemTemplate = ContentItemTemplate;
			}
		}

		/// <summary>
		/// Updates both header and content item templates based on the current ItemsSource.
		/// </summary>
		void UpdateItemsSource()
		{
			if (ItemsSource == null)
			{
				return;
			}

			UpdateHeaderItemTemplate();
			UpdateContentItemTemplate();
		}

		/// <summary>
		/// Initializes the control, setting up the main grid, header and content containers, and various properties.
		/// </summary>
		void InitializeControl()
		{
			Items = [];
			IsClippedToBounds = true;

			_parentGrid = new SfGrid
			{
				HorizontalOptions = LayoutOptions.Fill,
				VerticalOptions = LayoutOptions.Fill,
				RowSpacing = 0,
				ColumnSpacing = 0,
			};

			InitializeHeaderContainer();
			InitializeTabContentContainer();
			InitializeCenterButton();

			_parentGrid.Children.Add(_tabHeaderContainer);
			_parentGrid.Children.Add(_tabContentContainer);
			_parentGrid.Style = new Style(typeof(SfGrid));
			UpdateRowDefinitions();
			Content = _parentGrid;

			_selectionChangedEventArgs = new TabSelectionChangedEventArgs();

			SetupDynamicResources();
		}

		void SetupDynamicResources()
		{
			SetDynamicResource(ScrollButtonBackgroundProperty, "SfTabViewScrollButtonBackground");
			SetDynamicResource(ScrollButtonColorProperty, "SfTabViewScrollButtonIconColor");
			SetDynamicResource(ScrollButtonDisabledIconColorProperty, "SfTabViewScrollButtonDisabledIconColor");
			this.SetDynamicResource(HoverBackgroundProperty, "SfTabViewHoverBackground");
		}

		/// <summary>
		/// Updates the row definitions of the parent grid and adjusts the tab bar placement.
		/// </summary>
		void UpdateRowDefinitions()
		{
			_parentGrid?.RowDefinitions.Clear();
			_parentGrid?.RowDefinitions.Add(new RowDefinition());
			_parentGrid?.RowDefinitions.Add(new RowDefinition());
			UpdateTabBarPlacement(TabBarPlacement);
		}

		/// <summary>
		/// Initializes and configures the tab header container.
		/// </summary>
		void InitializeHeaderContainer()
		{
			_tabHeaderContainer ??= [];
			_tabHeaderContainer.HeightRequest = TabBarHeight;
			_tabHeaderContainer.Background = TabBarBackground;
			_tabHeaderContainer.Items = Items;

			UpdateIndicatorWidthMode(IndicatorWidthMode);
			UpdateIndicatorBackground(IndicatorBackground);
			UpdateIndicatorPlacement(IndicatorPlacement);

			UpdateHeaderHorizontalTextAlignment(HeaderHorizontalTextAlignment);
			UpdateTabWidthMode(TabWidthMode);

			_tabHeaderContainer.SelectionChanging += HeaderContainer_SelectionChanging;
			_tabHeaderContainer.SelectionChanged += HeaderContainer_SelectionChanged;
			_tabHeaderContainer.TabItemTapped += TabHeaderContainer_TabItemTapped;
			_tabHeaderContainer.ScrollButtonBackground = ScrollButtonBackground;
			_tabHeaderContainer.ScrollButtonColor = ScrollButtonColor;
			_tabHeaderContainer.ScrollButtonDisabledIconColor = ScrollButtonDisabledIconColor;
			_tabHeaderContainer.EnableRippleAnimation = EnableRippleAnimation;
		}

		/// <summary>
		/// Initialize the center button.
		/// </summary>
		void InitializeCenterButton()
		{
			_centerButtonView = new CenterButtonView()
			{
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Start,
			};

			_centerButtonView.CenterButtonTapped += OnCenterButtonTapped;
		}

		/// <summary>
		/// Handles the CenterButtonTapped event of the center button.
		/// </summary>
		void OnCenterButtonTapped(object? sender, EventArgs e)
		{
			RaiseCenterButtonTappedEvent(e);
		}

		/// <summary>
		/// Handles the SelectionChanging event of the header container.
		/// </summary>
		void HeaderContainer_SelectionChanging(object? sender, SelectionChangingEventArgs e)
		{
			RaiseSelectionChangingEvent(e);
		}

		/// <summary>
		/// Handles the SelectionChanging event of the tab content container.
		/// </summary>
		void TabContentContainer_SelectionChanging(object? sender, SelectionChangingEventArgs e)
		{
			RaiseSelectionChangingEvent(e);
		}

		/// <summary>
		/// Handles the TabItemTapped event of the tab header container.
		/// </summary>
		void TabHeaderContainer_TabItemTapped(object? sender, TabItemTappedEventArgs e)
		{
			RaiseTabItemTappedEvent(e);
		}

		/// <summary>
		/// Updates the horizontal text alignment of the tab header.
		/// </summary>
		/// <param name="textAlignment">The new text alignment to set.</param>
		void UpdateHeaderHorizontalTextAlignment(TextAlignment textAlignment)
		{
			if (_tabHeaderContainer != null)
			{
				_tabHeaderContainer.HeaderHorizontalTextAlignment = textAlignment;
			}
		}

		/// <summary>
		/// Initializes the tab content container.
		/// </summary>
		void InitializeTabContentContainer()
		{
			if (_tabHeaderContainer != null)
			{
				_tabContentContainer ??= new SfHorizontalContent(this, _tabHeaderContainer) { HorizontalOptions = LayoutOptions.Fill, VerticalOptions = LayoutOptions.Fill };
				_tabContentContainer.Items = Items;
				_tabContentContainer.SelectionChanging += TabContentContainer_SelectionChanging;
			}
		}

		/// <summary>
		/// Handles the SelectionChanged event of the header container.
		/// </summary>
		void HeaderContainer_SelectionChanged(object? sender, TabSelectionChangedEventArgs e)
		{
			if (SelectedIndex != e.NewIndex)
			{
				SelectedIndex = e.NewIndex;
			}
		}

		/// <summary>
		/// Updates the selected index and raises the SelectionChanged event if necessary.
		/// </summary>
		void UpdateSelectedIndex(int newIndex, int oldIndex)
		{
			if (Items != null || ItemsSource != null)
			{
				if (_tabHeaderContainer != null && _tabHeaderContainer.SelectedIndex != newIndex)
				{
					_tabHeaderContainer.SelectedIndex = newIndex;
				}

				if (_tabContentContainer != null && _tabContentContainer.SelectedIndex != newIndex)
				{
					_tabContentContainer.SelectedIndex = SelectedIndex;
				}

				if (_selectionChangedEventArgs != null)
				{
					_selectionChangedEventArgs.OldIndex = oldIndex;
					_selectionChangedEventArgs.NewIndex = newIndex;
					RaiseSelectionChangedEvent(_selectionChangedEventArgs);
				}
			}
		}

		/// <summary>
		/// Updates the background of the tab indicator.
		/// </summary>
		void UpdateIndicatorBackground(Brush indicatorBackground)
		{
			if (_tabHeaderContainer != null)
			{
				_tabHeaderContainer.IndicatorBackground = indicatorBackground;
			}
		}

		/// <summary>
		/// Updates the background of the tab bar.
		/// </summary>
		void UpdateTabBarBackground(Brush tabBarBackground)
		{
			if (_tabHeaderContainer != null)
			{
				_tabHeaderContainer.Background = tabBarBackground;
			}
		}

		/// <summary>
		/// Updates the placement of the tab indicator.
		/// </summary>
		void UpdateIndicatorPlacement(TabIndicatorPlacement indicatorPlacement)
		{
			if (_tabHeaderContainer != null)
			{
				_tabHeaderContainer.IndicatorPlacement = indicatorPlacement;
			}
		}

		/// <summary>  
		/// Updates the header position of the MAUI TabView. 
		/// </summary>  
		void UpdateTabHeaderAlignment(TabHeaderAlignment tabHeaderAlignment)
		{
			if (_tabHeaderContainer != null)
			{
				_tabHeaderContainer.TabHeaderAlignment = tabHeaderAlignment;
			}
		}

		/// <summary>
		/// Updates the placement of the tab bar within the grid.
		/// </summary>
		void UpdateTabBarPlacement(TabBarPlacement tabBarPlacement)
		{
			if (_tabHeaderContainer != null && _tabContentContainer != null)
			{
				int tabContentContainerIndex = 0;
				int tabHeaderContainerIndex = 1;
				if (tabBarPlacement == TabBarPlacement.Top)
				{
					tabContentContainerIndex = 1;
					tabHeaderContainerIndex = 0;
				}

				Grid.SetRow(_tabContentContainer, tabContentContainerIndex);
				Grid.SetRow(_tabHeaderContainer, tabHeaderContainerIndex);

				if (_parentGrid != null)
				{
					if (TabBarHeight >= 0)
					{
						_parentGrid.RowDefinitions[tabHeaderContainerIndex].Height = TabBarHeight;
						
					}
					_parentGrid.RowDefinitions[tabContentContainerIndex].Height = GridLength.Star;
				}

				UpdateCenterButtonPosition();
			}
		}

		/// <summary>
		/// Updates the position and size of the center button based on the tab bar placement.
		/// </summary>
		void UpdateCenterButtonPosition()
		{
			if (IsCenterButtonEnabled && _centerButtonView is not null && TabWidthMode is TabWidthMode.Default)
			{
				if (TabBarPlacement is TabBarPlacement.Top)
				{
					_centerButtonView.HorizontalOptions = LayoutOptions.Center;
					_centerButtonView.VerticalOptions = LayoutOptions.Start;
				}
				else
				{
					_centerButtonView.HorizontalOptions = LayoutOptions.Center;
					_centerButtonView.VerticalOptions = LayoutOptions.End;
				}

				if (CenterButtonSettings is not null && CenterButtonSettings.Height >= 0)
				{
					_centerButtonView.HeightRequest = CenterButtonSettings.Height;
				}
			}
		}

		/// <summary>
		/// Updates the width mode of the tabs.
		/// </summary>
		void UpdateTabWidthMode(TabWidthMode tabWidthMode)
		{
			if (_tabHeaderContainer != null)
			{
				_tabHeaderContainer.TabWidthMode = tabWidthMode;

				if (IsCenterButtonEnabled && _parentGrid is not null)
				{
					if (tabWidthMode is TabWidthMode.Default)
					{
						UpdateCenterButtonPosition();
						_parentGrid.Children.Add(_centerButtonView);
						Grid.SetRowSpan(_centerButtonView, 2);
					}
					else
					{
						if (_parentGrid.Children.Contains(_centerButtonView))
						{
							_parentGrid.Children.Remove(_centerButtonView);
						}
					}
				}
			}
		}

		/// <summary>
		/// Update the indicator width when the indicator width mode property is changed.
		/// </summary>
		/// <param name="indicatorWidthMode">The <see cref="IndicatorWidthMode"/> to apply to the tab indicator.</param>
		void UpdateIndicatorWidthMode(IndicatorWidthMode indicatorWidthMode)
		{
			if (_tabHeaderContainer != null)
			{
				_tabHeaderContainer.IndicatorWidthMode = indicatorWidthMode;
			}
		}

		/// <summary>
		/// Updates the display mode of the tab header based on the specified <see cref="TabBarDisplayMode"/>.
		/// </summary>
		/// <param name="headerDisplayMode">The <see cref="TabBarDisplayMode"/> to apply to the tab header.</param>
		void UpdateHeaderDisplayMode(TabBarDisplayMode headerDisplayMode)
		{
			if (_tabHeaderContainer != null)
			{
				_tabHeaderContainer.HeaderDisplayMode = headerDisplayMode;
			}
		}

		/// <summary>
		/// Updates the padding of the tab headers.
		/// </summary>
		void UpdateTabHeaderPadding(Thickness tabHeaderPadding)
		{
			if (_tabHeaderContainer != null)
			{
				_tabHeaderContainer.TabHeaderPadding = tabHeaderPadding;
			}
		}

		/// <summary>
		/// Updates the items collection for both the header and content containers.
		/// </summary>
		void UpdateItems()
		{
			if (_tabHeaderContainer != null)
			{
				_tabHeaderContainer.Items = Items;
			}

			if (_tabContentContainer != null)
			{
				_tabContentContainer.Items = Items;
			}

			if (SelectedIndex == -1 && Items?.Count > 0)
			{
				SelectedIndex = 0;
			}
		}

		/// <summary>
		/// Updates the height of the tab bar.
		/// </summary>
		void UpdateTabBarHeight(double tabBarHeight)
		{
			if (_tabHeaderContainer != null)
			{
				if (TabBarHeight > 0)
				{
					_tabHeaderContainer.HeightRequest = tabBarHeight;
					_tabHeaderContainer.UpdateHeaderItemHeight();
					UpdateTabBarPlacement(TabBarPlacement);
					_tabHeaderContainer.IsVisible = true;
				}
				else
				{
					_tabHeaderContainer.IsVisible = false;
					UpdateTabBarPlacement(TabBarPlacement);
				}
			}
		}

		/// <summary>
		/// Updates whether scroll buttons are enabled in the tab header.
		/// </summary>
		void UpdateIsScrollButtonEnabled(bool newValue)
		{
			if (_tabHeaderContainer != null)
			{
				_tabHeaderContainer.IsScrollButtonEnabled = newValue;
			}
		}

		/// <summary>
		/// Updates the duration of content transition animations.
		/// </summary>
		void UpdateContentTransitionDuration()
		{
			double transitionDuration = this.IsContentTransitionEnabled ? (this.ContentTransitionDuration > 0 ? this.ContentTransitionDuration : (double)ContentTransitionDurationProperty.DefaultValue) : 0;

			if (_tabContentContainer != null)
			{
				_tabContentContainer.ContentTransitionDuration = transitionDuration;
			}

			if (_tabHeaderContainer != null)
			{
				_tabHeaderContainer.ContentTransitionDuration = transitionDuration;
			}
		}

		/// <summary>
		/// Updates the corner radius of the tab indicator.
		/// </summary>
		void UpdateCornerRadius(CornerRadius newValue)
		{
			if (_tabHeaderContainer != null)
			{
				_tabHeaderContainer.IndicatorCornerRadius = newValue;
			}
		}

		void UpdateIndicatorStrokeThickness(double newValue)
		{
			if (this._tabHeaderContainer != null)
			{
				this._tabHeaderContainer.IndicatorStrokeThickness = newValue;
			}
		}

		/// <summary>
		/// Updates whether font auto-scaling is enabled.
		/// </summary>
		void UpdateFontAutoScalingEnabled(bool newValue)
		{
			if (_tabHeaderContainer != null)
			{
				_tabHeaderContainer.FontAutoScalingEnabled = newValue;
			}
		}

		/// <summary>
		/// Updates the enable virtualization.
		/// </summary>
		void UpdateEnableVirtualization()
		{
			if (_tabContentContainer != null)
				_tabContentContainer.EnableVirtualization = EnableVirtualization;
		}

		/// <summary>
		/// Updates the state of the center button in the tab header based on the current value of the <see cref="IsCenterButtonEnabled"/> property.
		/// </summary>
		void UpdateIsCenterButtonEnabled()
		{
			if (_parentGrid is null)
			{
				return;
			}

			if (_tabHeaderContainer != null)
			{
				_tabHeaderContainer.IsCenterButtonEnabled = IsCenterButtonEnabled;
			}

			if (IsCenterButtonEnabled && TabWidthMode is TabWidthMode.Default)
			{
				Grid.SetRowSpan(_centerButtonView, 2);
				UpdateCenterButtonPosition();
				_parentGrid.Children.Add(_centerButtonView);
				if (_centerButtonView is not null && CenterButtonSettings is not null && _centerButtonView.WidthRequest != CenterButtonSettings.Width)
				{
					OnDefaultCenterButtonSettingsChanged();
				}
			}
			else
			{
				if (_parentGrid.Children.Contains(_centerButtonView))
				{
					_parentGrid.Children.Remove(_centerButtonView);
				}
			}
		}

		/// <summary>
		/// Updates the center button's properties in the tab header container based on the current CenterButtonSettings.
		/// </summary>
		void OnDefaultCenterButtonSettingsChanged()
		{
			if (_tabHeaderContainer is null || CenterButtonSettings is null || _centerButtonView is null)
			{
				return;
			}

			_tabHeaderContainer.UpdateCenterButtonWidth(CenterButtonSettings.Width);
			_tabHeaderContainer.UpdateCenterButtonHeight(CenterButtonSettings.Height);

			_centerButtonView.UpdateCenterButtonTitle(CenterButtonSettings.Title);
			_centerButtonView.UpdateCenterButtonWidth(CenterButtonSettings.Width);
			_centerButtonView.UpdateCenterButtonHeight(CenterButtonSettings.Height);
			_centerButtonView.UpdateCenterButtonBackground(CenterButtonSettings.Background);
			_centerButtonView.UpdateCenterButtonStroke(CenterButtonSettings.Stroke);
			_centerButtonView.UpdateCenterButtonStrokeThickness(CenterButtonSettings.StrokeThickness);
			_centerButtonView.UpdateCenterButtonCornerRadius(CenterButtonSettings.CornerRadius);
			_centerButtonView.UpdateCenterButtonFontFamily(CenterButtonSettings.FontFamily);
			_centerButtonView.UpdateCenterButtonFontAttributes(CenterButtonSettings.FontAttributes);
			_centerButtonView.UpdateCenterButtonFontSize(CenterButtonSettings.FontSize);
			_centerButtonView.UpdateCenterButtonTextColor(CenterButtonSettings.TextColor);
			_centerButtonView.UpdateCenterButtonImageSource(CenterButtonSettings.ImageSource);
			_centerButtonView.UpdateCenterButtonImageSize(CenterButtonSettings.ImageSize);
			_centerButtonView.UpdateCenterButtonDisplayMode(CenterButtonSettings.DisplayMode);
			_centerButtonView.UpdateCenterButtonFontAutoScalingEnabled(CenterButtonSettings.FontAutoScalingEnabled);
		}

		/// <summary>
		/// Updates specific properties of the center button in the tab header container when the property of center button seetings changed.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The PropertyChangedEventArgs containing the name of the property that changed.</param>
		void OnDefaultCenterButtonSettings_PropertyChanged(object? sender, PropertyChangedEventArgs e)
		{
			if (_tabHeaderContainer is null || CenterButtonSettings is null || _centerButtonView is null)
			{
				return;
			}

			switch (e.PropertyName)
			{
				case "Title":
					_centerButtonView.UpdateCenterButtonTitle(CenterButtonSettings.Title);
					break;
				case "Width":
					_tabHeaderContainer.UpdateCenterButtonWidth(CenterButtonSettings.Width);
					_centerButtonView.UpdateCenterButtonWidth(CenterButtonSettings.Width);
					break;
				case "Height":
					_tabHeaderContainer.UpdateCenterButtonHeight(CenterButtonSettings.Height);
					_centerButtonView.UpdateCenterButtonHeight(CenterButtonSettings.Height);
					break;
				case "Background":
					_centerButtonView.UpdateCenterButtonBackground(CenterButtonSettings.Background);
					break;
				case "Stroke":
					_centerButtonView.UpdateCenterButtonStroke(CenterButtonSettings.Stroke);
					break;
				case "StrokeThickness":
					_centerButtonView.UpdateCenterButtonStrokeThickness(CenterButtonSettings.StrokeThickness);
					break;
				case "CornerRadius":
					_centerButtonView.UpdateCenterButtonCornerRadius(CenterButtonSettings.CornerRadius);
					break;
				case "FontFamily":
					_centerButtonView.UpdateCenterButtonFontFamily(CenterButtonSettings.FontFamily);
					break;
				case "FontAttributes":
					_centerButtonView.UpdateCenterButtonFontAttributes(CenterButtonSettings.FontAttributes);
					break;
				case "FontSize":
					_centerButtonView.UpdateCenterButtonFontSize(CenterButtonSettings.FontSize);
					break;
				case "FontAutoScalingEnabled":
					_centerButtonView.UpdateCenterButtonFontAutoScalingEnabled(CenterButtonSettings.FontAutoScalingEnabled);
					break;
				case "TextColor":
					_centerButtonView.UpdateCenterButtonTextColor(CenterButtonSettings.TextColor);
					break;
				case "ImageSource":
					_centerButtonView.UpdateCenterButtonImageSource(CenterButtonSettings.ImageSource);
					break;
				case "ImageSize":
					_centerButtonView.UpdateCenterButtonImageSize(CenterButtonSettings.ImageSize);
					break;
				case "DisplayMode":
					_centerButtonView.UpdateCenterButtonDisplayMode(CenterButtonSettings.DisplayMode);
					break;
			}
		}
		
		/// <summary>
		/// Updates the animation easing for both tab header and content containers.
		/// </summary>
		/// <param name="animationEasing">The new animation easing to apply.</param>
		void UpdateAnimationEasing(Easing animationEasing)
		{
			if (_tabHeaderContainer != null)
			{
				_tabHeaderContainer.AnimationEasing = animationEasing;
			}

			if (_tabContentContainer != null)
			{
				_tabContentContainer.AnimationEasing = animationEasing;
			}
		}

		/// <summary>
		/// Gets the theme dictionary for the tab view.
		/// </summary>
		/// <returns>A new instance of SfTabViewStyles.</returns>
		ResourceDictionary IParentThemeElement.GetThemeDictionary()
		{
			return new SfTabViewStyles();
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

		/// <summary>
		/// This method is called when the <see cref="SfTabBar.ScrollButtonBackground"/> property value changes.It updates the <see cref="SfTabBar.ScrollButtonBackground"/> accordingly.
		/// </summary>
		/// <param name="bindable">The bindable object.</param>
		/// <param name="oldValue">The old value of the property.</param>
		/// <param name="newValue">The new value of the property.</param>
		static void OnScrollButtonBackgroundPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfTabView tabView && tabView != null && tabView._tabHeaderContainer != null && newValue is Brush newBrush)
			{
				tabView._tabHeaderContainer.ScrollButtonBackground = newBrush;
			}
		}

		/// <summary>
		/// This method is called when the <see cref="SfTabBar.ScrollButtonColor"/> property value changes.It updates the ScrollButtonColor of the tab header container accordingly.
		/// </summary>
		/// <param name="bindable">The bindable object.</param>
		/// <param name="oldValue">The old value of the property.</param>
		/// <param name="newValue">The new value of the property.</param>
		static void OnScrollButtonColorChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfTabView tabView && tabView != null && tabView._tabHeaderContainer != null)
			{
				tabView._tabHeaderContainer.ScrollButtonColor = (Color)newValue;
			}
		}

		/// <summary>
		/// This method is called when the <see cref="SfTabBar.ScrollButtonDisabledIconColor"/> property value changes.
		/// It updates the <see cref="SfTabBar.ScrollButtonDisabledIconColor"/> accordingly.
		/// </summary>
		/// <param name="bindable">The bindable object.</param>
		/// <param name="oldValue">The old value of the property.</param>
		/// <param name="newValue">The new value of the property.</param>
		static void OnScrollButtonDisabledIconColorChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfTabView tabView && tabView != null && tabView._tabHeaderContainer != null)
			{
				tabView._tabHeaderContainer.ScrollButtonDisabledIconColor = (Color)newValue;
			}
		}

		/// <summary>
		/// This method is called when the <see cref="SfTabBar.HoverBackground"/> property value changes.
		/// It updates the <see cref="SfTabBar.HoverBackground"/> accordingly.
		/// </summary>
		/// <param name="bindable">The bindable object.</param>
		/// <param name="oldValue">The old value of the property.</param>
		/// <param name="newValue">The new value of the property.</param>
		static void OnHoverBackgroundChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfTabView tabView && tabView != null && tabView._tabHeaderContainer != null)
			{
				tabView._tabHeaderContainer.HoverBackground = (Brush)newValue;
			}
		}

		/// <summary>
		/// Updates whether ripple animation is enabled in the tab view.
		/// </summary>
		/// <param name="enableRippleAnimation">The new value for EnableRippleAnimation.</param>
		void UpdateEnableRippleAnimation(bool enableRippleAnimation)
		{
			if (_tabHeaderContainer != null)
			{
				_tabHeaderContainer.EnableRippleAnimation = enableRippleAnimation;
			}
		}
		#endregion
	}
}
