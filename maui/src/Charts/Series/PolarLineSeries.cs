namespace Syncfusion.Maui.Toolkit.Charts
{
    /// <summary>
    /// The <see cref="PolarLineSeries"/> is a series that displays data in terms of values and angles by using a collection of straight lines. It allows for the visually comparing several quantitative or qualitative aspects of a situation.
    /// </summary>
    /// <remarks>
    /// <para>To render a series, create an instance of <see cref="PolarLineSeries"/> class, and add it to the <see cref="SfPolarChart.Series"/> collection.</para>
    /// 
    /// <para>It provides options for <see cref="ChartSeries.Fill"/>, <see cref="ChartSeries.PaletteBrushes"/>, <see cref="PolarSeries.StrokeWidth"/>, and <see cref="ChartSeries.Opacity"/> to customize the appearance.</para>
    /// 
    /// <para> <b>EnableTooltip - </b> A tooltip displays information while tapping or mouse hovering above a segment. To display the tooltip on a chart, set the <see cref="ChartSeries.EnableTooltip"/> property as <b>true</b> in <see cref="PolarLineSeries"/> class, and also refer <seealso cref="ChartBase.TooltipBehavior"/> property.</para>
    /// <para> <b>Data Label - </b> Data labels are used to display values related to a chart segment. To render the data labels, set the <see cref="ChartSeries.ShowDataLabels"/> property as <b>true</b> in <see cref="PolarLineSeries"/> class. To customize the chart data labels alignment, placement, and label styles, create an instance of <see cref="PolarDataLabelSettings"/> and set it to the <see cref="PolarSeries.DataLabelSettings"/> property.</para>
    /// <para> <b>Animation - </b> To animate the series, set <b>true</b> to the <see cref="ChartSeries.EnableAnimation"/> property.</para>
    /// <para> <b>LegendIcon - </b> Customize the legend icon using the <see cref="ChartSeries.LegendIcon"/> property.</para>
    /// </remarks>
    /// <example>
    /// # [Xaml](#tab/tabid-1)
    /// <code><![CDATA[
    ///     <chart:SfPolarChart>
    ///
    ///           <chart:SfPolarChart.PrimaryAxis>
    ///               <chart:NumericalAxis/>
    ///           </chart:SfPolarChart.PrimaryAxis>
    ///
    ///           <chart:SfPolarChart.SecondaryAxis>
    ///               <chart:NumericalAxis/>
    ///           </chart:SfPolarChart.SecondaryAxis>
    ///
    ///               <chart:PolarLineSeries
    ///                   ItemsSource="{Binding Data}"
    ///                   XBindingPath="XValue"
    ///                   YBindingPath="YValue"/> 
    ///           
    ///     </chart:SfPolarChart>
    /// ]]></code>
    /// # [C#](#tab/tabid-2)
    /// <code><![CDATA[
    ///     SfPolarChart chart = new SfPolarChart();
    ///     
    ///     NumericalAxis primaryAxis = new NumericalAxis();
    ///     NumericalAxis secondaryAxis = new NumericalAxis();
    ///     
    ///     chart.PrimaryAxis = primaryAxis;
    ///     chart.SecondaryAxis = secondaryAxis;
    ///     
    ///     ViewModel viewModel = new ViewModel();
    /// 
    ///     PolarLineSeries series = new PolarLineSeries();
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
    ///        Data.Add(new Model() { XValue = A, YValue = 100 });
    ///        Data.Add(new Model() { XValue = B, YValue = 150 });
    ///        Data.Add(new Model() { XValue = C, YValue = 110 });
    ///        Data.Add(new Model() { XValue = D, YValue = 230 });
    ///     }
    /// ]]></code>
    /// ***
    /// </example>
    public class PolarLineSeries : PolarSeries
    {
        #region Methods

        #region Protected Method

        /// <inheritdoc/>
        protected override ChartSegment? CreateSegment()
        {
            return new PolarLineSegment();
        }

        #endregion

        #region Internal Method

        internal override void GenerateSegments(SeriesView seriesView)
        {
            var xValues = GetXValues();

            if (xValues == null)
            {
                return;
            }

            if (PointsCount == 1)
            {
                CreateSegment(seriesView, new[] { xValues[0], YValues[0], double.NaN, double.NaN }, 0);
            }
            else
            {
                for (int i = 0; i < PointsCount; i++)
                {
                    if (i < Segments.Count)
                    {
                        Segments[i].SetData(new[] { xValues[i], YValues[i], xValues[i + 1], YValues[i + 1] });
                    }
                    else
                    {
                        if (i == PointsCount - 1)
                        {
                            if (IsClosed == true)
                            {
                                CreateSegment(seriesView, new[] { xValues[i], YValues[i], xValues[0], YValues[0] }, i);
                            }
                            else
                            {
                                CreateSegment(seriesView, new[] { xValues[i], YValues[i], double.NaN, double.NaN }, i);
                            }
                        }
                        else
                            CreateSegment(seriesView, new[] { xValues[i], YValues[i], xValues[i + 1], YValues[i + 1] }, i);
                    }
                }
            }
        }

        internal override void DrawDataLabels(ICanvas canvas)
        {
            var dataLabeSettings = DataLabelSettings;
            var xValues = GetXValues();

            if ((!ShowDataLabels) || ActualXAxis == null || ActualYAxis == null)
            {
                return;
            }

            ChartDataLabelStyle labelStyle = DataLabelSettings.LabelStyle;
            PolarLineSegment? dataLabel = Segments[0] as PolarLineSegment;

            if (dataLabel == null || dataLabel.XPoints == null || dataLabel.YPoints == null) return;

            for (int i = 0; i < PointsCount; i++)
            {
                double x = xValues![i], y = YValues[i];

                if (double.IsNaN(y)) continue;

                CalculateDataPointPosition(i, ref x, ref y);
                PointF labelPoint = new PointF((float)x, (float)y);

                dataLabel.LabelContent = GetLabelContent(YValues[i], SumOfValues(YValues));
                dataLabel.LabelPositionPoint = CalculateDataLabelPoint(dataLabel, labelPoint, labelStyle);
                UpdateDataLabelAppearance(canvas, dataLabel, dataLabeSettings, labelStyle);
            }
        }

        #endregion

        #region Private Method

        void CreateSegment(SeriesView seriesView, double[] values, int index)
        {
            var segment = CreateSegment() as PolarLineSegment;
            if (segment != null)
            {
                segment.Series = this;
                segment.SeriesView = seriesView;
                segment.Index = index;
                segment.Item = ActualData?[index];
                segment.SetData(values);
                InitiateDataLabels(segment);
                Segments.Add(segment);
            }
        }

        #endregion 

        #endregion
    }
}