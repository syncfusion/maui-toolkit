using Syncfusion.Maui.Toolkit.Themes;

namespace Syncfusion.Maui.Toolkit.Calendar
{
    /// <summary>
    /// Represents a class which is used to customize all the properties of footer view of the SfCalendar.
    /// </summary>
    public class CalendarFooterView : Element, IThemeElement
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
                typeof(CalendarFooterView),
                50d,
                propertyChanged: OnHeightChanged);

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
                typeof(CalendarFooterView),
                defaultValueCreator: bindable => Brush.Transparent,
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
                typeof(CalendarFooterView),
                defaultValueCreator: bindable => Color.FromArgb("#CAC4D0"),
                propertyChanged: OnDividerColorChanged);

        /// <summary>
        /// Identifies the <see cref="ShowActionButtons"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="ShowActionButtons"/> dependency property.
        /// </value>
        public static readonly BindableProperty ShowActionButtonsProperty =
            BindableProperty.Create(
                nameof(ShowActionButtons),
                typeof(bool),
                typeof(CalendarFooterView),
                false,
                propertyChanged: OnActionButtonsPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="ShowTodayButton"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="ShowTodayButton"/> dependency property.
        /// </value>
        public static readonly BindableProperty ShowTodayButtonProperty =
            BindableProperty.Create(
                nameof(ShowTodayButton),
                typeof(bool),
                typeof(CalendarFooterView),
                false,
                propertyChanged: OnShowTodayButtonChanged);

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
                typeof(CalendarFooterView),
                defaultValueCreator: bindable => GetFooterTextStyle(bindable),
                propertyChanged: OnTextStyleChanged);

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarFooterView"/> class.
        /// </summary>
        public CalendarFooterView()
        {
            ThemeElement.InitializeThemeResources(this, "SfCalendarTheme");
            TextStyle.Parent = this;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the value to specify the height of footer view on SfCalendar.
        /// </summary>
        /// <value> The default value of <see cref="CalendarFooterView.Height"/> is 50. </value>
        /// <seealso cref="SfCalendar.FooterView"/>
        /// <example>
        /// The following code demonstrates, how to use the Height property in the calendar
        /// <code Lang="C#"><![CDATA[
        /// this.Calendar.FooterView.Height = 50;
        /// ]]></code>
        /// </example>
        public double Height
        {
            get { return (double)GetValue(HeightProperty); }
            set { SetValue(HeightProperty, value); }
        }

        /// <summary>
        /// Gets or sets the background of the footer view in SfCalendar.
        /// </summary>
        /// <value>The Default value of <see cref="CalendarFooterView.Background"/> is Transparent. </value>
        /// <example>
        /// The following code demonstrates, how to use the Background property in the calendar
        /// <code Lang="C#"><![CDATA[
        /// this.Calendar.FooterView.Background = Colors.Red;
        /// ]]></code>
        /// </example>
        public Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets the background of the footer separator line background in SfCalendar.
        /// </summary>
        /// <value>The Default value of <see cref="CalendarFooterView.DividerColor"/> is Transparent. </value>
        /// <example>
        /// The following code demonstrates, how to use the DividerColor property in the calendar
        /// <code Lang="C#"><![CDATA[
        /// this.Calendar.FooterView.DividerColor = Colors.Red;
        /// ]]></code>
        /// </example>
        public Color DividerColor
        {
            get { return (Color)GetValue(DividerColorProperty); }
            set { SetValue(DividerColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show the cancel button in the footer view of SfCalendar.
        /// </summary>
        /// <value>The Default value of <see cref="CalendarFooterView.ShowActionButtons"/> is false. </value>
        /// <example>
        /// The following code demonstrates, how to use the ShowActionButtons property in the calendar
        /// <code Lang="C#"><![CDATA[
        /// this.Calendar.FooterView.ShowActionButtons = true;
        /// ]]></code>
        /// </example>
        public bool ShowActionButtons
        {
            get { return (bool)GetValue(ShowActionButtonsProperty); }
            set { SetValue(ShowActionButtonsProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show the today button in the footer view of SfCalendar.
        /// </summary>
        /// <value>The Default value of <see cref="CalendarFooterView.ShowTodayButton"/> is false. </value>
        /// <example>
        /// The following code demonstrates, how to use the ShowTodayButton property in the calendar
        /// <code Lang="C#"><![CDATA[
        /// this.Calendar.FooterView.ShowTodayButton = true;
        /// ]]></code>
        /// </example>
        public bool ShowTodayButton
        {
            get { return (bool)GetValue(ShowTodayButtonProperty); }
            set { SetValue(ShowTodayButtonProperty, value); }
        }

        /// <summary>
        /// Gets or sets the ok and cancel button text style in the footer view of SfCalendar.
        /// </summary>
        /// <seealso cref="SfCalendar.FooterView"/>
        /// <example>
        /// The following code demonstrates, how to use the TextStyle property in the calendar
        /// <code Lang="C#"><![CDATA[
        ///  var footerTextStyle = new CalendarTextStyle()
        ///  {
        ///      TextColor = Colors.Blue,
        ///      FontSize = 14,
        ///  };
        /// this.Calendar.FooterView.TextStyle = footerTextStyle;
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
        /// Method invokes on the calendar footer height changed.
        /// </summary>
        /// <param name="bindable">The footer settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnHeightChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as CalendarFooterView)?.RaisePropertyChanged(nameof(Height));
        }

        /// <summary>
        /// Method invokes on the calendar footer background changed.
        /// </summary>
        /// <param name="bindable">The footer settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnBackgroundChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as CalendarFooterView)?.RaisePropertyChanged(nameof(Background));
        }

        /// <summary>
        /// Method invokes on the calendar footer separator line background changed.
        /// </summary>
        /// <param name="bindable">The footer settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnDividerColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as CalendarFooterView)?.RaisePropertyChanged(nameof(DividerColor));
        }

        /// <summary>
        /// Method invokes to get the default text style of the footer view.
        /// </summary>
        /// <returns>Returns the footer view text style.</returns>
        static Syncfusion.Maui.Toolkit.Graphics.Internals.ITextElement GetFooterTextStyle(BindableObject bindable)
        {
            var calendarFooterView = (CalendarFooterView)bindable;
            CalendarTextStyle textStyle = new CalendarTextStyle()
            {
                FontSize = 14,
                TextColor = Color.FromArgb("#6750A4"),
                Parent = calendarFooterView,
            };

            return textStyle;
        }

        /// <summary>
        /// Method invokes on the calendar footer show ok button changed.
        /// </summary>
        /// <param name="bindable">The footer settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnActionButtonsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as CalendarFooterView)?.RaisePropertyChanged(nameof(ShowActionButtons));
        }

        /// <summary>
        /// Method invokes on the calendar footer show today button changed.
        /// </summary>
        /// <param name="bindable">The footer settings.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnShowTodayButtonChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as CalendarFooterView)?.RaisePropertyChanged(nameof(ShowTodayButton));
        }

        /// <summary>
        /// Method invokes on the calendar footer ok and cancel button text style changed.
        /// </summary>
        /// <param name="bindable">The footer settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnTextStyleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as CalendarFooterView)?.RaisePropertyChanged(nameof(TextStyle));
        }

        /// <summary>
        /// Method invokes on the calendar property changed event on footer settings properties changed.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        /// <param name="oldValue">Property old value.</param>
        void RaisePropertyChanged(string propertyName, object? oldValue = null)
        {
            CalendarPropertyChanged?.Invoke(this, new CalendarPropertyChangedEventArgs(propertyName) { OldValue = oldValue });
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
        /// Event Invokes on footer settings property changed and this includes old value of the changed property which is used to unwire events for nested classes.
        /// </summary>
        internal event EventHandler<CalendarPropertyChangedEventArgs>? CalendarPropertyChanged;

        #endregion
    }
}
