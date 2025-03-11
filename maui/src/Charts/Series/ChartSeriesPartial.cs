using Syncfusion.Maui.Toolkit.Graphics.Internals;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Syncfusion.Maui.Toolkit.Charts
{
	public partial class ChartSeries
	{
		#region Internal Fields

		internal delegate object? GetReflectedProperty(object obj, string[] paths);
		internal float _sumOfYValues = float.NaN;

		#endregion

		#region Private Fields

		bool _isComplexYProperty;

		ChartValueType _xValueType;

		bool _isRepeatPoint;

		#endregion

		#region Internal Properties

		internal readonly ObservableCollection<ChartSegment> _segments;

		internal virtual bool IsMultipleYPathRequired
		{
			get
			{
				return false;
			}
		}

		internal virtual bool IsSideBySide => false;

		internal double XData { get; set; }

		internal int PointsCount { get; set; }

		internal ChartValueType XValueType
		{
			get
			{
				return _xValueType;
			}

			set
			{
				_xValueType = value;
			}
		}

		internal IEnumerable? XValues { get; set; }

		internal string[][]? YComplexPaths { get; private set; }

		internal IEnumerable? ActualXValues { get; set; }

		internal IList<double>[]? SeriesYValues { get; private set; }

		internal IList<double>[]? ActualSeriesYValues { get; private set; }

		internal string[]? YPaths { get; private set; }

		internal List<object>? ActualData { get; set; }

		internal string[]? XComplexPaths { get; set; }

		internal bool IsLinearData { get; set; } = true;

		internal bool IsDataPointAddedDynamically { get; set; }

		#endregion

		#region Methods

		#region Internal Methods

#pragma warning disable IDE0060 // Remove unused parameter
		internal virtual void AddDataPoint(object data, int index, NotifyCollectionChangedEventArgs e)
#pragma warning restore IDE0060 // Remove unused parameter
		{
			SetIndividualPoint(data, index, false);
		}

		internal virtual void LegendItemToggled(LegendItem chartLegendItem)
		{
		}

		internal virtual void OnDataSourceChanged(object oldValue, object newValue)
		{
		}

		internal virtual void OnBindingPathChanged()
		{
			UpdateLegendItems();
			SegmentsCreated = false;

			if (Chart != null)
			{
				Chart.IsRequiredDataLabelsMeasure = true;
			}

			ScheduleUpdateChart();
		}

		internal virtual void GenerateDataPoints()
		{
		}

		internal virtual void OnDataSource_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
		{
			IsDataPointAddedDynamically = false;
			e.ApplyCollectionChanges((obj, index, canInsert) => AddDataPoint(index, obj, e), (obj, index) => RemoveData(index, e), ResetDataPoint);

			if (e.Action == NotifyCollectionChangedAction.Add && EnableAnimation && AnimationDuration > 0)
			{
				IsDataPointAddedDynamically = true;
				NeedToAnimateSeries = true;
			}

			if (IsSideBySide)
			{
				if (this is CartesianSeries series && series.ChartArea != null)
				{
					series.InvalidateSideBySideSeries();
					series.ChartArea.ResetSBSSegments();
				}
			}

			InvalidateGroupValues();
			SegmentsCreated = false;

			if (Chart != null)
			{
				Chart.IsRequiredDataLabelsMeasure = true;
			}

			UpdateLegendItems();
			ScheduleUpdateChart();
		}

		internal virtual void RemoveData(int index, NotifyCollectionChangedEventArgs e)
		{
			if (XValues is IList<double> list)
			{
				list.RemoveAt(index);
				PointsCount--;
			}
			else if (XValues is IList<string> list1)
			{
				list1.RemoveAt(index);
				PointsCount--;
			}

			for (var i = 0; i < SeriesYValues?.Length; i++)
			{
				if (YPaths != null && YPaths.Length > 0)
				{
					RemoveYValue(index, YPaths[i], SeriesYValues[i]);
				}

				SeriesYValues[i].RemoveAt(index);
			}

			ActualData?.RemoveAt(index);
		}

		internal virtual void UpdateRange()
		{
		}

		internal virtual void HookAndUnhookCollectionChangedEvent(object oldValue, object? newValue)
		{
			if (newValue != null)
			{
				if (newValue is INotifyCollectionChanged newCollectionValue)
				{
					newCollectionValue.CollectionChanged += OnDataSource_CollectionChanged;
				}
			}

			if (oldValue != null)
			{
				if (oldValue is INotifyCollectionChanged oldCollectionValue)
				{
					oldCollectionValue.CollectionChanged -= OnDataSource_CollectionChanged;
				}
			}

			InvalidateGroupValues();
		}

		internal void InvalidateGroupValues()
		{
			if (this is CartesianSeries series && series.ChartArea is CartesianChartArea cartesianChartArea && series.ActualXAxis is CategoryAxis categoryAxis && !categoryAxis.ArrangeByIndex)
			{
				if (cartesianChartArea.VisibleSeries != null)
				{
					categoryAxis.GroupData();

					if (categoryAxis.RegisteredSeries.Count > 0)
					{
						foreach (CartesianSeries chartSeries in categoryAxis.RegisteredSeries.Cast<CartesianSeries>())
						{
							chartSeries.SegmentsCreated = false;
							chartSeries.ChartArea?.UpdateVisibleSeries();
						}
					}
				}
			}
		}

		//TODO:Need to remove the replace parameter from this method,
		//because new notify collectionChanged event first remove
		//the data and then insert the data. So no need replace parameter here after.
		internal virtual void SetIndividualPoint(int index, object obj, bool replace)
		{
			if (SeriesYValues != null && YPaths != null && ItemsSource != null)
			{
				double yData;
				var xvalueType = GetArrayPropertyValue(obj, XComplexPaths);

				if (xvalueType != null)
				{
					XValueType = GetDataType(xvalueType);
				}

				if (IsMultipleYPathRequired)
				{
					if (XValueType == ChartValueType.String)
					{
						if (XValues is not List<string>)
						{
							XValues = ActualXValues = new List<string>();
						}

						var xValue = (List<string>)XValues;
						var xVal = GetArrayPropertyValue(obj, XComplexPaths);
						var xData = xVal as string;

						if (replace && xValue.Count > index)
						{
							if (xValue[index] == xData)
							{
								_isRepeatPoint = true;
							}
							else
							{
								xValue[index] = xData.Tostring();
							}
						}
						else
						{
							xValue.Insert(index, xData.Tostring());
						}

						for (int i = 0; i < YPaths.Length; i++)
						{
							var yVal = YComplexPaths == null ? obj : GetArrayPropertyValue(obj, YComplexPaths[i]);
							yData = Convert.ToDouble(yVal ?? double.NaN);

							if (replace && SeriesYValues[i].Count > index)
							{
								if (SeriesYValues[i][index] == yData && _isRepeatPoint)
								{
									_isRepeatPoint = true;
								}
								else
								{
									SeriesYValues[i][index] = yData;
									_isRepeatPoint = false;
								}
							}
							else
							{
								SeriesYValues[i].Insert(index, yData);
								UpdateSumOfValues(YPaths[i], (float)yData);
							}
						}

						PointsCount = xValue.Count;
					}
					else if (XValueType == ChartValueType.Double ||
					   XValueType == ChartValueType.Logarithmic)
					{
						if (XValues is not List<double>)
						{
							XValues = ActualXValues = new List<double>();
						}

						var xValue = (List<double>)XValues;
						var xVal = GetArrayPropertyValue(obj, XComplexPaths);
						XData = Convert.ToDouble(xVal ?? double.NaN);

						// Check the Data Collection is linear or not
						if (IsLinearData && (index > 0 && XData < xValue[index - 1]) || (index == 0 && xValue.Count > 0 && XData > xValue[0]))
						{
							IsLinearData = false;
						}

						if (replace && xValue.Count > index)
						{
							if (xValue[index] == XData)
							{
								_isRepeatPoint = true;
							}
							else
							{
								xValue[index] = XData;
							}
						}
						else
						{
							xValue.Insert(index, XData);
						}

						for (int i = 0; i < YPaths.Length; i++)
						{
							var yVal = YComplexPaths == null ? obj : GetArrayPropertyValue(obj, YComplexPaths[i]);
							yData = Convert.ToDouble(yVal ?? double.NaN);

							if (replace && SeriesYValues[i].Count > index)
							{
								if (SeriesYValues[i][index] == yData && _isRepeatPoint)
								{
									_isRepeatPoint = true;
								}
								else
								{
									SeriesYValues[i][index] = yData;
									_isRepeatPoint = false;
								}
							}
							else
							{
								SeriesYValues[i].Insert(index, yData);
								UpdateSumOfValues(YPaths[i], (float)yData);
							}
						}

						PointsCount = xValue.Count;
					}
					else if (XValueType == ChartValueType.DateTime)
					{
						if (XValues is not List<double>)
						{
							XValues = ActualXValues = new List<double>();
						}

						var xValue = (List<double>)XValues;
						var xVal = GetArrayPropertyValue(obj, XComplexPaths);
						XData = Convert.ToDateTime(xVal).ToOADate();

						// Check the Data Collection is linear or not
						if (IsLinearData && index > 0 && XData < xValue[index - 1])
						{
							IsLinearData = false;
						}

						if (replace && xValue.Count > index)
						{
							if (xValue[index] == XData)
							{
								_isRepeatPoint = true;
							}
							else
							{
								xValue[index] = XData;
							}
						}
						else
						{
							xValue.Insert(index, XData);
						}

						for (int i = 0; i < YPaths.Length; i++)
						{
							var yVal = YComplexPaths == null ? obj : GetArrayPropertyValue(obj, YComplexPaths[i]);
							yData = Convert.ToDouble(yVal ?? double.NaN);

							if (replace && SeriesYValues[i].Count > index)
							{
								if (SeriesYValues[i][index] == yData && _isRepeatPoint)
								{
									_isRepeatPoint = true;
								}
								else
								{
									SeriesYValues[i][index] = yData;
									_isRepeatPoint = false;
								}
							}
							else
							{
								SeriesYValues[i].Insert(index, yData);
								UpdateSumOfValues(YPaths[i], (float)yData);
							}
						}

						PointsCount = xValue.Count;
					}
					else if (XValueType == ChartValueType.TimeSpan)
					{
						//TODO: Implement on timespan support. 
					}
				}
				else
				{
					var tempYPath = YComplexPaths?[0];
					var yValue = SeriesYValues[0];

					if (XValueType == ChartValueType.String)
					{
						if (XValues is not List<string>)
						{
							XValues = ActualXValues = new List<string>();
						}

						var xValue = (List<string>)XValues;
						var xVal = GetArrayPropertyValue(obj, XComplexPaths);
						var yVal = GetArrayPropertyValue(obj, tempYPath);
						yData = yVal != null ? Convert.ToDouble(yVal) : double.NaN;

						if (replace && xValue.Count > index)
						{
							if (xValue[index] == xVal.Tostring())
							{
								_isRepeatPoint = true;
							}
							else
							{
								xValue[index] = xVal.Tostring();
							}
						}
						else
						{
							xValue.Insert(index, xVal.Tostring());
						}

						if (replace && yValue.Count > index)
						{
							if (yValue[index] == yData && _isRepeatPoint)
							{
								_isRepeatPoint = true;
							}
							else
							{
								yValue[index] = yData;
								_isRepeatPoint = false;
							}
						}
						else
						{
							yValue.Insert(index, yData);
							_sumOfYValues = float.IsNaN(_sumOfYValues) ? (float)yData : _sumOfYValues + (float)yData;
						}

						PointsCount = xValue.Count;
					}
					else if (XValueType == ChartValueType.Double ||
						XValueType == ChartValueType.Logarithmic)
					{
						if (XValues is not List<double>)
						{
							XValues = ActualXValues = new List<double>();
						}

						var xValue = (List<double>)XValues;
						var xVal = GetArrayPropertyValue(obj, XComplexPaths);
						var yVal = GetArrayPropertyValue(obj, tempYPath);
						XData = xVal != null ? Convert.ToDouble(xVal) : double.NaN;
						yData = yVal != null ? Convert.ToDouble(yVal) : double.NaN;

						// Check the Data Collection is linear or not
						if (IsLinearData && (index > 0 && XData < xValue[index - 1]) || (index == 0 && xValue.Count > 0 && XData > xValue[0]))
						{
							IsLinearData = false;
						}

						if (replace && xValue.Count > index)
						{
							if (xValue[index] == XData)
							{
								_isRepeatPoint = true;
							}
							else
							{
								xValue[index] = XData;
							}
						}
						else
						{
							xValue.Insert(index, XData);
						}

						if (replace && yValue.Count > index)
						{
							if (yValue[index] == yData && _isRepeatPoint)
							{
								_isRepeatPoint = true;
							}
							else
							{
								yValue[index] = yData;
								_isRepeatPoint = false;
							}
						}
						else
						{
							yValue.Insert(index, yData);
							_sumOfYValues = float.IsNaN(_sumOfYValues) ? (float)yData : _sumOfYValues + (float)yData;
						}

						PointsCount = xValue.Count;
					}
					else if (XValueType == ChartValueType.DateTime)
					{
						if (XValues is not List<double>)
						{
							XValues = ActualXValues = new List<double>();
						}

						var xValue = (List<double>)XValues;
						var xVal = GetArrayPropertyValue(obj, XComplexPaths);
						var yVal = GetArrayPropertyValue(obj, tempYPath);
						XData = Convert.ToDateTime(xVal).ToOADate();
						yData = yVal != null ? Convert.ToDouble(yVal) : double.NaN;

						// Check the Data Collection is linear or not
						if (IsLinearData && index > 0 && XData < xValue[index - 1])
						{
							IsLinearData = false;
						}

						if (replace && xValue.Count > index)
						{
							if (xValue[index] == XData)
							{
								_isRepeatPoint = true;
							}
							else
							{
								xValue[index] = XData;
							}
						}
						else
						{
							xValue.Insert(index, XData);
						}

						if (replace && yValue.Count > index)
						{
							if (yValue[index] == yData && _isRepeatPoint)
							{
								_isRepeatPoint = true;
							}
							else
							{
								yValue[index] = yData;
								_isRepeatPoint = false;
							}
						}
						else
						{
							yValue.Insert(index, yData);
							_sumOfYValues = float.IsNaN(_sumOfYValues) ? (float)yData : _sumOfYValues + (float)yData;
						}

						PointsCount = xValue.Count;
					}
					else if (XValueType == ChartValueType.TimeSpan)
					{
						//TODO: Ensure on time span implementation.
					}
				}

				if (ActualData != null)
				{
					if (replace && ActualData.Count > index)
					{
						ActualData[index] = obj;
					}
					else if (ActualData.Count == index)
					{
						ActualData.Add(obj);
					}
					else
					{
						ActualData.Insert(index, obj);
					}
				}
			}

			// TODO:Need to enable this method for MAUI when provide ListenPropertyChange support 
		}

		internal object? GetActualXValue(int index)
		{
			if (XValues == null || index > PointsCount)
			{
				return null;
			}

			if (XValueType == ChartValueType.String)
			{
				return ((IList<string>)XValues)[index];
			}
			else if (XValueType == ChartValueType.DateTime)
			{
				return DateTime.FromOADate(((IList<double>)XValues)[index]).ToString("MM/dd/yyyy");
			}
			else if (XValueType == ChartValueType.Double || XValueType == ChartValueType.Logarithmic)
			{
				//Logic is to cut off the 0 decimal value from the number.
				object label = ((List<double>)XValues)[index];
				var actualVal = (double)label;

				if (actualVal == (long)actualVal)
				{
					label = (long)actualVal;
				}

				return label;
			}
			else
			{
				return ((IList)XValues)[index];
			}
		}

		internal virtual void GeneratePoints(string[] yPaths, params IList<double>[] yValueLists)
		{
			if (yPaths == null)
			{
				return;
			}

			IList<double>[]? yLists = null;
			_isComplexYProperty = false;
			bool isArrayProperty = false;
			YComplexPaths = new string[yPaths.Length][];

			for (int i = 0; i < yPaths.Length; i++)
			{
				if (string.IsNullOrEmpty(yPaths[i]))
				{
					return;
				}

				YComplexPaths[i] = yPaths[i].Split(['.']);

				if (yPaths[i].Contains('.', StringComparison.Ordinal))
				{
					_isComplexYProperty = true;
				}

				if (yPaths[i].Contains('[', StringComparison.Ordinal))
				{
					isArrayProperty = true;
				}
			}

			SeriesYValues = ActualSeriesYValues = yLists = yValueLists;

			YPaths = yPaths;

			ActualData ??= [];

			if (ItemsSource != null && !string.IsNullOrEmpty(XBindingPath))
			{
				if (ItemsSource is IEnumerable)
				{
					if (XBindingPath.Contains('[', StringComparison.Ordinal) || isArrayProperty)
					{
						GenerateComplexPropertyPoints(yPaths, yLists, GetArrayPropertyValue);
					}
					else if (XBindingPath.Contains('.', StringComparison.Ordinal) || _isComplexYProperty)
					{
						GenerateComplexPropertyPoints(yPaths, yLists, GetPropertyValue);
					}
					else
					{
						GeneratePropertyPoints(yPaths, yLists);
					}
				}
			}
		}

		internal virtual void ResetData()
		{
			if (ActualXValues is IList list && XValues is IList iList)
			{
				iList.Clear();
				list.Clear();
			}

			ActualData?.Clear();

			if (ActualSeriesYValues != null && ActualSeriesYValues.Length != 0)
			{
				foreach (var list1 in ActualSeriesYValues)
				{
					list1?.Clear();
				}

				if (SeriesYValues != null)
				{
					foreach (var list2 in SeriesYValues)
					{
						list2?.Clear();
					}
				}
			}

			_sumOfYValues = float.NaN;

			if (this is FinancialSeriesBase financialSeries)
			{
				financialSeries.ResetSumOfValues();
			}

			if (this is RangeSeriesBase rangeSeries)
			{
				rangeSeries.ResetSumOfValues();
			}

			PointsCount = 0;

			if (XBindingPath != null && YPaths != null && YPaths.Length != 0)
			{
				_segments.Clear();
			}
		}

		internal virtual void GeneratePropertyPoints(string[] yPaths, IList<double>[] yLists)
		{
			var enumerable = ItemsSource as IEnumerable;
			var enumerator = enumerable?.GetEnumerator();

			if (enumerable != null && enumerator != null && enumerator.MoveNext())
			{
				var currObj = enumerator.Current;

				FastReflection xProperty = new FastReflection();

				if (!xProperty.SetPropertyName(XBindingPath, currObj) || xProperty.IsArray(currObj))
				{
					return;
				}

				XValueType = GetDataType(xProperty, enumerable);

				if (XValueType == ChartValueType.DateTime || XValueType == ChartValueType.Double ||
					XValueType == ChartValueType.Logarithmic || XValueType == ChartValueType.TimeSpan)
				{
					if (ActualXValues is not List<double>)
					{
						ActualXValues = XValues = new List<double>();
					}
				}
				else
				{
					if (ActualXValues is not List<string>)
					{
						ActualXValues = XValues = new List<string>();
					}
				}

				if (IsMultipleYPathRequired)
				{
					var yPropertyAccessor = new List<FastReflection>();
					if (string.IsNullOrEmpty(yPaths[0]))
					{
						return;
					}

					for (int i = 0; i < yPaths.Length; i++)
					{
						var fastReflection = new FastReflection();
						if (!fastReflection.SetPropertyName(yPaths[i], currObj) || fastReflection.IsArray(currObj))
						{
							return;
						}

						yPropertyAccessor.Add(fastReflection);
					}

					if (XValueType == ChartValueType.String)
					{
						if (XValues is List<string> xValue)
						{
							do
							{
								var xVal = xProperty.GetValue(enumerator.Current);
								xValue.Add(xVal.Tostring());
								for (int i = 0; i < yPropertyAccessor.Count; i++)
								{
									var yVal = yPropertyAccessor[i].GetValue(enumerator.Current);
									yLists[i].Add(Convert.ToDouble(yVal ?? double.NaN));
								}

								ActualData?.Add(enumerator.Current);
							}
							while (enumerator.MoveNext());
							PointsCount = xValue.Count;
						}
					}
					else if (XValueType == ChartValueType.Double || XValueType == ChartValueType.Logarithmic)
					{
						if (XValues is List<double> xValue)
						{
							do
							{
								var xVal = xProperty.GetValue(enumerator.Current);
								XData = Convert.ToDouble(xVal ?? double.NaN);

								// Check the Data Collection is linear or not
								if (IsLinearData && xValue.Count > 0 && XData <= xValue[^1])
								{
									IsLinearData = false;
								}

								xValue.Add(XData);
								for (int i = 0; i < yPropertyAccessor.Count; i++)
								{
									var yVal = yPropertyAccessor[i].GetValue(enumerator.Current);
									yLists[i].Add(Convert.ToDouble(yVal ?? double.NaN));
								}

								ActualData?.Add(enumerator.Current);
							}
							while (enumerator.MoveNext());
							PointsCount = xValue.Count;
						}
					}
					else if (XValueType == ChartValueType.DateTime)
					{
						if (XValues is List<double> xValue)
						{
							do
							{
								var xVal = xProperty.GetValue(enumerator.Current);
								XData = xVal != null ? ((DateTime)xVal).ToOADate() : double.NaN;

								// Check the Data Collection is linear or not
								if (IsLinearData && xValue.Count > 0 && XData <= xValue[^1])
								{
									IsLinearData = false;
								}

								xValue.Add(XData);
								for (int i = 0; i < yPropertyAccessor.Count; i++)
								{
									var yVal = yPropertyAccessor[i].GetValue(enumerator.Current);
									yLists[i].Add(Convert.ToDouble(yVal ?? double.NaN));
								}

								ActualData?.Add(enumerator.Current);
							}
							while (enumerator.MoveNext());
							PointsCount = xValue.Count;
						}
					}
					else if (XValueType == ChartValueType.TimeSpan)
					{
						//TODO: Ensure while providing timespan support.
					}
				}
				else
				{
					string yPath;

					if (string.IsNullOrEmpty(yPaths[0]))
					{
						return;
					}
					else
					{
						yPath = yPaths[0];
					}

					var yProperty = new FastReflection();

					if (!yProperty.SetPropertyName(yPath, currObj) || yProperty.IsArray(currObj))
					{
						return;
					}

					IList<double> yValue = yLists[0];

					if (XValueType == ChartValueType.String)
					{
						if (XValues is List<string> xValue)
						{
							do
							{
								var xVal = xProperty.GetValue(enumerator.Current);
								var yVal = yProperty.GetValue(enumerator.Current);
								xValue.Add(xVal.Tostring());
								yValue.Add(Convert.ToDouble(yVal ?? double.NaN));
								ActualData?.Add(enumerator.Current);
							}
							while (enumerator.MoveNext());
							PointsCount = xValue.Count;
						}
					}
					else if (XValueType == ChartValueType.DateTime)
					{
						if (XValues is List<double> xValue)
						{
							do
							{
								var xVal = xProperty.GetValue(enumerator.Current);
								var yVal = yProperty.GetValue(enumerator.Current);

								XData = xVal != null ? ((DateTime)xVal).ToOADate() : double.NaN;

								// Check the Data Collection is linear or not
								if (IsLinearData && xValue.Count > 0 && XData <= xValue[^1])
								{
									IsLinearData = false;
								}

								xValue.Add(XData);
								yValue.Add(Convert.ToDouble(yVal ?? double.NaN));
								ActualData?.Add(enumerator.Current);
							}
							while (enumerator.MoveNext());
							PointsCount = xValue.Count;
						}
					}
					else if (XValueType == ChartValueType.Double || XValueType == ChartValueType.Logarithmic)
					{
						if (XValues is List<double> xValue)
						{
							do
							{
								var xVal = xProperty.GetValue(enumerator.Current);
								var yVal = yProperty.GetValue(enumerator.Current);
								XData = Convert.ToDouble(xVal ?? double.NaN);

								// Check the Data Collection is linear or not
								if (IsLinearData && xValue.Count > 0 && XData <= xValue[^1])
								{
									IsLinearData = false;
								}

								xValue.Add(XData);
								yValue.Add(Convert.ToDouble(yVal ?? double.NaN));
								ActualData?.Add(enumerator.Current);
							}
							while (enumerator.MoveNext());
							PointsCount = xValue.Count;
						}
					}
					else if (XValueType == ChartValueType.TimeSpan)
					{
						//TODO: ensure while implementing timespan.
					}
				}
			}
		}

		internal virtual void GenerateComplexPropertyPoints(string[] yPaths, IList<double>[] yLists, GetReflectedProperty? getPropertyValue)
		{
			var enumerable = ItemsSource as IEnumerable;
			var enumerator = enumerable?.GetEnumerator();

			if (enumerable != null && enumerator != null && getPropertyValue != null && enumerator.MoveNext() && XComplexPaths != null && YComplexPaths != null)
			{
				XValueType = GetDataType(enumerator, XComplexPaths);
				if (XValueType == ChartValueType.DateTime || XValueType == ChartValueType.Double ||
					XValueType == ChartValueType.Logarithmic || XValueType == ChartValueType.TimeSpan)
				{
					if (XValues is not List<double>)
					{
						ActualXValues = XValues = new List<double>();
					}
				}
				else
				{
					if (XValues is not List<string>)
					{
						ActualXValues = XValues = new List<string>();
					}
				}

				if (IsMultipleYPathRequired)
				{
					if (string.IsNullOrEmpty(yPaths[0]))
					{
						return;
					}

					object? xVal = getPropertyValue(enumerator.Current, XComplexPaths);
					if (xVal == null)
					{
						return;
					}

					for (int i = 0; i < yPaths.Length; i++)
					{
						var yPropertyValue = getPropertyValue(enumerator.Current, YComplexPaths[i]);
						if (yPropertyValue == null)
						{
							return;
						}
					}

					if (XValueType == ChartValueType.String)
					{
						if (XValues is List<string> xValue)
						{
							do
							{
								xVal = getPropertyValue(enumerator.Current, XComplexPaths);
								xValue.Add(xVal.Tostring());
								for (int i = 0; i < yPaths.Length; i++)
								{
									var yVal = getPropertyValue(enumerator.Current, YComplexPaths[i]);
									yLists[i].Add(Convert.ToDouble(yVal ?? double.NaN));
								}

								ActualData?.Add(enumerator.Current);
							}
							while (enumerator.MoveNext());
							PointsCount = xValue.Count;
						}
					}
					else if (XValueType == ChartValueType.Double || XValueType == ChartValueType.Logarithmic)
					{
						if (XValues is List<double> xValue)
						{
							do
							{
								xVal = getPropertyValue(enumerator.Current, XComplexPaths);
								XData = Convert.ToDouble(xVal ?? double.NaN);

								// Check the Data Collection is linear or not
								if (IsLinearData && xValue.Count > 0 && XData <= xValue[^1])
								{
									IsLinearData = false;
								}

								xValue.Add(XData);
								for (int i = 0; i < yPaths.Length; i++)
								{
									var yVal = getPropertyValue(enumerator.Current, YComplexPaths[i]);
									yLists[i].Add(Convert.ToDouble(yVal ?? double.NaN));
								}

								ActualData?.Add(enumerator.Current);
							}
							while (enumerator.MoveNext());
							PointsCount = xValue.Count;
						}
					}
					else if (XValueType == ChartValueType.DateTime)
					{
						if (XValues is List<double> xValue)
						{
							do
							{
								xVal = getPropertyValue(enumerator.Current, XComplexPaths);
								XData = xVal != null ? ((DateTime)xVal).ToOADate() : double.NaN;

								// Check the Data Collection is linear or not
								if (IsLinearData && xValue.Count > 0 && XData <= xValue[^1])
								{
									IsLinearData = false;
								}

								xValue.Add(XData);
								for (int i = 0; i < yPaths.Length; i++)
								{
									var yVal = getPropertyValue(enumerator.Current, YComplexPaths[i]);
									yLists[i].Add(Convert.ToDouble(yVal ?? double.NaN));
								}

								ActualData?.Add(enumerator.Current);
							}
							while (enumerator.MoveNext());
							PointsCount = xValue.Count;
						}
					}
					else if (XValueType == ChartValueType.TimeSpan)
					{
						//TODO:Validate on timespan implementation.
					}
				}
				else
				{
					string[] tempYPath = YComplexPaths[0];
					if (string.IsNullOrEmpty(yPaths[0]))
					{
						return;
					}

					IList<double> yValue = yLists[0];
					object? xVal, yVal;
					if (XValueType == ChartValueType.String)
					{
						if (XValues is List<string> xValue)
						{
							do
							{
								xVal = getPropertyValue(enumerator.Current, XComplexPaths);
								yVal = getPropertyValue(enumerator.Current, tempYPath);
								if (xVal == null)
								{
									return;
								}

								xValue.Add((string)xVal);
								yValue.Add(Convert.ToDouble(yVal ?? double.NaN));
								ActualData?.Add(enumerator.Current);
							}
							while (enumerator.MoveNext());
							PointsCount = xValue.Count;
						}
					}
					else if (XValueType == ChartValueType.Double || XValueType == ChartValueType.Logarithmic)
					{
						if (XValues is List<double> xValue)
						{
							do
							{
								xVal = getPropertyValue(enumerator.Current, XComplexPaths);
								yVal = getPropertyValue(enumerator.Current, tempYPath);
								XData = Convert.ToDouble(xVal ?? double.NaN);

								// Check the Data Collection is linear or not
								if (IsLinearData && xValue.Count > 0 && XData <= xValue[^1])
								{
									IsLinearData = false;
								}

								xValue.Add(XData);
								yValue.Add(Convert.ToDouble(yVal ?? double.NaN));
								ActualData?.Add(enumerator.Current);
							}
							while (enumerator.MoveNext());
							PointsCount = xValue.Count;
						}
					}
					else if (XValueType == ChartValueType.DateTime)
					{
						if (XValues is List<double> xValue)
						{
							do
							{
								xVal = getPropertyValue(enumerator.Current, XComplexPaths);
								yVal = getPropertyValue(enumerator.Current, tempYPath);

								XData = xVal != null ? ((DateTime)xVal).ToOADate() : double.NaN;

								// Check the Data Collection is linear or not
								if (IsLinearData && xValue.Count > 0 && XData <= xValue[^1])
								{
									IsLinearData = false;
								}

								xValue.Add(XData);
								yValue.Add(Convert.ToDouble(yVal ?? double.NaN));
								ActualData?.Add(enumerator.Current);
							}
							while (enumerator.MoveNext());
							PointsCount = xValue.Count;
						}
					}
					else if (XValueType == ChartValueType.TimeSpan)
					{
						//TODO: Ensure for timespan;
					}
				}
			}
		}

		internal static ChartValueType GetDataType(object? xValue)
		{
			if (xValue is string || xValue is string[])
			{
				return ChartValueType.String;
			}
			else if (xValue is DateTime || xValue is DateTime[])
			{
				return ChartValueType.DateTime;
			}
			else if (xValue is TimeSpan || xValue is TimeSpan[])
			{
				return ChartValueType.TimeSpan;
			}
			else
			{
				return ChartValueType.Double;
			}
		}

		internal static ChartValueType GetDataType(IEnumerator enumerator, string[] paths)
		{
			// GetArrayPropertyValue method is used to get value from the path of current object
			object? parentObj = GetArrayPropertyValue(enumerator.Current, paths);

			return GetDataType(parentObj);
		}

		internal static ChartValueType GetDataType(FastReflection fastReflection, IEnumerable dataSource)
		{
			if (dataSource == null)
			{
				return ChartValueType.Double;
			}

			var enumerator = dataSource.GetEnumerator();
			object? obj = null;

			if (enumerator.MoveNext())
			{
				do
				{
					obj = fastReflection.GetValue(enumerator.Current);
				}
				while (enumerator.MoveNext() && obj == null);
			}

			return GetDataType(obj);
		}

		internal static object? GetArrayPropertyValue(object obj, string[]? paths)
		{
			var parentObj = obj;

			if (paths == null)
			{
				return parentObj;
			}

			for (int i = 0; i < paths.Length; i++)
			{
				var path = paths[i];
				if (path.Contains('[', StringComparison.Ordinal))
				{
					int index = Convert.ToInt32(path.Substring(path.IndexOf('[', StringComparison.Ordinal) + 1, path.IndexOf(']', StringComparison.Ordinal) - path.IndexOf('[', StringComparison.Ordinal) - 1));
					string actualPath = path.Replace(path[path.IndexOf('[', StringComparison.Ordinal)..], string.Empty, StringComparison.Ordinal);
					parentObj = ReflectedObject(parentObj, actualPath);

					if (parentObj == null)
					{
						return null;
					}

					if (parentObj is IList array && array.Count > index)
					{
						parentObj = array[index];
					}
					else
					{
						return null;
					}
				}
				else
				{
					parentObj = ReflectedObject(parentObj, path);

					if (parentObj == null)
					{
						return null;
					}

					if (parentObj.GetType().IsArray)
					{
						return null;
					}
				}
			}

			return parentObj;
		}

		#endregion

		#region Private Methods

		static object? ReflectedObject(object? parentObj, string actualPath)
		{
			var fastReflection = new FastReflection();
			if (parentObj != null && fastReflection.SetPropertyName(actualPath, parentObj))
			{
				return fastReflection.GetValue(parentObj);
			}

			return null;
		}

		static object? GetPropertyValue(object obj, string[] paths)
		{
			object? parentObj = obj;
			for (int i = 0; i < paths.Length; i++)
			{
				parentObj = ReflectedObject(parentObj, paths[i]);
			}

			if (parentObj != null)
			{
				if (parentObj.GetType().IsArray)
				{
					return null;
				}
			}

			return parentObj;
		}

		void ResetDataPoint()
		{
			ResetData();

			if (ItemsSource != null)
			{
				var items = ItemsSource is IList ? ItemsSource as IList : null;
				if (items == null)
				{
					if (ItemsSource is IEnumerable source)
					{
						items = source.Cast<object>().ToList();
					}
				}

				if (items != null && items.Count > 0)
				{
					GenerateDataPoints();
				}
			}
		}

		void UpdateSumOfValues(string yPath, float yValue)
		{
			if (this is FinancialSeriesBase financialSeries)
			{
				financialSeries.SumOfValuesDynamicAdd(yPath, yValue);
			}
			else if (this is RangeSeriesBase rangeSeries)
			{
				rangeSeries.SumOfValuesDynamicAdd(yPath, yValue);
			}
			else
			{
				_sumOfYValues = float.IsNaN(_sumOfYValues) ? yValue : _sumOfYValues + yValue;
			}
		}

		void RemoveYValue(int index, string yPath, IList<double> seriesYValues)
		{
			if (this is FinancialSeriesBase financialSeries)
			{
				financialSeries.SumOfValuesDynmaicRemove(yPath, (float)seriesYValues[index]);
			}

			if (this is RangeSeriesBase rangeSeries)
			{
				rangeSeries.SumOfValuesDynmaicRemove(yPath, (float)seriesYValues[index]);
			}
			else
			{
				_sumOfYValues -= (float)seriesYValues[index];
			}
		}

		#endregion

		#endregion
	}
}