using Syncfusion.Maui.Toolkit.Charts.Chart.Layouts;
using Syncfusion.Maui.Toolkit.Graphics.Internals;
using Syncfusion.Maui.Toolkit.Internals;
using Syncfusion.Maui.Toolkit.Themes;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using PointerEventArgs = Syncfusion.Maui.Toolkit.Internals.PointerEventArgs;

namespace Syncfusion.Maui.Toolkit.Charts
{
    /// <summary>
    /// Renders different types of cartesian-type charts, each representing a unique style of representing data with a more user-friendly and greater UI visualization.
    /// </summary>
    /// <remarks>
    /// <para>The Cartesian chart control is used to visualize the data graphically, it typically have horizontal and vertical axes. </para>
    ///
    /// <para>SfCartesianChart class properties provides an option to add the series and axis collection, allows to customize the chart elements such as series, axis, legend, data label and tooltip features.</para>
    ///
    /// <img src="https://cdn.syncfusion.com/content/images/maui/MAUI_CartesianChart.png"/>
    /// 
    /// <b>Axis</b>
    /// 
    /// <para><b>ChartAxis</b> is used to locate a data point inside the chart area. Charts typically have two axes that are used to measure and categorize data. 
    /// <b>Vertical(Y)</b> axis always uses numerical scale. <b>Horizontal(X)</b> axis supports the <b>Category, Numeric and Date time</b>.</para>
    /// 
    /// <para>To render an axis, the chart axis instance has to be added in chart’s <see cref="XAxes"/> and <see cref="YAxes"/> collection as per the following code snippet.</para>
    /// 
    /// # [MainPage.xaml](#tab/tabid-1)
    /// <code><![CDATA[
    /// <chart:SfCartesianChart>
    /// 
    ///         <chart:SfCartesianChart.BindingContext>
    ///             <local:ViewModel/>
    ///         </chart:SfCartesianChart.BindingContext>
    /// 
    ///         <chart:SfCartesianChart.XAxes>
    ///             <chart:NumericalAxis/>
    ///         </chart:SfCartesianChart.XAxes>
    /// 
    ///         <chart:SfCartesianChart.YAxes>
    ///             <chart:NumericalAxis/>
    ///         </chart:SfCartesianChart.YAxes>
    /// 
    /// </chart:SfCartesianChart>
    /// ]]>
    /// </code>
    /// # [MainPage.xaml.cs](#tab/tabid-2)
    /// <code><![CDATA[
    /// SfCartesianChart chart = new SfCartesianChart();
    /// 
    /// ViewModel viewModel = new ViewModel();
    ///	chart.BindingContext = viewModel;
    /// 
    /// NumericalAxis xAxis = new NumericalAxis();
    /// chart.XAxes.Add(xAxis);	
    /// 
    /// NumericalAxis yAxis = new NumericalAxis();
    /// chart.YAxes.Add(yAxis);
    /// 
    /// ]]>
    /// </code>
    /// ***
    /// 
    /// <para><b>Series</b></para>
    /// 
    /// <para>ChartSeries is the visual representation of data. SfCartesianChart offers many types such as Line, Fast line, Spline, Column, Scatter, Area and SplineArea series. Based on your requirements and specifications, any type of series can be added for data visualization.</para>
    /// 
    /// <para>To render a series, create an instance of required series class, and add it to the <see cref="Series"/> collection.</para>
    /// 
    /// # [MainPage.xaml](#tab/tabid-3)
    /// <code> <![CDATA[
    /// <chart:SfCartesianChart>
    ///
    ///        <chart:SfCartesianChart.BindingContext>
    ///            <local:ViewModel/>
    ///        </chart:SfCartesianChart.BindingContext>
    ///
    ///        <chart:SfCartesianChart.XAxes>
    ///            <chart:NumericalAxis/>
    ///        </chart:SfCartesianChart.XAxes>
    ///
    ///        <chart:SfCartesianChart.YAxes>
    ///            <chart:NumericalAxis/>
    ///        </chart:SfCartesianChart.YAxes>
    ///
    ///        <chart:SfCartesianChart.Series>
    ///            <chart:LineSeries ItemsSource = "{Binding Data}" XBindingPath="XValue" YBindingPath="YValue1"/>
    ///            <chart:LineSeries ItemsSource = "{Binding Data}" XBindingPath="XValue" YBindingPath="YValue2"/>
    ///        </chart:SfCartesianChart.Series>
    /// </chart:SfCartesianChart>
    /// ]]>
    /// </code>
    /// # [MainPage.xaml.cs](#tab/tabid-4)
    /// <code><![CDATA[
    /// SfCartesianChart chart = new SfCartesianChart();
    /// 
    /// ViewModel viewModel = new ViewModel();
    ///	chart.BindingContext = viewModel;
    /// 
    /// NumericalAxis xaxis = new NumericalAxis();
    /// chart.XAxes.Add(xaxis);	
    /// 
    /// NumericalAxis yAxis = new NumericalAxis();
    /// chart.YAxes.Add(yAxis);
    /// 
    /// LineSeries series1 = new LineSeries()
    /// {
    ///     ItemsSource = viewModel.Data,
    ///     XBindingPath = "XValue",
    ///     YBindingPath = "YValue1"
    /// };
    /// chart.Series.Add(series1);
    /// 
    /// LineSeries series2 = new LineSeries()
    /// {
    ///     ItemsSource = viewModel.Data,
    ///     XBindingPath = "XValue",
    ///     YBindingPath = "YValue2"
    /// };
    /// chart.Series.Add(series2);
    /// ]]>
    /// </code>
    /// # [ViewModel.cs](#tab/tabid-5)
    /// <code><![CDATA[
    /// public ObservableCollection<Model> Data { get; set; }
    /// 
    /// public ViewModel()
    /// {
    ///    Data = new ObservableCollection<Model>();
    ///    Data.Add(new Model() { XValue = 10, YValue1 = 100, YValue2 = 110 });
    ///    Data.Add(new Model() { XValue = 20, YValue1 = 150, YValue2 = 100 });
    ///    Data.Add(new Model() { XValue = 30, YValue1 = 110, YValue2 = 130 });
    ///    Data.Add(new Model() { XValue = 40, YValue1 = 230, YValue2 = 180 });
    /// }
    /// ]]>
    /// </code>
    /// ***
    /// 
    /// <para><b>Legend</b></para>
    /// 
    /// <para>The Legend contains list of chart series or data points in chart. The information provided in each legend item helps to identify the corresponding data series in chart. The Series <see cref="CartesianSeries.Label"/> property text will be displayed in the associated legend item.</para>
    /// 
    /// <para>To render a legend, create an instance of <see cref="ChartLegend"/>, and assign it to the <see cref="ChartBase.Legend"/> property. </para>
    /// 
    /// # [MainPage.xaml](#tab/tabid-6)
    /// <code> <![CDATA[
    /// <chart:SfCartesianChart>
    ///
    ///        <chart:SfCartesianChart.BindingContext>
    ///            <local:ViewModel/>
    ///        </chart:SfCartesianChart.BindingContext>
    ///        
    ///        <chart:SfCartesianChart.Legend>
    ///            <chart:ChartLegend/>
    ///        </chart:SfCartesianChart.Legend>
    ///
    ///        <chart:SfCartesianChart.XAxes>
    ///            <chart:NumericalAxis/>
    ///        </chart:SfCartesianChart.XAxes>
    ///
    ///        <chart:SfCartesianChart.YAxes>
    ///            <chart:NumericalAxis/>
    ///        </chart:SfCartesianChart.YAxes>
    ///
    ///        <chart:SfCartesianChart.Series>
    ///            <chart:LineSeries Label="Singapore" ItemsSource = "{Binding Data}" XBindingPath="XValue" YBindingPath="YValue1"/>
    ///            <chart:LineSeries Label="Spain" ItemsSource = "{Binding Data}" XBindingPath="XValue" YBindingPath="YValue2"/>
    ///        </chart:SfCartesianChart.Series>
    /// </chart:SfCartesianChart>
    /// ]]>
    /// </code>
    /// # [MainPage.xaml.cs](#tab/tabid-7)
    /// <code><![CDATA[
    /// SfCartesianChart chart = new SfCartesianChart();
    /// 
    /// ViewModel viewModel = new ViewModel();
    ///	chart.BindingContext = viewModel;
    ///	
    /// chart.Legend = new ChartLegend();
    /// 
    /// NumericalAxis xAxis = new NumericalAxis();
    /// chart.XAxes.Add(xAxis);	
    /// 
    /// NumericalAxis yAxis = new NumericalAxis();
    /// chart.YAxes.Add(yAxis);
    /// 
    /// LineSeries series1 = new LineSeries()
    /// {
    ///     ItemsSource = viewModel.Data,
    ///     XBindingPath = "XValue",
    ///     YBindingPath = "YValue1",
    ///     Label = "Singapore"
    /// };
    /// chart.Series.Add(series1);
    /// 
    /// LineSeries series2 = new LineSeries()
    /// {
    ///     ItemsSource = viewModel.Data,
    ///     XBindingPath = "XValue",
    ///     YBindingPath = "YValue2",
    ///     Label = "Spain"
    /// };
    /// chart.Series.Add(series2);
    /// ]]>
    /// </code>
    /// ***
    /// 
    /// <para><b>Tooltip</b></para>
    /// 
    /// <para>Tooltip displays information while tapping or mouse hover on the segment. To display the tooltip on chart, you need to set the <see cref="ChartSeries.EnableTooltip"/> property as <b>true</b> in <see cref="ChartSeries"/>. </para>
    /// 
    /// <para>To customize the appearance of the tooltip elements like Background, TextColor and Font, create an instance of <see cref="ChartTooltipBehavior"/> class, modify the values, and assign it to the chart’s <see cref="ChartBase.TooltipBehavior"/> property. </para>
    /// 
    /// # [MainPage.xaml](#tab/tabid-8)
    /// <code><![CDATA[
    /// <chart:SfCartesianChart>
    /// 
    ///         <chart:SfCartesianChart.BindingContext>
    ///             <local:ViewModel/>
    ///         </chart:SfCartesianChart.BindingContext>
    /// 
    ///         <chart:SfCartesianChart.TooltipBehavior>
    ///             <chart:ChartTooltipBehavior/>
    ///         </chart:SfCartesianChart.TooltipBehavior>
    /// 
    ///         <chart:SfCartesianChart.XAxes>
    ///             <chart:NumericalAxis/>
    ///         </chart:SfCartesianChart.XAxes>
    /// 
    ///         <chart:SfCartesianChart.YAxes>
    ///             <chart:NumericalAxis/>
    ///         </chart:SfCartesianChart.YAxes>
    /// 
    ///         <chart:SfCartesianChart.Series>
    ///             <chart:LineSeries EnableTooltip = "True" ItemsSource="{Binding Data}" XBindingPath="XValue" YBindingPath="YValue1"/>
    ///             <chart:LineSeries EnableTooltip = "True" ItemsSource="{Binding Data}" XBindingPath="XValue" YBindingPath="YValue2"/>
    ///         </chart:SfCartesianChart.Series>
    /// 
    /// </chart:SfCartesianChart>
    /// ]]>
    /// </code>
    /// # [MainPage.xaml.cs](#tab/tabid-9)
    /// <code><![CDATA[
    /// SfCartesianChart chart = new SfCartesianChart();
    /// 
    /// ViewModel viewModel = new ViewModel();
    ///	chart.BindingContext = viewModel;
    ///
    /// chart.TooltipBehavior = new ChartTooltipBehavior();
    ///
    /// NumericalAxis xAxis = new NumericalAxis();
    /// chart.XAxes.Add(xAxis);	
    ///
    /// NumericalAxis yAxis = new NumericalAxis();
    /// chart.YAxes.Add(yAxis);
    ///
    /// LineSeries series1 = new LineSeries()
    /// {
    ///    ItemsSource = viewModel.Data,
    ///    XBindingPath = "XValue",
    ///    YBindingPath = "YValue1",
    ///    EnableTooltip = true
    /// };
    /// chart.Series.Add(series1);
    ///
    /// LineSeries series2 = new LineSeries()
    /// {
    ///    ItemsSource = viewModel.Data,
    ///    XBindingPath = "XValue",
    ///    YBindingPath = "YValue2",
    ///    EnableTooltip = true
    /// };
    /// chart.Series.Add(series2);
    /// ]]>
    /// </code>
    /// ***
    /// 
    /// <para><b>Data Label</b></para>
    /// 
    /// <para>Data labels are used to display values related to a chart segment. To render the data labels, you need to enable the <see cref="ChartSeries.ShowDataLabels"/> property as <b>true</b> in <b>ChartSeries</b> class. </para>
    /// 
    /// <para>To customize the chart data labels alignment, placement and label styles, need to create an instance of <see cref="CartesianDataLabelSettings"/> and set to the <see cref="CartesianSeries.DataLabelSettings"/> property.</para>
    /// 
    /// # [MainPage.xaml](#tab/tabid-10)
    /// <code><![CDATA[
    /// <chart:SfCartesianChart>
    ///
    ///        <chart:SfCartesianChart.BindingContext>
    ///            <local:ViewModel/>
    ///        </chart:SfCartesianChart.BindingContext>
    ///
    ///        <chart:SfCartesianChart.XAxes>
    ///            <chart:NumericalAxis/>
    ///        </chart:SfCartesianChart.XAxes>
    ///
    ///        <chart:SfCartesianChart.YAxes>
    ///            <chart:NumericalAxis/>
    ///        </chart:SfCartesianChart.YAxes>
    ///
    ///        <chart:SfCartesianChart.Series>
    ///            <chart:ColumnSeries ShowDataLabels = "True" ItemsSource="{Binding Data}" XBindingPath="XValue" YBindingPath="YValue1"/>
    ///        </chart:SfCartesianChart.Series>
    /// </chart:SfCartesianChart>
    ///
    /// ]]>
    /// </code>
    /// # [MainPage.xaml.cs](#tab/tabid-11)
    /// <code><![CDATA[
    /// SfCartesianChart chart = new SfCartesianChart();
    /// 
    /// ViewModel viewModel = new ViewModel();
    ///	chart.BindingContext = viewModel;
    /// 
    /// NumericalAxis xAxis = new NumericalAxis();
    /// chart.XAxes.Add(xAxis);	
    /// 
    /// NumericalAxis yAxis = new NumericalAxis();
    /// chart.YAxes.Add(yAxis);
    /// 
    /// ColumnSeries series = new ColumnSeries()
    /// {
    ///     ItemsSource = viewModel.Data,
    ///     XBindingPath = "XValue",
    ///     YBindingPath = "YValue1",
    ///     ShowDataLabels = true
    /// };
    /// chart.Series.Add(series);
    /// ]]>
    /// </code>
    /// ***
    /// 
    /// <para><b>Zooming and Panning</b></para>
    /// 
    /// <para>SfCartesianChart allows you to zoom the chart area with the help of the zoom feature. This behavior is mostly used to view the data point in the specific area, when there are large number of data points inside the chart.</para>
    /// 
    /// <para>Zooming and panning provides you to take a close-up look of the data point plotted in the series. To enable the zooming and panning in the chart, create an instance of <see cref="ChartZoomPanBehavior"/> and set it to the <see cref="ZoomPanBehavior"/> property of SfCartesianChart.</para>
    /// 
    /// # [MainPage.xaml](#tab/tabid-12)
    /// <code><![CDATA[
    /// <chart:SfCartesianChart>
    ///
    ///        <chart:SfCartesianChart.BindingContext>
    ///            <local:ViewModel/>
    ///        </chart:SfCartesianChart.BindingContext>
    ///        
    ///        <chart:SfCartesianChart.ZoomPanBehavior>
    ///            <chart:ChartZoomPanBehavior EnablePanning = "True" EnableDoubleTap="True" EnablePinchZooming="True"/>
    ///        </chart:SfCartesianChart.ZoomPanBehavior>
    ///
    ///        <chart:SfCartesianChart.XAxes>
    ///            <chart:NumericalAxis/>
    ///        </chart:SfCartesianChart.XAxes>
    ///
    ///        <chart:SfCartesianChart.YAxes>
    ///            <chart:NumericalAxis/>
    ///        </chart:SfCartesianChart.YAxes>
    ///
    ///        <chart:SfCartesianChart.Series>
    ///             <chart:LineSeries ItemsSource="{Binding Data}" XBindingPath="XValue" YBindingPath="YValue1"/>
    ///             <chart:LineSeries ItemsSource="{Binding Data}" XBindingPath="XValue" YBindingPath="YValue2"/>
    ///         </chart:SfCartesianChart.Series>
    /// </chart:SfCartesianChart>
    ///
    /// ]]>
    /// </code>
    /// # [MainPage.xaml.cs](#tab/tabid-13)
    /// <code><![CDATA[
    /// SfCartesianChart chart = new SfCartesianChart();
    /// 
    /// ViewModel viewModel = new ViewModel();
    ///	chart.BindingContext = viewModel;
    ///	
    /// chart.ZoomPanBehavior = new ChartZoomPanBehavior() { EnablePinchZooming = true, EnableDoubleTap = true, EnablePanning = true };
    /// 
    /// NumericalAxis xAxis = new NumericalAxis();
    /// chart.XAxes.Add(xAxis);	
    /// 
    /// NumericalAxis yAxis = new NumericalAxis();
    /// chart.YAxes.Add(yAxis);
    /// 
    /// LineSeries series1 = new LineSeries()
    /// {
    ///    ItemsSource = viewModel.Data,
    ///    XBindingPath = "XValue",
    ///    YBindingPath = "YValue1",
    /// };
    /// chart.Series.Add(series1);
    ///
    /// LineSeries series2 = new LineSeries()
    /// {
    ///    ItemsSource = viewModel.Data,
    ///    XBindingPath = "XValue",
    ///    YBindingPath = "YValue2",
    /// };
    /// chart.Series.Add(series2);
    /// ]]>
    /// </code>
    /// ***
    /// </remarks>
    [ContentProperty(nameof(Series))]
    public partial class SfCartesianChart : ChartBase, ITouchListener, IDoubleTapGestureListener, ITapGestureListener, IPanGestureListener, IPinchGestureListener, ILongPressGestureListener, IParentThemeElement
    {
        #region Fields
        internal readonly CartesianChartArea _chartArea;
        internal readonly ChartTrackballView _trackballView;
        internal readonly ChartZoomPanView _zoomPanView;
        internal readonly AnnotationLayout _annotationLayout;

		readonly ObservableCollection<ChartAxis> _xAxes = [];
		readonly ObservableCollection<RangeAxisBase> _yAxes = [];

        /// <summary>
        /// Identifies the <see cref="TrackLineStroke"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The identifier for the <see cref="TrackLineStroke"/> bindable property specifies the stroke color of the 
        /// axis track line used for the selection zooming rectangle.
        /// </remarks>
        internal static readonly BindableProperty TrackLineStrokeProperty = BindableProperty.Create(
            nameof(TrackLineStroke),
            typeof(Brush),
            typeof(SfCartesianChart),
            new SolidColorBrush(Color.FromArgb("#DE000000")),
            BindingMode.Default,
            null,
            null);

        internal Brush TrackLineStroke
        {
            get { return (Brush)GetValue(TrackLineStrokeProperty); }
            set { SetValue(TrackLineStrokeProperty, value); }
        }

        #endregion

        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="Series"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The identifier for the <see cref="Series"/> bindable property determines the collection 
        /// of series in the cartesian chart.
        /// </remarks>
        public static readonly BindableProperty SeriesProperty = BindableProperty.Create(
            nameof(Series),
            typeof(ChartSeriesCollection),
            typeof(SfCartesianChart),
            null,
            BindingMode.Default,
            null,
            OnSeriesPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="EnableSideBySideSeriesPlacement"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The identifier for the <see cref="EnableSideBySideSeriesPlacement"/> bindable property determines
        /// whether side-by-side series placement is enabled.
        /// </remarks>
        public static readonly BindableProperty EnableSideBySideSeriesPlacementProperty = BindableProperty.Create(
            nameof(EnableSideBySideSeriesPlacement),
            typeof(bool),
            typeof(SfCartesianChart),
            true,
            BindingMode.Default,
            null,
            OnSideBySideSeriesPlacementChanged);

        /// <summary>
        /// Identifies the <see cref="IsTransposed"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The identifier for the <see cref="IsTransposed"/> bindable property determines whether the chart is transposed.
        /// </remarks>
        public static readonly BindableProperty IsTransposedProperty = BindableProperty.Create(
            nameof(IsTransposed),
            typeof(bool),
            typeof(SfCartesianChart),
            false,
            BindingMode.Default,
            null,
            OnTransposedPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="PaletteBrushes"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The identifier for the <see cref="PaletteBrushes"/> bindable property determines the 
        /// palette brushes used for the chart series.
        /// </remarks>
        public static readonly BindableProperty PaletteBrushesProperty = BindableProperty.Create(
            nameof(PaletteBrushes),
            typeof(IList<Brush>),
            typeof(SfCartesianChart),
            ChartColorModel.DefaultBrushes,
            BindingMode.Default,
            null,
            OnPaletteBrushesChanged);

        /// <summary>
        /// Identifies the <see cref="ZoomPanBehavior"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The identifier for the <see cref="ZoomPanBehavior"/> bindable property determines the 
        /// zoom and pan behavior of the chart.
        /// </remarks>
        public static readonly BindableProperty ZoomPanBehaviorProperty = BindableProperty.Create(
            nameof(ZoomPanBehavior),
            typeof(ChartZoomPanBehavior),
            typeof(SfCartesianChart),
            null,
            BindingMode.Default,
            null,
            OnZoomPanBehaviorPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="SelectionBehavior"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The identifier for the <see cref="SelectionBehavior"/> bindable property determines the 
        /// selection behavior of the chart.
        /// </remarks>
        public static readonly BindableProperty SelectionBehaviorProperty = BindableProperty.Create(
            nameof(SelectionBehavior),
            typeof(SeriesSelectionBehavior),
            typeof(SfCartesianChart),
            null,
            BindingMode.Default,
            null,
            OnSelectionBehaviorPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="TrackballBehavior"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The identifier for the <see cref="TrackballBehavior"/> bindable property determines the 
        /// trackball behavior of the chart.
        /// </remarks>
        public static readonly BindableProperty TrackballBehaviorProperty = BindableProperty.Create(
            nameof(TrackballBehavior),
            typeof(ChartTrackballBehavior),
            typeof(SfCartesianChart),
            null,
            BindingMode.Default,
            null,
            OnTrackballBehaviorPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="Annotations"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The identifier for the <see cref="Annotations"/> bindable property determines the 
        /// collection of annotations in the chart.
        /// </remarks>
        public static readonly BindableProperty AnnotationsProperty = BindableProperty.Create(
            nameof(Annotations),
            typeof(ChartAnnotationCollection),
            typeof(SfCartesianChart),
            null,
            BindingMode.Default,
            null,
            OnAnnotationsPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="CanAnnotationUnderPlotArea"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The identifier for the <see cref="CanAnnotationUnderPlotArea"/> bindable property determines whether 
        /// annotations can be placed under the plot area.
        /// </remarks>
        internal static readonly BindableProperty CanAnnotationUnderPlotAreaProperty = BindableProperty.Create(
            nameof(CanAnnotationUnderPlotArea),
            typeof(bool),
            typeof(SfCartesianChart),
            false,
            BindingMode.Default,
            null,
            OnAnnotationBehindSeries);

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a collection of chart series to be added in cartesian chart.
        /// </summary>
        /// <value>This property takes <see cref="ChartSeriesCollection"/> instance as value.</value>
        /// <remarks><para>To render a series, create an instance of required series class, and add it to the <see cref="Series"/> collection.</para></remarks>
        /// <example>
        /// # [Xaml](#tab/tabid-14)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///        <chart:SfCartesianChart.BindingContext>
        ///            <local:ViewModel/>
        ///        </chart:SfCartesianChart.BindingContext>
        ///
        ///        <chart:SfCartesianChart.XAxes>
        ///            <chart:NumericalAxis/>
        ///        </chart:SfCartesianChart.XAxes>
        ///
        ///        <chart:SfCartesianChart.YAxes>
        ///            <chart:NumericalAxis/>
        ///        </chart:SfCartesianChart.YAxes>
        ///
        ///        <chart:SfCartesianChart.Series>
        ///            <chart:LineSeries ItemsSource="{Binding Data}" XBindingPath="XValue" YBindingPath="YValue1"/>
        ///            <chart:LineSeries ItemsSource="{Binding Data}" XBindingPath="XValue" YBindingPath="YValue2"/>
        ///        </chart:SfCartesianChart.Series>  
        ///           
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-15)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     
        ///     ViewModel viewModel = new ViewModel();
        ///	    chart.BindingContext = viewModel;
        ///     
        ///     NumericalAxis xAxis = new NumericalAxis();
        ///     chart.XAxes.Add(xAxis);	
        ///     
        ///     NumericalAxis yAxis = new NumericalAxis();
        ///     chart.YAxes.Add(yAxis);
        ///     
        ///     LineSeries series1 = new LineSeries()
        ///     {
        ///         ItemsSource = viewModel.Data,
        ///         XBindingPath = "XValue",
        ///         YBindingPath = "YValue1"
        ///     };
        ///     chart.Series.Add(series1);
        ///     
        ///     LineSeries series2 = new LineSeries()
        ///     {
        ///         ItemsSource = viewModel.Data,
        ///         XBindingPath = "XValue",
        ///         YBindingPath = "YValue2"
        ///     };
        ///     chart.Series.Add(series2);
        ///     
        /// ]]></code>
        /// # [ViewModel](#tab/tabid-16)
        /// <code><![CDATA[
        /// public ObservableCollection<Model> Data { get; set; }
        /// 
        /// public ViewModel()
        /// {
        ///    Data = new ObservableCollection<Model>();
        ///    Data.Add(new Model() { XValue = 10, YValue1 = 100, YValue2 = 110 });
        ///    Data.Add(new Model() { XValue = 20, YValue1 = 150, YValue2 = 100 });
        ///    Data.Add(new Model() { XValue = 30, YValue1 = 110, YValue2 = 130 });
        ///    Data.Add(new Model() { XValue = 40, YValue1 = 230, YValue2 = 180 });
        /// }
        /// ]]></code>
        /// ***
        /// </example>
        public ChartSeriesCollection Series
        {
            get { return (ChartSeriesCollection)GetValue(SeriesProperty); }
            set { SetValue(SeriesProperty, value); }
        }

        /// <summary>
        /// Gets the collection of horizontal axes in the chart.
        /// </summary>
        /// <value>Returns the collection of <see cref="ChartAxis"/>.</value>
        /// <remarks>
        /// <b>Horizontal(X)</b> axis supports the <b>Category, Numeric and Date time</b>.
        /// </remarks>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-17)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        /// 
        ///         <chart:SfCartesianChart.BindingContext>
        ///             <local:ViewModel/>
        ///         </chart:SfCartesianChart.BindingContext>
        /// 
        ///         <chart:SfCartesianChart.XAxes>
        ///             <chart:NumericalAxis/>
        ///         </chart:SfCartesianChart.XAxes>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-18)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// 
        /// ViewModel viewModel = new ViewModel();
        ///	chart.BindingContext = viewModel;
        /// 
        /// NumericalAxis xaxis = new NumericalAxis();
        /// chart.XAxes.Add(xaxis);	
        /// 
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public ObservableCollection<ChartAxis> XAxes => _xAxes;

        /// <summary>
        /// Gets the collection of vertical axes in the chart.
        /// </summary>
        /// <value>Returns the collection of <see cref="RangeAxisBase"/>.</value>
        /// <remarks><b>Vertical(Y)</b> axis always uses numerical scale.</remarks>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-19)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        /// 
        ///         <chart:SfCartesianChart.BindingContext>
        ///             <local:ViewModel/>
        ///         </chart:SfCartesianChart.BindingContext>
        /// 
        ///         <chart:SfCartesianChart.YAxes>
        ///             <chart:NumericalAxis/>
        ///         </chart:SfCartesianChart.YAxes>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-20)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// 
        /// ViewModel viewModel = new ViewModel();
        ///	chart.BindingContext = viewModel;
        /// 
        /// NumericalAxis yAxis = new NumericalAxis();
        /// chart.YAxes.Add(yAxis);
        /// 
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public ObservableCollection<RangeAxisBase> YAxes => _yAxes;

        /// <summary>
        /// Gets or sets a <see cref="bool"/> value that indicates whether the series are placed side by side or overlapped.
        /// </summary>
        /// <value>This property takes the boolean value and its default value is <c>true</c>.</value>
        /// <remarks>If the value is true, series placed side by side, else series rendered one over other(overlapped).</remarks>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-25)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart EnableSideBySideSeriesPlacement = "True">
        ///           
        ///           <!-- ... Eliminated for simplicity-->
        ///           
        ///           <chart:SfCartesianChart.Series>
        ///                <chart:LineSeries ItemsSource="{Binding Data}" XBindingPath="XValue" YBindingPath="YValue"/>
        ///           </chart:SfCartesianChart.Series>
        ///           
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [MainPage.xaml.cs](#tab/tabid-26)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     chart.EnableSideBySideSeriesPlacement = true;
        ///     
        ///     ViewModel viewModel = new ViewModel();
        ///     chart.BindingContext = viewModel;
        ///
        ///     // Eliminated for simplicity
        ///     
        ///     LineSeries series = new LineSeries()
        ///     {
        ///        ItemsSource = viewModel.Data,
        ///        XBindingPath = "XValue",
        ///        YBindingPath = "YValue",
        ///     };
        ///     chart.Series.Add(series);
        ///     
        /// ]]></code>
        /// ***
        /// </example>
        public bool EnableSideBySideSeriesPlacement
        {
            get { return (bool)GetValue(EnableSideBySideSeriesPlacementProperty); }
            set { SetValue(EnableSideBySideSeriesPlacementProperty, value); }
        }

        /// <summary>
        /// Gets or sets a <see cref="bool"/> value that indicates whether to change the cartesian chart orientation.
        /// </summary>
        /// <value>This property takes the boolean value and its default value is <c>false</c>.</value>
        /// <remarks>If the value is true, the orientation of x-axis is set to vertical and orientation of y-axis is set to horizontal.</remarks>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-27)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart IsTransposed = "True">
        ///           
        ///           <!-- ... Eliminated for simplicity-->
        ///           
        ///           <chart:SfCartesianChart.Series>
        ///                <chart:LineSeries ItemsSource="{Binding Data}" XBindingPath="XValue" YBindingPath="YValue1"/>
        ///           </chart:SfCartesianChart.Series>
        ///           
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [MainPage.xaml.cs](#tab/tabid-28)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     chart.IsTransposed = true;
        ///     
        ///     ViewModel viewModel = new ViewModel();
        ///     chart.BindingContext = viewModel;
        ///
        ///     // Eliminated for simplicity
        ///     
        ///     LineSeries series = new LineSeries()
        ///     {
        ///        ItemsSource = viewModel.Data,
        ///        XBindingPath = "XValue",
        ///        YBindingPath = "YValue1",
        ///     };
        ///     chart.Series.Add(series);
        ///     
        /// ]]></code>
        /// ***
        /// </example>
        public bool IsTransposed
        {
            get { return (bool)GetValue(IsTransposedProperty); }
            set { SetValue(IsTransposedProperty, value); }
        }

        /// <summary>
        /// Gets or sets the palette brushes for chart.
        /// </summary>
        /// <value>This property takes the list of <see cref="Brush"/> and its default value is predefined palette.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-29)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart PaletteBrushes = "{Binding CustomBrushes}">
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:ColumnSeries ItemsSource="{Binding Data}" XBindingPath="XValue" YBindingPath="YValue1" />\
        ///          <chart:ColumnSeries ItemsSource="{Binding Data}" XBindingPath="XValue" YBindingPath="YValue2" />
        ///          <chart:ColumnSeries ItemsSource="{Binding Data}" XBindingPath="XValue" YBindingPath="YValue3" />
        ///
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-30)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///     List<Brush> CustomBrushes = new List<Brush>();
        ///     CustomBrushes.Add(new SolidColorBrush(Color.FromRgb(77, 208, 225)));
        ///     CustomBrushes.Add(new SolidColorBrush(Color.FromRgb(38, 198, 218)));
        ///     CustomBrushes.Add(new SolidColorBrush(Color.FromRgb(0, 188, 212)));
        ///     CustomBrushes.Add(new SolidColorBrush(Color.FromRgb(0, 172, 193)));
        ///     CustomBrushes.Add(new SolidColorBrush(Color.FromRgb(0, 151, 167)));
        ///     CustomBrushes.Add(new SolidColorBrush(Color.FromRgb(0, 131, 143)));
        ///
        ///     chart.PaletteBrushes = CustomBrushes;
        ///     // Eliminated for simplicity
        ///
        ///     ColumnSeries columnSeries1 = new ColumnSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue1
        ///     };
        ///     ColumnSeries columnSeries2 = new ColumnSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue2",
        ///     };
        ///     ColumnSeries columnSeries3 = new ColumnSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue3",
        ///     };
        ///     
        ///     chart.Series.Add(columnSeries1);
        ///     chart.Series.Add(columnSeries2);
        ///     chart.Series.Add(columnSeries3);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public IList<Brush> PaletteBrushes
        {
            get { return (IList<Brush>)GetValue(PaletteBrushesProperty); }
            set { SetValue(PaletteBrushesProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value for initiating the zooming and panning operations in chart.
        /// </summary>
        /// <value>This property takes the <see cref="ChartZoomPanBehavior"/> value and its default value is null.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-21)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///           
        ///           <chart:SfCartesianChart.BindingContext>
        ///               <local:ViewModel/>
        ///           </chart:SfCartesianChart.BindingContext>
        ///        
        ///           <chart:SfCartesianChart.ZoomPanBehavior>
        ///               <chart:ChartZoomPanBehavior EnableDoubleTap="True" EnablePinchZooming="True" EnablePanning="True"/>
        ///           </chart:SfCartesianChart.ZoomPanBehavior>
        ///           
        ///           <chart:SfCartesianChart.XAxes>
        ///               <chart:NumericalAxis/>
        ///           </chart:SfCartesianChart.XAxes>
        ///           
        ///           <chart:SfCartesianChart.YAxes>
        ///               <chart:NumericalAxis/>
        ///           </chart:SfCartesianChart.YAxes>
        ///           
        ///           <chart:SfCartesianChart.Series>
        ///                <chart:LineSeries ItemsSource="{Binding Data}" XBindingPath="XValue" YBindingPath="YValue"/>
        ///           </chart:SfCartesianChart.Series>
        ///           
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [MainPage.xaml.cs](#tab/tabid-22)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     
        ///     ViewModel viewModel = new ViewModel();
        ///	    chart.BindingContext = viewModel;
        ///	    
        ///     chart.ZoomPanBehavior = new ChartZoomPanBehavior() { EnableDoubleTap = true, EnablePinchZooming = true, EnablePanning = true };
        ///     
        ///     NumericalAxis xAxis = new NumericalAxis();
        ///     chart.XAxes.Add(xAxis);	
        ///     
        ///     NumericalAxis yAxis = new NumericalAxis();
        ///     chart.YAxes.Add(yAxis);
        ///     
        ///     LineSeries series = new LineSeries()
        ///     {
        ///        ItemsSource = viewModel.Data,
        ///        XBindingPath = "XValue",
        ///        YBindingPath = "YValue",
        ///     };
        ///     chart.Series.Add(series);
        ///     
        /// ]]></code>
        /// ***
        /// </example>
        public ChartZoomPanBehavior ZoomPanBehavior
        {
            get { return (ChartZoomPanBehavior)GetValue(ZoomPanBehaviorProperty); }
            set { SetValue(ZoomPanBehaviorProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value for initiating selection or highlighting of a single or multiple series in the chart.
        /// </summary>
        /// <value>This property takes a <see cref="SeriesSelectionBehavior"/> instance as a value, and its default value is null.</value>
        ///  <example>
        /// # [MainPage.xaml](#tab/tabid-23)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        ///         <chart:SfCartesianChart.XAxes>
        ///             <chart:DateTimeAxis/>
        ///         </chart:SfCartesianChart.XAxes>
        ///         <chart:SfCartesianChart.YAxes>
        ///             <chart:NumericalAxis/>
        ///         </chart:SfCartesianChart.YAxes>
        ///         <chart:SfCartesianChart.SelectionBehavior>
        ///             <chart:SeriesSelectionBehavior SelectionBrush = "Blue"/>
        ///         </chart:SfCartesianChart.SelectionBehavior>
        ///         <chart:ColumnSeries ItemsSource="{Binding Data}" XBindingPath="Date" YBindingPath="High"/>
        ///         <chart:ColumnSeries ItemsSource="{Binding Data}" XBindingPath="Date" YBindingPath="Low"/>
        /// </chart:SfCircularChart>
        /// ]]>
        /// </code>
        /// 
        /// # [MainPage.xaml.cs](#tab/tabid-24)
        /// <code><![CDATA[
        ///  SfCartesianChart chart = new SfCartesianChart();
        ///  
        ///  ViewModel viewModel = new ViewModel();
        ///	 chart.BindingContext = viewModel;
        /// 
        ///  chart.XAxes.Add(new DateTimeAxis())
        ///  chart.YAxes.Add(new NumericalAxis())
        /// 
        ///  chart.SelectionBehavior = new SeriesSelectionBehavior() {SelectionBrush = Colors.Blue};
        ///  
        ///  ColumnSeries series = new ColumnSeries()
        ///  {
        ///     ItemsSource = viewModel.Data,
        ///     XBindingPath = "Date",
        ///     YBindingPath = "High"
        ///  };
        /// 
        /// ColumnSeries series2 = new ColumnSeries()
        ///  {
        ///     ItemsSource = viewModel.Data,
        ///     XBindingPath = "Date",
        ///     YBindingPath = "Low"
        ///  };
        /// 
        ///  chart.Series.Add(series);
        ///  chart.Series.Add(series2);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public SeriesSelectionBehavior SelectionBehavior
        {
            get { return (SeriesSelectionBehavior)GetValue(SelectionBehaviorProperty); }
            set { SetValue(SelectionBehaviorProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value for initiating trackball, which displays the tooltip for the data points that are closer to the point where you interact on the chart area.
        /// </summary>
        /// <value>This property takes a<see cref= "ChartTrackballBehavior" /> instance as a value, and its default value is null.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-37)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///           <chart:SfCartesianChart.TrackballBehavior>
        ///               <chart:ChartTrackballBehavior/>
        ///           </chart:SfCartesianChart.TrackballBehavior>
        ///           
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-38)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     . . .
        ///     ChartTrackballBehavior trackballBehavior = new ChartTrackballBehavior();
        ///     chart.Add(trackballBehavior);
        ///     
        /// ]]></code>
        /// ***
        /// </example>
        public ChartTrackballBehavior TrackballBehavior
        {
            get { return (ChartTrackballBehavior)GetValue(TrackballBehaviorProperty); }
            set { SetValue(TrackballBehaviorProperty, value); }
        }

        /// <summary>
        /// Gets or sets a collection of annotations to the chart.
        /// </summary>
        /// <value>This property takes a<see cref= "ChartAnnotationCollection" /> instance as a value, and its default value is null.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-37)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///     <chart:SfCartesianChart.Annotations>
        ///          <chart:VerticalLineAnnotation X1="1"/>
        ///     </chart:SfCartesianChart.Annotations>  
        ///     
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-29)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();     
        ///
        ///     // Eliminated for simplicity
        ///     var verticalLineAnnotation = new VerticalLineAnnotation()
        ///     {
        ///         X1 = 1,
        ///     };
        ///  
        ///     chart.Annotations.Add(verticalLineAnnotation);
        /// ]]></code>
        /// ***
        /// </example>
        public ChartAnnotationCollection Annotations
        {
            get { return (ChartAnnotationCollection)GetValue(AnnotationsProperty); }
            set { SetValue(AnnotationsProperty, value); }
        }

        internal bool CanAnnotationUnderPlotArea
        {
            get { return (bool)GetValue(CanAnnotationUnderPlotAreaProperty); }
            set { SetValue(CanAnnotationUnderPlotAreaProperty, value); }
        }

		#region Event

		/// <summary>
		/// This event is raised when the trackball is moved from one data point to another. This helps to customize the trackball label and marker based on the condition.
		/// </summary>
		public event EventHandler<TrackballEventArgs> TrackballCreated;

        /// <summary>
        /// This event is triggered when zooming begins.
        /// </summary>
        public event EventHandler<ChartZoomStartEventArgs> ZoomStart;

        /// <summary>
        /// This event is triggered during the zooming process.
        /// </summary>
        public event EventHandler<ChartZoomDeltaEventArgs> ZoomDelta;

        /// <summary>
        /// This event is triggered when zooming concludes.
        /// </summary>
        public event EventHandler<ChartZoomEventArgs> ZoomEnd;

        /// <summary>
        /// This event is triggered when a selection zoom action begins.
        /// </summary>
        public event EventHandler<ChartSelectionZoomEventArgs> SelectionZoomStart;

        /// <summary>
        /// This event is triggered during the process of selection zooming.
        /// </summary>
        public event EventHandler<ChartSelectionZoomDeltaEventArgs> SelectionZoomDelta;

        /// <summary>
        /// This event is triggered when a selection zoom action concludes.
        /// </summary>
        public event EventHandler<ChartSelectionZoomEventArgs> SelectionZoomEnd;

        /// <summary>
        /// This event is triggered when panning the chart.
        /// </summary>
        public event EventHandler<ChartScrollEventArgs> Scroll;

        /// <summary>
        /// This event is triggered when reset the chart.
        /// </summary>
        public event EventHandler<ChartResetZoomEventArgs> ResetZoom;

		#endregion

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="SfCartesianChart"/> class.
		/// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
		public SfCartesianChart()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
		{
			ThemeElement.InitializeThemeResources(this, "SfCartesianChartTheme");
            SfCartesianChartResources.InitializeDefaultResource("Chart.Resources.SfCartesianChart", typeof(SfCartesianChart));
            SetDynamicResource(TrackLineStrokeProperty, "SfCartesianChartTrackballLineStroke");
            SetDynamicResource(TooltipBackgroundProperty, "SfCartesianChartTooltipBackground");
            SetDynamicResource(TooltipTextColorProperty, "SfCartesianChartTooltipTextColor");
            SetDynamicResource(TooltipFontSizeProperty, "SfCartesianChartTooltipTextFontSize");
            _chartArea = (CartesianChartArea)_legendLayout._areaBase;
            _annotationLayout = _chartArea._annotationLayout;
            Series = [];
            Annotations = [];
            this.AddTouchListener(this);
            this.AddGestureListener(this);
			
            _trackballView ??= [];

            _zoomPanView ??= [];

            UpdateView();
        }

        internal override AreaBase CreateChartArea()
        {
            return new CartesianChartArea(this);
        }

        #endregion

        #region Methods

        #region Theme Interface Implementation

        ResourceDictionary IParentThemeElement.GetThemeDictionary()
        {
            return new SfCartesianChartStyles();
        }

        void IThemeElement.OnControlThemeChanged(string oldTheme, string newTheme)
        {
        }

        void IThemeElement.OnCommonThemeChanged(string oldTheme, string newTheme)
        {
        }

        internal override void UpdateLegendItems()
        {
            if (_chartArea != null && _chartArea.PlotArea is ChartPlotArea plotArea)
            {
                plotArea.ShouldPopulateLegendItems = true;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Animates the visible series collection dynamically.
        /// </summary>
        public void AnimateSeries()
        {
            var visibleSeries = _chartArea.VisibleSeries;

            if (visibleSeries != null)
            {
                foreach (ChartSeries series in visibleSeries)
                {
                    series.Animate();
                }
            }
        }

        #endregion

        #region Protected Methods

        /// <inheritdoc/>
        /// <exclude/>
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            void UpdateBindingContext(object? target)
            {
                if (target is BindableObject bindableObject)
                {
                    SetInheritedBindingContext(bindableObject, BindingContext);
                }
            }

            if (Series != null)
            {
                foreach (var series in Series)
                {
                    UpdateBindingContext(series);
                }
            }

            UpdateBindingContext(ZoomPanBehavior);
            UpdateBindingContext(TrackballBehavior);
            UpdateBindingContext(SelectionBehavior);

            if (XAxes != null)
            {
                foreach (var axis in XAxes)
                {
                    UpdateBindingContext(axis);
                }
            }

            if (YAxes != null)
            {
                foreach (var axis in YAxes)
                {
                    UpdateBindingContext(axis);
                }
            }

            if (Annotations != null)
            {
                foreach (var annotation in Annotations)
                {
                    UpdateBindingContext(annotation);
                }
            }
        }

        /// <summary>
        /// Return a float value that representing the point value that corresponds to the given data value on the specified axis.
        /// </summary>
        /// <param name="axis">The ChartAxis along which the conversion is performed.</param>
        /// <param name="value">The data value to convert to a point coordinate.</param>
        /// <returns>A float representing the point coordinate in pixels.</returns>
        public float ValueToPoint(ChartAxis axis, double value)
        {
            var chart = this as IChart;
            var clipRect = chart?.ActualSeriesClipRect;

            if (axis != null && clipRect != null)
            {
                float offset = axis.IsVertical ? (float)clipRect.Value.Top : (float)clipRect.Value.Left;
                return axis.ValueToPoint(value) + offset;
            }

            return float.NaN;
        }

        /// <summary>
        /// Returns a double value representing the data value that corresponds to the given point on the specified axis.
        /// </summary>
        /// <param name="axis">The ChartAxis along which the conversion is performed.</param>
        /// <param name="x">The x-coordinate of the point in pixels.</param>
        /// <param name="y">The y-coordinate of the point in pixels.</param>
        /// <returns>A double representing the corresponding data value along the specified ChartAxis.</returns>
        public double PointToValue(ChartAxis axis, double x, double y)
        {
            var chart = this as IChart;
            var clipRect = chart?.ActualSeriesClipRect;

            if (axis != null && clipRect != null)
            {
                var xValue = x - (float)clipRect.Value.Left;
                var yValue = y - (float)clipRect.Value.Top;

                return axis.PointToValue(xValue, yValue);
            }

            return double.NaN;
        }

        #endregion

        #region Internal Override Methods

        internal override Brush? GetSelectionBrush(ChartSeries series)
        {
            if (SelectionBehavior != null)
            {
                var visibleSeries = _chartArea.VisibleSeries;
                if (visibleSeries != null && SelectionBehavior.Type != ChartSelectionType.None && visibleSeries.Contains(series))
                {
                    return SelectionBehavior.GetSelectionBrush(visibleSeries.IndexOf(series));
                }
            }

            return null;
        }

        internal void UpdateView()
        {
            if (_chartArea != null)
            {
                if (!_chartArea.Contains(_trackballView))
                {
                    _chartArea.Add(_trackballView);
				
					if (BehaviorLayout != null) 
					{
						BehaviorLayout.ZIndex = _chartArea.Count;
					}
                }

                if (!_chartArea.Contains(_zoomPanView))
                {
                   _chartArea.Add(_zoomPanView);
                }
            }
        }

        #region Interaction Overrides

        bool IGestureListener.IsTouchHandled
        {
            get { return IsHandled; }
        }

        internal bool IsHandled { get; set; }

        /// <inheritdoc/>
        void IPanGestureListener.OnPan(PanEventArgs e)
        {
            if (e.Status == GestureStatus.Running)
            {
                OnPanStateChanged(e.TouchPoint, e.TranslatePoint);
            }
            else if (e.Status == (GestureStatus.Canceled | GestureStatus.Completed))
            {
                OnPanEnded();
            }
        }

        /// <inheritdoc/>
        void IPinchGestureListener.OnPinch(PinchEventArgs e)
        {
            OnPinchStateChanged(e.Status, e.TouchPoint, e.Angle, e.Scale);
        }

        /// <inheritdoc/>
        void ITapGestureListener.OnTap(TapEventArgs e)
        {
            OnTapAction(this, e.TapPoint, e.TapCount);
        }

        /// <inheritdoc/>
        void IDoubleTapGestureListener.OnDoubleTap(TapEventArgs e)
        {
            OnTapAction(this, e.TapPoint, e.TapCount);
        }

        /// <inheritdoc/>
        void ITouchListener.OnTouch(PointerEventArgs e)
        {
            long pointerId = e.Id;
            Point point = e.TouchPoint;

            switch (e.Action)
            {
                case PointerActions.Pressed:
                    OnTouchDown(this, pointerId, point, e.PointerDeviceType);
                    break;
                case PointerActions.Moved:
                    OnTouchMove(this, point, e.PointerDeviceType);
                    break;
                case PointerActions.Released:
                    OnTouchUp(this, pointerId, point);
                    break;
                case PointerActions.Cancelled:
                    OnTouchCancel(pointerId, point);
                    break;
                case PointerActions.Exited:
                    OnTouchExit();
                    break;
            }
        }

        void ILongPressGestureListener.OnLongPress(LongPressEventArgs e)
        {
            Point point = e.TouchPoint;

            GestureStatus status = GestureStatus.Running;

#if IOS || MACCATALYST
            status = e.Status;
#endif
            OnLongPress(point.X, point.Y, status);
        }

        void OnLongPress(double x, double y, GestureStatus status)
        {
            TrackballBehavior?.OnLongPressActivation(this, (float)x, (float)y, status);
        }

        /// <inheritdoc/>
        void ITouchListener.OnScrollWheel(ScrollEventArgs e)
        {
            OnMouseWheelChanged(e);
        }

        #endregion

        internal void OnTouchDown(IChart chart, long pointerId, Point point, PointerDeviceType deviceType)
        {
            InteractiveBehavior?.OnTouchDown(this, (float)point.X, (float)point.Y);

            if (ZoomPanBehavior != null)
            {
                ZoomPanBehavior.SetTouchHandled(this);
                ZoomPanBehavior.OnTouchDown((float)point.X, (float)point.Y);
            }

            var tooltipBehavior = chart.ActualTooltipBehavior;
            if (tooltipBehavior != null)
            {
                tooltipBehavior.SetTouchHandled(this);
                tooltipBehavior.OnTouchDown(this, (float)point.X, (float)point.Y);
            }

            if (TrackballBehavior != null)
            {
                TrackballBehavior.DeviceType = deviceType;
                TrackballBehavior.OnTouchDown(this, (float)point.X, (float)point.Y);
            }
        }

        internal void OnTouchUp(IChart chart, long pointerId, Point point)
        {
#if MONOANDROID || WINDOWS
            IsHandled = false;
#endif

            OnPanEnded();
            InteractiveBehavior?.OnTouchUp(this, (float)point.X, (float)point.Y);

            ZoomPanBehavior?.OnTouchUp(this, (float)point.X, (float)point.Y);

            var tooltipBehavior = chart.ActualTooltipBehavior;
            //Updating the the TouchUp method from OnSingleTap method for custom tooltip behavior 
#if ANDROID || IOS
            tooltipBehavior?.OnTouchUp((float)point.X, (float)point.Y);
#else
            tooltipBehavior?.OnTouchUp(this, (float)point.X, (float)point.Y);
#endif
            TrackballBehavior?.OnTouchUp(this, (float)point.X, (float)point.Y);
        }

        internal void OnPinchStateChanged(GestureStatus action, Point location, double angle, float scale)
        {
            HideTooltipView();
            HideTrackballView();

            ZoomPanBehavior?.OnPinchStateChanged(this, action, location, angle, scale);
        }

        internal void OnPanStateChanged(Point touchPoint, Point translatePoint)
        {
#if WINDOWS
            if (TrackballBehavior != null && TrackballBehavior.LongPressActive)
			{
				return;
			}
#endif

#if WINDOWS || MACCATALYST
			HideTooltipView();
#endif

            if (TrackballBehavior != null && !TrackballBehavior.IsPressed)
			{
				HideTrackballView();
			}

			ZoomPanBehavior?.OnScrollChanged(this, touchPoint, translatePoint);
        }

        void OnPanEnded()
        {
            foreach (var axis in XAxes)
            {
                axis.IsScrolling = false;
            }

            foreach (var axis in YAxes)
            {
                axis.IsScrolling = false;
            }
        }

        internal void OnTapAction(IChart chart, Point tapPoint, int tapCount)
        {
            HideTooltipView();
            HideTrackballView();

            if (chart.ActualSeriesClipRect.Contains(tapPoint))
            {
                ZoomPanBehavior?.OnTapped(this, tapPoint, tapCount);

                if (SelectionBehavior != null && SelectionBehavior.Type != ChartSelectionType.None)
                {
                    SelectionBehavior.OnTapped((float)tapPoint.X, (float)tapPoint.Y);
                }
                else
                {
                    var visibleSeries = _chartArea.VisibleSeries;
                    if (visibleSeries != null)
                    {
                        foreach (var series in visibleSeries.Reverse())
                        {
                            if (series.SelectionHitTest((float)tapPoint.X, (float)tapPoint.Y))
							{
								break;
							}
						}
                    }
                }

                var tooltipBehavior = chart.ActualTooltipBehavior;
                tooltipBehavior?.OnTapped(this, tapPoint, tapCount);
            }
        }

        internal void OnTouchMove(IChart chart, Point point, PointerDeviceType deviceType)
        {
            InteractiveBehavior?.OnTouchMove(this, (float)point.X, (float)point.Y);

            var tooltipBehavior = chart.ActualTooltipBehavior;
            if (tooltipBehavior != null)
            {
                tooltipBehavior.DeviceType = deviceType;
                tooltipBehavior.OnTouchMove(this, (float)point.X, (float)point.Y);
            }

            if (TrackballBehavior != null)
            {
                TrackballBehavior.DeviceType = deviceType;
                TrackballBehavior.OnTouchMove(this, (float)point.X, (float)point.Y);
            }

            ZoomPanBehavior?.OnTouchMove(this, (float)point.X, (float)point.Y);
        }

        internal void OnTouchCancel(long pointerId, Point point)
        {
            TrackballBehavior?.OnTouchCancel((float)point.X, (float)point.Y);
        }

        void OnMouseWheelChanged(ScrollEventArgs e)
        {
            HideTrackballView();
            HideTooltipView();

            ZoomPanBehavior?.OnMouseWheelChanged(this, e.TouchPoint, e.ScrollDelta);
        }

        internal void OnTouchExit()
        {
            TrackballBehavior?.OnTouchExit();
        }

        internal void RaiseTrackballCreatedEvent(List<TrackballPointInfo> pointInfos)
        {
            if (TrackballCreated != null)
            {
                TrackballEventArgs arg = new(pointInfos);
                TrackballCreated.Invoke(this, arg);
            }
        }

        internal bool RaiseZoomStartEvent(ChartAxis axis)
        {
            if (ZoomStart != null)
            {
                ChartZoomStartEventArgs args = new(axis, axis.ZoomFactor, axis.ZoomPosition);
                ZoomStart.Invoke(this, args);
                return !args.Cancel;
            }

            return true;
        }

        internal bool RaiseZoomDeltaEvent(ChartAxis axis, double currentZoomFactor, double currentZoomPosition)
        {
            if (ZoomDelta != null)
            {
                ChartZoomDeltaEventArgs args = new(axis, currentZoomFactor, currentZoomPosition, axis.ZoomFactor, axis.ZoomPosition);
                ZoomDelta.Invoke(this, args);
                return !args.Cancel;
            }

            return true;
        }

        internal void RaiseZoomEndEvent(ChartAxis axis)
        {
            if (ZoomEnd != null)
            {
                ChartZoomEventArgs args = new(axis, axis.ZoomFactor, axis.ZoomPosition);
                ZoomEnd.Invoke(this, args);
            }
        }

        internal void RaiseSelectionZoomStartEvent(Rect selectionRect)
        {
            if (SelectionZoomStart != null)
            {
                ChartSelectionZoomEventArgs args = new(selectionRect);
                SelectionZoomStart.Invoke(this, args);
            }
        }

        internal bool RaiseSelectionZoomDeltaEvent(Rect selectionRect)
        {
            if (SelectionZoomDelta != null)
            {
                ChartSelectionZoomDeltaEventArgs args = new(selectionRect);
                SelectionZoomDelta.Invoke(this, args);
                return !args.Cancel;
            }

            return true;
        }

        internal void RaiseSelectionZoomEndEvent(Rect selectedRect)
        {
            if (SelectionZoomEnd != null)
            {
                ChartSelectionZoomEventArgs args = new(selectedRect);
                SelectionZoomEnd.Invoke(this, args);
            }
        }

        internal bool RaiseScrollEvent(ChartAxis axis, double currentPosition)
        {
            if (Scroll != null)
            {
                ChartScrollEventArgs args = new(axis, currentPosition);
                Scroll.Invoke(this, args);
                return !args.Cancel;
            }

            return true;
        }

        internal void RaiseResetZoomEvent(ChartAxis axis, double previousZoomFactor, double previousZoomPosition)
        {
            if (ResetZoom != null)
            {
                ChartResetZoomEventArgs args = new(axis, previousZoomFactor, previousZoomPosition);
                ResetZoom.Invoke(this, args);
            }
        }

        internal void UpdateAnnotation()
        {
            UpdateAnnotationLayout();
            ScheduleUpdateChart();
        }

        internal void UpdateAnnotationLayout()
        {
            foreach (var annotation in Annotations)
            {
                annotation.UpdateLayout();
            }
        }

        #endregion

        #region Property Callback Methods

        internal void HideTooltipView()
        {
            TooltipBehavior?.Hide();
        }

        void HideTrackballView()
        {
            TrackballBehavior?.Hide();
        }

        static void OnTransposedPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfCartesianChart chart)
            {
                chart._chartArea.IsTransposed = (bool)newValue;
            }
        }

        static void OnSeriesPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfCartesianChart chart)
            {
                chart._chartArea.Series = newValue as ChartSeriesCollection;
            }
        }

        static void OnSideBySideSeriesPlacementChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfCartesianChart chart)
            {
                chart._chartArea.EnableSideBySideSeriesPlacement = (bool)newValue;
            }
        }

        static void OnPaletteBrushesChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (Equals(oldValue, newValue))
            {
                return;
            }

            if (bindable is SfCartesianChart chart)
            {
                var area = chart._chartArea;
                //TODO: Need to ensure the behavior. 
                area.PaletteColors = (IList<Brush>)newValue ?? ChartColorModel.DefaultBrushes;
                chart.OnPaletteBrushesChanged(oldValue as ObservableCollection<Brush>, newValue as ObservableCollection<Brush>);

                if (area.AreaBounds != Rect.Zero)//Not to call at load time
				{
					area.OnPaletteColorChanged();
				}
				else if (area.PlotArea.LegendItems.Count > 0 && area.Series != null)
				{
					foreach (var series in area.Series)
					{
						series.UpdateLegendIconColor();
					}
				}
			}
        }

        static void OnZoomPanBehaviorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfCartesianChart chart)
            {
                if (newValue is ChartZoomPanBehavior zoomPan)
                {
                    zoomPan.Chart = chart;
                    SetInheritedBindingContext(zoomPan, chart.BindingContext);

                    var drawableView = chart._zoomPanView;
                    drawableView.Behavior = zoomPan;
                    AbsoluteLayout.SetLayoutBounds(drawableView, new Rect(0, 0, 1, 1));
                    AbsoluteLayout.SetLayoutFlags(drawableView, Microsoft.Maui.Layouts.AbsoluteLayoutFlags.All);
                    chart.UpdateView();
                }

                if (oldValue is ChartZoomPanBehavior oldZoomPan)
                {
                    chart._zoomPanView?.RemoveBinding(AbsoluteLayout.LayoutBoundsProperty);
                    chart._zoomPanView?.RemoveBinding(AbsoluteLayout.LayoutFlagsProperty);
                    chart._chartArea.Remove(chart._zoomPanView);
                    SetInheritedBindingContext(oldZoomPan, null);
                }
            }
        }

        static void OnSelectionBehaviorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfCartesianChart chart)
            {
                if (newValue is SeriesSelectionBehavior selection)
                {
                    selection.ChartArea = chart._chartArea;
                    selection.Chart = chart;
                    SetInheritedBindingContext(selection, chart.BindingContext);
                }

                if (oldValue is SeriesSelectionBehavior oldSelection)
                {
                    SetInheritedBindingContext(oldSelection, null);
                }
            }
        }

        static void OnTrackballBehaviorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfCartesianChart chart)
            {
                if (newValue is ChartTrackballBehavior trackball)
                {
                    trackball.CartesianChart = chart;
                    SetInheritedBindingContext(trackball, chart.BindingContext);

                    var drawableView = chart._trackballView;
                    drawableView.Behavior = trackball;
                    AbsoluteLayout.SetLayoutBounds(drawableView, new Rect(0, 0, 1, 1));
                    AbsoluteLayout.SetLayoutFlags(drawableView, Microsoft.Maui.Layouts.AbsoluteLayoutFlags.All);
                    chart.UpdateView();
                }

                if (oldValue is ChartTrackballBehavior oldTrackball)
                {
                    chart._trackballView?.RemoveBinding(AbsoluteLayout.LayoutBoundsProperty);
                    chart._trackballView?.RemoveBinding(AbsoluteLayout.LayoutFlagsProperty);
                    chart._chartArea.Remove(chart._trackballView);
                    SetInheritedBindingContext(oldTrackball, null);
                }

                SetParent((Element)oldValue, (Element)newValue, (Element)bindable);
            }
        }

        static void OnAnnotationsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfCartesianChart chart)
            {
                chart.OnAnnotationPropertyChanged(oldValue as ChartAnnotationCollection, newValue as ChartAnnotationCollection);
            }
        }

        #endregion

        #region Private Methods

        void OnPaletteBrushesChanged(ObservableCollection<Brush>? oldValue, ObservableCollection<Brush>? newValue)
        {
            if (oldValue != null)
            {
                oldValue.CollectionChanged -= PaletteBrushes_CollectionChanged;
            }

            if (newValue != null)
            {
                newValue.CollectionChanged += PaletteBrushes_CollectionChanged;
            }
        }

        void PaletteBrushes_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (sender is SfCartesianChart chart)
            {
                chart._chartArea.OnPaletteColorChanged();
            }
        }

        void OnAnnotationPropertyChanged(ChartAnnotationCollection? oldValue, ChartAnnotationCollection? newValue)
        {
            if (oldValue != null)
            {
                foreach (var annotation in oldValue)
                {
                    SetInheritedBindingContext(annotation, null);
                    ViewAnnotationDetachedToChart(annotation);
                    annotation.Chart = null;
                    annotation.Parent = null;
                }

                oldValue.CollectionChanged -= AnnotationCollection_CollectionChanged;
            }

            if (newValue != null)
            {
                newValue.CollectionChanged += AnnotationCollection_CollectionChanged;

                if (newValue != null)
                {
                    foreach (var annotation in newValue)
                    {
                        annotation.Parent = this;
                        annotation.Chart = this;
                        SetInheritedBindingContext(annotation, BindingContext);
                        ViewAnnotationAttachedToChart(annotation);
                    }
                }

                UpdateAnnotation();
            }
        }

        internal void ViewAnnotationAttachedToChart(ChartAnnotation annotation)
        {
            if (annotation is ViewAnnotation viewAnnotation && viewAnnotation.View is View view && viewAnnotation.IsVisible)
            {
                if (viewAnnotation.X1 != null || !double.IsNaN(viewAnnotation.Y1) && !_annotationLayout.Children.Contains(view))
                {
                    _annotationLayout.Children.Add(view);
                }
            }
        }

        internal void ViewAnnotationDetachedToChart(ChartAnnotation annotation)
        {
            if (annotation is ViewAnnotation viewAnnotation && viewAnnotation.View is View view)
			{
				if (_annotationLayout.Children.Contains(view))
                {
                    _annotationLayout.Children.Remove(view);
                }
			}
		}

        void AnnotationCollection_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            e.ApplyCollectionChanges(
                (obj, index, canInsert) => AddAnnotation(index, obj),
                (obj, index) => RemoveAnnotation(index, obj),
                ResetAnnotation);

            UpdateAnnotation();
        }

        void ScheduleUpdateChart()
        {
            if (_chartArea != null)
            {
                _chartArea.NeedsRelayout = true;
                _chartArea.ScheduleUpdateArea();
            }
        }

        void ResetAnnotation()
        {
        }

        void RemoveAnnotation(int index, object item)
        {
            if (item is ChartAnnotation annotation)
            {
                ViewAnnotationDetachedToChart(annotation);
                SetInheritedBindingContext(annotation, null);
                annotation.Chart = null;
                annotation.Parent = null;
            }
        }

        void AddAnnotation(int index, object item)
        {
            if (item is ChartAnnotation annotation)
            {
                annotation.Parent = this;
                annotation.Chart = this;
                SetInheritedBindingContext(annotation, BindingContext);
                ViewAnnotationAttachedToChart(annotation);
            }
        }

        internal void InvalidateAnnotation()
        {
            _annotationLayout.InvalidateDrawable();
        }

        static void OnAnnotationBehindSeries(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfCartesianChart chart)
            {
                if ((bool)newValue)
				{
					chart._annotationLayout.ZIndex = -1;
				}
				else
				{
					chart._annotationLayout.ZIndex = 0;
				}
			}
        }

        #endregion

        #endregion
    }
}
