using System.Collections.ObjectModel;

namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// ZoomPanBehavior enables zooming and panning operations over a cartesian chart.
	/// </summary>
	/// <remarks>
	/// To enable the zooming and panning in the chart, create an instance of <see cref="ChartZoomPanBehavior"/> and set it to the <see cref="SfCartesianChart.ZoomPanBehavior"/> property of <see cref="SfCartesianChart"/>.
	/// </remarks>
	/// <example>
	/// # [Xaml](#tab/tabid-1)
	/// <code><![CDATA[
	///     <chart:SfCartesianChart>
	///
	///           <chart:SfCartesianChart.ZoomPanBehavior>
	///               <chart:ChartZoomPanBehavior />
	///           </chart:SfCartesianChart.ZoomPanBehavior>
	///           
	///     </chart:SfCartesianChart>
	/// ]]></code>
	/// # [C#](#tab/tabid-2)
	/// <code><![CDATA[
	///     SfCartesianChart chart = new SfCartesianChart();
	///     
	///     chart.ZoomPanBehavior = new ChartZoomPanBehavior();
	///     
	/// ]]></code>
	/// ***
	/// </example>
	public partial class ChartZoomPanBehavior : ChartBehavior
	{
		#region Fields

		GestureStatus _pinchStatus = GestureStatus.Canceled;
		bool _isPinchZoomingActivated;
		bool _isSelectionActivated;
		double _cumulativeZoomLevel = 1;
		float _startX;
		float _startY;
		float _endX;
		float _endY;
		Rect _currentSelectionRect;
		bool _zoomCancel;
		bool _isDoubleTapped;

#if MACCATALYST || IOS
		bool _isScrollChanged;
		bool _isSelected;
		bool _isPinchZoomed;
		Rect _selectedRegionRect;
		DateTime _startTime;
		int _downEntered;
		DateTime _endTime;
#else
		bool _isSelectionZoomingEnded;
#endif

		#endregion

		#region Internal Properties

		internal SfCartesianChart? Chart { get; set; }
		internal bool IsSelectionZoomingActivated { get; set; }
		internal Rect SelectionRect { get; set; }

		#endregion

		#region Bindable Properties

		/// <summary>
		/// Identifies the <see cref="EnablePinchZooming"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="EnablePinchZooming"/> bindable property determines whether
		/// pinch zooming is enabled for the chart.
		/// </remarks>
		public static readonly BindableProperty EnablePinchZoomingProperty = BindableProperty.Create(
			nameof(EnablePinchZooming),
			typeof(bool),
			typeof(ChartZoomPanBehavior),
			true,
			BindingMode.Default,
			null,
			OnEnableZoomingPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="EnablePanning"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="EnablePanning"/> bindable property determines whether
		/// panning is enabled for the chart.
		/// </remarks>
		public static readonly BindableProperty EnablePanningProperty = BindableProperty.Create(
			nameof(EnablePanning),
			typeof(bool),
			typeof(ChartZoomPanBehavior),
			true,
			BindingMode.Default,
			null,
			OnEnablePanningPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="EnableDoubleTap"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="EnableDoubleTap"/> bindable property determines whether
		/// double-tap zooming is enabled for the chart.
		/// </remarks>
		public static readonly BindableProperty EnableDoubleTapProperty = BindableProperty.Create(
			nameof(EnableDoubleTap),
			typeof(bool),
			typeof(ChartZoomPanBehavior),
			true,
			BindingMode.Default,
			null,
			OnEnableDoubleTapPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="ZoomMode"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="ZoomMode"/> bindable property determines the zoom mode
		/// of the chart, which in turn defines the direction of zooming.
		/// </remarks>
		public static readonly BindableProperty ZoomModeProperty = BindableProperty.Create(
			nameof(ZoomMode),
			typeof(ZoomMode),
			typeof(ChartZoomPanBehavior),
			ZoomMode.XY,
			BindingMode.Default,
			null,
			OnZoomModePropertyChanged);

		/// <summary>
		/// Identifies the <see cref="EnableSelectionZooming"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="EnableSelectionZooming"/> bindable property determines whether
		/// selection zooming is enabled for the chart.
		/// </remarks>
		public static readonly BindableProperty EnableSelectionZoomingProperty = BindableProperty.Create(
			nameof(EnableSelectionZooming),
			typeof(bool),
			typeof(ChartZoomPanBehavior),
			false,
			BindingMode.Default,
			null);

		/// <summary>
		/// Identifies the <see cref="SelectionRectStroke"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="SelectionRectStroke"/> bindable property determines the stroke
		/// color of the selection rectangle.
		/// </remarks>
		public static readonly BindableProperty SelectionRectStrokeProperty = BindableProperty.Create(
			nameof(SelectionRectStroke),
			typeof(Brush),
			typeof(ChartZoomPanBehavior),
			null,
			BindingMode.Default,
			null,
			defaultValueCreator: SelectionRectStrokeDefaultValueCreator);

		/// <summary>
		/// Identifies the <see cref="SelectionRectStrokeWidth"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="SelectionRectStrokeWidth"/> bindable property determines the stroke
		/// width of the selection rectangle.
		/// </remarks>
		public static readonly BindableProperty SelectionRectStrokeWidthProperty = BindableProperty.Create(
			nameof(SelectionRectStrokeWidth),
			typeof(double),
			typeof(ChartZoomPanBehavior),
			1d,
			BindingMode.Default,
			null);

		/// <summary>
		/// Identifies the <see cref="SelectionRectFill"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="SelectionRectFill"/> bindable property determines the fill
		/// color of the selection rectangle.
		/// </remarks>
		public static readonly BindableProperty SelectionRectFillProperty = BindableProperty.Create(
			nameof(SelectionRectFill),
			typeof(Brush),
			typeof(ChartZoomPanBehavior),
			null,
			BindingMode.Default,
			null,
			defaultValueCreator: SelectionRectFillDefaultValueCreator);

		/// <summary>
		/// Identifies the <see cref="SelectionRectStrokeDashArray"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="SelectionRectStrokeDashArray"/> bindable property determines the dash
		/// array of the selection rectangle's stroke.
		/// </remarks>
		public static readonly BindableProperty SelectionRectStrokeDashArrayProperty = BindableProperty.Create(
			nameof(SelectionRectStrokeDashArray),
			typeof(DoubleCollection),
			typeof(ChartZoomPanBehavior),
			new DoubleCollection { 5, 5 },
			BindingMode.Default,
			null);

		/// <summary>
		/// Identifies the <see cref="EnableDirectionalZooming"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="EnableDirectionalZooming"/> bindable property determines whether
		/// directional zooming is enabled for the chart.
		/// </remarks>
		public static readonly BindableProperty EnableDirectionalZoomingProperty = BindableProperty.Create(
			nameof(EnableDirectionalZooming),
			typeof(bool),
			typeof(ChartZoomPanBehavior),
			false,
			BindingMode.Default,
			null);

		/// <summary>
		/// Identifies the <see cref="MaximumZoomLevel"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="MaximumZoomLevel"/> bindable property determines the maximum
		/// zoom level for the chart.
		/// </remarks>
		public static readonly BindableProperty MaximumZoomLevelProperty = BindableProperty.Create(
			nameof(MaximumZoomLevel),
			typeof(double),
			typeof(ChartZoomPanBehavior),
			double.NaN,
			BindingMode.Default,
			null,
			OnMaximumZoomLevelPropertyChanged);

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets a value indicating whether the finger gesture is enabled or disabled.
		/// </summary>
		/// <value>
		/// It accepts the <c>bool</c> values and its default value is <c>true</c>.
		/// </value>
		/// <remarks>
		/// If this property is true, zooming is performed based on the pinch gesture of the user. If this property is false, zooming is performed based on the mouse wheel of the user.
		/// </remarks>
		/// <example>
		/// # [MainPage.xaml](#tab/tabid-3)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		///
		///     <!--omitted for brevity-->
		///
		///     <chart:SfCartesianChart.ZoomPanBehavior>
		///        <chart:ChartZoomPanBehavior EnablePinchZooming="True"/>
		///     </chart:SfCartesianChart.ZoomPanBehavior>
		///
		///     <chart:LineSeries ItemsSource="{Binding Data}"
		///                       XBindingPath="XValue"
		///                       YBindingPath="YValue"/>
		/// 
		/// </chart:SfCartesianChart>
		///
		/// ]]>
		/// </code>
		/// # [MainPage.xaml.cs](#tab/tabid-4)
		/// <code><![CDATA[
		/// SfCartesianChart chart = new SfCartesianChart();
		/// ViewModel viewModel = new ViewModel();
		/// 
		/// // omitted for brevity
		/// chart.ZoomPanBehavior = new ChartZoomPanBehavior() 
		/// { 
		///    EnablePinchZooming = true, 
		/// };
		/// 
		/// LineSeries series = new LineSeries()
		/// {
		///    ItemsSource = viewModel.Data,
		///    XBindingPath = "XValue",
		///    YBindingPath = "YValue",
		/// };
		/// chart.Series.Add(series);
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public bool EnablePinchZooming
		{
			get { return (bool)GetValue(EnablePinchZoomingProperty); }
			set { SetValue(EnablePinchZoomingProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether the panning is enabled.
		/// </summary>
		/// <value>
		/// It accepts the <c>bool</c> values and the default value is <c>true</c>.
		/// </value>
		/// <example>
		/// # [MainPage.xaml](#tab/tabid-5)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		///
		///     <!--omitted for brevity-->
		///
		///     <chart:SfCartesianChart.ZoomPanBehavior>
		///        <chart:ChartZoomPanBehavior EnablePanning = "True" />
		///     </chart:SfCartesianChart.ZoomPanBehavior>
		///
		///     <chart:LineSeries ItemsSource="{Binding Data}"
		///                       XBindingPath="XValue"
		///                       YBindingPath="YValue"/>
		/// 
		/// </chart:SfCartesianChart>
		///
		/// ]]>
		/// </code>
		/// # [MainPage.xaml.cs](#tab/tabid-6)
		/// <code><![CDATA[
		/// SfCartesianChart chart = new SfCartesianChart();
		/// ViewModel viewModel = new ViewModel();
		/// 
		/// // omitted for brevity
		/// chart.ZoomPanBehavior = new ChartZoomPanBehavior() 
		/// { 
		///    EnablePanning = true,
		/// };
		/// 
		/// LineSeries series = new LineSeries()
		/// {
		///    ItemsSource = viewModel.Data,
		///    XBindingPath = "XValue",
		///    YBindingPath = "YValue",
		/// };
		/// chart.Series.Add(series);
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public bool EnablePanning
		{
			get { return (bool)GetValue(EnablePanningProperty); }
			set { SetValue(EnablePanningProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether zooming is enabled through double tapping.
		/// </summary>
		/// <value>
		/// It accepts the <c>bool</c> values and the default value is <c>true</c>.
		/// </value>
		/// <example>
		/// # [MainPage.xaml](#tab/tabid-7)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		///
		///     <!--omitted for brevity-->
		///
		///     <chart:SfCartesianChart.ZoomPanBehavior>
		///        <chart:ChartZoomPanBehavior EnableDoubleTap = "True" />
		///     </chart:SfCartesianChart.ZoomPanBehavior>
		///
		///     <chart:LineSeries ItemsSource="{Binding Data}"
		///                       XBindingPath="XValue"
		///                       YBindingPath="YValue"/>
		/// 
		/// </chart:SfCartesianChart>
		///
		/// ]]>
		/// </code>
		/// # [MainPage.xaml.cs](#tab/tabid-8)
		/// <code><![CDATA[
		/// SfCartesianChart chart = new SfCartesianChart();
		/// ViewModel viewModel = new ViewModel();
		/// 
		/// // omitted for brevity
		/// chart.ZoomPanBehavior = new ChartZoomPanBehavior() 
		/// { 
		///    EnableDoubleTap = true,
		/// };
		/// 
		/// LineSeries series = new LineSeries()
		/// {
		///    ItemsSource = viewModel.Data,
		///    XBindingPath = "XValue",
		///    YBindingPath = "YValue",
		/// };
		/// chart.Series.Add(series);
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public bool EnableDoubleTap
		{
			get { return (bool)GetValue(EnableDoubleTapProperty); }
			set { SetValue(EnableDoubleTapProperty, value); }
		}

		/// <summary>
		/// Gets or sets the mode for zooming direction.
		/// </summary>
		/// <value>
		/// It accepts the <see cref="Charts.ZoomMode"/> values and the default value is <see cref="ZoomMode.XY"/>.
		/// </value>
		/// <remarks>The zooming can be done both horizontally and vertically.</remarks>
		/// <example>
		/// # [MainPage.xaml](#tab/tabid-9)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		///
		///     <!--omitted for brevity-->
		///
		///     <chart:SfCartesianChart.ZoomPanBehavior>
		///        <chart:ChartZoomPanBehavior ZoomMode="X"/>
		///     </chart:SfCartesianChart.ZoomPanBehavior>
		///
		///     <chart:LineSeries ItemsSource="{Binding Data}"
		///                       XBindingPath="XValue"
		///                       YBindingPath="YValue"/>
		///          
		/// </chart:SfCartesianChart>
		///
		/// ]]>
		/// </code>
		/// # [MainPage.xaml.cs](#tab/tabid-10)
		/// <code><![CDATA[
		/// SfCartesianChart chart = new SfCartesianChart();
		/// ViewModel viewModel = new ViewModel();
		/// 
		/// // omitted for brevity
		/// chart.ZoomPanBehavior = new ChartZoomPanBehavior() 
		/// { 
		///    ZoomMode = ZoomMode.X,
		/// };
		/// 
		/// LineSeries series = new LineSeries()
		/// {
		///    ItemsSource = viewModel.Data,
		///    XBindingPath = "XValue",
		///    YBindingPath = "YValue",
		/// };
		/// chart.Series.Add(series);
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public ZoomMode ZoomMode
		{
			get { return (ZoomMode)GetValue(ZoomModeProperty); }
			set { SetValue(ZoomModeProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether the selection zooming is enabled.
		/// </summary>
		/// <value>
		/// It accept the <c>bool</c> values and the default value is <c>false</c>.
		/// </value>
		/// <remarks>
		/// To show the axis trackball label while <b>Selection Zooming</b>, you must set the <see cref="ChartAxis.ShowTrackballLabel"/> as <c>True</c>.
		/// </remarks>
		/// <example>
		/// # [MainPage.xaml](#tab/tabid-11)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		///
		///     <!--omitted for brevity-->
		///
		///     <chart:SfCartesianChart.ZoomPanBehavior>
		///        <chart:ChartZoomPanBehavior EnableSelectionZooming = "True" />
		///     </chart:SfCartesianChart.ZoomPanBehavior>
		///
		///     <chart:LineSeries ItemsSource="{Binding Data}"
		///                       XBindingPath="XValue"
		///                       YBindingPath="YValue"/>
		/// 
		/// </chart:SfCartesianChart>
		///
		/// ]]>
		/// </code>
		/// # [MainPage.xaml.cs](#tab/tabid-12)
		/// <code><![CDATA[
		/// SfCartesianChart chart = new SfCartesianChart();
		/// ViewModel viewModel = new ViewModel();
		/// 
		/// // omitted for brevity
		/// chart.ZoomPanBehavior = new ChartZoomPanBehavior() 
		/// { 
		///    EnableSelectionZooming = true,
		/// };
		/// 
		/// LineSeries series = new LineSeries()
		/// {
		///    ItemsSource = viewModel.Data,
		///    XBindingPath = "XValue",
		///    YBindingPath = "YValue",
		/// };
		/// chart.Series.Add(series);
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public bool EnableSelectionZooming
		{
			get { return (bool)GetValue(EnableSelectionZoomingProperty); }
			set { SetValue(EnableSelectionZoomingProperty, value); }
		}

		/// <summary>
		/// Gets or sets the stroke color of the selection rectangle.
		/// </summary>
		/// <value>This property takes the <see cref="Brush"/> as its value.</value>
		/// <example>
		/// # [MainPage.xaml](#tab/tabid-13)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		///
		///     <!--omitted for brevity-->
		///
		///     <chart:SfCartesianChart.ZoomPanBehavior>
		///        <chart:ChartZoomPanBehavior EnableSelectionZooming = "True" SelectionRectStroke = "Red" />
		///     </chart:SfCartesianChart.ZoomPanBehavior>
		///
		///     <chart:LineSeries ItemsSource="{Binding Data}"
		///                       XBindingPath="XValue"
		///                       YBindingPath="YValue"/>
		/// 
		/// </chart:SfCartesianChart>
		///
		/// ]]>
		/// </code>
		/// # [MainPage.xaml.cs](#tab/tabid-14)
		/// <code><![CDATA[
		/// SfCartesianChart chart = new SfCartesianChart();
		/// ViewModel viewModel = new ViewModel();
		/// 
		/// // omitted for brevity
		/// chart.ZoomPanBehavior = new ChartZoomPanBehavior() 
		/// { 
		///    EnableSelectionZooming = true,
		///    SelectionRectStroke = Colors.Red,
		/// };
		/// 
		/// LineSeries series = new LineSeries()
		/// {
		///    ItemsSource = viewModel.Data,
		///    XBindingPath = "XValue",
		///    YBindingPath = "YValue",
		/// };
		/// chart.Series.Add(series);
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public Brush SelectionRectStroke
		{
			get { return (Brush)GetValue(SelectionRectStrokeProperty); }
			set { SetValue(SelectionRectStrokeProperty, value); }
		}

		/// <summary>
		/// Gets or sets the stroke width of the selection rectangle.
		/// </summary>
		/// <value>
		/// It accepts <c>double</c> values, and the default value is 1.
		/// </value>
		/// <example>
		/// # [MainPage.xaml](#tab/tabid-15)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		///
		///     <!--omitted for brevity-->
		///
		///     <chart:SfCartesianChart.ZoomPanBehavior>
		///        <chart:ChartZoomPanBehavior EnableSelectionZooming = "True" SelectionRectStrokeWidth = "3" />
		///     </chart:SfCartesianChart.ZoomPanBehavior>
		///
		///     <chart:LineSeries ItemsSource="{Binding Data}"
		///                       XBindingPath="XValue"
		///                       YBindingPath="YValue"/>
		/// 
		/// </chart:SfCartesianChart>
		///
		/// ]]>
		/// </code>
		/// # [MainPage.xaml.cs](#tab/tabid-16)
		/// <code><![CDATA[
		/// SfCartesianChart chart = new SfCartesianChart();
		/// ViewModel viewModel = new ViewModel();
		/// 
		/// // omitted for brevity
		/// chart.ZoomPanBehavior = new ChartZoomPanBehavior() 
		/// { 
		///    EnableSelectionZooming = true,
		///    SelectionRectStrokeWidth = 3,
		/// };
		/// 
		/// LineSeries series = new LineSeries()
		/// {
		///    ItemsSource = viewModel.Data,
		///    XBindingPath = "XValue",
		///    YBindingPath = "YValue",
		/// };
		/// chart.Series.Add(series);
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public double SelectionRectStrokeWidth
		{
			get { return (double)GetValue(SelectionRectStrokeWidthProperty); }
			set { SetValue(SelectionRectStrokeWidthProperty, value); }
		}

		/// <summary>
		/// Gets or sets the fill color of the selection rectangle.
		/// </summary>
		/// <value>This property takes the <see cref="Brush"/> as its value.</value>
		/// <example>
		/// # [MainPage.xaml](#tab/tabid-17)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		///
		///     <!--omitted for brevity-->
		///
		///     <chart:SfCartesianChart.ZoomPanBehavior>
		///        <chart:ChartZoomPanBehavior EnableSelectionZooming = "True" SelectionRectFill = "Red" />
		///     </chart:SfCartesianChart.ZoomPanBehavior>
		///
		///     <chart:LineSeries ItemsSource="{Binding Data}"
		///                       XBindingPath="XValue"
		///                       YBindingPath="YValue"/>
		/// 
		/// </chart:SfCartesianChart>
		///
		/// ]]>
		/// </code>
		/// # [MainPage.xaml.cs](#tab/tabid-18)
		/// <code><![CDATA[
		/// SfCartesianChart chart = new SfCartesianChart();
		/// ViewModel viewModel = new ViewModel();
		/// 
		/// // omitted for brevity
		/// chart.ZoomPanBehavior = new ChartZoomPanBehavior() 
		/// { 
		///    EnableSelectionZooming = true,
		///    SelectionRectFill = Colors.Red,
		/// };
		/// 
		/// LineSeries series = new LineSeries()
		/// {
		///    ItemsSource = viewModel.Data,
		///    XBindingPath = "XValue",
		///    YBindingPath = "YValue",
		/// };
		/// chart.Series.Add(series);
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public Brush SelectionRectFill
		{
			get { return (Brush)GetValue(SelectionRectFillProperty); }
			set { SetValue(SelectionRectFillProperty, value); }
		}

		/// <summary>
		/// Gets or sets the stroke dash array for the selection zooming rectangle.
		/// </summary>
		/// <value>
		/// It accepts <see cref="DoubleCollection"/> values, and the default value is <c>5,5</c>.
		/// </value>
		/// <example>
		/// # [MainPage.xaml](#tab/tabid-19)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		/// 
		///  <chart:SfCartesianChart.Resources>
		///    <DoubleCollection x:Key="dashArray">
		///        <x:Double>5</x:Double>
		///        <x:Double>2</x:Double>
		///    </DoubleCollection>
		///   </chart:SfCartesianChart.Resources>
		///
		///     <!--omitted for brevity-->
		///
		///     <chart:SfCartesianChart.ZoomPanBehavior>
		///        <chart:ChartZoomPanBehavior EnableSelectionZooming = "True" SelectionRectStrokeDashArray = "{StaticResource dashArray}" />
		///     </chart:SfCartesianChart.ZoomPanBehavior>
		///
		///     <chart:LineSeries ItemsSource="{Binding Data}"
		///                       XBindingPath="XValue"
		///                       YBindingPath="YValue"/>
		/// 
		/// </chart:SfCartesianChart>
		///
		/// ]]>
		/// </code>
		/// # [MainPage.xaml.cs](#tab/tabid-20)
		/// <code><![CDATA[
		/// SfCartesianChart chart = new SfCartesianChart();
		/// DoubleCollection doubleCollection = new DoubleCollection();
		/// doubleCollection.Add(5);
		/// doubleCollection.Add(2);
		/// 
		/// ViewModel viewModel = new ViewModel();
		/// 
		/// // omitted for brevity
		/// chart.ZoomPanBehavior = new ChartZoomPanBehavior() 
		/// { 
		///    EnableSelectionZooming = true,
		///    SelectionRectStrokeDashArray = doubleCollection,
		/// };
		/// 
		/// LineSeries series = new LineSeries()
		/// {
		///    ItemsSource = viewModel.Data,
		///    XBindingPath = "XValue",
		///    YBindingPath = "YValue",
		/// };
		/// chart.Series.Add(series);
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public DoubleCollection SelectionRectStrokeDashArray
		{
			get { return (DoubleCollection)GetValue(SelectionRectStrokeDashArrayProperty); }
			set { SetValue(SelectionRectStrokeDashArrayProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether zooming is enabled using the direction of the axis.
		/// </summary>
		/// <value>
		/// It accept the <c>bool</c> values and the default value is <c>false</c>.
		/// </value>
		/// <example>
		/// # [MainPage.xaml](#tab/tabid-21)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		///
		///     <!--omitted for brevity-->
		///
		///     <chart:SfCartesianChart.ZoomPanBehavior>
		///        <chart:ChartZoomPanBehavior EnablePinchZooming = "True" 
		///                                    EnableDirectionalZooming = "True"/>
		///     </chart:SfCartesianChart.ZoomPanBehavior>
		///
		///     <chart:LineSeries ItemsSource="{Binding Data}"
		///                       XBindingPath="XValue"
		///                       YBindingPath="YValue"/>
		/// 
		/// </chart:SfCartesianChart>
		///
		/// ]]>
		/// </code>
		/// # [MainPage.xaml.cs](#tab/tabid-22)
		/// <code><![CDATA[
		/// SfCartesianChart chart = new SfCartesianChart();
		/// ViewModel viewModel = new ViewModel();
		/// 
		/// // omitted for brevity
		/// chart.ZoomPanBehavior = new ChartZoomPanBehavior() 
		/// { 
		///    EnablePinchZooming = true,
		///    EnableDirectionalZooming = true,
		/// };
		/// 
		/// LineSeries series = new LineSeries()
		/// {
		///    ItemsSource = viewModel.Data,
		///    XBindingPath = "XValue",
		///    YBindingPath = "YValue",
		/// };
		/// chart.Series.Add(series);
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public bool EnableDirectionalZooming
		{
			get { return (bool)GetValue(EnableDirectionalZoomingProperty); }
			set { SetValue(EnableDirectionalZoomingProperty, value); }
		}

		/// <summary>
		/// Gets or sets the value that determines the maximum zoom level of the chart. 
		/// </summary>
		/// <value>It accepts <c>double</c> values and its default value is double.NaN.</value>
		/// <example>
		/// # [MainPage.xaml](#tab/tabid-23)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		///
		///     <!--omitted for brevity-->
		///
		///     <chart:SfCartesianChart.ZoomPanBehavior>
		///        <chart:ChartZoomPanBehavior EnablePinchZooming = "True" 
		///                                    MaximumZoomLevel="2"/>
		///     </chart:SfCartesianChart.ZoomPanBehavior>
		///
		///     <chart:LineSeries ItemsSource="{Binding Data}"
		///                       XBindingPath="XValue"
		///                       YBindingPath="YValue"/>
		/// 
		/// </chart:SfCartesianChart>
		///
		/// ]]>
		/// </code>
		/// # [MainPage.xaml.cs](#tab/tabid-24)
		/// <code><![CDATA[
		/// SfCartesianChart chart = new SfCartesianChart();
		/// ViewModel viewModel = new ViewModel();
		/// 
		/// // omitted for brevity
		/// chart.ZoomPanBehavior = new ChartZoomPanBehavior() 
		/// { 
		///    EnablePinchZooming = true,
		///    MaximumZoomLevel=2,
		/// };
		/// 
		/// LineSeries series = new LineSeries()
		/// {
		///    ItemsSource = viewModel.Data,
		///    XBindingPath = "XValue",
		///    YBindingPath = "YValue",
		/// };
		/// chart.Series.Add(series);
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public double MaximumZoomLevel
		{
			get { return (double)GetValue(MaximumZoomLevelProperty); }
			set { SetValue(MaximumZoomLevelProperty, value); }
		}

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="ChartZoomPanBehavior"/> class.
		/// </summary>
		public ChartZoomPanBehavior()
		{
		}

		#endregion

		#region Methods

		#region Public Methods

		/// <summary>
		/// Zooms in on the chart.
		/// </summary>
		/// <remarks>This method increases the zoom level of the chart.</remarks>
		public void ZoomIn()
		{
			if (Chart == null)
			{
				return;
			}

			var area = Chart._chartArea;
			if (area._xAxes.Count > 0 && area._xAxes[0].ZoomPosition < 1)
			{
				_cumulativeZoomLevel += 0.25;
			}

			var origin = 0.5f;

			foreach (var chartAxis in area._xAxes)
			{
				if (CanZoom(chartAxis, double.NaN, area.IsTransposed))
				{
					Zoom(_cumulativeZoomLevel, origin, chartAxis);
				}
			}

			foreach (var chartAxis in area._yAxes)
			{
				if (CanZoom(chartAxis, double.NaN, area.IsTransposed))
				{
					Zoom(_cumulativeZoomLevel, origin, chartAxis);
				}
			}
		}

		/// <summary>
		/// Zooms out from the chart.
		/// </summary>
		/// <remarks>This method decreases the zoom level of the chart.</remarks>
		public void ZoomOut()
		{
			if (Chart == null)
			{
				return;
			}

			var area = Chart._chartArea;

			if (area._xAxes.Count > 0 && area._xAxes[0].ZoomPosition > 0)
			{
				_cumulativeZoomLevel -= 0.25;
			}

			var origin = 0.5f;

			foreach (var chartAxis in area._xAxes)
			{
				if (chartAxis != null && CanZoom(chartAxis, double.NaN, area.IsTransposed))
				{
					Zoom(_cumulativeZoomLevel, origin, chartAxis);
				}
			}

			foreach (var chartAxis in area._yAxes)
			{
				if (chartAxis != null && CanZoom(chartAxis, double.NaN, area.IsTransposed))
				{
					Zoom(_cumulativeZoomLevel, origin, chartAxis);
				}
			}
		}

		/// <summary>
		///  Resets the zoom factor and zoom position for all the axis.
		/// </summary>
		public void Reset()
		{
			if (Chart == null)
			{
				return;
			}

			var area = Chart._chartArea;
			var isTransposed = area.IsTransposed;
			var layout = area._axisLayout;

			Reset(layout.HorizontalAxes, isTransposed);
			Reset(layout.VerticalAxes, isTransposed);
		}

		/// <summary>
		/// Zooms the chart by a specified range.
		/// </summary>
		/// <remarks>This method changes the zoom position and zoom factor of the given chart axis by a specified range.</remarks>
		public void ZoomByRange(ChartAxis chartAxis, double start, double end)
		{
			if (chartAxis != null && chartAxis.CartesianArea != null)
			{
				if (CanZoom(chartAxis, double.NaN, chartAxis.CartesianArea.IsTransposed))
				{
					if (start > end)
					{
						(end, start) = (start, end);
					}

					DoubleRange axisRange = chartAxis.ActualRange;

					if (start >= axisRange.End || end <= axisRange.Start)
					{
						return;
					}

					if (start < axisRange.Start)
					{
						start = axisRange.Start;
					}

					if (end > axisRange.End)
					{
						end = axisRange.End;
					}

					chartAxis.ZoomPosition = (start - axisRange.Start) / axisRange.Delta;
					chartAxis.ZoomFactor = (end - start) / axisRange.Delta;
				}
			}
		}

		/// <summary>
		/// Zooms the chart by a specified range.
		/// </summary>
		/// <remarks>This method changes the zoom position and zoom factor of the given date time axis by a specified range.</remarks>
		public void ZoomByRange(DateTimeAxis dateTimeAxis, DateTime start, DateTime end)
		{
			ZoomByRange(dateTimeAxis, start.ToOADate(), end.ToOADate());
		}

		/// <summary>
		/// Zooms the chart by the specified zoom position and zoom factor.
		/// </summary>
		/// <remarks>This method changes the zoom position and zoom factor of the given chart axis.</remarks>
		public void ZoomToFactor(ChartAxis chartAxis, double zoomPosition, double zoomFactor)
		{
			if (chartAxis.CartesianArea != null &&
				(chartAxis.ZoomFactor != zoomFactor || chartAxis.ZoomPosition != zoomPosition))
			{
				if (CanZoom(chartAxis, double.NaN, chartAxis.CartesianArea.IsTransposed))
				{
					chartAxis.ZoomFactor = zoomFactor;
					chartAxis.ZoomPosition = zoomPosition;
				}
			}
		}

		/// <summary>
		/// Zooms the chart by the specified zoom factor.
		/// </summary>
		/// <remarks>This method changes the zoom factor of the horizontal and vertical axes.</remarks>
		public void ZoomToFactor(double zoomFactor)
		{
			if (Chart == null)
			{
				return;
			}

			var area = Chart._chartArea;
			var isTransposed = area.IsTransposed;
			var layout = area._axisLayout;

			Zoom(layout.HorizontalAxes, zoomFactor, isTransposed);
			Zoom(layout.VerticalAxes, zoomFactor, isTransposed);
		}

		#endregion

		#region Internal Methods

		internal override void SetTouchHandled(IChart chart)
		{
#if MONOANDROID || WINDOWS
			if (chart is SfCartesianChart cartesianChart && EnablePanning)
			{
				cartesianChart.IsHandled = true;
			}
#endif
		}

		internal override void OnDoubleTap(IChart chart, float x, float y)
		{
			base.OnDoubleTap(chart, x, y);

			var clipRect = chart.ActualSeriesClipRect;
			if (clipRect.Contains(x, y) && chart is SfCartesianChart cartesianChart)
			{
				OnDoubleTap(cartesianChart._chartArea, clipRect, x, y);
			}
		}

		internal void OnScrollChanged(IChart chart, Point touchPoint, Point translatePoint)
		{
			var clipRect = chart.ActualSeriesClipRect;
#if MACCATALYST || IOS
			_isScrollChanged = true;
#endif
			if (chart is SfCartesianChart cartesianChart)
			{
				cartesianChart.IsHandled = TouchHandled(cartesianChart, translatePoint);

				if (!IsSelectionZoomingActivated && clipRect.Contains(touchPoint))
				{
					PanTranslate(cartesianChart, clipRect, translatePoint);
				}
			}
		}

		/// <inheritdoc/>
		protected internal override void OnTouchMove(ChartBase chart, float pointX, float pointY)
		{
			if (chart is IChart iChart && chart is SfCartesianChart cartesianChart)
			{
				var clipRect = iChart.ActualSeriesClipRect;

#if !MACCATALYST && !IOS
				if (_isDoubleTapped && IsSelectionZoomingActivated && clipRect.Contains(_startX, _startY))
				{
#else
				if (IsSelectionZoomingActivated && clipRect.Contains(_startX, _startY))
				{
#endif
					_isSelectionActivated = true;
					if (_zoomCancel = OnSelectionZoomDelta(cartesianChart, pointX, pointY))
					{
						Invalidate();
					}
				}
			}
		}

		internal void OnMouseWheelChanged(IChart chart, Point position, double delta)
		{
			var width = (float)chart.ActualSeriesClipRect.Width;
			var height = (float)chart.ActualSeriesClipRect.Height;

			if (EnablePinchZooming && chart.ActualSeriesClipRect.Contains(position) && chart.Area is CartesianChartArea area)
			{
				var direction = delta > 0 ? 1 : -1;

				foreach (var chartAxis in area._xAxes)
				{
					var zoomFactor = chartAxis.ZoomFactor;
					var cumulativeZoomLevel = Math.Max(1 / ChartMath.MinMax(zoomFactor, 0, 1), 1);
					cumulativeZoomLevel = Math.Max(cumulativeZoomLevel + (0.25 * direction), 1);

					if (CanZoom(chartAxis, double.NaN, area.IsTransposed))
					{
						var origin = ChartZoomPanBehavior.GetOrigin((float)position.X, (float)position.Y, width, height, chartAxis);
						Zoom(cumulativeZoomLevel, origin, chartAxis);
					}
				}

				foreach (var chartAxis in area._yAxes)
				{
					var zoomFactor = chartAxis.ZoomFactor;
					var cumulativeZoomLevel = Math.Max(1 / ChartMath.MinMax(zoomFactor, 0, 1), 1);
					cumulativeZoomLevel = Math.Max(cumulativeZoomLevel + (0.25 * direction), 1);

					if (CanZoom(chartAxis, double.NaN, area.IsTransposed))
					{
						var origin = ChartZoomPanBehavior.GetOrigin((float)position.X, (float)position.Y, width, height, chartAxis);
						Zoom(cumulativeZoomLevel, origin, chartAxis);
					}
				}
			}
		}

		internal void OnPinchStateChanged(IChart chart, GestureStatus action, Point location, double angle, float scale)
		{
			var clipRect = chart.ActualSeriesClipRect;
			_pinchStatus = action;

			if ((_pinchStatus != GestureStatus.Completed) && EnablePinchZooming && clipRect.Contains(location) && chart is SfCartesianChart cartesianChart)
			{
				PinchZoom(cartesianChart, clipRect, location, angle, scale);
			}
			else
			{
				_isPinchZoomingActivated = false;
			}

#if WINDOWS
			if (_pinchStatus == GestureStatus.Completed && chart is SfCartesianChart sfCartesianChart)
			{
				sfCartesianChart.IsHandled = false;
			}
#endif
		}

		/// <inheritdoc/>
		protected internal override void OnTouchUp(ChartBase chart, float pointX, float pointY)
		{
#if WINDOWS || ANDROID
			_isDoubleTapped = false;
#endif

			if (chart is IChart iChart && chart is SfCartesianChart cartesianChart)
			{
				if (EnableSelectionZooming && IsSelectionZoomingActivated && _zoomCancel)
				{
					if (_isSelectionActivated)
					{
						if (Math.Abs(SelectionRect.Width) > 20 || Math.Abs(SelectionRect.Height) > 20)
						{
							Zoom(SelectionRect, cartesianChart._chartArea);
							cartesianChart.RaiseSelectionZoomEndEvent(SelectionRect);
						}

						SelectionRect = new Rect(0, 0, 0, 0);
						Invalidate();
						_isSelectionActivated = false;
						IsSelectionZoomingActivated = false;
#if MACCATALYST || IOS
						_isSelected = false;
						_isDoubleTapped = false;
#endif
					}
#if WINDOWS || ANDROID
					else if (EnableDoubleTap)
					{
						_isSelectionZoomingEnded = true;
						SelectionRect = new Rect(0, 0, 0, 0);
						OnDoubleTap(cartesianChart._chartArea, iChart.ActualSeriesClipRect, pointX, pointY);
					}
#endif
				}
			}
		}

		internal override void OnTouchDown(float pointX, float pointY)
		{
			base.OnTouchDown(pointX, pointY);
#if MACCATALYST || IOS
			_downEntered++;

			if (_isPinchZoomed || _isScrollChanged)
			{
				_downEntered = 1;
			}

			if (_downEntered == 1)
			{
				_selectedRegionRect = new Rect(pointX - 10, pointY - 10, 20, 20);
				_startTime = DateTime.Now;
				_isScrollChanged = false;
				_isPinchZoomed = false;
			}
			else if (_downEntered == 2)
			{
				_endTime = DateTime.Now;
				var ellapsedTime = _startTime - _endTime;

				_isDoubleTapped = ellapsedTime < TimeSpan.FromMilliseconds(150) && _selectedRegionRect.Contains(pointX, pointY);

				if (_isDoubleTapped && !_isSelected)
				{
					if (EnableSelectionZooming && Chart != null)
					{
						IsSelectionZoomingActivated = true;
						SelectionRect = new Rect(pointX, pointY, pointX, pointY);
						_startX = pointX;
						_startY = pointY;
						_isSelected = true;
						Chart.RaiseSelectionZoomStartEvent(SelectionRect);
					}
				}

				_downEntered = 0;
			}
#endif
		}

		#endregion

		#region Private Methods

		bool TouchHandled(SfCartesianChart cartesian, Point velocity)
		{
			var area = cartesian._chartArea;
			bool isPanEnd = true;

			if (!EnablePanning || area == null)
			{
				return false;
			}

			if (Math.Abs(velocity.Y) < Math.Abs(velocity.X))
			{
				foreach (ChartAxis axis in area._xAxes)
				{
					double position = axis.ZoomPosition;
					double factor = 1.0f - axis.ZoomFactor;
					bool velocityIsCrossed = axis.IsInversed ? velocity.X > 0 : velocity.X < 0;

					if ((position == factor && velocityIsCrossed) || (position == 0 && !velocityIsCrossed))
					{
						isPanEnd = false;
					}
					else
					{
						isPanEnd = true;
						break;
					}
				}

				return isPanEnd;
			}
			else
			{
				foreach (ChartAxis axis in area._yAxes)
				{
					double position = axis.ZoomPosition;
					double factor = 1.0f - axis.ZoomFactor;
#if MONOANDROID || WINDOWS
					bool velocityIsCrossed = axis.IsInversed ? velocity.Y < 0 : velocity.Y > 0;

					if ((position == factor && velocity.Y < 0) || (position == 0 && velocityIsCrossed))
#else
					bool velocityIsCrossed = axis.IsInversed ? velocity.Y > 0 : velocity.Y <= 0;

					if ((position == factor && velocity.Y > 0) || (position == 0 && velocityIsCrossed))
#endif
					{
						isPanEnd = false;
					}
					else
					{
						isPanEnd = true;
						break;
					}
				}

				return isPanEnd;
			}
		}

		void Zoom(ObservableCollection<ChartAxis> axes, double zoomFactor, bool transposed)
		{
			foreach (var chartAxis in axes)
			{
				if (CanZoom(chartAxis, double.NaN, transposed))
				{
					if (chartAxis.ZoomFactor <= 1 && chartAxis.ZoomFactor >= 0.1)
					{
						chartAxis.ZoomFactor = zoomFactor;
						chartAxis.ZoomPosition = 0.5f;
					}
				}
			}
		}

		static bool CanReset(ObservableCollection<ChartAxis> axes)
		{
			if (axes != null)
			{
				return axes.Any(axis => axis.ZoomFactor < 1);
			}

			return false;
		}

		void Reset(ObservableCollection<ChartAxis> axes, bool transpose)
		{
			foreach (var chartAxis in axes)
			{
				if (Chart != null && CanZoom(chartAxis, double.NaN, transpose))
				{
					var previousZoomFactor = chartAxis.ZoomFactor;
					var previousZoomPosition = chartAxis.ZoomPosition;
					chartAxis.ZoomPosition = 0;
					chartAxis.ZoomFactor = 1;
					_cumulativeZoomLevel = 1;
					Chart.RaiseResetZoomEvent(chartAxis, previousZoomFactor, previousZoomPosition);
				}
			}
		}

		void OnDoubleTap(CartesianChartArea area, Rect clipRect, float pointX, float pointY)
		{
#if !MACCATALYST && !IOS
			if (EnableSelectionZooming && !_isSelectionZoomingEnded && Chart != null)
			{
				IsSelectionZoomingActivated = true;
				SelectionRect = new Rect(pointX, pointY, pointX, pointY);
				_startX = pointX;
				_startY = pointY;
				_isDoubleTapped = true;
				Chart.RaiseSelectionZoomStartEvent(SelectionRect);
			}

			_isSelectionZoomingEnded = !EnableSelectionZooming || _isSelectionZoomingEnded;

			if (EnableDoubleTap && _isSelectionZoomingEnded)
#else
			if (EnableDoubleTap)
#endif
			{
				var clip = clipRect;
				var axisLayout = area._axisLayout;
				var xAxes = axisLayout.HorizontalAxes;
				var yAxes = axisLayout.VerticalAxes;

				float manipulationX = (float)(pointX - clip.Left);
				float manipulationY = (float)(pointY - clip.Top);

				float width = (float)clip.Width;
				float height = (float)clip.Height;

				if (ChartZoomPanBehavior.CanReset(xAxes) || ChartZoomPanBehavior.CanReset(yAxes))
				{
					Reset(xAxes, area.IsTransposed);
					Reset(yAxes, area.IsTransposed);
				}
				else
				{
					foreach (var chartAxis in xAxes)
					{
						if (CanZoom(chartAxis, double.NaN, area.IsTransposed))
						{
							var origin = ChartZoomPanBehavior.GetOrigin(manipulationX, manipulationY, width, height, chartAxis);
							Zoom(2.5, origin, chartAxis);
						}
					}

					foreach (var chartAxis in yAxes)
					{
						if (CanZoom(chartAxis, double.NaN, area.IsTransposed))
						{
							var origin = ChartZoomPanBehavior.GetOrigin(manipulationX, manipulationY, width, height, chartAxis);

							Zoom(2.5, origin, chartAxis);
						}
					}
					_cumulativeZoomLevel = 2.5;
				}

				IsSelectionZoomingActivated = false;

#if MACCATALYST || IOS
				_isDoubleTapped = false;
				_isSelected = false;
#else
				_isSelectionZoomingEnded = false;
#endif
			}
		}

		bool CanZoom(ChartAxis chartAxis, double angle, bool transposed)
		{
			if (Chart == null)
			{
				return false;
			}

			bool canDirectionalZoom = ZoomMode == ZoomMode.XY;

			if (_isPinchZoomingActivated && !double.IsNaN(angle) && EnableDirectionalZooming && canDirectionalZoom)
			{
				bool isXDirection = (angle >= 340 && angle <= 360) || (angle >= 0 && angle <= 20) || (angle >= 160 && angle <= 200);
				bool isYDirection = (angle >= 70 && angle <= 110) || (angle >= 250 && angle <= 290);
				bool isBothDirection = (angle > 20 && angle < 70) || (angle > 110 && angle < 160) || (angle > 200 && angle < 250) || (angle > 290 && angle < 340);

				canDirectionalZoom = (!chartAxis.IsVertical && isXDirection) || (chartAxis.IsVertical && isYDirection) || isBothDirection;
			}

			if (chartAxis.RegisteredSeries.Count > 0 && chartAxis.RegisteredSeries[0] != null && transposed)
			{
				if ((!chartAxis.IsVertical && ZoomMode == ZoomMode.Y) || (chartAxis.IsVertical && ZoomMode == ZoomMode.X) || canDirectionalZoom)
				{
					Chart.IsRequiredDataLabelsMeasure = false;
					return true;
				}
			}
			else
			{
				if ((chartAxis.IsVertical && ZoomMode == ZoomMode.Y) || (!chartAxis.IsVertical && ZoomMode == ZoomMode.X) || canDirectionalZoom)
				{
					Chart.IsRequiredDataLabelsMeasure = false;
					return true;
				}
			}

			return false;
		}

		static float GetOrigin(float manipulationX, float manipulationY, float width, float height, ChartAxis chartAxis)
		{
			float origin;
			double plotOffsetStart = chartAxis.ActualPlotOffsetStart;
			double plotOffsetEnd = chartAxis.ActualPlotOffsetEnd;

			if (chartAxis.IsVertical)
			{
				origin = (float)(chartAxis.IsInversed
					? ((manipulationY - plotOffsetEnd) / height)
					: 1 - ((manipulationY - plotOffsetStart) / height));
			}
			else
			{
				origin = (float)(chartAxis.IsInversed
					? 1.0 - ((manipulationX - plotOffsetEnd) / width)
					: (manipulationX - plotOffsetStart) / width);
			}

			return origin;
		}

		void PinchZoom(SfCartesianChart cartesianChart, Rect clipRect, Point scaleOrigin, double angle, double scale)
		{
			var clip = clipRect;
			var chartArea = cartesianChart._chartArea;

			foreach (var chartAxis in chartArea._xAxes)
			{
				PinchZoom(chartAxis, scaleOrigin, angle, clip.Size, scale, chartArea.IsTransposed);
			}

			foreach (var chartAxis in chartArea._yAxes)
			{
				PinchZoom(chartAxis, scaleOrigin, angle, clip.Size, scale, chartArea.IsTransposed);
			}

			_isPinchZoomingActivated = true;
		}

		void PinchZoom(ChartAxis chartAxis, Point scaleOrigin, double angle, SizeF size, double scale, bool transpose)
		{
			if (CanZoom(chartAxis, angle, transpose))
			{
#if MACCATALYST || IOS
				_isPinchZoomed = true;
#endif
				var zoomFactor = chartAxis.ZoomFactor;
				var currentScale = Math.Max(1 / ChartMath.MinMax(zoomFactor, 0, 1), 1);
				currentScale *= scale;
				var origin = ChartZoomPanBehavior.GetOrigin((float)scaleOrigin.X, (float)scaleOrigin.Y, size.Width, size.Height, chartAxis);

				Zoom(currentScale, origin, chartAxis);
			}
		}

		void PanTranslate(SfCartesianChart cartesianChart, Rect clipRect, Point translatePoint)
		{
			var area = cartesianChart._chartArea;

			foreach (var axis in area._xAxes)
			{
				if (EnablePanning && !_isPinchZoomingActivated && CanZoom(axis, double.NaN, area.IsTransposed))
				{
					axis.IsScrolling = true;
					Translate(axis, clipRect, translatePoint, Math.Max(1 / ChartMath.MinMax(axis.ZoomFactor, 0, 1), 1));
				}
			}

			foreach (var axis in area._yAxes)
			{
				if (EnablePanning && !_isPinchZoomingActivated && CanZoom(axis, double.NaN, area.IsTransposed))
				{
					axis.IsScrolling = true;
					Translate(axis, clipRect, translatePoint, Math.Max(1 / ChartMath.MinMax(axis.ZoomFactor, 0, 1), 1));
				}
			}
		}

		void Translate(ChartAxis axis, RectF clip, Point translatePoint, double currentScale)
		{
			double previousZoomPosition = axis.ZoomPosition;
			double currentZoomPosition;

			//Todo : Need to check the translate value with android and iOS.
			if (axis.IsVertical)
			{
				double offset = translatePoint.Y / clip.Height / currentScale;
#if ANDROID
				offset = axis.IsInversed ? previousZoomPosition + offset : previousZoomPosition - offset;
#else
				offset = axis.IsInversed ? previousZoomPosition - offset : previousZoomPosition + offset;
#endif
				currentZoomPosition = ChartMath.MinMax(offset, 0, 1 - axis.ZoomFactor);
			}
			else
			{
				double offset = translatePoint.X / clip.Width / currentScale;
#if ANDROID
				offset = axis.IsInversed ? previousZoomPosition - offset : previousZoomPosition + offset;
#else
				offset = axis.IsInversed ? previousZoomPosition + offset : previousZoomPosition - offset;
#endif
				currentZoomPosition = ChartMath.MinMax(offset, 0, 1 - axis.ZoomFactor);
			}

			if ((_pinchStatus == GestureStatus.Completed || _pinchStatus == GestureStatus.Canceled) && previousZoomPosition != currentZoomPosition)
			{
				if (Chart != null && Chart.RaiseScrollEvent(axis, currentZoomPosition))
				{
#if ANDROID || IOS
					Chart.HideTooltipView();
#endif
					axis.ZoomPosition = currentZoomPosition;
				}
			}
		}

		/// <summary>
		/// Zoom at cumulative level to the corresponding origin.
		/// </summary>
		bool Zoom(double cumulativeLevel, float origin, ChartAxis axis)
		{
			if (Chart != null && axis != null)
			{
				double calcZoomPos;
				double calcZoomFactor;
				_zoomCancel = Chart.RaiseZoomStartEvent(axis);
				if (!double.IsNaN(MaximumZoomLevel))
				{
					cumulativeLevel = cumulativeLevel <= MaximumZoomLevel ? cumulativeLevel : MaximumZoomLevel;
				}

				if (cumulativeLevel == 1)
				{
					calcZoomFactor = 1;
					calcZoomPos = 0;
				}
				else
				{
					calcZoomFactor = ChartMath.MinMax(1 / cumulativeLevel, 0, 1);
					calcZoomPos = axis.ZoomPosition + ((axis.ZoomFactor - calcZoomFactor) * origin);
				}

				var newZoomFactor = calcZoomPos + calcZoomFactor > 1 ? 1 - calcZoomPos : calcZoomFactor;
				if (_zoomCancel)
				{
					_zoomCancel = Chart.RaiseZoomDeltaEvent(axis, newZoomFactor, calcZoomPos);
				}

				if (_zoomCancel && (axis.ZoomPosition != calcZoomPos || axis.ZoomFactor != calcZoomFactor))
				{
					axis.ZoomPosition = calcZoomPos;
					axis.ZoomFactor = newZoomFactor;
					Chart.RaiseZoomEndEvent(axis);
					return true;
				}
			}

			return false;
		}

		bool OnSelectionZoomDelta(SfCartesianChart chart, float pointX, float pointY)
		{
			if (EnableSelectionZooming)
			{
				var iChart = chart as IChart;
				var chartBounds = iChart.ActualSeriesClipRect;
				var plotBounds = chart._chartArea.PlotArea.PlotAreaBounds;
				var actualClipRect = chart._chartArea.ActualSeriesClipRect;
				_endX = pointX;
				_endY = pointY;
				var left = _startX;
				var top = _startY;

				if (chartBounds.Left > _endX)
				{
					_endX = (float)chartBounds.Left;
				}

				if (chartBounds.Right < _endX)
				{
					_endX = (float)chartBounds.Right;
				}

				if (chartBounds.Top > _endY)
				{
					_endY = (float)chartBounds.Top;
				}

				if (actualClipRect.Bottom + chartBounds.Top - actualClipRect.Top < _endY)
				{
					_endY = actualClipRect.Bottom + (float)chartBounds.Top - actualClipRect.Top;
				}

				_currentSelectionRect = new Rect(left - plotBounds.Left, top - chartBounds.Top + actualClipRect.Top, _endX - left, _endY - top);
				SelectionRect = _currentSelectionRect;
				return chart.RaiseSelectionZoomDeltaEvent(SelectionRect);
			}

			return false;
		}

		void Zoom(Rect selectionRect, CartesianChartArea area)
		{
			var clipRect = area.ActualSeriesClipRect;
			var axisLayout = area._axisLayout;
			var xAxes = axisLayout.HorizontalAxes;
			var yAxes = axisLayout.VerticalAxes;

			foreach (var chartAxis in xAxes)
			{
				var previousZoomFactor = chartAxis.ZoomFactor;
				var previousZoomPosition = chartAxis.ZoomPosition;
				double currentZoomFactor;
				currentZoomFactor = previousZoomFactor * (selectionRect.Width / clipRect.Width);
				if (!double.IsNaN(MaximumZoomLevel))
				{
					currentZoomFactor = 1 / currentZoomFactor <= MaximumZoomLevel
						? currentZoomFactor
						: 1 / MaximumZoomLevel;
				}

				chartAxis.ZoomFactor = ZoomMode != ZoomMode.Y ? currentZoomFactor : 1f;
				if (currentZoomFactor != previousZoomFactor)
				{
					chartAxis.ZoomPosition = ZoomMode != ZoomMode.Y
						? previousZoomPosition +
						  (Math.Abs((float)(chartAxis.IsInversed ? clipRect.Right - selectionRect.Right : selectionRect.Left - clipRect.Left) / clipRect.Width) * previousZoomFactor) : 0;
				}
			}

			foreach (var chartAxis in yAxes)
			{
				var previousZoomFactor = chartAxis.ZoomFactor;
				var previousZoomPosition = chartAxis.ZoomPosition;
				double currentZoomFactor;

				currentZoomFactor = previousZoomFactor * selectionRect.Height / clipRect.Height;
				if (!double.IsNaN(MaximumZoomLevel))
				{
					currentZoomFactor = 1 / currentZoomFactor <= MaximumZoomLevel
						? currentZoomFactor
						: 1 / MaximumZoomLevel;
				}

				chartAxis.ZoomFactor = ZoomMode != ZoomMode.X ? currentZoomFactor : 1;
				if (currentZoomFactor != previousZoomFactor)
				{
					chartAxis.ZoomPosition = ZoomMode != ZoomMode.X
						? previousZoomPosition +
							((1 - Math.Abs(((chartAxis.IsInversed ? clipRect.Bottom - selectionRect.Bottom : selectionRect.Top - clipRect.Top) + selectionRect.Height) / clipRect.Height)) * previousZoomFactor) : 0;
				}
			}
		}

		void Invalidate()
		{
			Chart?._zoomPanView.InvalidateDrawable();
		}

		static void OnEnableDoubleTapPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
		}

		static void OnEnableZoomingPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
		}

		static void OnEnablePanningPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
		}

		static void OnZoomModePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
		}

		static void OnMaximumZoomLevelPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
		}

		static object SelectionRectStrokeDefaultValueCreator(BindableObject bindable)
		{
			return new SolidColorBrush(Color.FromArgb("#6200EE"));
		}

		static object SelectionRectFillDefaultValueCreator(BindableObject bindable)
		{
			return new SolidColorBrush(Color.FromArgb("#146200EE"));
		}

		#endregion

		#endregion
	}
}