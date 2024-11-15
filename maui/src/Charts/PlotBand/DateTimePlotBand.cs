using System.Collections.ObjectModel;

namespace Syncfusion.Maui.Toolkit.Charts
{
    /// <summary>
    /// The DateTimePlotBand class can render the plot band using the DateTime values. 
    /// </summary>
    /// <remarks> 
    /// <para>DateTimePlotBand requires <see cref="Start"/> and <see cref="End"/> properties to be set to render the Plot band. </para>
    /// </remarks>
    /// <example>
    /// # [MainWindow.xaml](#tab/tabid-1)
    /// <code><![CDATA[
    /// <chart:SfCartesianChart>
    /// 
    ///     <chart:SfCartesianChart.XAxes>
    ///         <chart:DateTimeAxis>
    ///            <chart:DateTimeAxis.PlotBands>
    ///              <chart:DateTimePlotBandCollection>
    ///                <chart:DateTimePlotBand Start="2000-01-01" Size="2" SizeType="Month"/>
    ///              </chart:DateTimePlotBandCollection>
    ///            </chart:DateTimeAxis.PlotBands>
    ///        </chart:DateTimeAxis>
    ///     </chart:SfCartesianChart.XAxes>
    /// 
    /// </chart:SfCartesianChart>
    /// ]]>
    /// </code>
    /// # [MainWindow.cs](#tab/tabid-2)
    /// <code><![CDATA[
    /// SfCartesianChart chart = new SfCartesianChart();
    /// 
    /// DateTimeAxis xAxis = new DateTimeAxis();
    /// DateTimePlotBandCollection bands = new DateTimePlotBandCollection();
    /// DateTimePlotBand plotBand = new DateTimePlotBand();
    /// plotBand.Start = new DateTime(2000,01,01);
    /// plotBand.Size = 2;
    /// plotBand.SizeType = DateTimeIntervalType.Months;
    /// bands.Add(plotBand);
    /// xAxis.PlotBands = bands;
    /// 
    /// chart.XAxes.Add(xAxis);
    /// ]]>
    /// </code>
    /// ***
    /// </example>
    public class DateTimePlotBand : ChartPlotBand
    {
        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="Start"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The identifier for the <see cref="Start"/> bindable property determines the
        /// start date and time of the plot band.
        /// </remarks>
        public static readonly BindableProperty StartProperty = BindableProperty.Create(
            nameof(Start),
            typeof(DateTime?),
            typeof(DateTimePlotBand),
            null,
            BindingMode.Default,
            null,
            null,
            OnPlotBandPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="End"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The identifier for the <see cref="End"/> bindable property determines the 
        /// end date and time of the plot band.
        /// </remarks>
        public static readonly BindableProperty EndProperty = BindableProperty.Create(
            nameof(End),
            typeof(DateTime?),
            typeof(DateTimePlotBand),
            null,
            BindingMode.Default,
            null,
            null,
            OnPlotBandPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="RepeatUntil"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The identifier for the <see cref="RepeatUntil"/> bindable property determines the 
        /// date and time until which the plot band repeats.
        /// </remarks>
        public static readonly BindableProperty RepeatUntilProperty = BindableProperty.Create(
            nameof(RepeatUntil),
            typeof(DateTime?),
            typeof(DateTimePlotBand),
            null,
            BindingMode.Default,
            null,
            null,
            OnPlotBandPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="SizeType"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The identifier for the <see cref="SizeType"/> bindable property determines the 
        /// interval type for the size of the plot band.
        /// </remarks>
        public static readonly BindableProperty SizeTypeProperty = BindableProperty.Create(
            nameof(SizeType),
            typeof(DateTimeIntervalType),
            typeof(DateTimePlotBand),
            DateTimeIntervalType.Auto,
            BindingMode.Default,
            null,
            null,
            OnPlotBandPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="RepeatEveryType"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The identifier for the <see cref="RepeatEveryType"/> bindable property determines the 
        /// interval type for the repetition of the plot band.
        /// </remarks>
        public static readonly BindableProperty RepeatEveryTypeProperty = BindableProperty.Create(
            nameof(RepeatEveryType),
            typeof(DateTimeIntervalType),
            typeof(DateTimePlotBand),
            DateTimeIntervalType.Auto,
            BindingMode.Default,
            null,
            null,
            OnPlotBandPropertyChanged);

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the axis value representing where the plot band should start on the axis.
        /// </summary>
        /// <value>This property takes <see cref="DateTime"/> as its value and the default value is null.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-3)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///        <chart:SfCartesianChart.XAxes>
        ///           <chart:DateTimeAxis>
        ///              <chart:DateTimeAxis.PlotBands>
        ///                 <chart:DateTimePlotBandCollection>
        ///                   <chart:DateTimePlotBand Start="2000-01-01" Size="2"/>
        ///                 </chart:DateTimePlotBandCollection>
        ///              </chart:DateTimeAxis.PlotBands>
        ///             </chart:DateTimeAxis>
        ///         </chart:SfCartesianChart.XAxes>
        ///     <!-- ... Eliminated for simplicity-->             
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-4)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        /// 
        ///     DateTimeAxis xAxis = new DateTimeAxis();
        ///     DateTimePlotBandCollection bands = new DateTimePlotBandCollection();  
        ///     DateTimePlotBand plotBand = new DateTimePlotBand();
        ///     plotBand.Start = new DateTime(2000,01,01);
        ///     plotBand.Size = 2;
        ///     bands.Add(plotBand);
        ///     xAxis.PlotBands = bands;
        /// 
        ///     chart.XAxes.Add(xAxis);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public DateTime? Start
        {
            get { return (DateTime?)GetValue(StartProperty); }
            set { SetValue(StartProperty, value); }
        }

        /// <summary>
        /// Gets or sets the axis value representing where the plot band should end on the axis.
        /// </summary>
        /// <value>This property takes <see cref="DateTime"/> as its value and the default value is null.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-5)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///        <chart:SfCartesianChart.XAxes>
        ///           <chart:DateTimeAxis>
        ///              <chart:DateTimeAxis.PlotBands>
        ///                 <chart:DateTimePlotBandCollection>
        ///                     <chart:DateTimePlotBand Start="2000-01-01" End="2000-02-02"/>
        ///                 <chart:DateTimePlotBandCollection>
        ///              </chart:DateTimeAxis.PlotBands>
        ///             </chart:DateTimeAxis>
        ///         </chart:SfCartesianChart.XAxes>
        ///     <!-- ... Eliminated for simplicity-->             
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-6)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        /// 
        ///     DateTimeAxis xAxis = new DateTimeAxis();
        ///     DateTimePlotBandCollection bands = new DateTimePlotBandCollection(); 
        ///     DateTimePlotBand plotBand = new DateTimePlotBand();
        ///     plotBand.Start = new DateTime(2000,01,01);
        ///     plotBand.End = new DateTime(2000,02,02);
        ///     bands.Add(plotBand);
        ///     xAxis.PlotBands = bands;
        /// 
        ///     chart.XAxes.Add(xAxis);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public DateTime? End
        {
            get { return (DateTime?)GetValue(EndProperty); }
            set { SetValue(EndProperty, value); }
        }

        /// <summary>
        /// Gets or sets the axis value that determines how long the plot band will be repeated along the axis.
        /// </summary>
        /// <value>This property takes <see cref="DateTime"/> as its value and the default value is null.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-7)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///        <chart:SfCartesianChart.XAxes>
        ///           <chart:DateTimeAxis>
        ///              <chart:DateTimeAxis.PlotBands>
        ///                 <chart:DateTimePlotBandCollection>
        ///                     <chart:DateTimePlotBand Start="2000-01-01" End="2000-02-02" RepeatUntil="2000-03-01"/>
        ///                 <chart:DateTimePlotBandCollection>
        ///              </chart:DateTimeAxis.PlotBands>
        ///             </chart:DateTimeAxis>
        ///         </chart:SfCartesianChart.XAxes>
        ///     <!-- ... Eliminated for simplicity-->             
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-8)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        /// 
        ///     DateTimeAxis xAxis = new DateTimeAxis();
        ///     DateTimePlotBandCollection bands = new DateTimePlotBandCollection(); 
        ///     DateTimePlotBand plotBand = new DateTimePlotBand();
        ///     plotBand.Start = new DateTime(2000,01,01);
        ///     plotBand.End =  new DateTime(2000,02,02);
        ///     plotBand.RepeatUntil = new DateTime(2000,03,01);
        ///     bands.Add(plotBand);
        ///     xAxis.PlotBands = bands;
        /// 
        ///     chart.XAxes.Add(xAxis);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public DateTime? RepeatUntil
        {
            get { return (DateTime?)GetValue(RepeatUntilProperty); }
            set { SetValue(RepeatUntilProperty, value); }
        }

        /// <summary>
        /// Gets or sets the date time unit specified in the <b> SizeType </b> property to determine the size of the plot band. 
        /// </summary>
        /// <value>This property takes the <see cref="DateTimeIntervalType"/> value and the default value is <see cref="DateTimeIntervalType.Auto"/>.</value>
        /// <remarks> If <see cref="SizeType"/> is auto, it will take the default interval from the dateTime axis.</remarks>
        /// <example>
        /// # [Xaml](#tab/tabid-9)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///        <chart:SfCartesianChart.XAxes>
        ///           <chart:DateTimeAxis>
        ///              <chart:DateTimeAxis.PlotBands>
        ///                  <chart:DateTimePlotBandCollection>
        ///                     <chart:DateTimePlotBand Start="2000-01-01" End="2000-02-02" SizeType="Months"/>
        ///                   </chart:DateTimePlotBandCollection>
        ///              </chart:DateTimeAxis.PlotBands>
        ///             </chart:DateTimeAxis>
        ///         </chart:SfCartesianChart.XAxes>
        ///     <!-- ... Eliminated for simplicity-->             
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-10)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        /// 
        ///     DateTimeAxis xAxis = new DateTimeAxis();
        ///     DateTimePlotBandCollection bands = new DateTimePlotBandCollection();  
        ///     DateTimePlotBand plotBand = new DateTimePlotBand();
        ///     plotBand.Start = new DateTime(2000,01,01);
        ///     plotBand.End =  new DateTime(2000,02,02);
        ///     plotBand.SizeType = DateTimeIntervalType.Months;
        ///     bands.Add(plotBand);
        ///     xAxis.PlotBands = bands;
        /// 
        ///     chart.XAxes.Add(xAxis);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example> 
        public DateTimeIntervalType SizeType
        {
            get { return (DateTimeIntervalType)GetValue(SizeTypeProperty); }
            set { SetValue(SizeTypeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the date time unit of the value specified in the <b> RepeatEvery </b> property.
        /// </summary>
        /// <value>This property takes the <see cref="DateTimeIntervalType"/> value and the default value is <see cref="DateTimeIntervalType.Auto"/>.</value>
        /// <remarks> If <see cref="RepeatEveryType"/> is auto, it will take the default interval from the dateTime axis.</remarks>
        /// <example>
        /// # [Xaml](#tab/tabid-11)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///        <chart:SfCartesianChart.XAxes>
        ///           <chart:DateTimeAxis>
        ///              <chart:DateTimeAxis.PlotBands>
        ///                 <chart:DateTimePlotBandCollection>
        ///                     <chart:DateTimePlotBand Start="2000-01-01" End="2000-02-02" SizeType="Months" RepeatEveryType="Days"/>
        ///                  </chart:DateTimePlotBandCollection>
        ///              </chart:DateTimeAxis.PlotBands>
        ///             </chart:DateTimeAxis>
        ///         </chart:SfCartesianChart.XAxes>
        ///     <!-- ... Eliminated for simplicity-->             
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-12)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        /// 
        ///     DateTimeAxis xAxis = new DateTimeAxis();
        ///     DateTimePlotBandCollection bands = new DateTimePlotBandCollection();  
        ///     DateTimePlotBand plotBand = new DateTimePlotBand();
        ///     plotBand.Start = new DateTime(2000,01,01);
        ///     plotBand.End = new DateTime(2000,02,02);
        ///     plotBand.SizeType = DateTimeIntervalType.Months;
        ///     plotBand.RepeatEveryType = DateTimeIntervalType.Days;
        ///     
        ///     bands.Add(plotBand);
        ///     xAxis.PlotBands = bands;
        /// 
        ///     chart.XAxes.Add(xAxis);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example> 
        public DateTimeIntervalType RepeatEveryType
        {
            get { return (DateTimeIntervalType)GetValue(RepeatEveryTypeProperty); }
            set { SetValue(RepeatEveryTypeProperty, value); }
        }

        #endregion

        #region Methods

        #region Internal Method

        internal override void DrawPlotBands(ICanvas canvas, RectF dirtyRect)
        {
            var axis = Axis;
            var start = ChartUtils.ConvertToDouble(Start);
            var repeatUntil = ChartUtils.ConvertToDouble(RepeatUntil);

            if (axis == null || double.IsNaN(start))
                return;

            var associatedAxis = GetAssociatedAxis(axis);
            if (associatedAxis == null)
                return;

            var repeatEnd = double.IsNaN(repeatUntil) || repeatUntil == 0 ? axis.VisibleRange.End : repeatUntil;
            var periodBand = Math.Abs(GetActualPeriodStrip(start));
            periodBand = repeatEnd < start ? -periodBand : periodBand;
            DrawRepeatingPlotBands(canvas, axis, associatedAxis, start, repeatEnd, periodBand);
        }

        #endregion

        #region Private Methods

        double GetBandWidth(double startBand)
        {
            var end = ChartUtils.ConvertToDouble(End);

            if (!double.IsNaN(end))
            {
                return Start < End ? Math.Abs(end - startBand) : Math.Abs(startBand - end);
            }

            return !double.IsNaN(Size) ? GetBandWidth(startBand, Size, SizeType) : double.NaN;
        }

        void DrawRepeatingPlotBands(ICanvas canvas, ChartAxis axis, ChartAxis associatedAxis, double start, double repeatEnd, double periodBand)
        {
            var width = GetBandWidth(start);
            if (double.IsNaN(width))
            {
                width = axis.VisibleRange.End;
            }

            do
            {
                var endBand = start + width;
                var drawRect = !axis.IsVertical
                    ? GetHorizontalDrawRect(axis, associatedAxis, start, endBand)
                    : GetVerticalDrawRect(axis, associatedAxis, start, endBand);

                if (Start == End && (axis.IsVertical ? drawRect.Height : drawRect.Width) == 0)
                {
                    if (axis.IsVertical)
                    {
                        drawRect.Height = 1;
                    }
                    else
                    {
                        drawRect.Width = 1;
                    }
                }

                DrawPlotBandRect(canvas, drawRect);

                start += periodBand;
            } while (IsRepeatable && (periodBand != 0) && (repeatEnd > start));
        }

        double GetActualPeriodStrip(double start)
        {
            return double.IsNaN(RepeatEvery) ? double.NaN : GetBandWidth(start, RepeatEvery, RepeatEveryType);
        }

        double GetBandWidth(double start, double size, DateTimeIntervalType type)
        {
            if (type == DateTimeIntervalType.Auto && Axis is DateTimeAxis dateTimeAxis)
            {
                dateTimeAxis.CalculateDateTimeIntervalType(dateTimeAxis.ActualRange, dateTimeAxis.AvailableSize, ref type);
            }

            var date = DateTime.FromOADate(start);

            switch (type)
            {
                case DateTimeIntervalType.Years:
                    date = date.AddYears((int)size);
                    break;
                case DateTimeIntervalType.Months:
                    date = date.AddMonths((int)size);
                    break;
                case DateTimeIntervalType.Days:
                    date = date.AddDays((int)size);
                    break;
                case DateTimeIntervalType.Hours:
                    date = date.AddHours((int)size);
                    break;
                case DateTimeIntervalType.Minutes:
                    date = date.AddMinutes((int)size);
                    break;
                case DateTimeIntervalType.Seconds:
                    date = date.AddSeconds((int)size);
                    break;
                case DateTimeIntervalType.Milliseconds:
                    date = date.AddMilliseconds((int)size);
                    break;
            }

            return date.ToOADate() - start;
        }

        #endregion

        #endregion
    }

    /// <summary>
    /// Represents a collection of <see cref="DateTimePlotBand"/>
    /// </summary>
    public class DateTimePlotBandCollection : ObservableCollection<DateTimePlotBand>
    {

    }
}