using Syncfusion.Maui.Toolkit.Localization;
using Syncfusion.Maui.Toolkit;
using Syncfusion.Maui.Toolkit.Calendar;
using System.Globalization;

namespace Syncfusion.Maui.Toolkit.UnitTest
{
    public class CalendarInternalUnitTests : BaseUnitTest
	{
        #region Internal Properties

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void HeaderView_DisabledNavigationArrowColor_GetAndSet_UsingColor(byte red, byte green, byte blue)
        {
            SfCalendar calendar = new SfCalendar();

            Color expectedValue = Color.FromRgb(red, green, blue);
            calendar.HeaderView.DisabledNavigationArrowColor = expectedValue;
            Brush actualValue = calendar.HeaderView.DisabledNavigationArrowColor;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void ButtonTextColor_GetAndSet_UsingBrush(byte red, byte green, byte blue)
        {
            SfCalendar calendar = new SfCalendar();

            Brush expectedValue = Color.FromRgb(red, green, blue);
            calendar.ButtonTextColor = expectedValue;
            Brush actualValue = calendar.ButtonTextColor;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void HeaderHoverTextColor_GetAndSet_UsingBrush(byte red, byte green, byte blue)
        {
            SfCalendar calendar = new SfCalendar();

            Brush expectedValue = Color.FromRgb(red, green, blue);
            calendar.HeaderHoverTextColor = expectedValue;
            Brush actualValue = calendar.HeaderHoverTextColor;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void HoverTextColor_GetAndSet_UsingBrush(byte red, byte green, byte blue)
        {
            SfCalendar calendar = new SfCalendar();

            Brush expectedValue = Color.FromRgb(red, green, blue);
            calendar.HoverTextColor = expectedValue;
            Brush actualValue = calendar.HoverTextColor;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void RangeSelectionColor_GetAndSet_UsingBrush(byte red, byte green, byte blue)
        {
            SfCalendar calendar = new SfCalendar();

            Brush expectedValue = Color.FromRgb(red, green, blue);
            calendar.RangeSelectionColor = expectedValue;
            Brush actualValue = calendar.RangeSelectionColor;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void CalendarBackground_GetAndSet_UsingColor(byte red, byte green, byte blue)
        {
            SfCalendar calendar = new SfCalendar();

            Color expectedValue = Color.FromRgb(red, green, blue);
            calendar.CalendarBackground = expectedValue;
            Color actualValue = calendar.CalendarBackground;

            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void CultureInfo_GetAndSet_CultureInfo()
        {
            CultureInfo expectedValue = CultureInfo.CurrentUICulture;
            CultureInfo actualValue = SfCalendarResources.CultureInfo;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData("2001-02-19")]
        [InlineData("9999-08-04")]
        [InlineData("1234-03-08")]
        [InlineData("1995-02-16")]
        public void CalendarHeaderDetails_StartDateRange_GetAndSet_DateTime(String dateValue)
        {
            CalendarHeaderDetails calendarHeader = new CalendarHeaderDetails();

            DateTime expectedValue = DateTime.Parse(dateValue);
            calendarHeader.StartDateRange = expectedValue;
            DateTime actualValue = calendarHeader.StartDateRange;

            Assert.Equal(expectedValue.Date, actualValue.Date);
        }

        [Theory]
        [InlineData("2001-02-19")]
        [InlineData("9999-08-04")]
        [InlineData("1234-03-08")]
        [InlineData("1995-02-16")]
        public void CalendarHeaderDetails_EndDateRange_GetAndSet_DateTime(String dateValue)
        {
            CalendarHeaderDetails calendarHeader = new CalendarHeaderDetails();

            DateTime expectedValue = DateTime.Parse(dateValue);
            calendarHeader.EndDateRange = expectedValue;
            DateTime actualValue = calendarHeader.EndDateRange;

            Assert.Equal(expectedValue.Date, actualValue.Date);
        }

        [Theory]
        [InlineData("January")]
        [InlineData("Febrauary")]
        [InlineData("March")]
        [InlineData("April")]
        [InlineData("May")]
        [InlineData("June")]
        [InlineData("July")]
        [InlineData("August")]
        [InlineData("September")]
        [InlineData("October")]
        [InlineData("November")]
        [InlineData("December")]
        public void CalendarHeaderDetails_Text_GetAndSet_String(string expectedValue)
        {
            CalendarHeaderDetails calendarHeader = new CalendarHeaderDetails();

            calendarHeader.Text = expectedValue;
            string actualValue = calendarHeader.Text;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(CalendarView.Year)]
        [InlineData(CalendarView.Century)]
        [InlineData(CalendarView.Decade)]
        [InlineData(CalendarView.Month)]
        public void CalendarHeaderDetails_View_GetAndSet_CalendarView(CalendarView expectedValue)
        {
            CalendarHeaderDetails calendarHeader = new CalendarHeaderDetails();

            calendarHeader.View = expectedValue;
            CalendarView actualValue = calendarHeader.View;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData("2001-02-19")]
        [InlineData("9999-08-04")]
        [InlineData("1234-03-08")]
        [InlineData("1995-02-16")]
        public void CalendarCellDetails_Date_GetAndSet_DateTime(String dateValue)
        {
            CalendarCellDetails calendarCell = new CalendarCellDetails();

            DateTime expectedValue = DateTime.Parse(dateValue);
            calendarCell.Date = expectedValue;
            DateTime actualValue = calendarCell.Date;

            Assert.Equal(expectedValue.Date, actualValue.Date);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void CalendarCellDetails_IsTrailingOrLeadingDate_GetAndSet_DateTime(bool expectedValue)
        {
            CalendarCellDetails calendarCell = new CalendarCellDetails();
            
            calendarCell.IsTrailingOrLeadingDate = expectedValue;
            bool actualValue = calendarCell.IsTrailingOrLeadingDate;

            Assert.Equal(expectedValue, actualValue);
        }

        #endregion

        #region Internal Methods

        [Theory]
        [InlineData("OK")]
        [InlineData("Cancel")]
        [InlineData("Today")]
        [InlineData("Special Date")]
        [InlineData("Blackout Date")]
        [InlineData("Disabled Date")]
        [InlineData("Week")]
        [InlineData("To")]
        [InlineData("Backward")]
        [InlineData("Forward")]
        [InlineData("Disabled Cell")]
        public void GetLocalizedString_ReturnsLocalString_BasedOnGivenString(string expectedValue)
        {
            string actualValue = SfCalendarResources.GetLocalizedString(expectedValue);
            SfCalendarResources.ResourceManager = new System.Resources.ResourceManager(typeof(System.Resources.ResourceManager));

            Assert.Equal(expectedValue, actualValue);

        }

        #endregion
    }
}
