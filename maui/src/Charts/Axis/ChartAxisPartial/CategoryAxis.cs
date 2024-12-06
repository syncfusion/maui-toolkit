namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// The CategoryAxis is an indexed based axis that plots values based on the index of the data point collection. It displays string values in axis labels.
	/// </summary>
	/// <remarks>
	/// 
	/// <para>Category axis supports only for the X(horizontal) axis. </para>
	/// 
	/// <para>To render an axis, add the category axis instance to the chart’s <see cref="SfCartesianChart.XAxes"/> collection as shown in the following code sample.</para>
	/// 
	/// # [MainPage.xaml](#tab/tabid-1)
	/// <code><![CDATA[
	/// <chart:SfCartesianChart>
	///  
	///         <chart:SfCartesianChart.XAxes>
	///             <chart:CategoryAxis/>
	///         </chart:SfCartesianChart.XAxes>
	/// 
	/// </chart:SfCartesianChart>
	/// ]]>
	/// </code>
	/// # [MainPage.xaml.cs](#tab/tabid-2)
	/// <code><![CDATA[
	/// SfCartesianChart chart = new SfCartesianChart();
	/// 
	/// CategoryAxis xAxis = new CategoryAxis();
	/// chart.XAxes.Add(xAxis);	
	/// 
	/// ]]>
	/// </code>
	/// ***
	/// 
	/// <para>The CategoryAxis supports the following features. Refer to the corresponding APIs, for more details and example codes.</para>
	/// 
	/// <para> <b>Title - </b> To render the title, refer to this <see cref="ChartAxis.Title"/> property.</para>
	/// <para> <b>Grid Lines - </b> To show and customize the grid lines, refer these <see cref="ChartAxis.ShowMajorGridLines"/>, and <see cref="ChartAxis.MajorGridLineStyle"/> properties.</para>
	/// <para> <b>Axis Line - </b> To customize the axis line using the <see cref="ChartAxis.AxisLineStyle"/> property.</para>
	/// <para> <b>Labels Customization - </b> To customize the axis labels, refer to this <see cref="ChartAxis.LabelStyle"/> property.</para>
	/// <para> <b>Inversed Axis - </b> Inverse the axis using the <see cref="ChartAxis.IsInversed"/> property.</para>
	/// <para> <b>Axis Crossing - </b> For axis crossing, refer these <see cref="ChartAxis.CrossesAt"/>, <see cref="ChartAxis.CrossAxisName"/>, and <see cref="ChartAxis.RenderNextToCrossingValue"/> properties.</para>
	/// <para> <b>Label Placement - </b> To place the axis labels in between or on the tick lines, refer to this <see cref="LabelPlacement"/> property.</para>
	/// <para> <b>Interval - </b> To define the interval between the axis labels, refer to this <see cref="Interval"/> property.</para>
	/// </remarks>
	public partial class CategoryAxis
	{
		#region Bindable Properties

		/// <summary>
		/// Identifies the <see cref="Interval"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="Interval"/> property represents the interval value for the axis.
		/// </remarks> 
		public static readonly BindableProperty IntervalProperty = BindableProperty.Create(
			nameof(Interval),
			typeof(double),
			typeof(CategoryAxis),
			double.NaN,
			BindingMode.Default,
			null,
			OnIntervalPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="LabelPlacement"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="LabelPlacement"/> property determines whether axis labels are placed on the ticks or between them.
		/// </remarks>
		public static readonly BindableProperty LabelPlacementProperty = BindableProperty.Create(
			nameof(LabelPlacement),
			typeof(LabelPlacement),
			typeof(CategoryAxis),
			LabelPlacement.OnTicks,
			BindingMode.Default,
			null,
			OnLabelPlacementPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="ArrangeByIndex"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="ArrangeByIndex"/> property specifies whether axis labels are arranged based on their index or by value.
		/// </remarks>
		public static readonly BindableProperty ArrangeByIndexProperty = BindableProperty.Create(
			nameof(ArrangeByIndex),
			typeof(bool),
			typeof(CategoryAxis),
			true,
			BindingMode.Default,
			null,
			OnArrangeByIndexPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="PlotBands"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The <see cref="PlotBands"/> property represents a collection of plot bands used to highlight ranges of values on the chart.
		/// </remarks>
		public static readonly BindableProperty PlotBandsProperty = BindableProperty.Create(
		   nameof(PlotBands),
		   typeof(NumericalPlotBandCollection),
		   typeof(CategoryAxis),
		   null,
		   BindingMode.Default,
		   null,
		   OnPlotBandsPropertyChanged);

		#endregion

		#region Public Properties
		/// <summary>
		/// Gets or sets a value that determines whether to place the axis label in between or on the tick lines.
		/// </summary>
		/// <value>It accepts the <see cref="Charts.LabelPlacement"/> values and the default value is <c>OnTicks</c>. </value>
		/// <remarks>
		/// <para> <b>BetweenTicks - </b> Used to place the axis label between the ticks.</para>
		/// <para> <b>OnTicks - </b> Used to place the axis label with the tick as the center.</para>
		/// <para> <b>Note:</b> This is only applicable for <see cref="SfCartesianChart"/>.</para>
		/// </remarks>
		/// <example>
		/// # [MainPage.xaml](#tab/tabid-3)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		///  
		///     <chart:SfCartesianChart.XAxes>
		///         <chart:CategoryAxis LabelPlacement="BetweenTicks" />
		///     </chart:SfCartesianChart.XAxes>
		/// 
		/// </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [MainPage.xaml.cs](#tab/tabid-4)
		/// <code><![CDATA[
		/// SfCartesianChart chart = new SfCartesianChart();
		/// 
		/// CategoryAxis xAxis = new CategoryAxis();
		/// xAxis.LabelPlacement = LabelPlacement.BetweenTicks;
		/// chart.XAxes.Add(xAxis);
		/// 
		/// ]]>
		/// </code>
		/// ***
		/// </example> 
		public LabelPlacement LabelPlacement
		{
			get { return (LabelPlacement)GetValue(LabelPlacementProperty); }
			set { SetValue(LabelPlacementProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value that can be used to customize the interval between the axis labels.
		/// </summary>
		/// <value>It accepts double values and the default value is double.NaN.</value>
		/// <remarks>If this property is not set, the interval will be calculated automatically.</remarks>
		/// <example>
		/// # [MainPage.xaml](#tab/tabid-5)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		///  
		///         <chart:SfCartesianChart.XAxes>
		///             <chart:CategoryAxis Interval="2" />
		///         </chart:SfCartesianChart.XAxes>
		/// 
		/// </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [MainPage.xaml.cs](#tab/tabid-6)
		/// <code><![CDATA[
		/// SfCartesianChart chart = new SfCartesianChart();
		/// 
		/// CategoryAxis xAxis = new CategoryAxis(){ Interval = 2, };
		/// chart.XAxes.Add(xAxis);
		/// 
		/// ]]>
		/// </code>
		/// *** 
		/// </example>
		public double Interval
		{
			get { return (double)GetValue(IntervalProperty); }
			set { SetValue(IntervalProperty, value); }
		}

		/// <summary>
		///  Gets or sets a value that determines whether to arrange the axis labels by index or by value.
		/// </summary>
		/// <value>This property takes the <c>bool</c> as its value and its default is <c>True</c>. </value>
		/// <remarks>
		/// <para> <b>True - </b> Used to arrange the axis labels by index.</para>
		/// <para> <b>False - </b> Used to arrange the axis labels by value.</para>
		/// </remarks>
		/// <example>
		/// # [MainPage.xaml](#tab/tabid-5)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		///  
		///         <chart:SfCartesianChart.XAxes>
		///             <chart:CategoryAxis ArrangeByIndex="False" />
		///         </chart:SfCartesianChart.XAxes>
		/// 
		/// </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [MainPage.xaml.cs](#tab/tabid-6)
		/// <code><![CDATA[
		/// SfCartesianChart chart = new SfCartesianChart();
		/// 
		/// CategoryAxis xAxis = new CategoryAxis(){ ArrangeByIndex = false, };
		/// chart.XAxes.Add(xAxis);
		/// 
		/// ]]>
		/// </code>
		/// *** 
		/// </example>
		public bool ArrangeByIndex
		{
			get { return (bool)GetValue(ArrangeByIndexProperty); }
			set { SetValue(ArrangeByIndexProperty, value); }
		}

		/// <summary>
		///  Gets or sets the collection of plot bands to be added to the chart axis.
		/// </summary>
		/// # [MainPage.xaml](#tab/tabid-1)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		/// 
		///     <chart:SfCartesianChart.XAxes>
		///         <chart:CategoryAxis>
		///            <chart:CategoryAxis.PlotBands>
		///                <chart:NumericalPlotBand Start="1" Width="2"/>
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
		/// NumericalPlotBand plotBand = new NumericalPlotBand();
		/// plotBand.Start = 1;
		/// plotBand.Width = 2;
		/// xAxis.PlotBands.Add(plotBand);
		/// 
		/// chart.XAxes.Add(xAxis);
		///
		/// ]]>
		/// </code>
		public NumericalPlotBandCollection PlotBands
		{
			get { return (NumericalPlotBandCollection)GetValue(PlotBandsProperty); }
			set { SetValue(PlotBandsProperty, value); }
		}
		#endregion

		#region Private Methods
		static void OnIntervalPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is CategoryAxis axis)
			{
				axis.UpdateAxisInterval((double)newValue);
				axis.UpdateLayout();
			}
		}

		static void OnLabelPlacementPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is CategoryAxis axis)
			{
				axis.UpdateLayout();
			}
		}

		static void OnArrangeByIndexPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is CategoryAxis categoryAxis)
			{
				foreach (var series in categoryAxis.RegisteredSeries)
				{
					series.SegmentsCreated = false;
					if (series.Chart != null)
					{
						series.Chart.IsRequiredDataLabelsMeasure = true;
					}
				}

				categoryAxis.UpdateLayout();
			}
		}

		#endregion
	}
}