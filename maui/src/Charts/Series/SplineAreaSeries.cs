namespace Syncfusion.Maui.Toolkit.Charts
{
    /// <summary>
    /// The <see cref="SplineAreaSeries"/> is a collection of data points using smooth bezier line curves with the areas below filled.
    /// </summary>
    /// <remarks>
    /// <para>To render a series, create an instance of <see cref="SplineAreaSeries"/> class, and add it to the <see cref="SfCartesianChart.Series"/> collection.</para>
    /// 
    /// <para>It provides options for <see cref="ChartSeries.Fill"/>, <see cref="ChartSeries.PaletteBrushes"/>, <see cref="XYDataSeries.StrokeWidth"/>, and <see cref="ChartSeries.Opacity"/> to customize the appearance.</para>
    /// 
    /// <para> <b>EnableTooltip - </b> A tooltip displays information while tapping or mouse hovering above a segment. To display the tooltip on a chart, you need to set the <see cref="ChartSeries.EnableTooltip"/> property as <b>true</b> in <see cref="SplineAreaSeries"/> class, and also refer <seealso cref="ChartBase.TooltipBehavior"/> property.</para>
    /// <para> <b>Data Label - </b> Data labels are used to display values related to a chart segment. To render the data labels, you need to set the <see cref="ChartSeries.ShowDataLabels"/> property as <b>true</b> in <see cref="SplineAreaSeries"/> class. To customize the chart data labels alignment, placement, and label styles, you need to create an instance of <see cref="CartesianDataLabelSettings"/> and set to the <see cref="CartesianSeries.DataLabelSettings"/> property.</para>
    /// <para> <b>Animation - </b> To animate the series, set <b>True</b> to the <see cref="ChartSeries.EnableAnimation"/> property.</para>
    /// <para> <b>LegendIcon - </b> To customize the legend icon using the <see cref="ChartSeries.LegendIcon"/> property.</para>
    /// </remarks>
    /// <example>
    /// # [Xaml](#tab/tabid-1)
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
    ///               <chart:SplineAreaSeries
    ///                   ItemsSource="{Binding Data}"
    ///                   XBindingPath="XValue"
    ///                   YBindingPath="YValue"/>
    ///           </chart:SfCartesianChart.Series>  
    /// 
    ///     </chart:SfCartesianChart>
    /// ]]></code>
    /// # [C#](#tab/tabid-2)
    /// <code><![CDATA[
    ///     SfCartesianChart chart = new SfCartesianChart();
    /// 
    ///     NumericalAxis xAxis = new NumericalAxis();
    ///     NumericalAxis yAxis = new NumericalAxis();
    /// 
    ///     chart.XAxes.Add(xAxis);
    ///     chart.YAxes.Add(yAxis);
    /// 
    ///     ViewModel viewModel = new ViewModel();
    /// 
    ///     SplineAreaSeries series = new SplineAreaSeries();
    ///     series.ItemsSource = viewModel.Data;
    ///     series.XBindingPath = "XValue";
    ///     series.YBindingPath = "YValue";
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
    ///        Data.Add(new Model() { XValue = 10, YValue = 100 });
    ///        Data.Add(new Model() { XValue = 20, YValue = 150 });
    ///        Data.Add(new Model() { XValue = 30, YValue = 110 });
    ///        Data.Add(new Model() { XValue = 40, YValue = 230 });
    ///     }
    /// ]]></code>
    /// ***
    /// </example>
    public partial class SplineAreaSeries : AreaSeries
    {
        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="Type"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="Type"/> property indicates the shape of the spline area series.
        /// </remarks>  
        public static readonly BindableProperty TypeProperty =BindableProperty.Create(
            nameof(Type),
            typeof(SplineType),
            typeof(SplineAreaSeries),
            SplineType.Natural,
            BindingMode.Default,
            null,
            OnSplineTypeChanged);

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a value that indicates the shape of the spline area series.
        /// </summary>
        /// <value>It accepts <see cref="SplineType"/> values and its default value is <see cref="SplineType.Natural"/>.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-4)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:SplineAreaSeries ItemsSource = "{Binding Data}"
        ///                                  XBindingPath = "XValue"
        ///                                  YBindingPath = "YValue"
        ///                                  Type = "Monotonic"/>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-5)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     SplineAreaSeries series = new SplineAreaSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           Type = SplineType.Monotonic,
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public SplineType Type
        {
            get { return (SplineType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        #endregion

        #region Methods

        #region Public Override Methods

        /// <inheritdoc/>
        public override int GetDataPointIndex(float pointX, float pointY)
        {
            if (Chart != null)
            {
                var dataPoint = FindNearestChartPoint(pointX, pointY);
                if (dataPoint == null || ActualData == null)
                {
                    return -1;
                }

                var tooltipIndex = ActualData.IndexOf(dataPoint);
                if (tooltipIndex < 0)
                {
                    return -1;
                }

                double yValue = YValues[tooltipIndex];

                if (double.IsNaN(yValue))
                {
                    return -1;
                }

                PointF visiblePoint = new PointF();
                List<PointF> segPoints = new List<PointF>();
                var xValues = GetXValues();
                if (xValues != null)
                {
                    double xVal = xValues[tooltipIndex];
                    var visibleX = TransformToVisibleX(xVal, 0);
                    if (((pointX - (float)AreaBounds.Left) < visibleX) && tooltipIndex != 0)
                    {
                        xVal = xValues[tooltipIndex - 1];
                        yValue = YValues[tooltipIndex - 1];
                        visiblePoint.X = TransformToVisibleX(xVal, 0);
                        visiblePoint.Y = TransformToVisibleY(xVal, 0);
                        segPoints.Add(new PointF(visiblePoint.X, visiblePoint.Y));
                        visiblePoint.X = TransformToVisibleX(xVal, yValue);
                        visiblePoint.Y = TransformToVisibleY(xVal, yValue);
                        segPoints.Add(new PointF(visiblePoint.X, visiblePoint.Y));
                    }
                    else if (((pointX - (float)AreaBounds.Left) > visibleX) || tooltipIndex == 0)
                    {
                        yValue = YValues[tooltipIndex];
                        xVal = xValues[tooltipIndex];
                        visiblePoint.X = TransformToVisibleX(xVal, 0);
                        visiblePoint.Y = TransformToVisibleY(xVal, 0);
                        segPoints.Add(new PointF(visiblePoint.X, visiblePoint.Y));
                        visiblePoint.X = TransformToVisibleX(xVal, yValue);
                        visiblePoint.Y = TransformToVisibleY(xVal, yValue);
                        segPoints.Add(new PointF(visiblePoint.X, visiblePoint.Y));
                    }

                    if ((tooltipIndex == ActualData.Count - 1) || (pointX - (float)AreaBounds.Left) < visibleX)
                    {
                        yValue = YValues[tooltipIndex];
                        xVal = xValues[tooltipIndex];
                        visiblePoint.X = TransformToVisibleX(xVal, yValue);
                        visiblePoint.Y = TransformToVisibleY(xVal, yValue);
                        segPoints.Add(new PointF(visiblePoint.X, visiblePoint.Y));
                        visiblePoint.X = TransformToVisibleX(xVal, 0);
                        visiblePoint.Y = TransformToVisibleY(xVal, 0);
                        segPoints.Add(new PointF(visiblePoint.X, visiblePoint.Y));
                    }
                    else
                    {
                        xVal = xValues[tooltipIndex + 1];
                        yValue = YValues[tooltipIndex + 1];
                        visiblePoint.X = TransformToVisibleX(xVal, yValue);
                        visiblePoint.Y = TransformToVisibleY(xVal, yValue);
                        segPoints.Add(new PointF(visiblePoint.X, visiblePoint.Y));
                        visiblePoint.X = TransformToVisibleX(xVal, 0);
                        visiblePoint.Y = TransformToVisibleY(xVal, 0);
                        segPoints.Add(new PointF(visiblePoint.X, visiblePoint.Y));
                    }
                }

                var x = pointX - (float)AreaBounds.Left;
                var y = pointY - (float)AreaBounds.Top;

                if (ChartUtils.IsPathContains(segPoints, x, y))
                {
                    return tooltipIndex;
                }
            }

            return -1;
        }

        #endregion

        #region Protected Methods

        /// <inheritdoc/>
        protected override ChartSegment CreateSegment()
        {
            return new SplineAreaSegment();
        }

        #endregion

        #region Internal Methods

        internal override void GenerateSegments(SeriesView seriesView)
        {
            var xValues = GetXValues();
            if (xValues == null || xValues.Count == 0)
            {
                return;
            }

            double[] startControlPointsX = new double[PointsCount - 1];
            double[] startControlPointsY = new double[PointsCount - 1];
            double[] endControlPointsX = new double[PointsCount - 1];
            double[] endControlPointsY = new double[PointsCount - 1];
            double[]? dx = null;

            double[]? yCoef;

            if (Type == SplineType.Monotonic)
            {
                yCoef = GetMonotonicSpline(xValues, YValues, out dx);
            }
            else if (Type == SplineType.Cardinal)
            {
                yCoef = GetCardinalSpline(xValues, YValues);
            }
            else
            {
                yCoef = NaturalSpline(YValues, Type);
            }

            if (yCoef == null)
            {
                return;
            }

            for (int i = 0; i < PointsCount - 1; i++)
            {
                var x = xValues[i];
                var y = YValues[i];

                var nextX = xValues[i + 1];
                var nextY = YValues[i + 1];

                List<double>? controlPoints;

                if (dx != null && Type == SplineType.Monotonic && dx.Length > 0)
                {
                    controlPoints = CalculateControlPoints(x, y, nextX, nextY, yCoef[i], yCoef[i + 1], dx[i]);
                }
                else if (Type == SplineType.Cardinal)
                {
                    controlPoints = CalculateControlPoints(x, y, nextX, nextY, yCoef[i], yCoef[i + 1]);
                }
                else
                {
                    controlPoints = CalculateControlPoints(YValues, yCoef[i], yCoef[i + 1], i);
                }

                if (controlPoints != null)
                {
                    startControlPointsX[i] = controlPoints[0];
                    startControlPointsY[i] = controlPoints[1];
                    endControlPointsX[i] = controlPoints[2];
                    endControlPointsY[i] = controlPoints[3];
                }
            }

            CreateSegment(seriesView, xValues, YValues, startControlPointsX, startControlPointsY, endControlPointsX, endControlPointsY);
        }

        internal override void SetStrokeColor(ChartSegment segment)
        {
            segment.Stroke = Stroke;
        }

        internal override bool IsIndividualSegment()
        {
            return false;
        }

        #endregion

        #region Private Methods

        void CreateSegment(SeriesView seriesView, IList<double> xVals, IList<double> yVals, double[] startPointsX, double[] startPointsY, double[] endPointsX, double[] endPointsY)
        {
            List<double>? xValues = null, yValues = null, startControlPointsX = null, startControlPointsY = null,
                endControlPointsX = null, endControlPointsY = null;
            List<object>? items = null;

            for (int i = 0; i < PointsCount; i++)
            {
                if (!double.IsNaN(YValues[i]))
                {
                    if (xValues == null)
                    {
                        xValues = new List<double>();
                        yValues = new List<double>();
                        startControlPointsX = new List<double>();
                        startControlPointsY = new List<double>();
                        endControlPointsX = new List<double>();
                        endControlPointsY = new List<double>();
                        items = new List<object>();
                    }

                    xValues.Add(xVals[i]);
                    yValues?.Add(yVals[i]);
                    items?.Add(ActualData![i]);

                    if (i != PointsCount - 1)
                    {
                        startControlPointsX?.Add(startPointsX[i]);
                        startControlPointsY?.Add(startPointsY[i]);
                        endControlPointsX?.Add(endPointsX[i]);
                        endControlPointsY?.Add(endPointsY[i]);
                    }
                }

                if (double.IsNaN(YValues[i]) || i == PointsCount - 1)
                {
                    if (xValues != null)
                    {
                        var segment = CreateSegment() as SplineAreaSegment;
                        if (segment != null)
                        {
                            segment.Series = this;
                            segment.SeriesView = seriesView;
                            segment.SetData(xValues, yValues, startControlPointsX, startControlPointsY, endControlPointsX, endControlPointsY);
                            segment.Item = items;
                            InitiateDataLabels(segment);
                            Segments.Add(segment);
                        }

                        yValues = xValues = startControlPointsX = startControlPointsY = endControlPointsX = endControlPointsY = null;
                        items = null;
                    }

                    if (double.IsNaN(YValues[i]))
                    {
                        xValues = new List<double> { xVals[i] };
                        yValues = new List<double> { yVals[i] };
                        items = new List<object> { ActualData![i] };
                        startControlPointsX = new List<double>();
                        startControlPointsY = new List<double>();
                        endControlPointsX = new List<double>();
                        endControlPointsY = new List<double>();

                        var segment = (SplineAreaSegment)CreateSegment();
                        segment.Series = this;
                        segment.SeriesView = seriesView;
                        segment.Item = items;
                        segment.SetData(xValues, yValues, startControlPointsX, startControlPointsY, endControlPointsX, endControlPointsY);
                        yValues = xValues = startControlPointsX = startControlPointsY = endControlPointsX = endControlPointsY = null;
                        items = null;
                    }
                }
            }
        }

        static void OnSplineTypeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SplineSeries series)
            {
                series.SegmentsCreated = false;
                series.ScheduleUpdateChart();
            }
        }

        #endregion

        #endregion
    }
}