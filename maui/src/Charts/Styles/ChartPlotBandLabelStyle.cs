namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// Represents a LabelStyle class that can be used to customize the plot band labels.
	/// </summary>
	/// <remarks>
	/// It provides more options to customize the plot band label.
	/// <para> <b>HorizontalAlignment - </b> To adjust the alignment for labels, refer to this <see cref="HorizontalTextAlignment"/> property. </para>
	/// <para> <b>VerticalAlignment - </b> To adjust the alignment for labels, refer to this <see cref="VerticalTextAlignment"/> property. </para>
	/// <para> <b>Angle - </b> To adjust the angle rotation for labels, refer to this <see cref="Angle"/> property. </para>
	/// <para> <b>OffsetX - </b> To adjust the padding for labels, refer to this <see cref="OffsetX"/> property. </para>
	/// <para> <b>OffsetY - </b> To adjust the padding for labels, refer to this <see cref="OffsetY"/> property. </para>
	/// </remarks>
	public partial class ChartPlotBandLabelStyle : ChartLabelStyle
	{
		#region Bindable Properties

		/// <summary>
		/// Identifies the <see cref="HorizontalTextAlignment"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="HorizontalTextAlignment"/> bindable property determines the vertical
		/// text alignment of the chart plot band labels.
		/// </remarks>
		public static readonly BindableProperty HorizontalTextAlignmentProperty = BindableProperty.Create(
			nameof(HorizontalTextAlignment),
			typeof(ChartLabelAlignment),
			typeof(ChartPlotBandLabelStyle),
			ChartLabelAlignment.Center,
			BindingMode.Default,
			null);

		/// <summary>
		/// Identifies the <see cref="VerticalTextAlignment"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="VerticalTextAlignment"/> bindable property determines the vertical
		/// text alignment of the chart plot band labels.
		/// </remarks>
		public static readonly BindableProperty VerticalTextAlignmentProperty = BindableProperty.Create(
			nameof(VerticalTextAlignment),
			typeof(ChartLabelAlignment),
			typeof(ChartPlotBandLabelStyle),
			ChartLabelAlignment.Center,
			BindingMode.Default,
			null);

		/// <summary>
		/// Identifies the <see cref="Angle"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="Angle"/> bindable property determines the rotation angle of the chart plot band labels.
		/// </remarks>
		public static readonly BindableProperty AngleProperty = BindableProperty.Create(
			nameof(Angle),
			typeof(double),
			typeof(ChartPlotBandLabelStyle),
			0.0d,
			BindingMode.Default,
			null);

		/// <summary>
		/// Identifies the <see cref="OffsetX"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="OffsetX"/> bindable property determines the horizontal offset 
		/// of the chart plot band labels.
		/// </remarks>
		public static readonly BindableProperty OffsetXProperty = BindableProperty.Create(
			nameof(OffsetX),
			typeof(double),
			typeof(ChartPlotBandLabelStyle),
			0.0d,
			BindingMode.Default,
			null);

		/// <summary>
		/// Identifies the <see cref="OffsetY"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="OffsetY"/> bindable property determines the vertical offset
		/// of the chart plot band labels.
		/// </remarks>
		public static readonly BindableProperty OffsetYProperty = BindableProperty.Create(
			nameof(OffsetY),
			typeof(double),
			typeof(ChartPlotBandLabelStyle),
			0.0d,
			BindingMode.Default,
			null);

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="ChartPlotBandLabelStyle"/>.
		/// </summary>
		public ChartPlotBandLabelStyle()
		{

		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets the horizontal alignment of the plot band label text.
		/// </summary>
		/// <value>This property takes the <see cref="ChartLabelAlignment"/> as its value. Its default value is Center.</value>
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
		///                  <chart:ChartPlotBandLabelStyle HorizontalTextAlignment="Start"/>
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
		public ChartLabelAlignment HorizontalTextAlignment
		{
			get { return (ChartLabelAlignment)GetValue(HorizontalTextAlignmentProperty); }
			set { SetValue(HorizontalTextAlignmentProperty, value); }
		}

		/// <summary>
		/// Gets or sets the vertical alignment of the plot band label text.
		/// </summary>
		/// <value>This property takes the <see cref="ChartLabelAlignment"/> as its value. Its default value is Center.</value>
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
		///                  <chart:ChartPlotBandLabelStyle VerticalTextAlignment="End"/>
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
		/// style.VerticalTextAlignment = ChartLabelAlignment.End;
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
		public ChartLabelAlignment VerticalTextAlignment
		{
			get { return (ChartLabelAlignment)GetValue(VerticalTextAlignmentProperty); }
			set { SetValue(VerticalTextAlignmentProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value that indicates the angle rotation of the plot band label text.
		/// </summary>
		/// <value>It accepts <see cref="double"/> values and the default value is 0.</value>
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
		///                  <chart:ChartPlotBandLabelStyle Angle="90"/>
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
		public double Angle
		{
			get { return (double)GetValue(AngleProperty); }
			set { SetValue(AngleProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value to provide horizontal padding to the plot band label text.
		/// </summary>
		/// <value>It accepts <see cref="double"/> values and the default value is 0.</value>
		/// <example>
		/// # [Xaml](#tab/tabid-1)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		/// 
		///     <chart:SfCartesianChart.XAxes>
		///         <chart:CategoryAxis>
		///            <chart:CategoryAxis.PlotBands>
		///             <chart:NumericalPlotBandCollection>
		///                <chart:NumericalPlotBand Start="1" Width="1">
		///                 <chart:NumericalPlotBand.LabelStyle>
		///                  <chart:ChartPlotBandLabelStyle OffsetX="5"/>
		///                 </chart:NumericalPlotBand.LabelStyle>
		///              </chart:NumericalPlotBandCollection>
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
		/// plotBand.Width = 1;
		/// ChartPlotBandLabelStyle style = new ChartPlotBandLabelStyle();
		/// style.OffsetX = 5;
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
		public double OffsetX
		{
			get { return (double)GetValue(OffsetXProperty); }
			set { SetValue(OffsetXProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value to provide vertical padding to the plot band label text.
		/// </summary>
		/// <value>It accepts <see cref="double"/> values and the default value is 0.</value>
		/// <example>
		/// # [Xaml](#tab/tabid-1)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		/// 
		///     <chart:SfCartesianChart.XAxes>
		///         <chart:CategoryAxis>
		///            <chart:CategoryAxis.PlotBands>
		///             <chart:NumericalPlotBandCollection>
		///                <chart:NumericalPlotBand Start="1" Size="1">
		///                 <chart:NumericalPlotBand.LabelStyle>
		///                  <chart:ChartPlotBandLabelStyle OffsetY="10"/>
		///                 </chart:NumericalPlotBand.LabelStyle>
		///              </chart:NumericalPlotBandCollection>
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
		/// style.OffsetY = 10;
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
		public double OffsetY
		{
			get { return (double)GetValue(OffsetYProperty); }
			set { SetValue(OffsetYProperty, value); }
		}

		#endregion
	}
}