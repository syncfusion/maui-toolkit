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
        /// Holds the picker column collection.
        /// </summary>
        ObservableCollection<PickerColumn> _columns;

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
        /// Identifies the <see cref="MilliSecondInterval"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="MilliSecondInterval"/> dependency property.
        /// </value>
        public static readonly BindableProperty MilliSecondIntervalProperty =
            BindableProperty.Create(
                nameof(MilliSecondInterval),
                typeof(int),
                typeof(SfDateTimePicker),
                1,
                propertyChanged: OnMilliSecondIntervalPropertyChanged);

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
        
        /// <summary>
        /// Identifies the <see cref="ActiveViewProperty"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="ActiveViewProperty"/> dependency property.
        /// </value>
        public static readonly BindableProperty ActiveViewProperty =
            BindableProperty.Create(
                nameof(ActiveView),
                typeof(DateTimePickerView),
                typeof(SfDateTimePicker),
                DateTimePickerView.Date, 
                propertyChanged: OnActiveViewPropertyChanged);

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

        /// <summary>
        /// Identifies the <see cref="NormalDayColumnTextColor"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="NormalDayColumnTextColor"/> dependency property.
        /// </value>
        internal static readonly BindableProperty NormalDayColumnTextColorProperty =
            BindableProperty.Create(
                nameof(NormalDayColumnTextColor),
                typeof(Color),
                typeof(SfDateTimePicker),
                defaultValueCreator: bindable => Color.FromArgb("#1C1B1F"),
                propertyChanged: OnNormalDayColumnTextColorChanged);

        /// <summary>
        /// Identifies the <see cref="NormalMonthColumnTextColor"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="NormalMonthColumnTextColor"/> dependency property.
        /// </value>
        internal static readonly BindableProperty NormalMonthColumnTextColorProperty =
            BindableProperty.Create(
                nameof(NormalMonthColumnTextColor),
                typeof(Color),
                typeof(SfDateTimePicker),
                defaultValueCreator: bindable => Color.FromArgb("#1C1B1F"),
                propertyChanged: OnNormalMonthColumnTextColorChanged);

        /// <summary>
        /// Identifies the <see cref="NormalYearColumnTextColor"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="NormalYearColumnTextColor"/> dependency property.
        /// </value>
        internal static readonly BindableProperty NormalYearColumnTextColorProperty =
            BindableProperty.Create(
                nameof(NormalYearColumnTextColor),
                typeof(Color),
                typeof(SfDateTimePicker),
                defaultValueCreator: bindable => Color.FromArgb("#1C1B1F"),
                propertyChanged: OnNormalYearColumnTextColorChanged);

        /// <summary>
        /// Identifies the <see cref="NormalDayColumnFontSize"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="NormalDayColumnFontSize"/> dependency property.
        /// </value>
        internal static readonly BindableProperty NormalDayColumnFontSizeProperty =
            BindableProperty.Create(
                nameof(NormalDayColumnFontSize),
                typeof(double),
                typeof(SfDateTimePicker),
                defaultValueCreator: bindable => 16d,
                propertyChanged: OnNormalDayColumnFontSizeChanged);

        /// <summary>
        /// Identifies the <see cref="NormalMonthColumnFontSize"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="NormalMonthColumnFontSize"/> dependenc
        /// ///  property.
        /// </value>
        internal static readonly BindableProperty NormalMonthColumnFontSizeProperty =
            BindableProperty.Create(
                nameof(NormalMonthColumnFontSize),
                typeof(double),
                typeof(SfDateTimePicker),
                defaultValueCreator: bindable => 16d,
                propertyChanged: OnNormalMonthColumnFontSizeChanged);

        /// <summary>
        /// Identifies the <see cref="NormalYearColumnFontSize"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="NormalYearColumnFontSize"/> dependency property.
        /// </value>
        internal static readonly BindableProperty NormalYearColumnFontSizeProperty =
            BindableProperty.Create(
                nameof(NormalYearColumnFontSize),
                typeof(double),
                typeof(SfDateTimePicker),
                defaultValueCreator: bindable => 16d,
                propertyChanged: OnNormalYearColumnFontSizeChanged);

        /// <summary>
        /// Identifies the <see cref="NormalHourColumnTextColor"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="NormalHourColumnTextColor"/> dependency property.
        /// </value>
        internal static readonly BindableProperty NormalHourColumnTextColorProperty =
            BindableProperty.Create(
                nameof(NormalHourColumnTextColor),
                typeof(Color),
                typeof(SfDateTimePicker),
                defaultValueCreator: bindable => Color.FromArgb("#1C1B1F"),
                propertyChanged: OnNormalHourColumnTextColorChanged);

        /// <summary>
        /// Identifies the <see cref="NormalMinuteColumnTextColor"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="NormalMinuteColumnTextColor"/> dependency property.
        /// </value>
        internal static readonly BindableProperty NormalMinuteColumnTextColorProperty =
            BindableProperty.Create(
                nameof(NormalMinuteColumnTextColor),
                typeof(Color),
                typeof(SfDateTimePicker),
                defaultValueCreator: bindable => Color.FromArgb("#1C1B1F"),
                propertyChanged: OnNormalMinuteColumnTextColorChanged);

        /// <summary>
        /// Identifies the <see cref="NormalSecondColumnTextColor"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="NormalSecondColumnTextColor"/> dependency property.
        /// </value>
        internal static readonly BindableProperty NormalSecondColumnTextColorProperty =
            BindableProperty.Create(
                nameof(NormalSecondColumnTextColor),
                typeof(Color),
                typeof(SfDateTimePicker),
                defaultValueCreator: bindable => Color.FromArgb("#1C1B1F"),
                propertyChanged: OnNormalSecondColumnTextColorChanged);

        /// <summary>
        /// Identifies the <see cref="NormalMeridiemColumnTextColor"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="NormalMeridiemColumnTextColor"/> dependency property.
        /// </value>
        internal static readonly BindableProperty NormalMeridiemColumnTextColorProperty =
            BindableProperty.Create(
                nameof(NormalMeridiemColumnTextColor),
                typeof(Color),
                typeof(SfDateTimePicker),
                defaultValueCreator: bindable => Color.FromArgb("#1C1B1F"),
                propertyChanged: OnNormalMeridiemColumnTextColorChanged);

        /// <summary>
        /// Identifies the <see cref="NormalMilliSecondColumnTextColor"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="NormalMilliSecondColumnTextColor"/> dependency property.
        /// </value>
        internal static readonly BindableProperty NormalMilliSecondColumnTextColorProperty =
            BindableProperty.Create(
                nameof(NormalMilliSecondColumnTextColor),
                typeof(Color),
                typeof(SfDateTimePicker),
                defaultValueCreator: bindable => Color.FromArgb("#1C1B1F"),
                propertyChanged: OnNormalMilliSecondColumnTextColorChanged);

        /// <summary>
        /// Identifies the <see cref="NormalHourColumnFontSize"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="NormalHourColumnFontSize"/> dependency property.
        /// </value>
        internal static readonly BindableProperty NormalHourColumnFontSizeProperty =
            BindableProperty.Create(
                nameof(NormalHourColumnFontSize),
                typeof(double),
                typeof(SfDateTimePicker),
                defaultValueCreator: bindable => 16d,
                propertyChanged: OnNormalHourColumnFontSizeChanged);

        /// <summary>
        /// Identifies the <see cref="NormalMinuteColumnFontSize"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="NormalMinuteColumnFontSize"/> dependency property.
        /// </value>
        internal static readonly BindableProperty NormalMinuteColumnFontSizeProperty =
            BindableProperty.Create(
                nameof(NormalMinuteColumnFontSize),
                typeof(double),
                typeof(SfDateTimePicker),
                defaultValueCreator: bindable => 16d,
                propertyChanged: OnNormalMinuteColumnFontSizeChanged);

        /// <summary>
        /// Identifies the <see cref="NormalSecondColumnFontSize"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="NormalSecondColumnFontSize"/> dependency property.
        /// </value>
        internal static readonly BindableProperty NormalSecondColumnFontSizeProperty =
            BindableProperty.Create(
                nameof(NormalSecondColumnFontSize),
                typeof(double),
                typeof(SfDateTimePicker),
                defaultValueCreator: bindable => 16d,
                propertyChanged: OnNormalSecondColumnFontSizeChanged);

        /// <summary>
        /// Identifies the <see cref="NormalMeridiemColumnFontSize"/> dependency property.
        /// </summary>
        /// <value>
        /// /// The identifier for <see cref="NormalMeridiemColumnFontSize"/> dependency property.
        /// </value>
        internal static readonly BindableProperty NormalMeridiemColumnFontSizeProperty =
            BindableProperty.Create(
                nameof(NormalMeridiemColumnFontSize),
                typeof(double),
                typeof(SfDateTimePicker),
                defaultValueCreator: bindable => 16d,
                propertyChanged: OnNormalMeridiemColumnFontSizeChanged);

        /// <summary>
        /// Identifies the <see cref="NormalMilliSecondColumnFontSize"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="NormalMilliSecondColumnFontSize"/> dependency property.
        /// </value>
        internal static readonly BindableProperty NormalMilliSecondColumnFontSizeProperty =
            BindableProperty.Create(
                nameof(NormalMilliSecondColumnFontSize),
                typeof(double),
                typeof(SfDateTimePicker),
                defaultValueCreator: bindable => 16d,
                propertyChanged: OnNormalMilliSecondColumnFontSizeChanged);


        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SfDateTimePicker"/> class.
        /// </summary>
        public SfDateTimePicker()
        {
            DayColumn = new PickerColumn();
            MonthColumn = new PickerColumn();
            YearColumn = new PickerColumn();
            HourColumn = new PickerColumn();
            MinuteColumn = new PickerColumn();
            SecondColumn = new PickerColumn();
            MeridiemColumn = new PickerColumn();
            MillisecondColumn = new PickerColumn();
            _columns = new ObservableCollection<PickerColumn>();
            SelectedIndex = 0;
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
        /// Gets or sets the millisecond interval in SfDateTimePicker.
        /// </summary>
        /// <value>The default value of <see cref="SfDateTimePicker.MilliSecondInterval"/> is 1.</value>
        /// <example>
        /// The following examples demonstrate how to set the millisecond interval in SfDateTimePicker.
        /// # [XAML](#tab/tabid-13)
        /// <code language="xaml">
        /// <![CDATA[
        /// <Picker:SfDateTimePicker x:Name="DateTimePicker"
        ///                      MilliSecondInterval="2" />
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-14)
        /// <code language="C#">
        /// SfDateTimePicker dateTimePicker = new SfDateTimePicker();
        /// dateTimePicker.MilliSecondInterval = 2;
        /// </code>
        /// </example>
        public int MilliSecondInterval
        {
            get { return (int)GetValue(MilliSecondIntervalProperty); }
            set { SetValue(MilliSecondIntervalProperty, value); }
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

        /// <summary>
        /// Gets or sets the currently active tab selection (Date or Time) in the DateTimePicker.
        /// </summary>
        public DateTimePickerView ActiveView
        {
            get { return (DateTimePickerView)GetValue(ActiveViewProperty); }
            set { SetValue(ActiveViewProperty, value); }
        }

        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets or sets the value to identify the header selection(date or time).
        /// </summary>
        internal int SelectedIndex { get; set; }

        /// <summary>
        /// Gets or sets the day column information.
        /// </summary>
        internal PickerColumn DayColumn { get; set; }

        /// <summary>
        /// Gets or sets the month column information.
        /// </summary>
        internal PickerColumn MonthColumn { get; set; }

        /// <summary>
        /// Gets or sets the year column information.
        /// </summary>
        internal PickerColumn YearColumn { get; set; }

        /// <summary>
        /// Gets or sets the hour column information.
        /// </summary>
        internal PickerColumn HourColumn { get; set; }

        /// <summary>
        /// Gets or sets the minute column information.
        /// </summary>
        internal PickerColumn MinuteColumn { get; set; }

        /// <summary>
        /// Gets or sets the second column information.
        /// </summary>
        internal PickerColumn SecondColumn { get; set; }

        /// <summary>
        /// Gets or sets the meridiem column information.
        /// </summary>
        internal PickerColumn MeridiemColumn { get; set; }

        /// <summary>
        /// Gets or sets the millisecond column information.
        /// </summary>
        internal PickerColumn MillisecondColumn { get; set; }

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

        /// <summary>
        /// Gets or sets the normal day column text color of the text style.
        /// </summary>
        internal Color NormalDayColumnTextColor
        {
            get { return (Color)GetValue(NormalDayColumnTextColorProperty); }
            set { SetValue(NormalDayColumnTextColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the normal month column text color of the text style.
        /// </summary>
        internal Color NormalMonthColumnTextColor
        {
            get { return (Color)GetValue(NormalMonthColumnTextColorProperty); }
            set { SetValue(NormalMonthColumnTextColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the normal year column text color of the text style.
        /// </summary>
        internal Color NormalYearColumnTextColor
        {
            get { return (Color)GetValue(NormalYearColumnTextColorProperty); }
            set { SetValue(NormalYearColumnTextColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the normal day column font size of the text style.
        /// </summary>
        internal double NormalDayColumnFontSize
        {
            get { return (double)GetValue(NormalDayColumnFontSizeProperty); }
            set { SetValue(NormalDayColumnFontSizeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the normal month column font size of the text style.
        /// </summary>
        internal double NormalMonthColumnFontSize
        {
            get { return (double)GetValue(NormalMonthColumnFontSizeProperty); }
            set { SetValue(NormalMonthColumnFontSizeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the normal year column font size of the text style.
        /// </summary>
        internal double NormalYearColumnFontSize
        {
            get { return (double)GetValue(NormalYearColumnFontSizeProperty); }
            set { SetValue(NormalYearColumnFontSizeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the normal hour column text color of the text style.
        /// </summary>
        internal Color NormalHourColumnTextColor
        {
            get { return (Color)GetValue(NormalHourColumnTextColorProperty); }
            set { SetValue(NormalHourColumnTextColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the normal minute column text color of the text style.
        /// </summary>
        internal Color NormalMinuteColumnTextColor
        {
            get { return (Color)GetValue(NormalMinuteColumnTextColorProperty); }
            set { SetValue(NormalMinuteColumnTextColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the normal second column text color of the text style.
        /// </summary>
        internal Color NormalSecondColumnTextColor
        {
            get { return (Color)GetValue(NormalSecondColumnTextColorProperty); }
            set { SetValue(NormalSecondColumnTextColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the normal meridiem column text color of the text style.
        /// </summary>
        internal Color NormalMeridiemColumnTextColor
        {
            get { return (Color)GetValue(NormalMeridiemColumnTextColorProperty); }
            set { SetValue(NormalMeridiemColumnTextColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the normal milli second column text color of the text style.
        /// </summary>
        internal Color NormalMilliSecondColumnTextColor
        {
            get { return (Color)GetValue(NormalMilliSecondColumnTextColorProperty); }
            set { SetValue(NormalMilliSecondColumnTextColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the normal hour column font size of the text style.
        /// </summary>
        internal double NormalHourColumnFontSize
        {
            get { return (double)GetValue(NormalHourColumnFontSizeProperty); }
            set { SetValue(NormalHourColumnFontSizeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the normal minute column font size of the text style.
        /// </summary>
        internal double NormalMinuteColumnFontSize
        {
            get { return (double)GetValue(NormalMinuteColumnFontSizeProperty); }
            set { SetValue(NormalMinuteColumnFontSizeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the normal second column font size of the text style.
        /// </summary>
        internal double NormalSecondColumnFontSize
        {
            get { return (double)GetValue(NormalSecondColumnFontSizeProperty); }
            set { SetValue(NormalSecondColumnFontSizeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the normal meridiem column font size of the text style.
        /// </summary>
        internal double NormalMeridiemColumnFontSize
        {
            get { return (double)GetValue(NormalMeridiemColumnFontSizeProperty); }
            set { SetValue(NormalMeridiemColumnFontSizeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the normal milli second column font size of the text style.
        /// </summary>
        internal double NormalMilliSecondColumnFontSize
        {
            get { return (double)GetValue(NormalMilliSecondColumnFontSizeProperty); }
            set { SetValue(NormalMilliSecondColumnFontSizeProperty, value); }
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Method to reset the picker date columns.
        /// </summary>
        internal void ResetDateColumns()
        {
            DayColumn = new PickerColumn();
            MonthColumn = new PickerColumn();
            YearColumn = new PickerColumn();
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
                selectedTimeNullable = SelectedDate;
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
            if (SelectedIndex == 0)
            {
                OnDatePickerSelectionIndexChanged(e);
            }
            else
            {
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
                        if (DayColumn.ItemsSource != null && DayColumn.ItemsSource is ObservableCollection<string> dayCollection && dayCollection.Count > e.NewValue)
                        {
                            //// Get the day value based on the selected index changes value.
                            day = dayCollection[e.NewValue].Length <= 2 ? int.Parse(dayCollection[e.NewValue]) : int.Parse(dayCollection[e.NewValue].Substring(dayCollection[e.NewValue].Length - 2));
                        }

                        selectedDate = new DateTime(previousSelectedDate.Year, previousSelectedDate.Month, day, previousSelectedDate.Hour, previousSelectedDate.Minute, previousSelectedDate.Second, previousSelectedDate.Millisecond);
                    }

                    break;
                //// Need to handle the month selection changes.
                case 1:
                    {
                        int month = 1;
                        if (MonthColumn.ItemsSource != null && MonthColumn.ItemsSource is ObservableCollection<string> monthCollection && monthCollection.Count > e.NewValue)
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
                            else if (monthFormat == "MMMM")
                            {
                                List<string> monthStrings = DateTimeFormatInfo.CurrentInfo.MonthNames.ToList();
                                //// Get the month value based on the selected index changes value.
                                month = monthStrings.IndexOf(monthCollection[e.NewValue]) + 1;
                            }
                        }

                        ObservableCollection<string> days = DatePickerHelper.GetDays(dayFormat, month, previousSelectedDate.Year, MinimumDate, maxDate, DayInterval);
                        ObservableCollection<string> previousDays = DayColumn.ItemsSource is ObservableCollection<string> previousDayCollection ? previousDayCollection : new ObservableCollection<string>();
                        //// Check the month selection changes needed to update the days collection.
                        if (!PickerHelper.IsCollectionEquals(days, previousDays))
                        {
                            DayColumn.ItemsSource = days;
                        }

                        //// Check the new days collection have a selected day value, if not then update the nearby value.
                        int index = DatePickerHelper.GetDayIndex(dayFormat, days, previousSelectedDate.Day, DayInterval);
                        int day = index == -1 ? 1 : days[index].Length <= 2 ? int.Parse(days[index]) : int.Parse(days[index].Substring(days[index].Length - 2));

                        selectedDate = new DateTime(previousSelectedDate.Year, month, day, previousSelectedDate.Hour, previousSelectedDate.Minute, previousSelectedDate.Second, previousSelectedDate.Millisecond);
                    }

                    break;
                //// Need to handle the year selection changes.
                case 2:
                    {
                        int year = MinimumDate.Year;
                        if (YearColumn.ItemsSource != null && YearColumn.ItemsSource is ObservableCollection<string> yearCollection && yearCollection.Count > e.NewValue)
                        {
                            //// Get the year value based on the selected index changes value.
                            year = int.Parse(yearCollection[e.NewValue]);
                        }

                        ObservableCollection<string> months = DatePickerHelper.GetMonths(monthFormat, year, MinimumDate, maxDate, MonthInterval);
                        ObservableCollection<string> previousMonths = MonthColumn.ItemsSource is ObservableCollection<string> previousMonthCollection ? previousMonthCollection : new ObservableCollection<string>();
                        //// Check the year index changes needed to update the month collection.
                        if (!PickerHelper.IsCollectionEquals(months, previousMonths))
                        {
                            MonthColumn.ItemsSource = months;
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
                        else if (monthFormat == "MMMM")
                        {
                            List<string> monthStrings = DateTimeFormatInfo.CurrentInfo.MonthNames.ToList();
                            month = monthStrings.IndexOf(months[monthIndex]) + 1;
                        }

                        ObservableCollection<string> days = DatePickerHelper.GetDays(dayFormat, month, year, MinimumDate, maxDate, DayInterval);
                        ObservableCollection<string> previousDays = DayColumn.ItemsSource is ObservableCollection<string> previousDayCollection ? previousDayCollection : new ObservableCollection<string>();
                        //// Check the year and month(if month items source updated) changes needed to change the day collection.
                        if (!PickerHelper.IsCollectionEquals(days, previousDays))
                        {
                            DayColumn.ItemsSource = days;
                        }

                        //// Check the day collection have selected day value, if not then update the nearby value.
                        int index = DatePickerHelper.GetDayIndex(dayFormat, days, previousSelectedDate.Day, DayInterval);
                        int day = index == -1 ? 1 : days[index].Length <= 2 ? int.Parse(days[index]) : int.Parse(days[index].Substring(days[index].Length - 2));

                        selectedDate = new DateTime(year, month, day, previousSelectedDate.Hour, previousSelectedDate.Minute, previousSelectedDate.Second, previousSelectedDate.Millisecond);
                    }

                    break;
            }

            if (IsScrollSelectionAllowed())
            {
                // Check if the selected date-time falls within any blackout date-times
                // If it does, revert the day column selection to the previous value
                if (BlackoutDateTimes.Any(blackOutDateTime => DatePickerHelper.IsBlackoutDateTime(blackOutDateTime, selectedDate, out bool isTimeSpanAtZero)))
                {
                    DayColumn.SelectedIndex = e.OldValue;
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
                        if (HourColumn.ItemsSource != null && HourColumn.ItemsSource is ObservableCollection<string> hourCollection && hourCollection.Count > e.NewValue)
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
                        ObservableCollection<string> previousMinutes = MinuteColumn.ItemsSource is ObservableCollection<string> previousMinuteCollection ? previousMinuteCollection : new ObservableCollection<string>();
                        if (!PickerHelper.IsCollectionEquals(minutes, previousMinutes))
                        {
                            MinuteColumn.ItemsSource = minutes;
                        }

                        int minuteIndex = TimePickerHelper.GetMinuteOrSecondOrMilliSecondsIndex(minutes, previousSelectedDate.Minute);
                        //// Get the minute value based on the selected index changes value.
                        int minute = int.Parse(minutes[minuteIndex]);

                        ObservableCollection<string> seconds = TimePickerHelper.GetSeconds(SecondInterval, hour, minute, previousSelectedDate, isMinDate ? MinimumDate : null, isMaxDate ? maxDate : null);
                        ObservableCollection<string> previousSeconds = SecondColumn.ItemsSource is ObservableCollection<string> previousSecondCollection ? previousSecondCollection : new ObservableCollection<string>();
                        if (!PickerHelper.IsCollectionEquals(seconds, previousSeconds))
                        {
                            SecondColumn.ItemsSource = seconds;
                        }

                        int secondIndex = TimePickerHelper.GetMinuteOrSecondOrMilliSecondsIndex(seconds, previousSelectedDate.Second);
                        //// Get the second value based on the selected index changes value.
                        int second = int.Parse(seconds[secondIndex]);

                        ObservableCollection<string> milliseconds = TimePickerHelper.GetMilliseconds(MilliSecondInterval, hour, minute, second, previousSelectedDate, isMinDate ? MinimumDate : null, isMaxDate ? maxDate : null);
                        ObservableCollection<string> previousMilliseconds = MillisecondColumn.ItemsSource is ObservableCollection<string> previousMillisecondCollection ? previousMillisecondCollection : new ObservableCollection<string>();
                        if (!PickerHelper.IsCollectionEquals(milliseconds, previousMilliseconds))
                        {
                            MillisecondColumn.ItemsSource = milliseconds;
                        }

                        int millisecondIndex = TimePickerHelper.GetMinuteOrSecondOrMilliSecondsIndex(milliseconds, previousSelectedDate.Millisecond);
                        ////Get the millisecond value based on the selected index changes value.
                        int millisecond = int.Parse(milliseconds[millisecondIndex]);

                        selectedDate = new DateTime(previousSelectedDate.Year, previousSelectedDate.Month, previousSelectedDate.Day, hour, minute, second, millisecond);
                    }

                    break;
                case 1:
                    {
                        int minutes = 0;
                        if (MinuteColumn.ItemsSource != null && MinuteColumn.ItemsSource is ObservableCollection<string> minuteCollection && minuteCollection.Count > e.NewValue)
                        {
                            //// Get the minute value based on the selected index changes value.
                            minutes = int.Parse(minuteCollection[e.NewValue]);
                        }

                        ObservableCollection<string> seconds = TimePickerHelper.GetSeconds(SecondInterval, previousSelectedDate.Hour, minutes, previousSelectedDate, isMinDate ? MinimumDate : null, isMaxDate ? maxDate : null);
                        ObservableCollection<string> previousSeconds = SecondColumn.ItemsSource is ObservableCollection<string> previousSecondCollection ? previousSecondCollection : new ObservableCollection<string>();
                        if (!PickerHelper.IsCollectionEquals(seconds, previousSeconds))
                        {
                            SecondColumn.ItemsSource = seconds;
                        }

                        int secondIndex = TimePickerHelper.GetMinuteOrSecondOrMilliSecondsIndex(seconds, previousSelectedDate.Second);
                        //// Get the second value based on the selected index changes value.
                        int second = int.Parse(seconds[secondIndex]);

                        ObservableCollection<string> milliseconds = TimePickerHelper.GetMilliseconds(MilliSecondInterval, previousSelectedDate.Hour, minutes, second, previousSelectedDate, isMinDate ? MinimumDate : null, isMaxDate ? maxDate : null);
                        ObservableCollection<string> previousMilliseconds = MillisecondColumn.ItemsSource is ObservableCollection<string> previousMillisecondCollection ? previousMillisecondCollection : new ObservableCollection<string>();
                        if (!PickerHelper.IsCollectionEquals(milliseconds, previousMilliseconds))
                        {
                            MillisecondColumn.ItemsSource = milliseconds;
                        }

                        int millisecondIndex = TimePickerHelper.GetMinuteOrSecondOrMilliSecondsIndex(milliseconds, previousSelectedDate.Millisecond);
                        ////Get the millisecond value based on the selected index changes value.
                        int millisecond = int.Parse(milliseconds[millisecondIndex]);

                        selectedDate = new DateTime(previousSelectedDate.Year, previousSelectedDate.Month, previousSelectedDate.Day, previousSelectedDate.Hour, minutes, second, millisecond);
                    }

                    break;
                case 2:
                    {
                        int seconds = 0;
                        if (SecondColumn.ItemsSource != null && SecondColumn.ItemsSource is ObservableCollection<string> secondCollection && secondCollection.Count > e.NewValue)
                        {
                            //// Get the seconds value based on the selected index changes value.
                            seconds = int.Parse(secondCollection[e.NewValue]);
                        }

                        ObservableCollection<string> milliseconds = TimePickerHelper.GetMilliseconds(MilliSecondInterval, previousSelectedDate.Hour, previousSelectedDate.Minute, seconds, previousSelectedDate, isMinDate ? MinimumDate : null, isMaxDate ? maxDate : null);
                        ObservableCollection<string> previousMilliseconds = MillisecondColumn.ItemsSource is ObservableCollection<string> previousMillisecondCollection ? previousMillisecondCollection : new ObservableCollection<string>();
                        if (!PickerHelper.IsCollectionEquals(milliseconds, previousMilliseconds))
                        {
                            MillisecondColumn.ItemsSource = milliseconds;
                        }

                        int millisecondIndex = TimePickerHelper.GetMinuteOrSecondOrMilliSecondsIndex(milliseconds, previousSelectedDate.Millisecond);
                        ////Get the millisecond value based on the selected index changes value.
                        int millisecond = int.Parse(milliseconds[millisecondIndex]);

                        selectedDate = new DateTime(previousSelectedDate.Year, previousSelectedDate.Month, previousSelectedDate.Day, previousSelectedDate.Hour, previousSelectedDate.Minute, seconds, millisecond);
                    }

                    break;
                case 3:
                    {
                        ObservableCollection<string> meridiemCollection = new ObservableCollection<string>();
                        if (MeridiemColumn.ItemsSource != null && MeridiemColumn.ItemsSource is ObservableCollection<string> meridiems)
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
                        ObservableCollection<string> previousHour = HourColumn.ItemsSource is ObservableCollection<string> previousHourCollection ? previousHourCollection : new ObservableCollection<string>();
                        if (!PickerHelper.IsCollectionEquals(hours, previousHour))
                        {
                            HourColumn.ItemsSource = hours;
                        }

                        int? hourIndex = TimePickerHelper.GetHourIndex(hourFormat, hours, previousSelectedDate.Hour);
                        if (hourIndex.HasValue)
                        {
                            int hour = int.Parse(hours[hourIndex.Value]);
                            hour = (hour % 12) + neededHour;

                            ObservableCollection<string> minutes = TimePickerHelper.GetMinutes(MinuteInterval, hour, previousSelectedDate, isMinDate ? MinimumDate : null, isMaxDate ? maxDate : null);
                            ObservableCollection<string> previousMinutes = MinuteColumn.ItemsSource is ObservableCollection<string> previousMinuteCollection ? previousMinuteCollection : new ObservableCollection<string>();
                            if (!PickerHelper.IsCollectionEquals(minutes, previousMinutes))
                            {
                                MinuteColumn.ItemsSource = minutes;
                            }

                            int minuteIndex = TimePickerHelper.GetMinuteOrSecondOrMilliSecondsIndex(minutes, previousSelectedDate.Minute);
                            //// Get the minute value based on the selected index changes value.
                            int minute = int.Parse(minutes[minuteIndex]);

                            ObservableCollection<string> seconds = TimePickerHelper.GetSeconds(SecondInterval, hour, minute, previousSelectedDate, isMinDate ? MinimumDate : null, isMaxDate ? maxDate : null);
                            ObservableCollection<string> previousSeconds = SecondColumn.ItemsSource is ObservableCollection<string> previousSecondCollection ? previousSecondCollection : new ObservableCollection<string>();
                            if (!PickerHelper.IsCollectionEquals(seconds, previousSeconds))
                            {
                                SecondColumn.ItemsSource = seconds;
                            }

                            int secondIndex = TimePickerHelper.GetMinuteOrSecondOrMilliSecondsIndex(seconds, previousSelectedDate.Second);
                            //// Get the second value based on the selected index changes value.
                            int second = int.Parse(seconds[secondIndex]);

                            ObservableCollection<string> milliseconds = TimePickerHelper.GetMilliseconds(MilliSecondInterval, hour, minute, second, previousSelectedDate, isMinDate ? MinimumDate : null, isMaxDate ? maxDate : null);
                            ObservableCollection<string> previousMilliseconds = MillisecondColumn.ItemsSource is ObservableCollection<string> previousMillisecondCollection ? previousMillisecondCollection : new ObservableCollection<string>();
                            if (!PickerHelper.IsCollectionEquals(milliseconds, previousMilliseconds))
                            {
                                MillisecondColumn.ItemsSource = milliseconds;
                            }

                            int millisecondIndex = TimePickerHelper.GetMinuteOrSecondOrMilliSecondsIndex(milliseconds, previousSelectedDate.Millisecond);
                            //// Get the millisecond value based on the selected index changes value.
                            int millisecond = int.Parse(milliseconds[millisecondIndex]);

                            selectedDate = new DateTime(previousSelectedDate.Year, previousSelectedDate.Month, previousSelectedDate.Day, hour, minute, second, millisecond);
                        }
                    }

                    break;
                case 4:
                    {
                        int millisecond = 0;
                        if (MillisecondColumn.ItemsSource != null && MillisecondColumn.ItemsSource is ObservableCollection<string> millisecondCollection && millisecondCollection.Count > e.NewValue)
                        {
                            ////Get the millisecond value based on the selected index change value.
                            millisecond = int.Parse(millisecondCollection[e.NewValue]);
                        }

                        selectedDate = new DateTime(previousSelectedDate.Year, previousSelectedDate.Month, previousSelectedDate.Day, previousSelectedDate.Hour, previousSelectedDate.Minute, previousSelectedDate.Second, millisecond);
                    }

                    break;
            }

            if (IsScrollSelectionAllowed())
            {
                // Check if the selected date and time falls within any blackout date-times
                // If it does, revert the minute column selection to the previous value
                if (BlackoutDateTimes.Any(blackOutDateTime => DatePickerHelper.IsBlackoutDateTime(blackOutDateTime, selectedDate, out bool isTimeSpanAtZero)))
                {
                    MinuteColumn.SelectedIndex = e.OldValue;
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
            if (SelectedIndex == 0)
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
            if (SelectedIndex == 0)
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
                YearColumn = GenerateYearColumn(validSelectedDate);
                YearColumn.Parent = this;
                _columns[yearIndex] = YearColumn;
            }

            if (validSelectedDate.Year == MinimumDate.Year || validSelectedDate.Year == maxDate.Year)
            {
                ObservableCollection<string> month = DatePickerHelper.GetMonths(monthFormat, validSelectedDate.Year, MinimumDate, maxDate, MonthInterval);
                ObservableCollection<string> previousMonths = MonthColumn.ItemsSource is ObservableCollection<string> previousMonthCollection ? previousMonthCollection : new ObservableCollection<string>();
                //// Check the year index changes needed to update the month collection.
                if (!PickerHelper.IsCollectionEquals(month, previousMonths))
                {
                    MonthColumn = new PickerColumn()
                    {
                        ItemsSource = month,
                        SelectedIndex = DatePickerHelper.GetMonthIndex(monthFormat, month, validSelectedDate.Month),
                        HeaderText = SfPickerResources.GetLocalizedString(ColumnHeaderView.MonthHeaderText),
                    };
                    int monthIndex = formatString.IndexOf(1);
                    if (monthIndex != -1)
                    {
                        _columns[monthIndex] = MonthColumn;
                    }
                }
            }

            if (!string.IsNullOrEmpty(dayFormat) && ((validSelectedDate.Year == MinimumDate.Year && validSelectedDate.Month == MinimumDate.Month) || (validSelectedDate.Year == maxDate.Year && validSelectedDate.Month == maxDate.Month)))
            {
                ObservableCollection<string> days = DatePickerHelper.GetDays(dayFormat, validSelectedDate.Month, validSelectedDate.Year, MinimumDate, maxDate, DayInterval);
                ObservableCollection<string> previousDays = DayColumn.ItemsSource is ObservableCollection<string> previousDayCollection ? previousDayCollection : new ObservableCollection<string>();
                //// Check the year and month(if month items source updated) changes needed to change the day collection.
                if (!PickerHelper.IsCollectionEquals(days, previousDays))
                {
                    DayColumn = new PickerColumn()
                    {
                        ItemsSource = days,
                        SelectedIndex = DatePickerHelper.GetDayIndex(dayFormat, days, validSelectedDate.Day, DayInterval),
                        HeaderText = SfPickerResources.GetLocalizedString(ColumnHeaderView.DayHeaderText),
                    };
                    int dayIndex = formatString.IndexOf(0);
                    if (dayIndex != -1)
                    {
                        _columns[dayIndex] = DayColumn;
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
                HourColumn = GenerateHourColumn(hourFormat, selectedTime, validSelectedDate);
                int hourIndex = index;
                HourColumn.Parent = this;
                _columns[hourIndex] = HourColumn;
            }

            index = formatStringOrder.IndexOf(1);
            if (index != -1 && (validSelectedDate.Hour == oldDate.Hour || validSelectedDate.Hour == newDate.Hour))
            {
                ObservableCollection<string> minutes = TimePickerHelper.GetMinutes(MinuteInterval, validSelectedDate.Hour, validSelectedDate, MinimumDate, maxDate);
                ObservableCollection<string> previousMinutes = MinuteColumn.ItemsSource is ObservableCollection<string> previousMinuteCollection ? previousMinuteCollection : new ObservableCollection<string>();
                if (!PickerHelper.IsCollectionEquals(minutes, previousMinutes))
                {
                    MinuteColumn = new PickerColumn()
                    {
                        ItemsSource = minutes,
                        SelectedIndex = TimePickerHelper.GetMinuteOrSecondOrMilliSecondsIndex(minutes, validSelectedDate.Minute),
                        HeaderText = SfPickerResources.GetLocalizedString(ColumnHeaderView.MinuteHeaderText),
                    };
                    _columns[index] = MinuteColumn;
                }
            }

            index = formatStringOrder.IndexOf(2);
            if (index != -1 && ((validSelectedDate.Hour == oldDate.Hour && validSelectedDate.Minute == oldDate.Minute) || (validSelectedDate.Hour == newDate.Hour && validSelectedDate.Minute == newDate.Minute)))
            {
                ObservableCollection<string> seconds = TimePickerHelper.GetSeconds(SecondInterval, validSelectedDate.Hour, validSelectedDate.Minute, validSelectedDate, MinimumDate, maxDate);
                ObservableCollection<string> previousSeconds = SecondColumn.ItemsSource is ObservableCollection<string> previousSecondCollection ? previousSecondCollection : new ObservableCollection<string>();
                if (!PickerHelper.IsCollectionEquals(seconds, previousSeconds))
                {
                    SecondColumn = new PickerColumn()
                    {
                        ItemsSource = seconds,
                        SelectedIndex = TimePickerHelper.GetMinuteOrSecondOrMilliSecondsIndex(seconds, validSelectedDate.Second),
                        HeaderText = SfPickerResources.GetLocalizedString(ColumnHeaderView.SecondHeaderText),
                    };
                    _columns[index] = SecondColumn;
                }
            }

            index = formatStringOrder.IndexOf(3);
            if (index != -1 && ((int)(validSelectedDate.Hour / 12) == (int)(oldDate.Hour / 12) || (int)(validSelectedDate.Hour / 12) == (int)(newDate.Hour / 12)))
            {
                ObservableCollection<string> meridiems = TimePickerHelper.GetMeridiem(MinimumDate, maxDate, validSelectedDate);
                ObservableCollection<string> previousCollection = MeridiemColumn.ItemsSource is ObservableCollection<string> previousMeridiemCollection ? previousMeridiemCollection : new ObservableCollection<string>();
                if (!PickerHelper.IsCollectionEquals(meridiems, previousCollection))
                {
                    MeridiemColumn = new PickerColumn()
                    {
                        ItemsSource = meridiems,
                        SelectedIndex = validSelectedDate.Hour >= 12 ? meridiems.Count > 1 ? 1 : 0 : 0,
                        HeaderText = SfPickerResources.GetLocalizedString(ColumnHeaderView.MeridiemHeaderText),
                    };
                    _columns[index] = MeridiemColumn;
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
            if (YearColumn.ItemsSource != null && YearColumn.ItemsSource is ObservableCollection<string> yearCollection && yearCollection.Count > 0)
            {
                int index = DatePickerHelper.GetYearIndex(yearCollection, date.Value.Year);
                if (YearColumn.SelectedIndex != index)
                {
                    YearColumn.SelectedIndex = index;
                }
            }

            if (MonthColumn.ItemsSource != null && MonthColumn.ItemsSource is ObservableCollection<string> monthCollection && !string.IsNullOrEmpty(monthFormat))
            {
                int index = DatePickerHelper.GetMonthIndex(monthFormat, monthCollection, date.Value.Month);
                if (MonthColumn.SelectedIndex != index)
                {
                    MonthColumn.SelectedIndex = index;
                }
            }

            if (DayColumn.ItemsSource != null && DayColumn.ItemsSource is ObservableCollection<string> dayCollection && !string.IsNullOrEmpty(dayFormat))
            {
                int index = DatePickerHelper.GetDayIndex(dayFormat, dayCollection, date.Value.Day, DayInterval);
                if (DayColumn.SelectedIndex != index)
                {
                    DayColumn.SelectedIndex = index;
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
            if (HourColumn.ItemsSource != null && HourColumn.ItemsSource is ObservableCollection<string> hourCollection && hourCollection.Count > 0)
            {
                int? index = TimePickerHelper.GetHourIndex(hourFormat, hourCollection, date.Value.Hour);
                if (index.HasValue && HourColumn.SelectedIndex != index)
                {
                    HourColumn.SelectedIndex = index.Value;
                }
            }

            if (MinuteColumn.ItemsSource != null && MinuteColumn.ItemsSource is ObservableCollection<string> minuteCollection && minuteCollection.Count > 0)
            {
                int index = TimePickerHelper.GetMinuteOrSecondOrMilliSecondsIndex(minuteCollection, date.Value.Minute);
                if (MinuteColumn.SelectedIndex != index)
                {
                    MinuteColumn.SelectedIndex = index;
                }
            }

            if (SecondColumn.ItemsSource != null && SecondColumn.ItemsSource is ObservableCollection<string> secondCollection && secondCollection.Count > 0)
            {
                int index = TimePickerHelper.GetMinuteOrSecondOrMilliSecondsIndex(secondCollection, date.Value.Second);
                if (SecondColumn.SelectedIndex != index)
                {
                    SecondColumn.SelectedIndex = index;
                }
            }

            if (MeridiemColumn.ItemsSource != null && MeridiemColumn.ItemsSource is ObservableCollection<string> meridiemCollection && meridiemCollection.Count > 0)
            {
                int index = date.Value.Hour >= 12 ? 1 : 0;
                if (MeridiemColumn.SelectedIndex != index)
                {
                    MeridiemColumn.SelectedIndex = index;
                }
            }

            if (MillisecondColumn.ItemsSource != null && MillisecondColumn.ItemsSource is ObservableCollection<string> millisecondCollection && millisecondCollection.Count > 0)
            {
                int index = TimePickerHelper.GetMinuteOrSecondOrMilliSecondsIndex(millisecondCollection, date.Value.Millisecond);
                if (MillisecondColumn.SelectedIndex != index)
                {
                    MillisecondColumn.SelectedIndex = index;
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
                DayColumn.HeaderText = SfPickerResources.GetLocalizedString(ColumnHeaderView.DayHeaderText);
            }
            else if (e.PropertyName == nameof(DateTimePickerColumnHeaderView.MonthHeaderText))
            {
                MonthColumn.HeaderText = SfPickerResources.GetLocalizedString(ColumnHeaderView.MonthHeaderText);
            }
            else if (e.PropertyName == nameof(DateTimePickerColumnHeaderView.YearHeaderText))
            {
                YearColumn.HeaderText = SfPickerResources.GetLocalizedString(ColumnHeaderView.YearHeaderText);
            }
            else if (e.PropertyName == nameof(DateTimePickerColumnHeaderView.HourHeaderText))
            {
                HourColumn.HeaderText = SfPickerResources.GetLocalizedString(ColumnHeaderView.HourHeaderText);
            }
            else if (e.PropertyName == nameof(DateTimePickerColumnHeaderView.MinuteHeaderText))
            {
                MinuteColumn.HeaderText = SfPickerResources.GetLocalizedString(ColumnHeaderView.MinuteHeaderText);
            }
            else if (e.PropertyName == nameof(DateTimePickerColumnHeaderView.SecondHeaderText))
            {
                SecondColumn.HeaderText = SfPickerResources.GetLocalizedString(ColumnHeaderView.SecondHeaderText);
            }
            else if (e.PropertyName == nameof(DateTimePickerColumnHeaderView.MeridiemHeaderText))
            {
                MeridiemColumn.HeaderText = SfPickerResources.GetLocalizedString(ColumnHeaderView.MeridiemHeaderText);
            }
            else if (e.PropertyName == nameof(DateTimePickerColumnHeaderView.MilliSecondHeaderText))
            {
                MillisecondColumn.HeaderText = SfPickerResources.GetLocalizedString(ColumnHeaderView.MilliSecondHeaderText);
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
            if (SelectedIndex == 0)
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
			date = GetScrollSelectedDateTime(date);
            DateTime selectedDate = DatePickerHelper.GetValidDateTime(date, MinimumDate, maxDate);
            ObservableCollection<PickerColumn> pickerColumns = new ObservableCollection<PickerColumn>();
            foreach (int index in formatStringOrder)
            {
                switch (index)
                {
                    case 0:
                        DayColumn = GenerateDayColumn(dayFormat, selectedDate);
                        if (Mode == PickerMode.Default)
                        {
                            DayColumn.SelectedItem = SelectedDate != null ? PickerHelper.GetSelectedItemDefaultValue(DayColumn) : null;
                        }
                        else
                        {
                            DayColumn.SelectedItem = SelectedDate == null && _internalSelectedDateTime == null ? null : PickerHelper.GetSelectedItemDefaultValue(DayColumn);
                        }

                        DayColumn.Width = DayColumnWidth;
                        pickerColumns.Add(DayColumn);
                        break;
                    case 1:
                        MonthColumn = GenerateMonthColumn(monthFormat, selectedDate);
                        if (Mode == PickerMode.Default)
                        {
                            MonthColumn.SelectedItem = SelectedDate != null ? PickerHelper.GetSelectedItemDefaultValue(MonthColumn) : null;
                        }
                        else
                        {
                            MonthColumn.SelectedItem = SelectedDate == null && _internalSelectedDateTime == null ? null : PickerHelper.GetSelectedItemDefaultValue(MonthColumn);
                        }

                        MonthColumn.Width = MonthColumnWidth;
                        pickerColumns.Add(MonthColumn);
                        break;
                    case 2:
                        YearColumn = GenerateYearColumn(selectedDate);
                        if (Mode == PickerMode.Default)
                        {
                            YearColumn.SelectedItem = SelectedDate != null ? PickerHelper.GetSelectedItemDefaultValue(YearColumn) : null;
                        }
                        else
                        {
                            YearColumn.SelectedItem = SelectedDate == null && _internalSelectedDateTime == null ? null : PickerHelper.GetSelectedItemDefaultValue(YearColumn);
                        }

                        YearColumn.Width = YearColumnWidth;
                        pickerColumns.Add(YearColumn);
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
            HourColumn = new PickerColumn();
            MinuteColumn = new PickerColumn();
            SecondColumn = new PickerColumn();
            MeridiemColumn = new PickerColumn();
            MillisecondColumn = new PickerColumn();
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
            YearColumn.SelectedItem = PickerHelper.GetSelectedItemDefaultValue(YearColumn);
            MonthColumn.SelectedItem = PickerHelper.GetSelectedItemDefaultValue(MonthColumn);
            DayColumn.SelectedItem = PickerHelper.GetSelectedItemDefaultValue(DayColumn);
            HourColumn.SelectedItem = PickerHelper.GetSelectedItemDefaultValue(HourColumn);
            MinuteColumn.SelectedItem = PickerHelper.GetSelectedItemDefaultValue(MinuteColumn);
            SecondColumn.SelectedItem = PickerHelper.GetSelectedItemDefaultValue(SecondColumn);
            MeridiemColumn.SelectedItem = PickerHelper.GetSelectedItemDefaultValue(MeridiemColumn);
            MillisecondColumn.SelectedItem = PickerHelper.GetSelectedItemDefaultValue(MillisecondColumn);
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
            //// Use the selectedDate if provided, otherwise use 'this.previous'
            DateTime referenceDate = selectedDate ?? _previousSelectedDateTime;
            ObservableCollection<string> days = DatePickerHelper.GetDays(format, referenceDate.Month, referenceDate.Year, MinimumDate, maxDate, DayInterval);

            return new PickerColumn()
            {
                ItemsSource = days,
                SelectedIndex = selectedDate != null ? DatePickerHelper.GetDayIndex(format, days, selectedDate.Value.Day, DayInterval) : _previousSelectedDateTime.Day - 1,
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
			date = GetScrollSelectedDateTime(date);
            DateTime selectedDate = DatePickerHelper.GetValidDateTime(date, MinimumDate, maxDate);
            TimeSpan selectedTime = new TimeSpan(0, selectedDate.Hour, selectedDate.Minute, selectedDate.Second, selectedDate.Millisecond);
            ObservableCollection<PickerColumn> pickerColumns = new ObservableCollection<PickerColumn>();
            foreach (int index in formatStringOrder)
            {
                switch (index)
                {
                    case 0:
                        HourColumn = GenerateHourColumn(hourFormat, selectedTime, selectedDate);
                        if (Mode == PickerMode.Default)
                        {
                            HourColumn.SelectedItem = SelectedDate != null ? PickerHelper.GetSelectedItemDefaultValue(HourColumn) : null;
                        }
                        else
                        {
                            HourColumn.SelectedItem = SelectedDate == null && _internalSelectedDateTime == null ? null : PickerHelper.GetSelectedItemDefaultValue(HourColumn);
                        }

                        HourColumn.Width = HourColumnWidth;
                        pickerColumns.Add(HourColumn);
                        break;
                    case 1:
                        MinuteColumn = GenerateMinuteColumn(selectedTime, selectedDate);
                        if (Mode == PickerMode.Default)
                        {
                            MinuteColumn.SelectedItem = SelectedDate != null ? PickerHelper.GetSelectedItemDefaultValue(MinuteColumn) : null;
                        }
                        else
                        {
                            MinuteColumn.SelectedItem = SelectedDate == null && _internalSelectedDateTime == null ? null : PickerHelper.GetSelectedItemDefaultValue(MinuteColumn);
                        }

                        MinuteColumn.Width = MinuteColumnWidth;
                        pickerColumns.Add(MinuteColumn);
                        break;
                    case 2:
                        SecondColumn = GenerateSecondColumn(selectedTime, selectedDate);
                        if (Mode == PickerMode.Default)
                        {
                            SecondColumn.SelectedItem = SelectedDate != null ? PickerHelper.GetSelectedItemDefaultValue(SecondColumn) : null;
                        }
                        else
                        {
                            SecondColumn.SelectedItem = SelectedDate == null && _internalSelectedDateTime == null ? null : PickerHelper.GetSelectedItemDefaultValue(SecondColumn);
                        }

                        SecondColumn.Width = SecondColumnWidth;
                        pickerColumns.Add(SecondColumn);
                        break;
                    case 3:
                        MeridiemColumn = GenerateMeridiemColumn(selectedTime, selectedDate);
                        if (Mode == PickerMode.Default)
                        {
                            MeridiemColumn.SelectedItem = SelectedDate != null ? PickerHelper.GetSelectedItemDefaultValue(MeridiemColumn) : null;
                        }
                        else
                        {
                            MeridiemColumn.SelectedItem = SelectedDate == null && _internalSelectedDateTime == null ? null : PickerHelper.GetSelectedItemDefaultValue(MeridiemColumn);
                        }

                        MeridiemColumn.Width = MeridiemColumnWidth;
                        pickerColumns.Add(MeridiemColumn);
                        break;
                    case 4:
                        MillisecondColumn = GenerateMillisecondColumn(selectedTime, selectedDate);
                        if (Mode == PickerMode.Default)
                        {
                            MillisecondColumn.SelectedItem = SelectedDate != null ? PickerHelper.GetSelectedItemDefaultValue(MillisecondColumn) : null;
                        }
                        else
                        {
                            MillisecondColumn.SelectedItem = SelectedDate == null && _internalSelectedDateTime == null ? null : PickerHelper.GetSelectedItemDefaultValue(MillisecondColumn);
                        }

                        MillisecondColumn.Width = MilliSecondColumnWidth;
                        pickerColumns.Add(MillisecondColumn);
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
            DateTime? date = selectedDate ?? _previousSelectedDateTime;
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
                SelectedIndex = selectedTime != null ? TimePickerHelper.GetMinuteOrSecondOrMilliSecondsIndex(minutes, date.Value.Minute) : _previousSelectedDateTime.Minute,
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
                SelectedIndex = selectedTime != null ? TimePickerHelper.GetMinuteOrSecondOrMilliSecondsIndex(seconds, selectedTime.Value.Seconds) : _previousSelectedDateTime.Second,
                HeaderText = SfPickerResources.GetLocalizedString(ColumnHeaderView.SecondHeaderText),
            };
        }

        /// <summary>
        /// Method to generate the millisecond column with items source and selected index based on format.
        /// </summary>
        /// <param name="selectedTime">The selected time value.</param>
        /// <param name="selectedDate">The valid selected date value.</param>
        /// <returns>Returns millisecond column details.</returns>
        PickerColumn GenerateMillisecondColumn(TimeSpan? selectedTime, DateTime? selectedDate)
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

            ObservableCollection<string> milliseconds = TimePickerHelper.GetMilliseconds(MilliSecondInterval, date.Hour, date.Minute, date.Second, selectedDate, minimumDate, maximumDate);
            return new PickerColumn()
            {
                ItemsSource = milliseconds,
                SelectedIndex = selectedTime != null ? TimePickerHelper.GetMinuteOrSecondOrMilliSecondsIndex(milliseconds, selectedTime.Value.Milliseconds) : _previousSelectedDateTime.Millisecond,
                HeaderText = SfPickerResources.GetLocalizedString(ColumnHeaderView.MilliSecondHeaderText),
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

            SetDynamicResource(NormalDayColumnTextColorProperty, "SfDateTimePickerNormalDayColumnTextColor");
            SetDynamicResource(NormalMonthColumnTextColorProperty, "SfDateTimePickerNormalMonthColumnTextColor");
            SetDynamicResource(NormalYearColumnTextColorProperty, "SfDateTimePickerNormalYearColumnTextColor");
            SetDynamicResource(NormalHourColumnTextColorProperty, "SfDateTimePickerNormalHourColumnTextColor");
            SetDynamicResource(NormalMinuteColumnTextColorProperty, "SfDateTimePickerNormalMinuteColumnTextColor");
            SetDynamicResource(NormalSecondColumnTextColorProperty, "SfDateTimePickerNormalSecondColumnTextColor");
            SetDynamicResource(NormalMeridiemColumnTextColorProperty, "SfDateTimePickerNormalMeridiemColumnTextColor");
            SetDynamicResource(NormalMilliSecondColumnTextColorProperty, "SfDateTimePickerNormalMilliSecondColumnTextColor");

            SetDynamicResource(NormalDayColumnFontSizeProperty, "SfDateTimePickerNormalDayColumnFontSize");
            SetDynamicResource(NormalMonthColumnFontSizeProperty, "SfDateTimePickerNormalMonthColumnFontSize");
            SetDynamicResource(NormalYearColumnFontSizeProperty, "SfDateTimePickerNormalYearColumnFontSize");
            SetDynamicResource(NormalHourColumnFontSizeProperty, "SfDateTimePickerNormalHourColumnFontSize");
            SetDynamicResource(NormalMinuteColumnFontSizeProperty, "SfDateTimePickerNormalMinuteColumnFontSize");
            SetDynamicResource(NormalSecondColumnFontSizeProperty, "SfDateTimePickerNormalSecondColumnFontSize");
            SetDynamicResource(NormalMeridiemColumnFontSizeProperty, "SfDateTimePickerNormalMeridiemColumnFontSize");
            SetDynamicResource(NormalMilliSecondColumnFontSizeProperty, "SfDateTimePickerNormalMilliSecondColumnFontSize");
        }

		/// <summary>
		/// Method to update Selected Date and Time based on confirmation.
		/// </summary>
		/// <param name="shouldUpdateSelection">Denotes whether selected value needs to be updated</param>
		void UpdateInternalValueToSelection(bool shouldUpdateSelection)
		{
			// If the picker is not in Default mode and an internal selected date-time exists
			if (shouldUpdateSelection && _internalSelectedDateTime != null)
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
		}
		/// <summary>
		/// Gets the internally selected date and time when scroll selection is allowed.
		/// </summary>
		/// <returns>
		/// The <see cref="DateTime"/> value if scroll selection is enabled and an internal selection exists; otherwise, <c>null</c>.
		/// </returns>
		DateTime GetScrollSelectedDateTime(DateTime dateTime)
		{
			return IsScrollSelectionAllowed() && _internalSelectedDateTime.HasValue
				? _internalSelectedDateTime.Value
				: dateTime;
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

            PickerHelper.SetColumnTextStyleBinding(DayColumnTextStyle, this);
            PickerHelper.SetColumnTextStyleBinding(MonthColumnTextStyle, this);
            PickerHelper.SetColumnTextStyleBinding(YearColumnTextStyle, this);
            PickerHelper.SetColumnTextStyleBinding(HourColumnTextStyle, this);
            PickerHelper.SetColumnTextStyleBinding(MinuteColumnTextStyle, this);
            PickerHelper.SetColumnTextStyleBinding(SecondColumnTextStyle, this);
            PickerHelper.SetColumnTextStyleBinding(MilliSecondColumnTextStyle, this);
            PickerHelper.SetColumnTextStyleBinding(MeridiemColumnTextStyle, this);
        }

        /// <summary>
        /// Method triggers while the header button clicked.
        /// </summary>
        /// <param name="index">Index of the header button.</param>
        protected override void OnHeaderButtonClicked(int index)
        {
            if (SelectedIndex == index)
            {
                return;
            }

            SelectedIndex = index;
            if (SelectedIndex == 0)
            {
                ResetDateColumns();
                ActiveView = DateTimePickerView.Date;
            }
            else
            {
                ResetTimeColumns();
                ActiveView = DateTimePickerView.Time;
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

            if (SelectedIndex == 0)
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
                            int dayIndex = DatePickerHelper.GetDayIndex(dayFormat, (ObservableCollection<string>)DayColumn.ItemsSource, selectedDate.Day, DayInterval);
                            if (DayColumn.SelectedIndex != dayIndex)
                            {
                                DayColumn.SelectedIndex = dayIndex;
                            }

                            break;
                        case 1:
                            int monthIndex = DatePickerHelper.GetMonthIndex(monthFormat, (ObservableCollection<string>)MonthColumn.ItemsSource, selectedDate.Month);
                            if (MonthColumn.SelectedIndex != monthIndex)
                            {
                                MonthColumn.SelectedIndex = monthIndex;
                            }

                            break;
                        case 2:
                            int yearIndex = DatePickerHelper.GetYearIndex((ObservableCollection<string>)YearColumn.ItemsSource, selectedDate.Year);
                            if (YearColumn.SelectedIndex != yearIndex)
                            {
                                YearColumn.SelectedIndex = yearIndex;
                            }

                            break;
                    }
                }
            }
            else
            {
                SelectedIndex = 0;
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
            if (_internalSelectedDateTime != null)
            {
                _internalSelectedDateTime = null;
                if (SelectedIndex == 0)
                {
                    BaseHeaderView.DateText = GetDateHeaderText();
                }
                else
                {
                    BaseHeaderView.TimeText = GetTimeHeaderText();
                }
            }

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
            if (_internalSelectedDateTime != null && _internalSelectedDateTime != SelectedDate)
            {
                _internalSelectedDateTime = null;
                BaseHeaderView.DateText = GetDateHeaderText();
                BaseHeaderView.TimeText = GetTimeHeaderText();
            }
        }

        /// <summary>
        /// Method triggers when the date time picker ok button clicked.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnOkButtonClicked(EventArgs e)
        {
			UpdateInternalValueToSelection(IsScrollSelectionAllowed());
            InvokeOkButtonClickedEvent(this, e);
            if (AcceptCommand != null && AcceptCommand.CanExecute(e))
            {
                AcceptCommand.Execute(e);
            }

            IsOpen = false;
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
                    if (_internalSelectedDateTime != null)
                    {
                        _internalSelectedDateTime = null;
                    }

                    UpdateSelectedIndex(SelectedDate);
                }
                else
                {
                    // If no date is selected, clear all column selections (date and time parts)
                    DayColumn.SelectedItem = null;
                    MonthColumn.SelectedItem = null;
                    YearColumn.SelectedItem = null;
                    HourColumn.SelectedItem = null;
                    MinuteColumn.SelectedItem = null;
                    SecondColumn.SelectedItem = null;
                    MillisecondColumn.SelectedItem = null;
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

            IsOpen = false;
        }

        /// <summary>
        /// Method to dispose items.
        /// </summary>
        protected override void OnHandlerChanged()
        {
            if (Handler == null)
            {
                if (ColumnHeaderView != null)
                {
                    ColumnHeaderView.PickerPropertyChanged -= OnColumnHeaderPropertyChanged;
                }

                DisposeBaseItems();
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

                picker.DayColumn.HeaderText = SfPickerResources.GetLocalizedString(picker.ColumnHeaderView.DayHeaderText);
                picker.MonthColumn.HeaderText = SfPickerResources.GetLocalizedString(picker.ColumnHeaderView.MonthHeaderText);
                picker.YearColumn.HeaderText = SfPickerResources.GetLocalizedString(picker.ColumnHeaderView.YearHeaderText);
                picker.HourColumn.HeaderText = SfPickerResources.GetLocalizedString(picker.ColumnHeaderView.HourHeaderText);
                picker.MinuteColumn.HeaderText = SfPickerResources.GetLocalizedString(picker.ColumnHeaderView.MinuteHeaderText);
                picker.SecondColumn.HeaderText = SfPickerResources.GetLocalizedString(picker.ColumnHeaderView.SecondHeaderText);
                picker.MeridiemColumn.HeaderText = SfPickerResources.GetLocalizedString(picker.ColumnHeaderView.MeridiemHeaderText);
                picker.MillisecondColumn.HeaderText = SfPickerResources.GetLocalizedString(picker.ColumnHeaderView.MilliSecondHeaderText);
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
                picker.YearColumn.SelectedItem = null;
                picker.MonthColumn.SelectedItem = null;
                picker.DayColumn.SelectedItem = null;
                picker.HourColumn.SelectedItem = null;
                picker.MinuteColumn.SelectedItem = null;
                picker.SecondColumn.SelectedItem = null;
                picker.MeridiemColumn.SelectedItem = null;
                picker.MillisecondColumn.SelectedItem = null;
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
            if (string.IsNullOrEmpty(dayFormat) || picker.SelectedIndex != 0)
            {
                return;
            }

            DateTime maxDate = DatePickerHelper.GetValidMaxDate(picker.MinimumDate, picker.MaximumDate);
            DateTime currentSelectedDate = DatePickerHelper.GetValidDateTime(picker.SelectedDate, picker.MinimumDate, maxDate);
            picker.DayColumn = picker.GenerateDayColumn(dayFormat, currentSelectedDate);
            int dayIndex = formatStringOrder.IndexOf(0);
            //// Replace the day column with day interval.
            picker._columns[dayIndex] = picker.DayColumn;
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
            if (string.IsNullOrEmpty(monthFormat) || picker.SelectedIndex != 0)
            {
                return;
            }

            DateTime maxDate = DatePickerHelper.GetValidMaxDate(picker.MinimumDate, picker.MaximumDate);
            DateTime currentSelectedDate = DatePickerHelper.GetValidDateTime(picker.SelectedDate, picker.MinimumDate, maxDate);
            picker.MonthColumn = picker.GenerateMonthColumn(monthFormat, currentSelectedDate);
            int monthIndex = formatStringOrder.IndexOf(1);
            //// Replace the month column with month interval.
            picker._columns[monthIndex] = picker.MonthColumn;
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
            if (yearIndex == -1 || picker.SelectedIndex != 0)
            {
                return;
            }

            DateTime maxDate = DatePickerHelper.GetValidMaxDate(picker.MinimumDate, picker.MaximumDate);
            DateTime currentSelectedDate = DatePickerHelper.GetValidDateTime(picker.SelectedDate, picker.MinimumDate, maxDate);
            picker.YearColumn = picker.GenerateYearColumn(currentSelectedDate);
            //// Replace the year column with year interval.
            picker._columns[yearIndex] = picker.YearColumn;
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
            picker.HourColumn = picker.GenerateHourColumn(hourFormat, selectedTime, selectedDate);
            if (picker.SelectedIndex == 1)
            {
                int hourIndex = formatStringOrder.IndexOf(0);
                //// Replace the hour column with hour interval.
                picker._columns[hourIndex] = picker.HourColumn;
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
            if (picker.SelectedIndex == 1)
            {
                picker.MinuteColumn = picker.GenerateMinuteColumn(selectedTime, selectedDate);
                //// Replace the minute column with minute interval.
                picker._columns[minuteIndex] = picker.MinuteColumn;
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
            if (picker.SelectedIndex == 1)
            {
                picker.SecondColumn = picker.GenerateSecondColumn(selectedTime, selectedDate);
                //// Replace the second column with second interval.
                picker._columns[secondIndex] = picker.SecondColumn;
            }
        }

        /// <summary>
        /// Method invokes on millisecond interval property changed.
        /// </summary>
        /// <param name="bindable">The picker object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnMilliSecondIntervalPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfDateTimePicker? picker = bindable as SfDateTimePicker;
            if (picker == null || (int)newValue <= 0)
            {
                return;
            }

            //// Get the format string order and check the index.
            List<int> formatStringOrder = TimePickerHelper.GetFormatStringOrder(out _, picker.TimeFormat);
            int millisecondIndex = formatStringOrder.IndexOf(4);
            if (millisecondIndex == -1)
            {
                return;
            }

            DateTime maxDate = DatePickerHelper.GetValidMaxDate(picker.MinimumDate, picker.MaximumDate);
            DateTime selectedDate = DatePickerHelper.GetValidDateTime(picker.SelectedDate, picker.MinimumDate, maxDate);
            TimeSpan selectedTime = new TimeSpan(0, selectedDate.Hour, selectedDate.Minute, selectedDate.Second, selectedDate.Millisecond);
            if (picker.SelectedIndex == 1)
            {
                picker.MillisecondColumn = picker.GenerateMillisecondColumn(selectedTime, selectedDate);
                //// Replace the second column with second interval.
                picker._columns[millisecondIndex] = picker.MillisecondColumn;
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

        /// <summary>
        /// Method invokes in ActiveView property changed.
        /// </summary>
        /// <param name="bindable">The sfdatetimepicker object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        private static void OnActiveViewPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfDateTimePicker? datetimepicker = bindable as SfDateTimePicker;
            if (datetimepicker == null)
            {
                return;
            }

            datetimepicker.ResetHeaderHighlight();
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

        /// <summary>
        /// Method invokes on the picker day column text color changed.
        /// </summary>
        /// <param name="bindable">The text style object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        private static void OnNormalDayColumnTextColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfDateTimePicker? picker = bindable as SfDateTimePicker;
            if (picker == null)
            {
                return;
            }

            picker.DayColumnTextStyle.TextColor = picker.NormalDayColumnTextColor;
        }

        /// <summary>
        /// Method invokes on the picker month column text color changed.
        /// </summary>
        /// <param name="bindable">The text style object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        private static void OnNormalMonthColumnTextColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfDateTimePicker? picker = bindable as SfDateTimePicker;
            if (picker == null)
            {
                return;
            }

            picker.MonthColumnTextStyle.TextColor = picker.NormalMonthColumnTextColor;
        }

        /// <summary>
        /// Method invokes on the picker year column text color changed.
        /// </summary>
        /// <param name="bindable">The text style object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        private static void OnNormalYearColumnTextColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfDateTimePicker? picker = bindable as SfDateTimePicker;
            if (picker == null)
            {
                return;
            }

            picker.YearColumnTextStyle.TextColor = picker.NormalYearColumnTextColor;
        }

        /// <summary>
        /// Method invokes on the picker normal day column font size changed.
        /// </summary>
        /// <param name="bindable">The text style object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        private static void OnNormalDayColumnFontSizeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfDateTimePicker? picker = bindable as SfDateTimePicker;
            if (picker == null)
            {
                return;
            }

            picker.DayColumnTextStyle.FontSize = picker.NormalDayColumnFontSize;
        }

        /// <summary>
        /// Method invokes on the picker normal month column font size changed.
        /// </summary>
        /// <param name="bindable">The text style object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        private static void OnNormalMonthColumnFontSizeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfDateTimePicker? picker = bindable as SfDateTimePicker;
            if (picker == null)
            {
                return;
            }

            picker.MonthColumnTextStyle.FontSize = picker.NormalMonthColumnFontSize;
        }

        /// <summary>
        /// Method invokes on the picker normal year column font size changed.
        /// </summary>
        /// <param name="bindable">The text style object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        private static void OnNormalYearColumnFontSizeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfDateTimePicker? picker = bindable as SfDateTimePicker;
            if (picker == null)
            {
                return;
            }

            picker.YearColumnTextStyle.FontSize = picker.NormalYearColumnFontSize;
        }

        /// <summary>
        /// Method invokes on the picker hour column text color changed.
        /// </summary>
        /// <param name="bindable">The text style object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        private static void OnNormalHourColumnTextColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfDateTimePicker? picker = bindable as SfDateTimePicker;
            if (picker == null)
            {
                return;
            }

            picker.HourColumnTextStyle.TextColor = picker.NormalHourColumnTextColor;
        }

        /// <summary>
        /// Method invokes on the picker minute column text color changed.
        /// </summary>
        /// <param name="bindable">The text style object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        private static void OnNormalMinuteColumnTextColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfDateTimePicker? picker = bindable as SfDateTimePicker;
            if (picker == null)
            {
                return;
            }

            picker.MinuteColumnTextStyle.TextColor = picker.NormalMinuteColumnTextColor;
        }

        /// <summary>
        /// Method invokes on the picker second column text color changed.
        /// </summary>
        /// <param name="bindable">The text style object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        private static void OnNormalSecondColumnTextColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfDateTimePicker? picker = bindable as SfDateTimePicker;
            if (picker == null)
            {
                return;
            }

            picker.SecondColumnTextStyle.TextColor = picker.NormalSecondColumnTextColor;
        }

        /// <summary>
        /// Method invokes on the picker meridiem column text color changed.
        /// </summary>
        /// <param name="bindable">The text style object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        private static void OnNormalMeridiemColumnTextColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfDateTimePicker? picker = bindable as SfDateTimePicker;
            if (picker == null)
            {
                return;
            }

            picker.MeridiemColumnTextStyle.TextColor = picker.NormalMeridiemColumnTextColor;
        }

        /// <summary>
        /// Method invokes on the picker milli second column text color changed.
        /// </summary>
        /// <param name="bindable">The text style object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        private static void OnNormalMilliSecondColumnTextColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfDateTimePicker? picker = bindable as SfDateTimePicker;
            if (picker == null)
            {
                return;
            }

            picker.MilliSecondColumnTextStyle.TextColor = picker.NormalMilliSecondColumnTextColor;
        }

        /// <summary>
        /// Method invokes on the picker normal hour column font size changed.
        /// </summary>
        /// <param name="bindable">The text style object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        private static void OnNormalHourColumnFontSizeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfDateTimePicker? picker = bindable as SfDateTimePicker;
            if (picker == null)
            {
                return;
            }

            picker.HourColumnTextStyle.FontSize = picker.NormalHourColumnFontSize;
        }

        /// <summary>
        /// Method invokes on the picker normal minute column font size changed.
        /// </summary>
        /// <param name="bindable">The text style object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        private static void OnNormalMinuteColumnFontSizeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfDateTimePicker? picker = bindable as SfDateTimePicker;
            if (picker == null)
            {
                return;
            }

            picker.MinuteColumnTextStyle.FontSize = picker.NormalMinuteColumnFontSize;
        }

        /// <summary>
        /// Method invokes on the picker normal second column font size changed.
        /// </summary>
        /// <param name="bindable">The text style object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        private static void OnNormalSecondColumnFontSizeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfDateTimePicker? picker = bindable as SfDateTimePicker;
            if (picker == null)
            {
                return;
            }

            picker.SecondColumnTextStyle.FontSize = picker.NormalSecondColumnFontSize;
        }

        /// <summary>
        /// Method invokes on the picker normal meridiem column font size changed.
        /// </summary>
        /// <param name="bindable">The text style object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        private static void OnNormalMeridiemColumnFontSizeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfDateTimePicker? picker = bindable as SfDateTimePicker;
            if (picker == null)
            {
                return;
            }

            picker.MeridiemColumnTextStyle.FontSize = picker.NormalMeridiemColumnFontSize;
        }

        /// <summary>
        /// Method invokes on the picker normal milli second font size changed.
        /// </summary>
        /// <param name="bindable">The text style object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        private static void OnNormalMilliSecondColumnFontSizeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfDateTimePicker? picker = bindable as SfDateTimePicker;
            if (picker == null)
            {
                return;
            }

            picker.MilliSecondColumnTextStyle.FontSize = picker.NormalMilliSecondColumnFontSize;
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