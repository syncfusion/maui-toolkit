using Syncfusion.Maui.Toolkit.Themes;

namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// Represents the chart axis's label style class.
	/// </summary>
	/// <remarks>
	/// To customize the axis labels appearance, create an instance of the <see cref="ChartAxisLabelStyle"/> class and set to the <see cref="ChartAxis.LabelStyle"/> property.
	/// 
	/// # [MainPage.xaml](#tab/tabid-1)
	/// <code><![CDATA[
	/// <chart:SfCartesianChart>
	///  
	///     <chart:SfCartesianChart.XAxes>
	///         <chart:CategoryAxis>
	///            <chart:CategoryAxis.LabelStyle>
	///                <chart:ChartAxisLabelStyle TextColor = "Red" FontSize="14"/>
	///            </chart:CategoryAxis.LabelStyle>
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
	/// CategoryAxis xaxis = new CategoryAxis();
	/// xaxis.LabelStyle = new ChartAxisLabelStyle()
	/// {
	///     TextColor = Colors.Red,
	///     FontSize = 14,
	/// };
	/// chart.XAxes.Add(xaxis);
	///
	/// ]]>
	/// </code>
	/// *** 
	/// <para>It provides more options to customize the chart axis label.</para>
	/// 
	/// <para> <b>LabelAlignment - </b> To position the axis label, refer to this <see cref="LabelAlignment"/> property.</para>
	/// <para> <b>LabelFormat - </b> To customize the numeric or date-time format of the axis label, refer to this <see cref="ChartLabelStyle.LabelFormat"/> property. </para>
	/// <para> <b>TextColor - </b> To customize the text color, refer to this <see cref="ChartLabelStyle.TextColor"/> property. </para>
	/// <para> <b>Background - </b> To customize the background color, refer to this <see cref="ChartLabelStyle.Background"/> property. </para>
	/// <para> <b>Stroke - </b> To customize the stroke color, refer to this <see cref="ChartLabelStyle.Stroke"/> property. </para>
	/// <para> <b>StrokeWidth - </b> To modify the stroke width, refer to this <see cref="ChartLabelStyle.StrokeWidth"/> property. </para>
	/// </remarks>
	public partial class ChartAxisLabelStyle : ChartLabelStyle
	{
		#region BindableProperties

		/// <summary>
		/// Identifies the <see cref="LabelAlignment"/> bindable property.
		/// </summary>   
		/// <remarks>
		/// The identifier for the <see cref="LabelAlignment"/> bindable property indicates the position of the axis label.
		/// </remarks>
		public static readonly BindableProperty LabelAlignmentProperty = BindableProperty.Create(
			nameof(LabelAlignment),
			typeof(ChartAxisLabelAlignment),
			typeof(ChartAxisLabelStyle),
			ChartAxisLabelAlignment.Center,
			BindingMode.Default,
			null,
			OnAxisAlignmentChanged);

		/// <summary>
		/// Identifies the <see cref="MaxWidth"/> bindable property.
		/// </summary>        
		/// <remarks>
		/// The identifier for the <see cref="MaxWidth"/> bindable property represents the 
		/// maximum width value for wrapped axis labels.
		/// </remarks>
		public static readonly BindableProperty MaxWidthProperty = BindableProperty.Create(
			nameof(MaxWidth),
			typeof(double),
			typeof(ChartAxisLabelStyle),
			double.NaN,
			BindingMode.Default,
			null,
			OnMaxWidthChanged);

		/// <summary>
		/// Identifies the <see cref="WrappedLabelAlignment"/> bindable property.
		/// </summary>        
		/// <remarks>
		/// The identifier for the <see cref="WrappedLabelAlignment"/> bindable property is used to align 
		/// wrapped axis labels based on the label's rectangle.
		/// </remarks>
		public static readonly BindableProperty WrappedLabelAlignmentProperty = BindableProperty.Create(
			nameof(WrappedLabelAlignment),
			typeof(ChartAxisLabelAlignment),
			typeof(ChartAxisLabelStyle),
			ChartAxisLabelAlignment.Start,
			BindingMode.Default,
			null,
			OnWrappedLabelAlignmentChanged);

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets a value that indicates the position of the axis label.
		/// </summary>
		/// <value>It accepts <see cref="ChartAxisLabelAlignment"/> values and the default value is <see cref="ChartAxisLabelAlignment.Center"/>.</value>
		/// <example>
		///  # [MainPage.xaml](#tab/tabid-3)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		///  <chart:SfCartesianChart.XAxes>
		///   <chart:CategoryAxis>
		///     <chart:CategoryAxis.LabelStyle>
		///      <chart:ChartAxisLabelStyle LabelAlignment="Center"/>
		///        </chart:CategoryAxis.LabelStyle>
		///   </chart:CategoryAxis>
		///  </chart:SfCartesianChart.XAxes>
		///
		///  <chart:SfCartesianChart.YAxes>
		///   <chart:NumericalAxis>
		///    <chart:NumericalAxis.LabelStyle>
		///           <chart:ChartAxisLabelStyleLabelAlignment="Center"/>
		///       </chart:NumericalAxi.LabelStyle>
		///  </chart:NumericalAxis>
		/// </chart:SfCartesianChart.YAxes>
		/// </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [MainPage.xaml.cs](#tab/tabid-4)
		/// <code><![CDATA[
		///   SfCartesianChart chart = new SfCartesianChart();
		///   CategoryAxis primaryAxis = new CategoryAxis();
		///   ChartAxisLabelStyle chartAxisLabelStyle = new ChartAxisLabelStyle()
		///   {
		///        LabelAlignment = ChartAxisLabelAlignment.Center,
		///   };
		///   primaryAxis.LabelStyle = chartAxisLabelStyle;
		///   chart.XAxes.Add(primaryAxis);
		///   NumericalAxis secondaryAxis = new NumericalAxis();
		///   ChartAxisLabelStyle secondaryAxisStyle = new ChartAxisLabelStyle()
		///   {
		///       LabelAlignment = ChartAxisLabelAlignment.Center,
		///   };
		///   secondaryAxis.LabelStyle = secondaryAxisStyle;
		///   chart.YAxes.Add(secondaryAxis);
		/// ]]>
		/// </code>
		/// </example>
		public ChartAxisLabelAlignment LabelAlignment
		{
			get { return (ChartAxisLabelAlignment)GetValue(LabelAlignmentProperty); }
			set { SetValue(LabelAlignmentProperty, value); }
		}

		/// <summary>
		/// Gets or sets the maximum width value for wrapped axis labels.
		/// </summary>
		/// <value>It accepts <c>double</c> values and its default value is double.NaN.</value>
		/// <example>
		///  # [MainPage.xaml](#tab/tabid-5)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		///  <chart:SfCartesianChart.XAxes>
		///   <chart:CategoryAxis LabelsIntersectAction="Wrap">
		///     <chart:CategoryAxis.LabelStyle>
		///      <chart:ChartAxisLabelStyle MaxWidth="30"/>
		///        </chart:CategoryAxis.LabelStyle>
		///   </chart:CategoryAxis>
		///  </chart:SfCartesianChart.XAxes>
		///
		///  <chart:SfCartesianChart.YAxes>
		///   <chart:NumericalAxis LabelsIntersectAction="Wrap">
		///    <chart:NumericalAxis.LabelStyle>
		///           <chart:ChartAxisLabelStyle MaxWidth="30"/>
		///       </chart:NumericalAxi.LabelStyle>
		///  </chart:NumericalAxis>
		/// </chart:SfCartesianChart.YAxes>
		/// </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [MainPage.xaml.cs](#tab/tabid-6)
		/// <code><![CDATA[
		///   SfCartesianChart chart = new SfCartesianChart();
		///   CategoryAxis primaryAxis = new CategoryAxis()
		///   {
		///        LabelsIntersectAction = AxisLabelsIntersectAction.Wrap,
		///   };
		///   ChartAxisLabelStyle chartAxisLabelStyle = new ChartAxisLabelStyle()
		///   {
		///        MaxWidth = 30,
		///   };
		///   primaryAxis.LabelStyle = chartAxisLabelStyle;
		///   chart.XAxes.Add(primaryAxis);
		///   NumericalAxis secondaryAxis = new NumericalAxis()
		///   {
		///       LabelsIntersectAction = AxisLabelsIntersectAction.Wrap,
		///   };
		///   ChartAxisLabelStyle secondaryAxisStyle = new ChartAxisLabelStyle()
		///   {
		///        MaxWidth = 30,
		///   };
		///   secondaryAxis.LabelStyle = secondaryAxisStyle;
		///   chart.YAxes.Add(secondaryAxis);
		/// ]]>
		/// </code>
		/// </example>
		public double MaxWidth
		{
			get { return (double)GetValue(MaxWidthProperty); }
			set { SetValue(MaxWidthProperty, value); }
		}

		/// <summary>
		/// Gets or sets the value to align wrapped axis labels based upon the labels rect.
		/// </summary>
		/// <value>It accepts <see cref="ChartAxisLabelAlignment"/> values and the default value is <see cref="ChartAxisLabelAlignment.Start"/>.</value>
		/// <example>
		///  # [MainPage.xaml](#tab/tabid-7)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		///  <chart:SfCartesianChart.XAxes>
		///   <chart:CategoryAxis LabelsIntersectAction="Wrap">
		///     <chart:CategoryAxis.LabelStyle>
		///      <chart:ChartAxisLabelStyle WrappedLabelAlignment="Center" MaxWidth="30"/>
		///        </chart:CategoryAxis.LabelStyle>
		///   </chart:CategoryAxis>
		///  </chart:SfCartesianChart.XAxes>
		///
		///  <chart:SfCartesianChart.YAxes>
		///   <chart:NumericalAxis LabelsIntersectAction="Wrap">
		///    <chart:NumericalAxis.LabelStyle>
		///           <chart:ChartAxisLabelStyle WrappedLabelAlignment="Center" MaxWidth="30"/>
		///       </chart:NumericalAxi.LabelStyle>
		///  </chart:NumericalAxis>
		/// </chart:SfCartesianChart.YAxes>
		/// </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [MainPage.xaml.cs](#tab/tabid-8)
		/// <code><![CDATA[
		///   SfCartesianChart chart = new SfCartesianChart();
		///   CategoryAxis primaryAxis = new CategoryAxis()
		///   {
		///        LabelsIntersectAction = AxisLabelsIntersectAction.Wrap,
		///   };
		///   ChartAxisLabelStyle chartAxisLabelStyle = new ChartAxisLabelStyle()
		///   {
		///        MaxWidth = 30,
		///        WrappedLabelAlignment = ChartAxisLabelAlignment.Center,
		///    };
		///    
		///    primaryAxis.LabelStyle = chartAxisLabelStyle;
		///    chart.XAxes.Add(primaryAxis);
		///    NumericalAxis secondaryAxis = new NumericalAxis()
		///    {
		///        LabelsIntersectAction = AxisLabelsIntersectAction.Wrap,
		///    };
		///    ChartAxisLabelStyle secondaryAxisStyle = new ChartAxisLabelStyle()
		///    {
		///         MaxWidth = 30,
		///         WrappedLabelAlignment = ChartAxisLabelAlignment.Center,
		///    };
		///    secondaryAxis.LabelStyle = secondaryAxisStyle;
		///    chart.YAxes.Add(secondaryAxis);
		/// ]]>
		/// </code>
		/// </example>
		public ChartAxisLabelAlignment WrappedLabelAlignment
		{
			get { return (ChartAxisLabelAlignment)GetValue(WrappedLabelAlignmentProperty); }
			set { SetValue(WrappedLabelAlignmentProperty, value); }
		}

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="ChartAxisLabelStyle"/>.
		/// </summary>
		public ChartAxisLabelStyle()
		{
			ThemeElement.InitializeThemeResources(this, "SfCartesianChartTheme");
		}

		#endregion

		#region Internal Properties

		internal ChartTextWrapMode TextWrapMode { get; set; } = ChartTextWrapMode.WordWrap;

		internal Dictionary<string, double>? WrapWidthCollection { get; set; }

		internal AxisLabelsIntersectAction LabelsIntersectAction { get; set; } = AxisLabelsIntersectAction.Hide;

		#endregion

		#region Methods

		#region Internal Methods

		internal override Color GetDefaultTextColor()
		{
			return Color.FromArgb("#49454F");
		}

		internal override Brush GetDefaultBackgroundColor()
		{
			return new SolidColorBrush(Colors.Transparent);
		}

		internal override double GetDefaultFontSize()
		{
			return 12;
		}

		internal override Thickness GetDefaultMargin()
		{
			return new Thickness(4f);
		}

		#endregion

		#region Private Methods

		static void OnAxisAlignmentChanged(BindableObject bindable, object oldValue, object newValue)
		{
		}

		static void OnMaxWidthChanged(BindableObject bindable, object oldValue, object newValue)
		{
		}

		static void OnWrappedLabelAlignmentChanged(BindableObject bindable, object oldValue, object newValue)
		{
		}

		#endregion

		#endregion
	}
}
