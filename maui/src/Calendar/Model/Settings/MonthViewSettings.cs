using Syncfusion.Maui.Toolkit.Themes;

namespace Syncfusion.Maui.Toolkit.Calendar
{
    /// <summary>
    /// Represents a class which is used to configure all the properties and styles of calendar month view.
    /// </summary>
    public class CalendarMonthView : Element, IThemeElement
    {
        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="WeekNumberStyle"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="WeekNumberStyle"/> dependency property.
        /// </value>
        public static readonly BindableProperty WeekNumberStyleProperty =
            BindableProperty.Create(
                nameof(WeekNumberStyle),
                typeof(CalendarWeekNumberStyle),
                typeof(CalendarMonthView),
                defaultValueCreator: bindable => new CalendarWeekNumberStyle()
                {
                    Parent = bindable as Element
                },
                propertyChanged: OnWeekNumberStyleChanged);

        /// <summary>
        /// Identifies the <see cref="NumberOfVisibleWeeks"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="NumberOfVisibleWeeks"/> dependency property.
        /// </value>
        public static readonly BindableProperty NumberOfVisibleWeeksProperty =
            BindableProperty.Create(
                nameof(NumberOfVisibleWeeks),
                typeof(int),
                typeof(CalendarMonthView),
                6,
                propertyChanged: NumberOfVisibleWeeksChanged);

        /// <summary>
        /// Identifies the <see cref="FirstDayOfWeek"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="FirstDayOfWeek"/> dependency property.
        /// </value>
        public static readonly BindableProperty FirstDayOfWeekProperty =
            BindableProperty.Create(
                nameof(FirstDayOfWeek),
                typeof(DayOfWeek),
                typeof(CalendarMonthView),
                DayOfWeek.Sunday,
                propertyChanged: OnFirstDayOfWeekChanged);

        /// <summary>
        /// Identifies the <see cref="ShowWeekNumber"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="ShowWeekNumber"/> dependency property.
        /// </value>
        public static readonly BindableProperty ShowWeekNumberProperty =
            BindableProperty.Create(
                nameof(ShowWeekNumber),
                typeof(bool),
                typeof(CalendarMonthView),
                false,
                propertyChanged: OnShowWeekNumberChanged);

        /// <summary>
        /// Identifies the <see cref="HeaderView"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="HeaderView"/> dependency property.
        /// </value>
        public static readonly BindableProperty HeaderViewProperty =
            BindableProperty.Create(
                nameof(HeaderView),
                typeof(CalendarMonthHeaderView),
                typeof(CalendarMonthView),
                defaultValueCreator: bindable => new CalendarMonthHeaderView()
                {
                    Parent = bindable as Element
                },
                propertyChanged: OnViewHeaderViewChanged);

        /// <summary>
        /// Identifies the <see cref="WeekendDays"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="WeekendDays"/> dependency property.
        /// </value>
        public static readonly BindableProperty WeekendDaysProperty =
            BindableProperty.Create(
                nameof(WeekendDays),
                typeof(List<DayOfWeek>),
                typeof(CalendarMonthView),
                new List<DayOfWeek>(),
                propertyChanged: OnWeekendDaysChanged);

        /// <summary>
        /// Identifies the <see cref="SpecialDatesBackground"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="SpecialDatesBackground"/> dependency property.
        /// </value>
        public static readonly BindableProperty SpecialDatesBackgroundProperty =
            BindableProperty.Create(
                nameof(SpecialDatesBackground),
                typeof(Brush),
                typeof(CalendarMonthView),
                defaultValueCreator: bindable => Brush.Transparent,
                propertyChanged: OnSpecialDatesBackgroundChanged);

        /// <summary>
        /// Identifies the <see cref="WeekendDatesBackground"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="WeekendDatesBackground"/> dependency property.
        /// </value>
        public static readonly BindableProperty WeekendDatesBackgroundProperty =
            BindableProperty.Create(
                nameof(WeekendDatesBackground),
                typeof(Brush),
                typeof(CalendarMonthView),
                defaultValueCreator: bindable => null,
                propertyChanged: OnWeekendsBackgroundChanged);

        /// <summary>
        /// Identifies the <see cref="DisabledDatesBackground"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="DisabledDatesBackground"/> dependency property.
        /// </value>
        public static readonly BindableProperty DisabledDatesBackgroundProperty =
            BindableProperty.Create(
                nameof(DisabledDatesBackground),
                typeof(Brush),
                typeof(CalendarMonthView),
                defaultValueCreator: bindable => Brush.Transparent,
                propertyChanged: OnDisabledDatesBackgroundChanged);

        /// <summary>
        /// Identifies the <see cref="Background"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Background"/> dependency property.
        /// </value>
        public static readonly BindableProperty BackgroundProperty =
            BindableProperty.Create(
                nameof(Background),
                typeof(Brush),
                typeof(CalendarMonthView),
                defaultValueCreator: bindable => Brush.Transparent,
                propertyChanged: OnBackgroundChanged);

        /// <summary>
        /// Identifies the <see cref="TodayBackground"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="TodayBackground"/> dependency property.
        /// </value>
        public static readonly BindableProperty TodayBackgroundProperty =
            BindableProperty.Create(
                nameof(TodayBackground),
                typeof(Brush),
                typeof(CalendarMonthView),
                defaultValueCreator: bindable => Brush.Transparent,
                propertyChanged: OnTodayBackgroundChanged);

        /// <summary>
        /// Identifies the <see cref="TrailingLeadingDatesBackground"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="TrailingLeadingDatesBackground"/> dependency property.
        /// </value>
        public static readonly BindableProperty TrailingLeadingDatesBackgroundProperty =
            BindableProperty.Create(
                nameof(TrailingLeadingDatesBackground),
                typeof(Brush),
                typeof(CalendarMonthView),
                defaultValueCreator: bindable => Brush.Transparent,
                propertyChanged: OnTrailingLeadingDatesBackgroundChanged);

        /// <summary>
        /// Identifies the <see cref="TextStyle"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="TextStyle"/> dependency property.
        /// </value>
        public static readonly BindableProperty TextStyleProperty =
            BindableProperty.Create(
                nameof(TextStyle),
                typeof(CalendarTextStyle),
                typeof(CalendarMonthView),
                defaultValueCreator: bindable => new CalendarTextStyle()
                {
                    Parent = bindable as Element,
                    FontSize = CalendarFonts.GetMonthBodyTextSize(),
                    TextColor = CalendarColors.GetOnSecondaryVariantColor()
                },
                propertyChanged: OnTextStyleChanged);

        /// <summary>
        /// Identifies the <see cref="TodayTextStyle"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="TodayTextStyle"/> dependency property.
        /// </value>
        public static readonly BindableProperty TodayTextStyleProperty =
            BindableProperty.Create(
                nameof(TodayTextStyle),
                typeof(CalendarTextStyle),
                typeof(CalendarMonthView),
                defaultValueCreator: bindable => new CalendarTextStyle()
                {
                    Parent = bindable as Element,
                    FontSize = CalendarFonts.GetMonthBodyTextSize(),
                    TextColor = CalendarColors.GetPrimaryColor()
                },
                propertyChanged: OnTodayTextStyleChanged);

        /// <summary>
        /// Identifies the <see cref="TrailingLeadingDatesTextStyle"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="TrailingLeadingDatesTextStyle"/> dependency property.
        /// </value>
        public static readonly BindableProperty TrailingLeadingDatesTextStyleProperty =
            BindableProperty.Create(
                nameof(TrailingLeadingDatesTextStyle),
                typeof(CalendarTextStyle),
                typeof(CalendarMonthView),
                defaultValueCreator: bindable => new CalendarTextStyle()
                {
                    Parent = bindable as Element,
                    TextColor = CalendarColors.GetOnSecondaryColor(),
                    FontSize = CalendarFonts.GetMonthBodyTextSize()
                },
                propertyChanged: OnTrailingLeadingDatesTextStyleChanged);

        /// <summary>
        /// Identifies the <see cref="DisabledDatesTextStyle"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="DisabledDatesTextStyle"/> dependency property.
        /// </value>
        public static readonly BindableProperty DisabledDatesTextStyleProperty =
            BindableProperty.Create(
                nameof(DisabledDatesTextStyle),
                typeof(CalendarTextStyle),
                typeof(CalendarMonthView),
                defaultValueCreator: bindable => new CalendarTextStyle()
                {
                    Parent = bindable as Element,
                    FontSize = CalendarFonts.GetMonthBodyTextSize(),
                    TextColor = CalendarColors.GetDisabledColor()
                },
                propertyChanged: OnDisabledDatesTextStyleChanged);

        /// <summary>
        /// Identifies the <see cref="WeekendDatesTextStyle"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="WeekendDatesTextStyle"/> dependency property.
        /// </value>
        public static readonly BindableProperty WeekendDatesTextStyleProperty =
            BindableProperty.Create(
                nameof(WeekendDatesTextStyle),
                typeof(CalendarTextStyle),
                typeof(CalendarMonthView),
                defaultValueCreator: bindable => null,
                propertyChanged: OnWeekendDatesTextStyleChanged);

        /// <summary>
        /// Identifies the <see cref="SpecialDatesTextStyle"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="SpecialDatesTextStyle"/> dependency property.
        /// </value>
        public static readonly BindableProperty SpecialDatesTextStyleProperty =
            BindableProperty.Create(
                nameof(SpecialDatesTextStyle),
                typeof(CalendarTextStyle),
                typeof(CalendarMonthView),
                defaultValueCreator: bindable => new CalendarTextStyle()
                {
                    Parent = bindable as Element,
                    FontSize = CalendarFonts.GetMonthBodyTextSize(),
                    TextColor = CalendarColors.GetOnSecondaryVariantColor()
                },
                propertyChanged: OnSpecialDatesTextStyleChanged);

        /// <summary>
        /// Identifies the <see cref="RangeTextStyle"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="RangeTextStyle"/> dependency property.
        /// </value>
        public static readonly BindableProperty RangeTextStyleProperty =
            BindableProperty.Create(
                nameof(RangeTextStyle),
                typeof(CalendarTextStyle),
                typeof(CalendarMonthView),
                defaultValueCreator: bindable => new CalendarTextStyle()
                {
                    Parent = bindable as Element,
                    FontSize = CalendarFonts.GetMonthBodyTextSize(),
                    TextColor = CalendarColors.GetOnSecondaryColor()
                },
                propertyChanged: OnRangeTextStyleChanged);

        /// <summary>
        /// Identifies the <see cref="SelectionTextStyle"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="SelectionTextStyle"/> dependency property.
        /// </value>
        public static readonly BindableProperty SelectionTextStyleProperty =
            BindableProperty.Create(
                nameof(SelectionTextStyle),
                typeof(CalendarTextStyle),
                typeof(CalendarMonthView),
                defaultValueCreator: bindable => new CalendarTextStyle()
                {
                    Parent = bindable as Element,
                    FontSize = CalendarFonts.GetMonthBodyTextSize(),
                    TextColor = CalendarColors.GetOnPrimaryColor()
                },
                propertyChanged: OnSelectionTextStyleChanged);

        /// <summary>
        /// Identifies the <see cref="CellTemplate"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="CellTemplate"/> dependency property.
        /// </value>
        public static readonly BindableProperty CellTemplateProperty =
            BindableProperty.Create(
                nameof(CellTemplate),
                typeof(DataTemplate),
                typeof(CalendarMonthView),
                null,
                propertyChanged: OnCellTemplateChanged);

        /// <summary>
        /// Identifies the <see cref="SpecialDayPredicate"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="SpecialDayPredicate"/> dependency property.
        /// </value>
        public static readonly BindableProperty SpecialDayPredicateProperty =
            BindableProperty.Create(
                nameof(SpecialDayPredicate),
                typeof(Func<DateTime, CalendarIconDetails>),
                typeof(CalendarMonthView),
                null,
                propertyChanged: OnSpecialDayPredicateChanged);

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarMonthView"/> class.
        /// </summary>
        public CalendarMonthView()
        {
            ThemeElement.InitializeThemeResources(this, "SfCalendarTheme");
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the properties which allows to customize the week number view of the calendar month view.
        /// </summary>
        /// <seealso cref="SfCalendar.MonthView"/>
        /// <example>
        /// The following code demonstrates, how to use the WeekNumberStyle property in the calendar
        /// <code Lang="C#"><![CDATA[
        /// this.Calendar.MonthView.WeekNumberStyle = new CalendarWeekNumberStyle()
        /// {
        ///     Background = Colors.Grey,
        /// };
        /// ]]></code>
        /// </example>
        public CalendarWeekNumberStyle WeekNumberStyle
        {
            get { return (CalendarWeekNumberStyle)GetValue(WeekNumberStyleProperty); }
            set { SetValue(WeekNumberStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value to display the number of weeks in calendar month view.
        /// </summary>
        /// <value>The default value of <see cref="CalendarMonthView.NumberOfVisibleWeeks"/> is 6.</value>
        /// <remarks>
        /// Leading and trailing dates will be considered only when the <see cref="CalendarMonthView.NumberOfVisibleWeeks"/> is 6.
        /// <see cref="SfCalendar.ShowTrailingAndLeadingDates"/> property will not applicable, when <see cref="CalendarMonthView.NumberOfVisibleWeeks"/> is not 6.
        /// </remarks>
        /// <seealso cref="SfCalendar.MonthView"/>
        /// <example>
        /// The following code demonstrates, how to use the NumberOfVisibleWeeks property in the calendar
        /// <code Lang="C#"><![CDATA[
        /// this.Calendar.MonthView.NumberOfVisibleWeeks = 6;
        /// ]]></code>
        /// </example>
        public int NumberOfVisibleWeeks
        {
            get { return (int)GetValue(NumberOfVisibleWeeksProperty); }
            set { SetValue(NumberOfVisibleWeeksProperty, value); }
        }

        /// <summary>
        /// Gets or sets the day of week that used to change the default first day of week in SfCalendar.
        /// </summary>
        /// <value>The default value of <see cref="CalendarMonthView.FirstDayOfWeek"/> is <see cref="DayOfWeek.Sunday"/>.</value>
        /// <seealso cref="SfCalendar.MonthView"/>
        /// <example>
        /// The following code demonstrates, how to use the FirstDayOfWeek property in the calendar
        /// <code Lang="C#"><![CDATA[
        /// this.Calendar.MonthView.FirstDayOfWeek = Sunday;
        /// ]]></code>
        /// </example>
        public DayOfWeek FirstDayOfWeek
        {
            get { return (DayOfWeek)GetValue(FirstDayOfWeekProperty); }
            set { SetValue(FirstDayOfWeekProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to displays the week number of the year in the SfCalendar.
        /// Week number value calculated based on the full week rule with Monday as first day of week.
        /// </summary>
        /// <value>The default value of <see cref="CalendarMonthView.ShowWeekNumber"/> is false.</value>
        /// <seealso cref="SfCalendar.MonthView"/>
        /// <seealso cref="CalendarMonthView.WeekNumberStyle"/>
        /// <example>
        /// The following code demonstrates, how to use the ShowWeekNumber property in the calendar
        /// <code Lang="C#"><![CDATA[
        /// this.Calendar.MonthView.ShowWeekNumber = false;
        /// ]]></code>
        /// </example>
        public bool ShowWeekNumber
        {
            get { return (bool)GetValue(ShowWeekNumberProperty); }
            set { SetValue(ShowWeekNumberProperty, value); }
        }

        /// <summary>
        /// Gets or sets properties which allows to customize the view header view of the calendar month view.
        /// </summary>
        /// <seealso cref="SfCalendar.MonthView"/>
        /// <example>
        /// The following code demonstrates, how to use the HeaderView property in the calendar
        /// <code Lang="C#"><![CDATA[
        /// this.calendar.MonthView.HeaderView = new CalendarMonthHeaderView()
        /// {
        /// 	Background = Colors.Yellow,
        /// 	TextFormat = "ddd"
        /// };
        /// ]]></code>
        /// </example>
        public CalendarMonthHeaderView HeaderView
        {
            get { return (CalendarMonthHeaderView)GetValue(HeaderViewProperty); }
            set { SetValue(HeaderViewProperty, value); }
        }

        /// <summary>
        /// Gets or sets the weekend days collection of the calendar month view.
        /// </summary>
        /// <value>The default value of <see cref="CalendarMonthView.WeekendDays"/> is empty.</value>
        /// <seealso cref="SfCalendar.MonthView"/>
        /// <seealso cref="CalendarMonthView.WeekendDatesBackground"/>
        /// <seealso cref="CalendarMonthView.WeekendDatesTextStyle"/>
        /// <example>
        /// The following code demonstrates, how to use the WeekendDays property in the calendar
        /// <code Lang="C#"><![CDATA[
        /// this.Calendar.MonthView.WeekendDays = new List<DayOfWeek>
        /// {
        ///    DayOfWeek.Sunday,
        ///    DayOfWeek.Saturday,
        /// };
        /// ]]></code>
        /// </example>
        public List<DayOfWeek> WeekendDays
        {
            get { return (List<DayOfWeek>)GetValue(WeekendDaysProperty); }
            set { SetValue(WeekendDaysProperty, value); }
        }

        /// <summary>
        /// Gets or sets the background for the special month cells of the calendar month view.
        /// </summary>
        /// <value> The default value of <see cref="CalendarMonthView.SpecialDatesBackground"/> is Transparent</value>
        /// <seealso cref="SfCalendar.MonthView"/>
        /// <seealso cref="CalendarMonthView.SpecialDatesTextStyle"/>
        /// <example>
        /// The following code demonstrates, how to use the SpecialDatesBackground property in the calendar
        /// <code Lang="C#"><![CDATA[
        /// this.Calendar.MonthView.SpecialDatesBackground = Colors.Orange;
        /// ]]></code>
        /// </example>
        public Brush SpecialDatesBackground
        {
            get { return (Brush)GetValue(SpecialDatesBackgroundProperty); }
            set { SetValue(SpecialDatesBackgroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets the background for the weekend month cells of the calendar month view.
        /// </summary>
        /// <value> The default value of <see cref="CalendarMonthView.WeekendDatesBackground"/> is null</value>
        /// <remarks> If the value is null, then the weekend dates are not highlighted.</remarks>
        /// <seealso cref="SfCalendar.MonthView"/>
        /// <seealso cref="CalendarMonthView.WeekendDatesTextStyle"/>
        /// <example>
        /// The following code demonstrates, how to use the WeekendDatesBackground property in the calendar
        /// <code Lang="C#"><![CDATA[
        /// this.Calendar.MonthView.WeekendDatesBackground = Colors.PaleVioletRed;
        /// ]]></code>
        /// </example>
        public Brush WeekendDatesBackground
        {
            get { return (Brush)GetValue(WeekendDatesBackgroundProperty); }
            set { SetValue(WeekendDatesBackgroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets the background for the disabled month cells of the calendar month view.
        /// </summary>
        /// <value> The default value of <see cref="CalendarMonthView.DisabledDatesBackground"/> is Transparent. </value>
        /// <seealso cref="SfCalendar.MonthView"/>
        /// <seealso cref="SfCalendar.MinimumDate"/>
        /// <seealso cref="SfCalendar.MaximumDate"/>
        /// <seealso cref="SfCalendar.EnablePastDates"/>
        /// <seealso cref="SfCalendar.SelectableDayPredicate"/>
        /// <seealso cref="CalendarMonthView.DisabledDatesTextStyle"/>
        /// <example>
        /// The following code demonstrates, how to use the DisabledDatesBackground property in the calendar
        /// <code Lang="C#"><![CDATA[
        /// this.Calendar.MonthView.DisabledDatesBackground = Colors.Grey;
        /// ]]></code>
        /// </example>
        public Brush DisabledDatesBackground
        {
            get { return (Brush)GetValue(DisabledDatesBackgroundProperty); }
            set { SetValue(DisabledDatesBackgroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets the background for the month cells of the calendar month view.
        /// </summary>
        /// <example>
        /// <value> The default value of <see cref="CalendarMonthView.Background"/> is Transparent. </value>
        /// <seealso cref="SfCalendar.MonthView"/>
        /// <seealso cref="CalendarMonthView.TextStyle"/>
        /// The following code demonstrates, how to use the Background property in the calendar
        /// <code Lang="C#"><![CDATA[
        /// this.Calendar.MonthView.Background = Colors.PaleGreen;
        /// ]]></code>
        /// </example>
        public Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets the background for the today month cell of the calendar month view.
        /// </summary>
        /// <value> The default value of <see cref="CalendarYearView.TodayBackground"/> is Transparent. </value>
        /// <seealso cref="SfCalendar.MonthView"/>
        /// <seealso cref="CalendarMonthView.TodayTextStyle"/>
        /// <example>
        /// The following code demonstrates, how to use the TodayBackground property in the calendar
        /// <code Lang="C#"><![CDATA[
        /// this.Calendar.MonthView.TodayBackground = Colors.Pink;
        /// ]]></code>
        /// </example>
        public Brush TodayBackground
        {
            get { return (Brush)GetValue(TodayBackgroundProperty); }
            set { SetValue(TodayBackgroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets the background for the trailing and leading month cells of the calendar month view.
        /// </summary>
        /// <seealso cref="SfCalendar.MonthView"/>
        /// <seealso cref="SfCalendar.ShowTrailingAndLeadingDates"/>
        /// <seealso cref="CalendarMonthView.TrailingLeadingDatesTextStyle"/>
        /// <example>
        /// The following code demonstrates, how to use the TrailingLeadingDatesBackground property in the calendar
        /// <code Lang="C#"><![CDATA[
        /// this.Calendar.MonthView.TrailingLeadingDatesBackground = Colors.Red;
        /// ]]></code>
        /// </example>
        public Brush TrailingLeadingDatesBackground
        {
            get { return (Brush)GetValue(TrailingLeadingDatesBackgroundProperty); }
            set { SetValue(TrailingLeadingDatesBackgroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets the text style of the month cells text, that used to customize the text color, font, font size, font family and font attributes.
        /// </summary>
        /// <seealso cref="SfCalendar.MonthView"/>
        /// <seealso cref="CalendarMonthView.Background"/>
        /// <example>
        /// The following code demonstrates, how to use the TextStyle property in the calendar
        /// <code Lang="C#"><![CDATA[
        /// CalendarTextStyle textStyle = new CalendarTextStyle()
        /// {
        ///     FontSize = 12,
        ///     TextColor = Colors.Red,
        /// };
        /// this.Calendar.MonthView.TextStyle = textStyle;
        /// ]]></code>
        /// </example>
        public CalendarTextStyle TextStyle
        {
            get { return (CalendarTextStyle)GetValue(TextStyleProperty); }
            set { SetValue(TextStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the text style of the today month cell text, that used to customize the text color, font, font size, font family and font attributes.
        /// </summary>
        /// <seealso cref="SfCalendar.MonthView"/>
        /// <seealso cref="CalendarMonthView.TodayBackground"/>
        /// <example>
        /// The following code demonstrates, how to use the TodayTextStyle property in the calendar
        /// <code Lang="C#"><![CDATA[
        /// CalendarTextStyle textStyle = new CalendarTextStyle()
        /// {
        ///     FontSize = 12,
        ///     TextColor = Colors.Red,
        /// };
        /// this.Calendar.MonthView.TodayTextStyle = textStyle;
        /// ]]></code>
        /// </example>
        public CalendarTextStyle TodayTextStyle
        {
            get { return (CalendarTextStyle)GetValue(TodayTextStyleProperty); }
            set { SetValue(TodayTextStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the text style of the trailing and leading month cells text, that used to customize the text color, font, font size, font family and font attributes.
        /// </summary>
        /// <seealso cref="SfCalendar.ShowTrailingAndLeadingDates"/>
        /// <seealso cref="SfCalendar.MonthView"/>
        /// <seealso cref="CalendarMonthView.TrailingLeadingDatesBackground"/>
        /// <example>
        /// The following code demonstrates, how to use the TrailingLeadingDatesTextStyle property in the calendar
        /// <code Lang="C#"><![CDATA[
        /// CalendarTextStyle textStyle = new CalendarTextStyle()
        /// {
        ///     FontSize = 12,
        ///     TextColor = Colors.Red,
        /// };
        /// this.Calendar.MonthView.TrailingLeadingDatesTextStyle = textStyle;
        /// ]]></code>
        /// </example>
        public CalendarTextStyle TrailingLeadingDatesTextStyle
        {
            get { return (CalendarTextStyle)GetValue(TrailingLeadingDatesTextStyleProperty); }
            set { SetValue(TrailingLeadingDatesTextStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the text style of the disabled month cells text, that used to customize the text color, font, font size, font family and font attributes.
        /// </summary>
        /// <seealso cref="SfCalendar.MaximumDate"/>
        /// <seealso cref="SfCalendar.MinimumDate"/>
        /// <seealso cref="SfCalendar.EnablePastDates"/>
        /// <seealso cref="SfCalendar.SelectableDayPredicate"/>
        /// <seealso cref="SfCalendar.MonthView"/>
        /// <seealso cref="CalendarMonthView.DisabledDatesBackground"/>
        /// <example>
        /// The following code demonstrates, how to use the DisabledDatesTextStyle property in the calendar
        /// <code Lang="C#"><![CDATA[
        /// CalendarTextStyle textStyle = new CalendarTextStyle()
        /// {
        ///     FontSize = 12,
        ///     TextColor = Colors.Red,
        /// };
        /// this.Calendar.MonthView.DisabledDatesTextStyle = textStyle;
        /// ]]></code>
        /// </example>
        public CalendarTextStyle DisabledDatesTextStyle
        {
            get { return (CalendarTextStyle)GetValue(DisabledDatesTextStyleProperty); }
            set { SetValue(DisabledDatesTextStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the text style of the weekend month cells text, that used to customize the text color, font, font size, font family and font attributes.
        /// </summary>
        /// <value> The default value of <see cref="CalendarMonthView.WeekendDatesTextStyle"/> is null.</value>
        /// <seealso cref="SfCalendar.MonthView"/>
        /// <seealso cref="CalendarMonthView.WeekendDatesBackground"/>
        /// <example>
        /// The following code demonstrates, how to use the WeekendDatesTextStyle property in the calendar
        /// <code Lang="C#"><![CDATA[
        /// CalendarTextStyle textStyle = new CalendarTextStyle()
        /// {
        ///     FontSize = 12,
        ///     TextColor = Colors.Red,
        /// };
        /// this.Calendar.MonthView.WeekendDatesTextStyle = textStyle;
        /// ]]></code>
        /// </example>
        public CalendarTextStyle WeekendDatesTextStyle
        {
            get { return (CalendarTextStyle)GetValue(WeekendDatesTextStyleProperty); }
            set { SetValue(WeekendDatesTextStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the text style of the special month cells text, that used to customize the text color, font, font size, font family and font attributes.
        /// </summary>
        /// <seealso cref="SfCalendar.MonthView"/>
        /// <seealso cref="CalendarMonthView.SpecialDatesBackground"/>
        /// <example>
        /// The following code demonstrates, how to use the SpecialDatesTextStyle property in the calendar
        /// <code Lang="C#"><![CDATA[
        /// CalendarTextStyle textStyle = new CalendarTextStyle()
        /// {
        ///     FontSize = 12,
        ///     TextColor = Colors.Red,
        /// };
        /// this.Calendar.MonthView.SpecialDatesTextStyle = textStyle;
        /// ]]></code>
        /// </example>
        public CalendarTextStyle SpecialDatesTextStyle
        {
            get { return (CalendarTextStyle)GetValue(SpecialDatesTextStyleProperty); }
            set { SetValue(SpecialDatesTextStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the text style of the range in-between month cells text, that used to customize the text color, font, font size, font family and font attributes.
        /// </summary>
        /// <seealso cref="CalendarSelectionMode.Range"/>
        /// <seealso cref="SfCalendar.MonthView"/>
        /// <seealso cref="SfCalendar.SelectionBackground"/>
        /// <example>
        /// The following code demonstrates, how to use the RangeTextStyle property in the calendar
        /// <code Lang="C#"><![CDATA[
        /// CalendarTextStyle textStyle = new CalendarTextStyle()
        /// {
        ///     FontSize = 12,
        ///     TextColor = Colors.Red,
        /// };
        /// this.Calendar.MonthView.RangeTextStyle = textStyle;
        /// ]]></code>
        /// </example>
        public CalendarTextStyle RangeTextStyle
        {
            get { return (CalendarTextStyle)GetValue(RangeTextStyleProperty); }
            set { SetValue(RangeTextStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the text style of the selection month cells text, that used to customize the text color, font, font size, font family and font attributes.
        /// </summary>
        /// <seealso cref="SfCalendar.SelectionMode"/>
        /// <seealso cref="SfCalendar.MonthView"/>
        /// <seealso cref="SfCalendar.SelectionBackground"/>
        /// <seealso cref="SfCalendar.StartRangeSelectionBackground"/>
        /// <seealso cref="SfCalendar.EndRangeSelectionBackground"/>
        /// <example>
        /// The following code demonstrates, how to use the SelectionTextStyle property in the calendar
        /// <code Lang="C#"><![CDATA[
        /// CalendarTextStyle textStyle = new CalendarTextStyle()
        /// {
        ///     FontSize = 12,
        ///     TextColor = Colors.Red,
        /// };
        /// this.Calendar.MonthView.SelectionTextStyle = textStyle;
        /// ]]></code>
        /// </example>
        public CalendarTextStyle SelectionTextStyle
        {
            get { return (CalendarTextStyle)GetValue(SelectionTextStyleProperty); }
            set { SetValue(SelectionTextStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the month cell template or template selector.
        /// </summary>
        /// <value> The default value of <see cref="CalendarMonthView.CellTemplate"/> is null. </value>
        /// <remarks>
        /// The BindingContext of the <see cref="CalendarMonthView.CellTemplate"/> is the <see cref="CalendarCellDetails" />
        /// If the cell template is not null, Range selection direction of forward, backward and both direction does not drawn the dashed line hovering.
        /// Hovering draws like default range selection direction.
        /// </remarks>
        /// <seealso cref="CalendarCellDetails.Date"/>
        /// <seealso cref="CalendarCellDetails.IsTrailingOrLeadingDate"/>
        /// <seealso cref="SfCalendar.SelectionMode"/>
        /// <example>
        /// The following code demonstrates, how to use the Cell Template property in the month view.
        /// #[XAML](#tab/tabid-1)
        /// <code Lang="XAML"><![CDATA[
        /// <calendar:SfCalendar x:Name= "Calendar">
        ///              < calendar:SfCalendar.MonthView>
        ///                  <calendar:CalendarMonthView>
        ///                      <calendar:CalendarMonthView.CellTemplate>
        ///                          <DataTemplate>
        ///                              <Grid BackgroundColor = "Pink" >
        ///                                  < Label HorizontalTextAlignment= "Center" VerticalTextAlignment= "Center" TextColor= "Purple" Text= "{Binding Date.Day}" />
        ///                              </Grid >
        ///                          </ DataTemplate >
        ///                      </ calendar:CalendarMonthView.CellTemplate>
        ///                  </calendar:CalendarMonthView>
        ///              </calendar:SfCalendar.MonthView>
        /// </calendar:SfCalendar>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code Lang="C#"><![CDATA[
        /// this.Calendar.MonthView = new CalendarMonthView()
        /// {
        ///     CellTemplate = dataTemplate,
        /// };
        ///
        /// DataTemplate dataTemplate = new DataTemplate(() =>
        /// {
        ///      Grid grid = new Grid
        ///      {
        ///          BackgroundColor = Colors.Pink,
        ///      };
        ///
        ///      Label label = new Label
        ///      {
        ///          HorizontalTextAlignment = TextAlignment.Center,
        ///          VerticalTextAlignment = TextAlignment.Center,
        ///          TextColor = Colors.Purple,
        ///      };
        ///
        ///      label.SetBinding(Label.TextProperty, new Binding("Date.Day"));
        ///      grid.Children.Add(label);
        ///      return grid;
        ///  });
        /// ]]></code>
        /// </example>
        public DataTemplate CellTemplate
        {
            get { return (DataTemplate)GetValue(CellTemplateProperty); }
            set { SetValue(CellTemplateProperty, value); }
        }

        /// <summary>
        /// Gets or sets the function to decide whether the month cell date is a special date or not in the calendar.
        /// </summary>
        /// <seealso cref="CalendarMonthView.SpecialDatesBackground"/>
        /// <seealso cref="CalendarMonthView.SpecialDatesTextStyle"/>
        /// <example>
        /// The following code demonstrates, how to use the SpecialDayPredicate function.
        /// <code Lang="C#"><![CDATA[
        ///  this.calendar.MonthView.SpecialDayPredicate = (date) =>
        ///  {
        ///    if (date.Date == DateTime.Now.AddDays(2).Date)
        ///    {
        ///      CalendarIconDetails iconDetails = new CalendarIconDetails();
        ///      iconDetails.Icon = CalendarIcon.Dot;
        ///      iconDetails.Fill = Colors.Red;
        ///      return iconDetails;
        ///     }
        ///     return null;
        ///  };
        /// ]]></code>
        /// </example>
        public Func<DateTime, CalendarIconDetails> SpecialDayPredicate
        {
            get { return (Func<DateTime, CalendarIconDetails>)GetValue(SpecialDayPredicateProperty); }
            set { SetValue(SpecialDayPredicateProperty, value); }
        }

        #endregion

        #region Property Changed Methods

        /// <summary>
        /// Invokes on week number style property changed.
        /// </summary>
        /// <param name="bindable">The month view settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnWeekNumberStyleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as CalendarMonthView)?.RaisePropertyChanged(nameof(WeekNumberStyle), oldValue);
            if (bindable is CalendarMonthView calendarMonthView)
            {
                calendarMonthView.SetParent(oldValue as Element, newValue as Element);
            }
        }

        /// <summary>
        /// Invokes on number of weeks value changed.
        /// </summary>
        /// <param name="bindable">The month view settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void NumberOfVisibleWeeksChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as CalendarMonthView)?.RaisePropertyChanged(nameof(NumberOfVisibleWeeks), oldValue);
        }

        /// <summary>
        /// Method invokes on first day of week changed.
        /// </summary>
        /// <param name="bindable">The month view settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnFirstDayOfWeekChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as CalendarMonthView)?.RaisePropertyChanged(nameof(FirstDayOfWeek));
        }

        /// <summary>
        /// Method invokes on show week number changed.
        /// </summary>
        /// <param name="bindable">The month view settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnShowWeekNumberChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as CalendarMonthView)?.RaisePropertyChanged(nameof(ShowWeekNumber));
        }

        /// <summary>
        /// Method invokes on view header view changed.
        /// </summary>
        /// <param name="bindable">The month view settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnViewHeaderViewChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as CalendarMonthView)?.RaisePropertyChanged(nameof(HeaderView));
            if (bindable is CalendarMonthView calendarMonthView)
            {
                calendarMonthView.SetParent(oldValue as Element, newValue as Element);
            }
        }

        /// <summary>
        /// Invokes on weekend days changed.
        /// </summary>
        /// <param name="bindable">The month view settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnWeekendDaysChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as CalendarMonthView)?.RaisePropertyChanged(nameof(WeekendDays));
        }

        /// <summary>
        /// Invokes on month cell style special dates background changed.
        /// </summary>
        /// <param name="bindable">The month view settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnSpecialDatesBackgroundChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as CalendarMonthView)?.RaisePropertyChanged(nameof(SpecialDatesBackground));
        }

        /// <summary>
        /// Invokes on month cell style weekend dates background changed.
        /// </summary>
        /// <param name="bindable">The month view settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnWeekendsBackgroundChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as CalendarMonthView)?.RaisePropertyChanged(nameof(WeekendDatesBackground));
        }

        /// <summary>
        /// Invokes on month cell style disabled dates background changed.
        /// </summary>
        /// <param name="bindable">The month view settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnDisabledDatesBackgroundChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as CalendarMonthView)?.RaisePropertyChanged(nameof(DisabledDatesBackground));
        }

        /// <summary>
        /// Invokes on month cell style background changed.
        /// </summary>
        /// <param name="bindable">The month view settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnBackgroundChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as CalendarMonthView)?.RaisePropertyChanged(nameof(Background));
        }

        /// <summary>
        /// Invokes on month cell style today background changed.
        /// </summary>
        /// <param name="bindable">The month view settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnTodayBackgroundChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as CalendarMonthView)?.RaisePropertyChanged(nameof(TodayBackground));
        }

        /// <summary>
        /// Invokes on trailing and leading month cell background changed.
        /// </summary>
        /// <param name="bindable">The month view settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnTrailingLeadingDatesBackgroundChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as CalendarMonthView)?.RaisePropertyChanged(nameof(TrailingLeadingDatesBackground));
        }

        /// <summary>
        /// Invokes on month cell text style changed.
        /// </summary>
        /// <param name="bindable">The month view settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnTextStyleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as CalendarMonthView)?.RaisePropertyChanged(nameof(TextStyle), oldValue);
            if (bindable is CalendarMonthView calendarMonthView)
            {
                calendarMonthView.SetParent(oldValue as Element, newValue as Element);
            }
        }

        /// <summary>
        /// Invokes on month cell today text style changed.
        /// </summary>
        /// <param name="bindable">The month view settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnTodayTextStyleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as CalendarMonthView)?.RaisePropertyChanged(nameof(TodayTextStyle), oldValue);
            if (bindable is CalendarMonthView calendarMonthView)
            {
                calendarMonthView.SetParent(oldValue as Element, newValue as Element);
            }
        }

        /// <summary>
        /// Invokes on trailing and leading month cell text style changed.
        /// </summary>
        /// <param name="bindable">The month view settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnTrailingLeadingDatesTextStyleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as CalendarMonthView)?.RaisePropertyChanged(nameof(TrailingLeadingDatesTextStyle), oldValue);
            if (bindable is CalendarMonthView calendarMonthView)
            {
                calendarMonthView.SetParent(oldValue as Element, newValue as Element);
            }
        }

        /// <summary>
        /// Invokes on month cell disabled dates text style changed.
        /// </summary>
        /// <param name="bindable">The month view settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnDisabledDatesTextStyleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as CalendarMonthView)?.RaisePropertyChanged(nameof(DisabledDatesTextStyle), oldValue);
            if (bindable is CalendarMonthView calendarMonthView)
            {
                calendarMonthView.SetParent(oldValue as Element, newValue as Element);
            }
        }

        /// <summary>
        /// Invokes on month cell weekend dates text style changed.
        /// </summary>
        /// <param name="bindable">The month view settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnWeekendDatesTextStyleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as CalendarMonthView)?.RaisePropertyChanged(nameof(WeekendDatesTextStyle), oldValue);
            if (bindable is CalendarMonthView calendarMonthView)
            {
                calendarMonthView.SetParent(oldValue as Element, newValue as Element);
            }
        }

        /// <summary>
        /// Invokes on month cell special dates text style changed.
        /// </summary>
        /// <param name="bindable">The month view settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnSpecialDatesTextStyleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as CalendarMonthView)?.RaisePropertyChanged(nameof(SpecialDatesTextStyle), oldValue);
            if (bindable is CalendarMonthView calendarMonthView)
            {
                calendarMonthView.SetParent(oldValue as Element, newValue as Element);
            }
        }

        /// <summary>
        /// Invokes on month cell range text style changed.
        /// </summary>
        /// <param name="bindable">The month view settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnRangeTextStyleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as CalendarMonthView)?.RaisePropertyChanged(nameof(RangeTextStyle), oldValue);
            if (bindable is CalendarMonthView calendarMonthView)
            {
                calendarMonthView.SetParent(oldValue as Element, newValue as Element);
            }
        }

        /// <summary>
        /// Invokes on month cell selection text style changed.
        /// </summary>
        /// <param name="bindable">The month view settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnSelectionTextStyleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as CalendarMonthView)?.RaisePropertyChanged(nameof(SelectionTextStyle), oldValue);
            if (bindable is CalendarMonthView calendarMonthView)
            {
                calendarMonthView.SetParent(oldValue as Element, newValue as Element);
            }
        }

        /// <summary>
        /// Invokes on month cell template property changed.
        /// </summary>
        /// <param name="bindable">The month view settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnCellTemplateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as CalendarMonthView)?.RaisePropertyChanged(nameof(CellTemplate), oldValue);
        }

        /// <summary>
        /// Invokes on special day predicate property changed.
        /// </summary>
        /// <param name="bindable">The month view settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnSpecialDayPredicateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as CalendarMonthView)?.RaisePropertyChanged(nameof(SpecialDayPredicate), oldValue);
        }

        /// <summary>
        /// Method to invoke calendar property changed event on month view settings properties changed.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        /// <param name="oldValue">Property old value.</param>
        void RaisePropertyChanged(string propertyName, object? oldValue = null)
        {
            CalendarPropertyChanged?.Invoke(this, new CalendarPropertyChangedEventArgs(propertyName) { OldValue = oldValue });
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

        #region Interface Implementation

        /// <summary>
        /// This method will be called when users merge a theme dictionary
        /// that contains value for “SyncfusionTheme” dynamic resource key.
        /// </summary>
        /// <param name="oldTheme">Old theme.</param>
        /// <param name="newTheme">New theme.</param>
        void IThemeElement.OnControlThemeChanged(string oldTheme, string newTheme)
        {
        }

        /// <summary>
        /// This method will be called when a theme dictionary
        /// that contains the value for your control key is merged in application.
        /// </summary>
        /// <param name="oldTheme">The old value.</param>
        /// <param name="newTheme">The new value.</param>
        void IThemeElement.OnCommonThemeChanged(string oldTheme, string newTheme)
        {
        }

        #endregion

        #region Event

        /// <summary>
        /// Invokes on month view settings property changed and this includes old value of the changed property which is used to wire and unwire events for nested classes.
        /// </summary>
        internal event EventHandler<CalendarPropertyChangedEventArgs>? CalendarPropertyChanged;

        #endregion
    }
}