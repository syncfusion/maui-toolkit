using Syncfusion.Maui.Toolkit.Graphics.Internals;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Syncfusion.Maui.Toolkit.Charts
{
	public partial class ChartSeries
	{
		#region Internal Fields

		internal delegate object? GetReflectedProperty(object obj, string[] paths);
		internal float _sumOfYValues = float.NaN;
		internal readonly ObservableCollection<ChartSegment> _segments;
		internal readonly ReadOnlyObservableCollection<ChartSegment> _readOnlySegments;

		#endregion

		#region Private Fields

		bool _isComplexYProperty;

		bool _isComplexXProperty;

		ChartValueType _xValueType;

		bool _isRepeatPoint;

		bool isComplexColorProperty;

		#endregion

		#region Internal Properties

		internal virtual bool IsMultipleYPathRequired
		{
			get
			{
				return false;
			}
		}

		internal virtual bool IsSideBySide => false;

		internal virtual bool IsColorPathSeries => true;

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

		internal IList<Brush?> PointColorValues { get; set; } = new List<Brush?>();

		internal string[]? YPaths { get; private set; }

		internal List<object>? ActualData { get; set; }

		internal string[]? XComplexPaths { get; set; }

		internal string[]? ColorComplexPaths { get; set; }

		internal bool IsLinearData { get; set; } = true;

		internal bool IsDataPointAddedDynamically { get; set; }

		internal virtual bool IsDataLevelLegendSeries => false;

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
			e.ApplyCollectionChanges((obj, index, canInsert) => AddDataPoint(obj, index, e), (obj, index) => RemoveData(index, e), ResetDataPoint);

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

			if (!string.IsNullOrEmpty(PointColorPath) && IsColorPathSeries)
				PointColorValues.RemoveAt(index);

			ActualData?.RemoveAt(index);

			if (e.OldItems is not null)
			{
				UnhookPropertyChangedEvent(ListenPropertyChange, e.OldItems[0]);
			}
		}

		internal virtual void UpdateRange()
		{
		}

		internal virtual void UnhookPropertyChangedEvent(object oldValue)
		{
			if (oldValue is IEnumerable enumerable)
			{
				foreach (var item in enumerable)
				{
					if (item is INotifyPropertyChanged notifyItem)
					{
						notifyItem.PropertyChanged -= OnItemPropertyChanged;
					}
				}
			}
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
						for (int i = 0; i < categoryAxis.RegisteredSeries.Count; i++)
						{
							if (categoryAxis.RegisteredSeries[i] is CartesianSeries chartSeries)
							{
								chartSeries.SegmentsCreated = false;
								chartSeries.ChartArea?.UpdateVisibleSeries();
							}
						}
					}
				}
			}
		}

		//TODO:Need to remove the replace parameter from this method,
		//because new notify collectionChanged event first remove
		//the data and then insert the data. So no need replace parameter here after.
		internal virtual void SetIndividualPoint(object obj, int index, bool replace)
		{
			if (SeriesYValues == null || YPaths == null || ItemsSource == null)
			{
				return;
			}

			var colorValues = PointColorValues;
			var xvalueType = GetArrayPropertyValue(obj, XComplexPaths);
			if (xvalueType != null)
				XValueType = GetDataType(xvalueType);

			// Set up X values list based on type
			if (XValueType == ChartValueType.DateTime || XValueType == ChartValueType.Double ||
				XValueType == ChartValueType.Logarithmic || XValueType == ChartValueType.TimeSpan)
			{
				if (!(this.XValues is List<double>))
					this.XValues = this.ActualXValues = new List<double>();
			}
			else
			{
				if (!(this.XValues is List<string>))
					this.XValues = this.ActualXValues = new List<string>();
			}

			// Define action to set/insert X value based on type
			Action setXValue;
			var xValueDouble = this.XValues as List<double>;
			var xValueString = this.XValues as List<string>;

			switch (XValueType)
			{
				case ChartValueType.String:
					setXValue = () =>
					{
						var xVal = GetArrayPropertyValue(obj, XComplexPaths);
						var xData = xVal as string;
						if (replace && xValueString != null && xValueString.Count > index)
						{
							if (xValueString[index] == xData)
								_isRepeatPoint = true;
							else
								xValueString[index] = xData?.ToString() ?? "";
						}
						else
						{
							xValueString?.Insert(index, xData?.ToString() ?? "");
						}
					};
					break;
				case ChartValueType.Double:
				case ChartValueType.Logarithmic:
					setXValue = () =>
					{
						var xVal = GetArrayPropertyValue(obj, XComplexPaths);
						XData = Convert.ToDouble(xVal ?? double.NaN);
						if (IsLinearData && ((index > 0 && xValueDouble != null && XData <= xValueDouble[index - 1]) || (index == 0 && xValueDouble != null && xValueDouble.Count > 0 && XData > xValueDouble[0])))
						{
							IsLinearData = false;
						}
						if (replace && xValueDouble != null && xValueDouble.Count > index)
						{
							if (xValueDouble[index] == XData)
								_isRepeatPoint = true;
							else
								xValueDouble[index] = XData;
						}
						else
						{
							xValueDouble?.Insert(index, XData);
						}
					};
					break;
				case ChartValueType.DateTime:
					setXValue = () =>
					{
						var xVal = GetArrayPropertyValue(obj, XComplexPaths);
						XData = Convert.ToDateTime(xVal).ToOADate();
						if (IsLinearData && index > 0 && xValueDouble != null && XData <= xValueDouble[index - 1])
						{
							IsLinearData = false;
						}
						if (replace && xValueDouble != null && xValueDouble.Count > index)
						{
							if (xValueDouble[index] == XData)
								_isRepeatPoint = true;
							else
								xValueDouble[index] = XData;
						}
						else
						{
							xValueDouble?.Insert(index, XData);
						}
					};
					break;
				case ChartValueType.TimeSpan:
					// TODO: Implement TimeSpan support
					return;
				default:
					return;
			}

			// Define action to set/insert Y values
			Action setYValues;
			if (IsMultipleYPathRequired)
			{
				setYValues = () =>
				{
					for (int i = 0; i < YPaths.Count(); i++)
					{
						var yVal = YComplexPaths == null ? obj : GetArrayPropertyValue(obj, YComplexPaths[i]);
						double yData = Convert.ToDouble(yVal ?? double.NaN);
						if (replace && SeriesYValues[i].Count > index)
						{
							if (SeriesYValues[i][index] == yData && _isRepeatPoint)
								_isRepeatPoint = true;
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
				};
			}
			else
			{
				var tempYPath = YComplexPaths?[0];
				var yValue = SeriesYValues[0];
				setYValues = () =>
				{
					var yVal = GetArrayPropertyValue(obj, tempYPath);
					double yData = Convert.ToDouble(yVal ?? double.NaN);
					if (replace && yValue.Count > index)
					{
						if (yValue[index] == yData && _isRepeatPoint)
							_isRepeatPoint = true;
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
				};
			}

			// Initialize color action with no-op lambda to avoid null reference warnings
			Action setColorValue = () => { };
			if (!string.IsNullOrEmpty(PointColorPath) && IsColorPathSeries)
			{
				setColorValue = () =>
				{
					var colorValue = GetArrayPropertyValue(obj, ColorComplexPaths);
					Brush? color = GetBrushFromColor(colorValue);
					if (replace && colorValues != null && colorValues.Count > index)
					{
						if (colorValues[index] == color && _isRepeatPoint)
							_isRepeatPoint = true;
						else
						{
							colorValues[index] = color;
							_isRepeatPoint = false;
						}
					}
					else
					{
						colorValues?.Insert(index, color);
					}
				};
			}

			// Execute the actions
			setXValue();
			setYValues();
			setColorValue();

			// Update ActualData
			if (ActualData != null)
			{
				if (replace && ActualData.Count > index)
					ActualData[index] = obj;
				else if (ActualData.Count == index)
					ActualData.Add(obj);
				else
					ActualData.Insert(index, obj);
			}

			// Update PointsCount
			PointsCount = (XValueType == ChartValueType.String ? xValueString?.Count : xValueDouble?.Count) ?? 0;

			// TODO: Need to enable this method for MAUI when provide ListenPropertyChange support
			HookPropertyChangedEvent(ListenPropertyChange, obj);

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
				if (this is CartesianSeries)
				{
					return DateTime.FromOADate(((IList<double>)XValues)[index]);
				}
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
			_isComplexXProperty = !string.IsNullOrEmpty(XBindingPath) && XBindingPath.Contains('.', StringComparison.Ordinal);
			isComplexColorProperty = false;
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

			// Detect if PointColorPath is complex (contains "." or "[")
			if (!string.IsNullOrEmpty(PointColorPath))
			{
				if (PointColorPath.Contains(".", StringComparison.Ordinal) || PointColorPath.Contains("[", StringComparison.Ordinal))
				{
					isComplexColorProperty = true;
				}
			}

			SeriesYValues = ActualSeriesYValues = yLists = yValueLists;

			YPaths = yPaths;

			ActualData ??= [];

			if (ItemsSource != null && !string.IsNullOrEmpty(XBindingPath))
			{
				if (ItemsSource is IEnumerable)
				{
					if (XBindingPath.Contains('[', StringComparison.Ordinal) || isArrayProperty || isComplexColorProperty)
					{
						GenerateComplexPropertyPoints(yPaths, yLists, GetArrayPropertyValue);
					}
					else if (_isComplexXProperty || _isComplexYProperty || isComplexColorProperty)
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

			// Clear PointColorValues
			if (PointColorValues != null)
			{
				PointColorValues.Clear();
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

			if (enumerable == null || enumerator == null || !enumerator.MoveNext())
			{
				return;
			}

			var currObj = enumerator.Current;

			FastReflection xProperty = new FastReflection();
			if (!xProperty.SetPropertyName(XBindingPath, currObj) || xProperty.IsArray(currObj))
			{
				return;
			}

			// Initialize color property accessor if PointColorPath is set
			FastReflection? colorProperty = null;
			if (!string.IsNullOrEmpty(PointColorPath) && IsColorPathSeries)
			{
				colorProperty = new FastReflection();
				if (!colorProperty.SetPropertyName(PointColorPath, currObj) || colorProperty.IsArray(currObj))
				{
					colorProperty = null;
				}
			}

			XValueType = GetDataType(xProperty, enumerable);

			// Set up X values list based on type
			if (XValueType == ChartValueType.DateTime || XValueType == ChartValueType.Double ||
				XValueType == ChartValueType.Logarithmic || XValueType == ChartValueType.TimeSpan)
			{
				if (!(ActualXValues is List<double>))
				{
					this.ActualXValues = this.XValues = new List<double>();
				}
			}
			else
			{
				if (!(ActualXValues is List<string>))
				{
					this.ActualXValues = this.XValues = new List<string>();
				}
			}

			// Set up Y properties
			var yProperties = new List<FastReflection>();
			if (IsMultipleYPathRequired)
			{
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
					yProperties.Add(fastReflection);
				}
			}
			else
			{
				if (string.IsNullOrEmpty(yPaths[0]))
				{
					return;
				}

				var yProperty = new FastReflection();
				if (!yProperty.SetPropertyName(yPaths[0], currObj) || yProperty.IsArray(currObj))
				{
					return;
				}
				yProperties.Add(yProperty);
			}

			// Define action to add X value based on type
			Action<object> addXValue;
			var xValueDouble = this.XValues as List<double>;
			var xValueString = this.XValues as List<string>;

			switch (XValueType)
			{
				case ChartValueType.String:
					addXValue = (obj) =>
					{
						var xVal = xProperty.GetValue(obj);
						xValueString?.Add(xVal?.ToString() ?? "");
					};
					break;
				case ChartValueType.Double:
				case ChartValueType.Logarithmic:
					addXValue = (obj) =>
					{
						var xVal = xProperty.GetValue(obj);
						XData = Convert.ToDouble(xVal ?? double.NaN);
						if (IsLinearData && xValueDouble != null && xValueDouble.Count > 0 && XData <= xValueDouble[xValueDouble.Count - 1])
						{
							IsLinearData = false;
						}
						xValueDouble?.Add(XData);
					};
					break;
				case ChartValueType.DateTime:
					addXValue = (obj) =>
					{
						var xVal = xProperty.GetValue(obj);
						XData = xVal != null ? ((DateTime)xVal).ToOADate() : double.NaN;
						if (IsLinearData && xValueDouble != null && xValueDouble.Count > 0 && XData <= xValueDouble[xValueDouble.Count - 1])
						{
							IsLinearData = false;
						}
						xValueDouble?.Add(XData);
					};
					break;
				case ChartValueType.TimeSpan:
					// TODO: Implement TimeSpan support
					return;
				default:
					return;
			}

			Action<object>? addPointColorAction = (obj) =>
			{
				if (PointColorValues == null)
				{
					PointColorValues = new List<Brush?>();
				}

				if (colorProperty != null)
				{
					var colorVal = colorProperty.GetValue(obj);
					PointColorValues.Add(GetBrushFromColor(colorVal));
				}
			};

			// Define the loop body action (prevents per-iteration checks)
			Action processItem;
			if (!string.IsNullOrEmpty(PointColorPath) && IsColorPathSeries)
			{
				processItem = () =>
				{
					var current = enumerator.Current;
					addXValue(current);
					for (int i = 0; i < yProperties.Count; i++)
					{
						var yVal = yProperties[i].GetValue(current);
						yLists[i].Add(Convert.ToDouble(yVal ?? double.NaN));
					}
					addPointColorAction(current);
					ActualData?.Add(current);
				};
			}
			else
			{
				processItem = () =>
				{
					var current = enumerator.Current;
					addXValue(current);
					for (int i = 0; i < yProperties.Count; i++)
					{
						var yVal = yProperties[i].GetValue(current);
						yLists[i].Add(Convert.ToDouble(yVal ?? double.NaN));
					}
					ActualData?.Add(current);
				};
			}

			// Process all items (no conditional checks inside the loop)
			do
			{
				processItem();
			}
			while (enumerator.MoveNext());

			PointsCount = (XValueType == ChartValueType.String ? xValueString?.Count : xValueDouble?.Count) ?? 0;
			HookPropertyChangedEvent(ListenPropertyChange);
		}

		internal virtual void GenerateComplexPropertyPoints(string[] yPaths, IList<double>[] yLists, GetReflectedProperty? getPropertyValue)
		{
			var enumerable = ItemsSource as IEnumerable;
			var enumerator = enumerable?.GetEnumerator();

			if (enumerable == null || enumerator == null || getPropertyValue == null ||
				!enumerator.MoveNext() || XComplexPaths == null || YComplexPaths == null)
			{
				return;
			}

			// Initialize color paths if PointColorPath is set
			string[]? colorComplexPaths = null;
			if (!string.IsNullOrEmpty(PointColorPath) && IsColorPathSeries)
			{
				colorComplexPaths = PointColorPath.Split(new char[] { '.' });
			}

			XValueType = GetDataType(enumerator, XComplexPaths);

			// Set up X values list based on type
			if (XValueType == ChartValueType.DateTime || XValueType == ChartValueType.Double ||
				XValueType == ChartValueType.Logarithmic || XValueType == ChartValueType.TimeSpan)
			{
				if (!(XValues is List<double>))
				{
					this.ActualXValues = this.XValues = new List<double>();
				}
			}
			else
			{
				if (!(XValues is List<string>))
				{
					this.ActualXValues = this.XValues = new List<string>();
				}
			}

			// Validate paths and initial values
			if (string.IsNullOrEmpty(yPaths[0]))
			{
				return;
			}

			object? xVal = getPropertyValue(enumerator.Current, XComplexPaths);
			if (xVal == null)
			{
				return;
			}

			// For multiple Y paths, validate all of them
			if (IsMultipleYPathRequired)
			{
				for (int i = 0; i < yPaths.Count(); i++)
				{
					var yPropertyValue = getPropertyValue(enumerator.Current, YComplexPaths[i]);
					if (yPropertyValue == null)
					{
						return;
					}
				}
			}

			// Define action to add X value based on type
			Action<object> addXValue;
			var xValueDouble = this.XValues as List<double>;
			var xValueString = this.XValues as List<string>;

			switch (XValueType)
			{
				case ChartValueType.String:
					addXValue = (obj) =>
					{
						var xVal = getPropertyValue(obj, XComplexPaths);
						xValueString?.Add(xVal?.ToString() ?? "");
					};
					break;
				case ChartValueType.Double:
				case ChartValueType.Logarithmic:
					addXValue = (obj) =>
					{
						var xVal = getPropertyValue(obj, XComplexPaths);
						XData = Convert.ToDouble(xVal ?? double.NaN);
						if (IsLinearData && xValueDouble != null && xValueDouble.Count > 0 && XData <= xValueDouble[xValueDouble.Count - 1])
						{
							IsLinearData = false;
						}
						xValueDouble?.Add(XData);
					};
					break;
				case ChartValueType.DateTime:
					addXValue = (obj) =>
					{
						var xVal = getPropertyValue(obj, XComplexPaths);
						XData = xVal != null ? ((DateTime)xVal).ToOADate() : double.NaN;
						if (IsLinearData && xValueDouble != null && xValueDouble.Count > 0 && XData <= xValueDouble[xValueDouble.Count - 1])
						{
							IsLinearData = false;
						}
						xValueDouble?.Add(XData);
					};
					break;
				case ChartValueType.TimeSpan:
					// TODO: Implement TimeSpan support
					return;
				default:
					return;
			}

			// Initialize color action with no-op lambda to avoid null reference warnings
			Action<object> addPointColorAction = (obj) =>
			{
				if (PointColorValues == null)
				{
					PointColorValues = new List<Brush?>();
				}

				if (colorComplexPaths != null && getPropertyValue != null)
				{
					var colorVal = getPropertyValue(obj, colorComplexPaths);
					PointColorValues.Add(GetBrushFromColor(colorVal));
				}
			};

			// Define the loop body action based on whether we have color path
			Action processItem;
			if (!string.IsNullOrEmpty(PointColorPath) && IsColorPathSeries)
			{
				processItem = () =>
				{
					var current = enumerator.Current;
					addXValue(current);
					for (int i = 0; i < yPaths.Count(); i++)
					{
						var yVal = getPropertyValue(current, YComplexPaths[i]);
						yLists[i].Add(Convert.ToDouble(yVal ?? double.NaN));
					}
					addPointColorAction(current);
					ActualData?.Add(current);
				};
			}
			else
			{
				processItem = () =>
				{
					var current = enumerator.Current;
					addXValue(current);
					for (int i = 0; i < yPaths.Count(); i++)
					{
						var yVal = getPropertyValue(current, YComplexPaths[i]);
						yLists[i].Add(Convert.ToDouble(yVal ?? double.NaN));
					}
					ActualData?.Add(current);
				};
			}

			// Process all items (no conditional checks inside the loop)
			do
			{
				processItem();
			}
			while (enumerator.MoveNext());

			PointsCount = (XValueType == ChartValueType.String ? xValueString?.Count : xValueDouble?.Count) ?? 0;

			HookPropertyChangedEvent(ListenPropertyChange);
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
					int bracketOpen = path.IndexOf('[', StringComparison.Ordinal);
					int bracketClose = path.IndexOf(']', StringComparison.Ordinal);
					int index = Convert.ToInt32(path.Substring(bracketOpen + 1, bracketClose - bracketOpen - 1));
					string actualPath = path.Replace(path[bracketOpen..], string.Empty, StringComparison.Ordinal);
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

		internal void HookPropertyChangedEvent(bool listenToPropertyChange)
		{
			if (ItemsSource is not IEnumerable enumerable)
			{
				return;
			}

			var enumerator = enumerable.GetEnumerator();

			if (!enumerator.MoveNext())
			{
				return;
			}

			if (enumerator.Current is INotifyPropertyChanged)
			{
				do
				{
					if (enumerator.Current is INotifyPropertyChanged notifyPropertyChanged)
					{
						if (listenToPropertyChange)
						{
							if (_isComplexYProperty || _isComplexXProperty)
							{
								HookComplexProperty(enumerator.Current, XComplexPaths!);

								for (int i = 0; i < YComplexPaths!.Length; i++)
								{
									HookComplexProperty(enumerator.Current, YComplexPaths[i]);
								}
							}

							notifyPropertyChanged.PropertyChanged -= OnItemPropertyChanged;
							notifyPropertyChanged.PropertyChanged += OnItemPropertyChanged;
						}
						else
						{
							notifyPropertyChanged.PropertyChanged -= OnItemPropertyChanged;
						}
					}
				} while (enumerator.MoveNext());
			}
		}

		internal void HookPropertyChangedEvent(bool listenToPropertyChange, object obj)
		{
			if (listenToPropertyChange)
			{
				if (obj is INotifyPropertyChanged model)
				{
					model.PropertyChanged -= OnItemPropertyChanged;
					model.PropertyChanged += OnItemPropertyChanged;
				}
			}
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
				bool hasData = false;
				if (ItemsSource is IList list)
				{
					hasData = list.Count > 0;
				}
				else if (ItemsSource is IEnumerable source)
				{
					var enumerator = source.GetEnumerator();
					hasData = enumerator.MoveNext();
					(enumerator as IDisposable)?.Dispose();
				}

				if (hasData)
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

		void OnItemPropertyChanged(object? sender, PropertyChangedEventArgs e)
		{
			if (sender is null)
			{
				return;
			}

			if (_isComplexYProperty || _isComplexXProperty || isComplexColorProperty)
			{
				ComplexPropertyChanged(sender, e);
			}
			else if (XBindingPath == e.PropertyName || YPaths != null && YPaths.Contains(e.PropertyName) || PointColorPath == e.PropertyName)
			{
				int position = -1;

				var itemsSource = ItemsSource as IEnumerable;

				foreach (object obj in itemsSource!)
				{
					position++;

					if (obj == sender)
					{
						break;
					}
				}

				if (position != -1)
				{
					SetIndividualPoint(sender, position, true);
				}

				SegmentsCreated = false;

				UpdateLegendItems();

				if (!_isRepeatPoint)
				{
					ScheduleUpdateChart();
				}
			}
		}

		void ComplexPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			int position = -1;
			object? parentObj = null;
			var complexPaths = XComplexPaths;
			bool isYPath = false;

			for (int i = 0; i < YPaths!.Length; i++)
			{
				if (YPaths[i].Contains(e.PropertyName!, StringComparison.Ordinal))
				{
					isYPath = true;

					if (isYPath)
					{
						complexPaths = YComplexPaths![i];
					}

					break;
				}
			}

			if (XBindingPath.Contains(e.PropertyName!, StringComparison.Ordinal) || isYPath || PointColorPath.Contains(e.PropertyName!, StringComparison.Ordinal))
			{
				IEnumerable enumerable = (IEnumerable)ItemsSource;

				foreach (object obj in enumerable)
				{
					parentObj = obj;

					for (int i = 0; i < complexPaths!.Length - 1; i++)
					{
						parentObj = ReflectedObject(parentObj, complexPaths[i]);
					}

					position++;

					if (parentObj == sender)
					{
						parentObj = obj;
						break;
					}
				}

				if (position != -1 && parentObj is not null)
				{
					SetIndividualPoint(parentObj, position, true);
				}

				if (isYPath)
				{
					SegmentsCreated = false;
				}

				UpdateLegendItems();

				ScheduleUpdateChart();
			}
		}

		void HookComplexProperty(object? parentObj, string[] paths)
		{
			for (int i = 0; i < paths.Length; i++)
			{
				parentObj = ReflectedObject(parentObj, paths[i]);

				if (parentObj is INotifyPropertyChanged notifiableObject)
				{
					notifiableObject.PropertyChanged -= OnItemPropertyChanged;
					notifiableObject.PropertyChanged += OnItemPropertyChanged;
				}
			}
		}

		/// <summary>
		/// Method to unhook the PropertyChange event for individual data point
		/// </summary>
		void UnhookPropertyChangedEvent(bool listenToPropertyChange, object? oldValue)
		{
			if (oldValue is INotifyPropertyChanged model && listenToPropertyChange)
			{
				model.PropertyChanged -= OnItemPropertyChanged;
			}
		}

		/// <summary>
		/// Converts a color value to a Brush. Supports Color, Brush, or Color hex string.
		/// </summary>
		Brush? GetBrushFromColor(object? colorValue)
		{
			if (colorValue == null)
			{
				return null;
			}

			// If already a Brush, return it
			if (colorValue is Brush brush)
			{
				return brush;
			}

			// If it's a Color, convert to SolidColorBrush
			if (colorValue is Color color)
			{
				return new SolidColorBrush(color);
			}

			// If it's a string, try to parse as hex color
			if (colorValue is string colorString)
			{
				if (Color.TryParse(colorString, out Color parsedColor))
				{
					return new SolidColorBrush(parsedColor);
				}
			}

			return null;
		}
		#endregion

		#endregion
	}
}
