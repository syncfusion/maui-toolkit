namespace Syncfusion.Maui.Toolkit.Calendar
{
    /// <summary>
    /// Represents a class which holds the start and end date of the range in SfCalendar.
    /// </summary>
    public class CalendarDateRange : BindableObject
    {
        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="StartDate"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="StartDate"/> dependency property.
        /// </value>
        public static readonly BindableProperty StartDateProperty =
            BindableProperty.Create(
                nameof(StartDate),
                typeof(DateTime?),
                typeof(CalendarDateRange),
                defaultValueCreator: bindable => null,
                propertyChanged: OnRangeStartDatePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="EndDate"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="EndDate"/> dependency property.
        /// </value>
        public static readonly BindableProperty EndDateProperty =
            BindableProperty.Create(
                nameof(EndDate),
                typeof(DateTime?),
                typeof(CalendarDateRange),
                defaultValueCreator: bindable => null,
                propertyChanged: OnRangeEndDatePropertyChanged);


        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarDateRange"/> class.
        /// </summary>
        /// <param name="startDate">The start date of range</param>
        /// <param name="endDate">The end date of range</param>
        public CalendarDateRange(DateTime? startDate, DateTime? endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the start date of the range.
        /// </summary>
        /// <remarks>
        /// This property is used only to define the start date for range and multi range selection.
        /// </remarks>
        public DateTime? StartDate
        {
            get { return (DateTime?)GetValue(StartDateProperty); }
            set { SetValue(StartDateProperty, value); }
        }

        /// <summary>
        /// Gets or sets the end date of the range.
        /// </summary>
        /// <remarks>
        /// This property is used only to define the end date for range and multi range selection.
        /// </remarks>
        public DateTime? EndDate
        {
            get { return (DateTime?)GetValue(EndDateProperty); }
            set { SetValue(EndDateProperty, value); }
        }

        #endregion

        #region Property Changed Methods

        /// <summary>
        /// Invokes calendar start date property changed.
        /// </summary>
        /// <param name="bindable">The calendar date range object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnRangeStartDatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as CalendarDateRange)?.RaisePropertyChanged(nameof(StartDate));
        }

        /// <summary>
        /// Invokes calendar end date property changed.
        /// </summary>
        /// <param name="bindable">The calendar date range object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnRangeEndDatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as CalendarDateRange)?.RaisePropertyChanged(nameof(EndDate));
        }

        /// <summary>
        /// Method to invoke calendar property changed event on calendar date range properties changed.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        void RaisePropertyChanged(string propertyName)
        {
            CalendarPropertyChanged?.Invoke(this, new CalendarPropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Event

        /// <summary>
        /// Invokes on calendar start and end date range property changed and this includes old value of the changed property which is used to wire and unwire events for nested classes.
        /// </summary>
        internal event EventHandler<CalendarPropertyChangedEventArgs>? CalendarPropertyChanged;

        #endregion
    }
}