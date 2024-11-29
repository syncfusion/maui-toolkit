using Syncfusion.Maui.Toolkit.Themes;

namespace Syncfusion.Maui.Toolkit.Calendar
{
    /// <summary>
    /// Represents a class which is used to customize all the properties of header view of the SfCalendar.
    /// </summary>
    public class CalendarHeaderView : Element, IThemeElement
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
                typeof(CalendarHeaderView),
                35d,
                propertyChanged: OnHeightChanged);

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
                typeof(CalendarHeaderView),
                string.Empty,
                propertyChanged: OnTextFormatChanged);

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
                typeof(CalendarHeaderView),
                defaultValueCreator: bindable => new CalendarTextStyle()
                {
                    Parent = bindable as Element,
                    FontSize = CalendarFonts.GetHeaderTextSize()
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
                typeof(CalendarHeaderView),
                defaultValueCreator: bindable => Brush.Transparent,
                propertyChanged: OnBackgroundChanged);

        /// <summary>
        /// Identifies the <see cref="ShowNavigationArrows"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="ShowNavigationArrows"/> dependency property.
        /// </value>
        public static readonly BindableProperty ShowNavigationArrowsProperty =
            BindableProperty.Create(
                nameof(ShowNavigationArrows),
                typeof(bool),
                typeof(CalendarMonthView),
                true,
                propertyChanged: OnShowNavigationArrowChanged);

        #endregion

        #region Internal Bindable Properties

        /// <summary>
        /// Identifies the <see cref="DisabledNavigationArrowColor"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="DisabledNavigationArrowColor"/> dependency property.
        /// </value>
        internal static readonly BindableProperty DisabledNavigationArrowColorProperty =
            BindableProperty.Create(
                nameof(DisabledNavigationArrowColor),
                typeof(Color),
                typeof(CalendarHeaderView),
                defaultValueCreator: bindable => null,
                propertyChanged: OnTextStyleChanged);

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarHeaderView"/> class.
        /// </summary>
        public CalendarHeaderView()
        {
            ThemeElement.InitializeThemeResources(this, "SfCalendarTheme");
            SetDynamicResource(DisabledNavigationArrowColorProperty, "SfCalendarDisabledNavigationArrowColor");
            TextStyle.Parent = this;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the value to specify the height of header view on SfCalendar.
        /// </summary>
        /// <value> The default value of <see cref="CalendarHeaderView.Height"/> is 35. </value>
        /// <seealso cref="SfCalendar.HeaderView"/>
        /// <example>
        /// The following code demonstrates, how to use the Height property in the calendar
        /// <code Lang="C#"><![CDATA[
        /// this.Calendar.HeaderView.Height = 50;
        /// ]]></code>
        /// </example>
        public double Height
        {
            get { return (double)GetValue(HeightProperty); }
            set { SetValue(HeightProperty, value); }
        }

        /// <summary>
        /// Gets or sets the format of the header text in calendar.
        /// </summary>
        /// <value> The default value of <see cref="CalendarHeaderView.TextFormat"/> is "string.Empty". </value>
        /// <remarks>
        /// It will be applicable to all <see cref="SfCalendar.View"/>.
        /// If it is empty then the header text will render based on <see cref="SfCalendar.View"/>.
        /// </remarks>
        /// <seealso cref="SfCalendar.HeaderView"/>
        /// <example>
        /// The following code demonstrates, how to use the TextFormat property in the calendar
        /// <code Lang="C#"><![CDATA[
        /// this.Calendar.HeaderView.TextFormat = "MMM yy";
        /// ]]></code>
        /// </example>
        public string TextFormat
        {
            get { return (string)GetValue(TextFormatProperty); }
            set { SetValue(TextFormatProperty, value); }
        }

        /// <summary>
        /// Gets or sets the text style of the header text in SfCalendar.
        /// </summary>
        /// <seealso cref="SfCalendar.HeaderView"/>
        /// <example>
        /// The following code demonstrates, how to use the TextStyle property in the calendar
        /// <code Lang="C#"><![CDATA[
        ///  var headerTextStyle = new CalendarTextStyle()
        ///  {
        ///      TextColor = Colors.Blue,
        ///      FontSize = 14,
        ///  };
        /// this.Calendar.HeaderView.TextStyle = headerTextStyle;
        /// ]]></code>
        /// </example>
        public CalendarTextStyle TextStyle
        {
            get { return (CalendarTextStyle)GetValue(TextStyleProperty); }
            set { SetValue(TextStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the background of the header view in SfCalendar.
        /// </summary>
        /// <value>The Default value of <see cref="CalendarHeaderView.Background"/> is Transparent. </value>
        /// <example>
        /// The following code demonstrates, how to use the Background property in the calendar
        /// <code Lang="C#"><![CDATA[
        /// this.Calendar.HeaderView.Background = Colors.Grey;
        /// ]]></code>
        /// </example>
        public Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to displays the navigation arrows on the header view of the SfCalendar.
        /// </summary>
        /// <value>The Default value of <see cref="CalendarHeaderView.ShowNavigationArrows"/> is true. </value>
        /// <example>
        /// The following code demonstrates, how to use the ShowNavigationArrows property in the calendar
        /// <code Lang="C#"><![CDATA[
        /// this.Calendar.HeaderView.ShowNavigationArrows = true;
        /// ]]></code>
        /// </example>
        public bool ShowNavigationArrows
        {
            get { return (bool)GetValue(ShowNavigationArrowsProperty); }
            set { SetValue(ShowNavigationArrowsProperty, value); }
        }

        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets or sets the disable navigation arrow color in SfCalendar.
        /// </summary>
        internal Color DisabledNavigationArrowColor
        {
            get { return (Color)GetValue(DisabledNavigationArrowColorProperty); }
            set { SetValue(DisabledNavigationArrowColorProperty, value); }
        }

        #endregion

        #region Property Changed Methods

        /// <summary>
        /// Invokes on the calendar header height changed.
        /// </summary>
        /// <param name="bindable">The header settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnHeightChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as CalendarHeaderView)?.RaisePropertyChanged(nameof(Height));
        }

        /// <summary>
        /// Invokes on the calendar header text format changed.
        /// </summary>
        /// <param name="bindable">The header settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnTextFormatChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as CalendarHeaderView)?.RaisePropertyChanged(nameof(TextFormat));
        }

        /// <summary>
        /// Invokes on header text style property changed.
        /// </summary>
        /// <param name="bindable">The header settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnTextStyleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as CalendarHeaderView)?.RaisePropertyChanged(nameof(TextStyle), oldValue);
            if (bindable is CalendarHeaderView calendarHeaderView)
            {
                calendarHeaderView.SetParent(oldValue as Element, newValue as Element);
            }
        }

        /// <summary>
        /// Invokes on the calendar header background changed.
        /// </summary>
        /// <param name="bindable">The header settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnBackgroundChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as CalendarHeaderView)?.RaisePropertyChanged(nameof(Background));
        }

        /// <summary>
        /// Method invokes on show navigation arrow changed.
        /// </summary>
        /// <param name="bindable">The header settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnShowNavigationArrowChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as CalendarHeaderView)?.RaisePropertyChanged(nameof(ShowNavigationArrows), oldValue);
        }

        /// <summary>
        /// Method to invoke calendar property changed event on header settings properties changed.
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
        /// Event Invokes on header settings property changed and this includes old value of the changed property which is used to unwire events for nested classes.
        /// </summary>
        internal event EventHandler<CalendarPropertyChangedEventArgs>? CalendarPropertyChanged;

        #endregion
    }
}