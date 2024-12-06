using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Toolkit.Internals;
using Syncfusion.Maui.Toolkit.Graphics.Internals;
using Syncfusion.Maui.Toolkit.Themes;

namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// Enables the trackball functionality in the <see cref="SfCartesianChart"/> to provide interactive data point tracking.
	/// </summary>
	/// <remarks>
	/// <para>The <see cref="ChartTrackballBehavior"/> class allows users to track data points as they move the pointer over the chart. The trackball provides visual feedback by displaying information about the data points at the nearest location to the pointer.</para>
	/// <para>It can be customized to display the exact values of one or more series at a specific point, which is especially useful for comparing data across multiple series or analyzing trends in the chart.</para>
	/// <para>To enable and customize the trackball behavior, add an instance of <see cref="ChartTrackballBehavior"/> to the <see cref="SfCartesianChart.TrackballBehavior"/></para>
	/// </remarks>
	/// <example>
	/// # [Xaml](#tab/tabid-1)
	/// <code><![CDATA[
	///  <chart:SfCartesianChart>
	/// 
	///   <chart:SfCartesianChart.TrackballBehavior>
	///     <chart:ChartTrackballBehavior/>
	///   </chart:SfCartesianChart.TrackballBehavior>
	/// 
	/// </chart:SfCartesianChart>
	/// ]]></code>
	/// # [C#](#tab/tabid-2)
	/// <code><![CDATA[
	/// SfCartesianChart chart = new SfCartesianChart();
	/// ChartTrackballBehavior trackball = new ChartTrackballBehavior();
	/// chart.TrackballBehavior= trackball;
	///     
	/// ]]></code>
	/// ***
	/// </example>
	public partial class ChartTrackballBehavior : ChartBehavior, IMarkerDependent, IThemeElement
	{
		#region Fields

		bool _isAnySideBySideSeries;
		bool _isAnyContinuesSeries;
		float _currX;
		Point _linePoint1;
		Point _linePoint2;
		bool _animateMarker;
		bool _isScheduled;
		readonly float _trackLabelSpacing = 4f;
		readonly List<TrackballAxisInfo> _axisPointInfos;
		List<TrackballAxisInfo> _previousAxisPointInfos;
		ChartLabelStyle _actualLabelStyle;
		List<List<TrackballPointInfo>>? _intersectedGroups;
		bool _canAutoHideOnExit;

		#endregion

		#region Internal Properties

		internal bool IsPressed { get; set; }
		internal bool LongPressActive { get; set; }
		internal SfCartesianChart? CartesianChart { get; set; }
		internal RectF SeriesBounds { get; set; }
		internal List<TrackballPointInfo> PointInfos { get; set; }
		internal List<TrackballPointInfo> PreviousPointInfos { get; set; }
		internal List<SfTooltip> ContentList { get; set; }
		internal SfTooltip? GroupedLabelView { get; set; }

		/// <summary>
		/// Identifies the <see cref="TrackballBackground"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="TrackballBackground"/> bindable property determines the background
		/// color of the trackball.
		/// </remarks>
		internal static readonly BindableProperty TrackballBackgroundProperty = BindableProperty.Create(
			nameof(TrackballBackground),
			typeof(Brush),
			typeof(ChartTrackballBehavior),
			SolidColorBrush.Black,
			BindingMode.Default,
			null,
			null);

		/// <summary>
		/// Identifies the <see cref="TrackballFontSize"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="TrackballFontSize"/> bindable property determines the font size
		/// of the trackball labels.
		/// </remarks>
		internal static readonly BindableProperty TrackballFontSizeProperty = BindableProperty.Create(
			nameof(TrackballFontSize),
			typeof(double),
			typeof(ChartTrackballBehavior),
			14.0,
			BindingMode.Default);

		/// <summary>
		/// Identifies the <see cref="LineStroke"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="LineStroke"/> bindable property determines the stroke color
		/// of the trackball line.
		/// </remarks>
		internal static readonly BindableProperty LineStrokeProperty = BindableProperty.Create(
			nameof(LineStroke),
			typeof(Brush),
			typeof(ChartTrackballBehavior),
			SolidColorBrush.Black,
			BindingMode.Default,
			null,
			null);

		/// <summary>
		/// Identifies the <see cref="LineStrokeWidth"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="LineStrokeWidth"/> bindable property determines the stroke width
		/// of the trackball line.
		/// </remarks>
		internal static readonly BindableProperty LineStrokeWidthProperty = BindableProperty.Create(
			nameof(LineStrokeWidth),
			typeof(double),
			typeof(ChartTrackballBehavior),
			1d,
			BindingMode.Default,
			null,
			null);

		internal Brush TrackballBackground
		{
			get { return (Brush)GetValue(TrackballBackgroundProperty); }
			set { SetValue(TrackballBackgroundProperty, value); }
		}

		internal double TrackballFontSize
		{
			get { return (double)GetValue(TrackballFontSizeProperty); }
			set { SetValue(TrackballFontSizeProperty, value); }
		}

		internal Brush LineStroke
		{
			get { return (Brush)GetValue(LineStrokeProperty); }
			set { SetValue(LineStrokeProperty, value); }
		}

		internal double LineStrokeWidth
		{
			get { return (double)GetValue(LineStrokeWidthProperty); }
			set { SetValue(LineStrokeWidthProperty, value); }
		}

		#endregion

		#region Bindable Properties

		/// <summary>
		/// Identifies the <see cref="DisplayMode"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="DisplayMode"/> bindable property determines the display mode
		/// of the trackball labels.
		/// </remarks>
		public static readonly BindableProperty DisplayModeProperty = BindableProperty.Create(
			nameof(DisplayMode),
			typeof(LabelDisplayMode),
			typeof(ChartTrackballBehavior),
			LabelDisplayMode.FloatAllPoints);

		/// <summary>
		/// Identifies the <see cref="LineStyle"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="LineStyle"/> bindable property determines the style
		/// of the trackball line.
		/// </remarks>
		public static readonly BindableProperty LineStyleProperty = BindableProperty.Create(
			nameof(LineStyle),
			typeof(ChartLineStyle),
			typeof(ChartTrackballBehavior),
			propertyChanged: OnLineStyleChanged);

		/// <summary>
		/// Identifies the <see cref="LabelStyle"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="LabelStyle"/> bindable property determines the style
		/// of the trackball labels.
		/// </remarks>
		public static readonly BindableProperty LabelStyleProperty = BindableProperty.Create(
			nameof(LabelStyle),
			typeof(ChartLabelStyle),
			typeof(ChartTrackballBehavior),
			null,
			propertyChanged: OnLabelStyleChanged);

		/// <summary>
		/// Identifies the <see cref="ShowLine"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="ShowLine"/> bindable property determines whether the trackball line
		/// is visible.
		/// </remarks>
		public static readonly BindableProperty ShowLineProperty = BindableProperty.Create(
			nameof(ShowLine),
			typeof(bool),
			typeof(ChartTrackballBehavior),
			true);

		/// <summary>
		/// Identifies the <see cref="ShowLabel"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="ShowLabel"/> bindable property determines whether the trackball labels
		/// are visible.
		/// </remarks>
		public static readonly BindableProperty ShowLabelProperty = BindableProperty.Create(
			nameof(ShowLabel),
			typeof(bool),
			typeof(ChartTrackballBehavior),
			true);

		/// <summary>
		/// Identifies the <see cref="MarkerSettings"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="MarkerSettings"/> bindable property determines the customization options
		/// for the trackball markers.
		/// </remarks>
		public static readonly BindableProperty MarkerSettingsProperty = ChartMarker.MarkerSettingsProperty;

		/// <summary>
		/// Identifies the <see cref="ShowMarkers"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="ShowMarkers"/> bindable property determines whether the trackball markers
		/// are visible.
		/// </remarks>
		public static readonly BindableProperty ShowMarkersProperty = ChartMarker.ShowMarkersProperty;

		/// <summary>
		/// Identifies the <see cref="ActivationMode"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="ActivationMode"/> bindable property determines the activation mode
		/// of the trackball.
		/// </remarks>
		public static readonly BindableProperty ActivationModeProperty = BindableProperty.Create(
			nameof(ActivationMode),
			typeof(ChartTrackballActivationMode),
			typeof(ChartTrackballBehavior),
			null,
			defaultValueCreator: DefaultActivationMode);

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets display mode of trackball labels. By default, labels for all the series under the current point index value will be shown.
		/// </summary>
		/// <value>It accepts <see cref="LabelDisplayMode"/> values and its defaults value is <see cref="LabelDisplayMode.FloatAllPoints"/></value>
		/// <example>
		/// # [Xaml](#tab/tabid-3)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///           <!--omitted for brevity-->
		///
		///           <chart:SfCartesianChart.TrackballBehavior>
		///               <chart:ChartTrackballBehavior DisplayMode="NearestPoint"/>
		///           </chart:SfCartesianChart.TrackballBehavior>
		///
		///           <chart:LineSeries ItemsSource="{Binding Data}"
		///                             XBindingPath="XValue"
		///                             YBindingPath="YValue"/>
		///
		///     </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [C#](#tab/tabid-4)
		/// <code><![CDATA[
		///     SfCartesianChart chart = new SfCartesianChart();
		///     ViewModel viewModel = new ViewModel();
		///
		///    // omitted for brevity
		///    chart.TrackballBehavior = new ChartTrackballBehavior()
		///    {
		///        DisplayMode = LabelDisplayMode.NearestPoint,
		///    };
		///
		///     LineSeries series = new LineSeries();
		///     series.ItemsSource = viewModel.Data;
		///     series.XBindingPath = "XValue";
		///     series.YBindingPath = "YValue";
		///     chart.Series.Add(series);
		///
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public LabelDisplayMode DisplayMode
		{
			get { return (LabelDisplayMode)GetValue(DisplayModeProperty); }
			set { SetValue(DisplayModeProperty, value); }
		}

		/// <summary>
		/// Gets or sets the value to customize the appearance of the trackball line.
		/// </summary>
		/// <remarks>
		/// To customize the trackball line appearance, you need to create an instance of the <see cref="ChartLineStyle"/> class and set to the <see cref="LineStyle"/> property.
		/// <para>Null values are invalid.</para>
		/// </remarks>
		/// <value>This property takes the <see cref="ChartLineStyle"/> as its value.</value>
		/// <example>
		/// # [Xaml](#tab/tabid-5)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///           <!--omitted for brevity-->
		///
		///           <chart:SfCartesianChart.TrackballBehavior>
		///               <chart:ChartTrackballBehavior>
		///                   <chart:ChartTrackballBehavior.LineStyle>
		///                       <chart:ChartLineStyle Stroke = "Red" StrokeWidth="2"/>
		///                   </chart:ChartTrackballBehavior.LineStyle>
		///        </chart:ChartTrackballBehavior>
		///           </chart:SfCartesianChart.TrackballBehavior>
		///
		///           <chart:LineSeries ItemsSource="{Binding Data}"
		///                             XBindingPath="XValue"
		///                             YBindingPath="YValue"/>
		///
		///     </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [C#](#tab/tabid-6)
		/// <code><![CDATA[
		///     SfCartesianChart chart = new SfCartesianChart();
		///     ViewModel viewModel = new ViewModel();
		///
		///    // omitted for brevity 
		///    var lineStyle = new ChartLineStyle()
		///    {
		///        Stroke = new SolidColorBrush(Colors.Red), StrokeWidth = 2,
		///    };
		///    chart.TrackballBehavior = new ChartTrackballBehavior()
		///    {
		///        LineStyle = lineStyle,
		///    };
		///
		///     LineSeries series = new LineSeries();
		///     series.ItemsSource = viewModel.Data;
		///     series.XBindingPath = "XValue";
		///     series.YBindingPath = "YValue";
		///     chart.Series.Add(series);
		///
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public ChartLineStyle LineStyle
		{
			get { return (ChartLineStyle)GetValue(LineStyleProperty); }
			set { SetValue(LineStyleProperty, value); }
		}

		/// <summary>
		/// Gets or sets the value to customize the appearance of trackball labels. 
		/// </summary>
		/// <remarks>
		/// To customize the trackball label appearance, you need to create an instance of the <see cref="ChartLabelStyle"/> class and set to the <see cref="LabelStyle"/> property.
		/// <para>Null values are invalid.</para>
		/// </remarks>
		/// <value>This property takes the <see cref="ChartLabelStyle"/> as its value.</value>
		/// <example>
		/// # [Xaml](#tab/tabid-7)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///           <!--omitted for brevity-->
		///
		///           <chart:SfCartesianChart.TrackballBehavior>
		///               <chart:ChartTrackballBehavior>
		///                   <chart:ChartTrackballBehavior.LabelStyle>
		///                       <chart:ChartLabelStyle TextColor="Red" FontSize="14"/>
		///                   </chart:ChartTrackballBehavior.LabelStyle>
		///        </chart:ChartTrackballBehavior>
		///           </chart:SfCartesianChart.TrackballBehavior>
		///
		///           <chart:LineSeries ItemsSource="{Binding Data}"
		///                             XBindingPath="XValue"
		///                             YBindingPath="YValue"/>
		///
		///     </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [C#](#tab/tabid-8)
		/// <code><![CDATA[
		///     SfCartesianChart chart = new SfCartesianChart();
		///     ViewModel viewModel = new ViewModel();
		///
		///    // omitted for brevity 
		///    var labelStyle = new ChartLabelStyle()
		///    {
		///        TextColor = Colors.Red, FontSize = 12,
		///    };
		///    chart.TrackballBehavior = new ChartTrackballBehavior()
		///    {
		///        LabelStyle = labelStyle,
		///    };
		///
		///     LineSeries series = new LineSeries();
		///     series.ItemsSource = viewModel.Data;
		///     series.XBindingPath = "XValue";
		///     series.YBindingPath = "YValue";
		///     chart.Series.Add(series);
		///
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public ChartLabelStyle LabelStyle
		{
			get { return (ChartLabelStyle)GetValue(LabelStyleProperty); }
			set { SetValue(LabelStyleProperty, value); }
		}

		/// <summary>
		/// Gets or sets the option for customize the trackball markers.
		/// </summary>
		/// <value>This property takes the <see cref="ChartMarkerSettings"/> as its value.</value>
		/// <example>
		/// # [Xaml](#tab/tabid-9)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///           <!--omitted for brevity-->
		///
		///           <chart:SfCartesianChart.TrackballBehavior>
		///               <chart:ChartTrackballBehavior>
		///                   <chart:ChartTrackballBehavior.MarkerSettings>
		///                       <chart:ChartMarkerSettings Height = "10" Width="10" Fill="Red"/>
		///                   </chart:ChartTrackballBehavior.MarkerSettings>
		///        </chart:ChartTrackballBehavior>
		///           </chart:SfCartesianChart.TrackballBehavior>
		///
		///           <chart:LineSeries ItemsSource="{Binding Data}"
		///                             XBindingPath="XValue"
		///                             YBindingPath="YValue"/>
		///
		///     </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [C#](#tab/tabid-10)
		/// <code><![CDATA[
		///     SfCartesianChart chart = new SfCartesianChart();
		///     ViewModel viewModel = new ViewModel();
		///
		///    // omitted for brevity 
		///    var markerSettings = new ChartMarkerSettings()
		///    {
		///        Height = 10, Width = 10,
		///        Fill = new SolidColorBrush(Colors.Red),
		///    };
		///    chart.TrackballBehavior = new ChartTrackballBehavior()
		///    {
		///        MarkerSettings = markerSettings,
		///    };
		///
		///     LineSeries series = new LineSeries();
		///     series.ItemsSource = viewModel.Data;
		///     series.XBindingPath = "XValue";
		///     series.YBindingPath = "YValue";
		///     chart.Series.Add(series);
		///
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public ChartMarkerSettings MarkerSettings
		{
			get { return (ChartMarkerSettings)GetValue(MarkerSettingsProperty); }
			set { SetValue(MarkerSettingsProperty, value); }
		}

		/// <summary>
		/// Gets or sets the value that indicates whether to show markers for the trackball.
		/// </summary>
		/// <value>It accepts <c>bool</c> values and its default value is <c>True</c>.</value>
		/// <example>
		/// # [Xaml](#tab/tabid-11)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///           <!--omitted for brevity-->
		///
		///           <chart:SfCartesianChart.TrackballBehavior>
		///               <chart:ChartTrackballBehavior ShowMarkers="False"/>
		///           </chart:SfCartesianChart.TrackballBehavior>
		///
		///           <chart:LineSeries ItemsSource="{Binding Data}"
		///                             XBindingPath="XValue"
		///                             YBindingPath="YValue"/>
		///
		///     </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [C#](#tab/tabid-12)
		/// <code><![CDATA[
		///     SfCartesianChart chart = new SfCartesianChart();
		///     ViewModel viewModel = new ViewModel();
		///
		///    // omitted for brevity 
		///    chart.TrackballBehavior = new ChartTrackballBehavior()
		///    {
		///        ShowMarkers = false,
		///    };
		///
		///     LineSeries series = new LineSeries();
		///     series.ItemsSource = viewModel.Data;
		///     series.XBindingPath = "XValue";
		///     series.YBindingPath = "YValue";
		///     chart.Series.Add(series);
		///
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public bool ShowMarkers
		{
			get { return (bool)GetValue(ShowMarkersProperty); }
			set { SetValue(ShowMarkersProperty, value); }
		}

		/// <summary>
		/// Gets or sets the value that indicates whether to show label for the trackball.
		/// </summary>
		/// <value>It accepts <c>bool</c> values and its default value is <c>True</c>.</value>
		/// <example>
		/// # [Xaml](#tab/tabid-13)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///           <!--omitted for brevity-->
		///
		///           <chart:SfCartesianChart.TrackballBehavior>
		///               <chart:ChartTrackballBehavior ShowLabel="False"/>
		///           </chart:SfCartesianChart.TrackballBehavior>
		///
		///           <chart:LineSeries ItemsSource="{Binding Data}"
		///                             XBindingPath="XValue"
		///                             YBindingPath="YValue"/>
		///
		///     </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [C#](#tab/tabid-14)
		/// <code><![CDATA[
		///     SfCartesianChart chart = new SfCartesianChart();
		///     ViewModel viewModel = new ViewModel();
		///
		///    // omitted for brevity 
		///    chart.TrackballBehavior = new ChartTrackballBehavior()
		///    {
		///        ShowLabel = false,
		///    };
		///
		///     LineSeries series = new LineSeries();
		///     series.ItemsSource = viewModel.Data;
		///     series.XBindingPath = "XValue";
		///     series.YBindingPath = "YValue";
		///     chart.Series.Add(series);
		///
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public bool ShowLabel
		{
			get { return (bool)GetValue(ShowLabelProperty); }
			set { SetValue(ShowLabelProperty, value); }
		}

		/// <summary>
		/// Gets or sets the value that indicates whether to show the trackball line.
		/// </summary>
		/// <value>It accepts <c>bool</c> values and its default value is <c>True</c>.</value>
		/// <example>
		/// # [Xaml](#tab/tabid-15)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///           <!--omitted for brevity-->
		///
		///           <chart:SfCartesianChart.TrackballBehavior>
		///               <chart:ChartTrackballBehavior ShowLine="False"/>
		///           </chart:SfCartesianChart.TrackballBehavior>
		///
		///           <chart:LineSeries ItemsSource="{Binding Data}"
		///                             XBindingPath="XValue"
		///                             YBindingPath="YValue"/>
		///
		///     </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [C#](#tab/tabid-16)
		/// <code><![CDATA[
		///     SfCartesianChart chart = new SfCartesianChart();
		///     ViewModel viewModel = new ViewModel();
		///
		///    // omitted for brevity 
		///    chart.TrackballBehavior = new ChartTrackballBehavior()
		///    {
		///        ShowLine = false,
		///    };
		///
		///     LineSeries series = new LineSeries();
		///     series.ItemsSource = viewModel.Data;
		///     series.XBindingPath = "XValue";
		///     series.YBindingPath = "YValue";
		///     chart.Series.Add(series);
		///
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public bool ShowLine
		{
			get { return (bool)GetValue(ShowLineProperty); }
			set { SetValue(ShowLineProperty, value); }
		}

		/// <summary>
		/// Gets or sets the value that indicates whether to activate trackball based on the specified touch mode.
		/// </summary>
		/// <value>It accepts <see cref="ChartTrackballActivationMode"/> values, with the default value being <see cref="ChartTrackballActivationMode.TouchMove"/> for Windows and Mac, and <see cref="ChartTrackballActivationMode.LongPress"/> for Android and iOS.</value>
		/// <remarks>
		/// On Windows, LongPress gesture is supported only through touch input, not with a mouse. Consequently, when ActivationMode is set to LongPress, the trackball activates only via touch interaction, not with a mouse interaction.
		/// </remarks>
		/// <example>
		/// # [Xaml](#tab/tabid-17)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///           <!--omitted for brevity-->
		///
		///           <chart:SfCartesianChart.TrackballBehavior>
		///               <chart:ChartTrackballBehavior ActivationMode="TouchMove"/>
		///           </chart:SfCartesianChart.TrackballBehavior>
		///
		///           <chart:LineSeries ItemsSource="{Binding Data}"
		///                             XBindingPath="XValue"
		///                             YBindingPath="YValue"/>
		///
		///     </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [C#](#tab/tabid-18)
		/// <code><![CDATA[
		///     SfCartesianChart chart = new SfCartesianChart();
		///     ViewModel viewModel = new ViewModel();
		///
		///    // omitted for brevity 
		///    chart.TrackballBehavior = new ChartTrackballBehavior()
		///    {
		///        ActivationMode = ChartTrackballActivationMode.TouchMove,
		///    };
		///
		///     LineSeries series = new LineSeries();
		///     series.ItemsSource = viewModel.Data;
		///     series.XBindingPath = "XValue";
		///     series.YBindingPath = "YValue";
		///     chart.Series.Add(series);
		///
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public ChartTrackballActivationMode ActivationMode
		{
			get { return (ChartTrackballActivationMode)GetValue(ActivationModeProperty); }
			set { SetValue(ActivationModeProperty, value); }
		}

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="ChartTrackballBehavior"/> class.
		/// </summary>
		public ChartTrackballBehavior()
		{
			ThemeElement.InitializeThemeResources(this, "SfCartesianChartTheme");
			SetDynamicResource(TrackballBackgroundProperty, "SfCartesianChartTrackballLabelBackground");
			SetDynamicResource(TrackballFontSizeProperty, "SfCartesianChartTrackballLabelTextFontSize");
			SetDynamicResource(LineStrokeProperty, "SfCartesianChartTrackballLineStroke");
			ShowMarkers = true;
			LineStyle = new ChartLineStyle() { Stroke = LineStroke, StrokeWidth = LineStrokeWidth };
			LabelStyle = _actualLabelStyle = DefaultLabelStyle();
			MarkerSettings = new ChartMarkerSettings() { Fill = SolidColorBrush.Black };
			_axisPointInfos = [];
			_previousAxisPointInfos = [];
			PointInfos = [];
			PreviousPointInfos = [];
			ContentList = [];
		}

		#endregion

		#region Interface Implementation

		bool IMarkerDependent.NeedToAnimateMarker { get { return _animateMarker; } set { _animateMarker = value; } }

		ChartMarkerSettings IMarkerDependent.MarkerSettings => MarkerSettings ?? new ChartMarkerSettings() { Fill = SolidColorBrush.Black };

		void IMarkerDependent.InvalidateDrawable()
		{
		}

		void IMarkerDependent.DrawMarker(ICanvas canvas, int index, ShapeType type, Rect rect)
		{
		}

		void IThemeElement.OnControlThemeChanged(string oldTheme, string newTheme)
		{
		}

		void IThemeElement.OnCommonThemeChanged(string oldTheme, string newTheme)
		{
		}

		#endregion

		#region Methods

		#region Public Methods

		/// <summary>
		/// Activate the trackball at the nearest point to the specified location.
		/// </summary>
		public void Show(float pointX, float pointY)
		{
			if (CartesianChart == null || CartesianChart is not IChart chart || CartesianChart._chartArea is not CartesianChartArea area)
			{
				return;
			}

			SeriesBounds = area.ActualSeriesClipRect;
			if (!_isScheduled)
			{
				if (chart.ActualSeriesClipRect.Contains(pointX, pointY))
				{
					_isScheduled = true;
					GenerateTrackball(pointX, pointY);
					_isScheduled = false;
				}

				_isScheduled = false;
			}
		}

		/// <summary>
		/// Hides the trackball that is visible in the chart.
		/// </summary>
		public void Hide()
		{
			if (CartesianChart?.BehaviorLayout.Children.Count > 0)
			{
				RemoveTrackballTemplateInfo(CartesianChart);
			}

			PointInfos.Clear();
			PreviousPointInfos.Clear();
			ContentList.Clear();
			_previousAxisPointInfos.Clear();
			RemoveViews();
			GroupedLabelView = null;
			_axisPointInfos.Clear();
			Invalidate();
		}

		#endregion

		#region Protected Methods

		/// <inheritdoc/>
		/// <exclude/>
		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();

			if (MarkerSettings != null)
			{
				SetInheritedBindingContext(MarkerSettings, BindingContext);
			}

			if (LineStyle != null)
			{
				SetInheritedBindingContext(LineStyle, BindingContext);
			}

			if (LabelStyle != null)
			{
				SetInheritedBindingContext(LabelStyle, BindingContext);
			}
		}

		/// <inheritdoc/>
		protected internal override void OnTouchMove(ChartBase chart, float pointX, float pointY)
		{
			if (chart.SeriesBounds.Contains(pointX, pointY))
			{
				if (ActivationMode == ChartTrackballActivationMode.TouchMove)
				{
					_canAutoHideOnExit = true;
					Show(pointX, pointY);
				}
				else if (ActivationMode == ChartTrackballActivationMode.LongPress)
				{
					_canAutoHideOnExit = true;

					if (LongPressActive)
					{
						Show(pointX, pointY);
					}
				}
			}
			else
			{
				_canAutoHideOnExit = false;
				Hide();
			}
		}

		/// <inheritdoc/>
		protected internal override void OnTouchUp(ChartBase chart, float pointX, float pointY)
		{
			if (IsPressed)
			{
				IsPressed = false;
			}

			LongPressActive = false;
			Hide();
		}

		/// <inheritdoc/>
		protected internal override void OnTouchDown(ChartBase chart, float pointX, float pointY)
		{
#if WINDOWS || MACCATALYST
            if (ActivationMode == ChartTrackballActivationMode.TouchMove && DeviceType == PointerDeviceType.Touch)
#elif IOS || ANDROID
			if (ActivationMode == ChartTrackballActivationMode.TouchMove)
#endif
			{
				IsPressed = true;
				Show(pointX, pointY);
			}

			if (!IsPressed)
			{
				Hide();
			}
		}

		/// <summary>
		/// Invokes when the trackball is moved from one data point to another. This helps to customize the trackball label and marker based on the condition.
		/// </summary>
		protected internal virtual void LabelsGenerated(List<TrackballPointInfo> pointInfos)
		{
		}

		#endregion

		#region Internal Methods

		internal override void OnLongPressActivation(IChart chart, float pointX, float pointY, GestureStatus status)
		{
			if (chart is SfCartesianChart cartesianChart)
			{
				if ((cartesianChart.ZoomPanBehavior == null || cartesianChart.ZoomPanBehavior is ChartZoomPanBehavior behavior && !behavior.IsSelectionZoomingActivated) && ((status != GestureStatus.Completed && status != GestureStatus.Canceled) && ActivationMode == ChartTrackballActivationMode.LongPress))
				{
#if WINDOWS
                    IsPressed = true;
#endif
					LongPressActive = true;
					Show(pointX, pointY);
				}
			}
		}

		internal override void OnTouchCancel(float x, float y)
		{
			if (LongPressActive)
			{
				LongPressActive = false;
			}

			Hide();
		}

		internal override void OnTouchExit()
		{
			if (_canAutoHideOnExit)
			{
				Hide();
			}
		}

		internal void DrawElements(ICanvas canvas, Rect dirtyRect)
		{
			if (CartesianChart == null)
			{
				return;
			}

			var rect = dirtyRect.SubtractThickness(CartesianChart._chartArea.PlotAreaMargin);

			if (ShowLine)
			{
				DrawTrackballLine(canvas);
			}

			canvas.ResetStrokeDashPattern();
			canvas.Translate(rect.Left, rect.Top);

			DrawTrackballLabels(canvas); //Draw only if not grouped label. 

			foreach (var item in _axisPointInfos)
			{
				if (ShowLabel && !item.HasTrackballAxisTemplate)
				{
					item.Helper.Draw(canvas);
				}
			}
		}

		#endregion

		#region Private Methods

		void GenerateTrackball(float pointX, float pointY)
		{
			if (CartesianChart != null && (CartesianChart is IChart chart))
			{
				PreviousPointInfos = new List<TrackballPointInfo>(PointInfos);
				PointInfos.Clear();
				_isAnySideBySideSeries = false;
				_isAnyContinuesSeries = false;
				var xAxes = CartesianChart._chartArea?._xAxes;

				if (xAxes == null || xAxes.Count == 0)
				{
					return;
				}

				foreach (ChartAxis chartAxis in xAxes)
				{
					foreach (CartesianSeries series in chartAxis.RegisteredSeries.Cast<CartesianSeries>())
					{
						if (series.IsVisible && series.ShowTrackballLabel && series.PointsCount > 0)
						{
							List<object> nearestDataPoints = series.FindNearestChartPoints(pointX, pointY);
							if (nearestDataPoints.Count > 0)
							{
								if (series.IsSideBySide)
								{
									_isAnySideBySideSeries = true;
								}
								else
								{
									_isAnyContinuesSeries = true;
								}

								series.GenerateTrackballPointInfo(nearestDataPoints, PointInfos, ref _isAnySideBySideSeries);
							}
						}
					}
				}

				UpdateTrackballPointInfos(pointX - (float)chart.ActualSeriesClipRect.Left, pointY - (float)chart.ActualSeriesClipRect.Top);
				Invalidate();
			}
		}

		void UpdateTrackballPointInfos(float pointX, float pointY)
		{
			if (CartesianChart == null || PointInfos.Count == 0)
			{
				Hide();
				return;
			}

			float leastX = FindLeastXValue(pointX, pointY);

			if (CartesianChart.IsTransposed)
			{
				_linePoint1 = new Point(SeriesBounds.Left, leastX);
				_linePoint2 = new Point(SeriesBounds.Right, leastX);
			}
			else
			{
				_linePoint1 = new Point(leastX - SeriesBounds.Left, SeriesBounds.Top);
				_linePoint2 = new Point(leastX - SeriesBounds.Left, SeriesBounds.Top + SeriesBounds.Height);
			}

			//TODO: Validate for group label.
			if (DisplayMode == LabelDisplayMode.NearestPoint || (_isAnySideBySideSeries && DisplayMode != LabelDisplayMode.GroupAllPoints))
			{
				ValidateTrackballBehaviorForAllSeries(leastX, pointX, pointY);
			}

			if (PointInfos.Count == 0)
			{
				// if point info zero means the axis and previous trackball info no need to create ,so remove the template view from layout.
				Hide();
				return;
			}

			foreach (var items in PointInfos)
			{
				items.LabelStyle = _actualLabelStyle;
				items.MarkerSettings = ((IMarkerDependent)this).MarkerSettings;
			}

			LabelsGenerated(PointInfos);

			CartesianChart.RaiseTrackballCreatedEvent(PointInfos);

			if (DisplayMode == LabelDisplayMode.GroupAllPoints)
			{
				if (PointInfos.Count > 0)
				{
					GenerateTrackballGroupedLabel();

					if (GroupedLabelView != null)
					{
						if (!CartesianChart._trackballView.Contains(GroupedLabelView))
						{
							CartesianChart._trackballView.Add(GroupedLabelView);
						}
					}

					UpdateGroupedTooltip();
				}
			}

			foreach (var item in PointInfos)
			{
				CustomizeAppearance(item);
			}

			if (ShowLabel && DisplayMode == LabelDisplayMode.FloatAllPoints)
			{
				if (PointInfos.Count == 0)
				{
					return;
				}

				SmartLabelAlignment(CartesianChart);
			}

			GenerateAxisTrackballInfos(leastX);

			if (PreviousPointInfos.Count > 0)
			{
				foreach (var info in PreviousPointInfos)
				{
					if (CartesianChart.BehaviorLayout.Contains(info.ContentTemplateView))
					{
						CartesianChart.BehaviorLayout.Remove(info.ContentTemplateView);
					}
				}

				PreviousPointInfos.Clear();
			}

			if (_previousAxisPointInfos.Count > 0)
			{
				foreach (var info in _previousAxisPointInfos)
				{
					if (CartesianChart.BehaviorLayout.Contains(info.AxisTemplateView))
					{
						CartesianChart.BehaviorLayout.Remove(info.AxisTemplateView);
					}
				}

				_previousAxisPointInfos.Clear();
			}
		}

		void GenerateTrackballGroupedLabel()
		{
			GroupedLabelView ??= new SfTooltip
				{
					Duration = double.NaN,
					Background = _actualLabelStyle.Background
				};

			var cellContent = new SfItemViewCell
			{
				ItemTemplate = new DataTemplate(() =>
			{

				var layout = new VerticalStackLayout();

				BindableLayout.SetItemsSource(layout, PointInfos);
				var labelTemplate = new DataTemplate(() =>
				{
					var label = new Label
					{
						LineBreakMode = LineBreakMode.NoWrap
					};
					label.SetBinding(Label.TextProperty, new Binding() { Path = "Label" });
					label.VerticalOptions = LayoutOptions.Fill;
					label.HorizontalOptions = LayoutOptions.Fill;
					label.VerticalTextAlignment = TextAlignment.Center;
					label.HorizontalTextAlignment = TextAlignment.Center;
					label.SetBinding(Label.TextColorProperty, new Binding() { Path = "LabelStyle.TextColor" });
					label.SetBinding(Label.BackgroundProperty, new Binding() { Path = "LabelStyle.Background" });
					label.SetBinding(Label.FontAttributesProperty, new Binding() { Path = "LabelStyle.FontAttributes" });
					label.SetBinding(Label.FontFamilyProperty, new Binding() { Path = "LabelStyle.FontFamily" });
					label.SetBinding(Label.FontSizeProperty, new Binding() { Path = "LabelStyle.FontSize" });
					label.SetBinding(Label.MarginProperty, new Binding() { Path = "LabelStyle.Margin" });
					return label;
				});

				BindableLayout.SetItemTemplate(layout, labelTemplate);

				return layout;
			}),

				Item = PointInfos
			};

			GroupedLabelView.Content = cellContent;
		}

		void GenerateAxisTrackballInfos(float leastX)
		{
			_previousAxisPointInfos = new List<TrackballAxisInfo>(_axisPointInfos);
			_axisPointInfos.Clear();

			if (CartesianChart == null)
			{
				return;
			}

			ChartAxis? previousAxis = null;

			foreach (TrackballPointInfo pointInfo in PointInfos)
			{
				ChartAxis? axis = pointInfo.Series.ActualXAxis;

				if (axis == null || !axis.ShowTrackballLabel || !axis.IsVisible || previousAxis == axis)
				{
					continue;
				}

				previousAxis = axis;
				float x = float.NaN;
				float y = float.NaN;
				double xValue = pointInfo.XValue;
				double offset = 0;

				if (axis.AxisLineStyle != null)
				{
					offset = axis.AxisLineStyle.StrokeWidth / 2;
				}

				bool isOpposed = axis.IsOpposed();

				Rect actualArrangeRect = axis.ArrangeRect;

				if (axis.IsVertical)
				{
					y = leastX - SeriesBounds.Top;
					x = (float)(isOpposed ? actualArrangeRect.X - offset : actualArrangeRect.Right - SeriesBounds.X + offset);
				}
				else
				{
					x = (float)(leastX - actualArrangeRect.X);
					y = (float)(isOpposed ? actualArrangeRect.Bottom - SeriesBounds.Y - offset : actualArrangeRect.Y + offset);
				}

				string labelFormat = "##.##";

				if (axis.TrackballLabelStyle != null)
				{
					labelFormat = axis.TrackballLabelStyle.LabelFormat;
				}
				else
				{
					//Todo: need to check all axis based on label format.
					if (axis is DateTimeAxis)
					{
						//Default Label format assign it.
						labelFormat = "MM-dd-yyyy";
					}
				}

				TrackballAxisInfo axisPointInfo = new TrackballAxisInfo(axis, new TooltipHelper(Drawable) { Duration = int.MaxValue }, ChartTrackballBehavior.GetAxisLabel(axis, xValue, labelFormat), x, y);

				if (axisPointInfo.HasTrackballAxisTemplate)
				{
					TrackballAxisInfo? prevTrackballAxisInfo = _previousAxisPointInfos.Find(previousPointInfo =>
						previousPointInfo.Label == axisPointInfo.Label);

					SfTooltip? trackballTemp = prevTrackballAxisInfo?.AxisTemplateView;

					if (trackballTemp == null && ShowLabel)
					{
						trackballTemp = new SfTooltip
						{
							Duration = double.NaN
						};
						CartesianChart.BehaviorLayout.Add(trackballTemp);
						trackballTemp.Helper.CanNosePointTarget = true;
						trackballTemp.Background = TrackballBackground;
						ContentList.Add(trackballTemp);
					}

					if (trackballTemp != null)
					{
						if (!CartesianChart.IsTransposed)
						{
							trackballTemp.Position = isOpposed ? TooltipPosition.Top : TooltipPosition.Bottom;
						}
						else
						{
							trackballTemp.Position = isOpposed ? TooltipPosition.Right : TooltipPosition.Left;
						}

						axisPointInfo.AxisTemplateView = trackballTemp;

						if (axisPointInfo.Label != prevTrackballAxisInfo?.Label)
						{
							trackballTemp.BindingContext = axisPointInfo.Label;
							trackballTemp.Content = ChartTrackballBehavior.GetTheTrackballTemplate(axis.TrackballLabelTemplate, axisPointInfo.Label);
						}

						if (prevTrackballAxisInfo != null)
						{
							if (_previousAxisPointInfos.Contains(prevTrackballAxisInfo))
							{
								_previousAxisPointInfos.Remove(prevTrackballAxisInfo);
							}
						}

						trackballTemp.Show(actualArrangeRect, new Rect(x - 1, y - 1, 2, 2), false);
					}
				}
				else
				{
					axisPointInfo.Helper.FontManager = CartesianChart.GetFontManager();

					var labelStyle = axis.TrackballLabelStyle ?? ChartTrackballBehavior.DefaultAxisLabelStyle(axis);

					MapChartLabelStyle(axisPointInfo.Helper, labelStyle);
					if (!CartesianChart.IsTransposed)
					{
						axisPointInfo.Helper.Position = isOpposed ? TooltipPosition.Top : TooltipPosition.Bottom;
					}
					else
					{
						axisPointInfo.Helper.Position = isOpposed ? TooltipPosition.Right : TooltipPosition.Left;
					}

					axisPointInfo.Helper.Show(actualArrangeRect, new Rect(x - 1, y - 1, 2, 2), false);
				}

				_axisPointInfos.Add(axisPointInfo);
			}
		}

		void SmartLabelAlignment(SfCartesianChart CartesianChart)
		{
			if (PointInfos.Count > 0)
			{
				//TODO: Enable for group all points.
				//if (DisplayMode != LabelDisplayMode.GroupAllPoints)
				//{
				if (CartesianChart.IsTransposed)
				{
					PointInfos = [.. PointInfos.Where(item => IsRectContainsPoints(item)).OrderBy(item => item.X)];
				}
				else
				{
					PointInfos = [.. PointInfos.Where(item => IsRectContainsPoints(item)).OrderBy(item => item.Y)];
				}
				//}
			}

			if (PointInfos.Count == 0)
			{
				Hide();
				return;
			}

			List<TrackballPointInfo> tempTrackballInfo = new List<TrackballPointInfo>(PointInfos);
			_intersectedGroups = [];
			List<TrackballPointInfo> tempIntersectedLabels = [tempTrackballInfo[0]];

			for (int i = 0; i < tempTrackballInfo.Count - 1; i++)
			{
				var tempTrackballInfo1 = tempTrackballInfo[i];
				var tempTrackballInfo2 = tempTrackballInfo[i + 1];
				Rect rect1 = tempTrackballInfo1.HaveTemplateView && tempTrackballInfo1.ContentTemplateView != null ? tempTrackballInfo1.ContentTemplateView.Helper.TooltipRect : tempTrackballInfo1.TooltipHelper.TooltipRect;
				Rect rect2 = tempTrackballInfo2.HaveTemplateView && tempTrackballInfo2.ContentTemplateView != null ? tempTrackballInfo2.ContentTemplateView.Helper.TooltipRect : tempTrackballInfo2.TooltipHelper.TooltipRect;
				if (rect1.IntersectsWith(rect2))
				{
					tempIntersectedLabels.Add(tempTrackballInfo2);
				}
				else
				{
					_intersectedGroups.Add(new List<TrackballPointInfo>(tempIntersectedLabels));
					tempIntersectedLabels.Clear();
					tempIntersectedLabels.Add(tempTrackballInfo2);
				}
			}

			if (tempIntersectedLabels.Count > 0)
			{
				_intersectedGroups.Add(new List<TrackballPointInfo>(tempIntersectedLabels));
				tempIntersectedLabels.Clear();
			}

			UpdatePositionForIntersectedLabels();
		}

		void UpdatePositionForIntersectedLabels()
		{
			if (CartesianChart is IChart chart && _intersectedGroups != null)
			{
				var prevRect = Rect.Zero;
				foreach (var intersectedGroupLabels in _intersectedGroups)
				{
					if (!CartesianChart.IsTransposed)
					{
						HandleMultipleGroupLabels(intersectedGroupLabels, chart, ref prevRect);
					}
					else
					{
						HandleTransposedMultipleGroupLabels(intersectedGroupLabels, chart, ref prevRect);
					}
				}
			}
		}

		// Method to handle multiple group labels
		void HandleMultipleGroupLabels(List<TrackballPointInfo> intersectedGroupLabels, IChart chart, ref Rect prevRect)
		{
			var info = intersectedGroupLabels[0];
			Rect infoTooltipRect = info.HaveTemplateView && info.ContentTemplateView != null ? info.ContentTemplateView.Helper.TooltipRect : info.TooltipHelper.TooltipRect;
			double tempYValue = info.Y;
			double intersectedGroupLabelsCount = intersectedGroupLabels.Count;
			double halfHeight = ((infoTooltipRect.Height * intersectedGroupLabelsCount)
								+ _trackLabelSpacing * (intersectedGroupLabelsCount - 1)) / 2 - infoTooltipRect.Height / 2;
			if (info.Y - halfHeight <= 0)
			{
				tempYValue = tempYValue + halfHeight - info.Y + _trackLabelSpacing;
			}

			for (int i = 0; i < intersectedGroupLabelsCount; i++)
			{
				var intersectedGroupLabel = intersectedGroupLabels[i];
				Rect rect = intersectedGroupLabel.HaveTemplateView && intersectedGroupLabel.ContentTemplateView != null ? intersectedGroupLabel.ContentTemplateView.Helper.TooltipRect : intersectedGroupLabel.TooltipHelper.TooltipRect;

				if (intersectedGroupLabelsCount > 1)
				{
					rect.Y = tempYValue - halfHeight + (i * (rect.Height + _trackLabelSpacing));
				}

				if (rect.IntersectsWith(prevRect) || (rect.Y < prevRect.Y))
				{
					rect.Y = prevRect.Y + prevRect.Height + _trackLabelSpacing;
				}

				if (rect.Y + rect.Height > chart.ActualSeriesClipRect.Height)
				{
					HandleOverflowingLabel(intersectedGroupLabels);
				}
				else
				{
					if (intersectedGroupLabel.HaveTemplateView && intersectedGroupLabel.ContentTemplateView != null)
					{
						intersectedGroupLabel.ContentTemplateView.Helper.TooltipRect = rect;
						AbsoluteLayout.SetLayoutBounds(intersectedGroupLabel.ContentTemplateView, rect);
					}
					else
					{
						intersectedGroupLabel.TooltipHelper.TooltipRect = rect;
					}
					prevRect = rect;
				}
			}
		}

		void HandleTransposedMultipleGroupLabels(List<TrackballPointInfo> intersectedGroupLabels, IChart chart, ref Rect transposedPrevRect)
		{
			var info = intersectedGroupLabels[0];
			var infoTooltipRect = info.HaveTemplateView && info.ContentTemplateView != null ? info.ContentTemplateView.Helper.TooltipRect : info.TooltipHelper.TooltipRect;
			double tempXValue = info.X;
			double intersectedGroupLabelsCount = intersectedGroupLabels.Count;
			double halfWidth = ((infoTooltipRect.Width * intersectedGroupLabelsCount)
							   + _trackLabelSpacing * (intersectedGroupLabelsCount - 1)) / 2 - infoTooltipRect.Width / 2;
			if (info.X - halfWidth <= 0)
			{
				tempXValue = tempXValue + halfWidth - info.X + _trackLabelSpacing;
			}

			for (int i = 0; i < intersectedGroupLabelsCount; i++)
			{
				var intersectedGroupLabel = intersectedGroupLabels[i];
				var rect = intersectedGroupLabel.HaveTemplateView && intersectedGroupLabel.ContentTemplateView != null ? intersectedGroupLabel.ContentTemplateView.Helper.TooltipRect : intersectedGroupLabel.TooltipHelper.TooltipRect;

				if (intersectedGroupLabelsCount > 1)
				{
					rect.X = tempXValue - halfWidth + (i * (rect.Width + _trackLabelSpacing));
				}

				if (rect.IntersectsWith(transposedPrevRect))
				{
					rect.X = transposedPrevRect.X + transposedPrevRect.Width + _trackLabelSpacing;
				}

				if (rect.X + rect.Width > chart.ActualSeriesClipRect.Width)
				{
					HandleOverflowingLabel(intersectedGroupLabels);
				}
				else
				{
					if (intersectedGroupLabel.HaveTemplateView && intersectedGroupLabel.ContentTemplateView != null)
					{
						intersectedGroupLabel.ContentTemplateView.Helper.TooltipRect = rect;
						AbsoluteLayout.SetLayoutBounds(intersectedGroupLabel.ContentTemplateView, rect);
					}
					else
					{
						intersectedGroupLabel.TooltipHelper.TooltipRect = rect;
					}
					transposedPrevRect = rect;
				}
			}
		}

		// Method to handle overflowing label
		void HandleOverflowingLabel(List<TrackballPointInfo> intersectedGroupLabels)
		{
			if (CartesianChart is IChart chart && _intersectedGroups != null)
			{
				var chartActualClipRect = chart.ActualSeriesClipRect;
				var bottomPrevRect = Rect.Zero;
				bool isAlignmentStarted = false;

				if (intersectedGroupLabels.Count >= 1)
				{
					for (int j = _intersectedGroups.Count - 1; j >= 0; j--)
					{
						var newIntersectedGroups = _intersectedGroups[j];
						for (int k = newIntersectedGroups.Count - 1; k >= 0; k--)
						{
							var info = newIntersectedGroups[k];
							Rect rect = info.HaveTemplateView && info.ContentTemplateView != null ? info.ContentTemplateView.Helper.TooltipRect : info.TooltipHelper.TooltipRect;

							if (!CartesianChart.IsTransposed)
							{
								var rectArranged = new Rect(bottomPrevRect.X, bottomPrevRect.Y, chartActualClipRect.Width - bottomPrevRect.X, chartActualClipRect.Height - bottomPrevRect.Y);
								if (rect.IntersectsWith(bottomPrevRect) || (rectArranged.Contains(rect.X, rect.Y) && isAlignmentStarted))
								{
									rect.Y = bottomPrevRect.Y - _trackLabelSpacing - rect.Height;
								}
								else if (rect.Y + rect.Height >= chartActualClipRect.Height && !isAlignmentStarted)
								{
									rect.Y = chartActualClipRect.Height - rect.Height;
									isAlignmentStarted = true;
								}
							}
							else
							{
								var rectArranged = new Rect(bottomPrevRect.X, 0, chartActualClipRect.Width - bottomPrevRect.X, chartActualClipRect.Height);

								if (rect.IntersectsWith(bottomPrevRect) || (rectArranged.Contains(rect.X, rect.Y) && isAlignmentStarted))
								{
									rect.X = bottomPrevRect.X - _trackLabelSpacing - rect.Width;
								}
								else if (rect.X + rect.Width >= chartActualClipRect.Width && !isAlignmentStarted)
								{
									rect.X = chartActualClipRect.Width - rect.Width;
									isAlignmentStarted = true;
								}
							}

							if (info.HaveTemplateView && info.ContentTemplateView != null)
							{
								info.ContentTemplateView.Helper.TooltipRect = rect;
								AbsoluteLayout.SetLayoutBounds(info.ContentTemplateView, rect);
							}
							else
							{
								info.TooltipHelper.TooltipRect = rect;
							}
							bottomPrevRect = rect;
						}
					}
				}
			}
		}

		void MapChartLabelStyle(TooltipHelper helper, ChartLabelStyle chartLabelStyle)
		{
			helper.FontAttributes = chartLabelStyle.FontAttributes;
			helper.FontFamily = chartLabelStyle.FontFamily;
			helper.FontSize = chartLabelStyle.FontSize;
			helper.Padding = chartLabelStyle.Margin;
			helper.Stroke = chartLabelStyle.Stroke;
			helper.StrokeWidth = (float)chartLabelStyle.StrokeWidth;
			helper.Background = chartLabelStyle.Background;
			helper.TextColor = GetTextColor(chartLabelStyle);
			helper.Font = ((ITextElement)chartLabelStyle).Font;
		}

		Color GetTextColor(ChartLabelStyle chartLabelStyle)
		{
			var background = chartLabelStyle.Background;
			if (!chartLabelStyle.IsTextColorUpdated)
			{
				var fontColor = background == default(Brush) || background.ToColor() == Colors.Transparent ?
						CartesianChart.GetTextColorBasedOnChartBackground() :
						ChartUtils.GetContrastColor((background as SolidColorBrush).ToColor());

				return fontColor;
			}
			else
			{
				return chartLabelStyle.TextColor;
			}
		}

		void UpdateGroupedTooltip()
		{
			if (CartesianChart == null || GroupedLabelView == null)
			{
				return;
			}

			var plotAreaMargin = CartesianChart._chartArea.PlotAreaMargin;
			CartesianChart._trackballView.Padding = plotAreaMargin;
			var helper = GroupedLabelView.Helper;
			helper.Duration = int.MaxValue;

			if (GroupedLabelView.Content != null)
			{
				Size size = GroupedLabelView.Content.Measure(double.PositiveInfinity, double.PositiveInfinity);
				var padding = GroupedLabelView.Helper.Padding;
				Size contentSize = new Size(size.Width + padding.Left + padding.Right, size.Height + padding.Top + padding.Bottom);

				if (CartesianChart.IsTransposed)
				{
					GroupedLabelView.Helper.PriorityPosition = TooltipPosition.Right;
					var xPos = SeriesBounds.Width - (contentSize.Width + helper._noseWidth + helper._noseOffset);
					GroupedLabelView.Show(SeriesBounds, new Rect(xPos, _linePoint2.Y, 1, 1), false);
				}
				else
				{
					GroupedLabelView.Helper.PriorityPosition = TooltipPosition.Top;
					var yPos = SeriesBounds.Top - _linePoint1.Y + contentSize.Height + helper._noseHeight + helper._noseOffset;
					GroupedLabelView.Show(SeriesBounds, new Rect(_linePoint1.X, yPos, 1, 1), false);
				}
			}
		}

		void RemoveViews()
		{
			if (GroupedLabelView != null && CartesianChart != null)
			{
				if (CartesianChart._trackballView.Contains(GroupedLabelView))
				{
					CartesianChart._trackballView.Remove(GroupedLabelView);
				}
			}
		}

		float FindLeastXValue(float pointX, float pointY)
		{
			if (CartesianChart == null)
			{
				return float.NaN;
			}

			var xAxes = CartesianChart._chartArea?._xAxes;

			if (xAxes == null)
			{
				return float.NaN;
			}

			bool isTransposed = CartesianChart.IsTransposed;


			double startXValue = isTransposed
								 ? SeriesBounds.Top
								 : SeriesBounds.Left;

			double nearPointX = startXValue;

			double touchXValue = isTransposed ? pointY + SeriesBounds.Top : pointX + SeriesBounds.Left;
			double delta = 0;

			List<TrackballPointInfo> leastXPointsInfo = [];

			foreach (TrackballPointInfo pointInfo in PointInfos)
			{
				var axis = pointInfo.Series.ActualXAxis;
				if (axis == null)
				{
					continue;
				}

				_currX = axis.ValueToPoint(pointInfo.XValue) + (float)startXValue;

				var difference = Math.Abs(touchXValue - _currX);

				if (delta == touchXValue - _currX)
				{
					nearPointX = _currX;
					leastXPointsInfo.Add(pointInfo);
				}
				else if (difference <= Math.Abs(touchXValue - nearPointX))
				{
					nearPointX = _currX;
					delta = touchXValue - _currX;
					leastXPointsInfo.Clear();
					leastXPointsInfo.Add(pointInfo);
				}

			}

			var copyList = PointInfos.ToList();

			foreach (var pointInfo in copyList)
			{
				if (!leastXPointsInfo.Contains(pointInfo))
				{
					RemoveTrackballInfo(pointInfo);
				}
			}

			float leastX = float.NaN;

			if (PointInfos.Count > 0)
			{
				TrackballPointInfo pointInfo = PointInfos[0];

				var axis = pointInfo.Series.ActualXAxis;
				if (axis == null)
				{
					return float.NaN;
				}

				leastX = (float)(axis.ValueToPoint(pointInfo.XValue) + startXValue);

				leastX = (float)Math.Round(leastX, 3);
			}

			return leastX;
		}

		void ValidateTrackballBehaviorForAllSeries(double leastX, double pointX, double pointY)
		{
			if (CartesianChart == null)
			{
				return;
			}

			List<TrackballPointInfo> tempTrackballPointInfos = new List<TrackballPointInfo>(PointInfos);
			bool isTransposed = CartesianChart.IsTransposed;

			tempTrackballPointInfos = isTransposed ? [.. tempTrackballPointInfos.OrderBy(a => a.X)] : [.. tempTrackballPointInfos.OrderBy(a => a.Y)];

			foreach (TrackballPointInfo pointInfo in tempTrackballPointInfos)
			{
				CartesianSeries series = pointInfo.Series;
				ChartAxis? axis = series.ActualXAxis;
				ChartAxis? verticalAxis = series.ActualYAxis;

				if (axis == null || verticalAxis == null)
				{
					return;
				}

				double locationX = isTransposed ? pointY + SeriesBounds.Top : pointX + SeriesBounds.Left;
				double locationY = isTransposed ? pointX + SeriesBounds.Left : pointY + SeriesBounds.Top;

				double startXValue = isTransposed ? SeriesBounds.Top : SeriesBounds.Left;

				//TODO: Validate for grouped label.
				//if (series.IsSideBySide && CartesianChart.EnableSideBySideSeriesPlacement && DisplayMode != LabelDisplayMode.GroupAllPoints)
				if (series.IsSideBySide && CartesianChart.EnableSideBySideSeriesPlacement)
				{
					DoubleRange sbsInfo = series.SbsInfo;
					double xVal = pointInfo.XValue;
					bool isXaxisInversed = axis.IsInversed;
					double xStartValue = xVal + sbsInfo.Start;
					double xEndValue = xVal + sbsInfo.End;
					double xEnd = axis.ValueToPoint(xEndValue) + startXValue;

					double xStart = axis.ValueToPoint(xStartValue) + startXValue;
					bool isStartIndex = series.SideBySideIndex == 0;
					bool isEndIndex = series.SideBySideIndex == axis.SideBySideSeriesCount - 1;

					if (isXaxisInversed || isTransposed)
					{
						if (!(isXaxisInversed && isTransposed))
						{
							(xEnd, xStart) = (xStart, xEnd);
							(isStartIndex, isEndIndex) = (isEndIndex, isStartIndex);
						}
					}

					if (locationX < leastX && isStartIndex)
					{
						if (!(locationX < xStart) && !(locationX < xEnd && locationX >= xStart))
						{
							RemoveTrackballInfo(pointInfo);
						}
					}
					else if (locationX > leastX && isEndIndex)
					{
						if (!(locationX > xEnd && locationX > xStart) && !(locationX < xEnd && locationX >= xStart))
						{
							RemoveTrackballInfo(pointInfo);
						}
					}
					else if (!(locationX < xEnd && locationX >= xStart))
					{
						RemoveTrackballInfo(pointInfo);
					}
				}

				//Todo: Here need to check StackingSeries.

				if (DisplayMode != LabelDisplayMode.FloatAllPoints &&
					((_isAnyContinuesSeries) || (!isTransposed && !CartesianChart.EnableSideBySideSeriesPlacement) ||
					 (pointInfo.X == leastX)))
				{
					int pointInfoIndex = tempTrackballPointInfos.IndexOf(pointInfo);

					if (pointInfoIndex < tempTrackballPointInfos.Count - 1)
					{
						TrackballPointInfo nextPointInfo = tempTrackballPointInfos[(pointInfoIndex + 1)];

						if (nextPointInfo.Y == pointInfo.Y || (pointInfo.Y > locationY && pointInfoIndex == 0))
						{
							//Todo: When provide RangeAreaSeries, need to uncomment these codes. 
							//if (!(series is RangeAreaSeries))
							//{
							//    continue;
							//}
						}

						if (!(locationY < (nextPointInfo.Y - ((nextPointInfo.Y - pointInfo.Y) / 2))))
						{
							RemoveTrackballInfo(pointInfo);
						}
						else if (pointInfoIndex != 0)
						{
							TrackballPointInfo previousPointInfo = tempTrackballPointInfos[pointInfoIndex - 1];

							if (locationY < (pointInfo.Y - ((pointInfo.Y - previousPointInfo.Y) / 2)))
							{
								RemoveTrackballInfo(pointInfo);
							}
						}
					}
					else
					{
						if (pointInfoIndex != 0 && pointInfoIndex == tempTrackballPointInfos.Count - 1)
						{
							TrackballPointInfo previousPointInfo = tempTrackballPointInfos[pointInfoIndex - 1];

							if (locationY < previousPointInfo.Y)
							{
								RemoveTrackballInfo(pointInfo);
							}

							if (locationY < (pointInfo.Y - ((pointInfo.Y - previousPointInfo.Y) / 2)))
							{
								RemoveTrackballInfo(pointInfo);
							}
						}
					}
				}
			}

			//Todo: When provide RangeAreaSeries, need to uncomment these codes. 
			//iOS-1812: The below code has been implemented since the trackball nearest point for rangeArea series need to show both high and low values
			//if (PointInfos.Count > 0)
			//{
			//    List<TrackballPointInfo> trackballPoints = new List<TrackballPointInfo>();

			//    foreach (TrackballPointInfo points in PointInfos)
			//    {
			//        if (points.Series is RangeAreaSeries)
			//        {
			//            foreach (TrackballPointInfo rangePoint in tempTrackballPointInfos)
			//            {
			//                if (rangePoint.Series == points.Series && rangePoint != points)
			//                {
			//                    trackballPoints.Add(rangePoint);
			//                }
			//            }
			//        }
			//    }

			//    foreach (var values in trackballPoints)
			//    {
			//        PointInfos.Add(values);
			//    }
			//}
		}

		void RemoveTrackballInfo(TrackballPointInfo pointInfo)
		{
			if (PointInfos.Contains(pointInfo))
			{
				PointInfos.Remove(pointInfo);

				if (pointInfo.HaveTemplateView)
				{
					foreach (var info in PreviousPointInfos)
					{
						if (info.DataItem == pointInfo.DataItem && info.Series == pointInfo.Series)
						{
							CartesianChart?.BehaviorLayout.Remove(info.ContentTemplateView);
							PreviousPointInfos.Remove(info);

							return;
						}
					}
				}
			}
		}

		void CustomizeAppearance(TrackballPointInfo pointInfo)
		{
			if (CartesianChart == null)
			{
				return;
			}

			if (pointInfo != null)
			{
				pointInfo.SetTargetRect(this);

				//Validate not for grouped all points.
				if (DisplayMode != LabelDisplayMode.GroupAllPoints)
				{
					if (pointInfo.HaveTemplateView && CartesianChart is SfCartesianChart chart)
					{
						TrackballPointInfo? prevTrackballInfo = PreviousPointInfos.Find(previousPointInfo =>
														previousPointInfo.DataItem == pointInfo.DataItem && previousPointInfo.Series == pointInfo.Series);

						var trackballView = GetOrCreateTrackballView(chart, prevTrackballInfo);

						if (trackballView is not null)
						{
							pointInfo.ContentTemplateView = trackballView;

							if (pointInfo.DataItem != prevTrackballInfo?.DataItem)
							{
								trackballView.BindingContext = pointInfo;
								trackballView.Content = ChartTrackballBehavior.GetTheTrackballTemplate(pointInfo.Series.TrackballLabelTemplate, pointInfo);
								ChartTrackballBehavior.SetTemplatePosition(chart, trackballView);
							}

							trackballView.IsVisible = IsRectContainsPoints(pointInfo);

							trackballView.Show(SeriesBounds, pointInfo.TargetRect, false);
							PreviousPointInfos.Remove(prevTrackballInfo!);
						}
					}
					else if (IsRectContainsPoints(pointInfo))
					{
						var labelStyle = pointInfo.LabelStyle ?? _actualLabelStyle;
						if (!string.IsNullOrEmpty(labelStyle.LabelFormat) && labelStyle.LabelFormat != "#.##")
						{
							pointInfo.Series.ApplyTrackballLabelFormat(pointInfo, labelStyle.LabelFormat);
						}

						MapChartLabelStyle(pointInfo.TooltipHelper, labelStyle);
						pointInfo.ShowTrackballLabel(CartesianChart, SeriesBounds);
					}
				}
			}
		}

		bool IsRectContainsPoints(TrackballPointInfo pointInfo)
		{
			return pointInfo.X >= 0 && pointInfo.Y >= 0 && pointInfo.X <= SeriesBounds.Width && pointInfo.Y <= SeriesBounds.Height;
		}

		SfTooltip? GetOrCreateTrackballView(SfCartesianChart chart, TrackballPointInfo? prevTrackballInfo)
		{
			var trackballView = prevTrackballInfo?.ContentTemplateView as SfTooltip;

			if (trackballView == null && ShowLabel)
			{
				trackballView = new SfTooltip
				{
					Background = _actualLabelStyle.Background
				};
				chart.BehaviorLayout.Add(trackballView);
				trackballView.Duration = double.NaN;
				trackballView.Helper.CanNosePointTarget = true;
				ContentList.Add(trackballView);
			}

			return trackballView;
		}

		static View? GetTheTrackballTemplate(DataTemplate trackballTemplate, object bindingContext)
		{
			if (trackballTemplate != null)
			{
				return new SfItemViewCell()
				{
					ItemTemplate = trackballTemplate,
					Item = bindingContext
				};
			}

			return null;
		}

		void RemoveTrackballTemplateInfo(SfCartesianChart cartesianChart)
		{
			if (ContentList.Count > 0)
			{
				foreach (var item in ContentList)
				{
					cartesianChart.BehaviorLayout.Remove(item);
				}
			}
		}

		static void SetTemplatePosition(SfCartesianChart cartesianChart, SfTooltip trackball)
		{
			if (!cartesianChart.IsTransposed)
			{
				trackball.Helper.PriorityPosition = TooltipPosition.Right;
				trackball.Helper.PriorityPositionList = [TooltipPosition.Left, TooltipPosition.Top, TooltipPosition.Bottom];
			}
			else
			{
				trackball.Helper.PriorityPosition = TooltipPosition.Top;
				trackball.Helper.PriorityPositionList = [TooltipPosition.Bottom, TooltipPosition.Right, TooltipPosition.Left];
			}
		}

		ChartLabelStyle DefaultLabelStyle()
		{
			return new ChartLabelStyle()
			{
				FontSize = TrackballFontSize,
				Background = TrackballBackground,
				Margin = 5f,
				LabelFormat = "#.##",
			};
		}

		static ChartLabelStyle DefaultAxisLabelStyle(ChartAxis axis)
		{
			return new ChartLabelStyle()
			{
				FontSize = axis.TrackballAxisFontSize,
				Background = axis.TrackballAxisBackground,
				Margin = new Thickness(7, 4),
			};
		}

		static object DefaultActivationMode(BindableObject bindable)
		{
#if WINDOWS || MACCATALYST
            return ChartTrackballActivationMode.TouchMove;
#elif ANDROID || IOS
			return ChartTrackballActivationMode.LongPress;
#else
            return ChartTrackballActivationMode.TouchMove;
#endif
		}

		void Invalidate()
		{
			CartesianChart?._trackballView.InvalidateDrawable();
		}

		static void DrawMarker(ICanvas canvas, Rect trackRect, ChartMarkerSettings chartMarkerSettings)
		{
			var settings = chartMarkerSettings;
			var fill = settings.Fill;
			var type = settings.Type;

			if (settings.HasBorder)
			{
				canvas.StrokeSize = (float)settings.StrokeWidth;
				canvas.StrokeColor = settings.Stroke.ToColor();
			}

			canvas.SetFillPaint(fill == default(Brush) ? SolidColorBrush.Transparent : fill, trackRect);
			canvas.DrawShape(trackRect, type, settings.HasBorder, false);
		}

		static string GetAxisLabel(ChartAxis axis, double xValue, string labelFormat)
		{
			string label = string.Empty;

			if (axis is CategoryAxis categoryAxis)
			{
				var currSeries = categoryAxis.GetActualSeries();
				if (currSeries != null)
				{
					label = categoryAxis.GetLabelContent(currSeries, (int)Math.Round(xValue), labelFormat);
				}
			}
			else if (axis is NumericalAxis)
			{
				label = ((int)Math.Round(xValue)).ToString(labelFormat);
			}
			else if (axis is LogarithmicAxis)
			{
				label = ChartAxis.GetActualLabelContent(xValue, labelFormat).ToString();
			}
			else if (axis is DateTimeAxis datetimeAxis)
			{
				string format;
				if (labelFormat != null)
				{
					format = labelFormat;
				}
				else
				{
					format = ChartAxis.GetSpecificFormattedLabel(datetimeAxis.ActualIntervalType);
				}

				label = ChartAxis.GetFormattedAxisLabel(format, xValue);
			}
			else
			{
				label = ChartAxis.GetActualLabelContent(xValue, labelFormat);
			}

			return label;
		}

		void DrawTrackballLabels(ICanvas canvas)
		{
			foreach (var info in PointInfos)
			{
				if (info.Y <= SeriesBounds.Bottom && info.X >= 0 && info.Y >= 0 && info.X <= SeriesBounds.Width)
				{
					if (ShowMarkers)
					{
						ChartTrackballBehavior.DrawMarker(canvas, info.TargetRect, info.MarkerSettings ?? ((IMarkerDependent)this).MarkerSettings);
					}
				}

				if (ShowLabel && !info.HaveTemplateView && DisplayMode != LabelDisplayMode.GroupAllPoints)
				{
					info.TooltipHelper.Draw(canvas);
				}
			}
		}

		void DrawTrackballLine(ICanvas canvas)
		{
			var lineStyle = LineStyle;
			if (lineStyle != null && CartesianChart != null)
			{
				if (lineStyle.StrokeDashArray != null)
				{
					canvas.StrokeDashPattern = lineStyle.StrokeDashArray.ToFloatArray();
				}

				canvas.StrokeColor = lineStyle.Stroke.ToColor();
				canvas.StrokeSize = (float)lineStyle.StrokeWidth;

				if (!CartesianChart.IsTransposed)
				{
					canvas.DrawLine((float)_linePoint1.X + SeriesBounds.Left, SeriesBounds.Top, (float)_linePoint2.X + SeriesBounds.Left, SeriesBounds.Bottom);
				}
				else
				{
					canvas.DrawLine(SeriesBounds.Left, (float)_linePoint1.Y, SeriesBounds.Right, (float)_linePoint2.Y);
				}
			}
		}

		static void OnLabelStyleChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is ChartTrackballBehavior behavior)
			{
				if (oldValue is ChartLabelStyle labelStyle)
				{
					SetInheritedBindingContext(labelStyle, null);
				}

				if (newValue is ChartLabelStyle style)
				{
					behavior._actualLabelStyle = (ChartLabelStyle)newValue;
					SetInheritedBindingContext(style, behavior.BindingContext);
					behavior.InitializeTrackballDynamicResource();
				}

				ChartBase.SetParent((Element?)oldValue, (Element?)newValue, behavior.Parent);
			}
		}

		void InitializeTrackballDynamicResource()
		{
			_actualLabelStyle.SetDynamicResource(ChartLabelStyle.StrokeProperty, "SfCartesianChartTrackballLabelStroke");
			_actualLabelStyle.SetDynamicResource(ChartLabelStyle.TextColorProperty, "SfCartesianChartTrackballLabelTextColor");
			_actualLabelStyle.SetDynamicResource(ChartLabelStyle.CornerRadiusProperty, "SfCartesianChartTrackballLabelCornerRadius");
		}

		static void OnLineStyleChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is ChartTrackballBehavior behavior)
			{
				if (oldValue is ChartLineStyle style)
				{
					SetInheritedBindingContext(style, null);
				}

				if (newValue is ChartLineStyle lineStyle)
				{
					SetInheritedBindingContext(lineStyle, behavior.BindingContext);
				}

				ChartBase.SetParent((Element?)oldValue, (Element?)newValue, behavior.Parent);
			}
		}

		void Drawable()
		{
		}

		#endregion

		#endregion
	}

	/// <summary>
	/// Stores the information to generate trackball axis label
	/// </summary>
	internal class TrackballAxisInfo
	{
		#region Properties

		public ChartAxis Axis { get; set; }

		public float X { get; set; }

		public float Y { get; set; }

		public TooltipHelper Helper { get; set; }

		internal string Label { get; set; }

		internal SfTooltip? AxisTemplateView { get; set; }

		internal bool HasTrackballAxisTemplate => Axis.TrackballLabelTemplate != null;

		#endregion

		#region Constructor

		public TrackballAxisInfo(ChartAxis axis, TooltipHelper helper, string label, float x, float y)
		{
			Axis = axis;
			X = x;
			Y = y;
			Label = label;
			Helper = helper;
			helper.Text = label;
		}

		#endregion
	}

	internal partial class SfItemViewCell : ContentView
	{
		#region Properties

		public DataTemplate ItemTemplate
		{
			get => (DataTemplate)GetValue(ItemTemplateProperty);
			set => SetValue(ItemTemplateProperty, value);
		}
		public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(SfItemViewCell), propertyChanged: ItemTemplateChanged);

		public object Item
		{
			get => (object)GetValue(ItemProperty);
			set => SetValue(ItemProperty, value);
		}
		public static readonly BindableProperty ItemProperty = BindableProperty.Create(nameof(Item), typeof(object), typeof(SfItemViewCell), null, propertyChanged: SourceChanged);

		public bool HideOnNullContent { get; set; }

		#endregion

		#region Methods

		static void SourceChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfItemViewCell control && control.ItemTemplate != null)
			{
				control.GenerateItem();
			}
		}

		static void ItemTemplateChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfItemViewCell control && control.ItemTemplate != null)
			{
				control.GenerateItem();
			}
		}

		void GenerateItem()
		{
			if (Item == null)
			{
				Content = null;
				return;
			}

			//Create the content
			try
			{
				if (Content != null && ItemTemplate is not DataTemplateSelector)
				{
					Content.BindingContext = Item;
				}
				else
				{
					Content = SfItemViewCell.CreateTemplateForItem(Item, ItemTemplate, false);
				}
			}
			catch
			{
				Content = null;
			}
			finally
			{
				if (HideOnNullContent)
				{
					IsVisible = Content != null;
				}
			}
		}

		static View? CreateTemplateForItem(object item, DataTemplate itemTemplate, bool createDefaultIfNoTemplate = true)
		{
			//Check to see if we have a template selector or just a template
			var templateToUse = itemTemplate is DataTemplateSelector templateSelector ? templateSelector.SelectTemplate(item, null) : itemTemplate;

			//If we still don't have a template, create a label
			if (templateToUse == null)
			{
				return createDefaultIfNoTemplate ? new Label() { Text = item.ToString() } : null;
			}

			//Create the content
			//If a view wasn't created, we can't use it, exit
			if (templateToUse.CreateContent() is not View view)
			{
				return new Label() { Text = item.ToString() };
			};

			//Set the binding
			view.BindingContext = item;

			return view;
		}

		#endregion
	}
}
