using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// The <see cref="RadialBarSeries"/> displays data in different categories. Its typically used to make comparisons among a set of given data using the circular shapes of the bars.
	/// </summary>
	/// <remarks>
	/// <para>It is similar to the DoughnutSeries. To render a series, create an instance of the <see cref="RadialBarSeries"/> class, and add it to the <see cref="SfCircularChart.Series"/> collection.</para>
	/// <para>It provides options for <see cref="TrackFill"/>, <see cref="TrackStroke"/>, <see cref="TrackStrokeWidth"/>, <see cref="MaximumValue"/>, <see cref="GapRatio"/>, <see cref="CapStyle"/>, <see cref="CenterView"/>, <see cref="ChartSeries.PaletteBrushes"/>, <see cref="ChartSeries.Fill"/>, <see cref="CircularSeries.Stroke"/>, <see cref="CircularSeries.StrokeWidth"/>, and <see cref="InnerRadius"/> to customize the appearance.</para>
	/// </remarks>
	/// <example>
	/// # [Xaml](#tab/tabid-1)
	/// <code><![CDATA[
	///     <chart:SfCircularChart>
	///
	///           <chart:SfCircularChart.Series>
	///               <chart:RadialBarSeries
	///                   ItemsSource="{Binding Data}"
	///                   XBindingPath="XValue"
	///                   YBindingPath="YValue"
	///                   TrackFill="Red"
	///                   TrackStroke="Green"
	///                   TrackStrokeWidth="1"/>
	///           </chart:SfCircularChart.Series>
	///           
	///     </chart:SfCircularChart>
	/// ]]></code>
	/// # [C#](#tab/tabid-2)
	/// <code><![CDATA[
	///     SfCircularChart chart = new SfCircularChart();
	///     
	///     ViewModel viewModel = new ViewModel();
	/// 
	///     RadialBarSeries series = new RadialBarSeries();
	///     series.ItemsSource = viewModel.Data;
	///     series.XBindingPath = "XValue";
	///     series.YBindingPath = "YValue";
	///     series.TrackFill = Brush.Red;
	///     series.TrackStroke = Brush.Green;
	///     series.TrackStrokeWidth = 1;
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
	public class RadialBarSeries : CircularSeries, IDrawCustomLegendIcon
    {
        #region Internal Fields

        internal int visibleSegmentCount = 0;

        #endregion

        #region Fields

        float _yValue;
        double _total = 0;
        float _startAngle;
        float _endAngle;
        double _angleDifference;
        double _centerHoleSize = 1;

        #endregion

        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="TrackFill"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="TrackFill"/> property specifies fill color of the track in a radial bar chart.
        /// </remarks>
        public static readonly BindableProperty TrackFillProperty = BindableProperty.Create(
            nameof(TrackFill),
            typeof(Brush),
            typeof(RadialBarSeries),
            null,
            BindingMode.Default,
            null,
            OnTrackFillChanged,
            defaultValueCreator: TrackFillValueCreator);

        /// <summary>
        /// Identifies the <see cref="TrackStroke"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="TrackStroke"/> property defines the stroke color of the track in a radial bar chart.
        /// </remarks>
        public static readonly BindableProperty TrackStrokeProperty = BindableProperty.Create(
            nameof(TrackStroke),
            typeof(Brush),
            typeof(RadialBarSeries),
            null,
            BindingMode.Default,
            null,
            OnTrackStrokeChanged,
            defaultValueCreator: TrackStrokeValueCreator);

        /// <summary>
        /// Identifies the <see cref="TrackStrokeWidth"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="TrackStrokeWidth"/> property specifies the stroke thickness of the track in a radial bar chart.
        /// </remarks>
        public static readonly BindableProperty TrackStrokeWidthProperty = BindableProperty.Create(
            nameof(TrackStrokeWidth),
            typeof(double),
            typeof(RadialBarSeries),
            0d,
            BindingMode.Default,
            null,
            OnTrackStrokeWidthChanged);

        /// <summary>
        /// Identifies the <see cref="MaximumValue"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="MaximumValue"/> property represents maximum value for the radial bar segments.
        /// </remarks>
        public static readonly BindableProperty MaximumValueProperty = BindableProperty.Create(
            nameof(MaximumValue),
            typeof(double),
            typeof(RadialBarSeries),
            double.NaN,
            BindingMode.Default,
            null,
            OnRadialBarSeriesPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="GapRatio"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="GapRatio"/> property indicates the distance between two individual segments.
        /// </remarks>
        public static readonly BindableProperty GapRatioProperty = BindableProperty.Create(
            nameof(GapRatio),
            typeof(double),
            typeof(RadialBarSeries),
            0.2d,
            BindingMode.Default,
            null,
            OnSpacingPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="CenterView"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="CenterView"/> property indicates the view that added to the center of the radial bar.
        /// </remarks>
        public static readonly BindableProperty CenterViewProperty = BindableProperty.Create(
            nameof(CenterView),
            typeof(View),
            typeof(RadialBarSeries),
            null,
            BindingMode.Default,
            null,
            OnCenterViewPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="CapStyle"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="CapStyle"/> property represents the shape of the start and end points of a radial segment.
        /// </remarks>
        public static readonly BindableProperty CapStyleProperty = BindableProperty.Create(
            nameof(CapStyle),
            typeof(CapStyle),
            typeof(RadialBarSeries),
            CapStyle.BothFlat,
            BindingMode.Default,
            null,
            OnRadialBarSeriesPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="InnerRadius"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="InnerRadius"/> property determines the size of the inner circle.
        /// </remarks>
        public static readonly BindableProperty InnerRadiusProperty = BindableProperty.Create(
            nameof(InnerRadius),
            typeof(double),
            typeof(RadialBarSeries),
            0.4d,
            BindingMode.Default,
            null,
            OnInnerRadiusPropertyChanged,
            null,
            coerceValue: CoerceRadialBarCoefficient);

        #endregion

        #region Puplic properties

        /// <summary>
        /// Gets or sets the brush value that represents the fill color of the track in a radial bar chart.
        /// </summary>
        /// <value>It accepts <see cref="Brush"/> values.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-4)
        /// <code><![CDATA[
        ///     <chart:SfCircularChart>
        ///     
        ///         <chart:SfCircularChart.BindingContext>
        ///             <local:ViewModel/>
        ///         </chart:SfCircularChart.BindingContext>
        ///         
        ///         <chart:RadialBarSeries ItemsSource="{Binding Data}"
        ///                                XBindingPath="XValue"
        ///                                YBindingPath="YValue"
        ///                                TrackFill="Red"/>                       
        ///     </chart:SfCircularChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-5)
        /// <code><![CDATA[
        ///     SfCircularChart chart = new SfCircularChart();
        ///     
        ///     ViewModel viewModel = new ViewModel();
        ///     chart.BindingContext = viewModel;
        ///     
        ///     RadialBarSeries series = new RadialBarSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           TrackFill = Brush.Red
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public Brush TrackFill
        {
            get { return (Brush)GetValue(TrackFillProperty); }
            set { SetValue(TrackFillProperty, value); }
        }

        /// <summary>
        /// Gets or sets the brush value that represents the stroke color of the track in a radial bar chart.
        /// </summary>
        /// <value>It accepts <see cref="Brush"/> values.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-6)
        /// <code><![CDATA[
        ///     <chart:SfCircularChart>
        ///     
        ///         <chart:SfCircularChart.BindingContext>
        ///             <local:ViewModel/>
        ///         </chart:SfCircularChart.BindingContext>
        ///         
        ///         <chart:RadialBarSeries ItemsSource="{Binding Data}"
        ///                                XBindingPath="XValue"
        ///                                YBindingPath="YValue"
        ///                                TrackStroke="Red"
        ///                                TrackStrokeWidth="1"/>                       
        ///     </chart:SfCircularChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-7)
        /// <code><![CDATA[
        ///     SfCircularChart chart = new SfCircularChart();
        ///     
        ///     ViewModel viewModel = new ViewModel();
        ///     chart.BindingContext = viewModel;
        ///     
        ///     RadialBarSeries series = new RadialBarSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           TrackStroke = Brush.Red,
        ///           TrackStrokeWidth = 1
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public Brush TrackStroke
        {
            get { return (Brush)GetValue(TrackStrokeProperty); }
            set { SetValue(TrackStrokeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value that represents the stroke thickness of the track in a radial bar chart. 
        /// </summary>
        /// <value>It accepts <see cref="double"/> values and its default value is 1.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-8)
        /// <code><![CDATA[
        ///     <chart:SfCircularChart>
        ///     
        ///         <chart:SfCircularChart.BindingContext>
        ///             <local:ViewModel/>
        ///         </chart:SfCircularChart.BindingContext>
        ///         
        ///         <chart:RadialBarSeries ItemsSource="{Binding Data}"
        ///                                XBindingPath="XValue"
        ///                                YBindingPath="YValue"
        ///                                TrackStroke="Red"
        ///                                TrackStrokeWidth="2"/>                       
        ///     </chart:SfCircularChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-9)
        /// <code><![CDATA[
        ///     SfCircularChart chart = new SfCircularChart();
        ///     
        ///     ViewModel viewModel = new ViewModel();
        ///     chart.BindingContext = viewModel;
        ///     
        ///     RadialBarSeries series = new RadialBarSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           TrackStroke = Brush.Red,
        ///           TrackStrokeWidth = 2
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public double TrackStrokeWidth
        {
            get { return (double)GetValue(TrackStrokeWidthProperty); }
            set { SetValue(TrackStrokeWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the maximum value for the radial bar segments.
        /// </summary>
        /// <value>It accepts <see cref="double"/> values and its default value is <see cref="double.NaN"/></value>.
        /// <example>
        /// # [Xaml](#tab/tabid-10)
        /// <code><![CDATA[
        ///     <chart:SfCircularChart>
        ///     
        ///         <chart:SfCircularChart.BindingContext>
        ///             <local:ViewModel/>
        ///         </chart:SfCircularChart.BindingContext>
        ///         
        ///         <chart:RadialBarSeries ItemsSource="{Binding Data}"
        ///                                XBindingPath="XValue"
        ///                                YBindingPath="YValue"
        ///                                MaximumValue="100"/>                       
        ///     </chart:SfCircularChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-11)
        /// <code><![CDATA[
        ///     SfCircularChart chart = new SfCircularChart();
        ///     
        ///     ViewModel viewModel = new ViewModel();
        ///     chart.BindingContext = viewModel;
        ///     
        ///     RadialBarSeries series = new RadialBarSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           MaximumValue = 100;
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public double MaximumValue
        {
            get { return (double)GetValue(MaximumValueProperty); }
            set { SetValue(MaximumValueProperty, value); }
        }

        /// <summary>
        /// Gets or sets the gap ratio, which indicates the distance between two individual segments.
        /// </summary>
        /// <value>It accepts <see cref="double"/> values, and its default value is <c>0.2</c>.</value>
        /// <remarks>The value ranges between 0 and 1.</remarks>
        /// <example>
        /// # [Xaml](#tab/tabid-12)
        /// <code><![CDATA[
        ///     <chart:SfCircularChart>
        ///     
        ///         <chart:SfCircularChart.BindingContext>
        ///             <local:ViewModel/>
        ///         </chart:SfCircularChart.BindingContext>
        ///         
        ///         <chart:RadialBarSeries ItemsSource="{Binding Data}"
        ///                                XBindingPath="XValue"
        ///                                YBindingPath="YValue"
        ///                                GapRatio = "0.3" />                       
        ///     </chart:SfCircularChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-13)
        /// <code><![CDATA[
        ///     SfCircularChart chart = new SfCircularChart();
        ///     
        ///     ViewModel viewModel = new ViewModel();
        ///     chart.BindingContext = viewModel;
        ///     
        ///     RadialBarSeries series = new RadialBarSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           GapRatio = 0.3
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public double GapRatio
        {
            get { return (double)GetValue(GapRatioProperty); }
            set { SetValue(GapRatioProperty, value); }
        }

        /// <summary>
        /// Gets or sets the view that added to the center of the radial bar.
        /// </summary>
        /// <value>It accepts any <see cref="View"/> object, and its default value is null.</value> 
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-14)
        /// <code><![CDATA[
        /// <chart:SfCircularChart>
        /// 
        ///         <chart:SfCircularChart.BindingContext>
        ///             <local:ViewModel/>
        ///         </chart:SfCircularChart.BindingContext>  
        /// 
        ///         <chart:SfCircularChart.Series>
        ///             <chart:RadialBarSeries ItemsSource="{Binding Data}" 
        ///                                    XBindingPath="XValue" 
        ///                                    YBindingPath="YValue"/>
        ///                  <chart:RadialBarSeries.CenterView>
        ///                         <Label Text="CenterView"/>
        ///                 </chart:RadialBarSeries.CenterView>
        ///         </chart:SfCircularChart.Series>
        /// 
        /// </chart:SfCircularChart>
        /// ]]>
        /// </code>
        /// 
        /// # [C#](#tab/tabid-15)
        /// <code><![CDATA[
        ///  SfCircularChart chart = new SfCircularChart();
        ///  
        ///  ViewModel viewModel = new ViewModel();
        ///	 chart.BindingContext = viewModel;
        ///  
        ///  RadialBarSeries series = new RadialBarSeries()
        ///  {
        ///     ItemsSource = viewModel.Data,
        ///     XBindingPath = "XValue",
        ///     YBindingPath = "YValue",
        ///  };
        ///  
        ///  Label label = new Label();
        ///  label.Text = "CenterView"
        ///  series.CenterView = label;
        ///  chart.Series.Add(series);
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public View CenterView
        {
            get { return (View)GetValue(CenterViewProperty); }
            set { SetValue(CenterViewProperty, value); }
        }

        /// <summary>
        /// Gets or sets the <see cref="CapStyle"/> value, that represents the shape of the start and end points of a radial segment.
        /// </summary>
        /// <value>It accepts <see cref="CapStyle"/> values, and its default is <see cref="CapStyle.BothFlat"/></value>
        /// <example>
        /// # [Xaml](#tab/tabid-16)
        /// <code><![CDATA[
        ///     <chart:SfCircularChart>
        ///     
        ///         <chart:SfCircularChart.BindingContext>
        ///             <local:ViewModel/>
        ///         </chart:SfCircularChart.BindingContext>
        ///         
        ///         <chart:RadialBarSeries ItemsSource="{Binding Data}"
        ///                                XBindingPath="XValue"
        ///                                YBindingPath="YValue"
        ///                                CapStyle = "BothCurve"  />                       
        ///     </chart:SfCircularChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-17)
        /// <code><![CDATA[
        ///     SfCircularChart chart = new SfCircularChart();
        ///     
        ///     ViewModel viewModel = new ViewModel();
        ///     chart.BindingContext = viewModel;
        ///     
        ///     RadialBarSeries series = new RadialBarSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           CapStyle = CapStyle.BothCurve
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public CapStyle CapStyle
        {
            get { return (CapStyle)GetValue(CapStyleProperty); }
            set { SetValue(CapStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that determines the size of the inner circle.
        /// </summary>
        /// <value>It accepts <see cref="double"/> values, and the default value is 0.4.</value>
        /// <remarks>The value ranges between 0 and 1.</remarks>
        /// <example>
        /// # [Xaml](#tab/tabid-18)
        /// <code><![CDATA[
        ///     <chart:SfCircularChart>
        ///     
        ///         <chart:SfCircularChart.BindingContext>
        ///             <local:ViewModel/>
        ///         </chart:SfCircularChart.BindingContext>
        ///         
        ///         <chart:RadialBarSeries ItemsSource="{Binding Data}"
        ///                                XBindingPath="XValue"
        ///                                YBindingPath="YValue"
        ///                                InnerRadius = "0.2" />                       
        ///     </chart:SfCircularChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-19)
        /// <code><![CDATA[
        ///     SfCircularChart chart = new SfCircularChart();
        ///     
        ///     ViewModel viewModel = new ViewModel();
        ///     chart.BindingContext = viewModel;
        ///     
        ///     RadialBarSeries series = new RadialBarSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           InnerRadius = 0.2
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public double InnerRadius
        {
            get { return (double)GetValue(InnerRadiusProperty); }
            set { SetValue(InnerRadiusProperty, value); }
        }

        /// <summary>
        /// Gets the size of the radial bar center hole.
        /// </summary>
        public double CenterHoleSize
        {
            get { return _centerHoleSize; }
            internal set
            {
                if (value >= 1)
                {
                    _centerHoleSize = value;
                }

                OnPropertyChanged(nameof(CenterHoleSize));
            }
        }

        #endregion

        #region Constructor 

        /// <summary>
        /// Initializes a new instance of the <see cref="RadialBarSeries"/> class.
        /// </summary>
        public RadialBarSeries() : base()
        {
            PaletteBrushes = ChartColorModel.DefaultBrushes;
            StartAngle = -90;
            EndAngle = 270;
        }

        #endregion

        #region Interface Implementation

        void IDrawCustomLegendIcon.DrawSeriesLegend(ICanvas canvas, RectF rect, Brush fillColor, bool isSaveState)
        {
            float centerX = rect.Center.X;
            float centerY = rect.Center.Y;
            float num4 = rect.Width / 3;

            if (isSaveState)
            {
                canvas.CanvasSaveState();
            }

            RectF rect1 = new RectF(centerX / 3, centerY / 3, centerX + 2, centerY + 2);
            RectF rect2 = new RectF(num4, num4, num4, num4);

            PathF path = new PathF();
            path.AddArc(rect1.Left, rect1.Top, rect1.Right, rect1.Bottom, 0, (float)359.99, false);
            path.AddArc(rect2.Left, rect2.Top, rect2.Right, rect2.Bottom, (float)359.99, 0, true);
            path.Close();
            canvas.SetFillPaint(fillColor, path.Bounds);
            canvas.FillPath(path);

            if (isSaveState)
            {
                canvas.CanvasRestoreState();
            }
        }

        #endregion

        #region Methods

        #region Protected Methods

        /// <inheritdoc/>
        /// <exclude/>
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (CenterView != null)
            {
                SetInheritedBindingContext(CenterView, BindingContext);
            }
        }

        /// <inheritdoc />
        protected override ChartSegment CreateSegment()
        {
            return new RadialBarSegment();
        }

        #endregion

        #region Internal Methods

        internal override void GenerateSegments(SeriesView seriesView)
        {
            if (YValues != null && ActualData != null)
            {
                _startAngle = (float)StartAngle;
                visibleSegmentCount = 0;
                _angleDifference = GetAngleDifference();
                _total = !double.IsNaN(MaximumValue) ? MaximumValue : CalculateTotalYValues();
                var legendItems = GetLegendItems();

                for (int i = 0; i < YValues.Count; i++)
                {
                    _yValue = (legendItems == null || legendItems.Count == 0) ? (float)Math.Abs(double.IsNaN(YValues[i]) ? double.NaN : YValues[i]) : (float)(legendItems[i].IsToggled ? double.NaN : YValues[i]);
                    _endAngle = (float)(Math.Abs(float.IsNaN(_yValue) ? 0 : _yValue >= _total ? _total : _yValue) * (_angleDifference / _total));

                    if (i < Segments.Count && Segments[i] is RadialBarSegment)
                    {
                        ((RadialBarSegment)Segments[i]).SetData(_startAngle, _endAngle, _yValue);
                    }
                    else
                    {
                        RadialBarSegment radialBarSegment = (RadialBarSegment)CreateSegment();
                        radialBarSegment.Series = this;
                        radialBarSegment.Index = i;
                        radialBarSegment.VisibleSegmentIndex = visibleSegmentCount;
                        radialBarSegment.Item = ActualData[i];
                        radialBarSegment.SetData(_startAngle, _endAngle, _yValue);
                        radialBarSegment.SeriesView = seriesView;
                        Segments.Add(radialBarSegment);
                    }

                    if (!double.IsNaN(YValues[i]) && (legendItems == null || legendItems.Count == 0 || !legendItems[i].IsToggled))
                    {
                        visibleSegmentCount++;
                    }
                }
            }
        }

        internal override void OnDetachedToChart()
        {
            RemoveCenterView(CenterView);
            base.OnDetachedToChart();
        }

        internal override void OnAttachedToChart(IChart? chart)
        {
            base.OnAttachedToChart(chart);
            AddCenterView();
        }

        internal override void OnSeriesLayout()
        {
            base.OnSeriesLayout();
            CenterHoleSize = GetCenterHoleSize();
            UpdateCenterViewBounds(CenterView);
        }

        internal override TooltipInfo? GetTooltipInfo(ChartTooltipBehavior tooltipBehavior, float x, float y)
        {
            if (AreaBounds == Rect.Zero) return null;

            int index = GetDataPointIndex(x, y);

            if (index < 0 || ActualData == null || YValues == null)
            {
                return null;
            }

            object dataPoint = ActualData[index];
            double yValue = YValues[index];
            var radialBarSegment = Segments[index] as RadialBarSegment;

            if (radialBarSegment == null) return null;

            float segmentRadius = GetTooltipRadius(radialBarSegment);
            PointF center = Center;
            double midAngle = (radialBarSegment.StartAngle + (radialBarSegment.EndAngle / 2)) * 0.0174532925f;
            float xPosition = (float)(center.X + (Math.Cos(midAngle) * segmentRadius));
            float yPosition = (float)(center.Y + (Math.Sin(midAngle) * segmentRadius));

            TooltipInfo tooltipInfo = new TooltipInfo(this);
            tooltipInfo.X = xPosition;
            tooltipInfo.Y = yPosition;
            tooltipInfo.Index = index;
            tooltipInfo.Margin = tooltipBehavior.Margin;
            tooltipInfo.TextColor = tooltipBehavior.TextColor;
            tooltipInfo.FontFamily = tooltipBehavior.FontFamily;
            tooltipInfo.FontSize = tooltipBehavior.FontSize;
            tooltipInfo.FontAttributes = tooltipBehavior.FontAttributes;
            tooltipInfo.Background = tooltipBehavior.Background;
            tooltipInfo.Text = yValue.ToString("#.##");
            tooltipInfo.Item = dataPoint;
            return tooltipInfo;
        }

        #endregion

        #region Private Methods

        static void OnTrackFillChanged(BindableObject bindable, Object oldValue, Object newValue)
        {
            if (bindable is RadialBarSeries radialBarSeries)
            {
                radialBarSeries.UpdateTrackProperties(newValue);
                radialBarSeries.ScheduleUpdateChart();
            }
        }

        static void OnTrackStrokeChanged(BindableObject bindable, Object oldValue, Object newValue)
        {
            if (bindable is RadialBarSeries series)
            {
                series.UpdateTrackProperties(newValue);
                series.InvalidateSeries();
            }
        }

        static void OnTrackStrokeWidthChanged(BindableObject bindable, Object oldValue, Object newValue)
        {
            if (bindable is RadialBarSeries series)
            {
                series.UpdateTrackProperties(newValue);
                series.InvalidateSeries();
            }
        }

        static void OnRadialBarSeriesPropertyChanged(BindableObject bindable, Object oldValue, Object newValue)
        {
            if (bindable is RadialBarSeries series)
            {
                series.SegmentsCreated = false;
                series.ScheduleUpdateChart();
            }
        }

        static void OnSpacingPropertyChanged(BindableObject bindable, Object oldValue, Object newValue)
        {
            if (bindable is RadialBarSeries series)
            {
                series.ScheduleUpdateChart();
            }
        }

        static void OnCenterViewPropertyChanged(BindableObject bindable, Object oldValue, Object newValue)
        {
            if (bindable is RadialBarSeries series)
            {
                if (oldValue is View oldView)
                {
                    series.RemoveCenterView(oldView);
                }

                if (newValue is View newView)
                {
                    series.AddCenterView();
                    if (series.Chart != null)
                    {
                        series.UpdateCenterViewBounds(newView);
                    }
                }
            }
        }

        static void OnInnerRadiusPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (oldValue == newValue) return;

            if (bindable is RadialBarSeries series)
            {
                series.ScheduleUpdateChart();
            }
        }

        static object CoerceRadialBarCoefficient(BindableObject bindable, object value)
        {
            double coefficient = Convert.ToDouble(value);

            return coefficient > 1 ? 1 : coefficient < 0 ? 0 : value;
        }

        static object TrackFillValueCreator(BindableObject bindable)
        {
            return new SolidColorBrush(Color.FromRgba(0, 0, 0, 0.08));
        }

        static object TrackStrokeValueCreator(BindableObject bindable)
        {
            return new SolidColorBrush(Color.FromRgba(0, 0, 0, 0.24));
        }

        float GetTooltipRadius(RadialBarSegment radialBarSegment)
        {
            return (radialBarSegment.InnerRingRadius + radialBarSegment.OuterRingRadius) / 2;
        }

        void UpdateTrackProperties(Object property)
        {
            for (int i = 0; i < Segments.Count; i++)
            {
                var segment = (RadialBarSegment)Segments[i];
                switch (property)
                {
                    case double:
                        {
                            segment.TrackStrokeWidth = (float)TrackStrokeWidth;
                            break;
                        }
                    case Brush:
                        {
                            if (TrackFill != segment.TrackFill)
                            {
                                segment.TrackFill = TrackFill;
                            }
                            else if (TrackStroke != segment.TrackStroke)
                            {
                                segment.TrackStroke = TrackStroke;
                            }
                            break;
                        }
                }
            }
        }

        void AddCenterView()
        {
            if (ChartArea?.PlotArea is ChartPlotArea plotArea && CenterView != null)
            {
				CenterView.BindingContext = this;
				plotArea.Add(CenterView);
            }
        }

        void RemoveCenterView(View oldView)
        {
            if (ChartArea?.PlotArea is ChartPlotArea plotArea && plotArea.Contains(oldView))
            {
                oldView.RemoveBinding(AbsoluteLayout.LayoutBoundsProperty);
                oldView.RemoveBinding(AbsoluteLayout.LayoutFlagsProperty);
                SetInheritedBindingContext(oldView, null);
                plotArea.Remove(oldView);
            }
        }

        double GetCenterHoleSize()
        {
            var actualBounds = GetActualBound();

            return (float)InnerRadius * Math.Min(actualBounds.Width, actualBounds.Height);
        }

        RectF GetActualBound()
        {
            if (AreaBounds != Rect.Zero)
            {
                float minScale = (float)(Math.Min(AreaBounds.Width, AreaBounds.Height) * Radius);
                return new RectF(((Center.X * 2) - minScale) / 2, ((Center.Y * 2) - minScale) / 2, minScale, minScale);
            }

            return default(RectF);
        }

        #endregion

        #endregion
    }
}
