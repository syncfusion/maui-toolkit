namespace Syncfusion.Maui.Toolkit.Charts
{
    /// <summary>
    /// The <see cref="StepAreaSeries"/> is a chart type to represent data using connected horizontal and vertical steps, creating a stepped area visualization.
    /// </summary>
    /// <remarks>
    /// <para>To render a series, create an instance of <see cref="StepAreaSeries"/> class, and add it to the <see cref="SfCartesianChart.Series"/> collection.</para>
    /// 
    /// <para>It provides options for <see cref="ChartSeries.Fill"/>, <see cref="ChartSeries.PaletteBrushes"/>, <see cref="XYDataSeries.StrokeWidth"/>, <see cref="AreaSeries.Stroke"/>, and <see cref="ChartSeries.Opacity"/> to customize the appearance.</para>
    /// 
    /// <para> <b>EnableTooltip - </b> A tooltip displays information while tapping or mouse hovering above a segment. To display the tooltip on a chart, you need to set the <see cref="ChartSeries.EnableTooltip"/> property as <b>true</b> in <see cref="StepAreaSeries"/> class, and also refer <seealso cref="ChartBase.TooltipBehavior"/> property.</para>
    /// <para> <b>Data Label - </b> Data labels are used to display values related to a chart segment. To render the data labels, you need to set the <see cref="ChartSeries.ShowDataLabels"/> property as <b>true</b> in <see cref="StepAreaSeries"/> class. To customize the chart data labels alignment, placement, and label styles, you need to create an instance of <see cref="CartesianDataLabelSettings"/> and set to the <see cref="CartesianSeries.DataLabelSettings"/> property.</para>
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
    ///               <chart:stepAreaSeries
    ///                   ItemsSource = "{Binding Data}"
    ///                   XBindingPath = "XValue"
    ///                   YBindingPath = "YValue"/>
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
    ///     stepAreaSeries series = new stepAreaSeries();
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
    public class StepAreaSeries : AreaSeries
    {
        #region Methods

        #region Public Methods

        /// <inheritdoc/>
        public override int GetDataPointIndex(float pointX, float pointY)
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

            List<PointF> segPoints = new List<PointF>();

            foreach (StepAreaSegment segment in Segments)
            {
                var points = segment.FillPoints;
                if (points != null)
                {
                    var index = 4 * (tooltipIndex) + 2;

                    if (index > points.Count) continue;

                    float xValue = points[index];
                    float yValue = points[index + 1];

                    if (((pointX - (float)AreaBounds.Left) > xValue) || tooltipIndex == 0)
                    {
                        segPoints.Add(new PointF(xValue, points[1]));
                        segPoints.Add(new PointF(xValue, yValue));

                        if (index + 3 > points.Count) continue;

                        xValue = points[index + 2];
                        yValue = points[index + 3];

                        if (!double.IsNaN(yValue))
                        {
                            segPoints.Add(new PointF(xValue, yValue));
                            segPoints.Add(new PointF(xValue, points[1]));
                        }
                    }
                    else if (((pointX - (float)AreaBounds.Left) < xValue) && tooltipIndex != 0)
                    {
                        xValue = points[index - 2];
                        yValue = points[index - 1];

                        if (!double.IsNaN(yValue))
                        {
                            segPoints.Add(new PointF(xValue, points[1]));
                            segPoints.Add(new PointF(xValue, yValue));
                        }

                        xValue = points[index];
                        yValue = points[index + 1];

                        segPoints.Add(new PointF(xValue, yValue));
                        segPoints.Add(new PointF(xValue, points[1]));
                    }
                }
            }

            var x = pointX - (float)AreaBounds.Left;
            var y = pointY - (float)AreaBounds.Top;

            if (ChartUtils.IsPathContains(segPoints, x, y))
            {
                return tooltipIndex;
            }

            return -1;
        }

        #endregion

        #region Protected Methods

        /// <inheritdoc/>
        protected override ChartSegment CreateSegment()
        {
            return new StepAreaSegment();
        }

        #endregion

        #endregion
    }
}