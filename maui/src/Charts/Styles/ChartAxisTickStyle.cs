using Syncfusion.Maui.Toolkit.Themes;

namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// Represents a style class that can be used to customize the axis's tick.
	/// </summary>
	/// <remarks>
	/// <para>To customize the chart axis's tick, create an instance of <see cref="ChartAxisTickStyle"/> and set it to the <see cref="ChartAxis.MajorTickStyle"/> property as shown in the following code sample.</para>
	/// # [MainWindow.xaml](#tab/tabid-1)
	/// <code><![CDATA[
	///     <chart:SfCartesianChart>
	///      
	///           <chart:SfCartesianChart.XAxes>
	///               <chart:NumericalAxis>
	///                   <chart:NumericalAxis.MajorTickStyle>
	///                       <chart:ChartAxisTickStyle Stroke="Red" StrokeWidth="1"/>
	///                   </chart:NumericalAxis.MajorTickStyle>
	///               </chart:NumericalAxis>
	///           </chart:SfCartesianChart.XAxes>
	///
	///           <chart:SfCartesianChart.YAxes>
	///               <chart:NumericalAxis/>
	///           </chart:SfCartesianChart.YAxes>
	///
	///     </chart:SfCartesianChart>
	/// ]]></code>
	/// # [MainWindow.cs](#tab/tabid-2)
	/// <code><![CDATA[
	///     SfCartesianChart chart = new SfCartesianChart();
	///     
	///     NumericalAxis xAxis = new NumericalAxis();
	///     xAxis.MajorTickStyle.StrokeWidth = 1;
	///     xAxis.MajorTickStyle.Stroke = new SolidColorBrush(Colors.Red);
	///     chart.XAxes.Add(xAxis);
	///     
	///     NumericalAxis yAxis = new NumericalAxis();
	///     chart.YAxes.Add(yAxis)
	/// ]]></code>
	/// ***
	/// 
	/// <para>It provides more options to customize the chart axis title.</para>
	/// 
	/// <para> <b>TickSize - </b> To customize the length of the axis tick, refer to this <see cref="TickSize"/> property.</para>
	/// <para> <b>Stroke - </b> To customize the color of the axis tick, refer to this <see cref="Stroke"/> property. </para>
	/// <para> <b>StrokeWidth - </b> To customize the width of the axis ticks, refer to this <see cref="StrokeWidth"/> property. </para>
	/// 
	/// </remarks>
	public partial class ChartAxisTickStyle : Element, IThemeElement
	{
		#region Bindable Properties

		/// <summary>
		/// Identifies the <see cref="TickSize"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="TickSize"/> bindable property determines the length of the axis tick.
		/// </remarks>
		public static readonly BindableProperty TickSizeProperty = BindableProperty.Create(
			nameof(TickSize),
			typeof(double),
			typeof(ChartAxisTickStyle),
			8d,
			BindingMode.Default,
			null,
			OnTickSizeChanged);

		/// <summary>
		/// Identifies the <see cref="Stroke"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="Stroke"/> bindable property determines the stroke color of the axis tick.
		/// </remarks>
		public static readonly BindableProperty StrokeProperty = BindableProperty.Create(
			nameof(Stroke),
			typeof(Brush),
			typeof(ChartAxisTickStyle),
			new SolidColorBrush(Color.FromArgb("#CAC4D0")),
			BindingMode.Default,
			null,
			OnStrokeColorChanged);

		/// <summary>
		/// Identifies the <see cref="StrokeWidth"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="StrokeWidth"/> bindable property determines the stroke width of the axis tick.
		/// </remarks>
		public static readonly BindableProperty StrokeWidthProperty = BindableProperty.Create(
			nameof(StrokeWidth),
			typeof(double),
			typeof(ChartAxisTickStyle),
			1d,
			BindingMode.Default,
			null,
			OnStrokeWidthChanged);

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets a value that indicates the length of the axis's tick.
		/// </summary>
		/// <value>It accepts <see cref="double"/> values and the default value is 8.</value>
		public double TickSize
		{
			get { return (double)GetValue(TickSizeProperty); }
			set { SetValue(TickSizeProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value to customize the stroke color of the axis's tick.
		/// </summary>
		/// <value>It accepts <see cref="Brush"/> values.</value>
		public Brush Stroke
		{
			get { return (Brush)GetValue(StrokeProperty); }
			set { SetValue(StrokeProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value that indicates the width of the axis's tick.
		/// </summary>
		/// <remarks>The value needs to be greater than zero.</remarks>
		/// <value>It accepts <see cref="double"/> values and the default value is 1.</value>
		public double StrokeWidth
		{
			get { return (double)GetValue(StrokeWidthProperty); }
			set { SetValue(StrokeWidthProperty, value); }
		}

		#endregion

		#region Constructor 

		/// <summary>
		/// Initializes a new instance of the <see cref="ChartAxisTickStyle"/>.
		/// </summary>
		public ChartAxisTickStyle()
		{
			ThemeElement.InitializeThemeResources(this, "SfCartesianChartTheme");
		}

		#endregion

		#region Internal Properties

		internal bool CanDraw()
		{
			return StrokeWidth > 0 && TickSize > 0;
		}

		#endregion

		#region Interface Implementation

		void IThemeElement.OnControlThemeChanged(string oldTheme, string newTheme)
		{
		}

		void IThemeElement.OnCommonThemeChanged(string oldTheme, string newTheme)
		{
		}

		#endregion

		#region Internal Methods

		internal void InitializeDynamicResource(string tickStyle)
		{
			if (tickStyle == "MajorTickStyle")
			{
				SetDynamicResource(StrokeProperty, "SfCartesianChartMajorTickLineStroke");
			}

			if (tickStyle == "MinorTickStyle")
			{
				SetDynamicResource(StrokeProperty, "SfCartesianChartMinorTickLineStroke");
			}
		}

		#endregion

		#region Callback
		static void OnTickSizeChanged(BindableObject bindable, object oldValue, object newValue)
		{
		}

		static void OnStrokeColorChanged(BindableObject bindable, object oldValue, object newValue)
		{
		}

		static void OnStrokeWidthChanged(BindableObject bindable, object oldValue, object newValue)
		{
		}

		#endregion

	}
}
