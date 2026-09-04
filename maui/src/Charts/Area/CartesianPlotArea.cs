using Microsoft.Maui.Layouts;

namespace Syncfusion.Maui.Toolkit.Charts
{
	internal partial class CartesianPlotArea : ChartPlotArea
	{
		#region Fields

		readonly CartesianGridLineLayout _gridLineLayout;
		internal readonly ChartTrendlineView _chartTrendlineView;
		readonly CartesianChartArea _chartArea;
		readonly ChartPlotBandView _chartPlotBandView;

		#endregion

		#region Constructor

		public CartesianPlotArea(CartesianChartArea area) : base()
		{
			BatchBegin();
			_chartArea = area;
			_gridLineLayout = new CartesianGridLineLayout(area);
			_chartTrendlineView = new ChartTrendlineView(this);
			_chartPlotBandView = new ChartPlotBandView(area);
			AbsoluteLayout.SetLayoutBounds(_gridLineLayout, new Rect(0, 0, 1, 1));
			AbsoluteLayout.SetLayoutFlags(_gridLineLayout, AbsoluteLayoutFlags.All);
			AbsoluteLayout.SetLayoutBounds(_chartTrendlineView, new Rect(0, 0, 1, 1));
			AbsoluteLayout.SetLayoutFlags(_chartTrendlineView, AbsoluteLayoutFlags.All);
			AbsoluteLayout.SetLayoutBounds(_chartPlotBandView, new Rect(0, 0, 1, 1));
			AbsoluteLayout.SetLayoutFlags(_chartPlotBandView, AbsoluteLayoutFlags.All);
			Insert(0, _gridLineLayout);
			Insert(1, _chartPlotBandView);
			InsertTrendlineLayout();
			BatchCommit();
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Inserts the TrendlineLayout at the correct position in the visual tree
		/// to ensure trendlines render above series but below data labels.
		/// </summary>
		void InsertTrendlineLayout()
		{
			// Find the index of SeriesViews to insert TrendlineLayout right after it
			int seriesViewsIndex = Children.IndexOf(_seriesViews);
			if (seriesViewsIndex >= 0)
			{
				// Insert TrendlineLayout after SeriesViews
				Insert(seriesViewsIndex + 1, _chartTrendlineView);
			}
		}

		#endregion

		#region Protected Methods

		protected override void UpdateLegendItemsSource()
		{
			if (Series == null || _legend == null)
			{
				return;
			}

			_legendItems.Clear();
			int index = 0;

			foreach (CartesianSeries series in Series.Cast<CartesianSeries>())
			{
				if (series.IsVisibleOnLegend)
				{
					AddLegendItem(series, index);
					index++;
				}
			}

			foreach (CartesianSeries series in Series)
			{
				if (series.IsVisibleOnLegend && series.Trendlines != null)
				{
					foreach (ChartTrendline trendline in series.Trendlines)
					{
						AddLegendItem(trendline, index);
						index++;
					}
				}
			}
		}

		/// <summary>
		/// Creates and adds a legend item for either a chart series or a trendline.
		/// </summary>
		private void AddLegendItem(ICartesianLegendDependent? series, int index)
		{
			var legendItem = new LegendItem();

			if (series != null)
			{
				legendItem.IconType = series.LegendIcon;
				legendItem.Item = series;
				legendItem.Source = series;
				Brush? solidColor = series.GetLegendBrush(legendItem, index);
				legendItem.IconBrush = solidColor != null ? solidColor : new SolidColorBrush(Colors.Transparent);
				legendItem.Text = series.LegendText;
				legendItem.IsToggled = !series.IsVisible;
			}

			legendItem.Index = index;
			((IChartPlotArea)this).UpdateLegendLabelStyle(legendItem, _legend?.LabelStyle ?? _chart?.LegendLabelStyle);
			_legend?.OnLegendItemCreated(legendItem);
			_legendItems?.Add(legendItem);
		}

		#endregion

		#region Internal Methods

		internal void InvalidateRender()
		{
			_gridLineLayout?.InvalidateDrawable();
			InvalidatePlotBands();
		}

		internal void InvalidatePlotBands()
		{
			_chartPlotBandView.InvalidateDrawable();
		}

		internal override void AddSeries(int index, object chartSeries)
		{
			if (chartSeries is CartesianSeries cartesian)
			{
				cartesian.ChartArea = _chartArea;
			}

			base.AddSeries(index, chartSeries);
		}

		#endregion
	}
}