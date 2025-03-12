using Syncfusion.Maui.Toolkit.Themes;

namespace Syncfusion.Maui.Toolkit.Calendar
{
    /// <summary>
    /// Represents a class which is used to used to configure all the properties and styles of calendar year, decade and century view.
    /// </summary>
    public class CalendarYearView : Element, IThemeElement
    {
        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="MonthFormat"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="MonthFormat"/> dependency property.
        /// </value>
        public static readonly BindableProperty MonthFormatProperty =
            BindableProperty.Create(
                nameof(MonthFormat),
                typeof(string),
                typeof(CalendarYearView),
                "MMM",
                propertyChanged: OnMonthFormatChanged);

        /// <summary>
        /// Identifies the <see cref="DisabledDatesBackground"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="DisabledDatesBackground"/> dependency property.
        /// </value>
        public static readonly BindableProperty DisabledDatesBackgroundProperty =
            BindableProperty.Create(
                nameof(DisabledDatesBackground),
                typeof(Brush),
                typeof(CalendarYearView),
                defaultValueCreator: bindable => Brush.Transparent,
                propertyChanged: OnDisabledDatesBackgroundChanged);

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
                typeof(CalendarYearView),
                defaultValueCreator: bindable => Brush.Transparent,
                propertyChanged: OnBackgroundChanged);

        /// <summary>
        /// Identifies the <see cref="TodayBackground"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="TodayBackground"/> dependency property.
        /// </value>
        public static readonly BindableProperty TodayBackgroundProperty =
            BindableProperty.Create(
                nameof(TodayBackground),
                typeof(Brush),
                typeof(CalendarYearView),
                defaultValueCreator: bindable => Brush.Transparent,
                propertyChanged: OnTodayBackgroundChanged);

        /// <summary>
        /// Identifies the <see cref="LeadingDatesBackground"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="LeadingDatesBackground"/> dependency property.
        /// </value>
        public static readonly BindableProperty LeadingDatesBackgroundProperty =
            BindableProperty.Create(
                nameof(LeadingDatesBackground),
                typeof(Brush),
                typeof(CalendarYearView),
                defaultValueCreator: bindable => Brush.Transparent,
                propertyChanged: OnLeadingDatesBackgroundChanged);

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
                typeof(CalendarYearView),
                defaultValueCreator: bindable => new CalendarTextStyle()
                {
                    Parent = bindable as Element,
                    FontSize = CalendarFonts.GetBodyTextSize(),
                    TextColor = CalendarColors.GetOnSecondaryVariantColor()
                },
                propertyChanged: OnTextStyleChanged);

        /// <summary>
        /// Identifies the <see cref="TodayTextStyle"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="TodayTextStyle"/> dependency property.
        /// </value>
        public static readonly BindableProperty TodayTextStyleProperty =
            BindableProperty.Create(
                nameof(TodayTextStyle),
                typeof(CalendarTextStyle),
                typeof(CalendarYearView),
                defaultValueCreator: bindable => new CalendarTextStyle()
                {
                    Parent = bindable as Element,
                    FontSize = CalendarFonts.GetBodyTextSize(),
                    TextColor = CalendarColors.GetPrimaryColor()
                },
                propertyChanged: OnTodayTextStyleChanged);

        /// <summary>
        /// Identifies the <see cref="LeadingDatesTextStyle"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="LeadingDatesTextStyle"/> dependency property.
        /// </value>
        public static readonly BindableProperty LeadingDatesTextStyleProperty =
            BindableProperty.Create(
                nameof(LeadingDatesTextStyle),
                typeof(CalendarTextStyle),
                typeof(CalendarYearView),
                defaultValueCreator: bindable => new CalendarTextStyle()
                {
                    Parent = bindable as Element,
                    FontSize = CalendarFonts.GetBodyTextSize(),
                    TextColor = CalendarColors.GetOnSecondaryColor()
                },
                propertyChanged: OnLeadingDatesTextStyleChanged);

        /// <summary>
        /// Identifies the <see cref="DisabledDatesTextStyle"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="DisabledDatesTextStyle"/> dependency property.
        /// </value>
        public static readonly BindableProperty DisabledDatesTextStyleProperty =
            BindableProperty.Create(
                nameof(DisabledDatesTextStyle),
                typeof(CalendarTextStyle),
                typeof(CalendarYearView),
                defaultValueCreator: bindable => new CalendarTextStyle()
                {
                    Parent = bindable as Element,
                    FontSize = CalendarFonts.GetBodyTextSize(),
                    TextColor = CalendarColors.GetDisabledColor()
                },
                propertyChanged: OnDisabledDatesTextStyleChanged);

        /// <summary>
        /// Identifies the <see cref="RangeTextStyle"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="RangeTextStyle"/> dependency property.
        /// </value>
        public static readonly BindableProperty RangeTextStyleProperty =
            BindableProperty.Create(
                nameof(RangeTextStyle),
                typeof(CalendarTextStyle),
                typeof(CalendarYearView),
                defaultValueCreator: bindable => new CalendarTextStyle()
                {
                    Parent = bindable as Element,
                    FontSize = CalendarFonts.GetBodyTextSize(),
                    TextColor = CalendarColors.GetOnSecondaryColor()
                },
                propertyChanged: OnRangeTextStyleChanged);

        /// <summary>
        /// Identifies the <see cref="SelectionTextStyle"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="SelectionTextStyle"/> dependency property.
        /// </value>
        public static readonly BindableProperty SelectionTextStyleProperty =
            BindableProperty.Create(
                nameof(SelectionTextStyle),
                typeof(CalendarTextStyle),
                typeof(CalendarYearView),
                defaultValueCreator: bindable => new CalendarTextStyle()
                {
                    Parent = bindable as Element,
                    FontSize = CalendarFonts.GetBodyTextSize(),
                    TextColor = CalendarColors.GetOnPrimaryColor()
                },
                propertyChanged: OnSelectionTextStyleChanged);

        /// <summary>
        /// Identifies the <see cref="CellTemplate"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="CellTemplate"/> dependency property.
        /// </value>
        public static readonly BindableProperty CellTemplateProperty =
            BindableProperty.Create(
                nameof(CellTemplate),
                typeof(DataTemplate),
                typeof(CalendarYearView),
                null,
                propertyChanged: OnCellTemplatePropertyChanged);

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarYearView"/> class.
        /// </summary>
        public CalendarYearView()
        {
            ThemeElement.InitializeThemeResources(this, "SfCalendarTheme");
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the format to customize the month text in calendar year view.
        /// </summary>
        /// <value> The default value of <see cref="CalendarYearView.MonthFormat"/> is "MMM". </value>
        /// <remarks>
        /// It is only applicable for <see cref="CalendarView.Year"/> view.
        /// </remarks>
        /// <seealso cref="SfCalendar.YearView"/>
        /// <example>
        /// The following code demonstrates, how to use the MonthFormat property in the calendar
        /// <code Lang="C#"><![CDATA[
        /// this.Calendar.YearView.MonthFormat = "MMM";
        /// ]]></code>
        /// </example>
        public string MonthFormat
        {
            get { return (string)GetValue(MonthFormatProperty); }
            set { SetValue(MonthFormatProperty, value); }
        }

        /// <summary>
        /// Gets or sets the background for the disabled year, decade and century cells of the calendar year, decade and century view.
        /// </summary>
        /// <value> The default value of <see cref="CalendarYearView.DisabledDatesBackground"/> is Transparent. </value>
        /// <seealso cref="SfCalendar.MinimumDate"/>
        /// <seealso cref="SfCalendar.MaximumDate"/>
        /// <seealso cref="SfCalendar.EnablePastDates"/>
        /// <seealso cref="SfCalendar.SelectableDayPredicate"/>
        /// <seealso cref="SfCalendar.YearView"/>
        /// <seealso cref="CalendarYearView.DisabledDatesTextStyle"/>
        /// <example>
        /// The following code demonstrates, how to use the DisabledDatesBackground property in the calendar
        /// <code Lang="C#"><![CDATA[
        /// this.Calendar.YearView.DisabledDatesBackground = Colors.Grey;
        /// ]]></code>
        /// </example>
        public Brush DisabledDatesBackground
        {
            get { return (Brush)GetValue(DisabledDatesBackgroundProperty); }
            set { SetValue(DisabledDatesBackgroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets the background for the year, decade and century cells of the calendar year, decade and century view.
        /// </summary>
        /// <value> The default value of <see cref="CalendarYearView.Background"/> is Transparent. </value>
        /// <seealso cref="SfCalendar.YearView"/>
        /// <seealso cref="CalendarYearView.TextStyle"/>
        /// <example>
        /// The following code demonstrates, how to use the Background property in the calendar
        /// <code Lang="C#"><![CDATA[
        /// this.Calendar.YearView.Background = Colors.PaleGreen;
        /// ]]></code>
        /// </example>
        public Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets the background for the today year, decade and century cell of the calendar year, decade and century view.
        /// </summary>
        /// <value> The default value of <see cref="CalendarYearView.TodayBackground"/> is Transparent. </value>
        /// <seealso cref="SfCalendar.YearView"/>
        /// <seealso cref="CalendarYearView.TodayTextStyle"/>
        /// <example>
        /// The following code demonstrates, how to use the TodayBackground property in the calendar
        /// <code Lang="C#"><![CDATA[
        /// this.Calendar.YearView.TodayBackground = Colors.Pink;
        /// ]]></code>
        /// </example>
        public Brush TodayBackground
        {
            get { return (Brush)GetValue(TodayBackgroundProperty); }
            set { SetValue(TodayBackgroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets the background for the leading decade and century cells of the calendar decade and century view.
        /// </summary>
        /// <value> The default value of <see cref="CalendarYearView.LeadingDatesBackground"/> is Transparent. </value>
        /// <seealso cref="SfCalendar.YearView"/>
        /// <seealso cref="SfCalendar.ShowTrailingAndLeadingDates"/>
        /// <seealso cref="CalendarYearView.LeadingDatesTextStyle"/>
        /// <example>
        /// The following code demonstrates, how to use the LeadingDatesBackground property in the calendar
        /// <code Lang="C#"><![CDATA[
        /// this.Calendar.YearView.LeadingDatesBackground = Colors.Red;
        /// ]]></code>
        /// </example>
        public Brush LeadingDatesBackground
        {
            get { return (Brush)GetValue(LeadingDatesBackgroundProperty); }
            set { SetValue(LeadingDatesBackgroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets the text style of the year, decade and century cells text, that used to customize the text color, font, font size, font family and font attributes.
        /// </summary>
        /// <seealso cref="SfCalendar.YearView"/>
        /// <seealso cref="CalendarYearView.Background"/>
        /// <example>
        /// The following code demonstrates, how to use the TextStyle property in the calendar
        /// <code Lang="C#"><![CDATA[
        /// CalendarTextStyle textStyle = new CalendarTextStyle()
        /// {
        /// 	FontSize = 14,
        /// 	TextColor = Colors.Red,
        /// };
        /// this.Calendar.YearView.TextStyle = textStyle;
        /// ]]></code>
        /// </example>
        public CalendarTextStyle TextStyle
        {
            get { return (CalendarTextStyle)GetValue(TextStyleProperty); }
            set { SetValue(TextStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the text style of the today year, decade and century cell text, that used to customize the text color, font, font size, font family and font attributes.
        /// </summary>
        /// <seealso cref="SfCalendar.YearView"/>
        /// <seealso cref="CalendarYearView.TodayBackground"/>
        /// <example>
        /// The following code demonstrates, how to use the TodayTextStyle property in the calendar
        /// <code Lang="C#"><![CDATA[
        /// CalendarTextStyle textStyle = new CalendarTextStyle()
        /// {
        /// 	FontSize = 14,
        /// 	TextColor = Colors.Red,
        /// };
        /// this.Calendar.YearView.TodayTextStyle = textStyle;
        /// ]]></code>
        /// </example>
        public CalendarTextStyle TodayTextStyle
        {
            get { return (CalendarTextStyle)GetValue(TodayTextStyleProperty); }
            set { SetValue(TodayTextStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the text style of the leading decade and century cells text, that used to customize the text color, font, font size, font family and font attributes.
        /// </summary>
        /// <seealso cref="SfCalendar.ShowTrailingAndLeadingDates"/>
        /// <seealso cref="SfCalendar.YearView"/>
        /// <seealso cref="CalendarYearView.LeadingDatesBackground"/>
        /// <example>
        /// The following code demonstrates, how to use the LeadingDatesBackground property in the calendar
        /// <code Lang="C#"><![CDATA[
        /// CalendarTextStyle textStyle = new CalendarTextStyle()
        /// {
        /// 	FontSize = 14,
        /// 	TextColor = Colors.Red,
        /// };
        /// this.Calendar.YearView.LeadingDatesTextStyle = textStyle;
        /// ]]></code>
        /// </example>
        public CalendarTextStyle LeadingDatesTextStyle
        {
            get { return (CalendarTextStyle)GetValue(LeadingDatesTextStyleProperty); }
            set { SetValue(LeadingDatesTextStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the text style of the disabled year, decade and century cells text, that used to customize the text color, font, font size, font family and font attributes.
        /// </summary>
        /// <seealso cref="SfCalendar.MaximumDate"/>
        /// <seealso cref="SfCalendar.MinimumDate"/>
        /// <seealso cref="SfCalendar.EnablePastDates"/>
        /// <seealso cref="SfCalendar.YearView"/>
        /// <seealso cref="SfCalendar.SelectableDayPredicate"/>
        /// <seealso cref="CalendarYearView.DisabledDatesBackground"/>
        /// <example>
        /// The following code demonstrates, how to use the DisabledDatesTextStyle property in the calendar
        /// <code Lang="C#"><![CDATA[
        /// CalendarTextStyle textStyle = new CalendarTextStyle()
        /// {
        /// 	FontSize = 14,
        /// 	TextColor = Colors.Red,
        /// };
        /// this.Calendar.YearView.DisabledDatesTextStyle = textStyle;
        /// ]]></code>
        /// </example>
        public CalendarTextStyle DisabledDatesTextStyle
        {
            get { return (CalendarTextStyle)GetValue(DisabledDatesTextStyleProperty); }
            set { SetValue(DisabledDatesTextStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the text style of the range in-between year, decade and century cells text, that used to customize the text color, font, font size, font family and font attributes.
        /// </summary>
        /// <seealso cref="SfCalendar.YearView"/>
        /// <seealso cref="SfCalendar.SelectionBackground"/>
        /// <example>
        /// The following code demonstrates, how to use the RangeTextStyle property in the calendar
        /// <code Lang="C#"><![CDATA[
        /// CalendarTextStyle textStyle = new CalendarTextStyle()
        /// {
        /// 	FontSize = 14,
        /// 	TextColor = Colors.Red,
        /// };
        /// this.Calendar.YearView.RangeTextStyle = textStyle;
        /// ]]></code>
        /// </example>
        public CalendarTextStyle RangeTextStyle
        {
            get { return (CalendarTextStyle)GetValue(RangeTextStyleProperty); }
            set { SetValue(RangeTextStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the text style of the selection year, decade and century cells text, that used to customize the text color, font, font size, font family and font attributes.
        /// </summary>
        /// <seealso cref="SfCalendar.YearView"/>
        /// <seealso cref="SfCalendar.StartRangeSelectionBackground"/>
        /// <seealso cref="SfCalendar.EndRangeSelectionBackground"/>
        /// <example>
        /// The following code demonstrates, how to use the SelectionTextStyle property in the calendar
        /// <code Lang="C#"><![CDATA[
        /// CalendarTextStyle textStyle = new CalendarTextStyle()
        /// {
        /// 	FontSize = 14,
        /// 	TextColor = Colors.Red,
        /// };
        /// this.Calendar.YearView.SelectionTextStyle = textStyle;
        /// ]]></code>
        /// </example>
        public CalendarTextStyle SelectionTextStyle
        {
            get { return (CalendarTextStyle)GetValue(SelectionTextStyleProperty); }
            set { SetValue(SelectionTextStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the year cell template or template selector.
        /// </summary>
        /// <value> The default value of <see cref="CalendarYearView.CellTemplate"/> is null. </value>
        /// <remarks>
        /// The BindingContext of the <see cref="CalendarYearView.CellTemplate"/> is the <see cref="CalendarCellDetails" />
        /// If the cell template is not null, Range selection direction of forward, backward and both direction does not drawn the dashed line hovering.
        /// Hovering draws like default range selection direction.
        /// </remarks>
        /// <seealso cref="CalendarCellDetails.Date"/>
        /// <seealso cref="CalendarCellDetails.IsTrailingOrLeadingDate"/>
        /// <seealso cref="SfCalendar.SelectionMode"/>
        /// <example>
        /// The following code demonstrates, how to use the Cell Template property in the year view.
        /// # [XAML](#tab/tabid-1)
        /// <code Lang="XAML"><![CDATA[
        /// <calendar:SfCalendar x:Name= "Calendar" View = "Decade">
        ///              < calendar:SfCalendar.YearView>
        ///                  <calendar:CalendarYearView>
        ///                      <calendar:CalendarYearView.CellTemplate>
        ///                          <DataTemplate>
        ///                              <Grid BackgroundColor = "Pink" >
        ///                                  < Label HorizontalTextAlignment= "Center" VerticalTextAlignment= "Center" TextColor= "Purple" Text= "{Binding Date.Year}" />
        ///                              </Grid >
        ///                          </ DataTemplate >
        ///                      </ calendar:CalendarYearView.CellTemplate>
        ///                  </calendar:CalendarYearView>
        ///              </calendar:SfCalendar.YearView>
        /// </calendar:SfCalendar>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code Lang="C#"><![CDATA[
        /// this.Calendar.View = CalendarView.Decade;
        /// this.Calendar.YearView = new CalendarYearView()
        /// {
        ///     CellTemplate = dataTemplate,
        /// };
        ///
        /// DataTemplate dataTemplate = new DataTemplate(() =>
        /// {
        ///      Grid grid = new Grid
        ///      {
        ///          BackgroundColor = Colors.Pink,
        ///      };
        ///
        ///      Label label = new Label
        ///      {
        ///          HorizontalTextAlignment = TextAlignment.Center,
        ///          VerticalTextAlignment = TextAlignment.Center,
        ///          TextColor = Colors.Purple,
        ///      };
        ///
        ///      label.SetBinding(Label.TextProperty, new Binding("Date.Year"));
        ///      grid.Children.Add(label);
        ///      return grid;
        ///  });
        /// ]]></code>
        /// </example>
        public DataTemplate CellTemplate
        {
            get { return (DataTemplate)GetValue(CellTemplateProperty); }
            set { SetValue(CellTemplateProperty, value); }
        }

        #endregion

        #region Property Changed Methods

        /// <summary>
        /// Invokes on month format value changed.
        /// </summary>
        /// <param name="bindable">The year view settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnMonthFormatChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as CalendarYearView)?.RaisePropertyChanged(nameof(MonthFormat));
        }

        /// <summary>
        /// Invokes on year cell style disabled dates background changed.
        /// </summary>
        /// <param name="bindable">The year view settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnDisabledDatesBackgroundChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as CalendarYearView)?.RaisePropertyChanged(nameof(DisabledDatesBackground));
        }

        /// <summary>
        /// Invokes on year cell style background changed.
        /// </summary>
        /// <param name="bindable">The year view settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnBackgroundChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as CalendarYearView)?.RaisePropertyChanged(nameof(Background));
        }

        /// <summary>
        /// Invokes on year cell style today background changed.
        /// </summary>
        /// <param name="bindable">The year view settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnTodayBackgroundChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as CalendarYearView)?.RaisePropertyChanged(nameof(TodayBackground));
        }

        /// <summary>
        /// Invokes on year cell style leading year cell background changed.
        /// </summary>
        /// <param name="bindable">The year view settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnLeadingDatesBackgroundChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as CalendarYearView)?.RaisePropertyChanged(nameof(LeadingDatesBackground));
        }

        /// <summary>
        /// Invokes on year cell text style changed.
        /// </summary>
        /// <param name="bindable">The year view settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnTextStyleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as CalendarYearView)?.RaisePropertyChanged(nameof(TextStyle), oldValue);
            if (bindable is CalendarYearView calendarYearView)
            {
                calendarYearView.SetParent(oldValue as Element, newValue as Element);
            }
        }

        /// <summary>
        /// Invokes on year cell today text style changed.
        /// </summary>
        /// <param name="bindable">The year view settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnTodayTextStyleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as CalendarYearView)?.RaisePropertyChanged(nameof(TodayTextStyle), oldValue);
            if (bindable is CalendarYearView calendarYearView)
            {
                calendarYearView.SetParent(oldValue as Element, newValue as Element);
            }
        }

        /// <summary>
        /// Invokes on year cell leading dates text style changed.
        /// </summary>
        /// <param name="bindable">The year view settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnLeadingDatesTextStyleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as CalendarYearView)?.RaisePropertyChanged(nameof(LeadingDatesTextStyle), oldValue);
            if (bindable is CalendarYearView calendarYearView)
            {
                calendarYearView.SetParent(oldValue as Element, newValue as Element);
            }
        }

        /// <summary>
        /// Invokes on year cell disabled dates text style changed.
        /// </summary>
        /// <param name="bindable">The year view settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnDisabledDatesTextStyleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as CalendarYearView)?.RaisePropertyChanged(nameof(DisabledDatesTextStyle), oldValue);
            if (bindable is CalendarYearView calendarYearView)
            {
                calendarYearView.SetParent(oldValue as Element, newValue as Element);
            }
        }

        /// <summary>
        /// Invokes on year cell range text style changed.
        /// </summary>
        /// <param name="bindable">The year view settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnRangeTextStyleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as CalendarYearView)?.RaisePropertyChanged(nameof(RangeTextStyle), oldValue);
            if (bindable is CalendarYearView calendarYearView)
            {
                calendarYearView.SetParent(oldValue as Element, newValue as Element);
            }
        }

        /// <summary>
        /// Invokes on year cell selection text style changed.
        /// </summary>
        /// <param name="bindable">The year view settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnSelectionTextStyleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as CalendarYearView)?.RaisePropertyChanged(nameof(SelectionTextStyle), oldValue);
            if (bindable is CalendarYearView calendarYearView)
            {
                calendarYearView.SetParent(oldValue as Element, newValue as Element);
            }
        }

        /// <summary>
        /// Method to invoke calendar property changed event on Cell template properties changed.
        /// </summary>
        /// <param name="bindable">Property name.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnCellTemplatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as CalendarYearView)?.RaisePropertyChanged(nameof(CellTemplate), oldValue);
        }

        /// <summary>
        /// Method to invoke calendar property changed event on year cell style properties changed.
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
        /// Invokes on year cell style property changed and this includes old value of the changed property which is used to wire and unwire events for nested classes.
        /// </summary>
        internal event EventHandler<CalendarPropertyChangedEventArgs>? CalendarPropertyChanged;

        #endregion
    }
}