using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using Microsoft.Maui.Layouts;
using Syncfusion.Maui.Toolkit.Charts;
using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.SparkCharts
{
	/// <summary>
	/// Represents the base class for all Spark Charts and provides common behaviors and control flow.
	/// </summary>
	public abstract class SfSparkChart : View, IContentView
	{
		readonly SparkChartView seriesView;

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="SfSparkChart"/> class.
		/// </summary>
		public SfSparkChart()
		{
			seriesView = new SparkChartView(this);
			AxisLineStyle = new SparkChartLineStyle();
			_content = seriesView;
		}

		#endregion

		#region Bindable Properties

		/// <summary>
		/// Identifies the ItemsSource bindable property.
		/// </summary>
		public static readonly BindableProperty ItemsSourceProperty =
			BindableProperty.Create(
				nameof(ItemsSource),
				typeof(object),
				typeof(SfSparkChart),
				null,
				BindingMode.Default,
				propertyChanged: OnBindableItemsSourceChanged);

		/// <summary>
		/// Identifies the YBindingPath bindable property.
		/// </summary>
		public static readonly BindableProperty YBindingPathProperty =
			BindableProperty.Create(
				nameof(YBindingPath),
				typeof(string),
				typeof(SfSparkChart),
				string.Empty,
				BindingMode.Default,
				propertyChanged: OnSparkChartSourcePropertyChanged);

		/// <summary>
		/// Identifies the ShowAxis bindable property.
		/// </summary>
		public static readonly BindableProperty ShowAxisProperty =
			BindableProperty.Create(
				nameof(ShowAxis),
				typeof(bool),
				typeof(SfSparkChart),
				false,
				BindingMode.Default,
				propertyChanged: OnSparkChartPropertyChanged);

		/// <summary>
		/// Identifies the AxisOrigin bindable property.
		/// </summary>
		public static readonly BindableProperty AxisOriginProperty =
			BindableProperty.Create(
				nameof(AxisOrigin),
				typeof(double),
				typeof(SfSparkChart),
				double.NaN,
				BindingMode.Default,
				propertyChanged: OnSparkChartPropertyChanged);

		/// <summary>
		/// Identifies the AxisLineStyle bindable property.
		/// </summary>
		public static readonly BindableProperty AxisLineStyleProperty =
			BindableProperty.Create(
				nameof(AxisLineStyle),
				typeof(SparkChartLineStyle),
				typeof(SfSparkChart),
				null,
				BindingMode.Default,
				propertyChanged: OnAxisStylePropertyChanged);

		/// <summary>
		/// Identifies the EmptyPointMode bindable property.
		/// </summary>
		internal static readonly BindableProperty EmptyPointModeProperty =
			BindableProperty.Create(
				nameof(EmptyPointMode),
				typeof(SparkChartEmptyPointMode),
				typeof(SfSparkChart),
				SparkChartEmptyPointMode.Zero,
				BindingMode.Default,
				propertyChanged: OnSparkChartSourcePropertyChanged);

		/// <summary>
		/// Identifies the MinimumYValue bindable property.
		/// </summary>
		public static readonly BindableProperty MinimumYValueProperty =
			BindableProperty.Create(
				nameof(MinimumYValue),
				typeof(double),
				typeof(SfSparkChart),
				double.NaN,
				propertyChanged: OnMinMaxChanged);

		/// <summary>
		/// Identifies the MaximumYValue bindable property.
		/// </summary>
		public static readonly BindableProperty MaximumYValueProperty =
			BindableProperty.Create(
				nameof(MaximumYValue),
				typeof(double),
				typeof(SfSparkChart),
				double.NaN,
				propertyChanged: OnMinMaxChanged);

		/// <summary>
		/// Identifies the Stroke bindable property.
		/// </summary>
		public static readonly BindableProperty StrokeProperty =
			BindableProperty.Create(
				nameof(Stroke),
				typeof(Brush),
				typeof(SfSparkChart),
				new SolidColorBrush(Color.FromArgb("#116DF9")),
				BindingMode.Default,
				propertyChanged: OnSparkChartPropertyChanged);

		/// <summary>
		/// Identifies the Padding bindable property.
		/// </summary>
		public static readonly BindableProperty PaddingProperty =
			BindableProperty.Create(
				nameof(Padding),
				typeof(Thickness),
				typeof(SfSparkChart),
				Thickness.Zero,
				BindingMode.Default,
				propertyChanged: OnSparkChartPropertyChanged);

		/// <summary>
		/// Identifies the XBindingPath bindable property.
		/// </summary>
		public static readonly BindableProperty XBindingPathProperty =
			BindableProperty.Create(
				nameof(XBindingPath),
				typeof(string),
				typeof(SfSparkChart),
				string.Empty,
				BindingMode.Default,
				propertyChanged: OnSparkChartSourcePropertyChanged);

		/// <summary>
		/// Identifies the AxisType bindable property.
		/// </summary>
		public static readonly BindableProperty AxisTypeProperty =
			BindableProperty.Create(
				nameof(AxisType),
				typeof(SparkChartAxisType),
				typeof(SfSparkChart),
				SparkChartAxisType.Numeric,
				BindingMode.Default,
				propertyChanged: OnSparkChartSourcePropertyChanged);
		/// <summary>
		/// Identifies the RangeBandStart bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="RangeBandStart"/> bindable property determines 
		/// the start value of the range band in y-axis.
		/// </remarks>
		public static readonly BindableProperty RangeBandStartProperty =
			BindableProperty.Create(
				nameof(RangeBandStart),
				typeof(double),
				typeof(SfSparkChart),
				double.NaN,
				BindingMode.Default,
				propertyChanged: OnSparkChartPropertyChanged);

		/// <summary>
		/// Identifies the RangeBandEnd bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="RangeBandEnd"/> bindable property determines 
		/// the end value of the range band in y-axis.
		/// </remarks>
		public static readonly BindableProperty RangeBandEndProperty =
			BindableProperty.Create(
				nameof(RangeBandEnd),
				typeof(double),
				typeof(SfSparkChart),
				double.NaN,
				BindingMode.Default,
				propertyChanged: OnSparkChartPropertyChanged);

		/// <summary>
		/// Identifies the RangeBandFill bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="RangeBandFill"/> bindable property specifies 
		/// the range band color applied to the sparkchart.
		/// </remarks>
		public static readonly BindableProperty RangeBandFillProperty =
			BindableProperty.Create(
				nameof(RangeBandFill),
				typeof(Brush),
				typeof(SfSparkChart),
				new SolidColorBrush(Color.FromArgb("#E7E0EC")),
				BindingMode.Default,
				propertyChanged: OnSparkChartPropertyChanged);

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets the collection used to generate the _content of the chart.
		/// </summary>
		public object ItemsSource
		{
			get => GetValue(ItemsSourceProperty);
			set => SetValue(ItemsSourceProperty, value);
		}

		/// <summary>
		/// Gets or sets the path to the property used for Y-axis values.
		/// </summary>
		public string YBindingPath
		{
			get => (string)GetValue(YBindingPathProperty);
			set => SetValue(YBindingPathProperty, value);
		}

		/// <summary>
		/// Gets or sets the minimum value for the Y-axis.
		/// </summary>
		public double MinimumYValue
		{
			get => (double)GetValue(MinimumYValueProperty);
			set => SetValue(MinimumYValueProperty, value);
		}

		/// <summary>
		/// Gets or sets the maximum value for the Y-axis.
		/// </summary>
		public double MaximumYValue
		{
			get => (double)GetValue(MaximumYValueProperty);
			set => SetValue(MaximumYValueProperty, value);
		}

		/// <summary>
		/// Gets or sets the brush used to draw the stroke for the chart line, area, column or win-loss.
		/// </summary>
		public Brush Stroke
		{
			get => (Brush)GetValue(StrokeProperty);
			set => SetValue(StrokeProperty, value);
		}

		/// <summary>
		/// Gets or sets the padding for the chart line, area, column or win-loss.
		/// </summary>
		public Thickness Padding
		{
			get => (Thickness)GetValue(PaddingProperty);
			set => SetValue(PaddingProperty, value);
		}

		/// <summary>
		/// Gets or sets the path to the property used for X-axis values.
		/// </summary>
		public string XBindingPath
		{
			get => (string)GetValue(XBindingPathProperty);
			set => SetValue(XBindingPathProperty, value);
		}

		/// <summary>
		/// Gets or sets how the spark chart interprets X-axis values.
		/// </summary>
		public SparkChartAxisType AxisType
		{
			get => (SparkChartAxisType)GetValue(AxisTypeProperty);
			set => SetValue(AxisTypeProperty, value);
		}
		/// <summary>
		/// Gets or sets the start Y value of the highlighted range band.
		/// </summary>
		/// <value>It accepts <see cref="double"/> values and the default value is <c>double.NaN</c>.</value>
		public double RangeBandStart
		{
			get => (double)GetValue(RangeBandStartProperty);
			set => SetValue(RangeBandStartProperty, value);
		}

		/// <summary>
		/// Gets or sets the end Y value of the highlighted range band.
		/// </summary>
		/// <value>It accepts <see cref="double"/> values and the default value is <c>double.NaN</c>.</value>
		public double RangeBandEnd
		{
			get => (double)GetValue(RangeBandEndProperty);
			set => SetValue(RangeBandEndProperty, value);
		}

		/// <summary>
		/// Gets or sets the fill brush used to render the range band.
		/// </summary>
		/// <value> This property takes <see cref= "Brush"/> values and the default value is <c>#E7E0EC"</c>.</value>
		public Brush RangeBandFill
		{
			get => (Brush)GetValue(RangeBandFillProperty);
			set => SetValue(RangeBandFillProperty, value);
		}

		#endregion

		#region Internal Properties

		object? _content;

		/// <summary>
		/// Internal variables
		/// </summary>
		internal double minYValue, maxYValue, deltaY, minXValue, maxXValue, deltaX;

		internal List<double> yValues { get; private set; } = [];

		internal List<double> xValues { get; private set; } = [];

		internal int DataCount { get; private set; }

		internal List<int> EmptyPointIndexes { get; private set; } = [];

		// Tracks the positions of intentional "gaps" inserted for missing X in numeric/datetime axes.
		internal HashSet<int> GapIndexes { get; private set; } = new();

		internal SparkChartEmptyPointMode EmptyPointMode
		{
			get => (SparkChartEmptyPointMode)GetValue(EmptyPointModeProperty);
			set => SetValue(EmptyPointModeProperty, value);
		}

		/// <summary>
		/// Gets or sets the flag to visible axis.
		/// </summary>
		public bool ShowAxis
		{
			get => (bool)GetValue(ShowAxisProperty);
			set => SetValue(ShowAxisProperty, value);
		}

		/// <summary>
		/// Gets or sets the value to configure axis orgin.
		/// </summary>
		public double AxisOrigin
		{
			get => (double)GetValue(AxisOriginProperty);
			set => SetValue(AxisOriginProperty, value);
		}

		/// <summary>
		/// Gets or sets the line style for axis. 
		/// </summary>
		public SparkChartLineStyle AxisLineStyle
		{
			get => (SparkChartLineStyle)GetValue(AxisLineStyleProperty);
			set => SetValue(AxisLineStyleProperty, value);
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();
			if (AxisLineStyle != null)
			{
				SetInheritedBindingContext(AxisLineStyle, BindingContext);
				AxisLineStyle.Parent = this;
			}
		}

		#endregion

		#region Methods

		#region Draw Method

		/// <summary>
		/// Draws the chart _content. This method can be overridden by derived chart types.
		/// </summary>
		internal virtual void OnDraw(ICanvas canvas, Rect rect)
		{
			if (ShowAxis && AxisLineStyle != null)
			{
				canvas.SaveState();
				var path = new PathF();
				var point1 = new PointF();
				var point2 = new PointF();

				GetAxisPosition(ref point1, ref point2, rect);

				canvas.StrokeSize = (float)AxisLineStyle.StrokeWidth;
				var _stroke = AxisLineStyle.Stroke;
				if (_stroke != null)
				{
					canvas.StrokeColor = _stroke.ToColor();
				}

				var _strokeDashArray = AxisLineStyle.StrokeDashArray;
				if (_strokeDashArray != null && _strokeDashArray.Count > 0)
				{
					canvas.StrokeDashPattern = _strokeDashArray.ToFloatArray();
				}

				canvas.DrawLine(point1, point2);
				canvas.RestoreState();
			}
		}

		internal virtual void GetAxisPosition(ref PointF point1, ref PointF point2, Rect rect)
		{
			var y = double.IsNaN(AxisOrigin) ? 0 : AxisOrigin;
			point1 = TransformToVisible(0, y, rect);
			point2 = new PointF((float)rect.Width, point1.Y);
		}

		internal virtual Rect GetTranslatedRect(Rect rect)
		{
			if (Padding != Thickness.Zero)
			{
				return new Rect(
					rect.X + Padding.Left,
					rect.Y + Padding.Top,
					rect.Width - (Padding.Left + Padding.Right),
					rect.Height - (Padding.Top + Padding.Bottom));
			}

			return rect;
		}

		/// <summary>
		/// Draw the range band (plot band). Call this method before drawing series.
		/// </summary>
		internal void DrawRangeBand(ICanvas canvas, Rect rect)
		{
			if (double.IsNaN(RangeBandStart) || double.IsNaN(RangeBandEnd))
			{
				return;
			}

			var start = RangeBandStart;
			var end = RangeBandEnd;
			if (start > end)
			{
				(start, end) = (end, start);
			}

			var pStart = TransformToVisible(0, start, rect);
			var pEnd = TransformToVisible(0, end, rect);

			float yTop = MathF.Min(pStart.Y, pEnd.Y);
			float yBottom = MathF.Max(pStart.Y, pEnd.Y);

			float top = MathF.Max(0, yTop);
			float bottom = MathF.Min((float)rect.Height, yBottom);

			if (bottom <= top)
			{
				return;
			}

			var width = (float)rect.Width;
			var height = bottom - top;
			var drawRect = new Rect(0f, top, width, height);

			canvas.SaveState();
			canvas.SetFillPaint(RangeBandFill, drawRect);
			canvas.FillRectangle(drawRect);
			canvas.RestoreState();
		}
		#endregion

		#region Property Callback Methods
		static void OnBindableItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfSparkChart sparkChart)
			{
				sparkChart.OnItemsSourceChanged(oldValue, newValue);
			}
		}

		static void OnSparkChartSourcePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfSparkChart sparkChart && sparkChart.ItemsSource != null)
			{
				sparkChart.ProcessData();
			}
		}

		internal static void OnAxisStylePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfSparkChart _chart)
			{
				if (oldValue is SparkChartLineStyle oldStyle)
				{
					SetInheritedBindingContext(oldStyle, null);
					oldStyle.PropertyChanged -= AxisLineStyle_PropertyChanged;
					oldStyle.Parent = null;
				}

				if (newValue is SparkChartLineStyle newStyle)
				{
					SetInheritedBindingContext(newStyle, _chart.BindingContext);
					newStyle.PropertyChanged += AxisLineStyle_PropertyChanged;
					newStyle.Parent = _chart;
				}

				_chart.ScheduleUpdateArea();
			}
		}

		static void AxisLineStyle_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (sender is SparkChartLineStyle style && style.Parent is SfSparkChart _chart)
			{
				_chart.ScheduleUpdateArea();
			}
		}

		internal static void OnSparkChartPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfSparkChart sparkChart && sparkChart.ItemsSource != null)
			{
				sparkChart.ScheduleUpdateArea();
			}
		}

		static void OnMinMaxChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfSparkChart sparkChart && sparkChart.ItemsSource != null)
			{
				sparkChart.UpdateMinMaxValues();
				sparkChart.ScheduleUpdateArea();
			}
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Refresh the series when the ItemsSource is changed.
		/// </summary>
		void OnItemsSourceChanged(object oldValue, object newValue)
		{
			if (oldValue is INotifyCollectionChanged oldCollection)
			{
				oldCollection.CollectionChanged -= OnItemsSourceCollectionChanged;
			}

			if (newValue is INotifyCollectionChanged newCollection)
			{
				newCollection.CollectionChanged += OnItemsSourceCollectionChanged;
			}

			ProcessData();
		}

		/// <summary>
		/// Updates the series when the ItemsSource collection is changed
		/// </summary>
		void OnItemsSourceCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
		{
			ProcessData();
		}

		/// <summary>
		/// Central method to trigger data processing and UI update.
		/// </summary>
		void ProcessData()
		{
			GeneratePoints();
			ScheduleUpdateArea();
		}
		private static double NormalizeNumericKey(double x)
		{
			// Round to a stable precision so 1 and 1.0000001 are treated the same
			return Math.Round(x, 6, MidpointRounding.AwayFromZero);
		}

		private static DateTime NormalizeDateKey(double oaDate)
		{
			// Bucket by day for spark charts
			return DateTime.FromOADate(oaDate).Date;
		}

		/// <summary>
		/// Generates the data points for the Spark Chart from the ItemsSource.
		/// Axis types support is applied only for X (XBindingPath) to avoid affecting previous functionalities.
		/// For Numeric and DateTime:
		/// - SparkLineChart and SparkAreaChart: sort the given data by X only (no gap insertion).
		/// - SparkColumnChart and SparkWinLossChart: sort and generate a dense sequence (unit=1 or 1 day) inserting NaN for each missing unit.
		/// </summary>
		internal void GeneratePoints()
		{
			yValues.Clear();
			xValues.Clear();
			EmptyPointIndexes.Clear();
			GapIndexes.Clear();
			DataCount = 0;
			if (ItemsSource is not IEnumerable items)
			{
				UpdateMinMaxValues();
				return;
			}

			var enumerator = items.GetEnumerator();
			if (!enumerator.MoveNext())
			{
				UpdateMinMaxValues();
				return;
			}

			var itemRuntimeType = enumerator.Current?.GetType();
			Func<object, object?>? getY = string.IsNullOrEmpty(YBindingPath) ? null : GetMethod(itemRuntimeType, YBindingPath);
			Func<object, object?>? getX = string.IsNullOrEmpty(XBindingPath) ? null : GetMethod(itemRuntimeType, XBindingPath);
			var sourceYValues = new List<double>();
			var sourceXValuesRaw = new List<object?>();
			int categoryIndex = 0;
			do
			{
				var item = enumerator.Current;
				if (item == null)
				{
					sourceYValues.Add(double.NaN);
				}
				else if (getY != null)
				{
					var yObj = getY(item);
					sourceYValues.Add(yObj == null ? double.NaN : Convert.ToDouble(yObj, CultureInfo.InvariantCulture));
				}
				else if (item is double or int or float or decimal)
				{
					sourceYValues.Add(Convert.ToDouble(item, CultureInfo.InvariantCulture));
				}
				else
				{
					sourceYValues.Add(double.NaN);
				}

				if (getX != null && item != null)
				{
					sourceXValuesRaw.Add(getX(item));
				}
				else
				{
					sourceXValuesRaw.Add(categoryIndex);
				}

				categoryIndex++;
			}
			while (enumerator.MoveNext());

			bool useCategoryAxis = string.IsNullOrEmpty(XBindingPath) || AxisType == SparkChartAxisType.Category;
			if (useCategoryAxis)
			{
				for (int i = 0; i < sourceYValues.Count; i++)
				{
					xValues.Add(i);
					yValues.Add(sourceYValues[i]);
				}

				DataCount = xValues.Count;
				FindEmptyPoints();
				ValidateEmptyPoints(yValues);
				UpdateMinMaxValues();
				return;
			}

			bool isLineOrAreaChart = this is SfSparkLineChart || this is SfSparkAreaChart;
			if (isLineOrAreaChart)
			{
				if (AxisType == SparkChartAxisType.Numeric)
				{
					var pairs = new List<(double x, double y)>();
					for (int i = 0; i < sourceYValues.Count; i++)
					{
						var x = ConvertToDouble(sourceXValuesRaw[i]);
						if (!double.IsNaN(x))
						{
							var normalizedX = NormalizeNumericKey(x);
							pairs.Add((normalizedX, sourceYValues[i]));
						}
					}

					foreach (var value in pairs.OrderBy(value => value.x))
					{
						xValues.Add(value.x);
						yValues.Add(value.y);
					}
				}
				else // DateTime axis
				{
					var pairs = new List<(double x, double y)>();
					for (int i = 0; i < sourceYValues.Count; i++)
					{
						var oa = ConvertToDouble(sourceXValuesRaw[i]);
						if (!double.IsNaN(oa))
						{
							var day = NormalizeDateKey(oa);
							pairs.Add((day.ToOADate(), sourceYValues[i]));
						}
					}

					foreach (var value in pairs.OrderBy(value => value.x))
					{
						xValues.Add(value.x);
						yValues.Add(value.y);
					}
				}

				DataCount = xValues.Count;
				FindEmptyPoints();
				ValidateEmptyPoints(yValues);
				UpdateMinMaxValues();
				return;
			}

			if (AxisType == SparkChartAxisType.Numeric)
			{
				var lastYAtIndex = new Dictionary<long, double>();
				for (int i = 0; i < sourceYValues.Count; i++)
				{
					var x = ConvertToDouble(sourceXValuesRaw[i]);
					if (!double.IsNaN(x))
					{
						long bucket = (long)Math.Round(x, MidpointRounding.AwayFromZero);
						lastYAtIndex[bucket] = sourceYValues[i];
					}
				}

				if (lastYAtIndex.Count == 0)
				{
					UpdateMinMaxValues();
					return;
				}

				long minBucket = lastYAtIndex.Keys.Min();
				long maxBucket = lastYAtIndex.Keys.Max();
				for (long bucket = minBucket; bucket <= maxBucket; bucket++)
				{
					xValues.Add(bucket);
					if (lastYAtIndex.TryGetValue(bucket, out var y))
					{
						yValues.Add(y);
					}
					else
					{
						yValues.Add(double.NaN);
						GapIndexes.Add(yValues.Count - 1);
					}
				}
			}
			else // DateTime axis
			{
				var lastYAtDay = new Dictionary<DateTime, double>();
				for (int i = 0; i < sourceYValues.Count; i++)
				{
					var oa = ConvertToDouble(sourceXValuesRaw[i]);
					if (!double.IsNaN(oa))
					{
						var day = NormalizeDateKey(oa);
						lastYAtDay[day] = sourceYValues[i];
					}
				}

				if (lastYAtDay.Count == 0)
				{
					UpdateMinMaxValues();
					return;
				}

				var minDay = lastYAtDay.Keys.Min();
				var maxDay = lastYAtDay.Keys.Max();
				for (var day = minDay; day <= maxDay; day = day.AddDays(1))
				{
					xValues.Add(day.ToOADate());
					if (lastYAtDay.TryGetValue(day, out var y))
					{
						yValues.Add(y);
					}
					else
					{
						yValues.Add(double.NaN);
						GapIndexes.Add(yValues.Count - 1);
					}
				}
			}

			DataCount = xValues.Count;
			FindEmptyPoints();
			ValidateEmptyPoints(yValues);
			UpdateMinMaxValues();
		}

		#endregion

		#region Internal and Private Methods

		/// <summary>
		/// Converts a data point to its visible screen coordinate.
		/// </summary>
		internal PointF TransformToVisible(double x, double y, Rect rect)
		{
			float xVal = (float)(rect.Width * ((x - minXValue) / deltaX));
			float yVal = (float)(rect.Height * (1 - ((y - minYValue) / deltaY)));
			return new PointF(xVal, yVal);
		}

		/// <summary>
		/// Retrieves a property getter delegate for the specified property name.
		/// </summary>

		[UnconditionalSuppressMessage("Trimming", "IL2070: Members that access public properties via reflection require dynamic access annotations to preserve metadata during trimming. The parameter 'type' in 'SfSparkChart.GetMethod(Type, String)' must be annotated with 'DynamicallyAccessedMemberTypes.PublicProperties' to ensure compatibility with trimming tools", Justification = "<Pending>")]
		private Func<object, object?> GetMethod(Type? type, string propName)
		{
			if (type == null)
			{
				return (obj) => null;
			}

			var propInfo = type.GetProperty(propName, BindingFlags.Public | BindingFlags.Instance);
			return (obj) => propInfo?.GetValue(obj);
		}

		internal static double ConvertToDouble(object? val)
		{
			if (val == null)
			{
				return double.NaN;
			}

			if (double.TryParse(val.ToString(), out double doubleVal))
			{
				return doubleVal;
			}

			if (DateTime.TryParse(val.ToString(), out DateTime date))
			{
				if (date == DateTime.MaxValue)
				{
					return double.MaxValue;
				}
				else if (date == DateTime.MinValue)
				{
					return double.MinValue;
				}

				return date.ToOADate();
			}

			return double.NaN;
		}

		#endregion

		#region Internal Methods

		/// <summary>
		/// Find empty points.
		/// </summary>
		internal void FindEmptyPoints()
		{
			EmptyPointIndexes.Clear();
			for (var i = 0; i < yValues.Count; i++)
			{
				if (double.IsNaN(yValues[i]))
				{
					EmptyPointIndexes.Add(i);
				}
			}
		}

		/// <summary>
		/// Validate empty points
		/// </summary>
		internal void ValidateEmptyPoints(List<double> yValues)
		{
			if (EmptyPointIndexes.Count == 0)
			{
				return;
			}

			switch (EmptyPointMode)
			{
				case SparkChartEmptyPointMode.Average:
					{
						foreach (int index in EmptyPointIndexes)
						{
							if (GapIndexes.Contains(index))
							{
								continue;
							}

							if (index == 0)
							{
								var nextIndex = index + 1;
								yValues[index] = (nextIndex < yValues.Count && !double.IsNaN(yValues[nextIndex])) ? yValues[nextIndex] / 2 : 0;
							}
							else if (index == DataCount - 1)
							{
								yValues[index] = !double.IsNaN(yValues[index - 1]) ? yValues[index - 1] / 2 : 0;
							}
							else
							{
								var prev = !double.IsNaN(yValues[index - 1]) ? yValues[index - 1] : 0;
								var nextIndex = index + 1;
								var next = (nextIndex < yValues.Count && !double.IsNaN(yValues[nextIndex])) ? yValues[nextIndex] : 0;
								yValues[index] = (prev + next) / 2;
							}
						}
						break;
					}

				case SparkChartEmptyPointMode.Zero:
					{
						foreach (int index in EmptyPointIndexes)
						{
							if (GapIndexes.Contains(index))
							{
								continue;
							}

							yValues[index] = 0;
						}
						break;
					}

				case SparkChartEmptyPointMode.None:
				default:
					{
						break;
					}
			}
		}

		/// <summary>
		/// Updates the minimum and maximum values for both X and Y axes.
		/// </summary>
		internal virtual void UpdateMinMaxValues()
		{
			if (yValues.Count == 0)
			{
				minXValue = minYValue = 0;
				maxXValue = maxYValue = 0;
				deltaX = deltaY = 1;
				return;
			}

			var validYValues = yValues.Where(y => !double.IsNaN(y)).ToList();

			if (validYValues.Count == 0)
			{
				minYValue = maxYValue = 0;
			}
			else
			{
				minYValue = validYValues.Min();
				maxYValue = validYValues.Max();
			}

			if (!double.IsNaN(MinimumYValue))
			{
				minYValue = MinimumYValue;
			}

			if (!double.IsNaN(MaximumYValue))
			{
				maxYValue = MaximumYValue;
			}

			if (maxYValue < minYValue)
			{
				var temp = minYValue;
				minYValue = maxYValue;
				maxYValue = temp;
			}

			if (xValues.Count > 0)
			{
				minXValue = xValues.Min();
				maxXValue = xValues.Max();
			}
			else
			{
				minXValue = maxXValue = 0;
			}

			deltaY = maxYValue - minYValue;
			deltaX = maxXValue - minXValue;
			deltaY = deltaY == 0 ? 1 : deltaY;
			deltaX = deltaX == 0 ? 1 : deltaX;
		}

		internal void ScheduleUpdateArea()
		{
			seriesView.InvalidateDrawable();
		}

		#endregion

		#region Interface Overrides

		object? IContentView.Content => _content;

		/// <summary>
		/// Gets the presented _content value.
		/// </summary>
		IView? IContentView.PresentedContent => _content as IView;

		/// <summary>
		/// Gets the padding value.
		/// </summary>
		Thickness IPadding.Padding => Thickness.Zero;

		Size IContentView.CrossPlatformMeasure(double widthConstraint, double heightConstraint)
		{
			return this.MeasureContent(widthConstraint, heightConstraint);
		}

		Size IContentView.CrossPlatformArrange(Rect bounds)
		{
			this.ArrangeContent(bounds);
			return bounds.Size;
		}

		#endregion

		#endregion
	}

}
