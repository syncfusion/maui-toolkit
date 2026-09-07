using System;
using System.Collections.ObjectModel;
using Syncfusion.Maui.Toolkit.Calendar;
namespace Syncfusion.Maui.Toolkit.UnitTest;

public class CalendarMethodsUnitTests : BaseUnitTest
{
    #region Internal Class Methods

    [Theory]
    [InlineData(0, CalendarIdentifier.Gregorian)]
    [InlineData(10, CalendarIdentifier.Gregorian)]
    [InlineData(200, CalendarIdentifier.Gregorian)]
    [InlineData(400, CalendarIdentifier.Gregorian)]
    [InlineData(500, CalendarIdentifier.Gregorian)]
    [InlineData(700, CalendarIdentifier.Gregorian)]
    [InlineData(0, CalendarIdentifier.Hijri)]
    [InlineData(10, CalendarIdentifier.Hijri)]
    [InlineData(200, CalendarIdentifier.Hijri)]
    [InlineData(400, CalendarIdentifier.Hijri)]
    [InlineData(500, CalendarIdentifier.Hijri)]
    [InlineData(700, CalendarIdentifier.Hijri)]
    [InlineData(0, CalendarIdentifier.Persian)]
    [InlineData(10, CalendarIdentifier.Persian)]
    [InlineData(200, CalendarIdentifier.Persian)]
    [InlineData(400, CalendarIdentifier.Persian)]
    [InlineData(500, CalendarIdentifier.Persian)]
    [InlineData(700, CalendarIdentifier.Persian)]
    [InlineData(0, CalendarIdentifier.Korean)]
    [InlineData(10, CalendarIdentifier.Korean)]
    [InlineData(200, CalendarIdentifier.Korean)]
    [InlineData(400, CalendarIdentifier.Korean)]
    [InlineData(500, CalendarIdentifier.Korean)]
    [InlineData(700, CalendarIdentifier.Korean)]
    [InlineData(0, CalendarIdentifier.Taiwan)]
    [InlineData(10, CalendarIdentifier.Taiwan)]
    [InlineData(200, CalendarIdentifier.Taiwan)]
    [InlineData(400, CalendarIdentifier.Taiwan)]
    [InlineData(500, CalendarIdentifier.Taiwan)]
    [InlineData(700, CalendarIdentifier.Taiwan)]
    [InlineData(0, CalendarIdentifier.ThaiBuddhist)]
    [InlineData(10, CalendarIdentifier.ThaiBuddhist)]
    [InlineData(200, CalendarIdentifier.ThaiBuddhist)]
    [InlineData(400, CalendarIdentifier.ThaiBuddhist)]
    [InlineData(500, CalendarIdentifier.ThaiBuddhist)]
    [InlineData(700, CalendarIdentifier.ThaiBuddhist)]
    [InlineData(0, CalendarIdentifier.UmAlQura)]
    [InlineData(10, CalendarIdentifier.UmAlQura)]
    [InlineData(200, CalendarIdentifier.UmAlQura)]
    [InlineData(400, CalendarIdentifier.UmAlQura)]
    [InlineData(500, CalendarIdentifier.UmAlQura)]
    [InlineData(700, CalendarIdentifier.UmAlQura)]
    public void IsGreaterDate_Month_ReturnsFalse_WhenPositiveValues(int dayValues, CalendarIdentifier calendarIdentifier)
    {
        bool actualValue = CalendarViewHelper.IsGreaterDate(DateTime.Now, CalendarView.Month, DateTime.Now.AddDays(dayValues), calendarIdentifier);
        Assert.False(actualValue);
    }

    [Theory]
    [InlineData(-10, CalendarIdentifier.Gregorian)]
    [InlineData(-200, CalendarIdentifier.Gregorian)]
    [InlineData(-400, CalendarIdentifier.Gregorian)]
    [InlineData(-500, CalendarIdentifier.Gregorian)]
    [InlineData(-700, CalendarIdentifier.Gregorian)]
    [InlineData(-10, CalendarIdentifier.Hijri)]
    [InlineData(-200, CalendarIdentifier.Hijri)]
    [InlineData(-400, CalendarIdentifier.Hijri)]
    [InlineData(-500, CalendarIdentifier.Hijri)]
    [InlineData(-700, CalendarIdentifier.Hijri)]
    [InlineData(-10, CalendarIdentifier.Persian)]
    [InlineData(-200, CalendarIdentifier.Persian)]
    [InlineData(-400, CalendarIdentifier.Persian)]
    [InlineData(-500, CalendarIdentifier.Persian)]
    [InlineData(-700, CalendarIdentifier.Persian)]
    [InlineData(-10, CalendarIdentifier.Korean)]
    [InlineData(-200, CalendarIdentifier.Korean)]
    [InlineData(-400, CalendarIdentifier.Korean)]
    [InlineData(-500, CalendarIdentifier.Korean)]
    [InlineData(-700, CalendarIdentifier.Korean)]
    [InlineData(-10, CalendarIdentifier.Taiwan)]
    [InlineData(-200, CalendarIdentifier.Taiwan)]
    [InlineData(-400, CalendarIdentifier.Taiwan)]
    [InlineData(-500, CalendarIdentifier.Taiwan)]
    [InlineData(-700, CalendarIdentifier.Taiwan)]
    [InlineData(-10, CalendarIdentifier.ThaiBuddhist)]
    [InlineData(-200, CalendarIdentifier.ThaiBuddhist)]
    [InlineData(-400, CalendarIdentifier.ThaiBuddhist)]
    [InlineData(-500, CalendarIdentifier.ThaiBuddhist)]
    [InlineData(-700, CalendarIdentifier.ThaiBuddhist)]
    [InlineData(-10, CalendarIdentifier.UmAlQura)]
    [InlineData(-200, CalendarIdentifier.UmAlQura)]
    [InlineData(-400, CalendarIdentifier.UmAlQura)]
    [InlineData(-500, CalendarIdentifier.UmAlQura)]
    [InlineData(-700, CalendarIdentifier.UmAlQura)]
    public void IsGreaterDate_Month_ReturnsTrue_WhenNegativeValues(int dayValues, CalendarIdentifier calendarIdentifier)
    {
        bool actualValue = CalendarViewHelper.IsGreaterDate(DateTime.Now, CalendarView.Month, DateTime.Now.AddDays(dayValues), calendarIdentifier);
        Assert.True(actualValue);
    }

    [Theory]
    [InlineData(0, CalendarIdentifier.Gregorian)]
    [InlineData(10, CalendarIdentifier.Gregorian)]
    [InlineData(200, CalendarIdentifier.Gregorian)]
    [InlineData(400, CalendarIdentifier.Gregorian)]
    [InlineData(500, CalendarIdentifier.Gregorian)]
    [InlineData(700, CalendarIdentifier.Gregorian)]
    [InlineData(0, CalendarIdentifier.Hijri)]
    [InlineData(10, CalendarIdentifier.Hijri)]
    [InlineData(200, CalendarIdentifier.Hijri)]
    [InlineData(400, CalendarIdentifier.Hijri)]
    [InlineData(500, CalendarIdentifier.Hijri)]
    [InlineData(700, CalendarIdentifier.Hijri)]
    [InlineData(0, CalendarIdentifier.Persian)]
    [InlineData(10, CalendarIdentifier.Persian)]
    [InlineData(200, CalendarIdentifier.Persian)]
    [InlineData(400, CalendarIdentifier.Persian)]
    [InlineData(500, CalendarIdentifier.Persian)]
    [InlineData(700, CalendarIdentifier.Persian)]
    [InlineData(0, CalendarIdentifier.Korean)]
    [InlineData(10, CalendarIdentifier.Korean)]
    [InlineData(200, CalendarIdentifier.Korean)]
    [InlineData(400, CalendarIdentifier.Korean)]
    [InlineData(500, CalendarIdentifier.Korean)]
    [InlineData(700, CalendarIdentifier.Korean)]
    [InlineData(0, CalendarIdentifier.Taiwan)]
    [InlineData(10, CalendarIdentifier.Taiwan)]
    [InlineData(200, CalendarIdentifier.Taiwan)]
    [InlineData(400, CalendarIdentifier.Taiwan)]
    [InlineData(500, CalendarIdentifier.Taiwan)]
    [InlineData(700, CalendarIdentifier.Taiwan)]
    [InlineData(0, CalendarIdentifier.ThaiBuddhist)]
    [InlineData(10, CalendarIdentifier.ThaiBuddhist)]
    [InlineData(200, CalendarIdentifier.ThaiBuddhist)]
    [InlineData(400, CalendarIdentifier.ThaiBuddhist)]
    [InlineData(500, CalendarIdentifier.ThaiBuddhist)]
    [InlineData(700, CalendarIdentifier.ThaiBuddhist)]
    [InlineData(0, CalendarIdentifier.UmAlQura)]
    [InlineData(10, CalendarIdentifier.UmAlQura)]
    [InlineData(200, CalendarIdentifier.UmAlQura)]
    [InlineData(400, CalendarIdentifier.UmAlQura)]
    [InlineData(500, CalendarIdentifier.UmAlQura)]
    [InlineData(700, CalendarIdentifier.UmAlQura)]
    public void IsGreaterDate_Year_ReturnsFalse_WhenPositiveValues(int dayValues, CalendarIdentifier calendarIdentifier)
    {
        bool actualValue = CalendarViewHelper.IsGreaterDate(DateTime.Now, CalendarView.Year, DateTime.Now.AddYears(dayValues), calendarIdentifier);
        Assert.False(actualValue);
    }

    [Theory]
    [InlineData(-10, CalendarIdentifier.Gregorian)]
    [InlineData(-200, CalendarIdentifier.Gregorian)]
    [InlineData(-400, CalendarIdentifier.Gregorian)]
    [InlineData(-500, CalendarIdentifier.Gregorian)]
    [InlineData(-700, CalendarIdentifier.Gregorian)]
    [InlineData(-10, CalendarIdentifier.Hijri)]
    [InlineData(-200, CalendarIdentifier.Hijri)]
    [InlineData(-400, CalendarIdentifier.Hijri)]
    [InlineData(-500, CalendarIdentifier.Hijri)]
    [InlineData(-700, CalendarIdentifier.Hijri)]
    [InlineData(-10, CalendarIdentifier.Persian)]
    [InlineData(-200, CalendarIdentifier.Persian)]
    [InlineData(-400, CalendarIdentifier.Persian)]
    [InlineData(-500, CalendarIdentifier.Persian)]
    [InlineData(-700, CalendarIdentifier.Persian)]
    [InlineData(-10, CalendarIdentifier.Korean)]
    [InlineData(-200, CalendarIdentifier.Korean)]
    [InlineData(-400, CalendarIdentifier.Korean)]
    [InlineData(-500, CalendarIdentifier.Korean)]
    [InlineData(-700, CalendarIdentifier.Korean)]
    [InlineData(-10, CalendarIdentifier.Taiwan)]
    [InlineData(-200, CalendarIdentifier.Taiwan)]
    [InlineData(-400, CalendarIdentifier.Taiwan)]
    [InlineData(-500, CalendarIdentifier.Taiwan)]
    [InlineData(-700, CalendarIdentifier.Taiwan)]
    [InlineData(-10, CalendarIdentifier.ThaiBuddhist)]
    [InlineData(-200, CalendarIdentifier.ThaiBuddhist)]
    [InlineData(-400, CalendarIdentifier.ThaiBuddhist)]
    [InlineData(-500, CalendarIdentifier.ThaiBuddhist)]
    [InlineData(-700, CalendarIdentifier.ThaiBuddhist)]
    [InlineData(-10, CalendarIdentifier.UmAlQura)]
    [InlineData(-200, CalendarIdentifier.UmAlQura)]
    [InlineData(-400, CalendarIdentifier.UmAlQura)]
    [InlineData(-500, CalendarIdentifier.UmAlQura)]
    [InlineData(-700, CalendarIdentifier.UmAlQura)]
    public void IsGreaterDate_Year_ReturnsTrue_WhenNegativeValues(int dayValues, CalendarIdentifier calendarIdentifier)
    {
        bool actualValue = CalendarViewHelper.IsGreaterDate(DateTime.Now, CalendarView.Year, DateTime.Now.AddYears(dayValues), calendarIdentifier);
        Assert.True(actualValue);
    }

    [Theory]
    [InlineData(0, CalendarIdentifier.Gregorian)]
    [InlineData(100, CalendarIdentifier.Gregorian)]
    [InlineData(200, CalendarIdentifier.Gregorian)]
    [InlineData(400, CalendarIdentifier.Gregorian)]
    [InlineData(500, CalendarIdentifier.Gregorian)]
    [InlineData(700, CalendarIdentifier.Gregorian)]
    [InlineData(0, CalendarIdentifier.Hijri)]
    [InlineData(100, CalendarIdentifier.Hijri)]
    [InlineData(200, CalendarIdentifier.Hijri)]
    [InlineData(400, CalendarIdentifier.Hijri)]
    [InlineData(500, CalendarIdentifier.Hijri)]
    [InlineData(700, CalendarIdentifier.Hijri)]
    [InlineData(0, CalendarIdentifier.Persian)]
    [InlineData(100, CalendarIdentifier.Persian)]
    [InlineData(200, CalendarIdentifier.Persian)]
    [InlineData(400, CalendarIdentifier.Persian)]
    [InlineData(500, CalendarIdentifier.Persian)]
    [InlineData(700, CalendarIdentifier.Persian)]
    [InlineData(0, CalendarIdentifier.Korean)]
    [InlineData(100, CalendarIdentifier.Korean)]
    [InlineData(200, CalendarIdentifier.Korean)]
    [InlineData(400, CalendarIdentifier.Korean)]
    [InlineData(500, CalendarIdentifier.Korean)]
    [InlineData(700, CalendarIdentifier.Korean)]
    [InlineData(0, CalendarIdentifier.Taiwan)]
    [InlineData(100, CalendarIdentifier.Taiwan)]
    [InlineData(200, CalendarIdentifier.Taiwan)]
    [InlineData(400, CalendarIdentifier.Taiwan)]
    [InlineData(500, CalendarIdentifier.Taiwan)]
    [InlineData(700, CalendarIdentifier.Taiwan)]
    [InlineData(0, CalendarIdentifier.ThaiBuddhist)]
    [InlineData(100, CalendarIdentifier.ThaiBuddhist)]
    [InlineData(200, CalendarIdentifier.ThaiBuddhist)]
    [InlineData(400, CalendarIdentifier.ThaiBuddhist)]
    [InlineData(500, CalendarIdentifier.ThaiBuddhist)]
    [InlineData(700, CalendarIdentifier.ThaiBuddhist)]
    [InlineData(0, CalendarIdentifier.UmAlQura)]
    [InlineData(100, CalendarIdentifier.UmAlQura)]
    [InlineData(200, CalendarIdentifier.UmAlQura)]
    [InlineData(400, CalendarIdentifier.UmAlQura)]
    [InlineData(500, CalendarIdentifier.UmAlQura)]
    [InlineData(700, CalendarIdentifier.UmAlQura)]
    public void IsGreaterDate_Century_ReturnsFalse_WhenPositiveValues(int dayValues, CalendarIdentifier calendarIdentifier)
    {
        bool actualValue = CalendarViewHelper.IsGreaterDate(DateTime.Now, CalendarView.Century, DateTime.Now.AddYears(dayValues), calendarIdentifier);
        Assert.False(actualValue);
    }

    [Theory]
    [InlineData(-100, CalendarIdentifier.Gregorian)]
    [InlineData(-200, CalendarIdentifier.Gregorian)]
    [InlineData(-400, CalendarIdentifier.Gregorian)]
    [InlineData(-500, CalendarIdentifier.Gregorian)]
    [InlineData(-700, CalendarIdentifier.Gregorian)]
    [InlineData(-100, CalendarIdentifier.Hijri)]
    [InlineData(-200, CalendarIdentifier.Hijri)]
    [InlineData(-400, CalendarIdentifier.Hijri)]
    [InlineData(-500, CalendarIdentifier.Hijri)]
    [InlineData(-700, CalendarIdentifier.Hijri)]
    [InlineData(-100, CalendarIdentifier.Persian)]
    [InlineData(-200, CalendarIdentifier.Persian)]
    [InlineData(-400, CalendarIdentifier.Persian)]
    [InlineData(-500, CalendarIdentifier.Persian)]
    [InlineData(-700, CalendarIdentifier.Persian)]
    [InlineData(-100, CalendarIdentifier.Korean)]
    [InlineData(-200, CalendarIdentifier.Korean)]
    [InlineData(-400, CalendarIdentifier.Korean)]
    [InlineData(-500, CalendarIdentifier.Korean)]
    [InlineData(-700, CalendarIdentifier.Korean)]
    [InlineData(-100, CalendarIdentifier.Taiwan)]
    [InlineData(-200, CalendarIdentifier.Taiwan)]
    [InlineData(-400, CalendarIdentifier.Taiwan)]
    [InlineData(-500, CalendarIdentifier.Taiwan)]
    [InlineData(-700, CalendarIdentifier.Taiwan)]
    [InlineData(-100, CalendarIdentifier.ThaiBuddhist)]
    [InlineData(-200, CalendarIdentifier.ThaiBuddhist)]
    [InlineData(-400, CalendarIdentifier.ThaiBuddhist)]
    [InlineData(-500, CalendarIdentifier.ThaiBuddhist)]
    [InlineData(-700, CalendarIdentifier.ThaiBuddhist)]
    [InlineData(-100, CalendarIdentifier.UmAlQura)]
    [InlineData(-200, CalendarIdentifier.UmAlQura)]
    [InlineData(-400, CalendarIdentifier.UmAlQura)]
    [InlineData(-500, CalendarIdentifier.UmAlQura)]
    [InlineData(-700, CalendarIdentifier.UmAlQura)]
    public void IsGreaterDate_Century_ReturnsTrue_WhenNegativeValues(int dayValues, CalendarIdentifier calendarIdentifier)
    {
        bool actualValue = CalendarViewHelper.IsGreaterDate(DateTime.Now, CalendarView.Century, DateTime.Now.AddYears(dayValues), calendarIdentifier);
        Assert.True(actualValue);
    }

    [Theory]
    [InlineData(0, CalendarIdentifier.Gregorian)]
    [InlineData(100, CalendarIdentifier.Gregorian)]
    [InlineData(200, CalendarIdentifier.Gregorian)]
    [InlineData(400, CalendarIdentifier.Gregorian)]
    [InlineData(500, CalendarIdentifier.Gregorian)]
    [InlineData(700, CalendarIdentifier.Gregorian)]
    [InlineData(0, CalendarIdentifier.Hijri)]
    [InlineData(100, CalendarIdentifier.Hijri)]
    [InlineData(200, CalendarIdentifier.Hijri)]
    [InlineData(400, CalendarIdentifier.Hijri)]
    [InlineData(500, CalendarIdentifier.Hijri)]
    [InlineData(700, CalendarIdentifier.Hijri)]
    [InlineData(0, CalendarIdentifier.Persian)]
    [InlineData(100, CalendarIdentifier.Persian)]
    [InlineData(200, CalendarIdentifier.Persian)]
    [InlineData(400, CalendarIdentifier.Persian)]
    [InlineData(500, CalendarIdentifier.Persian)]
    [InlineData(700, CalendarIdentifier.Persian)]
    [InlineData(0, CalendarIdentifier.Korean)]
    [InlineData(100, CalendarIdentifier.Korean)]
    [InlineData(200, CalendarIdentifier.Korean)]
    [InlineData(400, CalendarIdentifier.Korean)]
    [InlineData(500, CalendarIdentifier.Korean)]
    [InlineData(700, CalendarIdentifier.Korean)]
    [InlineData(0, CalendarIdentifier.Taiwan)]
    [InlineData(100, CalendarIdentifier.Taiwan)]
    [InlineData(200, CalendarIdentifier.Taiwan)]
    [InlineData(400, CalendarIdentifier.Taiwan)]
    [InlineData(500, CalendarIdentifier.Taiwan)]
    [InlineData(700, CalendarIdentifier.Taiwan)]
    [InlineData(0, CalendarIdentifier.ThaiBuddhist)]
    [InlineData(100, CalendarIdentifier.ThaiBuddhist)]
    [InlineData(200, CalendarIdentifier.ThaiBuddhist)]
    [InlineData(400, CalendarIdentifier.ThaiBuddhist)]
    [InlineData(500, CalendarIdentifier.ThaiBuddhist)]
    [InlineData(700, CalendarIdentifier.ThaiBuddhist)]
    [InlineData(0, CalendarIdentifier.UmAlQura)]
    [InlineData(100, CalendarIdentifier.UmAlQura)]
    [InlineData(200, CalendarIdentifier.UmAlQura)]
    [InlineData(400, CalendarIdentifier.UmAlQura)]
    [InlineData(500, CalendarIdentifier.UmAlQura)]
    [InlineData(700, CalendarIdentifier.UmAlQura)]
    public void IsGreaterDate_Decade_ReturnsFalse_WhenPositiveValues(int dayValues, CalendarIdentifier calendarIdentifier)
    {
        bool actualValue = CalendarViewHelper.IsGreaterDate(DateTime.Now, CalendarView.Decade, DateTime.Now.AddYears(dayValues), calendarIdentifier);
        Assert.False(actualValue);
    }

    [Theory]
    [InlineData(-100, CalendarIdentifier.Gregorian)]
    [InlineData(-200, CalendarIdentifier.Gregorian)]
    [InlineData(-400, CalendarIdentifier.Gregorian)]
    [InlineData(-500, CalendarIdentifier.Gregorian)]
    [InlineData(-700, CalendarIdentifier.Gregorian)]
    [InlineData(-100, CalendarIdentifier.Hijri)]
    [InlineData(-200, CalendarIdentifier.Hijri)]
    [InlineData(-400, CalendarIdentifier.Hijri)]
    [InlineData(-500, CalendarIdentifier.Hijri)]
    [InlineData(-700, CalendarIdentifier.Hijri)]
    [InlineData(-100, CalendarIdentifier.Persian)]
    [InlineData(-200, CalendarIdentifier.Persian)]
    [InlineData(-400, CalendarIdentifier.Persian)]
    [InlineData(-500, CalendarIdentifier.Persian)]
    [InlineData(-700, CalendarIdentifier.Persian)]
    [InlineData(-100, CalendarIdentifier.Korean)]
    [InlineData(-200, CalendarIdentifier.Korean)]
    [InlineData(-400, CalendarIdentifier.Korean)]
    [InlineData(-500, CalendarIdentifier.Korean)]
    [InlineData(-700, CalendarIdentifier.Korean)]
    [InlineData(-100, CalendarIdentifier.Taiwan)]
    [InlineData(-200, CalendarIdentifier.Taiwan)]
    [InlineData(-400, CalendarIdentifier.Taiwan)]
    [InlineData(-500, CalendarIdentifier.Taiwan)]
    [InlineData(-700, CalendarIdentifier.Taiwan)]
    [InlineData(-100, CalendarIdentifier.ThaiBuddhist)]
    [InlineData(-200, CalendarIdentifier.ThaiBuddhist)]
    [InlineData(-400, CalendarIdentifier.ThaiBuddhist)]
    [InlineData(-500, CalendarIdentifier.ThaiBuddhist)]
    [InlineData(-700, CalendarIdentifier.ThaiBuddhist)]
    [InlineData(-100, CalendarIdentifier.UmAlQura)]
    [InlineData(-200, CalendarIdentifier.UmAlQura)]
    [InlineData(-400, CalendarIdentifier.UmAlQura)]
    [InlineData(-500, CalendarIdentifier.UmAlQura)]
    [InlineData(-700, CalendarIdentifier.UmAlQura)]
    public void IsGreaterDate_Decade_ReturnsTrue_WhenNegativeValues(int dayValues, CalendarIdentifier calendarIdentifier)
    {
        bool actualValue = CalendarViewHelper.IsGreaterDate(DateTime.Now, CalendarView.Decade, DateTime.Now.AddYears(dayValues), calendarIdentifier);
        Assert.True(actualValue);
    }

    [Theory]
    [InlineData("2001-02-19")]
    [InlineData("2001-08-04")]
    [InlineData("2030-03-08")]
    [InlineData("2040-02-16")]
    public void IsDateWithinDateRange_ReturnsTrue_WhenWithinRange(String dateValue)
    {
        DateTime expectedDate = DateTime.Parse(dateValue);
        bool actualValue = CalendarViewHelper.IsDateWithinDateRange(expectedDate, new DateTime(2001, 01, 01), new DateTime(2050, 01, 01));
        Assert.True(actualValue);
    }

    [Theory]
    [InlineData("2001-02-19")]
    [InlineData("2001-08-04")]
    [InlineData("2030-03-08")]
    [InlineData("2040-02-16")]
    public void IsDateWithinDateRange_ReturnsFalse_WhenNotWithinRange(String dateValue)
    {
        DateTime expectedDate = DateTime.Parse(dateValue);
        bool actualValue = CalendarViewHelper.IsDateWithinDateRange(expectedDate, new DateTime(1995, 01, 01), new DateTime(2001, 01, 01));
        Assert.False(actualValue);
    }

    [Theory]
    [InlineData(CalendarIdentifier.Gregorian)]
    [InlineData(CalendarIdentifier.Hijri)]
    [InlineData(CalendarIdentifier.Korean)]
    [InlineData(CalendarIdentifier.Persian)]
    [InlineData(CalendarIdentifier.Taiwan)]
    [InlineData(CalendarIdentifier.ThaiBuddhist)]
    [InlineData(CalendarIdentifier.UmAlQura)]
    public void GetStartDate_Month_ReturnsStartDate_WhenCalled(CalendarIdentifier calendarIdentifier)
    {
        DateTime? expectedValue = new DateTime(2001, 08, 04);
        DateTime? actualValue = CalendarViewHelper.GetStartDate(expectedValue, CalendarView.Month, calendarIdentifier);
        Assert.Equal(expectedValue, actualValue);
    }

    [Theory]
    [InlineData(CalendarIdentifier.Gregorian)]
    [InlineData(CalendarIdentifier.Korean)]
    [InlineData(CalendarIdentifier.Taiwan)]
    [InlineData(CalendarIdentifier.ThaiBuddhist)]
    public void GetStartDate_LTR_Year_ReturnsStartDate_WhenCalled(CalendarIdentifier calendarIdentifier)
    {
        DateTime? actualValue = CalendarViewHelper.GetStartDate(new DateTime(2001,08,31), CalendarView.Year, calendarIdentifier);
        Assert.Equal(new DateTime(2001,08,01), actualValue);
    }

    [Theory]
    [InlineData(CalendarIdentifier.Hijri)]
    [InlineData(CalendarIdentifier.UmAlQura)]
    public void GetStartDate_RTL_Year_ReturnsStartDate_WhenCalled(CalendarIdentifier calendarIdentifier)
    {
        DateTime? actualValue = CalendarViewHelper.GetStartDate(new DateTime(2001,08,31), CalendarView.Year, calendarIdentifier);
        Assert.Equal(new DateTime(2001,08,20), actualValue);
    }

    [Fact]
    public void GetStartDate_Persian_Year_ReturnsStartDate_WhenCalled()
    {
        DateTime? actualValue = CalendarViewHelper.GetStartDate(new DateTime(2001,08,31), CalendarView.Year, CalendarIdentifier.Persian);
        Assert.Equal(new DateTime(2001,08,23), actualValue);
    }

    [Theory]
    [InlineData(CalendarIdentifier.Gregorian)]
    [InlineData(CalendarIdentifier.Korean)]
    [InlineData(CalendarIdentifier.Taiwan)]
    [InlineData(CalendarIdentifier.ThaiBuddhist)]
    public void GetStartDate_LTR_Decade_ReturnsStartDate_WhenCalled(CalendarIdentifier calendarIdentifier)
    {
        DateTime? actualValue = CalendarViewHelper.GetStartDate(new DateTime(2001,08,31), CalendarView.Decade, calendarIdentifier);
        Assert.Equal(new DateTime(2001,01,01), actualValue);
    }

    [Fact]
    public void GetStartDate_Hijri_Decade_ReturnsStartDate_WhenCalled()
    {
        DateTime? actualValue = CalendarViewHelper.GetStartDate(new DateTime(2001,08,25), CalendarView.Decade, CalendarIdentifier.Hijri);
        Assert.Equal(new DateTime(2001,03,25), actualValue);
    }

    [Fact]
    public void GetStartDate_UmAlQura_Decade_ReturnsStartDate_WhenCalled()
    {
        DateTime? actualValue = CalendarViewHelper.GetStartDate(new DateTime(2001,08,25), CalendarView.Decade, CalendarIdentifier.UmAlQura);
        Assert.Equal(new DateTime(2001,03,26), actualValue);
    }

    [Fact]
    public void GetStartDate_Persian_Decade_ReturnsStartDate_WhenCalled()
    {
        DateTime? actualValue = CalendarViewHelper.GetStartDate(new DateTime(2001,08,31), CalendarView.Decade, CalendarIdentifier.Persian);
        Assert.Equal(new DateTime(2001,03,21), actualValue);
    }

    [Fact]
    public void GetStartDate_Gregorian_Century_ReturnsStartDate_WhenCalled()
    {
        DateTime? actualValue = CalendarViewHelper.GetStartDate(new DateTime(2001,08,31), CalendarView.Century, CalendarIdentifier.Gregorian);
        Assert.Equal(new DateTime(2000,01,01), actualValue);
    }

    [Fact]
    public void GetStartDate_Taiwan_Century_ReturnsStartDate_WhenCalled()
    {
        DateTime? actualValue = CalendarViewHelper.GetStartDate(new DateTime(2001,08,31), CalendarView.Century, CalendarIdentifier.Taiwan);
        Assert.Equal(new DateTime(2001,01,01), actualValue);
    }

    [Fact]
    public void GetStartDate_ThaiBuddhist_Century_ReturnsStartDate_WhenCalled()
    {
        DateTime? actualValue = CalendarViewHelper.GetStartDate(new DateTime(2001,08,31), CalendarView.Century, CalendarIdentifier.ThaiBuddhist);
        Assert.Equal(new DateTime(1997,01,01), actualValue);
    }

    [Fact]
    public void GetStartDate_Korean_Century_ReturnsStartDate_WhenCalled()
    {
        DateTime? actualValue = CalendarViewHelper.GetStartDate(new DateTime(2001,08,31), CalendarView.Century, CalendarIdentifier.Korean);
        Assert.Equal(new DateTime(1997,01,01), actualValue);
    }

    [Fact]
    public void GetStartDate_Hijri_Century_ReturnsStartDate_WhenCalled()
    {
        DateTime? actualValue = CalendarViewHelper.GetStartDate(new DateTime(2001,08,25), CalendarView.Century, CalendarIdentifier.Hijri);
        Assert.Equal(new DateTime(1999,04,16), actualValue);
    }

    [Fact]
    public void GetStartDate_UmAlQura_Century_ReturnsStartDate_WhenCalled()
    {
        DateTime? actualValue = CalendarViewHelper.GetStartDate(new DateTime(2001,08,25), CalendarView.Century, CalendarIdentifier.UmAlQura);
        Assert.Equal(new DateTime(1999,04,17), actualValue);
    }

    [Fact]
    public void GetStartDate_Persian_Cetury_ReturnsStartDate_WhenCalled()
    {
        DateTime? actualValue = CalendarViewHelper.GetStartDate(new DateTime(2001,08,31), CalendarView.Century, CalendarIdentifier.Persian);
        Assert.Equal(new DateTime(2001,03,21), actualValue);
    }

	[Theory]
	[InlineData(CalendarIdentifier.Gregorian)]
	[InlineData(CalendarIdentifier.Hijri)]
	[InlineData(CalendarIdentifier.Korean)]
	[InlineData(CalendarIdentifier.Persian)]
	[InlineData(CalendarIdentifier.Taiwan)]
	[InlineData(CalendarIdentifier.ThaiBuddhist)]
	[InlineData(CalendarIdentifier.UmAlQura)]
	public void GetLastDate_Month_ReturnsLastDate_WhenCalled(CalendarIdentifier calendarIdentifier)
	{
		DateTime? expectedValue = new DateTime(2001, 08, 04);
		DateTime? actualValue = CalendarViewHelper.GetLastDate(CalendarView.Month, expectedValue, calendarIdentifier);
		Assert.Equal(expectedValue, actualValue);
	}

	[Theory]
	[InlineData(CalendarIdentifier.Gregorian)]
	[InlineData(CalendarIdentifier.Korean)]
	[InlineData(CalendarIdentifier.Taiwan)]
	[InlineData(CalendarIdentifier.ThaiBuddhist)]
	public void GetLastDate_LTR_Year_ReturnsLastDate_WhenCalled(CalendarIdentifier calendarIdentifier)
	{
		DateTime? actualValue = CalendarViewHelper.GetLastDate(CalendarView.Year, new DateTime(2001, 08, 31), calendarIdentifier);
		Assert.Equal(new DateTime(2001, 08, 31), actualValue);
	}

	[Theory]
	[InlineData(CalendarIdentifier.Hijri)]
	[InlineData(CalendarIdentifier.UmAlQura)]
	public void GetLastDate_RTL_Year_ReturnsLastDate_WhenCalled(CalendarIdentifier calendarIdentifier)
	{
		DateTime? actualValue = CalendarViewHelper.GetLastDate(CalendarView.Year, new DateTime(2001, 08, 31), calendarIdentifier);
		Assert.Equal(new DateTime(2001, 09, 17), actualValue);
	}

	[Fact]
	public void GetLastDate_Persian_Year_ReturnsLastDate_WhenCalled()
	{
		DateTime? actualValue = CalendarViewHelper.GetLastDate(CalendarView.Year, new DateTime(2001, 08, 31), CalendarIdentifier.Persian);
		Assert.Equal(new DateTime(2001, 09, 22), actualValue);
	}

	[Theory]
	[InlineData(CalendarIdentifier.Gregorian)]
	[InlineData(CalendarIdentifier.Korean)]
	[InlineData(CalendarIdentifier.Taiwan)]
	[InlineData(CalendarIdentifier.ThaiBuddhist)]
	public void GetLastDate_LTR_Decade_ReturnsLastDate_WhenCalled(CalendarIdentifier calendarIdentifier)
	{
		DateTime? actualValue = CalendarViewHelper.GetLastDate(CalendarView.Decade, new DateTime(2001, 08, 31), calendarIdentifier);
		Assert.Equal(new DateTime(2001, 08, 31), actualValue);
	}

	[Theory]
	[InlineData(CalendarIdentifier.Hijri)]
	[InlineData(CalendarIdentifier.UmAlQura)]
	public void GetLastDate_Hijri_UmalQura_Decade_ReturnsLastDate_WhenCalled(CalendarIdentifier calendarIdentifier)
	{
		DateTime? actualValue = CalendarViewHelper.GetLastDate(CalendarView.Decade, new DateTime(2001, 08, 25), calendarIdentifier);
		Assert.Equal(new DateTime(2001, 09, 17), actualValue);
	}

	[Fact]
	public void GetLastDate_Persian_Decade_ReturnsLastDate_WhenCalled()
	{
		DateTime? actualValue = CalendarViewHelper.GetLastDate(CalendarView.Decade, new DateTime(2001, 08, 31), CalendarIdentifier.Persian);
		Assert.Equal(new DateTime(2001, 09, 22), actualValue);
	}

	[Theory]
	[InlineData(CalendarIdentifier.Gregorian)]
	[InlineData(CalendarIdentifier.Korean)]
	[InlineData(CalendarIdentifier.Taiwan)]
	[InlineData(CalendarIdentifier.ThaiBuddhist)]
	public void GetLastDate_LTR_Century_ReturnsLastDate_WhenCalled(CalendarIdentifier calendarIdentifier)
	{
		DateTime? actualValue = CalendarViewHelper.GetLastDate( CalendarView.Century, new DateTime(2001, 08, 31), calendarIdentifier);
		Assert.Equal(new DateTime(2009, 12, 31), actualValue);
	}

	[Fact]
	public void GetLastDate_Hijri_Century_ReturnsLastDate_WhenCalled()
	{
		DateTime? actualValue = CalendarViewHelper.GetLastDate(CalendarView.Century, new DateTime(2001, 08, 25), CalendarIdentifier.Hijri);
		Assert.Equal(new DateTime(2008, 12, 27), actualValue);
	}

	[Fact]
	public void GetLastDate_UmAlQura_Century_ReturnsLastDate_WhenCalled()
	{
		DateTime? actualValue = CalendarViewHelper.GetLastDate(CalendarView.Century, new DateTime(2001, 08, 25), CalendarIdentifier.UmAlQura);
		Assert.Equal(new DateTime(2008, 12, 28), actualValue);
	}

	[Fact]
	public void GetLastDate_Persian_Cetury_ReturnsLastDate_WhenCalled()
	{
		DateTime? actualValue = CalendarViewHelper.GetLastDate(CalendarView.Century, new DateTime(2001, 08, 31), CalendarIdentifier.Persian);
		Assert.Equal(new DateTime(2009, 03, 20), actualValue);
	}

	[Theory]
	[InlineData(CalendarIdentifier.Gregorian, 31)]
	[InlineData(CalendarIdentifier.Korean, 32)]
	[InlineData(CalendarIdentifier.Taiwan, 32)]
	[InlineData(CalendarIdentifier.ThaiBuddhist, 32)]
	public void GetWeekNumber_LTR_Returns_Weekumber_WhenCalled(CalendarIdentifier calendarIdentifier, int expectedValue)
	{
		int actualValue = CalendarViewHelper.GetWeekNumber(calendarIdentifier, new DateTime(2001, 08, 04), new DateTime(2001, 08, 04).DayOfWeek);
		Assert.Equal(expectedValue, actualValue);
	}

	[Theory]
	[InlineData(CalendarIdentifier.Hijri, 20)]
	[InlineData(CalendarIdentifier.Persian, 21)]
	[InlineData(CalendarIdentifier.UmAlQura, 20)]
	public void GetWeekNumber_RTL_Returns_Weekumber_WhenCalled(CalendarIdentifier calendarIdentifier, int expectedValue)
	{
		int actualValue = CalendarViewHelper.GetWeekNumber(calendarIdentifier, new DateTime(2001, 08, 04), new DateTime(2001, 08, 04).DayOfWeek);
		Assert.Equal(expectedValue, actualValue);
	}

	[Theory]
	[InlineData("2001-02-19")]
	[InlineData("9999-02-24")]
	[InlineData("1234-02-24")]
	[InlineData("1995-02-22")]
	public void GetWeekNumberOfYear_Returns_WeekNumber_WhenCalled(String dateValue)
	{
		DateTime expectedValue = DateTime.Parse(dateValue);
		int actualValue = CalendarViewHelper.GetWeekNumberOfYear(expectedValue);
		Assert.Equal(8, actualValue);
	}

	[Theory]
	[InlineData(CalendarView.Year,1)]
	[InlineData(CalendarView.Century,100)]
	[InlineData(CalendarView.Decade,10)]
	[InlineData(CalendarView.Month,0)]
	public void GetOffset_ReturnsOffset_WhenCalled(CalendarView calendarView,int expectedValue)
	{
		int actualValue = CalendarViewHelper.GetOffset(calendarView);
		Assert.Equal(expectedValue, actualValue);
	}

	[Theory]
	[InlineData("2001-02-19")]
	[InlineData("9999-08-04")]
	[InlineData("1234-03-08")]
	[InlineData("1995-02-16")]
	public void CloneSelectedRanges_ReturnClonedRange_WhenCalled(String dateValue)
	{
		DateTime expectedDate = DateTime.Parse(dateValue);
		ObservableCollection<CalendarDateRange> expectedValue = new ObservableCollection<CalendarDateRange>();
		ObservableCollection<CalendarDateRange> actualValue = new ObservableCollection<CalendarDateRange>();

		expectedValue.Add(new CalendarDateRange(expectedDate, null));
		actualValue = CalendarViewHelper.CloneSelectedRanges(expectedValue);

		Assert.Equal(expectedValue[0].EndDate, actualValue[0].EndDate);
		Assert.Equal(expectedValue[0].StartDate, actualValue[0].StartDate);
	}

	[Theory]
	[InlineData(CalendarIdentifier.Gregorian, CalendarView.Month)]
	[InlineData(CalendarIdentifier.Hijri, CalendarView.Month)]
	[InlineData(CalendarIdentifier.Korean, CalendarView.Month)]
	[InlineData(CalendarIdentifier.Persian, CalendarView.Month)]
	[InlineData(CalendarIdentifier.Taiwan, CalendarView.Month)]
	[InlineData(CalendarIdentifier.ThaiBuddhist, CalendarView.Month)]
	[InlineData(CalendarIdentifier.UmAlQura, CalendarView.Month)]
	[InlineData(CalendarIdentifier.Gregorian, CalendarView.Year)]
	[InlineData(CalendarIdentifier.Hijri, CalendarView.Year)]
	[InlineData(CalendarIdentifier.Korean, CalendarView.Year)]
	[InlineData(CalendarIdentifier.Persian, CalendarView.Year)]
	[InlineData(CalendarIdentifier.Taiwan, CalendarView.Year)]
	[InlineData(CalendarIdentifier.ThaiBuddhist, CalendarView.Year)]
	[InlineData(CalendarIdentifier.UmAlQura, CalendarView.Year)]
	[InlineData(CalendarIdentifier.Gregorian, CalendarView.Century)]
	[InlineData(CalendarIdentifier.Hijri, CalendarView.Century)]
	[InlineData(CalendarIdentifier.Korean, CalendarView.Century)]
	[InlineData(CalendarIdentifier.Persian, CalendarView.Century)]
	[InlineData(CalendarIdentifier.Taiwan, CalendarView.Century)]
	[InlineData(CalendarIdentifier.ThaiBuddhist, CalendarView.Century)]
	[InlineData(CalendarIdentifier.UmAlQura, CalendarView.Century)]
	[InlineData(CalendarIdentifier.Gregorian, CalendarView.Decade)]
	[InlineData(CalendarIdentifier.Hijri, CalendarView.Decade)]
	[InlineData(CalendarIdentifier.Korean, CalendarView.Decade)]
	[InlineData(CalendarIdentifier.Persian, CalendarView.Decade)]
	[InlineData(CalendarIdentifier.Taiwan, CalendarView.Decade)]
	[InlineData(CalendarIdentifier.ThaiBuddhist, CalendarView.Decade)]
	[InlineData(CalendarIdentifier.UmAlQura, CalendarView.Decade)]

	public void AreRangesIntercept_ReturnsTrue_WhenInRange(CalendarIdentifier calendarIdentifier, CalendarView calendarView)
	{
		CalendarDateRange startRange = new CalendarDateRange(new DateTime(2001, 01, 04), new DateTime(2001, 08, 04));
		CalendarDateRange endRange = new CalendarDateRange(new DateTime(2001, 02, 19), new DateTime(2001, 08, 04));

		bool actualValue = CalendarViewHelper.AreRangesIntercept(calendarView, startRange, endRange, calendarIdentifier);

		Assert.True(actualValue);
	}

	[Theory]
	[InlineData(CalendarIdentifier.Gregorian, CalendarView.Month)]
	[InlineData(CalendarIdentifier.Hijri, CalendarView.Month)]
	[InlineData(CalendarIdentifier.Korean, CalendarView.Month)]
	[InlineData(CalendarIdentifier.Persian, CalendarView.Month)]
	[InlineData(CalendarIdentifier.Taiwan, CalendarView.Month)]
	[InlineData(CalendarIdentifier.ThaiBuddhist, CalendarView.Month)]
	[InlineData(CalendarIdentifier.UmAlQura, CalendarView.Month)]
	[InlineData(CalendarIdentifier.Gregorian, CalendarView.Year)]
	[InlineData(CalendarIdentifier.Hijri, CalendarView.Year)]
	[InlineData(CalendarIdentifier.Korean, CalendarView.Year)]
	[InlineData(CalendarIdentifier.Persian, CalendarView.Year)]
	[InlineData(CalendarIdentifier.Taiwan, CalendarView.Year)]
	[InlineData(CalendarIdentifier.ThaiBuddhist, CalendarView.Year)]
	[InlineData(CalendarIdentifier.UmAlQura, CalendarView.Year)]

	public void AreRangesIntercept_ReturnsFalse_WhenNotInRange(CalendarIdentifier calendarIdentifier, CalendarView calendarView)
	{
		CalendarDateRange startRange = new CalendarDateRange(new DateTime(2001, 02, 19), new DateTime(2001, 08, 04));
		CalendarDateRange endRange = new CalendarDateRange(new DateTime(2001, 01, 01), new DateTime(2001, 01, 04));

		bool actualValue = CalendarViewHelper.AreRangesIntercept(calendarView, startRange, endRange, calendarIdentifier);

		Assert.False(actualValue);
	}

	[Theory]
	[InlineData(CalendarIdentifier.Gregorian, CalendarView.Century)]
	[InlineData(CalendarIdentifier.Hijri, CalendarView.Century)]
	[InlineData(CalendarIdentifier.Korean, CalendarView.Century)]
	[InlineData(CalendarIdentifier.Persian, CalendarView.Century)]
	[InlineData(CalendarIdentifier.Taiwan, CalendarView.Century)]
	[InlineData(CalendarIdentifier.ThaiBuddhist, CalendarView.Century)]
	[InlineData(CalendarIdentifier.UmAlQura, CalendarView.Century)]
	[InlineData(CalendarIdentifier.Gregorian, CalendarView.Decade)]
	[InlineData(CalendarIdentifier.Hijri, CalendarView.Decade)]
	[InlineData(CalendarIdentifier.Korean, CalendarView.Decade)]
	[InlineData(CalendarIdentifier.Persian, CalendarView.Decade)]
	[InlineData(CalendarIdentifier.Taiwan, CalendarView.Decade)]
	[InlineData(CalendarIdentifier.ThaiBuddhist, CalendarView.Decade)]
	[InlineData(CalendarIdentifier.UmAlQura, CalendarView.Decade)]
	public void AreRangesIntercept_Century_Decade_ReturnsFalse_WhenNotInRange(CalendarIdentifier calendarIdentifier, CalendarView calendarView)
	{
		CalendarDateRange startRange = new CalendarDateRange(new DateTime(1700, 02, 19), new DateTime(1700, 08, 04));
		CalendarDateRange endRange = new CalendarDateRange(new DateTime(2201, 02, 19), new DateTime(2201, 08, 04));

		bool actualValue = CalendarViewHelper.AreRangesIntercept(calendarView, startRange, endRange, calendarIdentifier);

		Assert.False(actualValue);
	}

	[Theory]
	[InlineData(CalendarIdentifier.Gregorian,"")]
	[InlineData(CalendarIdentifier.Hijri,"ar-SA")]
	[InlineData(CalendarIdentifier.Korean,"ko-KR")]
	[InlineData(CalendarIdentifier.Persian,"fa")]
	[InlineData(CalendarIdentifier.Taiwan,"zh-TW")]
	[InlineData(CalendarIdentifier.ThaiBuddhist,"th-TH")]
	[InlineData(CalendarIdentifier.UmAlQura,"ar-SA")]
	public void GetLanguage_ReturnsCultureLanguage_WhenCalled(CalendarIdentifier calendarIdentifier,string expectedValue)
	{
		string calendarValue = calendarIdentifier.ToString();
		
		string actualValue = CalendarViewHelper.GetLanguage(calendarValue);

		Assert.Equal(expectedValue, actualValue);
	}

    [Theory]
	[InlineData(CalendarIdentifier.Gregorian,"2001-08-31")]
	[InlineData(CalendarIdentifier.Hijri,"2001-08-19")]
	[InlineData(CalendarIdentifier.Korean,"2001-08-31")]
	[InlineData(CalendarIdentifier.Persian,"2001-08-22")]
	[InlineData(CalendarIdentifier.Taiwan,"2001-08-31")]
	[InlineData(CalendarIdentifier.ThaiBuddhist,"2001-08-31")]
	[InlineData(CalendarIdentifier.UmAlQura,"2001-08-19")]
    public void GetViewLastDate_Month_ReturnViewsLastDate_WhenCalled(CalendarIdentifier calendarIdentifier,string dateValue)
    {
        DateTime actualValue = CalendarViewHelper.GetViewLastDate(CalendarView.Month,new DateTime(2001,08,04),calendarIdentifier);
        DateTime expectedValue = DateTime.Parse(dateValue);
        Assert.Equal(expectedValue,actualValue);
    }

    [Theory]
	[InlineData(CalendarIdentifier.Gregorian,"2001-12-01")]
	[InlineData(CalendarIdentifier.Hijri,"2002-02-13")]
	[InlineData(CalendarIdentifier.Korean,"2001-12-01")]
	[InlineData(CalendarIdentifier.Persian,"2002-02-20")]
	[InlineData(CalendarIdentifier.Taiwan,"2001-12-01")]
	[InlineData(CalendarIdentifier.ThaiBuddhist,"2001-12-01")]
	[InlineData(CalendarIdentifier.UmAlQura,"2002-02-13")]
    public void GetViewLastDate_Year_ReturnViewsLastDate_WhenCalled(CalendarIdentifier calendarIdentifier,string dateValue)
    {
        DateTime actualValue = CalendarViewHelper.GetViewLastDate(CalendarView.Year,new DateTime(2001,08,04),calendarIdentifier);
        DateTime expectedValue = DateTime.Parse(dateValue);
        Assert.Equal(expectedValue,actualValue);
    }
    
    [Theory]
	[InlineData(CalendarIdentifier.Gregorian,"2009-01-01")]
	[InlineData(CalendarIdentifier.Hijri,"2008-01-09")]
	[InlineData(CalendarIdentifier.Korean,"2006-01-01")]
	[InlineData(CalendarIdentifier.Persian,"2010-03-21")]
	[InlineData(CalendarIdentifier.Taiwan,"2010-01-01")]
	[InlineData(CalendarIdentifier.ThaiBuddhist,"2006-01-01")]
	[InlineData(CalendarIdentifier.UmAlQura,"2008-01-10")]
    public void GetViewLastDate_Decade_ReturnViewsLastDate_WhenCalled(CalendarIdentifier calendarIdentifier,string dateValue)
    {
        DateTime actualValue = CalendarViewHelper.GetViewLastDate(CalendarView.Decade,new DateTime(2001,08,04),calendarIdentifier);
        DateTime expectedValue = DateTime.Parse(dateValue);
        Assert.Equal(expectedValue,actualValue);
    }
    
    [Theory]
	[InlineData(CalendarIdentifier.Gregorian,"2099-01-01")]
	[InlineData(CalendarIdentifier.Hijri,"2075-12-08")]
	[InlineData(CalendarIdentifier.Korean,"2066-01-01")]
	[InlineData(CalendarIdentifier.Persian,"2020-03-20")]
	[InlineData(CalendarIdentifier.Taiwan,"2010-01-01")]
	[InlineData(CalendarIdentifier.ThaiBuddhist,"2056-01-01")]
	[InlineData(CalendarIdentifier.UmAlQura,"2075-12-09")]
    public void GetViewLastDate_Century_ReturnViewsLastDate_WhenCalled(CalendarIdentifier calendarIdentifier,string dateValue)
    {
        DateTime actualValue = CalendarViewHelper.GetViewLastDate(CalendarView.Century,new DateTime(2001,08,04),calendarIdentifier);
        DateTime expectedValue = DateTime.Parse(dateValue);
        Assert.Equal(expectedValue,actualValue);
    }

    [Theory]
    [InlineData (1)]
    [InlineData (2)]
    [InlineData (3)]
    [InlineData (4)]
    [InlineData (5)]
    [InlineData (6)]
    public void GetNumberOfWeeks_Returns_WeeksCount_WhenCalled(int expectedValue)
    {
        SfCalendar calendar = new SfCalendar();
        
        calendar.MonthView.NumberOfVisibleWeeks = expectedValue;
        int actualValue = CalendarViewHelper.GetNumberOfWeeks(calendar.MonthView);

        Assert.Equal(expectedValue,actualValue);
    }

    [Theory]
    [InlineData (10)]
    [InlineData (225)]
    [InlineData (36)]
    [InlineData (4235)]
    [InlineData (51)]
    [InlineData (60)]
    public void GetHeaderHeight_ReturnsHeight_WhenCalled(double expectedValue)
    {
        SfCalendar calendar = new SfCalendar();
        
        calendar.HeaderView.Height = expectedValue;
        double actualValue = CalendarViewHelper.GetHeaderHeight(calendar.HeaderView);

        Assert.Equal(expectedValue,actualValue);
    }

    [Theory]
    [InlineData (10)]
    [InlineData (225)]
    [InlineData (36)]
    [InlineData (4235)]
    [InlineData (51)]
    [InlineData (60)]
    public void GetFooterHeight_ReturnsHeight_WhenCalled(double expectedValue)
    {
        SfCalendar calendar = new SfCalendar();
        
        calendar.FooterView.Height = expectedValue;
        double actualValue = CalendarViewHelper.GetFooterHeight(calendar.FooterView);

        Assert.Equal(expectedValue,actualValue);
    }

    [Theory]
    [InlineData (10)]
    [InlineData (225)]
    [InlineData (36)]
    [InlineData (4235)]
    [InlineData (51)]
    [InlineData (60)]
    public void GetViewHeaderHeight_ReturnsHeight_WhenCalled(double expectedValue)
    {
        SfCalendar calendar = new SfCalendar();
        
        calendar.MonthView.HeaderView.Height = expectedValue;
        double actualValue = CalendarViewHelper.GetViewHeaderHeight(calendar.MonthView.HeaderView);

        Assert.Equal(expectedValue,actualValue);
    }

    [Theory]
    [InlineData (0.5,7)]
    [InlineData (10,140)]
    [InlineData (0,0)]
    public void GetWeekNumberWidth_ReturnsWidth_WhenCalled(float expectedValue,float widthValue)
    {
        SfCalendar calendar = new SfCalendar();

        calendar.MonthView.ShowWeekNumber = true;
        double actualValue = CalendarViewHelper.GetWeekNumberWidth(calendar.MonthView,widthValue);

        Assert.Equal(expectedValue,Math.Round(actualValue,2));
    }

	[Theory]
	[InlineData("2001-08-19")]
	[InlineData("9999-08-04")]
	[InlineData("1234-08-08")]
	[InlineData("1995-08-16")]
    public void GetYearCellText_Year_ReturnsText_WhenCalled(string dateValue)
    {
        SfCalendar calendar = new SfCalendar();
        calendar.View = CalendarView.Year;
        DateTime expectedDate = DateTime.Parse(dateValue);
        string actualValue = CalendarViewHelper.GetYearCellText(expectedDate,calendar);

        Assert.Equal("Aug",actualValue);
    }

    [Theory]
	[InlineData("2001-08-19")]
	[InlineData("9999-08-04")]
	[InlineData("1234-08-08")]
	[InlineData("1995-08-16")]
    public void GetYearCellText_Decade_ReturnsText_WhenCalled(string dateValue)
    {
        SfCalendar calendar = new SfCalendar();
        calendar.View = CalendarView.Decade;
        DateTime expectedValue = DateTime.Parse(dateValue);
        string actualValue = CalendarViewHelper.GetYearCellText(expectedValue,calendar);

        Assert.Equal(expectedValue.Year.ToString(),actualValue);
    }

    [Theory]
	[InlineData("2001-08-19","2001 - 2010")]
	[InlineData("9999-08-04","9999 - 9999")]
	[InlineData("1234-08-08","1234 - 1243")]
	[InlineData("1995-08-16","1995 - 2004")]
    public void GetYearCellText_Else_ReturnsText_WhenCalled(string dateValue,string expectedValue)
    {
        SfCalendar calendar = new SfCalendar();
        DateTime expectedDate = DateTime.Parse(dateValue);
        string actualValue = CalendarViewHelper.GetYearCellText(expectedDate,calendar);

        Assert.Equal(expectedValue,actualValue);
    }

	#endregion
}
