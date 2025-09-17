using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Input;
using Syncfusion.Maui.Toolkit.Themes;

namespace Syncfusion.Maui.Toolkit.Picker
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SfDateTimePicker"/> class that represents a control, used to select the date with in specified date range.
    /// </summary>
    public class SfDateTimePicker : PickerBase, IParentThemeElement, IThemeElement
    {
        #region Fields

        /// <summary>
        /// Holds the selected date time on dialog mode.
        /// </summary>
        internal DateTime? _internalSelectedDateTime;

        /// <summary>
        /// Holds the day column information.
        /// </summary>
        PickerColumn _dayColumn;

        /// <summary>
        /// Holds the month column information.
        /// </summary>
        PickerColumn _monthColumn;

        /// <summary>
        /// Holds the year column information.
        /// </summary>
        PickerColumn _yearColumn;

        /// <summary>
        /// Holds the hour column information.
        /// </summary>
        PickerColumn _hourColumn;

        /// <summary>
        /// Holds the minute column information.
        /// </summary>
        PickerColumn _minuteColumn;

        /// <summary>
        /// Holds the second column information.
        /// </summary>
        PickerColumn _secondColumn;

        /// <summary>
        /// Holds the meridiem column information.
        /// </summary>
        PickerColumn _meridiemColumn;

        /// <summary>
        /// Holds the picker column collection.
        /// </summary>
        ObservableCollection<PickerColumn> _columns;

        /// <summary>
        /// Holds the value to identify the header selection(date or time).
        /// </summary>
        int _selectedIndex;

        /// <summary>
        /// Holds the header changed or not
        /// </summary>
        bool _isCurrentPickerViewChanged = false;

        #endregion

        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="HeaderView"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="HeaderView"/> dependency property.
        /// </value>
        public static readonly BindableProperty HeaderViewProperty =
            BindableProperty.Create(
                nameof(HeaderView),
                typeof(DateTimePickerHeaderView),
                typeof(SfDateTimePicker),
                defaultValueCreator: bindable => new DateTimePickerHeaderView(),
                propertyChanged: OnHeaderViewChanged);

        /// <summary>
        /// Identifies the <see cref="ColumnHeaderView"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="ColumnHeaderView"/> dependency property.
        /// </value>
        public static readonly BindableProperty ColumnHeaderViewProperty =
           BindableProperty.Create(
               nameof(ColumnHeaderView),
               typeof(DateTimePickerColumnHeaderView),
               typeof(SfDateTimePicker),
               defaultValueCreator: bindable => new DateTimePickerColumnHeaderView(),
               propertyChanged: OnColumnHeaderViewChanged);

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
                typeof(SfDateTimePicker),
                defaultValueCreator: bindable => DateTime.Now,
                propertyChanged: OnSelectedDatePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="DayInterval"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="DayInterval"/> dependency property.
        /// </value>
        public static readonly BindableProperty DayIntervalProperty =
            BindableProperty.Create(
                nameof(DayInterval),
                typeof(int),
                typeof(SfDateTimePicker),
                1,
                propertyChanged: OnDayIntervalPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="MonthInterval"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="MonthInterval"/> dependency property.
        /// </value>
        public static readonly BindableProperty MonthIntervalProperty =
            BindableProperty.Create(
                nameof(MonthInterval),
                typeof(int),
                typeof(SfDateTimePicker),
                1,
                propertyChanged: OnMonthIntervalPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="YearInterval"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="YearInterval"/> dependency property.
        /// </value>
        public static readonly BindableProperty YearIntervalProperty =
            BindableProperty.Create(
                nameof(YearInterval),
                typeof(int),
                typeof(SfDateTimePicker),
                1,
                propertyChanged: OnYearIntervalPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="HourInterval"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="HourInterval"/> dependency property.
        /// </value>
        public static readonly BindableProperty HourIntervalProperty =
            BindableProperty.Create(
                nameof(HourInterval),
                typeof(int),
                typeof(SfDateTimePicker),
                1,
                propertyChanged: OnHourIntervalPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="MinuteInterval"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="MinuteInterval"/> dependency property.
        /// </value>
        public static readonly BindableProperty MinuteIntervalProperty =
            BindableProperty.Create(
                nameof(MinuteInterval),
                typeof(int),
                typeof(SfDateTimePicker),
                1,
                propertyChanged: OnMinuteIntervalPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="SecondInterval"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="SecondInterval"/> dependency property.
        /// </value>
        public static readonly BindableProperty SecondIntervalProperty =
            BindableProperty.Create(
                nameof(SecondInterval),
                typeof(int),
                typeof(SfDateTimePicker),
                1,
                propertyChanged: OnSecondIntervalPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="DateFormat"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="DateFormat"/> dependency property.
        /// </value>
        public static readonly BindableProperty DateFormatProperty =
            BindableProperty.Create(
                nameof(DateFormat),
                typeof(PickerDateFormat),
                typeof(SfDateTimePicker),
                PickerDateFormat.yyyy_MM_dd,
                propertyChanged: OnDateFormatPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="TimeFormat"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="TimeFormat"/> dependency property.
        /// </value>
        public static readonly BindableProperty TimeFormatProperty =
            BindableProperty.Create(
                nameof(TimeFormat),
                typeof(PickerTimeFormat),
                typeof(SfDateTimePicker),
                PickerTimeFormat.HH_mm_ss,
                propertyChanged: OnTimeFormatPropertyChanged);

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
                typeof(SfDateTimePicker),
                defaultValueCreator: bindable => new DateTime(1900, 01, 01),
                propertyChanged: OnMinimumDatePropertyChanged);

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
                typeof(SfDateTimePicker),
                defaultValueCreator: bindable => new DateTime(2100, 12, 31, 23, 59, 59),
                propertyChanged: OnMaximumDatePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="SelectionChangedCommand"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="SelectionChangedCommand"/> dependency property.
        /// </value>
        public static readonly BindableProperty SelectionChangedCommandProperty =
            BindableProperty.Create(
                nameof(SelectionChangedCommand),
                typeof(ICommand),
                typeof(SfDateTimePicker),
                null);

        /// <summary>
        /// Identifies the <see cref="BlackoutDateTimes"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="BlackoutDateTimes"/> dependency property.
        /// </value>
        public static readonly BindableProperty BlackoutDateTimesProperty =
            BindableProperty.Create(
                nameof(BlackoutDateTimes),
                typeof(ObservableCollection<DateTime>),
                typeof(SfDateTimePicker),
                defaultValueCreator: bindable => new ObservableCollection<DateTime>(),
                propertyChanged: OnBlackOutDateTimesPropertyChanged);

        #endregion

        #region Internal Bindable Properties

        /// <summary>
        /// Identifies the <see cref="DateTimePickerBackground"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="DateTimePickerBackground"/> bindable property.
        /// </value>
        internal static readonly BindableProperty DateTimePickerBackgroundProperty =
            BindableProperty.Create(
                nameof(DateTimePickerBackground),
                typeof(Color),
                typeof(SfDateTimePicker),
                defaultValueCreator: bindable => Color.FromArgb("#EEE8F4"),
                propertyChanged: OnDateTimePickerBackgroundChanged);

        /// <summary>
        /// Identifies the <see cref="FooterTextColor"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="FooterTextColor"/> dependency property.
        /// </value>
        internal static readonly BindableProperty FooterTextColorProperty =
            BindableProperty.Create(
                nameof(FooterTextColor),
                typeof(Color),
                typeof(SfDateTimePicker),
                defaultValueCreator: bindable => Color.FromArgb("#6750A4"),
                propertyChanged: OnFooterTextColorChanged);

        /// <summary>
        /// Identifies the <see cref="FooterFontSize"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="FooterFontSize"/> dependency property.
        /// </value>
        internal static readonly BindableProperty FooterFontSizeProperty =
            BindableProperty.Create(
                nameof(FooterFontSize),
                typeof(double),
                typeof(SfDateTimePicker),
                defaultValueCreator: bindable => 14d,
                propertyChanged: OnFooterFontSizeChanged);

        /// <summary>
        /// Identifies the <see cref="SelectedTextColor"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="SelectedTextColor"/> dependency property.
        /// </value>
        internal static readonly BindableProperty SelectedTextColorProperty =
            BindableProperty.Create(
                nameof(SelectedTextColor),
                typeof(Color),
                typeof(SfDateTimePicker),
                defaultValueCreator: bindable => Colors.White,
                propertyChanged: OnSelectedTextColorChanged);

        /// <summary>
        /// Identifies the <see cref="SelectionTextColor"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="SelectionTextColor"/> dependency property.
        /// </value>
        internal static readonly BindableProperty SelectionTextColorProperty =
            BindableProperty.Create(
                nameof(SelectionTextColor),
                typeof(Color),
                typeof(SfPicker),
                defaultValueCreator: bindable => Color.FromArgb("#6750A4"),
                propertyChanged: OnSelectedTextColorChanged);

        /// <summary>
        /// Identifies the <see cref="SelectedFontSize"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="SelectedFontSize"/> dependency property.
        /// </value>
        internal static readonly BindableProperty SelectedFontSizeProperty =
            BindableProperty.Create(
                nameof(SelectedFontSize),
                typeof(double),
                typeof(SfDateTimePicker),
                defaultValueCreator: bindable => 16d,
                propertyChanged: OnSelectedFontSizeChanged);

        /// <summary>
        /// Identifies the <see cref="NormalTextColor"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="NormalTextColor"/> dependency property.
        /// </value>
        internal static readonly BindableProperty NormalTextColorProperty =
            BindableProperty.Create(
                nameof(NormalTextColor),
                typeof(Color),
                typeof(SfDateTimePicker),
                defaultValueCreator: bindable => Color.FromArgb("#1C1B1F"),
                propertyChanged: OnNormalTextColorChanged);

        /// <summary>
        /// Identifies the <see cref="NormalFontSize"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="NormalFontSize"/> dependency property.
        /// </value>
        internal static readonly BindableProperty NormalFontSizeProperty =
            BindableProperty.Create(
                nameof(NormalFontSize),
                typeof(double),
                typeof(SfDateTimePicker),
                defaultValueCreator: bindable => 16d,
                propertyChanged: OnNormalFontSizeChanged);

        /// <summary>
        /// Identifies the <see cref="DisabledTextColor"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="DisabledTextColor"/> dependency property.
        /// </value>
        internal static readonly BindableProperty DisabledTextColorProperty =
            BindableProperty.Create(
                nameof(DisabledTextColor),
                typeof(Color),
                typeof(SfDateTimePicker),
                defaultValueCreator: bindable => Color.FromArgb("#611C1B1F"),
                propertyChanged: OnDisabledTextColorChanged);

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SfDateTimePicker"/> class.
        /// </summary>
        public SfDateTimePicker()
        {
            _dayColumn = new PickerColumn();
            _monthColumn = new PickerColumn();
            _yearColumn = new PickerColumn();
            _hourColumn = new PickerColumn();
            _minuteColumn = new PickerColumn();
            _secondColumn = new PickerColumn();
            _meridiemColumn = new PickerColumn();
            _columns = new ObservableCollection<PickerColumn>();
            _selectedIndex = 0;
            Initialize();
            GeneratePickerColumns();
            BaseColumns = _columns;
            SelectionIndexChanged += OnPickerSelectionIndexChanged;
            BlackoutDateTimes.CollectionChanged += OnBlackoutDateTimes_CollectionChanged;
            BackgroundColor = DateTimePickerBackground;
            IntializePickerStyle();
            Dispatcher.Dispatch(() =>
            {
                InitializeTheme();
            });
            HeaderView.Parent = this;
            ColumnHeaderView.Parent = this;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the value of header view. This property can be used to customize the header in SfDateTimePicker.
        /// </summary>
        /// <example>
        /// The following example demonstrates how to customize the header view of SfDateTimePicker.
        /// <code>
        /// <![CDATA[
        /// SfDateTimePicker dateTimePicker = new SfDateTimePicker();
        /// dateTimePicker.HeaderView = new DateTimePickerHeaderView
        /// {
        ///     TextStyle = new PickerTextStyle
        ///     {
        ///         TextColor = Colors.Blue,
        ///         FontSize = 18,
        ///         FontAttributes = FontAttributes.Bold
        ///     },
        ///     Background = new SolidColorBrush(Colors.LightGray)
        /// };
        /// ]]>
        /// </code>
        /// </example>
        public DateTimePickerHeaderView HeaderView
        {
            get { return (DateTimePickerHeaderView)GetValue(HeaderViewProperty); }
            set { SetValue(HeaderViewProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value of column header view. This property can be used to customize the header column in SfDateTimePicker.
        /// </summary>
        /// <example>
        /// The following example demonstrates how to customize the column header view of SfDateTimePicker.
        /// <code>
        /// <![CDATA[
        /// SfDateTimePicker dateTimePicker = new SfDateTimePicker();
        /// dateTimePicker.ColumnHeaderView = new DateTimePickerColumnHeaderView
        /// {
        ///     Background = new SolidColorBrush(Colors.LightBlue),
        ///     Height = 40,
        ///     DividerColor = Colors.Gray,
        ///     TextStyle = new PickerTextStyle
        ///     {
        ///         TextColor = Colors.DarkBlue,
        ///         FontSize = 16
        ///     },
        /// };
        /// ]]>
        /// </code>
        /// </example>
        public DateTimePickerColumnHeaderView ColumnHeaderView
        {
            get { return (DateTimePickerColumnHeaderView)GetValue(ColumnHeaderViewProperty); }
            set { SetValue(ColumnHeaderViewProperty, value); }
        }

        /// <summary>
        /// Gets or sets the date picker selected date in SfDateTimePicker.
        /// </summary>
        /// <value>The default value of <see cref="SfDateTimePicker.SelectedDate"/> is <see cref="DateTime.Now"/>.</value>
        /// <example>
        /// The following examples demonstrate how to set the selected date in SfDateTimePicker.
        /// # [XAML](#tab/tabid-1)
        /// <code language="xaml">
        /// <![CDATA[
        /// <Picker:SfDateTimePicker x:Name="DateTimePicker"
        ///                          SelectedDate="2023-06-15 14:30:00" />
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-2)
        /// <code language="C#">
        /// SfDateTimePicker dateTimePicker = new SfDateTimePicker();
        /// dateTimePicker.SelectedDate = new DateTime(2023, 6, 15, 14, 30, 0);
        /// </code>
        /// </example>
        public DateTime? SelectedDate
        {
            get { return (DateTime?)GetValue(SelectedDateProperty); }
            set { SetValue(SelectedDateProperty, value); }
        }

        /// <summary>
        /// Gets or sets the day interval in SfDateTimePicker.
        /// </summary>
        /// <value>The default value of <see cref="SfDateTimePicker.DayInterval"/> is 1.</value>
        /// <example>
        /// The following examples demonstrate how to set the day interval in SfDateTimePicker.
        /// # [XAML](#tab/tabid-3)
        /// <code language="xaml">
        /// <![CDATA[
        /// <Picker:SfDateTimePicker x:Name="DateTimePicker"
        ///                      DayInterval="2" />
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-4)
        /// <code language="C#">
        /// SfDateTimePicker dateTimePicker = new SfDateTimePicker();
        /// dateTimePicker.DayInterval = 2;
        /// </code>
        /// </example>
        public int DayInterval
        {
            get { return (int)GetValue(DayIntervalProperty); }
            set { SetValue(DayIntervalProperty, value); }
        }

        /// <summary>
        /// Gets or sets the month interval in SfDateTimePicker.
        /// </summary>
        /// <value>The default value of <see cref="SfDateTimePicker.MonthInterval"/> is 1.</value>
        /// <example>
        /// The following examples demonstrate how to set the month interval in SfDateTimePicker.
        /// # [XAML](#tab/tabid-5)
        /// <code language="xaml">
        /// <![CDATA[
        /// <Picker:SfDateTimePicker x:Name="DateTimePicker"
        ///                      MonthInterval="2" />
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-6)
        /// <code language="C#">
        /// SfDateTimePicker dateTimePicker = new SfDateTimePicker();
        /// dateTimePicker.MonthInterval = 2;
        /// </code>
        /// </example>
        public int MonthInterval
        {
            get { return (int)GetValue(MonthIntervalProperty); }
            set { SetValue(MonthIntervalProperty, value); }
        }

        /// <summary>
        /// Gets or sets the year interval in SfDateTimePicker.
        /// </summary>
        /// <value>The default value of <see cref="SfDateTimePicker.YearInterval"/> is 1.</value>
        /// <example>
        /// The following examples demonstrate how to set the year interval in SfDateTimePicker.
        /// # [XAML](#tab/tabid-7)
        /// <code language="xaml">
        /// <![CDATA[
        /// <Picker:SfDateTimePicker x:Name="DateTimePicker"
        ///                      YearInterval="2" />
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-8)
        /// <code language="C#">
        /// SfDateTimePicker dateTimePicker = new SfDateTimePicker();
        /// dateTimePicker.YearInterval = 2;
        /// </code>
        /// </example>
        public int YearInterval
        {
            get { return (int)GetValue(YearIntervalProperty); }
            set { SetValue(YearIntervalProperty, value); }
        }

        /// <summary>
        /// Gets or sets the hour interval in SfDateTimePicker.
        /// </summary>
        /// <value>The default value of <see cref="SfDateTimePicker.HourInterval"/> is 1.</value>
        /// <example>
        /// The following examples demonstrate how to set the hour interval in SfDateTimePicker.
        /// # [XAML](#tab/tabid-9)
        /// <code language="xaml">
        /// <![CDATA[
        /// <Picker:SfDateTimePicker x:Name="DateTimePicker"
        ///                      HourInterval="2" />
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-10)
        /// <code language="C#">
        /// SfDateTimePicker dateTimePicker = new SfDateTimePicker();
        /// dateTimePicker.HourInterval = 2;
        /// </code>
        /// </example>
        public int HourInterval
        {
            get { return (int)GetValue(HourIntervalProperty); }
            set { SetValue(HourIntervalProperty, value); }
        }

        /// <summary>
        /// Gets or sets the minute interval in SfDateTimePicker.
        /// </summary>
        /// <value>The default value of <see cref="SfDateTimePicker.MinuteInterval"/> is 1.</value>
        /// <example>
        /// The following examples demonstrate how to set the minute interval in SfDateTimePicker.
        /// # [XAML](#tab/tabid-11)
        /// <code language="xaml">
        /// <![CDATA[
        /// <Picker:SfDateTimePicker x:Name="DateTimePicker"
        ///                      MinuteInterval="2" />
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-12)
        /// <code language="C#">
        /// SfDateTimePicker dateTimePicker = new SfDateTimePicker();
        /// dateTimePicker.MinuteInterval = 2;
        /// </code>
        /// </example>
        public int MinuteInterval
        {
            get { return (int)GetValue(MinuteIntervalProperty); }
            set { SetValue(MinuteIntervalProperty, value); }
        }

        /// <summary>
        /// Gets or sets the second interval in SfDateTimePicker.
        /// </summary>
        /// <value>The default value of <see cref="SfDateTimePicker.SecondInterval"/> is 1.</value>
        /// /// <example>
        /// The following examples demonstrate how to set the second interval in SfDateTimePicker.
        /// # [XAML](#tab/tabid-13)
        /// <code language="xaml">
        /// <![CDATA[
        /// <Picker:SfDateTimePicker x:Name="DateTimePicker"
        ///                      SecondInterval="2" />
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-14)
        /// <code language="C#">
        /// SfDateTimePicker dateTimePicker = new SfDateTimePicker();
        /// dateTimePicker.SecondInterval = 2;
        /// </code>
        /// </example>
        public int SecondInterval
        {
            get { return (int)GetValue(SecondIntervalProperty); }
            set { SetValue(SecondIntervalProperty, value); }
        }

        /// <summary>
        /// Gets or sets the picker date format in SfDateTimePicker.
        /// </summary>
        /// <value>The default value of <see cref="SfDateTimePicker.DateFormat"/> is <see cref="PickerDateFormat.yyyy_MM_dd"/>.</value>
        /// <example>
        /// The following examples demonstrate how to set the date format in SfDateTimePicker.
        /// # [XAML](#tab/tabid-15)
        /// <code language="xaml">
        /// <![CDATA[
        /// <Picker:SfDateTimePicker x:Name="DateTimePicker"
        ///                          DateFormat="dd_MM_yyyy" />
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-16)
        /// <code language="C#">
        /// SfDateTimePicker dateTimePicker = new SfDateTimePicker();
        /// dateTimePicker.DateFormat = PickerDateFormat.dd_MM_yyyy;
        /// </code>
        /// </example>
        public PickerDateFormat DateFormat
        {
            get { return (PickerDateFormat)GetValue(DateFormatProperty); }
            set { SetValue(DateFormatProperty, value); }
        }

        /// <summary>
        /// Gets or sets the picker time format in SfDateTimePicker.
        /// </summary>
        /// <value>The default value of <see cref="SfDateTimePicker.TimeFormat"/> is <see cref="PickerTimeFormat.HH_mm_ss"/>.</value>
        /// <example>
        /// The following examples demonstrate how to set the time format in SfDateTimePicker.
        /// # [XAML](#tab/tabid-17)
        /// <code language="xaml">
        /// <![CDATA[
        /// <Picker:SfDateTimePicker x:Name="DateTimePicker"
        ///                          TimeFormat="hh_mm_tt" />
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-18)
        /// <code language="C#">
        /// SfDateTimePicker dateTimePicker = new SfDateTimePicker();
        /// dateTimePicker.TimeFormat = PickerTimeFormat.hh_mm_tt;
        /// </code>
        /// </example>
        public PickerTimeFormat TimeFormat
        {
            get { return (PickerTimeFormat)GetValue(TimeFormatProperty); }
            set { SetValue(TimeFormatProperty, value); }
        }

        /// <summary>
        /// Gets or sets the minimum date in SfDateTimePicker.
        /// </summary>
        /// <value>The default value of <see cref="SfDateTimePicker.MinimumDate"/> is "DateTime(1900, 01, 01)".</value>
        /// <example>
        /// The following examples demonstrate how to set the minimum date in SfDateTimePicker.
        /// # [XAML](#tab/tabid-19)
        /// <code language="xaml">
        /// <![CDATA[
        /// <Picker:SfDateTimePicker x:Name="DateTimePicker"
        ///                          MinimumDate="2023-01-01" />
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-20)
        /// <code language="C#">
        /// SfDateTimePicker dateTimePicker = new SfDateTimePicker();
        /// dateTimePicker.MinimumDate = new DateTime(2023, 1, 1);
        /// </code>
        /// </example>
        public DateTime MinimumDate
        {
            get { return (DateTime)GetValue(MinimumDateProperty); }
            set { SetValue(MinimumDateProperty, value); }
        }

        /// <summary>
        /// Gets or sets the maximum date in SfDateTimePicker.
        /// </summary>
        /// <value>The default value of <see cref="SfDateTimePicker.MaximumDate"/> is "DateTime(2100, 12, 31, 23, 59, 59)".</value>
        /// <example>
        /// The following examples demonstrate how to set the maximum date in SfDateTimePicker.
        /// # [XAML](#tab/tabid-21)
        /// <code language="xaml">
        /// <![CDATA[
        /// <Picker:SfDateTimePicker x:Name="DateTimePicker"
        ///                          MaximumDate="2023-12-31 23:59:59" />
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-22)
        /// <code language="C#">
        /// SfDateTimePicker dateTimePicker = new SfDateTimePicker();
        /// dateTimePicker.MaximumDate = new DateTime(2023, 12, 31, 23, 59, 59);
        /// </code>
        /// </example>
        public DateTime MaximumDate
        {
            get { return (DateTime)GetValue(MaximumDateProperty); }
            set { SetValue(MaximumDateProperty, value); }
        }

        /// <summary>
        /// Gets or sets the selection changed command in SfDateTimePicker.
        /// </summary>
        /// <value>The default value of <see cref="SfDateTimePicker.SelectionChangedCommand"/> is null.</value>
        /// <example>
        /// The following example demonstrates how to set the selection changed command in SfDateTimePicker.
        /// # [XAML](#tab/tabid-23)
        /// <code Lang="XAML"><![CDATA[
        /// <ContentPage.BindingContext>
        ///    <local:ViewModel/>
        /// </ContentPage.BindingContext>
        /// <Picker:SfDateTimePicker x:Name="DateTimePicker"
        ///                      SelectionChangedCommand="{Binding SelectionCommand}">
        /// </Picker:SfDateTimePicker>
        /// ]]></code>
        /// # [C#](#tab/tabid-24)
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
        ///    public ViewModel()
        ///    {
        ///      SelectionCommand = new Command(SelectionChanged);
        ///    }
        ///    private void SelectionChanged()
        ///    {
        ///    }
        ///  }
        /// ]]></code>
        /// </example>
        public ICommand SelectionChangedCommand
        {
            get { return (ICommand)GetValue(SelectionChangedCommandProperty); }
            set { SetValue(SelectionChangedCommandProperty, value); }
        }

        /// <summary>
        /// Gets or sets the BlackoutDateTimes in SfDateTimePicker.
        /// </summary>
        /// <remarks>The selection view will not be applicable when setting blackout datetimes.</remarks>
        /// <example>
        /// The following examples demonstrate how to set the blackout date times in SfDateTimePicker.
        /// # [XAML](#tab/tabid-25)
        /// <code language="xaml">
        /// <![CDATA[
        /// <picker:SfDateTimePicker x:Name="picker">
        ///    <picker:SfDateTimePicker.BlackoutDateTimes>
        ///       <date:DateTime>2001-08-10</date:DateTime>
        ///       <date:DateTime>2001-08-12</date:DateTime>
        ///       <date:DateTime>2001-08-14</date:DateTime>
        ///       <date:DateTime>2001-08-15 12:11:00</date:DateTime>
        ///       <date:DateTime>2001-08-15 12:12:00</date:DateTime>
        ///       <date:DateTime>2001-08-15 12:08:00</date:DateTime>
        ///       <date:DateTime>2001-08-15 12:06:00</date:DateTime>
        ///    </picker:SfDateTimePicker.BlackoutDateTimes>
        /// </picker:SfDateTimePicker>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-26)
        /// <code language="C#">
        /// SfDatePicker picker = new SfDatePicker();
        /// picker.BlackoutDateTimes.Add(new DateTime(2001, 8, 10));
        /// picker.BlackoutDateTimes.Add(new DateTime(2001, 8, 12));
        /// picker.BlackoutDateTimes.Add(new DateTime(2001, 8, 14));
        /// picker.BlackoutDateTimes.Add(new DateTime(2001, 8, 17));
        /// picker.BlackoutDateTimes.Add(new DateTime(2001, 8, 15, 12, 11, 0));
        /// picker.BlackoutDateTimes.Add(new DateTime(2001, 8, 15, 12, 12, 0));
        /// picker.BlackoutDateTimes.Add(new DateTime(2001, 8, 15, 12, 8, 0));
        /// </code>
        /// </example>
        public ObservableCollection<DateTime> BlackoutDateTimes
        {
            get { return (ObservableCollection<DateTime>)GetValue(BlackoutDateTimesProperty); }
            set { SetValue(BlackoutDateTimesProperty, value); }
        }

        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets or sets the background color of the picker.
        /// </summary>
        internal Color DateTimePickerBackground
        {
            get { return (Color)GetValue(DateTimePickerBackgroundProperty); }
            set { SetValue(DateTimePickerBackgroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets the footer text color of the text style.
        /// </summary>
        internal Color FooterTextColor
        {
            get { return (Color)GetValue(FooterTextColorProperty); }
            set { SetValue(FooterTextColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the footer font size of the text style.
        /// </summary>
        internal double FooterFontSize
        {
            get { return (double)GetValue(FooterFontSizeProperty); }
            set { SetValue(FooterFontSizeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the selection text color of the text style.
        /// </summary>
        /// <remarks>
        /// This color applicable for default text display mode.
        /// </remarks>
        internal Color SelectedTextColor
        {
            get { return (Color)GetValue(SelectedTextColorProperty); }
            set { SetValue(SelectedTextColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the selection text color of the text style.
        /// </summary>
        /// <remarks>
        /// This color is used for Fade, Shrink and FadeAndShrink mode.
        /// </remarks>
        internal Color SelectionTextColor
        {
            get { return (Color)GetValue(SelectionTextColorProperty); }
            set { SetValue(SelectionTextColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the selection font size of the text style.
        /// </summary>
        internal double SelectedFontSize
        {
            get { return (double)GetValue(SelectedFontSizeProperty); }
            set { SetValue(SelectedFontSizeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the normal text color of the text style.
        /// </summary>
        internal Color NormalTextColor
        {
            get { return (Color)GetValue(NormalTextColorProperty); }
            set { SetValue(NormalTextColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the normal font size of the text style.
        /// </summary>
        internal double NormalFontSize
        {
            get { return (double)GetValue(NormalFontSizeProperty); }
            set { SetValue(NormalFontSizeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the disabled text color of the text style.
        /// </summary>
        internal Color DisabledTextColor
        {
            get { return (Color)GetValue(DisabledTextColorProperty); }
            set { SetValue(DisabledTextColorProperty, value); }
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Method to reset the picker date columns.
        /// </summary>
        internal void ResetDateColumns()
        {
            _dayColumn = new PickerColumn();
            _monthColumn = new PickerColumn();
            _yearColumn = new PickerColumn();
            GeneratePickerColumns();
            BaseColumns.Clear();
            BaseColumns = _columns;
        }

        /// <summary>
        /// Method to get the date header text based on format.
        /// </summary>
        /// <returns>Returns the date header text.</returns>
        internal string GetDateHeaderText()
        {
            DateTime maxDate = DatePickerHelper.GetValidMaxDate(MinimumDate, MaximumDate);
            DateTime? selectedDateNullable = DateTime.Now;
            if (IsScrollSelectionAllowed())
            {
                selectedDateNullable = _internalSelectedDateTime.HasValue ? _internalSelectedDateTime : SelectedDate;
            }
            else
            {
                selectedDateNullable = SelectedDate;
            }

            // Check if selectedDateNullable is null, and return an empty string if it is
            if (!selectedDateNullable.HasValue)
            {
                return SfPickerResources.GetLocalizedString("Date");
            }

            DateTime selectedDate = DatePickerHelper.GetValidDateTime(selectedDateNullable.Value, MinimumDate, maxDate);
            string value = selectedDate.ToString(HeaderView.DateFormat, CultureInfo.CurrentUICulture);
            value = DatePickerHelper.ReplaceCultureMonthString(value, HeaderView.DateFormat, selectedDate);
            value = DatePickerHelper.ReplaceCultureMeridiemString(value, HeaderView.DateFormat);
            return value;
        }

        /// <summary>
        /// Method to get the time header text based on format.
        /// </summary>
        /// <returns>Returns the time header text.</returns>
        internal string GetTimeHeaderText()
        {
            DateTime maxDate = DatePickerHelper.GetValidMaxDate(MinimumDate, MaximumDate);
            DateTime? selectedTimeNullable = DateTime.Now;
            if (IsScrollSelectionAllowed())
            {
                selectedTimeNullable = _internalSelectedDateTime.HasValue ? _internalSelectedDateTime : SelectedDate;
            }
            else
            {
                selectedTimeNullable = this.SelectedDate;
            }

            if (!selectedTimeNullable.HasValue)
            {
                return SfPickerResources.GetLocalizedString("Time");
            }

            DateTime selectedDate = DatePickerHelper.GetValidDateTime(selectedTimeNullable, MinimumDate, maxDate);
            string value = selectedDate.ToString(HeaderView.TimeFormat, CultureInfo.InvariantCulture);
            value = DatePickerHelper.ReplaceCultureMonthString(value, HeaderView.TimeFormat, selectedDate);
            value = DatePickerHelper.ReplaceCultureMeridiemString(value, HeaderView.TimeFormat);
            return value;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Method trigged whenever the base panel selection is changed.
        /// </summary>
        /// <param name="sender">Base picker instance value.</param>
        /// <param name="e">Selection changed event arguments.</param>
        void OnPickerSelectionIndexChanged(object? sender, PickerSelectionChangedEventArgs e)
        {
            if (_selectedIndex == 0)
            {
                if (IsScrollSelectionAllowed())
                {
                    ResetDateTimeOnViewChange();
                }

                OnDatePickerSelectionIndexChanged(e);
            }
            else
            {
                if (IsScrollSelectionAllowed())
                {
                    ResetDateTimeOnViewChange();
                }

                OnTimePickerSelectionIndexChanged(e);
            }
        }

        /// <summary>
        /// Method trigged whenever the base panel date selection is changed.
        /// </summary>
        /// <param name="e">Selection changed event arguments.</param>
        void OnDatePickerSelectionIndexChanged(PickerSelectionChangedEventArgs e)
        {
            string dayFormat;
            string monthFormat;
            List<int> formatStringOrder = DatePickerHelper.GetFormatStringOrder(out dayFormat, out monthFormat, DateFormat);
            int changedColumnValue = formatStringOrder[e.ColumnIndex];
            DateTime maxDate = DatePickerHelper.GetValidMaxDate(MinimumDate, MaximumDate);
            DateTime date = IsScrollSelectionAllowed() && _internalSelectedDateTime.HasValue ? _internalSelectedDateTime.Value : SelectedDate ?? _previousSelectedDateTime;
            DateTime previousSelectedDate = DatePickerHelper.GetValidDateTime(date, MinimumDate, maxDate);
            DateTime selectedDate = DateTime.Now;
            switch (changedColumnValue)
            {
                //// Need to handle the day selection changes.
                case 0:
                    {
                        int day = 1;
                        if (_dayColumn.ItemsSource != null && _dayColumn.ItemsSource is ObservableCollection<string> dayCollection && dayCollection.Count > e.NewValue)
                        {
                            //// Get the day value based on the selected index changes value.
                            day = int.Parse(dayCollection[e.NewValue]);
                        }

                        selectedDate = new DateTime(previousSelectedDate.Year, previousSelectedDate.Month, day, previousSelectedDate.Hour, previousSelectedDate.Minute, previousSelectedDate.Second);
                    }

                    break;
                //// Need to handle the month selection changes.
                case 1:
                    {
                        int month = 1;
                        if (_monthColumn.ItemsSource != null && _monthColumn.ItemsSource is ObservableCollection<string> monthCollection && monthCollection.Count > e.NewValue)
                        {
                            if (monthFormat == "M" || monthFormat == "MM")
                            {
                                //// Get the month value based on the selected index changes value.
                                month = int.Parse(monthCollection[e.NewValue]);
                            }
                            else if (monthFormat == "MMM")
                            {
                                List<string> months = DateTimeFormatInfo.CurrentInfo.AbbreviatedMonthNames.ToList();
                                //// Get the month value based on the selected index changes value.
                                month = months.IndexOf(monthCollection[e.NewValue]) + 1;
                            }
                        }

                        ObservableCollection<string> days = DatePickerHelper.GetDays(dayFormat, month, previousSelectedDate.Year, MinimumDate, maxDate, DayInterval);
                        ObservableCollection<string> previousDays = _dayColumn.ItemsSource is ObservableCollection<string> previousDayCollection ? previousDayCollection : new ObservableCollection<string>();
                        //// Check the month selection changes needed to update the days collection.
                        if (!PickerHelper.IsCollectionEquals(days, previousDays))
                        {
                            _dayColumn.ItemsSource = days;
                        }

                        //// Check the new days collection have a selected day value, if not then update the nearby value.
                        int index = DatePickerHelper.GetDayIndex(dayFormat, days, previousSelectedDate.Day);
                        int day = index == -1 ? 1 : int.Parse(days[index]);

                        selectedDate = new DateTime(previousSelectedDate.Year, month, day, previousSelectedDate.Hour, previousSelectedDate.Minute, previousSelectedDate.Second);
                    }

                    break;
                //// Need to handle the year selection changes.
                case 2:
                    {
                        int year = MinimumDate.Year;
                        if (_yearColumn.ItemsSource != null && _yearColumn.ItemsSource is ObservableCollection<string> yearCollection && yearCollection.Count > e.NewValue)
                        {
                            //// Get the year value based on the selected index changes value.
                            year = int.Parse(yearCollection[e.NewValue]);
                        }

                        ObservableCollection<string> months = DatePickerHelper.GetMonths(monthFormat, year, MinimumDate, maxDate, MonthInterval);
                        ObservableCollection<string> previousMonths = _monthColumn.ItemsSource is ObservableCollection<string> previousMonthCollection ? previousMonthCollection : new ObservableCollection<string>();
                        //// Check the year index changes needed to update the month collection.
                        if (!PickerHelper.IsCollectionEquals(months, previousMonths))
                        {
                            _monthColumn.ItemsSource = months;
                        }

                        //// Check the month collection have selected month value, if not then update the nearby value.
                        int monthIndex = DatePickerHelper.GetMonthIndex(monthFormat, months, previousSelectedDate.Month);
                        int month = 1;
                        if (monthFormat == "M" || monthFormat == "MM")
                        {
                            //// Get the month value based on the selected index changes value.
                            month = int.Parse(months[monthIndex]);
                        }
                        else if (monthFormat == "MMM")
                        {
                            List<string> monthStrings = DateTimeFormatInfo.CurrentInfo.AbbreviatedMonthNames.ToList();
                            //// Get the month value based on the selected index changes value.
                            month = monthStrings.IndexOf(months[monthIndex]) + 1;
                        }

                        ObservableCollection<string> days = DatePickerHelper.GetDays(dayFormat, month, year, MinimumDate, maxDate, DayInterval);
                        ObservableCollection<string> previousDays = _dayColumn.ItemsSource is ObservableCollection<string> previousDayCollection ? previousDayCollection : new ObservableCollection<string>();
                        //// Check the year and month(if month items source updated) changes needed to change the day collection.
                        if (!PickerHelper.IsCollectionEquals(days, previousDays))
                        {
                            _dayColumn.ItemsSource = days;
                        }

                        //// Check the day collection have selected day value, if not then update the nearby value.
                        int index = DatePickerHelper.GetDayIndex(dayFormat, days, previousSelectedDate.Day);
                        int day = index == -1 ? 1 : int.Parse(days[index]);

                        selectedDate = new DateTime(year, month, day, previousSelectedDate.Hour, previousSelectedDate.Minute, previousSelectedDate.Second);
                    }

                    break;
            }

            if (IsScrollSelectionAllowed())
            {
                // Check if the selected date-time falls within any blackout date-times
                // If it does, revert the day column selection to the previous value
                if (BlackoutDateTimes.Any(blackOutDateTime => DatePickerHelper.IsBlackoutDateTime(blackOutDateTime, selectedDate, out bool isTimeSpanAtZero)))
                {
                    _dayColumn.SelectedIndex = e.OldValue;
                }
                // Set the internal selected date-time to the newly selected value
                _internalSelectedDateTime = selectedDate;
                // Update the selected items in all relevant columns (e.g., date and time parts)
                UpdateColumnsSelectedItem();
                // Ensure the internal selected date-time is within the allowed minimum and maximum range
                _internalSelectedDateTime = DatePickerHelper.GetValidDateTime(_internalSelectedDateTime, MinimumDate, MaximumDate);
                // Update the selected index in the UI to reflect the validated internal selected date-time
                UpdateSelectedIndex(_internalSelectedDateTime);
                // Update the date header text in the UI based on the selected date
                BaseHeaderView.DateText = GetDateHeaderText();
            }
            else
            {
                if (!DatePickerHelper.IsSameDateTime(selectedDate, SelectedDate))
                {
                    SelectedDate = selectedDate;
                }
            }
        }

        /// <summary>
        /// Method trigged whenever the base panel time picker selection is changed.
        /// </summary>
        /// <param name="e">Selection changed event arguments.</param>
        void OnTimePickerSelectionIndexChanged(PickerSelectionChangedEventArgs e)
        {
            string hourFormat;
            List<int> formatStringOrder = TimePickerHelper.GetFormatStringOrder(out hourFormat, TimeFormat);
            int changedColumnValue = formatStringOrder[e.ColumnIndex];
            DateTime maxDate = DatePickerHelper.GetValidMaxDate(MinimumDate, MaximumDate);
            DateTime date = IsScrollSelectionAllowed() && _internalSelectedDateTime.HasValue ? _internalSelectedDateTime.Value : SelectedDate ?? _previousSelectedDateTime;
            DateTime previousSelectedDate = DatePickerHelper.GetValidDateTime(date, MinimumDate, maxDate);
            bool isMinDate = previousSelectedDate.Date == MinimumDate.Date;
            bool isMaxDate = previousSelectedDate.Date == maxDate.Date;
            DateTime selectedDate = DateTime.Now;
            switch (changedColumnValue)
            {
                case 0:
                    {
                        int hour = 0;
                        if (_hourColumn.ItemsSource != null && _hourColumn.ItemsSource is ObservableCollection<string> hourCollection && hourCollection.Count > e.NewValue)
                        {
                            //// Get the hour value based on the selected index changes value.
                            hour = int.Parse(hourCollection[e.NewValue]);
                        }

                        if (hourFormat == "h" || hourFormat == "hh")
                        {
                            hour = hour == 12 ? 0 : hour;
                            if (previousSelectedDate.Hour >= 12)
                            {
                                hour += 12;
                            }
                        }

                        ObservableCollection<string> minutes = TimePickerHelper.GetMinutes(MinuteInterval, hour, previousSelectedDate, isMinDate ? MinimumDate : null, isMaxDate ? maxDate : null);
                        ObservableCollection<string> previousMinutes = _minuteColumn.ItemsSource is ObservableCollection<string> previousMinuteCollection ? previousMinuteCollection : new ObservableCollection<string>();
                        if (!PickerHelper.IsCollectionEquals(minutes, previousMinutes))
                        {
                            _minuteColumn.ItemsSource = minutes;
                        }

                        int minuteIndex = TimePickerHelper.GetMinuteOrSecondIndex(minutes, previousSelectedDate.Minute);
                        //// Get the minute value based on the selected index changes value.
                        int minute = int.Parse(minutes[minuteIndex]);

                        ObservableCollection<string> seconds = TimePickerHelper.GetSeconds(SecondInterval, hour, minute, previousSelectedDate, isMinDate ? MinimumDate : null, isMaxDate ? maxDate : null);
                        ObservableCollection<string> previousSeconds = _secondColumn.ItemsSource is ObservableCollection<string> previousSecondCollection ? previousSecondCollection : new ObservableCollection<string>();
                        if (!PickerHelper.IsCollectionEquals(seconds, previousSeconds))
                        {
                            _secondColumn.ItemsSource = seconds;
                        }

                        int secondIndex = TimePickerHelper.GetMinuteOrSecondIndex(seconds, previousSelectedDate.Second);
                        //// Get the second value based on the selected index changes value.
                        int second = int.Parse(seconds[secondIndex]);

                        selectedDate = new DateTime(previousSelectedDate.Year, previousSelectedDate.Month, previousSelectedDate.Day, hour, minute, second);
                    }

                    break;
                case 1:
                    {
                        int minutes = 0;
                        if (_minuteColumn.ItemsSource != null && _minuteColumn.ItemsSource is ObservableCollection<string> minuteCollection && minuteCollection.Count > e.NewValue)
                        {
                            //// Get the minute value based on the selected index changes value.
                            minutes = int.Parse(minuteCollection[e.NewValue]);
                        }

                        ObservableCollection<string> seconds = TimePickerHelper.GetSeconds(SecondInterval, previousSelectedDate.Hour, minutes, previousSelectedDate, isMinDate ? MinimumDate : null, isMaxDate ? maxDate : null);
                        ObservableCollection<string> previousSeconds = _secondColumn.ItemsSource is ObservableCollection<string> previousSecondCollection ? previousSecondCollection : new ObservableCollection<string>();
                        if (!PickerHelper.IsCollectionEquals(seconds, previousSeconds))
                        {
                            _secondColumn.ItemsSource = seconds;
                        }

                        int secondIndex = TimePickerHelper.GetMinuteOrSecondIndex(seconds, previousSelectedDate.Second);
                        //// Get the second value based on the selected index changes value.
                        int second = int.Parse(seconds[secondIndex]);

                        selectedDate = new DateTime(previousSelectedDate.Year, previousSelectedDate.Month, previousSelectedDate.Day, previousSelectedDate.Hour, minutes, second);
                    }

                    break;
                case 2:
                    {
                        int seconds = 0;
                        if (_secondColumn.ItemsSource != null && _secondColumn.ItemsSource is ObservableCollection<string> secondCollection && secondCollection.Count > e.NewValue)
                        {
                            //// Get the seconds value based on the selected index changes value.
                            seconds = int.Parse(secondCollection[e.NewValue]);
                        }

                        selectedDate = new DateTime(previousSelectedDate.Year, previousSelectedDate.Month, previousSelectedDate.Day, previousSelectedDate.Hour, previousSelectedDate.Minute, seconds);
                    }

                    break;
                case 3:
                    {
                        ObservableCollection<string> meridiemCollection = new ObservableCollection<string>();
                        if (_meridiemColumn.ItemsSource != null && _meridiemColumn.ItemsSource is ObservableCollection<string> meridiems)
                        {
                            meridiemCollection = meridiems;
                        }

                        if (meridiemCollection.Count <= e.NewValue)
                        {
                            return;
                        }

                        bool isAMSelected = TimePickerHelper.IsAMText(meridiemCollection, e.NewValue);
                        int neededHour = isAMSelected ? 0 : 12;

                        selectedDate = new DateTime(previousSelectedDate.Year, previousSelectedDate.Month, previousSelectedDate.Day, (previousSelectedDate.Hour % 12) + neededHour, previousSelectedDate.Minute, previousSelectedDate.Second);

                        ObservableCollection<string> hours = TimePickerHelper.GetHours(hourFormat, HourInterval, selectedDate, MinimumDate, maxDate);
                        ObservableCollection<string> previousHour = _hourColumn.ItemsSource is ObservableCollection<string> previousHourCollection ? previousHourCollection : new ObservableCollection<string>();
                        if (!PickerHelper.IsCollectionEquals(hours, previousHour))
                        {
                            _hourColumn.ItemsSource = hours;
                        }

                        int? hourIndex = TimePickerHelper.GetHourIndex(hourFormat, hours, previousSelectedDate.Hour);
                        if (hourIndex.HasValue)
                        {
                            int hour = int.Parse(hours[hourIndex.Value]);
                            hour = (hour % 12) + neededHour;

                            ObservableCollection<string> minutes = TimePickerHelper.GetMinutes(MinuteInterval, hour, previousSelectedDate, isMinDate ? MinimumDate : null, isMaxDate ? maxDate : null);
                            ObservableCollection<string> previousMinutes = _minuteColumn.ItemsSource is ObservableCollection<string> previousMinuteCollection ? previousMinuteCollection : new ObservableCollection<string>();
                            if (!PickerHelper.IsCollectionEquals(minutes, previousMinutes))
                            {
                                _minuteColumn.ItemsSource = minutes;
                            }

                            int minuteIndex = TimePickerHelper.GetMinuteOrSecondIndex(minutes, previousSelectedDate.Minute);
                            //// Get the minute value based on the selected index changes value.
                            int minute = int.Parse(minutes[minuteIndex]);

                            ObservableCollection<string> seconds = TimePickerHelper.GetSeconds(SecondInterval, hour, minute, previousSelectedDate, isMinDate ? MinimumDate : null, isMaxDate ? maxDate : null);
                            ObservableCollection<string> previousSeconds = _secondColumn.ItemsSource is ObservableCollection<string> previousSecondCollection ? previousSecondCollection : new ObservableCollection<string>();
                            if (!PickerHelper.IsCollectionEquals(seconds, previousSeconds))
                            {
                                _secondColumn.ItemsSource = seconds;
                            }

                            int secondIndex = TimePickerHelper.GetMinuteOrSecondIndex(seconds, previousSelectedDate.Second);
                            //// Get the second value based on the selected index changes value.
                            int second = int.Parse(seconds[secondIndex]);

                            selectedDate = new DateTime(previousSelectedDate.Year, previousSelectedDate.Month, previousSelectedDate.Day, hour, minute, second);
                        }
                    }

                    break;
            }

            if (IsScrollSelectionAllowed())
            {
                // Check if the selected date and time falls within any blackout date-times
                // If it does, revert the minute column selection to the previous value
                if (BlackoutDateTimes.Any(blackOutDateTime => DatePickerHelper.IsBlackoutDateTime(blackOutDateTime, selectedDate, out bool isTimeSpanAtZero)))
                {
                    _minuteColumn.SelectedIndex = e.OldValue;
                }
                // Set the internal selected date-time to the newly selected value
                _internalSelectedDateTime = selectedDate;
                // Update the selected items in all relevant columns (e.g., date and time parts)
                UpdateColumnsSelectedItem();
                // Ensure the internal selected date-time is within the allowed range
                _internalSelectedDateTime = DatePickerHelper.GetValidDateTime(_internalSelectedDateTime, MinimumDate, MaximumDate);
                // Update the selected index in the UI to reflect the validated internal selected date-time
                UpdateSelectedIndex(_internalSelectedDateTime);
                // Update the time header text in the UI based on the selected time
                BaseHeaderView.TimeText = GetTimeHeaderText();
            }
            else
            {
                if (!DatePickerHelper.IsSameDateTime(selectedDate, SelectedDate))
                {
                    SelectedDate = selectedDate;
                }
            }
        }

        /// <summary>
        /// Method to update the selected index value for all the picker column based on the date value.
        /// </summary>
        /// <param name="date">The selected date value.</param>
        void UpdateSelectedIndex(DateTime? date)
        {
            if (_selectedIndex == 0)
            {
                UpdateSelectedDateIndex(date);
            }
            else
            {
                UpdateSelectedTimeIndex(date);
            }

            BaseHeaderView.DateText = GetDateHeaderText();
            BaseHeaderView.TimeText = GetTimeHeaderText();
        }

        /// <summary>
        /// Method to update the minimum and maximum date value for all the picker column based on the date value.
        /// </summary>
        /// <param name="oldValue">Minimum and Maximum oldvalue.</param>
        /// <param name="newValue">Minimum and Maximum newvalue.</param>
        void UpdateMinimumMaximumDate(object oldValue, object newValue)
        {
            DateTime oldDate = (DateTime)oldValue;
            DateTime newDate = (DateTime)newValue;
            DateTime maxDate = DatePickerHelper.GetValidMaxDate(MinimumDate, MaximumDate);
            DateTime validSelectedDate = DatePickerHelper.GetValidDateTime(SelectedDate, MinimumDate, maxDate);
            if (_selectedIndex == 0)
            {
                UpdateMinimumMaximumDateColumns(oldDate, newDate, validSelectedDate, maxDate);
            }
            else
            {
                UpdateMinimumMaximumTimeColumns(oldDate, newDate, validSelectedDate, maxDate);
            }
        }

        /// <summary>
        /// Method to update the minimum and maximum date column based on the date value.
        /// </summary>
        /// <param name="oldDate">Minimum and Maximum old value.</param>
        /// <param name="newDate">Minimum and Maximum new value.</param>
        /// <param name="validSelectedDate">Valid Selected Date.</param>
        /// <param name="maxDate">Maximum Date.</param>
        void UpdateMinimumMaximumDateColumns(DateTime oldDate, DateTime newDate, DateTime validSelectedDate, DateTime maxDate)
        {
            string dayFormat;
            string monthFormat;
            List<int> formatString = DatePickerHelper.GetFormatStringOrder(out dayFormat, out monthFormat, DateFormat);
            int yearIndex = formatString.IndexOf(2);
            if (yearIndex != -1 && oldDate.Year != newDate.Year)
            {
                _yearColumn = GenerateYearColumn(validSelectedDate);
                _yearColumn.Parent = this;
                _columns[yearIndex] = _yearColumn;
            }

            if (validSelectedDate.Year == MinimumDate.Year || validSelectedDate.Year == maxDate.Year)
            {
                ObservableCollection<string> month = DatePickerHelper.GetMonths(monthFormat, validSelectedDate.Year, MinimumDate, maxDate, MonthInterval);
                ObservableCollection<string> previousMonths = _monthColumn.ItemsSource is ObservableCollection<string> previousMonthCollection ? previousMonthCollection : new ObservableCollection<string>();
                //// Check the year index changes needed to update the month collection.
                if (!PickerHelper.IsCollectionEquals(month, previousMonths))
                {
                    _monthColumn = new PickerColumn()
                    {
                        ItemsSource = month,
                        SelectedIndex = DatePickerHelper.GetMonthIndex(monthFormat, month, validSelectedDate.Month),
                        HeaderText = SfPickerResources.GetLocalizedString(ColumnHeaderView.MonthHeaderText),
                    };
                    int monthIndex = formatString.IndexOf(1);
                    if (monthIndex != -1)
                    {
                        _columns[monthIndex] = _monthColumn;
                    }
                }
            }

            if (!string.IsNullOrEmpty(dayFormat) && ((validSelectedDate.Year == MinimumDate.Year && validSelectedDate.Month == MinimumDate.Month) || (validSelectedDate.Year == maxDate.Year && validSelectedDate.Month == maxDate.Month)))
            {
                ObservableCollection<string> days = DatePickerHelper.GetDays(dayFormat, validSelectedDate.Month, validSelectedDate.Year, MinimumDate, maxDate, DayInterval);
                ObservableCollection<string> previousDays = _dayColumn.ItemsSource is ObservableCollection<string> previousDayCollection ? previousDayCollection : new ObservableCollection<string>();
                //// Check the year and month(if month items source updated) changes needed to change the day collection.
                if (!PickerHelper.IsCollectionEquals(days, previousDays))
                {
                    _dayColumn = new PickerColumn()
                    {
                        ItemsSource = days,
                        SelectedIndex = DatePickerHelper.GetDayIndex(dayFormat, days, validSelectedDate.Day),
                        HeaderText = SfPickerResources.GetLocalizedString(ColumnHeaderView.DayHeaderText),
                    };
                    int dayIndex = formatString.IndexOf(0);
                    if (dayIndex != -1)
                    {
                        _columns[dayIndex] = _dayColumn;
                    }
                }
            }
        }

        /// <summary>
        /// Method to update the minimum and maximum time column based on the date value.
        /// </summary>
        /// <param name="oldDate">Minimum and Maximum old value.</param>
        /// <param name="newDate">Minimum and Maximum new value.</param>
        /// <param name="validSelectedDate">Valid Selected Date.</param>
        /// <param name="maxDate">Maximum Date.</param>
        void UpdateMinimumMaximumTimeColumns(DateTime oldDate, DateTime newDate, DateTime validSelectedDate, DateTime maxDate)
        {
            bool isSelectedDate = validSelectedDate.Date == oldDate.Date || validSelectedDate.Date == newDate.Date;
            if (!isSelectedDate)
            {
                return;
            }

            string hourFormat;
            List<int> formatStringOrder = TimePickerHelper.GetFormatStringOrder(out hourFormat, TimeFormat);
            int index = formatStringOrder.IndexOf(0);
            if (oldDate.Hour != newDate.Hour && index != -1)
            {
                TimeSpan selectedTime = new TimeSpan(validSelectedDate.Hour, validSelectedDate.Minute, validSelectedDate.Second);
                _hourColumn = GenerateHourColumn(hourFormat, selectedTime, validSelectedDate);
                int hourIndex = index;
                _hourColumn.Parent = this;
                _columns[hourIndex] = _hourColumn;
            }

            index = formatStringOrder.IndexOf(1);
            if (index != -1 && (validSelectedDate.Hour == oldDate.Hour || validSelectedDate.Hour == newDate.Hour))
            {
                ObservableCollection<string> minutes = TimePickerHelper.GetMinutes(MinuteInterval, validSelectedDate.Hour, validSelectedDate, MinimumDate, maxDate);
                ObservableCollection<string> previousMinutes = _minuteColumn.ItemsSource is ObservableCollection<string> previousMinuteCollection ? previousMinuteCollection : new ObservableCollection<string>();
                if (!PickerHelper.IsCollectionEquals(minutes, previousMinutes))
                {
                    _minuteColumn = new PickerColumn()
                    {
                        ItemsSource = minutes,
                        SelectedIndex = TimePickerHelper.GetMinuteOrSecondIndex(minutes, validSelectedDate.Minute),
                        HeaderText = SfPickerResources.GetLocalizedString(ColumnHeaderView.MinuteHeaderText),
                    };
                    _columns[index] = _minuteColumn;
                }
            }

            index = formatStringOrder.IndexOf(2);
            if (index != -1 && ((validSelectedDate.Hour == oldDate.Hour && validSelectedDate.Minute == oldDate.Minute) || (validSelectedDate.Hour == newDate.Hour && validSelectedDate.Minute == newDate.Minute)))
            {
                ObservableCollection<string> seconds = TimePickerHelper.GetSeconds(SecondInterval, validSelectedDate.Hour, validSelectedDate.Minute, validSelectedDate, MinimumDate, maxDate);
                ObservableCollection<string> previousSeconds = _secondColumn.ItemsSource is ObservableCollection<string> previousSecondCollection ? previousSecondCollection : new ObservableCollection<string>();
                if (!PickerHelper.IsCollectionEquals(seconds, previousSeconds))
                {
                    _secondColumn = new PickerColumn()
                    {
                        ItemsSource = seconds,
                        SelectedIndex = TimePickerHelper.GetMinuteOrSecondIndex(seconds, validSelectedDate.Second),
                        HeaderText = SfPickerResources.GetLocalizedString(ColumnHeaderView.SecondHeaderText),
                    };
                    _columns[index] = _secondColumn;
                }
            }

            index = formatStringOrder.IndexOf(3);
            if (index != -1 && ((int)(validSelectedDate.Hour / 12) == (int)(oldDate.Hour / 12) || (int)(validSelectedDate.Hour / 12) == (int)(newDate.Hour / 12)))
            {
                ObservableCollection<string> meridiems = TimePickerHelper.GetMeridiem(MinimumDate, maxDate, validSelectedDate);
                ObservableCollection<string> previousCollection = _meridiemColumn.ItemsSource is ObservableCollection<string> previousMeridiemCollection ? previousMeridiemCollection : new ObservableCollection<string>();
                if (!PickerHelper.IsCollectionEquals(meridiems, previousCollection))
                {
                    _meridiemColumn = new PickerColumn()
                    {
                        ItemsSource = meridiems,
                        SelectedIndex = validSelectedDate.Hour >= 12 ? meridiems.Count > 1 ? 1 : 0 : 0,
                        HeaderText = SfPickerResources.GetLocalizedString(ColumnHeaderView.MeridiemHeaderText),
                    };
                    _columns[index] = _meridiemColumn;
                }
            }
        }

        /// <summary>
        /// Method to update the selected index value for all the picker column based on the date value.
        /// </summary>
        /// <param name="date">The selected date value.</param>
        void UpdateSelectedDateIndex(DateTime? date)
        {
            if (date == null)
            {
                return;
            }

            string dayFormat;
            string monthFormat;
            DatePickerHelper.GetFormatStringOrder(out dayFormat, out monthFormat, DateFormat);
            if (_yearColumn.ItemsSource != null && _yearColumn.ItemsSource is ObservableCollection<string> yearCollection && yearCollection.Count > 0)
            {
                int index = DatePickerHelper.GetYearIndex(yearCollection, date.Value.Year);
                if (_yearColumn.SelectedIndex != index)
                {
                    _yearColumn.SelectedIndex = index;
                }
            }

            if (_monthColumn.ItemsSource != null && _monthColumn.ItemsSource is ObservableCollection<string> monthCollection && !string.IsNullOrEmpty(monthFormat))
            {
                int index = DatePickerHelper.GetMonthIndex(monthFormat, monthCollection, date.Value.Month);
                if (_monthColumn.SelectedIndex != index)
                {
                    _monthColumn.SelectedIndex = index;
                }
            }

            if (_dayColumn.ItemsSource != null && _dayColumn.ItemsSource is ObservableCollection<string> dayCollection && !string.IsNullOrEmpty(dayFormat))
            {
                int index = DatePickerHelper.GetDayIndex(dayFormat, dayCollection, date.Value.Day);
                if (_dayColumn.SelectedIndex != index)
                {
                    _dayColumn.SelectedIndex = index;
                }
            }
        }

        /// <summary>
        /// Method to update the selected index value for all the picker column based on the selected time value.
        /// </summary>
        /// <param name="date">The selected date value.</param>
        void UpdateSelectedTimeIndex(DateTime? date)
        {
            if (date == null)
            {
                return;
            }

            string hourFormat;
            TimePickerHelper.GetFormatStringOrder(out hourFormat, TimeFormat);
            if (_hourColumn.ItemsSource != null && _hourColumn.ItemsSource is ObservableCollection<string> hourCollection && hourCollection.Count > 0)
            {
                int? index = TimePickerHelper.GetHourIndex(hourFormat, hourCollection, date.Value.Hour);
                if (index.HasValue && _hourColumn.SelectedIndex != index)
                {
                    _hourColumn.SelectedIndex = index.Value;
                }
            }

            if (_minuteColumn.ItemsSource != null && _minuteColumn.ItemsSource is ObservableCollection<string> minuteCollection && minuteCollection.Count > 0)
            {
                int index = TimePickerHelper.GetMinuteOrSecondIndex(minuteCollection, date.Value.Minute);
                if (_minuteColumn.SelectedIndex != index)
                {
                    _minuteColumn.SelectedIndex = index;
                }
            }

            if (_secondColumn.ItemsSource != null && _secondColumn.ItemsSource is ObservableCollection<string> secondCollection && secondCollection.Count > 0)
            {
                int index = TimePickerHelper.GetMinuteOrSecondIndex(secondCollection, date.Value.Second);
                if (_secondColumn.SelectedIndex != index)
                {
                    _secondColumn.SelectedIndex = index;
                }
            }

            if (_meridiemColumn.ItemsSource != null && _meridiemColumn.ItemsSource is ObservableCollection<string> meridiemCollection && meridiemCollection.Count > 0)
            {
                int index = date.Value.Hour >= 12 ? 1 : 0;
                if (_meridiemColumn.SelectedIndex != index)
                {
                    _meridiemColumn.SelectedIndex = index;
                }
            }
        }

        /// <summary>
        /// Method invokes on column header property changed.
        /// </summary>
        /// <param name="sender">Column header view value.</param>
        /// <param name="e">Property changed arguments.</param>
        void OnColumnHeaderPropertyChanged(object? sender, PickerPropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(DateTimePickerColumnHeaderView.Background))
            {
                BaseColumnHeaderView.Background = ColumnHeaderView.Background;
            }
            else if (e.PropertyName == nameof(DateTimePickerColumnHeaderView.Height))
            {
                BaseColumnHeaderView.Height = ColumnHeaderView.Height;
            }
            else if (e.PropertyName == nameof(DateTimePickerColumnHeaderView.DividerColor))
            {
                BaseColumnHeaderView.DividerColor = ColumnHeaderView.DividerColor;
            }
            else if (e.PropertyName == nameof(DateTimePickerColumnHeaderView.TextStyle))
            {
                SetInheritedBindingContext(ColumnHeaderView.TextStyle, BindingContext);
                BaseColumnHeaderView.TextStyle = ColumnHeaderView.TextStyle;
            }
            else if (e.PropertyName == nameof(DateTimePickerColumnHeaderView.DayHeaderText))
            {
                _dayColumn.HeaderText = SfPickerResources.GetLocalizedString(ColumnHeaderView.DayHeaderText);
            }
            else if (e.PropertyName == nameof(DateTimePickerColumnHeaderView.MonthHeaderText))
            {
                _monthColumn.HeaderText = SfPickerResources.GetLocalizedString(ColumnHeaderView.MonthHeaderText);
            }
            else if (e.PropertyName == nameof(DateTimePickerColumnHeaderView.YearHeaderText))
            {
                _yearColumn.HeaderText = SfPickerResources.GetLocalizedString(ColumnHeaderView.YearHeaderText);
            }
            else if (e.PropertyName == nameof(DateTimePickerColumnHeaderView.HourHeaderText))
            {
                _hourColumn.HeaderText = SfPickerResources.GetLocalizedString(ColumnHeaderView.HourHeaderText);
            }
            else if (e.PropertyName == nameof(DateTimePickerColumnHeaderView.MinuteHeaderText))
            {
                _minuteColumn.HeaderText = SfPickerResources.GetLocalizedString(ColumnHeaderView.MinuteHeaderText);
            }
            else if (e.PropertyName == nameof(DateTimePickerColumnHeaderView.SecondHeaderText))
            {
                _secondColumn.HeaderText = SfPickerResources.GetLocalizedString(ColumnHeaderView.SecondHeaderText);
            }
            else if (e.PropertyName == nameof(DateTimePickerColumnHeaderView.MeridiemHeaderText))
            {
                _meridiemColumn.HeaderText = SfPickerResources.GetLocalizedString(ColumnHeaderView.MeridiemHeaderText);
            }
        }

        /// <summary>
        /// Method invokes on header property changed.
        /// </summary>
        /// <param name="sender">Header view value.</param>
        /// <param name="e">Property changed arguments.</param>
        void OnHeaderPropertyChanged(object? sender, PickerPropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(DateTimePickerHeaderView.Background))
            {
                BaseHeaderView.Background = HeaderView.Background;
            }
            else if (e.PropertyName == nameof(DateTimePickerHeaderView.Height))
            {
                BaseHeaderView.Height = HeaderView.Height;
            }
            else if (e.PropertyName == nameof(DateTimePickerHeaderView.DividerColor))
            {
                BaseHeaderView.DividerColor = HeaderView.DividerColor;
            }
            else if (e.PropertyName == nameof(DateTimePickerHeaderView.TextStyle))
            {
                SetInheritedBindingContext(HeaderView.TextStyle, BindingContext);
                BaseHeaderView.TextStyle = HeaderView.TextStyle;
            }
            else if (e.PropertyName == nameof(DateTimePickerHeaderView.SelectionTextStyle))
            {
                SetInheritedBindingContext(HeaderView.SelectionTextStyle, BindingContext);
                BaseHeaderView.SelectionTextStyle = HeaderView.SelectionTextStyle;
            }
            else if (e.PropertyName == nameof(DateTimePickerHeaderView.DateFormat))
            {
                BaseHeaderView.DateText = GetDateHeaderText();
            }
            else if (e.PropertyName == nameof(DateTimePickerHeaderView.TimeFormat))
            {
                BaseHeaderView.TimeText = GetTimeHeaderText();
            }
        }

        /// <summary>
        /// Method to generate the day and time columns based on the selected date time value.
        /// </summary>
        void GeneratePickerColumns()
        {
            if (_selectedIndex == 0)
            {
                GenerateDatePickerColumns();
            }
            else
            {
                GenerateTimePickerColumns();
            }
        }

        /// <summary>
        /// Method to generate the day, month, year columns based on the selected date value.
        /// </summary>
        void GenerateDatePickerColumns()
        {
            string dayFormat;
            string monthFormat;
            List<int> formatStringOrder = DatePickerHelper.GetFormatStringOrder(out dayFormat, out monthFormat, DateFormat);
            DateTime maxDate = DatePickerHelper.GetValidMaxDate(MinimumDate, MaximumDate);
            DateTime date = SelectedDate ?? _previousSelectedDateTime;
            DateTime selectedDate = DatePickerHelper.GetValidDateTime(date, MinimumDate, maxDate);
            ObservableCollection<PickerColumn> pickerColumns = new ObservableCollection<PickerColumn>();
            foreach (int index in formatStringOrder)
            {
                switch (index)
                {
                    case 0:
                        _dayColumn = GenerateDayColumn(dayFormat, selectedDate);
                        if (Mode == PickerMode.Default)
                        {
                            _dayColumn.SelectedItem = SelectedDate != null ? PickerHelper.GetSelectedItemDefaultValue(_dayColumn) : null;
                        }
                        else
                        {
                            _dayColumn.SelectedItem = SelectedDate == null && _internalSelectedDateTime == null ? null : PickerHelper.GetSelectedItemDefaultValue(_dayColumn);
                        }

                        pickerColumns.Add(_dayColumn);
                        break;
                    case 1:
                        _monthColumn = GenerateMonthColumn(monthFormat, selectedDate);
                        if (Mode == PickerMode.Default)
                        {
                            _monthColumn.SelectedItem = SelectedDate != null ? PickerHelper.GetSelectedItemDefaultValue(_monthColumn) : null;
                        }
                        else
                        {
                            _monthColumn.SelectedItem = SelectedDate == null && _internalSelectedDateTime == null ? null : PickerHelper.GetSelectedItemDefaultValue(_monthColumn);
                        }

                        pickerColumns.Add(_monthColumn);
                        break;
                    case 2:
                        _yearColumn = GenerateYearColumn(selectedDate);
                        if (Mode == PickerMode.Default)
                        {
                            _yearColumn.SelectedItem = SelectedDate != null ? PickerHelper.GetSelectedItemDefaultValue(_yearColumn) : null;
                        }
                        else
                        {
                            _yearColumn.SelectedItem = SelectedDate == null && _internalSelectedDateTime == null ? null : PickerHelper.GetSelectedItemDefaultValue(_yearColumn);
                        }

                        pickerColumns.Add(_yearColumn);
                        break;
                }
            }

            _columns = pickerColumns;
        }

        /// <summary>
        /// Method to reset the picker time columns.
        /// </summary>
        void ResetTimeColumns()
        {
            _hourColumn = new PickerColumn();
            _minuteColumn = new PickerColumn();
            _secondColumn = new PickerColumn();
            _meridiemColumn = new PickerColumn();
            _columns = new ObservableCollection<PickerColumn>();
            GeneratePickerColumns();
            BaseColumns.Clear();
            BaseColumns = _columns;
        }

        /// <summary>
        /// It's need to update the selected item for each column.
        /// </summary>
        void UpdateColumnsSelectedItem()
        {
            _yearColumn.SelectedItem = PickerHelper.GetSelectedItemDefaultValue(_yearColumn);
            _monthColumn.SelectedItem = PickerHelper.GetSelectedItemDefaultValue(_monthColumn);
            _dayColumn.SelectedItem = PickerHelper.GetSelectedItemDefaultValue(_dayColumn);
            _hourColumn.SelectedItem = PickerHelper.GetSelectedItemDefaultValue(_hourColumn);
            _minuteColumn.SelectedItem = PickerHelper.GetSelectedItemDefaultValue(_minuteColumn);
            _secondColumn.SelectedItem = PickerHelper.GetSelectedItemDefaultValue(_secondColumn);
            _meridiemColumn.SelectedItem = PickerHelper.GetSelectedItemDefaultValue(_meridiemColumn);
        }

        /// <summary>
        /// Need to reset the current selected date or time value without okay button click.
        /// </summary>
        void ResetDateTimeOnViewChange()
        {
            if (_internalSelectedDateTime != null && _isCurrentPickerViewChanged)
            {
                _internalSelectedDateTime = null;
                _isCurrentPickerViewChanged = false;
            }
        }

        /// <summary>
        /// Method to generate the day column with items source and selected index based on format.
        /// </summary>
        /// <param name="format">The day format.</param>
        /// <param name="selectedDate">The valid selected date value.</param>
        /// <returns>Returns day column details.</returns>
        PickerColumn GenerateDayColumn(string format, DateTime? selectedDate)
        {
            DateTime maxDate = DatePickerHelper.GetValidMaxDate(MinimumDate, MaximumDate);
            ObservableCollection<string> days = DatePickerHelper.GetDays(format, DateTime.Now.Month, DateTime.Now.Year, MinimumDate, maxDate, DayInterval);

            return new PickerColumn()
            {
                ItemsSource = days,
                SelectedIndex = selectedDate != null ? DatePickerHelper.GetDayIndex(format, days, selectedDate.Value.Day) : _previousSelectedDateTime.Day - 1,
                HeaderText = SfPickerResources.GetLocalizedString(ColumnHeaderView.DayHeaderText),
            };
        }

        /// <summary>
        /// Method to generate the month column with items source and selected index based on format.
        /// </summary>
        /// <param name="format">The month format value.</param>
        /// <param name="selectedDate">The valid selected date value.</param>
        /// <returns>Returns the month column instance.</returns>
        PickerColumn GenerateMonthColumn(string format, DateTime? selectedDate)
        {
            DateTime maxDate = DatePickerHelper.GetValidMaxDate(MinimumDate, MaximumDate);
            ObservableCollection<string> months = DatePickerHelper.GetMonths(format, DateTime.Now.Year, MinimumDate, maxDate, MonthInterval);

            return new PickerColumn()
            {
                ItemsSource = months,
                SelectedIndex = selectedDate == null ? _previousSelectedDateTime.Month - 1 : DatePickerHelper.GetMonthIndex(format, months, selectedDate.Value.Month),
                HeaderText = SfPickerResources.GetLocalizedString(ColumnHeaderView.MonthHeaderText),
            };
        }

        /// <summary>
        /// Method to generate the year column with items source and selected index based on format.
        /// </summary>
        /// <param name="selectedDate">The valid selected date value.</param>
        /// <returns>Returns the year column instance.</returns>
        PickerColumn GenerateYearColumn(DateTime? selectedDate)
        {
            DateTime maxDate = DatePickerHelper.GetValidMaxDate(MinimumDate, MaximumDate);
            ObservableCollection<string> years = DatePickerHelper.GetYears(MinimumDate, maxDate, YearInterval);
            return new PickerColumn()
            {
                ItemsSource = years,
                SelectedIndex = selectedDate != null ? DatePickerHelper.GetYearIndex(years, selectedDate.Value.Year) : years.IndexOf(_previousSelectedDateTime.Year.ToString()),
                HeaderText = SfPickerResources.GetLocalizedString(ColumnHeaderView.YearHeaderText),
            };
        }

        /// <summary>
        /// Method to generate the hour, minute, second and meridiem columns based on the selected time value.
        /// </summary>
        void GenerateTimePickerColumns()
        {
            string hourFormat;
            List<int> formatStringOrder = TimePickerHelper.GetFormatStringOrder(out hourFormat, TimeFormat);
            DateTime maxDate = DatePickerHelper.GetValidMaxDate(MinimumDate, MaximumDate);
            DateTime date = SelectedDate ?? _previousSelectedDateTime;
            DateTime selectedDate = DatePickerHelper.GetValidDateTime(date, MinimumDate, maxDate);
            TimeSpan selectedTime = new TimeSpan(selectedDate.Hour, selectedDate.Minute, selectedDate.Second);
            ObservableCollection<PickerColumn> pickerColumns = new ObservableCollection<PickerColumn>();
            foreach (int index in formatStringOrder)
            {
                switch (index)
                {
                    case 0:
                        _hourColumn = GenerateHourColumn(hourFormat, selectedTime, selectedDate);
                        if (Mode == PickerMode.Default)
                        {
                            _hourColumn.SelectedItem = SelectedDate != null ? PickerHelper.GetSelectedItemDefaultValue(_hourColumn) : null;
                        }
                        else
                        {
                            _hourColumn.SelectedItem = SelectedDate == null && _internalSelectedDateTime == null ? null : PickerHelper.GetSelectedItemDefaultValue(_hourColumn);
                        }

                        pickerColumns.Add(_hourColumn);
                        break;
                    case 1:
                        _minuteColumn = GenerateMinuteColumn(selectedTime, selectedDate);
                        if (Mode == PickerMode.Default)
                        {
                            _minuteColumn.SelectedItem = SelectedDate != null ? PickerHelper.GetSelectedItemDefaultValue(_minuteColumn) : null;
                        }
                        else
                        {
                            _minuteColumn.SelectedItem = SelectedDate == null && _internalSelectedDateTime == null ? null : PickerHelper.GetSelectedItemDefaultValue(_minuteColumn);
                        }

                        pickerColumns.Add(_minuteColumn);
                        break;
                    case 2:
                        _secondColumn = GenerateSecondColumn(selectedTime, selectedDate);
                        if (this.Mode == PickerMode.Default)
                        {
                            _secondColumn.SelectedItem = SelectedDate != null ? PickerHelper.GetSelectedItemDefaultValue(_secondColumn) : null;
                        }
                        else
                        {
                            _secondColumn.SelectedItem = SelectedDate == null && _internalSelectedDateTime == null ? null : PickerHelper.GetSelectedItemDefaultValue(_secondColumn);
                        }

                        pickerColumns.Add(_secondColumn);
                        break;
                    case 3:
                        _meridiemColumn = GenerateMeridiemColumn(selectedTime, selectedDate);
                        if (Mode == PickerMode.Default)
                        {
                            _meridiemColumn.SelectedItem = SelectedDate != null ? PickerHelper.GetSelectedItemDefaultValue(_meridiemColumn) : null;
                        }
                        else
                        {
                            _meridiemColumn.SelectedItem = SelectedDate == null && _internalSelectedDateTime == null ? null : PickerHelper.GetSelectedItemDefaultValue(_meridiemColumn);
                        }

                        pickerColumns.Add(_meridiemColumn);
                        break;
                }
            }

            _columns = pickerColumns;
        }

        /// <summary>
        /// Method to generate the hour column with items source and selected index based on format.
        /// </summary>
        /// <param name="format">The hour format.</param>
        /// <param name="selectedTime">The selected time value.</param>
        /// <param name="selectedDate">The valid selected date value.</param>
        /// <returns>Returns hour column details.</returns>
        PickerColumn GenerateHourColumn(string format, TimeSpan? selectedTime, DateTime? selectedDate)
        {
            DateTime? minimumDate = null;
            DateTime? maximumDate = null;
            DateTime maxDate = DatePickerHelper.GetValidMaxDate(MinimumDate, MaximumDate);
            if (selectedDate != null && selectedDate.Value.Date <= MinimumDate.Date)
            {
                minimumDate = MinimumDate;
            }

            if (selectedDate != null && selectedDate.Value.Date >= maxDate.Date)
            {
                maximumDate = maxDate;
            }

            ObservableCollection<string> hours = TimePickerHelper.GetHours(format, HourInterval, selectedDate, minimumDate, maximumDate);

            int? hourIndex = selectedTime != null ? TimePickerHelper.GetHourIndex(format, hours, selectedTime.Value.Hours) : _previousSelectedDateTime.Hour;

            return new PickerColumn()
            {
                ItemsSource = hours,
                SelectedIndex = hourIndex != null ? (int)hourIndex : _previousSelectedDateTime.Hour,
                HeaderText = SfPickerResources.GetLocalizedString(ColumnHeaderView.HourHeaderText),
            };
        }

        /// <summary>
        /// Method to generate the minute column with items source and selected index based on format.
        /// </summary>
        /// <param name="selectedTime">The selected time value.</param>
        /// <param name="selectedDate">The valid selected date value.</param>
        /// <returns>Returns minute column details.</returns>
        PickerColumn GenerateMinuteColumn(TimeSpan? selectedTime, DateTime? selectedDate)
        {
            DateTime? minimumDate = null;
            DateTime? maximumDate = null;
            DateTime maxDate = DatePickerHelper.GetValidMaxDate(MinimumDate, MaximumDate);
            DateTime? date = SelectedDate ?? _previousSelectedDateTime;
            if (date.Value.Date <= MinimumDate.Date)
            {
                minimumDate = MinimumDate;
            }

            if (date.Value.Date >= maxDate.Date)
            {
                maximumDate = maxDate;
            }

            ObservableCollection<string> minutes = TimePickerHelper.GetMinutes(MinuteInterval, date.Value.Hour, selectedDate, minimumDate, maximumDate);
            return new PickerColumn()
            {
                ItemsSource = minutes,
                SelectedIndex = selectedTime != null ? TimePickerHelper.GetMinuteOrSecondIndex(minutes, date.Value.Minute) : _previousSelectedDateTime.Minute,
                HeaderText = SfPickerResources.GetLocalizedString(ColumnHeaderView.MinuteHeaderText),
            };
        }

        /// <summary>
        /// Method to generate the second column with items source and selected index based on format.
        /// </summary>
        /// <param name="selectedTime">The selected time value.</param>
        /// <param name="selectedDate">The valid selected date value.</param>
        /// <returns>Returns second column details.</returns>
        PickerColumn GenerateSecondColumn(TimeSpan? selectedTime, DateTime? selectedDate)
        {
            DateTime? minimumDate = null;
            DateTime? maximumDate = null;
            DateTime maxDate = DatePickerHelper.GetValidMaxDate(MinimumDate, MaximumDate);
            DateTime date = SelectedDate ?? _previousSelectedDateTime;
            if (date.Date <= MinimumDate.Date)
            {
                minimumDate = MinimumDate;
            }

            if (date.Date >= maxDate.Date)
            {
                maximumDate = maxDate;
            }

            ObservableCollection<string> seconds = TimePickerHelper.GetSeconds(SecondInterval, date.Hour, date.Minute, selectedDate, minimumDate, maximumDate);
            return new PickerColumn()
            {
                ItemsSource = seconds,
                SelectedIndex = selectedTime != null ? TimePickerHelper.GetMinuteOrSecondIndex(seconds, selectedTime.Value.Seconds) : _previousSelectedDateTime.Second,
                HeaderText = SfPickerResources.GetLocalizedString(ColumnHeaderView.SecondHeaderText),
            };
        }

        /// <summary>
        /// Method to generate the meridiem column with items source and selected index based on format.
        /// </summary>
        /// <param name="selectedTime">The selected time value.</param>
        /// <param name="selectedDate">The valid selected date value.</param>
        /// <returns>Returns meridiem column details.</returns>
        PickerColumn GenerateMeridiemColumn(TimeSpan? selectedTime, DateTime? selectedDate)
        {
            DateTime? minimumDate = null;
            DateTime? maximumDate = null;
            DateTime maxDate = DatePickerHelper.GetValidMaxDate(MinimumDate, MaximumDate);
            if (selectedDate != null && selectedDate.Value.Date <= MinimumDate.Date)
            {
                minimumDate = MinimumDate;
            }

            if (selectedDate != null && selectedDate.Value.Date >= maxDate.Date)
            {
                maximumDate = maxDate;
            }

            ObservableCollection<string> meridiems = TimePickerHelper.GetMeridiem(minimumDate, maximumDate, selectedDate);
            return new PickerColumn()
            {
                ItemsSource = meridiems,
                SelectedIndex = selectedTime != null && selectedTime.Value.Hours >= 12 ? meridiems.Count > 1 ? 1 : 0 : 0,
                HeaderText = SfPickerResources.GetLocalizedString(ColumnHeaderView.MeridiemHeaderText),
            };
        }

        /// <summary>
        /// Method trigged when the black out datetime collection gets changed.
        /// </summary>
        /// <param name="sender">Datetime picker instance.</param>
        /// <param name="e">Collection changed event arguments.</param>
        void OnBlackoutDateTimes_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (SelectedDate != null)
            {
                DateTime currentDate = SelectedDate.Value;
                bool isTimeSpanAtZero = false;
                while (BlackoutDateTimes.Any(blackOutDate => DatePickerHelper.IsBlackoutDateTime(blackOutDate, currentDate, out isTimeSpanAtZero)))
                {
                    currentDate = isTimeSpanAtZero ? currentDate.AddDays(1) : currentDate.AddMinutes(1);
                }

                if (SelectedDate != currentDate)
                {
                    SelectedDate = currentDate;
                }
            }
        }

        /// <summary>
        /// Method to initialize the theme and to set dynamic resources.
        /// </summary>
        void InitializeTheme()
        {
            ThemeElement.InitializeThemeResources(this, "SfDateTimePickerTheme");

            SetDynamicResource(FooterTextColorProperty, "SfDateTimePickerNormalFooterTextColor");
            SetDynamicResource(FooterFontSizeProperty, "SfDateTimePickerNormalFooterFontSize");
        }

        /// <summary>
        /// Method to initialize the defult picker style.
        /// </summary>
        void IntializePickerStyle()
        {
            SetDynamicResource(DateTimePickerBackgroundProperty, "SfDateTimePickerNormalBackground");

            SetDynamicResource(SelectedTextColorProperty, "SfDateTimePickerSelectedTextColor");
            SetDynamicResource(SelectionTextColorProperty, "SfDateTimePickerSelectionTextColor");
            SetDynamicResource(SelectedFontSizeProperty, "SfDateTimePickerSelectedFontSize");

            SetDynamicResource(NormalTextColorProperty, "SfDateTimePickerNormalTextColor");
            SetDynamicResource(NormalFontSizeProperty, "SfDateTimePickerNormalFontSize");

            SetDynamicResource(DisabledTextColorProperty, "SfDateTimePickerDisabledTextColor");
        }

        #endregion

        #region Override Methods

        /// <summary>
        /// Method to wire the events.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            BaseColumns = _columns;
            if (HeaderView != null)
            {
                SetInheritedBindingContext(HeaderView, BindingContext);
                HeaderView.PickerPropertyChanged += OnHeaderPropertyChanged;
                BaseHeaderView = new PickerHeaderView()
                {
                    Background = HeaderView.Background,
                    DividerColor = HeaderView.DividerColor,
                    Height = HeaderView.Height,
                    TextStyle = HeaderView.TextStyle,
                    SelectionTextStyle = HeaderView.SelectionTextStyle,
                    TimeText = GetTimeHeaderText(),
                    DateText = GetDateHeaderText(),
                };
            }

            if (ColumnHeaderView != null)
            {
                SetInheritedBindingContext(ColumnHeaderView, BindingContext);
                ColumnHeaderView.PickerPropertyChanged += OnColumnHeaderPropertyChanged;
                BaseColumnHeaderView = new PickerColumnHeaderView()
                {
                    Background = ColumnHeaderView.Background,
                    DividerColor = ColumnHeaderView.DividerColor,
                    Height = ColumnHeaderView.Height,
                    TextStyle = ColumnHeaderView.TextStyle,
                };
            }
        }

        /// <summary>
        /// Method triggers when the property binding context changed.
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

                if (HeaderView.SelectionTextStyle != null)
                {
                    SetInheritedBindingContext(HeaderView.SelectionTextStyle, BindingContext);
                }
            }

            if (ColumnHeaderView != null)
            {
                SetInheritedBindingContext(ColumnHeaderView, BindingContext);
                if (ColumnHeaderView.TextStyle != null)
                {
                    SetInheritedBindingContext(ColumnHeaderView.TextStyle, BindingContext);
                }
            }

            if (FooterView != null)
            {
                SetInheritedBindingContext(FooterView, BindingContext);
                if (FooterView.TextStyle != null)
                {
                    SetInheritedBindingContext(FooterView.TextStyle, BindingContext);
                }
            }

            if (SelectedTextStyle != null)
            {
                SetInheritedBindingContext(SelectedTextStyle, BindingContext);
            }

            if (TextStyle != null)
            {
                SetInheritedBindingContext(TextStyle, BindingContext);
            }

            if (SelectionView != null)
            {
                SetInheritedBindingContext(SelectionView, BindingContext);
            }
        }

        /// <summary>
        /// Method triggers while the header button clicked.
        /// </summary>
        /// <param name="index">Index of the header button.</param>
        protected override void OnHeaderButtonClicked(int index)
        {
            if (_selectedIndex == index)
            {
                return;
            }

            _selectedIndex = index;
            _isCurrentPickerViewChanged = true;
            if (_selectedIndex == 0)
            {
                ResetDateColumns();
            }
            else
            {
                ResetTimeColumns();
            }
        }

        /// <summary>
        /// Method triggers while the popup opening or switched from popup to default.
        /// </summary>
        protected override void OnPickerLoading()
        {
            if (SelectedDate == null)
            {
                return;
            }

            if (_selectedIndex == 0)
            {
                string dayFormat;
                string monthFormat;
                List<int> formatStringOrder = DatePickerHelper.GetFormatStringOrder(out dayFormat, out monthFormat, DateFormat);
                DateTime selectedDate = DatePickerHelper.GetValidDateTime(SelectedDate, MinimumDate, MaximumDate);
                foreach (int index in formatStringOrder)
                {
                    switch (index)
                    {
                        case 0:
                            int dayIndex = DatePickerHelper.GetDayIndex(dayFormat, (ObservableCollection<string>)_dayColumn.ItemsSource, selectedDate.Day);
                            if (_dayColumn.SelectedIndex != dayIndex)
                            {
                                _dayColumn.SelectedIndex = dayIndex;
                            }

                            break;
                        case 1:
                            int monthIndex = DatePickerHelper.GetMonthIndex(monthFormat, (ObservableCollection<string>)_monthColumn.ItemsSource, selectedDate.Month);
                            if (_monthColumn.SelectedIndex != monthIndex)
                            {
                                _monthColumn.SelectedIndex = monthIndex;
                            }

                            break;
                        case 2:
                            int yearIndex = DatePickerHelper.GetYearIndex((ObservableCollection<string>)_yearColumn.ItemsSource, selectedDate.Year);
                            if (_yearColumn.SelectedIndex != yearIndex)
                            {
                                _yearColumn.SelectedIndex = yearIndex;
                            }

                            break;
                    }
                }
            }
            else
            {
                _selectedIndex = 0;
                ResetHeaderHighlight();
                ResetDateColumns();
            }
        }

        /// <summary>
        /// Method triggers when the date time picker popup closed.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnPopupClosed(EventArgs e)
        {
            InvokeClosedEvent(this, e);
        }

        /// <summary>
        /// Method triggers when the date time picker popup closing.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnPopupClosing(CancelEventArgs e)
        {
            InvokeClosingEvent(this, e);
        }

        /// <summary>
        /// Method triggers when the date time picker popup opened.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnPopupOpened(EventArgs e)
        {
            InvokeOpenedEvent(this, e);
        }

        /// <summary>
        /// Method triggers when the date time picker ok button clicked.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnOkButtonClicked(EventArgs e)
        {
            // If the picker is not in Default mode and an internal selected date-time exists
            if (IsScrollSelectionAllowed() && _internalSelectedDateTime != null)
            {
                // If the internal selected date-time is different from the currently selected date
                if (!DatePickerHelper.IsSameDateTime(_internalSelectedDateTime, SelectedDate))
                {
                    // Update the selected date with the internal selected date-time
                    SelectedDate = _internalSelectedDateTime.Value;
                    // Clear the internal selected date-time after applying it
                    _internalSelectedDateTime = null;
                }
            }

            InvokeOkButtonClickedEvent(this, e);
            if (AcceptCommand != null && AcceptCommand.CanExecute(e))
            {
                AcceptCommand.Execute(e);
            }
        }

        /// <summary>
        /// Method triggers when the date time picker cancel button clicked.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnCancelButtonClicked(EventArgs e)
        {
            // If the picker is not in Default mode
            if (IsScrollSelectionAllowed())
            {
                // If a date is currently selected
                if (SelectedDate != null)
                {
                    // Update the selected index in the UI to reflect the selected date
                    UpdateSelectedIndex(SelectedDate);
                }
                else
                {
                    // If no date is selected, clear all column selections (date and time parts)
                    _dayColumn.SelectedItem = null;
                    _monthColumn.SelectedItem = null;
                    _yearColumn.SelectedItem = null;
                    _hourColumn.SelectedItem = null;
                    _minuteColumn.SelectedItem = null;
                    _secondColumn.SelectedItem = null;
                    BaseHeaderView.DateText = SfPickerResources.GetLocalizedString("Date");
                    BaseHeaderView.TimeText = SfPickerResources.GetLocalizedString("Time");
                }
                // Clear the internal selected date-time if it exists
                if (_internalSelectedDateTime != null)
                {
                    _internalSelectedDateTime = null;
                }
            }

            InvokeCancelButtonClickedEvent(this, e);
            if (DeclineCommand != null && DeclineCommand.CanExecute(e))
            {
                DeclineCommand.Execute(e);
            }
        }

        #endregion

        #region Property Changed Methods

        /// <summary>
        /// Method invokes on picker header view changed.
        /// </summary>
        /// <param name="bindable">The picker object.</param>
        /// <param name="oldValue">Old value of the property.</param>
        /// <param name="newValue">New value of the property.</param>
        static void OnHeaderViewChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfDateTimePicker? picker = bindable as SfDateTimePicker;
            if (picker == null)
            {
                return;
            }

            if (oldValue is DateTimePickerHeaderView oldStyle)
            {
                picker.HeaderView.PickerPropertyChanged -= picker.OnHeaderPropertyChanged;
                picker.HeaderView.BindingContext = null;
                oldStyle.Parent = null;
            }

            if (newValue is DateTimePickerHeaderView newStyle)
            {
                newStyle.Parent = picker;
                SetInheritedBindingContext(picker.HeaderView, picker.BindingContext);
                picker.HeaderView.PickerPropertyChanged += picker.OnHeaderPropertyChanged;
                picker.BaseHeaderView = new PickerHeaderView()
                {
                    Background = picker.HeaderView.Background,
                    DividerColor = picker.HeaderView.DividerColor,
                    Height = picker.HeaderView.Height,
                    TextStyle = picker.HeaderView.TextStyle,
                    SelectionTextStyle = picker.HeaderView.SelectionTextStyle,
                    TimeText = picker.GetTimeHeaderText(),
                    DateText = picker.GetDateHeaderText(),
                };
            }
        }

        /// <summary>
        /// Method invokes on picker column header view changed.
        /// </summary>
        /// <param name="bindable">The picker object.</param>
        /// <param name="oldValue">Old value of the property.</param>
        /// <param name="newValue">New value of the property.</param>
        static void OnColumnHeaderViewChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfDateTimePicker? picker = bindable as SfDateTimePicker;
            if (picker == null)
            {
                return;
            }

            if (oldValue is DateTimePickerColumnHeaderView oldStyle)
            {
                oldStyle.PickerPropertyChanged -= picker.OnColumnHeaderPropertyChanged;
                oldStyle.BindingContext = null;
                oldStyle.Parent = null;
            }

            if (newValue is DateTimePickerColumnHeaderView newStyle)
            {
                newStyle.Parent = picker;
                SetInheritedBindingContext(newStyle, picker.BindingContext);
                newStyle.PickerPropertyChanged += picker.OnColumnHeaderPropertyChanged;
                picker.BaseColumnHeaderView = new PickerColumnHeaderView()
                {
                    Background = newStyle.Background,
                    DividerColor = newStyle.DividerColor,
                    Height = newStyle.Height,
                    TextStyle = newStyle.TextStyle,
                };

                picker._dayColumn.HeaderText = SfPickerResources.GetLocalizedString(picker.ColumnHeaderView.DayHeaderText);
                picker._monthColumn.HeaderText = SfPickerResources.GetLocalizedString(picker.ColumnHeaderView.MonthHeaderText);
                picker._yearColumn.HeaderText = SfPickerResources.GetLocalizedString(picker.ColumnHeaderView.YearHeaderText);
                picker._hourColumn.HeaderText = SfPickerResources.GetLocalizedString(picker.ColumnHeaderView.HourHeaderText);
                picker._minuteColumn.HeaderText = SfPickerResources.GetLocalizedString(picker.ColumnHeaderView.MinuteHeaderText);
                picker._secondColumn.HeaderText = SfPickerResources.GetLocalizedString(picker.ColumnHeaderView.SecondHeaderText);
                picker._meridiemColumn.HeaderText = SfPickerResources.GetLocalizedString(picker.ColumnHeaderView.MeridiemHeaderText);
            }
        }

        /// <summary>
        /// Method invokes on selected date property changed.
        /// </summary>
        /// <param name="bindable">The picker object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnSelectedDatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfDateTimePicker? picker = bindable as SfDateTimePicker;
            if (picker == null)
            {
                return;
            }

            DateTime? previousSelectedDate = null;
            DateTime? currentSelectedDate = null;
            if (oldValue is DateTime oldSelectedDate)
            {
                previousSelectedDate = oldSelectedDate;
                bool isTimeSpanAtZero = false;
                //// Prevents Selection changed event from triggering if old value is black out date time.
                if (picker.BlackoutDateTimes.Any(blackOutDateTime => DatePickerHelper.IsBlackoutDateTime(blackOutDateTime, previousSelectedDate, out isTimeSpanAtZero)))
                {
                    picker.UpdateSelectedIndex((DateTime)newValue);

                    //// Skip the update and event call by checking if the whole date is blackout value or particular time.
                    if (isTimeSpanAtZero)
                    {
                        if (oldSelectedDate.Year == picker._previousSelectedDateTime.Year && oldSelectedDate.Month == picker._previousSelectedDateTime.Month)
                        {
                            return;
                        }
                    }
                    else
                    {
                        if (oldSelectedDate.Hour == picker._previousSelectedDateTime.Hour)
                        {
                            return;
                        }
                    }

                    previousSelectedDate = picker._previousSelectedDateTime;
                }

                picker._previousSelectedDateTime = oldSelectedDate;
            }

            if (newValue is DateTime newSelectedDate)
            {
                //// Prevents Selection changed event from triggering if new value is black out date time.
                if (picker.BlackoutDateTimes.Any(blackOutDateTime => DatePickerHelper.IsBlackoutDateTime(blackOutDateTime, newSelectedDate, out bool isTimeSpanAtZero)))
                {
                    return;
                }

                currentSelectedDate = newSelectedDate;
            }

            //// Skip the update and event call while the same date updated with different time value.
            if (DatePickerHelper.IsSameDateTime(previousSelectedDate, currentSelectedDate))
            {
                return;
            }

            if (newValue == null)
            {
                picker._yearColumn.SelectedItem = null;
                picker._monthColumn.SelectedItem = null;
                picker._dayColumn.SelectedItem = null;
                picker._hourColumn.SelectedItem = null;
                picker._minuteColumn.SelectedItem = null;
                picker._secondColumn.SelectedItem = null;
                picker._meridiemColumn.SelectedItem = null;
                picker.UpdateSelectedDateIndex(previousSelectedDate);
                picker.UpdateSelectedTimeIndex(previousSelectedDate);
                picker.SelectionChanged?.Invoke(picker, new DateTimePickerSelectionChangedEventArgs() { OldValue = previousSelectedDate, NewValue = currentSelectedDate });
                picker.BaseHeaderView.DateText = picker.GetDateHeaderText();
                picker.BaseHeaderView.TimeText = picker.GetTimeHeaderText();
                if (picker.IsScrollSelectionAllowed())
                {
                    if (picker._internalSelectedDateTime != null)
                    {
                        picker._internalSelectedDateTime = null;
                    }
                }

                return;
            }
            else
            {
                picker.UpdateColumnsSelectedItem();
                PickerContainer? pickerContainer = picker.GetPickerContainerValue();
                pickerContainer?.UpdateScrollViewDraw();
                pickerContainer?.InvalidateDrawable();
            }

            //// Update the column with valid date(date between min and max date).
            currentSelectedDate = DatePickerHelper.GetValidDateTime(currentSelectedDate, picker.MinimumDate, picker.MaximumDate);
            var dateTimePickerSelectionChangedEventArgs = new DateTimePickerSelectionChangedEventArgs() { OldValue = previousSelectedDate, NewValue = currentSelectedDate };
            if (picker.SelectionChanged != null)
            {
                picker.SelectionChanged?.Invoke(picker, dateTimePickerSelectionChangedEventArgs);
            }

            if (picker.SelectionChangedCommand != null && picker.SelectionChangedCommand.CanExecute(dateTimePickerSelectionChangedEventArgs))
            {
                picker.SelectionChangedCommand.Execute(dateTimePickerSelectionChangedEventArgs);
            }

            picker.UpdateSelectedIndex(currentSelectedDate);
        }

        /// <summary>
        /// Method invokes on date format property changed.
        /// </summary>
        /// <param name="bindable">The picker object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnDateFormatPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfDateTimePicker? picker = bindable as SfDateTimePicker;
            if (picker == null)
            {
                return;
            }

            picker.ResetDateColumns();
        }

        /// <summary>
        /// Method invokes on time format property changed.
        /// </summary>
        /// <param name="bindable">The picker object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnTimeFormatPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfDateTimePicker? picker = bindable as SfDateTimePicker;
            if (picker == null)
            {
                return;
            }

            picker.ResetTimeColumns();
        }

        /// <summary>
        /// Method invokes on minimum date property changed.
        /// </summary>
        /// <param name="bindable">The picker object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnMinimumDatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfDateTimePicker? picker = bindable as SfDateTimePicker;
            if (picker == null)
            {
                return;
            }

            picker.UpdateMinimumMaximumDate(oldValue, newValue);
            DateTime currentSelectedDate = DatePickerHelper.GetValidDateTime(picker.SelectedDate, picker.MinimumDate, picker.MaximumDate);
            picker.UpdateSelectedIndex(currentSelectedDate);
        }

        /// <summary>
        /// Method invokes on maximum date property changed.
        /// </summary>
        /// <param name="bindable">The picker object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnMaximumDatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfDateTimePicker? picker = bindable as SfDateTimePicker;
            if (picker == null)
            {
                return;
            }

            DateTime newDate = DatePickerHelper.GetValidMaxDate(picker.MinimumDate, (DateTime)newValue);
            picker.UpdateMinimumMaximumDate(oldValue, newDate);
            DateTime currentSelectedDate = DatePickerHelper.GetValidDateTime(picker.SelectedDate, picker.MinimumDate, newDate);
            picker.UpdateSelectedIndex(currentSelectedDate);
        }

        /// <summary>
        /// Method invokes on day interval property changed.
        /// </summary>
        /// <param name="bindable">The picker object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnDayIntervalPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfDateTimePicker? picker = bindable as SfDateTimePicker;
            if (picker == null || (int)newValue <= 0)
            {
                return;
            }

            string dayFormat;
            //// Get the day format and format string order and check the index.
            List<int> formatStringOrder = DatePickerHelper.GetFormatStringOrder(out dayFormat, out _, picker.DateFormat);
            if (string.IsNullOrEmpty(dayFormat) || picker._selectedIndex != 0)
            {
                return;
            }

            DateTime maxDate = DatePickerHelper.GetValidMaxDate(picker.MinimumDate, picker.MaximumDate);
            DateTime currentSelectedDate = DatePickerHelper.GetValidDateTime(picker.SelectedDate, picker.MinimumDate, maxDate);
            picker._dayColumn = picker.GenerateDayColumn(dayFormat, currentSelectedDate);
            int dayIndex = formatStringOrder.IndexOf(0);
            //// Replace the day column with day interval.
            picker._columns[dayIndex] = picker._dayColumn;
        }

        /// <summary>
        /// Method invokes on month interval property changed.
        /// </summary>
        /// <param name="bindable">The picker object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnMonthIntervalPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfDateTimePicker? picker = bindable as SfDateTimePicker;
            if (picker == null || (int)newValue <= 0)
            {
                return;
            }

            string monthFormat;
            //// Get the month format and format string order and check the index.
            List<int> formatStringOrder = DatePickerHelper.GetFormatStringOrder(out _, out monthFormat, picker.DateFormat);
            if (string.IsNullOrEmpty(monthFormat) || picker._selectedIndex != 0)
            {
                return;
            }

            DateTime maxDate = DatePickerHelper.GetValidMaxDate(picker.MinimumDate, picker.MaximumDate);
            DateTime currentSelectedDate = DatePickerHelper.GetValidDateTime(picker.SelectedDate, picker.MinimumDate, maxDate);
            picker._monthColumn = picker.GenerateMonthColumn(monthFormat, currentSelectedDate);
            int monthIndex = formatStringOrder.IndexOf(1);
            //// Replace the month column with month interval.
            picker._columns[monthIndex] = picker._monthColumn;
        }

        /// <summary>
        /// Method invokes on year interval property changed.
        /// </summary>
        /// <param name="bindable">The picker object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnYearIntervalPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfDateTimePicker? picker = bindable as SfDateTimePicker;
            if (picker == null || (int)newValue <= 0)
            {
                return;
            }

            //// Get the format string order and check the index.
            List<int> formatStringOrder = DatePickerHelper.GetFormatStringOrder(out _, out _, picker.DateFormat);
            int yearIndex = formatStringOrder.IndexOf(2);
            if (yearIndex == -1 || picker._selectedIndex != 0)
            {
                return;
            }

            DateTime maxDate = DatePickerHelper.GetValidMaxDate(picker.MinimumDate, picker.MaximumDate);
            DateTime currentSelectedDate = DatePickerHelper.GetValidDateTime(picker.SelectedDate, picker.MinimumDate, maxDate);
            picker._yearColumn = picker.GenerateYearColumn(currentSelectedDate);
            //// Replace the year column with year interval.
            picker._columns[yearIndex] = picker._yearColumn;
        }

        /// <summary>
        /// Method invokes on hour interval property changed.
        /// </summary>
        /// <param name="bindable">The picker object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnHourIntervalPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfDateTimePicker? picker = bindable as SfDateTimePicker;
            if (picker == null || (int)newValue <= 0)
            {
                return;
            }

            string hourFormat;
            //// Get the format string order and check the index.
            List<int> formatStringOrder = TimePickerHelper.GetFormatStringOrder(out hourFormat, picker.TimeFormat);
            DateTime maxDate = DatePickerHelper.GetValidMaxDate(picker.MinimumDate, picker.MaximumDate);
            DateTime selectedDate = DatePickerHelper.GetValidDateTime(picker.SelectedDate, picker.MinimumDate, maxDate);
            TimeSpan selectedTime = new TimeSpan(selectedDate.Hour, selectedDate.Minute, selectedDate.Second);
            picker._hourColumn = picker.GenerateHourColumn(hourFormat, selectedTime, selectedDate);
            if (picker._selectedIndex == 1)
            {
                int hourIndex = formatStringOrder.IndexOf(0);
                //// Replace the hour column with hour interval.
                picker._columns[hourIndex] = picker._hourColumn;
            }
        }

        /// <summary>
        /// Method invokes on minute interval property changed.
        /// </summary>
        /// <param name="bindable">The picker object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnMinuteIntervalPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfDateTimePicker? picker = bindable as SfDateTimePicker;
            if (picker == null || (int)newValue <= 0)
            {
                return;
            }

            //// Get the format string order and check the index.
            List<int> formatStringOrder = TimePickerHelper.GetFormatStringOrder(out _, picker.TimeFormat);
            int minuteIndex = formatStringOrder.IndexOf(1);
            if (minuteIndex == -1)
            {
                return;
            }

            DateTime maxDate = DatePickerHelper.GetValidMaxDate(picker.MinimumDate, picker.MaximumDate);
            DateTime selectedDate = DatePickerHelper.GetValidDateTime(picker.SelectedDate, picker.MinimumDate, maxDate);
            TimeSpan selectedTime = new TimeSpan(selectedDate.Hour, selectedDate.Minute, selectedDate.Second);
            if (picker._selectedIndex == 1)
            {
                picker._minuteColumn = picker.GenerateMinuteColumn(selectedTime, selectedDate);
                //// Replace the minute column with minute interval.
                picker._columns[minuteIndex] = picker._minuteColumn;
            }
        }

        /// <summary>
        /// Method invokes on second interval property changed.
        /// </summary>
        /// <param name="bindable">The picker object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnSecondIntervalPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfDateTimePicker? picker = bindable as SfDateTimePicker;
            if (picker == null || (int)newValue <= 0)
            {
                return;
            }

            //// Get the format string order and check the index.
            List<int> formatStringOrder = TimePickerHelper.GetFormatStringOrder(out _, picker.TimeFormat);
            int secondIndex = formatStringOrder.IndexOf(2);
            if (secondIndex == -1)
            {
                return;
            }

            DateTime maxDate = DatePickerHelper.GetValidMaxDate(picker.MinimumDate, picker.MaximumDate);
            DateTime selectedDate = DatePickerHelper.GetValidDateTime(picker.SelectedDate, picker.MinimumDate, maxDate);
            TimeSpan selectedTime = new TimeSpan(selectedDate.Hour, selectedDate.Minute, selectedDate.Second);
            if (picker._selectedIndex == 1)
            {
                picker._secondColumn = picker.GenerateSecondColumn(selectedTime, selectedDate);
                //// Replace the second column with second interval.
                picker._columns[secondIndex] = picker._secondColumn;
            }
        }

        /// <summary>
        /// Method invokes on blackout datetimes property changed.
        /// </summary>
        /// <param name="bindable">The sfdatetimepicker object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnBlackOutDateTimesPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfDateTimePicker? datetimepicker = bindable as SfDateTimePicker;
            if (datetimepicker == null)
            {
                return;
            }

            //// Unwires collection changed from old and wires on new collection.
            ((ObservableCollection<DateTime>)oldValue).CollectionChanged -= datetimepicker.OnBlackoutDateTimes_CollectionChanged;
            ((ObservableCollection<DateTime>)newValue).CollectionChanged += datetimepicker.OnBlackoutDateTimes_CollectionChanged;

            if (datetimepicker.SelectedDate != null)
            {
                DateTime currentDate = datetimepicker.SelectedDate.Value;
                bool isTimeSpanAtZero = false;
                while (datetimepicker.BlackoutDateTimes.Any(blackOutDate => DatePickerHelper.IsBlackoutDateTime(blackOutDate, currentDate, out isTimeSpanAtZero)))
                {
                    currentDate = isTimeSpanAtZero ? currentDate.AddDays(1) : currentDate.AddMinutes(1);
                }

                if (datetimepicker.SelectedDate != currentDate)
                {
                    datetimepicker.SelectedDate = currentDate;
                }
            }

            //// Gets picker container value to update the view.
            PickerContainer? pickerContainer = datetimepicker.GetPickerContainerValue();

            pickerContainer?.UpdateScrollViewDraw();
            pickerContainer?.UpdatePickerSelectionView();
        }

        #endregion

        #region Internal Property Changed Methods

        /// <summary>
        /// called when <see cref="DateTimePickerBackground"/> property changed.
        /// </summary>
        /// <param name="bindable">The bindable object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        static void OnDateTimePickerBackgroundChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfDateTimePicker? picker = bindable as SfDateTimePicker;
            if (picker == null)
            {
                return;
            }

            picker.BackgroundColor = picker.DateTimePickerBackground;
        }

        /// <summary>
        /// Method invokes on the picker footer text color changed.
        /// </summary>
        /// <param name="bindable">The footer text style object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnFooterTextColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfDateTimePicker? picker = bindable as SfDateTimePicker;
            if (picker == null)
            {
                return;
            }

            picker.FooterView.TextStyle.TextColor = picker.FooterTextColor;
        }

        /// <summary>
        /// Method invokes on the picker footer font size changed.
        /// </summary>
        /// <param name="bindable">The footer text style object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnFooterFontSizeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfDateTimePicker? picker = bindable as SfDateTimePicker;
            if (picker == null)
            {
                return;
            }

            picker.FooterView.TextStyle.FontSize = picker.FooterFontSize;
        }

        /// <summary>
        /// Method invokes on the picker selection text color changed.
        /// </summary>
        /// <param name="bindable">The selection text style object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnSelectedTextColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfDateTimePicker? picker = bindable as SfDateTimePicker;
            if (picker == null)
            {
                return;
            }

            picker.SelectedTextStyle.TextColor = picker.TextDisplayMode == PickerTextDisplayMode.Default ? picker.SelectedTextColor : picker.SelectionTextColor;
        }

        /// <summary>
        /// Method invokes on the picker selection font size changed.
        /// </summary>
        /// <param name="bindable">The selection text style object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnSelectedFontSizeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfDateTimePicker? picker = bindable as SfDateTimePicker;
            if (picker == null)
            {
                return;
            }

            picker.SelectedTextStyle.FontSize = picker.SelectedFontSize;
        }

        /// <summary>
        /// Method invokes on the picker normal text color changed.
        /// </summary>
        /// <param name="bindable">The text style object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnNormalTextColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfDateTimePicker? picker = bindable as SfDateTimePicker;
            if (picker == null)
            {
                return;
            }

            picker.TextStyle.TextColor = picker.NormalTextColor;
        }

        /// <summary>
        /// Method invokes on the picker normal font size changed.
        /// </summary>
        /// <param name="bindable">The text style object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnNormalFontSizeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfDateTimePicker? picker = bindable as SfDateTimePicker;
            if (picker == null)
            {
                return;
            }

            picker.TextStyle.FontSize = picker.NormalFontSize;
        }

        /// <summary>
        /// Method invokes on the picker disabled text color changed.
        /// </summary>
        /// <param name="bindable">The text style object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnDisabledTextColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfDateTimePicker? picker = bindable as SfDateTimePicker;
            if (picker == null)
            {
                return;
            }

            picker.DisabledTextStyle.TextColor = picker.DisabledTextColor;
        }

        #endregion

        #region Interface Implementation

        /// <summary>
        /// This method is declared only in IParentThemeElement
        /// and you need to implement this method only in main control.
        /// </summary>
        /// <returns>ResourceDictionary</returns>
        ResourceDictionary IParentThemeElement.GetThemeDictionary()
        {
            return new SfDateTimePickerStyles();
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

        /// <summary>
        /// This method will be called when users merge a theme dictionary
        /// that contains value for “SyncfusionTheme” dynamic resource key.
        /// </summary>
        /// <param name="oldTheme">Old theme.</param>
        /// <param name="newTheme">New theme.</param>
        void IThemeElement.OnControlThemeChanged(string oldTheme, string newTheme)
        {
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs after the selected date is changed on SfDateTimePicker.
        /// </summary>
        public event EventHandler<DateTimePickerSelectionChangedEventArgs>? SelectionChanged;

        #endregion
    }
}