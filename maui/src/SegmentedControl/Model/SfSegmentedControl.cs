using System.Collections.Specialized;
using System.Collections;

namespace Syncfusion.Maui.Toolkit.SegmentedControl
{
	/// <summary>
	/// The SfSegmentedControl that allows you to display a set of segments, typically used for switch among different views. Each segment in the control represents an item that the user can select. 
	/// The control can be populated with a collection of <see cref="SfSegmentItem"/> objects and a collection of strings.
	/// The SfSegmentedControl provides various customization options such as segment height, width, selection indicator settings, text and icon styles, and more.
	/// It supports setting the currently selected segment using the SelectedIndex property and provides an option to enable auto-scrolling when the selected index changes.
	/// You can also customize the appearance of individual segments using the SegmentTemplate.
	/// </summary>
	/// <example>
	/// <para>
	/// The following code demonstrates, how to bind <c>ItemsSource</c> and populate segment items in the <see cref="SfSegmentedControl"/>.
	/// </para>
	/// # [C#](#tab/tabid-1)
	/// <code><![CDATA[
	/// public class SegmentViewModel
	/// {
	///    public List<SfSegmentItem> Employees { get; set; }
	///    public SegmentViewModel()
	///    {
	///        Employees = new List<SfSegmentItem>
	///        {
	///           new SfSegmentItem() {  ImageSource="jackson.png", Text="Jackson" },
	///           new SfSegmentItem() { ImageSource ="gabriella.png" ,Text="Gabriella"},
	///           new SfSegmentItem() { ImageSource="liam.png", Text="Liam"},
	///        };
	///    }
	/// }
	/// ]]></code>
	/// # [XAML](#tab/tabid-2)
	/// <code><![CDATA[
	/// <button:SfSegmentedControl x:Name="segmentedControl"
	///                            ItemsSource="{Binding Employees}">
	///    <button:SfSegmentedControl.BindingContext>
	///       <local:SegmentViewModel/>
	///    </button:SfSegmentedControl.BindingContext>
	/// </button:SfSegmentedControl>
	/// ]]></code>
	/// </example>
	public partial class SfSegmentedControl
	{
		#region Bindable properties

		/// <summary>
		/// Identifies the <see cref="ItemsSource"/> dependency property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="ItemsSource"/> dependency property.
		/// </value>
		public static readonly BindableProperty ItemsSourceProperty =
			BindableProperty.Create(
				nameof(ItemsSource),
				typeof(object),
				typeof(SfSegmentedControl),
				defaultValueCreator: bindable => null,
				propertyChanged: OnItemsSourceChanged);

		/// <summary>
		/// Identifies the <see cref="SelectedIndex"/> dependency property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="SelectedIndex"/> dependency property.
		/// </value>
		public static readonly BindableProperty SelectedIndexProperty =
			BindableProperty.Create(
				nameof(SelectedIndex),
				typeof(int?),
				typeof(SfSegmentedControl),
				defaultValueCreator: bindable => null,
				propertyChanged: OnSelectedIndexPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="SegmentHeight"/> dependency property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="SegmentHeight"/> dependency property.
		/// </value>
		public static readonly BindableProperty SegmentHeightProperty =
			BindableProperty.Create(
				nameof(SegmentHeight),
				typeof(double),
				typeof(SfSegmentedControl),
				defaultValueCreator: bindable => 38d);

		/// <summary>
		/// Identifies the <see cref="SegmentWidth"/> dependency property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="SegmentWidth"/> dependency property.
		/// </value>
		public static readonly BindableProperty SegmentWidthProperty =
			BindableProperty.Create(
				nameof(SegmentWidth),
				typeof(double),
				typeof(SfSegmentedControl),
				defaultValueCreator: bindable => 100d);

		/// <summary>
		/// Identifies the <see cref="VisibleSegmentsCount"/> dependency property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="VisibleSegmentsCount"/> dependency property.
		/// </value>
		public static readonly BindableProperty VisibleSegmentsCountProperty =
			BindableProperty.Create(
				nameof(VisibleSegmentsCount),
				typeof(int),
				typeof(SfSegmentedControl),
				defaultValueCreator: bindable => -1,
				propertyChanged: OnVisibleSegmentsCountChanged);

		/// <summary>
		/// Identifies the <see cref="SelectionIndicatorSettings"/> dependency property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="SelectionIndicatorSettings"/> dependency property.
		/// </value>
		public static readonly BindableProperty SelectionIndicatorSettingsProperty =
			BindableProperty.Create(
				nameof(SelectionIndicatorSettings),
				typeof(SelectionIndicatorSettings),
				typeof(SfSegmentedControl),
				defaultValueCreator: bindable => new SelectionIndicatorSettings(),
				propertyChanged: OnSelectionSettingsChanged);

		/// <summary>
		/// Identifies the <see cref="DisabledSegmentTextColor"/> dependency property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="DisabledSegmentTextColor"/> dependency property.
		/// </value>
		public static readonly BindableProperty DisabledSegmentTextColorProperty =
			BindableProperty.Create(
				nameof(DisabledSegmentTextColor),
				typeof(Color),
				typeof(SfSegmentedControl),
				defaultValueCreator: bindable => Color.FromArgb("#1C1B1F61"),
				propertyChanged: OnDisabledSegmentTextColorChanged);

		/// <summary>
		/// Identifies the <see cref="DisabledSegmentBackground"/> dependency property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="DisabledSegmentBackground"/> dependency property.
		/// </value>
		public static readonly BindableProperty DisabledSegmentBackgroundProperty =
			BindableProperty.Create(
				nameof(DisabledSegmentBackground),
				typeof(Brush),
				typeof(SfSegmentedControl),
				defaultValueCreator: bindable => Brush.Transparent,
				propertyChanged: OnDisabledSegmentBackgroundChanged);

		/// <summary>
		/// Identifies the <see cref="TextStyle"/> dependency property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="TextStyle"/> dependency property.
		/// </value>
		public static readonly BindableProperty TextStyleProperty =
			BindableProperty.Create(
				nameof(TextStyle),
				typeof(SegmentTextStyle),
				typeof(SfSegmentedControl),
				defaultValueCreator: bindable => new SegmentTextStyle(),
				propertyChanged: OnTextStyleChanged);

		/// <summary>
		/// Identifies the <see cref="SegmentBackground"/> dependency property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="SegmentBackground"/> dependency property.
		/// </value>
		public static readonly BindableProperty SegmentBackgroundProperty =
			BindableProperty.Create(
				nameof(SegmentBackground),
				typeof(Brush),
				typeof(SfSegmentedControl),
				defaultValueCreator: bindable => Brush.Transparent,
				propertyChanged: OnSegmentBackgroundChanged);

		/// <summary>
		/// Identifies the <see cref="Stroke"/> dependency property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="Stroke"/> dependency property.
		/// </value>
		public static readonly BindableProperty StrokeProperty =
			BindableProperty.Create(
				nameof(Stroke),
				typeof(Brush),
				typeof(SfSegmentedControl),
				defaultValueCreator: bindable => new SolidColorBrush(Color.FromArgb("#79747E")),
				propertyChanged: OnStrokeChanged);

		/// <summary>
		/// Identifies the <see cref="StrokeThickness"/> dependency property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="StrokeThickness"/> dependency property.
		/// </value>
		public static readonly BindableProperty StrokeThicknessProperty =
			BindableProperty.Create(
				nameof(StrokeThickness),
				typeof(double),
				typeof(SfSegmentedControl),
				defaultValueCreator: bindable => 1d,
				propertyChanged: OnStrokeThicknessChanged);

		/// <summary>
		/// Identifies the <see cref="CornerRadius"/> dependency property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="CornerRadius"/> dependency property.
		/// </value>
		public static readonly BindableProperty CornerRadiusProperty =
			BindableProperty.Create(
				nameof(CornerRadius),
				typeof(CornerRadius),
				typeof(SfSegmentedControl),
				defaultValueCreator: bindable => new CornerRadius(20),
				propertyChanged: OnCornerRadiusChanged);

		/// <summary>
		/// Identifies the <see cref="SegmentCornerRadius"/> dependency property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="SegmentCornerRadius"/> dependency property.
		/// </value>
		public static readonly BindableProperty SegmentCornerRadiusProperty =
			BindableProperty.Create(
				nameof(SegmentCornerRadius),
				typeof(CornerRadius),
				typeof(SfSegmentedControl),
				defaultValueCreator: bindable => new CornerRadius(0),
				propertyChanged: OnSegmentCornerRadiusChanged);

		/// <summary>
		/// Identifies the <see cref="AutoScrollToSelectedSegment"/> dependency property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="AutoScrollToSelectedSegment"/> dependency property.
		/// </value>
		public static readonly BindableProperty AutoScrollToSelectedSegmentProperty =
			BindableProperty.Create(
				nameof(AutoScrollToSelectedSegment),
				typeof(bool),
				typeof(SfSegmentedControl),
				defaultValueCreator: bindable => true);

		/// <summary>
		/// Identifies the <see cref="SegmentTemplate"/> dependency property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="SegmentTemplate"/> dependency property.
		/// </value>
		public static readonly BindableProperty SegmentTemplateProperty =
			BindableProperty.Create(
				nameof(SegmentTemplate),
				typeof(DataTemplate),
				typeof(SfSegmentedControl),
				defaultValueCreator: bindable => null);

		/// <summary>
		/// Identifies the <see cref="ShowSeparator"/> dependency property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="ShowSeparator"/> dependency property.
		/// </value>
		public static readonly BindableProperty ShowSeparatorProperty =
			BindableProperty.Create(
				nameof(ShowSeparator),
				typeof(bool),
				typeof(SfSegmentedControl),
				defaultValueCreator: bindable => true);

		/// <summary>
		/// Identifies the <see cref="EnableRippleEffect"/> dependency property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="EnableRippleEffect"/> dependency property.
		/// </value>
		public static readonly BindableProperty EnableRippleEffectProperty =
			BindableProperty.Create(
				nameof(EnableRippleEffect), 
				typeof(bool), 
				typeof(SfSegmentedControl), 
				true);

		/// <summary>
		/// Identifies the <see cref="SelectionMode"/> dependency property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="SelectionMode"/> dependency property.
		/// </value>
		public static readonly BindableProperty SelectionModeProperty =
			BindableProperty.Create(
				nameof(SelectionMode), 
				typeof(SegmentSelectionMode), 
				typeof(SfSegmentedControl),
				defaultValueCreator: bindable => SegmentSelectionMode.Single);

		/// <summary>
		/// Identifies the <see cref="HoveredBackground"/> dependency property.
		/// </summary>
		/// <value>
		/// Identifies the <see cref="HoveredBackground"/> bindable property.
		/// </value>
		internal static readonly BindableProperty HoveredBackgroundProperty =
			BindableProperty.Create(
				nameof(HoveredBackground),
				typeof(Brush),
				typeof(SfSegmentedControl),
				defaultValueCreator: bindable => null);

		/// <summary>
		/// Identifies the <see cref="SelectedSegmentTextColor"/> dependency property.
		/// </summary>
		/// <value>
		/// Identifies the <see cref="SelectedSegmentTextColor"/> bindable property.
		/// </value>
		internal static readonly BindableProperty SelectedSegmentTextColorProperty =
			BindableProperty.Create(
				nameof(SelectedSegmentTextColor),
				typeof(Color), typeof(SfSegmentedControl),
				defaultValueCreator: bindable => Color.FromArgb("#FFFFFF"),
				propertyChanged: OnSelectedSegmentTextColorChanged);

		/// <summary>
		/// Identifies the <see cref="KeyboardFocusStroke"/> dependency property.
		/// </summary>
		/// <value>
		/// Identifies the <see cref="KeyboardFocusStroke"/> bindable property.
		/// </value>
		internal static readonly BindableProperty KeyboardFocusStrokeProperty =
			BindableProperty.Create(
				nameof(KeyboardFocusStroke),
				typeof(Brush),
				typeof(SfSegmentedControl),
				defaultValueCreator: bindable => null,
				propertyChanged: OnKeyboardFocusStrokeChanged);

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets a collection used to generate the <see cref="SfSegmentItem"/> in the <see cref="SfSegmentedControl"/>.
		/// </summary>
		/// <value>
		/// The default value is <c>null</c>.
		/// </value> 
		/// <summary>
		/// The <see cref="ItemsSource"/> of the <see cref="SfSegmentedControl"/> can accept either a collection of <see cref="SfSegmentItem"/> or a collection of <see cref="string"/> values.
		/// </summary>
		/// <example>
		/// The below examples shows, how to set the items collection to the ItemsSource in the <see cref="SfSegmentedControl"/>.
		/// # [XAML](#tab/tabid-3)
		/// <code Lang="XAML"><![CDATA[
		/// <button:SfSegmentedControl x:Name="segmentedControl"
		///                            ItemsSource="{Binding Items}">
		///    <button:SfSegmentedControl.BindingContext>
		///        <local:SegmentViewModel/>
		///    </button:SfSegmentedControl.BindingContext>
		/// </button:SfSegmentedControl>
		/// ]]></code>
		///  # [C#](#tab/tabid-4)
		/// <code Lang="C#"><![CDATA[
		/// public class SegmentViewModel
		/// {
		///    public List<SfSegmentItem> Items { get; set; }
		///    public SegmentViewModel()
		///    {
		///        Items = new List<SfSegmentItem>
		///        {
		///            new SfSegmentItem() { Text="Day" },
		///            new SfSegmentItem() { Text="Week"},
		///            new SfSegmentItem() { Text="Month"},
		///            new SfSegmentItem() { Text="Year"}
		///        };
		///    }
		/// }
		/// ]]></code>
		///  # [C#](#tab/tabid-5)
		/// <code Lang="C#"><![CDATA[
		///  var items = new List<SfSegmentItem>();
		///  items.Add(new SfSegmentItem() { Text = "Day" });
		///  items.Add(new SfSegmentItem() { Text = "Week" });
		///  items.Add(new SfSegmentItem() { Text = "Month" });
		///  items.Add(new SfSegmentItem() { Text = "Year" });
		///  segmentedControl.ItemsSource = items;
		/// ]]></code>
		/// </example>
		public object ItemsSource
		{
			get { return (object)GetValue(ItemsSourceProperty); }
			set { SetValue(ItemsSourceProperty, value); }
		}

		/// <summary>
		/// Gets or sets the index of the currently selected item in the <see cref="SfSegmentedControl"/>.
		/// </summary>
		/// <value>
		/// The index of the selected item defaults to <c>null</c>, indicating that no item is currently selected.
		/// </value>
		/// <seealso cref="SfSegmentedControl.SelectionChanged"/>
		/// /// <example>
		/// The below examples shows, how to use the selected index property in the <see cref="SfSegmentedControl"/>.
		///  # [XAML](#tab/tabid-6)
		/// <code Lang="XAML"><![CDATA[
		/// <button:SfSegmentedControl x:Name="segmentedControl"
		///                            SelectedIndex="1">
		///    <button:SfSegmentedControl.ItemsSource>
		///        <x:Array Type="{x:Type x:String}">
		///            <x:String>Day</x:String>
		///            <x:String>Week</x:String>
		///            <x:String>Month</x:String>
		///            <x:String>Year</x:String>
		///        </x:Array>
		///    </button:SfSegmentedControl.ItemsSource>
		/// </button:SfSegmentedControl>
		/// ]]></code>
		/// # [C#](#tab/tabid-7)
		/// <code Lang="C#"><![CDATA[
		/// segmentedControl.SelectedIndex = 1;
		/// ]]></code>
		/// </example>
		public int? SelectedIndex
		{
			get { return (int?)GetValue(SelectedIndexProperty); }
			set { SetValue(SelectedIndexProperty, value); }
		}

		/// <summary>
		/// Gets or sets the height of the segments in the <see cref="SfSegmentedControl"/>.
		/// </summary>
		/// <value>
		/// The default value is <c>40</c>.
		/// </value>
		/// <seealso cref="SegmentWidth"/>
		/// <seealso cref="SegmentBackground"/>
		/// <seealso cref="CornerRadius"/>
		/// <seealso cref="SegmentCornerRadius"/>
		/// <example>
		/// The below examples shows, how to use the segment height property in the <see cref="SfSegmentedControl"/>.
		/// # [XAML](#tab/tabid-8)
		/// <code Lang="XAML"><![CDATA[
		/// <button:SfSegmentedControl x:Name="segmentedControl"
		///                            SegmentHeight="60">
		///    <button:SfSegmentedControl.ItemsSource>
		///        <x:Array Type="{x:Type x:String}">
		///            <x:String>Day</x:String>
		///            <x:String>Week</x:String>
		///            <x:String>Month</x:String>
		///            <x:String>Year</x:String>
		///        </x:Array>
		///    </button:SfSegmentedControl.ItemsSource>
		/// </button:SfSegmentedControl>
		/// ]]></code>
		/// # [C#](#tab/tabid-9)
		/// <code Lang="C#"><![CDATA[
		/// segmentedControl.SegmentHeight = 60;
		/// ]]></code>
		/// </example>
		public double SegmentHeight
		{
			get { return (double)GetValue(SegmentHeightProperty); }
			set { SetValue(SegmentHeightProperty, value); }
		}

		/// <summary>
		/// Gets or sets the width of the segments in the <see cref="SfSegmentedControl"/>.
		/// </summary>
		/// <value>
		/// The default value is <c>100</c>.
		/// </value> 
		/// <seealso cref="SegmentHeight"/>
		/// <seealso cref="SegmentBackground"/>
		/// <seealso cref="CornerRadius"/>
		/// <seealso cref="SegmentCornerRadius"/>
		/// <example>
		/// The below examples shows, how to use the segment width property in the <see cref="SfSegmentedControl"/>.
		/// # [XAML](#tab/tabid-10)
		/// <code Lang="XAML"><![CDATA[
		/// <button:SfSegmentedControl x:Name="segmentedControl"
		///                            SegmentWidth="120">
		///    <button:SfSegmentedControl.ItemsSource>
		///        <x:Array Type="{x:Type x:String}">
		///            <x:String>Day</x:String>
		///            <x:String>Week</x:String>
		///            <x:String>Month</x:String>
		///            <x:String>Year</x:String>
		///        </x:Array>
		///    </button:SfSegmentedControl.ItemsSource>
		/// </button:SfSegmentedControl>
		/// ]]></code>
		/// # [C#](#tab/tabid-11)
		/// <code Lang="C#"><![CDATA[
		/// segmentedControl.SegmentWidth = 120;
		/// ]]></code>
		/// </example>
		public double SegmentWidth
		{
			get { return (double)GetValue(SegmentWidthProperty); }
			set { SetValue(SegmentWidthProperty, value); }
		}

		/// <summary>
		/// Gets or sets the number of visible segments to be displayed in the <see cref="SfSegmentedControl"/>.
		/// </summary>
		/// <remarks>
		/// When set the <see cref="VisibleSegmentsCount"/> property, it automatically adjusts the layout of segments.
		/// This means that the  <see cref="SegmentWidth"/> and <see cref="SfSegmentItem.Width"/> properties will not apply, and the WidthRequest value should be divided by the <see cref="VisibleSegmentsCount"/> to determine the width of each segment.
		/// </remarks>
		/// <value>
		/// The default value is <c>-1</c>.
		/// </value>
		/// <example>
		/// The below examples shows, how to use the visible segments count property in the <see cref="SfSegmentedControl"/>.
		/// # [XAML](#tab/tabid-12)
		/// <code Lang="XAML"><![CDATA[
		/// <button:SfSegmentedControl x:Name="segmentedControl"
		///                            WidthRequest="102"
		///                            VisibleSegmentsCount="2">
		///    <button:SfSegmentedControl.ItemsSource>
		///        <x:Array Type="{x:Type x:String}">
		///            <x:String>Day</x:String>
		///            <x:String>Week</x:String>
		///            <x:String>Month</x:String>
		///            <x:String>Year</x:String>
		///        </x:Array>
		///    </button:SfSegmentedControl.ItemsSource>
		/// </button:SfSegmentedControl>
		/// ]]></code>
		/// # [C#](#tab/tabid-13)
		/// <code Lang="C#"><![CDATA[
		/// segmentedControl.WidthRequest = 102;
		/// segmentedControl.VisibleSegmentsCount = 2;
		/// ]]></code>
		/// </example>
		public int VisibleSegmentsCount
		{
			get { return (int)GetValue(VisibleSegmentsCountProperty); }
			set { SetValue(VisibleSegmentsCountProperty, value); }
		}

		/// <summary>
		/// Gets or sets the settings for the segment selection indicator, which is used to highlight the selected item in the <see cref="SfSegmentedControl"/>.
		/// </summary>
		/// <remarks>
		/// When the selection mode is set to <see cref="SelectionIndicatorPlacement.Fill"/>, the selected item's appearance is determined by the <see cref="SelectionIndicatorSettings.Background"/> property. However, when the selection mode is set to <see cref="SelectionIndicatorPlacement.Border"/>, <see cref="SelectionIndicatorPlacement.TopBorder"/>, or <see cref="SelectionIndicatorPlacement.BottomBorder"/>, the selected color is determined by the <see cref="SelectionIndicatorSettings.Stroke"/> and <see cref="SelectionIndicatorSettings.StrokeThickness"/> properties, and for border selection, the text color of the selected item is determined by the <see cref="SelectionIndicatorSettings.Background"/> property.
		/// </remarks>
		/// <value>The default value of <see cref="SelectionIndicatorSettings.TextColor"/> is <see cref="Colors.White"/>, <see cref="SelectionIndicatorSettings.Background"/> is "new SolidColorBrush(Color.FromArgb("#6750A4"))", <see cref="SelectionIndicatorSettings.SelectionIndicatorPlacement"/> is <see cref="SelectionIndicatorPlacement.Fill"/>, <see cref="SelectionIndicatorSettings.Stroke"/> is "Color.FromArgb("#6750A4")"> <see cref="SelectionIndicatorSettings.StrokeThickness"/> is "3"/>.</value>
		/// <seealso cref="TextStyle"/>
		/// <example>
		/// The below examples shows, how to use the <see cref="SelectionIndicatorSettings"/> property in the <see cref="SfSegmentedControl"/>.
		/// # [XAML](#tab/tabid-14)
		/// <code Lang="XAML"><![CDATA[
		/// <button:SfSegmentedControl x:Name="segmentedControl"
		///                            SelectionIndicatorSettings="{button:SelectionIndicatorSettings SelectionIndicatorPlacement=Border, Stroke=Orange}">
		///    <button:SfSegmentedControl.ItemsSource>
		///        <x:Array Type="{x:Type x:String}">
		///            <x:String>Day</x:String>
		///            <x:String>Week</x:String>
		///            <x:String>Month</x:String>
		///            <x:String>Year</x:String>
		///        </x:Array>
		///    </button:SfSegmentedControl.ItemsSource>
		/// </button:SfSegmentedControl>
		/// ]]></code>
		/// # [C#](#tab/tabid-15)
		/// <code Lang="C#"><![CDATA[
		/// segmentedControl.SelectionIndicatorSettings = new SelectionIndicatorSettings() { SelectionIndicatorPlacement = SelectionIndicatorPlacement.Border, Stroke = Colors.Orange };
		/// ]]></code>
		/// </example>
		public SelectionIndicatorSettings SelectionIndicatorSettings
		{
			get { return (SelectionIndicatorSettings)GetValue(SelectionIndicatorSettingsProperty); }
			set { SetValue(SelectionIndicatorSettingsProperty, value); }
		}

		/// <summary>
		/// Gets or sets the text color of the disabled segment items in the <see cref="SfSegmentedControl"/>.
		/// </summary>
		/// <remarks>
		/// This property will be applicable to only when the <see cref="SfSegmentItem.IsEnabled"/> is set to "false"/>.
		/// </remarks>
		/// <value>
		/// The default value is "Color.FromArgb("#1C1B1F61")"/>.
		/// </value>
		/// <seealso cref="SetSegmentEnabled(int, bool)"/>
		/// <seealso cref="SfSegmentItem.IsEnabled"/>
		/// <seealso cref="DisabledSegmentBackground"/>
		/// <example>
		/// The below examples shows, how to use the <see cref="DisabledSegmentTextColor"/> property in the <see cref="SfSegmentedControl"/>.
		/// # [XAML](#tab/tabid-16)
		/// <code Lang="XAML"><![CDATA[
		/// <button:SfSegmentedControl x:Name="segmentedControl"
		///                            ItemsSource="{Binding Items}"
		///                            DisabledSegmentTextColor="Gray">
		///    <button:SfSegmentedControl.BindingContext>
		///        <local:SegmentViewModel/>
		///    </button:SfSegmentedControl.BindingContext>
		/// </button:SfSegmentedControl>
		/// ]]></code>
		///  # [C#](#tab/tabid-17)
		/// <code Lang="C#"><![CDATA[
		/// public class SegmentViewModel
		/// {
		///    public List<SfSegmentItem> Items { get; set; }
		///    public SegmentViewModel()
		///    {
		///        Items = new List<SfSegmentItem>
		///        {
		///           new SfSegmentItem() { Text = "Day" },
		///           new SfSegmentItem() { Text = "Week", IsEnabled=false},
		///           new SfSegmentItem() { Text = "Month"},
		///           new SfSegmentItem() { Text = "Year", IsEnabled=false}
		///        };
		///    }
		/// }
		/// ]]></code>
		/// # [C#](#tab/tabid-18)
		/// <code Lang="C#"><![CDATA[
		/// segmentedControl.DisabledSegmentTextColor = Colors.Gray;
		/// ]]></code>
		/// </example>
		public Color DisabledSegmentTextColor
		{
			get { return (Color)GetValue(DisabledSegmentTextColorProperty); }
			set { SetValue(DisabledSegmentTextColorProperty, value); }
		}

		/// <summary>
		/// Gets or sets the background brush of the disabled segment items in the <see cref="SfSegmentedControl"/>.
		/// </summary>
		/// <remarks>
		/// This property will be applicable to only when the <see cref="SfSegmentItem.IsEnabled"/> is set to "false"/>.
		/// </remarks>
		/// <value>
		/// The default value is <see cref="Brush.Transparent"/>.
		/// </value>
		/// <seealso cref="SetSegmentEnabled(int, bool)"/>
		/// <seealso cref="SfSegmentItem.IsEnabled"/>
		/// <seealso cref="DisabledSegmentTextColor"/>
		/// <example>
		/// The below examples shows, how to use the <see cref="DisabledSegmentBackground"/> property in the <see cref="SfSegmentedControl"/>.
		/// # [XAML](#tab/tabid-19)
		/// <code Lang="XAML"><![CDATA[
		/// <button:SfSegmentedControl x:Name="segmentedControl"
		///                            ItemsSource="{Binding Items}"
		///                            DisabledSegmentBackground="Red">
		///    <button:SfSegmentedControl.BindingContext>
		///        <local:SegmentViewModel/>
		///    </button:SfSegmentedControl.BindingContext>
		/// </button:SfSegmentedControl>
		/// ]]></code>
		///  # [C#](#tab/tabid-20)
		/// <code Lang="C#"><![CDATA[
		/// public class SegmentViewModel
		/// {
		///    public List<SfSegmentItem> Items { get; set; }
		///    public SegmentViewModel()
		///    {
		///        Items = new List<SfSegmentItem>
		///        {
		///           new SfSegmentItem() { Text = "Day" },
		///           new SfSegmentItem() { Text = "Week", IsEnabled=false},
		///           new SfSegmentItem() { Text = "Month"},
		///           new SfSegmentItem() { Text = "Year", IsEnabled=false}
		///        };
		///    }
		/// }
		/// ]]></code>
		/// # [C#](#tab/tabid-21)
		/// <code Lang="C#"><![CDATA[
		/// segmentedControl.DisabledSegmentBackground = Brush.Red;
		/// ]]></code>
		/// </example>
		public Brush DisabledSegmentBackground
		{
			get { return (Brush)GetValue(DisabledSegmentBackgroundProperty); }
			set { SetValue(DisabledSegmentBackgroundProperty, value); }
		}

		/// <summary>
		/// Gets or sets the style of segment item text, that used to customize the text color, font, font size, font family and font attributes.
		/// </summary>
		/// <value>The default value of <see cref="SegmentTextStyle.TextColor"/> is "Color.FromArgb("#1C1B1F")", <see cref="SegmentTextStyle.FontSize"/> is 14, <see cref="SegmentTextStyle.FontFamily"/> is null, <see cref="SegmentTextStyle.FontAttributes"/> is <see cref="FontAttributes.None"/>.</value>
		/// <seealso cref="SegmentBackground"/>
		/// <seealso cref="SelectionIndicatorSettings"/>
		/// <example>
		/// The below examples shows, how to use the <see cref="TextStyle"/> property in the <see cref="SfSegmentedControl"/>.
		/// # [XAML](#tab/tabid-22)
		/// <code Lang="XAML"><![CDATA[
		/// <button:SfSegmentedControl x:Name="segmentedControl"
		///                            TextStyle="{button:SegmentTextStyle TextColor=Blue, FontAttributes=Italic}">
		///    <button:SfSegmentedControl.ItemsSource>
		///        <x:Array Type="{x:Type x:String}">
		///            <x:String>Day</x:String>
		///            <x:String>Week</x:String>
		///            <x:String>Month</x:String>
		///            <x:String>Year</x:String>
		///        </x:Array>
		///    </button:SfSegmentedControl.ItemsSource>
		/// </button:SfSegmentedControl>
		/// ]]></code>
		/// # [C#](#tab/tabid-23)
		/// <code Lang="C#"><![CDATA[
		/// segmentedControl.TextStyle = new SegmentTextStyle() { TextColor = Colors.Blue, FontAttributes = FontAttributes.Italic };
		/// ]]></code>
		/// </example>
		public SegmentTextStyle TextStyle
		{
			get { return (SegmentTextStyle)GetValue(TextStyleProperty); }
			set { SetValue(TextStyleProperty, value); }
		}

		/// <summary>
		/// Gets or sets the background brush for the segments in the <see cref="SfSegmentedControl"/>.
		/// </summary>
		/// <value>
		/// The default value of <see cref="Brush.Transparent"/>.
		/// </value>
		/// <seealso cref="TextStyle"/>
		/// <seealso cref="SelectionIndicatorSettings"/>
		/// <example>
		/// The below examples shows, how to use the <see cref="SegmentBackground"/> property in the <see cref="SfSegmentedControl"/>.
		/// # [XAML](#tab/tabid-24)
		/// <code Lang="XAML"><![CDATA[
		/// <button:SfSegmentedControl x:Name="segmentedControl"
		///                            SegmentBackground="Orange">
		///    <button:SfSegmentedControl.ItemsSource>
		///        <x:Array Type="{x:Type x:String}">
		///            <x:String>Day</x:String>
		///            <x:String>Week</x:String>
		///            <x:String>Month</x:String>
		///            <x:String>Year</x:String>
		///        </x:Array>
		///    </button:SfSegmentedControl.ItemsSource>
		/// </button:SfSegmentedControl>
		/// ]]></code>
		/// # [C#](#tab/tabid-25)
		/// <code Lang="C#"><![CDATA[
		/// segmentedControl.SegmentBackground = Brush.Orange;
		/// ]]></code>
		/// </example>
		public Brush SegmentBackground
		{
			get { return (Brush)GetValue(SegmentBackgroundProperty); }
			set { SetValue(SegmentBackgroundProperty, value); }
		}

		/// <summary>
		/// Gets or sets the stroke brush for the segments in the <see cref="SfSegmentedControl"/>.
		/// </summary>
		/// <value>
		/// The default value of "new SolidColorBrush(Color.FromArgb("#79747E")"/>.
		/// </value>
		/// <seealso cref="StrokeThickness"/>
		/// <seealso cref="ShowSeparator"/>
		/// <example>
		/// The below examples shows, how to use the <see cref="Stroke"/> property in the <see cref="SfSegmentedControl"/>.
		/// # [XAML](#tab/tabid-26)
		/// <code Lang="XAML"><![CDATA[
		/// <button:SfSegmentedControl x:Name="segmentedControl"
		///                            Stroke="Orange">
		///    <button:SfSegmentedControl.ItemsSource>
		///        <x:Array Type="{x:Type x:String}">
		///            <x:String>Day</x:String>
		///            <x:String>Week</x:String>
		///            <x:String>Month</x:String>
		///            <x:String>Year</x:String>
		///        </x:Array>
		///    </button:SfSegmentedControl.ItemsSource>
		/// </button:SfSegmentedControl>
		/// ]]></code>
		/// # [C#](#tab/tabid-27)
		/// <code Lang="C#"><![CDATA[
		/// segmentedControl.Stroke = Brush.Orange;
		/// ]]></code>
		/// </example>
		public Brush Stroke
		{
			get { return (Brush)GetValue(StrokeProperty); }
			set { SetValue(StrokeProperty, value); }
		}

		/// <summary>
		/// Gets or sets the thickness of the segment stroke in the <see cref="SfSegmentedControl"/>.
		/// </summary>
		/// <value>
		/// The default value of 1.
		/// </value>
		/// <seealso cref="Stroke"/>
		/// <seealso cref="ShowSeparator"/>
		/// <example>
		/// The below examples shows, how to use the <see cref="StrokeThickness"/> property in the <see cref="SfSegmentedControl"/>.
		/// # [XAML](#tab/tabid-28)
		/// <code Lang="XAML"><![CDATA[
		/// <button:SfSegmentedControl x:Name="segmentedControl"
		///                            StrokeThickness="2">
		///    <button:SfSegmentedControl.ItemsSource>
		///        <x:Array Type="{x:Type x:String}">
		///            <x:String>Day</x:String>
		///            <x:String>Week</x:String>
		///            <x:String>Month</x:String>
		///            <x:String>Year</x:String>
		///        </x:Array>
		///    </button:SfSegmentedControl.ItemsSource>
		/// </button:SfSegmentedControl>
		/// ]]></code>
		/// # [C#](#tab/tabid-29)
		/// <code Lang="C#"><![CDATA[
		/// segmentedControl.StrokeThickness = 2;
		/// ]]></code>
		/// </example>
		public double StrokeThickness
		{
			get { return (double)GetValue(StrokeThicknessProperty); }
			set { SetValue(StrokeThicknessProperty, value); }
		}

		/// <summary>
		/// Gets or sets the corner radius for the border of the <see cref="SfSegmentedControl"/>.
		/// </summary>
		/// <remarks>
		/// This property will be applicable to only for the first and last segment items. To set CornerRadius for all segments, use <see cref="SegmentCornerRadius"/>.
		/// </remarks>
		/// <value>
		/// The default value is <see cref="CornerRadius.CornerRadius(double)"/> with a value of 20.
		/// </value>
		/// <seealso cref="SegmentCornerRadius"/>
		/// <seealso cref="ShowSeparator"/>
		/// <example>
		/// The below examples shows, how to use the <see cref="CornerRadius"/> property in the <see cref="SfSegmentedControl"/>.
		/// # [XAML](#tab/tabid-30)
		/// <code Lang="XAML"><![CDATA[
		/// <button:SfSegmentedControl x:Name="segmentedControl"
		///                            CornerRadius="3">
		///    <button:SfSegmentedControl.ItemsSource>
		///        <x:Array Type="{x:Type x:String}">
		///            <x:String>Day</x:String>
		///            <x:String>Week</x:String>
		///            <x:String>Month</x:String>
		///            <x:String>Year</x:String>
		///        </x:Array>
		///    </button:SfSegmentedControl.ItemsSource>
		/// </button:SfSegmentedControl>
		/// ]]></code>
		/// # [C#](#tab/tabid-31)
		/// <code Lang="C#"><![CDATA[
		/// segmentedControl.CornerRadius = 3;
		/// ]]></code>
		/// </example>
		public CornerRadius CornerRadius
		{
			get { return (CornerRadius)GetValue(CornerRadiusProperty); }
			set { SetValue(CornerRadiusProperty, value); }
		}

		/// <summary>
		/// Gets or sets the segment corner radius for the segment items of the <see cref="SfSegmentedControl"/>.
		/// </summary>
		/// <value>
		/// The default value is <see cref="CornerRadius.CornerRadius(double)"/> with a value of 0.
		/// </value>
		/// <seealso cref="CornerRadius"/>
		/// <seealso cref="ShowSeparator"/>
		/// <example>
		/// The below examples shows, how to use the <see cref="SegmentCornerRadius"/> property in the <see cref="SfSegmentedControl"/>.
		/// # [XAML](#tab/tabid-32)
		/// <code Lang="XAML"><![CDATA[
		/// <button:SfSegmentedControl x:Name="segmentedControl"
		///                            SegmentCornerRadius="30"
		///                            StrokeThickness="0"
		///                            ShowSeparator="False">
		///    <button:SfSegmentedControl.ItemsSource>
		///        <x:Array Type="{x:Type x:String}">
		///            <x:String>Day</x:String>
		///            <x:String>Week</x:String>
		///            <x:String>Month</x:String>
		///            <x:String>Year</x:String>
		///        </x:Array>
		///    </button:SfSegmentedControl.ItemsSource>
		/// </button:SfSegmentedControl>
		/// ]]></code>
		/// # [C#](#tab/tabid-33)
		/// <code Lang="C#"><![CDATA[
		/// segmentedControl.SegmentCornerRadius = 30;
		/// segmentedControl.StrokeThickness = 0;
		/// segmentedControl.ShowSeparator = false;
		/// ]]></code>
		/// </example>
		public CornerRadius SegmentCornerRadius
		{
			get { return (CornerRadius)GetValue(SegmentCornerRadiusProperty); }
			set { SetValue(SegmentCornerRadiusProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether to enable auto-scrolling when the selected index is changed in the <see cref="SfSegmentedControl"/>.
		/// </summary>
		/// <remarks>
		/// This property will be applicable to only during the initial loading of the <see cref="SfSegmentedControl"/>.
		/// </remarks>
		/// <value>
		/// The default value is true.
		/// </value>
		/// <seealso cref="SfSegmentedControl.ScrollTo(int)"/>
		/// <seealso cref="SelectedIndex"/>
		/// <seealso cref="VisibleSegmentsCount"/>
		/// <example>
		/// The below examples shows, how to use the <see cref="AutoScrollToSelectedSegment"/> property in the <see cref="SfSegmentedControl"/>.
		/// # [XAML](#tab/tabid-34)
		/// <code Lang="XAML"><![CDATA[
		/// <button:SfSegmentedControl x:Name="segmentedControl"
		///                            AutoScrollToSelectedSegment="False"
		///                            WidthRequest="102"
		///                            SelectedIndex="2"
		///                            VisibleSegmentsCount="2">
		///    <button:SfSegmentedControl.ItemsSource>
		///        <x:Array Type="{x:Type x:String}">
		///            <x:String>Day</x:String>
		///            <x:String>Week</x:String>
		///            <x:String>Month</x:String>
		///            <x:String>Year</x:String>
		///        </x:Array>
		///    </button:SfSegmentedControl.ItemsSource>
		/// </button:SfSegmentedControl>
		/// ]]></code>
		/// # [C#](#tab/tabid-35)
		/// <code Lang="C#"><![CDATA[
		///  segmentedControl.AutoScrollToSelectedSegment = false;
		///  segmentedControl.WidthRequest = 102;
		///  segmentedControl.SelectedIndex = 2;
		///  segmentedControl.VisibleSegmentsCount = 2;
		/// ]]></code>
		/// </example>
		public bool AutoScrollToSelectedSegment
		{
			get { return (bool)GetValue(AutoScrollToSelectedSegmentProperty); }
			set { SetValue(AutoScrollToSelectedSegmentProperty, value); }
		}

		/// <summary>
		/// Gets or sets the data template to use for customizing the appearance of individual segments in the <see cref="SfSegmentedControl"/>.
		/// </summary>
		/// <remarks>
		/// The <see cref="SfSegmentItem"/> will be set as binding context.
		/// </remarks>
		/// <seealso cref="SfSegmentItem.IsEnabled"/>
		/// <example>
		/// The below examples shows, how to use the <see cref="SegmentTemplate"/> property in the <see cref="SfSegmentedControl"/>.
		/// # [XAML](#tab/tabid-36)
		/// <code Lang="XAML"><![CDATA[
		/// <button:SfSegmentedControl x:Name="segmentedControl">
		///    <button:SfSegmentedControl.ItemsSource>
		///        <x:Array Type="{x:Type x:String}">
		///            <x:String>Day</x:String>
		///            <x:String>Week</x:String>
		///            <x:String>Month</x:String>
		///            <x:String>Year</x:String>
		///        </x:Array>
		///    </button:SfSegmentedControl.ItemsSource>
		///    <button:SfSegmentedControl.SegmentTemplate>
		///      <DataTemplate>
		///        <Grid Background="Orange">
		///            <Label Text="{Binding Text}"
		///                   TextColor="Green"
		///                   Margin="6"/>
		///        </Grid>
		///      </DataTemplate>
		///    </button:SfSegmentedControl.SegmentTemplate>
		/// </button:SfSegmentedControl>
		/// ]]></code>
		/// </example>
		public DataTemplate SegmentTemplate
		{
			get { return (DataTemplate)GetValue(SegmentTemplateProperty); }
			set { SetValue(SegmentTemplateProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether to show separators between segments in the <see cref="SfSegmentedControl"/>.
		/// </summary>
		/// <value>
		/// The default value is true.
		/// </value>
		/// <seealso cref="SegmentCornerRadius"/>
		/// <seealso cref="StrokeThickness"/>
		/// <example>
		/// The below examples shows, how to use the <see cref="ShowSeparator"/> property in the <see cref="SfSegmentedControl"/>.
		/// # [XAML](#tab/tabid-37)
		/// <code Lang="XAML"><![CDATA[
		/// <button:SfSegmentedControl x:Name="segmentedControl"
		///                            ShowSeparator="False"
		///                            SegmentCornerRadius="30"
		///                            StrokeThickness="0">
		///    <button:SfSegmentedControl.ItemsSource>
		///        <x:Array Type="{x:Type x:String}">
		///            <x:String>Day</x:String>
		///            <x:String>Week</x:String>
		///            <x:String>Month</x:String>
		///            <x:String>Year</x:String>
		///        </x:Array>
		///    </button:SfSegmentedControl.ItemsSource>
		/// </button:SfSegmentedControl>
		/// ]]></code>
		/// # [C#](#tab/tabid-38)
		/// <code Lang="C#"><![CDATA[
		/// segmentedControl.ShowSeparator = false;
		/// segmentedControl.SegmentCornerRadius = 30;
		/// segmentedControl.StrokeThickness = 0;
		/// ]]></code>
		/// </example>
		public bool ShowSeparator
		{
			get { return (bool)GetValue(ShowSeparatorProperty); }
			set { SetValue(ShowSeparatorProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether the ripple effect animation should be applied to a segment item when it is selected for default and segment template added.
		/// </summary>
		/// <value>
		/// The default value is true.
		/// </value>
		/// <example>
		/// The below examples shows, how to use the <see cref="EnableRippleEffect"/> property in the <see cref="SfSegmentedControl"/>.
		/// # [XAML](#tab/tabid-37)
		/// <code Lang="XAML"><![CDATA[
		/// <button:SfSegmentedControl x:Name="segmentedControl"
		///                            EnableRippleEffect="False">
		///    <button:SfSegmentedControl.ItemsSource>
		///        <x:Array Type="{x:Type x:String}">
		///            <x:String>Day</x:String>
		///            <x:String>Week</x:String>
		///            <x:String>Month</x:String>
		///            <x:String>Year</x:String>
		///        </x:Array>
		///    </button:SfSegmentedControl.ItemsSource>
		/// </button:SfSegmentedControl>
		/// ]]></code>
		/// # [C#](#tab/tabid-38)
		/// <code Lang="C#"><![CDATA[
		/// this.segmentedControl.EnableRippleEffect = false;
		/// ]]></code>
		/// </example>
		public bool EnableRippleEffect
		{
			get { return (bool)this.GetValue(EnableRippleEffectProperty); }
			set { this.SetValue(EnableRippleEffectProperty, value); }
		}

		/// <summary>
		/// Gets or sets the selection behavior of segment items, allowing either single selection or single deselection.
		/// </summary>
		/// <value>
		/// The default value of <see cref="SegmentSelectionMode"/> is <see cref="SegmentSelectionMode.Single"/>.
		/// </value>
		/// <example>
		/// The below examples shows, how to use the <see cref="SelectionMode"/> property in the <see cref="SfSegmentedControl"/>.
		/// # [XAML](#tab/tabid-37)
		/// <code Lang="XAML"><![CDATA[
		/// <button:SfSegmentedControl x:Name="segmentedControl"
		///                            SelectionMode="Single">
		///    <button:SfSegmentedControl.ItemsSource>
		///        <x:Array Type="{x:Type x:String}">
		///            <x:String>Day</x:String>
		///            <x:String>Week</x:String>
		///            <x:String>Month</x:String>
		///            <x:String>Year</x:String>
		///        </x:Array>
		///    </button:SfSegmentedControl.ItemsSource>
		/// </button:SfSegmentedControl>
		/// ]]></code>
		/// # [C#](#tab/tabid-38)
		/// <code Lang="C#"><![CDATA[
		/// this.segmentedControl.SelectionMode = SegmentSelectionMode.Single;
		/// ]]></code>
		/// </example>
		public SegmentSelectionMode SelectionMode
		{
			get { return (SegmentSelectionMode)this.GetValue(SelectionModeProperty); }
			set { this.SetValue(SelectionModeProperty, value); }
		}

		/// <summary>
		/// Gets or sets the hovered selected background brush for the segment.
		/// </summary>
		internal Brush HoveredBackground
		{
			get { return (Brush)GetValue(HoveredBackgroundProperty); }
			set { SetValue(HoveredBackgroundProperty, value); }
		}

		/// <summary>
		/// Gets or sets the selected text color brush for the segment.
		/// </summary>
		internal Color SelectedSegmentTextColor
		{
			get { return (Color)this.GetValue(SelectedSegmentTextColorProperty); }
			set { this.SetValue(SelectedSegmentTextColorProperty, value); }
		}

		/// <summary>
		/// Gets or sets the focused keyboard stroke for the segment.
		/// </summary>
		internal Brush KeyboardFocusStroke
		{
			get { return (Brush)GetValue(KeyboardFocusStrokeProperty); }
			set { SetValue(KeyboardFocusStrokeProperty, value); }
		}

		#endregion

		#region Property changed

		/// <summary>
		/// Occurs when <see cref="ItemsSource"/> property changed.
		/// </summary>
		/// <param name="bindable">The bindable object.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
		{
			SfSegmentedControl segmentedControl = (SfSegmentedControl)bindable;
			if (segmentedControl == null || !segmentedControl.IsLoaded)
			{
				return;
			}

			if (oldValue is IEnumerable oldSegmentItems)
			{
				if (oldSegmentItems is INotifyCollectionChanged oldSegmentItemsChanged)
				{
					oldSegmentItemsChanged.CollectionChanged -= segmentedControl.OnSegmentItemsCollectionChanged;
				}
			}

			if (newValue is IEnumerable newSegmentItems)
			{
				if (newSegmentItems is INotifyCollectionChanged newSegmentItemsChanged)
				{
					newSegmentItemsChanged.CollectionChanged += segmentedControl.OnSegmentItemsCollectionChanged;
				}
			}

			// If the segment items not set on control initial loading, and generating items at run items. We need to initialize the segment items views and scroll view etc.
			if (segmentedControl._segmentLayout == null)
			{
				segmentedControl.InitializeSegmentItems();
				segmentedControl.InitializeSegment();
				segmentedControl._segmentLayout?.InvalidateLayout();
				segmentedControl.InvalidateLayout();
				return;
			}

			segmentedControl._segmentLayout?.ClearSegmentItemsView();
			segmentedControl.InitializeSegmentItems();
			segmentedControl._segmentLayout?.UpdateLayout();
			segmentedControl._segmentLayout?.UpdateItemSelection();
			segmentedControl.InvalidateLayout();
		}

		/// <summary>
		/// Called when <see cref="SelectedIndex"/> property changed.
		/// </summary>
		/// <param name="bindable">The bindable object.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		static void OnSelectedIndexPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			SfSegmentedControl segmentedControl = (SfSegmentedControl)bindable;
			if (segmentedControl == null || segmentedControl._segmentLayout == null || segmentedControl._items == null)
			{
				return;
			}

			// Get the old and new indices as integers, handling possible null values.
			int oldIndex = (int)(oldValue ?? -1);
			int newIndex = (int)(newValue ?? -1);

			// Update the item selection in the segment layout.
			segmentedControl._segmentLayout.UpdateItemSelection();

			// Get the old and new selected items based on the indices.
			SfSegmentItem? oldSelectedItem = oldIndex >= 0 ? segmentedControl._items.ElementAtOrDefault(oldIndex) : null;
			SfSegmentItem? newSelectedItem = newIndex >= 0 ? segmentedControl._items.ElementAtOrDefault(newIndex) : null;

			// Create a selection changed event args with the old and new index and items.
			SelectionChangedEventArgs eventArgs = new SelectionChangedEventArgs()
			{
				OldIndex = oldIndex,
				NewIndex = newIndex,
				OldValue = oldSelectedItem,
				NewValue = newSelectedItem
			};

			// Update the scroll position to the selected index.
			segmentedControl.UpdateScrollPositionToSelectedIndex(newIndex, segmentedControl._isAutoScrollToSelectedIndex);

			// Trigger the selection changed event for the segment item.
			((ISegmentItemInfo)segmentedControl).TriggerSelectionChangedEvent(eventArgs);
		}

		/// <summary>
		/// Occurs when <see cref="VisibleSegmentsCount"/> property changed.
		/// </summary>
		/// <param name="bindable">The bindable object.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		static void OnVisibleSegmentsCountChanged(BindableObject bindable, object oldValue, object newValue)
		{
			SfSegmentedControl segmentedControl = (SfSegmentedControl)bindable;
			if (segmentedControl == null || !segmentedControl.IsLoaded)
			{
				return;
			}

			segmentedControl.InvalidateLayout();
			segmentedControl._segmentLayout?.InvalidateLayout();
		}

		/// <summary>
		/// Occurs when <see cref="Stroke"/> property changed.
		/// </summary>
		/// <param name="bindable">The bindable object.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		static void OnStrokeChanged(BindableObject bindable, object oldValue, object newValue)
		{
			SfSegmentedControl segmentedControl = (SfSegmentedControl)bindable;
			if (segmentedControl == null || !segmentedControl.IsLoaded)
			{
				return;
			}

			segmentedControl._outlinedBorderView?.InvalidateDrawable();
			segmentedControl._segmentLayout?.InvalidateDrawable();
		}

		/// <summary>
		/// Occurs when <see cref="StrokeThickness"/> property changed.
		/// </summary>
		/// <param name="bindable">The bindable object.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		static void OnStrokeThicknessChanged(BindableObject bindable, object oldValue, object newValue)
		{
			SfSegmentedControl segmentedControl = (SfSegmentedControl)bindable;
			if (segmentedControl == null || !segmentedControl.IsLoaded)
			{
				return;
			}

			segmentedControl.InvalidateLayout();
			// As we handled key navigation focused view at this layout. So while stroke thickness we need to update the key navigation for the item at the focused index based on the stroke thickness value.
			segmentedControl._segmentLayout?.UpdateKeyNavigationViewOnScroll();
			segmentedControl._outlinedBorderView?.InvalidateDrawable();
			segmentedControl._segmentLayout?.InvalidateLayout();
			segmentedControl._segmentLayout?.InvalidateDrawable();
		}

		/// <summary>
		/// Occurs when <see cref="SegmentCornerRadius"/> property changed.
		/// </summary>
		/// <param name="bindable">The bindable object.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		static void OnSegmentCornerRadiusChanged(BindableObject bindable, object oldValue, object newValue)
		{
			SfSegmentedControl segmentedControl = (SfSegmentedControl)bindable;
			if (segmentedControl == null || !segmentedControl.IsLoaded)
			{
				return;
			}

			segmentedControl.InvalidateLayout();
			segmentedControl._keyNavigationView?.InvalidateDrawable();
			// As we have clipped the parent grid view based on segment corner radius. so we need to set the corner radius for the segment item first and last views to show the border properly for segment item selections.
			// While changing segment corner radius value we need to update the segment item first and last views corner radius again. So clearing existing items.
			segmentedControl._segmentLayout?.UpdateSegmentItemsView();
		}

		/// <summary>
		/// Occurs when <see cref="SegmentBackground"/> property changed.
		/// </summary>
		/// <param name="bindable">The bindable object.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		static void OnSegmentBackgroundChanged(BindableObject bindable, object oldValue, object newValue)
		{
			SfSegmentedControl segmentedControl = (SfSegmentedControl)bindable;
			if (segmentedControl == null || segmentedControl._segmentLayout == null || !segmentedControl.IsLoaded)
			{
				return;
			}

			segmentedControl._segmentLayout.InvalidateSegmentItemsDraw();
		}

		/// <summary>
		/// Occurs when <see cref="SelectionIndicatorSettings"/> property changed.
		/// </summary>
		/// <param name="bindable">The bindable object.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		static void OnSelectionSettingsChanged(BindableObject bindable, object oldValue, object newValue)
		{
			SfSegmentedControl segmentedControl = (SfSegmentedControl)bindable;
			SegmentViewHelper.SetParent((Element)oldValue, (Element)newValue, segmentedControl);
			if (segmentedControl == null || segmentedControl._segmentLayout == null || !segmentedControl.IsLoaded)
			{
				return;
			}

			if (oldValue is SelectionIndicatorSettings oldSettings)
			{
				oldSettings.PropertyChanged -= segmentedControl.OnSelectionIndicatorSettingsPropertyChanged;
			}

			if (newValue is SelectionIndicatorSettings newSettings)
			{
				newSettings.PropertyChanged += segmentedControl.OnSelectionIndicatorSettingsPropertyChanged;
			}

			segmentedControl._segmentLayout.InvalidateSegmentItemsDraw();
		}

		/// <summary>
		/// Occurs when <see cref="TextStyle"/> property changed.
		/// </summary>
		/// <param name="bindable">The bindable object.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		static void OnTextStyleChanged(BindableObject bindable, object oldValue, object newValue)
		{
			SfSegmentedControl segmentedControl = (SfSegmentedControl)bindable;
			SegmentViewHelper.SetParent((Element)oldValue, (Element)newValue, segmentedControl);
			if (segmentedControl == null || segmentedControl._segmentLayout == null || !segmentedControl.IsLoaded)
			{
				return;
			}

			if (oldValue is SegmentTextStyle oldStyle)
			{
				oldStyle.PropertyChanged -= segmentedControl.OnTextStylePropertyChanged;
			}

			if (newValue is SegmentTextStyle newStyle)
			{
				newStyle.PropertyChanged += segmentedControl.OnTextStylePropertyChanged;
			}

			segmentedControl._segmentLayout.InvalidateSegmentItemsDraw();
		}

		/// <summary>
		/// Occurs when <see cref="DisabledSegmentTextColor"/> property changed.
		/// </summary>
		/// <param name="bindable">The bindable object.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		static void OnDisabledSegmentTextColorChanged(BindableObject bindable, object oldValue, object newValue)
		{
			SfSegmentedControl segmentedControl = (SfSegmentedControl)bindable;
			if (segmentedControl == null || segmentedControl._segmentLayout == null || !segmentedControl.IsLoaded)
			{
				return;
			}

			segmentedControl._segmentLayout.UpdateDisabledSegmentItemStyle();
		}

		/// <summary>
		/// Occurs when <see cref="KeyboardFocusStroke"/> property changed.
		/// </summary>
		/// <param name="bindable">The bindable object.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		static void OnKeyboardFocusStrokeChanged(BindableObject bindable, object oldValue, object newValue)
		{
			SfSegmentedControl segmentedControl = (SfSegmentedControl)bindable;
			if (segmentedControl == null || segmentedControl._segmentLayout == null || !segmentedControl.IsLoaded)
			{
				return;
			}

			segmentedControl._keyNavigationView?.InvalidateDrawable();
		}

		/// <summary>
		/// Occurs when <see cref="SelectedSegmentTextColor"/> property changed.
		/// </summary>
		/// <param name="bindable">The bindable object.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		static void OnSelectedSegmentTextColorChanged(BindableObject bindable, object oldValue, object newValue)
		{
			SfSegmentedControl segmentedControl = (SfSegmentedControl)bindable;
			if (segmentedControl == null || segmentedControl._segmentLayout == null || !segmentedControl.IsLoaded)
			{
				return;
			}

			segmentedControl._segmentLayout?.UpdateSelectedSegmentItemStyle();
		}

		/// <summary>
		/// Occurs when <see cref="DisabledSegmentBackground"/> property changed.
		/// </summary>
		/// <param name="bindable">The bindable object.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		static void OnDisabledSegmentBackgroundChanged(BindableObject bindable, object oldValue, object newValue)
		{
			SfSegmentedControl segmentedControl = (SfSegmentedControl)bindable;
			if (segmentedControl == null || segmentedControl._segmentLayout == null || !segmentedControl.IsLoaded)
			{
				return;
			}

			segmentedControl._segmentLayout.UpdateDisabledSegmentItemStyle();
		}

		/// <summary>
		/// Occurs when <see cref="CornerRadius"/> property changed.
		/// </summary>
		/// <param name="bindable">The bindable object.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		static void OnCornerRadiusChanged(BindableObject bindable, object oldValue, object newValue)
		{
			SfSegmentedControl segmentedControl = (SfSegmentedControl)bindable;
			if (segmentedControl == null || !segmentedControl.IsLoaded)
			{
				return;
			}

			segmentedControl.InvalidateLayout();
			segmentedControl._outlinedBorderView?.InvalidateDrawable();
			segmentedControl._keyNavigationView?.InvalidateDrawable();
			// As we have clipped the parent grid view based on corner radius. so we need to set the corner radius for the segment item first and last views to show the border properly for segment item selections.
			// While changing corner radius value we need to update the segment item first and last views corner radius again. So clearing existing items.
			segmentedControl._segmentLayout?.UpdateSegmentItemsView();
		}

		#endregion

		#region Event
#nullable disable
		/// <summary>
		/// Occurs when the selection within the segment item is changed.
		/// </summary>
		/// <example>
		/// The following code demonstrates, how to use the SfSegmentedControl's selection changed event.
		/// <code Lang="C#"><![CDATA[
		/// segmentedControl.SelectionChanged += OnSegmentedControlSelectionChanged;
		/// void OnSegmentedControlSelectionChanged(object sender, Syncfusion.Maui.Buttons.SelectionChangedEventArgs e)
		/// {
		///    var oldValue = e.OldValue;
		///    var newValue = e.NewValue;
		///    int? oldIndex = e.OldIndex;
		///    int? newIndex = e.NewIndex;
		/// }
		/// ]]></code>
		/// </example>
		public event EventHandler<SelectionChangedEventArgs> SelectionChanged;

		/// <summary>
		/// Occurs when tapped on a segment item.
		/// </summary>
		/// <example>
		/// The following code demonstrates, how to use the SfSegmentedControl's tapped event.
		/// <code Lang="C#"><![CDATA[
		/// this.segmentedControl.Tapped += SegmentedControl_Tapped;
		/// private void SegmentedControl_Tapped(object sender, Syncfusion.Maui.Tolkit.SegmentTappedEventArgs e)
		/// {
		///    var tappedItem = e.TappedItem;
		/// }
		/// ]]></code>
		/// </example>
		public event EventHandler<SegmentTappedEventArgs> Tapped;
#nullable enable
		#endregion
	}
}