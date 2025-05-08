using System.Collections.Specialized;

#if MACCATALYST || IOS || WINDOWS
using Microsoft.Maui.Graphics;
#elif ANDROID
using System.Runtime.CompilerServices;
#endif

namespace Syncfusion.Maui.Toolkit.Carousel
{
	/// <summary>
	/// The <see cref="SfCarousel"/> class represents a control that allows user navigate through a collection of galleried items.
	/// </summary>
	/// <example>
	/// The below example shows how to initialize the <see cref="SfCarousel"/> with model and view model.
	/// # [C#](#tab/tabid-1)
	/// <code Lang="C#"><![CDATA[
	/// public class ImageModel
	/// {
	///     public string Image { get; set; }
	///     public ImageModel(string image)
	///     {
	///         Image = image;
	///     }
	/// }
	/// 
	/// public class GalleryViewModel
	/// {
	///     private ObservableCollection<ImageModel> imageCollection;
	///     public ObservableCollection<ImageModel> ImageCollection { get; set; }
	///     public GalleryViewModel()
	///     {
	///         ImageCollection = new ObservableCollection<ImageModel>()
	///         {
	///             new ImageModel("image1.png"),
	///             new ImageModel("image2.png"),
	///             new ImageModel("image3.png"),
	///         };
	///     }
	/// }
	/// ]]></code>
	/// ***
	/// # [XAML](#tab/tabid-1)
	/// <code Lang="XAML"><![CDATA[
	/// <ContentPage.BindingContext>
	///     <local:GalleryViewModel x:Name="galleryViewModel" />
	/// </ContentPage.BindingContext>
	/// <ContentPage.Resources>
	///     <ResourceDictionary>
	///         <DataTemplate x:Key="itemTemplate">
	///             <Grid>
	///                <Image x:Name="image" Source="{Binding Image}" />
	///             </Grid>
	///         </DataTemplate>
	///     </ResourceDictionary>
	/// </ContentPage.Resources>
	/// <ContentPage.Content>
	///     <carousel:SfCarousel
	///         x:Name="carousel"
	///         ItemTemplate="{x:StaticResource itemTemplate}"
	///         ItemsSource="{Binding ImageCollection}" />
	/// </ContentPage.Content>
	/// ]]></code>
	/// # [C#](#tab/tabid-2)
	/// <code Lang="C#"><![CDATA[
	/// public partial class MainPage : ContentPage
	/// {
	///     public MainPage()
	///     {
	///         InitializeComponent();
	///         GalleryViewModel galleryViewModel = new GalleryViewModel();
	///         this.BindingContext = galleryViewModel;
	///         var itemTemplate = new DataTemplate(() =>
	///         {
	///             var grid = new Grid();
	///             var image = new Image();
	///             image.SetBinding(Image.SourceProperty, "Image");
	///             grid.Children.Add(image);
	///             return grid;
	///         });
	///         SfCarousel carousel = new SfCarousel();
	///         carousel.ItemTemplate = itemTemplate;
	///         carousel.SetBinding(SfCarousel.ItemsSourceProperty, "ImageCollection");
	///         this.Content = carousel;
	///     }
	/// }
	/// ]]></code>
	/// ***
	/// </example>
	public partial class SfCarousel : View, ICarousel
	{
		#region Fields
#if WINDOWS || IOS || MACCATALYST
		private double _minHeight;

		private double _minWidth;
#endif
		#endregion

		#region Bindable Properties
		/// <summary>
		/// Identifies the <see cref="AllowLoadMore"/> bindable property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="AllowLoadMore"/> bindable property.
		/// </value>
		public static readonly BindableProperty AllowLoadMoreProperty = BindableProperty.Create(
			nameof(AllowLoadMore),
			typeof(bool),
			typeof(SfCarousel),
			false,
			BindingMode.Default,
			null,
			null);

		/// <summary>
		/// Identifies the <see cref="EnableVirtualization"/> bindable property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="EnableVirtualization"/> bindable property.
		/// </value>
		public static readonly BindableProperty EnableVirtualizationProperty = BindableProperty.Create(
			nameof(EnableVirtualization),
			typeof(bool), typeof(SfCarousel),
			false,
			BindingMode.Default,
			null,
			null);

		/// <summary>
		/// Identifies the <see cref="LoadMoreItemsCount"/> bindable property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="LoadMoreItemsCount"/> bindable property.
		/// </value>
		public static readonly BindableProperty LoadMoreItemsCountProperty = BindableProperty.Create(
			nameof(LoadMoreItemsCount),
			typeof(int),
			typeof(SfCarousel),
			3,
			BindingMode.Default,
			null,
			null);

		/// <summary>
		/// Identifies the <see cref="LoadMoreView"/> bindable property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="LoadMoreView"/> bindable property.
		/// </value>
		public static readonly BindableProperty LoadMoreViewProperty = BindableProperty.Create(
			nameof(LoadMoreView),
			typeof(View),
			typeof(SfCarousel),
			null,
			BindingMode.Default,
			null,
			null);

		/// <summary>
		/// Identifies the <see cref="SelectedIndex"/> bindable property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="SelectedIndex"/> bindable property.
		/// </value>
		public static readonly BindableProperty SelectedIndexProperty = BindableProperty.Create(
			nameof(SelectedIndex),
			typeof(int),
			typeof(SfCarousel),
			0,
			BindingMode.TwoWay,
			null,
			null);

		/// <summary>
		/// Identifies the <see cref="ItemsSource"/> bindable property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="ItemsSource"/> bindable property.
		/// </value>
		public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
			nameof(ItemsSource),
			typeof(IEnumerable<object>),
			typeof(SfCarousel),
			null,
			BindingMode.Default,
			null,
			propertyChanged: OnItemsSourcePropertyChanged);

		/// <summary>
		///Identifies the <see cref="ItemTemplate"/> bindable property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="ItemTemplate"/> bindable property.
		/// </value>
		public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(
			nameof(ItemTemplate),
			typeof(DataTemplate),
			typeof(SfCarousel),
			null,
			BindingMode.Default,
			null,
			null);

		/// <summary>
		/// Identifies the <see cref="ViewMode"/> bindable property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="ViewMode"/> bindable property.
		/// </value>
		public static readonly BindableProperty ViewModeProperty = BindableProperty.Create(
			nameof(ViewMode),
			typeof(ViewMode),
			typeof(SfCarousel),
			ViewMode.Default,
			BindingMode.Default,
			null,
			null);

		/// <summary>
		/// Identifies the <see cref="ItemSpacing"/> bindable property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="ItemSpacing"/> bindable property.
		/// </value>
		public static readonly BindableProperty ItemSpacingProperty = BindableProperty.Create(
			nameof(ItemSpacing),
			typeof(int),
			typeof(SfCarousel),
			12,
			BindingMode.Default,
			null,
			null);

		/// <summary>
		/// Identifies the <see cref="RotationAngle"/> bindable property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="RotationAngle"/> bindable property.
		/// </value>
		public static readonly BindableProperty RotationAngleProperty = BindableProperty.Create(
			nameof(RotationAngle),
			typeof(int),
			typeof(SfCarousel),
			45,
			BindingMode.Default,
			null,
			null);

		/// <summary>
		/// Identifies the <see cref="Duration"/> bindable property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="Duration"/> bindable property.
		/// </value>
		public static readonly BindableProperty DurationProperty = BindableProperty.Create(
			nameof(Duration),
			typeof(int),
			typeof(SfCarousel),
			600,
			BindingMode.Default,
			null,
			null);

		/// <summary>
		/// Identifies the <see cref="ScaleOffset"/> bindable property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="ScaleOffset"/> bindable property.
		/// </value>
		public static readonly BindableProperty ScaleOffsetProperty = BindableProperty.Create(
			nameof(ScaleOffset),
			typeof(float),
			typeof(SfCarousel),
			GetDefaultScale(),
			BindingMode.Default,
			null,
			null);

		/// <summary>
		/// Identifies the <see cref="Offset"/> bindable property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="Offset"/> bindable property.
		/// </value>
		public static readonly BindableProperty OffsetProperty = BindableProperty.Create(
			nameof(Offset),
			typeof(float),
			typeof(SfCarousel),
			GetDefaultOffset(),
			BindingMode.Default,
			null,
			null);

		/// <summary>
		/// Identifies the <see cref="SelectedItemOffset"/> bindable property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="SelectedItemOffset"/> bindable property.
		/// </value>
		public static readonly BindableProperty SelectedItemOffsetProperty = BindableProperty.Create(
			nameof(SelectedItemOffset),
			typeof(int),
			typeof(SfCarousel),
			GetDefaultSelectedOffset(),
			BindingMode.Default,
			null,
			null);

		/// <summary>
		/// Identifies the <see cref="ItemWidth"/> bindable property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="ItemWidth"/> bindable property.
		/// </value>
		public static readonly BindableProperty ItemWidthProperty = BindableProperty.Create(
			nameof(ItemWidth),
			typeof(int),
			typeof(SfCarousel),
			GetDefaultItemWidth(),
			BindingMode.Default,
			null,
			null);

		/// <summary>
		/// Identifies the <see cref="ItemHeight"/> bindable property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="ItemHeight"/> bindable property.
		/// </value>
		public static readonly BindableProperty ItemHeightProperty = BindableProperty.Create(
			nameof(ItemHeight),
			typeof(int),
			typeof(SfCarousel),
			GetDefaultItemHeight(),
			BindingMode.Default,
			null,
			null);

		/// <summary>
		/// Identifies the <see cref="EnableInteraction"/> bindable property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="EnableInteraction"/> bindable property.
		/// </value>
		public static readonly BindableProperty EnableInteractionProperty = BindableProperty.Create(
			nameof(EnableInteraction),
			typeof(bool),
			typeof(SfCarousel),
			true,
			BindingMode.Default,
			null,
			null);

		/// <summary>
		/// Identifies the <see cref="SwipeMovementMode"/> bindable property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="SwipeMovementMode"/> bindable property.
		/// </value>
		public static readonly BindableProperty SwipeMovementModeProperty = BindableProperty.Create(
			nameof(SwipeMovementMode),
			typeof(SwipeMovementMode),
			typeof(SfCarousel),
			SwipeMovementMode.MultipleItems,
			BindingMode.Default,
			null,
			null);

		#endregion

		#region Constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="SfCarousel"/> class.
		/// </summary>
		public SfCarousel()
		{
		}
		#endregion

		#region Events
		/// <summary>
		/// Triggered when an item is selected from the list of unselected items in the <see cref="SfCarousel"/>.
		/// </summary>
		public event EventHandler<Toolkit.Carousel.SelectionChangedEventArgs>? SelectionChanged;

		/// <summary>
		/// Triggered when a swipe gesture initiates an item selection in the <see cref="SfCarousel"/>.
		/// </summary>
		public event EventHandler<Toolkit.Carousel.SwipeStartedEventArgs>? SwipeStarted;

		/// <summary>
		/// Triggered when a swipe gesture is completed in the <see cref="SfCarousel"/>.
		/// </summary>
		public event EventHandler<EventArgs>? SwipeEnded;
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets a value indicating whether the carousel control is configured to load additional items dynamically.
		/// </summary>
		/// <value><c>true</c> if additional items can be loaded dynamically; otherwise, <c>false</c>.</value>
		/// <example>
		/// Below is an example of how to configure the <see cref="AllowLoadMore"/> property using XAML and C#:
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <carousel:SfCarousel
		///     x:Name="carousel"
		///     AllowLoadMore="True" />
		/// ]]></code>
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// SfCarousel carousel = new SfCarousel();
		/// carousel.AllowLoadMore = true;
		/// this.Content = carousel;
		/// ]]></code>
		/// ***
		/// </example>
		public bool AllowLoadMore
		{
			get { return (bool)GetValue(AllowLoadMoreProperty); }
			set { SetValue(AllowLoadMoreProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether the virtualization is enabled or not.
		/// </summary>
		/// <value><c>true</c> if virtualization is enabled; otherwise, <c>false</c>.</value>
		/// <remarks>This property is effective only when the carousel's view mode is set to Linear.</remarks>
		/// <example>
		/// Below is an example of how to set the <see cref="EnableVirtualization"/> property using XAML and C#:
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <carousel:SfCarousel
		///     x:Name="carousel"
		///     EnableVirtualization="True" />
		/// ]]></code>
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// SfCarousel carousel = new SfCarousel();
		/// carousel.EnableVirtualization = true;
		/// this.Content = carousel;
		/// ]]></code>
		/// ***
		/// </example>
		public bool EnableVirtualization
		{
			get { return (bool)GetValue(EnableVirtualizationProperty); }
			set { SetValue(EnableVirtualizationProperty, value); }
		}

		/// <summary>
		/// Gets or sets the number of additional items to load when the load more functionality is triggered in the carousel.
		/// </summary>
		/// <value>The default value is 3.</value>
		/// <example>
		/// Below is an example of how to configure the <see cref="LoadMoreItemsCount"/> property using XAML and C#:
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <carousel:SfCarousel
		///     x:Name="carousel"
		///     AllowLoadMore="True"
		///     LoadMoreItemsCount="2" />
		/// ]]></code>
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// SfCarousel carousel = new SfCarousel();
		/// carousel.AllowLoadMore = true;
		/// LoadMoreItemsCount = 2;
		/// this.Content = carousel;
		/// ]]></code>
		/// ***
		/// </example>
		public int LoadMoreItemsCount
		{
			get { return (int)GetValue(LoadMoreItemsCountProperty); }
			set { SetValue(LoadMoreItemsCountProperty, value); }
		}

		/// <summary>
		/// Gets or sets the index of the item that is currently selected and centered in the carousel control.
		/// </summary>
		/// <value>The default value is 0, indicating the first item.</value>
		/// <example>
		/// Below is an example of how to set the <see cref="SelectedIndex"/> property using XAML and C#:
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <carousel:SfCarousel
		///     x:Name="carousel"
		///     SelectedIndex="2" />
		/// ]]></code>
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// SfCarousel carousel = new SfCarousel();
		/// carousel.SelectedIndex = 2;
		/// this.Content = carousel;
		/// ]]></code>
		/// ***
		/// </example>
		public int SelectedIndex
		{
			get { return (int)GetValue(SelectedIndexProperty); }
			set { SetValue(SelectedIndexProperty, value); }
		}

		/// <summary>
		///  Gets or sets the collection of items to be displayed in the carousel, allowing each item to have a different view based on the template.
		/// </summary>
		/// <value>The default value is null, indicating no items are initially set.</value>
		/// <example>
		/// Below is an example of how to configure the <see cref="ItemsSource"/> property using XAML and C#:
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <carousel:SfCarousel
		///     x:Name="carousel"
		///     ItemsSource="{Binding ImageCollection}" />
		/// ]]></code>
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// SfCarousel carousel = new SfCarousel();
		/// carousel.SetBinding(SfCarousel.ItemsSourceProperty, "ImageCollection");
		/// this.Content = carousel;
		/// ]]></code>
		/// ***
		/// </example>
		public IEnumerable<object> ItemsSource
		{
			get { return (IEnumerable<object>)GetValue(ItemsSourceProperty); }
			set { SetValue(ItemsSourceProperty, value); }
		}

		/// <summary>
		/// Gets or sets the custom view displayed when additional items are loaded in the carousel, replacing the default "Load More" label.
		/// </summary>
		/// <value>The default value is null, indicating no custom view is set initially.</value>
		/// <example>
		/// Below is an example of how to configure the <see cref="LoadMoreView"/> property using XAML and C#:
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <carousel:SfCarousel
		///     x:Name="carousel"
		///     AllowLoadMore="True"
		///     LoadMoreItemsCount="2">
		///     <carousel:SfCarousel.LoadMoreView>
		///         <Border Stroke="LightGray" StrokeThickness="1" StrokeShape="RoundRectangle 8">
		///             <Label
		///                 HorizontalTextAlignment="Center"
		///                 Text="Load More View"
		///                 VerticalTextAlignment="Center" />
		///         </Border>
		///     </carousel:SfCarousel.LoadMoreView>
		/// </carousel:SfCarousel>
		/// ]]></code>
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// SfCarousel carousel = new SfCarousel();
		/// carousel.AllowLoadMore = true;
		/// carousel.LoadMoreItemsCount = 2;
		/// carousel.LoadMoreView = new Border()
		/// {
		///     Content = new Label() { Text = "Load More View", HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center },
		///     Stroke = Colors.LightGray,
		///     StrokeThickness = 1,
		///     StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle() { CornerRadius = 8 },
		/// };
		/// this.Content = carousel;
		/// ]]></code>
		/// ***
		/// </example>
		public View LoadMoreView
		{
			get { return (View)GetValue(LoadMoreViewProperty); }
			set { SetValue(LoadMoreViewProperty, value); }
		}

		/// <summary>
		/// Gets or sets the data template that defines the visual representation of each item in the carousel.
		/// </summary>
		/// <value>The default value is null, indicating no template is set initially.</value>
		/// <example>
		/// Below is an example of how to configure the <see cref="ItemTemplate"/> property using XAML and C#:
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <carousel:SfCarousel
		///		x:Name="carousel">
		///		<carousel:SfCarousel.ItemTemplate>
		///			<DataTemplate>
		///				<Image Aspect = "Fill"
		///						HeightRequest="100"
		///						Source="{Binding ImageName}"
		///						WidthRequest="100" />
		///			</DataTemplate>
		///		</carousel:SfCarousel.ItemTemplate>
		/// </carousel:SfCarousel>
		/// ]]></code>
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// var itemTemplate = new DataTemplate(() =>
		///	{
		///		var grid = new Grid();
		///		var nameLabel = new Image();
		///		nameLabel.SetBinding(Image.SourceProperty, "ImageName");
		///		grid.Children.Add(nameLabel);
		///		return grid;
		///	});
		/// SfCarousel carousel = new SfCarousel();
		/// carousel.ItemTemplate = itemTemplate;
		/// this.Content = carousel;
		/// ]]></code>
		/// ***
		/// </example>
		public DataTemplate ItemTemplate
		{
			get { return (DataTemplate)GetValue(ItemTemplateProperty); }
			set { SetValue(ItemTemplateProperty, value); }
		}

		/// <summary>
		/// Gets or sets the mode that determines whether the carousel items are arranged in a default 3D layout or a linear horizontal layout.
		/// </summary>
		/// <value>The default value is "Default", indicating the 3D view mode..</value>
		/// <example>
		/// Below is an example of how to configure the <see cref="ViewMode"/> property using XAML and C#:
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <carousel:SfCarousel
		///     x:Name="carousel"
		///     ViewMode="Linear" />
		/// ]]></code>
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// SfCarousel carousel = new SfCarousel();
		/// carousel.ViewMode = ViewMode.Linear;
		/// this.Content = carousel;
		/// ]]></code>
		/// ***
		/// </example>
		public ViewMode ViewMode
		{
			get { return (ViewMode)GetValue(ViewModeProperty); }
			set { SetValue(ViewModeProperty, value); }
		}

		/// <summary>
		/// Gets or sets the spacing value between items in the carousel. This property adjusts the space separating each item.
		/// </summary>
		/// <value>The default value is 12.</value>
		/// <remarks>This property is effective only when the view mode is set to linear.</remarks>
		/// <example>
		/// Below is an example of how to configure the <see cref="ItemSpacing"/> property using XAML and C#:
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <carousel:SfCarousel
		///     x:Name="carousel"
		///     ItemSpacing="10" />
		/// ]]></code>
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// SfCarousel carousel = new SfCarousel();
		/// carousel.ItemSpacing = 10;
		/// this.Content = carousel;
		/// ]]></code>
		/// ***
		/// </example>
		public int ItemSpacing
		{
			get { return (int)GetValue(ItemSpacingProperty); }
			set { SetValue(ItemSpacingProperty, value); }
		}

		/// <summary>
		/// Gets or sets the angle at which unselected items are tilted in the carousel.
		/// </summary>
		/// <value>The default value is 45 degrees.</value>
		/// <remarks>This property is effective only when the view mode is set to the default mode.</remarks>
		/// <example>
		/// Below is an example of how to configure the <see cref="RotationAngle"/> property using XAML and C#:
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <carousel:SfCarousel
		///     x:Name="carousel"
		///     RotationAngle="30" />
		/// ]]></code>
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// SfCarousel carousel = new SfCarousel();
		/// carousel.RotationAngle = 30;
		/// this.Content = carousel;
		/// ]]></code>
		/// ***
		/// </example>
		public int RotationAngle
		{
			get { return (int)GetValue(RotationAngleProperty); }
			set { SetValue(RotationAngleProperty, value); }
		}

		/// <summary>
		/// Gets or sets the spacing between each item in the carousel when the view mode is set to default, allowing for visual separation.
		/// </summary>
		/// <value>The default value is 40f for desktop and 18f for mobile devices.</value>
		/// <remarks>This property is effective only in the default view mode.</remarks>
		/// <example>
		/// Below is an example of how to configure the <see cref="Offset"/> property using XAML and C#:
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <carousel:SfCarousel
		///     x:Name="carousel"
		///     Offset="10" />
		/// ]]></code>
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// SfCarousel carousel = new SfCarousel();
		/// carousel.Offset = 10;
		/// this.Content = carousel;
		/// ]]></code>
		/// ***
		/// </example>
		public float Offset
		{
			get { return (float)GetValue(OffsetProperty); }
			set { SetValue(OffsetProperty, value); }
		}

		/// <summary>
		/// Gets or sets the scaling ratio that differentiates unselected items in the carousel when the view mode is set to default.
		/// </summary>
		/// <value>The default value is 0.8f for desktop and 0.7f for mobile devices.</value>
		/// <remarks>This property is effective only in the default view mode.</remarks>
		/// <example>
		/// Below is an example of how to configure the <see cref="ScaleOffset"/> property using XAML and C#:
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <carousel:SfCarousel
		///     x:Name="carousel"
		///     ScaleOffset="0.7" />
		/// ]]></code>
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// SfCarousel carousel = new SfCarousel();
		/// carousel.ScaleOffset = 0.7;
		/// this.Content = carousel;
		/// ]]></code>
		/// ***
		/// </example>
		public float ScaleOffset
		{
			get { return (float)GetValue(ScaleOffsetProperty); }
			set { SetValue(ScaleOffsetProperty, value); }
		}

		/// <summary>
		/// Gets or sets the space between the selected item and unselected items in the carousel when the view mode is set to default.
		/// </summary>
		/// <value>The default value is 20 for Windows and 40 for other platforms.</value>
		/// <remarks>This property is effective only in the default view mode.</remarks>
		/// <example>
		/// Below is an example of how to configure the <see cref="SelectedItemOffset"/> property using XAML and C#:
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <carousel:SfCarousel
		///     x:Name="carousel"
		///     SelectedItemOffset="30" />
		/// ]]></code>
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// SfCarousel carousel = new SfCarousel();
		/// carousel.SelectedItemOffset = 30;
		/// this.Content = carousel;
		/// ]]></code>
		/// ***
		/// </example>
		public int SelectedItemOffset
		{
			get { return (int)GetValue(SelectedItemOffsetProperty); }
			set { SetValue(SelectedItemOffsetProperty, value); }
		}

		/// <summary>
		/// Gets or sets the duration for the selection animation in the carousel, allowing you to adjust the delay of the effect.
		/// </summary>
		/// <value>The default value is 600 milliseconds.</value>
		/// <example>
		/// Below is an example of how to configure the <see cref="Duration"/> property using XAML and C#:
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <carousel:SfCarousel
		///     x:Name="carousel"
		///     Duration="300" />
		/// ]]></code>
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// SfCarousel carousel = new SfCarousel();
		/// carousel.Duration = 300;
		/// this.Content = carousel;
		/// ]]></code>
		/// ***
		/// </example>
		public int Duration
		{
			get { return (int)GetValue(DurationProperty); }
			set { SetValue(DurationProperty, value); }
		}

		/// <summary>
		/// Gets or sets the width of each item in the carousel, allowing customization to suit design specifications.
		/// </summary>
		/// <value>The default value is 250 for desktop platforms and 200 for mobile devices.</value>
		/// <example>
		/// Below is an example of how to configure the <see cref="ItemWidth"/> property using XAML and C#:
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <carousel:SfCarousel
		///     x:Name="carousel"
		///     ItemWidth="150" />
		/// ]]></code>
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// SfCarousel carousel = new SfCarousel();
		/// carousel.ItemWidth = 150;
		/// this.Content = carousel;
		/// ]]></code>
		/// ***
		/// </example>
		public int ItemWidth
		{
			get { return (int)GetValue(ItemWidthProperty); }
			set { SetValue(ItemWidthProperty, value); }
		}

		/// <summary>
		/// Gets or sets the height of each item in the carousel, allowing customization to suit design specifications.
		/// </summary>
		/// <value>The default value is 350 for desktop platforms and 300 for mobile devices.</value>
		/// <example>
		/// Below is an example of how to configure the <see cref="ItemHeight"/> property using XAML and C#:
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <carousel:SfCarousel
		///     x:Name="carousel"
		///     ItemHeight="150" />
		/// ]]></code>
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// SfCarousel carousel = new SfCarousel();
		/// carousel.ItemHeight = 150;
		/// this.Content = carousel;
		/// ]]></code>
		/// ***
		/// </example>
		public int ItemHeight
		{
			get { return (int)GetValue(ItemHeightProperty); }
			set { SetValue(ItemHeightProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether user interaction with the carousel is enabled or disabled.
		/// </summary>
		/// /// <value><c>true</c> if interaction is enabled; otherwise, <c>false</c>.</value>
		/// <example>
		/// Below is an example of how to configure the <see cref="EnableInteraction"/> property using XAML and C#:
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <carousel:SfCarousel
		///     x:Name="carousel"
		///     EnableInteraction="false" />
		/// ]]></code>
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// SfCarousel carousel = new SfCarousel();
		/// carousel.EnableInteraction = false;
		/// this.Content = carousel;
		/// ]]></code>
		/// ***
		/// </example>
		public bool EnableInteraction
		{
			get { return (bool)GetValue(EnableInteractionProperty); }
			set { SetValue(EnableInteractionProperty, value); }
		}

		/// <summary>
		/// Gets or sets the scroll mode for the carousel, determining whether items are scrolled individually or in groups.
		/// </summary>
		/// <example>
		///  Below is an example of how to configure the <see cref="SwipeMovementMode"/> property using XAML and C#:
		/// # [XAML](#tab/tabid-1)
		/// <code Lang="XAML"><![CDATA[
		/// <carousel:SfCarousel
		///     x:Name="carousel"
		///     SwipeMovementMode="MultipleItems" />
		/// ]]></code>
		/// # [C#](#tab/tabid-2)
		/// <code Lang="C#"><![CDATA[
		/// SfCarousel carousel = new SfCarousel();
		/// carousel.SwipeMovementMode = SwipeMovementMode.MultipleItems;
		/// this.Content = carousel;
		/// ]]></code>
		/// ***
		/// </example>
		public SwipeMovementMode SwipeMovementMode
		{
			get { return (SwipeMovementMode)GetValue(SwipeMovementModeProperty); }
			set { SetValue(SwipeMovementModeProperty, value); }
		}
		#endregion

		#region Methods

		#region Private Methods
		/// <summary>
		/// Determines the default scale factor for unselected items based on the platform.
		/// </summary>
		/// <returns>
		/// Returns 0.8f for Windows and Mac Catalyst platforms, and 0.7f for other platforms.
		/// </returns>
		private static float GetDefaultScale()
		{
#if WINDOWS || MACCATALYST
			return 0.8f;
#else
			return 0.7f;
#endif
		}

		/// <summary>
		/// Retrieves the default offset value based on the platform.
		/// </summary>
		/// <returns>
		/// Returns 40f for Windows and Mac Catalyst platforms, and 18f for other platforms.
		/// </returns>
		private static float GetDefaultOffset()
		{
#if WINDOWS || MACCATALYST
			return 40f;
#else
			return 18f;
#endif
		}

		/// <summary>
		/// Retrieves the default offset for the selected item based on the platform.
		/// </summary>
		/// <returns>
		/// Returns 20 for Windows platforms and 40 for other platforms.
		/// </returns>
		private static int GetDefaultSelectedOffset()
		{
#if WINDOWS
			return 20;
#else
			return 40;
#endif
		}

		/// <summary>
		/// Retrieves the default width for carousel items based on the platform.
		/// </summary>
		/// <returns>
		/// Returns 250 for Windows and Mac Catalyst platforms, and 200 for other platforms.
		/// </returns>
		private static int GetDefaultItemWidth()
		{
#if WINDOWS || MACCATALYST
			return 250;
#else
			return 200;
#endif
		}

		/// <summary>
		/// Retrieves the default height for carousel items based on the platform.
		/// </summary>
		/// <returns>
		/// Returns 350 for Windows and Mac Catalyst platforms, and 300 for other platforms.
		/// </returns>
		private static int GetDefaultItemHeight()
		{
#if WINDOWS || MACCATALYST
			return 350;
#else
			return 300;
#endif
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Advances the carousel selection from right to left, incrementing the selected index by one.
		/// </summary>
		public void MoveNext()
		{
			Handler?.Invoke(nameof(ICarousel.MoveNext));
		}

		/// <summary>
		/// Moves the carousel selection from left to right, decrementing the selected index by one.
		/// </summary>
		public void MovePrevious()
		{
			Handler?.Invoke(nameof(ICarousel.MovePrevious));
		}

		/// <summary>
		/// Triggers the loading of additional items into the carousel.
		/// </summary>
		public void LoadMore()
		{
			Handler?.Invoke(nameof(ICarousel.LoadMore));
		}
		#endregion

#if ANDROID
		/// <summary>
		/// Overrides the OnPropertyChanged method to handle changes to the "IsEnabled" property.
		/// </summary>
		/// <exclude/>
		/// <param name="propertyName">The name of the property that changed.</param>
		protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
		{
			if (propertyName != null && propertyName.Equals("IsEnabled", StringComparison.Ordinal))
			{
				if (Handler != null && Handler.PlatformView is PlatformCarousel platformCarousel)
				{
					platformCarousel.IsEnabled = IsEnabled;
				}
			}

			base.OnPropertyChanged(propertyName);
		}
#endif

#if WINDOWS || MACCATALYST || IOS
		#region Override Methods
		/// <summary>
		/// Handles size changes for the carousel, adjusting layout specifics based on platform requirements.
		/// </summary>
		/// <exclude/>
		/// <param name="width">The new width allocated to the carousel.</param>
		/// <param name="height">The new height allocated to the carousel.</param>
		protected override void OnSizeAllocated(double width, double height)
		{
			base.OnSizeAllocated(width, height);
#if WINDOWS
			if (Handler != null && Handler.PlatformView is PlatformCarousel platformCarousel)
			{
				if (platformCarousel.ViewMode == ViewMode.Default)
				{
					platformCarousel.ViewMode = ViewMode.Default;
				}
			}
			UpdateClip(DesiredSize);
#else
			if (Handler != null && Handler.PlatformView is PlatformCarousel carousel)
			{
				double x = carousel.Frame.X;
				double y = carousel.Frame.Y;

				if (HeightRequest <= 0 || WidthRequest <= 0)
				{
					carousel.Frame = new CoreGraphics.CGRect(x, y, width, height);
				}
			}
#endif
		}

		/// <summary>
		/// Calculates the desired size of the carousel during the measure pass of a layout cycle based on the provided constraints.
		/// </summary>
		/// <exclude/>
		/// <param name="widthConstraint">The available width that the element can occupy.</param>
		/// <param name="heightConstraint">The available height that the element can occupy.</param>
		/// <returns>A <see cref="Size"/> structure representing the desired size for the element.</returns>
		protected override Size MeasureOverride(double widthConstraint, double heightConstraint)
		{
#if IOS || MACCATALYST
			return MeasureOverrideForIOS(widthConstraint, heightConstraint);
#else
			return MeasureOverrideForOtherPlatforms(widthConstraint, heightConstraint);
#endif
		}
		#endregion
		#region Private Methods
#if IOS || MACCATALYST

		/// <summary>
		/// Provides a specialized implementation of the measure pass for iOS and MacCatalyst platforms, calculating the desired size based on layout constraints.
		/// </summary>
		/// <param name="widthConstraint">The maximum width that the element can use.</param>
		/// <param name="heightConstraint">The maximum height that the element can use.</param>
		/// <returns>A <see cref="Size"/> structure representing the calculated desired size.</returns>
		private Size MeasureOverrideForIOS(double widthConstraint, double heightConstraint)
		{
			InitializeConstraints(ref widthConstraint, ref heightConstraint);
			DesiredSize = new Size(widthConstraint, heightConstraint);

			if (Parent is StackLayout stackLayout)
			{
				DesiredSize = MeasureForStackLayout(stackLayout, widthConstraint, heightConstraint, _minWidth, _minHeight);
			}
			else if (Parent is ScrollView scrollView)
			{
				DesiredSize = MeasureForScrollView(scrollView, widthConstraint, heightConstraint, _minWidth, _minHeight);
			}

			UpdateClip(DesiredSize);
			return DesiredSize;
		}
#else
		/// <summary>
		/// Provides a specialized implementation of the measure pass for platforms other than iOS and MacCatalyst, calculating the desired size based on layout constraints.
		/// </summary>
		/// <param name="widthConstraint">The maximum width available for the element.</param>
		/// <param name="heightConstraint">The maximum height available for the element.</param>
		/// <returns>A <see cref="Size"/> structure representing the calculated desired size.</returns>
		private Size MeasureOverrideForOtherPlatforms(double widthConstraint, double heightConstraint)
		{
			base.MeasureOverride(widthConstraint, heightConstraint);
			InitializeConstraints(ref widthConstraint, ref heightConstraint);
			DesiredSize = new Size(widthConstraint, heightConstraint);
			if (Parent is HorizontalStackLayout)
			{
				DesiredSize = MeasureForHorizontalLayout(heightConstraint);
			}
			else if (Parent is StackLayout stackLayout)
			{
				DesiredSize = MeasureForStackLayout(stackLayout, widthConstraint, heightConstraint, ItemWidth, ItemHeight);
			}
			else if (Parent is VerticalStackLayout)
			{
				DesiredSize = MeasureForVerticalLayout(widthConstraint);
			}
			else if (Parent is ScrollView scrollView)
			{
				DesiredSize = MeasureForScrollView(scrollView, widthConstraint, heightConstraint, ItemWidth, ItemHeight);
			}

			return DesiredSize;
		}
#endif

		/// <summary>
		/// Initializes the width and height constraints for the carousel items, ensuring they have valid size boundaries.
		/// </summary>
		/// <param name="widthConstraint">A reference to the available width constraint that will be updated as necessary.</param>
		/// <param name="heightConstraint">A reference to the available height constraint that will be updated as necessary.</param>
		private void InitializeConstraints(ref double widthConstraint, ref double heightConstraint)
		{
			_minHeight = ItemHeight;
			_minWidth = ItemWidth;

			widthConstraint = double.IsNaN(widthConstraint) || double.IsInfinity(widthConstraint) ? _minWidth : widthConstraint;
			heightConstraint = double.IsNaN(heightConstraint) || double.IsInfinity(heightConstraint) ? _minHeight : heightConstraint;

			if (HeightRequest > 0)
			{
				heightConstraint = HeightRequest;
			}

			if (WidthRequest > 0)
			{
				widthConstraint = WidthRequest;
			}
		}

#if !IOS && !MACCATALYST
		/// <summary>
		/// Calculates the size requirement for an element within a <see cref="HorizontalStackLayout"/> parent.
		/// </summary>
		/// <param name="heightConstraint">The maximum height available for the element.</param>
		/// <returns>A <see cref="Size"/> structure representing the calculated size based on the constraints and requests.</returns>
		Size MeasureForHorizontalLayout(double heightConstraint) => new(WidthRequest >= 0 ? WidthRequest : ItemWidth, HeightRequest >= 0 && WidthRequest >= 0 ? HeightRequest : heightConstraint);
		/// <summary>
		/// Calculates the size of a VerticalStackLayout based on given width and height constraints.
		/// Takes into account specified width and height requests, defaulting to constraints or item height as needed.
		/// </summary>
		/// <param name="widthConstraint">The available width for the layout.</param>
		/// <returns>A Size object representing the dimensions of the layout.</returns>
		Size MeasureForVerticalLayout(double widthConstraint)
		{
			return new Size(HeightRequest >= 0 && WidthRequest >= 0 ? WidthRequest : widthConstraint, HeightRequest >= 0 ? HeightRequest : ItemHeight);
		}
#endif

		/// <summary>
		/// Updates the clipping region of a BoxView using the specified dimensions.
		/// This is achieved by setting a rectangular clipping path based on the provided size.
		/// </summary>
		/// <param name="size">The dimensions used to define the clipping rectangle.</param>
		private void UpdateClip(Microsoft.Maui.Graphics.Size size)
		{
			var clipPath = new Microsoft.Maui.Controls.Shapes.RectangleGeometry
			{
				Rect = new Rect(0, 0, size.Width, size.Height) // X, Y, Width, Height
			};

			// Set the Clip property on the BoxView
			Clip = clipPath;
		}

		/// <summary>
		/// Calculates the size for a StackLayout based on its orientation and given constraints,
		/// taking into account specified width and height requests as well as minimum dimensions.
		/// </summary>
		/// <param name="stackLayout">The StackLayout whose size is being measured.</param>
		/// <param name="widthConstraint">The available width for the layout.</param>
		/// <param name="heightConstraint">The available height for the layout.</param>
		/// <param name="minWidth">The minimum width the layout should occupy.</param>
		/// <param name="minHeight">The minimum height the layout should occupy.</param>
		/// <returns>A Size object representing the dimensions of the layout.</returns>
		private Size MeasureForStackLayout(StackLayout stackLayout, double widthConstraint, double heightConstraint, double minWidth, double minHeight)
		{
			if (stackLayout.Orientation == StackOrientation.Horizontal)
			{
				return new Size(WidthRequest >= 0 ? WidthRequest : minWidth, HeightRequest >= 0 && WidthRequest >= 0 ? HeightRequest : heightConstraint);
			}
			else
			{
				return new Size(HeightRequest >= 0 && WidthRequest >= 0 ? WidthRequest : widthConstraint, HeightRequest >= 0 ? HeightRequest : minHeight);
			}
		}

		/// <summary>
		/// Determines the size for a ScrollView based on its orientation and given constraints,
		/// considering specified width and height requests along with minimum dimensions.
		/// </summary>
		/// <param name="scrollView">The ScrollView whose size is being measured.</param>
		/// <param name="widthConstraint">The maximum width available for the ScrollView.</param>
		/// <param name="heightConstraint">The maximum height available for the ScrollView.</param>
		/// <param name="minWidth">The minimum width the ScrollView should occupy.</param>
		/// <param name="minHeight">The minimum height the ScrollView should occupy.</param>
		/// <returns>A Size object representing the determined dimensions for the ScrollView.</returns>
		private Size MeasureForScrollView(ScrollView scrollView, double widthConstraint, double heightConstraint, double minWidth, double minHeight)
		{
			if (scrollView.Orientation == Microsoft.Maui.ScrollOrientation.Horizontal)
			{
				return new Size(WidthRequest >= 0 ? WidthRequest : minWidth, HeightRequest >= 0 && WidthRequest >= 0 ? HeightRequest : heightConstraint);
			}
			else
			{
				return new Size(HeightRequest >= 0 && WidthRequest >= 0 ? WidthRequest : widthConstraint, HeightRequest >= 0 ? HeightRequest : minHeight);
			}
		}
		#endregion
#endif

		#region Private Methods
		/// <summary>
		/// Invoked when the ItemsSource property of a bindable object is changed.
		/// Updates the property and initiates necessary changes within the carousel.
		/// </summary>
		/// <param name="bindable">The object with the bindable property that has changed.</param>
		/// <param name="oldValue">The previous value of the ItemsSource property.</param>
		/// <param name="newValue">The new value assigned to the ItemsSource property.</param>
		private static void OnItemsSourcePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfCarousel carousel)
			{
				carousel.OnPropertyChanged(ItemsSourceProperty.PropertyName);
				carousel.ChangeCollection();
			}
		}

		/// <summary>
		/// Sets up event handlers to respond to changes in the ItemsSource collection.
		/// Ensures the event handler is attached only once by first removing any existing handler.
		/// </summary>
		private void ChangeCollection()
		{
			if (ItemsSource is INotifyCollectionChanged itemsource)
			{
				itemsource.CollectionChanged -= Itemsource_CollectionChanged;
				itemsource.CollectionChanged += Itemsource_CollectionChanged;
			}
		}

		/// <summary>
		/// Handles changes in the ItemsSource collection and notifies the system of property changes.
		/// </summary>
		/// <param name="sender">The source of the event, typically the modified collection.</param>
		/// <param name="e">Event arguments containing information about the collection change.</param>
		private void Itemsource_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			OnPropertyChanged(nameof(ItemsSource));
		}

		/// <summary>
		/// Raises the SelectionChanged event within the <see cref="SfCarousel"/> control.
		/// Invokes the event, if any handlers are subscribed, passing the relevant event arguments.
		/// </summary>
		/// <exclude/>
		/// <param name="args">The arguments for the SelectionChanged event, containing details about the selection change.</param>
		void ICarousel.RaiseSelectionChanged(Toolkit.Carousel.SelectionChangedEventArgs args)
		{
			SelectionChanged?.Invoke(this, args);
		}

		/// <summary>
		/// Triggers the SwipeStarted event within the <see cref="SfCarousel"/> control.
		/// This method invokes the event, providing the associated event arguments to any subscribed handlers.
		/// </summary>
		/// <exclude/>
		/// <param name="args">The arguments for the SwipeStarted event, detailing the swipe initiation.</param>
		void ICarousel.RaiseSwipeStarted(Toolkit.Carousel.SwipeStartedEventArgs args)
		{
			SwipeStarted?.Invoke(this, args);
		}

		/// <summary>
		/// Triggers the SwipeEnded event within the <see cref="SfCarousel"/> control.
		/// This method raises the event, passing the relevant event arguments to any subscribed handlers.
		/// </summary>
		/// <exclude/>
		/// <param name="args">The event arguments for the SwipeEnded event, providing context for the swipe completion.</param>
		void ICarousel.RaiseSwipeEnded(EventArgs args)
		{
			SwipeEnded?.Invoke(this, args);
		}
		#endregion
		#endregion
	}
}
