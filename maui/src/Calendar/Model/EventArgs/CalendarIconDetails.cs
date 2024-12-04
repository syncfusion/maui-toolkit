using Syncfusion.Maui.Toolkit.Themes;

namespace Syncfusion.Maui.Toolkit.Calendar
{
    /// <summary>
    /// Holds the information about special date icon and its color for the SfCalendar.
    /// </summary>
    public class CalendarIconDetails : Element, IThemeElement
    {
        #region Bindable Property

        /// <summary>
        /// Identifies the <see cref="Fill"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Fill"/> dependency property.
        /// </value>
        public static readonly BindableProperty FillProperty =
            BindableProperty.Create(
                nameof(Fill),
                typeof(Brush),
                typeof(CalendarIconDetails),
                defaultValueCreator: bindable => CalendarColors.GetPrimaryBrush());

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarIconDetails"/> class.
        /// </summary>
        public CalendarIconDetails()
        {
            ThemeElement.InitializeThemeResources(this, "SfCalendarTheme");
            Icon = CalendarIcon.Dot;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the type of special date icon for the SfCalendar.
        /// </summary>
        /// <example>
        /// The following code demonstrates, how to use the calendar icon property in calendar.
        /// <code Lang="C#"><![CDATA[
        ///  this.calendar.MonthView.SpecialDayPredicate = (date) =>
        ///  {
        ///    if (date.Date == DateTime.Now.AddDays(2).Date)
        ///    {
        ///      CalendarIconDetails iconDetails = new CalendarIconDetails();
        ///      iconDetails.Icon = CalendarIcon.Dot;
        ///      return iconDetails;
        ///     }
        ///     return null;
        ///  };
        /// ]]></code>
        /// </example>
        public CalendarIcon Icon { get; set; }

        /// <summary>
        /// Gets or sets the color of special date icon for the SfCalendar.
        /// </summary>
        /// <example>
        /// The following code demonstrates, how to use the fill property in calendar.
        /// <code Lang="C#"><![CDATA[
        ///  this.calendar.MonthView.SpecialDayPredicate = (date) =>
        ///  {
        ///    if (date.Date == DateTime.Now.AddDays(2).Date)
        ///    {
        ///      CalendarIconDetails iconDetails = new CalendarIconDetails();
        ///      iconDetails.Fill = Colors.Red;
        ///      return iconDetails;
        ///     }
        ///     return null;
        ///  };
        /// ]]></code>
        /// </example>
        public Brush Fill
        {
            get { return (Brush)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }

        #endregion

        #region Internal Property

        /// <summary>
        /// Gets or sets the date value for the special day icon details.
        /// </summary>
        internal DateTime Date { get; set; }

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
    }
}