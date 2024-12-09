using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// The <see cref="StackingLineSeries"/> is a collection of data points, where the lines are stacked on top of each other.
	/// </summary>
	/// <remarks>
	/// <para>To render a series, create an instance of <see cref="StackingLineSeries"/> class, and add it to the <see cref="SfCartesianChart.Series"/> collection.</para>
	/// 
	/// <para>It provides options for <see cref="ChartSeries.Fill"/>, <see cref="ChartSeries.PaletteBrushes"/>, <see cref="XYDataSeries.StrokeWidth"/>, <see cref="StackingSeriesBase.StrokeDashArray"/>, and <see cref="ChartSeries.Opacity"/> to customize the appearance.</para>
	/// <para>Utilize the <see cref="ChartSeries.Fill"/> property to customize the line stroke.</para>
	/// <para> <b>EnableTooltip - </b> A tooltip displays information while tapping or mouse hovering above a segment. To display the tooltip on a chart, you need to set the <see cref="ChartSeries.EnableTooltip"/> property as <b>true</b> in <see cref="StackingLineSeries"/> class, and also refer <seealso cref="ChartBase.TooltipBehavior"/> property.</para>
	/// <para> <b>Data Label - </b> Data labels are used to display values related to a chart segment. To render the data labels, you need to set the <see cref="ChartSeries.ShowDataLabels"/> property as <b>true</b> in <see cref="StackingLineSeries"/> class. To customize the chart data labels placement, and label styles, you need to create an instance of <see cref="CartesianDataLabelSettings"/> and set to the <see cref="CartesianSeries.DataLabelSettings"/> property.</para>
	/// <para> <b>Animation - </b> To animate the series, set <b>True</b> to the <see cref="ChartSeries.EnableAnimation"/> property.</para>
	/// <para> <b>LegendIcon - </b> To customize the legend icon using the <see cref="ChartSeries.LegendIcon"/> property.</para>
	/// </remarks>
	/// <example>
	/// # [Xaml](#tab/tabid-1)
	/// <code><![CDATA[
	/// <chart:SfCartesianChart>
	///     <chart:SfCartesianChart.XAxes>
	///         <chart:CategoryAxis/>
	///     </chart:SfCartesianChart.XAxes>
	///
	///     <chart:SfCartesianChart.YAxes>
	///         <chart:NumericalAxis/>
	///     </chart:SfCartesianChart.YAxes>
	///
	///     <chart:StackingLineSeries
	///         ItemsSource = "{Binding MedalDetails}"
	///         XBindingPath = "CountryName"
	///         YBindingPath = "GoldMedals"/>
	///
	///     <chart:StackingLineSeries
	///         ItemsSource = "{Binding MedalDetails}"
	///         XBindingPath = "CountryName"
	///         YBindingPath = "SilverMedals"/>
	///
	///     <chart:StackingLineSeries
	///         ItemsSource = "{Binding MedalDetails}"
	///         XBindingPath = "CountryName"
	///         YBindingPath = "BronzeMedals"/>
	/// </chart:SfCartesianChart>
	/// ]]></code>
	/// # [C#](#tab/tabid-2)
	/// <code><![CDATA[
	/// SfCartesianChart chart = new SfCartesianChart();
	///
	/// CategoryAxis xAxis = new CategoryAxis();
	/// NumericalAxis yAxis = new NumericalAxis();
	///
	/// chart.XAxes.Add(xAxis);
	/// chart.YAxes.Add(yAxis);
	///
	/// ViewModel viewModel = new ViewModel();
	///
	/// StackingLineSeries goldSeries = new StackingLineSeries();
	/// goldSeries.ItemsSource = viewModel.MedalDetails;
	/// goldSeries.XBindingPath = "CountryName";
	/// goldSeries.YBindingPath = "GoldMedals";
	///
	/// StackingLineSeries silverSeries = new StackingLineSeries();
	/// silverSeries.ItemsSource = viewModel.MedalDetails;
	/// silverSeries.XBindingPath = "CountryName";
	/// silverSeries.YBindingPath = "SilverMedals";
	///
	/// StackingLineSeries bronzeSeries = new StackingLineSeries();
	/// bronzeSeries.ItemsSource = viewModel.MedalDetails;
	/// bronzeSeries.XBindingPath = "CountryName";
	/// bronzeSeries.YBindingPath = "BronzeMedals";
	///
	/// chart.Series.Add(goldSeries);
	/// chart.Series.Add(silverSeries);
	/// chart.Series.Add(bronzeSeries);
	///
	/// this.Content = chart;
	///     
	/// ]]></code>
	/// # [ViewModel](#tab/tabid-3)
	/// <code><![CDATA[
	///     public ObservableCollection<MedalData> MedalDetails { get; set; }
	///
	///     public ViewModel()
	///     {
	///         MedalDetails = new ObservableCollection<MedalData>
	///         {
	///             new MedalData() { CountryName = "USA", GoldMedals = 10, SilverMedals = 5, BronzeMedals = 7 },
	///             new MedalData() { CountryName = "China", GoldMedals = 8, SilverMedals = 10, BronzeMedals = 6 },
	///             new MedalData() { CountryName = "Russia", GoldMedals = 6, SilverMedals = 4, BronzeMedals = 8 },
	///             new MedalData() { CountryName = "UK", GoldMedals = 4, SilverMedals = 7, BronzeMedals = 3 }
	///         };
	///     }
	/// ]]></code>
	/// ***
	/// </example>
	public partial class StackingLineSeries : StackingSeriesBase, IMarkerDependent, IDrawCustomLegendIcon
    {
        #region Fields

        bool _needToAnimateMarker;

        #endregion

        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="ShowMarkers"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="ShowMarkers"/> property determines whether markers are displayed on the chart points.
        /// </remarks>
        public static readonly BindableProperty ShowMarkersProperty = ChartMarker.ShowMarkersProperty;

        /// <summary>
        /// Identifies the <see cref="MarkerSettings"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="MarkerSettings"/> property allows customization of the series markers.
        /// </remarks>
        public static readonly BindableProperty MarkerSettingsProperty = ChartMarker.MarkerSettingsProperty;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the value indicating whether to show markers for the series data point.
        /// </summary>
        /// <value>It accepts <c>bool</c>> values and its default value is false.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-4)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:StackingLineSeries ItemsSource = "{Binding Data}"
        ///                                    XBindingPath = "XValue"
        ///                                    YBindingPath = "YValue"
        ///                                    ShowMarkers = "True"/>
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
        ///     StackingLineSeries series = new StackingLineSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           ShowMarkers = true,
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public bool ShowMarkers
        {
            get { return (bool)GetValue(ShowMarkersProperty); }
            set { SetValue(ShowMarkersProperty, value); }
        }

        /// <summary>
        /// Gets or sets the option to customize the series markers.
        /// </summary>
        /// <value>It accepts <see cref="ChartMarkerSettings"/>.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-6)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:StackingLineSeries ItemsSource = "{Binding Data}"
        ///                                    XBindingPath = "XValue"
        ///                                    YBindingPath = "YValue"
        ///                                    ShowMarkers = "True">
        ///               <chart:StackingLineSeries.MarkerSettings>
        ///                     <chart:ChartMarkerSettings Fill = "Red" Height = "15" Width = "15" />
        ///               </chart:StackingLineSeries.MarkerSettings>
        ///          </chart:StackingLineSeries>
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
        ///    ChartMarkerSettings chartMarkerSettings = new ChartMarkerSettings()
        ///    {
        ///        Fill = new SolidColorBrush(Colors.Red),
        ///        Height = 15,
        ///        Width = 15,
        ///    };
        ///     StackingLineSeries series = new StackingLineSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           ShowMarkers = true,
        ///           MarkerSettings = chartMarkerSettings,
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public ChartMarkerSettings MarkerSettings
        {
            get { return (ChartMarkerSettings)GetValue(MarkerSettingsProperty); }
            set { SetValue(MarkerSettingsProperty, value); }
        }

        #endregion

        #region Interface Implementation

        bool IMarkerDependent.NeedToAnimateMarker { get => _needToAnimateMarker; set => _needToAnimateMarker = EnableAnimation; }

        void IDrawCustomLegendIcon.DrawSeriesLegend(ICanvas canvas, RectF rect, Brush fillColor, bool isSaveState)
        {
            if (isSaveState)
            {
                canvas.CanvasSaveState();
            }

            var pathF = new PathF();
            pathF.MoveTo(0.552786f, 1.22361f);
            pathF.LineTo(3.91459f, 7.94721f);
            pathF.LineTo(6.91459f, 3.94721f);
            pathF.LineTo(10.5528f, 11.2236f);
            pathF.LineTo(11.4472f, 10.7764f);
            pathF.LineTo(7.08541f, 2.05279f);
            pathF.LineTo(4.08541f, 6.05279f);
            pathF.LineTo(1.44721f, 0.776394f);
            pathF.LineTo(0.552786f, 1.22361f);
            pathF.Close();
            canvas.FillPath(pathF);

            if (isSaveState)
            {
                canvas.CanvasRestoreState();
            }
        }

        void IMarkerDependent.InvalidateDrawable()
        {
            InvalidateSeries();
        }

        ChartMarkerSettings IMarkerDependent.MarkerSettings => MarkerSettings ?? new ChartMarkerSettings();

        void IMarkerDependent.DrawMarker(ICanvas canvas, int index, ShapeType type, Rect rect) => DrawMarker(canvas, index, type, rect);

        #endregion

        #region Methods

        #region Protected Methods

        ///  <inheritdoc/>
        protected override ChartSegment CreateSegment()
        {
            return new LineSegment();
        }

        /// <summary>
        /// Draws the markers for the stacking line series at each data point location on the chart.
        /// </summary>
        protected virtual void DrawMarker(ICanvas canvas, int index, ShapeType type, Rect rect)
        {
            if (this is IMarkerDependent markerDependent)
            {
                canvas.DrawShape(rect, shapeType: type, markerDependent.MarkerSettings.HasBorder, false);
            }
        }

        /// <inheritdoc/>
        /// <exclude/>
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (MarkerSettings != null)
            {
                SetInheritedBindingContext(MarkerSettings, BindingContext);
            }
        }

        #endregion

        #region Internal Methods

        internal override void GenerateSegments(SeriesView seriesView)
        {
            var xValues = GetXValues();
            if (xValues == null)
            {
                return;
            }

            if (BottomValues == null)
            {
                ChartArea?.UpdateStackingSeries();
            }

            if (BottomValues == null)
            {
                return;
            }

            var yEnd = TopValues;
            if (PointsCount == 1)
            {
                CreateSegment(seriesView, [xValues[0], YValues[0], double.NaN, double.NaN], 0);
            }
            else
            {
                for (int i = 0; i < PointsCount; i++)
                {
                    if (i < _segments.Count)
                    {
						_segments[i].SetData([xValues[i], yEnd![i], xValues[i + 1], yEnd[i + 1]]);
                    }
                    else
                    {
                        if (double.IsNaN(YValues[i]))
                        {
                            if (i == PointsCount - 1)
                            {
                                CreateSegment(seriesView, [xValues[i], yEnd![i], double.NaN, double.NaN], i);
                            }
                            else
							{
								CreateSegment(seriesView, [xValues[i], YValues[i], xValues[i + 1], yEnd![i + 1]], i);
							}
						}
                        else if (i < PointsCount - 1 && double.IsNaN(YValues[i + 1]))
                        {
                            CreateSegment(seriesView, [xValues[i], yEnd![i], xValues[i + 1], YValues[i + 1]], i);
                        }
                        else
                        {
                            if (i == PointsCount - 1)
                            {
                                CreateSegment(seriesView, [xValues[i], yEnd![i], double.NaN, double.NaN], i);
                            }
                            else
                            {
                                CreateSegment(seriesView, [xValues[i], yEnd![i], xValues[i + 1], yEnd![i + 1]], i);
                            }
                        }
                    }
                }
            }
        }

        internal override void GenerateTrackballPointInfo(List<object> nearestDataPoints, List<TrackballPointInfo> pointInfos, ref bool isSideBySide)
        {
            var xValues = GetXValues();
            if (nearestDataPoints != null && ActualData != null && xValues != null && SeriesYValues != null && TopValues != null)
            {
                IList<double> topValues = TopValues;
                IList<double> yValues = SeriesYValues[0];

                foreach (object point in nearestDataPoints)
                {
                    int index = ActualData.IndexOf(point);
                    var xValue = xValues[index];
                    double topValue = topValues[index];
                    double yValue = yValues[index];
                    if (double.IsNaN(yValue))
                    {
                        continue;
                    }

                    string label = yValue.ToString();
                    var xPoint = TransformToVisibleX(xValue, topValue);
                    var yPoint = TransformToVisibleY(xValue, topValue);

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

        internal override PointF GetDataLabelPosition(ChartSegment dataLabel, SizeF labelSize, PointF labelPosition, float padding)
        {
            return DataLabelSettings.GetLabelPositionForContinuousSeries(this, dataLabel.Index, labelSize, labelPosition, padding);
        }

        internal override void DrawDataLabels(ICanvas canvas)
        {
            var dataLabelSettings = DataLabelSettings;

            List<double> xValues = GetXValues()!;

            IList<double> actualYValues = YValues;

            if (dataLabelSettings == null || _segments == null || _segments.Count <= 0)
            {
                return;
            }

            ChartDataLabelStyle labelStyle = DataLabelSettings.LabelStyle;

            foreach (LineSegment dataLabel in _segments.Cast<LineSegment>())
            {
                if (dataLabel == null)
                {
                    return;
                }

                int indexValue = dataLabel.Index;
                double x = xValues[indexValue];
                double y = TopValues![indexValue];
                double currentValue = YValues[indexValue];
                if (double.IsNaN(currentValue))
                {
                    continue;
                }

                CalculateDataPointPosition(indexValue, ref x, ref y);
                PointF labelPoint = new PointF((float)x, (float)y);
                dataLabel.LabelContent = GetLabelContent(currentValue, SumOfValues(YValues));
				dataLabel.LabelPositionPoint = CartesianDataLabelSettings.CalculateDataLabelPoint(this, dataLabel, labelPoint, labelStyle);
                UpdateDataLabelAppearance(canvas, dataLabel, dataLabelSettings, labelStyle);
            }
        }

        internal override TooltipInfo? GetTooltipInfo(ChartTooltipBehavior tooltipBehavior, float x, float y)
        {
            if (_segments == null)
			{
				return null;
			}

			int index = SeriesContainsPoint(new PointF(x, y)) ? TooltipDataPointIndex : -1;

            if (index < 0 || ItemsSource == null || ActualData == null || ActualXAxis == null
                || ActualYAxis == null || SeriesYValues == null)
            {
                return null;
            }

            var xValues = GetXValues();

            if (xValues == null || ChartArea == null || TopValues == null)
			{
				return null;
			}

			object dataPoint = ActualData[index];
            double xValue = xValues[index];
            IList<double> yValues = SeriesYValues[0];
            double content = Convert.ToDouble(yValues[index]);
            double yValue = TopValues[index];
            float xPosition = TransformToVisibleX(xValue, yValue);

            if (!double.IsNaN(xPosition) && !double.IsNaN(yValue) && !double.IsNaN(content))
            {
                float yPosition = TransformToVisibleY(xValue, yValue);

                RectF seriesBounds = AreaBounds;
                seriesBounds = new RectF(0, 0, seriesBounds.Width, seriesBounds.Height);
                yPosition = Math.Min(Math.Max(seriesBounds.Top, yPosition), seriesBounds.Bottom);
                xPosition = Math.Min(Math.Max(seriesBounds.Left, xPosition), seriesBounds.Right);

				TooltipInfo tooltipInfo = new TooltipInfo(this)
				{
					X = xPosition,
					Y = yPosition,
					Index = index,
					Margin = tooltipBehavior.Margin,
					TextColor = tooltipBehavior.TextColor,
					FontFamily = tooltipBehavior.FontFamily,
					FontSize = tooltipBehavior.FontSize,
					FontAttributes = tooltipBehavior.FontAttributes,
					Background = tooltipBehavior.Background,
					Text = content.ToString(),
					Item = dataPoint
				};

				return tooltipInfo;
            }

            return null;
        }

        internal override bool SeriesContainsPoint(PointF point)
        {
            if (Chart != null)
            {
                if (base.SeriesContainsPoint(point))
                {
                    return true;
                }

                var dataPoint = FindNearestChartPoint(point.X, point.Y);

                if (dataPoint == null || ActualData == null)
                {
                    return false;
                }

                TooltipDataPointIndex = ActualData.IndexOf(dataPoint);

                if (_segments.Count == 0 || TooltipDataPointIndex < 0 || double.IsNaN(YValues[TooltipDataPointIndex]))
                {
                    return false;
                }

                LineSegment? startSegment = null;
                LineSegment? endSegment = null;
                var seriesClipRect = AreaBounds;
                point.X -= ((float)seriesClipRect.Left);
                point.Y -= ((float)seriesClipRect.Top);

                if (TooltipDataPointIndex == 0)
                {
                    startSegment = _segments[TooltipDataPointIndex] as LineSegment;
                }
                else if (TooltipDataPointIndex == PointsCount - 1)
                {
                    startSegment = _segments[TooltipDataPointIndex - 1] as LineSegment;
                }
                else
                {
                    startSegment = _segments[TooltipDataPointIndex - 1] as LineSegment;
                    endSegment = _segments[TooltipDataPointIndex] as LineSegment;
                }

                return SegmentContains(startSegment, endSegment, point, this);
            }

            return false;
        }

        internal override void InitiateDataLabels(ChartSegment segment)
        {
            for (int i = 0; i < PointsCount; i++)
            {
                var dataLabel = new ChartDataLabel();
                segment.DataLabels.Add(dataLabel);
                DataLabels.Add(dataLabel);
            }
        }

        #endregion

        #region Private Methods

        void CreateSegment(SeriesView seriesView, double[] values, int index)
        {
			if (CreateSegment() is LineSegment segment)
			{
				segment.Series = this;
				segment.SeriesView = seriesView;
				segment.Index = index;
				segment.SetData(values);
				InitiateDataLabels(segment);
				_segments.Add(segment);
			}
		}

        bool SegmentContains(ChartSegment? startSegment, ChartSegment? endSegment, PointF point, ChartSeries series)
        {
            if (startSegment != null && ChartUtils.SegmentContains((LineSegment)startSegment, point, this))
            {
                return true;
            }

            if (endSegment != null)
            {
                return ChartUtils.SegmentContains((LineSegment)endSegment, point, this);
            }

            return false;
        }

        #endregion 

        #endregion
    }
}