using Syncfusion.Maui.Toolkit.Graphics.Internals;
using Syncfusion.Maui.Toolkit.Themes;
using ITextElement = Syncfusion.Maui.Toolkit.Graphics.Internals.ITextElement;

namespace Syncfusion.Maui.Toolkit.Picker
{
    /// <summary>
    /// Represents a class which is used to customize all the properties of column header view of the SfDatePicker.
    /// </summary>
    public class DatePickerColumnHeaderView : Element, IThemeElement
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
                typeof(DatePickerColumnHeaderView),
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
                typeof(DatePickerColumnHeaderView),
                defaultValueCreator: bindable => GetColumnHeaderTextStyle(bindable),
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
                typeof(DatePickerColumnHeaderView),
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
                typeof(DatePickerColumnHeaderView),
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
                typeof(DatePickerColumnHeaderView),
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
                typeof(DatePickerColumnHeaderView),
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
                typeof(DatePickerColumnHeaderView),
                "Year",
                propertyChanged: OnYearHeaderTextChanged);

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DatePickerColumnHeaderView"/> class.
        /// </summary>
        public DatePickerColumnHeaderView()
        {
            ThemeElement.InitializeThemeResources(this, "SfDatePickerTheme");
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the value to specify the height of column header view on SfDatePicker.
        /// </summary>
        /// <value>The default value of <see cref="DatePickerColumnHeaderView.Height"/> is 40d.</value>
        /// <example>
        /// The following example demonstrates how to set the height of the date picker column header view.
        /// # [XAML](#tab/tabid-1)
        /// <code language="xaml">
        /// <![CDATA[
        /// <picker:SfDatePicker>
        ///     <picker:SfDatePicker.ColumnHeaderView>
        ///         <picker:DatePickerColumnHeaderView Height="50" />
        ///     </picker:SfDatePicker.ColumnHeaderView>
        /// </picker:SfDatePicker>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-2)
        /// <code language="C#">
        /// SfDatePicker datePicker = new SfDatePicker();
        /// datePicker.ColumnHeaderView = new DatePickerColumnHeaderView
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
        /// Gets or sets the text style of the column header text in SfDatePicker.
        /// </summary>
        /// <example>
        /// The following example demonstrates how to set the text style of the date picker column header view.
        /// # [XAML](#tab/tabid-3)
        /// <code language="xaml">
        /// <![CDATA[
        /// <picker:SfDatePicker>
        ///     <picker:SfDatePicker.ColumnHeaderView>
        ///         <picker:DatePickerColumnHeaderView>
        ///             <picker:DatePickerColumnHeaderView.TextStyle>
        ///                 <picker:PickerTextStyle TextColor="Blue" FontSize="16" />
        ///             </picker:DatePickerColumnHeaderView.TextStyle>
        ///         </picker:DatePickerColumnHeaderView>
        ///     </picker:SfDatePicker.ColumnHeaderView>
        /// </picker:SfDatePicker>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-4)
        /// <code language="C#">
        /// SfDatePicker datePicker = new SfDatePicker();
        /// datePicker.ColumnHeaderView = new DatePickerColumnHeaderView
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
        /// Gets or sets the background of the column header view in SfDatePicker.
        /// </summary>
        /// <value>The default value of <see cref="DatePickerColumnHeaderView.Background"/> is "#F7F2FB".</value>
        /// <example>
        /// The following example demonstrates how to set the background of the date picker column header view.
        /// # [XAML](#tab/tabid-5)
        /// <code language="xaml">
        /// <![CDATA[
        /// <picker:SfDatePicker>
        ///     <picker:SfDatePicker.ColumnHeaderView>
        ///         <picker:DatePickerColumnHeaderView Background="#E0E0E0" />
        ///     </picker:SfDatePicker.ColumnHeaderView>
        /// </picker:SfDatePicker>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-6)
        /// <code language="C#">
        /// SfDatePicker datePicker = new SfDatePicker();
        /// datePicker.ColumnHeaderView = new DatePickerColumnHeaderView
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
        /// Gets or sets the background of the column header separator line background in SfDatePicker.
        /// </summary>
        /// <value>The default value of <see cref="DatePickerColumnHeaderView.DividerColor"/> is "#CAC4D0".</value>
        /// <example>
        /// The following example demonstrates how to set the divider color of the date picker column header view.
        /// # [XAML](#tab/tabid-7)
        /// <code language="xaml">
        /// <![CDATA[
        /// <picker:SfDatePicker>
        ///     <picker:SfDatePicker.ColumnHeaderView>
        ///         <picker:DatePickerColumnHeaderView DividerColor="Gray" />
        ///     </picker:SfDatePicker.ColumnHeaderView>
        /// </picker:SfDatePicker>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-8)
        /// <code language="C#">
        /// SfDatePicker datePicker = new SfDatePicker();
        /// datePicker.ColumnHeaderView = new DatePickerColumnHeaderView
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
        /// Gets or sets the value to day header text in SfDatePicker.
        /// </summary>
        /// <value>The default value of <see cref="DatePickerColumnHeaderView.DayHeaderText"/> is "Day".</value>
        /// <example>
        /// The following example demonstrates how to set the day header text of the date picker column header view.
        /// # [XAML](#tab/tabid-9)
        /// <code language="xaml">
        /// <![CDATA[
        /// <picker:SfDatePicker>
        ///     <picker:SfDatePicker.ColumnHeaderView>
        ///         <picker:DatePickerColumnHeaderView DayHeaderText="Date" />
        ///     </picker:SfDatePicker.ColumnHeaderView>
        /// </picker:SfDatePicker>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-10)
        /// <code language="C#">
        /// SfDatePicker datePicker = new SfDatePicker();
        /// datePicker.ColumnHeaderView = new DatePickerColumnHeaderView
        /// {
        ///     DayHeaderText = "Date"
        /// };
        /// </code>
        /// </example>
        public string DayHeaderText
        {
            get { return (string)GetValue(DayHeaderTextProperty); }
            set { SetValue(DayHeaderTextProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value to month header text in SfDatePicker.
        /// </summary>
        /// <value>The default value of <see cref="DatePickerColumnHeaderView.MonthHeaderText"/> is "Month".</value>
        /// <example>
        /// The following example demonstrates how to set the month header text of the date picker column header view.
        /// # [XAML](#tab/tabid-11)
        /// <code language="xaml">
        /// <![CDATA[
        /// <picker:SfDatePicker>
        ///     <picker:SfDatePicker.ColumnHeaderView>
        ///         <picker:DatePickerColumnHeaderView MonthHeaderText="Month" />
        ///     </picker:SfDatePicker.ColumnHeaderView>
        /// </picker:SfDatePicker>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-12)
        /// <code language="C#">
        /// SfDatePicker datePicker = new SfDatePicker();
        /// datePicker.ColumnHeaderView = new DatePickerColumnHeaderView
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
        /// Gets or sets the value to year header text in SfDatePicker.
        /// </summary>
        /// <value>The default value of <see cref="DatePickerColumnHeaderView.YearHeaderText"/> is "Year".</value>
        /// <example>
        /// The following example demonstrates how to set the year header text of the date picker column header view.
        /// # [XAML](#tab/tabid-13)
        /// <code language="xaml">
        /// <![CDATA[
        /// <picker:SfDatePicker>
        ///     <picker:SfDatePicker.ColumnHeaderView>
        ///         <picker:DatePickerColumnHeaderView YearHeaderText="Year" />
        ///     </picker:SfDatePicker.ColumnHeaderView>
        /// </picker:SfDatePicker>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-14)
        /// <code language="C#">
        /// SfDatePicker datePicker = new SfDatePicker();
        /// datePicker.ColumnHeaderView = new DatePickerColumnHeaderView
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
            (bindable as DatePickerColumnHeaderView)?.RaisePropertyChanged(nameof(Height));
        }

        /// <summary>
        /// Method invokes on picker column header text style property changed.
        /// </summary>
        /// <param name="bindable">The column header settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnTextStyleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as DatePickerColumnHeaderView)?.RaisePropertyChanged(nameof(TextStyle), oldValue);
        }

        /// <summary>
        /// Method invokes on the picker column header background changed.
        /// </summary>
        /// <param name="bindable">The column header settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnBackgroundChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as DatePickerColumnHeaderView)?.RaisePropertyChanged(nameof(Background));
        }

        /// <summary>
        /// Method invokes on the picker column header separator line background changed.
        /// </summary>
        /// <param name="bindable">The column header settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnDividerColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as DatePickerColumnHeaderView)?.RaisePropertyChanged(nameof(DividerColor));
        }

        /// <summary>
        /// Method invokes on is day header text property changed.
        /// </summary>
        /// <param name="bindable">The column header view.</param>
        /// <param name="oldValue">Property old value</param>
        /// <param name="newValue">Property new value</param>
        static void OnDayHeaderTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as DatePickerColumnHeaderView)?.RaisePropertyChanged(nameof(DayHeaderText));
        }

        /// <summary>
        /// Method invokes on month header text property changed.
        /// </summary>
        /// <param name="bindable">The column header view.</param>
        /// <param name="oldValue">Property old value</param>
        /// <param name="newValue">Property new value</param>
        static void OnMonthHeaderTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as DatePickerColumnHeaderView)?.RaisePropertyChanged(nameof(MonthHeaderText));
        }

        /// <summary>
        /// Method invokes on year header text property changed.
        /// </summary>
        /// <param name="bindable">The column header view.</param>
        /// <param name="oldValue">Property old value</param>
        /// <param name="newValue">Property new value</param>
        static void OnYearHeaderTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as DatePickerColumnHeaderView)?.RaisePropertyChanged(nameof(YearHeaderText));
        }

        /// <summary>
        /// Method to get the default text style for the column header view.
        /// </summary>
        /// <returns>Returns the default column header text style.</returns>
        static ITextElement GetColumnHeaderTextStyle(BindableObject bindable)
        {
            DatePickerColumnHeaderView columnHeaderView = (DatePickerColumnHeaderView)bindable;
            PickerTextStyle pickerTextStyle = new PickerTextStyle()
            {
                FontSize = 14,
                TextColor = Color.FromArgb("#49454F"),
                Parent = columnHeaderView,
            };

            return pickerTextStyle;
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