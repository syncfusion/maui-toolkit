using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
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
				}

				if (newValue is SparkChartLineStyle newStyle)
				{
					SetInheritedBindingContext(newStyle, _chart.BindingContext);
					newStyle.PropertyChanged += AxisLineStyle_PropertyChanged;
				}

				_chart.ScheduleUpdateArea();
			}
		}

		static void AxisLineStyle_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (sender is SfSparkLineChart _chart)
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

		/// <summary>
		/// Generates the data points for the Spark Chart from the ItemsSource.
		/// </summary>
		internal void GeneratePoints()
		{
			// Always clear previous data before generating new points.
			yValues.Clear();
			xValues.Clear();
			EmptyPointIndexes.Clear();
			DataCount = 0;

			// Exit if the source is null or not a valid collection.
			if (ItemsSource is not IEnumerable enumerable)
			{
				UpdateMinMaxValues();
				return;
			}

			var enumerator = enumerable.GetEnumerator();

			// Exit for empty collections.
			if (!enumerator.MoveNext())
			{
				UpdateMinMaxValues();
				return;
			}

			var type = enumerator.Current?.GetType();
			Func<object, object?>? yValueAccessor = string.IsNullOrEmpty(YBindingPath) ? null : GetMethod(type, YBindingPath);
			double i = 0;

			do
			{
				var item = enumerator.Current;
				if (item == null)
				{
					yValues.Add(double.NaN);
				}
				else if (yValueAccessor != null)
				{
					var yVal = yValueAccessor(item);
					yValues.Add(yVal == null ? double.NaN : Convert.ToDouble(yVal));
				}
				else if (item is double or int or float or decimal)
				{
					yValues.Add(Convert.ToDouble(item));
				}

				xValues.Add(i++);

			} while (enumerator.MoveNext());

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
				return (obj) => null;
			var propInfo = type.GetProperty(propName, BindingFlags.Public | BindingFlags.Instance);
			return (obj) => propInfo?.GetValue(obj);
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
