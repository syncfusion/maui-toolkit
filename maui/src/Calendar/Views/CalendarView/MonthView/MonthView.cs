using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using Syncfusion.Maui.Toolkit.Internals;
using Syncfusion.Maui.Toolkit.Localization;
using Syncfusion.Maui.Toolkit.Graphics.Internals;
using PointerEventArgs = Syncfusion.Maui.Toolkit.Internals.PointerEventArgs;
using Globalization = System.Globalization;
using Rect = Microsoft.Maui.Graphics.Rect;

namespace Syncfusion.Maui.Toolkit.Calendar
{
    /// <summary>
    /// Represents month view which contains month cells, week number views and month view header.
    /// </summary>
    internal class MonthView : SfView, ITapGestureListener, IDoubleTapGestureListener, ILongPressGestureListener, ITouchListener, IKeyboardListener
    {
        #region Fields

        /// <summary>
        /// The days per week.
        /// </summary>
        const int DaysPerWeek = 7;

        /// <summary>
        /// Maximum count of the cell when ShowTrailingAndLeadingDates set to false.
        /// </summary>
        const int MaxCellCount = 31;

        /// <summary>
        /// The HighlightPadding is used for the space in between the selection shape of the cell.
        /// </summary>
        const int HighlightPadding = 2;

        /// <summary>
        /// The calendar view info.
        /// </summary>
        readonly ICalendar _calendarViewInfo;

        /// <summary>
        /// The visible dates for the view.
        /// </summary>
        List<DateTime> _visibleDates;

        /// <summary>
        /// The disabled dates for the view.
        /// </summary>
        List<DateTime> _disabledDates;

        /// <summary>
        /// The special dates for the view.
        /// </summary>
        List<CalendarIconDetails> _specialDates;

        /// <summary>
        /// The selected date.
        /// </summary>
        DateTime? _selectedDate;

#if __IOS__ || __MACCATALYST__
        /// <summary>
        /// It holds whether the view is double tap or not.
        /// </summary>
        bool _isDoubleTap;
#endif

        /// <summary>
        /// The selected range.
        /// </summary>
        CalendarDateRange? _selectedRange;

        /// <summary>
        /// The selected date ranges.
        /// </summary>
        ObservableCollection<CalendarDateRange>? _selectedDateRanges;

        /// <summary>
        /// It holds the valid interaction start date value on swipe range selection.
        /// The pan gesture started then the initialStartRangeDate will update. The pan date is greater than initialStartRangeDate then range selection end date will update.
        /// The pan date is lesser than initialStartRangeDate then start date will update.
        /// The pan gesture status was completed or canceled the initialStartDateRange value updated as null.
        /// </summary>
        DateTime? _initialStartRangeDate;

        /// <summary>
        /// The selected dates.
        /// </summary>
        ObservableCollection<DateTime> _selectedDates;

        /// <summary>
        /// Gets or sets the month cell views created from data template.
        /// </summary>
        List<View>? _monthCells;

        /// <summary>
        /// The number of visible weeks in the month view.
        /// </summary>
        int _numberOfWeeks;

#if MACCATALYST || (!ANDROID && !IOS)
        /// <summary>
        /// The month hover view.
        /// </summary>
        readonly MonthHoverView _hoverView;
#endif

        /// <summary>
        /// Gets or sets the virtual month cell and week number semantic nodes.
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
        DateTime? _previousSelectedDate;

        /// <summary>
        /// To store the previous selected date range for range selection.
        /// </summary>
        DateTime? _previousSelectedRangeDate;

		/// <summary>
		/// To store the selected date data template view
		/// </summary>
		View? _selectionCellTemplateView;

		/// <summary>
		/// To store the selected cell template previous view
		/// </summary>
		View? _previousSelectionCellTemplateView;

		/// <summary>
		/// To store the previous month cell template view
		/// </summary>
		View? _previousMonthCellTemplateView;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="MonthView"/> class.
		/// </summary>
		/// <param name="calendarView">The calendar view.</param>
		/// <param name="visibleDates">The visible dates for the view.</param>
		/// <param name="selectedDate">The selected date for the view.</param>
		/// <param name="disabledDates">The disabled dates for the view.</param>
		/// <param name="specialDatesDetails">The special dates for the view.</param>
		/// <param name="isCurrentView">Defines whether the view is current view or not.</param>
		internal MonthView(ICalendar calendarView, List<DateTime> visibleDates, DateTime? selectedDate, List<DateTime> disabledDates, List<CalendarIconDetails> specialDatesDetails, bool isCurrentView)
        {
            _isCurrentView = isCurrentView;
            _calendarViewInfo = calendarView;
            _visibleDates = visibleDates;
            _selectedDate = selectedDate;
            _disabledDates = disabledDates;
            _specialDates = specialDatesDetails;
            _numberOfWeeks = CalendarViewHelper.GetNumberOfWeeks(_calendarViewInfo.MonthView);
            UpdateSelectedRangeValue(_calendarViewInfo.SelectedDateRange);
            UpdateSelectedDateRangesValue(_calendarViewInfo.SelectedDateRanges);
            _selectedDates = new ObservableCollection<DateTime>(_calendarViewInfo.SelectedDates);
            if (_calendarViewInfo.MonthView.CellTemplate != null)
            {
                DrawingOrder = DrawingOrder.AboveContent;
                _monthCells = new List<View>();
                GenerateMonthCells(isCurrentView);
            }
            else
            {
                DrawingOrder = DrawingOrder.BelowContent;
            }

#if MACCATALYST || (!ANDROID && !IOS)
            _hoverView = new MonthHoverView(_calendarViewInfo, visibleDates, disabledDates, _selectedRange);
            Add(_hoverView);
            this.AddTouchListener(this);
#endif
            this.AddKeyboardListener(this);
            this.AddGestureListener(this);
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Method to update the private variable for the keyboard interaction.
        /// </summary>
        /// <param name="date">The previous selected date.</param>
        internal void UpdatePreviousSelectedDateOnRangeSelection(DateTime date)
        {
            _previousSelectedRangeDate = date;
        }

#if __IOS__ || __MACCATALYST__
        /// <summary>
        /// Occurs on single tap inside month view.
        /// </summary>
        /// <param name="point">The tap point value.</param>
        /// <param name="tapCount">The tap count value.</param>
        internal void OnSingleTapConfirmed(Point point, int tapCount)
        {
            if (_isDoubleTap)
            {
                _isDoubleTap = false;
                return;
            }

            OnInteractionEvent(point, true, tapCount);
        }

        /// <summary>
        /// Occurs on selection inside month view.
        /// </summary>
        /// <param name="point">The select point.</param>
        internal void OnSelection(Point point)
        {
            DateTime? tappedDate = CalendarViewHelper.GetMonthDateFromPosition(point, Width, Height, _calendarViewInfo, _visibleDates);
            //// To skip the selection interaction while disabled dates are tapped.
            if (tappedDate == null || !CalendarViewHelper.IsInteractableDate(tappedDate.Value, _disabledDates, _visibleDates, _calendarViewInfo, _numberOfWeeks))
            {
                return;
            }

            SetFocus(10);
            switch (_calendarViewInfo.SelectionMode)
            {
                case CalendarSelectionMode.Single:
                    {
                        if (_calendarViewInfo.CanToggleDaySelection && _selectedDate?.Date == tappedDate?.Date)
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
        }
#endif

		/// <summary>
		/// Method to update when visible dates changed.
		/// </summary>
		/// <param name="visibleDates">The visible dates.</param>
		/// <param name="isCurrentView">Defines whether the view is current view or not.</param>
		/// <param name="customSnapLayout">Gets the month view instance for current canvas.</param>
		internal void UpdateVisibleDatesChange(List<DateTime> visibleDates, bool isCurrentView, CustomSnapLayout customSnapLayout)
        {
            int numberOfVisibleWeeks = CalendarViewHelper.GetNumberOfWeeks(_calendarViewInfo.MonthView);
            bool isNumberOfWeeksChanged = _numberOfWeeks != numberOfVisibleWeeks;
            _numberOfWeeks = numberOfVisibleWeeks;
            int previousVisibleDatesCount = _visibleDates.Count;
            //// This method only triggers when the previous visible dates are not equal to the current visible dates.
            _visibleDates = visibleDates;

			// Proceed only if a SelectionCellTemplate is defined in the calendar view info
			if (_calendarViewInfo.SelectionCellTemplate != null && _calendarViewInfo.SelectionMode == CalendarSelectionMode.Single)
			{
				// If the custom snap layout is null, exit early (no layout to process)
				if (customSnapLayout == null)
				{
					return;
				}

				// Iterate through each MonthViewLayout inside the customSnapLayout
				foreach (MonthViewLayout layout in customSnapLayout.Children)
				{
					// Skip if the layout or its children are null
					if (layout?.Children == null)
					{
						continue;
					}

					// Iterate through each view in the layout's children
					foreach (var view in layout.Children)
					{
						// Check if the view is a MonthView and has child views
						if (view is not MonthView month || month.Children == null)
						{
							continue;
						}

						// Get the reference to the current selectionCellTemplateView from the month view
						var child = month._selectionCellTemplateView;

						// Skip if the selection cell template view is not present
						if (child == null)
						{
							continue;
						}

						// If a CellTemplate is defined for the MonthView
						if (_calendarViewInfo.MonthView.CellTemplate != null)
						{
							// Find the index of the selection cell in the month view's children
							int index = month.Children.IndexOf(child);

							// Validate the index and ensure the cell at the index matches the selection view
							if (month._monthCells != null && index >= 0 && index < month._monthCells.Count)
							{
								// Skip if the selection view is already correctly placed
								if (month._monthCells[index] == child)
								{
									continue;
								}

								// Skip if the selected date is already within the visible dates of this month
								if (month._visibleDates.Contains(_selectedDate!.Value))
								{
									continue;
								}
							}
						}
						else
						{
							// Skip if the child view is not the current selection cell template view
							if (child != month._selectionCellTemplateView)
							{
								continue;
							}
						}

						// Default values for visibility checks
						bool isVisible = false;
						bool checkVisibility = false;

						// Proceed only for single selection mode and a selected date is available
						if (_selectedDate.HasValue)
						{
							DateTime selectedDate = _selectedDate.Value;

							// Check if the selected date is among the visible dates of the month
							bool isInVisibleDates = month._visibleDates?.Contains(selectedDate) ?? false;

							// Check if the selected date is allowed via the selection predicate
							bool isSelectable = _calendarViewInfo.IsSelectableDayPredicate(selectedDate);

							// Validate the selected date against calendar rules and settings
							if (selectedDate >= _calendarViewInfo.MinimumDate && selectedDate <= _calendarViewInfo.MaximumDate &&
								(!isInVisibleDates || _calendarViewInfo.EnablePastDates) && (isInVisibleDates && isSelectable) &&
								(_calendarViewInfo.ShowTrailingAndLeadingDates || selectedDate.Month == _calendarViewInfo.DisplayDate.Month))
							{
								// Mark that visibility check is needed
								checkVisibility = true;

								// If using a cell template and selected date is not in visible dates, remove the template
								if (_calendarViewInfo.MonthView.CellTemplate != null)
								{
									if (!isInVisibleDates)
									{
										MonthView.RemoveTemplateView(month, child);
										continue;
									}
								}
								else
								{
									isVisible = isInVisibleDates;
								}
							}
						}

						// If no visibility check passed (invalid selection or not visible), handle cleanup
						if (!checkVisibility)
						{
							// If using a cell template, remove it
							if (_calendarViewInfo.MonthView.CellTemplate != null)
							{
								// If the selection cell template on canvas following condition based that time should remove the selection cell template.
								MonthView.RemoveTemplateView(month, child);
							}
							else
							{
								// Otherwise, just hide the view
								child.IsVisible = isVisible;
							}
						}

						// Exit inner loop after processing the first valid selection cell
						break;
					}
				}
			}

#if MACCATALYST || (!ANDROID && !IOS)
			_hoverView.UpdateVisibleDatesChange(visibleDates);
#endif
            //// In cell template need to draw the selection.
            //// If cell template null then need to draw the visible dates and selection.
            InvalidateDrawable();
            if (_calendarViewInfo.MonthView.CellTemplate == null)
            {
                return;
            }

            //// If previous and current month visible dates count are not equal we have to remove all the cells and generate the month cells again,
            //// else we just update the views like just adding and removing the needed cells.
            if (isNumberOfWeeksChanged)
            {
                RemoveMonthCellsHandler();
                _monthCells = new List<View>();
                GenerateMonthCells(isCurrentView);
                //// Need to measure the children when the number of weeks is changed.
                InvalidateViewMeasure();
            }
            else
            {
                Globalization.Calendar cultureCalendar = CalendarViewHelper.GetCalendar(_calendarViewInfo.Identifier.ToString());
                int visibleDatesCount = _visibleDates.Count;
                int currentMonth = _numberOfWeeks == 6 ? cultureCalendar.GetMonth(_visibleDates[visibleDatesCount / 2]) : cultureCalendar.GetMonth(_visibleDates[0]);
                //// If the ShowTrailingLeadingDates are set to false and previous or current visible dates are not equal then we have to invalidate the measure.
                //// Because the child position change on every views when ShowTrailingLeadingDates set to false in the month view.
                //// Need to update the children visibility and measure while the view reaches min date and max date.
                bool isNeedInvalidate = !CalendarViewHelper.ShowTrailingLeadingDates(_calendarViewInfo) || previousVisibleDatesCount != visibleDatesCount;
                if (_monthCells != null && _monthCells.Count != 0)
                {
                    UpdateMonthCellTemplateViews(currentMonth, isNeedInvalidate);
                }
            }
        }

        /// <summary>
        /// Method to update when special and disabled dates on view changed.
        /// </summary>
        /// <param name="disabledDates">The disabled dates for this view.</param>
        /// <param name="specialDatesDetails">The special dates for this view.</param>
        internal void UpdateDisableAndSpecialDateChange(List<DateTime>? disabledDates, List<CalendarIconDetails>? specialDatesDetails)
        {
            bool isNeedToInvalidate = false;
#if MACCATALYST || (!ANDROID && !IOS)
            //// Here we are updating the hover position before checking the previous and current disabled dates are equal.
            //// Because the previous and current view disabled dates may be same and we can't update the hover position on swiping to next or previous view.
            //// On view navigation, the mouse hover position will remain same but visible dates will be changed.
            //// So we are removing hovering when updating disabled date for each view.
            //// If we not update the hover position, the hovering view will be remain even after view changed.
            _hoverView.UpdateHoverPosition(null);
#endif
            //// Disabled dates is null then the method called only for update special dates.
            //// Condition to check whether the previous disabled dates and current disabled dates are equal to prevent the triggering of invalidate draw.
            if (disabledDates != null && _disabledDates != disabledDates && !_disabledDates.SequenceEqual(disabledDates))
            {
                _disabledDates = disabledDates;
#if MACCATALYST || (!ANDROID && !IOS)
                _hoverView.UpdateDisabledDates(disabledDates);
#endif
                isNeedToInvalidate = true;
            }

            //// Special dates is null then the method called only for update disabled dates.
            //// Condition to check whether the previous special dates and current special dates are equal to prevent the triggering of invalidate draw.
            if (specialDatesDetails != null && _specialDates != specialDatesDetails && !_specialDates.SequenceEqual(specialDatesDetails))
            {
                _specialDates = specialDatesDetails;
                isNeedToInvalidate = true;
            }

			if (_selectionCellTemplateView != null && _calendarViewInfo.SelectionMode == CalendarSelectionMode.Single)
			{
				// If I select any date on current month and selectable predicate date is same as selected date.
				if (_selectedDate != null && !_calendarViewInfo.IsSelectableDayPredicate((DateTime)_selectedDate))
				{
					// To handle the selection cell template visibility
					HideSelectionCellTemplateView();
				}
			}

			if (isNeedToInvalidate)
            {
                InvalidateDrawable();
            }
        }

        /// <summary>
        /// Method to update selected date only on needed views.
        /// </summary>
        internal void UpdateSelectionValue()
        {
			// If Selected date changed dynamically that time it can perform
			if (_selectionCellTemplateView != null && _calendarViewInfo.SelectionMode == CalendarSelectionMode.Single)
			{
				// To handle the selection cell template visibility
				HideSelectionCellTemplateView();
			}

			DateTime? previousSelectedDate = _selectedDate;
            _selectedDate = _calendarViewInfo.SelectedDate;
            if (_calendarViewInfo.SelectionMode != CalendarSelectionMode.Single || _selectedDate?.Date == previousSelectedDate?.Date)
            {
                return;
            }

            DateTime firstDate = _visibleDates[0];
            DateTime lastDate = _visibleDates[_visibleDates.Count - 1];
            if (CalendarViewHelper.IsDateWithinDateRange(_selectedDate, firstDate, lastDate) || CalendarViewHelper.IsDateWithinDateRange(previousSelectedDate, firstDate, lastDate))
            {
                InvalidateDrawable();
            }
        }

        /// <summary>
        /// Method to update range start and end date.
        /// </summary>
        internal void UpdateRangeSelection()
        {
            CalendarDateRange? oldSelectedRange = _selectedRange;
            UpdateSelectedRangeValue(_calendarViewInfo.SelectedDateRange);
            if (CalendarViewHelper.IsSameRange(_calendarViewInfo.View, oldSelectedRange, _calendarViewInfo.SelectedDateRange, _calendarViewInfo.Identifier))
            {
                return;
            }

#if MACCATALYST || (!ANDROID && !IOS)
            _hoverView.UpdateSelectedDateRange(_selectedRange);
#endif
            //// Does not need to update the UI while the selection selected range changes on single and multiple selection.
            if (_calendarViewInfo.SelectionMode != CalendarSelectionMode.Range)
            {
                return;
            }

            DateTime firstDate = _visibleDates[0].Date;
            DateTime lastDate = _visibleDates[_visibleDates.Count - 1].Date;

            // To remove the previous range is present in current view.
            bool isPreviousRangeInBetweenRange = IsRangeInBetweenRange(oldSelectedRange?.StartDate, oldSelectedRange?.EndDate, firstDate, lastDate);

            // To update the new range is present in current view.
            bool isRangeInBetweenRange = IsRangeInBetweenRange(_selectedRange?.StartDate, _selectedRange?.EndDate, firstDate, lastDate);

            bool isViewNeedUpdateRangeSelection = IsViewNeedUpdateRangeSelection(oldSelectedRange, firstDate, lastDate);
            if (isPreviousRangeInBetweenRange || isRangeInBetweenRange || isViewNeedUpdateRangeSelection)
            {
                InvalidateDrawable();
            }
        }

        /// <summary>
        /// Method to update the selected date ranges.
        /// </summary>
        internal void UpdateMultiRangeSelection()
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

            // Need to update the view while the new ranges are present in the current view or existing current view ranges are removed.
            if (IsCurrentViewRangesUpdated(oldSelectedRanges))
            {
                InvalidateDrawable();
            }
        }

        /// <summary>
        /// Method to update selected dates.
        /// </summary>
        internal void UpdateSelectedDates()
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
        internal void UpdateSelectedDatesOnAction(NotifyCollectionChangedEventArgs e)
        {
            ObservableCollection<DateTime> oldSelectedDates = new ObservableCollection<DateTime>(_selectedDates);
            _selectedDates = new ObservableCollection<DateTime>(_calendarViewInfo.SelectedDates);
            if (_calendarViewInfo.SelectionMode != CalendarSelectionMode.Multiple)
            {
                return;
            }

            DateTime firstDate = _visibleDates[0];
            DateTime lastDate = _visibleDates[_visibleDates.Count - 1];
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
        /// Method to invalidate view cells.
        /// </summary>
        internal void InvalidateMonthView()
        {
			// Check if the SelectionMode is not set to 'Single'.
			// If the SelectionMode is not 'Single', hide the selection cell template view by calling the SelectionCellTemplateVisibility method.
			if (_selectionCellTemplateView != null && _calendarViewInfo.SelectionMode != CalendarSelectionMode.Single)
			{
				// To handle the selection cell template visibility
				HideSelectionCellTemplateView();
			}

			InvalidateDrawable();
        }

        /// <summary>
        /// Method to invalidate view measures.
        /// </summary>
        internal void InvalidateViewMeasure()
        {
#if __ANDROID__
            MeasureContent(Width, Height);
            ArrangeContent(new Rect(0, 0, Width, Height));
#else
            InvalidateMeasure();
#endif
        }

        /// <summary>
        /// Method to invalidate view cells and hover view.
        /// </summary>
        internal void InvalidateView()
        {
			// Check if selectedDate and selectionCellTemplateView is not null before proceeding with the date restriction validation logic.
			if (_selectionCellTemplateView != null && _selectedDate != null)
			{
				bool isInVisibleDates = _visibleDates?.Contains(_selectedDate.Value) ?? false;
				bool isTemplateView = false;
				if (_selectedDate >= _calendarViewInfo.MinimumDate && _selectedDate <= _calendarViewInfo.MaximumDate &&
					(_calendarViewInfo.ShowTrailingAndLeadingDates || _selectedDate.Value.Month == _calendarViewInfo.DisplayDate.Month) &&
					(!isInVisibleDates || _calendarViewInfo.EnablePastDates))
				{
					isTemplateView = isInVisibleDates;
				}

				if (!isTemplateView)
				{
					// To handle the selection cell template visibility
					HideSelectionCellTemplateView();
				}
			}

			InvalidateDrawable();
#if MACCATALYST || (!ANDROID && !IOS)
            _hoverView.InvalidateDrawable();
#endif
        }

        /// <summary>
        /// Method to update template views.
        /// </summary>
        /// <param name="isCurrentView">Defines the view is current visible view.</param>
        internal void UpdateTemplateViews(bool isCurrentView)
        {
            if (_calendarViewInfo.MonthView.CellTemplate == null)
            {
                DrawingOrder = DrawingOrder.BelowContent;
                //// If the previous month view is not CellTemplate then we just need to update the view.
                if (_monthCells == null)
                {
                    InvalidateView();
                    return;
                }

                //// If the previous month view is CellTemplate then we need to remove the month cells from the view and update the view.
                RemoveMonthCellsHandler();
                InvalidateView();
            }
            else
            {
				// It's perform when i change show trailing and leading date dynamically
				if (_calendarViewInfo.SelectionCellTemplate != null && _previousMonthCellTemplateView != null)
				{
					_previousMonthCellTemplateView = null;
				}

				DrawingOrder = DrawingOrder.AboveContent;
                RemoveMonthCellsHandler();
                _monthCells = new List<View>();
                GenerateMonthCells(isCurrentView);
            }

            InvalidateViewMeasure();
        }

        /// <summary>
        /// Method to update the swipe range selection.
        /// </summary>
        /// <param name="status">The pan gesture status.</param>
        /// <param name="point">The point.</param>
        internal void HandleSwipeRangeSelection(GestureStatus status, Point point)
        {
            DateTime? panDate = CalendarViewHelper.GetMonthDateFromPosition(point, Width, Height, _calendarViewInfo, _visibleDates);
            if (_calendarViewInfo.SelectionMode == CalendarSelectionMode.MultiRange)
            {
                UpdateSwipeRangeSelection(status, panDate, _selectedDateRanges != null && _selectedDateRanges.Count > 0 ? _selectedDateRanges[_selectedDateRanges.Count - 1] : null);
                return;
            }

            switch (_calendarViewInfo.RangeSelectionDirection)
            {
                case CalendarRangeSelectionDirection.Default:
                    UpdateSwipeRangeSelection(status, panDate, _selectedRange);
                    break;
                case CalendarRangeSelectionDirection.Forward:
                case CalendarRangeSelectionDirection.Backward:
                case CalendarRangeSelectionDirection.Both:
                case CalendarRangeSelectionDirection.None:
                    _calendarViewInfo.UpdateSelectedDate(panDate);
                    break;
            }
        }

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

        /// <summary>
        /// Method to update the selection in key navigation.
        /// </summary>
        /// <param name="args">The key args.</param>
        /// <param name="oldSelectedDate">The old selected date.</param>
        internal void UpdateSelectionWhileKeyNavigation(KeyEventArgs args, DateTime oldSelectedDate)
        {
            DateTime? newDate;
            DateTime? oldSelectedDates = oldSelectedDate;
            DateTime? startDate = _calendarViewInfo.SelectedDateRange?.StartDate?.Date;

            if (_calendarViewInfo.SelectionMode == CalendarSelectionMode.Multiple)
            {
                if (_previousSelectedDate == null)
                {
                    oldSelectedDates = oldSelectedDate;
                }
                else
                {
                    oldSelectedDates = _previousSelectedDate.Value.Date;
                    _previousSelectedDate = null;
                }
            }
            else if (_calendarViewInfo.SelectionMode == CalendarSelectionMode.Range)
            {
                if (_calendarViewInfo.SelectedDateRange == null || startDate == null)
                {
                    return;
                }

                if (_previousSelectedRangeDate != null && _previousSelectedRangeDate.Value.Date == startDate.Value.Date)
                {
                    oldSelectedDates = startDate.Value.Date;
                }
            }

            newDate = CalendarViewHelper.GeKeyNavigationDate(_calendarViewInfo, args, (DateTime)oldSelectedDates);
            if (_calendarViewInfo.SelectionMode == CalendarSelectionMode.Multiple && newDate != null && CalendarViewHelper.IsDateInDateCollection(newDate.Value, _calendarViewInfo.SelectedDates))
            {
                _previousSelectedDate = newDate.Value;
                return;
            }

            CalendarViewHelper.ValidateDateOnKeyNavigation(args, oldSelectedDates, newDate, _calendarViewInfo, _visibleDates, _disabledDates);
        }

        /// <summary>
        /// Method to update the key navigation while navigating to different views.
        /// </summary>
        /// <param name="args">The event args.</param>
        /// <param name="oldSelectedDate">The old selected date.</param>
        /// <param name="newDate">The new selected date.</param>
        internal void UpdateKeyNavigation(KeyEventArgs args, DateTime oldSelectedDate, DateTime newDate)
        {
            CalendarViewHelper.ValidateDateOnKeyNavigation(args, oldSelectedDate, newDate, _calendarViewInfo, _visibleDates, _disabledDates);
        }

        /// <summary>
        /// Method to update the focus while view is changed on keyboard interaction.
        /// </summary>
        internal void SetFocusOnViewChanged()
        {
            Focus();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Method to find the range is present in current view or not.
        /// </summary>
        /// <returns>It returns whether the range is present in current view or not.</returns>
        static bool IsRangeInBetweenRange(DateTime? startRange, DateTime? endRange, DateTime firstDate, DateTime lastDate)
        {
            DateTime? startDate = startRange?.Date;
            DateTime? endDate = endRange?.Date;

            // Example: Assume startDate= Oct-10, endDate= Oct-16, firstDateTime= Sept-25,lastDateTime= Nov 5.
            // To find whether the selected range start date present in the current view or not.
            //  Oct-10 >= Sept-25 && Oct-10 <= Nov-5 condition true.
            if (CalendarViewHelper.IsDateWithinDateRange(startDate, firstDate, lastDate))
            {
                return true;
            }

            // To check whether selected range end date present int the current view or not.
            // Oct-16 >= Sept-25 && Oct-16 <= Nov-5 condition true.
            else if (CalendarViewHelper.IsDateWithinDateRange(endDate, firstDate, lastDate))
            {
                return true;
            }

            // To check whether the start and end range both are not present in the current view then the current view fully with in the range.
            // Example: Assume startDate= Jan-1, endDate= Dec-1, firstDateTime= Sept-25,lastDateTime= Nov 5.
            // Jan-1 <= Sept-25 && Dec-1 >= Nov-5 condition true.
            else if (startDate <= firstDate && endDate >= lastDate)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Method to draw the icon based on the calendar icon details.
        /// </summary>
        /// <param name="canvas">The canvas.</param>
        /// <param name="bounds">The rectangle cell size.</param>
        /// <param name="iconSize">The icon size.</param>
        /// <param name="calendarSpecialDayIconDetails">The calendar special day icon details.</param>
        static void DrawIcon(ICanvas canvas, RectF bounds, float iconSize, CalendarIconDetails calendarSpecialDayIconDetails)
        {
            //// The rectXPosition = xPosition(0) + (monthCellWidth(100) - iconSize(9)) / 2 = 95.5(rectXPosition).
            float rectXPosition = bounds.Left + ((bounds.Width - iconSize) / 2);
            //// The rectYPosition = month cell bounds bottom(90) - iconSize(9)) - 5 = 85.5(rectYPosition).
            //// 5 denotes the bottom padding between the icon and month cell.
            float bottomPadding = 5;
#if WINDOWS || MACCATALYST
            bottomPadding = 10;
#endif
            float rectYPosition = bounds.Bottom - iconSize - bottomPadding;
            //// The iconRect = new RectF(95.5, 85.5, 9, 9).
            RectF iconRect = new RectF(rectXPosition, rectYPosition, iconSize, iconSize);
            canvas.FillColor = calendarSpecialDayIconDetails.Fill.ToColor();
            switch (calendarSpecialDayIconDetails.Icon)
            {
                case CalendarIcon.Dot:
                    //// From example the rectCenter = new PointF(100, 90).
                    canvas.FillCircle(iconRect.Center, iconSize / 2);
                    break;
                case CalendarIcon.Square:
                    //// From example the square = new RectF(95.5, 85.5, 9, 9).
                    canvas.FillRectangle(iconRect);
                    break;
                case CalendarIcon.Triangle:
                    //// From example the triangle = new RectF(95.5, 85.5, 9, 9).
                    canvas.DrawTriangle(iconRect, false);
                    break;

                case CalendarIcon.Heart:
                    PathF heartPath = CreateHeartPath(iconRect);
                    canvas.FillPath(heartPath);
                    break;
                case CalendarIcon.Diamond:
                    //// From example the rhombus = new RectF(95.5, 85.5, 9, 9).
                    canvas.DrawDiamond(iconRect, false);
                    break;
                case CalendarIcon.Star:
                    PathF starPath = GetStarShapePath(iconRect);
                    canvas.FillPath(starPath);
                    break;
                case CalendarIcon.Bell:
                    PathF bellPath = GetBellPath(iconRect);
                    canvas.FillPath(bellPath);
                    break;
            }
        }

        /// <summary>
        /// Method to get the bell path based on the rectangle.
        /// </summary>
        /// <param name="rect">The rectangle.</param>
        /// <returns>The bell shape path.</returns>
        static PathF GetBellPath(RectF rect)
        {
            PathF bellPath = new PathF();
            bellPath.MoveTo(rect.X + (rect.Width * .5f), rect.Y);
            bellPath.CurveTo(rect.X + (rect.Width * .5f), rect.Y, rect.X + (rect.Width * .45f), rect.Y + (rect.Height * .1f), rect.X + (rect.Width * .4f), rect.Y + (rect.Height * .2f));
            bellPath.CurveTo(rect.X + (rect.Width * .4f), rect.Y + (rect.Height * .2f), rect.X + (rect.Width * .25f), rect.Y + (rect.Height * .3f), rect.X + (rect.Width * .2f), rect.Y + (rect.Height * .4f));
            bellPath.CurveTo(rect.X + (rect.Width * .2f), rect.Y + (rect.Height * .4f), rect.X + (rect.Width * .2f), rect.Y + (rect.Height * .6f), rect.X + (rect.Width * .2f), rect.Y + (rect.Height * .75f));
            bellPath.CurveTo(rect.X + (rect.Width * .2f), rect.Y + (rect.Height * .75f), rect.X + (rect.Width * .1f), rect.Y + (rect.Height * .85f), rect.X, rect.Y + rect.Height);
            bellPath.CurveTo(rect.X, rect.Y + rect.Height, rect.X + (rect.Width * .5f), rect.Y + rect.Height, rect.X + rect.Width, rect.Y + rect.Height);
            bellPath.CurveTo(rect.X + rect.Width, rect.Y + rect.Height, rect.X + (rect.Width * .95f), rect.Y + (rect.Height * .85f), rect.X + (rect.Width * .8f), rect.Y + (rect.Height * .75f));
            bellPath.CurveTo(rect.X + (rect.Width * .8f), rect.Y + (rect.Height * .75f), rect.X + (rect.Width * .8f), rect.Y + (rect.Height * .6f), rect.X + (rect.Width * .8f), rect.Y + (rect.Height * .4f));
            bellPath.CurveTo(rect.X + (rect.Width * .8f), rect.Y + (rect.Height * .4f), rect.X + (rect.Width * .75f), rect.Y + (rect.Height * .25f), rect.X + (rect.Width * .6f), rect.Y + (rect.Height * .2f));
            bellPath.CurveTo(rect.X + (rect.Width * .6f), rect.Y + (rect.Height * .2f), rect.X + (rect.Width * .55f), rect.Y + (rect.Height * .1f), rect.X + (rect.Width * .5f), rect.Y);
            bellPath.MoveTo(rect.X + (rect.Width * .5f), rect.Y + (rect.Height * 1.1f));
            bellPath.CurveTo(rect.X + (rect.Width * .25f), rect.Y + (rect.Height * 1.1f), rect.X + (rect.Width * .5f), rect.Y + (rect.Height * 1.3f), rect.X + (rect.Width * .75f), rect.Y + (rect.Height * 1.1f));
            return bellPath;
        }

        /// <summary>
        /// Method to get the path for the heart shape based on the rectangle size.
        /// </summary>
        /// <param name="rect">The rectangle.</param>
        /// <returns>The heart shape path.</returns>
        static PathF CreateHeartPath(RectF rect)
        {
            PathF heartPath = new PathF();
            heartPath.MoveTo(rect.X + (rect.Width * .5f), rect.Y + (rect.Height * .15f));
            heartPath.CurveTo(rect.X + (rect.Width * 0.60f), rect.Y + (rect.Height * 0.05f), rect.X + (rect.Width * 0.9f), rect.Y + (rect.Height * 0.05f), rect.X + (rect.Width * .95f), rect.Y + (rect.Height * .25f));
            heartPath.CurveTo(rect.X + (rect.Width * 1f), rect.Y + (rect.Height * .45f), rect.X + (rect.Width * .75f), rect.Y + (rect.Height * .70f), rect.X + (rect.Width * .50f), rect.Y + (rect.Height * .95f));
			heartPath.CurveTo(rect.X + (rect.Width * .25f), rect.Y + (rect.Height * .70f), rect.X, rect.Y + (rect.Height *  .45f), rect.X + (rect.Width * .05f), rect.Y + (rect.Height * .25f));
			heartPath.CurveTo(rect.X + (rect.Width * 0.01f), rect.Y + (rect.Height * .05f), rect.X + (rect.Width * .4f), rect.Y + (rect.Height * 0.05f), rect.X + (rect.Width * .50f), rect.Y + (rect.Height * .15f));
            return heartPath;
        }

        /// <summary>
        /// Method to get the path for the star shape based on the rectangle size.
        /// </summary>
        /// <param name="rect">The rectangle.</param>
        /// <returns>The path for star shape.</returns>
        static PathF GetStarShapePath(RectF rect)
        {
            PathF starPath = new PathF();
            starPath.MoveTo(rect.X + (rect.Width * .5f), rect.Y);
            starPath.LineTo(rect.X + (rect.Width * .65f), rect.Y + (rect.Height * .35f));
            starPath.LineTo(rect.X + rect.Width, rect.Y + (rect.Height * .35f));
            starPath.LineTo(rect.X + (rect.Width * .75f), rect.Y + (rect.Height * .6f));
            starPath.LineTo(rect.X + (rect.Width * .85f), rect.Y + rect.Height);
            starPath.LineTo(rect.X + (rect.Width * .5f), rect.Y + (rect.Height * .8f));
            starPath.LineTo(rect.X + (rect.Width * .15f), rect.Y + rect.Height);
            starPath.LineTo(rect.X + (rect.Width * .25f), rect.Y + (rect.Height * .6f));
            starPath.LineTo(rect.X, rect.Y + (rect.Height * .35f));
            starPath.LineTo(rect.X + (rect.Width * .35f), rect.Y + (rect.Height * .35f));
            return starPath;
        }

        /// <summary>
        /// Method to get the selection status for Range and MultiRange selection.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="date">The dateTime.</param>
        /// <returns>Returns the selection status of the range.</returns>
        static SelectedRangeStatus? GetSelectionStatus(DateTime? startDate, DateTime? endDate, DateTime? date)
        {
            // The start date have a value and end date is null or start and end date are equal and date time equal to start date.
            // The following cases UI was same. Example: Range (Sept-10 to null). or Range (Sept-10 to Sept-10).
            if (((startDate != null && endDate == null) || startDate == endDate) && date == startDate)
            {
                return SelectedRangeStatus.SelectedRange;
            }

            // The date is range start date or not.
            else if (date == startDate)
            {
                return SelectedRangeStatus.StartRange;
            }

            // The date is range end range or not.
            else if (date == endDate)
            {
                return SelectedRangeStatus.EndRange;
            }

            // The date is in between range start and end date or not.
            else if (startDate < date && date < endDate)
            {
                return SelectedRangeStatus.InBetweenRange;
            }

            return null;
        }

        /// <summary>
        /// Method to find the multi ranges date is start range or end range or in between range.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <param name="visibleSelectedDateRanges">The current view selected date ranges.</param>
        /// <returns>The rage is startRange or endRange or inBetween range.</returns>
        static SelectedRangeStatus? GetMultiRangeSelectionStatus(DateTime? dateTime, ObservableCollection<CalendarDateRange>? visibleSelectedDateRanges)
        {
            if (visibleSelectedDateRanges == null || visibleSelectedDateRanges.Count == 0)
            {
                return null;
            }

            DateTime? date = dateTime?.Date;
            foreach (CalendarDateRange selectedRange in visibleSelectedDateRanges)
            {
                DateTime? startDate = selectedRange.StartDate?.Date;
                DateTime? endDate = selectedRange.EndDate?.Date;

                SelectedRangeStatus? status = GetSelectionStatus(startDate, endDate, date);
                if (status != null)
                {
                    return status;
                }
            }

            return null;
        }

		/// <summary>
		/// This method is used to remove selection cell template and insert the normal month cell template
		/// </summary>
		/// <param name="month">Get the month details from current canvas</param>
		/// <param name="child">Get the view details from the month</param>
		static void RemoveTemplateView(MonthView month, View child)
		{
			int index = -1;
#if !WINDOWS
            if (month._monthCells != null && month._monthCells.Count > 0)
            {
                for (int i = 0; i < month._monthCells.Count; i++)
                {
                    if (month.Children[i] != month._monthCells[i])
                    {
                        index = i;
                        break;
                    }
                }
            }
#else
			index = month.Children.IndexOf(child);
#endif
			if (index != -1)
			{
				month.Children.RemoveAt(index);
				if (month._previousMonthCellTemplateView != null)
				{
					month.Insert(index, month._previousMonthCellTemplateView);
				}

				month._previousMonthCellTemplateView = null;
				month._selectionCellTemplateView = null;
			}
		}

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
		/// Method to find the range is start range or end range or in between range.
		/// </summary>
		/// <param name="dateTime">The date time.</param>
		/// <returns>The rage is startRange or endRange or inBetween range.</returns>
		SelectedRangeStatus? GetRangeSelectionStatus(DateTime? dateTime)
        {
            if (_selectedRange == null)
            {
                return null;
            }

            DateTime? date = dateTime?.Date;
            DateTime? startDate = _selectedRange.StartDate?.Date;
            DateTime? endDate = _selectedRange.EndDate?.Date;

            return GetSelectionStatus(startDate, endDate, date);
        }

        /// <summary>
        /// Method to check the view can be draw for range changes based on selection direction.
        /// </summary>
        /// <param name="oldSelectedDateRange">Previous selected range start date </param>
        /// <param name="firstDate">The visible start date.</param>
        /// <param name="lastDate">The visible end date.</param>
        /// <returns>Returns true when the view is need to redraw</returns>
        bool IsViewNeedUpdateRangeSelection(CalendarDateRange? oldSelectedDateRange, DateTime firstDate, DateTime lastDate)
        {
            DateTime? newStartDate = _selectedRange?.StartDate?.Date;
            DateTime? newEndDate = _selectedRange?.EndDate == null ? newStartDate : _selectedRange?.EndDate;
            DateTime? oldStartDate = oldSelectedDateRange?.StartDate?.Date;
            DateTime? oldEndDate = oldSelectedDateRange?.EndDate?.Date == null ? oldStartDate : oldSelectedDateRange?.EndDate?.Date;
            if (_calendarViewInfo.SelectionMode == CalendarSelectionMode.Range)
            {
                switch (_calendarViewInfo.RangeSelectionDirection)
                {
                    case CalendarRangeSelectionDirection.Default:
                    case CalendarRangeSelectionDirection.Both:
                        break;
                    case CalendarRangeSelectionDirection.Forward:
                        if (oldStartDate == null && newStartDate > firstDate)
                        {
                            return true;
                        }

                        // The previous range and new range is a valid range(range start and end date are different), So need to draw in between new range, remove previous range and draw new range.
                        // Example: previous range = (Nov-15 to 18) and new range (Sept-18 to Oct-20), So need to draw Oct and Nov month only.
                        // Example: previous range = (Nov-15 to 18) and new range (Dec-18 to 25), So need to draw Nov, Dec month only.
                        if (oldStartDate > newStartDate)
                        {
                            DateTime? dateTime = oldStartDate;
                            oldStartDate = newStartDate;
                            newStartDate = dateTime;
                        }

                        return IsRangeInBetweenRange(oldStartDate, newStartDate, firstDate, lastDate);
                    case CalendarRangeSelectionDirection.Backward:
                        //// If the visible date is greater than the end date need to update the view
                        if (oldEndDate == null && firstDate > newEndDate)
                        {
                            return true;
                        }

                        // The previous range and new range is a valid range(range start and end date are different), So need to draw in between new range, remove previous range and draw new range.
                        // Example: previous range = (Nov-15 to 18) and new range (Sept-18 to Oct-20), So need to draw Oct and Nov month only.
                        // Example: previous range = (Nov-15 to 18) and new range (Dec-18 to 25), So need to draw Nov, Dec month only.
                        if (oldEndDate > newEndDate)
                        {
                            DateTime? dateTime = oldEndDate;
                            oldEndDate = newEndDate;
                            newEndDate = dateTime;
                        }

                        return IsRangeInBetweenRange(oldEndDate, newEndDate, firstDate, lastDate);
                    case CalendarRangeSelectionDirection.None:
                        bool isValidOldRange = oldStartDate != oldEndDate;
                        bool isValidNewRange = newStartDate != newEndDate;

                        // The previous range and new range is not a valid range(range start and end date are different), So need to draw new range and remove previous range.
                        // Example: previous range = (Nov-15 to null) and new range (Nov-18 to Nov-18).
                        if (!isValidOldRange && !isValidNewRange)
                        {
                            return IsRangeInBetweenRange(newStartDate, newEndDate, firstDate, lastDate) || IsRangeInBetweenRange(oldStartDate, oldEndDate, firstDate, lastDate);
                        }

                        // The previous range is not a valid range(range start and end date are different), So need to remove previous range, draw new range and draw disabled date(Before new range start date and after new range end date).
                        // Example: previous range = (Nov-15 to null) and new range (Nov-18 to Nov-20).
                        else if (!isValidOldRange)
                        {
                            return IsRangeInBetweenRange(newStartDate, newEndDate, firstDate, lastDate) || IsRangeInBetweenRange(oldStartDate, oldEndDate, firstDate, lastDate)
                                || newStartDate >= firstDate || newEndDate <= lastDate;
                        }

                        // The new range is not a valid range(range start and end date are different), So need to draw new range, remove previous range and remove disabled date(Before old range start date and after old range end date).
                        // Example: previous range = (Nov-15 to null) and new range (Nov-18 to Nov-20).
                        else if (!isValidNewRange)
                        {
                            return IsRangeInBetweenRange(newStartDate, newEndDate, firstDate, lastDate) || IsRangeInBetweenRange(oldStartDate, oldEndDate, firstDate, lastDate)
                                || oldStartDate >= firstDate || oldEndDate <= lastDate;
                        }

                        // The previous range and new range is a valid range(range start and end date are different), So need to draw in between new range and previous range.
                        // Example: previous range = (Nov-15 to 18) and new range (Nov-18 to Nov-20), So need to draw Nov month only.
                        // Example: previous range = (Nov-15 to 18) and new range (Dec-18 to Dec-20), So need to draw Nov and Dec month only.
                        // Example: previous range = (Nov-15 to 18) and new range (Sept-18 to Sept-20), So need to draw Sept, Oct and Nov month only.
                        else
                        {
                            if (oldStartDate > newStartDate)
                            {
                                DateTime? dateTime = oldStartDate;
                                oldStartDate = newStartDate;
                                newStartDate = dateTime;
                            }

                            if (oldEndDate > newEndDate)
                            {
                                DateTime? dateTime = oldEndDate;
                                oldEndDate = newEndDate;
                                newEndDate = dateTime;
                            }

                            return IsRangeInBetweenRange(oldStartDate, newStartDate, firstDate, lastDate) || IsRangeInBetweenRange(oldEndDate, newEndDate, firstDate, lastDate);
                        }
                }
            }

            return false;
        }

        /// <summary>
        /// Method to update the swipe selection for range selection and multi range selection.
        /// </summary>
        /// <param name="status">The pan gesture status.</param>
        /// <param name="panDate">The pan date.</param>
        /// <param name="selectedRange">The selected date range.</param>
        void UpdateSwipeRangeSelection(GestureStatus status, DateTime? panDate, CalendarDateRange? selectedRange)
        {
            CalendarDateRange newRange;
            if (panDate != null && !CalendarViewHelper.IsInteractableDate(panDate.Value, _disabledDates, _visibleDates, _calendarViewInfo, _numberOfWeeks))
            {
                panDate = null;
            }

            if (panDate == null)
            {
                // The pan gesture status was canceled or completed then update the initial start range date.
                if (status == GestureStatus.Completed || status == GestureStatus.Canceled)
                {
                    _initialStartRangeDate = null;
                }

                return;
            }

            //// The pan gesture started or the initialStartRangeDate value is null then the isNewRange will updated as true.
            //// The isNewRange is used to create a new instance for the selected range while new swipe started.
            bool isNewRange = false;

            // Whenever the pan gesture started then need to create instance for selected range.
            if (status == GestureStatus.Started)
            {
                newRange = new CalendarDateRange(panDate, null);
                _initialStartRangeDate = panDate;
                isNewRange = true;

                // Here true represent the swipe status is started.
                _calendarViewInfo.UpdateSwipeSelection(newRange, isNewRange);
            }
            else if (status == GestureStatus.Running)
            {
                // Initially(pan status started) user interacted with disabled date or blackout date then calendar date range instance did not created. So need to create a instance for calendar date range.
                if (_initialStartRangeDate == null)
                {
                    newRange = new CalendarDateRange(panDate, null);
                    _initialStartRangeDate = panDate;
                    isNewRange = true;
                }
                //// The user interacted with within the start date only(same cell) then need to update the start date only.
                else if (_initialStartRangeDate?.Date == panDate.Value.Date)
                {
                    newRange = new CalendarDateRange(panDate, null);
                    if (selectedRange != null && CalendarViewHelper.IsSameRange(_calendarViewInfo.View, newRange, selectedRange, _calendarViewInfo.Identifier))
                    {
                        return;
                    }
                }
                //// The user interacted with the greater than initial start date of pan gesture started date then need to update the end date.
                else if (_initialStartRangeDate?.Date < panDate.Value.Date)
                {
                    // The most recently updated value of end date equal to current interaction date then no need to update the end date value again.
                    // Because of user interact with the same cell.
                    if (selectedRange?.EndDate?.Date == panDate.Value.Date)
                    {
                        return;
                    }

                    newRange = new CalendarDateRange(_initialStartRangeDate, panDate);
                }
                //// The user interacted with the lesser than initial start date then need to update the start date.
                else
                {
                    // The most recently updated value of end date equal to current interaction date then no need to update the start and end date value again.
                    // Because of user interact with the same cell.
                    if (selectedRange?.StartDate?.Date == panDate.Value.Date)
                    {
                        return;
                    }

                    newRange = new CalendarDateRange(panDate, _initialStartRangeDate);
                }

                _calendarViewInfo.UpdateSwipeSelection(newRange, isNewRange);
            }
            else if (status == GestureStatus.Completed || status == GestureStatus.Canceled)
            {
                _initialStartRangeDate = null;
            }
        }

        /// <summary>
        /// Method to draw the end range selection.
        /// </summary>
        /// <param name="canvas">The draw canvas</param>
        /// <param name="cellRect">The cell rectangle hold the current month cell bound(x position, y position, width, height).</param>
        /// <param name="circleSize">The circleSize height.</param>
        /// <param name="selectionRadius">The selection radius of the circle.</param>
        /// <param name="endRangeSelectionBackground">The end range selection background.</param>
        /// <param name="rangeSelectionBackground">The range selection background.</param>
        /// <param name="rectCornerRadius">The rectangle corner radius.</param>
        /// <param name="circleTopPosition">The starting position of the month cell height.</param>
        /// <param name="centerPosition">The center position of the month cell.</param>
        void DrawEndRangeSelection(ICanvas canvas, RectF cellRect, float circleSize, float selectionRadius, Color endRangeSelectionBackground, Color rangeSelectionBackground, float rectCornerRadius, float circleTopPosition, PointF centerPosition)
        {
            if (_calendarViewInfo.SelectionShape == CalendarSelectionShape.Rectangle)
            {
                RectF rectF = new RectF(cellRect.Left, cellRect.Top + HighlightPadding, cellRect.Width - HighlightPadding, cellRect.Height - (2 * HighlightPadding));
                canvas.FillColor = endRangeSelectionBackground;
                //// The cellRect denotes xPosition, yPosition, width, height.
                //// The 0 denotes topLeftCornerRadius of the rectangle.
                //// The rectCornerRadius denotes topRightCornerRadius of the rectangle.
                //// The 0 denotes bottomLeftCornerRadius of the rectangle.
                //// The rectCornerRadius denotes bottomRightCornerRadius of the rectangle.
                canvas.FillRoundedRectangle(rectF, 0, rectCornerRadius, 0, rectCornerRadius);
            }
            else
            {
                canvas.FillColor = rangeSelectionBackground;
                canvas.FillRectangle(cellRect.Left, circleTopPosition, cellRect.Width / 2, circleSize);
                canvas.FillColor = endRangeSelectionBackground;
                canvas.FillCircle(centerPosition, selectionRadius);
            }
        }

        /// <summary>
        /// Method to draw the start range selection.
        /// </summary>
        /// <param name="canvas">The draw canvas</param>
        /// <param name="cellRect">The cell rectangle hold the current month cell bound(x position, y position, width, height).</param>
        /// <param name="circleSize">The circleSize height.</param>
        /// <param name="selectionRadius">The selection radius of the circle.</param>
        /// <param name="starRangeSelectionBackground">The start range selection background.</param>
        /// <param name="rangeSelectionBackground">The range selection background.</param>
        /// <param name="rectCornerRadius">The rectangle corner radius.</param>
        /// <param name="circleTopPosition">The starting position of the month cell height.</param>
        /// <param name="centerPosition">The center position of the month cell.</param>
        void DrawStartRangeSelection(ICanvas canvas, RectF cellRect, float circleSize, float selectionRadius, Color starRangeSelectionBackground, Color rangeSelectionBackground, float rectCornerRadius, float circleTopPosition, PointF centerPosition)
        {
            if (_calendarViewInfo.SelectionShape == CalendarSelectionShape.Rectangle)
            {
                // The 2 is used to give a padding for the top of the rectangle.
                // The 4 is used to give a padding for the bottom of the rectangle.
                RectF rectF = new RectF(cellRect.Left + HighlightPadding, cellRect.Top + HighlightPadding, cellRect.Width - HighlightPadding, cellRect.Height - (2 * HighlightPadding));
                canvas.FillColor = starRangeSelectionBackground;
                //// The cellRect denotes xPosition, yPosition, width, height.
                //// The rectCornerRadius denotes top left corner radius of the rectangle.
                //// The 0 denotes top right corner radius of the rectangle.
                //// The rectCornerRadius denotes bottom left corner radius of the rectangle.
                //// The 0 denotes bottom right corner radius of the rectangle.
                canvas.FillRoundedRectangle(rectF, rectCornerRadius, 0, rectCornerRadius, 0);
            }
            else
            {
                canvas.FillColor = rangeSelectionBackground;
                canvas.FillRectangle(centerPosition.X, circleTopPosition, cellRect.Width / 2, circleSize);
                canvas.FillColor = starRangeSelectionBackground;
                canvas.FillCircle(centerPosition, selectionRadius);
            }
        }

        /// <summary>
        /// Method to draw the range selection based on the range start and end date.
        /// </summary>
        /// <param name="canvas">The draw canvas</param>
        /// <param name="highlightBounds">The cell rectangle hold the current month cell bound(x position, y position, width, height).</param>
        /// <param name="rangeSelectionStatus">The range is start range or in between range or end range.</param>
        /// <param name="rangeSelectionBackground">The range selection background.</param>
        /// <param name="startRangeSelectionBackground">The start date background.</param>
        /// <param name="endRangeSelectionBackground">The end date background.</param>
        /// <param name="cornerRadius">The corner radius.</param>
        /// <param name="isRTL">The RTL true or false.</param>
        /// <param name="selectionRadius">The selection radius of the circle.</param>
        /// <param name="circleSize">The size of the circle.</param>
        /// <param name="circleTopPosition">The starting position of the month cell height.</param>
        /// <param name="centerPosition">The center position of the month cell.</param>
        /// <param name="columnIndex">The column index of the month cell.</param>
        CalendarTextStyle DrawRangeSelection(ICanvas canvas, RectF highlightBounds, SelectedRangeStatus? rangeSelectionStatus, Color rangeSelectionBackground, Color startRangeSelectionBackground, Color endRangeSelectionBackground, float cornerRadius, bool isRTL, float selectionRadius, float circleSize, float circleTopPosition, PointF centerPosition, int columnIndex)
        {
            CalendarTextStyle calendarTextStyle;
            if (rangeSelectionStatus == SelectedRangeStatus.SelectedRange)
            {
                calendarTextStyle = _calendarViewInfo.MonthView.SelectionTextStyle;
                DrawSelectionShape(canvas, highlightBounds, selectionRadius, cornerRadius, startRangeSelectionBackground, centerPosition);
            }

            // The date is start date then need to draw the start range selection.
            else if (rangeSelectionStatus == SelectedRangeStatus.StartRange)
            {
                calendarTextStyle = _calendarViewInfo.MonthView.SelectionTextStyle;
                if (isRTL)
                {
                    DrawEndRangeSelection(canvas, highlightBounds, circleSize, selectionRadius, startRangeSelectionBackground, rangeSelectionBackground, cornerRadius, circleTopPosition, centerPosition);
                }
                else
                {
                    DrawStartRangeSelection(canvas, highlightBounds, circleSize, selectionRadius, startRangeSelectionBackground, rangeSelectionBackground, cornerRadius, circleTopPosition, centerPosition);
                }
            }

            // The date is in between the range start and end date then need to fill the in between the cells with the range selection background.
            else if (rangeSelectionStatus == SelectedRangeStatus.InBetweenRange)
            {
                calendarTextStyle = _calendarViewInfo.MonthView.RangeTextStyle;
                canvas.FillColor = rangeSelectionBackground;
                float height = circleSize;
                float topPosition = circleTopPosition;
                if (_calendarViewInfo.SelectionShape == CalendarSelectionShape.Rectangle)
                {
                    topPosition = circleTopPosition + HighlightPadding;
                    height = highlightBounds.Height - (2 * HighlightPadding);
                }

                //// Handle the padding for drawing at right side end of selection,
                //// because range selection UI from current view to next view shown like
                //// a continuous one while swipe the view.
                float highlightWidth = columnIndex == 6 ? highlightBounds.Width - 1 : highlightBounds.Width;
                canvas.FillRectangle(highlightBounds.Left, topPosition, highlightWidth, height);
            }

            // The date is end range then need to draw end range selection.
            else
            {
                calendarTextStyle = _calendarViewInfo.MonthView.SelectionTextStyle;
                if (isRTL)
                {
                    DrawStartRangeSelection(canvas, highlightBounds, circleSize, selectionRadius, endRangeSelectionBackground, rangeSelectionBackground, cornerRadius, circleTopPosition, centerPosition);
                }
                else
                {
                    DrawEndRangeSelection(canvas, highlightBounds, circleSize, selectionRadius, endRangeSelectionBackground, rangeSelectionBackground, cornerRadius, circleTopPosition, centerPosition);
                }
            }

            return calendarTextStyle;
        }

        /// <summary>
        /// Method to draw the month cells.
        /// </summary>
        /// <param name="canvas">The draw canvas.</param>
        /// <param name="isRTL">Is RTL layout or not.</param>
        /// <param name="weekNumberWidth">The week number width.</param>
        /// <param name="monthViewWidth">The month view width.</param>
        /// <param name="monthCellWidth">The month cell width.</param>
        /// <param name="currentMonth">The current month index.</param>
        /// <param name="monthCellHeight">The month cell height.</param>
        void DrawMonthCells(ICanvas canvas, bool isRTL, float weekNumberWidth, float monthViewWidth, float monthCellWidth, DateTime currentMonth, float monthCellHeight)
        {
            //// xPosition is used to define the starting position for drawing the cells.
            //// Example: Let total width of the month view = 70 and month cell width = 10.
            //// If the flow direction is RTL then it should draw the cells from the right side of the view.
            //// xPosition = 70 - 10 = 60.
            //// Else the cells drawn from the left side of the view.
            float xPosition = isRTL ? monthViewWidth - monthCellWidth : weekNumberWidth;
            float yPosition = 0;
            Color fillColor = Colors.Transparent;
            CalendarMonthView monthViewSettings = _calendarViewInfo.MonthView;
            Color cellBackground = monthViewSettings.Background.ToColor();
            Color trailingLeadingDateBackground = monthViewSettings.TrailingLeadingDatesBackground.ToColor();
            Color? weekendsBackground = monthViewSettings.WeekendDatesBackground?.ToColor() ?? null;
            Color todayBackground = monthViewSettings.TodayBackground.ToColor();
            Color disabledDatesBackground = monthViewSettings.DisabledDatesBackground.ToColor();
            Color specialDatesBackground = monthViewSettings.SpecialDatesBackground.ToColor();
            Color todayHighlightColor = _calendarViewInfo.TodayHighlightBrush.ToColor();
            Color startRangeSelectionBackground = _calendarViewInfo.StartRangeSelectionBackground.ToColor();
            Color selectedDateBackground = CalendarViewHelper.GetSelectionBackground(_calendarViewInfo.SelectionBackground, _calendarViewInfo.SelectionMode).ToColor();
            if (_calendarViewInfo.SelectionMode == CalendarSelectionMode.Range || _calendarViewInfo.SelectionMode == CalendarSelectionMode.MultiRange)
            {
                if (_calendarViewInfo.SelectionBackground != null && _calendarViewInfo.SelectedRangeColor != null)
                {
                    selectedDateBackground = CalendarViewHelper.GetSelectionBackground(_calendarViewInfo.SelectedRangeColor, _calendarViewInfo.SelectionMode).ToColor();
                }
            }

            Color endRangeSelectionBackground = _calendarViewInfo.EndRangeSelectionBackground.ToColor();
            Color rangeSelectionBackground = selectedDateBackground;
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
            bool isMonthView = _numberOfWeeks == 6;
            DateTime todayDate = DateTime.Now.Date;
            float cornerRadius = (float)(monthCellHeight * 0.1);
            cornerRadius = cornerRadius > 2 ? 2 : cornerRadius;
            //// Calculate the maximum size based on the month cell height and width.
            float maximumSquareSize = monthCellHeight < monthCellWidth ? monthCellHeight : monthCellWidth;
            //// The HighlightPadding is used for the space around the circle from the cell.
            float selectionRadius = (maximumSquareSize / 2) - HighlightPadding;

            // The circle size. Example: radius = 25.
            // The circleSize = 2 * 25 = 50(circleSize).
            float circleSize = 2 * selectionRadius;

            // This is the valid start index for to fetch the visible date from the visible date collection.
            int startIndex = 0;
            //// The number of dates in the collection.
            int visibleDateCount = _visibleDates.Count;

            ObservableCollection<CalendarDateRange>? visibleSelectedDateRanges = null;
            //// Get the current view selected date ranges.
            if (_calendarViewInfo.SelectionMode == CalendarSelectionMode.MultiRange && _selectedDateRanges != null)
            {
                visibleSelectedDateRanges = GetVisibleSelectedDateRanges(_selectedDateRanges);
            }

            // Get the culture info based on the calendar identifier.
            CultureInfo cultureInfo = CalendarViewHelper.GetCurrentUICultureInfo(_calendarViewInfo.Identifier);
            bool isGregorianCalendar = CalendarViewHelper.IsGregorianCalendar(_calendarViewInfo.Identifier);
            Globalization.Calendar cultureCalendar = CalendarViewHelper.GetCalendar(_calendarViewInfo.Identifier.ToString());

            // The visible dates count not a multiple value of 7 and number of weeks then need to render the date based on the first day of week basis.
            // Example: Assume display date is 0001,01,01 first day of week is Tuesday(Enumeration Value = 2) and number of weeks is 2.
            // From above scenario the visible dates contains from 0001,1,1 to 0001,1,8.
            // The visible dates[0] = (0001,1,1) and first date of day of week = Monday.
            // Here condition is true while visible date count is 8. Calculation 8 % 7 != 0.
            if (visibleDateCount % DaysPerWeek != 0)
            {
                DateTime currentDate = _visibleDates.First();
                startIndex = CalendarViewHelper.GetFirstDayOfWeekDifference(currentDate, _calendarViewInfo.MonthView.FirstDayOfWeek, _calendarViewInfo.Identifier);

                // From example the 0001,1,1 day of week is Monday. So need to render the 1st date in index value 5. Because of the first date of day week text is rendered in index value 5.
                // The 0th index to 4th-index need to render empty. So draw will start from 5th-index. So need to change the xPosition value based on the valid start index.
                xPosition += startIndex * cellWidthOffset;
            }

            for (int i = 0; i < visibleDateCount; i++)
            {
                //// From example the start index value 6.
                int index = startIndex + i;
                DateTime dateTime = _visibleDates[i];
                int column = index % DaysPerWeek;
                //// If date arrangement reached to last column then need to start from next row and first column.
                if (column == 0 && index != 0)
                {
                    xPosition = isRTL ? monthViewWidth - monthCellWidth : weekNumberWidth;
                    yPosition += monthCellHeight;
                }

                bool isLeadingAndTrailingDates = isMonthView && CalendarViewHelper.IsLeadingAndTrailingDate(dateTime, currentMonth, _calendarViewInfo.View, _calendarViewInfo.Identifier);
                //// If ShowLeadingAndTrailingDates set to false no need to draw leading and trailing view.
                //// But when number of weeks is less than 6, leading and trailing view will draw even ShowLeadingAndTrailingDates set to false.
                if (isLeadingAndTrailingDates && !_calendarViewInfo.ShowTrailingAndLeadingDates)
                {
                    xPosition += cellWidthOffset;
                    continue;
                }

                bool isBlackoutDate = CalendarViewHelper.IsDateInDateCollection(dateTime, _disabledDates);
                bool isDisabledDate = CalendarViewHelper.IsDisabledDate(dateTime, _calendarViewInfo.View, _calendarViewInfo.EnablePastDates, _calendarViewInfo.MinimumDate, _calendarViewInfo.MaximumDate, _calendarViewInfo.SelectionMode, _calendarViewInfo.RangeSelectionDirection, _selectedRange, _calendarViewInfo.AllowViewNavigation, _calendarViewInfo.Identifier);

                // The current date is today date and not a range then need to considered the today text style.
                bool isTodayDate = todayDate.Date.Equals(dateTime.Date);
                 //// Stores the special dates icon details for drawing.
                CalendarIconDetails? calendarSpecialDayIconDetails = _specialDates.FirstOrDefault(details => CalendarViewHelper.IsSameDate(_calendarViewInfo.View, details.Date, dateTime, _calendarViewInfo.Identifier));
                CalendarTextStyle textStyle = GetMonthCellStyle(dateTime, isTodayDate, isLeadingAndTrailingDates, isBlackoutDate, isDisabledDate, _calendarViewInfo.ShowOutOfRangeDates, calendarSpecialDayIconDetails != null, ref fillColor, cellBackground, trailingLeadingDateBackground, weekendsBackground, todayBackground, disabledDatesBackground, specialDatesBackground, cultureCalendar);
                //// If background color is not transparent then the background color for month cell is applied.
                if (fillColor != Colors.Transparent)
                {
                    canvas.FillColor = fillColor;
                    canvas.FillRectangle(new Rect(xPosition, yPosition, monthCellWidth, monthCellHeight));
                }

                bool isSelectedDate = false;

                // The actual cell bounds. Based on this cell rectangle the selection highlight, selection range, extendable range hovering today highlight will draw.
                RectF highlightBounds = new Rect(xPosition, yPosition, monthCellWidth, monthCellHeight);

                // The center position contains centerXPosition and centerYPosition of the month cell.
                PointF centerPosition = highlightBounds.Center;

                // Assume the centerPosition.Y = 150. selectionRadius = 25.
                // The circleTopPosition = 150 - 25 = 125(circleTopPosition). The draw will start from the y axis with position value of 125.
                float circleTopPosition = _calendarViewInfo.SelectionShape == CalendarSelectionShape.Rectangle ? highlightBounds.Top : centerPosition.Y - selectionRadius;

                // If it is selected date then the selection is drawn based on the selection shape.
                switch (_calendarViewInfo.SelectionMode)
                {
                    case CalendarSelectionMode.Single:
                    case CalendarSelectionMode.Multiple:
                        isSelectedDate = IsSelectedDate(isBlackoutDate, isDisabledDate, dateTime);
                        //// If it is selected date then the selection is drawn based on the selection shape.
                        if (isSelectedDate)
                        {
							bool useCustomTemplate = _calendarViewInfo.SelectionCellTemplate == null;
							textStyle = useCustomTemplate ? monthViewSettings.SelectionTextStyle : (_calendarViewInfo.SelectionMode == CalendarSelectionMode.Single ? new CalendarTextStyle { TextColor = Colors.Transparent } : monthViewSettings.SelectionTextStyle);
							DrawSelectionShape(canvas, highlightBounds, selectionRadius, cornerRadius, selectedDateBackground, centerPosition);
                        }

                        break;

                    case CalendarSelectionMode.Range:
                        if (!isDisabledDate && !isBlackoutDate)
                        {
                            // To find the range selection is start range or end range or in between range or not.
                            SelectedRangeStatus? rangeSelectionStatus = GetRangeSelectionStatus(dateTime);
                            isSelectedDate = rangeSelectionStatus != null;
                            if (isSelectedDate)
                            {
                                textStyle = DrawRangeSelection(canvas, highlightBounds, rangeSelectionStatus, rangeSelectionBackground, startRangeSelectionBackground, endRangeSelectionBackground, cornerRadius, isRTL, selectionRadius, circleSize, circleTopPosition, centerPosition, column);
                            }
                        }

                        break;

                    case CalendarSelectionMode.MultiRange:
                        if (!isDisabledDate && !isBlackoutDate && visibleSelectedDateRanges != null)
                        {
                            SelectedRangeStatus? selectedRangeStatus = GetMultiRangeSelectionStatus(dateTime, visibleSelectedDateRanges);
                            isSelectedDate = selectedRangeStatus != null;
                            if (isSelectedDate)
                            {
                                textStyle = DrawRangeSelection(canvas, highlightBounds, selectedRangeStatus, rangeSelectionBackground, startRangeSelectionBackground, endRangeSelectionBackground, cornerRadius, isRTL, selectionRadius, circleSize, circleTopPosition, centerPosition, column);
                            }
                        }

                        break;
                }

                //// Today Highlight draw based on selection shape. The date is not in range selection(It means selection status was empty), then only draw today highlight and today date properties.
                if (isTodayDate && !isSelectedDate)
                {
                    DrawTodayHighlight(canvas, xPosition, yPosition, monthCellWidth, monthCellHeight, cornerRadius, selectionRadius, todayHighlightColor);
                }

                //// If we use format "d" then it return full date string like "1/1/2023". But we need to draw only day value like "1", so we specify the date time day value for Gregorian calendar type.
                string dateText = isGregorianCalendar ? dateTime.Day.ToString() : dateTime.ToString("dd", cultureInfo);
                CalendarViewHelper.DrawText(canvas, dateText, textStyle, new RectF(xPosition, yPosition, monthCellWidth, monthCellHeight), HorizontalAlignment.Center, VerticalAlignment.Center);

                //// No need to draw the special day icon while the date is selected date, dates and range.
                if (calendarSpecialDayIconDetails != null && !isSelectedDate)
                {
                    float iconSize = maximumSquareSize * 0.15f;
                    //// Dot and square shapes are bigger than the other shapes, so the size reduced for this icons.
                    if (calendarSpecialDayIconDetails.Icon == CalendarIcon.Dot || calendarSpecialDayIconDetails.Icon == CalendarIcon.Square)
                    {
                        iconSize = maximumSquareSize * 0.12f;
                    }
                    //// Triangle shape and bell shape is bigger than the other shapes, so the size reduced for this icon.
                    else if (calendarSpecialDayIconDetails.Icon == CalendarIcon.Triangle || calendarSpecialDayIconDetails.Icon == CalendarIcon.Bell)
                    {
                        iconSize = maximumSquareSize * 0.13f;
                    }

                    DrawIcon(canvas, highlightBounds, iconSize, calendarSpecialDayIconDetails);
                }

                // Condition to draw horizontal line at the center of the blackout date based on text width.
                if (isBlackoutDate)
                {
                    float textWidth = (float)dateText.Measure(textStyle).Width;
                    float textWidthCenter = textWidth / 2;
                    float widthCenter = monthCellWidth / 2;
                    float heightCenter = monthCellHeight / 2;
                    canvas.StrokeColor = textStyle.TextColor;
                    canvas.DrawLine(xPosition + (widthCenter - textWidthCenter), yPosition + heightCenter, xPosition + (widthCenter + textWidthCenter), yPosition + heightCenter);
                }

                xPosition += cellWidthOffset;
            }
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
            DateTime firstVisibleDate = _visibleDates[0];
            DateTime lastVisibleDate = _visibleDates[_visibleDates.Count - 1];
            foreach (CalendarDateRange range in dateRangeCollection)
            {
                DateTime? startDate = range.StartDate?.Date;
                DateTime? endDate = range.EndDate?.Date;

                // Check the start date and end date range is in between the current view visible dates collection.
                if (CalendarViewHelper.IsDateWithinDateRange(startDate, firstVisibleDate, lastVisibleDate) || CalendarViewHelper.IsDateWithinDateRange(endDate, firstVisibleDate, lastVisibleDate) || CalendarViewHelper.IsDateWithinDateRange(firstVisibleDate, startDate, endDate) || CalendarViewHelper.IsDateWithinDateRange(lastVisibleDate, startDate, endDate))
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
        /// Method to draw the selection for custom views.
        /// </summary>
        /// <param name="canvas">The draw canvas.</param>
        /// <param name="isRTL">Is RTL layout or not.</param>
        /// <param name="weekNumberWidth">The week number width.</param>
        /// <param name="monthViewWidth">The month view width.</param>
        /// <param name="monthCellWidth">The month cell width.</param>
        /// <param name="currentMonth">The current month index.</param>
        /// <param name="monthCellHeight">The month cell height.</param>
        void DrawTemplateSelection(ICanvas canvas, bool isRTL, float weekNumberWidth, float monthViewWidth, float monthCellWidth, DateTime currentMonth, float monthCellHeight)
        {
            //// xPosition is used to define the starting position for drawing the cells.
            //// Example: Let total width of the month view = 70 and month cell width = 10.
            //// If the flow direction is RTL then it should draw the cells from the right side of the view.
            //// xPosition = 70 - 10 = 60.
            //// Else the cells drawn from the left side of the view.
            float xPosition = isRTL ? monthViewWidth - monthCellWidth : weekNumberWidth;
            float yPosition = 0;
            Color startRangeSelectionBackground = _calendarViewInfo.StartRangeSelectionBackground.ToColor();
            Color selectedDateBackground = CalendarViewHelper.GetSelectionBackground(_calendarViewInfo.SelectionBackground, _calendarViewInfo.SelectionMode).ToColor();
            if (_calendarViewInfo.SelectionMode == CalendarSelectionMode.Range || _calendarViewInfo.SelectionMode == CalendarSelectionMode.MultiRange)
            {
                if (_calendarViewInfo.SelectionBackground != null && _calendarViewInfo.SelectedRangeColor != null)
                {
                    selectedDateBackground = CalendarViewHelper.GetSelectionBackground(_calendarViewInfo.SelectedRangeColor, _calendarViewInfo.SelectionMode).ToColor();
                }
            }

            Color endRangeSelectionBackground = _calendarViewInfo.EndRangeSelectionBackground.ToColor();
            Color rangeSelectionBackground = selectedDateBackground;

            ObservableCollection<CalendarDateRange>? visibleSelectedDateRanges = null;
            //// Get the current view selected date ranges.
            if (_calendarViewInfo.SelectionMode == CalendarSelectionMode.MultiRange && _selectedDateRanges != null)
            {
                visibleSelectedDateRanges = GetVisibleSelectedDateRanges(_selectedDateRanges);
            }

            //// If the selection and range selection backgrounds are set to transparent then no need to draw the selection on the cell template.
            if (_calendarViewInfo.SelectionMode != CalendarSelectionMode.Range && _calendarViewInfo.SelectionMode != CalendarSelectionMode.MultiRange)
            {
                if (selectedDateBackground == Colors.Transparent)
                {
                    return;
                }
            }
            else
            {
                if (startRangeSelectionBackground == Colors.Transparent && endRangeSelectionBackground == Colors.Transparent && rangeSelectionBackground == Colors.Transparent)
                {
                    return;
                }
            }

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
            bool isMonthView = _numberOfWeeks == 6;
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
            // Example: Assume display date is 0001,01,01 first day of week is Tuesday(Enumeration Value = 2) and number of weeks is 2.
            // From above scenario the visible dates contains from 0001,1,1 to 0001,1,8.
            // The visible dates[0] = (0001,1,1) and first date of day of week = Monday.
            // Here condition is true while visible date count is 8. Calculation 8 % 7 != 0.
            if (visibleDateCount % DaysPerWeek != 0)
            {
                DateTime currentDate = _visibleDates.First();
                startIndex = CalendarViewHelper.GetFirstDayOfWeekDifference(currentDate, _calendarViewInfo.MonthView.FirstDayOfWeek, _calendarViewInfo.Identifier);

                // From example the 0001,1,1 day of week is Monday. So need to render the 1st date in index value 5. Because of the first date of day week text is rendered in index value 5.
                // The 0th index to 4th-index need to render empty. So draw will start from 5th-index. So need to change the xPosition value based on the valid start index.
                xPosition += startIndex * cellWidthOffset;
            }

            Globalization.Calendar cultureCalendar = CalendarViewHelper.GetCalendar(_calendarViewInfo.Identifier.ToString());
            int currentMonthValue = cultureCalendar.GetMonth(currentMonth);
            for (int i = 0; i < visibleDateCount; i++)
            {
                //// From example the start index value 6.
                int index = startIndex + i;
                DateTime dateTime = _visibleDates[i];
                int column = index % DaysPerWeek;
                //// If date arrangement reached to last column then need to start from next row and first column.
                if (column == 0 && index != 0)
                {
                    xPosition = isRTL ? monthViewWidth - monthCellWidth : weekNumberWidth;
                    yPosition += monthCellHeight;
                }

                bool isLeadingAndTrailingDates = cultureCalendar.GetMonth(dateTime) != currentMonthValue && isMonthView;
                //// If ShowLeadingAndTrailingDates set to false no need to draw leading and trailing view.
                //// But when number of weeks is less than 6, leading and trailing view will draw even ShowLeadingAndTrailingDates set to false.
                if (isLeadingAndTrailingDates && !_calendarViewInfo.ShowTrailingAndLeadingDates)
                {
                    xPosition += cellWidthOffset;
                    continue;
                }

                // The actual cell bounds. Based on this cell rectangle the selection highlight, selection range, extendable range hovering today highlight will draw.
                RectF highlightBounds = new Rect(xPosition, yPosition, monthCellWidth, monthCellHeight);

                // The center position contains centerXPosition and centerYPosition of the month cell.
                PointF centerPosition = highlightBounds.Center;

                // Assume the centerPosition.Y = 150. selectionRadius = 25.
                // The circleTopPosition = 150 - 25 = 125(circleTopPosition). The draw will start from the y axis with position value of 125.
                float circleTopPosition = _calendarViewInfo.SelectionShape == CalendarSelectionShape.Rectangle ? highlightBounds.Top : centerPosition.Y - selectionRadius;

                // If it is selected date then the selection is drawn based on the selection shape.
                switch (_calendarViewInfo.SelectionMode)
                {
                    case CalendarSelectionMode.Single:
                    case CalendarSelectionMode.Multiple:
                        {
                            bool isSelectedDate = IsSelectedDate(false, false, dateTime);
                            //// If it is selected date then the selection is drawn based on the selection shape.
                            if (isSelectedDate)
                            {
                                bool isBlackoutDate = CalendarViewHelper.IsDateInDateCollection(dateTime, _disabledDates);
                                bool isDisabledDate = CalendarViewHelper.IsDisabledDate(dateTime, _calendarViewInfo.View, _calendarViewInfo.EnablePastDates, _calendarViewInfo.MinimumDate, _calendarViewInfo.MaximumDate, _calendarViewInfo.SelectionMode, _calendarViewInfo.RangeSelectionDirection, _selectedRange, _calendarViewInfo.AllowViewNavigation, _calendarViewInfo.Identifier);
                                if (!isBlackoutDate && !isDisabledDate)
                                {
                                    DrawSelectionShape(canvas, highlightBounds, selectionRadius, cornerRadius, selectedDateBackground, centerPosition);
                                }
                            }

                            break;
                        }

                    case CalendarSelectionMode.Range:
                        {
                            // To find the range selection is start range or end range or in between range or not.
                            SelectedRangeStatus? rangeSelectionStatus = GetRangeSelectionStatus(dateTime);
                            bool isSelectedDate = rangeSelectionStatus != null;
                            if (isSelectedDate)
                            {
                                bool isBlackoutDate = CalendarViewHelper.IsDateInDateCollection(dateTime, _disabledDates);
                                bool isDisabledDate = CalendarViewHelper.IsDisabledDate(dateTime, _calendarViewInfo.View, _calendarViewInfo.EnablePastDates, _calendarViewInfo.MinimumDate, _calendarViewInfo.MaximumDate, _calendarViewInfo.SelectionMode, _calendarViewInfo.RangeSelectionDirection, _selectedRange, _calendarViewInfo.AllowViewNavigation, _calendarViewInfo.Identifier);
                                if (!isBlackoutDate && !isDisabledDate)
                                {
                                    DrawRangeSelection(canvas, highlightBounds, rangeSelectionStatus, rangeSelectionBackground, startRangeSelectionBackground, endRangeSelectionBackground, cornerRadius, isRTL, selectionRadius, circleSize, circleTopPosition, centerPosition, column);
                                }
                            }

                            break;
                        }

                    case CalendarSelectionMode.MultiRange:
                        {
                            // To find the range is start range or end range or in between range or not for the multi range.
                            SelectedRangeStatus? multiRangeSelectionStatus = GetMultiRangeSelectionStatus(dateTime, visibleSelectedDateRanges);
                            bool isSelectedDate = multiRangeSelectionStatus != null;
                            if (isSelectedDate)
                            {
                                bool isBlackoutDate = CalendarViewHelper.IsDateInDateCollection(dateTime, _disabledDates);
                                bool isDisabledDate = CalendarViewHelper.IsDisabledDate(dateTime, _calendarViewInfo.View, _calendarViewInfo.EnablePastDates, _calendarViewInfo.MinimumDate, _calendarViewInfo.MaximumDate, _calendarViewInfo.SelectionMode, _calendarViewInfo.RangeSelectionDirection, _selectedRange, _calendarViewInfo.AllowViewNavigation, _calendarViewInfo.Identifier);
                                if (!isBlackoutDate && !isDisabledDate)
                                {
                                    DrawRangeSelection(canvas, highlightBounds, multiRangeSelectionStatus, rangeSelectionBackground, startRangeSelectionBackground, endRangeSelectionBackground, cornerRadius, isRTL, selectionRadius, circleSize, circleTopPosition, centerPosition, column);
                                }
                            }

                            break;
                        }
                }

                xPosition += cellWidthOffset;
            }
        }

        /// <summary>
        /// Method to draw the week number views.
        /// </summary>
        /// <param name="canvas">The draw canvas.</param>
        /// <param name="isRTL">Is RTL flow direction or not.</param>
        /// <param name="weekNumberWidth">The Week number cell width.</param>
        /// <param name="monthViewWidth">The month view total width.</param>
        /// <param name="monthViewHeight">The month view total height.</param>
        /// <param name="monthCellHeight">The month cell height.</param>
        /// <param name="currentMonth">"The current month of the view.</param>
        void DrawWeekNumbers(ICanvas canvas, bool isRTL, float weekNumberWidth, float monthViewWidth, float monthViewHeight, float monthCellHeight, DateTime currentMonth)
        {
            float xPosition = isRTL ? monthViewWidth : 0;
            float yPosition = 0;
            Color weekNumberBackground = _calendarViewInfo.MonthView.WeekNumberStyle.Background.ToColor();
            if (weekNumberBackground != Colors.Transparent)
            {
                canvas.FillColor = weekNumberBackground;
                canvas.FillRectangle(xPosition, yPosition, weekNumberWidth, monthViewHeight);
            }

            CalendarTextStyle weekNumberTextStyle = _calendarViewInfo.MonthView.WeekNumberStyle.TextStyle;
            DateTime monthWeekStartDate = CalendarViewHelper.GetWeekStartDate(_visibleDates, _calendarViewInfo.Identifier);
            Globalization.Calendar cultureCalendar = CalendarViewHelper.GetCalendar(_calendarViewInfo.Identifier.ToString());
            //// The starting date of the calendar.
            DateTime minDate = cultureCalendar.MinSupportedDateTime;
            //// The ending date of the calendar.
            DateTime maxDate = cultureCalendar.MaxSupportedDateTime;
            int currentMonthValue = cultureCalendar.GetMonth(currentMonth);
            //// This is used to get a valid date to get valid week number based on the maxDate.
            DateTime visibleMaxDate = maxDate.AddDays(-DaysPerWeek);
            //// This is used to get a valid enDate. Based on the maxDate.
            DateTime visibleWeekDate = maxDate.AddDays(-6);
            //// The number of dates in the collection.
            double visibleDateCount = _visibleDates.Count;
            double visibleWeeks = Math.Ceiling(visibleDateCount / DaysPerWeek);
            for (int i = 0; i < visibleWeeks; i++)
            {
                //// Calculation to find the start date of the week based on days per week and number of weeks in view.
                DateTime startDate = _visibleDates[(i % DaysPerWeek) * DaysPerWeek];
                //// Calculation to find the end date of the current week by adding days based on days per week.
                DateTime endDate = visibleWeekDate.Date < startDate.Date ? maxDate : startDate.AddDays(DaysPerWeek - 1);

                int startDateMonth = cultureCalendar.GetMonth(startDate);
                int endDateMonth = cultureCalendar.GetMonth(endDate);
                //// Checking whether the start date and end date is in the current month week.
                //// Check the current month week based on week start date and end date while number of weeks is 6.
                //// Check the start date value with the current month value to ensure the week is leading month week.
                //// Check the end date value with the current month value to ensure the week is trailing month week.
                bool isCurrentMonthWeek = _numberOfWeeks < 6 ? true : currentMonthValue == startDateMonth || currentMonthValue == endDateMonth;
                if (_calendarViewInfo.ShowTrailingAndLeadingDates || isCurrentMonthWeek)
                {
                    //// To check the first row has Monday while reaching the Min date.
                    if (startDate.Date == minDate.Date && !CalendarViewHelper.IsInValidWeekNumberWeek(cultureCalendar, startDate, _calendarViewInfo.MonthView.FirstDayOfWeek, DaysPerWeek))
                    {
                        //// If the first row doesnt have Monday, need to render form the second row with week number starting form 1.
                        yPosition += monthCellHeight;
                        continue;
                    }

                    string weekNumber = CalendarViewHelper.GetWeekNumber(_calendarViewInfo.Identifier, monthWeekStartDate).ToString();
                    CalendarViewHelper.DrawText(canvas, weekNumber, weekNumberTextStyle, new RectF(xPosition, yPosition, weekNumberWidth, monthCellHeight), HorizontalAlignment.Center, VerticalAlignment.Center);
                }

                //// Getting week number by adding days based on number of weeks in view and days per week from the month's start date of the week.
                if (visibleMaxDate.Date >= monthWeekStartDate)
                {
                    monthWeekStartDate = monthWeekStartDate.AddDays(DaysPerWeek);
                }
                else
                {
                    monthWeekStartDate = maxDate;
                }

                yPosition += monthCellHeight;
            }
        }

        /// <summary>
        /// Method to draw selection shape.
        /// </summary>
        /// <param name="canvas">The draw canvas.</param>
        /// <param name="highlightBounds">The cell rectangle hold the current month cell bound(x position, y position, width, height).</param>
        /// <param name="selectionRadius">The selection radius for the circle.</param>
        /// <param name="cornerRadius">The corner radius for rounded rectangle.</param>
        /// <param name="cellBackground">The cell background.</param>
        /// <param name="centerPosition">The center position of the month cell.</param>
        void DrawSelectionShape(ICanvas canvas, RectF highlightBounds, float selectionRadius, float cornerRadius, Color cellBackground, PointF centerPosition)
        {
			// Check if a SelectionCellTemplate is defined, if the selection mode is Single,
            // and if the current view is a Month view (i.e., all conditions must be true).
            if (_calendarViewInfo.SelectionCellTemplate != null && _calendarViewInfo.SelectionMode == CalendarSelectionMode.Single && _calendarViewInfo.View == CalendarView.Month && _selectedDate != null)
			{
				// Create the selection cell template view based on the selected date and template settings.
				// This calls a helper function to generate the view using the template.
				// To get the month cell details for selected date
				CalendarCellDetails details = GetMonthCellDetails(_selectedDate.Value.Month, _selectedDate.Value);
				if (details != null)
				{
					_selectionCellTemplateView = CalendarViewHelper.CreateSelectionCellTemplate(_selectedDate, _calendarViewInfo.SelectionCellTemplate, _calendarViewInfo.MonthView, details, highlightBounds);
				}

				// Only proceed if the selection cell template view was successfully created (not null).
				if (_selectionCellTemplateView != null)
				{
					// Check if a CellTemplate is defined for the MonthView
					if (_calendarViewInfo.MonthView.CellTemplate != null)
					{
						// If there are no monthCells available, exit early (nothing to process)
						if (_monthCells == null || _monthCells.Count == 0)
						{
							return;
						}

						// Try to find the index of the month cell that matches the currently selected date
						int index = _monthCells.FindIndex(cell =>
						{
							var cellDetails = ((View)cell).BindingContext as CalendarCellDetails;
							return cellDetails?.Date == _selectedDate;
						});

						// If a matching month cell was found
						if (index != -1)
						{
							// Get the view corresponding to the found month cell
							var currentCellView = (View)_monthCells[index];

							// If there is a previously stored template view and its index is valid and different from the current one
							if (_previousMonthCellTemplateView != null)
							{
								int previousMonthCellIndex = _monthCells.IndexOf(_previousMonthCellTemplateView);
								if (previousMonthCellIndex != index && previousMonthCellIndex >= 0 && previousMonthCellIndex < _monthCells.Count)
								{
									// Restore the previous template view to its original position
									Children.RemoveAt(previousMonthCellIndex);
									Insert(previousMonthCellIndex, _previousMonthCellTemplateView);
#if !WINDOWS
                                    // Clear the previous template tracking variables
                                    _previousMonthCellTemplateView = null;
#endif
								}
							}

#if WINDOWS
							UpdateSelectionCellTemplate(index);
#else
                            // If there is no currently tracked previous template (i.e., first time or after clearing)
                            if (_previousMonthCellTemplateView == null)
                            {
                                // Update the cell at the selected index with the new selection cell template view
                                UpdateSelectionCellTemplate(index);
                            }
#endif
						}
					}
					else
					{
						// Add the new selection view to the parent container (the current view).
						Add(_selectionCellTemplateView);

						// If there was a previously existing cell template view, remove it from the container.
						// This ensures only one selection cell template is visible at a time.
						if (_previousSelectionCellTemplateView != null)
						{
							Remove(_previousSelectionCellTemplateView);
						}

						// Update the previous cell template view reference to the current one.
						// This allows the next time to know which view to remove (for reusability).
						_previousSelectionCellTemplateView = _selectionCellTemplateView;
						AbsoluteLayout.SetLayoutBounds(_selectionCellTemplateView, highlightBounds);
					}

#if ANDROID
                    this.InvalidateViewMeasure();
#endif
				}
			}
			else
			{
				canvas.FillColor = cellBackground;
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
        }

        /// <summary>
        /// Method to draw today highlight.
        /// </summary>
        /// <param name="canvas">The draw canvas.</param>
        /// <param name="xPosition">Starting position of X-axis.</param>
        /// <param name="yPosition">Starting position of Y-axis.</param>
        /// <param name="monthCellWidth">The month cell width.</param>
        /// <param name="monthCellHeight">The month cell height.</param>
        /// <param name="cornerRadius">The corner radius for rounded rectangle.</param>
        /// <param name="radius">The radius for the circle.</param>
        /// <param name="todayHighlightColor">The color for the today highlight.</param>
        void DrawTodayHighlight(ICanvas canvas, float xPosition, float yPosition, float monthCellWidth, float monthCellHeight, float cornerRadius, float radius, Color todayHighlightColor)
        {
            canvas.StrokeColor = todayHighlightColor;
            float strokeThickness = 1;
            canvas.StrokeSize = strokeThickness;
            float strokeLinePadding = strokeThickness / 2;
            if (_calendarViewInfo.SelectionShape == CalendarSelectionShape.Circle)
            {
                //// Need to consider the stroke size also in radius because stoke increase the size of circle based on its stroke size.
                canvas.DrawCircle(xPosition + (monthCellWidth / 2), yPosition + (monthCellHeight / 2), radius - strokeLinePadding);
            }
            else
            {
                //// Example: Stroke size is 10 and rectangle bound is 0,0,100,100
                //// The drawn rectangle will show like -5,-5,105,105. Here 105 is the value of width and height not right(105) and bottom(105) position.
                //// So we need to draw the rectangle 5,5,95,95. Here 95 is the value of width and height not right(95) and bottom(95) position.
                canvas.DrawRoundedRectangle(xPosition + strokeLinePadding + HighlightPadding, yPosition + strokeLinePadding + HighlightPadding, monthCellWidth - strokeThickness - (2 * HighlightPadding), monthCellHeight - strokeThickness - (2 * HighlightPadding), (float)cornerRadius);
            }
        }

        /// <summary>
        /// Method to get the month cell style.
        /// </summary>
        /// <param name="dateTime">The current visible date.</param>
        /// <param name="isTodayDate">Defines whether the date is today date or not. </param>
        /// <param name="isLeadingAndTrailingDates">Defines whether the date is leading and trailing date or not.</param>
        /// <param name="isBlackoutDate">Defines whether the date is blackout date or not.</param>
        /// <param name="isDisabledDate">Defined whether the date is disabled date or not.</param>
        /// <param name="isShowOutOfRangeDates">The date is out of range dates.</param>
        /// <param name="isSpecialDate">Defined whether the date is special date or not.</param>
        /// <param name="fillColor">The color to fill month cell.</param>
        /// <param name="cellBackground">The background color for the month cell.</param>
        /// <param name="trailingLeadingDatesBackground">Background for trailing and leading dates.</param>
        /// <param name="weekendsBackground">Background for weekend dates.</param>
        /// <param name="todayBackground">Background for today date.</param>
        /// <param name="disabledDatesBackground">Background for disabled dates.</param>
        /// <param name="specialDatesBackground">Background for special dates.</param>
        /// <param name="cultureCalendar">The culture calendar.</param>
        /// <returns>Returns the text style for the month cells.</returns>
        CalendarTextStyle GetMonthCellStyle(DateTime dateTime, bool isTodayDate, bool isLeadingAndTrailingDates, bool isBlackoutDate, bool isDisabledDate, bool isShowOutOfRangeDates, bool isSpecialDate, ref Color fillColor, Color cellBackground, Color trailingLeadingDatesBackground, Color? weekendsBackground, Color todayBackground, Color disabledDatesBackground, Color specialDatesBackground, Globalization.Calendar cultureCalendar)
        {
            CalendarMonthView monthViewSettings = _calendarViewInfo.MonthView;
            fillColor = cellBackground;

            //// If it is dates is out of range then change new text style and backgrounds are applied.
            if (!isShowOutOfRangeDates && (_calendarViewInfo.MinimumDate.Date > dateTime.Date || _calendarViewInfo.MaximumDate.Date < dateTime.Date))
            {
                fillColor = disabledDatesBackground;
                return CalendarViewHelper.GetOutOfRangeDatesTextStyle();
            }

            //// If it is blackout dates then blackout dates text style and backgrounds are applied.
            if (isBlackoutDate)
            {
                fillColor = disabledDatesBackground;
                return monthViewSettings.DisabledDatesTextStyle;
            }

            //// If it is special date then special dates text style and background are applied.
            if (isSpecialDate)
            {
                fillColor = specialDatesBackground;
                return monthViewSettings.SpecialDatesTextStyle;
            }

            // If it is disabled date then disabled date text styles and background are applied.
            if (isDisabledDate)
            {
                fillColor = disabledDatesBackground;
                return monthViewSettings.DisabledDatesTextStyle;
            }

            // If it is today date then today date text style and background are applied.
            if (isTodayDate)
            {
                fillColor = todayBackground;
                return monthViewSettings.TodayTextStyle;
            }

            CalendarTextStyle textStyle = monthViewSettings.TextStyle;

            //// If the month of current visible date is not equal to the current month then leading and trailing background and text styles are applied only when NumberOfWeeks is equal to 6.
            if (isLeadingAndTrailingDates)
            {
                fillColor = trailingLeadingDatesBackground;
                textStyle = monthViewSettings.TrailingLeadingDatesTextStyle;
            }

            //// If the day of current visible date is equal to weekend days then weekends background and text styles are applied.
            if (monthViewSettings.WeekendDays.Contains(cultureCalendar.GetDayOfWeek(dateTime)))
            {
                if (weekendsBackground != null)
                {
                    fillColor = weekendsBackground;
                }

                if (monthViewSettings.WeekendDatesTextStyle != null)
                {
                    textStyle = monthViewSettings.WeekendDatesTextStyle;
                }
            }

            return textStyle;
        }

        /// <summary>
        /// Method to update selected range value.
        /// </summary>
        void UpdateSelectedRangeValue(CalendarDateRange? selectedRange)
        {
            DateTime? startDate = selectedRange?.StartDate;
            DateTime? endDate = selectedRange?.EndDate?.Date;
            if (selectedRange == null || (startDate == null && endDate == null))
            {
                _selectedRange = null;
            }
            else if (startDate > endDate)
            {
                _selectedRange = new CalendarDateRange(endDate, startDate);
            }
            else
            {
                _selectedRange = new CalendarDateRange(startDate, endDate);
            }
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
                    if (startDate > endDate)
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
        /// Method to check whether the date is selected date based on the selection mode.
        /// </summary>
        /// <param name="isBlackoutDate">Defines whether the date is blackout date or not.</param>
        /// <param name="isDisabledDate">Defines whether the date is disabled date or not/</param>
        /// <param name="date">The calendar date.</param>
        /// <returns>Returns true if the date is selected date and false if the date is not selected date.</returns>
        bool IsSelectedDate(bool isBlackoutDate, bool isDisabledDate, DateTime date)
        {
            if (isBlackoutDate || isDisabledDate)
            {
                return false;
            }

            switch (_calendarViewInfo.SelectionMode)
            {
                case CalendarSelectionMode.Single:
                    return date.Date == _selectedDate?.Date;
                case CalendarSelectionMode.Multiple:
                    return CalendarViewHelper.IsDateInDateCollection(date, _selectedDates);
            }

            return false;
        }

        /// <summary>
        /// Triggers when month cells and week numbers are interacted.
        /// </summary>
        /// <param name="tapPoint">The interacted point.</param>
        /// <param name="isTapped">Is tapped action.</param>
        /// <param name="tapCount">The tap count.</param>
        DateTime? OnInteractionEvent(Point tapPoint, bool isTapped, int tapCount = 1)
        {
            DateTime? tappedDate = CalendarViewHelper.GetMonthDateFromPosition(tapPoint, Width, Height, _calendarViewInfo, _visibleDates);
            double weekNumberWidth = CalendarViewHelper.GetWeekNumberWidth(_calendarViewInfo.MonthView, (float)Width);
            bool isWeekNumberTapped = _calendarViewInfo.IsRTLLayout ? tapPoint.X >= Width - weekNumberWidth : tapPoint.X <= weekNumberWidth;
            if (_calendarViewInfo.MonthView.ShowWeekNumber && isWeekNumberTapped)
            {
                DateTime? weekDate;
                int weekIndex = (int)tapPoint.Y / (int)(Height / _calendarViewInfo.MonthView.NumberOfVisibleWeeks);

                // Number of days to find the current week can be calculated by using weekIndex(denotes tapped week's index) multiplied with seven since a week contains 7 days
                int numberOfDays = weekIndex * 7;
                weekDate = _visibleDates[0].AddDays(numberOfDays);
                if (!CalendarViewHelper.IsInteractableDate(weekDate.Value, _disabledDates, _visibleDates, _calendarViewInfo, _numberOfWeeks))
                {
                    return null;
                }

                _calendarViewInfo.TriggerCalendarInteractionEvent(isTapped, tapCount, weekDate.Value, CalendarElement.WeekNumber);
            }

            // Tap, double tapped and long pressed events should not be triggered when the date is disabled date, so we returning null.
            if (tappedDate == null || !CalendarViewHelper.IsInteractableDate(tappedDate.Value, _disabledDates, _visibleDates, _calendarViewInfo, _numberOfWeeks))
            {
                return null;
            }

#if IOS || MACCATALYST
            _calendarViewInfo.TriggerCalendarInteractionEvent(isTapped, tapCount, tappedDate.Value, CalendarElement.CalendarCell);
#endif
            return tappedDate;
        }

        /// <summary>
        /// Method to get the current month start date index on the visible date collection.
        /// </summary>
        /// <param name="currentMonth">The current month.</param>
        /// <returns>Returns the current month start date index in the visible date collection.</returns>
        int GetMonthStartDateIndex(int currentMonth)
        {
            Globalization.Calendar cultureCalendar = CalendarViewHelper.GetCalendar(_calendarViewInfo.Identifier.ToString());
            for (int i = 0; i < _visibleDates.Count; i++)
            {
                int month = cultureCalendar.GetMonth(_visibleDates[i]);
                if (month == currentMonth)
                {
                    return i;
                }
            }

            return 0;
        }

        /// <summary>
        /// Method to remove month cells handlers.
        /// </summary>
        void RemoveMonthCellsHandler()
        {
            if (_monthCells == null)
            {
                return;
            }

            foreach (View monthCell in _monthCells)
            {
                Remove(monthCell);
                if (monthCell.Handler != null && monthCell.Handler.PlatformView != null)
                {
                    monthCell.Handler.DisconnectHandler();
                }
            }

            _monthCells.Clear();
            _monthCells = null;
        }

        /// <summary>
        /// Method to get month cell details to use inside month cell template.
        /// </summary>
        /// <param name="currentMonth">The current month index.</param>
        /// <param name="dateTime">The cell date time value.</param>
        /// <returns>Returns the month cell details.</returns>
        CalendarCellDetails GetMonthCellDetails(int currentMonth, DateTime dateTime)
        {
            Globalization.Calendar cultureCalendar = CalendarViewHelper.GetCalendar(_calendarViewInfo.Identifier.ToString());
            int month = cultureCalendar.GetMonth(dateTime);
            return new CalendarCellDetails
            {
                Date = dateTime,
                IsTrailingOrLeadingDate = _numberOfWeeks == 6 && currentMonth != month,
            };
        }

        /// <summary>
        /// Method to generate month cells.
        /// </summary>
        /// <param name="isCurrentView">Checks whether the view is current view or not.</param>
        void GenerateMonthCells(bool isCurrentView)
        {
            Globalization.Calendar cultureCalendar = CalendarViewHelper.GetCalendar(_calendarViewInfo.Identifier.ToString());
            int currentMonth = cultureCalendar.GetMonth(_visibleDates[_visibleDates.Count / 2]);
            DataTemplate? template = _calendarViewInfo.MonthView.CellTemplate;

            if (template == null)
            {
                return;
            }

            //// Generate the view for template selector based on dispatcher.
            if (template is DataTemplateSelector selector)
            {
                //// If it is current view, then need to generate the template cells.
                if (isCurrentView)
                {
                    GenerateTemplateSelectorMonthCells(currentMonth, selector);
                }
                else
                {
                    //// If it is not current view, dispatch a delegate to the UI thread.
                    //// Method inside the dispatcher trigger once the current thread completed.
                    Dispatcher.Dispatch(() =>
                    {
                        GenerateTemplateSelectorMonthCells(currentMonth, selector);
                        //// Need to invalidate the views because this method triggered after the main thread.
                        InvalidateViewMeasure();
                    });
                }
            }
            else
            {
                bool hideLeadingTrailingDates = !CalendarViewHelper.ShowTrailingLeadingDates(_calendarViewInfo);
                int index = 0;
                foreach (DateTime dateTime in _visibleDates)
                {
                    CalendarCellDetails details = GetMonthCellDetails(currentMonth, dateTime);
                    //// If the cell is leading and trailing cell and if ShowTrailingAndLeadingDates is false, then no need to create month cells.
                    if (details.IsTrailingOrLeadingDate && hideLeadingTrailingDates)
                    {
                        continue;
                    }

                    CreateMonthCellTemplateView(template, details, index);
                    index++;
                }

                int maximumCellCount = _numberOfWeeks == 6 && hideLeadingTrailingDates ? MaxCellCount : _numberOfWeeks * DaysPerWeek;
                if (index != maximumCellCount)
                {
                    int neededViewCount = maximumCellCount - index;
                    for (int i = 0; i < neededViewCount; i++)
                    {
                        CreateMonthCellTemplateView(template, null, index);
                        index++;
                    }
                }
            }
        }

        /// <summary>
        /// Month to generate the month cells using template selector.
        /// </summary>
        /// <param name="currentMonth">The current month index.</param>
        /// <param name="templateSelector">The data template selector.</param>
        void GenerateTemplateSelectorMonthCells(int currentMonth, DataTemplateSelector templateSelector)
        {
            bool hideLeadingTrailingDates = !CalendarViewHelper.ShowTrailingLeadingDates(_calendarViewInfo);
            int index = 0;
            foreach (DateTime dateTime in _visibleDates)
            {
                CalendarCellDetails details = GetMonthCellDetails(currentMonth, dateTime);
                //// If the cell is leading and trailing cell and if ShowTrailingAndLeadingDates is false, then no need to create month cells.
                if (details.IsTrailingOrLeadingDate && hideLeadingTrailingDates)
                {
                    continue;
                }

                DataTemplate template = templateSelector.SelectTemplate(details, _calendarViewInfo.MonthView);
                CreateMonthCellTemplateView(template, details, index);
                index++;
            }
        }

        /// <summary>
        /// Method to create the month cell template views.
        /// </summary>
        /// <param name="template">The data template.</param>
        /// <param name="details">The binding context details.</param>
        /// <param name="index">The child index.</param>
        void CreateMonthCellTemplateView(DataTemplate template, CalendarCellDetails? details, int index)
        {
            //// Creating month cell template views from template based on details.
            View monthCellTemplateview = CalendarViewHelper.CreateTemplateView(template, details);
            _monthCells?.Add(monthCellTemplateview);
            Insert(index, monthCellTemplateview);
        }

        /// <summary>
        /// Method to update the month cell template views.
        /// </summary>
        /// <param name="currentMonth">The current month.</param>
        /// <param name="isNeedInvalidate">Defines whether the view is need to invalidate or not.</param>
        void UpdateMonthCellTemplateViews(int currentMonth, bool isNeedInvalidate)
        {
            //// To get the Cell Template for the month view.
            DataTemplate? template = _calendarViewInfo.MonthView.CellTemplate;
            if (template == null)
            {
                return;
            }

            if (template is DataTemplateSelector)
            {
                RemoveMonthCellsHandler();
                _monthCells = new List<View>();
                GenerateTemplateSelectorMonthCells(currentMonth, (DataTemplateSelector)template);
                InvalidateViewMeasure();
            }
            else
            {
                if (_monthCells == null)
                {
                    return;
                }

                bool hideTrailingLeadingDates = !CalendarViewHelper.ShowTrailingLeadingDates(_calendarViewInfo);
                int index = 0;
                foreach (DateTime dateTime in _visibleDates)
                {
                    CalendarCellDetails details = GetMonthCellDetails(currentMonth, dateTime);
                    //// If the cell is leading and trailing cell and if ShowTrailingAndLeadingDates is false, then no need to update the binding context for the view.
                    if (details.IsTrailingOrLeadingDate && hideTrailingLeadingDates)
                    {
                        continue;
                    }

                    //// Skip the update when the index value is greater than month cells count.
                    if (index >= _monthCells.Count)
                    {
                        continue;
                    }

                    View monthCell = _monthCells[index];
                    monthCell.BindingContext = details;
                    index++;
                }

                //// Here, we need to update the month cell position.
                if (isNeedInvalidate)
                {
                    InvalidateViewMeasure();
                }
            }
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
		/// Insert the selection cell template instead of selected month cell.
		/// </summary>
		/// <param name="index">Gets the selected month cell index</param>
		void UpdateSelectionCellTemplate(int index)
		{
			if (_monthCells != null && _selectionCellTemplateView != null)
			{
				// Store current selection info
				_previousMonthCellTemplateView = (View)_monthCells[index];

				// Replace with the selection view
				Children.RemoveAt(index);
				Insert(index, _selectionCellTemplateView);
			}
		}

		/// <summary>
		/// This method handle the selection cell template visibility for date restrictions
		/// </summary>
		void HideSelectionCellTemplateView()
		{
			if (_selectionCellTemplateView == null)
			{
				return;
			}

			// to check If the month cell template value not null
			if (_calendarViewInfo.MonthView.CellTemplate != null)
			{
				MonthView.RemoveTemplateView(this, _selectionCellTemplateView);
			}
			else
			{
				// Check if the selectionCellTemplateView is not null and is already part of the view's children.
				if (_selectionCellTemplateView != null && Children.Contains(_selectionCellTemplateView))
				{
					// Set the IsVisible property to false, effectively hiding the selectionCellTemplateView.
					_selectionCellTemplateView.IsVisible = false;
				}
			}
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
            //// Method to save the current state of the graphics in the canvas.
            canvas.SaveState();
            canvas.Antialias = true;
            bool isRTL = _calendarViewInfo.IsRTLLayout;
            float weekNumberWidth = CalendarViewHelper.GetWeekNumberWidth(_calendarViewInfo.MonthView, dirtyRect.Width);
            float monthViewWidth = dirtyRect.Width - weekNumberWidth;
            float monthViewHeight = dirtyRect.Height;
            float monthCellWidth = monthViewWidth / DaysPerWeek;
            float monthCellHeight = monthViewHeight / _numberOfWeeks;
            DateTime currentMonth = _visibleDates[_visibleDates.Count / 2];
            if (_calendarViewInfo.MonthView.ShowWeekNumber)
            {
                DrawWeekNumbers(canvas, isRTL, weekNumberWidth, monthViewWidth, monthViewHeight, monthCellHeight, currentMonth);
            }

            if (_calendarViewInfo.MonthView.CellTemplate == null)
            {
                DrawMonthCells(canvas, isRTL, weekNumberWidth, monthViewWidth, monthCellWidth, currentMonth, monthCellHeight);
            }
            else
            {
                DrawTemplateSelection(canvas, isRTL, weekNumberWidth, monthViewWidth, monthCellWidth, currentMonth, monthCellHeight);
            }

            //// Method to set the graphics state to most recently saved state.
            canvas.RestoreState();
        }

        /// <summary>
        /// Method to measure template views.
        /// </summary>
        /// <param name="widthConstraint">The available width.</param>
        /// <param name="heightConstraint">The available height.</param>
        /// <returns>The actual size.</returns>
        protected override Size MeasureContent(double widthConstraint, double heightConstraint)
        {
            if (_calendarViewInfo.MonthView.CellTemplate == null || _monthCells == null)
            {
                return base.MeasureContent(widthConstraint, heightConstraint);
            }

            double width = double.IsFinite(widthConstraint) ? widthConstraint : 0;
            double height = double.IsFinite(heightConstraint) ? heightConstraint : 0;
            double weekNumberWidth = CalendarViewHelper.GetWeekNumberWidth(_calendarViewInfo.MonthView, (float)width);
            double monthCellWidth = (width - weekNumberWidth) / DaysPerWeek;
            double monthCellHeight = height / _numberOfWeeks;

            foreach (View child in Children)
            {
#if MACCATALYST || (!ANDROID && !IOS)
                if (child is MonthHoverView)
                {
                    child.Measure(width, height);
                    continue;
                }
#endif
                child.Measure(monthCellWidth, monthCellHeight);
            }

            return new Size(width, height);
        }

        /// <summary>
        /// Method to arrange the template views.
        /// </summary>
        /// <param name="bounds">The available size.</param>
        /// <returns>The actual size.</returns>
        protected override Size ArrangeContent(Rect bounds)
        {
            if (_calendarViewInfo.MonthView.CellTemplate == null || _monthCells == null)
            {
                return base.ArrangeContent(bounds);
            }

            double weekNumberWidth = CalendarViewHelper.GetWeekNumberWidth(_calendarViewInfo.MonthView, (float)bounds.Width);
            double width = bounds.Width - weekNumberWidth;
            double monthCellWidth = width / DaysPerWeek;
            double monthCellHeight = bounds.Height / _numberOfWeeks;
            bool isRTL = _calendarViewInfo.IsRTLLayout;
            double monthCellXPosition = isRTL ? width - monthCellWidth : weekNumberWidth;
            double yPosition = 0;
            double cellWidthOffset = isRTL ? -monthCellWidth : monthCellWidth;
            int childIndex;
            //// This is the valid start index for to fetch the visible date from the visible date collection.
            int startIndex = 0;
            //// The number of dates in the collection.
            int visibleDateCount = _visibleDates.Count;

            // The visible dates count not a multiple value of 7 and number of weeks then need to render the date based on the first day of week basis.
            // Example: Assume display date is 0001,01,01 first day of week is Tuesday(Enumeration Value = 2) and number of weeks is 2.
            // From above scenario the visible dates contains from 0001,1,1 to 0001,1,8.
            // The visible dates[0] = (0001,1,1) and first date of day of week = Monday.
            // Here condition is true while visible date count is 8. Calculation 8 % 7 != 0.
            if (visibleDateCount % DaysPerWeek != 0)
            {
                DateTime currentDate = _visibleDates.First();
                //// From example the 0001,1,1 day of week is Monday. So need to render the 1st date in index value 5. Because of the first date of day week text is rendered in index value 5.
                //// The 0th index to 4th-index need to render empty. So draw will start from 5th-index.
                startIndex = CalendarViewHelper.GetFirstDayOfWeekDifference(currentDate, _calendarViewInfo.MonthView.FirstDayOfWeek, _calendarViewInfo.Identifier);
            }

            DateTime currentMonthDate = _visibleDates[visibleDateCount / 2];
            Globalization.Calendar cultureCalendar = CalendarViewHelper.GetCalendar(_calendarViewInfo.Identifier.ToString());
            int currentYear = cultureCalendar.GetYear(currentMonthDate);
            int currentMonth = cultureCalendar.GetMonth(currentMonthDate);

            //// We have to arrange month cells based on the ShowTrailingAndLeadingDates property.
            if (CalendarViewHelper.ShowTrailingLeadingDates(_calendarViewInfo))
            {
                //// Children should arrange from the starting position when ShowTrailingAndLeadingDates property is set to true.
                childIndex = startIndex;
            }
            else
            {
                //// Children should arrange from the position where the current month date starts so we are getting month start date index
                //// and updating x position of the month cell based on the child index.
                //// Example: If the child index is 3, width offset is 50 and monthCellXPosition is 0 then,
                //// monthCellXPosition = 0 + (3 * 50)
                //// So the child arrange from the position 150.
                childIndex = startIndex + GetMonthStartDateIndex(currentMonth);
            }

            monthCellXPosition = monthCellXPosition + (childIndex * cellWidthOffset);
            int daysInMonth = cultureCalendar.GetDaysInMonth(currentYear, currentMonth);
            bool hideTrailingLeadingDates = !CalendarViewHelper.ShowTrailingLeadingDates(_calendarViewInfo);
            //// Month cell index value, it does not considered the other views.
            int index = 0;
            foreach (View child in Children)
            {
#if MACCATALYST || (!ANDROID && !IOS)
                if (child is MonthHoverView)
                {
                    AbsoluteLayout.SetLayoutBounds(child, new Rect(0, 0, bounds.Width, bounds.Height));
                    continue;
                }
#endif
                if (childIndex % DaysPerWeek == 0 && childIndex != 0)
                {
                    monthCellXPosition = isRTL ? width - monthCellWidth : weekNumberWidth;
                    yPosition += monthCellHeight;
                }

                //// If ShowLeadingAndTrailingDates set to false no need to show leading and trailing dates while the number of visible weeks value equal to 6.
                //// When the view reaches the minimum and maximum date the visibility of the cells in the empty space must be set to false.
                if ((hideTrailingLeadingDates && daysInMonth <= index) || (!hideTrailingLeadingDates && index >= _visibleDates.Count))
                {
                    child.IsVisible = false;
                    childIndex++;
                    index++;
                    continue;
                }

                child.IsVisible = true;
                AbsoluteLayout.SetLayoutBounds(child, new Rect(monthCellXPosition, yPosition, monthCellWidth, monthCellHeight));
                monthCellXPosition += cellWidthOffset;
                childIndex++;
                index++;
            }

            return base.ArrangeContent(bounds);
        }

        /// <summary>
        /// Method to create the semantics node for each virtual view with bounds inside the month view.
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
            bool isRTL = _calendarViewInfo.IsRTLLayout;
            float weekNumberWidth = CalendarViewHelper.GetWeekNumberWidth(_calendarViewInfo.MonthView, (float)width);
            float monthViewWidth = (float)width - weekNumberWidth;
            float monthCellWidth = monthViewWidth / DaysPerWeek;
            float monthCellHeight = (float)height / _numberOfWeeks;
            DateTime currentMonth = _visibleDates[_visibleDates.Count / 2];
            float xPosition = isRTL ? monthViewWidth : 0;
            float yPosition = 0;
            //// The number of dates in the collection.
            double visibleDateCount = _visibleDates.Count;
            double visibleWeeks = Math.Ceiling(visibleDateCount / DaysPerWeek);
            if (_calendarViewInfo.MonthView.ShowWeekNumber)
            {
                DateTime monthWeekStartDate = CalendarViewHelper.GetWeekStartDate(_visibleDates, _calendarViewInfo.Identifier);
                Globalization.Calendar cultureCalendar = CalendarViewHelper.GetCalendar(_calendarViewInfo.Identifier.ToString());
                //// The starting date of the calendar.
                DateTime minDate = cultureCalendar.MinSupportedDateTime;
                //// The ending date of the calendar.
                DateTime maxDate = cultureCalendar.MaxSupportedDateTime;
                int currentMonthValue = cultureCalendar.GetMonth(currentMonth);
                //// This is used to get a valid date to get valid week number based on the maxDate.
                DateTime visibleMaxDate = maxDate.AddDays(-DaysPerWeek);
                //// This is used to get a valid enDate. Based on the maxDate.
                DateTime visibleWeekDate = maxDate.AddDays(-6);
                string weekText = SfCalendarResources.GetLocalizedString("Week");
                for (int i = 0; i < visibleWeeks; i++)
                {
                    //// Calculation to find the start date of the week based on days per week and number of weeks in view.
                    DateTime startDate = _visibleDates[(i % DaysPerWeek) * DaysPerWeek];
                    //// Calculation to find the end date of the current week by adding days based on days per week.
                    DateTime endDate = visibleWeekDate.Date < startDate.Date ? maxDate : startDate.AddDays(DaysPerWeek - 1);
                    int startDateMonth = cultureCalendar.GetMonth(startDate);
                    int endDateMonth = cultureCalendar.GetMonth(endDate);
                    //// Checking whether the start date and end date is in the current month week.
                    //// Check the current month week based on week start date and end date while number of weeks is 6.
                    //// Check the start date value with the current month value to ensure the week is leading month week.
                    //// Check the end date value with the current month value to ensure the week is trailing month week.
                    bool isCurrentMonthWeek = _numberOfWeeks < 6 ? true : currentMonthValue == startDateMonth || currentMonthValue == endDateMonth;
                    string weekNumber = string.Empty;
                    if (_calendarViewInfo.ShowTrailingAndLeadingDates || isCurrentMonthWeek)
                    {
                        //// To check the first row has Monday while reaching the Min date.
                        if (startDate.Date == minDate.Date && !CalendarViewHelper.IsInValidWeekNumberWeek(cultureCalendar, startDate, _calendarViewInfo.MonthView.FirstDayOfWeek, DaysPerWeek))
                        {
                            //// If the first row does not have Monday, need to render form the second row with week number starting form 1.
                            yPosition += monthCellHeight;
                            continue;
                        }

                        weekNumber = CalendarViewHelper.GetWeekNumber(_calendarViewInfo.Identifier, monthWeekStartDate).ToString();
                    }

                    //// Getting week number by adding days based on number of weeks in view and days per week from the month's start date of the week.
                    if (visibleMaxDate.Date >= monthWeekStartDate)
                    {
                        monthWeekStartDate = monthWeekStartDate.AddDays(DaysPerWeek);
                    }
                    else
                    {
                        monthWeekStartDate = maxDate;
                    }

                    //// The week number not in view then no need to generate the semantic node.
                    if (string.IsNullOrEmpty(weekNumber))
                    {
                        continue;
                    }

                    SemanticsNode node = new SemanticsNode()
                    {
                        Id = i,
                        Text = weekText + weekNumber,
                        Bounds = new Rect(xPosition, yPosition, weekNumberWidth, monthCellHeight),
                    };

                    _semanticsNodes.Add(node);
                    yPosition += monthCellHeight;
                }
            }

            //// The month cell template is not null then no need to generate the semantic node.
            if (_calendarViewInfo.MonthView.CellTemplate != null)
            {
                return _semanticsNodes;
            }

            //// The monthXPosition is used to define the starting position for drawing the cells.
            //// Example: Let total width of the month view = 70 and month cell width = 10.
            //// If the flow direction is RTL then it should draw the cells from the right side of the view.
            //// monthXPosition = 70 - 10 = 60.
            //// Else the cells drawn from the left side of the view.
            float monthXPosition = isRTL ? monthViewWidth - monthCellWidth : weekNumberWidth;
            float monthYPosition = 0;
            //// Cell width offset is used to define the drawing position of the cell based on the flow direction and the month cell width.
            //// Example: Let the total width of the month view = 70 and month cell width = 10.
            //// RTL - monthXPosition = 70 - 10 = 60
            //// monthXPosition = 60 - 10 = 50
            //// LTR - monthXPosition = 0
            //// monthXPosition = 0 + 10 = 10
            float cellWidthOffset = isRTL ? -monthCellWidth : monthCellWidth;
            //// Boolean to check whether the view is month view or not.
            //// If the number of weeks is less than 6 then the view is not considered as a month view.
            //// Leading and trailing dates can't be hide when the view is not month view.
            bool isMonthView = _numberOfWeeks == 6;
            //// This is the valid start index for to fetch the visible date from the visible date collection.
            int startIndex = 0;
            //// Get the culture info based on the calendar identifier.
            CultureInfo cultureInfo = CalendarViewHelper.GetCurrentUICultureInfo(_calendarViewInfo.Identifier);
            bool isGregorianCalendar = CalendarViewHelper.IsGregorianCalendar(_calendarViewInfo.Identifier);
            //// The visible dates count not a multiple value of 7 and number of weeks then need to render the date based on the first day of week basis.
            //// Example: Assume display date is 0001,01,01 first day of week is Tuesday(Enumeration Value = 2) and number of weeks is 2.
            //// From above scenario the visible dates contains from 0001,1,1 to 0001,1,8.
            //// The visible dates[0] = (0001,1,1) and first date of day of week = Monday.
            //// Here condition is true while visible date count is 8. Calculation 8 % 7 != 0.
            if (visibleDateCount % DaysPerWeek != 0)
            {
                DateTime currentDate = _visibleDates.First();
                startIndex = CalendarViewHelper.GetFirstDayOfWeekDifference(currentDate, _calendarViewInfo.MonthView.FirstDayOfWeek, _calendarViewInfo.Identifier);
                //// From example the 0001,1,1 day of week is Monday. So need to render the 1st date in index value 5. Because of the first date of day week text is rendered in index value 5.
                //// The 0th index to 4th-index need to render empty. So draw will start from 5th-index. So need to change the xPosition value based on the valid start index.
                monthXPosition += startIndex * cellWidthOffset;
            }

            for (int j = 0; j < visibleDateCount; j++)
            {
                //// From example the start index value 6.
                int index = startIndex + j;
                DateTime dateTime = _visibleDates[j];
                int column = index % DaysPerWeek;
                //// If date arrangement reached to last column then need to start from next row and first column.
                if (column == 0 && index != 0)
                {
                    monthXPosition = isRTL ? monthViewWidth - monthCellWidth : weekNumberWidth;
                    monthYPosition += monthCellHeight;
                }

                bool isLeadingAndTrailingDates = isMonthView && CalendarViewHelper.IsLeadingAndTrailingDate(dateTime, currentMonth, _calendarViewInfo.View, _calendarViewInfo.Identifier);
                //// If ShowLeadingAndTrailingDates set to false no need to draw leading and trailing view.
                //// But when number of weeks is less than 6, leading and trailing view will draw even ShowLeadingAndTrailingDates set to false.
                bool isLeadingAndTrailingDatesCells = isLeadingAndTrailingDates && !_calendarViewInfo.ShowTrailingAndLeadingDates;
                if (isLeadingAndTrailingDatesCells)
                {
                    monthXPosition += cellWidthOffset;
                    continue;
                }

                string blackOutDate = CalendarViewHelper.IsDateInDateCollection(dateTime, _disabledDates) ? SfCalendarResources.GetLocalizedString("Blackout Date") : string.Empty;
                string disabledDate = CalendarViewHelper.IsDisabledDate(dateTime, _calendarViewInfo.View, _calendarViewInfo.EnablePastDates, _calendarViewInfo.MinimumDate, _calendarViewInfo.MaximumDate, _calendarViewInfo.SelectionMode, _calendarViewInfo.RangeSelectionDirection, _selectedRange, _calendarViewInfo.AllowViewNavigation, _calendarViewInfo.Identifier) ? SfCalendarResources.GetLocalizedString("Disabled Date") : string.Empty;
                CalendarIconDetails? calendarSpecialDayIconDetails = _specialDates.FirstOrDefault(details => CalendarViewHelper.IsSameDate(_calendarViewInfo.View, details.Date, dateTime, _calendarViewInfo.Identifier));
                string specialDate = calendarSpecialDayIconDetails == null ? string.Empty : SfCalendarResources.GetLocalizedString("Special Date");
                string dateType = string.IsNullOrEmpty(specialDate) ? !string.IsNullOrEmpty(blackOutDate) ? blackOutDate : disabledDate : specialDate;
                string dateText = isGregorianCalendar ? dateTime.ToString("dddd, dd/MMMM/yyyy") + dateType : dateTime.ToString("dddd, dd/MMMM/yyyy", cultureInfo) + dateType;
                SemanticsNode monthCellsNodes = new SemanticsNode()
                {
                    //// Here need to add the j + visibleWeeks. Because if the week number is rendering then already ID is generate from 0 to 7. So that in month cells need to generate unique Id.
                    Id = j + (int)visibleWeeks,
                    Text = dateText,
                    Bounds = new Rect(monthXPosition, monthYPosition, monthCellWidth, monthCellHeight),
                };

                _semanticsNodes.Add(monthCellsNodes);
                monthXPosition += cellWidthOffset;
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

        /// <summary>
        /// Gets a value indicating whether to handle both single tap and double tap in the view or not.
        /// </summary>
        bool IGestureListener.IsRequiredSingleTapGestureRecognizerToFail => false;
#endif

#if WINDOWS || __ANDROID__
        /// <summary>
        /// Gets a value indicating whether to handle both single tap and double tap in the view or not.
        /// </summary>
        bool IGestureListener.IsRequiredSingleTapGestureRecognizerToFail => false;
#endif

        /// <summary>
        /// Occurs on tap interaction inside month view.
        /// </summary>
        /// <param name="e">The tap gesture listener event arguments.</param>
        void ITapGestureListener.OnTap(TapEventArgs e)
        {
#if __IOS__ || __MACCATALYST__
            OnSelection(e.TapPoint);
            Foundation.NSTimer.CreateScheduledTimer(0.38, false, (timer) => OnSingleTapConfirmed(new Point(e.TapPoint.X, e.TapPoint.Y), e.TapCount));
#endif
            //// To set the focus for keyboard interaction after the tap interaction.
            Focus();
#if !(__IOS__ || __MACCATALYST__)
            DateTime? tappedDate = OnInteractionEvent(e.TapPoint, true, e.TapCount);
            //// To skip the selection interaction while disabled dates are tapped.
            if (tappedDate == null)
            {
                return;
            }

            switch (_calendarViewInfo.SelectionMode)
            {
                case CalendarSelectionMode.Single:
                    {
                        if (_calendarViewInfo.CanToggleDaySelection && _selectedDate?.Date == tappedDate?.Date)
                        {
                            tappedDate = null;

							// If the selection cell template applied over click the same date while the CanToggleDaySelection value true.
							if (_selectionCellTemplateView != null)
							{
								// To handle the selection cell template visibility
								HideSelectionCellTemplateView();
							}
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
#endif
        }

        /// <summary>
        /// Occurs on double tap inside month view.
        /// </summary>
        /// <param name="e">The tap gesture listener event arguments.</param>
        void IDoubleTapGestureListener.OnDoubleTap(TapEventArgs e)
        {
#if __IOS__ || __MACCATALYST__
            _isDoubleTap = true;
#endif
#if !(__IOS__ || __MACCATALYST__)
            DateTime? tappedDate = OnInteractionEvent(e.TapPoint, true, e.TapCount);
            //// To skip the selection interaction while disabled dates are tapped.
            if (tappedDate == null)
            {
                return;
            }

            switch (_calendarViewInfo.SelectionMode)
            {
                case CalendarSelectionMode.Single:
                    {
                        if (_calendarViewInfo.CanToggleDaySelection && _selectedDate?.Date == tappedDate?.Date)
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
#endif

        }

        /// <summary>
        /// Occurs on long pressed interaction inside month view.
        /// </summary>
        /// <param name="e">The long pressed gesture listener event arguments.</param>
        void ILongPressGestureListener.OnLongPress(LongPressEventArgs e)
        {
            DateTime? tappedDate = OnInteractionEvent(e.TouchPoint, false);
            if (tappedDate != null)
            {
                _calendarViewInfo.TriggerCalendarInteractionEvent(false, 1, tappedDate.Value, CalendarElement.CalendarCell);
            }
        }

        /// <summary>
        /// Occurs on touch interaction.
        /// </summary>
        /// <param name="e">The pointer event arguments.</param>
        void ITouchListener.OnTouch(PointerEventArgs e)
        {
#if MACCATALYST || (!ANDROID && !IOS)
            //// Here the width and height values are considered as the out of the view so the conditions are checked with greater than or equal.
            if (e.TouchPoint.X >= Width || e.TouchPoint.X < 0 || e.TouchPoint.Y >= Height || e.TouchPoint.Y < 0)
            {
                _hoverView.UpdateHoverPosition(null);
                return;
            }

            if (e.Action == PointerActions.Moved || e.Action == PointerActions.Entered)
            {
                _hoverView.UpdateHoverPosition(e.TouchPoint);
            }
            else if (e.Action == PointerActions.Exited || e.Action == PointerActions.Cancelled)
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

        #endregion
    }
}