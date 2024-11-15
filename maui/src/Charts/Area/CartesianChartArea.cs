using Syncfusion.Maui.Toolkit.Graphics.Internals;
using Syncfusion.Maui.Toolkit.Internals;
using System.Collections.ObjectModel;

namespace Syncfusion.Maui.Toolkit.Charts
{
    /// <summary>
    /// Helps to define and manage the area in a Cartesian chart where data is plotted, including gridlines, axes, and layout customization for Cartesian-based data series.
    /// </summary>
    internal partial class CartesianChartArea : AreaBase, ICartesianChartArea
    {
        #region Private Fields
        readonly CartesianPlotArea _cartesianPlotArea;
        readonly AbsoluteLayout _behaviorLayout;
        bool _isSbsWithOneData = false;
        bool _isTransposed = false;
        bool _enableSideBySideSeriesPlacement = true;
        Dictionary<string, List<StackingSeriesBase>>? _seriesGroup;
        RectF _actualSeriesClipRect;
        ChartSeriesCollection? _series;
        readonly Element _sourceParent;
        #endregion

        #region Internal Properties
        internal readonly AnnotationLayout AnnotationLayout;
        internal readonly CartesianAxisLayout AxisLayout;
        internal readonly ObservableCollection<ChartAxis> XAxes;
        internal readonly ObservableCollection<RangeAxisBase> YAxes;

        internal Thickness PlotAreaMargin { get; set; } = Thickness.Zero;

        #region Chart Properties

        /// <summary>
        /// Boolean used to clear associated axis and register series. 
        /// </summary>
        internal bool RequiredAxisReset { get; set; } = true;

        internal IList<Brush>? PaletteColors { get; set; }

        internal Rect SeriesClipRect { get; set; }

        internal RectF ActualSeriesClipRect { get { return _actualSeriesClipRect; } set { _actualSeriesClipRect = value; } }

        #endregion

        #region Public Properties

        public ChartAxis? PrimaryAxis { get; set; }

        public RangeAxisBase? SecondaryAxis { get; set; }

        public ChartSeriesCollection? Series
        {
            get
            {
                return _series;
            }
            set
            {
                if (value != _series)
                {
                    _series = value;
                    _cartesianPlotArea.Series = value;
                }
            }
        }

        public ReadOnlyObservableCollection<ChartSeries>? VisibleSeries => Series?.GetVisibleSeries();

        public override IPlotArea PlotArea => _cartesianPlotArea;
        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public CartesianChartArea(SfCartesianChart chart)
        {
            BatchBegin();
            XAxes = chart.XAxes;
            XAxes.CollectionChanged += XAxes_CollectionChanged;
            YAxes = chart.YAxes;
            YAxes.CollectionChanged += YAxes_CollectionChanged;
            PaletteColors = ChartColorModel.DefaultBrushes;
            _cartesianPlotArea = new CartesianPlotArea(this);
            _cartesianPlotArea.Chart = chart;
            _sourceParent = chart;
            AxisLayout = new CartesianAxisLayout(this);
            AnnotationLayout = new AnnotationLayout(chart);
            _behaviorLayout = chart.BehaviorLayout;
            AbsoluteLayout.SetLayoutBounds(AxisLayout, new Rect(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(AxisLayout, Microsoft.Maui.Layouts.AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(_cartesianPlotArea, new Rect(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(_cartesianPlotArea, Microsoft.Maui.Layouts.AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(AnnotationLayout, new Rect(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(AnnotationLayout, Microsoft.Maui.Layouts.AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(_behaviorLayout, new Rect(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(_behaviorLayout, Microsoft.Maui.Layouts.AbsoluteLayoutFlags.All);
            Add(_cartesianPlotArea);
            Add(AxisLayout);
            AxisLayout.InputTransparent = true;
            Add(AnnotationLayout);
            Add(_behaviorLayout);
            BatchCommit();
        }

        #endregion

        #region Methods

        #region Protected Methods

        protected override void UpdateAreaCore()
        {
            if (_cartesianPlotArea.Chart is not IChart cartesianChart)
                return;

            //Hide tooltip if any update happen in chart area.
            cartesianChart.ResetTooltip();

            if (XAxes.Count <= 0 || YAxes.Count <= 0)
                return;

            AxisLayout.AssignAxisToSeries();
            AxisLayout.LayoutAxis(AreaBounds);

            _cartesianPlotArea.Margin = PlotAreaMargin;
            _behaviorLayout.Margin = PlotAreaMargin;

            //Size and position of the plot area, subtracting title and legend size.
            cartesianChart.ActualSeriesClipRect = ChartUtils.GetSeriesClipRect(
                AreaBounds.SubtractThickness(PlotAreaMargin),
                _cartesianPlotArea.Chart.TitleHeight);

            UpdateVisibleSeries(); //series create segment logics.

            if (cartesianChart is SfCartesianChart sfChart)
            {
                //Casting required as annotation only support for cartesian chart. 
                sfChart.UpdateAnnotationLayout();
            }

            //Invalidate views.
            AnnotationLayout.InvalidateDrawable();
            AxisLayout.InvalidateRender();
            _cartesianPlotArea.InvalidateRender();
        }

        public void UpdateVisibleSeries()
        {
            _cartesianPlotArea.UpdateVisibleSeries();
        }

        #endregion

        internal void OnPaletteColorChanged()
        {
            if (Series?.Count > 0)
            {
                foreach (var series in Series)
                {
                    if (series is CartesianSeries cartesian && cartesian.Chart != null)
                    {
                        series.UpdateColor();
                        series.InvalidateSeries();

                        if (series.ShowDataLabels)
                        {
                            series.InvalidateDataLabel();
                        }

                        series.UpdateLegendIconColor();
                    }
                }
            }
        }

        #region Axes Collection Changed
        void XAxes_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            e.ApplyCollectionChanges(
                (obj, index, canInsert) => AddAxes(obj),
                (obj, index) => RemoveAxes(obj),
                ResetAxes
                );

            if (sender is ObservableCollection<ChartAxis> axes && axes.Count > 0)
            {
                PrimaryAxis = axes[0];
            }

            // Mark for relayout and schedule an update of the chart area
            NeedsRelayout = true;
            ScheduleUpdateArea();
        }

        void YAxes_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            e.ApplyCollectionChanges(
                (obj, index, canInsert) => AddAxes(obj),
                (obj, index) => RemoveAxes(obj),
                ResetAxes
                );

            if (sender is ObservableCollection<RangeAxisBase> axes && axes.Count > 0)
            {
                SecondaryAxis = axes[0];
            }

            // Mark for relayout and schedule an update of the chart area
            NeedsRelayout = true;
            ScheduleUpdateArea();
        }

        void AddAxes(object obj)
        {
            if (obj is ChartAxis axis)
            {
                axis.Parent = _sourceParent;
                axis.Area = this;
                RequiredAxisReset = true;
                SetInheritedBindingContext(axis, BindingContext);
            }
        }

        void RemoveAxes(object obj)
        {
            if (obj is ChartAxis axis)
            {
                axis.Parent = null;
                RequiredAxisReset = true;
                axis.Area = null;
                SetInheritedBindingContext(axis, null);
                //TODO:Need to unhook if any event hooked.
            }
        }

        void ResetAxes()
        {
            //Axes not be reset.
        }
        #endregion

        #endregion

        #region Destructor

        ~CartesianChartArea()
        {
            XAxes.CollectionChanged -= XAxes_CollectionChanged;
            YAxes.CollectionChanged -= YAxes_CollectionChanged;
        }

        #endregion
    }
}
