using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// Serves as a base class for <see cref="NumericalPlotBand"/>, <see cref="DateTimePlotBand"/>. This class provides options to customize the appearance of plot bands.
	/// </summary>
	/// <remarks>
	///   
	/// <para> <b>IsVisible - </b> To customize the visibility of plot band, refer to this <see cref="IsVisible"/> property. </para>  
	/// <para> <b>AssociatedAxisStart - </b> To customize the segment start of the plot band, refer to this <see cref="AssociatedAxisStart"/> property. </para>
	/// <para> <b>AssociatedAxisEnd - </b> To customize the segment end of the plot band, refer to this <see cref="AssociatedAxisEnd"/> property. </para>
	/// <para> <b>AssociatedAxisName - </b> To set the axis name for the plot band, refer to this <see cref="AssociatedAxisName"/> property. </para>
	/// <para> <b>Fill - </b> To customize the background color of the plot band, refer to this <see cref="Fill"/> property. </para>
	/// <para> <b>Stroke - </b> To customize the stroke color, refer to this <see cref="Stroke"/> property. </para>
	/// <para> <b>StrokeWidth - </b> To customize the stroke width, refer to this <see cref="StrokeWidth"/> property. </para>
	/// <para> <b>Text - </b> To set the label for the plot band, refer to this <see cref="Text"/> property. </para>
	/// <para> <b>Size - </b> To customize the width of the plot band, refer to this <see cref="Size"/> property. </para>
	/// <para> <b>IsRepeatable - </b> To customize the repetition of the plot band, refer to this <see cref="IsRepeatable"/> property. </para>
	/// <para> <b>LabelStyle - </b> To customize the label for the plot band, refer to this <see cref="LabelStyle"/> property. </para>   
	/// 
	/// </remarks>
	public abstract class ChartPlotBand : Element
	{
		#region Fields

		ChartPlotBandLabelStyle _actualLabelStyle;

		#endregion

		#region Bindable Properties

		/// <summary>
		/// Identifies the <see cref="IsPixelWidth"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="IsPixelWidth"/> bindable property determines whether 
		/// the width of the plot band is measured in pixels.
		/// </remarks>
		internal static readonly BindableProperty IsPixelWidthProperty = BindableProperty.Create(
			nameof(IsPixelWidth),
			typeof(bool),
			typeof(ChartPlotBand),
			false,
			BindingMode.Default,
			null,
			OnPlotBandPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="AssociatedAxisStart"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="AssociatedAxisStart"/> bindable property determines 
		/// the start value of the associated axis for the plot band.
		/// </remarks>
		public static readonly BindableProperty AssociatedAxisStartProperty = BindableProperty.Create(
			nameof(AssociatedAxisStart),
			typeof(object),
			typeof(ChartPlotBand),
			null,
			BindingMode.Default,
			null,
			OnPlotBandPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="AssociatedAxisEnd"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="AssociatedAxisEnd"/> bindable property determines 
		/// the end value of the associated axis for the plot band.
		/// </remarks>
		public static readonly BindableProperty AssociatedAxisEndProperty = BindableProperty.Create(
			nameof(AssociatedAxisEnd),
			typeof(object),
			typeof(ChartPlotBand),
			null,
			BindingMode.Default,
			null,
			OnPlotBandPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="AssociatedAxisName"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="AssociatedAxisName"/> bindable property determines 
		/// the name of the associated axis for the plot band.
		/// </remarks>
		public static readonly BindableProperty AssociatedAxisNameProperty = BindableProperty.Create(
			nameof(AssociatedAxisName),
			typeof(string),
			typeof(ChartPlotBand),
			string.Empty,
			BindingMode.Default,
			null,
			OnPlotBandPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="Fill"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="Fill"/> bindable property determines the fill color of the plot band.
		/// </remarks>
		public static readonly BindableProperty FillProperty = BindableProperty.Create(
			nameof(Fill),
			typeof(Brush),
			typeof(ChartPlotBand),
			null,
			BindingMode.Default,
			null,
			OnPlotBandPropertyChanged,
			defaultValueCreator: FillDefaultValueCreator);

		/// <summary>
		/// Identifies the <see cref="Stroke"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="Stroke"/> bindable property determines the stroke color of the plot band.
		/// </remarks>
		public static readonly BindableProperty StrokeProperty = BindableProperty.Create(
			nameof(Stroke),
			typeof(Brush),
			typeof(ChartPlotBand),
			Brush.Default,
			BindingMode.Default,
			null,
			OnPlotBandPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="StrokeWidth"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="StrokeWidth"/> bindable property determines the width 
		/// of the stroke for the plot band.
		/// </remarks>
		public static readonly BindableProperty StrokeWidthProperty = BindableProperty.Create(
			nameof(StrokeWidth),
			typeof(double),
			typeof(ChartPlotBand),
			1d,
			BindingMode.Default,
			null,
			OnPlotBandPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="StrokeDashArray"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="StrokeDashArray"/> bindable property determines 
		/// the dash pattern of the stroke for the plot band.
		/// </remarks>
		public static readonly BindableProperty StrokeDashArrayProperty = BindableProperty.Create(
			nameof(StrokeDashArray),
			typeof(DoubleCollection),
			typeof(ChartPlotBand),
			null,
			BindingMode.Default,
			null,
			OnPlotBandPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="IsVisible"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="IsVisible"/> bindable property determines whether the plot band is visible.
		/// </remarks>
		public static readonly BindableProperty IsVisibleProperty = BindableProperty.Create(
			nameof(IsVisible),
			typeof(bool),
			typeof(ChartPlotBand),
			true,
			BindingMode.Default,
			null,
			OnPlotBandPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="Text"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="Text"/> bindable property determines the text displayed in the plot band.
		/// </remarks>
		public static readonly BindableProperty TextProperty = BindableProperty.Create(
			nameof(Text),
			typeof(string),
			typeof(ChartPlotBand),
			string.Empty,
			BindingMode.Default,
			null,
			OnPlotBandPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="Size"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="Size"/> bindable property determines the size of the plot band.
		/// </remarks>
		public static readonly BindableProperty SizeProperty = BindableProperty.Create(
			nameof(Size),
			typeof(double),
			typeof(ChartPlotBand),
			double.NaN,
			BindingMode.Default,
			null,
			OnPlotBandPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="RepeatEvery"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="RepeatEvery"/> bindable property determines the 
		/// interval at which the plot band repeats.
		/// </remarks>
		public static readonly BindableProperty RepeatEveryProperty = BindableProperty.Create(
			nameof(RepeatEvery),
			typeof(double),
			typeof(ChartPlotBand),
			double.NaN,
			BindingMode.Default,
			null,
			OnPlotBandPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="IsRepeatable"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="IsRepeatable"/> bindable property determines whether 
		/// the plot band is repeatable.
		/// </remarks>
		public static readonly BindableProperty IsRepeatableProperty = BindableProperty.Create(
			nameof(IsRepeatable),
			typeof(bool),
			typeof(ChartPlotBand),
			false,
			BindingMode.Default,
			null,
			OnPlotBandPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="LabelStyle"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="LabelStyle"/> bindable property determines the style of 
		/// the label displayed in the plot band.
		/// </remarks>
		public static readonly BindableProperty LabelStyleProperty = BindableProperty.Create(
			nameof(LabelStyle),
			typeof(ChartPlotBandLabelStyle),
			typeof(ChartPlotBand),
			null,
			BindingMode.Default,
			null,
			OnLabelStylePropertyChanged);

		#endregion

		#region Constructor

		/// <summary>
		///  Initializes a new instance of the <see cref="ChartPlotBand"/> class.
		/// </summary>
		public ChartPlotBand()
		{
			_actualLabelStyle = new ChartPlotBandLabelStyle()
			{
				FontSize = 12f,
				TextColor = Color.FromArgb("#000000"),
			};
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets the start value for the plot band segmentation. It can be a double, date ,time or logarithmic value.
		/// </summary>
		/// <value> This property takes <see cref= "object"/> value.</value>
		/// # [MainPage.xaml](#tab/tabid-1)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		/// 
		///     <chart:SfCartesianChart.XAxes>
		///         <chart:CategoryAxis>
		///            <chart:CategoryAxis.PlotBands>
		///              <chart:NumericalPlotBandCollection>
		///                <chart:NumericalPlotBand Start="1" End="2" AssociatedAxisStart="10"/>
		///                 </chart:NumericalPlotBandCollection>
		///            </chart:CategoryAxis.PlotBands>
		///        </chart:CategoryAxis>
		///     </chart:SfCartesianChart.XAxes>
		/// 
		/// </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [MainPage.xaml.cs](#tab/tabid-2)
		/// <code><![CDATA[
		/// SfCartesianChart chart = new SfCartesianChart();
		/// 
		/// CategoryAxis xAxis = new CategoryAxis();
		/// NumericalPlotBandCollection bands = new NumericalPlotBandCollection();
		/// NumericalPlotBand plotBand = new NumericalPlotBand();
		/// plotBand.Start = 1;
		/// plotBand.End = 2;
		/// plotBand.AssociatedAxisStart = 10;
		/// bands.Add(plotBand);
		/// xAxis.PlotBands = bands;
		/// 
		/// chart.XAxes.Add(xAxis);
		///
		/// ]]>
		/// </code>
		public object AssociatedAxisStart
		{
			get { return (object)GetValue(AssociatedAxisStartProperty); }
			set { SetValue(AssociatedAxisStartProperty, value); }
		}

		/// <summary>
		/// Gets or sets the end value for the plot band segmentation. It can be a double, date, time or logarithmic value.
		/// </summary>
		/// <value> This property takes <see cref= "object"/> value and the default value is null.</value>
		/// # [MainPage.xaml](#tab/tabid-1)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		/// 
		///     <chart:SfCartesianChart.XAxes>
		///         <chart:CategoryAxis>
		///            <chart:CategoryAxis.PlotBands>
		///               <chart:NumericalPlotBandCollection>
		///                <chart:NumericalPlotBand Start="1" End="2" AssociatedAxisEnd="20"/>
		///               </chart:NumericalPlotBandCollection>
		///            </chart:CategoryAxis.PlotBands>
		///        </chart:CategoryAxis>
		///     </chart:SfCartesianChart.XAxes>
		/// 
		/// </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [MainPage.xaml.cs](#tab/tabid-2)
		/// <code><![CDATA[
		/// SfCartesianChart chart = new SfCartesianChart();
		/// 
		/// CategoryAxis xAxis = new CategoryAxis();
		/// NumericalPlotBandCollection bands = new NumericalPlotBandCollection();
		/// NumericalPlotBand plotBand = new NumericalPlotBand();
		/// plotBand.Start = 1;
		/// plotBand.End = 2;
		/// plotBand.AssociatedAxisEnd = 20;
		/// bands.Add(plotBand);
		/// xAxis.PlotBands = bands;
		/// 
		/// chart.XAxes.Add(xAxis);
		///
		/// ]]>
		/// </code>
		public object AssociatedAxisEnd
		{
			get { return (object)GetValue(AssociatedAxisEndProperty); }
			set { SetValue(AssociatedAxisEndProperty, value); }
		}

		/// <summary>
		/// Gets or sets the name of the segment axis for the plot band.
		/// </summary>
		/// <value> This property takes <see cref= "string"/> value and the default value is string.Empty.</value>
		/// # [MainPage.xaml](#tab/tabid-1)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		/// 
		///     <chart:SfCartesianChart.XAxes>
		///         <chart:CategoryAxis>
		///            <chart:CategoryAxis.PlotBands>
		///               <chart:NumericalPlotBandCollection>
		///                <chart:NumericalPlotBand Start="1" End="2" AssociatedAxisName="YAxes"/>
		///               </chart:NumericalPlotBandCollection>
		///            </chart:CategoryAxis.PlotBands>
		///        </chart:CategoryAxis>
		///     </chart:SfCartesianChart.XAxes>
		/// 
		/// </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [MainPage.xaml.cs](#tab/tabid-2)
		/// <code><![CDATA[
		/// SfCartesianChart chart = new SfCartesianChart();
		/// 
		/// CategoryAxis xAxis = new CategoryAxis();
		/// NumericalPlotBandCollection bands = new NumericalPlotBandCollection();
		/// NumericalPlotBand plotBand = new NumericalPlotBand();
		/// plotBand.Start = 1;
		/// plotBand.End = 2;
		/// plotBand.AssociatedAxisName = "YAxes";
		/// bands.Add(plotBand);
		/// xAxis.PlotBands = bands;
		/// 
		/// chart.XAxes.Add(xAxis);
		///
		/// ]]>
		/// </code>
		public string AssociatedAxisName
		{
			get { return (string)GetValue(AssociatedAxisNameProperty); }
			set { SetValue(AssociatedAxisNameProperty, value); }
		}

		/// <summary>
		/// Gets or sets the color of the plot band. 
		/// </summary>
		/// <value> This property takes <see cref= "Brush"/> value.</value>
		/// # [MainPage.xaml](#tab/tabid-1)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		/// 
		///     <chart:SfCartesianChart.XAxes>
		///         <chart:CategoryAxis>
		///            <chart:CategoryAxis.PlotBands>
		///               <chart:NumericalPlotBandCollection>
		///                <chart:NumericalPlotBand Start="1" Size="2" Fill="Orange"/>
		///               </chart:NumericalPlotBandCollection>
		///            </chart:CategoryAxis.PlotBands>
		///        </chart:CategoryAxis>
		///     </chart:SfCartesianChart.XAxes>
		/// 
		/// </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [MainPage.xaml.cs](#tab/tabid-2)
		/// <code><![CDATA[
		/// SfCartesianChart chart = new SfCartesianChart();
		/// 
		/// CategoryAxis xAxis = new CategoryAxis();
		/// NumericalPlotBandCollection bands = new NumericalPlotBandCollection();
		/// NumericalPlotBand plotBand = new NumericalPlotBand();
		/// plotBand.Start = 1;
		/// plotBand.End = 2;
		/// plotBand.Fill= Colors.Orange;
		/// bands.Add(plotBand);
		/// xAxis.PlotBands = bands;
		/// 
		/// chart.XAxes.Add(xAxis);
		///
		/// ]]>
		/// </code>
		public Brush Fill
		{
			get { return (Brush)GetValue(FillProperty); }
			set { SetValue(FillProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value to customize the outer stroke appearance of the plot band.
		/// </summary>
		/// <value>It accepts <see cref="Brush"/> values.</value>
		/// # [MainPage.xaml](#tab/tabid-1)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		/// 
		///     <chart:SfCartesianChart.XAxes>
		///         <chart:CategoryAxis>
		///            <chart:CategoryAxis.PlotBands>
		///              <chart:NumericalPlotBandCollection>
		///                <chart:NumericalPlotBand Start="1" Size="2" Stroke="Red"/>
		///              </chart:NumericalPlotBandCollection>
		///            </chart:CategoryAxis.PlotBands>
		///        </chart:CategoryAxis>
		///     </chart:SfCartesianChart.XAxes>
		/// 
		/// </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [MainPage.xaml.cs](#tab/tabid-2)
		/// <code><![CDATA[
		/// SfCartesianChart chart = new SfCartesianChart();
		/// 
		/// CategoryAxis xAxis = new CategoryAxis();
		/// NumericalPlotBandCollection bands = new NumericalPlotBandCollection();
		/// NumericalPlotBand plotBand = new NumericalPlotBand();
		/// plotBand.Start = 1;
		/// plotBand.Size = 2;
		/// plotBand.Stroke = Colors.Red;
		/// bands.Add(plotBand);
		/// xAxis.PlotBands = bands;
		/// 
		/// chart.XAxes.Add(xAxis);
		///
		/// ]]>
		/// </code>
		public Brush Stroke
		{
			get { return (Brush)GetValue(StrokeProperty); }
			set { SetValue(StrokeProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value that indicates the stroke thickness of the plot band.
		/// </summary>
		/// <value>It accepts <see cref="double"/> values and the default value is 1.</value>
		/// # [MainPage.xaml](#tab/tabid-1)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		/// 
		///     <chart:SfCartesianChart.XAxes>
		///         <chart:CategoryAxis>
		///            <chart:CategoryAxis.PlotBands>
		///              <chart:NumericalPlotBandCollection>
		///                <chart:NumericalPlotBand Start="1" Size="2" StrokeWidth="3"/>
		///              </chart:NumericalPlotBandCollection>
		///            </chart:CategoryAxis.PlotBands>
		///        </chart:CategoryAxis>
		///     </chart:SfCartesianChart.XAxes>
		/// 
		/// </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [MainPage.xaml.cs](#tab/tabid-2)
		/// <code><![CDATA[
		/// SfCartesianChart chart = new SfCartesianChart();
		/// 
		/// CategoryAxis xAxis = new CategoryAxis();
		/// NumericalPlotBandCollection bands = new NumericalPlotBandCollection();
		/// NumericalPlotBand plotBand = new NumericalPlotBand();
		/// plotBand.Start = 1;
		/// plotBand.Size = 2;
		/// plotBand.StrokeWidth = 3;
		/// bands.Add(plotBand);
		/// xAxis.PlotBands = bands;
		/// 
		/// chart.XAxes.Add(xAxis);
		///
		/// ]]>
		/// </code>
		public double StrokeWidth
		{
			get { return (double)GetValue(StrokeWidthProperty); }
			set { SetValue(StrokeWidthProperty, value); }
		}

		/// <summary>
		/// Gets or sets the stroke dash array to customize the appearance of the stroke.
		/// </summary>
		/// <value>It accepts the <see cref="DoubleCollection"/> value and the default value is null.</value>
		/// # [MainPage.xaml](#tab/tabid-1)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		/// 
		///     <chart:SfCartesianChart.XAxes>
		///         <chart:CategoryAxis>
		///            <chart:CategoryAxis.PlotBands>
		///              <chart:NumericalPlotBandCollection>
		///                <chart:NumericalPlotBand Start="1" Size="2" StrokeDashArray ="3,3"/>
		///               </chart:NumericalPlotBandCollection>
		///            </chart:CategoryAxis.PlotBands>
		///        </chart:CategoryAxis>
		///     </chart:SfCartesianChart.XAxes>
		/// 
		/// </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [MainPage.xaml.cs](#tab/tabid-2)
		/// <code><![CDATA[
		/// SfCartesianChart chart = new SfCartesianChart();
		/// 
		/// CategoryAxis xAxis = new CategoryAxis();
		/// NumericalPlotBandCollection bands = new NumericalPlotBandCollection();
		/// DoubleCollection doubleCollection = new DoubleCollection();
		/// doubleCollection.Add(3);
		/// doubleCollection.Add(3);
		/// NumericalPlotBand plotBand = new NumericalPlotBand();
		/// plotBand.Start = 1;
		/// plotBand.Size = 2;
		/// plotBand.StrokeDashArray = doubleCollection;
		/// bands.Add(plotBand);
		/// xAxis.PlotBands = bands;
		/// 
		/// chart.XAxes.Add(xAxis);
		///
		/// ]]>
		/// </code>
		public DoubleCollection StrokeDashArray
		{
			get { return (DoubleCollection)GetValue(StrokeDashArrayProperty); }
			set { SetValue(StrokeDashArrayProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether the plot band is visible on the axis.
		/// </summary>
		/// <value>It accepts <see cref="bool"/> values and the default value is true.</value>
		/// # [MainPage.xaml](#tab/tabid-1)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		/// 
		///     <chart:SfCartesianChart.XAxes>
		///         <chart:CategoryAxis>
		///            <chart:CategoryAxis.PlotBands>
		///              <chart:NumericalPlotBandCollection>
		///                <chart:NumericalPlotBand Start="1" Size="2" IsVisible="False"/>
		///              </chart:NumericalPlotBandCollection>
		///            </chart:CategoryAxis.PlotBands>
		///        </chart:CategoryAxis>
		///     </chart:SfCartesianChart.XAxes>
		/// 
		/// </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [MainPage.xaml.cs](#tab/tabid-2)
		/// <code><![CDATA[
		/// SfCartesianChart chart = new SfCartesianChart();
		/// 
		/// CategoryAxis xAxis = new CategoryAxis();
		/// NumericalPlotBandCollection bands = new NumericalPlotBandCollection();
		/// NumericalPlotBand plotBand = new NumericalPlotBand();
		/// plotBand.Start = 1;
		/// plotBand.Size = 2;
		/// plotBand.IsVisible = false;
		/// bands.Add(plotBand);
		/// xAxis.PlotBands = bands;
		/// 
		/// chart.XAxes.Add(xAxis);
		///
		/// ]]>
		/// </code> 
		public bool IsVisible
		{
			get { return (bool)GetValue(IsVisibleProperty); }
			set { SetValue(IsVisibleProperty, value); }
		}

		/// <summary>
		/// Gets or sets the text to be displayed on the plot band.
		/// </summary>
		/// <value>This property takes the <see cref="string"/> value and the default value is string.Empty.</value>
		/// # [MainPage.xaml](#tab/tabid-1)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		/// 
		///     <chart:SfCartesianChart.XAxes>
		///         <chart:CategoryAxis>
		///            <chart:CategoryAxis.PlotBands>
		///              <chart:NumericalPlotBandCollection>
		///                <chart:NumericalPlotBand Start="1" Size="2" Text="Category PlotBand"/>
		///              </chart:NumericalPlotBandCollection>
		///            </chart:CategoryAxis.PlotBands>
		///        </chart:CategoryAxis>
		///     </chart:SfCartesianChart.XAxes>
		/// 
		/// </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [MainPage.xaml.cs](#tab/tabid-2)
		/// <code><![CDATA[
		/// SfCartesianChart chart = new SfCartesianChart();
		/// 
		/// CategoryAxis xAxis = new CategoryAxis();
		/// NumericalPlotBandCollection bands = new NumericalPlotBandCollection();
		/// NumericalPlotBand plotBand = new NumericalPlotBand();
		/// plotBand.Start = 1;
		/// plotBand.Size = 2;
		/// plotBand.Text = "Category PlotBand";
		/// bands.Add(plotBand);
		/// xAxis.PlotBands = bands;
		/// 
		/// chart.XAxes.Add(xAxis);
		///
		/// ]]>
		/// </code> 
		public string Text
		{
			get { return (string)GetValue(TextProperty); }
			set { SetValue(TextProperty, value); }
		}

		/// <summary>
		/// Gets or sets the size of the plot band.
		/// </summary>
		/// <value>This property takes the <see cref="double"/> value. Its default value is double.NaN.</value>
		/// # [MainPage.xaml](#tab/tabid-1)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		/// 
		///     <chart:SfCartesianChart.XAxes>
		///         <chart:CategoryAxis>
		///            <chart:CategoryAxis.PlotBands>
		///               <chart:NumericalPlotBandCollection>
		///                <chart:NumericalPlotBand Start="1" Size="1" RepeatEvery="2"/>
		///               </chart:NumericalPlotBandCollection>
		///            </chart:CategoryAxis.PlotBands>
		///        </chart:CategoryAxis>
		///     </chart:SfCartesianChart.XAxes>
		/// 
		/// </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [MainPage.xaml.cs](#tab/tabid-2)
		/// <code><![CDATA[
		/// SfCartesianChart chart = new SfCartesianChart();
		/// 
		/// CategoryAxis xAxis = new CategoryAxis();
		/// NumericalPlotBandCollection bands = new NumericalPlotBandCollection();
		/// NumericalPlotBand plotBand = new NumericalPlotBand();
		/// plotBand.Start = 1;
		/// plotBand.Size = 1;
		/// plotBand.RepeatEvery = 2;
		/// bands.Add(plotBand);
		/// xAxis.PlotBands = bands;
		/// 
		/// chart.XAxes.Add(xAxis);
		///
		/// ]]>
		/// </code> 
		public double Size
		{
			get { return (double)GetValue(SizeProperty); }
			set { SetValue(SizeProperty, value); }
		}

		/// <summary>
		/// Gets or sets the frequency of the plot band.
		/// </summary>
		/// <value>This property takes the <see cref="double"/> value. Its default value is double.NaN.</value>
		/// # [MainPage.xaml](#tab/tabid-1)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		/// 
		///     <chart:SfCartesianChart.XAxes>
		///         <chart:CategoryAxis>
		///            <chart:CategoryAxis.PlotBands>
		///              <chart:NumericalPlotBandCollection>
		///                <chart:NumericalPlotBand Start="1" Size="1" RepeatEvery="2"/>
		///              </chart:NumericalPlotBandCollection>
		///            </chart:CategoryAxis.PlotBands>
		///        </chart:CategoryAxis>
		///     </chart:SfCartesianChart.XAxes>
		/// 
		/// </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [MainPage.xaml.cs](#tab/tabid-2)
		/// <code><![CDATA[
		/// SfCartesianChart chart = new SfCartesianChart();
		/// 
		/// CategoryAxis xAxis = new CategoryAxis();
		/// NumericalPlotBandCollection bands = new NumericalPlotBandCollection();
		/// NumericalPlotBand plotBand = new NumericalPlotBand();
		/// plotBand.Start = 1;
		/// plotBand.Size = 1;
		/// plotBand.RepeatEvery = 2;
		/// bands.Add(plotBand);
		/// xAxis.PlotBands = bands;
		/// 
		/// chart.XAxes.Add(xAxis);
		///
		/// ]]>
		/// </code> 
		public double RepeatEvery
		{
			get { return (double)GetValue(RepeatEveryProperty); }
			set { SetValue(RepeatEveryProperty, value); }
		}

		/// <summary>
		/// Gets or sets the bool value to indicate the plot band recurrence.
		/// </summary>
		/// <value>This property takes the <see cref="bool"/> value. Its default value is false.</value>
		/// # [MainPage.xaml](#tab/tabid-1)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		/// 
		///     <chart:SfCartesianChart.XAxes>
		///         <chart:CategoryAxis>
		///            <chart:CategoryAxis.PlotBands>
		///              <chart:NumericalPlotBandCollection>
		///                <chart:NumericalPlotBand Start="1" Size="1" RepeatEvery="2" IsRepeatable="True"/>
		///              </chart:NumericalPlotBandCollection>
		///            </chart:CategoryAxis.PlotBands>
		///        </chart:CategoryAxis>
		///     </chart:SfCartesianChart.XAxes>
		/// 
		/// </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [MainPage.xaml.cs](#tab/tabid-2)
		/// <code><![CDATA[
		/// SfCartesianChart chart = new SfCartesianChart();
		/// 
		/// CategoryAxis xAxis = new CategoryAxis();
		/// NumericalPlotBandCollection bands = new NumericalPlotBandCollection();
		/// NumericalPlotBand plotBand = new NumericalPlotBand();
		/// plotBand.Start = 1;
		/// plotBand.Size = 1;
		/// plotBand.RepeatEvery = 2;
		/// plotBand.IsRepeatable = true;
		/// bands.Add(plotBand);
		/// xAxis.PlotBands = bands;
		/// 
		/// chart.XAxes.Add(xAxis);
		///
		/// ]]>
		/// </code> 
		public bool IsRepeatable
		{
			get { return (bool)GetValue(IsRepeatableProperty); }
			set { SetValue(IsRepeatableProperty, value); }
		}

		/// <summary>
		///  Gets or sets the customized style for the plot band labels. 
		/// </summary>
		/// <value>This property takes the <see cref="ChartPlotBandLabelStyle"/> as its value.</value>
		/// <example>
		/// # [Xaml](#tab/tabid-1)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		/// 
		///     <chart:SfCartesianChart.XAxes>
		///         <chart:CategoryAxis>
		///            <chart:CategoryAxis.PlotBands>
		///              <chart:NumericalPlotBandCollection>
		///                <chart:NumericalPlotBand Start="1" Size="1">
		///                 <chart:NumericalPlotBand.LabelStyle>
		///                  <chart:ChartPlotBandLabelStyle HorizontalTextAlignment="Start" Angle="90"/>
		///                 </chart:NumericalPlotBand.LabelStyle>
		///               </chart:NumericalPlotBandCollection>
		///            </chart:CategoryAxis.PlotBands>
		///        </chart:CategoryAxis>
		///     </chart:SfCartesianChart.XAxes>
		/// 
		/// </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [C#](#tab/tabid-2)
		/// <code><![CDATA[
		/// SfCartesianChart chart = new SfCartesianChart();
		/// 
		/// CategoryAxis xaxis = new CategoryAxis();
		/// NumericalPlotBandCollection bands = new NumericalPlotBandCollection();
		/// NumericalPlotBand plotBand = new NumericalPlotBand();
		/// plotBand.Start = 1;
		/// plotBand.Size = 1;
		/// ChartPlotBandLabelStyle style = new ChartPlotBandLabelStyle();
		/// style.HorizontalTextAlignment = ChartLabelAlignment.Start;
		/// style.Angle = 90;
		/// plotBand.LabelStyle = style;
		/// bands.Add(plotBand);
		/// xaxis.PlotBands = bands;
		/// 
		/// chart.XAxes.Add(xaxis);
		///
		/// ]]>
		/// </code> 
		/// *** 
		/// </example>
		public ChartPlotBandLabelStyle LabelStyle
		{
			get { return (ChartPlotBandLabelStyle)GetValue(LabelStyleProperty); }
			set { SetValue(LabelStyleProperty, value); }
		}

		#region Internal Properties

		internal ChartAxis? Axis { get; set; }

		internal bool IsSegmented => (AssociatedAxisStart != null) || (AssociatedAxisEnd != null);

		internal bool IsPixelWidth
		{
			get { return (bool)GetValue(IsPixelWidthProperty); }
			set { SetValue(IsPixelWidthProperty, value); }
		}

		#endregion

		#endregion

		#region Methods

		#region Internal Methods

		internal RectF GetVerticalDrawRect(ChartAxis axis, ChartAxis associatedAxis, double startBand, double endBand)
		{
			if (axis == null || associatedAxis == null || axis.Area == null)
			{
				return RectF.Zero;
			}

			var y1 = axis.ValueToPoint(startBand);
			float y2;

			if (!IsSegmented)
			{
				if (!(endBand < axis.VisibleRange.Start) && !(startBand > axis.VisibleRange.End))
				{
					y2 = axis.ValueToPoint(endBand);
					var top = axis.ArrangeRect.Top - axis.Area.ActualSeriesClipRect.Top;

					if (y1 > y2)
					{
						Swap(ref y1, ref y2);
					}

					y2 = (float)(y2 > top ? y2 : top);
					var x1 = associatedAxis.ArrangeRect.Left - axis.Area.ActualSeriesClipRect.Left;
					var x2 = associatedAxis.ArrangeRect.Width + associatedAxis.ArrangeRect.Left -
							 axis.Area.ActualSeriesClipRect.Left;
					return new RectF((float)x1, y1, (float)(x2 - x1), y2 - y1);
				}
			}
			else
			{
				var associatedAxisStart = AssociatedAxisStart == null ? associatedAxis.VisibleRange.Start : ChartUtils.ConvertToDouble(AssociatedAxisStart);
				var associatedAxisEnd = AssociatedAxisEnd == null ? associatedAxis.VisibleRange.End : ChartUtils.ConvertToDouble(AssociatedAxisEnd);

				if (!double.IsNaN(associatedAxisStart) && !double.IsNaN(associatedAxisEnd))
				{
					endBand = Math.Min(endBand, axis.VisibleRange.End);
					y2 = axis.ValueToPoint(endBand);

					var startValue = associatedAxis.ValueToPoint(associatedAxisStart);
					var endValue = associatedAxis.ValueToPoint(associatedAxisEnd);

					if (startValue > endValue)
					{
						Swap(ref startValue, ref endValue);
					}

					if (y1 > y2)
					{
						Swap(ref y1, ref y2);
					}

					return new RectF(startValue, y1, endValue - startValue, y2 - y1);
				}
			}

			return RectF.Zero;
		}

		internal RectF GetHorizontalDrawRect(ChartAxis axis, ChartAxis associatedAxis, double startBand, double endBand)
		{
			if (axis == null || associatedAxis == null || axis.Area == null)
			{
				return RectF.Zero;
			}

			var x1 = axis.ValueToPoint(startBand);
			float x2;

			if (!IsSegmented)
			{
				if (!(endBand < axis.VisibleRange.Start) && !(startBand > axis.VisibleRange.End))
				{
					x2 = axis.ValueToPoint(endBand);

					var left = axis.ArrangeRect.Width + axis.ArrangeRect.Left - axis.Area.ActualSeriesClipRect.Left;
					x2 = (float)(left > x2 ? x2 : left);

					if (x1 > x2)
					{
						Swap(ref x1, ref x2);
					}

					var y1 = 0;
					var y2 = associatedAxis.ArrangeRect.Height + associatedAxis.ArrangeRect.Top;
					return new RectF(x1, y1, x2 - x1, (float)(y2 - y1));
				}
			}
			else
			{
				var associatedAxisStart = AssociatedAxisStart == null ? associatedAxis.VisibleRange.Start : ChartUtils.ConvertToDouble(AssociatedAxisStart);
				var associatedAxisEnd = AssociatedAxisEnd == null ? associatedAxis.VisibleRange.End : ChartUtils.ConvertToDouble(AssociatedAxisEnd);

				if (!double.IsNaN(associatedAxisStart) && !double.IsNaN(associatedAxisEnd))
				{
					endBand = Math.Min(endBand, axis.VisibleRange.End);
					x2 = axis.ValueToPoint(endBand);

					var startValue = associatedAxis.ValueToPoint(associatedAxisStart);
					var endValue = associatedAxis.ValueToPoint(associatedAxisEnd);

					if (startValue > endValue)
					{
						Swap(ref startValue, ref endValue);
					}

					if (x1 > x2)
					{
						Swap(ref x1, ref x2);
					}

					return new RectF(x1, startValue, x2 - x1, endValue - startValue);
				}
			}

			return RectF.Zero;
		}

		internal void DrawPlotBandRect(ICanvas canvas, RectF drawRect)
		{
			if (!drawRect.IsEmpty)
			{
				canvas.CanvasSaveState();
				canvas.SetFillPaint(Fill, drawRect);
				canvas.FillRectangle(drawRect);

				if (StrokeWidth > 0 && !ChartColor.IsEmpty(Stroke.ToColor()))
				{
					canvas.StrokeSize = (float)StrokeWidth;
					canvas.StrokeColor = Stroke.ToColor();

					if (StrokeDashArray != null && StrokeDashArray.Count > 0)
					{
						canvas.StrokeDashPattern = StrokeDashArray.ToFloatArray();
					}

					canvas.DrawRectangle(drawRect);
				}

				if (!string.IsNullOrEmpty(Text))
				{
					DrawText(canvas, drawRect);
				}

				canvas.CanvasRestoreState();
			}
		}

		internal static void OnPlotBandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is ChartPlotBand plotBand)
			{
				plotBand.Axis?.InvalidatePlotBands();
			}
		}

		internal virtual void DrawPlotBands(ICanvas canvas, RectF dirtyRect)
		{

		}

		internal ChartAxis? GetAssociatedAxis(ChartAxis axis)
		{
			if (axis.CartesianArea is CartesianChartArea area)
			{
				if (area._xAxes.Contains(axis))
				{
					if (!string.IsNullOrEmpty(AssociatedAxisName))
					{
						foreach (var yAxis in area._yAxes)
						{
							if (yAxis.Name.Equals(AssociatedAxisName, StringComparison.Ordinal))
							{
								return yAxis;
							}
						}
					}
				}
				else if (area._yAxes.Contains(axis))
				{
					if (!string.IsNullOrEmpty(AssociatedAxisName))
					{
						foreach (var xAxis in area._xAxes)
						{
							if (xAxis.Name.Equals(AssociatedAxisName, StringComparison.Ordinal))
							{
								return xAxis;
							}
						}
					}
				}
			}

			return axis.AssociatedAxes.Count > 0 ? axis.AssociatedAxes[0] : null;
		}

		#endregion

		#region Protected Methods

		/// <inheritdoc/>
		/// <exclude/>
		protected override void OnParentSet()
		{
			base.OnParentSet();

			if (_actualLabelStyle != null)
			{
				_actualLabelStyle.Parent = Parent;
			}
		}

		/// <inheritdoc/>
		/// <exclude/>
		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();

			if (_actualLabelStyle != null)
			{
				SetInheritedBindingContext(_actualLabelStyle, BindingContext);
			}
		}

		#endregion

		#region Private Methods

		void DrawText(ICanvas canvas, RectF bandRect)
		{
		    float x = double.IsNaN(_actualLabelStyle.OffsetX) ? 0f : (float)_actualLabelStyle.OffsetX;
			float y = double.IsNaN(_actualLabelStyle.OffsetY) ? 0f : (float)_actualLabelStyle.OffsetY;
			Brush? fillColor = _actualLabelStyle.Background ?? Fill;

			var margin = _actualLabelStyle.Margin;
			var marginLeft = (float)margin.Left;
			var marginRight = (float)margin.Right;
			var marginTop = (float)margin.Top;
			var marginBottom = (float)margin.Bottom;

			// Measure label size and calculate actual size with margins
			SizeF labelSize = Text.Measure(_actualLabelStyle);
			SizeF actualSize = new SizeF(labelSize.Width + marginLeft + marginRight, labelSize.Height + marginTop + marginBottom);

			// Set horizontal alignment
			x = SetHorizontalAlignment(bandRect, actualSize, x);

			// Set vertical alignment
			y = SetVerticalAlignment(bandRect, actualSize, y);

			var backgroundRect = new RectF();

			DrawPlotBandBackgroundRect(canvas, fillColor, x, y, actualSize, backgroundRect);

			x += marginLeft;
			y += marginTop;

#if ANDROID
			y += actualSize.Height / 2;
#endif

			canvas.DrawText(Text, x, y, _actualLabelStyle);

			canvas.CanvasRestoreState();
		}

#pragma warning disable IDE0060 // Remove unused parameter
		void DrawPlotBandBackgroundRect(ICanvas canvas, Brush fillColor, float x, float y, SizeF labelSize, RectF backgroundRect)
#pragma warning restore IDE0060 // Remove unused parameter
		{
			if (!double.IsNaN(_actualLabelStyle.Angle))
			{
				float angle = (float)(_actualLabelStyle.Angle > 360 ? _actualLabelStyle.Angle % 360 : _actualLabelStyle.Angle);
				canvas.CanvasSaveState();
				canvas.Rotate(angle, x + labelSize.Width / 2, y + labelSize.Height / 2);
			}

			canvas.StrokeSize = (float)_actualLabelStyle.StrokeWidth;
			canvas.StrokeColor = _actualLabelStyle.Stroke.ToColor();

			backgroundRect = new RectF(x, y, labelSize.Width, labelSize.Height);
			canvas.SetFillPaint(fillColor, backgroundRect);
			canvas.FillRectangle(backgroundRect);
		}

		static void OnLabelStylePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is ChartPlotBand plotBand)
			{
				ChartBase.SetParent((Element?)oldValue, (Element?)newValue, plotBand.Parent);

				if (oldValue is ChartPlotBandLabelStyle style)
				{
					style.PropertyChanged -= plotBand.Style_PropertyChanged;
					style.Parent = null;
					SetInheritedBindingContext(style, null);
				}

				if (newValue is ChartPlotBandLabelStyle newStyle)
				{
					plotBand._actualLabelStyle = newStyle;
					newStyle.Parent = plotBand.Parent;
					SetInheritedBindingContext(newStyle, plotBand.BindingContext);
					newStyle.PropertyChanged += plotBand.Style_PropertyChanged;
				}
				else
				{
					plotBand._actualLabelStyle = new ChartPlotBandLabelStyle
					{
						Parent = plotBand.Parent
					};
					SetInheritedBindingContext(plotBand._actualLabelStyle, plotBand.BindingContext);
				}

				plotBand.Axis?.InvalidatePlotBands();
			}
		}

		void Style_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			Axis?.InvalidatePlotBands();
		}

		// Helper method for swapping values.
		static void Swap(ref float val1, ref float val2)
		{
			(val2, val1) = (val1, val2);
		}

		// Method to set horizontal alignment
		float SetHorizontalAlignment(RectF bandRect, SizeF actualSize, float x)
		{
			return _actualLabelStyle.HorizontalTextAlignment switch
			{
				ChartLabelAlignment.Center => x + bandRect.Left + (bandRect.Width - actualSize.Width) / 2,
				ChartLabelAlignment.End => x + (bandRect.Right - actualSize.Width),
				_ => x + bandRect.Left,
			};
		}

		// Method to set vertical alignment
		float SetVerticalAlignment(RectF bandRect, SizeF actualSize, float y)
		{
			return _actualLabelStyle.VerticalTextAlignment switch
			{
				ChartLabelAlignment.Center => y + bandRect.Top + (bandRect.Height - actualSize.Height) / 2,
				ChartLabelAlignment.End => y + (bandRect.Bottom - actualSize.Height),
				_ => y + bandRect.Top,
			};
		}

		static object FillDefaultValueCreator(BindableObject bindable)
		{
			return new SolidColorBrush(Color.FromArgb("#66E7E0EC"));
		}

		#endregion

		#endregion
	}
}