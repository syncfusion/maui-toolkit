#if MACCATALYST || (!ANDROID && !IOS)
using Syncfusion.Maui.Toolkit.Graphics.Internals;
using Globalization = System.Globalization;

namespace Syncfusion.Maui.Toolkit.Calendar
{
    /// <summary>
    /// Represents a class which holds hover view for month cells.
    /// </summary>
    internal class MonthHoverView : SfDrawableView
    {
        #region Fields

        /// <summary>
        /// The days per week.
        /// </summary>
        const int DaysPerWeek = 7;

        // The HighlightPadding is used for the space in between the selection shape of the cell.
        const int HighlightPadding = 2;

        /// <summary>
        /// The hovered position.
        /// </summary>
        Point? _hoverPosition;

        /// <summary>
        /// The calendar view info.
        /// </summary>
        readonly ICalendar _calendarViewInfo;

        /// <summary>
        /// The visible dates for the view.
        /// </summary>
        List<DateTime> _visibleDates;

        /// <summary>
        /// The visible dates for the view.
        /// </summary>
        List<DateTime> _disabledDates;

        /// <summary>
        /// The previous hovered date.
        /// </summary>
        DateTime? _previousHoveredDate;

        /// <summary>
        /// The selected range.
        /// </summary>
        CalendarDateRange? _selectedDateRange;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MonthHoverView"/> class.
        /// </summary>
        /// <param name="calendarViewInfo">The calendar view info.</param>
        /// <param name="visibleDates">The visible dates for the view.</param>
        /// <param name="disabledDates">The disabled dates collection.</param>
        /// <param name="selectedDateRange">The calendar date range.</param>
        internal MonthHoverView(ICalendar calendarViewInfo, List<DateTime> visibleDates, List<DateTime> disabledDates, CalendarDateRange? selectedDateRange)
        {
            _calendarViewInfo = calendarViewInfo;
            _visibleDates = visibleDates;
            _disabledDates = disabledDates;
            _selectedDateRange = selectedDateRange;
            InputTransparent = true;
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Method to update the selected range.
        /// </summary>
        /// <param name="selectedDateRange">The calendar date range.</param>
        internal void UpdateSelectedDateRange(CalendarDateRange? selectedDateRange)
        {
            _selectedDateRange = selectedDateRange;
            //// Does not need to update the UI while selected range value changes on
            //// single and multiple selection and default range selection.
            if (_calendarViewInfo.SelectionMode != CalendarSelectionMode.Range || _calendarViewInfo.RangeSelectionDirection == CalendarRangeSelectionDirection.Default)
            {
                return;
            }

            InvalidateDrawable();
        }

        /// <summary>
        /// Method to update the hover position.
        /// </summary>
        /// <param name="hoverPosition">The hover position.</param>
        internal void UpdateHoverPosition(Point? hoverPosition)
        {
            _hoverPosition = hoverPosition;
            DateTime? hoveredDate = _hoverPosition == null ? null : CalendarViewHelper.GetMonthDateFromPosition(_hoverPosition.Value, Width, Height, _calendarViewInfo, _visibleDates);
            int numberOfWeeks = CalendarViewHelper.GetActualNumberOfWeeks(_calendarViewInfo, _visibleDates);
            //// Condition to check whether the interaction is enabled for hovered date.
            if (hoveredDate != null && !CalendarViewHelper.IsInteractableDate(hoveredDate.Value, _disabledDates, _visibleDates, _calendarViewInfo, numberOfWeeks))
            {
                hoveredDate = null;
            }

            if (_previousHoveredDate == hoveredDate)
            {
                return;
            }

            InvalidateDrawable();
        }

        /// <summary>
        /// Method to update when visible dates changed.
        /// </summary>
        /// <param name="visibleDates">The visible dates.</param>
        internal void UpdateVisibleDatesChange(List<DateTime> visibleDates)
        {
            _visibleDates = visibleDates;
            //// On view navigation, the mouse hover position will remain same but visible dates will be changed.
            //// If we not update the hover position, the hovering view will be remain even after view changed.
            UpdateHoverPosition(null);
        }

        /// <summary>
        /// Method to update the disabled dates for ho
        /// </summary>
        /// <param name="disabledDates">The disabled dates.</param>
        internal void UpdateDisabledDates(List<DateTime> disabledDates)
        {
            //// Here there is no need to update the hovering to null because we have to update the hovering
            //// before checking whether the previous disabled dates are equal to the current disabled dates.
            _disabledDates = disabledDates;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Method to draw the start range dash line.
        /// </summary>
        /// <param name="canvas">The draw canvas.</param>
        /// <param name="highlightBounds">The highlight bounds position values</param>
        /// <param name="leftXPosition">The left position of month cell(x position + month cell width / 2).</param>
        /// <param name="rightXPosition">The right position of month cell(x position + month cell width).</param>
        /// <param name="circleTopPadding">The circle top padding value.</param>
        /// <param name="circleSize">The circle size.</param>
        void DrawStartDashLine(ICanvas canvas, RectF highlightBounds, float leftXPosition, float rightXPosition, float circleTopPadding, float circleSize)
        {
            float firstYPosition;
            float secondYPosition;

            // The first dash line draw from top of the circle.
            // The second dash line draw from bottom of the circle.
            // The dash line draw from center position of the month cell width to right side end of the month cell.
            //// Example: Stroke size is 10 and rectangle bound is 0,0,100,100
            //// The drawn rectangle will show like -5,-5,105,105. Here 105 is the value of width and height not right(105) and bottom(105) position.
            //// So we need to draw the rectangle 5,5,95,95. Here 95 is the value of width and height not right(95) and bottom(95) position.
            float strokeThickness = 1;
            canvas.StrokeSize = strokeThickness;
            float strokeLinePadding = strokeThickness / 2;

            if (_calendarViewInfo.SelectionShape == CalendarSelectionShape.Rectangle)
            {
                // The highlight padding is used to differentiate between the month cell the rectangle and selection highlight.
                firstYPosition = circleTopPadding + strokeLinePadding + HighlightPadding;
                secondYPosition = highlightBounds.Bottom - strokeLinePadding - HighlightPadding;
                leftXPosition = highlightBounds.Right - HighlightPadding;
                rightXPosition = highlightBounds.Right;
            }
            else
            {
                firstYPosition = circleTopPadding + strokeLinePadding;
                secondYPosition = circleTopPadding + circleSize - strokeLinePadding;
            }

            canvas.DrawLine(leftXPosition, firstYPosition, rightXPosition, firstYPosition);
            canvas.DrawLine(leftXPosition, secondYPosition, rightXPosition, secondYPosition);
        }

        /// <summary>
        /// Method to draw the end range dash line.
        /// </summary>
        /// <param name="canvas">The draw canvas.</param>
        /// <param name="highlightBounds">The highlight bounds position values</param>
        /// <param name="leftXPosition">The left position of momth cell.</param>
        /// <param name="rightXPosition">The right position of month cell(x position + month cell width / 2).</param>
        /// <param name="circleTopPadding">The circle top padding value.</param>
        /// <param name="circleSize">The circle size.</param>
        void DrawEndDashLine(ICanvas canvas, RectF highlightBounds, float leftXPosition, float rightXPosition, float circleTopPadding, float circleSize)
        {
            float firstYPosition;
            float secondYPosition;

            // The first dash line draw from top of the circle.
            // The second dash line draw from bottom of the circle.
            // The dash line draw from starting xPosition to center position of the month cell width.
            //// Example: Stroke size is 10 and rectangle bound is 0,0,100,100
            //// The drawn rectangle will show like -5,-5,105,105. Here 105 is the value of width and height not right(105) and bottom(105) position.
            //// So we need to draw the rectangle 5,5,95,95. Here 95 is the value of width and height not right(95) and bottom(95) position.
            float strokeThickness = 1;
            canvas.StrokeSize = strokeThickness;
            float strokeLinePadding = strokeThickness / 2;

            if (_calendarViewInfo.SelectionShape == CalendarSelectionShape.Rectangle)
            {
                // The highlight padding is used to differentiate between the month cell the rectangle and selection highlight.
                firstYPosition = circleTopPadding + strokeLinePadding + HighlightPadding;
                secondYPosition = highlightBounds.Bottom - strokeLinePadding - HighlightPadding;
                leftXPosition = highlightBounds.X;
                rightXPosition = highlightBounds.X + HighlightPadding;
            }
            else
            {
                firstYPosition = circleTopPadding + strokeLinePadding;
                secondYPosition = circleTopPadding + circleSize - strokeLinePadding;
            }

            canvas.DrawLine(leftXPosition, firstYPosition, rightXPosition, firstYPosition);
            canvas.DrawLine(leftXPosition, secondYPosition, rightXPosition, secondYPosition);
        }

        /// <summary>
        /// Method to draw the hovering highlight.
        /// </summary>
        /// <param name="canvas">The draw canvas.</param>
        /// <param name="monthCellWidth">The month cell width.</param>
        /// <param name="monthCellHeight">The month cell height.</param>
        /// <param name="monthViewWidth">The month view width.</param>
        /// <param name="monthViewHeight">The moth view height.</param>
        /// <param name="weekNumberWidth">The week number width.</param>
        /// <param name="numberOfVisibleWeeks">The number of visible weeks.</param>
        /// <param name="isRTL">The flow direction is RTl or not.</param>
        /// <param name="hoverPosition">The hovering position.</param>
        void DrawHover(ICanvas canvas, float monthCellWidth, float monthCellHeight, float monthViewWidth, float monthViewHeight, float weekNumberWidth, int numberOfVisibleWeeks, bool isRTL, Point hoverPosition)
        {
            float xPosition = (float)(isRTL ? monthViewWidth - hoverPosition.X : hoverPosition.X - weekNumberWidth);
            float yPosition = (float)hoverPosition.Y;
            if (xPosition < 0 || yPosition < 0)
            {
                return;
            }

            int row = (int)(yPosition / (monthViewHeight / numberOfVisibleWeeks));
            int column = (int)(xPosition / (monthViewWidth / DaysPerWeek));
            if (row >= numberOfVisibleWeeks || column >= DaysPerWeek)
            {
                return;
            }

            //// In RTL flow direction, day position will be calculated from reverse hence (DaysPerWeek - 1 = 6) is used to find the cell position in reverse.
            xPosition = isRTL ? (DaysPerWeek - 1 - column) * monthCellWidth : weekNumberWidth + (column * monthCellWidth);
            yPosition = row * monthCellHeight;
            //// Corner radius for hovering when the selection shape is rectangle.
            float cornerRadius = (float)(monthCellHeight * 0.1);
            cornerRadius = cornerRadius > 2 ? 2 : cornerRadius;

            // The actual cell bounds. Based on this cell rectangle the selection highlight, selection range, extendable range hovering today highlight will draw.
            RectF highlightBounds = new RectF(xPosition, yPosition, monthCellWidth, monthCellHeight);

            // The center position contains centerXPosition and centerYPosition of the month cell.
            PointF centerPosition = highlightBounds.Center;
            float selectionRadius = monthCellWidth > monthCellHeight ? (monthCellHeight / 2) - HighlightPadding : (monthCellWidth / 2) - HighlightPadding;
            DrawHoverHighlight(canvas, highlightBounds, selectionRadius, cornerRadius, centerPosition);
        }

        /// <summary>
        /// Method to draw the range selection hover.
        /// </summary>
        /// <param name="canvas">The draw canvas.</param>
        /// <param name="monthCellWidth">The month cell width.</param>
        /// <param name="monthCellHeight">The month cell height.</param>
        /// <param name="monthViewWidth">The month view width.</param>
        /// <param name="weekNumberWidth">The week number width.</param>
        /// <param name="numberOfVisibleWeeks">The number of visible weeks.</param>
        /// <param name="isRTL">The flow direction is RTl or not.</param>
        void DrawRangeSelectionHover(ICanvas canvas, float monthCellWidth, float monthCellHeight, float monthViewWidth, float weekNumberWidth, int numberOfVisibleWeeks, bool isRTL)
        {
            DateTime currentMonth = _visibleDates[_visibleDates.Count / 2];
            //// xPosition is used to define the starting position for drawing the cells.
            //// Example: Let total width of the month view = 70 and month cell width = 10.
            //// If the flow direction is RTL then it should draw the cells from the right side of the view.
            //// xPosition = 70 - 10 = 60.
            //// Else the cells drawn from the left side of the view.
            float xPosition = isRTL ? monthViewWidth - monthCellWidth : weekNumberWidth;
            float yPosition = 0;
            //// Cell width offset is used to define the drawing position of the cell based on the flow direction and the month cell width.
            //// Example: Let the total width of the month view = 70 and month cell width = 10.
            //// RTL - xPosition = 70 - 10 = 60
            //// xPosition = 60 - 10 = 50
            //// LTR - xPosition = 0
            //// xPosition = 0 + 10 = 10
            float cellWidthOffset = isRTL ? -monthCellWidth : monthCellWidth;
            //// Boolean to check whether the view is month view or not.
            //// If the number of weeks is less than 6 then the view is not considered as a month view.
            //// Leading and trailing dates can't be hide when the view is not month view.
            bool isMonthView = numberOfVisibleWeeks == 6;
            float cornerRadius = (float)(monthCellHeight * 0.1);
            cornerRadius = cornerRadius > 2 ? 2 : cornerRadius;
            //// The HighlightPadding is used for the space around the circle from the cell.
            float selectionRadius = monthCellWidth > monthCellHeight ? (monthCellHeight / 2) - HighlightPadding : (monthCellWidth / 2) - HighlightPadding;

            // The circle size. Example: radius = 25.
            // The circleSize = 2 * 25 = 50(circleSize).
            float circleSize = 2 * selectionRadius;

            // This is the valid start index for to fetch the visible date from the visible date collection.
            int startIndex = 0;
            //// The number of dates in the collection.
            int visibleDateCount = _visibleDates.Count;

            // The visible dates count not a multiple value of 7 and number of weeks then need to render the date based on the first day of week basis.
            // Example: Assume display date is 0001,01,01 first day of week is Tuesday(Enum Value = 2) and number of weeks is 2.
            // From above scenario the visible dates contains from 0001,1,1 to 0001,1,8.
            // The visible dates[0] = (0001,1,1) and first date of day of week = Monday.
            // Here condition is true while visible date count is 8. Calculation 8 % 7 != 0.
            if (visibleDateCount % 7 != 0)
            {
                DateTime currentDate = _visibleDates.First();
                startIndex = CalendarViewHelper.GetFirstDayOfWeekDifference(currentDate, _calendarViewInfo.MonthView.FirstDayOfWeek, _calendarViewInfo.Identifier);

                // From example the 0001,1,1 day of week is Monday. So need to render the 1st date in index value 5. Because of the first date of day week text is rendered in index value 5.
                // The 0th index to 4th-index need to render empty. So draw will start from 5th-index. So need to change the xPosition value based on the valid start index.
                xPosition += startIndex * cellWidthOffset;
            }

            Globalization.Calendar cultureCalendar = CalendarViewHelper.GetCalendar(_calendarViewInfo.Identifier.ToString());
            int currentMonthYear = cultureCalendar.GetMonth(currentMonth);
            for (int i = 0; i < visibleDateCount; i++)
            {
                //// From example the start index value 6.
                int index = startIndex + i;
                DateTime dateTime = _visibleDates[i];
                //// If date arrangement reached to last column then need to start from next row and first column.
                if (index % DaysPerWeek == 0 && index != 0)
                {
                    xPosition = isRTL ? monthViewWidth - monthCellWidth : weekNumberWidth;
                    yPosition += monthCellHeight;
                }

                bool isLeadingAndTrailingDates = cultureCalendar.GetMonth(dateTime) != currentMonthYear && isMonthView;
                //// If ShowLeadingAndTrailingDates set to false no need to draw leading and trailing view.
                //// But when number of weeks is less than 6, leading and trailing view will draw even ShowLeadingAndTrailingDates set to false.
                if (isLeadingAndTrailingDates && !_calendarViewInfo.ShowTrailingAndLeadingDates)
                {
                    xPosition += cellWidthOffset;
                    continue;
                }

                bool isBlackoutDate = CalendarViewHelper.IsDateInDateCollection(dateTime, _disabledDates);
                bool isDisabledDate = CalendarViewHelper.IsDisabledDate(dateTime, _calendarViewInfo.View, _calendarViewInfo.EnablePastDates, _calendarViewInfo.MinimumDate, _calendarViewInfo.MaximumDate, _calendarViewInfo.SelectionMode, _calendarViewInfo.RangeSelectionDirection, _selectedDateRange, _calendarViewInfo.AllowViewNavigation, _calendarViewInfo.Identifier);

                // The actual cell bounds. Based on this cell rectangle the selection highlight, selection range, extendable range hovering today highlight will draw.
                RectF highlightBounds = new RectF(xPosition, yPosition, monthCellWidth, monthCellHeight);

                // The center position contains centerXPosition and centerYPosition of the month cell.
                PointF centerPosition = highlightBounds.Center;

                // Assume the centerPosition.Y = 150. selectionRadius = 25.
                // The circleTopPadding = 150 - 25 = 125(circleTopPadding). The draw will start from the y axis with position value of 125.
                float circleTopPadding = _calendarViewInfo.SelectionShape == CalendarSelectionShape.Rectangle ? highlightBounds.Top : centerPosition.Y - selectionRadius;
                if (!isDisabledDate && !isBlackoutDate)
                {
                    // To find the range selection is start range or end range or in between range or not.
                    SelectedRangeStatus? rangeSelectionStatus = RangeHoveringStatus(dateTime);
                    if (rangeSelectionStatus != null)
                    {
                        DrawRangeHovering(canvas, highlightBounds, rangeSelectionStatus, isRTL, selectionRadius, cornerRadius, circleSize, circleTopPadding, centerPosition);
                    }
                }

                xPosition += cellWidthOffset;
            }
        }

        /// <summary>
        /// Method to draw the hover selection highlight.
        /// </summary>
        /// <param name="canvas">The draw canvas.</param>
        /// <param name="highlightBounds">The cell rectangle hold the current month cell bound(x position, y position, width, height).</param>
        /// <param name="selectionRadius">The selection radius of the circle.</param>
        /// <param name="cornerRadius">The corner radius.</param>
        /// <param name="centerPosition">The center position of the month cell.</param>
        void DrawHoverHighlight(ICanvas canvas, RectF highlightBounds, float selectionRadius, float cornerRadius, PointF centerPosition)
        {
            if (_calendarViewInfo.SelectionShape == CalendarSelectionShape.Rectangle)
            {
                RectF rectF = new RectF(highlightBounds.Left + HighlightPadding, highlightBounds.Top + HighlightPadding, highlightBounds.Width - (2 * HighlightPadding), highlightBounds.Height - (2 * HighlightPadding));
                canvas.FillRoundedRectangle(rectF, cornerRadius);
            }
            else
            {
                canvas.FillCircle(centerPosition, selectionRadius);
            }
        }

        /// <summary>
        /// Method to draw the range selection hover based on range selection status.
        /// </summary>
        /// <param name="canvas">The draw canvas.</param>
        /// <param name="highlightBounds">The cell rectangle hold the current month cell bound(x position, y position, width, height).</param>
        /// <param name="rangeSelectionStatus">The status of forward range selection status.</param>
        /// <param name="isRTL">The flow direction is RTL or not.</param>
        /// <param name="selectionRadius">The selection radius of circle.</param>
        /// <param name="cornerRadius">The corner radius.</param>
        /// <param name="circleSize">The size of the circle.</param>
        /// <param name="circleTopPadding">The starting position of the month cell height.</param>
        /// <param name="centerPosition">The center position of the month cell.</param>
        void DrawRangeHovering(ICanvas canvas, RectF highlightBounds, SelectedRangeStatus? rangeSelectionStatus, bool isRTL, float selectionRadius, float cornerRadius, float circleSize, float circleTopPadding, PointF centerPosition)
        {
            if (rangeSelectionStatus == SelectedRangeStatus.SelectedRange)
            {
                DrawHoverHighlight(canvas, highlightBounds, selectionRadius, cornerRadius, centerPosition);
            }
            else if (rangeSelectionStatus == SelectedRangeStatus.HoverStartDate)
            {
                if (isRTL)
                {
                    DrawEndDashLine(canvas, highlightBounds, highlightBounds.Left, centerPosition.X, circleTopPadding, circleSize);
                }
                else
                {
                    DrawStartDashLine(canvas, highlightBounds, centerPosition.X, highlightBounds.Right, circleTopPadding, circleSize);
                }
            }
            else if (rangeSelectionStatus == SelectedRangeStatus.StartRange)
            {
                if (isRTL)
                {
                    DrawEndRangeHovering(canvas, highlightBounds, circleTopPadding, cornerRadius, circleSize, selectionRadius, centerPosition);
                }
                else
                {
                    DrawStartRangeHovering(canvas, highlightBounds, circleTopPadding, cornerRadius, circleSize, selectionRadius, centerPosition);
                }
            }
            else if (rangeSelectionStatus == SelectedRangeStatus.EndRange)
            {
                if (isRTL)
                {
                    DrawStartRangeHovering(canvas, highlightBounds, circleTopPadding, cornerRadius, circleSize, selectionRadius, centerPosition);
                }
                else
                {
                    DrawEndRangeHovering(canvas, highlightBounds, circleTopPadding, cornerRadius, circleSize, selectionRadius, centerPosition);
                }
            }
            else if (rangeSelectionStatus == SelectedRangeStatus.InBetweenRange)
            {
                float firstYPosition;
                float secondYPosition;
                //// Example: Stroke size is 10 and rectangle bound is 0,0,100,100
                //// The drawn rectangle will show like -5,-5,110,110. Here 110 is the value of width and height not right(105) and bottom(105) position.
                //// So we need to draw the rectangle 5,5,90,90. Here 90 is the value of width and height not right(95) and bottom(95) position.
                float strokeThickness = 1;
                canvas.StrokeSize = strokeThickness;
                float strokeLinePadding = strokeThickness / 2;
                if (_calendarViewInfo.SelectionShape == CalendarSelectionShape.Rectangle)
                {
                    // The highlight padding is used to differentiate between the month cell the rectangle and selection highlight.
                    firstYPosition = circleTopPadding + strokeLinePadding + HighlightPadding;
                    secondYPosition = highlightBounds.Bottom - strokeLinePadding - HighlightPadding;
                }
                else
                {
                    firstYPosition = circleTopPadding + strokeLinePadding;
                    secondYPosition = circleTopPadding + circleSize - strokeLinePadding;
                }

                // The dash line are draw based on the circle size.
                // The dash line draw from the xPosition of the month cell to end of the month cell.
                canvas.DrawLine(highlightBounds.Left, firstYPosition, highlightBounds.Right, firstYPosition);
                canvas.DrawLine(highlightBounds.Left, secondYPosition, highlightBounds.Right, secondYPosition);
            }
            else
            {
                if (isRTL)
                {
                    DrawStartDashLine(canvas, highlightBounds, centerPosition.X, highlightBounds.Right, circleTopPadding, circleSize);
                }
                else
                {
                    DrawEndDashLine(canvas, highlightBounds, highlightBounds.Left, centerPosition.X, circleTopPadding, circleSize);
                }
            }
        }

        /// <summary>
        /// Method to draw end range dash line with the selection highlight hovering.
        /// </summary>
        /// <param name="canvas">The draw canvas.</param>
        /// <param name="highlightBounds">The highlight bounds hold the current month cell bound(x position, y position, width, height).</param>
        /// <param name="circleTopPadding">The starting position of Y-axis.</param>
        /// <param name="cornerRadius">The corner radius.</param>
        /// <param name="circleSize">The circle size.</param>
        /// <param name="selectionRadius">The selection radius of circle.</param>
        /// <param name="centerPosition">The center position of the month cell.</param>
        void DrawEndRangeHovering(ICanvas canvas, RectF highlightBounds, float circleTopPadding, float cornerRadius, float circleSize, float selectionRadius, PointF centerPosition)
        {
            if (_calendarViewInfo.SelectionShape == CalendarSelectionShape.Circle)
            {
                DrawEndDashLine(canvas, highlightBounds, highlightBounds.Left, centerPosition.X, circleTopPadding, circleSize);
                canvas.FillCircle(centerPosition, selectionRadius);
            }
            else
            {
                RectF rectF = new RectF(highlightBounds.Left + HighlightPadding, highlightBounds.Top + HighlightPadding, highlightBounds.Width - (2 * HighlightPadding), highlightBounds.Height - (2 * HighlightPadding));

                // In below method the Last four arguments are denotes cornerRadius of the rectangle.
                // 0,cornerRadius, 0, cornerRadius.
                // First argument denotes the topLeftCornerRadius.
                // Second argument denotes topRightCornerRadius.
                // Third argument denotes bottomLeftCornerRadius.
                // Fourth argument denotes bottomRightCornerRadius.
                DrawEndDashLine(canvas, highlightBounds, highlightBounds.Left, centerPosition.X, circleTopPadding, circleSize);
                canvas.FillRoundedRectangle(rectF, 0, cornerRadius, 0, cornerRadius);
            }
        }

        /// <summary>
        /// Method to draw the start range dash line with selection highlight hovering.
        /// </summary>
        /// <param name="canvas">The draw canvas.</param>
        /// <param name="highlightBounds">The cell rect hold the current month cell bound(x position, y position, width, height).</param>
        /// <param name="circleTopPadding">The starting position of Y-axis.</param>
        /// <param name="cornerRadius">The corner radius.</param>
        /// <param name="circleSize">The circle size.</param>
        /// <param name="selectionRadius">The selection radius of circle.</param>
        /// <param name="centerPosition">The center position of the month cell.</param>
        void DrawStartRangeHovering(ICanvas canvas, RectF highlightBounds, float circleTopPadding, float cornerRadius, float circleSize, float selectionRadius, PointF centerPosition)
        {
            if (_calendarViewInfo.SelectionShape == CalendarSelectionShape.Circle)
            {
                DrawStartDashLine(canvas, highlightBounds, centerPosition.X, highlightBounds.Right, circleTopPadding, circleSize);
                canvas.FillCircle(centerPosition, selectionRadius);
            }
            else
            {
                RectF rectF = new RectF(highlightBounds.Left + HighlightPadding, highlightBounds.Top + HighlightPadding, highlightBounds.Width - (2 * HighlightPadding), highlightBounds.Height - (2 * HighlightPadding));

                // In below method the Last four arguments are denotes cornerRadius of the rectangle.
                // cornerRadius, 0, cornerRadius,0.
                // First argument denotes the topLeftCornerRadius.
                // Second argument denotes topRightCornerRadius.
                // Third argument denotes bottomLeftCornerRadius.
                // Fourth argument denotes bottomRightCornerRadius.
                DrawStartDashLine(canvas, highlightBounds, centerPosition.X, highlightBounds.Right, circleTopPadding, circleSize);
                canvas.FillRoundedRectangle(rectF, cornerRadius, 0, cornerRadius, 0);
            }
        }

        /// <summary>
        /// Method get the extendable range selection status based on the calendar range selection direction.
        /// </summary>
        /// <param name="dateTime">The date.</param>
        /// <returns>The hover status.</returns>
        SelectedRangeStatus? RangeHoveringStatus(DateTime? dateTime)
        {
            DateTime? startDate = _selectedDateRange?.StartDate?.Date;
            DateTime? endDate = _selectedDateRange?.EndDate == null ? startDate : _selectedDateRange?.EndDate;

            // Example: Assume Range(Sep-10 to Sep-15). Explained the below scenarios based on this range.
            // The hovered date in between the Sep-10 to Sep-15 that hovered date are considered as SelectedRange.
            // The selected range is null or endRangeDate null then the hovered date is considered as SelectedRange.
            if (_previousHoveredDate?.Date == dateTime?.Date && ((startDate <= _previousHoveredDate?.Date && _previousHoveredDate?.Date <= endDate) || _selectedDateRange == null || startDate == null || endDate == null))
            {
                return SelectedRangeStatus.SelectedRange;
            }

            // The hovered date is after Sep-15. And endRange equal to date time then endRangedate considered as HoverStartDate.
            else if (endDate == dateTime?.Date && _previousHoveredDate?.Date > endDate)
            {
                return SelectedRangeStatus.HoverStartDate;
            }

            // The hovered date is Sep-16. The hovered date and date time are equal then hovered date will considered as end range.
            else if (_previousHoveredDate?.Date == dateTime?.Date && _previousHoveredDate?.Date > endDate)
            {
                return SelectedRangeStatus.EndRange;
            }

            // The date is start date and hovered date is Sep-09 then start date considered as HoverEndDate.
            else if (startDate == dateTime?.Date && _previousHoveredDate?.Date < startDate)
            {
                return SelectedRangeStatus.HoverEndDate;
            }

            // The hovered date is equal to datetime and hovered date is lesser than Sep-09 then hovered date considered as StartRange.
            else if (_previousHoveredDate?.Date == dateTime?.Date && _previousHoveredDate?.Date < startDate)
            {
                return SelectedRangeStatus.StartRange;
            }

            // Here hovered date is Sep-07 then the Sep-08, Sep-09 dates are InbetweenRange.
            // Here hovered Sep-18 then the Sep-16,17 are InbetweenRange.
            else if ((dateTime?.Date < _previousHoveredDate && endDate < dateTime?.Date) || (_previousHoveredDate?.Date < dateTime?.Date && dateTime?.Date < startDate))
            {
                return SelectedRangeStatus.InBetweenRange;
            }

            return null;
        }

        #endregion

        #region Override Methods

        /// <summary>
        /// Method to draw the month view.
        /// </summary>
        /// <param name="canvas">The draw canvas.</param>
        /// <param name="dirtyRect">The rectangle.</param>
        protected override void OnDraw(ICanvas canvas, RectF dirtyRect)
        {
            if (_hoverPosition == null)
            {
                _previousHoveredDate = null;
                return;
            }

            int numberOfWeeks = CalendarViewHelper.GetActualNumberOfWeeks(_calendarViewInfo, _visibleDates);
            _previousHoveredDate = CalendarViewHelper.GetMonthDateFromPosition(_hoverPosition.Value, dirtyRect.Width, dirtyRect.Height, _calendarViewInfo, _visibleDates);
            //// Condition to check whether the interaction is enabled for hovered date.
            if (_previousHoveredDate == null || !CalendarViewHelper.IsInteractableDate(_previousHoveredDate.Value, _disabledDates, _visibleDates, _calendarViewInfo, numberOfWeeks))
            {
                return;
            }

            bool isRTL = _calendarViewInfo.IsRTLLayout;
            float weekNumberWidth = CalendarViewHelper.GetWeekNumberWidth(_calendarViewInfo.MonthView, dirtyRect.Width);
            float monthViewWidth = dirtyRect.Width - weekNumberWidth;
            float monthViewHeight = dirtyRect.Height;
            float monthCellWidth = monthViewWidth / DaysPerWeek;
            float monthCellHeight = monthViewHeight / numberOfWeeks;
            Color hoverBackground = CalendarViewHelper.GetMouseHoverColor(_calendarViewInfo.SelectionBackground, _calendarViewInfo.MonthView.CellTemplate);
            Color hoverDashlineColor = CalendarViewHelper.GetMouseHoverDashlineColor(_calendarViewInfo.SelectionBackground);
            if (_calendarViewInfo.SelectionBackground != null && _calendarViewInfo.HoverColor != null)
            {
                hoverBackground = CalendarViewHelper.GetMouseHoverColor(_calendarViewInfo.HoverColor, _calendarViewInfo.MonthView.CellTemplate);
                hoverDashlineColor = CalendarViewHelper.GetMouseHoverDashlineColor(_calendarViewInfo.HoverColor);
            }

            canvas.SaveState();
            canvas.Antialias = true;

            // The stroke dash pattern is used to draw a line in dash(- - -) pattern.
            // The new float[] {5,5} values that indicate the pattern of dashes and gaps that are to be used when drawing the stroke.
            // Because of the range selection is extendable so the lines are rendered like dash line with 5,5 value. Purpose of this value 5,5 is good UI.
            // The first item in the array specifies the length of a dash, while the second item in the array specifies the length of a gap.
            canvas.StrokeDashPattern = new float[] { 5, 5 };
            canvas.FillColor = hoverBackground;
            canvas.StrokeColor = hoverDashlineColor;
            switch (_calendarViewInfo.SelectionMode)
            {
                case CalendarSelectionMode.Single:
                case CalendarSelectionMode.Multiple:
                case CalendarSelectionMode.MultiRange:
                    DrawHover(canvas, monthCellWidth, monthCellHeight, monthViewWidth, monthViewHeight, weekNumberWidth, numberOfWeeks, isRTL, _hoverPosition.Value);
                    break;
                case CalendarSelectionMode.Range:
                    if (_calendarViewInfo.MonthView.CellTemplate != null)
                    {
                        DrawHover(canvas, monthCellWidth, monthCellHeight, monthViewWidth, monthViewHeight, weekNumberWidth, numberOfWeeks, isRTL, _hoverPosition.Value);
                        break;
                    }

                    switch (_calendarViewInfo.RangeSelectionDirection)
                    {
                        case CalendarRangeSelectionDirection.Default:
                            DrawHover(canvas, monthCellWidth, monthCellHeight, monthViewWidth, monthViewHeight, weekNumberWidth, numberOfWeeks, isRTL, _hoverPosition.Value);
                            break;
                        case CalendarRangeSelectionDirection.Forward:
                        case CalendarRangeSelectionDirection.Backward:
                        case CalendarRangeSelectionDirection.Both:
                        case CalendarRangeSelectionDirection.None:
                            DrawRangeSelectionHover(canvas, monthCellWidth, monthCellHeight, monthViewWidth, weekNumberWidth, numberOfWeeks, isRTL);
                            break;
                    }

                    break;
            }

            canvas.RestoreState();
        }

        #endregion

    }
}
#endif