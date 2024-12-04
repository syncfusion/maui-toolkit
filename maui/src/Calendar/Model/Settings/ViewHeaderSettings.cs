using Syncfusion.Maui.Toolkit.Themes;

namespace Syncfusion.Maui.Toolkit.Calendar
{
    /// <summary>
    /// Represents a class which is used to customize all the properties of view header view of the SfCalendar.
    /// </summary>
    public class CalendarMonthHeaderView : Element, IThemeElement
    {
        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="TextFormat"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="TextFormat"/> dependency property.
        /// </value>
        public static readonly BindableProperty TextFormatProperty =
            BindableProperty.Create(
                nameof(TextFormat),
                typeof(string),
                typeof(CalendarMonthHeaderView),
                "ddddd",
                propertyChanged: OnTextFormatChanged);

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
                typeof(CalendarMonthHeaderView),
                30d,
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
                typeof(CalendarTextStyle),
                typeof(CalendarMonthHeaderView),
                defaultValueCreator: bindable => new CalendarTextStyle()
                {
                    Parent = bindable as Element,
                    FontSize = CalendarFonts.GetSecondaryHeaderTextSize(),
                    TextColor = CalendarColors.GetOnSecondaryVariantColor()
                },
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
                typeof(CalendarMonthHeaderView),
                defaultValueCreator: bindable => Brush.Transparent,
                propertyChanged: OnBackgroundChanged);

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarMonthHeaderView"/> class.
        /// </summary>
        public CalendarMonthHeaderView()
        {
            ThemeElement.InitializeThemeResources(this, "SfCalendarTheme");
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the format of the view header text in calendar month view.
        /// </summary>
        /// <value> The default value of <see cref="CalendarMonthHeaderView.TextFormat"/> is "ddddd". </value>
        /// <remarks>
        /// The format "ddddd" stands for showing two characters of the day of week text for Gregorian calendar. Other than Gregorian calendar, it shown like "dddd" format.
        /// </remarks>
        /// <seealso cref="SfCalendar.MonthView"/>
        /// <example>
        /// The following code demonstrates, how to use the TextFormat property in the calendar
        /// <code Lang="C#"><![CDATA[
        /// this.Calendar.MonthView.HeaderView.TextFormat = "ddddd";
        /// ]]></code>
        /// </example>
        public string TextFormat
        {
            get { return (string)GetValue(TextFormatProperty); }
            set { SetValue(TextFormatProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value to specify the height of view header view on SfCalendar.
        /// </summary>
        /// <value> The default value of <see cref="CalendarMonthHeaderView.Height"/> is 30d. </value>
        /// <seealso cref="SfCalendar.MonthView"/>
        /// <example>
        /// The following code demonstrates, how to use the TextFormat property in the calendar
        /// <code Lang="C#"><![CDATA[
        /// this.Calendar.MonthView.HeaderView.Height = "50";
        /// ]]></code>
        /// </example>
        public double Height
        {
            get { return (double)GetValue(HeightProperty); }
            set { SetValue(HeightProperty, value); }
        }

        /// <summary>
        /// Gets or sets the text style of the view header text in SfCalendar.
        /// </summary>
        /// <seealso cref="SfCalendar.MonthView"/>
        /// <example>
        /// The following code demonstrates, how to use the TextStyle property in the calendar
        /// <code Lang="C#"><![CDATA[
        ///  var headerTextStyle = new CalendarTextStyle()
        ///  {
        ///      TextColor = Colors.Blue,
        ///      FontSize = 14,
        ///  };
        /// this.Calendar.MonthView.HeaderView.TextStyle = headerTextStyle;
        /// ]]></code>
        /// </example>
        public CalendarTextStyle TextStyle
        {
            get { return (CalendarTextStyle)GetValue(TextStyleProperty); }
            set { SetValue(TextStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the background of the view header view in SfCalendar.
        /// </summary>
        /// <value>The default value of <see cref="CalendarMonthHeaderView.Background"/> is Transparent. </value>
        /// <seealso cref="SfCalendar.MonthView"/>
        /// <example>
        /// The following code demonstrates, how to use the Background property in the calendar
        /// <code Lang="C#"><![CDATA[
        /// this.Calendar.MonthView.HeaderView.Background = Colors.Yellow;
        /// ]]></code>
        /// </example>
        public Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        #endregion

        #region Property Changed Methods

        /// <summary>
        /// Invokes on the calendar view header text format changed.
        /// </summary>
        /// <param name="bindable">The view header settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnTextFormatChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as CalendarMonthHeaderView)?.RaisePropertyChanged(nameof(TextFormat));
        }

        /// <summary>
        /// Invokes on the calendar view header height changed.
        /// </summary>
        /// <param name="bindable">The view header settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnHeightChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as CalendarMonthHeaderView)?.RaisePropertyChanged(nameof(Height));
        }

        /// <summary>
        /// Invokes on view header text style property changed.
        /// </summary>
        /// <param name="bindable">The view header settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnTextStyleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as CalendarMonthHeaderView)?.RaisePropertyChanged(nameof(TextStyle), oldValue);
            if (bindable is CalendarMonthHeaderView calendarMonthHeaderView)
            {
                calendarMonthHeaderView.SetParent(oldValue as Element, newValue as Element);
            }
        }

        /// <summary>
        /// Invokes on the calendar view header background changed.
        /// </summary>
        /// <param name="bindable">The view header settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnBackgroundChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as CalendarMonthHeaderView)?.RaisePropertyChanged(nameof(Background));
        }

        /// <summary>
        /// Method to invoke calendar view header settings properties changed.
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
        /// Event Invokes on view header settings property changed and this includes old value of the changed property which is used to unwire events for nested classes.
        /// </summary>
        internal event EventHandler<CalendarPropertyChangedEventArgs>? CalendarPropertyChanged;

        #endregion
    }
}