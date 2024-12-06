using Microsoft.Maui.Controls;
using System;

namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// Represents a specialized axis to plot data with number values on a logarithmic scale.
	/// </summary>
	/// <remarks>
	/// <para> This class contains properties to customize range, interval, grid lines, ticks, and labels.</para>
	/// <para> <b>Title - </b> To render the title, refer to this <see cref="ChartAxis.Title"/> property.</para>
	/// <para> <b>Grid Lines - </b> To show and customize the grid lines refer these <see cref="ChartAxis.ShowMajorGridLines"/>, and <see cref="ChartAxis.MajorGridLineStyle"/>, <see cref="RangeAxisBase.ShowMinorGridLines"/>, <see cref="RangeAxisBase.MinorTicksPerInterval"/>, <see cref="RangeAxisBase.MinorGridLineStyle"/> properties.</para>
	/// <para> <b>Tick Lines - </b> To show and customize the tick lines refer these <see cref="ChartAxis.MajorTickStyle"/>, <see cref="RangeAxisBase.MinorTickStyle"/>, <see cref="RangeAxisBase.MinorTicksPerInterval"/> properties.</para>
	/// <para> <b>Axis Line - </b> To customize the axis line using the <see cref="ChartAxis.AxisLineStyle"/> property.</para>
	/// <para> <b>Range Customization - </b> To customize the axis range with help of the <see cref="Minimum"/>, and <see cref="Maximum"/> properties.</para>
	/// <para> <b>Labels Customization - </b> To customize the axis labels, refer to this <see cref="ChartAxis.LabelStyle"/> property.</para>
	/// <para> <b>Inversed Axis - </b> Inverse the axis using the <see cref="ChartAxis.IsInversed"/> property.</para>
	/// <para> <b>Axis Crossing - </b> For axis crossing, refer these <see cref="ChartAxis.CrossesAt"/>, <see cref="ChartAxis.CrossAxisName"/>, and <see cref="ChartAxis.RenderNextToCrossingValue"/> properties.</para>
	/// <para> <b>Interval - </b> Define the axis labels interval using the <see cref="Interval"/> property.</para>
	/// </remarks>
	/// <example>
	/// # [MainPage.xaml](#tab/tabid-1)
	/// <code><![CDATA[
	/// <chart:SfCartesianChart>
	///  
	///         <chart:SfCartesianChart.XAxes>
	///             <chart:LogarithmicAxis/>
	///         </chart:SfCartesianChart.XAxes>
	///         
	///         <chart:SfCartesianChart.YAxes>
	///             <chart:LogarithmicAxis/>
	///         </chart:SfCartesianChart.YAxes>
	/// 
	/// </chart:SfCartesianChart>
	/// ]]>
	/// </code>
	/// # [MainPage.xaml.cs](#tab/tabid-2)
	/// <code><![CDATA[
	/// SfCartesianChart chart = new SfCartesianChart();
	/// 
	/// LogarithmicAxis xAxis = new LogarithmicAxis();
	/// chart.XAxes.Add(xAxis);	
	/// 
	/// LogarithmicAxis yAxis = new LogarithmicAxis();
	/// chart.YAxes.Add(yAxis);		
	/// 
	/// ]]>
	/// </code>
	/// ***
	/// 
	/// </example>
	public partial class LogarithmicAxis
	{
		#region Fields

		double _actualMinimum;
		double _actualMaximum;

		#endregion

		#region Bindable Properties

		/// <summary>
		/// Identifies the <see cref="Minimum"/> bindable property.
		/// </summary>
		/// <remarks>
		/// Defines the minimum value for the logarithmic axis.
		/// </remarks>
		public static readonly BindableProperty MinimumProperty = BindableProperty.Create(
			nameof(Minimum),
			typeof(double?),
			typeof(LogarithmicAxis),
			null,
			BindingMode.Default,
			null,
			OnMinimumPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="Maximum"/> bindable property.
		/// </summary>
		/// <remarks>
		/// Defines the maximum value for the logarithmic axis.
		/// </remarks>
		public static readonly BindableProperty MaximumProperty = BindableProperty.Create(
			nameof(Maximum),
			typeof(double?),
			typeof(LogarithmicAxis),
			null,
			BindingMode.Default,
			null,
			OnMaximumPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="LogarithmicBase"/> bindable property.
		/// </summary>
		/// <remarks>
		/// Defines the base of the logarithm for the logarithmic axis.
		/// </remarks>
		public static readonly BindableProperty LogarithmicBaseProperty = BindableProperty.Create(
			nameof(LogarithmicBase),
			typeof(double),
			typeof(LogarithmicAxis),
			10d,
			BindingMode.Default,
			null,
			OnLogarithmicBasePropertyChanged);

		/// <summary>
		/// Identifies the <see cref="Interval"/> bindable property.
		/// </summary>
		/// <remarks>
		/// Defines the interval value for the logarithmic axis.
		/// </remarks>
		public static readonly BindableProperty IntervalProperty = BindableProperty.Create(
			nameof(Interval),
			typeof(double),
			typeof(LogarithmicAxis),
			double.NaN,
			BindingMode.Default,
			null,
			OnIntervalPropertyChanged);

		/// <summary>
		/// Identifies the <see cref="PlotBands"/> bindable property.
		/// </summary>
		/// <remarks>
		/// Defines the collection of plot bands for the logarithmic axis.
		/// </remarks>
		public static readonly BindableProperty PlotBandsProperty = BindableProperty.Create(
			nameof(PlotBands),
			typeof(NumericalPlotBandCollection),
			typeof(LogarithmicAxis),
			null,
			BindingMode.Default,
			null,
			OnPlotBandsPropertyChanged);

		#endregion

		#region Properties

		internal DoubleRange VisibleLogRange { get; set; }
		internal double DefaultMinimum { get; set; } = double.NaN;
		internal double DefaultMaximum { get; set; } = double.NaN;

		/// <summary>
		/// Gets the actual minimum value of the logarithmic axis.
		/// </summary>
		/// <example>
		/// # [MainWindow.xaml](#tab/tabid-3)
		/// <code><![CDATA[
		///   <VerticalStackLayout>
		///     <Label Text="{Binding Source={x:Reference xAxis}, Path=ActualMinimum }" />
		///     <chart:SfCartesianChart>
		///
		///           <chart:SfCartesianChart.XAxes>
		///               <chart:LogarithmicAxis x:Name="xAxis"/>
		///           </chart:SfCartesianChart.XAxes>
		///
		///           <chart:SfCartesianChart.YAxes>
		///               <chart:LogarithmicAxis x:Name="yAxis"/>
		///           </chart:SfCartesianChart.YAxes>
		///
		///               <chart:ColumnSeries ItemsSource="{Binding Data}" 
		///                                   XBindingPath="XValue"
		///                                   YBindingPath="YValue" />
		///           
		///     </chart:SfCartesianChart>
		///   </VerticalStackLayout>
		/// ]]></code>
		/// # [MainWindow.cs](#tab/tabid-4)
		/// <code><![CDATA[
		///     SfCartesianChart chart = new SfCartesianChart();
		///     
		///     LogarithmicAxis xAxis = new LogarithmicAxis();
		///     LogarithmicAxis yAxis = new LogarithmicAxis();
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
		/// Gets the actual maximum value of the logarithmic axis
		/// </summary>
		/// <example>
		/// # [MainWindow.xaml](#tab/tabid-5)
		/// <code><![CDATA[
		///   <VerticalStackLayout>
		///     <Label Text="{Binding Source={x:Reference xAxis}, Path=ActualMaximum }" />
		///     <chart:SfCartesianChart>
		///
		///           <chart:SfCartesianChart.XAxes>
		///               <chart:LogarithmicAxis x:Name="xAxis"/>
		///           </chart:SfCartesianChart.XAxes>
		///
		///           <chart:SfCartesianChart.YAxes>
		///               <chart:LogarithmicAxis x:Name="yAxis"/>
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
		/// # [MainWindow.cs](#tab/tabid-6)
		/// <code><![CDATA[
		///     SfCartesianChart chart = new SfCartesianChart();
		///     
		///     LogarithmicAxis xAxis = new LogarithmicAxis();
		///     LogarithmicAxis yAxis = new LogarithmicAxis();
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
		/// Gets or sets the minimum value of visible range to be displayed on chart axis.
		/// </summary>
		/// <value>Defaults to null</value>
		/// <remarks>The value must be greater than 0.</remarks>
		/// <example>
		/// # [MainPage.xaml](#tab/tabid-7)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		///    <chart:SfCartesianChart.XAxes>
		///        <chart:CategoryAxis/>
		///    </chart:SfCartesianChart.XAxes>
		///    <chart:SfCartesianChart.YAxes>
		///        <chart:LogarithmicAxis Minimum="1"/>
		///    </chart:SfCartesianChart.YAxes>
		/// </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [MainPage.xaml.cs](#tab/tabid-8)
		/// <code><![CDATA[
		/// SfCartesianChart chart = new SfCartesianChart();
		/// 
		/// ViewModel viewModel = new ViewModel();
		///	chart.BindingContext = viewModel;
		/// 
		/// LogarithmicAxis yAxis = new LogarithmicAxis();
		/// yAxis.Minimum = 1;
		/// chart.YAxes.Add(yAxis);	
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
		/// Gets or sets the maximum value of the visible range to be displayed on chart axis.
		/// </summary>
		/// <value>Defaults to null</value>
		/// <remarks>The value must be greater than 1.</remarks>
		/// <example>
		/// # [MainPage.xaml](#tab/tabid-9)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		///    <chart:SfCartesianChart.XAxes>
		///        <chart:CategoryAxis/>
		///    </chart:SfCartesianChart.XAxes>
		///    <chart:SfCartesianChart.YAxes>
		///        <chart:LogarithmicAxis Maximum="1000"/>
		///    </chart:SfCartesianChart.YAxes>
		/// </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [MainPage.xaml.cs](#tab/tabid-10)
		/// <code><![CDATA[
		/// SfCartesianChart chart = new SfCartesianChart();
		/// 
		/// ViewModel viewModel = new ViewModel();
		///	chart.BindingContext = viewModel;
		/// 
		/// LogarithmicAxis yAxis = new LogarithmicAxis();
		/// yAxis.Maximum = 1000;
		/// chart.YAxes.Add(yAxis);	
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
		/// Gets or sets the logarithmic base value for the axis.
		/// </summary>
		/// <value>Default value is 10.</value>
		/// <remarks>The value cannot be less than 2.</remarks>
		/// <example>
		///  # [MainPage.xaml](#tab/tabid-11)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		///    <chart:SfCartesianChart.XAxes>
		///        <chart:CategoryAxis/>
		///    </chart:SfCartesianChart.XAxes>
		///    <chart:SfCartesianChart.YAxes>
		///        <chart:LogarithmicAxis LogarithmicBase="10"/>
		///    </chart:SfCartesianChart.YAxes>
		/// </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [MainPage.xaml.cs](#tab/tabid-12)
		/// <code><![CDATA[
		/// SfCartesianChart chart = new SfCartesianChart();
		/// 
		/// ViewModel viewModel = new ViewModel();
		///	chart.BindingContext = viewModel;
		/// 
		/// LogarithmicAxis yAxis = new LogarithmicAxis();
		/// yAxis.LogarithmicBase = 10;
		/// chart.YAxes.Add(yAxis);	
		/// 
		/// ]]>
		/// </code>
		/// ***
		/// </example>
		public double LogarithmicBase
		{
			get { return (double)GetValue(LogarithmicBaseProperty); }
			set { SetValue(LogarithmicBaseProperty, value); }
		}

		/// <summary>
		/// Gets or sets the interval value that represents the number of divisions required in the logarithmic axis.
		/// </summary>
		/// <value>Defaults to double.NaN</value>
		/// <remarks>Value must be greater than 0.</remarks>
		/// <example>
		///  # [MainPage.xaml](#tab/tabid-13)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		///    <chart:SfCartesianChart.XAxes>
		///        <chart:CategoryAxis/>
		///    </chart:SfCartesianChart.XAxes>
		///    <chart:SfCartesianChart.YAxes>
		///        <chart:LogarithmicAxis Interval="2"/>
		///    </chart:SfCartesianChart.YAxes>
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
		/// LogarithmicAxis yAxis = new LogarithmicAxis();
		/// yAxis.Interval = 2;
		/// chart.YAxes.Add(yAxis);
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
		/// Gets or sets the collection of plot bands to be added to the chart axis.
		/// </summary>
		/// # [MainPage.xaml](#tab/tabid-1)
		/// <code><![CDATA[
		/// <chart:SfCartesianChart>
		/// 
		///     <chart:SfCartesianChart.XAxes>
		///         <chart:LogarithmicAxis>
		///            <chart:NumericalAxis.PlotBands>
		///                <chart:NumericalPlotBand Start="1" Width="2"/>
		///            </chart:NumericalAxis.PlotBands>
		///        </chart:LogarithmicAxis>
		///     </chart:SfCartesianChart.XAxes>
		/// 
		/// </chart:SfCartesianChart>
		/// ]]>
		/// </code>
		/// # [MainPage.xaml.cs](#tab/tabid-2)
		/// <code><![CDATA[
		/// SfCartesianChart chart = new SfCartesianChart();
		/// 
		/// LogarithmicAxis xAxis = new LogarithmicAxis();
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

		static void OnMinimumPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is LogarithmicAxis axis)
			{
				axis.DefaultMinimum = Convert.ToDouble(newValue ?? double.NaN);
				axis.OnMinMaxChanged();
				axis.UpdateLayout();
			}
		}

		static void OnLogarithmicBasePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is LogarithmicAxis axis)
			{
				axis.UpdateLayout();
			}
		}

		static void OnIntervalPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is LogarithmicAxis axis)
			{
				axis.UpdateAxisInterval((double)newValue);
				axis.UpdateLayout();
			}
		}

		static void OnMaximumPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is LogarithmicAxis axis)
			{
				axis.DefaultMaximum = Convert.ToDouble(newValue ?? double.NaN);
				axis.OnMinMaxChanged();
				axis.UpdateLayout();
			}
		}

		#endregion
	}
}
