using System.Collections;

namespace Syncfusion.Maui.Toolkit.Charts
{
	public partial class CategoryAxis : ChartAxis
	{
		#region Methods

		#region Protected Methods

		/// <inheritdoc/>
		protected override double CalculateActualInterval(DoubleRange range, Size availableSize)
		{
			if (double.IsNaN(AxisInterval) || AxisInterval <= 0)
			{
				return Math.Max(1d, Math.Floor(range.Delta / GetActualDesiredIntervalsCount(availableSize)));
			}

			return AxisInterval;
		}

		/// <inheritdoc/>
		protected override DoubleRange ApplyRangePadding(DoubleRange range, double interval)
		{
			return LabelPlacement == LabelPlacement.BetweenTicks ? new DoubleRange(-0.5, (int)range.End + 0.5) : range;
		}

		#endregion

		#region Internal Methods
		internal void GroupData()
		{
			List<string> groupingValues = [];
			List<object> groupedDatas = [];

			foreach (CartesianSeries series in RegisteredSeries.Cast<CartesianSeries>())
			{
				if (series.ActualXValues is List<string> xValues)
				{
					if (groupedDatas.Count != 0)
					{
						for (int j = 0; j <= xValues.Count - 1; j++)
						{
							if (!groupingValues.Contains(xValues[j]))
							{
								groupingValues.Add(xValues[j]);
							}
						}
					}
					else
					{
						groupingValues.AddRange(xValues);
					}
				}
				else if (series.ActualXValues != null)
				{
					groupingValues.AddRange(from val in (series.ActualXValues as List<double>) select val.ToString());
				}

				if (groupingValues.Count != groupedDatas.Count)
				{
					groupedDatas.AddRange(series.ActualData ?? groupedDatas);
				}
			}

			var distinctXValues = groupingValues.Distinct().ToList();

			foreach (CartesianSeries series in RegisteredSeries.Cast<CartesianSeries>())
			{
				if (series.ActualXValues is List<string> list)
				{
					series.GroupedXValuesIndexes = (from val in list select (double)distinctXValues.IndexOf(val)).ToList();
				}
				else if (series.ActualXValues != null)
				{
					series.GroupedXValuesIndexes = (from val in series.ActualXValues as List<double> select (double)distinctXValues.IndexOf(val.ToString())).ToList();
				}

				series.GroupedXValues = distinctXValues;

				//TODO: Remove group values for dynamic update. 

				series.GroupedActualData = groupedDatas;
			}
		}

		internal override void GenerateVisibleLabels()
		{
			if (VisibleRange.IsEmpty)
			{
				return;
			}

			_isOverriddenOnCreateLabelsMethod = ChartUtils.IsOverriddenMethod(this, "OnCreateLabels");

			var actualLabels = VisibleLabels;
			DoubleRange visibleRange = VisibleRange;
			double actualInterval = ActualInterval;
			double interval = VisibleInterval;
			double position = visibleRange.Start - (visibleRange.Start % actualInterval);
			var actualSeries = GetActualSeries();

			var roundInterval = Math.Ceiling(interval);

			var values = ArrangeByIndex ? actualSeries?.XValues as IList : actualSeries?.GroupedXValues as IList;

			var maxDataCount = values?.Count;

			for (; position <= visibleRange.End; position += roundInterval)
			{
				int pos = (int)position;

				if (visibleRange.Inside(pos) && pos < maxDataCount && pos > -1)
				{
					var format = LabelStyle != null ? LabelStyle.LabelFormat : string.Empty;
					var content = GetLabelContent(actualSeries, pos, format);
					var axisLabel = new ChartAxisLabel(pos, content ?? string.Empty);
					actualLabels?.Add(axisLabel);

					if (!_isOverriddenOnCreateLabelsMethod && LabelPlacement != LabelPlacement.BetweenTicks)
					{
						TickPositions.Add(pos);
					}
				}
			}

			if (LabelPlacement == LabelPlacement.BetweenTicks)
			{
				double pos = 0;
				position = visibleRange.Start - (visibleRange.Start % actualInterval);

				for (; position <= visibleRange.End; position += 1)
				{
					pos = ((int)Math.Round(position)) - 0.5;

					if (visibleRange.Inside(pos) && pos < maxDataCount && pos > -1)
					{
						TickPositions.Add(pos);
					}
				}

				pos += 1;

				if (visibleRange.Inside(pos) && pos < maxDataCount && pos > -1)
				{
					TickPositions.Add(pos);
				}
			}

			if (_isOverriddenOnCreateLabelsMethod)
			{
				OnCreateLabels();
				AddVisibleLabels();
			}

		}

#pragma warning disable IDE0060 // Remove unused parameter
		internal string GetLabelContent(ChartSeries? chartSeries, int pos, string labelFormat)
#pragma warning restore IDE0060 // Remove unused parameter
		{
			var labelContent = string.Empty;
			int count = 0;

			foreach (var series in RegisteredSeries)
			{
				string label = string.Empty;

				var cartesianSeries = series;
				var values = ArrangeByIndex ? cartesianSeries.XValues as IList : cartesianSeries.GroupedXValues as IList;

				if (values != null && values.Count > pos && pos >= 0)
				{
					var xValue = values[pos];

					if (xValue != null)
					{
						if (!ArrangeByIndex && series.XValues is IList xValueList)
						{
							if (series.XValueType == ChartValueType.String)
							{
								if (!xValueList.Contains(xValue))
								{
									continue;
								}
							}
							else if (series.XValueType == ChartValueType.Double || series.XValueType == ChartValueType.DateTime)
							{
								if (double.TryParse(xValue.ToString(), out double result))
								{
									if (!xValueList.Contains(result))
									{
										continue;
									}
								}
								else
								{
									continue;
								}
							}
						}

						if (cartesianSeries.XValueType == ChartValueType.String)
						{
							label = (string)xValue;
						}
						else if (cartesianSeries.XValueType == ChartValueType.DateTime)
						{
							if (string.IsNullOrEmpty(labelFormat))
							{
								labelFormat = "dd/MM/yyyy";
							}
							xValue = Convert.ToDouble(xValue);
							label = GetFormattedAxisLabel(labelFormat, xValue);
						}
						else if (cartesianSeries?.XValueType == ChartValueType.Double)
						{
							labelFormat = "";
							xValue = Convert.ToDouble(xValue);
							label = GetActualLabelContent(xValue, labelFormat);
						}

						if (!string.IsNullOrEmpty(label.ToString()) && !labelContent.Equals(label, StringComparison.Ordinal) && ArrangeByIndex)
						{
							labelContent = count > 0 && !string.IsNullOrEmpty(labelContent) ? labelContent + ", " + label : label.ToString();
						}

						if (!ArrangeByIndex)
						{
							return label;
						}

						count++;
					}
				}
			}

			return labelContent;
		}

		#endregion

		#region Private Methods

		//Todo: Remove this method while implementing ArrangeByIndex feature.
		internal ChartSeries? GetActualSeries()
		{
			var visibleSeries = GetVisibleSeries();
			if (visibleSeries == null)
			{
				return null;
			}

			int dataCount = 0;
			ChartSeries? selectedSeries = null;

			if (IsPolarArea)
			{
				foreach (PolarSeries series in visibleSeries.Cast<PolarSeries>())
				{
					if (series != null && series.ActualXAxis == this && series.PointsCount > dataCount)
					{
						selectedSeries = series;
						dataCount = series.PointsCount;
					}
				}
			}
			else
			{

				foreach (CartesianSeries series in visibleSeries.Cast<CartesianSeries>())
				{
					if (series != null && series.ActualXAxis == this && series.PointsCount > dataCount)
					{
						selectedSeries = series;
						dataCount = series.PointsCount;
					}
				}
			}

			return selectedSeries;
		}

		#endregion

		#endregion
	}
}
