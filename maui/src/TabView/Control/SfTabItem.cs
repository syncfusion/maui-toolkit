using Syncfusion.Maui.Toolkit.Graphics.Internals;
using Syncfusion.Maui.Toolkit.Internals;
using TouchEventArgs = Syncfusion.Maui.Toolkit.Internals.PointerEventArgs;
using ITextElement = Syncfusion.Maui.Toolkit.Graphics.Internals.ITextElement;

namespace Syncfusion.Maui.Toolkit.TabView
{

	/// <summary>
	/// Represents a class which defines the visual and interactive behavior of individual tab item inside a <see cref="SfTabView"/> control.
	/// </summary>
	/// <example>
	/// The following example shows how to initialize tab items and use in tab view.
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
	[ContentProperty(nameof(Content))]
	public partial class SfTabItem : ContentView, ITouchListener, ITextElement
	{
		#region Fields

		bool _isSelected;
		bool _isDescriptionNotSetByUser;
		bool _isPreviousItemSelected;

		#endregion

		#region Bindable Properties

		/// <summary>
		/// Identifies the <see cref="ImageTextSpacing"/> bindable property.
		/// </summary>
		public static readonly BindableProperty ImageTextSpacingProperty =
			BindableProperty.Create(
				nameof(ImageTextSpacing),
				typeof(double),
				typeof(SfTabItem),
				10d,
				BindingMode.TwoWay);

		/// <summary>
		/// Identifies the <see cref="Header"/> bindable property.
		/// </summary>
		public static readonly BindableProperty HeaderProperty =
			BindableProperty.Create(
				nameof(Header),
				typeof(string),
				typeof(SfTabItem),
				string.Empty,
				BindingMode.TwoWay);

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
		/// Identifies the <see cref="TextColor"/> bindable property.
		/// </summary>
		public static readonly BindableProperty TextColorProperty =
			BindableProperty.Create(
				nameof(TextColor),
				typeof(Color),
				typeof(SfTabItem),
				Color.FromArgb("#49454F"));

		/// <summary>
		/// Identifies the <see cref="ImageSource"/> bindable property.
		/// </summary>
		public static readonly BindableProperty ImageSourceProperty =
			BindableProperty.Create(
				nameof(ImageSource),
				typeof(ImageSource),
				typeof(SfTabItem),
				null);

		/// <summary>
		/// Identifies the <see cref="ImagePosition"/> bindable property.
		/// </summary>
		public static readonly BindableProperty ImagePositionProperty =
		   BindableProperty.Create(
			   nameof(ImagePosition),
			   typeof(TabImagePosition),
			   typeof(SfTabItem),
			   TabImagePosition.Top);

		/// <summary>
		/// Identifies the <see cref="FontAutoScalingEnabled"/> bindable property.
		/// </summary>
		public static readonly BindableProperty FontAutoScalingEnabledProperty =
			FontElement.FontAutoScalingEnabledProperty;

		/// <summary>
		/// Identifies the <see cref="ImageSize"/> bindable property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="ImageSize"/> bindable property.
		/// </value>
		public static readonly BindableProperty ImageSizeProperty =
		   BindableProperty.Create(
			   nameof(ImageSize),
			   typeof(double),
			   typeof(SfTabItem),
			   24d,
			   BindingMode.TwoWay);

		/// <summary>
		/// Identifies the <see cref="HeaderDisplayMode"/> bindable property.
		/// </summary>
		internal static readonly BindableProperty HeaderDisplayModeProperty =
		   BindableProperty.Create(
			   nameof(HeaderDisplayMode),
			   typeof(TabBarDisplayMode),
			   typeof(SfTabItem),
			   TabBarDisplayMode.Default);

		/// <summary>
		/// Identifies the <see cref="TabWidthMode"/> bindable property.
		/// </summary>
		internal static readonly BindableProperty TabWidthModeProperty =
		   BindableProperty.Create(
			   nameof(TabWidthMode),
			   typeof(TabWidthMode),
			   typeof(SfTabItem),
			   TabWidthMode.Default);

		/// <summary>
		/// Identifies the <see cref="IndicatorPlacement"/> bindable property.
		/// </summary>
		internal static readonly BindableProperty IndicatorPlacementProperty =
		   BindableProperty.Create(
			   nameof(IndicatorPlacement),
			   typeof(TabIndicatorPlacement),
			   typeof(SfTabItem),
			   TabIndicatorPlacement.Bottom);

		/// <summary>
		/// Identifies the <see cref="HeaderHorizontalTextAlignment "/> bindable property.
		/// </summary>
		internal static readonly BindableProperty HeaderHorizontalTextAlignmentProperty =
		   BindableProperty.Create(
			   nameof(HeaderHorizontalTextAlignment),
			   typeof(TextAlignment),
			   typeof(SfTabItem),
			   TextAlignment.Center);

		/// <summary>
		/// Identifies the <see cref="HeaderContent"/> bindable property.
		/// </summary>
		public static readonly BindableProperty HeaderContentProperty =
			BindableProperty.Create(
				nameof(HeaderContent),
				typeof(View),
				typeof(SfTabItem),
				null);

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="SfTabItem"/> class.
		/// </summary>
		public SfTabItem()
		{
			BackgroundColor = Colors.Transparent;
			this.AddTouchListener(this);
			ChangeSelectedState();
		}

		#endregion

		#region Events

		/// <summary>
		/// Occurs when the tab item is touched.
		/// </summary>
		internal event EventHandler<TouchEventArgs>? Touched;

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the text for the tab header.
		/// </summary>
		/// <value>
		/// Specifies the header text of the tab item. The default value is an empty string.
		/// </value>
		/// <example>
		/// Here is an example of how to set the <see cref="Header"/> property.
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <tabView:SfTabView>
		///     <tabView:SfTabItem Header="TAB 1">
		///         <tabView:SfTabItem.Content>
		///             <Label Text="Content" />
		///         </tabView:SfTabItem.Content>
		///     </tabView:SfTabItem>
		/// </tabView:SfTabView>
		/// ]]></code>
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
		public string Header
		{
			get => (string)GetValue(HeaderProperty);
			set => SetValue(HeaderProperty, value);
		}

		/// <summary>
		/// Gets or sets the value that defines the font family of the header.
		/// </summary>
		/// <value>
		/// Specifies the font family of the tab item's header text. The default value is null.
		/// </value>
		/// <example>
		/// Here is an example of how to set the <see cref="FontFamily"/> property.
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <tabView:SfTabView>
		///     <tabView:SfTabItem Header="TAB 1" FontFamily="OpenSansRegular">
		///         <tabView:SfTabItem.Content>
		///             <Label Text="Content" />
		///         </tabView:SfTabItem.Content>
		///     </tabView:SfTabItem>
		/// </tabView:SfTabView>
		/// ]]></code>
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var tabView = new SfTabView();
		/// var tabItems = new TabItemCollection
		/// {
		///     new SfTabItem
		///     {
		///         Header = "TAB 1",
		///         FontFamily = "OpenSansRegular",
		///         Content = new Label { Text = "Content" }
		///     }
		/// };
		///
		/// tabView.Items = tabItems;
		/// Content = tabView;
		/// ]]></code>
		/// </example>
		public string FontFamily
		{
			get => (string)GetValue(FontFamilyProperty);
			set => SetValue(FontFamilyProperty, value);
		}

		/// <summary>
		/// Gets or sets the value that defines the font attributes of the tab header.
		/// </summary>
		/// <value>
		/// One of the <see cref="Microsoft.Maui.Controls.FontAttributes"/> enumeration that specifies the font attributes of the tab item's header text. The default value is is <see cref="Microsoft.Maui.Controls.FontAttributes.None"/>.
		/// </value>
		/// <example>
		/// Here is an example of how to set the <see cref="FontAttributes"/> property.
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <tabView:SfTabView>
		///     <tabView:SfTabItem Header="TAB 1" FontAttributes="Bold">
		///         <tabView:SfTabItem.Content>
		///             <Label Text="Content" />
		///         </tabView:SfTabItem.Content>
		///     </tabView:SfTabItem>
		/// </tabView:SfTabView>
		/// ]]></code>
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var tabView = new SfTabView();
		/// var tabItems = new TabItemCollection
		/// {
		///     new SfTabItem
		///     {
		///         Header = "TAB 1",
		///         FontAttributes = FontAttributes.Bold,
		///         Content = new Label { Text = "Content" }
		///     }
		/// };
		///
		/// tabView.Items = tabItems;
		/// Content = tabView;
		/// ]]></code>
		/// </example>
		public FontAttributes FontAttributes
		{
			get => (FontAttributes)GetValue(FontAttributesProperty);
			set => SetValue(FontAttributesProperty, value);
		}

		/// <summary> 
		/// Gets or sets a value that determines whether the font of the control should scale automatically according to the operating system settings.
		/// </summary>
		/// <value>
		/// A boolean value indicating whether the font should scale automatically. The default value is false.
		/// </value>
		/// <example>
		/// Here is an example of how to set the <see cref="FontAutoScalingEnabled"/> property.
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <tabView:SfTabView>
		///     <tabView:SfTabItem Header="TAB 1" FontAutoScalingEnabled="True">
		///         <tabView:SfTabItem.Content>
		///             <Label Text="Content" />
		///         </tabView:SfTabItem.Content>
		///     </tabView:SfTabItem>
		/// </tabView:SfTabView>
		/// ]]></code>
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var tabView = new SfTabView();
		/// var tabItems = new TabItemCollection
		/// {
		///     new SfTabItem
		///     {
		///         Header = "TAB 1",
		///         FontAutoScalingEnabled = true,
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
		/// Gets or sets the font size of the tab header.
		/// </summary>
		/// <value>
		/// Specifies the font size of the tab item's header text. The default value is 14d.
		/// </value>
		/// <example>
		/// Here is an example of how to set the <see cref="FontSize"/> property.
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <tabView:SfTabView>
		///     <tabView:SfTabItem Header="TAB 1" FontSize="32">
		///         <tabView:SfTabItem.Content>
		///             <Label Text="Content" />
		///         </tabView:SfTabItem.Content>
		///     </tabView:SfTabItem>
		/// </tabView:SfTabView>
		/// ]]></code>
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var tabView = new SfTabView();
		/// var tabItems = new TabItemCollection
		/// {
		///     new SfTabItem
		///     {
		///         Header = "TAB 1",
		///         FontSize = 32,
		///         Content = new Label { Text = "Content" }
		///     }
		/// };
		///
		/// tabView.Items = tabItems;
		/// Content = tabView;
		/// ]]></code>
		/// </example>
		[System.ComponentModel.TypeConverter(typeof(FontSizeConverter))]
		public double FontSize
		{
			get => (double)GetValue(FontSizeProperty);
			set => SetValue(FontSizeProperty, value);
		}

		/// <summary>
		/// Gets or sets the text color of the tab header.
		/// </summary>
		/// <value>
		/// Specifies the color of the tab item's header text. The default value is black.
		/// </value>
		/// <example>
		/// Here is an example of how to set the <see cref="TextColor"/> property.
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <tabView:SfTabView>
		///     <tabView:SfTabItem Header="TAB 1" TextColor="Red">
		///         <tabView:SfTabItem.Content>
		///             <Label Text="Content" />
		///         </tabView:SfTabItem.Content>
		///     </tabView:SfTabItem>
		/// </tabView:SfTabView>
		/// ]]></code>
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var tabView = new SfTabView();
		/// var tabItems = new TabItemCollection
		/// {
		///     new SfTabItem
		///     {
		///         Header = "TAB 1",
		///         TextColor = Colors.Red,
		///         Content = new Label { Text = "Content" }
		///     }
		/// };
		///
		/// tabView.Items = tabItems;
		/// Content = tabView;
		/// ]]></code>
		/// </example>
		public Color TextColor
		{
			get => (Color)GetValue(TextColorProperty);
			set => SetValue(TextColorProperty, value);
		}

		/// <summary>
		/// Gets or sets the image source for the tab header.
		/// </summary>
		/// <value>
		/// Specifies the image of the tab item. The default value is null.
		/// </value>
		/// <example>
		/// Here is an example of how to set the <see cref="ImageSource"/> property.
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <tabView:SfTabView>
		///     <tabView:SfTabItem Header="TAB 1" ImageSource="image_name">
		///         <tabView:SfTabItem.Content>
		///             <Label Text="Content" />
		///         </tabView:SfTabItem.Content>
		///     </tabView:SfTabItem>
		/// </tabView:SfTabView>
		/// ]]></code>
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var tabView = new SfTabView();
		/// var tabItems = new TabItemCollection
		/// {
		///     new SfTabItem
		///     {
		///         Header = "TAB 1",
		///         ImageSource = "image_name",
		///         Content = new Label { Text = "Content" }
		///     }
		/// };
		///
		/// tabView.Items = tabItems;
		/// Content = tabView;
		/// ]]></code>
		/// </example>
		public ImageSource ImageSource
		{
			get => (ImageSource)GetValue(ImageSourceProperty);
			set => SetValue(ImageSourceProperty, value);
		}

		/// <summary>
		/// Gets or sets the position of the image relative to the text in a tab item.
		/// </summary>
		/// <value>
		/// One of the <see cref="Syncfusion.Maui.Toolkit.TabView.TabImagePosition"/> enumeration that specifies the image position relative to the text. The default mode is <see cref="Syncfusion.Maui.Toolkit.TabView.TabImagePosition.Top"/>.
		/// </value>
		/// <example>
		/// Here is an example of how to set the <see cref="ImagePosition"/> property.
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <tabView:SfTabView>
		///     <tabView:SfTabItem Header="TAB 1" ImageSource="image_name" ImagePosition="Right">
		///         <tabView:SfTabItem.Content>
		///             <Label Text="Content" />
		///         </tabView:SfTabItem.Content>
		///     </tabView:SfTabItem>
		/// </tabView:SfTabView>
		/// ]]></code>
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var tabView = new SfTabView();
		/// var tabItems = new TabItemCollection
		/// {
		///     new SfTabItem
		///     {
		///         Header = "TAB 1",
		///         ImageSource = "image_name",
		///         ImagePosition = TabImagePosition.Right,
		///         Content = new Label { Text = "Content" }
		///     }
		/// };
		///
		/// tabView.Items = tabItems;
		/// Content = tabView;
		/// ]]></code>
		/// </example>
		public TabImagePosition ImagePosition
		{
			get => (TabImagePosition)GetValue(ImagePositionProperty);
			set => SetValue(ImagePositionProperty, value);
		}

		/// <summary>
		/// Gets or sets the spacing between the header text and image.
		/// </summary>
		/// <value>
		/// Specifies the spacing between the header and image. The default spacing value is 10d.
		/// </value>
		/// <example>
		/// Here is an example of how to set the <see cref="ImageTextSpacing"/> property.
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <tabView:SfTabView>
		///   <tabView:SfTabItem Header = "TAB 1" ImageTextSpacing="2"  ImageSource="avatar1.png"></tabView:SfTabItem>
		///   <tabView:SfTabItem Header = "TAB 2" ImageTextSpacing="20" ImageSource="avatar1.png"></tabView:SfTabItem>
		/// </tabView:SfTabView>
		/// ]]></code>
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var tabView = new SfTabView();
		/// var tabItems = new TabItemCollection
		/// {
		///     new SfTabItem
		///     {
		///         Header = "TAB 1",
		///         ImageTextSpacing="2"
		///         ImageSource="avatar1.png"
		///     }
		///     new SfTabItem
		///     {
		///         Header="TAB 2,
		///         ImageTextSpacing="20",
		///         ImageSource="avatar1.png"
		///     }
		/// };
		///
		/// tabView.Items = tabItems;
		/// Content = tabView;
		/// ]]></code>
		/// </example>
		public double ImageTextSpacing
		{
			get => (double)GetValue(ImageTextSpacingProperty);
			set => SetValue(ImageTextSpacingProperty, value);
		}

		/// <summary>
		/// Gets a value indicating whether the tab item is selected.
		/// </summary>
		/// <value>
		/// A boolean value indicating whether the tab item is selected.
		/// </value> 
		/// <example>
		/// Here is an example of how to set the <see cref="IsSelected"/> property.
		/// 
		/// # [C#](#tab/tabid-1)
		/// <code><![CDATA[
		/// SfTabItem tabItem = new SfTabItem();
		/// bool isSelected = tabItem.IsSelected;
		/// ]]></code>
		/// </example>
		public bool IsSelected
		{
			get
			{
				return _isSelected;
			}

			internal set
			{
				_isSelected = value;
				if (IndicatorPlacement != TabIndicatorPlacement.Fill ||
					(!value && IndicatorPlacement == TabIndicatorPlacement.Fill))
				{
					ChangeSelectedState();
				}
			}
		}

		/// <summary>
		///  Gets or sets a value that can be used to customize the image size in tab header.
		/// </summary>
		public double ImageSize
		{
			get => (double)this.GetValue(ImageSizeProperty);
			set => this.SetValue(ImageSizeProperty, value);
		}

		/// <summary>
		/// Gets or sets a value indicating whether the SemanticProperties.Description is set by the user.
		/// </summary>
		internal bool IsDescriptionNotSetByUser
		{
			get { return _isDescriptionNotSetByUser; }
			set { _isDescriptionNotSetByUser = value; }
		}

		/// <summary>
		/// Gets or sets the header display mode for the tab item.
		/// </summary>
		internal TabBarDisplayMode HeaderDisplayMode
		{
			get => (TabBarDisplayMode)GetValue(HeaderDisplayModeProperty);
			set => SetValue(HeaderDisplayModeProperty, value);
		}

		/// <summary>
		/// Gets or sets the width mode for the tab item.
		/// </summary>
		internal TabWidthMode TabWidthMode
		{
			get => (TabWidthMode)GetValue(TabWidthModeProperty);
			set => SetValue(TabWidthModeProperty, value);
		}

		/// <summary>
		/// Gets or sets the placement of the tab indicator.
		/// </summary>
		internal TabIndicatorPlacement IndicatorPlacement
		{
			get => (TabIndicatorPlacement)GetValue(IndicatorPlacementProperty);
			set => SetValue(IndicatorPlacementProperty, value);
		}

		/// <summary>
		/// Gets or sets the horizontal text alignment for the tab header.
		/// </summary>
		internal TextAlignment HeaderHorizontalTextAlignment
		{
			get => (TextAlignment)GetValue(HeaderHorizontalTextAlignmentProperty);
			set => SetValue(HeaderHorizontalTextAlignmentProperty, value);
		}

		/// <summary>
		/// Gets or sets the custom view content for the tab header.
		/// </summary>
		/// <value>
		/// Specifies the custom view content for the tab header. When set, this takes precedence over the Header property. The default value is null.
		/// </value>
		/// <example>
		/// Here is an example of how to set custom header content for a tab item.
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <tabView:SfTabView>
		///     <tabView:SfTabItem>
		///         <tabView:SfTabItem.HeaderContent>
		///             <Grid BackgroundColor="LightBlue">
		///                 <Label Text="TAB 1" FontSize="16" />
		///             </Grid>
		///         </tabView:SfTabItem.HeaderContent>
		///         <tabView:SfTabItem.Content>
		///             <Label Text="Content" />
		///         </tabView:SfTabItem.Content>
		///     </tabView:SfTabItem>
		/// </tabView:SfTabView>
		/// ]]></code>
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var tabView = new SfTabView();
		/// var customHeader = new Grid { BackgroundColor = Colors.LightBlue };
		/// customHeader.Children.Add(new Label { Text = "TAB 1", FontSize = 16 });
		/// 
		/// var tabItems = new TabItemCollection
		/// {
		///     new SfTabItem
		///     {
		///         HeaderContent = customHeader,
		///         Content = new Label { Text = "Content" }
		///     }
		/// };
		///
		/// tabView.Items = tabItems;
		/// Content = tabView;
		/// ]]></code>
		/// </example>
		public View HeaderContent
		{
			get => (View)GetValue(HeaderContentProperty);
			set => SetValue(HeaderContentProperty, value);
		}

		/// <summary>
		/// Gets the font of the tab item.
		/// </summary>
		/// <example>
		/// Here is an example of how to retrieve the Font property from tab item.
		/// 
		/// # [C#](#tab/tabid-1)
		/// <code><![CDATA[
		/// var tabItem = new SfTabItem
		/// {
		///		Header = "Tab 1",
		///		Content = new Label { Text = "Content for Tab 1" }
		///	};
		///	// Example of retrieving the Font property from tab item.
		///	var font = tabItem.Font;
		/// ]]></code>
		/// </example>
		public Microsoft.Maui.Font Font { get; }

#if WINDOWS || IOS
		/// <summary>
		/// Gets or sets a value indicating whether a mouse is currently moved over the tab item.
		/// </summary>
		internal bool IsMouseMoved { get; set; }
#endif

		#endregion

		#region Public Methods

		/// <summary>
		/// Handles touch events for the tab item.
		/// </summary>
		/// <param name="e">The touch event arguments containing details about the touch event.</param>
		/// <exclude/>
		public void OnTouch(TouchEventArgs e)
		{
			Touched?.Invoke(this, e);
			switch (e.Action)
			{
				case PointerActions.Entered:
					{
						OnPointerEntered();
						break;
					}
				case PointerActions.Pressed:
					{
						OnPointerPressed();
						break;
					}
				case PointerActions.Released:
					{
						OnPointerReleased();
						break;
					}
				case PointerActions.Exited:
					{
						OnPointerExited();
						break;
					}
			}
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Called when the pointer enters the tab item area.
		/// </summary>
		void OnPointerEntered()
		{
			_isPreviousItemSelected = IsSelected;
			if (!IsSelected)
			{
				var stateName = IndicatorPlacement == TabIndicatorPlacement.Fill ? "InActiveHoverFilled" : "InActiveHover";
				VisualStateManager.GoToState(this, stateName);
			}
			else
			{
				var stateName = IndicatorPlacement == TabIndicatorPlacement.Fill ? "HoverFilled" : "Hover";
				VisualStateManager.GoToState(this, stateName);
			}
		}

		/// <summary>
		/// Called when the pointer is pressed on the tab item.
		/// </summary>
		void OnPointerPressed()
		{
			_isPreviousItemSelected = IsSelected;
			if (!IsSelected)
			{
				var stateName = IndicatorPlacement == TabIndicatorPlacement.Fill ? "InActivePressedFilled" : "InActivePressed";
				VisualStateManager.GoToState(this, stateName);
			}
			else
			{
				var stateName = IndicatorPlacement == TabIndicatorPlacement.Fill ? "PressedFilled" : "Pressed";
				VisualStateManager.GoToState(this, stateName);
			}
		}

		/// <summary>
		/// Called when the pointer is released from the tab item.
		/// </summary>
		void OnPointerReleased()
		{
			if (IndicatorPlacement != TabIndicatorPlacement.Fill ||
				(!_isSelected && IndicatorPlacement == TabIndicatorPlacement.Fill))
			{
				ChangeSelectedState();
			}
			else if (_isPreviousItemSelected)
			{
				ChangeSelectedState();
				_isPreviousItemSelected = false;
			}
		}

		/// <summary>
		/// Called when the pointer exits the tab item area.
		/// </summary>
		void OnPointerExited()
		{
			if (IsSelected)
			{
				if (IndicatorPlacement != TabIndicatorPlacement.Fill ||
					(!IsSelected && IndicatorPlacement == TabIndicatorPlacement.Fill))
				{
					ChangeSelectedState();
				}
			}
			else
			{
				ChangeSelectedState();
			}
			_isPreviousItemSelected = false;
		}

		#endregion

		#region Internal Methods

		/// <summary>
		/// Changes the selected state of the tab item and updates its visual state.
		/// </summary>
		internal void ChangeSelectedState()
		{
			if (IsEnabled)
			{
				if (IsSelected)
				{
					string stateName = IndicatorPlacement == TabIndicatorPlacement.Fill ? "SelectedFilled" : "Selected";
					VisualStateManager.GoToState(this, stateName);
				}
				else
				{
					string stateName = IndicatorPlacement == TabIndicatorPlacement.Fill ? "NormalFilled" : "Normal";
					VisualStateManager.GoToState(this, stateName);
				}
			}
			else
			{
				string stateName = IndicatorPlacement == TabIndicatorPlacement.Fill ? "DisabledFilled" : "Disabled";
				VisualStateManager.GoToState(this, stateName);
			}
		}

		#endregion

		#region Protected Methods

		/// <summary>
		/// Measures the size of the control based on the given width and height constraints.
		/// </summary>
		/// <param name="widthConstraint">The width constraint.</param>
		/// <param name="heightConstraint">The height constraint.</param>
		/// <returns>The measured size.</returns>
		/// <exclude/>
		protected override Size MeasureOverride(double widthConstraint, double heightConstraint)
		{
			if (IsEnabled)
			{
				if (IsSelected)
				{
					if (IndicatorPlacement != TabIndicatorPlacement.Fill ||
						(!IsSelected && IndicatorPlacement == TabIndicatorPlacement.Fill))
					{
						string stateName = IndicatorPlacement == TabIndicatorPlacement.Fill ? "SelectedFilled" : "Selected";
						VisualStateManager.GoToState(this, stateName);
					}
				}
				else
				{
					string stateName = IndicatorPlacement == TabIndicatorPlacement.Fill ? "NormalFilled" : "Normal";
					VisualStateManager.GoToState(this, stateName);
				}
			}
			else
			{
				string stateName = IndicatorPlacement == TabIndicatorPlacement.Fill ? "DisabledFilled" : "Disabled";
				VisualStateManager.GoToState(this, stateName);
			}

			return base.MeasureOverride(widthConstraint, heightConstraint);
		}

		#endregion

		#region ITextElement Implementation

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

		#endregion
	}
}
