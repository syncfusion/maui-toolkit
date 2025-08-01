using System.Collections.Specialized;

namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// <see cref="CartesianSeries"/> is the base class for all cartesian based series such as column, line, area, and so on.
	/// </summary>
	public abstract class CartesianSeries : ChartSeries
	{
		#region Fields

		ChartAxis? _actualXAxis;
		ChartAxis? _actualYAxis;
		double _xAxisMin = double.MaxValue;
		double _xAxisMax = double.MinValue;
		double _yAxisMin = double.MaxValue;
		double _yAxisMax = double.MinValue;

		#endregion

		#region Internal Properties

		internal EmptyPointSettings _emptyPointSettings;

		internal IList<int>[] EmptyPointIndexes { get; set; } = [];

		internal CartesianChartArea? ChartArea { get; set; }

		internal bool IsIndexed
		{
			get { return ActualXAxis is CategoryAxis || ActualXAxis is DateTimeCategoryAxis; }
		}

		internal DoubleRange SbsInfo { get; set; } = DoubleRange.Empty;

		internal int SideBySideIndex { get; set; }

		internal bool IsSbsValueCalculated { get; set; }

		internal override ChartDataLabelSettings ChartDataLabelSettings => DataLabelSettings;

		internal bool RequiredEmptyPointReset { get; set; } = false;
		internal virtual bool IsFillEmptyPoint { get { return true; } }

		#endregion

		#region Bindable properties

		/// <summary>
		/// Identifies the <see cref="EmptyPointMode"/> bindable property.
		/// </summary>
		/// <remarks>
		/// Represents how to calculate value for empty point in the series.
		/// </remarks>
		public static readonly BindableProperty EmptyPointModeProperty =
		   BindableProperty.Create(nameof(EmptyPointMode), typeof(EmptyPointMode), typeof(CartesianSeries), EmptyPointMode.None, BindingMode.Default, null, OnEmptyPointModeChanged);

		/// <summary>
		/// Identifies the <see cref="EmptyPointSettings"/> bindable property.
		/// </summary>
		/// <remarks>
		/// Provides customization for the empty points.
		/// </remarks>
		public static readonly BindableProperty EmptyPointSettingsProperty =
		   BindableProperty.Create(nameof(EmptyPointSettings), typeof(EmptyPointSettings), typeof(CartesianSeries), null, BindingMode.Default, null, propertyChanged: OnEmptyPointSettingsChanged);

		/// <summary>
		/// Identifies the <see cref="XAxisName"/> bindable property.
		/// </summary>
		/// <remarks>
		/// Represents the name of the X-axis for the cartesian series.
		/// </remarks>
		public static readonly BindableProperty XAxisNameProperty = BindableProperty.Create(
			nameof(XAxisName),
			typeof(string),
			typeof(CartesianSeries),
			null,
			BindingMode.Default,
			null,
			OnAxisNameChanged);

		/// <summary>
		/// Identifies the <see cref="YAxisName"/> bindable property.
		/// </summary>
		/// <remarks>
		/// Represents the name of the Y-axis for the cartesian series.
		/// </remarks>
		public static readonly BindableProperty YAxisNameProperty = BindableProperty.Create(
			nameof(YAxisName),
			typeof(string),
			typeof(CartesianSeries),
			null,
			BindingMode.Default,
			null,
			OnAxisNameChanged);

		/// <summary>
		/// Identifies the <see cref="DataLabelSettings"/> bindable property.
		/// </summary>
		/// <remarks>
		/// Provides customization for the data labels.
		/// </remarks>
		public static readonly BindableProperty DataLabelSettingsProperty = BindableProperty.Create(
			nameof(DataLabelSettings),
			typeof(CartesianDataLabelSettings),
			typeof(CartesianSeries),
			null,
			BindingMode.Default,
			null,
			OnDataLabelSettingsChanged);

		/// <summary>
		/// Identifies the <see cref="Label"/> bindable property.
		/// </summary>
		/// <remarks>
		/// Represents the label that will be displayed in the associated legend item.
		/// </remarks>
		public static readonly BindableProperty LabelProperty = BindableProperty.Create(
			nameof(Label),
			typeof(string),
			typeof(CartesianSeries),
			string.Empty,
			BindingMode.Default,
			null,
			OnLabelPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="ShowTrackballLabel"/> bindable property.
		/// </summary>
		/// <remarks>
		/// Indicates whether to display the trackball label in the cartesian series.
		/// </remarks>
		public static readonly BindableProperty ShowTrackballLabelProperty = BindableProperty.Create(
			nameof(ShowTrackballLabel),
			typeof(bool),
			typeof(CartesianSeries),
			true,
			BindingMode.Default,
			null);

		/// <summary>
		/// Identifies the <see cref="TrackballLabelTemplate"/> bindable property.
		/// </summary>
		/// <remarks>
		/// Provides a template for customizing the trackball label display in the cartesian series.
		/// </remarks>
		public static readonly BindableProperty TrackballLabelTemplateProperty = BindableProperty.Create(
			nameof(TrackballLabelTemplate),
			typeof(DataTemplate),
			typeof(CartesianSeries),
			null,
			BindingMode.Default,
			null,
			null);

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets the name of the (horizontal) axis in the <see cref="SfCartesianChart.XAxes"/> collection which is used to plot the series with particular axis.
		/// </summary>
		/// <value>It takes the <c>string</c> value and its default value is null.</value>
		/// <example>
		/// # [Xaml](#tab/tabid-1)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///     <chart:SfCartesianChart.XAxes>
		///        <chart:CategoryAxis x:Name="XAxis1"/>
		///        <chart:NumericalAxis x:Name="XAxis2"/>
		///    </chart:SfCartesianChart.XAxes>
		///    
		///    <chart:SfCartesianChart.YAxes>
		///       <chart:NumericalAxis />
		///   </chart:SfCartesianChart.YAxes>
		///
		///          <chart:SplineSeries ItemsSource="{Binding Data}"
		///                              XBindingPath="XValue"
		///                              YBindingPath="YValue" 
		///                              XAxisName="XAxis1" />
		///          <chart:ColumnSeries ItemsSource = "{Binding Data}"
		///                              XBindingPath="XValue"
		///                              YBindingPath="YValue"
		///                              XAxisName="XAxis2" />
		///
		///     </chart:SfCartesianChart>
		/// ]]></code>
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		///     SfCartesianChart chart = new SfCartesianChart();
		///     ViewModel viewModel = new ViewModel();
		///
		///     var XAxis1 = new CategoryAxis() { Name = "XAxis1" };
		///     var XAxis2 = new NumericalAxis() { Name = "XAxis2" }; 
		///     chart.XAxes.Add(XAxis1);
		///     chart.XAxes.Add(XAxis2);
		///     var YAxis = new NumericalAxis();
		///     chart.YAxes.Add(YAxis);
		///
		///     SplineSeries splineSeries = new SplineSeries()
		///     {
		///           ItemsSource = viewModel.Data,
		///           XBindingPath = "XValue",
		///           YBindingPath = "YValue",
		///           XAxisName = "XAxis1",
		///     };
		///     
		///     ColumnSeries columnSeries = new ColumnSeries()
		///     {
		///           ItemsSource = viewModel.Data,
		///           XBindingPath = "XValue",
		///           YBindingPath = "YValue",
		///           XAxisName = "XAxis2",
		///     };
		///     
		///     chart.Series.Add(splineSeries);
		///     chart.Series.Add(columnSeries);
		///
		/// ]]></code>
		/// ***
		/// </example>
		public string XAxisName
		{
			get { return (string)GetValue(XAxisNameProperty); }
			set { SetValue(XAxisNameProperty, value); }
		}

		/// <summary>
		/// Gets or sets the name of the (vertical) axis in the <see cref="SfCartesianChart.YAxes"/> collection which is used to plot the series with particular axis.
		/// </summary>
		/// <value>It takes the <c>string</c> value and its default value is null.</value>
		/// <example>
		/// # [Xaml](#tab/tabid-3)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///     <chart:SfCartesianChart.XAxes>
		///        <chart:CategoryAxis />
		///    </chart:SfCartesianChart.XAxes>
		///    
		///    <chart:SfCartesianChart.YAxes>
		///       <chart:NumericalAxis x:Name="YAxis1"/>
		///       <chart:NumericalAxis x:Name="YAxis2"/>
		///   </chart:SfCartesianChart.YAxes>
		///
		///          <chart:SplineSeries ItemsSource="{Binding Data}"
		///                              XBindingPath="XValue"
		///                              YBindingPath="YValue" 
		///                              YAxisName="YAxis1" />
		///          <chart:ColumnSeries ItemsSource = "{Binding Data}"
		///                              XBindingPath="XValue"
		///                              YBindingPath="YValue"
		///                              YAxisName="YAxis2" />
		///
		///     </chart:SfCartesianChart>
		/// ]]></code>
		/// # [C#](#tab/tabid-4)
		/// <code><![CDATA[
		///     SfCartesianChart chart = new SfCartesianChart();
		///     ViewModel viewModel = new ViewModel();
		///
		///     var XAxis = new CategoryAxis();
		///     chart.XAxes.Add(XAxis);
		///     var YAxis1 = new NumericalAxis(){Name = "YAxis1"};
		///     var YAxis2 = new NumericalAxis(){Name = "YAxis2"};
		///     chart.YAxes.Add(YAxis1);
		///     chart.YAxes.Add(YAxis2);
		///
		///     SplineSeries splineSeries = new SplineSeries()
		///     {
		///           ItemsSource = viewModel.Data,
		///           XBindingPath = "XValue",
		///           YBindingPath = "YValue",
		///           YAxisName = "YAxis1",
		///     };
		///     
		///     ColumnSeries columnSeries = new ColumnSeries()
		///     {
		///           ItemsSource = viewModel.Data,
		///           XBindingPath = "XValue",
		///           YBindingPath = "YValue",
		///           YAxisName = "YAxis2",
		///     };
		///     
		///     chart.Series.Add(splineSeries);
		///     chart.Series.Add(columnSeries);
		///
		/// ]]></code>
		/// ***
		/// </example>
		public string YAxisName
		{
			get { return (string)GetValue(YAxisNameProperty); }
			set { SetValue(YAxisNameProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value to customize the appearance of the displaying data labels in the cartesian series.
		/// </summary>
		/// <value>
		/// It accepts the <see cref="CartesianDataLabelSettings"/> values.
		/// </value>
		/// <remarks> This allows us to change the look of the displaying labels' content, and appearance at the data point.</remarks>
		/// <example>
		/// # [MainWindow.xaml](#tab/tabid-5)
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
		///                    <chart:ColumnSeries.DataLabelSettings>
		///                         <chart:CartesianDataLabelSettings BarAlignment="Middle" />
		///                    </ chart:ColumnSeries.DataLabelSettings>
		///               </chart:ColumnSeries> 
		///           </chart:SfCartesianChart.Series>
		///           
		///     </chart:SfCartesianChart>
		/// ]]></code>
		/// # [MainWindow.cs](#tab/tabid-6)
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
		///     
		///     series.DataLabelSettings = new CartesianDataLabelSettings()
		///     {
		///         BarAlignment = DataLabelAlignment.Middle,
		///     };
		///     chart.Series.Add(series);
		///     
		/// ]]></code>
		/// ***
		/// </example>
		public CartesianDataLabelSettings DataLabelSettings
		{
			get { return (CartesianDataLabelSettings)GetValue(DataLabelSettingsProperty); }
			set { SetValue(DataLabelSettingsProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value that will be displayed in the associated legend item.
		/// </summary>
		/// <value>It accepts a <c>string</c> value and its default value is <c>string.Empty</c>.</value>
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
		///                              Label = "ColumnSeries"/>
		///
		///     </chart:SfCartesianChart>
		/// ]]></code>
		/// # [C#](#tab/tabid-8)
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
		///           Label = "ColumnSeries",
		///     };
		///     
		///     chart.Series.Add(columnSeries);
		///
		/// ]]></code>
		/// ***
		/// </example>
		public string Label
		{
			get { return (string)GetValue(LabelProperty); }
			set { SetValue(LabelProperty, value); }
		}

		/// <summary>
		/// Gets or sets whether to show trackball label on the corresponding series
		/// </summary>
		/// <value>This property takes the <c>bool</c> as its value and its default value is true.</value>
		/// <example>
		/// # [MainWindow.xaml](#tab/tabid-9)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///     
		///           <chart:SfCartesianChart.TrackballBehavior>
		///                <chart:ChartTrackballBehavior  />
		///           </chart:SfCartesianChart.TrackballBehavior>
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
		///               <chart:LineSeries ItemsSource="{Binding Data}"
		///                                 XBindingPath="XValue"
		///                                 YBindingPath="YValue"
		///                                 ShowTrackballLabel = "True">
		///               </chart:LineSeries> 
		///           </chart:SfCartesianChart.Series>
		///           
		///     </chart:SfCartesianChart>
		/// ]]></code>
		/// # [MainPage.xaml.cs](#tab/tabid-10)
		/// <code><![CDATA[
		/// SfCartesianChart chart = new SfCartesianChart();
		/// 
		/// ChartTrackballBehavior trackballBehavior = new ChartTrackballBehavior();
		/// chart.TrackballBehavior = trackballBehavior;
		/// 
		///  NumericalAxis xAxis = new NumericalAxis();
		///  NumericalAxis yAxis = new NumericalAxis();
		///     
		///  chart.XAxes.Add(xAxis);
		///  chart.YAxes.Add(yAxis);
		/// 
		/// LineSeries series = new LineSeries();
		/// series.ItemsSource = new ViewModel().Data;
		/// series.XBindingPath = "XValue";
		/// series.YBindingPath = "YValue";
		/// series.ShowTrackballLabel = true;
		/// chart.Series.Add(series);
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
		/// Gets or sets the DataTemplate to customize the appearance of the corresponding series Trackball labels.
		/// </summary>
		/// <value>
		/// It accepts the <see cref="DataTemplate"/>value and its default value is null.
		/// </value>
		/// <example>
		/// # [MainWindow.xaml](#tab/tabid-11)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///     
		///           <chart:SfCartesianChart.TrackballBehavior>
		///                <chart:ChartTrackballBehavior  />
		///           </chart:SfCartesianChart.TrackballBehavior>
		///
		///           <chart:SfCartesianChart.Resources>
		///               <DataTemplate x:Key="TrackballTemplate">
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
		///               <chart:NumericalAxis/>
		///           </chart:SfCartesianChart.XAxes>
		///
		///           <chart:SfCartesianChart.YAxes>
		///               <chart:NumericalAxis/>
		///           </chart:SfCartesianChart.YAxes>
		///
		///           <chart:SfCartesianChart.Series>
		///               <chart:LineSeries ItemsSource="{Binding Data}"
		///                                   XBindingPath="XValue"
		///                                   YBindingPath="YValue"
		///                                   ShowTrackballLabel = "True"
		///                                   TrackballLabelTemplate="{StaticResource TrackballTemplate}">
		///               </chart:LineSeries> 
		///           </chart:SfCartesianChart.Series>
		///           
		///     </chart:SfCartesianChart>
		/// ]]></code>
		/// # [MainPage.xaml.cs](#tab/tabid-12)
		/// <code><![CDATA[
		/// SfCartesianChart chart = new SfCartesianChart();
		/// 
		/// ChartTrackballBehavior trackballBehavior = new ChartTrackballBehavior();
		/// chart.TrackballBehavior = trackballBehavior;
		/// 
		///  NumericalAxis xAxis = new NumericalAxis();
		///  NumericalAxis yAxis = new NumericalAxis();
		///     
		///  chart.XAxes.Add(xAxis);
		///  chart.YAxes.Add(yAxis);
		/// 
		/// LineSeries series = new LineSeries();
		/// series.ItemsSource = new ViewModel().Data;
		/// series.XBindingPath = "XValue";
		/// series.YBindingPath = "YValue";
		/// series.ShowTrackballLabel = true;
		///     
		/// DataTemplate labelTemplate = new DataTemplate(()=>
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
		/// series.TrackballLabelTemplate = labelTemplate;
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
		/// Gets or sets a value that indicates to displays NaN data points for the series. 
		/// </summary>
		/// <value> It accepts <see cref="EmptyPointMode"/> values and its default value is <see cref="EmptyPointMode.None"/>.</value>
		/// <remarks> Empty points are not supported for the Histogram and BoxAndWhisker series</remarks>
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
		///                              EmptyPointMode = "Zero"/>
		///
		///     </chart:SfCartesianChart>
		/// ]]></code>
		/// # [C#](#tab/tabid-14)
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
		///           EmptyPointMode = EmptyPointMode.Zero,
		///     };
		///     
		///     chart.Series.Add(columnSeries);
		///
		/// ]]></code>
		/// ***
		/// </example>
		public EmptyPointMode EmptyPointMode
		{
			get { return (EmptyPointMode)GetValue(EmptyPointModeProperty); }
			set { SetValue(EmptyPointModeProperty, value); }
		}

		/// <summary>
		/// Gets or sets the configuration for how empty or missing data points are handled and displayed within a chart series.
		/// </summary>
		/// <value>It accepts <see cref="EmptyPointSettings"/> values.</value>
		/// <remarks> <para> EmptyPointSettings is not supported for all area-related series, as well as for FastChart and ErrorBarSeries.</para> </remarks>
		/// <example>
		/// # [Xaml](#tab/tabid-15)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///     <!-- ... Eliminated for simplicity-->
		///
		///          <chart:LineSeries ItemsSource="{Binding Data}"
		///                            XBindingPath="XValue"
		///                            YBindingPath="YValue"
		///                            EmptyPointMode="Zero">
		///               <chart:LineSeries.EmptyPointSettings>
		///                     <chart:EmptyPointSettings Fill="Red"/>
		///               </chart:LineSeries.MarkerSettings>
		///          </chart:LineSeries>
		///
		///     </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [C#](#tab/tabid-16)
		/// <code><![CDATA[
		///     SfCartesianChart chart = new SfCartesianChart();
		///     ViewModel viewModel = new ViewModel();
		///
		///     // Eliminated for simplicity
		///
		///    EmptyPointSettings emptyPointSettings = new EmptyPointSettings()
		///    {
		///        Fill = new SolidColorBrush(Colors.Red), 
		///    };
		///     LineSeries series = new LineSeries()
		///     {
		///           ItemsSource = viewModel.Data,
		///           XBindingPath = "XValue",
		///           YBindingPath = "YValue",
		///           EmptyPointMode= EmptyPointMode.Zero,
		///           EmptyPointSettings= emptyPointSettings,
		///     };
		///     
		///     chart.Series.Add(series);
		///
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public EmptyPointSettings EmptyPointSettings
		{
			get { return (EmptyPointSettings)GetValue(EmptyPointSettingsProperty); }
			set { SetValue(EmptyPointSettingsProperty, value); }
		}

		/// <summary>
		/// Gets the actual XAxis value.
		/// </summary>
		public ChartAxis? ActualXAxis
		{
			get
			{
				return _actualXAxis;
			}

			internal set
			{
				if (_actualXAxis != null && value == null)
				{
					_actualXAxis.ClearRegisteredSeries();
				}

				if (_actualXAxis != value)
				{
					_actualXAxis = value;
				}
			}
		}

		/// <summary>
		/// Gets the actual YAxis value.
		/// </summary>
		public ChartAxis? ActualYAxis
		{
			get
			{
				return _actualYAxis;
			}

			internal set
			{
				if (_actualXAxis != null && value == null)
				{
					_actualXAxis.ClearRegisteredSeries();
				}

				if (_actualYAxis != value)
				{
					_actualYAxis = value;
				}
			}
		} 

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="CartesianSeries"/>.
		/// </summary>
		public CartesianSeries()
		{
			DataLabelSettings = new CartesianDataLabelSettings();
			_emptyPointSettings = new EmptyPointSettings();
		}

		#endregion

		#region Methods

		#region Public Methods

		/// <summary>
		/// Retrieves a list of data points that are present within the specified rectangle.
		/// </summary>
		public List<object>? GetDataPoints(Rect rectangle)
		{
			if (Chart == null || ActualXAxis == null || ActualYAxis == null)
			{
				return null;
			}

			double startX = double.NaN, startY = double.NaN, endX = double.NaN, endY = double.NaN;

			CartesianSeries.ConvertRectToValue(ref startX, ref endX, ref startY, ref endY, rectangle, ActualXAxis, ActualYAxis);

			return GetDataPoints(startX, endX, startY, endY);
		}

		/// <summary>
		/// Retrieves a list of data points present within the specified coordinate bounds.
		/// </summary>
		public List<object>? GetDataPoints(double startX, double endX, double startY, double endY)
		{
			var xValues = GetXValues();
			if (xValues == null || xValues.Count == 0)
			{
				return null;
			}

			int minimum = 0, maximum = xValues.Count - 1;

			if (IsLinearData)
			{
				CalculateNearestIndex(ref minimum, ref maximum, xValues, startX, endX);
			}

			return GetDataPoints(startX, endX, startY, endY, minimum, maximum, xValues, IsLinearData);
		}

		#endregion

		#region Protected Methods

		/// <inheritdoc/>
		protected override void OnParentSet()
		{
			base.OnParentSet();

			if (EmptyPointSettings != null)
			{
				EmptyPointSettings.Parent = Parent;
			}
		}

		/// <inheritdoc/>
		/// <exclude/>
		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();

			if (DataLabelSettings != null)
			{
				SetInheritedBindingContext(DataLabelSettings, BindingContext);
			}

			if (_emptyPointSettings != null)
			{
				SetInheritedBindingContext(_emptyPointSettings, BindingContext);
			}
		}

		#endregion

		#region Internal Methods

		internal override void UpdateLegendItems()
		{
			//Todo: While dynamically updating the ItemSource, the legend items not to be updated again
			//Task 866797 : When inheriting ObservableObject (https://learn.microsoft.com/en-us/dotnet/communitytoolkit/mvvm/observableobject#simple-property) in the ViewModel, the legend items not updated for load time.
			//Issue fixed PR: https://github.com/essential-studio/maui-charts/pull/1139
		}

		internal List<double>? CalculateControlPoints(IList<double> values, double yCoef, double nextyCoef, int i)
		{
			List<double> controlPoints = [];
			var xValues = GetXValues();

			if (xValues == null)
			{
				return null;
			}

			double yCoeff1 = yCoef;
			double yCoeff2 = nextyCoef;
			double x = xValues[i];
			double y = values[i];
			double nextX = xValues[i + 1];
			double nextY = values[i + 1];

			double one_third = 1 / 3.0;

			double deltaX2 = nextX - x;

			deltaX2 *= deltaX2;

			double dx1 = (2 * x) + nextX;
			double dx2 = x + (2 * nextX);

			double dy1 = (2 * y) + nextY;
			double dy2 = y + (2 * nextY);

			double y1 = one_third * (dy1 - (one_third * deltaX2 * (yCoeff1 + (0.5 * yCoeff2))));
			double y2 = one_third * (dy2 - (one_third * deltaX2 * ((0.5 * yCoeff1) + yCoeff2)));

			var startControlPointsX = dx1 * one_third;
			var startControlPointsY = y1;
			var endControlPointsX = dx2 * one_third;
			var endControlPointsY = y2;

			controlPoints.Add(startControlPointsX);
			controlPoints.Add(startControlPointsY);
			controlPoints.Add(endControlPointsX);
			controlPoints.Add(endControlPointsY);

			return controlPoints;
		}

		internal static List<double> CalculateControlPoints(double pointX, double pointY, double pointX1, double pointY1, double coefficientY, double coefficientY1)
		{
			return [pointX + (coefficientY / 3), pointY + (coefficientY / 3), pointX1 - (coefficientY1 / 3), pointY1 - (coefficientY1 / 3)];
		}

		internal static List<double> CalculateControlPoints(double pointX, double pointY, double pointX1, double pointY1, double coefficientY, double coefficientY1, double dx)
		{
			var value = dx / 3;

			return [pointX + value, pointY + (coefficientY * value), pointX1 - value, pointY1 - (coefficientY1 * value)];
		}

		internal override void LegendItemToggled(LegendItem legendItem)
		{
			IsVisible = !legendItem.IsToggled;
		}

		internal override float TransformToVisibleX(double x, double y)
		{
			if (ChartArea != null && ChartArea.IsTransposed)
			{
				if (ActualYAxis != null)
				{
					return ActualYAxis.ValueToPoint(y);
				}
			}
			else
			{
				if (ActualXAxis != null)
				{
					return ActualXAxis.ValueToPoint(x);
				}
			}

			return float.NaN;
		}

		internal override float TransformToVisibleY(double x, double y)
		{
			if (ChartArea != null && ChartArea.IsTransposed)
			{
				if (ActualXAxis != null)
				{
					return ActualXAxis.ValueToPoint(x);
				}
			}
			else
			{
				if (ActualYAxis != null)
				{
					return ActualYAxis.ValueToPoint(y);
				}
			}

			return float.NaN;
		}

		internal virtual double GetActualSpacing()
		{
			return 0;
		}

		internal virtual double GetActualWidth()
		{
			return 0;
		}

		internal void UpdateAssociatedAxes()
		{
			if (_actualXAxis != null && _actualYAxis != null)
			{
				if (!_actualXAxis.AssociatedAxes.Contains(_actualYAxis))
				{
					_actualXAxis.AssociatedAxes.Add(_actualYAxis);
				}

				if (!_actualYAxis.AssociatedAxes.Contains(_actualXAxis))
				{
					_actualYAxis.AssociatedAxes.Add(_actualXAxis);
				}
			}
		}

		internal override void UpdateRange()
		{
			if (ChartArea == null)
			{
				return;
			}

			VisibleXRange = XRange;
			VisibleYRange = YRange;

			if (PointsCount <= 0)
			{
				XRange = DoubleRange.Empty;
				YRange = DoubleRange.Empty;
				VisibleXRange = XRange;
				VisibleYRange = YRange;
			}
			else
			{
				if (_xAxisMin == double.MaxValue)
				{
					_xAxisMin = 0;
				}

				if (_xAxisMax == double.MinValue)
				{
					_xAxisMax = 0;
				}

				if (_yAxisMin == double.MaxValue)
				{
					_yAxisMin = 0;
				}

				if (_yAxisMax == double.MinValue)
				{
					_yAxisMax = 0;
				}

				//TODO: Need to remove use of chart area. 

				var sbsMinWidth = ChartArea != null && ChartArea.SideBySideMinWidth != double.MaxValue ? ChartArea.SideBySideMinWidth : 1;

				var diff = sbsMinWidth / 2;

				if (IsSideBySide && ItemsSource != null)
				{
					VisibleXRange = new DoubleRange(XRange.Start - diff, XRange.End + diff);

					if ((ActualXAxis is NumericalAxis axisNumeric &&
						 axisNumeric.RangePadding == NumericalPadding.None)
						||
						(ActualXAxis is DateTimeAxis axisDateTime &&
						axisDateTime.RangePadding == DateTimeRangePadding.None))
					{
						diff = sbsMinWidth / 2;
						VisibleXRange = new DoubleRange(VisibleXRange.Start + diff, VisibleXRange.End - diff);
					}
				}
				else if (PointsCount == 1 && ItemsSource != null && _segments != null && _segments.Count == 0)
				{
					var xValues = GetXValues();
					var yValues = ((XYDataSeries)this).YValues;

					if (xValues != null && xValues.Count > 0)
					{
						VisibleXRange = new DoubleRange(xValues[0], xValues[0]);
					}

					if (yValues != null && yValues.Count > 0)
					{
						VisibleYRange = new DoubleRange(yValues[0], yValues[0]);
					}
				}
			}
		}

		internal List<double>? GetXValues()
		{
			if (ActualXValues == null)
			{
				return null;
			}

			double xIndexValues = 0d;
			var xValues = ActualXValues as List<double>;

			if (IsIndexed || xValues == null)
			{
				if (ActualXAxis is CategoryAxis categoryAxis && !categoryAxis.ArrangeByIndex || ActualXAxis == null)
				{
					xValues = GroupedXValuesIndexes.Count > 0 ? GroupedXValuesIndexes : (from val in (ActualXValues as List<string>) select (xIndexValues++)).ToList();
				}
				else
				{
					xValues = xValues != null ? (from val in xValues select (xIndexValues++)).ToList() : (from val in (ActualXValues as List<string>) select (xIndexValues++)).ToList();
				}
			}

			return xValues;
		}

		internal override void OnDataSourceChanged(object oldValue, object newValue)
		{
			ResetAutoScroll();
			InvalidateSideBySideSeries();
			foreach (var item in EmptyPointIndexes)
			{
				item?.Clear();
			}

			base.OnDataSourceChanged(oldValue, newValue);
		}

		internal override void OnDataSource_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
		{
			ResetAutoScroll();
			base.OnDataSource_CollectionChanged(sender, e);
		}

		internal override void AddDataPoint(object data, int index, NotifyCollectionChangedEventArgs e)
		{
			ResetEmptyPointIndexes();
			foreach(var item in EmptyPointIndexes)
			{
				item?.Clear();
			}	

			base.AddDataPoint(data, index, e);
		}

		internal override void RemoveData(int index, NotifyCollectionChangedEventArgs e)
		{
			ResetEmptyPointIndexes();
			foreach (var item in EmptyPointIndexes)
			{
				item?.Clear();
			}

			base.RemoveData(index, e);
		}

		internal override void OnBindingPathChanged()
		{
			RequiredEmptyPointReset = true;
			base.OnBindingPathChanged();
		}

		internal void ResetAutoScroll()
		{
			if (ActualXAxis != null)
			{
				ActualXAxis.CanAutoScroll = true;
			}

			if (ActualYAxis != null)
			{
				ActualYAxis.CanAutoScroll = true;
			}
		}

		internal override void HookAndUnhookCollectionChangedEvent(object oldValue, object? newValue)
		{
			base.HookAndUnhookCollectionChangedEvent(oldValue, newValue);

			if (ChartArea != null)
			{
				ChartArea.SideBySideSeriesPosition = null;
			}
		}

		internal void InvalidateSideBySideSeries()
		{
			//TODO: Reset segments for side by side changes.

			if (ChartArea != null)
			{
				ChartArea.InvalidateMinWidth();

				if (ChartArea.PreviousSBSMinWidth != ChartArea.SideBySideMinWidth)
				{
					ChartArea.UpdateSBS();
				}
			}
		}

		internal void UpdateSbsSeries()
		{
			if (ChartArea != null)
			{
				var sideBySideSeries = ChartArea.VisibleSeries?.Where(series => series.IsSideBySide).ToList();

				if (sideBySideSeries != null && sideBySideSeries.Count > 0)
				{
					foreach (var chartSeries in sideBySideSeries)
					{
						chartSeries.SegmentsCreated = false;
					}
				}

				if (ChartArea.SideBySideSeriesPosition != null)
				{
					ChartArea.UpdateSBS();
				}

				ScheduleUpdateChart();
			}
		}

		internal override Brush? GetFillColor(object item, int index)
		{
			Brush? fillColor = base.GetFillColor(item, index);

			if (fillColor == null && ChartArea != null)
			{
				if (ChartArea.PaletteColors != null && ChartArea.PaletteColors.Count > 0)
				{
					if (ChartArea.Series is ChartSeriesCollection series)
					{
						var seriesIndex = series.IndexOf(this);

						if (seriesIndex >= 0)
						{
							fillColor = ChartArea.PaletteColors[seriesIndex % ChartArea.PaletteColors.Count];
						}
					}
				}
			}

			return fillColor ?? new SolidColorBrush(Colors.Transparent);
		}

		internal override Brush? GetSelectionBrush(object item, int index)
		{
			if (item is LegendItem)
			{
				return null;
			}

			return base.GetSelectionBrush(item, index);
		}

		internal override void OnAttachedToChart(IChart? chart)
		{
			base.OnAttachedToChart(chart);

			if (ChartArea != null)
			{
				ChartArea.RequiredAxisReset = true;
				ChartArea.SideBySideSeriesPosition = null;
			}

			ResetVisibleSeries();
			NeedToAnimateSeries = EnableAnimation;
		}

		internal override void OnDetachedToChart()
		{
			if (ChartArea != null)
			{
				ChartArea.RequiredAxisReset = true;

				if (IsSideBySide)
				{
					ChartArea.ResetSBSSegments();
				}
			}

			ActualXAxis = null;
			ActualYAxis = null;
			InvalidateGroupValues();
			ResetVisibleSeries();
			base.OnDetachedToChart();
		}

		void ResetVisibleSeries()
		{
			if (ChartArea != null)
			{
				var visibleSeries = ChartArea.VisibleSeries;

				if (visibleSeries == null)
				{
					return;
				}

				foreach (var chartSeries in visibleSeries)
				{
					chartSeries.SegmentsCreated = false;
				}
			}
		}

		internal virtual void CalculateDataPointPosition(int index, ref double x, ref double y)
		{
			if (ActualYAxis == null || ActualXAxis == null || ChartArea == null)
			{
				return;
			}

			double X = x;

			if (ActualXAxis != null && !double.IsNaN(x))
			{
				x = ChartArea.IsTransposed ? ActualYAxis.ValueToPoint(y) : ActualXAxis.ValueToPoint(X);
			}

			if (ActualYAxis != null && !double.IsNaN(y))
			{
				y = ChartArea.IsTransposed ? ActualXAxis != null ? ActualXAxis.ValueToPoint(X) : double.NaN : ActualYAxis.ValueToPoint(y);
			}
		}

		internal bool IsDataInVisibleRange(double xValue, double yValue)
		{
			if (ActualYAxis == null || ActualXAxis == null)
			{
				return false;
			}

			if (xValue < ActualXAxis.VisibleRange.Start || xValue > ActualXAxis.VisibleRange.End
						  || yValue < ActualYAxis.VisibleRange.Start || yValue > ActualYAxis.VisibleRange.End)
			{
				return false;
			}
			else
			{
				return true;
			}
		}

		internal override TooltipInfo? GetTooltipInfo(ChartTooltipBehavior tooltipBehavior, float x, float y)
		{
			if (_segments == null)
			{
				return null;
			}

			int index = IsSideBySide ? GetDataPointIndex(x, y) : SeriesContainsPoint(new PointF(x, y)) ? TooltipDataPointIndex : -1;

			if (index < 0 || ItemsSource == null || ActualData == null || ActualXAxis == null
				|| ActualYAxis == null || SeriesYValues == null)
			{
				return null;
			}

			var xValues = GetXValues();

			if (xValues == null || ChartArea == null)
			{
				return null;
			}

			object dataPoint = ActualData[index];
			double xValue = xValues[index];
			IList<double> yValues = SeriesYValues[0];
			double yValue = Convert.ToDouble(yValues[index]);
			float xPosition = TransformToVisibleX(xValue, yValue);

			if (!double.IsNaN(xPosition) && !double.IsNaN(yValue))
			{
				float yPosition = TransformToVisibleY(xValue, yValue);

				if (IsSideBySide)
				{
					double xMidVal = xValue + SbsInfo.Start + ((SbsInfo.End - SbsInfo.Start) / 2);
					xPosition = TransformToVisibleX(xMidVal, yValue);
					yPosition = TransformToVisibleY(xMidVal, yValue);
				}

				RectF seriesBounds = AreaBounds;
				seriesBounds = new RectF(0, 0, seriesBounds.Width, seriesBounds.Height);

				yPosition = seriesBounds.Top < yPosition ? yPosition : seriesBounds.Top;
				yPosition = seriesBounds.Bottom > yPosition ? yPosition : seriesBounds.Bottom;
				xPosition = seriesBounds.Left < xPosition ? xPosition : seriesBounds.Left;
				xPosition = seriesBounds.Right > xPosition ? xPosition : seriesBounds.Right;

				TooltipInfo tooltipInfo = new TooltipInfo(this)
				{
					X = xPosition,
					Y = yPosition,
					Index = index,
					Margin = tooltipBehavior.Margin,
					FontFamily = tooltipBehavior.FontFamily,
					FontAttributes = tooltipBehavior.FontAttributes,
					Text = yValue == 0 ? yValue.ToString("0.##") : yValue.ToString("#.##"),
					Item = dataPoint
				};

				UpdateTooltipAppearance(tooltipInfo, tooltipBehavior);

				return tooltipInfo;
			}

			return null;
		}

		internal override void SetTooltipTargetRect(TooltipInfo tooltipInfo, Rect seriesBounds)
		{
			float xPosition = tooltipInfo.X;
			float yPosition = tooltipInfo.Y;
			float sizeValue = 1;
			float halfSizeValue = 0.5f;

			Rect targetRect = tooltipInfo.TargetRect.IsEmpty ? new Rect(xPosition - halfSizeValue, yPosition, sizeValue, sizeValue) : tooltipInfo.TargetRect;

			if (tooltipInfo.TargetRect.IsEmpty)
			{

				if ((xPosition + seriesBounds.Left) == seriesBounds.Left)
				{
					targetRect = new Rect(xPosition - sizeValue, yPosition - halfSizeValue, sizeValue, sizeValue);
					tooltipInfo.Position = TooltipPosition.Right;
				}
				else if (xPosition == seriesBounds.Width)
				{
					targetRect = new Rect(xPosition + sizeValue, yPosition - halfSizeValue, sizeValue, sizeValue);
					tooltipInfo.Position = TooltipPosition.Left;
				}
				else if (yPosition == seriesBounds.Top)
				{
					targetRect = new Rect(xPosition - halfSizeValue, -sizeValue, sizeValue, sizeValue);
					tooltipInfo.Position = TooltipPosition.Bottom;
				}
			}
			else
			{
				var markerToolTip = tooltipInfo.TargetRect;

				if ((markerToolTip.X + markerToolTip.Width / 2 + seriesBounds.Left) == seriesBounds.Left)
				{
					targetRect = new Rect(markerToolTip.X + markerToolTip.Width / 2, markerToolTip.Y, markerToolTip.Width / 2, markerToolTip.Height);
					tooltipInfo.Position = TooltipPosition.Right;
				}
				else if ((markerToolTip.X + markerToolTip.Width / 2) == seriesBounds.Width)
				{
					tooltipInfo.Position = TooltipPosition.Left;
				}
			}

			tooltipInfo.TargetRect = targetRect;
		}

		internal override void UpdateLegendIconColor()
		{
			var legend = Chart?.Legend;
			var legendItems = ChartArea?.PlotArea.LegendItems;

			if (legend != null && legend.IsVisible && legendItems != null)
			{
				foreach (LegendItem legendItem in legendItems.Cast<LegendItem>())
				{
					if (legendItem != null && legendItem.Item == this)
					{
						legendItem.IconBrush = GetFillColor(legendItem, legendItem.Index) ?? new SolidColorBrush(Colors.Transparent);
						break;
					}
				}
			}
		}

		internal override void UpdateLegendItemToggle()
		{
			var legend = Chart?.Legend;
			var legendItems = ChartArea?.PlotArea.LegendItems;

			if (legend != null && legend.IsVisible && legendItems != null)
			{
				foreach (LegendItem legendItem in legendItems.Cast<LegendItem>())
				{
					if (legendItem != null && legendItem.Item == this)
					{
						legendItem.IsToggled = !IsVisible;
						break;
					}
				}
			}
		}

		internal override void UpdateLegendIconColor(ChartSelectionBehavior sender, int index)
		{
			if (sender is DataPointSelectionBehavior)
			{
				return;
			}

			var legend = Chart?.Legend;
			var legendItems = ChartArea?.PlotArea.LegendItems;

			if (legend != null && legend.IsVisible && legendItems != null && index < legendItems.Count)
			{
				if (legendItems[index] is LegendItem legendItem && legendItem.Item == this)
				{
					legendItem.IconBrush = GetFillColor(legendItem, index) ?? new SolidColorBrush(Colors.Transparent);
				}
			}
		}

		internal object? FindNearestChartPoint(float pointX, float pointY)
		{
			List<object> dataPointsList = FindNearestChartPoints(pointX, pointY);

			if (dataPointsList.Count > 0)
			{
				return dataPointsList[0];
			}
			else
			{
				return null;
			}
		}

		internal List<object> FindNearestChartPoints(float pointX, float pointY)
		{
			List<object> dataPointsList = [];
			double delta = 0;

			if (_actualXAxis != null && ChartArea != null)
			{
				double xStart = _actualXAxis.VisibleRange.Start;
				double xEnd = _actualXAxis.VisibleRange.End;
				//TODO: Need to recheck this case if the bounds is correct or not.
				double xValue = _actualXAxis.PointToValue(pointX - AreaBounds.Left, pointY - AreaBounds.Top);

				if (IsIndexed)
				{
					xValue = Math.Round(xValue);
					var isGrouped = ActualXAxis is CategoryAxis category && !category.ArrangeByIndex;
					int dataCount = isGrouped ? GroupedXValues.Count : PointsCount;

					if (xValue <= xEnd && xValue >= xStart && xValue < dataCount && xValue >= 0)
					{
						object? dataPoint;

						if (isGrouped)
						{
							dataPoint = GroupedActualData?[(int)xValue];
						}
						else
						{
							dataPoint = ActualData?[(int)xValue];
						}

						if (dataPoint != null)
						{
							dataPointsList.Add(dataPoint);
						}
					}
				}
				else
				{
					var xValues = GetXValues();
					if (xValues != null)
					{
						if (IsLinearData)
						{
							var index = ChartUtils.BinarySearch(xValues, xValue, 0, PointsCount - 1);
							var dataPoint = ActualData?[index];
							if (dataPoint != null)
							{
								dataPointsList.Add(dataPoint);
							}
						}
						else
						{
							double nearPointX = xStart;
							float xPoint = ChartArea.IsTransposed ? pointY : pointX;

							for (int i = 0; i < PointsCount; i++)
							{
								double currX = _actualXAxis.ValueToPoint(xValues[i]) + AreaBounds.Left;
								if (delta == xPoint - currX)
								{
									var dataPoint = ActualData?[i];
									if (dataPoint != null)
									{
										dataPointsList.Add(dataPoint);
									}
								}
								else if (Math.Abs(pointX - currX) <= Math.Abs(pointX - nearPointX))
								{
									nearPointX = currX;
									delta = pointX - currX;
									dataPointsList.Clear();
									var dataPoint = ActualData?[i];

									if (dataPoint != null)
									{
										dataPointsList.Add(dataPoint);
									}
								}
							}
						}
					}
				}
			}

			return dataPointsList;
		}

		internal virtual void GenerateTrackballPointInfo(List<object> nearestDataPoints, List<TrackballPointInfo> pointInfos, ref bool isSideBySide)
		{
			GeneratePointInfos(nearestDataPoints, pointInfos);
		}

		internal virtual void ApplyTrackballLabelFormat(TrackballPointInfo pointInfo, string labelFormat)
		{
			var label = pointInfo.YValues[0].ToString(labelFormat);
			pointInfo.Label = label;
		}

		internal TrackballPointInfo? CreateTrackballPointInfo(float x, float y, string label, object data)
		{
			if (Chart is SfCartesianChart chart)
			{
				TrackballPointInfo pointInfo = new TrackballPointInfo(this)
				{
					DataItem = data,
					X = x,
					Y = y,
					Label = label,
				};

				pointInfo.TooltipHelper.FontManager = chart.GetFontManager();

				return pointInfo;
			}

			return null;
		}

		internal virtual List<object>? GetDataPoints(double startX, double endX, double startY, double endY, int minimum, int maximum, List<double> xValues, bool validateYValues)
		{
			List<object> dataPoints = [];
			if (ActualSeriesYValues == null || ActualData == null || xValues.Count != ActualSeriesYValues[0].Count)
			{
				return null;
			}

			var yValues = ActualSeriesYValues[0];
			for (int i = minimum; i <= maximum; i++)
			{
				double xValue = xValues[i];
				if (validateYValues || (startX <= xValue && xValue <= endX))
				{
					double yValue = yValues[i];
					if (startY <= yValue && yValue <= endY)
					{
						dataPoints.Add(ActualData[i]);
					}
				}
			}

			return dataPoints;
		}

		internal double GetAxisCrossingValue(ChartAxis axis)
		{
			var area = ChartArea;

			if (area == null)
			{
				return 0;
			}

			var associatedAxis = axis.GetCrossingAxis(area);
			var crossingValue = axis.ActualCrossingValue == double.MinValue ? associatedAxis?.ActualRange.Start ?? 0 :
				axis.ActualCrossingValue == double.MaxValue ? associatedAxis?.ActualRange.End ?? 0 :
			double.IsNaN(axis.ActualCrossingValue) ? 0 : axis.ActualCrossingValue;
			axis.RenderingCrossesValue = crossingValue;
			return crossingValue;
		}

		#region Spline base

		internal double[]? GetMonotonicSpline(List<double> xValues, IList<double> yValues, out double[] dx)
		{
			int count = PointsCount, index = -1;
			dx = new double[count - 1];
			double[] slope = new double[count - 1];
			double[] coEfficient = new double[count];

			// Find the slope between the values.
			for (int i = 0; i < count - 1; i++)
			{
				if (!double.IsNaN(yValues[i + 1]) && !double.IsNaN(yValues[i])
					&& !double.IsNaN(xValues[i + 1]) && !double.IsNaN(xValues[i]))
				{
					dx[i] = xValues[i + 1] - xValues[i];
					slope[i] = (yValues[i + 1] - yValues[i]) / dx[i];
					if (double.IsInfinity(slope[i]))
					{
						slope[i] = 0;
					}
				}
			}

			// Add the first and last coEfficient value as Slope[0] and Slope[n-1]
			if (slope.Length == 0)
			{
				return null;
			}

			coEfficient[++index] = double.IsNaN(slope[0]) ? 0 : slope[0];

			for (int i = 0; i < dx.Length - 1; i++)
			{
				if (slope.Length > i + 1)
				{
					double m = slope[i], next = slope[i + 1];
					if (m * next <= 0)
					{
						coEfficient[++index] = 0;
					}
					else
					{
						if (dx[i] == 0)
						{
							coEfficient[++index] = 0;
						}
						else
						{
							double firstPoint = dx[i], nextPoint = dx[i + 1];
							double interPoint = firstPoint + nextPoint;
							coEfficient[++index] = 3 * interPoint / (((interPoint + nextPoint) / m) + ((interPoint + firstPoint) / next));
						}
					}
				}
			}

			coEfficient[++index] = double.IsNaN(slope[^1]) ? 0 : slope[^1];

			return coEfficient;
		}

		internal double[] GetCardinalSpline(List<double> xValues, IList<double> yValues)
		{
			int count = PointsCount;
			double[] tangentsX = new double[count];

			for (int i = 0; i < count; i++)
			{
				if (i == 0 && xValues.Count > 2)
				{
					tangentsX[i] = 0.5 * (xValues[i + 2] - xValues[i]);
				}
				else if (i == count - 1 && count - 3 >= 0)
				{
					tangentsX[i] = 0.5 * (xValues[count - 1] - xValues[count - 3]);
				}
				else if (i - 1 >= 0 && xValues.Count > i + 1)
				{
					tangentsX[i] = 0.5 * (xValues[i + 1] - xValues[i - 1]);
				}

				if (double.IsNaN(tangentsX[i]))
				{
					tangentsX[i] = 0;
				}
			}

			return tangentsX;
		}

		/// <summary>
		/// Calculate the co-efficient values for the natural Spline
		/// </summary>
		internal double[]? NaturalSpline(IList<double> values, SplineType splineType)
		{
			double a = 6;
			int count = PointsCount;
			var xValues = GetXValues();
			if (xValues == null)
			{
				return null;
			}

			double[] yCoeff = new double[count], u = new double[count];

			if (splineType == SplineType.Clamped && xValues.Count > 1)
			{
				u[0] = 0.5;
				double d0 = (xValues[1] - xValues[0]) / (values[1] - values[0]);
				double dn = (xValues[count - 1] - xValues[count - 2]) / (values[count - 1] - values[count - 2]);
				yCoeff[0] = (3 * (values[1] - values[0]) / (xValues[1] - xValues[0])) - (3 * d0);
				yCoeff[count - 1] = (3 * dn) - ((3 * (values[count - 1] - values[count - 2])) / (xValues[count - 1] - xValues[count - 2]));

				if (double.IsInfinity(yCoeff[0]) || double.IsNaN(yCoeff[0]))
				{
					yCoeff[0] = 0;
				}

				if (double.IsInfinity(yCoeff[count - 1]) || double.IsNaN(yCoeff[count - 1]))
				{
					yCoeff[count - 1] = 0;
				}
			}
			else
			{
				yCoeff[0] = u[0] = 0;
				yCoeff[count - 1] = 0;
			}

			for (int i = 1; i < count - 1; i++)
			{
				if (!double.IsNaN(values[i + 1]) && !double.IsNaN(values[i - 1]) && !double.IsNaN(values[i]))
				{
					double d1 = xValues[i] - xValues[i - 1];
					double d2 = xValues[i + 1] - xValues[i - 1];
					double d3 = xValues[i + 1] - xValues[i];
					double dy1 = values[i + 1] - values[i];
					double dy2 = values[i] - values[i - 1];
					if (xValues[i] == xValues[i - 1] || xValues[i] == xValues[i + 1])
					{
						yCoeff[i] = 0;
						u[i] = 0;
					}
					else
					{
						var p = 1 / ((d1 * yCoeff[i - 1]) + (2 * d2));
						yCoeff[i] = -p * d3;
						u[i] = p * ((a * ((dy1 / d3) - (dy2 / d1))) - (d1 * u[i - 1]));
					}
				}
			}

			for (int k = count - 2; k >= 0; k--)
			{
				yCoeff[k] = (yCoeff[k] * yCoeff[k + 1]) + u[k];
			}

			return yCoeff;
		}

		internal override void UpdateEmptyPointSettings()
		{
			if (!IsFillEmptyPoint || EmptyPointMode == EmptyPointMode.None)
			{
				return;
			}

			// Early return if no empty points or segments
			if (EmptyPointIndexes == null || _segments.Count == 0)
			{
				return;
			}

			// Cache the empty point settings for performance
			var fill = _emptyPointSettings?.Fill;
			var stroke = _emptyPointSettings?.Stroke;
			var strokeWidth = _emptyPointSettings?.StrokeWidth ?? 0;

			// Apply settings to all empty points
			foreach (var indexCollection in EmptyPointIndexes)
			{
				if (indexCollection == null)
				{
					continue;
				}

				foreach (var index in indexCollection)
				{
					// Bounds check to prevent index out of range exceptions
					if (index < 0 || index >= _segments.Count)
					{
						continue;
					}

					if (_segments[index] is CartesianSegment segment)
					{
						segment.Fill = fill;
						segment.Stroke = stroke;
						segment.StrokeWidth = strokeWidth;

						// Mark segment as empty if it's a average or zero mode, for custom drawing. 
						segment.IsEmpty = true;
					}
				}
			}
		}

		internal void ValidateDataPoints(params IList<double>[] yValues)
		{
			if (EmptyPointIndexes == null || EmptyPointIndexes.Length == 0)
			{
				EmptyPointIndexes = new List<int>[yValues.Length];
			}

			for (int i = 0; i < yValues.Length; i++)
			{
				var values = yValues[i];

				if (EmptyPointIndexes != null && EmptyPointIndexes[i] == null)
				{
					EmptyPointIndexes[i] = [];
				}

				if (values.Count != 0)
				{
					switch (EmptyPointMode)
					{
						case EmptyPointMode.Zero:
							HandleZeroMode(values, i);
							break;

						case EmptyPointMode.Average:
							HandleAverageMode(values, i);
							break;

						default:
							break;
					}
				}

				yValues[i] = values;
			}
		}

		internal virtual void ResetEmptyPointIndexes()
		{
		}

		internal virtual void ValidateYValues()
		{
		}

		#endregion

		#endregion

		#region Private Methods

		void HandleZeroMode(IList<double> values, int emptyPointIndex)
		{
			for (int i = 0; i < values.Count; i++)
			{
				if (double.IsNaN(values[i]))
				{
					values[i] = 0;
					if (!EmptyPointIndexes[emptyPointIndex].Contains(i))
					{
						EmptyPointIndexes[emptyPointIndex].Add(i);
					}
				}
			}
		}

		void HandleAverageMode(IList<double> values, int emptyPointIndex)
		{
			int j = 0;
			//Single Data point
			if (values.Count == 1 && double.IsNaN(values[j]))
			{
				values[j] = 0;
				EmptyPointIndexes[emptyPointIndex].Add(0);
				return;
			}

			//First data point
			if (double.IsNaN(values[j]))
			{
				values[j] = (0 + (double.IsNaN(values[j + 1]) ? 0 : values[j + 1])) / 2;
				if (!EmptyPointIndexes[emptyPointIndex].Contains(j))
				{
					EmptyPointIndexes[emptyPointIndex].Add(0);
				}
			}

			//Middle data points
			for (; j < values.Count - 1; j++)
			{
				if (double.IsNaN(values[j]))
				{
					values[j] = (values[j - 1] + (double.IsNaN(values[j + 1]) ? 0 : values[j + 1])) / 2;
					if (!EmptyPointIndexes[emptyPointIndex].Contains(j))
					{
						EmptyPointIndexes[emptyPointIndex].Add(j);
					}
				}
			}

			//Last data point
			if (double.IsNaN(values[j]))
			{
				values[j] = values[j - 1] / 2;
				if (!EmptyPointIndexes[emptyPointIndex].Contains(j))
				{
					EmptyPointIndexes[emptyPointIndex].Add(j);
				}
			}
		}

		static void OnEmptyPointSettingsChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is CartesianSeries series)
			{
				series.OnEmptyPointSettingsChanged(oldValue, newValue);
			}
		}

		void OnEmptyPointSettingsChanged(object oldValue, object newValue)
		{
			// Clean up old settings
			if (oldValue is EmptyPointSettings oldSettings)
			{
				oldSettings.PropertyChanged -= EmptyPointSettings_PropertyChanged;
				SetInheritedBindingContext(oldSettings, null);
				oldSettings.Parent = null;
			}

			// Initialize new settings
			EmptyPointSettings settingsToUse;

			if (newValue is EmptyPointSettings newSettings)
			{
				settingsToUse = newSettings;
				_emptyPointSettings = newSettings;
			}
			else
			{
				settingsToUse = new EmptyPointSettings();
				_emptyPointSettings = settingsToUse;
			}

			// Configure the settings to use
			settingsToUse.PropertyChanged += EmptyPointSettings_PropertyChanged;
			if (Parent != null)
			{
				settingsToUse.Parent = Parent;
			}

			UpdateEmptyPointSettings();

			SetInheritedBindingContext(settingsToUse, BindingContext);

			InValidateViews();
		}

		void EmptyPointSettings_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			UpdateEmptyPointSettings();
			InValidateViews();
		}

		static void OnEmptyPointModeChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is CartesianSeries series)
			{
				series.SegmentsCreated = false;
				if (series.SeriesYValues != null)
				{
					series.RequiredEmptyPointReset = true;
				}

				series.ScheduleUpdateChart();
			}
		}

		void GeneratePointInfos(List<object> nearestDataPoints, List<TrackballPointInfo> pointInfos)
		{
			var xValues = GetXValues();

			if (nearestDataPoints != null && ActualData != null && xValues != null && SeriesYValues != null)
			{
				IList<double> yValues = SeriesYValues[0];
				foreach (object point in nearestDataPoints)
				{
					int index = ActualData.IndexOf(point);
					if (index == -1)
					{
						continue;
					}

					var xValue = xValues[index];
					double yValue = yValues[index];
					string label = yValue.ToString();
					var xPoint = TransformToVisibleX(xValue, yValue);
					var yPoint = TransformToVisibleY(xValue, yValue);

					// Checking YValue is contain in plotArea
					//Todo: need to check with transposed
					//if (!AreaBounds.Contains(xPoint + AreaBounds.Left, yPoint + AreaBounds.Top)) continue;

					TrackballPointInfo? chartPointInfo = CreateTrackballPointInfo(xPoint, yPoint, label, point);

					if (chartPointInfo != null)
					{
						chartPointInfo.XValue = xValue;
						chartPointInfo.YValues.Add(yValue);
						pointInfos.Add(chartPointInfo);
					}
				}
			}
		}

		static void CalculateNearestIndex(ref int minimum, ref int maximum, List<double> xValues, double startX, double endX)
		{
			minimum = ChartUtils.BinarySearch(xValues, startX, 0, maximum);
			maximum = ChartUtils.BinarySearch(xValues, endX, 0, maximum);
			minimum = startX <= xValues[minimum] ? minimum : minimum + 1;
			maximum = endX >= xValues[maximum] ? maximum : maximum - 1;
		}

		static void ConvertRectToValue(ref double startX, ref double endX, ref double startY, ref double endY, Rect rect, ChartAxis actualXAxis, ChartAxis actualYAxis)
		{
			bool isVertical = actualXAxis.IsVertical;

			startX = actualXAxis.PointToValue(rect.X, rect.Y);
			endX = isVertical ? actualXAxis.PointToValue(rect.X, rect.Y + rect.Height) :
				actualXAxis.PointToValue(rect.X + rect.Width, rect.Y);

			startY = actualYAxis.PointToValue(rect.X, rect.Y);
			endY = isVertical ? actualYAxis.PointToValue(rect.X + rect.Width, rect.Y) :
				actualYAxis.PointToValue(rect.X, rect.Y + rect.Height);

			if (startX > endX)
			{
				(startX, endX) = (endX, startX);
			}

			if (startY > endY)
			{
				(startY, endY) = (endY, startY);
			}
		}

		static void OnDataLabelSettingsChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var chartSeries = bindable as CartesianSeries;

			chartSeries?.OnDataLabelSettingsPropertyChanged(oldValue as ChartDataLabelSettings, newValue as ChartDataLabelSettings);
		}

		static void OnAxisNameChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is CartesianSeries series && series.ChartArea != null)
			{
				series.ChartArea.RequiredAxisReset = true;
				series.ScheduleUpdateChart();
			}
		}

		static void OnLabelPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is CartesianSeries chartSeries)
			{
				var legendItems = chartSeries.ChartArea?.PlotArea.LegendItems;

				if (legendItems != null)
				{
					foreach (LegendItem legendItem in legendItems.Cast<LegendItem>())
					{
						if (legendItem != null && legendItem.Item == chartSeries)
						{
							legendItem.Text = chartSeries.Label;
							break;
						}
					}
				}
			}
		}

		#endregion

		#endregion
	}
}