using System.Collections.ObjectModel;
using System.ComponentModel;
using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// The <see cref="HistogramSeries"/> displays the distribution of a dataset by dividing it into intervals and showing the count of values within each interval.
	/// </summary>
	/// <remarks>
	/// <para>To render a series, create an instance of <see cref="HistogramSeries"/> class, and add it to the <see cref="SfCartesianChart.Series"/> collection.</para>
	/// 
	/// <para>It provides options for <see cref="ChartSeries.Fill"/>, <see cref="ChartSeries.PaletteBrushes"/>, <see cref="XYDataSeries.StrokeWidth"/>, <see cref="Stroke"/>, and <see cref="ChartSeries.Opacity"/> to customize the appearance.</para>
	/// 
	/// <para> <b>EnableTooltip - </b> A tooltip displays information while tapping or mouse hovering above a segment. To display the tooltip on a chart, you need to set the <see cref="ChartSeries.EnableTooltip"/> property as <b>true</b> in <see cref="HistogramSeries"/> class, and also refer <seealso cref="ChartBase.TooltipBehavior"/> property.</para>
	/// <para> <b>Data Label - </b> Data labels are used to display values related to a chart segment. To render the data labels, you need to set the <see cref="ChartSeries.ShowDataLabels"/> property as <b>true</b> in <see cref="HistogramSeries"/> class. To customize the chart data labels alignment, placement, and label styles, you need to create an instance of <see cref="CartesianDataLabelSettings"/> and set to the <see cref="CartesianSeries.DataLabelSettings"/> property.</para>
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
	///               <chart:HistogramSeries
	///                   ItemsSource="{Binding Data}"
	///                   XBindingPath="XValue"
	///                   YBindingPath="YValue"
	///                   HistogramInterval="10"/>
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
	///     HistogramSeries series = new HistogramSeries();
	///     series.ItemsSource = viewModel.Data;
	///     series.XBindingPath = "XValue";
	///     series.YBindingPath = "YValue";
	///     series.HistogramInterval = 10;
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
	///        Data.Add(new Model() { XValue = 9, YValue = 0 });
	///        Data.Add(new Model() { XValue = 12, YValue = 0 });
	///        Data.Add(new Model() { XValue = 17, YValue = 0 });
	///        Data.Add(new Model() { XValue = 22, YValue = 0 });
	///        Data.Add(new Model() { XValue = 25, YValue = 0 });
	///        Data.Add(new Model() { XValue = 28, YValue = 0 });
	///        Data.Add(new Model() { XValue = 31, YValue = 0 });
	///        Data.Add(new Model() { XValue = 37, YValue = 0 });
	///        Data.Add(new Model() { XValue = 44, YValue = 0 });       
	///     }
	/// ]]></code>
	/// ***
	/// </example>
	public class HistogramSeries : XYDataSeries, IDrawCustomLegendIcon
    {
        #region Fields

        const int _distributionPointsCount = 100;
        static readonly double _sqrtDoublePI = Math.Sqrt(2 * Math.PI);
        List<double> _distributionYValues;
        List<double> _distributionXValues;
        DistributionSegment _distributionSegment;
#if __ANDROID__
         readonly float _displayScale;
#endif

        #endregion

        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="ShowNormalDistributionCurve"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="ShowNormalDistributionCurve"/> property indicates whether to show the normal distribution curve.
        /// </remarks>
        public static readonly BindableProperty ShowNormalDistributionCurveProperty = BindableProperty.Create(
            nameof(ShowNormalDistributionCurve),
            typeof(bool),
            typeof(HistogramSeries),
            true,
            BindingMode.Default,
            null,
            OnShowNormalDistributionCurvePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="HistogramInterval"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="HistogramInterval"/> property indicates interval of histogram series.
        /// </remarks>
        public static readonly BindableProperty HistogramIntervalProperty = BindableProperty.Create(
            nameof(HistogramInterval),
            typeof(double),
            typeof(HistogramSeries),
            1.0,
            BindingMode.Default,
            null,
            OnHistogramIntervalPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="CurveStyle"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="CurveStyle"/> property helps to customize the appearance of the curve in the histogram series.
        /// </remarks>
        public static readonly BindableProperty CurveStyleProperty = BindableProperty.Create(
            nameof(CurveStyle),
            typeof(ChartLineStyle),
            typeof(HistogramSeries),
            null,
            BindingMode.Default,
            null,
            OnCurveStylePropertyChanged,
            defaultValueCreator: OnCurveStyleDefaultValueCreator);

        /// <summary>
        /// Identifies the <see cref="Stroke"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="Stroke"/> property indicates stroke brush used for the series.
        /// </remarks>
        public static readonly BindableProperty StrokeProperty = BindableProperty.Create(
            nameof(Stroke),
            typeof(Brush),
            typeof(HistogramSeries),
            SolidColorBrush.Transparent,
            BindingMode.Default,
            null,
            OnStrokeColorChanged);

        #endregion

        #region Public  Properties

        /// <summary>
        /// Gets or sets a value indicating whether to show the normal distribution curve.
        /// </summary>
        /// <value>It accepts <see cref="bool"/> values and its default value is true.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-4)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:HistogramSeries ItemsSource="{Binding Data}"
        ///                                 XBindingPath="XValue"
        ///                                 YBindingPath="YValue"
        ///                                 ShowNormalDistributionCurve="False" />
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
        ///     HistogramSeries series = new HistogramSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           ShowNormalDistributionCurve = false,
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public bool ShowNormalDistributionCurve
        {
            get { return (bool)GetValue(ShowNormalDistributionCurveProperty); }
            set { SetValue(ShowNormalDistributionCurveProperty, value); }
        }

        /// <summary>
        /// Gets or sets the interval for the histogram bars, determining the range of values that each bar represents.
        /// </summary>
        /// <value>It accepts <see cref="double"/> values and its default value is 1.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-6)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:HistogramSeries ItemsSource="{Binding Data}"
        ///                                 XBindingPath="XValue"
        ///                                 YBindingPath="YValue"
        ///                                 HistogramInterval="10" />
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
        ///     HistogramSeries series = new HistogramSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           HistogramInterval = 10,
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public double HistogramInterval
        {
            get { return (double)GetValue(HistogramIntervalProperty); }
            set { SetValue(HistogramIntervalProperty, value); }
        }

        /// <summary>
        /// Gets or sets the style for the curve in the histogram, which defines the appearance of the line connecting the bars or points
        /// </summary>
        /// <value>It accepts <see cref="ChartLineStyle"/> values and its default value is null.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-8)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:HistogramSeries ItemsSource="{Binding Data}"
        ///                                 XBindingPath="XValue"
        ///                                 YBindingPath="YValue"
        ///                                 Stroke="Red">
        ///             <chart:HistogramSeries.CurveStyle>
        ///               <chart:ChartLineStyle Stroke ="Red" StrokeWidth ="2"/>
        ///              <chart:HistogramSeries.CurveStyle>
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-9)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///    
        ///     HistogramSeries series = new HistogramSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           Stroke = new SolidColorBrush(Colors.Red),
        ///           CurveStyle = new ChartLineStyle()
        ///           {
        ///              Stroke = Colors.Red,
        ///              StrokeWidth = 2;
        ///           }
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public ChartLineStyle CurveStyle
        {
            get { return (ChartLineStyle)GetValue(CurveStyleProperty); }
            set { SetValue(CurveStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the stroke of the histogram segments, which defines the color of the borders around each segment.
        /// </summary>
        /// <value>It accepts <see cref="Brush"/> values and its default value is Transparent.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-10)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:HistogramSeries ItemsSource="{Binding Data}"
        ///                                 XBindingPath="XValue"
        ///                                 YBindingPath="YValue"
        ///                                 Stroke = "Red" />
        ///
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-11)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     HistogramSeries series = new HistogramSeries()
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

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="HistogramSeries"/> class.
        /// </summary>
        public HistogramSeries()
        {
#if __ANDROID__
#nullable disable
           _displayScale = Android.Content.Res.Resources.System.DisplayMetrics.Density;
#nullable enable
#endif
            _distributionSegment = new DistributionSegment(this);
            _distributionXValues = new List<double>();
            _distributionYValues = new List<double>();
        }

        #endregion

        #region Interface Implementation 

        void IDrawCustomLegendIcon.DrawSeriesLegend(ICanvas canvas, RectF rect, Brush fillColor, bool isSaveState)
        {
            if (isSaveState)
                canvas.CanvasSaveState();

            DrawFirstPath(canvas);
            DrawSecondPath(canvas);

            if (isSaveState)
                canvas.CanvasRestoreState();
        }

        #endregion

        #region Methods

        #region Protected Internal Methods

        /// <inheritdoc/>
        protected override ChartSegment? CreateSegment()
        {
            return new HistogramSegment();
        }

        /// <inheritdoc/>
        /// <exclude/>
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (CurveStyle != null)
            {
                SetInheritedBindingContext(CurveStyle, BindingContext);
            }
        }

        /// <inheritdoc/>
        protected internal override void DrawSeries(ICanvas canvas, ReadOnlyObservableCollection<ChartSegment> segments, RectF clipRect)
        {
            base.DrawSeries(canvas, segments, clipRect);
            // Check that HistogramInterval is not zero.
            if (HistogramInterval != 0 && ShowNormalDistributionCurve)
            {
                _distributionSegment.DrawPoints = GetDistributionCurvePoints();
                _distributionSegment.OnDraw(canvas);
            }
        }

        #endregion

        #region Internal Methods

        internal override void GenerateSegments(SeriesView seriesView)
        {
            var xValues = GetXValues();
            int index = 0;
            double bottomValue = 0;
            var xAxis = ActualXAxis;

            if (xAxis != null)
                bottomValue = double.IsNaN(xAxis.ActualCrossingValue) ? 0 : xAxis.ActualCrossingValue;

            if (xValues != null)
            {
                xValues = xValues.Where(value => !double.IsNaN(value)).ToList();
                xValues.Sort();
                Segments.Clear();
                List<Point> dataPoints = GetDataPoints(xValues);
                // Take the absolute value of HistogramInterval to ensure a positive interval value.
                double interval = Math.Abs(HistogramInterval);
                if (interval == 0)
                    return;

                double start = 0;

                if (dataPoints.Count > 0)
                    start = ChartMath.Round(dataPoints[0].X, interval, false);

                int pointsCount;
                double position = start;
                int intervalsCount = 0;
                List<object> data = new();
                List<Point> points = new();
                HistogramSegment? histogramSegment;

                for (int i = 0, currentIndex = dataPoints.Count; i < currentIndex; i++)
                {
                    var currentPoint = dataPoints[i];
                    while (currentPoint.X > position + interval)
                    {
                        pointsCount = points.Count;
                        if (pointsCount > 0)
                        {
                            histogramSegment = GetHistogramSegment(position, interval, pointsCount, bottomValue, i, index, data);
                            data = new();
                            points.Clear();
                            index++;
                        }

                        position += interval;
                        intervalsCount++;
                    }

                    points.Add(dataPoints[i]);
                    if (ActualData?[i] != null)
                        data.Add(ActualData[i]);
                }

                pointsCount = points.Count;
                if (pointsCount > 0)
                {
                    intervalsCount++;
                    histogramSegment = GetHistogramSegment(position, interval, pointsCount, bottomValue, 0, index, data);
                    points.Clear();
                }

                if (ShowNormalDistributionCurve)
                {
                    CalculateNormalDistributionCurve(dataPoints, start, intervalsCount, interval);
                }
            }
        }

        internal override void UpdateRange()
        {
            if (ShowNormalDistributionCurve && _distributionXValues != null)
            {
                double xMin = double.MaxValue, xMax = double.MinValue, yMin = double.MaxValue, yMax = double.MinValue;
                int dataCount = _distributionXValues.Count;

                for (int i = 0; i < dataCount; i++)
                {
                    var xValue = _distributionXValues[i];
                    var yValue = _distributionYValues[i];

                    if (xValue > xMax)
                    {
                        xMax = xValue;
                    }

                    if (xValue < xMin)
                    {
                        xMin = xValue;
                    }

                    if (yValue > yMax)
                    {
                        yMax = yValue;
                    }

                    if (yValue < yMin)
                    {
                        yMin = yValue;
                    }
                }

                XRange += new DoubleRange(xMin, xMax);

                if (PointsCount > 1 && !(yMin == double.MaxValue))
                {
                    YRange += new DoubleRange(yMin, yMax);
                }
            }

            base.UpdateRange();
        }

        internal override TooltipInfo? GetTooltipInfo(ChartTooltipBehavior tooltipBehavior, float x, float y)
        {
            int index = IsSideBySide ? GetDataPointIndex(x, y) : SeriesContainsPoint(new PointF(x, y)) ? TooltipDataPointIndex : -1;

            if (ChartArea == null || ItemsSource == null || ActualData == null || ActualXAxis == null
                || ActualYAxis == null || SeriesYValues == null)
                return null;

            TooltipInfo? tooltipInfo = base.GetTooltipInfo(tooltipBehavior, x, y);
            if (tooltipInfo != null)
            {
                if (Segments[index] is HistogramSegment histogramSegment)
                {
                    tooltipInfo.X = TransformToVisibleX(histogramSegment.HistogramLabelPosition, histogramSegment.PointsCount);

                    if (ChartArea.IsTransposed)
                    {
                        tooltipInfo.X /= 2;

                        if (ActualYAxis.IsInversed)
                        {
                            tooltipInfo.X = TransformToVisibleX(histogramSegment.HistogramLabelPosition, histogramSegment.PointsCount / 2);
                        }
                    }

                    tooltipInfo.Y = ActualYAxis.IsInversed && !ChartArea.IsTransposed
                        ? TransformToVisibleY(histogramSegment.HistogramLabelPosition, histogramSegment.PointsCount)
                        : histogramSegment.Top;

                    tooltipInfo.Item = histogramSegment.Item;
                    tooltipInfo.Text = histogramSegment.PointsCount.ToString();
                }
            }

            return tooltipInfo;
        }

        internal override void SetStrokeColor(ChartSegment segment)
        {
            segment.Stroke = Stroke;
        }

        internal override double GetDataLabelPositionAtIndex(int index)
        {
            if (DataLabelSettings == null) return 0;

            var segment = Segments?[index] as HistogramSegment;
            double yValue = 0;

            if (segment != null)
            {
                yValue = segment.PointsCount;
                switch (DataLabelSettings.BarAlignment)
                {
                    case DataLabelAlignment.Bottom:
                        yValue = 0;
                        break;
                    case DataLabelAlignment.Middle:
                        yValue = yValue / 2;
                        break;
                }
            }

            return yValue;
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

        internal override void GenerateTrackballPointInfo(List<object> nearestDataPoints, List<TrackballPointInfo> pointInfos, ref bool isSideBySide)
        {
            //Disabled the trackball for histogram.
        }

        #endregion

        #region Private Methods

        static void OnShowNormalDistributionCurvePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is HistogramSeries series)
            {
                series.SegmentsCreated = false;
                series.ScheduleUpdateChart();
            }
        }

        static void OnHistogramIntervalPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is HistogramSeries series)
            {
                series.SegmentsCreated = false;
                series.ScheduleUpdateChart();
            }
        }

        static void OnCurveStylePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is HistogramSeries series)
                series.OnStylePropertyChanged(oldValue as ChartLineStyle, newValue as ChartLineStyle);
        }

        void OnStylePropertyChanged(ChartLineStyle? oldValue, ChartLineStyle? newValue)
        {
            if (oldValue != null)
            {
                oldValue.PropertyChanged -= CurveLineStyles_PropertyChanged;
                SetInheritedBindingContext(oldValue, null);
            }


            if (newValue != null)
            {
                newValue.PropertyChanged += CurveLineStyles_PropertyChanged;
                SetInheritedBindingContext(newValue, BindingContext);
                _distributionSegment.UpdateCurveStyle();
            }

            if (AreaBounds != Rect.Zero)
                InvalidateSeries();
        }

        void CurveLineStyles_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            _distributionSegment.UpdateCurveStyle();
            InvalidateSeries();
        }

        static void OnStrokeColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is HistogramSeries series)
            {
                if (series != null && series.Chart != null)
                {
                    series.UpdateStrokeColor();
                    series.InvalidateSeries();
                }
            }
        }

        List<Point> GetDataPoints(List<double> xValues)
        {
            List<Point> dataPoints = new();

            for (int i = 0; i < xValues.Count; i++)
            {
                dataPoints.Add(new Point(xValues[i], YValues[i]));
            }

            return dataPoints;
        }

        float[] GetDistributionCurvePoints()
        {
            int dataCount = _distributionXValues != null ? _distributionXValues.Count : 0;
            float[] linePoints = new float[dataCount * 4];
            double preXValue;
            double preYValue;
            int arrayCount = 0;
            float preXPos = 0, preYPos = 0;

            if (_distributionXValues != null)
            {
                if (dataCount > 0)
                {
                    preXValue = _distributionXValues.Count > 0 ? _distributionXValues[0] : 0;
                    preYValue = _distributionYValues.Count > 0 ? _distributionYValues[0] : 0;
                    preXPos = TransformToVisibleX(preXValue, preYValue);
                    preYPos = TransformToVisibleY(preXValue, preYValue);
                }

                for (int i = 1; i < dataCount; i++)
                {
                    double xValue = _distributionXValues[i];
                    double yValue = _distributionYValues[i];
                    float x = TransformToVisibleX(xValue, yValue);
                    float y = TransformToVisibleY(xValue, yValue);
                    UpdateLinePoints(linePoints, preXPos, preYPos, x, y, ref arrayCount);
                    preXPos = x;
                    preYPos = y;
                }
            }

            float[] points = new float[arrayCount];
            Array.Copy(linePoints, 0, points, 0, arrayCount);
            return points;
        }

        void UpdateLinePoints(float[] linePoints, float preXPos, float preYPos, float x, float y, ref int arrayCount)
        {
#if __ANDROID__
                linePoints[arrayCount++] = preXPos * _displayScale;
                linePoints[arrayCount++] = preYPos * _displayScale;
                linePoints[arrayCount++] = x * _displayScale;
                linePoints[arrayCount++] = y * _displayScale;
#else
            linePoints[arrayCount++] = preXPos;
            linePoints[arrayCount++] = preYPos;
            linePoints[arrayCount++] = x;
            linePoints[arrayCount++] = y;
#endif
        }

        void CalculateNormalDistributionCurve(List<Point> dataPoints, double start, int intervalsCount, double interval)
        {
            double mean, deviation;
            GetHistogramMeanAndDeviation(dataPoints, out mean, out deviation);
            double min = start;
            double max = start + (intervalsCount * interval);
            double del = (max - min) / (_distributionPointsCount - 1);
            _distributionXValues = new List<double>();
            _distributionYValues = new List<double>();

            for (int i = 0; i < _distributionPointsCount; i++)
            {
                double tx = min + (i * del);
                double ty = NormalDistribution(tx, mean, deviation) * dataPoints.Count * interval;
                _distributionXValues.Add(tx);
                _distributionYValues.Add(ty);
            }
        }

        static double NormalDistribution(double x, double m, double sigma)
        {
            return Math.Exp(-(x - m) * (x - m) / (2 * sigma * sigma)) / (sigma * _sqrtDoublePI);
        }

        static void GetHistogramMeanAndDeviation(List<Point> points, out double mean, out double standardDeviation)
        {
            int count = points.Count;
            double sum = 0;

            for (int i = 0; i < count; i++)
            {
                sum += points[i].X;
            }

            mean = sum / count;
            sum = 0;

            for (int i = 0; i < count; i++)
            {
                double dif = points[i].X - mean;
                sum += dif * dif;
            }

            standardDeviation = Math.Sqrt(sum / count);
        }

        HistogramSegment? GetHistogramSegment(double position, double interval, int pointsCount, double bottomValue, int i, int index, List<object> data)
        {
            var histogramSegment = CreateSegment() as HistogramSegment;
            if (histogramSegment != null)
            {
                histogramSegment.Series = this;
                histogramSegment.SetData(new double[] { position, position + interval, pointsCount, bottomValue, position, i });
                histogramSegment.PointsCount = pointsCount;
                histogramSegment.Index = index;
                histogramSegment.Item = data;
                histogramSegment.HistogramLabelPosition = (position + (position + interval)) / 2;
                InitiateDataLabels(histogramSegment);
                Segments.Add(histogramSegment);
            }

            return histogramSegment;
        }

        static void DrawFirstPath(ICanvas canvas)
        {
            PathF pathF = new PathF();
            pathF.MoveTo(1, 12);
            pathF.LineTo(1, 8);
            pathF.LineTo(3, 6);
            pathF.LineTo(3, 12);
            pathF.LineTo(1, 12);
            pathF.Close();
            pathF.MoveTo(5, 12);
            pathF.LineTo(5, 5);
            pathF.CurveTo(5, 5, 6, (float)5.5, 7, 5);
            pathF.LineTo(7, 12);
            pathF.LineTo(5, 12);
            pathF.Close();
            pathF.MoveTo(9, 12);
            pathF.LineTo(9, 10);
            pathF.CurveTo(9, 10, (float)9.5, (float)10.5, 10, 10);
            pathF.CurveTo(10, 10, (float)10.5, (float)9.5, 11, 11);
            pathF.LineTo(11, 12);
            pathF.LineTo(9, 12);
            pathF.Close();
            pathF.MoveTo(1, 4);
            pathF.LineTo(1, 0);
            pathF.LineTo(3, 0);
            pathF.LineTo(3, 3);
            pathF.LineTo(1, 4);
            pathF.Close();
            pathF.MoveTo(5, 0);
            pathF.LineTo(7, 0);
            pathF.LineTo(7, 2);
            pathF.CurveTo(7, 2, 6, (float)1.5, 5, 2);
            pathF.LineTo(5, 0);
            pathF.Close();
            pathF.MoveTo(9, 4);
            pathF.LineTo(11, 4);
            pathF.LineTo(11, 8);
            pathF.CurveTo(11, 8, (float)10.5, 8, 10, (float)7.5);
            pathF.CurveTo(10, (float)7.5, 9, (float)6.5, 9, 6);
            pathF.LineTo(9, 6);
            pathF.Close();
            canvas.FillPath(pathF);
        }

        static void DrawSecondPath(ICanvas canvas)
        {
            PathF pathF1 = new PathF();
            pathF1.MoveTo(0, 6);
            pathF1.CurveTo(0, 6, 6, (float)2.5, 7, (float)2.3);
            pathF1.LineTo(7, (float)3.3);
            pathF1.CurveTo(8, (float)3.3, 6, (float)3.5, 0, 8);
            pathF1.LineTo(0, 6);
            pathF1.Close();
            pathF1.MoveTo(7, (float)2.3);
            pathF1.CurveTo(7, (float)2.3, 8, (float)2.5, (float)8.5, 4);
            pathF1.LineTo((float)7.5, 5);
            pathF1.CurveTo((float)7.5, 5, (float)7.4, 4, 7, (float)3.3);
            pathF1.Close();
            pathF1.MoveTo((float)8.5, 4);
            pathF1.LineTo((float)8.5, 7);
            pathF1.LineTo((float)7.5, 7);
            pathF1.LineTo((float)7.5, 4);
            pathF1.MoveTo((float)8.5, 4);
            pathF1.Close();
            pathF1.MoveTo((float)8.5, 7);
            pathF1.CurveTo((float)8.5, 7, 9, 8, 10, (float)8.5);
            pathF1.LineTo(10, (float)9.5);
            pathF1.CurveTo(10, (float)9.5, 8, (float)8.5, (float)7.5, 7);
            pathF1.Close();
            pathF1.MoveTo(10, (float)8.5);
            pathF1.LineTo(11, (float)8.5);
            pathF1.LineTo(11, (float)9.5);
            pathF1.LineTo(10, (float)9.5);
            pathF1.LineTo(10, (float)8.5);
            pathF1.Close();
            pathF1.MoveTo(11, (float)8.5);
            pathF1.CurveTo(11, (float)8.5, (float)11.5, (float)8.5, 12, 9);
            pathF1.LineTo(12, 10);
            pathF1.CurveTo(12, 10, (float)11.5, (float)9.5, 11, (float)9.5);
            pathF1.Close();
            canvas.FillPath(pathF1);
        }

        static object OnCurveStyleDefaultValueCreator(BindableObject bindable)
        {
            return new ChartLineStyle
            {
                Stroke = new SolidColorBrush(Color.FromArgb("#FF1717"))
            };
        }

        #endregion

        #endregion
    }
}