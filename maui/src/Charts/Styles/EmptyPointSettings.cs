namespace Syncfusion.Maui.Toolkit.Charts
{

	/// <summary>
	/// Used to customize the empty points which are NaN data points.
	/// </summary>
	/// <remarks>
	/// <para> By customizing these empty points, the positions of the data points are highlighted, thereby improving data visualization. </para>
	/// 
	/// <para> EmptyPointSettings class provides properties to customize these empty points by modifying attributes such as <see cref="EmptyPointSettings.Fill"/>, <see cref="EmptyPointSettings.Stroke"/>, and <see cref="EmptyPointSettings.StrokeWidth"/>. </para>
	///
	/// <para> To customize empty points, create an instance of <see cref="EmptyPointSettings"/>, configure it as needed, and then add it to the series.</para>
	///
	/// <para> EmptyPointSettings is not supported for all area-related series, as well as for FastChart and ErrorBarSeries.</para>
	/// # [MainPage.xaml](#tab/tabid-1)
	/// <code><![CDATA[
	/// <chart:SfCartesianChart>
	/// 
	///         <chart:SfCartesianChart.BindingContext>
	///             <local:ViewModel/>
	///         </chart:SfCartesianChart.BindingContext>
	/// 
	///         <chart:SfCartesianChart.XAxes>
	///             <chart:CategoryAxis/>
	///         </chart:SfCartesianChart.XAxes>
	/// 
	///         <chart:SfCartesianChart.YAxes>
	///             <chart:NumericalAxis/>
	///         </chart:SfCartesianChart.YAxes>
	///         
	///         <chart:SfCartesianChart.Series>
	///            <chart:LineSeries ItemsSource = "{Binding Data}" XBindingPath="XValue" YBindingPath="YValue" EmptyPointMode="Zero"/>
	///            <chart:LineSeries.EmptyPointSettings>
	///                    <chart:EmptyPointSettings Fill="Orange" Stroke="Red" StrokeWidth="3"/>
	///                </chart:LineSeries.EmptyPointSettings>         
	///        </chart:SfCartesianChart.Series>
	///        
	/// </chart:SfCartesianChart>
	/// ]]>
	/// </code>
	/// # [MainPage.xaml.cs](#tab/tabid-2)
	/// <code><![CDATA[
	/// SfCartesianChart chart = new SfCartesianChart();
	/// 
	/// ViewModel viewModel = new ViewModel();
	///	chart.BindingContext = viewModel;
	/// 
	/// NumericalAxis xaxis = new NumericalAxis();
	/// chart.XAxes.Add(xaxis);	
	/// 
	/// NumericalAxis yAxis = new NumericalAxis();
	/// chart.YAxes.Add(yAxis);
	/// 
	///  LineSeries series = new LineSeries();
	///  series.ItemsSource = viewModel.Data;
	///  series.XBindingPath = "XValue";
	///  series.YBindingPath = "YValue";
	///  series.EmptyPointMode = EmptyPointMode.Zero;
	///  series.EmptyPointSettings = new EmptyPointSettings();
	///  series.EmptyPointSettings.Fill = Colors.Orange;
	///  series.EmptyPointSettings.Stroke = Colors.Red;
	///  series.EmptyPointSettings.StrokeWidth = 3; 
	///  chart.Series.Add(series);
	/// ]]>
	/// </code>
	/// ***
	/// </remarks>
	public class EmptyPointSettings : Element
	{
		#region Bindable Properties

		/// <summary>
		/// Identifies the <see cref="Fill"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="Fill"/> bindable property determines the fill color of the empty point.
		/// </remarks>
		public static readonly BindableProperty FillProperty =
			BindableProperty.Create(nameof(Fill), typeof(Brush), typeof(EmptyPointSettings), null, BindingMode.Default, null, OnFillChanged, null, defaultValueCreator: FillDefaultValueCreator);

		/// <summary>
		/// Identifies the <see cref="Stroke"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="Stroke"/> bindable property determines the stroke color of the empty point.
		/// </remarks>
		public static readonly BindableProperty StrokeProperty =
			BindableProperty.Create(nameof(Stroke), typeof(Brush), typeof(EmptyPointSettings), null, BindingMode.Default, null, OnStrokeChanged, null, defaultValueCreator: StrokeDefaultValueCreator);

		/// <summary>
		/// Identifies the <see cref="StrokeWidth"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="StrokeWidth"/> bindable property determines the stroke width of the empty point.
		/// </remarks>
		public static readonly BindableProperty StrokeWidthProperty =
			BindableProperty.Create(nameof(StrokeWidth), typeof(double), typeof(EmptyPointSettings), 1.0);

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets the value to fill empty point.
		/// </summary>
		/// <value>It accepts a <see cref="Brush"/> value and its default value is FF4F4F.</value>
		/// <example>
		/// # [Xaml](#tab/tabid-3)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///     <!-- ... Eliminated for simplicity-->
		///
		///          <chart:LineSeries ItemsSource="{Binding Data}"
		///                            XBindingPath="XValue"
		///                            YBindingPath="YValue"
		///                            EmptyPointMode="Zero">
		///                <chart:LineSeries.EmptyPointSettings>
		///                    <chart:EmptyPointSettings Fill="Orange"/>
		///                </chart:LineSeries.EmptyPointSettings>   
		///          </chart:LineSeries>
		///									
		///     </chart:SfCartesianChart>
		/// ]]></code>
		/// # [C#](#tab/tabid-4)
		/// <code><![CDATA[
		///     SfCartesianChart chart = new SfCartesianChart();
		///     ViewModel viewModel = new ViewModel();
		///
		///     // Eliminated for simplicity
		///
		///     LineSeries series = new LineSeries()
		///     {
		///           ItemsSource = viewModel.Data,
		///           XBindingPath = "XValue",
		///           YBindingPath = "YValue",
		///           EmptyPointMode = EmptyPointMode.Zero,
		///     };
		///     series.EmptyPointSettings = new EmptyPointSettings();
		///     series.EmptyPointSettings.Fill = Colors.Orange;
		///     chart.Series.Add(series);
		///
		/// ]]></code>
		/// ***
		/// </example>
		public Brush Fill
		{
			get { return (Brush)GetValue(FillProperty); }
			set { SetValue(FillProperty, value); }
		}

		/// <summary>
		/// Gets or sets the value for empty point stroke.
		/// </summary>
		/// <value>It accepts <see cref="Brush"/> values and its default value is <c>Transparent</c>.</value>
		/// <remarks><para> EmptyPointSettings of stroke is not supported for Waterfall series.</para></remarks>
		/// <example>
		/// # [Xaml](#tab/tabid-5)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///     <!-- ... Eliminated for simplicity-->
		///
		///          <chart:ColumnSeries ItemsSource="{Binding Data}"
		///                            XBindingPath="XValue"
		///                            YBindingPath="YValue"
		///                            EmptyPointMode="Average">
		///                <chart:ColumnSeries.EmptyPointSettings>
		///                    <chart:EmptyPointSettings Stroke="Red" StrokeWidth="3"/>
		///                </chart:ColumnSeries.EmptyPointSettings>   
		///          </chart:ColumnSeries>
		///									
		///     </chart:SfCartesianChart>
		/// ]]></code>
		/// # [C#](#tab/tabid-6)
		/// <code><![CDATA[
		///     SfCartesianChart chart = new SfCartesianChart();
		///     ViewModel viewModel = new ViewModel();
		///
		///     // Eliminated for simplicity
		///
		///     ColumnSeries series = new ColumnSeries()
		///     {
		///           ItemsSource = viewModel.Data,
		///           XBindingPath = "XValue",
		///           YBindingPath = "YValue",
		///           EmptyPointMode = EmptyPointMode.Average,
		///     };
		///     series.EmptyPointSettings = new EmptyPointSettings();
		///     series.EmptyPointSettings.Stroke = Colors.Red;
		///     series.EmptyPointSettings.StrokeWidth = 3; 
		///     chart.Series.Add(series);
		///
		/// ]]></code>
		/// ***
		/// </example>
		public Brush Stroke
		{
			get { return (Brush)GetValue(StrokeProperty); }
			set { SetValue(StrokeProperty, value); }
		}

		/// <summary>
		/// Gets or sets the value for empty point stroke width.
		/// </summary> 
		/// <remarks>The value needs to be greater than zero.
		/// <para> EmptyPointSettings of StrokeWidth is not supported for Waterfall series.</para>
		/// </remarks>
		/// <value>It accepts <c>double</c> values and its default value is 1.</value>
		/// <example>
		/// # [Xaml](#tab/tabid-7)
		/// <code><![CDATA[
		///     <chart:SfCartesianChart>
		///
		///     <!-- ... Eliminated for simplicity-->
		///
		///          <chart:LineSeries ItemsSource="{Binding Data}"
		///                            XBindingPath="XValue"
		///                            YBindingPath="YValue"
		///                            EmptyPointMode="Zero">
		///                <chart:LineSeries.EmptyPointSettings>
		///                    <chart:EmptyPointSettings StrokeWidth="3"/>
		///                </chart:LineSeries.EmptyPointSettings>   
		///          </chart:LineSeries>
		///									
		///     </chart:SfCartesianChart>
		/// ]]></code>
		/// # [C#](#tab/tabid-8)
		/// <code><![CDATA[
		///     SfCartesianChart chart = new SfCartesianChart();
		///     ViewModel viewModel = new ViewModel();
		///
		///     // Eliminated for simplicity
		///
		///     LineSeries series = new LineSeries()
		///     {
		///           ItemsSource = viewModel.Data,
		///           XBindingPath = "XValue",
		///           YBindingPath = "YValue",
		///           EmptyPointMode = EmptyPointMode.Zero,
		///     };
		///     series.EmptyPointSettings = new EmptyPointSettings();
		///     series.EmptyPointSettings.StrokeWidth = 3; 
		///     chart.Series.Add(series);
		///
		/// ]]></code>
		/// ***
		/// </example>
		public double StrokeWidth
		{
			get { return (double)GetValue(StrokeWidthProperty); }
			set { SetValue(StrokeWidthProperty, value); }
		}

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="EmptyPointSettings"/>
		/// </summary>
		public EmptyPointSettings()
		{
		}

		#endregion

		#region Methods

		#region private methods

		static void OnFillChanged(BindableObject bindable, object oldValue, object newValue)
		{
		}

		static void OnStrokeChanged(BindableObject bindable, object oldValue, object newValue)
		{
		}

		static object? StrokeDefaultValueCreator(BindableObject bindable)
		{
			return null;
		}
		static object FillDefaultValueCreator(BindableObject bindable)
		{
			return new SolidColorBrush(Color.FromArgb("FF4E4E"));
		}

		#endregion

		#endregion
	}
}