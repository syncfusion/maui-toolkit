using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// Represents a range column series in .NET MAUI <see cref="SfCartesianChart"/>.
	/// </summary>
	/// <remarks>
	/// <para>To render a series, create an instance of <see cref="RangeColumnSeries"/> class, and add it to the <see cref="SfCartesianChart.Series"/> collection.</para>
	/// <para> <b>EnableTooltip - </b> A tooltip displays information while tapping or mouse hovering above a segment. To display the tooltip on a chart, you need to set the <see cref="ChartSeries.EnableTooltip"/> property as <b>true</b> in <see cref="RangeColumnSeries"/> class, and also refer <seealso cref="ChartBase.TooltipBehavior"/> property.</para>
	/// <para> <b>Data Label - </b> Data labels are used to display values related to a chart segment. To render the data labels, you need to set the <see cref="ChartSeries.ShowDataLabels"/> property as <b>true</b> in <see cref="RangeColumnSeries"/> class. To customize the chart data labels alignment, placement, and label styles, you need to create an instance of <see cref="CartesianDataLabelSettings"/> and set to the <see cref="CartesianSeries.DataLabelSettings"/> property.</para>
	/// <para> <b>Animation - </b> To animate the series, set <b>True</b> to the <see cref="ChartSeries.EnableAnimation"/> property.</para>
	/// <para> <b>LegendIcon - </b> To customize the legend icon using the <see cref="ChartSeries.LegendIcon"/> property.</para>
	/// <para> Range column series do not yet support trackball behavior.</para>
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
	///               <chart:RangeColumnSeries
	///                   ItemsSource="{Binding Data}"
	///                   XBindingPath="XValue"
	///                   High="HighValue"
	///                   Low="LowValue"/>
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
	///     RangeColumnSeries series = new RangeColumnSeries();
	///     series.ItemsSource = viewModel.Data;
	///     series.XBindingPath = "XValue";
	///     series.High = "HighValue";
	///     series.Low = "LowValue";
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
	///        Data.Add(new Model() { XValue = Value 1, HighValue = 3, LowValue = 6 });
	///        Data.Add(new Model() { XValue = Value 2, HighValue = 3, LowValue = 7 });
	///        Data.Add(new Model() { XValue = Value 3, HighValue = 4, LowValue = 10 });
	///        Data.Add(new Model() { XValue = Value 4, HighValue = 6, LowValue = 13 });
	///        Data.Add(new Model() { XValue = Value 5, HighValue = 9, LowValue = 17 });
	///     }
	/// ]]></code>
	/// ***
	/// </example>
	public partial class RangeColumnSeries : RangeSeriesBase, IDrawCustomLegendIcon
    {
        #region Internal properties

        internal override bool IsSideBySide
        {
            get
            {
                return true;
            }
        }

        #endregion

        #region BindableProperty Registration

        /// <summary>
        /// Identifies the <see cref="Spacing"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="Spacing"/> property indicate spacing between the segments across the series.
        /// </remarks>
        public static readonly BindableProperty SpacingProperty = BindableProperty.Create(
            nameof(Spacing),
            typeof(double),
            typeof(RangeColumnSeries),
            0.0d,
            BindingMode.Default,
            null,
            OnSpacingChanged);

        /// <summary>
        /// Identifies the <see cref="CornerRadius"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="CornerRadius"/> property helps to smooth column edges in range column series.
        /// </remarks>
        public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(
            nameof(CornerRadius),
            typeof(CornerRadius),
            typeof(RangeColumnSeries),
            null,
            BindingMode.Default,
            null,
            OnCornerRadiusChanged);

        /// <summary>
        /// Identifies the <see cref="Width"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="Width"/> property indicates the width of the range column segment.
        /// </remarks>
        public static readonly BindableProperty WidthProperty = BindableProperty.Create(
            nameof(Width),
            typeof(double),
            typeof(RangeColumnSeries),
            0.8d,
            BindingMode.Default,
            null,
            OnWidthChanged);

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a value to indicate space between the segments across the series.
        /// </summary>
        /// <value>It accepts <c>double</c> values ranging from 0 to 1, where the default value is 0.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-4)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:RangeColumnSeries ItemsSource = "{Binding Data}"
        ///                                   XBindingPath = "XValue"
        ///                                   High = "HighValue"
        ///                                   Low = "LowValue"
        ///                                   Spacing = "0.3"/>
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
        ///     RangeColumnSeries rangeColumnSeries = new RangeColumnSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           High = "HighValue",
        ///           Low = "LowValue",
        ///           Spacing = 0.3,
        ///     };
        ///     
        ///     chart.Series.Add(rangeColumnSeries);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public double Spacing
        {
            get { return (double)GetValue(SpacingProperty); }
            set { SetValue(SpacingProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that defines the rounded corners for range column segments.
        /// </summary>
        ///  <value>It accepts <see cref="Microsoft.Maui.CornerRadius"/> value, the default is null</value>
        /// <example>
        /// # [Xaml](#tab/tabid-6)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:RangeColumnSeries ItemsSource = "{Binding Data}"
        ///                                   XBindingPath = "XValue"
        ///                                   High = "HighValue"
        ///                                   Low = "LowValue"
        ///                                   CornerRadius = "5"/>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-7)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     RangeColumnSeries rangeColumnSeries = new RangeColumnSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           High = "HighValue",
        ///           Low = "LowValue",
        ///           CornerRadius = new CornerRadius(5)
        ///     };
        ///     
        ///     chart.Series.Add(rangeColumnSeries);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value to change the width of the range column segment.
        /// </summary>
        /// <value>It accepts <c>double</c> values ranging from 0 to 1, where the default value is <c>0.8</c>.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-8)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:RangeColumnSeries ItemsSource = "{Binding Data}"
        ///                                   XBindingPath = "XValue"
        ///                                   High = "HighValue"
        ///                                   Low = "LowValue"
        ///                                   Width = "0.7"/>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-9)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     RangeColumnSeries rangeColumnSeries = new RangeColumnSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           High = "HighValue",
        ///           Low = "LowValue",
        ///           Width = 0.7,
        ///     };
        ///     
        ///     chart.Series.Add(rangeColumnSeries);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public double Width
        {
            get { return (double)GetValue(WidthProperty); }
            set { SetValue(WidthProperty, value); }
        }

        #endregion

        #region Interface Implementation

        void IDrawCustomLegendIcon.DrawSeriesLegend(ICanvas canvas, RectF rect, Brush fillColor, bool isSaveState)
        {
            if (isSaveState)
            {
                canvas.CanvasSaveState();
            }

            RectF innerRect1 = new(1, 4, 2, 8);
            canvas.SetFillPaint(fillColor, innerRect1);
            canvas.FillRectangle(innerRect1);

            RectF innerRect2 = new(5, 0, 2, 8);
            canvas.SetFillPaint(fillColor, innerRect2);
            canvas.FillRectangle(innerRect2);

            RectF innerRect3 = new(9, 3, 2, 7);
            canvas.SetFillPaint(fillColor, innerRect3);
            canvas.FillRectangle(innerRect3);

            if (isSaveState)
            {
                canvas.CanvasRestoreState();
            }
        }

        #endregion

        #region Methods

        #region Protected Methods

        /// <inheritdoc/>
        protected override ChartSegment CreateSegment()
        {
            return new RangeColumnSegment();
        }

        #endregion

        #region Internal Methods

		internal override void GenerateSegments(SeriesView seriesView)
		{
			var xValues = GetXValues();

			if (xValues != null)
			{
				for (var i = 0; i < PointsCount; i++)
				{
					var x = xValues[i];
					if (i < _segments.Count && _segments[i] is RangeColumnSegment)
					{
						((RangeColumnSegment)_segments[i]).SetData([x + SbsInfo.Start, x + SbsInfo.End, HighValues[i], LowValues[i], x]);
					}
					else
					{
						CreateSegment(seriesView, [x + SbsInfo.Start, x + SbsInfo.End, HighValues[i], LowValues[i], x], i);
					}
				}
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

        internal override TooltipInfo? GetTooltipInfo(ChartTooltipBehavior tooltipBehavior, float x, float y)
        {
            if (ChartArea == null || SeriesYValues == null)
			{
				return null;
			}

			var tooltipInfo = base.GetTooltipInfo(tooltipBehavior, x, y);

            if (tooltipInfo != null)
            {
                var index = tooltipInfo.Index;

                IList<double> lowValue = SeriesYValues[1];
                double lowValueContent = Convert.ToDouble(lowValue[index]);

                if (ChartArea.IsTransposed)
                {
                    var segment = (ChartSegment)_segments[index];
                    tooltipInfo.X = segment.SegmentBounds.Center.X;
                    tooltipInfo.Y = segment.SegmentBounds.Top;
                }

                if (!double.IsNaN(lowValueContent))
                {
                    tooltipInfo.Text += "/" + (lowValueContent == 0 ? lowValueContent.ToString("0.##") : lowValueContent.ToString("#.##"));
				}

                return tooltipInfo;
            }

            return null;
        }

        internal override void GenerateTrackballPointInfo(List<object> nearestDataPoints, List<TrackballPointInfo> PointInfos, ref bool isSideBySide)
        {
            var xValues = GetXValues();
            float xPosition = 0f;
            float yPosition = 0f;
            if (nearestDataPoints != null && ActualData != null && xValues != null && SeriesYValues != null)
            {
                IList<double> yValues = SeriesYValues[0];
                IList<double> yValues1 = SeriesYValues[1];
                foreach (object point in nearestDataPoints)
                {
                    int index = ActualData.IndexOf(point);
                    var xValue = xValues[index];
                    double yValue = yValues[index];
                    double yValue1 = yValues1[index];
                    string label = string.Format("{0} : {1:#.##}\n{2} : {3:#.##}", SfCartesianChartResources.High, yValue, SfCartesianChartResources.Low, yValue1);
					if (yValue == 0 || yValue1 == 0)
					{
						label = string.Format("{0} : {1:0.##}\n{2} : {3:0.##}", SfCartesianChartResources.High, yValue, SfCartesianChartResources.Low, yValue1);
					}

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
#if ANDROID
                            Size contentSize = ChartUtils.GetLabelSize(label, chartPointInfo.TooltipHelper);
                            chartPointInfo.GroupLabelSize = contentSize;
#endif
                        chartPointInfo.XValue = xValue;
                        chartPointInfo.YValues = [yValue, yValue1];
                        PointInfos.Add(chartPointInfo);
                    }
                }
            }
        }

        internal override void ApplyTrackballLabelFormat(TrackballPointInfo pointInfo, string labelFormat)
        {
            var yValues = pointInfo.YValues;
            string label = string.Format("{0} : {1:#.##}\n{2} : {3:#.##}", SfCartesianChartResources.High, yValues[0].ToString(labelFormat), SfCartesianChartResources.Low, yValues[1].ToString(labelFormat));

#if ANDROID
            Size contentSize = ChartUtils.GetLabelSize(label, pointInfo.TooltipHelper);
            pointInfo.GroupLabelSize = contentSize;
#endif
            pointInfo.Label = label;
        }

        internal override void DrawDataLabels(ICanvas canvas)
        {
            var dataLabelSettings = ChartDataLabelSettings;
            if (dataLabelSettings == null)
			{
				return;
			}

			ChartDataLabelStyle labelStyle = dataLabelSettings.LabelStyle;
            foreach (RangeColumnSegment dataLabel in _segments)
            {
                if (!dataLabel.InVisibleRange || dataLabel.IsZero)
				{
					continue;
				}

				RangeColumnSeriesDataLabelAppearance(canvas, dataLabel, dataLabelSettings, labelStyle);
            }
        }

        internal double GetDataLabelPositionAtIndex(int index, double value)
        {
            if (DataLabelSettings == null)
			{
				return 0;
			}

			var yValue = HighValues?[index] ?? 0f;
            var yValue1 = LowValues?[index] ?? 0f;
            var returnValue = value == yValue ? yValue : yValue1;

            return returnValue;
        }

        #endregion

        #region Private Methods

        void CreateSegment(SeriesView seriesView, double[] values, int index)
        {
            var rangeColumn = CreateSegment() as RangeColumnSegment;
            if (rangeColumn != null)
            {
                rangeColumn.Series = this;
                rangeColumn.SeriesView = seriesView;
                rangeColumn.SetData(values);
                rangeColumn.Index = index;
                rangeColumn.Item = ActualData?[index];
                InitiateDataLabels(rangeColumn);
                _segments.Add(rangeColumn);

                if (OldSegments != null && OldSegments.Count > 0 && OldSegments.Count > index)
                {
                    var oldSegment = OldSegments[index] as RangeColumnSegment;
                    if (oldSegment != null)
					{
						rangeColumn.SetPreviousData([oldSegment.Y1, oldSegment.Y2]);
					}
				}
            }
        }

        static void OnWidthChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is RangeColumnSeries rangeColumn)
            {
                rangeColumn.UpdateSbsSeries();
            }
        }

        static void OnSpacingChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is RangeColumnSeries rangeColumn)
            {
                rangeColumn.UpdateSbsSeries();
            }
        }

        static void OnCornerRadiusChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is RangeColumnSeries rangeColumn)
            {
                rangeColumn.InvalidateSeries();
            }
        }

        void RangeColumnSeriesDataLabelAppearance(ICanvas canvas, RangeColumnSegment dataLabel, ChartDataLabelSettings dataLabelSettings, ChartDataLabelStyle labelStyle)
        {
            for (int i = 0; i < 2; i++)
            {
                string labelText;
                PointF position;

                if (i == 0)
                {
                    labelText = dataLabel._dataLabel[0] ?? string.Empty;
                    position = dataLabel._labelPositionPoints[0];
                }
                else
                {
                    labelText = dataLabel._dataLabel[1] ?? string.Empty;
                    position = dataLabel._labelPositionPoints[1];
                }

                if (labelStyle.Angle != 0)
                {
                    float angle = (float)(labelStyle.Angle > 360 ? labelStyle.Angle % 360 : labelStyle.Angle);
                    canvas.CanvasSaveState();
                    canvas.Rotate(angle, position.X, position.Y);
                }

                canvas.StrokeSize = (float)labelStyle.StrokeWidth;
                canvas.StrokeColor = labelStyle.Stroke.ToColor();

                var fillColor = labelStyle.IsBackgroundColorUpdated ? labelStyle.Background : dataLabelSettings.UseSeriesPalette ? dataLabel.Fill : labelStyle.Background;
                DrawDataLabel(canvas, fillColor, labelText, position, dataLabel.Index);
            }
        }

        #endregion

        #endregion
    }
}