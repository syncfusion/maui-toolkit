using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Syncfusion.Maui.Toolkit.Internals;
using Syncfusion.Maui.Toolkit.Localization;
using Syncfusion.Maui.Toolkit.Graphics.Internals;
using PointerEventArgs = Syncfusion.Maui.Toolkit.Internals.PointerEventArgs;
using Globalization = System.Globalization;

namespace Syncfusion.Maui.Toolkit.Calendar
{
    /// <summary>
    /// Represents Year view
    /// </summary>
    internal class YearView : SfView, ITapGestureListener, IDoubleTapGestureListener, ILongPressGestureListener, ITouchListener, ICalendarView, IKeyboardListener
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
        readonly ICalendar _calendarViewInfo;

        /// <summary>
        /// The selected date of the calendar
        /// </summary>
        DateTime? _selectedDate;

#if __MACCATALYST__ || (!__ANDROID__ && !__IOS__)
        /// <summary>
        /// The year hover view.
        /// </summary>
        readonly YearHoverView _hoverView;
#endif

        /// <summary>
        /// The selected dates.
        /// </summary>
        ObservableCollection<DateTime> _selectedDates;

        /// <summary>
        /// The selected date range of the calendar
        /// </summary>
        CalendarDateRange? _selectedDateRange;

        /// <summary>
        /// The selected date ranges.
        /// </summary>
        ObservableCollection<CalendarDateRange>? _selectedDateRanges;

        /// <summary>
        /// The pan gesture started then the initialStartRangeDate will update. The pan date is greater than initialStartRangeDate then range selection end date will update.
        /// The pan date is lesser than initialStartRangeDate then start date will update.
        /// The pan gesture status was completed or canceled the initialStartDateRange value updated as null.
        /// </summary>
        DateTime? _initialRangeStartDate;

        /// <summary>
        /// The disabled dates.
        /// </summary>
        List<DateTime> _disabledDates;

        /// <summary>
        /// Gets or sets the year cell views created from data template. Applicable only on Year view cell template provided.
        /// </summary>
        List<View>? _yearCells;

        /// <summary>
        /// Gets or sets the virtual year cell semantic nodes.
        /// </summary>
        List<SemanticsNode>? _semanticsNodes;

        /// <summary>
        /// Gets or sets the size of the semantic.
        /// </summary>
        Size _semanticsSize = Size.Zero;

        /// <summary>
        /// It holds whether the view is current view or not.
        /// </summary>
        bool _isCurrentView;

        /// <summary>
        /// To store the previous selected date time for multiple selection.
        /// </summary>
        DateTime? _lastSelectedDate;

        /// <summary>
        /// To store the previous selected date range for range selection.
        /// </summary>
        DateTime? _previousSelectedDateRange;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="YearView"/> class.
        /// </summary>
        /// <param name="calendarViewInfo">The Calendar Year view info.</param>
        /// <param name="visibleDates">The visible dates collection.</param>
        /// <param name="disabledDates">The disabled dates collection.</param>
        /// <param name="isCurrentView">Defines whether the view is current view or not.</param>
        internal YearView(ICalendar calendarViewInfo, List<DateTime> visibleDates, List<DateTime> disabledDates, bool isCurrentView)
        {
            _calendarViewInfo = calendarViewInfo;
            _visibleDates = visibleDates;
            _selectedDate = _calendarViewInfo.SelectedDate;
            _selectedDates = new ObservableCollection<DateTime>(_calendarViewInfo.SelectedDates);
            _selectedDateRange = GetValidSelectedRange(_calendarViewInfo.SelectedDateRange);
            UpdateSelectedDateRangesValue(_calendarViewInfo.SelectedDateRanges);
            _disabledDates = disabledDates;
            if (_calendarViewInfo.YearView.CellTemplate != null)
            {
                DrawingOrder = DrawingOrder.AboveContent;
                _yearCells = new List<View>();
                GenerateYearCells(isCurrentView);
            }
            else
            {
                DrawingOrder = DrawingOrder.BelowContent;
            }

#if __MACCATALYST__ || (!__ANDROID__ && !__IOS__)
            _hoverView = new YearHoverView(_calendarViewInfo, visibleDates, disabledDates, _selectedDateRange);
            Add(_hoverView);
            this.AddTouchListener(this);
#endif

            this.AddKeyboardListener(this);
            this.AddGestureListener(this);
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Method to update the semantics node.
        /// </summary>
        /// <param name="isCurrentView">The view is current view or not.</param>
        internal void InvalidateSemanticsNode(bool isCurrentView)
        {
            _semanticsSize = Size.Zero;
            _semanticsNodes?.Clear();
            _semanticsNodes = null;
            _isCurrentView = isCurrentView;
            InvalidateSemantics();
        }

        #endregion

        #region Private Methods

#if __IOS__ || __MACCATALYST__

        /// <summary>
        /// Method to set the focus for layout changed to other view. Focus removes while new view generation or inner view gets focus.
        /// Hence used delay to set focus.
        /// </summary>
        /// <param name="delay">The focus delay in milliseconds.</param>
        async void SetFocus(int delay)
        {
            await Task.Delay(delay);
            Focus();
        }

#endif

        /// <summary>
        /// Method to draw the start range selection highlight
        /// </summary>
        /// <param name="canvas">The canvas to draw</param>
        /// <param name="rect">Position to draw</param>
        /// <param name="highlightRect">Position and size for highlight</param>
        /// <param name="circleCornerRadius">The circle corner radius</param>
        /// <param name="rectCornerRadius">The rectangle corner radius</param>
        /// <param name="selectionBackground">The end range background to apply</param>
        void DrawStartRangeSelectionHighlight(ICanvas canvas, RectF rect, RectF highlightRect, float circleCornerRadius, float rectCornerRadius, Color selectionBackground)
        {
            canvas.FillColor = selectionBackground;
            float leftPadding = (rect.Width - highlightRect.Width) * 0.5f;
            RectF selectionRectangle = new RectF(rect.X + leftPadding, highlightRect.Y, rect.Width - leftPadding, highlightRect.Height);
            //// If the corner radius is 2, topLeftCornerRadius is 2, topRightCornerRadius as 0, bottomLeftCornerRadius as 2, bottomRightCornerRadius as 0
            //// To draw the start range, left corner only has cornerRadius
            if (_calendarViewInfo.SelectionShape == CalendarSelectionShape.Circle)
            {
                canvas.FillRoundedRectangle(selectionRectangle, circleCornerRadius, 0, circleCornerRadius, 0);
            }
            else
            {
                canvas.FillRoundedRectangle(selectionRectangle, rectCornerRadius, 0, rectCornerRadius, 0);
            }
        }

        /// <summary>
        /// Method to draw the end range selection highlight
        /// </summary>
        /// <param name="canvas">The canvas to draw</param>
        /// <param name="rect">Position to draw</param>
        /// <param name="highlightRect">Position and size for highlight</param>
        /// <param name="circleCornerRadius">The circle corner radius</param>
        /// <param name="rectCornerRadius">The rectangle corner radius</param>
        /// <param name="selectionBackground">The end range background to apply</param>
        void DrawEndRangeSelectionHighlight(ICanvas canvas, RectF rect, RectF highlightRect, float circleCornerRadius, float rectCornerRadius, Color selectionBackground)
        {
            canvas.FillColor = selectionBackground;
            float rightPadding = (rect.Width - highlightRect.Width) * 0.5f;
            RectF selectionRectangle = new RectF(rect.X, highlightRect.Y, rect.Width - rightPadding, highlightRect.Height);
            //// If the corner radius is 2, topLeftCornerRadius is 0, topRightCornerRadius as 2, bottomLeftCornerRadius as 0, bottomRightCornerRadius as 2
            //// To draw the end range, right corner only has cornerRadius
            if (_calendarViewInfo.SelectionShape == CalendarSelectionShape.Circle)
            {
                canvas.FillRoundedRectangle(selectionRectangle, 0, circleCornerRadius, 0, circleCornerRadius);
            }
            else
            {
                canvas.FillRoundedRectangle(selectionRectangle, 0, rectCornerRadius, 0, rectCornerRadius);
            }
        }

        /// <summary>
        /// Method to draw the year view Panel(Year, Decade and Century)
        /// </summary>
        /// <param name="canvas">The draw canvas.</param>
        /// <param name="xPosition">Value of the xPosition</param>
        /// <param name="yPosition">Value of the yPosition</param>
        /// <param name="isRTL">Is RTL flow direction.</param>
        /// <param name="yearViewWidth">The year view width</param>
        /// <param name="yearCellWidth">The year cell width</param>
        /// <param name="yearCellHeight">The year cell height</param>
        /// <param name="cellWidthOffset">The cell width offset value</param>
        void DrawYearViewPanelCells(ICanvas canvas, float xPosition, float yPosition, bool isRTL, float yearViewWidth, float yearCellWidth, float yearCellHeight, float cellWidthOffset)
        {
            // To fill the Background Colors on the basis of Conditions for the year, decade and Century
            Color fillColor = Colors.Transparent;
            Color selectionBackground = CalendarViewHelper.GetSelectionBackground(_calendarViewInfo.SelectionBackground, _calendarViewInfo.SelectionMode).ToColor();
            if (_calendarViewInfo.SelectionMode == CalendarSelectionMode.Range || _calendarViewInfo.SelectionMode == CalendarSelectionMode.MultiRange)
            {
                if (_calendarViewInfo.SelectionBackground != null && _calendarViewInfo.SelectedRangeColor != null)
                {
                    selectionBackground = CalendarViewHelper.GetSelectionBackground(_calendarViewInfo.SelectedRangeColor, _calendarViewInfo.SelectionMode).ToColor();
                }
            }

            Color startRangeSelectionBackground = _calendarViewInfo.StartRangeSelectionBackground.ToColor();
            Color endRangeSelectionBackground = _calendarViewInfo.EndRangeSelectionBackground.ToColor();
            Color inBetweenRangeBackground = selectionBackground;
            //// Calculate the horizontal padding between the year cell and its highlight from cell width value.
            //// Step1: Assign the year cell width and height as 100, 100 and horizontal padding as left 10 and right 10.
            float horizontalPadding = yearCellWidth * 0.1f;
            //// Calculate the highlight width based on the horizontal padding(left and right).
            //// Step2: highlight width value as 80.
            float highlightWidth = yearCellWidth - (2 * horizontalPadding);
            //// Holds the padding between the highlight and the year cell text.
            float textPadding = 2;
            //// Holds the max text width based on its text padding value.
            //// Step3: total text width as 80 - 4 = 76.
            float totalTextWidth = highlightWidth - (2 * textPadding);
            //// Calculate the initial year cell text from its visible dates for calculate the year cell highlight height.
            string yearCellText = CalendarViewHelper.GetYearCellText(_visibleDates[0], _calendarViewInfo);
            //// Holds the year cell text height based on its text style.
            //// Step4: Assume the text height as 40.
            float textHeight = (float)yearCellText.Measure(_calendarViewInfo.YearView.TextStyle).Height;
            //// Calculate the vertical space between year cell top position and year cell text.
            //// Step5: text top position as (100 - 40) * 0.5 = 30.
            float textTopPosition = (yearCellHeight - textHeight) * 0.5f;
            //// Check the year cell does not have vertical space.
            textTopPosition = textTopPosition < 0 ? 0 : textTopPosition;
            //// Assign the vertical highlight size as 30% of vertical space.
            //// Step6: vertical text padding value is 30 * 0.3 = 9.
            float verticalTextPadding = textTopPosition * 0.3f;
            //// Calculate the highlight padding from its top position by vertical space and vertical text padding.
            //// Step7: Padding above the highlight is 30 - 9 = 21.
            float verticalHighlightPadding = textTopPosition - verticalTextPadding;
            //// Calculate the highlight height from text height and vertical text padding.
            //// Step8: highlight height is 40 + (2* 9) = 58.
            float highlightHeight = textHeight + (2 * verticalTextPadding);
            //// Corner radius for the hovering when the selection shape is rectangle.
            float rectCornerRadius = (float)((highlightHeight * 0.1) > 2 ? 2 : highlightHeight * 0.1);
            //// Corner radius for the hovering when the selection shape is circle.
            float circleCornerRadius = highlightHeight / 2;

            ObservableCollection<CalendarDateRange>? visibleSelectedDateRanges = null;
            //// Get the current view selected date ranges.
            if (_calendarViewInfo.SelectionMode == CalendarSelectionMode.MultiRange && _selectedDateRanges != null)
            {
                visibleSelectedDateRanges = GetVisibleSelectedDateRanges(_selectedDateRanges);
            }

            for (int i = 0; i < _visibleDates.Count; i++)
            {
                DateTime dateTime = _visibleDates[i];
                int column = i % ColumnCount;

                // If year arrangement reached to last column then need to start from the next row first column.
                if (column == 0 && i != 0)
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

                // Bool Property to check the today date
                bool isTodayDate = CalendarViewHelper.IsSameDate(_calendarViewInfo.View, DateTime.Now, dateTime, _calendarViewInfo.Identifier);

                // Boolean to check whether the date is blackout date.
                bool isBlackoutDate = CalendarViewHelper.IsDateInDateCollection(dateTime, _disabledDates);

                // Bool Property to check the disabled date
                bool isDisabledDate = CalendarViewHelper.IsDisabledDate(dateTime, _calendarViewInfo.View, _calendarViewInfo.EnablePastDates, _calendarViewInfo.MinimumDate, _calendarViewInfo.MaximumDate, _calendarViewInfo.SelectionMode, _calendarViewInfo.RangeSelectionDirection, _selectedDateRange, _calendarViewInfo.AllowViewNavigation, _calendarViewInfo.Identifier);

                // To get the textStyle for the year view panel cells
                CalendarTextStyle textStyle = GetYearCellStyles(dateTime, _calendarViewInfo.View, isBlackoutDate, isDisabledDate, _calendarViewInfo.ShowOutOfRangeDates, isTodayDate, isLeadingDate, _calendarViewInfo.YearView.Background.ToColor(), ref fillColor, _calendarViewInfo.YearView.DisabledDatesBackground.ToColor(), _calendarViewInfo.YearView.LeadingDatesBackground.ToColor());

                // Method to get the year cell date text
                string dateText = CalendarViewHelper.GetYearCellText(dateTime, _calendarViewInfo);
                dateText = CalendarViewHelper.TrimText(dateText, totalTextWidth, textStyle);

                RectF rect = new RectF(xPosition, yPosition, yearCellWidth, yearCellHeight);

                // If background color is provided in year cell style then need to fill color based on leading and current year cell.
                if (fillColor != Colors.Transparent)
                {
                    canvas.FillColor = fillColor;
                    canvas.FillRectangle(rect);
                }

                float highlightXPosition = xPosition + horizontalPadding;
                float highlightYPosition = yPosition + verticalHighlightPadding;
                Rect highlightRect = new Rect(highlightXPosition, highlightYPosition, highlightWidth, highlightHeight);
                bool isSelectedDate = false;
                switch (_calendarViewInfo.SelectionMode)
                {
                    case CalendarSelectionMode.Single:
                    case CalendarSelectionMode.Multiple:
                        isSelectedDate = IsSelectedDate(isBlackoutDate, isDisabledDate, dateTime);
                        //// If it is selected date then the selection is drawn based on the selection shape.
                        if (isSelectedDate)
                        {
                            textStyle = _calendarViewInfo.YearView.SelectionTextStyle;
                            DrawSelectionHighlight(canvas, highlightRect, rectCornerRadius, circleCornerRadius, selectionBackground);
                        }

                        break;

                    case CalendarSelectionMode.Range:
                        {
                            if (!_calendarViewInfo.AllowViewNavigation && !isDisabledDate && !isBlackoutDate)
                            {
                                SelectedRangeStatus? selectionStatus = GetRangeSelectionStatus(dateTime);
                                isSelectedDate = selectionStatus != null;
                                if (isSelectedDate)
                                {
                                    // If the selected date is range need to apply the range text style, otherwise need to apply selection text style
                                    textStyle = DrawRangeSelectionHighlight(canvas, rect, highlightRect, rectCornerRadius, circleCornerRadius, selectionStatus, textStyle, startRangeSelectionBackground, endRangeSelectionBackground, inBetweenRangeBackground, column);
                                }
                            }

                            break;
                        }

                    case CalendarSelectionMode.MultiRange:
                        {
                            if (!_calendarViewInfo.AllowViewNavigation && !isDisabledDate && !isBlackoutDate && visibleSelectedDateRanges != null)
                            {
                                SelectedRangeStatus? selectionStatus = GetMultiRangeSelectionStatus(dateTime, visibleSelectedDateRanges);
                                isSelectedDate = selectionStatus != null;
                                if (isSelectedDate)
                                {
                                    // If the selected date is range need to apply the range text style, otherwise need to apply selection text style
                                    textStyle = DrawRangeSelectionHighlight(canvas, rect, highlightRect, rectCornerRadius, circleCornerRadius, selectionStatus, textStyle, startRangeSelectionBackground, endRangeSelectionBackground, inBetweenRangeBackground, column);
                                }
                            }

                            break;
                        }
                }

                //// Condition to draw horizontal line at the center of the blackout date based on text width.
                if (isBlackoutDate)
                {
                    float textWidth = (float)dateText.Measure(textStyle).Width;
                    float textWidthCenter = textWidth / 2;
                    float widthCenter = yearCellWidth / 2;
                    float heightCenter = yearCellHeight / 2;
                    canvas.StrokeColor = textStyle.TextColor;
                    canvas.DrawLine(xPosition + (widthCenter - textWidthCenter), yPosition + heightCenter, xPosition + (widthCenter + textWidthCenter), yPosition + heightCenter);
                }

                // Condition to Draw the Rectangle or Circle based on the Selection shape
                if (isTodayDate && !isSelectedDate)
                {
                    // To draw the today highlight shape
                    DrawTodayHighlight(canvas, highlightRect, rectCornerRadius, circleCornerRadius);
                }

                CalendarViewHelper.DrawText(canvas, dateText, textStyle, highlightRect, HorizontalAlignment.Center, VerticalAlignment.Center);
                xPosition += cellWidthOffset;
            }
        }

        /// <summary>
        /// Method to draw the selection for custom views.
        /// </summary>
        /// <param name="canvas">The draw canvas.</param>
        /// <param name="xPosition">Value of the xPosition</param>
        /// <param name="yPosition">Value of the yPosition</param>
        /// <param name="isRTL">Is RTL flow direction.</param>
        /// <param name="yearViewWidth">The year view width</param>
        /// <param name="yearCellWidth">The year cell width</param>
        /// <param name="yearCellHeight">The year cell height</param>
        /// <param name="cellWidthOffset">The cell width offset value</param>
        void DrawTemplateSelection(ICanvas canvas, float xPosition, float yPosition, bool isRTL, float yearViewWidth, float yearCellWidth, float yearCellHeight, float cellWidthOffset)
        {
            if (_calendarViewInfo.AllowViewNavigation)
            {
                return;
            }

            Color selectionBackground = CalendarViewHelper.GetSelectionBackground(_calendarViewInfo.SelectionBackground, _calendarViewInfo.SelectionMode).ToColor();
            if (_calendarViewInfo.SelectionMode == CalendarSelectionMode.Range || _calendarViewInfo.SelectionMode == CalendarSelectionMode.MultiRange)
            {
                if (_calendarViewInfo.SelectionBackground != null && _calendarViewInfo.SelectedRangeColor != null)
                {
                    selectionBackground = CalendarViewHelper.GetSelectionBackground(_calendarViewInfo.SelectedRangeColor, _calendarViewInfo.SelectionMode).ToColor();
                }
            }

            Color startRangeSelectionBackground = _calendarViewInfo.StartRangeSelectionBackground.ToColor();
            Color endRangeSelectionBackground = _calendarViewInfo.EndRangeSelectionBackground.ToColor();
            Color inBetweenRangeBackground = selectionBackground;

            //// If the selection and range selection backgrounds are set to transparent then no need to draw the selection on the cell template.
            if (_calendarViewInfo.SelectionMode != CalendarSelectionMode.Range && _calendarViewInfo.SelectionMode != CalendarSelectionMode.MultiRange)
            {
                if (selectionBackground == Colors.Transparent)
                {
                    return;
                }
            }
            else
            {
                if (startRangeSelectionBackground == Colors.Transparent && endRangeSelectionBackground == Colors.Transparent && inBetweenRangeBackground == Colors.Transparent)
                {
                    return;
                }
            }

            //// Calculate the horizontal padding between the year cell and its highlight from cell width value.
            //// Step1: Assign the year cell width and height as 100, 100 and horizontal padding as left 10 and right 10.
            float horizontalPadding = yearCellWidth * 0.1f;
            //// Calculate the highlight width based on the horizontal padding(left and right).
            //// Step2: highlight width value as 80.
            float highlightWidth = yearCellWidth - (2 * horizontalPadding);
            //// Calculate the initial year cell text from its visible dates for calculate the year cell highlight height.
            string yearCellText = CalendarViewHelper.GetYearCellText(_visibleDates[0], _calendarViewInfo);
            //// Holds the year cell text height based on its text style.
            //// Step4: Assume the text height as 40.
            float textHeight = (float)yearCellText.Measure(_calendarViewInfo.YearView.TextStyle).Height;
            //// Calculate the vertical space between year cell top position and year cell text.
            //// Step5: text top position as (100 - 40) * 0.5 = 30.
            float textTopPosition = (yearCellHeight - textHeight) * 0.5f;
            //// Check the year cell does not have vertical space.
            textTopPosition = textTopPosition < 0 ? 0 : textTopPosition;
            //// Assign the vertical highlight size as 30% of vertical space.
            //// Step6: vertical text padding value is 30 * 0.3 = 9.
            float verticalTextPadding = textTopPosition * 0.3f;
            //// Calculate the highlight padding from its top position by vertical space and vertical text padding.
            //// Step7: Padding above the highlight is 30 - 9 = 21.
            float verticalHighlightPadding = textTopPosition - verticalTextPadding;
            //// Calculate the highlight height from text height and vertical text padding.
            //// Step8: highlight height is 40 + (2* 9) = 58.
            float highlightHeight = textHeight + (2 * verticalTextPadding);
            //// Corner radius for the hovering when the selection shape is rectangle.
            float rectCornerRadius = (float)((highlightHeight * 0.1) > 2 ? 2 : highlightHeight * 0.1);
            //// Corner radius for the hovering when the selection shape is circle.
            float circleCornerRadius = highlightHeight / 2;

            ObservableCollection<CalendarDateRange>? visibleSelectedDateRanges = null;
            //// Get the current view selected date ranges.
            if (_calendarViewInfo.SelectionMode == CalendarSelectionMode.MultiRange && _selectedDateRanges != null)
            {
                visibleSelectedDateRanges = GetVisibleSelectedDateRanges(_selectedDateRanges);
            }

            for (int i = 0; i < _visibleDates.Count; i++)
            {
                DateTime dateTime = _visibleDates[i];
                int column = i % ColumnCount;

                // If year arrangement reached to last column then need to start from the next row first column.
                if (column == 0 && i != 0)
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

                RectF rect = new RectF(xPosition, yPosition, yearCellWidth, yearCellHeight);

                float highlightXPosition = xPosition + horizontalPadding;
                float highlightYPosition = yPosition + verticalHighlightPadding;
                Rect highlightRect = new Rect(highlightXPosition, highlightYPosition, highlightWidth, highlightHeight);
                switch (_calendarViewInfo.SelectionMode)
                {
                    case CalendarSelectionMode.Single:
                    case CalendarSelectionMode.Multiple:
                        {
                            bool isSelectedDate = IsSelectedDate(false, false, dateTime);
                            //// If it is selected date then the selection is drawn based on the selection shape.
                            if (isSelectedDate)
                            {
                                // Boolean to check whether the date is blackout date.
                                bool isBlackoutDate = CalendarViewHelper.IsDateInDateCollection(dateTime, _disabledDates);

                                // Bool Property to check the disabled date
                                bool isDisabledDate = CalendarViewHelper.IsDisabledDate(dateTime, _calendarViewInfo.View, _calendarViewInfo.EnablePastDates, _calendarViewInfo.MinimumDate, _calendarViewInfo.MaximumDate, _calendarViewInfo.SelectionMode, _calendarViewInfo.RangeSelectionDirection, _selectedDateRange, _calendarViewInfo.AllowViewNavigation, _calendarViewInfo.Identifier);
                                if (!isBlackoutDate && !isDisabledDate)
                                {
                                    DrawSelectionHighlight(canvas, highlightRect, rectCornerRadius, circleCornerRadius, selectionBackground);
                                }
                            }

                            break;
                        }

                    case CalendarSelectionMode.Range:
                        {
                            SelectedRangeStatus? selectionStatus = GetRangeSelectionStatus(dateTime);
                            bool isSelectedDate = selectionStatus != null;
                            if (isSelectedDate)
                            {
                                // Boolean to check whether the date is blackout date.
                                bool isBlackoutDate = CalendarViewHelper.IsDateInDateCollection(dateTime, _disabledDates);

                                // Bool Property to check the disabled date
                                bool isDisabledDate = CalendarViewHelper.IsDisabledDate(dateTime, _calendarViewInfo.View, _calendarViewInfo.EnablePastDates, _calendarViewInfo.MinimumDate, _calendarViewInfo.MaximumDate, _calendarViewInfo.SelectionMode, _calendarViewInfo.RangeSelectionDirection, _selectedDateRange, _calendarViewInfo.AllowViewNavigation, _calendarViewInfo.Identifier);
                                if (!isBlackoutDate && !isDisabledDate)
                                {
                                    // If the selected date is range need to apply the range text style, otherwise need to apply selection text style
                                    DrawRangeSelectionHighlight(canvas, rect, highlightRect, rectCornerRadius, circleCornerRadius, selectionStatus, _calendarViewInfo.YearView.TextStyle, startRangeSelectionBackground, endRangeSelectionBackground, inBetweenRangeBackground, column);
                                }
                            }

                            break;
                        }

                    case CalendarSelectionMode.MultiRange:
                        {
                            SelectedRangeStatus? selectionStatus = GetMultiRangeSelectionStatus(dateTime, visibleSelectedDateRanges);
                            bool isSelectedDate = selectionStatus != null;
                            if (isSelectedDate)
                            {
                                // Boolean to check whether the date is blackout date.
                                bool isBlackoutDate = CalendarViewHelper.IsDateInDateCollection(dateTime, _disabledDates);

                                // Bool Property to check the disabled date
                                bool isDisabledDate = CalendarViewHelper.IsDisabledDate(dateTime, _calendarViewInfo.View, _calendarViewInfo.EnablePastDates, _calendarViewInfo.MinimumDate, _calendarViewInfo.MaximumDate, _calendarViewInfo.SelectionMode, _calendarViewInfo.RangeSelectionDirection, _selectedDateRange, _calendarViewInfo.AllowViewNavigation, _calendarViewInfo.Identifier);
                                if (!isBlackoutDate && !isDisabledDate)
                                {
                                    // If the selected date is range need to apply the range text style, otherwise need to apply selection text style
                                    DrawRangeSelectionHighlight(canvas, rect, highlightRect, rectCornerRadius, circleCornerRadius, selectionStatus, _calendarViewInfo.YearView.TextStyle, startRangeSelectionBackground, endRangeSelectionBackground, inBetweenRangeBackground, column);
                                }
                            }

                            break;
                        }
                }

                xPosition += cellWidthOffset;
            }
        }

        /// <summary>
        ///  Method to get cell styles to canvas based on disabled date, today date and leading date in the year view panel.
        /// </summary>
        /// <param name="dateTime">The current visible dates.</param>
        /// <param name="view">The calendar view.</param>
        /// <param name="isBlackoutDate">Defines whether the date is blackout date or not.</param>
        /// <param name="isDisabledDate">The date is disabled date</param>
        /// <param name="isShowOutOfRangeDates">The date is out of range dates.</param>
        /// <param name="isTodayDate">The date is today date</param>
        /// <param name="isLeadingDate">Leading and Trailing date</param>
        /// <param name="backgroundColor">The background color to fill normal cell background</param>
        /// <param name="fillColor">The background color to fill year cell background.</param>
        /// <param name="disabledBackground">The background color to fill disabled cell Background.</param>
        /// <param name="leadingBackground">The background color to fill leading cell background.</param>
        /// <returns>Returns the text style</returns>
        CalendarTextStyle GetYearCellStyles(DateTime dateTime, CalendarView view, bool isBlackoutDate, bool isDisabledDate, bool isShowOutOfRangeDates, bool isTodayDate, bool isLeadingDate, Color backgroundColor, ref Color fillColor, Color disabledBackground, Color leadingBackground)
        {
            CalendarYearView yearCellStyle = _calendarViewInfo.YearView;
            CalendarTextStyle textStyle = yearCellStyle.TextStyle;
            fillColor = backgroundColor;

            // Blackout Dates text style and background
            if (isBlackoutDate)
            {
                fillColor = disabledBackground;
                textStyle = yearCellStyle.DisabledDatesTextStyle;
            }

            //// Check the min and max year and month are same also year is greater or lesser than current date for year view.
            bool isBeforeMinDate = (_calendarViewInfo.MinimumDate.Year == dateTime.Year && _calendarViewInfo.MinimumDate.Month > dateTime.Month) || _calendarViewInfo.MinimumDate.Year > dateTime.Year;
            bool isAfterMaxDate = (_calendarViewInfo.MaximumDate.Year == dateTime.Year && _calendarViewInfo.MaximumDate.Month < dateTime.Month) || _calendarViewInfo.MaximumDate.Year < dateTime.Year;

            //// If it is dates is out of range then change new text style and backgrounds are applied based on view.
            if (!isShowOutOfRangeDates && (isBeforeMinDate || isAfterMaxDate) && view == CalendarView.Year)
            {
                fillColor = disabledBackground;
                return CalendarViewHelper.GetOutOfRangeDatesTextStyle();
            }
            //// Check the min and max year is greater or lesser than current year for decade view.
            else if (!isShowOutOfRangeDates && (_calendarViewInfo.MinimumDate.Year > dateTime.Year || _calendarViewInfo.MaximumDate.Year < dateTime.Year) && view == CalendarView.Decade)
            {
                fillColor = disabledBackground;
                return CalendarViewHelper.GetOutOfRangeDatesTextStyle();
            }
            //// Check the min and max year is greater or lesser than current year with divided by 10 for century view.
            else if (!isShowOutOfRangeDates && (_calendarViewInfo.MinimumDate.Year / 10 > dateTime.Year / 10 || _calendarViewInfo.MaximumDate.Year / 10 < dateTime.Year / 10) && view == CalendarView.Century)
            {
                fillColor = disabledBackground;
                return CalendarViewHelper.GetOutOfRangeDatesTextStyle();
            }

            //// Disabled text styles and Background
            else if (isDisabledDate)
            {
                fillColor = disabledBackground;
                textStyle = yearCellStyle.DisabledDatesTextStyle;
            }
            //// Today text styles and Background
            else if (isTodayDate)
            {
                fillColor = CalendarViewHelper.ToColor(yearCellStyle.TodayBackground);
                textStyle = yearCellStyle.TodayTextStyle;
            }
            //// Leading Text styles and Background
            else if (isLeadingDate)
            {
                fillColor = leadingBackground;
                textStyle = yearCellStyle.LeadingDatesTextStyle;
            }

            return textStyle;
        }

        /// <summary>
        /// Method to draw the Today highlight shape for year, decade and Century views
        /// </summary>
        /// <param name="canvas">The draw canvas.</param>
        /// <param name="highlightRect">The year cell highlight rect values based on its text size.</param>
        /// <param name="rectCornerRadius">The rectangle corner radius</param>
        /// <param name="circleCornerRadius">The circle corner radius</param>
        void DrawTodayHighlight(ICanvas canvas, RectF highlightRect, float rectCornerRadius, float circleCornerRadius)
        {
            float strokeSize = 1;
            canvas.StrokeSize = strokeSize;
            float stokeSizePadding = strokeSize / 2;
            RectF todayHighlightRect = new RectF(highlightRect.X + stokeSizePadding, highlightRect.Y + stokeSizePadding, highlightRect.Width - strokeSize, highlightRect.Height - strokeSize);
            canvas.StrokeColor = _calendarViewInfo.TodayHighlightBrush.ToColor();
            if (_calendarViewInfo.SelectionShape == CalendarSelectionShape.Circle)
            {
                canvas.DrawRoundedRectangle(todayHighlightRect, circleCornerRadius);
            }
            else
            {
                canvas.DrawRoundedRectangle(todayHighlightRect, rectCornerRadius);
            }
        }

        /// <summary>
        /// Method to draw the selection highlight shape for year, decade and Century views
        /// </summary>
        /// <param name="canvas">The draw canvas.</param>
        /// <param name="highlightRect">The year cell highlight rect values based on its text size.</param>
        /// <param name="rectCornerRadius">The rectangle corner radius</param>
        /// <param name="circleCornerRadius">The circle corner radius</param>
        /// <param name="backgroundColor">The background Color to fill</param>
        void DrawSelectionHighlight(ICanvas canvas, RectF highlightRect, float rectCornerRadius, float circleCornerRadius, Color backgroundColor)
        {
            canvas.FillColor = backgroundColor;
            if (_calendarViewInfo.SelectionShape == CalendarSelectionShape.Circle)
            {
                canvas.FillRoundedRectangle(highlightRect, circleCornerRadius);
            }
            else
            {
                canvas.FillRoundedRectangle(highlightRect, rectCornerRadius);
            }
        }

        /// <summary>
        /// Method to draw the Range selection highlight
        /// </summary>
        /// <param name="canvas">The draw canvas.</param>
        /// <param name="rect">The Position, width and height value</param>
        /// <param name="highlightRect">The Position, width and height value for the highlight based on the text size.</param>
        /// <param name="rectCornerRadius">The rectangle corner radius</param>
        /// <param name="circleCornerRadius">The circle corner radius</param>
        /// <param name="selectionStatus">the selection status</param>
        /// <param name="textStyle">The calendar text style</param>
        /// <param name="startRangeSelectionBackground">To fill the start date selection background</param>
        /// <param name="endRangeSelectionBackground">To fill the end date selection background</param>
        /// <param name="inBetweenRangeBackground">To fill the inBetween date selection Background</param>
        /// <param name="columnIndex">The column index for the year cell.</param>
        CalendarTextStyle DrawRangeSelectionHighlight(ICanvas canvas, RectF rect, RectF highlightRect, float rectCornerRadius, float circleCornerRadius, SelectedRangeStatus? selectionStatus, CalendarTextStyle textStyle, Color startRangeSelectionBackground, Color endRangeSelectionBackground, Color inBetweenRangeBackground, int columnIndex)
        {
            // To highlight the Start date of the Range selection
            if (selectionStatus == SelectedRangeStatus.StartRange)
            {
                textStyle = _calendarViewInfo.YearView.SelectionTextStyle;
                if (_calendarViewInfo.IsRTLLayout)
                {
                    DrawEndRangeSelectionHighlight(canvas, rect, highlightRect, circleCornerRadius, rectCornerRadius, startRangeSelectionBackground);
                }
                else
                {
                    DrawStartRangeSelectionHighlight(canvas, rect, highlightRect, circleCornerRadius, rectCornerRadius, startRangeSelectionBackground);
                }
            }

            // To highlight the End date of the Range selection
            else if (selectionStatus == SelectedRangeStatus.EndRange)
            {
                textStyle = _calendarViewInfo.YearView.SelectionTextStyle;
                if (_calendarViewInfo.IsRTLLayout)
                {
                    DrawStartRangeSelectionHighlight(canvas, rect, highlightRect, circleCornerRadius, rectCornerRadius, endRangeSelectionBackground);
                }
                else
                {
                    DrawEndRangeSelectionHighlight(canvas, rect, highlightRect, circleCornerRadius, rectCornerRadius, endRangeSelectionBackground);
                }
            }

            // To highlight the inbetween date of the range selection
            else if (selectionStatus == SelectedRangeStatus.InBetweenRange)
            {
                textStyle = _calendarViewInfo.YearView.RangeTextStyle;
                canvas.FillColor = inBetweenRangeBackground;
                //// Handle the padding for drawing at right side end of selection,
                //// because range selection UI from current view to next view shown like
                //// a continuous one while swipe the view.
                float highlightWidth = columnIndex == 2 ? rect.Width - 1 : rect.Width;
                RectF selectionHighlightRect = new RectF(rect.X, highlightRect.Y, highlightWidth, highlightRect.Height);
                canvas.FillRectangle(selectionHighlightRect);
            }

            // To highlight the start date when the end date is null
            else if (selectionStatus == SelectedRangeStatus.SelectedRange)
            {
                textStyle = _calendarViewInfo.YearView.SelectionTextStyle;
                DrawSelectionHighlight(canvas, highlightRect, rectCornerRadius, circleCornerRadius, startRangeSelectionBackground);
            }

            return textStyle;
        }

        /// <summary>
        /// Occurs on tap interaction in the Year view Panel
        /// </summary>
        /// <param name="tapPoint">The interaction point.</param>
        /// <param name="isTapped">Is tapped.</param>
        /// <param name="tapCount">The tap count.</param>
        DateTime? OnInteractionEvent(Point tapPoint, bool isTapped, int tapCount = 1)
        {
            DateTime? tappedDate = CalendarViewHelper.GetYearDateFromPosition(tapPoint, Width, Height, _calendarViewInfo, _visibleDates);
            if (tappedDate == null || !CalendarViewHelper.IsInteractableDate(tappedDate.Value, _disabledDates, _visibleDates, _calendarViewInfo, 0))
            {
                return null;
            }

            if (_calendarViewInfo.AllowViewNavigation)
            {
                _calendarViewInfo.TriggerCalendarInteractionEvent(isTapped, tapCount, tappedDate.Value, CalendarElement.CalendarCell);
            }

            return tappedDate;
        }

        /// <summary>
        /// Method to update the swipe selection for range selection and multi range selection.
        /// <param name="status">The pan gesture status.</param>
        /// <param name="tappedDate">Interacted date when swipe</param>
        /// <param name="selectedRange">The selected date range.</param>
        /// </summary>
        void UpdateRangeSelectionOnSwipe(GestureStatus status, DateTime? tappedDate, CalendarDateRange? selectedRange)
        {
            CalendarDateRange currentSelectedRange;
            if (tappedDate == null)
            {
                //// Need to reset the initialRangeStartDate value while the touch completed or canceled on disabled date
                if (status == GestureStatus.Completed || status == GestureStatus.Canceled)
                {
                    _initialRangeStartDate = null;
                }

                return;
            }

            //// Bool property is used whether need to create new instance to update the selected date range
            bool isNewRange = false;

            // If the Pan is started, it updates the start date
            if (status == GestureStatus.Started)
            {
                currentSelectedRange = new CalendarDateRange(tappedDate, null);
                _initialRangeStartDate = tappedDate;
                //// Need to set isNewRange is true to create the new instance when swiping is started
                isNewRange = true;
                _calendarViewInfo.UpdateSwipeSelection(currentSelectedRange, isNewRange);
            }

            // When it is running, it updates the end date
            else if (status == GestureStatus.Running)
            {
                // When swiping from the disabled date, the initial start date becomes null
                // So need to update the start date in which is from first intractable date
                if (_initialRangeStartDate == null)
                {
                    currentSelectedRange = new CalendarDateRange(tappedDate, null);
                    _initialRangeStartDate = tappedDate;
                    //// Need to set isNewRange is true to create the new instance when swiping is started
                    isNewRange = true;
                }

                // If the initial range start date and the tapped date is same date, need to update the end date as null
                else if (CalendarViewHelper.IsSameDate(_calendarViewInfo.View, tappedDate, _initialRangeStartDate, _calendarViewInfo.Identifier))
                {
                    currentSelectedRange = new CalendarDateRange(tappedDate, null);
                    //// When the user is swiping on the same cell, no need to update the range.
                    if (CalendarViewHelper.IsSameRange(_calendarViewInfo.View, selectedRange, currentSelectedRange, _calendarViewInfo.Identifier))
                    {
                        return;
                    }
                }

                // If the initial range start date is smaller than the tapped date, need to update the end date as tapped date
                else if (_initialRangeStartDate < tappedDate)
                {
                    if (CalendarViewHelper.IsSameDate(_calendarViewInfo.View, tappedDate, selectedRange?.EndDate, _calendarViewInfo.Identifier))
                    {
                        return;
                    }

                    currentSelectedRange = new CalendarDateRange(_initialRangeStartDate, tappedDate);
                }

                // If the initial range start date is greater than the tapped date, need to update the end date as initial start range date
                else
                {
                    if (CalendarViewHelper.IsSameDate(_calendarViewInfo.View, tappedDate, selectedRange?.StartDate, _calendarViewInfo.Identifier))
                    {
                        return;
                    }

                    currentSelectedRange = new CalendarDateRange(tappedDate, _initialRangeStartDate);
                }

                _calendarViewInfo.UpdateSwipeSelection(currentSelectedRange, isNewRange);
            }
            //// Need to reset the initialRangeStartDate value while the touch completed or canceled status
            else if (status == GestureStatus.Completed || status == GestureStatus.Canceled)
            {
                _initialRangeStartDate = null;
            }
        }

        /// <summary>
        /// Method to check whether the date is selected date based on the selection mode.
        /// </summary>
        /// <param name="isBlackoutDate">Defines whether the date is blackout date or not.</param>
        /// <param name="isDisabledDate">Defines whether the date is disabled date or not.</param>
        /// <param name="dateTime">The calendar date time.</param>
        /// <returns>Returns true if the date is selected date and false if the date is not selected date.</returns>
        bool IsSelectedDate(bool isBlackoutDate, bool isDisabledDate, DateTime dateTime)
        {
            if (_calendarViewInfo.AllowViewNavigation || isDisabledDate || isBlackoutDate)
            {
                return false;
            }

            switch (_calendarViewInfo.SelectionMode)
            {
                case CalendarSelectionMode.Single:
                    return CalendarViewHelper.IsSameDate(_calendarViewInfo.View, _selectedDate, dateTime, _calendarViewInfo.Identifier);
                case CalendarSelectionMode.Multiple:
                    foreach (DateTime date in _selectedDates)
                    {
                        if (CalendarViewHelper.IsSameDate(_calendarViewInfo.View, date, dateTime, _calendarViewInfo.Identifier))
                        {
                            return true;
                        }
                    }

                    return false;
            }

            return false;
        }

        /// <summary>
        /// Method to get the last date in the visible date collection.
        /// </summary>
        /// <returns>Returns the last date.</returns>
        DateTime GetVisibleEndDate()
        {
            DateTime visibleEndDate = _visibleDates[_visibleDates.Count - 1];
            Globalization.Calendar cultureCalendar = CalendarViewHelper.GetCalendar(_calendarViewInfo.Identifier.ToString());
            DateTime maxDate = cultureCalendar.MaxSupportedDateTime.Date;
            int maxDateYear = cultureCalendar.GetYear(maxDate);
            switch (_calendarViewInfo.View)
            {
                case CalendarView.Month:
                    break;
                case CalendarView.Year:
                case CalendarView.Decade:
                    {
                        int visibleEndDateYear = cultureCalendar.GetYear(visibleEndDate);
                        //// In year view, if we interacted with December 2022, the date would be 1/12/2022.
                        //// We need the last date of December to check whether the date is within the visible date.
                        //// If the user gives 12/12/2022, then the condition fails and the draw will not be triggered.
                        //// This is the same for the decade view also. If we interacted with 2031, the date would be 1/1/2031.
                        return visibleEndDateYear == maxDateYear ? maxDate :
                           new DateTime(visibleEndDateYear, 12, cultureCalendar.GetDaysInMonth(visibleEndDateYear, 12), cultureCalendar);
                    }

                case CalendarView.Century:
                    {
                        int visibleEndDateYear = cultureCalendar.GetYear(visibleEndDate);
                        //// In century view, if we interacted with 2010-2019 the date would be 1/1/2010.
                        //// If the user gives dates between 2010 and 2019 the draw will not be triggered.
                        //// So here we are adding 9 to get the last date 31/12/2019.
                        int lastDateYear = visibleEndDateYear + 9;
                        if (lastDateYear >= maxDateYear)
                        {
                            return maxDate;
                        }

                        return new DateTime(lastDateYear, 12, cultureCalendar.GetDaysInMonth(lastDateYear, 12), cultureCalendar);
                    }
            }

            return visibleEndDate;
        }

        /// <summary>
        /// Method to get the valid selected range
        /// </summary>
        /// <returns>Returns the range</returns>
        CalendarDateRange? GetValidSelectedRange(CalendarDateRange? selectedDateRange)
        {
            //// If the selected date range is null or start date and end date is also null, need to return null
            if (selectedDateRange == null || (selectedDateRange.StartDate == null && selectedDateRange.EndDate == null))
            {
                return null;
            }
            //// If the start date is greater than the end date need to swap the dates
            else if ((selectedDateRange?.StartDate).IsGreaterDate(_calendarViewInfo.View, selectedDateRange?.EndDate, _calendarViewInfo.Identifier))
            {
                return new CalendarDateRange(selectedDateRange?.EndDate, selectedDateRange?.StartDate);
            }

            //// If the start and end date are different and start is smaller than end date, need to update the dates as such
            return new CalendarDateRange(selectedDateRange?.StartDate, selectedDateRange?.EndDate);
        }

        /// <summary>
        /// Method to update private variable selected date ranges value.
        /// </summary>
        /// <param name="selectedDateRanges">The selected date ranges value.</param>
        void UpdateSelectedDateRangesValue(ObservableCollection<CalendarDateRange>? selectedDateRanges)
        {
            if (selectedDateRanges == null || selectedDateRanges.Count == 0)
            {
                _selectedDateRanges = null;
            }
            else
            {
                _selectedDateRanges = new ObservableCollection<CalendarDateRange>();
                foreach (CalendarDateRange range in selectedDateRanges)
                {
                    DateTime? startDate = range.StartDate?.Date;
                    DateTime? endDate = range.EndDate?.Date;
                    //// If the start date is greater than the end date need to swap the dates
                    if (startDate.IsGreaterDate(_calendarViewInfo.View, endDate, _calendarViewInfo.Identifier))
                    {
                        _selectedDateRanges.Add(new CalendarDateRange(endDate, startDate));
                    }
                    else
                    {
                        _selectedDateRanges.Add(new CalendarDateRange(startDate, endDate));
                    }
                }
            }
        }

        /// <summary>
        /// Method to get the selection status for Range selection
        /// </summary>
        /// <param name="dateRange">The selected date range.</param>
        /// <param name="dateTime">The current date in the loop</param>
        /// <returns>Returns the string based on the condition</returns>
        SelectedRangeStatus? GetSelectionStatus(CalendarDateRange dateRange, DateTime? dateTime)
        {
            if (dateRange == null)
            {
                return null;
            }

            bool isStartDate = CalendarViewHelper.IsSameDate(_calendarViewInfo.View, dateRange?.StartDate, dateTime, _calendarViewInfo.Identifier);
            DateTime? endDate = dateRange?.EndDate;
            if (endDate == null)
            {
                endDate = dateRange?.StartDate;
            }

            bool isEndDate = CalendarViewHelper.IsSameDate(_calendarViewInfo.View, endDate, dateTime, _calendarViewInfo.Identifier);

            // If the start date range is only selected or else start and end date is same or else end date is null
            // Then the date is considered as selected date range
            if (isStartDate && isEndDate)
            {
                return SelectedRangeStatus.SelectedRange;
            }

            // If the start range is selected and also end date is not null the date is considered as start range
            // Start date must be smaller than the end date
            else if (isStartDate)
            {
                return SelectedRangeStatus.StartRange;
            }

            // If the end date is selected then the date is considered as end date
            // End date must be greater than the start date
            else if (isEndDate)
            {
                return SelectedRangeStatus.EndRange;
            }

            // If the start date and end date is selected then the in between dates are considered as inBetweenRange
            else if (CalendarViewHelper.IsInBetweenSelectedRange(_calendarViewInfo.View, dateRange, dateTime, _calendarViewInfo.Identifier))
            {
                return SelectedRangeStatus.InBetweenRange;
            }

            return null;
        }

        /// <summary>
        /// Returns the selection status for the date on range selection..
        /// </summary>
        /// <param name="dateTime">Current date value.</param>
        /// <returns>Returns selection status for the date on range selection.</returns>
        SelectedRangeStatus? GetRangeSelectionStatus(DateTime? dateTime)
        {
            if (_selectedDateRange == null)
            {
                return null;
            }

            DateTime? date = dateTime?.Date;
            return GetSelectionStatus(_selectedDateRange, date);
        }

        /// <summary>
        /// Method to get the multi range selection status.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <param name="visibleSelectedDateRanges">The visible selected date ranges.</param>
        /// <returns>Returns the selected date ranges status.</returns>
        SelectedRangeStatus? GetMultiRangeSelectionStatus(DateTime? dateTime, ObservableCollection<CalendarDateRange>? visibleSelectedDateRanges)
        {
            if (visibleSelectedDateRanges == null || visibleSelectedDateRanges.Count == 0)
            {
                return null;
            }

            DateTime? date = dateTime?.Date;
            foreach (var selectedRange in visibleSelectedDateRanges)
            {
                SelectedRangeStatus? status = GetSelectionStatus(selectedRange, date);
                if (status != null)
                {
                    return status;
                }
            }

            return null;
        }

        /// <summary>
        /// Method to check the current range and previous range is in visible dates
        /// </summary>
        /// <param name="startDate">Start range value</param>
        /// <param name="endDate">End range value</param>
        /// <returns>Returns true while the range is within the visible dates</returns>
        bool IsRangeInVisibleDates(DateTime? startDate, DateTime? endDate)
        {
            DateTime firstDate = _visibleDates[0];
            DateTime lastDate = _visibleDates[_visibleDates.Count - 1];

            // Bool property to check the range is in the visible dates collection
            bool isRangeInVisibleDates = CalendarViewHelper.IsDateWithinDateRange(startDate, firstDate, lastDate) || CalendarViewHelper.IsDateWithinDateRange(endDate, firstDate, lastDate) || (startDate <= firstDate && endDate >= lastDate);

            // Return the start date or end date is within the collection
            return isRangeInVisibleDates;
        }

        /// <summary>
        /// Method to check the view can be draw for range based on selection direction
        /// </summary>
        /// <param name="previousStartDate">Previous selected range start date </param>
        /// <param name="previousEndDate">Previous selected range end date</param>
        /// <param name="selectedDateRange">Current Selected date range</param>
        /// <returns>Returns true when the view is need to redraw</returns>
        bool IsViewNeedUpdateRangeSelection(DateTime? previousStartDate, DateTime? previousEndDate, CalendarDateRange? selectedDateRange)
        {
            DateTime? currentStartDate = CalendarViewHelper.GetStartDate(selectedDateRange?.StartDate, _calendarViewInfo.View, _calendarViewInfo.Identifier);
            DateTime? currentEndDate = CalendarViewHelper.GetStartDate(selectedDateRange?.EndDate, _calendarViewInfo.View, _calendarViewInfo.Identifier);
            DateTime firstVisibleDate = _visibleDates[0];
            DateTime lastVisibleDate = _visibleDates[_visibleDates.Count - 1];
            if (_calendarViewInfo.SelectionMode == CalendarSelectionMode.Range)
            {
                switch (_calendarViewInfo.RangeSelectionDirection)
                {
                    case CalendarRangeSelectionDirection.Default:
                    case CalendarRangeSelectionDirection.Both:
                        break;

                    case CalendarRangeSelectionDirection.Forward:
                        //// If the start date is greater than the previous view visible date, need to apply disabled date property to the previous view
                        //// So, need to update the previous view
                        if (previousStartDate == null && currentStartDate.IsGreaterDate(_calendarViewInfo.View, firstVisibleDate, _calendarViewInfo.Identifier))
                        {
                            return true;
                        }
                        //// If the start date and previous range start date is not same, need to update the view
                        else if (previousStartDate != null && !CalendarViewHelper.IsSameDate(_calendarViewInfo.View, currentStartDate, previousStartDate, _calendarViewInfo.Identifier))
                        {
                            return CalendarViewHelper.IsDateWithinDateRange(firstVisibleDate, previousStartDate, currentStartDate) || CalendarViewHelper.IsDateWithinDateRange(firstVisibleDate, currentStartDate, previousStartDate);
                        }

                        break;

                    case CalendarRangeSelectionDirection.Backward:
                        //// If the visible date is greater than the end date need to update the view
                        if (previousEndDate == null && CalendarViewHelper.IsGreaterDate(firstVisibleDate, _calendarViewInfo.View, currentEndDate, _calendarViewInfo.Identifier))
                        {
                            return true;
                        }
                        //// if the end date and previous end date is not same, need to update the view
                        else if (previousEndDate != null && !CalendarViewHelper.IsSameDate(_calendarViewInfo.View, selectedDateRange?.EndDate, previousEndDate, _calendarViewInfo.Identifier))
                        {
                            return CalendarViewHelper.IsDateWithinDateRange(firstVisibleDate, previousEndDate, currentEndDate) || CalendarViewHelper.IsDateWithinDateRange(firstVisibleDate, currentEndDate, previousEndDate);
                        }

                        break;

                    case CalendarRangeSelectionDirection.None:
                        previousEndDate = previousEndDate == null ? previousStartDate : previousEndDate;
                        currentEndDate = currentEndDate == null ? currentStartDate : currentEndDate;
                        bool isValidOldRange = !CalendarViewHelper.IsSameDate(_calendarViewInfo.View, previousStartDate, previousEndDate, _calendarViewInfo.Identifier);
                        bool isValidNewRange = !CalendarViewHelper.IsSameDate(_calendarViewInfo.View, currentStartDate, currentEndDate, _calendarViewInfo.Identifier);

                        // The previous range and new range is not a valid range(range start and end date are different), So need to draw new range and remove previous range.
                        // Example: previous range = (Nov-15 to null) and new range (Nov-18 to Nov-18).
                        if (!isValidOldRange && !isValidNewRange)
                        {
                            return IsRangeInVisibleDates(currentStartDate, currentEndDate) || IsRangeInVisibleDates(previousStartDate, previousEndDate);
                        }

                        // The previous range is not a valid range(range start and end date are different), So need to remove previous range, draw new range and draw disabled date(Before new range start date and after new range end date).
                        // Example: previous range = (Nov-15 to null) and new range (Nov-18 to Nov-20).
                        else if (!isValidOldRange)
                        {
                            return IsRangeInVisibleDates(currentStartDate, currentEndDate) || IsRangeInVisibleDates(previousStartDate, previousEndDate) || currentStartDate >= firstVisibleDate || currentEndDate <= lastVisibleDate;
                        }

                        // The new range is not a valid range(range start and end date are different), So need to draw new range, remove previous range and remove disabled date(Before old range start date and after old range end date).
                        // Example: previous range = (Nov-15 to null) and new range (Nov-18 to Nov-20).
                        else if (!isValidNewRange)
                        {
                            return IsRangeInVisibleDates(currentStartDate, currentEndDate) || IsRangeInVisibleDates(previousStartDate, previousEndDate) || previousStartDate >= firstVisibleDate || previousEndDate <= lastVisibleDate;
                        }

                        // The previous range and new range is a valid range(range start and end date are different), So need to draw inbetween new range and and previous range.
                        // Example: previous range = (Nov-15 to 18) and new range (Nov-18 to Nov-20), So need to draw Nov month only.
                        // Example: previous range = (Nov-15 to 18) and new range (Dec-18 to Dec-20), So need to draw Nov and Dec month only.
                        // Example: previous range = (Nov-15 to 18) and new range (Sep-18 to Sep-20), So need to draw Sep, Oct and Nov month only.
                        else
                        {
                            if (previousStartDate > currentStartDate)
                            {
                                DateTime? dateTime = previousStartDate;
                                previousStartDate = currentStartDate;
                                currentStartDate = dateTime;
                            }

                            if (previousEndDate > currentEndDate)
                            {
                                DateTime? dateTime = previousEndDate;
                                previousEndDate = currentEndDate;
                                currentEndDate = dateTime;
                            }

                            return IsRangeInVisibleDates(previousStartDate, currentStartDate) || IsRangeInVisibleDates(previousEndDate, currentEndDate);
                        }
                }
            }

            return false;
        }

        /// <summary>
        /// Method to invalidate view measures.
        /// </summary>
        void InvalidateViewMeasure()
        {
#if __ANDROID__
            MeasureContent(Width, Height);
            ArrangeContent(new Rect(0, 0, Width, Height));
#else
            InvalidateMeasure();
#endif
        }

        /// <summary>
        /// Method to remove the year cells and its handlers.
        /// </summary>
        void RemoveYearCellsHandler()
        {
            if (_yearCells == null)
            {
                return;
            }

            foreach (View yearCell in _yearCells)
            {
                Remove(yearCell);
                if (yearCell.Handler != null && yearCell.Handler.PlatformView != null)
                {
                    yearCell.Handler.DisconnectHandler();
                }
            }

            _yearCells.Clear();
            _yearCells = null;
        }

        /// <summary>
        /// Method to get the cell template details for the year view.
        /// </summary>
        /// <param name="dateTime">The date time value.</param>
        /// <param name="currentViewDate">The current view date value.</param>
        /// <returns>Returns the year cell details.</returns>
        CalendarCellDetails GetYearCellDetails(DateTime dateTime, DateTime currentViewDate)
        {
            //// Create a new CalendarYearCellDetails object to get the details
            CalendarCellDetails yearCellDetails = new CalendarCellDetails();
            yearCellDetails.Date = dateTime;
            yearCellDetails.IsTrailingOrLeadingDate = CalendarViewHelper.IsLeadingAndTrailingDate(dateTime, currentViewDate, _calendarViewInfo.View, _calendarViewInfo.Identifier);
            return yearCellDetails;
        }

        /// <summary>
        /// Method to Generate the year view cells for Cell template and Template selector
        /// </summary>
        /// <param name="isCurrentView">Returns true if it is current view</param>
        void GenerateYearCells(bool isCurrentView)
        {
            DataTemplate? template = _calendarViewInfo.YearView.CellTemplate;
            if (template == null)
            {
                return;
            }

            //// To check if the "template" variable is a DataTemplateSelector object to determine the appearance of the cells
            if (template is DataTemplateSelector selector)
            {
                //// If it is current view, then need to generate the template cells
                if (isCurrentView)
                {
                    GenerateTemplateSelectorYearCells(selector);
                }
                else
                {
                    //// If it is not current view, dispatch a delegate to the UI thread.
                    //// Method inside the dispatcher trigger once the current thread completed.
                    Dispatcher.Dispatch(() =>
                    {
                        //// Generate the year cells using the template selector
                        GenerateTemplateSelectorYearCells(selector);
                        //// Need to invalidate the views because this method triggered after the main thread.
                        InvalidateViewMeasure();
                    });
                }
            }
            //// If the template is not a template selector
            else
            {
                DateTime currentViewDate = _visibleDates[0];
                int maxChildCount = RowCount * ColumnCount;
                //// Need to set the maxChildCount as 10 for the Century and Decade view, while the ShowTrailingAndLeadingDates is set to false.
                if (!_calendarViewInfo.ShowTrailingAndLeadingDates && _calendarViewInfo.View != CalendarView.Year)
                {
                    maxChildCount = 10;
                }

                int index = 0;
                foreach (DateTime dateTime in _visibleDates)
                {
                    //// Get the year cell details for the current date
                    CalendarCellDetails details = GetYearCellDetails(dateTime, currentViewDate);
                    //// If the cell is leading and trailing cell and if ShowTrailingAndLeadingDates is false, then no need to create year cells.
                    if (details.IsTrailingOrLeadingDate && !_calendarViewInfo.ShowTrailingAndLeadingDates)
                    {
                        continue;
                    }

                    //// Create the year cell template view using the template, cell details and index
                    CreateYearCellTemplateView(template, details, index);
                    index++;
                }

                if (index != maxChildCount)
                {
                    int neededViewCount = maxChildCount - index;
                    for (int i = 0; i < neededViewCount; i++)
                    {
                        CreateYearCellTemplateView(template, null, index);
                        index++;
                    }
                }
            }
        }

        /// <summary>
        /// Method to generate year cells using a DataTemplateSelector.
        /// </summary>
        /// <param name="templateSelector">The template selector argument</param>
        void GenerateTemplateSelectorYearCells(DataTemplateSelector templateSelector)
        {
            DateTime currentViewDate = _visibleDates[0];
            for (int i = 0; i < _visibleDates.Count; i++)
            {
                CalendarCellDetails details = GetYearCellDetails(_visibleDates[i], currentViewDate);
                //// If the cell is leading and trailing cell and if ShowTrailingAndLeadingDates is false, then no need to create year cells.
                if (details.IsTrailingOrLeadingDate && !_calendarViewInfo.ShowTrailingAndLeadingDates)
                {
                    continue;
                }

                //// Get the year cell details for the current date
                DataTemplate template = templateSelector.SelectTemplate(details, _calendarViewInfo.YearView);
                //// Select the template using the templateSelector and yearCellDetails, this.calendarViewInfo.YearView
                CreateYearCellTemplateView(template, details, i);
            }
        }

        /// <summary>
        /// Method to create the template for the year view cells.
        /// </summary>
        /// <param name="template">The template parameter</param>
        /// <param name="cellDetails">Cell details of the year view</param>
        /// <param name="index">Index value of the date</param>
        void CreateYearCellTemplateView(DataTemplate template, CalendarCellDetails? cellDetails, int index)
        {
            //// Create a view from the given data template and cell details whether the layout is RTL
            View yearCellTemplateview = CalendarViewHelper.CreateTemplateView(template, cellDetails);
            //// Add the view to the year cells.
            _yearCells?.Add(yearCellTemplateview);
            //// Insert the view at the specified index.
            Insert(index, yearCellTemplateview);
        }

        /// <summary>
        /// Method to update the year cell template views.
        /// </summary>
        /// <param name="isRequestMeasure">Check the visible date update need measure request.</param>
        void UpdateYearCellTemplateViews(bool isRequestMeasure)
        {
            //// To get the Cell Template for the year view.
            DataTemplate? template = _calendarViewInfo.YearView.CellTemplate;
            if (template == null)
            {
                return;
            }

            if (template is DataTemplateSelector)
            {
                RemoveYearCellsHandler();
                _yearCells = new List<View>();
                GenerateTemplateSelectorYearCells((DataTemplateSelector)template);
            }
            else
            {
                if (_yearCells == null)
                {
                    return;
                }

                int index = 0;
                DateTime currentViewDate = _visibleDates[0];
                //// If the template is cell template
                foreach (DateTime dateTime in _visibleDates)
                {
                    //// Get the details for the current year cell
                    CalendarCellDetails details = GetYearCellDetails(dateTime, currentViewDate);
                    //// If the cellDetails is leading date and if ShowTrailingAndLeadingDates is false, then return without adding the view.
                    if (details.IsTrailingOrLeadingDate && !_calendarViewInfo.ShowTrailingAndLeadingDates)
                    {
                        if (index < _yearCells.Count)
                        {
                            _yearCells[index].IsVisible = false;
                        }

                        continue;
                    }

                    //// Skip the update when the index value is greater than month cells count.
                    if (index >= _yearCells.Count)
                    {
                        continue;
                    }

                    View yearCell = _yearCells[index];
                    yearCell.IsVisible = true;
                    yearCell.BindingContext = details;
                    index++;
                }

                //// Here, we need to update the year cells
                //// while visible dates count changed and visible cell count changes.
                if (isRequestMeasure)
                {
                    InvalidateViewMeasure();
                }
            }
        }

        /// <summary>
        /// Method to update the year cell based on the cell count
        /// </summary>
        /// <param name="template">The Template parameter</param>
        /// <param name="currentViewDate">Returns the current view date</param>
        void UpdateYearCellCount(DataTemplate template, DateTime currentViewDate)
        {
            if (_yearCells == null)
            {
                return;
            }

            int maxChildCount = RowCount * ColumnCount;
            //// Need to set the maxChildCount as 10 for the Century and Decade view, while the ShowTrailingAndLeadingDates is set to false.
            if (!_calendarViewInfo.ShowTrailingAndLeadingDates && _calendarViewInfo.View != CalendarView.Year)
            {
                maxChildCount = 10;
            }

            // The index variable defines how many dates have to bind to the views
            int count = maxChildCount - _yearCells.Count;
            //// To remove the unneeded cells from the year count.
            if (count < 0)
            {
                for (int i = 0; i < Math.Abs(count); i++)
                {
                    View yearCell = _yearCells[_yearCells.Count - 1];
                    Remove(yearCell);
                    _yearCells.Remove(yearCell);
                    if (yearCell.Handler != null && yearCell.Handler.PlatformView != null)
                    {
                        yearCell.Handler.DisconnectHandler();
                    }
                }
            }
            //// To create the view based on the cell count.
            else if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    CreateYearCellTemplateView(template, null, _yearCells.Count);
                }
            }

            UpdateYearCellTemplateViews(true);
        }

        /// <summary>
        /// Method to get the current view selected date ranges.
        /// </summary>
        /// <param name="dateRangeCollection">The date range collection.</param>
        /// <returns>Returns the visible date range collection.</returns>
        ObservableCollection<CalendarDateRange>? GetVisibleSelectedDateRanges(ObservableCollection<CalendarDateRange> dateRangeCollection)
        {
            if (dateRangeCollection == null || dateRangeCollection.Count == 0)
            {
                return null;
            }

            ObservableCollection<CalendarDateRange> visibleRanges = new ObservableCollection<CalendarDateRange>();
            DateTime? firstVisibleDate = _visibleDates[0];
            DateTime? lastVisibleDate = CalendarViewHelper.GetLastDate(_calendarViewInfo.View, _visibleDates[_visibleDates.Count - 1], _calendarViewInfo.Identifier);
            foreach (CalendarDateRange range in dateRangeCollection)
            {
                DateTime? startDate = range.StartDate?.Date;
                DateTime? endDate = range.EndDate?.Date;

                // Check the start date and end date range is in between the current view visible dates collection.
                if (CalendarViewHelper.IsDateWithinDateRange(startDate, firstVisibleDate, lastVisibleDate) || CalendarViewHelper.IsDateWithinDateRange(endDate, firstVisibleDate, lastVisibleDate) || (firstVisibleDate.IsGreaterDate(_calendarViewInfo.View, startDate, _calendarViewInfo.Identifier) && endDate.IsGreaterDate(_calendarViewInfo.View, lastVisibleDate, _calendarViewInfo.Identifier)))
                {
                    visibleRanges.Add(range);
                }
            }

            if (visibleRanges.Count == 0)
            {
                return null;
            }

            return visibleRanges;
        }

        /// <summary>
        /// Method to check the current view previous selected ranges and new selected ranges are equal or not.
        /// </summary>
        /// <param name="oldSelectedRanges">The ranges.</param>
        /// <returns>Returns true if the ranges changes present in the current view.</returns>
        bool IsCurrentViewRangesUpdated(ObservableCollection<CalendarDateRange>? oldSelectedRanges)
        {
            //// If both ranges are null, no need to update the UI.
            if (oldSelectedRanges == _selectedDateRanges)
            {
                return false;
            }

            ObservableCollection<CalendarDateRange>? currentViewOldSelectedRanges = null;
            if (oldSelectedRanges != null)
            {
                currentViewOldSelectedRanges = GetVisibleSelectedDateRanges(oldSelectedRanges);
            }

            ObservableCollection<CalendarDateRange>? currentViewNewSelectedRanges = null;
            if (_selectedDateRanges != null)
            {
                currentViewNewSelectedRanges = GetVisibleSelectedDateRanges(_selectedDateRanges);
            }

            return !CalendarViewHelper.IsSameDateRanges(_calendarViewInfo.View, currentViewOldSelectedRanges, currentViewNewSelectedRanges, _calendarViewInfo.Identifier);
        }

        /// <summary>
        /// Method to update the selection while key navigation.
        /// </summary>
        /// <param name="args">The event args.</param>
        /// <param name="oldSelectedDate">The old selected date.</param>
        void UpdateSelectionWhileKeyNavigation(KeyEventArgs args, DateTime oldSelectedDate)
        {
            DateTime? newDate;
            DateTime? oldSelectedDates = oldSelectedDate;
            DateTime? startDate = _calendarViewInfo.SelectedDateRange?.StartDate?.Date;

            if (_calendarViewInfo.SelectionMode == CalendarSelectionMode.Multiple)
            {
                if (_lastSelectedDate == null)
                {
                    oldSelectedDates = oldSelectedDate;
                }
                else
                {
                    oldSelectedDates = _lastSelectedDate.Value.Date;
                    _lastSelectedDate = null;
                }
            }
            else if (_calendarViewInfo.SelectionMode == CalendarSelectionMode.Range)
            {
                if (_calendarViewInfo.SelectedDateRange == null || startDate == null)
                {
                    return;
                }

                //// To get the new date with the start date. Because the start date is changed or the last updated is start date.
                if (_previousSelectedDateRange != null && CalendarViewHelper.IsSameDate(_calendarViewInfo.View, _previousSelectedDateRange.Value, startDate, _calendarViewInfo.Identifier))
                {
                    oldSelectedDates = _previousSelectedDateRange.Value;
                }
            }

            newDate = CalendarViewHelper.GeKeyNavigationDate(_calendarViewInfo, args, (DateTime)oldSelectedDates);
            if (_calendarViewInfo.SelectionMode == CalendarSelectionMode.Multiple && newDate != null && CalendarViewHelper.IsDateInDateCollection(newDate.Value, _calendarViewInfo.SelectedDates))
            {
                _lastSelectedDate = newDate.Value;
                return;
            }

            CalendarViewHelper.ValidateDateOnKeyNavigation(args, oldSelectedDates, newDate, _calendarViewInfo, _visibleDates, _disabledDates);
        }

        #endregion

        #region Override Methods

        /// <summary>
        /// Method to draw the Year view cells.
        /// </summary>
        /// <param name="canvas">The draw canvas.</param>
        /// <param name="dirtyRect">The rectangle.</param>
        protected override void OnDraw(ICanvas canvas, RectF dirtyRect)
        {
            //// If the Width and Height value is less than or equal to zero, no need to draw the view.
            if (dirtyRect.Width <= 0 || dirtyRect.Height <= 0)
            {
                return;
            }

            canvas.SaveState();
            canvas.Antialias = true;
            float yearViewWidth = dirtyRect.Width;
            float yearViewHeight = dirtyRect.Height;
            bool isRTL = _calendarViewInfo.IsRTLLayout;

            // Number of column count in year, decade and Century view is 3
            float yearCellWidth = yearViewWidth / ColumnCount;

            // Number of row count in year, decade and Century view is 4
            float yearCellHeight = yearViewHeight / RowCount;
            float xPosition = isRTL ? yearViewWidth - yearCellWidth : 0;
            float yPosition = 0;
            float cellWidthOffset = isRTL ? -yearCellWidth : yearCellWidth;

            if (_calendarViewInfo.YearView.CellTemplate == null)
            {
                // Method to draw the year view panel.
                DrawYearViewPanelCells(canvas, xPosition, yPosition, isRTL, yearViewWidth, yearCellWidth, yearCellHeight, cellWidthOffset);
            }
            else
            {
                // Method to draw the year view panel selection.
                DrawTemplateSelection(canvas, xPosition, yPosition, isRTL, yearViewWidth, yearCellWidth, yearCellHeight, cellWidthOffset);
            }

            canvas.RestoreState();
        }

        /// <summary>
        /// Method to measure template views.
        /// </summary>
        /// <param name="widthConstraint">The available width.</param>
        /// <param name="heightConstraint">he available height.</param>
        /// <returns>Returns the actual size.</returns>
        protected override Size MeasureContent(double widthConstraint, double heightConstraint)
        {
            if (_calendarViewInfo.YearView.CellTemplate == null || _yearCells == null)
            {
                return base.MeasureContent(widthConstraint, heightConstraint);
            }

            double width = double.IsFinite(widthConstraint) ? widthConstraint : 0;
            double height = double.IsFinite(heightConstraint) ? heightConstraint : 0;
            double yearCellWidth = width / ColumnCount;
            double yearCellHeight = height / RowCount;
            foreach (View child in Children)
            {
#if MACCATALYST || (!ANDROID && !IOS)
                if (child is YearHoverView)
                {
                    child.Measure(width, height);
                    continue;
                }
#endif

                child.Measure(yearCellWidth, yearCellHeight);
            }

            return new Size(width, height);
        }

        /// <summary>
        /// Method to arrange the template views.
        /// </summary>
        /// <param name="bounds">The available size.</param>
        /// <returns>Returns the size.</returns>
        protected override Size ArrangeContent(Rect bounds)
        {
            if (_calendarViewInfo.YearView.CellTemplate == null || _yearCells == null)
            {
                return base.ArrangeContent(bounds);
            }

            double width = bounds.Width;
            double yearCellWidth = width / ColumnCount;
            double yearCellHeight = bounds.Height / RowCount;
            bool isRTL = _calendarViewInfo.IsRTLLayout;
            double yearCellXPosition = isRTL ? width - yearCellWidth : 0;
            double yPosition = 0;
            double cellWidthOffset = isRTL ? -yearCellWidth : yearCellWidth;
            int childIndex = 0;
            DateTime currentViewDate = _visibleDates[0];
            int maxCellCount = _visibleDates.Count;
            Globalization.Calendar cultureCalendar = CalendarViewHelper.GetCalendar(_calendarViewInfo.Identifier.ToString());
            int minYear = cultureCalendar.GetYear(cultureCalendar.MinSupportedDateTime);
            if (!_calendarViewInfo.ShowTrailingAndLeadingDates)
            {
                if (_calendarViewInfo.View == CalendarView.Decade)
                {
                    int currentYear = cultureCalendar.GetYear(currentViewDate);
                    maxCellCount = currentYear == minYear ? 10 - (minYear % 10) : 10;
                }
                else if (_calendarViewInfo.View == CalendarView.Century)
                {
                    maxCellCount = 10;
                }
            }

            foreach (View child in Children)
            {
#if MACCATALYST || (!ANDROID && !IOS)
                if (child is YearHoverView)
                {
                    AbsoluteLayout.SetLayoutBounds(child, new Rect(0, 0, bounds.Width, bounds.Height));
                    continue;
                }
#endif
                if (childIndex % ColumnCount == 0 && childIndex != 0)
                {
                    yearCellXPosition = isRTL ? width - yearCellWidth : 0;
                    yPosition += yearCellHeight;
                }

                //// If ShowLeadingAndTrailingDates set to false no need to show leading and trailing date views.
                //// When the view reaches the minimum and maximum date the visibility of the cells in the empty space must be set to false.
                if ((!_calendarViewInfo.ShowTrailingAndLeadingDates && maxCellCount <= childIndex) || childIndex >= _visibleDates.Count)
                {
                    child.IsVisible = false;
                    childIndex++;
                    continue;
                }

                child.IsVisible = true;
                AbsoluteLayout.SetLayoutBounds(child, new Rect(yearCellXPosition, yPosition, yearCellWidth, yearCellHeight));
                yearCellXPosition += cellWidthOffset;
                childIndex++;
            }

            return base.ArrangeContent(bounds);
        }

        /// <summary>
        /// Method to create the semantics node for each virtual view with bounds inside the year view.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <returns>Returns semantic virtual view.</returns>
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
            float yearViewWidth = (float)width;
            bool isRTL = _calendarViewInfo.IsRTLLayout;
            //// Number of column count in year, decade and Century view is 3
            float yearCellWidth = yearViewWidth / ColumnCount;
            //// Number of row count in year, decade and Century view is 4
            float yearCellHeight = (float)height / RowCount;
            float xPosition = isRTL ? yearViewWidth - yearCellWidth : 0;
            float yPosition = 0;
            float cellWidthOffset = isRTL ? -yearCellWidth : yearCellWidth;
            //// Calculate the horizontal padding between the year cell and its highlight from cell width value.
            //// Step1: Assign the year cell width and height as 100, 100 and horizontal padding as left 10 and right 10.
            float horizontalPadding = yearCellWidth * 0.1f;
            //// Calculate the highlight width based on the horizontal padding(left and right).
            //// Step2: highlight width value as 80.
            float highlightWidth = yearCellWidth - (2 * horizontalPadding);
            //// Calculate the initial year cell text from its visible dates for calculate the year cell highlight height.
            string yearCellText = CalendarViewHelper.GetYearCellText(_visibleDates[0], _calendarViewInfo);
            //// Holds the year cell text height based on its text style.
            //// Step4: Assume the text height as 40.
            float textHeight = (float)yearCellText.Measure(_calendarViewInfo.YearView.TextStyle).Height;
            //// Calculate the vertical space between year cell top position and year cell text.
            //// Step5: text top position as (100 - 40) * 0.5 = 30.
            float textTopPosition = (yearCellHeight - textHeight) * 0.5f;
            //// Check the year cell does not have vertical space.
            textTopPosition = textTopPosition < 0 ? 0 : textTopPosition;
            //// Assign the vertical highlight size as 30% of vertical space.
            //// Step6: vertical text padding value is 30 * 0.3 = 9.
            float verticalTextPadding = textTopPosition * 0.3f;
            //// Calculate the highlight padding from its top position by vertical space and vertical text padding.
            //// Step7: Padding above the highlight is 30 - 9 = 21.
            float verticalHighlightPadding = textTopPosition - verticalTextPadding;
            //// Calculate the highlight height from text height and vertical text padding.
            //// Step8: highlight height is 40 + (2* 9) = 58.
            float highlightHeight = textHeight + (2 * verticalTextPadding);
            for (int i = 0; i < _visibleDates.Count; i++)
            {
                DateTime dateTime = _visibleDates[i];
                int column = i % ColumnCount;
                //// If year arrangement reached to last column then need to start from the next row first column.
                if (column == 0 && i != 0)
                {
                    xPosition = isRTL ? yearViewWidth - yearCellWidth : 0;
                    yPosition += yearCellHeight;
                }

                //// Bool Property to check the Leading date
                bool isLeadingDate = CalendarViewHelper.IsLeadingAndTrailingDate(dateTime, _visibleDates[0], _calendarViewInfo.View, _calendarViewInfo.Identifier);
                //// If ShowLeadingAndTrailingDates set to false no need to draw leading and trailing date.
                if (isLeadingDate && !_calendarViewInfo.ShowTrailingAndLeadingDates)
                {
                    xPosition += cellWidthOffset;
                    continue;
                }

                bool isBlackoutDate = CalendarViewHelper.IsDateInDateCollection(dateTime, _disabledDates);
                bool isDisabledDate = CalendarViewHelper.IsDisabledDate(dateTime, _calendarViewInfo.View, _calendarViewInfo.EnablePastDates, _calendarViewInfo.MinimumDate, _calendarViewInfo.MaximumDate, _calendarViewInfo.SelectionMode, _calendarViewInfo.RangeSelectionDirection, _selectedDateRange, _calendarViewInfo.AllowViewNavigation, _calendarViewInfo.Identifier);
                string dateType = isBlackoutDate || isDisabledDate ? SfCalendarResources.GetLocalizedString("Disabled Cell") : string.Empty;
                //// Method to get the year cell date text
                //// Here pass the arguments as true then it considered the control accessibility is enabled.
                string dateText = CalendarViewHelper.GetYearCellText(dateTime, _calendarViewInfo, true) + dateType;
                float highlightXPosition = xPosition + horizontalPadding;
                float highlightYPosition = yPosition + verticalHighlightPadding;
                SemanticsNode node = new SemanticsNode()
                {
                    Id = i,
                    Text = dateText,
                    Bounds = new Rect(highlightXPosition, highlightYPosition, highlightWidth, highlightHeight),
                };

                _semanticsNodes.Add(node);
                xPosition += cellWidthOffset;
            }

            return _semanticsNodes;
        }

        #endregion

        #region Interface Implementation

#if __IOS__ || __MACCATALYST__

        /// <summary>
        /// Gets a value indicating whether the view can become the first responder to listen the keyboard actions.
        /// </summary>
        /// <remarks>This property will be considered only in maccatalyst and iOS Platform.</remarks>
        bool IKeyboardListener.CanBecomeFirstResponder
        {
            get
            {
                return true;
            }
        }

#endif

#if WINDOWS || __ANDROID__
        /// <summary>
        /// Gets a value indicating whether to handle both single tap and double tap in the view or not.
        /// </summary>
        bool IGestureListener.IsRequiredSingleTapGestureRecognizerToFail => false;
#endif

        /// <summary>
        /// Occurs on tap interaction inside the year view panel.
        /// </summary>
        /// <param name="e">The tap gesture listener event arguments.</param>
        void ITapGestureListener.OnTap(TapEventArgs e)
        {
            //// To set the focus for keyboard interaction after the tap interaction.
            Focus();
            DateTime? tappedDate = OnInteractionEvent(e.TapPoint, true, e.TapCount);

            // If the Allow view navigation is true, no need to select the cell
            // No need to update when the tapped date is null
            if (_calendarViewInfo.AllowViewNavigation || tappedDate == null)
            {
                return;
            }

#if __IOS__ || __MACCATALYST__
            SetFocus(10);
#endif
            switch (_calendarViewInfo.SelectionMode)
            {
                case CalendarSelectionMode.Single:
                    {
                        if (_calendarViewInfo.CanToggleDaySelection && CalendarViewHelper.IsSameDate(_calendarViewInfo.View, _selectedDate, tappedDate, _calendarViewInfo.Identifier))
                        {
                            tappedDate = null;
                        }

                        _calendarViewInfo.UpdateSelectedDate(tappedDate);
                        break;
                    }

                case CalendarSelectionMode.Multiple:
                case CalendarSelectionMode.Range:
                case CalendarSelectionMode.MultiRange:
                    {
                        _calendarViewInfo.UpdateSelectedDate(tappedDate);
                        break;
                    }
            }

            if (tappedDate != null)
            {
                _calendarViewInfo.TriggerCalendarInteractionEvent(true, e.TapCount, tappedDate.Value, CalendarElement.CalendarCell);
            }
        }

        /// <summary>
        /// Occurs on double tap interaction inside the Year view panel.
        /// </summary>
        /// <param name="e">The tap gesture listener event arguments.</param>
        void IDoubleTapGestureListener.OnDoubleTap(TapEventArgs e)
        {
            DateTime? tappedDate = OnInteractionEvent(e.TapPoint, true, e.TapCount);
            if (tappedDate != null)
            {
                _calendarViewInfo.TriggerCalendarInteractionEvent(true, e.TapCount, tappedDate.Value, CalendarElement.CalendarCell);
            }
        }

        /// <summary>
        /// Occurs on long pressed interaction inside the year view panel.
        /// </summary>
        /// <param name="e">The tap gesture listener event arguments.</param>
        void ILongPressGestureListener.OnLongPress(LongPressEventArgs e)
        {
            DateTime? tappedDate = OnInteractionEvent(e.TouchPoint, false);
            if (tappedDate != null)
            {
                _calendarViewInfo.TriggerCalendarInteractionEvent(false, 1, tappedDate.Value, CalendarElement.CalendarCell);
            }
        }

        /// <summary>
        /// Method invokes on touch interaction.
        /// </summary>
        /// <param name="e">The touch event arguments.</param>
        void ITouchListener.OnTouch(PointerEventArgs e)
        {
#if __MACCATALYST__ || (!__ANDROID__ && !__IOS__)
            // If the height and width is greater than or equal to the touch point it update the hover date as null
            // Example : width = 10 : Cell width is considered as 0 to 9.99 : Width value 10 is considered as the next cell starting width value
            // The above example is same for height and Touch point of X and Y should be greater than zero
            if (e.TouchPoint.X < 0 || e.TouchPoint.Y < 0 || e.TouchPoint.X >= Width || e.TouchPoint.Y >= Height)
            {
                _hoverView.UpdateHoverPosition(null);
            }
            else if (e.Action == PointerActions.Moved || e.Action == PointerActions.Entered)
            {
                _hoverView.UpdateHoverPosition(e.TouchPoint);
            }
            else if (e.Action == PointerActions.Cancelled || e.Action == PointerActions.Exited)
            {
                _hoverView.UpdateHoverPosition(null);
            }
#endif
        }

        /// <summary>
        /// Process keyboard interaction.
        /// </summary>
        /// <param name="args">The keyboard event args.</param>
        void IKeyboardListener.OnKeyDown(KeyEventArgs args)
        {
            _calendarViewInfo.ProcessKeyDown(args);
        }

        /// <inheritdoc/>
        void IKeyboardListener.OnKeyUp(KeyEventArgs args)
        {
        }

        /// <summary>
        /// Method to update visible dates on view change.
        /// </summary>
        /// <param name="visibleDatesCollection">The visible dates collection.</param>
        /// <param name="isCurrentView">Defines whether the view is current view or not.</param>
        void ICalendarView.UpdateVisibleDatesChange(List<DateTime> visibleDatesCollection, bool isCurrentView)
        {
            if (_visibleDates == visibleDatesCollection)
            {
                return;
            }

            List<DateTime> previousVisibleDates = _visibleDates;
            _visibleDates = visibleDatesCollection;
#if MACCATALYST || (!ANDROID && !IOS)
            _hoverView.UpdateVisibleDatesChange(_visibleDates);
#endif
            //// In cell template need to draw the selection.
            //// If cell template null then need to draw the visible dates and selection.
            InvalidateDrawable();
            if (_calendarViewInfo.YearView.CellTemplate == null)
            {
                return;
            }

            Globalization.Calendar cultureCalendar = CalendarViewHelper.GetCalendar(_calendarViewInfo.Identifier.ToString());
            DateTime previousViewStartDate = previousVisibleDates[0];
            DateTime currentViewStartDate = _visibleDates[0];
            int previousYearDifference = cultureCalendar.GetYear(previousVisibleDates[1]) - cultureCalendar.GetYear(previousViewStartDate);
            int currentYearDifference = cultureCalendar.GetYear(_visibleDates[1]) - cultureCalendar.GetYear(currentViewStartDate);
            bool isViewChanged = previousYearDifference != currentYearDifference;

            //// If the view is changed, need to update the cell template based on cell count and
            //// For template selector need to remove the previous cells and generate the cells.
            if (isViewChanged)
            {
                if (_calendarViewInfo.YearView.CellTemplate is DataTemplateSelector)
                {
                    RemoveYearCellsHandler();
                    _yearCells = new List<View>();
                    GenerateYearCells(isCurrentView);
                    InvalidateViewMeasure();
                }
                else
                {
                    UpdateYearCellCount(_calendarViewInfo.YearView.CellTemplate, currentViewStartDate);
                }
            }

            //// If previous and current visible dates count are not equal we have to remove all the cells and generate the year cells again,
            //// else we just update the views like just adding and removing the needed cells.
            else if (previousVisibleDates.Count != _visibleDates.Count && _calendarViewInfo.YearView.CellTemplate is DataTemplateSelector)
            {
                RemoveYearCellsHandler();
                _yearCells = new List<View>();
                GenerateYearCells(isCurrentView);
                InvalidateViewMeasure();
            }
            else
            {
                //// Need to measure the children while the visible dates count changes(visible dates count will change only the reaching 9999 year in decade and century view).
                //// Need to measure the children while the calendar view changes.
                //// Need to update the children visibility and measure while the view reaches min date(0001) year or update from (0001)
                //// because 0001 decade view shows only 9 decade cells when leading and trailing dates hidden.
                bool isNeedMeasure = previousVisibleDates.Count != _visibleDates.Count ||
                    isViewChanged ||
                    (!_calendarViewInfo.ShowTrailingAndLeadingDates &&
                    _calendarViewInfo.View == CalendarView.Decade &&
                    (cultureCalendar.GetYear(previousViewStartDate) == 1 || cultureCalendar.GetYear(currentViewStartDate) == 1));
                if (_yearCells != null && _yearCells.Count != 0)
                {
                    UpdateYearCellTemplateViews(isNeedMeasure);
                }

                //// While the visible date is changed for the current view, need to measure the children in both Template and template selector
                //// Need to measure the children while the display date changed dynamically.
                if (isCurrentView)
                {
                    InvalidateViewMeasure();
                }
            }
        }

        /// <summary>
        /// Method to update selection.
        /// </summary>
        void ICalendarView.UpdateSelectionValue()
        {
            DateTime? previousSelectedDate = _selectedDate;
            _selectedDate = _calendarViewInfo.SelectedDate;
            if (_calendarViewInfo.SelectionMode != CalendarSelectionMode.Single || CalendarViewHelper.IsSameDate(_calendarViewInfo.View, previousSelectedDate, _selectedDate, _calendarViewInfo.Identifier))
            {
                return;
            }

            DateTime? currentDate = CalendarViewHelper.GetStartDate(_selectedDate, _calendarViewInfo.View, _calendarViewInfo.Identifier);
            DateTime? previousDate = CalendarViewHelper.GetStartDate(previousSelectedDate, _calendarViewInfo.View, _calendarViewInfo.Identifier);
            DateTime startDate = _visibleDates[0];
            DateTime endDate = _visibleDates[_visibleDates.Count - 1];
            if (CalendarViewHelper.IsDateWithinDateRange(currentDate, startDate, endDate) || CalendarViewHelper.IsDateWithinDateRange(previousDate, startDate, endDate))
            {
                InvalidateDrawable();
            }
        }

        /// <summary>
        /// Method to update selected dates.
        /// </summary>
        void ICalendarView.UpdateSelectedDates()
        {
            if (_selectedDates == _calendarViewInfo.SelectedDates || _selectedDates.SequenceEqual(_calendarViewInfo.SelectedDates))
            {
                return;
            }

            _selectedDates = new ObservableCollection<DateTime>(_calendarViewInfo.SelectedDates);
            if (_calendarViewInfo.SelectionMode != CalendarSelectionMode.Multiple)
            {
                return;
            }

            InvalidateDrawable();
        }

        /// <summary>
        /// Method to update the selected dates based on the action.
        /// </summary>
        /// <param name="e">The collection changed event arguments.</param>
        void ICalendarView.UpdateSelectedDatesOnAction(NotifyCollectionChangedEventArgs e)
        {
            ObservableCollection<DateTime> oldSelectedDates = new ObservableCollection<DateTime>(_selectedDates);
            _selectedDates = new ObservableCollection<DateTime>(_calendarViewInfo.SelectedDates);

            if (_calendarViewInfo.SelectionMode != CalendarSelectionMode.Multiple)
            {
                return;
            }

            DateTime firstDate = _visibleDates[0];
            DateTime lastDate = GetVisibleEndDate();
            DateTime? newItem = null;
            DateTime? oldItem = null;

            if (e.NewItems != null && e.NewItems.Count > 0)
            {
                newItem = (DateTime?)e.NewItems[0];
            }

            if (e.OldItems != null && e.OldItems.Count > 0)
            {
                oldItem = (DateTime?)e.OldItems[0];
            }

            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                //// Condition to check whether the added date is already on the collection to restrict the draw.
                if (newItem == null || (oldSelectedDates.Count > 0 && CalendarViewHelper.IsDateInDateCollection(newItem.Value, oldSelectedDates)))
                {
                    return;
                }

                //// Condition to check whether the selected date is within the visible dates.
                if (CalendarViewHelper.IsDateWithinDateRange(newItem, firstDate, lastDate))
                {
                    InvalidateDrawable();
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                //// Condition to check whether the removed date is within the visible dates.
                if (CalendarViewHelper.IsDateWithinDateRange(oldItem, firstDate, lastDate))
                {
                    InvalidateDrawable();
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Replace)
            {
                //// Condition to check whether the added or removed date is within the visible dates.
                if (CalendarViewHelper.IsDateWithinDateRange(newItem, firstDate, lastDate) || CalendarViewHelper.IsDateWithinDateRange(oldItem, firstDate, lastDate))
                {
                    InvalidateDrawable();
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                if (oldSelectedDates.Count > 0)
                {
                    InvalidateDrawable();
                }
            }
        }

        /// <summary>
        /// Method to update when special and disabled dates on view changed.
        /// </summary>
        /// <param name="disabledDates">The disabled dates for this view.</param>
        /// <param name="specialDatesDetails">The special dates for this view.</param>
        /// <param name="visibleDates">The visible dates collection.</param>
        void ICalendarView.UpdateDisableAndSpecialDateChange(List<DateTime>? disabledDates, List<CalendarIconDetails>? specialDatesDetails, List<DateTime> visibleDates)
        {
            if (_visibleDates != visibleDates)
            {
                return;
            }

#if MACCATALYST || (!ANDROID && !IOS)
            //// Here we are updating the hover position before checking the previous and current disabled dates are equal.
            //// Because the previous and current view disabled dates may be same and we can't update the hover position on swiping to next or previous view.
            //// On view navigation, the mouse hover position will remain same but visible dates will be changed.
            //// So we are removing hovering when updating disabled date for each view.
            //// If we not update the hover position, the hovering view will be remain even after view changed.
            _hoverView.UpdateHoverPosition(null);
#endif

            //// Disabled dates is null then the method called only for update special dates.
            //// but year view does not have special dates.
            if (disabledDates == null)
            {
                return;
            }

            //// Condition to check whether the previous disabled dates and current disabled dates are equal to prevent the triggering of invalidate draw.
            if (_disabledDates == disabledDates || _disabledDates.SequenceEqual(disabledDates))
            {
                return;
            }

            _disabledDates = disabledDates;
#if MACCATALYST || (!ANDROID && !IOS)
            _hoverView.UpdateDisabledDates(disabledDates);
#endif
            InvalidateDrawable();
        }

        /// <summary>
        /// Method to invalidate view cells and hover view.
        /// </summary>
        void ICalendarView.InvalidateView()
        {
            InvalidateDrawable();
#if MACCATALYST || (!ANDROID && !IOS)
            _hoverView.InvalidateDrawable();
#endif
        }

        /// <summary>
        /// Method to invalidate view cells.
        /// </summary>
        void ICalendarView.InvalidateViewCells()
        {
            InvalidateDrawable();
        }

        /// <summary>
        /// Method to update template views.
        /// </summary>
        /// <param name="isCurrentView">Defines the view is current visible view.</param>
        void ICalendarView.UpdateTemplateViews(bool isCurrentView)
        {
            if (_calendarViewInfo.YearView.CellTemplate == null)
            {
                DrawingOrder = DrawingOrder.BelowContent;
                //// If the previous year view is not CellTemplate then we just need to update the view.
                if (_yearCells == null)
                {
                    InvalidateDrawable();
                    return;
                }

                //// If the previous year view is CellTemplate then we need to remove the year cells from the view and update the view.
                RemoveYearCellsHandler();
                InvalidateDrawable();
            }
            else
            {
                DrawingOrder = DrawingOrder.AboveContent;
                RemoveYearCellsHandler();
                _yearCells = new List<View>();
                GenerateYearCells(isCurrentView);
            }

            InvalidateViewMeasure();
        }

        /// <summary>
        /// Method to update range selection value
        /// </summary>
        void ICalendarView.UpdateSelectedRangeValue()
        {
            DateTime? previousStartDate = CalendarViewHelper.GetStartDate(_selectedDateRange?.StartDate, _calendarViewInfo.View, _calendarViewInfo.Identifier);
            DateTime? previousEndDate = CalendarViewHelper.GetStartDate(_selectedDateRange?.EndDate, _calendarViewInfo.View, _calendarViewInfo.Identifier);
            bool isSameSelectedRange = CalendarViewHelper.IsSameRange(_calendarViewInfo.View, _selectedDateRange, _calendarViewInfo.SelectedDateRange, _calendarViewInfo.Identifier);
            _selectedDateRange = GetValidSelectedRange(_calendarViewInfo.SelectedDateRange);
            if (isSameSelectedRange)
            {
                return;
            }

#if MACCATALYST || (!ANDROID && !IOS)
            _hoverView.UpdateSelectedDateRange(_selectedDateRange);
#endif
            if (_calendarViewInfo.AllowViewNavigation || _calendarViewInfo.SelectionMode != CalendarSelectionMode.Range)
            {
                return;
            }

            bool isPreviousRangeInVisibleDates = IsRangeInVisibleDates(previousStartDate, previousEndDate);
            bool isCurrentRangeInVisibleDates = IsRangeInVisibleDates(_selectedDateRange?.StartDate, _selectedDateRange?.EndDate);
            bool isViewNeedToUpdate = IsViewNeedUpdateRangeSelection(previousStartDate, previousEndDate, _selectedDateRange);

            // Check the view is needed to redraw based on previous and current range
            if (isPreviousRangeInVisibleDates || isCurrentRangeInVisibleDates || isViewNeedToUpdate)
            {
                InvalidateDrawable();
            }
        }

        /// <summary>
        /// Method to update selected date ranges value.
        /// </summary>
        void ICalendarView.UpdateSelectedMultiRangesValue()
        {
            ObservableCollection<CalendarDateRange>? oldSelectedRanges = _selectedDateRanges;
            UpdateSelectedDateRangesValue(_calendarViewInfo.SelectedDateRanges);
            if (CalendarViewHelper.IsSameDateRanges(_calendarViewInfo.View, oldSelectedRanges, _calendarViewInfo.SelectedDateRanges, _calendarViewInfo.Identifier))
            {
                return;
            }

            //// Does not need to update the UI while the selection mode is other than the multi range selection.
            if (_calendarViewInfo.SelectionMode != CalendarSelectionMode.MultiRange)
            {
                return;
            }

            // To update the new ranges that are present in the current view.
            if (IsCurrentViewRangesUpdated(oldSelectedRanges))
            {
                InvalidateDrawable();
            }
        }

        /// <summary>
        /// Method to update the range selection when swiping is performed
        /// </summary>
        /// <param name="status">The pan gesture status.</param>
        /// <param name="point">The pan gesture touch point.</param>
        void ICalendarView.HandleSwipeRangeSelection(GestureStatus status, Point point)
        {
            DateTime? tappedDate = CalendarViewHelper.GetYearDateFromPosition(point, Width, Height, _calendarViewInfo, _visibleDates);
            if (tappedDate != null && !CalendarViewHelper.IsInteractableDate(tappedDate.Value, _disabledDates, _visibleDates, _calendarViewInfo, 0))
            {
                tappedDate = null;
            }

            if (_calendarViewInfo.SelectionMode == CalendarSelectionMode.MultiRange)
            {
                UpdateRangeSelectionOnSwipe(status, tappedDate, _selectedDateRanges != null && _selectedDateRanges.Count > 0 ? _selectedDateRanges[_selectedDateRanges.Count - 1] : null);
                return;
            }

            //// It is perform for range selection mode when swiping
            //// It works when AllowViewNavigation is false and EnableSwipeSelection is true
            switch (_calendarViewInfo.RangeSelectionDirection)
            {
                case CalendarRangeSelectionDirection.Default:
                    UpdateRangeSelectionOnSwipe(status, tappedDate, _selectedDateRange);
                    break;

                case CalendarRangeSelectionDirection.Forward:
                case CalendarRangeSelectionDirection.Backward:
                case CalendarRangeSelectionDirection.Both:
                case CalendarRangeSelectionDirection.None:
                    _calendarViewInfo.UpdateSelectedDate(tappedDate);
                    break;
            }
        }

        /// <summary>
        /// Invokes on keyboard key press event.
        /// </summary>
        /// <param name="args">The keyboard event args.</param>
        /// <param name="selectedDate">Previous selected date time. In after swipe scenario it holds the current view selected date.</param>
        /// <param name="isAfterSwipe">Key down triggers after view swipe.</param>
        void ICalendarView.OnKeyDown(KeyEventArgs args, DateTime selectedDate, bool isAfterSwipe)
        {
            if (!isAfterSwipe)
            {
                UpdateSelectionWhileKeyNavigation(args, selectedDate);
            }
            else
            {
                CalendarViewHelper.ValidateDateOnKeyNavigation(args, selectedDate, selectedDate, _calendarViewInfo, _visibleDates, _disabledDates);
            }
        }

        /// <summary>
        /// Method to update the private variable for the keyboard interaction.
        /// </summary>
        /// <param name="date">The previous selected date.</param>
        void ICalendarView.UpdatePreviousSelectedDateOnRangeSelection(DateTime date)
        {
            _previousSelectedDateRange = date;
        }

        /// <summary>
        /// Method to update the focus while view is changed on keyboard interaction.
        /// </summary>
        void ICalendarView.SetFocusOnViewChanged()
        {
            Focus();
        }

        #endregion
    }
}