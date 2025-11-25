using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Input;
using Syncfusion.Maui.Toolkit.Themes;

namespace Syncfusion.Maui.Toolkit.Picker
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SfDatePicker"/> class that represents a control, used to select the date with in specified date range.
    /// </summary>
    public class SfDatePicker : PickerBase, IParentThemeElement, IThemeElement
    {
        #region Fields

        /// <summary>
        /// Holds the selected date in dialog mode.
        /// </summary>
        internal DateTime? _internalSelectedDate;

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
                typeof(PickerHeaderView),
                typeof(SfDatePicker),
                defaultValueCreator: bindable => new PickerHeaderView(),
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
               typeof(DatePickerColumnHeaderView),
               typeof(SfDatePicker),
               defaultValueCreator: bindable => new DatePickerColumnHeaderView(),
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
                typeof(SfDatePicker),
                defaultValueCreator: bindable => DateTime.Now.Date,
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
                typeof(SfDatePicker),
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
                typeof(SfDatePicker),
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
                typeof(SfDatePicker),
                1,
                propertyChanged: OnYearIntervalPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="Format"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Format"/> dependency property.
        /// </value>
        public static readonly BindableProperty FormatProperty =
            BindableProperty.Create(
                nameof(Format),
                typeof(PickerDateFormat),
                typeof(SfDatePicker),
                PickerDateFormat.yyyy_MM_dd,
                propertyChanged: OnFormatPropertyChanged);

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
                typeof(SfDatePicker),
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
                typeof(SfDatePicker),
                defaultValueCreator: bindable => new DateTime(2100, 12, 31),
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
                typeof(SfDatePicker),
                null);

        /// <summary>
        /// Identifies the <see cref="BlackoutDates"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="BlackoutDates"/> dependency property.
        /// </value>
        public static readonly BindableProperty BlackoutDatesProperty =
            BindableProperty.Create(
                nameof(BlackoutDates),
                typeof(ObservableCollection<DateTime>),
                typeof(SfDatePicker),
                defaultValueCreator: bindable => new ObservableCollection<DateTime>(),
                propertyChanged: OnBlackOutDatesPropertyChanged);

        #endregion

        #region Internal Bindable Properties

        /// <summary>
        /// Identifies the <see cref="DatePickerBackground"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="DatePickerBackground"/> bindable property.
        /// </value>
        internal static readonly BindableProperty DatePickerBackgroundProperty =
            BindableProperty.Create(
                nameof(DatePickerBackground),
                typeof(Color),
                typeof(SfDatePicker),
                defaultValueCreator: bindable => Color.FromArgb("#EEE8F4"),
                propertyChanged: OnDatePickerBackgroundChanged);

        /// <summary>
        /// Identifies the <see cref="HeaderTextColor"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="HeaderTextColor"/> dependency property.
        /// </value>
        internal static readonly BindableProperty HeaderTextColorProperty =
            BindableProperty.Create(
                nameof(HeaderTextColor),
                typeof(Color),
                typeof(SfDatePicker),
                defaultValueCreator: bindable => Color.FromArgb("#49454F"),
                propertyChanged: OnHeaderTextColorChanged);

        /// <summary>
        /// Identifies the <see cref="HeaderFontSize"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="HeaderFontSize"/> dependency property.
        /// </value>
        internal static readonly BindableProperty HeaderFontSizeProperty =
            BindableProperty.Create(
                nameof(HeaderFontSize),
                typeof(double),
                typeof(SfDatePicker),
                defaultValueCreator: bindable => 16d,
                propertyChanged: OnHeaderFontSizeChanged);

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
                typeof(SfDatePicker),
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
                typeof(SfDatePicker),
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
                typeof(SfDatePicker),
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
                typeof(SfDatePicker),
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
                typeof(SfDatePicker),
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
                typeof(SfDatePicker),
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
                typeof(SfDatePicker),
                defaultValueCreator: bindable => Color.FromArgb("#611C1B1F"),
                propertyChanged: OnDisabledTextColorChanged);

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SfDatePicker"/> class.
        /// </summary>
        public SfDatePicker()
        {
            _dayColumn = new PickerColumn();
            _monthColumn = new PickerColumn();
            _yearColumn = new PickerColumn();
            _columns = new ObservableCollection<PickerColumn>();
            Initialize();
            GeneratePickerColumns();
            BaseColumns = _columns;
            SelectionIndexChanged += OnPickerSelectionIndexChanged;
            BlackoutDates.CollectionChanged += OnBlackoutDates_CollectionChanged;
            BackgroundColor = DatePickerBackground;
            InitializePickerStyle();
            Dispatcher.Dispatch(() =>
            {
                InitializeTheme();
            });
            ColumnHeaderView.Parent = this;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the value of header view. This property can be used to customize the header in SfDatePicker.
        /// </summary>
        /// <example>
        /// The following example demonstrates how to customize the header view of SfDatePicker.
        /// <code>
        /// <![CDATA[
        /// SfDatePicker datePicker = new SfDatePicker();
        /// datePicker.HeaderView = new PickerHeaderView
        /// {
        ///     Text = "Select Date",
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
        public PickerHeaderView HeaderView
        {
            get { return (PickerHeaderView)GetValue(HeaderViewProperty); }
            set { SetValue(HeaderViewProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value of column header view. This property can be used to customize the header column in SfDatePicker.
        /// </summary>
        /// /// <example>
        /// The following example demonstrates how to customize the column header view of SfDatePicker.
        /// <code>
        /// <![CDATA[
        /// SfDatePicker datePicker = new SfDatePicker();
        /// datePicker.ColumnHeaderView = new DatePickerColumnHeaderView
        /// {
        ///     Background = new SolidColorBrush(Colors.LightBlue),
        ///     Height = 40,
        ///     DividerColor = Colors.Gray,
        ///     TextStyle = new PickerTextStyle
        ///     {
        ///         TextColor = Colors.DarkBlue,
        ///         FontSize = 16
        ///     },
        ///     DayHeaderText = "Date",
        ///     MonthHeaderText = "Month",
        ///     YearHeaderText = "Year"
        /// };
        /// ]]>
        /// </code>
        /// </example>
        public DatePickerColumnHeaderView ColumnHeaderView
        {
            get { return (DatePickerColumnHeaderView)GetValue(ColumnHeaderViewProperty); }
            set { SetValue(ColumnHeaderViewProperty, value); }
        }

        /// <summary>
        /// Gets or sets the date picker selection date in SfDatePicker.
        /// </summary>
        /// <value>The default value of <see cref="SfDatePicker.SelectedDate"/> is <see cref="DateTime.Now"/>.</value>
        /// <example>
        /// The following examples demonstrate how to set the selected date in SfDatePicker.
        /// # [XAML](#tab/tabid-1)
        /// <code language="xaml">
        /// <![CDATA[
        /// <Picker:SfDatePicker x:Name="DatePicker"
        ///                      SelectedDate="2023-06-15" />
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-2)
        /// <code language="C#">
        /// SfDatePicker datePicker = new SfDatePicker();
        /// datePicker.SelectedDate = new DateTime(2023, 6, 15);
        /// </code>
        /// </example>
        public DateTime? SelectedDate
        {
            get { return (DateTime?)GetValue(SelectedDateProperty); }
            set { SetValue(SelectedDateProperty, value); }
        }

        /// <summary>
        /// Gets or sets the day interval in SfDatePicker.
        /// </summary>
        /// <value>The default value of <see cref="SfDatePicker.DayInterval"/> is 1.</value>
        /// <example>
        /// The following examples demonstrate how to set the day interval in SfDatePicker.
        /// # [XAML](#tab/tabid-3)
        /// <code language="xaml">
        /// <![CDATA[
        /// <Picker:SfDatePicker x:Name="DatePicker"
        ///                      DayInterval="2" />
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-4)
        /// <code language="C#">
        /// SfDatePicker datePicker = new SfDatePicker();
        /// datePicker.DayInterval = 2;
        /// </code>
        /// </example>
        public int DayInterval
        {
            get { return (int)GetValue(DayIntervalProperty); }
            set { SetValue(DayIntervalProperty, value); }
        }

        /// <summary>
        /// Gets or sets the month interval in SfDatePicker.
        /// </summary>
        /// <value>The default value of <see cref="SfDatePicker.MonthInterval"/> is 1.</value>
        /// <example>
        /// The following examples demonstrate how to set the month interval in SfDatePicker.
        /// # [XAML](#tab/tabid-5)
        /// <code language="xaml">
        /// <![CDATA[
        /// <Picker:SfDatePicker x:Name="DatePicker"
        ///                      MonthInterval="2" />
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-6)
        /// <code language="C#">
        /// SfDatePicker datePicker = new SfDatePicker();
        /// datePicker.MonthInterval = 2;
        /// </code>
        /// </example>
        public int MonthInterval
        {
            get { return (int)GetValue(MonthIntervalProperty); }
            set { SetValue(MonthIntervalProperty, value); }
        }

        /// <summary>
        /// Gets or sets the year interval in SfDatePicker.
        /// </summary>
        /// <value>The default value of <see cref="SfDatePicker.YearInterval"/> is 1.</value>
        /// <example>
        /// The following examples demonstrate how to set the year interval in SfDatePicker.
        /// # [XAML](#tab/tabid-7)
        /// <code language="xaml">
        /// <![CDATA[
        /// <Picker:SfDatePicker x:Name="DatePicker"
        ///                      YearInterval="2" />
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-8)
        /// <code language="C#">
        /// SfDatePicker datePicker = new SfDatePicker();
        /// datePicker.YearInterval = 2;
        /// </code>
        /// </example>
        public int YearInterval
        {
            get { return (int)GetValue(YearIntervalProperty); }
            set { SetValue(YearIntervalProperty, value); }
        }

        /// <summary>
        /// Gets or sets the picker date format in SfDatePicker.
        /// </summary>
        /// <value>The default value of <see cref="SfDatePicker.Format"/> is <see cref="PickerDateFormat.yyyy_MM_dd"/>.</value>
        /// <example>
        /// The following examples demonstrate how to set the format in SfDatePicker.
        /// # [XAML](#tab/tabid-9)
        /// <code language="xaml">
        /// <![CDATA[
        /// <Picker:SfDatePicker x:Name="DatePicker"
        ///                      Format="dd_MM_yyyy" />
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-10)
        /// <code language="C#">
        /// SfDatePicker datePicker = new SfDatePicker();
        /// datePicker.Format = PickerDateFormat.dd_MM_yyyy;
        /// </code>
        /// </example>
        public PickerDateFormat Format
        {
            get { return (PickerDateFormat)GetValue(FormatProperty); }
            set { SetValue(FormatProperty, value); }
        }

        /// <summary>
        /// Gets or sets the minimum date in SfDatePicker.
        /// </summary>
        /// <value>The default value of <see cref="SfDatePicker.MinimumDate"/> is "DateTime(1900, 01, 01)".</value>
        /// <example>
        /// The following examples demonstrate how to set the minimum date in SfDatePicker.
        /// # [XAML](#tab/tabid-11)
        /// <code language="xaml">
        /// <![CDATA[
        /// <Picker:SfDatePicker x:Name="DatePicker"
        ///                      MinimumDate="2023-01-01" />
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-12)
        /// <code language="C#">
        /// SfDatePicker datePicker = new SfDatePicker();
        /// datePicker.MinimumDate = new DateTime(2023, 1, 1);
        /// </code>
        /// </example>
        public DateTime MinimumDate
        {
            get { return (DateTime)GetValue(MinimumDateProperty); }
            set { SetValue(MinimumDateProperty, value); }
        }

        /// <summary>
        /// Gets or sets the maximum date in SfDatePicker.
        /// </summary>
        /// <value>The default value of <see cref="SfDatePicker.MaximumDate"/> is "DateTime(2100, 12, 31)".</value>
        /// <example>
        /// The following examples demonstrate how to set the maximum date in SfDatePicker.
        /// # [XAML](#tab/tabid-13)
        /// <code language="xaml">
        /// <![CDATA[
        /// <Picker:SfDatePicker x:Name="DatePicker"
        ///                      MaximumDate="2023-12-31" />
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-14)
        /// <code language="C#">
        /// SfDatePicker datePicker = new SfDatePicker();
        /// datePicker.MaximumDate = new DateTime(2023, 12, 31);
        /// </code>
        /// </example>
        public DateTime MaximumDate
        {
            get { return (DateTime)GetValue(MaximumDateProperty); }
            set { SetValue(MaximumDateProperty, value); }
        }

        /// <summary>
        /// Gets or sets the selection changed command in SfDatePicker.
        /// </summary>
        /// <value>The default value of <see cref="SfDatePicker.SelectionChangedCommand"/> is null.</value>
        /// <example>
        /// The following example demonstrates how to set the selection changed command in SfDatePicker.
        /// # [XAML](#tab/tabid-15)
        /// <code Lang="XAML"><![CDATA[
        /// <ContentPage.BindingContext>
        ///    <local:ViewModel/>
        /// </ContentPage.BindingContext>
        /// <Picker:SfDatePicker x:Name="DatePicker"
        ///                      SelectionChangedCommand="{Binding SelectionCommand}">
        /// </Picker:SfDatePicker>
        /// ]]></code>
        /// # [C#](#tab/tabid-16)
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
        /// Gets or sets the BlackoutDates in SfDatePicker.
        /// </summary>
        /// <remarks>The selection view will not be applicable when setting blackout dates.</remarks>
        /// <example>
        /// The following examples demonstrate how to set the blackout dates in SfDatePicker.
        /// # [XAML](#tab/tabid-17)
        /// <code language="xaml">
        /// <![CDATA[
        /// <picker:SfDatePicker x:Name="picker">
        ///     <picker:SfDatePicker.BlackoutDates>
        ///         <date:DateTime>2001-08-10</date:DateTime>
        ///         <date:DateTime>2001-08-12</date:DateTime>
        ///         <date:DateTime>2001-08-14</date:DateTime>
        ///         <date:DateTime>2001-08-17</date:DateTime>
        ///         <date:DateTime>2001-08-18</date:DateTime>
        ///         <date:DateTime>2001-08-20</date:DateTime>
        ///     </picker:SfDatePicker.BlackoutDates>
        /// </picker:SfDatePicker>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-18)
        /// <code language="C#">
        /// SfDatePicker picker = new SfDatePicker();
        /// picker.BlackoutDates.Add(new DateTime(2001, 8, 10));
        /// picker.BlackoutDates.Add(new DateTime(2001, 8, 12));
        /// picker.BlackoutDates.Add(new DateTime(2001, 8, 14));
        /// picker.BlackoutDates.Add(new DateTime(2001, 8, 17));
        /// picker.BlackoutDates.Add(new DateTime(2001, 8, 18));
        /// picker.BlackoutDates.Add(new DateTime(2001, 8, 20));
        /// </code>
        /// </example>
        public ObservableCollection<DateTime> BlackoutDates
        {
            get { return (ObservableCollection<DateTime>)GetValue(BlackoutDatesProperty); }
            set { SetValue(BlackoutDatesProperty, value); }
        }

        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets or sets the background color of the date picker.
        /// </summary>
        internal Color DatePickerBackground
        {
            get { return (Color)GetValue(DatePickerBackgroundProperty); }
            set { SetValue(DatePickerBackgroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets the header text color of the text style.
        /// </summary>
        internal Color HeaderTextColor
        {
            get { return (Color)GetValue(HeaderTextColorProperty); }
            set { SetValue(HeaderTextColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the header font size of the text style.
        /// </summary>
        internal double HeaderFontSize
        {
            get { return (double)GetValue(HeaderFontSizeProperty); }
            set { SetValue(HeaderFontSizeProperty, value); }
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
        /// Method to update the date picker format.
        /// </summary>
        internal void UpdateFormat()
        {
            _dayColumn = new PickerColumn();
            _monthColumn = new PickerColumn();
            _yearColumn = new PickerColumn();
            GeneratePickerColumns();
            BaseColumns = _columns;
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
            string dayFormat;
            string monthFormat;
            List<int> formatStringOrder = DatePickerHelper.GetFormatStringOrder(out dayFormat, out monthFormat, Format);
            int changedColumnValue = formatStringOrder[e.ColumnIndex];
            DateTime maxDate = DatePickerHelper.GetValidMaxDate(MinimumDate, MaximumDate);

            // Determine the date to use based on the current mode and available date values:
            // - If not in Default mode and InternalSelectedDate has a value, use it.
            // - Otherwise, use SelectedDate if available.
            // - If neither is available, fall back to PreviousSelectedDateTime's date part.
            DateTime date = IsScrollSelectionAllowed() && _internalSelectedDate.HasValue ? _internalSelectedDate.Value : SelectedDate ?? _previousSelectedDateTime.Date;
            DateTime? previousSelectedDate = DatePickerHelper.GetValidDate(date, MinimumDate, maxDate);
            if (previousSelectedDate == null)
            {
                return;
            }

            DateTime selectedDate = DateTime.Now;

            switch (changedColumnValue)
            {
                //// Need to handle the day selection changes.
                case 0:
                    {
                        int day = GetDayFromCollection(e);
                        selectedDate = new DateTime(previousSelectedDate.Value.Year, previousSelectedDate.Value.Month, day);
                    }

                    break;
                //// Need to handle the month selection changes.
                case 1:
                    {
                        int month, day;
                        GetMonthFromCollection(e, dayFormat, monthFormat, maxDate, previousSelectedDate, out month, out day);
                        selectedDate = new DateTime(previousSelectedDate.Value.Year, month, day);
                    }

                    break;
                //// Need to handle the year selection changes.
                case 2:
                    {
                        int year, month, day;
                        GetYearFromCollection(e, dayFormat, monthFormat, maxDate, previousSelectedDate, out year, out month, out day);
                        selectedDate = new DateTime(year, month, day);
                    }

                    break;
            }

            // Check if the current mode is not the default picker mode
            if (IsScrollSelectionAllowed())
            {
                // Check if the selected date falls within any blackout dates
                if (BlackoutDates.Any(blackOutDate => DatePickerHelper.IsBlackoutDate(true, string.Empty, blackOutDate, selectedDate)))
                {
                    // If the selected date is a blackout date, revert the selection to the previous value
                    _dayColumn.SelectedIndex = e.OldValue;
                }
                // Set the internal selected date to the selected date (without time component)
                _internalSelectedDate = selectedDate.Date;
                // Update the selected item in the column based on the new internal selected date
                UpdateSelectedItemInColumn();
                // Ensure the internal selected date is within the allowed minimum and maximum date range
                _internalSelectedDate = DatePickerHelper.GetValidDate(_internalSelectedDate, MinimumDate, MaximumDate);
                // Update the selected index in the UI to reflect the validated internal selected date
                UpdateSelectedIndex(_internalSelectedDate);
            }
            else
            {
                // If in default mode, update the selected date only if it has changed
                if (!DatePickerHelper.IsSameDate(selectedDate, SelectedDate))
                {
                    // Set the selected date to the new date (without time component)
                    SelectedDate = selectedDate.Date;
                }
            }
        }

        /// <summary>
        /// Method to get the year from the year collection.
        /// </summary>
        /// <param name="e">The event args.</param>
        /// <param name="dayFormat">The day format.</param>
        /// <param name="monthFormat">The month format.</param>
        /// <param name="maxDate">The maximum date.</param>
        /// <param name="previousSelectedDate">The previous selected date.</param>
        /// <param name="year">The selected year.</param>
        /// <param name="month">The selected month.</param>
        /// <param name="day">The selected day.</param>
        void GetYearFromCollection(PickerSelectionChangedEventArgs e, string dayFormat, string monthFormat, DateTime maxDate, DateTime? previousSelectedDate, out int year, out int month, out int day)
        {
            year = MinimumDate.Year;
            month = MinimumDate.Month;
            day = MinimumDate.Day;
            if (previousSelectedDate == null)
            {
                return;
            }

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
            int monthIndex = DatePickerHelper.GetMonthIndex(monthFormat, months, previousSelectedDate.Value.Month);
            month = 1;
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
            int index = DatePickerHelper.GetDayIndex(dayFormat, days, previousSelectedDate.Value.Day);
            day = index == -1 ? 1 : int.Parse(days[index]);
        }

        /// <summary>
        /// Method to get the month value from the month collection.
        /// </summary>
        /// <param name="e">The event args.</param>
        /// <param name="dayFormat">The day format.</param>
        /// <param name="monthFormat">The month format.</param>
        /// <param name="maxDate">The maximum date.</param>
        /// <param name="previousSelectedDate">The previous selected date.</param>
        /// <param name="month">The selected month.</param>
        /// <param name="day">The selected day.</param>
        void GetMonthFromCollection(PickerSelectionChangedEventArgs e, string dayFormat, string monthFormat, DateTime maxDate, DateTime? previousSelectedDate, out int month, out int day)
        {
            month = 1;
            day = 1;
            if (previousSelectedDate == null)
            {
                return;
            }

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

            ObservableCollection<string> days = DatePickerHelper.GetDays(dayFormat, month, previousSelectedDate.Value.Year, MinimumDate, maxDate, DayInterval);
            ObservableCollection<string> previousDays = _dayColumn.ItemsSource is ObservableCollection<string> previousDayCollection ? previousDayCollection : new ObservableCollection<string>();
            //// Check the month selection changes needed to update the days collection.
            if (!PickerHelper.IsCollectionEquals(days, previousDays))
            {
                _dayColumn.ItemsSource = days;
            }

            //// Check the new days collection have a selected day value, if not then update the nearby value.
            int index = DatePickerHelper.GetDayIndex(dayFormat, days, previousSelectedDate.Value.Day);
            day = index == -1 ? 1 : int.Parse(days[index]);
        }

        /// <summary>
        /// Method to get the day value from day collection.
        /// </summary>
        /// <param name="e">The event args.</param>
        /// <returns>The day collection index value.</returns>
        int GetDayFromCollection(PickerSelectionChangedEventArgs e)
        {
            int day = 1;
            if (_dayColumn.ItemsSource != null && _dayColumn.ItemsSource is ObservableCollection<string> dayCollection && dayCollection.Count > e.NewValue)
            {
                //// Get the day value based on the selected index changes value.
                day = int.Parse(dayCollection[e.NewValue]);
            }

            return day;
        }

        /// <summary>
        /// Method to update the minimum and maximum date value for all the picker column based on the date value.
        /// </summary>
        /// <param name="oldValue">Minimum and Maximum oldvalue.</param>
        /// <param name="newValue">Minimum and Maximum newvalue.</param>
        void UpdateMinimumMaximumDate(object oldValue, object newValue)
        {
            string dayFormat;
            string monthFormat;
            List<int> formatString = DatePickerHelper.GetFormatStringOrder(out dayFormat, out monthFormat, Format);
            DateTime oldDate = (DateTime)oldValue;
            DateTime newDate = (DateTime)newValue;
            DateTime maxDate = DatePickerHelper.GetValidMaxDate(MinimumDate, MaximumDate);
            DateTime? validSelectedDate = DatePickerHelper.GetValidDate(SelectedDate, MinimumDate, maxDate);
            int yearIndex = formatString.IndexOf(2);
            if (yearIndex != -1 && oldDate.Year != newDate.Year)
            {
                _yearColumn = GenerateYearColumn(validSelectedDate);
                _yearColumn.Parent = this;
                _columns[yearIndex] = _yearColumn;
            }

            if (validSelectedDate != null)
            {
                if (validSelectedDate.Value.Year == MinimumDate.Year || validSelectedDate.Value.Year == maxDate.Year)
                {
                    ObservableCollection<string> month = DatePickerHelper.GetMonths(monthFormat, validSelectedDate.Value.Year, MinimumDate, maxDate, MonthInterval);
                    ObservableCollection<string> previousMonths = _monthColumn.ItemsSource is ObservableCollection<string> previousMonthCollection ? previousMonthCollection : new ObservableCollection<string>();
                    //// Check the year index changes needed to update the month collection.
                    if (!PickerHelper.IsCollectionEquals(month, previousMonths))
                    {
                        _monthColumn = new PickerColumn()
                        {
                            ItemsSource = month,
                            SelectedIndex = DatePickerHelper.GetMonthIndex(monthFormat, month, validSelectedDate.Value.Month),
                            HeaderText = SfPickerResources.GetLocalizedString(ColumnHeaderView.MonthHeaderText),
                        };
                        int monthIndex = formatString.IndexOf(1);
                        if (monthIndex != -1)
                        {
                            _columns[monthIndex] = _monthColumn;
                        }
                    }
                }

                if (!string.IsNullOrEmpty(dayFormat) && ((validSelectedDate.Value.Year == MinimumDate.Year && validSelectedDate.Value.Month == MinimumDate.Month) || (validSelectedDate.Value.Year == maxDate.Year && validSelectedDate.Value.Month == maxDate.Month)))
                {
                    ObservableCollection<string> days = DatePickerHelper.GetDays(dayFormat, validSelectedDate.Value.Month, validSelectedDate.Value.Year, MinimumDate, maxDate, DayInterval);
                    ObservableCollection<string> previousDays = _dayColumn.ItemsSource is ObservableCollection<string> previousDayCollection ? previousDayCollection : new ObservableCollection<string>();
                    //// Check the year and month(if month items source updated) changes needed to change the day collection.
                    if (!PickerHelper.IsCollectionEquals(days, previousDays))
                    {
                        _dayColumn = new PickerColumn()
                        {
                            ItemsSource = days,
                            SelectedIndex = DatePickerHelper.GetDayIndex(dayFormat, days, validSelectedDate.Value.Day),
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
        }

        /// <summary>
        /// Method to update the selected index value for all the picker column based on the date value.
        /// </summary>
        /// <param name="date">The selected date value.</param>
        void UpdateSelectedIndex(DateTime? date)
        {
            if (date == null)
            {
                return;
            }

            string dayFormat;
            string monthFormat;
            DatePickerHelper.GetFormatStringOrder(out dayFormat, out monthFormat, Format);
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

            if (IsScrollSelectionAllowed() && SelectedDate == null)
            {
                UpdateSelectedItemInColumn();
            }
        }

        /// <summary>
        /// Method invokes on column header property changed.
        /// </summary>
        /// <param name="sender">Column header view value.</param>
        /// <param name="e">Property changed arguments.</param>
        void OnColumnHeaderPropertyChanged(object? sender, PickerPropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(DatePickerColumnHeaderView.Background))
            {
                BaseColumnHeaderView.Background = ColumnHeaderView.Background;
            }
            else if (e.PropertyName == nameof(DatePickerColumnHeaderView.Height))
            {
                BaseColumnHeaderView.Height = ColumnHeaderView.Height;
            }
            else if (e.PropertyName == nameof(DatePickerColumnHeaderView.DividerColor))
            {
                BaseColumnHeaderView.DividerColor = ColumnHeaderView.DividerColor;
            }
            else if (e.PropertyName == nameof(DatePickerColumnHeaderView.TextStyle))
            {
                SetInheritedBindingContext(ColumnHeaderView.TextStyle, BindingContext);
                BaseColumnHeaderView.TextStyle = ColumnHeaderView.TextStyle;
            }
            else if (e.PropertyName == nameof(DatePickerColumnHeaderView.DayHeaderText))
            {
                _dayColumn.HeaderText = SfPickerResources.GetLocalizedString(ColumnHeaderView.DayHeaderText);
            }
            else if (e.PropertyName == nameof(DatePickerColumnHeaderView.MonthHeaderText))
            {
                _monthColumn.HeaderText = SfPickerResources.GetLocalizedString(ColumnHeaderView.MonthHeaderText);
            }
            else if (e.PropertyName == nameof(DatePickerColumnHeaderView.YearHeaderText))
            {
                _yearColumn.HeaderText = SfPickerResources.GetLocalizedString(ColumnHeaderView.YearHeaderText);
            }
        }

        /// <summary>
        /// Method to generate the day, month and year columns based on the selected date value.
        /// </summary>
        void GeneratePickerColumns()
        {
            string dayFormat;
            string monthFormat;
            List<int> formatStringOrder = DatePickerHelper.GetFormatStringOrder(out dayFormat, out monthFormat, Format);
            DateTime maxDate = DatePickerHelper.GetValidMaxDate(MinimumDate, MaximumDate);
            DateTime? selectedDate = DatePickerHelper.GetValidDate(SelectedDate, MinimumDate, maxDate);
            ObservableCollection<PickerColumn> pickerColumns = new ObservableCollection<PickerColumn>();
            foreach (int index in formatStringOrder)
            {
                switch (index)
                {
                    case 0:
                        _dayColumn = GenerateDayColumn(dayFormat, selectedDate);
                        _dayColumn.SelectedItem = SelectedDate != null ? PickerHelper.GetSelectedItemDefaultValue(_dayColumn) : null;
                        pickerColumns.Add(_dayColumn);
                        break;
                    case 1:
                        _monthColumn = GenerateMonthColumn(monthFormat, selectedDate);
                        _monthColumn.SelectedItem = SelectedDate != null ? PickerHelper.GetSelectedItemDefaultValue(_monthColumn) : null;
                        pickerColumns.Add(_monthColumn);
                        break;
                    case 2:
                        _yearColumn = GenerateYearColumn(selectedDate);
                        _yearColumn.SelectedItem = SelectedDate != null ? PickerHelper.GetSelectedItemDefaultValue(_yearColumn) : null;
                        pickerColumns.Add(_yearColumn);
                        break;
                }
            }

            _columns = pickerColumns;
        }

        /// <summary>
        /// Need to update the each column selected item in a datepicker.
        /// </summary>
        void UpdateSelectedItemInColumn()
        {
            _dayColumn.SelectedItem = PickerHelper.GetSelectedItemDefaultValue(_dayColumn);
            _yearColumn.SelectedItem = PickerHelper.GetSelectedItemDefaultValue(_yearColumn);
            _monthColumn.SelectedItem = PickerHelper.GetSelectedItemDefaultValue(_monthColumn);
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

            // Use the selectedDate if provided, otherwise use 'previous'
            DateTime referenceDate = selectedDate ?? _previousSelectedDateTime;

            ObservableCollection<string> days = DatePickerHelper.GetDays(
                format,
                referenceDate.Month,
                referenceDate.Year,
                MinimumDate,
                maxDate,
                DayInterval);

            int selectedIndex = DatePickerHelper.GetDayIndex(
                format,
                days,
                referenceDate.Day);

            return new PickerColumn
            {
                ItemsSource = days,
                SelectedIndex = selectedIndex,
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

            // Use the selectedDate if provided, otherwise use 'previous'
            DateTime referenceDate = selectedDate ?? _previousSelectedDateTime;

            ObservableCollection<string> months = DatePickerHelper.GetMonths(
                format,
                referenceDate.Year,
                MinimumDate,
                maxDate,
                MonthInterval);

            int selectedIndex = DatePickerHelper.GetMonthIndex(
                format,
                months,
                referenceDate.Month);

            return new PickerColumn
            {
                ItemsSource = months,
                SelectedIndex = selectedIndex,
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

            // Use 'selectedDate' if provided, otherwise fall back to 'previous'
            int year = selectedDate?.Year ?? _previousSelectedDateTime.Year;

            ObservableCollection<string> years = DatePickerHelper.GetYears(
                MinimumDate,
                maxDate,
                YearInterval);

            int selectedIndex = DatePickerHelper.GetYearIndex(years, year);

            return new PickerColumn
            {
                ItemsSource = years,
                SelectedIndex = selectedIndex,
                HeaderText = SfPickerResources.GetLocalizedString(ColumnHeaderView.YearHeaderText),
            };
        }

        /// <summary>
        /// Method trigged when the black out date collection gets changed.
        /// </summary>
        /// <param name="sender">Date picker instance</param>
        /// <param name="e">Collection changed event arguments</param>
        void OnBlackoutDates_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (SelectedDate != null)
            {
                DateTime currentDate = SelectedDate.Value;
                while (BlackoutDates.Any(blackOutDate => DatePickerHelper.IsBlackoutDate(true, string.Empty, blackOutDate, currentDate)))
                {
                    currentDate = currentDate.AddDays(1);
                }

                if (SelectedDate != currentDate)
                {
                    SelectedDate = currentDate;
                }
            }
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

        /// <summary>
        /// Method to initialize the theme and to set dynamic resources.
        /// </summary>
        void InitializeTheme()
        {
            ThemeElement.InitializeThemeResources(this, "SfDatePickerTheme");

            SetDynamicResource(HeaderTextColorProperty, "SfDatePickerNormalHeaderTextColor");
            SetDynamicResource(HeaderFontSizeProperty, "SfDatePickerNormalHeaderFontSize");

            SetDynamicResource(FooterTextColorProperty, "SfDatePickerNormalFooterTextColor");
            SetDynamicResource(FooterFontSizeProperty, "SfDatePickerNormalFooterFontSize");
        }

        /// <summary>
        /// Method to initialize the defult picker style.
        /// </summary>
        void InitializePickerStyle()
        {
            SetDynamicResource(DatePickerBackgroundProperty, "SfDatePickerNormalBackground");

            SetDynamicResource(SelectedTextColorProperty, "SfDatePickerSelectedTextColor");
            SetDynamicResource(SelectionTextColorProperty, "SfDatePickerSelectionTextColor");
            SetDynamicResource(SelectedFontSizeProperty, "SfDatePickerSelectedFontSize");

            SetDynamicResource(NormalTextColorProperty, "SfDatePickerNormalTextColor");
            SetDynamicResource(NormalFontSizeProperty, "SfDatePickerNormalFontSize");

            SetDynamicResource(DisabledTextColorProperty, "SfDatePickerDisabledTextColor");
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
            BaseHeaderView = HeaderView;
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
        /// Method triggers when closed the date picker popup.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnPopupClosed(EventArgs e)
        {
            if (IsScrollSelectionAllowed())
            {
                if (_internalSelectedDate != null)
                {
                    _internalSelectedDate = null;
                    UpdateSelectedIndex(SelectedDate);
                }
            }

            InvokeClosedEvent(this, e);
        }

        /// <summary>
        /// Method triggers when closing the date picker popup.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnPopupClosing(CancelEventArgs e)
        {
            InvokeClosingEvent(this, e);
        }

        /// <summary>
        /// Method triggers when opened the date picker popup.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnPopupOpened(EventArgs e)
        {
            InvokeOpenedEvent(this, e);
        }

        /// <summary>
        /// Method triggers when clicked the date picker ok button.
        /// </summary>
        /// <param name="e">The event arguments</param>
        protected override void OnOkButtonClicked(EventArgs e)
        {
            // If the picker is not in Default mode and an internal selected date exists
            if (IsScrollSelectionAllowed() && _internalSelectedDate != null)
            {
                // If the internal selected date is different from the currently selected date
                if (!DatePickerHelper.IsSameDate(_internalSelectedDate, SelectedDate))
                {
                    // Update the selected date with the internal selected date
                    SelectedDate = _internalSelectedDate.Value;
                    // Clear the internal selected date after applying it
                    _internalSelectedDate = null;
                }
            }

            InvokeOkButtonClickedEvent(this, e);
            if (AcceptCommand != null && AcceptCommand.CanExecute(e))
            {
                AcceptCommand.Execute(e);
            }

            IsOpen = false;
        }

        /// <summary>
        /// Method triggers when clicked the date picker cancel button.
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
                    // If no date is selected, clear all column selections
                    _yearColumn.SelectedItem = null;
                    _monthColumn.SelectedItem = null;
                    _dayColumn.SelectedItem = null;
                }
                // Clear the internal selected date if it exists
                if (_internalSelectedDate != null)
                {
                    _internalSelectedDate = null;
                }
            }

            InvokeCancelButtonClickedEvent(this, e);
            if (DeclineCommand != null && DeclineCommand.CanExecute(e))
            {
                DeclineCommand.Execute(e);
            }

            IsOpen = false;
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
            SfDatePicker? picker = bindable as SfDatePicker;
            if (picker == null)
            {
                return;
            }

            picker.HeaderView.Parent = picker;
            picker.BaseHeaderView = picker.HeaderView;
            if (bindable is SfDatePicker datePicker)
            {
                datePicker.SetParent(oldValue as Element, newValue as Element);
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
            SfDatePicker? picker = bindable as SfDatePicker;
            if (picker == null)
            {
                return;
            }

            if (oldValue is DatePickerColumnHeaderView oldStyle)
            {
                oldStyle.PickerPropertyChanged -= picker.OnColumnHeaderPropertyChanged;
                oldStyle.BindingContext = null;
                oldStyle.Parent = null;
            }

            if (newValue is DatePickerColumnHeaderView newStyle)
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

                if (picker._dayColumn != null)
                {
                    picker._dayColumn.HeaderText = SfPickerResources.GetLocalizedString(picker.ColumnHeaderView.DayHeaderText);
                }

                if (picker._monthColumn != null)
                {
                    picker._monthColumn.HeaderText = SfPickerResources.GetLocalizedString(picker.ColumnHeaderView.MonthHeaderText);
                }

                if (picker._yearColumn != null)
                {
                    picker._yearColumn.HeaderText = SfPickerResources.GetLocalizedString(picker.ColumnHeaderView.YearHeaderText);
                }
            }
        }

        /// <summary>
        /// Method invokes on selection date property changed.
        /// </summary>
        /// <param name="bindable">The SfDatePicker object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnSelectedDatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfDatePicker? datepicker = bindable as SfDatePicker;
            if (datepicker == null)
            {
                return;
            }

            DateTime? previousSelectedDate = null;
            DateTime? currentSelectedDate = null;
            if (oldValue is DateTime oldSelectedDate)
            {
                previousSelectedDate = oldSelectedDate;
                //// Prevents Selection changed event from triggering if old value is black out date.
                if (datepicker.BlackoutDates.Any(blackOutDate => DatePickerHelper.IsBlackoutDate(true, string.Empty, blackOutDate, previousSelectedDate.Value)))
                {
                    datepicker.UpdateSelectedIndex((DateTime)newValue);
                    //// Skip the update and event call by checking if the date is blackout value and within current month.
                    if (oldSelectedDate.Year == datepicker._previousSelectedDateTime.Year && oldSelectedDate.Month == datepicker._previousSelectedDateTime.Month)
                    {
                        return;
                    }

                    previousSelectedDate = datepicker._previousSelectedDateTime;
                }

                datepicker._previousSelectedDateTime = oldSelectedDate;
            }

            if (newValue is DateTime newSelectedDate)
            {
                //// Prevents Selection changed event from triggering if new value is black out date.
                if (datepicker.BlackoutDates.Any(blackOutDate => DatePickerHelper.IsBlackoutDate(true, string.Empty, blackOutDate, newSelectedDate)))
                {
                    return;
                }

                currentSelectedDate = newSelectedDate;
            }

            //// Skip the update and event call while the same date updated with different time value.
            if (DatePickerHelper.IsSameDate(previousSelectedDate, currentSelectedDate))
            {
                return;
            }

            //// Update the column with valid date(date between min and max date).
            if (newValue == null)
            {
                datepicker._dayColumn.SelectedItem = null;
                datepicker._yearColumn.SelectedItem = null;
                datepicker._monthColumn.SelectedItem = null;
                datepicker.SelectedDate = null;
                datepicker.SelectionChanged?.Invoke(datepicker, new DatePickerSelectionChangedEventArgs() { OldValue = previousSelectedDate, NewValue = currentSelectedDate });
                return;
            }
            else
            {
                datepicker.UpdateSelectedItemInColumn();
                PickerContainer? pickerContainer = datepicker.GetPickerContainerValue();
                pickerContainer?.UpdateScrollViewDraw();
                pickerContainer?.InvalidateDrawable();
            }

            currentSelectedDate = DatePickerHelper.GetValidDate(currentSelectedDate, datepicker.MinimumDate, datepicker.MaximumDate);
            var datePickerSelectionChangedEventArgs = new DatePickerSelectionChangedEventArgs() { OldValue = previousSelectedDate, NewValue = currentSelectedDate };
            if (datepicker.SelectionChanged != null)
            {
                datepicker.SelectionChanged?.Invoke(datepicker, datePickerSelectionChangedEventArgs);
            }

            if (datepicker.SelectionChangedCommand != null && datepicker.SelectionChangedCommand.CanExecute(datePickerSelectionChangedEventArgs))
            {
                datepicker.SelectionChangedCommand.Execute(datePickerSelectionChangedEventArgs);
            }

            datepicker.UpdateSelectedIndex(currentSelectedDate);
        }

        /// <summary>
        /// Method invokes on format property changed.
        /// </summary>
        /// <param name="bindable">The SfDatePicker object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnFormatPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfDatePicker? datepicker = bindable as SfDatePicker;
            if (datepicker == null)
            {
                return;
            }

            datepicker.UpdateFormat();
        }

        /// <summary>
        /// Method invokes on minimum date property changed.
        /// </summary>
        /// <param name="bindable">The SfDatePicker object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnMinimumDatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfDatePicker? datepicker = bindable as SfDatePicker;
            if (datepicker == null)
            {
                return;
            }

            datepicker.UpdateMinimumMaximumDate(oldValue, newValue);
            DateTime? currentSelectedDate = DatePickerHelper.GetValidDate(datepicker.SelectedDate, datepicker.MinimumDate, datepicker.MaximumDate);
            datepicker.UpdateSelectedIndex(currentSelectedDate);
        }

        /// <summary>
        /// Method invokes on maximum date property changed.
        /// </summary>
        /// <param name="bindable">The SfDatePicker object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnMaximumDatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfDatePicker? datepicker = bindable as SfDatePicker;
            if (datepicker == null)
            {
                return;
            }

            DateTime newDate = DatePickerHelper.GetValidMaxDate(datepicker.MinimumDate, (DateTime)newValue);
            datepicker.UpdateMinimumMaximumDate(oldValue, newDate);
            DateTime? currentSelectedDate = DatePickerHelper.GetValidDate(datepicker.SelectedDate, datepicker.MinimumDate, newDate);
            datepicker.UpdateSelectedIndex(currentSelectedDate);
        }

        /// <summary>
        /// Method invokes on day interval property changed.
        /// </summary>
        /// <param name="bindable">The SfDatePicker object</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnDayIntervalPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfDatePicker? datepicker = bindable as SfDatePicker;
            if (datepicker == null || (int)newValue <= 0)
            {
                return;
            }

            string dayFormat;
            //// Get the day format and format string order.
            List<int> formatStringOrder = DatePickerHelper.GetFormatStringOrder(out dayFormat, out _, datepicker.Format);
            if (string.IsNullOrEmpty(dayFormat))
            {
                return;
            }

            DateTime maxDate = DatePickerHelper.GetValidMaxDate(datepicker.MinimumDate, datepicker.MaximumDate);
            DateTime? currentSelectedDate = DatePickerHelper.GetValidDate(datepicker.SelectedDate, datepicker.MinimumDate, maxDate);
            datepicker._dayColumn = datepicker.GenerateDayColumn(dayFormat, currentSelectedDate);
            int dayIndex = formatStringOrder.IndexOf(0);
            //// Replace the day column with updated day interval.
            datepicker._columns[dayIndex] = datepicker._dayColumn;
        }

        /// <summary>
        /// Method invokes on month interval property changed.
        /// </summary>
        /// <param name="bindable">The SfDatePicker object</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnMonthIntervalPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfDatePicker? datepicker = bindable as SfDatePicker;
            if (datepicker == null || (int)newValue <= 0)
            {
                return;
            }

            string monthFormat;
            //// Get the day format and format string order.
            List<int> formatStringOrder = DatePickerHelper.GetFormatStringOrder(out _, out monthFormat, datepicker.Format);
            if (string.IsNullOrEmpty(monthFormat))
            {
                return;
            }

            DateTime maxDate = DatePickerHelper.GetValidMaxDate(datepicker.MinimumDate, datepicker.MaximumDate);
            DateTime? currentSelectedDate = DatePickerHelper.GetValidDate(datepicker.SelectedDate, datepicker.MinimumDate, maxDate);
            datepicker._monthColumn = datepicker.GenerateMonthColumn(monthFormat, currentSelectedDate);
            int monthIndex = formatStringOrder.IndexOf(1);
            //// Replace the month column with updated month interval.
            datepicker._columns[monthIndex] = datepicker._monthColumn;
        }

        /// <summary>
        /// Method invokes on year interval property changed.
        /// </summary>
        /// <param name="bindable">The SfDatePicker object</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnYearIntervalPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfDatePicker? datepicker = bindable as SfDatePicker;
            if (datepicker == null || (int)newValue <= 0)
            {
                return;
            }

            //// Get the day format and format string order.
            List<int> formatStringOrder = DatePickerHelper.GetFormatStringOrder(out _, out _, datepicker.Format);
            int yearIndex = formatStringOrder.IndexOf(2);
            if (yearIndex == -1)
            {
                return;
            }

            DateTime maxDate = DatePickerHelper.GetValidMaxDate(datepicker.MinimumDate, datepicker.MaximumDate);
            DateTime? currentSelectedDate = DatePickerHelper.GetValidDate(datepicker.SelectedDate, datepicker.MinimumDate, maxDate);
            datepicker._yearColumn = datepicker.GenerateYearColumn(currentSelectedDate);
            //// Replace the year column with updated year interval.
            datepicker._columns[yearIndex] = datepicker._yearColumn;
        }

        /// <summary>
        /// Method invokes on blackout dates property changed.
        /// </summary>
        /// <param name="bindable">The sfdatepicker object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnBlackOutDatesPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfDatePicker? datepicker = bindable as SfDatePicker;
            if (datepicker == null)
            {
                return;
            }

            //// Unwires collection changed from old and wires on new collection.
            ((ObservableCollection<DateTime>)oldValue).CollectionChanged -= datepicker.OnBlackoutDates_CollectionChanged;
            ((ObservableCollection<DateTime>)newValue).CollectionChanged += datepicker.OnBlackoutDates_CollectionChanged;

            if (datepicker.SelectedDate != null)
            {
                DateTime currentDate = datepicker.SelectedDate.Value;
                while (datepicker.BlackoutDates.Any(blackOutDate => DatePickerHelper.IsBlackoutDate(true, string.Empty, blackOutDate, currentDate)))
                {
                    currentDate = currentDate.AddDays(1);
                }

                if (datepicker.SelectedDate != currentDate)
                {
                    datepicker.SelectedDate = currentDate;
                }
            }

            //// Gets picker container value to update the view.
            PickerContainer? pickerContainer = datepicker.GetPickerContainerValue();

            pickerContainer?.UpdateScrollViewDraw();
            pickerContainer?.UpdatePickerSelectionView();
        }

        #endregion

        #region Internal Property Changed Methods

        /// <summary>
        /// called when <see cref="DatePickerBackground"/> property changed.
        /// </summary>
        /// <param name="bindable">The bindable object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        static void OnDatePickerBackgroundChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfDatePicker? picker = bindable as SfDatePicker;
            if (picker == null)
            {
                return;
            }

            picker.BackgroundColor = picker.DatePickerBackground;
        }

        /// <summary>
        /// Method invokes on the picker header text color changed.
        /// </summary>
        /// <param name="bindable">The header text style object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnHeaderTextColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfDatePicker? picker = bindable as SfDatePicker;
            if (picker == null)
            {
                return;
            }

            picker.HeaderView.TextStyle.TextColor = picker.HeaderTextColor;
        }

        /// <summary>
        /// Method invokes on the picker header font size changed.
        /// </summary>
        /// <param name="bindable">The header text style object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnHeaderFontSizeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfDatePicker? picker = bindable as SfDatePicker;
            if (picker == null)
            {
                return;
            }

            picker.HeaderView.TextStyle.FontSize = picker.HeaderFontSize;
        }

        /// <summary>
        /// Method invokes on the picker footer text color changed.
        /// </summary>
        /// <param name="bindable">The footer text style object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnFooterTextColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfDatePicker? picker = bindable as SfDatePicker;
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
            SfDatePicker? picker = bindable as SfDatePicker;
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
            SfDatePicker? picker = bindable as SfDatePicker;
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
            SfDatePicker? picker = bindable as SfDatePicker;
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
            SfDatePicker? picker = bindable as SfDatePicker;
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
            SfDatePicker? picker = bindable as SfDatePicker;
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
            SfDatePicker? picker = bindable as SfDatePicker;
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
            return new SfDatePickerStyles();
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
        /// Occurs after the selected date is changed on SfDatePicker.
        /// </summary>
        public event EventHandler<DatePickerSelectionChangedEventArgs>? SelectionChanged;

        #endregion
    }
}