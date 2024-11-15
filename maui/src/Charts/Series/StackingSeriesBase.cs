using System.Collections.Specialized;

namespace Syncfusion.Maui.Toolkit.Charts
{
    /// <summary>
    /// <see cref="StackingSeriesBase"/> is a base class for stacking chart series, including <see cref="StackingColumnSeries"/>,<see cref="StackingColumn100Series"/>,<see cref="StackingLineSeries"/>, <see cref="StackingLine100Series"/>, <see cref="StackingAreaSeries"/>, and <see cref="StackingArea100Series"/>. 
    /// </summary>
    public abstract class StackingSeriesBase : XYDataSeries
    {
        #region Internal Properties

        internal override bool IsStacking => true;

        internal IList<double>? TopValues { get; set; }

        internal IList<double>? BottomValues { get; set; }

        #endregion

        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="GroupingLabel"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="GroupingLabel"/> property allows for the grouping of series in a stacked chart.
        /// </remarks>
        public static readonly BindableProperty GroupingLabelProperty = BindableProperty.Create(
            nameof(GroupingLabel),
            typeof(string),
            typeof(StackingSeriesBase),
            string.Empty,
            BindingMode.Default,
            propertyChanged: OnGroupingLabelChanged);

        /// <summary>
        /// Identifies the <see cref="Stroke"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="Stroke"/> property determines the brush used for the stroke (outline) of the series.
        /// </remarks>
        public static readonly BindableProperty StrokeProperty = BindableProperty.Create(
            nameof(Stroke),
            typeof(Brush),
            typeof(StackingSeriesBase),
            null,
            BindingMode.Default,
            null,
            OnStrokeColorChanged);

        /// <summary>
        /// Identifies the <see cref="StrokeDashArray"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="StrokeDashArray"/> property specifies customization of the stroke patterns in the series.
        /// </remarks>
        public static readonly BindableProperty StrokeDashArrayProperty = BindableProperty.Create(
            nameof(StrokeDashArray),
            typeof(DoubleCollection),
            typeof(StackingSeriesBase),
            null,
            BindingMode.Default,
            null,
            OnStrokeDashArrayPropertyChanged);

        #endregion

        #region Public properties

        /// <summary>
        /// This property allows for the grouping of series in a stacked chart.
        /// </summary>
        /// <value>it accept <c>string</c> values and its default value is <c> string.Empty</c>.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-1)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///  <chart:StackingColumnSeries ItemsSource = "{Binding RoadSafetyData}"
        ///                              XBindingPath = "Month"
        ///                              YBindingPath = "Bus"
        ///                              GroupingLabel = "GroupOne"/>
        ///                              
        ///  <chart:StackingColumnSeries ItemsSource = "{Binding RoadSafetyData}"
        ///                              XBindingPath = "Month"
        ///                              YBindingPath = "Car"
        ///                              GroupingLabel = "GroupTwo"/>
        ///                              
        ///  <chart:StackingColumnSeries ItemsSource = "{Binding RoadSafetyData}"
        ///                              XBindingPath = "Month"
        ///                              YBindingPath = "Truck"
        ///                              GroupingLabel = "GroupOne"/>
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
        ///     StackingColumnSeries stackingSeries1 = new StackingColumnSeries()
        ///     {
        ///           ItemsSource = viewModel.RoadSafetyData,
        ///           XBindingPath = "Month",
        ///           YBindingPath = "Bus",
        ///           GroupingLabel = "GroupOne"
        ///     };
        ///     StackingColumnSeries stackingSeries2 = new StackingColumnSeries()
        ///     {
        ///           ItemsSource = viewModel.RoadSafetyData,
        ///           XBindingPath = "Month",
        ///           YBindingPath = "Car",
        ///           GroupingLabel = "GroupTwo"
        ///     };
        ///     StackingColumnSeries stackingSeries3 = new StackingColumnSeries()
        ///     {
        ///           ItemsSource = viewModel.RoadSafetyData,
        ///           XBindingPath = "Month",
        ///           YBindingPath = "Truck",
        ///           GroupingLabel = "GroupOne"
        ///     };
        ///     
        ///     chart.Series.Add(stackingSeries1);
        ///     chart.Series.Add(stackingSeries2);
        ///     chart.Series.Add(stackingSeries3);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public string GroupingLabel
        {
            get { return (string)GetValue(GroupingLabelProperty); }
            set { SetValue(GroupingLabelProperty, value); }
        }

        /// <summary>
        ///  Gets or sets a value to customize the stroke appearance.
        /// </summary>
        /// <value>It accepts <see cref="Brush"/> values and its default value is null.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-3)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:StackingAreaSeries ItemsSource = "{Binding Data}"
        ///                                    XBindingPath = "XValue"
        ///                                    YBindingPath = "YValue"
        ///                                    Stroke = "Red" />
        ///
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-4)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///     
        ///     StackingAreaSeries series = new StackingAreaSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
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
        ///  Gets or sets the stroke dash array to customize the appearance of the stroke.
        /// </summary>
        /// <value>It accepts the <see cref="DoubleCollection"/> value and the default value is null.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-5)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:StackingAreaSeries ItemsSource = "{Binding Data}"
        ///                            XBindingPath = "XValue"
        ///                            YBindingPath = "YValue"
        ///                            StrokeDashArray = "5,3"
        ///                            Stroke = "Red" />
        ///
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-6)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     DoubleCollection doubleCollection = new DoubleCollection();
        ///     doubleCollection.Add(5);
        ///     doubleCollection.Add(3);
        ///     StackingAreaSeries series = new StackingAreaSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           StrokeDashArray = doubleCollection,
        ///           Stroke = new SolidColorBrush(Colors.Red),
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public DoubleCollection StrokeDashArray
        {
            get { return (DoubleCollection)GetValue(StrokeDashArrayProperty); }
            set { SetValue(StrokeDashArrayProperty, value); }
        }

        #endregion

        #region Methods

        #region Internal Methods

        internal override void SetStrokeColor(ChartSegment segment)
        {
            segment.Stroke = Stroke;
        }

        internal override void SetDashArray(ChartSegment segment)
        {
            segment.StrokeDashArray = StrokeDashArray;
        }

        internal override List<object>? GetDataPoints(double startX, double endX, double startY, double endY, int minimum, int maximum, List<double> xValues, bool validateYValues)
        {
            List<object> dataPoints = new List<object>();

            if (TopValues == null || ActualData == null || xValues.Count != TopValues.Count)
            {
                return null;
            }

            for (int i = minimum; i <= maximum; i++)
            {
                double xValue = xValues[i];
                if (validateYValues || (startX <= xValue && xValue <= endX))
                {
                    double topValue = TopValues[i];
                    if (startY <= topValue && topValue <= endY)
                    {
                        dataPoints.Add(ActualData[i]);
                    }
                }
            }

            return dataPoints;
        }

        internal override void OnDataSource_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            ResetVisibleSeries();
            base.OnDataSource_CollectionChanged(sender, e);
        }

        #endregion

        #region Private Methods

        static void OnStrokeColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is StackingSeriesBase series)
            {
                series.UpdateStrokeColor();
                series.InvalidateSeries();
            }
        }

        static void OnStrokeDashArrayPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is StackingSeriesBase series)
            {
                series.UpdateDashArray();
                series.InvalidateSeries();
            }
        }

        static void OnGroupingLabelChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is StackingSeriesBase series && series.ChartArea != null)
            {
                series.RefreshSeries();
            }
        }

        void RefreshSeries()
        {
            if (ChartArea == null || ChartArea.Series == null)
            {
                return;
            }

            foreach (var series in ChartArea.Series.OfType<StackingSeriesBase>())
            {
                series.SegmentsCreated = false;
            }

            ChartArea.NeedsRelayout = true;
            ChartArea.SideBySideSeriesPosition = null;
            ChartArea.ScheduleUpdateArea();
        }

        void ResetVisibleSeries()
        {
            if (ChartArea != null)
            {
                var visibleSeries = ChartArea.VisibleSeries;
                var stackingSeries = visibleSeries?.Where(series => series.IsStacking).ToList();

                if (stackingSeries == null) return;

                foreach (var chartSeries in stackingSeries)
                {
                    chartSeries.SegmentsCreated = false;
                }
            }
        }

        #endregion

        #endregion
    }
}