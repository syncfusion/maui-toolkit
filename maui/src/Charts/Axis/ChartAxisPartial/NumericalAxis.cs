namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// The numerical axis uses a numerical scale and it displays numbers as labels. It supports both horizontal and vertical axes.
	/// </summary>
	/// <remarks>
	///  
	/// <para>To render an axis, the numerical axis instance to be added in chart’s <see cref="SfCartesianChart.XAxes"/> and <see cref="SfCartesianChart.YAxes"/> collection as per the following code snippet.</para>
	/// 
	/// # [MainPage.xaml](#tab/tabid-1)
	/// <code><![CDATA[
	/// <chart:SfCartesianChart>
	///  
	///         <chart:SfCartesianChart.XAxes>
	///             <chart:NumericalAxis/>
	///         </chart:SfCartesianChart.XAxes>
	///         
	///         <chart:SfCartesianChart.YAxes>
	///             <chart:NumericalAxis/>
	///         </chart:SfCartesianChart.YAxes>
	/// 
	/// </chart:SfCartesianChart>
	/// ]]>
	/// </code>
	/// # [MainPage.xaml.cs](#tab/tabid-2)
	/// <code><![CDATA[
	/// SfCartesianChart chart = new SfCartesianChart();
	/// 
	/// NumericalAxis xAxis = new NumericalAxis();
	/// chart.XAxes.Add(xAxis);
	/// 
	/// NumericalAxis yAxis = new NumericalAxis();
	/// chart.YAxes.Add(yAxis);
	/// 
	/// ]]>
	/// </code>
	/// ***
	/// 
	/// <para> <b>Title - </b> To render the title, refer to this <see cref="ChartAxis.Title"/> property.</para>
	/// <para> <b>Grid Lines - </b> To show and customize the grid lines refer these <see cref="ChartAxis.ShowMajorGridLines"/>, and <see cref="ChartAxis.MajorGridLineStyle"/>, <see cref="RangeAxisBase.ShowMinorGridLines"/>, <see cref="RangeAxisBase.MinorTicksPerInterval"/>, <see cref="RangeAxisBase.MinorGridLineStyle"/> properties.</para>
	/// <para> <b>Tick Lines - </b> To show and customize the tick lines refer these <see cref="ChartAxis.MajorTickStyle"/>, <see cref="RangeAxisBase.MinorTickStyle"/>, <see cref="RangeAxisBase.MinorTicksPerInterval"/> properties.</para>
	/// <para> <b>Axis Line - </b> To customize the axis line using the <see cref="ChartAxis.AxisLineStyle"/> property.</para>
	/// <para> <b>Range Customization - </b> To customize the axis range with help of the <see cref="Minimum"/>, and <see cref="Maximum"/> properties.</para>
	/// <para> <b>Labels Customization - </b> To customize the axis labels, refer to this <see cref="ChartAxis.LabelStyle"/> property.</para>
	/// <para> <b>Inversed Axis - </b> Inverse the axis using the <see cref="ChartAxis.IsInversed"/> property.</para>
	/// <para> <b>Axis Crossing - </b> For axis crossing, refer these <see cref="ChartAxis.CrossesAt"/>, <see cref="ChartAxis.CrossAxisName"/>, and <see cref="ChartAxis.RenderNextToCrossingValue"/> properties.</para>
	/// <para> <b>Interval - </b> To define the axis labels interval using the <see cref="Interval"/> property.</para>
	/// </remarks>
	public partial class NumericalAxis
	{
		#region Fields
		/// <summary>
		/// Gets the actual minimum value of axis range.
		/// </summary>
		double _actualMinimum;

		/// <summary>
		/// Gets the actual maximum value of axis range.
		/// </summary>
		double _actualMaximum;
		#endregion

		#region Bindable Properties

		/// <summary>
		/// Identifies the <see cref="RangePadding"/> bindable property.
		/// </summary>
		/// <remarks>
		/// Defines the padding for the numerical axis range.
		/// </remarks>
		public static readonly BindableProperty RangePaddingProperty = BindableProperty.Create(
			nameof(RangePadding),
			typeof(NumericalPadding),
			typeof(NumericalAxis),
			NumericalPadding.Auto,
			BindingMode.Default,
			null,
			OnRangePaddingPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="Interval"/> bindable property.
		/// </summary>
		/// <remarks>
		/// Defines the interval value for the numerical axis.
		/// </remarks>
		public static readonly BindableProperty IntervalProperty = BindableProperty.Create(
			nameof(Interval),
			typeof(double),
			typeof(NumericalAxis),
			double.NaN,
			BindingMode.Default,
			null,
			OnIntervalPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="Minimum"/> bindable property.
		/// </summary>
		/// <remarks>
		/// Defines the minimum value for the numerical axis.
		/// </remarks>
		public static readonly BindableProperty MinimumProperty = BindableProperty.Create(
			nameof(Minimum),
			typeof(double?),
			typeof(NumericalAxis),
			null,
			BindingMode.Default,
			null,
			OnMinimumPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="Maximum"/> bindable property.
		/// </summary>
		/// <remarks>
		/// Defines the maximum value for the numerical axis.
		/// </remarks>
		public static readonly BindableProperty MaximumProperty = BindableProperty.Create(
			nameof(Maximum),
			typeof(double?),
			typeof(NumericalAxis),
			null,
			BindingMode.Default,
			null,
			OnMaximumPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="PlotBands"/> bindable property.
		/// </summary>
		/// <remarks>
		/// Defines the collection of plot bands for the numerical axis.
		/// </remarks>
		public static readonly BindableProperty PlotBandsProperty = BindableProperty.Create(
			nameof(PlotBands),
			typeof(NumericalPlotBandCollection),
			typeof(NumericalAxis),
			null,
			BindingMode.Default,
			null,
			OnPlotBandsPropertyChanged);

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets a value that determines the interval in axis's range.
		/// </summary>
		/// <value>The default value is double.NaN.</value>
		/// <remarks> 
		/// If this property is not set, the interval will be calculated automatically.
		/// </remarks>
		/// <example>
		/// # [MainPage.xaml](#tab/tabid-3)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		///  
		///         <chart:SfCartesianChart.XAxes>
		///             <chart:NumericalAxis Interval="10" />
		///         </chart:SfCartesianChart.XAxes>
		/// 
		/// </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [MainPage.xaml.cs](#tab/tabid-4)
		/// <code><![CDATA[
		/// SfCartesianChart chart = new SfCartesianChart();
		/// 
		/// NumericalAxis xAxis = new NumericalAxis();
		/// xAxis.Interval = 10;
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
		/// Gets or sets the minimum value for the axis range.
		/// </summary>
		/// <value>It accepts double values and the default value is null.</value>
		/// <remarks>
		/// If we didn't set the minimum value, it will be calculated from the underlying collection.
		/// </remarks>
		/// <example>
		/// # [MainPage.xaml](#tab/tabid-5)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		///  
		///         <chart:SfCartesianChart.XAxes>
		///             <chart:NumericalAxis Maximum="100" Minimum="0"/>
		///         </chart:SfCartesianChart.XAxes>
		///         
		/// </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [MainPage.xaml.cs](#tab/tabid-6)
		/// <code><![CDATA[
		/// SfCartesianChart chart = new SfCartesianChart();
		/// 
		/// NumericalAxis xAxis = new NumericalAxis()
		/// {
		///     Maximum = 100,
		///     Minimum = 0,
		/// };
		/// chart.XAxes.Add(xAxis);	
		/// 
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public double? Minimum
		{
			get { return (double?)GetValue(MinimumProperty); }
			set { SetValue(MinimumProperty, value); }
		}

		/// <summary>
		/// Gets or sets the maximum value for the axis range.
		/// </summary>
		/// <value>It accepts double values and the default value is null.</value>
		/// <remarks>
		/// If we didn't set the maximum value, it will be calculated from the underlying collection.
		/// </remarks>
		/// <example>
		/// # [MainPage.xaml](#tab/tabid-7)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		///  
		///         <chart:SfCartesianChart.XAxes>
		///             <chart:NumericalAxis Maximum="100" Minimum="0"/>
		///         </chart:SfCartesianChart.XAxes>
		///         
		/// </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [MainPage.xaml.cs](#tab/tabid-8)
		/// <code><![CDATA[
		/// SfCartesianChart chart = new SfCartesianChart();
		/// 
		/// NumericalAxis xAxis = new NumericalAxis()
		/// {
		///     Maximum = 100,
		///     Minimum = 0,
		/// };
		/// chart.XAxes.Add(xAxis);	
		/// 
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public double? Maximum
		{
			get { return (double?)GetValue(MaximumProperty); }
			set { SetValue(MaximumProperty, value); }
		}

		/// <summary>
		/// Gets or sets a padding type for the numerical axis range. 
		/// </summary>
		/// <value>It accepts the <see cref="NumericalPadding"/> value and its default value is <see cref="NumericalPadding.Auto"/>.</value>
		/// <example>
		/// # [MainPage.xaml](#tab/tabid-9)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		/// 
		///         <chart:SfCartesianChart.XAxes>
		///             <chart:NumericalAxis RangePadding ="Round"/>
		///         </chart:SfCartesianChart.XAxes>
		/// 
		/// </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [MainPage.xaml.cs](#tab/tabid-10)
		/// <code><![CDATA[
		/// SfCartesianChart chart = new SfCartesianChart();
		/// 
		/// NumericalAxis xAxis = new NumericalAxis()
		/// {
		///     RangePadding = NumericalPadding.Round,
		/// };
		/// chart.XAxes.Add(xAxis);
		/// 
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public NumericalPadding RangePadding
		{
			get { return (NumericalPadding)GetValue(RangePaddingProperty); }
			set { SetValue(RangePaddingProperty, value); }
		}

		/// <summary>
		/// Gets the actual minimum value of the numerical axis.
		/// </summary>
		/// <example>
		/// # [MainWindow.xaml](#tab/tabid-11)
		/// <code><![CDATA[
		///   <VerticalStackLayout>
		///     <Label Text="{Binding Source={x:Reference xAxis}, Path=ActualMinimum }" />
		///     <chart:SfCartesianChart>
		///
		///           <chart:SfCartesianChart.XAxes>
		///               <chart:NumericalAxis x:Name="xAxis"/>
		///           </chart:SfCartesianChart.XAxes>
		///
		///           <chart:SfCartesianChart.YAxes>
		///               <chart:NumericalAxis x:Name="yAxis"/>
		///           </chart:SfCartesianChart.YAxes>
		///
		///           <chart:SfCartesianChart.Series>
		///               <chart:ColumnSeries ItemsSource="{Binding Data}"
		///                                   XBindingPath="XValue"
		///                                   YBindingPath="YValue" />
		///           </chart:SfCartesianChart.Series>
		///           
		///     </chart:SfCartesianChart>
		///   </VerticalStackLayout>
		/// ]]></code>
		/// # [MainWindow.cs](#tab/tabid-12)
		/// <code><![CDATA[
		///     SfCartesianChart chart = new SfCartesianChart();
		///     
		///     NumericalAxis xAxis = new NumericalAxis();
		///     NumericalAxis yAxis = new NumericalAxis();
		///     
		///     chart.XAxes.Add(xAxis);
		///     chart.YAxes.Add(yAxis);
		///     
		///     ColumnSeries series = new ColumnSeries();
		///     series.ItemsSource = viewModel.Data;
		///     series.XBindingPath = "XValue";
		///     series.YBindingPath = "YValue";
		///     chart.Series.Add(series);
		///     
		///     Label label = new Label();
		///     label.SetBinding(Label.TextProperty, new Binding("ActualMinimum", source: xAxis));
		///     
		/// ]]></code>
		/// ***
		/// </example>
		public double ActualMinimum
		{
			get { return _actualMinimum; }
			internal set
			{
				if (_actualMinimum != value)
				{
					_actualMinimum = value;
					OnPropertyChanged(nameof(ActualMinimum));
				}
			}
		}

		/// <summary>
		/// Gets the actual maximum value of the numerical axis.
		/// </summary>
		/// <example>
		/// # [MainWindow.xaml](#tab/tabid-13)
		/// <code><![CDATA[
		///   <VerticalStackLayout>
		///     <Label Text="{Binding Source={x:Reference xAxis}, Path=ActualMaximum }" />
		///     <chart:SfCartesianChart>
		///
		///           <chart:SfCartesianChart.XAxes>
		///               <chart:NumericalAxis x:Name="xAxis"/>
		///           </chart:SfCartesianChart.XAxes>
		///
		///           <chart:SfCartesianChart.YAxes>
		///               <chart:NumericalAxis x:Name="yAxis"/>
		///           </chart:SfCartesianChart.YAxes>
		///
		///           <chart:SfCartesianChart.Series>
		///               <chart:ColumnSeries ItemsSource="{Binding Data}"
		///                                   XBindingPath="XValue"
		///                                   YBindingPath="YValue" />
		///           </chart:SfCartesianChart.Series>
		///           
		///     </chart:SfCartesianChart>
		///   </VerticalStackLayout>
		/// ]]></code>
		/// # [MainWindow.cs](#tab/tabid-14)
		/// <code><![CDATA[
		///     SfCartesianChart chart = new SfCartesianChart();
		///     
		///     NumericalAxis xAxis = new NumericalAxis();
		///     NumericalAxis yAxis = new NumericalAxis();
		///     
		///     chart.XAxes.Add(xAxis);
		///     chart.YAxes.Add(yAxis);
		///     
		///     ColumnSeries series = new ColumnSeries();
		///     series.ItemsSource = viewModel.Data;
		///     series.XBindingPath = "XValue";
		///     series.YBindingPath = "YValue";
		///     chart.Series.Add(series);
		///     
		///     Label label = new Label();
		///     label.SetBinding(Label.TextProperty, new Binding("ActualMaximum", source: xAxis));
		///     
		/// ]]></code>
		/// ***
		/// </example>
		public double ActualMaximum
		{
			get { return _actualMaximum; }
			internal set
			{
				if (_actualMaximum != value)
				{
					_actualMaximum = value;
					OnPropertyChanged(nameof(ActualMaximum));
				}
			}
		}

		/// <summary>
		/// Gets or sets the collection of plot bands to be added to the chart axis.
		/// </summary>
		/// # [MainPage.xaml](#tab/tabid-1)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		/// 
		///     <chart:SfCartesianChart.XAxes>
		///         <chart:NumericalAxis>
		///            <chart:NumericalAxis.PlotBands>
		///                <chart:NumericalPlotBand Start="1" Width="2"/>
		///            </chart:NumericalAxis.PlotBands>
		///        </chart:NumericalAxis>
		///     </chart:SfCartesianChart.XAxes>
		/// 
		/// </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [MainPage.xaml.cs](#tab/tabid-2)
		/// <code><![CDATA[
		/// SfCartesianChart chart = new SfCartesianChart();
		/// 
		/// NumericalAxis xAxis = new NumericalAxis();
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

		#region Callback Methods
		static void OnIntervalPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is NumericalAxis axis)
			{
				axis.UpdateAxisInterval((double)newValue);
				axis.UpdateLayout();
			}
		}

		static void OnMinimumPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is NumericalAxis axis)
			{
				axis.UpdateDefaultMinimum((double?)newValue);
				axis.OnMinMaxChanged();
				axis.UpdateLayout();
			}
		}

		static void OnMaximumPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is NumericalAxis axis)
			{
				axis.UpdateDefaultMaximum((double?)newValue);
				axis.OnMinMaxChanged();
				axis.UpdateLayout();
			}
		}

		static void OnRangePaddingPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			(bindable as ChartAxis)?.UpdateLayout();
		}

		#endregion
	}
}
