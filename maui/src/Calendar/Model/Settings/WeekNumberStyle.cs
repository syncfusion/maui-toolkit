using Syncfusion.Maui.Toolkit.Themes;

namespace Syncfusion.Maui.Toolkit.Calendar
{
    /// <summary>
    /// Represents a class which is used to customize the week number style on month view of the SfCalendar.
    /// </summary>
    public class CalendarWeekNumberStyle : Element, IThemeElement
    {
        #region Bindable Properties

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
                typeof(CalendarWeekNumberStyle),
                defaultValueCreator: bindable => Brush.Transparent,
                propertyChanged: OnBackgroundChanged);

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
                typeof(CalendarWeekNumberStyle),
                defaultValueCreator: bindable => new CalendarTextStyle()
                {
                    Parent = bindable as Element,
                    FontSize = CalendarFonts.GetMonthBodyTextSize()
                },
                propertyChanged: OnTextStyleChanged);

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarWeekNumberStyle"/> class.
        /// </summary>
        public CalendarWeekNumberStyle()
        {
            ThemeElement.InitializeThemeResources(this, "SfCalendarTheme");
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the background of the week number view in SfCalendar.
        /// </summary>
        /// <value>The default value of <see cref="CalendarWeekNumberStyle.Background"/> is "#2121210A". </value>
        /// <seealso cref="SfCalendar.MonthView"/>
        /// <seealso cref="CalendarMonthView.WeekNumberStyle"/>
        /// <seealso cref="CalendarMonthView.ShowWeekNumber"/>
        /// <example>
        /// The following code demonstrates, how to use the Background property in the calendar
        /// <code Lang="C#"><![CDATA[
        /// this.Calendar.MonthView.WeekNumberStyle.Background = Colors.Grey;
        /// ]]></code>
        /// </example>
        public Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets the text style of the week number text in SfCalendar.
        /// </summary>
        /// <seealso cref="SfCalendar.MonthView"/>
        /// <seealso cref="CalendarMonthView.WeekNumberStyle"/>
        /// <seealso cref="CalendarMonthView.ShowWeekNumber"/>
        /// <example>
        /// The following code demonstrates, how to use the TextStyle property in the calendar
        /// <code Lang="C#"><![CDATA[
        ///  var weekTextStyle = new CalendarTextStyle()
        ///  {
        ///      TextColor = Colors.Blue,
        ///      FontSize = 14,
        ///  };
        /// this.Calendar.MonthView.WeekNumberStyle.TextStyle = weekTextStyle;
        /// ]]></code>
        /// </example>
        public CalendarTextStyle TextStyle
        {
            get { return (CalendarTextStyle)GetValue(TextStyleProperty); }
            set { SetValue(TextStyleProperty, value); }
        }

        #endregion

        #region Property Changed Methods

        /// <summary>
        /// Invokes on background property changed.
        /// </summary>
        /// <param name="bindable">The Week number style object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        private static void OnBackgroundChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as CalendarWeekNumberStyle)?.RaisePropertyChanged(nameof(Background));
        }

        /// <summary>
        /// Invokes on text style property changed.
        /// </summary>
        /// <param name="bindable">The Week number style object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        private static void OnTextStyleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as CalendarWeekNumberStyle)?.RaisePropertyChanged(nameof(TextStyle), oldValue);
            if (bindable is CalendarWeekNumberStyle calendarWeekNumberStyle)
            {
                calendarWeekNumberStyle.SetParent(oldValue as Element, newValue as Element);
            }
        }

        /// <summary>
        /// Method to invoke calendar property changed event on week number style properties changed.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        /// <param name="oldValue">Property old value.</param>
        private void RaisePropertyChanged(string propertyName, object? oldValue = null)
        {
            this.CalendarPropertyChanged?.Invoke(this, new CalendarPropertyChangedEventArgs(propertyName) { OldValue = oldValue });
        }

        /// <summary>
        /// Need to update the parent for the new value.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private void SetParent(Element? oldValue, Element? newValue)
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
        /// Event Invokes on week number style property changed and this includes old value of the changed property which is used to unwire events for nested classes.
        /// </summary>
        internal event EventHandler<CalendarPropertyChangedEventArgs>? CalendarPropertyChanged;

        #endregion
    }
}