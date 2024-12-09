namespace Syncfusion.Maui.Toolkit.Charts
{
	internal partial class CircularPlotArea : ChartPlotArea
	{
		#region Private Fields

		readonly CircularChartArea _circularChartArea;

		#endregion

		#region Constructor
		/// <summary>
		/// 
		/// </summary>
		public CircularPlotArea(CircularChartArea chartArea) : base()
		{
			_circularChartArea = chartArea;
		}

		#endregion

		#region Methods
		protected override void UpdateLegendItemsSource()
		{
			if (Series == null || _legend == null || !_legend.IsVisible)
			{
				return;
			}

			_legendItems.Clear();

			foreach (ChartSeries series in Series)
			{
				if (series.IsVisibleOnLegend)
				{
					var count = 0;

					double sumOfYValues = double.NaN;

					if (series is PieSeries pieSeries)
					{
						if (!double.IsNaN(pieSeries.GroupTo))
						{
							sumOfYValues = pieSeries.SumOfYValues();
						}

						for (int i = 0; i < pieSeries.PointsCount; i++)
						{
							if (!double.IsNaN(sumOfYValues))
							{
								if (pieSeries.GetGroupModeValue(pieSeries.YValues[i], sumOfYValues) > pieSeries.GroupTo)
								{
									InsertLegendItemAt(i, pieSeries);
								}
								else
								{
									count++;
								}

								if (i == series.PointsCount - 1 && count > 0)
								{
									InsertLegendItemAt(-1, pieSeries);
								}
							}
							else
							{
								InsertLegendItemAt(i, pieSeries);
							}
						}
					}
					else
					{
						int index = 0;

						for (int i = 0; i < series.PointsCount; i++)
						{
							var legendItem = CircularPlotArea.CreateLegendItem(index, series);
							((IChartPlotArea)this).UpdateLegendLabelStyle(legendItem, _legend.LabelStyle ?? _chart?.LegendLabelStyle);
							_legend.OnLegendItemCreated(legendItem);
							_legendItems.Add(legendItem);
							index++;
						}
					}
				}
			}
		}

		static LegendItem CreateLegendItem(int index, ChartSeries series)
		{
			var legendItem = new LegendItem()
			{
				IconType = ChartUtils.GetShapeType(series.LegendIcon),
				Text = series.GetActualXValue(index)?.ToString() ?? string.Empty,
				Index = index,
				Source = series,
				Item = series.ActualData?[index]
			};

			Brush? solidColor = series.GetFillColor(legendItem, index);
			legendItem.IconBrush = solidColor ?? new SolidColorBrush(Colors.Transparent);

			return legendItem;
		}
		void InsertLegendItemAt(int index, PieSeries series)
		{
			var legendItem = new LegendItem();

			bool isGroupTo = !double.IsNaN(series.GroupTo);

			//legendItem.Series = series;
			//Need to set source for the legend item.
			if (index == -1)
			{
				legendItem.Text = isGroupTo ? SfCircularChartResources.Others : string.Empty;
				index = _legendItems.Count == 0 ? 0 : _legendItems.Count + 1;
			}
			else
			{
				legendItem.Text = series.GetActualXValue(index)?.ToString() ?? string.Empty;
			}

			index = index > _legendItems.Count ? _legendItems.Count : index;
			legendItem.Item = isGroupTo ? series.GroupToDataPoints[index] : series.ActualData?[index];
			legendItem.Source = series;
			legendItem.Index = index;

			legendItem.IconType = ChartUtils.GetShapeType(series.LegendIcon);
			Brush? solidColor = series.GetFillColor(legendItem, index);
			legendItem.IconBrush = solidColor ?? new SolidColorBrush(Colors.Transparent);
			((IChartPlotArea)this).UpdateLegendLabelStyle(legendItem, _legend?.LabelStyle ?? _chart?.LegendLabelStyle);

			_legend?.OnLegendItemCreated(legendItem);
			if (_legendItems.Count < index)
			{
				_legendItems.Add(legendItem);
			}
			else
			{
				_legendItems.Insert(index, legendItem);
			}
		}

		internal override void AddSeries(int index, object chartSeries)
		{
			if (chartSeries is CircularSeries circularSeries)
			{
				circularSeries.ChartArea = _circularChartArea;
			}

			base.AddSeries(index, chartSeries);
		}

		#endregion
	}

}
