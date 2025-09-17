namespace Syncfusion.Maui.Toolkit.SparkCharts
{
	/// <summary>
	/// Configures the markers for spark charts.
	/// </summary>
	public class SparkChartMarkerSettings : Element
	{
		/// <summary>
		/// Identifies the Fill bindable property.
		/// </summary>
		public static readonly BindableProperty FillProperty = BindableProperty.Create(nameof(Fill), typeof(Brush), typeof(SparkChartMarkerSettings), null);

		/// <summary>
		/// Identifies the Height bindable property.
		/// </summary>
		public static readonly BindableProperty HeightProperty = BindableProperty.Create(nameof(Height), typeof(double), typeof(SparkChartMarkerSettings), 8d);

		/// <summary>
		/// Identifies the Width bindable property.
		/// </summary>
		public static readonly BindableProperty WidthProperty = BindableProperty.Create(nameof(Width), typeof(double), typeof(SparkChartMarkerSettings), 8d);

		/// <summary>
		/// Identifies the Stroke bindable property.
		/// </summary>
		public static readonly BindableProperty StrokeProperty = BindableProperty.Create(nameof(Stroke), typeof(Brush), typeof(SparkChartMarkerSettings), null);

		/// <summary>
		/// Identifies the StrokeWidth bindable property.
		/// </summary>
		public static readonly BindableProperty StrokeWidthProperty = BindableProperty.Create(nameof(StrokeWidth), typeof(double), typeof(SparkChartMarkerSettings), double.NaN);

		/// <summary>
		/// Identifies the ShapeType bindable property.
		/// </summary>
		public static readonly BindableProperty ShapeTypeProperty = BindableProperty.Create(nameof(ShapeType), typeof(SparkChartMarkerShape), typeof(SparkChartMarkerSettings), SparkChartMarkerShape.Circle);

		/// <summary>
		/// Gets or sets the color for the markers.
		/// </summary>
		public Brush Fill
		{
			get => (Brush)GetValue(FillProperty);
			set => SetValue(FillProperty, value);
		}

		/// <summary>
		/// Gets or sets the height for the markers.
		/// </summary>
		public double Height
		{
			get => (double)GetValue(HeightProperty);
			set => SetValue(HeightProperty, value);
		}

		/// <summary>
		/// Gets or sets the width for the markers.
		/// </summary>
		public double Width
		{
			get => (double)GetValue(WidthProperty);
			set => SetValue(WidthProperty, value);
		}

		/// <summary>
		/// Gets or sets the stroke for the marker.
		/// </summary>
		public Brush Stroke
		{
			get => (Brush)GetValue(StrokeProperty);
			set => SetValue(StrokeProperty, value);
		}

		/// <summary>
		/// Gets or sets the stroke width for the marker.
		/// </summary>
		public double StrokeWidth
		{
			get => (double)GetValue(StrokeWidthProperty);
			set => SetValue(StrokeWidthProperty, value);
		}

		/// <summary>
		/// Gets or sets the shape for the markers.
		/// </summary>
		public SparkChartMarkerShape ShapeType
		{
			get => (SparkChartMarkerShape)GetValue(ShapeTypeProperty);
			set => SetValue(ShapeTypeProperty, value);
		}
	}
}
