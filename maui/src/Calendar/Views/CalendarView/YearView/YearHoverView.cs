#if MACCATALYST || (!ANDROID && !IOS)
using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.Calendar
{
    /// <summary>
    /// Represents a class which holds hover view for year view cells.
    /// </summary>
    internal class YearHoverView : SfDrawableView
    {
        #region Fields

        /// <summary>
        /// Year view panel row count
        /// </summary>
        const int RowCount = 4;

        /// <summary>
        /// Year view panel column count
        /// </summary>
        const int ColumnCount = 3;

        /// <summary>
        /// Visible dates Collections holds the Year view dates.
        /// </summary>
        List<DateTime> _visibleDates;

        /// <summary>
        /// Calendar Year view info that holds properties.
        /// </summary>
        readonly ICalendarYear _calendarViewInfo;

        /// <summary>
        /// The previous hovered date.
        /// </summary>
        DateTime? _previousHoveredDate;

        /// <summary>
        /// The hovered position.
        /// </summary>
        Point? _hoverPosition;

        /// <summary>
        /// The visible dates for the view.
        /// </summary>
        List<DateTime> _disabledDates;

        /// <summary>
        /// The selected date range of the calendar
        /// </summary>
        CalendarDateRange? _selectedDateRange;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="YearHoverView"/> class.
        /// </summary>
        /// <param name="calendarViewInfo">The calendar year view info.</param>
        /// <param name="visibleDates">The visible dates collection.</param>
        /// <param name="disabledDates">The disabled dates collection.</param>
        /// <param name="selectedDateRange">The selected date range</param>
        internal YearHoverView(ICalendarYear calendarViewInfo, List<DateTime> visibleDates, List<DateTime> disabledDates, CalendarDateRange? selectedDateRange)
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
        /// Method to update hover position.
        /// </summary>
        /// <param name="hoverPosition">The hover position.</param>
        internal void UpdateHoverPosition(Point? hoverPosition)
        {
            _hoverPosition = hoverPosition;
            DateTime? hoveredDate = _hoverPosition == null ? null : CalendarViewHelper.GetYearDateFromPosition(_hoverPosition.Value, Width, Height, _calendarViewInfo, _visibleDates);
            //// Condition to check whether the interaction is enabled for hovered date.
            if (hoveredDate != null && !CalendarViewHelper.IsInteractableDate(hoveredDate.Value, _disabledDates, _visibleDates, _calendarViewInfo, 0))
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
        /// Method to update disabled dates for year hover view.
        /// </summary>
        /// <param name="disabledDates">The disabled dates for view.</param>
        internal void UpdateDisabledDates(List<DateTime> disabledDates)
        {
            //// Here there is no need to update the hovering to null because we have to update the hovering
            //// before checking whether the previous disabled dates are equal to the current disabled dates.
            _disabledDates = disabledDates;
        }

        /// <summary>
        /// Method to update the selected range.
        /// </summary>
        /// <param name="selectedDateRange">The calendar date range.</param>
        internal void UpdateSelectedDateRange(CalendarDateRange? selectedDateRange)
        {
            _selectedDateRange = selectedDateRange;
            //// Does not need to update the UI while selected range value changes on
            //// single and multiple selection mode and in default range selection direction.
            if (_calendarViewInfo.SelectionMode != CalendarSelectionMode.Range || _calendarViewInfo.RangeSelectionDirection == CalendarRangeSelectionDirection.Default || _calendarViewInfo.AllowViewNavigation)
            {
                return;
            }

            InvalidateDrawable();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Method to draw the hovering for Range selection mode(Forward, Backward, Both and None)
        /// </summary>
        /// <param name="canvas">The draw canvas.</param>
        /// <param name="isRTL">Bool value for RTL flow direction</param>
        /// <param name="yearCellSize">Year cell width and height value</param>
        /// <param name="yearViewWidth">The year view width</param>
        /// <param name="highlightSize">Highlight width and height value</param>
        /// <param name="highlightPadding">Highlight horizontal and vertical padding value</param>
        /// <param name="rectCornerRadius">Rectangle corner radius value</param>
        /// <param name="circleCornerRadius">Circle corner radius value</param>
        void DrawRangeHighlight(ICanvas canvas, bool isRTL, SizeF yearCellSize, float yearViewWidth, SizeF highlightSize, SizeF highlightPadding, double rectCornerRadius, double circleCornerRadius)
        {
            float yearCellWidth = yearCellSize.Width;
            float yearCellHeight = yearCellSize.Height;
            float horizontalPadding = highlightPadding.Width;
            float verticalHighlightPadding = highlightPadding.Height;
            float cellWidthOffset = isRTL ? -yearCellWidth : yearCellWidth;
            canvas.StrokeColor = CalendarViewHelper.GetMouseHoverDashlineColor(_calendarViewInfo.SelectionBackground);
            if (_calendarViewInfo.SelectionBackground != null && _calendarViewInfo.HoverColor != null)
            {
                canvas.StrokeColor = CalendarViewHelper.GetMouseHoverDashlineColor(_calendarViewInfo.HoverColor);
            }

            float strokeSize = 1;
            canvas.StrokeSize = strokeSize;
            //// The stroke dash pattern value 4,4 specifies the first parameter denotes the size of the line
            //// The second parameter 4 denotes the spaces between the dashes
            canvas.StrokeDashPattern = new float[] { 4, 4 };
            float xPosition = isRTL ? yearViewWidth - yearCellWidth : 0;
            float yPosition = 0;
            for (int i = 0; i < _visibleDates.Count; i++)
            {
                DateTime dateTime = _visibleDates[i];

                // If year arrangement reached to last column then need to start from the next row first column.
                if (i % ColumnCount == 0 && i != 0)
                {
                    xPosition = isRTL ? yearViewWidth - yearCellWidth : 0;
                    yPosition += yearCellHeight;
                }

                // Bool Property to check the Leading date
                bool isLeadingDate = CalendarViewHelper.IsLeadingAndTrailingDate(dateTime, _visibleDates[0], _calendarViewInfo.View, _calendarViewInfo.Identifier);

                // If ShowLeadingAndTrailingDates set to false no need to draw leading and trailing date.
                if (isLeadingDate && !_calendarViewInfo.ShowTrailingAndLeadingDates)
                {
                    xPosition += cellWidthOffset;
                    continue;
                }

                // Boolean to check whether the date is blackout date.
                bool isBlackoutDate = CalendarViewHelper.IsDateInDateCollection(dateTime, _disabledDates);

                // Bool Property to check the disabled date
                bool isDisabledDate = CalendarViewHelper.IsDisabledDate(dateTime, _calendarViewInfo.View, _calendarViewInfo.EnablePastDates, _calendarViewInfo.MinimumDate, _calendarViewInfo.MaximumDate, _calendarViewInfo.SelectionMode, _calendarViewInfo.RangeSelectionDirection, _selectedDateRange, _calendarViewInfo.AllowViewNavigation, _calendarViewInfo.Identifier);
                RectF rect = new RectF(xPosition, yPosition, yearCellWidth, yearCellHeight);
                float highlightXPosition = xPosition + horizontalPadding;
                float highlightYPosition = yPosition + verticalHighlightPadding;
                Rect highlightRect = new Rect(new Point(highlightXPosition, highlightYPosition), highlightSize);
                //// If it is hovered date, need to fill the hovering background color.
                if (!isBlackoutDate && !isDisabledDate)
                {
                    SelectedRangeStatus? selectionStatus = GetRangeHoveringStatus(dateTime);
                    if (selectionStatus != null)
                    {
                        //// To draw the forward, Backward, Both and None range selection Hovering.
                        //// Here, Not using switch case to check the direction. Because except default direction, need to call same method to hover
                        //// And also Default direction is not come within this method
                        DrawRangeSelectionHovering(canvas, selectionStatus, rect, highlightRect, rectCornerRadius, circleCornerRadius, strokeSize);
                    }
                }

                xPosition += cellWidthOffset;
            }
        }

        /// <summary>
        /// Method to draw the hovering for year, decade and Century views
        /// </summary>
        /// <param name="canvas">The draw canvas.</param>
        /// <param name="hoveringRect">The year cell highlight rect values based on its text size.</param>
        /// <param name="rectCornerRadius">The rectangle corner radius</param>
        /// <param name="circleCornerRadius">The circle corner radius</param>
        void DrawSelectionHighlight(ICanvas canvas, RectF hoveringRect, double rectCornerRadius, double circleCornerRadius)
        {
            if (_calendarViewInfo.SelectionShape == CalendarSelectionShape.Circle)
            {
                canvas.FillRoundedRectangle(hoveringRect, circleCornerRadius);
            }
            else
            {
                canvas.FillRoundedRectangle(hoveringRect, rectCornerRadius);
            }
        }

        /// <summary>
        /// Method to draw the range selection Hovering
        /// </summary>
        /// <param name="canvas">The calendar canvas</param>
        /// <param name="selectionStatus">Selection status</param>
        /// <param name="rect">The rect position values</param>
        /// <param name="highlightRect">Position and size for highlight</param>
        /// <param name="rectCornerRadius">The rectangle corner radius</param>
        /// <param name="circleCornerRadius">The circle corner radius</param>
        /// <param name="strokeSize">The stroke size</param>
        void DrawRangeSelectionHovering(ICanvas canvas, SelectedRangeStatus? selectionStatus, RectF rect, RectF highlightRect, double rectCornerRadius, double circleCornerRadius, float strokeSize)
        {
            if (selectionStatus == SelectedRangeStatus.SelectedRange)
            {
                DrawSelectionHighlight(canvas, highlightRect, rectCornerRadius, circleCornerRadius);
            }
            else if (selectionStatus == SelectedRangeStatus.HoverStartDate)
            {
                if (_calendarViewInfo.IsRTLLayout)
                {
                    DrawEndDashLineHovering(canvas, rect, highlightRect, rectCornerRadius, circleCornerRadius, strokeSize);
                }
                else
                {
                    DrawStartDashLineHovering(canvas, rect, highlightRect, rectCornerRadius, circleCornerRadius, strokeSize);
                }
            }
            else if (selectionStatus == SelectedRangeStatus.EndRange)
            {
                if (_calendarViewInfo.IsRTLLayout)
                {
                    StartRangeHovering(canvas, rect, highlightRect, rectCornerRadius, circleCornerRadius, strokeSize);
                }
                else
                {
                    EndRangeHovering(canvas, rect, highlightRect, rectCornerRadius, circleCornerRadius, strokeSize);
                }
            }
            else if (selectionStatus == SelectedRangeStatus.HoverEndDate)
            {
                if (_calendarViewInfo.IsRTLLayout)
                {
                    DrawStartDashLineHovering(canvas, rect, highlightRect, rectCornerRadius, circleCornerRadius, strokeSize);
                }
                else
                {
                    DrawEndDashLineHovering(canvas, rect, highlightRect, rectCornerRadius, circleCornerRadius, strokeSize);
                }
            }
            else if (selectionStatus == SelectedRangeStatus.StartRange)
            {
                if (_calendarViewInfo.IsRTLLayout)
                {
                    EndRangeHovering(canvas, rect, highlightRect, rectCornerRadius, circleCornerRadius, strokeSize);
                }
                else
                {
                    StartRangeHovering(canvas, rect, highlightRect, rectCornerRadius, circleCornerRadius, strokeSize);
                }
            }
            else if (selectionStatus == SelectedRangeStatus.InBetweenRange)
            {
                RectF selectionRectangle = new RectF(rect.Left + (strokeSize / 2), highlightRect.Top + (strokeSize / 2), rect.Width - strokeSize, highlightRect.Height - strokeSize);
                canvas.DrawLine(selectionRectangle.Left, selectionRectangle.Top, selectionRectangle.Right, selectionRectangle.Top);
                canvas.DrawLine(selectionRectangle.Left, selectionRectangle.Top + selectionRectangle.Height, selectionRectangle.Right, selectionRectangle.Bottom);
            }
        }

        /// <summary>
        /// Method to draw the Start range Hovering
        /// </summary>
        /// <param name="canvas">The canvas</param>
        /// <param name="rect">Position to draw</param>
        /// <param name="highlightRect">Position and size for highlight</param>
        /// <param name="rectCornerRadius">Corner radius value</param>
        /// <param name="circleCornerRadius">The circle corner radius</param>
        /// <param name="strokeSize">The stroke size</param>
        void StartRangeHovering(ICanvas canvas, RectF rect, RectF highlightRect, double rectCornerRadius, double circleCornerRadius, float strokeSize)
        {
            DrawStartDashLineHovering(canvas, rect, highlightRect, rectCornerRadius, circleCornerRadius, strokeSize);
            DrawSelectionHighlight(canvas, highlightRect, rectCornerRadius, circleCornerRadius);
        }

        /// <summary>
        /// Method to draw the End range Hovering
        /// </summary>
        /// <param name="canvas">The canvas</param>
        /// <param name="rect">Position to draw</param>
        /// <param name="highlightRect">Position and size for highlight</param>
        /// <param name="rectCornerRadius">Corner radius value</param>
        /// <param name="circleCornerRadius">The circle corner radius</param>
        /// <param name="strokeSize">The stroke size</param>
        void EndRangeHovering(ICanvas canvas, RectF rect, RectF highlightRect, double rectCornerRadius, double circleCornerRadius, float strokeSize)
        {
            DrawEndDashLineHovering(canvas, rect, highlightRect, rectCornerRadius, circleCornerRadius, strokeSize);
            DrawSelectionHighlight(canvas, highlightRect, rectCornerRadius, circleCornerRadius);
        }

        /// <summary>
        /// Method to draw the start dashed line for Range selection
        /// </summary>
        /// <param name="canvas">The canvas</param>
        /// <param name="rectF">Position to draw</param>
        /// <param name="highlightRect">Position and size for highlight</param>
        /// <param name="rectCornerRadius">The corner radius value</param>
        /// <param name="circleCornerRadius">The circle corner radius</param>
        /// <param name="strokeSize">The stroke size</param>
        void DrawStartDashLineHovering(ICanvas canvas, RectF rectF, RectF highlightRect, double rectCornerRadius, double circleCornerRadius, float strokeSize)
        {
            float padding = (rectF.Width - highlightRect.Width) * 0.5f;
            float cornerRadius = (float)(_calendarViewInfo.SelectionShape == CalendarSelectionShape.Circle ? circleCornerRadius : rectCornerRadius);
            highlightRect = new RectF(highlightRect.Left, highlightRect.Top + (strokeSize / 2), highlightRect.Width, highlightRect.Height - strokeSize);
            canvas.DrawLine(rectF.Right - padding - cornerRadius, highlightRect.Top, rectF.Right, highlightRect.Top);
            canvas.DrawLine(rectF.Right - padding - cornerRadius, highlightRect.Bottom, rectF.Right, highlightRect.Bottom);
        }

        /// <summary>
        /// Method to draw the end dashed line for Range selection
        /// </summary>
        /// <param name="canvas">The canvas</param>
        /// <param name="rectF">Position to draw</param>
        /// <param name="highlightRect">Position and size for highlight</param>
        /// <param name="rectCornerRadius">The corner radius value</param>
        /// <param name="circleCornerRadius">The circle corner radius</param>
        /// <param name="strokeSize">The stroke size</param>
        void DrawEndDashLineHovering(ICanvas canvas, RectF rectF, RectF highlightRect, double rectCornerRadius, double circleCornerRadius, float strokeSize)
        {
            float cornerRadius = (float)(_calendarViewInfo.SelectionShape == CalendarSelectionShape.Circle ? circleCornerRadius : rectCornerRadius);
            float padding = (rectF.Width - highlightRect.Width) * 0.5f;
            highlightRect = new RectF(highlightRect.Left, highlightRect.Top + (strokeSize / 2), highlightRect.Width, highlightRect.Height - strokeSize);
            canvas.DrawLine(rectF.Left, highlightRect.Top, rectF.Left + padding + cornerRadius, highlightRect.Top);
            canvas.DrawLine(rectF.Left, highlightRect.Bottom, rectF.Left + padding + cornerRadius, highlightRect.Bottom);
        }

        /// <summary>
        /// Method to get the Range Hovering status
        /// </summary>
        /// <param name="dateTime">The date time value</param>
        /// <returns>Returns string value</returns>
        SelectedRangeStatus? GetRangeHoveringStatus(DateTime dateTime)
        {
            bool isHoveredDateTime = CalendarViewHelper.IsSameDate(_calendarViewInfo.View, _previousHoveredDate, dateTime, _calendarViewInfo.Identifier);

            DateTime? startDate = _selectedDateRange?.StartDate;
            DateTime? endDate = _selectedDateRange?.EndDate == null ? startDate : _selectedDateRange?.EndDate;

            // If the selected range is null or start and end range is also null when the allow view navigation is false
            // Then the hovered date is said to selected range
            if ((_selectedDateRange == null || (startDate == null && endDate == null)) && isHoveredDateTime)
            {
                return SelectedRangeStatus.SelectedRange;
            }

            bool isHoveredStartDate = CalendarViewHelper.IsSameDate(_calendarViewInfo.View, _previousHoveredDate, _selectedDateRange?.StartDate, _calendarViewInfo.Identifier);
            bool isHoveredEndDate = CalendarViewHelper.IsSameDate(_calendarViewInfo.View, _previousHoveredDate, _selectedDateRange?.EndDate, _calendarViewInfo.Identifier);
            bool isGreaterStartDate = startDate.IsGreaterDate(_calendarViewInfo.View, _previousHoveredDate, _calendarViewInfo.Identifier);
            bool isGreaterEndDate = endDate.IsGreaterDate(_calendarViewInfo.View, _previousHoveredDate, _calendarViewInfo.Identifier);

            //// If the date is in between the selected range, need to hover based on the selection shape
            //// Example : Selected range(2022/1/1 to 2025/1/1).. The in-between dates are said to selected range
            if (isHoveredDateTime && (isHoveredStartDate || isHoveredEndDate || (!isGreaterStartDate && isGreaterEndDate)))
            {
                return SelectedRangeStatus.SelectedRange;
            }
            //// End range and dateTime is 2025, hovering date is 2026 or above date, then the 2025 is said to hovering start date
            else if (CalendarViewHelper.IsSameDate(_calendarViewInfo.View, endDate, dateTime, _calendarViewInfo.Identifier) && _previousHoveredDate.IsGreaterDate(_calendarViewInfo.View, endDate, _calendarViewInfo.Identifier))
            {
                return SelectedRangeStatus.HoverStartDate;
            }
            //// Hovering date and dateTime is 2027, end range is 2025.. Hovering date is greater than end range then the 2027 is said to End range
            //// Here, we don't used the isGreaterEndDate variable. we need to check the previousHoveredDate is greater than the enddate.
            //// By using inverse for isGreaterEndDate, when the end date or previousHoveredDate is null it returns true. So, can't used isGreaterEndDate variableS
            else if (isHoveredDateTime && _previousHoveredDate.IsGreaterDate(_calendarViewInfo.View, endDate, _calendarViewInfo.Identifier))
            {
                return SelectedRangeStatus.EndRange;
            }
            //// Start range and dateTime is 2022, hovering date is 2021 or below, then the 2022 is said to hovering end date
            else if (CalendarViewHelper.IsSameDate(_calendarViewInfo.View, startDate, dateTime, _calendarViewInfo.Identifier) && isGreaterStartDate)
            {
                return SelectedRangeStatus.HoverEndDate;
            }
            //// Hovering date and dateTime is 2020, start range is 2022.. start range is greater than hovering date then the 2020 is said to start range
            else if (isHoveredDateTime && isGreaterStartDate)
            {
                return SelectedRangeStatus.StartRange;
            }
            //// Example 1 - HoveringStartDate(2025), EndRange(2028). Then 2026, 2027 are said to InBetweenRange
            else if ((endDate != null && CalendarViewHelper.IsGreaterDate(dateTime, _calendarViewInfo.View, endDate, _calendarViewInfo.Identifier) && _previousHoveredDate.IsGreaterDate(_calendarViewInfo.View, dateTime, _calendarViewInfo.Identifier)) || (startDate != null && CalendarViewHelper.IsGreaterDate(dateTime, _calendarViewInfo.View, _previousHoveredDate, _calendarViewInfo.Identifier) && startDate.IsGreaterDate(_calendarViewInfo.View, dateTime, _calendarViewInfo.Identifier)))
            {
                return SelectedRangeStatus.InBetweenRange;
            }

            return null;
        }

        #endregion

        #region Override Methods

        /// <summary>
        /// Method to draw the Year view hovering panel.
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

            _previousHoveredDate = CalendarViewHelper.GetYearDateFromPosition(_hoverPosition.Value, dirtyRect.Width, dirtyRect.Height, _calendarViewInfo, _visibleDates);
            //// Condition to check whether the interaction is enabled for hovered date.
            if (_previousHoveredDate == null || !CalendarViewHelper.IsInteractableDate(_previousHoveredDate.Value, _disabledDates, _visibleDates, _calendarViewInfo, 0))
            {
                return;
            }

            bool isRTL = _calendarViewInfo.IsRTLLayout;
            float yearViewWidth = dirtyRect.Width;
            float yearViewHeight = dirtyRect.Height;
            //// Number of column count in year, decade and Century view is 3
            float yearCellWidth = yearViewWidth / ColumnCount;
            //// Number of row count in year, decade and Century view is 4
            float yearCellHeight = yearViewHeight / RowCount;
            float xPosition = 0;
            float yPosition = 0;
            //// When the mode is range selection and not default direction, need to draw the dashed line hovering
            if (_calendarViewInfo.SelectionMode != CalendarSelectionMode.Range || _calendarViewInfo.RangeSelectionDirection == CalendarRangeSelectionDirection.Default || _calendarViewInfo.AllowViewNavigation || _calendarViewInfo.YearView.CellTemplate != null)
            {
                xPosition = (float)(isRTL ? yearViewWidth - _hoverPosition.Value.X : _hoverPosition.Value.X);
                yPosition = (float)_hoverPosition.Value.Y;
                int row = (int)(yPosition / (yearViewHeight / RowCount));
                int column = (int)(xPosition / (yearViewWidth / ColumnCount));
                if (row >= RowCount || column >= ColumnCount)
                {
                    return;
                }
                //// In RTL flow direction, day position will be calculated from reverse hence (ColumnCount - 1 = 2) is used to find the cell position in reverse.
                xPosition = (isRTL ? ColumnCount - 1 - column : column) * yearCellWidth;
                yPosition = row * yearCellHeight;
            }

            //// Calculate the initial year cell text from its visible dates for calculate the year cell highlight height.
            string yearCellText = CalendarViewHelper.GetYearCellText(_visibleDates[0], _calendarViewInfo);
            //// Holds the year cell text height based on its text style.
            float textHeight = (float)yearCellText.Measure(_calendarViewInfo.YearView.TextStyle).Height;
            //// Space between the text and top or bottom of the cell.
            float textTopPosition = (yearCellHeight - textHeight) * 0.5f;
            //// Check the year cell does not have vertical space.
            textTopPosition = textTopPosition < 0 ? 0 : textTopPosition;
            //// Padding value between the text and top or bottom of the highlight.
            //// Example: Consider yearCellHeight = 120 and textHeight = 30,
            //// textTopPosition = (120 - 30) / 2 = 45
            //// verticalTextPadding = 45 * 0.3 = 15
            float verticalTextPadding = textTopPosition * 0.3f;
            //// Padding value from the left and right side of the cell for hovering.
            float horizontalPadding = yearCellWidth * 0.1f;
            float verticalHighlightPadding = textTopPosition - verticalTextPadding;
            //// Height value for hovering highlight.
            float highlightHeight = textHeight + (2 * verticalTextPadding);
            //// Width of the hovering highlight.
            float highlightWidth = yearCellWidth - (2 * horizontalPadding);
            //// Corner radius for the hovering when the selection shape is rectangle.
            double rectCornerRadius = (float)((highlightHeight * 0.1) > 2 ? 2 : highlightHeight * 0.1);
            //// Corner radius for the hovering when the selection shape is circle.
            double circleCornerRadius = highlightHeight / 2;
            canvas.SaveState();
            canvas.Antialias = true;
            Color hoverBackground = CalendarViewHelper.GetMouseHoverColor(_calendarViewInfo.SelectionBackground, _calendarViewInfo.YearView.CellTemplate);
            if (_calendarViewInfo.SelectionBackground != null && _calendarViewInfo.HoverColor != null)
            {
                hoverBackground = CalendarViewHelper.GetMouseHoverColor(_calendarViewInfo.HoverColor, _calendarViewInfo.YearView.CellTemplate);
            }

            canvas.FillColor = hoverBackground;
            switch (_calendarViewInfo.SelectionMode)
            {
                case CalendarSelectionMode.Single:
                case CalendarSelectionMode.Multiple:
                case CalendarSelectionMode.MultiRange:
                    {
                        float highlightXPosition = xPosition + horizontalPadding;
                        float highlightYPosition = yPosition + verticalHighlightPadding;
                        RectF hoveringRect = new RectF(highlightXPosition, highlightYPosition, highlightWidth, highlightHeight);
                        DrawSelectionHighlight(canvas, hoveringRect, rectCornerRadius, circleCornerRadius);
                        break;
                    }

                case CalendarSelectionMode.Range:
                    if (_calendarViewInfo.RangeSelectionDirection == CalendarRangeSelectionDirection.Default || _calendarViewInfo.AllowViewNavigation || _calendarViewInfo.YearView.CellTemplate != null)
                    {
                        float highlightXPosition = xPosition + horizontalPadding;
                        float highlightYPosition = yPosition + verticalHighlightPadding;
                        RectF hoveringRect = new RectF(highlightXPosition, highlightYPosition, highlightWidth, highlightHeight);
                        DrawSelectionHighlight(canvas, hoveringRect, rectCornerRadius, circleCornerRadius);
                    }
                    else
                    {
                        SizeF yearCellSize = new SizeF(yearCellWidth, yearCellHeight);
                        SizeF highlightPadding = new SizeF(horizontalPadding, verticalHighlightPadding);
                        SizeF highlightSize = new SizeF(highlightWidth, highlightHeight);
                        DrawRangeHighlight(canvas, isRTL, yearCellSize, yearViewWidth, highlightSize, highlightPadding, rectCornerRadius, circleCornerRadius);
                    }

                    break;
            }

            canvas.RestoreState();
        }

        #endregion
    }
}
#endif