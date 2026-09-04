using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Toolkit;
using Syncfusion.Maui.Toolkit.Internals;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core = Syncfusion.Maui.Toolkit;

namespace Syncfusion.Maui.Toolkit.Charts
{
    /// <summary>
    /// Represents the base abstract class for all trendline types in the chart.
    /// </summary>
    public abstract class ChartTrendline : BindableObject, ITooltipDependent, IMarkerDependent, ICartesianLegendDependent
    {
        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="Stroke"/> bindable property.
        /// </summary>
        public static readonly BindableProperty StrokeProperty =
            BindableProperty.Create(nameof(Stroke), typeof(Brush), typeof(ChartTrendline), null, BindingMode.Default, null, OnStrokePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="StrokeWidth"/> bindable property.
        /// </summary>
        public static readonly BindableProperty StrokeWidthProperty =
            BindableProperty.Create(nameof(StrokeWidth), typeof(double), typeof(ChartTrendline), 2.0, BindingMode.Default, null, OnStrokeWidthPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="StrokeDashArray"/> bindable property.
        /// </summary>
        public static readonly BindableProperty StrokeDashArrayProperty =
            BindableProperty.Create(nameof(StrokeDashArray), typeof(DoubleCollection), typeof(ChartTrendline), null, BindingMode.Default, null, OnStrokeDashArrayPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="Opacity"/> bindable property.
        /// </summary>
        public static readonly BindableProperty OpacityProperty =
            BindableProperty.Create(nameof(Opacity), typeof(double), typeof(ChartTrendline), 1.0, BindingMode.Default, null, OnOpacityPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="IsVisible"/> bindable property.
        /// </summary>
        public static readonly BindableProperty IsVisibleProperty =
            BindableProperty.Create(nameof(IsVisible), typeof(bool), typeof(ChartTrendline), true, BindingMode.TwoWay, null, OnIsVisiblePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="Label"/> bindable property.
        /// </summary>
        public static readonly BindableProperty LabelProperty =
            BindableProperty.Create(nameof(Label), typeof(string), typeof(ChartTrendline), null, BindingMode.Default, null, OnLabelPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="ForwardForecast"/> bindable property.
        /// </summary>
        public static readonly BindableProperty ForwardForecastProperty =
            BindableProperty.Create(nameof(ForwardForecast), typeof(int), typeof(ChartTrendline), 0, BindingMode.Default, null, OnForwardForecastPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="BackwardForecast"/> bindable property.
        /// </summary>
        public static readonly BindableProperty BackwardForecastProperty =
            BindableProperty.Create(nameof(BackwardForecast), typeof(int), typeof(ChartTrendline), 0, BindingMode.Default, null, OnBackwardForecastPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="EnableTooltip"/> bindable property.
        /// </summary>
        public static readonly BindableProperty EnableTooltipProperty =
            BindableProperty.Create(nameof(EnableTooltip), typeof(bool), typeof(ChartTrendline), false, BindingMode.Default, null);

        /// <summary>
        /// Identifies the <see cref="TooltipTemplate"/> bindable property.
        /// </summary>
        public static readonly BindableProperty TooltipTemplateProperty =
            BindableProperty.Create(nameof(TooltipTemplate), typeof(DataTemplate), typeof(ChartTrendline), null, BindingMode.Default, null);

        /// <summary>
        /// Identifies the <see cref="ShowTrackballLabel"/> bindable property.
        /// </summary>
        public static readonly BindableProperty ShowTrackballLabelProperty =
            BindableProperty.Create(nameof(ShowTrackballLabel), typeof(bool), typeof(ChartTrendline), false, BindingMode.Default, null);

        /// <summary>
        /// Identifies the <see cref="TrackballLabelTemplate"/> bindable property.
        /// </summary>
        public static readonly BindableProperty TrackballLabelTemplateProperty =
            BindableProperty.Create(nameof(TrackballLabelTemplate), typeof(DataTemplate), typeof(ChartTrendline), null, BindingMode.Default, null);

        /// <summary>
        /// Identifies the <see cref="ShowMarkers"/> bindable property.
        /// </summary>
        public static readonly BindableProperty ShowMarkersProperty = ChartMarker.ShowMarkersProperty;

        /// <summary>
        /// Identifies the <see cref="MarkerSettings"/> bindable property.
        /// </summary>
        public static readonly BindableProperty MarkerSettingsProperty = ChartMarker.MarkerSettingsProperty;

        /// <summary>
        /// Identifies the <see cref="UseSeriesFillColor"/> bindable property.
        /// </summary>
        public static readonly BindableProperty UseSeriesFillColorProperty =
            BindableProperty.Create(nameof(UseSeriesFillColor), typeof(bool), typeof(ChartTrendline), false, BindingMode.Default, null);

        /// <summary>
        /// Identifies the internal value-member bindable property used to pick a Y field.
        /// (defaults to high) when the parent series exposes multiple value members.
        /// </summary>
        internal static readonly BindableProperty ValueMemberPathProperty =
            BindableProperty.Create(nameof(ValueMemberPath), typeof(string), typeof(ChartTrendline), "high", BindingMode.Default, null, OnValueMemberFieldPropertyChanged);

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the brush used to paint the trendline.
        /// </summary>
        /// <value>The default value is null.</value>
        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the width of the trendline stroke.
        /// </summary>
        /// <value>The default value is 2.0.</value>
        public double StrokeWidth
        {
            get { return (double)GetValue(StrokeWidthProperty); }
            set { SetValue(StrokeWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the stroke dash pattern for the trendline.
        /// </summary>
        /// <value>The default value is null.</value>
        public DoubleCollection StrokeDashArray
        {
            get { return (DoubleCollection)GetValue(StrokeDashArrayProperty); }
            set { SetValue(StrokeDashArrayProperty, value); }
        }

        /// <summary>
        /// Gets or sets the opacity of the trendline.
        /// </summary>
        /// <value>The default value is 1.0.</value>
        public double Opacity
        {
            get { return (double)GetValue(OpacityProperty); }
            set { SetValue(OpacityProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the trendline is visible.
        /// </summary>
        /// <value>The default value is true.</value>
        public bool IsVisible
        {
            get { return (bool)GetValue(IsVisibleProperty); }
            set { SetValue(IsVisibleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the label for the trendline.
        /// </summary>
        /// <value>The default value is null.</value>
        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        /// <summary>
        /// Gets or sets the number of periods to forecast forward.
        /// </summary>
        /// <value>The default value is 0.</value>
        public int ForwardForecast
        {
            get { return (int)GetValue(ForwardForecastProperty); }
            set { SetValue(ForwardForecastProperty, value); }
        }

        /// <summary>
        /// Gets or sets the number of periods to forecast backward.
        /// </summary>
        /// <value>The default value is 0.</value>
        public int BackwardForecast
        {
            get { return (int)GetValue(BackwardForecastProperty); }
            set { SetValue(BackwardForecastProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the tooltip for trendline should be shown or hidden.
        /// </summary>
        /// <remarks>The trendline tooltip will appear when you click or tap the trendline area.</remarks>
        /// <value>It accepts bool values and its default value is <c>False</c>.</value>
        public bool EnableTooltip
        {
            get { return (bool)GetValue(EnableTooltipProperty); }
            set { SetValue(EnableTooltipProperty, value); }
        }

        /// <summary>
        /// Gets or sets the DataTemplate that can be used to customize the appearance of the trendline tooltip.
        /// </summary>
        /// <value>
        /// It accepts a <see cref="DataTemplate"/> value.
        /// </value>
        public DataTemplate TooltipTemplate
        {
            get { return (DataTemplate)GetValue(TooltipTemplateProperty); }
            set { SetValue(TooltipTemplateProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show trackball label for the trendline.
        /// </summary>
        /// <remarks>The trendline trackball label will appear when trackball is activated and intersects with the trendline.</remarks>
        /// <value>It accepts bool values and its default value is <c>False</c>.</value>
        public bool ShowTrackballLabel
        {
            get { return (bool)GetValue(ShowTrackballLabelProperty); }
            set { SetValue(ShowTrackballLabelProperty, value); }
        }

        /// <summary>
        /// Gets or sets the DataTemplate that can be used to customize the appearance of the trendline trackball label.
        /// </summary>
        /// <value>
        /// It accepts a <see cref="DataTemplate"/> value.
        /// </value>
        public DataTemplate TrackballLabelTemplate
        {
            get { return (DataTemplate)GetValue(TrackballLabelTemplateProperty); }
            set { SetValue(TrackballLabelTemplateProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show markers for the trendline data points.
        /// </summary>
        /// <remarks>
        /// <para>When enabled, markers will be displayed at calculated trendline points to highlight key positions along the trendline.</para>
        /// <para>The marker appearance can be customized using the <see cref="MarkerSettings"/> property.</para>
        /// <para>Markers support tooltip interaction when <see cref="EnableTooltip"/> is also enabled.</para>
        /// </remarks>
        /// <value>The default value is <c>false</c>.</value>
        /// <example>
        /// <code><![CDATA[
        /// <chart:LinearTrendline ShowMarkers="True">
        ///     <chart:LinearTrendline.MarkerSettings>
        ///         <chart:ChartMarkerSettings Type="Circle" Fill="Red" Width="8" Height="8" />
        ///     </chart:LinearTrendline.MarkerSettings>
        /// </chart:LinearTrendline>
        /// ]]></code>
        /// </example>
        public bool ShowMarkers
        {
            get { return (bool)GetValue(ShowMarkersProperty); }
            set { SetValue(ShowMarkersProperty, value); }
        }

        /// <summary>
        /// Gets or sets the marker settings to customize the appearance of trendline markers.
        /// </summary>
        /// <remarks>
        /// <para>This property allows customization of marker appearance including type, size, fill, stroke, and stroke width.</para>
        /// <para>Markers are only displayed when <see cref="ShowMarkers"/> is set to <c>true</c>.</para>
        /// <para>The marker settings follow the same pattern as series markers, supporting all standard marker shapes and styles.</para>
        /// </remarks>
        /// <value>
        /// It accepts a <see cref="ChartMarkerSettings"/> instance. If null, default marker settings will be used.
        /// </value>
        /// <example>
        /// <code><![CDATA[
        /// <chart:LinearTrendline ShowMarkers="True">
        ///     <chart:LinearTrendline.MarkerSettings>
        ///         <chart:ChartMarkerSettings Type="Diamond" Fill="Blue" Stroke="DarkBlue" StrokeWidth="2" Width="10" Height="10" />
        ///     </chart:LinearTrendline.MarkerSettings>
        /// </chart:LinearTrendline>
        /// ]]></code>
        /// </example>
        public ChartMarkerSettings MarkerSettings
        {
            get { return (ChartMarkerSettings)GetValue(MarkerSettingsProperty); }
            set { SetValue(MarkerSettingsProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the trendline tooltip background uses the associated series fill color.
        /// </summary>
        /// <remarks>
        /// <para>This property allows the trendline tooltip to visually match its corresponding series by applying the series fill color.</para>
        /// <para>For gradient series fills, the tooltip uses the primary (first gradient stop) color as a solid background.</para>
        /// <para>This improves visual association between the trendline tooltip and its related series, especially in multi-series charts.</para>
        /// </remarks>
        /// <value>
        /// It accepts <see cref="bool"/> values. The default value is <c>false</c>.
        /// When set to <c>true</c>, the tooltip background uses the associated series fill color.
        /// </value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-1)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart.Series>
        ///     <chart:ColumnSeries ItemsSource="{Binding Data}"
        ///                         XBindingPath="Category"
        ///                         YBindingPath="Value"
        ///                         Fill="ForestGreen">
        ///
        ///         <chart:ColumnSeries.Trendlines>
        ///             <chart:LinearTrendline EnableTooltip="True"
        ///                                    ShowTrackballLabel="True"
        ///                                    UseSeriesFillColor="True"/>
        ///         </chart:ColumnSeries.Trendlines>
        ///
        ///     </chart:ColumnSeries>
        /// </chart:SfCartesianChart.Series>
        /// ]]></code>
        ///
        /// # [MainPage.xaml.cs](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// ViewModel viewModel = new ViewModel();
        ///
        /// ColumnSeries series = new ColumnSeries()
        /// {
        ///     ItemsSource = viewModel.Data,
        ///     XBindingPath = "Category",
        ///     YBindingPath = "Value",
        ///     Fill = new SolidColorBrush(Colors.ForestGreen)
        /// };
        ///
        /// LinearTrendline trendline = new LinearTrendline()
        /// {
        ///     EnableTooltip = true,
        ///     ShowTrackballLabel = true,
        ///     UseSeriesFillColor = true
        /// };
        ///
        /// series.Trendlines.Add(trendline);
        /// chart.Series.Add(series);
        /// ]]></code>
        /// </example>
        public bool UseSeriesFillColor
        {
            get { return (bool)GetValue(UseSeriesFillColorProperty); }
            set { SetValue(UseSeriesFillColorProperty, value); }
        }

        bool ICartesianLegendDependent.IsVisible { get { return IsVisible; } }

        string ICartesianLegendDependent.LegendText { get { return Label; } }

        Core.ShapeType ICartesianLegendDependent.LegendIcon => (Core.ShapeType)ChartUtils.GetTrendlineIconType(this);

        Brush? ICartesianLegendDependent.GetLegendBrush(object item, int index)
        {
            return GetLegendBrush();
        }

        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets or sets the internal value-member path (e.g., high) consumed while extracting Y-values.
        /// </summary>
        internal string ValueMemberPath
        {
            get { return (string)GetValue(ValueMemberPathProperty); }
            set { SetValue(ValueMemberPathProperty, value); }
        }

        /// <summary>
        /// Gets or sets the parent series for this trendline.
        /// </summary>
        internal CartesianSeries? Series { get; set; }

        internal bool needsDeferredRefresh;

        internal bool? CachedVisibilityFromSeriesToggle { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the trendline calculation failed or data is invalid.
        /// Following ChartSegment Empty property pattern.
        /// </summary>
        internal bool Empty { get; set; } = false;

        internal List<double> XValues { get; set; } = new List<double>();
        internal List<double> YValues { get; set; } = new List<double>();
        internal List<float> XCoordinates { get; set; } = new List<float>();
        internal List<float> YCoordinates { get; set; } = new List<float>();

        internal double XMin = double.PositiveInfinity;
        internal double XMax = double.NegativeInfinity;
        internal double YMin = double.PositiveInfinity;
        internal double YMax = double.NegativeInfinity;

        DataTemplate ITooltipDependent.TooltipTemplate { get => TooltipTemplate; set => TooltipTemplate = value; }
        bool ITooltipDependent.EnableTooltip { get => EnableTooltip; set => EnableTooltip = value; }

        DataTemplate? ITooltipDependent.GetDefaultTooltipTemplate(TooltipInfo info)
        {
            return GetDefaultTrendlineTooltipTemplate(info);
        }


        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartTrendline"/> class.
        /// </summary>
        public ChartTrendline()
        {
            MarkerSettings = new ChartMarkerSettings();
        }

		#endregion

		#region Methods

		#region Binding Context Management

		/// <summary>
		/// Called when the binding context changes.
		/// </summary>
		protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (MarkerSettings != null)
            {
                SetInheritedBindingContext(MarkerSettings, BindingContext);
            }
        }

        #endregion

        #region ChartSegment-Style Methods

        /// <summary>
        /// Performs mathematical calculations and coordinate transformation for the trendline.
        /// This method follows the ChartSegment OnLayout pattern exactly like LineSegment.
        /// </summary>
        internal virtual void OnLayout()
        {
            BuildScreenCoordinates();
        }

        /// <summary>
        /// 
        /// </summary>
        internal virtual void GeneratePoints()
        {

        }

        internal void MarkForRefresh()
        {
            if (Series != null)
            {
                RefreshTrendline();
            }
            else
            {
                needsDeferredRefresh = true;
            }
        }

        internal void RefreshTrendline()
        {
            GeneratePoints();
            OnLayout();
            InvalidateTrendlines();
            needsDeferredRefresh = false;
        }

        /// <summary>
        /// Renders the trendline using screen coordinates and canvas operations.
        /// </summary>
        /// <param name="canvas">The canvas on which to render the trendline.</param>
        internal virtual void Draw(ICanvas canvas)
        {
            if (Series == null || Empty)
            {
                return;
            }

            if (ShowMarkers && MarkerSettings != null)
            {
                DrawMarkers(canvas);
            }
        }

        #endregion

        #region Shared Calculation Helpers

        /// <summary>
        /// Resets internal collections, bounds, and emptiness flags before recomputing points.
        /// </summary>
        /// <param name="clearScreenCoordinates">True to also clear cached screen coordinates.</param>
        internal void ResetComputationState(bool clearScreenCoordinates = true)
        {
            XValues.Clear();
            YValues.Clear();

            if (clearScreenCoordinates)
            {
                XCoordinates.Clear();
                YCoordinates.Clear();
            }

            XMin = double.PositiveInfinity;
            XMax = double.NegativeInfinity;
            YMin = double.PositiveInfinity;
            YMax = double.NegativeInfinity;
            Empty = true;
        }

        /// <summary>
        /// Retrieves usable source data, trimming empty points and applying optional filters.
        /// </summary>
        /// <param name="minimumCount">Minimum number of points required to proceed.</param>
        /// <param name="xValues">Filtered X values (output).</param>
        /// <param name="yValues">Filtered Y values (output).</param>
        /// <param name="pointFilter">Optional predicate to keep/discard a (x,y) pair.</param>
        /// <returns>True when enough valid points are available.</returns>
        internal bool TryPrepareSourceData(
            int minimumCount,
            out List<double> xValues,
            out List<double> yValues,
            Func<double, double, bool>? pointFilter = null)
        {
            xValues = new List<double>();
            yValues = new List<double>();

            if (Series == null)
            {
                return false;
            }

            List<double>? seriesXValues = Series.GetXValues();
            var stackedTotals = (Series as StackingSeriesBase)?.GetStackedYTotals();
            var seriesYValues = stackedTotals ?? Series.GetYValues(ValueMemberPath);
            var emptyPointIndexes = stackedTotals != null
                ? Series.EmptyPointIndexes.Length > 0 ? Series.EmptyPointIndexes[0] : new List<int>()
                : Series.GetEmptyPointIndexes(ValueMemberPath);

            if (seriesXValues == null || seriesYValues == null)
            {
                return false;
            }

            int count = Math.Min(seriesXValues.Count, seriesYValues.Count);
            if (count < minimumCount)
            {
                return false;
            }

            bool removeEmptyPoints = Series.EmptyPointMode == EmptyPointMode.None &&
                                     emptyPointIndexes != null &&
                                     emptyPointIndexes.Count > 0;

            HashSet<int>? emptyIndexSet = removeEmptyPoints
                ? new HashSet<int>(emptyPointIndexes!)
                : null;

            for (int i = 0; i < count; i++)
            {
                if (removeEmptyPoints && emptyIndexSet!.Contains(i))
                {
                    continue;
                }

                double x = seriesXValues[i];
                double y = seriesYValues[i];

                if (!ChartUtils.IsFinite(x) || !ChartUtils.IsFinite(y))
                {
                    continue;
                }

                if (pointFilter != null && !pointFilter(x, y))
                {
                    continue;
                }

                xValues.Add(x);
                yValues.Add(y);
            }

            return Math.Min(xValues.Count, yValues.Count) >= minimumCount;
        }

        /// <summary>
        /// Updates X/Y min/max based on computed data points and toggles the Empty flag.
        /// </summary>
        /// <returns>True when at least two data points exist.</returns>
        internal bool UpdateBoundsFromValues()
        {
            int usableCount = Math.Min(XValues.Count, YValues.Count);

            if (usableCount < 2)
            {
                XMin = double.PositiveInfinity;
                XMax = double.NegativeInfinity;
                YMin = double.PositiveInfinity;
                YMax = double.NegativeInfinity;
                Empty = true;
                return false;
            }

            double xMin = XValues[0];
            double xMax = XValues[0];
            double yMin = YValues[0];
            double yMax = YValues[0];

            for (int i = 1; i < usableCount; i++)
            {
                double x = XValues[i];
                double y = YValues[i];

                if (x < xMin) xMin = x;
                if (x > xMax) xMax = x;
                if (y < yMin) yMin = y;
                if (y > yMax) yMax = y;
            }

            XMin = xMin;
            XMax = xMax;
            YMin = yMin;
            YMax = yMax;
            Empty = false;
            return true;
        }

        /// <summary>
        /// Transforms data coordinates into screen coordinates and updates the Empty flag.
        /// </summary>
        /// <returns>True when at least two screen points are available.</returns>
        internal bool BuildScreenCoordinates()
        {
            XCoordinates.Clear();
            YCoordinates.Clear();

            if (Series == null)
            {
                Empty = true;
                return false;
            }

            int usableCount = Math.Min(XValues.Count, YValues.Count);

            for (int i = 0; i < usableCount; i++)
            {
                float visibleX = Series.TransformToVisibleX(XValues[i], YValues[i]);
                float visibleY = Series.TransformToVisibleY(XValues[i], YValues[i]);

                if (!float.IsNaN(visibleX) && !float.IsNaN(visibleY))
                {
                    XCoordinates.Add(visibleX);
                    YCoordinates.Add(visibleY);
                }
            }

            Empty = XCoordinates.Count < 2;
            return !Empty;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Generates trackball point information for trendline intersections.
        /// This method follows the pattern of CartesianSeries.GeneratePointInfos() method.
        /// </summary>
        /// <param name="nearestPoints">The nearest trendline intersection points</param>
        /// <param name="pointInfos">The list to add trackball info to</param>
        internal void GeneratePointInfo(List<Point> nearestPoints, List<TrackballPointInfo> pointInfos)
        {
            if (!ShowTrackballLabel || Series == null || nearestPoints.Count == 0)
            {
                return;
            }

            foreach (Point intersectionPoint in nearestPoints)
            {
                float xPoint = Series.TransformToVisibleX(intersectionPoint.X, intersectionPoint.Y);
                float yPoint = Series.TransformToVisibleY(intersectionPoint.X, intersectionPoint.Y);

                string label = $"{intersectionPoint.Y:F2}";

                TrackballPointInfo? trackballInfo = Series.CreateTrackballPointInfo(xPoint, yPoint, label, intersectionPoint);

                if (trackballInfo != null)
                {
                    trackballInfo.TrendlineTrackball = this;
                    trackballInfo.TrackballTemplate = TrackballLabelTemplate;
                    trackballInfo.XValue = intersectionPoint.X;
                    trackballInfo.YValues.Add(intersectionPoint.Y);

                    int index = (int)(Series.XValues != null ? FindIndex(Series.XValues, trackballInfo.XValue) ?? -1 : -1);
                    var seriesXValues = Series.GetXValues();
                    object actualX = ((int)index >= 0 && (int)index <= seriesXValues!.Count - 1) ? Series.GetActualXValue((int)index) ?? trackballInfo.XValue : trackballInfo.XValue;
                    if (Series.ActualXAxis is DateTimeAxis && actualX is not DateTime)
                    {
                        actualX = DateTime.FromOADate((double)trackballInfo.XValue);
                    }

                    object[] dataItem = new object[2] { actualX, Math.Round(intersectionPoint.Y, 2) };

                    trackballInfo.DataItem = dataItem;
                    pointInfos.Add(trackballInfo);
                }
            }
        }

        /// <summary>
        /// Gets tooltip information for the trendline at the specified interaction point.
        /// This method provides a base implementation that derived classes can override.
        /// </summary>
        /// <param name="tooltipBehavior">The tooltip behavior that contains styling information.</param>
        /// <param name="x">The X coordinate of the interaction point in screen coordinates.</param>
        /// <param name="y">The Y coordinate of the interaction point in screen coordinates.</param>
        /// <returns>A TooltipInfo object with calculated coordinates, or null if tooltip should not be displayed.</returns>
        internal TooltipInfo? GetTooltipInfo(ChartTooltipBehavior tooltipBehavior, float x, float y)
        {
            if (!EnableTooltip || !IsVisible)
            {
                return null;
            }

            if (Series == null || Empty || XCoordinates.Count < 2)
            {
                return null;
            }

            if (!IsPointNearTrendline(x, y, out int pointIndex, tolerance: 15.0f))
            {
                return null;
            }

            if (pointIndex < 0 || pointIndex >= XValues.Count)
            {
                return null;
            }

            double dataX = XValues[pointIndex];
            double dataY = YValues[pointIndex];

            int index = (int)(Series.XValues != null ? FindIndex(Series.XValues, dataX) ?? -1 : -1);
            object actualX = (int)index >= 0 ? Series.GetActualXValue((int)index) ?? dataX : dataX;
            if (Series.ActualXAxis is DateTimeAxis && actualX is not DateTime)
            {
                actualX = DateTime.FromOADate((double)dataX);
            }

            object[] item = new object[2] { actualX, Math.Round(dataY, 2) };

            TooltipInfo tooltipInfo = new TooltipInfo(this)
            {
                X = XCoordinates[pointIndex] < 0 ? 0 : XCoordinates[pointIndex],
                Y = YCoordinates[pointIndex] < 0 ? 0 : YCoordinates[pointIndex],
                Index = pointIndex,
                Margin = tooltipBehavior.Margin,
                TextColor = tooltipBehavior.GetTooltipTextColor(),
                FontFamily = tooltipBehavior.FontFamily,
                FontSize = tooltipBehavior.GetTooltipFontSize(),
                FontAttributes = tooltipBehavior.FontAttributes,
                Background = GetTooltipBackground(GetLegendBrush(), tooltipBehavior.Background),
                Text = dataY == 0 ? dataY.ToString("0.##") : dataY.ToString("#.##"),
                Item = item,
            };
            return tooltipInfo;
        }

        /// <summary>
        /// Calculates intersection points between trackball line and trendline.
        /// This method provides a base implementation that derived classes can override.
        /// </summary>
        /// <param name="trackballX">The X coordinate of the trackball line</param>
        /// <returns>List of intersection points in data coordinates</returns>
        internal List<Point> CalculateTrackballIntersection(double trackballX)
        {
            List<Point> intersectionPoints = new List<Point>();

            if (Empty || XValues.Count == 0)
            {
                return intersectionPoints;
            }

            double tolerance = 0.5;
            double nearestDistance = double.MaxValue;
            Point? nearestPoint = null;
            int index = 0;
            foreach (double dataPoint in XValues)
            {
                double distance = Math.Abs(dataPoint - trackballX);

                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestPoint = new Point(dataPoint, YValues[index]);
                }
                index++;
            }

            if (nearestPoint.HasValue && nearestDistance <= tolerance)
            {
                intersectionPoints.Add(nearestPoint.Value);
            }

            return intersectionPoints;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="XValues"></param>
        /// <param name="targetValue"></param>
        /// <returns></returns>
        internal object FindIndex(IEnumerable XValues, double targetValue)
        {
            int index = -1;
            int i = 0;
            foreach (object item in XValues)
            {
                if (item.Equals(targetValue))
                {
                    index = i;
                    break;
                }
                i++;
            }
            return (index < 0) ? (int)targetValue : index;
        }

        internal void UpdateRange(ChartTrendline trendline)
        {
            if (Series?.ActualXAxis is DateTimeAxis || this is MovingAverageTrendline || !trendline.IsVisible)
                return;

            if (Series?.XRange == null || Series?.YRange == null || Series.VisibleXRange.IsEmpty || Series.VisibleYRange.IsEmpty)
                return;

            double currentXMin = Series.VisibleXRange.Start;
            double currentXMax = Series.VisibleXRange.End;
            double currentYMin = Series.VisibleYRange.Start;
            double currentYMax = Series.VisibleYRange.End;

            double extendedXMin = Math.Min(currentXMin, trendline.XMin);
            double extendedXMax = Math.Max(currentXMax, trendline.XMax);
            double extendedYMin = Math.Min(currentYMin, trendline.YMin);
            double extendedYMax = Math.Max(currentYMax, trendline.YMax);

            if ((trendline.ForwardForecast > 0 || trendline.BackwardForecast > 0) && trendline.XValues.Count > 1)
            {
                double originalXMin = Series.XRange.Start;
                double originalXMax = Series.XRange.End;
                double dataInterval = CalculateDataInterval(trendline.XValues);

                if (!double.IsNaN(dataInterval) && dataInterval > 0)
                {
                    bool needsBackwardExtension = trendline.BackwardForecast > 0 && extendedXMin >= originalXMin;
                    bool needsForwardExtension = trendline.ForwardForecast > 0 && extendedXMax <= originalXMax;

                    if (needsBackwardExtension)
                    {
                        double backwardXMin = originalXMin - (trendline.BackwardForecast * dataInterval);
                        DoubleRange backwardYRange = CalculateTrendlineYRangeForXRange(this, backwardXMin, originalXMin);

                        extendedXMin = Math.Min(extendedXMin, backwardXMin);
                        if (ChartUtils.IsFinite(backwardYRange.Start))
                            extendedYMin = Math.Min(extendedYMin, backwardYRange.Start);
                        if (ChartUtils.IsFinite(backwardYRange.End))
                            extendedYMax = Math.Max(extendedYMax, backwardYRange.End);
                    }

                    if (needsForwardExtension)
                    {
                        double forwardXMax = originalXMax + (trendline.ForwardForecast * dataInterval);
                        DoubleRange forwardYRange = CalculateTrendlineYRangeForXRange(this, originalXMax, forwardXMax);

                        extendedXMax = Math.Max(extendedXMax, forwardXMax);
                        if (ChartUtils.IsFinite(forwardYRange.Start))
                            extendedYMin = Math.Min(extendedYMin, forwardYRange.Start);
                        if (ChartUtils.IsFinite(forwardYRange.End))
                            extendedYMax = Math.Max(extendedYMax, forwardYRange.End);
                    }
                }
            }

            if (extendedXMin < currentXMin || extendedXMax > currentXMax)
            {
                Series.VisibleXRange = new DoubleRange(extendedXMin, extendedXMax);
            }

            if (extendedYMin < currentYMin || extendedYMax > currentYMax)
            {
                Series.VisibleYRange = new DoubleRange(extendedYMin, extendedYMax);
            }
        }

        /// <summary>
        /// Calculates the data interval between consecutive X values for forecasting calculations.
        /// </summary>
        /// <param name="xValues">The X values list</param>
        /// <returns>The calculated interval or NaN if cannot be determined</returns>
        private double CalculateDataInterval(List<double> xValues)
        {
            if (xValues == null || xValues.Count < 2)
            {
                return double.NaN;
            }

            List<double> intervals = new List<double>();
            for (int i = 1; i < xValues.Count; i++)
            {
                double interval = xValues[i] - xValues[i - 1];
                if (!double.IsNaN(interval) && interval > 0)
                {
                    intervals.Add(interval);
                }
            }

            if (intervals.Count == 0)
            {
                return double.NaN;
            }

            if (intervals.Count == 1)
            {
                return intervals[0];
            }

            double avgInterval = intervals.Average();
            bool isUniform = intervals.All(i => Math.Abs(i - avgInterval) / avgInterval <= 0.1);

            return isUniform ? avgInterval : intervals.Min();
        }

        /// <summary>
        /// Calculates the Y-value range for a given X-range using the trendline equation.
        /// </summary>
        /// <param name="trendline">The trendline to use for calculation</param>
        /// <param name="xStart">Start X value</param>
        /// <param name="xEnd">End X value</param>
        /// <returns>The calculated Y range</returns>
        private DoubleRange CalculateTrendlineYRangeForXRange(ChartTrendline trendline, double xStart, double xEnd)
        {
            if (trendline == null || double.IsNaN(xStart) || double.IsNaN(xEnd))
            {
                return new DoubleRange(double.NaN, double.NaN);
            }

            int sampleCount = Math.Max(10, (int)Math.Ceiling(Math.Abs(xEnd - xStart)));
            double step = (xEnd - xStart) / (sampleCount - 1);

            double yMin = double.MaxValue;
            double yMax = double.MinValue;

            for (int i = 0; i < sampleCount; i++)
            {
                double x = xStart + (i * step);
                double y = CalculateTrendlineYValue(trendline, x);

                if (!double.IsNaN(y) && !double.IsInfinity(y))
                {
                    yMin = Math.Min(yMin, y);
                    yMax = Math.Max(yMax, y);
                }
            }

            if (yMin == double.MaxValue || yMax == double.MinValue)
            {
                return new DoubleRange(double.NaN, double.NaN);
            }

            return new DoubleRange(yMin, yMax);
        }

        /// <summary>
        /// Calculates a Y value for a given X value using the trendline equation.
        /// This method works with all trendline types by using a common interface approach.
        /// </summary>
        /// <param name="trendline">The trendline to use</param>
        /// <param name="x">The X value</param>
        /// <returns>The calculated Y value</returns>
        private double CalculateTrendlineYValue(ChartTrendline trendline, double x)
        {
            if (trendline == null || trendline.Empty || trendline.YValues.Count == 0)
            {
                return double.NaN;
            }

            List<Point> dataPoints = trendline.XValues.Zip(trendline.YValues, (x, y) => new Point(x, y)).ToList();
            if (dataPoints != null && dataPoints.Count > 1)
            {
                return InterpolateTrendlineValue(dataPoints, x);
            }

            return double.NaN;
        }

        /// <summary>
        /// Interpolates Y value from trendline data points for given X value.
        /// This works for all trendline types since all trendlines generate data points.
        /// </summary>
        /// <param name="dataPoints">The trendline data points</param>
        /// <param name="x">The X value to interpolate</param>
        /// <returns>The interpolated Y value</returns>
        private double InterpolateTrendlineValue(IList<Point> dataPoints, double x)
        {
            if (dataPoints == null || dataPoints.Count == 0)
            {
                return double.NaN;
            }

            if (dataPoints.Count == 1)
            {
                return dataPoints[0].Y;
            }

            Point? leftPoint = null;
            Point? rightPoint = null;

            for (int i = 0; i < dataPoints.Count; i++)
            {
                Point point = dataPoints[i];

                if (point.X <= x)
                {
                    leftPoint = point;
                }

                if (point.X >= x)
                {
                    rightPoint = point;
                    break;
                }
            }

            if (!leftPoint.HasValue && rightPoint.HasValue)
            {
                return rightPoint.Value.Y;
            }

            if (leftPoint.HasValue && !rightPoint.HasValue)
            {
                return leftPoint.Value.Y;
            }

            if (!leftPoint.HasValue || !rightPoint.HasValue)
            {
                return double.NaN;
            }

            if (Math.Abs(rightPoint.Value.X - leftPoint.Value.X) < 1e-10)
            {
                return leftPoint.Value.Y;
            }

            double ratio = (x - leftPoint.Value.X) / (rightPoint.Value.X - leftPoint.Value.X);
            return leftPoint.Value.Y + ratio * (rightPoint.Value.Y - leftPoint.Value.Y);
        }

        /// <summary>
        /// Determines if the interaction point is near any individual trendline data point by calculating distance.
        /// Only returns true if the touch point is closest to a specific trendline data point within tolerance.
        /// </summary>
        /// <param name="x">The X coordinate of the interaction point in pixels.</param>
        /// <param name="y">The Y coordinate of the interaction point in pixels.</param>
        /// <param name="pointIndex">The index of the nearest trendline point, or -1 if none within tolerance.</param>
        /// <param name="tolerance">The maximum distance tolerance in pixels (default: 15.0).</param>
        /// <returns>True if the touch point is near a specific trendline data point; otherwise, false.</returns>
        bool IsPointNearTrendline(float x, float y, out int pointIndex, float tolerance = 15.0f)
        {
            pointIndex = -1;

            if (Series?.Chart == null || XCoordinates.Count == 0 || XValues.Count == 0)
            {
                return false;
            }

            var clipRect = Series.Chart.ActualSeriesClipRect;

            float touchX = (float)(x - clipRect.Left);
            float touchY = (float)(y - clipRect.Top);

            double nearestDistance = double.MaxValue;
            int nearestIndex = -1;

            for (int i = 0; i < XCoordinates.Count; i++)
            {

                double distance = Math.Sqrt(
                    Math.Pow(touchX - XCoordinates[i], 2) +
                    Math.Pow(touchY - YCoordinates[i], 2)
                );

                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestIndex = i;
                }
            }

            if (nearestIndex >= 0 && nearestDistance <= tolerance)
            {
                pointIndex = nearestIndex;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Forecast value calculation following dart's forecastValue function.
        /// This is a common utility method used by all trendline types.
        /// </summary>
        /// <param name="value">The base value to forecast from.</param>
        /// <param name="forecast">The forecast amount to add.</param>
        /// <returns>The forecasted value.</returns>
        internal double ForecastValue(double value, double forecast)
        {
            if (Series?.ActualXAxis is DateTimeAxis)
            {
                return value;
            }
            else
            {
                return value + forecast;
            }
        }

        /// <summary>
        /// Computes slope and intercept values using exact dart implementation logic.
        /// This is a common method used by Linear, Exponential, Logarithmic, and Power trendlines.
        /// </summary>
        /// <param name="xValues">The X values.</param>
        /// <param name="yValues">The Y values.</param>
        /// <param name="length">The length of the data.</param>
        /// <param name="customIntercept">Optional custom intercept value.</param>
        /// <returns>A tuple containing (slope, intercept).</returns>
        internal virtual (double slope, double intercept)
    ComputeSlopeInterceptValues(List<double> xValues, List<double> yValues, int length, double? customIntercept = null)
        {
            double xSum = 0.0;
            double ySum = 0.0;
            double xySum = 0.0;
            double xxSum = 0.0;
            int count = xValues.Count;

            for (int i = 0; i < length; i++)
            {
                double x = xValues[i];
                double y = yValues[i];

                xSum += x;
                ySum += y;
                xySum += x * y;
                xxSum += x * x;
            }

            double slope;
            double trendIntercept;

            if (customIntercept.HasValue && customIntercept.Value != 0)
            {
                trendIntercept = customIntercept.Value;
                slope = (xySum - (trendIntercept * xSum)) / xxSum;

            }
            else
            {
                slope = ((count * xySum) - (xSum * ySum)) / ((count * xxSum) - (xSum * xSum));
                trendIntercept = (ySum - (slope * xSum)) / count;
            }

            return (slope, trendIntercept);
        }

        #endregion

        #region Internal Methods

        internal Brush GetLegendBrush()
        {
            if (Stroke is SolidColorBrush solid)
            {
                float alpha = (float)Math.Clamp(Opacity, 0.0, 1.0);
                return new SolidColorBrush(solid.Color.WithAlpha(alpha));
            }

            // Fallback color if no stroke is set.
            return new SolidColorBrush(Colors.Purple.WithAlpha((float)Math.Clamp(Opacity, 0.0, 1.0)));
        }

        internal DataTemplate? GetDefaultTrendlineTooltipTemplate(TooltipInfo info)
        {
            return new DataTemplate(() =>
            {
                Label label = new Microsoft.Maui.Controls.Label
                {
                    VerticalOptions = LayoutOptions.Fill,
                    HorizontalOptions = LayoutOptions.Fill,
                    VerticalTextAlignment = TextAlignment.Center,
                    HorizontalTextAlignment = TextAlignment.Center,
                    LineBreakMode = LineBreakMode.TailTruncation
                };

                label.SetBinding(Microsoft.Maui.Controls.Label.TextProperty,
                    BindingHelper.CreateBinding(nameof(TooltipInfo.Text),
                        getter: static (TooltipInfo tooltipInfo) => tooltipInfo.Text));

                label.SetBinding(Microsoft.Maui.Controls.Label.TextColorProperty,
                    BindingHelper.CreateBinding(nameof(TooltipInfo.TextColor),
                        getter: static (TooltipInfo tooltipInfo) => tooltipInfo.TextColor));

                label.SetBinding(Microsoft.Maui.Controls.Label.MarginProperty,
                    BindingHelper.CreateBinding(nameof(TooltipInfo.Margin),
                        getter: static (TooltipInfo tooltipInfo) => tooltipInfo.Margin));

                label.SetBinding(Microsoft.Maui.Controls.Label.FontSizeProperty,
                    BindingHelper.CreateBinding(nameof(TooltipInfo.FontSize),
                        getter: static (TooltipInfo tooltipInfo) => tooltipInfo.FontSize));

                label.SetBinding(Microsoft.Maui.Controls.Label.FontFamilyProperty,
                    BindingHelper.CreateBinding(nameof(TooltipInfo.FontFamily),
                        getter: static (TooltipInfo tooltipInfo) => tooltipInfo.FontFamily));

                label.SetBinding(Microsoft.Maui.Controls.Label.FontAttributesProperty,
                    BindingHelper.CreateBinding(nameof(TooltipInfo.FontAttributes),
                        getter: static (TooltipInfo tooltipInfo) => tooltipInfo.FontAttributes));

#if NET10_0_OR_GREATER
                return label;
#else
                return new ViewCell { View = label };
#endif
            });
        }

        /// <summary>
        /// Configures the drawing surface (canvas) with stroke-related rendering options
        /// used by the chart segment, including stroke size, color (with opacity),
        /// dash pattern, and line caps/joins.
        /// </summary>
        /// <param name="canvas">
        /// The drawing canvas to apply stroke settings to. Must be a valid, non-null instance
        /// provided by the rendering pipeline.
        /// </param>
        /// <remarks>
        /// This method:
        /// - Applies a minimum stroke width of 0.1 to avoid invisible lines.
        /// - Uses the segment's <c>Stroke</c> brush if it is a <see cref="SolidColorBrush"/>; otherwise,
        ///   falls back to a default color (Purple), honoring the <c>Opacity</c>.
        /// - Applies <c>StrokeDashArray</c> as the dash pattern if present; otherwise draws a solid line.
        /// - Sets line caps and joins to rounded for smoother visuals.
        /// </remarks>
        internal void ConfigureCanvasRenderingStyle(ICanvas canvas)
        {
            // Stroke size
            canvas.StrokeSize = (float)Math.Max(0.1, StrokeWidth);

            // Stroke Color 
            if (Stroke is SolidColorBrush solidBrush)
            {
                Color color = solidBrush.Color;
                canvas.StrokeColor = color.WithAlpha((float)Math.Clamp(Opacity, 0.0, 1.0));
            }
            else
            {
                Color defaultColor = Colors.Purple;
                canvas.StrokeColor = defaultColor.WithAlpha((float)Math.Clamp(Opacity, 0.0, 1.0));
            }

            // Stroke Dash Pattern
            if (StrokeDashArray != null && StrokeDashArray.Count > 0)
            {
                float[] dashPattern = new float[StrokeDashArray.Count];
                for (int i = 0; i < StrokeDashArray.Count; i++)
                {
                    dashPattern[i] = Math.Max(0.1f, (float)StrokeDashArray[i]);
                }
                canvas.StrokeDashPattern = dashPattern;
            }
            else
            {
                canvas.StrokeDashPattern = null;
            }

            // Stroke Line / Cap
            canvas.StrokeLineCap = LineCap.Round;
            canvas.StrokeLineJoin = LineJoin.Round;
        }

        /// <summary>
        /// Draws markers on the trendline using the current marker settings.
        /// This method is called from the Draw() method when ShowMarkers is true.
        /// </summary>
        internal void DrawMarkers(ICanvas canvas)
        {
            if (!ShowMarkers || MarkerSettings == null || XCoordinates.Count == 0)
            {
                return;
            }

            if (this is IMarkerDependent markerDependent)
            {
                var markerSettings = markerDependent.MarkerSettings;
                if (markerSettings == null)
                {
                    return;
                }
                canvas.SaveState();

                if (MarkerSettings.Fill != null)
                {
                    canvas.SetFillPaint(MarkerSettings.Fill, RectF.Zero);
                }
                else
                {
                    if (Stroke is SolidColorBrush solidBrush)
                    {
                        canvas.FillColor = solidBrush.Color;
                    }
                    else
                    {
                        canvas.FillColor = Colors.Purple;
                    }
                }

                if (MarkerSettings.HasBorder)
                {
                    canvas.StrokeSize = (float)MarkerSettings.StrokeWidth;
                    if (MarkerSettings.Stroke is SolidColorBrush strokeBrush)
                    {
                        canvas.StrokeColor = strokeBrush.Color;
                    }
                    else
                    {
                        canvas.StrokeColor = Colors.Purple;
                    }
                }

                float animationValue = 1.0f;
                if (Series?.EnableAnimation == true)
                {
                    animationValue = Series.AnimationValue;
                }

                for (int i = 0; i < XCoordinates.Count; i++)
                {
                    double width = MarkerSettings.Width;
                    double height = MarkerSettings.Height;

                    if (width > 0 && height > 0)
                    {
                        Rect markerRect = new Rect(
                            XCoordinates[i] - (width / 2),
                            YCoordinates[i] - (height / 2),
                            width,
                            height
                        );

                        canvas.Alpha = animationValue;
                        canvas.DrawShape(markerRect, shapeType: MarkerSettings.Type, hasBorder: markerDependent.MarkerSettings?.HasBorder == true, false);
                    }
                }
                canvas.RestoreState();
            }
        }

        internal void LegendItemToggled(LegendItem legendItem)
        {
            if (Series is CartesianSeries cartesianSeries && !cartesianSeries.IsVisible)
            {
                legendItem.IsToggled = !IsVisible;
                return;
            }

            IsVisible = !legendItem.IsToggled;
        }

        /// <summary>
        /// To update the toggle state of the respective trendline legend.
        /// </summary>
        void UpdateLegendItemToggle()
        {
            ChartLegend? legend = Series?.Chart?.Legend;
            var legendItems = Series?.ChartArea?.PlotArea.LegendItems;

            if (legend != null && legend.IsVisible && legendItems != null)
            {
                foreach (LegendItem legendItem in legendItems)
                {
                    if (legendItem != null && legendItem.Item == this)
                    {
                        legendItem.IsToggled = !IsVisible;
                        break;
                    }
                }
            }
        }

        internal void UpdateLegendIconColor()
        {
            var legendItems = Series?.ChartArea?.PlotArea.LegendItems;
            if (legendItems == null)
            {
                return;
            }

            foreach (var item in legendItems)
            {
                if (item is LegendItem legendItem && legendItem.Item == this)
                {
                    legendItem.IconBrush = GetLegendBrush();
                    break;
                }
            }
        }

        internal void UpdateLegendLabel()
        {
            var legendItems = Series?.ChartArea?.PlotArea.LegendItems;
            if (legendItems == null)
            {
                return;
            }

            foreach (var item in legendItems)
            {
                if (item is LegendItem legendItem && legendItem.Item == this)
                {
                    legendItem.Text = GetLegendText();
                    break;
                }
            }
        }

        internal string GetLegendText()
        {
            if (!string.IsNullOrWhiteSpace(Label))
            {
                return Label;
            }

            return string.Empty; ;
        }

        /// <summary>
        /// Invalidates the charts trendline layer and schedules a redraw so recent trendline changes are rendered.
        /// </summary>
        internal void InvalidateTrendlines()
        {
            if (Series != null)
            {
                IPlotArea? plotArea = Series.Chart?.Area.PlotArea;

                if (plotArea != null && plotArea is CartesianPlotArea cartesianPlotArea)
                {
                    ChartTrendlineView chartTrendlineView = cartesianPlotArea._chartTrendlineView;
                    chartTrendlineView?.InvalidateDrawable();
                }
            }
        }

        /// <summary>
        /// Gets the appropriate tooltip background brush based on the UseSeriesFillColor property and explicit Background setting.
        /// </summary>
        /// <param name="seriesFill">The fill brush of the associated series.</param>
        /// <param name="tooltipBackground"></param>
        /// <returns>The brush to use as the tooltip background.</returns>
        private Brush? GetTooltipBackground(Brush? seriesFill, Brush? tooltipBackground)
        {
            // If UseSeriesFillColor is true and we have a series fill, derive the background from it
            if (UseSeriesFillColor && seriesFill != null)
            {
                // For gradient brushes, extract the primary (first) color as a solid brush
                if (seriesFill is GradientBrush gradient && gradient.GradientStops != null && gradient.GradientStops.Count > 0)
                {
                    Color stopColor = gradient.GradientStops[0].Color;
                    if (stopColor != Colors.Transparent)
                    {
                        return new SolidColorBrush(stopColor);
                    }
                }
                else
                {
                    // For solid brushes, use directly if not transparent
                    Color brushColor = seriesFill.ToColor();
                    if (brushColor != Colors.Transparent)
                    {
                        return seriesFill;
                    }
                }
            }

            // Fall back to the default Background
            return tooltipBackground;
        }


        #endregion

        #region Property Changed Callbacks

        private static void OnStrokePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ChartTrendline trendline)
            {
                trendline.InvalidateTrendlines();
                trendline.UpdateLegendIconColor();
            }
        }

        private static void OnStrokeWidthPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ChartTrendline trendline)
            {
                trendline.InvalidateTrendlines();
            }

        }

        private static void OnStrokeDashArrayPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ChartTrendline trendline)
            {
                trendline.InvalidateTrendlines();
            }
        }

        private static void OnValueMemberFieldPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ChartTrendline trendline)
            {
                trendline.MarkForRefresh();
            }

        }

        private static void OnOpacityPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ChartTrendline trendline)
            {
                trendline.InvalidateTrendlines();
                trendline.UpdateLegendIconColor();
            }
        }

        private static void OnIsVisiblePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ChartTrendline trendline)
            {
                if (!(bool)newValue)
                {
                    trendline.Series?.Chart?.ResetTooltip();
                }

                trendline.InvalidateTrendlines();
                trendline.ScheduleUpdate();
                trendline.UpdateLegendItemToggle();
            }
        }

        private static void OnLabelPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ChartTrendline trendline)
            {
                trendline.UpdateLegendLabel();
                trendline.InvalidateTrendlines();
            }
        }

        static void OnForwardForecastPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ChartTrendline trendline)
            {
                trendline.MarkForRefresh();
                trendline.ScheduleUpdate();
            }
        }

        static void OnBackwardForecastPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ChartTrendline trendline)
            {
                trendline.MarkForRefresh();
                trendline.ScheduleUpdate();
            }
        }

        internal void ScheduleUpdate()
        {
            if (Series != null)
            {
                Series.ScheduleUpdateChart();
            }
        }

        #endregion

        #region INotifyPropertyChanged Implementation

        void ITooltipDependent.SetTooltipTargetRect(TooltipInfo tooltipInfo, Rect chartBounds)
        {
            if (Series?.ChartArea == null)
            {
                return;
            }

            float xPosition = tooltipInfo.X;
            float yPosition = tooltipInfo.Y;
            float sizeValue = 1;
            float halfSizeValue = 0.5f;

            Rect targetRect = tooltipInfo.TargetRect.IsEmpty ? new Rect(xPosition - halfSizeValue, yPosition, sizeValue, sizeValue) : tooltipInfo.TargetRect;

            if (tooltipInfo.TargetRect.IsEmpty)
            {
                if ((xPosition + chartBounds.Left) == chartBounds.Left)
                {
                    targetRect = new Rect(xPosition - sizeValue, yPosition - halfSizeValue, sizeValue, sizeValue);
                    tooltipInfo.Position = Core.TooltipPosition.Right;
                }
                else if (xPosition == chartBounds.Width)
                {
                    targetRect = new Rect(xPosition - sizeValue, yPosition - halfSizeValue, sizeValue, sizeValue);
                    tooltipInfo.Position = Core.TooltipPosition.Left;
                }
                else if (yPosition == chartBounds.Top)
                {
                    targetRect = new Rect(xPosition - halfSizeValue, -sizeValue, sizeValue, sizeValue);
                    tooltipInfo.Position = Core.TooltipPosition.Bottom;
                }

                if (Series.ChartArea.IsTransposed)
                {
                    float width = 2;
                    float height = 2;
                    targetRect = new Rect(xPosition - width, yPosition - height / 2, width, height);
                }
            }
            else
            {
                var markerToolTip = tooltipInfo.TargetRect;

                if ((markerToolTip.X + markerToolTip.Width / 2 + chartBounds.Left) == chartBounds.Left)
                {
                    targetRect = new Rect(markerToolTip.X + markerToolTip.Width / 2, markerToolTip.Y, markerToolTip.Width / 2, markerToolTip.Height);
                    tooltipInfo.Position = Core.TooltipPosition.Right;
                }
                else if ((markerToolTip.X + markerToolTip.Width / 2) == chartBounds.Width)
                {
                    tooltipInfo.Position = Core.TooltipPosition.Left;
                }
            }

            tooltipInfo.TargetRect = targetRect;
        }

        #endregion

        #region Shared Cubic Curve Drawing Methods

        /// <summary>
        /// Draws a smooth cubic Bezier curve through all trendline points.
        /// </summary>
        internal void DrawCubicCurveToCanvas(ICanvas canvas)
        {
            if (XCoordinates.Count < 2) return;

            PathF path = new PathF();
            path.MoveTo(XCoordinates[0], YCoordinates[0]);

            DrawCubicCurve(path);
            canvas.DrawPath(path);
        }

        /// <summary>
        /// Draws trendline as cubic bezier curve exactly like dart implementation.
        /// This method follows the dart's _computeCubicPath() logic for smooth trendlines.
        /// </summary>
        /// <param name="path">The path to draw the cubic curve on.</param>
        internal void DrawCubicCurve(PathF path)
        {
            if (XCoordinates.Count < 2 || XValues.Count < 2)
            {
                return;
            }

            for (int i = 0; i < XCoordinates.Count - 1; i++)
            {
                var nextPoint = new Point(XCoordinates[i + 1], YCoordinates[i + 1]);

                var controlPoints = ComputeCubicControlPoints(i);

                if (controlPoints != null && controlPoints.Count >= 4)
                {
                    var series = Series as CartesianSeries;
                    if (series != null)
                    {
                        float controlX1 = series.TransformToVisibleX(controlPoints[0], controlPoints[1]);
                        float controlY1 = series.TransformToVisibleY(controlPoints[0], controlPoints[1]);
                        float controlX2 = series.TransformToVisibleX(controlPoints[2], controlPoints[3]);
                        float controlY2 = series.TransformToVisibleY(controlPoints[2], controlPoints[3]);

                        path.CurveTo(controlX1, controlY1, controlX2, controlY2, (float)nextPoint.X, (float)nextPoint.Y);
                    }
                }
                else
                {
                    path.LineTo((float)nextPoint.X, (float)nextPoint.Y);
                }
            }
        }

        /// <summary>
        /// Computes cubic bezier control points for trendline following dart implementation.
        /// This method replicates the dart's _computeControlPoints logic.
        /// </summary>
        /// <param name="index">The index of the current point.</param>
        /// <returns>A list containing [controlX1, controlY1, controlX2, controlY2] or null if calculation fails.</returns>
        internal List<double>? ComputeCubicControlPoints(int index)
        {
            if (index < 0 || index >= XValues.Count - 1)
            {
                return null;
            }

            var xValues = XValues;
            var yValues = YValues;

            int length = xValues.Count;

            var yCoefficients = ComputeNaturalSplineCoefficients(xValues, yValues, length);

            if (yCoefficients != null && index < yCoefficients.Count - 1)
            {
                return CalculateControlPointsFromCoefficients(xValues, yValues, yCoefficients[index], yCoefficients[index + 1], index);
            }
            return null;
        }

        /// <summary>
        /// Computes natural spline coefficients following dart's _computeNaturalSpline method.
        /// </summary>
        /// <param name="xValues">X coordinate values.</param>
        /// <param name="yValues">Y coordinate values.</param>
        /// <param name="length">Length of the data points.</param>
        /// <returns>List of Y coefficients for spline calculation.</returns>
        internal List<double>? ComputeNaturalSplineCoefficients(List<double> xValues, List<double> yValues, int length)
        {
            if (length < 2)
            {
                return null;
            }

            const double a = 6;
            var yCoefficient = new List<double>(new double[length]);
            var u = new List<double?>(new double?[length]);

            yCoefficient[0] = 0;
            u[0] = 0;
            yCoefficient[length - 1] = 0;

            for (int i = 1; i < length - 1; i++)
            {
                if (!double.IsNaN(yValues[i + 1]) && !double.IsNaN(yValues[i - 1]) && !double.IsNaN(yValues[i]))
                {
                    double d1 = xValues[i] - xValues[i - 1];
                    double d2 = xValues[i + 1] - xValues[i - 1];
                    double d3 = xValues[i + 1] - xValues[i];
                    double dy1 = yValues[i + 1] - yValues[i];
                    double dy2 = yValues[i] - yValues[i - 1];

                    if (Math.Abs(xValues[i] - xValues[i - 1]) < 1e-10 || Math.Abs(xValues[i] - xValues[i + 1]) < 1e-10)
                    {
                        yCoefficient[i] = 0;
                        u[i] = 0;
                    }
                    else
                    {
                        double p = 1.0 / ((d1 * yCoefficient[i - 1]) + (2 * d2));
                        yCoefficient[i] = -p * d3;
                        if (u[i - 1].HasValue)
                        {
                            u[i] = p * ((a * ((dy1 / d3) - (dy2 / d1))) - (d1 * u[i - 1]!.Value));
                        }
                    }
                }
            }

            for (int k = length - 2; k >= 0; k--)
            {
                if (u[k].HasValue)
                {
                    yCoefficient[k] = (yCoefficient[k] * yCoefficient[k + 1]) + u[k]!.Value;
                }
            }

            return yCoefficient;
        }

        /// <summary>
        /// Calculates control points from spline coefficients following dart's _controlPoints method.
        /// </summary>
        /// <param name="xValues">X coordinate values.</param>
        /// <param name="yValues">Y coordinate values.</param>
        /// <param name="yCoefficient">Current Y coefficient.</param>
        /// <param name="nextYCoefficient">Next Y coefficient.</param>
        /// <param name="i">Current index.</param>
        /// <returns>List containing [controlX1, controlY1, controlX2, controlY2].</returns>
        internal List<double> CalculateControlPointsFromCoefficients(
            List<double> xValues, List<double> yValues,
            double yCoefficient, double nextYCoefficient, int i)
        {
            var values = new List<double>(4);
            double x = xValues[i];
            double y = yValues[i];
            double nextX = xValues[i + 1];
            double nextY = yValues[i + 1];
            const double oneThird = 1.0 / 3.0;

            double deltaX2 = nextX - x;
            deltaX2 = deltaX2 * deltaX2;

            double dx1 = (2 * x) + nextX;
            double dx2 = x + (2 * nextX);
            double dy1 = (2 * y) + nextY;
            double dy2 = y + (2 * nextY);

            double y1 = oneThird * (dy1 - (oneThird * deltaX2 * (yCoefficient + (0.5 * nextYCoefficient))));
            double y2 = oneThird * (dy2 - (oneThird * deltaX2 * ((0.5 * yCoefficient) + nextYCoefficient)));

            values.Add(dx1 * oneThird); values.Add(y1); values.Add(dx2 * oneThird); values.Add(y2);
            return values;
        }

        /// <summary>
        /// Calculates linear regression coefficients with optional value transformations.
        /// </summary>
        internal (double slope, double intercept) ComputeTransformedRegression(
            List<double> xValues,
            List<double> yValues,
            int length,
            Func<double, double> xTransform,
            Func<double, double> yTransform,
            Func<double, double> interceptTransform,
            double? customIntercept = null)
        {
            double xSum = 0.0, ySum = 0.0, xySum = 0.0, xxSum = 0.0;

            for (int i = 0; i < length; i++)
            {
                double x = xTransform(xValues[i]);
                double y = yTransform(yValues[i]);

                if (ChartUtils.IsFinite(y))
                {
                    xSum += x;
                    ySum += y;
                    xySum += x * y;
                    xxSum += x * x;
                }
            }

            int count = xValues.Count;
            double slope, intercept;

            if (customIntercept.HasValue && customIntercept.Value != 0)
            {
                intercept = customIntercept.Value;
                slope = (xySum - (Math.Log(Math.Abs(intercept)) * xSum)) / xxSum;
            }
            else
            {
                slope = ((count * xySum) - (xSum * ySum)) / ((count * xxSum) - (xSum * xSum));
                double rawIntercept = (ySum - (slope * xSum)) / count;
                intercept = interceptTransform(rawIntercept);
            }

            return (slope, intercept);
        }


        void IMarkerDependent.InvalidateDrawable()
        {
            InvalidateTrendlines();
        }

        ChartMarkerSettings IMarkerDependent.MarkerSettings => MarkerSettings ?? new ChartMarkerSettings();

        bool needToAnimateMarker;

        bool IMarkerDependent.NeedToAnimateMarker { get => needToAnimateMarker; set => needToAnimateMarker = false; }

        void IMarkerDependent.DrawMarker(ICanvas canvas, int index, ShapeType type, Rect rect) => this.DrawMarkers(canvas);

		#endregion

		#endregion

	}
}
