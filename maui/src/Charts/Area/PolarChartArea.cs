using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Layouts;
using Syncfusion.Maui.Toolkit.Graphics.Internals;
using Syncfusion.Maui.Toolkit.Internals;

namespace Syncfusion.Maui.Toolkit.Charts
{
	internal class PolarChartArea : AreaBase, IChartPlotArea
    {
        #region Fields
        readonly AbsoluteLayout _behaviorLayout;
        readonly AbsoluteLayout _dataLabelLayout;
        readonly PolarGridLineLayout _gridLineLayout;
        readonly ObservableCollection<ILegendItem> _legendItems;
        readonly AbsoluteLayout _seriesViews;
        readonly PolarDataLabelView _dataLabelView;
        readonly PolarAxisLayoutView _axisLayout;

        bool _shouldPopulateLegendItems = true;
        View? _plotAreaBackgroundView;
        RectF _actualSeriesClipRect;
        IChartLegend? _legend;
        ChartPolarSeriesCollection? _series;

        IChart chart => PolarChart;
        #endregion

        #region Public Properties
        public override IPlotArea PlotArea => this;

        public ChartPolarSeriesCollection? Series
        {
            get
            {
                return _series;
            }
            set
            {
                if (value != _series)
                {
                    OnSeriesCollectionChanging();
                    _series = value;
                    OnSeriesCollectionChanged();
                }
            }
        }

        public ReadOnlyObservableCollection<ChartSeries>? VisibleSeries => Series?.GetVisibleSeries();

        #endregion

        #region Internal Properties

        internal readonly SfPolarChart PolarChart;
        internal RectF ActualSeriesClipRect { get { return _actualSeriesClipRect; } set { _actualSeriesClipRect = value; } }
        internal Thickness PlotAreaMargin { get; set; } = Thickness.Zero;
        internal IList<Brush>? PaletteColors { get; set; }
        internal View? PlotAreaBackgroundView
        {
            get
            {
                return _plotAreaBackgroundView;
            }
            set
            {
                if (_plotAreaBackgroundView != null && Contains(_plotAreaBackgroundView))
                {
                    _plotAreaBackgroundView.RemoveBinding(AbsoluteLayout.LayoutBoundsProperty);
                    _plotAreaBackgroundView.RemoveBinding(AbsoluteLayout.LayoutFlagsProperty);
                    Remove(_plotAreaBackgroundView);
                }

                if (value != null)
                {
                    _plotAreaBackgroundView = value;
                    AbsoluteLayout.SetLayoutBounds(_plotAreaBackgroundView, new Rect(0, 0, 1, 1));
                    AbsoluteLayout.SetLayoutFlags(_plotAreaBackgroundView, AbsoluteLayoutFlags.All);
                    Insert(0, _plotAreaBackgroundView);
                }
            }
        }
        internal PointF PolarAxisCenter { get; set; }

        #endregion

        #region Interface Implementation

        ILegend? IPlotArea.Legend
        {
            get
            {
                return _legend;
            }
            set
            {
                if (value != _legend && value is IChartLegend chartLegend)
                {
                    _legend = chartLegend;
                }
            }
        }

        public ReadOnlyObservableCollection<ILegendItem> LegendItems => new ReadOnlyObservableCollection<ILegendItem>(_legendItems);
        EventHandler<EventArgs>? _legendItemsUpdated;
        EventHandler<LegendItemEventArgs>? _legendItemsToggled;
        event EventHandler<EventArgs> IPlotArea.LegendItemsUpdated { add { _legendItemsUpdated += value; } remove { _legendItemsUpdated -= value; } }
        event EventHandler<LegendItemEventArgs>? IPlotArea.LegendItemToggled { add { _legendItemsToggled += value; } remove { _legendItemsToggled -= value; } }
        public Rect PlotAreaBounds { get; set; }
        public bool ShouldPopulateLegendItems { get => _shouldPopulateLegendItems; set => _shouldPopulateLegendItems = value; }

        LegendHandler IPlotArea.LegendItemToggleHandler
        {
            get
            {
                return ToggleLegendItem;
            }
        }

        SfDrawableView IChartPlotArea.DataLabelView => _dataLabelView;

        #endregion

        #region Constructor

        /// <summary>
        /// It helps to create chart area to hold the view for polar charts.
        /// </summary>
        /// <param name="polarChart"></param>
        public PolarChartArea(SfPolarChart polarChart)
        {
            PolarChart = polarChart;
            _series = new ChartPolarSeriesCollection();
            _legendItems = new ObservableCollection<ILegendItem>();
            PaletteColors = ChartColorModel.DefaultBrushes;
            _gridLineLayout = new PolarGridLineLayout(this);
            _seriesViews = new AbsoluteLayout();
            _axisLayout = new PolarAxisLayoutView(this);
            _dataLabelView = new PolarDataLabelView(this);
            _behaviorLayout = polarChart.BehaviorLayout;
            _dataLabelLayout = new AbsoluteLayout();
            AbsoluteLayout.SetLayoutBounds(_gridLineLayout, new Rect(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(_gridLineLayout, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(_seriesViews, new Rect(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(_seriesViews, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(_axisLayout, new Rect(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(_axisLayout, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(_dataLabelView, new Rect(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(_dataLabelView, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(_behaviorLayout, new Rect(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(_behaviorLayout, Microsoft.Maui.Layouts.AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(_dataLabelLayout, new Rect(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(_dataLabelLayout, AbsoluteLayoutFlags.All);
            Add(_gridLineLayout);
            Add(_seriesViews);
            Add(_dataLabelView);
            Add(_axisLayout);
            Add(_behaviorLayout);
            Add(_dataLabelLayout);
        }

        #endregion

        #region Methods

        #region Public Method

        public void UpdateLegendItems()
        {
            if (_shouldPopulateLegendItems)
            {
                UpdateLegendItemsSource();
                _shouldPopulateLegendItems = false;
                _legendItemsUpdated?.Invoke(this, EventArgs.Empty);
            }
        }

        #endregion

        #region Internal Methods

        internal ChartAxis GetPrimaryAxis()
        {
            return PolarChart.PrimaryAxis;
        }

        internal ChartAxis GetSecondaryAxis()
        {
            return PolarChart.SecondaryAxis;
        }

        internal void InternalCreateSegments(ChartSeries series)
        {
            foreach (var view in _seriesViews.Children)
            {
                if (view is SeriesView seriesView && seriesView.IsVisible && series == seriesView.Series)
                {
                    seriesView.InternalCreateSegments();
                }
            }
        }

        internal void OnPaletteColorChanged()
        {
            if (Series != null)
            {
                foreach (var series in Series)
                {
                    if (series is PolarSeries polar && polar.Chart != null)
                    {
                        series.PaletteColorsChanged();
                    }
                }
            }
        }

        internal void ScheduleUpdate(bool reLayout)
        {
            NeedsRelayout = reLayout;
            ScheduleUpdateArea();
        }

        internal PointF PolarAngleToPoint(ChartAxis axis, float radius, float theta)
        {
            PointF point = new PointF();
            theta = axis.IsInversed ? -theta : theta;
            var angle = PolarChart.PolarStartAngle;
            point.X = (float)(PolarAxisCenter.X + (radius * Math.Cos((theta + angle) * (Math.PI / 180))));
            point.Y = (float)(PolarAxisCenter.Y + (radius * Math.Sin((theta + angle) * (Math.PI / 180))));
            return point;
        }

        #endregion

        #region Protected Methods

        protected override void UpdateAreaCore()
        {
            if (chart == null) return;

            chart.ResetTooltip();

            _axisLayout.AssignAxisToSeries();
            _axisLayout.LayoutAxis(AreaBounds);
            _gridLineLayout.Measure(ActualSeriesClipRect.Size);

            chart.ActualSeriesClipRect = ChartUtils.GetSeriesClipRect(AreaBounds, chart.TitleHeight);

            UpdateVisibleSeries();
            _axisLayout.InvalidateDrawable();
            InvalidateGridLayout();
        }

        protected void UpdateLegendItemsSource()
        {
            if (Series == null || _legend == null || !_legend.IsVisible)
            {
                return;
            }

            _legendItems.Clear();
            int index = 0;
            foreach (PolarSeries series in Series)
            {
                if (series.IsVisibleOnLegend)
                {
                    var legendItem = new LegendItem()
                    {
                        IconType = ChartUtils.GetShapeType(series.LegendIcon),
                        Item = series,
                        Source = series,
                        Text = series.Label,
                        Index = index,
                        IsToggled = !series.IsVisible
                    };

                    Brush? solidColor = series.GetFillColor(legendItem, index);
                    legendItem.IconBrush = solidColor ?? new SolidColorBrush(Colors.Transparent);

                    ((IChartPlotArea)this).UpdateLegendLabelStyle(legendItem, _legend.LabelStyle ?? chart?.LegendLabelStyle);
                    _legend.OnLegendItemCreated(legendItem);
                    _legendItems?.Add(legendItem);
                    index++;
                }
            }
        }

        #endregion

        #region Private Methods

        void ToggleLegendItem(ILegendItem legendItem)
        {
            if (_legend?.IsVisible == true && _legend.ToggleVisibility && legendItem is LegendItem polarLegendItem && polarLegendItem.Item is PolarSeries series)
            {
                series.IsVisible = !legendItem.IsToggled;
            }
        }

        void UpdateVisibleSeries()
        {
            foreach (SeriesView seriesView in _seriesViews.Children)
            {
                if (seriesView != null && seriesView.IsVisible)
                {
                    seriesView.UpdateSeries();
                }
            }

            _dataLabelView?.InvalidateDrawable();
        }

        void AddSeries(int index, object series)
        {
            var chartSeries = series as PolarSeries;
            if (chartSeries != null)
            {
                chartSeries.Chart = PolarChart;
                chartSeries.Parent = PolarChart;
                SetInheritedBindingContext(chartSeries, BindingContext);
                chartSeries.ChartArea = this;
                chartSeries.SegmentsCreated = false;
				var seriesView = new SeriesView(chartSeries, this);
                chartSeries.NeedToAnimateSeries = chartSeries.EnableAnimation;
                AbsoluteLayout.SetLayoutBounds(seriesView, new Rect(0, 0, 1, 1));
                AbsoluteLayout.SetLayoutFlags(seriesView, AbsoluteLayoutFlags.All);
                _seriesViews.Insert(index, seriesView);
                var labelView = chartSeries.LabelTemplateView;
                AbsoluteLayout.SetLayoutBounds(labelView, new Rect(0, 0, 1, 1));
                AbsoluteLayout.SetLayoutFlags(labelView, AbsoluteLayoutFlags.All);
                _dataLabelLayout.Insert(index, labelView);
            }
        }

        void RemoveSeries(int index, object series)
        {
            var chartSeries = series as ChartSeries;
            if (chartSeries != null)
            {
                chartSeries.ResetData();
                chartSeries.SegmentsCreated = false;
                SetInheritedBindingContext(chartSeries, null);
                chart.IsRequiredDataLabelsMeasure = true;
                _dataLabelLayout.Children.RemoveAt(index);
                _seriesViews.Children.RemoveAt(index);
                chartSeries.Chart = null;
                chartSeries.Parent = null;
            }
        }

        void ResetSeries()
        {
            foreach (SeriesView seriesView in _seriesViews)
            {
                seriesView.Series.ResetData();
            }

            _dataLabelLayout.Clear();
            _seriesViews.Clear();
        }

        void OnSeriesCollectionChanging()
        {
            if (_series != null)
            {
                _series.CollectionChanged -= Series_CollectionChanged;
                ResetSeries();
            }
        }

        void OnSeriesCollectionChanged()
        {
            if (_series != null)
            {
                _series.CollectionChanged += Series_CollectionChanged;

                int count = 0;
                foreach (var chartSeries in _series)
                {
                    AddSeries(count, chartSeries);
                    count++;
                }

                if (_legend != null)
                {
                    UpdateLegendItems();
                }

                ScheduleUpdate(true);
            }
        }

        void Series_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            e.ApplyCollectionChanges(
                (obj, index, canInsert) => AddSeries(index, obj),
                (obj, index) => RemoveSeries(index, obj),
                ResetSeries);

            ShouldPopulateLegendItems = true;

            ScheduleUpdate(true);
        }

        internal void InvalidateSeries(PolarSeries polarSeries)
        {
            foreach (var seriesView in _seriesViews.Children)
            {
                if (seriesView is SeriesView view && view.Series == polarSeries)
                {
                    view.InvalidateDrawable();
                    break;
                }
            }
        }

        internal void InvalidateDataLabelView()
        {
            _dataLabelView.InvalidateDrawable();
        }

        internal void InvalidateGridLayout()
        {
            _gridLineLayout.InvalidateDrawable();
        }
        #endregion

        #endregion
    }
}
