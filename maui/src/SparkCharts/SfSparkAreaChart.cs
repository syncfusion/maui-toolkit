using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.Toolkit.SparkCharts
{
	/// <summary>
	/// Renders a Spark Area Chart with fill and stroke customization.
	/// </summary>
	public class SfSparkAreaChart : SfSparkChart
	{
		#region Bindable Properties

		/// <summary>
		/// Identifies the Fill bindable property.
		/// </summary>
		public static readonly BindableProperty FillProperty =
			BindableProperty.Create(
				nameof(Fill),
				typeof(Brush),
				typeof(SfSparkAreaChart),
				new SolidColorBrush(Color.FromArgb("#4D116DF9")),
				BindingMode.Default,
				propertyChanged: OnSparkChartPropertyChanged);


		/// <summary>
		/// Identifies the ShowMarkers bindable property.
		/// </summary>
		public static readonly BindableProperty ShowMarkersProperty =
			 BindableProperty.Create(
				 nameof(ShowMarkers),
				 typeof(bool),
				 typeof(SfSparkAreaChart),
				 false,
				 BindingMode.Default,
				propertyChanged: OnSparkChartPropertyChanged);

		/// <summary>
		/// Identifies the MarkerSettings bindable property.
		/// </summary>
		public static readonly BindableProperty MarkerSettingsProperty =
			BindableProperty.Create(nameof(MarkerSettings),
				typeof(SparkChartMarkerSettings),
				typeof(SfSparkAreaChart),
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
		/// Gets or sets the brush for the first point in the Spark Area Chart.
		/// </summary>
		public static readonly BindableProperty FirstPointFillProperty =
			BindableProperty.Create(
				nameof(FirstPointFill),
				typeof(Brush),
				typeof(SfSparkAreaChart),
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
				typeof(SfSparkAreaChart),
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
				typeof(SfSparkAreaChart),
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
				typeof(SfSparkAreaChart),
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
				typeof(SfSparkAreaChart),
				null,
				BindingMode.Default,
				propertyChanged: OnSparkChartPropertyChanged);

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets the Fill property to fill the interior of the area spark chart.
		/// </summary>
		public Brush Fill
		{
			get => (Brush)GetValue(FillProperty);
			set => SetValue(FillProperty, value);
		}


		/// <summary>
		/// Gets or sets a value indicating whether markers should be shown on the Spark Area Chart.
		/// </summary>
		public bool ShowMarkers
		{
			get => (bool)GetValue(ShowMarkersProperty);
			set => SetValue(ShowMarkersProperty, value);
		}

		/// <summary>
		/// Gets or sets the marker settings for the Spark Area Chart.
		/// This allows customization of marker appearance such as size, color, stroke and visibility.
		/// </summary>
		public SparkChartMarkerSettings MarkerSettings
		{
			get => (SparkChartMarkerSettings)GetValue(MarkerSettingsProperty);
			set => SetValue(MarkerSettingsProperty, value);
		}

		/// <summary>
		/// Gets or sets the stroke width for the Spark Area Chart.
		/// </summary>
		public double StrokeWidth
		{
			get => (double)GetValue(StrokeWidthProperty);
			set => SetValue(StrokeWidthProperty, value);
		}

		/// <summary>
		/// Gets or sets the brush for the first point in the Spark Area Chart.
		/// </summary>
		public Brush FirstPointFill
		{
			get => (Brush)GetValue(FirstPointFillProperty);
			set => SetValue(FirstPointFillProperty, value);
		}

		/// <summary>
		/// Gets or sets the brush for the last point in the Spark Area Chart.
		/// </summary>
		public Brush LastPointFill
		{
			get => (Brush)GetValue(LastPointFillProperty);
			set => SetValue(LastPointFillProperty, value);
		}

		/// <summary>
		/// Gets or sets the brush for the highest point in the Spark Area Chart.
		/// </summary>
		public Brush HighPointFill
		{
			get => (Brush)GetValue(HighPointFillProperty);
			set => SetValue(HighPointFillProperty, value);
		}

		/// <summary>
		/// Gets or sets the brush for the lowest point in the Spark Area Chart.
		/// </summary>
		public Brush LowPointFill
		{
			get => (Brush)GetValue(LowPointFillProperty);
			set => SetValue(LowPointFillProperty, value);
		}

		/// <summary>
		/// Gets or sets the brush for negative points in the area chart.
		/// </summary>
		public Brush NegativePointsFill
		{
			get => (Brush)GetValue(NegativePointsFillProperty);
			set => SetValue(NegativePointsFillProperty, value);
		}

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="SfSparkAreaChart"/> class.
		/// Sets default values for marker settings.
		/// </summary>

		public SfSparkAreaChart()
		{
			MarkerSettings = new SparkChartMarkerSettings();
		}

		#endregion

		#region Draw Method

		/// <summary>
		/// Draws the visual elements of the Spark Area Chart.
		/// </summary>
		/// <param name="canvas">The canvas to draw on.</param>
		/// <param name="rect">The rectangle area to draw within.</param>
		internal override void OnDraw(ICanvas canvas, Rect rect)
		{
			if (yValues == null || yValues.Count == 0)
			{
				return;
			}

			canvas.SaveState();
			rect = GetTranslatedRect(rect);
			canvas.Translate((float)rect.X, (float)rect.Y);

			// Determine the baseline for the area, clamped within the visible Y-range.
			var _baseline = Math.Max(minYValue, Math.Min(maxYValue, 0d));
			var _baselineY = TransformToVisible(0, _baseline, rect).Y;
			var _segments = CreateSegments(rect, _baselineY);
			foreach (var segment in _segments)
			{
				if (segment.Count < 1)
				{
					continue;
				}

				// Create the filled area path for the segment.
				var _areaPath = new PathF();
				_areaPath.MoveTo(segment.First().X, _baselineY);
				foreach (var point in segment)
				{
					_areaPath.LineTo(point);
				}

				_areaPath.LineTo(segment.Last().X, _baselineY);
				_areaPath.Close();

				canvas.SetFillPaint(Fill, _areaPath.Bounds);
				canvas.FillPath(_areaPath);

				// Create and draw the stroke path along the top of the area.
				var _strokePath = new PathF(segment.First());
				for (int i = 1; i < segment.Count; i++)
				{
					_strokePath.LineTo(segment[i]);
				}

				if (Stroke != null && StrokeWidth > 0)
				{
					canvas.StrokeSize = (float)StrokeWidth;
					canvas.StrokeColor = Stroke.ToColor();

					canvas.DrawPath(_strokePath);
				}
			}

			if (ShowMarkers)
			{
				DrawMarkers(canvas, rect);
			}

			base.OnDraw(canvas, rect);
			canvas.RestoreState();
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
			Charts.ShapeType shapeType = ToInternalShapeType(MarkerSettings.ShapeType);
			bool hasBorder = MarkerSettings.StrokeWidth > 0 && MarkerSettings.Stroke != null;

			for (int i = 0; i < DataCount; i++)
			{
				if (double.IsNaN(yValues[i]))
					continue;

				PointF point = TransformToVisible(xValues[i], yValues[i], rect);
				Brush fill = GetMarkerFill(i);

				if (fill != null)
				{
					canvas.FillColor = fill.ToColor();
				}

				var stroke = MarkerSettings.Stroke;
				if (hasBorder && stroke != null)
				{
					canvas.StrokeColor = stroke.ToColor();
				}

				canvas.StrokeSize = strokeWidth;
				var _rect = new RectF(point.X - (width / 2), point.Y - (height / 2), width, height);
				canvas.DrawShape(_rect, shapeType, hasBorder, false);
			}
		}

		Brush GetMarkerFill(int index)
		{
			double yValue = yValues[index];

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

			return MarkerSettings?.Fill ?? Stroke;
		}

		Charts.ShapeType ToInternalShapeType(SparkChartMarkerShape shape)
		{
			return (Charts.ShapeType)Enum.Parse(typeof(Charts.ShapeType), shape.ToString());
		}

		static void OnMarkerSettingsChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfSparkAreaChart _chart)
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
			if (sender is SfSparkAreaChart _chart)
			{
				_chart.ScheduleUpdateArea();
			}
		}

		/// <summary>
		/// Breaks the data into continuous segments based on empty (NaN) data points.
		/// </summary>
		List<List<PointF>> CreateSegments(Rect rect, float baselineY)
		{
			var segments = new List<List<PointF>>();
			var currentSegment = new List<PointF>();

			for (int i = 0; i < DataCount; i++)
			{
				if (double.IsNaN(yValues[i]))
				{
					if (currentSegment.Any())
					{
						segments.Add(currentSegment);
						currentSegment = new List<PointF>();
					}
				}
				else
				{
					currentSegment.Add(TransformToVisible(xValues[i], yValues[i], rect));
				}
			}

			if (currentSegment.Any())
			{
				segments.Add(currentSegment);
			}

			return segments;
		}

		#endregion
	}
}