namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// Used to customize the data marker symbol
	/// </summary>
	/// <remarks>
	/// <para> By customizing data marker symbol, the position of the data points are highlighted and improves data visualization. </para>
	/// 
	/// <para> ChartMarkerSettings class provides properties to customize the marker symbol by changing the type, fill, stroke, strokewidth, width and height. </para>
	///
	/// <para><b>Series</b></para>
	/// 
	/// <para> Data marker symbol renders on series types such as Line, Spline and Area series. Based on your requirements and specifications, series and data marker can be added for data visualization.</para>
	/// 
	/// <para>To render data marker, create an instance of <see cref="ChartMarkerSettings"/> and set it to the MarkerSettings and add it to the series</para>
	/// 
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
	///            <chart:LineSeries ItemsSource = "{Binding Data}" XBindingPath="XValue" YBindingPath="YValue1" ShowMarker="True"/>
	///            <chart:LineSeries.MarkerSettings>
	///                    <chart:ChartMarkerSettings Type="Diamond" Fill="white" Stroke="Red" StrokeWidth="1" Width="8" Height="8"/>
	///                </chart:LineSeries.MarkerSettings>         
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
	///  series.ShowMarker = true;
	///  series.MarkerSettings = new ChartMarkerSettings();
	///  series.MarkerSettings.Type = ShapeType.Diamond;
	///  series.MarkerSettings.Fill = Colors.White;
	///  series.MarkerSettings.Stroke = Colors.Red;
	///  series.MarkerSettings.StrokeWidth = 1;
	///  series.MarkerSettings.Width = 8;
	///  series.MarkerSettings.Height = 8;
	/// chart.Series.Add(series);
	/// ]]>
	/// </code>
	/// ***
	/// </remarks>
	public partial class ChartMarkerSettings : BindableObject
	{
		#region Bindable Properties

		/// <summary>
		/// Identifies the <see cref="Type"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="Type"/> bindable property determines the shape type of the data marker.
		/// </remarks>
		public static readonly BindableProperty TypeProperty = BindableProperty.Create(
			nameof(Type),
			typeof(ShapeType),
			typeof(ChartMarkerSettings),
			ShapeType.Circle);

		/// <summary>
		/// Identifies the <see cref="Fill"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="Fill"/> bindable property determines the fill color of the data marker.
		/// </remarks>
		public static readonly BindableProperty FillProperty = BindableProperty.Create(
			nameof(Fill),
			typeof(Brush),
			typeof(ChartMarkerSettings),
			default(Brush));

		/// <summary>
		/// Identifies the <see cref="Stroke"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="Stroke"/> bindable property determines the stroke color of the data marker.
		/// </remarks>
		public static readonly BindableProperty StrokeProperty = BindableProperty.Create(
			nameof(Stroke),
			typeof(Brush),
			typeof(ChartMarkerSettings),
			default(Brush));

		/// <summary>
		/// Identifies the <see cref="StrokeWidth"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="StrokeWidth"/> bindable property determines the stroke width of the data marker.
		/// </remarks>
		public static readonly BindableProperty StrokeWidthProperty = BindableProperty.Create(
			nameof(StrokeWidth),
			typeof(double),
			typeof(ChartMarkerSettings),
			0d);

		/// <summary>
		/// Identifies the <see cref="Width"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="Width"/> bindable property determines the width of the data marker.
		/// </remarks>
		public static readonly BindableProperty WidthProperty = BindableProperty.Create(
			nameof(Width),
			typeof(double),
			typeof(ChartMarkerSettings),
			8d);

		/// <summary>
		/// Identifies the <see cref="Height"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="Height"/> bindable property determines the height of the data marker.
		/// </remarks>
		public static readonly BindableProperty HeightProperty = BindableProperty.Create(
			nameof(Height),
			typeof(double),
			typeof(ChartMarkerSettings),
			8d);

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets the option for marker shape type.
		/// </summary>
		public ShapeType Type
		{
			get { return (ShapeType)GetValue(TypeProperty); }
			set { SetValue(TypeProperty, value); }
		}

		/// <summary>
		/// Gets or sets the value to fill marker.
		/// </summary>
		public Brush Fill
		{
			get { return (Brush)GetValue(FillProperty); }
			set { SetValue(FillProperty, value); }
		}

		/// <summary>
		/// Gets or sets the value for marker stroke.
		/// </summary>
		public Brush Stroke
		{
			get { return (Brush)GetValue(StrokeProperty); }
			set { SetValue(StrokeProperty, value); }
		}

		/// <summary>
		/// Gets or sets the value for marker stroke width.
		/// </summary>
		/// <remarks>The value needs to be greater than zero.</remarks>
		public double StrokeWidth
		{
			get { return (double)GetValue(StrokeWidthProperty); }
			set { SetValue(StrokeWidthProperty, value); }
		}

		/// <summary>
		/// Gets or sets the value for marker width.
		/// </summary>
		public double Width
		{
			get { return (double)GetValue(WidthProperty); }
			set { SetValue(WidthProperty, value); }
		}

		/// <summary>
		/// Gets or sets the value for marker height.
		/// </summary>
		public double Height
		{
			get { return (double)GetValue(HeightProperty); }
			set { SetValue(HeightProperty, value); }
		}

		internal bool HasBorder { get { return StrokeWidth > 0 && Stroke != default(Brush); } }

		#endregion

		#region Methods

		internal void HookStylePropertyChanged(object sender, ChartMarkerSettings markerSettings)
		{
			if (markerSettings != null)
			{
				SetInheritedBindingContext(markerSettings, BindingContext);
				markerSettings.PropertyChanged += ((IMarkerDependent)sender).Setting_PropertyChanged;
			}
		}

		internal static void UnHookStylePropertyChanged(object sender, ChartMarkerSettings markerSettings)
		{
			if (markerSettings != null)
			{
				SetInheritedBindingContext(markerSettings, null);
				markerSettings.PropertyChanged -= ((IMarkerDependent)sender).Setting_PropertyChanged;
			}
		}

		#endregion
	}
}
