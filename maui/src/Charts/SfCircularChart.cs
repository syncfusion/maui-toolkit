using Syncfusion.Maui.Toolkit.Themes;
using Syncfusion.Maui.Toolkit.Internals;
using PointerEventArgs = Syncfusion.Maui.Toolkit.Internals.PointerEventArgs;

namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// Renders different types of circular charts, including doughnut, pie and radial bar charts. Each chart type presents data in a unique way, designed to enhance user-friendliness.
	/// </summary>
	/// <remarks>
	/// <para>Circular chart control is used to visualize the data graphically and to display the data with proportions and percentage of different categories.</para>
	///
	/// <para>SfCircularChart class properties provides an option to add the series collection, allows to customize the chart elements such as legend, data label, and tooltip features. </para>
	///
	/// <img src="https://cdn.syncfusion.com/content/images/maui/MAUI_Pie.jpg"/>
	/// 
	/// <para><b>Series</b></para>
	/// 
	/// <para>ChartSeries is the visual representation of data. SfCircularChart offers <see cref="PieSeries"/> and <see cref="DoughnutSeries"/>.</para>
	/// 
	/// <para>To render a series, create an instance of required series class, and add it to the <see cref="Series"/> collection.</para>
	/// 
	/// # [MainPage.xaml](#tab/tabid-1)
	/// <code> <![CDATA[
	/// <chart:SfCircularChart>
	///
	///        <chart:SfCircularChart.BindingContext>
	///            <local:ViewModel/>
	///        </chart:SfCircularChart.BindingContext>
	///    
	///        <chart:SfCircularChart.Series>
	///            <chart:PieSeries ItemsSource = "{Binding Data}" XBindingPath="XValue" YBindingPath="YValue"/>
	///        </chart:SfCircularChart.Series>
	/// </chart:SfCircularChart>
	/// ]]>
	/// </code>
	/// # [MainPage.xaml.cs](#tab/tabid-2)
	/// <code><![CDATA[
	/// SfCircularChart chart = new SfCircularChart();
	/// 
	/// ViewModel viewModel = new ViewModel();
	///	chart.BindingContext = viewModel;
	/// 
	/// PieSeries series = new PieSeries()
	/// {
	///     ItemsSource = viewModel.Data,
	///     XBindingPath = "XValue",
	///     YBindingPath = "YValue"
	/// };
	/// chart.Series.Add(series);
	/// ]]>
	/// </code>
	/// # [ViewModel.cs](#tab/tabid-3)
	/// <code><![CDATA[
	/// public ObservableCollection<Model> Data { get; set; }
	/// 
	/// public ViewModel()
	/// {
	///    Data = new ObservableCollection<Model>();
	///    Data.Add(new Model() { XValue = "A", YValue = 100 });
	///    Data.Add(new Model() { XValue = "B", YValue = 150 });
	///    Data.Add(new Model() { XValue = "C", YValue = 110 });
	///    Data.Add(new Model() { XValue = "D", YValue = 230 });
	/// }
	/// ]]>
	/// </code>
	/// ***
	/// 
	/// <para><b>Legend</b></para>
	/// 
	/// <para>The Legend contains list of data points in chart series. The information provided in each legend item helps to identify the corresponding data point in chart series. The Series <see cref="ChartSeries.XBindingPath"/> property value will be displayed in the associated legend item.</para>
	/// 
	/// <para>To render a legend, create an instance of <see cref="ChartLegend"/>, and assign it to the <see cref="ChartBase.Legend"/> property. </para>
	/// 
	/// # [MainPage.xaml](#tab/tabid-4)
	/// <code> <![CDATA[
	/// <chart:SfCircularChart>
	///
	///        <chart:SfCircularChart.BindingContext>
	///            <local:ViewModel/>
	///        </chart:SfCircularChart.BindingContext>
	///        
	///        <chart:SfCircularChart.Legend>
	///            <chart:ChartLegend/>
	///        </chart:SfCircularChart.Legend>
	///
	///        <chart:SfCircularChart.Series>
	///            <chart:PieSeries ItemsSource = "{Binding Data}" XBindingPath="XValue" YBindingPath="YValue"/>
	///        </chart:SfCircularChart.Series>
	/// </chart:SfCircularChart>
	/// ]]>
	/// </code>
	/// # [MainPage.xaml.cs](#tab/tabid-5)
	/// <code><![CDATA[
	/// SfCircularChart chart = new SfCircularChart();
	/// 
	/// ViewModel viewModel = new ViewModel();
	///	chart.BindingContext = viewModel;
	///	
	/// chart.Legend = new ChartLegend();
	/// 
	/// PieSeries series = new PieSeries()
	/// {
	///     ItemsSource = viewModel.Data,
	///     XBindingPath = "XValue",
	///     YBindingPath = "YValue"
	/// };
	/// chart.Series.Add(series);
	/// ]]>
	/// </code>
	/// ***
	/// 
	/// <para><b>Tooltip</b></para>
	/// 
	/// <para>Tooltip displays information while tapping or mouse hover on the segment. To display the tooltip on the chart, you need to set the <see cref="ChartSeries.EnableTooltip"/> property as <b>true</b> in <see cref="ChartSeries"/>. </para>
	/// 
	/// <para>To customize the appearance of the tooltip elements like Background, TextColor and Font, create an instance of <see cref="ChartTooltipBehavior"/> class, modify the values, and assign it to the chart’s <see cref="ChartBase.TooltipBehavior"/> property. </para>
	/// 
	/// # [MainPage.xaml](#tab/tabid-6)
	/// <code><![CDATA[
	/// <chart:SfCircularChart>
	/// 
	///         <chart:SfCircularChart.BindingContext>
	///             <local:ViewModel/>
	///         </chart:SfCircularChart.BindingContext>
	/// 
	///         <chart:SfCircularChart.TooltipBehavior>
	///             <chart:ChartTooltipBehavior/>
	///         </chart:SfCircularChart.TooltipBehavior>
	/// 
	///         <chart:SfCircularChart.Series>
	///             <chart:PieSeries EnableTooltip = "True"
	///                              ItemsSource="{Binding Data}"
	///                              XBindingPath="XValue"
	///                              YBindingPath="YValue"/>
	///         </chart:SfCircularChart.Series>
	/// 
	/// </chart:SfCircularChart>
	/// ]]>
	/// </code>
	/// # [MainPage.xaml.cs](#tab/tabid-7)
	/// <code><![CDATA[
	/// SfCircularChart chart = new SfCircularChart();
	/// 
	/// ViewModel viewModel = new ViewModel();
	///	chart.BindingContext = viewModel;
	///
	/// chart.TooltipBehavior = new ChartTooltipBehavior();
	///
	/// PieSeries series = new PieSeries()
	/// {
	///    ItemsSource = viewModel.Data,
	///    XBindingPath = "XValue",
	///    YBindingPath = "YValue",
	///    EnableTooltip = true
	/// };
	/// chart.Series.Add(series);
	///
	/// ]]>
	/// </code>
	/// ***
	/// 
	/// <para><b>Data Label</b></para>
	/// 
	/// <para>Data labels are used to display values related to a chart segment. To render the data labels, you need to enable the <see cref="ChartSeries.ShowDataLabels"/> property as <b>true</b> in <b>ChartSeries</b> class. </para>
	/// 
	/// <para>To customize the chart data labels alignment, placement and label styles, need to create an instance of <see cref="CircularDataLabelSettings"/> and set to the <see cref="CircularSeries.DataLabelSettings"/> property.</para>
	/// 
	/// # [MainPage.xaml](#tab/tabid-8)
	/// <code><![CDATA[
	/// <chart:SfCircularChart>
	///
	///        <chart:SfCircularChart.BindingContext>
	///            <local:ViewModel/>
	///        </chart:SfCircularChart.BindingContext>
	///    
	///        <chart:SfCircularChart.Series>
	///            <chart:DoughnutSeries ShowDataLabels = "True"
	///                                  ItemsSource="{Binding Data}"
	///                                  XBindingPath="XValue"
	///                                  YBindingPath="YValue"/>
	///        </chart:SfCircularChart.Series>
	/// </chart:SfCircularChart>
	///
	/// ]]>
	/// </code>
	/// # [MainPage.xaml.cs](#tab/tabid-9)
	/// <code><![CDATA[
	/// SfCircularChart chart = new SfCircularChart();
	/// 
	/// ViewModel viewModel = new ViewModel();
	///	chart.BindingContext = viewModel;
	/// 
	/// DoughnutSeries series = new DoughnutSeries()
	/// {
	///     ItemsSource = viewModel.Data,
	///     XBindingPath = "XValue",
	///     YBindingPath = "YValue",
	///     ShowDataLabels = true
	/// };
	/// chart.Series.Add(series);
	/// ]]>
	/// </code>
	/// ***
	/// </remarks>
	[ContentProperty(nameof(Series))]
	public partial class SfCircularChart : ChartBase, ITouchListener, ITapGestureListener, IParentThemeElement
	{
		#region Fields

		internal readonly CircularChartArea _chartArea;

		#endregion

		#region Bindable Properties

		/// <summary>
		/// Identifies the <see cref="Series"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="Series"/> bindable property determines the collection 
		/// of series in the circular chart.
		/// </remarks>
		public static readonly BindableProperty SeriesProperty = BindableProperty.Create(
			nameof(Series),
			typeof(ChartSeriesCollection),
			typeof(SfCircularChart),
			null,
			BindingMode.Default,
			null,
			OnSeriesPropertyChanged);

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets a collection of chart series to be added to the circular chart.
		/// </summary>
		/// <value>This property takes <see cref="ChartSeriesCollection"/> instance as value.</value>
		/// <example>
		/// # [Xaml](#tab/tabid-10)
		/// <code><![CDATA[
		///     <chart:SfCircularChart>
		///     
		///           <chart:SfCircularChart.BindingContext>
		///               <local:ViewModel/>
		///           </chart:SfCircularChart.BindingContext>
		///
		///           <chart:SfCircularChart.Series>
		///               <chart:DoughnutSeries
		///                   ItemsSource="{Binding Data}"
		///                   XBindingPath="XValue"
		///                   YBindingPath="YValue"/>
		///           </chart:SfCircularChart.Series>  
		///           
		///     </chart:SfCircularChart>
		/// ]]></code>
		/// # [C#](#tab/tabid-11)
		/// <code><![CDATA[
		///     SfCircularChart chart = new SfCircularChart();
		///     
		///     ViewModel viewModel = new ViewModel();
		///     chart.BindingContext = viewModel;
		///     
		///     DoughnutSeries series = new DoughnutSeries();
		///     series.ItemsSource = viewModel.Data;
		///     series.XBindingPath = "XValue";
		///     series.YBindingPath = "YValue";
		///     chart.Series.Add(series);
		///     
		/// ]]></code>
		/// # [ViewModel](#tab/tabid-12)
		/// <code><![CDATA[
		/// public ObservableCollection<Model> Data { get; set; }
		/// 
		/// public ViewModel()
		/// {
		///    Data = new ObservableCollection<Model>();
		///    Data.Add(new Model() { XValue = "A", YValue = 100 });
		///    Data.Add(new Model() { XValue = "B", YValue = 150 });
		///    Data.Add(new Model() { XValue = "C", YValue = 110 });
		///    Data.Add(new Model() { XValue = "D", YValue = 230 });
		/// }
		/// ]]></code>
		/// ***
		/// </example>
		/// <remarks><para>To render a series, create an instance of required series class, and add it to the <see cref="Series"/> collection.</para></remarks>
		public ChartSeriesCollection Series
		{
			get { return (ChartSeriesCollection)GetValue(SeriesProperty); }
			set { SetValue(SeriesProperty, value); }
		}

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="SfCircularChart"/> class.
		/// </summary>
		public SfCircularChart()
		{
			ThemeElement.InitializeThemeResources(this, "SfCircularChartTheme");
			SfCircularChartResources.InitializeDefaultResource("Chart.Resources.SfCircularChart");
			SetDynamicResource(TooltipBackgroundProperty, "SfCircularChartTooltipBackground");
			SetDynamicResource(TooltipTextColorProperty, "SfCircularChartTooltipTextColor");
			SetDynamicResource(TooltipFontSizeProperty, "SfCircularChartTooltipTextFontSize");
			_chartArea = (CircularChartArea)_legendLayout._areaBase;
			Series = [];
			this.AddGestureListener(this);
			this.AddTouchListener(this);
		}

		internal override AreaBase CreateChartArea()
		{
			return new CircularChartArea(this);
		}

		#endregion

		#region Methods

		#region Theme Interface Implementation

		ResourceDictionary IParentThemeElement.GetThemeDictionary()
		{
			return new SfCircularChartStyles();
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

		#region Protected Methods

		/// <inheritdoc/>
		/// <exclude/>
		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();

			if (Series != null)
			{
				//Todo: Need to cross check and remove this method.
				foreach (CircularSeries series in Series.Cast<CircularSeries>())
				{
					SetInheritedBindingContext(series, BindingContext);
				}
			}
		}

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

		#region Internal Methods

		internal void OnTapAction(IChart chart, Point tapPoint, int tapCount)
		{
			var tooltipBehavior = chart.ActualTooltipBehavior;
			tooltipBehavior?.OnTapped(this, tapPoint, tapCount);

			var visibleSeries = _chartArea.VisibleSeries;
			if (visibleSeries != null)
			{
				for (int i = visibleSeries.Count - 1; i >= 0; i--)
				{
					if (visibleSeries[i] is PieSeries series && series.ExplodeOnTouch)
					{
						series.UpdateExplodeOnTouch((float)tapPoint.X, (float)tapPoint.Y);
					}

					if (visibleSeries[i].SelectionHitTest((float)tapPoint.X, (float)tapPoint.Y))
					{
						break;
					}
				}
			}
		}

		/// <inheritdoc/>
		void ITapGestureListener.OnTap(TapEventArgs e)
		{
			OnTapAction(this, e.TapPoint, e.TapCount);
		}

		/// <inheritdoc/>
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

		#endregion

		#region Property Callback Methods

		static void OnSeriesPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfCircularChart chart)
			{
				chart._chartArea.Series = newValue as ChartSeriesCollection;
			}
		}

		#endregion

		#endregion
	}
}