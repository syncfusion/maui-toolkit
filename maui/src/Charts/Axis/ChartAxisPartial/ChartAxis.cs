using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Syncfusion.Maui.Toolkit.Themes;
using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// The ChartAxis is the base class for all types of axes.
	/// </summary>
	/// <remarks>
	/// <para>The ChartAxis is used to locate a data point inside the chart area. Charts typically have two axes that are used to measure and categorize data.</para>
	/// <para>The Vertical(Y) axis always uses numerical scale.</para>
	/// <para>The Horizontal(X) axis supports the Category, Numeric and Date-time.</para>  
	/// </remarks>
	public partial class ChartAxis
	{
		#region Bindable Properties

		/// <summary>
		/// Identifies the <see cref="IsVisible"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="IsVisible"/> property determines whether the axis is visible in the chart.
		/// </remarks>
		public static readonly BindableProperty IsVisibleProperty = BindableProperty.Create(
			nameof(IsVisible),
			typeof(bool),
			typeof(ChartAxis),
			true,
			BindingMode.Default,
			null,
			OnIsVisibleChanged);

		/// <summary>
		/// Identifies the <see cref="AxisLineOffset"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="AxisLineOffset"/> property defines the offset of the axis line from its original position.
		/// </remarks>
		public static readonly BindableProperty AxisLineOffsetProperty = BindableProperty.Create(
			nameof(AxisLineOffset),
			typeof(double),
			typeof(ChartAxis),
			0d,
			BindingMode.Default,
			null,
			OnAxisLineOffsetChanged);

		/// <summary>
		/// Identifies the <see cref="LabelRotation"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="LabelRotation"/> property specifies the rotation angle for the axis labels.
		/// </remarks>    
		public static readonly BindableProperty LabelRotationProperty = BindableProperty.Create(
			nameof(LabelRotation),
			typeof(double),
			typeof(ChartAxis),
			0d,
			BindingMode.Default,
			null,
			OnLabelRotationAngleChanged);

		/// <summary>
		/// Identifies the <see cref="LabelStyle"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="LabelStyle"/> property defines the style of the axis labels, 
		/// including font size, color, and font family.
		/// </remarks>   
		public static readonly BindableProperty LabelStyleProperty = BindableProperty.Create(
			nameof(LabelStyle),
			typeof(ChartAxisLabelStyle),
			typeof(ChartAxis),
			null,
			BindingMode.Default,
			null,
			OnLabelStyleChanged);

		/// <summary>
		/// Identifies the <see cref="AxisLineStyle"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="AxisLineStyle"/> property sets the style of the axis line, such as its color, 
		/// width, and dash pattern.
		/// </remarks>    
		public static readonly BindableProperty AxisLineStyleProperty = BindableProperty.Create(
			nameof(AxisLineStyle),
			typeof(ChartLineStyle),
			typeof(ChartAxis),
			null,
			BindingMode.Default,
			null,
			OnAxisLineStyleChanged);

		/// <summary>
		/// Identifies the <see cref="CrossesAt"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="CrossesAt"/> property specifies the value at which the axis crosses another axis.
		/// </remarks>
		public static readonly BindableProperty CrossesAtProperty = BindableProperty.Create(
			nameof(CrossesAt),
			typeof(object),
			typeof(ChartAxis),
			double.NaN,
			BindingMode.Default,
			null,
			OnCrossesChanged);

		/// <summary>
		/// Identifies the <see cref="RenderNextToCrossingValue"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="RenderNextToCrossingValue"/> property defines whether the axis is rendered next to its crossing value.
		/// </remarks>
		public static readonly BindableProperty RenderNextToCrossingValueProperty = BindableProperty.Create(
			nameof(RenderNextToCrossingValue),
			typeof(bool),
			typeof(ChartAxis),
			true,
			BindingMode.Default,
			null,
			OnRenderNextToCrossingValuePropertyChanged);

		/// <summary>
		/// Identifies the <see cref="CrossAxisName"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="CrossAxisName"/> property defines the name of the axis with which this axis crosses.
		/// </remarks>
		public static readonly BindableProperty CrossAxisNameProperty = BindableProperty.Create(
			nameof(CrossAxisName),
			typeof(string),
			typeof(ChartAxis),
			null,
			BindingMode.Default,
			null,
			OnCrossAxisNameChanged);

		/// <summary>
		/// Identifies the <see cref="Title"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="Title"/> property defines the title of the axis, which appears next to the axis line.
		/// </remarks> 
		public static readonly BindableProperty TitleProperty = BindableProperty.Create(
			nameof(Title),
			typeof(ChartAxisTitle),
			typeof(ChartAxis),
			null,
			BindingMode.Default,
			null,
			OnTitleChanged);

		/// <summary>
		/// Identifies the <see cref="IsInversed"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="IsInversed"/> property specifies whether the axis is rendered in reverse order.
		/// </remarks>
		public static readonly BindableProperty IsInversedProperty = BindableProperty.Create(
			nameof(IsInversed),
			typeof(bool),
			typeof(ChartAxis),
			false,
			BindingMode.Default,
			null,
			OnInversedChanged);

		/// <summary>
		/// Identifies the <see cref="EdgeLabelsDrawingMode"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="EdgeLabelsDrawingMode"/> property defines the rendering position of the edge axis labels.
		/// </remarks>
		public static readonly BindableProperty EdgeLabelsDrawingModeProperty = BindableProperty.Create(
			nameof(EdgeLabelsDrawingMode),
			typeof(EdgeLabelsDrawingMode),
			typeof(ChartAxis),
			EdgeLabelsDrawingMode.Center,
			BindingMode.Default,
			null,
			OnEdgeLabelsDrawingModeChanged);

		/// <summary>
		/// Identifies the <see cref="MajorGridLineStyle"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="MajorGridLineStyle"/> property specifies the style of the major grid lines on the chart.
		/// </remarks>
		public static readonly BindableProperty MajorGridLineStyleProperty = BindableProperty.Create(
			nameof(MajorGridLineStyle),
			typeof(ChartLineStyle),
			typeof(ChartAxis),
			null,
			BindingMode.Default,
			null,
			OnMajorGridLineStyleChanged);

		/// <summary>
		/// Identifies the <see cref="MajorTickStyle"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="MajorTickStyle"/> property defines the style of the major ticks on the axis.
		/// </remarks>
		public static readonly BindableProperty MajorTickStyleProperty = BindableProperty.Create(
			nameof(MajorTickStyle),
			typeof(ChartAxisTickStyle),
			typeof(ChartAxis),
			null,
			BindingMode.Default,
			null,
			OnMajorTickStyleChanged);

		/// <summary>
		/// Identifies the <see cref="ZoomPosition"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="ZoomPosition"/> property defines the zoom position for the actual range of the axis.
		/// </remarks>   
		public static readonly BindableProperty ZoomPositionProperty = BindableProperty.Create(
			nameof(ZoomPosition),
			typeof(double),
			typeof(ChartAxis),
			0d,
			BindingMode.TwoWay,
			null,
			OnZoomPositionChanged);

		/// <summary>
		/// Identifies the <see cref="ZoomFactor"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="ZoomFactor"/> property defines the percentage of the visible range from the total range of axis values.
		/// </remarks> 
		public static readonly BindableProperty ZoomFactorProperty = BindableProperty.Create(
			nameof(ZoomFactor),
			typeof(double),
			typeof(ChartAxis),
			1d,
			BindingMode.TwoWay,
			null,
			OnZoomFactorChanged);

		/// <summary>
		/// Identifies the <see cref="ShowMajorGridLines"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="ShowMajorGridLines"/> property specifies whether the major grid lines are shown on the chart.
		/// </remarks>
		public static readonly BindableProperty ShowMajorGridLinesProperty = BindableProperty.Create(
			nameof(ShowMajorGridLines),
			typeof(bool),
			typeof(ChartAxis),
			true,
			BindingMode.Default,
			null,
			OnShowMajorGridLinesChanged);

		/// <summary>
		/// Identifies the <see cref="Name"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="Name"/> property defines the name of the axis, used for identification purposes.
		/// </remarks>
		public static readonly BindableProperty NameProperty = BindableProperty.Create(
			nameof(Name),
			typeof(string),
			typeof(ChartAxis),
			string.Empty,
			BindingMode.Default,
			null,
			OnNameChanged);

		/// <summary>
		/// Identifies the <see cref="PlotOffsetStart"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="PlotOffsetStart"/> property specifies the starting offset for the plot area relative to the axis.
		/// </remarks>
		public static readonly BindableProperty PlotOffsetStartProperty = BindableProperty.Create(
			nameof(PlotOffsetStart),
			typeof(double),
			typeof(ChartAxis),
			0d,
			BindingMode.Default,
			null,
			OnPlotOffsetStartChanged);

		/// <summary>
		/// Identifies the <see cref="PlotOffsetEnd"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="PlotOffsetEnd"/> property specifies the ending offset for the plot area relative to the axis.
		/// </remarks>
		public static readonly BindableProperty PlotOffsetEndProperty = BindableProperty.Create(
			nameof(PlotOffsetEnd),
			typeof(double),
			typeof(ChartAxis),
			0d,
			BindingMode.Default,
			null,
			OnPlotOffsetEndChanged);

		/// <summary>
		/// Identifies the <see cref="EnableAutoIntervalOnZooming"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="EnableAutoIntervalOnZooming"/> property controls whether the interval is automatically adjusted when zooming.
		/// </remarks>
		public static readonly BindableProperty EnableAutoIntervalOnZoomingProperty = BindableProperty.Create(
			nameof(EnableAutoIntervalOnZooming),
			typeof(bool),
			typeof(ChartAxis),
			true,
			BindingMode.Default,
			null,
			OnEnableAutoIntervalOnZoomingPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="ShowTrackballLabel"/> bindable property.
		/// </summary>
		/// <remarks>
		/// Determines whether the trackball label is displayed or hidden.
		/// </remarks>
		public static readonly BindableProperty ShowTrackballLabelProperty =
			BindableProperty.Create(
				nameof(ShowTrackballLabel),
				typeof(bool),
				typeof(ChartAxis),
				false,
				BindingMode.Default,
				null);

		/// <summary>
		/// Identifies the <see cref="TrackballLabelStyle"/> bindable property.
		/// </summary>
		/// <remarks>
		/// Defines the style for the trackball label.
		/// </remarks>
		public static readonly BindableProperty TrackballLabelStyleProperty = BindableProperty.Create(
				nameof(TrackballLabelStyle),
				typeof(ChartLabelStyle),
				typeof(ChartAxis),
				null,
				BindingMode.Default,
				null,
				OnTrackballLabelStyleChanged);

		/// <summary>
		/// Identifies the <see cref="TrackballLabelTemplate"/> bindable property.
		/// </summary>
		/// <remarks>
		/// Defines the template for the trackball label content.
		/// </remarks>
		public static readonly BindableProperty TrackballLabelTemplateProperty = BindableProperty.Create(
			nameof(TrackballLabelTemplate),
			typeof(DataTemplate),
			typeof(ChartAxis),
			null,
			BindingMode.Default,
			null,
			null);

		/// <summary>
		/// Identifies the <see cref="AutoScrollingMode"/> bindable property.
		/// </summary>
		/// <remarks>
		/// Determine whether the axis should be scrolled from start position or end position.
		/// </remarks>
		public static readonly BindableProperty AutoScrollingModeProperty = BindableProperty.Create(
			nameof(AutoScrollingMode),
			typeof(ChartAutoScrollingMode),
			typeof(ChartAxis),
			ChartAutoScrollingMode.End,
			BindingMode.Default,
			null,
			OnAutoScrollingModeChanged);

		/// <summary>
		/// Identifies the <see cref="AutoScrollingDelta"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="AutoScrollingDelta"/> bindable property specifies the range of data that should always be visible in the chart.
		/// </remarks>
		public static readonly BindableProperty AutoScrollingDeltaProperty = BindableProperty.Create(
			nameof(AutoScrollingDelta),
			typeof(double),
			typeof(ChartAxis),
			double.NaN,
			BindingMode.Default,
			null,
			OnAutoScrollingDeltaPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="LabelExtent"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="LabelExtent"/> bindable property determines the distance between the axis label and axis title.
		/// </remarks>
		public static readonly BindableProperty LabelExtentProperty = BindableProperty.Create(
			nameof(LabelExtent),
			typeof(double),
			typeof(ChartAxis),
			0d,
			BindingMode.Default,
			null,
			OnLabelExtentChanged);

		/// <summary>
		/// Identifies the <see cref="LabelsIntersectAction"/> bindable property.
		/// </summary>
		/// <remarks>
		/// Defines the behavior for intersecting axis labels.
		/// </remarks>
		public static readonly BindableProperty LabelsIntersectActionProperty = BindableProperty.Create(
			nameof(LabelsIntersectAction),
			typeof(AxisLabelsIntersectAction),
			typeof(ChartAxis),
			AxisLabelsIntersectAction.Hide,
			BindingMode.Default,
			null,
			OnLabelsIntersectActionChanged);

		#region Internal Properties

		//Todo: Need to provide support for next release.

		/// <summary>
		/// Identifies the <see cref="RangeStyles"/> bindable property.
		/// </summary>
		/// <remarks>
		/// Defines the range styles for customizing the axis.
		/// </remarks>
		internal static readonly BindableProperty RangeStylesProperty = BindableProperty.Create(
			nameof(RangeStyles),
			typeof(ChartAxisRangeStyleCollection),
			typeof(ChartAxis),
			null,
			BindingMode.Default,
			null,
			OnRangeStylesPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="TickPosition"/> bindable property.
		/// </summary>
		/// <remarks>
		/// Defines the position of the axis tick lines.
		/// </remarks>
		public static readonly BindableProperty TickPositionProperty = BindableProperty.Create(
			nameof(TickPosition),
			typeof(AxisElementPosition),
			typeof(ChartAxis),
			AxisElementPosition.Outside,
			BindingMode.Default,
			null,
			OnTickPositionChanged);

		/// <summary>
		/// Identifies the <see cref="LabelsPosition"/> bindable property.
		/// </summary>
		/// <remarks>
		/// Defines the position of the axis labels.
		/// </remarks>       
		public static readonly BindableProperty LabelsPositionProperty = BindableProperty.Create(
			nameof(LabelsPosition),
			typeof(AxisElementPosition),
			typeof(ChartAxis),
			AxisElementPosition.Outside,
			BindingMode.Default,
			null,
			OnLabelsPositionChanged);

		/// <summary>
		/// Identifies the <see cref="MaximumLabels"/> bindable property.
		/// </summary>
		/// <remarks>
		/// Determines the maximum number of labels displayed per 100 pixels.
		/// </remarks>
		internal static readonly BindableProperty MaximumLabelsProperty = BindableProperty.Create(
			nameof(MaximumLabels),
			typeof(int),
			typeof(ChartAxis),
			3,
			BindingMode.Default,
			null,
			OnMaximumLabelsChanged);

		#endregion

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets a value indicating whether to show/hide the chart axis.
		/// </summary>
		/// <value>It accepts bool values and its default value is <c>True</c>.</value>
		/// <example>
		/// # [MainPage.xaml](#tab/tabid-5)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		///  
		///     <chart:SfCartesianChart.XAxes>
		///         <chart:CategoryAxis IsVisible = "False" />
		///     </chart:SfCartesianChart.XAxes>
		/// 
		/// </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [MainPage.xaml.cs](#tab/tabid-6)
		/// <code><![CDATA[
		/// SfCartesianChart chart = new SfCartesianChart();
		/// 
		/// CategoryAxis xAxis = new CategoryAxis()
		/// {
		///    IsVisible = false
		/// };
		/// chart.XAxes.Add(xAxis);
		/// 
		/// ]]>
		/// </code>
		/// *** 
		/// </example>
		public bool IsVisible
		{
			get { return (bool)GetValue(IsVisibleProperty); }
			set { SetValue(IsVisibleProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value to provide padding to the axis at the start position.
		/// </summary>
		/// <value>It accepts <c>double</c> values and its default value is 0.</value>
		/// <remarks>
		/// <see cref="PlotOffsetStart"/> applies padding at the start of a plot area where the axis and its elements are rendered in a chart with padding at the start.
		/// </remarks>
		/// <example>
		/// # [MainPage.xaml](#tab/tabid-1)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		///  
		///     <chart:SfCartesianChart.XAxes>
		///         <chart:CategoryAxis PlotOffsetStart = "30" />
		///     </chart:SfCartesianChart.XAxes>
		///
		///     <chart:SfCartesianChart.YAxes>
		///         <chart:NumericalAxis PlotOffsetStart = "30" />
		///     </chart:SfCartesianChart.YAxes>
		/// 
		/// </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [MainPage.xaml.cs](#tab/tabid-2)
		/// <code><![CDATA[
		/// SfCartesianChart chart = new SfCartesianChart();
		/// 
		/// CategoryAxis xAxis = new CategoryAxis()
		/// {
		///    PlotOffsetStart = 30
		/// };
		/// 
		/// NumericalAxis yAxis = new NumericalAxis()
		/// {
		///    PlotOffsetStart = 30
		/// };
		///
		/// chart.XAxes.Add(xAxis);
		/// chart.YAxes.Add(yAxis);
		/// 
		/// ]]>
		/// </code>
		/// *** 
		/// </example>
		/// <see cref="PlotOffsetEnd"/>
		public double PlotOffsetStart
		{
			get { return (double)GetValue(PlotOffsetStartProperty); }
			set { SetValue(PlotOffsetStartProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value to provide padding to the axis at end position.
		/// </summary>
		/// <value>It accepts <c>double</c> values and its default value is 0.</value>
		/// <remarks>
		/// <see cref="PlotOffsetEnd"/> applies padding at end of the plot area where the axis and its elements are rendered in the chart with padding at the end.
		/// </remarks>
		/// <example>
		/// # [MainPage.xaml](#tab/tabid-3)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		///  
		///     <chart:SfCartesianChart.XAxes>
		///         <chart:CategoryAxis PlotOffsetEnd = "30" />
		///     </chart:SfCartesianChart.XAxes>
		///
		///     <chart:SfCartesianChart.YAxes>
		///         <chart:NumericalAxis PlotOffsetEnd = "30" />
		///     </chart:SfCartesianChart.YAxes>
		/// 
		/// </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [MainPage.xaml.cs](#tab/tabid-4)
		/// <code><![CDATA[
		/// SfCartesianChart chart = new SfCartesianChart();
		/// 
		/// CategoryAxis xAxis = new CategoryAxis()
		/// {
		///    PlotOffsetEnd = 30
		/// };
		/// 
		/// NumericalAxis yAxis = new NumericalAxis()
		/// {
		///    PlotOffsetEnd = 30
		/// };
		///
		/// chart.XAxes.Add(xAxis);
		/// chart.YAxes.Add(yAxis);
		/// 
		/// ]]>
		/// </code>
		/// *** 
		/// </example>
		/// <see cref="PlotOffsetStart"/>
		public double PlotOffsetEnd
		{
			get { return (double)GetValue(PlotOffsetEndProperty); }
			set { SetValue(PlotOffsetEndProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value to provide padding to the axis line.
		/// </summary>
		/// <value>It accepts <c>double</c> values and its default value is 0.</value>
		/// <example>
		/// # [MainPage.xaml](#tab/tabid-5)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		///  
		///     <chart:SfCartesianChart.XAxes>
		///         <chart:CategoryAxis AxisLineOffset = "30" />
		///     </chart:SfCartesianChart.XAxes>
		/// 
		/// </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [MainPage.xaml.cs](#tab/tabid-6)
		/// <code><![CDATA[
		/// SfCartesianChart chart = new SfCartesianChart();
		/// 
		/// CategoryAxis xAxis = new CategoryAxis()
		/// {
		///    AxisLineOffset = 30
		/// };
		/// chart.XAxes.Add(xAxis);
		/// 
		/// ]]>
		/// </code>
		/// *** 
		/// </example>
		public double AxisLineOffset
		{
			get { return (double)GetValue(AxisLineOffsetProperty); }
			set { SetValue(AxisLineOffsetProperty, value); }
		}

		/// <summary>
		/// Gets or sets the value for the rotation angle of the axis labels.
		/// </summary>
		/// <value>It accepts the <c>double</c> values and the default value is 0.</value>
		/// <remarks>
		/// Label rotation angle can be set from -90 to 90 degrees.
		/// <para> <b>Note:</b> This is only applicable for the secondary axis of the <see cref="SfPolarChart"/>.</para>
		/// </remarks>
		/// <example>
		/// # [MainPage.xaml](#tab/tabid-7)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		///  
		///     <chart:SfCartesianChart.XAxes>
		///         <chart:CategoryAxis LabelRotation = "90" />
		///     </chart:SfCartesianChart.XAxes>
		/// 
		/// </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [MainPage.xaml.cs](#tab/tabid-8)
		/// <code><![CDATA[
		/// SfCartesianChart chart = new SfCartesianChart();
		/// 
		/// CategoryAxis xAxis = new CategoryAxis()
		/// {
		///    LabelRotation = 90,
		/// };
		/// chart.XAxes.Add(xAxis);
		/// 
		/// ]]>
		/// </code>
		/// *** 
		/// </example>
		public double LabelRotation
		{
			get { return (double)GetValue(LabelRotationProperty); }
			set { SetValue(LabelRotationProperty, value); }
		}

		/// <summary>
		/// Gets or sets the value to customize the appearance of chart axis labels. 
		/// </summary>
		/// <value>It accepts the <see cref="Charts.ChartAxisLabelStyle"/> value.</value>
		/// <remarks>
		/// To customize the axis labels appearance, you need to create an instance of the <see cref="ChartAxisLabelStyle"/> class and set to the <see cref="LabelStyle"/> property.
		/// <para>Null values are invalid.</para>
		/// </remarks>
		/// <example>
		/// # [MainPage.xaml](#tab/tabid-9)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		///  
		///     <chart:SfCartesianChart.XAxes>
		///         <chart:CategoryAxis>
		///            <chart:CategoryAxis.LabelStyle>
		///                <chart:ChartAxisLabelStyle TextColor = "Red" FontSize="14"/>
		///            </chart:CategoryAxis.LabelStyle>
		///        </chart:CategoryAxis>
		///     </chart:SfCartesianChart.XAxes>
		/// 
		/// </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [MainPage.xaml.cs](#tab/tabid-10)
		/// <code><![CDATA[
		/// SfCartesianChart chart = new SfCartesianChart();
		/// 
		/// CategoryAxis xAxis = new CategoryAxis();
		/// xAxis.LabelStyle = new ChartAxisLabelStyle()
		/// {
		///     TextColor = Colors.Red,
		///     FontSize = 14,
		/// };
		/// chart.XAxes.Add(xAxis);
		///
		/// ]]>
		/// </code>
		/// *** 
		/// </example> 
		public ChartAxisLabelStyle LabelStyle
		{
			get { return (ChartAxisLabelStyle)GetValue(LabelStyleProperty); }
			set { SetValue(LabelStyleProperty, value); }
		}

		/// <summary>
		/// Gets or sets the value to customize the appearance of the chart axis line.
		/// </summary>
		/// <value>This property accepts the <see cref="ChartLineStyle"/> value.</value>
		/// <remarks>
		/// To customize the axis line appearance, you need to create an instance of the <see cref="ChartLineStyle"/> class and set to the <see cref="AxisLineStyle"/> property.
		/// <para>Null values are invalid.</para>
		/// <para> <b>Note:</b> This is only applicable for the secondary axis of the <see cref="SfPolarChart"/>.</para>
		/// </remarks>
		/// <example>
		/// # [MainWindow.xaml](#tab/tabid-11)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///           <chart:SfCartesianChart.XAxes>
		///               <chart:NumericalAxis>
		///                   <chart:NumericalAxis.AxisLineStyle>
		///                       <chart:ChartLineStyle StrokeWidth="2" Stroke="Red"/>
		///                   </chart:NumericalAxis.AxisLineStyle>
		///               </chart:NumericalAxis>
		///           </chart:SfCartesianChart.XAxes>
		///
		///           <chart:SfCartesianChart.YAxes>
		///               <chart:NumericalAxis/>
		///           </chart:SfCartesianChart.YAxes>
		///
		///     </chart:SfCartesianChart>
		/// ]]></code>
		/// # [MainWindow.cs](#tab/tabid-12)
		/// <code><![CDATA[
		///     SfCartesianChart chart = new SfCartesianChart();
		///     
		///     NumericalAxis xAxis = new NumericalAxis();
		///     ChartLineStyle axisLineStyle = new ChartLineStyle();
		///     axisLineStyle.Stroke = SolidColorBrush.Red;
		///     axisLineStyle.StrokeWidth = 2;
		///     xAxis.AxisLineStyle = axisLineStyle;
		///     chart.XAxes.Add(xAxis);
		///     
		///     NumericalAxis yAxis = new NumericalAxis();
		///     chart.YAxes.Add(yAxis);
		/// ]]></code>
		/// ***
		/// </example>
		public ChartLineStyle AxisLineStyle
		{
			get { return (ChartLineStyle)GetValue(AxisLineStyleProperty); }
			set { SetValue(AxisLineStyleProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value that can be used to position an axis anywhere in the chart area. 
		/// </summary>
		/// <value>This property takes the <c>object</c> as its value and the default value is double.NaN.</value>
		/// <remarks>
		/// <para> <b>Note:</b> This is only applicable for <see cref="SfCartesianChart"/>.</para>
		/// </remarks>
		/// <example>
		/// # [MainWindow.xaml](#tab/tabid-13)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///           <chart:SfCartesianChart.XAxes>
		///               <chart:NumericalAxis CrossesAt="{Static x:Double.MaxValue}"/>
		///           </chart:SfCartesianChart.XAxes>
		///
		///           <chart:SfCartesianChart.YAxes>
		///               <chart:NumericalAxis/>
		///           </chart:SfCartesianChart.YAxes>
		///
		///     </chart:SfCartesianChart>
		/// ]]></code>
		/// # [MainWindow.cs](#tab/tabid-14)
		/// <code><![CDATA[
		///     SfCartesianChart chart = new SfCartesianChart();
		///     
		///     NumericalAxis xAxis = new NumericalAxis();
		///     xAxis.CrossesAt = double.MaxValue;
		///     chart.XAxes.Add(xAxis);
		///     
		///     NumericalAxis yAxis = new NumericalAxis();
		///     chart.YAxes.Add(yAxis);
		/// ]]></code>
		/// ***
		/// </example>
		public object CrossesAt
		{
			get { return (object)GetValue(CrossesAtProperty); }
			set { SetValue(CrossesAtProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value that determines whether the crossing axis should be placed at the crossing position or not.
		/// </summary>
		/// <value>It accepts bool values and the default value is <c>True</c>.</value>
		/// <remarks>
		/// <para> <b>Note:</b> This is only applicable for <see cref="SfCartesianChart"/>.</para>
		/// </remarks>
		/// <example>
		/// # [MainWindow.xaml](#tab/tabid-13)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///           <chart:SfCartesianChart.XAxes>
		///               <chart:NumericalAxis RenderNextToCrossingValue="False"/>
		///           </chart:SfCartesianChart.XAxes>
		///
		///           <chart:SfCartesianChart.YAxes>
		///               <chart:NumericalAxis/>
		///           </chart:SfCartesianChart.YAxes>
		///
		///     </chart:SfCartesianChart>
		/// ]]></code>
		/// # [MainWindow.cs](#tab/tabid-14)
		/// <code><![CDATA[
		///     SfCartesianChart chart = new SfCartesianChart();
		///     
		///     NumericalAxis xAxis = new NumericalAxis();
		///     xAxis.RenderNextToCrossingValue = false;
		///     chart.XAxes.Add(xAxis);
		///     
		///     NumericalAxis yAxis = new NumericalAxis();
		///     chart.YAxes.Add(yAxis);
		/// ]]></code>
		/// ***
		/// </example>
		public bool RenderNextToCrossingValue
		{
			get { return (bool)GetValue(RenderNextToCrossingValueProperty); }
			set { SetValue(RenderNextToCrossingValueProperty, value); }
		}

		/// <summary>
		/// Gets or sets the value for the CrossAxisName of chart axis.
		///</summary>
		///<value>It accepts string value and the default value is null.</value>
		///<remarks>
		///<para> <b>Note:</b> This is only applicable for <see cref="SfCartesianChart"/>.</para>
		///</remarks>
		/// <example>
		/// # [MainWindow.xaml](#tab/tabid-15)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///           <chart:SfCartesianChart.XAxes>
		///               <chart:NumericalAxis CrossesAt="5" CrossingAxisName="yAxis"/>
		///           </chart:SfCartesianChart.XAxes>
		///
		///           <chart:SfCartesianChart.YAxes>
		///               <chart:NumericalAxis Name="yAxis"/>
		///           </chart:SfCartesianChart.YAxes>
		///
		///     </chart:SfCartesianChart>
		/// ]]></code>
		/// # [MainWindow.cs](#tab/tabid-16)
		/// <code><![CDATA[
		///     SfCartesianChart chart = new SfCartesianChart();
		///     
		///     NumericalAxis yAxis = new NumericalAxis();
		///     NumericalAxis.Name = "yAxis";
		///     chart.YAxes.Add(yAxis);;
		///     
		///     NumericalAxis xAxis = new NumericalAxis();
		///     xAxis.CrossesAt = 5;
		///     xAxis.CrossingAxisName = "yAxis";
		///     chart.XAxes.Add(xAxis);
		///     
		/// ]]></code>
		/// ***
		/// </example>
		public string CrossAxisName
		{
			get { return (string)GetValue(CrossAxisNameProperty); }
			set { SetValue(CrossAxisNameProperty, value); }
		}

		/// <summary>
		/// Gets or sets the title for the chart axis.
		/// </summary>
		/// <value>It accepts <see cref="ChartAxisTitle"/> value.</value>
		/// <remarks>The <see cref="ChartAxisTitle"/> provides options to customize the text and font of axis title.
		/// <para> <b>Note:</b> This is only applicable for the secondary axis of the <see cref="SfPolarChart"/>.</para>
		/// </remarks>
		/// <example>
		/// # [MainPage.xaml](#tab/tabid-17)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		/// 
		///     <chart:SfCartesianChart.XAxes>
		///         <chart:CategoryAxis>
		///            <chart:CategoryAxis.Title>
		///                <chart:ChartAxisTitle Text="Category"/>
		///            </chart:CategoryAxis.Title>
		///        </chart:CategoryAxis>
		///     </chart:SfCartesianChart.XAxes>
		/// 
		/// </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [MainPage.xaml.cs](#tab/tabid-18)
		/// <code><![CDATA[
		/// SfCartesianChart chart = new SfCartesianChart();
		/// 
		/// CategoryAxis xAxis = new CategoryAxis();
		/// xAxis.Title = new ChartAxisTitle()
		/// {
		///     Text = "Category"
		/// };
		/// chart.XAxes.Add(xAxis);
		///
		/// ]]>
		/// </code>
		/// *** 
		/// </example> 
		public ChartAxisTitle Title
		{
			get { return (ChartAxisTitle)GetValue(TitleProperty); }
			set { SetValue(TitleProperty, value); }
		}

		/// <summary>
		/// Gets or sets the value that indicates whether the axis' visible range is inversed.
		/// </summary>
		/// <value>It accepts the bool values and its default value is <c>False</c>.</value>
		/// <remarks>When the axis is inversed, it will render points from right to left for the horizontal axis, and top to bottom for the vertical axis.</remarks>
		/// <example>
		/// # [MainPage.xaml](#tab/tabid-19)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		///  
		///     <chart:SfCartesianChart.XAxes>
		///         <chart:CategoryAxis IsInversed ="True" />
		///     </chart:SfCartesianChart.XAxes>
		/// 
		/// </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [MainPage.xaml.cs](#tab/tabid-20)
		/// <code><![CDATA[
		/// SfCartesianChart chart = new SfCartesianChart();
		/// 
		/// CategoryAxis xAxis = new CategoryAxis()
		/// {
		///    IsInversed = true,
		/// };
		/// chart.XAxes.Add(xAxis);
		/// 
		/// ]]>
		/// </code>
		/// *** 
		/// </example>
		public bool IsInversed
		{
			get { return (bool)GetValue(IsInversedProperty); }
			set { SetValue(IsInversedProperty, value); }
		}

		/// <summary>
		///  Gets or sets a value to customize the rendering position of the edge labels. 
		/// </summary>
		/// <value>It accepts the <see cref="Charts.EdgeLabelsDrawingMode"/> value and its default value is <see cref="EdgeLabelsDrawingMode.Center"/>.</value>
		/// <remarks>
		/// <para> <b>Note:</b> This is only applicable for the secondary axis of the <see cref="SfPolarChart"/>.</para>
		/// </remarks>
		/// <example>
		/// # [MainPage.xaml](#tab/tabid-21)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		///  
		///     <chart:SfCartesianChart.XAxes>
		///         <chart:CategoryAxis EdgeLabelsDrawingMode="Fit" />
		///     </chart:SfCartesianChart.XAxes>
		/// 
		/// </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [MainPage.xaml.cs](#tab/tabid-22)
		/// <code><![CDATA[
		/// SfCartesianChart chart = new SfCartesianChart();
		/// 
		/// CategoryAxis xAxis = new CategoryAxis()
		/// {
		///    EdgeLabelsDrawingMode= EdgeLabelsDrawingMode.Fit,
		/// };
		/// chart.XAxes.Add(xAxis);
		/// 
		/// ]]>
		/// </code>
		/// *** 
		/// </example>
		public EdgeLabelsDrawingMode EdgeLabelsDrawingMode
		{
			get { return (EdgeLabelsDrawingMode)GetValue(EdgeLabelsDrawingModeProperty); }
			set { SetValue(EdgeLabelsDrawingModeProperty, value); }
		}

		/// <summary>
		/// Gets or sets the <see cref="ChartLineStyle"/> to customize the appearance of the major grid lines.
		/// </summary>
		/// <remarks>
		/// To customize the major grid line appearance, you need to create an instance of the <see cref="ChartLineStyle"/> class and set to the <see cref="MajorGridLineStyle"/> property.
		/// <para>Null values are invalid.</para>
		/// </remarks>
		/// <value>It accepts the <see cref="ChartLineStyle"/>.</value>
		/// <example>
		/// # [MainWindow.xaml](#tab/tabid-23)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///      
		///          <chart:SfCartesianChart.Resources>
		///              <DoubleCollection x:Key="dashArray">
		///                  <x:Double>3</x:Double>
		///                  <x:Double>3</x:Double>
		///              </DoubleCollection>
		///          </chart:SfCartesianChart.Resources>
		///
		///           <chart:SfCartesianChart.XAxes>
		///               <chart:NumericalAxis>
		///                   <chart:NumericalAxis.MajorGridLineStyle>
		///                       <chart:ChartLineStyle StrokeDashArray="{StaticResource dashArray}" Stroke="Black" StrokeWidth="2" />
		///                   </chart:NumericalAxis.MajorGridLineStyle>
		///               </chart:NumericalAxis>
		///           </chart:SfCartesianChart.XAxes>
		///
		///           <chart:SfCartesianChart.YAxes>
		///               <chart:NumericalAxis/>
		///           </chart:SfCartesianChart.YAxes>
		///
		///     </chart:SfCartesianChart>
		/// ]]></code>
		/// # [MainWindow.cs](#tab/tabid-24)
		/// <code><![CDATA[
		///     SfCartesianChart chart = new SfCartesianChart();
		///     
		///     DoubleCollection doubleCollection = new DoubleCollection();
		///     doubleCollection.Add(3);
		///     doubleCollection.Add(3);
		///     
		///     NumericalAxis xAxis = new NumericalAxis();
		///     ChartLineStyle axisLineStyle = new ChartLineStyle();
		///     axisLineStyle.Stroke = SolidColorBrush.Black;
		///     axisLineStyle.StrokeWidth = 2;
		///     axisLineStyle.StrokeDashArray = doubleCollection
		///     xAxis.MajorGridLineStyle = axisLineStyle;
		///     chart.XAxes.Add(xAxis);
		///     
		///     NumericalAxis yAxis = new NumericalAxis();
		///     chart.YAxes.Add(yAxis);
		/// ]]></code>
		/// ***
		/// </example>
		public ChartLineStyle MajorGridLineStyle
		{
			get { return (ChartLineStyle)GetValue(MajorGridLineStyleProperty); }
			set { SetValue(MajorGridLineStyleProperty, value); }
		}

		/// <summary>
		/// Gets or sets the <see cref="ChartAxisTickStyle"/> to customize the appearance of the major tick lines.
		/// </summary>
		/// <remarks>
		/// To customize the axis major tick line appearance, you need to create an instance of the <see cref="ChartAxisTickStyle"/> class and set to the <see cref="MajorTickStyle"/> property.
		/// <para>Null values are invalid.</para>
		/// </remarks>
		/// <value>It accepts the <see cref="ChartAxisTickStyle"/> value.</value>
		/// <example>
		/// # [MainWindow.xaml](#tab/tabid-25)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///      
		///           <chart:SfCartesianChart.XAxes>
		///               <chart:NumericalAxis>
		///                   <chart:NumericalAxis.MajorTickStyle>
		///                       <chart:ChartAxisTickStyle Stroke="Red" StrokeWidth="1"/>
		///                   </chart:NumericalAxis.MajorTickStyle>
		///               </chart:NumericalAxis>
		///           </chart:SfCartesianChart.XAxes>
		///
		///           <chart:SfCartesianChart.YAxes>
		///               <chart:NumericalAxis/>
		///           </chart:SfCartesianChart.YAxes>
		///
		///     </chart:SfCartesianChart>
		/// ]]></code>
		/// # [MainWindow.cs](#tab/tabid-26)
		/// <code><![CDATA[
		///     SfCartesianChart chart = new SfCartesianChart();
		///     
		///     NumericalAxis xAxis = new NumericalAxis();
		///     xAxis.MajorTickStyle.StrokeWidth = 1;
		///     xAxis.MajorTickStyle.Stroke = SolidColorBrush.Red;
		///     chart.XAxes.Add(xAxis);
		///     
		///     NumericalAxis yAxis = new NumericalAxis();
		///     chart.YAxes.Add(yAxis)
		/// ]]></code>
		/// ***
		/// </example>
		public ChartAxisTickStyle MajorTickStyle
		{
			get { return (ChartAxisTickStyle)GetValue(MajorTickStyleProperty); }
			set { SetValue(MajorTickStyleProperty, value); }
		}

		/// <summary>
		/// Gets or sets the value that defines the zoom position for the actual range of the axis.
		/// </summary>
		/// <value>It accepts the double values and its default value is 0.</value>
		/// <remarks> The value must be between 0 and 1.
		/// <para> <b>Note:</b> This is only applicable for <see cref="SfCartesianChart"/>.</para>
		/// </remarks>
		/// <example>
		/// # [MainPage.xaml](#tab/tabid-27)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		///
		///         <chart:SfCartesianChart.XAxes>
		///             <chart:CategoryAxis ZoomFactor="0.3" ZoomPosition="0.5" />
		///         </chart:SfCartesianChart.XAxes>
		///
		///         <chart:SfCartesianChart.ZoomPanBehavior>
		///              <chart:ChartZoomPanBehavior/>
		///         </chart:SfCartesianChart.ZoomPanBehavior>
		/// 
		/// </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [MainPage.xaml.cs](#tab/tabid-28)
		/// <code><![CDATA[
		/// SfCartesianChart chart = new SfCartesianChart();
		/// 
		/// CategoryAxis xAxis = new CategoryAxis(){  ZoomFactor = 0.3, ZoomPosition = 0.5  };
		/// chart.XAxes.Add(xAxis);
		/// 
		/// chart.ZoomPanBehavior = new ChartZoomPanBehavior();
		/// 
		/// ]]>
		/// </code>
		/// *** 
		/// </example>
		public double ZoomPosition
		{
			get { return (double)GetValue(ZoomPositionProperty); }
			set { SetValue(ZoomPositionProperty, value); }
		}

		/// <summary>
		/// Gets or sets the value that defines the percentage of the visible range from the total range of axis values.
		/// </summary>
		/// <value>It accepts the double values and its default value is 1.</value> 
		/// <remarks>
		/// The value must be between 0 and 1.
		/// <para> <b>Note:</b> This is only applicable for <see cref="SfCartesianChart"/>.</para>
		/// </remarks>
		/// <summary>
		/// Gets or sets the value that defines the position for the actual range of the axis.
		/// </summary>
		/// <remarks> The value must be between 0 and 1.</remarks>
		/// <value>It accepts the double values and its default value is 0.</value>
		/// <example>
		/// # [MainPage.xaml](#tab/tabid-29)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		///  
		///         <chart:SfCartesianChart.XAxes>
		///             <chart:CategoryAxis ZoomFactor="0.3" ZoomPosition="0.5" />
		///         </chart:SfCartesianChart.XAxes>
		///         
		///         <chart:SfCartesianChart.ZoomPanBehavior>
		///              <chart:ChartZoomPanBehavior/>
		///         </chart:SfCartesianChart.ZoomPanBehavior>
		/// 
		/// </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [MainPage.xaml.cs](#tab/tabid-30)
		/// <code><![CDATA[
		/// SfCartesianChart chart = new SfCartesianChart();
		/// 
		/// CategoryAxis xAxis = new CategoryAxis(){  ZoomFactor = 0.3, ZoomPosition = 0.5  };
		/// chart.XAxes.Add(xAxis);	
		/// 
		/// ChartZoomPanBehavior zoomPanBehavior = new ChartZoomPanBehavior();
		/// chart.ZoomPanBehavior = zoomPanBehavior;
		/// 
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public double ZoomFactor
		{
			get { return (double)GetValue(ZoomFactorProperty); }
			set { SetValue(ZoomFactorProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether the axis grid lines can be displayed or not.
		/// </summary>
		/// <value>It accepts the bool value and its default value is <c>True</c>.</value>
		/// <example>
		/// # [MainPage.xaml](#tab/tabid-31)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		///  
		///     <chart:SfCartesianChart.XAxes>
		///         <chart:CategoryAxis ShowMajorGridLines = "False" />
		///     </chart:SfCartesianChart.XAxes>
		/// 
		/// </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [MainPage.xaml.cs](#tab/tabid-32)
		/// <code><![CDATA[
		/// SfCartesianChart chart = new SfCartesianChart();
		/// 
		/// CategoryAxis xAxis = new CategoryAxis()
		/// {
		///    ShowMajorGridLines = false,
		/// };
		/// chart.XAxes.Add(xAxis);
		/// 
		/// ]]>
		/// </code>
		/// *** 
		/// </example>
		public bool ShowMajorGridLines
		{
			get { return (bool)GetValue(ShowMajorGridLinesProperty); }
			set { SetValue(ShowMajorGridLinesProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether to calculate the axis intervals on zooming. 
		/// </summary>
		/// <value>It accepts the bool values and its default value is <c>True</c>.</value>
		/// <remarks>
		/// <para> <b>Note:</b> This is only applicable for <see cref="SfCartesianChart"/>.</para>
		/// </remarks>
		/// <example>
		/// # [MainPage.xaml](#tab/tabid-33)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		///  
		///     <chart:SfCartesianChart.XAxes>
		///         <chart:CategoryAxis EnableAutoIntervalOnZooming = "False" />
		///     </chart:SfCartesianChart.XAxes>
		/// 
		/// </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [MainPage.xaml.cs](#tab/tabid-34)
		/// <code><![CDATA[
		/// SfCartesianChart chart = new SfCartesianChart();
		/// 
		/// CategoryAxis xAxis = new CategoryAxis()
		/// {
		///    EnableAutoIntervalOnZooming = false,
		/// };
		/// chart.XAxes.Add(xAxis);
		/// 
		/// ]]>
		/// </code>
		/// *** 
		/// </example>
		public bool EnableAutoIntervalOnZooming
		{
			get { return (bool)GetValue(EnableAutoIntervalOnZoomingProperty); }
			set { SetValue(EnableAutoIntervalOnZoomingProperty, value); }
		}

		/// <summary>
		/// This event occurs when the axis label is created.
		/// </summary>
		/// <remarks>The <see cref="ChartAxisLabelEventArgs"/> contains the information of AxisLabel.</remarks>
		/// <example>
		/// # [MainPage.xaml](#tab/tabid-35)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		///  
		///     <chart:SfCartesianChart.XAxes>
		///         <chart:CategoryAxis LabelCreated="OnLabelCreated" />
		///     </chart:SfCartesianChart.XAxes>
		/// 
		/// </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [MainPage.xaml.cs](#tab/tabid-36)
		/// <code><![CDATA[
		/// SfCartesianChart chart = new SfCartesianChart();
		/// 
		/// CategoryAxis xAxis = new CategoryAxis();
		/// xAxis.LabelCreated += OnLabelCreated;
		/// chart.XAxes.Add(xAxis);
		/// 
		/// void OnLabelCreated(object sender, ChartAxisLabelEventArgs e)
		/// {
		///      // You can customize the content of the axis label.
		/// }
		/// 
		/// ]]>
		/// </code>
		/// *** 
		/// </example>
		public event EventHandler<ChartAxisLabelEventArgs>? LabelCreated;

		/// <summary>
		/// This event occurs when the actual range is changed.
		/// </summary>
		/// <remarks>
		/// The <see cref="ActualRangeChangedEventArgs"/> contains information on the chart axis' minimum and maximum values.
		/// </remarks>
		/// <example>
		/// # [MainWindow.xaml](#tab/tabid-37)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///           <chart:SfCartesianChart.XAxes>
		///               <chart:NumericalAxis ActualRangeChanged="xAxis_ActualRangeChanged"/>
		///           </chart:SfCartesianChart.XAxes>
		///
		///           <chart:SfCartesianChart.YAxes>
		///               <chart:NumericalAxis/>
		///           </chart:SfCartesianChart.YAxes>
		///
		///     </chart:SfCartesianChart>
		/// ]]></code>
		/// # [MainWindow.cs](#tab/tabid-38)
		/// <code><![CDATA[
		///     SfCartesianChart chart = new SfCartesianChart();
		///     
		///     NumericalAxis xAxis = new NumericalAxis();
		///     NumericalAxis yAxis = new NumericalAxis();
		///     
		///     chart.XAxes.Add(xAxis);
		///     chart.YAxes.Add(yAxis);
		///     
		///     xAxis.ActualRangeChanged += xAxis_ActualRangeChanged;
		///     
		///     void xAxis_ActualRangeChanged(object sender, Syncfusion.Maui.Toolkit.Charts.ActualRangeChangedEventArgs e)
		///     {
		///         e.VisibleMinimum = 2;
		///         e.VisibleMaximum = 5;
		///         e.ActualMinimum = 0;
		///         e.ActualMaximum = 8;
		///     } 
		/// ]]></code>
		/// ***
		/// </example>
		public event EventHandler<ActualRangeChangedEventArgs>? ActualRangeChanged;

		/// <summary>
		/// Gets or sets the value that indicates whether to show the trackball axis label.
		/// </summary>
		/// <value>It accepts the bool values and its default value is <c>True</c>.</value>
		/// <remarks>
		/// <para> <b>Note:</b> This is only applicable for <see cref="SfCartesianChart"/>.</para>
		/// </remarks>
		/// <example>
		/// # [MainPage.xaml](#tab/tabid-33)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		///  
		///     <chart:SfCartesianChart.XAxes>
		///         <chart:CategoryAxis ShowTrackballLabel = "True" />
		///     </chart:SfCartesianChart.XAxes>
		/// 
		/// </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [MainPage.xaml.cs](#tab/tabid-34)
		/// <code><![CDATA[
		/// SfCartesianChart chart = new SfCartesianChart();
		/// 
		/// CategoryAxis xAxis = new CategoryAxis()
		/// {
		///    ShowTrackballLabel = true,
		/// };
		/// chart.XAxes.Add(xAxis);
		/// 
		/// ]]>
		/// </code>
		/// *** 
		/// </example>
		public bool ShowTrackballLabel
		{
			get { return (bool)GetValue(ShowTrackballLabelProperty); }
			set { SetValue(ShowTrackballLabelProperty, value); }
		}

		/// <summary>
		/// Gets or sets option for customize the trackball axis label.
		/// </summary>
		/// <remarks>
		/// To customize the trackball label appearance, you need to create an instance of the <see cref="ChartLabelStyle"/> class and set to the <see cref="TrackballLabelStyle"/> property.
		/// <para>Null values are invalid.</para>
		/// </remarks>
		/// <value>It accepts the bool values and its default value is <c>True</c>.</value>
		/// <example>
		/// # [MainPage.xaml](#tab/tabid-33)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		///  
		///     <chart:SfCartesianChart.XAxes>
		///         <chart:CategoryAxis ShowTrackballLabel="True">
		///            <chart:CategoryAxis.TrackballLabelStyle>
		///                <chart:ChartLabelStyle Background = "Black" TextColor="White"/>
		///           </chart:CategoryAxis.TrackballLabelStyle>
		///        </chart:CategoryAxis>
		///     </chart:SfCartesianChart.XAxes>
		/// 
		/// </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [MainPage.xaml.cs](#tab/tabid-34)
		/// <code><![CDATA[
		/// SfCartesianChart chart = new SfCartesianChart();
		/// var trackballLabelStyle = new ChartLabelStyle()
		/// { 
		///     Background = new SolidColorBrush(Colors.Black),
		///     TextColor = Colors.White,
		/// };
		/// CategoryAxis xAxis = new CategoryAxis()
		/// {
		///    ShowTrackballLabel = true,
		///    TrackballLabelStyle = trackballLabelStyle,
		/// };
		/// chart.XAxes.Add(xAxis);
		/// 
		/// ]]>
		/// </code>
		/// *** 
		/// 
		/// <para> <b>Note:</b> This is only applicable for <see cref="SfCartesianChart"/>.</para>
		/// </example>
		public ChartLabelStyle TrackballLabelStyle
		{
			get { return (ChartLabelStyle)GetValue(TrackballLabelStyleProperty); }
			set { SetValue(TrackballLabelStyleProperty, value); }
		}

		/// <summary>
		/// Gets or sets the delta value that specifies the range of data that should always be visible in the chart. It determines the amount of range the chart will scroll by when new data points are added, ensuring that a specified range of data is always visible.
		/// </summary>
		/// <value>It accepts <see cref="double"/> values, and its default value is <see cref="double.NaN"/></value>
		/// <remarks>
		/// <para> <b>Note:</b> This is only applicable for <see cref="SfCartesianChart"/>.</para>
		/// </remarks>
		/// <example>
		/// # [MainPage.xaml](#tab/tabid-7)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		///    <chart:SfCartesianChart.XAxes>
		///        <chart:CategoryAxis AutoScrollingDelta="3"/>
		///    </chart:SfCartesianChart.XAxes>
		/// </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [MainPage.xaml.cs](#tab/tabid-8)
		/// <code><![CDATA[
		/// SfCartesianChart chart = new SfCartesianChart();
		/// 
		/// CategoryAxis xAxis = new CategoryAxis();
		/// xAxis.AutoScrollingDelta = 3;
		/// 
		/// chart.XAxes.Add(xAxis);	
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public double AutoScrollingDelta
		{
			get { return (double)GetValue(AutoScrollingDeltaProperty); }
			set { SetValue(AutoScrollingDeltaProperty, value); }
		}

		/// <summary>
		/// Gets or sets the mode to determine whether the axis should be auto scrolled from the start or end positions.
		/// </summary>
		/// <value>It accepts <see cref="ChartAutoScrollingMode"/> values, and its default value is <see cref="ChartAutoScrollingMode.End"/>, means that the chart will always display the most recent data points, and the axis will be automatically scrolled to the right to show the newly added data.</value>
		/// <remarks>
		/// <para> <b>Note:</b> This is only applicable for <see cref="SfCartesianChart"/>.</para>
		/// </remarks>
		/// <example>
		/// # [MainPage.xaml](#tab/tabid-7)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		///    <chart:SfCartesianChart.XAxes>
		///        <chart:CategoryAxis AutoScrollingMode="End"/>
		///    </chart:SfCartesianChart.XAxes>
		/// </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [MainPage.xaml.cs](#tab/tabid-8)
		/// <code><![CDATA[
		/// SfCartesianChart chart = new SfCartesianChart();
		/// 
		/// CategoryAxis xAxis = new CategoryAxis();
		/// xAxis.AutoScrollingMode = ChartAutoScrollingMode.End;
		/// 
		/// chart.XAxes.Add(xAxis);	
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public ChartAutoScrollingMode AutoScrollingMode
		{
			get { return (ChartAutoScrollingMode)GetValue(AutoScrollingModeProperty); }
			set { SetValue(AutoScrollingModeProperty, value); }
		}

		/// <summary>
		/// Gets or sets the unique name of the axis, which will be used to identify the segment axis of the strip line. 
		/// </summary>
		/// <value>This property takes the string value and its default value is string.Empty.</value>
		public string Name
		{
			get { return (string)GetValue(NameProperty); }
			set { SetValue(NameProperty, value); }
		}

		/// <summary>
		/// Gets or sets the value that determines the distance between the axis label and axis title. 
		/// </summary>
		/// <value>This property take double value and its default value is 0.</value>
		/// <example>
		/// # [MainPage.xaml](#tab/tabid-1)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		///  
		///     <chart:SfCartesianChart.XAxes>
		///         <chart:CategoryAxis LabelExtent="30" />
		///     </chart:SfCartesianChart.XAxes>
		///
		///     <chart:SfCartesianChart.YAxes>
		///         <chart:NumericalAxis LabelExtent="30" />
		///     </chart:SfCartesianChart.YAxes>
		/// 
		/// </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [MainPage.xaml.cs](#tab/tabid-2)
		/// <code><![CDATA[
		/// SfCartesianChart chart = new SfCartesianChart();
		/// 
		/// CategoryAxis xAxis = new CategoryAxis()
		/// {
		///    LabelExtent= 30
		/// };
		/// 
		/// NumericalAxis yAxis = new NumericalAxis()
		/// {
		///    LabelExtent= 30
		/// };
		///
		/// chart.XAxes.Add(xAxis);
		/// chart.YAxes.Add(yAxis);
		/// 
		/// ]]>
		/// </code>
		/// *** 
		/// </example>
		public double LabelExtent
		{
			get { return (double)GetValue(LabelExtentProperty); }
			set { SetValue(LabelExtentProperty, value); }
		}

		/// <summary>
		/// Gets or sets the DataTemplate to customize the appearance of the axis Trackball labels. 
		/// </summary>
		/// <value>
		/// It accepts the <see cref="DataTemplate"/>value and its default value is null.
		/// </value>
		/// <example>
		/// # [MainWindow.xaml](#tab/tabid-49)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///     
		///           <chart:SfCartesianChart.TrackballBehavior>
		///                <chart:ChartTrackballBehavior  />
		///           </chart:SfCartesianChart.TrackballBehavior>
		///
		///           <chart:SfCartesianChart.Resources>
		///               <DataTemplate x:Key="AxisTrackballTemplate">
		///                  <HorizontalStackLayout>
		///                     <Image Source="image.png" 
		///                            WidthRequest="20" 
		///                            HeightRequest="20"/>
		///                     <Label Text="{Binding Label}" 
		///                            TextColor="Black"
		///                            FontAttributes="Bold"
		///                            FontSize="12"/>
		///                  </HorizontalStackLayout>
		///               </DataTemplate>
		///           </chart:SfCartesianChart.Resources>
		/// 
		///           <chart:SfCartesianChart.XAxes>
		///               <chart:NumericalAxis ShowTrackballLabel = "True"
		///                                    TrackballLabelTemplate="{StaticResource AxisTrackballTemplate}" />
		///           </chart:SfCartesianChart.XAxes>
		///
		///           <chart:SfCartesianChart.YAxes>
		///               <chart:NumericalAxis/>
		///           </chart:SfCartesianChart.YAxes>
		///
		///           <chart:SfCartesianChart.Series>
		///               <chart:LineSeries ItemsSource="{Binding Data}"
		///                                   XBindingPath="XValue"
		///                                   YBindingPath="YValue">
		///               </chart:LineSeries> 
		///           </chart:SfCartesianChart.Series>
		///           
		///     </chart:SfCartesianChart>
		/// ]]></code>
		/// # [MainPage.xaml.cs](#tab/tabid-50)
		/// <code><![CDATA[
		/// SfCartesianChart chart = new SfCartesianChart();
		/// 
		/// ChartTrackballBehavior trackballBehavior = new ChartTrackballBehavior();
		/// chart.TrackballBehavior = trackballBehavior;
		/// 
		///  NumericalAxis xAxis = new NumericalAxis();
		///  xAxis.ShowTrackballLabel = true;
		///  
		///  NumericalAxis yAxis = new NumericalAxis();
		/// 
		/// LineSeries series = new LineSeries();
		/// series.ItemsSource = new ViewModel().Data;
		/// series.XBindingPath = "XValue";
		/// series.YBindingPath = "YValue";
		///     
		/// DataTemplate axisLabelTemplate = new DataTemplate(()=>
		/// {
		///     HorizontalStackLayout layout = new HorizontalStackLayout();
		///     Image image = new Image()
		///     {
		///         Source = "image.png",
		///         WightRequest = 20,
		///         HeightRequest = 20
		///     };
		///     
		///     Label label = new Label()
		///     {
		///         TextColor = Colors.Black,
		///         FontAttributes = FontAttributes.Bold,
		///         FontSize = 12,
		///     }
		///     
		///     label.SetBinding(Label.TextProperty, new Binding("Label"));
		///     layout.Children.Add(image);
		///     layout.Children.Add(label);
		///     return layout;
		/// }    
		///  
		/// xAxis.TrackballLabelTemplate = axisLabelTemplate;
		/// 
		/// chart.XAxes.Add(xAxis);
		/// chart.YAxes.Add(yAxis);
		/// chart.Series.Add(series);
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public DataTemplate TrackballLabelTemplate
		{
			get { return (DataTemplate)GetValue(TrackballLabelTemplateProperty); }
			set { SetValue(TrackballLabelTemplateProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value that determines the mechanism for avoiding the overlapping of axis labels. The overlapping labels can be hidden, wrapped or placed on the next row.
		/// </summary>
		/// <value>It accepts <see cref="AxisLabelsIntersectAction"/> values and the default value is <see cref="AxisLabelsIntersectAction.Hide"/>.</value>
		/// <example>
		/// # [MainPage.xaml](#tab/tabid-1)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		///  
		///     <chart:SfCartesianChart.XAxes>
		///         <chart:CategoryAxis LabelsIntersectAction="Wrap"/>
		///     </chart:SfCartesianChart.XAxes>
		///
		///     <chart:SfCartesianChart.YAxes>
		///         <chart:NumericalAxis LabelsIntersectAction="Wrap"/>
		///     </chart:SfCartesianChart.YAxes>
		/// 
		/// </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [MainPage.xaml.cs](#tab/tabid-2)
		/// <code><![CDATA[
		/// SfCartesianChart chart = new SfCartesianChart();
		/// 
		/// CategoryAxis xAxis = new CategoryAxis()
		/// {
		///    LabelsIntersectAction= AxisLabelsIntersectAction.Wrap
		/// };
		/// 
		/// NumericalAxis yAxis = new NumericalAxis()
		/// {
		///   LabelsIntersectAction= AxisLabelsIntersectAction.Wrap
		/// };
		///
		/// chart.XAxes.Add(xAxis);
		/// chart.YAxes.Add(yAxis);
		/// 
		/// ]]>
		/// </code>
		/// *** 
		/// </example>
		public AxisLabelsIntersectAction LabelsIntersectAction
		{
			get { return (AxisLabelsIntersectAction)GetValue(LabelsIntersectActionProperty); }
			set { SetValue(LabelsIntersectActionProperty, value); }
		}

		/// <summary>
		/// Gets or sets the position of the axis tick lines. 
		/// </summary>
		/// <value>It accepts <see cref="AxisElementPosition"/> values and its default value is <see cref="AxisElementPosition.Outside"/>.</value>
		/// <remarks><para> <b>Note:</b> This is only applicable for the secondary axis of the <see cref="SfPolarChart"/>.</para></remarks>
		/// <example>
		/// # [MainPage.xaml](#tab/tabid-1)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		///  
		///     <chart:SfCartesianChart.XAxes>
		///         <chart:CategoryAxis TickPosition="Inside" />
		///     </chart:SfCartesianChart.XAxes>
		///
		///     <chart:SfCartesianChart.YAxes>
		///         <chart:NumericalAxis TickPosition="Inside" />
		///     </chart:SfCartesianChart.YAxes>
		/// 
		/// </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [MainPage.xaml.cs](#tab/tabid-2)
		/// <code><![CDATA[
		/// SfCartesianChart chart = new SfCartesianChart();
		/// 
		/// CategoryAxis xAxis = new CategoryAxis()
		/// {
		///    TickPosition = AxisElementPosition.Inside 
		/// };
		/// 
		/// NumericalAxis yAxis = new NumericalAxis()
		/// {
		///    TickPosition = AxisElementPosition.Inside 
		/// };
		///
		/// chart.XAxes.Add(xAxis);
		/// chart.YAxes.Add(yAxis);
		/// 
		/// ]]>
		/// </code>
		/// *** 
		/// </example>
		public AxisElementPosition TickPosition
		{
			get { return (AxisElementPosition)GetValue(TickPositionProperty); }
			set { SetValue(TickPositionProperty, value); }
		}

		/// <summary>
		/// Gets or sets the position of the axis labels. 
		/// </summary>
		/// <value>It accepts <see cref="AxisElementPosition"/> values and its default value is <see cref="AxisElementPosition.Outside"/>.</value>
		/// <remarks>
		/// Edge labels may overlap when both primary and secondary axis labels are positioned inside. 
		/// In this case, we can utilize the inside element support, depending on the scenario.
		/// <para> <b>Note:</b> This is only applicable for the secondary axis of the <see cref="SfPolarChart"/>.</para>
		/// </remarks>
		/// <example>
		/// # [MainPage.xaml](#tab/tabid-1)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		///  
		///     <chart:SfCartesianChart.XAxes>
		///         <chart:CategoryAxis LabelsPosition="Inside" />
		///     </chart:SfCartesianChart.XAxes>
		///
		///     <chart:SfCartesianChart.YAxes>
		///         <chart:NumericalAxis LabelsPosition="Inside" />
		///     </chart:SfCartesianChart.YAxes>
		/// 
		/// </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [MainPage.xaml.cs](#tab/tabid-2)
		/// <code><![CDATA[
		/// SfCartesianChart chart = new SfCartesianChart();
		/// 
		/// CategoryAxis xAxis = new CategoryAxis()
		/// {
		///    LabelsPosition = AxisElementPosition.Inside
		/// };
		/// 
		/// NumericalAxis yAxis = new NumericalAxis()
		/// { 
		///    LabelsPosition = AxisElementPosition.Inside
		/// };
		///
		/// chart.XAxes.Add(xAxis);
		/// chart.YAxes.Add(yAxis);
		/// 
		/// ]]>
		/// </code>
		/// *** 
		/// </example>
		public AxisElementPosition LabelsPosition
		{
			get { return (AxisElementPosition)GetValue(LabelsPositionProperty); }
			set { SetValue(LabelsPositionProperty, value); }
		}

		/// <summary>
		/// Gets the axis labels visible
		/// </summary>
		public ObservableCollection<ChartAxisLabel> VisibleLabels { get; internal set; }

		#endregion

		#region Internal Properties

		internal double ActualCrossingValue { get; set; } = double.NaN;

		internal CartesianAxisLabelsRenderer? AxisLabelsRenderer { get; set; }

		internal CartesianAxisElementRenderer? AxisElementRenderer { get; set; }

		internal CartesianAxisRenderer? AxisRenderer { get; set; }

		internal List<ChartSeries> RegisteredSeries { get; set; }

		internal ObservableCollection<ChartPlotBand> ActualPlotBands { get; set; }

		internal int SideBySideSeriesCount { get; set; }

		internal bool IsScrolling { get; set; }

		//Todo: Need to provide support for next release.

		/// <summary>
		/// Gets or sets the collection of the ChartAxisRangeStyle to customize the axis GridLine, TickLine and LabelStyle for specific range. 
		/// </summary>
		/// <value>This property takes the <see cref="ChartAxisRangeStyleCollection"/> as its value.</value>
		internal ChartAxisRangeStyleCollection RangeStyles
		{
			get { return (ChartAxisRangeStyleCollection)GetValue(RangeStylesProperty); }
			set { SetValue(RangeStylesProperty, value); }
		}

		/// <summary>
		/// Gets or sets the value that determines the number of labels to be displayed per 100 pixels. 
		/// </summary>
		/// <value>This property takes the integer value.</value>
		/// <remarks>This property used to give constrain over the auto generated labels, which reduces the number elements rendering in view.</remarks>
		///TODO: Provide alter way for this support. 
		internal int MaximumLabels
		{
			get { return (int)GetValue(MaximumLabelsProperty); }
			set { SetValue(MaximumLabelsProperty, value); }
		}

		internal static readonly BindableProperty TrackballAxisBackgroundProperty = BindableProperty.Create(nameof(TrackballAxisBackground), typeof(Brush), typeof(ChartAxis), SolidColorBrush.Black, BindingMode.Default, null, null);

		internal Brush TrackballAxisBackground
		{
			get { return (Brush)GetValue(TrackballAxisBackgroundProperty); }
			set { SetValue(TrackballAxisBackgroundProperty, value); }
		}

		internal static readonly BindableProperty TrackballAxisFontSizeProperty = BindableProperty.Create(nameof(TrackballAxisFontSize), typeof(double), typeof(ChartAxis), 12.0, BindingMode.Default, null, null, null, null);

		internal double TrackballAxisFontSize
		{
			get { return (double)GetValue(TrackballAxisFontSizeProperty); }
			set { SetValue(TrackballAxisFontSizeProperty, value); }
		}

		internal static readonly BindableProperty MajorGridLineStrokeProperty = BindableProperty.Create(nameof(MajorGridLineStroke), typeof(Brush), typeof(ChartAxis), new SolidColorBrush(Color.FromArgb("#E7E0EC")), BindingMode.Default, null, null);

		internal Brush MajorGridLineStroke
		{
			get { return (Brush)GetValue(MajorGridLineStrokeProperty); }
			set { SetValue(MajorGridLineStrokeProperty, value); }
		}

		#region is Polar
		internal PolarChartArea? PolarArea { get; set; }

		internal int PolarStartAngle
		{
			get
			{
				if (Parent is SfPolarChart chart)
				{
					return chart.PolarStartAngle;
				}

				return 270;
			}
		}
		#endregion


		#endregion

		#region Methods

		#region Theme Interface Implementation

		void IThemeElement.OnControlThemeChanged(string oldTheme, string newTheme)
		{
		}

		void IThemeElement.OnCommonThemeChanged(string oldTheme, string newTheme)
		{
		}

		#endregion

		#region Internal Methods

		internal float PointToValue(PointF point)
		{
			if (point == PointF.Zero)
			{
				return float.NaN;
			}

			return (float)PointToValue(point.X, point.Y);
		}

		/// <summary>
		/// Calculate axis desired size.
		/// </summary>
		/// <param name="size"></param>
		/// <returns></returns>
		internal void ComputeSize(Size size)
		{
			AvailableSize = size;

			var plotSize = GetPlotSize(size);

			CalculateRangeAndInterval(plotSize);

			if (IsVisible)
			{
				UpdateRenderers();
				UpdateLabels(); //Generate visible labels
				ComputedDesiredSize = ComputeDesiredSize(size);
			}
			else
			{
				//TODO: Need to validate desired size.
				UpdateLabels(); //Generate visible labels
				ActualPlotOffsetStart = double.IsNaN(PlotOffsetStart) ? 0f : PlotOffsetStart;
				ActualPlotOffsetEnd = double.IsNaN(PlotOffsetEnd) ? 0f : PlotOffsetEnd;
				ActualPlotOffset = ActualPlotOffsetStart + ActualPlotOffsetEnd;
				InsidePadding = 0;
				AxisRenderer = null;
				ComputedDesiredSize = !IsVertical ? new Size(size.Width, 0) : new Size(0, size.Height);
			}
		}

		internal bool RenderRectContains(float x, float y)
		{
			Rect rect;
			var labelsRect = AxisLabelsRenderer?.LabelLayout?.LabelsRect;

			if (labelsRect == null || labelsRect.Count == 0)
			{
				return false;
			}

			int count = labelsRect.Count;

			if (IsVertical)
			{
				rect = new Rect(ArrangeRect.Left, ArrangeRect.Top - (labelsRect[count - 1].Height / 2), ArrangeRect.Width, ArrangeRect.Height + (labelsRect[count - 1].Height / 2) + (labelsRect[0].Height / 2));
			}
			else
			{
				rect = new Rect(ArrangeRect.Left - (labelsRect[0].Width / 2), ArrangeRect.Top, ArrangeRect.Width + (labelsRect[0].Width / 2) + (labelsRect[count - 1].Width / 2), ArrangeRect.Height);
			}

			if (LabelsPosition == AxisElementPosition.Inside)
			{
				var isOpposed = IsOpposed();

				if (IsVertical && !isOpposed)
				{
					rect.Width += InsidePadding;
				}
				else if (!IsVertical && isOpposed)
				{
					rect.Height += InsidePadding;
				}
			}

			return rect.Contains(x, y);
		}

		internal bool IsOpposed()
		{
			var crossAxis = _cartesianArea != null ? GetCrossingAxis(_cartesianArea) : null;

			if (crossAxis == null)
			{
				return false;
			}

			var isInversedAxis = crossAxis.IsInversed;
			var isMaxValueAndNotInversed = ActualCrossingValue == double.MaxValue && !isInversedAxis;
			var isNaNOrMinValueAndInversed = (double.IsNaN(ActualCrossingValue) || ActualCrossingValue == double.MinValue) && isInversedAxis;

			return isMaxValueAndNotInversed || isNaNOrMinValueAndInversed;
		}

		internal virtual double ValueToPolarCoefficient(double value)
		{
			double result;
			double start = VisibleRange.Start;
			double delta = VisibleRange.Delta;
			result = (value - start) / delta;

			double scalingFactor = VisibleLabels.Count > 0
				? 1 - (1.0 / VisibleLabels.Count)
				: 1 - (1.0 / (delta + 1));

			result *= scalingFactor;

			return (float)result;
		}

		internal double ValueToPolarAngle(double value)
		{
			double angleValue = ValueToPolarCoefficient(value);

			return angleValue * 360;
		}

		internal void UpdateLayout()
		{
			if (Area != null)
			{
				Area.NeedsRelayout = true;
				Area.ScheduleUpdateArea();
			}

			if (PolarArea != null)
			{
				PolarArea.NeedsRelayout = true;
				PolarArea.ScheduleUpdateArea();
			}
		}

		internal bool CanRenderNextToCrossingValue()
		{
			return RenderNextToCrossingValue
				&& !double.IsNaN(ActualCrossingValue)
				&& ActualCrossingValue != double.MinValue
				&& ActualCrossingValue != double.MaxValue;
		}

		internal ChartAxis? GetCrossingAxis(CartesianChartArea area)
		{
			if (!string.IsNullOrEmpty(CrossAxisName))
			{
				var axisLayout = area._axisLayout;
				var axes = IsVertical ? axisLayout.HorizontalAxes : axisLayout.VerticalAxes;

				foreach (var axis in axes.Where(a => a.Name != null && a.Name.Equals(CrossAxisName, StringComparison.Ordinal)))
				{
					return axis;
				}
			}

			if (AssociatedAxes.Count > 0)
			{
				return AssociatedAxes[0];
			}
			else
			{
				return _isVertical ^ (RegisteredSeries.Count > 0 && area.IsTransposed)
												? area.PrimaryAxis
													: area.SecondaryAxis;
			}
		}

		internal bool CanDrawMajorGridLines()
		{
			return ShowMajorGridLines && MajorGridLineStyle.CanDraw();
		}

		//Axes change on register series change. 
		internal void AddRegisteredSeries(ChartSeries series)
		{
			if (!RegisteredSeries.Contains(series))
			{
				RegisteredSeries.Add(series);
			}
		}

		internal void ClearRegisteredSeries()
		{
			if (Area != null)
			{
				RegisteredSeries.Clear();
				AssociatedAxes.Clear();
			}
		}

		internal void RemoveRegisteredSeries(CartesianSeries series)
		{
			RegisteredSeries.Remove(series);
			if (series != null)
			{
				var xAxis = series.ActualXAxis;
				var area = _cartesianArea;
				if (area != null && xAxis != null && series.ActualYAxis is RangeAxisBase yAxis)
				{
					if (!area._yAxes.Contains(yAxis))
					{
						xAxis.AssociatedAxes.Remove(yAxis);
					}

					if (!area._xAxes.Contains(xAxis))
					{
						yAxis.AssociatedAxes.Remove(xAxis);
					}
				}
			}
		}

		internal virtual void Dispose()
		{
			if (Title != null)
			{
				SetInheritedBindingContext(Title, null);
				Title.Axis = null;
				Title.Parent = null;
			}

			if (AxisLineStyle != null)
			{
				SetInheritedBindingContext(AxisLineStyle, null);
				AxisLineStyle.PropertyChanged -= Style_PropertyChanged;
				AxisLineStyle.Parent = null;
			}

			if (LabelStyle != null)
			{
				SetInheritedBindingContext(LabelStyle, null);
				LabelStyle.PropertyChanged -= Style_PropertyChanged;
				LabelStyle.Parent = null;
			}

			if (MajorGridLineStyle != null)
			{
				SetInheritedBindingContext(MajorGridLineStyle, null);
				MajorGridLineStyle.PropertyChanged -= Style_PropertyChanged;
				MajorGridLineStyle.Parent = null;
			}

			if (MajorTickStyle != null)
			{
				SetInheritedBindingContext(MajorTickStyle, null);
				MajorTickStyle.PropertyChanged -= Style_PropertyChanged;
				MajorTickStyle.Parent = null;
			}

			if (ActualPlotBands != null)
			{
				foreach (var plotBand in ActualPlotBands)
				{
					SetInheritedBindingContext(plotBand, null);
					plotBand.Parent = null;
				}
			}

			RegisteredSeries?.Clear();

			AssociatedAxes?.Clear();

			AxisLabelsRenderer = null;
			AxisElementRenderer = null;
			Area = null;
		}

		internal void InvalidatePlotBands()
		{
			if (_cartesianArea != null && _cartesianArea.PlotArea is CartesianPlotArea plotArea)
			{
				plotArea.InvalidatePlotBands();
			}
		}

		internal static void InitializeTrackballAxisDynamicResource(ChartLabelStyle style)
		{
			style.SetDynamicResource(ChartLabelStyle.StrokeProperty, "SfCartesianChartTrackballAxisLabelStroke");
			style.SetDynamicResource(ChartLabelStyle.TextColorProperty, "SfCartesianChartTrackballAxisLabelTextColor");
			style.SetDynamicResource(ChartLabelStyle.CornerRadiusProperty, "SfCartesianChartTrackballAxisLabelCornerRadius");
		}

		internal void PlotBandsCollection_Changed(object? sender, NotifyCollectionChangedEventArgs e)
		{
			e.ApplyCollectionChanges((plotBand, index, canInsert) => AddPlotBand(index, plotBand), (plotBand, index) => RemovePlotBand(index, plotBand), ResetPlotBand);
			InvalidatePlotBands();
		}

		internal static void OnPlotBandsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is ChartAxis axis)
			{
				axis.OnPlotBandsPropertyChanged(oldValue as NumericalPlotBandCollection, newValue as NumericalPlotBandCollection);
			}
		}

		#endregion

		#region Protected Methods

		/// <summary>
		/// Calculates the desired size of a chart axis elements.
		/// </summary>
		protected virtual Size ComputeDesiredSize(Size availableSize)
		{
			if (AxisRenderer != null)
			{
				return AxisRenderer.ComputeDesiredSize(availableSize);
			}

			return availableSize;
		}

		/// <summary>
		/// Draws the axis and its elements.
		/// </summary>
		/// <param name="canvas"></param>
		/// <param name="arrangeRect"></param>
		protected internal virtual void DrawAxis(ICanvas canvas, Rect arrangeRect)
		{
			if (AxisRenderer != null)
			{
				UpdateLayoutTranslate(canvas, AxisRenderer, arrangeRect);
				AxisRenderer.OnDraw(canvas);
			}
		}

		/// <summary>
		/// Draws the axis major tick lines.
		/// </summary>
		protected internal virtual void DrawMajorTick(ICanvas canvas, double tickPosition, PointF point1, PointF point2)
		{
			canvas.DrawLine(point1, point2);
		}

		/// <summary>
		/// Draw the axis minor tick lines.
		/// </summary>
		protected internal virtual void DrawMinorTick(ICanvas canvas, double tickPosition, PointF point1, PointF point2)
		{
			canvas.DrawLine(point1, point2);
		}

		/// <summary>
		/// Draws the axis lines.
		/// </summary>
		protected internal virtual void DrawAxisLine(ICanvas canvas, float x1, float y1, float x2, float y2)
		{
			canvas.DrawLine(x1, y1, x2, y2);
		}

		/// <summary>
		/// Draws the axis grid lines.
		/// </summary>
		protected internal virtual void DrawGridLine(ICanvas canvas, double position, float x1, float y1, float x2, float y2)
		{
			canvas.DrawLine(x1, y1, x2, y2);
		}

		/// <inheritdoc/>
		/// <exclude/>
		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();

			if (AxisLineStyle != null)
			{
				SetInheritedBindingContext(AxisLineStyle, BindingContext);
			}

			if (MajorGridLineStyle != null)
			{
				SetInheritedBindingContext(MajorGridLineStyle, BindingContext);
			}

			if (MajorTickStyle != null)
			{
				SetInheritedBindingContext(MajorTickStyle, BindingContext);
			}

			if (LabelStyle != null)
			{
				SetInheritedBindingContext(LabelStyle, BindingContext);
			}

			if (Title != null)
			{
				SetInheritedBindingContext(Title, BindingContext);
			}

			if (TrackballLabelStyle != null)
            {
                SetInheritedBindingContext(TrackballLabelStyle, BindingContext);
            }

			if (ActualPlotBands != null)
			{
				foreach (var plotBand in ActualPlotBands)
				{
					SetInheritedBindingContext(plotBand, BindingContext);
				}
			}
		}

		/// <summary>
		/// Sets the parent for the axis elements.
		/// </summary>
		/// <exclude/>
		protected override void OnParentSet()
		{
			base.OnParentSet();

			if (LabelStyle != null)
			{
				LabelStyle.Parent = Parent;
			}

			if (MajorGridLineStyle != null)
			{
				MajorGridLineStyle.Parent = Parent;
			}

			if (AxisLineStyle != null)
			{
				AxisLineStyle.Parent = Parent;
			}

			if (MajorTickStyle != null)
			{
				MajorTickStyle.Parent = Parent;
			}

			if (Title != null)
			{
				Title.Parent = Parent;
			}

			if (TrackballLabelStyle != null)
			{
				TrackballLabelStyle.Parent = Parent;
			}

			if (ActualPlotBands != null)
			{
				foreach (var band in ActualPlotBands)
				{
					band.Parent = Parent;
				}
			}
		}

		#endregion

		#region Private Methods

		void InitializeConstructor()
		{
			ThemeElement.InitializeThemeResources(this, "SfCartesianChartTheme");
			SetDynamicResource(TrackballAxisBackgroundProperty, "SfCartesianChartTrackballAxisLabelBackground");
			SetDynamicResource(TrackballAxisFontSizeProperty, "SfCartesianChartTrackballAxisLabelTextFontSize");
			SetDynamicResource(MajorGridLineStrokeProperty, "SfCartesianChartMajorGridLineStroke");
			//Todo: Remove this code, After ClipsToBounds works in iOS and Windows.
			EdgeLabelsDrawingMode = EdgeLabelsDrawingMode.Shift;
			AxisLineStyle = new ChartLineStyle();
			LabelStyle = new ChartAxisLabelStyle();
			MajorGridLineStyle = new ChartLineStyle() { Stroke = MajorGridLineStroke };
			MajorTickStyle = new ChartAxisTickStyle();
			TrackballLabelStyle = new ChartLabelStyle() { FontSize = TrackballAxisFontSize, Background = TrackballAxisBackground };
		}

		void UpdateRenderers()
		{
			AxisLabelsRenderer ??= new CartesianAxisLabelsRenderer(this);

			AxisElementRenderer ??= new CartesianAxisElementRenderer(this);

			if (AxisRenderer != null)
			{
				AxisRenderer.LayoutCalculators.Clear();
			}
			else
			{
				AxisRenderer = new CartesianAxisRenderer(this);
			}

			AxisRenderer.LayoutCalculators.Add(AxisLabelsRenderer);
			AxisRenderer.LayoutCalculators.Add(AxisElementRenderer);
		}

		Size GetPlotSize(Size availableSize)
		{
			if (!IsVertical)
			{
				return new Size(availableSize.Width - GetActualPlotOffset(), availableSize.Height);
			}
			else
			{
				return new Size(availableSize.Width, availableSize.Height - GetActualPlotOffset());
			}
		}

		void InitTitle(ChartAxisTitle title)
		{
			title.Axis = this;
		}

		void UpdateAxisLayout()
		{
			AxisRenderer?.Layout(new Size(_arrangeRect.Width, _arrangeRect.Height));
		}

		void ResetPlotBand()
		{
			ActualPlotBands.Clear();
		}

#pragma warning disable IDE0060 // Remove unused parameter
		void RemovePlotBand(int index, object item)
#pragma warning restore IDE0060 // Remove unused parameter
		{
			if (item is ChartPlotBand plotBand)
			{
				ActualPlotBands.Remove(plotBand);
			}
		}

#pragma warning disable IDE0060 // Remove unused parameter
		void AddPlotBand(int index, object item)
#pragma warning restore IDE0060 // Remove unused parameter
		{
			if (item is ChartPlotBand plotBand)
			{
				plotBand.Parent = Parent;
				plotBand.Axis = this;
				ActualPlotBands.Add(plotBand);
			}
		}

		/// <summary>
		/// Translate the axis position to edge of series clip when axis crosses enable. 
		/// </summary>
		void UpdateLayoutTranslate(ICanvas canvas, CartesianAxisRenderer axisRenderer, Rect arrangeRect)
		{
			var axisArrangeRect = new Rect(arrangeRect.Location, arrangeRect.Size);

			if (CanRenderNextToCrossingValue() && _cartesianArea != null)
			{
				Rect clip = _cartesianArea.ActualSeriesClipRect;

				axisArrangeRect = ClipRect(axisArrangeRect, clip, arrangeRect, IsVertical);
			}

			if (axisArrangeRect != arrangeRect)
			{
				//Hide line and ticks when axis tries to render out side of series clip.
				axisRenderer.UpdateRendererVisible(false);
				canvas.Translate((float)axisArrangeRect.Left, (float)axisArrangeRect.Top);
			}
			else
			{
				axisRenderer.UpdateRendererVisible(true);
				canvas.Translate((float)arrangeRect.Left, (float)arrangeRect.Top);
			}
		}

		static Rect ClipRect(Rect axisArrangeRect, Rect clip, Rect arrangeRect, bool isVertical)
		{
			if (isVertical)
			{
				if (axisArrangeRect.Left < clip.Left)
				{
					axisArrangeRect.Left = clip.Left;
				}

				if (axisArrangeRect.Right > clip.Right)
				{
					axisArrangeRect.Left = clip.Right - arrangeRect.Width;
				}
			}
			else
			{
				if (axisArrangeRect.Top < clip.Top)
				{
					axisArrangeRect.Top = clip.Top;
				}

				if (axisArrangeRect.Bottom > clip.Bottom)
				{
					axisArrangeRect.Top = clip.Bottom - arrangeRect.Height;
				}
			}

			return axisArrangeRect;
		}

		#region Property Changed Methods

		static void OnShowMajorGridLinesChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is ChartAxis axis)
			{
				axis.UpdateLayout();
			}
		}

		static void OnMajorTickStyleChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is ChartAxis axis)
			{
				if (oldValue is ChartAxisTickStyle style)
				{
					SetInheritedBindingContext(style, null);
					style.PropertyChanged -= axis.Style_PropertyChanged;
				}

				if (newValue is ChartAxisTickStyle tickStyle)
				{
					SetInheritedBindingContext(tickStyle, axis.BindingContext);
					axis.MajorTickStyle.InitializeDynamicResource("MajorTickStyle");
					tickStyle.PropertyChanged += axis.Style_PropertyChanged;
				}

				ChartBase.SetParent((Element?)oldValue, (Element?)newValue, axis.Parent);
				axis?.UpdateLayout();
			}
		}

		static void OnMajorGridLineStyleChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is ChartAxis axis)
			{
				if (oldValue is ChartLineStyle style)
				{
					SetInheritedBindingContext(style, null);
					style.PropertyChanged -= axis.Style_PropertyChanged;
				}

				if (newValue is ChartLineStyle tickStyle)
				{
					SetInheritedBindingContext(tickStyle, axis.BindingContext);
					tickStyle.PropertyChanged += axis.Style_PropertyChanged;
				}

				ChartBase.SetParent((Element?)oldValue, (Element?)newValue, axis.Parent);
				axis?.UpdateLayout();
			}
		}

		static void OnEdgeLabelsDrawingModeChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is ChartAxis axis)
			{
				axis.UpdateLayout();
			}
		}

		static void OnInversedChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is ChartAxis axis)
			{
				axis.UpdateLayout();
			}
		}

		static void OnTitleChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is ChartAxis axis)
			{
				// Resolved the issue with dynamically adding axis titles to charts.
				if (oldValue is ChartAxisTitle axisTitle)
				{
					SetInheritedBindingContext(axisTitle, null);
					axisTitle.PropertyChanged -= axis.Style_PropertyChanged;
				}

				if (newValue is ChartAxisTitle title)
				{
					SetInheritedBindingContext(title, axis.BindingContext);
					axis.InitTitle(title);
					title.PropertyChanged += axis.Style_PropertyChanged;
				}

				ChartBase.SetParent((Element?)oldValue, (Element?)newValue, axis.Parent);
				axis.UpdateLayout();
			}
		}

		static void OnCrossesChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is ChartAxis axis && newValue != null)
			{
				axis.ActualCrossingValue = ChartUtils.ConvertToDouble(newValue);
				axis.UpdateLayout();
			}
		}

		static void OnRenderNextToCrossingValuePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is ChartAxis axis)
			{
				axis.UpdateLayout();
			}
		}

		static void OnCrossAxisNameChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is ChartAxis axis)
			{
				axis.UpdateLayout();
			}
		}

		static void OnAxisLineStyleChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is ChartAxis axis)
			{
				if (oldValue is ChartLineStyle style)
				{
					SetInheritedBindingContext(style, null);
					style.PropertyChanged -= axis.Style_PropertyChanged;
				}

				if (newValue is ChartLineStyle lineStyle)
				{
					SetInheritedBindingContext(lineStyle, axis.BindingContext);
					lineStyle.SetDynamicResource(ChartLineStyle.StrokeProperty, "SfCartesianChartAxisLineStroke");
					lineStyle.PropertyChanged += axis.Style_PropertyChanged;
				}

				ChartBase.SetParent((Element?)oldValue, (Element?)newValue, axis.Parent);
				axis.UpdateLayout();
			}
		}

		static void OnRangeStylesPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (oldValue != null)
			{
				//TODO: Unhook collection changed
			}

			if (newValue != null)
			{
				//TODO: Hook collection changed.
			}

			(bindable as ChartAxis)?.UpdateLayout();
		}

		static void OnLabelStyleChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is ChartAxis axis)
			{
				if (oldValue is ChartAxisLabelStyle labelStyle)
				{
					SetInheritedBindingContext(labelStyle, null);
					labelStyle.PropertyChanged -= axis.Style_PropertyChanged;
				}

				if (newValue is ChartAxisLabelStyle style)
				{
					SetInheritedBindingContext(style, axis.BindingContext);
					style.PropertyChanged += axis.Style_PropertyChanged;
				}

				ChartBase.SetParent((Element?)oldValue, (Element?)newValue, axis.Parent);
				axis.UpdateLayout();
			}
		}

		static void OnLabelRotationAngleChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is ChartAxis axis)
			{
				axis.UpdateLayout();
			}
		}

		static void OnLabelExtentChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is ChartAxis axis)
			{
				axis.UpdateLayout();
			}
		}

		static void OnTrackballLabelStyleChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is ChartAxis axis)
			{
				if (oldValue is ChartLabelStyle labelStyle)
				{
					SetInheritedBindingContext(labelStyle, null);
					ChartAxis.InitializeTrackballAxisDynamicResource(labelStyle);
				}

				if (newValue is ChartLabelStyle style)
				{
					SetInheritedBindingContext(style, axis.BindingContext);
					ChartAxis.InitializeTrackballAxisDynamicResource(style);
				}

				ChartBase.SetParent((Element?)oldValue, (Element?)newValue, axis.Parent);
			}
		}

		static void OnAutoScrollingModeChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is ChartAxis axis)
			{
				axis.CanAutoScroll = true;
				axis.UpdateLayout();
			}
		}

		static void OnLabelsIntersectActionChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is ChartAxis axis)
			{
				axis.UpdateLayout();
			}
		}

		static void OnAxisLineOffsetChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is ChartAxis axis)
			{
				axis.UpdateLayout();
			}
		}

		static void OnPlotOffsetStartChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is ChartAxis axis)
			{
				axis.UpdateActualPlotOffsetStart((double)newValue);
				axis.UpdateLayout();
			}
		}

		static void OnPlotOffsetEndChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is ChartAxis axis)
			{
				axis.UpdateActualPlotOffsetEnd((double)newValue);
				axis.UpdateLayout();
			}
		}

		static void OnIsVisibleChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is ChartAxis axis)
			{
				axis.UpdateLayout();
			}
		}

		static void OnMaximumLabelsChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is ChartAxis axis)
			{
				axis.UpdateLayout();
			}
		}

		static void OnTickPositionChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is ChartAxis axis)
			{
				axis.UpdateLayout();
			}
		}

		static void OnLabelsPositionChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is ChartAxis axis)
			{
				axis.UpdateLayout();
			}
		}

		static void OnAutoScrollingDeltaPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is ChartAxis axis && newValue != null)
			{
				var delta = Convert.ToDouble(newValue ?? double.NaN);
				bool needAutoScroll = !double.IsNaN(delta) && delta > 0;

				if (needAutoScroll)
				{
					axis.ActualAutoScrollDelta = delta;
					axis.CanAutoScroll = true;
					axis.UpdateLayout();
				}
			}
		}

		static void OnNameChanged(BindableObject bindable, object oldValue, object newValue)
		{
		}

		static void OnEnableAutoIntervalOnZoomingPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
		}

		static void OnZoomFactorChanged(BindableObject bindable, object oldValue, object newValue)
		{
			//TODO: zoom factor need to set between 0 to 1
			if (bindable is ChartAxis axis)
			{
				var oldFactor = oldValue == null ? 1 : (double)oldValue;
				var newFactor = newValue == null ? 1 : (double)newValue > 1 ? 1 : (double)newValue < 0 ? 0 : (double)newValue;

				if (oldFactor != newFactor)
				{
					//TODO: Set new factor to actual value;
					axis.UpdateLayout();
				}
			}
		}

		static void OnZoomPositionChanged(BindableObject bindable, object oldValue, object newValue)
		{
			//TODO: ZoomPosition need to set between 0 to 1
			if (bindable is ChartAxis axis)
			{
				var oldPosition = oldValue == null ? 1 : (double)oldValue;
				var newPosition = newValue == null ? 1 : (double)newValue > 1 ? 1 : (double)newValue < 0 ? 0 : (double)newValue;

				if (oldPosition != newPosition)
				{
					//TODO: Set new position to actual value;
					axis.UpdateLayout();
				}
			}
		}

		void Style_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			UpdateLayout();
		}

		void OnPlotBandsPropertyChanged(NumericalPlotBandCollection? oldValue, NumericalPlotBandCollection? newValue)
		{
			if (oldValue != null)
			{
				oldValue.CollectionChanged -= PlotBandsCollection_Changed;
				ActualPlotBands.Clear();

				foreach (var band in oldValue)
				{
					band.Parent = null;
					band.Axis = null;
				}
			}

			if (newValue != null)
			{
				newValue.CollectionChanged += PlotBandsCollection_Changed;
				ActualPlotBands.Clear();

				foreach (var plotBand in newValue)
				{
					plotBand.Parent = Parent;
					plotBand.Axis = this;
					ActualPlotBands.Add(plotBand);
				}
			}

			InvalidatePlotBands();
		}

		#endregion

		#endregion

		#endregion
	}
}
