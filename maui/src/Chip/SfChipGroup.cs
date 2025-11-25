using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;
using Microsoft.Maui.Controls.Internals;
using Syncfusion.Maui.Toolkit.Themes;
using SelectionChangedEvent = Syncfusion.Maui.Toolkit.Chips.SelectionChangedEventArgs;
using SelectionChangingEvent = Syncfusion.Maui.Toolkit.Chips.SelectionChangingEventArgs;

namespace Syncfusion.Maui.Toolkit.Chips
{
	/// <summary>
	/// Represents a grouping control which adds <see cref="SfChip" /> control to a layout and grouped them for selection.
	/// </summary>
	[System.ComponentModel.DesignTimeVisible(true)]
	[ContentProperty(nameof(Items))]
	public partial class SfChipGroup : ContentView, IParentThemeElement
	{
		#region Fields

		List<SfChip>? _chipGroupChildren;
		SfChip? _previouslySelectedChip;
		SfChip? _previousSelectedItem;

		#endregion

		#region Bindable Properties

		/// <summary>
		/// Identifies the <see cref="DisplayMemberPath"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="DisplayMemberPath"/> property determines the member path for the display text in the chip.
		/// </remarks>
		public static readonly BindableProperty DisplayMemberPathProperty =
			BindableProperty.Create(
				nameof(DisplayMemberPath),
				typeof(string),
				typeof(SfChipGroup),
				string.Empty,
				BindingMode.Default,
				null,
				OnMemberPathChanged);

		/// <summary>
		/// Identifies the <see cref="ImageMemberPath"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="ImageMemberPath"/> property determines the member path for the icon image in the chip.
		/// </remarks>
		public static readonly BindableProperty ImageMemberPathProperty =
			BindableProperty.Create(
				nameof(ImageMemberPath),
				typeof(string),
				typeof(SfChipGroup),
				string.Empty,
				BindingMode.Default,
				null,
				OnMemberPathChanged);

		/// <summary>
		/// Identifies the <see cref="Items"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="Items"/> property determines the items to be set in the chip group.
		/// </remarks>
		public static readonly BindableProperty ItemsProperty =
			BindableProperty.Create(
				nameof(Items),
				typeof(ChipCollection),
				typeof(SfChipGroup),
				null,
				BindingMode.Default,
				null,
				OnItemsPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="ItemsSource"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="ItemsSource"/> property determines the items source for the chip group.
		/// </remarks>
		public static readonly BindableProperty ItemsSourceProperty =
			BindableProperty.Create(
				nameof(ItemsSource),
				typeof(IList),
				typeof(SfChipGroup),
				null,
				BindingMode.Default,
				null,
				OnItemsSourcePropertyChanged);

		/// <summary>
		/// Identifies the <see cref="ItemTemplate"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="ItemTemplate"/> property determines the custom content for data items in the chip group control.
		/// </remarks>
		public static readonly BindableProperty ItemTemplateProperty =
			BindableProperty.Create(
				nameof(ItemTemplate),
				typeof(DataTemplate),
				typeof(SfChipGroup),
				null,
				BindingMode.Default,
				null,
				OnMemberPathChanged);

		/// <summary>
		/// Identifies the <see cref="ChipLayout"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="ChipLayout"/> property determines the layout for all the chips in the chip group.
		/// </remarks>
		public static readonly BindableProperty ChipLayoutProperty =
			BindableProperty.Create(
				nameof(ChipLayout),
				typeof(Layout),
				typeof(SfChipGroup),
				null,
				BindingMode.Default,
				null,
				OnLayoutPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="ChipType"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="ChipType"/> property determines the type of the chips in the chip group.
		/// </remarks>
		public static readonly BindableProperty ChipTypeProperty =
			BindableProperty.Create(
				nameof(ChipType),
				typeof(SfChipsType),
				typeof(SfChipGroup),
				SfChipsType.Input,
				BindingMode.Default,
				null,
				OnChipTypePropertyChanged);

		/// <summary>
		/// Identifies the <see cref="SelectedItem"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="SelectedItem"/> property determines the selected item in the Choice chip group.
		/// </remarks>
		public static readonly BindableProperty SelectedItemProperty =
			BindableProperty.Create(
				nameof(SelectedItem),
				typeof(object),
				typeof(SfChipGroup),
				null,
				BindingMode.TwoWay,
				null,
				OnSelectedItemPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="InputView"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="InputView"/> property determines the input view or editor in the Input chip group.
		/// </remarks>
		public static readonly BindableProperty InputViewProperty =
			BindableProperty.Create(
				nameof(InputView),
				typeof(View),
				typeof(SfChipGroup),
				null,
				BindingMode.Default,
				null,
				OnInputViewPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="Command"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="Command"/> property determines the command to be set for the chip group.
		/// </remarks>
		public static readonly BindableProperty CommandProperty =
			BindableProperty.Create(
				nameof(Command),
				typeof(ICommand),
				typeof(SfChipGroup),
				null,
				BindingMode.OneWay,
				null,
				OnCommandChanged);

		/// <summary>
		/// Identifies the <see cref="ChipBackground"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="ChipBackground"/> property determines the background color of the chip in the chip group.
		/// </remarks>
		public static readonly BindableProperty ChipBackgroundProperty =
			BindableProperty.Create(
				nameof(ChipBackground),
				typeof(Brush),
				typeof(SfChipGroup),
				new SolidColorBrush(Colors.Transparent),
				BindingMode.Default,
				null,
				OnChipBackgroundPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="ChipStroke"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="ChipStroke"/> property determines the border color of the chip in the chip group.
		/// </remarks>
		public static readonly BindableProperty ChipStrokeProperty =
			BindableProperty.Create(
				nameof(ChipStroke),
				typeof(Brush),
				typeof(SfChipGroup),
				new SolidColorBrush(Color.FromArgb("#79747E")),
				BindingMode.Default,
				null,
				OnChipStrokePropertyChanged);

		/// <summary>
		/// Identifies the <see cref="ChipTextColor"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="ChipTextColor"/> property determines the text color of the chip in the chip group.
		/// </remarks>
		public static readonly BindableProperty ChipTextColorProperty =
			BindableProperty.Create(
				nameof(ChipTextColor),
				typeof(Color),
				typeof(SfChipGroup),
				Color.FromArgb("#49454F"),
				BindingMode.Default,
				null,
				OnChipTextColorPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="ChipTextSize"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="ChipTextSize"/> property determines the text size for the chip in the chip group.
		/// </remarks>
		public static readonly BindableProperty ChipTextSizeProperty =
			BindableProperty.Create(
				nameof(ChipTextSize),
				typeof(double),
				typeof(SfChipGroup),
				14d,
				BindingMode.Default,
				null,
				OnChipTextSizePropertyChanged);

		/// <summary>
		/// Identifies the <see cref="ChipFontAttributes"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="ChipFontAttributes"/> property determines the font attributes (bold or italic) for the chip text.
		/// </remarks>
		public static readonly BindableProperty ChipFontAttributesProperty =
			BindableProperty.Create(
				nameof(ChipFontAttributes),
				typeof(FontAttributes),
				typeof(SfChipGroup),
				FontAttributes.None,
				BindingMode.Default,
				null,
				OnChipFontAttributesPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="ChipFontFamily"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="ChipFontFamily"/> property determines the font family for the chip in the chip group.
		/// </remarks>
		public static readonly BindableProperty ChipFontFamilyProperty =
			BindableProperty.Create(
				nameof(ChipFontFamily),
				typeof(string),
				typeof(SfChipGroup),
				string.Empty,
				BindingMode.Default,
				null,
				OnChipFontFamilyPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="ChipPadding"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="ChipPadding"/> property determines the spacing between the chips.
		/// </remarks>
		public static readonly BindableProperty ChipPaddingProperty =
			BindableProperty.Create(
				nameof(ChipPadding),
				typeof(Thickness),
				typeof(SfChipGroup),
				new Thickness(2d, 1d, 1d, 1d),
				BindingMode.Default,
				null,
				OnChipPaddingPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="ChipStrokeThickness"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="ChipStrokeThickness"/> property determines the border width of the chips.
		/// </remarks>
		public static readonly BindableProperty ChipStrokeThicknessProperty =
			BindableProperty.Create(
				nameof(ChipStrokeThickness),
				typeof(double),
				typeof(SfChipGroup),
				1d,
				BindingMode.Default,
				null,
				OnChipStrokeThicknessPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="ItemHeight"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="ItemHeight"/> property determines the height of the chips in the group.
		/// </remarks>
		public static readonly BindableProperty ItemHeightProperty =
			BindableProperty.Create(
				nameof(ItemHeight),
				typeof(double),
				typeof(SfChipGroup),
				double.NaN,
				BindingMode.Default,
				null,
				OnItemHeightPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="ShowIcon"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="ShowIcon"/> property determines whether the icon is visible in the chip.
		/// </remarks>
		public static readonly BindableProperty ShowIconProperty =
			BindableProperty.Create(
				nameof(ShowIcon),
				typeof(bool),
				typeof(SfChipGroup),
				false,
				BindingMode.Default,
				null,
				OnShowIconPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="CloseButtonColor"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="CloseButtonColor"/> property determines the color of the close button in the chip.
		/// </remarks>
		public static readonly BindableProperty CloseButtonColorProperty =
			BindableProperty.Create(
				nameof(CloseButtonColor),
				typeof(Color),
				typeof(SfChipGroup),
				Color.FromArgb("#1C1B1F"),
				BindingMode.Default,
				null,
				OnCloseButtonColorPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="SelectionIndicatorColor"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="SelectionIndicatorColor"/> property determines the color of the selection indicator in the chip.
		/// </remarks>
		public static readonly BindableProperty SelectionIndicatorColorProperty =
			BindableProperty.Create(
				nameof(SelectionIndicatorColor),
				typeof(Color),
				typeof(SfChipGroup),
				Color.FromArgb("#49454F"),
				BindingMode.Default,
				null,
				OnSelectionIndicatorColorPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="ChipImageSize"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="ChipImageSize"/> property determines the width and height of the chip image.
		/// </remarks>
		public static readonly BindableProperty ChipImageSizeProperty =
			BindableProperty.Create(
				nameof(ChipImageSize),
				typeof(double),
				typeof(SfChipGroup),
				18d,
				BindingMode.Default,
				null,
				OnChipImageSizePropertyChanged);

		/// <summary>
		/// Identifies the <see cref="ChoiceMode"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="ChoiceMode"/> property determines the choice mode of the chip group.
		/// </remarks>
		public static readonly BindableProperty ChoiceModeProperty =
			BindableProperty.Create(
				nameof(ChoiceMode),
				typeof(ChoiceMode),
				typeof(SfChipGroup),
				ChoiceMode.Single,
				BindingMode.Default,
				null,
				null);

		/// <summary>
		/// Identifies the <see cref="ChipCornerRadius"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="ChipCornerRadius"/> property determines the corner radius of the chip in the chip group.
		/// </remarks>
		public static readonly BindableProperty ChipCornerRadiusProperty =
			BindableProperty.Create(
				nameof(ChipCornerRadius),
				typeof(CornerRadius),
				typeof(SfChipGroup),
				new CornerRadius(8),
				BindingMode.Default,
				null,
				OnCornerRadiusPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="SelectedChipBackground"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="SelectedChipBackground"/> property determines the background color of the selected chip in the chip group.
		/// </remarks>
		public static readonly BindableProperty SelectedChipBackgroundProperty =
			BindableProperty.Create(
				nameof(SelectedChipBackground),
				typeof(Brush),
				typeof(SfChipGroup),
				new SolidColorBrush(Color.FromArgb("#E8DEF8")),
				BindingMode.Default,
				null,
				OnSelectedChipBackgroundPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="SelectedChipTextColor"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="SelectedChipTextColor"/> property determines the text color for the selected chip in the chip group.
		/// </remarks>
		public static readonly BindableProperty SelectedChipTextColorProperty =
			BindableProperty.Create(
				nameof(SelectedChipTextColor),
				typeof(Color),
				typeof(SfChipGroup),
				Color.FromArgb("#1D192B"),
				BindingMode.Default,
				null,
				OnSelectedChipTextColorPropertyChanged);

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="SfChipGroup"/> class. This is constructor of the SfChipGroup class.
		/// </summary>
		public SfChipGroup()
		{
			_chipGroupChildren = [];
			ThemeElement.InitializeThemeResources(this, "SfChipGroupTheme");
			InitializeCollection();
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the value of the ChipCollection. This chip collection of <see cref="SfChip"/> is used to generate the chips in <see cref="SfChipGroup"/>.
		/// </summary>
		/// <value>
		/// The collection used to populate the chip items. The default is null.
		/// </value>
		/// <example>
		/// Here is an example of how to set the <see cref="Items"/> property
		///
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <local:SfChipGroup>
		///   <local:SfChipGroup.Items>
		///      <local:SfChip Text="Chip1" />
		///          <local:SfChip Text="Chip2" />
		///   </local:SfChipGroup.Items>
		/// </local:SfChipGroup>
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var chipGroup = new SfChipGroup();
		/// 
		/// chipGroup.Items.Add(new SfChip { Text = "Chip1" });
		/// chipGroup.Items.Add(new SfChip { Text = "Chip2" });
		/// ]]></code>
		/// </example>
		public ChipCollection? Items
		{
			get { return (ChipCollection)GetValue(ItemsProperty); }
			set { SetValue(ItemsProperty, value); }
		}

		/// <summary>
		/// Gets or sets the value of ItemsSource. It is a collection of items to generate the chips in <see cref="SfChipGroup"/>.
		/// </summary>
		/// <value>
		/// The collection used to populate the items. The default is null.
		/// </value>
		/// <example>
		/// Here is an example of how to set the <see cref="ItemsSource"/> property
		///
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <local:SfChipGroup ItemsSource="{Binding ChipItems}" />
		/// ]]></code>
		///
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var chipItems = new List<string> { "Chip1", "Chip2" };
		/// 
		/// var chipGroup = new SfChipGroup();
		/// chipGroup.ItemsSource = chipItems;
		/// ]]></code>
		/// </example>
		public IList ItemsSource
		{
			get { return (IList)GetValue(ItemsSourceProperty); }
			set { SetValue(ItemsSourceProperty, value); }
		}

		/// <summary>
		/// Gets or sets the value of ChipFontAttributes. This property can be used to change the font of the chip text in either bold or italic in the chip group.
		/// </summary>
		/// <value>
		/// Specifies the chip text font attributes. The default value is <see cref="FontAttributes.None"/>.
		/// </value>
		/// <example>
		/// Here is an example of how to set the <see cref="ChipFontAttributes"/> property
		///
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <local:SfChipGroup ChipFontAttributes="Bold" />
		/// ]]></code>
		///
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var chipGroup = new SfChipGroup
		/// {
		///     ChipFontAttributes = FontAttributes.Bold
		/// };
		/// ]]></code>
		/// </example>
		public FontAttributes ChipFontAttributes
		{
			get { return (FontAttributes)GetValue(ChipFontAttributesProperty); }
			set { SetValue(ChipFontAttributesProperty, value); }
		}

		/// <summary>
		/// Gets or sets the value of ItemTemplate. This property can be used to set custom content for data item in the chip group control.
		/// </summary>
		/// <value>
		/// A DataTemplate object that is used to display the custom content. The default is null.
		/// </value>
		/// <example>
		/// Here is an example of how to set the <see cref="ItemTemplate"/> property
		///
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <local:SfChipGroup>
		///    <local:SfChipGroup.ItemTemplate>
		///        <DataTemplate>
		///            <local:SfChip Text="{Binding Name}" />
		///        </DataTemplate>
		///    </local:SfChipGroup.ItemTemplate>
		/// </local:SfChipGroup>
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var chipGroup = new SfChipGroup
		/// {
		///     ItemTemplate = new DataTemplate(() =>
		///     {
		///         var chip = new SfChip();
		///         chip.SetBinding(SfChip.TextProperty, "Name");
		///         return chip;
		///     })
		/// };
		/// ]]></code>
		/// </example>
		public DataTemplate ItemTemplate
		{
			get { return (DataTemplate)GetValue(ItemTemplateProperty); }
			set { SetValue(ItemTemplateProperty, value); }
		}

		/// <summary>
		/// Gets or sets the value of the ChipImageSize. This property can be used to customize the width and height of the chip image. This is a bindable property.
		/// </summary>
		/// <value>
		/// Specifies the chip image size. The default value is 18d.
		/// </value>
		/// <example>
		/// Here is an example of how to set the <see cref="ChipImageSize"/> property
		///
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <local:SfChipGroup ChipImageSize="30" />
		/// ]]>
		/// </code>
		///
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var chipGroup = new SfChipGroup
		/// {
		///     ChipImageSize = 30
		/// };
		/// ]]></code>
		/// </example>
		public double ChipImageSize
		{
			get { return (double)GetValue(ChipImageSizeProperty); }
			set { SetValue(ChipImageSizeProperty, value); }
		}

		/// <summary>
		/// Gets or sets the value of ChipLayout. This property can be used to add all the chips in a chip group.
		/// </summary>
		/// <value>
		/// Specifies the chip layout. The default value is null.
		/// </value>
		/// <example>
		/// Here is an example of how to set the <see cref="ChipLayout"/> property
		///
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <local:SfChipGroup>
		///    <local:SfChipGroup.ChipLayout>
		///       <StackLayout Orientation="Horizontal" />
		///    </local:SfChipGroup.ChipLayout>
		/// </local:SfChipGroup>
		/// ]]>
		/// </code>
		///
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var chipGroup = new SfChipGroup
		/// {
		///     ChipLayout = new StackLayout { Orientation = StackOrientation.Horizontal }
		/// };
		/// ]]></code>
		/// </example>
		public Layout? ChipLayout
		{
			get { return (Layout)GetValue(ChipLayoutProperty); }
			set { SetValue(ChipLayoutProperty, value); }
		}

		/// <summary>
		/// Gets or sets the value of DisplayMemberPath, which is the member path value of the display text in the chip.
		/// </summary>
		/// <value>
		/// Specifies the display member path. The default value is string.Empty.
		/// </value>
		/// <example>
		/// Here is an example of how to set the <see cref="DisplayMemberPath"/> property
		///
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <local:SfChipGroup DisplayMemberPath="Name" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var chipGroup = new SfChipGroup
		/// {
		///     DisplayMemberPath = "Name"
		/// };
		/// ]]></code>
		/// </example>
		public string DisplayMemberPath
		{
			get { return (string)GetValue(DisplayMemberPathProperty); }
			set { SetValue(DisplayMemberPathProperty, value); }
		}

		/// <summary>
		/// Gets or sets the value of ImageMemberPath. It is the member path for the icon image of the chip in a chip group.
		/// </summary>
		/// <value>
		/// Specifies the image member path. The default value is string.Empty.
		/// </value>
		/// <example>
		/// Here is an example of how to set the <see cref="ImageMemberPath"/> property
		///
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <local:SfChipGroup ImageMemberPath="Icon" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var chipGroup = new SfChipGroup
		/// {
		///     ImageMemberPath = "Icon"
		/// };
		/// ]]></code>
		/// </example>
		public string ImageMemberPath
		{
			get { return (string)GetValue(ImageMemberPathProperty); }
			set { SetValue(ImageMemberPathProperty, value); }
		}

		/// <summary>
		/// Gets or sets the value of ChipType. This property can be used to set the type of the chips in the chip group.
		/// </summary>
		/// <value>
		/// Specifies the chip type. The default value is <see cref="SfChipsType.Input"/>.
		/// </value>
		/// <example>
		/// Here is an example of how to set the <see cref="ChipType"/> property
		///
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <local:SfChipGroup ChipType="Choice" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var chipGroup = new SfChipGroup
		/// {
		///     ChipType = SfChipsType.Choice
		/// };
		/// ]]></code>
		/// </example>
		public SfChipsType ChipType
		{
			get { return (SfChipsType)GetValue(ChipTypeProperty); }
			set { SetValue(ChipTypeProperty, value); }
		}

		/// <summary>
		/// Gets or sets the value of ChipCornerRadius. This property can be used to customize the corners of the chip control.
		/// </summary>
		/// <value>
		/// Specifies the corner radius. The default value is CornerRadius(8).
		/// </value>
		/// <example>
		/// Here is an example of how to set the <see cref="ChipCornerRadius"/> property
		///
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <local:SfChipGroup ChipCornerRadius="10" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var chipGroup = new SfChipGroup
		/// {
		///     ChipCornerRadius = new CornerRadius(10)
		/// };
		/// ]]></code>
		/// </example>
		public CornerRadius ChipCornerRadius
		{
			get { return (CornerRadius)GetValue(ChipCornerRadiusProperty); }
			set { SetValue(ChipCornerRadiusProperty, value); }
		}

		/// <summary>
		/// Gets or sets the value of SelectedItem. This property can be used to select a particular item in the choice and fliter chip group.
		/// </summary>
		/// <value>
		/// Specifies the selected item. The default value is null.
		/// </value>
		/// <example>
		/// Here is an example of how to set the <see cref="SelectedItem"/> property
		///
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <local:SfChipGroup SelectedItem="{Binding SelectedChip}" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var chipGroup = new SfChipGroup
		/// {
		///     SelectedItem = SelectedChip
		/// };
		/// ]]></code>
		/// </example>
		public object? SelectedItem
		{
			get { return (object)GetValue(SelectedItemProperty); }
			set { SetValue(SelectedItemProperty, value); }
		}

		/// <summary>
		/// Gets or sets the value of InputView. This property can be used to set input view or editor in the input chip group.
		/// </summary>
		/// <value>
		/// Specifies the input view. The default value is null.
		/// </value>
		/// <example>
		/// Here is an example of how to set the <see cref="InputView"/> property
		///
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <local:SfChip ChipType="Input" >
		/// 	<local:SfChip.InputView>
		/// 		<Entry Placeholder="Enter Name" />
		///		</local:SfChip.InputView>
		/// </local:SfChip>
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var chip = new SfChip
		/// {
		///     ChipType = SfChipsType.Input
		/// };
		/// chip.InputView = new Entry() { Placeholder = "Enter Name" };
		/// ]]></code>
		/// </example>
		public View? InputView
		{
			get { return (View)GetValue(InputViewProperty); }
			set { SetValue(InputViewProperty, value); }
		}

		/// <summary>
		/// Gets or sets the value of Command. This property can be used to give command to chip group.
		/// </summary>
		/// <value>
		/// Specifies the command. The default value is null.
		/// </value>
		/// <example>
		/// Here is an example of how to set the <see cref="Command"/> property
		///
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <local:SfChipGroup Command="{Binding ChipCommand}" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var chipGroup = new SfChipGroup
		/// {
		///     Command = new Command(() => { /* Command logic here */ })
		/// };
		/// ]]></code>
		/// </example>
		public ICommand Command
		{
			get { return (ICommand)GetValue(CommandProperty); }
			set { SetValue(CommandProperty, value); }
		}

		/// <summary>
		/// Gets or sets the value of the ChipStroke. This property can be used to change the border color of the chip in the chip group.
		/// </summary>
		/// <value>
		/// Specifies the chip stroke color. The default value is SolidColorBrush(Color.FromArgb("#79747E")).
		/// </value>
		/// <example>
		/// Here is an example of how to set the <see cref="ChipStroke"/> property
		///
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <local:SfChipGroup ChipStroke="Red" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var chipGroup = new SfChipGroup
		/// {
		///     ChipStroke = new SolidColorBrush(Colors.Red)
		/// };
		/// ]]></code>
		/// </example>
		public Brush ChipStroke
		{
			get { return (Brush)GetValue(ChipStrokeProperty); }
			set { SetValue(ChipStrokeProperty, value); }
		}

		/// <summary>
		/// Gets or sets the value of the ChipStrokeThickness. This property can be used to change the border width of chips.
		/// </summary>
		/// <value>
		/// Specifies the chip border width. The default value is 1d.
		/// </value>
		/// <example>
		/// Here is an example of how to set the <see cref="ChipStrokeThickness"/> property
		///
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <local:SfChipGroup ChipStrokeThickness="2" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var chipGroup = new SfChipGroup
		/// {
		///     ChipStrokeThickness = 2
		/// };
		/// ]]></code>
		/// </example>
		public double ChipStrokeThickness
		{
			get { return (double)GetValue(ChipStrokeThicknessProperty); }
			set { SetValue(ChipStrokeThicknessProperty, value); }
		}

		/// <summary>
		/// Gets or sets the value of ChipTextColor. This property can be used to change the text color of the chip in the chip group.
		/// </summary>
		/// <value>
		/// Specifies the chip text color. The default value is Color.FromArgb("#49454F").
		/// </value>
		/// <example>
		/// Here is an example of how to set the <see cref="ChipTextColor"/> property
		///
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <local:SfChipGroup ChipTextColor="Blue" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var chipGroup = new SfChipGroup
		/// {
		///     ChipTextColor = Colors.Blue
		/// };
		/// ]]></code>
		/// </example>
		public Color ChipTextColor
		{
			get { return (Color)GetValue(ChipTextColorProperty); }
			set { SetValue(ChipTextColorProperty, value); }
		}

		/// <summary>
		/// Gets or sets a color value that can be used to customize the text color of the selected chips in the chip group.
		/// </summary>
		/// <value>
		/// Specifies the text color of the selected chips. The default value is Color.FromArgb("#1D192B").
		/// </value>
		/// <example>
		/// Here is an example of how to set the <see cref="SelectedChipTextColor"/> property
		///
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <local:SfChipGroup SelectedChipTextColor="Red" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var chipGroup = new SfChipGroup
		/// {
		///     SelectedChipTextColor = Colors.Red
		/// };
		/// ]]></code>
		/// </example>
		public Color SelectedChipTextColor
		{
			get { return (Color)GetValue(SelectedChipTextColorProperty); }
			set { SetValue(SelectedChipTextColorProperty, value); }
		}

		/// <summary>
		/// Gets or sets a color value that can be used to customize the background color of the selected chips in the chip group.
		/// </summary>
		/// <value>
		/// Specifies the background color of the selected chips. The default value is SolidColorBrush(Color.FromArgb("#E8DEF8")).
		/// </value>
		/// <example>
		/// Here is an example of how to set the <see cref="SelectedChipBackground"/> property
		///
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <local:SfChipGroup SelectedChipBackground="LightBlue" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var chipGroup = new SfChipGroup
		/// {
		///     SelectedChipBackground = new SolidColorBrush(Colors.LightBlue)
		/// };
		/// ]]></code>
		/// </example>
		public Brush SelectedChipBackground
		{
			get { return (Brush)GetValue(SelectedChipBackgroundProperty); }
			set { SetValue(SelectedChipBackgroundProperty, value); }
		}

		/// <summary>
		/// Gets or sets the value of ChipTextSize. This property can be used to change the text size for the chip in the chip group.
		/// </summary>
		/// <value>
		/// Specifies the chip text size. The default value is 14d.
		/// </value>
		/// <example>
		/// Here is an example of how to set the <see cref="ChipTextSize"/> property
		///
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <local:SfChipGroup ChipTextSize="16" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var chipGroup = new SfChipGroup
		/// {
		///     ChipTextSize = 16
		/// };
		/// ]]></code>
		/// </example>
		[TypeConverter(typeof(FontSizeConverter))]
		public double ChipTextSize
		{
			get { return (double)GetValue(ChipTextSizeProperty); }
			set { SetValue(ChipTextSizeProperty, value); }
		}

		/// <summary>
		/// Gets or sets the font family for the chip text.
		/// </summary>
		/// <value>
		/// A <see cref="string"/> representing the font family to be used for the chip text. The default value is string.Empty.
		/// </value>
		/// <example>
		/// Here is an example of how to set the <see cref="ChipFontFamily"/> property
		///
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <local:SfChipGroup ChipFontFamily="Arial" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var chipGroup = new SfChipGroup
		/// {
		///     ChipFontFamily = "Arial"
		/// };
		/// ]]></code>
		/// </example>
		public string ChipFontFamily
		{
			get { return (string)GetValue(ChipFontFamilyProperty); }
			set { SetValue(ChipFontFamilyProperty, value); }
		}

		/// <summary>
		/// Gets or sets the value of ChipBackgroundColor. This property can be used to change the background color of the chip in the chip group.
		/// </summary>
		/// <value>
		/// Specifies the chip background color. The default value is SolidColorBrush(Colors.Transparent).
		/// </value>
		/// <example>
		/// Here is an example of how to set the <see cref="ChipBackground"/> property
		///
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <local:SfChipGroup ChipBackground="Yellow" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var chipGroup = new SfChipGroup
		/// {
		///     ChipBackground = new SolidColorBrush(Colors.Yellow)
		/// };
		/// ]]></code>
		/// </example>
		public Brush ChipBackground
		{
			get { return (Brush)GetValue(ChipBackgroundProperty); }
			set { SetValue(ChipBackgroundProperty, value); }
		}

		/// <summary>
		/// Gets or sets the value of SelectionIndicatorColor. Using this property, you can change the color of the selection indicator in chip.
		/// </summary>
		/// <value>
		/// Specifies the selection indicator color. The default value is Color.FromArgb("#49454F").
		/// </value>
		/// <example>
		/// Here is an example of how to set the <see cref="SelectionIndicatorColor"/> property
		///
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <local:SfChipGroup SelectionIndicatorColor="Green" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var chipGroup = new SfChipGroup
		/// {
		///     SelectionIndicatorColor = Colors.Green
		/// };
		/// ]]></code>
		/// </example>
		public Color SelectionIndicatorColor
		{
			get { return (Color)GetValue(SelectionIndicatorColorProperty); }
			set { SetValue(SelectionIndicatorColorProperty, value); }
		}

		/// <summary>
		/// Gets or sets the value of CloseButtonColor. Using this property, you can change the color of the close button in chip.
		/// </summary>
		/// <value>
		/// Specifies the close button color. The default value is Color.FromArgb("#1C1B1F").
		/// </value>
		/// <example>
		/// Here is an example of how to set the <see cref="CloseButtonColor"/> property
		///
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <local:SfChipGroup CloseButtonColor="Red" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var chipGroup = new SfChipGroup
		/// {
		///     CloseButtonColor = Colors.Red
		/// };
		/// ]]></code>
		/// </example>
		public Color CloseButtonColor
		{
			get { return (Color)GetValue(CloseButtonColorProperty); }
			set { SetValue(CloseButtonColorProperty, value); }
		}

		/// <summary>
		/// Gets or sets the value of ChipPadding. This chip padding is used for arranging the items with padding.
		/// </summary>
		/// <value>
		/// Specifies the chip padding. The default value is Thickness(2d, 1d, 1d, 1d).
		/// </value>
		/// <example>
		/// Here is an example of how to set the <see cref="ChipPadding"/> property
		///
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <local:SfChipGroup ChipPadding="10,5,10,5" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var chipGroup = new SfChipGroup
		/// {
		///     ChipPadding = new Thickness(10, 5, 10, 5)
		/// };
		/// ]]></code>
		/// </example>
		public Thickness ChipPadding
		{
			get { return (Thickness)GetValue(ChipPaddingProperty); }
			set { SetValue(ChipPaddingProperty, value); }
		}

		/// <summary>
		/// Gets or sets the value of the ItemHeight. This property can be used to customize the height of the chips in the group.
		/// </summary>
		/// <value>
		/// Specifies the height of the item. The default value is double.NaN.
		/// </value>
		/// <example>
		/// Here is an example of how to set the <see cref="ItemHeight"/> property
		///
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <local:SfChipGroup ItemHeight="50" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var chipGroup = new SfChipGroup
		/// {
		///     ItemHeight = 50
		/// };
		/// ]]></code>
		/// </example>
		public double ItemHeight
		{
			get { return (double)GetValue(ItemHeightProperty); }
			set { SetValue(ItemHeightProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether an icon is shown in chips. The icon is in the visible state when the ShowIcon property is true; otherwise, icons will be collapsed.
		/// </summary>
		/// <value>
		/// Specifies whether to show the icon. The default value is false.
		/// </value>
		/// <example>
		/// Here is an example of how to set the <see cref="ShowIcon"/> property
		///
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <local:SfChipGroup ShowIcon="True" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var chipGroup = new SfChipGroup
		/// {
		///     ShowIcon = true
		/// };
		/// ]]></code>
		/// </example>
		public bool ShowIcon
		{
			get { return (bool)GetValue(ShowIconProperty); }
			set { SetValue(ShowIconProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value that indicates the selection mode of chip item in the <see cref="SfChipsType.Choice"/> type.
		/// </summary>
		/// <value> Specifies the choice mode. The default value is <see cref="ChoiceMode.Single"/>. </value>
		/// <remarks> This property is applicable for <see cref="SfChipsType.Choice"/> only. </remarks>  
		/// <example>
		/// Here is an example of how to set the <see cref="ChoiceMode"/> property
		///
		/// # [XAML](#tab/tabid-1)
		/// <code><![CDATA[
		/// <local:SfChipGroup ChoiceMode="Multiple" />
		/// ]]></code>
		/// 
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// var chipGroup = new SfChipGroup
		/// {
		///     ChoiceMode = ChoiceMode.Multiple
		/// };
		/// ]]></code>
		/// </example>
		public ChoiceMode ChoiceMode
		{
			get { return (ChoiceMode)GetValue(ChoiceModeProperty); }
			set { SetValue(ChoiceModeProperty, value); }
		}

		#endregion

		#region Internal Properties

		/// <summary>
		/// Sets the content. It is set to internal for hiding this property.
		/// </summary>
		/// <value>The content.</value>
		internal new View? Content
		{
			set { base.Content = value; }
		}

		internal bool IsEditorControl { get; set; }

		#endregion

		#region Public Methods

		/// <summary>
		/// This method returns a read-only collection of chips in <see cref="SfChipGroup"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="ReadOnlyCollection{T}"/> of <see cref="SfChip"/> representing the children.
		/// </returns>
		/// <remarks>
		/// This method ensures that the collection of chips is not modifiable from outside the class.
		/// </remarks>
		/// <example>
		/// The following XAML demonstrates how to define a <see cref="SfChipGroup"/>:
		/// <code lang="xaml">
		/// <![CDATA[
		///     <local:SfChipGroup x:Name="chipGroup">
		///         <local:SfChip Text="Chip 1" />
		///         <local:SfChip Text="Chip 2" />
		///         <local:SfChip Text="Chip 3" />
		///     </local:SfChipGroup>
		/// ]]>
		/// </code>
		/// The following C# code demonstrates how to use the <see cref="GetChips"/> method:
		/// <code lang="C#">
		/// <![CDATA[
		/// ReadOnlyCollection<SfChip> chips = chipGroup.GetChips();
		/// ]]>
		/// </code>
		/// </example>
		public ReadOnlyCollection<SfChip> GetChips()
		{
			return new ReadOnlyCollection<SfChip>(_chipGroupChildren ?? []);
		}

		#endregion

		#region Internal methods

		/// <summary>
		/// Handles the Items collection changed.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="eventArgs">Event arguments.</param>
		internal void ItemsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs eventArgs)
		{
			CollectionChanged(SfChipGroup.ItemsProperty.PropertyName, eventArgs);
		}

		/// <summary>
		/// Removes the chip from layout.
		/// </summary>
		/// <param name="chip">Chip.</param>
		internal void RemoveChipFromLayout(SfChip chip)
		{
			if (chip != null)
			{
				UnsubscribeChipEvents(chip);
				if (ChipLayout != null && ChipLayout.Count > 0 && ChipLayout.Children.Contains(chip))
				{
					ChipLayout.Children.Remove(chip);
				}

				if (_chipGroupChildren != null && _chipGroupChildren.Count > 0 && _chipGroupChildren.Contains(chip))
				{
					_chipGroupChildren.Remove(chip);
				}
			}
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Returns the value of a property in an object.
		/// </summary>
		/// <param name="propertyName">The property name.</param>
		/// <param name="item">The element from chips collection.</param>
		/// <returns>The property value from item.</returns>
		[RequiresUnreferencedCode("The GetPropertyValue method is not trim compatible")] 
		static object GetPropertyValue(string propertyName, object item)
		{
			var propertyInfo = item.GetType().GetProperty(propertyName);
			return propertyInfo?.GetValue(item) ?? string.Empty;
		}


		/// <summary>
		/// Gets the chip data context from ItemsSource property.
		/// </summary>
		/// <returns>The chip data context.</returns>
		/// <param name="chip">The Chip.</param>
		static object? GetChipDataContext(SfChip chip)
		{
			return chip.IsCreatedInternally ? chip.DataContext : chip;
		}


		/// <summary>
		/// Trigger the selection change event.
		/// </summary>
		/// <param name="item">Selected chip item.</param>
		/// <param name="isSelected">Tells weather chip is selected or not.</param>
		void SelectedItemChanged(object item, bool isSelected)
		{
			if (item is SfChip chip)
			{
				SelectionChangedEvent eventArgs = new SelectionChangedEvent();
				if (ChipType == SfChipsType.Filter)
				{
					if (isSelected)
					{
						eventArgs.AddedItem = GetChipDataContext(chip);
					}
					else
					{
						eventArgs.RemovedItem = GetChipDataContext(chip);
					}
				}
				else
				{
					eventArgs.RemovedItem = _previouslySelectedChip == null || _previousSelectedItem == null ? null : SfChipGroup.GetChipDataContext(_previousSelectedItem);
					eventArgs.AddedItem = GetChipDataContext(chip);
				}

				RaiseSelectionChangedEvent(this, eventArgs);
			}
		}

		/// <summary>
		/// ItemsSource collection changed.
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="collection">New value.</param>
		[RequiresUnreferencedCode("The SubscribeCollectionChanged method is not trim compatible")] 
		void SubscribeCollectionChanged(string propertyName, INotifyCollectionChanged? collection)
		{
			if (collection != null)
			{
				if (propertyName == SfChipGroup.ItemsSourceProperty.PropertyName)
				{
					collection.CollectionChanged -= ItemsSourceCollectionChanged;
					collection.CollectionChanged += ItemsSourceCollectionChanged;
				}
				else
				{
					collection.CollectionChanged -= ItemsCollectionChanged;
					collection.CollectionChanged += ItemsCollectionChanged;
				}
			}
		}

		void UpdateInputView(View oldInputView, View newInputView)
		{
			if (ChipLayout == null)
			{
				return;
			}

			if (oldInputView != null && ChipLayout.Children.Contains(oldInputView))
			{
				ChipLayout.Children.Remove(oldInputView);
			}

			if (ChipType == SfChipsType.Input && newInputView != null)
			{
				AddChildToLayout(newInputView);
			}
		}

		/// <summary>
		/// Chip group collection changed.
		/// </summary>
		/// <param name="oldValue">Old value.</param>
		/// <param name="newValue">New value.</param>
		/// <param name="propertyName">Property name.</param>
		[UnconditionalSuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code", Justification = "<Pending>")]
		void ChipGroupCollectionChanged(object oldValue, object newValue, string propertyName)
		{
			if (oldValue != null)
			{
				if (oldValue is INotifyCollectionChanged collection)
				{
					UnsubscribeCollectionChanged(propertyName, collection);
				}
			}

			if (newValue != null)
			{
				if (newValue is INotifyCollectionChanged collection)
				{
					SubscribeCollectionChanged(propertyName, collection);
				}
			}

			ClearAndCreateChips(oldValue, false);
		}

		/// <summary>
		/// Unsubscribes the collection events.
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="collection">Collection.</param>
		void UnsubscribeCollectionChanged(string propertyName, INotifyCollectionChanged collection)
		{
			if (collection != null)
			{
				if (propertyName == SfChipGroup.ItemsSourceProperty.PropertyName)
				{
					collection.CollectionChanged -= ItemsSourceCollectionChanged;
				}
				else
				{
					collection.CollectionChanged -= ItemsCollectionChanged;
				}
			}
		}

		/// <summary>
		/// Updates the selected chips in the group.
		/// </summary>
		/// <param name="oldValue">The old selected value.</param>
		/// <param name="newValue">The new selected value.</param>
		void OnSelectedItemsPropertyChanged(object oldValue, object newValue)
		{
			if (newValue != null)
			{
				if (newValue is IList list && list.Count > 0)
				{
					SelectOrUnselectMultipleChips(list);
					RaiseSelectionChangedEvent(this, new SelectionChangedEvent() { AddedItem = newValue, RemovedItem = oldValue });
				}
			}

			if (newValue == null && oldValue != null)
			{
				if (oldValue is IList list && list.Count > 0)
				{
					UnselectChips(list);
					RaiseSelectionChangedEvent(this, new SelectionChangedEvent() { AddedItem = null, RemovedItem = list });
				}
			}

			if (SelectedItem is INotifyCollectionChanged selectedItem)
			{
				selectedItem.CollectionChanged -= SelectedItemsCollectionChanged;
				selectedItem.CollectionChanged += SelectedItemsCollectionChanged;
			}
		}

		void UnselectChips(IList list)
		{
			foreach (var item in list)
			{
				SfChip? chip = item as SfChip ?? GetChip(item);

				if (chip != null)
				{
					if (ChipType == SfChipsType.Filter)
					{
						UnselectFilterChip(chip);
					}
					else if (ChipType == SfChipsType.Choice)
					{
						UnselectChoiceChip();
					}
				}
			}
		}

		/// <summary>
		/// Triggers when the selected items collection changed.
		/// </summary>
		/// <param name="sender">The sender of the collection.</param>
		/// <param name="e">The notify collection changed event arguments.</param>
		void SelectedItemsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
		{
			if (ChipType == SfChipsType.Filter || ChipType == SfChipsType.Choice)
			{
				if (e.Action == NotifyCollectionChangedAction.Add)
				{
					if (e.NewItems != null && e.NewItems.Count > 0)
					{
						SfChip? chip = GetChip(e.NewItems[0]);
						if (chip != null)
						{
							if (ItemTemplate != null)
							{
								chip.ShowSelectionIndicator = false;
								chip.IsSelected = true;
							}
							else
							{
								chip.ShowSelectionIndicator = chip.IsSelected = true;
							}
							ApplyChipColor(chip);
							SelectedItemChanged(chip, true);
						}
					}
				}
				else if (e.Action == NotifyCollectionChangedAction.Remove)
				{
					if (e.OldItems != null && e.OldItems.Count > 0)
					{
						SfChip? chip = GetChip(e.OldItems[0]);
						if (chip != null)
						{
							DeselectChip(chip);
						}
					}
				}
				else if (e.Action == NotifyCollectionChangedAction.Reset)
				{
					if (_chipGroupChildren != null)
					{
						foreach (SfChip chip in _chipGroupChildren)
						{
							DeselectChip(chip);
						}
					}
				}
			}
		}

		/// <summary>
		/// Deselect the chip, when it remove from <see cref="SelectedItem"/>.
		/// </summary>
		/// <param name="chip">chip.</param>
		void DeselectChip(SfChip chip)
		{
			chip.ShowSelectionIndicator = chip.IsSelected = false;
			ApplyChipColor(chip);
			SelectedItemChanged(chip, false);
		}

		/// <summary>
		/// Handles the notify collection changed event handler.
		/// </summary>
		/// <param name="propertyName">The property name.</param>
		/// <param name="e">E.</param>
		[UnconditionalSuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code", Justification = "<Pending>")]
		void CollectionChanged(string? propertyName, NotifyCollectionChangedEventArgs e)
		{
			if (e.NewItems != null && e.NewItems.Count > 0)
			{
				foreach (var newItem in e.NewItems)
				{
					AddNewItem(propertyName, newItem, e.NewStartingIndex);
				}
			}

			if (e.OldItems != null && e.OldItems.Count > 0)
			{
				foreach (var oldItem in e.OldItems)
				{
					RemoveOldItem(propertyName, oldItem);
				}
			}

			if (e.Action == NotifyCollectionChangedAction.Reset)
			{
				ClearLayoutChildren();
			}
		}

		void AddNewItem(string? propertyName, object newItem, int newIndex)
		{
			var chip = propertyName == SfChipGroup.ItemsProperty.PropertyName ? (newItem as SfChip) : GetNewChip(newItem);
			if (chip != null)
			{
				SubscribeChipEvents(chip);
				ChipLayout?.Children.Insert(newIndex, chip);
				_chipGroupChildren?.Add(chip);
				UpdateChipProperties(nameof(ChipStrokeThickness));
			}
		}

		void RemoveOldItem(string? propertyName, object oldItem)
		{
			var chip = propertyName == SfChipGroup.ItemsProperty.PropertyName ? (oldItem as SfChip) : GetChip(oldItem);
			if (chip != null)
			{
				RemoveChipFromLayout(chip);
			}
		}

		/// <summary>
		/// Executes the chip command.
		/// </summary>
		void SubscribeCommandEvents()
		{
			if (Command != null)
			{
				Command.CanExecuteChanged -= Command_CanExecuteChanged;
				Command.CanExecuteChanged += Command_CanExecuteChanged;
				if (ChipLayout != null && ChipLayout.Children.Count > 0)
				{
					foreach (var chip in ChipLayout.Children)
					{
						Command_CanExecuteChanged(chip as SfChip, EventArgs.Empty);
					}
				}
			}
		}

		/// <summary>
		/// Clears the layout children.
		/// </summary>
		void ClearLayoutChildren()
		{
			if (_chipGroupChildren != null && ChipLayout != null)
			{
				foreach (SfChip chip in _chipGroupChildren)
				{
					UnsubscribeChipEvents(chip);
					if (ChipLayout.Children.Contains(chip))
					{
						ChipLayout.Children.Remove(chip);
					}
				}

				_chipGroupChildren.Clear();
			}
		}

		/// <summary>
		/// Generate and add the chip to the Layout of ChipGroup.
		/// </summary>
		void CreateAndInitializeChips()
		{
			if (ItemsSource is IList items)
			{
				foreach (var item in items)
				{
					var chip = item as SfChip ?? GetNewChip(item);
					chip.IsDescriptionNotSetByUser = string.IsNullOrEmpty(SemanticProperties.GetDescription(this));
					AddChildToLayout(chip);
				}
			}
			else if (Items != null)
			{
				foreach (SfChip chip in Items)
				{
					AddChildToLayout(chip);
				}
			}

			if (ChipType == SfChipsType.Input && InputView != null && !IsEditorControl)
			{
				AddChildToLayout(InputView);
			}
		}

		/// <summary>
		/// Creates a new chip using given context.
		/// </summary>
		/// <param name="chipData">The context of the chip.</param>
		/// <returns>A chip with default customization.</returns>
		/// It represents single chip.
		/// +-0 or 18-+--0 or 24-+---------star----------+---0 or 18----+CORNERRADIUS
		/// |         |          |                       |              |         
		/// |         |          |      PADDING          |              |         
		/// |         |          |                       |              |        
		/// |---------|----------|-----------------------|--------------|  HEIGHT = Default button height (or) ItemHeight  
		/// |         |          |                       |              |          
		/// |INDICATOR|  ICON    |  CHIP TEXT            | CLOSEBUTTON  | 
		/// |         |          |                       |              |        
		/// |---------|----------|-----------------------|--------------+   
		/// |         |          |                       |              |           
		/// |         |          |      PADDING          |              |           
		/// +---------+----------+-----------------------+--------------+CORNERRADIUS   
		[UnconditionalSuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code", Justification = "<Pending>")]
		SfChip GetNewChip(object? chipData)
		{
			SfChip chip = InitializeChip();
			if (chipData != null)
			{
				chip.DataContext = chipData;
				if (ItemTemplate != null)
				{
					ConfigureChipWithTemplate(chip, chipData);
				}
				else
				{
					ConfigureChipWithoutTemplate(chip, chipData);
				}

				UpdateChipProperties(chip);
				ApplyChipColor(chip);
				SubscribeChipEvents(chip);
			}

			return chip;
		}

		SfChip InitializeChip()
		{
			SfChip chip = new SfChip(true);
			SetChipHeight(chip);
			chip.IsEditorControl = IsEditorControl;
			chip.Margin = ChipPadding;
			chip.VerticalOptions = LayoutOptions.Center;
			chip.FontSize = ChipTextSize;
			chip.FontFamily = ChipFontFamily;
			chip.FontAttributes = ChipFontAttributes;
			chip.CornerRadius = ChipCornerRadius;
			chip.Stroke = ChipStroke;
			return chip;
		}

		void ConfigureChipWithTemplate(SfChip chip, object chipData)
		{
			chip.IsItemTemplate = true;
			DataTemplate dataTemplate = ItemTemplate;

			if (ItemTemplate is DataTemplateSelector)
			{
				dataTemplate = (ItemTemplate as DataTemplateSelector).SelectDataTemplate(chipData, this);
			}

			var layout = dataTemplate.CreateContent();
#if NET10_0_OR_GREATER
			View? customView = layout as View;
#else
			View? customView = layout is ViewCell ? (layout as ViewCell)?.View : (View)layout;
#endif

			if (customView != null)
			{
				customView.BindingContext = chipData;
				if (customView is SfChip)
				{
					chip.SetBinding(SfChip.IsVisibleProperty, BindingHelper.CreateBinding(propertyName: "IsVisible", getter: static(View view) => view.IsVisible, source:customView));
				}

				chip.Add(customView);
			}
		}

		[RequiresUnreferencedCode("The ConfigureChipWithoutTemplate method is not trim compatible")] 
		void ConfigureChipWithoutTemplate(SfChip chip, object chipData)
		{
			chip.IsItemTemplate = false;
			if (!string.IsNullOrEmpty(DisplayMemberPath))
			{
				chip.Text = GetPropertyValue(DisplayMemberPath, chipData).ToString() ?? string.Empty;
			}
			else
			{
				chip.Text = chipData.ToString() ?? string.Empty;
			}

			chip.AutomationId = AutomationId + "_" + chip.Text;

			if (!string.IsNullOrEmpty(ImageMemberPath))
			{
				var imageSource = GetPropertyValue(ImageMemberPath, chipData);

				chip.ImageSource = imageSource is ImageSource ? (ImageSource)imageSource : (string)imageSource;
			}
		}


		/// <summary>
		/// Customization of chip based on the visual mode.
		/// </summary>
		/// <param name="chip">The Chip.</param>
		void ApplyChipColor(SfChip chip)
		{
			if (chip == null)
			{
				return;
			}

			switch (ChipType)
			{
				case SfChipsType.Choice:
				case SfChipsType.Filter:
					SetChoiceOrFilterChipColor(chip);
					break;
				default:
					SetDefaultChipColor(chip);
					break;
			}
		}

		void SetChoiceOrFilterChipColor(SfChip chip)
		{
			chip.TextColor = chip.IsSelected ? SelectedChipTextColor : ChipTextColor;
			chip.Background = chip.IsSelected ? ((SelectedChipBackground as SolidColorBrush)?.Color ?? Color.FromRgb(232, 222, 248)) : (ChipBackground as SolidColorBrush)?.Color;
			chip.SelectionIndicatorColor = SelectionIndicatorColor;
			chip.StrokeThickness = chip.IsSelected ? 0 : ChipStrokeThickness;
		}

		void SetDefaultChipColor(SfChip chip)
		{
			chip.TextColor = ChipTextColor;
			chip.Background = ChipBackground;
			chip.CloseButtonColor = CloseButtonColor;
		}

		/// <summary>
		/// Arranges the close and selection indicator in chip based on <see cref="ChipType"/> of chip group./>.
		/// </summary>
		/// <param name="chip">the chip.</param>
		void UpdateChipProperties(SfChip chip)
		{
			chip.ShowIcon = ShowIcon;
			chip.StrokeThickness = ChipStrokeThickness;
			chip.ImageSize = ChipImageSize;
			chip._filterType = false;

			switch (ChipType)
			{
				case SfChipsType.Input:
					ConfigureInputChip(chip);
					break;
				case SfChipsType.Choice:
					ConfigureChoiceChip(chip);
					break;
				case SfChipsType.Filter:
					ConfigureFilterChip(chip);
					break;
				case SfChipsType.Action:
					ConfigureActionChip(chip);
					break;
			}

			chip._chipType = ChipType;
			chip.InvalidateSemantics();
		}

		void ConfigureInputChip(SfChip chip)
		{
			chip.ShowCloseButton = !chip.IsItemTemplate;
			SubscribeChipEvents(chip);
			chip.ShowSelectionIndicator = false;
		}

		void ConfigureChoiceChip(SfChip chip)
		{
			chip.ShowCloseButton = false;
			chip.CloseButtonClicked -= Chip_CloseButtonClicked;
			chip.ShowSelectionIndicator = false;
		}

		void ConfigureFilterChip(SfChip chip)
		{
			chip.ShowCloseButton = false;
			chip._filterType = true;
			chip.CloseButtonClicked -= Chip_CloseButtonClicked;
		}

		void ConfigureActionChip(SfChip chip)
		{
			chip.ShowCloseButton = false;
			chip.CloseButtonClicked -= Chip_CloseButtonClicked;
			chip.ShowSelectionIndicator = false;
		}


		/// <summary>
		/// Updates the chip properties.
		/// </summary>
		/// <param name="propertyName">Property name.</param>
		void UpdateChipProperties(string propertyName)
		{
			if (ItemsSource != null)
			{
				if (_chipGroupChildren != null)
				{
					foreach (SfChip chip in _chipGroupChildren)
					{
						chip.IsEnabled = IsEnabled;
						switch (propertyName)
						{
							case "ShowIcon":
								chip.ShowIcon = ShowIcon;
								break;
							case "ChipPadding":
								chip.Margin = ChipPadding;
								break;
							case "ItemHeight":
								SetChipHeight(chip);
								break;
							case "ChipStroke":
								chip.Stroke = ChipStroke;
								break;
							case "ChipBackground":
								ApplyChipColor(chip);
								break;
							case "ChipTextColor":
								ApplyChipColor(chip);
								break;
							case "SelectedChipTextColor":
								ApplyChipColor(chip);
								break;
							case "SelectedChipBackground":
								ApplyChipColor(chip);
								break;
							case "SelectionIndicatorColor":
								chip.SelectionIndicatorColor = SelectionIndicatorColor;
								break;
							case "CloseButtonColor":
								chip.CloseButtonColor = CloseButtonColor;
								break;
							case "ChipTextSize":
								chip.FontSize = ChipTextSize;
								break;
							case "ChipFontFamily":
								chip.FontFamily = ChipFontFamily;
								break;
							case "ChipFontAttributes":
								chip.FontAttributes = ChipFontAttributes;
								break;
							case "ChipStrokeThickness":
								chip.StrokeThickness = chip.IsSelected ? 0 : ChipStrokeThickness;
								break;
							case "ChipImageSize":
								chip.ImageSize = ChipImageSize;
								break;
							case "ChipCornerRadius":
								chip.CornerRadius = ChipCornerRadius;
								break;
							default:
								break;
						}
					}
				}
			}
		}

		/// <summary>
		/// Add the child to layout.
		/// </summary>
		/// <param name="child">The child to be added.</param>
		void AddChildToLayout(View child)
		{
			if (child == null)
			{
				return;
			}

			if (ChipLayout != null)
			{
				if (ChipType == SfChipsType.Input && InputView != null)
				{
					HandleInputView(child);
				}
				else
				{
					ChipLayout.Children.Add(child);
				}
			}

			if (_chipGroupChildren != null && child is SfChip chip)
			{
				_chipGroupChildren.Add(chip);
			}
		}

		void HandleInputView(View child)
		{
			if (ChipLayout != null)
			{
				if (child == InputView)
				{
					if (ChipLayout.Contains(child))
					{
						ChipLayout.Children.Remove(child);
					}
					ChipLayout.Children.Add(child);
				}
				else
				{
					if (ChipLayout.Children.Contains(InputView))
					{
						ChipLayout.Children.Insert(ChipLayout.Children.IndexOf(InputView), child);
					}
					else
					{
						ChipLayout.Children.Add(child);
					}
				}
			}
		}

		/// <summary>
		/// Selects or unselects the chip based on type.
		/// </summary>
		/// <param name="item">The item to be selected.</param>
		void SelectOrUnselectChip(object? item)
		{
			SfChip? chip = item as SfChip;
			chip ??= GetChip(item);

			if (chip != null)
			{
				object? chipData = GetChipDataContext(chip);

				switch (ChipType)
				{
					case SfChipsType.Choice:
						HandleChoiceChipSelection(chip, chipData);
						break;
					case SfChipsType.Filter:
						HandleFilterChipSelection(chip, chipData);
						break;
				}

				_previouslySelectedChip = chip;
			}
		}

		void HandleChoiceChipSelection(SfChip chip, object? chipData)
		{
			if (_previouslySelectedChip != chip)
			{
				UnselectChoiceChip();
			}

			if (!chip.IsSelected)
			{
				SelectedItem = chipData;
				chip.IsSelected = true;
				ApplyChipColor(chip);
			}
			else
			{
				UnselectChoiceChip();
				SelectedItem = null;
			}
		}

		void HandleFilterChipSelection(SfChip chip, object? chipData)
		{
			if (!chip.IsSelected)
			{
				if (ItemTemplate != null)
				{
					chip.ShowSelectionIndicator = false;
					chip.IsSelected = true;
				}
				else
				{
					chip.ShowSelectionIndicator = chip.IsSelected = true;
				}

				ApplyChipColor(chip);

				SelectedItem ??= new ObservableCollection<object>();

				if (SelectedItem is IList selectedItem && !selectedItem.Contains(chipData))
				{
					selectedItem.Add(chipData);
				}
			}
			else
			{
				UnselectFilterChip(chip);
			}
		}

		void UnselectpreviousChip(SfChipsType chipsType)
		{
			if (chipsType == SfChipsType.Filter)
			{
				if (_chipGroupChildren != null)
				{
					foreach (var chip in _chipGroupChildren)
					{
						if (chip != null)
						{
							chip.ShowSelectionIndicator = chip.IsSelected = false;
							ApplyChipColor(chip);
							RemoveChipFromSelectedItems(chip);
						}
					}
				}
			}

			else if (chipsType == SfChipsType.Choice)
			{
				UnselectChoiceChip();
			}
		}

		/// <summary>
		/// Unselects the filter chip.
		/// </summary>
		/// <param name="chip">The Chip.</param>
		void UnselectFilterChip(SfChip chip)
		{
			if (ChipType == SfChipsType.Filter)
			{
				if (chip != null)
				{
					chip.ShowSelectionIndicator = chip.IsSelected = false;
					if (SelectedItem != null && ItemTemplate == null)
					{
						chip.ShowSelectionIndicator = chip.IsSelected = true;
					}
					ApplyChipColor(chip);
					RemoveChipFromSelectedItems(chip);
				}
			}
		}

		/// <summary>
		/// Unselects the choice chip.
		/// </summary>
		void UnselectChoiceChip()
		{
			if (_previouslySelectedChip != null && ChipLayout != null)
			{
				int index = ChipLayout.Children.IndexOf(_previouslySelectedChip);
				if (index >= 0 && ChipLayout.Children.Count > index)
				{
					_previouslySelectedChip.IsSelected = false;
					_previouslySelectedChip.StrokeThickness = ChipStrokeThickness;
					ApplyChipColor(_previouslySelectedChip);
				}
			}
		}

		/// <summary>
		/// Removes the selected chip from selected items.
		/// </summary>
		/// <param name="chip">The Chip.</param>
		void RemoveChipFromSelectedItems(SfChip chip)
		{
			object? chipData = GetChipDataContext(chip);
			if (SelectedItem is IList list && list.Count > 0)
			{
				if (list.Contains(chipData))
				{
					list.Remove(chipData);
				}
			}
		}

		/// <summary>
		/// Unsubscribes the chip events.
		/// </summary>
		/// <param name="chip">The Chip.</param>
		void UnsubscribeChipEvents(SfChip chip)
		{
			if (chip != null)
			{
				chip.Clicked -= Chip_Clicked;
				chip.CloseButtonClicked -= Chip_CloseButtonClicked;
			}
		}

		/// <summary>
		/// Subscribes the chip Clicked and CloseButtonClicked events.
		/// </summary>
		/// <param name="chip">Chip.</param>
		void SubscribeChipEvents(SfChip chip)
		{
			if (chip != null)
			{
				chip.Clicked -= Chip_Clicked;
				chip.Clicked += Chip_Clicked;
				chip.CloseButtonClicked -= Chip_CloseButtonClicked;
				if (chip.ShowCloseButton)
				{
					chip.CloseButtonClicked += Chip_CloseButtonClicked;
				}
			}
		}

		/// <summary>
		/// Selects or unselects the multiple chips based on type.
		/// </summary>
		/// <param name="list">The items to be selected.</param>
		void SelectOrUnselectMultipleChips(IList list)
		{
			if (list != null && list.Count > 0)
			{
				foreach (var item in list)
				{
					SelectOrUnselectChip(item);
				}
			}
		}

		/// <summary>
		/// Chips the group layout changed.
		/// </summary>
		/// <param name="oldValue">Old value.</param>
		/// <param name="newValue">New value.</param>
		void ChipGroupLayoutChanged(object oldValue, object newValue)
		{
			if (oldValue != null)
			{
				if (newValue == null)
				{
					Content = null;
				}
				else
				{
					SetContentView();
					UpdateChipGroupChildren(oldValue);
				}
			}

			if (newValue != null && oldValue == null)
			{
				ClearAndCreateChips(oldValue, true);
			}
		}

		void UpdateChipGroupChildren(object oldValue)
		{
			if (_chipGroupChildren != null && _chipGroupChildren.Count > 0)
			{
				if (oldValue is Layout oldLayout)
				{
					foreach (var chip in _chipGroupChildren)
					{
						if (oldLayout.Children.Contains(chip))
						{
							oldLayout.Children.Remove(chip);
						}
						ChipLayout?.Children.Add(chip);
					}
				}

				if (ChipType == SfChipsType.Input && InputView != null)
				{
					ChipLayout?.Children.Add(InputView);
				}
			}
		}


		/// <summary>
		/// The raise clicked event for chip.
		/// </summary>
		/// <param name="chip"> The Event Argument as Chip. </param>
		void HandleClicked(SfChip chip)
		{
			if (chip != null)
			{
				if (ChipType == SfChipsType.Action)
				{
					if (Command != null && chip.IsEnabled)
					{
						Command.Execute(GetChipDataContext(chip));
					}
				}

				ChipClicked?.Invoke(chip, EventArgs.Empty);
			}
		}

		/// <summary>
		/// Initializes and create chip group.
		/// </summary>
		/// <param name="oldValue">Old layout.</param>
		/// <param name="isLayoutChanged">If set to <c>true</c> is layout changed.</param>
		[UnconditionalSuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code", Justification = "<Pending>")]
		void ClearAndCreateChips(object? oldValue, bool isLayoutChanged)
		{
			if (oldValue != null || (_chipGroupChildren != null && _chipGroupChildren.Count > 0))
			{
				ClearLayoutChildren();
			}

			if (isLayoutChanged)
			{
				SetContentView();
			}

			CreateAndInitializeChips();
			if (ChipType == SfChipsType.Choice)
			{
				_previouslySelectedChip = GetChip(SelectedItem);
				if (SelectedItem != null)
				{
					SelectOrUnselectChip(SelectedItem);
				}
			}
			else if (ChipType == SfChipsType.Filter)
			{
				if (SelectedItem is IList list)
				{
					SelectOrUnselectMultipleChips(list);
				}
			}

			ChangeVisualState();
		}

		/// <summary>
		/// Initializes the collection.
		/// </summary>
		void InitializeCollection()
		{
			Items = new ChipCollection(this);
			ChipLayout = new StackLayout() { Orientation = StackOrientation.Horizontal, Spacing = 4 };
		}

		/// <summary>
		/// Render the chip using its data context.
		/// </summary>
		/// <returns>The chip data context.</returns>
		/// <param name="dataContext">The Chip.</param>
		SfChip? GetChip(object? dataContext)
		{
			if (_chipGroupChildren != null)
			{
				foreach (SfChip chip in _chipGroupChildren)
				{
					if ((chip.IsCreatedInternally && chip.DataContext != null && chip.DataContext.Equals(dataContext)) || (dataContext == chip))
					{
						return chip;
					}
				}
			}

			return null;
		}

		/// <summary>
		/// Sets the layout as content.
		/// </summary>
		void SetContentView()
		{
			if (ChipLayout != null)
			{
				Content = ChipLayout;
			}
		}

		/// <summary>
		/// Sets the height of the chip only for Items property.
		/// </summary>
		/// <param name="chip">The Chip.</param>
		void SetChipHeight(SfChip chip)
		{
			if (!double.IsNaN(ItemHeight) && chip != null && chip.IsCreatedInternally)
			{
				chip.HeightRequest = ItemHeight;
				chip.InvalidateDrawable();
			}
		}

		/// <summary>
		/// Invoked when the chip is tapped.
		/// </summary>
		/// <param name="sender">The original sender of the event.</param>
		/// <param name="e">The event arguments.</param>
		void Chip_Clicked(object? sender, EventArgs e)
		{
			if (sender is SfChip chip)
			{
				if (ChipType == SfChipsType.Filter || ChipType == SfChipsType.Choice)
				{
					object? addValue = null;
					object? removeValue = null;
					object? chipData = GetChipDataContext(chip);

					if (ChipType == SfChipsType.Choice)
					{
						if (chipData == SelectedItem)
						{
							if (ChoiceMode == ChoiceMode.Single)
							{
								HandleClicked(chip);
								return;
							}
							else
							{
								removeValue = SelectedItem;
							}
						}
						else
						{
							addValue = chipData;
							removeValue = SelectedItem;
						}
					}
					else if (ChipType == SfChipsType.Filter)
					{
						if (SelectedItem is IList selectedItem)
						{
							if (selectedItem != null && selectedItem.Contains(chipData))
							{
								removeValue = chipData;
							}
							else
							{
								addValue = chipData;
							}
						}
					}

					SelectionChangingEvent eventArgs = new SelectionChangingEvent() { AddedItem = addValue, RemovedItem = removeValue };
					RaiseSelectionChangingEvent(chip, eventArgs);

					if (eventArgs.Cancel)
					{
						HandleClicked(chip);
						return;
					}

					if (ChipType == SfChipsType.Choice)
					{
						if (_previouslySelectedChip != null)
						{
							VisualStateManager.GoToState(this, "Normal");
							_previouslySelectedChip.TextColor = ChipTextColor;
							_previouslySelectedChip.Background = ChipBackground;
						}

					}

					_previousSelectedItem = _previouslySelectedChip;
					SelectOrUnselectChip(chip);
					ApplyChipColor(chip);
					VisualStateManager.GoToState(this, "Selected");
					_previousSelectedItem = null;
				}

				HandleClicked(chip);
			}
		}

		void UpdatePreviouslySelectedChip()
		{
			if (_previouslySelectedChip != null)
			{
				VisualStateManager.GoToState(this, "Normal");
				_previouslySelectedChip.TextColor = ChipTextColor;
				_previouslySelectedChip.Background = ChipBackground;
			}
		}

		/// <summary>
		/// Invoked when the chip close button is tapped.
		/// </summary>
		/// <param name="sender">The original sender of the event.</param>
		/// <param name="e">The event arguments.</param>
		void Chip_CloseButtonClicked(object? sender, EventArgs e)
		{
			if (sender is SfChip chip)
			{
				RemoveChipFromLayout(chip);
				object? item = chip.IsCreatedInternally
					? ItemsSource?.Contains(chip.DataContext) == true ? chip.DataContext : null
					: Items?.Contains(chip) == true ? chip : null;

				if (item != null)
				{
					ItemsSource?.Remove(chip.DataContext);
					Items?.Remove(chip);
					if (ChipType == SfChipsType.Input)
					{
						ItemRemoved?.Invoke(this, new SelectionChangedEvent { RemovedItem = item });
					}
				}
			}
		}

		/// <summary>
		/// Handles the ItemsSource collection changed.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="eventArgs">Event arguments.</param>
		void ItemsSourceCollectionChanged(object? sender, NotifyCollectionChangedEventArgs eventArgs)
		{
			CollectionChanged(SfChipGroup.ItemsSourceProperty.PropertyName, eventArgs);
		}

		/// <summary>
		/// Call the Commands can execute changed method.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">Event args.</param>
		void Command_CanExecuteChanged(object? sender, EventArgs e)
		{
			if (Command != null && sender is SfChip chip)
			{
				chip.IsEnabled = Command.CanExecute(GetChipDataContext(chip));
			}
		}


		/// <summary>
		/// Called when selection changed in SfChipGroup.
		/// </summary>
		/// <param name="sender">The original sender of the event.</param>
		/// <param name="args">Event arguments.</param>
		void RaiseSelectionChangedEvent(object sender, SelectionChangedEvent args)
		{
			if (sender is SfChipGroup chip)
			{
				SelectionChanged?.Invoke(this, args);
			}
		}

		/// <summary>
		/// Called when selection changing in sfchip group.
		/// </summary>
		/// <param name="sender">The original sender of the event.</param>
		/// <param name="args">Event arguments.</param>
		void RaiseSelectionChangingEvent(object sender, SelectionChangingEvent args)
		{
			if (sender is SfChip chip)
			{
				SelectionChanging?.Invoke(this, args);
			}
		}

		static void UpdateChipGroupChildren(SfChipGroup chipGroup)
		{
			if (chipGroup._chipGroupChildren != null)
			{
				foreach (SfChip chip in chipGroup._chipGroupChildren)
				{
					chipGroup.UpdateChipProperties(chip);
				}
			}
		}

		static void UpdateInputView(SfChipGroup chipGroup, object oldValue, object newValue)
		{
			if (chipGroup.InputView != null && chipGroup.ChipLayout != null)
			{
				if ((SfChipsType)oldValue == SfChipsType.Input)
				{
					if (chipGroup.ChipLayout.Children.Contains(chipGroup.InputView))
					{
						chipGroup.ChipLayout.Children.Remove(chipGroup.InputView);
					}
				}

				if ((SfChipsType)newValue == SfChipsType.Input)
				{
					chipGroup.ChipLayout.Children.Add(chipGroup.InputView);
				}
			}
		}

		static void HandleChipTypeChange(SfChipGroup chipGroup, object oldValue)
		{
			if (chipGroup.ChipType == SfChipsType.Action || chipGroup.ChipType == SfChipsType.Input)
			{
				chipGroup.UnselectpreviousChip((SfChipsType)oldValue);
			}

			if (chipGroup.ChipType == SfChipsType.Choice)
			{
				if (chipGroup.SelectedItem != null)
				{
					if (chipGroup.SelectedItem is IList selectedItem)
					{
						chipGroup.SelectOrUnselectMultipleChips(selectedItem);
					}

					chipGroup.SelectOrUnselectChip(chipGroup.SelectedItem);
				}
			}

			if (chipGroup.ChipType == SfChipsType.Filter)
			{
				if (chipGroup.SelectedItem != null)
				{
					chipGroup.SelectOrUnselectChip(chipGroup.SelectedItem);

					if (chipGroup.SelectedItem is IList selectedItem)
					{
						chipGroup.SelectOrUnselectMultipleChips(selectedItem);
					}
				}
			}
		}

#endregion

		#region Property Changed Methods

		/// <summary>
		/// Command property has been changed.
		/// </summary>
		/// <param name="bindableObject">The original source of property changed event.</param>
		/// <param name="oldValue">The old value of Command property.</param>
		/// <param name="newValue">The new value of Command property.</param>
		static void OnCommandChanged(BindableObject bindableObject, object oldValue, object newValue)
		{
			if (bindableObject is SfChipGroup chipGroup && oldValue != null)
			{
				if (oldValue is ICommand command)
				{
					command.CanExecuteChanged -= chipGroup.Command_CanExecuteChanged;
				}

				chipGroup.SubscribeCommandEvents();
			}
		}
		/// <summary>
		/// Updates the background color of the selected chip in the group.
		/// </summary>
		/// <param name="bindableObject">The original source of property changed event.</param>
		/// <param name="oldValue">The old value of SelectedChipBackground property.</param>
		/// <param name="newValue">The new value of SelectedChipBackground property.</param>        
		static void OnSelectedChipBackgroundPropertyChanged(BindableObject bindableObject, object oldValue, object newValue)
		{
			if (bindableObject is SfChipGroup chipGroup)
			{
				chipGroup.UpdateChipProperties(nameof(chipGroup.SelectedChipBackground));
			}
		}

		/// <summary>
		/// Updates the text color of the selected chip in the group.
		/// </summary>
		/// <param name="bindableObject">The original source of property changed event.</param>
		/// <param name="oldValue">The old value of SelectedChipTextColor property.</param>
		/// <param name="newValue">The new value of SelectedChipTextColor property.</param>
		static void OnSelectedChipTextColorPropertyChanged(BindableObject bindableObject, object oldValue, object newValue)
		{
			if (bindableObject is SfChipGroup chipGroup)
			{
				chipGroup.UpdateChipProperties(nameof(chipGroup.SelectedChipTextColor));
			}
		}
		/// <summary>
		/// Updates the selected chip in the group.
		/// </summary>
		/// <param name="bindableObject">The original source of property changed event.</param>
		/// <param name="oldValue">The old value of SelectedItem property.</param>
		/// <param name="newValue">The new value of SelectedItem property.</param>
		static void OnSelectedItemPropertyChanged(BindableObject bindableObject, object oldValue, object newValue)
		{
			if (bindableObject is SfChipGroup chipGroup)
			{
				if (chipGroup.ChipType == SfChipsType.Choice)
				{
					if (newValue != null)
					{
						chipGroup.SelectOrUnselectChip(newValue);
						chipGroup.RaiseSelectionChangedEvent(
							chipGroup,
							new SelectionChangedEvent() { AddedItem = newValue, RemovedItem = oldValue });
					}
					else if (newValue == null && oldValue != null)
					{
						chipGroup.UnselectChoiceChip();
						chipGroup.RaiseSelectionChangedEvent(
							chipGroup,
							new SelectionChangedEvent() { AddedItem = null, RemovedItem = chipGroup._previouslySelectedChip });
						chipGroup._previouslySelectedChip = null;
					}
				}
				else if (chipGroup.ChipType == SfChipsType.Filter)
				{
					chipGroup.OnSelectedItemsPropertyChanged(oldValue, newValue);
				}
			}
		}

		/// <summary>
		/// Creates chips for the given items and adds it in layout.
		/// </summary>
		/// <param name="bindableObject">The original source of property changed event.</param>
		/// <param name="oldValue">The old value of ItemsSource property.</param>
		/// <param name="newValue">The new value of ItemsSource property.</param>
		static void OnItemsPropertyChanged(BindableObject bindableObject, object oldValue, object newValue)
		{
			if (bindableObject is SfChipGroup chipGroup)
			{
				chipGroup.ChipGroupCollectionChanged(oldValue, newValue, SfChipGroup.ItemsProperty.PropertyName);
			}
		}

		static void OnMemberPathChanged(BindableObject bindableObject, object oldValue, object newValue)
		{
			if (bindableObject is SfChipGroup chipGroup)
			{
				chipGroup.ClearAndCreateChips(oldValue, false);
			}
		}

		/// <summary>
		/// Creates chips for the given items source and adds it in layout.
		/// </summary>
		/// <param name="bindableObject">The original source of property changed event.</param>
		/// <param name="oldValue">The old value of ItemsSource property.</param>
		/// <param name="newValue">The new value of ItemsSource property.</param>
		static void OnItemsSourcePropertyChanged(BindableObject bindableObject, object oldValue, object newValue)
		{
			if (bindableObject is SfChipGroup chipGroup)
			{
				chipGroup.ChipGroupCollectionChanged(oldValue, newValue, SfChipGroup.ItemsSourceProperty.PropertyName);
			}
		}

		/// <summary>
		/// Removes the item from current layout and adds it in new layout.
		/// </summary>
		/// <param name="bindableObject">The original source of property changed event.</param>
		/// <param name="oldValue">The old value of Layout property.</param>
		/// <param name="newValue">The new value of Layout property.</param>
		static void OnLayoutPropertyChanged(BindableObject bindableObject, object oldValue, object newValue)
		{
			if (bindableObject is SfChipGroup chipGroup)
			{
				chipGroup.ChipGroupLayoutChanged(oldValue, newValue);
			}
		}

		/// <summary>
		/// Updates the font attributes property for all chips in group.
		/// </summary>
		/// <param name="bindableObject">The original source of property changed event.</param>
		/// <param name="oldValue">The old value of ChipFontAttributes property.</param>
		/// <param name="newValue">The new value of ChipFontAttributes property.</param>
		static void OnChipFontAttributesPropertyChanged(BindableObject bindableObject, object oldValue, object newValue)
		{
			if (bindableObject is SfChipGroup chipGroup)
			{
				chipGroup.UpdateChipProperties(nameof(chipGroup.ChipFontAttributes));
			}
		}

		/// <summary>
		/// Updates the height of item in chip group.
		/// </summary>
		/// <param name="bindableObject">The original source of property changed event.</param>
		/// <param name="oldValue">The old value of item height property.</param>
		/// <param name="newValue">The new value of item height property.</param>
		static void OnItemHeightPropertyChanged(BindableObject bindableObject, object oldValue, object newValue)
		{
			if (bindableObject is SfChipGroup chipGroup)
			{
				chipGroup.UpdateChipProperties(nameof(chipGroup.ItemHeight));
			}
		}

		/// <summary>
		/// Updates the padding of chip item in group.
		/// </summary>
		/// <param name="bindableObject">The original source of property changed event.</param>
		/// <param name="oldValue">The old value of ChipPadding property.</param>
		/// <param name="newValue">The new value of ChipPadding property.</param>
		static void OnChipPaddingPropertyChanged(BindableObject bindableObject, object oldValue, object newValue)
		{
			if (bindableObject is SfChipGroup chipGroup)
			{
				chipGroup.UpdateChipProperties(nameof(chipGroup.ChipPadding));
			}
		}

		/// <summary>
		/// Updates the border width of of chip item in group.
		/// </summary>
		/// <param name="bindableObject">The original source of property changed event.</param>
		/// <param name="oldValue">The old value of ChipPadding property.</param>
		/// <param name="newValue">The new value of ChipPadding property.</param>
		static void OnChipStrokeThicknessPropertyChanged(BindableObject bindableObject, object oldValue, object newValue)
		{
			if (bindableObject is SfChipGroup chipGroup)
			{
				chipGroup.UpdateChipProperties(nameof(chipGroup.ChipStrokeThickness));
			}
		}

		/// <summary>
		/// Updates the visibility of icons in chip.
		/// </summary>
		/// <param name="bindableObject">The original source of property changed event.</param>
		/// <param name="oldValue">The old value of ShowIcon property.</param>
		/// <param name="newValue">The new value of ShowIcon property.</param>
		static void OnShowIconPropertyChanged(BindableObject bindableObject, object oldValue, object newValue)
		{
			if (bindableObject is SfChipGroup chipGroup)
			{
				chipGroup.UpdateChipProperties(nameof(chipGroup.ShowIcon));
			}
		}

		/// <summary>
		/// Updates the background color of chip in group.
		/// </summary>
		/// <param name="bindableObject">The original source of property changed event.</param>
		/// <param name="oldValue">The old value of ChipBackgroundColor property.</param>
		/// <param name="newValue">The new value of ChipBackgroundColor property.</param>
		static void OnChipBackgroundPropertyChanged(BindableObject bindableObject, object oldValue, object newValue)
		{
			if (bindableObject is SfChipGroup chipGroup)
			{
				chipGroup.UpdateChipProperties(nameof(chipGroup.ChipBackground));
			}
		}

		/// <summary>
		/// Updates the text color property for all chips in group.
		/// </summary>
		/// <param name="bindableObject">The original source of property changed event.</param>
		/// <param name="oldValue">The old value of ChipTextColor property.</param>
		/// <param name="newValue">The new value of ChipTextColor property.</param>
		static void OnChipTextColorPropertyChanged(BindableObject bindableObject, object oldValue, object newValue)
		{
			if (bindableObject is SfChipGroup chipGroup)
			{
				chipGroup.UpdateChipProperties(nameof(chipGroup.ChipTextColor));
			}
		}

		/// <summary>
		/// Updates the font size property for all chips in group.
		/// </summary>
		/// <param name="bindableObject">The original source of property changed event.</param>
		/// <param name="oldValue">The old value of ChipTextSize property.</param>
		/// <param name="newValue">The new value of ChipTextSize property.</param>
		static void OnChipTextSizePropertyChanged(BindableObject bindableObject, object oldValue, object newValue)
		{
			if (bindableObject is SfChipGroup chipGroup)
			{
				chipGroup.UpdateChipProperties(nameof(chipGroup.ChipTextSize));
			}
		}

		static void OnChipFontFamilyPropertyChanged(BindableObject bindableObject, object oldValue, object newValue)
		{
			if (bindableObject is SfChipGroup chipGroup)
			{
				chipGroup.UpdateChipProperties(nameof(chipGroup.ChipFontFamily));
			}
		}

		/// <summary>
		/// Property changed method for corner radius property.
		/// </summary>
		/// <param name="bindableObject">The original source of property changed event.</param>
		/// <param name="oldValue">The old value of CornerRadius property.</param>
		/// <param name="newValue">The new value of CornerRadius property </param>
		static void OnCornerRadiusPropertyChanged(BindableObject bindableObject, object oldValue, object newValue)
		{
			if (bindableObject is SfChipGroup chipGroup)
			{
				chipGroup.UpdateChipProperties(nameof(chipGroup.ChipCornerRadius));
			}
		}

		/// <summary>
		/// Updates the border color property for all chips in group.
		/// </summary>
		/// <param name="bindableObject">The original source of property changed event.</param>
		/// <param name="oldValue">The old value of ChipStroke property.</param>
		/// <param name="newValue">The new value of ChipStroke property.</param>
		static void OnChipStrokePropertyChanged(BindableObject bindableObject, object oldValue, object newValue)
		{
			if (bindableObject is SfChipGroup chipGroup)
			{
				chipGroup.UpdateChipProperties(nameof(chipGroup.ChipStroke));
			}
		}

		/// <summary>
		/// Updates the close button color property for all chips in group.
		/// </summary>
		/// <param name="bindableObject">The original source of property changed event.</param>
		/// <param name="oldValue">The old value of CloseButtonColor property.</param>
		/// <param name="newValue">The new value of CloseButtonColor property.</param>
		static void OnCloseButtonColorPropertyChanged(BindableObject bindableObject, object oldValue, object newValue)
		{
			if (bindableObject is SfChipGroup chipGroup)
			{
				chipGroup.UpdateChipProperties(nameof(chipGroup.CloseButtonColor));
			}
		}

		/// <summary>
		/// Updates the selection indicator color property for all chips in group.
		/// </summary>
		/// <param name="bindableObject">The original source of property changed event.</param>
		/// <param name="oldValue">The old value of SelectionIndicatorColor property.</param>
		/// <param name="newValue">The new value of SelectionIndicatorColor property.</param>
		static void OnSelectionIndicatorColorPropertyChanged(BindableObject bindableObject, object oldValue, object newValue)
		{
			if (bindableObject is SfChipGroup chipGroup)
			{
				chipGroup.UpdateChipProperties(nameof(chipGroup.SelectionIndicatorColor));
			}
		}

		/// <summary>
		/// Updates the chip image width property changed.
		/// </summary>
		/// <param name="bindableObject">The original source of property changed event.</param>
		/// <param name="oldValue">The old value of SelectionIndicatorColor property.</param>
		/// <param name="newValue">The new value of SelectionIndicatorColor property.</param>
		static void OnChipImageSizePropertyChanged(BindableObject bindableObject, object oldValue, object newValue)
		{
			if (bindableObject is SfChipGroup chipGroup)
			{
				chipGroup.UpdateChipProperties(nameof(chipGroup.ChipImageSize));
			}
		}

		static void OnInputViewPropertyChanged(BindableObject bindableObject, object oldValue, object newValue)
		{
			if (bindableObject is SfChipGroup chipGroup)
			{
				chipGroup.UpdateInputView((View)oldValue, (View)newValue);
			}
		}

		/// <summary>
		/// Configures the chip properties based on its type.
		/// </summary>
		/// <param name="bindableObject">The original source of property changed event.</param>
		/// <param name="oldValue">The old value of Type property.</param>
		/// <param name="newValue">The new value of Type property.</param>
		static void OnChipTypePropertyChanged(BindableObject bindableObject, object oldValue, object newValue)
		{
			if (bindableObject is SfChipGroup chipGroup)
			{
				if (oldValue != newValue)
				{
					UpdateChipGroupChildren(chipGroup);
					UpdateInputView(chipGroup, oldValue, newValue);
					HandleChipTypeChange(chipGroup, oldValue);
				}
			}
		}

		#endregion

		#region Interface Implementation

		ResourceDictionary IParentThemeElement.GetThemeDictionary()
		{
			return new SfChipGroupStyles();
		}

		void IThemeElement.OnControlThemeChanged(string oldTheme, string newTheme)
		{

		}

		void IThemeElement.OnCommonThemeChanged(string oldTheme, string newTheme)
		{

		}

		#endregion

		#region Events

		/// <summary>
		/// Raised when the item in SfChips control is tapped.
		/// </summary>
		/// <example>
		/// Here is an example of how to register the <see cref="ChipClicked"/> event.
		/// 
		/// # [C#](#tab/tabid-1)
		/// <code><![CDATA[
		/// SfChipGroup chipGroup = new SfChipGroup();
		/// chipGroup.ChipClicked += chipgroup_ChipClicked;
		/// 
		/// private void chipgroup_ChipClicked(object sender, EventArgs e)
		///	{
		///	}
		/// ]]></code>
		/// </example>
		public event EventHandler<EventArgs>? ChipClicked;

		/// <summary>
		/// Occurs when the user selects an item from unselected items.
		/// </summary>
		/// <example>
		/// Here is an example of how to register the <see cref="SelectionChanging"/> event.
		/// 
		/// # [C#](#tab/tabid-1)
		/// <code><![CDATA[
		/// SfChipGroup chipGroup = new SfChipGroup();
		/// chipGroup.SelectionChanging += Chipgroup_SelectionChanging;
		/// 
		/// private void Chipgroup_SelectionChanging(object? sender, Toolkit.Chips.SelectionChangingEventArgs e)
		/// {
		/// }
		/// ]]></code>
		/// </example>

		public event EventHandler<SelectionChangingEvent>? SelectionChanging;

		/// <summary>
		/// Occurs when the user selects an item from unselected items. 
		/// </summary>
		/// <example>
		/// Here is an example of how to register the <see cref="SelectionChanged"/> event.
		/// 
		/// # [C#](#tab/tabid-1)
		/// <code><![CDATA[
		/// SfChipGroup chipGroup = new SfChipGroup();
		/// chipGroup.SelectionChanged += Chipgroup_SelectionChanged;
		/// 
		/// private void Chipgroup_SelectionChanged(object? sender, Toolkit.Chips.SelectionChangedEventArgs e)
		/// {
		/// }
		/// ]]></code>
		/// </example>
		public event EventHandler<SelectionChangedEvent>? SelectionChanged;

		/// <summary>
		/// Occurs when the user clicked the close button in <see cref="SfChipGroup"/>.
		/// </summary>
		/// <remarks>
		/// This event support for <see cref="SfChipsType.Input"/> type <see cref="SfChipGroup"/> only.
		/// </remarks>
		/// <example>
		/// Here is an example of how to register the <see cref="ItemRemoved"/> event.
		/// 
		/// # [C#](#tab/tabid-1)
		/// <code><![CDATA[
		/// SfChipGroup chipGroup = new SfChipGroup();
		/// chipGroup.ItemRemoved += Chipgroup_ItemRemoved;
		/// 
		/// private void Chipgroup_ItemRemoved(object? sender, Toolkit.Chips.SelectionChangedEventArgs e)
		/// {
		/// }
		///	]]></code>
		/// </example>
		public event EventHandler<SelectionChangedEvent>? ItemRemoved;

		#endregion
	}
}
