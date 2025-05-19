using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Microsoft.Maui.Layouts;
using Syncfusion.Maui.Toolkit.Graphics.Internals;
using Syncfusion.Maui.Toolkit.Internals;
using Syncfusion.Maui.Toolkit.Themes;
using Point = Microsoft.Maui.Graphics.Point;
using PointerEventArgs = Syncfusion.Maui.Toolkit.Internals.PointerEventArgs;

namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// Provides the funnel chart with a unique style of data representation that is more UI-visualising and user-friendly.
	/// </summary>
	/// <remarks>
	/// <para>The streamlined data is displayed using a funnel chart, where each slice of the funnel indicates a process that has filtered out data.</para>
	/// <para>SfFunnelChart class allows to customize the chart elements such as legend, data label, and tooltip features.</para>
	/// 
	///  # [MainPage.xaml](#tab/tabid-1)
	/// <code> <![CDATA[
	/// <chart:SfFunnelChart ItemsSource="{Binding Data}" 
	///                      XBindingPath="XValue" 
	///                      YBindingPath="YValue">
	///      <chart:SfFunnelChart.BindingContext>
	///          <model:ChartViewModel/>
	///      </chart:SfFunnelChart.BindingContext>
	/// </chart:SfFunnelChart>
	/// ]]>
	/// </code>
	/// # [MainPage.xaml.cs](#tab/tabid-2)
	/// <code><![CDATA[
	/// SfFunnelChart chart = new SfFunnelChart();
	/// ChartViewModel viewModel = new ChartViewModel();
	/// chart.ItemsSource = viewModel.Data;
	/// chart.XBindingPath = "XValue";
	/// chart.YBindingPath = "YValue";
	/// ]]>
	/// </code>
	/// # [ViewModel.cs](#tab/tabid-3)
	/// <code><![CDATA[
	/// public ObservableCollection<Model> Data { get; set; }
	/// 
	/// public ViewModel()
	/// {
	///    Data = new ObservableCollection<Model>();
	///    Data.Add(new Model() { XValue = "A", YValue = 18 });
	///    Data.Add(new Model() { XValue = "B", YValue = 34 });
	///    Data.Add(new Model() { XValue = "C", YValue = 52 });
	///    Data.Add(new Model() { XValue = "D", YValue = 68 });
	///    Data.Add(new Model() { XValue = "E", YValue = 100 });
	/// }
	/// ]]>
	/// </code>
	/// ***
	/// 
	/// <para><b>Legend</b></para>
	/// 
	/// <para>The Legend contains list of data points in chart. The information provided in each legend item helps to identify the corresponding data point in funnel chart. The chart <see cref="SfFunnelChart.XBindingPath"/> property value will be displayed in the associated legend item.</para>
	/// 
	/// <para>To render a legend, create an instance of <see cref="ChartLegend"/>, and assign it to the <see cref="ChartBase.Legend"/> property. </para>
	/// 
	/// # [MainPage.xaml](#tab/tabid-4)
	/// <code> <![CDATA[
	/// <chart:SfFunnelChart ItemsSource = "{Binding Data}"
	///                      XBindingPath="XValue" 
	///                      YBindingPath="YValue">
	///   <chart:SfFunnelChart.Legend>
	///        <chart:ChartLegend/>
	///   </chart:SfFunnelChart.Legend>
	/// </chart:SfFunnelChart>
	/// ]]>
	/// </code>
	/// # [MainPage.xaml.cs](#tab/tabid-5)
	/// <code><![CDATA[
	/// SfFunnelChart chart = new SfFunnelChart();
	/// chart.Legend = new ChartLegend();
	/// ChartViewModel viewModel = new ChartViewModel();
	/// chart.ItemsSource = viewModel.Data;
	/// chart.XBindingPath = "XValue";
	/// chart.YBindingPath = "YValue";
	/// ]]>
	/// </code>
	/// ***
	/// 
	/// <para><b>Tooltip</b></para>
	/// 
	/// <para>Tooltip displays information while tapping or mouse hover on the segment. To display the tooltip on the chart, you need to set the <see cref="SfFunnelChart.EnableTooltip"/> property as <b>true</b> in <see cref="SfFunnelChart"/>. </para>
	/// 
	/// <para>To customize the appearance of the tooltip elements like Background, TextColor and Font, create an instance of <see cref="ChartTooltipBehavior"/> class, modify the values, and assign it to the chart’s <see cref="ChartBase.TooltipBehavior"/> property. </para>
	/// 
	/// # [MainPage.xaml](#tab/tabid-6)
	/// <code><![CDATA[
	/// <chart:SfFunnelChart ItemsSource = "{Binding Data}"
	///                      XBindingPath="XValue" 
	///                      YBindingPath="YValue"
	///                      EnableTooltip="True">
	/// </chart:SfFunnelChart>
	/// ]]>
	/// </code>
	/// # [MainPage.xaml.cs](#tab/tabid-7)
	/// <code><![CDATA[
	/// SfFunnelChart chart = new SfFunnelChart();
	/// ChartViewModel viewModel = new ChartViewModel();
	/// chart.ItemsSource = viewModel.Data;
	/// chart.XBindingPath = "XValue";
	/// chart.YBindingPath = "YValue";
	/// chart.EnableTooltip=true;
	///
	/// ]]>
	/// </code>
	/// ***
	/// 
	/// <para><b>Data Label</b></para>
	/// 
	/// <para>Data labels are used to display values related to a chart segment. To render the data labels, you need to enable the <see cref="SfFunnelChart.ShowDataLabels"/> property as <b>true</b> in <see cref="SfFunnelChart"/> class. </para>
	/// 
	/// <para>To customize the chart data labels alignment, placement and label styles, need to create an instance of <see cref="FunnelDataLabelSettings"/> and set to the <see cref="SfFunnelChart.DataLabelSettings"/> property.</para>
	/// 
	/// # [MainPage.xaml](#tab/tabid-8)
	/// <code><![CDATA[
	/// <chart:SfFunnelChart ItemsSource="{Binding Data}" 
	///                      XBindingPath="XValue" 
	///                      YBindingPath="YValue"
	///                      ShowDataLabels="True">
	/// </chart:SfFunnelChart>
	/// ]]>
	/// </code>
	/// # [MainPage.xaml.cs](#tab/tabid-9)
	/// <code><![CDATA[
	/// SfFunnelChart chart = new SfFunnelChart();
	/// ChartViewModel viewModel = new ChartViewModel();
	/// chart.ItemsSource = viewModel.Data;
	/// chart.XBindingPath = "XValue";
	/// chart.YBindingPath = "YValue";
	/// chart.ShowDataLabels=true;
	/// ]]>
	/// </code>
	/// ***
	/// </remarks>
	public partial class SfFunnelChart : ChartBase, IPyramidChartDependent, IDrawCustomLegendIcon, ITapGestureListener, ITouchListener, IParentThemeElement
	{
		#region Private fields

		readonly ObservableCollection<ChartSegment> _segments;
		readonly PyramidChartArea _chartArea;
		readonly PyramidDataLabelHelper _dataLabelHelper;
		bool _segmentsCreated;
		ChartValueType _xValueType;

		#region ItemsSource related fields

		bool _isComplexYProperty;
		delegate object? GetReflectedProperty(object obj, string[] paths);
		IEnumerable? _actualXValues;
		string[][]? _yComplexPaths;
		int _pointsCount;
		List<object>? _actualData;
		bool _isLinearData = true;
		double _xData;
		string[]? _xComplexPaths;
		IEnumerable? _xValues;
		readonly IList<double> _yValues;
		internal Rect _seriesBounds;
		string[]? _yPaths;
		ObservableCollection<ChartDataLabel> _dataLabels;

		#endregion

		#endregion

		#region Bindable properties

		/// <summary>
		/// Identifies the <see cref="ItemsSource"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="ItemsSource"/> bindable property determines the 
		/// data source for the funnel chart.
		/// </remarks>
		public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
			nameof(ItemsSource),
			typeof(object),
			typeof(SfFunnelChart),
			null,
			BindingMode.Default,
			null,
			OnItemsSourceChanged);

		/// <summary>
		/// Identifies the <see cref="YBindingPath"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="YBindingPath"/> bindable property determines the binding path for the Y values.
		/// </remarks>
		public static readonly BindableProperty YBindingPathProperty = BindableProperty.Create(
			nameof(YBindingPath),
			typeof(string),
			typeof(SfFunnelChart),
			null,
			BindingMode.Default,
			null,
			OnYBindingPathChanged);

		/// <summary>
		/// Identifies the <see cref="XBindingPath"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="XBindingPath"/> bindable property determines the binding path for the X values.
		/// </remarks>
		public static readonly BindableProperty XBindingPathProperty = BindableProperty.Create(
			nameof(XBindingPath),
			typeof(string),
			typeof(SfFunnelChart),
			null,
			BindingMode.Default,
			null,
			OnXBindingPathChanged);

		/// <summary>
		/// Identifies the <see cref="PaletteBrushes"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="PaletteBrushes"/> bindable property determines the 
		/// palette brushes for the funnel chart.
		/// </remarks>
		public static readonly BindableProperty PaletteBrushesProperty = PyramidChartBase.PaletteBrushesProperty;

		/// <summary>
		/// Identifies the <see cref="Stroke"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="Stroke"/> bindable property determines the 
		/// stroke color for the funnel chart segments.
		/// </remarks>
		public static readonly BindableProperty StrokeProperty = PyramidChartBase.StrokeProperty;

		/// <summary>
		/// Identifies the <see cref="StrokeWidth"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="StrokeWidth"/> bindable property determines the 
		/// stroke width for the funnel chart segments.
		/// </remarks>
		public static readonly BindableProperty StrokeWidthProperty = PyramidChartBase.StrokeWidthProperty;

		/// <summary>
		/// Identifies the <see cref="LegendIcon"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="LegendIcon"/> bindable property determines the 
		/// legend icon type for the funnel chart.
		/// </remarks>
		public static readonly BindableProperty LegendIconProperty = PyramidChartBase.LegendIconProperty;

		/// <summary>
		/// Identifies the <see cref="TooltipTemplate"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="TooltipTemplate"/> bindable property determines the 
		/// template for the funnel chart tooltip.
		/// </remarks>
		public static readonly BindableProperty TooltipTemplateProperty = PyramidChartBase.TooltipTemplateProperty;

		/// <summary>
		/// Identifies the <see cref="EnableTooltip"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="EnableTooltip"/> bindable property determines 
		/// whether tooltips are enabled for the funnel chart.
		/// </remarks>
		public static readonly BindableProperty EnableTooltipProperty = PyramidChartBase.EnableTooltipProperty;

		/// <summary>
		/// Identifies the <see cref="SelectionBehavior"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="SelectionBehavior"/> bindable property determines the
		/// selection behavior for the funnel chart.
		/// </remarks>
		public static readonly BindableProperty SelectionBehaviorProperty = PyramidChartBase.SelectionBehaviorProperty;

		/// <summary>
		/// Identifies the <see cref="ShowDataLabels"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="ShowDataLabels"/> bindable property determines whether
		/// data labels are shown on the funnel chart.
		/// </remarks>
		public static readonly BindableProperty ShowDataLabelsProperty = PyramidChartBase.ShowDataLabelsProperty;

		/// <summary>
		/// Identifies the <see cref="LabelTemplate"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="LabelTemplate"/> bindable property determines the 
		/// template for the data labels.
		/// </remarks>
		public static readonly BindableProperty LabelTemplateProperty = BindableProperty.Create(
			nameof(LabelTemplate),
			typeof(DataTemplate),
			typeof(SfFunnelChart),
			null,
			BindingMode.Default,
			null,
			OnLabelTemplateChanged);

		/// <summary>
		/// Identifies the <see cref="DataLabelSettings"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="DataLabelSettings"/> bindable property determines the 
		/// customization options for the data labels.
		/// </remarks>
		public static readonly BindableProperty DataLabelSettingsProperty = BindableProperty.Create(
			nameof(DataLabelSettings),
			typeof(FunnelDataLabelSettings),
			typeof(SfFunnelChart),
			null,
			BindingMode.Default,
			null,
			OnDataLabelSettingsChanged);

		/// <summary>
		/// Identifies the <see cref="GapRatio"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="GapRatio"/> bindable property determines the 
		/// gap ratio between the segments of the funnel chart.
		/// </remarks>
		public static readonly BindableProperty GapRatioProperty = PyramidChartBase.GapRatioProperty;

		/// <summary>
		/// Identifies the <see cref="NeckWidth"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="NeckWidth"/> bindable property determines the neck width of the funnel chart.
		/// </remarks>
		internal static readonly BindableProperty NeckWidthProperty = BindableProperty.Create(
			nameof(NeckWidth),
			typeof(double),
			typeof(SfFunnelChart),
			40d,
			BindingMode.Default,
			null,
			NeckWidthChanged);

		/// <summary>
		/// Identifies the <see cref="NeckHeight"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="NeckHeight"/> bindable property determines the neck height of the funnel chart.
		/// </remarks>
		internal static readonly BindableProperty NeckHeightProperty = BindableProperty.Create(
			nameof(NeckHeight),
			typeof(double),
			typeof(SfFunnelChart),
			0d,
			BindingMode.Default,
			null,
			NeckHeightChanged);

		#endregion

		#region Constructor

		/// <summary>
		///  Initializes a new instance of the <see cref="SfFunnelChart"/> class.
		/// </summary>
		public SfFunnelChart()
		{
			ThemeElement.InitializeThemeResources(this, "SfFunnelChartTheme");
			SetDynamicResource(TooltipBackgroundProperty, "SfFunnelChartTooltipBackground");
			SetDynamicResource(TooltipTextColorProperty, "SfFunnelChartTooltipTextColor");
			SetDynamicResource(TooltipFontSizeProperty, "SfFunnelChartTooltipTextFontSize");
			DataLabelSettings = new FunnelDataLabelSettings();
			PaletteBrushes = ChartColorModel.DefaultBrushes;
			_chartArea = (PyramidChartArea)_legendLayout._areaBase;
			_dataLabelHelper = new PyramidDataLabelHelper(this);
			_yValues = [];
			_segments = [];
			_dataLabels = [];
			PyramidChartBase.InvokeSegmentsCollectionChanged(this, _segments);

			this.AddGestureListener(this);
			this.AddTouchListener(this);

			LabelTemplateView = new DataLabelLayout(this);
			AbsoluteLayout.SetLayoutBounds(LabelTemplateView, new Rect(0, 0, 1, 1));
			AbsoluteLayout.SetLayoutFlags(LabelTemplateView, AbsoluteLayoutFlags.All);
			_chartArea.Add(LabelTemplateView);
		}

		internal override AreaBase CreateChartArea()
		{
			return new PyramidChartArea(this);
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets a data points collection that will be used to plot a chart.
		/// </summary>
		/// <value>It accepts the data points collections and its default value is null.</value>
		/// <example>
		/// # [MainWindow.xaml](#tab/tabid-10)
		/// <code><![CDATA[
		/// <chart:SfFunnelChart ItemsSource="{Binding Data}" 
		///                      XBindingPath="XValue" 
		///                      YBindingPath="YValue">
		/// </chart:SfFunnelChart>
		/// ]]>
		/// </code>
		/// # [MainWindow.cs](#tab/tabid-11)
		/// <code><![CDATA[
		/// SfFunnelChart chart = new SfFunnelChart();
		///     
		/// ViewModel viewModel = new ViewModel();
		///
		/// chart.ItemsSource = viewModel.Data;
		/// chart.XBindingPath = "XValue";
		/// chart.YBindingPath = "YValue";
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public object ItemsSource
		{
			get { return (object)GetValue(ItemsSourceProperty); }
			set { SetValue(ItemsSourceProperty, value); }
		}

		/// <summary>
		/// Gets or sets a path value on the source object to serve a x value to the chart.
		/// </summary>
		/// <value>
		/// The string that represents the property name for the x plotting data, and its default value is null.
		/// </value>
		/// <example>
		/// # [MainWindow.xaml](#tab/tabid-12)
		/// <code><![CDATA[
		/// <chart:SfFunnelChart ItemsSource="{Binding Data}" 
		///                      XBindingPath="XValue" 
		///                      YBindingPath="YValue">
		/// </chart:SfFunnelChart>
		/// ]]>
		/// </code>
		/// # [MainWindow.cs](#tab/tabid-13)
		/// <code><![CDATA[
		/// SfFunnelChart chart = new SfFunnelChart();
		///
		/// ViewModel viewModel = new ViewModel();
		///
		/// chart.ItemsSource = viewModel.Data;
		/// chart.XBindingPath = "XValue";
		/// chart.YBindingPath = "YValue";
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public string XBindingPath
		{
			get { return (string)GetValue(XBindingPathProperty); }
			set { SetValue(XBindingPathProperty, value); }
		}

		/// <summary>
		/// Gets or sets a path value on the source object to serve a y value to the chart.
		/// </summary>
		/// <value>
		/// The string that represents the property name for the y plotting data, and its default value is null.
		/// </value>
		/// <example>
		/// # [MainWindow.xaml](#tab/tabid-14)
		/// <code><![CDATA[
		/// <chart:SfFunnelChart ItemsSource="{Binding Data}" 
		///                      XBindingPath="XValue" 
		///                      YBindingPath="YValue">
		/// </chart:SfFunnelChart>
		/// ]]>
		/// </code>
		/// # [MainWindow.cs](#tab/tabid-15)
		/// <code><![CDATA[
		/// SfFunnelChart chart = new SfFunnelChart();
		///
		/// ViewModel viewModel = new ViewModel();
		///
		/// chart.ItemsSource = viewModel.Data;
		/// chart.XBindingPath = "XValue";
		/// chart.YBindingPath = "YValue";
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public string YBindingPath
		{
			get { return (string)GetValue(YBindingPathProperty); }
			set { SetValue(YBindingPathProperty, value); }
		}

		/// <summary>
		/// Gets or sets the list of brushes that can be used to customize the appearance of the chart.
		/// </summary>
		/// <remarks>It allows custom brushes, and gradient brushes to customize the appearance.</remarks>
		/// <value>This property accepts a list of brushes as input and comes with a set of predefined brushes by default.</value>
		/// <example>
		/// # [MainPage.xaml](#tab/tabid-16)
		/// <code><![CDATA[
		/// <chart:SfFunnelChart ItemsSource="{Binding Data}"  
		///                      XBindingPath="Category"
		///                      YBindingPath="Value"
		///                      PaletteBrushes="{Binding CustomBrushes}">
		/// </chart:SfFunnelChart>
		/// ]]>
		/// </code>
		///
		/// # [MainPage.xaml.cs](#tab/tabid-17)
		/// <code><![CDATA[
		/// SfFunnelChart chart = new SfFunnelChart();
		/// ViewModel viewModel = new ViewModel();
		/// List<Brush> CustomBrushes = new List<Brush>();
		/// CustomBrushes.Add(new SolidColorBrush(Color.FromRgb(77, 208, 225)));
		/// CustomBrushes.Add(new SolidColorBrush(Color.FromRgb(38, 198, 218)));
		/// CustomBrushes.Add(new SolidColorBrush(Color.FromRgb(0, 188, 212)));
		/// CustomBrushes.Add(new SolidColorBrush(Color.FromRgb(0, 172, 193)));
		/// CustomBrushes.Add(new SolidColorBrush(Color.FromRgb(0, 151, 167)));
		/// CustomBrushes.Add(new SolidColorBrush(Color.FromRgb(0, 131, 143)));
		///
		/// chart.ItemsSource = viewModel.Data;
		/// chart.XBindingPath = "Category";
		/// chart.YBindingPath = "Value";
		/// chart.PaletteBrushes = CustomBrushes;
		/// 
		/// this.Content = chart;
		///
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public IList<Brush> PaletteBrushes
		{
			get { return (IList<Brush>)GetValue(PaletteBrushesProperty); }
			set { SetValue(PaletteBrushesProperty, value); }
		}

		/// <summary>
		/// Gets or sets the color used to paint the funnel segments' outline.
		/// </summary>
		/// <value>This property takes the <see cref="Brush"/> values and its default value is <c>Transparent</c>.</value>
		/// <example>
		/// # [MainPage.xaml](#tab/tabid-18)
		/// <code><![CDATA[
		/// <chart:SfFunnelChart ItemsSource="{Binding Data}"
		///                      XBindingPath="Category"
		///                      YBindingPath="Value"
		///                      Stroke="Red">
		/// </chart:SfFunnelChart>
		/// ]]>
		/// </code>
		///
		/// # [MainPage.xaml.cs](#tab/tabid-19)
		/// <code><![CDATA[
		/// SfFunnelChart chart = new SfFunnelChart();
		/// ViewModel viewModel = new ViewModel();
		/// chart.ItemsSource = viewModel.Data;
		/// chart.XBindingPath = "Category";
		/// chart.YBindingPath = "Value";
		/// chart.Stroke = Colors.Red;
		/// 
		/// this.Content = chart;
		///
		/// ]]>
		/// </code>
		/// ***
		/// </example> 
		public Brush Stroke
		{
			get { return (Brush)GetValue(StrokeProperty); }
			set { SetValue(StrokeProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value to specify the width of the stroke drawn.
		/// </summary>
		/// <value>This property takes the <see cref="double"/> values and its default value is 2.</value>
		/// <value>.</value>
		/// <example>
		/// # [MainPage.xaml](#tab/tabid-20)
		/// <code><![CDATA[
		/// <chart:SfFunnelChart ItemsSource="{Binding Data}"
		///                      XBindingPath="Category"
		///                      YBindingPath="Value"
		///                      Stroke="Red"
		///                      StrokeWidth="4">
		/// </chart:SfFunnelChart>
		/// ]]>
		/// </code>
		///
		/// # [MainPage.xaml.cs](#tab/tabid-21)
		/// <code><![CDATA[
		/// SfFunnelChart chart = new SfFunnelChart();
		/// ViewModel viewModel = new ViewModel();
		/// chart.ItemsSource = viewModel.Data;
		/// chart.XBindingPath = "Category";
		/// chart.YBindingPath = "Value";
		/// chart.Stroke = Colors.Red;
		/// chart.StrokeWidth = 4;
		/// 
		/// this.Content = chart;
		///
		/// ]]>
		/// </code>
		/// ***
		/// </example> 
		public double StrokeWidth
		{
			get { return (double)GetValue(StrokeWidthProperty); }
			set { SetValue(StrokeWidthProperty, value); }
		}

		/// <summary>
		/// Gets or sets a legend icon that will be displayed in the associated legend item.
		/// </summary>
		/// <value>This property takes the list of <see cref="ChartLegendIconType"/> and its default value is <see cref="ChartLegendIconType.Circle"/>.</value>
		/// <example>
		/// # [MainPage.xaml](#tab/tabid-22)
		/// <code><![CDATA[
		/// <chart:SfFunnelChart ItemsSource="{Binding Data}"
		///                      XBindingPath="Category"
		///                      YBindingPath="Value"
		///                      LegendIcon="Diamond">
		///    <chart:SfFunnelChart.Legend>
		///        <chart:ChartLegend />
		///    </chart:SfFunnelChart.Legend>
		/// </chart:SfFunnelChart>
		/// ]]>
		/// </code>
		///
		/// # [MainPage.xaml.cs](#tab/tabid-23)
		/// <code><![CDATA[
		/// SfFunnelChart chart = new SfFunnelChart();
		/// ViewModel viewModel = new ViewModel();
		/// chart.Legend = new ChartLegend();
		/// chart.ItemsSource = viewModel.Data;
		/// chart.XBindingPath = "Category";
		/// chart.YBindingPath = "Value";
		/// chart.LegendIcon = ChartLegendIconType.Diamond;
		/// 
		/// this.Content = chart;
		///
		/// ]]>
		/// </code>
		/// ***
		/// </example> 
		public ChartLegendIconType LegendIcon
		{
			get { return (ChartLegendIconType)GetValue(LegendIconProperty); }
			set { SetValue(LegendIconProperty, value); }
		}

		/// <summary>
		/// Gets or sets the DataTemplate that can be used to customize the appearance of the tooltip.
		/// </summary>
		/// <value>It accepts a <see cref="DataTemplate"/> value. and its default value is null.</value>
		/// <example>
		/// # [MainPage.xaml](#tab/tabid-24)
		/// <code><![CDATA[
		/// <chart:SfFunnelChart ItemsSource="{Binding Data}"
		///                      XBindingPath="Category"
		///                      YBindingPath="Value"
		///                      EnableTooltip="True">
		///    <chart:SfFunnelChart.TooltipTemplate>
		///        <DataTemplate>
		///            <Border Background = "DarkGreen"
		///                    StrokeThickness="2" Stroke="Black" >
		///            <Label Text = "{Binding Item.YValue}"
		///                   TextColor="White" FontAttributes="Bold"
		///                   HorizontalOptions="Center" VerticalOptions="Center"/>
		///            </Border>
		///        </DataTemplate>
		///    </chart:SfFunnelChart.TooltipTemplate>
		/// </chart:SfFunnelChart>
		/// ]]>
		/// </code>
		/// ***
		/// </example> 
		public DataTemplate TooltipTemplate
		{
			get { return (DataTemplate)GetValue(TooltipTemplateProperty); }
			set { SetValue(TooltipTemplateProperty, value); }
		}

		/// <summary>
		/// Gets or sets a Boolean value indicating whether the tooltip for chart should be shown or hidden.
		/// </summary>
		/// <value>This property takes the <c>bool</c> as its values and its default value is <c>False</c>.</value>
		/// <remarks>The tooltip will appear when you mouse over or tap on the funnel segments.</remarks>
		/// <example>
		/// # [MainPage.xaml](#tab/tabid-25)
		/// <code><![CDATA[
		/// <chart:SfFunnelChart ItemsSource="{Binding Data}"
		///                      XBindingPath="Category"
		///                      YBindingPath="Value"
		///                      EnableTooltip="True">
		/// </chart:SfFunnelChart>
		/// ]]>
		/// </code>
		///
		/// # [MainPage.xaml.cs](#tab/tabid-26)
		/// <code><![CDATA[
		/// SfFunnelChart chart = new SfFunnelChart();
		/// ViewModel viewModel = new ViewModel();
		/// chart.ItemsSource = viewModel.Data;
		/// chart.XBindingPath = "Category";
		/// chart.YBindingPath = "Value";
		/// chart.EnableTooltip= true;
		/// 
		/// this.Content = chart;
		///
		/// ]]>
		/// </code>
		/// ***
		/// </example> 
		public bool EnableTooltip
		{
			get { return (bool)GetValue(EnableTooltipProperty); }
			set { SetValue(EnableTooltipProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value for initiating selection or highlighting of a single or multiple data points in the chart.
		/// </summary>
		/// <value>This property takes a <see cref="DataPointSelectionBehavior"/> instance as a value, and its default value is null.</value>
		/// 
		/// <remarks>
		/// To highlight the selected segment, set the value for the <see cref="ChartSelectionBehavior.SelectionBrush"/> property in the <see cref="DataPointSelectionBehavior"/> class.
		/// </remarks>
		/// <example>
		/// # [MainPage.xaml](#tab/tabid-27)
		/// <code><![CDATA[
		/// <chart:SfFunnelChart ItemsSource = "{Binding Data}"
		///                      XBindingPath="XValue"
		///                      YBindingPath="YValue">
		///     <chart:SfFunnelChart.SelectionBehavior>
		///          <chart:DataPointSelectionBehavior SelectionBrush="Red" />
		///     </chart:SfFunnelChart.SelectionBehavior>
		///
		/// </chart:SfFunnelChart>
		/// ]]>
		/// </code>
		///
		/// # [MainPage.xaml.cs](#tab/tabid-28)
		/// <code><![CDATA[
		/// ViewModel viewModel = new ViewModel();
		/// 
		/// SfFunnelChart chart = new SfFunnelChart();
		/// chart.DataContext = viewModel;
		/// chart.ItemsSource = viewModel.Data;
		/// chart.XBindingPath = "XValue";
		/// chart.YBindingPath = "YValue";
		/// chart.SelectionBehavior = new DataPointSelectionBehavior()
		/// {
		///     SelectionBrush = new SolidColorBrush(Colors.Red),
		/// };
		///
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		/// <seealso cref="ChartSelectionBehavior.SelectedIndex"/>
		/// <seealso cref="ChartSelectionBehavior.SelectionBrush"/>
		/// <seealso cref="ChartSelectionBehavior.SelectionChanging"/>
		/// <seealso cref="ChartSelectionBehavior.SelectionChanged"/>
		public DataPointSelectionBehavior SelectionBehavior
		{
			get { return (DataPointSelectionBehavior)GetValue(SelectionBehaviorProperty); }
			set { SetValue(SelectionBehaviorProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value that indicates to enable the data labels for the chart.
		/// </summary>
		/// <value>This property takes the <c>bool</c> values and its default value is False.</value>
		/// <example>
		/// # [MainWindow.xaml](#tab/tabid-29)
		/// <code><![CDATA[
		/// <chart:SfFunnelChart ItemsSource="{Binding Data}" 
		///                      XBindingPath="XValue" 
		///                      YBindingPath="YValue"
		///                      ShowDataLabels="True">
		/// </chart:SfFunnelChart>
		/// ]]>
		/// </code>
		/// # [MainWindow.cs](#tab/tabid-30)
		/// <code><![CDATA[
		/// SfFunnelChart chart = new SfFunnelChart();
		///     
		/// ViewModel viewModel = new ViewModel();
		///
		/// chart.ItemsSource = viewModel.Data;
		/// chart.XBindingPath = "XValue";
		/// chart.YBindingPath = "YValue";
		/// chart.ShowDataLabels = true;
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public bool ShowDataLabels
		{
			get { return (bool)GetValue(ShowDataLabelsProperty); }
			set { SetValue(ShowDataLabelsProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value to customize the appearance of the displaying data labels in the chart.
		/// </summary>
		/// <remarks> This allows us to customize the appearance and position of data label.</remarks>
		/// <value>
		/// It takes the <see cref="FunnelDataLabelSettings"/>.
		/// </value>
		/// <example>
		/// # [MainWindow.xaml](#tab/tabid-31)
		/// <code><![CDATA[
		/// <chart:SfFunnelChart ItemsSource="{Binding Data}" 
		///                      XBindingPath="XValue" 
		///                      YBindingPath="YValue"
		///                      ShowDataLabels="True">
		///
		///     <chart:SfFunnelChart.DataLabelSettings>
		///         <chart:FunnelDataLabelSettings />
		///     </chart:SfFunnelChart.DataLabelSettings>
		///
		/// </chart:SfFunnelChart>
		/// ]]>
		/// </code>
		/// # [MainWindow.cs](#tab/tabid-32)
		/// <code><![CDATA[
		/// SfFunnelChart chart = new SfFunnelChart();
		///
		/// ViewModel viewModel = new ViewModel();
		///
		/// chart.ItemsSource = viewModel.Data;
		/// chart.XBindingPath = "XValue";
		/// chart.YBindingPath = "YValue";
		/// chart.ShowDataLabels = true;
		/// chart.DataLabelSettings = new FunnelDataLabelSettings();
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public FunnelDataLabelSettings DataLabelSettings
		{
			get { return (FunnelDataLabelSettings)GetValue(DataLabelSettingsProperty); }
			set { SetValue(DataLabelSettingsProperty, value); }
		}

		/// <summary>
		/// Gets or sets the ratio of the distance between the chart segments.
		/// </summary>
		/// <value>Its default value is 0. Its value ranges from 0 to 1.</value>
		/// <remarks>It is used to provide the spacing between the segments</remarks>
		/// <example>
		/// # [MainWindow.xaml](#tab/tabid-33)
		/// <code><![CDATA[
		/// <chart:SfFunnelChart ItemsSource="{Binding Data}" 
		///                      XBindingPath="XValue" 
		///                      YBindingPath="YValue"
		///                      GapRatio="0.3">
		/// </chart:SfFunnelChart>
		/// ]]>
		/// </code>
		/// # [MainWindow.cs](#tab/tabid-34)
		/// <code><![CDATA[
		/// SfFunnelChart chart = new SfFunnelChart();
		///
		/// ViewModel viewModel = new ViewModel();
		///
		/// chart.ItemsSource = viewModel.Data;
		/// chart.XBindingPath = "XValue";
		/// chart.YBindingPath = "YValue";
		/// chart.GapRatio = 0.3;
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public double GapRatio
		{
			get { return (double)GetValue(GapRatioProperty); }
			set { SetValue(GapRatioProperty, value); }
		}

		/// <summary>
		/// Gets or sets the DataTemplate that can be used to customize the appearance of the Data label.
		/// </summary>
		/// /// <value>
		/// It accepts the <see cref="DataTemplate"/> value.
		/// </value>
		/// <example>
		/// # [MainWindow.xaml](#tab/tabid-35)
		/// <code><![CDATA[
		/// <chart:SfFunnelChart ItemsSource="{Binding Data}" 
		///                      XBindingPath="XValue" 
		///                      YBindingPath="YValue"
		///                      ShowDataLabels="True">
		///   <chart:SfFunnelChart.LabelTemplate>
		///        <DataTemplate>
		///            <VerticalStackLayout>
		///                     <Image Source="image.png" 
		///                            WidthRequest="20" 
		///                            HeightRequest="20"/>
		///                     <Label Text="{Binding Item.YValue}" 
		///                            TextColor="Black"
		///                            FontAttributes="Bold"
		///                            FontSize="12"/>
		///             </VerticalStackLayout>
		///        </DataTemplate>
		///    </chart:SfFunnelChart.LabelTemplate>
		/// </chart:SfFunnelChart>
		/// ]]>
		/// </code>
		/// # [MainWindow.cs](#tab/tabid-36)
		/// <code><![CDATA[
		/// SfFunnelChart chart = new SfFunnelChart();
		///
		/// chart.ItemsSource = new ViewModel().Data;
		/// chart.XBindingPath = "XValue";
		/// chart.YBindingPath = "YValue";
		/// chart.ShowDataLabels = true;
		/// 
		/// DataTemplate labelTemplate = new DataTemplate(()=>
		/// {
		///     VerticalStackLayout layout = new VerticalStackLayout();
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
		///     label.SetBinding(Label.TextProperty, new Binding("YValue"));
		///     layout.Children.Add(image);
		///     layout.Children.Add(label);
		///     return layout;
		/// }    
		/// 
		/// chart.LabelTemplate = labelTemplate;
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public DataTemplate LabelTemplate
		{
			get { return (DataTemplate)GetValue(LabelTemplateProperty); }
			set { SetValue(LabelTemplateProperty, value); }
		}

		internal double NeckWidth
		{
			get { return (double)GetValue(NeckWidthProperty); }
			set { SetValue(NeckWidthProperty, value); }
		}

		internal double NeckHeight
		{
			get { return (double)GetValue(NeckHeightProperty); }
			set { SetValue(NeckHeightProperty, value); }
		}

		internal ObservableCollection<ChartDataLabel> DataLabels
		{
			get { return _dataLabels; }
			set
			{
				_dataLabels = value;
				OnPropertyChanged(nameof(DataLabels));
			}
		}

		internal DataLabelLayout? LabelTemplateView { get; set; }

		#endregion

		#region Methods

		#region Protected Methods

		/// <inheritdoc/>
		/// <exclude/>
		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();

			if (DataLabelSettings != null)
			{
				SetInheritedBindingContext(DataLabelSettings, BindingContext);
			}

			if (SelectionBehavior != null)
			{
				SetInheritedBindingContext(SelectionBehavior, BindingContext);
			}
		}

		#endregion

		#region Interface Implementation

		ResourceDictionary IParentThemeElement.GetThemeDictionary()
		{
			return new SfFunnelChartStyles();
		}

		void IThemeElement.OnControlThemeChanged(string oldTheme, string newTheme)
		{
		}

		void IThemeElement.OnCommonThemeChanged(string oldTheme, string newTheme)
		{
		}

		//GestureListener interface implementation
		void ITapGestureListener.OnTap(TapEventArgs e)
		{
			OnTapAction(this, e.TapPoint, e.TapCount);
		}

		void ITouchListener.OnTouch(PointerEventArgs e)
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

		void IDrawCustomLegendIcon.DrawSeriesLegend(ICanvas canvas, RectF rect, Brush fillColor, bool isSaveState)
		{
			if (isSaveState)
			{
				canvas.CanvasSaveState();
			}

			var path = new PathF();
			path.MoveTo((float)7.63636, 9);
			path.LineTo(6, 12);
			path.LineTo((float)4.36364, 9);
			path.Close();
			path.MoveTo((float)8.18182, 8);
			path.LineTo((float)3.81818, 8);
			path.LineTo((float)2.18182, 5);
			path.LineTo((float)9.81818, 5);
			path.Close();
			path.MoveTo((float)10.3636, 4);
			path.LineTo((float)1.63636, 4);
			path.LineTo(0, 1);
			path.LineTo(12, 1);
			path.Close();
			canvas.FillPath(path);

			if (isSaveState)
			{
				canvas.CanvasRestoreState();
			}

		}

		internal override void UpdateLegendItems()
		{
			if (_chartArea != null && !_chartArea.AreaBounds.IsEmpty)
			{
				_chartArea.ShouldPopulateLegendItems = true;
			}
		}

		//IDatapointSelectionDependent interface implementation
		ObservableCollection<ChartSegment> IDatapointSelectionDependent.Segments => _segments;

		Rect IDatapointSelectionDependent.AreaBounds => this is IChart chart ? chart.ActualSeriesClipRect : Rect.Zero;

		//IPyramidChartDependent  interface implementation

		IPyramidDataLabelSettings IPyramidChartDependent.DataLabelSettings => DataLabelSettings ?? new FunnelDataLabelSettings();

		bool IPyramidChartDependent.ArrangeReverse => true;

		PyramidDataLabelHelper IPyramidChartDependent.DataLabelHelper => _dataLabelHelper;

		AreaBase IPyramidChartDependent.Area => _chartArea;

		ChartLegend? IPyramidChartDependent.ChartLegend => Legend;

		Rect IPyramidChartDependent.SeriesBounds
		{
			get { return _seriesBounds; }

			set
			{
				_seriesBounds = value;
			}
		}

		bool IPyramidChartDependent.SegmentsCreated
		{
			get { return _segmentsCreated; }

			set
			{
				_segmentsCreated = value;
			}
		}

		ObservableCollection<ChartDataLabel> IDataTemplateDependent.DataLabels => DataLabels;

		DataTemplate IDataTemplateDependent.LabelTemplate => LabelTemplate;

		DataLabelLayout? IPyramidChartDependent.LabelTemplateView => LabelTemplateView;

		void IPyramidChartDependent.UpdateLegendItemsSource(ObservableCollection<ILegendItem> legendItems)
		{
			if (Legend == null)
			{
				return;
			}

			legendItems.Clear();

			for (int i = _pointsCount - 1; i >= 0; i--)
			{
				var legendItem = new LegendItem
				{
					IconType = ChartUtils.GetShapeType(LegendIcon)
				};
				Brush? solidColor = PyramidChartBase.GetFillColor(this, i);
				legendItem.IconBrush = solidColor ?? new SolidColorBrush(Colors.Transparent);
				legendItem.Text = GetActualXValue(i)?.ToString() ?? string.Empty;
				legendItem.Index = i;
				legendItem.Item = _actualData?[i];
				legendItem.Source = this;
				((IChartPlotArea)_chartArea).UpdateLegendLabelStyle(legendItem, Legend.LabelStyle ?? LegendStyle);
				((IChartLegend)Legend).OnLegendItemCreated(legendItem);
				legendItems.Add(legendItem);
			}
		}

		void IPyramidChartDependent.GenerateSegments()
		{
			double total = CalculateTotalValue();
			CalculateSegment(total, _yValues);
		}

		void IPyramidChartDependent.OnSelectionBehaviorPropertyChanged(object oldValue, object newValue)
		{
			if (oldValue is DataPointSelectionBehavior selectionBehavior)
			{
				SetInheritedBindingContext(selectionBehavior, null);
			}

			if (newValue is DataPointSelectionBehavior selection)
			{
				selection.Source = this;
				SetInheritedBindingContext(selection, BindingContext);
			}
		}

		#endregion

		#region Internal override methods

		internal override TooltipInfo? GetTooltipInfo(ChartTooltipBehavior behavior, float x, float y)
		{
			if (EnableTooltip)
			{
				int index = this.GetDataPointIndex(x, y);

				if (index < 0 || _actualData == null || _yValues == null)
				{
					return null;
				}

				object dataPoint = _actualData[index];
				double yValue = _yValues[index];
				var segment = (FunnelSegment)_segments[index];

				TooltipInfo tooltipInfo = new TooltipInfo(this)
				{
					X = segment.SegmentBounds.Center.X + (float)_seriesBounds.Left,
					Y = segment.SegmentBounds.Center.Y + (float)_seriesBounds.Top,
					Index = index,
					Margin = behavior.Margin,
					TextColor = behavior.TextColor ?? TooltipTextColor ?? Color.FromArgb("#F4EFF4"),
					FontFamily = behavior.FontFamily,
					FontSize = !float.IsNaN(behavior.FontSize) ? behavior.FontSize : !float.IsNaN((float)TooltipFontSize) ? (float)TooltipFontSize : 14.0f,
					FontAttributes = behavior.FontAttributes,
					Background = behavior.Background ?? TooltipBackground ?? new SolidColorBrush(Color.FromArgb("#1C1B1F")),
					Text = yValue.ToString("#.##"),
					Item = dataPoint
				};

				return tooltipInfo;
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

		#region private Static property changed methods

		static void OnXBindingPathChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfFunnelChart chart)
			{
				if (newValue != null && newValue is string v)
				{
					chart._xComplexPaths = v.Split(['.']);
				}

				chart.OnBindingPathChanged();
			}
		}

		static void OnYBindingPathChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfFunnelChart chart)
			{
				chart.OnBindingPathChanged();
			}
		}

		static void NeckWidthChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is IPyramidChartDependent chart)
			{
				chart.ScheduleUpdateChart();
			}
		}

		static void NeckHeightChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is IPyramidChartDependent chart)
			{
				chart.ScheduleUpdateChart();
			}
		}

		static void OnLabelTemplateChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is IPyramidChartDependent chart)
			{
				chart.OnLabelTemplateChanged((object)oldValue, (object)newValue);
			}
		}

		#endregion

		#region Private Methods

		bool OnTapAction(IChart chart, Point tapPoint, int tapCount)
		{
			var tooltipBehavior = chart.ActualTooltipBehavior;
			tooltipBehavior?.OnTapped(this, tapPoint, tapCount);

			if (SelectionBehavior != null && SelectionBehavior.Type != 0)
			{
				return SelectionBehavior.OnTapped((float)(tapPoint.X - _seriesBounds.Left), (float)(tapPoint.Y - _seriesBounds.Top));
			}

			return false;
		}

#pragma warning disable IDE0060 // Remove unused parameter
#pragma warning disable IDE0060 // Remove unused parameter
		internal void OnTouchUp(IChart chart, long pointerId, Point point)
#pragma warning restore IDE0060 // Remove unused parameter
#pragma warning restore IDE0060 // Remove unused parameter
		{
			InteractiveBehavior?.OnTouchUp(this, (float)point.X, (float)point.Y);
		}

#pragma warning disable IDE0060 // Remove unused parameter
#pragma warning disable IDE0060 // Remove unused parameter
		internal void OnTouchDown(IChart chart, long pointerId, Point point)
#pragma warning restore IDE0060 // Remove unused parameter
#pragma warning restore IDE0060 // Remove unused parameter
		{
			InteractiveBehavior?.OnTouchDown(this, (float)point.X, (float)point.Y);
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

		void CalculateSegment(double total, IList<double> yValues)
		{
			if (yValues != null)
			{
				double currY = 0;
				double coefHeight = 1 / (total * (1 + (GapRatio / (1 - GapRatio))));
				int dataCount = _pointsCount;
				var legendItems = _chartArea.LegendItems;

				//TODO: Data count used for gap ratio calculation, need to consider with dataManager implementation.[Get Non - nan values count]
				for (int i = 0; i < dataCount; i++)
				{
					if (double.IsNaN(yValues[i]))
					{
						dataCount -= 1;
					}
				}

				double spacing = GapRatio / (dataCount - 1);

				for (int i = _pointsCount - 1; i >= 0; i--)
				{
					float yValue = (legendItems.Count == 0) ? (float)Math.Abs(double.IsNaN(yValues[i]) ? double.NaN : yValues[i]) : (float)(legendItems[_pointsCount - (i + 1)].IsToggled ? double.NaN : yValues[i]);
					double height = coefHeight * Math.Abs(float.IsNaN(yValue) ? 0 : yValue);

					if (_pointsCount <= _segments.Count && _segments[i] is FunnelSegment segment)
					{
						segment.SetData(currY, height, NeckWidth, NeckHeight, GetActualXValue(i), yValue);
					}
					else
					{
						var funnelSegment = (FunnelSegment)CreateSegment();
						funnelSegment.Chart = this;
						funnelSegment.SetData(currY, height, NeckWidth, NeckHeight, GetActualXValue(i), yValue);
						funnelSegment.Index = i;
						funnelSegment.Item = _actualData?[i];
						InitiateDataLabels(funnelSegment);
						_segments.Insert(0, funnelSegment);
					}

					if (!float.IsNaN(yValue))
					{
						currY += height + spacing;
					}
				}
			}
		}

		void InitiateDataLabels(FunnelSegment funnelSegment)
		{
			if (DataLabels.Count > _segments.Count)
			{
				DataLabels.Clear();
			}

			if (!funnelSegment.Empty)
			{
				var dataLabel = new ChartDataLabel();
				funnelSegment.DataLabels.Add(dataLabel);
				DataLabels.Insert(0, dataLabel);
			}
		}

		double CalculateTotalValue()
		{
			double sumValues = 0;
			var legendItems = _chartArea.LegendItems;

			if (_yValues != null)
			{
				for (int i = 0; i < _pointsCount; i++)
				{
					if (!double.IsNaN(_yValues[i]))
					{
						if (legendItems.Count == 0)
						{
							sumValues += Math.Abs(_yValues[i]);
						}
						else
						{
							sumValues += legendItems[_pointsCount - (i + 1)].IsToggled ? 0 : Math.Abs(_yValues[i]);
						}
					}
				}
			}

			return sumValues;
		}

		void OnBindingPathChanged()
		{
			ResetData();
			GeneratePoints([YBindingPath], _yValues);
			UpdateLegendItems();
			_segmentsCreated = false;
			ScheduleUpdateChart();
		}

		/// <summary>
		/// Creates and initializes a new chart segment for the funnel chart.
		/// </summary>
		protected internal virtual ChartSegment CreateSegment()
		{
			return new FunnelSegment();
		}

		object? GetActualXValue(int index)
		{
			if (_xValues == null || index > _pointsCount)
			{
				return null;
			}

			if (_xValueType == ChartValueType.String)
			{
				return ((IList<string>)_xValues)[index];
			}
			else if (_xValueType == ChartValueType.DateTime)
			{
				return DateTime.FromOADate(((IList<double>)_xValues)[index]).ToString("MM/dd/yyyy");
			}
			else if (_xValueType == ChartValueType.Double || _xValueType == ChartValueType.Logarithmic)
			{
				//Logic is to cut off the 0 decimal value from the number.
				object label = ((List<double>)_xValues)[index];
				var actualVal = (double)label;
				if (actualVal == (long)actualVal)
				{
					label = (long)actualVal;
				}

				return label;
			}
			else
			{
				return ((IList)_xValues)[index];
			}
		}

		static void OnDataLabelSettingsChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfFunnelChart chart)
			{
				chart.OnDataLabelSettingsPropertyChanged(oldValue as ChartDataLabelSettings, newValue as ChartDataLabelSettings);
			}
		}

		void OnDataLabelSettingsPropertyChanged(ChartDataLabelSettings? oldValue, ChartDataLabelSettings? newValue)
		{
			if (oldValue is FunnelDataLabelSettings oldSettings)
			{
				oldSettings.PropertyChanged -= Settings_PropertyChanged;
				SetInheritedBindingContext(oldSettings, null);
				oldSettings.Parent = null;

				if (oldSettings.LabelStyle != null)
				{
					oldSettings.LabelStyle.PropertyChanged -= Style_PropertyChanged;
					oldSettings.LabelStyle.Parent = null;
					SetInheritedBindingContext(oldSettings.LabelStyle, null);
				}
			}

			if (newValue is FunnelDataLabelSettings newSettings)
			{
				newSettings.PropertyChanged += Settings_PropertyChanged;
				newSettings.Parent = this;
				SetInheritedBindingContext(newSettings, BindingContext);

				if (newSettings.LabelStyle is ChartDataLabelStyle style)
				{
					style.PropertyChanged += Style_PropertyChanged;
					SetInheritedBindingContext(style, BindingContext);
				}
			}
		}

		void Settings_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (sender is not ChartDataLabelSettings dataLabelSettings)
			{
				return;
			}

			if (e.PropertyName == nameof(dataLabelSettings.LabelStyle))
			{
				dataLabelSettings.LabelStyle.Parent = this;
			}

			ScheduleUpdateChart();
		}

		void Style_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			ScheduleUpdateChart();
		}

		#endregion

		#region ItemSource Update

		static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfFunnelChart chart)
			{
				chart.OnItemsSourceChanged(oldValue, newValue);
			}
		}

		void OnItemsSourceChanged(object oldValue, object newValue)
		{
			if (Equals(oldValue, newValue))
			{
				return;
			}

			UpdateLegendItems();
			ResetData();
			OnDataSourceChanged(oldValue, newValue);
			HookAndUnhookCollectionChangedEvent(oldValue, newValue);
			_segmentsCreated = false;
			ScheduleUpdateChart();
		}

#pragma warning disable IDE0060 // Remove unused parameter
#pragma warning disable IDE0060 // Remove unused parameter
		void OnDataSourceChanged(object oldValue, object newValue)
#pragma warning restore IDE0060 // Remove unused parameter
#pragma warning restore IDE0060 // Remove unused parameter
		{
			_yValues.Clear();
			GeneratePoints([YBindingPath], _yValues);
		}

		void ScheduleUpdateChart()
		{
			if (_chartArea != null)
			{
				_chartArea.NeedsRelayout = true;
				_chartArea.ScheduleUpdateArea();
			}
		}

		static object? GetPropertyValue(object obj, string[] paths)
		{
			object? parentObj = obj;

			for (int i = 0; i < paths.Length; i++)
			{
				parentObj = ReflectedObject(parentObj, paths[i]);
			}

			if (parentObj != null)
			{
				if (parentObj.GetType().IsArray)
				{
					return null;
				}
			}

			return parentObj;
		}

		void GenerateDataPoints()
		{
			GeneratePoints([YBindingPath], _yValues);
		}

		void HookAndUnhookCollectionChangedEvent(object oldValue, object? newValue)
		{
			if (newValue != null)
			{
				if (newValue is INotifyCollectionChanged newCollectionValue)
				{
					newCollectionValue.CollectionChanged += OnDataSource_CollectionChanged;
				}
			}

			if (oldValue != null)
			{
				if (oldValue is INotifyCollectionChanged oldCollectionValue)
				{
					oldCollectionValue.CollectionChanged -= OnDataSource_CollectionChanged;
				}
			}
		}

		void OnDataSource_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
		{
			e.ApplyCollectionChanges(
				(obj, index, canInsert) => AddDataPoint(index, obj),
				(obj, index) => RemoveData(index),
				ResetDataPoint);

			_chartArea.ShouldPopulateLegendItems = true;

			UpdateLegendItems();
			ScheduleUpdateChart();
		}

		void AddDataPoint(int index, object data)
		{
			SetIndividualPoint(index, data, false);
		}

		void ResetDataPoint()
		{
			ResetData();

			if (ItemsSource != null)
			{
				var items = ItemsSource is IList ? ItemsSource as IList : null;

				if (items == null)
				{
					if (ItemsSource is IEnumerable source)
					{
						//TODO: Consider removing the toList();
						items = source.Cast<object>().ToList();
					}
				}

				if (items != null && items.Count > 0)
				{
					GenerateDataPoints();
				}
			}
		}

		void RemoveData(int index)
		{
			if (_xValues != null)
			{
				if (_xValues is IList<double> list)
				{
					list.RemoveAt(index);
					_pointsCount--;
				}
				else if (_xValues is IList<string> items)
				{
					items.RemoveAt(index);
					_pointsCount--;
				}
			}

			_actualData?.RemoveAt(index);
			_yValues?.RemoveAt(index);
		}

		static ChartValueType GetDataType(IEnumerator enumerator, string[] paths)
		{
			// GetArrayPropertyValue method is used to get value from the path of current object
			object? parentObj = GetArrayPropertyValue(enumerator.Current, paths);

			return GetDataTypes(parentObj);
		}

		void ResetData()
		{
			if (_actualXValues is IList list && _xValues is IList items)
			{
				list.Clear();
				items.Clear();
			}

			_actualData?.Clear();
			_yValues.Clear();

			_pointsCount = 0;

			if (XBindingPath != null && _yPaths != null && _yPaths.Length != 0)
			{
				_segments.Clear();
				DataLabels.Clear();
			}
		}

		static ChartValueType GetDataType(FastReflection fastReflection, IEnumerable dataSource)
		{
			if (dataSource == null)
			{
				return ChartValueType.Double;
			}

			var enumerator = dataSource.GetEnumerator();
			object? obj = null;

			if (enumerator.MoveNext())
			{
				do
				{
					obj = fastReflection.GetValue(enumerator.Current);
				}
				while (enumerator.MoveNext() && obj == null);
			}

			return SfFunnelChart.GetDataType(obj);
		}

		static ChartValueType GetDataType(object? xValue)
		{
			if (xValue is string || xValue is string[])
			{
				return ChartValueType.String;
			}
			else if (xValue is DateTime || xValue is DateTime[])
			{
				return ChartValueType.DateTime;
			}
			else if (xValue is TimeSpan || xValue is TimeSpan[])
			{
				return ChartValueType.TimeSpan;
			}
			else
			{
				return ChartValueType.Double;
			}
		}

		static object? ReflectedObject(object? parentObj, string actualPath)
		{
			var fastReflection = new FastReflection();

			if (parentObj != null && fastReflection.SetPropertyName(actualPath, parentObj))
			{
				return fastReflection.GetValue(parentObj);
			}

			return null;
		}

		static object? GetArrayPropertyValue(object obj, string[]? paths)
		{
			var parentObj = obj;

			if (paths == null)
			{
				return parentObj;
			}

			for (int i = 0; i < paths.Length; i++)
			{
				var path = paths[i];

				if (path.Contains("[", StringComparison.Ordinal))
				{
					int index = Convert.ToInt32(path.Substring(path.IndexOf('[', StringComparison.Ordinal) + 1, path.IndexOf(']', StringComparison.Ordinal) - path.IndexOf('[', StringComparison.Ordinal) - 1));
					string actualPath = path.Replace(path[path.IndexOf('[', StringComparison.Ordinal)..], string.Empty, StringComparison.Ordinal);
					parentObj = ReflectedObject(parentObj, actualPath);

					if (parentObj == null)
					{
						return null;
					}

					if (parentObj is IList array && array.Count > index)
					{
						parentObj = array[index];
					}
					else
					{
						return null;
					}
				}
				else
				{
					parentObj = ReflectedObject(parentObj, path);

					if (parentObj == null)
					{
						return null;
					}

					if (parentObj.GetType().IsArray)
					{
						return null;
					}
				}
			}

			return parentObj;
		}

		static ChartValueType GetDataTypes(object? xValue)
		{
			if (xValue is string || xValue is string[])
			{
				return ChartValueType.String;
			}
			else if (xValue is DateTime || xValue is DateTime[])
			{
				return ChartValueType.DateTime;
			}
			else if (xValue is TimeSpan || xValue is TimeSpan[])
			{
				return ChartValueType.TimeSpan;
			}
			else
			{
				return ChartValueType.Double;
			}
		}

		void GeneratePoints(string[] yBindingPaths, params IList<double>[] yValueLists)
		{
			if (yBindingPaths == null)
			{
				return;
			}

			IList<double>[]? yLists = null;
			_isComplexYProperty = false;
			bool isArrayProperty = false;
			_yComplexPaths = new string[yBindingPaths.Length][];

			for (int i = 0; i < yBindingPaths.Length; i++)
			{
				if (string.IsNullOrEmpty(yBindingPaths[i]))
				{
					return;
				}

				_yComplexPaths[i] = yBindingPaths[i].Split(['.']);

				if (yBindingPaths[i].Contains('.', StringComparison.Ordinal))
				{
					_isComplexYProperty = true;
				}

				if (yBindingPaths[i].Contains('[', StringComparison.Ordinal))
				{
					isArrayProperty = true;
				}
			}

			yLists = yValueLists;

			_yPaths = yBindingPaths;

			_actualData ??= [];

			if (ItemsSource != null && !string.IsNullOrEmpty(XBindingPath))
			{
				if (ItemsSource is IEnumerable)
				{
					if (XBindingPath.Contains('[', StringComparison.Ordinal) || isArrayProperty)
					{
						GenerateComplexPropertyPoints(yBindingPaths, yLists, GetArrayPropertyValue);
					}
					else if (XBindingPath.Contains('.', StringComparison.Ordinal) || _isComplexYProperty)
					{
						GenerateComplexPropertyPoints(yBindingPaths, yLists, GetPropertyValue);
					}
					else
					{
						GeneratePropertyPoints(yBindingPaths, yLists);
					}
				}
			}
		}

		void GeneratePropertyPoints(string[] yPaths, IList<double>[] yLists)
		{
			var enumerable = ItemsSource as IEnumerable;
			var enumerator = enumerable?.GetEnumerator();

			if (enumerable != null && enumerator != null && enumerator.MoveNext())
			{
				var currObj = enumerator.Current;

				FastReflection xProperty = new FastReflection();

				if (!xProperty.SetPropertyName(XBindingPath, currObj) || xProperty.IsArray(currObj))
				{
					return;
				}

				_xValueType = SfFunnelChart.GetDataType(xProperty, enumerable);

				if (_xValueType == ChartValueType.DateTime || _xValueType == ChartValueType.Double ||
					_xValueType == ChartValueType.Logarithmic || _xValueType == ChartValueType.TimeSpan)
				{
					if (_actualXValues is not List<double>)
					{
						_actualXValues = _xValues = new List<double>();
					}
				}
				else
				{
					if (_actualXValues is not List<string>)
					{
						_actualXValues = _xValues = new List<string>();
					}
				}

				string yPath;

				if (string.IsNullOrEmpty(yPaths[0]))
				{
					return;
				}
				else
				{
					yPath = yPaths[0];
				}

				var yProperty = new FastReflection();

				if (!yProperty.SetPropertyName(yPath, currObj) || yProperty.IsArray(currObj))
				{
					return;
				}

				IList<double> yValue = yLists[0];

				if (_xValueType == ChartValueType.String)
				{
					if (_xValues is List<string> xValue)
					{
						do
						{
							var xVal = xProperty.GetValue(enumerator.Current);
							var yVal = yProperty.GetValue(enumerator.Current);
							xValue.Add(xVal.Tostring());
							yValue.Add(Convert.ToDouble(yVal ?? double.NaN));
							_actualData?.Add(enumerator.Current);
						}
						while (enumerator.MoveNext());
						_pointsCount = xValue.Count;
					}
				}
				else if (_xValueType == ChartValueType.DateTime)
				{
					if (_xValues is List<double> xValue)
					{
						do
						{
							var xVal = xProperty.GetValue(enumerator.Current);
							var yVal = yProperty.GetValue(enumerator.Current);

							_xData = xVal != null ? ((DateTime)xVal).ToOADate() : double.NaN;

							// Check the Data Collection is linear or not
							if (_isLinearData && xValue.Count > 0 && _xData <= xValue[^1])
							{
								_isLinearData = false;
							}

							xValue.Add(_xData);
							yValue.Add(Convert.ToDouble(yVal ?? double.NaN));
							_actualData?.Add(enumerator.Current);
						}
						while (enumerator.MoveNext());
						_pointsCount = xValue.Count;
					}
				}
				else if (_xValueType == ChartValueType.Double ||
						 _xValueType == ChartValueType.Logarithmic)
				{
					if (_xValues is List<double> xValue)
					{
						do
						{
							var xVal = xProperty.GetValue(enumerator.Current);
							var yVal = yProperty.GetValue(enumerator.Current);
							_xData = Convert.ToDouble(xVal ?? double.NaN);

							// Check the Data Collection is linear or not
							if (_isLinearData && xValue.Count > 0 && _xData <= xValue[^1])
							{
								_isLinearData = false;
							}

							xValue.Add(_xData);
							yValue.Add(Convert.ToDouble(yVal ?? double.NaN));
							_actualData?.Add(enumerator.Current);
						}
						while (enumerator.MoveNext());
						_pointsCount = xValue.Count;
					}
				}
				else if (_xValueType == ChartValueType.TimeSpan)
				{
					//TODO: ensure while implementing timespan.
				}
			}
		}

		void GenerateComplexPropertyPoints(string[] yPaths, IList<double>[] yLists, GetReflectedProperty? getPropertyValue)
		{
			var enumerable = ItemsSource as IEnumerable;
			var enumerator = enumerable?.GetEnumerator();

			if (enumerable != null && enumerator != null && getPropertyValue != null && enumerator.MoveNext() && _xComplexPaths != null && _yComplexPaths != null)
			{
				_xValueType = GetDataType(enumerator, _xComplexPaths);

				if (_xValueType == ChartValueType.DateTime || _xValueType == ChartValueType.Double ||
					_xValueType == ChartValueType.Logarithmic || _xValueType == ChartValueType.TimeSpan)
				{
					if (_xValues is not List<double>)
					{
						_actualXValues = _xValues = new List<double>();
					}
				}
				else
				{
					if (_xValues is not List<string>)
					{
						_actualXValues = _xValues = new List<string>();
					}
				}

				string[] tempYPath = _yComplexPaths[0];

				if (string.IsNullOrEmpty(yPaths[0]))
				{
					return;
				}

				IList<double> yValue = yLists[0];
				object? xVal, yVal;

				if (_xValueType == ChartValueType.String)
				{
					if (_xValues is List<string> xValue)
					{
						do
						{
							xVal = getPropertyValue(enumerator.Current, _xComplexPaths);
							yVal = getPropertyValue(enumerator.Current, tempYPath);

							if (xVal == null)
							{
								return;
							}

							xValue.Add((string)xVal);
							yValue.Add(Convert.ToDouble(yVal ?? double.NaN));
							_actualData?.Add(enumerator.Current);
						}
						while (enumerator.MoveNext());
						_pointsCount = xValue.Count;
					}
				}
				else if (_xValueType == ChartValueType.Double ||
					_xValueType == ChartValueType.Logarithmic)
				{
					if (_xValues is List<double> xValue)
					{
						do
						{
							xVal = getPropertyValue(enumerator.Current, _xComplexPaths);
							yVal = getPropertyValue(enumerator.Current, tempYPath);
							_xData = Convert.ToDouble(xVal ?? double.NaN);

							// Check the Data Collection is linear or not
							if (_isLinearData && xValue.Count > 0 && _xData <= xValue[^1])
							{
								_isLinearData = false;
							}

							xValue.Add(_xData);
							yValue.Add(Convert.ToDouble(yVal ?? double.NaN));
							_actualData?.Add(enumerator.Current);
						}
						while (enumerator.MoveNext());
						_pointsCount = xValue.Count;
					}
				}
				else if (_xValueType == ChartValueType.DateTime)
				{
					if (_xValues is List<double> xValue)
					{
						do
						{
							xVal = getPropertyValue(enumerator.Current, _xComplexPaths);
							yVal = getPropertyValue(enumerator.Current, tempYPath);

							_xData = xVal != null ? ((DateTime)xVal).ToOADate() : double.NaN;

							// Check the Data Collection is linear or not
							if (_isLinearData && xValue.Count > 0 && _xData <= xValue[^1])
							{
								_isLinearData = false;
							}

							xValue.Add(_xData);
							yValue.Add(Convert.ToDouble(yVal ?? double.NaN));
							_actualData?.Add(enumerator.Current);
						}
						while (enumerator.MoveNext());
						_pointsCount = xValue.Count;
					}
				}
				else if (_xValueType == ChartValueType.TimeSpan)
				{
					//TODO: Ensure for timespan;
				}
			}
		}

		void SetIndividualPoint(int index, object value, bool replace)
		{
			if (_yValues != null && _yPaths != null && ItemsSource != null)
			{
				var xvalueType = GetArrayPropertyValue(value, _xComplexPaths);

				if (xvalueType != null)
				{
					_xValueType = SfFunnelChart.GetDataType(xvalueType);
				}

				double yData;
				var tempYPath = _yComplexPaths?[0];
				var yValue = _yValues;

				switch (_xValueType)
				{
					case ChartValueType.String:
						{
							if (_xValues is not List<string>)
							{
								_xValues = _actualXValues = new List<string>();
							}

							IList<string> xValue = (List<string>)_xValues;
							var xVal = GetArrayPropertyValue(value, _xComplexPaths);
							var yVal = GetArrayPropertyValue(value, tempYPath);
							yData = yVal != null ? Convert.ToDouble(yVal) : double.NaN;

							if (replace && xValue.Count > index)
							{
								xValue[index] = xVal.Tostring();
							}
							else
							{
								xValue.Insert(index, xVal.Tostring());
							}

							if (replace && yValue.Count > index)
							{
								yValue[index] = yData;
							}
							else
							{
								yValue.Insert(index, yData);
							}

							_pointsCount = xValue.Count;
						}

						break;
					case ChartValueType.Double:
					case ChartValueType.Logarithmic:
						{
							if (_xValues is not List<double>)
							{
								_xValues = _actualXValues = new List<double>();
							}

							IList<double> xValue = (List<double>)_xValues;
							var xVal = GetArrayPropertyValue(value, _xComplexPaths);
							var yVal = GetArrayPropertyValue(value, tempYPath);
							_xData = xVal != null ? Convert.ToDouble(xVal) : double.NaN;
							yData = yVal != null ? Convert.ToDouble(yVal) : double.NaN;

							// Check the Data Collection is linear or not
							if (_isLinearData && xValue.Count > 0 && _xData <= xValue[xValue.Count - 1])
							{
								_isLinearData = false;
							}

							if (replace && xValue.Count > index)
							{
								xValue[index] = _xData;
							}
							else
							{
								xValue.Insert(index, _xData);
							}

							if (replace && yValue.Count > index)
							{
								yValue[index] = yData;
							}
							else
							{
								yValue.Insert(index, yData);
							}

							_pointsCount = xValue.Count;
						}

						break;
					case ChartValueType.DateTime:
						{
							if (_xValues is not List<double>)
							{
								_xValues = _actualXValues = new List<double>();
							}

							IList<double> xValue = (List<double>)_xValues;
							var xVal = GetArrayPropertyValue(value, _xComplexPaths);
							var yVal = GetArrayPropertyValue(value, tempYPath);
							_xData = Convert.ToDateTime(xVal).ToOADate();
							yData = yVal != null ? Convert.ToDouble(yVal) : double.NaN;

							// Check the Data Collection is linear or not
							if (_isLinearData && xValue.Count > 0 && _xData <= xValue[xValue.Count - 1])
							{
								_isLinearData = false;
							}

							if (replace && xValue.Count > index)
							{
								xValue[index] = _xData;
							}
							else
							{
								xValue.Insert(index, _xData);
							}

							if (replace && yValue.Count > index)
							{
								yValue[index] = yData;
							}
							else
							{
								yValue.Insert(index, yData);
							}

							_pointsCount = xValue.Count;
						}

						break;
					case ChartValueType.TimeSpan:
						{
							//TODO: Ensure on time span implementation.
						}

						break;
				}

				if (_actualData != null)
				{
					if (replace && _actualData.Count > index)
					{
						_actualData[index] = value;
					}
					else if (_actualData.Count == index)
					{
						_actualData.Add(value);
					}
					else
					{
						_actualData.Insert(index, value);
					}
				}
			}
		}

		#endregion

		#endregion

		#region Destructor

		/// <summary>
		/// Removed unmanaged resources
		/// </summary>
		/// <exclude/>
		~SfFunnelChart()
		{
			PyramidChartBase.UnhookSegmentsCollectionChanged(this, _segments);
		}
		#endregion
	}
}