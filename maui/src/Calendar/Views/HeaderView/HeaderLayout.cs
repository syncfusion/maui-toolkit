using System.Globalization;
using Syncfusion.Maui.Toolkit.Internals;
using Syncfusion.Maui.Toolkit.Localization;
using Syncfusion.Maui.Toolkit.Graphics.Internals;
using PointerEventArgs = Syncfusion.Maui.Toolkit.Internals.PointerEventArgs;
using Globalization = System.Globalization;
using ITextElement = Syncfusion.Maui.Toolkit.Graphics.Internals.ITextElement;

namespace Syncfusion.Maui.Toolkit.Calendar
{
    /// <summary>
    /// Represents a class which contains information of calendar header layout.
    /// </summary>
#if __MACCATALYST__ || (!__ANDROID__ && !__IOS__)
    internal class HeaderLayout : CalendarLayout, ITapGestureListener, IDoubleTapGestureListener, ILongPressGestureListener, ITouchListener
#else
    internal class HeaderLayout : CalendarLayout, ITapGestureListener, IDoubleTapGestureListener, ILongPressGestureListener
#endif
    {
        #region Fields

        /// <summary>
        /// The header view info.
        /// </summary>
        readonly IHeader _headerInfo;

        /// <summary>
        /// The backward arrow icon.
        /// </summary>
        SfIconButton? _backwardArrow;

        /// <summary>
        /// The forward arrow icon.
        /// </summary>
        SfIconButton? _forwardArrow;

        /// <summary>
        /// The backward arrow view.
        /// </summary>
        SfIconView? _backwardArrowView;

        /// <summary>
        /// The forward arrow view.
        /// </summary>
        SfIconView? _forwardArrowView;

        /// <summary>
        /// The header text label.
        /// </summary>
        Label? _headerTextLabel;

        /// <summary>
        /// The navigation arrow width.
        /// </summary>
        readonly double _iconWidth = 36;

        /// <summary>
        /// Padding between the left and right side of the arrows.
        /// </summary>
        readonly double _iconPadding = 3;

#if ANDROID || IOS
        /// <summary>
        /// To skip the un needed measure and arrange in initial rendering when the template selector is not null.
        /// </summary>
        bool _isTemplateAddedDynamically;
#endif

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="HeaderLayout"/> class.
        /// </summary>
        /// <param name="headerInfo">The header view info.</param>
        internal HeaderLayout(IHeader headerInfo)
        {
            this.AddGestureListener(this);
#if __MACCATALYST__ || (!__ANDROID__ && !__IOS__)
            this.AddTouchListener(this);
#endif
#if ANDROID || IOS
            _isTemplateAddedDynamically = false;
#endif
            _headerInfo = headerInfo;
            if (_headerInfo.HeaderTemplate == null)
            {
                Background = _headerInfo.HeaderView.Background;
                _headerTextLabel = new Label()
                {
                    Margin = new Thickness(0),
                    TextColor = headerInfo.HeaderView.TextStyle.TextColor,
                    FontSize = headerInfo.HeaderView.TextStyle.FontSize,
                    FontAttributes = headerInfo.HeaderView.TextStyle.FontAttributes,
                    FontFamily = headerInfo.HeaderView.TextStyle.FontFamily,
                    FontAutoScalingEnabled = headerInfo.HeaderView.TextStyle.FontAutoScalingEnabled,
                    VerticalTextAlignment = TextAlignment.Center,
                    FlowDirection = _headerInfo.IsRTLLayout ? FlowDirection.RightToLeft : FlowDirection.LeftToRight,
#if WINDOWS
                    //// to position the text exact to center position with the given font size and layout height
                    Padding = new Thickness(15, 3, 5, 5),
#elif !__IOS__
                    Padding = new Thickness(15, 0, 5, 5),
#else
                    Padding = new Thickness(15, 0, 5, 0),
#endif
                    Text = GetHeaderText(),
                    MaxLines = 1,
                };

                SemanticProperties.SetDescription(_headerTextLabel, GetHeaderText(true));
                Add(_headerTextLabel);
                AddOrRemoveNavigationArrows();
            }
#if IOS

#if NET10_0
            this.SafeAreaEdges = SafeAreaEdges.None;
#else
            this.IgnoreSafeArea = true;
#endif

#endif
		}

        #endregion

        #region Internal Methods

        /// <summary>
        /// Method to update the header template dynamically.
        /// </summary>
        internal void InvalidateHeaderTemplate()
        {
            if (_headerInfo.HeaderTemplate != null)
            {
                Children.Clear();
                _backwardArrow = null;
                _forwardArrow = null;
                GenerateHeaderViewTemplate();
#if ANDROID || IOS
                InvalidateViewMeasure();
#endif
            }
            else
            {
                Children.Clear();
                Background = _headerInfo.HeaderView.Background;
                _headerTextLabel = new Label()
                {
                    Margin = new Thickness(0),
                    TextColor = _headerInfo.HeaderView.TextStyle.TextColor,
                    FontSize = _headerInfo.HeaderView.TextStyle.FontSize,
                    FontAttributes = _headerInfo.HeaderView.TextStyle.FontAttributes,
                    FontFamily = _headerInfo.HeaderView.TextStyle.FontFamily,
                    FontAutoScalingEnabled = _headerInfo.HeaderView.TextStyle.FontAutoScalingEnabled,
                    VerticalTextAlignment = TextAlignment.Center,
                    FlowDirection = _headerInfo.IsRTLLayout ? FlowDirection.RightToLeft : FlowDirection.LeftToRight,
#if WINDOWS
                    //// to position the text exact to center position with the given font size and layout height
                    Padding = new Thickness(15, 3, 5, 5),
#elif !__IOS__
                    Padding = new Thickness(15, 0, 5, 5),
#else
                    Padding = new Thickness(15, 0, 5, 0),
#endif
                    Text = GetHeaderText(),
                    MaxLines = 1,
                };

                SemanticProperties.SetDescription(_headerTextLabel, GetHeaderText(true));
                Add(_headerTextLabel);
                AddOrRemoveNavigationArrows();
#if ANDROID
                InvalidateViewMeasure();
#endif
            }
        }

        /// <summary>
        /// Method to update the visible dates.
        /// </summary>
        internal void UpdateVisibleDates()
        {
            if (_headerInfo.HeaderTemplate != null)
            {
                UpdateHeaderTemplateView();
#if ANDROID || IOS
                InvalidateViewMeasure();
#endif
            }
            else
            {
                if (_headerTextLabel != null)
                {
                    _headerTextLabel.Text = GetHeaderText();
                }

                SemanticProperties.SetDescription(_headerTextLabel, GetHeaderText(true));
                UpdateNavigationButtonState();
            }
        }

        /// <summary>
        /// Method to update the navigation arrows while the navigation direction changed.
        /// </summary>
        internal void UpdateNavigationArrowsIcon()
        {
            if (_forwardArrowView == null || _backwardArrowView == null)
            {
                return;
            }

            bool isVertical = _headerInfo.NavigationDirection == CalendarNavigationDirection.Vertical;
            _forwardArrowView.Icon = isVertical ? SfIcon.Downward : SfIcon.Forward;
            _backwardArrowView.Icon = isVertical ? SfIcon.Upward : SfIcon.Backward;
            _forwardArrowView?.InvalidateDrawable();
            _backwardArrowView?.InvalidateDrawable();
        }

        /// <summary>
        /// Method to update the header text flow direction.
        /// </summary>
        internal void UpdateHeaderTextFlowDirection()
        {
            if (_headerTextLabel == null)
            {
                return;
            }

            _headerTextLabel.FlowDirection = _headerInfo.IsRTLLayout ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
#if ANDROID
            this.TriggerInvalidateMeasure();
#else
            InvalidateMeasure();
#endif
        }

        /// <summary>
        /// Method to update the header text.
        /// </summary>
        internal void UpdateHeaderText()
        {
            if (_headerTextLabel != null)
            {
                _headerTextLabel.Text = GetHeaderText();
                SemanticProperties.SetDescription(_headerTextLabel, GetHeaderText(true));
            }

#if __ANDROID__
            //// TODO: In Android, header text label does not triggers measure after it text changes,
            //// so it render text with previous width request and it makes text cropping.
            if (Width <= 0 || Height <= 0)
            {
                return;
            }

            this.TriggerInvalidateMeasure();
#endif
        }

        /// <summary>
        /// Create and remove the navigation arrows based on the arrows visibility.
        /// </summary>
        internal void InvalidateNavigationArrowVisibility()
        {
            AddOrRemoveNavigationArrows();
#if __ANDROID__
            this.TriggerInvalidateMeasure();
#else
            InvalidateMeasure();
#endif
        }

        /// <summary>
        /// Method to update the header text style.
        /// </summary>
        internal void UpdateHeaderTextStyle()
        {
            if (_headerTextLabel != null)
            {
                _headerTextLabel.TextColor = _headerInfo.HeaderView.TextStyle.TextColor;
                _headerTextLabel.FontSize = _headerInfo.HeaderView.TextStyle.FontSize;
                _headerTextLabel.FontAttributes = _headerInfo.HeaderView.TextStyle.FontAttributes;
                _headerTextLabel.FontFamily = _headerInfo.HeaderView.TextStyle.FontFamily;
                _headerTextLabel.FontAutoScalingEnabled = _headerInfo.HeaderView.TextStyle.FontAutoScalingEnabled;
            }

            _forwardArrowView?.UpdateStyle(_headerInfo.HeaderView.TextStyle);
            _backwardArrowView?.UpdateStyle(_headerInfo.HeaderView.TextStyle);
            UpdateNavigationButtonState();
        }

        /// <summary>
        /// Methods to update navigation button state in header.
        /// </summary>
        internal void UpdateNavigationButtonState()
        {
            if (_forwardArrowView == null || _forwardArrow == null || _backwardArrowView == null || _backwardArrow == null)
            {
                return;
            }

            bool isBackwardEnabled, isForwardEnabled;
            Color arrowColor = _headerInfo.HeaderView.TextStyle.TextColor;
            Color disabledNavigationArrowColor = _headerInfo.HeaderView.DisabledNavigationArrowColor ?? arrowColor.WithAlpha(0.4f);
            int numberOfWeeks = _headerInfo.NumberOfVisibleWeeks;
            if (_headerInfo.IsRTLLayout && _headerInfo.NavigationDirection == CalendarNavigationDirection.Horizontal)
            {
                isBackwardEnabled = CalendarViewHelper.IsValidNextDatesNavigation(_headerInfo.VisibleDates, _headerInfo.View, numberOfWeeks, _headerInfo.MaximumDate, _headerInfo.Identifier);
                isForwardEnabled = CalendarViewHelper.IsValidPreviousDatesNavigation(_headerInfo.VisibleDates, _headerInfo.View, numberOfWeeks, _headerInfo.MinimumDate, _headerInfo.Identifier);
            }
            else
            {
                isForwardEnabled = CalendarViewHelper.IsValidNextDatesNavigation(_headerInfo.VisibleDates, _headerInfo.View, numberOfWeeks, _headerInfo.MaximumDate, _headerInfo.Identifier);
                isBackwardEnabled = CalendarViewHelper.IsValidPreviousDatesNavigation(_headerInfo.VisibleDates, _headerInfo.View, numberOfWeeks, _headerInfo.MinimumDate, _headerInfo.Identifier);
            }

            _forwardArrow.ShowTouchEffect = isForwardEnabled;
            _backwardArrow.ShowTouchEffect = isBackwardEnabled;
            _forwardArrowView.UpdateSemantics(isTouchEnabled: isForwardEnabled);
            _backwardArrowView.UpdateSemantics(isTouchEnabled: isBackwardEnabled);
            _forwardArrowView.UpdateIconColor(isForwardEnabled ? arrowColor : disabledNavigationArrowColor);
            _backwardArrowView.UpdateIconColor(isBackwardEnabled ? arrowColor : disabledNavigationArrowColor);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Method to Initialize the navigation arrows.
        /// </summary>
        void AddOrRemoveNavigationArrows()
        {
            SemanticsNode backwardNode = new SemanticsNode()
            {
                Id = 0,
                Text = SfCalendarResources.GetLocalizedString("Backward"),
                IsTouchEnabled = true,
                OnClick = OnBackwardSemanticsNodeClicked,
            };
            SemanticsNode forwardNode = new SemanticsNode()
            {
                Id = 0,
                Text = SfCalendarResources.GetLocalizedString("Forward"),
                IsTouchEnabled = true,
                OnClick = OnForwardSemanticsNodeClicked,
            };
            //// Its true when the navigation direction is vertical otherwise its false.
            bool isVertical = _headerInfo.NavigationDirection == CalendarNavigationDirection.Vertical;
            if (_headerInfo.HeaderView.ShowNavigationArrows && _backwardArrow == null && _forwardArrow == null)
            {
                _backwardArrowView = new SfIconView(isVertical ? SfIcon.Upward : SfIcon.Backward, _headerInfo.HeaderView.TextStyle, string.Empty, Colors.Transparent, Colors.Transparent, semanticsNode: backwardNode);
                //// Need to update the isSquareSelection property to false for the navigation arrows to draw the circle effect.
                _backwardArrow = new SfIconButton(_backwardArrowView, showTouchEffect: true, isSquareSelection: false);
                _forwardArrowView = new SfIconView(isVertical ? SfIcon.Downward : SfIcon.Forward, _headerInfo.HeaderView.TextStyle, string.Empty, Colors.Transparent, Colors.Transparent, semanticsNode: forwardNode);
                //// Need to update the isSquareSelection property to false for the navigation arrows to draw the circle effect.
                _forwardArrow = new SfIconButton(_forwardArrowView, showTouchEffect: true, isSquareSelection: false);
                Add(_backwardArrow);
                Add(_forwardArrow);
                _backwardArrow.Clicked = OnBackwardClick;
                _forwardArrow.Clicked = OnForwardClick;
            }
            else if (!_headerInfo.HeaderView.ShowNavigationArrows && _backwardArrow != null && _forwardArrow != null)
            {
                for (int i = _backwardArrow.Children.Count - 1; i >= 0; i--)
                {
                    _backwardArrow.RemoveAt(i);
                }

                if (_backwardArrow.Handler != null && _backwardArrow.Handler.PlatformView != null)
                {
                    _backwardArrow.Handler.DisconnectHandler();
                }

                _backwardArrow = null;
                if (_backwardArrowView != null)
                {
                    if (_backwardArrowView.Handler != null && _backwardArrowView.Handler.PlatformView != null)
                    {
                        _backwardArrowView.Handler.DisconnectHandler();
                    }

                    _backwardArrowView = null;
                }

                for (int i = _forwardArrow.Children.Count - 1; i >= 0; i--)
                {
                    _forwardArrow.RemoveAt(i);
                }

                if (_forwardArrow.Handler != null && _forwardArrow.Handler.PlatformView != null)
                {
                    _forwardArrow.Handler.DisconnectHandler();
                }

                _forwardArrow = null;
                if (_forwardArrowView != null)
                {
                    if (_forwardArrowView.Handler != null && _forwardArrowView.Handler.PlatformView != null)
                    {
                        _forwardArrowView.Handler.DisconnectHandler();
                    }

                    _forwardArrowView = null;
                }
            }
        }

        /// <summary>
        /// Method to get the header text.
        /// </summary>
        /// <param name="isAccessibility">Accessibility is enabled or not.</param>
        /// <returns>The header text.</returns>
        string GetHeaderText(bool isAccessibility = false)
        {
            List<DateTime> visibleDates = _headerInfo.VisibleDates;
            DateTime startDate = visibleDates[0];
            DateTime endDate = visibleDates[^1];
            string headerTextFormat = _headerInfo.HeaderView.TextFormat;
            bool isEmptyFormat = string.IsNullOrEmpty(headerTextFormat);
            string yearFormat = isEmptyFormat ? "yyyy" : headerTextFormat;
            string headerDateString = string.Empty;
            int numberOfWeeks = _headerInfo.NumberOfVisibleWeeks;
            CultureInfo cultureInfo = CalendarViewHelper.GetCurrentUICultureInfo(_headerInfo.Identifier);
            Globalization.Calendar calendar = CalendarViewHelper.GetCalendar(_headerInfo.Identifier.ToString());
            switch (_headerInfo.View)
            {
                case CalendarView.Month:
                    {
                        string format = isEmptyFormat ? "MMMM yyyy" : headerTextFormat;
                        //// Get middle of visible date while number of week in view is equal to 6.
                        if (numberOfWeeks == 6)
                        {
                            startDate = visibleDates[visibleDates.Count / 2];
                            if (isAccessibility)
                            {
                                return startDate.ToString("MMMM yyyy");
                            }

                            headerDateString = startDate.ToString(format, cultureInfo);
                            return headerDateString;
                        }
                        else if (numberOfWeeks != 6 && calendar.GetMonth(startDate) == calendar.GetMonth(endDate))
                        {
                            if (isAccessibility)
                            {
                                return startDate.ToString("MMMM yyyy");
                            }

                            headerDateString = startDate.ToString(format, cultureInfo);
                            return headerDateString;
                        }

                        format = isEmptyFormat ? "MMM" : headerTextFormat;
                        string endDateFormat = isEmptyFormat ? "MMM yyyy" : headerTextFormat;
                        if (isAccessibility)
                        {
                            return startDate.ToString("MMMM", cultureInfo) + SfCalendarResources.GetLocalizedString("To") + endDate.ToString("MMMM yyyy", cultureInfo);
                        }

                        headerDateString = startDate.ToString(format, cultureInfo) + " - " + endDate.ToString(endDateFormat, cultureInfo);
                        return headerDateString;
                    }

                case CalendarView.Year:
                    {
                        DateTime year = visibleDates[0];
                        if (isAccessibility)
                        {
                            return year.ToString("yyyy");
                        }

                        headerDateString = year.ToString(yearFormat, cultureInfo);
                        return headerDateString;
                    }

                case CalendarView.Decade:
                case CalendarView.Century:
                    {
                        int offset = _headerInfo.View.GetOffset();

                        // Example for Decade View: Assume current year is 2022.
                        // The offset value is 10 while the view is decade view.
                        // Calculation: 2022 / 10 * 10 => 202 * 10 => 2020
                        // Therefore, 2020 is the start year for the decade view's header date.
                        int year = calendar.GetYear(visibleDates[0].Date);
                        int month = 1;
                        int day = 1;
                        int minYear = calendar.GetYear(calendar.MinSupportedDateTime.Date);
                        int minMonth = calendar.GetMonth(calendar.MinSupportedDateTime.Date);
                        int minDay = calendar.GetDayOfMonth(calendar.MinSupportedDateTime.Date);
                        int maxyear = calendar.GetYear(calendar.MaxSupportedDateTime.Date);

                        // year returns 0 when the visible date year is 0001. So, need to start from the year 0001
                        // End date must be within the range(0001 - 0099). So, Offset value is decremented by 1
                        if (year == 0 || year <= minYear)
                        {
                            year = minYear;
                            month = minMonth;
                            day = minDay;
                            //// Minimum supported year is 1993, so the offset value must be 7 in decade view. The range becomes 1993 - 1999.
                            //// In Century view, minimum supported year is 1993, so the offset value must be 7 in century view. The range becomes 1993 - 1999.
                            offset = offset - (minYear % offset);
                        }

                        DateTime startYear = new DateTime(year, month, day, calendar);

                        // Calculation for end year: DateTime(2020 + 10 - 1, 1, 1) => DateTime(2029, 1, 1)
                        // Therefore, 2029 is the end year for the decade view's header date. So the decade views header dates range from 2020 to 2029.
                        DateTime endYear = (year + offset - 1) > maxyear ? new DateTime(maxyear, 1, 1, calendar) : new DateTime(year + offset - 1, 1, 1, calendar);
                        if (isAccessibility)
                        {
                            return startYear.ToString("yyyy", cultureInfo) + SfCalendarResources.GetLocalizedString("To") + endYear.ToString("yyyy", cultureInfo);
                        }

                        // If the year is 0001 need to draw within the range of 0009.
                        headerDateString = startYear.ToString(yearFormat, cultureInfo) + " - " + endYear.ToString(yearFormat, cultureInfo);
                        return headerDateString;
                    }
            }

            return headerDateString;
        }

        /// <summary>
        /// Method to get the vertical padding space.
        /// </summary>
        /// <returns>The padding values.</returns>
        double GetIconVerticalPadding()
        {
            //// value 10 is the padding for both top 5 and bottom 5 padding value.
            double height = _headerInfo.HeaderView.TextStyle.FontSize + 10;
            height = _headerInfo.HeaderView.Height > height ? height : _headerInfo.HeaderView.Height;
            return _headerInfo.HeaderView.Height - height;
        }

        /// <summary>
        /// Occurs when backward icon button is clicked.
        /// </summary>
        /// <param name="text">Icon text value.</param>
        void OnBackwardClick(string text)
        {
            _headerInfo.AnimateMoveToPreviousView();
#if __ANDROID__
            this.TriggerInvalidateMeasure();
#endif
        }

        /// <summary>
        /// Occurs when forward icon button is clicked.
        /// </summary>
        /// <param name="text">Icon text value.</param>
        void OnForwardClick(string text)
        {
            _headerInfo.AnimateMoveToNextView();
#if __ANDROID__
            this.TriggerInvalidateMeasure();
#endif
        }

        /// <summary>
        /// Occurs on tap interaction inside time line view layout.
        /// </summary>
        /// <param name="tapPoint">The interaction point.</param>
        /// <param name="isTapped">Is tapped.</param>
        /// <param name="tapCount">The tap count.</param>
        void OnInteractionEvent(Point tapPoint, bool isTapped, int tapCount = 1)
        {
            bool isRTL = _headerInfo.IsRTLLayout;

            // Example: The icon width = 35. So the icon(forward and backward) with multiply by 2 then we can get two icon width = 70.
            // If the show navigation arrow true then need consider the arrow width.
            // arrow width have forward and backward icon width with icon left, right and inbetween padding.
            float arrowWidth = _headerInfo.HeaderView.ShowNavigationArrows ? (float)((_iconWidth * 2) + (3 * _iconPadding)) : 0;

            // Example: The header with is 200.
            // If tap or double tapped or long pressed actions are preformed on iconButton(backward and froward arrows)then should be restrict the Event and ICommand.
            // Calculation:The actual header width = 200(header height) - 70(iconWidth) the actual header with is 130.
            float headerWidth = (float)(Width - arrowWidth);

            // To restrict arrow tapped and double tapped and long pressed event and command. Assume the tap point is 150.
            // Calculation: XPosition(The actual tapped XPosition) = true ? 130(header height) - 150(tapedXPosition) = -20(xPosition). So tapped or double tapped or long pressed are not triggered.
            float xPosition = (float)(isRTL ? tapPoint.X - arrowWidth : headerWidth - tapPoint.X);
            if (xPosition < 0)
            {
                return;
            }

            DateTime tappedDate = _headerInfo.VisibleDates[0];
            int numberOfWeeks = _headerInfo.NumberOfVisibleWeeks;
            if (_headerInfo.View == CalendarView.Month && numberOfWeeks == 6)
            {
                //// Set the tapped value as month start date(1) for calendar month view to restrict
                //// the assignment of trailing dates value.
                tappedDate = _headerInfo.VisibleDates[_headerInfo.VisibleDates.Count / 2];
                Globalization.Calendar cultureCalendar = CalendarViewHelper.GetCalendar(_headerInfo.Identifier.ToString());
                DateTime minDate = cultureCalendar.MinSupportedDateTime;
                int minDateYear = cultureCalendar.GetYear(minDate);
                int minDateMonth = cultureCalendar.GetMonth(minDate);
                int tappedDateYear = cultureCalendar.GetYear(tappedDate);
                int tappedDateMonth = cultureCalendar.GetMonth(tappedDate);
                tappedDate = minDateYear == tappedDateYear && minDateMonth == tappedDateMonth ? minDate : new DateTime(tappedDateYear, tappedDateMonth, 1, cultureCalendar);
            }

            _headerInfo.TriggerCalendarInteractionEvent(isTapped, tapCount, tappedDate, CalendarElement.Header);
        }

        /// <summary>
        /// Occurs when backward arrow tapped while accessibility enabled.
        /// </summary>
        /// <param name="node">Backward arrow semantic node.</param>
        void OnBackwardSemanticsNodeClicked(SemanticsNode node)
        {
            _headerInfo.AnimateMoveToPreviousView();
#if __ANDROID__
            this.TriggerInvalidateMeasure();
#endif
        }

        /// <summary>
        /// Occurs when forward arrow tapped while accessibility enabled.
        /// </summary>
        /// <param name="node">Backward arrow semantic node.</param>
        void OnForwardSemanticsNodeClicked(SemanticsNode node)
        {
            _headerInfo.AnimateMoveToNextView();
#if __ANDROID__
            this.TriggerInvalidateMeasure();
#endif
        }

        /// <summary>
        /// Method to generate the header views template.
        /// </summary>
        void GenerateHeaderViewTemplate()
        {
            var headerTemplate = _headerInfo.HeaderTemplate;
            CalendarHeaderDetails headerDetails = GetHeaderViewInfo();
            if (headerTemplate is DataTemplateSelector)
            {
                var templateSelector = (DataTemplateSelector)headerTemplate;
                if (templateSelector != null)
                {
                    headerTemplate = templateSelector.SelectTemplate(headerDetails, _headerInfo.HeaderView);
                }
            }

            var headerTemplateView = CalendarViewHelper.CreateTemplateView(headerTemplate, headerDetails);
            Add(headerTemplateView);
        }

        /// <summary>
        /// Method to update the header view templates.
        /// </summary>
        void UpdateHeaderTemplateView()
        {
            if (Children.Count == 0 || _headerInfo.VisibleDates.Count == 0)
            {
                return;
            }

            if (_headerInfo.HeaderTemplate is DataTemplateSelector)
            {
                Children.Clear();
                GenerateHeaderViewTemplate();
            }
            else
            {
                //// When data template added, we have added only one child to this layout , then directly used Children[0] to change the binding context of the templated view.
                ((View)Children[0]).BindingContext = GetHeaderViewInfo();
            }
        }

        /// <summary>
        /// Method to get the header view data template details.
        /// </summary>
        /// <returns>Returns the header template details.</returns>
        CalendarHeaderDetails GetHeaderViewInfo()
        {
            List<DateTime> visibleDates = _headerInfo.VisibleDates;
            DateTime dateTime = CalendarViewHelper.GetWeekStartDate(visibleDates, _headerInfo.Identifier, _headerInfo.FirstDayOfWeek);
            int weekNumber = CalendarViewHelper.GetWeekNumber(_headerInfo.Identifier, dateTime, _headerInfo.FirstDayOfWeek);
            CalendarHeaderDetails? headerViewInfo = new CalendarHeaderDetails();
            if (_headerInfo.View == CalendarView.Month)
            {
                List<DateTime> currentMonthDates = CalendarViewHelper.GetCurrentMonthDates(visibleDates);
                DateTime? startDate = CalendarViewHelper.GetStartDate(currentMonthDates[0].Date, _headerInfo.View, _headerInfo.Identifier);
                headerViewInfo.StartDateRange = startDate.HasValue ? startDate.Value : currentMonthDates[0];

                DateTime? endDate = CalendarViewHelper.GetLastDate(_headerInfo.View, currentMonthDates[currentMonthDates.Count - 1].Date, _headerInfo.Identifier);
                headerViewInfo.EndDateRange = endDate.HasValue ? endDate.Value : currentMonthDates[currentMonthDates.Count - 1];
            }
            else
            {
                DateTime? startDate = CalendarViewHelper.GetStartDate(visibleDates[0].Date, _headerInfo.View, _headerInfo.Identifier);
                headerViewInfo.StartDateRange = startDate.HasValue ? startDate.Value : visibleDates[0];

                DateTime? endDate = CalendarViewHelper.GetLastDate(_headerInfo.View, visibleDates[visibleDates.Count - 1].Date, _headerInfo.Identifier);
                headerViewInfo.EndDateRange = endDate.HasValue ? endDate.Value : visibleDates[visibleDates.Count - 1];
            }

            headerViewInfo.Text = GetHeaderText();
            headerViewInfo.View = _headerInfo.View;
            return headerViewInfo;
        }

#if ANDROID || IOS
        /// <summary>
        /// Method to invalidate view measures.
        /// </summary>
        void InvalidateViewMeasure()
        {
            //// In android and ios, to skip the un needed measure and arrange in initial rendering when the template selector is not null.
            if (!_isTemplateAddedDynamically)
            {
                _isTemplateAddedDynamically = true;
                return;
            }

            LayoutMeasure(Width, Height);
            LayoutArrangeChildren(new Rect(0, 0, Width, Height));
        }
#endif

        #endregion

        #region Override Methods

        /// <summary>
        /// Method used to arrange the children with in the bounds.
        /// </summary>
        /// <param name="bounds">The size of the layout.</param>
        /// <returns>The layout size.</returns>
        internal override Size LayoutArrangeChildren(Rect bounds)
        {
            double headerTextWidth = bounds.Width;
            double arrowWidth = 0;

            if (_headerInfo.HeaderView.ShowNavigationArrows)
            {
                //// arrow width have forward and backward icon width with
                //// icon left, right and inbetween padding.
                arrowWidth = (2 * _iconWidth) + (3 * _iconPadding);
                headerTextWidth -= arrowWidth;
            }

            bool isRTL = _headerInfo.IsRTLLayout;
            double childStartXPosition;
            double iconVerticalPadding = GetIconVerticalPadding();
            double headerIconHeight = bounds.Height - iconVerticalPadding;
            iconVerticalPadding = iconVerticalPadding / 2;
            foreach (var child in Children)
            {
                if (_headerInfo.HeaderTemplate != null)
                {
                    child.Arrange(new Rect(bounds.Left, bounds.Top, bounds.Width, bounds.Height));
                }
                else
                {
                    if (child == _backwardArrow)
                    {
                        //// In RTL, arrow have left padding.
                        //// In LTR, arrow have padding after the header text.
                        childStartXPosition = isRTL ? _iconPadding : bounds.Width - arrowWidth + _iconPadding;
                        child.Arrange(new Rect(childStartXPosition, iconVerticalPadding, _iconWidth, headerIconHeight));
                    }
                    else if (child == _forwardArrow)
                    {
                        //// In RTL, arrow start position calculated by backward arrow with its left padding and inbetween padding.
                        //// In LTR, arrow have right end padding.
                        childStartXPosition = isRTL ? _iconWidth + (2 * _iconPadding) : bounds.Width - _iconWidth - _iconPadding;
                        child.Arrange(new Rect(childStartXPosition, iconVerticalPadding, _iconWidth, headerIconHeight));
                    }
                    else if (child == _headerTextLabel)
                    {
                        childStartXPosition = isRTL ? arrowWidth : 0;
                        child.Arrange(new Rect(childStartXPosition, 0, headerTextWidth, bounds.Height));
                    }
                }
            }

            return bounds.Size;
        }

        /// <summary>
        /// Method used to measure the children based on width and height value.
        /// </summary>
        /// <param name="widthConstraint">The maximum width request of the layout.</param>
        /// <param name="heightConstraint">The maximum height request of the layout.</param>
        /// <returns>The maximum size of the layout.</returns>
        internal override Size LayoutMeasure(double widthConstraint, double heightConstraint)
        {
            double width = double.IsFinite(widthConstraint) ? widthConstraint : 0;
            double height = double.IsFinite(heightConstraint) ? heightConstraint : 0;
            double headerTextWidth = width;
            double iconVerticalPadding = GetIconVerticalPadding();
            double headerIconHeight = height - iconVerticalPadding;
            if (_headerInfo.HeaderTemplate != null && Children.Count == 0)
            {
                GenerateHeaderViewTemplate();
            }

            if (_headerInfo.HeaderTemplate != null)
            {
                if (Children.Count > 0)
                {
                    //// When data template added, we have added only one child to this layout , then directly used Children[0] to meaure the templated view layout.
                    ((View)Children[0]).Measure(width, height);
                }
            }
            else
            {
                if (_headerInfo.HeaderView.ShowNavigationArrows)
                {
                    //// arrow width have forward and backward icon width with
                    //// icon left, right and inbetween padding.
                    double arrowWidth = (2 * _iconWidth) + (3 * _iconPadding);
                    headerTextWidth -= arrowWidth;
                }

                _headerTextLabel?.Measure(headerTextWidth, height);
                _backwardArrow?.Measure(_iconWidth, headerIconHeight);
                _forwardArrow?.Measure(_iconWidth, headerIconHeight);
            }

            return new Size(width, height);
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
        /// Occurs on tap interaction inside header layout.
        /// </summary>
        /// <param name="e">The tap gesture listener event arguments.</param>
        void ITapGestureListener.OnTap(TapEventArgs e)
        {
            OnInteractionEvent(e.TapPoint, true, e.TapCount);
        }

        /// <summary>
        /// Occurs on double tap interaction inside header layout.
        /// </summary>
        /// <param name="e">The double-tap gesture listener event arguments.</param>
        void IDoubleTapGestureListener.OnDoubleTap(TapEventArgs e)
        {
            OnInteractionEvent(e.TapPoint, true, e.TapCount);
        }

        /// <summary>
        /// Occurs on long press action inside header layout.
        /// </summary>
        /// <param name="e">The long press listener event arguments.</param>
        void ILongPressGestureListener.OnLongPress(LongPressEventArgs e)
        {
            OnInteractionEvent(e.TouchPoint, false);
        }

#if __MACCATALYST__ || (!__ANDROID__ && !__IOS__)
        /// <summary>
        /// Method invokes on touch interaction.
        /// </summary>
        /// <param name="e">The touch event arguments.</param>
        void ITouchListener.OnTouch(PointerEventArgs e)
        {
            // Not to change the text style for century header text
            // No need to hover the header text when the allow view navigation is false.
            if (_headerInfo.View == CalendarView.Century || !_headerInfo.AllowViewNavigation)
            {
                if (_headerTextLabel != null)
                {
                    _headerTextLabel.TextColor = _headerInfo.HeaderView.TextStyle.TextColor;
                }

                return;
            }

            // If the height and width is greater than or equal to the touch point it update the hover date value
            // Example : width = 10, Cell width is considered as 0 to 9.99.
            // Width value 10 is considered as the next cell starting width value
            // The above example is same for height and Touch point of X and Y should be greater than zero
            if (e.TouchPoint.X >= Width || e.TouchPoint.Y >= Height || e.TouchPoint.X < 0 || e.TouchPoint.Y < 0)
            {
                if (_headerTextLabel != null)
                {
                    _headerTextLabel.TextColor = _headerInfo.HeaderView.TextStyle.TextColor;
                }
            }
            else if (e.Action == PointerActions.Moved)
            {
                //// arrow width have forward and backward icon width with
                //// icon left, right and inbetween padding.
                double arrowWidth = _headerInfo.HeaderView.ShowNavigationArrows ? (2 * _iconWidth) + (3 * _iconPadding) : 0;
                double headerTextWidth = Width - arrowWidth;
                bool isRTL = _headerInfo.IsRTLLayout;
                Color headerTextStyleBackground = _headerInfo.TodayHighlightBrush.ToColor();
                if (_headerInfo.HeaderHoverColor != null)
                {
                    headerTextStyleBackground = _headerInfo.HeaderHoverColor.ToColor();
                }

                if (_headerTextLabel != null)
                {
                    if (isRTL ? e.TouchPoint.X > arrowWidth : e.TouchPoint.X < headerTextWidth)
                    {
                        _headerTextLabel.TextColor = headerTextStyleBackground == Colors.Transparent ? _headerTextLabel.TextColor : headerTextStyleBackground;
                    }
                    else
                    {
                        _headerTextLabel.TextColor = _headerInfo.HeaderView.TextStyle.TextColor;
                    }
                }
            }
            else if (e.Action == PointerActions.Exited || e.Action == PointerActions.Cancelled)
            {
                if (_headerTextLabel != null)
                {
                    _headerTextLabel.TextColor = _headerInfo.HeaderView.TextStyle.TextColor;
                }
            }
        }
#endif

        #endregion
    }
}