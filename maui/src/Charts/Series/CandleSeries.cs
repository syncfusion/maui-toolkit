namespace Syncfusion.Maui.Toolkit.Charts
{
    /// <summary>
    /// The <see cref="CandleSeries"/> displays a set of candle used in financial analysis to represent open, high, low, and close values of an asset or security.
    /// </summary>
    /// <remarks>
    /// <para>To render a series, create an instance of <see cref="CandleSeries"/> class, and add it to the <see cref="SfCartesianChart.Series"/> collection.</para>
    /// 
    /// <para>It provides options for <see cref="ChartSeries.Fill"/>, <see cref="ChartSeries.PaletteBrushes"/>, <see cref="XYDataSeries.StrokeWidth"/>, <see cref="Stroke"/>, and <see cref="ChartSeries.Opacity"/> to customize the appearance.</para>
    /// 
    /// <para> <b>EnableTooltip - </b> A tooltip displays information while tapping or mouse hovering above a segment. To display the tooltip on a chart, you need to set the <see cref="ChartSeries.EnableTooltip"/> property as <b>true</b> in <see cref="CandleSeries"/> class, and also refer <seealso cref="ChartBase.TooltipBehavior"/> property.</para>
    /// <para> <b>Animation - </b> To animate the series, set <b>True</b> to the <see cref="ChartSeries.EnableAnimation"/> property.</para>
    /// <para> <b>LegendIcon - </b> To customize the legend icon using the <see cref="ChartSeries.LegendIcon"/> property.</para>
    /// </remarks>
    /// <example>
    /// # [Xaml](#tab/tabid-1)
    /// <code><![CDATA[
    ///     <chart:SfCartesianChart>
    ///
    ///           <chart:SfCartesianChart.XAxes>
    ///               <chart:CategoryAxis/>
    ///           </chart:SfCartesianChart.XAxes>
    ///
    ///           <chart:SfCartesianChart.YAxes>
    ///               <chart:NumericalAxis/>
    ///           </chart:SfCartesianChart.YAxes>
    ///
    ///               <chart:CandleSeries
    ///                   ItemsSource="{Binding Data}"
    ///                   XBindingPath="XValue"
    ///                   High="High"
    ///                   Low="Low"
    ///                   Open="Open"
    ///                   Close="Close"/>
    ///           
    ///     </chart:SfCartesianChart>
    /// ]]></code>
    /// # [C#](#tab/tabid-2)
    /// <code><![CDATA[
    ///     SfCartesianChart chart = new SfCartesianChart();
    ///     
    ///     CategoryAxis xAxis = new CategoryAxis();
    ///     NumericalAxis yAxis = new NumericalAxis();
    ///     
    ///     chart.XAxes.Add(xAxis);
    ///     chart.YAxes.Add(yAxis);
    ///     
    ///     ViewModel viewModel = new ViewModel();
    /// 
    ///     CandleSeries series = new CandleSeries();
    ///     series.ItemsSource = viewModel.Data;
    ///     series.XBindingPath = "XValue";
    ///     series.High = "High";
    ///     series.Low = "Low";
    ///     series.Open = "Open";
    ///     series.Close = "Close";
    ///     chart.Series.Add(series);
    ///     
    /// ]]></code>
    /// # [ViewModel](#tab/tabid-3)
    /// <code><![CDATA[
    ///     public ObservableCollection<Model> Data { get; set; }
    /// 
    ///     public ViewModel()
    ///     {
    ///        Data = new ObservableCollection<Model>();
    ///        Data.Add(new Model() { XValue = "2000", High = 50, Low = 40, Open = 47, Close = 45});
    ///        Data.Add(new Model() { XValue = "2001", High = 50, Low = 35, Open = 45, Close = 40});
    ///        Data.Add(new Model() { XValue = "2002", High = 45, Low = 30, Open = 37, Close = 40 });
    ///        Data.Add(new Model() { XValue = "2003", High = 50, Low = 35, Open = 40, Close = 45});
    ///        Data.Add(new Model() { XValue = "2004", High = 45, Low = 30, Open = 35, Close = 32 });
    ///        Data.Add(new Model() { XValue = "2005", High = 50, Low = 35, Open = 40, Close = 45 });
    ///        Data.Add(new Model() { XValue = "2006", High = 40, Low = 31, Open = 36, Close = 34 });
    ///        Data.Add(new Model() { XValue = "2007", High = 48, Low = 38, Open = 43, Close = 40});
    ///        Data.Add(new Model() { XValue = "2008", High = 55, Low = 45, Open = 48, Close = 50});
    ///        Data.Add(new Model() { XValue = "2009", High = 45, Low = 30, Open = 35, Close = 40});
    ///        Data.Add(new Model() { XValue = "2010", High = 50, Low = 50, Open = 50, Close = 50 });
    ///     }
    /// ]]></code>
    /// ***
    /// </example>
    public class CandleSeries : HiLoOpenCloseSeries
    {
        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="Stroke"/> bindable property.
        /// </summary>
        /// <remarks>
        /// Defines the stroke brush used for the outline of the candle series.
        /// </remarks>
        public static readonly BindableProperty StrokeProperty = BindableProperty.Create(
            nameof(Stroke),
            typeof(Brush),
            typeof(CandleSeries),
            null,
            BindingMode.Default,
            null,
            OnStrokePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="EnableSolidCandle"/> bindable property.
        /// </summary>
        /// <remarks>
        /// Indicates whether solid candles should be enabled in the candle series.
        /// </remarks>
        public static readonly BindableProperty EnableSolidCandleProperty = BindableProperty.Create(
            nameof(EnableSolidCandle),
            typeof(bool),
            typeof(CandleSeries),
            false,
            BindingMode.Default,
            null,
            OnSolidCandlePropertyChanged);

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the stroke to the candle data point.
        /// </summary>
        /// <value>It accept the <see cref="Brush"/> values and its default value is null</value>
        /// <example>
        /// # [Xaml](#tab/tabid-4)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:CandleSeries ItemsSource="{Binding Data}"
        ///                              XBindingPath="XValue"
        ///                              High="High"
        ///                              Low="Low"
        ///                              Open="Open"
        ///                              Close="Close"
        ///                              Stroke="Red" />
        ///
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-5)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     CandleSeries series = new CandleSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           High = "High",
        ///           Low = "Low",
        ///           Open = "Open",
        ///           Close = "Close",
        ///           Stroke = new SolidColorBrush(Colors.Red),
        ///     };
        ///     
        ///     chart.Series.Add(series);
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
        /// Gets or sets a value indicating a whether enable solid candles.
        /// </summary>
        /// <value>It accepts the <c>bool</c> values and its default value is <c>false</c>.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-6)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:CandleSeries ItemsSource="{Binding Data}"
        ///                              XBindingPath="XValue"
        ///                              High="High"
        ///                              Low="Low"
        ///                              Open="Open"
        ///                              Close="Close"
        ///                              EnableSolidCandles="True"/>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-7)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     CandleSeries series = new CandleSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           High = "High",
        ///           Low = "Low",
        ///           Open = "Open",
        ///           Close = "Close",
        ///           EnableSolidCandles = true,
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public bool EnableSolidCandle
        {
            get { return (bool)GetValue(EnableSolidCandleProperty); }
            set { SetValue(EnableSolidCandleProperty, value); }
        }

        #endregion

        #region Methods

        #region Protected method

        /// <inheritdoc/>
        protected override ChartSegment? CreateSegment()
        {
            return new CandleSegment();
        }

        #endregion

        #region Internal Methods

        internal override void SetStrokeColor(ChartSegment segment)
        {
            segment.Stroke = Stroke;
        }

        internal override void CreateSegment(SeriesView seriesView, double[] values, bool isFill, bool isBull, int index)
        {
            var segment = CreateSegment() as CandleSegment;
            if (segment != null && ActualData != null)
            {
                segment.Series = this;
                segment.Index = index;
                segment.SeriesView = seriesView;
                segment.Item = ActualData[index];
                segment.SetData(values, isFill, isBull);
                InitiateDataLabels(segment);
                Segments.Add(segment);
            }
        }

        #endregion

        #region Private Methods

        static void OnStrokePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CandleSeries series && series.Chart != null)
            {
                series.UpdateStrokeColor();
                series.InvalidateSeries();
            }
        }

        static void OnSolidCandlePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CandleSeries series)
            {
                series.UpdateSbsSeries();
            }
        }

        #endregion

        #endregion
    }
}