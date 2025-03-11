using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace Syncfusion.Maui.Toolkit.Calendar
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SfCalendar"/> class that represents a control,
    /// used to display and select one or more dates with in specified date range.
    /// </summary>
#pragma warning disable CA1506
    public partial class SfCalendar
    {
        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="View"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="View"/> dependency property.
        /// </value>
        public static readonly BindableProperty ViewProperty =
            BindableProperty.Create(
                nameof(View),
                typeof(CalendarView),
                typeof(SfCalendar),
                CalendarView.Month,
                propertyChanged: OnViewChanged);

        /// <summary>
        /// Identifies the <see cref="HeaderView"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="HeaderView"/> dependency property.
        /// </value>
        public static readonly BindableProperty HeaderViewProperty =
            BindableProperty.Create(
                nameof(HeaderView),
                typeof(CalendarHeaderView),
                typeof(SfCalendar),
                defaultValueCreator: bindable => new CalendarHeaderView()
                {
                    Parent = bindable as Element
                },
                propertyChanged: OnHeaderViewChanged);

        /// <summary>
        /// Identifies the <see cref="FooterView"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="FooterView"/> dependency property.
        /// </value>
        public static readonly BindableProperty FooterViewProperty =
            BindableProperty.Create(
                nameof(FooterView),
                typeof(CalendarFooterView),
                typeof(SfCalendar),
                defaultValueCreator: bindable => new CalendarFooterView()
                {
                    Parent = bindable as Element
                },
                propertyChanged: OnFooterViewChanged);

        /// <summary>
        /// Identifies the <see cref="NavigationDirection"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="NavigationDirection"/> dependency property.
        /// </value>
        public static readonly BindableProperty NavigationDirectionProperty =
            BindableProperty.Create(
                nameof(NavigationDirection),
                typeof(CalendarNavigationDirection),
                typeof(SfCalendar),
                CalendarNavigationDirection.Vertical,
                propertyChanged: OnNavigationDirectionChanged);

        /// <summary>
        /// Identifies the <see cref="DisplayDate"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="DisplayDate"/> dependency property.
        /// </value>
        public static readonly BindableProperty DisplayDateProperty =
            BindableProperty.Create(
                nameof(DisplayDate),
                typeof(DateTime),
                typeof(SfCalendar),
                DateTime.Now,
                propertyChanged: OnDisplayDateChanged);

        /// <summary>
        /// Identifies the <see cref="MinimumDate"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="MinimumDate"/> dependency property.
        /// </value>
        public static readonly BindableProperty MinimumDateProperty =
            BindableProperty.Create(
                nameof(MinimumDate),
                typeof(DateTime),
                typeof(SfCalendar),
                DateTime.MinValue,
                propertyChanged: OnMinMaxDateChanged);

        /// <summary>
        /// Identifies the <see cref="MaximumDate"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="MaximumDate"/> dependency property.
        /// </value>
        public static readonly BindableProperty MaximumDateProperty =
            BindableProperty.Create(
                nameof(MaximumDate),
                typeof(DateTime),
                typeof(SfCalendar),
                DateTime.MaxValue,
                propertyChanged: OnMinMaxDateChanged);

        /// <summary>
        /// Identifies the <see cref="EnablePastDates"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="EnablePastDates"/> dependency property.
        /// </value>
        public static readonly BindableProperty EnablePastDatesProperty =
            BindableProperty.Create(
                nameof(EnablePastDates),
                typeof(bool),
                typeof(SfCalendar),
                true,
                propertyChanged: OnEnablePastDatesChanged);

        /// <summary>
        /// Identifies the <see cref="MonthView"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="MonthView"/> dependency property.
        /// </value>
        public static readonly BindableProperty MonthViewProperty =
            BindableProperty.Create(
                nameof(MonthView),
                typeof(CalendarMonthView),
                typeof(SfCalendar),
                defaultValueCreator: bindable => new CalendarMonthView()
                {
                    Parent = bindable as Element
                },
                propertyChanged: OnMonthViewSettingsChanged);

        /// <summary>
        /// Identifies the <see cref="SelectionShape"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="SelectionShape"/> dependency property.
        /// </value>
        public static readonly BindableProperty SelectionShapeProperty =
            BindableProperty.Create(
                nameof(SelectionShape),
                typeof(CalendarSelectionShape),
                typeof(SfCalendar),
                CalendarSelectionShape.Circle,
                propertyChanged: OnSelectionShapeChanged);

        /// <summary>
        /// Identifies the <see cref="EnableSwipeSelection"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="EnableSwipeSelection"/> dependency property.
        /// </value>
        public static readonly BindableProperty EnableSwipeSelectionProperty =
            BindableProperty.Create(
                nameof(EnableSwipeSelection),
                typeof(bool),
                typeof(SfCalendar),
                false,
                propertyChanged: OnEnableSwipeSelectionChanged);

        /// <summary>
        /// Identifies the <see cref="YearView"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="YearView"/> dependency property.
        /// </value>
        public static readonly BindableProperty YearViewProperty =
            BindableProperty.Create(
                nameof(YearView),
                typeof(CalendarYearView),
                typeof(SfCalendar),
                defaultValueCreator: bindable => new CalendarYearView()
                {
                    Parent = bindable as Element
                },
                propertyChanged: OnYearViewChanged);

        /// <summary>
        /// Identifies the <see cref="SelectedDate"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="SelectedDate"/> dependency property.
        /// </value>
        public static readonly BindableProperty SelectedDateProperty =
            BindableProperty.Create(
                nameof(SelectedDate),
                typeof(DateTime?),
                typeof(SfCalendar),
                null,
                propertyChanged: OnSelectedDateChanged);

        /// <summary>
        /// Identifies the <see cref="SelectedDates"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="SelectedDates"/> dependency property.
        /// </value>
        public static readonly BindableProperty SelectedDatesProperty =
            BindableProperty.Create(
                nameof(SelectedDates),
                typeof(ObservableCollection<DateTime>),
                typeof(SfCalendar),
                new ObservableCollection<DateTime>(),
                propertyChanged: OnSelectedDatesChanged);

        /// <summary>
        /// Identifies the <see cref="SelectedDateRange"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="SelectedDateRange"/> dependency property.
        /// </value>
        public static readonly BindableProperty SelectedDateRangeProperty =
            BindableProperty.Create(
                nameof(SelectedDateRange),
                typeof(CalendarDateRange),
                typeof(SfCalendar),
                defaultValueCreator: bindable => null,
                propertyChanged: OnSelectedRangeChanged);

        /// <summary>
        /// Identifies the <see cref="SelectedDateRanges"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="SelectedDateRanges"/> dependency property.
        /// </value>
        public static readonly BindableProperty SelectedDateRangesProperty =
            BindableProperty.Create(
                nameof(SelectedDateRanges),
                typeof(ObservableCollection<CalendarDateRange>),
                typeof(SfCalendar),
                defaultValueCreator: bindable => new ObservableCollection<CalendarDateRange>(),
                propertyChanged: OnSelectedDateRangesChanged);

        /// <summary>
        /// Identifies the <see cref="SelectionMode"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="SelectionMode"/> dependency property.
        /// </value>
        public static readonly BindableProperty SelectionModeProperty =
            BindableProperty.Create(
                nameof(SelectionMode),
                typeof(CalendarSelectionMode),
                typeof(SfCalendar),
                CalendarSelectionMode.Single,
                propertyChanged: OnSelectionModeChanged);

        /// <summary>
        /// Identifies the <see cref="CanToggleDaySelection"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="CanToggleDaySelection"/> dependency property.
        /// </value>
        public static readonly BindableProperty CanToggleDaySelectionProperty =
            BindableProperty.Create(
                nameof(CanToggleDaySelection),
                typeof(bool),
                typeof(SfCalendar),
                false,
                propertyChanged: OnToggleDaySelectionChanged);

        /// <summary>
        /// Identifies the <see cref="AllowViewNavigation"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="AllowViewNavigation"/> dependency property.
        /// </value>
        public static readonly BindableProperty AllowViewNavigationProperty =
            BindableProperty.Create(
                nameof(AllowViewNavigation),
                typeof(bool),
                typeof(SfCalendar),
                true,
                propertyChanged: OnAllowViewNavigationChanged);

        /// <summary>
        /// Identifies the <see cref="TodayHighlightBrush"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="TodayHighlightBrush"/> dependency property.
        /// </value>
        public static readonly BindableProperty TodayHighlightBrushProperty =
            BindableProperty.Create(
                nameof(TodayHighlightBrush),
                typeof(Brush),
                typeof(SfCalendar),
                defaultValueCreator: bindable => CalendarColors.GetPrimaryBrush(),
                propertyChanged: OnTodayHighlightBrushChanged);

        /// <summary>
        /// Identifies the <see cref="SelectionBackground"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="SelectionBackground"/> dependency property.
        /// </value>
        public static readonly BindableProperty SelectionBackgroundProperty =
            BindableProperty.Create(
                nameof(SelectionBackground),
                typeof(Brush),
                typeof(SfCalendar),
                defaultValueCreator: bindable => null,
                propertyChanged: OnSelectionBackgroundChanged);

        /// <summary>
        /// Identifies the <see cref="StartRangeSelectionBackground"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="StartRangeSelectionBackground"/> dependency property.
        /// </value>
        public static readonly BindableProperty StartRangeSelectionBackgroundProperty =
            BindableProperty.Create(
                nameof(StartRangeSelectionBackground),
                typeof(Brush),
                typeof(SfCalendar),
                defaultValueCreator: bindable => CalendarColors.GetPrimaryBrush(),
                propertyChanged: OnStartRangeSelectionBackgroundChanged);

        /// <summary>
        /// Identifies the <see cref="EndRangeSelectionBackground"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="EndRangeSelectionBackground"/> dependency property.
        /// </value>
        public static readonly BindableProperty EndRangeSelectionBackgroundProperty =
            BindableProperty.Create(
                nameof(EndRangeSelectionBackground),
                typeof(Brush),
                typeof(SfCalendar),
                defaultValueCreator: bindable => CalendarColors.GetPrimaryBrush(),
                propertyChanged: OnEndRangeSelectionBackgroundChanged);

        /// <summary>
        /// Identifies the <see cref="ShowTrailingAndLeadingDates"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="ShowTrailingAndLeadingDates"/> dependency property.
        /// </value>
        public static readonly BindableProperty ShowTrailingAndLeadingDatesProperty =
            BindableProperty.Create(
                nameof(ShowTrailingAndLeadingDates),
                typeof(bool),
                typeof(SfCalendar),
                true,
                propertyChanged: OnShowLeadingAndTrailingDatesChanged);

        /// <summary>
        /// Identifies the <see cref="SelectableDayPredicate"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="SelectableDayPredicate"/> dependency property.
        /// </value>
        public static readonly BindableProperty SelectableDayPredicateProperty =
            BindableProperty.Create(
                nameof(SelectableDayPredicate),
                typeof(Func<DateTime, bool>),
                typeof(SfCalendar),
                null,
                propertyChanged: OnSelectableDayPredicateChanged);

        /// <summary>
        /// Identifies the <see cref="RangeSelectionDirection"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="RangeSelectionDirection"/> dependency property.
        /// </value>
        public static readonly BindableProperty RangeSelectionDirectionProperty =
            BindableProperty.Create(
                nameof(RangeSelectionDirection),
                typeof(CalendarRangeSelectionDirection),
                typeof(SfCalendar),
                CalendarRangeSelectionDirection.Default,
                propertyChanged: OnRangeSelectionDirectionChanged);

        /// <summary>
        /// Identifies the <see cref="Identifier"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Identifier"/> dependency property.
        /// </value>
        public static readonly BindableProperty IdentifierProperty =
            BindableProperty.Create(
                nameof(Identifier),
                typeof(CalendarIdentifier),
                typeof(SfCalendar),
                CalendarIdentifier.Gregorian,
                propertyChanged: OnCalendarIdentifierChanged);

        /// <summary>
        /// Identifies the <see cref="ShowOutOfRangeDates"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="ShowOutOfRangeDates"/> dependency property.
        /// </value>
        public static readonly BindableProperty ShowOutOfRangeDatesProperty =
            BindableProperty.Create(
                nameof(ShowOutOfRangeDates),
                typeof(bool),
                typeof(SfCalendar),
                true,
                propertyChanged: OnShowOutOfRangeDatesChanged);

        /// <summary>
        /// Identifies the <see cref="CornerRadius"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="CornerRadius"/> bindable property.
        /// </value>
        public static readonly BindableProperty CornerRadiusProperty =
            BindableProperty.Create(
                nameof(CornerRadius),
                typeof(CornerRadius),
                typeof(SfCalendar),
                new CornerRadius(20),
                propertyChanged: OnCornerRadiusChanged);

        /// <summary>
        /// Identifies the <see cref="NavigateToAdjacentMonth"/> dependency property
        /// </summary>
        /// <value>
        /// The identifier for <see cref="NavigateToAdjacentMonth"/> dependency property
        /// </value>
        public static readonly BindableProperty NavigateToAdjacentMonthProperty =
            BindableProperty.Create(
                nameof(NavigateToAdjacentMonth),
                typeof(bool),
                typeof(SfCalendar),
                defaultValueCreator: bindable => true);

        /// <summary>
        /// Identifies the <see cref="SelectionChangedCommand"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="SelectionChangedCommand"/> bindable property.
        /// </value>
        public static readonly BindableProperty SelectionChangedCommandProperty =
            BindableProperty.Create(
                nameof(SelectionChangedCommand),
                typeof(ICommand),
                typeof(SfCalendar),
                null);

        /// <summary>
        /// Identifies the <see cref="ViewChangedCommand"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="ViewChangedCommand"/> bindable property.
        /// </value>
        public static readonly BindableProperty ViewChangedCommandProperty =
            BindableProperty.Create(
                nameof(ViewChangedCommand),
                typeof(ICommand),
                typeof(SfCalendar),
                null);

        /// <summary>
        /// Identifies the <see cref="TappedCommand"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="TappedCommand"/> bindable property.
        /// </value>
        public static readonly BindableProperty TappedCommandProperty =
            BindableProperty.Create(
                nameof(TappedCommand),
                typeof(ICommand),
                typeof(SfCalendar),
                null);

        /// <summary>
        /// Identifies the <see cref="DoubleTappedCommand"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="DoubleTappedCommand"/> bindable property.
        /// </value>
        public static readonly BindableProperty DoubleTappedCommandProperty =
            BindableProperty.Create(
                nameof(DoubleTappedCommand),
                typeof(ICommand),
                typeof(SfCalendar),
                null);

        /// <summary>
        /// Identifies the <see cref="LongPressedCommand"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="LongPressedCommand"/> bindable property.
        /// </value>
        public static readonly BindableProperty LongPressedCommandProperty =
            BindableProperty.Create(
                nameof(LongPressedCommand),
                typeof(ICommand),
                typeof(SfCalendar),
                null);

        /// <summary>
        /// Identifies the <see cref="AcceptCommand"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="AcceptCommand"/> bindable property.
        /// </value>
        public static readonly BindableProperty AcceptCommandProperty =
            BindableProperty.Create(
                nameof(AcceptCommand),
                typeof(ICommand),
                typeof(SfCalendar),
                null);

        /// <summary>
        /// Identifies the <see cref="DeclineCommand"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="DeclineCommand"/> bindable property.
        /// </value>
        public static readonly BindableProperty DeclineCommandProperty =
            BindableProperty.Create(
                nameof(DeclineCommand),
                typeof(ICommand),
                typeof(SfCalendar),
                null);

        /// <summary>
        /// Identifies the <see cref="HeaderTemplate"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="HeaderTemplate"/> dependency property.
        /// </value>
        public static readonly BindableProperty HeaderTemplateProperty =
            BindableProperty.Create(
                nameof(HeaderTemplate),
                typeof(DataTemplate),
                typeof(SfCalendar),
                defaultValueCreator: bindable => null,
                propertyChanged: OnHeaderTemplateChanged);

        /// <summary>
        /// Identifies the <see cref="MonthViewHeaderTemplate"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="MonthViewHeaderTemplate"/> dependency property.
        /// </value>
        public static readonly BindableProperty MonthViewHeaderTemplateProperty =
            BindableProperty.Create(
                nameof(MonthViewHeaderTemplate),
                typeof(DataTemplate),
                typeof(SfCalendar),
                defaultValueCreator: bindable => null,
                propertyChanged: OnMonthViewHeaderTemplateChanged);

        /// <summary>
        /// Identifies the <see cref="IsOpen"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="IsOpen"/> dependency property.
        /// </value>
        public static readonly BindableProperty IsOpenProperty =
            BindableProperty.Create(
                nameof(IsOpen),
                typeof(bool),
                typeof(SfCalendar),
                false,
                propertyChanged: OnIsOpenPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="Mode"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Mode"/> dependency property.
        /// </value>
        public static readonly BindableProperty ModeProperty =
            BindableProperty.Create(
                nameof(Mode),
                typeof(CalendarMode),
                typeof(SfCalendar),
                CalendarMode.Default,
                propertyChanged: OnModeChanged);

        /// <summary>
        /// Identifies the <see cref="RelativePosition"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="RelativePosition"/> dependency property.
        /// </value>
        public static readonly BindableProperty RelativePositionProperty =
            BindableProperty.Create(
                nameof(RelativePosition),
                typeof(CalendarRelativePosition),
                typeof(SfCalendar),
                CalendarRelativePosition.AlignTop,
                propertyChanged: OnRelativePositionChanged);

#if WINDOWS
        /// <summary>
        /// Identifies the <see cref="FlowDirectionProperty"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="FlowDirectionProperty"/> dependency property.
        /// </value>
        public static readonly new BindableProperty FlowDirectionProperty =
            BindableProperty.Create(
                nameof(FlowDirection),
                typeof(FlowDirection),
                typeof(SfCalendar),
                FlowDirection.LeftToRight,
                propertyChanged: OnFlowDirectionChanged);
#endif

        #endregion

        #region Internal Bindable Properties

        /// <summary>
        /// Identifies the <see cref="ButtonTextColor"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="ButtonTextColor"/> dependency property.
        /// </value>
        internal static readonly BindableProperty ButtonTextColorProperty =
            BindableProperty.Create(
                nameof(ButtonTextColor),
                typeof(Brush),
                typeof(SfCalendar),
                defaultValueCreator: bindable => null,
                propertyChanged: OnTodayHighlightBrushChanged);

        /// <summary>
        /// Identifies the <see cref="HeaderHoverTextColor"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="HeaderHoverTextColor"/> dependency property.
        /// </value>
        internal static readonly BindableProperty HeaderHoverTextColorProperty =
            BindableProperty.Create(
                nameof(HeaderHoverTextColor),
                typeof(Brush),
                typeof(SfCalendar),
                defaultValueCreator: bindable => null,
                propertyChanged: OnTodayHighlightBrushChanged);

        /// <summary>
        /// Identifies the <see cref="RangeSelectionColor"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="RangeSelectionColor"/> dependency property.
        /// </value>
        internal static readonly BindableProperty RangeSelectionColorProperty =
            BindableProperty.Create(
                nameof(RangeSelectionColor),
                typeof(Brush),
                typeof(SfCalendar),
                defaultValueCreator: bindable => null,
                propertyChanged: OnSelectionBackgroundChanged);

        /// <summary>
        /// Identifies the <see cref="HoverTextColor"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="HoverTextColor"/> dependency property.
        /// </value>
        internal static readonly BindableProperty HoverTextColorProperty =
            BindableProperty.Create(
                nameof(HoverTextColor),
                typeof(Brush),
                typeof(SfCalendar),
                defaultValueCreator: bindable => null,
                propertyChanged: OnSelectionBackgroundChanged);

        /// <summary>
        /// Identifies the <see cref="CalendarBackground"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="CalendarBackground"/> bindable property.
        /// </value>
        internal static readonly BindableProperty CalendarBackgroundProperty =
            BindableProperty.Create(
                nameof(CalendarBackground),
                typeof(Color),
                typeof(SfCalendar),
                defaultValueCreator: bindable => Color.FromArgb("#EEE8F4"),
                propertyChanged: OnCalendarBackgroundChanged);

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the function to decide whether the cell is selectable or not in the calendar.
        /// </summary>
        /// <remarks>
        /// It will be applicable to all <see cref="SfCalendar.View"/>.
        /// </remarks>
        /// <seealso cref="CalendarMonthView.DisabledDatesTextStyle"/>
        /// <seealso cref="CalendarMonthView.DisabledDatesBackground"/>
        /// <seealso cref="CalendarYearView.DisabledDatesTextStyle"/>
        /// <seealso cref="CalendarYearView.DisabledDatesBackground"/>
        /// <example>
        /// The following code demonstrates, how to use the SelectableDayPredicate function.
        /// <code Lang="C#"><![CDATA[
        /// Calendar.SelectableDayPredicate = (date) =>
        /// {
        ///    if (date.DayOfWeek == DayOfWeek.Sunday || date.DayOfWeek == DayOfWeek.Saturday)
        ///    {
        ///        return false;
        ///    }
        ///    return true;
        ///  };
        /// ]]></code>
        /// </example>
        public Func<DateTime, bool> SelectableDayPredicate
        {
            get { return (Func<DateTime, bool>)GetValue(SelectableDayPredicateProperty); }
            set { SetValue(SelectableDayPredicateProperty, value); }
        }

        /// <summary>
        /// Gets or sets the built-in views such as month, year, decade and century of the SfCalendar.
        /// </summary>
        /// <value>The default value of <see cref="SfCalendar.View"/> is <see cref="CalendarView.Month"/>.</value>
        /// <seealso cref="SfCalendar.AllowViewNavigation"/>
        /// <seealso cref="SfCalendar.ViewChanged"/>
        /// <example>
        /// The following code demonstrates, how to use the View property in the calendar
        /// #[XAML](#tab/tabid-1)
        /// <code Lang="XAML"><![CDATA[
        /// <Calendar:SfCalendar x:Name="Calendar"
        ///                      View="Month">
        /// </Calendar:SfCalendar>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code Lang="C#"><![CDATA[
        /// Calendar.View = CalendarView.Month;
        /// ]]></code>
        /// </example>
        public CalendarView View
        {
            get { return (CalendarView)GetValue(ViewProperty); }
            set { SetValue(ViewProperty, value); }
        }

        /// <summary>
        /// Gets or sets the properties which allows to customize the calendar header of month, year, decade and century views.
        /// </summary>
        /// <remarks>
        /// It will be applicable to all <see cref="SfCalendar.View"/>.
        /// </remarks>
        /// <example>
        /// The following code demonstrates, how to use the HeaderView property in the calendar
        /// <code Lang="C#"><![CDATA[
        /// Calendar.View = CalendarView.Month;
        /// var calendarTextStyle = new CalendarTextStyle()
        /// {
        ///     TextColor = Colors.Black,
        /// };
        /// var calendarHeaderView = new CalendarHeaderView()
        /// {
        ///     Height = 100,
        ///     Background = Colors.Blue,
        ///     TextFormat = "dd-mmm-yyyy",
        ///     TextStyle = calendarTextStyle,
        ///     ShowNavigationArrows = true,
        /// };
        /// Calendar.HeaderView = calendarHeaderView;
        /// ]]></code>
        /// </example>
        public CalendarHeaderView HeaderView
        {
            get { return (CalendarHeaderView)GetValue(HeaderViewProperty); }
            set { SetValue(HeaderViewProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value which determines the direction of the calendar scrolls.
        /// </summary>
        /// <value>The default value of <see cref="SfCalendar.NavigationDirection"/> is Vertical.</value>
        /// <seealso cref="CalendarHeaderView.ShowNavigationArrows"/>
        /// <example>
        /// The following code demonstrates, how to use the NavigationDirection property in the calendar
        /// #[XAML](#tab/tabid-3)
        /// <code Lang="XAML"><![CDATA[
        /// <Calendar:SfCalendar x:Name="Calendar"
        ///                      NavigationDirection="Vertical">
        /// </Calendar:SfCalendar>
        /// ]]></code>
        /// # [C#](#tab/tabid-4)
        /// <code Lang="C#"><![CDATA[
        /// Calendar.NavigationDirection = CalendarNavigationDirection.Vertical;
        /// ]]></code>
        /// </example>
        public CalendarNavigationDirection NavigationDirection
        {
            get { return (CalendarNavigationDirection)GetValue(NavigationDirectionProperty); }
            set { SetValue(NavigationDirectionProperty, value); }
        }

        /// <summary>
        /// Gets or sets the display date to programmatically navigate the dates in the SfCalendar.
        /// </summary>
        /// <value>The default value of <see cref="SfCalendar.DisplayDate"/> is <see cref="DateTime.Now"/>.</value>
        /// <remarks>
        /// The date navigation before the <see cref="SfCalendar.MinimumDate"/> will be reset to the calendar minimum date value and
        /// date navigation beyond the <see cref="SfCalendar.MaximumDate"/> will be reset to the calendar maximum date value.
        /// </remarks>
        /// <seealso cref="SfCalendar.ViewChanged"/>
        /// <seealso cref="SfCalendar.MinimumDate"/>
        /// <seealso cref="SfCalendar.MaximumDate"/>
        /// <seealso cref="SfCalendar.Forward"/>
        /// <seealso cref="SfCalendar.Backward"/>
        /// <example>
        /// The following code demonstrates, how to use the DisplayDate property in the calendar
        /// #[XAML](#tab/tabid-5)
        /// <code Lang="XAML"><![CDATA[
        /// <Calendar:SfCalendar x:Name="Calendar"
        ///                      View="Month"
        ///                      DisplayDate="2022/11/29">
        /// </Calendar:SfCalendar>
        /// ]]></code>
        /// # [C#](#tab/tabid-6)
        /// <code Lang="C#"><![CDATA[
        /// Calendar.DisplayDate = new DateTime(2022, 11, 29);
        /// ]]></code>
        /// </example>
        public DateTime DisplayDate
        {
            get { return (DateTime)GetValue(DisplayDateProperty); }
            set { SetValue(DisplayDateProperty, value); }
        }

        /// <summary>
        /// Gets or sets the minimum display date to restrict the visible dates in the SfCalendar.
        /// </summary>
        /// <value>The default value of <see cref="SfCalendar.MinimumDate"/> is <see cref="DateTime.MinValue"/>.</value>
        /// <remarks>
        /// The date before the minimum date will be disabled date. The navigation before the minimum date using <see cref ="SfCalendar.DisplayDate"/> is not possible.
        /// The backward navigation arrow will be disabled while the view reaches the minimum date.
        /// </remarks>
        /// <seealso cref="SfCalendar.DisplayDate"/>
        /// <seealso cref="SfCalendar.Forward"/>
        /// <seealso cref="SfCalendar.Backward"/>
        /// <seealso cref="SfCalendar.SelectedDate"/>
        /// <seealso cref="SfCalendar.SelectedDates"/>
        /// <seealso cref="SfCalendar.SelectedDateRange"/>
        /// <example>
        /// The following code demonstrates, how to use the MinimumDate property in the calendar
        /// #[XAML](#tab/tabid-7)
        /// <code Lang="XAML"><![CDATA[
        /// <Calendar:SfCalendar x:Name="Calendar"
        ///                      MinimumDate="2022/11/24">
        /// </Calendar:SfCalendar>
        /// ]]></code>
        /// # [C#](#tab/tabid-8)
        /// <code Lang="C#"><![CDATA[
        /// Calendar.MinimumDate = new DateTime(2022, 11, 24);
        /// ]]></code>
        /// </example>
        public DateTime MinimumDate
        {
            get { return (DateTime)GetValue(MinimumDateProperty); }
            set { SetValue(MinimumDateProperty, value); }
        }

        /// <summary>
        /// Gets or sets the maximum display date to restrict the visible dates in the SfCalendar.
        /// </summary>
        /// <value>The default value of <see cref="SfCalendar.MaximumDate"/> is <see cref="DateTime.MaxValue"/>.</value>
        /// <remarks>
        /// The date after the maximum date will be disabled date. The navigation after the maximum date using <see cref ="SfCalendar.DisplayDate"/> is not possible.
        /// The forward navigation arrow will be disabled while the view reaches the maximum date.
        /// </remarks>
        /// <seealso cref="SfCalendar.DisplayDate"/>
        /// <seealso cref="SfCalendar.Forward"/>
        /// <seealso cref="SfCalendar.Backward"/>
        /// <seealso cref="SfCalendar.SelectedDate"/>
        /// <seealso cref="SfCalendar.SelectedDates"/>
        /// <seealso cref="SfCalendar.SelectedDateRange"/>
        /// <example>
        /// The following code demonstrates, how to use the MaximumDate property in the calendar
        /// #[XAML](#tab/tabid-9)
        /// <code Lang="XAML"><![CDATA[
        /// <Calendar:SfCalendar x:Name="Calendar"
        ///                      MaximumDate="2022/12/25">
        /// </Calendar:SfCalendar>
        /// ]]></code>
        /// # [C#](#tab/tabid-10)
        /// <code Lang="C#"><![CDATA[
        /// Calendar.MaximumDate = new DateTime(2022, 12, 25);
        /// ]]></code>
        /// </example>
        public DateTime MaximumDate
        {
            get { return (DateTime)GetValue(MaximumDateProperty); }
            set { SetValue(MaximumDateProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the dates enabled or disabled before today date.
        /// </summary>
        /// <value>The default value of <see cref="SfCalendar.EnablePastDates"/> is true. </value>
        /// <remarks>
        /// If the value is set to false, we cannot select the dates before the today date.
        /// </remarks>
        /// <seealso cref="SfCalendar.SelectedDate"/>
        /// <seealso cref="SfCalendar.SelectedDates"/>
        /// <seealso cref="SfCalendar.SelectedDateRange"/>
        /// <example>
        /// The following code demonstrates, how to use the EnablePastDates property in the calendar
        /// #[XAML](#tab/tabid-11)
        /// <code Lang="XAML"><![CDATA[
        /// <Calendar:SfCalendar x:Name="Calendar"
        ///                      EnablePastDates="False">
        /// </Calendar:SfCalendar>
        /// ]]></code>
        /// # [C#](#tab/tabid-12)
        /// <code Lang="C#"><![CDATA[
        /// Calendar.EnablePastDates = false;
        /// ]]></code>
        /// </example>
        public bool EnablePastDates
        {
            get { return (bool)GetValue(EnablePastDatesProperty); }
            set { SetValue(EnablePastDatesProperty, value); }
        }

        /// <summary>
        /// Gets or sets the properties which allows to customize the calendar month view.
        /// </summary>
        /// <example>
        /// The following code demonstrates, how to use the Month view property in the calendar
        /// #[XAML](#tab/tabid-13)
        /// <code Lang="XAML"><![CDATA[
        /// <Calendar:SfCalendar x:Name="Calendar"
        /// View="Month">
        /// <Calendar:SfCalendar.MonthView>
        ///     <Calendar:CalendarMonthView NumberOfVisibleWeeks = "2"
        ///                             FirstDayOfWeek="Wednesday"
        ///                             ShowWeekNumber="True"
        ///                             Background="Blue" />
        ///     </Calendar:SfCalendar.MonthView>
        /// </Calendar:SfCalendar>
        /// ]]></code>
        /// # [C#](#tab/tabid-14)
        /// <code Lang="C#"><![CDATA[
        /// Calendar.View = CalendarView.Month;
        /// Calendar.MonthView = new CalendarMonthView()
        /// {
        /// 	NumberOfVisibleWeeks = 2,
        ///     FirstDayOfWeek = DayOfWeek.Wednesday,
        ///     ShowWeekNumber = true,
        ///     Background = Colors.Blue
        /// };
        /// ]]></code>
        /// </example>
        public CalendarMonthView MonthView
        {
            get { return (CalendarMonthView)GetValue(MonthViewProperty); }
            set { SetValue(MonthViewProperty, value); }
        }

        /// <summary>
        /// Gets or sets the selection shape of the SfCalendar.
        /// </summary>
        /// <value>The default value of <see cref="SfCalendar.SelectionShape"/> is circle. </value>
        /// <example>
        /// The following code demonstrates, how to use the SelectionShape property in the calendar
        /// #[XAML](#tab/tabid-15)
        /// <code Lang="XAML"><![CDATA[
        /// <Calendar:SfCalendar x:Name="Calendar"
        ///                      SelectionShape="Circle">
        /// </Calendar:SfCalendar>
        /// ]]></code>
        /// # [C#](#tab/tabid-16)
        /// <code Lang="C#"><![CDATA[
        /// Calendar.SelectionShape = CalendarSelectionShape.Circle;
        /// ]]></code>
        /// </example>
        public CalendarSelectionShape SelectionShape
        {
            get { return (CalendarSelectionShape)GetValue(SelectionShapeProperty); }
            set { SetValue(SelectionShapeProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the swiping selection enabled for select the date range.
        /// </summary>
        /// <value> The default value of <seealso cref="SfCalendar.EnableSwipeSelection"/> is false. </value>
        /// <remarks>
        /// This property is only applicable, when the <see cref="SfCalendar.SelectionMode"/> is <see cref="CalendarSelectionMode.Range"/>.
        /// </remarks>
        /// <seealso cref="SfCalendar.SelectionMode"/>
        /// <seealso cref="SfCalendar.AllowViewNavigation"/>
        /// <example>
        /// The following code demonstrates, how to use the EnableSwipeSelection property in the calendar
        /// #[XAML](#tab/tabid-17)
        /// <code Lang="XAML"><![CDATA[
        /// <Calendar:SfCalendar x:Name="Calendar"
        ///                      EnableSwipeSelection="True">
        /// </Calendar:SfCalendar>
        /// ]]></code>
        /// # [C#](#tab/tabid-18)
        /// <code Lang="C#"><![CDATA[
        /// Calendar.EnableSwipeSelection = true;
        /// ]]></code>
        /// </example>
        public bool EnableSwipeSelection
        {
            get { return (bool)GetValue(EnableSwipeSelectionProperty); }
            set { SetValue(EnableSwipeSelectionProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the allow navigation by tapping adjacent month dates.
        /// </summary>
        /// <value> The default value of <seealso cref="SfCalendar.NavigateToAdjacentMonth"/> is true. </value>
        /// <remarks>
        /// This property is only applicable, on the <see cref="SfCalendar.MonthView"/>.
        /// </remarks>
        /// <example>
        /// The following code demonstrates, how to use the NavigateToAdjacentMonth property in the calendar
        /// #[XAML](#tab/tabid-17)
        /// <code Lang="XAML"><![CDATA[
        /// <Calendar:SfCalendar x:Name="Calendar"
        ///                      NavigateToAdjacentMonth="True">
        /// </Calendar:SfCalendar>
        /// ]]></code>
        /// </example>
        public bool NavigateToAdjacentMonth
        {
            get { return (bool)GetValue(NavigateToAdjacentMonthProperty); }
            set { SetValue(NavigateToAdjacentMonthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the properties which allows to customize the calendar year, decade and century views.
        /// </summary>
        /// <example>
        /// The following code demonstrates, how to use the YearView property in the calendar
        /// #[XAML](#tab/tabid-19)
        /// <code Lang="XAML"><![CDATA[
        /// <Calendar:SfCalendar x:Name="Calendar"
        ///                      View="Year">
        ///     <Calendar:SfCalendar.YearView>
        ///         <Calendar:CalendarYearView MonthFormat = "MMM"
        ///                                Background="Blue"
        ///                                TodayBackground="Red">
        ///         </Calendar:CalendarYearView>
        ///     </Calendar:SfCalendar.YearView>
        /// </Calendar:SfCalendar>
        /// ]]></code>
        /// # [C#](#tab/tabid-20)
        /// <code Lang="C#"><![CDATA[
        /// Calendar.View = CalendarView.Year;
        /// Calendar.YearView = new CalendarYearView()
        /// {
        ///     MonthFormat = "MMM",
        ///     Background = Colors.Blue,
        ///     TodayBackground = Colors.Red
        /// };
        /// ]]></code>
        /// </example>
        public CalendarYearView YearView
        {
            get { return (CalendarYearView)GetValue(YearViewProperty); }
            set { SetValue(YearViewProperty, value); }
        }

        /// <summary>
        /// Gets or sets the selected date to select the particular date of the calendar.
        /// </summary>
        /// <value> The default value of <see cref="SfCalendar.SelectedDate"/> is null. </value>
        /// <remarks>
        /// This property is only applicable, when the <see cref="SfCalendar.SelectionMode"/> is <see cref="CalendarSelectionMode.Single"/>.
        /// In year, decade, and century views, can select the date only when the allow view navigation is false.
        /// </remarks>
        /// <seealso cref="SfCalendar.SelectionMode"/>
        /// <seealso cref="SfCalendar.AllowViewNavigation"/>
        /// <seealso cref="SfCalendar.SelectionChanged"/>
        /// <seealso cref="SfCalendar.SelectionBackground"/>
        /// <seealso cref="CalendarMonthView.SelectionTextStyle"/>
        /// <seealso cref="CalendarYearView.SelectionTextStyle"/>
        /// <example>
        /// The following code demonstrates, how to use the SelectedDate property in the calendar
        /// #[XAML](#tab/tabid-21)
        /// <code Lang="XAML"><![CDATA[
        /// <Calendar:SfCalendar x:Name="Calendar"
        ///                      View="Month"
        ///                      SelectedDate="2022/11/25">
        /// </Calendar:SfCalendar>
        /// ]]></code>
        /// # [C#](#tab/tabid-22)
        /// <code Lang="C#"><![CDATA[
        /// Calendar.SelectedDate = new DateTime(2022, 11, 25);
        /// ]]></code>
        /// </example>
        public DateTime? SelectedDate
        {
            get { return (DateTime?)GetValue(SelectedDateProperty); }
            set { SetValue(SelectedDateProperty, value); }
        }

        /// <summary>
        /// Gets or sets the selected dates to select the multiple dates of the calendar.
        /// </summary>
        /// <remarks>
        /// This property is only applicable, when the <see cref="SfCalendar.SelectionMode"/> is <see cref="CalendarSelectionMode.Multiple"/>.
        /// In year, decade and century views, can select the dates only when the allow view navigation is false.
        /// Selected dates remove the date when the existing selected date is interacted by touch.
        /// If the selected dates have identical dates and the date is interacted,then it will not remove all the identical dates.
        /// It will remove a single date from the selected dates collection,
        /// so the selection highlight will not be removed until all the identical dates are removed.
        /// Example: Consider that there is a collection like [01-01-2022, 01-01-2022].
        /// When the interaction occurred on the same date in the UI, the collection was changed to [01-01-2022] from [01-01-2022, 01-01-2022].
        /// So the collection still has the date after it is interacted.
        /// The date has to be interacted again to remove it from the collection.
        /// </remarks>
        /// <seealso cref="SfCalendar.SelectionMode"/>
        /// <seealso cref="SfCalendar.AllowViewNavigation"/>
        /// <seealso cref="SfCalendar.SelectionChanged"/>
        /// <seealso cref="SfCalendar.SelectionBackground"/>
        /// <seealso cref="CalendarMonthView.SelectionTextStyle"/>
        /// <seealso cref="CalendarYearView.SelectionTextStyle"/>
        /// <example>
        /// The following code demonstrates, how to use the SelectedDates property in the calendar
        /// #[XAML](#tab/tabid-23)
        /// <code Lang="XAML"><![CDATA[
        /// <Calendar:SfCalendar x:Name="Calendar"
        ///                      View="Month">
        /// </Calendar:SfCalendar>
        /// ]]></code>
        /// # [C#](#tab/tabid-24)
        /// <code Lang="C#"><![CDATA[
        /// ObservableCollection<DateTime> dates = new ObservableCollection<DateTime>()
        /// {
        ///     new DateTime(2022, 11, 20),
        ///     new DateTime(2022, 11, 18),
        ///     new DateTime(2022, 10, 25),
        ///     new DateTime(2022, 12, 10),
        /// };
        /// Calendar.SelectedDates = dates;
        /// ]]></code>
        /// </example>
        public ObservableCollection<DateTime> SelectedDates
        {
            get { return (ObservableCollection<DateTime>)GetValue(SelectedDatesProperty); }
            set { SetValue(SelectedDatesProperty, value); }
        }

        /// <summary>
        /// Gets or sets the selected date range to select the range of dates of the calendar.
        /// </summary>
        /// <value> The default value of <see cref="SfCalendar.SelectedDateRange"/> is null. </value>
        /// <remarks>
        /// This property is only applicable, when the <see cref="SfCalendar.SelectionMode"/> is <see cref="CalendarSelectionMode.Range"/>.
        /// In Year, Decade and Century, can select the range of dates only when the allow view navigation is false.
        /// SelectedDateRange value must be valid range
        /// The default value of selected date range is null.
        /// Valid range means End date is before of start date
        /// Invalid range means end date have value while start date is nullExample:(null, 2022/11/9)
        /// </remarks>
        /// <seealso cref="SfCalendar.SelectionMode"/>
        /// <seealso cref="SfCalendar.RangeSelectionDirection"/>
        /// <seealso cref="SfCalendar.AllowViewNavigation"/>
        /// <seealso cref="SfCalendar.EnableSwipeSelection"/>
        /// <seealso cref="SfCalendar.SelectionChanged"/>
        /// <seealso cref="SfCalendar.SelectionBackground"/>
        /// <seealso cref="SfCalendar.StartRangeSelectionBackground"/>
        /// <seealso cref="SfCalendar.EndRangeSelectionBackground"/>
        /// <seealso cref="CalendarMonthView.SelectionTextStyle"/>
        /// <seealso cref="CalendarMonthView.RangeTextStyle"/>
        /// <seealso cref="CalendarYearView.SelectionTextStyle"/>
        /// <seealso cref="CalendarYearView.RangeTextStyle"/>
        /// <example>
        /// The following code demonstrates, how to use the SelectedDateRange property in the calendar
        /// #[XAML](#tab/tabid-25)
        /// <code Lang="XAML"><![CDATA[
        /// <Calendar:SfCalendar x:Name="Calendar"
        ///                      View="Month">
        /// </Calendar:SfCalendar>
        /// ]]></code>
        /// # [C#](#tab/tabid-26)
        /// <code Lang="C#"><![CDATA[
        /// Calendar.SelectedDateRange = new CalendarDateRange(new DateTime(2022, 11, 12), new DateTime(2022, 12, 15));
        /// ]]></code>
        /// </example>
        public CalendarDateRange SelectedDateRange
        {
            get { return (CalendarDateRange)GetValue(SelectedDateRangeProperty); }
            set { SetValue(SelectedDateRangeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the selected multi date ranges to select the multiple range of dates of the calendar.
        /// </summary>
        /// <value> The default value of <see cref="SfCalendar.SelectedDateRanges"/> is null. </value>
        /// <remarks>
        /// This property is only applicable, when the <see cref="SfCalendar.SelectionMode"/> is <see cref="CalendarSelectionMode.MultiRange"/>.
        /// In Year, Decade and Century, can select the multi range of dates only when the allow view navigation is false.
        /// SelectedDateRanges value must be valid range
        /// The default value of selected date ranges is null.
        /// Valid range means End date is before of start date
        /// Invalid range means end date have value while start date is nullExample:(null, 2022/11/9)
        /// </remarks>
        /// <seealso cref="SfCalendar.SelectionMode"/>
        /// <seealso cref="SfCalendar.RangeSelectionDirection"/>
        /// <seealso cref="SfCalendar.AllowViewNavigation"/>
        /// <seealso cref="SfCalendar.EnableSwipeSelection"/>
        /// <seealso cref="SfCalendar.SelectionChanged"/>
        /// <seealso cref="SfCalendar.SelectionBackground"/>
        /// <seealso cref="SfCalendar.StartRangeSelectionBackground"/>
        /// <seealso cref="SfCalendar.EndRangeSelectionBackground"/>
        /// <seealso cref="CalendarMonthView.SelectionTextStyle"/>
        /// <seealso cref="CalendarMonthView.RangeTextStyle"/>
        /// <seealso cref="CalendarYearView.SelectionTextStyle"/>
        /// <seealso cref="CalendarYearView.RangeTextStyle"/>
        /// <example>
        /// The following code demonstrates, how to use the SelectedDateRanges property in the calendar
        /// #[XAML](#tab/tabid-25)
        /// <code Lang="XAML"><![CDATA[
        /// <Calendar:SfCalendar x:Name="Calendar"
        ///                      View="Month">
        /// </Calendar:SfCalendar>
        /// ]]></code>
        /// # [C#](#tab/tabid-26)
        /// <code Lang="C#"><![CDATA[
        /// ObservableCollection<CalendarDateRange> dates = new ObservableCollection<CalendarDateRange>()
        /// {
        ///		new CalendarDateRange(new DateTime(2022, 10, 12), new DateTime(2022, 11, 15)),
        ///		new CalendarDateRange(new DateTime(2022, 11, 20), new DateTime(2022, 12, 15)),
        /// };
        /// Calendar.SelectedDateRanges = dates;
        /// ]]></code>
        /// </example>
        public ObservableCollection<CalendarDateRange> SelectedDateRanges
        {
            get { return (ObservableCollection<CalendarDateRange>)GetValue(SelectedDateRangesProperty); }
            set { SetValue(SelectedDateRangesProperty, value); }
        }

        /// <summary>
        /// Gets or sets the selection mode of the calendar.
        /// </summary>
        /// <value> The default value of <see cref="SfCalendar.SelectionMode"/> is <see cref="SelectionMode.Single"/></value>
        /// <remarks> In year, Decade and century view, the dates can be selected, only when the allow view navigation is false.</remarks>
        /// <seealso cref="SfCalendar.AllowViewNavigation"/>
        /// <seealso cref="SfCalendar.EnableSwipeSelection"/>
        /// <seealso cref="SfCalendar.RangeSelectionDirection"/>
        /// <seealso cref="SfCalendar.SelectionChanged"/>
        /// <example>
        /// The following code demonstrates, how to use the SelectionMode property in the calendar.
        /// #[XAML](#tab/tabid-27)
        /// <code Lang="XAML"><![CDATA[
        /// <Calendar:SfCalendar x:Name="Calendar"
        ///                      SelectionMode="Single">
        /// </Calendar:SfCalendar>
        /// ]]></code>
        /// #[C#](#tab/tabid-28)
        /// <code Lang="C#"><![CDATA[
        /// Calendar.SelectionMode = CalendarSelectionMode.Single;
        /// ]]></code>
        /// </example>
        public CalendarSelectionMode SelectionMode
        {
            get { return (CalendarSelectionMode)GetValue(SelectionModeProperty); }
            set { SetValue(SelectionModeProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the selected date is deselectable through interaction on single selection mode of SfCalendar.
        /// </summary>
        /// <value> The default value of <see cref="SfCalendar.CanToggleDaySelection"/> is false. </value>
        /// <remarks> This property is only applicable only for <see cref="CalendarSelectionMode.Single"/> selection mode.</remarks>
        /// <seealso cref="SfCalendar.AllowViewNavigation"/>
        /// <seealso cref="SfCalendar.SelectionMode"/>
        /// <seealso cref="SfCalendar.SelectionChanged"/>
        /// <example>
        /// The following code demonstrates, how to use the CanToggleDaySelection property in the calendar
        /// #[XAML](#tab/tabid-29)
        /// <code Lang="XAML"><![CDATA[
        /// <Calendar:SfCalendar x:Name="Calendar"
        ///                       CanToggleDaySelection="True">
        /// </Calendar:SfCalendar>
        /// ]]></code>
        /// #[C#](#tab/tabid-30)
        /// <code Lang="C#"><![CDATA[
        /// Calendar.CanToggleDaySelection = true;
        /// ]]></code>
        /// </example>
        public bool CanToggleDaySelection
        {
            get { return (bool)GetValue(CanToggleDaySelectionProperty); }
            set { SetValue(CanToggleDaySelectionProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the navigation enabled on year, decade and century cell interaction.
        /// </summary>
        /// <value>The default value of <see cref="SfCalendar.AllowViewNavigation"/> is true. </value>
        /// <seealso cref="SfCalendar.View"/>
        /// <seealso cref="SfCalendar.SelectedDate"/>
        /// <seealso cref="SfCalendar.SelectedDates"/>
        /// <seealso cref="SfCalendar.SelectedDateRange"/>
        /// <remarks> The selection will be enabled for year, decade and century views, while the property is disabled. </remarks>
        /// <example>
        /// The following code demonstrates, how to use the AllowViewNavigation property in the calendar.
        /// #[XAML](#tab/tabid-31)
        /// <code Lang="XAML"><![CDATA[
        /// <Calendar:SfCalendar x:Name="Calendar"
        ///                      AllowViewNavigation="True">
        /// </Calendar:SfCalendar>
        /// ]]></code>
        /// #[C#](#tab/tabid-32)
        /// <code Lang="C#"><![CDATA[
        /// Calendar.AllowViewNavigation = true;
        /// ]]></code>
        /// </example>
        public bool AllowViewNavigation
        {
            get { return (bool)GetValue(AllowViewNavigationProperty); }
            set { SetValue(AllowViewNavigationProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value that describes the today highlight color value.
        /// </summary>
        /// <value> The default value of <see cref="SfCalendar.TodayHighlightBrush"/> is "#6200EE"(Blue). </value>
        /// <remarks>
        /// This value is applied to month view header text, when the month view dates have a today date, and its value is not transparent.
        /// </remarks>
        /// <example>
        /// The following code demonstrates, how to use the TodayHighlightBrush property in the calendar
        /// #[XAML](#tab/tabid-33)
        /// <code Lang="XAML"><![CDATA[
        /// <Calendar:SfCalendar x:Name="Calendar"
        ///                      Calendar.TodayHighlightBrush = Colors.Blue;">
        /// </Calendar:SfCalendar>
        /// ]]></code>
        /// #[C#](#tab/tabid-34)
        /// <code Lang="C#"><![CDATA[
        /// Calendar.TodayHighlightBrush = Colors.Blue;
        /// ]]></code>
        /// </example>
        public Brush TodayHighlightBrush
        {
            get { return (Brush)GetValue(TodayHighlightBrushProperty); }
            set { SetValue(TodayHighlightBrushProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value that describes the highlight of selection based on selection mode of the calendar.
        /// 1. Highlight the selected date in single selection mode.
        /// 2. Highlight the selected dates in multiple selection mode.
        /// 3. Highlight the in between dates on range selection mode.
        /// </summary>
        /// <value> The default value of <see cref="SfCalendar.SelectionBackground"/> is null. </value>
        /// <remarks>
        /// Hovering background will change based on this value.
        /// If it is null then the selection background applied based on <see cref="SfCalendar.SelectionMode"/>.
        /// In single and multiple selection, the selection background will show on "#6200EE"(Blue).
        /// In range selection, the in between range background will show on "#6200EE"(Blue) with Opacity value of 0.1.
        /// </remarks>
        /// <seealso cref="SfCalendar.SelectionMode"/>
        /// <seealso cref="SfCalendar.RangeSelectionDirection"/>
        /// <example>
        /// The following code demonstrates, how to use the SelectionBackground property in the calendar
        /// #[XAML](#tab/tabid-35)
        /// <code Lang="XAML"><![CDATA[
        /// <Calendar:SfCalendar x:Name="Calendar"
        ///                      SelectionBackground="Blue">
        /// </Calendar:SfCalendar>
        /// ]]></code>
        /// #[C#](#tab/tabid-36)
        /// <code Lang="C#"><![CDATA[
        /// Calendar.SelectionBackground = Colors.Blue;
        /// ]]></code>
        /// </example>
        public Brush SelectionBackground
        {
            get { return (Brush)GetValue(SelectionBackgroundProperty); }
            set { SetValue(SelectionBackgroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value that describes the highlight for range start date of calendar.
        /// </summary>
        /// <value> The default value of <see cref="SfCalendar.StartRangeSelectionBackground"/> is "#6200EE"(Blue). </value>
        /// <remarks> This property is only applicable only for <see cref="CalendarSelectionMode.Range"/> selection mode.</remarks>
        /// <seealso cref="SfCalendar.SelectionMode"/>
        /// <seealso cref="SfCalendar.RangeSelectionDirection"/>
        /// <seealso cref="SfCalendar.SelectionBackground"/>
        /// <seealso cref="SfCalendar.EndRangeSelectionBackground"/>
        /// <seealso cref="CalendarMonthView.SelectionTextStyle"/>
        /// <seealso cref="CalendarYearView.SelectionTextStyle"/>
        /// <example>
        /// The following code demonstrates, how to use the StartRangeSelectionBackground property in the calendar
        /// #[XAML](#tab/tabid-37)
        /// <code Lang="XAML"><![CDATA[
        /// <Calendar:SfCalendar x:Name="Calendar"
        ///                      StartRangeSelectionBackground="Purple">
        /// </Calendar:SfCalendar>
        /// ]]></code>
        /// #[C#](#tab/tabid-38)
        /// <code Lang="C#"><![CDATA[
        /// Calendar.StartRangeSelectionBackground = Colors.Purple;
        /// ]]></code>
        /// </example>
        public Brush StartRangeSelectionBackground
        {
            get { return (Brush)GetValue(StartRangeSelectionBackgroundProperty); }
            set { SetValue(StartRangeSelectionBackgroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value that describes the highlight for range end date of calendar
        /// </summary>
        /// <value> The default value of <see cref="SfCalendar.EndRangeSelectionBackground"/> is "#6200EE"(Blue). </value>
        /// <remarks> This property is only applicable only for <see cref="CalendarSelectionMode.Range"/> selection mode.</remarks>
        /// <seealso cref="SfCalendar.SelectionMode"/>
        /// <seealso cref="SfCalendar.RangeSelectionDirection"/>
        /// <seealso cref="SfCalendar.SelectionBackground"/>
        /// <seealso cref="SfCalendar.StartRangeSelectionBackground"/>
        /// <seealso cref="CalendarMonthView.SelectionTextStyle"/>
        /// <seealso cref="CalendarYearView.SelectionTextStyle"/>
        /// <example>
        /// The following code demonstrates, how to use the EndRangeSelectionBackground property in the calendar
        /// #[XAML](#tab/tabid-39)
        /// <code Lang="XAML"><![CDATA[
        /// <Calendar:SfCalendar x:Name="Calendar"
        ///                      EndRangeSelectionBackground="purple">
        /// </Calendar:SfCalendar>
        /// ]]></code>
        /// #[C#](#tab/tabid-40)
        /// <code Lang="C#"><![CDATA[
        /// Calendar.EndRangeSelectionBackground = Colors.Purple;
        /// ]]></code>
        /// </example>
        public Brush EndRangeSelectionBackground
        {
            get { return (Brush)GetValue(EndRangeSelectionBackgroundProperty); }
            set { SetValue(EndRangeSelectionBackgroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to displays the leading and trailing dates in the month, decade, century views of the SfCalendar.
        /// </summary>
        /// <value> The default value of <see cref="SfCalendar.ShowTrailingAndLeadingDates"/> is true. </value>
        /// <remarks>
        /// This property is not applicable for <see cref="CalendarView.Year"/> view.
        /// </remarks>
        /// <seealso cref="SfCalendar.MonthView"/>
        /// <seealso cref="SfCalendar.YearView"/>
        /// <seealso cref="CalendarMonthView.TrailingLeadingDatesBackground"/>
        /// <seealso cref="CalendarMonthView.TrailingLeadingDatesTextStyle"/>
        /// <seealso cref="CalendarYearView.LeadingDatesBackground"/>
        /// <seealso cref="CalendarYearView.LeadingDatesTextStyle"/>
        /// <example>
        /// The following code demonstrates, how to use the ShowTrailingAndLeadingDates property in the calendar
        /// #[XAML](#tab/tabid-41)
        /// <code Lang="XAML"><![CDATA[
        /// <Calendar:SfCalendar x:Name="Calendar"
        ///                       ShowTrailingAndLeadingDates="True">
        /// </Calendar:SfCalendar>
        /// ]]></code>
        /// #[C#](#tab/tabid-42)
        /// <code Lang="C#"><![CDATA[
        /// Calendar.ShowTrailingAndLeadingDates = true;
        /// ]]></code>
        /// </example>
        public bool ShowTrailingAndLeadingDates
        {
            get { return (bool)GetValue(ShowTrailingAndLeadingDatesProperty); }
            set { SetValue(ShowTrailingAndLeadingDatesProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value determines the range selection direction of the SfCalendar.
        /// </summary>
        /// <value> The default value of <see cref="SfCalendar.RangeSelectionDirection"/> is <see cref="CalendarRangeSelectionDirection.Default"/>. </value>
        /// <seealso cref="SfCalendar.SelectionMode"/>
        /// <seealso cref="SfCalendar.AllowViewNavigation"/>
        /// <seealso cref="SfCalendar.EnableSwipeSelection"/>
        /// <example>
        /// The following code demonstrates, how to use the RangeSelectionDirection property in the calendar
        /// #[XAML](#tab/tabid-43)
        /// <code Lang="XAML"><![CDATA[
        /// <Calendar:SfCalendar x:Name="Calendar"
        ///                      RangeSelectionDirection="Default"">
        /// </Calendar:SfCalendar>
        /// ]]></code>
        /// #[XAML](#tab/tabid-44)
        /// <code Lang="C#"><![CDATA[
        /// Calendar.RangeSelectionDirection = CalendarRangeSelectionDirection.Default;
        /// ]]></code>
        /// </example>
        public CalendarRangeSelectionDirection RangeSelectionDirection
        {
            get { return (CalendarRangeSelectionDirection)GetValue(RangeSelectionDirectionProperty); }
            set { SetValue(RangeSelectionDirectionProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value determines the identifier of the calendar.
        /// </summary>
        /// <value>The default value of <see cref="SfCalendar.Identifier"/> is <see cref="CalendarIdentifier.Gregorian"/>.</value>
        /// <remarks>
        /// <para>FlowDirection will be updated based on the <see cref="Identifier"/> and if you want to override this behavior, set FlowDirection after <see cref="Identifier"/></para>
        /// <para>
        /// Other than the gregorian calendar, the DateTime values can be given in two ways.
        /// <list type="number">
        /// <item>
        ///     <description>The DateTime instance without specifying calendar identifier, the calendar will handle the DateTime value for the specified calendar identifier.</description>
        /// </item>
        /// <item>
        ///     <description>The DateTime instance with specifying calendar identifier, then the calendar handles it directly.</description>
        /// </item>
        /// </list>
        /// </para>
        /// <para>Non supported identifiers are Lunar type identifier, Hebrew, and Japanese Identifiers.</para>
        /// </remarks>
        /// <example>
        /// The following code demonstrates, how to use the Identifier in the calendar.
        /// #[XAML](#tab/tabid-49)
        /// <code Lang="XAML"><![CDATA[
        /// <Calendar:SfCalendar x:Name="Calendar"
        ///                      Identifier="Hijri">
        /// </Calendar:SfCalendar>
        /// ]]></code>
        /// # [C#](#tab/tabid-50)
        /// <code Lang="C#"><![CDATA[
        /// Calendar.Identifier = CalendarIdentifier.Hijri;
        /// ]]></code>
        /// </example>
        public CalendarIdentifier Identifier
        {
            get { return (CalendarIdentifier)GetValue(IdentifierProperty); }
            set { SetValue(IdentifierProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to dates is out of minimum and maximum date range on the calendar.
        /// </summary>
        /// <value> The default value of <see cref="SfCalendar.ShowOutOfRangeDates"/> is true.</value>
        /// <seealso cref="SfCalendar.View"/>
        /// <example>
        /// The following code demonstrates, how to use the ShowOutOfRangeDates property in the calendar
        /// #[XAML](#tab/tabid-47)
        /// <code Lang="XAML"><![CDATA[
        /// <Calendar:SfCalendar x:Name="Calendar"
        ///                      ShowOutOfRangeDates="False">
        /// </Calendar:SfCalendar>
        /// ]]></code>
        /// #[XAML](#tab/tabid-48)
        /// <code Lang="C#"><![CDATA[
        /// Calendar.ShowOutOfRangeDates = false;
        /// ]]></code>
        /// </example>
        public bool ShowOutOfRangeDates
        {
            get { return (bool)GetValue(ShowOutOfRangeDatesProperty); }
            set { SetValue(ShowOutOfRangeDatesProperty, value); }
        }

        /// <summary>
        /// Gets or sets the properties which allows to customize the calendar footer.
        /// </summary>
        /// /// <remarks>
        /// It will be applicable to all <see cref="SfCalendar.View"/>.
        /// </remarks>
        /// <example>
        /// The following code demonstrates, how to use the Footerview property in the calendar
        /// <code Lang="C#"><![CDATA[
        /// Calendar.View = CalendarView.Month;
        /// var calendarTextStyle = new CalendarTextStyle()
        /// {
        ///     TextColor = Colors.Black,
        /// };
        /// var calendarFooterView = new CalendarFooterView()
        /// {
        ///     ShowActionButtons = true,
        ///     ShowTodayButton = true,
        ///     TextStyle = calendarTextStyle,
        ///     Height = 45,
        ///     Background = Colors.LightBlue,
        /// };
        /// Calendar.FooterView = calendarFooterView;
        /// ]]></code>
        /// </example>
        public CalendarFooterView FooterView
        {
            get { return (CalendarFooterView)GetValue(FooterViewProperty); }
            set { SetValue(FooterViewProperty, value); }
        }

        /// <summary>
        /// Gets or sets the corner radius of the calendar view.
        /// </summary>
        /// <value> The default value of <see cref="SfCalendar.CornerRadius"/> is 20.</value>
        /// <example>
        /// The following code demonstrates, how to use the CornerRadius property in the calendar.
        /// #[XAML](#tab/tabid-49)
        /// <code Lang="XAML"><![CDATA[
        /// <Calendar:SfCalendar x:Name="Calendar"
        ///                      CornerRadius="10">
        /// </Calendar:SfCalendar>
        /// ]]></code>
        /// #[XAML](#tab/tabid-50)
        /// <code Lang="C#"><![CDATA[
        /// Calendar.CornerRadius = 10;
        /// ]]></code>
        /// </example>
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        /// <summary>
        /// Gets or sets the selection changed command calendar view.
        /// </summary>
        /// <value> The default value of <see cref="SfCalendar.SelectionChangedCommand"/> is null.</value>
        /// <example>
        /// The following code demonstrates, how to use the SelectionChangedCommand property in the calendar.
        /// #[XAML](#tab/tabid-49)
        /// <code Lang="XAML"><![CDATA[
        ///  <ContentPage.BindingContext>
        ///    <local:ViewModel/>
        ///  </ContentPage.BindingContext>
        /// <Calendar:SfCalendar x:Name="Calendar"
        ///                      SelectionChangedCommand="{Binding SelectionCommand}">
        /// </Calendar:SfCalendar>
        /// ]]></code>
        /// #[XAML](#tab/tabid-50)
        /// <code Lang="C#"><![CDATA[
        /// public class ViewModel : INotifyPropertyChanged
        /// {
        ///    private Command selectionCommand;
        ///    public ICommand SelectionCommand {
        ///        get
        ///        {
        ///            return selectionCommand;
        ///        }
        ///        set
        ///        {
        ///            if (selectionCommand != value)
        ///            {
        ///                selectionCommand = value;
        ///                OnPropertyChanged(nameof(SelectionCommand));
        ///            }
        ///        }
        ///    }
        ///   public ViewModel()
        ///    {
        ///      SelectionCommand = new Command(SelectionChanged);
        ///    }
        ///    private void SelectionChanged()
        ///     {
        ///     }
        ///  }
        /// ]]></code>
        /// </example>
        public ICommand SelectionChangedCommand
        {
            get { return (ICommand)GetValue(SelectionChangedCommandProperty); }
            set { SetValue(SelectionChangedCommandProperty, value); }
        }

        /// <summary>
        /// Gets or sets the view changed command calendar view.
        /// </summary>
        /// <value> The default value of <see cref="SfCalendar.ViewChangedCommand"/> is null.</value>
        /// <example>
        /// The following code demonstrates, how to use the ViewChangedCommand property in the calendar.
        /// #[XAML](#tab/tabid-49)
        /// <code Lang="XAML"><![CDATA[
        ///  <ContentPage.BindingContext>
        ///    <local:ViewModel/>
        ///  </ContentPage.BindingContext>
        /// <Calendar:SfCalendar x:Name="Calendar"
        ///                      ViewChanged="{Binding ViewChangedCommand}">
        /// </Calendar:SfCalendar>
        /// ]]></code>
        /// #[XAML](#tab/tabid-50)
        /// <code Lang="C#"><![CDATA[
        /// public class ViewModel : INotifyPropertyChanged
        /// {
        ///    private Command viewChangedCommand;
        ///    public ICommand ViewChangedCommand {
        ///        get
        ///        {
        ///            return viewChangedCommand;
        ///        }
        ///        set
        ///        {
        ///            if (viewChangedCommand != value)
        ///            {
        ///                viewChangedCommand = value;
        ///                OnPropertyChanged(nameof(ViewChangedCommand));
        ///            }
        ///        }
        ///    }
        ///   public ViewModel()
        ///    {
        ///      ViewChangedCommand = new Command(ViewChanged);
        ///    }
        ///   private void ViewChanged()
        ///     {
        ///     }
        ///  }
        /// ]]></code>
        /// </example>
        public ICommand ViewChangedCommand
        {
            get { return (ICommand)GetValue(ViewChangedCommandProperty); }
            set { SetValue(ViewChangedCommandProperty, value); }
        }

        /// <summary>
        /// Gets or sets the tapped command in calendar view.
        /// </summary>
        /// <value> The default value of <see cref="SfCalendar.TappedCommand"/> is null.</value>
        /// <example>
        /// The following code demonstrates, how to use the TappedCommand property in the calendar.
        /// #[XAML](#tab/tabid-49)
        /// <code Lang="XAML"><![CDATA[
        ///  <ContentPage.BindingContext>
        ///    <local:ViewModel/>
        ///  </ContentPage.BindingContext>
        /// <Calendar:SfCalendar x:Name="Calendar"
        ///                      ViewChanged="{Binding ViewChangedCommand}">
        /// </Calendar:SfCalendar>
        /// ]]></code>
        /// #[XAML](#tab/tabid-50)
        /// <code Lang="C#"><![CDATA[
        /// public class ViewModel : INotifyPropertyChanged
        /// {
        ///    private Command viewChangedCommand;
        ///    public ICommand ViewChangedCommand {
        ///        get
        ///        {
        ///            return viewChangedCommand;
        ///        }
        ///        set
        ///        {
        ///            if (viewChangedCommand != value)
        ///            {
        ///                viewChangedCommand = value;
        ///                OnPropertyChanged(nameof(ViewChangedCommand));
        ///            }
        ///        }
        ///    }
        ///   public ViewModel()
        ///    {
        ///      ViewChangedCommand = new Command(ViewChanged);
        ///    }
        ///   private void ViewChanged()
        ///     {
        ///     }
        ///  }
        /// ]]></code>
        /// </example>
        public ICommand TappedCommand
        {
            get { return (ICommand)GetValue(TappedCommandProperty); }
            set { SetValue(TappedCommandProperty, value); }
        }

        /// <summary>
        /// Gets or sets the double tapped command in calendar view.
        /// </summary>
        /// <value> The default value of <see cref="SfCalendar.DoubleTappedCommand"/> is null.</value>
        /// <example>
        /// The following code demonstrates, how to use the DoubleTappedCommand property in the calendar.
        /// #[XAML](#tab/tabid-49)
        /// <code Lang="XAML"><![CDATA[
        ///  <ContentPage.BindingContext>
        ///    <local:ViewModel/>
        ///  </ContentPage.BindingContext>
        /// <Calendar:SfCalendar x:Name="Calendar"
        ///                      DoubleTappedCommand="{Binding DoubleTappedCommand}">
        /// </Calendar:SfCalendar>
        /// ]]></code>
        /// #[XAML](#tab/tabid-50)
        /// <code Lang="C#"><![CDATA[
        /// public class ViewModel : INotifyPropertyChanged
        /// {
        ///    private Command doubleTappedCommand;
        ///    public ICommand DoubleTappedCommand {
        ///        get
        ///        {
        ///            return doubleTappedCommand;
        ///        }
        ///        set
        ///        {
        ///            if (doubleTappedCommand != value)
        ///            {
        ///                doubleTappedCommand = value;
        ///                OnPropertyChanged(nameof(DoubleTappedCommand));
        ///            }
        ///        }
        ///    }
        ///   public ViewModel()
        ///    {
        ///      DoubleTappedCommand = new Command(DoubleTapped);
        ///    }
        ///   private void DoubleTapped()
        ///     {
        ///     }
        ///  }
        /// ]]></code>
        /// </example>
        public ICommand DoubleTappedCommand
        {
            get { return (ICommand)GetValue(DoubleTappedCommandProperty); }
            set { SetValue(DoubleTappedCommandProperty, value); }
        }

        /// <summary>
        /// Gets or sets the long pressed command in calendar view.
        /// </summary>
        /// <value> The default value of <see cref="SfCalendar.LongPressedCommand"/> is null.</value>
        /// <example>
        /// The following code demonstrates, how to use the LongPressedCommand property in the calendar.
        /// #[XAML](#tab/tabid-49)
        /// <code Lang="XAML"><![CDATA[
        ///  <ContentPage.BindingContext>
        ///    <local:ViewModel/>
        ///  </ContentPage.BindingContext>
        /// <Calendar:SfCalendar x:Name="Calendar"
        ///                      LongPressedCommand="{Binding LongPressedCommand}">
        /// </Calendar:SfCalendar>
        /// ]]></code>
        /// #[XAML](#tab/tabid-50)
        /// <code Lang="C#"><![CDATA[
        /// public class ViewModel : INotifyPropertyChanged
        /// {
        ///    private Command longPressedCommand;
        ///    public ICommand LongPressedCommand {
        ///        get
        ///        {
        ///            return longPressedCommand;
        ///        }
        ///        set
        ///        {
        ///            if (longPressedCommand != value)
        ///            {
        ///                longPressedCommand = value;
        ///                OnPropertyChanged(nameof(LongPressedCommand));
        ///            }
        ///        }
        ///    }
        ///   public ViewModel()
        ///    {
        ///      LongPressedCommand = new Command(LongPressed);
        ///    }
        ///   private void LongPressed()
        ///     {
        ///     }
        ///  }
        /// ]]></code>
        /// </example>
        public ICommand LongPressedCommand
        {
            get { return (ICommand)GetValue(LongPressedCommandProperty); }
            set { SetValue(LongPressedCommandProperty, value); }
        }

        /// <summary>
        /// Gets or sets the action button clicked command in calendar view.
        /// </summary>
        /// <value> The default value of <see cref="SfCalendar.AcceptCommand"/> is null.</value>
        /// <example>
        /// The following code demonstrates, how to use the AcceptCommand property in the calendar.
        /// #[XAML](#tab/tabid-49)
        /// <code Lang="XAML"><![CDATA[
        ///  <ContentPage.BindingContext>
        ///    <local:ViewModel/>
        ///  </ContentPage.BindingContext>
        /// <Calendar:SfCalendar x:Name="Calendar" ShowActionButtons="True"
        ///                      AcceptCommand="{Binding AcceptCommand}">
        /// </Calendar:SfCalendar>
        /// ]]></code>
        /// #[XAML](#tab/tabid-50)
        /// <code Lang="C#"><![CDATA[
        /// public class ViewModel : INotifyPropertyChanged
        /// {
        ///    private Command actionButtonClickedCommand;
        ///    public ICommand AcceptCommand {
        ///        get
        ///        {
        ///            return actionButtonClickedCommand;
        ///        }
        ///        set
        ///        {
        ///            if (actionButtonClickedCommand != value)
        ///            {
        ///                actionButtonClickedCommand = value;
        ///                OnPropertyChanged(nameof(AcceptCommand));
        ///            }
        ///        }
        ///    }
        ///   public ViewModel()
        ///    {
        ///      AcceptCommand = new Command(ActionButtonClicked);
        ///    }
        ///   private void ActionButtonClicked()
        ///     {
        ///     }
        ///  }
        /// ]]></code>
        /// </example>
        public ICommand AcceptCommand
        {
            get { return (ICommand)GetValue(AcceptCommandProperty); }
            set { SetValue(AcceptCommandProperty, value); }
        }

        /// <summary>
        /// Gets or sets the action button canceled command in calendar view.
        /// </summary>
        /// <value> The default value of <see cref="SfCalendar.DeclineCommand"/> is null.</value>
        /// <example>
        /// The following code demonstrates, how to use the DeclineCommand property in the calendar.
        /// #[XAML](#tab/tabid-49)
        /// <code Lang="XAML"><![CDATA[
        ///  <ContentPage.BindingContext>
        ///    <local:ViewModel/>
        ///  </ContentPage.BindingContext>
        /// <Calendar:SfCalendar x:Name="Calendar" ShowActionButtons="True"
        ///                      DeclineCommand="{Binding DeclineCommand}">
        /// </Calendar:SfCalendar>
        /// ]]></code>
        /// #[XAML](#tab/tabid-50)
        /// <code Lang="C#"><![CDATA[
        /// public class ViewModel : INotifyPropertyChanged
        /// {
        ///    private Command actionButtonCanceledCommand;
        ///    public ICommand DeclineCommand {
        ///        get
        ///        {
        ///            return actionButtonCanceledCommand;
        ///        }
        ///        set
        ///        {
        ///            if (actionButtonCanceledCommand != value)
        ///            {
        ///                actionButtonCanceledCommand = value;
        ///                OnPropertyChanged(nameof(DeclineCommand));
        ///            }
        ///        }
        ///    }
        ///   public ViewModel()
        ///    {
        ///      DeclineCommand = new Command(ActionButtonCanceled);
        ///    }
        ///   private void ActionButtonCanceled()
        ///     {
        ///     }
        ///  }
        /// ]]></code>
        /// </example>
        public ICommand DeclineCommand
        {
            get { return (ICommand)GetValue(DeclineCommandProperty); }
            set { SetValue(DeclineCommandProperty, value); }
        }

        /// <summary>
        /// Gets or sets the header template or template selector for month, year, decade, and century view.
        /// </summary>
        /// <remarks>
        /// For default view customization, use the <see cref="SfCalendar.HeaderView"/>.
        /// </remarks>
        /// <example>
        /// <para>The following sample used to configure header view using the <see cref="DataTemplate"/>.</para>
        /// # [XAML](#tab/tabid-1)
        /// <code Lang="XAML">
        /// <![CDATA[
        /// <calendar:SfCalendar x:Name="Calendar"
        ///                        View="Month">
        ///		<calendar:SfCalendar.HeaderTemplate>
        ///			<DataTemplate>
        ///				<Grid Background = "LightBlue">
        ///					<Label x:Name="label" HorizontalOptions="Start" VerticalOptions="Center">
        ///                 <Label.Text>
        ///						 <MultiBinding StringFormat = "{}{0:MMM dd, yyyy} - {1:MMM dd, yyyy}">
        ///                       <Binding Path="StartDateRange" />
        ///                       <Binding Path = "EndDateRange" />
        ///                    </MultiBinding>
        ///                 </Label.Text>
        ///                 </Label>
        ///					<Label  HorizontalOptions="Start" VerticalOptions="End" Text="{Binding Text}" TextColor="Red" />
        ///             </Grid>
        ///        </DataTemplate>
        ///		</calendar:SfCalendar.HeaderTemplate>
        /// </calendar:SfCalendar>
        /// ]]>
        /// </code>
        /// <para>The following sample used to configure header view using the <see cref="DataTemplateSelector"/>.</para>
        /// # [DataTemplateSelector](#tab/tabid-2)
        /// <code Lang="C#">
        /// <![CDATA[
        /// public class HeaderTemplateSelector : DataTemplateSelector
        /// {
        ///    public HeaderTemplateSelector()
        ///    {
        ///    }
        ///    public DataTemplate TodayDatesTemplate { get; set; }
        ///    public DataTemplate NormaldatesTemplate { get; set; }
        ///    protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        ///    {
        ///        var headerDetails = item as CalendarHeaderDetails;
        ///        if (headerDetails != null)
        ///         {
        ///            if (headerDetails.StartDateRange.Date <= DateTime.Now.Date && headerDetails.EndDateRange >= DateTime.Now.Date)
        ///                return TodayDatesTemplate;
        ///         }
        ///        return NormaldatesTemplate;
        ///     }
        /// }
        /// ]]>
        /// </code>
        /// # [Resources](#tab/tabid-3)
        /// <code Lang="XAML">
        /// <![CDATA[
        /// <ContentPage.Resources>
        ///    <DataTemplate x:Key="todayDatesTemplate">
        ///    <Grid Background="LightBlue">
        ///      <Label x:Name="label" HorizontalOptions="Start" VerticalOptions="Center">
        ///       <Label.Text>
        ///           <MultiBinding StringFormat = "{}{0:MMM dd, yyyy} - {1:MMM dd, yyyy}">
        ///                 <Binding Path="StartDateRange" />
        ///                  <Binding Path="EndDateRange" />
        ///            </MultiBinding>
        ///        </Label.Text>
        ///       </Label>
        ///      <Label HorizontalOptions="Start" VerticalOptions="End" Text="{Binding Text}" TextColor="Red" />
        ///     /Grid>
        ///    </DataTemplate>
        ///    <DataTemplate x:Key="normaldatesTemplate">
        ///        <Grid Background="LightGreen">
        ///            <Label x:Name="label" HorizontalOptions="Start" VerticalOptions="Center">
        ///                <Label.Text>
        ///                    <MultiBinding StringFormat = "{}{0:MMM dd, yyyy} - {1:MMM dd, yyyy}" >
        ///                        <Binding Path="StartDateRange" />
        ///                        <Binding Path = "EndDateRange" />
        ///                    </MultiBinding >
        ///                </Label.Text >
        ///            </Label>
        ///            <Label HorizontalOptions="Start" VerticalOptions="End" Text="{Binding Text}" TextColor="Orange" />
        ///        </Grid>
        ///    </DataTemplate>
        ///   <local:HeaderTemplateSelector x:Key="headerTemplateSelector" TodayDatesTemplate="{StaticResource todayDatesTemplate}"  NormaldatesTemplate="{StaticResource normaldatesTemplate}" />
        /// </ContentPage.Resources>
        /// ]]>
        /// </code>
        /// #[MainPage](#tab/tabid-4)
        /// <code Lang="XAML">
        /// <![CDATA[
        /// <calendar:SfCalendar x:Name="Calendar"
        ///                        View="Month"
        ///                        HeaderTemplate="{StaticResource headerTemplateSelector}">
        /// </calendar:SfCalendar>
        /// ]]>
        /// </code>
        /// </example>
        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }

        /// <summary>
        /// Gets or sets the view header template or template selector for month view.
        /// </summary>
        /// <remarks>
        /// For default view customization, use <see cref="CalendarMonthHeaderView"/> from <see cref="SfCalendar.MonthView"/>.
        /// </remarks>
        /// <example>
        /// <para>The following code used to configure the month view header using the <see cref="DataTemplate"/>.</para>
        /// # [XAML](#tab/tabid-11)
        /// <code Lang="XAML">
        /// <![CDATA[
        /// <calendar:SfCalendar x:Name="Calendar"
        ///                        View="Month">
        ///     <calendar:SfCalendar.MonthViewHeaderTemplate>
        ///			<DataTemplate>
        ///             <Grid Background="lightBlue">
        ///                 <Label x:Name="label" HorizontalOptions="Center" VerticalOptions="Center" Text="{Binding StringFormat='{0:ddd}'}" TextColor="Red" />
        ///             </Grid>
        ///         </DataTemplate>
        ///    </calendar:SfCalendar.MonthViewHeaderTemplate>
        /// </calendar:SfCalendar>
        /// ]]>
        /// </code>
        /// <para>The following code used to configure the month view header using the <see cref="DataTemplateSelector"/>.</para>
        /// # [DataTemplateSelector](#tab/tabid-12)
        /// <code Lang="C#">
        /// <![CDATA[
        /// public class MonthViewHeaderTemplateSelector : DataTemplateSelector
        /// {
        ///    public ViewHeaderTemplateSelector()
        ///    {
        ///    }
        ///    public DataTemplate TodayDatesTemplate { get; set; }
        ///    public DataTemplate NormaldatesTemplate { get; set; }
        ///    protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        ///    {
        ///       var viewHeaderDetails = (DateTime)item;
        ///            if (viewHeaderDetails.DateTime.Month == DateTime.Today.Month)
        ///                return TodayDatesTemplate;
        ///        return NormaldatesTemplate;
        ///    }
        /// }
        /// ]]>
        /// </code>
        /// # [Resources](#tab/tabid-13)
        /// <code Lang="XAML">
        /// <![CDATA[
        /// <ContentPage.Resources>
        /// <DataTemplate x:Key="normaldatesTemplate">
        ///      <Grid Background="lightBlue">
        ///         <Label x:Name="label" HorizontalOptions="Center" VerticalOptions="Center" Text="{Binding StringFormat='{0:ddd}'}" TextColor="Red" />
        ///      </Grid>
        ///    </DataTemplate>
        ///    <DataTemplate x:Key="todayDatesTemplate">
        ///        <Grid Background="LightGreen">
        ///            <Label x:Name="label" HorizontalOptions="Center" VerticalOptions="Center" Text="{Binding StringFormat='{0:ddd}'}" TextColor="Orange" />
        ///        </Grid>
        ///   </DataTemplate>
        ///  <local:MonthViewHeaderTemplateSelector x:Key="monthViewHeaderTemplateSelector"  TodayDatesTemplate="{StaticResource todayDatesTemplate}" NormaldatesTemplate="{StaticResource normaldatesTemplate}" />
        /// </ContentPage.Resources>
        /// ]]>
        /// </code>
        /// #[MainPage](#tab/tabid-14)
        /// <code Lang="XAML">
        /// <![CDATA[
        /// <calendar:SfCalendar x:Name="Calendar"
        ///                       View="Month"
        ///                       MonthViewHeaderTemplate="{StaticResource monthViewHeaderTemplateSelector}">
        /// </calendar:SfCalendar>
        /// ]]>
        /// </code>
        /// </example>
        public DataTemplate MonthViewHeaderTemplate
        {
            get { return (DataTemplate)GetValue(MonthViewHeaderTemplateProperty); }
            set { SetValue(MonthViewHeaderTemplateProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the calendar popup is open or not.
        /// </summary>
        /// <value>The default value of <see cref="SfCalendar.IsOpen"/> is "False".</value>
        /// <remarks>
        /// It will be applicable to set the <see cref="CalendarMode.Dialog"/> or <see cref="CalendarMode.RelativeDialog"/>.
        /// </remarks>
        /// <example>
        /// <para>The following code example demonstrates, how to set IsOpen property for the <see cref="SfCalendar"/> control.</para>
        /// <code lang="C#">
        /// <![CDATA[
        /// using System.ComponentModel;
        ///
        /// namespace CalendarMAUI
        /// {
        ///     public partial class MainPage : ContentPage
        ///     {
        ///         public MainPage()
        ///         {
        ///            InitializeComponent();
        ///         }
        ///         void clickToShowPopup_Clicked(object sender, EventArgs e)
        ///         {
        ///             Calendar.IsOpen = true;
        ///         }
        ///      }
        /// }
        /// ]]>
        /// </code>
        /// <code lang="XAML">
        /// <![CDATA[
        /// <?xml version = "1.0" encoding="utf-8" ?>
        /// <ContentPage xmlns = "http://schemas.microsoft.com/dotnet/2021/maui"
        ///     xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
        ///     xmlns:syncfusion="clr-namespace:Syncfusion.Maui.Toolkit.Calendar;assembly=Syncfusion.Maui.Toolkit"
        ///     xmlns:local="clr-namespace:CalendarMAUI"
        ///     x:Class="CalendarMAUI.MainPage">
        /// <ContentPage.Content>
        ///    <StackLayout WidthRequest = "500" >
        ///        <syncfusion:SfCalendar x:Name="Calendar" Mode="Dialog"/>
        ///        <Button x:Name="clickToShowCalendar" Text="Click To Show Calendar" Clicked="clickToShowPopup_Clicked"/>
        ///    </StackLayout>
        /// </ContentPage.Content>
        /// </ContentPage>
        /// ]]>
        /// </code>
        /// </example>
        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        /// <summary>
        /// Gets or sets the calendar mode.
        /// </summary>
        /// <value>The default value of <see cref="SfCalendar.Mode"/> is <see cref="CalendarMode.Default"/>.</value>
        /// <remarks>
        /// The <see cref="CalendarMode.Dialog"/> and <see cref="CalendarMode.RelativeDialog"/> only visible to set the <see cref="SfCalendar.IsOpen"/> is "True".
        /// </remarks>
        /// <example>
        /// The following code demonstrates, how to use the Mode property in the calendar
        /// #[XAML](#tab/tabid-1)
        /// <code Lang="XAML"><![CDATA[
        /// <Calendar:SfCalendar x:Name="Calendar"
        ///                      Mode="Dialog">
        /// </Calendar:SfCalendar>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code Lang="C#"><![CDATA[
        /// Calendar.IsOpen = True;
        /// ]]></code>
        /// </example>
        public CalendarMode Mode
        {
            get { return (CalendarMode)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the relative position of the calendar popup.
        /// </summary>
        /// <value>The default value of <see cref="SfCalendar.RelativePosition"/> is <see cref="CalendarRelativePosition.AlignTop"/>.</value>
        /// <remarks>
        /// It will be applicable to set <see cref="SfCalendar.Mode"/> is <see cref="CalendarMode.RelativeDialog"/>.
        /// </remarks>
        /// <example>
        /// The following code demonstrates, how to use the View property in the calendar
        /// #[XAML](#tab/tabid-1)
        /// <code Lang="XAML"><![CDATA[
        /// <Calendar:SfCalendar x:Name="Calendar"
        ///                      Mode="RelativeDialog" RelativePosition="AlignBottom">
        /// </Calendar:SfCalendar>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code Lang="C#"><![CDATA[
        /// Calendar.IsOpen = True;
        /// ]]></code>
        /// </example>
        public CalendarRelativePosition RelativePosition
        {
            get { return (CalendarRelativePosition)GetValue(RelativePositionProperty); }
            set { SetValue(RelativePositionProperty, value); }
        }

        //// TODO: Workaround for RTL (Right-to-Left) layout issue - The coordinate points are not calculated correctly in RTL layouts,
        //// causing incorrect positioning. This flag helps to apply RTL-specific adjustments.
#if WINDOWS
        /// <summary>
        /// Gets or sets the flow direction for month view.
        /// </summary>
        /// <value> The default value of <see cref="SfCalendar.FlowDirection"/> is <see cref="FlowDirection.LeftToRight"/>.</value>
        public new FlowDirection FlowDirection
        {
            get { return (FlowDirection)GetValue(FlowDirectionProperty); }
            set { SetValue(FlowDirectionProperty, value); }
        }
#endif

        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets the value for button color.
        /// </summary>
        Brush IHeaderCommon.ButtonTextColor => ButtonTextColor;

        /// <summary>
        /// Gets the value for header hover color.
        /// </summary>
        Brush IHeaderCommon.HeaderHoverColor => HeaderHoverTextColor;

        /// <summary>
        /// Gets the value for selected range color.
        /// </summary>
        Brush ICalendarCommon.SelectedRangeColor => RangeSelectionColor;

        /// <summary>
        /// Gets the value for hover color.
        /// </summary>
        Brush ICalendarCommon.HoverColor => HoverTextColor;

        /// <summary>
        /// Gets or sets the value that describes the button text color value.
        /// </summary>
        internal Brush ButtonTextColor
        {
            get { return (Brush)GetValue(ButtonTextColorProperty); }
            set { SetValue(ButtonTextColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value that describes the header hover color value.
        /// </summary>
        internal Brush HeaderHoverTextColor
        {
            get { return (Brush)GetValue(HeaderHoverTextColorProperty); }
            set { SetValue(HeaderHoverTextColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value that describes the range selection color value.
        /// </summary>
        internal Brush RangeSelectionColor
        {
            get { return (Brush)GetValue(RangeSelectionColorProperty); }
            set { SetValue(RangeSelectionColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value that describes the hover color value.
        /// </summary>
        internal Brush HoverTextColor
        {
            get { return (Brush)GetValue(HoverTextColorProperty); }
            set { SetValue(HoverTextColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the background color of the calendar.
        /// </summary>
        internal Color CalendarBackground
        {
            get { return (Color)GetValue(CalendarBackgroundProperty); }
            set { SetValue(CalendarBackgroundProperty, value); }
        }

        #endregion

        #region Override Methods

        /// <summary>
        /// Triggers when property binding context changed.
        /// </summary>
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if (HeaderView != null)
            {
                SetInheritedBindingContext(HeaderView, BindingContext);
                if (HeaderView.TextStyle != null)
                {
                    SetInheritedBindingContext(HeaderView.TextStyle, BindingContext);
                }
            }

            if (MonthView != null)
            {
                SetInheritedBindingContext(MonthView, BindingContext);
                if (MonthView.TextStyle != null)
                {
                    SetInheritedBindingContext(MonthView.TextStyle, BindingContext);
                }

                if (MonthView.DisabledDatesTextStyle != null)
                {
                    SetInheritedBindingContext(MonthView.DisabledDatesTextStyle, BindingContext);
                }

                if (MonthView.RangeTextStyle != null)
                {
                    SetInheritedBindingContext(MonthView.RangeTextStyle, BindingContext);
                }

                if (MonthView.SelectionTextStyle != null)
                {
                    SetInheritedBindingContext(MonthView.SelectionTextStyle, BindingContext);
                }

                if (MonthView.SpecialDatesTextStyle != null)
                {
                    SetInheritedBindingContext(MonthView.SpecialDatesTextStyle, BindingContext);
                }

                if (MonthView.TodayTextStyle != null)
                {
                    SetInheritedBindingContext(MonthView.TodayTextStyle, BindingContext);
                }

                if (MonthView.TrailingLeadingDatesTextStyle != null)
                {
                    SetInheritedBindingContext(MonthView.TrailingLeadingDatesTextStyle, BindingContext);
                }

                if (MonthView.WeekendDatesTextStyle != null)
                {
                    SetInheritedBindingContext(MonthView.WeekendDatesTextStyle, BindingContext);
                }

                if (MonthView.WeekNumberStyle != null)
                {
                    SetInheritedBindingContext(MonthView.WeekNumberStyle, BindingContext);
                    if (MonthView.WeekNumberStyle.TextStyle != null)
                    {
                        SetInheritedBindingContext(MonthView.WeekNumberStyle.TextStyle, BindingContext);
                    }
                }

                if (MonthView.HeaderView != null)
                {
                    SetInheritedBindingContext(MonthView.HeaderView, BindingContext);
                    if (MonthView.HeaderView.TextStyle != null)
                    {
                        SetInheritedBindingContext(MonthView.HeaderView.TextStyle, BindingContext);
                    }
                }
            }

            if (YearView != null)
            {
                SetInheritedBindingContext(YearView, BindingContext);
                if (YearView.TextStyle != null)
                {
                    SetInheritedBindingContext(YearView.TextStyle, BindingContext);
                }

                if (YearView.DisabledDatesTextStyle != null)
                {
                    SetInheritedBindingContext(YearView.DisabledDatesTextStyle, BindingContext);
                }

                if (YearView.LeadingDatesTextStyle != null)
                {
                    SetInheritedBindingContext(YearView.LeadingDatesTextStyle, BindingContext);
                }

                if (YearView.RangeTextStyle != null)
                {
                    SetInheritedBindingContext(YearView.RangeTextStyle, BindingContext);
                }

                if (YearView.SelectionTextStyle != null)
                {
                    SetInheritedBindingContext(YearView.SelectionTextStyle, BindingContext);
                }

                if (YearView.TodayTextStyle != null)
                {
                    SetInheritedBindingContext(YearView.TodayTextStyle, BindingContext);
                }
            }
        }

        /// <summary>
        /// Triggers when the calendar property changed.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        protected override void OnPropertyChanged(string? propertyName = null)
        {
            if (propertyName == nameof(IsVisible) && Mode != CalendarMode.Default)
            {
                //// If the calendar is not visible, we have to close the calendar popup only when it is opened previously.
                if (!IsVisible)
                {
                    _isCalendarPreviouslyOpened = IsOpen;
                    if (_isCalendarPreviouslyOpened)
                    {
                        CloseCalendarPopup();
                    }
                }
                //// If the calendar is visible, we have to open the calendar popup only when it is opened initally.
                else if (IsOpen && IsVisible)
                {
                    AddCalendarToPopup();
                    ShowPopup();
                }
                //// If the calendar is visible, we have to open the calendar popup only when it is opened previously.
                else if (IsVisible && _isCalendarPreviouslyOpened)
                {
                    IsOpen = true;
                }
                else
                {
                    CloseCalendarPopup();
                }
            }
            else if(propertyName == "FlowDirection")
            {
                if (FlowDirection == FlowDirection.RightToLeft)
                {
                    if (Identifier == CalendarIdentifier.Korean || Identifier == CalendarIdentifier.Taiwan || Identifier == CalendarIdentifier.ThaiBuddhist)
                    {
                        _isRTLLayout = false;
                    }
                    else
                    {
                        _isRTLLayout = true;
                    }
                }
                else if (FlowDirection == FlowDirection.LeftToRight || FlowDirection == FlowDirection.MatchParent)
                {
                    _isRTLLayout = this.IsRTL(Identifier);
                    UpdateFlowDirection();
                    _customScrollLayout?.UpdateVisibleDateOnView();
                }
            }

            base.OnPropertyChanged(propertyName);
        }

        #endregion

        #region Property Changed Methods

        /// <summary>
        /// Method invokes on calendar view changed.
        /// </summary>
        /// <param name="bindable">The calendar object.</param>
        /// <param name="oldValue">Old value of the property.</param>
        /// <param name="newValue">New value of the property.</param>
        static void OnViewChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfCalendar? calendar = bindable as SfCalendar;
            if (calendar == null || calendar._customScrollLayout == null)
            {
                return;
            }

            calendar.AddOrRemoveMonthHeaderView();
            calendar.UpdatePopUpSize();
            calendar._customScrollLayout.UpdateViewChange((CalendarView)oldValue, (CalendarView)newValue);
            if (calendar._customScrollLayout != null)
            {
                void UpdateProperty(double value)
                {
                    // During animation to checking if the _customScrollLayout is not null because once the calendar application is closed while it makes performance issue.
                    if (calendar._customScrollLayout != null)
                    {
                        calendar._customScrollLayout.Opacity = value;
                    }
                }

                AnimationExtensions.Animate(calendar._customScrollLayout, "view switching", UpdateProperty, 0, 1, 16, 500U, Easing.Linear);
            }

            if (calendar._monthViewHeader != null)
            {
                void UpdateProperty(double value)
                {
                    // During animation to checking if the month View Header is not null because once the calendar application is closed while it makes performance issue.
                    if (calendar._monthViewHeader != null)
                    {
                        calendar._monthViewHeader.Opacity = value;
                    }
                }

                AnimationExtensions.Animate(calendar._monthViewHeader, "view switching", UpdateProperty, 0, 1, 16, 500U, Easing.Linear);
            }

            //// To update the focus while view is changed on keyboard interaction.
            //// To set the focus for the year view while navigate from the month view.
            if ((CalendarView)oldValue == CalendarView.Month)
            {
                calendar.SetFocusOnViewNavigation(500);
            }
            //// To set the focus for the month view while navigate from the year view panel.
            else if ((CalendarView)newValue == CalendarView.Month)
            {
                calendar.SetFocusOnViewNavigation(500);
            }
        }

        /// <summary>
        /// Method invokes on calendar header view changed.
        /// </summary>
        /// <param name="bindable">The calendar object.</param>
        /// <param name="oldValue">Old value of the property.</param>
        /// <param name="newValue">New value of the property.</param>
        static void OnHeaderViewChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfCalendar? calendar = bindable as SfCalendar;
            if (bindable is SfCalendar calendarView)
            {
                calendarView.SetParent(oldValue as Element, newValue as Element);
            }

            if (calendar == null)
            {
                return;
            }

            CalendarHeaderView? oldStyle = oldValue as CalendarHeaderView;
            if (oldStyle != null)
            {
                oldStyle.BindingContext = null;
                oldStyle.CalendarPropertyChanged -= calendar.OnHeaderSettingsPropertyChanged;
                if (oldStyle.TextStyle != null)
                {
                    oldStyle.TextStyle.BindingContext = null;
                    oldStyle.TextStyle.PropertyChanged -= calendar.OnHeaderTextStylePropertyChanged;
                }
            }

            CalendarHeaderView? newStyle = newValue as CalendarHeaderView;
            if (newStyle != null)
            {
                SetInheritedBindingContext(newStyle, calendar.BindingContext);
                newStyle.CalendarPropertyChanged += calendar.OnHeaderSettingsPropertyChanged;
                if (newStyle.TextStyle != null)
                {
                    SetInheritedBindingContext(newStyle.TextStyle, calendar.BindingContext);
                    newStyle.TextStyle.PropertyChanged += calendar.OnHeaderTextStylePropertyChanged;
                }
            }

            if (calendar._customScrollLayout == null)
            {
                return;
            }

            if (oldStyle != null && newStyle != null && oldStyle.Height != newStyle.Height)
            {
                calendar.AddOrRemoveHeaderLayout();
                calendar._layout.UpdateHeaderHeight(calendar.HeaderView.GetHeaderHeight());
            }

            if (calendar._headerLayout == null)
            {
                return;
            }

            calendar._headerLayout.UpdateHeaderText();
            calendar.UpdatePopUpSize();
            calendar._headerLayout.InvalidateNavigationArrowVisibility();
            calendar._headerLayout.UpdateHeaderTextStyle();
            calendar._headerLayout.Background = calendar.HeaderView.Background;
        }

        /// <summary>
        /// Method invokes on footer view changed.
        /// </summary>
        /// <param name="bindable">The calendar object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        static void OnFooterViewChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfCalendar? calendar = bindable as SfCalendar;
            if (calendar == null)
            {
                return;
            }

            CalendarFooterView? oldStyle = oldValue as CalendarFooterView;
            if (oldStyle != null)
            {
                oldStyle.CalendarPropertyChanged -= calendar.OnFooterSettingsPropertyChanged;
                if (oldStyle.TextStyle != null)
                {
                    oldStyle.TextStyle.BindingContext = null;
                    oldStyle.TextStyle.PropertyChanged -= calendar.OnFooterTextStylePropertyChanged;
                }

                oldStyle.Parent = null;
                oldStyle.BindingContext = null;
            }

            CalendarFooterView? newStyle = newValue as CalendarFooterView;
            if (newStyle != null)
            {
                newStyle.Parent = calendar;
                SetInheritedBindingContext(newStyle, calendar.BindingContext);
                newStyle.CalendarPropertyChanged += calendar.OnFooterSettingsPropertyChanged;
                if (newStyle.TextStyle != null)
                {
                    SetInheritedBindingContext(newStyle.TextStyle, calendar.BindingContext);
                    newStyle.TextStyle.PropertyChanged += calendar.OnFooterTextStylePropertyChanged;
                }
            }

            if (calendar._customScrollLayout == null)
            {
                return;
            }

            calendar.AddOrRemoveFooterLayout();
            calendar._layout.UpdateFooterHeight(calendar.FooterView.GetFooterHeight());

            if (calendar._footerLayout == null)
            {
                return;
            }

            calendar._footerLayout.UpdateFooterStyle();
            calendar._footerLayout.UpdateBackground();
        }

        /// <summary>
        /// Method invokes on calendar navigation direction changed.
        /// </summary>
        /// <param name="bindable">The calendar object.</param>
        /// <param name="oldValue">Old value of the property.</param>
        /// <param name="newValue">New value of the property.</param>
        static void OnNavigationDirectionChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfCalendar? calendar = bindable as SfCalendar;
            if (calendar == null || calendar._customScrollLayout == null)
            {
                return;
            }

            if (calendar.View == CalendarView.Month)
            {
                calendar._customScrollLayout.UpdateMonthViewHeader();
                //// Here we have to add or remove when navigation direction is changed dynamically.
                calendar.AddOrRemoveMonthHeaderView();
                //// The navigation direction is changed need to update the semantic for month view and month header view.
                calendar._monthViewHeader?.InvalidateSemanticsNode(true);
                calendar._customScrollLayout.UpdateSemanticNodes();
            }

            calendar._customScrollLayout.UpdateVisibleDateOnView();
            //// Here we have to update the navigation arrows based on the navigation direction dynamic update.
            calendar._headerLayout?.UpdateNavigationArrowsIcon();
        }

        /// <summary>
        /// Method invokes on calendar display date changed.
        /// </summary>
        /// <param name="bindable">The calendar object.</param>
        /// <param name="oldValue">Old value of the property.</param>
        /// <param name="newValue">New value of the property.</param>
        static void OnDisplayDateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfCalendar calendar = (SfCalendar)bindable;
            DateTime displayDate = (DateTime)newValue;
            if (calendar._displayDate == displayDate)
            {
                return;
            }

            DateTime previousDisplayDate = calendar._displayDate;
            calendar._displayDate = displayDate;
            if (calendar._customScrollLayout == null)
            {
                return;
            }

            if (CalendarViewHelper.IsSameDisplayDateView(calendar, calendar._visibleDates, previousDisplayDate, displayDate))
            {
                return;
            }

            calendar._customScrollLayout.UpdateVisibleDateOnView();
        }

        /// <summary>
        /// Method invokes on min or max date changed.
        /// </summary>
        /// <param name="bindable">The calendar object.</param>
        /// <param name="oldValue">Old value of the property.</param>
        /// <param name="newValue">New value of the property.</param>
        static void OnMinMaxDateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfCalendar? calendar = bindable as SfCalendar;
            if (calendar == null || calendar._customScrollLayout == null)
            {
                return;
            }

            calendar._customScrollLayout.UpdateMinMaxDateChange((DateTime)newValue, (DateTime)oldValue);
            calendar._headerLayout?.UpdateNavigationButtonState();
        }

        /// <summary>
        /// Method invokes on enable past date changed.
        /// </summary>
        /// <param name="bindable">The calendar object.</param>
        /// <param name="oldValue">Old value of the property.</param>
        /// <param name="newValue">New value of the property.</param>
        static void OnEnablePastDatesChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfCalendar? calendar = bindable as SfCalendar;
            if (calendar == null || calendar._customScrollLayout == null)
            {
                return;
            }

            //// Need to update the calendar view(month or year) with hover view before the today date.
            calendar._customScrollLayout.UpdateEnablePastDates();
        }

        /// <summary>
        /// Method invokes on month view settings property changed.
        /// </summary>
        /// <param name="bindable">The calendar object.</param>
        /// <param name="oldValue">Old value of the property.</param>
        /// <param name="newValue">New value of the property.</param>
        static void OnMonthViewSettingsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfCalendar? calendar = bindable as SfCalendar;
            if (bindable is SfCalendar calendarView)
            {
                calendarView.SetParent(oldValue as Element, newValue as Element);
            }

            if (calendar == null)
            {
                return;
            }

            CalendarMonthView? oldSettings = oldValue as CalendarMonthView;
            if (oldSettings != null)
            {
                calendar.UnWireMonthViewEvents(oldSettings);
            }

            CalendarMonthView? newSettings = newValue as CalendarMonthView;
            if (newSettings != null)
            {
                calendar.WireMonthViewEvents(newSettings);
            }

            //// Skip the update while the year view is not generated.
            if (calendar._customScrollLayout == null || calendar.View != CalendarView.Month)
            {
                return;
            }

            if (calendar.NavigationDirection == CalendarNavigationDirection.Horizontal)
            {
                calendar._customScrollLayout.InvalidateMonthHeaderDraw();
            }
            else
            {
                //// Need to update the view header height while old and new month setting view header height changed,
                //// because new view header view not updated with new height value.
                if (oldSettings != null && newSettings != null && oldSettings.HeaderView.Height != newSettings.HeaderView.Height)
                {
                    calendar._layout.UpdateViewHeaderHeight(newSettings.HeaderView.GetViewHeaderHeight());
                }

                calendar._monthViewHeader?.InvalidateDrawMonthHeaderView();
            }

            if (oldSettings != null && newSettings != null && (oldSettings.NumberOfVisibleWeeks != newSettings.NumberOfVisibleWeeks || oldSettings.FirstDayOfWeek != newSettings.FirstDayOfWeek))
            {
                calendar._customScrollLayout.UpdateVisibleDateOnView();
            }
            else if (oldSettings != null && newSettings != null && oldSettings.SpecialDayPredicate != newSettings.SpecialDayPredicate)
            {
                calendar._customScrollLayout.CreateOrResetSpecialDates();
            }

            calendar._customScrollLayout.InvalidateMonthViewLayout();
            calendar._customScrollLayout.UpdateTemplateViews();
        }

        /// <summary>
        /// Method invokes on calendar selection shape changed.
        /// </summary>
        /// <param name="bindable">The calendar object.</param>
        /// <param name="oldValue">Old value of the property.</param>
        /// <param name="newValue">New value of the property.</param>
        static void OnSelectionShapeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfCalendar? calendar = bindable as SfCalendar;
            if (calendar == null || calendar._customScrollLayout == null)
            {
                return;
            }

            //// Need to update the calendar view(month or year) with hover view.
            calendar._customScrollLayout.InvalidateViewDraw();
        }

        /// <summary>
        /// Method invokes on enable swipe selection changed.
        /// </summary>
        /// <param name="bindable">The calendar object.</param>
        /// <param name="oldValue">Old value of the property.</param>
        /// <param name="newValue">New value of the property.</param>
        static void OnEnableSwipeSelectionChanged(BindableObject bindable, object oldValue, object newValue)
        {
        }

        /// <summary>
        /// Method invokes on year view settings changed.
        /// </summary>
        /// <param name="bindable">The calendar object.</param>
        /// <param name="oldValue">Old value of the property.</param>
        /// <param name="newValue">New value of the property.</param>
        static void OnYearViewChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfCalendar? calendar = bindable as SfCalendar;
            if (bindable is SfCalendar calendarView)
            {
                calendarView.SetParent(oldValue as Element, newValue as Element);
            }

            if (calendar == null)
            {
                return;
            }

            CalendarYearView? oldSettings = oldValue as CalendarYearView;
            if (oldSettings != null)
            {
                calendar.UnWireYearViewEvents(oldSettings);
            }

            CalendarYearView? newSettings = newValue as CalendarYearView;
            if (newSettings != null)
            {
                calendar.WireYearViewEvents(newSettings);
            }

            //// Skip the update while the year view is not generated.
            if (calendar._customScrollLayout == null || calendar.View == CalendarView.Month)
            {
                return;
            }

            calendar._customScrollLayout.UpdateTemplateViews();
        }

        /// <summary>
        /// Method invokes on selected date changed.
        /// </summary>
        /// <param name="bindable">The calendar object.</param>
        /// <param name="oldValue">Old value of the property.</param>
        /// <param name="newValue">New value of the property.</param>
        static void OnSelectedDateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfCalendar? calendar = bindable as SfCalendar;
            if (calendar == null)
            {
                return;
            }

            var selectionChangedEventArgs = new CalendarSelectionChangedEventArgs { NewValue = newValue, OldValue = oldValue };
            if (calendar.SelectionChanged != null)
            {
                calendar.SelectionChanged?.Invoke(calendar, selectionChangedEventArgs);
            }

            if (calendar.SelectionChangedCommand != null && calendar.SelectionChangedCommand.CanExecute(selectionChangedEventArgs))
            {
                calendar.SelectionChangedCommand.Execute(selectionChangedEventArgs);
            }

            if (calendar._customScrollLayout == null)
            {
                return;
            }

            calendar._customScrollLayout.UpdateSelection();
        }

        /// <summary>
        /// Method invokes on selected dates changed.
        /// </summary>
        /// <param name="bindable">The calendar object.</param>
        /// <param name="oldValue">Old value of the property.</param>
        /// <param name="newValue">New value of the property.</param>
        static void OnSelectedDatesChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfCalendar? calendar = bindable as SfCalendar;
            if (calendar == null)
            {
                return;
            }

            ObservableCollection<DateTime> newDates = (ObservableCollection<DateTime>)newValue;
            calendar._selectedDates = new ObservableCollection<DateTime>(newDates);
            ObservableCollection<DateTime> oldDates = (ObservableCollection<DateTime>)oldValue;
            oldDates.CollectionChanged -= calendar._proxy.OnSelectedDatesCollectionChanged;
            newDates.CollectionChanged += calendar._proxy.OnSelectedDatesCollectionChanged;
            ReadOnlyObservableCollection<DateTime> oldSelectedDates = new ReadOnlyObservableCollection<DateTime>(oldDates);
            ReadOnlyObservableCollection<DateTime> newSelectedDates = new ReadOnlyObservableCollection<DateTime>(newDates);
            calendar.SelectionChanged?.Invoke(calendar, new CalendarSelectionChangedEventArgs { NewValue = newSelectedDates, OldValue = oldSelectedDates });

            if (calendar._customScrollLayout == null)
            {
                return;
            }

            calendar._customScrollLayout.UpdateSelectedDates();
        }

        /// <summary>
        /// Method invokes on selected date range changed.
        /// </summary>
        /// <param name="bindable">The calendar object.</param>
        /// <param name="oldValue">Old value of the property.</param>
        /// <param name="newValue">New value of the property.</param>
        static void OnSelectedRangeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfCalendar? calendar = bindable as SfCalendar;
            if (calendar == null)
            {
                return;
            }

            if (oldValue != null)
            {
                CalendarDateRange? oldRange = oldValue as CalendarDateRange;
                if (oldRange != null)
                {
                    oldRange.CalendarPropertyChanged -= calendar.OnSelectedRangePropertyChanged;
                }
            }

            if (newValue != null)
            {
                CalendarDateRange? newRange = newValue as CalendarDateRange;
                if (newRange != null)
                {
                    newRange.CalendarPropertyChanged += calendar.OnSelectedRangePropertyChanged;
                }
            }

            if (calendar.SelectionMode == CalendarSelectionMode.Range)
            {
                var selectionChangedEventArgs = new CalendarSelectionChangedEventArgs { NewValue = newValue, OldValue = oldValue };
                if (calendar.SelectionChanged != null)
                {
                    calendar.SelectionChanged?.Invoke(calendar, selectionChangedEventArgs);
                }

                if (calendar.SelectionChangedCommand != null && calendar.SelectionChangedCommand.CanExecute(selectionChangedEventArgs))
                {
                    calendar.SelectionChangedCommand.Execute(selectionChangedEventArgs);
                }
            }

            calendar._customScrollLayout?.UpdateSelectionRange();
        }

        /// <summary>
        /// Method invokes on selected date ranges changed.
        /// </summary>
        /// <param name="bindable">The calendar object.</param>
        /// <param name="oldValue">Old value of the property.</param>
        /// <param name="newValue">New value of the property.</param>
        static void OnSelectedDateRangesChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfCalendar? calendar = bindable as SfCalendar;
            if (calendar == null)
            {
                return;
            }

            ObservableCollection<CalendarDateRange>? oldDateRangesValue = oldValue as ObservableCollection<CalendarDateRange>;
            ObservableCollection<CalendarDateRange>? newDateRangesValue = newValue as ObservableCollection<CalendarDateRange>;

            if (oldDateRangesValue != null)
            {
                foreach (CalendarDateRange range in oldDateRangesValue)
                {
                    range.CalendarPropertyChanged -= calendar.OnSelectedRangesPropertyChanged;
                }

                oldDateRangesValue.CollectionChanged -= calendar.OnSelectedDateRangesCollectionChanged;
            }

            if (newDateRangesValue != null)
            {
                // To wire the properties in each CalendarDateRange object
                foreach (CalendarDateRange range in newDateRangesValue)
                {
                    range.CalendarPropertyChanged += calendar.OnSelectedRangesPropertyChanged;
                }

                //// To update the new value for the selectedDateRanges private variable.
                calendar._selectedDateRanges = CalendarViewHelper.CloneSelectedRanges(newDateRangesValue);
                newDateRangesValue.CollectionChanged += calendar.OnSelectedDateRangesCollectionChanged;
            }

            //// To prevent external modification of the collection's elements
            //// By using ReadOnlyObservableCollection, ensures that the old value cannot be modified from outside the class
            ReadOnlyObservableCollection<CalendarDateRange>? oldSelectedDateRanges = oldDateRangesValue != null
                ? new ReadOnlyObservableCollection<CalendarDateRange>(oldDateRangesValue)
                : null;
            ReadOnlyObservableCollection<CalendarDateRange>? newSelectedDateRanges = newDateRangesValue != null
                ? new ReadOnlyObservableCollection<CalendarDateRange>(newDateRangesValue)
                : null;

            calendar.SelectionChanged?.Invoke(calendar, new CalendarSelectionChangedEventArgs { NewValue = newSelectedDateRanges, OldValue = oldSelectedDateRanges });
            calendar._customScrollLayout?.UpdateMultiRangeSelectionValue();
        }

        /// <summary>
        /// Method invokes on selection mode changed.
        /// </summary>
        /// <param name="bindable">The calendar object.</param>
        /// <param name="oldValue">Old value of the property.</param>
        /// <param name="newValue">New value of the property.</param>
        static void OnSelectionModeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfCalendar? calendar = bindable as SfCalendar;
            if (calendar == null || calendar._customScrollLayout == null)
            {
                return;
            }

            if (calendar.View == CalendarView.Month || !calendar.AllowViewNavigation)
            {
                calendar._customScrollLayout.InvalidateViewCellsDraw();
            }
        }

        /// <summary>
        /// Method invokes on toggle day selection changed.
        /// </summary>
        /// <param name="bindable">The calendar object.</param>
        /// <param name="oldValue">Old value of the property.</param>
        /// <param name="newValue">New value of the property.</param>
        static void OnToggleDaySelectionChanged(BindableObject bindable, object oldValue, object newValue)
        {
            return;
        }

        /// <summary>
        /// Method invokes on allow view navigation changed.
        /// </summary>
        /// <param name="bindable">The calendar object.</param>
        /// <param name="oldValue">Old value of the property.</param>
        /// <param name="newValue">New value of the property.</param>
        static void OnAllowViewNavigationChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfCalendar? calendar = bindable as SfCalendar;
            if (calendar == null || calendar._customScrollLayout == null)
            {
                return;
            }

            if (calendar.View == CalendarView.Month)
            {
                return;
            }

            calendar._customScrollLayout.InvalidateViewCellsDraw();
        }

        /// <summary>
        /// Method invokes on today highlight brush changed.
        /// </summary>
        /// <param name="bindable">The calendar object.</param>
        /// <param name="oldValue">Old value of the property.</param>
        /// <param name="newValue">New value of the property.</param>
        static void OnTodayHighlightBrushChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfCalendar? calendar = bindable as SfCalendar;

            if (calendar == null || calendar._customScrollLayout == null)
            {
                return;
            }

            //// Need to draw the month header while vertical navigation.
            calendar._monthViewHeader?.InvalidateDrawMonthHeaderView();
            //// Need to draw the month header while horizontal navigation.
            calendar._customScrollLayout.InvalidateMonthHeaderDraw();
            //// Need to draw the month cells while today highlight color changes.
            calendar._customScrollLayout.UpdateTodayCellStyle();
            //// Need to update the action button and today button text color while today highlight color changes.
            calendar._footerLayout?.UpdateIconTextColor();
        }

        /// <summary>
        /// Method invokes on selection background changed.
        /// </summary>
        /// <param name="bindable">The calendar object.</param>
        /// <param name="oldValue">Old value of the property.</param>
        /// <param name="newValue">New value of the property.</param>
        static void OnSelectionBackgroundChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfCalendar? calendar = bindable as SfCalendar;

            if (calendar == null || calendar._customScrollLayout == null)
            {
                return;
            }

            calendar._customScrollLayout.UpdateSelectedCellStyle();
        }

        /// <summary>
        /// Method invokes on start range selection background changed.
        /// </summary>
        /// <param name="bindable">The calendar object.</param>
        /// <param name="oldValue">Old value of the property.</param>
        /// <param name="newValue">New value of the property.</param>
        static void OnStartRangeSelectionBackgroundChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfCalendar? calendar = bindable as SfCalendar;

            if (calendar == null || calendar._customScrollLayout == null)
            {
                return;
            }

            if (calendar.SelectionMode != CalendarSelectionMode.Range)
            {
                return;
            }

            calendar._customScrollLayout.UpdateSelectedCellStyle();
        }

        /// <summary>
        /// Method invokes on end range selection background changed.
        /// </summary>
        /// <param name="bindable">The calendar object.</param>
        /// <param name="oldValue">Old value of the property.</param>
        /// <param name="newValue">New value of the property.</param>
        static void OnEndRangeSelectionBackgroundChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfCalendar? calendar = bindable as SfCalendar;

            if (calendar == null || calendar._customScrollLayout == null)
            {
                return;
            }

            if (calendar.SelectionMode != CalendarSelectionMode.Range)
            {
                return;
            }

            calendar._customScrollLayout.UpdateSelectedCellStyle();
        }

        /// <summary>
        /// Invokes on show leading and trailing dates changed.
        /// </summary>
        /// <param name="bindable">The month view settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnShowLeadingAndTrailingDatesChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfCalendar? calendar = bindable as SfCalendar;
            if (calendar == null || calendar._customScrollLayout == null)
            {
                return;
            }

            //// Here year view does not contains leading and trailing dates and leading and trailing dates can be enabled or disabled
            //// only when the number of visible weeks is 6, so we are restricted the view update for these cases.
            if (calendar.View == CalendarView.Year || (calendar.View == CalendarView.Month && calendar.MonthView.GetNumberOfWeeks() != 6))
            {
                return;
            }

            calendar.OnCalendarViewChanged(calendar._visibleDates, calendar.View, (bool)oldValue);
            calendar._customScrollLayout.UpdateShowLeadingDates();
        }

        /// <summary>
        /// Method invokes on selectable day predicate changed.
        /// </summary>
        /// <param name="bindable">The calendar object.</param>
        /// <param name="oldValue">Old value of the property.</param>
        /// <param name="newValue">New value of the property.</param>
        static void OnSelectableDayPredicateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfCalendar? calendar = bindable as SfCalendar;
            if (calendar == null || calendar._customScrollLayout == null)
            {
                return;
            }

            calendar._customScrollLayout.CreateOrResetDisableDates();
        }

        /// <summary>
        /// Method to invokes the range selection direction is changed.
        /// </summary>
        /// <param name="bindable">The calendar object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnRangeSelectionDirectionChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfCalendar? calendar = bindable as SfCalendar;
            if (calendar == null || calendar._customScrollLayout == null)
            {
                return;
            }

            calendar._customScrollLayout.InvalidateViewCellsDraw();
        }

        /// <summary>
        /// Method to invokes the calendar identifier is changed.
        /// </summary>
        /// <param name="bindable">The calendar object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnCalendarIdentifierChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfCalendar? calendar = bindable as SfCalendar;
            if (calendar == null || calendar._customScrollLayout == null)
            {
                calendar?.UpdateLayoutFlowDirection();
                return;
            }

            // Need to update the flow direction while changing the calendar identifier.
            // Because, the flow direction is based on the culture of the calendar identifier.
            calendar.UpdateFlowDirection();
            calendar._customScrollLayout.UpdateVisibleDateOnView();
        }

        /// <summary>
        /// Method invokes on show out of range dates property changed.
        /// </summary>
        /// <param name="bindable">The calendar object.</param>
        /// <param name="oldValue">Old value of the property.</param>
        /// <param name="newValue">New value of the property.</param>
        static void OnShowOutOfRangeDatesChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfCalendar? calendar = bindable as SfCalendar;
            if (calendar == null || calendar._customScrollLayout == null)
            {
                return;
            }

            calendar._customScrollLayout.UpdateVisibleDateOnView();
        }

        /// <summary>
        /// called when <see cref="CalendarBackground"/> property changed.
        /// </summary>
        /// <param name="bindable">The bindable object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        static void OnCalendarBackgroundChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfCalendar calendar)
            {
                calendar.BackgroundColor = calendar.CalendarBackground;
            }
        }

        /// <summary>
        /// called when <see cref="CornerRadius"/> property changed.
        /// </summary>
        /// <param name="bindable">The bindable object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        static void OnCornerRadiusChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfCalendar? calendar = bindable as SfCalendar;
            if (calendar == null)
            {
                return;
            }

            calendar.InvalidateDrawable();
        }

        /// <summary>
        /// Method invokes when header template changed.
        /// </summary>
        /// <param name="bindable">The header settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnHeaderTemplateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfCalendar? calendar = bindable as SfCalendar;
            if (calendar == null)
            {
                return;
            }

            calendar._headerLayout?.InvalidateHeaderTemplate();
        }

        /// <summary>
        /// Invokes on view header template property changed.
        /// </summary>
        /// <param name="bindable">The month view settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnMonthViewHeaderTemplateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfCalendar? calendar = bindable as SfCalendar;
            if (calendar == null)
            {
                return;
            }

            if (calendar.View != CalendarView.Month)
            {
                return;
            }

            calendar._monthViewHeader?.CreateViewHeaderTemplate();
        }

        /// <summary>
        /// Called when <see cref="IsOpen"/> property changed.
        /// </summary>
        /// <param name="bindable">The bindable.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        static void OnIsOpenPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfCalendar? calendar = bindable as SfCalendar;
            if (calendar == null)
            {
                return;
            }

            if ((bool)newValue && calendar.IsVisible && (calendar.Mode == CalendarMode.Dialog || calendar.Mode == CalendarMode.RelativeDialog))
            {
                if (calendar.Children.Count != 0 && calendar._layout != null && calendar._layout.Parent == calendar)
                {
                    calendar.Remove(calendar._layout);
                }

                calendar.AddCalendarToPopup();
                calendar.ShowPopup();
            }
            else
            {
                calendar.CloseCalendarPopup();
                if (calendar.Children.Count != 0 && calendar._layout != null && calendar._layout.Parent != calendar && calendar._layout.Parent is ICollection<View> view)
                {
                    view.Remove(calendar._layout);
                }
            }
        }

        /// <summary>
        /// Called when <see cref="Mode"/> property changed.
        /// </summary>
        /// <param name="bindable">The bindable.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        static void OnModeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfCalendar? calendar = bindable as SfCalendar;
            if (calendar == null)
            {
                return;
            }

            CalendarMode mode = CalendarMode.Default;
            if (newValue is CalendarMode value)
            {
                mode = value;
            }

            if (mode == CalendarMode.Dialog || mode == CalendarMode.RelativeDialog)
            {
                if (calendar.Children.Count != 0 && calendar._layout != null && calendar._layout.Parent == calendar)
                {
                    calendar.Remove(calendar._layout);
                }

                if (calendar.IsOpen)
                {
                    calendar.AddCalendarToPopup();
                    calendar.ShowPopup();
                }
                else
                {
                    calendar.CloseCalendarPopup();
                }
            }
            else
            {
                calendar.ResetPopup();
                if (calendar._layout != null)
                {
                    if (calendar._layout.Parent != null && calendar._layout.Parent is ICollection<IView> view)
                    {
                        view.Remove(calendar._layout);
                    }

                    calendar.Add(calendar._layout);
                }
            }

            calendar.InvalidateMeasure();
        }

        /// <summary>
        /// Called when <see cref="RelativePosition"/> property changed.
        /// </summary>
        /// <param name="bindable">The bindable.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        static void OnRelativePositionChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfCalendar? calendar = bindable as SfCalendar;
            if (calendar == null)
            {
                return;
            }

            if (calendar.IsOpen && calendar.Mode == CalendarMode.RelativeDialog)
            {
                calendar.ShowPopup();
            }
        }

#if WINDOWS
        /// <summary>
        /// Method invoke when flow direction property changed.
        /// </summary>
        /// <param name="bindable">The flow direction object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        static void OnFlowDirectionChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfCalendar? calendar = bindable as SfCalendar;
            if (calendar == null || calendar._customScrollLayout == null)
            {
                calendar?.UpdateLayoutFlowDirection();
                return;
            }

            calendar.UpdateFlowDirection();
            calendar._customScrollLayout.UpdateVisibleDateOnView();
        }
#endif

        /// <summary>
        /// Method to update the focus while view is changed on keyboard interaction.
        /// </summary>
        async void SetFocusOnViewNavigation(int delay)
        {
            await Task.Delay(delay);
            _customScrollLayout?.UpdateFocusOnViewNavigation();
        }

        /// <summary>
        /// Need to update the parent for the new value.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        void SetParent(Element? oldValue, Element? newValue)
        {
            if (oldValue != null)
            {
                oldValue.Parent = null;
            }

            if (newValue != null)
            {
                newValue.Parent = this;
            }
        }

        #endregion

        #region Events

#nullable disable

        /// <summary>
        /// Occurs whenever the calendar view and visible dates changed on SfCalendar.
        /// </summary>
        public event EventHandler<CalendarViewChangedEventArgs> ViewChanged;

        /// <summary>
        /// Occurs after the selection changed on SfCalendar.
        /// </summary>
        public event EventHandler<CalendarSelectionChangedEventArgs> SelectionChanged;

        /// <summary>
        /// Occurs after the tap interaction on SfCalendar.
        /// </summary>
        public event EventHandler<CalendarTappedEventArgs> Tapped;

        /// <summary>
        /// Occurs after the double tapped interaction on SfCalendar.
        /// </summary>
        public event EventHandler<CalendarDoubleTappedEventArgs> DoubleTapped;

        /// <summary>
        /// Occurs after the long press interaction on SfCalendar.
        /// </summary>
        public event EventHandler<CalendarLongPressedEventArgs> LongPressed;

        /// <summary>
        /// Occurs whenever the confirm button tapped on calendar. The date that have been selected are confirmed.
        /// </summary>
        public event EventHandler<CalendarSubmittedEventArgs> ActionButtonClicked;

        /// <summary>
        /// Occurs whenever the cancel button tapped on calendar. It reset the selected values to confirmed selected values.
        /// </summary>
        public event EventHandler ActionButtonCanceled;

        /// <summary>
        /// Occurs after the calendar popup is opened.
        /// </summary>
        public event EventHandler CalendarPopupOpened;

        /// <summary>
        /// Occurs when the calendar popup is closed.
        /// </summary>
        public event EventHandler CalendarPopupClosed;

        /// <summary>
        /// Occurs when the calendar popup is closing.
        /// </summary>
        public event EventHandler<CancelEventArgs> CalendarPopupClosing;

        /// <summary>
        /// Occurs when the calendar popup is opening.
        /// </summary>
        public event EventHandler<CancelEventArgs> CalendarPopupOpening;

#nullable enable

        #endregion
    }
#pragma warning restore CA1506

}