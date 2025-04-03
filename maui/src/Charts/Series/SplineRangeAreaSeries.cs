using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// The <see cref="SplineRangeAreaSeries"/> is a set of data points represented by smooth bezier curves, with the area between the curves filled to illustrate the range between two values.
	/// </summary>
	/// <remarks>
	/// <para>To render a series, create an instance of <see cref="SplineRangeAreaSeries"/> class, and add it to the <see cref="SfCartesianChart.Series"/> collection.</para>
	/// <para> <b>EnableTooltip - </b> A tooltip displays information while tapping or mouse hovering above a segment. To display the tooltip on a chart, you need to set the <see cref="ChartSeries.EnableTooltip"/> property as <b>True</b> in <see cref="SplineRangeAreaSeries"/> class, and also refer <seealso cref="ChartBase.TooltipBehavior"/> property.</para>
	/// <para> <b>Data Label - </b> Data labels are used to display values related to a chart segment. To render the data labels, you need to set the <see cref="ChartSeries.ShowDataLabels"/> property as <b>True</b> in <see cref="SplineRangeAreaSeries"/> class. To customize the chart data labels alignment, placement, and label styles, you need to create an instance of <see cref="CartesianDataLabelSettings"/> and set to the <see cref="CartesianSeries.DataLabelSettings"/> property.</para>
	/// <para> <b>Animation - </b> To animate the series, set <b>True</b> to the <see cref="ChartSeries.EnableAnimation"/> property.</para>
	/// <para> <b>LegendIcon - </b> To customize the legend icon use the <see cref="ChartSeries.LegendIcon"/> property.</para>
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
	///               <chart:SplineRangeAreaSeries
	///                   ItemsSource = "{Binding Data}"
	///                   XBindingPath = "XValue"
	///                   High = "HighValue"
	///                   Low = "LowValue"/>
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
	///     SplineRangeAreaSeries series = new SplineRangeAreaSeries();
	///     series.ItemsSource = viewModel.Data;
	///     series.XBindingPath = "XValue";
	///     series.High = "HighValue";
	///     series.Low = "LowValue";
	///     chart.Series.Add(series);
	///     
	/// ]]></code>
	/// # [ViewModel](#tab/tabid-3)
	/// <code><![CDATA[
	/// public ObservableCollection<Model> Data { get; set; }
	/// public ViewModel()
	/// {
	///    Data = new ObservableCollection<Model>();
	///    Data.Add(new Model() { XValue =  1, HighValue = 2, LowValue = 4 });
	///    Data.Add(new Model() { XValue =  2, HighValue = 4, LowValue = 6 });
	///    Data.Add(new Model() { XValue =  3, HighValue = 3, LowValue = 5 });
	///    Data.Add(new Model() { XValue =  4, HighValue = 6, LowValue = 8 });
	///    Data.Add(new Model() { XValue =  5, HighValue = 7, LowValue = 9 });
	/// }
	/// ]]></code>
	/// ***
	/// </example>
	public partial class SplineRangeAreaSeries : RangeSeriesBase, IMarkerDependent, IDrawCustomLegendIcon
    {
        #region Fields

        bool _needToAnimateMarker;

		internal override bool IsFillEmptyPoint { get { return false; } }

		#endregion

		#region Bindable Properties

		/// <summary>
		/// Identifies the <see cref="ShowMarkers"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="ShowMarkers"/> property determines whether the chart markers are visible on the series.
		/// </remarks>
		public static readonly BindableProperty ShowMarkersProperty = ChartMarker.ShowMarkersProperty;

        /// <summary>
        /// Identifies the <see cref="MarkerSettings"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="MarkerSettings"/> property allows customize the series markers.
        /// </remarks>
        public static readonly BindableProperty MarkerSettingsProperty = ChartMarker.MarkerSettingsProperty;

        /// <summary>
        /// Identifies the <see cref="Type"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="Type"/> property indicates the shape of the spline range area series.
        /// </remarks>
        public static readonly BindableProperty TypeProperty = BindableProperty.Create(
            nameof(Type),
            typeof(SplineType),
            typeof(SplineRangeAreaSeries),
            SplineType.Natural,
            BindingMode.Default,
            null,
            OnSplineTypeChanged);

        #endregion

        #region  Public Properties

        /// <summary>
        /// Gets or sets the value indicating whether to show markers for the series data point.
        /// </summary>
        /// <value>It accepts <c>bool</c> values and its default value is false.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-4)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:SplineRangeAreaSeries ItemsSource = "{Binding Data}"
        ///                                       XBindingPath = "XValue"
        ///                                       High = "HighValue"
        ///                                       Low = "LowValue"
        ///                                       ShowMarkers = "True"/>
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
        ///     SplineRangeAreaSeries series = new SplineRangeAreaSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           HighValue = "HighValue",
        ///           LowValue = "LowValue",
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
        /// Gets or sets the option for customize the series markers.
        /// </summary>
        /// <value>It accepts <see cref="ChartMarkerSettings"/>.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-6)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:SplineRangeAreaSeries ItemsSource = "{Binding Data}"
        ///                                       XBindingPath = "XValue"
        ///                                       High = "HighValue"
        ///                                       Low = "LowValue"
        ///                                       ShowMarkers = "True">
        ///               <chart:SplineRangeAreaSeries.MarkerSettings>
        ///                     <chart:ChartMarkerSettings Fill = "Red" Height = "15" Width = "15" />
        ///               </chart:SplineRangeAreaSeries.MarkerSettings>
        ///          </chart:SplineRangeAreaSeries>
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
        ///     SplineRangeAreaSeries series = new SplineRangeAreaSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           High = "HighValue",
        ///           Low = "LowValue",
        ///           ShowMarkers = true,
        ///           MarkerSettings= chartMarkerSettings,
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

        /// <summary>
        /// Gets or sets a value that indicates the shape of the spline range area series.
        /// </summary>
        /// <value>It accepts <see cref="SplineType"/> values and its default value is <see cref="SplineType.Natural"/>.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-8)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:SplineRangeAreaSeries ItemsSource = "{Binding Data}"
        ///                                       XBindingPath = "XValue"
        ///                                       High = "HighValue"
        ///                                       Low = "LowValue"
        ///                                       Type = "Monotonic"/>
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
        ///     SplineRangeAreaSeries series = new SplineRangeAreaSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           High = "HighValue",
        ///           Low = "LowValue",
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

		#region Constructor

		/// <summary>
		///  Initializes a new instance of the <see cref="SplineRangeAreaSeries"/>.
		/// </summary>
		public SplineRangeAreaSeries()
		{
			MarkerSettings = new ChartMarkerSettings();
		}

		#endregion

		#region Interface Implementation

		ChartMarkerSettings IMarkerDependent.MarkerSettings => MarkerSettings ?? new ChartMarkerSettings();

        bool IMarkerDependent.NeedToAnimateMarker { get => _needToAnimateMarker; set => _needToAnimateMarker = EnableAnimation; }

        void IMarkerDependent.DrawMarker(ICanvas canvas, int index, ShapeType type, Rect rect) => DrawMarker(canvas, index, type, rect);

        void IMarkerDependent.InvalidateDrawable() => InvalidateSeries();

        void IDrawCustomLegendIcon.DrawSeriesLegend(ICanvas canvas, RectF rect, Brush fillColor, bool isSaveState)
        {
            if (isSaveState)
            {
                canvas.CanvasSaveState();
            }

            PathF pathF = new PathF();
            pathF.MoveTo((float)5.45455, 1.92678f);
            pathF.CurveTo((float)2.16168, (float)-1.56276, (float)0.606061, 0.963388f, 0, (float)2.89016);
            pathF.LineTo(0, (float)9.89016);
            pathF.CurveTo((float)0.606061, (float)7.96339, (float)2.16168, (float)5.43724, (float)5.45455, (float)8.92677);
            pathF.CurveTo((float)7.72727, (float)11.3352, (float)9.69697, (float)9.2479, 10, 7);
            pathF.LineTo(10, 0);
            pathF.CurveTo((float)9.69697, (float)2.2479, (float)7.72727, (float)4.33524, (float)5.45455, (float)1.92678);
            pathF.Close();
            canvas.FillPath(pathF);

            if (isSaveState)
            {
                canvas.CanvasRestoreState();
            }
        }

        #endregion

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

            double highValue = HighValues[tooltipIndex];
            double lowValue = LowValues[tooltipIndex];

            if (double.IsNaN(highValue) && double.IsNaN(lowValue))
            {
                return -1;
            }

            PointF visiblePoint = new PointF();
            List<PointF> segPoints = [];
            var xValues = GetXValues();

            if (xValues != null)
            {
                double xValue = xValues[tooltipIndex];
                var visibleX = TransformToVisibleX(xValue, 0);

                if ((tooltipIndex == ActualData.Count - 1) || (pointX - (float)AreaBounds.Left) < visibleX)
                {
                    xValue = xValues[tooltipIndex];
                    double y1Value = HighValues[tooltipIndex];
                    double y2Value = LowValues[tooltipIndex];
                    visiblePoint.X = TransformToVisibleX(xValue, y1Value);
                    visiblePoint.Y = TransformToVisibleY(xValue, y1Value);
                    segPoints.Add(new PointF(visiblePoint.X, visiblePoint.Y));
                    visiblePoint.X = TransformToVisibleX(xValue, y2Value);
                    visiblePoint.Y = TransformToVisibleY(xValue, y2Value);
                    segPoints.Add(new PointF(visiblePoint.X, visiblePoint.Y));
                }
                else
                {
                    xValue = xValues[tooltipIndex + 1];
                    double y1Value = HighValues[tooltipIndex + 1];
                    double y2Value = LowValues[tooltipIndex + 1];
                    visiblePoint.X = TransformToVisibleX(xValue, y1Value);
                    visiblePoint.Y = TransformToVisibleY(xValue, y1Value);
                    segPoints.Add(new PointF(visiblePoint.X, visiblePoint.Y));
                    visiblePoint.X = TransformToVisibleX(xValue, y2Value);
                    visiblePoint.Y = TransformToVisibleY(xValue, y2Value);
                    segPoints.Add(new PointF(visiblePoint.X, visiblePoint.Y));
                }

                if (((pointX - (float)AreaBounds.Left) < visibleX) && tooltipIndex != 0)
                {
                    xValue = xValues[tooltipIndex - 1];
                    double y1Value = HighValues[tooltipIndex - 1];
                    double y2Value = LowValues[tooltipIndex - 1];
                    visiblePoint.X = TransformToVisibleX(xValue, y2Value);
                    visiblePoint.Y = TransformToVisibleY(xValue, y2Value);
                    segPoints.Add(new PointF(visiblePoint.X, visiblePoint.Y));
                    visiblePoint.X = TransformToVisibleX(xValue, y1Value);
                    visiblePoint.Y = TransformToVisibleY(xValue, y1Value);
                    segPoints.Add(new PointF(visiblePoint.X, visiblePoint.Y));
                }
                else if (((pointX - (float)AreaBounds.Left) > visibleX) || tooltipIndex == 0)
                {
                    xValue = xValues[tooltipIndex];
                    double y1Value = HighValues[tooltipIndex];
                    double y2Value = LowValues[tooltipIndex];
                    visiblePoint.X = TransformToVisibleX(xValue, y2Value);
                    visiblePoint.Y = TransformToVisibleY(xValue, y2Value);
                    segPoints.Add(new PointF(visiblePoint.X, visiblePoint.Y));
                    visiblePoint.X = TransformToVisibleX(xValue, y1Value);
                    visiblePoint.Y = TransformToVisibleY(xValue, y1Value);
                    segPoints.Add(new PointF(visiblePoint.X, visiblePoint.Y));
                }
            }

            var xPos = pointX - (float)AreaBounds.Left;

            var yPos = pointY - (float)AreaBounds.Top;
            if (ChartUtils.IsAreaContains(segPoints, xPos, yPos))
            {
                return tooltipIndex;
            }

            return -1;
        }

        #endregion

        #region protected Methods

        /// <summary>
        /// Draws the markers for the spline range area series.
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

        /// <inheritdoc/>
        protected override ChartSegment? CreateSegment()
        {
            return new SplineRangeAreaSegment();
        }

        #endregion

        #region Internal Methods

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

                if (double.IsNaN(lowValueContent))
                {
                    return null;
                }
                else if (!double.IsNaN(lowValueContent))
                {
                    tooltipInfo.Text += "/" + (lowValueContent == 0 ? lowValueContent.ToString("0.##") : lowValueContent.ToString("#.##"));
				}

                return tooltipInfo;
            }

            return null;
        }

        internal override void GenerateTrackballPointInfo(List<object> nearestDataPoints, List<TrackballPointInfo> pointInfos, ref bool isSideBySide)
        {
            var xValues = GetXValues();

            if (nearestDataPoints != null && ActualData != null && xValues != null && SeriesYValues != null && HighValues != null)
            {
                IList<double> topValues = HighValues;
                IList<double> highValues = SeriesYValues[0];
                IList<double> lowValues = SeriesYValues[1];

                foreach (object point in nearestDataPoints)
                {
                    int index = ActualData.IndexOf(point);
                    var xValue = xValues[index];
                    double topValue = topValues[index];
                    double highValue = highValues[index];
                    double lowValue = lowValues[index];
                    string label = string.Format("{0} : {1:#.##}\n{2} : {3:#.##}", SfCartesianChartResources.High, highValue, SfCartesianChartResources.Low, lowValue);
					if (highValue == 0 || lowValue == 0)
					{
						label = string.Format("{0} : {1:0.##}\n{2} : {3:0.##}", SfCartesianChartResources.High, highValue, SfCartesianChartResources.Low, lowValue);
					}

					var xPoint = TransformToVisibleX(xValue, topValue);
                    var yPoint = TransformToVisibleY(xValue, topValue);

                    TrackballPointInfo? chartPointInfo = CreateTrackballPointInfo(xPoint, yPoint, label, point);

                    if (chartPointInfo != null)
                    {
#if ANDROID
                        Size contentSize = ChartUtils.GetLabelSize(label, chartPointInfo.TooltipHelper);
                        chartPointInfo.GroupLabelSize = contentSize;
#endif
                        chartPointInfo.XValue = xValue;
                        pointInfos.Add(chartPointInfo);
                    }
                }
            }
        }

        internal override void GenerateSegments(SeriesView seriesView)
        {
            var xValues = GetXValues();

            if (xValues == null || xValues.Count == 0)
            {
                return;
            }
            double[] highStartControlPointsX = new double[PointsCount - 1];
            double[] highStartControlPointsY = new double[PointsCount - 1];
            double[] highEndControlPointsX = new double[PointsCount - 1];
            double[] highEndControlPointsY = new double[PointsCount - 1];

            double[] lowStartControlPointsX = new double[PointsCount - 1];
            double[] lowStartControlPointsY = new double[PointsCount - 1];
            double[] lowEndControlPointsX = new double[PointsCount - 1];
            double[] lowEndControlPointsY = new double[PointsCount - 1];
            double[]? dx = null;

            double[]? highYCoef;
            double[]? lowYCoef;


            switch (Type)
            {
                case SplineType.Monotonic:
                    highYCoef = GetMonotonicSpline(xValues, HighValues, out dx);
                    lowYCoef = GetMonotonicSpline(xValues, LowValues, out dx);
                    break;

                case SplineType.Cardinal:
                    highYCoef = GetCardinalSpline(xValues, HighValues);
                    lowYCoef = GetCardinalSpline(xValues, LowValues);
                    break;

                default:
                    highYCoef = NaturalSpline(HighValues, Type);
                    lowYCoef = NaturalSpline(LowValues, Type);
                    break;
            }

            for (int i = 0; i < PointsCount - 1; i++)
            {
                var x = xValues[i];
                var high = HighValues[i];
                var low = LowValues[i];

                var nextX = xValues[i + 1];
                var nextHigh = HighValues[i + 1];
                var nextLow = LowValues[i + 1];

                List<double>? highControlPoints;
                List<double>? lowControlPoints;

                if (highYCoef != null && lowYCoef != null)
                {
                    if (dx != null && Type == SplineType.Monotonic && dx.Length > 0)
                    {
                        highControlPoints = CartesianSeries.CalculateControlPoints(x, high, nextX, nextHigh, highYCoef[i], highYCoef[i + 1], dx[i]);
                        lowControlPoints = CartesianSeries.CalculateControlPoints(x, low, nextX, nextLow, lowYCoef[i], lowYCoef[i + 1], dx[i]);
                    }
                    else if (Type == SplineType.Cardinal)
                    {
                        highControlPoints = CartesianSeries.CalculateControlPoints(x, high, nextX, nextHigh, highYCoef[i], highYCoef[i + 1]);
                        lowControlPoints = CartesianSeries.CalculateControlPoints(x, low, nextX, nextLow, lowYCoef[i], lowYCoef[i + 1]);
                    }
                    else
                    {
                        highControlPoints = CalculateControlPoints(HighValues, highYCoef[i], highYCoef[i + 1], i);
                        lowControlPoints = CalculateControlPoints(LowValues, lowYCoef[i], lowYCoef[i + 1], i);
                    }

                    if (highControlPoints != null && lowControlPoints != null)
                    {
                        highStartControlPointsX[i] = highControlPoints[0];
                        lowStartControlPointsX[i] = lowControlPoints[0];
                        highStartControlPointsY[i] = highControlPoints[1];
                        lowStartControlPointsY[i] = lowControlPoints[1];

                        highEndControlPointsX[i] = highControlPoints[2];
                        lowEndControlPointsX[i] = lowControlPoints[2];
                        highEndControlPointsY[i] = highControlPoints[3];
                        lowEndControlPointsY[i] = lowControlPoints[3];
                    }
                }
            }

            CreateSegment(
                seriesView,
                xValues,
                HighValues,
                highStartControlPointsX,
                highStartControlPointsY,
                highEndControlPointsX,
                highEndControlPointsY,
                LowValues,
                lowStartControlPointsX,
                lowStartControlPointsY,
                lowEndControlPointsX,
                lowEndControlPointsY);
        }

        internal override void CalculateDataPointPosition(int index, ref double x, ref double y)
        {
            if (ChartArea == null)
            {
                return;
            }
            var xCal = x;
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

        internal override void DrawDataLabels(ICanvas canvas)
        {
            var dataLabelSettings = DataLabelSettings;

            if (dataLabelSettings == null || _segments == null || _segments.Count <= 0)
            {
                return;
            }

            ChartDataLabelStyle labelStyle = DataLabelSettings.LabelStyle;

            foreach (SplineRangeAreaSegment dataLabel in _segments.Cast<SplineRangeAreaSegment>())
            {
                if (dataLabel == null || dataLabel.XVal == null || dataLabel.HighVal == null || dataLabel.LowVal == null)
                {
                    return;
                }

                for (int i = 0; i < dataLabel.XVal.Length; i++)
                {
                    dataLabel.Index = i;
                    for (int j = 0; j < 2; j++)
                    {
                        if (j == 0)
                        {
                            double x = dataLabel.XVal[i];
                            double y = dataLabel.HighVal[i];

                            if (double.IsNaN(y) || !IsDataInVisibleRange(x, y))
                            {
                                continue;
                            }

                            CalculateDataPointPosition(i, ref x, ref y);
                            PointF labelPoint = new PointF((float)x, (float)y);
                            _sumOfHighValues = float.IsNaN(_sumOfHighValues) ? SumOfValues(HighValues) : _sumOfHighValues;
                            dataLabel.LabelContent = GetLabelContent(dataLabel.HighVal[i], _sumOfHighValues);
                            dataLabel.LabelPositionPoint = dataLabelSettings.CalculateDataLabelPoint(this, dataLabel, labelPoint, dataLabelSettings.LabelStyle, "HighType");
                            UpdateDataLabelAppearance(canvas, dataLabel, dataLabelSettings, labelStyle);
                        }
                        else
                        {
                            double x = dataLabel.XVal[i];
                            double y = dataLabel.LowVal[i];

                            if (double.IsNaN(y) || !IsDataInVisibleRange(x, y))
                            {
                                continue;
                            }

                            CalculateDataPointPosition(i, ref x, ref y);
                            PointF labelPoint = new PointF((float)x, (float)y);
                            _sumOfLowValues = float.IsNaN(_sumOfLowValues) ? SumOfValues(LowValues) : _sumOfLowValues;
                            dataLabel.LabelContent = GetLabelContent(dataLabel.LowVal[i], _sumOfLowValues);
                            dataLabel.LabelPositionPoint = dataLabelSettings.CalculateDataLabelPoint(this, dataLabel, labelPoint, dataLabelSettings.LabelStyle, "LowType");
                            UpdateDataLabelAppearance(canvas, dataLabel, dataLabelSettings, labelStyle);
                        }
                    }

                }
            }
        }

        internal override Brush? GetSegmentFillColor(int index)
        {
            var segment = _segments[0];

            if (segment != null)
            {
                return segment.Fill;
            }

            return null;
        }

        internal override void InitiateDataLabels(ChartSegment segment)
        {
            for (int i = 0; i < PointsCount; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    var dataLabel = new ChartDataLabel();
                    segment.DataLabels.Add(dataLabel);
                    DataLabels.Add(dataLabel);
                }
            }
        }

        #endregion

        #region Private Methods

        void CreateSegment(SeriesView seriesView, IList<double> xVals, IList<double> highVals, double[] highStartPointsX, double[] highStartPointsY, double[] highEndPointsX, double[] highEndPointsY, IList<double> lowVals, double[] lowStartPointsX, double[] lowStartPointsY, double[] lowEndPointsX, double[] lowEndPointsY)
        {
            List<double>? xValues = null, highValues = null,
            highStartControlPointsX = null,
            highStartControlPointsY = null,
            highEndControlPointsX = null,
            highEndControlPointsY = null,
            lowValues = null,
            lowStartControlPointsX = null,
            lowStartControlPointsY = null,
            lowEndControlPointsX = null,
            lowEndControlPointsY = null;
            List<object>? items = null;

            for (int i = 0; i < PointsCount; i++)
            {
                var highValue = HighValues[i];
                var lowValue = LowValues[i];

                if (!double.IsNaN(highValue) && !double.IsNaN(lowValue))
                {
                    if (xValues == null)
                    {
                        xValues = [];
                        highValues = [];
                        highStartControlPointsX = [];
                        highStartControlPointsY = [];
                        highEndControlPointsX = [];
                        highEndControlPointsY = [];
                        lowValues = [];
                        lowStartControlPointsX = [];
                        lowStartControlPointsY = [];
                        lowEndControlPointsX = [];
                        lowEndControlPointsY = [];
                        items = [];
                    }

                    xValues.Add(xVals[i]);
                    highValues?.Add(highVals[i]);
                    lowValues?.Add(lowVals[i]);
                    items?.Add(ActualData![i]);

                    if (i != PointsCount - 1)
                    {
                        highStartControlPointsX?.Add(highStartPointsX[i]);
                        highStartControlPointsY?.Add(highStartPointsY[i]);
                        highEndControlPointsX?.Add(highEndPointsX[i]);
                        highEndControlPointsY?.Add(highEndPointsY[i]);
                        lowStartControlPointsX?.Add(lowStartPointsX[i]);
                        lowStartControlPointsY?.Add(lowStartPointsY[i]);
                        lowEndControlPointsX?.Add(lowEndPointsX[i]);
                        lowEndControlPointsY?.Add(lowEndPointsY[i]);
                    }
                }

                if ((double.IsNaN(highValue) || double.IsNaN(lowValue)) || i == PointsCount - 1)
                {
                    if (xValues != null)
                    {
						if (CreateSegment() is SplineRangeAreaSegment segment)
						{
							segment.Series = this;
							segment.SeriesView = seriesView;

							if (highValues != null && highStartControlPointsX != null && highStartControlPointsY != null && highStartControlPointsY != null &&
							   highEndControlPointsX != null && highEndControlPointsY != null && lowValues != null && lowStartControlPointsX != null &&
							   lowStartControlPointsY != null && lowEndControlPointsX != null && lowEndControlPointsY != null)
							{
								segment.SetData(
								xValues,
								highValues,
								highStartControlPointsX,
								highStartControlPointsY,
								highEndControlPointsX,
								highEndControlPointsY,
								lowValues,
								lowStartControlPointsX,
								lowStartControlPointsY,
								lowEndControlPointsX,
								lowEndControlPointsY);
							}

							segment.Item = items;
							InitiateDataLabels(segment);
							_segments.Add(segment);
						}

						highValues = xValues = highStartControlPointsX = highStartControlPointsY = highEndControlPointsX = highEndControlPointsY = null;
                        lowValues = lowStartControlPointsX = lowStartControlPointsY = lowEndControlPointsX = lowEndControlPointsY = null;
                        items = null;
                    }
                }

                if (double.IsNaN(highValue) || double.IsNaN(lowValue))
                {
                    xValues = [xVals[i]];
                    highValues = [highVals[i]];
                    highStartControlPointsX = [];
                    highStartControlPointsY = [];
                    highEndControlPointsX = [];
                    highEndControlPointsY = [];

                    lowValues = [lowVals[i]];
                    lowStartControlPointsX = [];
                    lowStartControlPointsY = [];
                    lowEndControlPointsX = [];
                    lowEndControlPointsY = [];

					if (CreateSegment() is SplineRangeAreaSegment segment)
					{
						segment.Series = this;
						segment.SeriesView = seriesView;
						segment.Item = items;
						segment.SetData(
							xValues,
							highValues,
							highStartControlPointsX,
							highStartControlPointsY,
							highEndControlPointsX,
							highEndControlPointsY,
							lowValues,
							lowStartControlPointsX,
							lowStartControlPointsY,
							lowEndControlPointsX,
							lowEndControlPointsY);
					}

					highValues = xValues = highStartControlPointsX = highStartControlPointsY = highEndControlPointsX = highEndControlPointsY = null;
                    lowValues = lowStartControlPointsX = lowStartControlPointsY = lowEndControlPointsX = lowEndControlPointsY = null;
                    items = null;
                }
            }
        }

        static void OnSplineTypeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SplineRangeAreaSeries series)
            {
                series.SegmentsCreated = false;
                series.ScheduleUpdateChart();
            }
        }

        #endregion

        #endregion
    }
}