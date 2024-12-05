using Microsoft.Maui.Controls;
using Syncfusion.Maui.Toolkit.Localization;
using Syncfusion.Maui.Toolkit;
using Syncfusion.Maui.Toolkit.Calendar;
using System.Globalization;

namespace Syncfusion.Maui.Toolkit.UnitTest
{
    public class CalendarCombinationUnitTests : BaseUnitTest
	{
        #region Properties Combination

        [Theory]
        [InlineData(CalendarView.Month,CalendarView.Year)]
        [InlineData(CalendarView.Month, CalendarView.Decade)]
        [InlineData(CalendarView.Month, CalendarView.Century)]
        [InlineData(CalendarView.Year, CalendarView.Month)]
        [InlineData(CalendarView.Year, CalendarView.Decade)]
        [InlineData(CalendarView.Year, CalendarView.Century)]
        [InlineData(CalendarView.Decade, CalendarView.Year)]
        [InlineData(CalendarView.Decade, CalendarView.Month)]
        [InlineData(CalendarView.Decade, CalendarView.Century)]
        [InlineData(CalendarView.Century, CalendarView.Decade)]
        [InlineData(CalendarView.Century, CalendarView.Month)]
        [InlineData(CalendarView.Century, CalendarView.Year)]
        public void DisplayDate_CombineWith_Views(CalendarView initialViewValue,CalendarView dynamicViewValue)
        {
            SfCalendar calendar = new SfCalendar() { View = initialViewValue };

            DateTime expectedValue = DateTime.Today;
            calendar.DisplayDate = expectedValue;
            calendar.View = dynamicViewValue;
            DateTime actualValue = calendar.DisplayDate;
            
            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData("2025-02-19")]
        [InlineData("2026-08-04")]
        [InlineData("2017-03-08")]
        [InlineData("2029-02-16")]
        [InlineData("2021-05-14")]
        public void DisplayDate_CombinesWith_MinimumDate(string dateValue)
        {
            SfCalendar calendar = new SfCalendar();

            DateTime expectedValue = DateTime.Parse("2024-10-28");
            calendar.DisplayDate = expectedValue;
            calendar.MinimumDate = DateTime.Parse(dateValue);
            DateTime actualValue = calendar.DisplayDate;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData("2025-02-19")]
        [InlineData("2026-08-04")]
        [InlineData("2017-03-08")]
        [InlineData("2029-02-16")]
        [InlineData("2021-05-14")]
        public void DisplayDate_CombinesWith_MaximumDate(string dateValue)
        {
            SfCalendar calendar = new SfCalendar();

            DateTime expectedValue = DateTime.Parse("2015-10-28");
            calendar.DisplayDate = expectedValue;
            calendar.MaximumDate = DateTime.Parse(dateValue);
            DateTime actualValue = calendar.DisplayDate;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(CalendarIdentifier.Gregorian, CalendarIdentifier.Hijri)]
        [InlineData(CalendarIdentifier.Gregorian, CalendarIdentifier.Korean)]
        [InlineData(CalendarIdentifier.Gregorian, CalendarIdentifier.Persian)]
        [InlineData(CalendarIdentifier.Gregorian, CalendarIdentifier.Taiwan)]
        [InlineData(CalendarIdentifier.Gregorian, CalendarIdentifier.ThaiBuddhist)]
        [InlineData(CalendarIdentifier.Gregorian, CalendarIdentifier.UmAlQura)]
        [InlineData(CalendarIdentifier.Hijri, CalendarIdentifier.Gregorian)]
        [InlineData(CalendarIdentifier.Hijri, CalendarIdentifier.Korean)]
        [InlineData(CalendarIdentifier.Hijri, CalendarIdentifier.Persian)]
        [InlineData(CalendarIdentifier.Hijri, CalendarIdentifier.Taiwan)]
        [InlineData(CalendarIdentifier.Hijri, CalendarIdentifier.ThaiBuddhist)]
        [InlineData(CalendarIdentifier.Hijri, CalendarIdentifier.UmAlQura)]
        [InlineData(CalendarIdentifier.Korean, CalendarIdentifier.Hijri)]
        [InlineData(CalendarIdentifier.Korean, CalendarIdentifier.Gregorian)]
        [InlineData(CalendarIdentifier.Korean, CalendarIdentifier.Persian)]
        [InlineData(CalendarIdentifier.Korean, CalendarIdentifier.Taiwan)]
        [InlineData(CalendarIdentifier.Korean, CalendarIdentifier.ThaiBuddhist)]
        [InlineData(CalendarIdentifier.Korean, CalendarIdentifier.UmAlQura)]
        [InlineData(CalendarIdentifier.Persian, CalendarIdentifier.Hijri)]
        [InlineData(CalendarIdentifier.Persian, CalendarIdentifier.Korean)]
        [InlineData(CalendarIdentifier.Persian, CalendarIdentifier.Gregorian)]
        [InlineData(CalendarIdentifier.Persian, CalendarIdentifier.Taiwan)]
        [InlineData(CalendarIdentifier.Persian, CalendarIdentifier.ThaiBuddhist)]
        [InlineData(CalendarIdentifier.Persian, CalendarIdentifier.UmAlQura)]
        [InlineData(CalendarIdentifier.Taiwan, CalendarIdentifier.Hijri)]
        [InlineData(CalendarIdentifier.Taiwan, CalendarIdentifier.Korean)]
        [InlineData(CalendarIdentifier.Taiwan, CalendarIdentifier.Persian)]
        [InlineData(CalendarIdentifier.Taiwan, CalendarIdentifier.Gregorian)]
        [InlineData(CalendarIdentifier.Taiwan, CalendarIdentifier.ThaiBuddhist)]
        [InlineData(CalendarIdentifier.Taiwan, CalendarIdentifier.UmAlQura)]
        [InlineData(CalendarIdentifier.ThaiBuddhist, CalendarIdentifier.Hijri)]
        [InlineData(CalendarIdentifier.ThaiBuddhist, CalendarIdentifier.Korean)]
        [InlineData(CalendarIdentifier.ThaiBuddhist, CalendarIdentifier.Persian)]
        [InlineData(CalendarIdentifier.ThaiBuddhist, CalendarIdentifier.Taiwan)]
        [InlineData(CalendarIdentifier.ThaiBuddhist, CalendarIdentifier.Gregorian)]
        [InlineData(CalendarIdentifier.ThaiBuddhist, CalendarIdentifier.UmAlQura)]
        [InlineData(CalendarIdentifier.UmAlQura, CalendarIdentifier.Hijri)]
        [InlineData(CalendarIdentifier.UmAlQura, CalendarIdentifier.Korean)]
        [InlineData(CalendarIdentifier.UmAlQura, CalendarIdentifier.Persian)]
        [InlineData(CalendarIdentifier.UmAlQura, CalendarIdentifier.Taiwan)]
        [InlineData(CalendarIdentifier.UmAlQura, CalendarIdentifier.ThaiBuddhist)]
        [InlineData(CalendarIdentifier.UmAlQura, CalendarIdentifier.Gregorian)]
        public void DisplayDate_CombineWith_Identifier(CalendarIdentifier initialValue, CalendarIdentifier dynamicValue)
        {
            SfCalendar calendar = new SfCalendar() { Identifier = initialValue };

            DateTime expectedValue = DateTime.Today;
            calendar.DisplayDate = expectedValue;
            calendar.Identifier = dynamicValue;
            DateTime actualValue = calendar.DisplayDate;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(80)]
        [InlineData(200)]
        [InlineData(-75)]
        [InlineData(450)]
        [InlineData(0)]
        public void HeaderView_CombinesWithDisplayDate(int headerHeight)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.View = CalendarView.Month;
            calendar.DisplayDate = DateTime.Today;
            calendar.MonthView.HeaderView.Height = headerHeight;

            Assert.Equal(DateTime.Today, calendar.DisplayDate);
        }

        [Theory]
        [InlineData(80)]
        [InlineData(200)]
        [InlineData(-75)]
        [InlineData(450)]
        [InlineData(0)]
        public void FooterView_CombinesWithDisplayDate(int footerHeight)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.View = CalendarView.Month;
            calendar.DisplayDate = DateTime.Today;
            calendar.FooterView.ShowActionButtons = true;
            calendar.FooterView.ShowTodayButton = true;
            calendar.FooterView.Height = footerHeight;

            Assert.Equal(DateTime.Today, calendar.DisplayDate);
        }

        #endregion

        #region Events Combination


        [Theory]
        [InlineData(CalendarView.Month)]
        [InlineData(CalendarView.Year)]
        [InlineData(CalendarView.Century)]
        [InlineData(CalendarView.Decade)]
        public void SelectionChangedInvokedCombination(CalendarView view)
        {
            SfCalendar calendar = new SfCalendar() { View = view };
            var fired = false;
            calendar.SelectionChanged += (sender, e) => fired = true;
            calendar.SelectedDate = DateTime.Now.AddDays(30);
            Assert.True(fired);
        }

        [Theory]
        [InlineData(CalendarView.Month)]
        [InlineData(CalendarView.Year)]
        [InlineData(CalendarView.Century)]
        [InlineData(CalendarView.Decade)]
        public void ViewChangedInvokedCombination(CalendarView view)
        {
            SfCalendar calendar = new SfCalendar() { View = view };

            calendar.View = CalendarView.Month;
            var fired = false;
            calendar.ViewChanged += (sender, e) => fired = true;
            CustomSnapLayout customSnapLayout = new CustomSnapLayout(calendar);
            InvokePrivateMethod(customSnapLayout, "CreateOrResetVisibleDates", DateTime.Now);
            InvokePrivateMethod(customSnapLayout, "CreateOrResetVisibleDates", DateTime.Now.AddMonths(2));

            Assert.True(fired);
        }

        [Theory]
        [InlineData(CalendarView.Month)]
        [InlineData(CalendarView.Year)]
        [InlineData(CalendarView.Century)]
        [InlineData(CalendarView.Decade)]
        public void TappedInvokedCombination(CalendarView view)
        {
            SfCalendar calendar = new SfCalendar() { View = view };

            var fired = false;
            calendar.Tapped += (sender, e) => fired = true;
            ICalendar calendarView = calendar;
            calendarView.TriggerCalendarInteractionEvent(true, 1, DateTime.Now, CalendarElement.WeekNumber);

            Assert.True(fired);
        }

        [Theory]
        [InlineData(CalendarView.Month)]
        [InlineData(CalendarView.Year)]
        [InlineData(CalendarView.Century)]
        [InlineData(CalendarView.Decade)]
        public void DoubleTappedInvokedCombination(CalendarView view)
        {
            SfCalendar calendar = new SfCalendar() { View = view };

            var fired = false;
            calendar.DoubleTapped += (sender, e) => fired = true;
            ICalendar calendarView = calendar;
            calendarView.TriggerCalendarInteractionEvent(true, 2, DateTime.Now, CalendarElement.WeekNumber);

            Assert.True(fired);
        }

        [Theory]
        [InlineData(CalendarView.Month)]
        [InlineData(CalendarView.Year)]
        [InlineData(CalendarView.Century)]
        [InlineData(CalendarView.Decade)]
        public void LongPressedInvokedCombination(CalendarView view)
        {
            SfCalendar calendar = new SfCalendar() { View = view };

            var fired = false;
            calendar.LongPressed += (sender, e) => fired = true;
            ICalendar calendarView = calendar;
            calendarView.TriggerCalendarInteractionEvent(false, 1, DateTime.Now, CalendarElement.WeekNumber);

            Assert.True(fired);
        }

        #endregion

        #region Methods Combination

        [Theory]
        [InlineData(CalendarView.Year)]
        [InlineData(CalendarView.Century)]
        [InlineData(CalendarView.Decade)]
        public void Forward_ChangesDisplayDateToAdjancentMonth_WhenCalled_Combination(CalendarView calendarView)
        {
            SfCalendar calendar = new SfCalendar() { View = calendarView };

            Assert.Equal(calendar.DisplayDate.Date, calendar.DisplayDate.Date);
            calendar.Backward();
            Assert.Equal(calendar.DisplayDate.Date, calendar.DisplayDate.Date);
        }

        [Theory]
        [InlineData(CalendarView.Year)]
        [InlineData(CalendarView.Century)]
        [InlineData(CalendarView.Decade)]
        public void Backward_ChangesDisplayDateToAdjancentMonth_WhenCalled_Combination(CalendarView calendarView)
        {
            SfCalendar calendar = new SfCalendar() { View = calendarView };
            
            Assert.Equal(calendar.DisplayDate.Date, calendar.DisplayDate.Date);
            calendar.Backward();
            Assert.Equal(calendar.DisplayDate.Date, calendar.DisplayDate.Date);
        }

        #endregion

    }
}
