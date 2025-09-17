using System.Diagnostics.CodeAnalysis;

namespace Syncfusion.Maui.Toolkit.Charts;

/// <summary>
/// The DateTimeCategoryAxis is a specialized axis for plotting chronological data where the axis labels represent dates and times.
/// </summary>
/// <remarks>
/// 
/// <para>The DateTimeCategoryAxis is used only for the X(horizontal) axis in charts.</para>
/// 
/// <para>To render a DateTimeCategory axis, add its instance to the charts <see cref="SfCartesianChart.XAxes"/> collection as shown in the following code sample.</para>
/// 
/// # [MainPage.xaml](#tab/tabid-1)
/// <code><![CDATA[
/// <chart:SfCartesianChart>
///  
///         <chart:SfCartesianChart.XAxes>
///             <chart:DateTimeCategoryAxis Interval="1" IntervalType="Months"/>
///         </chart:SfCartesianChart.XAxes>
/// 
/// </chart:SfCartesianChart>
/// ]]>
/// </code>
/// # [MainPage.xaml.cs](#tab/tabid-2)
/// <code><![CDATA[
/// SfCartesianChart chart = new SfCartesianChart();
/// 
/// DateTimeCategoryAxis xAxis = new DateTimeCategoryAxis()
/// {
///    Interval = 1,
///    IntervalType = DateTimeIntervalType.Months
/// };
/// chart.XAxes.Add(xAxis);
/// 
/// ]]>
/// </code>
/// ***
/// 
/// <para>The DateTimeCategoryAxis supports the following features. Refer to the corresponding APIs for more details and example codes.</para>
/// 
/// <para> <b>Title - </b> To render the title on the axis, refer to the <see cref="ChartAxis.Title"/> property.</para>
/// <para> <b>Grid Lines - </b> To show and customize grid lines, refer to <see cref="ChartAxis.ShowMajorGridLines"/> and <see cref="ChartAxis.MajorGridLineStyle"/> properties.</para>
/// <para> <b>Axis Line - </b> Customize the axis line using the <see cref="ChartAxis.AxisLineStyle"/> property.</para>
/// <para> <b>Labels Customization - </b> Customize axis labels using the <see cref="ChartAxis.LabelStyle"/> property.</para>
/// <para> <b>Inversed Axis - </b> Invert the axis using the <see cref="ChartAxis.IsInversed"/> property.</para>
/// <para> <b>Axis Crossing - </b> For axis crossing, refer to <see cref="ChartAxis.CrossesAt"/>, <see cref="ChartAxis.CrossAxisName"/>, and <see cref="ChartAxis.RenderNextToCrossingValue"/> properties.</para>
/// <para> <b>Interval - </b> Define the interval between axis labels using the <see cref="Interval"/> and <see cref="IntervalType"/> properties.</para>
/// </remarks>
public partial class DateTimeCategoryAxis : ChartAxis
{
	#region Bindable Properties

	/// <summary>
	/// Identifies the <see cref="PlotBands"/> bindable property.
	/// </summary>
	/// <remarks>
	/// Defines the collection of plot bands for the DateTimeCategory axis.
	/// </remarks>
	public static readonly BindableProperty PlotBandsProperty = BindableProperty.Create(nameof(PlotBands), typeof(NumericalPlotBandCollection), typeof(DateTimeCategoryAxis), null, BindingMode.Default, null, OnPlotBandsPropertyChanged);

	/// <summary>
	/// Identifies the <see cref="Interval"/> bindable property.
	/// </summary>
	/// <remarks>
	/// The <see cref="Interval"/> property represents the interval value for the DateTimeCategory axis.
	/// </remarks> 
	public static readonly BindableProperty IntervalProperty = BindableProperty.Create(nameof(Interval), typeof(double), typeof(DateTimeCategoryAxis), double.NaN, BindingMode.Default, null, OnIntervalPropertyChanged);

	/// <summary>
	/// Identifies the <see cref="IntervalType"/> bindable property.
	/// </summary>
	/// <remarks>
	/// Defines the type of interval for the DateTimeCategory axis.
	/// </remarks>
	public static readonly BindableProperty IntervalTypeProperty = BindableProperty.Create(nameof(IntervalType), typeof(DateTimeIntervalType), typeof(DateTimeCategoryAxis), DateTimeIntervalType.Auto, BindingMode.Default, null, OnIntervalTypePropertyChanged);

	#endregion

	#region Public Properties

	/// <summary>
	/// Gets or sets the collection of plot bands.
	/// </summary>
	/// <value>It accepts <see cref="NumericalPlotBandCollection"/> and the default value is null.</value>
	/// <remarks>
	/// <para>Plot bands are used to shade specific regions in the chart to improve data visualization.</para>
	/// </remarks>
	/// <example>
	/// # [MainPage.xaml](#tab/tabid-3)
	/// <code><![CDATA[
	/// <chart:SfCartesianChart>
	///  
	///         <chart:SfCartesianChart.XAxes>
	///             <chart:DateTimeCategoryAxis>
	///                 <chart:DateTimeCategoryAxis.PlotBands>
	///                     <chart:NumericalPlotBand Start="2" End="5" Fill="LightGray" />
	///                 </chart:DateTimeCategoryAxis.PlotBands>
	///             </chart:DateTimeCategoryAxis>
	///         </chart:SfCartesianChart.XAxes>
	/// 
	/// </chart:SfCartesianChart>
	/// ]]>
	/// </code>
	/// # [MainPage.xaml.cs](#tab/tabid-4)
	/// <code><![CDATA[
	/// SfCartesianChart chart = new SfCartesianChart();
	/// 
	/// DateTimeCategoryAxis xAxis = new DateTimeCategoryAxis();
	/// NumericalPlotBandCollection plotBands = new NumericalPlotBandCollection();
	/// plotBands.Add(new NumericalPlotBand { Start = 2, End = 5, Fill = new SolidColorBrush(Colors.LightGray) });
	/// xAxis.PlotBands = plotBands;
	/// chart.XAxes.Add(xAxis);
	/// 
	/// ]]>
	/// </code>
	/// ***
	/// 
	/// </example>
	public NumericalPlotBandCollection PlotBands
	{
		get { return (NumericalPlotBandCollection)GetValue(PlotBandsProperty); }
		set { SetValue(PlotBandsProperty, value); }
	}

	/// <summary>
	/// Gets or sets a value that can be used to change the interval between labels.
	/// </summary>
	/// <value>It accepts <c>double</c> values and the default value is double.NaN.</value>
	/// <remarks>
	/// <para>If this property is not set, the interval will be calculated automatically.</para>
	/// <para>By default, the interval will be calculated based on the minimum and maximum values of the provided data.</para>
	/// </remarks>
	/// <example>
	/// # [MainPage.xaml](#tab/tabid-5)
	/// <code><![CDATA[
	/// <chart:SfCartesianChart>
	///  
	///         <chart:SfCartesianChart.XAxes>
	///             <chart:DateTimeCategoryAxis Interval="6"/>
	///         </chart:SfCartesianChart.XAxes>
	/// 
	/// </chart:SfCartesianChart>
	/// ]]>
	/// </code>
	/// # [MainPage.xaml.cs](#tab/tabid-6)
	/// <code><![CDATA[
	/// SfCartesianChart chart = new SfCartesianChart();
	/// 
	/// DateTimeCategoryAxis xAxis = new DateTimeCategoryAxis()
	/// {
	///    Interval = 6, 
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
	/// Gets or sets the type of interval to be displayed in the axis.
	/// </summary>
	/// <value>It accepts <see cref="DateTimeIntervalType"/> values and the default value is DateTimeIntervalType.Auto.</value>
	/// <remarks>
	/// <para>The IntervalType property determines the unit of measurement for the axis interval.</para>
	/// <para>When set to Auto, the interval type will be calculated automatically based on the data range.</para>
	/// </remarks>
	/// <example>
	/// # [MainPage.xaml](#tab/tabid-7)
	/// <code><![CDATA[
	/// <chart:SfCartesianChart>
	///  
	///         <chart:SfCartesianChart.XAxes>
	///             <chart:DateTimeCategoryAxis Interval="1" IntervalType="Years" />
	///         </chart:SfCartesianChart.XAxes>
	/// 
	/// </chart:SfCartesianChart>
	/// ]]>
	/// </code>
	/// # [MainPage.xaml.cs](#tab/tabid-8)
	/// <code><![CDATA[
	/// SfCartesianChart chart = new SfCartesianChart();
	/// 
	/// DateTimeCategoryAxis xAxis = new DateTimeCategoryAxis()
	/// {
	///    Interval = 1, 
	///    IntervalType = DateTimeIntervalType.Years
	/// };
	/// chart.XAxes.Add(xAxis);
	/// 
	/// ]]>
	/// </code>
	/// ***
	/// 
	/// </example>
	/// <see cref="Interval"/>
	public DateTimeIntervalType IntervalType
	{
		get { return (DateTimeIntervalType)GetValue(IntervalTypeProperty); }
		set { SetValue(IntervalTypeProperty, value); }
	}

	#endregion

	#region Internal Property

	internal DateTimeIntervalType ActualIntervalType { get; set; }

	#endregion

	#region Methods

	#region Internal Override

	/// <inheritdoc/>
	protected sealed override DoubleRange ApplyRangePadding(DoubleRange range, double interval)
	{
		return range;
	}

	/// <inheritdoc/>
	protected override double CalculateNiceInterval(DoubleRange actualRange, Size availableSize)
	{
		var dateTimeMin = DateTime.FromOADate(actualRange.Start);
		var dateTimeMax = DateTime.FromOADate(actualRange.End);
		var timeSpan = dateTimeMax.Subtract(dateTimeMin);
		double interval = 0;

		switch (IntervalType)
		{
			case DateTimeIntervalType.Years:
				interval = base.CalculateNiceInterval(new DoubleRange(0, timeSpan.TotalDays / 365), availableSize);
				break;
			case DateTimeIntervalType.Months:
				interval = base.CalculateNiceInterval(new DoubleRange(0, timeSpan.TotalDays / 30), availableSize);
				break;
			case DateTimeIntervalType.Days:
				interval = base.CalculateNiceInterval(new DoubleRange(0, timeSpan.TotalDays), availableSize);
				break;
			case DateTimeIntervalType.Hours:
				interval = base.CalculateNiceInterval(new DoubleRange(0, timeSpan.TotalHours), availableSize);
				break;
			case DateTimeIntervalType.Minutes:
				interval = base.CalculateNiceInterval(new DoubleRange(0, timeSpan.TotalMinutes), availableSize);
				break;
			case DateTimeIntervalType.Seconds:
				interval = base.CalculateNiceInterval(new DoubleRange(0, timeSpan.TotalSeconds), availableSize);
				break;
			case DateTimeIntervalType.Milliseconds:
				interval = base.CalculateNiceInterval(new DoubleRange(0, timeSpan.TotalMilliseconds), availableSize);
				break;
			case DateTimeIntervalType.Auto:
				interval = CalculateIntervalForAutoType(availableSize, timeSpan);
				break;
		}

		return Math.Max(1d, interval);
	}

	/// <inheritdoc/>
	protected override double CalculateActualInterval(DoubleRange range, Size availableSize)
	{
		if (double.IsNaN(AxisInterval) || AxisInterval == 0)
		{
			return CalculateNiceInterval(range, availableSize);
		}

		return AxisInterval;
	}

	[RequiresUnreferencedCode("The GenerateVisibleLabels is not trim compatible")]
	internal override void GenerateVisibleLabels()
	{
		if (VisibleRange.IsEmpty)
		{
			return;
		}

		_isOverriddenOnCreateLabelsMethod = ChartUtils.IsOverriddenMethod(this, "OnCreateLabels");

		DoubleRange visibleRange = VisibleRange;
		double actualInterval = ActualInterval;
		double interval = (AxisInterval != 0 && !double.IsNaN(AxisInterval)) ? AxisInterval : ActualInterval;
		double position = visibleRange.Start - (visibleRange.Start % actualInterval);
		var actualSeries = GetActualSeries();

		if (actualSeries == null)
		{
			return;
		}

		if (actualSeries.ActualXValues is List<double> xValues)
		{
			switch (ActualIntervalType)
			{
				case DateTimeIntervalType.Years:
					AddYearTypeLabels(xValues, position, interval, _isOverriddenOnCreateLabelsMethod);
					break;
				case DateTimeIntervalType.Months:
					AddMonthTypeLabels(xValues, position, interval, _isOverriddenOnCreateLabelsMethod);
					break;
				case DateTimeIntervalType.Days:
					AddDayTypeLabels(xValues, position, interval, _isOverriddenOnCreateLabelsMethod);
					break;
				case DateTimeIntervalType.Hours:
					AddHourTypeLabels(xValues, position, interval, _isOverriddenOnCreateLabelsMethod);
					break;
				case DateTimeIntervalType.Minutes:
					AddMinutesTypeLabels(xValues, position, interval, _isOverriddenOnCreateLabelsMethod);
					break;
				case DateTimeIntervalType.Seconds:
					AddSecondTypeLabels(xValues, position, interval, _isOverriddenOnCreateLabelsMethod);
					break;
				case DateTimeIntervalType.Milliseconds:
					AddMillisecondTypeLabels(xValues, position, interval, _isOverriddenOnCreateLabelsMethod);
					break;
				case DateTimeIntervalType.Auto:
					AddAutoTypeLabels(xValues, position, interval, _isOverriddenOnCreateLabelsMethod);
					break;
			}
		}

		if (_isOverriddenOnCreateLabelsMethod)
		{
			OnCreateLabels();
			AddVisibleLabels();
		}
	}

	#endregion

	#region Internal Methods

	internal string GetLabelContent(double position, string labelFormat)
	{
		var actualSeries = GetActualSeries();
		if (actualSeries != null)
		{
			labelFormat = LabelStyle.LabelFormat ?? GetSpecificFormattedLabel(ActualIntervalType);
			if (actualSeries.ActualXValues is List<double> xValues && position < xValues.Count && position >= 0)
			{
				double xDateTime = xValues[(int)position];
				return GetFormattedAxisLabel(labelFormat, xDateTime);
			}
		}

		return position.ToString();
	}

	#endregion

	#region Callback Method

	private static void OnIntervalPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is DateTimeCategoryAxis dateTimeCategoryAxis)
		{
			dateTimeCategoryAxis.UpdateAxisInterval((double)newValue);
			dateTimeCategoryAxis.UpdateLayout();
		}
	}

	private static void OnIntervalTypePropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is DateTimeCategoryAxis dateTimeCategoryAxis)
		{
			dateTimeCategoryAxis.UpdateActualIntervalType((DateTimeIntervalType)newValue);
			dateTimeCategoryAxis.UpdateLayout();
		}
	}

	void UpdateActualIntervalType(DateTimeIntervalType intervalType)
	{
		ActualIntervalType = intervalType;
	}

	#endregion

	#region Private Methods

	private double CalculateIntervalForAutoType(SizeF availableSize, TimeSpan timeSpan)
	{
		var intervalTypes = new List<(double RangeDivision, DateTimeIntervalType IntervalType)>
		{
			(timeSpan.TotalDays / 365, DateTimeIntervalType.Years),
			(timeSpan.TotalDays / 30, DateTimeIntervalType.Months),
			(timeSpan.TotalDays, DateTimeIntervalType.Days),
			(timeSpan.TotalHours, DateTimeIntervalType.Hours),
			(timeSpan.TotalMinutes, DateTimeIntervalType.Minutes),
			(timeSpan.TotalSeconds, DateTimeIntervalType.Seconds),
			(timeSpan.TotalMilliseconds, DateTimeIntervalType.Milliseconds)
		};

		double interval = 0;

		// Before the loop starts, set ActualIntervalType to the last item in the list.
		// If none meet the condition interval >= 1, it remains as it is the last item.
		ActualIntervalType = intervalTypes.Last().IntervalType;

		foreach (var (rangeDivision, intervalType) in intervalTypes)
		{
			interval = base.CalculateNiceInterval(new DoubleRange(0, rangeDivision), availableSize);

			//If any interval meets the condition interval >= 1, it will override the default.
			if (interval >= 1)
			{
				ActualIntervalType = intervalType;
				break;
			}
		}

		return interval;
	}

	private void AddAutoTypeLabels(List<double> xValues, double position, double interval, bool isOverriddenOnCreateLabelsMethod)
	{
		for (; position <= VisibleRange.End; position += interval)
		{
			if (VisibleRange.Inside(position) && position < xValues.Count && position > -1)
			{
				int pos = (int)Math.Round(position);
				var value = xValues[pos];
				AddVisibleLabels(value, pos, isOverriddenOnCreateLabelsMethod);
			}
		}
	}

	private void AddMillisecondTypeLabels(List<double> xValues, double position, double interval, bool isOverriddenOnCreateLabelsMethod)
	{
		var xStartValue = DateTime.FromOADate(xValues[0]);
		int hour = xStartValue.Hour,
			mins = xStartValue.Minute,
			secs = xStartValue.Second,
			millisecs = xStartValue.Millisecond;
		DateTime date = xStartValue.Date;

		for (; position <= VisibleRange.End; position++)
		{
			int pos = (int)Math.Round(position);
			if (VisibleRange.Inside(position) && pos > -1 && pos < xValues.Count)
			{
				var xValue = DateTime.FromOADate(xValues[pos]);
				if (xValue.Date > date)
				{
					date = xValue.Date;
					hour = xValue.Hour;
				}

				if (xValue.Date == date)
				{
					if (xValue.Hour > hour)
					{
						hour = xValue.Hour;
						mins = xValue.Minute;
					}

					if (xValue.Hour == hour)
					{
						if (xValue.Minute > mins)
						{
							mins = xValue.Minute;
							secs = xValue.Second;
						}

						if (xValue.Minute == mins)
						{
							if (xValue.Second > secs)
							{
								secs = xValue.Second;
								millisecs = xValue.Millisecond;
							}

							if (xValue.Second == secs)
							{
								if (xValue.Millisecond > millisecs)
								{
									millisecs = xValue.Millisecond;
								}

								if (xValue.Millisecond == millisecs)
								{
									var value = xValues[pos];
									AddVisibleLabels(value, pos, isOverriddenOnCreateLabelsMethod);
									hour = xValue.AddMilliseconds((int)interval).Hour;
									mins = xValue.AddMilliseconds((int)interval).Minute;
									date = xValue.AddMilliseconds((int)interval).Date;
									secs = xValue.AddMilliseconds((int)interval).Second;
									millisecs = xValue.AddMilliseconds((int)interval).Millisecond;
								}
							}
						}
					}
				}
			}
		}
	}

	private void AddSecondTypeLabels(List<double> xValues, double position, double interval, bool isOverriddenOnCreateLabelsMethod)
	{
		var xStartValue = DateTime.FromOADate(xValues[0]);
		int hour = xStartValue.Hour,
			mins = xStartValue.Minute,
			secs = xStartValue.Second;
		DateTime date = xStartValue.Date;

		for (; position <= VisibleRange.End; position++)
		{
			int pos = (int)Math.Round(position);
			if (VisibleRange.Inside(position) && pos > -1 && pos < xValues.Count)
			{
				var xValue = DateTime.FromOADate(xValues[pos]);
				if (xValue.Date > date)
				{
					date = xValue.Date;
					hour = xValue.Hour;
				}

				if (xValue.Date == date)
				{
					if (xValue.Hour > hour)
					{
						hour = xValue.Hour;
						mins = xValue.Minute;
					}

					if (xValue.Hour == hour)
					{
						if (xValue.Minute > mins)
						{
							mins = xValue.Minute;
							secs = xValue.Second;
						}

						if (xValue.Minute == mins)
						{
							if (xValue.Second > secs)
							{
								secs = xValue.Second;
							}

							if (xValue.Second == secs)
							{
								var value = xValues[pos];
								AddVisibleLabels(value, pos, isOverriddenOnCreateLabelsMethod);
								hour = xValue.AddSeconds((int)interval).Hour;
								mins = xValue.AddSeconds((int)interval).Minute;
								date = xValue.AddSeconds((int)interval).Date;
								secs = xValue.AddSeconds((int)interval).Second;
							}
						}
					}
				}
			}
		}
	}

	private void AddMinutesTypeLabels(List<double> xValues, double position, double interval, bool isOverriddenOnCreateLabelsMethod)
	{
		var xStartValue = DateTime.FromOADate(xValues[0]);
		int hour = xStartValue.Hour, mins = xStartValue.Minute;
		DateTime date = xStartValue.Date;

		for (; position <= VisibleRange.End; position++)
		{
			int pos = (int)Math.Round(position);

			if (VisibleRange.Inside(position) && pos > -1 && pos < xValues.Count)
			{
				var xValue = DateTime.FromOADate(xValues[pos]);
				if (xValue.Date > date)
				{
					date = xValue.Date;
					hour = xValue.Hour;
				}

				if (xValue.Date == date)
				{
					if (xValue.Hour > hour)
					{
						hour = xValue.Hour;
						mins = xValue.Minute;
					}

					if (xValue.Hour == hour)
					{
						if (xValue.Minute > mins)
						{
							mins = xValue.Minute;
						}

						if (xValue.Minute == mins)
						{
							var value = xValues[pos];
							AddVisibleLabels(value, pos, isOverriddenOnCreateLabelsMethod);
							hour = xValue.AddMinutes((int)interval).Hour;
							mins = xValue.AddMinutes((int)interval).Minute;
							date = xValue.AddMinutes((int)interval).Date;
						}
					}
				}
			}
		}
	}

	private void AddHourTypeLabels(List<double> xValues, double position, double interval, bool isOverriddenOnCreateLabelsMethod)
	{
		var xStartValue = DateTime.FromOADate(xValues[0]);
		int hour = xStartValue.Hour;
		DateTime date = xStartValue.Date;

		for (; position <= VisibleRange.End; position++)
		{
			int pos = (int)Math.Round(position);
			if (VisibleRange.Inside(position) && pos > -1 && pos < xValues.Count)
			{
				var xValue = DateTime.FromOADate(xValues[pos]);
				if (xValue.Date > date)
				{
					date = xValue.Date;
					hour = xValue.Hour;
				}

				if (xValue.Date == date)
				{
					if (xValue.Hour > hour)
					{
						hour = xValue.Hour;
					}

					if (xValue.Hour == hour)
					{
						var value = xValues[pos];
						AddVisibleLabels(value, pos, isOverriddenOnCreateLabelsMethod);
						hour = xValue.AddHours((int)interval).Hour;
						date = xValue.AddHours((int)interval).Date;
					}
				}
			}
		}
	}

	private void AddDayTypeLabels(List<double> xValues, double position, double interval, bool isOverriddenOnCreateLabelsMethod)
	{
		var xStartValue = DateTime.FromOADate(xValues[0]);
		DateTime date = xStartValue.Date;

		for (; position <= VisibleRange.End; position++)
		{
			int pos = (int)Math.Round(position);
			if (VisibleRange.Inside(position) && pos > -1 && pos < xValues.Count)
			{
				var xValue = DateTime.FromOADate(xValues[pos]);
				if (xValue.Date > date)
				{
					date = xValue.Date;
				}

				if (xValue.Date == date)
				{
					var value = xValues[pos];
					AddVisibleLabels(value, pos, isOverriddenOnCreateLabelsMethod);
					date = xValue.AddDays((int)interval).Date;
				}
			}
		}
	}

	private void AddMonthTypeLabels(List<double> xValues, double position, double interval, bool isOverriddenOnCreateLabelsMethod)
	{
		var xStartValue = DateTime.FromOADate(xValues[0]);
		int year = xStartValue.Year, month = xStartValue.Month;

		for (; position <= VisibleRange.End; position++)
		{
			int pos = (int)Math.Round(position);
			if (VisibleRange.Inside(position) && pos > -1 && pos < xValues.Count)
			{
				var xValue = DateTime.FromOADate(xValues[pos]);
				if (xValue.Year > year)
				{
					year = xValue.Year;
					month = xValue.Month;
				}

				if (xValue.Year == year)
				{
					if (xValue.Month > month)
					{
						month = xValue.Month;
					}

					if (xValue.Month == month)
					{
						var value = xValues[pos];
						AddVisibleLabels(value, pos, isOverriddenOnCreateLabelsMethod);
						month = xValue.AddMonths((int)interval).Month;
						year = xValue.AddMonths((int)interval).Year;
					}
				}
			}
		}
	}

	private void AddYearTypeLabels(List<double> xValues, double position, double interval, bool isOverriddenOnCreateLabelsMethod)
	{
		var xStartValue = DateTime.FromOADate(xValues[0]);
		int year = xStartValue.Year;

		for (; position <= VisibleRange.End; position++)
		{
			int pos = (int)Math.Round(position);
			if (VisibleRange.Inside(position) && pos > -1 && pos < xValues.Count)
			{
				var xValue = DateTime.FromOADate(xValues[pos]);
				if (xValue.Year > year)
				{
					year = xValue.Year;
				}

				if (xValue.Year == year)
				{
					var value = xValues[pos];
					AddVisibleLabels(value, pos, _isOverriddenOnCreateLabelsMethod);
					year = xValue.AddYears((int)interval).Year;
				}
			}
		}
	}

	private void AddVisibleLabels(double value, int pos, bool isOverriddenOnCreateLabelsMethod)
	{
		var labelFormat = LabelStyle.LabelFormat ?? GetSpecificFormattedLabel(ActualIntervalType);
		var labelContent = GetFormattedAxisLabel(labelFormat, value);
		var axisLabel = new ChartAxisLabel(pos, labelContent);
		VisibleLabels.Add(axisLabel);
		
		if (!_isOverriddenOnCreateLabelsMethod)
		{
			TickPositions.Add(pos);
		}
	}

	private ChartSeries? GetActualSeries()
	{
		var visibleSeries = GetVisibleSeries();
		if (visibleSeries == null)
		{
			return null;
		}

		int dataCount = 0;
		ChartSeries? selectedSeries = null;

		if (IsPolarArea)
		{
			foreach (PolarSeries series in visibleSeries.Cast<PolarSeries>())
			{
				if (series != null && series.ActualXAxis == this && series.PointsCount > dataCount)
				{
					selectedSeries = series;
					dataCount = series.PointsCount;
				}
			}
		}
		else
		{

			foreach (CartesianSeries series in visibleSeries.Cast<CartesianSeries>())
			{
				if (series != null && series.ActualXAxis == this && series.PointsCount > dataCount)
				{
					selectedSeries = series;
					dataCount = series.PointsCount;
				}
			}
		}

		return selectedSeries;
	}

	#endregion

	#endregion
}
