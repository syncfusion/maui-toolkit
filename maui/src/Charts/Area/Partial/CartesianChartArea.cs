using System.Collections.ObjectModel;

namespace Syncfusion.Maui.Toolkit.Charts
{
	internal partial class CartesianChartArea
	{
		#region Properties
		internal Dictionary<object, List<CartesianSeries>>? SideBySideSeriesPosition { get; set; }

		internal double PreviousSBSMinWidth { get; set; }

		internal double SideBySideMinWidth { get; set; }

		internal bool EnableSideBySideSeriesPlacement
		{
			get
			{
				return _enableSideBySideSeriesPlacement;
			}
			set
			{
				if (value != _enableSideBySideSeriesPlacement)
				{
					_enableSideBySideSeriesPlacement = value;
					ResetSBSSegments();
					NeedsRelayout = true;
					ScheduleUpdateArea();
				}
			}
		}

		internal bool IsTransposed
		{
			get
			{
				return _isTransposed;
			}
			set
			{
				if (value != _isTransposed)
				{
					_isTransposed = value;
					NeedsRelayout = true;
					ScheduleUpdateArea();
				}
			}
		}
		#endregion

		#region SBS Members

		//TODO: Need to implement and simplify SBS calculations.
		internal void CalculateSbsPosition()
		{
			int index = -1;
			var visibleSeries = VisibleSeries;

			if (SideBySideSeriesPosition != null || visibleSeries == null)
			{
				return;
			}

			SideBySideSeriesPosition = [];
			var cartesianSeriesCollection = visibleSeries.OfType<CartesianSeries>();

			foreach (var cartesianSeries in cartesianSeriesCollection)
			{
				cartesianSeries.IsSbsValueCalculated = false;
			}

			foreach (var cartesianSeries in cartesianSeriesCollection)
			{
				if (cartesianSeries.ActualXAxis == null)
				{
					continue;
				}

				var groupingKeys = new Dictionary<string, int>();

				foreach (CartesianSeries xAxisRegSeries in cartesianSeries.ActualXAxis.RegisteredSeries.Cast<CartesianSeries>())
				{
					if (xAxisRegSeries.IsSideBySide)
					{
						if (!xAxisRegSeries.IsSbsValueCalculated && xAxisRegSeries.ActualXAxis != null)
						{
							//TODO: Need to update on stacking series. 
							if (xAxisRegSeries is StackingSeriesBase baseSeries)
							{
								if (baseSeries.ActualYAxis == null || baseSeries.ActualXAxis == null)
								{
									continue;
								}

								foreach (var yAxisRegSeries in baseSeries.ActualYAxis.RegisteredSeries)
								{
									if (!baseSeries.ActualXAxis.RegisteredSeries.Contains(yAxisRegSeries))
									{
										continue;
									}

									if (yAxisRegSeries is StackingSeriesBase stackingSeries)
									{
										if (!stackingSeries.IsSbsValueCalculated && _seriesGroup != null)
										{
											string groupID = _seriesGroup.FirstOrDefault(x => x.Value.Any(s => s.GroupingLabel == stackingSeries.GroupingLabel && s.GetType() == stackingSeries.GetType())).Key;
											StackingSeriesBase stackingSeriesBase; 
											int size = SideBySideSeriesPosition.Count > 0 && groupingKeys.Count > 0 && groupingKeys.TryGetValue(groupID, out var groupValue)
												? SideBySideSeriesPosition[groupValue].Count : 0;
											var isSameType = false;
											if (size > 0)
											{
												stackingSeriesBase = (StackingSeriesBase)SideBySideSeriesPosition[groupingKeys[groupID]][size - 1];
												isSameType = stackingSeriesBase != null
													&& stackingSeriesBase.ActualYAxis != null && stackingSeriesBase.ActualYAxis.RegisteredSeries.Contains(stackingSeries)
													&& stackingSeriesBase.CompareStackingSeries(stackingSeries);
											}

											if (groupingKeys.TryGetValue(groupID, out var value) && isSameType)
											{
												SideBySideSeriesPosition[value].Add(stackingSeries);
												stackingSeries.SideBySideIndex = value;
												stackingSeries.IsSbsValueCalculated = true;
											}

											else
											{
												if (groupingKeys.ContainsKey(groupID))
												{
													groupingKeys[groupID] = ++index;
												}
												else
												{
													groupingKeys.Add(groupID, ++index);
												}

												baseSeries.ActualXAxis.SideBySideSeriesCount = index + 1;
												if (SideBySideSeriesPosition.ContainsKey(index))
												{
													SideBySideSeriesPosition[index] = [];
												}
												else
												{
													SideBySideSeriesPosition.Add(index, []);
												}

												SideBySideSeriesPosition[index].Add(stackingSeries);
												stackingSeries.SideBySideIndex = index;
												stackingSeries.IsSbsValueCalculated = true;
											}
										}
									}
								}
							}
							else
							{
								if (SideBySideSeriesPosition.ContainsKey(++index))
								{
									SideBySideSeriesPosition[index] = [];
								}
								else
								{
									SideBySideSeriesPosition.Add(index, []);
								}

								xAxisRegSeries.ActualXAxis.SideBySideSeriesCount = index + 1;
								SideBySideSeriesPosition[index].Add(xAxisRegSeries);
								xAxisRegSeries.SideBySideIndex = index;
								xAxisRegSeries.IsSbsValueCalculated = true;
							}
						}
					}
				}

				UpdateSBS();
				index = -1;
			}
		}

		internal void UpdateSBS()
		{
			if (SideBySideSeriesPosition != null)
			{
				double totalWidth = GetTotalWidth() / SideBySideSeriesPosition.Count;
				double startPosition = 0, end = 0;

				for (int i = 0; i < SideBySideSeriesPosition.Count; i++)
				{
					var seriesGroup = SideBySideSeriesPosition.Values.ToList()[i];
					double sbsMaxWidth = GetSBSMaxWidth(seriesGroup);

					foreach (ChartSeries chartSeries in seriesGroup)
					{
						CartesianSeries cartesianSeries = (CartesianSeries)chartSeries;

						double spacing = cartesianSeries.GetActualSpacing() < 0 ? 0 : cartesianSeries.GetActualSpacing() > 1 ? 1 : cartesianSeries.GetActualSpacing();
						double width = cartesianSeries.GetActualWidth() < 0 ? 0 : cartesianSeries.GetActualWidth() > 1 ? 1 : cartesianSeries.GetActualWidth();

						if (!EnableSideBySideSeriesPlacement)
						{
							cartesianSeries.SbsInfo = new DoubleRange((-width * SideBySideMinWidth) / 2, (width * SideBySideMinWidth) / 2);
							continue;
						}

						if (cartesianSeries.ActualXAxis == null)
						{
							cartesianSeries.SbsInfo = DoubleRange.Empty;
						}

						double sideBySideMinWidth = SideBySideMinWidth == double.MaxValue ? 1 : SideBySideMinWidth;

						int seriesCount = cartesianSeries.ActualXAxis != null ? cartesianSeries.ActualXAxis.SideBySideSeriesCount : 0;

						if (cartesianSeries.SideBySideIndex == 0)
						{
							startPosition = -sideBySideMinWidth * (totalWidth / 2);
						}

						double space = (sbsMaxWidth - width) / seriesCount;
						double start = startPosition + ((space * sideBySideMinWidth) / 2);

						end = start + ((width / seriesCount) * sideBySideMinWidth);
						spacing = (spacing * sideBySideMinWidth) / seriesCount;

						if (!double.IsNaN(start))
						{
							start += spacing / 2;
							end -= spacing / 2;
						}

						cartesianSeries.SbsInfo = new DoubleRange(start, end);
						end += spacing / 2;
					}

					startPosition = end;
				}
			}
		}

		internal void UpdateMinWidth(CartesianSeries cartesianSeries, ref double minWidth, List<double> xValues)
		{
			int dataCount = xValues.Count;
			var visibleSeries = VisibleSeries;

			if (dataCount == 1)
			{
				_isSbsWithOneData = true;
			}

			if (xValues != null && xValues.Count > 0 && !cartesianSeries.IsIndexed)
			{
				//ColumnSeries looks tiny when series have single data point with IntervalType as Month and RangePadding as Additional.
				if (visibleSeries != null && dataCount == 1 && cartesianSeries.ActualXAxis is DateTimeAxis dateTimeAxis)
				{
					var seriesCollection = new ObservableCollection<CartesianSeries>();

					foreach (CartesianSeries series in visibleSeries.Cast<CartesianSeries>())
					{
						if (series != null && series.IsSideBySide && series.PointsCount > 0)
						{
							seriesCollection.Add(series);
						}
					}

					//When we set Minimum and Maximum property of DateTimeAxis with single data set for Stacking column series, segment has not been shown.
					if (seriesCollection != null && seriesCollection.All(x => x.PointsCount == 1 && x.GetXValues()?[0] == xValues[0]))
					{
						minWidth = dateTimeAxis.GetMinWidthForSingleDataPoint();
					}
				}

				var xData = new double[xValues.Count];
				xValues.CopyTo(xData, 0);
				Array.Sort(xData, 0, dataCount);

				for (int i = 0; i < dataCount - 1; i++)
				{
					double xValue = xData[i];
					double nextXValue = xData[i + 1];
					double delta = nextXValue - xValue;
					minWidth = Math.Min(delta == 0 ? minWidth : Math.Abs(delta), minWidth);
				}
			}
			else
			{
				minWidth = 1;
			}
		}

		internal void InvalidateMinWidth()
		{
			PreviousSBSMinWidth = SideBySideMinWidth;
			SideBySideMinWidth = double.MaxValue;
			double minWidth = double.MaxValue;

			var visibleSeries = VisibleSeries;

			if (visibleSeries != null)
			{
				foreach (ChartSeries chartSeries in visibleSeries)
				{
					if (chartSeries is CartesianSeries cartesianSeries && cartesianSeries.IsSideBySide && cartesianSeries.ItemsSource != null)
					{
						if (cartesianSeries.PointsCount >= 1)
						{
							List<double>? xValues = cartesianSeries.GetXValues();

							if (xValues != null)
							{
								UpdateMinWidth(cartesianSeries, ref minWidth, xValues);
							}
						}
						else if (cartesianSeries.ActualXAxis is DateTimeAxis dateTimeAxis)
						{
							SideBySideMinWidth = dateTimeAxis.GetMinWidthForSingleDataPoint();
						}
						else if (SideBySideMinWidth == double.MaxValue)
						{
							SideBySideMinWidth = 1;
						}
					}
				}

				if (visibleSeries != null && visibleSeries.Count > 1 && _isSbsWithOneData)
				{
					List<double> previousXValues = [];

					foreach (var chartSeries in visibleSeries)
					{
						if (chartSeries is CartesianSeries cartesianSeries && !cartesianSeries.IsIndexed && cartesianSeries.IsSideBySide)
						{
							var xValues = cartesianSeries.GetXValues();

							if (xValues != null && xValues.Count > 0)
							{
								//DateTimeAxis not rendered properly when series have single datapoint with different x position
								var actualXValues = xValues.ToList();
								previousXValues.AddRange(actualXValues);
								UpdateMinWidth(cartesianSeries, ref minWidth, previousXValues);
								previousXValues = actualXValues;
							}
						}
					}

					_isSbsWithOneData = false;
				}

				minWidth = minWidth == double.MaxValue ? 1 : minWidth;
				SideBySideMinWidth = Math.Min(SideBySideMinWidth, minWidth);
			}
		}

		internal void ResetSBSSegments()
		{
			var sideBySideSeries = VisibleSeries?.Where(series => series.IsSideBySide).ToList();

			if (sideBySideSeries != null && sideBySideSeries.Count > 0)
			{
				SideBySideSeriesPosition = null;

				foreach (var chartSeries in sideBySideSeries)
				{
					chartSeries.SegmentsCreated = false;
				}
			}
		}

		double GetTotalWidth()
		{
			double totalWidth = 0;

			if (SideBySideSeriesPosition != null)
			{
				for (int i = 0; i < SideBySideSeriesPosition.Count; i++)
				{
					double maxWidth = 0;
					foreach (ChartSeries sideBySideSeries in SideBySideSeriesPosition.Values.ToList()[i])
					{
						CartesianSeries cartesianSeries = (CartesianSeries)sideBySideSeries;
						double width = cartesianSeries.GetActualWidth();
						maxWidth = maxWidth > width ? maxWidth : width;
					}

					totalWidth += maxWidth;
				}
			}

			return totalWidth;
		}

		static double GetSBSMaxWidth(List<CartesianSeries> seriesGroup)
		{
			double maxWidth = 0;

			foreach (var cartesianSeries in seriesGroup)
			{
				double width = cartesianSeries.GetActualWidth();
				maxWidth = maxWidth > width ? maxWidth : width;
			}

			return maxWidth;
		}

		internal void UpdateStackingSeries()
		{
			//if visible series count is 0 or not contain any stacking series then return.
			if (VisibleSeries == null || VisibleSeries.Count == 0 || !VisibleSeries.Any(series => series is StackingSeriesBase && !series.SegmentsCreated))
			{
				return;
			}

			_seriesGroup = [];

			foreach (var series in VisibleSeries)
			{
				if (series is StackingSeriesBase stackingSeries && stackingSeries.IsVisible)
				{

					if (stackingSeries.RequiredEmptyPointReset)
					{
						stackingSeries.ResetEmptyPointIndexes();
						stackingSeries.RequiredEmptyPointReset = false;
					}

					stackingSeries.ValidateYValues();

					var stackingGroup = stackingSeries.GroupingLabel;
					var stackingXAxis = stackingSeries.ActualXAxis;
					var stackingYAxis = stackingSeries.ActualYAxis;

					if (string.IsNullOrEmpty(stackingGroup))
					{
						stackingGroup = "chart_default";
					}

					if (_seriesGroup.TryGetValue(stackingGroup, out List<StackingSeriesBase>? seriesList))
					{
						if (seriesList.Any(x => x.ActualXAxis != stackingXAxis || x.ActualYAxis != stackingYAxis) || (seriesList[0].GetType() != stackingSeries.GetType() && stackingGroup != stackingSeries.GroupingLabel))
						{
							string key = _seriesGroup.FirstOrDefault(x => x.Value.Any(y =>
											y.GetType().Name == stackingSeries.GetType().Name &&
											y.GroupingLabel == "" &&
											y.ActualYAxis?.RegisteredSeries.Contains(stackingSeries) == true)).Key;

							seriesList = key != null ? _seriesGroup[key] : [];
						}

						if (seriesList.Count > 0)
						{
							seriesList.Add(stackingSeries);
						}
						else
						{
							_seriesGroup.Add(stackingGroup + _seriesGroup.Count, [stackingSeries]);
						}
					}
					else
					{
						_seriesGroup.Add(stackingGroup, [stackingSeries]);
					}
				}
			}

			CalculateStackingValues(_seriesGroup);
		}

		static void CalculateStackingValues(Dictionary<string, List<StackingSeriesBase>> seriesGroup)
		{
			foreach (var seriesList in seriesGroup.Values)
			{
				var xValues = seriesList[0].GetXValues();
				var positiveYValues = new Dictionary<double, double>();
				var negativeYValues = new Dictionary<double, double>();

				double axisCross = 0;

				if (seriesList[0].ActualXAxis is ChartAxis axis)
				{
					if (!double.IsNaN(axis.ActualCrossingValue))
					{
						axisCross = axis.ActualCrossingValue;
					}
					else if (axis is LogarithmicAxis)
					{
						axisCross = 1;
					}
				}

				if (xValues != null)
				{
					foreach (var series in seriesList)
					{
						var yValues = series.YValues;
						var bottomValues = new List<double>();
						var topValues = new List<double>();

						for (int i = 0; i < xValues.Count; i++)
						{
							var xValue = xValues[i];
							var yValue = i < yValues.Count ? yValues[i] : 0;
							yValue = double.IsNaN(yValue) ? 0 : yValue;

							if (yValue >= 0)
							{
								if (positiveYValues.TryGetValue(xValue, out double currentValue))
								{
									bottomValues.Add((axisCross > currentValue) ? axisCross : currentValue);
									if (series.GetType().Name.Contains("Stacking", StringComparison.Ordinal) && series.GetType().Name.Contains("100Series", StringComparison.Ordinal))
									{
										yValue = GetYValue(seriesList, yValue, i);
									}
									positiveYValues[xValue] = currentValue + yValue;
								}
								else
								{
									bottomValues.Add(axisCross);
									if (series.GetType().Name.Contains("Stacking", StringComparison.Ordinal) && series.GetType().Name.Contains("100Series", StringComparison.Ordinal))
									{
										yValue = GetYValue(seriesList, yValue, i);
									}
									positiveYValues.Add(xValue, yValue);
								}

								topValues.Add(positiveYValues[xValue]);
							}
							else
							{
								if (!negativeYValues.TryAdd(xValue, yValue))
								{
									bottomValues.Add((axisCross < negativeYValues[xValue]) ? axisCross : negativeYValues[xValue]);
									if (series.GetType().Name.Contains("Stacking", StringComparison.Ordinal) && series.GetType().Name.Contains("100Series", StringComparison.Ordinal))
									{
										yValue = GetYValue(seriesList, yValue, i);
									}
									negativeYValues[xValue] += yValue;
								}
								else
								{
									bottomValues.Add(axisCross);
								}

								topValues.Add(negativeYValues[xValue]);
							}
						}

						series.BottomValues = bottomValues;
						series.TopValues = topValues;
					}
				}
			}
		}

		static double GetYValue(List<StackingSeriesBase> SeriesList, double yValue, int index)
		{
			double total = SeriesList.Where(series => series != null && series.YValues.Count > index).Sum(series => double.IsNaN(series.YValues[index]) ? 0 : Math.Abs(series.YValues[index]));

			if (yValue != 0)
			{ 
				yValue = (yValue / total) * 100;
			}

			return yValue;
		}
		#endregion
	}
}
