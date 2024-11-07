using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// It is the base class for all types of polar series.
	/// </summary>
	public abstract class PolarSeries : ChartSeries, IDrawCustomLegendIcon, IMarkerDependent
    {
        #region Fields

        ChartAxis? _actualXAxis;
        ChartAxis? _actualYAxis;
        bool _needToAnimateMarker;

        #endregion

        #region Private Properties

        bool IsIndexed
        {
            get { return ActualXAxis is CategoryAxis; }
        }

        #endregion

        #region Internal Properties

        internal PolarChartArea? ChartArea { get; set; }

        internal IList<double> YValues { get; set; }

        internal override ChartDataLabelSettings ChartDataLabelSettings => DataLabelSettings;

        internal ChartAxis? ActualXAxis
        {
            get
            {
                return _actualXAxis;
            }
            set
            {
                if (_actualXAxis != null && value == null)
                {
                    _actualXAxis.ClearRegisteredSeries();
                }

                if (_actualXAxis != value)
                {
                    _actualXAxis = value;
                }
            }
        }

        internal ChartAxis? ActualYAxis
        {
            get
            {
                return _actualYAxis;
            }

            set
            {
                if (_actualXAxis != null && value == null)
                {
                    _actualXAxis.ClearRegisteredSeries();
                }

                if (_actualYAxis != value)
                {
                    _actualYAxis = value;
                }
            }
        }

        #endregion

        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="YBindingPath"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="YBindingPath"/> property specifies the path to the Y-axis for the polar series. 
        /// </remarks>
        public static readonly BindableProperty YBindingPathProperty = BindableProperty.Create(
            nameof(YBindingPath),
            typeof(string),
            typeof(PolarSeries),
            null,
            BindingMode.Default,
            null,
            OnYBindingPathChanged);

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

        /// <summary>
        /// Identifies the <see cref="DataLabelSettings"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="DataLabelSettings"/> property helps to customize the data labels in the polar series.
        /// </remarks>
        public static readonly BindableProperty DataLabelSettingsProperty = BindableProperty.Create(
            nameof(DataLabelSettings),
            typeof(PolarDataLabelSettings),
            typeof(PolarSeries),
            null,
            BindingMode.Default,
            null,
            OnDataLabelSettingsChanged);

        /// <summary>
        /// Identifies the <see cref="Label"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="Label"/> property represents the label that will be displayed in the associated legend item.
        /// </remarks>
        public static readonly BindableProperty LabelProperty = BindableProperty.Create(
            nameof(Label),
            typeof(string),
            typeof(PolarSeries),
            string.Empty,
            BindingMode.Default,
            null,
            OnLabelPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="IsClosed"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="IsClosed"/> property indicating whether the area path for the polar series should be closed or opened.
        /// </remarks>
        public static readonly BindableProperty IsClosedProperty = BindableProperty.Create(
            nameof(IsClosed),
            typeof(bool),
            typeof(PolarSeries),
            true,
            BindingMode.Default,
            null,
            OnIsClosedChanged);

        /// <summary>
        /// Identifies the <see cref="StrokeWidth"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="StrokeWidth"/> property indicates the thickness of stroke.
        /// </remarks>
        public static readonly BindableProperty StrokeWidthProperty = BindableProperty.Create(
            nameof(StrokeWidth),
            typeof(double),
            typeof(PolarSeries),
            2d,
            BindingMode.Default,
            null,
            OnStrokeWidthPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="StrokeDashArray"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The <see cref="StrokeDashArray"/> property specifies customization of the stroke patterns in the series.
        /// </remarks>
        public static readonly BindableProperty StrokeDashArrayProperty = BindableProperty.Create(
            nameof(StrokeDashArray),
            typeof(DoubleCollection),
            typeof(PolarSeries),
            null,
            BindingMode.Default,
            null,
            OnStrokeDashArrayPropertyChanged);

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a path value on the source object to serve a y value to the series.
        /// </summary>
        /// <value>
        /// The <c>string</c> that represents the property name for the y plotting data, and its default value is null.
        /// </value>
        /// <example>
        /// # [Xaml](#tab/tabid-1)
        /// <code><![CDATA[
        ///     <chart:SfPolarChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:PolarAreaSeries ItemsSource="{Binding Data}"
        ///                                 XBindingPath="XValue"
        ///                                 YBindingPath="YValue" />
        ///
        ///     </chart:SfPolarChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        ///     SfPolarChart chart = new SfPolarChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     PolarAreaSeries polarAreaSeries = new PolarAreaSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue"
        ///     };
        ///     
        ///     chart.Series.Add(polarAreaSeries);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public string YBindingPath
        {
            get { return (string)GetValue(YBindingPathProperty); }
            set { SetValue(YBindingPathProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value indicating whether to show markers for the series data point.
        /// </summary>
        /// <value>It accepts <c>bool</c> values and its default value is false.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-3)
        /// <code><![CDATA[
        ///     <chart:SfPolarChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:PolarAreaSeries ItemsSource="{Binding Data}"
        ///                                 XBindingPath="XValue"
        ///                                 YBindingPath="YValue"
        ///                                 ShowMarkers="True"/>
        ///
        ///     </chart:SfPolarChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-4)
        /// <code><![CDATA[
        ///     SfPolarChart chart = new SfPolarChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     PolarAreaSeries series = new PolarAreaSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           ShowMarkers= true
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
        /// # [Xaml](#tab/tabid-5)
        /// <code><![CDATA[
        ///     <chart:SfPolarChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:PolarAreaSeries ItemsSource="{Binding Data}"
        ///                                 XBindingPath="XValue"
        ///                                 YBindingPath="YValue"
        ///                                 ShowMarkers="True">
        ///               <chart:PolarAreaSeries.MarkerSettings>
        ///                     <chart:ChartMarkerSettings Fill="Red" Height="15" Width="15" />
        ///               </chart:PolarAreaSeries.MarkerSettings>
        ///          </chart:PolarAreaSeries>
        ///
        ///     </chart:SfPolarChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-6)
        /// <code><![CDATA[
        ///     SfPolarChart chart = new SfPolarChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///    ChartMarkerSettings chartMarkerSettings = new ChartMarkerSettings()
        ///     {
        ///        Fill = new SolidColorBrush(Colors.Red),
        ///        Height = 15,
        ///        Width = 15
        ///     };
        ///     PolarAreaSeries series = new PolarAreaSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           ShowMarkers= true,
        ///           MarkerSettings= chartMarkerSettings
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
        /// Gets or sets a value to customize the appearance of the displaying data labels in the polar series.
        /// </summary>
        /// <value>
        /// It takes the <see cref="PolarDataLabelSettings"/>.
        /// </value>
        /// <remarks> This allows us to change the look of the displaying labels' content, and appearance at the data point.</remarks>
        /// <example>
        /// # [Xaml](#tab/tabid-7)
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
        ///               <chart:PolarAreaSeries ItemsSource="{Binding Data}"
        ///                                      XBindingPath="XValue"
        ///                                      YBindingPath="YValue"
        ///                                      ShowDataLabels="True">
        ///                    <chart:PolarAreaSeries.DataLabelSettings>
        ///                         <chart:PolarDataLabelSettings LabelPlacement="Inner"/>
        ///                    </chart:PolarAreaSeries.DataLabelSettings>
        ///               </chart:PolarAreaSeries> 
        ///           
        ///     </chart:SfPolarChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-8)
        /// <code><![CDATA[
        ///     SfPolarChart chart = new SfPolarChart();
        ///     
        ///     NumericalAxis primaryAxis = new NumericalAxis();
        ///     NumericalAxis secondaryAxis = new NumericalAxis();
        ///     
        ///     chart.PrimaryAxis = primaryAxis;
        ///     chart.SecondaryAxis = secondaryAxis;
        ///     
        ///     PolarAreaSeries series = new PolarAreaSeries();
        ///     series.ItemsSource = viewModel.Data;
        ///     series.XBindingPath = "XValue";
        ///     series.YBindingPath = "YValue";
        ///     series.ShowDataLabels = "True";
        ///     
        ///     series.DataLabelSettings = new PolarDataLabelSettings()
        ///     {
        ///         LabelPlacement = DataLabelPlacement.Inner
        ///     };
        ///     chart.Series.Add(series);
        ///     
        /// ]]></code>
        /// ***
        /// </example>
        public PolarDataLabelSettings DataLabelSettings
        {
            get { return (PolarDataLabelSettings)GetValue(DataLabelSettingsProperty); }
            set { SetValue(DataLabelSettingsProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value displayed in the associated legend item.
        /// </summary>
        /// <value>It accepts a <c>string</c> value, and its default value is <c>string.Empty</c>.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-9)
        /// <code><![CDATA[
        ///     <chart:SfPolarChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:PolarAreaSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"
        ///                            Label = "PolarAreaSeries"/>
        ///
        ///     </chart:SfPolarChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-10)
        /// <code><![CDATA[
        ///     SfPolarChart chart = new SfPolarChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     PolarAreaSeries polarAreaSeries = new PolarAreaSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           Label = "PolarAreaSeries"
        ///     };
        ///     
        ///     chart.Series.Add(polarAreaSeries);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the area path for the polar series should be closed or opened.
        /// </summary>
        /// <value>This property takes the <c>bool</c> as its value and its default value is true.</value>
        /// <remarks>If its <c>true</c>, series path will be closed; otherwise opened.</remarks>
        /// <example>
        /// # [Xaml](#tab/tabid-11)
        /// <code><![CDATA[
        /// <chart:SfPolarChart>
        ///
        ///     <!-- Eliminated for simplicity-->
        ///
        ///     <chart:PolarAreaSeries ItemsSource="{Binding Data}" 
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"
        ///                            IsClosed="False"/>
        ///
        /// </chart:SfPolarChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-12)
        /// <code><![CDATA[
        /// SfPolarChart chart = new SfPolarChart();
        ///
        /// ViewModel viewModel = new ViewModel();
        ///
        /// // Eliminated for simplicity
        /// PolarAreaSeries series = new PolarAreaSeries();
        /// series.ItemsSource = viewModel.Data;
        /// series.XBindingPath = "XValue";
        /// series.YBindingPath = "YValue";
        /// series.IsClosed = false;
        /// chart.Series.Add(series);
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public bool IsClosed
        {
            get { return (bool)GetValue(IsClosedProperty); }
            set { SetValue(IsClosedProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value to specify the stroke width of a chart series.
        /// </summary>
        /// <value>It accepts <c>double</c> values, and its default value is 2.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-13)
        /// <code><![CDATA[
        ///     <chart:SfPolarChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:PolarLineSeries ItemsSource="{Binding Data}"
        ///                                 XBindingPath="XValue"
        ///                                 YBindingPath="YValue"
        ///                                 StrokeWidth = "3" />
        ///
        ///     </chart:SfPolarChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-14)
        /// <code><![CDATA[
        ///     SfPolarChart chart = new SfPolarChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     PolarLineSeries series = new PolarLineSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           StrokeWidth = 3
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public double StrokeWidth
        {
            get { return (double)GetValue(StrokeWidthProperty); }
            set { SetValue(StrokeWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the stroke dash array to customize the appearance of stroke.
        /// </summary>
        /// <value>It accepts the <see cref="DoubleCollection"/> value and the default value is null.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-15)
        /// <code><![CDATA[
        ///     <chart:SfPolarChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:PolarLineSeries ItemsSource="{Binding Data}"
        ///                                 XBindingPath="XValue"
        ///                                 YBindingPath="YValue"
        ///                                 StrokeDashArray="5,3"
        ///                                 Stroke = "Red" />
        ///
        ///     </chart:SfPolarChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-16)
        /// <code><![CDATA[
        ///     SfPolarChart chart = new SfPolarChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     DoubleCollection doubleCollection = new DoubleCollection();
        ///     doubleCollection.Add(5);
        ///     doubleCollection.Add(3);
        ///     PolarLineSeries series = new PolarLineSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           StrokeDashArray = doubleCollection,
        ///           Stroke = new SolidColorBrush(Colors.Red)
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public DoubleCollection StrokeDashArray
        {
            get { return (DoubleCollection)GetValue(StrokeDashArrayProperty); }
            set { SetValue(StrokeDashArrayProperty, value); }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="PolarSeries"/>
        /// </summary>
        public PolarSeries() : base()
        {
            YValues = new List<double>();
            DataLabelSettings = new PolarDataLabelSettings();
            MarkerSettings = new ChartMarkerSettings();
        }

        #endregion

        #region Interface Implementation 

        ChartMarkerSettings IMarkerDependent.MarkerSettings => MarkerSettings ?? new ChartMarkerSettings();

        void IMarkerDependent.DrawMarker(ICanvas canvas, int index, ShapeType type, Rect rect) => DrawMarker(canvas, index, type, rect);

        void IMarkerDependent.InvalidateDrawable()
        {
            InvalidateSeries();
        }

        bool IMarkerDependent.NeedToAnimateMarker { get => _needToAnimateMarker; set => _needToAnimateMarker = EnableAnimation; }

        void IDrawCustomLegendIcon.DrawSeriesLegend(ICanvas canvas, RectF rect, Brush fillColor, bool isSaveState)
        {
            if (isSaveState)
            {
                canvas.CanvasSaveState();
            }

            var pathF = new PathF();
            pathF.MoveTo(3, 12);
            pathF.LineTo(0, 4);
            pathF.LineTo(5, 0);
            pathF.LineTo((float)5.2, (float)0.9);
            pathF.LineTo((float)0.8, (float)4.4);
            pathF.LineTo((float)3.5, (float)11.4);
            pathF.LineTo(3, 12);
            pathF.Close();
            canvas.FillPath(pathF);

            var pathF1 = new PathF();
            pathF1.MoveTo(5, 0);
            pathF1.LineTo(12, 6);
            pathF1.LineTo(8, 12);
            pathF1.LineTo((float)7.8, (float)11.4);
            pathF1.LineTo(11, (float)6.3);
            pathF1.LineTo((float)5.2, (float)0.9);
            pathF1.LineTo(5, 0);
            pathF1.Close();
            canvas.FillPath(pathF1);

            var pathF2 = new PathF();
            pathF2.MoveTo((float)7.8, (float)11.4);
            pathF2.LineTo((float)3.5, (float)11.4);
            pathF2.LineTo(3, 12);
            pathF2.LineTo(8, 12);
            pathF2.LineTo((float)7.8, (float)11.4);
            pathF2.Close();
            canvas.FillPath(pathF2);

            if (isSaveState)
            {
                canvas.CanvasRestoreState();
            }
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <inheritdoc/>
        public override int GetDataPointIndex(float x, float y)
        {
            float xPos = x;
            float yPos = y;
            RectF seriesBounds = AreaBounds;
            List<PointF> segPoints = new List<PointF>();
            for (int i = 0; i < PointsCount; i++)
            {
                RectF clipRect = seriesBounds;
                float xPoint = TransformVisiblePoint(GetXValues()![i], YValues[i]).X;
                float yPoint = TransformVisiblePoint(GetXValues()![i], YValues[i]).Y;
                segPoints.Add(new PointF(xPoint, yPoint));

                foreach (ChartSegment segment in Segments)
                {
                    if (ChartSegment.IsRectContains(xPoint + clipRect.Left, yPoint + clipRect.Top, xPos, yPos, (float)StrokeWidth))
                    {
                        return i;
                    }
                    else if (segment is PolarAreaSegment)
                    {
                        if (ChartUtils.IsAreaContains(segPoints, x - clipRect.Left, y - clipRect.Top))
                        {
                            int tooltipIndex = GetTouchPointIndex(x - clipRect.Left, y - clipRect.Top);
                            return tooltipIndex;
                        }
                    }
                }
            }

            return -1;
        }

        #endregion

        #region Protected Methods

        /// <inheritdoc/>
        /// <exclude/>
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (MarkerSettings != null)
            {
                SetInheritedBindingContext(MarkerSettings, BindingContext);
            }

            if (DataLabelSettings != null)
            {
                SetInheritedBindingContext(DataLabelSettings, BindingContext);
            }
        }

        /// <summary>
        /// Draws the markers for the polar series at each data point location on the chart.
        /// </summary>
        protected virtual void DrawMarker(ICanvas canvas, int index, ShapeType type, Rect rect)
        {
            if (this is IMarkerDependent markerDependent)
            {
                canvas.DrawShape(rect, shapeType: type, markerDependent.MarkerSettings.HasBorder, false);
            }
        }

        #endregion

        #region Internal Methods

        internal PointF TransformVisiblePoint(double xValue, double yValue, float animation)
        {
            var xAxis = ActualXAxis;
            var yAxis = ActualYAxis;
            float radius;

            if (xAxis == null || yAxis == null || ChartArea == null)
            {
                return PointF.Zero;
            }

            var clip = ChartArea.ActualSeriesClipRect;
            float polarClipHeight = Math.Min(clip.Width, clip.Height) - (2 * yAxis.RenderedRect.Height);
            float segmentRadius = yAxis.ValueToPoint(yValue);
            radius = (float)(Math.Min(xAxis.AvailableSize.Width / 2, xAxis.AvailableSize.Height / 2) - segmentRadius - (polarClipHeight / 2));
            float angleValue = (float)xAxis.ValueToPolarAngle(xValue);

            return ChartArea.PolarAngleToPoint(xAxis, radius * animation, angleValue);
        }

        internal override void OnBindingPathChanged()
        {
            ResetData();
            GeneratePoints(new[] { YBindingPath }, YValues);
            base.OnBindingPathChanged();
        }

        internal override void OnDataSourceChanged(object oldValue, object newValue)
        {
            YValues.Clear();
            GeneratePoints(new[] { YBindingPath }, YValues);
            base.OnDataSourceChanged(oldValue, newValue);
        }

        internal override void UpdateRange()
        {
            if (Segments.Count == 0 && PointsCount == 1)
            {
                var xValues = GetXValues();

                if (xValues != null && xValues.Count > 0)
                {
                    XRange = new DoubleRange(xValues[0], xValues[0]);
                }

                if (YValues != null && YValues.Count > 0)
                {
                    YRange = new DoubleRange(YValues[0], YValues[0]);
                }
            }

            VisibleXRange = XRange;
            VisibleYRange = YRange;
        }

        internal List<double>? GetXValues()
        {
            if (ActualXValues == null)
            {
                return null;
            }

            double xIndexValues = 0d;
            var xValues = ActualXValues as List<double>;

            if (IsIndexed || xValues == null)
            {
                if (ActualXAxis is CategoryAxis categoryAxis && !categoryAxis.ArrangeByIndex || ActualXAxis == null)
                {
                    xValues = GroupedXValuesIndexes.Count > 0 ? GroupedXValuesIndexes : (from val in (ActualXValues as List<string>) select (xIndexValues++)).ToList();
                }
                else
                {
                    xValues = xValues != null ? (from val in xValues select (xIndexValues++)).ToList() : (from val in (ActualXValues as List<string>) select (xIndexValues++)).ToList();
                }
            }

            return xValues;
        }

        internal void UpdateAssociatedAxes()
        {
            if (_actualXAxis != null && _actualYAxis != null)
            {
                if (!_actualXAxis.AssociatedAxes.Contains(_actualYAxis))
                {
                    _actualXAxis.AssociatedAxes.Add(_actualYAxis);
                }

                if (!_actualYAxis.AssociatedAxes.Contains(_actualXAxis))
                {
                    _actualYAxis.AssociatedAxes.Add(_actualXAxis);
                }
            }
        }

        internal override TooltipInfo? GetTooltipInfo(ChartTooltipBehavior tooltipBehavior, float x, float y)
        {
            RectF seriesBounds = AreaBounds;
            int index = SeriesContainsPoint(new PointF(x, y)) ? TooltipDataPointIndex : -1;
            if (index < 0)
            {
                return null;
            }

            var xValues = GetXValues();
            if (xValues == null || ChartArea == null) return null;

            object dataPoint = ActualData![index];
            double xValue = xValues[index];
            var yValues = SeriesYValues![0];
            double yValue = Convert.ToDouble(yValues[index]);
            float xPosition = TransformVisiblePoint(xValue, yValue).X;
            float yPosition = TransformVisiblePoint(xValue, yValue).Y;

            if (!double.IsNaN(xPosition))
            {
                if (double.IsNaN(yValue))
                {
                    return null;
                }

                yPosition = seriesBounds.Top < yPosition ? yPosition : seriesBounds.Top;
                yPosition = seriesBounds.Bottom > yPosition ? yPosition : seriesBounds.Bottom;
                xPosition = seriesBounds.Left < xPosition ? xPosition : seriesBounds.Left;
                xPosition = seriesBounds.Right > xPosition ? xPosition : seriesBounds.Right;

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

            return null;
        }

        internal override Brush? GetFillColor(object item, int index)
        {
            Brush? fillColor = base.GetFillColor(item, index);
            if (fillColor == null && ChartArea != null)
            {
                if (ChartArea.PaletteColors != null && ChartArea.PaletteColors.Count > 0)
                {
                    if (ChartArea.Series is ChartPolarSeriesCollection series)
                    {
                        var seriesIndex = series.IndexOf(this);

                        if (seriesIndex >= 0)
                        {
                            fillColor = ChartArea.PaletteColors[seriesIndex % ChartArea.PaletteColors.Count];
                        }
                    }
                }
            }

            return fillColor ?? new SolidColorBrush(Colors.Transparent);
        }

        internal override void UpdateLegendIconColor()
        {
            var legend = Chart?.Legend;
            var legendItems = ChartArea?.PlotArea.LegendItems;

            if (legend != null && legend.IsVisible && legendItems != null)
            {
                foreach (LegendItem legendItem in legendItems)
                {
                    if (legendItem != null && legendItem.Item == this)
                    {
                        legendItem.IconBrush = GetFillColor(legendItem, legendItem.Index) ?? new SolidColorBrush(Colors.Transparent);
                        break;
                    }
                }
            }
        }

        internal override void UpdateLegendIconColor(ChartSelectionBehavior sender, int index)
        {
            var legend = Chart?.Legend;
            var legendItems = ChartArea?.PlotArea.LegendItems;
            if (legend != null && legend.IsVisible && legendItems != null && index < legendItems.Count)
            {
                if (legendItems[index] is LegendItem legendItem && legendItem.Item == this)
                {
                    legendItem.IconBrush = GetFillColor(legendItem, index) ?? new SolidColorBrush(Colors.Transparent);
                }
            }
        }

        internal override void SetStrokeWidth(ChartSegment segment)
        {
            segment.StrokeWidth = StrokeWidth;
        }

        internal override void SetDashArray(ChartSegment segment)
        {
            segment.StrokeDashArray = StrokeDashArray;
        }

        internal PointF CalculateDataLabelPoint(ChartSegment dataLabel, PointF labelPoint, ChartDataLabelStyle labelStyle)
        {
            SizeF labelSize = LabelTemplate == null ? labelStyle.MeasureLabel(dataLabel.LabelContent ?? string.Empty) : GetLabelTemplateSize(dataLabel);
            float padding = (float)labelStyle.LabelPadding;
            dataLabel.LabelPositionPoint = GetDataLabelPosition(dataLabel, labelSize, labelPoint, padding);
            return dataLabel.LabelPositionPoint;
        }

        internal void CalculateDataPointPosition(int index, ref double x, ref double y)
        {
            PointF point = TransformVisiblePoint(x, y);
            x = point.X;
            y = point.Y;
        }

        internal override PointF GetDataLabelPosition(ChartSegment dataLabel, SizeF labelSize, PointF labelPoint, float padding)
        {
            var dataLabeSettings = DataLabelSettings;
            ChartDataLabelStyle labelStyle = dataLabeSettings.LabelStyle;

            labelPoint.X = (float)(labelPoint.X + labelStyle.OffsetX);
            labelPoint.Y = (float)(labelPoint.Y + labelStyle.OffsetY);

            PointF labelPosition = PointF.Zero;

            switch (dataLabeSettings.LabelPlacement)
            {
                case DataLabelPlacement.Auto:
                    labelPosition = base.GetDataLabelPosition(dataLabel, labelSize, labelPoint, padding);
                    break;
                case DataLabelPlacement.Center:
                    labelPosition = base.GetDataLabelPosition(dataLabel, labelSize, labelPoint, padding);
                    break;
                case DataLabelPlacement.Inner:
                    labelPosition = CalculateDataLabelInnerPosition(dataLabel, labelSize, labelPoint, padding);
                    break;
                case DataLabelPlacement.Outer:
                    labelPosition = CalculateDataLabelOuterPosition(dataLabel, labelSize, labelPoint, padding);
                    break;
            }

            return labelPosition;
        }

        internal void PaletteColorsChanged()
        {
            UpdateColor();
            InvalidateSeries();

            if (ShowDataLabels)
            {
                ChartArea?.InvalidateDataLabelView();
            }

            UpdateLegendIconColor();
        }

        internal override void InvalidateSeries()
        {
            ChartArea?.InvalidateSeries(this);
        }

        #endregion

        #region Private Methods

        PointF TransformVisiblePoint(double xValue, double yValue)
        {
            return TransformVisiblePoint(xValue, yValue, 1);
        }

        int GetTouchPointIndex(float x, float y)
        {
            if (ChartArea == null) return -1;
            PointF center = ChartArea.PolarAxisCenter;
            float touchPointAngle = ChartUtils.GetMidValue(center, x, y);

            List<float> midAngle = ChartUtils.GetMidAngles(center, GetMidPoints());

            for (int i = 0; i < midAngle.Count; i++)
            {
                if (touchPointAngle < midAngle[i])
                {
                    return i;
                }
            }

            return 0;
        }

        List<float> GetMidPoints()
        {
            List<float> midPoints = new List<float>();
            List<float> fillPoints = new List<float>();

            var xValues = GetXValues();
            if (xValues != null)
            {
                for (int i = 0; i < PointsCount; i++)
                {
                    float xPoint = TransformVisiblePoint(xValues[i], YValues[i]).X;
                    float yPoint = TransformVisiblePoint(xValues[i], YValues[i]).Y;
                    fillPoints.Add(xPoint);
                    fillPoints.Add(yPoint);

                    int count = fillPoints.Count;

                    if (fillPoints.Count > 3)
                    {
                        float midXPoint = (fillPoints[count - 4] + fillPoints[count - 2]) / 2;
                        float midYPoint = (fillPoints[count - 3] + fillPoints[count - 1]) / 2;
                        midPoints.Add(midXPoint);
                        midPoints.Add(midYPoint);
                    }
                    else
                    {
                        continue;
                    }
                }

                float lastXPoint = TransformVisiblePoint(xValues[0], YValues[0]).X;
                float lastYPoint = TransformVisiblePoint(xValues[0], YValues[0]).Y;
                fillPoints.Add(lastXPoint);
                fillPoints.Add(lastYPoint);

                int LastCount = fillPoints.Count;

                float lastMidXPoint = (fillPoints[LastCount - 4] + fillPoints[LastCount - 2]) / 2;
                float lastMidYPoint = (fillPoints[LastCount - 3] + fillPoints[LastCount - 1]) / 2;
                midPoints.Add(lastMidXPoint);
                midPoints.Add(lastMidYPoint);
            }

            return midPoints;
        }

        PointF CalculateDataLabelInnerPosition(ChartSegment dataLabel, SizeF labelSize, PointF labelPoint, float padding)
        {
            if (ChartArea == null) return labelPoint;

            var index = dataLabel.Index;
            float xPos = labelPoint.X;
            float yPos = labelPoint.Y;
            float labelWidth = labelSize.Width;
            float labelHeight = labelSize.Height;

            var centerX = ChartArea.ActualSeriesClipRect.Width / 2;
            var centerY = ChartArea.ActualSeriesClipRect.Height / 2;
            bool isLeft = xPos < centerX, isBottom = yPos > centerY;

            if (xPos == centerX)
            {
                yPos = isBottom ? yPos - (labelHeight / 2) - padding : yPos + (labelHeight / 2) + padding;
            }
            else if (yPos == centerY)
            {
                xPos = isLeft ? xPos + (labelWidth / 2) + padding : xPos - (labelWidth / 2) - padding;
            }
            else if (isLeft)
            {
                xPos = xPos + (labelWidth / 2) + padding;
                yPos = isBottom ? yPos - (labelHeight / 2) - padding : yPos + (labelHeight / 2) + padding;
            }
            else
            {
                xPos = xPos - (labelWidth / 2) - padding;
                yPos = isBottom ? yPos - (labelHeight / 2) - padding : yPos + (labelHeight / 2) + padding;
            }

            labelPoint.X = xPos;
            labelPoint.Y = yPos;

            return labelPoint;
        }

        PointF CalculateDataLabelOuterPosition(ChartSegment dataLabel, SizeF labelSize, PointF labelPoint, float padding)
        {
            if (ChartArea == null) return labelPoint;

            var index = dataLabel.Index; float xPos = labelPoint.X;
            float yPos = labelPoint.Y;
            float labelWidth = labelSize.Width;
            float labelHeight = labelSize.Height;

            var centerX = ChartArea.ActualSeriesClipRect.Width / 2;
            var centerY = ChartArea.ActualSeriesClipRect.Height / 2;
            bool isLeft = xPos < centerX, isBottom = yPos > centerY;

            if (xPos == centerX)
            {
                yPos = isBottom ? yPos + (labelHeight / 2) + padding : yPos - (labelHeight / 2) - padding;
            }
            else if (yPos == centerY)
            {
                xPos = isLeft ? xPos - (labelWidth / 2) - padding : xPos + (labelWidth / 2) + padding;
            }
            else if (isLeft)
            {
                xPos = xPos - (labelWidth / 2) - padding;
                yPos = isBottom ? yPos + (labelHeight / 2) + padding : yPos - (labelHeight / 2) - padding;
            }
            else
            {
                xPos = xPos + (labelWidth / 2) + padding;
                yPos = isBottom ? yPos + (labelHeight / 2) + padding : yPos - (labelHeight / 2) - padding;
            }

            labelPoint.X = xPos;
            labelPoint.Y = yPos;

            return labelPoint;
        }

        new void UpdateColor()
        {
            foreach (var segment in Segments)
            {
                SetFillColor(segment);
            }
        }

        static void OnYBindingPathChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is PolarSeries series)
            {
                series.OnBindingPathChanged();
            }
        }

        static void OnDataLabelSettingsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is PolarSeries series)
            {
                series.OnDataLabelSettingsPropertyChanged(oldValue as ChartDataLabelSettings, newValue as ChartDataLabelSettings);
            }
        }

        static void OnLabelPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is PolarSeries chartSeries)
            {
                var legendItems = chartSeries.ChartArea?.PlotArea.LegendItems;

                if (legendItems != null)
                {
                    foreach (LegendItem legendItem in legendItems)
                    {
                        if (legendItem != null && legendItem.Item == chartSeries)
                        {
                            legendItem.Text = chartSeries.Label;
                            break;
                        }
                    }
                }
            }
        }

        static void OnIsClosedChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is PolarSeries series)
            {
                series.SegmentsCreated = false;
                series.ScheduleUpdateChart();
            }
        }

        static void OnStrokeWidthPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is PolarSeries series)
            {
                series.UpdateStrokeWidth();
                series.InvalidateSeries();
            }
        }

        static void OnStrokeDashArrayPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is PolarSeries series)
            {
                series.UpdateDashArray();
                series.InvalidateSeries();
            }
        }

        #endregion

        #endregion
    }
}