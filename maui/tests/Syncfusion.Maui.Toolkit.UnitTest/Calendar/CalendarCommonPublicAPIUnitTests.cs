using Syncfusion.Maui.Toolkit.Themes;
using System.Windows.Input;
using Syncfusion.Maui.Toolkit;
using Syncfusion.Maui.Toolkit.Calendar;

namespace Syncfusion.Maui.Toolkit.UnitTest
{
    public class CalendarCommonPublicAPIUnitTests : BaseUnitTest
	{
        #region Public Methods

        [Fact]
        public void Forward_ChangesDisplayDateToAdjancentMonth_WhenCalled()
        {
            SfCalendar calendar = new SfCalendar();
            Assert.Equal(calendar.DisplayDate.Date, calendar.DisplayDate.Date);

            InvokePrivateMethod(calendar, "AddChildren");
            CalendarVerticalStackLayout? verticalLayout = GetPrivateField(calendar, "_layout") as CalendarVerticalStackLayout;
            CustomSnapLayout? customSnapLayout = null;
            if (verticalLayout != null)
            {
                customSnapLayout = verticalLayout.Children[2] as CustomSnapLayout;
            }
            InvokePrivateMethod(customSnapLayout, "CreateOrResetVisibleDates", DateTime.Now);

            Assert.Throws<ArgumentException>(() => calendar.Forward());
        }

        [Fact]
        public void Backward_ChangesDisplayDateToAdjancentMonth_WhenCalled()
        {
            SfCalendar calendar = new SfCalendar();
            Assert.Equal(calendar.DisplayDate.Date, calendar.DisplayDate.Date);

            InvokePrivateMethod(calendar, "AddChildren");
            CalendarVerticalStackLayout? verticalLayout = GetPrivateField(calendar, "_layout") as CalendarVerticalStackLayout;
            CustomSnapLayout? customSnapLayout = null;
            if (verticalLayout != null)
            {
                customSnapLayout = verticalLayout.Children[2] as CustomSnapLayout;
            }
            InvokePrivateMethod(customSnapLayout, "CreateOrResetVisibleDates", DateTime.Now);
            
            Assert.Throws<ArgumentException>(() => calendar.Backward());
        }

        [Fact]
        public void Forward_Null_WhenCalled()
        {
            SfCalendar calendar = new SfCalendar();

            var exception = Record.Exception(() => calendar.Forward());

            Assert.Null(exception);
        }

        [Fact]
        public void Backward_Null_WhenCalled()
        {
            SfCalendar calendar = new SfCalendar();

            var exception = Record.Exception(() => calendar.Backward());
            
            Assert.Null(exception);
        }

        #endregion

        #region FrameWork Properties

        [Theory]
        [InlineData(-10)]
        [InlineData(20)]
        [InlineData(0)]
        [InlineData(200)]
        [InlineData(-210)]
        public void Margin_GetAndSet_Thickness(int thicknessValue)
        {
            SfCalendar calendar = new SfCalendar();
            
            Thickness expectedValue = new Thickness(thicknessValue);
            calendar.Margin = expectedValue;
            Thickness actualValue = calendar.Margin;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(-10)]
        [InlineData(20)]
        [InlineData(0)]
        [InlineData(200)]
        [InlineData(-210)]
        public void Padding_GetAndSet_Thickness(int thicknessValue)
        {
            SfCalendar calendar = new SfCalendar();

            Thickness expectedValue = new Thickness(thicknessValue);
            calendar.Padding = expectedValue;
            Thickness actualValue = calendar.Padding;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void Background_GetAndSet_UsingBrush(byte red, byte green, byte blue)
        {
            SfCalendar calendar = new SfCalendar();

            Brush expectedValue = Color.FromRgb(red, green, blue);
            calendar.Background = expectedValue;
            Brush actualValue = calendar.Background;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(-10)]
        [InlineData(20)]
        [InlineData(0)]
        [InlineData(200)]
        [InlineData(-210)]
        public void Height_GetAndSet_Double(double expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.HeightRequest = expectedValue;
            double actualValue = calendar.HeightRequest;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(-10)]
        [InlineData(20)]
        [InlineData(0)]
        [InlineData(200)]
        [InlineData(-210)]
        public void Width_GetAndSet_Double(double expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.WidthRequest = expectedValue;
            double actualValue = calendar.WidthRequest;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(LayoutAlignment.Start)]
        [InlineData(LayoutAlignment.End)]
        [InlineData(LayoutAlignment.Center)]
        public void HorizontalOptions_GetAndSet_Alignment(LayoutAlignment alignmentValue)
        {
            SfCalendar calendar = new SfCalendar();

            LayoutOptions expectedValue = new LayoutOptions() { Alignment = alignmentValue };
            calendar.HorizontalOptions = expectedValue; 
            LayoutOptions actualValue = calendar.HorizontalOptions;

            Assert.Equal(expectedValue,actualValue);
        }

        [Theory]
        [InlineData(LayoutAlignment.Start)]
        [InlineData(LayoutAlignment.End)]
        [InlineData(LayoutAlignment.Center)]
        public void VertcalOptions_GetAndSet_Alignment(LayoutAlignment alignmentValue)
        {
            SfCalendar calendar = new SfCalendar();

            LayoutOptions expectedValue = new LayoutOptions() { Alignment = alignmentValue };
            calendar.VerticalOptions = expectedValue;
            LayoutOptions actualValue = calendar.VerticalOptions;

            Assert.Equal(expectedValue, actualValue);
        }

        #endregion

        #region Common Properties 

        [Fact]
        public void Constructor_InitializesDefaultsCorrectly()
        {
            SfCalendar calendar = new SfCalendar();

            // Common Properties
            Assert.Equal(CalendarView.Month, calendar.View);
            Assert.True(calendar.AllowViewNavigation);
            Assert.False(calendar.CanToggleDaySelection);
            Assert.Equal(new CornerRadius(20), calendar.CornerRadius);
            Assert.Equal(DateTime.Now.Date, calendar.DisplayDate.Date);
            Assert.True(calendar.EnablePastDates);
            Assert.False(calendar.EnableSwipeSelection);
            Assert.Equal(CalendarColors.GetPrimaryBrush().ToColor(), calendar.EndRangeSelectionBackground.ToColor());
            Assert.Equal(CalendarIdentifier.Gregorian, calendar.Identifier);
            Assert.Equal(DateTime.MaxValue, calendar.MaximumDate);
            Assert.Equal(DateTime.MinValue, calendar.MinimumDate);
            Assert.Equal(CalendarNavigationDirection.Vertical, calendar.NavigationDirection);
            Assert.Equal(CalendarRangeSelectionDirection.Default, calendar.RangeSelectionDirection);
            Assert.Null(calendar.SelectableDayPredicate);
            Assert.Null(calendar.SelectedDate);
            Assert.Null(calendar.SelectedDateRange);
            Assert.Empty(calendar.SelectedDates);
            Assert.Empty(calendar.SelectedDateRanges);
            Assert.Null(calendar.SelectionBackground);
            Assert.Equal(CalendarSelectionMode.Single, calendar.SelectionMode);
            Assert.Equal(CalendarSelectionShape.Circle, calendar.SelectionShape);
            Assert.True(calendar.ShowOutOfRangeDates);
            Assert.True(calendar.ShowTrailingAndLeadingDates);
            Assert.Equal(CalendarColors.GetPrimaryBrush().ToColor(), calendar.StartRangeSelectionBackground.ToColor());
            Assert.Equal(CalendarColors.GetPrimaryBrush().ToColor(), calendar.TodayHighlightBrush.ToColor());
            Assert.NotNull(calendar.MonthView);
            Assert.NotNull(calendar.YearView);
            Assert.NotNull(calendar.HeaderView);
            Assert.NotNull(calendar.FooterView);
            Assert.True(calendar.NavigateToAdjacentMonth);
            Assert.Null(calendar.HeaderTemplate);
            Assert.Null(calendar.MonthViewHeaderTemplate);
            Assert.Null(calendar.SelectionChangedCommand);
            Assert.Null(calendar.ViewChangedCommand);
            Assert.Null(calendar.TappedCommand);
            Assert.Null(calendar.DoubleTappedCommand);
            Assert.Null(calendar.LongPressedCommand);
            Assert.Null(calendar.AcceptCommand);
            Assert.Null(calendar.DeclineCommand);
        }

        [Theory]
        [InlineData(FlowDirection.LeftToRight,FlowDirection.RightToLeft)]
        [InlineData(FlowDirection.LeftToRight, FlowDirection.MatchParent)]
        [InlineData(FlowDirection.MatchParent,FlowDirection.RightToLeft)]
        [InlineData(FlowDirection.MatchParent, FlowDirection.LeftToRight)]
        [InlineData(FlowDirection.RightToLeft, FlowDirection.LeftToRight)]
        [InlineData(FlowDirection.RightToLeft, FlowDirection.MatchParent)]
        public void FlowDirection_GetAndSet_ReturnsFlowDirection(FlowDirection expectedValue1,FlowDirection expectedValu2)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.FlowDirection = expectedValue1;
            calendar.FlowDirection = expectedValu2;


            Assert.NotEqual(expectedValue1,calendar.FlowDirection);
        }

        [Theory]
        [InlineData(CalendarView.Year)]
        [InlineData(CalendarView.Century)]
        [InlineData(CalendarView.Decade)]
        [InlineData(CalendarView.Month)]
        public void View_GetAndSet_CaledarViews(CalendarView expectedValue)
        {
            SfCalendar calendar = new SfCalendar ();

            calendar.View = expectedValue;
            CalendarView actualValue = calendar.View;

            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void AllowViewNavigation_GetAndSet_WhenTrue()
        {
            SfCalendar calendar = new SfCalendar ();

            calendar.AllowViewNavigation = true;
            bool actualValue = calendar.AllowViewNavigation;

            Assert.True(actualValue);
        }

        [Fact]
        public void AllowViewNavigation_GetAndSet_WhenFalse()
        {
            SfCalendar calendar = new SfCalendar();

            calendar.AllowViewNavigation = false;
            bool actualValue = calendar.AllowViewNavigation;

            Assert.False(actualValue);
        }

        [Fact]
        public void CanToggleDaySelection_GetAndSet_WhenTrue()
        {
            SfCalendar calendar = new SfCalendar();

            calendar.CanToggleDaySelection = true;

            Assert.True(calendar.CanToggleDaySelection);
        }

        [Fact]
        public void CanToggleDaySelection_GetAndSet_WhenFalse()
        {
            SfCalendar calendar = new SfCalendar();

            calendar.CanToggleDaySelection = false;
            bool actualValue = calendar.CanToggleDaySelection;

            Assert.False(actualValue);
        }

        [Theory]
        [InlineData(50)]
        [InlineData(250)]
        [InlineData(-40)]
        [InlineData(-850)]
        [InlineData(0)]
        public void CornerRadius_GetAndSet_UsingCornerRadius(int cornerRadiusValue)
        {
            SfCalendar calendar = new SfCalendar();

            CornerRadius expectedValue = new CornerRadius(cornerRadiusValue);
            calendar.CornerRadius = expectedValue;
            CornerRadius actualValue = calendar.CornerRadius;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData("2001-02-19")]
        [InlineData("9999-08-04")]
        [InlineData("1234-03-08")]
        [InlineData("1995-02-16")]
        public void DisplayDate_GetAndSet_UsingDateTime(String dateValue)
        {
            SfCalendar calendar = new SfCalendar();
            
            DateTime expectedValue = DateTime.Parse(dateValue);
            calendar.DisplayDate = expectedValue;
            DateTime actualValue = calendar.DisplayDate;

            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void EnablePastDates_GetAndSet_WhenTrue()
        {
            SfCalendar calendar = new SfCalendar();

            calendar.EnablePastDates = true;
            bool actualValue = calendar.EnablePastDates;

            Assert.True(actualValue);
        }

        [Fact]
        public void EnablePastDates_GetAndSet_WhenFalse()
        {
            SfCalendar calendar = new SfCalendar();

            calendar.EnablePastDates = false;
            bool actualValue = calendar.EnablePastDates;

            Assert.False(actualValue);
        }

        [Fact]
        public void EnableSwipeSelection_GetAndSet_WhenTrue()
        {
            SfCalendar calendar = new SfCalendar();

            calendar.EnableSwipeSelection = true;
            bool actualValue = calendar.EnableSwipeSelection;

            Assert.True(actualValue);
        }

        [Fact]
        public void EnableSwipeSelection_GetAndSet_WhenFalse()
        {
            SfCalendar calendar = new SfCalendar();

            calendar.EnableSwipeSelection = false;
            bool actualValue = calendar.EnableSwipeSelection;

            Assert.False(actualValue);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void EndRangeSelectionBackground_GetAndSet_UsingBrush(byte red, byte green, byte blue)
        {
            SfCalendar calendar = new SfCalendar();

            Brush expectedValue = Color.FromRgb(red,green,blue);
            calendar.EndRangeSelectionBackground = expectedValue;
            Brush actualValue = calendar.EndRangeSelectionBackground;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(CalendarIdentifier.Gregorian)]
        [InlineData(CalendarIdentifier.Hijri)]
        [InlineData(CalendarIdentifier.Korean)]
        [InlineData(CalendarIdentifier.Persian)]
        [InlineData(CalendarIdentifier.Taiwan)]
        [InlineData(CalendarIdentifier.ThaiBuddhist)]
        [InlineData(CalendarIdentifier.UmAlQura)]
        public void Identifier_GetAndSet_UsingCalendarIdentifier(CalendarIdentifier expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.Identifier = expectedValue;
            CalendarIdentifier actualValue = calendar.Identifier;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData("2001-02-19")]
        [InlineData("9999-08-04")]
        [InlineData("1234-03-08")]
        [InlineData("1995-02-16")]
        public void MinimumDate_GetAndSet_UsingDateTime(String dateValue)
        {
            SfCalendar calendar = new SfCalendar();

            DateTime expectedValue = DateTime.Parse(dateValue);
            calendar.MinimumDate = expectedValue;
            DateTime actualValue = calendar.MinimumDate;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData("2001-02-19")]
        [InlineData("9999-08-04")]
        [InlineData("1234-03-08")]
        [InlineData("1995-02-16")]
        public void MaximumDate_GetAndSet_UsingDateTime(String dateValue)
        {
            SfCalendar calendar = new SfCalendar();

            DateTime expectedValue = DateTime.Parse(dateValue);
            calendar.MaximumDate = expectedValue;
            DateTime actualValue = calendar.MaximumDate;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(CalendarNavigationDirection.Vertical)]
        [InlineData(CalendarNavigationDirection.Horizontal)]
        public void NavigationDirection_GetAndSet_CalendarNavigationDirection(CalendarNavigationDirection expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.NavigationDirection = expectedValue;
            CalendarNavigationDirection actualValue = calendar.NavigationDirection;

            Assert.Equal(expectedValue, actualValue);
        }
        
        [Theory]
        [InlineData(CalendarRangeSelectionDirection.None)]
        [InlineData(CalendarRangeSelectionDirection.Default)]
        [InlineData(CalendarRangeSelectionDirection.Forward)]
        [InlineData(CalendarRangeSelectionDirection.Backward)]
        [InlineData(CalendarRangeSelectionDirection.Both)]
        public void RangeSelectionDirection_GetAndSet_CalendarRangeSelectionDirection(CalendarRangeSelectionDirection expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.RangeSelectionDirection = expectedValue;
            CalendarRangeSelectionDirection actualValue = calendar.RangeSelectionDirection;

            Assert.Equal(expectedValue, actualValue);
        }
        
        [Fact]
        public void SelectableDayPredicate_GetAndSet_Func()
        {
            SfCalendar calendar = new SfCalendar();

            Func<DateTime, bool> expectedValue = (date) =>
            {
                if (date.Date == DateTime.Now.AddDays(-2).Date || date.Date == DateTime.Now.AddDays(-7).Date || date.Date == DateTime.Now.AddDays(-12).Date || date.Date == DateTime.Now.AddDays(1).Date || date.Date == DateTime.Now.AddDays(15).Date)
                {
                    return false;
                }

                return true;
            };
            calendar.SelectableDayPredicate = expectedValue;
            Func<DateTime, bool> actualValue = calendar.SelectableDayPredicate;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(-2)]
        [InlineData(-7)]
        [InlineData(-12)]
        [InlineData(1)]
        [InlineData(15)]
        public void SelectableDayPredicate_Usage_False(int dateValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.SelectableDayPredicate = (date) =>
            {
                if (date.Date == DateTime.Now.AddDays(-2).Date || date.Date == DateTime.Now.AddDays(-7).Date || date.Date == DateTime.Now.AddDays(-12).Date || date.Date == DateTime.Now.AddDays(1).Date || date.Date == DateTime.Now.AddDays(15).Date)
                {
                    return false;
                }

                return true;
            };

            bool actualValue = calendar.SelectableDayPredicate(DateTime.Now.AddDays(dateValue));

            Assert.False(actualValue);
        }

        [Theory]
        [InlineData(30)]
        [InlineData(40)]
        [InlineData(56)]
        [InlineData(-16)]
        [InlineData(-32)]
        public void SelectableDayPredicate_Usage_True(int dateValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.SelectableDayPredicate = (date) =>
            {
                if (date.Date == DateTime.Now.AddDays(-2).Date || date.Date == DateTime.Now.AddDays(-7).Date || date.Date == DateTime.Now.AddDays(-12).Date || date.Date == DateTime.Now.AddDays(1).Date || date.Date == DateTime.Now.AddDays(15).Date)
                {
                    return false;
                }

                return true;
            };

            bool actualValue = calendar.SelectableDayPredicate(DateTime.Now.AddDays(dateValue));

            Assert.True(actualValue);
        }

        [Theory]
        [InlineData("2001-02-19")]
        [InlineData("9999-08-04")]
        [InlineData("1234-03-08")]
        [InlineData("1995-02-16")]
        public void SelectedDate_GetAndSet_UsingDateTime(String dateValue)
        {
            SfCalendar calendar = new SfCalendar();

            DateTime? expectedValue = DateTime.Parse(dateValue);
            calendar.SelectedDate = expectedValue;
            DateTime? actualValue = calendar.SelectedDate;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData("2001-02-19")]
        [InlineData("9999-08-04")]
        [InlineData("1234-03-08")]
        [InlineData("1995-02-16")]
        public void SelectedDateRange_GetAndSet_EndDate(String dateValue)
        {
            SfCalendar calendar = new SfCalendar();

            DateTime? expectedValue = DateTime.Parse(dateValue);
            calendar.SelectedDateRange = new CalendarDateRange(null, expectedValue);
            DateTime? actualValue = calendar.SelectedDateRange.EndDate;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData("2001-02-19")]
        [InlineData("9999-08-04")]
        [InlineData("1234-03-08")]
        [InlineData("1995-02-16")]
        public void SelectedDateRange_GetAndSet_StartDate(String dateValue)
        {
            SfCalendar calendar = new SfCalendar();

            DateTime? expectedValue = DateTime.Parse(dateValue);
            calendar.SelectedDateRange = new CalendarDateRange(expectedValue, null);
            DateTime? actualValue = calendar.SelectedDateRange.StartDate;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData("2001-02-19")]
        [InlineData("9999-08-04")]
        [InlineData("1234-03-08")]
        [InlineData("1995-02-16")]
        public void SelectedDates_GetAndSet_StartDate(String dateValue)
        {
            SfCalendar calendar = new SfCalendar();

            DateTime expectedValue = DateTime.Parse(dateValue);
            calendar.SelectedDates.Add(expectedValue);
            DateTime actualValue = calendar.SelectedDates[0];

            Assert.Contains(expectedValue, calendar.SelectedDates);
            Assert.Equal(expectedValue, actualValue);
            calendar.SelectedDates.Clear();
        }

        [Theory]
        [InlineData("2001-02-19")]
        [InlineData("9999-08-04")]
        [InlineData("1234-03-08")]
        [InlineData("1995-02-16")]
        public void SelectedDateRanges_GetAndSet_StartDate(String dateValue)
        {
            SfCalendar calendar = new SfCalendar();

            DateTime expectedDate = DateTime.Parse(dateValue);

            CalendarDateRange expectedValue = new CalendarDateRange(expectedDate, null);
            calendar.SelectedDateRanges.Add(new CalendarDateRange(expectedDate, null));
            CalendarDateRange actualValue = calendar.SelectedDateRanges[0];
            
            Assert.Equal(expectedValue.StartDate, actualValue.StartDate);
            calendar.SelectedDateRanges.Clear();
        }

        [Theory]
        [InlineData("2001-02-19")]
        [InlineData("9999-08-04")]
        [InlineData("1234-03-08")]
        [InlineData("1995-02-16")]
        public void SelectedDateRanges_GetAndSet_EndDate(String dateValue)
        {
            SfCalendar calendar = new SfCalendar();

            DateTime expectedDate = DateTime.Parse(dateValue);

            CalendarDateRange expectedValue = new CalendarDateRange(expectedDate, null);
            calendar.SelectedDateRanges.Add(new CalendarDateRange(expectedDate, null));
            CalendarDateRange actualValue = calendar.SelectedDateRanges[0];

            Assert.Equal(expectedValue.EndDate, actualValue.EndDate);
            calendar.SelectedDateRanges.Clear();
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void SelectionBackground_GetAndSet_UsingBrush(byte red, byte green, byte blue)
        {
            SfCalendar calendar = new SfCalendar();

            Brush expectedValue = Color.FromRgb(red, green, blue);
            calendar.SelectionBackground = expectedValue;
            Brush actualValue = calendar.SelectionBackground;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(CalendarSelectionShape.Circle)]
        [InlineData(CalendarSelectionShape.Rectangle)]
        public void SelectionShape_GetAndSet_CalendarSelectionShape(CalendarSelectionShape expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.SelectionShape = expectedValue;
            CalendarSelectionShape actualValue = calendar.SelectionShape;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(CalendarSelectionMode.Single)]
        [InlineData(CalendarSelectionMode.Multiple)]
        [InlineData(CalendarSelectionMode.Range)]
        [InlineData(CalendarSelectionMode.MultiRange)]
        public void SelectionMode_GetAndSet_CalendarSelectionMode(CalendarSelectionMode expectedValue)
        {
            SfCalendar calendar = new SfCalendar();

            calendar.SelectionMode = expectedValue;
            CalendarSelectionMode actualValue = calendar.SelectionMode;

            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void ShowOutOfRangeDates_GetAndSet_WhenTrue()
        {
            SfCalendar calendar = new SfCalendar();

            calendar.ShowOutOfRangeDates = true;
            bool actualValue = calendar.ShowOutOfRangeDates;

            Assert.True(actualValue);
        }

        [Fact]
        public void ShowOutOfRangeDates_GetAndSet_WhenFalse()
        {
            SfCalendar calendar = new SfCalendar();

            calendar.ShowOutOfRangeDates = false;
            bool actualValue = calendar.ShowOutOfRangeDates;

            Assert.False(actualValue);
        }

        [Fact]
        public void ShowTrailingAndLeadingDates_GetAndSet_WhenTrue()
        {
            SfCalendar calendar = new SfCalendar();

            calendar.ShowTrailingAndLeadingDates = true;
            bool actualValue = calendar.ShowTrailingAndLeadingDates;

            Assert.True(actualValue);
        }

        [Fact]
        public void ShowTrailingAndLeadingDates_GetAndSet_WhenFalse()
        {
            SfCalendar calendar = new SfCalendar();

            calendar.ShowTrailingAndLeadingDates = false;
            bool actualValue = calendar.ShowTrailingAndLeadingDates;

            Assert.False(actualValue);
        }

        [Fact]
        public void NavigateToAdjacentMonth_GetAndSet_WhenTrue()
        {
            SfCalendar calendar = new SfCalendar();

            calendar.NavigateToAdjacentMonth = true;
            bool actualValue = calendar.NavigateToAdjacentMonth;

            Assert.True(actualValue);
        }

        [Fact]
        public void NavigateToAdjacentMonth_GetAndSet_WhenFalse()
        {
            SfCalendar calendar = new SfCalendar();

            calendar.NavigateToAdjacentMonth = false;
            bool actualValue = calendar.NavigateToAdjacentMonth;

            Assert.False(actualValue);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void TodayHighlightBrush_GetAndSet_UsingBrush(byte red, byte green, byte blue)
        {
            SfCalendar calendar = new SfCalendar();

            Brush expectedValue = Color.FromRgb(red, green, blue);
            calendar.TodayHighlightBrush = expectedValue;
            Brush actualValue = calendar.TodayHighlightBrush;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void StartRangeSelectionBackground_GetAndSet_UsingBrush(byte red, byte green, byte blue)
        {
            SfCalendar calendar = new SfCalendar();

            Brush expectedValue = Color.FromRgb(red, green, blue);
            calendar.StartRangeSelectionBackground = expectedValue;
            Brush actualValue = calendar.StartRangeSelectionBackground;

            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void MonthView_GetAndSet_Null()
        {
            SfCalendar calendar = new SfCalendar();

            #pragma warning disable CS8625 
            calendar.MonthView = null;
            #pragma warning restore CS8625 

            Assert.Null(calendar.MonthView);
        }

        [Fact]
        public void YearView_GetAndSet_Null()
        {
            SfCalendar calendar = new SfCalendar();

            #pragma warning disable CS8625
            calendar.YearView = null;
            #pragma warning restore CS8625

            Assert.Null(calendar.YearView);
        }

        [Fact]
        public void HeaderView_GetAndSet_Null()
        {
            SfCalendar calendar = new SfCalendar();

            #pragma warning disable CS8625
            calendar.HeaderView = null;
            #pragma warning restore CS8625

            Assert.Null(calendar.HeaderView);
        }

        [Fact]
        public void FooterView_GetAndSet_Null()
        {
            SfCalendar calendar = new SfCalendar();

            #pragma warning disable CS8625
            calendar.FooterView = null;
            #pragma warning restore CS8625

            Assert.Null(calendar.FooterView);
        }

        [Fact]
        public void MonthViewHeaderTemplate_GetAndSet_DataTemplate()
        {
            SfCalendar calendar = new SfCalendar();


            // Create the Grid
            var grid = new Grid
            {
                BackgroundColor = Color.FromArgb("#BB9AB1")
            };

            // Create the Label
            var label = new Label
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                TextColor = Colors.White,
                FontSize = 14,
                FontFamily = "Bold"
            };
            label.SetBinding(Label.TextProperty, new Binding
            {
                StringFormat = "{0:ddd}"
            });
            grid.Children.Add(label);
            DataTemplate expectedValues = new DataTemplate(() => grid);
            calendar.MonthViewHeaderTemplate = expectedValues;
            DataTemplate actualValue = calendar.MonthViewHeaderTemplate;

            Assert.Equal(expectedValues, actualValue);
        }

        [Fact]
        public void HeaderTemplate_GetAndSet_DataTemplate()
        {
            SfCalendar calendar = new SfCalendar();


            // Create the Grid
            var grid = new Grid
            {
                BackgroundColor = Color.FromArgb("#BB9AB1")
            };

            // Create the Label
            var label = new Label
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                TextColor = Colors.White,
                FontSize = 14,
                FontFamily = "Bold"
            };
            label.SetBinding(Label.TextProperty, new Binding
            {
                StringFormat = "{0:ddd}"
            });
            grid.Children.Add(label);
            DataTemplate expectedValues = new DataTemplate(() => grid);
            calendar.HeaderTemplate = expectedValues;
            DataTemplate actualValue = calendar.HeaderTemplate;

            Assert.Equal(expectedValues, actualValue);
        }

		[Theory]
		[InlineData(CalendarView.Month)]
		[InlineData(CalendarView.Year)]
		[InlineData(CalendarView.Decade)]
		[InlineData(CalendarView.Century)]
		public void SelectionCellTemplate_GetAndSet_DataTemplate(CalendarView view)
		{
			SfCalendar calendar = new SfCalendar() { View = view };

			// Create the Grid
			var grid = new Grid
			{
				BackgroundColor = Colors.Green
			};

			// Create the Label
			var label = new Label
			{
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center,
				TextColor = Colors.Black,
				Text = "Custom View"
			};
			grid.Children.Add(label);
			DataTemplate expectedValue = new DataTemplate(() => grid);
			calendar.SelectionCellTemplate = expectedValue;
			DataTemplate actualValue = calendar.SelectionCellTemplate;
			Assert.Equal(expectedValue, actualValue);
		}

		[Fact]
		public void SelectionCellTemplate_CreatesView_WhenCalled()
		{
			SfCalendar calendar = new SfCalendar();

			// Create the Grid
			var grid = new Grid
			{
				BackgroundColor = Colors.Green
			};

			// Create the Label
			var label = new Label
			{
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center,
				TextColor = Colors.Black,
				Text = "Custom View"
			};
			grid.Children.Add(label);
			calendar.SelectionCellTemplate = new DataTemplate(() => grid);

			List<DateTime> visibleDates = new List<DateTime>();
			visibleDates.Add(new DateTime(2025, 03, 19));
			visibleDates.Add(new DateTime(2025, 03, 20));

			List<DateTime> disableDates = new List<DateTime>();
			disableDates.Add(new DateTime(2025, 03, 19));

			List<CalendarIconDetails> CalendarIconDetail = new List<CalendarIconDetails>();
			CalendarIconDetail.Add(new CalendarIconDetails() { Fill = Colors.Yellow });

			MonthView monthView = new MonthView(calendar, visibleDates, new DateTime(2025, 03, 20), disableDates, CalendarIconDetail, true);
			var details = InvokePrivateMethod(monthView, "GetMonthCellDetails", new DateTime(2025, 03, 20).Month, new DateTime(2025, 03, 20));
			CalendarCellDetails? selectedCellDetail = null;
			if (details != null)
			{
				selectedCellDetail = (CalendarCellDetails)details;
			}
			if (selectedCellDetail != null)
			{
				View? view = CalendarViewHelper.CreateSelectionCellTemplate(new DateTime(2025, 03, 20), calendar.SelectionCellTemplate, calendar.MonthView, selectedCellDetail, new RectF(10, 20, 40, 20));
				Assert.NotNull(view);
			}
		}

		[Fact]
		public void SelectionCellTemplate_CreatesView_WhenCalled_WithoutSelectionCellTemplate()
		{
			SfCalendar calendar = new SfCalendar();

			List<DateTime> visibleDates = new List<DateTime>();
			visibleDates.Add(new DateTime(2025, 03, 19));
			visibleDates.Add(new DateTime(2025, 03, 20));

			List<DateTime> disableDates = new List<DateTime>();
			disableDates.Add(new DateTime(2025, 03, 19));

			List<CalendarIconDetails> CalendarIconDetail = new List<CalendarIconDetails>();
			CalendarIconDetail.Add(new CalendarIconDetails() { Fill = Colors.Yellow });

			MonthView monthView = new MonthView(calendar, visibleDates, new DateTime(2025, 03, 20), disableDates, CalendarIconDetail, true);
			var details = InvokePrivateMethod(monthView, "GetMonthCellDetails", new DateTime(2025, 03, 20).Month, new DateTime(2025, 03, 20));

			CalendarCellDetails? selectedCellDetail = null;
			if (details != null)
			{
				selectedCellDetail = (CalendarCellDetails)details;
			}
			if (selectedCellDetail != null)
			{
				View? view = CalendarViewHelper.CreateSelectionCellTemplate(new DateTime(2025, 03, 20), calendar.SelectionCellTemplate, calendar.MonthView, selectedCellDetail, new RectF(10, 20, 40, 20));
				Assert.Null(view);
			}
		}

		[Theory]
		[InlineData(CalendarView.Year)]
		[InlineData(CalendarView.Decade)]
		[InlineData(CalendarView.Century)]
		public void SelectionCellTemplate_CreatesView_WhenCalled_YearView(CalendarView viewOfYear)
		{
			SfCalendar calendar = new SfCalendar() { View = viewOfYear };

			// Create the Grid
			var grid = new Grid
			{
				BackgroundColor = Colors.Green
			};

			// Create the Label
			var label = new Label
			{
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center,
				TextColor = Colors.Black,
				Text = "Custom View"
			};
			grid.Children.Add(label);
			calendar.SelectionCellTemplate = new DataTemplate(() => grid);

			List<DateTime> visibleDates = new List<DateTime>();
			visibleDates.Add(new DateTime(2025, 03, 01));
			visibleDates.Add(new DateTime(2025, 04, 01));
			visibleDates.Add(new DateTime(2025, 05, 01));

			List<DateTime> disableDates = new List<DateTime>();
			disableDates.Add(new DateTime(2025, 03, 01));

			YearView yearView = new YearView(calendar, visibleDates, disableDates, true);
			var details = InvokePrivateMethod(yearView, "GetYearCellDetails", new DateTime(2025, 04, 01), visibleDates[0]);
			CalendarCellDetails? selectedCellDetail = null;
			if (details != null)
			{
				selectedCellDetail = (CalendarCellDetails)details;
			}
			if (selectedCellDetail != null)
			{
				View? view = CalendarViewHelper.CreateSelectionCellTemplate(new DateTime(2025, 03, 20), calendar.SelectionCellTemplate, calendar.MonthView, selectedCellDetail, new RectF(10, 20, 40, 20));
				Assert.NotNull(view);
			}
		}

		[Theory]
		[InlineData(CalendarView.Year)]
		[InlineData(CalendarView.Decade)]
		[InlineData(CalendarView.Century)]
		public void SelectionCellTemplate_CreatesView_WhenCalled_OnYearView_WithoutSelectionCellTemplate(CalendarView viewOfYear)
		{
			SfCalendar calendar = new SfCalendar() { View = viewOfYear };

			List<DateTime> visibleDates = new List<DateTime>();
			visibleDates.Add(new DateTime(2025, 03, 01));
			visibleDates.Add(new DateTime(2025, 04, 01));
			visibleDates.Add(new DateTime(2025, 05, 01));

			List<DateTime> disableDates = new List<DateTime>();
			disableDates.Add(new DateTime(2025, 03, 01));

			YearView yearView = new YearView(calendar, visibleDates, disableDates, true);
			var details = InvokePrivateMethod(yearView, "GetYearCellDetails", new DateTime(2025, 04, 01), visibleDates[0]);
			CalendarCellDetails? selectedCellDetail = null;
			if (details != null)
			{
				selectedCellDetail = (CalendarCellDetails)details;
			}
			if (selectedCellDetail != null)
			{
				View? view = CalendarViewHelper.CreateSelectionCellTemplate(new DateTime(2025, 03, 20), calendar.SelectionCellTemplate, calendar.MonthView, selectedCellDetail, new RectF(10, 20, 40, 20));
				Assert.Null(view);
			}
		}


		#endregion

		#region Events

		[Fact]
        public void SelectionChangedInvoked()
        {
            SfCalendar calendar = new SfCalendar();
            var fired = false;
            calendar.SelectionChanged += (sender, e) => fired = true;
            calendar.SelectedDate = DateTime.Now.AddDays(30);
            Assert.True(fired);
        }

        [Fact]
        public void ViewChangedInvoked()
        {
            SfCalendar calendar = new SfCalendar();
            
            calendar.View = CalendarView.Month;
            var fired = false;
            calendar.ViewChanged += (sender, e) => fired = true;
            CustomSnapLayout customSnapLayout = new CustomSnapLayout(calendar);
            InvokePrivateMethod(customSnapLayout, "CreateOrResetVisibleDates", DateTime.Now);
            InvokePrivateMethod(customSnapLayout, "CreateOrResetVisibleDates", DateTime.Now.AddMonths(2));

            Assert.True(fired);
        }


        [Fact]
        public void TappedInvoked()
        {
            SfCalendar calendar = new SfCalendar();

            var fired = false;
            calendar.Tapped += (sender, e) => fired = true;
            ICalendar calendarView = calendar;
            calendarView.TriggerCalendarInteractionEvent(true, 1, DateTime.Now, CalendarElement.WeekNumber);

            Assert.True(fired);
        }

        [Fact]
        public void DoubleTappedInvoked()
        {
            SfCalendar calendar = new SfCalendar();

            var fired = false;
            calendar.DoubleTapped += (sender, e) => fired = true;
            ICalendar calendarView = calendar;
            calendarView.TriggerCalendarInteractionEvent(true, 2, DateTime.Now, CalendarElement.WeekNumber);

            Assert.True(fired);
        }

        [Fact]
        public void LongPressedInvoked()
        {
            SfCalendar calendar = new SfCalendar();

            var fired = false;
            calendar.LongPressed += (sender, e) => fired = true;
            ICalendar calendarView = calendar;
            calendarView.TriggerCalendarInteractionEvent(false, 1, DateTime.Now, CalendarElement.WeekNumber);

            Assert.True(fired);
        }

        #endregion

        #region Commands

        [Fact]
        public void ViewChangedCommand_Execute_ReturnsTrue()
        {
            SfCalendar calendar = new SfCalendar();
            
            bool commandExecuted = false;
            ICommand expectedValue = new Command(() =>
            {
                commandExecuted = true;
            });
            calendar.ViewChangedCommand = expectedValue;
            calendar.ViewChangedCommand.Execute(null);
            ICommand actualValue = calendar.ViewChangedCommand;

            Assert.True(commandExecuted);
            Assert.Same(expectedValue, actualValue);
        }

        [Fact]
        public void SelectionChangedCommand_Execute_ReturnsTrue()
        {
            SfCalendar calendar = new SfCalendar();

            bool commandExecuted = false;
            ICommand expectedValue = new Command(() =>
            {
                commandExecuted = true;
            });
            calendar.SelectionChangedCommand = expectedValue;
            calendar.SelectionChangedCommand.Execute(null);
            ICommand actualValue = calendar.SelectionChangedCommand;

            Assert.True(commandExecuted);
            Assert.Same(expectedValue, actualValue);
        }

        [Fact]
        public void TappedCommand_Execute_ReturnsTrue()
        {
            SfCalendar calendar = new SfCalendar();

            bool commandExecuted = false;
            ICommand expectedValue = new Command(() =>
            {
                commandExecuted = true;
            });
            calendar.TappedCommand = expectedValue;
            calendar.TappedCommand.Execute(null);
            ICommand actualValue = calendar.TappedCommand;

            Assert.True(commandExecuted);
            Assert.Same(expectedValue, actualValue);
        }

        [Fact]
        public void DoubleTappedCommand_Execute_ReturnsTrue()
        {
            SfCalendar calendar = new SfCalendar();

            bool commandExecuted = false;
            ICommand expectedValue = new Command(() =>
            {
                commandExecuted = true;
            });
            calendar.DoubleTappedCommand = expectedValue;
            calendar.DoubleTappedCommand.Execute(null);
            ICommand actualValue = calendar.DoubleTappedCommand;

            Assert.True(commandExecuted);
            Assert.Same(expectedValue, actualValue);
        }

        [Fact]
        public void LongPressedCommand_Execute_ReturnsTrue()
        {
            SfCalendar calendar = new SfCalendar();

            bool commandExecuted = false;
            ICommand expectedValue = new Command(() =>
            {
                commandExecuted = true;
            });
            calendar.LongPressedCommand = expectedValue;
            calendar.LongPressedCommand.Execute(null);
            ICommand actualValue = calendar.LongPressedCommand;

            Assert.True(commandExecuted);
            Assert.Same(expectedValue, actualValue);
        }

		#endregion

		#region PopupSize Feature

		[Fact]
		public void SfCalendar_SfPopupSize()
		{
			SfCalendar calendar = new SfCalendar();
			double expectedPopupWidth = 200;
			double expectedPopupHeight = 200;
			calendar.PopupWidth = expectedPopupWidth;
			calendar.PopupHeight = expectedPopupHeight;
			double actualPopupWidth = calendar.PopupWidth;
			double actualPopupHeight = calendar.PopupHeight;
			Assert.Equal(expectedPopupWidth, actualPopupWidth);
			Assert.Equal(expectedPopupHeight, actualPopupHeight);
		}

		[Fact]
		public void SfCalendar_OnPropertyChangedSfPopupWidthAndHeight()
		{
			SfCalendar calendar = new SfCalendar();
			double expectedPopupWidth = 200;
			double expectedPopupHeight = 400;
			calendar.PopupWidth = 100;
			calendar.PopupHeight = 500;
			calendar.PopupWidth = expectedPopupWidth;
			calendar.PopupHeight = expectedPopupHeight;
			double actualPopupWidth = calendar.PopupWidth;
			double actualPopupHeight = calendar.PopupHeight;
			Assert.Equal(expectedPopupWidth, actualPopupWidth);
			Assert.Equal(expectedPopupHeight, actualPopupHeight);
		}

		[Fact]
		public void SfCalendar_SfPopupWidthAndHeight_UsingHeaderHeight()
		{
			SfCalendar calendar = new SfCalendar();
			CalendarHeaderView headerView = calendar.HeaderView;
			double expectedHeight = 50;
			headerView.Height = expectedHeight;
			double expectedPopupWidth = 200;
			double expectedPopupHeight = 400;
			calendar.PopupWidth = expectedPopupWidth;
			calendar.PopupHeight = expectedPopupHeight;
			calendar.HeaderView = headerView;
			double actualPopupWidth = calendar.PopupWidth;
			double actualPopupHeight = calendar.PopupHeight;
			double actualHeight = calendar.HeaderView.Height;
			Assert.Equal(expectedPopupWidth, actualPopupWidth);
			Assert.Equal(expectedPopupHeight, actualPopupHeight);
			Assert.Equal(expectedHeight, actualHeight);
		}

		[Fact]
		public void SfCalendar_DialogMode()
		{
			SfCalendar calendar = new SfCalendar();
			double expectedPopupWidth = 200;
			double expectedPopupHeight = 400;
			CalendarMode expectedMode = CalendarMode.Dialog;
			calendar.PopupWidth = expectedPopupWidth;
			calendar.PopupHeight = expectedPopupHeight;
			calendar.Mode = expectedMode;
			double actualPopupWidth = calendar.PopupWidth;
			double actualPopupHeight = calendar.PopupHeight;
			CalendarMode actualMode = calendar.Mode;
			Assert.Equal(expectedPopupWidth, actualPopupWidth);
			Assert.Equal(expectedPopupHeight, actualPopupHeight);
			Assert.Equal(expectedMode, actualMode);
		}

		[Fact]
		public void SfCalendar_AlignTop()
		{
			SfCalendar calendar = new SfCalendar();
			double expectedPopupWidth = 200;
			double expectedPopupHeight = 400;
			CalendarMode expectedMode = CalendarMode.RelativeDialog;
			CalendarRelativePosition expectedRelativePosition = CalendarRelativePosition.AlignTop;
			calendar.PopupWidth = expectedPopupWidth;
			calendar.PopupHeight = expectedPopupHeight;
			calendar.Mode = expectedMode;
			calendar.RelativePosition = expectedRelativePosition;
			double actualPopupWidth = calendar.PopupWidth;
			double actualPopupHeight = calendar.PopupHeight;
			CalendarMode actualMode = calendar.Mode;
			CalendarRelativePosition actualRelativePosition = calendar.RelativePosition;
			Assert.Equal(expectedPopupWidth, actualPopupWidth);
			Assert.Equal(expectedPopupHeight, actualPopupHeight);
			Assert.Equal(expectedMode, actualMode);
			Assert.Equal(expectedRelativePosition, actualRelativePosition);
		}

		[Fact]
		public void SfCalendar_AlignToLeftOf()
		{
			SfCalendar calendar = new SfCalendar();
			double expectedPopupWidth = 200;
			double expectedPopupHeight = 400;
			CalendarMode expectedMode = CalendarMode.RelativeDialog;
			CalendarRelativePosition expectedRelativePosition = CalendarRelativePosition.AlignToLeftOf;
			calendar.PopupWidth = expectedPopupWidth;
			calendar.PopupHeight = expectedPopupHeight;
			calendar.Mode = expectedMode;
			calendar.RelativePosition = expectedRelativePosition;
			double actualPopupWidth = calendar.PopupWidth;
			double actualPopupHeight = calendar.PopupHeight;
			CalendarMode actualMode = calendar.Mode;
			CalendarRelativePosition actualRelativePosition = calendar.RelativePosition;
			Assert.Equal(expectedPopupWidth, actualPopupWidth);
			Assert.Equal(expectedPopupHeight, actualPopupHeight);
			Assert.Equal(expectedMode, actualMode);
			Assert.Equal(expectedRelativePosition, actualRelativePosition);
		}

		[Fact]
		public void SfCalendar_AlignToRightOf()
		{
			SfCalendar calendar = new SfCalendar();
			double expectedPopupWidth = 200;
			double expectedPopupHeight = 400;
			CalendarMode expectedMode = CalendarMode.RelativeDialog;
			CalendarRelativePosition expectedRelativePosition = CalendarRelativePosition.AlignToRightOf;
			calendar.PopupWidth = expectedPopupWidth;
			calendar.PopupHeight = expectedPopupHeight;
			calendar.Mode = expectedMode;
			calendar.RelativePosition = expectedRelativePosition;
			double actualPopupWidth = calendar.PopupWidth;
			double actualPopupHeight = calendar.PopupHeight;
			CalendarMode actualMode = calendar.Mode;
			CalendarRelativePosition actualRelativePosition = calendar.RelativePosition;
			Assert.Equal(expectedPopupWidth, actualPopupWidth);
			Assert.Equal(expectedPopupHeight, actualPopupHeight);
			Assert.Equal(expectedMode, actualMode);
			Assert.Equal(expectedRelativePosition, actualRelativePosition);
		}

		[Fact]
		public void SfCalendar_AlignBottom()
		{
			SfCalendar calendar = new SfCalendar();
			double expectedPopupWidth = 200;
			double expectedPopupHeight = 400;
			CalendarMode expectedMode = CalendarMode.RelativeDialog;
			CalendarRelativePosition expectedRelativePosition = CalendarRelativePosition.AlignBottom;
			calendar.PopupWidth = expectedPopupWidth;
			calendar.PopupHeight = expectedPopupHeight;
			calendar.Mode = expectedMode;
			calendar.RelativePosition = expectedRelativePosition;
			double actualPopupWidth = calendar.PopupWidth;
			double actualPopupHeight = calendar.PopupHeight;
			CalendarMode actualMode = calendar.Mode;
			CalendarRelativePosition actualRelativePosition = calendar.RelativePosition;
			Assert.Equal(expectedPopupWidth, actualPopupWidth);
			Assert.Equal(expectedPopupHeight, actualPopupHeight);
			Assert.Equal(expectedMode, actualMode);
			Assert.Equal(expectedRelativePosition, actualRelativePosition);
		}

		[Fact]
		public void SfCalendar_AlignTopLeft()
		{
			SfCalendar calendar = new SfCalendar();
			double expectedPopupWidth = 200;
			double expectedPopupHeight = 400;
			CalendarMode expectedMode = CalendarMode.RelativeDialog;
			CalendarRelativePosition expectedRelativePosition = CalendarRelativePosition.AlignTopLeft;
			calendar.PopupWidth = expectedPopupWidth;
			calendar.PopupHeight = expectedPopupHeight;
			calendar.Mode = expectedMode;
			calendar.RelativePosition = expectedRelativePosition;
			double actualPopupWidth = calendar.PopupWidth;
			double actualPopupHeight = calendar.PopupHeight;
			CalendarMode actualMode = calendar.Mode;
			CalendarRelativePosition actualRelativePosition = calendar.RelativePosition;
			Assert.Equal(expectedPopupWidth, actualPopupWidth);
			Assert.Equal(expectedPopupHeight, actualPopupHeight);
			Assert.Equal(expectedMode, actualMode);
			Assert.Equal(expectedRelativePosition, actualRelativePosition);
		}

		[Fact]
		public void SfCalendar_AlignTopRight()
		{
			SfCalendar calendar = new SfCalendar();
			double expectedPopupWidth = 200;
			double expectedPopupHeight = 400;
			CalendarMode expectedMode = CalendarMode.RelativeDialog;
			CalendarRelativePosition expectedRelativePosition = CalendarRelativePosition.AlignTopRight;
			calendar.PopupWidth = expectedPopupWidth;
			calendar.PopupHeight = expectedPopupHeight;
			calendar.Mode = expectedMode;
			calendar.RelativePosition = expectedRelativePosition;
			double actualPopupWidth = calendar.PopupWidth;
			double actualPopupHeight = calendar.PopupHeight;
			CalendarMode actualMode = calendar.Mode;
			CalendarRelativePosition actualRelativePosition = calendar.RelativePosition;
			Assert.Equal(expectedPopupWidth, actualPopupWidth);
			Assert.Equal(expectedPopupHeight, actualPopupHeight);
			Assert.Equal(expectedMode, actualMode);
			Assert.Equal(expectedRelativePosition, actualRelativePosition);
		}

		[Fact]
		public void SfCalendar_AlignBottomLeft()
		{
			SfCalendar calendar = new SfCalendar();
			double expectedPopupWidth = 200;
			double expectedPopupHeight = 400;
			CalendarMode expectedMode = CalendarMode.RelativeDialog;
			CalendarRelativePosition expectedRelativePosition = CalendarRelativePosition.AlignBottomLeft;
			calendar.PopupWidth = expectedPopupWidth;
			calendar.PopupHeight = expectedPopupHeight;
			calendar.Mode = expectedMode;
			calendar.RelativePosition = expectedRelativePosition;
			double actualPopupWidth = calendar.PopupWidth;
			double actualPopupHeight = calendar.PopupHeight;
			CalendarMode actualMode = calendar.Mode;
			CalendarRelativePosition actualRelativePosition = calendar.RelativePosition;
			Assert.Equal(expectedPopupWidth, actualPopupWidth);
			Assert.Equal(expectedPopupHeight, actualPopupHeight);
			Assert.Equal(expectedMode, actualMode);
			Assert.Equal(expectedRelativePosition, actualRelativePosition);
		}

		[Fact]
		public void SfCalendar_AlignBottomRight()
		{
			SfCalendar calendar = new SfCalendar();
			double expectedPopupWidth = 200;
			double expectedPopupHeight = 400;
			CalendarMode expectedMode = CalendarMode.RelativeDialog;
			CalendarRelativePosition expectedRelativePosition = CalendarRelativePosition.AlignBottomRight;
			calendar.PopupWidth = expectedPopupWidth;
			calendar.PopupHeight = expectedPopupHeight;
			calendar.Mode = expectedMode;
			calendar.RelativePosition = expectedRelativePosition;
			double actualPopupWidth = calendar.PopupWidth;
			double actualPopupHeight = calendar.PopupHeight;
			CalendarMode actualMode = calendar.Mode;
			CalendarRelativePosition actualRelativePosition = calendar.RelativePosition;
			Assert.Equal(expectedPopupWidth, actualPopupWidth);
			Assert.Equal(expectedPopupHeight, actualPopupHeight);
			Assert.Equal(expectedMode, actualMode);
			Assert.Equal(expectedRelativePosition, actualRelativePosition);
		}

		#endregion

		#region Autofit Scenario's
		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void ShowTrailingAndLeadingDates_GetAndSet(bool expected)
		{
			var calendar = new SfCalendar();
			calendar.Mode = CalendarMode.Dialog;
			calendar.ShowTrailingAndLeadingDates = expected;
			Assert.Equal(expected, calendar.ShowTrailingAndLeadingDates);
		}

		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(3)]
		[InlineData(4)]
		[InlineData(5)]
		[InlineData(6)]
		public void Autofit_WithNumberOfVisibleWeeks(int weeks)
		{
			var calendar = new SfCalendar();
			calendar.MonthView.NumberOfVisibleWeeks = weeks;
			calendar.Mode = CalendarMode.Dialog;
			calendar.ShowTrailingAndLeadingDates = false;
			Assert.Equal(weeks, calendar.MonthView.NumberOfVisibleWeeks);
			calendar.ShowTrailingAndLeadingDates = true;
			Assert.Equal(weeks, calendar.MonthView.NumberOfVisibleWeeks);
		}

		[Theory]
		[InlineData(CalendarMode.Default, 200)]
		[InlineData(CalendarMode.Dialog, 320)]
		[InlineData(CalendarMode.RelativeDialog, 420)]
		public void Autofit_WithPopupMode_And_PopupHeight(CalendarMode mode, double popupHeight)
		{
			var calendar = new SfCalendar();
			calendar.Mode = mode;
			calendar.PopupHeight = popupHeight;
			calendar.ShowTrailingAndLeadingDates = false;
			Assert.Equal(mode, calendar.Mode);
			Assert.Equal(popupHeight, calendar.PopupHeight);
			Assert.False(calendar.ShowTrailingAndLeadingDates);
			calendar.ShowTrailingAndLeadingDates = true;
			Assert.Equal(mode, calendar.Mode);
			Assert.Equal(popupHeight, calendar.PopupHeight);
			Assert.True(calendar.ShowTrailingAndLeadingDates);
		}

		[Theory]
		[InlineData(DayOfWeek.Sunday)]
		[InlineData(DayOfWeek.Monday)]
		[InlineData(DayOfWeek.Tuesday)]
		[InlineData(DayOfWeek.Wednesday)]
		[InlineData(DayOfWeek.Thursday)]
		[InlineData(DayOfWeek.Friday)]
		[InlineData(DayOfWeek.Saturday)]
		public void Autofit_WithFirstDayOfWeek(DayOfWeek firstDay)
		{
			var calendar = new SfCalendar();
			calendar.MonthView.FirstDayOfWeek = firstDay;
			calendar.Mode = CalendarMode.Dialog;
			calendar.ShowTrailingAndLeadingDates = false;
			Assert.Equal(firstDay, calendar.MonthView.FirstDayOfWeek);
			calendar.ShowTrailingAndLeadingDates = true;
			Assert.Equal(firstDay, calendar.MonthView.FirstDayOfWeek);
		}

		[Theory]
		[InlineData("2001-01-01", "2099-12-31", "2025-06-15")]
		[InlineData("1990-05-05", "2030-08-08", "2000-01-01")]
		public void Autofit_WithDisplay_AndMinimumn_AndMaximum(string min, string max, string display)
		{
			var calendar = new SfCalendar();
			calendar.MinimumDate = DateTime.Parse(min);
			calendar.MaximumDate = DateTime.Parse(max);
			calendar.DisplayDate = DateTime.Parse(display);
			calendar.Mode = CalendarMode.Dialog;
			calendar.ShowTrailingAndLeadingDates = false;
			Assert.Equal(DateTime.Parse(min), calendar.MinimumDate);
			Assert.Equal(DateTime.Parse(max), calendar.MaximumDate);
			Assert.Equal(DateTime.Parse(display), calendar.DisplayDate);
			calendar.ShowTrailingAndLeadingDates = true;
			Assert.Equal(DateTime.Parse(min), calendar.MinimumDate);
			Assert.Equal(DateTime.Parse(max), calendar.MaximumDate);
			Assert.Equal(DateTime.Parse(display), calendar.DisplayDate);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void Autofit_WithShowWeekNumber(bool showWeekNumber)
		{
			var calendar = new SfCalendar();
			calendar.MonthView.ShowWeekNumber = showWeekNumber;
			calendar.Mode = CalendarMode.Dialog;
			calendar.ShowTrailingAndLeadingDates = false;
			Assert.Equal(showWeekNumber, calendar.MonthView.ShowWeekNumber);
			calendar.ShowTrailingAndLeadingDates = true;
			Assert.Equal(showWeekNumber, calendar.MonthView.ShowWeekNumber);
		}

		[Theory]
		[InlineData(CalendarView.Decade)]
		[InlineData(CalendarView.Year)]
		[InlineData(CalendarView.Month)]
		[InlineData(CalendarView.Century)]
		public void Autofit_WithView(CalendarView view)
		{
			var calendar = new SfCalendar();
			calendar.View = view;
			calendar.ShowTrailingAndLeadingDates = false;
			calendar.Mode = CalendarMode.Dialog;
			Assert.Equal(view, calendar.View);
		}

		[Theory]
		[InlineData(CalendarIdentifier.Hijri)]
		[InlineData(CalendarIdentifier.Persian)]
		[InlineData(CalendarIdentifier.Korean)]
		[InlineData(CalendarIdentifier.ThaiBuddhist)]
		[InlineData(CalendarIdentifier.Gregorian)]
		[InlineData(CalendarIdentifier.UmAlQura)]
		[InlineData(CalendarIdentifier.Taiwan)]
		public void Autofit_WithIdentifier(CalendarIdentifier identifier)
		{
			var calendar = new SfCalendar();
			calendar.Identifier = identifier;
			calendar.ShowTrailingAndLeadingDates = false;
			calendar.Mode = CalendarMode.Dialog;
			Assert.Equal(identifier, calendar.Identifier);
		}

		#endregion

		#region Additional Public Property Tests (Autofit-related and Navigation)

		[Fact]
		public void Autofit_WithMonthView_CellTemplate()
		{
			var calendar = new SfCalendar();
			var grid = new Grid { BackgroundColor = Color.FromArgb("#123456") };
			var label = new Label { TextColor = Colors.White, FontSize = 12, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center };
			label.SetBinding(Label.TextProperty, new Binding { StringFormat = "{0:dd}" });
			grid.Children.Add(label);
			var template = new DataTemplate(() => grid);
			calendar.MonthView.CellTemplate = template;
			calendar.Mode = CalendarMode.Dialog;
			calendar.ShowTrailingAndLeadingDates = false;
			Assert.False(calendar.ShowTrailingAndLeadingDates);
			Assert.Equal(template, calendar.MonthView.CellTemplate);
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
			calendar.MonthView.CellTemplate = null;
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
			Assert.False(calendar.ShowTrailingAndLeadingDates);
			Assert.Null(calendar.MonthView.CellTemplate);
		}

		[Theory]
		[InlineData(CalendarSelectionMode.Single)]
		[InlineData(CalendarSelectionMode.Multiple)]
		[InlineData(CalendarSelectionMode.Range)]
		[InlineData(CalendarSelectionMode.MultiRange)]
		public void Autofit_WithSelectionMode(CalendarSelectionMode mode)
		{
			var calendar = new SfCalendar();
			calendar.SelectionMode = mode;
			calendar.ShowTrailingAndLeadingDates = false;
			calendar.Mode = CalendarMode.Dialog;
			Assert.False(calendar.ShowTrailingAndLeadingDates);
			Assert.Equal(mode, calendar.SelectionMode);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void Autofit_WithNavigateToAdjacentMonth(bool expected)
		{
			var calendar = new SfCalendar();
			calendar.NavigateToAdjacentMonth = expected;
			calendar.ShowTrailingAndLeadingDates = false;
			calendar.Mode = CalendarMode.Dialog;
			Assert.False(calendar.ShowTrailingAndLeadingDates);
			Assert.Equal(expected, calendar.NavigateToAdjacentMonth);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void Autofit_WithAllowViewNavigation(bool expected)
		{
			var calendar = new SfCalendar();
			calendar.AllowViewNavigation = expected;
			calendar.ShowTrailingAndLeadingDates = false;
			calendar.Mode = CalendarMode.RelativeDialog;
			Assert.False(calendar.ShowTrailingAndLeadingDates);
			Assert.Equal(expected, calendar.AllowViewNavigation);
		}

		[Theory]
		[InlineData(CalendarNavigationDirection.Horizontal)]
		[InlineData(CalendarNavigationDirection.Vertical)]
		public void Autofit_WithNavigationDirection(CalendarNavigationDirection direction)
		{
			var calendar = new SfCalendar();
			calendar.NavigationDirection = direction;
			calendar.ShowTrailingAndLeadingDates = false;
			calendar.Mode = CalendarMode.RelativeDialog;
			Assert.False(calendar.ShowTrailingAndLeadingDates);
			Assert.Equal(direction, calendar.NavigationDirection);
		}

		[Theory]
		[InlineData(CalendarRangeSelectionDirection.Default)]
		[InlineData(CalendarRangeSelectionDirection.Forward)]
		[InlineData(CalendarRangeSelectionDirection.Backward)]
		[InlineData(CalendarRangeSelectionDirection.Both)]
		[InlineData(CalendarRangeSelectionDirection.None)]
		public void Autofit_WithRangeSelectionDirection(CalendarRangeSelectionDirection direction)
		{
			var calendar = new SfCalendar();
			calendar.RangeSelectionDirection = direction;
			calendar.ShowTrailingAndLeadingDates = false;
			calendar.Mode = CalendarMode.RelativeDialog;
			Assert.False(calendar.ShowTrailingAndLeadingDates);
			Assert.Equal(direction, calendar.RangeSelectionDirection);
		}

		[Fact]
		public void Autofit_WithSelectableDayPredicate()
		{
			var calendar = new SfCalendar();
			Func<DateTime, bool> predicate = d => d.Day % 2 == 0; // even days selectable
			calendar.SelectableDayPredicate = predicate;
			calendar.ShowTrailingAndLeadingDates = false;
			Assert.False(calendar.ShowTrailingAndLeadingDates);
			Assert.Equal(predicate, calendar.SelectableDayPredicate);
			calendar.Mode = CalendarMode.RelativeDialog;
			Assert.True(calendar.SelectableDayPredicate(DateTime.Parse("2025-01-02")));
			Assert.False(calendar.SelectableDayPredicate(DateTime.Parse("2025-01-03")));
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void Autofit_WithMonthView_ShowWeekNumber(bool expected)
		{
			var calendar = new SfCalendar();
			calendar.MonthView.ShowWeekNumber = expected;
			calendar.ShowTrailingAndLeadingDates = false;
			Assert.False(calendar.ShowTrailingAndLeadingDates);
			calendar.Mode = CalendarMode.RelativeDialog;
			Assert.Equal(expected, calendar.MonthView.ShowWeekNumber);
		}

		[Theory]
		[InlineData(10)]
		[InlineData(0)]
		[InlineData(-10)]
		[InlineData(85)]
		public void Autofit_WithHeaderView_Height(double height)
		{
			var calendar = new SfCalendar();
			calendar.HeaderView.Height = height;
			calendar.ShowTrailingAndLeadingDates = false;
			Assert.False(calendar.ShowTrailingAndLeadingDates);
			calendar.Mode = CalendarMode.RelativeDialog;
			Assert.Equal(height, calendar.HeaderView.Height);
		}

		[Theory]
		[InlineData(20)]
		[InlineData(0)]
		[InlineData(-10)]
		[InlineData(85)]
		public void Autofit_WithMonthHeaderView_Height(double height)
		{
			var calendar = new SfCalendar();
			calendar.MonthView.HeaderView.Height = height;
			calendar.ShowTrailingAndLeadingDates = false;
			calendar.Mode = CalendarMode.RelativeDialog;
			Assert.False(calendar.ShowTrailingAndLeadingDates);
			Assert.Equal(height, calendar.MonthView.HeaderView.Height);
		}

		[Theory]
		[InlineData(20)]
		[InlineData(0)]
		[InlineData(-10)]
		[InlineData(85)]
		public void Autofit_WithFooterView_Height(double height)
		{
			var calendar = new SfCalendar();
			calendar.FooterView.Height = height;
			calendar.ShowTrailingAndLeadingDates = false;
			calendar.Mode = CalendarMode.RelativeDialog;
			Assert.False(calendar.ShowTrailingAndLeadingDates);
			Assert.Equal(height, calendar.FooterView.Height);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void Autofit_WithEnableSwipeSelection(bool expected)
		{
			var calendar = new SfCalendar();
			calendar.EnableSwipeSelection = expected;
			calendar.ShowTrailingAndLeadingDates = false;
			calendar.Mode = CalendarMode.RelativeDialog;
			Assert.False(calendar.ShowTrailingAndLeadingDates);
			Assert.Equal(expected, calendar.EnableSwipeSelection);
		}

		[Theory]
		[InlineData(CalendarSelectionShape.Circle)]
		[InlineData(CalendarSelectionShape.Rectangle)]
		public void Autofit_WithSelectionShape(CalendarSelectionShape expected)
		{
			var calendar = new SfCalendar();
			calendar.SelectionShape = expected;
			calendar.ShowTrailingAndLeadingDates = false;
			calendar.Mode = CalendarMode.RelativeDialog;
			Assert.False(calendar.ShowTrailingAndLeadingDates);
			Assert.Equal(expected, calendar.SelectionShape);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void Autofit_WithCanToggleDaySelection(bool expected)
		{
			var calendar = new SfCalendar();
			calendar.CanToggleDaySelection = expected;
			calendar.ShowTrailingAndLeadingDates = false;
			calendar.Mode = CalendarMode.RelativeDialog;
			Assert.False(calendar.ShowTrailingAndLeadingDates);
			Assert.Equal(expected, calendar.CanToggleDaySelection);
		}
		[Theory]
		[InlineData(CalendarRelativePosition.AlignTop)]
		[InlineData(CalendarRelativePosition.AlignBottom)]
		[InlineData(CalendarRelativePosition.AlignToLeftOf)]
		[InlineData(CalendarRelativePosition.AlignToRightOf)]
		[InlineData(CalendarRelativePosition.AlignTopLeft)]
		[InlineData(CalendarRelativePosition.AlignTopRight)]
		[InlineData(CalendarRelativePosition.AlignBottomLeft)]
		[InlineData(CalendarRelativePosition.AlignBottomRight)]
		public void Autofit_WithRelativePosition(CalendarRelativePosition position)
		{
			var calendar = new SfCalendar();
			calendar.View = CalendarView.Month;
			calendar.Mode = CalendarMode.RelativeDialog;
			calendar.RelativePosition = position;
			calendar.PopupHeight = 300;
			calendar.ShowTrailingAndLeadingDates = false;
			Assert.Equal(CalendarMode.RelativeDialog, calendar.Mode);
			Assert.Equal(position, calendar.RelativePosition);
			calendar.RelativePosition = CalendarRelativePosition.AlignTop;
			Assert.Equal(CalendarMode.RelativeDialog, calendar.Mode);
			Assert.Equal(CalendarRelativePosition.AlignTop, calendar.RelativePosition);
		}

		#endregion

		#region Autofit + Identifier Combination Properties

		[Theory]
		[InlineData(CalendarIdentifier.Gregorian, true, 6)]
		[InlineData(CalendarIdentifier.Gregorian, false, 6)]
		[InlineData(CalendarIdentifier.Hijri, true, 6)]
		[InlineData(CalendarIdentifier.Hijri, false, 6)]
		[InlineData(CalendarIdentifier.Persian, true, 6)]
		[InlineData(CalendarIdentifier.Persian, false, 6)]
		[InlineData(CalendarIdentifier.Korean, true, 5)]
		[InlineData(CalendarIdentifier.Korean, false, 5)]
		[InlineData(CalendarIdentifier.Taiwan, true, 4)]
		[InlineData(CalendarIdentifier.Taiwan, false, 4)]
		[InlineData(CalendarIdentifier.ThaiBuddhist, true, 6)]
		[InlineData(CalendarIdentifier.UmAlQura, false, 6)]
		public void Autofit_PropertyConsistency_WithIdentifier(CalendarIdentifier identifier, bool showTrailing, int weeks)
		{
			var calendar = new SfCalendar();
			calendar.View = CalendarView.Month;
			calendar.Identifier = identifier;
			calendar.ShowTrailingAndLeadingDates = showTrailing;
			calendar.MonthView.NumberOfVisibleWeeks = weeks;
			calendar.Mode = CalendarMode.RelativeDialog;
			Assert.Equal(identifier, calendar.Identifier);
			Assert.Equal(showTrailing, calendar.ShowTrailingAndLeadingDates);
			Assert.Equal(weeks, calendar.MonthView.NumberOfVisibleWeeks);
			Assert.Equal(CalendarView.Month, calendar.View);
		}

		[Theory]
		[InlineData(CalendarIdentifier.Hijri)]
		[InlineData(CalendarIdentifier.UmAlQura)]
		[InlineData(CalendarIdentifier.Gregorian)]
		[InlineData(CalendarIdentifier.Persian)]
		public void Trailing_Toggle_DoesNotChange_Identifier(CalendarIdentifier identifier)
		{
			var calendar = new SfCalendar();
			calendar.Identifier = identifier;
			calendar.View = CalendarView.Month;
			calendar.MonthView.NumberOfVisibleWeeks = 6;
			calendar.Mode = CalendarMode.RelativeDialog;
			calendar.ShowTrailingAndLeadingDates = false;
			Assert.Equal(identifier, calendar.Identifier);
			calendar.ShowTrailingAndLeadingDates = true;
			Assert.Equal(identifier, calendar.Identifier);
		}

		[Theory]
		[InlineData(CalendarMode.Default, CalendarIdentifier.Gregorian, 300)]
		[InlineData(CalendarMode.Dialog, CalendarIdentifier.Hijri, 280)]
		[InlineData(CalendarMode.RelativeDialog, CalendarIdentifier.Persian, 420)]
		public void PopupMode_WithIdentifier_PropertyConsistency(CalendarMode mode, CalendarIdentifier identifier, double popupHeight)
		{
			var calendar = new SfCalendar();
			calendar.View = CalendarView.Month;
			calendar.Mode = mode;
			calendar.Identifier = identifier;
			calendar.PopupHeight = popupHeight;
			calendar.ShowTrailingAndLeadingDates = false;
			calendar.MonthView.NumberOfVisibleWeeks = 6;
			Assert.Equal(mode, calendar.Mode);
			Assert.Equal(identifier, calendar.Identifier);
			Assert.Equal(popupHeight, calendar.PopupHeight);
			Assert.False(calendar.ShowTrailingAndLeadingDates);
			Assert.Equal(6, calendar.MonthView.NumberOfVisibleWeeks);
		}

		#endregion
	}
}
