namespace Syncfusion.Maui.Toolkit.SparkCharts
{
	/// <summary>
	/// Configures the axis and trackball line for spark charts.
	/// </summary>
	public class SparkChartLineStyle : Element
	{
		/// <summary>
		/// Identifies the Stroke bindable property.
		/// </summary>
		public static readonly BindableProperty StrokeProperty =
			BindableProperty.Create(nameof(Stroke), typeof(Brush), typeof(SparkChartLineStyle), new SolidColorBrush(Color.FromArgb("#F9113D")));

		/// <summary>
		/// Identifies the StrokeWidth bindable property.
		/// </summary>
		public static readonly BindableProperty StrokeWidthProperty =
			BindableProperty.Create(nameof(StrokeWidth), typeof(double), typeof(SparkChartLineStyle), 1d);

		/// <summary>
		/// Identifies the StrokeDashArray bindable property.
		/// </summary>
		public static readonly BindableProperty StrokeDashArrayProperty =
			BindableProperty.Create(nameof(StrokeDashArray), typeof(DoubleCollection), typeof(SparkChartLineStyle), null);


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
		/// Gets or sets the stroke dash array to customize the appearance of the stroke.
		/// </summary>
		public DoubleCollection StrokeDashArray
		{
			get => (DoubleCollection)GetValue(StrokeDashArrayProperty);
			set => SetValue(StrokeDashArrayProperty, value);
		}
	}
}
