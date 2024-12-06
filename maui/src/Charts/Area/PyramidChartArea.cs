using Microsoft.Maui.Layouts;
using Syncfusion.Maui.Toolkit.Graphics.Internals;
using Syncfusion.Maui.Toolkit.Internals;
using System.Collections.ObjectModel;

namespace Syncfusion.Maui.Toolkit.Charts
{
	internal partial class PyramidChartArea : AreaBase, IChartPlotArea
	{
		#region Fields
		const double SizeRatio = 0.8;

		readonly IPyramidChartDependent _chart;
		readonly PyramidChartView _seriesView;
		readonly PyramidDataLabelView _dataLabelView;
		readonly ObservableCollection<ILegendItem> _legendItems;

		bool _shouldPopulateLegendItems = true;

		EventHandler<EventArgs>? _legendItemsUpdated;
		EventHandler<LegendItemEventArgs>? _legendItemsToggled;
		View? _plotAreaBackgroundView;
		#endregion

		internal ILegend? _legend;

		#region Constructor

		/// <summary>
		/// It helps to create chart area to hold the view for pyramid and funnel charts.
		/// </summary>
		/// <param name="chart"></param>
		public PyramidChartArea(IPyramidChartDependent chart)
		{
			BatchBegin();
			_chart = chart;
			_legendItems = [];
			_seriesView = new PyramidChartView(chart);
			_dataLabelView = new PyramidDataLabelView(chart);
			AbsoluteLayout.SetLayoutBounds(_seriesView, new Rect(0, 0, 1, 1));
			AbsoluteLayout.SetLayoutFlags(_seriesView, AbsoluteLayoutFlags.All);
			AbsoluteLayout.SetLayoutBounds(_dataLabelView, new Rect(0, 0, 1, 1));
			AbsoluteLayout.SetLayoutFlags(_dataLabelView, AbsoluteLayoutFlags.All);
			Add(_seriesView);
			Add(_dataLabelView);

			if (chart is IChart iChart)
			{
				AbsoluteLayout.SetLayoutBounds(iChart.BehaviorLayout, new Rect(0, 0, 1, 1));
				AbsoluteLayout.SetLayoutFlags(iChart.BehaviorLayout, Microsoft.Maui.Layouts.AbsoluteLayoutFlags.All);
				Add(iChart.BehaviorLayout);
			}

			BatchCommit();
		}

		#endregion

		#region Properties
		public override IPlotArea PlotArea => this;

		event EventHandler<EventArgs> IPlotArea.LegendItemsUpdated { add { _legendItemsUpdated += value; } remove { _legendItemsUpdated -= value; } }
		event EventHandler<LegendItemEventArgs>? IPlotArea.LegendItemToggled { add { _legendItemsToggled += value; } remove { _legendItemsToggled -= value; } }

		LegendHandler IPlotArea.LegendItemToggleHandler
		{
			get
			{
				return ToggleLegendItem;
			}
		}

		ILegend? IPlotArea.Legend
		{
			get
			{
				return _legend;
			}
			set
			{
				if (value != _legend)
				{
					_legend = value;
				}
			}
		}
		public ReadOnlyObservableCollection<ILegendItem> LegendItems => new(_legendItems);

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

		#endregion

		#region Interface Implementation

		SfDrawableView IChartPlotArea.DataLabelView => _dataLabelView;

		#endregion

		#region Methods

		protected override void UpdateAreaCore()
		{
			if (_chart is IChart iChart)
			{
				iChart.ResetTooltip();
				var bounds = ChartUtils.GetSeriesClipRect(AreaBounds, iChart.TitleHeight);
				iChart.ActualSeriesClipRect = bounds;

				//Rendering funnel and pyramid only 80% of its actual size.
				var width = bounds.Width * SizeRatio;
				var height = bounds.Height * SizeRatio;
				var x = (bounds.Width - width) / 2;
				var y = (bounds.Height - height) / 2;
				var seriesBounds = new Rect(x, y, width, height);
				_chart.SeriesBounds = seriesBounds;
			}

			if (!_chart.SegmentsCreated)
			{
				if (_chart.Segments.Count != 0)
				{
					_chart.Segments.Clear();
					_chart.DataLabels.Clear();
				}
			}

			_chart.GenerateSegments();
			_chart.LayoutSegments();
			InvalidateChart();
		}

		public void UpdateLegendItems()
		{
			if (_shouldPopulateLegendItems)
			{
				UpdateLegendItemsSource();
				_shouldPopulateLegendItems = false;
				_legendItemsUpdated?.Invoke(this, EventArgs.Empty);
			}
		}

		internal void InvalidateChart()
		{
			_seriesView?.InvalidateDrawable();
			_dataLabelView?.InvalidateDrawable();
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

		void UpdateLegendItemsSource()
		{
			if (_chart is IPyramidChartDependent triangleChart)
			{
				triangleChart.UpdateLegendItemsSource(_legendItems);
			}
		}

		void ToggleLegendItem(ILegendItem legendItem)
		{
			if (_legend != null && _legend.IsVisible && _legend.ToggleVisibility)
			{
				_chart.SegmentsCreated = false;
				_chart.ScheduleUpdateChart();
			}
		}

		#endregion
	}
}
