using System.Collections;
using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// The <see cref="WaterfallSeries"/> shows that an initial value is affected by a series of intermediate positive or negative values, leading to a final value.
	/// </summary>
	/// <remarks>
	/// <para>To render a series, create an instance of <see cref="WaterfallSeries"/> class, and add it to the <see cref="SfCartesianChart.Series"/> collection.</para>
	/// <para>It provides options for <see cref="ChartSeries.Fill"/>, <see cref="ChartSeries.PaletteBrushes"/>, <see cref="XYDataSeries.StrokeWidth"/>,  and <see cref="ChartSeries.Opacity"/> to customize the appearance.</para>
	/// 
	/// <para> <b>EnableTooltip - </b> A tooltip displays information while tapping or mouse hovering above a segment. To display the tooltip on a chart, you need to set the <see cref="ChartSeries.EnableTooltip"/> property as <b>true</b> in <see cref="WaterfallSeries"/> class, and also refer <seealso cref="ChartBase.TooltipBehavior"/> property.</para>
	/// <para> <b>Data Label - </b> Data labels are used to display values related to a chart segment. To render the data labels, you need to set the <see cref="ChartSeries.ShowDataLabels"/> property as <b>true</b> in <see cref="WaterfallSeries"/> class. To customize the chart data labels alignment, placement, and label styles, you need to create an instance of <see cref="CartesianDataLabelSettings"/> and set to the <see cref="CartesianSeries.DataLabelSettings"/> property.</para>
	/// <para> <b>Animation - </b> To animate the series, set <b>True</b> to the <see cref="ChartSeries.EnableAnimation"/> property.</para>
	/// <para><b>Spacing - </b> To specify the spacing between segments using the <see cref="Spacing"/> property.</para>
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
	///           <chart:SfCartesianChart.Series>
	///               <chart:WaterfallSeries
	///                   ItemsSource = "{Binding Sales}"
	///                   XBindingPath = "Department"
	///                   YBindingPath = "Value"/>
	///           </chart:SfCartesianChart.Series>  
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
	///     WaterfallSeries series = new WaterfallSeries();
	///     series.ItemsSource = viewModel.Sales;
	///     series.XBindingPath = "Department";
	///     series.YBindingPath = "Value";
	///     chart.Series.Add(series);
	///     
	/// ]]></code>
	/// # [ViewModel](#tab/tabid-3)
	/// <code><![CDATA[
	///     public ObservableCollection<Model> Sales { get; set; }
	/// 
	///     public ViewModel()
	///     {
	///        Sales = new ObservableCollection<Model>();
	///        Sales.Add(new ChartDataModel() { Department = "Income", Value = 46 });
	///        Sales.Add(new ChartDataModel() { Department = "Sales", Value = -14 });
	///        Sales.Add(new ChartDataModel() { Department = "Research", Value = -9});
	///        Sales.Add(new ChartDataModel() { Department = "Revenue", Value = 15 });
	///        Sales.Add(new ChartDataModel() { Department = "Balance", Value = 38 , IsSummary= true });
	///        Sales.Add(new ChartDataModel() { Department = "Expense", Value = -13 });
	///        Sales.Add(new ChartDataModel() { Department = "Tax", Value = -8 });
	///        Sales.Add(new ChartDataModel() { Department = "Profit", Value =17,IsSummary=true });
	///     }
	/// ]]></code>
	/// ***
	/// </example>
	public class WaterfallSeries : XYDataSeries, IDrawCustomLegendIcon
    {
        #region Fields

        double _bottomValue;
        List<bool> _summaryValues;

        #endregion

        #region Internal Properties

        internal override bool IsSideBySide => true;

        #endregion

        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="AllowAutoSum"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="AllowAutoSum"/> property determines whether the intermediate sum values in a series should be automatically calculated or not.
        /// </remarks>
        public static readonly BindableProperty AllowAutoSumProperty = BindableProperty.Create(
            nameof(AllowAutoSum),
            typeof(bool),
            typeof(WaterfallSeries),
            true,
            BindingMode.Default,
            null,
            propertyChanged: OnAllowAutoSumChanged);

        /// <summary>
        /// Identifies the <see cref="ShowConnectorLine"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="ShowConnectorLine"/> property indicates whether to enable the connector line between the segements or not.
        /// </remarks>
        public static readonly BindableProperty ShowConnectorProperty = BindableProperty.Create(
            nameof(ShowConnectorLine),
            typeof(bool),
            typeof(WaterfallSeries),
            true,
            BindingMode.Default,
            null,
            propertyChanged: OnShowConnectorChanged);

        /// <summary>
        /// Identifies the <see cref="ConnectorLineStyle"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="ConnectorLineStyle"/> property defines the customization of the connector lines in the series.
        /// </remarks>
        public static readonly BindableProperty ConnectorLineStyleProperty = BindableProperty.Create(
            nameof(ConnectorLineStyle),
            typeof(ChartLineStyle),
            typeof(WaterfallSeries),
            null,
            BindingMode.Default,
            null,
            propertyChanged: OnConnectorLineStyleChanged,
            defaultValueCreator: ConnectorLineStyleDefaultValueCreator);

        /// <summary>
        /// Identifies the <see cref="SummaryBindingPath"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="SummaryBindingPath"/> property used to retrieve the sum of the previous segments.
        /// </remarks>
        public static readonly BindableProperty SummaryBindingPathProperty = BindableProperty.Create(
            nameof(SummaryBindingPath),
            typeof(string),
            typeof(WaterfallSeries),
            defaultValue: string.Empty,
            propertyChanged: OnSummaryBindingPathChanged);

        /// <summary>
        /// Identifies the <see cref="SummaryPointsBrush"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="SummaryPointsBrush"/> property indicates the summary segment's interior.
        /// </remarks>
        public static readonly BindableProperty SummaryPointsBrushProperty = BindableProperty.Create(
            nameof(SummaryPointsBrush),
            typeof(Brush),
            typeof(WaterfallSeries),
            null,
            BindingMode.Default,
            null,
            propertyChanged: OnSummaryPointsBrushChanged);

        /// <summary>
        /// Identifies the <see cref="NegativePointsBrush"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="NegativePointsBrush"/> property indicates the Negative segment's interior.
        /// </remarks>
        public static readonly BindableProperty NegativePointsBrushProperty = BindableProperty.Create(
            nameof(NegativePointsBrush),
            typeof(Brush),
            typeof(WaterfallSeries),
            null,
            BindingMode.Default,
            null,
            propertyChanged: OnNegativePointsBrushChanged);

        /// <summary>
        /// Identifies the <see cref="Width"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="Width"/> property defines the width of each waterfall segment.
        /// </remarks>
        public static readonly BindableProperty WidthProperty = BindableProperty.Create(
            nameof(Width),
            typeof(double),
            typeof(WaterfallSeries),
            defaultValue: 0.8d,
            BindingMode.Default,
            null,
            propertyChanged: OnWidthChanged);

        /// <summary>
        /// Identifies the <see cref="Spacing"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="Spacing"/> property indicates spacing between the segments across the series in cluster mode.
        /// </remarks>
        public static readonly BindableProperty SpacingProperty = BindableProperty.Create(
            nameof(Spacing),
            typeof(double),
            typeof(WaterfallSeries),
            0d,
            BindingMode.Default,
            null,
            propertyChanged: OnSpacingChanged);

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a value that determines whether the intermediate sum values in a series should be automatically calculated or not.
        /// </summary>
        /// <value>It accepts <see cref="bool"/> values, and its default value is true.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-4)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///           <chart:WaterfallSeries ItemsSource = "{Binding Sales}"   
        ///                                  XBindingPath = "Department"   
        ///                                  YBindingPath = "Value"
        ///                                  AllowAutoSum = "False"/>
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-5)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     WaterfallSeries series = new WaterfallSeries()
        ///     {
        ///           ItemsSource = viewModel.Sales,
        ///           XBindingPath = "Department",
        ///           YBindingPath = "Value",
        ///           AllowAutoSum = "False",
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public bool AllowAutoSum
        {
            get { return (bool)GetValue(AllowAutoSumProperty); }
            set { SetValue(AllowAutoSumProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to enable the connector line between the segments or not.
        /// </summary>
        /// <value>It accepts <see cref="bool"/> values, and its default value is true.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-6)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///           <chart:WaterfallSeries ItemsSource = "{Binding Sales}"   
        ///                                  XBindingPath = "Department"   
        ///                                  YBindingPath = "Value"
        ///                                  ShowConnectorLine = "False"/>      
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-7)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     WaterfallSeries series = new WaterfallSeries()
        ///     {
        ///           ItemsSource = viewModel.Sales,
        ///           XBindingPath = "Department",
        ///           YBindingPath = "Value",
        ///           ShowConnectorLine = "False",
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public bool ShowConnectorLine
        {
            get { return (bool)GetValue(ShowConnectorProperty); }
            set { SetValue(ShowConnectorProperty, value); }
        }

        /// <summary>
        /// Gets or sets a style for connector lines, and it is often used to customize the appearance of connector lines for visual purposes.
        /// </summary>
        /// <value>It accepts <see cref="ChartLineStyle"/> values, and its default value is null.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-8)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///           <chart:WaterfallSeries ItemsSource = "{Binding Sales}"   
        ///                                  XBindingPath = "Department"   
        ///                                  YBindingPath = "Value"/>
        ///               <chart:WaterfallSeries.ConnectorLineStyle>
        ///                       <chart:ChartLineStyle Stroke = "Red" >
        ///                </chart:WaterfallSeries.ConnectorLineStyle>
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-9)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     WaterfallSeries series = new WaterfallSeries()
        ///     {
        ///           ItemsSource = viewModel.Sales,
        ///           XBindingPath = "Department",
        ///           YBindingPath = "Value",
        ///     };
        ///     series.ConnectorLineStyle = new ChartLineStyle()
        ///     {
        ///          Stroke = new SolidColorBrush(Colors.Red),
        ///      }
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public ChartLineStyle ConnectorLineStyle
        {
            get { return (ChartLineStyle)GetValue(ConnectorLineStyleProperty); }
            set { SetValue(ConnectorLineStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets a string value that indicates the sum of previous segments.
        /// </summary>
        /// <value>It accepts <see cref="string"/> values, and its default value is <c>string.Empty</c>.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-10)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///           <chart:WaterfallSeries ItemsSource = "{Binding Sales}"   
        ///                                  XBindingPath = "Department"   
        ///                                  YBindingPath = "Value"
        ///                                  SummaryBindingPath = "IsSummary"/>
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-11)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     WaterfallSeries series = new WaterfallSeries()
        ///     {
        ///           ItemsSource = viewModel.Sales,
        ///           XBindingPath = "Department",
        ///           YBindingPath = "Value",
        ///           SummaryBindingPath = "IsSummary",
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public string SummaryBindingPath
        {
            get { return (string)GetValue(SummaryBindingPathProperty); }
            set { SetValue(SummaryBindingPathProperty, value); }
        }

        /// <summary>
        /// Gets or sets a brush value that indicates the summary segment's interior.
        /// </summary>
        /// <value>It accepts <see cref="Brush"/> values.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-12)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///           <chart:WaterfallSeries ItemsSource = "{Binding Sales}"   
        ///                                  XBindingPath = "Department"   
        ///                                  YBindingPath = "Value"
        ///                                  SummaryPointsBrush = "Blue"/>
        ///                                                     
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-13)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     WaterfallSeries series = new WaterfallSeries()
        ///     {
        ///           ItemsSource = viewModel.Sales,
        ///           XBindingPath = "Department",
        ///           YBindingPath = "Value",
        ///           SummaryPointsBrush = Colors.Blue,
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public Brush SummaryPointsBrush
        {
            get { return (Brush)GetValue(SummaryPointsBrushProperty); }
            set { SetValue(SummaryPointsBrushProperty, value); }
        }

        /// <summary>
        /// Gets or sets a brush value that indicates the Negative segment's interior.
        /// </summary>
        /// <value>It accepts <see cref="Brush"/> values.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-14)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///           <chart:WaterfallSeries ItemsSource = "{Binding Sales}"   
        ///                                  XBindingPath = "Department"   
        ///                                  YBindingPath = "Value"
        ///                                  NegativePointsBrush = "Red"/>
        ///                                                     
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-15)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     WaterfallSeries series = new WaterfallSeries()
        ///     {
        ///           ItemsSource = viewModel.Sales,
        ///           XBindingPath = "Department",
        ///           YBindingPath = "Value",
        ///           NegativePointsBrush = Colors.Red,
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public Brush NegativePointsBrush
        {
            get { return (Brush)GetValue(NegativePointsBrushProperty); }
            set { SetValue(NegativePointsBrushProperty, value); }
        }

        /// <summary>
        /// Gets or sets the width of each segment.
        /// </summary>
        /// <value>It accepts <see cref="double"/> values between 0 to 1 and its default value is 0.8 </value>
        /// <example>
        /// # [Xaml](#tab/tabid-16)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///           <chart:WaterfallSeries ItemsSource = "{Binding Sales}"   
        ///                                  XBindingPath = "Department"   
        ///                                  YBindingPath = "Value"
        ///                                  Width = "1"/>
        ///                                                     
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-17)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     WaterfallSeries series = new WaterfallSeries()
        ///     {
        ///           ItemsSource = viewModel.Sales,
        ///           XBindingPath = "Department",
        ///           YBindingPath = "Value",
        ///           Width = 1,
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public double Width
        {
            get { return (double)GetValue(WidthProperty); }
            set { SetValue(WidthProperty, value); }
        }

        /// <summary>
        ///  Gets or sets the spacing between the segments across the series in cluster mode.
        /// </summary>
        /// <value>It accepts <see cref="double"/> values and its default value is 0d </value>
        /// <example>
        /// # [Xaml](#tab/tabid-18)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///           <chart:WaterfallSeries ItemsSource = "{Binding Sales}"   
        ///                                  XBindingPath = "Department"   
        ///                                  YBindingPath = "Value"
        ///                                  Spacing = "0.5"/>
        ///                                                     
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-19)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     WaterfallSeries series = new WaterfallSeries()
        ///     {
        ///           ItemsSource = viewModel.Sales,
        ///           XBindingPath = "Department",
        ///           YBindingPath = "Value",
        ///           Spacing = 0.5,
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public double Spacing
        {
            get { return (double)GetValue(SpacingProperty); }
            set { SetValue(SpacingProperty, value); }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the WaterfallSeries class.
        /// </summary>
        public WaterfallSeries()
        {
            _summaryValues = new List<bool>();
        }

        #endregion

        #region Interface Implementation

        void IDrawCustomLegendIcon.DrawSeriesLegend(ICanvas canvas, RectF rect, Brush fillColor, bool isSaveState)
        {
            if (isSaveState)
            {
                canvas.CanvasSaveState();
            }

            PathF pathF = new();
            pathF.MoveTo(5, 0);
            pathF.LineTo(7, 0);
            pathF.LineTo(7, 7);
            pathF.LineTo(5, 7);
            pathF.LineTo(5, 0);
            pathF.Close();
            pathF.MoveTo(0, 4);
            pathF.LineTo(2, 4);
            pathF.LineTo(2, 12);
            pathF.LineTo(0, 12);
            pathF.LineTo(0, 4);
            pathF.Close();
            pathF.MoveTo(12, 0);
            pathF.LineTo(10, 0);
            pathF.LineTo(10, 12);
            pathF.LineTo(12, 12);
            pathF.LineTo(12, 0);
            pathF.Close();
            canvas.FillPath(pathF);

            if (isSaveState)
            {
                canvas.CanvasRestoreState();
            }
        }

        #endregion

        #region Methods

        #region Protected Methods

        /// <inheritdoc/>
        protected override ChartSegment? CreateSegment()
        {
            return new WaterfallSegment();
        }

        #endregion

        #region Internal Methods

        internal override void GeneratePoints(string[] yPaths, params IList<double>[] yValueLists)
        {
            base.GeneratePoints(yPaths, yValueLists);
            GetSummaryValues();
        }

        internal override void GenerateSegments(SeriesView seriesView)
        {
            var xValues = GetXValues();
            double x1, x2, y1, y2;

            if (ActualXAxis != null)
            {
                _bottomValue = double.IsNaN(ActualXAxis.ActualCrossingValue) ? 0 : ActualXAxis.ActualCrossingValue;
            }

            if (IsGrouped && (IsIndexed || xValues == null))
            {
                for (var i = 0; i < PointsCount; i++)
                {
                    if (xValues != null)
                    {
                        OnCalculateSegmentValues(out x1, out x2, out y1, out y2, i, _bottomValue, xValues[i]);

                        if (i < Segments.Count && Segments[i] is WaterfallSegment)
                        {
                            ((WaterfallSegment)Segments[i]).SetData(new[] { x1, x2, y1, y2, i, YValues[i] });
                        }
                        else
                        {
                            CreateSegment(seriesView, new[] { x1, x2, y1, y2, i, YValues[i] }, i);
                        }
                    }
                }
            }
            else
            {
                if (xValues != null)
                {
                    for (var i = 0; i < PointsCount; i++)
                    {
                        var x = xValues[i];

                        OnCalculateSegmentValues(out x1, out x2, out y1, out y2, i, _bottomValue, xValues[i]);

                        if (i < Segments.Count && Segments[i] is WaterfallSegment)
                        {
                            ((WaterfallSegment)Segments[i]).SetData(new[] { x1, x2, y1, y2, x, YValues[i] });
                        }
                        else
                        {
                            CreateSegment(seriesView, new[] { x1, x2, y1, y2, x, YValues[i] }, i);
                        }
                    }
                }
            }
        }

        internal override TooltipInfo? GetTooltipInfo(ChartTooltipBehavior tooltipBehavior, float x, float y)
        {
            TooltipInfo? tooltipInfo = base.GetTooltipInfo(tooltipBehavior, x, y);
            if (tooltipInfo != null)
            {
                if (Segments[tooltipInfo.Index] is WaterfallSegment waterfallSegment)
                {
                    if (waterfallSegment.SegmentType == WaterfallSegmentType.Sum)
                    {
                        tooltipInfo.Text = waterfallSegment.Sum.ToString();
                    }
                }
            }

            return tooltipInfo;
        }

        internal override void SetTooltipTargetRect(TooltipInfo tooltipInfo, Rect seriesBounds)
        {
            if (Segments[tooltipInfo.Index] is WaterfallSegment waterfallSegment)
            {
                RectF targetRect = waterfallSegment.SegmentBounds;
                float xPosition = tooltipInfo.X;
                float yPosition;
                float width = targetRect.Width;
                float height = targetRect.Height;

                if (ChartArea != null && ChartArea.IsTransposed)
                {
                    xPosition = waterfallSegment.SegmentBounds.Center.X;
                    yPosition = waterfallSegment.SegmentBounds.Top;
                }
                else
                {
                    yPosition = waterfallSegment.Top;
                }

                targetRect = new Rect(xPosition - width / 2, yPosition, width, height);
                tooltipInfo.TargetRect = targetRect;
            }
        }

        internal override double GetActualWidth()
        {
            return Width;
        }

        internal override double GetActualSpacing()
        {
            return Spacing;
        }

        internal override double GetDataLabelPositionAtIndex(int index)
        {
            double dataLabelPositionAtIndex = 0;
            if (Segments.Count >= index)
            {
                if (Segments[index] is WaterfallSegment segment)
                {
                    double median = segment.y1 + ((segment.y2 - segment.y1) / 2);
                    var segmentType = segment.SegmentType;
                    double waterfallSum = segment.WaterfallSum;
                    if (segmentType is WaterfallSegmentType.Sum)
                    {
                        dataLabelPositionAtIndex = AllowAutoSum ?
                            (DataLabelSettings.BarAlignment == DataLabelAlignment.Middle) ? (waterfallSum / 2) :
                            (waterfallSum >= 0) ? segment.y1 : segment.y2 : (DataLabelSettings.BarAlignment == DataLabelAlignment.Middle) ? median : (YValues[index] >= 0) ? segment.y1 : segment.y2;
                    }
                    else if (DataLabelSettings.BarAlignment == DataLabelAlignment.Top)
                    {
                        dataLabelPositionAtIndex = (segmentType is WaterfallSegmentType.Positive) ? segment.y1 : segment.y2;
                    }
                    else if (DataLabelSettings.BarAlignment == DataLabelAlignment.Bottom)
                    {
                        dataLabelPositionAtIndex = (segmentType is WaterfallSegmentType.Positive) ? segment.y2 : segment.y1;
                    }
                    else
                        dataLabelPositionAtIndex = median;
                }
            }

            return dataLabelPositionAtIndex;
        }

        internal override void CalculateDataPointPosition(int index, ref double x, ref double y)
        {
            if (ChartArea == null) return;

            var x1 = SbsInfo.Start + x;
            var x2 = SbsInfo.End + x;
            var xCal = x1 + ((x2 - x1) / 2);
            var yCal = y;

            if (ActualYAxis != null && ActualXAxis != null && !double.IsNaN(yCal))
            {
                y = ChartArea.IsTransposed ? ActualXAxis.ValueToPoint(xCal) : ActualYAxis.ValueToPoint(yCal);
            }

            if (ActualXAxis != null && ActualYAxis != null && !double.IsNaN(x))
            {
                x = ChartArea.IsTransposed ? ActualYAxis.ValueToPoint(yCal) : ActualXAxis.ValueToPoint(xCal);
            }
        }

        internal override PointF GetDataLabelPosition(ChartSegment dataLabel, SizeF labelSize, PointF labelPosition, float padding)
        {
            if (ChartArea == null) return labelPosition;

            if (ChartArea.IsTransposed)
            {
                return DataLabelSettings.GetLabelPositionForTransposedRectangularSeries(this, dataLabel.Index, labelSize, labelPosition, padding, DataLabelSettings.BarAlignment);
            }

            return DataLabelSettings.GetLabelPositionForRectangularSeries(this, dataLabel.Index, labelSize, labelPosition, padding, DataLabelSettings.BarAlignment);
        }

        internal override void DrawDataLabels(ICanvas canvas)
        {
            var dataLabelSettings = ChartDataLabelSettings;

            if (dataLabelSettings == null) return;

            ChartDataLabelStyle labelStyle = dataLabelSettings.LabelStyle;

            foreach (ChartSegment dataLabel in Segments)
            {

                if (!dataLabel.IsEmpty)
                {
                    UpdateDataLabelAppearance(canvas, dataLabel, dataLabelSettings, labelStyle);
                }
            }
        }

        internal override Brush? GetFillColor(object item, int index)
        {
            Brush? fillColor = base.GetFillColor(item, index);

            if (fillColor == Chart?.GetSelectionBrush(this) || fillColor == GetSelectionBrush(item, index))
            {
                return fillColor;
            }

            if (Segments.Count > index && Segments[index] is WaterfallSegment segment)
            {
                switch (segment.SegmentType)
                {
                    case WaterfallSegmentType.Negative:
                        return NegativePointsBrush ?? fillColor;
                    case WaterfallSegmentType.Sum:
                        return SummaryPointsBrush ?? fillColor;
                }
            }

            return fillColor;
        }

        internal override void GenerateTrackballPointInfo(List<object> nearestDataPoints, List<TrackballPointInfo> pointInfos, ref bool isSideBySide)
        {
            var xValues = GetXValues();
            float xPosition = 0f;
            float yPosition = 0f;
            if (nearestDataPoints != null && ActualData != null && xValues != null && SeriesYValues != null)
            {
                IList<double> yValues = SeriesYValues[0];
                foreach (object point in nearestDataPoints)
                {
                    int index = ActualData.IndexOf(point);
                    var xValue = xValues[index];
                    WaterfallSegment? segment = Segments[index] as WaterfallSegment;
                    double yValue = yValues[index];

                    if (segment != null)
                    {
                        yValue = segment.y1;
                        if (segment.SegmentType == WaterfallSegmentType.Negative)
                            yValue = segment.y2;
                    }

                    string label = yValue.ToString();

                    if (IsSideBySide)
                    {
                        isSideBySide = true;
                        double xMidVal = xValue + SbsInfo.Start + ((SbsInfo.End - SbsInfo.Start) / 2);
                        xPosition = TransformToVisibleX(xMidVal, yValue);
                        yPosition = TransformToVisibleY(xMidVal, yValue);
                    }

                    TrackballPointInfo? chartPointInfo = CreateTrackballPointInfo(xPosition, yPosition, label, point);

                    if (chartPointInfo != null)
                    {
                        chartPointInfo.XValue = xValue;
                        chartPointInfo.YValues.Add(yValue);
                        pointInfos.Add(chartPointInfo);
                    }
                }
            }
        }

        internal override void ApplyTrackballLabelFormat(TrackballPointInfo pointInfo, string labelFormat)
        {
            var label = pointInfo.YValues[0].ToString(labelFormat);
            pointInfo.Label = label;
        }

        #endregion

        #region Private Methods

        static object ConnectorLineStyleDefaultValueCreator(BindableObject bindable)
        {
            return new ChartLineStyle()
            {
                Stroke = Color.FromArgb("#ABABAB"),
                StrokeWidth = 1
            };
        }

        void GetSummaryValues()
        {
            var enumerable = ItemsSource as IEnumerable;
            var enumerator = enumerable?.GetEnumerator();

            if (enumerable != null && enumerator != null && enumerator.MoveNext())
            {
                var currObj = enumerator.Current;

                FastReflection summaryProperty = new FastReflection();

                if (!summaryProperty.SetPropertyName(SummaryBindingPath, currObj) || summaryProperty.IsArray(currObj))
                {
                    return;
                }

                _summaryValues ??= new List<bool>();

                if (_summaryValues != null)
                {
                    do
                    {
                        var summaryVal = summaryProperty.GetValue(enumerator.Current);
                        _summaryValues.Add(Convert.ToBoolean(summaryVal));
                    }
                    while (enumerator.MoveNext());
                }
            }
        }

        void OnCalculateSegmentValues(out double x1, out double x2, out double y1, out double y2, int i, double bottomValue, double xVal)
        {
            x1 = xVal + SbsInfo.Start;
            x2 = xVal + SbsInfo.End;
            y1 = y2 = double.NaN;

            //Calculation for First Segment
            if (i == 0)
            {
                if (YValues[i] >= 0)
                {
                    y1 = YValues[i];
                    y2 = bottomValue;
                }
                else if (double.IsNaN(YValues[i]))
                {
                    y2 = bottomValue;
                    y1 = bottomValue;
                }
                else
                {
                    y2 = YValues[i];
                    y1 = bottomValue;
                }
            }
            else
            {
                if (Segments[i - 1] is WaterfallSegment prevSegment)
                {
                    // Positive value calculation                       
                    if (YValues[i] >= 0)
                    {
                        if (YValues[i - 1] >= 0 || prevSegment.SegmentType == WaterfallSegmentType.Sum)
                        {
                            if (!AllowAutoSum && prevSegment.SegmentType == WaterfallSegmentType.Sum && YValues[i - 1] < 0)
                            {
                                y1 = YValues[i] + prevSegment.y2;
                                y2 = prevSegment.y2;
                            }
                            else
                            {
                                y1 = YValues[i] + prevSegment.y1;
                                y2 = prevSegment.y1;
                            }
                        }
                        else if (double.IsNaN(YValues[i - 1]))
                        {
                            y1 = YValues[i] == 0 ? prevSegment.y2
                                : prevSegment.y2 + YValues[i];
                            y2 = prevSegment.y2;
                        }
                        else
                        {
                            y1 = YValues[i] + prevSegment.y2;
                            y2 = prevSegment.y2;
                        }
                    }
                    else if (double.IsNaN(YValues[i]))
                    {
                        // Empty value calculation
                        if (YValues[i - 1] >= 0 || prevSegment.SegmentType == WaterfallSegmentType.Sum)
                            y1 = y2 = prevSegment.y1;
                        else
                            y1 = y2 = prevSegment.y2;
                    }
                    else
                    {
                        // Negative value calculation
                        if (YValues[i - 1] >= 0 || prevSegment.SegmentType == WaterfallSegmentType.Sum)
                        {
                            if (!AllowAutoSum && prevSegment.SegmentType == WaterfallSegmentType.Sum && YValues[i - 1] < 0)
                            {
                                y1 = prevSegment.y2;
                                y2 = YValues[i] + prevSegment.y2;
                            }
                            else
                            {
                                y1 = prevSegment.y1;
                                y2 = YValues[i] + prevSegment.y1;
                            }
                        }
                        else
                        {
                            y1 = prevSegment.y2;
                            y2 = YValues[i] + prevSegment.y2;
                        }
                    }
                }
            }
        }

        void CreateSegment(SeriesView seriesView, double[] values, int index)
        {
            var segment = CreateSegment() as WaterfallSegment;

            if (segment != null)
            {
                segment.Series = this;
                segment.SeriesView = seriesView;
                segment.SetData(values);
                segment.Index = index;
                segment.Item = ActualData?[index];

                //Updating the Values for Summary Segments
                OnUpdateSumSegmentValues(segment, index);

                InitiateDataLabels(segment);
                Segments.Add(segment);
            }
        }

        void OnUpdateSumSegmentValues(WaterfallSegment segment, int index)
        {
            if ((index - 1) >= 0)
            {
                segment.PreviousWaterfallSegment = Segments[index - 1] as WaterfallSegment;
            }

            if (_summaryValues != null && _summaryValues.Count > index && _summaryValues[index] == true)
            {
                segment.SegmentType = WaterfallSegmentType.Sum;

                if (segment.PreviousWaterfallSegment != null)
                {
                    segment.WaterfallSum = segment.PreviousWaterfallSegment.Sum;
                }
                else
                {
                    segment.WaterfallSum = YValues[index];
                }

                //Assigning the values for Summary Segment
                if (AllowAutoSum && segment.PreviousWaterfallSegment != null)
                {
                    segment.y1 = segment.WaterfallSum;
                    segment.y2 = _bottomValue;
                }
                else
                {
                    if (YValues[index] >= 0)
                    {
                        segment.y1 = YValues[index];
                        segment.y2 = _bottomValue;
                    }
                    else if (double.IsNaN(YValues[index]))
                    {
                        segment.Bottom = segment.Top = (float)_bottomValue;
                    }
                    else
                    {
                        segment.y1 = _bottomValue;
                        segment.y2 = YValues[index];
                    }
                }

                YRange += new DoubleRange(segment.y1, segment.y2);
            }
            else
            {
                if (YValues[index] < 0)
                {
                    segment.SegmentType = WaterfallSegmentType.Negative;
                }
                else
                {
                    segment.SegmentType = WaterfallSegmentType.Positive;
                }
            }

            //Sum Value Calculation
            var sum = double.NaN;
            if (AllowAutoSum == false && segment.SegmentType == WaterfallSegmentType.Sum)
                sum = YValues[index];
            else if (segment.PreviousWaterfallSegment != null && segment.SegmentType != WaterfallSegmentType.Sum) //If segment is positive or negative
                sum = YValues[index] + segment.PreviousWaterfallSegment.Sum;
            else if (segment.PreviousWaterfallSegment != null) //If segment is sum type
                sum = segment.PreviousWaterfallSegment.Sum;
            else
                sum = YValues[index];

            segment.Sum = sum;
        }

        static void OnWidthChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is WaterfallSeries series)
            {
                series.UpdateSbsSeries();
            }
        }

        static void OnAllowAutoSumChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is WaterfallSeries series)
            {
                series.SegmentsCreated = false;
                series.ScheduleUpdateChart();
            }
        }

        static void OnShowConnectorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is WaterfallSeries series)
            {
                series.ScheduleUpdateChart();
            }
        }

        static void OnSummaryBindingPathChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is WaterfallSeries series)
            {
                series.OnBindingPathChanged();
            }
        }

        static void OnSummaryPointsBrushChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is WaterfallSeries series)
            {
                series.SegmentsCreated = false;
                series.ScheduleUpdateChart();
            }
        }

        static void OnNegativePointsBrushChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is WaterfallSeries series)
            {
                series.SegmentsCreated = false;
                series.ScheduleUpdateChart();
            }
        }

        static void OnSpacingChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is WaterfallSeries series && series.ChartArea != null)
            {
                series.InvalidateSeries();
            }
        }

        static void OnConnectorLineStyleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is WaterfallSeries series)
            {
                series.OnStylePropertyChanged(oldValue as ChartLineStyle, newValue as ChartLineStyle);
            }
        }

        void OnStylePropertyChanged(ChartLineStyle? oldValue, ChartLineStyle? newValue)
        {
            if (oldValue != null)
            {
                oldValue.PropertyChanged -= ConnectorLineStyles_PropertyChanged;
                oldValue.Parent = null;
                SetInheritedBindingContext(oldValue, null);
            }

            if (newValue != null)
            {
                newValue.PropertyChanged += ConnectorLineStyles_PropertyChanged;
                newValue.Parent = Parent;
                SetInheritedBindingContext(newValue, BindingContext);
            }

            if (AreaBounds != Rect.Zero)
            {
                InvalidateSeries();
            }
        }

        void ConnectorLineStyles_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            InvalidateSeries();
        }

        #endregion

        #endregion
    }
}