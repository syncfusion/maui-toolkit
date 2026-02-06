using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Syncfusion.Maui.Toolkit.Calendar;

namespace Syncfusion.Maui.Toolkit.UnitTest;

public class CalendarSubclassesUnitTests : BaseUnitTest
{
	#region MonthHoverView
	[Fact]
	public void GetUpdateHoverPosition_ReturnsPoint_WhenCalled()
	{
		ICalendar calendar = new SfCalendar();
		List<DateTime> visibleDates = new List<DateTime>();
		visibleDates.Add(new DateTime(2025, 09, 19));
		visibleDates.Add(new DateTime(2025, 09, 20));
		List<DateTime> disableDates = new List<DateTime>();
		disableDates.Add(new DateTime(2025, 03, 19));
		CalendarDateRange calendarDateRange = new CalendarDateRange(new DateTime(2025, 09, 19), new DateTime(2025, 09, 20));
		MonthHoverView monthHoverView = new MonthHoverView(calendar, visibleDates, disableDates, calendarDateRange);
		Point point = new Point(10, 20);
		monthHoverView.UpdateHoverPosition(point);
		Point? actualValue = (Point?)GetPrivateField(monthHoverView, "_hoverPosition");
		Assert.Equal(point, actualValue);
	}
	#endregion
	#region CalendarViewHelper
	[Theory]
	[InlineData("2025-09-20", true, CalendarView.Month)]
	[InlineData("2025-08-31", false, CalendarView.Month)]
	[InlineData("2015-08-31", false, CalendarView.Decade)]
	public void IsInteractableDate_ReturnsBool_WhenCalled(string dateString, bool expcetedValue, CalendarView setView)
	{
		SfCalendar calendar = new SfCalendar();
		List<DateTime> visibleDates = new List<DateTime>();
		visibleDates.Add(new DateTime(2025, 09, 19));
		visibleDates.Add(new DateTime(2025, 09, 20));
		List<DateTime> disableDates = new List<DateTime>();
		disableDates.Add(new DateTime(2025, 03, 19));
		calendar.ShowTrailingAndLeadingDates = false;
		calendar.View = setView;
		DateTime setDateTime = DateTime.Parse(dateString);
		ICalendarCommon calendarCommon = calendar;
		int numberOfWeeks = CalendarViewHelper.GetCurrentMonthsWeeks(calendar.MonthView, visibleDates, calendar.Identifier, calendar.ShowTrailingAndLeadingDates);
		bool actualValue = CalendarViewHelper.IsInteractableDate(setDateTime, disableDates, visibleDates, calendarCommon, numberOfWeeks);
		if (calendar.View == CalendarView.Month)
		{
			Assert.True(actualValue);
		}
		else
		{
			Assert.Equal(expcetedValue, actualValue);
		}
		Assert.Equal(setView, calendar.View);
		Assert.False(calendar.ShowTrailingAndLeadingDates);
		Assert.Equal(5, numberOfWeeks);
	}
	#endregion
	#region MonthView
	[Fact]
	public void MonthView_MeasureContent_ReturnSize_WhenCalled()
	{
		SfCalendar calendar = new SfCalendar();
		List<DateTime> visibleDates = new List<DateTime>();
		visibleDates.Add(new DateTime(2025, 03, 19));
		visibleDates.Add(new DateTime(2025, 03, 20));
		List<DateTime> disableDates = new List<DateTime>();
		disableDates.Add(new DateTime(2025, 03, 19));
		calendar.ShowTrailingAndLeadingDates = false;
		List<CalendarIconDetails> CalendarIconDetail = new List<CalendarIconDetails>();
		CalendarIconDetail.Add(new CalendarIconDetails() { Fill = Colors.Yellow });
		MonthView monthView = new MonthView(calendar, visibleDates, DateTime.Today, disableDates, CalendarIconDetail, true);
		Size? actualValue = (Size?)InvokePrivateMethod(monthView, "MeasureContent", 100, 100);
		Assert.Equal(new Size(0, 0), actualValue);
	}
	#endregion
	#region MonthView Autofit Affected Scenarios
	private MonthView CreateMonthView(SfCalendar calendar)
	{
		var visibleDates = new List<DateTime>
			{
				new DateTime(2025, 09, 19),
				new DateTime(2025, 09, 20)
			};
		var disableDates = new List<DateTime> { new DateTime(2025, 03, 19) };
		var icons = new List<CalendarIconDetails> { new CalendarIconDetails { Fill = Colors.Yellow } };
		return new MonthView(calendar, visibleDates, DateTime.Today, disableDates, icons, true);
	}
	[Fact]
	public void MonthView_MeasureContent_TrailingEnabled_NoAutofit_FullSize()
	{
		var calendar = new SfCalendar
		{
			View = CalendarView.Month,
			ShowTrailingAndLeadingDates = true
		};
		calendar.MonthView.NumberOfVisibleWeeks = 6;
		var monthView = CreateMonthView(calendar);
		calendar.MonthView.CellTemplate = new DataTemplate(() => new Label());
		SetPrivateField(monthView, "_monthCells", new List<View>());
		var size = (Size?)InvokePrivateMethod(monthView, "MeasureContent", 100d, 100d);
		Assert.Equal(new Size(100, 100), size);
	}
	[Fact]
	public void MonthView_MeasureContent_TrailingDisabled_NumberOfWeeks()
	{
		var calendar = new SfCalendar
		{
			View = CalendarView.Month,
			ShowTrailingAndLeadingDates = false
		};
		calendar.MonthView.NumberOfVisibleWeeks = 6; // Guard disables calendar autofit
		var monthView = CreateMonthView(calendar);
		var size = (Size?)InvokePrivateMethod(monthView, "MeasureContent", 120d, 80d);
		Assert.Equal(new Size(0, 0), size);
	}
	[Fact]
	public void MonthView_MeasureContent_ToggleTrailingDates_SizeRemainsStable()
	{
		var calendar = new SfCalendar { View = CalendarView.Month };
		calendar.MonthView.NumberOfVisibleWeeks = 6;
		var monthView = CreateMonthView(calendar);
		calendar.ShowTrailingAndLeadingDates = false;
		var sizeReducedContext = (Size?)InvokePrivateMethod(monthView, "MeasureContent", 150d, 90d);
		Assert.Equal(new Size(0, 0), sizeReducedContext);
		calendar.ShowTrailingAndLeadingDates = true;
		var sizeFullContext = (Size?)InvokePrivateMethod(monthView, "MeasureContent", 150d, 90d);
		Assert.Equal(new Size(0, 0), sizeFullContext);
	}
	[Fact]
	public void MonthView_MeasureContent_WithMinMaxDates_SizeUnchanged()
	{
		var calendar = new SfCalendar
		{
			View = CalendarView.Month,
			ShowTrailingAndLeadingDates = false,
			MinimumDate = new DateTime(2000, 1, 1),
			MaximumDate = new DateTime(2100, 12, 31)
		};
		calendar.MonthView.NumberOfVisibleWeeks = 6;
		var monthView = CreateMonthView(calendar);
		var size = (Size?)InvokePrivateMethod(monthView, "MeasureContent", 200d, 60d);
		Assert.Equal(new Size(0, 0), size);
	}
	[Fact]
	public void MonthView_MeasureContent_PopupModes_ContextStillMeasuresSame()
	{
		var calendar = new SfCalendar
		{
			Mode = CalendarMode.Dialog,
			View = CalendarView.Month,
			ShowTrailingAndLeadingDates = false,
			PopupHeight = 300
		};
		calendar.IsOpen = true;
		calendar.MonthView.NumberOfVisibleWeeks = 6;
		var monthView = CreateMonthView(calendar);
		var size = (Size?)InvokePrivateMethod(monthView, "MeasureContent", 180d, 90d);
		Assert.Equal(new Size(0, 0), size);
	}

	#endregion
}
