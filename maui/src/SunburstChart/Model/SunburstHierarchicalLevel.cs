using Microsoft.Maui.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace Syncfusion.Maui.Toolkit.SunburstChart
{
    /// <summary>
    /// Represents the sunburst hierarchical levels.
    /// </summary>
    public class SunburstHierarchicalLevel : BindableObject
    {
        #region Fields

        List<object>? _groupInfoKeys;
        ValueInfo? _valueInfo;
        List<ValueInfo>? _groupValues;
        IEnumerable? _currentActualData;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SunburstHierarchicalLevel"/> class.
        /// </summary>
        public SunburstHierarchicalLevel()
        {
            _groupInfoKeys = new List<object>();
            _groupValues = new List<ValueInfo>();
        }

		#endregion

		#region Internal Properties

		/// <summary>
		/// Gets or sets the items for the current hierarchical level.
		/// </summary>
		internal IEnumerable? Items { get; set; }

		/// <summary>
		/// Gets or sets the grouping path used for this level.
		/// </summary>
		internal string? GroupingPath { get; set; }

		/// <summary>
		/// Gets or sets the associated SunburstChart instance.
		/// </summary>
		internal SfSunburstChart? SunburstChart { get; set; }

		/// <summary>
		/// Gets or sets the list of sunburst items for this level.
		/// </summary>
		internal List<SunburstItem>? SunburstItems { get; set; }

        /// <summary>
        /// Gets or sets <see cref="CurrentActualData"/> property metadata.
        /// </summary>
        internal IEnumerable<PropertyInfo>? ItemProperties { get; set; }

        /// <summary>
        /// Gets or sets the current grouping items source.
        /// </summary>
        internal IEnumerable? CurrentActualData
        {
            get => _currentActualData;
            set => SetCurrentActualData(value);
        }

        #endregion

        #region Bindable Property

        /// <summary>
        /// Identifies the <see cref="GroupMemberPath"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty GroupMemberPathProperty =
            BindableProperty.Create(
                nameof(GroupMemberPath),
                typeof(string), 
                typeof(SunburstHierarchicalLevel), 
                null, 
                BindingMode.Default, 
                null, 
                propertyChanged: OnGroupPathChanged);

        #endregion

        #region Public Property

        /// <summary>
        /// Gets or sets the GroupMemberPath property. This property is used to set the path of the value data in ItemsSource.
        /// </summary>
        public string GroupMemberPath
        {
            get { return (string)GetValue(GroupMemberPathProperty); }
            set { SetValue(GroupMemberPathProperty, value); }
        }

        #endregion

        #region Protected Methods

        /// <inheritdoc/>
		/// <exclude/>
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
        }

        #endregion

        #region Internal methods

        /// <summary>
        /// Method used to group the items based on the levels.
        /// </summary>
        /// <param name="groupingPath">Group path.</param>
        /// <returns>Collection of sunburst items.</returns>
        internal List<SunburstItem>? GenerateItem(string? groupingPath)
        {
            var itemsSource = CurrentActualData?.GetEnumerator();

            if (groupingPath != null && itemsSource != null)
            {
                while (itemsSource.MoveNext())
                {
                    var pathKey = GetValue(groupingPath, itemsSource.Current) as IComparable;
                    var groupKey = from key in _groupInfoKeys where key.Equals(pathKey ?? string.Empty) select key;
                    var comparable = groupKey as IComparable[] ?? groupKey.ToArray();
                    var index = comparable.Any() && _groupInfoKeys != null ? _groupInfoKeys.IndexOf(comparable.First()) : -1;

                    if (index == -1)
                    {
                        _valueInfo = new ValueInfo { Key = pathKey == null ? string.Empty : pathKey.ToString() };
                        _groupInfoKeys?.Add(pathKey ?? string.Empty);
                        _groupValues?.Add(_valueInfo);
                    }
                    else if (_groupValues != null)
                    {
                        _valueInfo = _groupValues[index];
                    }

                    _valueInfo?.Items.Add(itemsSource.Current);
                }
            }

            if (_groupValues != null)
            {
                return _groupValues?.Select(item => new SunburstItem { Values = item.Items, Key = item.Key }).ToList();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Method used to get the total y value of the particular slice and segment.
        /// </summary>
        /// <param name="groupedItems">Sunburst items.</param>
        /// <param name="valuePath">Absolute path of the y value.</param>
        internal void GetKeyValue(List<SunburstItem>? groupedItems, string? valuePath)
        {
            if (groupedItems != null)
            {
                foreach (var group in groupedItems)
                {
                    if (group.Values != null)
                    {
                        foreach (var item in group.Values)
                        {
                            try
                            {
                                if (valuePath != null && item != null)
                                {
                                    var keyValue = Math.Abs((double)(GetValue(valuePath, item) ?? double.NaN));
                                    group.KeyValue += double.IsNaN(keyValue) ? 0 : keyValue;
                                }
                            }
                            catch
                            {
                                throw new InvalidCastException("ValueMemberPath is invalid");
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Method used to determine the segment's slice info.
        /// </summary>
        /// <param name="groupItems">The sunburst items.</param>
        /// <param name="arcStartAngle">Start angle.</param>
        /// <param name="arcEndAngle">End angle.</param>
        /// <param name="index">Doughnut index.</param>
        internal void GetSliceInfo(List<SunburstItem>? groupItems, double arcStartAngle, double arcEndAngle, int index)
        {
            var sliceIndex = new List<int>();

            if (groupItems == null) { return; }

            if (index == -1)
            {
                sliceIndex.AddRange(groupItems.Select(item => groupItems.IndexOf(item)));
            }

            var keyValues = groupItems.Select(item => item.KeyValue).ToList();
            var total = keyValues.Sum();
            var arcLength = arcEndAngle - arcStartAngle;

            if (Math.Round(Math.Abs(arcLength), 3) > 6.283)
            {
                arcLength = arcLength % 6.283;
            }

            // Subtracted the fraction of value for single segment segment disappear issue
            if (Math.Round(Math.Abs(arcLength), 3) == 6.283)
            {
                arcLength = arcLength - 0.000001;
            }

            for (int i = 0; i < keyValues.Count; i++)
            {
                if (keyValues[i] == 0 || arcLength == 0 || groupItems[i] != null && string.IsNullOrEmpty(groupItems[i]?.Key?.ToString()))
                {
                    continue;
                }

                var groupElement = groupItems[i];
                groupElement.ArcStart = arcStartAngle;
                arcEndAngle = (double)(Math.Abs(keyValues[i]) * (arcLength / total));
                groupElement.ArcEnd = arcStartAngle + (double.IsNaN(arcEndAngle) ? 0 : arcEndAngle);
                groupElement.ArcMid = arcStartAngle + (arcEndAngle / 2);
                groupElement.SliceIndex = index == -1 ? sliceIndex[i] : index;

                arcStartAngle += arcEndAngle;
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Handles the property changed event for the <see cref="GroupMemberPathProperty"/>.
        /// </summary>
        /// <param name="bindable">The bindable object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        static void OnGroupPathChanged(BindableObject bindable, object oldValue, object newValue)
        {
			var sunburstHierarchical = bindable as SunburstHierarchicalLevel;
			if (sunburstHierarchical != null)
			{
				if (newValue != null)
				{
					sunburstHierarchical.GroupingPath = newValue?.ToString();
					sunburstHierarchical.SunburstChart?.GenerateSunburstItems();
					sunburstHierarchical.SunburstChart?.ScheduleUpdate();
				}
			}
		}

		/// <summary>
		/// Uses reflection to fetch the value of a property by grouping path from an item.
		/// </summary>
		/// <param name="groupingPath">The property path.</param>
		/// <param name="current">The instance to extract from.</param>
		/// <returns>The retrieved value or null.</returns>
		object? GetValue(string? groupingPath, object current)
        {
            try
            {

                if (ItemProperties != null)
                {
                    return ItemProperties.First(item => item.Name == groupingPath).GetValue(current);
                }

                return null;

            }
            catch
            {
                throw new InvalidOperationException("GroupMemberPath is invalid");
            }
        }

        /// <summary>
        /// Sets the current actual data source and populates the item properties metadata.
        /// </summary>
        /// <param name="value">The data source collection.</param>
		[UnconditionalSuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code", Justification = "<Pending>")]
		void SetCurrentActualData(IEnumerable? value)
		{
			_currentActualData = value;
			AssignItemProperties(_currentActualData);
		}

        /// <summary>
        /// Caches property metadata from the first item in the data source to optimize reflection.
        /// </summary>
        /// <param name="data">The data source collection.</param>
		[RequiresUnreferencedCode("The AssignItemProperties method is not trim compatible")]
		void AssignItemProperties(IEnumerable? data)
		{
			if (data == null)
				return;

			foreach (var item in data)
			{
				var type = item.GetType();
				if (type != null)
					ItemProperties = type.GetProperties();
				break;
			}
		}

		#endregion
	}

    /// <summary>
    /// Represents the SunburstLevelCollection. To render this, create an instance SunburstLevelCollection and set required properties.
    /// </summary>
    public class SunburstLevelCollection : ObservableCollection<SunburstHierarchicalLevel>
    {
        internal SfSunburstChart? SunburstChart;
    }

    /// <summary>
    /// Stores a grouping key and the list of grouped items at a sunburst level.
    /// </summary>
    internal class ValueInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValueInfo"/> class.
        /// </summary>
        public ValueInfo()
        {
            Items = new ObservableCollection<object>();
        }

        /// <summary>
        /// Gets or sets the current items source.
        /// </summary>
        internal ObservableCollection<object> Items { get; set; }

        /// <summary>
        /// Gets or sets the x values of the item.
        /// </summary>
        internal string? Key { get; set; }

    }
}
