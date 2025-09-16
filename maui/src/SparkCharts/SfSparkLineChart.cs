using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.Toolkit.SparkCharts
{
	/// <summary>
	/// Renders a Spark Line Chart with support for highlighting specific data points.
	/// </summary>
	public class SfSparkLineChart : SfSparkChart
	{
		#region Bindable Properties

		/// <summary>
		/// Identifies the ShowMarkers bindable property.
		/// </summary>
		public static readonly BindableProperty ShowMarkersProperty =
			BindableProperty.Create(
				nameof(ShowMarkers),
				typeof(bool),
				typeof(SfSparkLineChart),
				false,
				BindingMode.Default,
				propertyChanged: OnSparkChartPropertyChanged);

		/// <summary>
		/// Identifies the MarkerSettings bindable property.
		/// </summary>
		public static readonly BindableProperty MarkerSettingsProperty =
			BindableProperty.Create(nameof(MarkerSettings),
				typeof(SparkChartMarkerSettings),
				typeof(SfSparkLineChart),
				null,
				BindingMode.Default,
				propertyChanged: OnMarkerSettingsChanged);

		/// <summary>
		/// Identifies the StrokeWidth bindable property.
		/// </summary>
		public static readonly BindableProperty StrokeWidthProperty =
			BindableProperty.Create(
				nameof(StrokeWidth),
				typeof(double),
				typeof(SfSparkChart),
				1.0,
				BindingMode.Default,
				propertyChanged: OnSparkChartPropertyChanged);

		/// <summary>
		/// Identifies the FirstPointFill bindable property.
		/// </summary>
		public static readonly BindableProperty FirstPointFillProperty =
			BindableProperty.Create(
				nameof(FirstPointFill),
				typeof(Brush),
				typeof(SfSparkLineChart),
				null,
				BindingMode.Default,
				propertyChanged: OnSparkChartPropertyChanged);

		/// <summary>
		/// Identifies the LastPointFill bindable property.
		/// </summary>
		public static readonly BindableProperty LastPointFillProperty =
			BindableProperty.Create(
				nameof(LastPointFill),
				typeof(Brush),
				typeof(SfSparkLineChart),
				null,
				BindingMode.Default,
				propertyChanged: OnSparkChartPropertyChanged);

		/// <summary>
		/// Identifies the HighPointFill bindable property.
		/// </summary>
		public static readonly BindableProperty HighPointFillProperty =
			BindableProperty.Create(
				nameof(HighPointFill),
				typeof(Brush),
				typeof(SfSparkLineChart),
				null,
				BindingMode.Default,
				propertyChanged: OnSparkChartPropertyChanged);

		/// <summary>
		/// Identifies the LowPointFill bindable property.
		/// </summary>
		public static readonly BindableProperty LowPointFillProperty =
			BindableProperty.Create(
				nameof(LowPointFill),
				typeof(Brush),
				typeof(SfSparkLineChart),
				null,
				BindingMode.Default,
				propertyChanged: OnSparkChartPropertyChanged);

		/// <summary>
		/// Identifies the NegativePointsFill bindable property.
		/// </summary>
		public static readonly BindableProperty NegativePointsFillProperty =
			BindableProperty.Create(
				nameof(NegativePointsFill),
				typeof(Brush),
				typeof(SfSparkLineChart),
				null,
				BindingMode.Default,
				propertyChanged: OnSparkChartPropertyChanged);

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets a value indicating whether to show markers for the data points.
		/// </summary>
		public bool ShowMarkers
		{
			get => (bool)GetValue(ShowMarkersProperty);
			set => SetValue(ShowMarkersProperty, value);
		}

		/// <summary>
		/// Gets or sets the settings for the markers in the Spark Line Chart.
		/// </summary>
		public SparkChartMarkerSettings MarkerSettings
		{
			get => (SparkChartMarkerSettings)GetValue(MarkerSettingsProperty);
			set => SetValue(MarkerSettingsProperty, value);
		}

		/// <summary>
		/// Gets or sets the stroke width for the Spark Line Chart.
		/// </summary>
		public double StrokeWidth
		{
			get => (double)GetValue(StrokeWidthProperty);
			set => SetValue(StrokeWidthProperty, value);
		}

		/// <summary>
		/// Gets or sets the brush for the first point in the Spark Line Chart.
		/// </summary>
		public Brush FirstPointFill
		{
			get => (Brush)GetValue(FirstPointFillProperty);
			set => SetValue(FirstPointFillProperty, value);
		}

		/// <summary>
		/// Gets or sets the brush for the last point in the Spark Line Chart.
		/// </summary>
		public Brush LastPointFill
		{
			get => (Brush)GetValue(LastPointFillProperty);
			set => SetValue(LastPointFillProperty, value);
		}

		/// <summary>
		/// Gets or sets the brush for the highest point in the Spark Line Chart.
		/// </summary>
		public Brush HighPointFill
		{
			get => (Brush)GetValue(HighPointFillProperty);
			set => SetValue(HighPointFillProperty, value);
		}

		/// <summary>
		/// Gets or sets the brush for the lowest point in the Spark Line Chart.
		/// </summary>
		public Brush LowPointFill
		{
			get => (Brush)GetValue(LowPointFillProperty);
			set => SetValue(LowPointFillProperty, value);
		}

		/// <summary>
		/// Gets or sets the brush for negative points in the Spark Line Chart.
		/// </summary>
		public Brush NegativePointsFill
		{
			get => (Brush)GetValue(NegativePointsFillProperty);
			set => SetValue(NegativePointsFillProperty, value);
		}

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="SfSparkLineChart"/> class.
		/// </summary>
		public SfSparkLineChart()
		{
			MarkerSettings = new SparkChartMarkerSettings();
		}
		#endregion

		#region Draw Method

		internal override Rect GetTranslatedRect(Rect rect)
		{
			if (MarkerSettings != null)
			{
				var _markerWidth = MarkerSettings.Width;
				var _markerHeight = MarkerSettings.Height;
				return new Rect(
					rect.X + Padding.Left + _markerWidth / 2,
					rect.Y + Padding.Top + _markerHeight / 2,
					rect.Width - (Padding.Left + Padding.Right + _markerWidth),
					rect.Height - (Padding.Top + Padding.Bottom + _markerHeight));
			}

			return base.GetTranslatedRect(rect);
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();

			if (MarkerSettings != null)
			{
				SetInheritedBindingContext(MarkerSettings, BindingContext);
			}
		}

		/// <summary>
		/// Draws the visual elements of the Spark Line Chart.
		/// </summary>
		/// <param name="canvas">The canvas to draw on.</param>
		/// <param name="rect">The rectangle area to draw within.</param>
		internal override void OnDraw(ICanvas canvas, Rect rect)
		{
			canvas.SaveState();
			rect = GetTranslatedRect(rect);
			canvas.Translate((float)rect.X, (float)rect.Y);

			if (yValues.Count == 0)
			{
				return;
			}

			canvas.StrokeSize = (float)StrokeWidth;
			if (Stroke != null)
			{
				canvas.StrokeColor = Stroke.ToColor();
			}

			if (yValues.Count > 1)
			{
				var _path = new PathF();
				bool isPreviousPointValid = false;
				for (int i = 0; i < DataCount; i++)
				{
					double _yValue = yValues[i];

					if (double.IsNaN(_yValue))
					{
						isPreviousPointValid = false;
						continue;
					}

					PointF _currentPoint = TransformToVisible(xValues[i], _yValue, rect);
					if (isPreviousPointValid)
					{
						_path.LineTo(_currentPoint);
					}
					else
					{
						_path.MoveTo(_currentPoint);
					}

					isPreviousPointValid = true;
				}
				canvas.DrawPath(_path);
			}

			if (ShowMarkers)
			{
				DrawMarkers(canvas, rect);
			}

			base.OnDraw(canvas, rect);

			canvas.RestoreState();
		}

		#endregion

		#region Private Methods

		void DrawMarkers(ICanvas canvas, Rect rect)
		{
			if (MarkerSettings == null)
			{
				return;
			}

			float width = (float)MarkerSettings.Width;
			float height = (float)MarkerSettings.Height;
			var strokeWidth = (float)MarkerSettings.StrokeWidth;

			// Be explicit with the type here to avoid ambiguity
			Charts.ShapeType shapeType = ToInternalShapeType(MarkerSettings.ShapeType);
			bool hasBorder = MarkerSettings.StrokeWidth > 0 && MarkerSettings.Stroke != null;

			for (int i = 0; i < DataCount; i++)
			{
				var yvalue = yValues[i];

				if (double.IsNaN(yvalue))
				{
					continue;
				}

				PointF point = TransformToVisible(xValues[i], yvalue, rect);
				Brush fill = GetMarkerFill(i, yvalue);

				if (fill != null)
				{
					canvas.FillColor = fill.ToColor();
				}

				Brush? stroke = MarkerSettings.Stroke;
				if (hasBorder && stroke != null)
				{
					canvas.StrokeColor = stroke.ToColor();
				}

				canvas.StrokeSize = strokeWidth;
				canvas.DrawShape(new RectF(point.X - (width / 2), point.Y - (height / 2), width, height), shapeType, hasBorder, false);
			}
		}

		Brush GetMarkerFill(int index, double yValue)
		{
			if (yValue == maxYValue && HighPointFill != null)
				return HighPointFill;
			if (yValue == minYValue && LowPointFill != null)
				return LowPointFill;
			if (index == 0 && FirstPointFill != null)
				return FirstPointFill;
			if (index == DataCount - 1 && LastPointFill != null)
				return LastPointFill;
			if (yValue < 0 && NegativePointsFill != null)
				return NegativePointsFill;

			return MarkerSettings?.Fill ?? this.Stroke;
		}

		// Be explicit with the return type and the typeof() call to avoid ambiguity
		Charts.ShapeType ToInternalShapeType(SparkChartMarkerShape shape)
		{
			return (Charts.ShapeType)Enum.Parse(typeof(Charts.ShapeType), shape.ToString());
		}

		static void OnMarkerSettingsChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfSparkLineChart _chart)
			{
				if (oldValue is SparkChartMarkerSettings oldSettings)
				{
					SetInheritedBindingContext(oldSettings, null);

					oldSettings.PropertyChanged -= MarkerSettings_PropertyChanged;
				}

				if (newValue is SparkChartMarkerSettings newSettings)
				{
					SetInheritedBindingContext(newSettings, _chart.BindingContext);
					newSettings.PropertyChanged += MarkerSettings_PropertyChanged;
				}

				_chart.ScheduleUpdateArea();
			}
		}

		static void MarkerSettings_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (sender is SfSparkLineChart _chart)
			{
				_chart.ScheduleUpdateArea();
			}
		}

		#endregion
	}
}