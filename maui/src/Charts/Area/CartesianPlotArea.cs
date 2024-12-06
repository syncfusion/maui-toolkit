using Microsoft.Maui.Layouts;

namespace Syncfusion.Maui.Toolkit.Charts
{
	internal partial class CartesianPlotArea : ChartPlotArea
	{
		#region Fields

		readonly CartesianGridLineLayout _gridLineLayout;
		readonly CartesianChartArea _chartArea;
		readonly ChartPlotBandView _chartPlotBandView;

		#endregion

		#region Constructor

		public CartesianPlotArea(CartesianChartArea area) : base()
		{
			BatchBegin();
			_chartArea = area;
			_gridLineLayout = new CartesianGridLineLayout(area);
			_chartPlotBandView = new ChartPlotBandView(area);
			AbsoluteLayout.SetLayoutBounds(_gridLineLayout, new Rect(0, 0, 1, 1));
			AbsoluteLayout.SetLayoutFlags(_gridLineLayout, AbsoluteLayoutFlags.All);
			AbsoluteLayout.SetLayoutBounds(_chartPlotBandView, new Rect(0, 0, 1, 1));
			AbsoluteLayout.SetLayoutFlags(_chartPlotBandView, AbsoluteLayoutFlags.All);
			Insert(0, _gridLineLayout);
			Insert(1, _chartPlotBandView);
			BatchCommit();
		}

		#endregion

		#region Protected Methods

		protected override void UpdateLegendItemsSource()
		{
			if (Series == null || _legend == null || !_legend.IsVisible)
			{
				return;
			}

			_legendItems.Clear();
			int index = 0;

			foreach (CartesianSeries series in Series.Cast<CartesianSeries>())
			{
				if (series.IsVisibleOnLegend)
				{
					var legendItem = new LegendItem
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

					((IChartPlotArea)this).UpdateLegendLabelStyle(legendItem, _legend.LabelStyle ?? _chart?.LegendLabelStyle);
					_legend.OnLegendItemCreated(legendItem);
					_legendItems?.Add(legendItem);
					index++;
				}
			}
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