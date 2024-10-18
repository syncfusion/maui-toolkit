using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Toolkit.Themes;
using Syncfusion.Maui.Toolkit.Internals;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Core = Syncfusion.Maui.Toolkit;

namespace Syncfusion.Maui.Toolkit.Charts
{
    /// <summary>
    /// Renders polar line and area charts for data representation and enhanced user interface visualization.
    /// </summary>
    /// <remarks>
    /// <para>Polar chart control visualizes the data in terms of values and angles.</para>
    ///
    /// <para>SfPolarChart class properties allows adding the series collection, allowing customization of the chart elements such as legend, data label, and tooltip features.</para>
    /// 
    /// <b>Axis</b>
    /// 
    /// <para><b>ChartAxis</b> is used to locate a data point inside the chart area. Charts typically have two axes that are used to measure and categorize data. 
    /// <b>SecondaryAxis(Y)</b> axis always uses numerical scale. <b>PrimaryAxis(X)</b> axis supports the <b>Category, Numeric, Date time and Logarithmic</b>.</para>
    /// 
    /// <para>To render an axis, the chart axis instance has to be added in chart’s <see cref="PrimaryAxis"/> and <see cref="SecondaryAxis"/> collection as per the following code snippet.</para>
    /// 
    /// # [MainPage.xaml](#tab/tabid-1)
    /// <code><![CDATA[
    /// <chart:SfPolarChart>
    /// 
    ///         <chart:SfPolarChart.BindingContext>
    ///             <local:ViewModel/>
    ///         </chart:SfPolarChart.BindingContext>
    /// 
    ///         <chart:SfPolarChart.PrimaryAxis>
    ///             <chart:NumericalAxis/>
    ///         </chart:SfPolarChart.PrimaryAxis>
    /// 
    ///         <chart:SfPolarChart.SecondaryAxis>
    ///             <chart:NumericalAxis/>
    ///         </chart:SfPolarChart.SecondaryAxis>
    /// 
    /// </chart:SfPolarChart>
    /// ]]>
    /// </code>
    /// # [MainPage.xaml.cs](#tab/tabid-2)
    /// <code><![CDATA[
    /// SfPolarChart chart = new SfPolarChart();
    /// 
    /// ViewModel viewModel = new ViewModel();
    ///	chart.BindingContext = viewModel;
    /// 
    /// NumericalAxis primaryAxis = new NumericalAxis();
    /// chart.PrimaryAxis = primaryAxis;	
    /// 
    /// NumericalAxis secondaryAxis = new NumericalAxis();
    /// chart.SecondaryAxis = secondaryAxis;
    /// 
    /// ]]>
    /// </code>
    /// ***
    /// 
    /// <para><b>Series</b></para>
    /// 
    /// <para>ChartSeries is the visual representation of data. SfPolarChart offers <see cref="PolarAreaSeries"/> and <see cref="PolarLineSeries"/>.</para>
    /// 
    /// <para>To render a series, create an instance of the required series class and add it to the <see cref="Series"/> collection.</para>
    /// 
    /// # [MainPage.xaml](#tab/tabid-3)
    /// <code> <![CDATA[
    /// <chart:SfPolarChart>
    ///
    ///        <chart:SfPolarChart.BindingContext>
    ///            <local:ViewModel/>
    ///        </chart:SfPolarChart.BindingContext>
    ///
    ///        <chart:SfPolarChart.PrimaryAxis>
    ///            <chart:NumericalAxis/>
    ///        </chart:SfPolarChart.PrimaryAxis>
    ///
    ///        <chart:SfPolarChart.SecondaryAxis>
    ///            <chart:NumericalAxis/>
    ///        </chart:SfPolarChart.SecondaryAxis>
    ///
    ///        <chart:PolarLineSeries ItemsSource = "{Binding Data}" XBindingPath="XValue" YBindingPath="YValue1"/>
    ///        <chart:PolarLineSeries ItemsSource = "{Binding Data}" XBindingPath="XValue" YBindingPath="YValue2"/>
    ///        
    /// </chart:SfPolarChart>
    /// ]]>
    /// </code>
    /// # [MainPage.xaml.cs](#tab/tabid-4)
    /// <code><![CDATA[
    /// SfPolarChart chart = new SfPolarChart();
    /// 
    /// ViewModel viewModel = new ViewModel();
    ///	chart.BindingContext = viewModel;
    /// 
    /// NumericalAxis primaryAxis = new NumericalAxis();
    /// chart.PrimaryAxis = primaryAxis;	
    /// 
    /// NumericalAxis secondaryAxis = new NumericalAxis();
    /// chart.SecondaryAxis = secondaryAxis;
    /// 
    /// PolarLineSeries series1 = new PolarLineSeries()
    /// {
    ///     ItemsSource = viewModel.Data,
    ///     XBindingPath = "XValue",
    ///     YBindingPath = "YValue1"
    /// };
    /// chart.Series.Add(series1);
    /// 
    /// PolarLineSeries series2 = new PolarLineSeries()
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
    ///    Data.Add(new Model() { XValue = A, YValue1 = 100, YValue2 = 110 });
    ///    Data.Add(new Model() { XValue = B, YValue1 = 150, YValue2 = 100 });
    ///    Data.Add(new Model() { XValue = C, YValue1 = 110, YValue2 = 130 });
    ///    Data.Add(new Model() { XValue = D, YValue1 = 230, YValue2 = 180 });
    /// }
    /// ]]>
    /// </code>
    /// ***
    /// 
    /// <para><b>Legend</b></para>
    /// 
    /// <para>The information provided in each legend item helps to identify the corresponding chart series. The Series <see cref="PolarSeries.Label"/> property text will be displayed in the associated legend item.</para>
    /// 
    /// <para>To render a legend, create an instance of <see cref="ChartLegend"/>, and assign it to the <see cref="ChartBase.Legend"/> property. </para>
    /// 
    /// # [MainPage.xaml](#tab/tabid-6)
    /// <code> <![CDATA[
    /// <chart:SfPolarChart>
    ///
    ///        <chart:SfPolarChart.BindingContext>
    ///            <local:ViewModel/>
    ///        </chart:SfPolarChart.BindingContext>
    ///        
    ///        <chart:SfPolarChart.Legend>
    ///            <chart:ChartLegend/>
    ///        </chart:SfPolarChart.Legend>
    ///
    ///        <chart:SfPolarChart.PrimaryAxis>
    ///            <chart:NumericalAxis/>
    ///        </chart:SfPolarChart.PrimaryAxis>
    ///
    ///        <chart:SfPolarChart.SecondaryAxis>
    ///            <chart:NumericalAxis/>
    ///        </chart:SfPolarChart.SecondaryAxis>
    ///
    ///        <chart:PolarLineSeries Label="Singapore" ItemsSource = "{Binding Data}" XBindingPath="XValue" YBindingPath="YValue1"/>
    ///        <chart:PolarLineSeries Label="Spain" ItemsSource = "{Binding Data}" XBindingPath="XValue" YBindingPath="YValue2"/>
    ///        
    /// </chart:SfPolarChart>
    /// ]]>
    /// </code>
    /// # [MainPage.xaml.cs](#tab/tabid-7)
    /// <code><![CDATA[
    /// SfPolarChart chart = new SfPolarChart();
    /// 
    /// ViewModel viewModel = new ViewModel();
    ///	chart.BindingContext = viewModel;
    ///	
    /// chart.Legend = new ChartLegend();
    /// 
    /// NumericalAxis primaryAxis = new NumericalAxis();
    /// chart.PrimaryAxis = primaryAxis;	
    /// 
    /// NumericalAxis secondaryAxis = new NumericalAxis();
    /// chart.SecondaryAxis = secondaryAxis;
    /// 
    /// PolarLineSeries series1 = new PolarLineSeries()
    /// {
    ///     ItemsSource = viewModel.Data,
    ///     XBindingPath = "XValue",
    ///     YBindingPath = "YValue1",
    ///     Label = "Singapore"
    /// };
    /// chart.Series.Add(series1);
    /// 
    /// PolarLineSeries series2 = new PolarLineSeries()
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
    /// <para>Tooltip displays information while tapping or mouse hovering on the segment. To display the tooltip on the chart, set the <see cref="ChartSeries.EnableTooltip"/> property as <b>true</b> in <see cref="ChartSeries"/>. </para>
    /// 
    /// <para>To customize the appearance of the tooltip elements like Background, TextColor and Font, create an instance of <see cref="ChartTooltipBehavior"/> class, modify the values, and assign it to the chart’s <see cref="ChartBase.TooltipBehavior"/> property.</para>
    /// 
    /// # [MainPage.xaml](#tab/tabid-8)
    /// <code><![CDATA[
    /// <chart:SfPolarChart>
    /// 
    ///         <chart:SfPolarChart.BindingContext>
    ///             <local:ViewModel/>
    ///         </chart:SfPolarChart.BindingContext>
    /// 
    ///         <chart:SfPolarChart.TooltipBehavior>
    ///             <chart:ChartTooltipBehavior/>
    ///         </chart:SfPolarChart.TooltipBehavior>
    /// 
    ///         <chart:SfPolarChart.PrimaryAxis>
    ///             <chart:NumericalAxis/>
    ///         </chart:SfPolarChart.PrimaryAxis>
    /// 
    ///         <chart:SfPolarChart.SecondaryAxis>
    ///             <chart:NumericalAxis/>
    ///         </chart:SfPolarChart.SecondaryAxis>
    /// 
    ///         <chart:PolarLineSeries EnableTooltip = "True" ItemsSource="{Binding Data}" XBindingPath="XValue" YBindingPath="YValue1"/>
    ///         <chart:PolarLineSeries EnableTooltip = "True" ItemsSource="{Binding Data}" XBindingPath="XValue" YBindingPath="YValue2"/>
    /// 
    /// </chart:SfPolarChart>
    /// ]]>
    /// </code>
    /// # [MainPage.xaml.cs](#tab/tabid-9)
    /// <code><![CDATA[
    /// SfPolarChart chart = new SfPolarChart();
    /// 
    /// ViewModel viewModel = new ViewModel();
    ///	chart.BindingContext = viewModel;
    ///
    /// chart.TooltipBehavior = new ChartTooltipBehavior();
    ///
    /// NumericalAxis primaryAxis = new NumericalAxis();
    /// chart.PrimaryAxis = primaryAxis;	
    ///
    /// NumericalAxis secondaryAxis = new NumericalAxis();
    /// chart.SecondaryAxis = secondaryAxis;
    ///
    /// PolarLineSeries series1 = new PolarLineSeries()
    /// {
    ///    ItemsSource = viewModel.Data,
    ///    XBindingPath = "XValue",
    ///    YBindingPath = "YValue1",
    ///    EnableTooltip = true
    /// };
    /// chart.Series.Add(series1);
    ///
    /// PolarLineSeries series2 = new PolarLineSeries()
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
    /// <para>Data labels are used to display values related to a chart segment. To render the data labels, enable the <see cref="ChartSeries.ShowDataLabels"/> property as <b>true</b> in <b>ChartSeries</b> class. </para>
    /// 
    /// <para>To customize the chart data labels alignment, placement, and label styles, create an instance of <see cref="PolarDataLabelSettings"/> and set it to the <see cref="PolarSeries.DataLabelSettings"/> property.</para>
    /// 
    /// # [MainPage.xaml](#tab/tabid-10)
    /// <code><![CDATA[
    /// <chart:SfPolarChart>
    ///
    ///        <chart:SfPolarChart.BindingContext>
    ///            <local:ViewModel/>
    ///        </chart:SfPolarChart.BindingContext>
    ///
    ///        <chart:SfPolarChart.PrimaryAxis>
    ///            <chart:NumericalAxis/>
    ///        </chart:SfPolarChart.PrimaryAxis>
    ///
    ///        <chart:SfPolarChart.SecondaryAxis>
    ///            <chart:NumericalAxis/>
    ///        </chart:SfPolarChart.SecondaryAxis>
    ///
    ///         <chart:PolarAreaSeries ShowDataLabels ="True" ItemsSource="{Binding Data}" XBindingPath="XValue" YBindingPath="YValue1"/>
    ///         
    /// </chart:SfPolarChart>
    ///
    /// ]]>
    /// </code>
    /// # [MainPage.xaml.cs](#tab/tabid-11)
    /// <code><![CDATA[
    /// SfPolarChart chart = new SfPolarChart();
    /// 
    /// ViewModel viewModel = new ViewModel();
    ///	chart.BindingContext = viewModel;
    /// 
    /// NumericalAxis primaryAxis = new NumericalAxis();
    /// chart.PrimaryAxis = primaryAxis;	
    /// 
    /// NumericalAxis secondaryAxis = new NumericalAxis();
    /// chart.SecondaryAxis = secondaryAxis;
    /// 
    /// PolarAreaSeries series = new PolarAreaSeries()
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
    /// </remarks>
    [ContentProperty(nameof(Series))]
    public class SfPolarChart : ChartBase, ITapGestureListener, ITouchListener, IParentThemeElement
    {
        #region Fields

        readonly PolarChartArea _chartArea;

        #endregion

        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="PrimaryAxis"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The identifier for the <see cref="PrimaryAxis"/> bindable property determines the primary axis of the polar chart.
        /// </remarks>
        public static readonly BindableProperty PrimaryAxisProperty = BindableProperty.Create(
            nameof(PrimaryAxis),
            typeof(ChartAxis),
            typeof(SfPolarChart),
            null,
            BindingMode.Default,
            null,
            OnPrimaryAxisChanged);

        /// <summary>
        /// Identifies the <see cref="SecondaryAxis"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The identifier for the <see cref="SecondaryAxis"/> bindable property determines the 
        /// secondary axis of the polar chart.
        /// </remarks>
        public static readonly BindableProperty SecondaryAxisProperty = BindableProperty.Create(
            nameof(SecondaryAxis),
            typeof(RangeAxisBase),
            typeof(SfPolarChart),
            null,
            BindingMode.Default,
            null,
            OnSecondaryAxisChanged);

        /// <summary>
        /// Identifies the <see cref="Series"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The identifier for the <see cref="Series"/> bindable property determines the collection 
        /// of series to be displayed in the polar chart.
        /// </remarks>
        public static readonly BindableProperty SeriesProperty = BindableProperty.Create(
            nameof(Series),
            typeof(ChartPolarSeriesCollection),
            typeof(SfPolarChart),
            null,
            BindingMode.Default,
            null,
            OnSeriesPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="PaletteBrushes"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The identifier for the <see cref="PaletteBrushes"/> bindable property determines the 
        /// palette brushes for the polar chart.
        /// </remarks>
        public static readonly BindableProperty PaletteBrushesProperty = BindableProperty.Create(
            nameof(PaletteBrushes),
            typeof(IList<Brush>),
            typeof(SfPolarChart),
            ChartColorModel.DefaultBrushes,
            BindingMode.Default,
            null,
            OnPaletteBrushesChanged);

        /// <summary>
        /// Identifies the <see cref="GridLineType"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The identifier for the <see cref="GridLineType"/> bindable property determines the 
        /// type of grid lines used in the polar chart.
        /// </remarks>
        public static readonly BindableProperty GridLineTypeProperty = BindableProperty.Create(
            nameof(GridLineType),
            typeof(PolarChartGridLineType),
            typeof(SfPolarChart),
            PolarChartGridLineType.Circle,
            BindingMode.Default,
            null,
            OnGridlineTypeChanged);

        /// <summary>
        /// Identifies the <see cref="StartAngle"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The identifier for the <see cref="StartAngle"/> bindable property determines the start angle of the polar chart.
        /// </remarks>
        public static readonly BindableProperty StartAngleProperty = BindableProperty.Create(
            nameof(StartAngle),
            typeof(ChartPolarAngle),
            typeof(SfPolarChart),
            ChartPolarAngle.Rotate270,
            BindingMode.Default,
            null,
            OnStartAnglePropertyChanged);

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the primary axis in the chart.
        /// </summary>
        /// <value>It accepts the <see cref="ChartAxis"/> value.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-12)
        /// <code><![CDATA[
        /// <chart:SfPolarChart>
        /// 
        ///         <chart:SfPolarChart.BindingContext>
        ///             <local:ViewModel/>
        ///         </chart:SfPolarChart.BindingContext>
        /// 
        ///         <chart:SfPolarChart.PrimaryAxis>
        ///             <chart:NumericalAxis/>
        ///         </chart:SfPolarChart.PrimaryAxis>
        /// 
        /// </chart:SfPolarChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-13)
        /// <code><![CDATA[
        /// SfPolarChart chart = new SfPolarChart();
        /// 
        /// ViewModel viewModel = new ViewModel();
        ///	chart.BindingContext = viewModel;
        /// 
        /// NumericalAxis primaryAxis = new NumericalAxis();
        /// chart.PrimaryAxis = primaryAxis;	
        /// 
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        /// <value>Returns the collection of <see cref="ChartAxis"/>.</value>
        public ChartAxis PrimaryAxis
        {
            get { return (ChartAxis)GetValue(PrimaryAxisProperty); }
            set { SetValue(PrimaryAxisProperty, value); }
        }

        /// <summary>
        /// Gets or sets the collection of vertical axis in the chart.
        /// </summary>
        ///  <value>It accepts the <see cref="RangeAxisBase"/> value.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-14)
        /// <code><![CDATA[
        /// <chart:SfPolarChart>
        /// 
        ///         <chart:SfPolarChart.BindingContext>
        ///             <local:ViewModel/>
        ///         </chart:SfPolarChart.BindingContext>
        /// 
        ///         <chart:SfPolarChart.SecondaryAxis>
        ///             <chart:NumericalAxis/>
        ///         </chart:SfPolarChart.SecondaryAxis>
        /// 
        /// </chart:SfPolarChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-15)
        /// <code><![CDATA[
        /// SfPolarChart chart = new SfPolarChart();
        /// 
        /// ViewModel viewModel = new ViewModel();
        ///	chart.BindingContext = viewModel;
        /// 
        /// NumericalAxis secondaryAxis = new NumericalAxis();
        /// chart.SecondaryAxis = secondaryAxis;
        /// 
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        /// <value>Returns the collection of <see cref="RangeAxisBase"/>.</value>
        public RangeAxisBase SecondaryAxis
        {
            get { return (RangeAxisBase)GetValue(SecondaryAxisProperty); }
            set { SetValue(SecondaryAxisProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value that can be used to modify the series starting angle.
        /// </summary>
        /// <value>It accepts the <see cref="ChartPolarAngle"/> value and it has the default value of <see cref="ChartPolarAngle.Rotate270"/>.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-16)
        /// <code><![CDATA[
        /// <chart:SfPolarChart StartAngle="Rotate90">
        /// 
        /// <!-- ... Eliminated for simplicity-->
        /// 
        /// <chart:PolarLineSeries ItemsSource="{Binding Data}" XBindingPath="XValue" YBindingPath="YValue1" />
        /// <chart:PolarLineSeries ItemsSource="{Binding Data}" XBindingPath="XValue" YBindingPath="YValue2" />
        /// 
        /// </chart:SfPolarChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-17)
        /// <code><![CDATA[
        /// SfPolarChart chart = new SfPolarChart();
        /// chart.StartAngle = ChartPolarAngle.Rotate90;
        /// 
        /// // Eliminated for simplicity
        /// 
        /// PolarLineSeries polarLineSeries1 = new PolarLineSeries()
        /// {
        ///     ItemsSource = viewModel.Data,
        ///     XBindingPath = "XValue",
        ///     YBindingPath = "YValue1
        /// };
        ///     PolarLineSeries polarLineSeries2 = new PolarLineSeries()
        /// {
        ///     ItemsSource = viewModel.Data,
        ///     XBindingPath = "XValue",
        ///     YBindingPath = "YValue2"
        /// };
        /// 
        ///   chart.Series.Add(polarLineSeries1);
        ///   chart.Series.Add(polarLineSeries2);
        /// 
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public ChartPolarAngle StartAngle
        {
            get { return (ChartPolarAngle)GetValue(StartAngleProperty); }
            set { SetValue(StartAngleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the palette brushes for the chart.
        /// </summary>
        /// <value>This property takes the list of <see cref="Brush"/>, and comes with a set of predefined brushes by default.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-18)
        /// <code><![CDATA[
        ///     <chart:SfPolarChart PaletteBrushes = "{Binding CustomBrushes}">
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:PolarLineSeries ItemsSource="{Binding Data}" XBindingPath="XValue" YBindingPath="YValue1" />
        ///          <chart:PolarLineSeries ItemsSource="{Binding Data}" XBindingPath="XValue" YBindingPath="YValue2" />
        ///          <chart:PolarLineSeries ItemsSource="{Binding Data}" XBindingPath="XValue" YBindingPath="YValue3" />
        ///
        ///     </chart:SfPolarChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-19)
        /// <code><![CDATA[
        ///     SfPolarChart chart = new SfPolarChart();
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
        ///     PolarLineSeries polarLineSeries1 = new PolarLineSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue1
        ///     };
        ///     PolarLineSeries polarLineSeries2 = new PolarLineSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue2"
        ///     };
        ///     PolarLineSeries polarLineSeries3 = new PolarLineSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue3"
        ///     };
        ///     
        ///     chart.Series.Add(polarLineSeries1);
        ///     chart.Series.Add(polarLineSeries2);
        ///     chart.Series.Add(polarLineSeries3);
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
        /// Gets or sets a collection of chart series to be added in Polar chart.
        /// </summary>
        /// <value>This property takes <see cref="ChartPolarSeriesCollection"/> instance as value.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-20)
        /// <code><![CDATA[
        ///     <chart:SfPolarChart>
        ///
        ///        <chart:SfPolarChart.BindingContext>
        ///            <local:ViewModel/>
        ///        </chart:SfPolarChart.BindingContext>
        ///
        ///        <chart:SfPolarChart.PrimaryAxis>
        ///            <chart:NumericalAxis/>
        ///        </chart:SfPolarChart.PrimaryAxis>
        ///
        ///        <chart:SfPolarChart.SecondaryAxis>
        ///            <chart:NumericalAxis/>
        ///        </chart:SfPolarChart.SecondaryAxis>
        ///         
        ///        <chart:SfPolarChart.Series>
        ///            <chart:PolarLineSeries ItemsSource="{Binding Data}" XBindingPath="XValue" YBindingPath="YValue1"/>
        ///            <chart:PolarLineSeries ItemsSource="{Binding Data}" XBindingPath="XValue" YBindingPath="YValue2"/>  
        ///        </chart:SfPolarChart.Series>
        ///        
        ///     </chart:SfPolarChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-21)
        /// <code><![CDATA[
        ///     SfPolarChart chart = new SfPolarChart();
        ///     
        ///     ViewModel viewModel = new ViewModel();
        ///	    chart.BindingContext = viewModel;
        ///     
        ///     NumericalAxis primaryAxis = new NumericalAxis();
        ///     chart.PrimaryAxis = primaryAxis;	
        ///     
        ///     NumericalAxis secondaryAxis = new NumericalAxis();
        ///     chart.SecondaryAxis = secondaryAxis;
        ///     
        ///     PolarLineSeries series1 = new PolarLineSeries()
        ///     {
        ///         ItemsSource = viewModel.Data,
        ///         XBindingPath = "XValue",
        ///         YBindingPath = "YValue1"
        ///     };
        ///     chart.Series.Add(series1);
        ///     
        ///     PolarLineSeries series2 = new PolarLineSeries()
        ///     {
        ///         ItemsSource = viewModel.Data,
        ///         XBindingPath = "XValue",
        ///         YBindingPath = "YValue2"
        ///     };
        ///     chart.Series.Add(series2);
        ///     
        /// ]]></code>
        /// # [ViewModel](#tab/tabid-22)
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
        /// <remarks><para>To render a series, create an instance of required series class, and add it to the <see cref="Series"/> collection.</para></remarks>
        public ChartPolarSeriesCollection Series
        {
            get { return (ChartPolarSeriesCollection)GetValue(SeriesProperty); }
            set { SetValue(SeriesProperty, value); }
        }

        /// <summary>
        /// Gets or sets the gridline type value that can be used to modify the polar chart grid line type to Polygon or Circle.
        /// </summary>
        /// <value>It accepts the <see cref="PolarChartGridLineType"/> value and its default value is <see cref="PolarChartGridLineType.Circle"/>.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-23)
        /// <code><![CDATA[
        /// <chart:SfPolarChart GridLineType="Polygon">
        /// 
        /// <!-- ... Eliminated for simplicity-->
        /// 
        ///  <chart:PolarLineSeries ItemsSource = "{Binding Data}" XBindingPath="XValue" YBindingPath="YValue"/>        
        /// 
        /// </chart:SfPolarChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-24)
        /// <code><![CDATA[
        /// SfPolarChart chart = new SfPolarChart();
        /// chart.GridLineType = PolarChartGridLineType.Polygon;
        /// 
        ///  // Eliminated for simplicity
        ///  
        /// ViewModel viewModel = new ViewModel();
        /// 
        /// PolarLineSeries polarLineSeries = new PolarLineSeries();
        /// polarLineSeries.ItemsSource = viewModel.Data;
        /// polarLineSeries.XBindingPath = "XValue";
        /// polarLineSeries.YBindingPath = "YValue";
        /// chart.Series.Add(polarLineSeries);
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public PolarChartGridLineType GridLineType
        {
            get { return (PolarChartGridLineType)GetValue(GridLineTypeProperty); }
            set { SetValue(GridLineTypeProperty, value); }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SfPolarChart"/> class.
        /// </summary>
        public SfPolarChart()
        {
            ThemeElement.InitializeThemeResources(this, "SfCartesianChartTheme");
            SetDynamicResource(TooltipBackgroundProperty, "SfPolarChartTooltipBackground");
            SetDynamicResource(TooltipTextColorProperty, "SfPolarChartTooltipTextColor");
            SetDynamicResource(TooltipFontSizeProperty, "SfPolarChartTooltipTextFontSize");
            _chartArea = (PolarChartArea)LegendLayout.AreaBase;
            Series = new ChartPolarSeriesCollection();
            this.AddTouchListener(this);
            this.AddGestureListener(this);
        }

        internal override AreaBase CreateChartArea()
        {
            return new PolarChartArea(this);
        }

        internal int PolarStartAngle
        {
            get { return _polarStartAngle; }
            set
            {
                _polarStartAngle = value;
            }
        }

        int _polarStartAngle = 270;

        #endregion

        #region Methods

        #region Interface Implementation

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

        /// <inheritdoc/>
        void ITapGestureListener.OnTap(TapEventArgs e)
        {
            OnTapAction(this, e.TapPoint, e.TapCount);
        }

        /// <inheritdoc/>
        void ITouchListener.OnTouch(Core.Internals.PointerEventArgs e)
        {
            Point point = e.TouchPoint;
            long pointerId = e.Id;

            switch (e.Action)
            {
                case PointerActions.Pressed:
                    OnTouchDown(this, pointerId, point);
                    break;
                case PointerActions.Released:
                    OnTouchUp(this, pointerId, point);
                    break;
                case PointerActions.Moved:
                    OnTouchMove(this, point, e.PointerDeviceType);
                    break;
            }
        }

        internal override void UpdateLegendItems()
        {
            if (_chartArea != null && !_chartArea.AreaBounds.IsEmpty)
            {
                _chartArea.ShouldPopulateLegendItems = true;
            }
        }

        #endregion

        #region Protected Method

        /// <inheritdoc/>
        /// <exclude/>
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (Series != null)
            {
                foreach (var series in Series)
                {
                    SetInheritedBindingContext(series, BindingContext);
                }
            }

            if (PrimaryAxis != null)
            {
                SetInheritedBindingContext(PrimaryAxis, BindingContext);
            }

            if (SecondaryAxis != null)
            {
                SetInheritedBindingContext(SecondaryAxis, BindingContext);
            }
        }

        #endregion

        #region Internal Method

        internal override TooltipInfo? GetTooltipInfo(ChartTooltipBehavior behavior, float x, float y)
        {
            var visibleSeries = (_chartArea as PolarChartArea).VisibleSeries;

            if (visibleSeries != null)
            {
                for (int i = visibleSeries.Count - 1; i >= 0; i--)
                {
                    ChartSeries chartSeries = visibleSeries[i];

                    if (!chartSeries.EnableTooltip || chartSeries.PointsCount <= 0)
                    {
                        continue;
                    }

                    SetDefaultTooltipValue(behavior);
                    TooltipInfo? tooltipInfo = chartSeries.GetTooltipInfo(behavior, x, y);

                    if (tooltipInfo != null)
                    {
                        return tooltipInfo;
                    }
                }
            }

            return null;
        }

        internal override void OnPlotAreaBackgroundChanged(object newValue)
        {
            if (_chartArea != null)
            {
                _chartArea.PlotAreaBackgroundView = (View)newValue;
            }
        }

        #endregion

        #region Private Method

        bool OnTapAction(IChart chart, Point tapPoint, int tapCount)
        {
            var tooltipBehavior = chart.ActualTooltipBehavior;

            tooltipBehavior?.OnTapped(this, tapPoint, tapCount);

            return false;
        }

        void OnTouchUp(IChart chart, long pointerId, Point point)
        {
            InteractiveBehavior?.OnTouchUp(this, (float)point.X, (float)point.Y);
            var tooltipBehavior = chart.ActualTooltipBehavior;
            tooltipBehavior?.OnTouchUp(this, (float)point.X, (float)point.Y);
        }

        void OnTouchDown(IChart chart, long pointerId, Point point)
        {
            InteractiveBehavior?.OnTouchDown(this, (float)point.X, (float)point.Y);
            var tooltipBehavior = chart.ActualTooltipBehavior;

            if (tooltipBehavior != null)
            {
                tooltipBehavior.SetTouchHandled(this);
                tooltipBehavior.OnTouchDown((float)point.X, (float)point.Y);
            }
        }

        void OnTouchMove(IChart chart, Point point, PointerDeviceType deviceType)
        {
            InteractiveBehavior?.OnTouchMove(this, (float)point.X, (float)point.Y);
            var tooltipBehavior = chart.ActualTooltipBehavior;

            if (tooltipBehavior != null)
            {
                tooltipBehavior.DeviceType = deviceType;
                tooltipBehavior.OnTouchMove(this, (float)point.X, (float)point.Y);
            }
        }

        static void OnPaletteBrushesChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (Equals(oldValue, newValue))
            {
                return;
            }

            if (bindable is SfPolarChart chart)
            {
                var area = chart._chartArea;
                area.PaletteColors = (IList<Brush>)newValue ?? ChartColorModel.DefaultBrushes;
                chart.OnPaletteBrushesChanged(oldValue as ObservableCollection<Brush>, newValue as ObservableCollection<Brush>);

                if (area.AreaBounds != Rect.Zero)
                {
                    area.OnPaletteColorChanged();
                }
            }
        }

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
            if (sender is SfPolarChart chart)
            {
                chart._chartArea.OnPaletteColorChanged();
            }
        }

        static void OnSeriesPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfPolarChart chart)
            {
                chart._chartArea.Series = newValue as ChartPolarSeriesCollection;
            }
        }

        static void OnPrimaryAxisChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfPolarChart chart)
            {
                if (oldValue is ChartAxis chartAxis)
                {
                    chartAxis.Parent = null;
                    SetInheritedBindingContext(chartAxis, null);
                }

                if (newValue is ChartAxis axis)
                {
                    axis.Parent = chart;
                    axis.PolarArea = chart._chartArea;
                    SetInheritedBindingContext(axis, chart.BindingContext);
                    chart.ScheduleUpdateArea();
                }
            }
        }

        static void OnSecondaryAxisChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfPolarChart chart)
            {
                if (oldValue is ChartAxis chartAxis)
                {
                    chartAxis.Parent = null;
                    SetInheritedBindingContext(chartAxis, null);
                    chart.ScheduleUpdateArea();
                }

                if (newValue is ChartAxis axis)
                {
                    axis.Parent = chart;
                    axis.PolarArea = chart._chartArea;
                    SetInheritedBindingContext(axis, chart.BindingContext);
                    chart.ScheduleUpdateArea();
                }
            }
        }

        static void OnGridlineTypeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfPolarChart chart)
            {
                chart._chartArea.InvalidateGridLayout();
            }
        }

        static void OnStartAnglePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfPolarChart chart)
            {
                if (newValue is ChartPolarAngle angle)
                {
                    int polarAngle = 270;
                    switch (angle)
                    {
                        case ChartPolarAngle.Rotate0:
                            polarAngle = 0;
                            break;
                        case ChartPolarAngle.Rotate180:
                            polarAngle = 180;
                            break;
                        case ChartPolarAngle.Rotate90:
                            polarAngle = 90;
                            break;
                    }

                    chart.PolarStartAngle = polarAngle;
                    chart.ScheduleUpdateArea();
                }
            }
        }

        void ScheduleUpdateArea()
        {
            _chartArea.ScheduleUpdate(true);
        }

        #endregion

        #endregion
    }
}