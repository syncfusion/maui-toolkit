using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Toolkit.Charts
{
    /// <summary>
    /// The <see cref="DoughnutSeries"/> displays data as a proportion of the whole. Its most commonly used to make comparisons among a set of given data.
    /// </summary>
    /// <remarks>
    /// <para>It is similar to the PieSeries. To render a series, create an instance of the doughnut series class, and add it to the <see cref="SfCircularChart.Series"/> collection.</para>
    /// 
    /// <para>It Provides options for <see cref="ChartSeries.PaletteBrushes"/>, <see cref="ChartSeries.Fill"/>, <see cref="CircularSeries.Stroke"/>, <see cref="CircularSeries.StrokeWidth"/>, and <see cref="InnerRadius"/> to customize the appearance.</para>
    /// 
    /// <para> <b>EnableTooltip - </b> The tooltip displays information while tapping or mouse hovering on the segment. To display the tooltip on the chart, you need to set the <see cref="ChartSeries.EnableTooltip"/> property as <b>true</b> in <see cref="DoughnutSeries"/> and refer to the <seealso cref="ChartBase.TooltipBehavior"/> property.</para>
    /// <para> <b>Data Label - </b> Data labels are used to display values related to a chart segment. To render the data labels, you need to set the <see cref="ChartSeries.ShowDataLabels"/> property as <b>true</b> in the <see cref="DoughnutSeries"/> class. To customize the chart data labels’ alignment, placement, and label styles, you need to create an instance of <see cref="CircularDataLabelSettings"/> and set it to the <see cref="CircularSeries.DataLabelSettings"/> property. </para>
    /// <para> <b>Animation - </b> To animate the series, set <b>True</b> to the <see cref="ChartSeries.EnableAnimation"/> property.</para>
    /// <para> <b>Selection - </b> To enable the data point selection in the series, create an instance of the <see cref="DataPointSelectionBehavior"/> and set it to the <see cref="ChartSeries.SelectionBehavior"/> property of the doughnut series. To highlight the selected segment, set the value for the <see cref="ChartSelectionBehavior.SelectionBrush"/> property in the <see cref="DataPointSelectionBehavior"/> class.</para>
    /// <para> <b>LegendIcon - </b> To customize the legend icon using the <see cref="ChartSeries.LegendIcon"/> property.</para>
    /// 
    /// </remarks>
    /// <example>
    /// # [Xaml](#tab/tabid-1)
    /// <code><![CDATA[
    ///     <chart:SfCircularChart>
    ///
    ///           <chart:SfCircularChart.Series>
    ///               <chart:DoughnutSeries
    ///                   ItemsSource="{Binding Data}"
    ///                   XBindingPath="XValue"
    ///                   YBindingPath="YValue"/>
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
    ///     DoughnutSeries series = new DoughnutSeries();
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
    public class DoughnutSeries : PieSeries
    {
        #region Fields

        float _doughnutStartAngle;
        float _doughnutEndAngle;
        double _total = 0;
        double _angleDifference;
        float _yValue;
        double _centerHoleSize = 1;

        #endregion

        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="InnerRadius"/> bindable property.
        /// </summary>
        /// <remarks>
        /// Represents the inner radius of the doughnut series, defining the size of the hole in the center.
        /// </remarks>
        public static readonly BindableProperty InnerRadiusProperty = BindableProperty.Create(
            nameof(InnerRadius),
            typeof(double),
            typeof(DoughnutSeries),
            0.4d,
            BindingMode.Default,
            null,
            OnInnerRadiusPropertyChanged,
            null,
            coerceValue: CoerceDoughnutCoefficient);

        /// <summary>
        /// Identifies the <see cref="CenterView"/> bindable property.
        /// </summary>
        /// <remarks>
        /// Represents the view at the center of the doughnut series.
        /// </remarks>
        public static readonly BindableProperty CenterViewProperty = BindableProperty.Create(
            nameof(CenterView),
            typeof(View),
            typeof(DoughnutSeries),
            null,
            BindingMode.Default,
            null,
            OnCenterViewChanged);

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a value that can be used to define the inner circle.
        /// </summary>
        /// <value>It accepts <c>double</c> values, and the default value is 0.4. Here, the value is between 0 and 1.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-4)
        /// <code><![CDATA[
        ///     <chart:SfCircularChart>
        ///
        ///          <chart:DoughnutSeries ItemsSource="{Binding Data}"
        ///                                XBindingPath="XValue"
        ///                                YBindingPath="YValue"
        ///                                InnerRadius = "0.5"/>
        ///
        ///     </chart:SfCircularChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-5)
        /// <code><![CDATA[
        ///     SfCircularChart chart = new SfCircularChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     DoughnutSeries series = new DoughnutSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           InnerRadius = 0.5,
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
        /// Gets or sets the view to be added to the center of the doughnut.
        /// </summary>
        /// <value>It accepts the <see cref="View"/> values and its defaults is null.</value> 
        /// 
        /// <img src="https://cdn.syncfusion.com/content/images/maui/MAUIDoughnutCenterView.png"/>
        /// 
        /// # [MainPage.xaml](#tab/tabid-6)
        /// <code><![CDATA[
        /// <chart:SfCircularChart>
        /// 
        ///         <chart:SfCircularChart.BindingContext>
        ///             <local:DoughnutSeriesViewModel/>
        ///         </chart:SfCircularChart.BindingContext>    
        /// 
        ///         <chart:SfCircularChart.Series>
        ///             <chart:DoughnutSeries ItemsSource="{Binding DoughnutSeriesData}" XBindingPath="Name" YBindingPath="Value"/>
        ///                  <chart:DoughnutSeries.CenterView>
        ///                    <Border HeightRequest="{Binding CenterHoleSize}" WidthRequest="{Binding CenterHoleSize}">
        ///                       <Border.StrokeShape>
        ///                         <RoundRectangle CornerRadius = "200"/>
        ///                    </ Border.StrokeShape>
        ///                    <StackLayout>
        ///                       <Label Text="Total :"/>
        ///                       <Label Text = "357,580 km²"/>
        ///                  </StackLayout>
        ///                   </ Border >
        ///                 </chart:DoughnutSeries.CenterView>
        ///         </chart:SfCircularChart.Series>
        /// 
        /// </chart:SfCircularChart>
        /// ]]>
        /// </code>
        /// 
        /// # [C#](#tab/tabid-7)
        /// <code><![CDATA[
        ///  SfCircularChart chart = new SfCircularChart();
        ///  
        ///  DoughnutSeriesViewModel viewModel = new DoughnutSeriesViewModel();
        ///	 chart.BindingContext = viewModel;
        ///  
        ///  DoughnutSeries series = new DoughnutSeries()
        ///  {
        ///     ItemsSource = viewModel.DoughnutSeriesData,
        ///     XBindingPath = "Name",
        ///     YBindingPath = "Value",
        ///  };
        ///  
        ///  Border border = new Border();
        ///  Label name = new Label();
        ///  name.Text = "Total :";
        ///  Label value = new Label();
        ///  value.Text = "357,580 km²";
        ///  StackLayout layout = new StackLayout();
        ///  layout.Add(name);
        ///  layout.Add(value);
        ///  border.Content = layout;
        ///  
        ///  series.CenterView = layout;
        ///  chart.Series.Add(series);
        /// ]]>
        /// </code>
        /// ***
        /// 
        public View CenterView
        {
            get { return (View)GetValue(CenterViewProperty); }
            set { SetValue(CenterViewProperty, value); }
        }

        /// <summary>
        /// Gets the size of the doughnut center hole.
        /// </summary>
        /// <value>Default value is 1.</value>
        /// 
        /// <para>It used to customize the view size which is placed in the doughnut <see cref="CenterView"/>.</para>
        /// 
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
        /// Initializes a new instance of the <see cref="DoughnutSeries"/> class.
        /// </summary>
        public DoughnutSeries() : base()
        {
            PaletteBrushes = ChartColorModel.DefaultBrushes;
        }

        #endregion

        #region Methods

        #region Protected Methods

        /// <inheritdoc/>
        protected override ChartSegment CreateSegment()
        {
            return new DoughnutSegment();
        }

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

        #endregion

        #region Internal Methods

        internal override void GenerateSegments(SeriesView seriesView)
        {
            if (YValues != null)
            {
                _doughnutStartAngle = (float)StartAngle;
                _angleDifference = GetAngleDifference();
                _total = CalculateTotalYValues();

                var oldSegments = OldSegments != null && OldSegments.Count > 0 && PointsCount == OldSegments.Count ? OldSegments : null;
                var legendItems = GetLegendItems();

                for (int i = 0; i < ActualYValues.Count; i++)
                {
                    _yValue = (legendItems == null || legendItems.Count == 0) ? (float)Math.Abs(double.IsNaN(ActualYValues[i]) ? double.NaN : ActualYValues[i]) : (float)(legendItems[i].IsToggled ? double.NaN : ActualYValues[i]);
                    _doughnutEndAngle = (float)(Math.Abs(float.IsNaN(_yValue) ? 0 : _yValue) * (_angleDifference / _total));

                    if (i < Segments.Count && Segments[i] is DoughnutSegment)
                    {
                        ((DoughnutSegment)Segments[i]).SetData(_doughnutStartAngle, _doughnutEndAngle, _yValue);
                    }
                    else
                    {
                        DoughnutSegment doughnutSegment = (DoughnutSegment)CreateSegment();
                        doughnutSegment.Series = this;
                        doughnutSegment.SeriesView = seriesView;
                        doughnutSegment.Index = i;
                        doughnutSegment.Exploded = ExplodeIndex == i;
                        doughnutSegment.SetData(_doughnutStartAngle, _doughnutEndAngle, _yValue);
                        doughnutSegment.Item = ActualData?[i];
                        InitiateDataLabels(doughnutSegment);
                        Segments.Add(doughnutSegment);

                        if (oldSegments != null)
                        {
                            var oldSegment = oldSegments[i] as DoughnutSegment;

                            if (oldSegment != null)
                                doughnutSegment.SetPreviousData(new[] { oldSegment.StartAngle, oldSegment.EndAngle });
                        }
                    }

                    if (Segments[i] is DoughnutSegment segment)
                    {
                        segment.SegmentStartAngle = _doughnutStartAngle;
                        segment.SegmentEndAngle = _doughnutStartAngle + _doughnutEndAngle;
                    }

                    if (Segments[i].IsVisible)
                        _doughnutStartAngle += _doughnutEndAngle;
                }
            }
        }

        internal override void OnSeriesLayout()
        {
            base.OnSeriesLayout();
            CenterHoleSize = GetCenterHoleSize();
            UpdateCenterViewBounds(CenterView);
        }

        internal override float GetDataLabelRadius()
        {
            float innerRadius = GetInnerRadius();
            float radius = DataLabelSettings.LabelPosition == ChartDataLabelPosition.Inside ? ((GetRadius() - innerRadius) / 2) + innerRadius : GetRadius();
            return radius;
        }

        internal override float GetTooltipRadius()
        {
            float innerRadius = GetInnerRadius();
            return ((GetRadius() - innerRadius) / 2) + innerRadius;
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

        #endregion

        #region Private Methods

        static void OnInnerRadiusPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (oldValue == newValue) return;

            if (bindable is DoughnutSeries series)
            {
                series.ScheduleUpdateChart();
            }
        }

        static void OnCenterViewChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is DoughnutSeries series)
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

        static object CoerceDoughnutCoefficient(BindableObject bindable, object value)
        {
            double coefficient = Convert.ToDouble(value);
            return coefficient > 1 ? 1 : coefficient < 0 ? 0 : value;
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

        float GetInnerRadius()
        {
            var actualBounds = GetActualBound();

            return (float)InnerRadius * (Math.Min(actualBounds.Width, actualBounds.Height) / 2);
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