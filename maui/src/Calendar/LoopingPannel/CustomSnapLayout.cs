using Syncfusion.Maui.Toolkit.Internals;

namespace Syncfusion.Maui.Toolkit.Calendar
{
    /// <summary>
    /// Represents a class which holds the information of custom scroll layout.
    /// </summary>
#if ANDROID
    internal partial class CustomSnapLayout : SnapLayout
#else
    internal partial class CustomSnapLayout : SnapLayout, IPanGestureListener
#endif
    {
        #region Fields

        /// <summary>
        /// The looping panel children count.
        /// </summary>
        const int ChildCount = 3;

        /// <summary>
        /// The current child index while swipe or touch is occur or view is changed.
        /// </summary>
        int _currentChildIndex;

        /// <summary>
        /// The current position of children while swipe is occur other wise position value is null, after swipe action performed the position value is set to be as 0.
        /// </summary>
        double _position;

        /// <summary>
        /// The initial value of x and y position while touch or swipe is occur.
        /// </summary>
        Point? _touchDownPoint = null;

#if __ANDROID__
        /// <summary>
        /// Holds the value that decides to hold or pass the touch to it children.
        /// </summary>
        bool _isMoving = false;

        /// <summary>
        /// The initial value of x and y position while touch or swipe is occur on intercept event.
        /// </summary>
        Point? _interceptDownPoint = null;
#endif

        /// <summary>
        /// The next view navigation is valid or not based on the minimum and maximum date. This is applicable only for swiping.
        /// </summary>
        bool _isValidNextViewNavigation;

        /// <summary>
        /// The previous view navigation is valid or not based on the minimum and maximum date. This applicable only for swiping.
        /// </summary>
        bool _isValidPreviousViewNavigation;

        /// <summary>
        /// The starting date of the calendar.
        /// </summary>
        DateTime _startYearDateTime = new DateTime(0001, 01, 01);

        /// <summary>
        /// The ending date of the calendar.
        /// </summary>
        DateTime _endYearDateTime = new DateTime(9999, 12, 31);

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomSnapLayout"/> class.
        /// </summary>
        /// <param name="calendarInfo">The looping panel information.</param>
        internal CustomSnapLayout(ICalendar calendarInfo)
        {
            _calendarInfo = calendarInfo;
            _currentCalendarView = calendarInfo.View;

#if !__ANDROID__
            // Initialized the pan gesture recognizer instance.
            this.AddGestureListener(this);
#endif

            // Initially current child index equal to 1.
            _currentChildIndex = 1;

            // Initially position value is 0;
            _position = 0;

            // Layout should clip its children to its bounds when the value is true.
            IsClippedToBounds = true;
#if IOS

#if NET10_0
            this.SafeAreaEdges = SafeAreaEdges.None;
#else
            IgnoreSafeArea = true;
#endif

#endif
		}

#endregion

        #region Internal Methods

        /// <summary>
        /// Triggers the animation to move the right side of the calender view.
        /// </summary>
        internal void AnimateMoveToNextView()
        {
            //// Get the index value based on current child index.
            //// Current child index = 0(previous view). Current child index = 1(current view). Current child index 2(next view).
            //// Example: To renderer the current child 2(next view) as a current view. So the current child index = 1.
            //// Calculation: ( 1 + 1) % ChildCount = We get index value = 2(next view).
            int index = (_currentChildIndex + 1) % ChildCount;
            //// In forward, animate the view change by platform basis.
            //// In Windows, navigate the view by opacity animation.
            //// Others, navigate the view by position update animation(sliding animation).
            //// Passing false value denotes this animation is not a backward animation.
#if (__IOS__ && !__MACCATALYST__) || __ANDROID__

            Navigate(index, false);
#else
            NavigateWithOpacity(index, false);
#endif
        }

        /// <summary>
        /// Triggers the animation to move the left side of the calendar view.
        /// </summary>
        internal void AnimateMoveToPreviousView()
        {
            //// Get the index value based on current child index.
            //// Current child index = 0(previous view). Current child index = 1(current view). Current child index 2(next view).
            //// Example: To renderer the current child 0(previous view) as a current view. So the current child index = 1.
            //// Calculation: (ChildCount(3) + 1 - 1) % ChildCount(3) = 0. We get index value = 0(previous view).
            int index = (ChildCount + _currentChildIndex - 1) % ChildCount;
            //// In forward, animate the view change by platform basis.
            //// In Windows, navigate the view by opacity animation.
            //// Others, navigate the view by position update animation(sliding animation).
            //// Passing true value denotes this animation is a backward animation.
#if (__IOS__ && !__MACCATALYST__) || __ANDROID__
            Navigate(index, true);
#else
            NavigateWithOpacity(index, true);
#endif
        }

        /// <summary>
        /// Method to call when the navigation arrows performed it is applicable for windows.
        /// </summary>
        /// <param name="index">To navigate based on index.</param>
        /// <param name="isPreviousAnimation">Check the animation need.</param>
        internal void NavigateWithOpacity(int index, bool isPreviousAnimation)
        {
            // Return the operation while current index and index are same.
            if (_currentChildIndex == index)
            {
                return;
            }

            UpdateVisibleDates(isPreviousAnimation);
            UpdateSpecialAndDisableDates(_visibleDates);
            _position = 0;
            int previousIndex = _currentChildIndex;
            _currentChildIndex = index;
            UpdateCalendarViewVisibleDates(isPreviousAnimation, previousIndex);
            void UpdateProperty(double value)
            {
                // To Get the fade animation.
                Opacity = value;
                LayoutArrangeChildren(new Rect(0, 0, Width, Height));
            }

            AnimationExtensions.Animate(this, "ForwardAnimation", UpdateProperty, 0, 1, 16, 400U, Easing.Linear);
        }

        /// <summary>
        /// Method to navigate the calendar views when the navigation arrows performed.it is applicable for android, IOS, mac.
        /// </summary>
        /// <param name="index">The child index.</param>
        /// <param name="isPreviousAnimation">Check the animation need.</param>
        internal void Navigate(int index, bool isPreviousAnimation)
        {
            if (_currentChildIndex == index)
            {
                return;
            }

            if (AnimationExtensions.AnimationIsRunning(this, "NavigationAnimation") || AnimationExtensions.AnimationIsRunning(this, "NextViewNavigation"))
            {
                return;
            }

            bool isHorizontalDirection = _calendarInfo.NavigationDirection == CalendarNavigationDirection.Horizontal;
            if (isPreviousAnimation)
            {
                double startPosition = isHorizontalDirection ? Width : Height;
                void UpdateProperty(double value)
                {
                    //// On view swipe, same value return two times hence arrange occurs two times for a same position this makes performance issue on view swipe.
                    double tempPosition = (int)(startPosition * value);
                    if (tempPosition == (int)_position)
                    {
                        return;
                    }

                    _position = tempPosition;
                    LayoutArrangeChildren(new Rect(0, 0, Width, Height));
                }

                void Finished(double value, bool isFinished)
                {
                    AnimationFinished(true, index);
                }

                AnimationExtensions.Animate(this, "NavigationAnimation", UpdateProperty, 0, 1, 16, 400U, Easing.Linear, Finished, null);
            }
            else
            {
                double startPosition = isHorizontalDirection ? Width : Height;
                void UpdateProperty(double value)
                {
                    //// On view swipe, same value return two times hence arrange occurs two times for a same position this makes performance issue on view swipe.
                    double tempPosition = -(int)(startPosition * value);
                    if (tempPosition == _position)
                    {
                        return;
                    }

                    _position = tempPosition;
                    LayoutArrangeChildren(new Rect(0, 0, Width, Height));
                }

                void Finished(double value, bool isFinished)
                {
                    AnimationFinished(false, index);
                }

                AnimationExtensions.Animate(this, "NextViewNavigation", UpdateProperty, 0, 1, 16, 400U, Easing.Linear, Finished, null);
            }
        }

        /// <summary>
        /// Method to process key down event in calendar.
        /// </summary>
        /// <param name="args">The key event args.</param>
        internal void ProcessOnKeyDown(KeyEventArgs args)
        {
            if (args.Key == KeyboardKey.Enter)
            {
                DateTime? selectedDate = null;
                if (_calendarInfo.SelectionMode == CalendarSelectionMode.Single)
                {
                    selectedDate = _calendarInfo.SelectedDate;
                }
                else if (_calendarInfo.SelectionMode == CalendarSelectionMode.Multiple)
                {
                    selectedDate = _calendarInfo.SelectedDates[_calendarInfo.SelectedDates.Count - 1].Date;
                }

                if (selectedDate != null)
                {
                    _calendarInfo.TriggerCalendarInteractionEvent(true, 1, selectedDate.Value, CalendarElement.CalendarCell);
                }

                args.Handled = true;
            }
            else if (args.Key == KeyboardKey.Up || args.Key == KeyboardKey.Down || args.Key == KeyboardKey.Right || args.Key == KeyboardKey.Left)
            {
                //// This method will trigger while the pressing shift plus arrow keys and control plus arrows keys.
                ProcessOnArrowKeyPress(args);
            }
        }

        /// <summary>
        /// Method to update the selection.
        /// </summary>
        /// <param name="args">The key event args.</param>
        /// <param name="selectedDateTime">The selected date time.</param>
        internal void UpdateSelectionOnKeyNavigation(KeyEventArgs args, DateTime selectedDateTime)
        {
            (Children[_currentChildIndex] as ICalendarView)?.OnKeyDown(args, selectedDateTime, true);
        }

        /// <summary>
        /// Method to update the focus while view is changed on keyboard interaction.
        /// </summary>
        internal void UpdateFocusOnViewNavigation()
        {
            if (Children.Count == 0)
            {
                return;
            }

            (Children[_currentChildIndex] as ICalendarView)?.SetFocusOnViewChanged();
        }

        /// <summary>
        /// Method to update the last selected date for the keyboard interaction.
        /// </summary>
        /// <param name="selectedDate">The last selected date.</param>
        internal void UpdateLastSelectedDate(DateTime? selectedDate)
        {
            if (selectedDate == null)
            {
                return;
            }

            for (int i = 0; i < ChildCount; i++)
            {
                (Children[i] as ICalendarView)?.UpdatePreviousSelectedDateOnRangeSelection(selectedDate.Value);
            }
        }

		#endregion

        #region Private Methods

        /// <summary>
        /// Method to process the arrow key press on key navigation.
        /// </summary>
        /// <param name="args">The key event args.</param>
        void ProcessOnArrowKeyPress(KeyEventArgs args)
        {
            if (args.IsShiftKeyPressed && (_calendarInfo.SelectionMode == CalendarSelectionMode.Multiple || _calendarInfo.SelectionMode == CalendarSelectionMode.Range))
            {
                ProcessOnShiftKeyPress(args);
            }
#if __IOS__ || __MACCATALYST__
            else if (args.IsAltKeyPressed)
#else
            else if (args.IsCtrlKeyPressed)
#endif
            {
                _calendarInfo.ProcessOnControlKeyPress(args);
            }
            else if (_calendarInfo.SelectionMode == CalendarSelectionMode.Single)
            {
                if (_calendarInfo.SelectedDate.HasValue && CalendarViewHelper.IsDateInCurrentVisibleDate(_calendarInfo, _visibleDates, _calendarInfo.SelectedDate.Value.Date) &&
                    _calendarInfo.SelectedDate != null && Children.Count > _currentChildIndex && Children[_currentChildIndex] is ICalendarView keyListener)
                {
                    keyListener.OnKeyDown(args, _calendarInfo.SelectedDate.Value, false);
                }
            }
        }

        /// <summary>
        /// Method to process the shift key press on key navigation.
        /// </summary>
        /// <param name="args">The key event args.</param>
        void ProcessOnShiftKeyPress(KeyEventArgs args)
        {
            DateTime? selectedDate = null;
            if (_calendarInfo.SelectionMode == CalendarSelectionMode.Multiple)
            {
                if (_calendarInfo.SelectedDates.Count == 0)
                {
                    return;
                }

                DateTime? lastSelectedDate = _calendarInfo.SelectedDates[_calendarInfo.SelectedDates.Count - 1].Date;
                if (CalendarViewHelper.IsDateInCurrentVisibleDate(_calendarInfo, _visibleDates, lastSelectedDate.Value.Date))
                {
                    selectedDate = lastSelectedDate.Value;
                }
            }
            else if (_calendarInfo.SelectionMode == CalendarSelectionMode.Range)
            {
                if (_calendarInfo.SelectedDateRange == null || _calendarInfo.SelectedDateRange.StartDate == null)
                {
                    return;
                }

                DateTime? startDate = _calendarInfo.SelectedDateRange?.StartDate?.Date;
                DateTime? endDate = _calendarInfo.SelectedDateRange?.EndDate?.Date;
                if (endDate == null)
                {
                    endDate = startDate;
                }

                DateTime? dateTime = null;
                switch (_calendarInfo.RangeSelectionDirection)
                {
                    case CalendarRangeSelectionDirection.Default:
                    case CalendarRangeSelectionDirection.Forward:
                    case CalendarRangeSelectionDirection.Both:
                        {
                            dateTime = endDate;
                            break;
                        }

                    case CalendarRangeSelectionDirection.Backward:
                        {
                            dateTime = startDate;
                            break;
                        }

                    case CalendarRangeSelectionDirection.None:
                        {
                            break;
                        }
                }

                if (startDate != null && endDate != null && dateTime != null && (CalendarViewHelper.IsDateInCurrentVisibleDate(_calendarInfo, _visibleDates, dateTime.Value.Date) ||
                   CalendarViewHelper.IsDateInCurrentVisibleDate(_calendarInfo, _visibleDates, startDate.Value.Date) || CalendarViewHelper.IsDateInCurrentVisibleDate(_calendarInfo, _visibleDates, endDate.Value.Date)))
                {
                    selectedDate = dateTime.Value.Date;
                }
            }

            //// Need to process the last selected date and then have to update in the UI.
            if (selectedDate != null && Children.Count > _currentChildIndex && Children[_currentChildIndex] is ICalendarView keyListener)
            {
                keyListener.OnKeyDown(args, selectedDate.Value, false);
            }
        }

        /// <summary>
        /// Method occurs when animation is finished.
        /// </summary>
        /// <param name="isPreviousView">The view is next or previous based on bool.</param>
        /// <param name="index">Based on index view is changed.</param>
        void AnimationFinished(bool isPreviousView, int index)
        {
            UpdateVisibleDates(isPreviousView);
            UpdateSpecialAndDisableDates(_visibleDates);
            _position = 0;
            int previousIndex = _currentChildIndex;
            _currentChildIndex = index;
            LayoutArrangeChildren(new Rect(0, 0, Width, Height));
            UpdateCalendarViewVisibleDates(isPreviousView, previousIndex);
        }

        /// <summary>
        /// Method to update current, next, previous view visible dates.
        /// </summary>
        /// <param name="isPreviousView">The next or previous view.</param>
        void UpdateVisibleDates(bool isPreviousView)
        {
            if (isPreviousView)
            {
                UpdateVisibleDatesOnViewUpdate();
                UpdatePreviousViewVisibleDates();
            }
            else
            {
                UpdateVisibleDatesOnViewUpdate(true);
                UpdateNextViewVisibleDates();
            }
        }

        /// <summary>
        /// Method occurs when touch is completed.
        /// </summary>
        void TouchCompleted(bool isHorizontalDirection, Point velocity)
        {
            // To get the size of children based on direction.
            double size = isHorizontalDirection ? Width : Height;

            // Bool value to get whether the view is need to update. Position value is zero while fling to the next/previous nonvalid view. For that we no need to update the view.
            bool isValidFling = _position != 0 && ((!isHorizontalDirection && Math.Abs(velocity.Y) > 1000) || (isHorizontalDirection && Math.Abs(velocity.X) > 1000));

            // If the swiping position is greater than minimum swipe offset(size divided by 15) then view is move to the next/previous children view.
            // The 1000 specifies the units of the velocity.The velocity is in pixels/second. The velocity value greater than offset value 1000 then view is move to the next/previous children view.
            if ((Math.Abs(_position) > size / 15) || isValidFling)
            {
                // To calculate the difference between total width or height is subtracted by position of the touch point.
                double difference = size - Math.Abs(_position);
                double startPosition = _position;
                bool isPreviousView = startPosition > 0;

                // Method to update the position based on animation value.
                void UpdateProperty(double value)
                {
                    _position = isPreviousView ? startPosition + (difference * value) : startPosition - (difference * value);

                    // Arrange the children after touch is completed.
                    LayoutArrangeChildren(new Rect(0, 0, Width, Height));
                }

                // To update the current children view after the animation.
                void Finished(double value, bool isFinished)
                {
                    int index = isPreviousView ? (3 + _currentChildIndex - 1) % 3 : (_currentChildIndex + 1) % 3;
                    AnimationFinished(isPreviousView, index);
                }

                AnimationExtensions.Animate(this, "Navigation", UpdateProperty, 0, 1, 16, 250U, Easing.Linear, Finished, null);
            }
            else
            {
                // Retain the existing position.
                // No need to change update the next or previous view.
                double startPosition = _position;
                void UpdateProperty(double value)
                {
                    _position = startPosition - (startPosition * value);
                    LayoutArrangeChildren(new Rect(0, 0, Width, Height));
                }

                void Finished(double value, bool isFinished)
                {
                    _position = 0;
                }

                AnimationExtensions.Animate(this, "ResetNavigation", UpdateProperty, 0, 1, 16, 250U, Easing.Linear, Finished, null);
            }
        }

        /// <summary>
        /// Method to handle the pan gesture actions.
        /// </summary>
        /// <param name="status">The pan gesture status.</param>
        /// <param name="point">The pan gesture touch point.</param>
        /// <param name="velocity">The velocity</param>
        void HandlePanTouch(GestureStatus status, Point point, Point velocity)
        {
            // Don't need to navigate the view, when the Enable swipe selection is true in Range Selection mode
            if (_calendarInfo.EnableSwipeSelection && (_calendarInfo.SelectionMode == CalendarSelectionMode.Range || _calendarInfo.SelectionMode == CalendarSelectionMode.MultiRange))
            {
                // For Month view, don't need to navigate the view
                // For Year view, don't need to navigate the view only when the allow view navigation is false. Because we won't select the range while allow view navigation is false.
                if (_calendarInfo.View == CalendarView.Month || !_calendarInfo.AllowViewNavigation)
                {
                    (Children[_currentChildIndex] as ICalendarView)?.HandleSwipeRangeSelection(status, point);
                    return;
                }
            }

            bool isHorizontalDirection = _calendarInfo.NavigationDirection == CalendarNavigationDirection.Horizontal;
            if (status == GestureStatus.Started)
            {
                _touchDownPoint = new Point(point.X, point.Y);
                int numberOfWeeks = _calendarInfo.MonthView.GetNumberOfWeeks();
                _isValidPreviousViewNavigation = CalendarViewHelper.IsValidPreviousDatesNavigation(_visibleDates, _calendarInfo.View, numberOfWeeks, _calendarInfo.MinimumDate, _calendarInfo.Identifier);
                _isValidNextViewNavigation = CalendarViewHelper.IsValidNextDatesNavigation(_visibleDates, _calendarInfo.View, numberOfWeeks, _calendarInfo.MaximumDate, _calendarInfo.Identifier);
            }
            else if (status == GestureStatus.Running)
            {
                if (_touchDownPoint == null)
                {
                    _touchDownPoint = new Point(point.X, point.Y);
                    int numberOfWeeks = _calendarInfo.MonthView.GetNumberOfWeeks();
                    _isValidPreviousViewNavigation = CalendarViewHelper.IsValidPreviousDatesNavigation(_visibleDates, _calendarInfo.View, numberOfWeeks, _calendarInfo.MinimumDate, _calendarInfo.Identifier);
                    _isValidNextViewNavigation = CalendarViewHelper.IsValidNextDatesNavigation(_visibleDates, _calendarInfo.View, numberOfWeeks, _calendarInfo.MaximumDate, _calendarInfo.Identifier);
                }

                bool isNextView;
                if (isHorizontalDirection)
                {
                    isNextView = _calendarInfo.IsRTLLayout ? point.X - _touchDownPoint.Value.X > 0 : point.X - _touchDownPoint.Value.X < 0;
                }
                else
                {
                    isNextView = point.Y - _touchDownPoint.Value.Y < 0;
                }

                if ((!isNextView && !_isValidPreviousViewNavigation) || (isNextView && !_isValidNextViewNavigation))
                {
                    return;
                }

                // To get the position value based on Horizontal direction is true and false.
                _position = isHorizontalDirection ? point.X - _touchDownPoint.Value.X : point.Y - _touchDownPoint.Value.Y;
                bool isNextSwipe = _position < 0;
                List<DateTime> dates = GetNextViewVisibleDate(isNextSwipe);
                UpdateSpecialAndDisableDates(dates);

                // To arrange the children when the touch status type is running state.
                LayoutArrangeChildren(new Rect(0, 0, Width, Height));
            }
            else if (status == GestureStatus.Completed || status == GestureStatus.Canceled)
            {
                // To call the method when the touch is completed or canceled.
                TouchCompleted(isHorizontalDirection, velocity);
                _touchDownPoint = null;
            }
        }

        #endregion

        #region Override Methods

        /// <summary>
        /// Method used to arrange the children with in the bounds.
        /// </summary>
        /// <param name="bounds">The size of the layout.</param>
        /// <returns>The layout size.</returns>
        internal override Size LayoutArrangeChildren(Rect bounds)
        {
            if (Children.Count == 0)
            {
                return bounds.Size;
            }

            // To Arrange the children based on the x position when the navigation direction is horizontal.
            if (_calendarInfo.NavigationDirection == CalendarNavigationDirection.Horizontal)
            {
                // Initially the startXPosition value is 0.
                double startXPosition = 0;
                for (int i = 0; i < ChildCount; i++)
                {
                    // Current index is calculated based on current child and number of children of custom snap layout.
                    // Current child index = 0(previous view). Current child index = 1(current view). Current child index 2(next view).
                    // Example: To renderer the current child 2(next view) as a current view.
                    // Current child index = 2 while child count = 0.
                    // Current child index = 0 while child count = 1.
                    // Current child index = 1 while child count = 2.
                    int currentIndex = (i + _currentChildIndex) % ChildCount;
                    Children[currentIndex].Arrange(new Rect(startXPosition + _position, 0, bounds.Width, bounds.Height));

                    // The x position value is calculated based on current children count.
                    // If the current child 2(next view) as a current view and assume the width value is 100.
                    // Assume CurrentChildIndex is 2 and width is 100.
                    // In loop, i == 0 current index is 2, the current view will be rendered in the position of 0 to 100. This is the visible region in view. StartXPosition will be changed to 100 to render the next view.
                    // On i = 1 current index is 0, this view will be considered as next view and the view will be rendered in the position of 100 to 200. StartXPosition will be changed to -100 to render the previous view.
                    // On i = 2 current index is 1, this will be considered as previous view and will be renders in the position of -100 to 0.
                    startXPosition = i == 1 ? -bounds.Width : startXPosition + bounds.Width;
                }
            }
            else
            {
                // To Arrange the children based on the y position when the navigation direction is vertical.
                // Initially the startYPosition value is 0.
                double startYPosition = 0;
                for (int i = 0; i < ChildCount; i++)
                {
                    // Current index is calculated based on current child and number of children of custom snap layout.
                    // Current child index = 0(previous view). Current child index = 1(current view). Current child index 2(next view).
                    // Example: To renderer the current child 2(next view) as a current view.
                    // Current child index = 2 while child count = 0.
                    // Current child index = 0 while child count = 1.
                    // Current child index = 1 while child count = 2.
                    int currentIndex = (i + _currentChildIndex) % ChildCount;
                    Children[currentIndex].Arrange(new Rect(0, startYPosition + _position, bounds.Width, bounds.Height));

                    // The y position value is calculated based on current children count.
                    // If the current child 2(next view) as a current view and assume the height value is 100.
                    // Assume CurrentChildIndex is 2 and height is 100.
                    // In loop, i == 0 current index is 2, the current view will be rendered in the position of 0 to 100. This is the visible region in view. StartYPosition will be changed to 100 to render the next view.
                    // On i = 1 current index is 0, this view will be considered as next view and the view will be rendered in the position of 100 to 200. StartYPosition will be changed to -100 to render the previous view.
                    // On i = 2 current index is 1, this will be considered as previous view and will be renders in the position of -100 to 0.
                    startYPosition = i == 1 ? -bounds.Height : startYPosition + bounds.Height;
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
            if (Children.Count == 0)
            {
                // Get the display date based on minimum and maximum date time.
                DateTime displayDate = CalendarViewHelper.GetValidDisplayDate(_calendarInfo);

                // Initially generate the visible dates collections.
                CreateOrResetVisibleDates(displayDate);
                AddChildren();
            }

            foreach (View view in Children)
            {
                view.Measure(widthConstraint, heightConstraint);
            }

            return new Size(widthConstraint, heightConstraint);
        }

#if __ANDROID__

        /// <summary>
        ///  Return false then pass the touch to its children.
        ///  Return true then it holds the touch to its own.
        /// </summary>
        /// <param name="touchPoint">The touch position.</param>
        /// <param name="action">The touch action.</param>
        /// <returns>True, If action is performed with condition or else false. </returns>
        internal override bool OnInterceptTouchEvent(Point touchPoint, string action)
        {
            if (action == "Down")
            {
                _interceptDownPoint = touchPoint;
                //// Pass the touch to children, because we cannot decide the touch is for tap or scroll.
                _isMoving = false;
            }
            else if (action == "Move")
            {
                if (_interceptDownPoint == null)
                {
                    _interceptDownPoint = touchPoint;
                }

                bool isHorizontalDirection = _calendarInfo.NavigationDirection == CalendarNavigationDirection.Horizontal;
                double xOffset = Math.Abs(_interceptDownPoint.Value.X - touchPoint.X);
                double yOffset = Math.Abs(_interceptDownPoint.Value.Y - touchPoint.Y);
                //// Check the below conditions for vertical scrolling and tap action, If we tap then it also makes the x offset difference from the touch down point, so skip the touch. If we scroll vertically then it also makes x offset difference so skip that vertical scroll scenario also.
                if (isHorizontalDirection && xOffset > 5 && yOffset < xOffset)
                {
                    //// Hold the touch while pan or swipe the layout on horizontal while navigation direction is horizontal.
                    _isMoving = true;
                }
                else if (!isHorizontalDirection && yOffset > 5 && yOffset > xOffset)
                {
                    //// Hold the touch while pan or swipe the layout on vertical while navigation direction is vertical.
                    _isMoving = true;
                }
                else if (_calendarInfo.EnableSwipeSelection && (_calendarInfo.SelectionMode == CalendarSelectionMode.Range || _calendarInfo.SelectionMode == CalendarSelectionMode.MultiRange) && (yOffset > 5 || xOffset > 5))
                {
                    //// Hold the touch while swipe to select the range and restrict the current touch by checking offset 5 to differentiate the tap and swipe interaction.
                    _isMoving = true;
                }
            }
            else if (action == "Up" || action == "Cancel")
            {
                _interceptDownPoint = null;
            }

            return _isMoving;
        }

        /// <summary>
        ///  Return false then pass the touch to its parent.
        ///  Return true then it holds the touch to its own.
        ///  Its will triggered from intercept touch event method.
        /// </summary>
        /// <param name="touchPoint">The touch position.</param>
        /// <param name="action">The touch action.</param>
        /// <returns>True, If action is performed with condition or else false. </returns>
        internal override bool? OnDisAllowInterceptTouchEvent(Point touchPoint, string action)
        {
            if (action == "Down")
            {
                return true;
            }
            else if (action == "Move")
            {
                if (_interceptDownPoint == null)
                {
                    return true;
                }

                bool isHorizontalDirection = _calendarInfo.NavigationDirection == CalendarNavigationDirection.Horizontal;
                double xOffset = Math.Abs(_interceptDownPoint.Value.X - touchPoint.X);
                double yOffset = Math.Abs(_interceptDownPoint.Value.Y - touchPoint.Y);
                //// Return false while scroll vertically on horizontal navigation direction.
                if (isHorizontalDirection && yOffset <= xOffset)
                {
                    //// Hold the touch while pan or swipe the layout on horizontal while navigation direction is horizontal.
                    return true;
                }
                //// Return false while scroll horizontally on vertical navigation direction.
                else if (!isHorizontalDirection && yOffset >= xOffset)
                {
                    //// Hold the touch while pan or swipe the layout on vertical while navigation direction is vertical.
                    return true;
                }

                //// Hold the touch while swipe to select the range.
                return _calendarInfo.EnableSwipeSelection && (_calendarInfo.SelectionMode == CalendarSelectionMode.Range || _calendarInfo.SelectionMode == CalendarSelectionMode.MultiRange);
            }

            return false;
        }

        /// <summary>
        /// Used to handle the swiping action for custom scroll layout.
        /// </summary>
        /// <param name="touchPoint">The touch position.</param>
        /// <param name="action">Touch action.</param>
        /// <param name="velocity">The velocity.</param>
        internal override void OnHandleTouch(Point touchPoint, string action, Point velocity)
        {
            GestureStatus? status = null;
            if (action == "Down")
            {
                status = GestureStatus.Started;
            }
            else if (action == "Move")
            {
                status = GestureStatus.Running;
            }
            else if (action == "Up")
            {
                status = GestureStatus.Completed;
            }
            else if (action == "Cancel")
            {
                status = GestureStatus.Canceled;
            }

            if (status == null)
            {
                return;
            }

            HandlePanTouch(status.Value, touchPoint, velocity);
        }
#endif

        #endregion

#if !__ANDROID__

        #region Interface Implementation

        /// <summary>
        /// Gets a value indicating whether the view to pass the touches on either the parent or child view.
        /// </summary>
        bool IGestureListener.IsTouchHandled => true;

        /// <summary>
        /// Method invokes on pan gesture action is occur.
        /// </summary>
        /// <param name="e">Pan event argument details</param>
        void IPanGestureListener.OnPan(PanEventArgs e)
        {
            HandlePanTouch(e.Status, e.TouchPoint, e.Velocity);
        }

        #endregion

#endif
    }
}