using Microsoft.Maui.Layouts;
using Syncfusion.Maui.Toolkit.Graphics.Internals;
using Syncfusion.Maui.Toolkit.Internals;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Syncfusion.Maui.Toolkit.Charts
{
	internal partial class ChartPlotArea : AbsoluteLayout, IChartPlotArea
	{
		#region Fields
		readonly DataLabelView _dataLabelView;
		readonly AbsoluteLayout _itemLabelLayout;

		internal readonly AbsoluteLayout _seriesViews;
		internal readonly ObservableCollection<ILegendItem> _legendItems;

		ChartSeriesCollection? _series;
		View? _plotAreaBackgroundView;
		bool _shouldPopulateLegendItems = true;
		EventHandler<EventArgs>? _legendItemsUpdated;
		EventHandler<LegendItemEventArgs>? _legendItemsToggled;

		internal IChart? _chart;
		internal IChartLegend? _legend;
		#endregion

		#region Constructor

		/// <summary>
		/// Helps to arrange series and data labels views. 
		/// </summary>
		public ChartPlotArea()
		{
			BatchBegin();
			_series = [];
			_legendItems = [];
			_seriesViews = [];
			_itemLabelLayout = [];
			_dataLabelView = new DataLabelView(this);
			AbsoluteLayout.SetLayoutBounds(_seriesViews, new Rect(0, 0, 1, 1));
			AbsoluteLayout.SetLayoutFlags(_seriesViews, AbsoluteLayoutFlags.All);
			AbsoluteLayout.SetLayoutBounds(_itemLabelLayout, new Rect(0, 0, 1, 1));
			AbsoluteLayout.SetLayoutFlags(_itemLabelLayout, AbsoluteLayoutFlags.All);
			AbsoluteLayout.SetLayoutBounds(_dataLabelView, new Rect(0, 0, 1, 1));
			AbsoluteLayout.SetLayoutFlags(_dataLabelView, AbsoluteLayoutFlags.All);
			Add(_seriesViews);
			Add(_dataLabelView);
			Add(_itemLabelLayout);
			BatchCommit();
		}

		#endregion

		#region Properties

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
					OnSeriesCollectionChanging();
					_series = value;
					OnSeriesCollectionChanged();
				}
			}
		}

		public Rect PlotAreaBounds { get; set; }

		public bool ShouldPopulateLegendItems
		{
			get
			{
				return _shouldPopulateLegendItems;
			}
			set
			{
				_shouldPopulateLegendItems = value;
			}
		}

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

		public ReadOnlyObservableCollection<ILegendItem> LegendItems => new(_legendItems);

		public ReadOnlyObservableCollection<ChartSeries>? VisibleSeries => _series?.GetVisibleSeries();

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

		SfDrawableView IChartPlotArea.DataLabelView => _dataLabelView;

		LegendHandler IPlotArea.LegendItemToggleHandler
		{
			get
			{
				return ToggleLegendItem;
			}
		}

		event EventHandler<EventArgs> IPlotArea.LegendItemsUpdated { add { _legendItemsUpdated += value; } remove { _legendItemsUpdated -= value; } }
		event EventHandler<LegendItemEventArgs>? IPlotArea.LegendItemToggled { add { _legendItemsToggled += value; } remove { _legendItemsToggled -= value; } }

		#endregion

		#region Methods

		#region Public Members 

		public void UpdateLegendItems()
		{
			if (_shouldPopulateLegendItems)
			{
				UpdateLegendItemsSource();
				_shouldPopulateLegendItems = false;
				_legendItemsUpdated?.Invoke(this, EventArgs.Empty);
			}
		}

		public void UpdateVisibleSeries()
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

		#endregion

		#region Protected Methods

		protected virtual void UpdateLegendItemsSource()
		{

		}

		#endregion

		#region Series CollectionChanged Methods

		internal virtual void AddSeries(int index, object series)
		{
			if (series is ChartSeries chartSeries)
			{
				chartSeries.OnAttachedToChart(_chart);

				var seriesView = new SeriesView(chartSeries, this);
				AbsoluteLayout.SetLayoutBounds(seriesView, new Rect(0, 0, 1, 1));
				AbsoluteLayout.SetLayoutFlags(seriesView, AbsoluteLayoutFlags.All);

				if (chartSeries.IsStacking)
				{
					index = 0;
				}

				_seriesViews.Insert(index, seriesView);

				var labelView = chartSeries.LabelTemplateView;
				AbsoluteLayout.SetLayoutBounds(labelView, new Rect(0, 0, 1, 1));
				AbsoluteLayout.SetLayoutFlags(labelView, AbsoluteLayoutFlags.All);
				_itemLabelLayout.Insert(index, labelView);
			}
		}

		internal virtual void RemoveSeries(int index, object series)
		{
			if (series is ChartSeries chartSeries)
			{
				chartSeries.OnDetachedToChart();
				_itemLabelLayout.Children.Remove(chartSeries.LabelTemplateView);
				var seriesView = GetSeriesView(chartSeries);

				if (seriesView != null)
				{
					_seriesViews.Children.Remove(seriesView);
				}
			}
		}

		SeriesView? GetSeriesView(ChartSeries chartSeries)
		{
			foreach (SeriesView view in _seriesViews.Children.Cast<SeriesView>())
			{
				if (view._series == chartSeries)
				{
					return view;
				}
			}

			return null;
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
					//Todo:Validate series before add into the collection. its cartesian or circular chart.
					AddSeries(count, chartSeries);
					count++;//To maintain index for stacked series. 
				}

				if (_legend != null)
				{
					UpdateLegendItems();
				}
			}
		}

		void Series_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
		{
			e.ApplyCollectionChanges(
				(obj, index, canInsert) => AddSeries(index, obj),
				(obj, index) => RemoveSeries(index, obj),
				ResetSeries);

			ShouldPopulateLegendItems = true;

			if (_chart?.Area is { } area)
			{
				area.NeedsRelayout = true;
				area.ScheduleUpdateArea();
			}
		}

		void ResetSeries()
		{
			//Todo:Need to clear the ChartSeries ResetData method and ensure the dynamic changes
			foreach (SeriesView seriesView in _seriesViews.Children.Cast<SeriesView>())
			{
				seriesView._series.OnDetachedToChart();
			}

			if (_chart != null)
			{
				_chart.IsRequiredDataLabelsMeasure = true;
			}

			_itemLabelLayout.Clear();
			_seriesViews.Clear();
		}

		#endregion

		#region Private Methods

		void ToggleLegendItem(ILegendItem legendItem)
		{
			if (_legend?.IsVisible == true && _legend.ToggleVisibility && legendItem is LegendItem chartLegendItem && chartLegendItem.Item != null)
			{
				//Todo:Need to change ShouldLegendRefresh in Area class
				//chart.RequiredLegendRefresh = false;
				//Todo: Need to invalidate the series creation and drawing for dynamic changes.
				if (chartLegendItem.Source is ChartSeries series)
				{
					series.LegendItemToggled(chartLegendItem);
				}
			}
		}

		#endregion

		#endregion
	}
}
