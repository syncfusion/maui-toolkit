using System.Globalization;
using Syncfusion.Maui.Toolkit.Internals;
using Syncfusion.Maui.Toolkit.Graphics.Internals;
using Globalization = System.Globalization;

namespace Syncfusion.Maui.Toolkit.Calendar
{
    /// <summary>
    /// Represents a class to render the header view in month view.
    /// </summary>
    internal class MonthViewHeader : SfView, ITapGestureListener, IDoubleTapGestureListener, ILongPressGestureListener
    {
        #region Fields

        /// <summary>
        /// The calendar view details.
        /// </summary>
        readonly ICalendar _calendarInfo;

        /// <summary>
        /// The visible dates collection.
        /// </summary>
        List<DateTime> _visibleDates;

        /// <summary>
        /// Gets or sets the virtual month cell and week number semantic nodes.
        /// </summary>
        List<SemanticsNode>? _semanticsNodes;

        /// <summary>
        /// Gets or sets the size of the semantic.
        /// </summary>
        Size _semanticsSize = Size.Zero;

        /// <summary>
        /// Gets or sets the view is current view or not.
        /// </summary>
        bool _isCurrentView;

        /// <summary>
        /// Gets or sets the view header cell views created from data template. Applicable only on month view header template provided.
        /// </summary>
        List<View>? _viewHeaderCells;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MonthViewHeader"/> class.
        /// </summary>
        /// <param name="calendarInfo">The looping panel information.</param>
        /// <param name="visibleDates">The visible dates collection.</param>
        /// <param name="isCurrentView">The view header is current view or not.</param>
        internal MonthViewHeader(ICalendar calendarInfo, List<DateTime> visibleDates, bool isCurrentView)
        {
            _isCurrentView = isCurrentView;
            DrawingOrder = DrawingOrder.BelowContent;
            _calendarInfo = calendarInfo;
            _visibleDates = visibleDates;
            if (_calendarInfo.MonthViewHeaderTemplate != null)
            {
                int numberOfVisibleWeeks = CalendarViewHelper.GetNumberOfWeeks(_calendarInfo.MonthView);
                _viewHeaderCells = new List<View>();
                CreateViewHeaderCells(numberOfVisibleWeeks);
            }

            this.AddGestureListener(this);
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Method to update the visible dates.
        /// </summary>
        /// <param name="visibleDates">Gets the visible dates.</param>
        internal void UpdateVisibleDatesChange(List<DateTime> visibleDates)
        {
            //// If the visible dates are not changed then no need to update the UI.
            if (_visibleDates == visibleDates)
            {
                return;
            }

            if (_calendarInfo.MonthViewHeaderTemplate == null)
            {
                _visibleDates = visibleDates;
                InvalidateDrawable();
            }
            else
            {
                int numberOfVisibleWeeks = CalendarViewHelper.GetNumberOfWeeks(_calendarInfo.MonthView);

                //// If the visible dates count are not equal to current visible dates when number of visible weeks view is changed.
                if (_visibleDates.Count != visibleDates.Count)
                {
                    _visibleDates = visibleDates;
#if !ANDROID
                    UpdateViewHeaderTemplateViews(numberOfVisibleWeeks == 6 ? _visibleDates[_visibleDates.Count / 2].Month : _visibleDates[0].Month, numberOfVisibleWeeks);
#else
                    UpdateTemplatesWithDelay();
#endif
                    return;
                }

                _visibleDates = visibleDates;
#if ANDROID
                UpdateTemplatesWithDelay();
#else
                UpdateTemplateViews();
#endif
            }
        }

        /// <summary>
        /// Method to update the current view header.
        /// </summary>
        internal void InvalidateDrawMonthHeaderView()
        {
            InvalidateDrawable();
            if (_viewHeaderCells != null)
            {
                InvalidateViewMeasure();
            }
        }

        /// <summary>
        /// Method to update the is current view
        /// </summary>
        /// <param name="isCurrentView">The view is current view or not.</param>
        internal void InvalidateSemanticsNode(bool isCurrentView)
        {
            _isCurrentView = isCurrentView;
            _semanticsNodes?.Clear();
            _semanticsNodes = null;
            _semanticsSize = Size.Zero;
            InvalidateSemantics();
        }

        /// <summary>
        /// Method to create the view header template.
        /// </summary>
        internal void CreateViewHeaderTemplate()
        {
            if (_calendarInfo.MonthViewHeaderTemplate != null)
            {
                Children.Clear();
                int numberOfVisibleWeeks = CalendarViewHelper.GetNumberOfWeeks(_calendarInfo.MonthView);
                _viewHeaderCells = new List<View>();
                CreateViewHeaderCells(numberOfVisibleWeeks);
#if ANDROID
                InvalidateViewMeasure();
#endif
            }
        }

        /// <summary>
        /// Method to invalidate view measures.
        /// </summary>
        internal void InvalidateViewMeasure()
        {
            //// Invalidate measure invokes parent layout measure in all platforms thus it makes performance issue due to layout cycle.
            //// Hence used direct measure and arrange override methods to invalidate the child measure and arrange.
            MeasureContent(Width, Height);
            ArrangeContent(new Rect(0, 0, Width, Height));
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Occurs on tap interaction inside month view header.
        /// </summary>
        /// <param name="tapPoint">The interaction point.</param>
        /// <param name="isTapped">Is tapped.</param>
        /// <param name="tapCount">The tap count.</param>
        void OnInteractionEvent(Point tapPoint, bool isTapped, int tapCount = 1)
        {
            bool isRTL = _calendarInfo.IsRTLLayout;
            float weekNumberWidth = CalendarViewHelper.GetWeekNumberWidth(_calendarInfo.MonthView, (float)Width);

            // Example: The total month view width is 730.
            // Calculation: The actual month view width is 730. So 730 - (30)week number width = 700(the actual month view width).
            float monthViewWidth = (float)Width - weekNumberWidth;

            // The total number of column in month view header.
            int columCount = 7;

            // The cellWidth = 700 / 7 = 100(View header cell width).
            double cellWidth = monthViewWidth / columCount;

            // Example 1: To restrict the week number tapped and double tapped and long pressed event and command. Assume the tap point is 90.
            // Calculation: For RTL. The XPosition = 730(monthViewWidth)- 800(taped XPoint) = -70(xPosition); If we get the negative value then consider tapped or double tapped or long pressed event and command are restricted within the week number.
            // Example 2: To trigger the view header interaction support like, Tapped and double Tapped and longPressed Events and command. Assume the tap point is 700.
            // Calculation: For LTR. The XPosition = 700(tap point) - 70(week number width) = 630(xPosition); No need to restricted
            double xPosition = isRTL ? monthViewWidth - tapPoint.X : tapPoint.X - weekNumberWidth;

            // If the xPosition is lesser than 0 then it means user interact with week number view. So no need to trigger the Event and command.
            if (xPosition < 0)
            {
                return;
            }

            DateTime tappedDate;

            // To find the index we need to divide the XPosition by cellWidth.
            // Example: 200(XPosition) / 100(cell width) = 2 (The tapped index value).
            int index = (int)xPosition / (int)cellWidth;
            int numberOfWeeks = _calendarInfo.MonthView.GetNumberOfWeeks();
            int visibleDateCount = _visibleDates.Count;

            if (visibleDateCount % 7 != 0)
            {
                DateTime currentDate = _visibleDates.First();

                // Get start index based on different between the current date of day of week and first day of week property day of week.
                int startIndex = CalendarViewHelper.GetFirstDayOfWeekDifference(currentDate, _calendarInfo.MonthView.FirstDayOfWeek, _calendarInfo.Identifier);

                // Here startIndex = 6.
                // The index = 4 - 6= -2(index).
                index = index - startIndex;

                // Example: The actual visible date count is 14 while number of weeks is 2.
                // But while number of week is 1 from above visible date collection example the visible dates count is 1.
                if (visibleDateCount < 7)
                {
                    // If tapped before the first day of week does not need to trigger the tapped event.
                    // Condition true from example -2 < 0.
                    // While reaching end date after visible date count does not need to trigger the tapped event.
                    if (index < 0 || visibleDateCount <= index)
                    {
                        return;
                    }
                }
                //// Example: Condition become false while first day of week is Thursday and display date is 9999,12,25(Thursday is the day of week). In this case the startIndex is 0.
                //// Condition become true while first day of week is sunday and display date is 0001, 1, 1 with number of weeks is 6, so we need to consider the 2nd row dates.
                else if (startIndex != 0)
                {
                    // Adding the index value by 7 we can get the second row of visible dates.
                    index = index + 7;
                }
                //// Need to return the 2nd row dates while number of weeks as 6 because 1st row have trailing dates.
                //// If we consider the 6 row condition outside the if loop then it moves to 3rd row while above else if is true.
                else if (numberOfWeeks == 6)
                {
                    // Adding the index value by 7 we can get the second row of visible dates.
                    index = index + 7;
                }
            }
            //// Need to return the 2nd row dates while number of weeks as 6 because 1st row have trailing dates.
            else if (numberOfWeeks == 6)
            {
                // Adding the index value by 7 we can get the second row of visible dates.
                index = index + 7;
            }

            tappedDate = _visibleDates[index];
            _calendarInfo.TriggerCalendarInteractionEvent(isTapped, tapCount, tappedDate, CalendarElement.ViewHeader);
        }

        /// <summary>
        /// Method to update template views on view change.
        /// </summary>
        void UpdateTemplateViews()
        {
            if (_visibleDates.Count == 0)
            {
                return;
            }

            int numberOfVisibleWeeks = CalendarViewHelper.GetNumberOfWeeks(_calendarInfo.MonthView);
            int currentMonth = numberOfVisibleWeeks == 6 ? _visibleDates[_visibleDates.Count / 2].Month : _visibleDates[0].Month;
            if (_viewHeaderCells != null && _viewHeaderCells.Count != 0)
            {
                UpdateViewHeaderTemplateViews(currentMonth, numberOfVisibleWeeks);
            }
        }

        /// <summary>
        /// Method to update the view header template views.
        /// </summary>
        /// <param name="currentMonth">The current month.</param>
        /// <param name="numberOfVisibleWeeks">The number of visible weeks value.</param>
        void UpdateViewHeaderTemplateViews(int currentMonth, int numberOfVisibleWeeks)
        {
            DataTemplate viewHeaderTemplate = _calendarInfo.MonthViewHeaderTemplate;
            if (viewHeaderTemplate is DataTemplateSelector)
            {
                Children.Clear();
                //// If template selector is used then needed to create views on each time. Since selector can be decided based on date value.
                CreateViewHeaderSelectorCells(currentMonth, (DataTemplateSelector)viewHeaderTemplate, numberOfVisibleWeeks);
            }
            else
            {
                //// view header dates are calculated based on month visible dates. If first row contains leading days then second row dates will be considered for month view header dates.
                DateTime weekStartDate = CalendarViewHelper.GetMonthViewHeaderWeekStartDates(_calendarInfo, _visibleDates, currentMonth, numberOfVisibleWeeks);
                //// For date templates, binding context only updated for view header cells on view swipe.
                for (int i = 0; i < _viewHeaderCells?.Count; i++)
                {
                    var viewHeaderCell = _viewHeaderCells[i];
                    var dateTime = weekStartDate.AddDays(i);
                    viewHeaderCell.BindingContext = dateTime;
                }
            }
        }

        /// <summary>
        /// Method to create the view header cells.
        /// </summary>
        void CreateViewHeaderCells(int numberOfVisibleWeeks)
        {
            int currentMonth = _visibleDates[_visibleDates.Count / 2].Month;
            DataTemplate viewHeaderTemplate = _calendarInfo.MonthViewHeaderTemplate;

            if (viewHeaderTemplate is DataTemplateSelector)
            {
                CreateViewHeaderSelectorCells(currentMonth, (DataTemplateSelector)viewHeaderTemplate, numberOfVisibleWeeks);
            }
            else
            {
                DateTime weekStartDate = CalendarViewHelper.GetMonthViewHeaderWeekStartDates(_calendarInfo, _visibleDates, currentMonth, numberOfVisibleWeeks);
                for (int i = 0; i < 7; i++)
                {
                    CreateViewHeaderCellTemplateView(viewHeaderTemplate, weekStartDate.AddDays(i));
                }
            }
        }

        /// <summary>
        /// Method to create the view header cell using template selector.
        /// </summary>
        /// <param name="currentMonth">The current month.</param>
        /// <param name="templateSelector">The template selector.</param>
        /// <param name="numberOfVisibleWeeks">The number of visible weeks value.</param>
        void CreateViewHeaderSelectorCells(int currentMonth, DataTemplateSelector templateSelector, int numberOfVisibleWeeks)
        {
            if (templateSelector == null)
            {
                return;
            }

            DateTime weekStartDate = CalendarViewHelper.GetMonthViewHeaderWeekStartDates(_calendarInfo, _visibleDates, currentMonth, numberOfVisibleWeeks);
            for (int i = 0; i < 7; i++)
            {
                var dateTime = weekStartDate.AddDays(i);
                DataTemplate template = templateSelector.SelectTemplate(dateTime, _calendarInfo.MonthView);
                CreateViewHeaderCellTemplateView(template, dateTime);
            }
        }

        /// <summary>
        /// Method to create the view header cell template views.
        /// </summary>
        /// <param name="template">The data template.</param>
        /// <param name="details">The binding context details.</param>
        void CreateViewHeaderCellTemplateView(DataTemplate template, DateTime details)
        {
            var viewHeaderCellTemplateView = CalendarViewHelper.CreateTemplateView(template, details);
            _viewHeaderCells?.Add(viewHeaderCellTemplateView);
            Add(viewHeaderCellTemplateView);
        }

#if ANDROID
        /// <summary>
        /// Method to update template view with delay on view swipe.
        /// </summary>
        async void UpdateTemplatesWithDelay()
        {
            //// The delay is required since the template creation is started before the view swipe animation is completed for the last pixels.
            //// View creation holds the layout hence animation updated after a delay.
            //// 16ms delay is included since animation frame duration is 16ms.
            //// In Windows and Mac platforms fade animation is provided, hence delay is not visible with fade animation.
            await Task.Delay(16);
            UpdateTemplateViews();
            InvalidateViewMeasure();
        }
#endif

        #endregion

        #region Override Methods

        /// <summary>
        /// Method to draw the month view header.
        /// </summary>
        /// <param name="canvas">The draw canvas.</param>
        /// <param name="dirtyRect">The rectangle.</param>
        protected override void OnDraw(ICanvas canvas, RectF dirtyRect)
        {
            if (_calendarInfo.MonthViewHeaderTemplate != null)
            {
                return;
            }

            canvas.SaveState();
            canvas.Antialias = true;
            bool isRTL = _calendarInfo.IsRTLLayout;
            float weekNumberWidth = CalendarViewHelper.GetWeekNumberWidth(_calendarInfo.MonthView, dirtyRect.Width);
            float monthViewWidth = dirtyRect.Width - weekNumberWidth;

            // The total number of week days is 7. (Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday).
            // Month view width divided by 7 then we get the single cell width.
            // Example: Assume month view width = 210.
            // Month cell width(210) / number of days per week(7) = 30 (single cell width 30).
            float monthCellWidth = monthViewWidth / 7;

            // To get the starting x position where need render the first day of week.
            // If RTL then need to render first day of week from right to left.
            // From Example: 210(month view width) - 30 (per cell width) = 180. So the starting x position is 180.
            float xPosition = isRTL ? monthViewWidth - monthCellWidth : weekNumberWidth;

            // The cell width calculated based on RTL direction.
            float cellWidthOffset = isRTL ? -monthCellWidth : monthCellWidth;

            // To get the default or customized styles from the calendar API's for month view header.
            CalendarMonthHeaderView viewHeaderSettings = _calendarInfo.MonthView.HeaderView;
            Color viewHeaderStyleBackground = viewHeaderSettings.Background.ToColor();

            // Calendar month view header background color is changed from default color(Transparent), Then need to render the updated background color.
            if (viewHeaderStyleBackground != Colors.Transparent)
            {
                canvas.FillColor = viewHeaderStyleBackground;
                canvas.FillRectangle(0, 0, dirtyRect.Width, dirtyRect.Height);
            }

            //// To get the current or today date text style.
            //// Here we are checking whether the today highlight brush is transparent
            //// Because when the today highlight brush is transparent the today date's day also becomes transparent.
            //// So the view header text style is applied.
            CalendarTextStyle todayTextStyle = _calendarInfo.TodayHighlightBrush.ToColor() == Colors.Transparent
                ? viewHeaderSettings.TextStyle
                : new CalendarTextStyle()
                {
                    FontSize = viewHeaderSettings.TextStyle.FontSize,
                    FontFamily = viewHeaderSettings.TextStyle.FontFamily,
                    FontAttributes = viewHeaderSettings.TextStyle.FontAttributes,
                    FontAutoScalingEnabled = viewHeaderSettings.TextStyle.FontAutoScalingEnabled,
                    TextColor = _calendarInfo.TodayHighlightBrush.ToColor(),
                };

            //// Gets the first day of visible dates.
            DateTime visibleDateFirst = _visibleDates.First();
            int numberOfWeeks = _calendarInfo.MonthView.GetNumberOfWeeks();
            //// To get the current month.
            DateTime currentMonth = _visibleDates[_visibleDates.Count / 2];
            //// If first row contains leading dates then second row dates will be considered for month view header dates.
            DateTime weekStartDate = visibleDateFirst.Month != currentMonth.Month && numberOfWeeks == 6 ? visibleDateFirst.Date.AddDays(7) : visibleDateFirst;
            int visibleDateCount = 7;
            //// To check visible dates contains today date while number of weeks not equal to 6.
            bool isContainsTodayDate = numberOfWeeks != 6 && _visibleDates.Contains(DateTime.Now.Date);

            // The visible dates count not a multiple value of 7 and number of weeks then need to render the date of day of week based on the first day of week basis.
            // Example: Assume display date is 0001,01,01 first day of week is Sunday(Enum Value = 0) and number of weeks is 1.
            // From above scenario the visible dates contains from 0001,1,1 to 0001,1,6.
            // The visible dates[0] contains 0001,1,1 date and its day of week is Monday.
            // Here condition is true while visible date count is 6. Calculation 6 % 7 != 0.
            if (_visibleDates.Count % 7 != 0)
            {
                // Get start index based on the first day of week.
                int startIndex = CalendarViewHelper.GetFirstDayOfWeekDifference(visibleDateFirst, _calendarInfo.MonthView.FirstDayOfWeek, _calendarInfo.Identifier);

                // The number of week is 1 from above visible date collection example the visible dates count is 1.
                // So need to render the view header text based on respective visible date in the visible dates collection.
                // So assign the number of visible dates to visible dates count to restrict the for loop.
                if (_visibleDates.Count < 7)
                {
                    weekStartDate = visibleDateFirst;
                    visibleDateCount = _visibleDates.Count;

                    // From example the 0001,1,1 day of week is Monday. So need to render the day 1st date day of week in index value 6.
                    // The 1st month view header cell to 5th month view header cell need to render empty. So draw will start from 6th month view header cell. So need to change the xPosition value based on the valid start index.
                    xPosition += startIndex * cellWidthOffset;
                }

                // From example the start index = 1 != 0 condition true.
                // It means the first row of month cells may be null. So need to considered the view header text based on the second row dates.
                // Example: Condition become false while first day of week is Thursday and display date is 9999,12,25(Thursday is the day of week). In this case the startIndex is 0.
                // Condition become true while first day of week is sunday and display date is 0001, 1, 1 with number of weeks is 6, so we need to consider the 2nd row dates.
                else if (startIndex != 0)
                {
                    int difference = 7 - startIndex;
                    weekStartDate = weekStartDate.AddDays(difference);
                }
            }

            CultureInfo cultureInfo = CalendarViewHelper.GetCurrentUICultureInfo(_calendarInfo.Identifier);
            Globalization.Calendar calendar = CalendarViewHelper.GetCalendar(_calendarInfo.Identifier.ToString());
            string textFormat = viewHeaderSettings.TextFormat;
            //// Need to show full week day name for all calendars except Gregorian calendar while the text format is "ddddd".
            if (textFormat == "ddddd" && !CalendarViewHelper.IsGregorianCalendar(_calendarInfo.Identifier))
            {
                textFormat = "dddd";
            }

            int todayYear = calendar.GetYear(DateTime.Now);
            int todayMonth = calendar.GetMonth(DateTime.Now);
            DayOfWeek todayOfWeek = calendar.GetDayOfWeek(DateTime.Now);
            for (int i = 0; i < visibleDateCount; i++)
            {
                DateTime dateTime = weekStartDate.AddDays(i);
                CalendarTextStyle textStyle = (calendar.GetDayOfWeek(dateTime) == todayOfWeek && ((calendar.GetMonth(dateTime) == todayMonth && calendar.GetYear(dateTime) == todayYear && numberOfWeeks == 6) || isContainsTodayDate)) ? todayTextStyle : viewHeaderSettings.TextStyle;
                string dayText = dateTime.ToString(textFormat, cultureInfo);
                if (textFormat == "ddddd" && dayText.Length > 2)
                {
                    dayText = dayText.Substring(0, 2);
                }

                CalendarViewHelper.DrawText(canvas, dayText, textStyle, new RectF(xPosition, 0, monthCellWidth, dirtyRect.Height), HorizontalAlignment.Center, VerticalAlignment.Center);
                xPosition += cellWidthOffset;
            }

            canvas.RestoreState();
        }

        /// <summary>
        /// Method to create to create the semantics node for each virtual view with bounds inside the month header view.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <returns>Returns the semantics nodes.</returns>
        protected override List<SemanticsNode>? GetSemanticsNodesCore(double width, double height)
        {
            if (!_isCurrentView)
            {
                return null;
            }

            Size newSize = new Size(width, height);
            //// If semantics nodes already created with the size then return the already created.
            if (_semanticsNodes != null && _semanticsSize == newSize)
            {
                return _semanticsNodes;
            }

            _semanticsNodes = new List<SemanticsNode>();
            _semanticsSize = newSize;
            bool isRTL = _calendarInfo.IsRTLLayout;
            float weekNumberWidth = CalendarViewHelper.GetWeekNumberWidth(_calendarInfo.MonthView, (float)Width);
            float monthViewWidth = (float)Width - weekNumberWidth;
            //// The total number of week days is 7. (Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday).
            //// Month view width divided by 7 then we get the single cell width.
            //// Example: Assume month view width = 210.
            //// Month cell width(210) / number of days per week(7) = 30 (single cell width 30).
            float monthCellWidth = monthViewWidth / 7;
            //// To get the starting x position where need render the first day of week.
            //// If RTL then need to render first day of week from right to left.
            //// From Example: 210(month view width) - 30 (per cell width) = 180. So the starting x position is 180.
            float xPosition = isRTL ? monthViewWidth - monthCellWidth : weekNumberWidth;
            //// The cell width calculated based on RTL direction.
            float cellWidthOffset = isRTL ? -monthCellWidth : monthCellWidth;
            //// Gets the first day of visible dates.
            DateTime visibleDateFirst = _visibleDates.First();
            int numberOfWeeks = _calendarInfo.MonthView.GetNumberOfWeeks();
            //// To get the current month.
            DateTime currentMonth = _visibleDates[_visibleDates.Count / 2];
            //// If first row contains leading dates then second row dates will be considered for month view header dates.
            DateTime weekStartDate = visibleDateFirst.Month != currentMonth.Month && numberOfWeeks == 6 ? visibleDateFirst.Date.AddDays(7) : visibleDateFirst;
            int visibleDateCount = 7;
            //// The visible dates count not a multiple value of 7 and number of weeks then need to render the date of day of week based on the first day of week basis.
            //// Example: Assume display date is 0001,01,01 first day of week is Sunday(Enum Value = 0) and number of weeks is 1.
            //// From above scenario the visible dates contains from 0001,1,1 to 0001,1,6.
            //// The visible dates[0] contains 0001,1,1 date and its day of week is Monday.
            //// Here condition is true while visible date count is 6. Calculation 6 % 7 != 0.
            if (_visibleDates.Count % 7 != 0)
            {
                //// Get start index based on the first day of week.
                int startIndex = CalendarViewHelper.GetFirstDayOfWeekDifference(visibleDateFirst, _calendarInfo.MonthView.FirstDayOfWeek, _calendarInfo.Identifier);
                //// The number of week is 1 from above visible date collection example the visible dates count is 1.
                //// So need to render the view header text based on respective visible date in the visible dates collection.
                //// So assign the number of visible dates to visible dates count to restrict the for loop.
                if (_visibleDates.Count < 7)
                {
                    weekStartDate = visibleDateFirst;
                    visibleDateCount = _visibleDates.Count;
                    //// From example the 0001,1,1 day of week is Monday. So need to render the day 1st date day of week in index value 6.
                    //// The 1st month view header cell to 5th month view header cell need to render empty. So draw will start from 6th month view header cell. So need to change the xPosition value based on the valid start index.
                    xPosition += startIndex * cellWidthOffset;
                }

                //// From example the start index = 1 != 0 condition true.
                //// It means the first row of month cells may be null. So need to considered the view header text based on the second row dates.
                //// Example: Condition become false while first day of week is Thursday and display date is 9999,12,25(Thursday is the day of week). In this case the startIndex is 0.
                //// Condition become true while first day of week is Sunday and display date is 0001, 1, 1 with number of weeks is 6, so we need to consider the 2nd row dates.
                else if (startIndex != 0)
                {
                    int difference = 7 - startIndex;
                    weekStartDate = weekStartDate.AddDays(difference);
                }
            }

            CultureInfo cultureInfo = CalendarViewHelper.GetCurrentUICultureInfo(_calendarInfo.Identifier);
            for (int i = 0; i < visibleDateCount; i++)
            {
                DateTime dateTime = weekStartDate.AddDays(i);
                string dayText = dateTime.ToString("dddd", cultureInfo);
                SemanticsNode node = new SemanticsNode()
                {
                    Id = i,
                    Bounds = new Rect(xPosition, 0, monthCellWidth, Height),
                    Text = dayText,
                };
                _semanticsNodes.Add(node);
                xPosition += cellWidthOffset;
            }

            return _semanticsNodes;
        }

        /// <summary>
        /// Method to measure template views. Applicable only on month view header template provided.
        /// </summary>
        /// <param name="widthConstraint">The available width.</param>
        /// <param name="heightConstraint">The available height.</param>
        /// <returns>The actual size.</returns>
        protected override Size MeasureContent(double widthConstraint, double heightConstraint)
        {
            base.MeasureContent(widthConstraint, heightConstraint);
            if (_calendarInfo.MonthViewHeaderTemplate != null && _viewHeaderCells != null)
            {
                double width = double.IsFinite(widthConstraint) ? widthConstraint : 0;
                double viewHeaderHeight = CalendarViewHelper.GetViewHeaderHeight(_calendarInfo.MonthView.HeaderView);
                double weekNumberWidth = CalendarViewHelper.GetWeekNumberWidth(_calendarInfo.MonthView, (float)Width);
                double monthCellWidth = (width - weekNumberWidth) / 7;
                foreach (var child in Children)
                {
                    if (_viewHeaderCells != null && _viewHeaderCells.Count != 0 && _viewHeaderCells.Contains(child))
                    {
                        child.Measure(monthCellWidth, viewHeaderHeight);
                    }
                }
            }

            return new Size(widthConstraint, heightConstraint);
        }

        /// <summary>
        /// Method to arrange the template views. Applicable only on month view hedaer template provided.
        /// </summary>
        /// <param name="bounds">The available size.</param>
        /// <returns>The actual size.</returns>
        protected override Size ArrangeContent(Rect bounds)
        {
            if (_calendarInfo.MonthViewHeaderTemplate != null && _viewHeaderCells != null)
            {
                double weekNumberWidth = CalendarViewHelper.GetWeekNumberWidth(_calendarInfo.MonthView, (float)Width);
                double width = bounds.Width - weekNumberWidth;
                double viewHeaderHeight = CalendarViewHelper.GetViewHeaderHeight(_calendarInfo.MonthView.HeaderView);
                double monthCellWidth = width / 7;
                bool isRTL = _calendarInfo.IsRTLLayout;
                double viewHeaderCellXPosition = isRTL ? width - monthCellWidth : weekNumberWidth;
                double cellWidthOffset = isRTL ? -monthCellWidth : monthCellWidth;
                foreach (View child in Children)
                {
                    if (_viewHeaderCells != null && _viewHeaderCells.Count != 0 && _viewHeaderCells.Contains(child))
                    {
                        AbsoluteLayout.SetLayoutBounds(child, new Rect(viewHeaderCellXPosition, 0, monthCellWidth, viewHeaderHeight));
                        viewHeaderCellXPosition += cellWidthOffset;
                    }
                }
            }

            return base.ArrangeContent(bounds);
        }

        #endregion

        #region Interface Implementation

#if WINDOWS || __ANDROID__
        /// <summary>
        /// Gets a value indicating whether to handle both single tap and double tap in the view or not.
        /// </summary>
        bool IGestureListener.IsRequiredSingleTapGestureRecognizerToFail => false;
#endif

        /// <summary>
        /// Occurs on tap interaction inside month view header.
        /// </summary>
        /// <param name="e">The tap gesture listener event arguments.</param>
        void ITapGestureListener.OnTap(TapEventArgs e)
        {
            OnInteractionEvent(e.TapPoint, true, e.TapCount);
        }

        /// <summary>
        /// Occurs on double tap interaction inside month view header.
        /// </summary>
        /// <param name="e">The double-tap gesture listener event arguments.</param>
        void IDoubleTapGestureListener.OnDoubleTap(TapEventArgs e)
        {
            OnInteractionEvent(e.TapPoint, true, e.TapCount);
        }

        /// <summary>
        /// Occurs on long press action inside month view header.
        /// </summary>
        /// <param name="e">The long press listener event arguments.</param>
        void ILongPressGestureListener.OnLongPress(LongPressEventArgs e)
        {
            OnInteractionEvent(e.TouchPoint, false);
        }

        #endregion
    }
}