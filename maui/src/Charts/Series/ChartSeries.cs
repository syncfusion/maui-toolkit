using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// Represents the base class for all chart series types, including <see cref="SfCartesianChart"/>, <see cref="SfCircularChart"/>, and <see cref="SfPolarChart"/>.
	/// </summary>
	public abstract partial class ChartSeries : Element, IDatapointSelectionDependent, ITooltipDependent, IDataTemplateDependent
	{
		#region Fields

		ObservableCollection<ChartDataLabel> _dataLabels;
		IChart? _chart;
		IList<Brush>? _paletteColors;

		#endregion

		#region Internal Properties

		internal virtual ChartDataLabelSettings? ChartDataLabelSettings
		{
			get
			{
				return null;
			}
		}

		internal readonly float _defaultSelectionStrokeWidth = 5;//Todo: check necessary this default value

		internal IChart? Chart
		{
			get
			{
				return _chart;
			}
			set
			{
				if (_chart != null && value != null)
				{
					throw new ArgumentException("ChartSeries is already the child of another Chart.");
				}

				_chart = value;
				Parent = value as Element;
			}
		}

		internal Rect AreaBounds => Chart != null ? Chart.ActualSeriesClipRect : Rect.Zero;

		internal float AnimationValue { get; set; } = 1;

		internal Animation? SeriesAnimation { get; set; }

		internal bool NeedToAnimateSeries { get; set; }

		internal bool NeedToAnimateDataLabel { get; set; }

		internal bool SegmentsCreated { get; set; }

		internal virtual bool IsStacking
		{
			get
			{
				return false;
			}
		}

		internal DoubleRange VisibleXRange { get; set; }

		internal DoubleRange VisibleYRange { get; set; }

		internal ObservableCollection<ChartSegment>? OldSegments { get; set; }

		internal DoubleRange PreviousXRange { get; set; }

		internal int TooltipDataPointIndex { get; set; }

		internal List<double> GroupedXValuesIndexes { get; set; } = [];

		internal List<object> GroupedActualData { get; set; } = [];

		internal List<string> GroupedXValues { get; set; } = [];

		internal ObservableCollection<ChartDataLabel> DataLabels
		{
			get { return _dataLabels; }
			set
			{
				_dataLabels = value;
				OnPropertyChanged(nameof(DataLabels));
			}
		}

		internal DataLabelLayout LabelTemplateView { get; set; }

		internal static readonly BindableProperty AnimationDurationProperty =
			BindableProperty.Create(nameof(AnimationDuration), typeof(double), typeof(ChartSeries), 1d, BindingMode.Default, null, null);

		internal double AnimationDuration
		{
			get { return (double)GetValue(AnimationDurationProperty); }
			set { SetValue(AnimationDurationProperty, value); }
		}

		#endregion

		#region Bindable Properties

		/// <summary>
		/// Identifies the <see cref="ItemsSource"/> bindable property.
		/// </summary>
		/// <remarks>
		/// Represents the data source for the chart series.
		/// </remarks>
		public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
			nameof(ItemsSource),
			typeof(object),
			typeof(ChartSeries),
			null,
			BindingMode.Default,
			null,
			OnItemsSourceChanged);

		/// <summary>
		/// Identifies the <see cref="XBindingPath"/> bindable property.
		/// </summary>
		/// <remarks>
		/// Represents the binding path for the X-axis values in the chart series.
		/// </remarks>
		public static readonly BindableProperty XBindingPathProperty = BindableProperty.Create(
			nameof(XBindingPath),
			typeof(string),
			typeof(ChartSeries),
			null,
			BindingMode.Default,
			null,
			OnXBindingPathChanged);

		/// <summary>
		/// Identifies the <see cref="Fill"/> bindable property.
		/// </summary>
		/// <remarks>
		/// Represents the fill brush for the chart series.
		/// </remarks>
		public static readonly BindableProperty FillProperty = BindableProperty.Create(
			nameof(Fill),
			typeof(Brush),
			typeof(ChartSeries),
			null,
			BindingMode.Default,
			null,
			OnFillPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="PaletteBrushes"/> bindable property.
		/// </summary>
		/// <remarks>
		/// Represents a collection of brushes used in the chart series palette.
		/// </remarks>
		public static readonly BindableProperty PaletteBrushesProperty = BindableProperty.Create(
			nameof(PaletteBrushes),
			typeof(IList<Brush>),
			typeof(ChartSeries),
			null,
			BindingMode.Default,
			null,
			OnPaletteBrushesChanged);

		/// <summary>
		/// Identifies the <see cref="IsVisible"/> bindable property.
		/// </summary>
		/// <remarks>
		/// Indicates whether the chart series is visible.
		/// </remarks>
		public static readonly BindableProperty IsVisibleProperty = BindableProperty.Create(
			nameof(IsVisible),
			typeof(bool),
			typeof(ChartSeries),
			true,
			BindingMode.Default,
			null,
			OnVisiblePropertyChanged);

		/// <summary>
		/// Identifies the <see cref="Opacity"/> bindable property.
		/// </summary>
		/// <remarks>
		/// Represents the opacity of the chart series, where 0 is fully transparent and 1 is fully opaque.
		/// </remarks>
		public static readonly BindableProperty OpacityProperty = BindableProperty.Create(
			nameof(Opacity),
			typeof(double),
			typeof(ChartSeries),
			1d,
			BindingMode.Default,
			null,
			OnOpacityPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="EnableAnimation"/> bindable property.
		/// </summary>
		/// <remarks>
		/// Indicates whether animations are enabled for the chart series.
		/// </remarks>
		public static readonly BindableProperty EnableAnimationProperty = BindableProperty.Create(
			nameof(EnableAnimation),
			typeof(bool),
			typeof(ChartSeries),
			false,
			BindingMode.Default,
			null,
			OnEnableAnimationPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="EnableTooltip"/> bindable property.
		/// </summary>
		/// <remarks>
		/// Indicates whether tooltips are enabled for the chart series.
		/// </remarks>
		public static readonly BindableProperty EnableTooltipProperty = BindableProperty.Create(
			nameof(EnableTooltip),
			typeof(bool),
			typeof(ChartSeries),
			false,
			BindingMode.Default,
			null,
			OnEnableTooltipPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="TooltipTemplate"/> bindable property.
		/// </summary>
		/// <remarks>
		/// Provides a template for customizing the tooltip display in the chart series.
		/// </remarks>
		public static readonly BindableProperty TooltipTemplateProperty = BindableProperty.Create(
			nameof(TooltipTemplate),
			typeof(DataTemplate),
			typeof(ChartSeries),
			null,
			BindingMode.Default,
			null,
			null);

		/// <summary>
		/// Identifies the <see cref="ShowDataLabels"/> bindable property.
		/// </summary>
		/// <remarks>
		/// Indicates whether data labels are shown in the chart series.
		/// </remarks>
		public static readonly BindableProperty ShowDataLabelsProperty = BindableProperty.Create(
			nameof(ShowDataLabels),
			typeof(bool),
			typeof(ChartSeries),
			false,
			BindingMode.Default,
			null,
			OnShowDataLabelsChanged);

		/// <summary>
		/// Identifies the <see cref="LegendIcon"/> bindable property.
		/// </summary>
		/// <remarks>
		/// Represents the icon type for the legend.
		/// </remarks>
		public static readonly BindableProperty LegendIconProperty = BindableProperty.Create(
			nameof(LegendIcon),
			typeof(ChartLegendIconType),
			typeof(ChartSeries),
			ChartLegendIconType.Circle,
			BindingMode.Default,
			null,
			OnLegendIconChanged);

		/// <summary>
		/// Identifies the <see cref="IsVisibleOnLegend"/> bindable property.
		/// </summary>
		/// <remarks>
		/// Indicates whether to display a legend item for the chart series.
		/// </remarks>
		public static readonly BindableProperty IsVisibleOnLegendProperty = BindableProperty.Create(
			nameof(IsVisibleOnLegend),
			typeof(bool),
			typeof(ChartSeries),
			true,
			BindingMode.Default,
			null,
			OnIsVisibleOnLegendChanged);

		/// <summary>
		/// Identifies the <see cref="SelectionBehavior"/> bindable property.
		/// </summary>
		/// <remarks>
		/// Represents the behavior of data point selection in the chart series.
		/// </remarks>
		public static readonly BindableProperty SelectionBehaviorProperty = BindableProperty.Create(
			nameof(SelectionBehavior),
			typeof(DataPointSelectionBehavior),
			typeof(ChartSeries),
			null,
			BindingMode.Default,
			null,
			OnSelectionBehaviorPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="LabelContext"/> bindable property.
		/// </summary>
		/// <remarks>
		/// Determines the content to be displayed in the data labels.
		/// </remarks>
		public static readonly BindableProperty LabelContextProperty = BindableProperty.Create(
			nameof(LabelContext),
			typeof(LabelContext),
			typeof(ChartSeries),
			LabelContext.YValue,
			BindingMode.Default,
			null,
			OnLabelContextPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="LabelTemplate"/> bindable property.
		/// </summary>
		/// <remarks>
		/// Provides a template for customizing the data label.
		/// </remarks>
		public static readonly BindableProperty LabelTemplateProperty = BindableProperty.Create(
			nameof(LabelTemplate),
			typeof(DataTemplate),
			typeof(ChartSeries),
			null,
			BindingMode.Default,
			null,
			OnLabelTemplateChanged);

		/// <summary>
		/// Identifies the <see cref="ListenPropertyChange"/> bindable property.
		/// </summary>
		/// <remarks>
		/// This bindable property allows the <see cref="ChartSeries"/> to be notified of and react to property changes, ensuring your chart remains current and responsive in real-time scenarios.
		/// When activated, it facilitates automatic UI updates that are both adaptive and flexible, maintaining a seamless user experience.
		/// However, be mindful of the potential performance cost, particularly when dealing with large collections or multiple rapid updates, as the visual refresh may introduce additional load.
		/// For optimal performance, strategically employ this feature to maintain a balance between responsiveness and system efficiency.
		/// </remarks>
		public static readonly BindableProperty ListenPropertyChangeProperty = BindableProperty.Create(
			nameof(ListenPropertyChange),
			typeof(bool),
			typeof(ChartSeries),
			false,
			BindingMode.Default,
			null,
			OnListenPropertyChangeChanged);


		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets a data points collection that will be used to plot a chart.
		/// </summary>
		/// <example>
		/// # [Xaml](#tab/tabid-1)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///     <!-- ... Eliminated for simplicity-->
		///
		///          <chart:ColumnSeries ItemsSource="{Binding Data}"
		///                              XBindingPath="XValue"
		///                              YBindingPath="YValue" />
		///
		///     </chart:SfCartesianChart>
		/// ]]></code>
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		///     SfCartesianChart chart = new SfCartesianChart();
		///     ViewModel viewModel = new ViewModel();
		///
		///     // Eliminated for simplicity
		///
		///     ColumnSeries columnSeries = new ColumnSeries()
		///     {
		///           ItemsSource = viewModel.Data,
		///           XBindingPath = "XValue",
		///           YBindingPath = "YValue",
		///     };
		///     
		///     chart.Series.Add(columnSeries);
		///
		/// ]]></code>
		/// ***
		/// </example>
		public object ItemsSource
		{
			get { return (object)GetValue(ItemsSourceProperty); }
			set { SetValue(ItemsSourceProperty, value); }
		}

		/// <summary>
		/// Gets or sets a path value on the source object to serve a x value to the series.
		/// </summary>
		/// <value>
		/// The <c>string</c> that represents the property name for the x plotting data, and its default value is null.
		/// </value>
		/// <example>
		/// # [Xaml](#tab/tabid-3)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///     <!-- ... Eliminated for simplicity-->
		///
		///          <chart:ColumnSeries ItemsSource="{Binding Data}"
		///                              XBindingPath="XValue"
		///                              YBindingPath="YValue" />
		///
		///     </chart:SfCartesianChart>
		/// ]]></code>
		/// # [C#](#tab/tabid-4)
		/// <code><![CDATA[
		///     SfCartesianChart chart = new SfCartesianChart();
		///     ViewModel viewModel = new ViewModel();
		///
		///     // Eliminated for simplicity
		///
		///     ColumnSeries columnSeries = new ColumnSeries()
		///     {
		///           ItemsSource = viewModel.Data,
		///           XBindingPath = "XValue",
		///           YBindingPath = "YValue",
		///     };
		///     
		///     chart.Series.Add(columnSeries);
		///
		/// ]]></code>
		/// ***
		/// </example>
		public string XBindingPath
		{
			get { return (string)GetValue(XBindingPathProperty); }
			set { SetValue(XBindingPathProperty, value); }
		}

		/// <summary>
		/// Gets or sets a brush value to customize the series appearance.
		/// </summary>
		/// <value>It accepts a <see cref="Brush"/> value and its default value is null.</value>
		/// <example>
		/// # [Xaml](#tab/tabid-5)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///     <!-- ... Eliminated for simplicity-->
		///
		///          <chart:ColumnSeries ItemsSource="{Binding Data}"
		///                              XBindingPath="XValue"
		///                              YBindingPath="YValue"
		///                              Fill = "Red"/>
		///
		///     </chart:SfCartesianChart>
		/// ]]></code>
		/// # [C#](#tab/tabid-6)
		/// <code><![CDATA[
		///     SfCartesianChart chart = new SfCartesianChart();
		///     ViewModel viewModel = new ViewModel();
		///
		///     // Eliminated for simplicity
		///
		///     ColumnSeries columnSeries = new ColumnSeries()
		///     {
		///           ItemsSource = viewModel.Data,
		///           XBindingPath = "XValue",
		///           YBindingPath = "YValue",
		///           Fill = new SolidColorBrush(Colors.Red),
		///     };
		///     
		///     chart.Series.Add(columnSeries);
		///
		/// ]]></code>
		/// ***
		/// </example>
		public Brush Fill
		{
			get { return (Brush)GetValue(FillProperty); }
			set { SetValue(FillProperty, value); }
		}

		/// <summary>
		/// Gets or sets the list of brushes that can be used to customize the appearance of the series.
		/// </summary>
		/// <value>This property accepts a list of brushes as input and comes with a set of predefined brushes by default.</value>
		/// <example>
		/// # [Xaml](#tab/tabid-7)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///     <!-- ... Eliminated for simplicity-->
		///
		///          <chart:ColumnSeries ItemsSource="{Binding Data}"
		///                              XBindingPath="XValue"
		///                              YBindingPath="YValue"
		///                              PaletteBrushes = "{Binding CustomBrushes}"/>
		///
		///     </chart:SfCartesianChart>
		/// ]]></code>
		/// # [C#](#tab/tabid-8)
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
		///     // Eliminated for simplicity
		///
		///     ColumnSeries columnSeries = new ColumnSeries()
		///     {
		///           ItemsSource = viewModel.Data,
		///           XBindingPath = "XValue",
		///           YBindingPath = "YValue",
		///           PaletteBrushes = CustomBrushes;
		///     };
		///     
		///     chart.Series.Add(columnSeries);
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
		/// Gets or sets a value that indicates whether the series is visible or not.
		/// </summary>
		/// <value>It accepts <c>bool</c> values and its default value is <c>True</c>.</value>
		/// <example>
		/// # [Xaml](#tab/tabid-9)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///     <!-- ... Eliminated for simplicity-->
		///
		///          <chart:ColumnSeries ItemsSource="{Binding Data}"
		///                              XBindingPath="XValue"
		///                              YBindingPath="YValue"
		///                              IsVisible="False"/>
		///
		///     </chart:SfCartesianChart>
		/// ]]></code>
		/// # [C#](#tab/tabid-10)
		/// <code><![CDATA[
		///     SfCartesianChart chart = new SfCartesianChart();
		///     ViewModel viewModel = new ViewModel();
		///
		///     // Eliminated for simplicity
		///
		///     ColumnSeries columnSeries = new ColumnSeries()
		///     {
		///           ItemsSource = viewModel.Data,
		///           XBindingPath = "XValue",
		///           YBindingPath = "YValue",
		///           IsVisible = false,
		///     };
		///     
		///     chart.Series.Add(columnSeries);
		///
		/// ]]></code>
		/// ***
		/// </example>
		public bool IsVisible
		{
			get { return (bool)GetValue(IsVisibleProperty); }
			set { SetValue(IsVisibleProperty, value); }
		}

		/// <summary>
		/// Gets or sets opacity of the chart series.
		/// </summary>
		/// <value> Accepts <c>double</c> values ranging from 0 to 1, where 0 represents fully transparent, 
		/// 1 represents fully opaque, and intermediate values provide partial transparency. The default value is 1.</value>
		/// <example>
		/// # [Xaml](#tab/tabid-11)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///     <!-- ... Eliminated for simplicity-->
		///
		///          <chart:ColumnSeries ItemsSource="{Binding Data}"
		///                              XBindingPath="XValue"
		///                              YBindingPath="YValue"
		///                              Opacity="0.5"/>
		///
		///     </chart:SfCartesianChart>
		/// ]]></code>
		/// # [C#](#tab/tabid-12)
		/// <code><![CDATA[
		///     SfCartesianChart chart = new SfCartesianChart();
		///     ViewModel viewModel = new ViewModel();
		///
		///     // Eliminated for simplicity
		///
		///     ColumnSeries columnSeries = new ColumnSeries()
		///     {
		///           ItemsSource = viewModel.Data,
		///           XBindingPath = "XValue",
		///           YBindingPath = "YValue",
		///           Opacity = 0.5,
		///     };
		///     
		///     chart.Series.Add(columnSeries);
		///
		/// ]]></code>
		/// ***
		/// </example> 
		public double Opacity
		{
			get { return (double)GetValue(OpacityProperty); }
			set { SetValue(OpacityProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether to animate the chart series on loading.
		/// </summary>
		/// <value> It accepts <c>bool</c> values and its default value is <c>False</c>.</value>
		/// <example>
		/// # [Xaml](#tab/tabid-13)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///     <!-- ... Eliminated for simplicity-->
		///
		///          <chart:ColumnSeries ItemsSource="{Binding Data}"
		///                              XBindingPath="XValue"
		///                              YBindingPath="YValue"
		///                              EnableAnimation = "True"/>
		///
		///     </chart:SfCartesianChart>
		/// ]]></code>
		/// # [C#](#tab/tabid-14)
		/// <code><![CDATA[
		///     SfCartesianChart chart = new SfCartesianChart();
		///     ViewModel viewModel = new ViewModel();
		///
		///     // Eliminated for simplicity
		///
		///     ColumnSeries columnSeries = new ColumnSeries()
		///     {
		///           ItemsSource = viewModel.Data,
		///           XBindingPath = "XValue",
		///           YBindingPath = "YValue",
		///           EnableAnimation = true,
		///     };
		///     
		///     chart.Series.Add(columnSeries);
		///
		/// ]]></code>
		/// ***
		/// </example>
		public bool EnableAnimation
		{
			get { return (bool)GetValue(EnableAnimationProperty); }
			set { SetValue(EnableAnimationProperty, value); }
		}

		/// <summary>
		/// Gets or sets a boolean value indicating whether the tooltip for series should be shown or hidden.
		/// </summary>
		/// <value>It accepts <c>bool</c> values and its default value is <c>False</c>.</value>
		/// <remarks>The series tooltip will appear when you click or tap the series.</remarks>
		/// <example>
		/// # [Xaml](#tab/tabid-15)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///     <!-- ... Eliminated for simplicity-->
		///
		///          <chart:ColumnSeries ItemsSource="{Binding Data}"
		///                              XBindingPath="XValue"
		///                              YBindingPath="YValue"
		///                              EnableTooltip="True"/>
		///
		///     </chart:SfCartesianChart>
		/// ]]></code>
		/// # [C#](#tab/tabid-16)
		/// <code><![CDATA[
		///     SfCartesianChart chart = new SfCartesianChart();
		///     ViewModel viewModel = new ViewModel();
		///
		///     // Eliminated for simplicity
		///
		///     ColumnSeries columnSeries = new ColumnSeries()
		///     {
		///           ItemsSource = viewModel.Data,
		///           XBindingPath = "XValue",
		///           YBindingPath = "YValue",
		///           EnableTooltip = true,
		///     };
		///     
		///     chart.Series.Add(columnSeries);
		///
		/// ]]></code>
		/// ***
		/// </example>
		public bool EnableTooltip
		{
			get { return (bool)GetValue(EnableTooltipProperty); }
			set { SetValue(EnableTooltipProperty, value); }
		}

		/// <summary>
		/// Gets or sets the DataTemplate that can be used to customize the appearance of the tooltip.
		/// </summary>
		/// <value>
		/// It accepts a <see cref="DataTemplate"/> value.
		/// </value>
		/// <example>
		/// # [MainWindow.xaml](#tab/tabid-17)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///           <chart:SfCartesianChart.Resources>
		///               <DataTemplate x:Key="tooltipTemplate1">
		///                  <StackLayout Orientation = "Horizontal" >
		///                     <Label Text="{Binding Item.XValue}" 
		///                            TextColor="Black"
		///                            FontAttributes="Bold"
		///                            FontSize="12"
		///                            HorizontalOptions="Center"
		///                            VerticalOptions="Center"/>
		///                     <Label Text = " : "
		///                            TextColor="Black"
		///                            FontAttributes="Bold"
		///                            FontSize="12"
		///                            HorizontalOptions="Center"
		///                            VerticalOptions="Center"/>
		///                     <Label Text = "{Binding Item.YValue}"
		///                            TextColor="Black"
		///                            FontAttributes="Bold"
		///                            FontSize="12"
		///                            HorizontalOptions="Center"
		///                            VerticalOptions="Center"/>
		///                  </StackLayout>
		///               </DataTemplate>
		///           </chart:SfCartesianChart.Resources>
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
		///               <chart:ColumnSeries ItemsSource="{Binding Data}"
		///                                   XBindingPath="XValue"
		///                                   YBindingPath="YValue"
		///                                   EnableTooltip="True"
		///                                   TooltipTemplate="{StaticResource tooltipTemplate1}">
		///               </chart:ColumnSeries> 
		///           </chart:SfCartesianChart.Series>
		///           
		///     </chart:SfCartesianChart>
		/// ]]></code>
		/// # [C#](#tab/tabid-18)
		/// <code><![CDATA[
		///     SfCartesianChart chart = new SfCartesianChart();
		///     ViewModel viewModel = new ViewModel();
		///
		///     // Eliminated for simplicity
		///
		///     ColumnSeries columnSeries = new ColumnSeries()
		///     {
		///           ItemsSource = viewModel.Data,
		///           XBindingPath = "XValue",
		///           YBindingPath = "YValue",
		///           EnableTooltip = true,
		///           TooltipTemplate = chart.Resources["tooltipTemplate1"] as DataTemplate
		///     };
		///     
		///     chart.Series.Add(columnSeries);
		///
		/// ]]></code>
		/// ***
		/// </example>
		public DataTemplate TooltipTemplate
		{
			get { return (DataTemplate)GetValue(TooltipTemplateProperty); }
			set { SetValue(TooltipTemplateProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value that indicates to enable the data labels for the series..
		/// </summary>
		/// <value>It accepts <c>bool</c> values and the default value is <c>False</c>.</value>
		/// <example>
		/// # [MainWindow.xaml](#tab/tabid-19)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
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
		///               <chart:ColumnSeries ItemsSource="{Binding Data}"
		///                                   XBindingPath="XValue"
		///                                   YBindingPath="YValue"
		///                                   ShowDataLabels="True">
		///               </chart:ColumnSeries> 
		///           </chart:SfCartesianChart.Series>
		///           
		///     </chart:SfCartesianChart>
		/// ]]></code>
		/// # [MainWindow.cs](#tab/tabid-20)
		/// <code><![CDATA[
		///     SfCartesianChart chart = new SfCartesianChart();
		///     
		///     NumericalAxis xAxis = new NumericalAxis();
		///     NumericalAxis yAxis = new NumericalAxis();
		///     
		///     chart.XAxes.Add(xAxis);
		///     chart.YAxes.Add(yAxis);
		///     
		///     ColumnSeries series = new ColumnSeries();
		///     series.ItemsSource = viewModel.Data;
		///     series.XBindingPath = "XValue";
		///     series.YBindingPath = "YValue";
		///     series.ShowDataLabels = "True";
		///     chart.Series.Add(series);
		///     
		/// ]]></code>
		/// ***
		/// </example>
		public bool ShowDataLabels
		{
			get { return (bool)GetValue(ShowDataLabelsProperty); }
			set { SetValue(ShowDataLabelsProperty, value); }
		}

		/// <summary>
		/// Gets or sets a legend icon that will be displayed in the associated legend item.
		/// </summary>
		/// <value> It accepts <see cref="ChartLegendIconType"/> values and its default value is <see cref="ChartLegendIconType.Circle"/>.</value>
		/// <example>
		/// # [Xaml](#tab/tabid-21)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///     <!-- ... Eliminated for simplicity-->
		///     
		///         <chart:SfCartesianChart.Legend>
		///            <chart:ChartLegend />
		///         </chart:SfCartesianChart.Legend>
		///
		///          <chart:ColumnSeries ItemsSource="{Binding Data}"
		///                              XBindingPath="XValue"
		///                              YBindingPath="YValue"
		///                              LegendIcon = "Diamond"/>
		///
		///     </chart:SfCartesianChart>
		/// ]]></code>
		/// # [C#](#tab/tabid-22)
		/// <code><![CDATA[
		///     SfCartesianChart chart = new SfCartesianChart();
		///     chart.Legend = new ChartLegend();
		///     ViewModel viewModel = new ViewModel();
		///
		///     // Eliminated for simplicity
		///
		///     ColumnSeries columnSeries = new ColumnSeries()
		///     {
		///           ItemsSource = viewModel.Data,
		///           XBindingPath = "XValue",
		///           YBindingPath = "YValue",
		///           LegendIcon = ChartLegendIconType.Diamond,
		///     };
		///     
		///     chart.Series.Add(columnSeries);
		///
		/// ]]></code>
		/// ***
		/// </example>
		public ChartLegendIconType LegendIcon
		{
			get { return (ChartLegendIconType)GetValue(LegendIconProperty); }
			set { SetValue(LegendIconProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value that indicates whether to show a legend item for this series.
		/// </summary>
		/// <value> It accepts <c>bool</c> values and its default value is <c>True</c>.</value>
		/// <example>
		/// # [Xaml](#tab/tabid-23)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///     <!-- ... Eliminated for simplicity-->
		///     
		///         <chart:SfCartesianChart.Legend>
		///            <chart:ChartLegend />
		///         </chart:SfCartesianChart.Legend>
		///
		///          <chart:ColumnSeries ItemsSource="{Binding Data}"
		///                            XBindingPath="XValue"
		///                            YBindingPath="YValue"
		///                            IsVisibleOnLegend = "False"/>
		///
		///     </chart:SfCartesianChart>
		/// ]]></code>
		/// # [C#](#tab/tabid-24)
		/// <code><![CDATA[
		///     SfCartesianChart chart = new SfCartesianChart();
		///     chart.Legend = new ChartLegend();
		///     ViewModel viewModel = new ViewModel();
		///
		///     // Eliminated for simplicity
		///
		///     ColumnSeries columnSeries = new ColumnSeries()
		///     {
		///           ItemsSource = viewModel.Data,
		///           XBindingPath = "XValue",
		///           YBindingPath = "YValue",
		///           IsVisibleOnLegend = false,
		///     };
		///     
		///     chart.Series.Add(columnSeries);
		///
		/// ]]></code>
		/// ***
		/// </example>
		public bool IsVisibleOnLegend
		{
			get { return (bool)GetValue(IsVisibleOnLegendProperty); }
			set { SetValue(IsVisibleOnLegendProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value for initiating selection or highlighting of a single or multiple data points in the series.
		/// </summary>
		/// <value>It accepts the <see cref="DataPointSelectionBehavior"/> values and its default value is null</value>
		/// <remarks> This functionality is not supported for polar charts. </remarks>
		/// <example>
		/// # [MainPage.xaml](#tab/tabid-25)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		/// 
		///         <chart:SfCartesianChart.Series>
		///             <chart:ColumnSeries ItemsSource="{Binding Data}" XBindingPath="XValue" YBindingPath="YValue">
		///                 <chart:ColumnSeries.SelectionBehavior>
		///                     <chart:DataPointSelectionBehavior SelectionBrush="#314A6E"/>
		///                 </chart:ColumnSeries.SelectionBehavior>
		///             </chart:ColumnSeries>
		///         </chart:SfCartesianChart.Series>
		/// 
		/// </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// 
		/// # [MainPage.xaml.cs](#tab/tabid-26)
		/// <code><![CDATA[
		///  SfCartesianChart chart = new SfCartesianChart();
		///  
		///  ViewModel viewModel = new ViewModel();
		///  chart.BindingContext = viewModel;
		///  
		///  DataPointSelectionBehavior behavior = new DataPointSelectionBehavior();
		///  behavior.SelectionBrush=Color.FromArgb("#314A6E");
		///  
		///  ColumnSeries series = new ColumnSeries()
		///  {
		///      ItemsSource = viewModel.Data,
		///      XBindingPath = "XValue",
		///      YBindingPath = "YValue",
		///      SelectionBehavior = behavior
		///  };
		///  
		///  chart.Series.Add(series);
		/// ]]>
		/// </code>
		/// *** 
		/// 
		/// </example>
		public DataPointSelectionBehavior SelectionBehavior
		{
			get { return (DataPointSelectionBehavior)GetValue(SelectionBehaviorProperty); }
			set { SetValue(SelectionBehaviorProperty, value); }
		}

		/// <summary>
		/// Gets or sets an option that determines the content to be displayed in the data labels. It is recommended to use PieSeries, DoughnutSeries, and BarSeries with LabelContext set to Percentage. 
		/// </summary>
		/// <value>It accepts the <see cref="Charts.LabelContext"/> values and its default value is <see cref="LabelContext.YValue"/>.</value>
		/// <example>
		/// # [MainWindow.xaml](#tab/tabid-27)
		/// <code><![CDATA[
		///     <chart:SfCircularChart>
		///
		///           <chart:SfCircularChart.Series>
		///               <chart:PieSeries ItemsSource="{Binding Data}"
		///                                XBindingPath="XValue"
		///                                YBindingPath="YValue"
		///                                ShowDataLabels="True"
		///                                LabelContext="Percentage">
		///               </chart:PieSeries> 
		///           </chart:SfCircularChart.Series>
		///           
		///     </chart:SfCircularChart>
		/// ]]></code>
		/// # [MainWindow.cs](#tab/tabid-28)
		/// <code><![CDATA[
		///     SfCircularChart chart = new SfCircularChart();
		///     
		///     PieSeries series = new PieSeries();
		///     series.ItemsSource = new ViewModel().Data;
		///     series.XBindingPath = "XValue";
		///     series.YBindingPath = "YValue";
		///     series.ShowDataLabels = true;
		///     series.LabelContext = LabelContext.Percentage;
		///     chart.Series.Add(series);
		///     
		/// ]]></code>
		/// ***
		/// </example>
		public LabelContext LabelContext
		{
			get { return (LabelContext)GetValue(LabelContextProperty); }
			set { SetValue(LabelContextProperty, value); }
		}

		/// <summary>
		/// Gets or sets the <b> DataTemplate </b> that can be used to customize the appearance of the data label.
		/// </summary>
		/// <value>
		/// It accepts the <see cref="DataTemplate"/> values.
		/// </value>
		/// <example>
		/// # [MainWindow.xaml](#tab/tabid-29)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///           <chart:SfCartesianChart.Resources>
		///               <DataTemplate x:Key="DataLabelTemplate">
		///                  <VerticalStackLayout>
		///                     <Image Source="image.png" 
		///                            WidthRequest="20" 
		///                            HeightRequest="20"/>
		///                     <Label Text="{Binding Label}" 
		///                            TextColor="Black"
		///                            FontAttributes="Bold"
		///                            FontSize="12"/>
		///                  </VerticalStackLayout>
		///               </DataTemplate>
		///           </chart:SfCartesianChart.Resources>
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
		///               <chart:ColumnSeries ItemsSource="{Binding Data}"
		///                                   XBindingPath="XValue"
		///                                   YBindingPath="YValue"
		///                                   ShowDataLabels = "True"
		///                                   LabelTemplate="{StaticResource DataLabelTemplate}">
		///               </chart:ColumnSeries> 
		///           </chart:SfCartesianChart.Series>
		///           
		///     </chart:SfCartesianChart>
		/// ]]></code>
		/// # [MainWindow.cs](#tab/tabid-30)
		/// <code><![CDATA[
		/// SfCartesianChart chart = new SfCartesianChart();
		/// 
		///  NumericalAxis xAxis = new NumericalAxis();
		///  NumericalAxis yAxis = new NumericalAxis();
		///     
		///  chart.XAxes.Add(xAxis);
		///  chart.YAxes.Add(yAxis);
		/// 
		/// ColumnSeries series = new ColumnSeries();
		/// series.ItemsSource = new ViewModel().Data;
		/// series.XBindingPath = "XValue";
		/// series.YBindingPath = "YValue";
		/// series.ShowDataLabels = true;
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
		///     label.SetBinding(Label.TextProperty, new Binding("Label"));
		///     layout.Children.Add(image);
		///     layout.Children.Add(label);
		///     return layout;
		/// }    
		/// 
		/// series.LabelTemplate = labelTemplate;
		/// chart.Series.Add(series);
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public DataTemplate LabelTemplate
		{
			get { return (DataTemplate)GetValue(LabelTemplateProperty); }
			set { SetValue(LabelTemplateProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether the chart series should listen for changes in properties.
		/// </summary>
		/// <value>A boolean value that specifies if property change events should be listened to. The default value is <c>false</c>.</value>
		/// <remarks>
		/// Setting this property to <c>true</c> allows the series to dynamically and automatically update in response to changes in relevant property values, enhancing the interactivity and responsiveness of your chart.
		/// It's beneficial when you have dynamic data sources or configuration changes that need reflection in real time.
		/// It is important to note that while this feature enhances interactivity, it can impact performance, especially during bulk updates or rapid consecutive changes in the data source, as the UI is updated frequently.
		/// For improved performance and responsiveness, set this property when only necessary changes are expected.
		/// </remarks>
		/// <example>
		/// # [Xaml](#tab/tabid-31)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///     <!-- ... Eliminated for simplicity-->
		///
		///          <chart:LineSeries ItemsSource="{Binding Data}"
		///                            XBindingPath="XValue"
		///                            YBindingPath="YValue"
		///                            ListenPropertyChange = "True"/>
		///
		///     </chart:SfCartesianChart>
		/// ]]></code>
		/// # [C#](#tab/tabid-32)
		/// <code><![CDATA[
		///     SfCartesianChart chart = new SfCartesianChart();
		///     ViewModel viewModel = new ViewModel();
		///
		///     // Eliminated for simplicity
		///
		///     LineSeries lineSeries = new LineSeries()
		///     {
		///           ItemsSource = viewModel.Data,
		///           XBindingPath = "XValue",
		///           YBindingPath = "YValue",
		///           ListenPropertyChange = true,
		///     };
		///     
		///     chart.Series.Add(lineSeries);
		///
		/// ]]></code>
		/// ***
		/// </example>
		public bool ListenPropertyChange
		{
			get { return (bool)GetValue(ListenPropertyChangeProperty); }
			set { SetValue(ListenPropertyChangeProperty, value); }
		}

		/// <summary>
		/// Gets the XRange values.
		/// </summary>
		public DoubleRange XRange { get; internal set; } = DoubleRange.Empty;

		/// <summary>
		/// Gets the YRange values.
		/// </summary>
		public DoubleRange YRange { get; internal set; } = DoubleRange.Empty;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="ChartSeries"/>.
		/// </summary>
		public ChartSeries()
		{
			_segments = [];
			_segments.CollectionChanged += Segments_CollectionChanged;
			_dataLabels = [];
			LabelTemplateView = new DataLabelLayout(this);
		}

		#endregion

		#region Interface implementation

		ObservableCollection<ChartDataLabel> IDataTemplateDependent.DataLabels => DataLabels;

		DataTemplate IDataTemplateDependent.LabelTemplate => LabelTemplate;

		bool IDataTemplateDependent.IsVisible => IsVisible;

		ObservableCollection<ChartSegment> IDatapointSelectionDependent.Segments => _segments;

		Rect IDatapointSelectionDependent.AreaBounds => AreaBounds;

		void IDatapointSelectionDependent.Invalidate()
		{
			InValidateViews();
		}

		internal void InValidateViews()
		{
			InvalidateSeries();

			if (ShowDataLabels)
			{
				InvalidateDataLabel();
			}
		}

		void IDatapointSelectionDependent.UpdateSelectedItem(int index)
		{
			//Update selected segment.
			//While selection no need to update legend items for cartesian series. So created override to circular alone.
			SetFillColor(_segments[index]);
		}

		void IDatapointSelectionDependent.UpdateLegendIconColor(ChartSelectionBehavior sender, int index)
		{
			UpdateLegendIconColor(sender, index);
		}

		void IDatapointSelectionDependent.SetFillColor(ChartSegment segment) => SetFillColor(segment);

		void ITooltipDependent.SetTooltipTargetRect(TooltipInfo tooltipInfo, Rect seriesBounds)
		{
			SetTooltipTargetRect(tooltipInfo, seriesBounds);
		}

		DataTemplate? ITooltipDependent.GetDefaultTooltipTemplate(TooltipInfo info)
		{
			return GetDefaultTooltipTemplate(info);
		}

		bool IDataTemplateDependent.IsTemplateItemsChanged()
		{
			if (Chart != null)
			{
				return Chart.IsRequiredDataLabelsMeasure;
			}

			return true;
		}

		#endregion

		#region Methods

		#region Public Methods

		/// <summary>
		/// Retrieves the index of a specific data point within a chart series, typically based on the interaction or coordinates on the chart.
		/// </summary>
		public virtual int GetDataPointIndex(float pointX, float pointY)
		{
			int selectedDataPointIndex = -1;
			RectF seriesBounds = AreaBounds;

			for (int i = 0; i < _segments.Count; i++)
			{
				ChartSegment chartSegment = _segments[i];
				selectedDataPointIndex = chartSegment.GetDataPointIndex(pointX - seriesBounds.Left, pointY - seriesBounds.Top);
				if (selectedDataPointIndex >= 0)
				{
					return selectedDataPointIndex;
				}
			}

			return selectedDataPointIndex;
		}

		#endregion

		#region Protected Methods

		/// <summary>
		/// Creates and initializes a new chart segment for the chart.
		/// </summary>
		protected abstract ChartSegment? CreateSegment();

		/// <summary>
		/// Creates animation for chart elements, such as series and data labels.
		/// </summary>
		protected virtual Animation? CreateAnimation(Action<double> callback)
		{
			return null;
		}

		/// <summary>
		/// Draws the data labels for the series.
		/// </summary>
		protected internal virtual void DrawDataLabel(ICanvas canvas, Brush? fillColor, string label, PointF point, int index)
		{
			ChartDataLabelStyle? dataLabelStyle = ChartDataLabelSettings?.LabelStyle;

			var fill = GetSegmentFillColor(index);

			if (!string.IsNullOrEmpty(label) && dataLabelStyle != null && ChartDataLabelSettings != null)
			{
				dataLabelStyle.DrawBackground(canvas, label, fillColor, point);

				ChartLabelStyle style = dataLabelStyle;
				Color fontColor = dataLabelStyle.TextColor;
				if (fontColor == null || fontColor == Colors.Transparent)
				{
					fontColor = ChartDataLabelSettings.GetContrastTextColor(this, fillColor, fill);
					var textColor = fontColor.WithAlpha(NeedToAnimateDataLabel ? AnimationValue : 1);

					//Created new font family, as need to pass contrast text color for native font family rendering.
					style = dataLabelStyle.Clone();
					style.TextColor = textColor;
				}

				style.DrawLabel(canvas, label, point);
			}

			if (dataLabelStyle?.Angle != 0)
			{
				canvas.CanvasRestoreState();
			}
		}

		/// <summary>
		/// Draw the series for the chart.
		/// </summary>
		protected internal virtual void DrawSeries(ICanvas canvas, ReadOnlyObservableCollection<ChartSegment> segments, RectF clipRect)
		{
#if WINDOWS
            foreach (var segment in segments)
            {
                canvas.SaveState();
                segment.Draw(canvas);
                canvas.RestoreState();
            }
#else
			canvas.CanvasSaveState();
			foreach (var segment in segments)
			{
				segment.Draw(canvas);
			}

			canvas.CanvasRestoreState();
#endif
		}

		/// <inheritdoc/>
		/// <exclude/>
		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();

			if (SelectionBehavior != null)
			{
				SetInheritedBindingContext(SelectionBehavior, BindingContext);
			}
		}

		/// <inheritdoc/>
		/// <exclude/>
		protected override void OnParentSet()
		{
			base.OnParentSet();

			if (ChartDataLabelSettings != null)
			{
				ChartDataLabelSettings.Parent = Parent;
			}
		}

		#endregion

		#region Internal methods

		internal virtual Brush? GetSegmentFillColor(int index)
		{
			var segment = _segments[index];

			if (segment != null)
			{
				return segment.Fill;
			}

			return null;
		}

		internal virtual void OnAttachedToChart(IChart? chart)
		{
			Chart = chart;
			SetInheritedBindingContext(this, (chart as Element)?.BindingContext);
			SegmentsCreated = false;
		}

		internal virtual void OnDetachedToChart()
		{
			ResetData();
			SegmentsCreated = false;
			Chart = null;
		}

		internal void Animate()
		{
			if (EnableAnimation && _segments != null && _segments.Count > 0)
			{
				SeriesView? seriesView = _segments[0].SeriesView;

				seriesView?.Animate();
			}
		}

		internal virtual void DrawDataLabels(ICanvas canvas)
		{
			var dataLabelSettings = ChartDataLabelSettings;

			if (dataLabelSettings == null)
			{
				return;
			}

			ChartDataLabelStyle labelStyle = dataLabelSettings.LabelStyle;

			foreach (ChartSegment segment in _segments)
			{
				if (!segment.InVisibleRange || segment.IsZero)
				{
					continue;
				}

				UpdateDataLabelAppearance(canvas, segment, dataLabelSettings, labelStyle);
			}
		}

		internal void UpdateDataLabelAppearance(ICanvas canvas, ChartSegment dataLabel, ChartDataLabelSettings dataLabelSettings, ChartDataLabelStyle labelStyle)
		{
			if (labelStyle.Angle != 0)
			{
				float angle = (float)(labelStyle.Angle > 360 ? labelStyle.Angle % 360 : labelStyle.Angle);
				canvas.CanvasSaveState();
				canvas.Rotate(angle, dataLabel.LabelPositionPoint.X, dataLabel.LabelPositionPoint.Y);
			}

			//Setting label background properties.
			//Todo: Need to confirm colors
			canvas.StrokeSize = (float)labelStyle.StrokeWidth;
			canvas.StrokeColor = labelStyle.Stroke.ToColor();

			//Setting label properties.
			var fillColor = labelStyle.IsBackgroundColorUpdated ? labelStyle.Background : dataLabelSettings.UseSeriesPalette ? dataLabel.Fill : labelStyle.Background;
			DrawDataLabel(canvas, fillColor, dataLabel.LabelContent ?? string.Empty, dataLabel.LabelPositionPoint, dataLabel.Index);
		}

		internal virtual PointF GetDataLabelPosition(ChartSegment dataLabel, SizeF labelSize, PointF labelPosition, float padding)
		{
			var dataLabelSettings = ChartDataLabelSettings;

			if (dataLabelSettings == null)
			{
				return labelPosition;
			}

			if (dataLabelSettings.LabelPlacement == DataLabelPlacement.Outer)
			{
				labelPosition.Y = labelPosition.Y - (labelSize.Height / 2) - padding;
			}
			else if (dataLabelSettings.LabelPlacement == DataLabelPlacement.Inner || (dataLabelSettings.LabelPlacement == DataLabelPlacement.Auto && this is ColumnSeries))
			{
				labelPosition.Y = labelPosition.Y + (labelSize.Height / 2) + padding;
			}

			return labelPosition;
		}

		internal virtual SizeF GetLabelTemplateSize(ChartSegment segment)
		{
			if (LabelTemplateView != null && LabelTemplateView.Count > segment.Index && LabelTemplateView[segment.Index] is DataLabelItemView templateView)
			{
				if (templateView != null && templateView.ContentView is View content)
				{
					if (!content.DesiredSize.IsZero)
					{
						return content.DesiredSize;
					}

#if NET9_0_OR_GREATER
                    var desiredSize = templateView.Measure(double.PositiveInfinity, double.PositiveInfinity);

                    if (desiredSize.IsZero)
					{
						return content.Measure(double.PositiveInfinity, double.PositiveInfinity);
					}
#else
					var desiredSize = templateView.Measure(double.PositiveInfinity, double.PositiveInfinity, MeasureFlags.IncludeMargins).Request;

					if (desiredSize.IsZero)
					{
						return content.Measure(double.PositiveInfinity, double.PositiveInfinity, MeasureFlags.IncludeMargins).Request;
					}
#endif

					return desiredSize;
				}
			}

			return SizeF.Zero;
		}

		internal void OnDataLabelSettingsPropertyChanged(ChartDataLabelSettings? oldValue, ChartDataLabelSettings? newValue)
		{
			if (oldValue != null)
			{
				oldValue.PropertyChanged -= DataLabelSettings_PropertyChanged;
				SetInheritedBindingContext(oldValue, null);
				oldValue.Parent = null;

				if (oldValue.LabelStyle != null)
				{
					oldValue.LabelStyle.PropertyChanged -= LabelStyle_PropertyChanged;
					SetInheritedBindingContext(oldValue.LabelStyle, null);
					oldValue.Parent = null;
				}
			}

			if (newValue != null)
			{
				newValue.PropertyChanged += DataLabelSettings_PropertyChanged;
				SetInheritedBindingContext(newValue, BindingContext);
				newValue.Parent = Parent;

				if (newValue.LabelStyle != null)
				{
					newValue.LabelStyle.PropertyChanged += LabelStyle_PropertyChanged;
					SetInheritedBindingContext(newValue.LabelStyle, BindingContext);
					newValue.LabelStyle.Parent = Parent;
				}
			}

			if (AreaBounds != Rect.Zero)
			{
				InvalidateMeasureDataLabel();
				InvalidateDataLabel();
			}
		}

		internal void DataLabelSettings_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (sender is ChartDataLabelSettings dataLabelSettings)
			{
				if (e.PropertyName != null && dataLabelSettings.IsNeedDataLabelMeasure.Contains(e.PropertyName))
				{
					InvalidateMeasureDataLabel();

					if (e.PropertyName == nameof(dataLabelSettings.LabelStyle))
					{
						dataLabelSettings.LabelStyle.PropertyChanged += LabelStyle_PropertyChanged;
						dataLabelSettings.LabelStyle.Parent = Parent;
					}
				}

				InvalidateDataLabel();
			}
		}

		internal void UnhookDataLabelEvents(ChartDataLabelSettings dataLabelSettings)
		{
			if (dataLabelSettings != null)
			{
				if (dataLabelSettings.LabelStyle != null)
				{
					dataLabelSettings.LabelStyle.PropertyChanged -= LabelStyle_PropertyChanged;
				}

				dataLabelSettings.PropertyChanged -= DataLabelSettings_PropertyChanged;
			}
		}

		internal virtual void InvalidateMeasureDataLabel()
		{
			foreach (var segment in _segments)
			{
				segment.OnDataLabelLayout();
			}
		}

		internal void InvalidateDataLabel()
		{
			if (_chart?.Area.PlotArea is IChartPlotArea chartPlotArea)
			{
				chartPlotArea.DataLabelView?.InvalidateDrawable();
			}
		}

		internal string GetLabelContent(double value, double sumOfValue)
		{
			string labelContent = string.Empty;

			if (LabelContext == LabelContext.Percentage)
			{
				var percentage = Math.Floor((value / sumOfValue * 100) * 100) / 100;
				labelContent = percentage.ToString() + "%";
			}
			else
			{
				if (ChartDataLabelSettings != null)
				{
					labelContent = ChartDataLabelSettings.GetLabelContent(value);
				}
			}

			return labelContent;
		}

		internal virtual DataTemplate? GetDefaultTooltipTemplate(TooltipInfo info)
		{
			return ChartUtils.GetDefaultTooltipTemplate(info);
		}

		internal virtual float TransformToVisibleX(double x, double y) => 0f;

		internal virtual float TransformToVisibleY(double x, double y) => 0f;

		internal virtual bool IsIndividualSegment()
		{
			return true;
		}

		internal virtual void OnSeriesLayout()
		{
		}

		internal virtual void UpdateLegendIconColor()
		{
		}

		internal virtual void UpdateLegendItemToggle()
		{
		}

		internal virtual void UpdateLegendIconColor(ChartSelectionBehavior sender, int index)
		{
		}

		internal virtual void SetStrokeColor(ChartSegment segment)
		{
		}

		internal virtual void SetStrokeWidth(ChartSegment segment)
		{
		}

		internal virtual void SetDashArray(ChartSegment segment)
		{
		}

		internal virtual Brush? GetFillColor(object item, int index)
		{
			Brush? fillColor;

			// Chart selection check. 
			fillColor = Chart?.GetSelectionBrush(this);

			//Series selection behavior check.
			fillColor ??= GetSelectionBrush(item, index);

			if (fillColor == null)
			{
				if (Fill != null)
				{
					fillColor = Fill;
				}
				else if (_paletteColors != null)
				{
					fillColor = _paletteColors.Count > 0 ? _paletteColors[index % _paletteColors.Count] : new SolidColorBrush(Colors.Transparent);
				}
			}

			return fillColor;
		}

		internal virtual bool SeriesContainsPoint(PointF point)
		{
			if (PointsCount == 0)
			{
				return false;
			}

			TooltipDataPointIndex = GetDataPointIndex(point.X, point.Y);

			return TooltipDataPointIndex < PointsCount && TooltipDataPointIndex > -1;
		}

		internal virtual TooltipInfo? GetTooltipInfo(ChartTooltipBehavior tooltipBehavior, float x, float y)
		{
			return null;
		}

		internal virtual void SetTooltipTargetRect(TooltipInfo tooltipInfo, Rect seriesBounds)
		{
			float xPosition = tooltipInfo.X;
			float yPosition = tooltipInfo.Y;
			float sizeValue = 1;
			float halfSizeValue = 0.5f;
			Rect targetRect = new Rect(xPosition - halfSizeValue, yPosition - halfSizeValue, sizeValue, sizeValue);
			tooltipInfo.TargetRect = targetRect;
		}

		internal virtual void GenerateSegments(SeriesView seriesView)
		{
		}

		/// <summary>
		/// Updates the tooltip appearance including background, text color, and font size.
		/// 
		/// ChartTooltipBehavior Background with AppThemeBinding · Issue #159 · syncfusion/maui-toolkit
		/// Resolved the issue where tooltip background doesn't update dynamically by changing the theme when using AppThemeBinding.
		/// </summary>
		internal void UpdateTooltipAppearance(TooltipInfo info, ChartTooltipBehavior tooltipBehavior)
		{
			if (Chart is ChartBase chart)
			{
				info.Background = tooltipBehavior.Background ?? chart.TooltipBackground ?? new SolidColorBrush(Color.FromArgb("#1C1B1F"));
				info.TextColor = tooltipBehavior.TextColor ?? chart.TooltipTextColor ?? Color.FromArgb("#F4EFF4");
				info.FontSize = !float.IsNaN(tooltipBehavior.FontSize) ? tooltipBehavior.FontSize : !float.IsNaN((float)chart.TooltipFontSize) ? (float)chart.TooltipFontSize : 14.0f;
			}
		}	

		internal virtual void InitiateDataLabels(ChartSegment segment)
		{
			if (DataLabels.Count > _segments.Count)
			{
				DataLabels.Clear();
			}

			var dataLabel = new ChartDataLabel();
			segment.DataLabels.Add(dataLabel);
			DataLabels.Add(dataLabel);
		}

		internal void UpdateColor()
		{
			foreach (var segment in _segments)
			{
				SetFillColor(segment);
			}
		}

		internal void UpdateStrokeColor()
		{
			foreach (var segment in _segments)
			{
				SetStrokeColor(segment);
			}
		}

		internal void UpdateStrokeWidth()
		{
			foreach (var segment in _segments)
			{
				SetStrokeWidth(segment);
			}
		}

		internal void UpdateDashArray()
		{
			foreach (var segment in _segments)
			{
				SetDashArray(segment);
			}
		}

		internal void AnimateSeries(Action<double> callback)
		{
			if (SeriesAnimation == null)
			{
				return;
			}

			Animation? customAnimation = CreateAnimation(callback);

			if (customAnimation != null)
			{
				SeriesAnimation.Add(0, 1, customAnimation);
			}
		}

		internal bool CanAnimate()
		{
			return EnableAnimation && NeedToAnimateSeries;
		}

		internal virtual void InvalidateSeries()
		{
			if (Chart?.Area.PlotArea is ChartPlotArea plotArea)
			{
				foreach (var view in plotArea._seriesViews.Children)
				{
					if (view is SeriesView seriesView && this == seriesView._series)
					{
						seriesView.InvalidateDrawable();
						break;
					}
				}
			}
		}

		internal void Invalidate()
		{
			if (Chart?.Area.PlotArea is ChartPlotArea plotArea)
			{
				foreach (var view in plotArea._seriesViews.Children)
				{
					if (view is SeriesView seriesView && this == seriesView._series)
					{
						seriesView.Invalidate();
						break;
					}
				}
			}
		}

		internal void ScheduleUpdateChart()
		{
			var area = _chart?.Area;
			if (area != null)
			{
				area.NeedsRelayout = true;
				area.ScheduleUpdateArea();
			}
		}

		internal void SetFillColor(ChartSegment segment)
		{
			if (segment == null)
			{
				return;
			}

			segment.Fill = GetFillColor(segment, segment.Index) ?? Brush.Transparent;
		}

		internal virtual Brush? GetSelectionBrush(object item, int index)
		{
			if (SelectionBehavior != null)
			{
				return SelectionBehavior.GetSelectionBrush(index);
			}

			return null;
		}

		internal bool SelectionHitTest(float x, float y)
		{
			if (SelectionBehavior != null && SelectionBehavior.Type != 0)
			{
				return SelectionBehavior.OnTapped(x, y);
			}

			return false;
		}

		internal virtual void UpdateLegendItems()
		{
			var area = _chart?.Area;
			if (area != null && !area.AreaBounds.IsEmpty)
			{
				area.PlotArea.ShouldPopulateLegendItems = true;
			}
		}

		internal virtual float SumOfValues(IList<double> Yvalues)
		{
			float sum = 0f;

			if (Yvalues != null)
			{
				if (float.IsNaN(_sumOfYValues))
				{
					foreach (double number in Yvalues)
					{
						if (!double.IsNaN(number))
						{
							sum += (float)number;
						}
					}
					_sumOfYValues = sum;
				}
				else
				{
					return _sumOfYValues;
				}
			}

			return sum;
		}

		internal virtual void UpdateEmptyPointSettings()
		{
		}

		#endregion

		#region Private Methods

		static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is ChartSeries chartSeries)
			{
				chartSeries.OnItemsSourceChanged(oldValue, newValue);
			}
		}

		static void OnXBindingPathChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is ChartSeries chartSeries)
			{
				if (newValue is string path)
				{
					chartSeries.XComplexPaths = path.Split(['.']);
				}

				chartSeries.OnBindingPathChanged();
			}
		}

		static void OnFillPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is ChartSeries chartSeries)
			{
				chartSeries.UpdateColor();
				chartSeries.InvalidateSeries();

				if (chartSeries.ShowDataLabels)
				{
					chartSeries.InvalidateDataLabel();
				}

				chartSeries.UpdateLegendIconColor();
			}
		}

		static void OnOpacityPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is ChartSeries chartSeries)
			{
				chartSeries.UpdateAlpha();
				chartSeries.InvalidateSeries();
			}
		}

		void UpdateAlpha()
		{
			foreach (var segment in _segments)
			{
				segment.Opacity = Math.Clamp((float)Opacity, 0, 1);
			}
		}

		static void OnPaletteBrushesChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (Equals(oldValue, newValue))
			{
				return;
			}

			if (bindable is ChartSeries chartSeries)
			{
				chartSeries._paletteColors = (IList<Brush>)newValue;

				chartSeries.OnCustomBrushesChanged(oldValue as ObservableCollection<Brush>, newValue as ObservableCollection<Brush>);

				if (chartSeries.AreaBounds != Rect.Zero)//Not to call at load time
				{
					chartSeries.PaletteColorsChanged();
				}
			}
		}

		static void OnVisiblePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			//TODO:Need to move this code to CartesianSeries class.
			if (bindable is ChartSeries chartSeries)
			{
				chartSeries.InvalidateGroupValues();

				if (chartSeries.IsSideBySide && chartSeries._chart?.Area is CartesianChartArea chartArea)
				{
					chartArea.RequiredAxisReset = true;
					chartArea.ResetSBSSegments();
				}

				chartSeries.UpdateLegendItemToggle();
				chartSeries.ScheduleUpdateChart();
			}
		}

		static void OnEnableTooltipPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
		}

		static void OnEnableAnimationPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			//Not implemented.
			//chartSeries.OnAnimationPropertyChanged();
		}

		static void OnSelectionBehaviorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is ChartSeries series)
			{
				if (newValue is DataPointSelectionBehavior selection)
				{
					selection.Source = series;
					SetInheritedBindingContext(selection, series.BindingContext);
				}

				if (oldValue is DataPointSelectionBehavior oldSelection)
				{
					SetInheritedBindingContext(oldSelection, null);
				}
			}

		}

		static void OnShowDataLabelsChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is ChartSeries chartSeries && chartSeries.Chart != null)
			{
				if (!(bool)newValue && chartSeries.LabelTemplate != null)
				{
					BindableLayout.SetItemsSource(chartSeries.LabelTemplateView, null);
				}

				if ((bool)newValue)
				{
					if (chartSeries.LabelTemplate != null)
					{
						BindableLayout.SetItemsSource(chartSeries.LabelTemplateView, chartSeries.DataLabels);
					}

					chartSeries.InvalidateMeasureDataLabel();
				}

				chartSeries.InvalidateDataLabel();
			}
		}

		static void OnLegendIconChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is ChartSeries chartSeries && chartSeries.Chart != null)
			{
				chartSeries.UpdateLegendItems(true);
			}
		}

		static void OnIsVisibleOnLegendChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is ChartSeries chartSeries && chartSeries.Chart != null)
			{
				chartSeries.UpdateLegendItems(true);
				chartSeries.ScheduleUpdateChart();
			}
		}

		static void OnLabelContextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is ChartSeries chartSeries)
			{
				if (chartSeries.ShowDataLabels)
				{
					chartSeries.InvalidateMeasureDataLabel();
					chartSeries.InvalidateDataLabel();
				}
			}
		}

		static void OnLabelTemplateChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is ChartSeries chartSeries && chartSeries.LabelTemplateView != null)
			{
				if (oldValue == null || newValue == null)
				{
					BindableLayout.SetItemsSource(chartSeries.LabelTemplateView, null);
				}

				if (newValue != null)
				{
					BindableLayout.SetItemsSource(chartSeries.LabelTemplateView, chartSeries.DataLabels);
				}

				if (chartSeries.Chart != null)
				{
					chartSeries.Chart.IsRequiredDataLabelsMeasure = true;
				}

				chartSeries.InvalidateMeasureDataLabel();
				chartSeries.InvalidateDataLabel();
			}
		}

		private static void OnListenPropertyChangeChanged(BindableObject bindable, object oldValue, object newValue)
		{
			((ChartSeries)bindable).HookPropertyChangedEvent((bool)newValue);
		}

		void OnItemsSourceChanged(object oldValue, object newValue)
		{
			if (Equals(oldValue, newValue))
			{
				return;
			}

			if (EnableAnimation && AnimationDuration > 0 && _segments != null && _segments.Count > 0)
			{
				OldSegments = new ObservableCollection<ChartSegment>(_segments);
				PreviousXRange = XRange;
				_segments[0].SeriesView?.AbortAnimation();
			}

			if (DataLabels.Count > 0)
			{
				DataLabels.Clear();
			}

			UpdateLegendItems();
			NeedToAnimateSeries = EnableAnimation;
			ResetData();
			OnDataSourceChanged(oldValue, newValue);
			HookAndUnhookCollectionChangedEvent(oldValue, newValue);
			UnhookPropertyChangedEvent(oldValue);
			SegmentsCreated = false;
			ScheduleUpdateChart();

			if (Chart != null)
			{
				Chart.IsRequiredDataLabelsMeasure = true;
			}
		}

		void Segments_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
		{
			e.ApplyCollectionChanges((obj, index, canInsert) => AddSegment(obj), (obj, index) => RemoveSegment(obj), ResetSegment);
		}

		void AddSegment(object chartSegment)
		{
			if (chartSegment is ChartSegment segment)
			{
				SetFillColor(segment);
				SetStrokeColor(segment);
				SetStrokeWidth(segment);
				SetDashArray(segment);
				segment.Opacity = (float)Opacity;
			}
		}

		static void RemoveSegment(object chartSegment)
		{
			//Todo: Need to consider this case later.
			//ToDo: Need to remove corresponding data label. 
		}

		void ResetSegment()
		{
			//Todo: Need to consider this case later.
		}

		void LabelStyle_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName != null && sender is ChartDataLabelStyle labelStyle && labelStyle.NeedDataLabelMeasure(e.PropertyName))
			{
				InvalidateMeasureDataLabel();
			}

			InvalidateDataLabel();
		}

		void UpdateLegendItems(bool isUpdate)
		{
			var area = _chart?.Area;

			if (area != null && !area.AreaBounds.IsEmpty)
			{
				area.PlotArea.ShouldPopulateLegendItems = isUpdate;
				area.PlotArea.UpdateLegendItems();
			}
		}

		void PaletteColorsChanged()
		{
			UpdateColor();
			InvalidateSeries();

			if (ShowDataLabels)
			{
				InvalidateDataLabel();
			}

			UpdateLegendIconColor();
		}

		void OnCustomBrushesChanged(ObservableCollection<Brush>? oldValue, ObservableCollection<Brush>? newValue)
		{
			if (oldValue != null)
			{
				oldValue.CollectionChanged -= CustomBrushes_CollectionChanged;
			}

			if (newValue != null)
			{
				newValue.CollectionChanged += CustomBrushes_CollectionChanged;
			}
		}

		void CustomBrushes_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
		{
			if (sender != null)
			{
				PaletteColorsChanged();
			}
		}

		#endregion

		#endregion

		#region Destructor

		/// <summary>
		/// Removed unmanaged resources
		/// </summary>
		/// <exclude/>
		~ChartSeries()
		{
			_segments.CollectionChanged -= Segments_CollectionChanged;
		}
		#endregion
	}
}
