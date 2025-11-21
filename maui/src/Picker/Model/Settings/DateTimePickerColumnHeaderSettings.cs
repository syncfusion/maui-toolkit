using Syncfusion.Maui.Toolkit.Graphics.Internals;
using Syncfusion.Maui.Toolkit.Themes;
using ITextElement = Syncfusion.Maui.Toolkit.Graphics.Internals.ITextElement;

namespace Syncfusion.Maui.Toolkit.Picker
{
    /// <summary>
    /// Represents a class which is used to customize all the properties of column header view of the SfDateTimePicker.
    /// </summary>
    public class DateTimePickerColumnHeaderView : Element, IThemeElement
    {
        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="Height"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Height"/> dependency property.
        /// </value>
        public static readonly BindableProperty HeightProperty =
            BindableProperty.Create(
                nameof(Height),
                typeof(double),
                typeof(DateTimePickerColumnHeaderView),
                40d,
                propertyChanged: OnHeightChanged);

        /// <summary>
        /// Identifies the <see cref="TextStyle"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="TextStyle"/> dependency property.
        /// </value>
        public static readonly BindableProperty TextStyleProperty =
            BindableProperty.Create(
                nameof(TextStyle),
                typeof(PickerTextStyle),
                typeof(DateTimePickerColumnHeaderView),
                defaultValueCreator: bindable => GetColumnHeaderTextStyle(),
                propertyChanged: OnTextStyleChanged);

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
                typeof(DateTimePickerColumnHeaderView),
                defaultValueCreator: bindable => new SolidColorBrush(Color.FromArgb("#F7F2FB")),
                propertyChanged: OnBackgroundChanged);

        /// <summary>
        /// Identifies the <see cref="DividerColor"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="DividerColor"/> dependency property.
        /// </value>
        public static readonly BindableProperty DividerColorProperty =
            BindableProperty.Create(
                nameof(DividerColor),
                typeof(Color),
                typeof(DateTimePickerColumnHeaderView),
                defaultValueCreator: bindable => Color.FromArgb("#CAC4D0"),
                propertyChanged: OnDividerColorChanged);

        /// <summary>
        /// Identifies the <see cref="DayHeaderText"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="DayHeaderText"/> dependency property.
        /// </value>
        public static readonly BindableProperty DayHeaderTextProperty =
            BindableProperty.Create(
                nameof(DayHeaderText),
                typeof(string),
                typeof(DateTimePickerColumnHeaderView),
                "Day",
                propertyChanged: OnDayHeaderTextChanged);

        /// <summary>
        /// Identifies the <see cref="MonthHeaderText"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="MonthHeaderText"/> dependency property.
        /// </value>
        public static readonly BindableProperty MonthHeaderTextProperty =
            BindableProperty.Create(
                nameof(MonthHeaderText),
                typeof(string),
                typeof(DateTimePickerColumnHeaderView),
                "Month",
                propertyChanged: OnMonthHeaderTextChanged);

        /// <summary>
        /// Identifies the <see cref="YearHeaderText"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="YearHeaderText"/> dependency property.
        /// </value>
        public static readonly BindableProperty YearHeaderTextProperty =
            BindableProperty.Create(
                nameof(YearHeaderText),
                typeof(string),
                typeof(DateTimePickerColumnHeaderView),
                "Year",
                propertyChanged: OnYearHeaderTextChanged);

        /// <summary>
        /// Identifies the <see cref="HourHeaderText"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="HourHeaderText"/> dependency property.
        /// </value>
        public static readonly BindableProperty HourHeaderTextProperty =
            BindableProperty.Create(
                nameof(HourHeaderText),
                typeof(string),
                typeof(DateTimePickerColumnHeaderView),
                "Hour",
                propertyChanged: OnHourHeaderTextChanged);

        /// <summary>
        /// Identifies the <see cref="MinuteHeaderText"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="MinuteHeaderText"/> dependency property.
        /// </value>
        public static readonly BindableProperty MinuteHeaderTextProperty =
            BindableProperty.Create(
                nameof(MinuteHeaderText),
                typeof(string),
                typeof(DateTimePickerColumnHeaderView),
                "Minute",
                propertyChanged: OnMinuteHeaderTextChanged);

        /// <summary>
        /// Identifies the <see cref="SecondHeaderText"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="SecondHeaderText"/> dependency property.
        /// </value>
        public static readonly BindableProperty SecondHeaderTextProperty =
            BindableProperty.Create(
                nameof(SecondHeaderText),
                typeof(string),
                typeof(DateTimePickerColumnHeaderView),
                "Second",
                propertyChanged: OnSecondHeaderTextChanged);

        /// <summary>
        /// Identifies the <see cref="MilliSecondHeaderText"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="MilliSecondHeaderText"/> dependency property.
        /// </value>
        public static readonly BindableProperty MilliSecondHeaderTextProperty =
            BindableProperty.Create(
                nameof(MilliSecondHeaderText),
                typeof(string),
                typeof(DateTimePickerColumnHeaderView),
                "MilliSecond",
                propertyChanged: OnMilliSecondHeaderTextChanged);

        /// <summary>
        /// Identifies the <see cref="MeridiemHeaderText"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="MeridiemHeaderText"/> dependency property.
        /// </value>
        public static readonly BindableProperty MeridiemHeaderTextProperty =
            BindableProperty.Create(
                nameof(MeridiemHeaderText),
                typeof(string),
                typeof(DateTimePickerColumnHeaderView),
                string.Empty,
                propertyChanged: OnMeridiemHeaderTextChanged);

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimePickerColumnHeaderView"/> class.
        /// </summary>
        public DateTimePickerColumnHeaderView()
        {
            ThemeElement.InitializeThemeResources(this, "SfDateTimePickerTheme");
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the value to specify the height of column header view on SfDateTimePicker.
        /// </summary>
        /// <value>The default value of <see cref="DateTimePickerColumnHeaderView.Height"/> is 40d.</value>
        /// <example>
        /// The following example demonstrates how to set the height of the date time picker column header view.
        /// # [XAML](#tab/tabid-1)
        /// <code language="xaml">
        /// <![CDATA[
        /// <picker:SfDateTimePicker>
        ///     <picker:SfDateTimePicker.ColumnHeaderView>
        ///         <picker:DateTimePickerColumnHeaderView Height="50" />
        ///     </picker:SfDateTimePicker.ColumnHeaderView>
        /// </picker:SfDateTimePicker>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-2)
        /// <code language="C#">
        /// SfDateTimePicker dateTimePicker = new SfDateTimePicker();
        /// dateTimePicker.ColumnHeaderView = new DateTimePickerColumnHeaderView
        /// {
        ///     Height = 50
        /// };
        /// </code>
        /// </example>
        public double Height
        {
            get { return (double)GetValue(HeightProperty); }
            set { SetValue(HeightProperty, value); }
        }

        /// <summary>
        /// Gets or sets the text style of the column header text in SfDateTimePicker.
        /// </summary>
        /// <example>
        /// The following example demonstrates how to set the text style of the date time picker column header view.
        /// # [XAML](#tab/tabid-3)
        /// <code language="xaml">
        /// <![CDATA[
        /// <picker:SfDateTimePicker>
        ///     <picker:SfDateTimePicker.ColumnHeaderView>
        ///         <picker:DateTimePickerColumnHeaderView>
        ///             <picker:DateTimePickerColumnHeaderView.TextStyle>
        ///                 <picker:PickerTextStyle TextColor="Blue" FontSize="16" />
        ///             </picker:DateTimePickerColumnHeaderView.TextStyle>
        ///         </picker:DateTimePickerColumnHeaderView>
        ///     </picker:SfDateTimePicker.ColumnHeaderView>
        /// </picker:SfDateTimePicker>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-4)
        /// <code language="C#">
        /// SfDateTimePicker dateTimePicker = new SfDateTimePicker();
        /// dateTimePicker.ColumnHeaderView = new DateTimePickerColumnHeaderView
        /// {
        ///     TextStyle = new PickerTextStyle
        ///     {
        ///         TextColor = Colors.Blue,
        ///         FontSize = 16
        ///     }
        /// };
        /// </code>
        /// </example>
        public PickerTextStyle TextStyle
        {
            get { return (PickerTextStyle)GetValue(TextStyleProperty); }
            set { SetValue(TextStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the background of the column header view in SfDateTimePicker.
        /// </summary>
        /// <value>The default value of <see cref="DateTimePickerColumnHeaderView.Background"/> is "#F7F2FB".</value>
        /// <example>
        /// The following example demonstrates how to set the background of the date time picker column header view.
        /// # [XAML](#tab/tabid-5)
        /// <code language="xaml">
        /// <![CDATA[
        /// <picker:SfDateTimePicker>
        ///     <picker:SfDateTimePicker.ColumnHeaderView>
        ///         <picker:DateTimePickerColumnHeaderView Background="#E0E0E0" />
        ///     </picker:SfDateTimePicker.ColumnHeaderView>
        /// </picker:SfDateTimePicker>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-6)
        /// <code language="C#">
        /// SfDateTimePicker dateTimePicker = new SfDateTimePicker();
        /// dateTimePicker.ColumnHeaderView = new DateTimePickerColumnHeaderView
        /// {
        ///     Background = new SolidColorBrush(Color.FromHex("#E0E0E0"))
        /// };
        /// </code>
        /// </example>
        public Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets the background of the column header separator line background in SfDateTimePicker.
        /// </summary>
        /// <value>The default value of <see cref="DateTimePickerColumnHeaderView.DividerColor"/> is "#CAC4D0".</value>
        /// <example>
        /// The following example demonstrates how to set the divider color of the date time picker column header view.
        /// # [XAML](#tab/tabid-7)
        /// <code language="xaml">
        /// <![CDATA[
        /// <picker:SfDateTimePicker>
        ///     <picker:SfDateTimePicker.ColumnHeaderView>
        ///         <picker:DateTimePickerColumnHeaderView DividerColor="Gray" />
        ///     </picker:SfDateTimePicker.ColumnHeaderView>
        /// </picker:SfDateTimePicker>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-8)
        /// <code language="C#">
        /// SfDateTimePicker dateTimePicker = new SfDateTimePicker();
        /// dateTimePicker.ColumnHeaderView = new DateTimePickerColumnHeaderView
        /// {
        ///     DividerColor = Colors.Gray
        /// };
        /// </code>
        /// </example>
        public Color DividerColor
        {
            get { return (Color)GetValue(DividerColorProperty); }
            set { SetValue(DividerColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value to day header text in SfDateTimePicker.
        /// </summary>
        /// <value>The default value of <see cref="DateTimePickerColumnHeaderView.DayHeaderText"/> is "Day".</value>
        /// <example>
        /// The following example demonstrates how to set the day header text of the date time picker column header view.
        /// # [XAML](#tab/tabid-9)
        /// <code language="xaml">
        /// <![CDATA[
        /// <picker:SfDateTimePicker>
        ///     <picker:SfDateTimePicker.ColumnHeaderView>
        ///         <picker:DateTimePickerColumnHeaderView DayHeaderText="Day" />
        ///     </picker:SfDateTimePicker.ColumnHeaderView>
        /// </picker:SfDateTimePicker>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-10)
        /// <code language="C#">
        /// SfDateTimePicker dateTimePicker = new SfDateTimePicker();
        /// dateTimePicker.ColumnHeaderView = new DateTimePickerColumnHeaderView
        /// {
        ///     DayHeaderText = "Day"
        /// };
        /// </code>
        /// </example>
        public string DayHeaderText
        {
            get { return (string)GetValue(DayHeaderTextProperty); }
            set { SetValue(DayHeaderTextProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value to month header text in SfDateTimePicker.
        /// </summary>
        /// <value>The default value of <see cref="DateTimePickerColumnHeaderView.MonthHeaderText"/> is "Month".</value>
        /// <example>
        /// The following example demonstrates how to set the month header text of the date time picker column header view.
        /// # [XAML](#tab/tabid-11)
        /// <code language="xaml">
        /// <![CDATA[
        /// <picker:SfDateTimePicker>
        ///     <picker:SfDateTimePicker.ColumnHeaderView>
        ///         <picker:DateTimePickerColumnHeaderView MonthHeaderText="Month" />
        ///     </picker:SfDateTimePicker.ColumnHeaderView>
        /// </picker:SfDateTimePicker>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-12)
        /// <code language="C#">
        /// SfDateTimePicker dateTimePicker = new SfDateTimePicker();
        /// dateTimePicker.ColumnHeaderView = new DateTimePickerColumnHeaderView
        /// {
        ///     MonthHeaderText = "Month"
        /// };
        /// </code>
        /// </example>
        public string MonthHeaderText
        {
            get { return (string)GetValue(MonthHeaderTextProperty); }
            set { SetValue(MonthHeaderTextProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value to year header text in SfDateTimePicker.
        /// </summary>
        /// <value>The default value of <see cref="DateTimePickerColumnHeaderView.YearHeaderText"/> is "Year".</value>
        /// <example>
        /// The following example demonstrates how to set the year header text of the date time picker column header view.
        /// # [XAML](#tab/tabid-13)
        /// <code language="xaml">
        /// <![CDATA[
        /// <picker:SfDateTimePicker>
        ///     <picker:SfDateTimePicker.ColumnHeaderView>
        ///         <picker:DateTimePickerColumnHeaderView YearHeaderText="Year" />
        ///     </picker:SfDateTimePicker.ColumnHeaderView>
        /// </picker:SfDateTimePicker>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-14)
        /// <code language="C#">
        /// SfDateTimePicker dateTimePicker = new SfDateTimePicker();
        /// dateTimePicker.ColumnHeaderView = new DateTimePickerColumnHeaderView
        /// {
        ///     YearHeaderText = "Year"
        /// };
        /// </code>
        /// </example>
        public string YearHeaderText
        {
            get { return (string)GetValue(YearHeaderTextProperty); }
            set { SetValue(YearHeaderTextProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value to hour header text in SfDateTimePicker.
        /// </summary>
        /// <value>The default value of <see cref="DateTimePickerColumnHeaderView.HourHeaderText"/> is "Hour".</value>
        /// <example>
        /// The following example demonstrates how to set the hour header text of the date time picker column header view.
        /// # [XAML](#tab/tabid-15)
        /// <code language="xaml">
        /// <![CDATA[
        /// <picker:SfDateTimePicker>
        ///     <picker:SfDateTimePicker.ColumnHeaderView>
        ///         <picker:DateTimePickerColumnHeaderView HourHeaderText="Hour" />
        ///     </picker:SfDateTimePicker.ColumnHeaderView>
        /// </picker:SfDateTimePicker>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-16)
        /// <code language="C#">
        /// SfDateTimePicker dateTimePicker = new SfDateTimePicker();
        /// dateTimePicker.ColumnHeaderView = new DateTimePickerColumnHeaderView
        /// {
        ///     HourHeaderText = "Hour"
        /// };
        /// </code>
        /// </example>
        public string HourHeaderText
        {
            get { return (string)GetValue(HourHeaderTextProperty); }
            set { SetValue(HourHeaderTextProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value to minute header text in SfDateTimePicker.
        /// </summary>
        /// <value>The default value of <see cref="DateTimePickerColumnHeaderView.MinuteHeaderText"/> is "Minute".</value>
        /// <example>
        /// The following example demonstrates how to set the minute header text of the date time picker column header view.
        /// # [XAML](#tab/tabid-17)
        /// <code language="xaml">
        /// <![CDATA[
        /// <picker:SfDateTimePicker>
        ///     <picker:SfDateTimePicker.ColumnHeaderView>
        ///         <picker:DateTimePickerColumnHeaderView MinuteHeaderText="Minute" />
        ///     </picker:SfDateTimePicker.ColumnHeaderView>
        /// </picker:SfDateTimePicker>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-18)
        /// <code language="C#">
        /// SfDateTimePicker dateTimePicker = new SfDateTimePicker();
        /// dateTimePicker.ColumnHeaderView = new DateTimePickerColumnHeaderView
        /// {
        ///     MinuteHeaderText = "Minute"
        /// };
        /// </code>
        /// </example>
        public string MinuteHeaderText
        {
            get { return (string)GetValue(MinuteHeaderTextProperty); }
            set { SetValue(MinuteHeaderTextProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value to second header text in SfDateTimePicker.
        /// </summary>
        /// <value>The default value of <see cref="DateTimePickerColumnHeaderView.SecondHeaderText"/> is "Second".</value>
        /// <example>
        /// The following example demonstrates how to set the second header text of the date time picker column header view.
        /// # [XAML](#tab/tabid-19)
        /// <code language="xaml">
        /// <![CDATA[
        /// <picker:SfDateTimePicker>
        ///     <picker:SfDateTimePicker.ColumnHeaderView>
        ///         <picker:DateTimePickerColumnHeaderView SecondHeaderText="Second" />
        ///     </picker:SfDateTimePicker.ColumnHeaderView>
        /// </picker:SfDateTimePicker>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-20)
        /// <code language="C#">
        /// SfDateTimePicker dateTimePicker = new SfDateTimePicker();
        /// dateTimePicker.ColumnHeaderView = new DateTimePickerColumnHeaderView
        /// {
        ///     SecondHeaderText = "Second"
        /// };
        /// </code>
        /// </example>
        public string SecondHeaderText
        {
            get { return (string)GetValue(SecondHeaderTextProperty); }
            set { SetValue(SecondHeaderTextProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value to millisecond header text in SfDateTimePicker.
        /// </summary>
        /// <value>The default value of <see cref="DateTimePickerColumnHeaderView.MilliSecondHeaderText"/> is "MilliSecond".</value>
        /// <example>
        /// The following example demonstrates how to set the millisecond header text of the date time picker column header view.
        /// # [XAML](#tab/tabid-19)
        /// <code language="xaml">
        /// <![CDATA[
        /// <picker:SfDateTimePicker>
        ///     <picker:SfDateTimePicker.ColumnHeaderView>
        ///         <picker:DateTimePickerColumnHeaderView MilliSecondHeaderText="MilliSecond" />
        ///     </picker:SfDateTimePicker.ColumnHeaderView>
        /// </picker:SfDateTimePicker>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-20)
        /// <code language="C#">
        /// SfDateTimePicker dateTimePicker = new SfDateTimePicker();
        /// dateTimePicker.ColumnHeaderView = new DateTimePickerColumnHeaderView
        /// {
        ///     MilliSecondHeaderText = "Second"
        /// };
        /// </code>
        /// </example>
        public string MilliSecondHeaderText
        {
            get { return (string)GetValue(MilliSecondHeaderTextProperty); }
            set { SetValue(MilliSecondHeaderTextProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value to meridiem header text in SfDateTimePicker.
        /// </summary>
        /// <value>The default value of <see cref="DateTimePickerColumnHeaderView.MeridiemHeaderText"/> is an string.empty.</value>
        /// <example>
        /// The following example demonstrates how to set the meridiem header text of the date time picker column header view.
        /// # [XAML](#tab/tabid-21)
        /// <code language="xaml">
        /// <![CDATA[
        /// <picker:SfDateTimePicker>
        ///     <picker:SfDateTimePicker.ColumnHeaderView>
        ///         <picker:DateTimePickerColumnHeaderView MeridiemHeaderText="Meridiem" />
        ///     </picker:SfDateTimePicker.ColumnHeaderView>
        /// </picker:SfDateTimePicker>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-22)
        /// <code language="C#">
        /// SfDateTimePicker dateTimePicker = new SfDateTimePicker();
        /// dateTimePicker.ColumnHeaderView = new DateTimePickerColumnHeaderView
        /// {
        ///     MeridiemHeaderText = "Meridiem"
        /// };
        /// </code>
        /// </example>
        public string MeridiemHeaderText
        {
            get { return (string)GetValue(MeridiemHeaderTextProperty); }
            set { SetValue(MeridiemHeaderTextProperty, value); }
        }

        #endregion

        #region Property Changed Methods

        /// <summary>
        /// Method invokes on the picker column header height changed.
        /// </summary>
        /// <param name="bindable">The column header settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnHeightChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as DateTimePickerColumnHeaderView)?.RaisePropertyChanged(nameof(Height));
        }

        /// <summary>
        /// Method invokes on picker column header text style property changed.
        /// </summary>
        /// <param name="bindable">The column header settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnTextStyleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as DateTimePickerColumnHeaderView)?.RaisePropertyChanged(nameof(TextStyle), oldValue);
        }

        /// <summary>
        /// Method invokes on the picker column header background changed.
        /// </summary>
        /// <param name="bindable">The column header settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnBackgroundChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as DateTimePickerColumnHeaderView)?.RaisePropertyChanged(nameof(Background));
        }

        /// <summary>
        /// Method invokes on the picker column header separator line background changed.
        /// </summary>
        /// <param name="bindable">The column header settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnDividerColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as DateTimePickerColumnHeaderView)?.RaisePropertyChanged(nameof(DividerColor));
        }

        /// <summary>
        /// Method invokes on the day header text property changed.
        /// </summary>
        /// <param name="bindable">The column header view.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnDayHeaderTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as DateTimePickerColumnHeaderView)?.RaisePropertyChanged(nameof(DayHeaderText));
        }

        /// <summary>
        /// Method invokes on month header text property changed.
        /// </summary>
        /// <param name="bindable">The column header view.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnMonthHeaderTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as DateTimePickerColumnHeaderView)?.RaisePropertyChanged(nameof(MonthHeaderText));
        }

        /// <summary>
        /// Method invokes on year header text property changed.
        /// </summary>
        /// <param name="bindable">The column header view.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnYearHeaderTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as DateTimePickerColumnHeaderView)?.RaisePropertyChanged(nameof(YearHeaderText));
        }

        /// <summary>
        /// Method invokes on hour header text property changed.
        /// </summary>
        /// <param name="bindable">The column header view.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnHourHeaderTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as DateTimePickerColumnHeaderView)?.RaisePropertyChanged(nameof(HourHeaderText));
        }

        /// <summary>
        /// Method invokes on minute header text property changed.
        /// </summary>
        /// <param name="bindable">The column header view.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnMinuteHeaderTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as DateTimePickerColumnHeaderView)?.RaisePropertyChanged(nameof(MinuteHeaderText));
        }

        /// <summary>
        /// Method invokes on second header text property changed.
        /// </summary>
        /// <param name="bindable">The column header view.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnSecondHeaderTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as DateTimePickerColumnHeaderView)?.RaisePropertyChanged(nameof(SecondHeaderText));
        }

        /// <summary>
        /// Method invokes on millisecond header text property changed.
        /// </summary>
        /// <param name="bindable">The column header view.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnMilliSecondHeaderTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as DateTimePickerColumnHeaderView)?.RaisePropertyChanged(nameof(MilliSecondHeaderText));
        }

        /// <summary>
        /// Method invokes on meridiem header text property changed.
        /// </summary>
        /// <param name="bindable">The column header view.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnMeridiemHeaderTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as DateTimePickerColumnHeaderView)?.RaisePropertyChanged(nameof(MeridiemHeaderText));
        }

        /// <summary>
        /// Method to get the default text style for the column header view.
        /// </summary>
        /// <returns>Returns the default column header text style.</returns>
        static ITextElement GetColumnHeaderTextStyle()
        {
            return new PickerTextStyle()
            {
                FontSize = 14,
                TextColor = Color.FromArgb("#49454F"),
            };
        }

        /// <summary>
        /// Method to invoke picker property changed event on column header settings properties changed.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        /// <param name="oldValue">Property old value.</param>
        void RaisePropertyChanged(string propertyName, object? oldValue = null)
        {
            PickerPropertyChanged?.Invoke(this, new PickerPropertyChangedEventArgs(propertyName) { OldValue = oldValue });
        }

        #endregion

        #region Interface Implementation

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
        /// Event Invokes on column header settings property changed and this includes old value of the changed property which is used to unwire events for nested classes.
        /// </summary>
        internal event EventHandler<PickerPropertyChangedEventArgs>? PickerPropertyChanged;

        #endregion
    }
}