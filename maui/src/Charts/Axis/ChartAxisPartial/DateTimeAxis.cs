namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// DateTimeAxis is used to plot <b> DateTime </b> values. The date-time axis uses date time scale and displays date-time values as axis labels in the specified format.
	/// </summary>
	/// <remarks>
	/// 
	/// <para>To render an axis, the datetime axis instance to be added in chart’s <see cref="SfCartesianChart.XAxes"/> collection as per the following code snippet.</para>
	/// 
	/// # [MainPage.xaml](#tab/tabid-1)
	/// <code><![CDATA[
	/// <chart:SfCartesianChart>
	///  
	///         <chart:SfCartesianChart.XAxes>
	///             <chart:DateTimeAxis/>
	///         </chart:SfCartesianChart.XAxes>
	///         
	/// </chart:SfCartesianChart>
	/// ]]>
	/// </code>
	/// # [MainPage.xaml.cs](#tab/tabid-2)
	/// <code><![CDATA[
	/// SfCartesianChart chart = new SfCartesianChart();
	/// 
	/// DateTimeAxis xAxis = new DateTimeAxis();
	/// chart.XAxes.Add(xAxis);
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
	/// <para> <b>Interval - </b> To define the axis labels interval using the <see cref="Interval"/>, and <see cref="IntervalType"/> properties.</para>
	/// </remarks>
	public partial class DateTimeAxis
	{
		#region Fields
		/// <summary>
		/// Gets the actual minimum value of axis range.
		/// </summary>
		DateTime _actualMinimum;

		/// <summary>
		/// Gets the actual maximum value of axis range.
		/// </summary>
		DateTime _actualMaximum;
		#endregion

		#region Bindable Properties

		/// <summary>
		/// Identifies the <see cref="IntervalType"/> bindable property.
		/// </summary>
		/// <remarks>
		/// Defines the type of interval for the DateTime axis.
		/// </remarks>
		public static readonly BindableProperty IntervalTypeProperty = BindableProperty.Create(
			nameof(IntervalType),
			typeof(DateTimeIntervalType),
			typeof(DateTimeAxis),
			DateTimeIntervalType.Auto,
			BindingMode.Default,
			null,
			OnIntervalTypePropertyChanged);

		/// <summary>
		/// Identifies the <see cref="Minimum"/> bindable property.
		/// </summary>
		/// <remarks>
		/// Defines the minimum value for the DateTime axis.
		/// </remarks>
		public static readonly BindableProperty MinimumProperty = BindableProperty.Create(
			nameof(Minimum),
			typeof(DateTime?),
			typeof(DateTimeAxis),
			null,
			BindingMode.Default,
			null,
			OnMinimumPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="Interval"/> bindable property.
		/// </summary>
		/// <remarks>
		/// Defines the interval value for the DateTime axis.
		/// </remarks>
		public static readonly BindableProperty IntervalProperty = BindableProperty.Create(
			nameof(Interval),
			typeof(double),
			typeof(DateTimeAxis),
			double.NaN,
			BindingMode.Default,
			null,
			OnIntervalPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="Maximum"/> bindable property.
		/// </summary>
		/// <remarks>
		/// Defines the maximum value for the DateTime axis.
		/// </remarks>
		public static readonly BindableProperty MaximumProperty = BindableProperty.Create(
			nameof(Maximum),
			typeof(DateTime?),
			typeof(DateTimeAxis),
			null,
			BindingMode.Default,
			null,
			OnMaximumPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="RangePadding"/> bindable property.
		/// </summary>
		/// <remarks>
		/// Defines the padding around the range of the DateTime axis.
		/// </remarks>
		public static readonly BindableProperty RangePaddingProperty = BindableProperty.Create(
			nameof(RangePadding),
			typeof(DateTimeRangePadding),
			typeof(DateTimeAxis),
			DateTimeRangePadding.Auto,
			BindingMode.Default,
			null,
			OnRangePaddingPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="AutoScrollingDeltaType"/> bindable property.
		/// </summary>
		/// <remarks>
		/// Defines the type of interval[Years, Months, Days, Hours, Minutes, Seconds and Milliseconds] for auto-scrolling on the DateTime axis.
		/// </remarks>
		public static readonly BindableProperty AutoScrollingDeltaTypeProperty = BindableProperty.Create(
			nameof(AutoScrollingDeltaType),
			typeof(DateTimeIntervalType),
			typeof(DateTimeAxis),
			DateTimeIntervalType.Auto,
			BindingMode.Default,
			null,
			OnAutoScrollingDeltaTypePropertyChanged);

		/// <summary>
		/// Identifies the <see cref="PlotBands"/> bindable property.
		/// </summary>
		/// <remarks>
		/// Defines the collection of plot bands for the DateTime axis.
		/// </remarks>
		public static readonly BindableProperty PlotBandsProperty = BindableProperty.Create(
			nameof(PlotBands),
			typeof(DateTimePlotBandCollection),
			typeof(DateTimeAxis),
			null,
			BindingMode.Default,
			null,
			OnPlotBandsValueChanged);

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets a value that can be used to change the interval between labels.
		/// </summary>
		/// <value>It accepts <c>double</c> values and the default value is double.NaN.</value>
		/// <remarks>
		/// <para>If this property is not set, the interval will be calculated automatically.</para>
		/// <para>By default, the interval will be calculated based on the minimum and maximum values of the provided data.</para>
		/// </remarks>
		/// <example>
		/// # [MainPage.xaml](#tab/tabid-3)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		///  
		///         <chart:SfCartesianChart.XAxes>
		///             <chart:DateTimeAxis Interval="6" IntervalType="Months" />
		///         </chart:SfCartesianChart.XAxes>
		/// 
		/// </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [MainPage.xaml.cs](#tab/tabid-4)
		/// <code><![CDATA[
		/// SfCartesianChart chart = new SfCartesianChart();
		/// 
		/// DateTimeAxis xAxis = new DateTimeAxis()
		/// {
		///    Interval = 6, 
		///    IntervalType = DateTimeIntervalType.Months
		/// };
		/// chart.XAxes.Add(xAxis);
		/// 
		/// ]]>
		/// </code>
		/// ***
		/// 
		/// </example>
		/// <see cref="IntervalType"/>
		public double Interval
		{
			get { return (double)GetValue(IntervalProperty); }
			set { SetValue(IntervalProperty, value); }
		}

		/// <summary>
		/// Gets or sets the minimum value of the time period to be displayed on the chart axis.
		/// </summary>
		/// <value>It accepts DateTime values and the default value is null.</value>
		/// <remarks>
		/// If we didn't set the minimum value, it will be calculated from the underlying collection.
		/// </remarks>
		/// <example>
		/// # [MainPage.xaml](#tab/tabid-5)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		///  
		///         <chart:SfCartesianChart.XAxes>
		///             <chart:DateTimeAxis Minimum="2021/05/10" Maximum="2021/11/01"/>
		///         </chart:SfCartesianChart.XAxes>
		///         
		/// </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [MainPage.xaml.cs](#tab/tabid-6)
		/// <code><![CDATA[
		/// SfCartesianChart chart = new SfCartesianChart();
		/// 
		/// DateTimeAxis xAxis = new DateTimeAxis()
		/// {
		///     Minimum = new DateTime(2021,05,10),
		///     Maximum = new DateTime(2021,11,01),
		/// };
		/// chart.XAxes.Add(xAxis);	
		/// 
		/// ]]>
		/// </code>
		/// ***
		/// 
		/// </example>
		public DateTime? Minimum
		{
			get { return (DateTime?)GetValue(MinimumProperty); }
			set { SetValue(MinimumProperty, value); }
		}

		/// <summary>
		/// Gets or sets the maximum value of the time period to be displayed on the chart axis.
		/// </summary>
		/// <value>It accepts DateTime values and the default value is null.</value>
		/// <remarks>
		/// If we didn't set the maximum value, it will be calculated from the underlying collection.
		/// </remarks>
		/// <example>
		/// # [MainPage.xaml](#tab/tabid-7)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		///  
		///         <chart:SfCartesianChart.XAxes>
		///             <chart:DateTimeAxis Minimum="2021/05/10" Maximum="2021/11/01"/>
		///         </chart:SfCartesianChart.XAxes>
		///         
		/// </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [MainPage.xaml.cs](#tab/tabid-8)
		/// <code><![CDATA[
		/// SfCartesianChart chart = new SfCartesianChart();
		/// 
		/// DateTimeAxis xAxis = new DateTimeAxis()
		/// {
		///     Minimum = new DateTime(2021,05,10),
		///     Maximum = new DateTime(2021,11,01),
		/// };
		/// chart.XAxes.Add(xAxis);	
		/// 
		/// ]]>
		/// </code>
		/// ***
		/// 
		/// </example>
		public DateTime? Maximum
		{
			get { return (DateTime?)GetValue(MaximumProperty); }
			set { SetValue(MaximumProperty, value); }
		}

		/// <summary>
		/// Gets or sets a padding type for the date time axis range.
		/// </summary>
		/// <value>It accepts the <see cref="DateTimeRangePadding"/> value and its default value is <see cref="DateTimeRangePadding.Auto"/>.</value>
		/// <example>
		/// # [MainPage.xaml](#tab/tabid-9)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		///  
		///         <chart:SfCartesianChart.XAxes>
		///             <chart:DateTimeAxis RangePadding ="Round" />
		///         </chart:SfCartesianChart.XAxes>
		///         
		/// </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [MainPage.xaml.cs](#tab/tabid-10)
		/// <code><![CDATA[
		/// SfCartesianChart chart = new SfCartesianChart();
		/// 
		/// DateTimeAxis xAxis = new DateTimeAxis()
		/// {
		///     RangePadding = DateTimeRangePadding.Round,
		/// };
		/// chart.XAxes.Add(xAxis);
		/// 
		/// ]]>
		/// </code>
		/// ***
		/// 
		/// </example>
		public DateTimeRangePadding RangePadding
		{
			get { return (DateTimeRangePadding)GetValue(RangePaddingProperty); }
			set { SetValue(RangePaddingProperty, value); }
		}

		/// <summary>
		/// Gets or sets the date time unit of the value specified in the <see cref="Interval"/> property.
		/// </summary>
		/// <value>It accepts the <see cref="DateTimeIntervalType"/> value and its default value is <see cref="DateTimeIntervalType.Auto"/>.</value>
		/// <example>
		/// # [MainPage.xaml](#tab/tabid-11)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		/// 
		///         <chart:SfCartesianChart.XAxes>
		///             <chart:DateTimeAxis IntervalType="Months"/>
		///         </chart:SfCartesianChart.XAxes>
		/// 
		/// </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [MainPage.xaml.cs](#tab/tabid-12)
		/// <code><![CDATA[
		/// SfCartesianChart chart = new SfCartesianChart();
		/// 
		/// DateTimeAxis xAxis = new DateTimeAxis()
		/// {
		///     IntervalType = DateTimeIntervalType.Months
		/// };
		/// chart.XAxes.Add(xAxis);
		/// 
		/// ]]>
		/// </code>
		/// ***
		/// 
		/// </example>
		public DateTimeIntervalType IntervalType
		{
			get { return (DateTimeIntervalType)GetValue(IntervalTypeProperty); }
			set { SetValue(IntervalTypeProperty, value); }
		}

		/// <summary>
		/// Gets the actual minimum value of the DateTimeAxis.
		/// </summary>
		/// <example>
		/// # [MainWindow.xaml](#tab/tabid-13)
		/// <code><![CDATA[
		///   <VerticalStackLayout>
		///     <Label Text="{Binding Source={x:Reference xAxis}, Path=ActualMinimum }" />
		///     <chart:SfCartesianChart>
		///
		///           <chart:SfCartesianChart.XAxes>
		///               <chart:DateTimeAxis x:Name="xAxis"/>
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
		///     DateTimeAxis xAxis = new DateTimeAxis();
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
		public DateTime ActualMinimum
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
		/// Gets the actual maximum value of the DateTimeAxis.
		/// </summary>
		/// <example>
		/// # [MainWindow.xaml](#tab/tabid-15)
		/// <code><![CDATA[
		///   <VerticalStackLayout>
		///     <Label Text="{Binding Source={x:Reference xAxis}, Path=ActualMaximum }" />
		///     <chart:SfCartesianChart>
		///
		///           <chart:SfCartesianChart.XAxes>
		///               <chart:DateTimeAxis x:Name="xAxis"/>
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
		/// # [MainWindow.cs](#tab/tabid-16)
		/// <code><![CDATA[
		///     SfCartesianChart chart = new SfCartesianChart();
		///     
		///     DateTimeAxis xAxis = new DateTimeAxis();
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
		public DateTime ActualMaximum
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
		/// Gets or sets the date time unit of the <see cref="ChartAxis.AutoScrollingDelta"/> value. The value can be set to years, months, days, hours, minutes, seconds and auto. 
		/// </summary>
		/// <value>It accepts <see cref="DateTimeIntervalType"/> values, and its default value is <see cref="DateTimeIntervalType.Auto"/>, which means that the date time unit of the delta value will be automatically calculated based on the data points.</value>
		/// <remarks>
		/// <para> <b>Note:</b> This is only applicable for <see cref="SfCartesianChart"/>.</para>
		/// </remarks>
		/// <example>
		/// # [MainPage.xaml](#tab/tabid-7)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		///    <chart:SfCartesianChart.XAxes>
		///        <chart:DateTimeAxis AutoScrollingDeltaType="Years"/>
		///    </chart:SfCartesianChart.XAxes>
		/// </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [MainPage.xaml.cs](#tab/tabid-8)
		/// <code><![CDATA[
		/// SfCartesianChart chart = new SfCartesianChart();
		/// 
		/// DateTimeAxis xAxis = new DateTimeAxis();
		/// xAxis.AutoScrollingDeltaType = DateTimeIntervalType.Years;
		/// 
		/// chart.XAxes.Add(xAxis);	
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public DateTimeIntervalType AutoScrollingDeltaType
		{
			get { return (DateTimeIntervalType)GetValue(AutoScrollingDeltaTypeProperty); }
			set { SetValue(AutoScrollingDeltaTypeProperty, value); }
		}

		/// <summary>
		///  Gets or sets the collection of plot bands to be added to the chart axis.
		/// </summary>
		/// <example>
		/// # [MainPage.xaml](#tab/tabid-7)
		/// <code><![CDATA[
		///  <chart:SfCartesianChart>
		/// 
		///     <chart:SfCartesianChart.XAxes>
		///         <chart:DateTimeAxis>
		///            <chart:DateTimeAxis.PlotBands>
		///                <chart:DateTimePlotBand Start="2000-01-01" Width="2" WidthType="Month"/>
		///            </chart:DateTimeAxis.PlotBands>
		///        </chart:DateTimeAxis>
		///     </chart:SfCartesianChart.XAxes>
		/// 
		/// </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [MainPage.xaml.cs](#tab/tabid-2)
		/// <code><![CDATA[
		/// SfCartesianChart chart = new SfCartesianChart();
		/// 
		/// DateTimeAxis xAxis = new DateTimeAxis();
		/// DateTimePlotBand plotBand = new DateTimePlotBand();
		/// plotBand.Start = new DateTime(2000,01,01);
		/// plotBand.Width = 2;
		/// plotBand.WidthType = DateTimeIntervalType.Months;
		/// xAxis.PlotBands.Add(plotBand);
		/// 
		/// chart.XAxes.Add(xAxis);
		/// /// ]]>
		/// </code>
		/// ***
		/// </example>
		public DateTimePlotBandCollection PlotBands
		{
			get { return (DateTimePlotBandCollection)GetValue(PlotBandsProperty); }
			set { SetValue(PlotBandsProperty, value); }
		}

		#endregion

		#region Callback Methods

		static void OnIntervalPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is DateTimeAxis axis)
			{
				axis.UpdateAxisInterval((double)newValue);
				axis.UpdateLayout();
			}
		}

		static void OnRangePaddingPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is DateTimeAxis axis)
			{
				axis.UpdateLayout();
			}
		}

		static void OnIntervalTypePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is DateTimeAxis axis)
			{
				axis.UpdateActualIntervalType((DateTimeIntervalType)newValue);
				axis.UpdateLayout();
			}
		}

		static void OnAutoScrollingDeltaTypePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is DateTimeAxis axis)
			{
				axis.CanAutoScroll = true;
				axis.UpdateLayout();
			}
		}

		static void OnMaximumPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is DateTimeAxis axis)
			{
				axis.UpdateDefaultMaximum((DateTime?)newValue);
				axis.OnMinMaxChanged();
				axis.UpdateLayout();
			}
		}

		static void OnMinimumPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is DateTimeAxis axis)
			{
				axis.UpdateDefaultMinimum((DateTime?)newValue);
				axis.OnMinMaxChanged();
				axis.UpdateLayout();
			}
		}

		static void OnPlotBandsValueChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is DateTimeAxis axis)
			{
				axis.OnPlotBandPropertyChanged(oldValue as DateTimePlotBandCollection, newValue as DateTimePlotBandCollection);
			}
		}

		void OnPlotBandPropertyChanged(DateTimePlotBandCollection? oldValue, DateTimePlotBandCollection? newValue)
		{
			if (oldValue != null)
			{
				oldValue.CollectionChanged -= PlotBandsCollection_Changed;
				ActualPlotBands.Clear();

				foreach (var band in oldValue)
				{
					band.Parent = null;
					band.Axis = null;
				}
			}

			if (newValue != null)
			{
				newValue.CollectionChanged += PlotBandsCollection_Changed;
				ActualPlotBands.Clear();

				foreach (var plotBand in newValue)
				{
					plotBand.Parent = Parent;
					plotBand.Axis = this;
					ActualPlotBands.Add(plotBand);
				}
			}

			InvalidatePlotBands();
		}

		#endregion
	}
}
